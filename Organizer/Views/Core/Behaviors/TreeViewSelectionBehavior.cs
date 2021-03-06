﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Organizer.Views.Core.Behaviors
{
    internal class TreeViewSelectionBehavior : Behavior<TreeView>
    {
        public delegate bool IsChildOfPredicate(object nodeA, object nodeB);

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object),
                typeof(TreeViewSelectionBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemChanged));

        public static readonly DependencyProperty HierarchyPredicateProperty =
            DependencyProperty.Register(nameof(HierarchyPredicate), typeof(IsChildOfPredicate),
                typeof(TreeViewSelectionBehavior),
                new FrameworkPropertyMetadata(null));

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (TreeViewSelectionBehavior)sender;
            if (behavior._modelHandled) return;

            if (behavior.AssociatedObject == null)
                return;

            behavior._modelHandled = true;
            behavior.UpdateAllTreeViewItems();
            behavior._modelHandled = false;
        }

        private readonly EventSetter _treeViewItemEventSetter;
        private bool _modelHandled;

        // Bindable selected item
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        // Predicate that checks if two items are hierarchically related
        public IsChildOfPredicate HierarchyPredicate
        {
            get => (IsChildOfPredicate)GetValue(HierarchyPredicateProperty);
            set => SetValue(HierarchyPredicateProperty, value);
        }

        public TreeViewSelectionBehavior()
        {
            _treeViewItemEventSetter = new EventSetter(
                FrameworkElement.LoadedEvent,
                new RoutedEventHandler(OnTreeViewItemLoaded));
        }

        // Update state of all items starting with given, with optional recursion
        private void UpdateTreeViewItem(TreeViewItem item, bool recurse)
        {
            if (SelectedItem == null) return;
            var model = item.DataContext;

            // Recurse into children
            if (recurse)
            {
                foreach (var subitem in item.Items)
                {
                    var tvi = item.ItemContainerGenerator.ContainerFromItem(subitem) as TreeViewItem;
                    if (tvi != null)
                        UpdateTreeViewItem(tvi, true);
                }
            }
        }

        // Update state of all items
        private void UpdateAllTreeViewItems()
        {
            var treeView = AssociatedObject;
            foreach (var item in treeView.Items)
            {
                var tvi = treeView.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (tvi != null)
                    UpdateTreeViewItem(tvi, true);
            }
        }

        // Inject Loaded event handler into ItemContainerStyle
        private void UpdateTreeViewItemStyle()
        {
            if (AssociatedObject.ItemContainerStyle == null)
                AssociatedObject.ItemContainerStyle = new Style(
                    typeof(TreeViewItem),
                    Application.Current.TryFindResource(typeof(TreeViewItem)) as Style);

            if (!AssociatedObject.ItemContainerStyle.Setters.Contains(_treeViewItemEventSetter))
                AssociatedObject.ItemContainerStyle.Setters.Add(_treeViewItemEventSetter);
        }

        private void OnTreeViewItemsChanged(object sender,
            NotifyCollectionChangedEventArgs args)
        {
            UpdateAllTreeViewItems();
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            if (_modelHandled) return;
            if (AssociatedObject.Items.SourceCollection == null) return;
            SelectedItem = args.NewValue;
        }

        private void OnTreeViewItemLoaded(object sender, RoutedEventArgs args)
        {
            UpdateTreeViewItem((TreeViewItem)sender, false);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
            ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged += OnTreeViewItemsChanged;

            UpdateTreeViewItemStyle();
            _modelHandled = true;
            UpdateAllTreeViewItems();
            _modelHandled = false;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.ItemContainerStyle?.Setters?.Remove(_treeViewItemEventSetter);
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
                ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged -= OnTreeViewItemsChanged;
            }
        }
    }
}
