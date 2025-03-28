using System;

namespace WebApplicationApiCercleCultural.Models
{
    public class MensajesDTO
    {
        public int usuari_id { get; set; }
        public string nom_usuari { get; set; }
        public string missatge { get; set; }
        public DateTime? dataEnviament { get; set; }
    }
}
