using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyHotels.Domain.Identity;
using MyHotels.Infrastructure.Identity;

namespace MyHotels.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration) : ControllerBase
{
    public sealed record LoginRequest(string Email, string Password);

    public sealed record RegisterClienteRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string? Nif,
        string? Address);

    public sealed record AuthResponse(string Token, DateTime ExpiresAtUtc, string Email, IEnumerable<string> Roles);

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized(new { message = "Credenciais inválidas." });
        }

        if (await userManager.IsInRoleAsync(user, AppRoles.Cliente)
            && user.CustomerStatus != CustomerStatus.Active)
        {
            return Unauthorized(new { message = "Conta de cliente ainda pendente de ativação." });
        }

        return Ok(await CreateTokenAsync(user));
    }

    [HttpPost("register-cliente")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> RegisterCliente([FromBody] RegisterClienteRequest request)
    {
        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
        {
            return Conflict(new { message = "Já existe um utilizador com este email." });
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DisplayName = $"{request.FirstName} {request.LastName}".Trim(),
            Nif = request.Nif,
            Address = request.Address,
            IsActive = true,
            CustomerStatus = CustomerStatus.Pending
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await userManager.AddToRoleAsync(user, AppRoles.Cliente);
        return Ok(new { message = "Registo efetuado. Conta pendente de ativação pelo staff." });
    }

    private async Task<AuthResponse> CreateTokenAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Email ?? user.Id)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT signing key was not configured.");
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddHours(8);
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            expires,
            user.Email ?? string.Empty,
            roles);
    }
}
