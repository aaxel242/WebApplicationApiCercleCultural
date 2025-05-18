using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationApiCercleCultural.Models.DTOs
{
    public class ReservaRequest
    {
        public int usuari_id { get; set; }
        public DateTime dataReserva { get; set; }
        public string estat { get; set; }
        public string tipus { get; set; }
        public int espai_id { get; set; }
        public int esdeveniment_id { get; set; }
        public DateTime dataInici { get; set; }
        public DateTime dataFi { get; set; }
        public int numPlaces { get; set; }
    }

}
