using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace e_commerce_backend.Data.Respository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _config;

        public AuthRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<AuthResponse> ValidateUserAsync(string email, string password)
        {
            using SqlConnection conn = new(_config.GetConnectionString("DefaultConnection"));
            using SqlCommand cmd = new("GetUserByEmail", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password_hash", password);

            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                int status = reader.GetInt32(reader.GetOrdinal("Status"));
                string message = reader.GetString(reader.GetOrdinal("Message"));

                if (status == 0)
                {
                    return new AuthResponse
                    {
                        isSuccess = false,
                        Message = message
                    };
                }

                return new AuthResponse
                {
                    UserId = reader.IsDBNull(reader.GetOrdinal("id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("id")),
                    UserName = reader.IsDBNull(reader.GetOrdinal("full_name")) ? null : reader.GetString(reader.GetOrdinal("full_name")),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                    roleId = reader.IsDBNull(reader.GetOrdinal("role_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("role_id")),
                    profileImage = reader.IsDBNull(reader.GetOrdinal("profile_picture")) ? null : reader.GetString(reader.GetOrdinal("profile_picture")),
                    cartId = reader.IsDBNull(reader.GetOrdinal("CartId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("CartId")),
                    isSuccess = true,
                    Message = message
                };
            }

            return new AuthResponse
            {
                isSuccess = false,
                Message = "Invalid email or password"
            };
        }



        public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiresAt)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("INSERT INTO RefreshTokens (UserId, Token, ExpiresAt) VALUES (@UserId, @Token, @ExpiresAt)", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Token", refreshToken);
                    command.Parameters.AddWithValue("@ExpiresAt", expiresAt);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM RefreshTokens WHERE UserId=@UserId AND Token=@Token AND ExpiresAt > GETUTCDATE() AND IsRevoked=0", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Token", refreshToken);

                    var count = (int)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        public async Task RevokeAndSaveNewRefreshTokenAsync(Guid userId, string oldToken, string newToken, DateTime expiresAt)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    var revokeCmd = new SqlCommand("UPDATE RefreshTokens SET IsRevoked=1 WHERE UserId=@UserId AND Token=@Token", connection, transaction);
                    revokeCmd.Parameters.AddWithValue("@UserId", userId);
                    revokeCmd.Parameters.AddWithValue("@Token", oldToken);
                    await revokeCmd.ExecuteNonQueryAsync();

                    var insertCmd = new SqlCommand("INSERT INTO RefreshTokens (UserId, Token, ExpiresAt) VALUES (@UserId, @Token, @ExpiresAt)", connection, transaction);
                    insertCmd.Parameters.AddWithValue("@UserId", userId);
                    insertCmd.Parameters.AddWithValue("@Token", newToken);
                    insertCmd.Parameters.AddWithValue("@ExpiresAt", expiresAt);
                    await insertCmd.ExecuteNonQueryAsync();

                    transaction.Commit();
                }
            }
        }

        public async Task RevokeRefreshTokenAsync(Guid userId, string refreshToken)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("UPDATE RefreshTokens SET IsRevoked=1 WHERE UserId=@UserId AND Token=@Token", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Token", refreshToken);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
