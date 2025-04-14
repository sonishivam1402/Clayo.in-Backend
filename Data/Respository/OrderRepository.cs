using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Order;
using Microsoft.Data.SqlClient;

namespace e_commerce_backend.Data.Respository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<StatusMessage> PlaceOrder(PlaceOrder order)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand("PlaceOrder", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@cartIds", order.CartIds);
            await connection.OpenAsync();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            StatusMessage statusMessage = new StatusMessage();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    statusMessage.Message = reader.GetString(reader.GetOrdinal("MESSAGE"));
                    statusMessage.Status = reader.GetBoolean(reader.GetOrdinal("Status"));
                }
            }
            return statusMessage;
        }

        public async Task<IEnumerable<object>> GetOrderDetails(Guid userId)
        {
            List<GetOrderDetails> orderDetails = new List<GetOrderDetails>();
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("GetOrderDetails", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@userId", userId);

            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                if (reader.FieldCount > 2)
                {
                    while (await reader.ReadAsync())
                    {
                        orderDetails.Add(new GetOrderDetails
                        {
                            OrderId = reader.GetGuid(reader.GetOrdinal("id")),
                            Product_title = reader.GetString(reader.GetOrdinal("title")),
                            Product_image = reader.GetString(reader.GetOrdinal("image")),
                            Product_price = reader.GetDecimal(reader.GetOrdinal("price")),
                            Product_quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                            Product_total_price = reader.GetDecimal(reader.GetOrdinal("total_price")),
                            Order_delivery_date = reader.GetDateTime(reader.GetOrdinal("delivered_at")),
                            Order_date = reader.GetDateTime(reader.GetOrdinal("placed_at")),
                            Order_status = reader.GetString(reader.GetOrdinal("Status"))
                        });
                    }
                    return orderDetails;
                }
                else
                {
                    StatusMessage status = new StatusMessage();
                    while (await reader.ReadAsync())
                    {
                        status.Message = reader.GetString(reader.GetOrdinal("MESSAGE"));
                        status.Status = reader.GetBoolean(reader.GetOrdinal("Status"));
                    }
                    return new List<object> { status };
                }
            }
            else
            {
                StatusMessage status = new StatusMessage
                {
                    Message = "No order found",
                    Status = false
                };
                return new List<object> { status };
            }
        }

    }
}
