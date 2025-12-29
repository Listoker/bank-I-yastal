using bank_i_ystal.DTO;
using bank_i_ystal.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bank_i_ystal.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly IAccountRepository _repository;

    public TransactionsController(IAccountRepository repository)
    {
        _repository = repository;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Models.Transaction), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            var transaction = _repository.AddTransaction(dto);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Models.Transaction), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetTransaction(Guid id)
    {
        var accounts = _repository.GetAllAccounts();
        var transaction = accounts
            .SelectMany(a => a.Transactions)
            .FirstOrDefault(t => t.Id == id);
        
        if (transaction == null) return NotFound();
        
        return Ok(transaction);
    }
    
    [HttpGet("account/{accountId:guid}")]
    [ProducesResponseType(typeof(List<Models.Transaction>), 200)]
    public IActionResult GetAccountTransactions(Guid accountId)
    {
        return Ok(_repository.GetAccountTransactions(accountId));
    }
    
    [HttpPost("transfer")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult Transfer([FromBody] TransferDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var success = _repository.Transfer(dto, out var errorMessage);
        if (!success) return BadRequest(errorMessage);
        
        return NoContent();
    }
    
    [HttpGet("account/{accountId:guid}/statement")]
    [ProducesResponseType(typeof(AccountStatementDto), 200)]
    [ProducesResponseType(400)]
    public IActionResult GetAccountStatement(
        Guid accountId,
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
    {
        try
        {
            if (fromDate > toDate)
                return BadRequest("Дата начала не может быть позже даты окончания");
            
            if (!_repository.AccountExists(accountId))
                return NotFound("Счет не найден");
            
            var transactions = _repository.GetAccountStatement(accountId, fromDate, toDate);
            
            var statement = new AccountStatementDto
            {
                AccountId = accountId,
                FromDate = fromDate,
                ToDate = toDate,
                Transactions = transactions
            };
            
            return Ok(statement);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}