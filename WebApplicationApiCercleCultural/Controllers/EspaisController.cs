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

namespace WebApplicationApiCercleCultural.Controllers
{
    public class EspaisController : ApiController
    {
        private CercleCulturalEntities2 db = new CercleCulturalEntities2();

        // GET: api/Espais
        public IQueryable<Espai> GetEspai()
        {
            return db.Espai;
        }

        // GET: api/Espais/5
        [ResponseType(typeof(Espai))]
        public async Task<IHttpActionResult> GetEspai(int id)
        {
            Espai espai = await db.Espai.FindAsync(id);
            if (espai == null)
            {
                return NotFound();
            }

            return Ok(espai);
        }

        // GET: api/Espais/EspaiEsdeveniments
        [HttpGet]
        [Route("api/Espais/EspaisEsdeveniments")]
        public IQueryable<object> GetEspaisEsdeveniments()
        {
            return db.Espai.Select(e => new {

                e.id,
                e.nom,
                e.ubicacio,
                e.butaques_fixes
            });
        }

        // GET: api/Espais/CrearEventReservaEspai
        [HttpGet]
        [Route("api/Espais/CrearEventReservaEspai")]
        public IQueryable<object> GetEspaiCrearEvent()
        {
            return db.Espai.Select(e => new
            {
                e.id,
                e.nom,
                e.butaques_fixes,
                e.capacitat
            });
        }

        // PUT: api/Espais/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEspai(int id, Espai espai)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != espai.id)
            {
                return BadRequest();
            }

            db.Entry(espai).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspaiExists(id))
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

        // POST: api/Espais
        [ResponseType(typeof(Espai))]
        public async Task<IHttpActionResult> PostEspai(Espai espai)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Espai.Add(espai);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = espai.id }, espai);
        }

        // DELETE: api/Espais/5
        [ResponseType(typeof(Espai))]
        public async Task<IHttpActionResult> DeleteEspai(int id)
        {
            Espai espai = await db.Espai.FindAsync(id);
            if (espai == null)
            {
                return NotFound();
            }

            db.Espai.Remove(espai);
            await db.SaveChangesAsync();

            return Ok(espai);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EspaiExists(int id)
        {
            return db.Espai.Count(e => e.id == id) > 0;
        }
    }
}