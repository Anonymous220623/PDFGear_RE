// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SpeTextBlock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

internal class SpeTextBlock : TextBlock
{
  private DateTime _time;

  public double X { get; set; }

  public SpeTextBlock() => this.Width = 60.0;

  public SpeTextBlock(double x)
    : this()
  {
    this.X = x;
    Canvas.SetLeft((UIElement) this, this.X);
  }

  public DateTime Time
  {
    get => this._time;
    set
    {
      this._time = value;
      this.Text = value.ToString(this.TimeFormat) + "\r\n|";
    }
  }

  public string TimeFormat { get; set; } = "HH:mm";

  public void MoveX(double offsetX) => Canvas.SetLeft((UIElement) this, this.X + offsetX);
}
