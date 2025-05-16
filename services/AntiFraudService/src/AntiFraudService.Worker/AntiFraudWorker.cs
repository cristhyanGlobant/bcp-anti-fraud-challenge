using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class AntiFraudWorker : BackgroundService
{
    private readonly KafkaTransactionConsumer _consumer;
    private readonly ILogger<AntiFraudWorker> _logger;

    public AntiFraudWorker(KafkaTransactionConsumer consumer, ILogger<AntiFraudWorker> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AntiFraud Worker started.");
        return Task.Run(() => _consumer.StartConsuming("transaction-created", stoppingToken), stoppingToken);
    }
}
