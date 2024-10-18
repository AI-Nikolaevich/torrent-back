
using System.Security.Cryptography.X509Certificates;

namespace Torrent.Application.Interfaces.Chat
{
    public interface IChatService
    {
        public Task ReceiveMessage(string userName, string message);
    }
}
