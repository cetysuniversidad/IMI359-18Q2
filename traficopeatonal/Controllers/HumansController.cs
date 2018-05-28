using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CETYS.Posgrado.imi359.Models;

namespace CETYS.Posgrado.imi359.Controllers
{
    [ApiVersion("2")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HumansController : Controller
    {
        private readonly TraficoPeatonalContext _context;

        public HumansController(TraficoPeatonalContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetHumano/{id}")]
        public async Task<IActionResult> AsyncGetHuman([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var r = await _context.Human
                .Include(x => x.Event)
                .Include(x => x.Event.EventValueRaw)
                .Include(x => x.Event.EventValueComputed)
                .SingleOrDefaultAsync(m => m.HumanId == id);

            if (r == null)
            {
                return NotFound(new { message = $"Human Id {id} was not found", value = id });
            }

            return Ok(r);
        }

        [HttpGet]
        [Route("GetHumanoByFechaRango")]
        public async Task<IActionResult> AsyncGetHumanByDateRange(DateTime from, DateTime to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sql = $@"CALL getHumanByDateRange ('{from.Year:0000}.{from.Month}.{from.Day}', '{to.Year:0000}.{to.Month}.{to.Day}')";
            var r = await _context.HumanList.FromSql(sql).ToListAsync();

            if (r == null)
            {
                return NotFound(new { message = $"No humans found between '{from}' and '{to}'"
                    , value1 = from
                    , value2 = to
                });
            }

            return Ok(r);
        }

    }
}