// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.Grouping.BorderGroup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections.Grouping;

public class BorderGroup : CommonObject, IBorder, IParentApplication
{
  private ExcelBordersIndex m_index;
  private BordersGroup m_bordersGroup;

  public BorderGroup(IApplication application, object parent, ExcelBordersIndex index)
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

  public ExcelKnownColors Color
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelKnownColors.None;
      ExcelKnownColors color = this[0].Color;
      for (int index = 1; index < count; ++index)
      {
        if (color != this[index].Color)
          return ExcelKnownColors.None;
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

  public ColorObject ColorObject => throw new NotImplementedException();

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

  public ExcelLineStyle LineStyle
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelLineStyle.None;
      ExcelLineStyle lineStyle = this[0].LineStyle;
      for (int index = 1; index < count; ++index)
      {
        if (lineStyle != this[index].LineStyle)
          return ExcelLineStyle.None;
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
