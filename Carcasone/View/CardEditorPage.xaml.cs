using Carcasone.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Carcasone.View
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CardEditorPage : Page
    {
        public String pageName = "Carcassone Card Editor";
        private static ObservableCollection<String> _extensions;

        public CardEditorPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txbTitlePage.Text = pageName;
            
            //Es el mateix que fer: lsvFitxes.ItemsSource = Fitxa.getFitxes(); pero per poder agrupar
            IEnumerable<IGrouping<String, Fitxa>> groups = from c in Fitxa.getFitxes() group c by c.Extens;
            this.cvs.Source = groups;

            cbxRepeticions.ItemsSource = new int[] { 1, 2, 3, 4 };
            cbxRepeticions.SelectedIndex = 0;
        }




        #region addExtensio, removeExtensio, getExtensions
        public Boolean addExtensio(String novaExtensio)
        {
            if (_extensions.Contains(novaExtensio))
            {
                return false;
            }
            _extensions.Add(novaExtensio);
            return true;
        }

        public Boolean removeExtensio(String extensio)
        {
            if (!_extensions.Contains(extensio))
            {
                return false;
            }
            _extensions.Remove(extensio);
            return true;
        }

        public static ObservableCollection<String> getExtensions()
        {
            if (_extensions == null)
            {
                _extensions = new ObservableCollection<String>();

                String basic = "Basic";
                String theDarkAges = "The dark ages";

                _extensions.Add(basic);
                _extensions.Add(theDarkAges);
            }
            return _extensions;
        }
        #endregion addExtensio, removeExtensio, getExtensions

    }
}
