using Torrent.Core.Models;

namespace Torrent.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}
