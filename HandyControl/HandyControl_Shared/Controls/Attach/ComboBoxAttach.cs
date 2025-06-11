// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ComboBoxAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class ComboBoxAttach
{
  public static readonly DependencyProperty IsMouseWheelEnabledProperty = DependencyProperty.RegisterAttached("IsMouseWheelEnabled", typeof (bool), typeof (ComboBoxAttach), new PropertyMetadata(ValueBoxes.TrueBox, new PropertyChangedCallback(ComboBoxAttach.OnIsMouseWheelEnabledChanged)));

  private static void OnIsMouseWheelEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is System.Windows.Controls.ComboBox comboBox1))
      return;
    if (!(bool) e.NewValue)
      comboBox1.PreviewMouseWheel += new MouseWheelEventHandler(OnComboBoxPreviewMouseWheel);
    else
      comboBox1.PreviewMouseWheel -= new MouseWheelEventHandler(OnComboBoxPreviewMouseWheel);

    static void OnComboBoxPreviewMouseWheel(object sender, MouseWheelEventArgs args)
    {
      if (sender is System.Windows.Controls.ComboBox comboBox2 && comboBox2.IsDropDownOpen)
        return;
      args.Handled = true;
    }
  }

  public static void SetIsMouseWheelEnabled(DependencyObject element, bool value)
  {
    element.SetValue(ComboBoxAttach.IsMouseWheelEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsMouseWheelEnabled(DependencyObject element)
  {
    return (bool) element.GetValue(ComboBoxAttach.IsMouseWheelEnabledProperty);
  }
}
