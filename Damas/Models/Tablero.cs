using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

enum Jugador { jugador1 = -1, computadora = 1};

namespace Damas.Models
{
    public class Movimiento
    {
        public int Value { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
        public int XLast { get; set; }
        public int YLast { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Tablero
    {
        public sbyte[,] Tabla { get; set; } = new sbyte[8, 8];
        public const int Valor = 0;

        public int CalcularValor(int jugador)
        {
            int suma = 0;
            int reina = Math.Abs(jugador) + 1;
            if (jugador == -1)
                reina = -1 * reina;

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (Tabla[i, j] == jugador)
                        suma = suma + 1;

                    if (Tabla[i, j] == reina)
                        suma = suma + 100;
                }

            if (jugador == -1)
                suma = -1 * suma;

            return suma;
        }

        public int Ganador
        {
            get
            {
                bool usuario = true, comp = true;
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (Tabla[i, j] == -1 || Tabla[i, j] == -2)
                            usuario = false;

                        else if (Tabla[i, j] == 1 || Tabla[i, j] == 2)
                            comp = false;
                    }
                if (comp == true)
                    return -1;

                if (usuario == true)
                    return 1;

                return 0;
            }
        }

        public void checarC(ref List<Point> p, ref List<Movimiento> movimientos, int inic, int inicialj, int i, int j, int jugador, int rei)
        {
            bool x = false;
            bool ba = false;
            if (i < 6 && j < 6 && i >= 0 && j > 1)
                if ((Tabla[i + 1, j + 1] == (-jugador) || Tabla[i + 1, j + 1] == -rei) && Tabla[i + 2, j + 2] == 0)
                {
                    p.Add(new Point(i + 1, j + 1));
                    if (i + 2 < 8 || j + 2 < 8)
                    {
                        checarC(ref p, ref movimientos, inic, inicialj, i + 2, j + 2, jugador, rei);
                        x = true;
                    }
                }

            if (i < 6 && j > 1 && j < 8 && i > 1)
                if ((Tabla[i + 1, j - 1] == (-jugador) || Tabla[i + 1, j - 1] == -rei) && Tabla[i + 2, j - 2] == 0)
                {
                    p.Add(new Point(i + 1, j - 1));

                    checarC(ref p, ref movimientos, inic, inicialj, i + 2, j - 2, jugador, rei); ba = true;
                }

            if (ba == false && x == false)
                movimientos.Add(new Movimiento
                {
                    XLast = inic,
                    YLast = inicialj,
                    X = i,
                    Y = j,
                    Points = p
                });
        }

        public void checarCArr(ref List<Point> p, ref List<Movimiento> movimientos  , int inic, int inicialj, int i, int j, int jugador, int c)
        {
            bool x = false;
            bool ba = false;
            if (i > 1 && j < 6 && j > 1 && i < 8)//arr der
                if ((Tabla[i - 1, j + 1] == -jugador || Tabla[i - 1, j + 1] == -c) && Tabla[i - 2, j + 2] == 0)
                {
                    x = true;
                    p.Add(new Point(i - 1, j + 1));
                    x = true;
                    checarCArr(ref p, ref movimientos, inic, inicialj, i - 2, j + 2, jugador, c);
                }

            if (i > 1 && j > 1 && i < 8 && j < 8)//arr izq
                if ((Tabla[i - 1, j - 1] == -jugador || Tabla[i - 1, j - 1] == -c) && Tabla[i - 2, j - 2] == 0)
                {
                    p.Add(new Point(i - 1, j - 1));
                    ba = true;
                    checarCArr(ref p, ref movimientos, inic, inicialj, i - 2, j - 2, jugador, c);
                }

            if (ba == false && x == false)
                movimientos.Add(new Movimiento
                {
                    XLast = inic,
                    YLast = inicialj,
                    X = i,
                    Y = j,
                    Points = p
                });
        }

        public void checarTodos(ref List<Point> p, ref List<Movimiento> movimientos, int inic, int inicialj, int i, int j, int jugador, int b, int c)
        {
            bool ai = false;
            bool ad = false, abi = false, abd = false;
            if (b != 3)
                if (i < 6 && j > 1 && i >= 0 && j < 8) //ab izq
                    if ((Tabla[i + 1, j - 1] == (-jugador) || Tabla[i + 1, j - 1] == -c) && Tabla[i + 2, j - 2] == 0)
                    {
                        p.Add(new Point(i + 1, j - 1)); abi = true;
                        checarTodos(ref p, ref movimientos, inic, inicialj, i + 2, j - 2, jugador, 0, c);
                    }

            if (b != 1)
                if (i > 1 && j > 1 && i < 8 && j < 8)//arr izq
                    if ((Tabla[i - 1, j - 1] == -jugador || Tabla[i - 1, j - 1] == -c) && Tabla[i - 2, j - 2] == 0)
                    {
                        p.Add(new Point(i - 1, j - 1)); ai = true;

                        checarTodos(ref p, ref movimientos, inic, inicialj, i - 2, j - 2, jugador, 2, c);
                    }

            if (b != 2)
                if (i < 6 && j < 6 && i >= 0 && j >= 0)//abajo der
                    if ((Tabla[i + 1, j + 1] == (-jugador) || Tabla[i + 1, j + 1] == -c) && Tabla[i + 2, j + 2] == 0)
                    {
                        p.Add(new Point(i + 1, j + 1));
                        checarTodos(ref p, ref movimientos, inic, inicialj, i + 2, j + 2, jugador, 1, c);
                        abd = true;
                    }

            if (b != 0)
                if (i > 1 && j < 6 && i < 8 && j >= 0)//arr der
                    if ((Tabla[i - 1, j + 1] == (-jugador) || Tabla[i - 1, j + 1] == -c) && Tabla[i - 2, j + 2] == 0)
                    {
                        p.Add(new Point(i - 1, j + 1)); ad = true;
                        checarTodos(ref p, ref movimientos, inic, inicialj, i - 2, j + 2, jugador, 3, c);

                    }

            if (ai == false && abi == false && abd == false && ad == false)
                movimientos.Add(new Movimiento
                {
                    XLast = inic,
                    YLast = inicialj,
                    X = i,
                    Y = j,
                    Points = p
                });

        }

        public List<Movimiento> Movimientos(int jugador)
        {
            List<Movimiento> lista = new List<Movimiento>();
            List<Point> des;

            int reina = 0;
            reina = Math.Abs(jugador) + 1;
            if (jugador == -1)
                reina = -reina;

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if ((Tabla[i, j] == jugador && jugador == 1) || Tabla[i, j] == reina)
                    {
                        des = new List<Point>();
                        if (j < 6 && i < 6)
                            if (((Tabla[i + 1, j + 1] == (-jugador) || Tabla[i + 1, j + 1] == -reina)) && Tabla[i + 2, j + 2] == 0)
                            {
                                des.Add(new Point(i + 1, j + 1));

                                if (Tabla[i, j] == reina)
                                    checarTodos(ref des, ref lista, i, j, i + 2, j + 2, jugador, 1, reina);
                                else
                                    checarC(ref des, ref lista, i, j, i + 2, j + 2, jugador, reina);

                            }

                        if (i < 6 && j > 1)
                        {
                            des = new List<Point>();
                            if ((Tabla[i + 1, j - 1] == (-jugador) || Tabla[i + 1, j - 1] == -reina) && Tabla[i + 2, j - 2] == 0)
                            {
                                des.Add(new Point(i + 1, j - 1));

                                if (Tabla[i, j] == reina)
                                    checarTodos(ref des, ref lista, i, j, i + 2, j - 2, jugador, 0, reina);
                                else
                                    checarC(ref des, ref lista, i, j, i + 2, j - 2, jugador, reina);
                            }

                        }
                    }
                    if (Tabla[i, j] == reina || (jugador == -1 && Tabla[i, j] == jugador))
                    {
                        if (i > 1 && j < 6)
                        {
                            des = new List<Point>();
                            if ((Tabla[i - 1, j + 1] == -jugador || Tabla[i - 1, j + 1] == -reina) && Tabla[i - 2, j + 2] == 0)
                            {
                                des.Add(new Point(i - 1, j + 1));

                                if (Tabla[i, j] == reina)
                                    checarTodos(ref des, ref lista, i, j, i - 2, j + 2, jugador, 3, reina);
                                else
                                    checarCArr(ref des, ref lista, i, j, i - 2, j + 2, jugador, reina);
                            }
                        }

                        if (i > 1 && j > 1)
                        {
                            des = new List<Point>();
                            if ((Tabla[i - 1, j - 1] == -jugador || Tabla[i - 1, j - 1] == -reina) && Tabla[i - 2, j - 2] == 0)
                            {
                                des.Add(new Point(i - 1, j - 1));

                                if (Tabla[i, j] == reina)
                                    checarTodos(ref des, ref lista, i, j, i - 2, j - 2, jugador, 2, reina);
                                else
                                    checarCArr(ref des, ref lista, i, j, i - 2, j - 2, jugador, reina);
                            }
                        }
                    }
                }

            if (lista.Count > 0)
                return lista;

            if (jugador == 1)
            {
                lista = Mov(jugador);
                return lista;
            }
            return lista;
        }

        public List<Movimiento> Mov(int jugador)
        {
            int reina = 0;
            reina = Math.Abs(jugador) + 1;

            if (jugador == -1)
                reina = -reina;

            List<Movimiento> lista = new List<Movimiento>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((Tabla[i, j] == jugador && jugador == 1) || Tabla[i, j] == reina)
                    {

                        if (i < 7 && j < 7)
                            if (Tabla[i + 1, j + 1] == 0)
                                lista.Add(new Movimiento()
                                {
                                    X = i + 1,
                                    Y = j + 1,
                                    XLast = i,
                                    YLast = j
                                });

                        if (j > 0 && i < 7)
                            if (Tabla[i + 1, j - 1] == 0)
                                lista.Add(new Movimiento()
                                {
                                    X = i + 1,
                                    Y = j - 1,
                                    XLast = i,
                                    YLast = j
                                });
                    }

                    if (Tabla[i, j] == reina || (Tabla[i, j] == jugador && jugador == -1))
                    {
                        if (i > 0 && j < 7)
                            if (Tabla[i - 1, j + 1] == 0)
                                lista.Add(new Movimiento()
                                {
                                    X = i - 1,
                                    Y = j + 1,
                                    XLast = i,
                                    YLast = j
                                });

                        if (i > 0 && j > 0)
                            if (Tabla[i - 1, j - 1] == 0)
                                lista.Add(new Movimiento()
                                {
                                    X = i - 1,
                                    Y = j - 1,
                                    XLast = i,
                                    YLast = j
                                });
                    }

                }
            }
            return lista;
        }

        public void Nuevo()
        {
            Tabla = new sbyte[8, 8];

            bool bandera = true;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {

                    if (bandera == true)
                    {
                        Tabla[i, j] = 3;
                        bandera = false;
                    }
                    else
                    {
                        if (i < 3)
                            Tabla[i, j] = 1;
                        else if (i > 4)
                            Tabla[i, j] = -1;

                        bandera = true;
                    }
                    if (j == 7)
                        bandera = !bandera;
                }
        }

        public void jugar(int xanterior, int yanterior, int x, int y, int jugador, List<Point> des)
        {
            if (des != null)
                foreach (Point item in des)
                    Tabla[(int)item.X, (int)item.Y] = 0;

            Tabla[x, y] = Tabla[xanterior, yanterior];

            if (Tabla[xanterior, yanterior] == -1 && x == 0)
                Tabla[x, y] = -2;

            if (Tabla[xanterior, yanterior] == 1 && x == 7)
                Tabla[x, y] = 2;

            Tabla[xanterior, yanterior] = 0;
        }
    }
}
