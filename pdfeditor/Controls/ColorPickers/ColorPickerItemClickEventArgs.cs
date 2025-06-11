// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorPickerItemClickEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public class ColorPickerItemClickEventArgs : RoutedEventArgs
{
  public ColorPickerItemClickEventArgs(object source, ColorValue item)
    : base(ColorPicker.ItemClickEvent, source)
  {
    this.Item = item ?? throw new ArgumentException(nameof (item));
  }

  public ColorValue Item { get; }
}
