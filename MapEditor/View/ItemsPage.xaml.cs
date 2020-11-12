using MapEditor.Model;
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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace MapEditor.View
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ItemsPage : Page
    {
        public ItemsPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lsvItems.ItemsSource = Item.getItems();
            itemInventory.item = Item.getItem(0);
            itemInventory.MostrarDadesFormulari(Item.getItems()[0]);
            lsvItems.SelectedIndex = 0;
        }

        private void lsvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item itemSeleccionat = Item.getItem(lsvItems.SelectedIndex);
            itemInventory.item = itemSeleccionat;
            itemInventory.MostrarDadesFormulari(itemSeleccionat);
        }
    }
}
