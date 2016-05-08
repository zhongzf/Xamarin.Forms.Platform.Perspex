﻿using Perspex.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class VisualElementTracker<TElement, TNativeElement> : IDisposable where TElement : VisualElement where TNativeElement : Control
    {
		readonly NotifyCollectionChangedEventHandler _collectionChangedHandler;
		readonly List<uint> _fingers = new List<uint>();
        Control _container;
		TNativeElement _control;
		TElement _element;

		bool _invalidateArrangeNeeded;

		bool _isDisposed;
		bool _isPanning;
		bool _isPinching;
		bool _wasPanGestureStartedSent;
		bool _wasPinchGestureStartedSent;

		public VisualElementTracker()
		{
			_collectionChangedHandler = ModelGestureRecognizersOnCollectionChanged;
		}

		public Control Container
		{
			get { return _container; }
			set
			{
				if (_container == value)
					return;

				if (_container != null)
				{
					//_container.Tapped -= OnTap;
					//_container.DoubleTapped -= OnDoubleTap;
					//_container.ManipulationDelta -= OnManipulationDelta;
					//_container.ManipulationStarted -= OnManipulationStarted;
					//_container.ManipulationCompleted -= OnManipulationCompleted;
					//_container.PointerPressed -= OnPointerPressed;
					//_container.PointerExited -= OnPointerExited;
					//_container.PointerReleased -= OnPointerReleased;
					//_container.PointerCanceled -= OnPointerCanceled;
				}

				_container = value;

				UpdatingGestureRecognizers();

				UpdateNativeControl();
			}
		}

		public TNativeElement Control
		{
			get { return _control; }
			set
			{
				if (_control == value)
					return;

				_control = value;
				UpdateNativeControl();
			}
		}

		public TElement Element
		{
			get { return _element; }
			set
			{
				if (_element == value)
					return;

				if (_element != null)
				{
					_element.BatchCommitted -= OnRedrawNeeded;
					_element.PropertyChanged -= OnPropertyChanged;

					var view = _element as View;
					if (view != null)
					{
						var oldRecognizers = (ObservableCollection<IGestureRecognizer>)view.GestureRecognizers;
						oldRecognizers.CollectionChanged -= _collectionChangedHandler;
					}
				}

				_element = value;

				if (_element != null)
				{
					_element.BatchCommitted += OnRedrawNeeded;
					_element.PropertyChanged += OnPropertyChanged;

					var view = _element as View;
					if (view != null)
					{
						var newRecognizers = (ObservableCollection<IGestureRecognizer>)view.GestureRecognizers;
						newRecognizers.CollectionChanged += _collectionChangedHandler;
					}
				}

				UpdateNativeControl();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public event EventHandler Updated;

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			_isDisposed = true;

			if (!disposing)
				return;

			if (_container != null)
			{
				//_container.PointerPressed -= OnPointerPressed;
				//_container.PointerExited -= OnPointerExited;
				//_container.PointerReleased -= OnPointerReleased;
				//_container.PointerCanceled -= OnPointerCanceled;
				//_container.Tapped -= OnTap;
				//_container.DoubleTapped -= OnDoubleTap;
				//_container.ManipulationDelta -= OnManipulationDelta;
				//_container.ManipulationStarted -= OnManipulationStarted;
				//_container.ManipulationCompleted -= OnManipulationCompleted;
			}

			if (_element != null)
			{
				_element.BatchCommitted -= OnRedrawNeeded;
				_element.PropertyChanged -= OnPropertyChanged;

				var view = _element as View;
				if (view != null)
				{
					var oldRecognizers = (ObservableCollection<IGestureRecognizer>)view.GestureRecognizers;
					oldRecognizers.CollectionChanged -= _collectionChangedHandler;
				}
			}

			Control = null;
			Element = null;
			Container = null;
		}

		protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (Element.Batched)
			{
				if (e.PropertyName == VisualElement.XProperty.PropertyName || e.PropertyName == VisualElement.YProperty.PropertyName || e.PropertyName == VisualElement.WidthProperty.PropertyName ||
					e.PropertyName == VisualElement.HeightProperty.PropertyName)
				{
					_invalidateArrangeNeeded = true;
				}
				return;
			}

			if (e.PropertyName == VisualElement.XProperty.PropertyName || e.PropertyName == VisualElement.YProperty.PropertyName || e.PropertyName == VisualElement.WidthProperty.PropertyName ||
				e.PropertyName == VisualElement.HeightProperty.PropertyName)
			{
				MaybeInvalidate();
			}
			else if (e.PropertyName == VisualElement.AnchorXProperty.PropertyName || e.PropertyName == VisualElement.AnchorYProperty.PropertyName)
			{
				UpdateScaleAndRotation(Element, Container);
			}
			else if (e.PropertyName == VisualElement.ScaleProperty.PropertyName)
			{
				UpdateScaleAndRotation(Element, Container);
			}
			else if (e.PropertyName == VisualElement.TranslationXProperty.PropertyName || e.PropertyName == VisualElement.TranslationYProperty.PropertyName ||
					 e.PropertyName == VisualElement.RotationProperty.PropertyName || e.PropertyName == VisualElement.RotationXProperty.PropertyName || e.PropertyName == VisualElement.RotationYProperty.PropertyName)
			{
				UpdateRotation(Element, Container);
			}
			else if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
			{
				UpdateVisibility(Element, Container);
			}
			else if (e.PropertyName == VisualElement.OpacityProperty.PropertyName)
			{
				UpdateOpacity(Element, Container);
			}
			else if (e.PropertyName == VisualElement.InputTransparentProperty.PropertyName)
			{
				UpdateInputTransparent(Element, Container);
			}
		}

		protected virtual void UpdateNativeControl()
		{
			if (Element == null || Container == null)
				return;

			UpdateVisibility(Element, Container);
			UpdateOpacity(Element, Container);
			UpdateScaleAndRotation(Element, Container);
			UpdateInputTransparent(Element, Container);

			if (_invalidateArrangeNeeded)
			{
				MaybeInvalidate();
			}
			_invalidateArrangeNeeded = false;

			OnUpdated();
		}

		//void HandlePan(ManipulationDeltaRoutedEventArgs e, View view)
		//{
		//	if (view == null)
		//		return;

		//	_isPanning = true;

		//	foreach (PanGestureRecognizer recognizer in view.GestureRecognizers.GetGesturesFor<PanGestureRecognizer>().Where(g => g.TouchPoints == _fingers.Count))
		//	{
		//		if (!_wasPanGestureStartedSent)
		//		{
		//			((IPanGestureController)recognizer).SendPanStarted(view, Application.Current.PanGestureId);
		//		}
		//		((IPanGestureController)recognizer).SendPan(view, e.Delta.Translation.X + e.Cumulative.Translation.X, e.Delta.Translation.Y + e.Cumulative.Translation.Y, Application.Current.PanGestureId);
		//	}
		//	_wasPanGestureStartedSent = true;
		//}

		//void HandlePinch(ManipulationDeltaRoutedEventArgs e, View view)
		//{
		//	if (_fingers.Count < 2 || view == null)
		//		return;

		//	_isPinching = true;

		//	Windows.Foundation.Point translationPoint = e.Container.TransformToVisual(Container).TransformPoint(e.Position);

		//	var scaleOriginPoint = new Point(translationPoint.X / view.Width, translationPoint.Y / view.Height);
		//	IEnumerable<PinchGestureRecognizer> pinchGestures = view.GestureRecognizers.GetGesturesFor<PinchGestureRecognizer>();
		//	foreach (PinchGestureRecognizer recognizer in pinchGestures)
		//	{
		//		if (!_wasPinchGestureStartedSent)
		//		{
		//			((IPinchGestureController)recognizer).SendPinchStarted(view, scaleOriginPoint);
		//		}
		//		((IPinchGestureController)recognizer).SendPinch(view, e.Delta.Scale, scaleOriginPoint);
		//	}
		//	_wasPinchGestureStartedSent = true;
		//}

		void MaybeInvalidate()
		{
			if (Element.IsInNativeLayout)
				return;
			var parent = (Control)Container.Parent;
			parent?.InvalidateMeasure();
			Container.InvalidateMeasure();
		}

		void ModelGestureRecognizersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			UpdatingGestureRecognizers();
		}

		//void OnDoubleTap(object sender, DoubleTappedRoutedEventArgs e)
		//{
		//	var view = Element as View;
		//	if (view == null)
		//		return;

		//	IEnumerable<TapGestureRecognizer> doubleTapGestures = view.GestureRecognizers.GetGesturesFor<TapGestureRecognizer>(g => g.NumberOfTapsRequired == 2);
		//	foreach (TapGestureRecognizer recognizer in doubleTapGestures)
		//		recognizer.SendTapped(view);
		//}

		//void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		//{
		//	PinchComplete(true);
		//	PanComplete(true);
		//}

		//void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		//{
		//	var view = Element as View;
		//	if (view == null)
		//		return;

		//	HandlePinch(e, view);
		//	HandlePan(e, view);
		//}

		//void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		//{
		//	var view = Element as View;
		//	if (view == null)
		//		return;
		//	_wasPinchGestureStartedSent = false;
		//	_wasPanGestureStartedSent = false;
		//}

		//void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
		//{
		//	uint id = e.Pointer.PointerId;
		//	if (_fingers.Contains(id))
		//		_fingers.Remove(id);

		//	PinchComplete(false);
		//	PanComplete(false);
		//}

		//void OnPointerExited(object sender, PointerRoutedEventArgs e)
		//{
		//	uint id = e.Pointer.PointerId;
		//	if (_fingers.Contains(id))
		//		_fingers.Remove(id);

		//	PinchComplete(true);
		//	PanComplete(true);
		//}

		//void OnPointerPressed(object sender, PointerRoutedEventArgs e)
		//{
		//	uint id = e.Pointer.PointerId;
		//	if (!_fingers.Contains(id))
		//		_fingers.Add(id);
		//}

		//void OnPointerReleased(object sender, PointerRoutedEventArgs e)
		//{
		//	uint id = e.Pointer.PointerId;
		//	if (_fingers.Contains(id))
		//		_fingers.Remove(id);

		//	PinchComplete(true);
		//	PanComplete(true);
		//}

		void OnRedrawNeeded(object sender, EventArgs e)
		{
			UpdateNativeControl();
		}

		//void OnTap(object sender, TappedRoutedEventArgs e)
		//{
		//	var view = Element as View;
		//	if (view == null)
		//		return;

		//	IEnumerable<TapGestureRecognizer> tapGestures = view.GestureRecognizers.GetGesturesFor<TapGestureRecognizer>(g => g.NumberOfTapsRequired == 1);
		//	foreach (TapGestureRecognizer recognizer in tapGestures)
		//	{
		//		recognizer.SendTapped(view);
		//		e.Handled = true;
		//	}
		//}

		void OnUpdated()
		{
			if (Updated != null)
				Updated(this, EventArgs.Empty);
		}

		void PanComplete(bool success)
		{
			var view = Element as View;
			if (view == null || !_isPanning)
				return;

			foreach (PanGestureRecognizer recognizer in view.GestureRecognizers.GetGesturesFor<PanGestureRecognizer>().Where(g => g.TouchPoints == _fingers.Count))
			{
				if (success)
				{
					((IPanGestureController)recognizer).SendPanCompleted(view, Application.Current.PanGestureId);
				}
				else
				{
					((IPanGestureController)recognizer).SendPanCanceled(view, Application.Current.PanGestureId);
				}
			}

			Application.Current.PanGestureId++;
			_isPanning = false;
		}

		void PinchComplete(bool success)
		{
			var view = Element as View;
			if (view == null || !_isPinching)
				return;

			IEnumerable<PinchGestureRecognizer> pinchGestures = view.GestureRecognizers.GetGesturesFor<PinchGestureRecognizer>();
			foreach (PinchGestureRecognizer recognizer in pinchGestures)
			{
				if (success)
				{
					((IPinchGestureController)recognizer).SendPinchEnded(view);
				}
				else
				{
					((IPinchGestureController)recognizer).SendPinchCanceled(view);
				}
			}

			_isPinching = false;
		}

		static void UpdateInputTransparent(VisualElement view, Control frameworkElement)
		{
			frameworkElement.IsHitTestVisible = !view.InputTransparent;
		}

		static void UpdateOpacity(VisualElement view, Control frameworkElement)
		{
			frameworkElement.Opacity = view.Opacity;
		}

		static void UpdateRotation(VisualElement view, Control frameworkElement)
		{
			double anchorX = view.AnchorX;
			double anchorY = view.AnchorY;
			double rotationX = view.RotationX;
			double rotationY = view.RotationY;
			double rotation = view.Rotation;
			double translationX = view.TranslationX;
			double translationY = view.TranslationY;
			double scale = view.Scale;

            /*
			if (rotationX % 360 == 0 && rotationY % 360 == 0 && rotation % 360 == 0 && translationX == 0 && translationY == 0 && scale == 1)
			{
				frameworkElement.Projection = null;
			}
			else
			{
				frameworkElement.Projection = new PlaneProjection
				{
					CenterOfRotationX = anchorX,
					CenterOfRotationY = anchorY,
					GlobalOffsetX = scale == 0 ? 0 : translationX / scale,
					GlobalOffsetY = scale == 0 ? 0 : translationY / scale,
					RotationX = -rotationX,
					RotationY = -rotationY,
					RotationZ = -rotation
				};
			}
            */
		}

		static void UpdateScaleAndRotation(VisualElement view, Control frameworkElement)
		{
			double anchorX = view.AnchorX;
			double anchorY = view.AnchorY;
			double scale = view.Scale;
			//frameworkElement.RenderTransformOrigin = new Windows.Foundation.Point(anchorX, anchorY);
			//frameworkElement.RenderTransform = new ScaleTransform { ScaleX = scale, ScaleY = scale };

			//UpdateRotation(view, frameworkElement);
		}

		static void UpdateVisibility(VisualElement view, Control frameworkElement)
		{
			//frameworkElement.Visibility = view.IsVisible ? Visibility.Visible : Visibility.Collapsed;
		}

		void UpdatingGestureRecognizers()
		{
			var view = Element as View;
			IList<IGestureRecognizer> gestures = view?.GestureRecognizers;

			if (_container == null || gestures == null)
				return;

            /*
			_container.Tapped -= OnTap;
			_container.DoubleTapped -= OnDoubleTap;
			_container.ManipulationDelta -= OnManipulationDelta;
			_container.ManipulationStarted -= OnManipulationStarted;
			_container.ManipulationCompleted -= OnManipulationCompleted;
			_container.PointerPressed -= OnPointerPressed;
			_container.PointerExited -= OnPointerExited;
			_container.PointerReleased -= OnPointerReleased;
			_container.PointerCanceled -= OnPointerCanceled;

			if (gestures.GetGesturesFor<TapGestureRecognizer>(g => g.NumberOfTapsRequired == 1).GetEnumerator().MoveNext())
				_container.Tapped += OnTap;

			if (gestures.GetGesturesFor<TapGestureRecognizer>(g => g.NumberOfTapsRequired == 2).GetEnumerator().MoveNext())
				_container.DoubleTapped += OnDoubleTap;

			bool hasPinchGesture = gestures.GetGesturesFor<PinchGestureRecognizer>().GetEnumerator().MoveNext();
			bool hasPanGesture = gestures.GetGesturesFor<PanGestureRecognizer>().GetEnumerator().MoveNext();
			if (!hasPinchGesture && !hasPanGesture)
				return;

			//We can't handle ManipulationMode.Scale and System , so we don't support pinch/pan on a scrollview 
			if (Element is ScrollView)
			{
				if (hasPinchGesture)
					Log.Warning("Gestures", "PinchGestureRecognizer is not supported on a ScrollView in Windows Platforms");
				if (hasPanGesture)
					Log.Warning("Gestures", "PanGestureRecognizer is not supported on a ScrollView in Windows Platforms");
				return;
			}

			_container.ManipulationMode = ManipulationModes.Scale | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
			_container.ManipulationDelta += OnManipulationDelta;
			_container.ManipulationStarted += OnManipulationStarted;
			_container.ManipulationCompleted += OnManipulationCompleted;
			_container.PointerPressed += OnPointerPressed;
			_container.PointerExited += OnPointerExited;
			_container.PointerReleased += OnPointerReleased;
			_container.PointerCanceled += OnPointerCanceled;
            */
		}
	}
}