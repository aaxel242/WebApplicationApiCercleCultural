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
    public class EsdevenimentsController : ApiController
    {
        private CercleCulturalEntities2 db = new CercleCulturalEntities2();

        public EsdevenimentsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Esdeveniments
        public IQueryable<Esdeveniment> GetEsdeveniment()
        {
            return db.Esdeveniment;
        }

        // GET: api/Esdeveniments/5
        [ResponseType(typeof(Esdeveniment))]
        public async Task<IHttpActionResult> GetEsdeveniment(int id)
        {
            Esdeveniment esdeveniment = await db.Esdeveniment.FindAsync(id);
            if (esdeveniment == null)
            {
                return NotFound();
            }

            return Ok(esdeveniment);
        }

        // GET: api/Esdeveniments/EsdevenimentsEsdeveniments
        [HttpGet]
        [Route("api/Esdeveniments/EsdevenimentsEsdeveniments")]
        public IQueryable<object> GetEsdevenimentsEsdeveniments()
        {
            return db.Esdeveniment.Select(e => new {
                e.nom,
                e.descripcio,
                e.dataInici,
                e.imatge,
                e.per_infants
            });
        }

        // PUT: api/Esdeveniments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEsdeveniment(int id, Esdeveniment esdeveniment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != esdeveniment.id)
            {
                return BadRequest();
            }

            db.Entry(esdeveniment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EsdevenimentExists(id))
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

        // POST: api/Esdeveniments
        [ResponseType(typeof(Esdeveniment))]
        public async Task<IHttpActionResult> PostEsdeveniment(Esdeveniment esdeveniment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Esdeveniment.Add(esdeveniment);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = esdeveniment.id }, esdeveniment);
        }

        // DELETE: api/Esdeveniments/5
        [ResponseType(typeof(Esdeveniment))]
        public async Task<IHttpActionResult> DeleteEsdeveniment(int id)
        {
            Esdeveniment esdeveniment = await db.Esdeveniment.FindAsync(id);
            if (esdeveniment == null)
            {
                return NotFound();
            }

            db.Esdeveniment.Remove(esdeveniment);
            await db.SaveChangesAsync();

            return Ok(esdeveniment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EsdevenimentExists(int id)
        {
            return db.Esdeveniment.Count(e => e.id == id) > 0;
        }
    }
}