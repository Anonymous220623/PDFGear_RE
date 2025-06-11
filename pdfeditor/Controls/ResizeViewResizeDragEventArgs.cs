// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ResizeViewResizeDragEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;

#nullable disable
namespace pdfeditor.Controls;

public class ResizeViewResizeDragEventArgs
{
  public ResizeViewResizeDragEventArgs(
    Size oldSize,
    Size newSize,
    double offsetX,
    double offsetY,
    ResizeViewOperation operation)
  {
    this.OldSize = oldSize;
    this.NewSize = newSize;
    this.OffsetX = offsetX;
    this.OffsetY = offsetY;
    this.Operation = operation;
  }

  public Size OldSize { get; }

  public Size NewSize { get; }

  public double OffsetX { get; }

  public double OffsetY { get; }

  public ResizeViewOperation Operation { get; }
}
