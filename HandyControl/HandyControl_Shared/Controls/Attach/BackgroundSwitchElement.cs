// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.BackgroundSwitchElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class BackgroundSwitchElement
{
  public static readonly DependencyProperty MouseHoverBackgroundProperty = DependencyProperty.RegisterAttached("MouseHoverBackground", typeof (Brush), typeof (BackgroundSwitchElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty MouseDownBackgroundProperty = DependencyProperty.RegisterAttached("MouseDownBackground", typeof (Brush), typeof (BackgroundSwitchElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent, FrameworkPropertyMetadataOptions.Inherits));

  public static void SetMouseHoverBackground(DependencyObject element, Brush value)
  {
    element.SetValue(BackgroundSwitchElement.MouseHoverBackgroundProperty, (object) value);
  }

  public static Brush GetMouseHoverBackground(DependencyObject element)
  {
    return (Brush) element.GetValue(BackgroundSwitchElement.MouseHoverBackgroundProperty);
  }

  public static void SetMouseDownBackground(DependencyObject element, Brush value)
  {
    element.SetValue(BackgroundSwitchElement.MouseDownBackgroundProperty, (object) value);
  }

  public static Brush GetMouseDownBackground(DependencyObject element)
  {
    return (Brush) element.GetValue(BackgroundSwitchElement.MouseDownBackgroundProperty);
  }
}
