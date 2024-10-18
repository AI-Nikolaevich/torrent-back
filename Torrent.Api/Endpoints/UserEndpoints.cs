using Microsoft.AspNetCore.Cors;
using Torrent.Api.Contracts.Users;
using Torrent.Application.Services;

namespace Torrent.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("/", () => "Hello Auth");
            return app;
        }

        [EnableCors("AllowSpecificOrigin")]
        private static async Task<IResult> Register(
            RegisterUserRequest request,
            UserService userService)
        {
            await userService.Register(request.UserName, request.Email, request.Password);

            return Results.Ok(new { message = "Registration successful" });
        }

        [EnableCors("AllowSpecificOrigin")]
        private static async Task<IResult> Login(
            LoginUserRequest request,
            UserService userService,
            HttpContext context)
        {
            var token = await userService.Login(request.UserName, request.Email, request.Password);

            context.Response.Cookies.Append("torrent-cookies", token.Token);

            return Results.Ok(token);
        }


    }
}
