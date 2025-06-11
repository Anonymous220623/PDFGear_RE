// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.InteriorArrayWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class InteriorArrayWrapper : CommonObject, IInterior
{
  private List<IRange> m_arrCells = new List<IRange>();

  public InteriorArrayWrapper(IRange range)
    : base((range as RangeImpl).Application, (object) range)
  {
    this.m_arrCells.AddRange((IEnumerable<IRange>) range.Cells);
  }

  public OfficeKnownColors PatternColorIndex
  {
    get
    {
      OfficeKnownColors patternColorIndex = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          patternColorIndex = arrCell.CellStyle.Interior.PatternColorIndex;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.PatternColorIndex != patternColorIndex)
          return OfficeKnownColors.Black;
      }
      return patternColorIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.PatternColorIndex = value;
    }
  }

  public Color PatternColor
  {
    get
    {
      Color patternColor = ColorExtension.Empty;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          patternColor = arrCell.CellStyle.Interior.PatternColor;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.PatternColor != patternColor)
          return ColorExtension.Empty;
      }
      return patternColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.PatternColor = value;
    }
  }

  public OfficeKnownColors ColorIndex
  {
    get
    {
      OfficeKnownColors colorIndex = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          colorIndex = arrCell.CellStyle.Interior.ColorIndex;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.ColorIndex != colorIndex)
          return OfficeKnownColors.Black;
      }
      return colorIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.ColorIndex = value;
    }
  }

  public Color Color
  {
    get
    {
      Color color = ColorExtension.Empty;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          color = arrCell.CellStyle.Interior.Color;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.Color != color)
          return ColorExtension.Empty;
      }
      return color;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.Color = value;
    }
  }

  public IGradient Gradient
  {
    get
    {
      IGradient gradient = (IGradient) null;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          gradient = arrCell.CellStyle.Interior.Gradient;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.Gradient != gradient)
          return (IGradient) new GradientArrayWrapper((IRange) this.Parent);
      }
      return gradient;
    }
  }

  public OfficePattern FillPattern
  {
    get
    {
      OfficePattern fillPattern = OfficePattern.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          fillPattern = arrCell.CellStyle.Interior.FillPattern;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.FillPattern != fillPattern)
          return OfficePattern.None;
      }
      return fillPattern;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.FillPattern = value;
    }
  }

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }
}
