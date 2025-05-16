using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationApiCercleCultural.Models;
using WebApplicationApiCercleCultural.Models.DTOs;

namespace WebApplicationApiCercleCultural.Controllers
{
    public class ReservasController : ApiController
    {
        private CercleCulturalEntities2 db = new CercleCulturalEntities2();

        public ReservasController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Reservas
        public IQueryable<Reserva> GetReserva()
        {
            return db.Reserva;
        }

        // GET: api/Reservas/5
        [ResponseType(typeof(Reserva))]
        public async Task<IHttpActionResult> GetReserva(int id)
        {
            Reserva reserva = await db.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }

        // GET: api/Reservas/ReservasEsdeveniments
        [HttpGet]
        [Route("api/Reservas/ReservasEsdeveniments")]
        public IQueryable<object> GetReservasEsdeveniments()
        {
            return db.Reserva.Select(r => new
            {
                r.id,
                r.esdeveniment_id,
                r.espai_id
            });
        }

        // GET: api/Reservas/ByUser/5
        [HttpGet]
        [Route("api/Reservas/ReservasPerfil/{userId}")]
        public IQueryable<Reserva> GetReservasByUser(int userId)
        {
            return db.Reserva.Where(r => r.usuari_id == userId);
        }

        // GET: api/Reservas/CrearEventReservarEspai/{idEspai}/{dataInici}/{dataFi}
        [HttpGet]
        [Route("api/Reservas/CrearEventReservarEspai/{idEspai}/{dataInici}/{dataFi}")]
        public IQueryable<object> GetReservaCrearEvent(int idEspai, DateTime dataInici, DateTime dataFi)
        {
            return db.Reserva
                     .Where(r => r.espai_id == idEspai &&
                                 r.dataInici < dataFi &&
                                 r.dataFi > dataInici)
                     .Select(r => new
                     {
                         r.id,
                         r.estat,
                         r.dataInici,
                         r.dataFi
                     });
        }


        // PUT: api/Reservas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReserva(int id, Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reserva.id)
            {
                return BadRequest();
            }

            db.Entry(reserva).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reservas
        [ResponseType(typeof(Reserva))]
        public async Task<IHttpActionResult> PostReserva([FromBody] ReservaRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapea manualmente de DTO a entidad
            var reserva = new Reserva
            {
                usuari_id = req.usuari_id,
                esdeveniment_id = req.esdeveniment_id,
                espai_id = req.espai_id,
                dataReserva = req.dataReserva,
                estat = req.estat,
                tipus = req.tipus,
                dataInici = req.dataInici,
                dataFi = req.dataFi,
                numPlaces = req.numPlaces
                // NO toques Esdeveniment, Espai, Usuari ni Seients aquí
            };

            db.Reserva.Add(reserva);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = reserva.id }, reserva);
        }

        // DELETE: api/Reservas/5
        [ResponseType(typeof(Reserva))]
        public async Task<IHttpActionResult> DeleteReserva(int id)
        {
            Reserva reserva = await db.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            db.Reserva.Remove(reserva);
            await db.SaveChangesAsync();

            return Ok(reserva);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservaExists(int id)
        {
            return db.Reserva.Count(e => e.id == id) > 0;
        }
    }
}