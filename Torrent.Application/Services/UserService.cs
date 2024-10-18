using Torrent.Application.Interfaces.Auth;
using Torrent.Application.Interfaces.Repositories;
using Torrent.Core.Models;

namespace Torrent.Application.Services
{
    public class UserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _usersRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        public async Task Register(string userName, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var newUser = User.Create(Guid.NewGuid(), userName, hashedPassword, email);

            await _usersRepository.Add(newUser);
        }

        public async Task<ResponseModel> Login(string? userName, string? email, string password)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Either userName or email must be provided.");
            }

            User user = null;

            if (!string.IsNullOrEmpty(userName))
            {
                user = await _usersRepository.GetByUserName(userName);
            }
            if (!string.IsNullOrEmpty(email))
            {
                user = await _usersRepository.GetByEmail(email);
            }

            var result = _passwordHasher.Verify(password, user.PasswordHash);   // Проверили подходит ли введёный пароль к сохранённому кешу?

            if (!result)
            {
                throw new Exception("Failed to Login");
            }
            var token = _jwtProvider.GenerateToken(user);
            // Если да, то сгенерировали токен, который будет действовать определённое время, указанное в JwtOptions.
            return new ResponseModel
            {
                Token = token,
                UserName = user.UserName
            };
        }

        //TODO  Как это оформить правильно?
        public class ResponseModel
        {
            public required string Token { get; set; }
            public required string UserName { get; set; }
        }
    }
}
