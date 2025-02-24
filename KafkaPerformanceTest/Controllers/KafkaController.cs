using KafkaPerformanceTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KafkaPerformanceTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KafkaController : ControllerBase
    {
        private readonly KafkaProducer _producer;

        public KafkaController()
        {
            _producer = new KafkaProducer("localhost:9092");
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            await _producer.ProduceAsync("test-topic", message);
            return Ok("Message sent to Kafka");
        }
    }
}