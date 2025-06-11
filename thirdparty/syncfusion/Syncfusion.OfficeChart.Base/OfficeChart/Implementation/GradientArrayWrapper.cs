// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.GradientArrayWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class GradientArrayWrapper : CommonObject, IGradient
{
  private List<IRange> m_arrCells = new List<IRange>();

  public GradientArrayWrapper(IRange range)
    : base((range as RangeImpl).Application, (object) range)
  {
    this.m_arrCells.AddRange((IEnumerable<IRange>) range.Cells);
  }

  public ChartColor BackColorObject => throw new NotImplementedException();

  public Color BackColor
  {
    get
    {
      int num = 0;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          num = arrCell.CellStyle.Interior.Gradient.BackColor.ToArgb();
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.Gradient.BackColor.ToArgb() != num)
          return ColorExtension.Empty;
      }
      return ColorExtension.FromArgb(num);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.Gradient.BackColor = value;
    }
  }

  public OfficeKnownColors BackColorIndex
  {
    get
    {
      OfficeKnownColors backColorIndex = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          backColorIndex = arrCell.CellStyle.Interior.Gradient.BackColorIndex;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.Gradient.BackColorIndex != backColorIndex)
          return OfficeKnownColors.Black;
      }
      return backColorIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.Gradient.BackColorIndex = value;
    }
  }

  public ChartColor ForeColorObject => throw new NotImplementedException();

  public Color ForeColor
  {
    get
    {
      int num = 0;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          num = arrCell.CellStyle.Interior.Gradient.ForeColor.ToArgb();
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.Gradient.ForeColor.ToArgb() != num)
          return ColorExtension.Empty;
      }
      return ColorExtension.FromArgb(num);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.Gradient.ForeColor = value;
    }
  }

  public OfficeKnownColors ForeColorIndex
  {
    get
    {
      OfficeKnownColors foreColorIndex = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          foreColorIndex = arrCell.CellStyle.Interior.Gradient.ForeColorIndex;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.Gradient.ForeColorIndex != foreColorIndex)
          return OfficeKnownColors.Black;
      }
      return foreColorIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.Gradient.ForeColorIndex = value;
    }
  }

  public OfficeGradientStyle GradientStyle
  {
    get
    {
      OfficeGradientStyle gradientStyle = OfficeGradientStyle.Horizontal;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          gradientStyle = arrCell.CellStyle.Interior.Gradient.GradientStyle;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.Gradient.GradientStyle != gradientStyle)
          return OfficeGradientStyle.Horizontal;
      }
      return gradientStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.Gradient.GradientStyle = value;
    }
  }

  public OfficeGradientVariants GradientVariant
  {
    get
    {
      OfficeGradientVariants gradientVariant = OfficeGradientVariants.ShadingVariants_1;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          gradientVariant = arrCell.CellStyle.Interior.Gradient.GradientVariant;
          flag = false;
        }
        else if (arrCell.CellStyle.Interior.Gradient.GradientVariant != gradientVariant)
          return OfficeGradientVariants.ShadingVariants_1;
      }
      return gradientVariant;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Interior.Gradient.GradientVariant = value;
    }
  }

  public int CompareTo(IGradient gradient)
  {
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
    {
      if (this.m_arrCells[index].CellStyle.Interior.Gradient.CompareTo(gradient) != 0)
        return 1;
    }
    return 0;
  }

  public void TwoColorGradient()
  {
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
      this.m_arrCells[index].CellStyle.Interior.Gradient.TwoColorGradient();
  }

  public void TwoColorGradient(OfficeGradientStyle style, OfficeGradientVariants variant)
  {
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
      this.m_arrCells[index].CellStyle.Interior.Gradient.TwoColorGradient(style, variant);
  }

  public void BeginUpdate()
  {
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
      ((CommonWrapper) this.m_arrCells[index].CellStyle.Interior.Gradient).BeginUpdate();
  }

  public void EndUpdate()
  {
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
      ((CommonWrapper) this.m_arrCells[index].CellStyle.Interior.Gradient).EndUpdate();
  }
}
