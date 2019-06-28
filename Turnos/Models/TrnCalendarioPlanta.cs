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
        [RequiredIf("LunesActivo", "checked")]
        public DateTime? LunesDesde { get; set; }
        [RequiredIf("LunesActivo", "checked")]
        public DateTime? LunesHasta { get; set; }
        public string MartesActivo { get; set; }
        [RequiredIf("MartesActivo", "checked")]
        public DateTime? MartesDesde { get; set; }
        [RequiredIf("MartesActivo", "checked")]
        public DateTime? MartesHasta { get; set; }
        public string MiercolesActivo { get; set; }
        [RequiredIf("MiercolesActivo", "checked")]
        public DateTime? MiercolesDesde { get; set; }
        [RequiredIf("MiercolesActivo", "checked")]
        public DateTime? MiercolesHasta { get; set; }
        public string JuevesActivo { get; set; }
        [RequiredIf("JuevesActivo", "checked")]
        public DateTime? JuevesDesde { get; set; }
        [RequiredIf("JuevesActivo", "checked")]
        public DateTime? JuevesHasta { get; set; }
        public string ViernesActivo { get; set; }
        [RequiredIf("ViernesActivo", "checked")]
        public DateTime? ViernesDesde { get; set; }
        [RequiredIf("ViernesActivo", "checked")]
        public DateTime? ViernesHasta { get; set; }
        public string SabadoActivo { get; set; }
        [RequiredIf("SabadoActivo", "checked")]
        public DateTime? SabadoDesde { get; set; }
        [RequiredIf("SabadoActivo", "checked")]
        public DateTime? SabadoHasta { get; set; }
        public string DomingoActivo { get; set; }
        [RequiredIf("DomingoActivo", "checked")]
        public DateTime? DomingoDesde { get; set; }
        [RequiredIf("DomingoActivo", "checked")]
        public DateTime? DomingoHasta { get; set; }
    }
    

}
