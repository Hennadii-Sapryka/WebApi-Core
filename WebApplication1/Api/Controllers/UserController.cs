using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStack.DataAnnotations;
using System.Linq;
using WebApi.Data;
using WebApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebApi.Repo;
using AutoMapper;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly Repository<User> _repository;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, Repository<User> repository, IMapper mapper, Context dbContext)
        {
            _logger = logger;
            _mapper = mapper;
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
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _repository.Query().FirstOrDefaultAsync(u => u.Id == id);

            return user != null
                ? StatusCode(StatusCodes.Status200OK, user)
                : StatusCode(StatusCodes.Status204NoContent);
        }

        //[Authorize(Policy = "OnlyForAdmin")]
        [HttpGet("/All")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repository.Query().FirstOrDefaultAsync();

            return users != null
                ? StatusCode(StatusCodes.Status200OK, users)
                : StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("/Update")]
        public async Task<ActionResult<User>> Update([FromBody] User user)
        {
            var userToUpdate = await _repository.Query().FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userToUpdate == null)
                return StatusCode(StatusCodes.Status204NoContent);

            User newUser = new()
            {
                Email = "test@gmai.gmail"
            };

            //_repository.Detach(userToUpdate);

            userToUpdate = _mapper.Map(user, newUser);

            await _repository.UpdateAsync(userToUpdate);
            await _repository.SaveChangesAsync();

            return StatusCode(StatusCodes.Status202Accepted, userToUpdate);
        }

        [HttpDelete]
        [Route("/DeleteById")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var currentUser = await _repository.Query().FirstOrDefaultAsync(t => t.Id == id);

            if (currentUser == null)
                return StatusCode(StatusCodes.Status204NoContent);

            _repository.Delete(currentUser);
            await _repository.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
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
