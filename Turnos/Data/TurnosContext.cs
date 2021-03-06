﻿/*********************************************************************************************************************************************************************************************
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

        public virtual DbSet<Turnos.Models.TrnBoca> TrnBoca { get; set; }        
        public virtual DbSet<Turnos.Models.TrnBocaTipo> TrnBocaTipo { get; set; }
        public DbSet<Turnos.Models.TrnTurno> Turno { get; set; }
        public DbSet<Turnos.Models.TrnFeriadoCabecera> FeriadoCabecera { get; set; }
        public DbSet<Turnos.Models.TrnFeriado> Feriado { get; set; }
        public DbSet<Turnos.Models.TrnCustomizacion> customizacion { get; set; }
        public DbSet<Turnos.Models.TrnCalendarioPlantaCabecera> CalendarioPlantaCabecera { get; set; }
        public DbSet<Turnos.Models.TrnCalendarioPlanta> CalendarioPlanta { get; set; }
        public DbSet<Turnos.Models.TrnUsuarioPlanta> TrnUsuarioPlanta { get; set; }
        public DbSet<Turnos.Models.TrnProvMapSocio> TrnProvMapSocio { get; set; }
        public DbSet<Turnos.Models.TrnUsuarioMaestros> TrnUsuarioMaestros { get; set; }
        public DbSet<Turnos.Models.TrnUsuariosBoca> TrnUsuariosBoca { get; set; }
        public virtual DbSet<TransporteTipo> TransporteTipo { get; set; }
        public virtual DbSet<ProvMails> ProvMails { get; set; }
        public virtual DbSet<MailsEnvios> MailsEnvios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=10.20.20.50;Initial Catalog=ePlaceDB;Persist Security Info=False;User ID=executor;Password=executor;");
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           

            modelBuilder.Entity<TrnBoca>(entity =>
            {
                entity.HasKey(e => e.IdBoca);

                entity.Property(e => e.IdPlanta);

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

                entity.Property(e => e.Estado);

                entity.Property(e => e.DiasPrevision);

                entity.Property(e => e.user_name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);             

                entity.Property(e => e.color);

                entity.HasOne(d => d.TrnCalendarioplantaCabeceraNavigation)
                    .WithMany(p => p.TrnBoca)
                    .HasForeignKey(d => d.IdCalendarioPlanta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRN_Boca_TRN_CalendarioPlantaCabecera");

                entity.HasOne(d => d.TrnCalendarioFeriadoCabeceraNavigation)
                   .WithMany(p => p.TrnBoca)
                   .HasForeignKey(d => d.IdCalendarioFeriado)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_TRN_Boca_TRN_FeriadosCabecera");

                entity.HasOne(d => d.IdTipoBocaNavigation)
                    .WithMany(p => p.TrnBoca)
                    .HasForeignKey(d => d.IdTipoBoca)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRN_Boca_TRN_BocaTipo");

                entity.HasOne(d => d.TrnUsuariosBocaNavigation)
                    .WithMany(p => p.TrnBoca)
                    .HasForeignKey(d => d.user_name)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRN_Boca_Usuarios");

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

            modelBuilder.Entity<ProvMails>(entity =>
            {
                entity.HasKey(e => e.prov_id);

                entity.ToTable("ProvMails");

                entity.Property(e => e.user_name);

                entity.Property(e => e.tipo);

                entity.Property(e => e.emp_id);
                                  
            });

            modelBuilder.Entity<MailsEnvios>(entity =>
            {
                entity.HasKey(e => e.idMail);

                entity.ToTable("MailsEnvios");

                entity.Property(e => e.idFrom);

                entity.Property(e => e.idFrom);

                entity.Property(e => e.idTo);

                entity.Property(e => e.idUsTo);

                entity.Property(e => e.idTipo);

                entity.Property(e => e.fhAlta);

                entity.Property(e => e.param1);

                entity.Property(e => e.param2);

                entity.Property(e => e.param3);

                entity.Property(e => e.estado);

                entity.Property(e => e.fhProc);

                entity.Property(e => e.fhModif);

                entity.Property(e => e.URL);

                entity.Property(e => e.CuerpoLibre);

                entity.Property(e => e.AsuntoLibre);

            });

            modelBuilder.Entity<TransporteTipo>(entity =>
            {
                entity.HasKey(e => e.IdTransporteTipo);

                entity.ToTable("TRN_TransporteTipo");

                entity.Property(e => e.IdTransporteTipo);

                entity.Property(e => e.Empid)
                   .IsRequired()
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrnUsuariosBoca>(entity =>
            {
                entity.HasKey(e => e.user_name);

                entity.ToTable("Usuarios");

                entity.Property(e => e.user_id);

                entity.Property(e => e.perfil);

                entity.Property(e => e.nombre);

                entity.Property(e => e.apellido);

                entity.Property(e => e.eMail);

            });

            modelBuilder.Entity<TrnTurno>(entity =>
            {                
                entity.HasKey(e => e.EventID)
                    .HasName("PK_Turno");

                entity.ToTable("TRN_Turno");

                //entity.Property(e => e.EventID).HasColumnName("EventID");

                entity.Property(e => e.IdTipoBoca).HasColumnName("IdTipoBoca");

                entity.Property(e => e.IdBoca).HasColumnName("IdBoca");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Empid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Provid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ThemeColor).HasMaxLength(10);

                entity.Property(e => e.IdTransporteTipo).HasColumnName("IdTransporteTipo");

                entity.Property(e => e.TransporteTipo).HasColumnName("TransporteTipo");

                entity.Property(e => e.KGPrevistos).HasColumnName("KGPrevistos");

                entity.Property(e => e.PalletsPrevistos).HasColumnName("PalletsPrevistos");

                entity.Property(e => e.Rendering).HasColumnName("Rendering");

            });

            modelBuilder.Entity<TrnFeriadoCabecera>(entity =>
            {
                //entity.Ignore(i => i.turnos);
                entity.HasKey(e => e.IdCalendarioFeriado);                    

                entity.ToTable("TRN_FeriadosCabecera");

                entity.Property(e => e.Empid)
                            .IsRequired()
                            .HasMaxLength(20)
                            .IsUnicode(false);

                entity.Property(e => e.Descripcion).HasMaxLength(300);
                
            });

            modelBuilder.Entity<TrnFeriado>(entity =>
            {
                //entity.Ignore(i => i.turnos);
                entity.HasKey(e => e.EventID)
                    .HasName("PK_Feriados");

                entity.ToTable("TRN_Feriados");

                entity.Property(e => e.EventID).HasColumnName("EventID");

                entity.Property(e => e.IdCalendarioFeriado).HasColumnName("IdCalendarioFeriado");

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

            modelBuilder.Entity<TrnCustomizacion>(entity =>
            {                
                entity.HasKey(e => e.Empid)
                    .HasName("PK_TurnosCustomizacion");

                entity.Property(e => e.IdPlanta);

                entity.Property(e => e.HorarioMinimo).HasColumnType("datetime");

                entity.Property(e => e.HorarioMaximo).HasColumnType("datetime");
                
                entity.Property(e => e.DiasLaborables).HasColumnName("DiasLaborables");

            });

            modelBuilder.Entity<TrnCalendarioPlantaCabecera>(entity =>
            {
                //entity.Ignore(i => i.turnos);
                entity.HasKey(e => e.IdCalendarioPlanta);

                entity.ToTable("TRN_CalendarioPlantaCabecera");

                entity.Property(e => e.Empid)
                            .IsRequired()
                            .HasMaxLength(20)
                            .IsUnicode(false);

                entity.Property(e => e.Descripcion).HasMaxLength(300);

            });

            modelBuilder.Entity<TrnCalendarioPlanta>(entity =>
            {                
                entity.HasKey(e => e.EventID)
                    .HasName("PK_CalendarioPlanta");

                entity.ToTable("TRN_CalendarioPlanta");

                entity.Property(e => e.EventID).HasColumnName("EventID");

                entity.Property(e => e.IdCalendarioPlanta).HasColumnName("IdCalendarioPlanta");

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
                entity.Property(e => e.Dow).HasMaxLength(1);
            });

            modelBuilder.Entity<TrnUsuarioPlanta>(entity =>
            {
                
                entity.HasKey(e => e.User_Id);

                entity.ToTable("UsuarioPlantas");

                entity.Property(e => e.Codigo).HasColumnName("Codigo");

                entity.Property(e => e.Descripcion).HasColumnName("Codigo");

            });

            modelBuilder.Entity<TrnProvMapSocio>(entity =>
            {                                
                entity.HasKey(e => e.Prov_Id);

                entity.HasKey(e => e.Emp_Id);

                entity.ToTable("ProvMapSocio");

                entity.Property(e => e.Prov_Id).HasColumnName("prov_id");

                entity.Property(e => e.Emp_Id).HasColumnName("emp_id");
                

            });

            modelBuilder.Entity<TrnUsuarioMaestros>(entity =>
            {

                entity.HasKey(e => e.usr_Id);

                entity.ToTable("UsuarioMaestros");

                entity.Property(e => e.nombre).HasColumnName("nombre");

            });
        }
    }
}
