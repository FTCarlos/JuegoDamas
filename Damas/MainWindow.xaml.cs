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
                    grdTablero.Children.Add(cuadro);

                    if (cuadro.Background == Brushes.Black && (row <= 1 || row > tamañoTablero - 3))
                    {
                        Button fichaBtn = new Button();
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
                        grdTablero.Children.Add(fichaBtn);

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
                    }
                }
        }

    }
}
