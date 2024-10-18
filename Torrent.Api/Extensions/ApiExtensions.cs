using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Torrent.Api.Endpoints;
using Torrent.Infrastructure;

namespace Torrent.Api.Extensions
{
    public static class ApiExtensions
    {
        public static void AddMappedEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapUsersEndpoints();
        }

        public static void AddApiAuthentication(this IServiceCollection services)
        {

            var jwtOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)                     //указали, что будем использовать схему для JWT токена    
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>                   //нужно подробно указать конфигурацию для JWT токена
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,           //Валидация издетеля
                        ValidateAudience = false,         //          получателя
                        ValidateLifetime = true,          //          времени
                        ValidateIssuerSigningKey = true,  //          ключ
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["torrent-cookies"];
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization();
        }
    }
}
