using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Turnos.Models
{
    [Table("TRN_Turno")]
    public class TrnTurno
    {
        [Key]
        public int EventID { get; set; }
        public string Empid { get; set; }
        public string Provid { get; set; }        
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }

        public int IdTransporteTipo { get; set; }
        public string TransporteTipo { get; set; }
        public int KGPrevistos { get; set; }
        public int PalletsPrevistos { get; set; }
        public byte IdTipoBoca { get; set; }
        public int IdBoca { get; set; }
        public bool ConfirmadoAdherente { get; set; }
        public bool ConfirmadoProveedor { get; set; }
        public bool Rendering { get; set; }

        public virtual TrnBocaTipo IdTipoBocaNavigation { get; set; }

        private IConfiguration Configuration;

        public TrnTurno()
        {
        }

        public TrnTurno(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }

        public bool ExisteEvento(string empID, string idPlanta, int eventID, int idBoca, byte idTipoBoca, DateTime fdesde, DateTime? fhasta)
        {
            bool existeEvento = false;
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_VerificarEvento");
            sqlcommand.Parameters.AddWithValue("@EmpID", empID);
            sqlcommand.Parameters.AddWithValue("@IdPlanta", idPlanta);            
            sqlcommand.Parameters.AddWithValue("@EventID", eventID);
            sqlcommand.Parameters.AddWithValue("@IdBoca", idBoca);
            sqlcommand.Parameters.AddWithValue("@IdTipoBoca", idTipoBoca);
            sqlcommand.Parameters.AddWithValue("@FechaDesde", fdesde);
            sqlcommand.Parameters.AddWithValue("@FechaHasta", fhasta);
            DataTable dt = cn.Execute(sqlcommand);
            if (Convert.ToBoolean(dt.Rows[0]["existeEvento"]))
                existeEvento = true;

            return existeEvento;
        }

        internal List<TrnTurno> ObtenerTurnosPlanta(string codigo, string estado)
        {
            List<TrnTurno> turnos = new List<TrnTurno>();
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_ObtenerTurnosPlanta");
            sqlcommand.Parameters.AddWithValue("@EmpID", this.Empid);
            sqlcommand.Parameters.AddWithValue("@IdPlanta", codigo);
            if (estado == null)
                sqlcommand.Parameters.AddWithValue("@Estado", "");
            else
                sqlcommand.Parameters.AddWithValue("@Estado", "NoConfirmados");
            DataTable dt = cn.Execute(sqlcommand);
            turnos = cn.ConvertDataTable<TrnTurno>(dt);
            if (turnos.Count > 0)
                return turnos;
            else
                return new List<TrnTurno>();
        }

        internal List<TrnTurno> ObtenerTurnos(byte idTipoBoca, string idPlanta, string estado)
        {
            List<TrnTurno> turnos = new List<TrnTurno>();
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_ObtenerTurnos");
            sqlcommand.Parameters.AddWithValue("@EmpID", this.Empid);
            sqlcommand.Parameters.AddWithValue("@ProvID", this.Provid);
            sqlcommand.Parameters.AddWithValue("@IdPlanta", idPlanta);
            sqlcommand.Parameters.AddWithValue("@IdTipoBoca", idTipoBoca);
            if(estado == null)
                sqlcommand.Parameters.AddWithValue("@Estado", "");
            else
                sqlcommand.Parameters.AddWithValue("@Estado", "NoConfirmados");
            DataTable dt = cn.Execute(sqlcommand);
            turnos = cn.ConvertDataTable<TrnTurno>(dt);
            if (turnos.Count > 0)
                return turnos;
            else
                return new List<TrnTurno>();
        }

        internal List<TrnFeriado> ObtenerFeriados(string idPlanta)
        {
            List<TrnFeriado> feriados = new List<TrnFeriado>();
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_ObtenerFeriados");
            sqlcommand.Parameters.AddWithValue("@EmpID", this.Empid);
            sqlcommand.Parameters.AddWithValue("@IdPlanta", idPlanta);            
            DataTable dt = cn.Execute(sqlcommand);
            feriados = cn.ConvertDataTable<TrnFeriado>(dt);
            if (feriados.Count > 0)
                return feriados;
            else
                return new List<TrnFeriado>();
        }

    }
}
