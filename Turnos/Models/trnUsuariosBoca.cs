using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    public partial class TrnUsuariosBoca
    {
        
        public string user_id { get; set; }
        [Key]
        public string user_name { get; set; } //codigo para relacion con boca

        public string nombre { get; set; }

        public string apellido { get; set; }

        public string eMail { get; set; }

        public int perfil { get; set; }

        public string datosCompletos {
            get
            {
                return nombre + " " + apellido + " - " + eMail;
            }  }

        public virtual ICollection<TrnBoca> TrnBoca { get; set; }

        private IConfiguration Configuration;

        public TrnUsuariosBoca()
        {
        }

        public TrnUsuariosBoca(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }

        public List<TrnUsuariosBoca> TraerUsuariosPorEmpID()
        {
            List<TrnUsuariosBoca> usuarios = new List<TrnUsuariosBoca>();            
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_TraerProveedoresPorEmpID");
            
            sqlcommand.Parameters.AddWithValue("@EmpID", this.user_id);            
            DataTable dt = cn.Execute(sqlcommand);
            usuarios = cn.ConvertDataTable<TrnUsuariosBoca>(dt);

            if (usuarios.Count > 0)
                return usuarios;
            else
                return new List<TrnUsuariosBoca>();
        }

    }
}
