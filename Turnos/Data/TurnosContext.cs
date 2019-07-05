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
        public TurnosContext(DbContextOptions<TurnosContext> options)
            : base(options)
        {
        }

        public DbSet<Turnos.Models.TrnTurno> Turno { get; set; }
        public DbSet<Turnos.Models.TrnFeriado> Feriado { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=10.20.20.50;Initial Catalog=ePlaceDB;Persist Security Info=False;User ID=executor;Password=executor;");
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrnTurno>(entity =>
            {                
                entity.HasKey(e => e.EventID)
                    .HasName("PK_Turno");

                entity.ToTable("TRN_Turno");

                //entity.Property(e => e.EventID).HasColumnName("EventID");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Empid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ThemeColor).HasMaxLength(10);
            });

            modelBuilder.Entity<TrnFeriado>(entity =>
            {
                //entity.Ignore(i => i.turnos);
                entity.HasKey(e => e.EventID)
                    .HasName("PK_Feriados");

                entity.ToTable("TRN_Feriados");

                entity.Property(e => e.EventID).HasColumnName("EventID");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Empid)
                            .IsRequired()
                            .HasMaxLength(20)
                            .IsUnicode(false);

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.Subject)
                            .IsRequired()
                            .HasMaxLength(100);

                entity.Property(e => e.ThemeColor).HasMaxLength(10);
            });

        }
    }
}
