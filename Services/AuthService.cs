using CMS_.Introduction.Models;
using CMS_.Introduction.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace CMS_.Introduction.Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Authenticate(string username, string password)
        {

            User user = null;
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                connection.Open();
                string sql = "SELECT * FROM dbo.Users WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
