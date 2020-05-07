using System;
using System.Diagnostics;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace AnimatedIconPrototype
{
    public class AnimatedIcon : Panel
    {
        public AnimatedIcon()
        {
            AnimatedVisualPlayerProposed player = new AnimatedVisualPlayerProposed();
            Children.Add(player);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = new Size();

            FrameworkElement child = (FrameworkElement)Children[0];

            child.Measure(availableSize);
            desiredSize = child.DesiredSize;

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            FrameworkElement child = (FrameworkElement)Children[0];
            child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));

            return finalSize;
        }

        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public static readonly DependencyProperty IsAnimatingProperty =
            DependencyProperty.Register("IsAnimating", typeof(bool), typeof(AnimatedIcon), new PropertyMetadata(true, new PropertyChangedCallback(OnIsAnimatingChanged)));

        private static void OnIsAnimatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedIcon = d as AnimatedIcon;
            animatedIcon.UpdateStates();
        }

        public bool IsLooping
        {
            get { return (bool)GetValue(IsLoopingProperty); }
            set { SetValue(IsLoopingProperty, value); }
        }

        public static readonly DependencyProperty IsLoopingProperty =
            DependencyProperty.Register("IsLooping", typeof(bool), typeof(AnimatedIcon), new PropertyMetadata(true, new PropertyChangedCallback(OnIsLoopingChanged)));

        private static void OnIsLoopingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedIcon = d as AnimatedIcon;
            animatedIcon.UpdateStates();
        }

        public double ProgressPosition
        {
            get { return (double)GetValue(ProgressPositionProperty); }
            set { SetValue(ProgressPositionProperty, value); }
        }

        public static readonly DependencyProperty ProgressPositionProperty =
            DependencyProperty.Register("ProgressPosition", typeof(double), typeof(AnimatedIcon), new PropertyMetadata(0, new PropertyChangedCallback(OnProgressPositionChanged)));

        private static void OnProgressPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedIcon = d as AnimatedIcon;
            animatedIcon.UpdateProgressPosition(animatedIcon.ProgressPosition);
        }

        public IAnimatedVisualSource Source
        {
            get { return (IAnimatedVisualSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IAnimatedVisualSource), typeof(AnimatedIcon), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedIcon = d as AnimatedIcon;
            animatedIcon.UpdateSource(e);
        }

        private void UpdateStates()
        {
            AnimatedVisualPlayer player = (AnimatedVisualPlayer)Children[0];

            if (this.IsAnimating)
            {
                _ = player.PlayAsync(0, 1, IsLooping);
            }
            else
            {
                player.Stop();
            }
        }

        private void UpdateSource(DependencyPropertyChangedEventArgs e)
        {
            AnimatedVisualPlayer player = (AnimatedVisualPlayer)Children[0];
            player.Source = e.NewValue as IAnimatedVisualSource;
        }

        private void UpdateProgressPosition(double progressPosition)
        {
            AnimatedVisualPlayer player = (AnimatedVisualPlayer)Children[0];
            player.SetProgress(progressPosition);
        }
    }
}
