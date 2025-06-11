// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorPickerButtonSelectedColorChangedEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public class ColorPickerButtonSelectedColorChangedEventArgs : RoutedEventArgs
{
  public ColorPickerButtonSelectedColorChangedEventArgs(object source, Color color)
    : base(ColorPickerButton.SelectedColorChangedEvent, source)
  {
    this.Color = color;
  }

  public Color Color { get; }
}
