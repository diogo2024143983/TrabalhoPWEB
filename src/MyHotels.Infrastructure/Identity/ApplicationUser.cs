using Microsoft.AspNetCore.Identity;
using MyHotels.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyHotels.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{

    [StringLength(100)]
    public string? DisplayName { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }


    [StringLength(50)]
    public string? LastName { get; set; }

    

    [ValidarNIF(ErrorMessage = "O NIF inserido é inválido.")]
    public long? Nif { get; set; }

    [RegularExpression(@"^9\d{8}$", ErrorMessage = "O número de telemóvel deve ter 9 dígitos e começar por '9'.")]
    public string? PhoneNumberCustom { get; set; }


    [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O código postal deve estar no formato '1234-567'.")]
    public String? PostalCode { get; set; }

    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public CustomerStatus CustomerStatus { get; set; } = CustomerStatus.Pending;
}


public class ValidarNIF : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
                return ValidationResult.Success;
        }

        if (value is not long nif || nif.ToString().Length != 9)
        {
            return new ValidationResult("O NIF deve ter exatamente 9 dígitos.");
        }

        if (!IsValid(nif))
        {
            return new ValidationResult("O NIF não é válido.");
        }

        return ValidationResult.Success;
    }

    private bool IsValid(long nif)
    {
        string nifString = nif.ToString();
        int primeiroDigito = int.Parse(nifString[0].ToString());
        if (primeiroDigito < 1 || primeiroDigito > 9)
        {
            return false;
        }
        int total = 0;
        for (int i = 0; i < 8; i++)
        {
            total += int.Parse(nifString[i].ToString()) * (9 - i);
        }
        int digitoDeControlo = 11 - (total % 11);
        if (digitoDeControlo >= 10)
        {
            digitoDeControlo = 0;
        }
        return digitoDeControlo == int.Parse(nifString[8].ToString());
    }
}
