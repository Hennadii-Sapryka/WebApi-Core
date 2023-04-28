using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

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

        [HttpGet]
        public async Task<IActionResult> GetElec()
        {
            if (dbContext.Electricians == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            //var skils = dbContext.Electricians.Include(c=>c.ScillsList).ToList();
            List<Electrician> list = await dbContext.Electricians.ToListAsync();
            var temp = list;

            return StatusCode(StatusCodes.Status200OK, list);
        }

        [HttpPost]
        public async Task<ActionResult<Electrician>> AddElectrician(Electrician electrician)
        {

                dbContext.Electricians.Add(electrician);
                await dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);

        }

    }


}
