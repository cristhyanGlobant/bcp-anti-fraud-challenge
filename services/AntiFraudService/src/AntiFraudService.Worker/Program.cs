using AntiFraudService.Application.Interfaces;
using AntiFraudService.Domain.Interfaces;
using AntiFraudService.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // Asegura que cargue appsettings.json y appsettings.{env}.json
        config.AddJsonFile("appsettings.json", optional: true)
              .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        var kafkaSection = configuration.GetSection("Kafka");
        var kafkaConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaSection["BootstrapServers"],
            GroupId = kafkaSection["GroupId"],
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(kafkaSection["AutoOffsetReset"])
        };

        services.AddSingleton(kafkaConfig);

        services.AddSingleton<IAntiFraudService, AntiFraudService.Application.Services.AntiFraudService>();
        services.AddSingleton<IAntiFraudEvaluator, AntiFraudEvaluator>();

        services.AddSingleton<KafkaTransactionConsumer>();
        services.AddHostedService<AntiFraudWorker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build()
    .Run();
