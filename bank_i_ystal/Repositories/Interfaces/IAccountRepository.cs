using bank_i_ystal.DTO;
using bank_i_ystal.Models;

namespace bank_i_ystal.Repositories.Interfaces;

public interface IAccountRepository
{
    Account CreateAccount(CreateAccountDto dto);
    List<Account> GetAllAccounts();
    Account? GetAccountById(Guid id);
    List<Account> GetAccountsByOwner(Guid ownerId);
    bool UpdateAccount(Guid id, UpdateAccountDto dto);
    bool DeleteAccount(Guid id);
    Transaction AddTransaction(CreateTransactionDto dto);
    List<Transaction> GetAccountTransactions(Guid accountId);
    bool Transfer(TransferDto dto, out string errorMessage);
    List<Transaction> GetAccountStatement(Guid accountId, DateTime fromDate, DateTime toDate);
    bool AccountExists(Guid accountId);
}