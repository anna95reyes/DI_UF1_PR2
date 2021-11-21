using Carcasone.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace Carcasone.View
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CardEditorPage : Page
    {
        public enum Estat
        {
            VIEW,
            MODIFICACIO,
            ALTA
        }

        public String pageName = "Carcassone Card Editor";
        private static ObservableCollection<String> _extensions;
        private ObservableCollection<Image> imgSides = null;
        Estat estat = Estat.VIEW;

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

            lsvExtensions.ItemsSource = CardEditorPage.getExtensions();
            lsvExtensions.SelectedIndex = 0;

            cbxSide0Fitxa.ItemsSource = llistaImatgesSides();
            cbxSide1Fitxa.ItemsSource = llistaImatgesSides();
            cbxSide2Fitxa.ItemsSource = llistaImatgesSides();
            cbxSide3Fitxa.ItemsSource = llistaImatgesSides();
            
            canviEstat(Estat.VIEW);
        }

        private void canviEstat(Estat estatNou)
        {
            estat = estatNou;
            if (estat == Estat.VIEW)
            {
                netejarFormulari();
                mostrarDadesFormulari();
                activarDesactivarEdicio(false);
            } else if (estat == Estat.MODIFICACIO)
            {
                activarDesactivarEdicio(true);
                btnCancelFormulari.IsEnabled = false;
                btnSaveFormulari.IsEnabled = false;
            }
        }

        private void mostrarDadesFormulari()
        {
            if (lsvFitxes.SelectedItem != null)
            {
                //Fitxa fitxaSeleccionada = (Fitxa)lsvFitxes.SelectedItem;
                Fitxa fitxaSeleccionada = Fitxa.getFitxes()[lsvFitxes.SelectedIndex];
                BitmapImage btmImgFitxaSeleccionada = new BitmapImage(new Uri(fitxaSeleccionada.ImagePath));
                
                cbxRepeticions.SelectedValue = fitxaSeleccionada.Repeticions;
                txtTitle.Text = fitxaSeleccionada.Title;
                lsvExtensions.SelectedValue = fitxaSeleccionada.Extens;
                imgFitxa.Source = btmImgFitxaSeleccionada;
                txtNotes.Text = fitxaSeleccionada.Notes;
                ckbExtraStartingTile.IsChecked = (Fitxa.IsStartingTile.getCodi() == fitxaSeleccionada.getCodi())? true : false;
                ckbExtraMonastery.IsChecked = fitxaSeleccionada.IsMonastery;
                imgSidesFitxa.Source = btmImgFitxaSeleccionada;
                cbxSide0Fitxa.SelectedIndex = sidesSeleccionat(fitxaSeleccionada, 0);
                cbxSide1Fitxa.SelectedIndex = sidesSeleccionat(fitxaSeleccionada, 1);
                cbxSide2Fitxa.SelectedIndex = sidesSeleccionat(fitxaSeleccionada, 2);
                cbxSide3Fitxa.SelectedIndex = sidesSeleccionat(fitxaSeleccionada, 3);
            }
        }

        private void netejarFormulari()
        {
            cbxRepeticions.SelectedIndex = 0;
            txtTitle.Text = " ";
            lsvExtensions.SelectedIndex = 0;
            imgFitxa.Source = null;
            txtNotes.Text = " ";
            ckbExtraStartingTile.IsChecked = false;
            ckbExtraMonastery.IsChecked = false;
            imgSidesFitxa.Source = null;
            cbxSide0Fitxa.SelectedIndex = 2;
            cbxSide1Fitxa.SelectedIndex = 2;
            cbxSide2Fitxa.SelectedIndex = 2;
            cbxSide3Fitxa.SelectedIndex = 2;
        }


        public int sidesSeleccionat(Fitxa fitxaSeleccionada, int pos)
        {
            if (fitxaSeleccionada.Sides[pos].Equals(SideType.PATH))
            {
                return 0;
            }
            else if (fitxaSeleccionada.Sides[pos].Equals(SideType.CASTLE))
            {
                return 1;
            }
            else if (fitxaSeleccionada.Sides[pos].Equals(SideType.FIELD))
            {
                return 2;
            }
            return 2;
        }

        private void activarDesactivarEdicio(bool estaActiu)
        {
            cbxRepeticions.IsEnabled = estaActiu;
            txtTitle.IsEnabled = estaActiu;
            btnImatgeFitxa.IsEnabled = estaActiu;
            lsvExtensions.IsEnabled = estaActiu;
            btnAddExtension.IsEnabled = estaActiu;
            btnDeleteExtension.IsEnabled = estaActiu;
            txtAltaExtension.IsEnabled = estaActiu;
            txtNotes.IsEnabled = estaActiu;
            ckbExtraStartingTile.IsEnabled = estaActiu;
            ckbExtraMonastery.IsEnabled = estaActiu;
            cbxSide0Fitxa.IsEnabled = estaActiu;
            cbxSide1Fitxa.IsEnabled = estaActiu;
            cbxSide2Fitxa.IsEnabled = estaActiu;
            cbxSide3Fitxa.IsEnabled = estaActiu;
            btnCancelFormulari.IsEnabled = false;
            btnSaveFormulari.IsEnabled = false;
        }

        private ObservableCollection<Image> llistaImatgesSides()
        {
            imgSides = new ObservableCollection<Image>();
            imgSides.Add(afegirImatgeSides("ms-appx:///Assets/icons/icons8-path-50.png"));
            imgSides.Add(afegirImatgeSides("ms-appx:///Assets/icons/icons8-castle-50.png"));
            imgSides.Add(afegirImatgeSides("ms-appx:///Assets/icons/icons8-cancel-64.png"));
            return imgSides;
        }

        private Image afegirImatgeSides(string uriImatge)
        {
            Image img = new Image();
            BitmapImage btmImg = new BitmapImage(new Uri(uriImatge));
            img.Source = btmImg;
            img.Height = 30;
            return img;
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

        private void lsvFitxes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            canviEstat(Estat.VIEW);
        }

        private void btnEditFitxes_Click(object sender, RoutedEventArgs e)
        {
            if (lsvFitxes.SelectedItem != null)
            {
                canviEstat(Estat.MODIFICACIO);
            }
        }

        private bool formulariValid()
        {
            bool formulariValid = false;
            if (lsvFitxes.SelectedItem != null)
            {
                if (cbxRepeticions.SelectedItem != null &&
                    Fitxa.validaTitle(txtTitle.Text) &&
                    lsvExtensions.SelectedItem != null &&
                    imgFitxa.Source != null &&
                    cbxSide0Fitxa.SelectedItem != null && cbxSide1Fitxa.SelectedItem != null && 
                    cbxSide2Fitxa.SelectedItem != null && cbxSide3Fitxa.SelectedItem != null)
                {
                    formulariValid = true;
                }
            }

            return formulariValid;
        }

        private void validarDadesFormulari()
        {
            if (lsvFitxes.SelectedItem != null)
            {
                Fitxa fitxaEditada = (Fitxa)lsvFitxes.SelectedItem;

                if (formulariValid()) {
                    bool hiHaCanvis = !(
                    fitxaEditada.Repeticions.Equals((int)cbxRepeticions.SelectedItem) &&
                    fitxaEditada.Title.Equals(txtTitle.Text) &&
                    fitxaEditada.Extens.Equals((string)lsvExtensions.SelectedItem) &&
                    fitxaEditada.Notes.Equals(txtNotes.Text) &&
                    fitxaEditada.IsMonastery.Equals(ckbExtraMonastery.IsChecked) &&
                    Fitxa.IsStartingTile.Equals(fitxaEditada) &&
                    fitxaEditada.ImagePath.Equals(((BitmapImage)imgFitxa.Source).UriSource.AbsoluteUri) &&
                    fitxaEditada.Sides[0].Equals(sideSeleccionat(cbxSide0Fitxa.SelectedIndex)) &&
                    fitxaEditada.Sides[1].Equals(sideSeleccionat(cbxSide1Fitxa.SelectedIndex)) &&
                    fitxaEditada.Sides[2].Equals(sideSeleccionat(cbxSide2Fitxa.SelectedIndex)) &&
                    fitxaEditada.Sides[3].Equals(sideSeleccionat(cbxSide3Fitxa.SelectedIndex))
                    );

                    if (hiHaCanvis)
                    {
                        btnCancelFormulari.IsEnabled = true;
                        btnSaveFormulari.IsEnabled = true;
                    }
                }
                else
                {
                    btnCancelFormulari.IsEnabled = true;
                    btnSaveFormulari.IsEnabled = false;
                }
            }
        }

        public SideType sideSeleccionat(int index)
        {
            if (index == 0)
            {
                return SideType.PATH;
            }
            else if (index == 1)
            {
                return SideType.CASTLE;
            }
            else if (index == 2)
            {
                return SideType.FIELD;
            }
            return SideType.FIELD;
        }

        private void cbxRepeticions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void lsvExtensions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void ckbExtraMonastery_Checked(object sender, RoutedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void ckbExtraStartingTile_Checked(object sender, RoutedEventArgs e)
        {
            validarDadesFormulari();
        }


        private async void ckbExtraStartingTile_Unchecked(object sender, RoutedEventArgs e)
        {
            /*if (Fitxa.IsStartingTile.Equals((Fitxa)lsvFitxes.SelectedItem))
            {
                await dialogStartingTile();
                ckbExtraStartingTile.IsChecked = true;
            }*/
        }

        private async System.Threading.Tasks.Task dialogStartingTile()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "ERROR!!",
                Content = "The panel cannot be left without a starter tab. If you want to make the change go to another tab and select the Check Starting Tile.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await dialog.ShowAsync();
        }

        private void cbxSide0Fitxa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cbxSide1Fitxa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cbxSide2Fitxa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cbxSide3Fitxa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void btnCancelFormulari_Click(object sender, RoutedEventArgs e)
        {
            canviEstat(Estat.VIEW);
        }

        private void btnSaveFormulari_Click(object sender, RoutedEventArgs e)
        {
            if (estat == Estat.ALTA)
            {

            }
            else
            {
                Fitxa fitxaEditada = (Fitxa)lsvFitxes.SelectedItem;
                fitxaEditada.Repeticions = (int)cbxRepeticions.SelectedItem;
                fitxaEditada.Title = txtTitle.Text;
                fitxaEditada.Extens = (string)lsvExtensions.SelectedItem;
                fitxaEditada.Notes = txtNotes.Text;
                fitxaEditada.ImagePath = ((BitmapImage)imgFitxa.Source).UriSource.AbsoluteUri;

                if (ckbExtraMonastery.IsChecked != null)
                {
                    fitxaEditada.IsMonastery = (bool)ckbExtraMonastery.IsChecked;
                }
                fitxaEditada.Sides[0] = sideSeleccionat(cbxSide0Fitxa.SelectedIndex);
                fitxaEditada.Sides[1] = sideSeleccionat(cbxSide1Fitxa.SelectedIndex);
                fitxaEditada.Sides[2] = sideSeleccionat(cbxSide2Fitxa.SelectedIndex);
                fitxaEditada.Sides[3] = sideSeleccionat(cbxSide3Fitxa.SelectedIndex);

                if (ckbExtraStartingTile.IsChecked != null)
                {
                    if ((bool)ckbExtraStartingTile.IsChecked == true)
                    {
                        Fitxa.IsStartingTile = fitxaEditada;
                    }
                }

                if (!fitxaEditada.ImagePath.Equals(((BitmapImage)imgFitxa.Source).UriSource.AbsoluteUri) ||
                    !fitxaEditada.Extens.Equals((string)lsvExtensions.SelectedItem))
                {
                    //
                }
            }

            canviEstat(Estat.VIEW);
        }

        private async void btnImatgeFitxa_Click(object sender, RoutedEventArgs e)
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
                imgFitxa.Source = tmpBitmap;
                imgSidesFitxa.Source = tmpBitmap;

                validarDadesFormulari();
            }
        }
    }
}
