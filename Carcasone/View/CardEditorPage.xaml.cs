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
        IEnumerable<IGrouping<String, Fitxa>> groups;

        public CardEditorPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txbTitlePage.Text = pageName;

            groups = from c in Fitxa.getFitxes() group c by c.Extens;
            this.cvs.Source = groups;

            cbxRepeticions.ItemsSource = new int[] { 1, 2, 3, 4 };
            cbxRepeticions.SelectedIndex = 0;

            lsvExtensions.ItemsSource = CardEditorPage.getExtensions();
            lsvExtensions.SelectedIndex = 0;

            cbxSideTopFitxa.ItemsSource = llistaImatgesSides();
            cbxSideRightFitxa.ItemsSource = llistaImatgesSides();
            cbxSideBottomFitxa.ItemsSource = llistaImatgesSides();
            cbxSideLeftFitxa.ItemsSource = llistaImatgesSides();
            
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
            } 
            else if (estat == Estat.MODIFICACIO)
            {
                activarDesactivarEdicio(true);
                btnCancelFormulari.IsEnabled = false;
                btnSaveFormulari.IsEnabled = false;
            }
            else if (estat == Estat.ALTA)
            {
                netejarFormulari();
                activarDesactivarEdicio(true);
                btnCancelFormulari.IsEnabled = false;
                btnSaveFormulari.IsEnabled = false;
            }
        }

        private void mostrarDadesFormulari()
        {
            if (lsvFitxes.SelectedItem != null)
            {
                Fitxa fitxaSeleccionada = (Fitxa)lsvFitxes.SelectedItem;
                BitmapImage btmImgFitxaSeleccionada = new BitmapImage(new Uri(fitxaSeleccionada.ImagePath));
                
                cbxRepeticions.SelectedValue = fitxaSeleccionada.Repeticions;
                txtTitle.Text = fitxaSeleccionada.Title;
                lsvExtensions.SelectedValue = fitxaSeleccionada.Extens;
                imgFitxa.Source = btmImgFitxaSeleccionada;
                txtNotes.Text = fitxaSeleccionada.Notes;
                ckbExtraStartingTile.IsChecked = (Fitxa.IsStartingTile.getCodi() == fitxaSeleccionada.getCodi())? true : false;
                ckbExtraMonastery.IsChecked = fitxaSeleccionada.IsMonastery;
                imgSidesFitxa.Source = btmImgFitxaSeleccionada;

                cbxSideTopFitxa.SelectedIndex = sidesSeleccionat(fitxaSeleccionada, PosType.TOP);
                cbxSideRightFitxa.SelectedIndex = sidesSeleccionat(fitxaSeleccionada, PosType.RIGHT);
                cbxSideBottomFitxa.SelectedIndex = sidesSeleccionat(fitxaSeleccionada, PosType.BOTTOM);
                cbxSideLeftFitxa.SelectedIndex = sidesSeleccionat(fitxaSeleccionada, PosType.LEFT);
            }
        }

        private void netejarFormulari()
        {
            cbxRepeticions.SelectedIndex = 0;
            txtTitle.Text = " ";
            lsvExtensions.SelectedIndex = 0;
            imgFitxa.Source = null;
            txtNotes.Text = "";
            ckbExtraStartingTile.IsChecked = false;
            ckbExtraMonastery.IsChecked = false;
            imgSidesFitxa.Source = null;
            cbxSideTopFitxa.SelectedIndex = 2;
            cbxSideRightFitxa.SelectedIndex = 2;
            cbxSideBottomFitxa.SelectedIndex = 2;
            cbxSideLeftFitxa.SelectedIndex = 2;
        }

        public int sidesSeleccionat(Fitxa fitxaSeleccionada, PosType pos)
        {
            if (fitxaSeleccionada.Sides[(int)pos].Equals(SideType.PATH))
            {
                return 0;
            }
            else if (fitxaSeleccionada.Sides[(int)pos].Equals(SideType.CASTLE))
            {
                return 1;
            }
            else if (fitxaSeleccionada.Sides[(int)pos].Equals(SideType.FIELD))
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
            btnAddExtension.IsEnabled = false;
            btnDeleteExtension.IsEnabled = estat == Estat.MODIFICACIO && lsvExtensions.SelectedItem != null;
            txtAltaExtension.IsEnabled = estaActiu;
            txtNotes.IsEnabled = estaActiu;
            ckbExtraStartingTile.IsEnabled = estaActiu;
            ckbExtraMonastery.IsEnabled = estaActiu;
            cbxSideTopFitxa.IsEnabled = estaActiu;
            cbxSideRightFitxa.IsEnabled = estaActiu;
            cbxSideBottomFitxa.IsEnabled = estaActiu;
            cbxSideLeftFitxa.IsEnabled = estaActiu;
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
        public static Boolean addExtensio(String novaExtensio)
        {
            if (_extensions.Contains(novaExtensio))
            {
                return false;
            }
            _extensions.Add(novaExtensio);
            return true;
        }

        public static Boolean removeExtensio(String extensio)
        {
            if (_extensions.Contains(extensio))
            {
                _extensions.Remove(extensio);
                return true;
            }
            return false;
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
            
            if (cbxRepeticions.SelectedItem != null &&
                Fitxa.validaTitle(txtTitle.Text) &&
                lsvExtensions.SelectedItem != null &&
                imgFitxa.Source != null &&
                cbxSideTopFitxa.SelectedItem != null && cbxSideRightFitxa.SelectedItem != null && 
                cbxSideBottomFitxa.SelectedItem != null && cbxSideLeftFitxa.SelectedItem != null)
            {
                formulariValid = true;
            }

            return formulariValid;
        }

        private void validarDadesFormulari()
        {
            
            if (formulariValid())
            {
                bool hiHaCanvis = false;
                if (lsvFitxes.SelectedItem != null)
                {
                    Fitxa fitxaEditada = (Fitxa)lsvFitxes.SelectedItem;

                    hiHaCanvis = !(
                        fitxaEditada.Repeticions.Equals((int)cbxRepeticions.SelectedItem) &&
                        fitxaEditada.Title.Equals(txtTitle.Text) &&
                        fitxaEditada.Notes.Equals(txtNotes.Text) &&
                        fitxaEditada.IsMonastery.Equals(ckbExtraMonastery.IsChecked) &&
                        fitxaEditada.ImagePath.Equals(((BitmapImage)imgFitxa.Source).UriSource.AbsoluteUri) &&

                        fitxaEditada.Sides[(int)PosType.TOP].Equals(sideSeleccionat(cbxSideTopFitxa.SelectedIndex)) &&
                        fitxaEditada.Sides[(int)PosType.RIGHT].Equals(sideSeleccionat(cbxSideRightFitxa.SelectedIndex)) &&
                        fitxaEditada.Sides[(int)PosType.BOTTOM].Equals(sideSeleccionat(cbxSideBottomFitxa.SelectedIndex)) &&
                        fitxaEditada.Sides[(int)PosType.LEFT].Equals(sideSeleccionat(cbxSideLeftFitxa.SelectedIndex)) &&
                        fitxaEditada.Extens.Equals((String)lsvExtensions.SelectedItem)
                        );

                }
                if (estat == Estat.MODIFICACIO && hiHaCanvis || estat == Estat.ALTA)
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
            if (cbxRepeticions.SelectedItem != null)
            {
                validarDadesFormulari();
            }
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void lsvExtensions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDeleteExtension.IsEnabled = lsvExtensions.SelectedItem != null;
            if (lsvExtensions.SelectedItem != null)
            {
                validarDadesFormulari();
            }
            
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
            btnSaveFormulari.IsEnabled = true;
            validarDadesFormulari();
        }

        private void cbxSideTopFitxa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cbxSideRightFitxa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cbxSideBottomFitxa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cbxSideLeftFitxa_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                SideType[] sides = new SideType[4];

                sides[(int)PosType.TOP] = sideSeleccionat(cbxSideTopFitxa.SelectedIndex);
                sides[(int)PosType.RIGHT] = sideSeleccionat(cbxSideRightFitxa.SelectedIndex);
                sides[(int)PosType.BOTTOM] = sideSeleccionat(cbxSideBottomFitxa.SelectedIndex);
                sides[(int)PosType.LEFT] = sideSeleccionat(cbxSideLeftFitxa.SelectedIndex);

                Fitxa novaFitxa = new Fitxa(txtTitle.Text, ((BitmapImage)imgFitxa.Source).UriSource.AbsoluteUri, (int)cbxRepeticions.SelectedItem,
                                            txtNotes.Text, sides, (bool)ckbExtraMonastery.IsChecked, (string)lsvExtensions.SelectedItem);
                if (ckbExtraStartingTile.IsChecked != null)
                {
                    if ((bool)ckbExtraStartingTile.IsChecked == true)
                    {
                        Fitxa.IsStartingTile = novaFitxa;
                    }
                }
                Fitxa.addFitxa(novaFitxa);
                lsvFitxes.SelectedItem = novaFitxa;

                this.cvs.Source = groups;

                lsvFitxes.SelectedItem = novaFitxa;

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

                fitxaEditada.Sides[(int)PosType.TOP] = sideSeleccionat(cbxSideTopFitxa.SelectedIndex);
                fitxaEditada.Sides[(int)PosType.RIGHT] = sideSeleccionat(cbxSideRightFitxa.SelectedIndex);
                fitxaEditada.Sides[(int)PosType.BOTTOM] = sideSeleccionat(cbxSideBottomFitxa.SelectedIndex);
                fitxaEditada.Sides[(int)PosType.LEFT] = sideSeleccionat(cbxSideLeftFitxa.SelectedIndex);

                if (ckbExtraStartingTile.IsChecked != null)
                {
                    if ((bool)ckbExtraStartingTile.IsChecked == true)
                    {
                        Fitxa.IsStartingTile = fitxaEditada;
                    }
                    else
                    {
                        Fitxa.IsStartingTile = null;
                    }
                }

                if (Fitxa.IsStartingTile == null)
                {
                    int randomFitxa;

                    if (Fitxa.getFitxes().Count > 1)
                    {
                        missatgeAvisCanviIsStartingTile();

                        randomFitxa = Fitxa.generarRandom(0, Fitxa.getFitxes().Count - 1);

                        while (fitxaEditada.getCodi() == randomFitxa)
                        {
                            randomFitxa = Fitxa.generarRandom(0, Fitxa.getFitxes().Count - 1);
                        }

                        Fitxa.IsStartingTile = Fitxa.getFitxes()[randomFitxa];
                    }
                }

                Fitxa.getFitxes()[fitxaEditada.getCodi()] = fitxaEditada;
                this.cvs.Source = groups;

                lsvFitxes.SelectedItem = fitxaEditada;
            }

            canviEstat(Estat.VIEW);
        }

        private async System.Threading.Tasks.Task missatgeAvisCanviIsStartingTile()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "INFORMACIO",
                Content = "Al treure la fitxa d'inici, automaticament la carta buscara una alta carta per posar-la d'inici.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await dialog.ShowAsync();
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

        private void btnAddFitxes_Click(object sender, RoutedEventArgs e)
        {
            lsvFitxes.SelectedItem = null;
            canviEstat(Estat.ALTA);
        }

        private void btnDeleteFitxes_Click(object sender, RoutedEventArgs e)
        {
            if (estat == Estat.VIEW)
            {
                if (lsvFitxes.SelectedItem != null)
                {
                    Fitxa fitxaAElimnar = (Fitxa)lsvFitxes.SelectedItem;
                    if (Fitxa.removeFitxa(fitxaAElimnar))
                    {
                        netejarFormulari();
                        this.cvs.Source = groups;

                        lsvFitxes.SelectedItem = null;
                    }

                }
            }
        }

        private void txtAltaExtension_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnAddExtension.IsEnabled = txtAltaExtension.Text.Length > 0;
        }

        private void btnAddExtension_Click(object sender, RoutedEventArgs e)
        {
            addExtensio(txtAltaExtension.Text);
            txtAltaExtension.Text = "";
        }

        private void btnDeleteExtension_Click(object sender, RoutedEventArgs e)
        {
            String extensioAEliminar = (String)lsvExtensions.SelectedItem;
            if (removeExtensio((String)lsvExtensions.SelectedItem))
            {
                for (int i = 0; i < Fitxa.getFitxes().Count; i++)
                {
                    if (Fitxa.getFitxes()[i].Extens != null)
                    {
                        if (Fitxa.getFitxes()[i].Extens.Equals(extensioAEliminar))
                        {
                            Fitxa.getFitxes()[i].Extens = null;
                        }
                    }
                }
            }
            this.cvs.Source = groups;
        }

    }
}
