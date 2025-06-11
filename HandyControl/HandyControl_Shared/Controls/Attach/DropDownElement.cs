// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.DropDownElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class DropDownElement
{
  public static readonly DependencyProperty ConsistentWidthProperty = DependencyProperty.RegisterAttached("ConsistentWidth", typeof (bool), typeof (DropDownElement), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty AutoWidthProperty = DependencyProperty.RegisterAttached("AutoWidth", typeof (bool), typeof (DropDownElement), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

  public static void SetConsistentWidth(DependencyObject element, bool value)
  {
    element.SetValue(DropDownElement.ConsistentWidthProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetConsistentWidth(DependencyObject element)
  {
    return (bool) element.GetValue(DropDownElement.ConsistentWidthProperty);
  }

  public static void SetAutoWidth(DependencyObject element, bool value)
  {
    element.SetValue(DropDownElement.AutoWidthProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetAutoWidth(DependencyObject element)
  {
    return (bool) element.GetValue(DropDownElement.AutoWidthProperty);
  }
}
