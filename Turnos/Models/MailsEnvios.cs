using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    public partial class MailsEnvios
    {
        [Key]
        public int idMail { get; set; }
        public string idFrom { get; set; }
        public string idUsFrom { get; set; }        
        public string idTo { get; set; }
        public string idUsTo { get; set; }
        public int idTipo { get; set; }
        public DateTime fhAlta { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string estado { get; set; }
        public DateTime fhProc { get; set; }
        public DateTime fhModif { get; set; }
        public string URL { get; set; }
        public string CuerpoLibre { get; set; }
        public string AsuntoLibre { get; set; }

        private IConfiguration Configuration;
        public MailsEnvios()
        {
        }

        public MailsEnvios(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }        
       
    }
}
