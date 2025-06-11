// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CellStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class CellStyle : 
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
    int extendedFormatIndex = (int) this.m_range.ExtendedFormatIndex;
    if (this.m_xFormat.Index <= this.m_book.DefaultXFIndex || !this.m_book.m_xfCellCount.ContainsKey(this.m_xFormat.Index + this.m_book.m_XFRemovedCount))
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
        this.m_book.InnerExtFormats[this.m_xFormat.Index].UpdateFromCurrentExtendedFormatNew(this.m_xFormat, true);
        this.m_range.ExtendedFormatIndex = (ushort) this.m_xFormat.Index;
      }
      else
      {
        this.m_book.m_xfCellCount[this.m_xFormat.Index] = num - 1;
        this.m_xFormat = this.m_book.RegisterExtFormat(this.m_xFormat);
        this.m_range.ExtendedFormatIndex = (ushort) this.m_xFormat.Index;
      }
    }
    if (!this.m_book.m_bisXml || this.m_book.IsConverting || this.m_book.Options == ExcelParseOptions.ParseWorksheetsOnDemand)
      return;
    if (this.m_book.m_usedCellStyleIndex.ContainsKey(extendedFormatIndex) && extendedFormatIndex != this.m_book.DefaultXFIndex && !this.m_book.m_bisCopy && !this.m_book.Loading)
    {
      int num;
      this.m_book.m_usedCellStyleIndex.TryGetValue(extendedFormatIndex, out num);
      if (num == 1)
        this.m_book.m_usedCellStyleIndex.Remove(extendedFormatIndex);
      else if (num != 1)
        this.m_book.m_usedCellStyleIndex[extendedFormatIndex] = num - 1;
    }
    if (this.m_book.m_usedCellStyleIndex.ContainsKey((int) this.m_range.ExtendedFormatIndex))
    {
      int num;
      this.m_book.m_usedCellStyleIndex.TryGetValue((int) this.m_range.ExtendedFormatIndex, out num);
      this.m_book.m_usedCellStyleIndex[(int) this.m_range.ExtendedFormatIndex] = num + 1;
    }
    else
      this.m_book.m_usedCellStyleIndex.Add((int) this.m_range.ExtendedFormatIndex, this.m_range.Cells.Length);
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
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    this.SetFormatIndex((int) this.m_range.ExtendedFormatIndex);
  }

  public override ColorObject LeftBorderColor => this.GetLeftBorderColor(this.AskAdjacent);

  public override ColorObject RightBorderColor => this.GetRightBorderColor(this.AskAdjacent);

  public override ColorObject TopBorderColor => this.GetTopBorderColor(this.AskAdjacent);

  public override ColorObject BottomBorderColor => this.GetBottomBorderColor(this.AskAdjacent);

  public override ExcelLineStyle LeftBorderLineStyle
  {
    get
    {
      return this.AskAdjacent && this.m_range.IsMerged && this.m_range.MergeArea.Column != this.m_range.Column ? this.GetLeftLineStyle(false) : this.GetLeftLineStyle(this.AskAdjacent);
    }
    set => base.LeftBorderLineStyle = value;
  }

  public override ExcelLineStyle RightBorderLineStyle
  {
    get
    {
      return this.AskAdjacent && this.m_range.IsMerged && this.m_range.MergeArea.LastColumn != this.m_range.Column ? this.GetRightLineStyle(false) : this.GetRightLineStyle(this.AskAdjacent);
    }
    set => base.RightBorderLineStyle = value;
  }

  public override ExcelLineStyle TopBorderLineStyle
  {
    get
    {
      return this.AskAdjacent && this.m_range.IsMerged && this.m_range.MergeArea.Row != this.m_range.Row ? this.GetTopLineStyle(false) : this.GetTopLineStyle(this.AskAdjacent);
    }
    set => base.TopBorderLineStyle = value;
  }

  public override ExcelLineStyle BottomBorderLineStyle
  {
    get
    {
      return this.AskAdjacent && this.m_range.IsMerged && this.m_range.MergeArea.LastColumn != this.m_range.Column ? this.GetBottomLineStyle(false) : this.GetBottomLineStyle(this.AskAdjacent);
    }
    set => base.BottomBorderLineStyle = value;
  }

  internal bool AskAdjacent
  {
    get => this.m_bAskAdjacent;
    set => this.m_bAskAdjacent = value;
  }

  protected ExcelLineStyle GetLeftLineStyle(bool askAdjecent)
  {
    ExcelLineStyle lineStyle1 = base.LeftBorderLineStyle;
    IRange leftCell = this.GetLeftCell();
    ExcelLineStyle lineStyle2 = ExcelLineStyle.None;
    if (leftCell != null && leftCell.Columns[0].ColumnWidth != 0.0)
      lineStyle2 = (leftCell.CellStyle as CellStyle).GetRightLineStyle(false);
    if ((double) this.GetBorderWidth(lineStyle1) < (double) this.GetBorderWidth(lineStyle2) && askAdjecent)
      lineStyle1 = lineStyle2;
    return lineStyle1;
  }

  internal void SetBaseLineStyle(ExcelBordersIndex border, ExcelLineStyle lineStyle)
  {
    switch (border)
    {
      case ExcelBordersIndex.EdgeLeft:
        base.LeftBorderLineStyle = lineStyle;
        break;
      case ExcelBordersIndex.EdgeTop:
        base.TopBorderLineStyle = lineStyle;
        break;
      case ExcelBordersIndex.EdgeBottom:
        base.BottomBorderLineStyle = lineStyle;
        break;
      case ExcelBordersIndex.EdgeRight:
        base.RightBorderLineStyle = lineStyle;
        break;
    }
  }

  protected ExcelLineStyle GetRightLineStyle(bool askAdjecent)
  {
    ExcelLineStyle rightLineStyle = base.RightBorderLineStyle;
    if (rightLineStyle == ExcelLineStyle.None && askAdjecent)
    {
      IRange rightCell = this.GetRightCell();
      if (rightCell != null && rightCell.Columns[0].ColumnWidth != 0.0)
        rightLineStyle = (rightCell.CellStyle as CellStyle).GetLeftLineStyle(false);
    }
    return rightLineStyle;
  }

  protected ExcelLineStyle GetTopLineStyle(bool askAdjecent)
  {
    ExcelLineStyle lineStyle1 = base.TopBorderLineStyle;
    IRange topCell = this.GetTopCell();
    ExcelLineStyle lineStyle2 = ExcelLineStyle.None;
    if (topCell != null)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow(topCell.Worksheet as IInternalWorksheet, topCell.Row - 1, false);
      if (row == null || row != null && !row.IsHidden)
        lineStyle2 = (topCell.CellStyle as CellStyle).GetBottomLineStyle(false);
    }
    if ((double) this.GetBorderWidth(lineStyle1) < (double) this.GetBorderWidth(lineStyle2) && askAdjecent)
      lineStyle1 = lineStyle2;
    return lineStyle1;
  }

  protected ExcelLineStyle GetBottomLineStyle(bool askAdjecent)
  {
    ExcelLineStyle bottomLineStyle = base.BottomBorderLineStyle;
    if (bottomLineStyle == ExcelLineStyle.None && askAdjecent)
    {
      IRange bottomCell = this.GetBottomCell();
      if (bottomCell != null)
      {
        RowStorage row = WorksheetHelper.GetOrCreateRow(bottomCell.Worksheet as IInternalWorksheet, bottomCell.Row - 1, false);
        if (row == null || row != null && !row.IsHidden)
          bottomLineStyle = (bottomCell.CellStyle as CellStyle).GetTopLineStyle(false);
      }
    }
    return bottomLineStyle;
  }

  private float GetBorderWidth(ExcelLineStyle lineStyle)
  {
    float borderWidth = 0.0f;
    switch (lineStyle)
    {
      case ExcelLineStyle.None:
        borderWidth = 0.0f;
        break;
      case ExcelLineStyle.Thin:
      case ExcelLineStyle.Dashed:
      case ExcelLineStyle.Dotted:
      case ExcelLineStyle.Dash_dot:
      case ExcelLineStyle.Dash_dot_dot:
      case ExcelLineStyle.Slanted_dash_dot:
        borderWidth = 1f;
        break;
      case ExcelLineStyle.Medium:
      case ExcelLineStyle.Medium_dashed:
      case ExcelLineStyle.Medium_dash_dot:
      case ExcelLineStyle.Medium_dash_dot_dot:
        borderWidth = 2f;
        break;
      case ExcelLineStyle.Thick:
        borderWidth = 3f;
        break;
      case ExcelLineStyle.Double:
        borderWidth = 3.5f;
        break;
      case ExcelLineStyle.Hair:
        borderWidth = 0.5f;
        break;
    }
    return borderWidth;
  }

  protected ColorObject GetLeftBorderColor(bool askAdjacent)
  {
    ColorObject leftBorderColor = base.LeftBorderColor;
    if (base.LeftBorderLineStyle == ExcelLineStyle.None && askAdjacent)
    {
      IRange leftCell = this.GetLeftCell();
      if (leftCell != null)
        leftBorderColor = (leftCell.CellStyle as CellStyle).GetRightBorderColor(false);
    }
    return leftBorderColor;
  }

  protected ColorObject GetRightBorderColor(bool askAdjacent)
  {
    ColorObject rightBorderColor = base.RightBorderColor;
    if (base.RightBorderLineStyle == ExcelLineStyle.None && askAdjacent)
    {
      IRange rightCell = this.GetRightCell();
      if (rightCell != null)
        rightBorderColor = (rightCell.CellStyle as CellStyle).GetLeftBorderColor(false);
    }
    return rightBorderColor;
  }

  protected ColorObject GetTopBorderColor(bool askAdjecent)
  {
    ColorObject topBorderColor = base.TopBorderColor;
    if (base.TopBorderLineStyle == ExcelLineStyle.None && askAdjecent)
    {
      IRange topCell = this.GetTopCell();
      if (topCell != null)
        topBorderColor = (topCell.CellStyle as CellStyle).GetBottomBorderColor(false);
    }
    return topBorderColor;
  }

  protected ColorObject GetBottomBorderColor(bool askAdjecent)
  {
    ColorObject bottomBorderColor = base.BottomBorderColor;
    if (base.BottomBorderLineStyle == ExcelLineStyle.None && askAdjecent)
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
      cell = this.m_range.Worksheet[row, column];
    return cell;
  }
}
