using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    public partial class TrnProveedores
    {
        
        public string prov_id { get; set; }
        
        public string emp_id { get; set; }

        public string nombre { get; set; }

        private IConfiguration Configuration;

        public TrnProveedores()
        {
        }

        public TrnProveedores(IConfiguration configuracion)
        {
            this.Configuration = configuracion;
        }

        public List<TrnProveedores> TraerProveedoresPorEmpID()
        {
            List<TrnProveedores> proveedores = new List<TrnProveedores>();            
            Conexion cn = new Conexion(Configuration);
            SqlCommand sqlcommand = cn.GetCommand("TRN_TraerProveedoresPorEmpID");
            
            sqlcommand.Parameters.AddWithValue("@EmpID", this.emp_id);            
            DataTable dt = cn.Execute(sqlcommand);
            proveedores = cn.ConvertDataTable<TrnProveedores>(dt);

            if (proveedores.Count > 0)
                return proveedores;
            else
                return new List<TrnProveedores>();
        }

    }
}
