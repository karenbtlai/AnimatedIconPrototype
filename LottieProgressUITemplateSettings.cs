using Windows.UI.Xaml;
using Windows.UI;

namespace AnimatedIconPrototype
{
    public class LottieProgressUITemplateSettings : DependencyObject
    {
        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(ProgressUITemplateSettings), new PropertyMetadata(Colors.DarkRed));

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(ProgressUITemplateSettings), new PropertyMetadata(Colors.DarkRed));


        // TODO: Add a property for circle arc segment properties etc to support storyboards possibly in the future if we get asks.
    }
}