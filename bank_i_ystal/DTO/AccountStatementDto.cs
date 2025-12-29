using bank_i_ystal.Models;

namespace bank_i_ystal.DTO;

public class AccountStatementDto
{
    public Guid AccountId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
}