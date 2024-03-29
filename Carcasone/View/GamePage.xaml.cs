﻿using Carcasone.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Drawing;
using System.Diagnostics;

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
        ObservableCollection<FitxaMapa> fitxesTauler = new ObservableCollection<FitxaMapa>();
        ObservableCollection<FitxaMapa> fitxesLliures = new ObservableCollection<FitxaMapa>();
        ObservableCollection<UIFitxaMapa> uiFitxesColocades = null;
        UIFitxaMapa fitxaClicada;
        private static Random rnd;
        private int playerQueJuga = -1;
        private const int COLUM_ROW = 10;



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
            playerQueJuga = generarRandom(1, 4);

            for (int i = 0; i < COLUM_ROW; i++)
            {
                ColumnDefinition colum = new ColumnDefinition();
                RowDefinition row = new RowDefinition();

                grdUis.ColumnDefinitions.Add(colum);
                grdUis.RowDefinitions.Add(row);
            }

            Grid.SetColumn(uiFitxaMapaStarting, COLUM_ROW / 2);
            Grid.SetRow(uiFitxaMapaStarting, COLUM_ROW / 2);

            uiFitxaMapaStarting.LaFitxaMapa = FitxaMapa.fitxaMapaStarting();
            for (int i = 0; i < FitxaMapa.getFitxesMapa().Count; i++)
            {
                fitxesLliures.Add(FitxaMapa.getFitxesMapa()[i]);
            }
            fitxesTauler.Add(FitxaMapa.fitxaMapaStarting());
            fitxesLliures.Remove(FitxaMapa.fitxaMapaStarting());


            uiNextFitxaMapa.PerColocar = null;
            uiFitxaMapaStarting.PerColocar = null;
            jugarRonda();
        }

        private void jugarRonda()
        {
            //while (fitxesLliures.Count > 0)

            jugadorJugaRonda();

            activarDesactivarRotacioFitxaMapa(false);
            seguentFitxaAColocar();

        }

        private void seguentFitxaAColocar()
        {
            int i = 0;
            if (fitxesTauler.Count < FitxaMapa.getFitxesMapa().Count)
            {
                //busco la seguent fitxa a colocar
                while (i < FitxaMapa.getFitxesMapa().Count && fitxesTauler.Contains(FitxaMapa.getFitxesMapa()[i]))
                {
                    i++;
                }

                if (!fitxesTauler.Contains(FitxaMapa.getFitxesMapa()[i]))
                {
                    uiNextFitxaMapa.LaFitxaMapa = FitxaMapa.getFitxesMapa()[i];
                    uiNextFitxaMapa.Player = playerQueJuga;
                }
                activarDesactivarRotacioFitxaMapa(true);
                OnEsPotColocarLaFitxa(uiNextFitxaMapa.LaFitxaMapa);
            }
        }

        private void activarDesactivarRotacioFitxaMapa(bool activar)
        {
            btnRotateLeft.IsEnabled = activar;
            btnRotateRight.IsEnabled = activar;
        }

        private void jugadorJugaRonda()
        {
            GestionarUIPlayerQueComenca(uiPlayer1);
            GestionarUIPlayerQueComenca(uiPlayer2);
            GestionarUIPlayerQueComenca(uiPlayer3);
            GestionarUIPlayerQueComenca(uiPlayer4);
        }

        private void GestionarUIPlayerQueComenca(UIPlayer uip)
        {
            if (Int32.Parse((String)uip.Tag) == playerQueJuga)
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
                activarDesactivarRotacioFitxaMapa(false);
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

        private void btnOkRules_Click(object sender, RoutedEventArgs e)
        {
            canviEstat(Estat.GAME);
        }

        private void rdbPlayers_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            qtatPlayers = Int32.Parse((String)rb.Tag);
        }

        private void btnRotateLeft_Click(object sender, RoutedEventArgs e)
        {
            NetejarGridUISFitxaMapa();
            rotarFitxaMapa(-1);
        }

        private void btnRotateRight_Click(object sender, RoutedEventArgs e)
        {
            NetejarGridUISFitxaMapa();
            rotarFitxaMapa(1);
        }

        private void rotarFitxaMapa(int rotacio)
        {
            SideType[] sides = new SideType[4];

            sides[0] = uiNextFitxaMapa.LaFitxaMapa.Sides[((0 - rotacio) % 4 == -1) ? 3 : (0 - rotacio) % 4];
            sides[1] = uiNextFitxaMapa.LaFitxaMapa.Sides[((1 - rotacio) % 4 == -1) ? 3 : (1 - rotacio) % 4];
            sides[2] = uiNextFitxaMapa.LaFitxaMapa.Sides[((2 - rotacio) % 4 == -1) ? 3 : (2 - rotacio) % 4];
            sides[3] = uiNextFitxaMapa.LaFitxaMapa.Sides[((3 - rotacio) % 4 == -1) ? 3 : (3 - rotacio) % 4];
            uiNextFitxaMapa.LaFitxaMapa.Sides = sides;

            rotacio = rotacio * 90;
            uiNextFitxaMapa.Rotacio = uiNextFitxaMapa.LaFitxaMapa.Rotacio + rotacio;


            //COMPROBAR A ON ES POT COLOCAR
            OnEsPotColocarLaFitxa(uiNextFitxaMapa.LaFitxaMapa);
            
        }

        private void OnEsPotColocarLaFitxa(FitxaMapa uiNextFitxaMapa)
        {
            uiNextFitxaMapa.ElNino = new Nino((PosType)(-1), playerQueJuga);
            FitxaMapa fitxaMapa = new FitxaMapa(uiNextFitxaMapa);
            fitxaMapa = uiNextFitxaMapa;
            
            uiFitxesColocades = new ObservableCollection<UIFitxaMapa>();
            uiFitxesColocades.Add(uiFitxaMapaStarting);

            NetejarGridUISFitxaMapa();

            for (int i = 0; i < fitxesTauler.Count; i++)
            {
                for (int j = 0; j < fitxesTauler[i].PosOcupada.Length; j++)
                {

                    if (fitxesTauler[i].PosMapa.X == 0 && j == 3) break;
                    if (fitxesTauler[i].PosMapa.X == COLUM_ROW && j == 1) break;
                    if (fitxesTauler[i].PosMapa.Y == 0 && j == 0) break;
                    if (fitxesTauler[i].PosMapa.Y == COLUM_ROW && j == 2) break;

                    if (fitxesTauler[i].PosOcupada[j] == PosFitxaMapaType.POS_LLIURE)
                    {
                        int k = (j + 2) % 4;

                        if (fitxesTauler[i].Sides[j] == fitxaMapa.Sides[k])
                        {
                            UIFitxaMapa ui = new UIFitxaMapa();
                            PosibleColocacioUiFitxa(fitxaMapa, ui, i, j);
                            grdUis.Children.Add(ui);
                            Grid.SetColumn(ui, ui.LaFitxaMapa.PosMapa.X);
                            Grid.SetRow(ui, ui.LaFitxaMapa.PosMapa.Y);
                            ui.Click += UIFitxaMapa_Click;
                        }
                    }
                }
            }
        }
               

        private void PosibleColocacioUiFitxa(FitxaMapa fitxaMapa, UIFitxaMapa ui, int i, int j)
        {
            int x = 0;
            int y = 0;

            if (j == 0) y = -1;
            if (j == 1) x = 1;
            if (j == 2) y = 1;
            if (j == 3) x = -1;

            fitxaMapa.PosMapa = new Point(fitxesTauler[i].PosMapa.X + x, fitxesTauler[i].PosMapa.Y + y);
            ui.LaFitxaMapa = fitxaMapa;
            ui.PerColocar = true;
            ui.Rotacio = fitxaMapa.Rotacio;
            ui.Player = playerQueJuga;
        }

        private void activarDesactivarRotacio(Boolean actiu)
        {
            btnRotateLeft.IsEnabled = actiu;
            btnRotateRight.IsEnabled = actiu;
        }

        private void UIFitxaMapa_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(((UIFitxaMapa)sender).LaFitxaMapa.PosMapa);

            fitxaClicada = (UIFitxaMapa)sender;
            activarDesactivarRotacio(false);

            btnCancel.Visibility = Visibility.Visible;
            btnOk.Visibility = Visibility.Visible;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NetejarGridUISFitxaMapa();

            activarDesactivarRotacio(true);

            btnCancel.Visibility = Visibility.Collapsed;
            btnOk.Visibility = Visibility.Collapsed;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            fitxesTauler.Add(fitxaClicada.LaFitxaMapa);
            uiFitxesColocades.Add(fitxaClicada);
            NetejarGridUISFitxaMapa();

            btnCancel.Visibility = Visibility.Collapsed;
            btnOk.Visibility = Visibility.Collapsed;

            seguentFitxaAColocar();
        }

        private void NetejarGridUISFitxaMapa()
        {
            if (uiFitxesColocades != null) {
                int i = 0;
                while (i < grdUis.Children.Count)
                {
                    if (!uiFitxesColocades.Contains(grdUis.Children[i]))
                    {
                        grdUis.Children.RemoveAt(i);
                    }
                    else
                    { 
                        i++; 
                    }
                }
            }
        }
    }
}
