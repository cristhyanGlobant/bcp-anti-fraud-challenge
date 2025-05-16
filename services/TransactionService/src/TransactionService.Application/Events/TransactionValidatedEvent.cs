using TransactionService.Domain.Enums;

namespace TransactionService.Application.Events;

public class TransactionValidatedEvent
{
    public Guid TransactionId { get; set; }
    public TransactionStatus Status { get; set; }
}
