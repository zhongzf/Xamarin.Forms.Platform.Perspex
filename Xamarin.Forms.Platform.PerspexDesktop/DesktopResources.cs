using Perspex.Controls.Templates;
using Perspex.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perspex.Media.Imaging;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class DesktopResources
    {
        private static ResourceDictionary _defaultResources;

        internal static ResourceDictionary GetDefaultResources()
        {
            if (_defaultResources == null)
            {
                _defaultResources = new ResourceDictionary();
                _defaultResources.Add("ContentControlThemeFontFamily", "Segoe UI");
                _defaultResources.Add("ControlContentThemeFontSize", 22.667);
                _defaultResources.Add("FormsCancelForegroundBrush", Color.White.ToBrush());
                _defaultResources.Add("FormsCancelBackgroundBrush", Color.White.ToBrush());
                _defaultResources.Add("TextBoxButtonBackgroundThemeBrush", Color.White.ToBrush());
                _defaultResources.Add("SystemControlBackgroundChromeBlackMediumBrush", Color.White.ToBrush());

                // TODO: read default theme.
                _defaultResources.Add("ButtonBackgroundThemeBrush", Color.Silver.ToBrush());
                _defaultResources.Add("ButtonBorderThemeBrush", Color.Gray.ToBrush());
                _defaultResources.Add("DefaultTextForegroundThemeBrush", Color.Black.ToBrush());

                // TODO:
                _defaultResources.Add("ListViewHeaderTextCell", null);
                _defaultResources.Add("ListViewTextCell", null);
                _defaultResources.Add("TextCell", new FuncDataTemplate<TextCell>(x =>
                                new Perspex.Controls.StackPanel
                                {
                                    Children =
                                    {
                                        new Perspex.Controls.TextBlock
                                        {
                                            Text = x.Text
                                        },
                                        new Perspex.Controls.TextBlock
                                        {
                                            Text = x.Detail
                                        },
                                    }
                                }));
                _defaultResources.Add("EntryCell", null);
                _defaultResources.Add("ViewCell", null);
                _defaultResources.Add("SwitchCell", null);
                _defaultResources.Add("ListImageCell", null);
                _defaultResources.Add("ImageCell", new FuncDataTemplate<ImageCell>(x => GenerateImageCellTemplate(x)));
            }
            return _defaultResources;
        }

        public static Perspex.Controls.IControl GenerateImageCellTemplate(ImageCell x)
        {
            Perspex.Controls.Grid grid = null;
            Perspex.Controls.Image image = null;
            Perspex.Controls.TextBlock textBlock = null;
            Perspex.Controls.TextBlock detailBlock = null;
            grid = new Perspex.Controls.Grid
            {
                Children = new Perspex.Controls.Controls
                {
                    (image = new Perspex.Controls.Image
                    {
                        Width=32
                    }),
                    (textBlock = new Perspex.Controls.TextBlock
                    {
                        Text = x.Text,
                    }),
                    (detailBlock = new Perspex.Controls.TextBlock
                    {
                        Text = x.Detail
                    }),
                }
            };
            grid.RowDefinitions.Add(new Perspex.Controls.RowDefinition(Perspex.Controls.GridLength.Auto));
            grid.RowDefinitions.Add(new Perspex.Controls.RowDefinition(Perspex.Controls.GridLength.Auto));
            grid.ColumnDefinitions.Add(new Perspex.Controls.ColumnDefinition(Perspex.Controls.GridLength.Auto));
            grid.ColumnDefinitions.Add(new Perspex.Controls.ColumnDefinition(Perspex.Controls.GridLength.Auto));
            Perspex.Controls.Grid.SetRowSpan(image, 2);
            Perspex.Controls.Grid.SetColumn(textBlock, 1);
            Perspex.Controls.Grid.SetRow(detailBlock, 1);
            Perspex.Controls.Grid.SetColumn(detailBlock, 1);

            Task.Run(() => UpdateImageSource(image, x.ImageSource));

            return grid;
        }

        static async void UpdateImageSource(Perspex.Controls.Image image, ImageSource imageSource)
        {
            ImageSource source = imageSource;
            IImageSourceHandler handler;
            if (source != null && (handler = Registrar.Registered.GetHandler<IImageSourceHandler>(source.GetType())) != null)
            {
                IBitmap imagesource;
                try
                {
                    imagesource = await handler.LoadImageAsync(source);
                }
                catch (OperationCanceledException)
                {
                    imagesource = null;
                }

                // In the time it takes to await the imagesource, some zippy little app
                // might have disposed of this Image already.
                if (image != null)
                    SetImageSource(image, imagesource);
            }
            else
                SetImageSource(image, null);
        }

        static void SetImageSource(Perspex.Controls.Image image, IBitmap bitmap)
        {
            if (Device.IsInvokeRequired)
            {
                Device.BeginInvokeOnMainThread(() => { image.Source = bitmap; });
            }
            else
            {
                image.Source = bitmap;
            }
        }

        private static IBitmap ConvertImageSource(ImageSource imageSource)
        {
            var imageConverter = new ImageConverter();
            var value = imageConverter.Convert(imageSource, typeof(IBitmap), null, null);
            if (value is PerspexDesktop.AsyncValue<IBitmap>)
            {
                var asyncValue = (PerspexDesktop.AsyncValue<IBitmap>)value;
                return asyncValue.Value;
            }
            else
            {
                return (IBitmap)value;
            }
        }

        public static object GetDefault(string name)
        {
            if (Application.Current.Resources != null && Application.Current.Resources.ContainsKey(name))
            {
                return Application.Current.Resources[name];
            }
            else
            {
                return GetDefaultResources()[name];
            }
        }
    }
}
