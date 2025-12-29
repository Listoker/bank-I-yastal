using bank_i_ystal.DTO;
using bank_i_ystal.Models;
using bank_i_ystal.Repositories.Interfaces;
namespace bank_i_ystal.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts = new();
    private readonly List<Transaction> _transactions = new();
    
    public AccountRepository()
    {
        var checkingAccountId = Guid.NewGuid();
        var depositAccountId = Guid.NewGuid();
        
        _accounts.Add(new Account
        {
            Id = checkingAccountId,
            OwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Type = AccountType.Checking,
            Currency = "RUB",
            Balance = 1000,
            InterestRate = null,
            OpenedDate = DateTime.UtcNow.AddDays(-30)
        });

        _accounts.Add(new Account
        {
            Id = depositAccountId,
            OwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Type = AccountType.Deposit,
            Currency = "RUB",
            Balance = 0,
            InterestRate = 3,
            OpenedDate = DateTime.UtcNow.AddDays(-30)
        });
    }

    public Account CreateAccount(CreateAccountDto dto)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            OwnerId = dto.OwnerId,
            Type = dto.Type,
            Currency = dto.Currency,
            Balance = dto.InitialBalance,
            InterestRate = dto.InterestRate,
            OpenedDate = DateTime.UtcNow,
            ClosedDate = null
        };
        _accounts.Add(account);
        return account;
    }
    
    public List<Account> GetAllAccounts() => _accounts;
    
    public Account? GetAccountById(Guid id) => _accounts.Find(a => a.Id == id);
    
    public List<Account> GetAccountsByOwner(Guid ownerId) =>
        _accounts.Where(a => a.OwnerId == ownerId).ToList();
    
    public bool UpdateAccount(Guid id, UpdateAccountDto dto)
    {
        var account = GetAccountById(id);
        if (account == null) return false;
        
        if (dto.InterestRate.HasValue)
            account.InterestRate = dto.InterestRate.Value;
        
        if (dto.ClosedDate.HasValue)
            account.ClosedDate = dto.ClosedDate.Value;
        
        return true;
    }

    public bool DeleteAccount(Guid id)
    {
        var account = GetAccountById(id);
        if (account == null) return false;
        if (account.Balance > 0) return false;
        
        return _accounts.Remove(account);
    }
    
    public Transaction AddTransaction(CreateTransactionDto dto)
    {
        var account = GetAccountById(dto.AccountId);
        if (account == null)
            throw new ArgumentException("Счет не найден");
        
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = dto.AccountId,
            CounterpartyAccountId = dto.CounterpartyAccountId,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Type = dto.Type,
            Description = dto.Description,
            TransactionDate = DateTime.UtcNow
        };
        
        if (dto.Type == TransactionType.Credit)
            account.Balance += dto.Amount;
        else if (dto.Type == TransactionType.Debit)
        {
            if (account.Balance < dto.Amount)
                throw new InvalidOperationException("Недостаточно средств на счете");
            account.Balance -= dto.Amount;
        }
        
        _transactions.Add(transaction);
        account.Transactions.Add(transaction);
        
        return transaction;
    }

    public List<Transaction> GetAccountTransactions(Guid accountId) =>
        _transactions.Where(t => t.AccountId == accountId).ToList();

    public bool Transfer(TransferDto dto, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (dto.FromAccountId == dto.ToAccountId)
        {
            errorMessage = "Невозможно перевести средства на тот же счет";
            return false;
        }

        var fromAccount = GetAccountById(dto.FromAccountId);
        var toAccount = GetAccountById(dto.ToAccountId);

        if (fromAccount == null || toAccount == null)
        {
            errorMessage = "Один из счетов не найден";
            return false;
        }

        if (fromAccount.Balance < dto.Amount)
        {
            errorMessage = "Недостаточно средств на счете отправителя";
            return false;
        }

        if (fromAccount.Currency != toAccount.Currency)
        {
            errorMessage = "Валюты счетов не совпадают";
            return false;
        }
        
        fromAccount.Balance -= dto.Amount;
        toAccount.Balance += dto.Amount;
        
        var debitTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = dto.FromAccountId,
            CounterpartyAccountId = dto.ToAccountId,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Type = TransactionType.Debit,
            Description = $"Перевод на счет {dto.ToAccountId}: {dto.Description}",
            TransactionDate = DateTime.UtcNow
        };

        var creditTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = dto.ToAccountId,
            CounterpartyAccountId = dto.FromAccountId,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Type = TransactionType.Credit,
            Description = $"Перевод со счета {dto.FromAccountId}: {dto.Description}",
            TransactionDate = DateTime.UtcNow
        };
        
        _transactions.Add(debitTransaction);
        _transactions.Add(creditTransaction);
        fromAccount.Transactions.Add(debitTransaction);
        toAccount.Transactions.Add(creditTransaction);

        return true;
    }
    
    public List<Transaction> GetAccountStatement(Guid accountId, DateTime fromDate, DateTime toDate)
    {
        if (fromDate > toDate)
            throw new ArgumentException("Дата начала не может быть позже даты окончания");
        
        return _transactions
            .Where(t => t.AccountId == accountId && t.TransactionDate >= fromDate && t.TransactionDate <= toDate)
            .OrderBy(t => t.TransactionDate)
            .ToList();
    }
    
    public bool AccountExists(Guid accountId) =>
        _accounts.Any(a => a.Id == accountId);
}