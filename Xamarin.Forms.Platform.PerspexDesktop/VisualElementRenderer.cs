using System;
using System.ComponentModel;
using Perspex;
using Perspex.Controls;
using Perspex.Animation;
using Perspex.Interactivity;
using Xamarin.Forms;
using Perspex.Controls.Primitives;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class VisualElementRenderer<TElement, TNativeElement> : Panel, IVisualElementRenderer, IDisposable, IEffectControlProvider where TElement : VisualElement
                                                                                                                                      where TNativeElement : Control
    {
        bool _disposed;

        public TNativeElement Control { get; private set; }

        public TElement Element { get; private set; }

        public Control ContainerElement
        {
            get { return this; }
        }

        VisualElement IVisualElementRenderer.Element
        {
            get { return Element; }
        }

        protected bool AutoPackage { get; set; } = true;
        VisualElementPackager Packager { get; set; }

        EventHandler<VisualElementChangedEventArgs> _elementChangedHandlers;
        event EventHandler<VisualElementChangedEventArgs> IVisualElementRenderer.ElementChanged
        {
            add
            {
                if (_elementChangedHandlers == null)
                    _elementChangedHandlers = value;
                else
                    _elementChangedHandlers = (EventHandler<VisualElementChangedEventArgs>)Delegate.Combine(_elementChangedHandlers, value);
            }

            remove { _elementChangedHandlers = (EventHandler<VisualElementChangedEventArgs>)Delegate.Remove(_elementChangedHandlers, value); }
        }

        public event EventHandler<ElementChangedEventArgs<TElement>> ElementChanged;

        VisualElementTracker<TElement, TNativeElement> _tracker;
        protected bool AutoTrack { get; set; } = true;
        protected VisualElementTracker<TElement, TNativeElement> Tracker
        {
            get { return _tracker; }
            set
            {
                if (_tracker == value)
                    return;

                if (_tracker != null)
                {
                    _tracker.Dispose();
                    _tracker.Updated -= OnTrackerUpdated;
                }

                _tracker = value;

                if (_tracker != null)
                {
                    _tracker.Updated += OnTrackerUpdated;
                    UpdateTracker();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
                return;

            _disposed = true;

            SetNativeControl(null);
            SetElement(null);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #region Effect
        public void RegisterEffect(Effect effect)
        {
            var platformEffect = effect as PlatformEffect;
            if (platformEffect != null)
                OnRegisterEffect(platformEffect);
        }

        protected virtual void OnRegisterEffect(PlatformEffect effect)
        {
            effect.Container = this;
            effect.Control = Control;
        }
        #endregion

        protected virtual void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            var args = new VisualElementChangedEventArgs(e.OldElement, e.NewElement);
            if (_elementChangedHandlers != null)
                _elementChangedHandlers(this, args);

            EventHandler<ElementChangedEventArgs<TElement>> changed = ElementChanged;
            if (changed != null)
                changed(this, e);
        }

        protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
                UpdateEnabled();
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                UpdateBackgroundColor();
        }


        public void SetElement(VisualElement element)
        {
            TElement oldElement = Element;
            Element = (TElement)element;

            if (oldElement != null)
            {
                oldElement.PropertyChanged -= OnElementPropertyChanged;
                oldElement.FocusChangeRequested -= OnElementFocusChangeRequested;
            }

            if (element != null)
            {
                Element.PropertyChanged += OnElementPropertyChanged;
                Element.FocusChangeRequested += OnElementFocusChangeRequested;

                if (AutoPackage && Packager == null)
                    Packager = new VisualElementPackager(this);

                if (AutoTrack && Tracker == null)
                {
                    Tracker = new VisualElementTracker<TElement, TNativeElement>();
                }

                // Disabled until reason for crashes with unhandled exceptions is discovered
                // Without this some layouts may end up with improper sizes, however their children
                // will position correctly
                //Loaded += (sender, args) => {
                if (Packager != null)
                    Packager.Load();
                //};
            }


            OnElementChanged(new ElementChangedEventArgs<TElement>(oldElement, Element));

            // Effect
            var controller = (IElementController)oldElement;
            if (controller != null && controller.EffectControlProvider == this)
            {
                controller.EffectControlProvider = null;
            }

            controller = element;
            if (controller != null)
                controller.EffectControlProvider = this;
        }

        protected void SetNativeControl(TNativeElement control)
        {
            TNativeElement oldControl = Control;
            Control = control;

            if (oldControl != null)
            {
                Children.Remove(oldControl);

                //oldControl.Loaded -= OnControlLoaded;
                oldControl.AttachedToVisualTree -= Control_AttachedToVisualTree;
                oldControl.GotFocus -= OnControlGotFocus;
                oldControl.LostFocus -= OnControlLostFocus;
            }

            UpdateTracker();

            if (control == null)
                return;

            Control.HorizontalAlignment = Perspex.Layout.HorizontalAlignment.Stretch;
            Control.VerticalAlignment = Perspex.Layout.VerticalAlignment.Stretch;

            Children.Add(control);

            Element.IsNativeStateConsistent = false;
            //control.Loaded += OnControlLoaded;
            control.AttachedToVisualTree += Control_AttachedToVisualTree;
            control.GotFocus += OnControlGotFocus;
            control.LostFocus += OnControlLostFocus;

            UpdateBackgroundColor();

            if (Element != null && !string.IsNullOrEmpty(Element.AutomationId))
                SetAutomationId(Element.AutomationId);
        }

        private void Control_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            var args = new RoutedEventArgs();
            OnControlLoaded(sender, args);
        }

        protected virtual void UpdateNativeControl()
        {
            UpdateEnabled();
        }

        void UpdateEnabled()
        {
            var control = Control as Control;
            if (control != null)
                control.IsEnabled = Element.IsEnabled;
        }

        protected virtual void UpdateBackgroundColor()
        {
            Color backgroundColor = Element.BackgroundColor;
            var control = Control as TemplatedControl;
            if (control != null)
            {
                if (!backgroundColor.IsDefault)
                {
                    control.Background = backgroundColor.ToBrush();
                }
                else
                {
                    control.ClearValue(TemplatedControl.BackgroundProperty);
                }
            }
            else
            {
                if (!backgroundColor.IsDefault)
                {
                    Background = backgroundColor.ToBrush();
                }
                else
                {
                    ClearValue(BackgroundProperty);
                }
            }
        }


        void OnControlLoaded(object sender, RoutedEventArgs args)
        {
            Element.IsNativeStateConsistent = true;
        }

        #region Focus
        internal virtual void OnElementFocusChangeRequested(object sender, VisualElement.FocusRequestArgs args)
        {
            var control = Control as Control;
            if (control == null)
                return;

            if (args.Focus)
            {
                control.Focus();
                args.Result = true;
            }
            else
            {
                UnfocusControl(control);
                args.Result = true;
            }
        }

        internal void UnfocusControl(Control control)
        {
            if (control == null || !control.IsEnabled)
                return;

            control.IsEnabled = false;
            control.IsEnabled = true;
        }

        void OnControlGotFocus(object sender, RoutedEventArgs args)
        {
            ((IVisualElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        void OnControlLostFocus(object sender, RoutedEventArgs args)
        {
            ((IVisualElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }
        #endregion
        
        #region Tracker
        void OnTrackerUpdated(object sender, EventArgs e)
        {
            UpdateNativeControl();
        }

        void UpdateTracker()
        {
            if (_tracker == null)
                return;

            _tracker.Control = Control;
            _tracker.Element = Element;
            _tracker.Container = ContainerElement;
        }
        #endregion

        #region Layout
        public virtual SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Children.Count == 0)
                return new SizeRequest();

            var constraint = new Perspex.Size(widthConstraint, heightConstraint);
            TNativeElement child = Control;

            child.Measure(constraint);
            var result = new Size(Math.Ceiling(child.DesiredSize.Width), Math.Ceiling(child.DesiredSize.Height));

            return new SizeRequest(result);
        }

        protected override Perspex.Size MeasureOverride(Perspex.Size availableSize)
        {
            if (Element == null || availableSize.Width * availableSize.Height == 0)
                return new Perspex.Size(0, 0);

            Element.IsInNativeLayout = true;

            for (var i = 0; i < Element.LogicalChildren.Count; i++)
            {
                var child = Element.LogicalChildren[i] as VisualElement;
                if (child == null)
                    continue;
                IVisualElementRenderer renderer = Platform.GetRenderer(child);
                if (renderer == null)
                    continue;

                renderer.ContainerElement.Measure(availableSize);
            }

            double width = Math.Max(0, Element.Width);
            double height = Math.Max(0, Element.Height);
            var result = new Perspex.Size(width, height);
            if (Control != null)
            {
                double w = Element.Width;
                double h = Element.Height;
                if (w == -1)
                    w = availableSize.Width;
                if (h == -1)
                    h = availableSize.Height;
                w = Math.Max(0, w);
                h = Math.Max(0, h);
                Control.Measure(new Perspex.Size(w, h));
            }

            Element.IsInNativeLayout = false;

            return result;
        }

        protected override Perspex.Size ArrangeOverride(Perspex.Size finalSize)
        {
            if (Element == null || finalSize.Width * finalSize.Height == 0)
                return finalSize;

            Element.IsInNativeLayout = true;

            if (Control != null)
            {
                Control.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }

            for (var i = 0; i < Element.LogicalChildren.Count; i++)
            {
                var child = Element.LogicalChildren[i] as VisualElement;
                if (child == null)
                    continue;
                IVisualElementRenderer renderer = Platform.GetRenderer(child);
                if (renderer == null)
                    continue;
                Rectangle bounds = child.Bounds;

                renderer.ContainerElement.Arrange(new Rect(bounds.X, bounds.Y, Math.Max(0, bounds.Width), Math.Max(0, bounds.Height)));
            }

            Element.IsInNativeLayout = false;

            return finalSize;
        }
        #endregion

        #region Automation

        protected virtual void SetAutomationId(string id)
        {
            //SetValue(AutomationProperties.AutomationIdProperty, id);
            // TODO: Automation
        }

        #endregion
    }
}