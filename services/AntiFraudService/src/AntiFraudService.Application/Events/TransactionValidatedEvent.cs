using AntiFraudService.Domain.Enums;

namespace AntiFraudService.Application.Events;

public class TransactionValidatedEvent
{
    public Guid TransactionId { get; set; }
    public TransactionStatus Status { get; set; }
}
