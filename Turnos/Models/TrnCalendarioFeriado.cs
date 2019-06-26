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
    [Table("TRN_CalendarioFeriado")]
    public partial class TrnCalendarioFeriado
    {
        [Key]
        public int IdCalendarioFeriado { get; set; }
        public string Empid { get; set; }   
        [Required (ErrorMessage ="Complete la descripcion!")]
        public string Descripcion { get; set; }
        public ICollection<TrnCalendarioFeriadoDetalle> trnCalendarioFeriadoDetalles { get; set; }
}
}
