using TransactionService.Application.Commands;

namespace TransactionService.Application.Interfaces;

public interface ITransactionService
{
    Task<Guid> CreateTransactionAsync(CreateTransactionCommand command);
}
