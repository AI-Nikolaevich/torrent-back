using Torrent.Core.Models;

namespace Torrent.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User> GetByEmail(string email);
        Task<bool> ExistsUserByEMail(string email);
        Task<User> GetByUserName(string? userName);
    }
}
