using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;

namespace TransactionService.Domain.Interfaces;

public interface ITransactionRepository
{
    Task SaveAsync(Transaction transaction);
    Task UpdateStatusAsync(Guid transactionId, TransactionStatus status);
    Task<List<Transaction>> GetTransactionsByDateAsync(Guid sourceAccountId, DateTime date);
}
