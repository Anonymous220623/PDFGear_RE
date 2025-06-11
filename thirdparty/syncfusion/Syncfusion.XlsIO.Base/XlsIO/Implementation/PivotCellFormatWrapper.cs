// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotCellFormatWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.PivotTables;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class PivotCellFormatWrapper : IInternalPivotCellFormat, IPivotCellFormat
{
  private List<PivotFormat> m_arrPivotFormats;
  private List<PivotCellFormat> m_arrPivotCellFormats = new List<PivotCellFormat>();
  private WorksheetImpl m_worksheet;

  public ExcelKnownColors BackColor
  {
    get
    {
      ExcelKnownColors backColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          backColor = arrPivotCellFormat.BackColor;
          flag = false;
        }
        else if (arrPivotCellFormat.BackColor != backColor)
          return ExcelKnownColors.None;
      }
      return backColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].BackColor = value;
    }
  }

  public Color BackColorRGB
  {
    get
    {
      return this.BackColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].BackColorObject.GetRGB(this.m_worksheet.Workbook) : this.BackColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].BackColorRGB = value;
    }
  }

  public ExcelPattern PatternStyle
  {
    get
    {
      ExcelPattern patternStyle = ExcelPattern.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          patternStyle = arrPivotCellFormat.PatternStyle;
          flag = false;
        }
        else if (arrPivotCellFormat.PatternStyle != patternStyle)
          return ExcelPattern.None;
      }
      return patternStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].PatternStyle = value;
    }
  }

  public ExcelKnownColors PatternColor
  {
    get
    {
      ExcelKnownColors patternColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          patternColor = arrPivotCellFormat.PatternColor;
          flag = false;
        }
        else if (arrPivotCellFormat.PatternColor != patternColor)
          return ExcelKnownColors.None;
      }
      return patternColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].PatternColor = value;
    }
  }

  public Color PatternColorRGB
  {
    get
    {
      return this.ColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].ColorObject.GetRGB(this.m_worksheet.Workbook) : this.ColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].PatternColorRGB = value;
    }
  }

  public ExcelKnownColors FontColor
  {
    get
    {
      ExcelKnownColors fontColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          fontColor = arrPivotCellFormat.FontColor;
          flag = false;
        }
        else if (arrPivotCellFormat.FontColor != fontColor)
          return ExcelKnownColors.None;
      }
      return fontColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].FontColor = value;
    }
  }

  public Color FontColorRGB
  {
    get
    {
      return this.FontColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].FontColorObject.GetRGB(this.m_worksheet.Workbook) : this.FontColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].FontColorRGB = value;
    }
  }

  public double FontSize
  {
    get
    {
      double fontSize = 0.0;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          fontSize = arrPivotCellFormat.FontSize;
          flag = false;
        }
        else if ((double) arrPivotCellFormat.IndentLevel != fontSize)
          return 0.0;
      }
      return fontSize;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].FontSize = value;
    }
  }

  public string FontName
  {
    get
    {
      string fontName = (string) null;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          fontName = arrPivotCellFormat.FontName;
          flag = false;
        }
        else if (arrPivotCellFormat.FontName != fontName)
          return (string) null;
      }
      return fontName;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].FontName = value;
    }
  }

  public bool Bold
  {
    get
    {
      bool bold = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          bold = arrPivotCellFormat.Bold;
          flag = false;
        }
        else if (arrPivotCellFormat.Bold != bold)
          return false;
      }
      return bold;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].Bold = value;
    }
  }

  public bool Italic
  {
    get
    {
      bool italic = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          italic = arrPivotCellFormat.Italic;
          flag = false;
        }
        else if (arrPivotCellFormat.Italic != italic)
          return false;
      }
      return italic;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].Italic = value;
    }
  }

  public ExcelUnderline Underline
  {
    get
    {
      bool flag = false;
      ExcelUnderline underline = ExcelUnderline.None;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          underline = arrPivotCellFormat.Underline;
          flag = false;
        }
        else if (arrPivotCellFormat.Underline != underline)
          return ExcelUnderline.None;
      }
      return underline;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].Underline = value;
    }
  }

  public bool StrikeThrough
  {
    get
    {
      bool strikeThrough = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          strikeThrough = arrPivotCellFormat.Bold;
          flag = false;
        }
        else if (arrPivotCellFormat.Bold != strikeThrough)
          return false;
      }
      return strikeThrough;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].Bold = value;
    }
  }

  public ExcelKnownColors TopBorderColor
  {
    get
    {
      ExcelKnownColors topBorderColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          topBorderColor = arrPivotCellFormat.TopBorderColor;
          flag = false;
        }
        else if (arrPivotCellFormat.TopBorderColor != topBorderColor)
          return ExcelKnownColors.None;
      }
      return topBorderColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].TopBorderColor = value;
    }
  }

  public Color TopBorderColorRGB
  {
    get
    {
      return this.TopBorderColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].TopBorderColorObject.GetRGB(this.m_worksheet.Workbook) : this.TopBorderColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].TopBorderColorRGB = value;
    }
  }

  public ExcelLineStyle TopBorderStyle
  {
    get
    {
      bool flag = false;
      ExcelLineStyle topBorderStyle = ExcelLineStyle.None;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          topBorderStyle = arrPivotCellFormat.TopBorderStyle;
          flag = false;
        }
        else if (arrPivotCellFormat.TopBorderStyle != topBorderStyle)
          return ExcelLineStyle.None;
      }
      return topBorderStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].TopBorderStyle = value;
    }
  }

  public ExcelKnownColors VerticalBorderColor
  {
    get
    {
      ExcelKnownColors verticalBorderColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          verticalBorderColor = arrPivotCellFormat.VerticalBorderColor;
          flag = false;
        }
        else if (arrPivotCellFormat.VerticalBorderColor != verticalBorderColor)
          return ExcelKnownColors.None;
      }
      return verticalBorderColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].VerticalBorderColor = value;
    }
  }

  public Color VerticalBorderColorRGB
  {
    get
    {
      return this.VerticalBorderColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].VerticalBorderColorObject.GetRGB(this.m_worksheet.Workbook) : this.VerticalBorderColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].VerticalBorderColorRGB = value;
    }
  }

  public ExcelLineStyle VerticalBorderStyle
  {
    get
    {
      bool flag = false;
      ExcelLineStyle verticalBorderStyle = ExcelLineStyle.None;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          verticalBorderStyle = arrPivotCellFormat.VerticalBorderStyle;
          flag = false;
        }
        else if (arrPivotCellFormat.VerticalBorderStyle != verticalBorderStyle)
          return ExcelLineStyle.None;
      }
      return verticalBorderStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].VerticalBorderStyle = value;
    }
  }

  public ExcelKnownColors HorizontalBorderColor
  {
    get
    {
      ExcelKnownColors horizontalBorderColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          horizontalBorderColor = arrPivotCellFormat.HorizontalBorderColor;
          flag = false;
        }
        else if (arrPivotCellFormat.HorizontalBorderColor != horizontalBorderColor)
          return ExcelKnownColors.None;
      }
      return horizontalBorderColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].HorizontalBorderColor = value;
    }
  }

  public Color HorizontalBorderColorRGB
  {
    get
    {
      return this.HorizontalBorderColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].HorizontalBorderColorObject.GetRGB(this.m_worksheet.Workbook) : this.HorizontalBorderColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].HorizontalBorderColorRGB = value;
    }
  }

  public ExcelLineStyle HorizontalBorderStyle
  {
    get
    {
      bool flag = false;
      ExcelLineStyle horizontalBorderStyle = ExcelLineStyle.None;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          horizontalBorderStyle = arrPivotCellFormat.HorizontalBorderStyle;
          flag = false;
        }
        else if (arrPivotCellFormat.HorizontalBorderStyle != horizontalBorderStyle)
          return ExcelLineStyle.None;
      }
      return horizontalBorderStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].HorizontalBorderStyle = value;
    }
  }

  public ExcelKnownColors BottomBorderColor
  {
    get
    {
      ExcelKnownColors bottomBorderColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          bottomBorderColor = arrPivotCellFormat.BottomBorderColor;
          flag = false;
        }
        else if (arrPivotCellFormat.BottomBorderColor != bottomBorderColor)
          return ExcelKnownColors.None;
      }
      return bottomBorderColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].BottomBorderColor = value;
    }
  }

  public Color BottomBorderColorRGB
  {
    get
    {
      return this.BottomBorderColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].BottomBorderColorObject.GetRGB(this.m_worksheet.Workbook) : this.BottomBorderColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].BottomBorderColorRGB = value;
    }
  }

  public ExcelLineStyle BottomBorderStyle
  {
    get
    {
      bool flag = false;
      ExcelLineStyle bottomBorderStyle = ExcelLineStyle.None;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          bottomBorderStyle = arrPivotCellFormat.BottomBorderStyle;
          flag = false;
        }
        else if (arrPivotCellFormat.BottomBorderStyle != bottomBorderStyle)
          return ExcelLineStyle.None;
      }
      return bottomBorderStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].BottomBorderStyle = value;
    }
  }

  public ExcelKnownColors RightBorderColor
  {
    get
    {
      ExcelKnownColors rightBorderColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          rightBorderColor = arrPivotCellFormat.RightBorderColor;
          flag = false;
        }
        else if (arrPivotCellFormat.RightBorderColor != rightBorderColor)
          return ExcelKnownColors.None;
      }
      return rightBorderColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].RightBorderColor = value;
    }
  }

  public Color RightBorderColorRGB
  {
    get
    {
      return this.RightBorderColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].RightBorderColorObject.GetRGB(this.m_worksheet.Workbook) : this.RightBorderColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].RightBorderColorRGB = value;
    }
  }

  public ExcelLineStyle RightBorderStyle
  {
    get
    {
      bool flag = false;
      ExcelLineStyle rightBorderStyle = ExcelLineStyle.None;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          rightBorderStyle = arrPivotCellFormat.RightBorderStyle;
          flag = false;
        }
        else if (arrPivotCellFormat.RightBorderStyle != rightBorderStyle)
          return ExcelLineStyle.None;
      }
      return rightBorderStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].RightBorderStyle = value;
    }
  }

  public ExcelKnownColors LeftBorderColor
  {
    get
    {
      ExcelKnownColors leftBorderColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          leftBorderColor = arrPivotCellFormat.LeftBorderColor;
          flag = false;
        }
        else if (arrPivotCellFormat.LeftBorderColor != leftBorderColor)
          return ExcelKnownColors.None;
      }
      return leftBorderColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].LeftBorderColor = value;
    }
  }

  public Color LeftBorderColorRGB
  {
    get
    {
      return this.LeftBorderColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].LeftBorderColorObject.GetRGB(this.m_worksheet.Workbook) : this.LeftBorderColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].LeftBorderColorRGB = value;
    }
  }

  public ExcelLineStyle LeftBorderStyle
  {
    get
    {
      bool flag = false;
      ExcelLineStyle leftBorderStyle = ExcelLineStyle.None;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          leftBorderStyle = arrPivotCellFormat.LeftBorderStyle;
          flag = false;
        }
        else if (arrPivotCellFormat.LeftBorderStyle != leftBorderStyle)
          return ExcelLineStyle.None;
      }
      return leftBorderStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].LeftBorderStyle = value;
    }
  }

  public bool IsTopBorderModified
  {
    get
    {
      bool topBorderModified = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          topBorderModified = arrPivotCellFormat.IsTopBorderModified;
          flag = false;
        }
        else if (arrPivotCellFormat.IsTopBorderModified != topBorderModified)
          return false;
      }
      return topBorderModified;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsTopBorderModified = value;
    }
  }

  public bool IsBottomBorderModified
  {
    get
    {
      bool bottomBorderModified = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          bottomBorderModified = arrPivotCellFormat.IsBottomBorderModified;
          flag = false;
        }
        else if (arrPivotCellFormat.IsBottomBorderModified != bottomBorderModified)
          return false;
      }
      return bottomBorderModified;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsBottomBorderModified = value;
    }
  }

  public bool IsRightBorderModified
  {
    get
    {
      bool rightBorderModified = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          rightBorderModified = arrPivotCellFormat.IsRightBorderModified;
          flag = false;
        }
        else if (arrPivotCellFormat.IsRightBorderModified != rightBorderModified)
          return false;
      }
      return rightBorderModified;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsRightBorderModified = value;
    }
  }

  public bool IsLeftBorderModified
  {
    get
    {
      bool leftBorderModified = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          leftBorderModified = arrPivotCellFormat.IsLeftBorderModified;
          flag = false;
        }
        else if (arrPivotCellFormat.IsLeftBorderModified != leftBorderModified)
          return false;
      }
      return leftBorderModified;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsLeftBorderModified = value;
    }
  }

  public bool IsDiagonalBorderModified
  {
    get
    {
      bool diagonalBorderModified = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          diagonalBorderModified = arrPivotCellFormat.IsDiagonalBorderModified;
          flag = false;
        }
        else if (arrPivotCellFormat.IsDiagonalBorderModified != diagonalBorderModified)
          return false;
      }
      return diagonalBorderModified;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsDiagonalBorderModified = value;
    }
  }

  public bool IsFontFormatPresent
  {
    get
    {
      bool fontFormatPresent = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          fontFormatPresent = arrPivotCellFormat.IsFontFormatPresent;
          flag = false;
        }
        else if (arrPivotCellFormat.IsFontFormatPresent != fontFormatPresent)
          return false;
      }
      return fontFormatPresent;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsFontFormatPresent = value;
    }
  }

  public bool IsPatternColorModified
  {
    get
    {
      bool patternColorModified = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          patternColorModified = arrPivotCellFormat.IsPatternColorModified;
          flag = false;
        }
        else if (arrPivotCellFormat.IsPatternColorModified != patternColorModified)
          return false;
      }
      return patternColorModified;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsPatternColorModified = value;
    }
  }

  public bool IsPatternFormatPresent
  {
    get
    {
      bool patternFormatPresent = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          patternFormatPresent = arrPivotCellFormat.IsPatternFormatPresent;
          flag = false;
        }
        else if (arrPivotCellFormat.IsPatternFormatPresent != patternFormatPresent)
          return false;
      }
      return patternFormatPresent;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsPatternFormatPresent = value;
    }
  }

  public bool IsBackgroundColorPresent
  {
    get
    {
      bool backgroundColorPresent = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          backgroundColorPresent = arrPivotCellFormat.IsBackgroundColorPresent;
          flag = false;
        }
        else if (arrPivotCellFormat.IsBackgroundColorPresent != backgroundColorPresent)
          return false;
      }
      return backgroundColorPresent;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsBackgroundColorPresent = value;
    }
  }

  public bool IsBorderFormatPresent
  {
    get
    {
      bool borderFormatPresent = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          borderFormatPresent = arrPivotCellFormat.IsBorderFormatPresent;
          flag = false;
        }
        else if (arrPivotCellFormat.IsBorderFormatPresent != borderFormatPresent)
          return false;
      }
      return borderFormatPresent;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsBorderFormatPresent = value;
    }
  }

  public bool IsFontColorPresent
  {
    get
    {
      bool fontColorPresent = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          fontColorPresent = arrPivotCellFormat.IsFontColorPresent;
          flag = false;
        }
        else if (arrPivotCellFormat.IsFontColorPresent != fontColorPresent)
          return false;
      }
      return fontColorPresent;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsFontColorPresent = value;
    }
  }

  public ColorObject FontColorObject
  {
    get
    {
      ColorObject fontColorObject = this.m_arrPivotCellFormats[0].FontColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (fontColorObject != this.m_arrPivotCellFormats[index].FontColorObject)
          return (ColorObject) null;
      }
      return fontColorObject;
    }
  }

  public ColorObject ColorObject
  {
    get
    {
      ColorObject colorObject = this.m_arrPivotCellFormats[0].ColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (colorObject != this.m_arrPivotCellFormats[index].ColorObject)
          return (ColorObject) null;
      }
      return colorObject;
    }
  }

  public ColorObject BackColorObject
  {
    get
    {
      ColorObject backColorObject = this.m_arrPivotCellFormats[0].BackColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (backColorObject != this.m_arrPivotCellFormats[index].BackColorObject)
          return (ColorObject) null;
      }
      return backColorObject;
    }
  }

  public ColorObject TopBorderColorObject
  {
    get
    {
      ColorObject borderColorObject = this.m_arrPivotCellFormats[0].TopBorderColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (borderColorObject != this.m_arrPivotCellFormats[index].TopBorderColorObject)
          return (ColorObject) null;
      }
      return borderColorObject;
    }
  }

  public ColorObject BottomBorderColorObject
  {
    get
    {
      ColorObject borderColorObject = this.m_arrPivotCellFormats[0].BottomBorderColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (borderColorObject != this.m_arrPivotCellFormats[index].BottomBorderColorObject)
          return (ColorObject) null;
      }
      return borderColorObject;
    }
  }

  public ColorObject HorizontalBorderColorObject
  {
    get
    {
      ColorObject borderColorObject = this.m_arrPivotCellFormats[0].HorizontalBorderColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (borderColorObject != this.m_arrPivotCellFormats[index].HorizontalBorderColorObject)
          return (ColorObject) null;
      }
      return borderColorObject;
    }
  }

  public ColorObject VerticalBorderColorObject
  {
    get
    {
      ColorObject borderColorObject = this.m_arrPivotCellFormats[0].VerticalBorderColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (borderColorObject != this.m_arrPivotCellFormats[index].VerticalBorderColorObject)
          return (ColorObject) null;
      }
      return borderColorObject;
    }
  }

  public ColorObject RightBorderColorObject
  {
    get
    {
      ColorObject borderColorObject = this.m_arrPivotCellFormats[0].RightBorderColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (borderColorObject != this.m_arrPivotCellFormats[index].RightBorderColorObject)
          return (ColorObject) null;
      }
      return borderColorObject;
    }
  }

  public ColorObject LeftBorderColorObject
  {
    get
    {
      ColorObject borderColorObject = this.m_arrPivotCellFormats[0].LeftBorderColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (borderColorObject != this.m_arrPivotCellFormats[index].LeftBorderColorObject)
          return (ColorObject) null;
      }
      return borderColorObject;
    }
  }

  public bool IsVerticalBorderModified
  {
    get
    {
      bool verticalBorderModified = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          verticalBorderModified = arrPivotCellFormat.IsVerticalBorderModified;
          flag = false;
        }
        else if (arrPivotCellFormat.IsVerticalBorderModified != verticalBorderModified)
          return false;
      }
      return verticalBorderModified;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsVerticalBorderModified = value;
    }
  }

  public bool IsHorizontalBorderModified
  {
    get
    {
      bool horizontalBorderModified = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          horizontalBorderModified = arrPivotCellFormat.IsHorizontalBorderModified;
          flag = false;
        }
        else if (arrPivotCellFormat.IsHorizontalBorderModified != horizontalBorderModified)
          return false;
      }
      return horizontalBorderModified;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsHorizontalBorderModified = value;
    }
  }

  public ExcelHAlign HorizontalAlignment
  {
    get
    {
      ExcelHAlign horizontalAlignment = ExcelHAlign.HAlignGeneral;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          horizontalAlignment = arrPivotCellFormat.HorizontalAlignment;
          flag = false;
        }
        else if (arrPivotCellFormat.HorizontalAlignment != horizontalAlignment)
          return ExcelHAlign.HAlignGeneral;
      }
      return horizontalAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].HorizontalAlignment = value;
    }
  }

  public int IndentLevel
  {
    get
    {
      int indentLevel = 0;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          indentLevel = arrPivotCellFormat.IndentLevel;
          flag = false;
        }
        else if (arrPivotCellFormat.IndentLevel != indentLevel)
          return 0;
      }
      return indentLevel;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IndentLevel = value;
    }
  }

  public ExcelVAlign VerticalAlignment
  {
    get
    {
      ExcelVAlign verticalAlignment = ExcelVAlign.VAlignBottom;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          verticalAlignment = arrPivotCellFormat.VerticalAlignment;
          flag = false;
        }
        else if (arrPivotCellFormat.VerticalAlignment != verticalAlignment)
          return ExcelVAlign.VAlignBottom;
      }
      return verticalAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].VerticalAlignment = value;
    }
  }

  public ColorObject DiagonalBorderColorObject
  {
    get
    {
      ColorObject borderColorObject = this.m_arrPivotCellFormats[0].DiagonalBorderColorObject;
      for (int index = 1; index < this.m_arrPivotCellFormats.Count; ++index)
      {
        if (borderColorObject != this.m_arrPivotCellFormats[index].DiagonalBorderColorObject)
          return (ColorObject) null;
      }
      return borderColorObject;
    }
  }

  public ExcelKnownColors DiagonalBorderColor
  {
    get
    {
      ExcelKnownColors diagonalBorderColor = ExcelKnownColors.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          diagonalBorderColor = arrPivotCellFormat.DiagonalBorderColor;
          flag = false;
        }
        else if (arrPivotCellFormat.DiagonalBorderColor != diagonalBorderColor)
          return ExcelKnownColors.None;
      }
      return diagonalBorderColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].DiagonalBorderColor = value;
    }
  }

  public Color DiagonalBorderColorRGB
  {
    get
    {
      return this.DiagonalBorderColorObject == (ColorObject) null ? this.m_arrPivotCellFormats[0].DiagonalBorderColorObject.GetRGB(this.m_worksheet.Workbook) : this.DiagonalBorderColorObject.GetRGB(this.m_worksheet.Workbook);
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].DiagonalBorderColorRGB = value;
    }
  }

  public ExcelLineStyle DiagonalBorderStyle
  {
    get
    {
      bool flag = false;
      ExcelLineStyle diagonalBorderStyle = ExcelLineStyle.None;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          diagonalBorderStyle = arrPivotCellFormat.DiagonalBorderStyle;
          flag = false;
        }
        else if (arrPivotCellFormat.DiagonalBorderStyle != diagonalBorderStyle)
          return ExcelLineStyle.None;
      }
      return diagonalBorderStyle;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].DiagonalBorderStyle = value;
    }
  }

  public ExcelReadingOrderType ReadingOrder
  {
    get
    {
      ExcelReadingOrderType readingOrder = ExcelReadingOrderType.Context;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          readingOrder = arrPivotCellFormat.ReadingOrder;
          flag = false;
        }
        else if (arrPivotCellFormat.ReadingOrder != readingOrder)
          return ExcelReadingOrderType.Context;
      }
      return readingOrder;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].ReadingOrder = value;
    }
  }

  public bool WrapText
  {
    get
    {
      bool wrapText = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          wrapText = arrPivotCellFormat.WrapText;
          flag = false;
        }
        else if (arrPivotCellFormat.WrapText != wrapText)
          return false;
      }
      return wrapText;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].WrapText = value;
    }
  }

  public bool ShrinkToFit
  {
    get
    {
      bool shrinkToFit = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          shrinkToFit = arrPivotCellFormat.ShrinkToFit;
          flag = false;
        }
        else if (arrPivotCellFormat.ShrinkToFit != shrinkToFit)
          return false;
      }
      return shrinkToFit;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].ShrinkToFit = value;
    }
  }

  public bool Locked
  {
    get
    {
      bool locked = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          locked = arrPivotCellFormat.Locked;
          flag = false;
        }
        else if (arrPivotCellFormat.Locked != locked)
          return false;
      }
      return locked;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].Locked = value;
    }
  }

  public bool FormulaHidden
  {
    get
    {
      bool formulaHidden = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          formulaHidden = arrPivotCellFormat.FormulaHidden;
          flag = false;
        }
        else if (arrPivotCellFormat.FormulaHidden != formulaHidden)
          return false;
      }
      return formulaHidden;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].FormulaHidden = value;
    }
  }

  public int Rotation
  {
    get
    {
      int rotation = 0;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          rotation = arrPivotCellFormat.Rotation;
          flag = false;
        }
        else if (arrPivotCellFormat.Rotation != rotation)
          return 0;
      }
      return rotation;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].Rotation = value;
    }
  }

  public ushort NumberFormatIndex
  {
    get
    {
      ushort numberFormatIndex = 0;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          numberFormatIndex = arrPivotCellFormat.NumberFormatIndex;
          flag = false;
        }
        else if ((int) arrPivotCellFormat.NumberFormatIndex != (int) numberFormatIndex)
          return 0;
      }
      return numberFormatIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].NumberFormatIndex = value;
    }
  }

  public string NumberFormat
  {
    get
    {
      string numberFormat = (string) null;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        IPivotCellFormat arrPivotCellFormat = (IPivotCellFormat) this.m_arrPivotCellFormats[index];
        if (flag)
        {
          numberFormat = arrPivotCellFormat.NumberFormat;
          flag = false;
        }
        else if (arrPivotCellFormat.NumberFormat != numberFormat)
          return (string) null;
      }
      return numberFormat;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].NumberFormat = value;
    }
  }

  public bool IsNumberFormatPresent
  {
    get
    {
      bool numberFormatPresent = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          numberFormatPresent = arrPivotCellFormat.IsNumberFormatPresent;
          flag = false;
        }
        else if (arrPivotCellFormat.IsNumberFormatPresent != numberFormatPresent)
          return false;
      }
      return numberFormatPresent;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IsNumberFormatPresent = value;
    }
  }

  public bool IncludeAlignment
  {
    get
    {
      bool includeAlignment = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          includeAlignment = arrPivotCellFormat.IncludeAlignment;
          flag = false;
        }
        else if (arrPivotCellFormat.IncludeAlignment != includeAlignment)
          return false;
      }
      return includeAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IncludeAlignment = value;
    }
  }

  public bool IncludeProtection
  {
    get
    {
      bool includeProtection = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
      {
        PivotCellFormat arrPivotCellFormat = this.m_arrPivotCellFormats[index];
        if (flag)
        {
          includeProtection = arrPivotCellFormat.IncludeProtection;
          flag = false;
        }
        else if (arrPivotCellFormat.IncludeProtection != includeProtection)
          return false;
      }
      return includeProtection;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrPivotCellFormats.Count; index < count; ++index)
        this.m_arrPivotCellFormats[index].IncludeProtection = value;
    }
  }

  internal PivotCellFormatWrapper(
    List<PivotFormat> pivotFormats,
    List<PivotCellFormat> pivotCellFormats)
  {
    this.m_arrPivotFormats = pivotFormats;
    this.m_arrPivotCellFormats = pivotCellFormats;
    this.m_worksheet = pivotFormats[0].PivotTable.Worksheet;
  }

  public void Clear() => this.Dispose();

  internal void Dispose() => this.m_arrPivotFormats = (List<PivotFormat>) null;

  public override bool Equals(object obj)
  {
    TableStyleElement tableStyleElement = (TableStyleElement) obj;
    return this.BackColor.Equals((object) tableStyleElement.BackColor) && this.BackColorRGB.Equals((object) tableStyleElement.BackColorRGB) && this.FontColor.Equals((object) tableStyleElement.FontColor) && this.FontColorRGB.Equals((object) tableStyleElement.FontColorRGB) && this.PatternColor.Equals((object) tableStyleElement.PatternColor) && this.PatternColorRGB.Equals((object) tableStyleElement.PatternColorRGB) && this.PatternStyle.Equals((object) tableStyleElement.PatternStyle) && this.Bold.Equals(tableStyleElement.Bold) && this.Italic.Equals(tableStyleElement.Italic) && this.StrikeThrough.Equals(tableStyleElement.StrikeThrough) && this.Underline.Equals((object) tableStyleElement.Underline) && this.TopBorderColor.Equals((object) tableStyleElement.TopBorderColor) && this.TopBorderColorRGB.Equals((object) tableStyleElement.TopBorderColorRGB) && this.TopBorderStyle.Equals((object) tableStyleElement.TopBorderStyle) && this.BottomBorderColor.Equals((object) tableStyleElement.BottomBorderColor) && this.BottomBorderColorRGB.Equals((object) tableStyleElement.BottomBorderColorRGB) && this.BottomBorderStyle.Equals((object) tableStyleElement.BottomBorderStyle) && this.RightBorderColor.Equals((object) tableStyleElement.RightBorderColor) && this.RightBorderColorRGB.Equals((object) tableStyleElement.RightBorderColorRGB) && this.RightBorderStyle.Equals((object) tableStyleElement.RightBorderStyle) && this.LeftBorderColor.Equals((object) tableStyleElement.LeftBorderColor) && this.LeftBorderColorRGB.Equals((object) tableStyleElement.LeftBorderColorRGB) && this.LeftBorderStyle.Equals((object) tableStyleElement.LeftBorderStyle) && this.HorizontalBorderColor.Equals((object) tableStyleElement.HorizontalBorderColor) && this.HorizontalBorderColorRGB.Equals((object) tableStyleElement.HorizontalBorderColorRGB) && this.HorizontalBorderStyle.Equals((object) tableStyleElement.HorizontalBorderStyle) && this.VerticalBorderColor.Equals((object) tableStyleElement.VerticalBorderColor) && this.VerticalBorderColorRGB.Equals((object) tableStyleElement.VerticalBorderColorRGB) && this.VerticalBorderStyle.Equals((object) tableStyleElement.VerticalBorderStyle);
  }

  internal void SetChanged()
  {
  }
}
