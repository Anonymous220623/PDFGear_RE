// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Operations.OperationManager
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Contents.Operations;

public class OperationManager
{
  private List<IPdfUndoItem> undoItems;
  private int index;
  private readonly PdfEditor editor;

  public OperationManager(PdfEditor editor)
  {
    this.undoItems = new List<IPdfUndoItem>();
    this.index = -1;
    this.editor = editor;
  }

  public bool CanRedo => this.index < this.undoItems.Count - 1;

  public bool CanUndo => this.index >= 0;

  public void AddUndoItem(IPdfUndoItem undoItem)
  {
    if (undoItem == null)
      return;
    lock (this)
    {
      if (this.index < this.undoItems.Count - 1)
      {
        for (int index = this.index + 1; index < this.undoItems.Count; ++index)
          this.undoItems[index].Dispose();
        this.undoItems.RemoveRange(this.index + 1, this.undoItems.Count - this.index - 1);
      }
      this.undoItems.Add(undoItem);
      this.index = this.undoItems.Count - 1;
    }
    EventHandler stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged((object) this, EventArgs.Empty);
  }

  internal void Undo()
  {
    if (this.editor == null)
      return;
    bool flag = false;
    lock (this)
    {
      if (this.CanUndo)
      {
        IPdfUndoItem undoItem = this.undoItems[this.index];
        LogicalStructAnalyser pageStructAnalyser = this.editor.GetPageStructAnalyser(undoItem.PageIndex);
        if (pageStructAnalyser != null)
        {
          --this.index;
          undoItem.Undo(pageStructAnalyser);
          flag = true;
        }
      }
    }
    if (!flag)
      return;
    EventHandler stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged((object) this, EventArgs.Empty);
  }

  internal void Redo()
  {
    if (this.editor == null)
      return;
    bool flag = false;
    lock (this)
    {
      if (this.CanRedo)
      {
        IPdfUndoItem undoItem = this.undoItems[this.index + 1];
        LogicalStructAnalyser pageStructAnalyser = this.editor.GetPageStructAnalyser(undoItem.PageIndex);
        if (pageStructAnalyser != null)
        {
          ++this.index;
          undoItem.Redo(pageStructAnalyser);
          flag = true;
        }
      }
    }
    if (!flag)
      return;
    EventHandler stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged((object) this, EventArgs.Empty);
  }

  internal bool ContainsPageIndex(int pageIndex)
  {
    return this.undoItems.Any<IPdfUndoItem>((Func<IPdfUndoItem, bool>) (item => item.PageIndex == pageIndex));
  }

  internal int[] GetUndoItemPageIndexes(bool containsRedoItem)
  {
    int num = containsRedoItem ? this.undoItems.Count - 1 : this.index;
    HashSet<int> source = new HashSet<int>();
    for (int index = 0; index <= num; ++index)
    {
      IPdfUndoItem undoItem = this.undoItems[index];
      source.Add(undoItem.PageIndex);
    }
    return source.OrderBy<int, int>((Func<int, int>) (c => c)).ToArray<int>();
  }

  public void Clear()
  {
    bool canUndo = this.CanUndo;
    lock (this)
    {
      foreach (IDisposable undoItem in this.undoItems)
        undoItem.Dispose();
      this.undoItems = new List<IPdfUndoItem>();
      this.index = -1;
    }
    if (!canUndo)
      return;
    EventHandler stateChanged = this.StateChanged;
    if (stateChanged != null)
      stateChanged((object) this, EventArgs.Empty);
  }

  public event EventHandler StateChanged;
}
