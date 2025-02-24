using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaPerformanceTest.Services
{
    public class KafkaConsumer
    {
        private readonly ConsumerConfig _config;

        public KafkaConsumer(string bootstrapServers, string groupId)
        {
            _config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public void Consume(string topic, CancellationToken cancellationToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                consumer.Subscribe(topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Clean up
                    consumer.Close();
                }
            }
        }
    }
}