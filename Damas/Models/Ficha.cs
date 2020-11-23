using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Damas.Models
{
    public class Ficha
    {
        public Frame FichaJuego { get; set; }
        public bool EsDama { get; set; }
        public bool FichaDelJugador { get; set; }
        public bool Eliminada { get; set; }
    }
}
