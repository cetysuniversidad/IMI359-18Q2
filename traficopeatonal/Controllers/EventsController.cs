using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using CETYS.Posgrado.imi359.Models;

namespace CETYS.Posgrado.imi359.Controllers
{
    [ApiVersion("2")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventsController : Controller
    {
        private readonly TraficoPeatonalContext _context;

        public EventsController(TraficoPeatonalContext ctx)
        {
            _context = ctx;
        }

        [HttpGet]
        [Route("GetEvento/{id}")]
        public async Task<IActionResult> AsyncGetEvent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var r = await _context.Event
                .Include(raw => raw.EventValueRaw)
                .Include(raw => raw.EventValueComputed)
                .Include(raw => raw.Human)
                .SingleOrDefaultAsync(t => t.EventId == id);
            if (r == null)
            {
                return NotFound(new { message = $"Event Id {id} was not found", value = id});
            }

            return Ok(r);
        }
        
        [HttpPost]
        [Route("PostEvento")]
        public async Task<IActionResult> AsyncPostEvent([FromBody] dynamic arg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Hacer unas validaciones basicas a los datos entrantes de arg y crear
            // una instancia de Event() donde se agrgaran cada uno de los valores
            // en una lista de Event(), cada valor tiene una referencia a Event() padre
            var newEvent = new Event();
            if (arg == null)
            {
                return BadRequest(new { message = "No value was given in the Body of the HTTP post" });
            }

            try
            {
                newEvent.TimeElapsed = arg.TimeElapsed;
                newEvent.CreateDate = DateTime.Now;
            }
            catch (RuntimeBinderException)
            {
                return BadRequest(new { message = "Property TimeElapsed was not given in the JSON" });
            }

            try
            {
                foreach (var i in arg.Values)
                {
                    newEvent.EventValueRaw.Add(new EventValueRaw { Value = i, Event = newEvent});
                }
            }
            catch (RuntimeBinderException)
            {
                return BadRequest(new { message = "Property Values was not given in the JSON" });
            }

            // Usar la estrategia de resistencia del EF Core usando multiples DbContexts
            // dentro de una transaccion explicitada
            // Ref: https://docs.microsoft.com/ef/core/miscellaneous/connection-resiliency
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.Event.Add(newEvent);
                    // TODO: Hacer el compute de EventValueRaw guardarlo en EventValueComputed

                    await _context.SaveChangesAsync();

                    // TODO: Rework 
                    foreach (var i in newEvent.EventValueRaw)
                    {
                        newEvent.EventValueComputed.Add(new EventValueComputed { Value = i.Value, Event = newEvent });
                    }

                    await _context.SaveChangesAsync();

                    // Agregar registro a Human
                    var newHuman = new Human
                    {
                        EventId = newEvent.EventId,
                        IdentificationDate = DateTime.Now
                    };

                    _context.Human.Add(newHuman);

                    await _context.SaveChangesAsync();


                    transaction.Commit();
                }
            });


            return Ok(AsyncGetEvent(newEvent.EventId).Result);
        }
    }
}
