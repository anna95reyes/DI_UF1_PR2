using MapEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class MapsPage : Page
    {
        public MapsPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lsvItemsMap.ItemsSource = Map.getMap().MapItems;
            lsvItemsMap.SelectedIndex = 0;
            inicializeApp();
            enableButtonsSeleccioItem();
        }

        List<int> itemsAmount = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private void inicializeApp()
        {
            imgMap.Source = Map.getMap().ImageSource;
            colocarItemsDinsMapa();

            nudCellWidht.Value = Map.getMap().CellWidth;
            nudCellHeight.Value = Map.getMap().CellHeight;

            cboNewItem.ItemsSource = Item.getItems();

            cboAmountItemMap.ItemsSource = itemsAmount;
            cboAmountItemMap.SelectedIndex = 0;
        }

        private void colocarItemsDinsMapa()
        {
            for (int i = 0; i < Map.getMap().MapItems.Count; i++)
            {
                afegirItemsDinsMapa(i);
            }
        }

        private void afegirItemsDinsMapa(int mapItem)
        {
            //<view:UIItemOnTheMap x:Name="uiItemOnTheMap"/>
            UIItemOnTheMap uiItemOnTheMap = new UIItemOnTheMap();
            uiItemOnTheMap.mapItem = Map.getMap().MapItems[mapItem];

            //Afegir la UI al Canvas
            cnvItemOfMap.Children.Add(uiItemOnTheMap);

            //Colocar l'item al mapa
            Canvas.SetLeft(uiItemOnTheMap, Map.getMap().CellWidth * Map.getMap().MapItems[mapItem].X);
            Canvas.SetTop(uiItemOnTheMap, Map.getMap().CellHeight * Map.getMap().MapItems[mapItem].Y);
        }

        private void mostrarItemsDinsMapa()
        {
            for (int i = 0; i < Map.getMap().MapItems.Count; i++)
            {
                //<view:UIItemOnTheMap x:Name="uiItemOnTheMap"/>
                UIItemOnTheMap uiItemOnTheMap = new UIItemOnTheMap();
                uiItemOnTheMap.mapItem = Map.getMap().MapItems[i];

                //Colocar l'item al mapa
                Canvas.SetLeft(uiItemOnTheMap, Map.getMap().CellWidth * Map.getMap().MapItems[i].X);
                Canvas.SetTop(uiItemOnTheMap, Map.getMap().CellHeight * Map.getMap().MapItems[i].Y);
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
                imgMap.Source = tmpBitmap;
                Map.getMap().ImageSource = tmpBitmap;
                txtFile.Text = tmpBitmap.UriSource.LocalPath;
                //recolocarItemsMap();


            }
        }

        private void recolocarItemsMap()
        {
            Debug.WriteLine(Map.getMap().ImageSource.PixelWidth);
            Debug.WriteLine(Map.getMap().CellWidth);
            int maxX = Convert.ToInt32(Map.getMap().ImageSource.PixelWidth / Map.getMap().CellWidth);
            int maxY = Convert.ToInt32(Map.getMap().ImageSource.PixelHeight / Map.getMap().CellHeight);
            for (int i = 0; i < Map.getMap().MapItems.Count; i++)
            {
                if (Map.getMap().MapItems[i].X > maxX)
                {
                    Map.getMap().MapItems[i].X = maxX;
                    moureItemsEnMapa(i);
                }
                if (Map.getMap().MapItems[i].Y > maxY)
                {
                    Map.getMap().MapItems[i].Y = maxY;
                    moureItemsEnMapa(i);
                }
            }
        }

        private void activarBotoNewItem()
        {
            btnNewItem.IsEnabled = cboNewItem.SelectedIndex >= 0 && cboAmountItemMap.SelectedIndex >= 0;
        }


        private void cboNewItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarBotoNewItem();
            
        }

        private void cboAmountItemMap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activarBotoNewItem();
        }



        private void btnNewItem_Click(object sender, RoutedEventArgs e)
        {
            /*
             * Interpreto que l'item per defecte es coloca al 0,0 i es colocara al lloc on es desitji
             * a traves de les fletxes.
             */
            MapItem nouMapItem = new MapItem(Map.getMap(), Item.getItems()[cboNewItem.SelectedIndex], 0, 0,
                                             itemsAmount[cboAmountItemMap.SelectedIndex], ckbHidden.IsChecked.Value);
            Map.getMap().addItem(nouMapItem);

            afegirItemsDinsMapa(Map.getMap().MapItems.Count -1);
            //Map pMap, Item pItem, int pX, int pY, int pAmount, bool pIsHidden

            esborrarFormulariNewItem();
        }

        private void esborrarFormulariNewItem()
        {
            cboNewItem.SelectedIndex = -1;
            cboAmountItemMap.SelectedIndex = 0;
            ckbHidden.IsChecked = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lsvItemsMap.SelectedIndex >= 0)
            {
                cnvItemOfMap.Children.RemoveAt(lsvItemsMap.SelectedIndex);

                Map.getMap().removeItem(lsvItemsMap.SelectedIndex);

                mostrarItemsDinsMapa();
            }
        }

        private void moureItemsEnMapa(int seleccionat)
        {
            Canvas.SetLeft(cnvItemOfMap.Children[seleccionat], Map.getMap().CellWidth * Map.getMap().MapItems[seleccionat].X);
            Canvas.SetTop(cnvItemOfMap.Children[seleccionat], Map.getMap().CellHeight * Map.getMap().MapItems[seleccionat].Y);
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            
            int seleccionat = lsvItemsMap.SelectedIndex;
            if (seleccionat >= 0)
            {
                if (Map.getMap().MapItems[seleccionat].X > 0)
                {
                    Map.getMap().MapItems[seleccionat].X = Map.getMap().MapItems[seleccionat].X - 1;
                    moureItemsEnMapa(seleccionat);
                }  
            }
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            int seleccionat = lsvItemsMap.SelectedIndex;
            if (seleccionat >= 0)
            {
                if (Map.getMap().MapItems[seleccionat].X < Convert.ToInt32(Map.getMap().ImageSource.PixelWidth / Map.getMap().CellWidth))
                {
                    Map.getMap().MapItems[seleccionat].X = Map.getMap().MapItems[seleccionat].X + 1;
                    moureItemsEnMapa(seleccionat);
                }
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            int seleccionat = lsvItemsMap.SelectedIndex;
            if (seleccionat >= 0)
            {
                if (Map.getMap().MapItems[seleccionat].Y > 0)
                {
                    Map.getMap().MapItems[seleccionat].Y = Map.getMap().MapItems[seleccionat].Y - 1;
                    moureItemsEnMapa(seleccionat);
                }
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            int seleccionat = lsvItemsMap.SelectedIndex;
            if (seleccionat >= 0)
            {
                if (Map.getMap().MapItems[seleccionat].Y < Convert.ToInt32(Map.getMap().ImageSource.PixelHeight / Map.getMap().CellHeight))
                {
                    Map.getMap().MapItems[seleccionat].Y = Map.getMap().MapItems[seleccionat].Y + 1;
                    moureItemsEnMapa(seleccionat);
                }
            }
        }

        private void enableButtonsSeleccioItem()
        {
            Boolean actBtnL;
            Boolean actBtnR;
            Boolean actBtnU;
            Boolean actBtnDr;
            Boolean actBtnDlt;
            int seleccionat = lsvItemsMap.SelectedIndex;
            deseleccionarItemsMapa();
            if (seleccionat >= 0)
            {
                actBtnDlt = true;
                actBtnL = true;
                actBtnR = true;
                actBtnU = true;
                actBtnDr = true;
                if (cnvItemOfMap.Children.Count > 0)
                {
                    ((UIItemOnTheMap)cnvItemOfMap.Children[lsvItemsMap.SelectedIndex]).seleccionat = true;
                }
            } 
            else
            {
                actBtnDlt = false;
                actBtnL = false;
                actBtnR = false;
                actBtnU = false;
                actBtnDr = false;
            }
            btnDelete.IsEnabled = actBtnDlt;
            btnLeft.IsEnabled = actBtnL;
            btnRight.IsEnabled = actBtnR;
            btnUp.IsEnabled = actBtnU;
            btnDown.IsEnabled = actBtnDr;
        }

        private void deseleccionarItemsMapa()
        {
            for (int i = 0; i < cnvItemOfMap.Children.Count; i++)
            {
                ((UIItemOnTheMap)cnvItemOfMap.Children[i]).seleccionat = false;
            }
        }
        

        private void lsvItemsMap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            enableButtonsSeleccioItem();
            Debug.WriteLine(cnvItemOfMap.Children);
            if (cnvItemOfMap.Children.Count > 0)
            {
                //UIItemOnTheMap uiItemSeleccionat = (UIItemOnTheMap)cnvItemOfMap.Children[lsvItemsMap.SelectedIndex];
                //uiItemSeleccionat.seleccionat = true;
                mostrarItemsDinsMapa();
            }
            
        }
    }
    
}
