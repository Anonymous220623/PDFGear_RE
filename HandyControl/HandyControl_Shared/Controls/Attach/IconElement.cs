// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.IconElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class IconElement
{
  public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached("Geometry", typeof (Geometry), typeof (IconElement), new PropertyMetadata((object) null));
  public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width", typeof (double), typeof (IconElement), new PropertyMetadata((object) double.NaN));
  public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached("Height", typeof (double), typeof (IconElement), new PropertyMetadata((object) double.NaN));

  public static void SetGeometry(DependencyObject element, Geometry value)
  {
    element.SetValue(IconElement.GeometryProperty, (object) value);
  }

  public static Geometry GetGeometry(DependencyObject element)
  {
    return (Geometry) element.GetValue(IconElement.GeometryProperty);
  }

  public static void SetWidth(DependencyObject element, double value)
  {
    element.SetValue(IconElement.WidthProperty, (object) value);
  }

  public static double GetWidth(DependencyObject element)
  {
    return (double) element.GetValue(IconElement.WidthProperty);
  }

  public static void SetHeight(DependencyObject element, double value)
  {
    element.SetValue(IconElement.HeightProperty, (object) value);
  }

  public static double GetHeight(DependencyObject element)
  {
    return (double) element.GetValue(IconElement.HeightProperty);
  }
}
