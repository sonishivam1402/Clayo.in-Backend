﻿using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Order;
using e_commerce_backend.Models;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IEmailRepository _emailRepository;

        public UserController(IConfiguration config,IUserService userService, IEmailRepository emailRepository)
        {
            _config = config;
            _userService = userService;
            _emailRepository = emailRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hasAccess = GetRoleAccess();
            if (!hasAccess) return Unauthorized("You are not authorized to access this page.");

            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpPost("Id")]
        public async Task<IActionResult> GetById()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            var user = await _userService.GetUserById((Guid)userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest request)
        //{
        //    var user = await _userService.AuthUser(request.Email, request.Password);

        //    if (user.StatusMessage.Status)
        //    {
                
        //        // Create claims
        //        var claims = new[]
        //        {
        //            new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
        //            new Claim(JwtRegisteredClaimNames.Email, user.name),
        //            new Claim("UserId", user.id.ToString())
        //        };

        //            // Load JWT config from appsettings.json
        //            var jwtSettings = _config.GetSection("JwtSettings").Get<JwtSettings>();
        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //            // Create token
        //            var token = new JwtSecurityToken(
        //                issuer: jwtSettings.Issuer,
        //                audience: jwtSettings.Audience,
        //                claims: claims,
        //                expires: DateTime.Now.AddMinutes(jwtSettings.ExpiryInMinutes),
        //                signingCredentials: creds
        //            );

        //            // Add token to result and return
        //            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        //            user.Token = jwtToken;

        //            return Ok(user);
        //    }
        //    else
        //    {
        //        return Unauthorized(user.StatusMessage);
        //    }
        //}



        [HttpPost("AddOrUpdateUser")]
        public async Task<IActionResult> Register([FromBody]AddOrUpdateUsers data)
        {
            var result = await _userService.AddOrUpdateUsers(data);
            if (result.Status)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost("VerifyUser")]
        public async Task<IActionResult> VerifyUser([FromBody] VerifyAndUseOtp request)
        {
            var result = await _userService.VerifyUser(request);
            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
