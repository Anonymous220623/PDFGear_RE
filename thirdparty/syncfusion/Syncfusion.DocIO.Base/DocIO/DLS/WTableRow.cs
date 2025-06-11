// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTableRow
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTableRow : WidgetBase, ICompositeEntity, IEntity, IWidget
{
  private WCellCollection m_cells;
  private RowFormat m_tableFormat;
  private WCharacterFormat m_charFormat;
  private TableRowHeightType m_heightType;
  private byte[] m_internalData;
  internal RowFormat m_trackRowFormat;
  private byte m_bFlags;
  private RowContentControl m_RowContentControl;

  public EntityCollection ChildEntities => (EntityCollection) this.m_cells;

  public override EntityType EntityType => EntityType.TableRow;

  public WCellCollection Cells
  {
    get => this.m_cells;
    set => this.m_cells = value;
  }

  public TableRowHeightType HeightType
  {
    get => this.m_heightType;
    set => this.m_heightType = value;
  }

  public RowFormat RowFormat => this.m_tableFormat;

  public float Height
  {
    get => this.m_tableFormat.Height;
    set => this.m_tableFormat.Height = value;
  }

  public bool IsHeader
  {
    get => this.m_tableFormat.IsHeaderRow;
    set => this.m_tableFormat.IsHeaderRow = value;
  }

  internal WTable OwnerTable => this.Owner as WTable;

  internal byte[] DataArray
  {
    get => this.m_internalData;
    set => this.m_internalData = value;
  }

  internal WCharacterFormat CharacterFormat => this.m_charFormat;

  internal RowFormat TrackRowFormat
  {
    get
    {
      if (this.m_trackRowFormat == null)
        this.m_trackRowFormat = new RowFormat();
      return this.m_trackRowFormat;
    }
  }

  internal bool IsDeleteRevision
  {
    get => ((int) this.m_bFlags & 1) != 0 || this.CharacterFormat.IsDeleteRevision;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
      this.CharacterFormat.IsDeleteRevision = value;
    }
  }

  internal bool IsInsertRevision
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0 || this.CharacterFormat.IsInsertRevision;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
      this.CharacterFormat.IsInsertRevision = value;
    }
  }

  internal bool HasTblPrEx
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal RowContentControl ContentControl
  {
    get => this.m_RowContentControl;
    set => this.m_RowContentControl = value;
  }

  public WTableRow(IWordDocument document)
    : base((WordDocument) document, (Entity) null)
  {
    this.m_cells = new WCellCollection(this);
    this.m_charFormat = new WCharacterFormat((IWordDocument) this.Document);
    this.m_charFormat.SetOwner((OwnerHolder) this);
    this.m_tableFormat = new RowFormat();
    this.m_tableFormat.SetOwner((OwnerHolder) this);
  }

  public WTableRow Clone() => (WTableRow) this.CloneImpl();

  public WTableCell AddCell() => this.AddCell(true);

  public WTableCell AddCell(bool isCopyFormat)
  {
    if (this.Cells.Count > 62)
      throw new ArgumentException("This exceeds the maximum number of cells.");
    WTableCell cell = new WTableCell((IWordDocument) this.Document);
    WTableRow previousSibling = this.PreviousSibling as WTableRow;
    WTableCell wtableCell = (WTableCell) null;
    if (previousSibling != null && previousSibling.Cells.Count > this.Cells.Count)
      wtableCell = previousSibling.Cells[this.Cells.Count];
    if (isCopyFormat && wtableCell != null)
    {
      cell.CellFormat.ImportContainer((FormatBase) wtableCell.CellFormat);
      cell.Width = wtableCell.Width;
    }
    else if (isCopyFormat && wtableCell == null)
      cell.CellFormat.ImportContainer((FormatBase) this.m_tableFormat);
    this.Cells.Add(cell);
    return cell;
  }

  public int GetRowIndex() => this.GetIndexInOwnerCollection();

  internal bool IsCellWidthZero()
  {
    foreach (WTableCell cell in (CollectionImpl) this.Cells)
    {
      if (!cell.IsCellWidthZero())
        return false;
    }
    return (this.RowFormat.GridBeforeWidth.WidthType < FtsWidth.Percentage || (double) this.RowFormat.GridBeforeWidth.Width == 0.0) && (double) this.RowFormat.BeforeWidth == 0.0 && (this.RowFormat.GridAfterWidth.WidthType < FtsWidth.Percentage || (double) this.RowFormat.GridAfterWidth.Width == 0.0) && (double) this.RowFormat.AfterWidth == 0.0;
  }

  internal void UpdateCellWidthByPartitioning(float tableWidth, bool isSkipToEqualPartition)
  {
    if (isSkipToEqualPartition)
    {
      float num1 = tableWidth;
      List<int> intList = new List<int>();
      foreach (WTableCell cell in (CollectionImpl) this.Cells)
      {
        if (cell.PreferredWidth.WidthType == FtsWidth.Percentage && (double) cell.PreferredWidth.Width > 0.0 && (double) cell.PreferredWidth.Width <= 100.0)
        {
          cell.CellFormat.CellWidth = (float) ((double) cell.PreferredWidth.Width * (double) tableWidth / 100.0);
          num1 -= cell.CellFormat.CellWidth;
        }
        else if (cell.PreferredWidth.WidthType == FtsWidth.Point && (double) cell.PreferredWidth.Width > 0.0)
        {
          cell.CellFormat.CellWidth = cell.PreferredWidth.Width;
          num1 -= cell.CellFormat.CellWidth;
        }
        else
          intList.Add(cell.Index);
        foreach (WTable table in (IEnumerable) cell.Tables)
        {
          table.IsUpdateCellWidthByPartitioning = true;
          if (table.IsUpdateCellWidthByPartitioning)
            table.UpdateTableGrid(false, true);
        }
      }
      float num2 = num1 / (float) intList.Count;
      foreach (int index in intList)
        this.Cells[index].CellFormat.CellWidth = (double) num2 > 0.0 ? num2 : 0.0f;
    }
    else
    {
      float num = (float) Math.Round((double) tableWidth * 20.0 / (double) this.Cells.Count);
      foreach (WTableCell cell in (CollectionImpl) this.Cells)
      {
        cell.CellFormat.CellWidth = num / 20f;
        foreach (WTable table in (IEnumerable) cell.Tables)
        {
          table.IsUpdateCellWidthByPartitioning = true;
          if (table.IsUpdateCellWidthByPartitioning)
            table.UpdateTableGrid(false, true);
        }
      }
    }
  }

  internal void UpdateUnDefinedCellWidth(float tableWidth)
  {
    List<WTableCell> wtableCellList = new List<WTableCell>();
    float num1 = 0.0f;
    foreach (WTableCell cell in (CollectionImpl) this.Cells)
    {
      if (cell.IsCellWidthZero())
        wtableCellList.Add(cell);
      else
        num1 = cell.CellFormat.CellWidth;
    }
    if (wtableCellList.Count <= 0)
      return;
    float num2 = tableWidth - num1;
    if ((double) num2 > 0.0)
    {
      float num3 = num2 / (float) wtableCellList.Count;
      foreach (WTableCell wtableCell in wtableCellList)
      {
        wtableCell.CellFormat.CellWidth = num3;
        foreach (WTable table in (IEnumerable) wtableCell.Tables)
        {
          table.IsUpdateCellWidthByPartitioning = true;
          if (table.IsUpdateCellWidthByPartitioning)
            table.UpdateTableGrid(false, true);
        }
      }
    }
    else
    {
      tableWidth -= (float) wtableCellList.Count * 3f;
      foreach (WTableCell cell in (CollectionImpl) this.Cells)
      {
        if (!cell.IsCellWidthZero())
          cell.Width *= tableWidth / num1;
        else
          cell.CellFormat.CellWidth = 3f;
      }
    }
  }

  internal override void AddSelf()
  {
    foreach (Entity cell in (CollectionImpl) this.Cells)
      cell.AddSelf();
  }

  protected override object CloneImpl()
  {
    WTableRow owner = (WTableRow) base.CloneImpl();
    owner.m_charFormat = new WCharacterFormat((IWordDocument) this.Document);
    owner.m_charFormat.ImportContainer((FormatBase) this.CharacterFormat);
    owner.m_charFormat.SetOwner((OwnerHolder) owner);
    owner.m_tableFormat = new RowFormat((IWordDocument) this.Document);
    owner.m_tableFormat.ImportContainer((FormatBase) this.RowFormat);
    owner.m_tableFormat.SetOwner((OwnerHolder) owner);
    owner.m_tableFormat.Paddings.ImportPaddings(this.m_tableFormat.Paddings);
    if (this.DataArray != null)
    {
      owner.m_internalData = new byte[this.DataArray.Length];
      this.DataArray.CopyTo((Array) owner.m_internalData, 0);
    }
    owner.m_cells = new WCellCollection(owner);
    this.Cells.CloneTo((EntityCollection) owner.m_cells);
    foreach (WTableCell cell1 in (CollectionImpl) this.Cells)
    {
      foreach (WTableCell cell2 in (CollectionImpl) owner.m_cells)
        cell2.CellFormat.CopyCellFormatRevisions((FormatBase) cell1.CellFormat);
    }
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    int index = 0;
    for (int count = this.ChildEntities.Count; index < count; ++index)
    {
      Entity childEntity = this.ChildEntities[index];
      childEntity.CloneRelationsTo(doc, nextOwner);
      if (childEntity is WTableCell)
      {
        WTableCell wtableCell = childEntity as WTableCell;
        if (wtableCell.ChildEntities.Count > 0 && wtableCell.ChildEntities[wtableCell.ChildEntities.Count - 1] is WParagraph)
          wtableCell.CharacterFormat.ImportContainer((FormatBase) (wtableCell.ChildEntities[wtableCell.ChildEntities.Count - 1] as WParagraph).BreakCharacterFormat);
      }
    }
  }

  private void CheckFormatOwner()
  {
    if (this.RowFormat.OwnerBase == this)
      return;
    this.RowFormat.SetOwner((OwnerHolder) this);
  }

  internal void OnInsertCell(int index, CellFormat cellFormat)
  {
    if (this.OwnerTable == null)
      return;
    this.OwnerTable.IsTableGridUpdated = false;
    this.OwnerTable.IsTableGridVerified = false;
  }

  internal void OnRemoveCell(int index)
  {
    if (this.OwnerTable == null)
      return;
    this.OwnerTable.IsTableGridUpdated = false;
    this.OwnerTable.IsTableGridVerified = false;
  }

  internal float GetWidthToResizeCells(float clientWidth)
  {
    float num = 0.0f;
    bool flag = false;
    for (int index = 0; index < this.Cells.Count; ++index)
    {
      num += this.Cells[index].Width;
      foreach (WTable table in (IEnumerable) this.Cells[index].Tables)
      {
        if (table.PreferredTableWidth.WidthType == FtsWidth.Point && (double) table.PreferredTableWidth.Width > 0.0)
        {
          flag = true;
          break;
        }
      }
    }
    if (flag)
      return (float) ((double) num + ((double) this.RowFormat.BeforeWidth > 0.0 ? (double) this.RowFormat.BeforeWidth : (double) this.GetGridBeforeAfter(this.RowFormat.GridBeforeWidth, clientWidth)) + ((double) this.RowFormat.AfterWidth > 0.0 ? (double) this.RowFormat.AfterWidth : (double) this.GetGridBeforeAfter(this.RowFormat.GridAfterWidth, clientWidth)));
    return this.m_doc.IsDOCX() && (this.OwnerTable.IsAutoTableExceedsClientWidth() || this.OwnerTable.TableFormat.IsAutoResized && this.OwnerTable.PreferredTableWidth.WidthType == FtsWidth.Percentage && (double) this.OwnerTable.PreferredTableWidth.Width > 100.0 && !this.OwnerTable.IsInCell) ? num : clientWidth;
  }

  private float GetGridBeforeAfter(PreferredWidthInfo widthInfo, float clientWidth)
  {
    float gridBeforeAfter = 0.0f;
    if (widthInfo.WidthType == FtsWidth.Point)
      gridBeforeAfter = widthInfo.Width;
    else if (widthInfo.WidthType == FtsWidth.Percentage)
      gridBeforeAfter = (float) ((double) clientWidth * (double) widthInfo.Width / 100.0);
    return gridBeforeAfter;
  }

  internal float GetRowWidth()
  {
    float rowWidth = 0.0f;
    foreach (WTableCell cell in (CollectionImpl) this.Cells)
      rowWidth += cell.Width;
    return rowWidth;
  }

  internal void GetRowWidth(ref float preferredWidth, float tableWidth)
  {
    foreach (WTableCell cell in (CollectionImpl) this.Cells)
      preferredWidth += cell.PreferredWidth.Width;
    float num1 = 0.0f;
    float num2 = 0.0f;
    if ((double) this.RowFormat.GridBeforeWidth.Width > 0.0)
      num1 = this.GetGridBeforeAfter(this.RowFormat.GridBeforeWidth, tableWidth);
    if ((double) this.RowFormat.GridAfterWidth.Width > 0.0)
      num2 = this.GetGridBeforeAfter(this.RowFormat.GridAfterWidth, tableWidth);
    preferredWidth += num2 + num1;
  }

  internal float GetRowPreferredWidth(float tableWidth)
  {
    float rowPreferredWidth = 0.0f;
    foreach (WTableCell cell in (CollectionImpl) this.Cells)
    {
      int rowIndex = this.GetRowIndex();
      if (cell.CellFormat.VerticalMerge != CellMerge.Continue || cell.CellFormat.VerticalMerge == CellMerge.Continue && rowIndex == 0)
      {
        if (cell.PreferredWidth.WidthType == FtsWidth.Point)
          rowPreferredWidth += cell.PreferredWidth.Width * 20f;
        else if (cell.PreferredWidth.WidthType == FtsWidth.Percentage)
          rowPreferredWidth += (float) ((double) tableWidth * (double) cell.PreferredWidth.Width / 5.0);
      }
      else if (rowIndex > 0)
      {
        WTableCell verticalMergeStartCell = cell.GetVerticalMergeStartCell();
        if (verticalMergeStartCell != null)
        {
          if (verticalMergeStartCell.PreferredWidth.WidthType == FtsWidth.Point)
            rowPreferredWidth += verticalMergeStartCell.PreferredWidth.Width * 20f;
          else if (verticalMergeStartCell.PreferredWidth.WidthType == FtsWidth.Percentage)
            rowPreferredWidth += (float) ((double) tableWidth * (double) verticalMergeStartCell.PreferredWidth.Width / 5.0);
        }
      }
    }
    return rowPreferredWidth;
  }

  internal float GetRowPreferredWidthFromPoint(int defaultPrefCellWidth)
  {
    float preferredWidthFromPoint = 0.0f;
    foreach (WTableCell cell in (CollectionImpl) this.Cells)
    {
      int rowIndex = this.GetRowIndex();
      if (cell.CellFormat.VerticalMerge != CellMerge.Continue || cell.CellFormat.VerticalMerge == CellMerge.Continue && rowIndex == 0)
      {
        if (cell.PreferredWidth.WidthType == FtsWidth.Point)
          preferredWidthFromPoint += cell.PreferredWidth.Width;
        else if (cell.PreferredWidth.WidthType == FtsWidth.None)
          preferredWidthFromPoint += (float) defaultPrefCellWidth;
      }
      else if (rowIndex > 0)
      {
        WTableCell verticalMergeStartCell = cell.GetVerticalMergeStartCell();
        if (verticalMergeStartCell != null)
        {
          if (verticalMergeStartCell.PreferredWidth.WidthType == FtsWidth.Point)
            preferredWidthFromPoint += verticalMergeStartCell.PreferredWidth.Width;
          else if (cell.PreferredWidth.WidthType == FtsWidth.None)
            preferredWidthFromPoint += (float) defaultPrefCellWidth;
        }
      }
    }
    return preferredWidthFromPoint;
  }

  internal override void Close()
  {
    if (this.m_cells != null && this.m_cells.Count > 0)
    {
      int count = this.m_cells.Count;
      WTableCell wtableCell = (WTableCell) null;
      for (int index = 0; index < count; ++index)
      {
        this.m_cells[index].Close();
        wtableCell = (WTableCell) null;
      }
      this.m_cells.Close();
      this.m_cells = (WCellCollection) null;
    }
    if (this.m_tableFormat != null)
    {
      this.m_tableFormat.Close();
      this.m_tableFormat = (RowFormat) null;
    }
    if (this.m_charFormat != null)
    {
      this.m_charFormat.Close();
      this.m_charFormat = (WCharacterFormat) null;
    }
    if (this.m_trackRowFormat != null)
    {
      this.m_trackRowFormat.Close();
      this.m_trackRowFormat = (RowFormat) null;
    }
    if (this.m_RowContentControl != null)
    {
      this.m_RowContentControl.Close();
      this.m_RowContentControl = (RowContentControl) null;
    }
    if (this.m_internalData != null)
      this.m_internalData = (byte[]) null;
    base.Close();
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("cells", (object) this.Cells);
    this.XDLSHolder.AddElement("character-format", (object) this.CharacterFormat);
    this.XDLSHolder.AddElement("table-format", (object) this.RowFormat);
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    if (this.DataArray == null)
      return;
    writer.WriteChildBinaryElement("internal-data", this.DataArray);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    bool flag = base.ReadXmlContent(reader);
    if (reader.TagName == "internal-data")
    {
      this.DataArray = reader.ReadChildBinaryElement();
      flag = true;
    }
    return flag;
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    if (this.IsHeader)
      writer.WriteValue("IsHeader", this.IsHeader);
    if (this.m_tableFormat.HasSprms())
      return;
    if ((double) this.Height > 0.0)
      writer.WriteValue("RowHeight", this.Height);
    writer.WriteValue("HeightType", (Enum) this.HeightType);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    if (reader.HasAttribute("RowHeight"))
      this.Height = reader.ReadFloat("RowHeight");
    if (reader.HasAttribute("IsHeader"))
      this.IsHeader = reader.ReadBoolean("IsHeader");
    if (!reader.HasAttribute("HeightType"))
      return;
    this.HeightType = (TableRowHeightType) reader.ReadEnum("HeightType", typeof (TableRowHeightType));
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new RowLayoutInfo(this.HeightType == TableRowHeightType.Exactly && ((double) this.Height >= 0.0 ? (double) this.Height : -1.0 * (double) this.Height) > 0.0, this.Height);
    this.m_layoutInfo.IsVerticalText = true;
    this.m_layoutInfo.IsSkip = false;
    for (int index = 0; index < this.Cells.Count; ++index)
    {
      if (this.Cells[index].CellFormat.TextDirection != TextDirection.VerticalBottomToTop && this.Cells[index].CellFormat.TextDirection != TextDirection.VerticalTopToBottom)
      {
        this.m_layoutInfo.IsVerticalText = false;
        break;
      }
    }
    if (!this.OwnerTable.IsInCell && !this.OwnerTable.TableFormat.WrapTextAround)
      this.m_layoutInfo.IsKeepWithNext = this.IsKeepWithNext;
    bool flag = false;
    for (int index = 0; index < this.Cells.Count; ++index)
    {
      flag = this.Cells[index].CellFormat.HideMark && this.Cells[index].Paragraphs.Count == 1 && this.Cells[index].Tables.Count == 0 && (this.Cells[index].Paragraphs[0].Items.Count <= 0 || this.IsNeedToSkipParaMark(this.Cells[index].Paragraphs[0], this.Cells[index].Paragraphs[0].Items.Count));
      if (!flag)
        break;
    }
    if (flag)
      this.m_layoutInfo.IsHiddenRow = true;
    if (this.Cells.Count < 1)
      this.m_layoutInfo.IsSkip = true;
    if (!this.IsDeleteRevision)
      return;
    this.m_layoutInfo.IsSkip = true;
  }

  private bool IsNeedToSkipParaMark(WParagraph paragraph, int paraItemCount)
  {
    for (int index = 0; index < paraItemCount; ++index)
    {
      if (paragraph.ChildEntities[index] is ParagraphItem childEntity)
      {
        switch (childEntity)
        {
          case BookmarkStart _:
          case BookmarkEnd _:
            continue;
          case WTextRange _:
            if ((childEntity as WTextRange).Text == "")
              continue;
            break;
        }
        return false;
      }
    }
    return true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_cells == null || this.m_cells.Count <= 0)
      return;
    int count = this.m_cells.Count;
    WTableCell wtableCell = (WTableCell) null;
    for (int index = 0; index < count; ++index)
    {
      this.m_cells[index].InitLayoutInfo(entity, ref isLastTOCEntry);
      wtableCell = (WTableCell) null;
      if (isLastTOCEntry)
        break;
    }
  }

  internal bool IsKeepWithNext
  {
    get
    {
      WParagraph paragraph = (WParagraph) null;
      this.GetFirstParagraphOfRow(ref paragraph);
      return paragraph != null && paragraph.ParagraphFormat.KeepFollow;
    }
  }

  private void GetFirstParagraphOfRow(ref WParagraph paragraph)
  {
    if (this.ChildEntities.Count <= 0 || this.Cells[0].ChildEntities.Count <= 0)
      return;
    if (this.Cells[0].ChildEntities[0] is WTable && (this.Cells[0].ChildEntities[0] as WTable).Rows.Count > 0)
      (this.Cells[0].ChildEntities[0] as WTable).Rows[0].GetFirstParagraphOfRow(ref paragraph);
    if (!(this.Cells[0].ChildEntities[0] is WParagraph))
      return;
    paragraph = this.Cells[0].ChildEntities[0] as WParagraph;
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
