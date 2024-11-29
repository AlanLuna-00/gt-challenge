namespace api.features.products.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateProductDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The field Name is required.")]
    [StringLength(100, ErrorMessage = "The field Name must not exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The field Description is required.")]
    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "The field Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "The field Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The field Stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "The field Stock must be greater than or equal to 0.")]
    public int Stock { get; set; }
}
