using System;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;
using System.Reflection;
using System.IO;

namespace WzUXRibbon.Effects
{
    public class GrayscaleEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty(nameof(Input), typeof(GrayscaleEffect), 0);

        public static readonly DependencyProperty FilterColorProperty =
            DependencyProperty.Register(nameof(FilterColor), typeof(Color), typeof(GrayscaleEffect),
                new PropertyMetadata(Color.FromArgb(255, 255, 255, 255), PixelShaderConstantCallback(0)));

        public GrayscaleEffect()
        {
            this.PixelShader = this.CreatePixelShader();

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(FilterColorProperty);
        }

        private PixelShader CreatePixelShader()
        {
            var pixelShader = new PixelShader();
            pixelShader.SetStreamSource(new MemoryStream(Properties.Resources.Grayscale));

            return pixelShader;
        }

        private Uri MakePackUri(string relativeFile)
        {
            Assembly a = typeof(GrayscaleEffect).Assembly;

            string assemblyShortName = a.ToString().Split(',')[0];

            string uriString = "pack://application:,,,/" +
                assemblyShortName +
                ";component/" +
                relativeFile;

            return new Uri(uriString);
        }

        public Brush Input
        {
            get => (Brush)this.GetValue(InputProperty);
            set => this.SetValue(InputProperty, value);
        }

        public Color FilterColor
        {
            get => (Color)this.GetValue(FilterColorProperty);
            set => this.SetValue(FilterColorProperty, value);
        }
    }
}
