/*********************************************************************************************************************************************************************************************
* Módulo                : Turnos
* Versión               : v2.0.7
* Descripción           : Módulo de gestión y reserva de Turnos
* Autor                 : Fabricio Guardia
* Fecha de Creación     : 09/04/2019
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
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public string DiaCompleto { get; set; }        
        public DateTime? HoraDesde { get; set; }       
        public DateTime? HoraHasta { get; set; }
        public virtual TrnCalendarioFeriado trnCalendarioFeriado { get; set; }
    }
}
