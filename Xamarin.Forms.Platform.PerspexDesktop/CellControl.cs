﻿using Perspex;
using Perspex.Controls;
using Perspex.Controls.Templates;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class CellControl : /*ContentControl*/ Perspex.Controls.StackPanel
    {
        /// <summary>
        /// Defines the <see cref="Cell"/> property.
        /// </summary>
        public static readonly StyledProperty<object> CellProperty =
            PerspexProperty.Register<CellControl, object>(nameof(Cell));

        /// <summary>
        /// Defines the <see cref="IsGroupHeader"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsGroupHeaderProperty =
            PerspexProperty.Register<CellControl, bool>(nameof(IsGroupHeader));


        internal static readonly BindableProperty MeasuredEstimateProperty = BindableProperty.Create("MeasuredEstimate", typeof(double), typeof(ListView), -1d);
        readonly Lazy<ListView> _listView;
        readonly PropertyChangedEventHandler _propertyChangedHandler;

        IList<MenuItem> _contextActions;
        IDataTemplate _currentTemplate;
        bool _isListViewRealized;
        object _newValue;

        public CellControl()
        {
            _listView = new Lazy<ListView>(GetListView);

            //DataContextChanged += OnDataContextChanged;            

            //Unloaded += (sender, args) =>
            //{
            //    Cell cell = Cell;
            //    if (cell != null)
            //        cell.SendDisappearing();
            //};

            _propertyChangedHandler = OnCellPropertyChanged;
        }

        public Cell Cell
        {
            get { return (Cell)GetValue(CellProperty); }
            set
            {
                var oldCell = Cell;
                var newCell = value;
                SetSource(oldCell, newCell);
                SetValue(CellProperty, value);
            }
        }

        public bool IsGroupHeader
        {
            get { return (bool)GetValue(IsGroupHeaderProperty); }
            set { SetValue(IsGroupHeaderProperty, value); }
        }

        //protected Control CellContent
        //{
        //    get { return (Control)Content; }
        //}

        //protected override Perspex.Size MeasureOverride(Perspex.Size availableSize)
        //{
        //    ListView lv = _listView.Value;

        //    return new Perspex.Size(100, 100);

        //    // set the Cell now that we have a reference to the ListView, since it will have been skipped
        //    // on DataContextChanged.
        //    if (_newValue != null)
        //    {
        //        SetCell(_newValue);
        //        _newValue = null;
        //    }

        //    if (Content == null)
        //    {
        //        if (lv != null)
        //        {
        //            if (lv.HasUnevenRows)
        //            {
        //                var estimate = (double)lv.GetValue(MeasuredEstimateProperty);
        //                if (estimate > -1)
        //                    return new Perspex.Size(availableSize.Width, estimate);
        //            }
        //            else
        //            {
        //                double rowHeight = lv.RowHeight;
        //                if (rowHeight > -1)
        //                    return new Perspex.Size(availableSize.Width, rowHeight);
        //            }
        //        }

        //        return new Perspex.Size(0, 0);
        //    }

        //    // Children still need measure called on them
        //    Perspex.Size result = base.MeasureOverride(availableSize);

        //    if (lv != null)
        //    {
        //        lv.SetValue(MeasuredEstimateProperty, result.Height);
        //    }

        //    return result;
        //}

        static string GetDisplayTextFromGroup(ListView lv, TemplatedItemsList<ItemsView<Cell>, Cell> group)
        {
            string displayBinding = null;

            if (lv.GroupDisplayBinding != null)
                displayBinding = group.Name;

            if (lv.GroupShortNameBinding != null)
                displayBinding = group.ShortName;

            // TODO: what if they set both? should it default to the ShortName, like it will here?
            // ShortNames binding did not appear to be functional before.
            return displayBinding;
        }

        static TemplatedItemsList<ItemsView<Cell>, Cell> GetGroup(object newContext, ListView lv)
        {
            int groupIndex = lv.TemplatedItems.GetGlobalIndexOfGroup(newContext);
            TemplatedItemsList<ItemsView<Cell>, Cell> group = lv.TemplatedItems.GetGroup(groupIndex);
            return group;
        }

        ListView GetListView()
        {            
            var parent = VisualTreeHelper.GetParent(this);
            while (parent != null)
            {
                var lv = parent as ListViewRenderer;
                if (lv != null)
                {
                    _isListViewRealized = true;
                    return lv.Element;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        IDataTemplate GetTemplate(Cell cell)
        {
            var renderer = Registrar.Registered.GetHandler<ICellRenderer>(cell.GetType());
            return renderer.GetTemplate(cell);
        }

        void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HasContextActions")
            {
                SetupContextMenu();
            }
        }

        //void OnClick(object sender, PointerRoutedEventArgs e)
        //{
        //    PointerPoint point = e.GetCurrentPoint(CellContent);
        //    if (point.Properties.PointerUpdateKind != PointerUpdateKind.RightButtonReleased)
        //        return;

        //    OpenContextMenu();
        //}

        //void OnContextActionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    var flyout = FlyoutBase.GetAttachedFlyout(CellContent) as MenuFlyout;
        //    if (flyout != null)
        //    {
        //        flyout.Items.Clear();
        //        SetupMenuItems(flyout);
        //    }
        //}

        protected override void OnDataContextChanged()
        {
            base.OnDataContextChanged();
            // We don't want to set the Cell until the ListView is realized, just in case the 
            // Cell has an ItemTemplate. Instead, we'll store the new data item, and it will be
            // set on MeasureOverrideDelegate. However, if the parent is a TableView, we'll already 
            // have a complete Cell object to work with, so we can move ahead.
            if (_isListViewRealized || DataContext is Cell)
                SetCell(DataContext);
            else if (DataContext != null)
                _newValue = DataContext;
        }

        //void OnLongTap(object sender, HoldingRoutedEventArgs e)
        //{
        //    if (e.HoldingState == HoldingState.Started)
        //        OpenContextMenu();
        //}

        //void OnOpenContext(object sender, RightTappedRoutedEventArgs e)
        //{
        //    FlyoutBase.ShowAttachedFlyout(CellContent);
        //}

        //void OpenContextMenu()
        //{
        //    if (FlyoutBase.GetAttachedFlyout(CellContent) == null)
        //    {
        //        var flyout = new MenuFlyout();
        //        SetupMenuItems(flyout);

        //        ((INotifyCollectionChanged)Cell.ContextActions).CollectionChanged += OnContextActionsChanged;

        //        _contextActions = Cell.ContextActions;
        //        FlyoutBase.SetAttachedFlyout(CellContent, flyout);
        //    }

        //    FlyoutBase.ShowAttachedFlyout(CellContent);
        //}

        void SetCell(object newContext)
        {
            var cell = newContext as Cell;

            if (ReferenceEquals(Cell?.BindingContext, newContext))
                return;

            // If there is a ListView, load the Cell content from the ItemTemplate.
            // Otherwise, the given Cell is already a templated Cell from a TableView.
            ListView lv = _listView.Value;
            if (lv != null)
            {
                bool isGroupHeader = IsGroupHeader;
                DataTemplate template = isGroupHeader ? lv.GroupHeaderTemplate : lv.ItemTemplate;

                if (template is DataTemplateSelector)
                {
                    template = ((DataTemplateSelector)template).SelectTemplate(newContext, lv);
                }

                if (template != null)
                {
                    cell = template.CreateContent() as Cell;
                    cell.BindingContext = newContext;
                }
                else
                {
                    string textContent = newContext.ToString();

                    if (isGroupHeader)
                    {
                        TemplatedItemsList<ItemsView<Cell>, Cell> group = GetGroup(newContext, lv);
                        textContent = GetDisplayTextFromGroup(lv, group);
                    }

                    cell = lv.CreateDefaultCell(textContent);
                }

                // A TableView cell should already have its parent,
                // but we need to set the parent for a ListView cell.
                cell.Parent = lv;

                // This provides the Group Header styling (e.g., larger font, etc.) when the
                // template is loaded later.
                TemplatedItemsList<ItemsView<Cell>, Cell>.SetIsGroupHeader(cell, isGroupHeader);
            }

            Cell = cell;
        }

        void SetSource(Cell oldCell, Cell newCell)
        {
            if (oldCell != null)
            {
                oldCell.PropertyChanged -= _propertyChangedHandler;
                oldCell.SendDisappearing();
            }

            if (newCell != null)
            {
                newCell.SendAppearing();

                UpdateContent(newCell);
                SetupContextMenu();

                newCell.PropertyChanged += _propertyChangedHandler;
            }
        }

        void SetupContextMenu()
        {
            //if (CellContent == null || Cell == null)
            //    return;

            //if (!Cell.HasContextActions)
            //{
            //    CellContent.Holding -= OnLongTap;
            //    CellContent.PointerReleased -= OnClick;
            //    if (_contextActions != null)
            //    {
            //        ((INotifyCollectionChanged)_contextActions).CollectionChanged -= OnContextActionsChanged;
            //        _contextActions = null;
            //    }

            //    FlyoutBase.SetAttachedFlyout(CellContent, null);
            //    return;
            //}

            //CellContent.PointerReleased += OnClick;
            //CellContent.Holding += OnLongTap;
        }

        //void SetupMenuItems(MenuFlyout flyout)
        //{
        //    foreach (MenuItem item in Cell.ContextActions)
        //    {
        //        var flyoutItem = new MenuFlyoutItem();
        //        flyoutItem.SetBinding(MenuFlyoutItem.TextProperty, "Text");
        //        flyoutItem.Command = new MenuItemCommand(item);
        //        flyoutItem.DataContext = item;

        //        flyout.Items.Add(flyoutItem);
        //    }
        //}

        void UpdateContent(Cell newCell)
        {
            IDataTemplate dt = GetTemplate(newCell);
            //if (dt != _currentTemplate || Content == null)
            //{
            //    _currentTemplate = dt;
            //    // Content
            //    Content = dt.Build(newCell);
            //}

            //((Control)Content).DataContext = newCell;
            if(dt != _currentTemplate && Children.Count == 0)
            {
                _currentTemplate = dt;
                Children.Add(dt.Build(newCell));
            }
        }
    }
}