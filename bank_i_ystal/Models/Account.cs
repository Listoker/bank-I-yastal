using System.ComponentModel.DataAnnotations;

namespace bank_i_ystal.Models;

public class Account
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public Guid OwnerId { get; set; }
    
    [Required]
    public AccountType Type { get; set; }
    
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "RUB";
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }
    
    [Range(0, 100)]
    public decimal? InterestRate { get; set; }
    
    [Required]
    public DateTime OpenedDate { get; set; }
    
    public DateTime? ClosedDate { get; set; }
    
    public List<Transaction> Transactions { get; set; } = new();
}