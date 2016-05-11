using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class ProgressBarRenderer : ViewRenderer<ProgressBar, Perspex.Controls.ProgressBar>
	{
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var progressBar = new Perspex.Controls.ProgressBar();
                    progressBar.PropertyChanged += ProgressBar_PropertyChanged;
                    SetNativeControl(progressBar);
                }

                Control.Value = ConvertProgressValue(e.NewElement.Progress);
            }
        }

        protected double ConvertProgressValue(double progress)
        {
            var value = progress * 100;
            return value;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName)
            {
                Control.Value = ConvertProgressValue(Element.Progress);
            }
        }

        private void ProgressBar_PropertyChanged(object sender, Perspex.PerspexPropertyChangedEventArgs e)
        {
            if(e.Property == Perspex.Controls.ProgressBar.ValueProperty)
            {
                ProgressBarOnValueChanged(sender, e);
            }
        }

        void ProgressBarOnValueChanged(object sender, Perspex.PerspexPropertyChangedEventArgs e)
        {
            ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.MeasureChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Control != null)
                {
                    Control.PropertyChanged -= ProgressBar_PropertyChanged;
                }
            }

            base.Dispose(disposing);
        }
    }
}