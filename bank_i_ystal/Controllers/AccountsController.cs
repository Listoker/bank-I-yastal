using bank_i_ystal.DTO;
using bank_i_ystal.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bank_i_ystal.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _repository;

    public AccountsController(IAccountRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<Models.Account>), 200)]
    public IActionResult GetAllAccounts()
    {
        return Ok(_repository.GetAllAccounts());
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Models.Account), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetAccount(Guid id)
    {
        var account = _repository.GetAccountById(id);
        if (account == null) return NotFound();
        return Ok(account);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Models.Account), 201)]
    [ProducesResponseType(400)]
    public IActionResult CreateAccount([FromBody] CreateAccountDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if ((dto.Type == Models.AccountType.Deposit || dto.Type == Models.AccountType.Credit) &&
            !dto.InterestRate.HasValue)
        {
            return BadRequest("Процентная ставка обязательна для депозитных и кредитных счетов");
        }
        var account = _repository.CreateAccount(dto);
        return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
    }
    
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult UpdateAccount(Guid id, [FromBody] UpdateAccountDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var success = _repository.UpdateAccount(id, dto);
        if (!success) return NotFound();
        
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult DeleteAccount(Guid id)
    {
        var success = _repository.DeleteAccount(id);
        if (!success) return NotFound();
        
        return NoContent();
    }
    
    [HttpGet("owner/{ownerId:guid}")]
    [ProducesResponseType(typeof(List<Models.Account>), 200)]
    public IActionResult GetAccountsByOwner(Guid ownerId)
    {
        return Ok(_repository.GetAccountsByOwner(ownerId));
    }
    
    [HttpGet("{id:guid}/exists")]
    [ProducesResponseType(typeof(bool), 200)]
    public IActionResult AccountExists(Guid id)
    {
        return Ok(_repository.AccountExists(id));
    }
}