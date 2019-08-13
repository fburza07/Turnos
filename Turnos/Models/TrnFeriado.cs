using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    [Table("TRN_Feriados")]
    public partial class TrnFeriado
    {
        [Key]
        public int EventID { get; set; }
        public int IdCalendarioFeriado { get; set; }
        public string Empid { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
        private IConfiguration Configuration;

        public TrnFeriado()
        {
        }

        public TrnFeriado(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }

        public bool EsFeriado(string empID, DateTime fdesde, DateTime? fhasta)
        {
            bool esFeriado = false;
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_VerificarFeriado");
            sqlcommand.Parameters.AddWithValue("@EmpID", empID);
            sqlcommand.Parameters.AddWithValue("@FechaDesde", fdesde);
            sqlcommand.Parameters.AddWithValue("@FechaHasta", fhasta);
            DataTable dt = cn.Execute(sqlcommand);
            if (dt.Rows[0]["esFeriado"].ToString() == "1")
                esFeriado = true;

            return esFeriado;
        }
    }
}
