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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace Carcasone.View
{
    public sealed partial class UIPlayer : UserControl
    {
        public UIPlayer()
        {
            this.InitializeComponent();
        }

        public Nino ElNino
        {
            get { return (Nino)GetValue(ElNinoProperty); }
            set { SetValue(ElNinoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElNino.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElNinoProperty =
            DependencyProperty.Register("ElNino", typeof(Nino), typeof(UIPlayer), new PropertyMetadata(null, ElNinoChangedCallback_static));

        public int QtatNinos
        {
            get { return (int)GetValue(qtatNinosProperty); }
            set { SetValue(qtatNinosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for qtatNinos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty qtatNinosProperty =
            DependencyProperty.Register("qtatNinos", typeof(int), typeof(UIPlayer), new PropertyMetadata(0, ElNinoChangedCallback_static));



        public bool PlayerActual
        {
            get { return (bool)GetValue(PlayerActualProperty); }
            set { SetValue(PlayerActualProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayerActual.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerActualProperty =
            DependencyProperty.Register("PlayerActual", typeof(bool), typeof(UIPlayer), new PropertyMetadata(false, PlayerActualChangedCallback_static));

        private static void ElNinoChangedCallback_static(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIPlayer uip = (UIPlayer)d;
            uip.ElNinoChangedCallback(d, e);
        }

        private void ElNinoChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (ElNino != null && QtatNinos != 0)
            {
                txtPlayers.Text = "Player " + ElNino.Player;
                for (int i = 0; i < QtatNinos; i++)
                {
                    skpNinoPlayer.Children.Add(afegirImatge(ElNino.ImagePath));
                }
                brdPlayerActual.BorderThickness = new Thickness(10);
            }
        }

        private static void PlayerActualChangedCallback_static(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIPlayer uip = (UIPlayer)d;
            uip.PlayerActualChangedCallback(d, e);
        }

        private void PlayerActualChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (PlayerActual)
            {
                brdPlayerActual.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                brdPlayerActual.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }

        private Image afegirImatge(string uriImatge)
        {
            Image img = new Image();
            BitmapImage btmImg = new BitmapImage(new Uri(uriImatge));
            img.Source = btmImg;
            img.Height = 30;
            img.Width = 30;
            return img;
        }

        internal void gestionarUIPlayer(int player, int qtatNino, bool playerActual)
        {
            ElNino = new Nino(player);
            QtatNinos = qtatNino;
            PlayerActual = playerActual;
        }
    }
}
