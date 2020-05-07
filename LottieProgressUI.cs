using System;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AnimatedIconPrototype
{
    public sealed class LottieProgressUI : ProgressUI
    {
        public LottieProgressUI()
        {
            this.DefaultStyleKey = typeof(LottieProgressUI);
        }

        public LottieProgressUITemplateSettings LottieProgressUITemplateSettings { get; } = new LottieProgressUITemplateSettings();

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RegisterAndUpdateColors();
        }

        private void RegisterAndUpdateColors()
        {
            if (Foreground is SolidColorBrush)
            {
                var foreground = (Foreground as SolidColorBrush);
                RegisterPropertyChangedCallback(ForegroundProperty,
                    new DependencyPropertyChangedCallback(OnForegroundChanged));

                foreground.RegisterPropertyChangedCallback(SolidColorBrush.ColorProperty,
                    new DependencyPropertyChangedCallback(OnForegroundChanged));

                UpdateForegroundTemplateSettings();
            }

            if (Background is SolidColorBrush)
            {
                var background = (Background as SolidColorBrush);
                LottieProgressUITemplateSettings.BackgroundColor = background.Color;
                RegisterPropertyChangedCallback(BackgroundProperty,
                   new DependencyPropertyChangedCallback(OnBackgroundChanged));

                background.RegisterPropertyChangedCallback(SolidColorBrush.ColorProperty,
                    new DependencyPropertyChangedCallback(OnBackgroundChanged));

                UpdateBackgroundTemplateSettings();
            }
        }

        private void OnForegroundChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateForegroundTemplateSettings();
        }

        private void OnBackgroundChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateBackgroundTemplateSettings();
        }

        private void UpdateForegroundTemplateSettings()
        {
            if (Foreground is SolidColorBrush)
            {
                LottieProgressUITemplateSettings.ForegroundColor = (Foreground as SolidColorBrush).Color;
            }
        }

        private void UpdateBackgroundTemplateSettings()
        {
            if (Background is SolidColorBrush)
            {
                LottieProgressUITemplateSettings.BackgroundColor = (Background as SolidColorBrush).Color;
            }
        }

        public IAnimatedVisualSource DeterminateAnimationSource
        {
            get { return (IAnimatedVisualSource)GetValue(DeterminateAnimationSourceProperty); }
            set { SetValue(DeterminateAnimationSourceProperty, value); }
        }

        public static readonly DependencyProperty DeterminateAnimationSourceProperty = DependencyProperty.Register("DeterminateAnimationSource", typeof(IAnimatedVisualSource), typeof(ProgressUI), new PropertyMetadata(null, new PropertyChangedCallback(OnDeterminateAnimationSourceChanged)));

        public static void OnDeterminateAnimationSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public IAnimatedVisualSource IndeterminateAnimationSource
        {
            get { return (IAnimatedVisualSource)GetValue(IndeterminateAnimationSourceProperty); }
            set { SetValue(IndeterminateAnimationSourceProperty, value); }
        }

        public static readonly DependencyProperty IndeterminateAnimationSourceProperty = DependencyProperty.Register("IndeterminateAnimationSource", typeof(IAnimatedVisualSource), typeof(LottieProgressUI), new PropertyMetadata(null, new PropertyChangedCallback(OnIndeterminateAnimationSourceChanged)));

        public static void OnIndeterminateAnimationSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
