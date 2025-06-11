// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TipElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class TipElement
{
  public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached("Visibility", typeof (Visibility), typeof (TipElement), new PropertyMetadata((object) Visibility.Collapsed));
  public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement", typeof (PlacementType), typeof (TipElement), new PropertyMetadata((object) PlacementType.LeftTop));
  public static readonly DependencyProperty StringFormatProperty = DependencyProperty.RegisterAttached("StringFormat", typeof (string), typeof (TipElement), new PropertyMetadata((object) "#0.0"));

  public static void SetVisibility(DependencyObject element, Visibility value)
  {
    element.SetValue(TipElement.VisibilityProperty, (object) value);
  }

  public static Visibility GetVisibility(DependencyObject element)
  {
    return (Visibility) element.GetValue(TipElement.VisibilityProperty);
  }

  public static void SetPlacement(DependencyObject element, PlacementType value)
  {
    element.SetValue(TipElement.PlacementProperty, (object) value);
  }

  public static PlacementType GetPlacement(DependencyObject element)
  {
    return (PlacementType) element.GetValue(TipElement.PlacementProperty);
  }

  public static void SetStringFormat(DependencyObject element, string value)
  {
    element.SetValue(TipElement.StringFormatProperty, (object) value);
  }

  public static string GetStringFormat(DependencyObject element)
  {
    return (string) element.GetValue(TipElement.StringFormatProperty);
  }
}
