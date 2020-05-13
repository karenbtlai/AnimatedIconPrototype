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

namespace AnimatedIconPrototype
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        //private void OnPointerOverAnimatedIcon_PointerEntered(object sender, PointerRoutedEventArgs e)
        //{
        //    OnPointerOverAnimatedIcon.IsAnimating = true;
        //}

        //private void OnPointerOverAnimatedIcon_PointerExited(object sender, PointerRoutedEventArgs e)
        //{
        //    OnPointerOverAnimatedIcon.IsAnimating = false;
        //}
    }

    public class SolidColorBrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, String language)
        {
            var colorBrush = value as SolidColorBrush;
            return colorBrush.Color;
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
