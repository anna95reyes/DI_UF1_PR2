using MapEditor.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
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

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace MapEditor.View
{
    public sealed partial class UIItemInventory : UserControl
    {
        public UIItemInventory()
        {
            this.InitializeComponent();
        }




        public Item item
        {
            get { return (Item)GetValue(itemProperty); }
            set { SetValue(itemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty itemProperty =
            DependencyProperty.Register("item", typeof(Item), typeof(UIItemInventory), new PropertyMetadata(new Item()));


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

        private void deixarFormulariEnBlanc()
        {
            txtFilePath.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            brdItem.Child = null;
            brdItem.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        public void MostrarDadesFormulari(Item it)
        {
            txtName.Text = it.Name;
            txtDescription.Text = it.Desc;
            brdItem.Child = null;
            brdItem.BorderBrush = new SolidColorBrush(Colors.Transparent);
            crearImatgeAmbBorde(it.ImageSource);
        }


        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            deixarFormulariEnBlanc();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "" && txtDescription.Text == "")
            {
                Item.getItems().Add(new Item(txtName.Text, txtDescription.Text));
            } else
            {

            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
