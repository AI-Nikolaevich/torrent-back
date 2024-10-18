namespace Torrent.Application.Interfaces.Auth
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Гененируем хеш-код пароля.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string Generate(string password);
       /// <summary>
       /// Соответствует ли введёный пароль сохраннёному кешу?
       /// </summary>
       /// <param name="password"></param>
       /// <param name="hashedPassword"></param>
       /// <returns></returns>
        bool Verify(string password, string hashedPassword);
    }
}
