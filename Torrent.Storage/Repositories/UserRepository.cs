using Microsoft.EntityFrameworkCore;
using Torrent.Application.Interfaces.Repositories;
using Torrent.Core.Models;
using Torrent.Storage.Models;

namespace Torrent.Storage.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TorrentContext _context;
        public UserRepository(TorrentContext context)
        {
            _context = context;
        }
        public async Task Add(User user)
        {
           var AlreadyExistsEMail = await ExistsUserByEMail(user.Email);

            if (AlreadyExistsEMail)
            {
                throw new Exception("Email already exists");
            }

            var userEntity = new UserEntity
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Email = user.Email
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsUserByEMail(string email) => await _context.Users.AnyAsync(u=> u.Email == email);

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("Не нашлось такой почты");

            return User.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, userEntity.Email);
        }

        public async Task<User> GetByUserName(string? userName)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName) ?? throw new Exception("Не нашлось такого юзера");

            return User.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, userEntity.Email);
        }
    }
}
