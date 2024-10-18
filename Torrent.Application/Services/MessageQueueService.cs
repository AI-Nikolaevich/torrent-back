using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Torrent.Application.Interfaces.Chat;

namespace Torrent.Application.Services
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageQueueService()
        {
            var factory = new ConnectionFactory() { HostName = "93.183.70.206" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "chat_messages", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public Task SendMessageToQueue(string user, string message)
        {
            var messageObject = new { UserName = user, Message = message };
            var jsonString = JsonSerializer.Serialize(messageObject);
            var body = Encoding.UTF8.GetBytes(jsonString);

            _channel.BasicPublish(exchange: "", routingKey: "chat_messages", basicProperties: null, body: body);
            return Task.CompletedTask;
        }

    }
}
