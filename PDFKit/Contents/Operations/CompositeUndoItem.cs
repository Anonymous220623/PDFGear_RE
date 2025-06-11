// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Operations.CompositeUndoItem
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Contents.Operations;

internal class CompositeUndoItem : IPdfUndoItem, IDisposable
{
  private IReadOnlyList<IPdfUndoItem> undoItems;
  private bool disposedValue;

  internal CompositeUndoItem(IReadOnlyList<IPdfUndoItem> undoItems)
  {
    this.undoItems = undoItems != null ? (IReadOnlyList<IPdfUndoItem>) undoItems.ToArray<IPdfUndoItem>() : throw new ArgumentNullException(nameof (undoItems));
  }

  public int ParagraphId { get; internal set; }

  public int PageIndex { get; internal set; }

  public PdfTypeDictionary PageDict { get; internal set; }

  public int PrevCaret { get; internal set; } = -1;

  public int PrevEndCaret { get; internal set; } = -1;

  public int NextCaret { get; internal set; } = -1;

  public int NextEndCaret { get; internal set; } = -1;

  public void Redo(LogicalStructAnalyser analyser)
  {
    Common.WriteLog($"Composite Redo Page {this.PageIndex}");
    for (int index = 0; index < this.undoItems.Count; ++index)
    {
      IPdfUndoItem undoItem = this.undoItems[index];
      while (undoItem is CompositeUndoItem compositeUndoItem && compositeUndoItem.undoItems.Count == 1)
        undoItem = compositeUndoItem.undoItems[0];
      undoItem.Redo(analyser);
    }
    if (this.NextCaret == -1)
      return;
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId).SetCurrentCarets(this.NextCaret, this.NextEndCaret);
  }

  public void Undo(LogicalStructAnalyser analyser)
  {
    Common.WriteLog($"Composite Undo Page {this.PageIndex}");
    for (int index = this.undoItems.Count - 1; index >= 0; --index)
    {
      IPdfUndoItem undoItem = this.undoItems[index];
      while (undoItem is CompositeUndoItem compositeUndoItem && compositeUndoItem.undoItems.Count == 1)
        undoItem = compositeUndoItem.undoItems[0];
      undoItem.Undo(analyser);
    }
    if (this.PrevCaret == -1)
      return;
    ModifyParagraphUndoItem.GetParagraphFromAnalyser(analyser, this.PageDict, this.ParagraphId).SetCurrentCarets(this.PrevCaret, this.PrevEndCaret);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      foreach (IDisposable undoItem in (IEnumerable<IPdfUndoItem>) this.undoItems)
        undoItem.Dispose();
      this.undoItems = (IReadOnlyList<IPdfUndoItem>) null;
    }
    this.disposedValue = true;
  }

  void IDisposable.Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
