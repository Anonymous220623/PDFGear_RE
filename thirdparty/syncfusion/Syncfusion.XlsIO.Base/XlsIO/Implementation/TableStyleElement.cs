// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TableStyleElement
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class TableStyleElement : ITableStyleElement
{
  internal const string WholeTable = "wholeTable";
  internal const string HeaderRow = "headerRow";
  internal const string TotalRow = "totalRow";
  internal const string FirstColumn = "firstColumn";
  internal const string LastColumn = "lastColumn";
  internal const string FirstRowStrip = "firstRowStripe";
  internal const string SecondRowStripe = "secondRowStripe";
  internal const string FirstColumnStripe = "firstColumnStripe";
  internal const string SecondColumnStripe = "secondColumnStripe";
  internal const string FirstHeaderCell = "firstHeaderCell";
  internal const string LastHeaderCell = "lastHeaderCell";
  internal const string FirstTotalCell = "firstTotalCell";
  internal const string LastTotalCell = "lastTotalCell";
  private int m_stripeSize = 1;
  private ExcelTableStyleElementType m_tableStyleElementType;
  private TableStyleElements m_tableStyleElements;
  private ColorObject m_backColor;
  private ExcelPattern m_pattern;
  private ColorObject m_color;
  private ColorObject m_fontColor;
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
  private bool m_isTopBorderModified;
  private bool m_isBottomBorderModified;
  private bool m_isLeftBorderModified;
  private bool m_isRightBorderModified;
  private bool m_isBorderFormatPresent;
  private bool m_isFontColorPresent;
  private bool m_isFontFormatPresent;
  private bool m_isPatternBackColorModified;
  private bool m_isPatternFormatPresent;
  private bool m_isPatternColorModified;
  private string m_tableStyleElementName;
  private bool m_isVerticalBorderModified;
  private bool m_isHorizontalBorderModified;
  private ColorObject m_verticalBorderColor;
  private ColorObject m_horizontalBorderColor;
  private ExcelLineStyle m_verticalBorderLineStyle;
  private ExcelLineStyle m_horizontalBorderLineStyle;

  public string TableStyleElementName
  {
    get => this.m_tableStyleElementName;
    set => this.m_tableStyleElementName = value;
  }

  public int StripeSize
  {
    get => this.m_stripeSize;
    set
    {
      this.m_stripeSize = value <= 9 ? value : throw new ArgumentException("Parameter value is invaild");
    }
  }

  public TableStyleElements TableStyleElements
  {
    get => this.m_tableStyleElements;
    set => this.m_tableStyleElements = value;
  }

  public ExcelTableStyleElementType TableStyleElementType
  {
    get => this.m_tableStyleElementType;
    set => this.m_tableStyleElementType = value;
  }

  public ExcelKnownColors BackColor
  {
    get
    {
      return this.m_backColor.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_backColor.SetIndexed(value);
  }

  public Color BackColorRGB
  {
    get
    {
      return this.m_backColor.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set
    {
      this.m_backColor.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
  }

  public ExcelPattern PatternStyle
  {
    get => this.m_pattern;
    set
    {
      this.UpdatePatternFormat();
      this.m_pattern = value;
    }
  }

  public Color PatternColorRGB
  {
    get => this.m_color.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    set
    {
      this.m_color.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
  }

  public ExcelKnownColors PatternColor
  {
    get
    {
      return this.m_color.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_color.SetIndexed(value);
  }

  public ExcelKnownColors FontColor
  {
    get
    {
      return this.m_fontColor.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_fontColor.SetIndexed(value);
  }

  public Color FontColorRGB
  {
    get
    {
      return this.m_fontColor.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set
    {
      this.m_fontColor.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
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
    get
    {
      return this.m_topBorderColor.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_topBorderColor.SetIndexed(value);
  }

  public Color TopBorderColorRGB
  {
    get
    {
      return this.m_topBorderColor.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set
    {
      this.m_topBorderColor.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
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
    get
    {
      return this.m_verticalBorderColor.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_verticalBorderColor.SetIndexed(value);
  }

  public Color VerticalBorderColorRGB
  {
    get
    {
      return this.m_verticalBorderColor.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set
    {
      this.m_verticalBorderColor.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
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
    get
    {
      return this.m_horizontalBorderColor.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_horizontalBorderColor.SetIndexed(value);
  }

  public Color HorizontalBorderColorRGB
  {
    get
    {
      return this.m_horizontalBorderColor.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set
    {
      this.m_horizontalBorderColor.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
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
    get
    {
      return this.m_bottomBorderColor.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_bottomBorderColor.SetIndexed(value);
  }

  public Color BottomBorderColorRGB
  {
    get
    {
      return this.m_bottomBorderColor.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set
    {
      this.m_bottomBorderColor.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
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
    get
    {
      return this.m_rightBorderColor.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_rightBorderColor.SetIndexed(value);
  }

  public Color RightBorderColorRGB
  {
    get
    {
      return this.m_rightBorderColor.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set
    {
      this.m_rightBorderColor.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
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
    get
    {
      return this.m_leftBorderColor.GetIndexed((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set => this.m_leftBorderColor.SetIndexed(value);
  }

  public Color LeftBorderColorRGB
  {
    get
    {
      return this.m_leftBorderColor.GetRGB((IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
    set
    {
      this.m_leftBorderColor.SetRGB(value, (IWorkbook) this.TableStyleElements.TableStyle.TableStyles.Workbook);
    }
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

  internal bool IsTopBorderModified
  {
    get => this.m_isTopBorderModified;
    set => this.m_isTopBorderModified = value;
  }

  internal bool IsBottomBorderModified
  {
    get => this.m_isBottomBorderModified;
    set => this.m_isBottomBorderModified = value;
  }

  internal bool IsRightBorderModified
  {
    get => this.m_isRightBorderModified;
    set => this.m_isRightBorderModified = value;
  }

  internal bool IsLeftBorderModified
  {
    get => this.m_isLeftBorderModified;
    set => this.m_isLeftBorderModified = value;
  }

  internal bool IsFontFormatPresent
  {
    get => this.m_isFontFormatPresent;
    set => this.m_isFontFormatPresent = value;
  }

  internal bool IsPatternColorModified
  {
    get => this.m_isPatternColorModified;
    set => this.m_isPatternColorModified = value;
  }

  internal bool IsPatternFormatPresent
  {
    get => this.m_isPatternFormatPresent;
    set => this.m_isPatternFormatPresent = value;
  }

  internal bool IsBackgroundColorPresent
  {
    get => this.m_isPatternBackColorModified;
    set => this.m_isPatternBackColorModified = value;
  }

  internal bool IsBorderFormatPresent
  {
    get => this.m_isBorderFormatPresent;
    set => this.m_isBorderFormatPresent = value;
  }

  internal bool IsFontColorPresent
  {
    get => this.m_isFontFormatPresent;
    set => this.m_isFontFormatPresent = value;
  }

  internal ColorObject FontColorObject => this.m_fontColor;

  internal ColorObject ColorObject => this.m_color;

  internal ColorObject BackColorObject => this.m_backColor;

  internal ColorObject TopBorderColorObject => this.m_topBorderColor;

  internal ColorObject BottomBorderColorObject => this.m_bottomBorderColor;

  internal ColorObject HorizontalBorderColorObject
  {
    get => this.m_horizontalBorderColor;
    set => this.m_horizontalBorderColor = value;
  }

  internal ColorObject VerticalBorderColorObject
  {
    get => this.m_verticalBorderColor;
    set => this.m_verticalBorderColor = value;
  }

  internal ColorObject RightBorderColorObject => this.m_rightBorderColor;

  internal ColorObject LeftBorderColorObject => this.m_leftBorderColor;

  internal bool IsVerticalBorderModified
  {
    get => this.m_isVerticalBorderModified;
    set => this.m_isVerticalBorderModified = value;
  }

  internal bool IsHorizontalBorderModified
  {
    get => this.m_isHorizontalBorderModified;
    set => this.m_isHorizontalBorderModified = value;
  }

  public TableStyleElement(
    ExcelTableStyleElementType tableStyleElementType,
    TableStyleElements tableStyleElements)
  {
    this.m_tableStyleElementType = tableStyleElementType;
    this.m_tableStyleElements = tableStyleElements;
    this.InitializeColors();
  }

  public TableStyleElement(string tableStyleElementName, TableStyleElements tableStyleElements)
  {
    this.m_tableStyleElementName = tableStyleElementName;
    this.m_tableStyleElements = tableStyleElements;
    this.InitializeColors();
  }

  public TableStyleElement(TableStyleElements tableStyleElements)
  {
    this.m_tableStyleElements = tableStyleElements;
    this.InitializeColors();
  }

  public void Clear()
  {
    this.TableStyleElements.Remove((ITableStyleElement) this);
    this.Dispose();
  }

  internal object Clone(TableStyleElements tableStyleElements)
  {
    TableStyleElement tableStyleElement = (TableStyleElement) this.MemberwiseClone();
    tableStyleElement.m_tableStyleElements = tableStyleElements;
    return (object) tableStyleElement;
  }

  public ITableStyleElement Clone()
  {
    TableStyleElement tableStyleElement = (TableStyleElement) this.MemberwiseClone();
    tableStyleElement.m_tableStyleElements = this.TableStyleElements;
    return (ITableStyleElement) tableStyleElement;
  }

  internal void Dispose() => this.m_tableStyleElements = (TableStyleElements) null;

  public override bool Equals(object obj)
  {
    TableStyleElement tableStyleElement = (TableStyleElement) obj;
    return this.TableStyleElementType.Equals((object) tableStyleElement.TableStyleElementType) && this.BackColor.Equals((object) tableStyleElement.BackColor) && this.BackColorRGB.Equals((object) tableStyleElement.BackColorRGB) && this.FontColor.Equals((object) tableStyleElement.FontColor) && this.FontColorRGB.Equals((object) tableStyleElement.FontColorRGB) && this.PatternColor.Equals((object) tableStyleElement.PatternColor) && this.PatternColorRGB.Equals((object) tableStyleElement.PatternColorRGB) && this.PatternStyle.Equals((object) tableStyleElement.PatternStyle) && this.Bold.Equals(tableStyleElement.Bold) && this.Italic.Equals(tableStyleElement.Italic) && this.StrikeThrough.Equals(tableStyleElement.StrikeThrough) && this.Underline.Equals((object) tableStyleElement.Underline) && this.TopBorderColor.Equals((object) tableStyleElement.TopBorderColor) && this.TopBorderColorRGB.Equals((object) tableStyleElement.TopBorderColorRGB) && this.TopBorderStyle.Equals((object) tableStyleElement.TopBorderStyle) && this.BottomBorderColor.Equals((object) tableStyleElement.BottomBorderColor) && this.BottomBorderColorRGB.Equals((object) tableStyleElement.BottomBorderColorRGB) && this.BottomBorderStyle.Equals((object) tableStyleElement.BottomBorderStyle) && this.RightBorderColor.Equals((object) tableStyleElement.RightBorderColor) && this.RightBorderColorRGB.Equals((object) tableStyleElement.RightBorderColorRGB) && this.RightBorderStyle.Equals((object) tableStyleElement.RightBorderStyle) && this.LeftBorderColor.Equals((object) tableStyleElement.LeftBorderColor) && this.LeftBorderColorRGB.Equals((object) tableStyleElement.LeftBorderColorRGB) && this.LeftBorderStyle.Equals((object) tableStyleElement.LeftBorderStyle) && this.HorizontalBorderColor.Equals((object) tableStyleElement.HorizontalBorderColor) && this.HorizontalBorderColorRGB.Equals((object) tableStyleElement.HorizontalBorderColorRGB) && this.HorizontalBorderStyle.Equals((object) tableStyleElement.HorizontalBorderStyle) && this.VerticalBorderColor.Equals((object) tableStyleElement.VerticalBorderColor) && this.VerticalBorderColorRGB.Equals((object) tableStyleElement.VerticalBorderColorRGB) && this.VerticalBorderStyle.Equals((object) tableStyleElement.VerticalBorderStyle);
  }

  internal bool Equals(ExcelTableStyleElementType tableStyleElementType)
  {
    return this.TableStyleElementType.Equals((object) tableStyleElementType);
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
    this.m_isPatternBackColorModified = true;
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
