// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.MouseHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils;

public class MouseHelper
{
  private static ConcurrentDictionary<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper> eles = new ConcurrentDictionary<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper>();
  public static readonly DependencyProperty IsHorizontalWheelEnabledProperty = DependencyProperty.RegisterAttached("IsHorizontalWheelEnabled", typeof (bool), typeof (MouseHelper), new PropertyMetadata((object) false, new PropertyChangedCallback(MouseHelper.OnIsHorizontalWheelEnabledChanged)));

  public static bool GetIsHorizontalWheelEnabled(DependencyObject obj)
  {
    return (bool) obj.GetValue(MouseHelper.IsHorizontalWheelEnabledProperty);
  }

  public static void SetIsHorizontalWheelEnabled(DependencyObject obj, bool value)
  {
    obj.SetValue(MouseHelper.IsHorizontalWheelEnabledProperty, (object) value);
  }

  private static void OnIsHorizontalWheelEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    bool newValue = (bool) e.NewValue;
    if (newValue == (bool) e.OldValue || !(d is FrameworkElement frameworkElement))
      return;
    KeyValuePair<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper>? cachedItem = MouseHelper.TryGetCachedItem((UIElement) frameworkElement);
    if (cachedItem.HasValue)
    {
      MouseHelper.MouseHelperWrapper mouseHelperWrapper;
      if (!newValue && MouseHelper.eles.TryRemove(cachedItem.Value.Key, out mouseHelperWrapper))
        mouseHelperWrapper.Dispose();
    }
    else if (newValue)
      MouseHelper.eles.TryAdd(new WeakReference<UIElement>((UIElement) frameworkElement), new MouseHelper.MouseHelperWrapper(frameworkElement));
  }

  private static KeyValuePair<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper>? TryGetCachedItem(
    UIElement element)
  {
    if (element == null)
      return new KeyValuePair<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper>?();
    KeyValuePair<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper>? cachedItem = new KeyValuePair<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper>?();
    lock (MouseHelper.eles)
    {
      List<WeakReference<UIElement>> weakReferenceList = new List<WeakReference<UIElement>>();
      foreach (KeyValuePair<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper> ele in MouseHelper.eles)
      {
        UIElement target;
        if (ele.Key.TryGetTarget(out target))
        {
          if (target == element)
            cachedItem = new KeyValuePair<WeakReference<UIElement>, MouseHelper.MouseHelperWrapper>?(ele);
        }
        else
        {
          ele.Value.Dispose();
          weakReferenceList.Add(ele.Key);
        }
      }
      if (weakReferenceList.Count != 0)
      {
        foreach (WeakReference<UIElement> key in weakReferenceList)
          MouseHelper.eles.TryRemove(key, out MouseHelper.MouseHelperWrapper _);
      }
      return cachedItem;
    }
  }

  private class MouseHelperWrapper : IDisposable
  {
    private bool disposedValue;
    private MouseWheelHelper helper;
    private WeakReference<FrameworkElement> element;

    public MouseHelperWrapper(FrameworkElement element)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      element.Loaded += new RoutedEventHandler(this.Element_Loaded);
      element.Unloaded += new RoutedEventHandler(this.Element_Unloaded);
      this.element = new WeakReference<FrameworkElement>(element);
    }

    private void Element_Loaded(object sender, RoutedEventArgs e)
    {
      this.helper?.Dispose();
      this.helper = new MouseWheelHelper((Visual) sender);
    }

    private void Element_Unloaded(object sender, RoutedEventArgs e)
    {
      this.helper?.Dispose();
      this.helper = (MouseWheelHelper) null;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue)
        return;
      if (disposing)
      {
        this.helper?.Dispose();
        this.helper = (MouseWheelHelper) null;
        FrameworkElement target;
        if (this.element.TryGetTarget(out target))
        {
          target.Loaded -= new RoutedEventHandler(this.Element_Loaded);
          target.Unloaded -= new RoutedEventHandler(this.Element_Unloaded);
        }
      }
      this.disposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
