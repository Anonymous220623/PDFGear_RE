// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.BorderImplArrayWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class BorderImplArrayWrapper : CommonObject, IBorder, IParentApplication
{
  private List<IRange> m_arrCells = new List<IRange>();
  private OfficeBordersIndex m_border;
  private WorkbookImpl m_book;

  public BorderImplArrayWrapper(IRange range, OfficeBordersIndex index)
    : base((range as RangeImpl).Application, (object) range)
  {
    this.m_border = index;
    this.m_arrCells.AddRange((IEnumerable<IRange>) range.Cells);
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook");
  }

  public BorderImplArrayWrapper(
    List<IRange> lstRange,
    OfficeBordersIndex index,
    IApplication application)
    : base(application, (object) lstRange[0])
  {
    this.m_border = index;
    this.m_arrCells = lstRange;
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook");
  }

  public OfficeKnownColors Color
  {
    get
    {
      OfficeKnownColors color = this.m_arrCells[0].Borders[this.m_border].Color;
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (color != this.m_arrCells[index].Borders[this.m_border].Color)
          return OfficeKnownColors.Black;
      }
      return color;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].Borders[this.m_border].Color = value;
    }
  }

  public ChartColor ColorObject
  {
    get
    {
      ChartColor colorObject = this.m_arrCells[0].Borders[this.m_border].ColorObject;
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (colorObject != this.m_arrCells[index].Borders[this.m_border].ColorObject)
          return (ChartColor) null;
      }
      return colorObject;
    }
  }

  public System.Drawing.Color ColorRGB
  {
    get
    {
      return this.ColorObject == (ChartColor) null ? this.m_arrCells[0].Borders[this.m_border].ColorObject.GetRGB((IWorkbook) this.m_book) : this.ColorObject.GetRGB((IWorkbook) this.m_book);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].Borders[this.m_border].ColorRGB = value;
    }
  }

  public OfficeLineStyle LineStyle
  {
    get
    {
      OfficeLineStyle lineStyle = this.m_arrCells[0].Borders[this.m_border].LineStyle;
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (lineStyle != this.m_arrCells[index].Borders[this.m_border].LineStyle)
          return OfficeLineStyle.None;
      }
      return lineStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].Borders[this.m_border].LineStyle = value;
    }
  }

  public bool ShowDiagonalLine
  {
    get
    {
      bool showDiagonalLine = this.m_arrCells[0].Borders[this.m_border].ShowDiagonalLine;
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (showDiagonalLine != this.m_arrCells[index].Borders[this.m_border].ShowDiagonalLine)
          return false;
      }
      return showDiagonalLine;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].Borders[this.m_border].ShowDiagonalLine = value;
    }
  }
}
