using MapEditor.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
            lsvItems.SelectedIndex = 0;
            mostrarDadesFormulari(0);
        }

        private void lsvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvItems.SelectedIndex >= 0)
            {
                deixarFormulariEnBlanc();
                mostrarDadesFormulari(lsvItems.SelectedIndex);
            }
            
        }

        private void crearImatgeAmbBorde(BitmapImage bitmapImage)
        {
            Image img = new Image();
            img.Source = bitmapImage;
            brdItem.BorderBrush = new SolidColorBrush(Colors.Green);
            brdItem.BorderThickness = new Thickness(2);
            brdItem.Child = img;
            if (bitmapImage.UriSource != null)
            {
                txtFilePath.Text = bitmapImage.UriSource.LocalPath;
            }
            
        }

        private async void btnFile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fp = new FileOpenPicker();
            fp.FileTypeFilter.Add(".jpg");
            fp.FileTypeFilter.Add(".png");

            StorageFile sf = await fp.PickSingleFileAsync();
            if (sf != null)
            {
                // Cerca la carpeta de dades de l'aplicació, dins de ApplicationData
                var folder = ApplicationData.Current.LocalFolder;
                // Dins de la carpeta de dades, creem una nova carpeta "icons"
                var iconsFolder = await folder.CreateFolderAsync("icons", CreationCollisionOption.OpenIfExists);
                // Creem un nom usant la data i hora, de forma que no poguem repetir noms.
                string name = (DateTime.Now).ToString("yyyyMMddhhmmss") + "_" + sf.Name;
                // Copiar l'arxiu triat a la carpeta indicada, usant el nom que hem muntat
                StorageFile copiedFile = await sf.CopyAsync(iconsFolder, name);
                // Crear una imatge en memòria (BitmapImage) a partir de l'arxiu copiat a ApplicationData
                BitmapImage tmpBitmap = new BitmapImage(new Uri(copiedFile.Path));

                // ..... YOUR CODE HERE ...........
                crearImatgeAmbBorde(tmpBitmap);
            }
            validaDadesFormulari();
        }

        private void deixarFormulariEnBlanc()
        {
            txtFilePath.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            brdItem.Child = null;
            brdItem.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void mostrarDadesFormulari(int index)
        {
            Item item = Item.getItems()[index];
            txtName.Text = item.Name;
            txtDescription.Text = item.Desc;
            brdItem.Child = null;
            brdItem.BorderBrush = new SolidColorBrush(Colors.Green);
            crearImatgeAmbBorde(item.ImageSource);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Item.getItems().Add(new Item());
            lsvItems.SelectedIndex = Item.getItems().Count - 1;
            deixarFormulariEnBlanc();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lsvItems.SelectedIndex >= 0)
            {
                Image img = (Image)brdItem.Child;
                BitmapImage bmi = (BitmapImage)img.Source;
                Item itemSeleccionat = Item.getItems()[lsvItems.SelectedIndex];
                itemSeleccionat.Name = txtName.Text;
                itemSeleccionat.Desc = txtDescription.Text;
                itemSeleccionat.ImageSource = bmi;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lsvItems.SelectedIndex >= 0)
            {
                Item itemSeleccionat = Item.getItems()[lsvItems.SelectedIndex];
                Item.getItems().Remove(itemSeleccionat);
            }
        }

        private void validaDadesFormulari()
        {
            bool nomValid = false;
            bool DescValid = false;
            bool imgValid = false;
            if (!Item.ValidaName(txtName.Text))
            {
                if (txtName.Text.Length > 0)
                {
                    txtName.Background = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                txtName.Background = new SolidColorBrush(Colors.Transparent);
                nomValid = true;
            }
            if (!Item.ValidaDesc(txtDescription.Text))
            {
                if (txtDescription.Text.Length > 0)
                {
                    txtDescription.Background = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                txtDescription.Background = new SolidColorBrush(Colors.Transparent);
                DescValid = true;
            }
            if (txtFilePath.Text != "")
            {
                imgValid = true;
            }
            btnSave.IsEnabled = nomValid && DescValid && imgValid;
        }
        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            validaDadesFormulari();

        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            validaDadesFormulari();
        }
    }
}
