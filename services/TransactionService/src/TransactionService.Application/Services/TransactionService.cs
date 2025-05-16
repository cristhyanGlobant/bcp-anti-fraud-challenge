using TransactionService.Application.Commands;
using TransactionService.Application.Events;
using TransactionService.Application.Interfaces;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly IKafkaProducer _kafkaProducer;

    public TransactionService(ITransactionRepository repository, IKafkaProducer kafkaProducer)
    {
        _repository = repository;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<Guid> CreateTransactionAsync(CreateTransactionCommand command)
    {
        var transaction = new Transaction
        {
            TransactionExternalId = Guid.NewGuid(),
            SourceAccountId = command.SourceAccountId,
            TargetAccountId = command.TargetAccountId,
            TransferTypeId = command.TransferTypeId,
            Value = command.Value,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.SaveAsync(transaction);

        var transactionCreatedEvent = new TransactionCreatedEvent
        {
            TransactionId = transaction.TransactionExternalId,
            Value = transaction.Value,
            CreatedAt = transaction.CreatedAt,
            SourceAccountId = transaction.SourceAccountId
        };

        await _kafkaProducer.ProduceAsync("transaction-created", transactionCreatedEvent);

        return transaction.TransactionExternalId;
    }
}
