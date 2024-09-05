using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace Api.Services
{
    public class OrderProcessingService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public OrderProcessingService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "orderQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                // Mesajı işleyin (örneğin, veritabanı güncellemesi)
                Console.WriteLine("Received message: {0}", message);
            };

            _channel.BasicConsume(queue: "orderQueue",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
