using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Admin;
using e_commerce_backend.DTO.Order;
using Microsoft.Data.SqlClient;
using System.Data;

namespace e_commerce_backend.Data.Respository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ServiceResponse<Dashboard>> GetDashboardData(Guid userId, Guid roleId)
        {
            ServiceResponse<Dashboard> response = new ServiceResponse<Dashboard>();
            Dashboard dashboard = new Dashboard();
            List<RecentOrder> recentOrders = new List<RecentOrder>();
            List<MostOrderProduct> mostOrderedProducts = new List<MostOrderProduct>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("GetDashboardDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@roleId", roleId);
            
            await conn.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                if (reader.FieldCount > 2)
                {
                    while (await reader.ReadAsync())
                    {
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        string value = reader.GetString(reader.GetOrdinal("value"));

                        switch (name)
                        {
                            case "Total Users":
                                dashboard.TotalUsers = Convert.ToInt32(Convert.ToDecimal(value));
                                break;
                            case "Total Orders":
                                dashboard.TotalOrders = Convert.ToInt32(Convert.ToDecimal(value));
                                break;
                            case "Total Revenue":
                                dashboard.TotalRevenue = Convert.ToDecimal(value);
                                break;
                            case "Total Products":
                                dashboard.TotalProducts = Convert.ToInt32(Convert.ToDecimal(value));
                                break;
                        }

                    }

                    if (await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            recentOrders.Add(new RecentOrder
                            {
                                OrderId = reader.GetGuid(reader.GetOrdinal("OrderId")),
                                OrderNo = reader.GetString(reader.GetOrdinal("OrderNo")),
                                UserName = reader.GetString(reader.GetOrdinal("Customer")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                PlacedAt = reader.GetDateTime(reader.GetOrdinal("PlacedAt")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
                            });
                            
                        }
                    }

                    if(await reader.NextResultAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            MostOrderProduct mostOrderProduct = new MostOrderProduct
                            {
                                ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("title")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("totals"))
                            };
                            mostOrderedProducts.Add(mostOrderProduct);
                        }
                    }

                    dashboard.RecentOrders = recentOrders;
                    dashboard.MostOrderedProducts = mostOrderedProducts;
                    response.Status = true;
                    response.Message = "Dashboard data retrieved successfully";
                    response.Data = dashboard;

                    return response;
                }
                else
                {
                    response.Status = false;
                    response.Message = "No data found";
                    response.Data = null;

                    return response;
                }
            }
            else
            {
                response.Status = false;
                response.Message = "No data found";
                response.Data = null;

                return response;
            }
        }
    
        public async Task<StatusMessage> UpdateUserAccess(Guid userId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("BlockOrUnblockUser", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@userId", userId);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            StatusMessage statusMessage = new StatusMessage();
            if (await reader.ReadAsync())
            {
                statusMessage.Status = reader.GetBoolean(reader.GetOrdinal("Status"));
                statusMessage.Message = reader.GetString(reader.GetOrdinal("Message"));
            }
            return statusMessage;
        }

        public async Task<StatusMessage> UpdateOrderStatus(Guid orderItemId, Guid status)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("UpdateOrderStatus", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@orderItemId", orderItemId);
            cmd.Parameters.AddWithValue("@statusId", status);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            StatusMessage statusMessage = new StatusMessage();
            if (await reader.ReadAsync())
            {
                statusMessage.Status = reader.GetBoolean(reader.GetOrdinal("Status"));
                statusMessage.Message = reader.GetString(reader.GetOrdinal("Message"));
            }
            return statusMessage;
        }

        public async Task<List<OrderStatus>> GetAllOrderStatus()
        {
            List<OrderStatus> orderStatuses = new List<OrderStatus>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("GetAllOrderStatus", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    OrderStatus orderStatus = new OrderStatus
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Status = reader.GetString(reader.GetOrdinal("Status"))
                    };
                    orderStatuses.Add(orderStatus);
                }
            }
            return orderStatuses;
        }


    }
}
