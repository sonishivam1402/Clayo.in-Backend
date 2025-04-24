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
            command.Parameters.AddWithValue("@userId", order.userId);
            command.Parameters.AddWithValue("@cartId", order.cartId);
            command.Parameters.AddWithValue("@cartItemIds", order.CartItemIds);
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

        public async Task<IEnumerable<object>> GetOrderDetails(Guid? userId)
        {
            List<GetOrderDetails>orderDetails = new List<GetOrderDetails>();
            List<OrderItems> orderItems = new List<OrderItems>();
            List<ShippingOrder> shippingOrders = new List<ShippingOrder>();

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
                            OrderId = reader.GetGuid(reader.GetOrdinal("OrderId")),
                            customer = reader.GetString(reader.GetOrdinal("full_name")),
                            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                            PlacedAt = reader.GetDateTime(reader.GetOrdinal("PlacedAt")),
                            Status = reader.GetString(reader.GetOrdinal("Status"))
                        });
                    }

                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orderItems.Add(new OrderItems
                            {
                                OrderItemId = reader.GetGuid(reader.GetOrdinal("OrderItemId")),
                                OrderId = reader.GetGuid(reader.GetOrdinal("OrderId")),
                                ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("ProductPrice")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                image = reader.GetString(reader.GetOrdinal("image"))
                            });

                        }
                    }

                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            shippingOrders.Add(new ShippingOrder
                            {
                                ShippingOrderItemId = reader.GetGuid(reader.GetOrdinal("Id")),
                                OrderId = reader.GetGuid(reader.GetOrdinal("OrderId")),
                                OrderItemId = reader.GetGuid(reader.GetOrdinal("OrderItemId")),
                                Carrier = reader.GetString(reader.GetOrdinal("Carrier")),
                                TrackingNumber = reader.GetString(reader.GetOrdinal("Tracking_Number")),
                                ShippingAt = reader.GetDateTime(reader.GetOrdinal("Shipped_At")),
                                DeliveredAt = reader.GetDateTime(reader.GetOrdinal("Delivered_At")),
                                EstimatedDeliveryDate = reader.GetDateTime(reader.GetOrdinal("Estimated_Delivery")),
                                Status = reader.GetString(reader.GetOrdinal("Shipping_Status"))
                            });
                        }
                    }

                    foreach (var orderDetail in orderDetails)
                    {
                        orderDetail.OrderItems = orderItems.Where(o => o.OrderId == orderDetail.OrderId).ToList();
                        orderDetail.ShippingOrders = shippingOrders.Where(o => o.OrderId == orderDetail.OrderId).ToList();
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
                    return new List<StatusMessage> { status };
                }
            }
            else
            {
                StatusMessage status = new StatusMessage
                {
                    Message = "No order found",
                    Status = false
                };
                return new List<StatusMessage> { status };
            }
        }


        public async Task<StatusMessage> CancelOrder(Guid orderId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("CancelOrderByOrderId", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@orderId", orderId);
            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();
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
    }
}
