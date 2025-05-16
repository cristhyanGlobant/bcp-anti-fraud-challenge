using AntiFraudService.Application.Events;
using AntiFraudService.Application.Interfaces;
using AntiFraudService.Domain.Enums;
using AntiFraudService.Domain.Interfaces;
using AntiFraudService.Domain.Models;

namespace AntiFraudService.Application.Services;

public class AntiFraudService : IAntiFraudService
{
    private readonly IAntiFraudEvaluator _evaluator;

    public AntiFraudService(IAntiFraudEvaluator evaluator)
    {
        _evaluator = evaluator;
    }

    public async Task<TransactionStatus> ValidateTransaction(TransactionCreatedEvent transactionCreatedEvent)
    {
        var input = new TransactionEvaluationInput
        {
            SourceAccountId = transactionCreatedEvent.SourceAccountId,
            Value = transactionCreatedEvent.Value
        };

        var result = await _evaluator.EvaluateAsync(input);
        return result;
    }
}