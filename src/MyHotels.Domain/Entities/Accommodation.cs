using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyHotels.Domain.Entities;

public sealed class Accommodation : IValidatableObject
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int CategoryId { get; set; }
    public int AvailabilityModeId { get; set; }

    [Required(ErrorMessage = "O nome do alojamento é obrigatorio")]
    [StringLength(100, ErrorMessage = "O nome do alojamento é no maximo de 100 caracteres")]
    public required string Name { get; set; }

    [StringLength(200, ErrorMessage = "A descrição do alojamento é no maximo de 200 caracteres")]
    public string? Description { get; set; }

    [Range(1,20, ErrorMessage = "A capacidade do alojamento deve ser entre 1 e 20")]
    public int Capacity { get; set; }

    [Range(0,9999, ErrorMessage = "O numero de unidade não pode ser negativo")]
    public int Stock { get; set; }

    [Column(TypeName="decimal(10,2)")]
    [Range(0.01,99999.99, ErrorMessage = "O preço base não pode ser negativo")]
    public decimal BasePrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 9999999.99, ErrorMessage = "O preço final não pode ser negativo")]
    public decimal FinalPrice { get; set; }

    [StringLength(200)]
    public string? ImagePath { get; set; }
    public byte[]? Image { get; set; }
    public bool IsListed { get; set; } = true;
    public bool IsAvailableForBooking { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public Hotel? Hotel { get; set; }
    public AccommodationCategory? Category { get; set; }
    public AvailabilityMode? AvailabilityMode { get; set; }
    public ICollection<BookingItem> BookingItems { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsAvailableForBooking && Stock <= 0)
        {
            yield return new ValidationResult("Este alojamento não tem unidades disponiveis para reservar", [nameof(IsAvailableForBooking)]);
        }
    }
}
