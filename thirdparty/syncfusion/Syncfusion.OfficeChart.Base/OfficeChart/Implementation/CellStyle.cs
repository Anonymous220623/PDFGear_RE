// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.CellStyle
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class CellStyle : 
  ExtendedFormatWrapper,
  IStyle,
  IExtendedFormat,
  IParentApplication,
  IOptimizedUpdate
{
  private RangeImpl m_range;
  private bool m_bAskAdjacent = true;

  public CellStyle(RangeImpl range)
    : base(range.Workbook)
  {
    this.m_range = range;
  }

  public CellStyle(RangeImpl range, int iXFIndex)
    : base(range.Workbook, iXFIndex)
  {
    this.m_range = range;
  }

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0)
    {
      this.BeforeRead();
      this.m_xFormat = this.m_book.CreateExtFormatWithoutRegister((IExtendedFormat) this.m_xFormat);
    }
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    this.m_xFormat = this.m_book.AddExtendedProperties(this.m_xFormat);
    if (this.m_xFormat.Index <= this.m_book.DefaultXFIndex || !this.m_book.m_xfCellCount.ContainsKey(this.m_xFormat.Index))
    {
      this.m_xFormat = this.m_book.RegisterExtFormat(this.m_xFormat);
      this.m_range.ExtendedFormatIndex = (ushort) this.m_xFormat.Index;
    }
    else
    {
      int num;
      this.m_book.m_xfCellCount.TryGetValue(this.m_xFormat.Index, out num);
      if (num == 1)
      {
        this.m_book.InnerExtFormats[this.m_xFormat.Index].UpdateFromCurrentExtendedFormat(this.m_xFormat);
        this.m_range.ExtendedFormatIndex = (ushort) this.m_xFormat.Index;
      }
      else
      {
        this.m_book.m_xfCellCount[this.m_xFormat.Index] = num - 1;
        this.m_xFormat = this.m_book.RegisterExtFormat(this.m_xFormat);
        this.m_range.ExtendedFormatIndex = (ushort) this.m_xFormat.Index;
      }
    }
  }

  protected override void SetParents(object parent)
  {
    this.m_range = CommonObject.FindParent(parent, typeof (RangeImpl)) as RangeImpl;
    this.m_book = this.m_range != null ? this.m_range.Workbook : throw new ArgumentNullException(nameof (parent), "Can't find parent range.");
  }

  protected override void BeforeRead()
  {
    if (this.BeginCallsCount != 0)
      return;
    base.BeforeRead();
    this.SetFormatIndex((int) this.m_range.ExtendedFormatIndex);
  }

  public override ChartColor LeftBorderColor => this.GetLeftBorderColor(this.AskAdjacent);

  public override ChartColor RightBorderColor => this.GetRightBorderColor(this.AskAdjacent);

  public override ChartColor TopBorderColor => this.GetTopBorderColor(this.AskAdjacent);

  public override ChartColor BottomBorderColor => this.GetBottomBorderColor(this.AskAdjacent);

  public override OfficeLineStyle LeftBorderLineStyle
  {
    get
    {
      return this.m_range.IsMerged && this.m_range.MergeArea.Column != this.m_range.Column ? this.GetLeftLineStyle(false) : this.GetLeftLineStyle(this.AskAdjacent);
    }
    set
    {
      if (this.GetLeftLineStyle(false) == value)
        return;
      base.LeftBorderLineStyle = value;
    }
  }

  public override OfficeLineStyle RightBorderLineStyle
  {
    get
    {
      return this.m_range.IsMerged && this.m_range.MergeArea.LastColumn != this.m_range.Column ? this.GetRightLineStyle(false) : this.GetRightLineStyle(this.AskAdjacent);
    }
    set
    {
      if (this.GetRightLineStyle(false) == value)
        return;
      base.RightBorderLineStyle = value;
    }
  }

  public override OfficeLineStyle TopBorderLineStyle
  {
    get
    {
      return this.m_range.IsMerged && this.m_range.MergeArea.Row != this.m_range.Row ? this.GetTopLineStyle(false) : this.GetTopLineStyle(this.AskAdjacent);
    }
    set
    {
      if (this.GetTopLineStyle(false) == value)
        return;
      base.TopBorderLineStyle = value;
    }
  }

  public override OfficeLineStyle BottomBorderLineStyle
  {
    get
    {
      return this.m_range.IsMerged && this.m_range.MergeArea.LastColumn != this.m_range.Column ? this.GetBottomLineStyle(false) : this.GetBottomLineStyle(this.AskAdjacent);
    }
    set
    {
      if (this.GetBottomLineStyle(false) == value)
        return;
      base.BottomBorderLineStyle = value;
    }
  }

  internal bool AskAdjacent
  {
    get => this.m_bAskAdjacent;
    set => this.m_bAskAdjacent = value;
  }

  protected OfficeLineStyle GetLeftLineStyle(bool askAdjecent)
  {
    RangeImpl rangeImpl = (RangeImpl) null;
    bool flag = true;
    if (this.m_range.IsMerged && askAdjecent)
    {
      rangeImpl = this.m_range;
      this.m_range = this.m_range.MergeArea.Cells[0] as RangeImpl;
      if (!rangeImpl.CellsList.Contains((IRange) this.m_range))
        flag = false;
    }
    OfficeLineStyle leftLineStyle = base.LeftBorderLineStyle;
    if (leftLineStyle == OfficeLineStyle.None && askAdjecent && flag)
    {
      IRange leftCell = this.GetLeftCell();
      if (leftCell != null && leftCell.Columns[0].ColumnWidth != 0.0)
        leftLineStyle = (leftCell.CellStyle as CellStyle).GetRightLineStyle(false);
    }
    if (rangeImpl != null)
      this.m_range = rangeImpl;
    return leftLineStyle;
  }

  protected OfficeLineStyle GetRightLineStyle(bool askAdjecent)
  {
    RangeImpl rangeImpl = (RangeImpl) null;
    bool flag = true;
    if (this.m_range.IsMerged && askAdjecent)
    {
      rangeImpl = this.m_range;
      IRange[] cells = this.m_range.MergeArea.Cells;
      this.m_range = cells[cells.Length - 1] as RangeImpl;
      if (!rangeImpl.CellsList.Contains((IRange) this.m_range))
        flag = false;
    }
    OfficeLineStyle rightLineStyle = base.RightBorderLineStyle;
    if (rightLineStyle == OfficeLineStyle.None && askAdjecent && flag)
    {
      IRange rightCell = this.GetRightCell();
      if (rightCell != null && rightCell.Columns[0].ColumnWidth != 0.0)
        rightLineStyle = (rightCell.CellStyle as CellStyle).GetLeftLineStyle(false);
    }
    if (rangeImpl != null)
      this.m_range = rangeImpl;
    return rightLineStyle;
  }

  protected OfficeLineStyle GetTopLineStyle(bool askAdjecent)
  {
    RangeImpl rangeImpl = (RangeImpl) null;
    bool flag = true;
    if (this.m_range.IsMerged && askAdjecent)
    {
      rangeImpl = this.m_range;
      this.m_range = this.m_range.MergeArea.Cells[0] as RangeImpl;
      if (!rangeImpl.CellsList.Contains((IRange) this.m_range))
        flag = false;
    }
    OfficeLineStyle topLineStyle = base.TopBorderLineStyle;
    if (topLineStyle == OfficeLineStyle.None && askAdjecent && flag)
    {
      IRange topCell = this.GetTopCell();
      if (topCell != null && topCell.Rows[0].RowHeight != 0.0)
        topLineStyle = (topCell.CellStyle as CellStyle).GetBottomLineStyle(false);
    }
    if (rangeImpl != null)
      this.m_range = rangeImpl;
    return topLineStyle;
  }

  protected OfficeLineStyle GetBottomLineStyle(bool askAdjecent)
  {
    RangeImpl rangeImpl = (RangeImpl) null;
    bool flag = true;
    if (this.m_range.IsMerged && askAdjecent)
    {
      rangeImpl = this.m_range;
      IRange[] cells = this.m_range.MergeArea.Cells;
      this.m_range = cells[cells.Length - 1] as RangeImpl;
      if (!rangeImpl.CellsList.Contains((IRange) this.m_range))
        flag = false;
    }
    OfficeLineStyle bottomLineStyle = base.BottomBorderLineStyle;
    if (bottomLineStyle == OfficeLineStyle.None && askAdjecent && flag)
    {
      IRange bottomCell = this.GetBottomCell();
      if (bottomCell != null && bottomCell.Rows[0].RowHeight != 0.0)
        bottomLineStyle = (bottomCell.CellStyle as CellStyle).GetTopLineStyle(false);
    }
    if (rangeImpl != null)
      this.m_range = rangeImpl;
    return bottomLineStyle;
  }

  protected ChartColor GetLeftBorderColor(bool askAdjacent)
  {
    ChartColor leftBorderColor = base.LeftBorderColor;
    if (base.LeftBorderLineStyle == OfficeLineStyle.None && askAdjacent)
    {
      IRange leftCell = this.GetLeftCell();
      if (leftCell != null)
        leftBorderColor = (leftCell.CellStyle as CellStyle).GetRightBorderColor(false);
    }
    return leftBorderColor;
  }

  protected ChartColor GetRightBorderColor(bool askAdjacent)
  {
    ChartColor rightBorderColor = base.RightBorderColor;
    if (base.RightBorderLineStyle == OfficeLineStyle.None && askAdjacent)
    {
      IRange rightCell = this.GetRightCell();
      if (rightCell != null)
        rightBorderColor = (rightCell.CellStyle as CellStyle).GetLeftBorderColor(false);
    }
    return rightBorderColor;
  }

  protected ChartColor GetTopBorderColor(bool askAdjecent)
  {
    ChartColor topBorderColor = base.TopBorderColor;
    if (base.TopBorderLineStyle == OfficeLineStyle.None && askAdjecent)
    {
      IRange topCell = this.GetTopCell();
      if (topCell != null)
        topBorderColor = (topCell.CellStyle as CellStyle).GetBottomBorderColor(false);
    }
    return topBorderColor;
  }

  protected ChartColor GetBottomBorderColor(bool askAdjecent)
  {
    ChartColor bottomBorderColor = base.BottomBorderColor;
    if (base.BottomBorderLineStyle == OfficeLineStyle.None && askAdjecent)
    {
      IRange bottomCell = this.GetBottomCell();
      if (bottomCell != null)
        bottomBorderColor = (bottomCell.CellStyle as CellStyle).GetTopBorderColor(false);
    }
    return bottomBorderColor;
  }

  private IRange GetLeftCell() => this.GetCell(0, -1);

  private IRange GetRightCell() => this.GetCell(0, 1);

  private IRange GetTopCell() => this.GetCell(-1, 0);

  private IRange GetBottomCell() => this.GetCell(1, 0);

  private IRange GetCell(int rowDelta, int colDelta)
  {
    int row = this.m_range.Row + rowDelta;
    int column = this.m_range.Column + colDelta;
    IRange cell = (IRange) null;
    if (row > 0 && row <= this.m_book.MaxRowCount && column > 0 && column <= this.m_book.MaxColumnCount)
      cell = this.m_range[row, column];
    return cell;
  }
}
