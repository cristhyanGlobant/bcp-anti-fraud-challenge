namespace TransactionService.Infrastructure.Kafka;

using Confluent.Kafka;
using System.Text.Json;
using TransactionService.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Application.Events;

public class KafkaConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IServiceProvider _serviceProvider;

    public KafkaConsumer(ConsumerConfig config, IServiceProvider serviceProvider)
    {
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("transaction-validated");

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = _consumer.Consume(stoppingToken);
            var message = consumeResult.Message.Value;

            var validatedEvent = JsonSerializer.Deserialize<TransactionValidatedEvent>(message);

            if (validatedEvent != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();

                await repository.UpdateStatusAsync(validatedEvent.TransactionId, validatedEvent.Status);
            }
        }
    }
}
