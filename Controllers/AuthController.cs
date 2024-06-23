using CMS_.Introduction.Models;
using CMS_.Introduction.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CMS_.Introduction.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginModel loginModel)
    {
        var token = _authService.Authenticate(loginModel.Username, loginModel.Password);

        if (token == null)
            return Unauthorized();

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserLoginModel registerModel)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);
            string sql = "INSERT INTO dbo.Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Username", registerModel.Username);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                command.ExecuteNonQuery();
            }
        }

        return Ok();
    }
}

