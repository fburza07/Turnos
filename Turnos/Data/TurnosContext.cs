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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Turnos.Models
{
    public class TurnosContext : DbContext
    {
        public TurnosContext (DbContextOptions<TurnosContext> options)
            : base(options)
        {
        }

        public DbSet<Turnos.Models.Turno> Turno { get; set; }
    }
}
