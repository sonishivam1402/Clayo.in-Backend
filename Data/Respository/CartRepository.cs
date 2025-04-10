using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Cart;
using Microsoft.Data.SqlClient;
using System.Data;

namespace e_commerce_backend.Data.Respository
{
    public class CartRepository : ICartRepository
    {
        private readonly string connectionString;

        public CartRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<GetCartItems>> GetAllCartItemsAsync(Guid userId)
        {
            List<GetCartItems> cartItems = new List<GetCartItems>();
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("GetCartProductByUserId", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@userId", userId);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                cartItems.Add(MapToCartItem(reader));
            }
            return cartItems;
        }

        public async Task<StatusMessage> AddToCartAsync(AddToCart cartItem)
        {
            StatusMessage statusMessage = new StatusMessage();
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("AddProductsToCart", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@userId", cartItem.UserId);
            cmd.Parameters.AddWithValue("@productId", cartItem.ProductId);
            cmd.Parameters.AddWithValue("@quantity", cartItem.Quantity);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                statusMessage.Message = reader.GetString(reader.GetOrdinal("MESSAGE"));
                statusMessage.Status = reader.GetBoolean(reader.GetOrdinal("Status"));
            }
            
            return statusMessage;
        }


        public async Task<StatusMessage> DeleteCartItemAsync(Guid cartId)
        {
            StatusMessage statusMessage = new StatusMessage();
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("DeleteCartItem", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@cartId", cartId);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                statusMessage.Message = reader.GetString(reader.GetOrdinal("MESSAGE"));
                statusMessage.Status = reader.GetBoolean(reader.GetOrdinal("Status")); 
            }
            return statusMessage;
        }

        private GetCartItems MapToCartItem(SqlDataReader reader)
        {
            return new GetCartItems
            {
                cartId = reader.GetGuid(reader.GetOrdinal("Id")),
                userId = reader.GetGuid(reader.GetOrdinal("UserId")),
                productId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                title = reader.GetString(reader.GetOrdinal("Title")),
                description = reader.GetString(reader.GetOrdinal("Description")),
                price = reader.GetDecimal(reader.GetOrdinal("Price")),
                category = reader.GetString(reader.GetOrdinal("Category")),
                image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                rating_rate = reader.GetDecimal(reader.GetOrdinal("Rating_Rate")),
                rating_count = reader.GetInt32(reader.GetOrdinal("Rating_Count"))
            };
        }
    }
}
