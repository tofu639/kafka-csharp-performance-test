using KafkaPerformanceTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Start Kafka Consumer in the background
var cts = new CancellationTokenSource();
var consumer = new KafkaConsumer("localhost:9092", "test-group");
var consumerTask = Task.Run(() => consumer.Consume("test-topic", cts.Token));

app.Run();

// Clean up
cts.Cancel();
consumerTask.Wait();