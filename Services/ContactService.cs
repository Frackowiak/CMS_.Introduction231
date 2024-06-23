using CMS_.Introduction.Models;
using CMS_.Introduction.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace CMS_.Introduction.Services
{
    public class ContactService : IContactService
    {
        private readonly IConfiguration _configuration;
        public ContactService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<ContactModel> Get()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                String sql = "SELECT * FROM dbo.Contacts";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new ContactModel(
                                reader.GetGuid(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetString(5));
                        }
                    }
                }
            }
        }

        public ContactModel Get(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                string sql = "SELECT * FROM dbo.Contacts WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ContactModel(
                                reader.GetGuid(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetString(5));
                        }
                    }
                }
            }

            return null;
        }

        public void Add(ContactModel contact)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                string sql = "INSERT INTO dbo.Contacts (FirstName, LastName, PhoneNumber, Email, Notes) VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @Notes)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@Notes", contact.Notes);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(ContactModel contact)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                string sql = "UPDATE dbo.Contacts SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Email = @Email, Notes = @Notes WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@Id", contact.Id);
                    command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@Notes", contact.Notes);

                    command.ExecuteNonQuery();

                }
            }
        }

        public void Delete(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                string sql = "DELETE FROM dbo.Contacts WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
