// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.BordersCollectionArrayWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class BordersCollectionArrayWrapper : 
  CollectionBaseEx<object>,
  IBorders,
  IEnumerable,
  IParentApplication
{
  private System.Collections.Generic.List<IRange> m_arrCells = new System.Collections.Generic.List<IRange>();
  private WorkbookImpl m_book;
  private IApplication m_application;

  public BordersCollectionArrayWrapper(IRange range)
    : base((range as RangeImpl).Application, (object) range)
  {
    this.m_arrCells.AddRange((IEnumerable<IRange>) range.Cells);
  }

  public BordersCollectionArrayWrapper(System.Collections.Generic.List<IRange> lstRange, IApplication application)
    : base(application, (object) lstRange[0])
  {
    this.m_arrCells = lstRange;
    this.m_application = application;
  }

  public OfficeKnownColors Color
  {
    get
    {
      OfficeKnownColors color = this.m_arrCells[0].Borders.Color;
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (this.m_arrCells[index].Borders.Color != color)
          return OfficeKnownColors.Black;
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

  public IBorder this[OfficeBordersIndex Index]
  {
    get
    {
      RangeImpl parent = (IRange) this.Parent as RangeImpl;
      return parent.IsEntireRow || parent.IsEntireColumn ? (IBorder) new BorderImplArrayWrapper(this.m_arrCells, Index, this.m_application) : (IBorder) new BorderImplArrayWrapper((IRange) this.Parent, Index);
    }
  }

  public OfficeLineStyle LineStyle
  {
    get
    {
      OfficeLineStyle lineStyle = this.m_arrCells[0].Borders.LineStyle;
      for (int index = 1; index < this.m_arrCells.Count; ++index)
      {
        if (lineStyle != this.m_arrCells[index].Borders.LineStyle)
          return OfficeLineStyle.None;
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

  public OfficeLineStyle Value
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
