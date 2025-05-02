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
                    password_hash = reader.GetString(reader.GetOrdinal("password_hash")),
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
                        email = reader.GetString(reader.GetOrdinal("email")),
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

        public async Task<ServiceResponse<SendOtpEmailRequest>> AddOrUpdateUsers(AddOrUpdateUsers request)
        {
            ServiceResponse<SendOtpEmailRequest> serviceResponse = new ServiceResponse<SendOtpEmailRequest>();
            SendOtpEmailRequest otpVerification = new SendOtpEmailRequest();
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("AddOrUpdateUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id", request.Id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@name", request.name);
            cmd.Parameters.AddWithValue("@email", request.email);
            cmd.Parameters.AddWithValue("@phone_number", request.mobile_no);
            cmd.Parameters.AddWithValue("@password", request.password_hash);
            cmd.Parameters.AddWithValue("@address", request.address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@image", request.profile_picture ?? (object)DBNull.Value);

            await conn.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.Read())
            {
                serviceResponse.Message = reader.IsDBNull(reader.GetOrdinal("Message")) ? null : reader.GetString(reader.GetOrdinal("Message"));
                serviceResponse.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? false : reader.GetBoolean(reader.GetOrdinal("Status"));
                otpVerification.UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("UserId"));
                otpVerification.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"));
                otpVerification.Otp = reader.IsDBNull(reader.GetOrdinal("OTP")) ? null : reader.GetString(reader.GetOrdinal("OTP"));
                serviceResponse.Data = otpVerification;
            }
            else
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "No data returned from stored procedure.";
                serviceResponse.Data = null;
            }

            return serviceResponse;
        }

        public async Task<StatusMessage> VerifyUser(VerifyAndUseOtp request)
        {
            StatusMessage statusMessage = new StatusMessage();
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("VerifyAndUseOTP", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@UserId", request.userId);
            cmd.Parameters.AddWithValue("@InputOTP", request.otp);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.Read())
            {
                statusMessage.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? false : reader.GetBoolean(reader.GetOrdinal("Status"));
                statusMessage.Message = reader.IsDBNull(reader.GetOrdinal("Message")) ? null : reader.GetString(reader.GetOrdinal("Message"));
            }
            else
            {
                statusMessage.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? false : reader.GetBoolean(reader.GetOrdinal("Status"));
                statusMessage.Message = reader.IsDBNull(reader.GetOrdinal("Message")) ? null : reader.GetString(reader.GetOrdinal("Message"));
            }
            return statusMessage;
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
                address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address")),
                hasAccess = reader.GetBoolean(reader.GetOrdinal("hasAccess")),
                isVerified = reader.GetBoolean(reader.GetOrdinal("is_verified")),
                profile_picture = reader.IsDBNull(reader.GetOrdinal("profile_picture")) ? null : reader.GetString(reader.GetOrdinal("profile_picture")),
                last_updated = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
            };
        }
    }
}
