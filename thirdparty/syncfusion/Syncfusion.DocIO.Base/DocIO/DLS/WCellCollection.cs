// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WCellCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WCellCollection(WTableRow owner) : EntityCollection(owner.Document, (Entity) owner)
{
  private static readonly Type[] DEF_ELEMENT_TYPES = new Type[1]
  {
    typeof (WTableCell)
  };

  public WTableCell this[int index] => this.InnerList[index] as WTableCell;

  protected override Type[] TypesOfElement => WCellCollection.DEF_ELEMENT_TYPES;

  public int Add(WTableCell cell)
  {
    if (this.Count > 62)
      throw new ArgumentException("This exceeds the maximum number of cells.");
    int index = this.Add((IEntity) cell);
    this.OnInsertCell(index, cell.CellFormat);
    return index;
  }

  protected override void OnClear()
  {
    while (this.Count >= 1)
      this.Remove(this[this.Count - 1]);
  }

  public void Insert(int index, WTableCell cell)
  {
    this.Insert(index, (IEntity) cell);
    this.OnInsertCell(index, cell.CellFormat);
  }

  public int IndexOf(WTableCell cell) => this.IndexOf((IEntity) cell);

  public void Remove(WTableCell cell)
  {
    int cellIndex = cell.GetCellIndex();
    this.RemoveCellBookmark(cellIndex);
    this.Remove((IEntity) cell);
    this.OnRemoveCell(cellIndex);
  }

  public new void RemoveAt(int index)
  {
    this.RemoveCellBookmark(index);
    base.RemoveAt(index);
    this.OnRemoveCell(index);
  }

  private void RemoveCellBookmark(int index)
  {
    WTableCell cell = this[index];
    this.MoveBookmarkStart(cell);
    this.MoveBookmarkEnd(cell);
  }

  private void MoveBookmarkStart(WTableCell cell)
  {
    if (cell == null)
      return;
    Stack<BookmarkStart> bookmarkStartStack = new Stack<BookmarkStart>();
    WordDocument document = cell.Document;
    if (cell.Index != 0 || cell.ChildEntities.Count <= 0)
      return;
    TextBodyItem childEntity1 = cell.ChildEntities[0] as TextBodyItem;
    if (!(childEntity1 is WParagraph))
      return;
    foreach (ParagraphItem childEntity2 in (CollectionImpl) (childEntity1 as WParagraph).ChildEntities)
    {
      if (childEntity2 is BookmarkStart && (childEntity2 as BookmarkStart).Name != "_GoBack" && childEntity2 is BookmarkStart bookmarkStart && bookmarkStart.ColumnFirst != (short) -1 && bookmarkStart.ColumnLast != (short) -1)
      {
        BookmarkEnd bookmarkEnd = document.Bookmarks.FindByName(bookmarkStart.Name).BookmarkEnd;
        if ((WTable) bookmarkStart.OwnerParagraph.OwnerTextBody.Owner.Owner == (WTable) bookmarkEnd.OwnerParagraph.OwnerTextBody.Owner.Owner)
          bookmarkStartStack.Push(bookmarkStart);
      }
    }
    if (bookmarkStartStack.Count <= 0)
      return;
    WTableCell wtableCell = cell.NextSibling != null ? cell.NextSibling as WTableCell : (cell.OwnerRow.NextSibling != null ? (cell.OwnerRow.NextSibling as WTableRow).Cells[0] : (WTableCell) null);
    if (wtableCell == null || wtableCell.ChildEntities.Count <= 0)
      return;
    TextBodyItem childEntity3 = wtableCell.ChildEntities[0] as TextBodyItem;
    switch (childEntity3)
    {
      case WParagraph _:
        (childEntity3 as WParagraph).ChildEntities.Insert(0, (IEntity) bookmarkStartStack.Pop());
        break;
      case WTable _:
        WParagraph wparagraph = (WParagraph) null;
        if ((childEntity3 as WTable).Rows[0].Cells[0].ChildEntities.Count > 0)
          wparagraph = (childEntity3 as WTable).Rows[0].Cells[0].ChildEntities[0] as WParagraph;
        wparagraph?.ChildEntities.Insert(0, (IEntity) bookmarkStartStack.Pop());
        break;
    }
  }

  private void MoveBookmarkEnd(WTableCell cell)
  {
    if (cell == null)
      return;
    Queue<BookmarkEnd> bookmarkEndQueue = new Queue<BookmarkEnd>();
    WordDocument document = cell.Document;
    if (cell.Index != cell.OwnerRow.Cells.Count - 1 || cell.ChildEntities.Count <= 0)
      return;
    int index1 = cell.ChildEntities.Count - 1;
    TextBodyItem childEntity1 = cell.ChildEntities[index1] as TextBodyItem;
    if (!(childEntity1 is WParagraph))
      return;
    foreach (ParagraphItem childEntity2 in (CollectionImpl) (childEntity1 as WParagraph).ChildEntities)
    {
      if (childEntity2 is BookmarkEnd && (childEntity2 as BookmarkEnd).Name != "_GoBack" && childEntity2 is BookmarkEnd bookmarkEnd)
      {
        BookmarkStart bookmarkStart = document.Bookmarks.FindByName(bookmarkEnd.Name).BookmarkStart;
        WTable owner1 = (WTable) bookmarkStart.OwnerParagraph.OwnerTextBody.Owner.Owner;
        WTable owner2 = (WTable) bookmarkEnd.OwnerParagraph.OwnerTextBody.Owner.Owner;
        if (bookmarkStart != null && bookmarkStart.ColumnFirst != (short) -1 && bookmarkStart.ColumnLast != (short) -1 && owner1 == owner2)
          bookmarkEndQueue.Enqueue(bookmarkEnd);
      }
    }
    if (bookmarkEndQueue.Count <= 0)
      return;
    WTableCell wtableCell = cell.PreviousSibling != null ? cell.PreviousSibling as WTableCell : (cell.OwnerRow.PreviousSibling != null ? (cell.OwnerRow.PreviousSibling as WTableRow).Cells[(cell.OwnerRow.PreviousSibling as WTableRow).Cells.Count - 1] : (WTableCell) null);
    if (wtableCell == null || wtableCell.ChildEntities.Count <= 0)
      return;
    int index2 = wtableCell.ChildEntities.Count - 1;
    TextBodyItem childEntity3 = wtableCell.ChildEntities[index2] as TextBodyItem;
    switch (childEntity3)
    {
      case WParagraph _:
        (childEntity3 as WParagraph).ChildEntities.Add((IEntity) bookmarkEndQueue.Dequeue());
        break;
      case WTable _:
        WParagraph wparagraph = (WParagraph) null;
        if ((childEntity3 as WTable).Rows[0].Cells[0].ChildEntities.Count > 0)
          wparagraph = (childEntity3 as WTable).Rows[0].Cells[0].LastParagraph as WParagraph;
        if (wparagraph == null)
          break;
        wparagraph.ChildEntities.Add((IEntity) bookmarkEndQueue.Dequeue());
        break;
    }
  }

  protected override string GetTagItemName() => "cell";

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new WTableCell((IWordDocument) this.Document);
  }

  internal new void CloneTo(EntityCollection destColl)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      destColl.Add((IEntity) this[index].CloneCell());
  }

  private void OnInsertCell(int index, CellFormat cellFormat)
  {
    if (this.Owner == null || !(this.Owner is WTableRow) || this.Owner.Document.IsOpening)
      return;
    (this.Owner as WTableRow).OnInsertCell(index, cellFormat);
  }

  private void OnRemoveCell(int index)
  {
    if (this.Owner == null || !(this.Owner is WTableRow) || this.Owner.Document.IsOpening)
      return;
    (this.Owner as WTableRow).OnRemoveCell(index);
  }
}
