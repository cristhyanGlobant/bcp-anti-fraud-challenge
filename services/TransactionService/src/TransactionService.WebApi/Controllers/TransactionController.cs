using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands;
using TransactionService.Application.Interfaces;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
    {
        var transactionId = await _transactionService.CreateTransactionAsync(command);
        return Ok(new { TransactionExternalId = transactionId, CreatedAt = DateTime.UtcNow });
    }
}
