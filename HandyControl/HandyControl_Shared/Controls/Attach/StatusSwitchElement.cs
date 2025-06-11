// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.StatusSwitchElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class StatusSwitchElement
{
  public static readonly DependencyProperty CheckedElementProperty = DependencyProperty.RegisterAttached("CheckedElement", typeof (object), typeof (StatusSwitchElement), new PropertyMetadata((object) null));
  public static readonly DependencyProperty HideUncheckedElementProperty = DependencyProperty.RegisterAttached("HideUncheckedElement", typeof (bool), typeof (StatusSwitchElement), new PropertyMetadata(ValueBoxes.FalseBox));

  public static void SetCheckedElement(DependencyObject element, object value)
  {
    element.SetValue(StatusSwitchElement.CheckedElementProperty, value);
  }

  public static object GetCheckedElement(DependencyObject element)
  {
    return element.GetValue(StatusSwitchElement.CheckedElementProperty);
  }

  public static void SetHideUncheckedElement(DependencyObject element, bool value)
  {
    element.SetValue(StatusSwitchElement.HideUncheckedElementProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetHideUncheckedElement(DependencyObject element)
  {
    return (bool) element.GetValue(StatusSwitchElement.HideUncheckedElementProperty);
  }
}
