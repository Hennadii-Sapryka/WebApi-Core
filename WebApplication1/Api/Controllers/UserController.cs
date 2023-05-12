using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStack.DataAnnotations;
using System.Linq;
using WebApi.Data;
using WebApi.Models;
using System.Security.Claims;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using WebApi.Repo;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly Context dbContext;
        private readonly Repository<User> _repository;

        public UserController(ILogger<UserController> logger, Context context, Repository<User> repository)
        {
            _logger = logger;
            dbContext = context;
            _repository = repository;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetElectricians()
        //{
        //    var userId = HttpContext!.User;
        //    if (dbContext.Electricians == null)
        //    {
        //        return StatusCode(StatusCodes.Status204NoContent);
        //    }
        //    //var skills = dbContext.Electricians.Include(c=>c.SkillsList).ToList();
        //    List<Electrician> list = await dbContext.Electricians.ToListAsync();

        //    return StatusCode(StatusCodes.Status200OK, list);
        //}
        [Authorize(Policy = "OnlyForOwner")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetElectriciansById(int id)
        {
            var testUserRepo = await _repository.Query().FirstOrDefaultAsync(u => u.Id == id);
            var userId = await dbContext.Users!.Where(u => u.Id == id).FirstOrDefaultAsync();

            return userId != null
                ? StatusCode(StatusCodes.Status200OK, await dbContext.Users!.FirstOrDefaultAsync(p => p.Id == id))
                : StatusCode(StatusCodes.Status204NoContent);
        }

        //[Authorize(Policy = "OnlyForAdmin")]
        [HttpGet("/All")]
        public async Task<IActionResult> GetAll()
        {
            var users = await dbContext.Users!.FirstOrDefaultAsync();

            return users != null
                ? StatusCode(StatusCodes.Status200OK, await dbContext.Users!.FirstOrDefaultAsync())
                : StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        [Route("/AddUser")]
        public async Task<ActionResult> AddUser(string name, string email, string phone, bool isOwnShop)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            User user = new();
            user.Name = name;
            user.Email = email;
            user.PhoneNumber = phone;
            isOwnShop = !isOwnShop
                ? user.IsTechnicians = true
                : user.IsOwnBusiness = true;

            BusinessChecker(user);

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        public async Task<ActionResult<User>> Update([FromBody] User user)
        {
            var userToUpdate = await dbContext.Users!.FindAsync(user.Id);

            if (userToUpdate == null)
                return StatusCode(StatusCodes.Status204NoContent);

            userToUpdate.Name = user.Name;
            userToUpdate.Email = user.Email;

            dbContext.Update(userToUpdate);
            await dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpDelete]
        [Route("/DeleteById")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var currentUser = await dbContext.Users!.FirstOrDefaultAsync(t => t.Id == id);
            if (currentUser == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            dbContext.Users!.Remove(currentUser);
            dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
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

        //public async Task<ActionResult<Electrician>> AcceptPolicyAsync()
        //{
        //    var userContext = await dbContext.Electricians.FirstOrDefaultAsync(u => u.Id == userId);
        //    var userId = HttpContext!.User;

        //    var user = await dbContext.Electricians.FirstOrDefaultAsync(u => u.Id == userId);

        //    user.IsPolicyAccepted = true;
        //    await dbContext.Electricians.Update(user);
        //    await dbContext.SaveChangesAsync();

        //    return user;
        //}
    }


}
