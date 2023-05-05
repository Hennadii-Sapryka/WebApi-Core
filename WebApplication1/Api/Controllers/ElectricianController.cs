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

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ElectricianController : ControllerBase
    {
        private readonly ILogger<ElectricianController> _logger;
        private readonly Context dbContext;

        public ElectricianController(ILogger<ElectricianController> logger, Context context)
        {
            _logger = logger;
            dbContext = context;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetElectricians()
        //{
        //    var userId = HttpContext!.User;
        //    if (dbContext.Electricians == null)
        //    {
        //        return StatusCode(StatusCodes.Status204NoContent);
        //    }
        //    //var skils = dbContext.Electricians.Include(c=>c.ScillsList).ToList();
        //    List<Electrician> list = await dbContext.Electricians.ToListAsync();

        //    return StatusCode(StatusCodes.Status200OK, list);
        //}
        [Authorize(Policy = "OnlyForOwner")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetElectriciansById(int id)
        {
            var userId = await dbContext.Electricians?.Where(u => u.Id == id).FirstOrDefaultAsync();

            return  userId != null 
                ? StatusCode(StatusCodes.Status200OK, await dbContext.Electricians!.FirstOrDefaultAsync(p => p.Id == id)) 
                : StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        [Route("/AddElectrician")]
        public async Task<ActionResult<Electrician>> AddElectrician(Electrician electrician)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            dbContext.Electricians!.Add(electrician);
            await dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        public async Task<ActionResult<Electrician>> Update( [FromBody]Electrician electrician)
        {
            var userToUpdate = await dbContext.Electricians!.FindAsync(electrician.Id);

            if (userToUpdate == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            userToUpdate.Name = electrician.Name;
            userToUpdate.Email = electrician.Email;

            dbContext.Update(userToUpdate); 
            await dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted);
        }

        [HttpDelete]
        [Route("/DeleteById")]
        public async Task<ActionResult<Electrician>> DeleteElectrician(int id)
        {
            var electrician = await dbContext.Electricians?.FirstOrDefaultAsync(t => t.Id == id);
            if (electrician == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            dbContext.Electricians.Remove(electrician);
            dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        private bool isElectrician(int id)
        {
            if (dbContext.Electricians.Any(p => p.Id == id))
                return true;
            return false;
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
