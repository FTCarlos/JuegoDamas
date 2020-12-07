using System;
using System.Collections.Generic;
using System.Text;

enum Jugador { jugador1 = -1, computadora = 1};

namespace Damas.Models
{
    public class Tablero
    {
        public Tablero()
        {
            Iniciar();
        }

        public int[,] Tabla { get; set; } = new int[6, 6];

        public void Iniciar()
        {
            for (int column = 0; column < 6; column++)
                for (int row = 0; row < 6; row++)
                {
                    if (row < 2)
                    {
                        Tabla[column, row] = 1;
                    }

                    if(row > 3)
                    {
                        Tabla[column, row] = -1;
                    }
                }
        }
    }
}
