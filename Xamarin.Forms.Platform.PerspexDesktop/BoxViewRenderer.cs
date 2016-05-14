using Perspex;
using Perspex.Controls;
using Perspex.Controls.Shapes;
using Perspex.Markup.Data;
using Perspex.Media;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class BoxViewRenderer : ViewRenderer<BoxView, Perspex.Controls.Shapes.Rectangle>
	{
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var rectangle = new Perspex.Controls.Shapes.Rectangle();

                    rectangle.DataContext = Element;
                    var binding = new Perspex.Markup.Xaml.Data.Binding { Converter = new ColorConverter(), Path = "Color" };
                    rectangle.Bind(Shape.FillProperty, binding);

                    SetNativeControl(rectangle);
                }
            }
        }
    }
}