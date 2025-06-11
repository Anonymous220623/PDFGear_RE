// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ProgressButton
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

public class ProgressButton : ToggleButton
{
  public static readonly DependencyProperty ProgressStyleProperty = DependencyProperty.Register(nameof (ProgressStyle), typeof (Style), typeof (ProgressButton), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof (Progress), typeof (double), typeof (ProgressButton), new PropertyMetadata(ValueBoxes.Double0Box));

  public Style ProgressStyle
  {
    get => (Style) this.GetValue(ProgressButton.ProgressStyleProperty);
    set => this.SetValue(ProgressButton.ProgressStyleProperty, (object) value);
  }

  public double Progress
  {
    get => (double) this.GetValue(ProgressButton.ProgressProperty);
    set => this.SetValue(ProgressButton.ProgressProperty, (object) value);
  }
}
