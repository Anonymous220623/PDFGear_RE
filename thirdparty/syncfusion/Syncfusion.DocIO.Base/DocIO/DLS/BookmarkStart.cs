// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BookmarkStart
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BookmarkStart : ParagraphItem, ILeafWidget, IWidget
{
  private string m_strName = string.Empty;
  private short m_colFirst = -1;
  private short m_colLast = -1;
  private string m_displacedByCustomXml;
  private byte m_bFlags;

  public override EntityType EntityType => EntityType.BookmarkStart;

  public string Name => this.m_strName;

  internal bool IsCellGroupBkmk
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsDetached
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal short ColumnFirst
  {
    get => this.m_colFirst;
    set => this.m_colFirst = value;
  }

  internal short ColumnLast
  {
    get => this.m_colLast;
    set => this.m_colLast = value;
  }

  internal string DisplacedByCustomXml
  {
    get => this.m_displacedByCustomXml;
    set => this.m_displacedByCustomXml = value;
  }

  internal BookmarkStart(WordDocument doc)
    : this((IWordDocument) doc, string.Empty)
  {
  }

  public BookmarkStart(IWordDocument doc, string name)
    : base((WordDocument) doc)
  {
    this.SetName(name);
  }

  internal void SetName(string name) => this.m_strName = name.Replace('-', '_');

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
    if (!this.DeepDetached)
    {
      this.Document.Bookmarks.AttachBookmarkStart(this);
      this.IsDetached = false;
    }
    else
      this.IsDetached = true;
  }

  internal override void Detach()
  {
    base.Detach();
    if (this.DeepDetached)
      return;
    BookmarkCollection bookmarks = this.Document.Bookmarks;
    Bookmark byName = bookmarks.FindByName(this.Name);
    if (byName == null)
      return;
    byName.SetStart((BookmarkStart) null);
    bookmarks.Remove(byName);
  }

  internal override void AttachToDocument()
  {
    if (!this.IsDetached)
      return;
    this.Document.Bookmarks.AttachBookmarkStart(this);
    this.IsDetached = false;
  }

  protected override object CloneImpl()
  {
    BookmarkStart bookmarkStart = (BookmarkStart) base.CloneImpl();
    bookmarkStart.IsDetached = true;
    return (object) bookmarkStart;
  }

  internal override void Close()
  {
    if (!string.IsNullOrEmpty(this.m_strName))
      this.m_strName = string.Empty;
    base.Close();
  }

  internal void GetBkmkContentInDiffCell(
    WTable bkmkTable,
    int startTableRowIndex,
    int endTableRowIndex,
    int startCellIndex,
    int endCellIndex,
    WTextBody textBody)
  {
    WTable wtable = bkmkTable.Clone();
    while (startTableRowIndex > 0)
    {
      wtable.Rows[0].RemoveSelf();
      --startTableRowIndex;
      --endTableRowIndex;
    }
    while (wtable.Rows.Count > endTableRowIndex + 1)
      wtable.Rows[endTableRowIndex + 1].RemoveSelf();
    int num = startCellIndex;
    int maximumCellCount = this.GetMaximumCellCount(wtable, endCellIndex);
    for (int index = startTableRowIndex; index <= endTableRowIndex; ++index)
    {
      startCellIndex = num;
      endCellIndex = maximumCellCount < wtable.Rows[index].Cells.Count ? maximumCellCount : wtable.Rows[index].Cells.Count - 1;
      this.GetCellRangeForMergedCells(wtable.Rows[index].Cells[startCellIndex], wtable.Rows[index].Cells[endCellIndex], wtable, wtable, index, index, ref startCellIndex, ref endCellIndex);
      while (startCellIndex > 0)
      {
        wtable.Rows[index].Cells[0].RemoveSelf();
        --startCellIndex;
        --endCellIndex;
      }
      while (wtable.Rows[index].Cells.Count > endCellIndex + 1)
        wtable.Rows[index].Cells[endCellIndex + 1].RemoveSelf();
    }
    WTableCell cell = wtable.Rows[startTableRowIndex].Cells[startCellIndex];
    WTableCell lastItem = wtable.Rows[endTableRowIndex].Cells.LastItem as WTableCell;
    if (cell == lastItem)
    {
      foreach (TextBodyItem textBodyItem in (CollectionImpl) this.RemoveBkmkStartEndFromCell(cell).Items)
        textBody.ChildEntities.AddToInnerList((Entity) textBodyItem);
    }
    else
    {
      for (int index1 = startTableRowIndex; index1 <= endTableRowIndex; ++index1)
      {
        for (int index2 = startCellIndex; index2 < wtable.Rows[index1].Cells.Count && index2 <= endCellIndex; ++index2)
          this.RemoveBkmkStartEndFromCell(wtable.Rows[index1].Cells[index2]);
      }
      textBody.Items.AddToInnerList((Entity) wtable);
    }
  }

  private int GetMaximumCellCount(WTable Table, int bkmkEndCellIndex)
  {
    if (this.ColumnFirst == (short) -1 && this.ColumnLast == (short) -1)
    {
      for (int index = 0; index < Table.Rows.Count; ++index)
      {
        WTableRow row = Table.Rows[index];
        if (bkmkEndCellIndex < row.Cells.Count - 1)
          bkmkEndCellIndex = row.Cells.Count - 1;
      }
    }
    return bkmkEndCellIndex;
  }

  private WTableCell RemoveBkmkStartEndFromCell(WTableCell tableCell)
  {
    foreach (WParagraph paragraph in (IEnumerable) tableCell.Paragraphs)
    {
      for (int index = paragraph.Items.Count - 1; index >= 0; --index)
      {
        if (paragraph.Items[index] is BookmarkStart && (paragraph.Items[index] as BookmarkStart).Name == this.Name)
          paragraph.Items.RemoveFromInnerList(index);
        else if (paragraph.Items[index] is BookmarkEnd && (paragraph.Items[index] as BookmarkEnd).Name == this.Name)
          paragraph.Items.RemoveAt(index);
      }
    }
    return tableCell;
  }

  internal void GetBookmarkStartAndEndCell(
    WTableCell bkmkStartCell,
    WTableCell bkmkEndCell,
    WTableCell tempBkmkEndCell,
    WTable bkmkStartTable,
    WTable bkmkEndTable,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd,
    int startTableRowIndex,
    ref int endTableRowIndex,
    ref int bkmkStartCellIndex,
    ref int bkmkEndCellIndex)
  {
    int columnFirst = (int) bkmkStart.ColumnFirst;
    int columnLast = (int) bkmkStart.ColumnLast;
    bool flag1 = columnFirst >= 0 && columnFirst < bkmkStartTable.Rows[startTableRowIndex].Cells.Count;
    bool flag2 = columnLast >= 0 && columnLast < bkmkEndTable.Rows[endTableRowIndex].Cells.Count;
    bool flag3 = columnFirst > columnLast;
    bool flag4 = columnFirst < 0 || columnLast < 0;
    if (flag1 && flag2 && !flag3)
    {
      bkmkStartCellIndex = columnFirst;
      bkmkEndCellIndex = columnLast;
    }
    else if (flag1 && !flag2 && !flag3)
    {
      bkmkStartCellIndex = (int) bkmkStart.ColumnFirst;
      bkmkEndCellIndex = bkmkEndTable.Rows[endTableRowIndex].Cells.Count - 1;
    }
    else if (flag3 || flag4 || !flag1 || !flag2)
    {
      bkmkStartCellIndex = 0;
      bkmkEndCellIndex = bkmkEndTable.Rows[endTableRowIndex].Cells.Count - 1;
    }
    bkmkStartCell = bkmkStartTable.Rows[startTableRowIndex].Cells[bkmkStartCellIndex];
    bkmkEndCell = bkmkEndTable.Rows[endTableRowIndex].Cells[bkmkEndCellIndex];
    this.GetCellRangeForMergedCells(bkmkStartCell, bkmkEndCell, bkmkStartTable, bkmkEndTable, startTableRowIndex, endTableRowIndex, ref bkmkStartCellIndex, ref bkmkEndCellIndex);
  }

  private void GetCellRangeForMergedCells(
    WTableCell bkmkStartCell,
    WTableCell bkmkEndCell,
    WTable bkmkStartTable,
    WTable bkmkEndTable,
    int startTableRowIndex,
    int endTableRowIndex,
    ref int bkmkStartCellIndex,
    ref int bkmkEndCellIndex)
  {
    if (bkmkStartCell.CellFormat.HorizontalMerge == CellMerge.Continue)
    {
      for (; bkmkStartCell.CellFormat.HorizontalMerge == CellMerge.Continue; bkmkStartCell = bkmkStartTable.Rows[startTableRowIndex].Cells[bkmkStartCellIndex])
        --bkmkStartCellIndex;
    }
    if (bkmkEndCell.CellFormat.HorizontalMerge != CellMerge.Continue)
      return;
    for (; bkmkEndCell.CellFormat.HorizontalMerge == CellMerge.Continue; bkmkEndCell = bkmkEndTable.Rows[endTableRowIndex].Cells[bkmkEndCellIndex])
    {
      ++bkmkEndCellIndex;
      if (bkmkEndCellIndex >= bkmkEndTable.Rows[endTableRowIndex].Cells.Count)
        break;
    }
    --bkmkEndCellIndex;
    bkmkEndCell = bkmkEndTable.Rows[endTableRowIndex].Cells[bkmkEndCellIndex];
  }

  internal bool IsBookmarkEndAtSameCell(
    WTableCell bkmkStartCell,
    WTable bkmkStartTable,
    ref WTable bkmkEndTable,
    ref int bkmkEndRowIndex)
  {
    int num = bkmkEndRowIndex;
    WTable wtable = bkmkEndTable;
    for (owner = wtable.Owner as WTableCell; wtable.IsInCell && wtable.Owner is WTableCell owner && owner.Owner.Owner as WTable != bkmkStartTable; wtable = owner.Owner.Owner as WTable)
      num = owner.OwnerRow.Index;
    bool flag = bkmkEndTable != bkmkStartTable && bkmkStartCell == owner;
    if (flag)
    {
      bkmkEndTable = wtable;
      bkmkEndRowIndex = num;
    }
    return flag;
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.BookmarkStart);
    writer.WriteValue("BookmarkName", this.Name);
    if (!this.IsCellGroupBkmk)
      return;
    writer.WriteValue("IsCellGroupBkmk", this.IsCellGroupBkmk);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    this.m_strName = reader.ReadString("BookmarkName");
    this.Document.Bookmarks.AttachBookmarkStart(this);
    if (!reader.HasAttribute("IsCellGroupBkmk"))
      return;
    this.IsCellGroupBkmk = reader.ReadBoolean("IsCellGroupBkmk");
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    this.m_layoutInfo.IsSkipBottomAlign = true;
    this.m_layoutInfo.IsClipped = ((IWidget) this.GetOwnerParagraphValue()).LayoutInfo.IsClipped;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    SizeF sizeF = new SizeF();
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    if (ownerParagraphValue.IsNeedToMeasureBookMarkSize)
      sizeF.Height = ((IWidget) ownerParagraphValue).LayoutInfo.Size.Height;
    return sizeF;
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
