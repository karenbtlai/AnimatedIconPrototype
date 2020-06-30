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
using Windows.UI;
using Windows.UI.Xaml.Input;

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
        // Registration token from the subscription to the color changed property
        // on the Foreground brush. 0 if not currently subscribed.
        long _colorPropertyChangedRegistration;

        // The Lottie source object, or null.
        ILottieVisualSource _lottieSource;

        // The Lottie instance object, or null.
        ILottieVisual _lottieInstance;

        // The id of the current Lottie source and instance.
        QaIcon _lottieIconId;

        // A visual used for scaling and offsetting the animated icon.
        ContainerVisual _rootVisual;

        // True iff the icon is displaying "Mute". This is a special case
        // as it is displayed as frame 0 of the Volume Lottie.
        bool _isMuteIcon;

        // The Foreground brush, iff it's a SolidColorBrush.
        SolidColorBrush _foregroundSolidColorBrush;

        // The UIElement above this in the visual tree that has been annotated
        // with the IsIconController=true attached property.
        UIElement _controller;

        bool _toggleIsPressed = true;

        public AnimatedIcon()
        {
            // Add FontIcon to Panel for for fallback
            TextBlock fontText = new TextBlock();
            Children.Add(fontText);

            Loaded += AnimatedIcon_Loaded;
        }

        // Converts a Color to a Vector4 value, as required by Lottie animations.
        static Vector4 ColorAsVector4(Color color) => new Vector4(color.R, color.G, color.B, color.A);

        void AnimatedIcon_Loaded(object sender, RoutedEventArgs e)
        {
            _controller = FindController();

            // This is our opportunity to hook up to the controller's events:
            if (Toggle)
            {
                this.AddHandler(PointerPressedEvent, new PointerEventHandler(Button_PointerPressed), true);
            }
            else if (_controller is ButtonBase button)
            {
                button.PointerEntered += Button_PointerEntered;
                button.PointerExited += Button_PointerExited;
                button.AddHandler(PointerPressedEvent, new PointerEventHandler(Button_PointerPressed), true);
            }
            else if (_controller is RangeBase rangeBase)
            {
                rangeBase.ValueChanged += RangeBase_ValueChanged;

                SyncProgressToRangeBaseController();
            }
            else
            {
                // If no controller is present, add events to AnimatedIcon itself.
                this.PointerEntered += AnimatedIcon_PointerEntered;
                this.PointerExited += AnimatedIcon_PointerExited;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {

            FrameworkElement child = (FrameworkElement)Children[0];
            child.Measure(availableSize);

            var fontSize = FontSize;
            return new Size(fontSize, fontSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            FrameworkElement child = (FrameworkElement)Children[0];
            child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));

            if (_lottieInstance != null)
            {
                var lottieNaturalSize = _lottieInstance.Size;

                // Calculate the scale that needs to be applied in order to make the
                // Lottie animation fit inside the given size.
                var scale = new Size(finalSize.Width / lottieNaturalSize.X, finalSize.Height / lottieNaturalSize.Y);

                // Adjust the scale to make it uniform (same scaling for width and height).
                var smallestScaleDimension = Math.Min(scale.Width, scale.Height);

                // Calculate the size of the Lottie animation after scaling.
                var scaledLottieSize = new Size(lottieNaturalSize.X * smallestScaleDimension, lottieNaturalSize.Y * smallestScaleDimension);

                // Scale the Lottie to fit.
                _rootVisual.Scale = new Vector3((float)smallestScaleDimension);

                // Center the animation within the available space.
                _rootVisual.Offset = new Vector3(
                                            (float)((finalSize.Width - scaledLottieSize.Width) / 2),
                                            (float)((finalSize.Height - scaledLottieSize.Height) / 2),
                                            0);
                return scaledLottieSize;
            }

            return finalSize;
            
        }

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

            animatedIcon._isMuteIcon = lottieGlyph == QaIcon.Mute;

            if (lottieGlyph == QaIcon.None)
            {
                // There is no Lottie animation for this text, or animations
                // are disabled. Just show it as text.
                animatedIcon.ShowText();
            }
            else
            {
                if (animatedIcon._isMuteIcon)
                {
                    // Special case - use the Volume icon for mute. It will
                    // be prevented from animating.
                    lottieGlyph = QaIcon.Volume;
                }

                animatedIcon.UpdateLottieSourceAndInstance(lottieGlyph);

                animatedIcon.ShowLottie();
            }
        }

        public bool Toggle
        {
            get { return (bool)GetValue(ToggleProperty); }
            set { SetValue(ToggleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Toggle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleProperty =
            DependencyProperty.Register("Toggle", typeof(bool), typeof(AnimatedIcon), new PropertyMetadata(false, new PropertyChangedCallback(OnToggleChanged)));

        private static void OnToggleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedIcon = (AnimatedIcon)d;

            if (animatedIcon.Toggle)
            {
                animatedIcon.UpdateToggleState();
            }
        }

        // Sets the progress of the Lottie animation to the value of
        // the RangeBase controller, if there is one.
        void SyncProgressToRangeBaseController()
        {
            if (_controller is RangeBase rangeBase)
            {
                if (_isMuteIcon)
                {
                    // Special-case the mute icon - show frame 0 of the Volume animation.
                    SetProgress(0);
                }
                else
                {
                    // Set the progress to the current value of the RangeBase.
                    var range = rangeBase.Maximum - rangeBase.Minimum;
                    SetProgress(rangeBase.Value / range);
                }
            }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(AnimatedIcon), new PropertyMetadata((double)20, new PropertyChangedCallback(OnFontSizeChanged)));

        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var animatedIcon = d as AnimatedIcon;
            animatedIcon.UpdateSize();
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(AnimatedIcon), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00)), new PropertyChangedCallback(OnForegroundChanged)));

        // Handles ForegroundProperty change notifications.
        // Sets the Foreground of the Lottie (if one exists) to the color
        // of the Foreground brush (if it's a SolidColorBrush).
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (AnimatedIcon)d;
            self.RegisterColorChangeCallback(self.Foreground as SolidColorBrush);
            self.UpdateLottieForegroundColor();
        }

        // Register with the given SolidColorBrush to receive notifications whenever
        // the Color property changes.
        void RegisterColorChangeCallback(SolidColorBrush colorBrush)
        {
            if (colorBrush != _foregroundSolidColorBrush)
            {
                UnregisterColorPropertyChangedCallback();
                _foregroundSolidColorBrush = colorBrush;
                if (colorBrush != null)
                {
                    _colorPropertyChangedRegistration =
                        _foregroundSolidColorBrush.RegisterPropertyChangedCallback(
                            SolidColorBrush.ColorProperty,
                            OnForegroundBrushColorChanged);
                }
            }
        }

        void UnregisterColorPropertyChangedCallback()
        {
            if (_colorPropertyChangedRegistration != 0)
            {
                _foregroundSolidColorBrush.UnregisterPropertyChangedCallback(
                    SolidColorBrush.ColorProperty,
                    _colorPropertyChangedRegistration);

                _foregroundSolidColorBrush = null;
            }
        }

        // Called for each change in the color of the Foreground brush.
        void OnForegroundBrushColorChanged(DependencyObject d, DependencyProperty p)
            => UpdateLottieForegroundColor();

        // Sets the color of the Lottie "Foreground" property to the color of
        // the Foreground brush, or to default black if the brush is not a
        // SolidColorBrush.
        void UpdateLottieForegroundColor()
        {
            if (_lottieSource != null)
            {
                var color = _foregroundSolidColorBrush is null ? Color.FromArgb(0xFF, 0x00, 0x00, 0x00) : _foregroundSolidColorBrush.Color;
                var themeProperties = _lottieSource.GetThemeProperties(Window.Current.Compositor);
                themeProperties.InsertVector4("Foreground", ColorAsVector4(color));
            }
        }

        public static DependencyProperty IsIconControllerProperty { get; } =
            DependencyProperty.RegisterAttached(
                "IsIconController",
                typeof(bool),
                typeof(AnimatedIcon),
                new PropertyMetadata(false));

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
        {
            if (Toggle)
            {
                UpdateToggleState();
            }
            else
            {
                FindAnimatedIcon((ButtonBase)sender)?.PlayOnce();
            }
        }

        // Handles ValueChanged on a controller that is a RangeBase (typically a Slider).
        static void RangeBase_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var rangeBase = (RangeBase)sender;
            var range = rangeBase.Maximum - rangeBase.Minimum;
            var animatedIcon = FindAnimatedIcon(rangeBase);
            if (animatedIcon != null && !animatedIcon._isMuteIcon)
            {
                animatedIcon.AnimateToProgress(e.OldValue / range, e.NewValue / range);
            }
        }

        private void AnimatedIcon_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((AnimatedIcon)sender)?.PlayOnce();

        }
        private void AnimatedIcon_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((AnimatedIcon)sender)?.Stop();
        }

        

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

        void UpdateSize()
        {
            // Adjust Panel children sizing based on FontSize.
            var fontSize = new Size(FontSize, FontSize);
            var size = MeasureOverride(fontSize);
            ArrangeOverride(size);

            // Adjust Panel Width.
            this.Width = FontSize;

            // Adjust Textblock icon font size
            TextBlock iconText = (TextBlock)Children[0];
            iconText.FontSize = FontSize;
            
        }

        void UpdateToggleState()
        {
            if (!_toggleIsPressed)
            {
                AnimateToProgress(0, 1);
                _toggleIsPressed = true;
            }
            else
            {
                AnimateToProgress(1, 0);
                _toggleIsPressed = false;
            }

        }

        // Sets the current Progress position of the Lottie animation.
        void SetProgress(double progress)
        {
            if (_lottieInstance != null)
            {
                var propertySet = _lottieInstance.RootVisual.Properties;
                propertySet.InsertScalar("Progress", (float)Math.Min(Math.Max(progress, 0), 1));
            }
        }

        // Animates from oldProgress to newProgress.
        void AnimateToProgress(double oldProgress, double newProgress)
        {
            if (_lottieInstance != null)
            {
                var propertySet = _lottieInstance.RootVisual.Properties;

                var diff = Math.Abs(oldProgress - newProgress);
                if (diff < 0.1)
                {
                    // It's a small change. Jump straight to the new position.
                    // This ensures that the icon feels responsive when the slider
                    // is dragged.
                    propertySet.InsertScalar("Progress", (float)Math.Min(Math.Max(newProgress, 0), 1));
                }
                else
                {
                    // It's a large change. This is usually caused by a click on the
                    // slider. Animate it to the new position.
                    // Get the current position. This might be out of date if the value is currently
                    // animating, but it's good enough.
                    var compositor = propertySet.Compositor;
                    using (var kfa = compositor.CreateScalarKeyFrameAnimation())
                    using (var easing = compositor.CreateLinearEasingFunction())
                    {
                        kfa.Duration = _lottieInstance.Duration * diff;
                        kfa.IterationBehavior = AnimationIterationBehavior.Count;
                        kfa.IterationCount = 1;
                        kfa.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
                        kfa.InsertKeyFrame(1, (float)newProgress, easing);
                        propertySet.StartAnimation("Progress", kfa);
                    }
                }
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

        // Shows the current Text value as text.
        void ShowText()
        {
            //Apply Text from GlyphProperty
            TextBlock iconText = (TextBlock)Children[0]; 
            iconText.FontFamily = new FontFamily("Segoe MDL2 Assets");
            iconText.Text = Glyph;

            // Make the TextBlock visible.
            iconText.Opacity = 1;

            // Hide the lottie animation.
            if (_rootVisual != null)
            {
                _rootVisual.IsVisible = false;
            }

            InvalidateMeasure();
        }

        void ShowLottie()
        {
            if (_lottieSource is null || _lottieInstance is null)
            {
                ShowText();
            }
            else
            {
                // Hide the static text block.
                TextBlock iconText = (TextBlock)Children[0];
                iconText.Opacity = 0;

                // Make the Lottie animation visible.
                _rootVisual.IsVisible = true;

                // Synchronize the foreground color with the current Foreground property value.
                UpdateLottieForegroundColor();

                // If there's a RangeBase controller sync Progress to its current value.
                SyncProgressToRangeBaseController();

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

                    if (_lottieInstance != null)
                    {
                        var rootVisualChildren = _rootVisual.Children;
                        rootVisualChildren.RemoveAll();
                        rootVisualChildren.InsertAtTop(_lottieInstance.RootVisual);
                    }
                }
            }
        }

        static ILottieVisualSource CreateLottieSourceForIcon(QaIcon icon)
        {
            switch (icon)
            {
                case QaIcon.AirplaneMode: return new AnimatedIconPrototype.QA_AirplaneMode();
                case QaIcon.Bluetooth: return new AnimatedIconPrototype.QA_BlueTooth();
                case QaIcon.Brightness: return new AnimatedIconPrototype.QA_Brightness();
                case QaIcon.Cellular: return new AnimatedIconPrototype.QA_Cellular();
                case QaIcon.ComposeMode: return new AnimatedIconPrototype.QA_ComposeMode();
                case QaIcon.DoNotDisturb: return new AnimatedIconPrototype.QA_DoNotDisturb();
                case QaIcon.EaseOfAccess: return new AnimatedIconPrototype.QA_EaseOfAccess();
                case QaIcon.InputLanguage: return new AnimatedIconPrototype.QA_InputLanguage();
                case QaIcon.Location: return new AnimatedIconPrototype.QA_Location();
                case QaIcon.Project: return new AnimatedIconPrototype.QA_Project();
                case QaIcon.RotationLock: return new AnimatedIconPrototype.QA_RotationLock();
                case QaIcon.Volume: return new AnimatedIconPrototype.QA_Volume();
                case QaIcon.Wifi: return new AnimatedIconPrototype.QA_Wifi();
                case QaIcon.FingerHello: return new AnimatedIconPrototype.FingerHello();
                case QaIcon.Heart: return new AnimatedIconPrototype.Heart();
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
                    case QaIcon.FingerHello:
                    case QaIcon.Heart:
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
            FingerHello = 0xE928,
            Heart = 0xEB51,
        }
    }
}
