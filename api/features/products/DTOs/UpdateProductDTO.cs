namespace api.features.products.DTOs;

using System.ComponentModel.DataAnnotations;

public class UpdateProductDTO
{
    [StringLength(100, ErrorMessage = "The field Name must not exceed 100 characters.")]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "The field Price must be greater than 0.")]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "The field Stock must be greater than or equal to 0.")]
    public int? Stock { get; set; }
}
