using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    public partial class ProvMails
    {
        [Key]
        public string prov_id { get; set; }
        public string user_name { get; set; }
        public int tipo { get; set; }        
        public string emp_id { get; set; }        

        private IConfiguration Configuration;
        public ProvMails()
        {
        }

        public ProvMails(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }
       
    }
}
