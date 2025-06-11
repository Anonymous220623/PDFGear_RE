// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.TableLayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;

#nullable disable
namespace Syncfusion.Layouting;

internal class TableLayoutInfo : LayoutInfo, ITableLayoutInfo, ILayoutSpacingsInfo
{
  private WTable m_table;
  private float[] m_cellsWidth;
  private int m_headersRowCount;
  private bool[] m_isDefaultCells;
  private float m_width;
  private float m_headerRowHeight;
  private byte m_bFlags;
  private Spacings m_paddings;
  private Spacings m_margins;

  public float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  public float Height => 0.0f;

  public float[] CellsWidth
  {
    get => this.m_cellsWidth;
    set => this.m_cellsWidth = value;
  }

  public int HeadersRowCount => this.m_headersRowCount;

  public bool[] IsDefaultCells => this.m_isDefaultCells;

  public bool IsSplittedTable
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal float HeaderRowHeight
  {
    get => this.m_headerRowHeight;
    set => this.m_headerRowHeight = value;
  }

  internal bool IsHeaderRowHeightUpdated
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsHeaderNotRepeatForAllPages
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  public Spacings Paddings
  {
    get
    {
      if (this.m_paddings == null)
        this.m_paddings = new Spacings();
      return this.m_paddings;
    }
  }

  public Spacings Margins
  {
    get
    {
      if (this.m_margins == null)
        this.m_margins = new Spacings();
      return this.m_margins;
    }
  }

  public TableLayoutInfo(WTable table)
    : base(ChildrenLayoutDirection.Horizontal)
  {
    this.m_table = table;
    int length = 0;
    int index1 = 0;
    float num1 = 0.0f;
    for (int index2 = 0; index2 < this.m_table.Rows.Count; ++index2)
    {
      float num2 = 0.0f;
      for (int index3 = 0; index3 < this.m_table.Rows[index2].Cells.Count; ++index3)
        num2 += this.m_table.Rows[index2].Cells[index3].Width;
      if (index2 == 0 || (double) num1 < (double) num2)
      {
        num1 = num2;
        length = this.m_table.Rows[index2].Cells.Count;
        index1 = index2;
      }
    }
    if (!this.IsSplittedTable)
    {
      this.AddParagraphToEmptyCell(this.m_table);
      this.m_table.ApplyBaseStyleFormats();
    }
    if (!this.m_table.IsInCell && this.m_table.Document.ActualFormatType != FormatType.Doc && this.m_table.Document.ActualFormatType != FormatType.Dot)
      this.m_table.AutoFitColumns(false);
    else if (this.m_table.Document.ActualFormatType == FormatType.Doc && this.m_table.TableFormat.IsAutoResized && (double) this.m_table.PreferredTableWidth.Width == 0.0)
      this.CheckNeedToAutoFit(this.m_table);
    this.m_width = this.m_table.Width;
    this.m_cellsWidth = new float[length];
    this.m_isDefaultCells = new bool[length];
    int index4 = 0;
    for (int index5 = length; index4 < index5; ++index4)
    {
      WTableCell cell = this.m_table.Rows[index1].Cells[index4];
      this.m_cellsWidth[index4] = cell.Width;
      this.m_isDefaultCells[index4] = cell.IsFixedWidth;
    }
    this.m_headersRowCount = this.GetHeadersRowCount();
  }

  private bool CheckNeedToAutoFit(WTable table)
  {
    bool autoFit = false;
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if ((double) cell.PreferredWidth.Width != 0.0)
          return false;
        autoFit = true;
      }
    }
    return autoFit;
  }

  private void AddParagraphToEmptyCell(WTable table)
  {
    for (int index1 = 0; index1 < table.Rows.Count; ++index1)
    {
      WTableRow row = table.Rows[index1];
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        bool flag = true;
        if (cell.Paragraphs.Count == 0)
        {
          flag = false;
          for (int index3 = 0; index3 < cell.ChildEntities.Count && !flag; ++index3)
          {
            if (cell.ChildEntities[index3] is BlockContentControl)
              flag = (cell.ChildEntities[index3] as BlockContentControl).ContainsParagraph();
          }
        }
        if (!flag)
          cell.AddParagraph();
      }
    }
  }

  private int GetHeadersRowCount()
  {
    int headersRowCount = 0;
    for (int index = 0; index < this.m_table.Rows.Count && this.m_table.Rows[index].IsHeader; ++index)
      ++headersRowCount;
    return headersRowCount;
  }

  public double CellSpacings
  {
    get
    {
      return this.m_table.Owner.Owner is WTableCell ? Math.Max(0.0, (double) (this.m_table.Owner.Owner as WTableCell).OwnerRow.RowFormat.CellSpacing * 2.0) : 0.0;
    }
  }

  public double CellPaddings
  {
    get
    {
      return this.m_table.Owner.Owner is WTableCell ? (double) (this.m_table.Owner.Owner as WTableCell).CellFormat.Paddings.Left + (double) (this.m_table.Owner.Owner as WTableCell).CellFormat.Paddings.Right : 0.0;
    }
  }
}
