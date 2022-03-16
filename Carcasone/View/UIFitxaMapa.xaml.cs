using Carcasone.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class UIFitxaMapa : UserControl
    {
        public UIFitxaMapa()
        {
            this.InitializeComponent();
        }

        public FitxaMapa LaFitxaMapa
        {
            get { return (FitxaMapa)GetValue(LaFitxaMapaProperty); }
            set { SetValue(LaFitxaMapaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LaFitxaMapa.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LaFitxaMapaProperty =
            DependencyProperty.Register("LaFitxaMapa", typeof(FitxaMapa), typeof(UIFitxaMapa), new PropertyMetadata(null, LaFitxaMapaChangedCallback_static));

        private static void LaFitxaMapaChangedCallback_static(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFitxaMapa ufm = (UIFitxaMapa)d;
            ufm.LaFitxaMapaChangedCallback(d, e);
        }

        private void LaFitxaMapaChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            imgFitxaMapa.Source = new BitmapImage(new Uri(LaFitxaMapa.Fitxa.ImagePath));
            if (LaFitxaMapa.ElNino != null)
            {
                imgNinoTop.Source = new BitmapImage(new Uri(LaFitxaMapa.ElNino.ImagePath));
                imgNinoRight.Source = new BitmapImage(new Uri(LaFitxaMapa.ElNino.ImagePath));
                imgNinoLeft.Source = new BitmapImage(new Uri(LaFitxaMapa.ElNino.ImagePath));
                imgNinoBottom.Source = new BitmapImage(new Uri(LaFitxaMapa.ElNino.ImagePath));

                

                if (PerColocar == false)
                {
                    btnNinoTop.Visibility = Visibility.Visible;
                    btnNinoRight.Visibility = Visibility.Visible;
                    btnNinoLeft.Visibility = Visibility.Visible;
                    btnNinoBottom.Visibility = Visibility.Visible;
                    colorcarAlNino(LaFitxaMapa.ElNino.Pos);
                } 
                else
                {
                    btnNinoTop.Visibility = Visibility.Collapsed;
                    btnNinoRight.Visibility = Visibility.Collapsed;
                    btnNinoLeft.Visibility = Visibility.Collapsed;
                    btnNinoBottom.Visibility = Visibility.Collapsed;
                }
                
            }
        }

        public int Player
        {
            get { return (int)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Player.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register("Player", typeof(int), typeof(UIFitxaMapa), new PropertyMetadata(0));



        public Boolean? PerColocar
        {
            get { return (Boolean)GetValue(ColocadaProperty); }
            set { SetValue(ColocadaProperty, value); }
        }



        public int Rotacio
        {
            get { return (int)GetValue(RotacioProperty); }
            set { SetValue(RotacioProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rotacio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotacioProperty =
            DependencyProperty.Register("Rotacio", typeof(int), typeof(UIFitxaMapa), new PropertyMetadata(0, RotacioChangedCallback_static));

        private static void RotacioChangedCallback_static(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFitxaMapa ufm = (UIFitxaMapa)d;
            ufm.RotacioChangedCallback(d, e);
        }

        private void RotacioChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RotateTransform rt = new RotateTransform();
            if (Rotacio != 0)
            {
                Rotacio = Rotacio % 360;
            }
            LaFitxaMapa.Rotacio = Rotacio;
            rt.Angle = LaFitxaMapa.Rotacio;
            rt.CenterX = grdImgFitxaMapa.Height / 2;
            rt.CenterY = grdImgFitxaMapa.Width / 2;
            grdImgFitxaMapa.RenderTransform = rt;
        }

        // Using a DependencyProperty as the backing store for Colocada.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColocadaProperty =
            DependencyProperty.Register("Colocada", typeof(bool), typeof(UIFitxaMapa), new PropertyMetadata(false, ColocadaChangedCallback_static));

        private static void ColocadaChangedCallback_static(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFitxaMapa ufm = (UIFitxaMapa)d;
            ufm.ColocadaChangedCallback(d,e);
        }

        private void ColocadaChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (PerColocar == true || PerColocar != null)
            {
                grdColocada.Visibility = Visibility.Visible;
            } 
            else
            {
                grdColocada.Visibility = Visibility.Collapsed;
            }
        }

        private void colorcarAlNino(PosType pos)
        {
            btnNinoTop.Visibility = Visibility.Collapsed;
            btnNinoRight.Visibility = Visibility.Collapsed;
            btnNinoLeft.Visibility = Visibility.Collapsed;
            btnNinoBottom.Visibility = Visibility.Collapsed;
            if (pos == PosType.TOP) {
                btnNinoTop.Visibility = Visibility.Visible;
            } 
            else if (pos == PosType.RIGHT) {
                    btnNinoRight.Visibility = Visibility.Visible;
            } 
            else if (pos == PosType.BOTTOM) {
                    btnNinoLeft.Visibility = Visibility.Visible;
            } 
            else if (pos == PosType.LEFT) {
                    btnNinoBottom.Visibility = Visibility.Visible;
            }
        }

        private void btnNino_Click(object sender, RoutedEventArgs e)
        {
            PosType posClicada = (PosType)(-1);
            if (((Button)sender).Name.Equals("btnNinoTop"))
            {
                posClicada = PosType.TOP;
            }
            else if (((Button)sender).Name.Equals("btnNinoRight"))
            {
                posClicada = PosType.RIGHT;
            }
            else if (((Button)sender).Name.Equals("btnNinoLeft"))
            {
                posClicada = PosType.LEFT;
            }
            else if (((Button)sender).Name.Equals("btnNinoBottom"))
            {
                posClicada = PosType.BOTTOM;
            }
            LaFitxaMapa.ElNino.Pos = posClicada;

            Debug.WriteLine("EL NINO: " + ((Button)sender).Name);
        }
    }
}
