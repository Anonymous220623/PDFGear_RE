// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.WatermarkTextBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class WatermarkTextBox : TextBox
{
  public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof (Watermark), typeof (object), typeof (WatermarkTextBox), new PropertyMetadata((object) null));

  public object Watermark
  {
    get => this.GetValue(WatermarkTextBox.WatermarkProperty);
    set => this.SetValue(WatermarkTextBox.WatermarkProperty, value);
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    if (!this.IsEnabled || string.IsNullOrEmpty(this.Text))
      return;
    this.Select(0, this.Text.Length);
  }
}
