using System;
using System.Collections.Generic;

namespace Turnos.Models
{
    public partial class TrnBocaTipo
    {
        public TrnBocaTipo()
        {
            TrnBoca = new HashSet<TrnBoca>();
        }

        public byte IdTipoBoca { get; set; }
        public string Empid { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<TrnBoca> TrnBoca { get; set; }
    }
}
