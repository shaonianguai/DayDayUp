﻿using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using WzUXRibbon.Enumerations;
using WzUXRibbon.Internal.KnownBoxes;
using WzUXRibbon.Internal;
using WzUXRibbon.Effects;
using WzUXRibbon.Converters;

namespace WzUXRibbon.Controls
{
    public class IconPresenter : ContentControl
    {
#pragma warning disable WPF0010
        /// <summary>Identifies the <see cref="IconSize"/> dependency property.</summary>
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize), typeof(IconSize), typeof(IconPresenter), new PropertyMetadata(IconSizeBoxes.Small, PropertyChangedCallback));
#pragma warning restore WPF0010

        /// <summary>Identifies the <see cref="SmallSize"/> dependency property.</summary>
        public static readonly DependencyProperty SmallSizeProperty = DependencyProperty.Register(
            nameof(SmallSize), typeof(Size), typeof(IconPresenter), new PropertyMetadata(new Size(16, 16), PropertyChangedCallback));

        /// <summary>Identifies the <see cref="MediumSize"/> dependency property.</summary>
        public static readonly DependencyProperty MediumSizeProperty = DependencyProperty.Register(
            nameof(MediumSize), typeof(Size), typeof(IconPresenter), new PropertyMetadata(new Size(24, 24), PropertyChangedCallback));

        /// <summary>Identifies the <see cref="LargeSize"/> dependency property.</summary>
        public static readonly DependencyProperty LargeSizeProperty = DependencyProperty.Register(
            nameof(LargeSize), typeof(Size), typeof(IconPresenter), new PropertyMetadata(new Size(32, 32), PropertyChangedCallback));

        /// <summary>Identifies the <see cref="CustomSize"/> dependency property.</summary>
        public static readonly DependencyProperty CustomSizeProperty = DependencyProperty.Register(
            nameof(CustomSize), typeof(Size), typeof(IconPresenter), new PropertyMetadata(default(Size)));

        /// <summary>Identifies the <see cref="SmallIcon"/> dependency property.</summary>
        public static readonly DependencyProperty SmallIconProperty = DependencyProperty.Register(
            nameof(SmallIcon), typeof(object), typeof(IconPresenter), new PropertyMetadata(default, PropertyChangedCallback));

        /// <summary>Identifies the <see cref="MediumIcon"/> dependency property.</summary>
        public static readonly DependencyProperty MediumIconProperty = DependencyProperty.Register(
            nameof(MediumIcon), typeof(object), typeof(IconPresenter), new PropertyMetadata(default, PropertyChangedCallback));

        /// <summary>Identifies the <see cref="LargeIcon"/> dependency property.</summary>
        public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(
            nameof(LargeIcon), typeof(object), typeof(IconPresenter), new PropertyMetadata(default, PropertyChangedCallback));

        /// <summary>Identifies the <see cref="OptimalIcon"/> dependency property.</summary>
        public static readonly DependencyProperty OptimalIconProperty = DependencyProperty.Register(
            nameof(OptimalIcon), typeof(object), typeof(IconPresenter), new PropertyMetadata(default));

        /// <summary>Identifies the <see cref="CurrentIconSizeSize"/> dependency property.</summary>
        public static readonly DependencyProperty CurrentIconSizeSizeProperty = DependencyProperty.Register(
            nameof(CurrentIconSizeSize), typeof(Size), typeof(IconPresenter), new PropertyMetadata(new Size(16, 16)));

        private GrayscaleEffect grayscaleEffect = null;

        static IconPresenter()
        {
            SnapsToDevicePixelsProperty.OverrideMetadata(typeof(IconPresenter), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

            IsEnabledProperty.OverrideMetadata(typeof(IconPresenter), new UIPropertyMetadata(OnIsEnabledChanged));

            FocusableProperty.OverrideMetadata(typeof(IconPresenter), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
        }

        public IconPresenter()
        {
            var multiBinding = new MultiBinding { Converter = StaticConverters.ObjectToImageConverter };
            multiBinding.Bindings.Add(new Binding(nameof(this.OptimalIcon)) { Source = this });
            multiBinding.Bindings.Add(new Binding(nameof(this.CurrentIconSizeSize)) { Source = this });
            multiBinding.Bindings.Add(new Binding { Source = this });

            this.SetBinding(ContentProperty, multiBinding);

            this.UpdateSize();
        }

        public Size CurrentIconSizeSize
        {
            get => (Size)this.GetValue(CurrentIconSizeSizeProperty);
            set => this.SetValue(CurrentIconSizeSizeProperty, value);
        }

        public Size SmallSize
        {
            get => (Size)this.GetValue(SmallSizeProperty);
            set => this.SetValue(SmallSizeProperty, value);
        }

        public Size MediumSize
        {
            get => (Size)this.GetValue(MediumSizeProperty);
            set => this.SetValue(MediumSizeProperty, value);
        }

        public Size LargeSize
        {
            get => (Size)this.GetValue(LargeSizeProperty);
            set => this.SetValue(LargeSizeProperty, value);
        }

        public Size CustomSize
        {
            get => (Size)this.GetValue(CustomSizeProperty);
            set => this.SetValue(CustomSizeProperty, value);
        }

        public IconSize IconSize
        {
            get => (IconSize)this.GetValue(IconSizeProperty);
            set => this.SetValue(IconSizeProperty, IconSizeBoxes.Box(value));
        }

        public object SmallIcon
        {
            get => this.GetValue(SmallIconProperty);
            set => this.SetValue(SmallIconProperty, value);
        }

        public object MediumIcon
        {
            get => this.GetValue(MediumIconProperty);
            set => this.SetValue(MediumIconProperty, value);
        }

        public object LargeIcon
        {
            get => this.GetValue(LargeIconProperty);
            set => this.SetValue(LargeIconProperty, value);
        }

        public object OptimalIcon
        {
            get => this.GetValue(OptimalIconProperty);
            set => this.SetValue(OptimalIconProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (IconPresenter)d;
            var newValue = (bool)e.NewValue;

            if (newValue)
            {
                control.Effect = null;
            }
            else
            {
                if (control.grayscaleEffect != null)
                {
                    control.Effect = control.grayscaleEffect;
                }
                else
                {
                    control.Effect = new GrayscaleEffect();
                }
            }

            control.Opacity = newValue ? 1 : 0.5;
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (IconPresenter)d;
            control.Update();
        }

        private void Update()
        {
            this.UpdateSize();

            var optimalIcon = this.GetOptimalIcon();

            if (this.OptimalIcon != optimalIcon)
            {
                this.SetCurrentValue(OptimalIconProperty, optimalIcon);
            }
        }

        private void UpdateSize()
        {
            System.Windows.Size size;
            switch (this.IconSize)
            {
                case IconSize.Small:
                    size = this.SmallSize;
                    break;
                case IconSize.Medium:
                    size = this.MediumSize;
                    break;
                case IconSize.Large:
                    size = this.LargeSize;
                    break;
                case IconSize.Custom:
                    size = this.CustomSize;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.IconSize), this.IconSize, null);
            }


            if (DoubleUtil.AreClose(this.Width, size.Width) == false)
            {
                this.SetCurrentValue(WidthProperty, size.Width);
            }

            if (DoubleUtil.AreClose(this.Height, size.Height) == false)
            {
                this.SetCurrentValue(HeightProperty, size.Height);
            }

            this.SetCurrentValue(CurrentIconSizeSizeProperty, size);
        }

        public object GetOptimalIcon()
        {
            if (this.IconSize == IconSize.Small)
            {
                if (this.SmallIcon != null)
                {
                    return this.SmallIcon;
                }

                if (this.MediumIcon != null)
                {
                    return this.MediumIcon;
                }

                if (this.LargeIcon != null)
                {
                    return this.LargeIcon;
                }
            }
            else if (this.IconSize == IconSize.Medium)
            {
                if (this.MediumIcon != null)
                {
                    return this.MediumIcon;
                }

                if (this.LargeIcon != null)
                {
                    return this.LargeIcon;
                }

                if (this.SmallIcon != null)
                {
                    return this.SmallIcon;
                }

            }
            else
            {
                if (this.LargeIcon != null)
                {
                    return this.LargeIcon;
                }

                if (this.MediumIcon != null)
                {
                    return this.MediumIcon;
                }

                if (this.SmallIcon != null)
                {
                    return this.SmallIcon;
                }
            }

            return this.SmallIcon;
        }
    }
}
