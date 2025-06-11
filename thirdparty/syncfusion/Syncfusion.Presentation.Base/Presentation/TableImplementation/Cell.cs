// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.Cell
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Layouting;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class Cell : ICell, ICellBorders
{
  private const int ColumnBitsInCellIndex = 32 /*0x20*/;
  private Dictionary<BorderType, LineFormat> _borders;
  private BorderType _borderType;
  private int _colSpan;
  private Syncfusion.Presentation.Drawing.Fill _fillFormat;
  private bool _hMerge;
  private string _id;
  private Row _row;
  private Column _column;
  private int _rowSpan;
  private Syncfusion.Presentation.RichText.TextBody _textFrame;
  private bool _vMerge;
  private long _cellIndex;
  private bool _isInsertedCell;
  private IFont _prevCellTextPartFont;
  private CellInfo _cellInfo;
  private int _rowIndex;
  private int _columnIndex;
  private bool _isFillSet;
  internal bool HasDiagonalDownBorder;
  internal bool HasDiagonalUpBorder;

  internal Cell(Row row)
  {
    this._row = row;
    this._fillFormat = new Syncfusion.Presentation.Drawing.Fill(this);
  }

  internal IFont PrevCellTextPartFont
  {
    get => this._prevCellTextPartFont;
    set => this._prevCellTextPartFont = value;
  }

  internal bool IsInsertedCell
  {
    get => this._isInsertedCell;
    set => this._isInsertedCell = value;
  }

  public ICellBorders CellBorders => (ICellBorders) this;

  public double ColumnWidth
  {
    get => this._row.Table.Columns[this.ColumnIndex - 1].Width;
    set => this._row.Table.Columns[this.ColumnIndex - 1].Width = value;
  }

  public ILineFormat BorderBottom
  {
    get
    {
      if (this._borders == null)
        this._borders = new Dictionary<BorderType, LineFormat>(6);
      this._borderType |= BorderType.Bottom;
      return (ILineFormat) this.GetBorderLine(BorderType.Bottom);
    }
  }

  internal ILineFormat GetDefaultBottomBorder()
  {
    if (!Helper.HasFlag(this._borderType, BorderType.Bottom))
    {
      ILineFormat defaultBottomBorder = this.DefaultBottomBorder();
      if (defaultBottomBorder != null)
        return defaultBottomBorder;
    }
    return (ILineFormat) this.GetBorderLine(BorderType.Bottom);
  }

  private ILineFormat DefaultBottomBorder()
  {
    ILineFormat lineFormat = (ILineFormat) null;
    TableStyle tableStyle = this.Table.TableStyle;
    Table table = this._row.Table;
    if (tableStyle != null)
    {
      if (this.RowIndex == 1)
      {
        if (table.HasHeaderRow && tableStyle.FirstRow != null && tableStyle.FirstRow.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.FirstRow.TableCellStyle.CellBorderStyle.Bottom;
      }
      else if (this.RowIndex == table.Rows.Count)
      {
        if (table.HasTotalRow && tableStyle.LastRow != null && tableStyle.LastRow.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.LastRow.TableCellStyle.CellBorderStyle.Bottom;
        if (lineFormat == null)
          lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.Bottom;
      }
      if (this.ColumnIndex == 1)
      {
        if (table.HasFirstColumn && tableStyle.FirstColumn != null && tableStyle.FirstColumn.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.FirstColumn.TableCellStyle.CellBorderStyle.Bottom;
      }
      else if (this.ColumnIndex == table.Columns.Count && table.HasLastColumn && tableStyle.LastColumn != null && tableStyle.LastColumn.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.LastColumn.TableCellStyle.CellBorderStyle.Bottom;
      if (this.RowIndex != 1 && table.HasBandedRows && tableStyle.HorizontalBand1Style != null && tableStyle.HorizontalBand1Style.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.HorizontalBand1Style.TableCellStyle.CellBorderStyle.Bottom;
      if (lineFormat == null)
        lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.InsideHorzBorder;
    }
    return lineFormat;
  }

  public ILineFormat BorderDiagonalDown
  {
    get
    {
      if (this._borders == null)
        this._borders = new Dictionary<BorderType, LineFormat>(6);
      this._borderType |= BorderType.DiagonalDown;
      this.HasDiagonalDownBorder = true;
      return (ILineFormat) this.GetBorderLine(BorderType.DiagonalDown);
    }
  }

  internal ILineFormat GetDefaultDiagonalDownBorder()
  {
    if (!this.HasDiagonalDownBorder)
      return (ILineFormat) null;
    if (this._borders == null)
      this._borders = new Dictionary<BorderType, LineFormat>(6);
    this._borderType |= BorderType.DiagonalDown;
    return (ILineFormat) this.GetBorderLine(BorderType.DiagonalDown);
  }

  public ILineFormat BorderDiagonalUp
  {
    get
    {
      if (this._borders == null)
        this._borders = new Dictionary<BorderType, LineFormat>(6);
      this._borderType |= BorderType.DiagonalUp;
      this.HasDiagonalUpBorder = true;
      return (ILineFormat) this.GetBorderLine(BorderType.DiagonalUp);
    }
  }

  internal ILineFormat GetDefaultDiagonalUpBorder()
  {
    if (!this.HasDiagonalUpBorder)
      return (ILineFormat) null;
    if (this._borders == null)
      this._borders = new Dictionary<BorderType, LineFormat>(6);
    this._borderType |= BorderType.DiagonalUp;
    return (ILineFormat) this.GetBorderLine(BorderType.DiagonalUp);
  }

  public ILineFormat BorderLeft
  {
    get
    {
      if (this._borders == null)
        this._borders = new Dictionary<BorderType, LineFormat>(6);
      this._borderType |= BorderType.Left;
      return (ILineFormat) this.GetBorderLine(BorderType.Left);
    }
  }

  internal ILineFormat GetDefaultLeftBorder()
  {
    if (!Helper.HasFlag(this._borderType, BorderType.Left))
    {
      ILineFormat defaultLeftBorder = this.DefaultLeftBorder();
      if (defaultLeftBorder != null)
        return defaultLeftBorder;
    }
    return (ILineFormat) this.GetBorderLine(BorderType.Left);
  }

  private ILineFormat DefaultLeftBorder()
  {
    ILineFormat lineFormat = (ILineFormat) null;
    TableStyle tableStyle = this.Table.TableStyle;
    Table table = this._row.Table;
    if (tableStyle != null)
    {
      if (this.RowIndex == 1)
      {
        if (table.HasHeaderRow && tableStyle.FirstRow != null && tableStyle.FirstRow.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.FirstRow.TableCellStyle.CellBorderStyle.Left;
      }
      else if (this.RowIndex == table.Rows.Count && table.HasTotalRow && tableStyle.LastRow != null && tableStyle.LastRow.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.LastRow.TableCellStyle.CellBorderStyle.Left;
      if (this.ColumnIndex == 1)
      {
        if (table.HasFirstColumn && tableStyle.FirstColumn != null && tableStyle.FirstColumn.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.FirstColumn.TableCellStyle.CellBorderStyle.Left;
        if (lineFormat == null)
          lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.Left;
      }
      else if (this.ColumnIndex == table.Columns.Count && table.HasLastColumn && tableStyle.LastColumn != null && tableStyle.LastColumn.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.LastColumn.TableCellStyle.CellBorderStyle.Left;
      if (this.ColumnIndex != 1 && table.HasBandedColumns && tableStyle.VerticalBand1Style != null && tableStyle.VerticalBand1Style.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.VerticalBand1Style.TableCellStyle.CellBorderStyle.Left;
      if (lineFormat == null)
        lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.InsideVertBorder;
    }
    return lineFormat;
  }

  public ILineFormat BorderRight
  {
    get
    {
      if (this._borders == null)
        this._borders = new Dictionary<BorderType, LineFormat>(6);
      this._borderType |= BorderType.Right;
      return (ILineFormat) this.GetBorderLine(BorderType.Right);
    }
  }

  internal ILineFormat GetDefaultRightBorder()
  {
    if (!Helper.HasFlag(this._borderType, BorderType.Right))
    {
      ILineFormat defaultRightBorder = this.DefaultRightBorder();
      if (defaultRightBorder != null)
        return defaultRightBorder;
    }
    return (ILineFormat) this.GetBorderLine(BorderType.Right);
  }

  private ILineFormat DefaultRightBorder()
  {
    ILineFormat lineFormat = (ILineFormat) null;
    TableStyle tableStyle = this.Table.TableStyle;
    Table table = this._row.Table;
    if (tableStyle != null)
    {
      if (this.RowIndex == 1)
      {
        if (table.HasHeaderRow && tableStyle.FirstRow != null && tableStyle.FirstRow.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.FirstRow.TableCellStyle.CellBorderStyle.Right;
        if (lineFormat == null && this.RowIndex == table.Rows.Count)
          lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.Right;
      }
      else if (this.RowIndex == table.Rows.Count && table.HasTotalRow && tableStyle.LastRow != null && tableStyle.LastRow.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.LastRow.TableCellStyle.CellBorderStyle.Right;
      if (this.ColumnIndex == 1)
      {
        if (table.HasFirstColumn && tableStyle.FirstColumn != null && tableStyle.FirstColumn.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.FirstColumn.TableCellStyle.CellBorderStyle.Right;
        if (lineFormat == null && this.ColumnIndex == table.Columns.Count)
          lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.Right;
      }
      else if (this.ColumnIndex == table.Columns.Count)
      {
        if (table.HasLastColumn && tableStyle.LastColumn != null && tableStyle.LastColumn.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.LastColumn.TableCellStyle.CellBorderStyle.Right;
        if (lineFormat == null)
          lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.Right;
      }
      if (this.ColumnIndex != 1 && table.HasBandedColumns && tableStyle.VerticalBand1Style != null && tableStyle.VerticalBand1Style.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.VerticalBand1Style.TableCellStyle.CellBorderStyle.Right;
      if (lineFormat == null)
        lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.InsideVertBorder;
    }
    return lineFormat;
  }

  public ILineFormat BorderTop
  {
    get
    {
      if (this._borders == null)
        this._borders = new Dictionary<BorderType, LineFormat>(6);
      this._borderType |= BorderType.Top;
      return (ILineFormat) this.GetBorderLine(BorderType.Top);
    }
  }

  internal ILineFormat GetDefaultTopBorder()
  {
    if (!Helper.HasFlag(this._borderType, BorderType.Top))
    {
      ILineFormat defaultTopBorder = this.DefaultTopBorder();
      if (defaultTopBorder != null)
        return defaultTopBorder;
    }
    return (ILineFormat) this.GetBorderLine(BorderType.Top);
  }

  private ILineFormat DefaultTopBorder()
  {
    ILineFormat lineFormat = (ILineFormat) null;
    TableStyle tableStyle = this.Table.TableStyle;
    Table table = this._row.Table;
    if (tableStyle != null)
    {
      if (this.RowIndex == 1)
      {
        if (table.HasHeaderRow && tableStyle.FirstRow != null && tableStyle.FirstRow.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.FirstRow.TableCellStyle.CellBorderStyle.Top;
        if (lineFormat == null)
          lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.Top;
      }
      else if (this.RowIndex == table.Rows.Count && table.HasTotalRow && tableStyle.LastRow != null && tableStyle.LastRow.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.LastRow.TableCellStyle.CellBorderStyle.Top;
      if (this.ColumnIndex == 1)
      {
        if (table.HasFirstColumn && tableStyle.FirstColumn != null && tableStyle.FirstColumn.TableCellStyle.CellBorderStyle != null)
          lineFormat = (ILineFormat) tableStyle.FirstColumn.TableCellStyle.CellBorderStyle.Top;
      }
      else if (this.ColumnIndex == table.Columns.Count && table.HasLastColumn && tableStyle.LastColumn != null && tableStyle.LastColumn.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.LastColumn.TableCellStyle.CellBorderStyle.Top;
      if (this.RowIndex != 1 && table.HasBandedRows && tableStyle.HorizontalBand1Style != null && tableStyle.HorizontalBand1Style.TableCellStyle.CellBorderStyle != null)
        lineFormat = (ILineFormat) tableStyle.HorizontalBand1Style.TableCellStyle.CellBorderStyle.Top;
      if (lineFormat == null)
        lineFormat = (ILineFormat) tableStyle.WholeTableStyle.TableCellStyle.CellBorderStyle.InsideHorzBorder;
    }
    return lineFormat;
  }

  public int ColumnSpan
  {
    get => this._colSpan;
    set => this.SetColumnSpan(value, false);
  }

  internal void SetColumnSpan(int value, bool IsColumnAdded)
  {
    if (value < 1 && !IsColumnAdded)
      throw new ArgumentException("Invalid ColumnSpan" + value.ToString());
    int index1 = ((Cells) this._row.Cells).IndexOf((ICell) this);
    int num = value;
    if (IsColumnAdded)
      num = 0;
    for (int index2 = 1; index2 < num; ++index2)
    {
      if (index1 + index2 < this._row.Cells.Count)
      {
        foreach (Paragraph paragraph1 in (IEnumerable<IParagraph>) this._row.Cells[index1 + index2].TextBody.Paragraphs)
        {
          Paragraph paragraph2 = paragraph1.InternalClone();
          if (this._textFrame != null)
          {
            paragraph2.SetParent((Paragraphs) this._textFrame.Paragraphs);
            ((Paragraphs) this._textFrame.Paragraphs).Add((IParagraph) paragraph2);
          }
          else
          {
            this._textFrame = new Syncfusion.Presentation.RichText.TextBody(this);
            paragraph2.SetParent((Paragraphs) this._textFrame.Paragraphs);
            ((Paragraphs) this._textFrame.Paragraphs).Add((IParagraph) paragraph2);
          }
        }
      }
    }
    if (!this._row.Table.BaseSlide.Presentation.Created)
    {
      this._colSpan = value;
    }
    else
    {
      if (index1 == this._row.Cells.Count - 1)
      {
        index1 = this._row.Cells.Count - value;
        (this._row.Cells[index1] as Cell)._colSpan = value;
      }
      else
        this._colSpan = value;
      for (int index3 = 1; index3 < value; ++index3)
        (this._row.Cells[index1 + index3] as Cell).IsHorizontalMerge = true;
    }
  }

  public IFill Fill => (IFill) this._fillFormat;

  internal IFill GetDefaultFillFormat()
  {
    if (!this._isFillSet)
    {
      Syncfusion.Presentation.Drawing.Fill defaultFillFormat = (Syncfusion.Presentation.Drawing.Fill) this.DefaultFillFormat();
      if (defaultFillFormat != null)
        return (IFill) defaultFillFormat;
    }
    return (IFill) this._fillFormat;
  }

  private IFill DefaultFillFormat()
  {
    TableStyle tableStyle = this.Table.TableStyle;
    Table table = this._row.Table;
    if (tableStyle == null)
      return (IFill) null;
    if (table.HasHeaderRow && tableStyle.FirstRow != null && this.RowIndex == 1)
    {
      Syncfusion.Presentation.Drawing.Fill fill1 = tableStyle.FirstRow.TableCellStyle.Fill;
      if (fill1 != null)
        return (IFill) fill1;
      if (tableStyle.FirstRow.TableCellStyle.FillRefIndex == "1")
      {
        Syncfusion.Presentation.Drawing.Fill fill2 = new Syncfusion.Presentation.Drawing.Fill(this);
        fill2.FillType = FillType.Solid;
        ((SolidFill) fill2.SolidFill).SetColorObject(tableStyle.FirstRow.TableCellStyle.FillRefColor);
        return (IFill) fill2;
      }
    }
    if (table.HasFirstColumn && tableStyle.FirstColumn != null && this.ColumnIndex == 1)
      return (IFill) tableStyle.FirstColumn.TableCellStyle.Fill;
    if (table.HasLastColumn && tableStyle.LastColumn != null && this.ColumnIndex == table.Columns.Count)
      return (IFill) tableStyle.LastColumn.TableCellStyle.Fill;
    if (table.HasTotalRow && tableStyle.LastColumn != null && this.RowIndex == table.Rows.Count)
      return (IFill) tableStyle.LastColumn.TableCellStyle.Fill;
    if (table.HasBandedRows)
    {
      if (table.HasHeaderRow && tableStyle.HorizontalBand1Style != null && this.RowIndex != 1 && this.RowIndex % 2 == 0)
        return (IFill) tableStyle.HorizontalBand1Style.TableCellStyle.Fill;
      if (!table.HasHeaderRow)
      {
        if (tableStyle.HorizontalBand1Style != null && this.RowIndex % 2 == 1)
          return (IFill) tableStyle.HorizontalBand1Style.TableCellStyle.Fill;
        return tableStyle.HorizontalBand2Style != null && this.RowIndex % 2 == 0 && tableStyle.HorizontalBand2Style.TableCellStyle.Fill != null ? (IFill) tableStyle.HorizontalBand2Style.TableCellStyle.Fill : (IFill) tableStyle.WholeTableStyle.TableCellStyle.Fill;
      }
      if (tableStyle.HorizontalBand1Style != null && this.RowIndex % 2 == 0)
        return (IFill) tableStyle.HorizontalBand1Style.TableCellStyle.Fill;
      return tableStyle.HorizontalBand2Style != null && this.RowIndex % 2 == 1 && tableStyle.HorizontalBand2Style.TableCellStyle.Fill != null ? (IFill) tableStyle.HorizontalBand2Style.TableCellStyle.Fill : (IFill) tableStyle.WholeTableStyle.TableCellStyle.Fill;
    }
    if (!table.HasBandedColumns)
      return (IFill) tableStyle.WholeTableStyle.TableCellStyle.Fill;
    if (table.HasFirstColumn && tableStyle.VerticalBand1Style != null)
    {
      if (this.ColumnIndex != 1 && this.ColumnIndex % 2 == 0)
        return (IFill) tableStyle.VerticalBand1Style.TableCellStyle.Fill;
    }
    else if (tableStyle.VerticalBand1Style != null && this.ColumnIndex % 2 == 1)
      return (IFill) tableStyle.VerticalBand1Style.TableCellStyle.Fill;
    return (IFill) tableStyle.WholeTableStyle.TableCellStyle.Fill;
  }

  public bool IsHorizontalMerge
  {
    get => this._hMerge;
    set => this._hMerge = value;
  }

  public int RowSpan
  {
    get => this._rowSpan;
    set
    {
      if (value < 2)
        throw new ArgumentException("Invalid RowSpan " + value.ToString());
      int rowIndex = ((Rows) this.Table.Rows).IndexOf((IRow) this._row);
      int index1 = ((Cells) this._row.Cells).IndexOf((ICell) this);
      if (!this._row.Table.BaseSlide.Presentation.Created)
      {
        this._rowSpan = value;
      }
      else
      {
        if (rowIndex == this.Table.Rows.Count - 1)
        {
          rowIndex = this.Table.Rows.Count - value;
          (this.Table.Rows[rowIndex].Cells[index1] as Cell)._rowSpan = value;
        }
        else
          this._rowSpan = value;
        for (int index2 = 1; index2 < value; ++index2)
          (this.Table.Rows[rowIndex + index2].Cells[index1] as Cell).IsVerticalMerge = true;
      }
      for (int index3 = 1; index3 < this._rowSpan; ++index3)
      {
        if (rowIndex + index3 < this.Table.Rows.Count)
        {
          foreach (Paragraph paragraph1 in (IEnumerable<IParagraph>) this.Table.Rows[rowIndex + index3].Cells[index1].TextBody.Paragraphs)
          {
            Paragraph paragraph2 = paragraph1.InternalClone();
            if (this._textFrame != null)
            {
              paragraph2.SetParent((Paragraphs) this._textFrame.Paragraphs);
              ((Paragraphs) this._textFrame.Paragraphs).Add((IParagraph) paragraph2);
            }
            else
            {
              this._textFrame = new Syncfusion.Presentation.RichText.TextBody(this);
              paragraph2.SetParent((Paragraphs) this._textFrame.Paragraphs);
              ((Paragraphs) this._textFrame.Paragraphs).Add((IParagraph) paragraph2);
            }
          }
        }
      }
      if (this._row.Cells.Count != 1)
        return;
      for (int index4 = 1; index4 < this._rowSpan; ++index4)
      {
        this.Table.Rows[rowIndex].Height += this.Table.Rows[rowIndex + 1].Height;
        this.Table.Rows.RemoveAt(rowIndex + 1);
      }
    }
  }

  public ITextBody TextBody
  {
    get => (ITextBody) this._textFrame ?? (ITextBody) (this._textFrame = new Syncfusion.Presentation.RichText.TextBody(this));
  }

  public bool IsVerticalMerge
  {
    get => this._vMerge;
    set => this._vMerge = value;
  }

  internal bool IsFillSet
  {
    get => this._isFillSet;
    set => this._isFillSet = value;
  }

  internal string Id
  {
    get => this._id;
    set => this._id = value;
  }

  internal Table Table => this._row.Table;

  internal long CellIndex
  {
    get => this._cellIndex;
    set => this._cellIndex = value;
  }

  internal int RowIndex => this.ObtainRowIndex();

  internal int ColumnIndex => this.ObtainColumnIndex();

  internal CellInfo CellInfo
  {
    get => this._cellInfo;
    set => this._cellInfo = value;
  }

  internal ILineFormat GetBottomBorder()
  {
    return !Helper.EnumContains(this._borderType, BorderType.Bottom) ? (ILineFormat) null : this.BorderBottom;
  }

  internal ILineFormat GetDiagonalDownBorder()
  {
    return !Helper.EnumContains(this._borderType, BorderType.DiagonalDown) ? (ILineFormat) null : this.BorderDiagonalDown;
  }

  internal ILineFormat GetDiagonalUpBorder()
  {
    return !Helper.EnumContains(this._borderType, BorderType.DiagonalUp) ? (ILineFormat) null : this.BorderDiagonalUp;
  }

  internal ILineFormat GetLeftBorder()
  {
    return !Helper.EnumContains(this._borderType, BorderType.Left) ? (ILineFormat) null : this.BorderLeft;
  }

  internal ILineFormat GetRightBorder()
  {
    return !Helper.EnumContains(this._borderType, BorderType.Right) ? (ILineFormat) null : this.BorderRight;
  }

  internal ILineFormat GetTopBorder()
  {
    return !Helper.EnumContains(this._borderType, BorderType.Top) ? (ILineFormat) null : this.BorderTop;
  }

  internal long GetCellIndex(int column, int row)
  {
    return this._cellIndex = ((long) row << 32 /*0x20*/) + (long) column;
  }

  internal int ObtainRowIndex()
  {
    this._rowIndex = Cell.GetRowIndex(this._cellIndex);
    return this._rowIndex;
  }

  internal int ObtainColumnIndex()
  {
    this._columnIndex = Cell.GetColumnIndex(this._cellIndex);
    return this._columnIndex;
  }

  internal static int GetColumnIndex(long cellIndex) => (int) cellIndex;

  internal static int GetRowIndex(long cellIndex) => (int) (cellIndex >> 32 /*0x20*/);

  internal float Layout(
    float usedHeight,
    float usedWidth,
    float tableLeft,
    float tableTop,
    int rowIndex,
    int colIndex,
    ref float totalTextHeight)
  {
    this._cellInfo = new CellInfo(this);
    float x1 = tableLeft + usedWidth;
    float y1 = tableTop + usedHeight;
    float width = (float) this.Table.Columns[colIndex].Width;
    float height = (float) this._row.Height;
    float maxWidth = 0.0f;
    usedWidth += width;
    this.UpdateMergedCells(rowIndex, colIndex, ref width, ref height);
    this._cellInfo.Bounds = new RectangleF(x1, y1, width, height);
    Syncfusion.Presentation.RichText.TextBody textBody = this.TextBody as Syncfusion.Presentation.RichText.TextBody;
    float defaultLeftMargin = (float) textBody.GetDefaultLeftMargin();
    float defaultTopMargin = (float) textBody.GetDefaultTopMargin();
    float defaultRightMargin = (float) textBody.GetDefaultRightMargin();
    float defaultBottomMargin = (float) textBody.GetDefaultBottomMargin();
    float x2 = x1 + defaultLeftMargin;
    float y2 = y1 + defaultTopMargin;
    float num1 = width - (defaultLeftMargin + defaultRightMargin);
    float num2 = height - (defaultTopMargin + defaultBottomMargin);
    switch (textBody.ObatinTextDirection())
    {
      case TextDirection.Vertical:
      case TextDirection.Vertical270:
        this._cellInfo.TextLayoutingBounds = new RectangleF(x2, y2, num2, num1);
        break;
      default:
        this._cellInfo.TextLayoutingBounds = new RectangleF(x2, y2, num1, num2);
        break;
    }
    IParagraphs paragraphs = this.TextBody.Paragraphs;
    if (paragraphs.Count == 0)
      this.TextBody.AddParagraph("");
    foreach (Paragraph paragraph in (IEnumerable<IParagraph>) paragraphs)
      paragraph.Layout(this._cellInfo.TextLayoutingBounds, ref totalTextHeight, this.TextBody.WrapText, ref maxWidth);
    if (textBody.ObatinTextDirection() == TextDirection.Vertical270 && textBody.VerticalAlignment == VerticalAlignmentType.None)
      totalTextHeight = this._cellInfo.TextLayoutingBounds.Width;
    this._cellInfo.TotalTextHeight = totalTextHeight;
    this._cellInfo.MaxTextWidth = maxWidth;
    totalTextHeight += defaultTopMargin + defaultBottomMargin;
    if (this.RowSpan > 1)
      totalTextHeight = 0.0f;
    return usedWidth;
  }

  private void UpdateMergedCells(
    int rowIndex,
    int colIndex,
    ref float cellWidth,
    ref float cellHeight)
  {
    if (this.RowSpan > 0 && this.ColumnSpan > 0)
    {
      for (int index1 = rowIndex + 1; index1 <= this.RowSpan + rowIndex; ++index1)
      {
        for (int index2 = colIndex + 1; index2 <= this.ColumnSpan + colIndex; ++index2)
        {
          if (index1 != rowIndex + 1 || index2 != colIndex + 1)
          {
            this.Table.MergedCells.Add(((Cell) this.Table.Rows[index1 - 1].Cells[index2 - 1]).CellIndex);
            if (index1 == rowIndex + 1)
              cellWidth += (float) this.Table.Columns[index2 - 1].Width;
          }
        }
        if (index1 != rowIndex + 1)
          cellHeight += (float) this.Table.Rows[index1 - 1].Height;
      }
    }
    else if (this.RowSpan > 0)
    {
      for (int index = rowIndex + 1; index <= this.RowSpan + rowIndex; ++index)
      {
        if (index != rowIndex + 1)
        {
          this.Table.MergedCells.Add(((Cell) this.Table.Rows[index - 1].Cells[colIndex]).CellIndex);
          cellHeight += (float) this.Table.Rows[index - 1].Height;
        }
      }
    }
    else
    {
      if (this.ColumnSpan <= 0)
        return;
      for (int index = colIndex + 1; index <= this.ColumnSpan + colIndex; ++index)
      {
        if (index != colIndex + 1)
        {
          this.Table.MergedCells.Add(((Cell) this.Table.Rows[rowIndex].Cells[index - 1]).CellIndex);
          cellWidth += (float) this.Table.Columns[index - 1].Width;
        }
      }
    }
  }

  internal void LayoutXYPosition(
    float shapeHeight,
    float shapeWidth,
    float usedHeight,
    Syncfusion.Presentation.RichText.TextBody textFrame,
    float maxWidth)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    Paragraphs paragraphs = (Paragraphs) textFrame.Paragraphs;
    Syncfusion.Presentation.RichText.TextBody textBody = this.TextBody as Syncfusion.Presentation.RichText.TextBody;
    switch (textBody.VerticalAlignment)
    {
      case VerticalAlignmentType.Middle:
        num1 = (float) (((double) shapeHeight - (double) usedHeight) / 2.0);
        break;
      case VerticalAlignmentType.Bottom:
        num1 = shapeHeight - usedHeight;
        break;
    }
    if (textBody.AnchorCenter && paragraphs.Count > 0 && paragraphs.HasSameTextAlignment)
    {
      switch (((Paragraph) paragraphs[0]).GetDefaultAlignmentType())
      {
        case HorizontalAlignmentType.Left:
          num2 = (float) (((double) shapeWidth - (double) maxWidth) / 2.0);
          break;
        case HorizontalAlignmentType.Right:
          num2 = (float) (((double) shapeWidth - (double) maxWidth) / -2.0);
          break;
      }
    }
    if ((double) num1 == 0.0 && (double) num2 == 0.0)
      return;
    foreach (IParagraph paragraph in paragraphs)
    {
      if (((Paragraph) paragraph).ParagraphInfo != null)
      {
        foreach (LineInfo lineInfo in ((Paragraph) paragraph).ParagraphInfo.LineInfoCollection)
        {
          foreach (TextInfo textInfo in lineInfo.TextInfoCollection)
          {
            textInfo.Y += num1;
            textInfo.X += num2;
          }
        }
      }
    }
  }

  internal Syncfusion.Presentation.Drawing.Fill GetFillFormat() => this._fillFormat;

  private LineFormat GetBorderLine(BorderType borderType)
  {
    if (this._borders.ContainsKey(borderType))
      return this._borders[borderType];
    LineFormat borderLine = new LineFormat((Shape) this.Table, borderType, this._cellIndex);
    this._borders.Add(borderType, borderLine);
    return borderLine;
  }

  internal void Close()
  {
    if (this._borders != null)
    {
      foreach (KeyValuePair<BorderType, LineFormat> border in this._borders)
        border.Value.Close();
      this._borders.Clear();
      this._borders = (Dictionary<BorderType, LineFormat>) null;
    }
    if (this._fillFormat != null)
    {
      this._fillFormat.Close();
      this._fillFormat = (Syncfusion.Presentation.Drawing.Fill) null;
    }
    if (this._textFrame == null)
      return;
    this._textFrame.Close();
    this._textFrame = (Syncfusion.Presentation.RichText.TextBody) null;
  }

  public ICell Clone()
  {
    Cell cell = (Cell) this.MemberwiseClone();
    if (this._borders != null)
      cell._borders = this.CloneBorders();
    if (this._cellInfo != null)
      cell._cellInfo = this._cellInfo.Clone(cell);
    cell._fillFormat = this._fillFormat.Clone();
    cell._fillFormat.SetParent((object) cell);
    if (this._textFrame != null)
    {
      cell._textFrame = this._textFrame.Clone();
      cell._textFrame.SetParent(cell);
    }
    return (ICell) cell;
  }

  private Dictionary<BorderType, LineFormat> CloneBorders()
  {
    Dictionary<BorderType, LineFormat> dictionary = new Dictionary<BorderType, LineFormat>();
    foreach (KeyValuePair<BorderType, LineFormat> border in this._borders)
    {
      LineFormat lineFormat = border.Value.Clone();
      dictionary.Add(border.Key, lineFormat);
    }
    return dictionary;
  }

  internal void SetParent(Row row) => this._row = row;

  internal void SetParent(Column column) => this._column = column;

  internal void SetRowSpan(int rowSpan) => this._rowSpan = rowSpan;

  internal void SetParent(Table newParent)
  {
    if (this._borders == null)
      return;
    foreach (KeyValuePair<BorderType, LineFormat> border in this._borders)
      border.Value.SetParent((Shape) newParent);
  }

  internal void SetParent(BaseSlide newParent)
  {
    if (this._textFrame == null)
      return;
    this._textFrame.SetParent(newParent);
  }
}
