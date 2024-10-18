using System.ComponentModel.DataAnnotations;

namespace Torrent.Api.Contracts.Users
{
    public record RegisterUserRequest(
       [Required] string UserName,
       [Required] string Email,
       [Required] string Password);
}
