using System.ComponentModel.DataAnnotations;

namespace Admin.Domain.Contracts.Products;

public class DTOProduct
{
    public int Id { get; set; }
    [Required(ErrorMessage = "messageRequired")]    
    [MaxLength(50, ErrorMessage = "messageMaxLength")]    
    public string Name { get; set; }
    [Required(ErrorMessage = "messageRequired")]
    [MaxLength(1024, ErrorMessage = "messageMaxLength")]
    public string Description { get; set; }
    public decimal? Height { get; set; }
    public decimal? Width { get; set; }
    public decimal? Depth { get; set; }
    public decimal Price { get; set; }
    [RegularExpression("^(I|U)?$", ErrorMessage="messageUseType")]
    public string UseType { get; set; }
    public bool Enabled { get; set; }
    public DateTime ValidFrom { get; set; }
    public int CategoryId { get; set; }
}