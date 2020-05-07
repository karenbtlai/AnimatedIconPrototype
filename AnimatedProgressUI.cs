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
    public class AnimatedProgressUI : Panel
    {
        public AnimatedProgressUI()
        {
            AnimatedVisualPlayer player = new AnimatedVisualPlayer();
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

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(AnimatedProgressUI), new PropertyMetadata(true, new PropertyChangedCallback(OnIsActiveChanged)));

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedProgressUI = d as AnimatedProgressUI;
            animatedProgressUI.UpdateStates();

        }

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }

        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(AnimatedProgressUI), new PropertyMetadata(true, new PropertyChangedCallback(OnIsIndeterminateChanged)));

        private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedProgressUI = d as AnimatedProgressUI;
            animatedProgressUI.UpdateStates();
        }

        public bool ShowPaused
        {
            get { return (bool)GetValue(ShowPausedProperty); }
            set { SetValue(ShowPausedProperty, value); }
        }

        public static readonly DependencyProperty ShowPausedProperty =
            DependencyProperty.Register("ShowPaused", typeof(bool), typeof(AnimatedProgressUI), new PropertyMetadata(false, new PropertyChangedCallback(OnShowPausedChanged)));

        private static void OnShowPausedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedProgressUI = d as AnimatedProgressUI;
            animatedProgressUI.UpdateStates();
        }

        public bool IsLooping
        {
            get { return (bool)GetValue(IsLoopingProperty); }
            set { SetValue(IsLoopingProperty, value); }
        }

        public static readonly DependencyProperty IsLoopingProperty =
            DependencyProperty.Register("IsLooping", typeof(bool), typeof(AnimatedProgressUI), new PropertyMetadata(true, new PropertyChangedCallback(OnIsLoopingChanged)));

        private static void OnIsLoopingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedProgressUI = d as AnimatedProgressUI;
            animatedProgressUI.UpdateStates();
        }

        public bool ShowError
        {
            get { return (bool)GetValue(ShowErrorProperty); }
            set { SetValue(ShowErrorProperty, value); }
        }

        public static readonly DependencyProperty ShowErrorProperty =
            DependencyProperty.Register("ShowError", typeof(bool), typeof(AnimatedProgressUI), new PropertyMetadata(false, new PropertyChangedCallback(OnShowErrorChanged)));

        private static void OnShowErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedProgressUI = d as AnimatedProgressUI;
            animatedProgressUI.UpdateStates();
        }

        public IAnimatedVisualSource AnimationSource
        {
            get { return (IAnimatedVisualSource)GetValue(AnimationSourceProperty); }
            set { SetValue(AnimationSourceProperty, value); }
        }

        public static readonly DependencyProperty AnimationSourceProperty =
            DependencyProperty.Register("AnimationSource", typeof(IAnimatedVisualSource), typeof(AnimatedProgressUI), new PropertyMetadata(null, new PropertyChangedCallback(OnAnimationSourceChanged)));

        private static void OnAnimationSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedProgressUI = d as AnimatedProgressUI;

            animatedProgressUI.UpdateSource(e);
            animatedProgressUI.UpdateStates();
        }

        public double ProgressPosition
        {
            get { return (double)GetValue(ProgressPositionProperty); }
            set { SetValue(ProgressPositionProperty, value); }
        }

        public static readonly DependencyProperty ProgressPositionProperty =
            DependencyProperty.Register("ProgressPosition", typeof(double), typeof(AnimatedProgressUI), new PropertyMetadata(0, new PropertyChangedCallback(OnProgressPositionChanged)));

        private static void OnProgressPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedProgressUI = d as AnimatedProgressUI;
            if (!animatedProgressUI.IsIndeterminate)
            {
                animatedProgressUI.UpdateProgressPosition(animatedProgressUI.ProgressPosition);
            }
        }

        private void UpdateStates()
        {
            AnimatedVisualPlayer player = (AnimatedVisualPlayer)Children[0];

            if (this.IsActive)
            {
                player.Opacity = 1;

                if (this.ShowPaused)
                {
                    player.Pause();
                }
                else if (this.IsIndeterminate)
                {
                    _ = player.PlayAsync(0, 1, IsLooping);
                }
                else if (!this.IsIndeterminate)
                {
                    player.SetProgress(ProgressPosition);
                }
            }
            else
            {
                player.Stop();
                player.Opacity = 0;
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
