﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using WebApi.Data;
using WebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Context dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(Context context, IHttpContextAccessor httpContextAccessor)
        {
            dbContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] LoginUser loginUser)
        //{
        //    var isUser = new LoginUser() { Email = loginUser.Email, Password = loginUser.Password, UserName = loginUser.UserName, Role = "" };
        //}

        [HttpPost]
        [Route("/LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginUser loginUser)
        {
            if (loginUser.Email.IsNullOrEmpty()) return BadRequest("Field of Email is empty"); ;
            var loggedInUser = await dbContext.Electricians!.FirstOrDefaultAsync(u => u.Email == loginUser.Email);
            if (loggedInUser is null) return BadRequest("No exist user");
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loggedInUser.Email!),
                new Claim("role", loggedInUser.Role!)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await _httpContextAccessor.HttpContext!.SignInAsync(claimsPrincipal);
            
            return StatusCode(StatusCodes.Status200OK); ;
        }
        [HttpGet]
        [Route("/LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        //public Electrician Authentication(LoginUser login)
        //{
        //    Electrician user = null;
        //    if (dbContext.Electricians!.Where(u => u.Email == login.Email && u.).Any()  return user;
        //}
    }
}
