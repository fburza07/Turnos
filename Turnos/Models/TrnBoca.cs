using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    public partial class TrnBoca
    {
        [Key]
        public int IdBoca { get; set; }
        public int IdPlanta { get; set; }
        public string Empid { get; set; }
        public string BocaEntrega { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public short SegmentoCantMin { get; set; }
        public short SegmentoCantPalletMax { get; set; } 
        [Required]
        public int IdCalendarioPlanta { get; set; }        
        public int IdCalendarioFeriado { get; set; }
        public bool VerificaSobreposicionHoraria { get; set; }
        public short CantidadCitasSimultaneas { get; set; }
        public byte IdTipoBoca { get; set; }
        public string UsuarioResponsableBoca { get; set; }

        public virtual TrnBocaTipo IdTipoBocaNavigation { get; set; }
        public virtual TrnFeriadoCabecera TrnCalendarioFeriadoCabeceraNavigation { get; set; }
        public virtual TrnCalendarioPlantaCabecera TrnCalendarioplantaCabeceraNavigation { get; set; }
        private IConfiguration Configuration;
        public TrnBoca()
        {
        }

        public TrnBoca(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }

        public string TraerHorarioMinimoPorBoca(short dia)
        {
            string ret = "";
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_HorarioMinimoPorBoca");
            sqlcommand.Parameters.AddWithValue("@EmpID", this.Empid);
            sqlcommand.Parameters.AddWithValue("@Dia", dia);
            DataTable dt = cn.Execute(sqlcommand);
            if (dt.Rows[0]["HorarioMinimoPorBoca"] != null && dt.Rows[0]["HorarioMinimoPorBoca"].ToString() != "")
                ret = dt.Rows[0]["HorarioMinimoPorBoca"].ToString();

            return ret;
        }

        public string TraerHorarioMaximoPorBoca(short dia)
        {
            string ret = "";
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_HorarioMaximoPorBoca");
            sqlcommand.Parameters.AddWithValue("@EmpID", this.Empid);
            sqlcommand.Parameters.AddWithValue("@Dia", dia);
            DataTable dt = cn.Execute(sqlcommand);
            if (dt.Rows[0]["HorarioMaximoPorBoca"] != null && dt.Rows[0]["HorarioMaximoPorBoca"].ToString() != "")
                ret = dt.Rows[0]["HorarioMaximoPorBoca"].ToString();

            return ret;
        }

        public short TraerSegmentoMinimo()
        {
            short ret = 0;
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_SegmentoMinimoPorTipoBoca");
            sqlcommand.Parameters.AddWithValue("@empid", this.Empid);
            sqlcommand.Parameters.AddWithValue("@IdTipoBoca", this.IdTipoBoca);
            DataTable dt = cn.Execute(sqlcommand);            
            if (dt.Rows[0]["Segmento"] !=null && dt.Rows[0]["Segmento"].ToString() != "")
                ret = Convert.ToInt16(dt.Rows[0]["Segmento"]);

            return ret;
        }
    }
}
