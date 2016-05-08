using Perspex;
using Perspex.Controls;
using Perspex.Controls.Shapes;

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
                    // TODO: Binding
                    //rectangle.Bind(Shape.FillProperty, new Binding { Converter = new ColorConverter(), Path = new PropertyPath("Color") });
                    SetNativeControl(rectangle);
                }
            }
        }
    }
}