using MapEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace MapEditor.View
{
    public sealed partial class UIItemOnTheMap : UserControl
    {
        public UIItemOnTheMap()
        {
            this.InitializeComponent();
        }

        //Utilitzo el Tapped encontes del Clicked
        //public event EventHandler Clicked;
        
        public Boolean seleccionat
        {
            get { return (Boolean)GetValue(seleccionatProperty); }
            set { SetValue(seleccionatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for seleccionat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty seleccionatProperty =
            DependencyProperty.Register("seleccionat", typeof(Boolean), typeof(UIItemOnTheMap), 
                new PropertyMetadata(false, seleccionatChangedCallbackStatic));

        private static void seleccionatChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIItemOnTheMap im = (UIItemOnTheMap)d;
            im.seleccionatChangedCallback(e);
        }
        private void seleccionatChangedCallback(DependencyPropertyChangedEventArgs e)
        {
            itemSeleccionat();
        }


        Border brdItemSelected = new Border();

        private void itemSeleccionat()
        {
            
            if (seleccionat)
            {
                brdItemSelected.BorderBrush = new SolidColorBrush(Colors.Yellow);
                brdItemSelected.BorderThickness = new Thickness(4);
            }
            else
            {
                brdItemSelected.BorderBrush = new SolidColorBrush(Colors.White);
                brdItemSelected.BorderThickness = new Thickness(1);
            }
        }

        public MapItem mapItem
        {
            get { return (MapItem)GetValue(mapItemProperty); }
            set { SetValue(mapItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for mapItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty mapItemProperty =
            DependencyProperty.Register("mapItem", typeof(MapItem), typeof(UIItemOnTheMap), 
                new PropertyMetadata(null, ItemMapChangedCallbackStatic));

        private static void ItemMapChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIItemOnTheMap im = (UIItemOnTheMap)d;
            im.ValorsChangedCallback(e);
        }
        private void ValorsChangedCallback(DependencyPropertyChangedEventArgs e)
        {
            construirItem();
        }

        private void construirItem()
        {
            /*<Border x:Name="brdItemSelected">
                    <!-- El Height i el Width seran la mida que l'usuari dona a les celes -->
                    <Border x:Name="brdImage" BorderBrush="Red" BorderThickness="2" Height="49" Width="49">
                        <Image x:Name="imgItem" Source="/Assets/medal.png" Stretch="Fill"/>
                    </Border>
                </Border>
             */

            //Creació de la imatge del Item que hem pasen
            Image imgItem = new Image();
            imgItem.Source = mapItem.Item.ImageSource;
            imgItem.Stretch = Stretch.Fill;
            //Borde de la imatge del Item
            Border brdImage = new Border();
            brdImage.BorderBrush = new SolidColorBrush(Colors.Red);
            brdImage.BorderThickness = new Thickness(2);
            brdImage.Height = Map.getMap().CellWidth;
            brdImage.Width = Map.getMap().CellWidth;

            //Assignacio de la imatge al Borde
            brdImage.Child = imgItem;

            //Verificar si l'item esta seleccionat dins de la imatge

            itemSeleccionat();

            //Posar la imatge i el borde de la imatge dins del Border que es posara en groc si esta seleccionat
            brdItemSelected.Child = brdImage;

            cnvItemMap.Children.Add(brdItemSelected);


            /*
             <!-- relatiu al borde de la imatge -->
                <Grid Canvas.Top="-20" Canvas.Left="30"> <-- CONTINUAR PER AQUI
                    <Ellipse Width="30" Height="30" Stroke="Red" StrokeThickness="2" Fill="White"></Ellipse>
                    <TextBlock HorizontalAlignment="Center" VerticalAlignment="Center">x10</TextBlock>
                </Grid>
             */
            Grid grdAmount = new Grid();
            // relatiu al borde de la imatge
            Canvas.SetLeft(grdAmount, brdImage.Width - 15); //La cordenada de la imatge es WIDTH
            Canvas.SetTop(grdAmount, -15); //La cordenada de la imatge es 0

            Ellipse epsAmount = new Ellipse();
            epsAmount.Width = 30;
            epsAmount.Height = 30;
            epsAmount.Stroke = new SolidColorBrush(Colors.Red);
            epsAmount.StrokeThickness = 2;
            epsAmount.Fill = new SolidColorBrush(Colors.White);

            TextBlock txbAmount = new TextBlock();
            txbAmount.Text = "x" + mapItem.Amount.ToString();
            txbAmount.HorizontalAlignment = HorizontalAlignment.Center;
            txbAmount.VerticalAlignment = VerticalAlignment.Center;

            grdAmount.Children.Add(epsAmount);
            grdAmount.Children.Add(txbAmount);

            cnvItemMap.Children.Add(grdAmount);

            /*
            <Grid Canvas.Top="39" Canvas.Left="30">
                <Ellipse Width="30" Height="30" Stroke="Red" StrokeThickness="2" Fill="White"></Ellipse>
                <Image Source="/Assets/invisible.png" HorizontalAlignment="Center" VerticalAlignment="Center"
                        Width="25"/>
            </Grid> 
             */

            if (mapItem.IsHidden)
            {
                Grid grdHidden = new Grid();
                // relatiu al borde de la imatge
                Canvas.SetLeft(grdHidden, brdImage.Width - 15); //La cordenada de la imatge es WIDTH
                Canvas.SetTop(grdHidden, brdImage.Height - 15); //La cordenada de la imatge es HEIGHT

                Ellipse epsHidden = new Ellipse();
                epsHidden.Width = 30;
                epsHidden.Height = 30;
                epsHidden.Stroke = new SolidColorBrush(Colors.Red);
                epsHidden.StrokeThickness = 2;
                epsHidden.Fill = new SolidColorBrush(Colors.White);

                Image imgHidden = new Image();
                BitmapImage bitmapHidden = new BitmapImage(new Uri("ms-appx:///Assets/invisible.png"));
                imgHidden.Source = bitmapHidden;
                imgHidden.HorizontalAlignment = HorizontalAlignment.Center;
                imgHidden.VerticalAlignment = VerticalAlignment.Center;
                imgHidden.Width = 25;

                grdHidden.Children.Add(epsHidden);
                grdHidden.Children.Add(imgHidden);

                cnvItemMap.Children.Add(grdHidden);
            }
                

        }
    }
}
