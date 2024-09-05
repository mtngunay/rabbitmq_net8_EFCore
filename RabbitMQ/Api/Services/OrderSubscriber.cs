using Api.Data;
using Api.Data.Entity;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Services
{
    public class OrderSubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public OrderSubscriber(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: "order_log_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            _channel.QueueBind(queue: "order_log_queue",
                               exchange: "order_exchange",
                               routingKey: "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var orderLog = new OrderLog
                    {
                        OrderId = ExtractOrderIdFromMessage(message),
                        Message = message,
                        CreatedAt = DateTime.UtcNow
                    };

                    context.OrderLogs.Add(orderLog);
                    context.SaveChanges();
                }
            };

            _channel.BasicConsume(queue: "order_log_queue",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        private int ExtractOrderIdFromMessage(string message)
        {
            var parts = message.Split(':');
            return int.Parse(parts[1].Trim());
        }
    }
}
