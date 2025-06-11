// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.BordersCollectionArrayWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class BordersCollectionArrayWrapper : 
  CollectionBaseEx<object>,
  IBorders,
  IEnumerable,
  IParentApplication
{
  private const int DEF_BORDERS_COUNT = 6;
  private System.Collections.Generic.List<IRange> m_arrCells = new System.Collections.Generic.List<IRange>();
  private WorkbookImpl m_book;
  private IApplication m_application;

  public BordersCollectionArrayWrapper(IRange range)
    : base(range.Application, (object) range)
  {
    this.m_arrCells.AddRange((IEnumerable<IRange>) range.Cells);
  }

  public BordersCollectionArrayWrapper(System.Collections.Generic.List<IRange> lstRange, IApplication application)
    : base(application, (object) lstRange[0])
  {
    this.m_arrCells = lstRange;
    this.m_application = application;
  }

  public ExcelKnownColors Color
  {
    get
    {
      ExcelKnownColors color = this.m_arrCells[0].Borders.Color;
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (this.m_arrCells[index].Borders.Color != color)
          return ExcelKnownColors.None;
      }
      return color;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].Borders.Color = value;
    }
  }

  public System.Drawing.Color ColorRGB
  {
    get
    {
      System.Drawing.Color colorRgb = this.m_arrCells[0].Borders.ColorRGB;
      int argb = colorRgb.ToArgb();
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (this.m_arrCells[index].Borders.ColorRGB.ToArgb() != argb)
        {
          colorRgb = ColorExtension.Empty;
          break;
        }
      }
      return colorRgb;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].Borders.ColorRGB = value;
    }
  }

  public IBorder this[ExcelBordersIndex Index]
  {
    get
    {
      RangeImpl parent = (IRange) this.Parent as RangeImpl;
      return parent.IsEntireRow || parent.IsEntireColumn ? (IBorder) new BorderImplArrayWrapper(this.m_arrCells, Index, this.m_application) : (IBorder) new BorderImplArrayWrapper((IRange) this.Parent, Index);
    }
  }

  public ExcelLineStyle LineStyle
  {
    get
    {
      ExcelLineStyle lineStyle = this.m_arrCells[0].Borders.LineStyle;
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (lineStyle != this.m_arrCells[index].Borders.LineStyle)
          return ExcelLineStyle.None;
      }
      return lineStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].Borders.LineStyle = value;
    }
  }

  public new int Count => 6;

  public ExcelLineStyle Value
  {
    get => this.LineStyle;
    set => this.LineStyle = value;
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }
}
