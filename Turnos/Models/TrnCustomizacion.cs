using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    [Table("TurnosCustomizacion")]
    public partial class TrnCustomizacion
    {
        [Key]        
        public string Empid { get; set; }        
        public string IdPlanta { get; set; }
        public DateTime HorarioMinimo { get; set; }
        public DateTime HorarioMaximo { get; set; }
        public string DiasLaborables { get; set; }
        public bool EsObligatorioCantidadPallets { get; set; }
    }
}
