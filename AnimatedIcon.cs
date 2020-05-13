using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Composition;
using System.Numerics;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;

interface ILottieVisual : IDisposable
{
    Visual RootVisual { get; }
    TimeSpan Duration { get; }
    Vector2 Size { get; }
}

interface ILottieVisualSource
{
    ILottieVisual TryCreateAnimatedVisual(Compositor compositor, out object diagnostics);
    CompositionPropertySet GetThemeProperties(Compositor compositor);
}

namespace AnimatedIconPrototype
{
    public class AnimatedIcon : Panel
    {
        // The Lottie source object, or null.
        ILottieVisualSource _lottieSource;

        // The Lottie instance object, or null.
        ILottieVisual _lottieInstance;

        // The id of the current Lottie source and instance.
        QaIcon _lottieIconId;

        // A visual used for scaling and offsetting the animated icon.
        ContainerVisual _rootVisual;

        // The UIElement above this in the visual tree that has been annotated
        // with the IsIconController=true attached property.
        UIElement _controller;

        public AnimatedIcon()
        {
            FontIcon fontText = new FontIcon();
            fontText.Glyph = Glyph;

            Children.Add(fontText);

            Loaded += AnimatedIcon_Loaded;
        }

        void AnimatedIcon_Loaded(object sender, RoutedEventArgs e)
        {
            _controller = FindController();

            // This is our opportunity to hook up to the controller's events:
            if (_controller is ButtonBase button)
            {
                button.PointerEntered += Button_PointerEntered;
                button.PointerExited += Button_PointerExited;
                button.PointerPressed += Button_PointerPressed;
            }
            //else if (_controller is RangeBase rangeBase)
            //{
            //    rangeBase.ValueChanged += RangeBase_ValueChanged;

            //    SyncProgressToRangeBaseController();
            //}
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

        //public double ProgressPosition
        //{
        //    get { return (double)GetValue(ProgressPositionProperty); }
        //    set { SetValue(ProgressPositionProperty, value); }
        //}

        //public static readonly DependencyProperty ProgressPositionProperty =
        //    DependencyProperty.Register("ProgressPosition", typeof(double), typeof(AnimatedIcon), new PropertyMetadata(0, new PropertyChangedCallback(OnProgressPositionChanged)));

        //private static void OnProgressPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var animatedIcon = d as AnimatedIcon;
        //    animatedIcon.UpdateProgressPosition(animatedIcon.ProgressPosition);
        //}



        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Glyph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register("Glyph", typeof(string), typeof(AnimatedIcon), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnGlyphChanged)));

        private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedIcon = (AnimatedIcon)d;

            // The text has changed. See if the new text value matches
            // one of the well-known animated icons.
            var lottieGlyph = IconFromGlyph((string)e.NewValue);

            //animatedIcon._isMuteIcon = lottieIconId == QaIcon.Mute;

            if (lottieGlyph == QaIcon.None)
            {
                // There is no Lottie animation for this text, or animations
                // are disabled. Just show it as text.
                animatedIcon.ShowText();
            }
            else
            {
                //if (animatedIcon._isMuteIcon)
                //{
                //    // Special case - use the Volume icon for mute. It will
                //    // be prevented from animating.
                //    lottieGlyph = QaIcon.Volume;
                //}

                animatedIcon.UpdateLottieSourceAndInstance(lottieGlyph);

                animatedIcon.ShowLottie();
            }
        }

        public static DependencyProperty IsIconControllerProperty { get; } =
            DependencyProperty.RegisterAttached(
                "IsIconController",
                typeof(bool),
                typeof(AnimatedIcon),
                new PropertyMetadata(false));

        //public IAnimatedVisualSource Source
        //{
        //    get { return (IAnimatedVisualSource)GetValue(SourceProperty); }
        //    set { SetValue(SourceProperty, value); }
        //}

        //public static readonly DependencyProperty SourceProperty =
        //    DependencyProperty.Register("Source", typeof(IAnimatedVisualSource), typeof(AnimatedIcon), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        //private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var animatedIcon = d as AnimatedIcon;
        //    animatedIcon.UpdateSource(e);
        //}

        // Attached property setter for IsIconController.
        public static void SetIsIconController(UIElement element, bool value)
             => element.SetValue(IsIconControllerProperty, value);

        // Attached property getter for IsIconController.
        public static bool GetIsIconController(UIElement element)
            => (bool)element.GetValue(IsIconControllerProperty);

        // Handles pointer entered on a controller that is a Button.
        static void Button_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            => FindAnimatedIcon((ButtonBase)sender)?.PlayOnce();

        // Handles pointer exited on a controller that is a Button.
        static void Button_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            => FindAnimatedIcon((ButtonBase)sender)?.Stop();

        // Handles a press on a controller that is a Button.
        void Button_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            => FindAnimatedIcon((ButtonBase)sender)?.PlayOnce();

        // Searches the visual tree from the given root and returns the first
        // AnimatedIcon in the tree, or null if none found.
        static AnimatedIcon FindAnimatedIcon(DependencyObject root)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(root);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                if (child is AnimatedIcon animatedIcon ||
                    (animatedIcon = FindAnimatedIcon(child)) != null)
                {
                    return animatedIcon;
                }
            }

            return null;
        }

        // Searches up the visual tree and returns the first element marked as being
        // a controller for an animated icon, or null if none found.
        UIElement FindController()
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (parent != null)
            {
                if (parent.ReadLocalValue(IsIconControllerProperty) is bool isController && isController)
                {
                    return (UIElement)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
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

        // Stops the Lottie animation.
        void Stop()
        {
            if (_lottieInstance != null)
            {
                var propertySet = _lottieInstance.RootVisual.Properties;
                propertySet.InsertScalar("Progress", 0);
            }
        }

        // Plays the Lottie animation once.
        void PlayOnce()
        {
            if (_lottieInstance != null)
            {
                var propertySet = _lottieInstance.RootVisual.Properties;
                var compositor = propertySet.Compositor;
                using (var kfa = compositor.CreateScalarKeyFrameAnimation())
                using (var easing = compositor.CreateLinearEasingFunction())
                {
                    kfa.Duration = _lottieInstance.Duration;
                    kfa.IterationBehavior = AnimationIterationBehavior.Count;
                    kfa.IterationCount = 1;
                    kfa.InsertKeyFrame(0, 0, easing);
                    kfa.InsertKeyFrame(1, 1, easing);
                    propertySet.StartAnimation("Progress", kfa);
                }
            }
        }

        //private void UpdateSource(DependencyPropertyChangedEventArgs e)
        //{
        //    AnimatedVisualPlayer player = (AnimatedVisualPlayer)Children[0];
        //    player.Source = e.NewValue as IAnimatedVisualSource;
        //}

        //private void UpdateProgressPosition(double progressPosition)
        //{
        //    AnimatedVisualPlayer player = (AnimatedVisualPlayer)Children[0];
        //    player.SetProgress(progressPosition);
        //}

        // Shows the current Text value as text.
        void ShowText()
        {
            // Make the TextBlock visible.
            FontIcon fontText = new FontIcon();
            fontText.Glyph = Glyph;
            Children.Add(fontText);

            // Hide the lottie animation.
            if (_rootVisual != null)
            {
                _rootVisual.IsVisible = false;
            }

            InvalidateMeasure();
        }

        void ShowLottie()
        {
            if (_lottieSource is null)
            {
                ShowText();
            }
            else
            {
                // Hide the static text block.
                //Children.Clear();

                // Make the Lottie animation visible.
                _rootVisual.IsVisible = true;

                // Synchronize the foreground color with the current Foreground property value.
                //UpdateLottieForegroundColor();

                // If there's a RangeBase controller sync Progress to its current value.
                //SyncProgressToRangeBaseController();

                InvalidateMeasure();
            }
        }

        void UpdateLottieSourceAndInstance(QaIcon icon)
        {
            if (icon != _lottieIconId)
            {
                _lottieSource = CreateLottieSourceForIcon(icon);

                if (_lottieSource != null)
                {
                    _lottieIconId = icon;

                    if (_rootVisual is null)
                    {
                        _rootVisual = Window.Current.Compositor.CreateContainerVisual();
                        ElementCompositionPreview.SetElementChildVisual(this, _rootVisual);
                    }

                    // Create an instance of the Lottie animation.
                    _lottieInstance = _lottieSource.TryCreateAnimatedVisual(_rootVisual.Compositor, out var _);

                    var rootVisualChildren = _rootVisual.Children;
                    rootVisualChildren.RemoveAll();
                    rootVisualChildren.InsertAtTop(_lottieInstance.RootVisual);
                }
            }
        }

        static ILottieVisualSource CreateLottieSourceForIcon(QaIcon icon)
        {
            switch (icon)
            {
                case QaIcon.AirplaneMode: return new AnimatedIconPrototype.QA_AirplaneMode();
                //case QaIcon.Bluetooth: return new WindowsInternal.ComposableShell.Experiences.QA_BlueTooth();
                //case QaIcon.Brightness: return new WindowsInternal.ComposableShell.Experiences.QA_Brightness();
                //case QaIcon.Cellular: return new WindowsInternal.ComposableShell.Experiences.QA_Cellular();
                //case QaIcon.ComposeMode: return new WindowsInternal.ComposableShell.Experiences.QA_ComposeMode();
                //case QaIcon.DoNotDisturb: return new WindowsInternal.ComposableShell.Experiences.QA_DoNotDisturb();
                //case QaIcon.EaseOfAccess: return new WindowsInternal.ComposableShell.Experiences.QA_EaseOfAccess();
                //case QaIcon.InputLanguage: return new WindowsInternal.ComposableShell.Experiences.QA_InputLanguage();
                //case QaIcon.Location: return new WindowsInternal.ComposableShell.Experiences.QA_Location();
                //case QaIcon.Project: return new WindowsInternal.ComposableShell.Experiences.QA_Project();
                //case QaIcon.RotationLock: return new WindowsInternal.ComposableShell.Experiences.QA_RotationLock();
                //case QaIcon.Volume: return new WindowsInternal.ComposableShell.Experiences.QA_Volume();
                //case QaIcon.Wifi: return new WindowsInternal.ComposableShell.Experiences.QA_Wifi();
            }

            return null;
        }

        static QaIcon IconFromGlyph(string value)
        {
            var result = QaIcon.None;
            if (value.Length == 1)
            {
                result = (QaIcon)value[0];
                switch (result)
                {
                    // These are the icons for which there are Lottie equivalents.
                    case QaIcon.AirplaneMode:
                    case QaIcon.Bluetooth:
                    case QaIcon.Brightness:
                    case QaIcon.ComposeMode:
                    case QaIcon.Cellular:
                    case QaIcon.DoNotDisturb:
                    case QaIcon.EaseOfAccess:
                    case QaIcon.InputLanguage:
                    case QaIcon.Location:
                    case QaIcon.Mute:
                    case QaIcon.Project:
                    case QaIcon.RotationLock:
                    case QaIcon.Volume:
                    case QaIcon.Wifi:
                        break;

                    default:
                        // The icon does not have a Lottie equivalent.
                        result = QaIcon.None;
                        break;
                }
            }

            return result;
        }

        enum QaIcon
        {
            None = 0,
            AirplaneMode = 0xE709,
            AllSettings = 0xE713,
            Battery = 0xF5FA,
            Bluetooth = 0xE702,
            Brightness = 0xE706,
            ComposeMode = 0xEFA5,
            Cellular = 0xE870,
            DoNotDisturb = 0xE708,
            EaseOfAccess = 0xE776,
            EaseOfAccessKeyboard = 0xF158,
            Edit = 0xE70F,
            InputLanguage = 0xE8C1,
            Location = 0xE707,
            Lock = 0xE72E,
            Mute = 0xE74F,
            Magnify = 0xE8A3,
            Power = 0xE7E8,
            Project = 0xEBC6,
            RotationLock = 0xE755,
            TouchKeyboard = 0xE765,
            Volume = 0xE15D,
            Wifi = 0xE701,
        }
    }
}
