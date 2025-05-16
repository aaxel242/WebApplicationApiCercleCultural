using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationApiCercleCultural.Models;
using WebApplicationApiCercleCultural.Models.DTOs;
using WebApplicationApiCercleCultural.Services;

namespace WebApplicationApiCercleCultural.Controllers
{
    public class UsuarisController : ApiController
    {
        private CercleCulturalEntities2 db = new CercleCulturalEntities2();

        public UsuarisController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

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

        [HttpPost]
        [Route("api/Usuaris/UploadImage/{id}")]
        public async Task<IHttpActionResult> UploadImage(int id)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count == 0)
                {
                    return BadRequest("No se envió ninguna imagen");
                }

                var file = httpRequest.Files[0];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var folderPath = HttpContext.Current.Server.MapPath("~/imagenes");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);
                file.SaveAs(filePath);

                var usuario = await db.Usuari.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                usuario.FotoPerfil = fileName;
                await db.SaveChangesAsync();

                return Ok(new { fileName });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Usuaris/GetImage/nombre.jpg
        [HttpGet]
        [Route("api/Usuaris/GetImage/{fileName}")]
        public IHttpActionResult GetImage(string fileName)
        {
            try
            {
                // Crear una instancia de ImageService
                var imageService = new ImageService();

                // Usar la instancia para llamar a GetImagePath
                var imagePath = imageService.GetImagePath(fileName);

                if (!File.Exists(imagePath))
                    return NotFound();

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(imagePath, FileMode.Open));
                response.Content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    // Clase auxiliar para devolver el FileStream
    public class FileStreamResult : IHttpActionResult
    {
        private readonly FileStream _stream;
        private readonly string _mediaType;

        public FileStreamResult(FileStream stream, string mediaType)
        {
            _stream = stream;
            _mediaType = mediaType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(_stream)
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_mediaType);
            return Task.FromResult(response);
        }
    }
}
