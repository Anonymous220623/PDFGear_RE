// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TitleElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class TitleElement
{
  public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached("Title", typeof (string), typeof (TitleElement), new PropertyMetadata((object) null));
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof (Brush), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof (Brush), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.RegisterAttached("BorderBrush", typeof (Brush), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty TitlePlacementProperty = DependencyProperty.RegisterAttached("TitlePlacement", typeof (TitlePlacementType), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) TitlePlacementType.Top, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.RegisterAttached("TitleWidth", typeof (GridLength), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) GridLength.Auto, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalAlignment", typeof (HorizontalAlignment), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.RegisterAttached("VerticalAlignment", typeof (VerticalAlignment), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) VerticalAlignment.Top, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty MarginOnTheLeftProperty = DependencyProperty.RegisterAttached("MarginOnTheLeft", typeof (Thickness), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(), FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty MarginOnTheTopProperty = DependencyProperty.RegisterAttached("MarginOnTheTop", typeof (Thickness), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(), FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty PaddingProperty = DependencyProperty.RegisterAttached("Padding", typeof (Thickness), typeof (TitleElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(), FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty MinHeightProperty = DependencyProperty.RegisterAttached("MinHeight", typeof (double), typeof (TitleElement), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty MinWidthProperty = DependencyProperty.RegisterAttached("MinWidth", typeof (double), typeof (TitleElement), new PropertyMetadata(ValueBoxes.Double0Box));

  public static void SetTitle(DependencyObject element, string value)
  {
    element.SetValue(TitleElement.TitleProperty, (object) value);
  }

  public static string GetTitle(DependencyObject element)
  {
    return (string) element.GetValue(TitleElement.TitleProperty);
  }

  public static void SetBackground(DependencyObject element, Brush value)
  {
    element.SetValue(TitleElement.BackgroundProperty, (object) value);
  }

  public static Brush GetBackground(DependencyObject element)
  {
    return (Brush) element.GetValue(TitleElement.BackgroundProperty);
  }

  public static void SetForeground(DependencyObject element, Brush value)
  {
    element.SetValue(TitleElement.ForegroundProperty, (object) value);
  }

  public static Brush GetForeground(DependencyObject element)
  {
    return (Brush) element.GetValue(TitleElement.ForegroundProperty);
  }

  public static void SetBorderBrush(DependencyObject element, Brush value)
  {
    element.SetValue(TitleElement.BorderBrushProperty, (object) value);
  }

  public static Brush GetBorderBrush(DependencyObject element)
  {
    return (Brush) element.GetValue(TitleElement.BorderBrushProperty);
  }

  public static void SetTitlePlacement(DependencyObject element, TitlePlacementType value)
  {
    element.SetValue(TitleElement.TitlePlacementProperty, (object) value);
  }

  public static TitlePlacementType GetTitlePlacement(DependencyObject element)
  {
    return (TitlePlacementType) element.GetValue(TitleElement.TitlePlacementProperty);
  }

  public static void SetTitleWidth(DependencyObject element, GridLength value)
  {
    element.SetValue(TitleElement.TitleWidthProperty, (object) value);
  }

  public static GridLength GetTitleWidth(DependencyObject element)
  {
    return (GridLength) element.GetValue(TitleElement.TitleWidthProperty);
  }

  public static void SetHorizontalAlignment(DependencyObject element, HorizontalAlignment value)
  {
    element.SetValue(TitleElement.HorizontalAlignmentProperty, (object) value);
  }

  public static HorizontalAlignment GetHorizontalAlignment(DependencyObject element)
  {
    return (HorizontalAlignment) element.GetValue(TitleElement.HorizontalAlignmentProperty);
  }

  public static void SetVerticalAlignment(DependencyObject element, VerticalAlignment value)
  {
    element.SetValue(TitleElement.VerticalAlignmentProperty, (object) value);
  }

  public static VerticalAlignment GetVerticalAlignment(DependencyObject element)
  {
    return (VerticalAlignment) element.GetValue(TitleElement.VerticalAlignmentProperty);
  }

  public static void SetMarginOnTheLeft(DependencyObject element, Thickness value)
  {
    element.SetValue(TitleElement.MarginOnTheLeftProperty, (object) value);
  }

  public static Thickness GetMarginOnTheLeft(DependencyObject element)
  {
    return (Thickness) element.GetValue(TitleElement.MarginOnTheLeftProperty);
  }

  public static void SetMarginOnTheTop(DependencyObject element, Thickness value)
  {
    element.SetValue(TitleElement.MarginOnTheTopProperty, (object) value);
  }

  public static Thickness GetMarginOnTheTop(DependencyObject element)
  {
    return (Thickness) element.GetValue(TitleElement.MarginOnTheTopProperty);
  }

  public static void SetPadding(DependencyObject element, Thickness value)
  {
    element.SetValue(TitleElement.PaddingProperty, (object) value);
  }

  public static Thickness GetPadding(DependencyObject element)
  {
    return (Thickness) element.GetValue(TitleElement.PaddingProperty);
  }

  public static double GetMinHeight(DependencyObject obj)
  {
    return (double) obj.GetValue(TitleElement.MinHeightProperty);
  }

  public static void SetMinHeight(DependencyObject obj, double value)
  {
    obj.SetValue(TitleElement.MinHeightProperty, (object) value);
  }

  public static double GetMinWidth(DependencyObject obj)
  {
    return (double) obj.GetValue(TitleElement.MinWidthProperty);
  }

  public static void SetMinWidth(DependencyObject obj, double value)
  {
    obj.SetValue(TitleElement.MinWidthProperty, (object) value);
  }
}
