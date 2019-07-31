using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    [Table("UsuarioPlantas")]
    public partial class TrnUsuarioPlanta
    {
        [Key]
        public string User_Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }          

    }
}
