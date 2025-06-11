// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Speech.FormattedSlider
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Speech;

public class FormattedSlider : Slider
{
  private System.Windows.Controls.ToolTip _autoToolTip;
  private string _autoToolTipFormat;

  public string AutoToolTipFormat
  {
    get => this._autoToolTipFormat;
    set => this._autoToolTipFormat = value;
  }

  protected override void OnThumbDragStarted(DragStartedEventArgs e)
  {
    base.OnThumbDragStarted(e);
    this.FormatAutoToolTipContent();
  }

  protected override void OnThumbDragDelta(DragDeltaEventArgs e)
  {
    base.OnThumbDragDelta(e);
    this.FormatAutoToolTipContent();
  }

  private void FormatAutoToolTipContent()
  {
    if (string.IsNullOrEmpty(this.AutoToolTipFormat))
      return;
    this.AutoToolTip.Content = (object) $"{Convert.ToInt32(this.AutoToolTip.Content.ToString()) - 5}";
  }

  private System.Windows.Controls.ToolTip AutoToolTip
  {
    get
    {
      if (this._autoToolTip == null)
      {
        this._autoToolTip = typeof (Slider).GetField("_autoToolTip", BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) this) as System.Windows.Controls.ToolTip;
        this._autoToolTip.BorderThickness = new Thickness(0.0);
        this._autoToolTip.Background = (Brush) new SolidColorBrush(Colors.Transparent);
        this._autoToolTip.Margin = new Thickness(-3.5, 0.0, 0.0, 0.0);
      }
      return this._autoToolTip;
    }
  }
}
