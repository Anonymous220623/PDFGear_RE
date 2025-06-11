// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfPagePreviewGridViewItemDragCompletedEventArgs
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace pdfeditor.Controls;

public class PdfPagePreviewGridViewItemDragCompletedEventArgs
{
  public PdfPagePreviewGridViewItemDragCompletedEventArgs(
    object beforeItem,
    object afterItem,
    object[] dragItems,
    bool dragingContinuousRange,
    bool reordered)
  {
    this.BeforeItem = beforeItem;
    this.AfterItem = afterItem;
    this.DragItems = dragItems;
    this.DragingContinuousRange = dragingContinuousRange;
    this.Reordered = reordered;
  }

  public object BeforeItem { get; }

  public object AfterItem { get; }

  public object[] DragItems { get; }

  public bool DragingContinuousRange { get; }

  public bool Reordered { get; }
}
