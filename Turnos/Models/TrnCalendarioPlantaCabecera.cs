using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Turnos.Models
{
    public class TrnCalendarioPlantaCabecera
    {
        [Key]
        public int IdCalendarioPlanta { get; set; }
        public string Empid { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<TrnBoca> TrnBoca { get; set; }

    }
}
