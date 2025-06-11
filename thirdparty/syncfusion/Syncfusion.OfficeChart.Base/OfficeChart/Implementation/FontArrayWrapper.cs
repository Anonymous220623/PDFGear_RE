// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.FontArrayWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class FontArrayWrapper : CommonObject, IOfficeFont, IParentApplication, IOptimizedUpdate
{
  private List<IRange> m_arrCells = new List<IRange>();

  public FontArrayWrapper(IRange range)
    : base((range as RangeImpl).Application, (object) range)
  {
    this.m_arrCells.AddRange((IEnumerable<IRange>) range.Cells);
  }

  public FontArrayWrapper(List<IRange> lstRange, IApplication application)
    : base(application, (object) lstRange[0])
  {
    this.m_arrCells = lstRange;
  }

  public bool Italic
  {
    get
    {
      bool italic = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          italic = arrCell.CellStyle.Font.Italic;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.Italic != italic)
          return false;
      }
      return italic;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.Italic = value;
    }
  }

  public OfficeKnownColors Color
  {
    get
    {
      OfficeKnownColors color = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          color = arrCell.CellStyle.Font.Color;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.Color != color)
          return OfficeKnownColors.Black;
      }
      return color;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.Color = value;
    }
  }

  public System.Drawing.Color RGBColor
  {
    get => this.m_arrCells[0].Worksheet.Workbook.GetPaletteColor(this.Color);
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.RGBColor = value;
    }
  }

  public bool Bold
  {
    get
    {
      bool bold = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          bold = arrCell.CellStyle.Font.Bold;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.Bold != bold)
          return false;
      }
      return bold;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.Bold = value;
    }
  }

  public bool MacOSOutlineFont
  {
    get
    {
      bool macOsOutlineFont = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          macOsOutlineFont = arrCell.CellStyle.Font.MacOSOutlineFont;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.MacOSOutlineFont != macOsOutlineFont)
          return false;
      }
      return macOsOutlineFont;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.MacOSOutlineFont = value;
    }
  }

  public bool MacOSShadow
  {
    get
    {
      bool macOsShadow = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          macOsShadow = arrCell.CellStyle.Font.MacOSShadow;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.MacOSShadow != macOsShadow)
          return false;
      }
      return macOsShadow;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.MacOSShadow = value;
    }
  }

  public double Size
  {
    get
    {
      double size = 0.0;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          size = arrCell.CellStyle.Font.Size;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.Size != size)
          return 0.0;
      }
      return size;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.Size = value;
    }
  }

  public bool Strikethrough
  {
    get
    {
      bool strikethrough = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          strikethrough = arrCell.CellStyle.Font.Strikethrough;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.Strikethrough != strikethrough)
          return false;
      }
      return strikethrough;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.Strikethrough = value;
    }
  }

  public bool Subscript
  {
    get
    {
      bool subscript = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          subscript = arrCell.CellStyle.Font.Subscript;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.Subscript != subscript)
          return false;
      }
      return subscript;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.Subscript = value;
    }
  }

  public bool Superscript
  {
    get
    {
      bool superscript = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          superscript = arrCell.CellStyle.Font.Superscript;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.Superscript != superscript)
          return false;
      }
      return superscript;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.Superscript = value;
    }
  }

  public OfficeUnderline Underline
  {
    get
    {
      OfficeUnderline underline = OfficeUnderline.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          underline = arrCell.CellStyle.Font.Underline;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.Underline != underline)
          return OfficeUnderline.None;
      }
      return underline;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.Underline = value;
    }
  }

  public string FontName
  {
    get
    {
      string fontName = (string) null;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          fontName = arrCell.CellStyle.Font.FontName;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.FontName != fontName)
          return (string) null;
      }
      return fontName;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.FontName = value;
    }
  }

  public OfficeFontVerticalAlignment VerticalAlignment
  {
    get
    {
      OfficeFontVerticalAlignment verticalAlignment = OfficeFontVerticalAlignment.Baseline;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
      {
        IRange arrCell = this.m_arrCells[index];
        if (flag)
        {
          verticalAlignment = arrCell.CellStyle.Font.VerticalAlignment;
          flag = false;
        }
        else if (arrCell.CellStyle.Font.VerticalAlignment != verticalAlignment)
        {
          verticalAlignment = OfficeFontVerticalAlignment.Baseline;
          break;
        }
      }
      return verticalAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrCells.Count; index < count; ++index)
        this.m_arrCells[index].CellStyle.Font.VerticalAlignment = value;
    }
  }

  public Font GenerateNativeFont() => this.m_arrCells[0].CellStyle.Font.GenerateNativeFont();

  public bool IsAutoColor => false;

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }
}
