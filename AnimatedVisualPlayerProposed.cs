﻿using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml;

namespace ProgressUIPrototype
{
    public class AnimatedVisualPlayerProposed : AnimatedVisualPlayer
    {
        public AnimatedVisualPlayerProposed()
        {
            // some issue setting this in markup.. so hacking this for now.
            AutoPlay = false;
        }

        public bool IsLooping
        {
            get { return (bool)GetValue(IsLoopingProperty); }
            set { SetValue(IsLoopingProperty, value); }
        }

        public static readonly DependencyProperty IsLoopingProperty =
            DependencyProperty.Register("IsLooping", typeof(bool), typeof(AnimatedVisualPlayerProposed), new PropertyMetadata(false));

        public double StartPosition
        {
            get { return (double)GetValue(StartPositionProperty); }
            set { SetValue(StartPositionProperty, value); }
        }

        public static readonly DependencyProperty StartPositionProperty =
            DependencyProperty.Register("StartPosition", typeof(double), typeof(AnimatedVisualPlayerProposed), new PropertyMetadata(0.0));

        public double StopPosition
        {
            get { return (double)GetValue(StopPositionProperty); }
            set { SetValue(StopPositionProperty, value); }
        }

        public static readonly DependencyProperty StopPositionProperty =
            DependencyProperty.Register("StopPosition", typeof(double), typeof(AnimatedVisualPlayerProposed), new PropertyMetadata(1.0));

        public new bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        public static readonly new DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(AnimatedVisualPlayerProposed), new PropertyMetadata(false, new PropertyChangedCallback(OnIsPlayingChanged)));

        private static void OnIsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var player = d as AnimatedVisualPlayerProposed;
                _ = player.PlayAsync(player.StartPosition, player.StopPosition, player.IsLooping);
            }
        }

        public double Position
        {
            get { return (double)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(double), typeof(AnimatedVisualPlayerProposed), new PropertyMetadata(0, new PropertyChangedCallback(OnPositionChanged)));

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var player = d as AnimatedVisualPlayer;
            var value = (double)e.NewValue;

            if (player.IsAnimatedVisualLoaded)
            {
                player.SetProgress(value);
            }
     
        }

        public new IAnimatedVisualSource Source
        {
            get { return (IAnimatedVisualSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly new DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IAnimatedVisualSource), typeof(AnimatedVisualPlayerProposed), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var player = d as AnimatedVisualPlayer;
            player.Source = e.NewValue as IAnimatedVisualSource;
        }

        public new bool IsAnimatedVisualLoaded
        {
            get { return (bool)GetValue(IsAnimatedVisualLoadedProperty); }
            set { SetValue(IsAnimatedVisualLoadedProperty, value); }
        }

        public static readonly new DependencyProperty IsAnimatedVisualLoadedProperty =
            DependencyProperty.Register("IsAnimatedVisualLoaded", typeof(bool), typeof(AnimatedVisualPlayerProposed), new PropertyMetadata(null, new PropertyChangedCallback(OnIsAnimatedVisualLoadedChanged)));

        private static void OnIsAnimatedVisualLoadedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var player = d as AnimatedVisualPlayerProposed;
            var value = (double)e.NewValue;
            //player.SetProgress(value);
        }
    }
}
