﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using WzUXRibbon.AttachedProperties;
using WzUXRibbon.Internal;

namespace WzUXRibbon.Converters
{
    public struct DpiScale
    {
        public DpiScale(double dpiScaleX, double dpiScaleY)
        {
            _dpiScaleX = dpiScaleX;
            _dpiScaleY = dpiScaleY;
        }

        public double DpiScaleX
        {
            get { return _dpiScaleX; }
        }

        public double DpiScaleY
        {
            get { return _dpiScaleY; }
        }

        public double PixelsPerDip
        {
            get { return _dpiScaleY; }
        }

        internal bool Equals(DpiScale other) => _dpiScaleX == other._dpiScaleX && _dpiScaleY == other._dpiScaleY;

        private readonly double _dpiScaleX;
        private readonly double _dpiScaleY;
    }

    [MarkupExtensionReturnType(typeof(ImageSource))]
    [ValueConversion(sourceType: typeof(string), targetType: typeof(System.Windows.Controls.Image))]
    [ValueConversion(sourceType: typeof(Uri), targetType: typeof(System.Windows.Controls.Image))]
    [ValueConversion(sourceType: typeof(System.Drawing.Icon), targetType: typeof(System.Windows.Controls.Image))]
    [ValueConversion(sourceType: typeof(ImageSource), targetType: typeof(System.Windows.Controls.Image))]
    [ValueConversion(sourceType: typeof(string), targetType: typeof(ImageSource))]
    [ValueConversion(sourceType: typeof(Uri), targetType: typeof(ImageSource))]
    [ValueConversion(sourceType: typeof(System.Drawing.Icon), targetType: typeof(ImageSource))]
    [ValueConversion(sourceType: typeof(ImageSource), targetType: typeof(ImageSource))]
    public class ObjectToImageConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static readonly ImageSource imageNotFoundImageSource = (ImageSource)CreateImageNotFoundImageSource().GetAsFrozen();
        private static readonly SizeConverter sizeConverter = new SizeConverter();

        public ObjectToImageConverter()
        {
        }

        public ObjectToImageConverter(object input)
            : this(input, Size.Empty, null)
        {
        }

        public ObjectToImageConverter(object input, Size desiredSize)
            : this(input, desiredSize, null)
        {
        }

        public ObjectToImageConverter(object input, object desiredSize)
            : this(input, desiredSize, null)
        {
        }

        public ObjectToImageConverter(object input, object desiredSize, Binding targetVisualBinding)
        {
            if (desiredSize is Size desiredSizeValue
                && (desiredSizeValue.IsEmpty
                    || DoubleUtil.AreClose(desiredSizeValue.Width, 0)
                    || DoubleUtil.AreClose(desiredSizeValue.Height, 0)))
            {
                throw new ArgumentException("DesiredSize must not be empty and width/height must be greater than 0.", nameof(desiredSize));
            }

            this.IconBinding = input as Binding ?? new Binding { Source = input };
            this.DesiredSizeBinding = desiredSize as Binding ?? new Binding { Source = desiredSize };
            this.TargetVisualBinding = targetVisualBinding;
        }

        [ConstructorArgument("targetVisualBinding")]
        public Binding TargetVisualBinding { get; set; }

        [ConstructorArgument("iconBinding")]
        public Binding IconBinding { get; set; }

        [ConstructorArgument("desiredSize")]
        public Binding DesiredSizeBinding { get; set; }

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var desiredSize = Size.Empty;

            if (parameter is double
                || parameter is int
                || parameter is string)
            {
                var size = System.Convert.ToDouble(parameter);
                desiredSize = new Size(size, size);
            }
            else if (parameter is Size size)
            {
                desiredSize = size;
            }

            return this.Convert(value, null, desiredSize, targetType);
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        protected virtual object GetValueToConvert(object value, Size desiredSize, Visual targetVisual)
        {
            return value;
        }

        #endregion

        #region Implementation of IMultiValueConverter

        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var desiredSize = Size.Empty;
            var valuesLength = values.Length;

            var targetVisual = valuesLength == 2
                ? values[1] as Visual
                : null;

            if (targetVisual is null)
            {
                targetVisual = valuesLength == 3
                    ? values[2] as Visual
                    : null;
            }

            if (valuesLength >= 2
                && values[1] is Size desiredSizeFromIndex1)
            {
                desiredSize = desiredSizeFromIndex1;
            }
            else if (valuesLength >= 2
                     && (values[1] is Visual) == false)
            {
                var possibleDesiredSizeValue = values[1];
                object convertedValue;

                if (possibleDesiredSizeValue != null
                    && sizeConverter.CanConvertFrom(possibleDesiredSizeValue.GetType())
                    && (convertedValue = sizeConverter.ConvertFrom(possibleDesiredSizeValue)) != null)
                {
                    desiredSize = (Size)convertedValue;
                }
                else
                {
                    desiredSize = Size.Empty;
                }
            }

            return this.Convert(values[0], targetVisual, desiredSize, targetType);
        }

        
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Implementation of IMultiValueConverter

        #region Implementation of MarkupExtension

        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.CreateMultiBinding(serviceProvider);
        }

        #endregion Implementation of MarkupExtension

        private object CreateMultiBinding(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding
            {
                Converter = this
            };

            multiBinding.Bindings.Add(this.IconBinding);

            if (this.DesiredSizeBinding != null)
            {
                multiBinding.Bindings.Add(this.DesiredSizeBinding);
            }

            if (this.TargetVisualBinding != null)
            {
                multiBinding.Bindings.Add(this.TargetVisualBinding);
            }

            return multiBinding.ProvideValue(serviceProvider);
        }

        private object Convert(object value, Visual targetVisual, Size desiredSize, Type targetType)
        {
            var imageSource = CreateFrozenImageSource(this.GetValueToConvert(value, desiredSize, targetVisual), targetVisual, desiredSize);

            if (imageSource is null)
            {
                return value;
            }

            if (typeof(ImageSource).IsAssignableFrom(targetType))
            {
                return imageSource;
            }

            var image = new System.Windows.Controls.Image
            {
                Source = imageSource,
                Stretch = Stretch.Uniform
            };

            if (desiredSize.IsEmpty == false)
            {
                image.Width = desiredSize.Width;
                image.Height = desiredSize.Height;
            }

            return image;
        }

        public static ImageSource CreateFrozenImageSource(object value, Size desiredSize)
        {
            return GetAsFrozenIfPossible(CreateImageSource(value, desiredSize));
        }
        public static ImageSource CreateFrozenImageSource(object value, Visual targetVisual, Size desiredSize)
        {
            return GetAsFrozenIfPossible(CreateImageSource(value, targetVisual, desiredSize));
        }

        public static ImageSource CreateImageSource(object value, Size desiredSize)
        {
            return CreateImageSource(value, null, desiredSize);
        }

        public static ImageSource CreateImageSource(object value, Visual targetVisual, Size desiredSize)
        {
            if (value is null)
            {
                return null;
            }

            if (desiredSize == default
                || DoubleUtil.AreClose(desiredSize.Width, 0)
                || DoubleUtil.AreClose(desiredSize.Height, 0))
            {
                desiredSize = Size.Empty;
            }

            if (value is ImageSource imageSource)
            {
                return ExtractImageSource(imageSource, targetVisual, desiredSize);
            }

            if (value is string imagePath)
            {
                return CreateImageSource(imagePath, targetVisual, desiredSize);
            }

            if (value is Uri imageUri)
            {
                return CreateImageSource(imageUri, targetVisual, desiredSize);
            }

            if (value is System.Drawing.Icon icon)
            {
                return ExtractImageSource(icon, targetVisual, desiredSize);
            }

            {
                if (targetVisual != null // to get values for resource expressions we need a DependencyObject
                    && value is Expression expression)
                {
                    var type = expression.GetType();
                    var method = type.GetMethod("GetValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (method != null)
                    {
                        var valueFromExpression = method.Invoke(expression, new object[]
                        {
                        targetVisual,
                        RibbonProperties.SizeProperty
                        });

                        return CreateImageSource(valueFromExpression, targetVisual, desiredSize);
                    }
                }

                if (value.GetType().InheritsFrom("DeferredReference"))
                {
                    var type = value.GetType();
                    var method = type.GetMethod("GetValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (method != null)
                    {
                        var valueFromDeferredReference = method.Invoke(value, new object[]
                        {
                        null
                        });

                        return CreateImageSource(valueFromDeferredReference, targetVisual, desiredSize);
                    }
                }
            }

            return null;
        }

        private static ImageSource CreateImageSource(string imagePath, Visual targetVisual, Size desiredSize)
        {
            var imageUri = new Uri(imagePath, UriKind.RelativeOrAbsolute);

            if (imageUri.IsAbsoluteUri == false)
            {
                if (File.Exists(imagePath) == false)
                {
                    var slash = string.Empty;

                    if (imagePath.StartsWith("/", StringComparison.OrdinalIgnoreCase) == false)
                    {
                        slash = "/";
                    }

                    imageUri = new Uri("pack://application:,,," + slash + imagePath, UriKind.RelativeOrAbsolute);
                }
            }

            return CreateImageSource(imageUri, targetVisual, desiredSize);
        }

        private static ImageSource CreateImageSource(Uri imageUri, Visual targetVisual, Size desiredSize)
        {
            try
            {
                var decoder = BitmapDecoder.Create(imageUri, BitmapCreateOptions.None, BitmapCacheOption.Default);

                return ExtractImageSource(decoder, targetVisual, desiredSize);
            }
            catch (IOException exception) when (DesignerProperties.GetIsInDesignMode(targetVisual ?? new DependencyObject()))
            {
                Trace.WriteLine(exception);

                return imageNotFoundImageSource;
            }
        }

        private static ImageSource ExtractImageSource(System.Drawing.Icon icon, Visual targetVisual, Size desiredSize)
        {
            var imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            return ExtractImageSource(imageSource, targetVisual, desiredSize);
        }

        private static ImageSource ExtractImageSource(ImageSource imageSource, Visual targetVisual, Size desiredSize)
        {
            if (desiredSize.IsEmpty)
            {
                return imageSource;
            }

            var bitmapFrame = imageSource as BitmapFrame;

            // We may have some other type of ImageSource
            // that doesn't have a notion of frames or decoder
            if (bitmapFrame?.Decoder is null)
            {
                return imageSource;
            }

            return ExtractImageSource(bitmapFrame.Decoder, targetVisual, desiredSize);
        }

        private static ImageSource ExtractImageSource(BitmapDecoder decoder, Visual targetVisual, Size desiredSize)
        {
            var scaledDesiredSize = GetScaledDesiredSize(desiredSize, targetVisual);

            var framesOrderedByWidth = decoder.Frames
                .OrderBy(f => f.Width)
                .ThenBy(f => f.Height)
                .ToList();

            return framesOrderedByWidth
                       .FirstOrDefault(f => f.Width >= scaledDesiredSize.Width
                                            && f.Height >= scaledDesiredSize.Height)
                   ?? framesOrderedByWidth.Last();
        }

        protected static Size GetScaledDesiredSize(Size desiredSize, Visual targetVisual)
        {
            return GetScaledDesiredSize(desiredSize, GetDpiScale(targetVisual));
        }

        private static Size GetScaledDesiredSize(Size desiredSize, DpiScale dpiScale)
        {
            if (desiredSize.IsEmpty)
            {
                return desiredSize;
            }

            return new Size(desiredSize.Width * dpiScale.DpiScaleX, desiredSize.Height * dpiScale.DpiScaleY);
        }

        private static DpiScale GetDpiScale(Visual targetVisual)
        {
            if (targetVisual != null)
            {
                var source = PresentationSource.FromVisual(targetVisual);
                var dpiScaleX = source.CompositionTarget.TransformToDevice.M11;
                var dpiScaledpiY = source.CompositionTarget.TransformToDevice.M22;
                return new DpiScale(dpiScaleX, dpiScaledpiY);
            }

            if (Application.Current?.CheckAccess() == true
                && Application.Current.MainWindow?.CheckAccess() == true)
            {
                var presentationSource = PresentationSource.FromVisual(Application.Current.MainWindow);

                if (presentationSource?.CompositionTarget != null)
                {
                    return new DpiScale(presentationSource.CompositionTarget.TransformToDevice.M11, presentationSource.CompositionTarget.TransformToDevice.M22);
                }
            }

            return new DpiScale(1, 1);
        }

        private static ImageSource CreateImageNotFoundImageSource()
        {
            var drawingGroup = new DrawingGroup
            {
                ClipGeometry = Geometry.Parse("M0,0 V426,667 H426,667 V0 H0 Z")
            };
            var geometryDrawing = new GeometryDrawing(Brushes.Red, new Pen(), Geometry.Parse("F1 M426.667,426.667z M0,0z M213.333,0C95.514,0 0,95.514 0,213.333 0,331.152 95.514,426.666 213.333,426.666 331.152,426.666 426.666,331.152 426.666,213.333 426.666,95.514 331.153,0 213.333,0z M330.995,276.689L276.693,330.995 213.333,267.639 149.973,330.999 95.671,276.689 159.027,213.333 95.671,149.973 149.973,95.671 213.333,159.027 276.693,95.671 330.995,149.973 267.639,213.333 330.995,276.689z"));
            using (drawingGroup.Append())
            {
                drawingGroup.Children.Add(geometryDrawing);
            }

            var image = new DrawingImage
            {
                Drawing = drawingGroup
            };

            return image;
        }

        private static ImageSource GetAsFrozenIfPossible(ImageSource imageSource)
        {
            if (imageSource is null)
            {
                return null;
            }

            if (imageSource.CanFreeze)
            {
                return (ImageSource)imageSource.GetAsFrozen();
            }

            return imageSource;
        }
    }
}
