using MapEditor.Model;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace MapEditor.View
{
    public sealed partial class UIItemOnTheMap : UserControl
    {
        public UIItemOnTheMap()
        {
            this.InitializeComponent();
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
            /*
             <Canvas x:Name="cnvItemMap">
                <Border x:Name="brdItemSelected">
                    <!-- El Height i el Width seran la mida que l'usuari dona a les celes -->
                    <Border x:Name="brdImage" BorderBrush="Red" BorderThickness="2" Height="49" Width="49">
                        <Image x:Name="imgItem" Source="/Assets/medal.png" Stretch="Fill"/>
                    </Border>
                </Border>
                <!-- relatiu al borde de la imatge -->
                <Grid Canvas.Top="-20" Canvas.Left="30"> <-- CONTINUAR PER AQUI
                    <Ellipse Width="30" Height="30" Stroke="Red" StrokeThickness="2" Fill="White"></Ellipse>
                    <TextBlock HorizontalAlignment="Center" VerticalAlignment="Center">x10</TextBlock>
                </Grid>
                <Grid Canvas.Top="39" Canvas.Left="30">
                    <Ellipse Width="30" Height="30" Stroke="Red" StrokeThickness="2" Fill="White"></Ellipse>
                    <Image Source="/Assets/invisible.png" HorizontalAlignment="Center" VerticalAlignment="Center"
                           Width="25"/>
                </Grid>
            </Canvas>
             */

            //Creació de la imatge del Item que hem pasen
            Image imgItem = new Image();
            imgItem.Source = mapItem.Item.ImageSource;
            imgItem.Stretch = Stretch.Fill;

            //Borde de la imatge del Item
            Border brdImage = new Border();
            brdImage.BorderBrush = new SolidColorBrush(Colors.Red);
            brdImage.BorderThickness = new Thickness(2);
            brdImage.Height = Map.getMap().CellHeight;
            brdImage.Width = Map.getMap().CellWidth;

            //Assignacio de la imatge al Borde
            brdImage.Child = imgItem;

            //Verificar si l'item esta seleccionat dins de la imatge

            Border brdItemSelected = new Border();
            brdItemSelected.BorderBrush = new SolidColorBrush(Colors.Yellow);
            brdItemSelected.BorderThickness = new Thickness(4);

            //PER MOURE L'ITEM EN EL MAPA

            //Canvas.SetLeft(brdItemSelected, brdImage.Width - 1);
            //Canvas.SetTop(brdItemSelected, brdImage.Height - 1);

            //Posar la imatge i el borde de la imatge dins del Border que nomes es mostrara si l'item esta seleccionat
            brdItemSelected.Child = brdImage;

            cnvItemMap.Children.Add(brdItemSelected);
        }
    }
}
