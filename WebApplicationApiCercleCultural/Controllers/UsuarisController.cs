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
    public class UsuarisController : ApiController
    {
        private CercleCulturalEntities2 db = new CercleCulturalEntities2();

        // GET: api/Usuaris
        public IQueryable<Usuari> GetUsuari()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.Usuari;
        }

        // GET: api/Usuaris/5
        [ResponseType(typeof(Usuari))]
        public async Task<IHttpActionResult> GetUsuari(int id)
        {
            Usuari usuari = await db.Usuari.FindAsync(id);
            if (usuari == null)
            {
                return NotFound();
            }

            return Ok(usuari);
        }


        // GET: api/Usuaris/UsuarisPerfil
        [HttpGet]
        [Route("api/Usuaris/UsuarisPerfil")]
        public IQueryable<object> GetUsuarisPerfil()
        {
            return db.Usuari.Select(u => new
            {
                u.nom,
                u.email
            });
        }

        // GET: api/Usuaris/UsuarisPerfilConfiguracionIdioma
        [HttpGet]
        [Route("api/Usuaris/UsuarisPerfilConfiguracionIdioma")]
        public IQueryable<object> GetUsuarisPerfilConfiguracionIdioma()
        {
            return db.Usuari.Select(u => new
            {
                u.idioma
            });
        }

        // GET: api/Usuaris/UsuarisPerfilConfiguracion/CambiarContrasenya
        [HttpGet]
        [Route("api/Usuaris/UsuarisPerfilConfiguracion/CambiarContrasenya")]
        public IQueryable<object> GetUsuarisPerfilConfiguracionCambiarContrasenya()
        {
            return db.Usuari.Select(u => new
            {
                u.contrasenya
            });
        }

        // PUT: api/Usuaris/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuari(int id, Usuari usuari)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuari.id)
            {
                return BadRequest();
            }

            db.Entry(usuari).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariExists(id))
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


        // PUT: api/Usuaris/UsuariPerfilConfiguracion/5
        [HttpPut]
        [Route("api/Usuaris/UsuariPerfilConfiguracion/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuariActualizarNombre(int id, UsuarisPerfilConfiguracion usuarisPerfilConfiguracion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Buscar el usuario en la base de datos por su ID
            Usuari usuarioExistente = await db.Usuari.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }

            // Modificar solo el nombre del usuario
            usuarioExistente.nom = usuarisPerfilConfiguracion.nom;

            db.Entry(usuarioExistente).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent); // Respuesta 204
        }

        // PUT: api/Usuaris/UsuariPerfilConfiguracionCambiarContrasenya/5
        [HttpPut]
        [Route("api/Usuaris/ActualizarContrasenya/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuariActualizarContrasenya(int id, UsuarisPerfilConfiguracionCambiarContrasenyaDTO usuarisPerfilConfiguracionCambiarContrasenyaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Usuari usuarioExistente = await db.Usuari.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }
            // Modificar solo la contraseña del usuario
            usuarioExistente.contrasenya = usuarisPerfilConfiguracionCambiarContrasenyaDTO.contrasenya;
            db.Entry(usuarioExistente).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent); // Respuesta 204
        }


        // POST: api/Usuaris
        [ResponseType(typeof(Usuari))]
        public async Task<IHttpActionResult> PostUsuari(Usuari usuari)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuari.Add(usuari);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuari.id }, usuari);
        }

        

        // DELETE: api/Usuaris/5
        [ResponseType(typeof(Usuari))]
        public async Task<IHttpActionResult> DeleteUsuari(int id)
        {
            Usuari usuari = await db.Usuari.FindAsync(id);
            if (usuari == null)
            {
                return NotFound();
            }

            db.Usuari.Remove(usuari);
            await db.SaveChangesAsync();

            return Ok(usuari);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuariExists(int id)
        {
            return db.Usuari.Count(e => e.id == id) > 0;
        }
    }
}