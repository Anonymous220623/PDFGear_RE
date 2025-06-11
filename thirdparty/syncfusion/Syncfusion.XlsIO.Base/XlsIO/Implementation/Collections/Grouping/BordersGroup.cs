// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.Grouping.BordersGroup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections.Grouping;

public class BordersGroup : CollectionBaseEx<object>, IBorders, IEnumerable, IParentApplication
{
  private StyleGroup m_style;

  public BordersGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.InnerList.Add((object) new BorderGroup(application, (object) this, ExcelBordersIndex.DiagonalDown));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, ExcelBordersIndex.DiagonalUp));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, ExcelBordersIndex.EdgeBottom));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, ExcelBordersIndex.EdgeLeft));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, ExcelBordersIndex.EdgeRight));
    this.InnerList.Add((object) new BorderGroup(application, (object) this, ExcelBordersIndex.EdgeTop));
  }

  private void FindParents()
  {
    this.m_style = this.FindParent(typeof (StyleGroup)) as StyleGroup;
    if (this.m_style == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent style group.");
  }

  public IBorders this[int index] => this.m_style[index].Borders;

  public int GroupCount => this.m_style.Count;

  public ExcelKnownColors Color
  {
    get
    {
      int groupCount = this.GroupCount;
      if (groupCount == 0)
        return ExcelKnownColors.None;
      ExcelKnownColors color = this[0].Color;
      for (int index = 1; index < groupCount; ++index)
      {
        if (color != this[index].Color)
          return ExcelKnownColors.None;
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

  public IBorder this[ExcelBordersIndex Index] => (IBorder) null;

  public ExcelLineStyle LineStyle
  {
    get
    {
      int groupCount = this.GroupCount;
      if (groupCount == 0)
        return ExcelLineStyle.None;
      ExcelLineStyle lineStyle = this[0].LineStyle;
      for (int index = 1; index < groupCount; ++index)
      {
        if (lineStyle != this[index].LineStyle)
          return ExcelLineStyle.None;
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

  public ExcelLineStyle Value
  {
    get
    {
      int groupCount = this.GroupCount;
      if (groupCount == 0)
        return ExcelLineStyle.None;
      ExcelLineStyle excelLineStyle = this[0].Value;
      for (int index = 1; index < groupCount; ++index)
      {
        if (excelLineStyle != this[index].Value)
          return ExcelLineStyle.None;
      }
      return excelLineStyle;
    }
    set
    {
      int index = 0;
      for (int groupCount = this.GroupCount; index < groupCount; ++index)
        this[index].Value = value;
    }
  }
}
