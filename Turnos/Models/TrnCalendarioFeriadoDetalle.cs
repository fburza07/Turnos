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
    [Table("TRN_CalendarioFeriadoDetalle")]
    public partial class TrnCalendarioFeriadoDetalle
    {
        [Key]
        public int IdDetalle { get; set; }
        public int IdCalendarioFeriado { get; set; }
        public string Empid { get; set; }
        [Required(ErrorMessage = "Debe completador este campo!")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public string DiaCompleto { get; set; }
        [RequiredIf("DiaCompleto", "N")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? HoraDesde { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? HoraHasta { get; set; }
        public TrnCalendarioFeriado trnCalendarioFeriado { get; set; }
    }
}
