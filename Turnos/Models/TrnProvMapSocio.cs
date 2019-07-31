using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace Turnos.Models
{
    [Table("ProvMapSocio")]
    public partial class TrnProvMapSocio
    {        
        [Key]
        public string Prov_Id { get; set; }
        [Key]
        public string Emp_Id { get; set; }              

    }
}
