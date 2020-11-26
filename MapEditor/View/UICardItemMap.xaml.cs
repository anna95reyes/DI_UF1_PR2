using MapEditor.Model;
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

namespace MapEditor.View
{
    public sealed partial class UICardItemMap : UserControl
    {
        public UICardItemMap()
        {
            this.InitializeComponent();
        }

        // "propdp" i tabulacio 2 cops



        public MapItem mapItem
        {
            get { return (MapItem)GetValue(mapItemProperty); }
            set { SetValue(mapItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for mapItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty mapItemProperty =
            DependencyProperty.Register("mapItem", typeof(MapItem), typeof(UICardItemMap), 
                new PropertyMetadata(null, mapItemChangedCallbackStatic));

        private static void mapItemChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UICardItemMap m = (UICardItemMap)d;
            m.mapItemChangedCallback(e);
        }

        private void mapItemChangedCallback(DependencyPropertyChangedEventArgs e)
        {
            if (mapItem != null) {
                BitmapImage invisible = new BitmapImage(new Uri("ms-appx:///Assets/invisible.png"));
                Image img = new Image();
                txbAmount.Text = "X" + mapItem.Amount;
                txbAmount.FontSize = 20;
                if (mapItem.IsHidden)
                {
                    imgHidden.Source = img.Source = invisible;
                }
                imgHidden.Height = 30;
            }
        }
    }
}
