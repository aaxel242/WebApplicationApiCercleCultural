using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationApiCercleCultural.Models;

namespace WebApplicationApiCercleCultural.Controllers
{   
    public class MensajesController : ApiController
    {

        private CercleCulturalEntities3 db = new CercleCulturalEntities3();

        public MensajesController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }
        // GET: api/Mensajes
        public IQueryable<Mensajes> GetMensajes()
        {
            return db.Mensajes;
        }

        // GET: api/Mensajes/5
        [ResponseType(typeof(Mensajes))]
        public async Task<IHttpActionResult> GetMensajes(int id)
        {
            Mensajes mensajes = await db.Mensajes.FindAsync(id);
            if (mensajes == null)
            {
                return NotFound();
            }

            return Ok(mensajes);
        }

        // PUT: api/Mensajes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMensajes(int id, Mensajes mensajes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mensajes.id)
            {
                return BadRequest();
            }

            db.Entry(mensajes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajesExists(id))
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

        // POST: api/Mensajes
        [ResponseType(typeof(Mensajes))]
        public async Task<IHttpActionResult> PostMensajesChat(MensajesDTO mensajeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapear los datos recibidos en el DTO a la entidad Mensajes
            Mensajes nuevoMensaje = new Mensajes
            {
                usuari_id = mensajeDto.usuari_id,
                nom_usuari = mensajeDto.nom_usuari,
                missatge = mensajeDto.missatge,
                // Si no se envía una fecha o viene en un valor por defecto, asignamos la fecha actual
                dataEnviament = (mensajeDto.dataEnviament == null || mensajeDto.dataEnviament == DateTime.MinValue)
                                ? DateTime.Now
                                : mensajeDto.dataEnviament.Value
            };

            db.Mensajes.Add(nuevoMensaje);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = nuevoMensaje.id }, nuevoMensaje);
        }

        // DELETE: api/Mensajes/5
        [ResponseType(typeof(Mensajes))]
        public async Task<IHttpActionResult> DeleteMensajes(int id)
        {
            Mensajes mensajes = await db.Mensajes.FindAsync(id);
            if (mensajes == null)
            {
                return NotFound();
            }

            db.Mensajes.Remove(mensajes);
            await db.SaveChangesAsync();

            return Ok(mensajes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MensajesExists(int id)
        {
            return db.Mensajes.Count(e => e.id == id) > 0;
        }
    }
}
