using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Damas.Models
{
    public class DamasGame
    {
        public Tablero Tablero { get; set; } = new Tablero();
        public int Turno { get; set; } = 0;
            
        public event Action<string> Mensaje;
        public event Action JuegoIniciado;
        public event Action JuegoFinalizado;
        public event Action JugadorPerdio;
        public event Action JugadorGano;
        public event Action TurnoCambio;
        public event Action<int, int, int> Convertir;
        public event Action<int, int, int, int, int, int> Comer;
        public event Action<int, int, int, int, int, List<Point>> JuegoRealizado;

        public void FinalizarJuego()
        {
            Turno = 0;
            if (JuegoFinalizado != null)
                JuegoFinalizado();
        }

        public void IniciarJuego()
        {
            Tablero = new Tablero();
            Tablero.Nuevo();
            Turno = -1;

            if (JuegoIniciado != null)
                JuegoIniciado();
        }

        public void CambiarTurno()
        {
            if (Turno != 0)
            {
                if (Turno == -1)
                    Turno = 1;
                else
                    Turno = -1;

                if (TurnoCambio != null)
                    TurnoCambio();
            }
        }

        public void VerificarEstado()
        {
            int x = Tablero.Ganador;
            if (x == -1)
            {
                if (JugadorGano != null)
                    JugadorGano();

                FinalizarJuego();
            }
            else if (x == 1)
            {
                if (JugadorPerdio != null)
                    JugadorPerdio();

                FinalizarJuego();
            }
        }

        public void JugarUsuario(int xant, int yant, int x, int y)
        {
            VerificarEstado();

            IList<Movimiento> m = Tablero.Movimientos(-1);
            bool ban = false;
            if (m.Count > 0)
            {
                foreach (Movimiento item in m)
                    if (item.X == x && item.Y == y && item.XLast == xant && item.YLast == yant)
                    {
                        JuegoRealizado(xant, yant, x, y, -1, item.Points);
                        Tablero.jugar(xant, yant, x, y, -1, item.Points);

                        CambiarTurno(); ban = true;
                        if (x == 0)
                            if (Convertir != null)
                                Convertir(x, y, -1);

                        return;
                    }

                if (ban == false)
                    if (Mensaje != null)
                        Mensaje("Tienes que comer"); return;
            }
            else
            {
                List<Movimiento> mo = Tablero.Mov(-1);
                ban = false;
                foreach (Movimiento item in mo)
                    if (item.X == x && item.Y == y && item.XLast == xant && item.YLast == yant)
                    {
                        JuegoRealizado(xant, yant, x, y, -1, item.Points);
                        Tablero.jugar(xant, yant, x, y, -1, item.Points);

                        if (x == 0)
                            if (Convertir != null)
                                Convertir(x, y, -1);

                        CambiarTurno(); ban = true; return;
                    }

                if (Mensaje != null)
                    Mensaje("Movimiento invalido");
            }
        }

        public void JugarComputadora()
        {
            VerificarEstado();

            if (Tablero.Movimientos(1).Count > 0)
            {
                Movimiento m = Minimax(Tablero, 6);
                Tablero.jugar(m.XLast, m.YLast, m.X, m.Y, 1, m.Points);

                if (JuegoRealizado != null)
                    JuegoRealizado(m.XLast, m.YLast, m.X, m.Y, 1, m.Points);

                VerificarEstado();

                if (m.X == 7)
                    Convertir(m.X, m.Y, 1);
            }

            CambiarTurno();
        }

        public Movimiento Minimax(Tablero nodo, int altura)
        {
            Movimiento mejor = new Movimiento();
            mejor.Value = int.MinValue;
            foreach (Movimiento mov in nodo.Movimientos(1))
            {
                Tablero clon = new Tablero();
                clon.Tabla = (sbyte[,])nodo.Tabla.Clone();
                clon.jugar(mov.XLast, mov.YLast, mov.X, mov.Y, 1, mov.Points);
                Movimiento tmp = Min(clon, altura, 0);
                if (tmp.Value > mejor.Value)
                {
                    mejor.Value = tmp.Value;
                    mejor.X = mov.X;
                    mejor.Y = mov.Y;
                    mejor.XLast = mov.XLast;
                    mejor.YLast = mov.YLast;
                    mejor.Points = mov.Points;
                }
            }
            return mejor;
        }

        public Movimiento Min(Tablero nodo, int altura, int distancia)
        {
            if (altura == distancia || nodo.Ganador != 0)
            {
                Movimiento m = new Movimiento();
                m.Value = nodo.CalcularValor(-1);
                return m;
            }
            else
            {
                Movimiento menor = new Movimiento();
                menor.Value = int.MaxValue;
                List<Movimiento> m = nodo.Movimientos(-1);

                if (m.Count == 0)
                    m = nodo.Mov(-1);

                foreach (Movimiento mov in m)
                {
                    Tablero clon = new Tablero();
                    clon.Tabla = (sbyte[,])nodo.Tabla.Clone();
                    clon.jugar(mov.XLast, mov.YLast, mov.X, mov.Y, -1, mov.Points);
                    Movimiento tmp = Max(clon, altura, distancia + 1);
                    if (tmp.Value < menor.Value)
                    {
                        menor.Value = tmp.Value;
                        menor.X = mov.X;
                        menor.Y = mov.Y;
                        menor.XLast = mov.XLast;
                        menor.YLast = mov.YLast;
                        menor.Points = mov.Points;
                    }
                }
                return menor;
            }
        }

        public Movimiento Max(Tablero nodo, int altura, int distancia)
        {
            if (altura == distancia || nodo.Ganador != 0)
            {
                Movimiento m = new Movimiento();
                m.Value = nodo.CalcularValor(1);
                return m;
            }
            else
            {
                Movimiento mayor = new Movimiento();
                mayor.Value = int.MinValue;
                foreach (Movimiento mov in nodo.Movimientos(1))
                {
                    Tablero clon = new Tablero();
                    clon.Tabla = (sbyte[,])nodo.Tabla.Clone();
                    clon.jugar(mov.XLast, mov.YLast, mov.X, mov.Y, 1, mov.Points);
                    Movimiento tmp = Min(clon, altura, distancia + 1);
                    if (tmp.Value > mayor.Value)
                    {
                        mayor.Value = tmp.Value;
                        mayor.X = (int)mov.X;
                        mayor.Y = (int)mov.Y;
                        mayor.XLast = mov.XLast;
                        mayor.YLast = mov.YLast;
                        mayor.Points = mov.Points;
                    }
                }
                return mayor;
            }
        }
    }
}
