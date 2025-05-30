﻿using e_commerce_backend.DTO;
using e_commerce_backend.Models;

namespace e_commerce_backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<GetProduct>> GetAllProducts();

        Task<GetProduct> GetProductById(Guid productId);
        Task<StatusMessage> AddOrUpdateProduct(PostProduct product);
    }
}
