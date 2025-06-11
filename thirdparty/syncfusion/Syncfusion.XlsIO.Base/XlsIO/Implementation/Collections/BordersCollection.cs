// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.BordersCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class BordersCollection : CollectionBaseEx<IBorder>, IBorders, IEnumerable, IParentApplication
{
  private WorkbookImpl m_book;
  private bool m_bIsEmptyBorder = true;

  public ExcelKnownColors Color
  {
    get
    {
      ExcelKnownColors color = this.InnerList[0].Color;
      for (int index = 1; index < this.Count; ++index)
      {
        if (color != this.InnerList[index].Color)
          return ExcelKnownColors.None;
      }
      return color;
    }
    set
    {
      for (int index = 0; index < this.Count; ++index)
        this.InnerList[index].Color = value;
    }
  }

  public System.Drawing.Color ColorRGB
  {
    get
    {
      System.Drawing.Color colorRgb = this.InnerList[0].ColorRGB;
      int argb = colorRgb.ToArgb();
      for (int index = 1; index < this.Count; ++index)
      {
        if (argb != this.InnerList[index].ColorRGB.ToArgb())
        {
          colorRgb = ColorExtension.Empty;
          break;
        }
      }
      return colorRgb;
    }
    set
    {
      for (int index = 0; index < this.Count; ++index)
        this.InnerList[index].ColorRGB = value;
    }
  }

  public IBorder this[ExcelBordersIndex index]
  {
    get
    {
      switch (index)
      {
        case ExcelBordersIndex.DiagonalDown:
          return this.InnerList[0];
        case ExcelBordersIndex.DiagonalUp:
          return this.InnerList[1];
        case ExcelBordersIndex.EdgeLeft:
          return this.InnerList[3];
        case ExcelBordersIndex.EdgeTop:
          return this.InnerList[5];
        case ExcelBordersIndex.EdgeBottom:
          return this.InnerList[2];
        case ExcelBordersIndex.EdgeRight:
          return this.InnerList[4];
        default:
          return (IBorder) null;
      }
    }
  }

  public ExcelLineStyle LineStyle
  {
    get
    {
      ExcelLineStyle lineStyle = this.InnerList[0].LineStyle;
      for (int index = 1; index < this.Count; ++index)
      {
        if (lineStyle != this.InnerList[index].LineStyle)
          return ExcelLineStyle.None;
      }
      return lineStyle;
    }
    set
    {
      for (int index = 0; index < this.Count; ++index)
        this.InnerList[index].LineStyle = value;
    }
  }

  public ExcelLineStyle Value
  {
    get => this.LineStyle;
    set => this.LineStyle = value;
  }

  internal bool IsEmptyBorder
  {
    get => this.m_bIsEmptyBorder;
    set => this.m_bIsEmptyBorder = value;
  }

  internal BordersCollection(IApplication application, object parent, bool bAddEmpty)
    : base(application, parent)
  {
    this.SetParents();
    if (!bAddEmpty)
      return;
    this.InnerList.AddRange((IEnumerable<IBorder>) new BorderImpl[6]);
  }

  public BordersCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public BordersCollection(IApplication application, object parent, IInternalExtendedFormat wrap)
    : this(application, parent, false)
  {
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, ExcelBordersIndex.DiagonalDown));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, ExcelBordersIndex.DiagonalUp));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, ExcelBordersIndex.EdgeBottom));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, ExcelBordersIndex.EdgeLeft));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, ExcelBordersIndex.EdgeRight));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, ExcelBordersIndex.EdgeTop));
  }

  public override bool Equals(object obj)
  {
    if (!(obj is BordersCollection bordersCollection))
      return false;
    System.Collections.Generic.List<IBorder> innerList1 = this.InnerList;
    System.Collections.Generic.List<IBorder> innerList2 = bordersCollection.InnerList;
    bool flag = true;
    int index = 0;
    for (int count = this.Count; index < count && flag; ++index)
    {
      if (!innerList1[index].Equals((object) innerList2[index]))
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public override int GetHashCode()
  {
    int hashCode = 0;
    System.Collections.Generic.List<IBorder> innerList = this.InnerList;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      hashCode ^= innerList[index].GetHashCode();
    return hashCode;
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  internal void SetBorder(ExcelBordersIndex index, IBorder border)
  {
    switch (index)
    {
      case ExcelBordersIndex.DiagonalDown:
        this.InnerList[0] = border;
        break;
      case ExcelBordersIndex.DiagonalUp:
        this.InnerList[1] = border;
        break;
      case ExcelBordersIndex.EdgeLeft:
        this.InnerList[3] = border;
        break;
      case ExcelBordersIndex.EdgeTop:
        this.InnerList[5] = border;
        break;
      case ExcelBordersIndex.EdgeBottom:
        this.InnerList[2] = border;
        break;
      case ExcelBordersIndex.EdgeRight:
        this.InnerList[4] = border;
        break;
      default:
        this.Add(border);
        break;
    }
  }

  internal void Dispose()
  {
    foreach (BorderImpl inner in this.InnerList)
      inner.Clear();
  }
}
