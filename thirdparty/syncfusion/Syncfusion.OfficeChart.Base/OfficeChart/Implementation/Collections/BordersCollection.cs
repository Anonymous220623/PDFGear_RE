// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.BordersCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class BordersCollection : 
  CollectionBaseEx<IBorder>,
  IBorders,
  IEnumerable,
  IParentApplication
{
  private WorkbookImpl m_book;
  private bool m_bIsEmptyBorder = true;

  public OfficeKnownColors Color
  {
    get
    {
      OfficeKnownColors color = this.InnerList[0].Color;
      for (int index = 1; index < this.Count; ++index)
      {
        if (color != this.InnerList[index].Color)
          return OfficeKnownColors.Black;
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

  public IBorder this[OfficeBordersIndex index]
  {
    get
    {
      switch (index)
      {
        case OfficeBordersIndex.DiagonalDown:
          return this.InnerList[0];
        case OfficeBordersIndex.DiagonalUp:
          return this.InnerList[1];
        case OfficeBordersIndex.EdgeLeft:
          return this.InnerList[3];
        case OfficeBordersIndex.EdgeTop:
          return this.InnerList[5];
        case OfficeBordersIndex.EdgeBottom:
          return this.InnerList[2];
        case OfficeBordersIndex.EdgeRight:
          return this.InnerList[4];
        default:
          return (IBorder) null;
      }
    }
  }

  public OfficeLineStyle LineStyle
  {
    get
    {
      OfficeLineStyle lineStyle = this.InnerList[0].LineStyle;
      for (int index = 1; index < this.Count; ++index)
      {
        if (lineStyle != this.InnerList[index].LineStyle)
          return OfficeLineStyle.None;
      }
      return lineStyle;
    }
    set
    {
      for (int index = 0; index < this.Count; ++index)
        this.InnerList[index].LineStyle = value;
    }
  }

  public OfficeLineStyle Value
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

  public BordersCollection(IApplication application, object parent, IInternalExtendedFormat wrap)
    : this(application, parent, false)
  {
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, OfficeBordersIndex.DiagonalDown));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, OfficeBordersIndex.DiagonalUp));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, OfficeBordersIndex.EdgeBottom));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, OfficeBordersIndex.EdgeLeft));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, OfficeBordersIndex.EdgeRight));
    this.InnerList.Add((IBorder) new BorderImpl(application, (object) this, wrap, OfficeBordersIndex.EdgeTop));
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

  internal void SetBorder(OfficeBordersIndex index, IBorder border)
  {
    switch (index)
    {
      case OfficeBordersIndex.DiagonalDown:
        this.InnerList[0] = border;
        break;
      case OfficeBordersIndex.DiagonalUp:
        this.InnerList[1] = border;
        break;
      case OfficeBordersIndex.EdgeLeft:
        this.InnerList[3] = border;
        break;
      case OfficeBordersIndex.EdgeTop:
        this.InnerList[5] = border;
        break;
      case OfficeBordersIndex.EdgeBottom:
        this.InnerList[2] = border;
        break;
      case OfficeBordersIndex.EdgeRight:
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
