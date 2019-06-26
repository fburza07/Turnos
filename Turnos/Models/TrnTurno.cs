﻿using System;
using System.Collections.Generic;

namespace Turnos.Models
{
    public partial class TrnTurno
    {
        public int EventId { get; set; }
        public string Empid { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
    }
}