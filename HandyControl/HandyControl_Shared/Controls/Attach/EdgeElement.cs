// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.EdgeElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class EdgeElement
{
  public static readonly DependencyProperty LeftContentProperty = DependencyProperty.RegisterAttached("LeftContent", typeof (object), typeof (EdgeElement), new PropertyMetadata((object) null, new PropertyChangedCallback(EdgeElement.OnEdgeContentChanged)));
  public static readonly DependencyProperty TopContentProperty = DependencyProperty.RegisterAttached("TopContent", typeof (object), typeof (EdgeElement), new PropertyMetadata((object) null, new PropertyChangedCallback(EdgeElement.OnEdgeContentChanged)));
  public static readonly DependencyProperty RightContentProperty = DependencyProperty.RegisterAttached("RightContent", typeof (object), typeof (EdgeElement), new PropertyMetadata((object) null, new PropertyChangedCallback(EdgeElement.OnEdgeContentChanged)));
  public static readonly DependencyProperty BottomContentProperty = DependencyProperty.RegisterAttached("BottomContent", typeof (object), typeof (EdgeElement), new PropertyMetadata((object) null, new PropertyChangedCallback(EdgeElement.OnEdgeContentChanged)));
  public static readonly DependencyProperty ShowEdgeContentProperty = DependencyProperty.RegisterAttached("ShowEdgeContent", typeof (bool), typeof (EdgeElement), new PropertyMetadata(ValueBoxes.FalseBox));

  public static void SetLeftContent(DependencyObject element, object value)
  {
    element.SetValue(EdgeElement.LeftContentProperty, value);
  }

  public static object GetLeftContent(DependencyObject element)
  {
    return element.GetValue(EdgeElement.LeftContentProperty);
  }

  public static void SetTopContent(DependencyObject element, object value)
  {
    element.SetValue(EdgeElement.TopContentProperty, value);
  }

  public static object GetTopContent(DependencyObject element)
  {
    return element.GetValue(EdgeElement.TopContentProperty);
  }

  public static void SetRightContent(DependencyObject element, object value)
  {
    element.SetValue(EdgeElement.RightContentProperty, value);
  }

  public static object GetRightContent(DependencyObject element)
  {
    return element.GetValue(EdgeElement.RightContentProperty);
  }

  public static void SetBottomContent(DependencyObject element, object value)
  {
    element.SetValue(EdgeElement.BottomContentProperty, value);
  }

  public static object GetBottomContent(DependencyObject element)
  {
    return element.GetValue(EdgeElement.BottomContentProperty);
  }

  public static void SetShowEdgeContent(DependencyObject element, bool value)
  {
    element.SetValue(EdgeElement.ShowEdgeContentProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowEdgeContent(DependencyObject element)
  {
    return (bool) element.GetValue(EdgeElement.ShowEdgeContentProperty);
  }

  private static void OnEdgeContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    EdgeElement.SetShowEdgeContent(d, EdgeElement.GetLeftContent(d) != null || EdgeElement.GetTopContent(d) != null || EdgeElement.GetRightContent(d) != null || EdgeElement.GetBottomContent(d) != null);
  }
}
