﻿using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.Services.Interfaces;

namespace e_commerce_backend.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<GetProduct>> GetAllProducts()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<GetProduct> GetProductById(Guid productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }

        public async Task<StatusMessage> AddOrUpdateProduct(PostProduct product)
        {
            return await _productRepository.AddOrUpdateProduct(product);
        }

    }
}
