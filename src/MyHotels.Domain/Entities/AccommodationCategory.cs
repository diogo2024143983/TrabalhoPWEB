using System.ComponentModel.DataAnnotations;

namespace MyHotels.Domain.Entities;

public sealed class AccommodationCategory
{
    public int Id { get; set; }


    [Required(ErrorMessage = "O nome da categoria é obrigatprio")]
    [StringLength(100, ErrorMessage = "O nome da categoria é no maximo de 100 caracteres")]
    public required string Name { get; set; }


    [StringLength(200, ErrorMessage = "A descrição da categoria é no maximo de 200 caracteres")]
    public string? Description { get; set; }


    [StringLength(200)]
    public string? ImagePath { get; set; }

    public byte[]? Image { get; set; }  
    public bool IsActive { get; set; } = true;
    public ICollection<Accommodation> Accommodations { get; set; } = [];
}
