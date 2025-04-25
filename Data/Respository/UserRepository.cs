using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace e_commerce_backend.Data.Respository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            List<User> users = [];
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("GetAllUsers", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(MapToUser(reader));
            }

            return users;
        }

        public async Task<User> GetUserById(Guid Id)
        {
            User user = new User();
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("GetUserByUserId", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("id", Id);

            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                user = new User
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("full_name")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Mobile_no = reader.GetString(reader.GetOrdinal("phone_number")),
                    Role = reader.GetString(reader.GetOrdinal("Role_name")),
                    address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address")),
                    profile_picture = reader.IsDBNull(reader.GetOrdinal("profile_picture")) ? null : reader.GetString(reader.GetOrdinal("profile_picture")),
                    last_updated = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
                };
            }

            return user;
        }

        public async Task<AuthResponse> AuthenticateUser(string email, string password)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("GetUserByEmail", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password_hash", password);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows && await reader.ReadAsync())
            {
                if(reader.FieldCount == 1 && reader.GetName(0) == "MESSAGE")
                {
                    return new AuthResponse
                    {
                        StatusMessage = new StatusMessage
                        {
                            Status = false,
                            Message = reader["MESSAGE"].ToString()
                        }
                    };
                }
                else{
                    return new AuthResponse
                    {
                        id = reader.GetGuid(reader.GetOrdinal("Id")),
                        name = reader.GetString(reader.GetOrdinal("full_name")),
                        roleId = reader.GetGuid(reader.GetOrdinal("Role_id")),
                        profileImage = reader.IsDBNull(reader.GetOrdinal("profile_picture")) ? null : reader.GetString(reader.GetOrdinal("profile_picture")),
                        cartId = reader.IsDBNull(reader.GetOrdinal("cartId")) ? null : reader.GetGuid(reader.GetOrdinal("cartId")),
                        StatusMessage = new StatusMessage
                        {
                            Status = true,
                            Message = "Login successful"
                        }
                    };
                }
            }
            return new AuthResponse 
            {   
                StatusMessage = new StatusMessage
                {
                    Status = false,
                    Message = "Invalid email or password"
                }
            };

        }

        public async Task<string> RegisterUser(RegisterUser request)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("UserRegistration", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@name", request.Name);
            cmd.Parameters.AddWithValue("@email", request.Email);
            cmd.Parameters.AddWithValue("@phone_number", request.PhoneNumber);
            cmd.Parameters.AddWithValue("@password", request.Password);

            await conn.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            string response = string.Empty;

            if (await reader.ReadAsync())
            {
                response = reader["Message"].ToString();
            }

            return response;
        }



        private User MapToUser(SqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("full_name")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Mobile_no = reader.GetString(reader.GetOrdinal("phone_number")),
                Role = reader.GetString(reader.GetOrdinal("Role_name")),
                address = reader.GetString(reader.GetOrdinal("address")),
                hasAccess = reader.GetBoolean(reader.GetOrdinal("hasAccess")),
                isVerified = reader.GetBoolean(reader.GetOrdinal("is_verified")),
                profile_picture = reader.IsDBNull(reader.GetOrdinal("profile_picture")) ? null : reader.GetString(reader.GetOrdinal("profile_picture")),
                last_updated = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
            };
        }
    }
}
