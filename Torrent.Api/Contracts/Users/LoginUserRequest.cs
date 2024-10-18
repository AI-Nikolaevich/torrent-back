using System.ComponentModel.DataAnnotations;

namespace Torrent.Api.Contracts.Users
{
    public record LoginUserRequest(
       string? UserName,
       string? Email,
       [Required] string Password);
}
