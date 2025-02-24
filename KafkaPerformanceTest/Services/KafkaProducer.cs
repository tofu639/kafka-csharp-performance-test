using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace KafkaPerformanceTest.Services
{
    public class KafkaProducer
    {
        private readonly ProducerConfig _config;

        public KafkaProducer(string bootstrapServers)
        {
            _config = new ProducerConfig { BootstrapServers = bootstrapServers };
        }

        public async Task ProduceAsync(string topic, string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                Console.WriteLine($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
            }
        }
    }
}