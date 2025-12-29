using System.ComponentModel.DataAnnotations;

namespace bank_i_ystal.DTO;

public class TransferDto
{
    [Required]
    public Guid FromAccountId { get; set; }
    
    [Required]
    public Guid ToAccountId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "RUB";
    
    [StringLength(200)]
    public string Description { get; set; } = "Межсчетный перевод";
}