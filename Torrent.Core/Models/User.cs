namespace Torrent.Core.Models
{
    public class User
    {
        /// TODO: Сделать проверку всех вводимых аргументов методов и обязательно реализовать паттерн проверка email
        private User(Guid id, string userName, string passwordHash, string email)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
        }
        private User(string userName, string passwordHash, string email)
        {
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
        }

        public Guid Id { get; set; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public string Email { get; private set; }

        public static User Create(Guid id, string userName, string passwordHash, string email)
        {
            return new User(id, userName, passwordHash, email);
        }
        public static User GetUserModelForDb(string userName, string passwordHash, string email)
        {
            return new User(userName, passwordHash, email);
        }

    }
}
