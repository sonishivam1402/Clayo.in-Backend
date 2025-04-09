﻿using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.Models;
using e_commerce_backend.Services.Interfaces;

namespace e_commerce_backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<AuthResponse> AuthUser(string email, string password)
        {
            return  await _userRepository.AuthenticateUser(email,password);
        }

        public async Task<string> RegisterUser(RegisterUser request)
        {
            return await _userRepository.RegisterUser(request);
        }

    }
}
