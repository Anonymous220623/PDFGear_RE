// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGrid
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGrid : PdfLayoutElement
{
  private PdfGridHeaderCollection m_headers;
  private PdfGridRowCollection m_rows;
  private object m_dataSource;
  private string m_dataMember;
  private PdfDataSource m_dsParser;
  private PdfGridStyle m_style;
  private PdfGridColumnCollection m_columns;
  private bool m_bRepeatHeader;
  private SizeF m_size = SizeF.Empty;
  private bool m_breakRow = true;
  private bool m_isChildGrid;
  private PdfGridCell m_parentCell;
  private float m_initialWidth;
  private PdfLayoutFormat m_layoutFormat;
  internal bool isComplete;
  internal bool isWidthSet;
  internal int parentCellIndex;
  internal bool isDrawn;
  internal float m_rowLayoutBoundswidth;
  internal List<int> m_listOfNavigatePages = new List<int>();
  internal bool m_isRearranged;
  private bool m_headerRow = true;
  private bool m_bandedRow = true;
  private bool m_bandedColumn;
  private bool m_totalRow;
  private bool m_firstColumn;
  private bool m_lastColumn;
  internal RectangleF m_gridLocation;
  private bool m_isPageWidth;
  internal bool isBuiltinStyle;
  internal PdfGridBuiltinStyle m_gridBuiltinStyle;
  internal bool isSignleGrid = true;
  internal bool m_hasColumnSpan;
  internal bool m_hasRowSpanSpan;
  internal bool m_hasHTMLText;

  public PdfGridHeaderCollection Headers
  {
    get
    {
      if (this.m_headers == null)
        this.m_headers = new PdfGridHeaderCollection(this);
      return this.m_headers;
    }
  }

  public PdfGridRowCollection Rows
  {
    get
    {
      if (this.m_rows == null)
        this.m_rows = new PdfGridRowCollection(this);
      return this.m_rows;
    }
  }

  public object DataSource
  {
    get => this.m_dataSource;
    set
    {
      if (value == null || value == this.m_dataSource)
        return;
      this.m_dataSource = value;
      this.Columns.Clear();
      this.SetDataSource();
    }
  }

  public string DataMember
  {
    get => this.m_dataMember;
    set
    {
      if (value == null && !(this.m_dataMember != value))
        return;
      this.m_dataMember = value;
      this.Columns.Clear();
      this.SetDataSource();
    }
  }

  public PdfGridStyle Style
  {
    get
    {
      if (this.m_style == null)
        this.m_style = new PdfGridStyle();
      return this.m_style;
    }
    set => this.m_style = value;
  }

  internal PdfGridRow LastRow
  {
    get => this.Rows.Count > 0 ? this.Rows[this.Rows.Count - 1] : (PdfGridRow) null;
  }

  public PdfGridColumnCollection Columns
  {
    get
    {
      if (this.m_columns == null)
        this.m_columns = new PdfGridColumnCollection(this);
      return this.m_columns;
    }
  }

  public bool RepeatHeader
  {
    get => this.m_bRepeatHeader;
    set => this.m_bRepeatHeader = value;
  }

  internal SizeF Size
  {
    get
    {
      if (this.m_size == SizeF.Empty)
        this.m_size = this.Measure();
      return this.m_size;
    }
  }

  public bool AllowRowBreakAcrossPages
  {
    get => this.m_breakRow;
    set => this.m_breakRow = value;
  }

  internal bool IsChildGrid
  {
    get => this.m_isChildGrid;
    set => this.m_isChildGrid = value;
  }

  internal PdfGridCell ParentCell
  {
    get => this.m_parentCell;
    set => this.m_parentCell = value;
  }

  internal PdfLayoutFormat LayoutFormat
  {
    get => this.m_layoutFormat;
    set => this.m_layoutFormat = value;
  }

  internal bool RaiseBeginCellLayout => this.BeginCellLayout != null;

  internal bool RaiseEndCellLayout => this.EndCellLayout != null;

  internal bool IsPageWidth
  {
    get => this.m_isPageWidth;
    set => this.m_isPageWidth = value;
  }

  internal float InitialWidth
  {
    get => this.m_initialWidth;
    set => this.m_initialWidth = value;
  }

  public event PdfGridBeginCellLayoutEventHandler BeginCellLayout;

  public event PdfGridEndCellLayoutEventHandler EndCellLayout;

  public void Draw(PdfGraphics graphics, PointF location, float width)
  {
    this.Draw(graphics, location.X, location.Y, width);
  }

  public void Draw(PdfGraphics graphics, float x, float y, float width)
  {
    this.InitialWidth = width;
    this.isWidthSet = true;
    RectangleF bounds = new RectangleF(x, y, width, 0.0f);
    this.Draw(graphics, bounds);
  }

  internal PdfGrid Clone(PdfGrid grid) => (PdfGrid) grid.MemberwiseClone();

  public void Draw(PdfGraphics graphics, RectangleF bounds)
  {
    this.SetSpan();
    this.InitialWidth = bounds.Width;
    this.isWidthSet = true;
    new PdfGridLayouter(this).Layout(graphics, bounds);
    this.isComplete = true;
  }

  public PdfGridLayoutResult Draw(PdfPage page, PointF location)
  {
    this.InitialWidth = page.Graphics.ClientSize.Width;
    PdfLayoutResult pdfLayoutResult = base.Draw(page, location);
    this.isComplete = true;
    return (PdfGridLayoutResult) pdfLayoutResult;
  }

  public PdfGridLayoutResult Draw(PdfPage page, PointF location, PdfGridLayoutFormat format)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    this.InitialWidth = page.Graphics.ClientSize.Width;
    PdfLayoutResult pdfLayoutResult = this.Draw(page, location, (PdfLayoutFormat) format);
    this.isComplete = true;
    return (PdfGridLayoutResult) pdfLayoutResult;
  }

  public PdfGridLayoutResult Draw(PdfPage page, RectangleF bounds)
  {
    this.InitialWidth = bounds.Width;
    this.isWidthSet = true;
    PdfLayoutResult pdfLayoutResult = base.Draw(page, bounds);
    this.isComplete = true;
    return (PdfGridLayoutResult) pdfLayoutResult;
  }

  public PdfGridLayoutResult Draw(PdfPage page, RectangleF bounds, PdfGridLayoutFormat format)
  {
    this.InitialWidth = bounds.Width;
    this.isWidthSet = true;
    PdfLayoutResult pdfLayoutResult = this.Draw(page, bounds, (PdfLayoutFormat) format);
    this.isComplete = true;
    return (PdfGridLayoutResult) pdfLayoutResult;
  }

  public PdfGridLayoutResult Draw(PdfPage page, float x, float y)
  {
    this.InitialWidth = page.Graphics.ClientSize.Width;
    PdfLayoutResult pdfLayoutResult = base.Draw(page, x, y);
    this.isComplete = true;
    return (PdfGridLayoutResult) pdfLayoutResult;
  }

  public PdfGridLayoutResult Draw(PdfPage page, float x, float y, PdfGridLayoutFormat format)
  {
    this.InitialWidth = page.Graphics.ClientSize.Width;
    PdfLayoutResult pdfLayoutResult = this.Draw(page, x, y, (PdfLayoutFormat) format);
    this.isComplete = true;
    return (PdfGridLayoutResult) pdfLayoutResult;
  }

  public PdfGridLayoutResult Draw(PdfPage page, float x, float y, float width)
  {
    return this.Draw(page, x, y, width, (PdfGridLayoutFormat) null);
  }

  public PdfGridLayoutResult Draw(
    PdfPage page,
    float x,
    float y,
    float width,
    PdfGridLayoutFormat format)
  {
    RectangleF layoutRectangle = new RectangleF(x, y, width, 0.0f);
    this.InitialWidth = layoutRectangle.Width;
    this.isWidthSet = true;
    return (PdfGridLayoutResult) this.Draw(page, layoutRectangle, (PdfLayoutFormat) format);
  }

  protected override PdfLayoutResult Layout(PdfLayoutParams param)
  {
    if ((double) param.Bounds.Width < 0.0)
      throw new ArgumentOutOfRangeException("Width");
    this.SetSpan();
    this.m_layoutFormat = param.Format;
    this.m_gridLocation = param.Bounds;
    PdfGridLayoutResult gridLayoutResult = (PdfGridLayoutResult) new PdfGridLayouter(this).Layout(param);
    this.Dispose();
    return (PdfLayoutResult) gridLayoutResult;
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    this.SetSpan();
    new PdfGridLayouter(this).Layout(graphics, PointF.Empty);
    this.isComplete = true;
  }

  internal void SetSpan()
  {
    int num1 = 1;
    int num2 = 0;
    int index1 = 0;
    for (int count1 = this.Headers.Count; index1 < count1; ++index1)
    {
      PdfGridRow header = this.Headers[index1];
      int val1 = 0;
      int index2 = 0;
      for (int count2 = header.Cells.Count; index2 < count2; ++index2)
      {
        PdfGridCell cell = header.Cells[index2];
        val1 = Math.Max(val1, cell.RowSpan);
        if (!cell.IsCellMergeContinue && !cell.IsRowMergeContinue && (cell.ColumnSpan > 1 || cell.RowSpan > 1))
        {
          if (cell.ColumnSpan + index2 > header.Cells.Count)
            throw new ArgumentException($"Invalid span specified at row {index2.ToString()} column {index1.ToString()}");
          if (cell.RowSpan + index1 > this.Headers.Count)
            throw new ArgumentException($"Invalid span specified at Header {index2.ToString()} column {index1.ToString()}");
          if (cell.ColumnSpan > 1 && cell.RowSpan > 1)
          {
            int columnSpan1 = cell.ColumnSpan;
            int rowSpan = cell.RowSpan;
            int index3 = index2;
            int index4 = index1;
            cell.IsCellMergeStart = true;
            cell.IsRowMergeStart = true;
            for (; columnSpan1 > 1; --columnSpan1)
            {
              ++index3;
              header.Cells[index3].IsCellMergeContinue = true;
              header.Cells[index3].IsRowMergeContinue = true;
              header.Cells[index3].RowSpan = rowSpan;
            }
            int index5 = index2;
            int columnSpan2 = cell.ColumnSpan;
            while (rowSpan > 1)
            {
              ++index4;
              this.Headers[index4].Cells[index2].IsRowMergeContinue = true;
              this.Headers[index4].Cells[index5].IsRowMergeContinue = true;
              --rowSpan;
              for (; columnSpan2 > 1; --columnSpan2)
              {
                ++index5;
                this.Headers[index4].Cells[index5].IsCellMergeContinue = true;
                this.Headers[index4].Cells[index5].IsRowMergeContinue = true;
              }
              columnSpan2 = cell.ColumnSpan;
              index5 = index2;
            }
          }
          else if (cell.ColumnSpan > 1 && cell.RowSpan == 1)
          {
            int columnSpan = cell.ColumnSpan;
            int index6 = index2;
            cell.IsCellMergeStart = true;
            for (; columnSpan > 1; --columnSpan)
            {
              ++index6;
              header.Cells[index6].IsCellMergeContinue = true;
            }
          }
          else if (cell.ColumnSpan == 1 && cell.RowSpan > 1)
          {
            int rowSpan = cell.RowSpan;
            int index7 = index1;
            for (; rowSpan > 1; --rowSpan)
            {
              ++index7;
              this.Headers[index7].Cells[index2].IsRowMergeContinue = true;
            }
          }
        }
      }
      header.maximumRowSpan = val1;
    }
    int num3 = num1 = 1;
    int num4 = num2 = 0;
    if (!this.m_hasColumnSpan && !this.m_hasRowSpanSpan)
      return;
    int index8 = 0;
    for (int count3 = this.Rows.Count; index8 < count3; ++index8)
    {
      PdfGridRow row = this.Rows[index8];
      int val1 = 0;
      int index9 = 0;
      for (int count4 = row.Cells.Count; index9 < count4; ++index9)
      {
        PdfGridCell cell = row.Cells[index9];
        val1 = Math.Max(val1, cell.RowSpan);
        if (!cell.IsCellMergeContinue && !cell.IsRowMergeContinue && (cell.ColumnSpan > 1 || cell.RowSpan > 1))
        {
          if (cell.ColumnSpan + index9 > row.Cells.Count)
            throw new ArgumentException($"Invalid span specified at row {index9.ToString()} column {index8.ToString()}");
          if (cell.RowSpan + index8 > this.Rows.Count)
            throw new ArgumentException($"Invalid span specified at row {index9.ToString()} column {index8.ToString()}");
          if (cell.ColumnSpan > 1 && cell.RowSpan > 1)
          {
            int columnSpan3 = cell.ColumnSpan;
            int rowSpan = cell.RowSpan;
            int index10 = index9;
            int index11 = index8;
            cell.IsCellMergeStart = true;
            cell.IsRowMergeStart = true;
            for (; columnSpan3 > 1; --columnSpan3)
            {
              ++index10;
              row.Cells[index10].IsCellMergeContinue = true;
              row.Cells[index10].IsRowMergeContinue = true;
            }
            int index12 = index9;
            int columnSpan4 = cell.ColumnSpan;
            while (rowSpan > 1)
            {
              ++index11;
              this.Rows[index11].Cells[index9].IsRowMergeContinue = true;
              this.Rows[index11].Cells[index12].IsRowMergeContinue = true;
              --rowSpan;
              for (; columnSpan4 > 1; --columnSpan4)
              {
                ++index12;
                this.Rows[index11].Cells[index12].IsCellMergeContinue = true;
                this.Rows[index11].Cells[index12].IsRowMergeContinue = true;
              }
              columnSpan4 = cell.ColumnSpan;
              index12 = index9;
            }
          }
          else if (cell.ColumnSpan > 1 && cell.RowSpan == 1)
          {
            int columnSpan = cell.ColumnSpan;
            int index13 = index9;
            cell.IsCellMergeStart = true;
            for (; columnSpan > 1; --columnSpan)
            {
              ++index13;
              row.Cells[index13].IsCellMergeContinue = true;
            }
          }
          else if (cell.ColumnSpan == 1 && cell.RowSpan > 1)
          {
            int rowSpan = cell.RowSpan;
            int index14 = index8;
            for (; rowSpan > 1; --rowSpan)
            {
              ++index14;
              this.Rows[index14].Cells[index9].IsRowMergeContinue = true;
            }
          }
        }
      }
      row.maximumRowSpan = val1;
    }
  }

  private SizeF Measure()
  {
    float height = 0.0f;
    float width = this.Columns.Width;
    foreach (PdfGridRow header in this.Headers)
      height += header.Height;
    foreach (PdfGridRow row in (List<PdfGridRow>) this.Rows)
      height += row.Height;
    return new SizeF(width, height);
  }

  internal void OnBeginCellLayout(PdfGridBeginCellLayoutEventArgs args)
  {
    if (!this.RaiseBeginCellLayout)
      return;
    this.BeginCellLayout((object) this, args);
  }

  internal void OnEndCellLayout(PdfGridEndCellLayoutEventArgs args)
  {
    if (!this.RaiseEndCellLayout)
      return;
    this.EndCellLayout((object) this, args);
  }

  private void Dispose()
  {
    foreach (PdfGridRow header in this.Headers)
    {
      if ((double) header.RowBreakHeight > 0.0)
      {
        foreach (PdfGridCell cell in header.Cells)
        {
          cell.FinishedDrawingCell = true;
          cell.RemainingString = (string) null;
        }
      }
      header.RowBreakHeight = 0.0f;
      header.rowBreakHeight = 0.0f;
      header.isRowBreaksNextPage = false;
      header.isPageBreakRowSpanApplied = false;
    }
    foreach (PdfGridRow row in (List<PdfGridRow>) this.Rows)
    {
      if ((double) row.RowBreakHeight > 0.0)
      {
        foreach (PdfGridCell cell in row.Cells)
        {
          cell.FinishedDrawingCell = true;
          cell.RemainingString = (string) null;
        }
      }
      row.RowBreakHeight = 0.0f;
      row.rowBreakHeight = 0.0f;
      row.isRowBreaksNextPage = false;
      row.isPageBreakRowSpanApplied = false;
    }
  }

  public void ApplyBuiltinStyle(PdfGridBuiltinStyle gridStyle)
  {
    this.isBuiltinStyle = true;
    this.m_gridBuiltinStyle = gridStyle;
  }

  internal void ApplyBuiltinStyles(PdfGridBuiltinStyle gridStyle)
  {
    switch (gridStyle)
    {
      case PdfGridBuiltinStyle.PlainTable1:
        this.ApplyPlainTable1(Color.FromArgb((int) byte.MaxValue, 191, 191, 191), Color.FromArgb((int) byte.MaxValue, 242, 242, 242));
        break;
      case PdfGridBuiltinStyle.PlainTable2:
        this.ApplyPlainTable2(Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue));
        break;
      case PdfGridBuiltinStyle.PlainTable3:
        this.ApplyPlainTable3(Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), Color.FromArgb((int) byte.MaxValue, 242, 242, 242));
        break;
      case PdfGridBuiltinStyle.PlainTable4:
        this.ApplyPlainTable4(Color.FromArgb((int) byte.MaxValue, 242, 242, 242));
        break;
      case PdfGridBuiltinStyle.PlainTable5:
        this.ApplyPlainTable5(Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), Color.FromArgb((int) byte.MaxValue, 242, 242, 242));
        break;
      case PdfGridBuiltinStyle.GridTable1Light:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 153, 153, 153), Color.FromArgb((int) byte.MaxValue, 102, 102, 102));
        break;
      case PdfGridBuiltinStyle.GridTable1LightAccent1:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 189, 214, 238), Color.FromArgb((int) byte.MaxValue, 156, 194, 229));
        break;
      case PdfGridBuiltinStyle.GridTable1LightAccent2:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 247, 202, 172), Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131));
        break;
      case PdfGridBuiltinStyle.GridTable1LightAccent3:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 219, 219, 219), Color.FromArgb((int) byte.MaxValue, 201, 201, 201));
        break;
      case PdfGridBuiltinStyle.GridTable1LightAccent4:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 229, 153), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102));
        break;
      case PdfGridBuiltinStyle.GridTable1LightAccent5:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 180, 198, 231), Color.FromArgb((int) byte.MaxValue, 142, 170, 219));
        break;
      case PdfGridBuiltinStyle.GridTable1LightAccent6:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 224 /*0xE0*/, 179), Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141));
        break;
      case PdfGridBuiltinStyle.GridTable2:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfGridBuiltinStyle.GridTable2Accent1:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfGridBuiltinStyle.GridTable2Accent2:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfGridBuiltinStyle.GridTable2Accent3:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfGridBuiltinStyle.GridTable2Accent4:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfGridBuiltinStyle.GridTable2Accent5:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfGridBuiltinStyle.GridTable2Accent6:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfGridBuiltinStyle.GridTable3:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfGridBuiltinStyle.GridTable3Accent1:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfGridBuiltinStyle.GridTable3Accent2:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfGridBuiltinStyle.GridTable3Accent3:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfGridBuiltinStyle.GridTable3Accent4:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfGridBuiltinStyle.GridTable3Accent5:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfGridBuiltinStyle.GridTable3Accent6:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfGridBuiltinStyle.GridTable4:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfGridBuiltinStyle.GridTable4Accent1:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 91, 155, 213));
        break;
      case PdfGridBuiltinStyle.GridTable4Accent2:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 237, 125, 49));
        break;
      case PdfGridBuiltinStyle.GridTable4Accent3:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 165, 165, 165));
        break;
      case PdfGridBuiltinStyle.GridTable4Accent4:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0));
        break;
      case PdfGridBuiltinStyle.GridTable4Accent5:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 68, 114, 196));
        break;
      case PdfGridBuiltinStyle.GridTable4Accent6:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71));
        break;
      case PdfGridBuiltinStyle.GridTable5Dark:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 153, 153, 153), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfGridBuiltinStyle.GridTable5DarkAccent1:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 91, 155, 213), Color.FromArgb((int) byte.MaxValue, 189, 214, 238), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfGridBuiltinStyle.GridTable5DarkAccent2:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 237, 125, 49), Color.FromArgb((int) byte.MaxValue, 247, 202, 172), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfGridBuiltinStyle.GridTable5DarkAccent3:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 165, 165, 165), Color.FromArgb((int) byte.MaxValue, 219, 219, 219), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfGridBuiltinStyle.GridTable5DarkAccent4:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 229, 153), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfGridBuiltinStyle.GridTable5DarkAccent5:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 68, 114, 196), Color.FromArgb((int) byte.MaxValue, 180, 198, 231), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfGridBuiltinStyle.GridTable5DarkAccent6:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 171, 71), Color.FromArgb((int) byte.MaxValue, 197, 224 /*0xE0*/, 179), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfGridBuiltinStyle.GridTable6Colorful:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfGridBuiltinStyle.GridTable6ColorfulAccent1:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 46, 116, 181));
        break;
      case PdfGridBuiltinStyle.GridTable6ColorfulAccent2:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 196, 89, 17));
        break;
      case PdfGridBuiltinStyle.GridTable6ColorfulAccent3:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 123, 123, 123));
        break;
      case PdfGridBuiltinStyle.GridTable6ColorfulAccent4:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, 191, 143, 0));
        break;
      case PdfGridBuiltinStyle.GridTable6ColorfulAccent5:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 47, 84, 150));
        break;
      case PdfGridBuiltinStyle.GridTable6ColorfulAccent6:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 83, 129, 53));
        break;
      case PdfGridBuiltinStyle.GridTable7Colorful:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfGridBuiltinStyle.GridTable7ColorfulAccent1:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 46, 116, 181));
        break;
      case PdfGridBuiltinStyle.GridTable7ColorfulAccent2:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 196, 89, 17));
        break;
      case PdfGridBuiltinStyle.GridTable7ColorfulAccent3:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 123, 123, 123));
        break;
      case PdfGridBuiltinStyle.GridTable7ColorfulAccent4:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, 191, 143, 0));
        break;
      case PdfGridBuiltinStyle.GridTable7ColorfulAccent5:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 47, 84, 150));
        break;
      case PdfGridBuiltinStyle.GridTable7ColorfulAccent6:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 83, 129, 53));
        break;
      case PdfGridBuiltinStyle.ListTable1Light:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfGridBuiltinStyle.ListTable1LightAccent1:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfGridBuiltinStyle.ListTable1LightAccent2:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfGridBuiltinStyle.ListTable1LightAccent3:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfGridBuiltinStyle.ListTable1LightAccent4:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfGridBuiltinStyle.ListTable1LightAccent5:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfGridBuiltinStyle.ListTable1LightAccent6:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfGridBuiltinStyle.ListTable2:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfGridBuiltinStyle.ListTable2Accent1:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfGridBuiltinStyle.ListTable2Accent2:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfGridBuiltinStyle.ListTable2Accent3:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfGridBuiltinStyle.ListTable2Accent4:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfGridBuiltinStyle.ListTable2Accent5:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfGridBuiltinStyle.ListTable2Accent6:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfGridBuiltinStyle.ListTable3:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfGridBuiltinStyle.ListTable3Accent1:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 91, 155, 213));
        break;
      case PdfGridBuiltinStyle.ListTable3Accent2:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 237, 125, 49));
        break;
      case PdfGridBuiltinStyle.ListTable3Accent3:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 165, 165, 165));
        break;
      case PdfGridBuiltinStyle.ListTable3Accent4:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0));
        break;
      case PdfGridBuiltinStyle.ListTable3Accent5:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 68, 114, 196));
        break;
      case PdfGridBuiltinStyle.ListTable3Accent6:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 171, 71));
        break;
      case PdfGridBuiltinStyle.ListTable4:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfGridBuiltinStyle.ListTable4Accent1:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 91, 155, 213), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfGridBuiltinStyle.ListTable4Accent2:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 237, 125, 49), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfGridBuiltinStyle.ListTable4Accent3:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 165, 165, 165), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfGridBuiltinStyle.ListTable4Accent4:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfGridBuiltinStyle.ListTable4Accent5:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 68, 114, 196), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfGridBuiltinStyle.ListTable4Accent6:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfGridBuiltinStyle.ListTable5Dark:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfGridBuiltinStyle.ListTable5DarkAccent1:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 91, 155, 213));
        break;
      case PdfGridBuiltinStyle.ListTable5DarkAccent2:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 237, 125, 49));
        break;
      case PdfGridBuiltinStyle.ListTable5DarkAccent3:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 165, 165, 165));
        break;
      case PdfGridBuiltinStyle.ListTable5DarkAccent4:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0));
        break;
      case PdfGridBuiltinStyle.ListTable5DarkAccent5:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 68, 114, 196));
        break;
      case PdfGridBuiltinStyle.ListTable5DarkAccent6:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71));
        break;
      case PdfGridBuiltinStyle.ListTable6Colorful:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfGridBuiltinStyle.ListTable6ColorfulAccent1:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 91, 155, 213), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 46, 116, 181));
        break;
      case PdfGridBuiltinStyle.ListTable6ColorfulAccent2:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 237, 125, 49), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 196, 89, 17));
        break;
      case PdfGridBuiltinStyle.ListTable6ColorfulAccent3:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 165, 165, 165), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 123, 123, 123));
        break;
      case PdfGridBuiltinStyle.ListTable6ColorfulAccent4:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, 191, 143, 0));
        break;
      case PdfGridBuiltinStyle.ListTable6ColorfulAccent5:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 68, 114, 196), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 47, 84, 150));
        break;
      case PdfGridBuiltinStyle.ListTable6ColorfulAccent6:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 83, 129, 53));
        break;
      case PdfGridBuiltinStyle.ListTable7Colorful:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfGridBuiltinStyle.ListTable7ColorfulAccent1:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 91, 155, 213), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 46, 116, 181));
        break;
      case PdfGridBuiltinStyle.ListTable7ColorfulAccent2:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 237, 125, 49), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 196, 89, 17));
        break;
      case PdfGridBuiltinStyle.ListTable7ColorfulAccent3:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 165, 165, 165), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 123, 123, 123));
        break;
      case PdfGridBuiltinStyle.ListTable7ColorfulAccent4:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, 191, 143, 0));
        break;
      case PdfGridBuiltinStyle.ListTable7ColorfulAccent5:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 68, 114, 196), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 47, 84, 150));
        break;
      case PdfGridBuiltinStyle.ListTable7ColorfulAccent6:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 83, 129, 53));
        break;
      case PdfGridBuiltinStyle.TableGridLight:
        this.ApplyTableGridLight(Color.FromArgb((int) byte.MaxValue, 191, 191, 191));
        break;
      case PdfGridBuiltinStyle.TableGrid:
        this.ApplyTableGridLight(Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
    }
  }

  public void ApplyBuiltinStyle(
    PdfGridBuiltinStyle gridStyle,
    PdfGridBuiltinStyleSettings gridStyleSetting)
  {
    this.m_headerRow = gridStyleSetting.ApplyStyleForHeaderRow;
    this.m_totalRow = gridStyleSetting.ApplyStyleForLastRow;
    this.m_firstColumn = gridStyleSetting.ApplyStyleForFirstColumn;
    this.m_lastColumn = gridStyleSetting.ApplyStyleForLastColumn;
    this.m_bandedColumn = gridStyleSetting.ApplyStyleForBandedColumns;
    this.m_bandedRow = gridStyleSetting.ApplyStyleForBandedRows;
    this.ApplyBuiltinStyle(gridStyle);
  }

  private PdfFont CreateBoldFont(PdfFont font)
  {
    PdfFont boldFont;
    if (font is PdfStandardFont)
    {
      boldFont = (PdfFont) new PdfStandardFont(font as PdfStandardFont, font.Size, PdfFontStyle.Bold);
    }
    else
    {
      PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
      boldFont = (PdfFont) new PdfTrueTypeFont(new Font(font.Name, font.Size, FontStyle.Bold), pdfTrueTypeFont.Unicode);
    }
    return boldFont;
  }

  private PdfFont CreateRegularFont(PdfFont font)
  {
    PdfFont regularFont;
    if (font is PdfStandardFont)
    {
      regularFont = (PdfFont) new PdfStandardFont(font as PdfStandardFont, font.Size, PdfFontStyle.Regular);
    }
    else
    {
      PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
      regularFont = (PdfFont) new PdfTrueTypeFont(new Font(font.Name, font.Size, FontStyle.Regular), pdfTrueTypeFont.Unicode);
    }
    return regularFont;
  }

  private PdfFont CreateItalicFont(PdfFont font)
  {
    PdfFont italicFont;
    if (font is PdfStandardFont)
    {
      italicFont = (PdfFont) new PdfStandardFont(font as PdfStandardFont, font.Size, PdfFontStyle.Italic);
    }
    else
    {
      PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
      italicFont = (PdfFont) new PdfTrueTypeFont(new Font(font.Name, font.Size, FontStyle.Italic), pdfTrueTypeFont.Unicode);
    }
    return italicFont;
  }

  private PdfFont ChangeFontStyle(PdfFont font)
  {
    PdfFont pdfFont = (PdfFont) null;
    if (font.Style == PdfFontStyle.Regular)
      pdfFont = this.CreateBoldFont(font);
    else if (font.Style == PdfFontStyle.Bold)
      pdfFont = this.CreateRegularFont(font);
    return pdfFont;
  }

  private PdfBrush ApplyBandedColStyle(bool firstColumn, Color backColor, int cellIndex)
  {
    PdfBrush pdfBrush = (PdfBrush) null;
    if (firstColumn)
    {
      if (cellIndex % 2 == 0)
        pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    }
    else if (cellIndex % 2 != 0)
      pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    return pdfBrush;
  }

  private PdfBrush ApplyBandedRowStyle(bool headerRow, Color backColor, int rowIndex)
  {
    PdfBrush pdfBrush = (PdfBrush) null;
    if (headerRow)
    {
      if (rowIndex % 2 != 0)
        pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    }
    else if (rowIndex % 2 == 0)
      pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    return pdfBrush;
  }

  private void ApplyTableGridLight(Color borderColor)
  {
    PdfPen pdfPen = new PdfPen(borderColor, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index1 = 1; index1 <= this.Headers.Count; ++index1)
      {
        PdfGridRow header = this.Headers[index1 - 1];
        for (int index2 = 1; index2 <= header.Cells.Count; ++index2)
          header.Cells[index2 - 1].Style.Borders.All = pdfPen;
      }
    }
    for (int index3 = 1; index3 <= this.Rows.Count; ++index3)
    {
      PdfGridRow row = this.Rows[index3 - 1];
      for (int index4 = 1; index4 <= row.Cells.Count; ++index4)
        row.Cells[index4 - 1].Style.Borders.All = pdfPen;
    }
  }

  private void ApplyPlainTable1(Color borderColor, Color backColor)
  {
    PdfPen pdfPen = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.All = pdfPen;
            if (this.m_bandedColumn)
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
              cell.Style.BackgroundBrush = (PdfBrush) null;
          }
          else
          {
            if (this.m_bandedRow && index % 2 != 0)
              cell.Style.BackgroundBrush = pdfBrush;
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              if (this.m_bandedRow && index % 2 != 0)
                cell.Style.BackgroundBrush = pdfBrush;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen;
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        else
        {
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.Top = new PdfPen(borderColor);
          if (this.m_bandedColumn && (!this.m_lastColumn || cellIndex != row.Cells.Count))
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
        }
      }
    }
  }

  private void ApplyPlainTable2(Color borderColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    if (this.Headers.Count > 0)
    {
      for (int index1 = 1; index1 <= this.Headers.Count; ++index1)
      {
        PdfGridRow header = this.Headers[index1 - 1];
        for (int index2 = 1; index2 <= header.Cells.Count; ++index2)
        {
          PdfGridCell cell = header.Cells[index2 - 1];
          cell.Style.Borders.All = pdfPen2;
          cell.Style.Borders.Top = pdfPen1;
          if (this.m_bandedColumn)
          {
            cell.Style.Borders.Left = pdfPen1;
            cell.Style.Borders.Right = pdfPen1;
          }
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.Bottom = pdfPen1;
            if (this.m_firstColumn && index2 == 1)
              cell.Style.Borders.Left = pdfPen2;
            if (this.m_lastColumn && index2 == header.Cells.Count)
              cell.Style.Borders.Right = pdfPen2;
          }
          else
          {
            if (this.m_bandedRow)
            {
              cell.Style.Borders.Top = pdfPen1;
              cell.Style.Borders.Bottom = pdfPen1;
            }
            if (this.m_firstColumn && index2 == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
              cell.Style.Borders.Left = pdfPen2;
            }
            if (this.m_lastColumn && index2 == header.Cells.Count)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
              cell.Style.Borders.Right = pdfPen2;
            }
          }
        }
      }
    }
    for (int index3 = 1; index3 <= this.Rows.Count; ++index3)
    {
      PdfGridRow row = this.Rows[index3 - 1];
      for (int index4 = 1; index4 <= row.Cells.Count; ++index4)
      {
        PdfGridCell cell = row.Cells[index4 - 1];
        cell.Style.Borders.All = pdfPen2;
        if (index3 == this.Rows.Count)
          cell.Style.Borders.Bottom = pdfPen1;
        if (this.m_bandedColumn)
        {
          cell.Style.Borders.Left = pdfPen1;
          cell.Style.Borders.Right = pdfPen1;
        }
        if (this.m_bandedRow)
        {
          cell.Style.Borders.Top = pdfPen1;
          cell.Style.Borders.Bottom = pdfPen1;
        }
        if (index3 == this.Rows.Count && this.m_totalRow)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.Top = pdfPen1;
          if (this.m_bandedColumn)
          {
            cell.Style.Borders.Left = pdfPen1;
            cell.Style.Borders.Right = pdfPen1;
          }
        }
        if (this.m_lastColumn && index4 == row.Cells.Count)
        {
          if (!this.m_totalRow || index3 != this.Rows.Count)
          {
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.Right = pdfPen2;
          }
          else if (this.m_bandedColumn)
            cell.Style.Borders.Right = pdfPen2;
        }
        if (this.m_firstColumn && index4 == 1)
        {
          if (!this.m_totalRow || index3 != this.Rows.Count)
          {
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.Left = pdfPen2;
          }
          else if (this.m_bandedColumn)
            cell.Style.Borders.Left = pdfPen2;
        }
      }
    }
  }

  private void ApplyPlainTable3(Color borderColor, Color backColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen3 = new PdfPen(backColor, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen2;
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            if (cell.Value is string)
            {
              string str = cell.Value as string;
              cell.Value = (object) str.ToUpper();
            }
            if (index == 1)
              cell.Style.Borders.Bottom = pdfPen1;
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
              cell.Style.BackgroundBrush = (PdfBrush) null;
          }
          else
          {
            if (this.m_bandedRow && index % 2 != 0)
            {
              cell.Style.BackgroundBrush = pdfBrush;
              if (this.m_firstColumn && cellIndex == 2)
                cell.Style.Borders.Left = pdfPen1;
              if (this.m_headerRow && index == 1)
                cell.Style.Borders.Top = pdfPen1;
            }
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
              if (cell.Value is string)
              {
                string str = cell.Value as string;
                cell.Value = (object) str.ToUpper();
              }
              cell.Style.Borders.Right = pdfPen1;
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
              cell.Style.Borders.All = pdfPen2;
              if (cell.Value is string)
              {
                string str = cell.Value as string;
                cell.Value = (object) str.ToUpper();
              }
              if (this.m_bandedColumn)
              {
                cell.Style.BackgroundBrush = (PdfBrush) null;
                if (this.m_bandedRow && index % 2 != 0)
                  cell.Style.BackgroundBrush = pdfBrush;
              }
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen2;
        if (this.m_bandedRow && this.m_bandedColumn)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          if (this.m_firstColumn && cellIndex == 2)
            cell.Style.Borders.Left = pdfPen1;
          if (this.m_headerRow && rowIndex == 1)
            cell.Style.Borders.Top = pdfPen1;
        }
        else
        {
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
            {
              if (this.m_firstColumn && cellIndex == 2)
                cell.Style.Borders.Left = pdfPen1;
              if (this.m_headerRow && rowIndex == 1)
                cell.Style.Borders.Top = pdfPen1;
            }
          }
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (this.m_firstColumn && cellIndex == 2)
              cell.Style.Borders.Left = pdfPen1;
            if (this.m_headerRow && rowIndex == 1)
              cell.Style.Borders.Top = pdfPen1;
          }
        }
        if (rowIndex == this.Rows.Count && this.m_totalRow)
        {
          if (this.m_bandedRow)
            cell.Style.Borders.All = pdfPen2;
          cell.Style.BackgroundBrush = (PdfBrush) null;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          if (cell.Value is string)
          {
            string str = cell.Value as string;
            cell.Value = (object) str.ToUpper();
          }
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null && this.m_firstColumn && cellIndex == 2)
              cell.Style.Borders.Left = pdfPen1;
          }
        }
        if (this.m_firstColumn && cellIndex == 1)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            if (cell.Value is string)
            {
              string str = cell.Value as string;
              cell.Value = (object) str.ToUpper();
            }
          }
          cell.Style.Borders.Right = pdfPen1;
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            if (cell.Value is string)
            {
              string str = cell.Value as string;
              cell.Value = (object) str.ToUpper();
            }
            cell.Style.BackgroundBrush = (PdfBrush) null;
            if (this.m_bandedRow)
              cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            cell.Style.Borders.All = pdfPen2;
          }
          else if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = (PdfBrush) null;
        }
        if (this.m_headerRow && rowIndex == 1)
          cell.Style.Borders.Top = pdfPen1;
      }
    }
  }

  private void ApplyPlainTable4(Color backColor)
  {
    PdfPen pdfPen1 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen2 = new PdfPen(backColor, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null)
                cell.Style.Borders.All = pdfPen2;
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.All = pdfPen1;
            }
          }
          else
          {
            if (this.m_bandedColumn)
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (this.m_bandedRow && index % 2 != 0)
              cell.Style.BackgroundBrush = pdfBrush;
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
              cell.Style.BackgroundBrush = (PdfBrush) null;
              if (this.m_bandedRow && index % 2 != 0)
                cell.Style.BackgroundBrush = pdfBrush;
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen1;
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        else
        {
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (cell.Style.BackgroundBrush != null)
              cell.Style.Borders.All = pdfPen2;
          }
        }
        if (rowIndex == this.Rows.Count && this.m_totalRow)
        {
          cell.Style.Borders.All = pdfPen1;
          cell.Style.BackgroundBrush = (PdfBrush) null;
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.BackgroundBrush = (PdfBrush) null;
            if (this.m_bandedRow)
              cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          }
          else if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = (PdfBrush) null;
        }
      }
    }
  }

  private void ApplyPlainTable5(Color borderColor, Color backColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen3 = new PdfPen(backColor, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen2;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            if (font.Style != PdfFontStyle.Italic)
              cell.Style.Font = this.CreateItalicFont(font);
            if (index == 1)
              cell.Style.Borders.Bottom = pdfPen1;
          }
          else
          {
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null)
              {
                if (this.m_firstColumn && cellIndex == 2)
                  cell.Style.Borders.Left = pdfPen1;
                else
                  cell.Style.Borders.All = pdfPen3;
              }
            }
            if (this.m_bandedRow && index % 2 != 0)
            {
              cell.Style.BackgroundBrush = pdfBrush;
              if (this.m_firstColumn && cellIndex == 2)
                cell.Style.Borders.Left = pdfPen1;
              else
                cell.Style.Borders.All = pdfPen3;
            }
            if (this.m_firstColumn && cellIndex == 1)
            {
              cell.Style.Borders.All = pdfPen2;
              cell.Style.BackgroundBrush = (PdfBrush) null;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              if (font.Style != PdfFontStyle.Italic)
                cell.Style.Font = this.CreateItalicFont(font);
              cell.Style.Borders.Right = pdfPen1;
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.CreateItalicFont(font);
              cell.Style.Borders.All = pdfPen2;
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.Left = pdfPen1;
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen2;
        if (this.m_bandedRow && this.m_bandedColumn)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          if (cell.Style.BackgroundBrush != null)
          {
            if (this.m_firstColumn && cellIndex == 2)
              cell.Style.Borders.Left = pdfPen1;
            else
              cell.Style.Borders.All = pdfPen3;
          }
        }
        else
        {
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
            {
              if (this.m_firstColumn && cellIndex == 2)
                cell.Style.Borders.Left = pdfPen1;
              else
                cell.Style.Borders.All = pdfPen3;
            }
          }
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (this.m_firstColumn && cellIndex == 2)
              cell.Style.Borders.Left = pdfPen1;
            else
              cell.Style.Borders.All = pdfPen3;
          }
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.Borders.All = new PdfPen(Color.Empty);
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.Top = pdfPen1;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          if (font.Style != PdfFontStyle.Italic)
            cell.Style.Font = this.CreateItalicFont(font);
        }
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.Borders.All = pdfPen2;
          cell.Style.BackgroundBrush = (PdfBrush) null;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          if (font.Style != PdfFontStyle.Italic)
            cell.Style.Font = this.CreateItalicFont(font);
          cell.Style.Borders.Right = pdfPen1;
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.CreateItalicFont(font);
          cell.Style.Borders.All = pdfPen2;
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.Left = pdfPen1;
        }
        if (this.m_headerRow && rowIndex == 1)
          cell.Style.Borders.Top = pdfPen1;
      }
    }
  }

  private void ApplyGridTable1Light(Color borderColor, Color headerBottomColor)
  {
    PdfPen pdfPen = new PdfPen(borderColor, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index1 = 1; index1 <= this.Headers.Count; ++index1)
      {
        PdfGridRow header = this.Headers[index1 - 1];
        for (int index2 = 1; index2 <= header.Cells.Count; ++index2)
        {
          PdfGridCell cell = header.Cells[index2 - 1];
          cell.Style.Borders.All = pdfPen;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
          }
          else
          {
            if (this.m_firstColumn && index2 == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && index2 == header.Cells.Count)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
        }
      }
    }
    for (int index3 = 1; index3 <= this.Rows.Count; ++index3)
    {
      PdfGridRow row = this.Rows[index3 - 1];
      for (int index4 = 1; index4 <= row.Cells.Count; ++index4)
      {
        PdfGridCell cell = row.Cells[index4 - 1];
        cell.Style.Borders.All = pdfPen;
        if (this.m_headerRow && index3 == 1)
          cell.Style.Borders.Top = new PdfPen(headerBottomColor);
        if (this.m_totalRow && index3 == this.Rows.Count)
        {
          cell.Style.Borders.Top = new PdfPen(headerBottomColor);
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_firstColumn && index4 == 1 && (!this.m_totalRow || index3 != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_lastColumn && index4 == row.Cells.Count && (!this.m_totalRow || index3 != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
      }
    }
  }

  private void ApplyGridTable2(Color borderColor, Color backColor)
  {
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.25f);
    PdfPen pdfPen2 = new PdfPen(backColor, 0.25f);
    PdfPen pdfPen3 = new PdfPen(Color.Empty);
    PdfPen pdfPen4 = new PdfPen(borderColor);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen1;
          if (cellIndex == 1)
            cell.Style.Borders.Left = pdfPen3;
          else if (cellIndex == header.Cells.Count)
            cell.Style.Borders.Right = pdfPen3;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.All = new PdfPen(Color.Empty);
            if ((double) cell.Row.Grid.Style.CellSpacing > 0.0)
              cell.Style.Borders.Bottom = pdfPen4;
          }
          else
          {
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null)
              {
                if (cellIndex == 1)
                  cell.Style.Borders.Left = pdfPen2;
                else if (header.Cells.Count % 2 != 0 && cellIndex == header.Cells.Count)
                  cell.Style.Borders.Right = pdfPen2;
              }
            }
            if (this.m_bandedRow)
            {
              if (index % 2 != 0)
                cell.Style.BackgroundBrush = pdfBrush;
              if (cell.Style.BackgroundBrush != null)
              {
                if (cellIndex == 1)
                  cell.Style.Borders.Left = pdfPen2;
                else if (cellIndex == header.Cells.Count)
                  cell.Style.Borders.Right = pdfPen2;
              }
            }
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              if (this.m_bandedRow)
              {
                if (index % 2 != 0)
                  cell.Style.BackgroundBrush = pdfBrush;
                if (cell.Style.BackgroundBrush != null)
                {
                  if (cellIndex == 1)
                    cell.Style.Borders.Left = pdfPen2;
                  else if (cellIndex == header.Cells.Count)
                    cell.Style.Borders.Right = pdfPen2;
                }
              }
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen1;
        if (cellIndex == 1)
          cell.Style.Borders.Left = pdfPen3;
        else if (cellIndex == row.Cells.Count)
          cell.Style.Borders.Right = pdfPen3;
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush != null)
          {
            if (cellIndex == 1)
              cell.Style.Borders.Left = pdfPen2;
            else if (cellIndex == row.Cells.Count)
              cell.Style.Borders.Right = pdfPen2;
          }
        }
        else
        {
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (cell.Style.BackgroundBrush != null)
            {
              if (cellIndex == 1)
                cell.Style.Borders.Left = pdfPen2;
              else if (cellIndex == row.Cells.Count)
                cell.Style.Borders.Right = pdfPen2;
            }
          }
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
            {
              if (cellIndex == 1)
                cell.Style.Borders.Left = pdfPen2;
              else if (row.Cells.Count % 2 != 0 && cellIndex == row.Cells.Count)
                cell.Style.Borders.Right = pdfPen2;
            }
          }
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.All = pdfPen3;
          cell.Style.Borders.Top = pdfPen4;
        }
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.BackgroundBrush = (PdfBrush) null;
            if (this.m_bandedRow)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
              if (cell.Style.BackgroundBrush == null)
                cell.Style.Borders.Right = pdfPen3;
            }
          }
          else if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = (PdfBrush) null;
        }
        if (this.m_headerRow && this.m_headers.Count > 0 && rowIndex == 1)
          cell.Style.Borders.Top = pdfPen4;
      }
    }
  }

  private void ApplyGridTable3(Color borderColor, Color backColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.All = pdfPen2;
          }
          else
          {
            if (this.m_bandedColumn)
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (this.m_bandedRow && index % 2 != 0)
              cell.Style.BackgroundBrush = pdfBrush;
            if (this.m_firstColumn && cellIndex == 1)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.All = pdfPen2;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.CreateItalicFont(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.All = pdfPen2;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.CreateItalicFont(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen1;
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        else
        {
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.All = new PdfPen(Color.Empty);
          cell.Style.BackgroundBrush = (PdfBrush) null;
        }
        if (this.m_firstColumn && cellIndex == 1)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            cell.Style.BackgroundBrush = (PdfBrush) null;
            cell.Style.Borders.All = pdfPen2;
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.CreateItalicFont(font);
            if (rowIndex == 1 && this.m_headerRow)
              cell.Style.Borders.Top = pdfPen1;
          }
          else
            cell.Style.Borders.Top = pdfPen1;
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            cell.Style.BackgroundBrush = (PdfBrush) null;
            cell.Style.Borders.All = pdfPen2;
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.CreateItalicFont(font);
            if (rowIndex == 1 && this.m_headerRow)
              cell.Style.Borders.Top = pdfPen1;
          }
          else
            cell.Style.Borders.Top = pdfPen1;
        }
      }
    }
  }

  private void ApplyGridTable4(Color borderColor, Color backColor, Color headerBackColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen2 = new PdfPen(headerBackColor, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.All = new PdfPen(headerBackColor);
            cell.Style.TextBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
            cell.Style.BackgroundBrush = (PdfBrush) new PdfSolidBrush((PdfColor) headerBackColor);
          }
          else
          {
            if (this.m_bandedColumn)
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (this.m_bandedRow && index % 2 != 0)
              cell.Style.BackgroundBrush = pdfBrush;
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              if (this.m_bandedColumn)
                cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen1;
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        else
        {
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.Top = new PdfPen(borderColor);
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            cell.Style.BackgroundBrush = (PdfBrush) null;
            if (this.m_bandedRow)
              cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
          }
          else if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = (PdfBrush) null;
        }
        if (this.m_headerRow && this.m_headers.Count > 0 && rowIndex == 1)
          cell.Style.Borders.Top = pdfPen2;
      }
    }
  }

  private void ApplyGridTable5Dark(
    Color headerBackColor,
    Color oddRowBackColor,
    Color evenRowBackColor)
  {
    PdfPen pdfPen1 = new PdfPen(oddRowBackColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(evenRowBackColor, 0.5f);
    PdfPen pdfPen3 = new PdfPen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) evenRowBackColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) oddRowBackColor);
    PdfBrush pdfBrush3 = (PdfBrush) new PdfSolidBrush((PdfColor) headerBackColor);
    PdfBrush pdfBrush4 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen3;
          cell.Style.BackgroundBrush = pdfBrush1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.TextBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
            cell.Style.BackgroundBrush = pdfBrush3;
            cell.Style.Borders.All = new PdfPen(Color.Empty, 0.5f);
            if (cellIndex == 1)
              cell.Style.Borders.Left = pdfPen3;
            else if (cellIndex == header.Cells.Count)
              cell.Style.Borders.Right = pdfPen3;
          }
          else
          {
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, oddRowBackColor, cellIndex);
              if (cell.Style.BackgroundBrush == null)
                cell.Style.BackgroundBrush = pdfBrush1;
            }
            if (this.m_bandedRow && index % 2 != 0)
              cell.Style.BackgroundBrush = pdfBrush2;
            if (this.m_firstColumn && cellIndex == 1 || this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = pdfBrush3;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
              cell.Style.TextBrush = pdfBrush4;
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen3;
        cell.Style.BackgroundBrush = pdfBrush1;
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, oddRowBackColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, oddRowBackColor, rowIndex);
            if (cell.Style.BackgroundBrush == null)
              cell.Style.BackgroundBrush = pdfBrush1;
          }
        }
        else
        {
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, oddRowBackColor, cellIndex);
            if (cell.Style.BackgroundBrush == null)
              cell.Style.BackgroundBrush = pdfBrush1;
          }
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, oddRowBackColor, rowIndex);
            if (cell.Style.BackgroundBrush == null)
              cell.Style.BackgroundBrush = pdfBrush1;
          }
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.TextBrush = pdfBrush4;
          cell.Style.BackgroundBrush = (PdfBrush) new PdfSolidBrush((PdfColor) headerBackColor);
          cell.Style.Borders.All = pdfPen3;
          if (cellIndex == 1)
            cell.Style.Borders.Left = pdfPen3;
          else if (cellIndex == row.Cells.Count)
            cell.Style.Borders.Right = pdfPen3;
        }
        if ((this.m_firstColumn && cellIndex == 1 || this.m_lastColumn && cellIndex == row.Cells.Count) && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.BackgroundBrush = pdfBrush3;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.TextBrush = pdfBrush4;
        }
      }
    }
  }

  private void ApplyGridTable6Colorful(Color borderColor, Color backColor, Color textColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen2 = new PdfPen(borderColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) textColor);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen1;
          cell.Style.TextBrush = pdfBrush2;
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
              cell.Style.BackgroundBrush = (PdfBrush) null;
          }
          else
          {
            if (this.m_bandedRow && index % 2 != 0)
              cell.Style.BackgroundBrush = pdfBrush1;
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              if (index % 2 != 0)
                cell.Style.BackgroundBrush = pdfBrush1;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen1;
        cell.Style.TextBrush = pdfBrush2;
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        else
        {
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          if (this.m_bandedColumn && (!this.m_lastColumn || cellIndex != row.Cells.Count))
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.Top = new PdfPen(borderColor);
        }
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_headerRow && rowIndex == 1)
          cell.Style.Borders.Top = pdfPen2;
      }
    }
  }

  private void ApplyGridTable7Colorful(Color borderColor, Color backColor, Color textColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen2 = new PdfPen(borderColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) textColor);
    PdfPen pdfPen3 = new PdfPen(Color.Empty, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.TextBrush = pdfBrush2;
          cell.Style.Borders.All = pdfPen1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.All = new PdfPen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
          }
          else
          {
            if (this.m_bandedColumn)
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (this.m_bandedRow && index % 2 != 0)
              cell.Style.BackgroundBrush = pdfBrush1;
            if (this.m_firstColumn && cellIndex == 1)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.All = pdfPen3;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.CreateItalicFont(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.All = pdfPen3;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.CreateItalicFont(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen1;
        cell.Style.TextBrush = pdfBrush2;
        if (this.m_bandedRow && this.m_bandedColumn)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        else
        {
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.All = new PdfPen(Color.Empty);
        }
        if (this.m_firstColumn && cellIndex == 1)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            cell.Style.BackgroundBrush = (PdfBrush) null;
            cell.Style.Borders.All = pdfPen3;
            if (rowIndex == 1 && this.m_headerRow)
              cell.Style.Borders.Top = pdfPen1;
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.CreateItalicFont(font);
          }
          else
            cell.Style.Borders.Top = pdfPen1;
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            cell.Style.BackgroundBrush = (PdfBrush) null;
            cell.Style.Borders.All = pdfPen3;
            cell.Style.Borders.Left = pdfPen1;
            if (rowIndex == 1 && this.m_headerRow)
              cell.Style.Borders.Top = pdfPen1;
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.CreateItalicFont(font);
          }
          else
            cell.Style.Borders.Top = pdfPen1;
        }
      }
    }
  }

  private void ApplyListTable1Light(Color borderColor, Color backColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen3 = new PdfPen(backColor, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen2;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            if (index == 1)
              cell.Style.Borders.Bottom = new PdfPen(borderColor);
            if (this.m_bandedColumn)
            {
              if (this.m_lastColumn && cellIndex == this.Rows.Count)
                cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null)
                cell.Style.Borders.All = pdfPen3;
              if (this.m_lastColumn && cellIndex == header.Cells.Count)
              {
                cell.Style.Borders.All = pdfPen2;
                cell.Style.BackgroundBrush = (PdfBrush) null;
              }
            }
          }
          else
          {
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null)
                cell.Style.Borders.All = pdfPen3;
            }
            if (this.m_bandedRow && index % 2 != 0)
            {
              cell.Style.BackgroundBrush = pdfBrush;
              cell.Style.Borders.All = pdfPen3;
            }
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.All = pdfPen2;
              if (this.m_bandedRow && index % 2 != 0)
                cell.Style.BackgroundBrush = pdfBrush;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen2;
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          if (cell.Style.BackgroundBrush != null)
            cell.Style.Borders.All = pdfPen3;
        }
        else
        {
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
              cell.Style.Borders.All = pdfPen3;
          }
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (cell.Style.BackgroundBrush != null)
              cell.Style.Borders.All = pdfPen3;
          }
        }
        if (this.m_firstColumn && cellIndex == 1)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
          }
          else
            cell.Style.Borders.Top = pdfPen1;
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.All = pdfPen2;
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.All = pdfPen2;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          if (this.m_bandedColumn)
          {
            if (!this.m_lastColumn || cellIndex != row.Cells.Count)
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
              cell.Style.Borders.All = pdfPen3;
          }
          cell.Style.Borders.Top = new PdfPen(borderColor);
        }
        if (this.m_headerRow && rowIndex == 1)
          cell.Style.Borders.Top = pdfPen1;
      }
    }
  }

  private void ApplyListTable2(Color borderColor, Color backColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen2 = new PdfPen(backColor, 0.5f);
    PdfPen pdfPen3 = new PdfPen(Color.Empty, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen3;
          cell.Style.Borders.Bottom = pdfPen1;
          cell.Style.Borders.Top = pdfPen1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (this.m_lastColumn && cellIndex == header.Cells.Count)
                cell.Style.BackgroundBrush = (PdfBrush) null;
              if (cell.Style.BackgroundBrush != null)
              {
                cell.Style.Borders.Right = pdfPen2;
                cell.Style.Borders.Left = pdfPen2;
              }
            }
          }
          else
          {
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null)
              {
                cell.Style.Borders.Right = pdfPen2;
                cell.Style.Borders.Left = pdfPen2;
              }
            }
            if (this.m_bandedRow && index % 2 != 0)
            {
              cell.Style.Borders.Left = pdfPen2;
              cell.Style.Borders.Right = pdfPen2;
              cell.Style.BackgroundBrush = pdfBrush;
            }
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.Left = pdfPen3;
              cell.Style.Borders.Right = pdfPen3;
              if (this.m_bandedRow && index % 2 != 0)
              {
                cell.Style.BackgroundBrush = pdfBrush;
                if (cell.Style.BackgroundBrush != null)
                {
                  cell.Style.Borders.Left = pdfPen2;
                  cell.Style.Borders.Right = pdfPen2;
                }
              }
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen3;
        cell.Style.Borders.Bottom = pdfPen1;
        cell.Style.Borders.Top = pdfPen1;
        if (this.m_bandedRow && this.m_bandedColumn)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          if (cell.Style.BackgroundBrush != null)
          {
            cell.Style.Borders.Right = pdfPen2;
            cell.Style.Borders.Left = pdfPen2;
          }
        }
        else
        {
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
            {
              cell.Style.Borders.Right = pdfPen2;
              cell.Style.Borders.Left = pdfPen2;
            }
          }
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (cell.Style.BackgroundBrush != null)
            {
              cell.Style.Borders.Left = pdfPen2;
              cell.Style.Borders.Right = pdfPen2;
            }
          }
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.All = pdfPen3;
          cell.Style.Borders.Top = pdfPen1;
          cell.Style.Borders.Bottom = pdfPen1;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          if (this.m_bandedColumn)
          {
            if (cellIndex != row.Cells.Count || !this.m_lastColumn)
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
            {
              cell.Style.Borders.Right = pdfPen2;
              cell.Style.Borders.Left = pdfPen2;
            }
          }
        }
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.Left = pdfPen3;
          cell.Style.Borders.Right = pdfPen3;
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (cell.Style.BackgroundBrush != null)
              cell.Style.Borders.Right = pdfPen2;
          }
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
      }
    }
  }

  private void ApplyListTable3(Color backColor)
  {
    PdfPen pdfPen1 = new PdfPen(backColor, 0.5f);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen2 = new PdfPen(Color.Empty, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index1 = 1; index1 <= this.Headers.Count; ++index1)
      {
        PdfGridRow header = this.Headers[index1 - 1];
        for (int index2 = 1; index2 <= header.Cells.Count; ++index2)
        {
          PdfGridCell cell = header.Cells[index2 - 1];
          cell.Style.Borders.All = pdfPen2;
          cell.Style.Borders.Top = pdfPen1;
          if (index2 == 1)
            cell.Style.Borders.Left = pdfPen1;
          else if (index2 == header.Cells.Count)
            cell.Style.Borders.Right = pdfPen1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.All = pdfPen1;
            cell.Style.BackgroundBrush = pdfBrush;
            cell.Style.TextBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
          }
          else
          {
            if (this.m_firstColumn && index2 == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && index2 == header.Cells.Count)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
          if (this.m_bandedColumn)
            cell.Style.Borders.Left = pdfPen1;
        }
      }
    }
    for (int index3 = 1; index3 <= this.Rows.Count; ++index3)
    {
      PdfGridRow row = this.Rows[index3 - 1];
      for (int index4 = 1; index4 <= row.Cells.Count; ++index4)
      {
        PdfGridCell cell = row.Cells[index4 - 1];
        cell.Style.Borders.All = pdfPen2;
        if (this.Headers.Count == 0 && index3 == 1)
          cell.Style.Borders.Top = pdfPen1;
        if (index4 == 1)
          cell.Style.Borders.Left = pdfPen1;
        else if (index4 == row.Cells.Count)
          cell.Style.Borders.Right = pdfPen1;
        if (index3 == this.Rows.Count)
          cell.Style.Borders.Bottom = pdfPen1;
        if (this.m_bandedColumn)
          cell.Style.Borders.Left = pdfPen1;
        if (this.m_bandedRow)
          cell.Style.Borders.Top = pdfPen1;
        if (this.m_totalRow && index3 == this.Rows.Count)
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.Top = new PdfPen(backColor);
        }
        if (this.m_firstColumn && index4 == 1 && (!this.m_totalRow || index3 != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_lastColumn && index4 == row.Cells.Count && (!this.m_totalRow || index3 != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
      }
    }
  }

  private void ApplyListTable4(Color borderColor, Color headerBackColor, Color bandRowColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) headerBackColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) bandRowColor);
    PdfPen pdfPen2 = new PdfPen(bandRowColor, 0.5f);
    PdfPen pdfPen3 = new PdfPen(Color.Empty, 0.5f);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen3;
          cell.Style.Borders.Top = pdfPen1;
          if (cellIndex == 1)
            cell.Style.Borders.Left = pdfPen1;
          else if (cellIndex == header.Cells.Count)
            cell.Style.Borders.Right = pdfPen1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.All = new PdfPen(headerBackColor, 0.5f);
            cell.Style.BackgroundBrush = pdfBrush1;
            cell.Style.TextBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
          }
          else
          {
            if (this.m_bandedColumn)
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, bandRowColor, cellIndex);
            if (this.m_bandedRow && index % 2 != 0)
              cell.Style.BackgroundBrush = pdfBrush2;
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              if (this.m_bandedRow && index % 2 != 0)
                cell.Style.BackgroundBrush = pdfBrush2;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen3;
        cell.Style.Borders.Top = pdfPen1;
        if (cellIndex == 1)
          cell.Style.Borders.Left = pdfPen1;
        else if (cellIndex == row.Cells.Count)
          cell.Style.Borders.Right = pdfPen1;
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, bandRowColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, bandRowColor, rowIndex);
        }
        else
        {
          if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, bandRowColor, cellIndex);
          if (this.m_bandedRow)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, bandRowColor, rowIndex);
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.Top = new PdfPen(borderColor);
          if (this.m_bandedColumn && (!this.m_lastColumn || cellIndex != this.Rows.Count))
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, bandRowColor, cellIndex);
        }
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, bandRowColor, rowIndex);
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (rowIndex == this.Rows.Count)
          cell.Style.Borders.Bottom = pdfPen1;
      }
    }
  }

  private void ApplyListTable5Dark(Color backColor)
  {
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen1 = new PdfPen(backColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 0.5f);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    PdfPen pdfPen3 = new PdfPen(Color.Empty);
    if (this.Headers.Count > 0)
    {
      for (int index1 = 1; index1 <= this.Headers.Count; ++index1)
      {
        PdfGridRow header = this.Headers[index1 - 1];
        for (int index2 = 1; index2 <= header.Cells.Count; ++index2)
        {
          PdfGridCell cell = header.Cells[index2 - 1];
          cell.Style.Borders.All = pdfPen3;
          cell.Style.TextBrush = pdfBrush2;
          cell.Style.BackgroundBrush = pdfBrush1;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.BackgroundBrush = pdfBrush1;
            cell.Style.TextBrush = pdfBrush2;
            cell.Style.Borders.Bottom = new PdfPen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 2f);
            if (this.m_bandedColumn && index2 > 1)
              cell.Style.Borders.Left = pdfPen2;
          }
          else
          {
            if (this.m_firstColumn)
            {
              switch (index2)
              {
                case 1:
                  PdfFont font1 = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
                  cell.Style.Font = this.ChangeFontStyle(font1);
                  break;
                case 2:
                  cell.Style.Borders.Left = pdfPen2;
                  break;
              }
            }
            if (this.m_bandedColumn && index2 > 1)
              cell.Style.Borders.Left = pdfPen2;
            if (this.m_bandedRow)
              cell.Style.Borders.Top = pdfPen2;
            if (this.m_lastColumn && index2 == header.Cells.Count)
            {
              PdfFont font2 = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font2);
              cell.Style.Borders.Left = pdfPen2;
            }
          }
        }
      }
    }
    for (int index3 = 1; index3 <= this.Rows.Count; ++index3)
    {
      PdfGridRow row = this.Rows[index3 - 1];
      for (int index4 = 1; index4 <= row.Cells.Count; ++index4)
      {
        PdfGridCell cell = row.Cells[index4 - 1];
        cell.Style.Borders.All = pdfPen3;
        cell.Style.TextBrush = pdfBrush2;
        cell.Style.BackgroundBrush = pdfBrush1;
        if (this.m_firstColumn && (!this.m_totalRow || index3 != this.Rows.Count))
        {
          switch (index4)
          {
            case 1:
              PdfFont font3 = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font3);
              break;
            case 2:
              cell.Style.Borders.Left = pdfPen2;
              break;
          }
        }
        if (this.m_bandedColumn && index4 > 1)
          cell.Style.Borders.Left = pdfPen2;
        if (this.m_bandedRow)
          cell.Style.Borders.Top = pdfPen2;
        if (this.m_totalRow && index3 == this.Rows.Count)
        {
          PdfFont font4 = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font4);
          cell.Style.Borders.Top = pdfPen2;
          if (this.m_headerRow)
          {
            if (this.m_firstColumn && index4 == 1)
              cell.Style.Borders.Top = pdfPen3;
            if (this.m_lastColumn && index4 == row.Cells.Count)
              cell.Style.Borders.Top = pdfPen3;
          }
        }
        if (this.m_lastColumn && index4 == row.Cells.Count && (!this.m_totalRow || index3 != this.Rows.Count))
        {
          PdfFont font5 = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font5);
          cell.Style.Borders.Left = pdfPen2;
        }
      }
    }
  }

  private void ApplyListTable6Colorful(Color borderColor, Color backColor, Color textColor)
  {
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(backColor, 0.5f);
    PdfPen pdfPen3 = new PdfPen(Color.Empty, 0.5f);
    PdfSolidBrush pdfSolidBrush = new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) textColor);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen3;
          cell.Style.Borders.Top = pdfPen1;
          cell.Style.TextBrush = pdfBrush2;
          if (this.m_headerRow)
          {
            if (this.m_bandedColumn)
            {
              if (!this.m_lastColumn || cellIndex != header.Cells.Count)
                cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null)
              {
                cell.Style.Borders.Left = pdfPen2;
                cell.Style.Borders.Right = pdfPen2;
              }
            }
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            cell.Style.Borders.Bottom = pdfPen1;
          }
          else
          {
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null && index == 1 && this.m_headerRow)
                cell.Style.Borders.Top = pdfPen1;
            }
            if (this.m_bandedRow && index % 2 != 0)
            {
              cell.Style.BackgroundBrush = pdfBrush1;
              if (cellIndex == 1)
                cell.Style.Borders.Left = pdfPen2;
              else if (cellIndex == header.Cells.Count)
                cell.Style.Borders.Right = pdfPen2;
              if (index == 1)
                cell.Style.Borders.Top = pdfPen1;
            }
            if (this.m_firstColumn && cellIndex == 1)
            {
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.All = pdfPen3;
              cell.Style.Borders.Top = pdfPen1;
              if (this.m_bandedRow && index % 2 != 0)
                cell.Style.BackgroundBrush = pdfBrush1;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.ChangeFontStyle(font);
              cell.Style.Borders.Left = pdfPen3;
            }
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.TextBrush = pdfBrush2;
        cell.Style.Borders.All = pdfPen3;
        if (this.Headers.Count == 0 && rowIndex == 1)
          cell.Style.Borders.Top = pdfPen1;
        if (rowIndex == this.Rows.Count)
          cell.Style.Borders.Bottom = pdfPen1;
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          if (cell.Style.BackgroundBrush != null)
          {
            if (cellIndex == 1)
              cell.Style.Borders.Left = pdfPen2;
            else if (cellIndex == row.Cells.Count)
              cell.Style.Borders.Right = pdfPen2;
          }
          if (rowIndex == 1 && this.m_headerRow)
            cell.Style.Borders.Top = pdfPen1;
        }
        else
        {
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
            {
              cell.Style.Borders.Left = pdfPen2;
              cell.Style.Borders.Right = pdfPen2;
              if (rowIndex == 1 && this.m_headerRow)
                cell.Style.Borders.Top = pdfPen1;
            }
          }
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (cellIndex == 1)
              cell.Style.Borders.Left = pdfPen2;
            else if (cellIndex == row.Cells.Count)
              cell.Style.Borders.Right = pdfPen2;
            if (rowIndex == 1 && this.m_headerRow)
              cell.Style.Borders.Top = pdfPen1;
          }
        }
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count)
        {
          if (!this.m_totalRow || rowIndex != this.Rows.Count)
          {
            cell.Style.BackgroundBrush = (PdfBrush) null;
            cell.Style.Borders.Left = pdfPen3;
            cell.Style.Borders.Right = pdfPen3;
            if (this.m_bandedRow)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
              if (cell.Style.BackgroundBrush != null)
                cell.Style.Borders.Right = pdfPen2;
            }
            PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
            cell.Style.Font = this.ChangeFontStyle(font);
            if (rowIndex == 1 && this.m_headerRow)
              cell.Style.Borders.Top = pdfPen1;
          }
          else if (this.m_bandedColumn)
            cell.Style.BackgroundBrush = (PdfBrush) null;
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.Left = pdfPen3;
          cell.Style.Borders.Right = pdfPen3;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.ChangeFontStyle(font);
          cell.Style.Borders.Top = new PdfPen(borderColor);
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (this.m_lastColumn && cellIndex == row.Cells.Count)
              cell.Style.BackgroundBrush = (PdfBrush) null;
            if (cell.Style.BackgroundBrush != null)
            {
              cell.Style.Borders.Left = pdfPen2;
              cell.Style.Borders.Right = pdfPen2;
            }
          }
        }
      }
    }
  }

  private void ApplyListTable7Colorful(Color borderColor, Color backColor, Color textColor)
  {
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen3 = new PdfPen(backColor, 0.5f);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) textColor);
    PdfPen pdfPen4 = new PdfPen(borderColor);
    if (this.Headers.Count > 0)
    {
      for (int index = 1; index <= this.Headers.Count; ++index)
      {
        PdfGridRow header = this.Headers[index - 1];
        for (int cellIndex = 1; cellIndex <= header.Cells.Count; ++cellIndex)
        {
          PdfGridCell cell = header.Cells[cellIndex - 1];
          cell.Style.Borders.All = pdfPen2;
          cell.Style.TextBrush = pdfBrush2;
          if (this.m_headerRow)
          {
            PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
            if (font.Style != PdfFontStyle.Italic)
              cell.Style.Font = this.CreateItalicFont(font);
            if (index == 1)
              cell.Style.Borders.Bottom = pdfPen1;
          }
          else
          {
            if (this.m_bandedColumn)
            {
              cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
              if (cell.Style.BackgroundBrush != null)
                cell.Style.Borders.All = pdfPen3;
            }
            if (this.m_bandedRow && index % 2 != 0)
            {
              cell.Style.BackgroundBrush = pdfBrush1;
              cell.Style.Borders.All = pdfPen3;
            }
            if (this.m_firstColumn && cellIndex == 1)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              cell.Style.Borders.All = pdfPen2;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.CreateItalicFont(font);
              cell.Style.Borders.Right = pdfPen1;
            }
            if (this.m_lastColumn && cellIndex == header.Cells.Count)
            {
              cell.Style.BackgroundBrush = (PdfBrush) null;
              PdfFont font = cell.Style.Font ?? header.Style.Font ?? header.Grid.Style.Font ?? PdfDocument.DefaultFont;
              cell.Style.Font = this.CreateItalicFont(font);
              cell.Style.Borders.All = pdfPen2;
              cell.Style.Borders.Left = pdfPen1;
            }
            if (this.m_firstColumn && cellIndex == 2)
              cell.Style.Borders.Left = pdfPen1;
          }
        }
      }
    }
    for (int rowIndex = 1; rowIndex <= this.Rows.Count; ++rowIndex)
    {
      PdfGridRow row = this.Rows[rowIndex - 1];
      for (int cellIndex = 1; cellIndex <= row.Cells.Count; ++cellIndex)
      {
        PdfGridCell cell = row.Cells[cellIndex - 1];
        cell.Style.Borders.All = pdfPen2;
        cell.Style.TextBrush = pdfBrush2;
        if (this.m_bandedColumn && this.m_bandedRow)
        {
          cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
          if (cell.Style.BackgroundBrush == null)
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
          if (cell.Style.BackgroundBrush != null)
            cell.Style.Borders.All = pdfPen3;
        }
        else
        {
          if (this.m_bandedColumn)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedColStyle(this.m_firstColumn, backColor, cellIndex);
            if (cell.Style.BackgroundBrush != null)
              cell.Style.Borders.All = pdfPen3;
          }
          if (this.m_bandedRow)
          {
            cell.Style.BackgroundBrush = this.ApplyBandedRowStyle(this.m_headerRow, backColor, rowIndex);
            if (cell.Style.BackgroundBrush != null)
              cell.Style.Borders.All = pdfPen3;
          }
        }
        if (this.m_firstColumn && cellIndex == 1 && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          cell.Style.Borders.All = pdfPen2;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.CreateItalicFont(font);
          cell.Style.Borders.Right = pdfPen1;
        }
        if (this.m_firstColumn && cellIndex == 2)
        {
          cell.Style.Borders.All = pdfPen2;
          cell.Style.Borders.Left = pdfPen1;
        }
        if (this.m_totalRow && rowIndex == this.Rows.Count)
        {
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          if (font.Style != PdfFontStyle.Italic)
            cell.Style.Font = this.CreateItalicFont(font);
          cell.Style.Borders.All = pdfPen2;
          cell.Style.Borders.Top = pdfPen1;
          cell.Style.BackgroundBrush = (PdfBrush) null;
        }
        if (this.m_lastColumn && cellIndex == row.Cells.Count && (!this.m_totalRow || rowIndex != this.Rows.Count))
        {
          cell.Style.BackgroundBrush = (PdfBrush) null;
          PdfFont font = cell.Style.Font ?? row.Style.Font ?? row.Grid.Style.Font ?? PdfDocument.DefaultFont;
          cell.Style.Font = this.CreateItalicFont(font);
          cell.Style.Borders.All = pdfPen2;
          cell.Style.Borders.Left = pdfPen1;
        }
        if (this.m_headerRow && rowIndex == 1)
          cell.Style.Borders.Top = pdfPen1;
      }
    }
  }

  private void SetDataSource() => this.PopulateDataGrid();

  private void PopulateDataGrid()
  {
    Array dataSource1 = this.m_dataSource as Array;
    DataSet dataSource2 = this.m_dataSource as DataSet;
    DataColumn dataSource3 = this.m_dataSource as DataColumn;
    DataTable dataSource4 = this.m_dataSource as DataTable;
    DataView dataSource5 = this.m_dataSource as DataView;
    PdfDataSource pdfDataSource = (PdfDataSource) null;
    if (dataSource1 != null)
    {
      pdfDataSource = new PdfDataSource(dataSource1);
      if (dataSource1 != null && dataSource1.Length > 0)
      {
        int index = 0;
        string[] row = pdfDataSource.GetRow(ref index);
        if (dataSource1.GetType().FullName.TrimEnd(']').TrimEnd('[') == row[0])
          pdfDataSource = (PdfDataSource) null;
      }
    }
    else if (dataSource3 != null)
      pdfDataSource = new PdfDataSource(dataSource3);
    else if (dataSource4 != null)
      pdfDataSource = new PdfDataSource(dataSource4);
    else if (dataSource5 != null)
      pdfDataSource = new PdfDataSource(dataSource5);
    else if (dataSource2 != null)
      pdfDataSource = new PdfDataSource(dataSource2, this.m_dataMember);
    this.m_dsParser = pdfDataSource;
    if (pdfDataSource == null)
    {
      this.PopulateIEnumerableGrid();
    }
    else
    {
      this.PopulateHeader();
      this.PopulateGrid();
    }
  }

  private void PopulateIEnumerableGrid()
  {
    if (!(this.m_dataSource is IEnumerable))
      return;
    PropertyInfo[] propertyInfoArray = (PropertyInfo[]) null;
    foreach (object obj in this.m_dataSource as IEnumerable)
    {
      if (obj != null)
      {
        propertyInfoArray = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        this.Columns.Add(propertyInfoArray.Length);
        PdfGridRow row = new PdfGridRow(this);
        foreach (PropertyInfo propertyInfo in propertyInfoArray)
          row.Cells.Add(new PdfGridCell()
          {
            Value = (object) propertyInfo.Name
          });
        this.Headers.Add(row);
        break;
      }
    }
    foreach (object obj in this.m_dataSource as IEnumerable)
    {
      PdfGridRow row = new PdfGridRow(this);
      foreach (PropertyInfo propertyInfo in propertyInfoArray)
      {
        PropertyInfo property = obj.GetType().GetProperty(propertyInfo.Name);
        row.Cells.Add(new PdfGridCell(row)
        {
          Value = (object) Convert.ToString(property.GetValue(obj, (object[]) null))
        });
      }
      this.Rows.Add(row);
    }
  }

  private void PopulateGrid()
  {
    if (this.m_dsParser != null)
    {
      int index1 = 0;
      this.Rows.Clear();
      while (index1 < this.m_dsParser.RowCount)
      {
        PdfGridRow row1 = new PdfGridRow(this);
        string[] row2 = this.m_dsParser.GetRow(ref index1);
        for (int index2 = 0; index2 < this.m_dsParser.ColumnCount; ++index2)
          row1.Cells.Add(new PdfGridCell(row1)
          {
            Value = (object) row2[index2]
          });
        this.Rows.Add(row1);
      }
    }
    for (int index = 0; index < this.m_dsParser.ColumnCount; ++index)
      this.Columns.Add(new PdfGridColumn(this));
  }

  private void PopulateHeader()
  {
    this.Headers.Clear();
    string[] columnCaptions = this.m_dsParser.ColumnCaptions;
    if (columnCaptions == null)
      return;
    PdfGridRow row = new PdfGridRow(this);
    for (int index = 0; index < this.m_dsParser.ColumnCount; ++index)
      row.Cells.Add(new PdfGridCell(row)
      {
        Value = (object) columnCaptions[index]
      });
    this.Headers.Add(row);
  }

  internal void MeasureColumnsWidth()
  {
    float[] numArray = new float[this.Columns.Count];
    float val1_1 = 0.0f;
    if (this.Headers.Count > 0)
    {
      int index1 = 0;
      for (int count1 = this.Headers[0].Cells.Count; index1 < count1; ++index1)
      {
        int index2 = 0;
        for (int count2 = this.Headers.Count; index2 < count2; ++index2)
        {
          float val2 = (double) this.InitialWidth > 0.0 ? Math.Min(this.InitialWidth, this.Headers[index2].Cells[index1].Width) : this.Headers[index2].Cells[index1].Width;
          val1_1 = Math.Max(val1_1, val2);
        }
        numArray[index1] = val1_1;
      }
    }
    float val1_2 = 0.0f;
    int index3 = 0;
    for (int count3 = this.Columns.Count; index3 < count3; ++index3)
    {
      int index4 = 0;
      for (int count4 = this.Rows.Count; index4 < count4; ++index4)
      {
        if (this.Rows[index4].Cells[index3].ColumnSpan == 1 && !this.Rows[index4].Cells[index3].IsCellMergeContinue || this.Rows[index4].Cells[index3].Value is PdfGrid)
        {
          if (this.Rows[index4].Cells[index3].Value is PdfGrid && !this.Rows[index4].Grid.Style.AllowHorizontalOverflow)
          {
            float num = (float) ((double) this.Rows[index4].Grid.Style.CellPadding.Right + (double) this.Rows[index4].Grid.Style.CellPadding.Left + (double) this.Rows[index4].Cells[index3].Style.Borders.Left.Width / 2.0) + this.m_gridLocation.X;
            if ((double) this.InitialWidth != 0.0)
              (this.Rows[index4].Cells[index3].Value as PdfGrid).InitialWidth = this.InitialWidth - num;
          }
          float val2_1 = (double) this.InitialWidth > 0.0 ? Math.Min(this.InitialWidth, this.Rows[index4].Cells[index3].Width) : this.Rows[index4].Cells[index3].Width;
          float val2_2 = Math.Max(numArray[index3], Math.Max(val1_2, val2_1));
          val1_2 = Math.Max(this.Columns[index3].Width, val2_2);
        }
      }
      if (this.Rows.Count != 0)
        numArray[index3] = val1_2;
      val1_2 = 0.0f;
    }
    int index5 = 0;
    for (int count5 = this.Rows.Count; index5 < count5; ++index5)
    {
      int index6 = 0;
      for (int count6 = this.Columns.Count; index6 < count6; ++index6)
      {
        if (this.Rows[index5].Cells[index6].ColumnSpan > 1)
        {
          float num1 = numArray[index6];
          for (int index7 = 1; index7 < this.Rows[index5].Cells[index6].ColumnSpan; ++index7)
            num1 += numArray[index6 + index7];
          if ((double) this.Rows[index5].Cells[index6].Width > (double) num1)
          {
            float num2 = (this.Rows[index5].Cells[index6].Width - num1) / (float) this.Rows[index5].Cells[index6].ColumnSpan;
            for (int index8 = index6; index8 < index6 + this.Rows[index5].Cells[index6].ColumnSpan; ++index8)
              numArray[index8] += num2;
          }
        }
      }
    }
    if (this.IsChildGrid && (double) this.InitialWidth != 0.0)
      numArray = this.Columns.GetDefaultWidths(this.InitialWidth);
    int index9 = 0;
    for (int count = this.Columns.Count; index9 < count; ++index9)
    {
      if ((double) this.Columns[index9].Width < 0.0)
        this.Columns[index9].m_width = numArray[index9];
      else if ((double) this.Columns[index9].Width > 0.0 & !this.Columns[index9].isCustomWidth)
        this.Columns[index9].m_width = numArray[index9];
    }
  }

  internal void MeasureColumnsWidth(RectangleF bounds)
  {
    float[] defaultWidths1 = this.Columns.GetDefaultWidths(bounds.Width - bounds.X);
    int index1 = 0;
    for (int count = this.Columns.Count; index1 < count; ++index1)
    {
      if ((double) this.Columns[index1].Width < 0.0)
        this.Columns[index1].m_width = defaultWidths1[index1];
      else if ((double) this.Columns[index1].Width > 0.0 && !this.Columns[index1].isCustomWidth && (double) defaultWidths1[index1] > 0.0 && this.isComplete)
        this.Columns[index1].m_width = defaultWidths1[index1];
    }
    if (this.ParentCell != null && !this.Style.AllowHorizontalOverflow && !this.ParentCell.Row.Grid.Style.AllowHorizontalOverflow)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      int count = this.Columns.Count;
      if (this.ParentCell.Style.CellPadding != null)
        num1 += this.ParentCell.Style.CellPadding.Left + this.ParentCell.Style.CellPadding.Right;
      else if (this.ParentCell.Row.Grid.Style.CellPadding != null)
        num1 += this.ParentCell.Row.Grid.Style.CellPadding.Left + this.ParentCell.Row.Grid.Style.CellPadding.Right;
      for (int index2 = 0; index2 < this.ParentCell.ColumnSpan; ++index2)
        num2 += this.ParentCell.Row.Grid.Columns[this.parentCellIndex + index2].Width;
      for (int index3 = 0; index3 < this.Columns.Count; ++index3)
      {
        if ((double) this.m_columns[index3].Width > 0.0 && this.m_columns[index3].isCustomWidth)
        {
          num2 -= this.m_columns[index3].Width;
          --count;
        }
      }
      if ((double) num2 > (double) num1)
      {
        float num3 = (num2 - num1) / (float) count;
        if (this.ParentCell != null && this.ParentCell.StringFormat.Alignment != PdfTextAlignment.Right)
        {
          for (int index4 = 0; index4 < this.Columns.Count; ++index4)
          {
            if (!this.Columns[index4].isCustomWidth)
              this.Columns[index4].m_width = (double) num3 <= (double) bounds.Width || !this.IsChildGrid || this.Style == null || this.Style.CellPadding == null ? num3 : bounds.Width;
          }
        }
      }
    }
    if (this.ParentCell == null || (double) this.ParentCell.Row.Width <= 0.0 || !this.IsChildGrid || (double) this.m_size.Width <= (double) this.ParentCell.Row.Width)
      return;
    float[] defaultWidths2 = this.Columns.GetDefaultWidths(bounds.Width);
    for (int index5 = 0; index5 < this.Columns.Count; ++index5)
      this.Columns[index5].Width = defaultWidths2[index5];
  }
}
