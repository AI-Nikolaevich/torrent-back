using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Torrent.Api.Extensions;
using Torrent.Application.Interfaces.Auth;
using Torrent.Application.Interfaces.Chat;
using Torrent.Application.Interfaces.Repositories;
using Torrent.Application.Services;
using Torrent.Infrastructure;
using Torrent.Infrastructure.Chat;
using Torrent.Storage;
using Torrent.Storage.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("https://www.freetor.ru", "https://freetor.ru", "http://www.freetor.ru", "http://freetor.ru", "http://localhost:5111")
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

var connectionString = builder.Configuration.GetConnectionString("postgres");
builder.Services.AddDbContext<TorrentContext>(options
    => options.UseNpgsql(connectionString), ServiceLifetime.Singleton);


builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IMessageQueueService,MessageQueueService>();
builder.Services.AddSignalR();

builder.Services.AddApiAuthentication();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");

app.MapHub<ChatHub>("/chat");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCookiePolicy(new CookiePolicyOptions
                       {
                           MinimumSameSitePolicy = SameSiteMode.Strict,
                           HttpOnly = HttpOnlyPolicy.Always,
                           Secure = CookieSecurePolicy.Always,
                       });

//app.Services.GetRequiredService<TorrentContext>().Database.Migrate();

app.UseAuthentication();
app.UseAuthorization();

app.AddMappedEndpoints();

app.Run();
