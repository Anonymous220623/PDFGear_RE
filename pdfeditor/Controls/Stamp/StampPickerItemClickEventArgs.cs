// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Stamp.StampPickerItemClickEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls.Annotations;
using System;
using System.Windows;

#nullable disable
namespace pdfeditor.Controls.Stamp;

public class StampPickerItemClickEventArgs : RoutedEventArgs
{
  public StampPickerItemClickEventArgs(object source, ImageStampModel item)
    : base(StampPicker.ItemClickEvent, source)
  {
    this.Item = item ?? throw new ArgumentException(nameof (item));
  }

  public ImageStampModel Item { get; }
}
