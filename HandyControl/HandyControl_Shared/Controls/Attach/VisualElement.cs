// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.VisualElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class VisualElement
{
  public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.RegisterAttached("HighlightBrush", typeof (Brush), typeof (VisualElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty HighlightBackgroundProperty = DependencyProperty.RegisterAttached("HighlightBackground", typeof (Brush), typeof (VisualElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty HighlightBorderBrushProperty = DependencyProperty.RegisterAttached("HighlightBorderBrush", typeof (Brush), typeof (VisualElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty HighlightForegroundProperty = DependencyProperty.RegisterAttached("HighlightForeground", typeof (Brush), typeof (VisualElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof (string), typeof (VisualElement), new PropertyMetadata((object) null));

  public static void SetHighlightBrush(DependencyObject element, Brush value)
  {
    element.SetValue(VisualElement.HighlightBrushProperty, (object) value);
  }

  public static Brush GetHighlightBrush(DependencyObject element)
  {
    return (Brush) element.GetValue(VisualElement.HighlightBrushProperty);
  }

  public static void SetHighlightBackground(DependencyObject element, Brush value)
  {
    element.SetValue(VisualElement.HighlightBackgroundProperty, (object) value);
  }

  public static Brush GetHighlightBackground(DependencyObject element)
  {
    return (Brush) element.GetValue(VisualElement.HighlightBackgroundProperty);
  }

  public static void SetHighlightBorderBrush(DependencyObject element, Brush value)
  {
    element.SetValue(VisualElement.HighlightBorderBrushProperty, (object) value);
  }

  public static Brush GetHighlightBorderBrush(DependencyObject element)
  {
    return (Brush) element.GetValue(VisualElement.HighlightBorderBrushProperty);
  }

  public static void SetHighlightForeground(DependencyObject element, Brush value)
  {
    element.SetValue(VisualElement.HighlightForegroundProperty, (object) value);
  }

  public static Brush GetHighlightForeground(DependencyObject element)
  {
    return (Brush) element.GetValue(VisualElement.HighlightForegroundProperty);
  }

  public static void SetText(DependencyObject element, string value)
  {
    element.SetValue(VisualElement.TextProperty, (object) value);
  }

  public static string GetText(DependencyObject element)
  {
    return (string) element.GetValue(VisualElement.TextProperty);
  }
}
