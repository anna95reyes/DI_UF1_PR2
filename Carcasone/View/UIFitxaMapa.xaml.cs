using Carcasone.Model;
using System;
using System.Collections.Generic;
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
                imgNino.Source = imatgeNino(LaFitxaMapa.ElNino.Player);
                colorcarAlNino(LaFitxaMapa.ElNino.Pos);
            }
            
        }



        public bool PerColocar
        {
            get { return (bool)GetValue(ColocadaProperty); }
            set { SetValue(ColocadaProperty, value); }
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
            if (PerColocar)
            {
                grdColocada.Visibility = Visibility.Visible;
            } 
            else
            {
                grdColocada.Visibility = Visibility.Collapsed;
            }
        }

        private ImageSource imatgeNino(int player)
        {
            BitmapImage bmi = new BitmapImage();
            if (player == 1)
            {
                bmi = new BitmapImage(new Uri("ms-appx:///Assets/meeple/blue_meeple.png"));
            }
            else if (player == 2)
            {
                bmi = new BitmapImage(new Uri("ms-appx:///Assets/meeple/red_meeple.png"));
            }
            else if (player == 3)
            {
                bmi = new BitmapImage(new Uri("ms-appx:///Assets/meeple/green_meeple.png"));
            }
            else if (player == 4)
            {
                bmi = new BitmapImage(new Uri("ms-appx:///Assets/meeple/yellow_meeple.png"));
            }
            return bmi;
        }

        private void colorcarAlNino(PosType pos)
        {
            switch (pos)
            {
                case PosType.TOP:
                    imgNino.VerticalAlignment = VerticalAlignment.Top;
                    imgNino.HorizontalAlignment = HorizontalAlignment.Center;
                    break;
                case PosType.RIGHT:
                    imgNino.VerticalAlignment = VerticalAlignment.Center;
                    imgNino.HorizontalAlignment = HorizontalAlignment.Right;
                    break;
                case PosType.BOTTOM:
                    imgNino.VerticalAlignment = VerticalAlignment.Bottom;
                    imgNino.HorizontalAlignment = HorizontalAlignment.Center;
                    break;
                case PosType.LEFT:
                    imgNino.VerticalAlignment = VerticalAlignment.Center;
                    imgNino.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                default:
                    break;
            }
        }
    }
}
