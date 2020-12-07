using System;
using System.Collections.Generic;
using System.Text;

namespace Damas.Models
{
    public abstract class Nodo
    {
        public int minimax(int profundidad, bool maximizandoJugador)
        {
            if (profundidad == 0 || EsTerminal())
            {
                CalcularValor();
                return Valor;
            }
            if (maximizandoJugador)
            {
                GenerarHijos(maximizandoJugador);
                Valor = int.MinValue;
                foreach (var h in Hijos)
                {
                    Valor = Math.Max(Valor, h.minimax(profundidad - 1, false));
                }
                return Valor;
            }
            else
            {
                GenerarHijos(maximizandoJugador);
                Valor = int.MaxValue;
                foreach (var h in Hijos)
                {
                    Valor = Math.Min(Valor, h.minimax(profundidad - 1, true));
                }
                return Valor;
            }
        }

        public int Valor { get; protected set; }
        public abstract void CalcularValor();
        public List<Nodo> Hijos { get; protected set; }
        public abstract void GenerarHijos(bool EsTurnoPC);
        public abstract bool EsTerminal();
    }
}
