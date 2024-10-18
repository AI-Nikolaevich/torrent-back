

namespace Torrent.Application.Interfaces.Chat
{
    public interface IMessageQueueService
    {
        Task SendMessageToQueue(string user, string message);
    }
}
