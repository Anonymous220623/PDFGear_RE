// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.UIElementExtension
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Controls;

public static class UIElementExtension
{
  public static readonly DependencyProperty ExtendContextMenuDataContextProperty = DependencyProperty.RegisterAttached("ExtendContextMenuDataContext", typeof (ContextMenu), typeof (UIElementExtension), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is FrameworkElement frameworkElement2))
      return;
    if (a.OldValue is ContextMenu oldValue2)
      oldValue2.DataContext = (object) null;
    if (!(a.NewValue is ContextMenu newValue2))
      return;
    newValue2.DataContext = (object) frameworkElement2;
  })));
  public static readonly DependencyProperty TraceClickEventTagProperty = DependencyProperty.RegisterAttached("TraceClickEventTag", typeof (object), typeof (UIElementExtension), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty TraceClickEventFormatProperty = DependencyProperty.RegisterAttached("TraceClickEventFormat", typeof (string), typeof (UIElementExtension), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(UIElementExtension.OnTraceClickEventFormatPropertyChanged)));
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof (CornerRadius), typeof (UIElementExtension), new PropertyMetadata((object) new CornerRadius()));

  public static UIElement GetFirstVisualChild(DependencyObject parent)
  {
    if (parent == null)
      return (UIElement) null;
    return VisualTreeHelper.GetChildrenCount(parent) > 0 ? VisualTreeHelper.GetChild(parent, 0) as UIElement : (UIElement) null;
  }

  public static T FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
  {
    return UIElementExtension.FindVisualChildCore(parent, typeof (T), name) as T;
  }

  public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
  {
    return UIElementExtension.FindVisualChild<T>(parent, string.Empty);
  }

  private static DependencyObject FindVisualChildCore(
    DependencyObject parent,
    Type type,
    string name)
  {
    if (parent == null)
      return (DependencyObject) null;
    if (!typeof (DependencyObject).IsAssignableFrom(type))
      return (DependencyObject) null;
    int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
    for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
    {
      DependencyObject child = VisualTreeHelper.GetChild(parent, childIndex);
      if (child == null)
        return (DependencyObject) null;
      DependencyObject visualChildCore = (DependencyObject) null;
      if (type.IsAssignableFrom(child.GetType()))
      {
        if (string.IsNullOrEmpty(name))
          visualChildCore = child;
        else if (child is FrameworkElement frameworkElement && frameworkElement.Name == name || child is FrameworkContentElement frameworkContentElement && frameworkContentElement.Name == name)
          visualChildCore = child;
      }
      if (visualChildCore == null)
        visualChildCore = UIElementExtension.FindVisualChildCore(child, type, name);
      if (visualChildCore != null)
        return visualChildCore;
    }
    return (DependencyObject) null;
  }

  public static ContextMenu GetExtendContextMenuDataContext(DependencyObject obj)
  {
    return (ContextMenu) obj.GetValue(UIElementExtension.ExtendContextMenuDataContextProperty);
  }

  public static void SetExtendContextMenuDataContext(DependencyObject obj, ContextMenu value)
  {
    obj.SetValue(UIElementExtension.ExtendContextMenuDataContextProperty, (object) value);
  }

  public static object GetTraceClickEventTag(DependencyObject obj)
  {
    return obj.GetValue(UIElementExtension.TraceClickEventTagProperty);
  }

  public static void SetTraceClickEventTag(DependencyObject obj, object value)
  {
    obj.SetValue(UIElementExtension.TraceClickEventTagProperty, value);
  }

  public static string GetTraceClickEventFormat(DependencyObject obj)
  {
    return (string) obj.GetValue(UIElementExtension.TraceClickEventFormatProperty);
  }

  public static void SetTraceClickEventFormat(DependencyObject obj, string value)
  {
    obj.SetValue(UIElementExtension.TraceClickEventFormatProperty, (object) value);
  }

  private static void OnTraceClickEventFormatPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue))
      return;
    switch (d)
    {
      case ButtonBase buttonBase:
        buttonBase.Click -= new RoutedEventHandler(Btn_Click);
        if (!(e.NewValue is string))
          break;
        buttonBase.Click += new RoutedEventHandler(Btn_Click);
        break;
      case FrameworkElement frameworkElement:
        frameworkElement.PreviewMouseDown -= new MouseButtonEventHandler(Ele_PreviewMouseDown);
        if (!(e.NewValue is string))
          break;
        frameworkElement.PreviewMouseDown += new MouseButtonEventHandler(Ele_PreviewMouseDown);
        break;
    }

    static void OnElementClick(FrameworkElement _ele, string _format)
    {
      Log.WriteLog(GetElementEventMessage(_ele, _format));
    }

    static void Btn_Click(object sender, RoutedEventArgs _e)
    {
      if (!(sender is ButtonBase _ele1))
        return;
      string clickEventFormat = UIElementExtension.GetTraceClickEventFormat((DependencyObject) _ele1);
      if (string.IsNullOrEmpty(clickEventFormat))
        _ele1.Click -= new RoutedEventHandler(Btn_Click);
      else
        OnElementClick((FrameworkElement) _ele1, clickEventFormat);
    }

    static void Ele_PreviewMouseDown(object sender, MouseButtonEventArgs _e)
    {
      if (!(sender is FrameworkElement _ele2))
        return;
      string clickEventFormat = UIElementExtension.GetTraceClickEventFormat((DependencyObject) _ele2);
      if (string.IsNullOrEmpty(clickEventFormat))
        _ele2.PreviewMouseDown -= new MouseButtonEventHandler(Ele_PreviewMouseDown);
      else
        OnElementClick(_ele2, clickEventFormat);
    }

    static string GetViewTypeName(FrameworkElement _element)
    {
      Window window = Window.GetWindow((DependencyObject) _element);
      return window != null ? window.GetType().Name : string.Empty;
    }

    static string GetElementEventMessage(FrameworkElement _element, string _format)
    {
      return Regex.Replace(_format, "\\$\\{(.+?)\\}", (MatchEvaluator) (m =>
      {
        if (m.Success && m.Groups.Count > 1)
        {
          switch (m.Groups[1].Value.ToUpperInvariant())
          {
            case "VIEW":
              string viewTypeName = GetViewTypeName(_element);
              return !string.IsNullOrEmpty(viewTypeName) ? viewTypeName : m.Value;
            case "NAME":
              string name1 = _element.Name;
              return !string.IsNullOrEmpty(name1) ? name1 : m.Value;
            case "TYPE":
              string name2 = _element.GetType().Name;
              return !string.IsNullOrEmpty(name2) ? name2 : m.Value;
            case "CONTENT":
              string elementEventMessage1 = GetContent(_element)?.ToString() ?? "null";
              if (string.IsNullOrEmpty(elementEventMessage1))
                elementEventMessage1 = "empty";
              return elementEventMessage1;
            case "TAG":
              string elementEventMessage2 = _element.Tag?.ToString() ?? "null";
              if (string.IsNullOrEmpty(elementEventMessage2))
                elementEventMessage2 = "empty";
              return elementEventMessage2;
            case "TRACETAG":
              string elementEventMessage3 = UIElementExtension.GetTraceClickEventTag((DependencyObject) _element).ToString() ?? "null";
              if (string.IsNullOrEmpty(elementEventMessage3))
                elementEventMessage3 = "empty";
              return elementEventMessage3;
          }
        }
        return m.Value;
      }));
    }

    static object GetContent(FrameworkElement _element)
    {
      switch (_element)
      {
        case null:
          return (object) null;
        case ContentControl contentControl:
          return contentControl.Content;
        case ContentPresenter contentPresenter:
          return contentPresenter.Content;
        default:
          return (object) null;
      }
    }
  }

  public static CornerRadius GetCornerRadius(DependencyObject obj)
  {
    return (CornerRadius) obj.GetValue(UIElementExtension.CornerRadiusProperty);
  }

  public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
  {
    obj.SetValue(UIElementExtension.CornerRadiusProperty, (object) value);
  }

  public static bool ForceKillFocus(this UIElement focusedElement)
  {
    return focusedElement.ForceKillFocus((Func<UIElement, bool>) null);
  }

  public static bool ForceKillFocus(this UIElement focusedElement, Func<UIElement, bool> predicate)
  {
    if (focusedElement == null || !focusedElement.IsFocused)
      return false;
    Visual rootVisual = PresentationSource.FromVisual((Visual) focusedElement)?.RootVisual;
    if (rootVisual != null && rootVisual != focusedElement)
    {
      Stack<Visual> visualStack = new Stack<Visual>();
      visualStack.Push(rootVisual);
      while (visualStack.Count > 0)
      {
        Visual reference = visualStack.Pop();
        int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) reference);
        for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
        {
          if (VisualTreeHelper.GetChild((DependencyObject) reference, childIndex) is Visual child && child != focusedElement)
          {
            if (child is UIElement _ele && IsFocusable(_ele) && (predicate != null ? (predicate(_ele) ? 1 : 0) : 1) != 0 && _ele.Focus())
              return true;
            visualStack.Push(child);
          }
        }
      }
    }
    return false;

    static bool IsFocusable(UIElement _ele)
    {
      return _ele != null && _ele.Focusable && _ele.Visibility == Visibility.Visible && _ele.IsEnabled && (!(_ele is FrameworkElement frameworkElement) || frameworkElement.IsLoaded);
    }
  }
}
