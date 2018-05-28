using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CETYS.Posgrado.imi359.Servicios.Models;

namespace CETYS.Posgrado.imi359.Servicios.Controllers
{
    [ApiVersion("2")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HumanosController : Controller
    {
        private readonly BdContext _context;

        public HumanosController(BdContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ObtenerHumano/{id}")]
        public async Task<IActionResult> AsyncObtenerHumano([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var r = await _context.Humano
                .Include(x => x.Perturbacion)
                .SingleOrDefaultAsync(m => m.HumanoId == id);

            if (r == null)
            {
                return NotFound(new { message = $"Humano Id {id} no fue encontrado", value = id });
            }

            return Ok(r);
        }

        [HttpGet]
        [Route("ObtenerHumanoPorRangoFecha")]
        public async Task<IActionResult> AsyncObtenerHumanoPorRangoFecha(DateTime desde, DateTime hasta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sql = $@"CALL obtenerHumanoPorRangoFecha ('{desde.Year:0000}.{desde.Month}.{desde.Day}', '{hasta.Year:0000}.{hasta.Month}.{hasta.Day}')";
            var r = await _context.HumanoLista.FromSql(sql).ToListAsync();

            if (r == null)
            {
                return NotFound(new { message = $"No hay humanos encontrados desde '{desde}' hasta '{hasta}'"
                    , value1 = desde
                    , value2 = hasta
                });
            }

            return Ok(r);
        }

    }
}