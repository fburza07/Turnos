/*********************************************************************************************************************************************************************************************
* Módulo                : Turnos
* Versión               : v2.0.7
* Descripción           : Módulo de gestión y reserva de Turnos
* Autor                 : Fernando Burza
* Fecha de Creación     : 26/06/2019
* Base de Datos         : ePlaceDB
*********************************************************************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos.Models
{
    [Table("TRN_CalendarioPlanta")]
    public partial class TrnCalendarioPlanta
    {
        [Key]
        public int IdCalendarioPlanta { get; set; }
        public string CalendarioPlanta { get; set; }
        public string Empid { get; set; }   
        [Required (ErrorMessage ="Complete la descripcion!")]
        public string Descripcion { get; set; }
        public string LunesActivo { get; set; }
        public DateTime? LunesDesde { get; set; }
        public DateTime? LunesHasta { get; set; }
        public string MartesActivo { get; set; }
        public DateTime? MartesDesde { get; set; }
        public DateTime? MartesHasta { get; set; }
        public string MiercolesActivo { get; set; }
        public DateTime? MiercolesDesde { get; set; }
        public DateTime? MiercolesHasta { get; set; }
        public string JuevesActivo { get; set; }
        public DateTime? JuevesDesde { get; set; }
        public DateTime? JuevesHasta { get; set; }
        public string ViernesActivo { get; set; }
        public DateTime? ViernesDesde { get; set; }
        public DateTime? ViernesHasta { get; set; }
        public string SabadoActivo { get; set; }
        public DateTime? SabadoDesde { get; set; }
        public DateTime? SabadoHasta { get; set; }
        public string DomingoActivo { get; set; }
        public DateTime? DomingoDesde { get; set; }
        public DateTime? DomingoHasta { get; set; }
    }
}
