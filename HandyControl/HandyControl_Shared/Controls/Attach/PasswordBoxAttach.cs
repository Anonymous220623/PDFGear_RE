// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PasswordBoxAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class PasswordBoxAttach
{
  public static readonly DependencyProperty PasswordLengthProperty = DependencyProperty.RegisterAttached("PasswordLength", typeof (int), typeof (PasswordBoxAttach), new PropertyMetadata(ValueBoxes.Int0Box));
  public static readonly DependencyProperty IsMonitoringProperty = DependencyProperty.RegisterAttached("IsMonitoring", typeof (bool), typeof (PasswordBoxAttach), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(PasswordBoxAttach.OnIsMonitoringChanged)));

  public static void SetPasswordLength(DependencyObject element, int value)
  {
    element.SetValue(PasswordBoxAttach.PasswordLengthProperty, (object) value);
  }

  public static int GetPasswordLength(DependencyObject element)
  {
    return (int) element.GetValue(PasswordBoxAttach.PasswordLengthProperty);
  }

  private static void OnIsMonitoringChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is System.Windows.Controls.PasswordBox passwordBox) || !(e.NewValue is bool newValue))
      return;
    if (newValue)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      passwordBox.PasswordChanged += PasswordBoxAttach.\u003C\u003EO.\u003C0\u003E__PasswordChanged ?? (PasswordBoxAttach.\u003C\u003EO.\u003C0\u003E__PasswordChanged = new RoutedEventHandler(PasswordBoxAttach.PasswordChanged));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      passwordBox.PasswordChanged -= PasswordBoxAttach.\u003C\u003EO.\u003C0\u003E__PasswordChanged ?? (PasswordBoxAttach.\u003C\u003EO.\u003C0\u003E__PasswordChanged = new RoutedEventHandler(PasswordBoxAttach.PasswordChanged));
    }
  }

  public static void SetIsMonitoring(DependencyObject element, bool value)
  {
    element.SetValue(PasswordBoxAttach.IsMonitoringProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsMonitoring(DependencyObject element)
  {
    return (bool) element.GetValue(PasswordBoxAttach.IsMonitoringProperty);
  }

  private static void PasswordChanged(object sender, RoutedEventArgs e)
  {
    if (!(sender is System.Windows.Controls.PasswordBox element))
      return;
    PasswordBoxAttach.SetPasswordLength((DependencyObject) element, element.Password.Length);
  }
}
