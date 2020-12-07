using System;
using System.Collections.Generic;
using System.Text;

namespace Damas.Models
{
    public class NodoDama: Nodo
    {
        public char[] Estado { get; private set; }
        int[][] lineas;
        public NodoDama()
        {
            Estado = new[]
            {
                ' ', ' ', 'O',
                'O', 'X', ' ',
                'X', ' ', ' '
            };

            lineas = new int[8][];
            lineas[0] = new int[] { 0, 1, 2 };
            lineas[1] = new int[] { 3, 4, 5 };
            lineas[2] = new int[] { 6, 7, 8 };
            lineas[3] = new int[] { 0, 3, 6 };
            lineas[4] = new int[] { 1, 4, 7 };
            lineas[5] = new int[] { 2, 5, 8 };
            lineas[6] = new int[] { 0, 4, 8 };
            lineas[7] = new int[] { 6, 4, 2 };
            // Si usamos un vector para representar el 'tablero' del gato,
            // nuestro vector, con las posiciones 0, 1, 2, 3, 4, 5, 6, 7,
            // sería una representación unidimensional de las posiciones que se
            // verían así:
            //
            // 0 1 2
            // 3 4 5
            // 6 7 8
            //
            // Así, las lineas horizontales serían obvias (0, 1, 2; 3, 4, 5 y 5, 6, 7)
            // las verticales
            // 
            //           0 1 2
            //           3 4 5
            //           5 6 7
            //           ^ ^ ^
            //           | | | 
            // 0, 3, 5 --  | | Estas son las verticales
            // 1, 4, 6 ----  |
            // 2, 5, 7 ------
            //
            // mientras que las dos diagonales son: 0, 4, 7 y 2, 4, 5
        }


        /// <summary>
        /// Determina si alguien ganó
        /// </summary>
        /// <param name="XO">Debe contener 'X' u 'O'</param>
        /// <returns></returns>
        public bool Gano(char XO)
        {
            foreach (var linea in lineas)
            {
                if (Estado[linea[0]] == XO &&
                    Estado[linea[1]] == XO &&
                    Estado[linea[2]] == XO)
                {
                    return true;
                }
            }
            return false;
        }


        public override void CalcularValor()
        {

            foreach (var linea in lineas)
            {
                if (Estado[linea[0]] == 'O' && Estado[linea[1]] == 'O' && Estado[linea[2]] == 'O')
                {
                    Valor = -9;
                    return;
                }
                if (Estado[linea[0]] == 'X' && Estado[linea[1]] == 'X' && Estado[linea[2]] == 'X')
                {
                    Valor = 9;
                    return;
                }
                if (Estado[linea[0]] != 'O' && Estado[linea[1]] != 'O' && Estado[linea[2]] != 'O')
                {
                    Valor++;
                }
                if (Estado[linea[0]] != 'X' && Estado[linea[1]] != 'X' && Estado[linea[2]] != 'X')
                {
                    Valor--;
                }
            }

        }

        public override void GenerarHijos(bool EsTurnoPC)
        {
            char c = EsTurnoPC ? 'X' : 'O';
            for (int i = 0; i < Estado.Length; i++)
            {
                if (Estado[i] == ' ')
                {
                    if (Hijos == null)
                    {
                        Hijos = new List<Nodo>();
                    }
                    NodoDama n = new NodoDama();
                    n.Estado = (char[])Estado.Clone();
                    n.Estado[i] = c;

                    Hijos.Add(n);


                }
            }
        }

        public override bool EsTerminal()
        {
            return Gano('X') || Gano('O');
        }

        public override string ToString()
        {
            return Valor.ToString();
        }
    }
}
