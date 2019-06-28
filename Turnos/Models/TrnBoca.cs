using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{
    public partial class TrnBoca
    {
        public int IdPlanta { get; set; }
        public string Empid { get; set; }
        public string BocaEntrega { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public short SegmentoCantMin { get; set; }
        public short SegmentoCantPalletMax { get; set; } 
        [Required]
        public int IdCalendarioPlanta { get; set; }        
        public int IdCalendarioFeriado { get; set; }
        public bool VerificaSobreposicionHoraria { get; set; }
        public short CantidadCitasSimultaneas { get; set; }
        public byte IdTipoBoca { get; set; }
        public string UsuarioResponsableBoca { get; set; }

        public virtual TrnBocaTipo IdTipoBocaNavigation { get; set; }
        public virtual TrnCalendarioFeriado TrnCalendarioFeriadoNavication { get; set; }
        public virtual TrnCalendarioPlanta TrnCalendarioplantaNavication { get; set; }
    }
}
