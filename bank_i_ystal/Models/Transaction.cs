using System.ComponentModel.DataAnnotations;

namespace bank_i_ystal.Models;

public class Transaction
{
    [Required]
    public Guid Id { get; set; }
    
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
    
    [Required]
    public DateTime TransactionDate { get; set; }
}