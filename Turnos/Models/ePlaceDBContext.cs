using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Turnos.Models;

namespace Turnos.Models
{
    public partial class ePlaceDBContext : DbContext
    {
        public ePlaceDBContext()
        {
        }

        public ePlaceDBContext(DbContextOptions<ePlaceDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TrnBoca> TrnBoca { get; set; }
        public virtual DbSet<TrnBocaTipo> TrnBocaTipo { get; set; }
        
        public virtual DbSet<TrnTurno> TrnTurno { get; set; }
        public virtual DbSet<TrnCalendarioFeriado> TrnCalendarioFeriado { get; set; }
        public virtual DbSet<TrnCalendarioFeriadoDetalle> TrnCalendarioFeriadoDetalle { get; set; }
        public virtual DbSet<TrnCalendarioPlanta> TrnCalendarioPlanta { get; set; }

        public virtual DbSet<TrnFeriado> TrnFeriado { get; set; }


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
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<TrnBoca>(entity =>
            {
                entity.HasKey(e => e.IdPlanta);

                entity.ToTable("TRN_Boca");

                entity.Property(e => e.BocaEntrega)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Empid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioResponsableBoca)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTipoBocaNavigation)
                    .WithMany(p => p.TrnBoca)
                    .HasForeignKey(d => d.IdTipoBoca)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRN_Boca_TRN_BocaTipo");
            });

            modelBuilder.Entity<TrnBocaTipo>(entity =>
            {
                entity.HasKey(e => e.IdTipoBoca);

                entity.ToTable("TRN_BocaTipo");

                entity.Property(e => e.IdTipoBoca).ValueGeneratedOnAdd();

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Empid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
            
            modelBuilder.Entity<TrnTurno>(entity =>
            {
                entity.HasKey(e => e.EventID)
                    .HasName("PK_Turno");

                entity.ToTable("TRN_Turno");

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

            modelBuilder.Entity<TrnCalendarioFeriado>(entity =>
            {
                entity.HasKey(e => e.IdCalendarioFeriado).HasName("PK_CalendarioFeriados");;

                entity.ToTable("TRN_CalendarioFeriados");

                entity.Property(e => e.Empid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);                              
            });

            modelBuilder.Entity<TrnCalendarioFeriadoDetalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalle);

                entity.ToTable("TRN_CalendarioFeriadoDetalle");

                entity.Property(e => e.IdCalendarioFeriado);

                entity.HasOne(d => d.trnCalendarioFeriado)
                    .WithMany(p => p.detalle)
                    .HasForeignKey(d => d.IdCalendarioFeriado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRN_CalendarioFeriadoDetalle_TRN_CalendarioFeriados");                

                entity.Property(e => e.Empid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DiaCompleto)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.HoraDesde).HasColumnType("datetime");

                entity.Property(e => e.HoraDesde).HasColumnType("datetime");
            });

            modelBuilder.Entity<TrnCalendarioPlanta>(entity =>
            {
                entity.HasKey(e => e.IdCalendarioPlanta);

                entity.ToTable("TRN_CalendarioPlanta");

                entity.Property(e => e.CalendarioPlanta)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Empid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LunesActivo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LunesDesde).HasColumnType("datetime");

                entity.Property(e => e.LunesHasta).HasColumnType("datetime");

                entity.Property(e => e.MartesActivo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MartesDesde).HasColumnType("datetime");

                entity.Property(e => e.MartesHasta).HasColumnType("datetime");

                entity.Property(e => e.MiercolesActivo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MiercolesDesde).HasColumnType("datetime");

                entity.Property(e => e.MiercolesHasta).HasColumnType("datetime");

                entity.Property(e => e.JuevesActivo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.JuevesDesde).HasColumnType("datetime");

                entity.Property(e => e.JuevesHasta).HasColumnType("datetime");

                entity.Property(e => e.ViernesActivo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ViernesDesde).HasColumnType("datetime");

                entity.Property(e => e.ViernesHasta).HasColumnType("datetime");

                entity.Property(e => e.SabadoActivo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SabadoDesde).HasColumnType("datetime");

                entity.Property(e => e.SabadoHasta).HasColumnType("datetime");

                entity.Property(e => e.DomingoActivo)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DomingoDesde).HasColumnType("datetime");

                entity.Property(e => e.DomingoHasta).HasColumnType("datetime");
            });

            modelBuilder.Entity<TrnFeriado>(entity =>
            {
                entity.HasKey(e => e.EventID)
                    .HasName("PK_Feriado");

                entity.ToTable("TRN_Feriado");

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


        public DbSet<Turnos.Models.TransporteTipo> TransporteTipo { get; set; }

    }
}
