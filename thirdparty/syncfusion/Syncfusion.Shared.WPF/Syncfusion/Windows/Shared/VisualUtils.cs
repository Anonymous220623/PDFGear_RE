// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VisualUtils
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Shared;

public sealed class VisualUtils
{
  private const string RootPopupTypeName = "System.Windows.Controls.Primitives.PopupRoot";
  public static Type RootPopupType = Type.GetType(Assembly.CreateQualifiedName(Assembly.GetAssembly(typeof (FrameworkElement)).FullName, "System.Windows.Controls.Primitives.PopupRoot"));

  private VisualUtils()
  {
  }

  public static Visual FindRootVisual(Visual startingFrom)
  {
    Visual rootVisual = (Visual) null;
    if (startingFrom != null)
    {
      rootVisual = startingFrom;
      while ((startingFrom = VisualTreeHelper.GetParent((DependencyObject) startingFrom) as Visual) != null)
        rootVisual = startingFrom;
    }
    return rootVisual;
  }

  public static Visual FindAncestor(Visual startingFrom, Type typeAncestor)
  {
    if (startingFrom == null)
      return (Visual) null;
    DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject) startingFrom);
    while (parent != null && !typeAncestor.IsInstanceOfType((object) parent))
      parent = VisualTreeHelper.GetParent(parent);
    return parent as Visual;
  }

  public static Visual FindLogicalAncestor(Visual startingFrom, Type typeAncestor)
  {
    if (startingFrom == null)
      return (Visual) null;
    DependencyObject parent = LogicalTreeHelper.GetParent((DependencyObject) startingFrom);
    while (parent != null && !typeAncestor.IsInstanceOfType((object) parent))
      parent = LogicalTreeHelper.GetParent(parent);
    return parent as Visual;
  }

  public static Visual FindDescendant(Visual startingFrom, Type typeDescendant)
  {
    Visual descendant = (Visual) null;
    bool flag = false;
    int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) startingFrom);
    for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
    {
      Visual child = VisualTreeHelper.GetChild((DependencyObject) startingFrom, childIndex) as Visual;
      if (typeDescendant.IsInstanceOfType((object) child))
      {
        descendant = child;
        flag = true;
      }
      if (!flag)
      {
        if (child != null)
        {
          descendant = VisualUtils.FindDescendant(child, typeDescendant);
          if (descendant != null)
            break;
        }
      }
      else
        break;
    }
    return descendant;
  }

  public static Visual FindDescendant(Visual startingFrom, string typeNameDescendant)
  {
    Visual descendant = (Visual) null;
    bool flag = false;
    int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) startingFrom);
    for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
    {
      if (VisualTreeHelper.GetChild((DependencyObject) startingFrom, childIndex) is Visual child && child.GetType().Name == typeNameDescendant)
      {
        descendant = child;
        flag = true;
      }
      if (!flag)
      {
        if (child != null)
        {
          descendant = VisualUtils.FindDescendant(child, typeNameDescendant);
          if (descendant != null)
            break;
        }
      }
      else
        break;
    }
    return descendant;
  }

  public static IEnumerable<Visual> EnumChildrenOfType(Visual rootelement, Type typeChild)
  {
    int iCount = rootelement != null ? VisualTreeHelper.GetChildrenCount((DependencyObject) rootelement) : throw new ArgumentNullException(nameof (rootelement));
    for (int i = 0; i < iCount; ++i)
    {
      DependencyObject child = VisualTreeHelper.GetChild((DependencyObject) rootelement, i);
      if (child is Visual)
      {
        Visual visual = (Visual) child;
        if (typeChild.IsInstanceOfType((object) visual))
          yield return visual;
        foreach (Visual vis in VisualUtils.EnumChildrenOfType(visual, typeChild))
          yield return vis;
      }
    }
  }

  public static Panel GetItemsPanel(ItemsControl owner, Type panelType)
  {
    Panel itemsPanel = (Panel) null;
    if (owner != null && panelType != (Type) null)
    {
      foreach (Visual visual in VisualUtils.EnumChildrenOfType((Visual) owner, panelType))
      {
        if (visual is Panel element && VisualUtils.GetItemsControlFromChildren((FrameworkElement) element) == owner)
        {
          itemsPanel = element;
          break;
        }
      }
    }
    return itemsPanel;
  }

  public static ItemsControl GetItemsControlFromChildren(FrameworkElement element)
  {
    controlFromChildren = (ItemsControl) null;
    if (element != null && !(element is ItemsControl controlFromChildren))
    {
      while (element != null)
      {
        element = VisualTreeHelper.GetParent((DependencyObject) element) as FrameworkElement;
        if (element is ItemsControl)
        {
          controlFromChildren = (ItemsControl) element;
          break;
        }
      }
    }
    return controlFromChildren;
  }

  public static IEnumerable<DependencyObject> EnumLogicalChildrenOfType(
    DependencyObject rootelement,
    Type typeChild)
  {
    foreach (object obj in LogicalTreeHelper.GetChildren(rootelement))
    {
      if (obj is DependencyObject)
      {
        if (typeChild.IsInstanceOfType(obj))
          yield return obj as DependencyObject;
        foreach (DependencyObject obj1 in VisualUtils.EnumLogicalChildrenOfType(obj as DependencyObject, typeChild))
          yield return obj1;
      }
    }
  }

  public static bool IsDescendant(DependencyObject reference, DependencyObject node)
  {
    bool flag = false;
    while (node != null)
    {
      if (node == reference)
      {
        flag = true;
        break;
      }
      if (node.GetType() == VisualUtils.RootPopupType)
      {
        Popup parent = (node as FrameworkElement).Parent as Popup;
        node = (DependencyObject) parent;
        if (parent != null)
        {
          node = parent.Parent;
          if (node == null)
            node = (DependencyObject) parent.PlacementTarget;
        }
      }
      else
        node = VisualUtils.FindParent(node);
    }
    return flag;
  }

  public static void InvalidateParentMeasure(FrameworkElement element)
  {
    if (!(VisualTreeHelper.GetParent((DependencyObject) element) is FrameworkElement parent))
      return;
    parent.InvalidateMeasure();
  }

  public static FrameworkElement FindSomeParent(FrameworkElement rootelement, Type typeParent)
  {
    FrameworkElement parent = rootelement.Parent as FrameworkElement;
    while (parent != null && !typeParent.IsInstanceOfType((object) parent))
      parent = parent.Parent as FrameworkElement;
    return parent;
  }

  public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
  {
    DependencyObject parent = VisualTreeHelper.GetParent(child);
    if (parent == null)
      return default (T);
    return parent is T obj ? obj : VisualUtils.FindVisualParent<T>(parent);
  }

  [Obsolete("SetDependencyPropretyUsedByAnimation is deprecated, please use SetDependencyPropertyUsedByAnimation instead.")]
  public static void SetDependencyPropretyUsedByAnimation(
    UIElement targetElement,
    DependencyProperty dependencyProperty,
    double value)
  {
    VisualUtils.SetDependencyPropertyUsedByAnimation(targetElement, dependencyProperty, value);
  }

  public static void SetDependencyPropertyUsedByAnimation(
    UIElement targetElement,
    DependencyProperty dependencyProperty,
    double value)
  {
    targetElement.BeginAnimation(dependencyProperty, (AnimationTimeline) null);
    targetElement.SetValue(dependencyProperty, (object) value);
  }

  public static bool HasChildOfType(Visual rootelEment, Type searchType)
  {
    bool flag = false;
    int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) rootelEment);
    for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
    {
      Visual child = VisualTreeHelper.GetChild((DependencyObject) rootelEment, childIndex) as Visual;
      flag = searchType.IsInstanceOfType((object) child);
      if (!flag)
      {
        if (child != null)
        {
          flag = VisualUtils.HasChildOfType(child, searchType);
          if (flag)
            break;
        }
      }
      else
        break;
    }
    return flag;
  }

  public static Point PointToScreen(UIElement visual, Point point)
  {
    if (PermissionHelper.HasUnmanagedCodePermission)
      return PermissionHelper.GetSafePointToScreen((Visual) visual, point);
    Point pointRelativeTo = VisualUtils.GetPointRelativeTo(visual, (UIElement) null);
    return new Point(pointRelativeTo.X + point.X, pointRelativeTo.Y + point.Y);
  }

  public static Point GetPointRelativeTo(UIElement visual, UIElement relativeTo)
  {
    Point position1 = Mouse.PrimaryDevice.GetPosition((IInputElement) visual);
    Point position2 = Mouse.PrimaryDevice.GetPosition((IInputElement) relativeTo);
    return new Point(position2.X - position1.X, position2.Y - position1.Y);
  }

  private static DependencyObject FindParent(DependencyObject d)
  {
    ContentElement reference1 = !(d is Visual reference2) ? d as ContentElement : (ContentElement) null;
    if (reference1 != null)
    {
      d = ContentOperations.GetParent(reference1);
      if (d != null)
        return d;
      if (reference1 is FrameworkContentElement frameworkContentElement)
        return frameworkContentElement.Parent;
    }
    else if (reference2 != null)
      return VisualTreeHelper.GetParent((DependencyObject) reference2);
    return (DependencyObject) null;
  }

  public static Page GetPageFromChildren(FrameworkElement element)
  {
    pageFromChildren = (Page) null;
    if (element != null && !(element is Page pageFromChildren))
    {
      while (element != null)
      {
        element = VisualTreeHelper.GetParent((DependencyObject) element) as FrameworkElement;
        if (element is Page)
        {
          pageFromChildren = (Page) element;
          break;
        }
      }
    }
    return pageFromChildren;
  }

  public static Window GetWindowFromChildren(FrameworkElement element)
  {
    windowFromChildren = (Window) null;
    if (element != null && !(element is Window windowFromChildren))
    {
      while (element != null)
      {
        element = VisualTreeHelper.GetParent((DependencyObject) element) as FrameworkElement;
        if (element is Window)
        {
          windowFromChildren = (Window) element;
          break;
        }
      }
    }
    return windowFromChildren;
  }
}
