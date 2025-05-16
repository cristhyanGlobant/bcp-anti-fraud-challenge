using AntiFraudService.Domain.Enums;
using AntiFraudService.Domain.Interfaces;
using AntiFraudService.Domain.Models;
using System.Collections.Concurrent;

namespace AntiFraudService.Domain.Services;

public class AntiFraudEvaluator : IAntiFraudEvaluator
{
    private static readonly ConcurrentDictionary<Guid, decimal> DailyTotals = new();

    public Task<TransactionStatus> EvaluateAsync(TransactionEvaluationInput input)
    {
        if (input.Value > 2000)
            return Task.FromResult(TransactionStatus.Rejected);

        var todayKey = input.SourceAccountId;
        DailyTotals.AddOrUpdate(todayKey, input.Value, (_, currentTotal) => currentTotal + input.Value);

        var total = DailyTotals[todayKey];
        return Task.FromResult(total > 2000 ? TransactionStatus.Rejected : TransactionStatus.Approved);
    }
}