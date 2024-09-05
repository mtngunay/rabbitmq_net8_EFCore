using Api.Data.Entity;
using RabbitMQ.Client;
using System.Text;

namespace Api.Services
{
    public class OrderPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public OrderPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Fanout);
        }

        public void PublishOrderCreated(Order order)
        {
            var message = $"Order Created: {order.Id}";
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "order_exchange",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
