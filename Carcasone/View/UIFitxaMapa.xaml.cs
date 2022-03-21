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

        public event EventHandler Click;

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
            if (LaFitxaMapa != null)
            {
                imgFitxaMapa.Source = new BitmapImage(new Uri(LaFitxaMapa.Fitxa.ImagePath));

                btnNinoTop.Visibility = Visibility.Collapsed;
                btnNinoRight.Visibility = Visibility.Collapsed;
                btnNinoLeft.Visibility = Visibility.Collapsed;
                btnNinoBottom.Visibility = Visibility.Collapsed;

                if (PerColocar == false)
                {
                    btnUI.IsEnabled = false;
                    grdColocada.Visibility = Visibility.Collapsed;
                    if (LaFitxaMapa.ElNino != null)
                    {
                        imgNino.Source = new BitmapImage(new Uri(LaFitxaMapa.ElNino.ImagePath));
                    }
                }
                else if (PerColocar == true)
                {
                    btnUI.IsEnabled = true;
                    grdColocada.Visibility = Visibility.Visible;
                    imgNino.Source = null;
                    
                }
                else
                {
                    btnUI.IsEnabled = false;
                    grdColocada.Visibility = Visibility.Collapsed;
                    imgNino.Source = null;
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
            get { return (Boolean?)GetValue(PerColocarProperty); }
            set { SetValue(PerColocarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PerColocar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PerColocarProperty =
            DependencyProperty.Register("PerColocar", typeof(Boolean?), typeof(UIFitxaMapa), new PropertyMetadata(true, LaFitxaMapaChangedCallback_static));




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

        private void colorcarAlNino(PosType pos)
        {
            btnNinoTop.Visibility = Visibility.Collapsed;
            btnNinoRight.Visibility = Visibility.Collapsed;
            btnNinoLeft.Visibility = Visibility.Collapsed;
            btnNinoBottom.Visibility = Visibility.Collapsed;
            if (pos == PosType.TOP) {
                imgNino.VerticalAlignment = VerticalAlignment.Top;
                imgNino.HorizontalAlignment = HorizontalAlignment.Center;
            } 
            else if (pos == PosType.RIGHT) {
                imgNino.VerticalAlignment = VerticalAlignment.Center;
                imgNino.HorizontalAlignment = HorizontalAlignment.Right;
            } 
            else if (pos == PosType.BOTTOM) {
                imgNino.VerticalAlignment = VerticalAlignment.Bottom;
                imgNino.HorizontalAlignment = HorizontalAlignment.Center;
            } 
            else if (pos == PosType.LEFT) {
                imgNino.VerticalAlignment = VerticalAlignment.Center;
                imgNino.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }

        private void btnNino_Click(object sender, RoutedEventArgs e)
        {
            btnUI.IsEnabled = false;
            imgNino.Source = new BitmapImage(new Uri(LaFitxaMapa.ElNino.ImagePath));
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
            
            if (LaFitxaMapa.ElNino != null)
            {
                LaFitxaMapa.ElNino.Pos = posClicada;
                colorcarAlNino(LaFitxaMapa.ElNino.Pos);
            }
        }

        private void btnUI_Click(object sender, RoutedEventArgs e)
        {
            imgNino.Source = null;
            grdColocada.Visibility = Visibility.Collapsed;
            btnNinoTop.Visibility = Visibility.Visible;
            btnNinoRight.Visibility = Visibility.Visible;
            btnNinoLeft.Visibility = Visibility.Visible;
            btnNinoBottom.Visibility = Visibility.Visible;
            Click?.Invoke(this, new EventArgs());
        }


    }


}
