﻿using Microsoft.Extensions.Configuration;
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
        public string IdPlanta { get; set; }
        public string Empid { get; set; }
        [Required(ErrorMessage = "Debe completar el nombre de la boca")]
        public string BocaEntrega { get; set; }
        [Required(ErrorMessage = "Debe completar la descripción")]
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        [Required(ErrorMessage = "Debe completar la cantidad de minutos")]
        public short SegmentoCantMin { get; set; }
        [Required(ErrorMessage = "Debe completar la cantidad de pallets")]
        public short SegmentoCantPalletMax { get; set; }
        public int IdCalendarioPlanta { get; set; }
        public int IdCalendarioFeriado { get; set; }
        public bool VerificaSobreposicionHoraria { get; set; }
        [Required(ErrorMessage = "Debe completar la cantidad de citas simultaneas")]
        public short CantidadCitasSimultaneas { get; set; }
        public byte IdTipoBoca { get; set; }
        [Required(ErrorMessage = "Debe completar los dias de prevision")]
        public int DiasPrevision { get; set; }
        public string user_name { get; set; }        
        public string color { get; set; }
        
        public virtual TrnBocaTipo IdTipoBocaNavigation { get; set; }
        public virtual TrnFeriadoCabecera TrnCalendarioFeriadoCabeceraNavigation { get; set; }
        public virtual TrnCalendarioPlantaCabecera TrnCalendarioplantaCabeceraNavigation { get; set; }
        public virtual TrnUsuariosBoca TrnUsuariosBocaNavigation { get; set; }

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
            sqlcommand.Parameters.AddWithValue("@IdPlanta", this.IdPlanta);
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
            sqlcommand.Parameters.AddWithValue("@IdPlanta", this.IdPlanta);
            DataTable dt = cn.Execute(sqlcommand);
            if (dt.Rows[0]["HorarioMaximoPorBoca"] != null && dt.Rows[0]["HorarioMaximoPorBoca"].ToString() != "")
                ret = dt.Rows[0]["HorarioMaximoPorBoca"].ToString();

            return ret;
        }

        public short TraerSegmentoMinimo()
        {
            short ret = 0;
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_SegmentoMinimoPorPlanta");
            sqlcommand.Parameters.AddWithValue("@EmpID", this.Empid);
            sqlcommand.Parameters.AddWithValue("@IdPlanta", this.IdPlanta);
            DataTable dt = cn.Execute(sqlcommand);
            if (dt.Rows[0]["Segmento"] != null && dt.Rows[0]["Segmento"].ToString() != "")
                ret = Convert.ToInt16(dt.Rows[0]["Segmento"]);

            return ret;
        }

        internal double TraerSegmentoMinimoPorBoca()
        {
            short ret = 0;
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_SegmentoMinimoPorTipoBoca");
            sqlcommand.Parameters.AddWithValue("@Empid", this.Empid);
            sqlcommand.Parameters.AddWithValue("@IdPlanta", this.IdPlanta);
            sqlcommand.Parameters.AddWithValue("@IdTipoBoca", this.IdTipoBoca);
            DataTable dt = cn.Execute(sqlcommand);
            if (dt.Rows[0]["Segmento"] != null && dt.Rows[0]["Segmento"].ToString() != "")
                ret = Convert.ToInt16(dt.Rows[0]["Segmento"]);

            return ret;
        }        

    }
}
