using System;
using System.Threading.Tasks;
using CETYS.Posgrado.imi359.Servicios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using BdContext = CETYS.Posgrado.imi359.Servicios.Models.BdContext;

namespace CETYS.Posgrado.imi359.Servicios.Controllers
{
    [ApiVersion("2")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PerturbacionesController : Controller
    {
        private readonly BdContext _context;

        public PerturbacionesController(BdContext ctx)
        {
            _context = ctx;
        }

        [HttpGet]
        [Route("ObtenerPerturbacion/{id}")]
        public async Task<IActionResult> AsyncObtenerPerturbacion([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var r = await _context.Perturbacion
                .Include(raw => raw.PerturbacionValor)
                .Include(raw => raw.Humano)
                .SingleOrDefaultAsync(t => t.PerturbacionId == id);
            if (r == null)
            {
                return NotFound(new { message = $"Perturbacion Id {id} no fue encontrado", value = id});
            }

            return Ok(r);
        }
        
        [HttpPost]
        [Route("PostearPerturbacion")]
        public async Task<IActionResult> AsyncPostearPerturbacion([FromBody] dynamic arg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Hacer unas validaciones basicas a los datos entrantes de arg y crear
            // una instancia de Perturbacion() donde se agrgaran cada uno de los valores
            // en una lista de Perturbacion(), cada valor tiene una referencia a Perturbacion() padre
            var newPerturbacion = new Perturbacion();
            if (arg == null)
            {
                return BadRequest(new { message = "No existe ningun valor en Body del Post HTTP" });
            }

            try
            {
                newPerturbacion.Duracion = arg.TimeElapsed;
                newPerturbacion.Etiqueta = DateTime.Now;
            }
            catch (RuntimeBinderException)
            {
                return BadRequest(new { message = "Propiedad TimeElapsed no fue encontrada en el JSON enviado" });
            }

            try
            {
                foreach (var i in arg.Values)
                {
                    newPerturbacion.PerturbacionValor.Add(new PerturbacionValor { Valor = i, Perturbacion = newPerturbacion});
                }
            }
            catch (RuntimeBinderException)
            {
                return BadRequest(new { message = "Propiedad Values no fueron encontrados en el JSON enviado" });
            }

            // Usar la estrategia de resistencia del EF Core usando multiples DbContexts
            // dentro de una transaccion explicitada
            // Ref: https://docs.microsoft.com/ef/core/miscellaneous/connection-resiliency
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Perturbacion.Add(newPerturbacion);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
            });


            return Ok(AsyncObtenerPerturbacion(newPerturbacion.PerturbacionId).Result);
        }
    }
}
