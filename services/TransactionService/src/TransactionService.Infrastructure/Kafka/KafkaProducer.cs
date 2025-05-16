namespace TransactionService.Infrastructure.Kafka;

using Confluent.Kafka;
using System.Text.Json;
using TransactionService.Domain.Interfaces;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(ProducerConfig config)
    {
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync<T>(string topic, T message)
    {
        var json = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
    }
}
