using Windows.UI.Xaml;
using Windows.UI;

namespace AnimatedIconPrototype
{
    public class ProgressUITemplateSettings : DependencyObject
    {
        public double ProgressPosition
        {
            get { return (double)GetValue(ProgressPositionProperty); }
            set { SetValue(ProgressPositionProperty, value); }
        }

        public static readonly DependencyProperty ProgressPositionProperty =
            DependencyProperty.Register("ProgressPosition", typeof(double), typeof(ProgressUITemplateSettings), new PropertyMetadata(0));

    }
}
