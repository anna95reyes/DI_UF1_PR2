using Carcasone.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Carcasone.View
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public String pageName = "Carcassone Game";
        public int qtatPlayers = 0;
        ObservableCollection<FitxaMapa> fitxesJugades = new ObservableCollection<FitxaMapa>();
        private static Random rnd;

        public enum Estat
        {
            NEWGAME,
            GAME,
            HELP
        }
        Estat estat = Estat.NEWGAME;

        public GamePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txbTitlePage.Text = pageName;
            canviEstat(Estat.NEWGAME);
        }

        public static int generarRandom(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue);
        }

        private void inicialitzarPartida()
        {
            rnd = new Random();
            uiFitxaMapaStarting.LaFitxaMapa = FitxaMapa.fitxaMapaStarting();
            fitxesJugades.Add(FitxaMapa.fitxaMapaStarting());
            jugarPartida();
        }

        private void jugarPartida()
        {
            int i = 0;
            int playerQueJuga = generarRandom(1, 4);
            jugadorComencaPartida(playerQueJuga);
            btnRotateLeft.IsEnabled = false;
            btnRotateRight.IsEnabled = false;

            if (fitxesJugades.Count < FitxaMapa.getFitxesMapa().Count)
            {
                //busco la seguent fitxa a colocar
                while (i < FitxaMapa.getFitxesMapa().Count && fitxesJugades.Contains(FitxaMapa.getFitxesMapa()[i]))
                {
                    i++;
                }

                if (!fitxesJugades.Contains(FitxaMapa.getFitxesMapa()[i]))
                {
                    uiNextFitxaMapa.LaFitxaMapa = FitxaMapa.getFitxesMapa()[i];
                    uiNextFitxaMapa.Player = playerQueJuga;
                }
                btnRotateLeft.IsEnabled = true;
                btnRotateRight.IsEnabled = true;

            }

            
        }

        private void jugadorComencaPartida(int jugadorAComencar)
        {
            GestionarUIPlayerQueComenca(jugadorAComencar, uiPlayer1);
            GestionarUIPlayerQueComenca(jugadorAComencar, uiPlayer2);
            GestionarUIPlayerQueComenca(jugadorAComencar, uiPlayer3);
            GestionarUIPlayerQueComenca(jugadorAComencar, uiPlayer4);
        }

        private void GestionarUIPlayerQueComenca(int jugadorAComencar, UIPlayer uip)
        {
            if (Int32.Parse((String)uip.Tag) == jugadorAComencar)
            {
                uip.PlayerActual = true;
            }
        }

        private void canviEstat(Estat estatNou)
        {
            estat = estatNou;
            if (estat == Estat.NEWGAME)
            {
                menuGame.Visibility = Visibility.Collapsed;
                gridNewGame.Visibility = Visibility.Visible;
                rvpGame.Visibility = menuGame.Visibility;
                grdHelp.Visibility = Visibility.Collapsed;
                grdPagina.Background = new SolidColorBrush(Colors.Transparent);

            }
            else if (estat == Estat.GAME)
            {
                menuGame.Visibility = Visibility.Visible;
                gridNewGame.Visibility = Visibility.Collapsed;
                rvpGame.Visibility = menuGame.Visibility;
                grdHelp.Visibility = Visibility.Collapsed;
                grdPagina.Background = new SolidColorBrush(Colors.LightGreen);
            }
            else if (estat == Estat.HELP)
            {
                menuGame.Visibility = Visibility.Collapsed;
                gridNewGame.Visibility = Visibility.Collapsed;
                rvpGame.Visibility = menuGame.Visibility;
                grdHelp.Visibility = Visibility.Visible;
                grdPagina.Background = new SolidColorBrush(Colors.LightGreen);
            }
        }

        private void menuRestartGame_Click(object sender, RoutedEventArgs e)
        {
            canviEstat(Estat.NEWGAME);
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            CloseApp();
        }

        private void menuCntlQ_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            CloseApp();
        }

        public void CloseApp()
        {
            Application.Current.Exit();
        }

        private void menuHelp_Click(object sender, RoutedEventArgs e)
        {
            canviEstat(Estat.HELP);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int qtatNino = 5;

            uiPlayer1.gestionarUIPlayer(1, qtatNino, false);
            uiPlayer2.gestionarUIPlayer(2, qtatNino, false);
            uiPlayer3.gestionarUIPlayer(3, qtatNino, false);
            uiPlayer4.gestionarUIPlayer(4, qtatNino, false);

            uiPlayer1.Visibility = Visibility.Collapsed;
            uiPlayer2.Visibility = Visibility.Collapsed;
            uiPlayer3.Visibility = Visibility.Collapsed;
            uiPlayer4.Visibility = Visibility.Collapsed;

            if (qtatPlayers == 2)
            {
                uiPlayer1.Visibility = Visibility.Visible;
                uiPlayer2.Visibility = Visibility.Visible;
            }
            else if (qtatPlayers == 3)
            {
                uiPlayer1.Visibility = Visibility.Visible;
                uiPlayer2.Visibility = Visibility.Visible;
                uiPlayer3.Visibility = Visibility.Visible;
            }
            if (qtatPlayers == 4)
            {
                uiPlayer1.Visibility = Visibility.Visible;
                uiPlayer2.Visibility = Visibility.Visible;
                uiPlayer3.Visibility = Visibility.Visible;
                uiPlayer4.Visibility = Visibility.Visible;
            }

            canviEstat(Estat.GAME);
            inicialitzarPartida();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            canviEstat(Estat.GAME);
        }

        private void rdbPlayers_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            qtatPlayers = Int32.Parse((String)rb.Tag);
        }
    }
}
