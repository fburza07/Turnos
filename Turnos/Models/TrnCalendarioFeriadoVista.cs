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
    public partial class TrnCalendarioFeriadoVista
    {
        public TrnCalendarioFeriado trnCalendarioFeriado {get; set;}
        public TrnCalendarioFeriadoDetalle titulos { get; set; }
        public List<TrnCalendarioFeriadoDetalle> detalle { get; set; }
    }
}
