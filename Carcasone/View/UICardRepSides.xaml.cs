using Carcasone.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
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
    public sealed partial class UICardRepSides : UserControl
    {
        public UICardRepSides()
        {
            this.InitializeComponent();
        }

        public Fitxa fitxa
        {
            get { return (Fitxa)GetValue(fitxaProperty); }
            set { SetValue(fitxaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for fitxa.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fitxaProperty =
            DependencyProperty.Register("fitxa", typeof(Fitxa), typeof(UICardRepSides), new PropertyMetadata(null, fitxaChangedCallbackStatic));

        private static void fitxaChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UICardRepSides crs = (UICardRepSides)d;
            crs.fitxaChangedCallback(d,e);
        }

        private void fitxaChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int qtatSides = fitxa.Sides.Length;
            bool teMonasteri = fitxa.IsMonastery;

            bool teCastle = teSides(qtatSides, SideType.CASTLE);

            bool tePath = teSides(qtatSides, SideType.PATH);

            colocacioSides(teMonasteri, teCastle, tePath);
        }
        
        private bool teSides (int qtatSides, SideType sides)
        {
            bool hiEs = false;

            for (int i = 0; i < qtatSides; i++)
            {
                if (fitxa.Sides[i].Equals(sides))
                {
                    hiEs = true;
                }
            }

            return hiEs;
        }

        private void colocacioSides(bool teMonasteri, bool teCastle, bool tePath)
        {
            String uriImgMonasteri = "ms-appx:///Assets/icons/icons8-church-60.png";
            String uriImgCastle = "ms-appx:///Assets/icons/icons8-castle-50.png";
            String uriImgPath = "ms-appx:///Assets/icons/icons8-path-50.png";

            BitmapImage btmImgFitxa = new BitmapImage(new Uri(fitxa.ImagePath));
            imgFitxa.Source = btmImgFitxa;

            txbRepFitxa.Text = "x" + fitxa.Repeticions.ToString();

            if (grdFitxa.Children.Count == 3) grdFitxa.Children.RemoveAt(2);
            if (grdFitxa.Children.Count == 4) grdFitxa.Children.RemoveAt(3);
            if (grdFitxa.Children.Count == 5) grdFitxa.Children.RemoveAt(4);

            if (teMonasteri && !teCastle && !tePath)
            {
                imatgeSide(uriImgMonasteri, 2);
            }
            else if (!teMonasteri && teCastle && !tePath)
            {
                imatgeSide(uriImgCastle, 2);
            }
            else if (!teMonasteri && !teCastle && tePath)
            {
                imatgeSide(uriImgPath, 2);
            }
            else if (teMonasteri && teCastle && !tePath)
            {
                imatgeSide(uriImgMonasteri, 2);
                imatgeSide(uriImgCastle, 3);
            }
            else if (!teMonasteri && teCastle && tePath)
            {
                imatgeSide(uriImgCastle, 2);
                imatgeSide(uriImgPath, 3);
            }
            else if (teMonasteri && !teCastle && tePath)
            {
                imatgeSide(uriImgMonasteri, 2);
                imatgeSide(uriImgPath, 3);
            }
            else if (teMonasteri && teCastle && tePath)
            {
                imatgeSide(uriImgMonasteri, 2);
                imatgeSide(uriImgCastle, 3);
                imatgeSide(uriImgPath, 4);
            }
        }

        private void imatgeSide(string uri, int pos)
        {
            Image img = new Image();
            BitmapImage btmImg = new BitmapImage(new Uri(uri));
            img.Source = btmImg;

            img.Margin = new Thickness(5);

            Grid.SetColumn(img, pos);
            grdFitxa.Children.Add(img);
        }

    }
}
