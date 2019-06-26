using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turnos.Models;
using Microsoft.EntityFrameworkCore;

namespace Turnos.Data
{
    public class TurnoContext : DbContext
    {
        public TurnoContext(DbContextOptions<TurnoContext> options) : base(options) { }

        public DbSet<Turno> Turno { get; set; }
    }
}
