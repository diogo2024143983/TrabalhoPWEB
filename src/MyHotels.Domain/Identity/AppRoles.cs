namespace MyHotels.Domain.Identity;

public static class AppRoles
{
    public const string Administrador = "Administrador";
    public const string Funcionario = "Funcionario";
    public const string Cliente = "Cliente";
    public const string Fornecedor = "Fornecedor";

    public static readonly string[] All =
    [
        Administrador,
        Funcionario,
        Cliente,
        Fornecedor
    ];
}
