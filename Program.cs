using CMS_.Introduction.Services;
using CMS_.Introduction.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.ApplicationInsights;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyMethod());
});
builder.Services.AddDirectoryBrowser();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            Console.WriteLine($"Token: {token}");
            return Task.CompletedTask;
        },

        OnTokenValidated = context =>
        {
            var userId = context.Principal.Identity.Name;
            Console.WriteLine($"Authenticated User ID: {userId}");
            return Task.CompletedTask;
        }
    };
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
builder.Services.AddApplicationInsightsTelemetry();


builder.Services.AddAuthorization();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

