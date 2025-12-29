using System.ComponentModel.DataAnnotations;
using bank_i_ystal.Models;

namespace bank_i_ystal.DTO;

public class CreateAccountDto
{
    [Required]
    public Guid OwnerId { get; set; }
    
    [Required]
    public AccountType Type { get; set; }
    
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "RUB";
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal InitialBalance { get; set; }
    
    [Range(0, 100)]
    public decimal? InterestRate { get; set; }
}