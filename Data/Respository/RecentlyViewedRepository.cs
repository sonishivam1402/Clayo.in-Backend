using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace e_commerce_backend.Data.Respository
{
    public class RecentlyViewedRepository : IRecentlyViewedRepository
    {
        private readonly string _connectionString;

        public RecentlyViewedRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<StatusMessage> SetRecentlyViewedItemAsync(Guid userId, Guid productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SetRecentlyViewItems", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@productId", productId);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new StatusMessage
                        {
                            Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                            Message = reader.GetString(reader.GetOrdinal("Message"))
                        };
                    }
                    else
                    {
                        return new StatusMessage
                        {
                            Status = false,
                            Message = "No data returned from stored procedure."
                        };
                    }
                }
            }
        }

        public async Task<List<RecentlyViewedItem>> GetRecentlyViewedItemsAsync(Guid userId)
        {
            var result = new List<RecentlyViewedItem>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("GetRecentlyViewedItems", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new RecentlyViewedItem
                        {
                            productId = reader.GetGuid(reader.GetOrdinal("productId")),
                            ProductName = reader.GetString(reader.GetOrdinal("title")),
                            ViewedAt = reader.GetDateTime(reader.GetOrdinal("ViewedAt")),
                            Price = reader.GetDecimal(reader.GetOrdinal("price")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("image"))
                        };

                        result.Add(item);
                    }
                }
            }

            return result;
        }
    }
}
