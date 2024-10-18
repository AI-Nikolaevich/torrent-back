using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Torrent.Application.Interfaces.Chat;
namespace Torrent.Infrastructure.Chat
{
    public class ChatHub : Hub<IChatService>
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IMessageQueueService _messageQueueService;

        public ChatHub(ILogger<ChatHub> logger, IMessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
            _logger = logger;
        }

        public async Task JoinChat(string userName)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "main");
                await Clients.Group("main").ReceiveMessage(userName, "Присоединился к чату");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in JoinChat method");
                throw;
            }
        }
               
        public async Task SendMessage(string userName, string message)
        {
            _logger.LogInformation($"Received message from {userName}: {message}");
            await Clients.Group("main").ReceiveMessage(userName, message);
            await _messageQueueService.SendMessageToQueue(userName, message);
        }

       
    }
}
