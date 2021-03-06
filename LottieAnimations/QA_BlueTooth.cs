﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//       LottieGen version:
//           6.1.0-build.109+gebc2a6a0a3
//       
//       Command:
//           LottieGen -MinimumUapVersion 11 -Namespace WindowsInternal.ComposableShell.Experiences -Interface ILottieVisual -Language CSharp -InputFile QA_BlueTooth.json
//       
//       Input file:
//           QA_BlueTooth.json (7620 bytes created 10:10-07:00 Mar 30 2020)
//       
//       Invoked on:
//           SIM10FRESH @ 17:59-07:00 Apr 1 2020
//       
//       LottieGen source:
//           http://aka.ms/Lottie
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// ____________________________________
// |       Object stats       | Count |
// |__________________________|_______|
// | All CompositionObjects   |    89 |
// |--------------------------+-------|
// | Animators                |    19 |
// | Animated brushes         |     1 |
// | Animated gradient stops  |     - |
// | ExpressionAnimations     |     2 |
// | PathKeyFrameAnimations   |     - |
// |--------------------------+-------|
// | ContainerVisuals         |     1 |
// | ShapeVisuals             |     1 |
// |--------------------------+-------|
// | ContainerShapes          |     8 |
// | CompositionSpriteShapes  |     5 |
// |--------------------------+-------|
// | Brushes                  |     1 |
// | Gradient stops           |     - |
// | CompositionVisualSurface |     - |
// ------------------------------------
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;

namespace AnimatedIconPrototype
{
    // Name:        QA_BlueTooth
    // Frame rate:  60 fps
    // Frame count: 60
    // Duration:    1000.0 mS
    // ____________________________________________
    // | Theme property | Type  |  Default value  |
    // |________________|_______|_________________|
    // | Foreground     | Color | #FF000000 Black |
    // --------------------------------------------
    sealed class QA_BlueTooth : ILottieVisualSource
    {
        // Animation duration: 1.000 seconds.
        internal const long c_durationTicks = 10000000;

        // Theme property: Foreground.
        internal static readonly Color c_themeForeground = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);

        CompositionPropertySet _themeProperties;
        Color _themeForeground = c_themeForeground;

        // Theme properties.
        public Color Foreground
        {
            get => _themeForeground;
            set
            {
                _themeForeground = value;
                if (_themeProperties != null)
                {
                    _themeProperties.InsertVector4("Foreground", ColorAsVector4((Color)_themeForeground));
                }
            }
        }

        public CompositionPropertySet GetThemeProperties(Compositor compositor)
        {
            return EnsureThemeProperties(compositor);
        }

        internal static Vector4 ColorAsVector4(Color color) => new Vector4(color.R, color.G, color.B, color.A);

        CompositionPropertySet EnsureThemeProperties(Compositor compositor)
        {
            if (_themeProperties is null)
            {
                _themeProperties = compositor.CreatePropertySet();
                _themeProperties.InsertVector4("Foreground", ColorAsVector4((Color)Foreground));
            }
            return _themeProperties;
        }

        public ILottieVisual TryCreateAnimatedVisual(Compositor compositor, out object diagnostics)
        {
            diagnostics = null;
            EnsureThemeProperties(compositor);

            if (AnimatedVisual.IsRuntimeCompatible())
            {
                return
                    new AnimatedVisual(
                        compositor,
                        _themeProperties
                        );
            }

            return null;
        }

        sealed class AnimatedVisual : ILottieVisual
        {
            const long c_durationTicks = 10000000;
            readonly Compositor _c;
            readonly ExpressionAnimation _reusableExpressionAnimation;
            readonly CompositionPropertySet _themeProperties;
            CompositionColorBrush _themeColor_Foreground;
            CompositionRectangleGeometry _rectangle_6p999;
            ContainerVisual _root;
            CubicBezierEasingFunction _cubicBezierEasingFunction_0;
            CubicBezierEasingFunction _cubicBezierEasingFunction_1;
            CubicBezierEasingFunction _cubicBezierEasingFunction_2;
            ExpressionAnimation _rootProgress;
            StepEasingFunction _holdThenStepEasingFunction;
            StepEasingFunction _stepThenHoldEasingFunction;

            static void StartProgressBoundAnimation(
                CompositionObject target,
                string animatedPropertyName,
                CompositionAnimation animation,
                ExpressionAnimation controllerProgressExpression)
            {
                target.StartAnimation(animatedPropertyName, animation);
                var controller = target.TryGetAnimationController(animatedPropertyName);
                controller.Pause();
                controller.StartAnimation("Progress", controllerProgressExpression);
            }

            void BindProperty(
                CompositionObject target,
                string animatedPropertyName,
                string expression,
                string referenceParameterName,
                CompositionObject referencedObject)
            {
                _reusableExpressionAnimation.ClearAllParameters();
                _reusableExpressionAnimation.Expression = expression;
                _reusableExpressionAnimation.SetReferenceParameter(referenceParameterName, referencedObject);
                target.StartAnimation(animatedPropertyName, _reusableExpressionAnimation);
            }

            Vector2KeyFrameAnimation CreateVector2KeyFrameAnimation(float initialProgress, Vector2 initialValue, CompositionEasingFunction initialEasingFunction)
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(c_durationTicks);
                result.InsertKeyFrame(initialProgress, initialValue, initialEasingFunction);
                return result;
            }

            CompositionSpriteShape CreateSpriteShape(CompositionGeometry geometry, Matrix3x2 transformMatrix, CompositionBrush fillBrush)
            {
                var result = _c.CreateSpriteShape(geometry);
                result.TransformMatrix = transformMatrix;
                result.FillBrush = fillBrush;
                return result;
            }

            // - - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - - ShapeGroup: Group 1
            // - Path 3+Path 2+Path 1.PathGeometry
            CanvasGeometry Geometry_0()
            {
                var result = CanvasGeometry.CreateGroup(
                    null,
                    new CanvasGeometry[] { Geometry_1(), Geometry_2(), Geometry_3() },
                    CanvasFilledRegionDetermination.Winding);
                return result;
            }

            // - - - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - - - ShapeGroup: Group 1
            // - - Path 3+Path 2+Path 1.PathGeometry
            // Path 3
            CanvasGeometry Geometry_1()
            {
                CanvasGeometry result;
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(-1.02699995F, 37.8969994F));
                    builder.AddLine(new Vector2(-1.02699995F, 4.61600018F));
                    builder.AddLine(new Vector2(-15.7189999F, 19.2649994F));
                    builder.AddLine(new Vector2(-19.6189995F, 15.3640003F));
                    builder.AddLine(new Vector2(-2.19700003F, -2.10100007F));
                    builder.AddLine(new Vector2(-19.6189995F, -19.5669994F));
                    builder.AddLine(new Vector2(-15.7189999F, -23.4680004F));
                    builder.AddLine(new Vector2(-1.02699995F, -8.81900024F));
                    builder.AddLine(new Vector2(-1.02699995F, -42.1030006F));
                    builder.AddLine(new Vector2(22.2889996F, -18.7439995F));
                    builder.AddLine(new Vector2(5.64699984F, -2.10100007F));
                    builder.AddLine(new Vector2(22.2889996F, 14.5410004F));
                    builder.AddLine(new Vector2(-1.02699995F, 37.8969994F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    result = CanvasGeometry.CreatePath(builder);
                }
                return result;
            }

            // - - - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - - - ShapeGroup: Group 1
            // - - Path 3+Path 2+Path 1.PathGeometry
            // Path 2
            CanvasGeometry Geometry_2()
            {
                CanvasGeometry result;
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(4.51999998F, -28.6679993F));
                    builder.AddLine(new Vector2(4.51999998F, -8.81900024F));
                    builder.AddLine(new Vector2(14.4449997F, -18.7439995F));
                    builder.AddLine(new Vector2(4.51999998F, -28.6679993F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    result = CanvasGeometry.CreatePath(builder);
                }
                return result;
            }

            // - - - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - - - ShapeGroup: Group 1
            // - - Path 3+Path 2+Path 1.PathGeometry
            // Path 1
            CanvasGeometry Geometry_3()
            {
                CanvasGeometry result;
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(4.51999998F, 4.61600018F));
                    builder.AddLine(new Vector2(4.51999998F, 24.4619999F));
                    builder.AddLine(new Vector2(14.4449997F, 14.5410004F));
                    builder.AddLine(new Vector2(4.51999998F, 4.61600018F));
                    builder.EndFigure(CanvasFigureLoop.Open);
                    result = CanvasGeometry.CreatePath(builder);
                }
                return result;
            }

            // Color bound to theme property value: Foreground
            CompositionColorBrush ThemeColor_Foreground()
            {
                var result = _themeColor_Foreground = _c.CreateColorBrush();
                BindProperty(result, "Color", "ColorRGB(_theme.Foreground.W*1,_theme.Foreground.X,_theme.Foreground.Y,_theme.Foreground.Z)", "_theme", _themeProperties);
                return result;
            }

            // PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 3
            CompositionContainerShape ContainerShape_0()
            {
                var result = _c.CreateContainerShape();
                result.Scale = new Vector2(0F, 0F);
                result.Shapes.Add(ContainerShape_1());
                StartProgressBoundAnimation(result, "Scale", ShapeVisibilityAnimation_0(), _rootProgress);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 3
            CompositionContainerShape ContainerShape_1()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(-40.25F, 0.25F);
                result.Offset = new Vector2(130.5F, 50F);
                // ShapeGroup: Rectangle 1 Scale:1.32554, Offset:<-40.25, 0.25>
                result.Shapes.Add(SpriteShape_1());
                StartProgressBoundAnimation(result, "Scale", ScaleVector2Animation_1(), _rootProgress);
                return result;
            }

            // PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 4
            CompositionContainerShape ContainerShape_2()
            {
                var result = _c.CreateContainerShape();
                result.Scale = new Vector2(0F, 0F);
                result.Shapes.Add(ContainerShape_3());
                StartProgressBoundAnimation(result, "Scale", ShapeVisibilityAnimation_1(), _rootProgress);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 4
            CompositionContainerShape ContainerShape_3()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(-40.25F, 0.25F);
                result.Offset = new Vector2(112.25F, 50F);
                // ShapeGroup: Rectangle 1 Scale:1.32554, Offset:<-40.25, 0.25>
                result.Shapes.Add(SpriteShape_2());
                StartProgressBoundAnimation(result, "Scale", ScaleVector2Animation_2(), _rootProgress);
                return result;
            }

            // PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 1
            CompositionContainerShape ContainerShape_4()
            {
                var result = _c.CreateContainerShape();
                result.Scale = new Vector2(0F, 0F);
                result.Shapes.Add(ContainerShape_5());
                StartProgressBoundAnimation(result, "Scale", ShapeVisibilityAnimation_2(), _rootProgress);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 1
            CompositionContainerShape ContainerShape_5()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(-40.25F, 0.25F);
                result.Offset = new Vector2(68F, 50F);
                // ShapeGroup: Rectangle 1 Scale:1.32554, Offset:<-40.25, 0.25>
                result.Shapes.Add(SpriteShape_3());
                StartProgressBoundAnimation(result, "Scale", ScaleVector2Animation_3(), _rootProgress);
                return result;
            }

            // PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 2
            CompositionContainerShape ContainerShape_6()
            {
                var result = _c.CreateContainerShape();
                result.Shapes.Add(ContainerShape_7());
                StartProgressBoundAnimation(result, "Scale", ShapeVisibilityAnimation_3(), _rootProgress);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 2
            CompositionContainerShape ContainerShape_7()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(-40.25F, 0.25F);
                result.Offset = new Vector2(49.75F, 50F);
                // ShapeGroup: Rectangle 1 Scale:1.32554, Offset:<-40.25, 0.25>
                result.Shapes.Add(SpriteShape_4());
                StartProgressBoundAnimation(result, "Scale", ScaleVector2Animation_4(), _rootProgress);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // ShapeGroup: Group 1
            // Path 3+Path 2+Path 1.PathGeometry
            CompositionPathGeometry PathGeometry()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(Geometry_0()));
                return result;
            }

            // Rectangle Path 1.RectangleGeometry
            CompositionRectangleGeometry Rectangle_6p999()
            {
                var result = _rectangle_6p999 = _c.CreateRectangleGeometry();
                result.Offset = new Vector2(-3.49950004F, -3.49950004F);
                result.Size = new Vector2(6.99900007F, 6.99900007F);
                return result;
            }

            // PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Path 3+Path 2+Path 1
            CompositionSpriteShape SpriteShape_0()
            {
                var result = _c.CreateSpriteShape(PathGeometry());
                result.Offset = new Vector2(50F, 50F);
                result.FillBrush = ThemeColor_Foreground();
                StartProgressBoundAnimation(result, "Scale", ScaleVector2Animation_0(), RootProgress());
                return result;
            }

            // - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - Layer: Shape Layer 3
            // Rectangle Path 1
            CompositionSpriteShape SpriteShape_1()
            {
                // Offset:<-40.25, 0.25>, Scale:<1.32554, 1.32554>
                var result = CreateSpriteShape(Rectangle_6p999(), new Matrix3x2(1.32553995F, 0F, 0F, 1.32553995F, -40.25F, 0.25F), _themeColor_Foreground);
                return result;
            }

            // - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - Layer: Shape Layer 4
            // Rectangle Path 1
            CompositionSpriteShape SpriteShape_2()
            {
                // Offset:<-40.25, 0.25>, Scale:<1.32554, 1.32554>
                var result = CreateSpriteShape(_rectangle_6p999, new Matrix3x2(1.32553995F, 0F, 0F, 1.32553995F, -40.25F, 0.25F), _themeColor_Foreground);
                return result;
            }

            // - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - Layer: Shape Layer 1
            // Rectangle Path 1
            CompositionSpriteShape SpriteShape_3()
            {
                // Offset:<-40.25, 0.25>, Scale:<1.32554, 1.32554>
                var result = CreateSpriteShape(_rectangle_6p999, new Matrix3x2(1.32553995F, 0F, 0F, 1.32553995F, -40.25F, 0.25F), _themeColor_Foreground);
                return result;
            }

            // - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - Layer: Shape Layer 2
            // Rectangle Path 1
            CompositionSpriteShape SpriteShape_4()
            {
                // Offset:<-40.25, 0.25>, Scale:<1.32554, 1.32554>
                var result = CreateSpriteShape(_rectangle_6p999, new Matrix3x2(1.32553995F, 0F, 0F, 1.32553995F, -40.25F, 0.25F), _themeColor_Foreground);
                return result;
            }

            // The root of the composition.
            ContainerVisual Root()
            {
                var result = _root = _c.CreateContainerVisual();
                var propertySet = result.Properties;
                propertySet.InsertScalar("Progress", 0F);
                // PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
                result.Children.InsertAtTop(ShapeVisual_0());
                return result;
            }

            CubicBezierEasingFunction CubicBezierEasingFunction_0()
            {
                return _cubicBezierEasingFunction_0 = _c.CreateCubicBezierEasingFunction(new Vector2(0.850000024F, 0F), new Vector2(0.75F, 1F));
            }

            CubicBezierEasingFunction CubicBezierEasingFunction_1()
            {
                return _cubicBezierEasingFunction_1 = _c.CreateCubicBezierEasingFunction(new Vector2(0.349999994F, 0F), new Vector2(0F, 1F));
            }

            CubicBezierEasingFunction CubicBezierEasingFunction_2()
            {
                return _cubicBezierEasingFunction_2 = _c.CreateCubicBezierEasingFunction(new Vector2(0.850000024F, 0F), new Vector2(0.833000004F, 0.833000004F));
            }

            ExpressionAnimation RootProgress()
            {
                var result = _rootProgress = _c.CreateExpressionAnimation("_.Progress");
                result.SetReferenceParameter("_", _root);
                return result;
            }

            // Layer aggregator
            ShapeVisual ShapeVisual_0()
            {
                var result = _c.CreateShapeVisual();
                result.Size = new Vector2(100F, 100F);
                // Offset:<-2, -2>, Scale:<0.2, 0.2>
                result.TransformMatrix = new Matrix4x4(0.200000003F, 0F, 0F, 0F, 0F, 0.200000003F, 0F, 0F, 0F, 0F, 0F, 0F, -2F, -2F, 0F, 1F);
                var shapes = result.Shapes;
                // ShapeGroup: Group 1
                shapes.Add(SpriteShape_0());
                // Layer: Shape Layer 3
                shapes.Add(ContainerShape_0());
                // Layer: Shape Layer 4
                shapes.Add(ContainerShape_2());
                // Layer: Shape Layer 1
                shapes.Add(ContainerShape_4());
                // Layer: Shape Layer 2
                shapes.Add(ContainerShape_6());
                return result;
            }

            StepEasingFunction HoldThenStepEasingFunction()
            {
                var result = _holdThenStepEasingFunction = _c.CreateStepEasingFunction();
                result.IsFinalStepSingleFrame = true;
                return result;
            }

            StepEasingFunction StepThenHoldEasingFunction()
            {
                var result = _stepThenHoldEasingFunction = _c.CreateStepEasingFunction();
                result.IsInitialStepSingleFrame = true;
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // ShapeGroup: Group 1
            // Scale
            Vector2KeyFrameAnimation ScaleVector2Animation_0()
            {
                var result = CreateVector2KeyFrameAnimation(0F, new Vector2(1F, 1F), HoldThenStepEasingFunction());
                result.InsertKeyFrame(0.5F, new Vector2(0.800000012F, 0.800000012F), _c.CreateCubicBezierEasingFunction(new Vector2(0.166999996F, 0.166999996F), new Vector2(0F, 1F)));
                result.InsertKeyFrame(0.699999988F, new Vector2(1.10000002F, 1.10000002F), CubicBezierEasingFunction_0());
                result.InsertKeyFrame(0.983333349F, new Vector2(1F, 1F), CubicBezierEasingFunction_1());
                return result;
            }

            // - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - Layer: Shape Layer 3
            // Scale
            Vector2KeyFrameAnimation ScaleVector2Animation_1()
            {
                var result = CreateVector2KeyFrameAnimation(0F, new Vector2(0F, 0F), StepThenHoldEasingFunction());
                result.InsertKeyFrame(0.200000003F, new Vector2(0F, 0F), _holdThenStepEasingFunction);
                result.InsertKeyFrame(0.366666675F, new Vector2(1.75F, 1.75F), _cubicBezierEasingFunction_0);
                result.InsertKeyFrame(0.533333361F, new Vector2(1F, 1F), _cubicBezierEasingFunction_1);
                result.InsertKeyFrame(0.850000024F, new Vector2(0F, 0F), CubicBezierEasingFunction_2());
                return result;
            }

            // - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - Layer: Shape Layer 4
            // Scale
            Vector2KeyFrameAnimation ScaleVector2Animation_2()
            {
                var result = CreateVector2KeyFrameAnimation(0F, new Vector2(0F, 0F), _stepThenHoldEasingFunction);
                result.InsertKeyFrame(0.13333334F, new Vector2(0F, 0F), _holdThenStepEasingFunction);
                result.InsertKeyFrame(0.300000012F, new Vector2(1.75F, 1.75F), _cubicBezierEasingFunction_0);
                result.InsertKeyFrame(0.466666669F, new Vector2(1F, 1F), _cubicBezierEasingFunction_1);
                result.InsertKeyFrame(0.783333361F, new Vector2(0F, 0F), _cubicBezierEasingFunction_2);
                return result;
            }

            // - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - Layer: Shape Layer 1
            // Scale
            Vector2KeyFrameAnimation ScaleVector2Animation_3()
            {
                var result = CreateVector2KeyFrameAnimation(0F, new Vector2(0F, 0F), _stepThenHoldEasingFunction);
                result.InsertKeyFrame(0.0666666701F, new Vector2(0F, 0F), _holdThenStepEasingFunction);
                result.InsertKeyFrame(0.233333334F, new Vector2(1.75F, 1.75F), _cubicBezierEasingFunction_0);
                result.InsertKeyFrame(0.400000006F, new Vector2(1F, 1F), _cubicBezierEasingFunction_1);
                result.InsertKeyFrame(0.716666639F, new Vector2(0F, 0F), _cubicBezierEasingFunction_2);
                return result;
            }

            // - - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // - Layer: Shape Layer 2
            // Scale
            Vector2KeyFrameAnimation ScaleVector2Animation_4()
            {
                var result = CreateVector2KeyFrameAnimation(0F, new Vector2(0F, 0F), _holdThenStepEasingFunction);
                result.InsertKeyFrame(0.166666672F, new Vector2(1.75F, 1.75F), _cubicBezierEasingFunction_0);
                result.InsertKeyFrame(0.333333343F, new Vector2(1F, 1F), _cubicBezierEasingFunction_1);
                result.InsertKeyFrame(0.649999976F, new Vector2(0F, 0F), _cubicBezierEasingFunction_2);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 3
            Vector2KeyFrameAnimation ShapeVisibilityAnimation_0()
            {
                var result = CreateVector2KeyFrameAnimation(0.200000003F, new Vector2(1F, 1F), _holdThenStepEasingFunction);
                result.InsertKeyFrame(0.850000024F, new Vector2(0F, 0F), _holdThenStepEasingFunction);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 4
            Vector2KeyFrameAnimation ShapeVisibilityAnimation_1()
            {
                var result = CreateVector2KeyFrameAnimation(0.13333334F, new Vector2(1F, 1F), _holdThenStepEasingFunction);
                result.InsertKeyFrame(0.783333361F, new Vector2(0F, 0F), _holdThenStepEasingFunction);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 1
            Vector2KeyFrameAnimation ShapeVisibilityAnimation_2()
            {
                var result = CreateVector2KeyFrameAnimation(0.0666666701F, new Vector2(1F, 1F), _holdThenStepEasingFunction);
                result.InsertKeyFrame(0.716666639F, new Vector2(0F, 0F), _holdThenStepEasingFunction);
                return result;
            }

            // - PreComp layer: Asset - AnimIcon - Action Center - BlueTooth
            // Layer: Shape Layer 2
            Vector2KeyFrameAnimation ShapeVisibilityAnimation_3()
            {
                var result = CreateVector2KeyFrameAnimation(0.649999976F, new Vector2(0F, 0F), _holdThenStepEasingFunction);
                return result;
            }

            internal AnimatedVisual(
                Compositor compositor,
                CompositionPropertySet themeProperties
                )
            {
                _c = compositor;
                _themeProperties = themeProperties;
                _reusableExpressionAnimation = compositor.CreateExpressionAnimation();
                Root();
            }

            public Visual RootVisual => _root;
            public TimeSpan Duration => TimeSpan.FromTicks(c_durationTicks);
            public Vector2 Size => new Vector2(16F, 16F);
            void IDisposable.Dispose() => _root?.Dispose();

            internal static bool IsRuntimeCompatible()
            {
                return Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8);
            }
        }
    }
}
