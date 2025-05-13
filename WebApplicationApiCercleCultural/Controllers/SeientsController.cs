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
    public class SeientsController : ApiController
    {
        private CercleCulturalEntities2 db = new CercleCulturalEntities2();

        public SeientsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Seients
        public IQueryable<Seients> GetSeients()
        {
            return db.Seients;
        }

        // GET: api/Seients/5
        [ResponseType(typeof(Seients))]
        public async Task<IHttpActionResult> GetSeients(int id)
        {
            Seients seients = await db.Seients.FindAsync(id);
            if (seients == null)
            {
                return NotFound();
            }

            return Ok(seients);
        }

        // PUT: api/Seients/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSeients(int id, Seients seients)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != seients.id)
            {
                return BadRequest();
            }

            db.Entry(seients).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeientsExists(id))
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

        // POST: api/Seients
        [ResponseType(typeof(Seients))]
        public async Task<IHttpActionResult> PostSeients(Seients seients)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Seients.Add(seients);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = seients.id }, seients);
        }

        // DELETE: api/Seients/5
        [ResponseType(typeof(Seients))]
        public async Task<IHttpActionResult> DeleteSeients(int id)
        {
            Seients seients = await db.Seients.FindAsync(id);
            if (seients == null)
            {
                return NotFound();
            }

            db.Seients.Remove(seients);
            await db.SaveChangesAsync();

            return Ok(seients);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SeientsExists(int id)
        {
            return db.Seients.Count(e => e.id == id) > 0;
        }
    }
}