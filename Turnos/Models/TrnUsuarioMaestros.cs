using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    [Table("UsuarioMaestros")]
    public partial class TrnUsuarioMaestros
    {
        [Key]
        public string usr_Id { get; set; }
        public string nombre { get; set; }        

    }
}
