using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using WebApi.Data;
using WebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using WebApi.Repo;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Context _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Repository<User> _repository;
        public AccountController(Context context, IHttpContextAccessor httpContextAccessor, Repository<User> repository)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;

        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] LoginUser loginUser)
        {
            var userRepo = await _repository.Query().FirstOrDefaultAsync(u => u.Email == loginUser.Email);
            if (userRepo != null)
            {
                return StatusCode(StatusCodes.Status200OK);
            }
            var isUser = new LoginUser() { Email = loginUser.Email, Password = loginUser.Password, UserName = loginUser.UserName };
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        [Route("/logIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginUser loginUser)
        {
            if (loginUser.Email.IsNullOrEmpty()) return BadRequest("Field of Email is empty");
            var loggedInUser = await _dbContext.Users!.FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            if (loggedInUser is null) return BadRequest("No exist user");
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loggedInUser.Email!),
                new Claim("role", loggedInUser.Role!)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await _httpContextAccessor.HttpContext!.SignInAsync(claimsPrincipal);
            
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet]
        [Route("logOut")]
        public async Task<IActionResult> LogOut()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        //public User Authentication(LoginUser login)
        //{
        //    User user = null;
        //    if (dbContext.users!.Where(u => u.Email == login.Email).Any()  return user;
        //}
    }
}
