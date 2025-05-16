namespace AntiFraudService.Domain.Models;

public class TransactionEvaluationInput
{
    public Guid SourceAccountId { get; set; }
    public decimal Value { get; set; }
}