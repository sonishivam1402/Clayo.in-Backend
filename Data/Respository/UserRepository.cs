﻿using e_commerce_backend.Data.Interfaces;
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
                        Success = false,
                        Message = reader["MESSAGE"].ToString()
                    };
                }
                else{
                    return new AuthResponse
                    {
                        Success = true,
                        User = MapToUser(reader)
                    };
                }
            }
            return new AuthResponse 
            {   Success = false,
                Message = "Unexpected error occurred." 
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
                profile_picture = reader.IsDBNull(reader.GetOrdinal("profile_picture")) ? null : reader.GetString(reader.GetOrdinal("profile_picture")),
                last_updated = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
            };
        }
    }
}
