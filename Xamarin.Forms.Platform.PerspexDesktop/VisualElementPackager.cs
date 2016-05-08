using Perspex.Controls;
using System;
using System.Collections.ObjectModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class VisualElementPackager : IDisposable
	{
		readonly int _column;
		readonly int _columnSpan;

		readonly Panel _panel;
		readonly IVisualElementRenderer _renderer;
		readonly int _row;
		readonly int _rowSpan;
		bool _disposed;
		bool _isLoaded;

		public VisualElementPackager(IVisualElementRenderer renderer)
		{
			if (renderer == null)
				throw new ArgumentNullException("renderer");

			_renderer = renderer;

			_panel = renderer.ContainerElement as Panel;
			if (_panel == null)
				throw new ArgumentException("Renderer's container element must be a Panel");
		}

		public VisualElementPackager(IVisualElementRenderer renderer, int row = 0, int rowSpan = 0, int column = 0, int columnSpan = 0) : this(renderer)
		{
			_row = row;
			_rowSpan = rowSpan;
			_column = column;
			_columnSpan = columnSpan;
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			_disposed = true;

			VisualElement element = _renderer.Element;
			if (element != null)
			{
				element.ChildAdded -= OnChildAdded;
				element.ChildRemoved -= OnChildRemoved;
			}
		}

		public void Load()
		{
			if (_isLoaded)
				return;

			_isLoaded = true;
			_renderer.Element.ChildAdded += OnChildAdded;
			_renderer.Element.ChildRemoved += OnChildRemoved;

			ReadOnlyCollection<Element> children = _renderer.Element.LogicalChildren;
			for (var i = 0; i < children.Count; i++)
			{
				OnChildAdded(_renderer.Element, new ElementEventArgs(children[i]));
			}
		}

		void EnsureZIndex()
		{
			if (_renderer.Element.LogicalChildren.Count == 0)
				return;

			for (var z = 0; z < _renderer.Element.LogicalChildren.Count; z++)
			{
				var child = _renderer.Element.LogicalChildren[z] as VisualElement;
				if (child == null)
					continue;

				IVisualElementRenderer childRenderer = Platform.GetRenderer(child);

				if (childRenderer == null)
				{
					continue;
				}

				//Canvas.SetZIndex(childRenderer.ContainerElement, z + 1);
			}
		}

		void OnChildAdded(object sender, ElementEventArgs e)
		{
			var view = e.Element as VisualElement;

			if (view == null)
				return;

			IVisualElementRenderer childRenderer = Platform.CreateRenderer(view);
			Platform.SetRenderer(view, childRenderer);

			//if (_row > 0)
			//	Grid.SetRow(childRenderer.ContainerElement, _row);
			//if (_rowSpan > 0)
			//	Grid.SetRowSpan(childRenderer.ContainerElement, _rowSpan);
			//if (_column > 0)
			//	Grid.SetColumn(childRenderer.ContainerElement, _column);
			//if (_columnSpan > 0)
			//	Grid.SetColumnSpan(childRenderer.ContainerElement, _columnSpan);

			_panel.Children.Add(childRenderer.ContainerElement);

			EnsureZIndex();
		}

		void OnChildRemoved(object sender, ElementEventArgs e)
		{
			var view = e.Element as VisualElement;

			if (view == null)
				return;

			IVisualElementRenderer childRenderer = Platform.GetRenderer(view);
			if (childRenderer != null)
			{
				//if (_row > 0)
				//	childRenderer.ContainerElement.ClearValue(Grid.RowProperty);
				//if (_rowSpan > 0)
				//	childRenderer.ContainerElement.ClearValue(Grid.RowSpanProperty);
				//if (_column > 0)
				//	childRenderer.ContainerElement.ClearValue(ColumnProperty);
				//if (_columnSpan > 0)
				//	childRenderer.ContainerElement.ClearValue(Grid.ColumnSpanProperty);

				_panel.Children.Remove(childRenderer.ContainerElement);

				view.Cleanup();
			}
		}
	}
}