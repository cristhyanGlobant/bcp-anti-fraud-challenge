namespace TransactionService.Application.Events;

public class TransactionCreatedEvent
{
    public Guid TransactionId { get; set; }
    public Guid SourceAccountId { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
}
