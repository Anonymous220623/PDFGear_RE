// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.Grouping.BorderGroup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections.Grouping;

internal class BorderGroup : CommonObject, IBorder, IParentApplication
{
  private OfficeBordersIndex m_index;
  private BordersGroup m_bordersGroup;

  public BorderGroup(IApplication application, object parent, OfficeBordersIndex index)
    : base(application, parent)
  {
    this.m_index = index;
    this.FindParents();
  }

  private void FindParents()
  {
    this.m_bordersGroup = this.FindParent(typeof (BordersGroup)) as BordersGroup;
    if (this.m_bordersGroup == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent borders group.");
  }

  public IBorder this[int index] => this.m_bordersGroup[index][this.m_index];

  public int Count => this.m_bordersGroup.GroupCount;

  public OfficeKnownColors Color
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return OfficeKnownColors.Black;
      OfficeKnownColors color = this[0].Color;
      for (int index = 1; index < count; ++index)
      {
        if (color != this[index].Color)
          return OfficeKnownColors.Black;
      }
      return color;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Color = value;
    }
  }

  public ChartColor ColorObject => throw new NotImplementedException();

  public System.Drawing.Color ColorRGB
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ColorExtension.Empty;
      System.Drawing.Color colorRgb = this[0].ColorRGB;
      for (int index = 1; index < count; ++index)
      {
        if (colorRgb != this[index].ColorRGB)
          return ColorExtension.Empty;
      }
      return colorRgb;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].ColorRGB = value;
    }
  }

  public OfficeLineStyle LineStyle
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return OfficeLineStyle.None;
      OfficeLineStyle lineStyle = this[0].LineStyle;
      for (int index = 1; index < count; ++index)
      {
        if (lineStyle != this[index].LineStyle)
          return OfficeLineStyle.None;
      }
      return lineStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].LineStyle = value;
    }
  }

  public bool ShowDiagonalLine
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool showDiagonalLine = this[0].ShowDiagonalLine;
      for (int index = 1; index < count; ++index)
      {
        if (showDiagonalLine != this[index].ShowDiagonalLine)
          return false;
      }
      return showDiagonalLine;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].ShowDiagonalLine = value;
    }
  }
}
