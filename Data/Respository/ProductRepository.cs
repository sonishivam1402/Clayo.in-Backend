using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace e_commerce_backend.Data.Respository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<GetProduct>> GetAllProductsAsync()
        {
            List<GetProduct> products = [];

            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("GetAllProducts", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(MapToProduct(reader));
            }

            return products;

        }

        private GetProduct MapToProduct(SqlDataReader reader)
        {
            return new GetProduct
            {
                ProductId = reader.GetGuid(reader.GetOrdinal("Id")),
                title = reader.GetString(reader.GetOrdinal("title")),
                description = reader.GetString(reader.GetOrdinal("description")),
                category = reader.GetString(reader.GetOrdinal("category")),
                price = reader.GetDecimal(reader.GetOrdinal("price")),
                stock = reader.GetOrdinal("stock"),
                image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image")),
                rating_rate = reader.GetDecimal(reader.GetOrdinal("rating_rate")),
                rating_count = reader.GetOrdinal("rating_count"),
            };
        }
    }
}
