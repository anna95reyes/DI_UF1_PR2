using MapEditor.View;
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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace MapEditor
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            frmPrincipal.Navigate(typeof(ItemsPage), this);
        }

        private void nvwBarraNavegacio_ItemInvoked(
            Microsoft.UI.Xaml.Controls.NavigationView sender, 
            Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            Type t = typeof(ItemsPage);
            object parametre = null;
            switch (args.InvokedItemContainer.Tag.ToString())
            {
                case "items":
                    t = typeof(ItemsPage);
                    parametre = this;
                    break;
                case "maps":
                    t = typeof(MapsPage);
                    parametre = this;
                    break;
            }
            frmPrincipal.Navigate(t, parametre);


        }
    }
}
