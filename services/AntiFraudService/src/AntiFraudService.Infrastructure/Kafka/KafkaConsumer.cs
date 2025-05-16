// AntiFraudService.Infrastructure/Kafka/KafkaTransactionConsumer.cs
using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using AntiFraudService.Application.Interfaces;
using AntiFraudService.Application.Events;

public class KafkaTransactionConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IAntiFraudService _antiFraudService;
    private readonly ILogger<KafkaTransactionConsumer> _logger;

    public KafkaTransactionConsumer(
        ConsumerConfig config,
        IAntiFraudService antiFraudService,
        ILogger<KafkaTransactionConsumer> logger)
    {
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _antiFraudService = antiFraudService;
        _logger = logger;
    }

    public void StartConsuming(string topic, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var cr = _consumer.Consume(cancellationToken);
                var message = JsonSerializer.Deserialize<TransactionCreatedEvent>(cr.Message.Value);

                _logger.LogInformation($"Received message: {cr.Message.Value}");

                if (message != null)
                {
                    var result = _antiFraudService.ValidateTransaction(message).Result;

                    _logger.LogInformation($"Transaction {message.TransactionId} evaluated as {result}");
                }
            }
            catch (ConsumeException e)
            {
                _logger.LogError($"Consume error: {e.Error.Reason}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing transaction");
            }
        }

        _consumer.Close();
    }
}