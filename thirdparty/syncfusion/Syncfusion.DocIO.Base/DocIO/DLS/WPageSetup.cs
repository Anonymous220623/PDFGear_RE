// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WPageSetup
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WPageSetup : FormatBase
{
  private const float DEF_PAGE_WIDTH = 595.3f;
  private const float DEF_PAGE_HEIGHT = 841.9f;
  private const float DEF_PAGE_MARGINS = 20f;
  private const float DEF_PAGE_MARGIN_LEFT = 50f;
  internal const float DEF_AUTO_TAB_LENGHT = 36f;
  private const float DEF_AR_TO_LETTER_LIMIT = 26f;
  private const int DEF_A_ASCII_INDEX = 64 /*0x40*/;
  internal const int LinePitchKey = 1;
  internal const int PitchTypeKey = 2;
  internal const int VerticalAlignKey = 3;
  internal const int PageOrientKey = 4;
  internal const int PageSizeKey = 5;
  internal const int EqualColWidthKey = 6;
  internal const int MarginKey = 7;
  internal const int DrawLinesBetwColsKey = 8;
  internal const int LineNumberingModeKey = 9;
  internal const int FootnotePositionKey = 10;
  internal const int FootnoteNumberFormatKey = 11;
  internal const int EndnoteNumberFormatKey = 12;
  internal const int RestartIndexForFootnotesKey = 13;
  internal const int RestartIndexForEndnoteKey = 14;
  internal const int InitialFootnoteNumberKey = 15;
  internal const int InitialEndnoteNumberKey = 16 /*0x10*/;
  internal const int PageNumberStyleKey = 17;
  internal const int PageNumberRestartKey = 18;
  internal const int PageNumberStartAtKey = 19;
  internal const int BidiKey = 20;
  internal const int EndnotePositionKey = 21;
  internal const int HeaderDistanceKey = 22;
  internal const int FooterDistanceKey = 23;
  internal const int OtherPagesTrayKey = 24;
  internal const int FirstPageTrayKey = 25;
  internal const int LineNumberingStartValueKey = 27;
  internal const int PageBorderIsInFrontKey = 28;
  internal const int PageBorderOffsetFromKey = 29;
  internal const int LineNumDistanceFromTextKey = 30;
  internal const int PageBorderApplyKey = 31 /*0x1F*/;
  internal const int DifferentFirstPageKey = 32 /*0x20*/;
  internal const int DifferentOddAndEvenPageKey = 33;
  internal const int BorderKey = 34;
  internal const int PageNumbersKey = 35;
  internal const int spaceKey = 36;
  internal const int NumberOfColumnsKey = 37;
  internal const int LineNumnberingModKey = 39;

  internal FootEndNoteNumberFormat EndnoteNumberFormat
  {
    get => (FootEndNoteNumberFormat) this.GetPropertyValue(12);
    set => this.SetPropertyValue(12, (object) value);
  }

  internal FootEndNoteNumberFormat FootnoteNumberFormat
  {
    get => (FootEndNoteNumberFormat) this.GetPropertyValue(11);
    set => this.SetPropertyValue(11, (object) value);
  }

  internal EndnoteRestartIndex RestartIndexForEndnote
  {
    get => (EndnoteRestartIndex) this.GetPropertyValue(14);
    set => this.SetPropertyValue(14, (object) value);
  }

  internal FootnoteRestartIndex RestartIndexForFootnotes
  {
    get => (FootnoteRestartIndex) this.GetPropertyValue(13);
    set => this.SetPropertyValue(13, (object) value);
  }

  internal FootnotePosition FootnotePosition
  {
    get => (FootnotePosition) this.GetPropertyValue(10);
    set => this.SetPropertyValue(10, (object) value);
  }

  internal EndnotePosition EndnotePosition
  {
    get => (EndnotePosition) this.GetPropertyValue(21);
    set => this.SetPropertyValue(21, (object) value);
  }

  internal int InitialFootnoteNumber
  {
    get => (int) this.GetPropertyValue(15);
    set => this.SetPropertyValue(15, (object) value);
  }

  internal int InitialEndnoteNumber
  {
    get => (int) this.GetPropertyValue(16 /*0x10*/);
    set => this.SetPropertyValue(16 /*0x10*/, (object) value);
  }

  [Obsolete("This property has been deprecated. Use the DefaultTabWidth property of WordDocument class to set default tab width for the document.")]
  public float DefaultTabWidth
  {
    get => this.Document == null ? 36f : this.Document.DefaultTabWidth;
    set
    {
      if (this.Document == null)
        return;
      this.Document.DefaultTabWidth = value;
    }
  }

  public SizeF PageSize
  {
    get => (SizeF) this.GetPropertyValue(5);
    set
    {
      this.SetPropertyValue(5, (object) value);
      if (this.Document.IsOpening || this.Document.IsCloning)
        return;
      if ((double) this.PageSize.Height >= (double) this.PageSize.Width)
        this.SetPropertyValue(4, (object) PageOrientation.Portrait);
      else
        this.SetPropertyValue(4, (object) PageOrientation.Landscape);
    }
  }

  public PageOrientation Orientation
  {
    get => (PageOrientation) this.GetPropertyValue(4);
    set
    {
      if (this.Orientation == value)
        return;
      if (!this.Document.IsOpening && !this.Document.IsCloning)
      {
        this.PageSize = new SizeF(this.PageSize.Height, this.PageSize.Width);
        if ((double) this.PageSize.Height >= (double) this.PageSize.Width)
          this.SetPropertyValue(4, (object) PageOrientation.Portrait);
        else
          this.SetPropertyValue(4, (object) PageOrientation.Landscape);
      }
      else
        this.SetPropertyValue(4, (object) value);
    }
  }

  public PageAlignment VerticalAlignment
  {
    get => (PageAlignment) this.GetPropertyValue(3);
    set => this.SetPropertyValue(3, (object) value);
  }

  public MarginsF Margins
  {
    get => (MarginsF) this.GetPropertyValue(7);
    set => this.SetPropertyValue(7, (object) value);
  }

  public float HeaderDistance
  {
    get => (float) this.GetPropertyValue(22);
    set
    {
      if ((double) value < 0.0 || (double) value > 1584.0)
        throw new ArgumentException("HeaderDistance must be between 0 pt and 1584 pt.");
      this.SetPropertyValue(22, (object) value);
    }
  }

  public float FooterDistance
  {
    get => (float) this.GetPropertyValue(23);
    set
    {
      if ((double) value < 0.0 || (double) value > 1584.0)
        throw new ArgumentException("FooterDistance must be between 0 pt and 1584 pt.");
      this.SetPropertyValue(23, (object) value);
    }
  }

  public float ClientWidth => this.PageSize.Width - this.Margins.Left - this.Margins.Right;

  public bool DifferentFirstPage
  {
    get => (bool) this.GetPropertyValue(32 /*0x20*/);
    set => this.SetPropertyValue(32 /*0x20*/, (object) value);
  }

  public bool DifferentOddAndEvenPages
  {
    get => this.Document != null && this.Document.DifferentOddAndEvenPages;
    set
    {
      if (this.Document == null)
        return;
      this.Document.DifferentOddAndEvenPages = value;
    }
  }

  public LineNumberingMode LineNumberingMode
  {
    get => (LineNumberingMode) this.GetPropertyValue(9);
    set
    {
      this.SetPropertyValue(9, (object) value);
      if (this.LineNumberingStep != 0)
        return;
      this.SetPropertyValue(39, (object) 1);
    }
  }

  public int LineNumberingStep
  {
    get => (int) this.GetPropertyValue(39);
    set
    {
      if (value < 1 || value > 100)
        throw new ArgumentException("LineNumberingStep must be between 1 and 100");
      this.SetPropertyValue(39, (object) value);
    }
  }

  public int LineNumberingStartValue
  {
    get => (int) this.GetPropertyValue(27);
    set
    {
      if (value < 1 || value > (int) short.MaxValue)
        throw new ArgumentException("LineNumberingStartValue must be between 1 and 32767");
      this.SetPropertyValue(27, (object) value);
      if (this.LineNumberingStep != 0)
        return;
      this.SetPropertyValue(39, (object) 1);
    }
  }

  public float LineNumberingDistanceFromText
  {
    get => (float) this.GetPropertyValue(30);
    set
    {
      if ((double) value < 0.0 || (double) value > 1584.0)
        throw new ArgumentException("LineNumberingDistanceFromText must be between 1 pt and 1584 pt.");
      this.SetPropertyValue(30, (object) value);
      if (this.LineNumberingStep != 0)
        return;
      this.SetPropertyValue(39, (object) 1);
    }
  }

  public PageBordersApplyType PageBordersApplyType
  {
    get => (PageBordersApplyType) this.GetPropertyValue(31 /*0x1F*/);
    set => this.SetPropertyValue(31 /*0x1F*/, (object) value);
  }

  public PageBorderOffsetFrom PageBorderOffsetFrom
  {
    get => (PageBorderOffsetFrom) this.GetPropertyValue(29);
    set => this.SetPropertyValue(29, (object) value);
  }

  public bool IsFrontPageBorder
  {
    get => (bool) this.GetPropertyValue(28);
    set => this.SetPropertyValue(28, (object) value);
  }

  public Borders Borders
  {
    get => (Borders) this.GetPropertyValue(34);
    internal set => this.SetPropertyValue(34, (object) value);
  }

  public bool Bidi
  {
    get => (bool) this.GetPropertyValue(20);
    set => this.SetPropertyValue(20, (object) value);
  }

  internal bool EqualColumnWidth
  {
    get => (bool) this.GetPropertyValue(6);
    set => this.SetPropertyValue(6, (object) value);
  }

  public PageNumberStyle PageNumberStyle
  {
    get => (PageNumberStyle) this.GetPropertyValue(17);
    set => this.SetPropertyValue(17, (object) value);
  }

  public int PageStartingNumber
  {
    get => (int) this.GetPropertyValue(19);
    set => this.SetPropertyValue(19, (object) value);
  }

  public bool RestartPageNumbering
  {
    get => (bool) this.GetPropertyValue(18);
    set => this.SetPropertyValue(18, (object) value);
  }

  internal float LinePitch
  {
    get => (float) this.GetPropertyValue(1);
    set => this.SetPropertyValue(1, (object) value);
  }

  internal GridPitchType PitchType
  {
    get => (GridPitchType) this.GetPropertyValue(2);
    set => this.SetPropertyValue(2, (object) value);
  }

  internal bool DrawLinesBetweenCols
  {
    get => (bool) this.GetPropertyValue(8);
    set => this.SetPropertyValue(8, (object) value);
  }

  public PageNumbers PageNumbers
  {
    get => (PageNumbers) this.GetPropertyValue(35);
    internal set => this.SetPropertyValue(35, (object) value);
  }

  public PrinterPaperTray FirstPageTray
  {
    get => (PrinterPaperTray) this.GetPropertyValue(25);
    set => this.SetPropertyValue(25, (object) value);
  }

  public PrinterPaperTray OtherPagesTray
  {
    get => (PrinterPaperTray) this.GetPropertyValue(24);
    set => this.SetPropertyValue(24, (object) value);
  }

  internal int NumberOfColumns
  {
    get => (int) this.GetPropertyValue(37);
    set => this.SetPropertyValue(37, (object) value);
  }

  internal float ColumnSpace
  {
    get => (float) this.GetPropertyValue(36);
    set => this.SetPropertyValue(36, (object) value);
  }

  internal WPageSetup(WSection sec)
    : base((IWordDocument) sec.Document, (Entity) sec)
  {
    this.PageSize = new SizeF(595.3f, 841.9f);
    this.Margins = new MarginsF();
    if (this.IsFormattingChange)
    {
      this.Margins.SetOldPropertyHashMarginValues(50f, 20f, 20f, 20f, this.Margins.Gutter);
    }
    else
    {
      this.Margins.All = 20f;
      this.Margins.Left = 50f;
    }
    this.SetPropertyValue(22, (object) -0.05f);
    this.SetPropertyValue(23, (object) -0.05f);
  }

  internal void SetPageSetupProperty(string propertyName, object propertyValue)
  {
    switch (propertyName)
    {
      case "HeaderDistance":
        this.SetPropertyValue(22, (object) (float) propertyValue);
        break;
      case "FooterDistance":
        this.SetPropertyValue(23, (object) (float) propertyValue);
        break;
      case "LineNumberingStep":
        this.SetPropertyValue(39, (object) (int) propertyValue);
        break;
      case "LineNumberingStartValue":
        this.SetPropertyValue(27, (object) (int) propertyValue);
        break;
      case "LineNumberingDistanceFromText":
        this.SetPropertyValue(30, (object) (float) propertyValue);
        break;
      case "LineNumberingMode":
        this.SetPropertyValue(9, (object) (LineNumberingMode) propertyValue);
        break;
    }
  }

  internal void InitializeDocxPageSetup()
  {
    this.PageSize = new SizeF(612f, 792f);
    if (this.IsFormattingChange)
    {
      this.Margins.SetOldPropertyHashMarginValues(72f, 72f, 72f, 72f, 0.0f);
    }
    else
    {
      this.Margins.All = 72f;
      this.Margins.Gutter = 0.0f;
    }
    this.SetPropertyValue(23, (object) 36f);
    this.SetPropertyValue(22, (object) 36f);
  }

  internal bool Compare(WPageSetup pageSetup)
  {
    return this.Compare(1, (FormatBase) pageSetup) && this.Compare(2, (FormatBase) pageSetup) && this.Compare(4, (FormatBase) pageSetup) && this.Compare(3, (FormatBase) pageSetup) && this.Compare(5, (FormatBase) pageSetup) && this.Compare(6, (FormatBase) pageSetup) && this.Compare(8, (FormatBase) pageSetup) && this.Compare(9, (FormatBase) pageSetup) && this.Compare(10, (FormatBase) pageSetup) && this.Compare(21, (FormatBase) pageSetup) && this.Compare(11, (FormatBase) pageSetup) && this.Compare(12, (FormatBase) pageSetup) && this.Compare(13, (FormatBase) pageSetup) && this.Compare(14, (FormatBase) pageSetup) && this.Compare(15, (FormatBase) pageSetup) && this.Compare(16 /*0x10*/, (FormatBase) pageSetup) && this.Compare(17, (FormatBase) pageSetup) && this.Compare(18, (FormatBase) pageSetup) && this.Compare(19, (FormatBase) pageSetup) && this.Compare(22, (FormatBase) pageSetup) && this.Compare(20, (FormatBase) pageSetup) && this.Compare(23, (FormatBase) pageSetup) && this.Compare(24, (FormatBase) pageSetup) && this.Compare(25, (FormatBase) pageSetup) && this.Compare(39, (FormatBase) pageSetup) && this.Compare(27, (FormatBase) pageSetup) && this.Compare(28, (FormatBase) pageSetup) && this.Compare(29, (FormatBase) pageSetup) && this.Compare(31 /*0x1F*/, (FormatBase) pageSetup) && this.Compare(30, (FormatBase) pageSetup) && this.Compare(32 /*0x20*/, (FormatBase) pageSetup) && this.Compare(33, (FormatBase) pageSetup) && this.Compare(37, (FormatBase) pageSetup) && this.Compare(36, (FormatBase) pageSetup) && (this.Margins == null || pageSetup.Margins == null || this.Margins.Compare(pageSetup.Margins)) && (this.Borders == null || pageSetup.Borders == null || this.Borders.Compare(pageSetup.Borders)) && (this.PageNumbers == null || pageSetup.PageNumbers == null || this.PageNumbers.Compare(pageSetup.PageNumbers));
  }

  public void InsertPageNumbers(bool topOfPage, PageNumberAlignment horizontalAlignment)
  {
    HeaderFooter headerFooter = topOfPage ? (this.OwnerBase as WSection).HeadersFooters.Header : (this.OwnerBase as WSection).HeadersFooters.Footer;
    IWParagraph wparagraph = (IWParagraph) null;
    IWField wfield1 = (IWField) null;
    int index1 = 0;
    for (int count1 = headerFooter.Paragraphs.Count; index1 < count1; ++index1)
    {
      wparagraph = (IWParagraph) headerFooter.Paragraphs[index1];
      int index2 = 0;
      for (int count2 = wparagraph.Items.Count; index2 < count2; ++index2)
      {
        if (wparagraph.Items[index2].EntityType == EntityType.Field)
        {
          WField wfield2 = (WField) wparagraph.Items[index2];
          if (wfield2.FieldType == FieldType.FieldPage)
          {
            wfield1 = (IWField) wfield2;
            break;
          }
        }
      }
    }
    if (wfield1 == null)
    {
      wparagraph = headerFooter.AddParagraph();
      wparagraph.AppendField("", FieldType.FieldPage);
    }
    wparagraph.ParagraphFormat.WrapFrameAround = FrameWrapMode.Around;
    wparagraph.ParagraphFormat.FrameX = (float) (short) horizontalAlignment;
    wparagraph.ParagraphFormat.FrameVerticalPos = (byte) 2;
  }

  internal string GetNumberFormatValue(byte numberFormat, int number)
  {
    number.ToString();
    string numberFormatValue;
    switch (numberFormat)
    {
      case 1:
        numberFormatValue = this.GetAsRoman(number).ToUpper();
        break;
      case 2:
        numberFormatValue = this.GetAsRoman(number).ToLower();
        break;
      case 3:
        numberFormatValue = this.GetAsLetter(number).ToUpper();
        break;
      case 4:
        numberFormatValue = this.GetAsLetter(number).ToLower();
        break;
      default:
        numberFormatValue = number.ToString();
        break;
    }
    return numberFormatValue;
  }

  private string GetAsRoman(int number)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.GenerateNumber(ref number, 1000, "M"));
    stringBuilder.Append(this.GenerateNumber(ref number, 900, "CM"));
    stringBuilder.Append(this.GenerateNumber(ref number, 500, "D"));
    stringBuilder.Append(this.GenerateNumber(ref number, 400, "CD"));
    stringBuilder.Append(this.GenerateNumber(ref number, 100, "C"));
    stringBuilder.Append(this.GenerateNumber(ref number, 90, "XC"));
    stringBuilder.Append(this.GenerateNumber(ref number, 50, "L"));
    stringBuilder.Append(this.GenerateNumber(ref number, 40, "XL"));
    stringBuilder.Append(this.GenerateNumber(ref number, 10, "X"));
    stringBuilder.Append(this.GenerateNumber(ref number, 9, "IX"));
    stringBuilder.Append(this.GenerateNumber(ref number, 5, "V"));
    stringBuilder.Append(this.GenerateNumber(ref number, 4, "IV"));
    stringBuilder.Append(this.GenerateNumber(ref number, 1, "I"));
    return stringBuilder.ToString();
  }

  private string GetAsLetter(int number)
  {
    Stack<int> letter = WPageSetup.ConvertToLetter((float) number);
    StringBuilder builder = new StringBuilder();
    while (letter.Count > 0)
    {
      int number1 = letter.Pop();
      WPageSetup.AppendChar(builder, number1);
    }
    return builder.ToString();
  }

  private static void AppendChar(StringBuilder builder, int number)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    if (number <= 0 || number > 26)
      throw new ArgumentOutOfRangeException(nameof (number), "Value can not be less 0 and greater 26");
    char ch = (char) (64 /*0x40*/ + number);
    builder.Append(ch);
  }

  private static Stack<int> ConvertToLetter(float arabic)
  {
    if ((double) arabic < 0.0)
      throw new ArgumentOutOfRangeException(nameof (arabic), (object) arabic, "Value can not be less 0");
    Stack<int> letter = new Stack<int>();
    while ((double) (int) arabic > 26.0)
    {
      float num = arabic % 26f;
      if ((double) num == 0.0)
      {
        arabic = (float) ((double) arabic / 26.0 - 1.0);
        num = 26f;
      }
      else
        arabic /= 26f;
      letter.Push((int) num);
    }
    if ((double) arabic > 0.0)
      letter.Push((int) arabic);
    return letter;
  }

  private string GenerateNumber(ref int value, int magnitude, string letter)
  {
    StringBuilder stringBuilder = new StringBuilder();
    while (value >= magnitude)
    {
      value -= magnitude;
      stringBuilder.Append(letter);
    }
    return stringBuilder.ToString();
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if ((double) this.DefaultTabWidth != 36.0)
      writer.WriteValue("AutoTabWidth", this.DefaultTabWidth);
    if ((double) this.PageSize.Height != 0.0)
      writer.WriteValue("PageHeight", this.PageSize.Height);
    if ((double) this.PageSize.Width != 0.0)
      writer.WriteValue("PageWidth", this.PageSize.Width);
    if (this.VerticalAlignment != PageAlignment.Top)
      writer.WriteValue("Alignment", (Enum) this.VerticalAlignment);
    if ((double) this.FooterDistance >= 0.0)
      writer.WriteValue("FooterDistance", this.FooterDistance);
    if ((double) this.HeaderDistance >= 0.0)
      writer.WriteValue("HeaderDistance", this.HeaderDistance);
    if (this.Orientation != PageOrientation.Portrait)
      writer.WriteValue("Orientation", (Enum) this.Orientation);
    if ((double) this.Margins.Bottom >= 0.0)
      writer.WriteValue("BottomMargin", this.Margins.Bottom);
    if ((double) this.Margins.Top >= 0.0)
      writer.WriteValue("TopMargin", this.Margins.Top);
    if ((double) this.Margins.Left >= 0.0)
      writer.WriteValue("LeftMargin", this.Margins.Left);
    if ((double) this.Margins.Right >= 0.0)
      writer.WriteValue("RightMargin", this.Margins.Right);
    if (this.DifferentFirstPage)
      writer.WriteValue("DifferentFirstPage", this.DifferentFirstPage);
    if (this.DifferentOddAndEvenPages)
      writer.WriteValue("DifferentOddEvenPage", this.DifferentOddAndEvenPages);
    if (this.LineNumberingMode != LineNumberingMode.None)
    {
      writer.WriteValue("PageSetupLineNumMode", (Enum) this.LineNumberingMode);
      if (this.LineNumberingStep != 0)
        writer.WriteValue("PageSetupLineNumStep", this.LineNumberingStep);
      if ((double) this.LineNumberingDistanceFromText != 0.0)
        writer.WriteValue("PageSetupLineNumDistance", this.LineNumberingDistanceFromText);
      writer.WriteValue("PageSetupLineNumStartValue", this.LineNumberingStartValue);
    }
    if (this.PageBordersApplyType != PageBordersApplyType.AllPages)
      writer.WriteValue("PageSetupBorderApply", (Enum) this.PageBordersApplyType);
    if (!this.IsFrontPageBorder)
      writer.WriteValue("PageSetupBorderIsInFront", this.IsFrontPageBorder);
    if (this.PageBorderOffsetFrom != PageBorderOffsetFrom.Text)
      writer.WriteValue("PageSetupBorderOffsetFrom", (Enum) this.PageBorderOffsetFrom);
    if (!this.EqualColumnWidth)
      return;
    writer.WriteValue("EqualColWidth", this.EqualColumnWidth);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("AutoTabWidth"))
      this.DefaultTabWidth = reader.ReadFloat("AutoTabWidth");
    if (reader.HasAttribute("PageHeight"))
      this.PageSize = new SizeF(this.PageSize.Width, reader.ReadFloat("PageHeight"));
    if (reader.HasAttribute("PageWidth"))
      this.PageSize = new SizeF(reader.ReadFloat("PageWidth"), this.PageSize.Height);
    if (reader.HasAttribute("Alignment"))
      this.VerticalAlignment = (PageAlignment) reader.ReadEnum("Alignment", typeof (PageAlignment));
    if (reader.HasAttribute("FooterDistance"))
      this.SetPageSetupProperty("FooterDistance", (object) reader.ReadFloat("FooterDistance"));
    if (reader.HasAttribute("HeaderDistance"))
      this.SetPageSetupProperty("HeaderDistance", (object) reader.ReadFloat("HeaderDistance"));
    if (reader.HasAttribute("Orientation"))
      this.Orientation = (PageOrientation) reader.ReadEnum("Orientation", typeof (PageOrientation));
    if (reader.HasAttribute("BottomMargin"))
      this.Margins.Bottom = reader.ReadFloat("BottomMargin");
    if (reader.HasAttribute("TopMargin"))
      this.Margins.Top = reader.ReadFloat("TopMargin");
    if (reader.HasAttribute("LeftMargin"))
      this.Margins.Left = reader.ReadFloat("LeftMargin");
    if (reader.HasAttribute("RightMargin"))
      this.Margins.Right = reader.ReadFloat("RightMargin");
    if (reader.HasAttribute("DifferentFirstPage"))
      this.DifferentFirstPage = reader.ReadBoolean("DifferentFirstPage");
    if (reader.HasAttribute("DifferentOddEvenPage"))
      this.DifferentOddAndEvenPages = reader.ReadBoolean("DifferentOddEvenPage");
    if (reader.HasAttribute("PageSetupLineNumStep"))
      this.SetPageSetupProperty("LineNumberingStep", (object) reader.ReadInt("PageSetupLineNumStep"));
    if (reader.HasAttribute("PageSetupLineNumDistance"))
      this.SetPageSetupProperty("LineNumberingDistanceFromText", (object) reader.ReadFloat("PageSetupLineNumDistance"));
    if (reader.HasAttribute("PageSetupLineNumMode"))
      this.SetPageSetupProperty("LineNumberingMode", (object) (LineNumberingMode) reader.ReadEnum("PageSetupLineNumMode", typeof (LineNumberingMode)));
    if (reader.HasAttribute("PageSetupLineNumStartValue"))
      this.SetPageSetupProperty("LineNumberingStartValue", (object) reader.ReadInt("PageSetupLineNumStartValue"));
    if (reader.HasAttribute("PageSetupBorderApply"))
      this.PageBordersApplyType = (PageBordersApplyType) reader.ReadEnum("PageSetupBorderApply", typeof (PageBordersApplyType));
    if (reader.HasAttribute("PageSetupBorderIsInFront"))
      this.IsFrontPageBorder = reader.ReadBoolean("PageSetupBorderIsInFront");
    if (reader.HasAttribute("PageSetupBorderOffsetFrom"))
      this.PageBorderOffsetFrom = (PageBorderOffsetFrom) reader.ReadEnum("PageSetupBorderOffsetFrom", typeof (PageBorderOffsetFrom));
    if (!reader.HasAttribute("EqualColWidth"))
      return;
    this.EqualColumnWidth = reader.ReadBoolean("EqualColWidth");
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("borders", (object) this.Borders);
  }

  public override string ToString() => base.ToString();

  internal WPageSetup Clone()
  {
    WPageSetup owner = new WPageSetup(this.OwnerBase as WSection);
    owner.ImportContainer((FormatBase) this);
    owner.CopyProperties((FormatBase) this);
    owner.Borders = this.Borders.Clone();
    owner.Borders.SetOwner((OwnerHolder) owner);
    owner.Margins = this.Margins.Clone();
    owner.Margins.SetOwner((OwnerHolder) owner);
    owner.PageNumbers = this.PageNumbers.Clone();
    owner.PageNumbers.SetOwner((OwnerHolder) owner);
    owner.PageSize = this.ClonePageSize();
    return owner;
  }

  internal override void Close()
  {
    if (this.Borders != null)
      this.Borders.Close();
    if (this.PageNumbers != null)
      this.PageNumbers.Close();
    if (this.Margins != null)
      this.Margins.Close();
    base.Close();
  }

  internal SizeF ClonePageSize() => new SizeF(this.PageSize);

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  protected internal override void EnsureComposites() => this.EnsureComposites(34);

  protected override FormatBase GetDefComposite(int key)
  {
    switch (key)
    {
      case 7:
        return this.GetDefComposite(7, (FormatBase) new MarginsF());
      case 34:
        return this.GetDefComposite(34, (FormatBase) new Borders());
      case 35:
        return this.GetDefComposite(35, (FormatBase) new PageNumbers());
      default:
        return (FormatBase) null;
    }
  }

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 1:
        return (object) 0.0f;
      case 2:
        return (object) GridPitchType.NoGrid;
      case 3:
        return (object) PageAlignment.Top;
      case 4:
        return (object) PageOrientation.Portrait;
      case 5:
        return (object) new SizeF();
      case 6:
        return (object) true;
      case 8:
        return (object) false;
      case 9:
        return (object) LineNumberingMode.None;
      case 10:
        return (object) FootnotePosition.PrintAtBottomOfPage;
      case 11:
        return (object) FootEndNoteNumberFormat.Arabic;
      case 12:
        return (object) FootEndNoteNumberFormat.LowerCaseRoman;
      case 13:
      case 14:
        return (object) EndnoteRestartIndex.DoNotRestart;
      case 15:
      case 16 /*0x10*/:
        return (object) 1;
      case 17:
        return (object) PageNumberStyle.Arabic;
      case 18:
      case 20:
        return (object) false;
      case 19:
        return (object) 0;
      case 21:
        return (object) EndnotePosition.DisplayEndOfDocument;
      case 22:
      case 23:
        return (object) 0.0f;
      case 24:
      case 25:
      case 27:
      case 39:
        return (object) 0;
      case 28:
        return (object) true;
      case 29:
        return (object) PageBorderOffsetFrom.Text;
      case 30:
        return (object) 18f;
      case 31 /*0x1F*/:
        return (object) PageBordersApplyType.AllPages;
      case 32 /*0x20*/:
      case 33:
        return (object) false;
      case 36:
        return (object) 0.0f;
      case 37:
        return (object) 1;
      default:
        throw new ArgumentException("key not found");
    }
  }
}
