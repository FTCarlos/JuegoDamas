using Damas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Damas
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IniciarTablero();
        }

        const int tamañoTablero = 6;
        public List<Ficha> FichasEnJuego { get; set; } = new List<Ficha>();

        private void IniciarTablero()
        {
            for (int i = 0; i < tamañoTablero; i++)
            {
                grdTablero.ColumnDefinitions.Add(new ColumnDefinition());
                grdTablero.RowDefinitions.Add(new RowDefinition());
            }

            for (int column = 0; column < tamañoTablero; column++)
                for (int row = 0; row < tamañoTablero; row++)
                {
                    var cuadro = new Grid();
                    cuadro.Background = row % 2 == 0 ? column % 2 == 0 ? Brushes.White : Brushes.Black : column % 2 == 0 ? Brushes.Black : Brushes.White;
                    Grid.SetColumn(cuadro, column);
                    Grid.SetRow(cuadro, row);
                    cuadro.MouseDown += Cuadro_MouseDown;
                    grdTablero.Children.Add(cuadro);

                    if (column == 2 && row == 3)
                    {
                        Frame fichaBtn = new Frame();
                        fichaBtn.Background = Brushes.Transparent;
                        Image ficha = new Image();
                        ficha.Stretch = Stretch.Uniform;
                        Thickness thick = new Thickness();
                        thick.Top = thick.Right = thick.Bottom = thick.Left = 5;
                        ficha.Margin = thick;
                        ficha.Source = new BitmapImage(new Uri("IA.png", UriKind.Relative));
                        fichaBtn.Content = ficha;
                        Grid.SetColumn(fichaBtn, column);
                        Grid.SetRow(fichaBtn, row);

                        fichaBtn.MouseDown += FichaBtn_MouseDown;
                        grdTablero.Children.Add(fichaBtn);
                        FichasEnJuego.Add(new Ficha() { FichaJuego = fichaBtn, EsDama = false, FichaDelJugador = false });
                    }

                    if (cuadro.Background == Brushes.Black && (row <= 1 || row > tamañoTablero - 3))
                    {
                        Frame fichaBtn = new Frame();
                        fichaBtn.Background = Brushes.Transparent;
                        Image ficha = new Image();
                        ficha.Stretch = Stretch.Uniform;
                        Thickness thick = new Thickness();
                        thick.Top = thick.Right = thick.Bottom = thick.Left = 5;
                        ficha.Margin = thick;
                        //ficha.Source = new BitmapImage(new Uri("jugador.png", UriKind.Relative));
                        ficha.Source = row <= 1 ? new BitmapImage(new Uri("IA.png", UriKind.Relative)) : new BitmapImage(new Uri("jugador.png", UriKind.Relative));
                        fichaBtn.Content = ficha;
                        Grid.SetColumn(fichaBtn, column);
                        Grid.SetRow(fichaBtn, row);

                        fichaBtn.MouseDown += FichaBtn_MouseDown;
                        grdTablero.Children.Add(fichaBtn);
                        FichasEnJuego.Add(new Ficha() { FichaJuego = fichaBtn, EsDama = false, FichaDelJugador = row <= 1 ? false : true });
                        #region
                        //Button fichaBtn = new Button();
                        //fichaBtn.Background = Brushes.Transparent;
                        //Image ficha = new Image();
                        //ficha.Stretch = Stretch.Uniform;
                        //Thickness thick = new Thickness();
                        //thick.Top = thick.Right = thick.Bottom = thick.Left = 5;
                        //ficha.Margin = thick;
                        ////ficha.Source = new BitmapImage(new Uri("jugador.png", UriKind.Relative));
                        //ficha.Source = row <= 1 ? new BitmapImage(new Uri("IA.png", UriKind.Relative)) : new BitmapImage(new Uri("jugador.png", UriKind.Relative));
                        //fichaBtn.Content = ficha;
                        //Grid.SetColumn(fichaBtn, column);
                        //Grid.SetRow(fichaBtn, row);
                        //grdTablero.Children.Add(fichaBtn);

                        //Image ficha = new Image();
                        //ficha.Stretch = Stretch.Uniform;
                        //Thickness thick = new Thickness();
                        //thick.Top = thick.Right = thick.Bottom = thick.Left = 5;
                        //ficha.Margin = thick;
                        ////ficha.Source = new BitmapImage(new Uri("jugador.png", UriKind.Relative));
                        //ficha.Source = row <=1 ? new BitmapImage(new Uri("IA.png", UriKind.Relative)) : new BitmapImage(new Uri("jugador.png", UriKind.Relative));
                        //Grid.SetColumn(ficha, column);
                        //Grid.SetRow(ficha, row);
                        //grdTablero.Children.Add(ficha);
                        #endregion
                    }
                }
        }

        private void Cuadro_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (fichaBtn != null)
            {
                var grdTabla = (Grid)sender;
                MoverFicha(grdTabla);
                VerificarGanador();
            }
        }

        private void VerificarGanador()
        {
            if (FichasEnJuego.Count(x => x.FichaDelJugador == true) >= 1 && FichasEnJuego.Count(x => x.FichaDelJugador == false) == 0)
            {
                MessageBox.Show("Jugador Ganador", "Ganador");
            }
            else if (FichasEnJuego.Count(x => x.FichaDelJugador == true) == 0 && FichasEnJuego.Count(x => x.FichaDelJugador == false) >= 1)
            {
                MessageBox.Show("IA Ganador", "Ganador");
            }
        }

        private void FichaBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
           fichaBtn = (Frame)sender;
        }

        bool jugadorTurno = true;
        Frame fichaBtn;
        void MoverFicha(Grid grdTabla)
        {
            var tablaColumn = Grid.GetColumn(grdTabla);
            var tablaRow = Grid.GetRow(grdTabla);

            var fichaColumn = Grid.GetColumn(fichaBtn);
            var fichaRow = Grid.GetRow(fichaBtn);
            var ficha = FichasEnJuego.First(x => Grid.GetColumn(x.FichaJuego) == fichaColumn && Grid.GetRow(x.FichaJuego) == fichaRow);

            if (ficha.FichaDelJugador) //Jugador
            {
                if ((tablaColumn == fichaColumn + 1 && tablaRow == fichaRow - 1) || (tablaColumn == fichaColumn - 1 && tablaRow == fichaRow - 1))
                {
                    Grid.SetColumn(fichaBtn, tablaColumn);
                    Grid.SetRow(fichaBtn, tablaRow);
                    grdTablero.Children.Remove(fichaBtn);
                    grdTablero.Children.Add(fichaBtn);
                    jugadorTurno = false;
                    if (tablaRow == 0)
                    {
                        FichasEnJuego.Remove(ficha);
                        ficha.EsDama = true;
                        FichasEnJuego.Add(ficha);
                    }

                }
                else if (tablaColumn == fichaColumn + 2 && tablaRow == fichaRow - 2)
                {
                    var fichaEnemiga = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaColumn + 1 && Grid.GetRow(x.FichaJuego) == fichaRow - 1);
                    if (fichaEnemiga != null && !fichaEnemiga.FichaDelJugador)
                    {
                        //Mover y eliminar la ficha enemiga
                        FichasEnJuego.Remove(fichaEnemiga);

                        Grid.SetColumn(fichaBtn, tablaColumn);
                        Grid.SetRow(fichaBtn, tablaRow);
                        grdTablero.Children.Remove(fichaBtn);
                        grdTablero.Children.Remove(fichaEnemiga.FichaJuego);
                        grdTablero.Children.Add(fichaBtn);

                        if (tablaRow == 0)
                        {
                            FichasEnJuego.Remove(ficha);
                            ficha.EsDama = true;
                            FichasEnJuego.Add(ficha);
                        }

                    }
                }
                else if ((tablaColumn == fichaColumn - 2 && tablaRow == fichaRow - 2))
                {
                    var fichaEnemiga = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaColumn - 1 && Grid.GetRow(x.FichaJuego) == fichaRow - 1);
                    if (fichaEnemiga != null && !fichaEnemiga.FichaDelJugador)
                    {
                        //Mover y eliminar la ficha enemiga
                        FichasEnJuego.Remove(fichaEnemiga);

                        Grid.SetColumn(fichaBtn, tablaColumn);
                        Grid.SetRow(fichaBtn, tablaRow);
                        grdTablero.Children.Remove(fichaBtn);
                        grdTablero.Children.Remove(fichaEnemiga.FichaJuego);
                        grdTablero.Children.Add(fichaBtn);

                        if (tablaRow == 0)
                        {
                            FichasEnJuego.Remove(ficha);
                            ficha.EsDama = true;
                            FichasEnJuego.Add(ficha);
                        }

                    }
                }
                else if (ficha.EsDama)
                {
                    if ((tablaColumn == fichaColumn + 1 && tablaRow == fichaRow + 1) || (tablaColumn == fichaColumn - 1 && tablaRow == fichaRow + 1))
                    {
                        Grid.SetColumn(fichaBtn, tablaColumn);
                        Grid.SetRow(fichaBtn, tablaRow);
                        grdTablero.Children.Remove(fichaBtn);
                        grdTablero.Children.Add(fichaBtn);
                        jugadorTurno = false;
                    }
                    else if (tablaColumn == fichaColumn + 2 && tablaRow == fichaRow + 2)
                    {
                        var fichaEnemiga = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaColumn + 1 && Grid.GetRow(x.FichaJuego) == fichaRow + 1);
                        if (fichaEnemiga != null && !fichaEnemiga.FichaDelJugador)
                        {
                            //Mover y eliminar la ficha enemiga
                            FichasEnJuego.Remove(fichaEnemiga);

                            Grid.SetColumn(fichaBtn, tablaColumn);
                            Grid.SetRow(fichaBtn, tablaRow);
                            grdTablero.Children.Remove(fichaBtn);
                            grdTablero.Children.Remove(fichaEnemiga.FichaJuego);
                            grdTablero.Children.Add(fichaBtn);

                            //grdTablero.Children.Remove();
                        }
                    }
                    else if ((tablaColumn == fichaColumn - 2 && tablaRow == fichaRow + 2))
                    {
                        var fichaEnemiga = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaColumn - 1 && Grid.GetRow(x.FichaJuego) == fichaRow + 1);
                        if (fichaEnemiga != null && !fichaEnemiga.FichaDelJugador)
                        {
                            //Mover y eliminar la ficha enemiga
                            FichasEnJuego.Remove(fichaEnemiga);

                            Grid.SetColumn(fichaBtn, tablaColumn);
                            Grid.SetRow(fichaBtn, tablaRow);
                            grdTablero.Children.Remove(fichaBtn);
                            grdTablero.Children.Remove(fichaEnemiga.FichaJuego);
                            grdTablero.Children.Add(fichaBtn);

                            //grdTablero.Children.Remove();
                        }
                    }
                }
            }
            else
            {
                if ((tablaColumn == fichaColumn + 1 && tablaRow == fichaRow + 1) || (tablaColumn == fichaColumn - 1 && tablaRow == fichaRow + 1))
                {
                    Grid.SetColumn(fichaBtn, tablaColumn);
                    Grid.SetRow(fichaBtn, tablaRow);
                    grdTablero.Children.Remove(fichaBtn);
                    grdTablero.Children.Add(fichaBtn);
                    jugadorTurno = true;
                    if (tablaRow == tamañoTablero - 1)
                    {
                        FichasEnJuego.Remove(ficha);
                        ficha.EsDama = true;
                        FichasEnJuego.Add(ficha);
                    }
                }
                else if (tablaColumn == fichaColumn + 2 && tablaRow == fichaRow + 2)
                {
                    var fichaEnemiga = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaColumn + 1 && Grid.GetRow(x.FichaJuego) == fichaRow + 1);
                    if (fichaEnemiga != null && fichaEnemiga.FichaDelJugador)
                    {
                        //Mover y eliminar la ficha enemiga
                        FichasEnJuego.Remove(fichaEnemiga);

                        Grid.SetColumn(fichaBtn, tablaColumn);
                        Grid.SetRow(fichaBtn, tablaRow);
                        grdTablero.Children.Remove(fichaBtn);
                        grdTablero.Children.Remove(fichaEnemiga.FichaJuego);
                        grdTablero.Children.Add(fichaBtn);

                        if (tablaRow == tamañoTablero - 1)
                        {
                            FichasEnJuego.Remove(ficha);
                            ficha.EsDama = true;
                            FichasEnJuego.Add(ficha);
                        }
                    }
                }
                else if ((tablaColumn == fichaColumn - 2 && tablaRow == fichaRow + 2))
                {
                    var fichaEnemiga = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaColumn - 1 && Grid.GetRow(x.FichaJuego) == fichaRow + 1);
                    if (fichaEnemiga != null && fichaEnemiga.FichaDelJugador)
                    {
                        //Mover y eliminar la ficha enemiga
                        FichasEnJuego.Remove(fichaEnemiga);

                        Grid.SetColumn(fichaBtn, tablaColumn);
                        Grid.SetRow(fichaBtn, tablaRow);
                        grdTablero.Children.Remove(fichaBtn);
                        grdTablero.Children.Remove(fichaEnemiga.FichaJuego);
                        grdTablero.Children.Add(fichaBtn);

                        if (tablaRow == tamañoTablero - 1)
                        {
                            FichasEnJuego.Remove(ficha);
                            ficha.EsDama = true;
                            FichasEnJuego.Add(ficha);
                        }
                    }
                }
                else if (ficha.EsDama)
                {
                    if ((tablaColumn == fichaColumn + 1 && tablaRow == fichaRow - 1) || (tablaColumn == fichaColumn - 1 && tablaRow == fichaRow - 1))
                    {
                        Grid.SetColumn(fichaBtn, tablaColumn);
                        Grid.SetRow(fichaBtn, tablaRow);
                        grdTablero.Children.Remove(fichaBtn);
                        grdTablero.Children.Add(fichaBtn);
                        jugadorTurno = true;
                    }
                    else if (tablaColumn == fichaColumn + 2 && tablaRow == fichaRow - 2)
                    {
                        var fichaEnemiga = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaColumn + 1 && Grid.GetRow(x.FichaJuego) == fichaRow - 1);
                        if (fichaEnemiga != null && fichaEnemiga.FichaDelJugador)
                        {
                            //Mover y eliminar la ficha enemiga
                            FichasEnJuego.Remove(fichaEnemiga);

                            Grid.SetColumn(fichaBtn, tablaColumn);
                            Grid.SetRow(fichaBtn, tablaRow);
                            grdTablero.Children.Remove(fichaBtn);
                            grdTablero.Children.Remove(fichaEnemiga.FichaJuego);
                            grdTablero.Children.Add(fichaBtn);

                            //grdTablero.Children.Remove();
                        }
                    }
                    else if ((tablaColumn == fichaColumn - 2 && tablaRow == fichaRow - 2))
                    {
                        var fichaEnemiga = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaColumn - 1 && Grid.GetRow(x.FichaJuego) == fichaRow - 1);
                        if (fichaEnemiga != null && fichaEnemiga.FichaDelJugador)
                        {
                            //Mover y eliminar la ficha enemiga
                            FichasEnJuego.Remove(fichaEnemiga);

                            Grid.SetColumn(fichaBtn, tablaColumn);
                            Grid.SetRow(fichaBtn, tablaRow);
                            grdTablero.Children.Remove(fichaBtn);
                            grdTablero.Children.Remove(fichaEnemiga.FichaJuego);
                            grdTablero.Children.Add(fichaBtn);

                            //grdTablero.Children.Remove();
                        }
                    }
                }
            }
        }
    }
}
