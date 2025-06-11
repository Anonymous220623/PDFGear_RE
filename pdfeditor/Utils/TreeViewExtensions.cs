// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.TreeViewExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Utils;

public static class TreeViewExtensions
{
  public static TreeViewItem TreeViewItemFromElement(
    this TreeView treeView,
    object item,
    Func<object, object> parentSelector)
  {
    return TreeViewExtensions.GetContainerFast(treeView, item, parentSelector);
  }

  public static TreeViewItem TreeViewItemFromElement(this TreeView treeView, ITreeViewNode item)
  {
    return TreeViewExtensions.GetContainerFast(treeView, (object) item, (Func<object, object>) (o => !(o is ITreeViewNode treeViewNode) ? (object) null : (object) treeViewNode.Parent));
  }

  public static async Task ScrollIntoViewAsync(
    this TreeView treeView,
    ITreeViewNode item,
    ScrollIntoViewOrientation orientation = ScrollIntoViewOrientation.Both)
  {
    await treeView.ScrollIntoViewAsync((object) item, (Func<object, object>) (o => !(o is ITreeViewNode treeViewNode) ? (object) null : (object) treeViewNode.Parent), orientation).ConfigureAwait(false);
  }

  public static async Task ScrollIntoViewAsync(
    this TreeView treeView,
    object item,
    Func<object, object> parentSelector,
    ScrollIntoViewOrientation orientation = ScrollIntoViewOrientation.Both)
  {
    ItemContainerGenerator generator;
    Stack<object> stack;
    TreeViewItem lastContainer;
    if (treeView == null)
    {
      generator = (ItemContainerGenerator) null;
      stack = (Stack<object>) null;
      lastContainer = (TreeViewItem) null;
    }
    else if (item == null)
    {
      generator = (ItemContainerGenerator) null;
      stack = (Stack<object>) null;
      lastContainer = (TreeViewItem) null;
    }
    else if (treeView == null)
    {
      generator = (ItemContainerGenerator) null;
      stack = (Stack<object>) null;
      lastContainer = (TreeViewItem) null;
    }
    else
    {
      generator = treeView.ItemContainerGenerator;
      if (generator == null)
      {
        generator = (ItemContainerGenerator) null;
        stack = (Stack<object>) null;
        lastContainer = (TreeViewItem) null;
      }
      else
      {
        if (parentSelector == null)
        {
          if (!(item is ITreeViewNode))
          {
            generator = (ItemContainerGenerator) null;
            stack = (Stack<object>) null;
            lastContainer = (TreeViewItem) null;
            return;
          }
          parentSelector = (Func<object, object>) (i => !(i is ITreeViewNode treeViewNode) ? (object) null : (object) treeViewNode.Parent);
        }
        TreeViewItem containerFast = TreeViewExtensions.GetContainerFast(treeView, item, parentSelector);
        if (containerFast != null)
        {
          containerFast.BringIntoView();
          generator = (ItemContainerGenerator) null;
          stack = (Stack<object>) null;
          lastContainer = (TreeViewItem) null;
        }
        else
        {
          stack = new Stack<object>();
          object obj1 = item;
          for (object obj2 = parentSelector(obj1); obj2 != null; obj2 = parentSelector(obj1))
          {
            stack.Push(obj1);
            obj1 = obj2;
          }
          stack.Push(obj1);
          await TreeViewExtensions.UpdateLayoutAsync(treeView.Dispatcher, generator, (UIElement) treeView);
          lastContainer = (TreeViewItem) null;
          while (stack.Count > 0)
          {
            object cur = stack.Pop();
            TreeViewItem containerAsync = await TreeViewExtensions.GetContainerAsync(treeView.Dispatcher, generator, cur);
            if (containerAsync == null)
            {
              ReadOnlyCollection<object> items = generator.Items;
              // ISSUE: explicit non-virtual call
              int index = items != null ? __nonvirtual (items.IndexOf(cur)) : -1;
              if (index >= 0)
              {
                VirtualizingPanel panel = UIElementExtension.FindVisualChild<VirtualizingPanel>((DependencyObject) ((ItemsControl) lastContainer ?? (ItemsControl) treeView));
                if (panel != null)
                {
                  await TreeViewExtensions.BringIndexIntoViewPublicAsync(treeView.Dispatcher, generator, panel, index, orientation);
                  await TreeViewExtensions.UpdateLayoutAsync(treeView.Dispatcher, generator, (UIElement) panel);
                  containerAsync = await TreeViewExtensions.GetContainerAsync(treeView.Dispatcher, generator, cur);
                }
                panel = (VirtualizingPanel) null;
              }
            }
            if (containerAsync == null)
            {
              generator = (ItemContainerGenerator) null;
              stack = (Stack<object>) null;
              lastContainer = (TreeViewItem) null;
              return;
            }
            containerAsync.IsExpanded = true;
            lastContainer = containerAsync;
            if (stack.Count == 0)
              containerAsync.BringIntoView();
            generator = containerAsync.ItemContainerGenerator;
            await TreeViewExtensions.UpdateLayoutAsync(treeView.Dispatcher, generator, (UIElement) containerAsync);
            cur = (object) null;
          }
          generator = (ItemContainerGenerator) null;
          stack = (Stack<object>) null;
          lastContainer = (TreeViewItem) null;
        }
      }
    }
  }

  private static ScrollViewer GetNearestScrollViewer(UIElement element)
  {
    if (element is ItemsControl _element1)
    {
      ScrollViewer child = FindChild((UIElement) _element1);
      if (child != null)
        return child;
    }
    UIElement reference = element;
    ScrollViewer nearestScrollViewer;
    do
    {
      reference = VisualTreeHelper.GetParent((DependencyObject) reference) as UIElement;
      nearestScrollViewer = reference as ScrollViewer;
    }
    while (reference != null && nearestScrollViewer == null);
    return nearestScrollViewer;

    static ScrollViewer FindChild(UIElement _element)
    {
      if (_element == null)
        return (ScrollViewer) null;
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) _element); ++childIndex)
      {
        switch (VisualTreeHelper.GetChild((DependencyObject) _element, childIndex))
        {
          case ScrollViewer child1:
            return child1;
          case UIElement _element2:
            ScrollViewer child1 = FindChild(_element2);
            if (child1 != null)
              return child1;
            break;
        }
      }
      return (ScrollViewer) null;
    }
  }

  private static async Task UpdateLayoutAsync(
    Dispatcher dispatcher,
    ItemContainerGenerator generator,
    UIElement element)
  {
    if (dispatcher == null)
      throw new ArgumentNullException(nameof (dispatcher));
    if (generator == null)
      throw new ArgumentNullException(nameof (generator));
    if (element == null)
      throw new ArgumentNullException(nameof (element));
    if (generator.Status == GeneratorStatus.ContainersGenerated)
    {
      try
      {
        element.UpdateLayout();
      }
      catch
      {
      }
    }
    else
    {
      try
      {
        await dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
        {
          try
          {
            element.UpdateLayout();
          }
          catch
          {
          }
        }));
      }
      catch
      {
      }
    }
  }

  private static async Task BringIndexIntoViewPublicAsync(
    Dispatcher dispatcher,
    ItemContainerGenerator generator,
    VirtualizingPanel panel,
    int index,
    ScrollIntoViewOrientation orientation)
  {
    if (dispatcher == null)
      throw new ArgumentNullException(nameof (dispatcher));
    if (generator == null)
      throw new ArgumentNullException(nameof (generator));
    if (panel == null)
      throw new ArgumentNullException(nameof (panel));
    if (generator.Status == GeneratorStatus.ContainersGenerated)
    {
      try
      {
        panel.BringIndexIntoViewPublic(index);
      }
      catch
      {
      }
    }
    else
    {
      try
      {
        await dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
        {
          try
          {
            panel.BringIndexIntoViewPublic(index);
          }
          catch
          {
          }
        }));
      }
      catch
      {
      }
    }
  }

  private static async Task<TreeViewItem> GetContainerAsync(
    Dispatcher dispatcher,
    ItemContainerGenerator generator,
    object item)
  {
    if (dispatcher == null)
      throw new ArgumentNullException(nameof (dispatcher));
    if (generator == null)
      throw new ArgumentNullException(nameof (generator));
    if (item == null)
      return (TreeViewItem) null;
    if (generator.Status == GeneratorStatus.ContainersGenerated)
      return generator.ContainerFromItem(item) as TreeViewItem;
    TreeViewItem container = (TreeViewItem) null;
    try
    {
      await dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => container = generator.ContainerFromItem(item) as TreeViewItem));
    }
    catch
    {
    }
    return container;
  }

  private static TreeViewItem GetContainerFast(
    TreeView treeView,
    object item,
    Func<object, object> parentSelector)
  {
    if (treeView == null || item == null || parentSelector == null)
      return (TreeViewItem) null;
    Stack<object> objectStack = new Stack<object>();
    for (; item != null; item = parentSelector(item))
      objectStack.Push(item);
    ItemsControl containerFast = (ItemsControl) treeView;
    while (objectStack.Count > 0 && containerFast != null)
      containerFast = containerFast.ItemContainerGenerator.ContainerFromItem(objectStack.Pop()) as ItemsControl;
    return containerFast as TreeViewItem;
  }
}
