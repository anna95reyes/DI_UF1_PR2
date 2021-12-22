using Carcasone.Model;
using System;
using System.Collections.Generic;
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
            canviEstat(Estat.GAME);
            uiFitxaMapaStarting.LaFitxaMapa = FitxaMapa.getFitxesMapa()[0];
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
            canviEstat(Estat.GAME);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            canviEstat(Estat.GAME);
        }

    }
}
