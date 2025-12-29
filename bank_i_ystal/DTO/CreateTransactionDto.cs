using System.ComponentModel.DataAnnotations;
using bank_i_ystal.Models;

namespace bank_i_ystal.DTO;

public class CreateTransactionDto
{
    [Required]
    public Guid AccountId { get; set; }
    
    public Guid? CounterpartyAccountId { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "RUB";
    
    [Required]
    public TransactionType Type { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;
}