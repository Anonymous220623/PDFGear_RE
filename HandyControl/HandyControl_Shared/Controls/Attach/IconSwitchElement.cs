// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.IconSwitchElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class IconSwitchElement : IconElement
{
  public static readonly DependencyProperty GeometrySelectedProperty = DependencyProperty.RegisterAttached("GeometrySelected", typeof (Geometry), typeof (IconSwitchElement), new PropertyMetadata((object) null));

  public static void SetGeometrySelected(DependencyObject element, Geometry value)
  {
    element.SetValue(IconSwitchElement.GeometrySelectedProperty, (object) value);
  }

  public static Geometry GetGeometrySelected(DependencyObject element)
  {
    return (Geometry) element.GetValue(IconSwitchElement.GeometrySelectedProperty);
  }
}
