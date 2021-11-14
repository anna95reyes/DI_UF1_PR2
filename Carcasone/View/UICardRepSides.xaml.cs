using Carcasone.Model;
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

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace Carcasone.View
{
    public sealed partial class UICardRepSides : UserControl
    {
        public UICardRepSides()
        {
            this.InitializeComponent();
        }



        public Fitxa fitxa
        {
            get { return (Fitxa)GetValue(fitxaProperty); }
            set { SetValue(fitxaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for fitxa.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty fitxaProperty =
            DependencyProperty.Register("fitxa", typeof(Fitxa), typeof(UICardRepSides), new PropertyMetadata(null));




    }
}
