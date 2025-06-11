// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotCellFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.PivotTables;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class PivotCellFormat : IInternalPivotCellFormat, IPivotCellFormat
{
  private PivotFormat m_pivotFormat;
  private ColorObject m_backColor;
  private ExcelPattern m_pattern;
  private ColorObject m_color;
  private ColorObject m_fontColor;
  private double m_fontSize;
  private string m_fontName;
  private bool m_bold;
  private bool m_italic;
  private ExcelUnderline m_underline;
  private bool m_strikeThrough;
  private ColorObject m_topBorderColor;
  private ExcelLineStyle m_topBorderLineStyle;
  private ColorObject m_bottomBorderColor;
  private ExcelLineStyle m_bottomBorderLineStyle;
  private ColorObject m_rightBorderColor;
  private ExcelLineStyle m_rightBorderLineStyle;
  private ColorObject m_leftBorderColor;
  private ExcelLineStyle m_leftBorderLineStyle;
  private ColorObject m_diagonalBorderColor;
  private ExcelLineStyle m_diagonalBorderStyle;
  private bool m_isTopBorderModified;
  private bool m_isBottomBorderModified;
  private bool m_isLeftBorderModified;
  private bool m_isRightBorderModified;
  private bool m_isDiagonalBorderModified;
  private bool m_isBorderFormatPresent;
  private bool m_isFontColorPresent;
  private bool m_isFontFormatPresent;
  private bool m_isBackColorModified;
  private bool m_isPatternFormatPresent;
  private bool m_isPatternColorModified;
  private bool m_isVerticalBorderModified;
  private bool m_isHorizontalBorderModified;
  private ColorObject m_verticalBorderColor;
  private ColorObject m_horizontalBorderColor;
  private ExcelLineStyle m_verticalBorderLineStyle;
  private ExcelLineStyle m_horizontalBorderLineStyle;
  private ExcelHAlign m_horizontalAlignment;
  private int m_indent;
  private ExcelVAlign m_verticalAlignment;
  private int m_rotation;
  private ExcelReadingOrderType m_readingOrder;
  private bool m_bWrapText;
  private bool m_bShrinkToFit;
  private bool m_bLocked;
  private bool m_bFormulaHidden;
  private WorksheetImpl m_worksheet;
  private ushort m_numberFormatIndex;
  private bool m_bNumberFormatPresent;
  private bool m_bIncludeAlignment;
  private bool m_bIncludeProtection;

  internal PivotFormat PivotFormat
  {
    get => this.m_pivotFormat;
    set => this.m_pivotFormat = value;
  }

  public ExcelKnownColors BackColor
  {
    get => this.m_backColor.GetIndexed(this.m_worksheet.Workbook);
    set
    {
      this.m_backColor.SetIndexed(value);
      this.m_pattern = ExcelPattern.Solid;
      this.IsBackgroundColorPresent = true;
    }
  }

  public Color BackColorRGB
  {
    get => this.m_backColor.GetRGB(this.m_worksheet.Workbook);
    set
    {
      this.m_backColor.SetRGB(value, this.m_worksheet.Workbook);
      this.m_pattern = ExcelPattern.Solid;
      this.IsBackgroundColorPresent = true;
    }
  }

  public ExcelPattern PatternStyle
  {
    get => this.m_pattern;
    set
    {
      this.UpdatePatternFormat();
      this.m_pattern = value;
      this.IsPatternFormatPresent = true;
    }
  }

  public ExcelKnownColors PatternColor
  {
    get => this.m_color.GetIndexed(this.m_worksheet.Workbook);
    set
    {
      this.m_color.SetIndexed(value);
      this.IsPatternColorModified = true;
      this.IsPatternFormatPresent = true;
    }
  }

  public Color PatternColorRGB
  {
    get => this.m_color.GetRGB(this.m_worksheet.Workbook);
    set => this.m_color.SetRGB(value, this.m_worksheet.Workbook);
  }

  public ExcelKnownColors FontColor
  {
    get => this.m_fontColor.GetIndexed(this.m_worksheet.Workbook);
    set
    {
      this.m_fontColor.SetIndexed(value);
      this.UpdateFontColor();
    }
  }

  public Color FontColorRGB
  {
    get => this.m_fontColor.GetRGB(this.m_worksheet.Workbook);
    set
    {
      this.m_fontColor.SetRGB(value, this.m_worksheet.Workbook);
      this.UpdateFontColor();
    }
  }

  public double FontSize
  {
    get => this.m_fontSize;
    set
    {
      this.m_fontSize = value;
      this.UpdateFontFormat();
    }
  }

  public string FontName
  {
    get => this.m_fontName;
    set
    {
      this.m_fontName = value;
      this.UpdateFontFormat();
    }
  }

  public bool Bold
  {
    get => this.m_bold;
    set
    {
      this.m_bold = value;
      this.UpdateFontFormat();
    }
  }

  public bool Italic
  {
    get => this.m_italic;
    set
    {
      this.m_italic = value;
      this.UpdateFontFormat();
    }
  }

  public ExcelUnderline Underline
  {
    get => this.m_underline;
    set
    {
      this.m_underline = value;
      this.UpdateFontFormat();
    }
  }

  public bool StrikeThrough
  {
    get => this.m_strikeThrough;
    set
    {
      this.m_strikeThrough = value;
      this.UpdateFontFormat();
    }
  }

  public ExcelKnownColors TopBorderColor
  {
    get => this.m_topBorderColor.GetIndexed(this.m_worksheet.Workbook);
    set => this.m_topBorderColor.SetIndexed(value);
  }

  public Color TopBorderColorRGB
  {
    get => this.m_topBorderColor.GetRGB(this.m_worksheet.Workbook);
    set => this.m_topBorderColor.SetRGB(value, this.m_worksheet.Workbook);
  }

  public ExcelLineStyle TopBorderStyle
  {
    get => this.m_topBorderLineStyle;
    set
    {
      this.m_topBorderLineStyle = value;
      this.UpdateTopBorderFormat();
    }
  }

  public ExcelKnownColors VerticalBorderColor
  {
    get => this.m_verticalBorderColor.GetIndexed(this.m_worksheet.Workbook);
    set => this.m_verticalBorderColor.SetIndexed(value);
  }

  public Color VerticalBorderColorRGB
  {
    get => this.m_verticalBorderColor.GetRGB(this.m_worksheet.Workbook);
    set => this.m_verticalBorderColor.SetRGB(value, this.m_worksheet.Workbook);
  }

  public ExcelLineStyle VerticalBorderStyle
  {
    get => this.m_verticalBorderLineStyle;
    set
    {
      this.m_verticalBorderLineStyle = value;
      this.UpdateVerticalBorderFormat();
    }
  }

  public ExcelKnownColors HorizontalBorderColor
  {
    get => this.m_horizontalBorderColor.GetIndexed(this.m_worksheet.Workbook);
    set => this.m_horizontalBorderColor.SetIndexed(value);
  }

  public Color HorizontalBorderColorRGB
  {
    get => this.m_horizontalBorderColor.GetRGB(this.m_worksheet.Workbook);
    set => this.m_horizontalBorderColor.SetRGB(value, this.m_worksheet.Workbook);
  }

  public ExcelLineStyle HorizontalBorderStyle
  {
    get => this.m_horizontalBorderLineStyle;
    set
    {
      this.m_horizontalBorderLineStyle = value;
      this.UpdateHorizontalBorderFormat();
    }
  }

  public ExcelKnownColors BottomBorderColor
  {
    get => this.m_bottomBorderColor.GetIndexed(this.m_worksheet.Workbook);
    set => this.m_bottomBorderColor.SetIndexed(value);
  }

  public Color BottomBorderColorRGB
  {
    get => this.m_bottomBorderColor.GetRGB(this.m_worksheet.Workbook);
    set => this.m_bottomBorderColor.SetRGB(value, this.m_worksheet.Workbook);
  }

  public ExcelLineStyle BottomBorderStyle
  {
    get => this.m_bottomBorderLineStyle;
    set
    {
      this.m_bottomBorderLineStyle = value;
      this.UpdateBottomBorderFormat();
    }
  }

  public ExcelKnownColors RightBorderColor
  {
    get => this.m_rightBorderColor.GetIndexed(this.m_worksheet.Workbook);
    set => this.m_rightBorderColor.SetIndexed(value);
  }

  public Color RightBorderColorRGB
  {
    get => this.m_rightBorderColor.GetRGB(this.m_worksheet.Workbook);
    set => this.m_rightBorderColor.SetRGB(value, this.m_worksheet.Workbook);
  }

  public ExcelLineStyle RightBorderStyle
  {
    get => this.m_rightBorderLineStyle;
    set
    {
      this.m_rightBorderLineStyle = value;
      this.UpdateRightBorderFormat();
    }
  }

  public ExcelKnownColors LeftBorderColor
  {
    get => this.m_leftBorderColor.GetIndexed(this.m_worksheet.Workbook);
    set => this.m_leftBorderColor.SetIndexed(value);
  }

  public Color LeftBorderColorRGB
  {
    get => this.m_leftBorderColor.GetRGB(this.m_worksheet.Workbook);
    set => this.m_leftBorderColor.SetRGB(value, this.m_worksheet.Workbook);
  }

  public ExcelLineStyle LeftBorderStyle
  {
    get => this.m_leftBorderLineStyle;
    set
    {
      this.m_leftBorderLineStyle = value;
      this.UpdateLeftBorderFormat();
    }
  }

  public bool IsTopBorderModified
  {
    get => this.m_isTopBorderModified;
    set => this.m_isTopBorderModified = value;
  }

  public bool IsBottomBorderModified
  {
    get => this.m_isBottomBorderModified;
    set => this.m_isBottomBorderModified = value;
  }

  public bool IsRightBorderModified
  {
    get => this.m_isRightBorderModified;
    set => this.m_isRightBorderModified = value;
  }

  public bool IsLeftBorderModified
  {
    get => this.m_isLeftBorderModified;
    set => this.m_isLeftBorderModified = value;
  }

  public bool IsDiagonalBorderModified
  {
    get => this.m_isDiagonalBorderModified;
    set => this.m_isDiagonalBorderModified = value;
  }

  public bool IsFontFormatPresent
  {
    get => this.m_isFontFormatPresent;
    set => this.m_isFontFormatPresent = value;
  }

  public bool IsPatternColorModified
  {
    get => this.m_isPatternColorModified;
    set => this.m_isPatternColorModified = value;
  }

  public bool IsPatternFormatPresent
  {
    get => this.m_isPatternFormatPresent;
    set => this.m_isPatternFormatPresent = value;
  }

  public bool IsBackgroundColorPresent
  {
    get => this.m_isBackColorModified;
    set => this.m_isBackColorModified = value;
  }

  public bool IsBorderFormatPresent
  {
    get => this.m_isBorderFormatPresent;
    set => this.m_isBorderFormatPresent = value;
  }

  public bool IsFontColorPresent
  {
    get => this.m_isFontColorPresent;
    set => this.m_isFontColorPresent = value;
  }

  public ColorObject FontColorObject => this.m_fontColor;

  public ColorObject ColorObject => this.m_color;

  public ColorObject BackColorObject => this.m_backColor;

  public ColorObject TopBorderColorObject => this.m_topBorderColor;

  public ColorObject BottomBorderColorObject => this.m_bottomBorderColor;

  public ColorObject HorizontalBorderColorObject
  {
    get => this.m_horizontalBorderColor;
    set => this.m_horizontalBorderColor = value;
  }

  public ColorObject VerticalBorderColorObject
  {
    get => this.m_verticalBorderColor;
    set => this.m_verticalBorderColor = value;
  }

  public ColorObject RightBorderColorObject => this.m_rightBorderColor;

  public ColorObject LeftBorderColorObject => this.m_leftBorderColor;

  public bool IsVerticalBorderModified
  {
    get => this.m_isVerticalBorderModified;
    set => this.m_isVerticalBorderModified = value;
  }

  public bool IsHorizontalBorderModified
  {
    get => this.m_isHorizontalBorderModified;
    set => this.m_isHorizontalBorderModified = value;
  }

  public ExcelHAlign HorizontalAlignment
  {
    get => this.m_horizontalAlignment;
    set
    {
      if (this.HorizontalAlignment == value)
        return;
      this.IncludeAlignment = true;
      this.m_horizontalAlignment = value;
      if (value == ExcelHAlign.HAlignCenterAcrossSelection || value == ExcelHAlign.HAlignFill)
        this.m_rotation = 0;
      this.SetChanged();
    }
  }

  public int IndentLevel
  {
    get => this.m_indent;
    set
    {
      if (this.IndentLevel == value)
        return;
      if (value > (this.m_worksheet.Workbook as WorkbookImpl).MaxIndent)
        throw new ArgumentOutOfRangeException(nameof (IndentLevel));
      this.IncludeAlignment = true;
      this.m_indent = value;
      if (this.HorizontalAlignment == ExcelHAlign.HAlignGeneral)
        this.HorizontalAlignment = ExcelHAlign.HAlignLeft;
      if (value != 0)
        this.m_rotation = 0;
      this.SetChanged();
    }
  }

  public ExcelVAlign VerticalAlignment
  {
    get => this.m_verticalAlignment;
    set
    {
      if (this.VerticalAlignment == value)
        return;
      this.IncludeAlignment = true;
      this.m_verticalAlignment = value;
      this.SetChanged();
    }
  }

  public ColorObject DiagonalBorderColorObject => this.m_diagonalBorderColor;

  public ExcelKnownColors DiagonalBorderColor
  {
    get => this.m_diagonalBorderColor.GetIndexed(this.m_worksheet.Workbook);
    set => this.m_diagonalBorderColor.SetIndexed(value);
  }

  public Color DiagonalBorderColorRGB
  {
    get => this.m_diagonalBorderColor.GetRGB(this.m_worksheet.Workbook);
    set => this.m_diagonalBorderColor.SetRGB(value, this.m_worksheet.Workbook);
  }

  public ExcelLineStyle DiagonalBorderStyle
  {
    get => this.m_diagonalBorderStyle;
    set
    {
      this.m_diagonalBorderStyle = value;
      this.UpdateLeftBorderFormat();
    }
  }

  public ExcelReadingOrderType ReadingOrder
  {
    get => this.m_readingOrder;
    set
    {
      this.m_readingOrder = value;
      this.SetChanged();
    }
  }

  public bool WrapText
  {
    get => this.m_bWrapText;
    set
    {
      this.IncludeAlignment = true;
      this.m_bWrapText = value;
      this.SetChanged();
    }
  }

  public bool ShrinkToFit
  {
    get => this.m_bShrinkToFit;
    set
    {
      this.IncludeAlignment = true;
      this.m_bShrinkToFit = value;
      this.SetChanged();
    }
  }

  public bool Locked
  {
    get => this.m_bLocked;
    set
    {
      if (value == this.m_bLocked)
        return;
      this.m_bLocked = value;
      this.IncludeProtection = true;
      this.SetChanged();
    }
  }

  public bool FormulaHidden
  {
    get => this.m_bFormulaHidden;
    set
    {
      if (value == this.m_bFormulaHidden)
        return;
      this.m_bFormulaHidden = value;
      this.IncludeProtection = true;
      this.SetChanged();
    }
  }

  public int Rotation
  {
    get => this.m_rotation;
    set
    {
      if (value > (int) byte.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (Rotation));
      if (value == this.Rotation)
        return;
      this.IncludeAlignment = true;
      this.m_rotation = this.HorizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection || this.HorizontalAlignment == ExcelHAlign.HAlignFill ? 0 : (int) (ushort) value;
      if (value != 0)
        this.m_indent = 0;
      this.SetChanged();
    }
  }

  public ushort NumberFormatIndex
  {
    get => this.m_numberFormatIndex;
    set
    {
      if (!this.m_worksheet.ParentWorkbook.InnerFormats.Contains((int) value))
        throw new ArgumentOutOfRangeException("Unknown format index");
      this.IsNumberFormatPresent = true;
      this.m_numberFormatIndex = value;
    }
  }

  public string NumberFormat
  {
    get => this.m_worksheet.ParentWorkbook.InnerFormats[(int) this.NumberFormatIndex].FormatString;
    set
    {
      this.NumberFormatIndex = (ushort) this.m_worksheet.ParentWorkbook.InnerFormats.FindOrCreateFormat(value);
    }
  }

  public bool IsNumberFormatPresent
  {
    get => this.m_bNumberFormatPresent;
    set => this.m_bNumberFormatPresent = value;
  }

  public bool IncludeAlignment
  {
    get => this.m_bIncludeAlignment;
    set => this.m_bIncludeAlignment = value;
  }

  public bool IncludeProtection
  {
    get => this.m_bIncludeProtection;
    set => this.m_bIncludeProtection = value;
  }

  internal PivotCellFormat(PivotFormat pivotFormat)
  {
    this.m_pivotFormat = pivotFormat;
    this.m_worksheet = pivotFormat.PivotTable.Worksheet;
    this.m_bLocked = true;
    this.m_horizontalAlignment = ExcelHAlign.HAlignGeneral;
    this.m_verticalAlignment = ExcelVAlign.VAlignBottom;
    this.InitializeColors();
    this.m_fontName = this.m_worksheet.ParentWorkbook.InnerFonts[0].FontName;
    this.m_fontSize = this.m_worksheet.ParentWorkbook.InnerFonts[0].Size;
  }

  public void Clear() => this.Dispose();

  internal object Clone(PivotFormat pivotFormat)
  {
    PivotCellFormat pivotCellFormat = (PivotCellFormat) this.MemberwiseClone();
    pivotCellFormat.m_pivotFormat = pivotFormat;
    int orCreateFormat = this.m_worksheet.ParentWorkbook.InnerFormats.FindOrCreateFormat(this.NumberFormat);
    pivotCellFormat.m_worksheet.ParentWorkbook.InnerFormats.Add(orCreateFormat, this.NumberFormat);
    pivotCellFormat.NumberFormatIndex = (ushort) orCreateFormat;
    pivotCellFormat.InitializeColors();
    pivotCellFormat.m_color.CopyFrom(this.m_color, false);
    pivotCellFormat.m_backColor.CopyFrom(this.m_backColor, false);
    pivotCellFormat.m_fontColor.CopyFrom(this.m_fontColor, false);
    pivotCellFormat.m_leftBorderColor.CopyFrom(this.m_leftBorderColor, false);
    pivotCellFormat.m_rightBorderColor.CopyFrom(this.m_rightBorderColor, false);
    pivotCellFormat.m_topBorderColor.CopyFrom(this.m_topBorderColor, false);
    pivotCellFormat.m_bottomBorderColor.CopyFrom(this.m_bottomBorderColor, false);
    return (object) pivotCellFormat;
  }

  internal void Dispose() => this.m_pivotFormat = (PivotFormat) null;

  public override bool Equals(object obj)
  {
    TableStyleElement tableStyleElement = (TableStyleElement) obj;
    return this.BackColor.Equals((object) tableStyleElement.BackColor) && this.BackColorRGB.Equals((object) tableStyleElement.BackColorRGB) && this.FontColor.Equals((object) tableStyleElement.FontColor) && this.FontColorRGB.Equals((object) tableStyleElement.FontColorRGB) && this.PatternColor.Equals((object) tableStyleElement.PatternColor) && this.PatternColorRGB.Equals((object) tableStyleElement.PatternColorRGB) && this.PatternStyle.Equals((object) tableStyleElement.PatternStyle) && this.Bold.Equals(tableStyleElement.Bold) && this.Italic.Equals(tableStyleElement.Italic) && this.StrikeThrough.Equals(tableStyleElement.StrikeThrough) && this.Underline.Equals((object) tableStyleElement.Underline) && this.TopBorderColor.Equals((object) tableStyleElement.TopBorderColor) && this.TopBorderColorRGB.Equals((object) tableStyleElement.TopBorderColorRGB) && this.TopBorderStyle.Equals((object) tableStyleElement.TopBorderStyle) && this.BottomBorderColor.Equals((object) tableStyleElement.BottomBorderColor) && this.BottomBorderColorRGB.Equals((object) tableStyleElement.BottomBorderColorRGB) && this.BottomBorderStyle.Equals((object) tableStyleElement.BottomBorderStyle) && this.RightBorderColor.Equals((object) tableStyleElement.RightBorderColor) && this.RightBorderColorRGB.Equals((object) tableStyleElement.RightBorderColorRGB) && this.RightBorderStyle.Equals((object) tableStyleElement.RightBorderStyle) && this.LeftBorderColor.Equals((object) tableStyleElement.LeftBorderColor) && this.LeftBorderColorRGB.Equals((object) tableStyleElement.LeftBorderColorRGB) && this.LeftBorderStyle.Equals((object) tableStyleElement.LeftBorderStyle) && this.HorizontalBorderColor.Equals((object) tableStyleElement.HorizontalBorderColor) && this.HorizontalBorderColorRGB.Equals((object) tableStyleElement.HorizontalBorderColorRGB) && this.HorizontalBorderStyle.Equals((object) tableStyleElement.HorizontalBorderStyle) && this.VerticalBorderColor.Equals((object) tableStyleElement.VerticalBorderColor) && this.VerticalBorderColorRGB.Equals((object) tableStyleElement.VerticalBorderColorRGB) && this.VerticalBorderStyle.Equals((object) tableStyleElement.VerticalBorderStyle);
  }

  internal void SetChanged()
  {
  }

  private void InitializeColors()
  {
    this.m_color = new ColorObject(ExcelKnownColors.None);
    this.m_color.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateColor);
    this.m_backColor = new ColorObject(ExcelKnownColors.None);
    this.m_backColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateBackColor);
    this.m_topBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_topBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateTopBorderFormat);
    this.m_bottomBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_bottomBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateBottomBorderFormat);
    this.m_leftBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_leftBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateLeftBorderFormat);
    this.m_rightBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_rightBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateRightBorderFormat);
    this.m_verticalBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_verticalBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateVerticalBorderFormat);
    this.m_horizontalBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_horizontalBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateHorizontalBorderFormat);
    this.m_diagonalBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_diagonalBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateDiagonalBorderFormat);
    this.m_fontColor = new ColorObject(ExcelKnownColors.Black);
    this.m_fontColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateFontColor);
  }

  internal void UpdateTopBorderFormat()
  {
    this.m_isTopBorderModified = true;
    this.m_isBorderFormatPresent = true;
  }

  internal void UpdateBottomBorderFormat()
  {
    this.m_isBottomBorderModified = true;
    this.m_isBorderFormatPresent = true;
  }

  internal void UpdateBackColor()
  {
    this.m_isBackColorModified = true;
    this.m_isPatternFormatPresent = true;
  }

  internal void UpdateColor()
  {
    this.m_isPatternColorModified = true;
    this.m_isPatternFormatPresent = true;
  }

  internal void UpdateLeftBorderFormat()
  {
    this.m_isLeftBorderModified = true;
    this.m_isBorderFormatPresent = true;
  }

  internal void UpdateRightBorderFormat()
  {
    this.m_isRightBorderModified = true;
    this.m_isBorderFormatPresent = true;
  }

  internal void UpdateDiagonalBorderFormat()
  {
    this.m_isDiagonalBorderModified = true;
    this.m_isBorderFormatPresent = true;
  }

  internal void UpdateFontColor()
  {
    this.m_isFontColorPresent = true;
    this.m_isFontFormatPresent = true;
  }

  internal void UpdateBorderStyle() => this.m_isBorderFormatPresent = true;

  internal void UpdateFontFormat() => this.m_isFontFormatPresent = true;

  internal void UpdatePatternFormat() => this.m_isPatternFormatPresent = true;

  internal void UpdateVerticalBorderFormat()
  {
    this.m_isVerticalBorderModified = true;
    this.m_isBorderFormatPresent = true;
  }

  internal void UpdateHorizontalBorderFormat()
  {
    this.m_isHorizontalBorderModified = true;
    this.m_isBorderFormatPresent = true;
  }
}
