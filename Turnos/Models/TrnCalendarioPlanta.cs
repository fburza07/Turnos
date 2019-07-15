using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    [Table("TRN_CalendarioPlanta")]
    public partial class TrnCalendarioPlanta
    {
        [Key]
        public int EventID { get; set; }
        [Required]
        public int IdCalendarioPlanta { get; set; }        
        public string Empid { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
        public byte Dow { get; set; }
        

        private IConfiguration Configuration;

        public TrnCalendarioPlanta()
        {
        }

        public TrnCalendarioPlanta(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }
        
    }
}
