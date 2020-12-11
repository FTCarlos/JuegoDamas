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
using System.Windows.Threading;

namespace Damas
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DamasGame game = new DamasGame();
        DispatcherTimer timer = new DispatcherTimer();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game.JuegoIniciado += Game_JuegoIniciado;
            game.JuegoRealizado += Game_JuegoRealizado;
            game.JuegoFinalizado += Game_JuegoFinalizado;
            timer.Interval = TimeSpan.FromSeconds(2.5);
            timer.Tick += Timer_Tick;
            game.TurnoCambio += Game_TurnoCambio;
            game.IniciarJuego();
        }

        private void Game_JuegoFinalizado()
        {
            VerificarGanador();
        }

        private void Game_TurnoCambio()
        {
            if (game.Turno == 1)
                timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            game.JugarComputadora();
            timer.Stop();
        }

        private void Game_JuegoRealizado(int xLast, int yLast, int x, int y, int jugador, List<Point> points)
        {
            var ficha = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == yLast && Grid.GetRow(x.FichaJuego) == xLast && x.Eliminada == false);
            Grid.SetColumn(ficha.FichaJuego, y);
            Grid.SetRow(ficha.FichaJuego, x);
            grdTablero.Children.Remove(ficha.FichaJuego);
            grdTablero.Children.Add(ficha.FichaJuego);

            if (points != null && points.Count() > 0)
            {
                foreach (var fichaEliminada in points)
                {
                    var fichaE = FichasEnJuego.FirstOrDefault(x => Grid.GetColumn(x.FichaJuego) == fichaEliminada.Y && Grid.GetRow(x.FichaJuego) == fichaEliminada.X && x.Eliminada == false);
                    grdTablero.Children.Remove(fichaE.FichaJuego);
                    fichaE.Eliminada = true;
                }
            }

            if (jugador == -1)
                this.Title = "Turno de la IA";
            else
                this.Title = "Turno del jugador";
        }

        private void Game_JuegoIniciado()
        {
            IniciarTablero();
        }

        const int tamañoTablero = 8;
        public List<Ficha> FichasEnJuego { get; set; } = new List<Ficha>();

        private void VerificarGanador()
        {
            if (FichasEnJuego.Where(x => x.FichaDelJugador == false).All(x => x.Eliminada == true))
            {
                MessageBox.Show("Jugador Ganador", "Ganador");
            }
            else if (FichasEnJuego.Where(x => x.FichaDelJugador == true).All(x => x.Eliminada == true))
            {
                MessageBox.Show("IA Ganador", "Ganador");
            }
        }

        private void IniciarTablero()
        {
            Tablero tablero = new Tablero();

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

                    //if (cuadro.Background == Brushes.Black && (row <= 1 || row > tamañoTablero - 3))
                    if (cuadro.Background == Brushes.Black && (row <= 2 || row > tamañoTablero - 4))
                    {
                        Frame fichaBtn = new Frame();
                        fichaBtn.Background = Brushes.Transparent;
                        Image ficha = new Image();
                        ficha.Stretch = Stretch.Uniform;
                        Thickness thick = new Thickness();
                        thick.Top = thick.Right = thick.Bottom = thick.Left = 5;
                        ficha.Margin = thick;
                        //ficha.Source = row <= 1 ? new BitmapImage(new Uri("IA.png", UriKind.Relative)) : new BitmapImage(new Uri("jugador.png", UriKind.Relative));
                        ficha.Source = row <= 2 ? new BitmapImage(new Uri("IA.png", UriKind.Relative)) : new BitmapImage(new Uri("jugador.png", UriKind.Relative));
                        fichaBtn.Content = ficha;
                        Grid.SetColumn(fichaBtn, column);
                        Grid.SetRow(fichaBtn, row);

                        fichaBtn.MouseDown += FichaBtn_MouseDown;
                        grdTablero.Children.Add(fichaBtn);
                        //FichasEnJuego.Add(new Ficha() { FichaJuego = fichaBtn, EsDama = false, FichaDelJugador = row <= 1 ? false : true });
                        FichasEnJuego.Add(new Ficha() { FichaJuego = fichaBtn, EsDama = false, FichaDelJugador = row <= 2 ? false : true });
                    }
                }
        }

        private void FichaBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ficha = (Frame)sender;;

            if (fichaBtn != null)
                fichaBtn.Opacity = 1;

            if (ficha != null && FichasEnJuego.Any(x => x.FichaDelJugador == true && Grid.GetColumn(x.FichaJuego) == Grid.GetColumn(ficha) && Grid.GetRow(x.FichaJuego) == Grid.GetRow(ficha)))
            {
                fichaBtn = ficha;
                fichaBtn.Opacity = .5;
            }
        }

        Frame fichaBtn;
        private void Cuadro_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (fichaBtn != null)
            {
                var grdTabla = (Grid)sender;
                MoverFicha(grdTabla);
                fichaBtn.Opacity = 1;
                VerificarGanador();
            }
        }

        void MoverFicha(Grid grdTabla)
        {
            var tablaColumn = Grid.GetColumn(grdTabla);
            var tablaRow = Grid.GetRow(grdTabla);

            var fichaColumn = Grid.GetColumn(fichaBtn);
            var fichaRow = Grid.GetRow(fichaBtn);
            var ficha = FichasEnJuego.First(x => Grid.GetColumn(x.FichaJuego) == fichaColumn && Grid.GetRow(x.FichaJuego) == fichaRow && x.Eliminada == false);

            game.JugarUsuario(fichaRow, fichaColumn, tablaRow, tablaColumn);
        }
    }
}
