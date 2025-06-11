// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.Grouping.BordersGroup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections.Grouping;

internal class BordersGroup : CollectionBaseEx<object>, IBorders, IEnumerable, IParentApplication
{
  private StyleGroup m_style;

  public BordersGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.InnerList.Add((object) new BorderGroup(application, (object) this, OfficeBordersIndex.DiagonalDown));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, OfficeBordersIndex.DiagonalUp));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, OfficeBordersIndex.EdgeBottom));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, OfficeBordersIndex.EdgeLeft));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, OfficeBordersIndex.EdgeRight));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, OfficeBordersIndex.EdgeTop));
  }

  private void FindParents()
  {
    this.m_style = this.FindParent(typeof (StyleGroup)) as StyleGroup;
    if (this.m_style == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent style group.");
  }

  public IBorders this[int index] => this.m_style[index].Borders;

  public int GroupCount => this.m_style.Count;

  public OfficeKnownColors Color
  {
    get
    {
      int groupCount = this.GroupCount;
      if (groupCount == 0)
        return OfficeKnownColors.Black;
      OfficeKnownColors color = this[0].Color;
      for (int index = 1; index < groupCount; ++index)
      {
        if (color != this[index].Color)
          return OfficeKnownColors.Black;
      }
      return color;
    }
    set
    {
      int index = 0;
      for (int groupCount = this.GroupCount; index < groupCount; ++index)
        this[index].Color = value;
    }
  }

  public System.Drawing.Color ColorRGB
  {
    get
    {
      int groupCount = this.GroupCount;
      if (groupCount == 0)
        return ColorExtension.Empty;
      System.Drawing.Color colorRgb = this[0].ColorRGB;
      for (int index = 1; index < groupCount; ++index)
      {
        if (colorRgb != this[index].ColorRGB)
          return ColorExtension.Empty;
      }
      return colorRgb;
    }
    set
    {
      int index = 0;
      for (int groupCount = this.GroupCount; index < groupCount; ++index)
        this[index].ColorRGB = value;
    }
  }

  int IBorders.Count
  {
    get
    {
      int groupCount = this.GroupCount;
      if (groupCount == 0)
        return int.MinValue;
      int count = this[0].Count;
      for (int index = 1; index < groupCount; ++index)
      {
        if (count != this[index].Count)
          return int.MinValue;
      }
      return count;
    }
  }

  public IBorder this[OfficeBordersIndex Index] => (IBorder) null;

  public OfficeLineStyle LineStyle
  {
    get
    {
      int groupCount = this.GroupCount;
      if (groupCount == 0)
        return OfficeLineStyle.None;
      OfficeLineStyle lineStyle = this[0].LineStyle;
      for (int index = 1; index < groupCount; ++index)
      {
        if (lineStyle != this[index].LineStyle)
          return OfficeLineStyle.None;
      }
      return lineStyle;
    }
    set
    {
      int index = 0;
      for (int groupCount = this.GroupCount; index < groupCount; ++index)
        this[index].LineStyle = value;
    }
  }

  public OfficeLineStyle Value
  {
    get
    {
      int groupCount = this.GroupCount;
      if (groupCount == 0)
        return OfficeLineStyle.None;
      OfficeLineStyle officeLineStyle = this[0].Value;
      for (int index = 1; index < groupCount; ++index)
      {
        if (officeLineStyle != this[index].Value)
          return OfficeLineStyle.None;
      }
      return officeLineStyle;
    }
    set
    {
      int index = 0;
      for (int groupCount = this.GroupCount; index < groupCount; ++index)
        this[index].Value = value;
    }
  }
}
