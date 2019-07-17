using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Turnos.Models
{
    [Table("TRN_Turno")]
    public class TrnTurno
    {
        [Key]
        public int EventID { get; set; }
        public string Empid { get; set; }
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

        private IConfiguration Configuration;

        public TrnTurno()
        {
        }

        public TrnTurno(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }

        public bool ExisteEvento(string empID,int eventID, int idBoca, int idTipoBoca, DateTime fdesde, DateTime? fhasta)
        {
            bool existeEvento = false;
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_VerificarEvento");
            sqlcommand.Parameters.AddWithValue("@EmpID", empID);
            sqlcommand.Parameters.AddWithValue("@EventID", eventID);
            sqlcommand.Parameters.AddWithValue("@IdBoca", idBoca);
            sqlcommand.Parameters.AddWithValue("@IdTipoBoca", idTipoBoca);
            sqlcommand.Parameters.AddWithValue("@FechaDesde", fdesde);
            sqlcommand.Parameters.AddWithValue("@FechaHasta", fhasta);
            DataTable dt = cn.Execute(sqlcommand);
            if (dt.Rows[0]["existeEvento"].ToString() == "1")
                existeEvento = true;

            return existeEvento;
        }
       
    }
}
