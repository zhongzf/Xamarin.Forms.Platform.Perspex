using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class FrameRenderer : ViewRenderer<Frame, Perspex.Controls.Shapes.Rectangle>
	{
        public FrameRenderer()
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var rectangle = new Perspex.Controls.Shapes.Rectangle();
                    SetNativeControl(rectangle);
                }

                PackChild();
                UpdateBorder();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Content")
            {
                PackChild();
            }
            else if (e.PropertyName == Frame.OutlineColorProperty.PropertyName || e.PropertyName == Frame.HasShadowProperty.PropertyName)
            {
                UpdateBorder();
            }
        }

        void PackChild()
        {
            if (Element.Content == null)
                return;

            IVisualElementRenderer renderer = Element.Content.GetOrCreateRenderer();
            //Control.Child = renderer.ContainerElement;
        }

        void UpdateBorder()
        {
            //Control.CornerRadius = new CornerRadius(5);
            if (Element.OutlineColor != Color.Default)
            {
                //Control.BorderBrush = Element.OutlineColor.ToBrush();
                //Control.BorderThickness = new Windows.UI.Xaml.Thickness(1);
            }
            else
            {
                //Control.BorderBrush = new Color(0, 0, 0, 0).ToBrush();
            }
        }
    }
}