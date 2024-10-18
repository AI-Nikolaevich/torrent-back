
namespace Torrent.Infrastructure
{
    public class JwtOptions
    {
        /// <summary>
        /// Секретный ключ по коорому будет создаваться токен.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;
        /// <summary>
        /// Сколько часов будет действовать токен.
        /// </summary>
        public int ExpitesHourse { get; set; }
    }
}
