using System;
using Perspex;
using Perspex.Controls;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
	public interface IVisualElementRenderer : IRegisterable, IDisposable
	{
		Control ContainerElement { get; }

		VisualElement Element { get; }

		event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint);
		void SetElement(VisualElement element);
	}
}