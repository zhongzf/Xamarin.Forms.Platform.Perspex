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
                        Source = ConvertImageSource(x.ImageSource)
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
            return grid;
        }

        private static IBitmap ConvertImageSource(ImageSource imageSource)
        {
            var imageConverter = new ImageConverter();
            return (IBitmap)imageConverter.Convert(imageSource, typeof(IBitmap), null, null);
        }

        public static object GetDefault(string name)
        {
            if(Application.Current.Resources != null && Application.Current.Resources.ContainsKey(name))
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
