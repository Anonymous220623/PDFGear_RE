// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.GradientArrayWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class GradientArrayWrapper : CommonObject, IGradient
{
  private List<IRange> m_arrCells = new List<IRange>();

  public GradientArrayWrapper(IRange range)
    : base(range.Application, (object) range)
  {
    this.m_arrCells.AddRange((IEnumerable<IRange>) range.Cells);
  }

  public ColorObject BackColorObject => throw new NotImplementedException();

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

  public ExcelKnownColors BackColorIndex
  {
    get
    {
      ExcelKnownColors backColorIndex = ExcelKnownColors.None;
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
          return ExcelKnownColors.None;
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

  public ColorObject ForeColorObject => throw new NotImplementedException();

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

  public ExcelKnownColors ForeColorIndex
  {
    get
    {
      ExcelKnownColors foreColorIndex = ExcelKnownColors.None;
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
          return ExcelKnownColors.None;
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

  public ExcelGradientStyle GradientStyle
  {
    get
    {
      ExcelGradientStyle gradientStyle = ExcelGradientStyle.Horizontal;
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
          return ExcelGradientStyle.Horizontal;
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

  public ExcelGradientVariants GradientVariant
  {
    get
    {
      ExcelGradientVariants gradientVariant = ExcelGradientVariants.ShadingVariants_1;
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
          return ExcelGradientVariants.ShadingVariants_1;
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

  public void TwoColorGradient(ExcelGradientStyle style, ExcelGradientVariants variant)
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
