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
using BCrypt.Net;
using AutoMapper;
using WebApi.Mapping;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Context _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Repository<User> _repository;
        private readonly IMapper _mapper;
        public AccountController(Context context, IHttpContextAccessor httpContextAccessor, Repository<User> repository, IMapper mapper)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] LoginUser loginUser, bool isOwnShop)
        {
            var userRepo = await _repository.Query().FirstOrDefaultAsync(u => u.Email == loginUser.Email);
            if (userRepo != null && loginUser.Email == userRepo.Email) return await LogIn(loginUser);

            var newUser = new LoginUser()
            {
                Email = loginUser.Email!.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginUser.PasswordHash),
                Name = loginUser.Name
            };
            var user = _mapper.Map<LoginUser, User>(newUser);

            isOwnShop = !isOwnShop
                ? user.IsTechnicians = true
                : user.IsOwnBusiness = true;

            BusinessChecker(user);
            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, user);
        }

        [HttpPost]
        [Route("/logIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginUser loginUser)
        {
            if (loginUser.Email.IsNullOrEmpty()) return BadRequest("Field of Email is empty");
            var loggedInUser = await _dbContext.Users!.FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            if (loggedInUser is null) return BadRequest("No exist user");
            if (!BCrypt.Net.BCrypt.Verify(loginUser.PasswordHash, loginUser.PasswordHash)) return BadRequest("Password is incorrect");

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

        private User BusinessChecker(User user)
        {
            if (user.IsOwnBusiness == true)
            {
                user.Role = "owner";
                return user;
            }
            user.Role = "technician";
            return user;
        }

        //public User Authentication(LoginUser login)
        //{
        //    User user = null;
        //    if (dbContext.users!.Where(u => u.Email == login.Email).Any()  return user;
        //}
    }
}
