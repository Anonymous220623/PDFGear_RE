// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridRow
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridRow
{
  private PdfGridCellCollection m_cells;
  private PdfGrid m_grid;
  private PdfGridRowStyle m_style;
  private float m_height = float.MinValue;
  private float m_width = float.MinValue;
  private bool m_bRowSpanExists;
  private bool m_bColumnSpanExists;
  private float m_rowBreakHeight;
  private int m_rowOverflowIndex;
  private PdfLayoutResult m_gridResult;
  internal bool isRowBreaksNextPage;
  internal float rowBreakHeight;
  internal bool isrowFinish;
  internal bool isComplete;
  private bool m_rowMergeComplete = true;
  internal int m_noOfPageCount;
  internal bool m_isRowHeightSet;
  private PdfTag m_tag;
  internal int maximumRowSpan;
  internal bool isPageBreakRowSpanApplied;
  internal bool m_isRowSpanRowHeightSet;
  internal float m_rowSpanRemainingHeight;
  internal bool m_isHeaderRow;
  internal bool m_drawCellBroders;
  internal float m_borderReminingHeight;
  internal bool m_paginatedGridRow;

  public PdfGridCellCollection Cells
  {
    get
    {
      if (this.m_cells == null)
        this.m_cells = new PdfGridCellCollection(this);
      return this.m_cells;
    }
  }

  internal PdfGrid Grid
  {
    get => this.m_grid;
    set => this.m_grid = value;
  }

  internal bool IsHeaderRow
  {
    get => this.m_isHeaderRow;
    set => this.m_isHeaderRow = value;
  }

  public PdfGridRowStyle Style
  {
    get
    {
      if (this.m_style == null)
        this.m_style = new PdfGridRowStyle();
      return this.m_style;
    }
    set => this.m_style = value;
  }

  public float Height
  {
    get
    {
      if (!this.m_isRowHeightSet)
        this.m_height = this.MeasureHeight();
      return this.m_height;
    }
    set
    {
      this.m_height = value;
      this.m_isRowHeightSet = true;
    }
  }

  internal float Width
  {
    get
    {
      if ((double) this.m_width == -3.4028234663852886E+38)
        this.m_width = this.MeasureWidth();
      return this.m_width;
    }
  }

  internal bool RowSpanExists
  {
    get => this.m_bRowSpanExists;
    set => this.m_bRowSpanExists = value;
  }

  internal bool ColumnSpanExists
  {
    get => this.m_bColumnSpanExists;
    set => this.m_bColumnSpanExists = value;
  }

  internal float RowBreakHeight
  {
    get => this.m_rowBreakHeight;
    set => this.m_rowBreakHeight = value;
  }

  internal int RowOverflowIndex
  {
    get => this.m_rowOverflowIndex;
    set => this.m_rowOverflowIndex = value;
  }

  internal PdfLayoutResult NestedGridLayoutResult
  {
    get => this.m_gridResult;
    set => this.m_gridResult = value;
  }

  internal int RowIndex => this.Grid.Rows.IndexOf(this);

  internal bool RowMergeComplete
  {
    get => this.m_rowMergeComplete;
    set => this.m_rowMergeComplete = value;
  }

  public PdfTag PdfTag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  public PdfGridRow(PdfGrid grid) => this.m_grid = grid;

  private float MeasureHeight()
  {
    float num1 = 0.0f;
    bool flag = false;
    float num2 = 0.0f;
    float val1 = this.Cells[0].RowSpan <= 1 ? this.Cells[0].Height : 0.0f;
    if (this.Grid.Headers.IndexOf(this) != -1)
      flag = true;
    foreach (PdfGridCell cell in this.Cells)
    {
      if ((double) cell.m_rowSpanRemainingHeight > (double) num1)
        num1 = cell.m_rowSpanRemainingHeight;
      if (!cell.IsRowMergeContinue)
      {
        if (!cell.IsRowMergeContinue)
          this.RowMergeComplete = false;
        if (cell.RowSpan > 1)
        {
          if ((double) num2 < (double) cell.Height)
            num2 = cell.Height;
        }
        else
          val1 = Math.Max(val1, cell.Height);
      }
    }
    if ((double) val1 == 0.0)
      val1 = num2;
    else if ((double) num1 > 0.0)
      val1 += num1;
    if (flag && (double) num2 != 0.0 && (double) val1 != 0.0 && (double) val1 < (double) num2)
      val1 = num2;
    return val1;
  }

  private float MeasureWidth()
  {
    float num = 0.0f;
    foreach (PdfGridColumn column in this.Grid.Columns)
      num += column.Width;
    return num;
  }

  public void ApplyStyle(PdfGridCellStyle cellStyle)
  {
    foreach (PdfGridCell cell in this.Cells)
      cell.Style = cellStyle;
  }

  internal PdfGridRow CloneGridRow() => (PdfGridRow) this.MemberwiseClone();
}
