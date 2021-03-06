﻿/*********************************************************************************************************************************************************************************************
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
    [Table("TRN_CalendarioPlantas")]
    public partial class TrnCalendarioPlantas
    {
        [Key]
        public int IdCalendarioPlanta { get; set; }
        [Required (ErrorMessage ="Complete la clave!")]
        [MaxLength(2)]
        public string CalendarioPlanta { get; set; }
        public string Empid { get; set; }   
        [Required (ErrorMessage ="Complete la descripcion!")]
        public string Descripcion { get; set; }
        public string LunesActivo { get; set; }
        [RequiredIf("LunesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? LunesDesde { get; set; }
        [RequiredIf("LunesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? LunesHasta { get; set; }
        public string MartesActivo { get; set; }
        [RequiredIf("MartesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? MartesDesde { get; set; }
        [RequiredIf("MartesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? MartesHasta { get; set; }
        public string MiercolesActivo { get; set; }
        [RequiredIf("MiercolesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? MiercolesDesde { get; set; }
        [RequiredIf("MiercolesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? MiercolesHasta { get; set; }
        public string JuevesActivo { get; set; }
        [RequiredIf("JuevesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? JuevesDesde { get; set; }
        [RequiredIf("JuevesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? JuevesHasta { get; set; }
        public string ViernesActivo { get; set; }
        [RequiredIf("ViernesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? ViernesDesde { get; set; }
        [RequiredIf("ViernesActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? ViernesHasta { get; set; }
        public string SabadoActivo { get; set; }
        [RequiredIf("SabadoActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? SabadoDesde { get; set; }
        [RequiredIf("SabadoActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? SabadoHasta { get; set; }
        public string DomingoActivo { get; set; }
        [RequiredIf("DomingoActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? DomingoDesde { get; set; }
        [RequiredIf("DomingoActivo", "S")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? DomingoHasta { get; set; }
    }
    

}
