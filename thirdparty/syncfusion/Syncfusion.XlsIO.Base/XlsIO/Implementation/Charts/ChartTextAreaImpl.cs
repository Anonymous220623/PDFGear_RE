// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartTextAreaImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Interfaces.Charts;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartTextAreaImpl : 
  CommonObject,
  IChartDataLabels,
  ISerializable,
  IInternalChartTextArea,
  IChartTextArea,
  IInternalFont,
  IFont,
  IParentApplication,
  IOptimizedUpdate
{
  private bool m_isTitleElement;
  private bool m_isValueFromCells;
  private IRange m_valueFromCellsRange;
  private FontWrapper m_font;
  internal ChartTextRecord m_chartText = (ChartTextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartText);
  private WorkbookImpl m_book;
  private ChartFrameFormatImpl m_frame;
  private string m_strText;
  private ChartObjectLinkRecord m_link = (ChartObjectLinkRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartObjectLink);
  private ChartDataLabelsRecord m_dataLabels = (ChartDataLabelsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartDataLabels);
  private ChartAIRecord m_chartAi;
  private ChartAlrunsRecord m_chartAlRuns;
  private ChartPosRecord m_pos;
  private ChartAttachedLabelLayoutRecord m_attachedLabelLayout;
  private bool m_bIsTrend;
  private IChartLayout m_layout;
  private ChartParagraphType m_paraType;
  private bool m_bShowTextProperties = true;
  private bool m_bShowSizeProperties;
  private bool m_bShowBoldProperties;
  private bool m_bIsFormula;
  protected IChartRichTextString m_rtfString;
  private Excel2007TextRotation m_TextRotation;
  internal UnknownRecord m_titleRichTextStream;
  internal bool isEmptyDefPara;
  internal bool isEmptyTextPara;
  internal bool isFontChanged;
  private bool m_bIsTextParsed;
  private string[] m_stringCache;
  private bool m_bOverlay;
  internal IList<IInternalChartTextArea> DefaultParagarphProperties = (IList<IInternalChartTextArea>) new List<IInternalChartTextArea>();
  internal bool? IsNormalizeHeights;
  internal TextCapsType CapitalizationType;
  internal bool IsCapsUsed;

  [CLSCompliant(false)]
  public static BiffRecordRaw UnwrapRecord(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    return record.TypeCode == TBIFFRecord.ChartWrapper ? ((ChartWrapperRecord) record).Record : record;
  }

  public ChartTextAreaImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    ChartImpl parent1 = (ChartImpl) this.FindParent(typeof (ChartImpl));
    this.SetFontIndex(parent1.DefaultTextIndex);
    this.m_chartText.IsAutoMode = true;
    this.m_chartText.IsGenerated = false;
    this.m_chartText.IsAutoText = false;
    this.m_chartText.IsAutoColor = true;
    this.m_chartText.HorzAlign = ExcelChartHorzAlignment.Center;
    this.m_chartText.VertAlign = ExcelChartVertAlignment.Center;
    this.m_link.LinkObject = ExcelObjectTextLink.DataLabel;
    this.m_chartAi = (ChartAIRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAI);
    this.m_chartAi.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    this.m_chartAlRuns = (ChartAlrunsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAlruns);
    this.m_pos = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
    this.m_pos.TopLeft = (ushort) 2;
    this.m_pos.BottomRight = (ushort) 2;
    this.m_paraType = ChartParagraphType.Default;
    if (this.m_book.Loading || (ChartLegendImpl) this.FindParent(typeof (ChartLegendImpl)) == null || !ChartImpl.IsChartExSerieType(parent1.ChartType))
      return;
    this.Font.Size = 9.0;
  }

  [CLSCompliant(false)]
  public ChartTextAreaImpl(IApplication application, object parent, ExcelObjectTextLink textLink)
    : this(application, parent)
  {
    this.m_link.LinkObject = textLink;
  }

  public ChartTextAreaImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : this(application, parent)
  {
    iPos = this.Parse(data, iPos);
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  public bool Bold
  {
    get => this.m_font.Bold;
    set
    {
      this.m_font.Bold = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(0, (object) value);
    }
  }

  public ExcelKnownColors Color
  {
    get => this.m_font.Color;
    set
    {
      if (this.m_chartText.ColorIndex != value)
      {
        this.m_chartText.ColorIndex = value;
        this.m_font.Color = value;
        this.m_chartText.IsAutoColor = false;
      }
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(1, (object) value);
    }
  }

  public System.Drawing.Color RGBColor
  {
    get => this.m_font.RGBColor;
    set
    {
      if (this.m_font.RGBColor != value)
      {
        this.m_chartText.IsAutoColor = false;
        this.m_font.RGBColor = value;
      }
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(2, (object) value);
    }
  }

  public bool Italic
  {
    get => this.m_font.Italic;
    set
    {
      this.m_font.Italic = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(3, (object) value);
    }
  }

  public bool MacOSOutlineFont
  {
    get => this.m_font.MacOSOutlineFont;
    set
    {
      this.m_font.MacOSOutlineFont = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(4, (object) value);
    }
  }

  public bool MacOSShadow
  {
    get => this.m_font.MacOSShadow;
    set
    {
      this.m_font.MacOSShadow = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(5, (object) value);
    }
  }

  public double Size
  {
    get => this.m_font.Size;
    set
    {
      this.m_font.Size = value;
      if (!this.ParentWorkbook.Loading)
        this.ShowSizeProperties = true;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(6, (object) value);
    }
  }

  public bool Strikethrough
  {
    get => this.m_font.Strikethrough;
    set
    {
      this.m_font.Strikethrough = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(7, (object) value);
    }
  }

  internal int Baseline
  {
    get => this.m_font.Baseline;
    set => this.m_font.Baseline = value;
  }

  public bool Subscript
  {
    get => this.m_font.Subscript;
    set
    {
      this.m_font.Subscript = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(8, (object) value);
    }
  }

  public bool Superscript
  {
    get => this.m_font.Superscript;
    set
    {
      this.m_font.Superscript = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(9, (object) value);
    }
  }

  public ExcelUnderline Underline
  {
    get => this.m_font.Underline;
    set
    {
      this.m_font.Underline = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(10, (object) value);
    }
  }

  public string FontName
  {
    get => this.m_font.FontName;
    set
    {
      if (this.Parent is ChartDataTableImpl)
      {
        ChartDataTableImpl parent = this.Parent as ChartDataTableImpl;
        if (parent.isdefault)
          parent.hasDefaultFontName = true;
        else if (parent.hasDefaultFontName)
          parent.hasDefaultFontName = false;
      }
      if (!this.ParentWorkbook.Loading)
        this.isFontChanged = true;
      this.m_font.FontName = value;
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(11, (object) value);
    }
  }

  public ExcelFontVertialAlignment VerticalAlignment
  {
    get => this.m_font.VerticalAlignment;
    set
    {
      this.m_font.VerticalAlignment = value;
      if (this.m_font.VerticalAlignment == ExcelFontVertialAlignment.Superscript)
      {
        this.Superscript = true;
        this.Baseline = 30;
      }
      else if (this.m_font.VerticalAlignment == ExcelFontVertialAlignment.Subscript)
      {
        this.Subscript = true;
        this.Baseline = -25;
      }
      if (this.ParentWorkbook.Loading || !(this.Parent is ChartLegendImpl))
        return;
      (this.Parent as ChartLegendImpl).LegendStyleProperty(12, (object) value);
    }
  }

  public System.Drawing.Font GenerateNativeFont() => this.m_font.GenerateNativeFont();

  internal bool IsTitleElement
  {
    get => this.m_isTitleElement;
    set => this.m_isTitleElement = value;
  }

  public string Text
  {
    get
    {
      this.UpdateChartText();
      return this.m_strText;
    }
    set
    {
      this.m_strText = value;
      if (this.m_bIsTrend || this.m_link.LinkObject == ExcelObjectTextLink.DisplayUnit)
        this.m_chartText.IsAutoText = false;
      this.m_chartText.IsDeleted = value == null;
      if (this.m_chartAlRuns != null && this.m_chartAlRuns.Runs != null && this.m_chartAlRuns.Runs.Length > 0)
        this.m_chartAlRuns = (ChartAlrunsRecord) null;
      if (this.m_chartAi == null || !this.IsFormula)
        return;
      this.m_chartAi.ParsedExpression = this.GetNameTokens();
      this.m_chartAi.Reference = ChartAIRecord.ReferenceType.Worksheet;
    }
  }

  public IChartRichTextString RichText
  {
    get
    {
      this.CheckDisposed();
      if (this.m_rtfString == null)
        this.CreateRichTextString();
      return this.m_rtfString;
    }
  }

  public IChartFrameFormat FrameFormat
  {
    get
    {
      if (this.m_frame == null)
        this.InitFrameFormat();
      return (IChartFrameFormat) this.m_frame;
    }
  }

  [CLSCompliant(false)]
  public ChartObjectLinkRecord ObjectLink => this.m_link;

  public int TextRotationAngle
  {
    get
    {
      return this.m_chartText.TextRotation > (short) 90 ? (int) this.m_chartText.TextRotation - 90 : (int) -this.m_chartText.TextRotation;
    }
    set
    {
      if (value > 0)
        this.m_chartText.TextRotation = (short) (90 + (int) (short) value);
      else
        this.m_chartText.TextRotation = (short) -value;
    }
  }

  public bool HasTextRotation
  {
    get
    {
      short? textRotationOrNull = this.m_chartText.TextRotationOrNull;
      return (textRotationOrNull.HasValue ? new int?((int) textRotationOrNull.GetValueOrDefault()) : new int?()).HasValue;
    }
  }

  internal Excel2007TextRotation TextRotation
  {
    get => this.m_TextRotation;
    set => this.m_TextRotation = value;
  }

  [CLSCompliant(false)]
  public ChartTextRecord TextRecord => this.m_chartText;

  public string NumberFormat
  {
    get => this.m_book.InnerFormats[this.NumberFormatIndex].FormatString;
    set
    {
      this.ChartAI.NumberFormatIndex = (ushort) this.m_book.InnerFormats.FindOrCreateFormat(value);
    }
  }

  public int NumberFormatIndex
  {
    get => this.m_chartAi == null ? 0 : (int) this.m_chartAi.NumberFormatIndex;
  }

  [CLSCompliant(false)]
  public ChartAIRecord ChartAI
  {
    get
    {
      if (this.m_chartAi == null)
        this.m_chartAi = (ChartAIRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAI);
      return this.m_chartAi;
    }
  }

  [CLSCompliant(false)]
  public ChartAlrunsRecord ChartAlRuns
  {
    get
    {
      if (this.m_chartAlRuns == null)
        this.m_chartAlRuns = (ChartAlrunsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAlruns);
      return this.m_chartAlRuns;
    }
  }

  public bool ContainDataLabels => !(this.m_dataLabels == (ChartDataLabelsRecord) null);

  public ExcelChartBackgroundMode BackgroundMode
  {
    get => this.m_chartText.BackgroundMode;
    set
    {
      this.m_chartText.BackgroundMode = value;
      this.IsAutoMode = false;
    }
  }

  public bool IsAutoMode
  {
    get => this.m_chartText.IsAutoMode;
    set => this.m_chartText.IsAutoMode = value;
  }

  public bool IsTrend
  {
    get => this.m_bIsTrend;
    set
    {
      this.m_bIsTrend = value;
      if (this.m_strText != null && this.m_strText.Length != 0)
        return;
      this.m_chartText.IsAutoText = true;
    }
  }

  public bool IsAutoColor => this.m_chartText.IsAutoColor;

  public IChartLayout Layout
  {
    get
    {
      if (this.m_layout == null)
        this.m_layout = (IChartLayout) new ChartLayoutImpl(this.Application, (object) this, this.Parent);
      return this.m_layout;
    }
    set => this.m_layout = value;
  }

  public WorkbookImpl ParentWorkbook => this.m_book;

  public ChartParagraphType ParagraphType
  {
    get
    {
      if (this.m_paraType != ChartParagraphType.CustomDefault)
        ChartParserCommon.CheckDefaultSettings(this);
      return this.m_paraType;
    }
    set => this.m_paraType = value;
  }

  internal bool ShowTextProperties
  {
    get => this.m_bShowTextProperties;
    set => this.m_bShowTextProperties = value;
  }

  internal bool ShowSizeProperties
  {
    get => this.m_bShowSizeProperties;
    set => this.m_bShowSizeProperties = value;
  }

  internal bool ShowBoldProperties
  {
    get => this.m_bShowBoldProperties;
    set => this.m_bShowBoldProperties = value;
  }

  internal string[] StringCache
  {
    get => this.m_stringCache;
    set => this.m_stringCache = value;
  }

  internal bool Overlay
  {
    get => this.m_bOverlay;
    set => this.m_bOverlay = value;
  }

  [CLSCompliant(false)]
  public int Parse(IList<BiffRecordRaw> data, int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos >= data.Count)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Length");
    this.m_chartText = (ChartTextRecord) ChartTextAreaImpl.UnwrapRecord(data[iPos]);
    ++iPos;
    BiffRecordRaw record = ChartTextAreaImpl.UnwrapRecord(data[iPos]);
    ++iPos;
    record.CheckTypeCode(TBIFFRecord.Begin);
    this.m_dataLabels = (ChartDataLabelsRecord) null;
    while (record.TypeCode != TBIFFRecord.End)
    {
      record = ChartTextAreaImpl.UnwrapRecord(data[iPos]);
      ++iPos;
      iPos = this.ParseRecord(record, data, iPos);
    }
    return iPos;
  }

  private void ParseFontx(ChartFontxRecord fontx)
  {
    if (fontx == null)
      throw new ArgumentNullException(nameof (fontx));
    this.SetFontIndex((int) fontx.FontIndex);
  }

  [CLSCompliant(false)]
  protected int ParseRecord(BiffRecordRaw record, IList<BiffRecordRaw> data, int iPos)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    switch (record.TypeCode)
    {
      case TBIFFRecord.ChartDataLabels:
        this.m_dataLabels = (ChartDataLabelsRecord) record;
        break;
      case TBIFFRecord.ChartAttachedLabelLayout:
        if (this.m_attachedLabelLayout == null)
          this.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) record;
        ChartManualLayoutImpl manualLayout = this.Layout.ManualLayout as ChartManualLayoutImpl;
        manualLayout.AttachedLabelLayout = this.m_attachedLabelLayout;
        if (this.m_attachedLabelLayout != null && this.m_attachedLabelLayout.X > 0.0 && this.m_attachedLabelLayout.Y > 0.0 && this.m_attachedLabelLayout.WXMode != LayoutModes.auto && this.m_attachedLabelLayout.WYMode != LayoutModes.auto)
        {
          manualLayout.FlagOptions = (byte) 3;
          break;
        }
        break;
      case (TBIFFRecord) 2214:
        this.m_titleRichTextStream = (UnknownRecord) record;
        int length = 0;
        this.ExtractRichTextStreamRecord(ref record, out length);
        this.m_titleRichTextStream.Length = length;
        this.m_titleRichTextStream.InfillInternalData((DataProvider) new ByteArrayDataProvider(record.Data), 0, ExcelVersion.Excel97to2003);
        break;
      case TBIFFRecord.ChartSeriesText:
        ChartSeriesTextRecord seriesTextRecord = (ChartSeriesTextRecord) record;
        this.m_strText = !this.IsFormula || this.m_chartAi == null || this.m_chartAi.ParsedExpression == null ? seriesTextRecord.Text : this.m_book.FormulaUtil.ParsePtgArray(this.m_chartAi.ParsedExpression);
        break;
      case TBIFFRecord.ChartFontx:
        this.ParseFontx((ChartFontxRecord) record);
        this.ParagraphType = ChartParagraphType.CustomDefault;
        break;
      case TBIFFRecord.ChartObjectLink:
        this.m_link = (ChartObjectLinkRecord) record;
        break;
      case TBIFFRecord.ChartFrame:
        --iPos;
        this.InitFrameFormat();
        this.m_frame.Parse(data, ref iPos);
        break;
      case TBIFFRecord.ChartPos:
        this.m_pos = (ChartPosRecord) record;
        break;
      case TBIFFRecord.ChartAlruns:
        this.m_chartAlRuns = (ChartAlrunsRecord) record;
        break;
      case TBIFFRecord.ChartAI:
        this.m_chartAi = (ChartAIRecord) record;
        if (this.m_chartAi != null && this.m_chartAi.FormulaSize > (ushort) 0)
        {
          this.IsFormula = true;
          break;
        }
        break;
    }
    this.m_font.ColorObject.SetIndexed(this.m_chartText.ColorIndex);
    return iPos;
  }

  private void ExtractRichTextStreamRecord(ref BiffRecordRaw record, out int length)
  {
    ChartTextAreaImpl textArea = this;
    length = BitConverter.ToInt32(record.Data, 16 /*0x10*/);
    XmlReader reader = UtilityMethods.CreateReader((Stream) new MemoryStream(record.Data, 20, length));
    int fontIndex = this.FontIndex;
    this.ParseRichTextXMLStream(reader, textArea);
    MemoryStream memoryStream = ChartSerializatorCommon.SerializeRichTextStream(textArea, (IWorkbook) textArea.m_book, textArea.m_book.StandardFontSize);
    length = (int) memoryStream.Length;
    byte[] destinationArray = new byte[16 /*0x10*/];
    Array.Copy((Array) record.Data, 0, (Array) destinationArray, 0, 16 /*0x10*/);
    byte[] bytes = BitConverter.GetBytes((uint) length);
    byte[] buffer = new byte[length];
    record.Data = new byte[length + 20];
    memoryStream.Position = 0L;
    memoryStream.Read(buffer, 0, length);
    destinationArray.CopyTo((Array) record.Data, 0);
    bytes.CopyTo((Array) record.Data, 16 /*0x10*/);
    buffer.CopyTo((Array) record.Data, 20);
    length += 20;
    this.SetFontIndex(fontIndex);
    this.ShowSizeProperties = true;
    reader.Close();
    memoryStream.Close();
  }

  private void ParseRichTextXMLStream(XmlReader reader, ChartTextAreaImpl textArea)
  {
    List<ChartAlrunsRecord.TRuns> tRuns = (List<ChartAlrunsRecord.TRuns>) null;
    if (textArea.ChartAlRuns != null)
      tRuns = new List<ChartAlrunsRecord.TRuns>();
    ChartAlrunsRecord.TRuns[] trunsArray = (ChartAlrunsRecord.TRuns[]) textArea.ChartAlRuns.Runs.Clone();
    float? defaultFontSize = new float?();
    string text = textArea.Text;
    textArea.Text = string.Empty;
    ChartParserCommon.SetWorkbook(textArea.ParentWorkbook);
    Excel2007Parser parser = new Excel2007Parser(textArea.ParentWorkbook);
    ChartParserCommon.ParseRichText(reader, (IInternalChartTextArea) textArea, parser, defaultFontSize, tRuns, true);
    parser.Dispose();
    textArea.Text = text;
    textArea.ChartAlRuns.Runs = trunsArray;
    textArea.DefaultParagarphProperties = (IList<IInternalChartTextArea>) null;
  }

  protected virtual bool ShouldSerialize
  {
    get
    {
      return this.HasText || this.m_bIsTrend || this.m_link.LinkObject == ExcelObjectTextLink.DataLabel || this.m_link.LinkObject == ExcelObjectTextLink.DisplayUnit || this.m_chartText.IsDeleted || !this.m_chartText.IsAutoColor || !this.m_chartText.IsAutoMode || !this.m_chartText.IsAutoText;
    }
  }

  public bool HasText => this.m_strText != null;

  public bool IsFormula
  {
    get => this.m_bIsFormula;
    set => this.m_bIsFormula = value;
  }

  internal bool IsTextParsed
  {
    get => this.m_bIsTextParsed;
    set => this.m_bIsTextParsed = value;
  }

  [CLSCompliant(false)]
  public virtual void Serialize(IList<IBiffStorage> records) => this.Serialize(records, false);

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records, bool bIsLegendEntry)
  {
    this.Serialize(records, bIsLegendEntry, true);
  }

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records, bool bIsLegendEntry, bool bSerializeFontX)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (!this.ShouldSerialize)
      return;
    if (this.m_bIsTrend)
      this.UpdateAsTrend();
    this.m_chartText.ColorIndex = this.m_font.ColorObject.GetIndexed((IWorkbook) this.m_book);
    this.SerializeRecord(records, (BiffRecordRaw) this.m_chartText);
    this.SerializeRecord(records, BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    bool flag = this.m_link.LinkObject == ExcelObjectTextLink.DataLabel;
    if (!flag)
      this.m_chartText.DataLabelPlacement = ExcelDataLabelPosition.Automatic;
    if (this.m_pos != null)
      this.SerializeRecord(records, (BiffRecordRaw) this.m_pos);
    if (bSerializeFontX)
      this.SerializeFontx(records);
    if (this.m_chartAlRuns != null && this.m_chartAlRuns.Runs.Length >= 3 && this.m_chartAlRuns.Runs.Length <= 256 /*0x0100*/)
      this.SerializeRecord(records, (BiffRecordRaw) this.m_chartAlRuns);
    this.SerializeRecord(records, (BiffRecordRaw) this.m_chartAi);
    if (bIsLegendEntry)
    {
      this.SerializeRecord(records, BiffRecordFactory.GetRecord(TBIFFRecord.End));
    }
    else
    {
      if (this.m_strText != null)
      {
        ChartSeriesTextRecord record = (ChartSeriesTextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSeriesText);
        record.Text = this.m_strText;
        this.SerializeRecord(records, (BiffRecordRaw) record);
      }
      if (this.m_frame != null)
        this.m_frame.Serialize(records);
      this.SerializeRecord(records, (BiffRecordRaw) this.m_link);
      if (this.m_attachedLabelLayout != null)
        this.SerializeRecord(records, (BiffRecordRaw) this.m_attachedLabelLayout);
      if (this.m_titleRichTextStream != null)
        records.Add((IBiffStorage) this.m_titleRichTextStream.Clone());
      if (!this.m_bIsTrend && flag && this.m_dataLabels != (ChartDataLabelsRecord) null)
        this.SerializeRecord(records, (BiffRecordRaw) this.m_dataLabels);
      this.SerializeRecord(records, BiffRecordFactory.GetRecord(TBIFFRecord.End));
    }
  }

  private void SerializeFontx(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    List<int> intList = (List<int>) null;
    if (this.ParentWorkbook.ArrayFontIndex != null)
      intList = this.ParentWorkbook.ArrayFontIndex;
    FontWrapper font = this.m_font;
    int num = intList == null ? font.Wrapped.Index : intList[font.Wrapped.Index];
    if (num <= 0)
      return;
    ChartFontxRecord record = (ChartFontxRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartFontx);
    record.FontIndex = (ushort) num;
    this.SerializeRecord(records, (BiffRecordRaw) record);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeRecord(IList<IBiffStorage> records, BiffRecordRaw record)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (record == null)
      return;
    records.Add((IBiffStorage) record.Clone());
  }

  internal void UpdateChartText()
  {
    if (this.m_chartAi == null || !this.IsFormula || this.m_chartAi.ParsedExpression == null || this.m_chartAi.ParsedExpression.Length <= 0 || !(this.m_chartAi.ParsedExpression[0] is IRangeGetter rangeGetter))
      return;
    IRange range = rangeGetter.GetRange((IWorkbook) this.ParentWorkbook, (IWorksheet) null);
    if (range == null || range is ExternalRange)
      return;
    string address = range.Address;
    if (address.Equals(this.m_strText))
      return;
    this.m_strText = address;
  }

  protected virtual ChartFrameFormatImpl CreateFrameFormat()
  {
    return new ChartFrameFormatImpl(this.Application, (object) this);
  }

  protected void InitFrameFormat()
  {
    this.m_frame = this.CreateFrameFormat();
    this.m_frame.FrameRecord.AutoSize = true;
    this.m_frame.Border.LinePattern = ExcelChartLinePattern.None;
    this.m_frame.Border.AutoFormat = false;
    this.m_frame.Interior.UseAutomaticFormat = false;
    this.m_frame.Interior.Pattern = ExcelPattern.None;
  }

  internal void SetFontIndex(int index)
  {
    if ((index < 0 || index >= this.m_book.InnerFonts.Count) && this.m_font != null && this.m_font.FontIndex == index)
    {
      this.m_font.Wrapped = this.m_book.InnerFonts.Add((IFont) this.m_font.Font) as FontImpl;
    }
    else
    {
      this.DetachEvents();
      FontImpl innerFont = (FontImpl) this.m_book.InnerFonts[index];
      if (this.m_font == null)
        this.m_font = new FontWrapper();
      this.m_font.Wrapped = innerFont;
      this.AttachEvents();
    }
  }

  private void CreateDataLabels()
  {
    if (!(this.m_dataLabels == (ChartDataLabelsRecord) null))
      return;
    this.m_dataLabels = new ChartDataLabelsRecord();
  }

  public object Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartTextAreaImpl parent1 = (ChartTextAreaImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_bIsDisposed = this.m_bIsDisposed;
    parent1.m_chartText = (ChartTextRecord) CloneUtils.CloneCloneable((ICloneable) this.m_chartText);
    if (this.m_font != null)
      parent1.m_font = this.m_font.Clone(parent1.m_book, (object) parent1, (IDictionary) dicFontIndexes);
    if (this.m_frame != null)
      parent1.m_frame = this.m_frame.Clone((object) parent1);
    parent1.m_link = (ChartObjectLinkRecord) CloneUtils.CloneCloneable((ICloneable) this.m_link);
    if (this.m_chartAi != null)
    {
      parent1.m_chartAi = (ChartAIRecord) this.m_chartAi.Clone();
      parent1.NumberFormat = this.NumberFormat;
      Ptg[] parsedExpression = parent1.m_chartAi.ParsedExpression;
      int length = parsedExpression != null ? parsedExpression.Length : 0;
      for (int index = 0; index < length; ++index)
      {
        if (parsedExpression[index] is ISheetReference sheetReference)
        {
          int refIndex = (int) sheetReference.RefIndex;
          int num = refIndex;
          if (!this.m_book.IsExternalReference(refIndex))
          {
            string str = this.m_book.GetSheetNameByReference(refIndex);
            if (dicNewSheetNames != null && dicNewSheetNames.ContainsKey(str))
              str = dicNewSheetNames[str];
            num = parent1.m_book.AddSheetReference(str);
          }
          sheetReference.RefIndex = (ushort) num;
        }
      }
    }
    parent1.m_strText = this.m_strText;
    parent1.m_bOverlay = this.m_bOverlay;
    if (this.m_layout != null)
      parent1.m_layout = this.m_layout;
    if (this.m_chartAlRuns != null)
      parent1.m_chartAlRuns = (ChartAlrunsRecord) this.m_chartAlRuns.Clone();
    parent1.m_strText = this.m_strText;
    parent1.m_TextRotation = this.m_TextRotation;
    if (this.m_pos != null)
      parent1.m_pos = (ChartPosRecord) CloneUtils.CloneCloneable((ICloneable) this.m_pos);
    parent1.BackgroundMode = this.m_chartText.BackgroundMode;
    this.UpdateChartText();
    return (object) parent1;
  }

  public object Clone(object parent)
  {
    return this.Clone(parent, (Dictionary<int, int>) null, (Dictionary<string, string>) null);
  }

  public void UpdateSerieIndex(int iNewIndex) => this.m_link.SeriesNumber = (ushort) iNewIndex;

  public void UpdateAsTrend()
  {
    this.ObjectLink.DataPointNumber = ushort.MaxValue;
    this.m_chartText.IsShowLabel = true;
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    if (this.m_chartAi == null)
      return;
    FormulaUtil.MarkUsedReferences(this.m_chartAi.ParsedExpression, usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    if (this.m_chartAi == null)
      return;
    Ptg[] parsedExpression = this.m_chartAi.ParsedExpression;
    if (!FormulaUtil.UpdateReferenceIndexes(parsedExpression, arrUpdatedIndexes))
      return;
    this.m_chartAi.ParsedExpression = parsedExpression;
  }

  private void AttachEvents()
  {
    if (this.m_font == null)
      return;
    this.m_font.ColorObject.AfterChange += new ColorObject.AfterChangeHandler(this.ColorChangeEventHandler);
  }

  internal void DetachEvents()
  {
    if (this.m_font == null)
      return;
    this.m_font.ColorObject.AfterChange -= new ColorObject.AfterChangeHandler(this.ColorChangeEventHandler);
  }

  private void ColorChangeEventHandler() => this.m_chartText.IsAutoColor = false;

  public int FontIndex => this.m_font == null ? 0 : this.m_font.Index;

  private Ptg[] GetNameTokens()
  {
    Ptg[] nameTokens = (Ptg[]) null;
    string strFormula = this.m_strText;
    if (strFormula != null)
    {
      if (strFormula[0] == '=')
        strFormula = UtilityMethods.RemoveFirstCharUnsafe(strFormula);
      nameTokens = this.m_book.FormulaUtil.ParseString(strFormula);
    }
    return nameTokens;
  }

  public bool IsValueFromCells
  {
    get => this.m_isValueFromCells;
    set => this.m_isValueFromCells = value;
  }

  public IRange ValueFromCellsRange
  {
    get => this.m_valueFromCellsRange;
    set => this.m_valueFromCellsRange = value;
  }

  public bool IsSeriesName
  {
    get => this.m_dataLabels != (ChartDataLabelsRecord) null && this.m_dataLabels.IsSeriesName;
    set => this.m_dataLabels.IsSeriesName = value;
  }

  public bool IsCategoryName
  {
    get => this.m_dataLabels != (ChartDataLabelsRecord) null && this.m_dataLabels.IsCategoryName;
    set => this.m_dataLabels.IsCategoryName = value;
  }

  public bool IsValue
  {
    get => this.m_dataLabels != (ChartDataLabelsRecord) null && this.m_dataLabels.IsValue;
    set
    {
      if (this.m_dataLabels == (ChartDataLabelsRecord) null)
        this.CreateDataLabels();
      this.m_dataLabels.IsValue = value;
      this.TextRecord.IsShowValue = value;
    }
  }

  public bool IsPercentage
  {
    get => this.m_dataLabels != (ChartDataLabelsRecord) null && this.m_dataLabels.IsPercentage;
    set
    {
      this.m_dataLabels.IsPercentage = value;
      this.TextRecord.IsShowPercent = value;
    }
  }

  public bool IsBubbleSize
  {
    get => this.m_dataLabels != (ChartDataLabelsRecord) null && this.m_dataLabels.IsBubbleSize;
    set => this.m_dataLabels.IsBubbleSize = value;
  }

  public bool ShowLeaderLines
  {
    get => throw new NotSupportedException("Liner lines are not supported.");
    set
    {
    }
  }

  public string Delimiter
  {
    get
    {
      return !(this.m_dataLabels != (ChartDataLabelsRecord) null) ? (string) null : this.m_dataLabels.Delimiter;
    }
    set => this.m_dataLabels.Delimiter = value;
  }

  public bool IsLegendKey
  {
    get => this.TextRecord.IsShowKey;
    set => this.TextRecord.IsShowKey = value;
  }

  public ExcelDataLabelPosition Position
  {
    get => this.m_chartText.DataLabelPlacement;
    set
    {
      this.m_chartText.DataLabelPlacement = value != ExcelDataLabelPosition.Moved ? value : throw new NotSupportedException("This flag doesnot support.");
    }
  }

  public ChartAttachedLabelLayoutRecord AttachedLabelLayout
  {
    get
    {
      if (this.m_attachedLabelLayout == null)
        this.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAttachedLabelLayout);
      return this.m_attachedLabelLayout;
    }
  }

  public bool IsShowLabelPercent
  {
    get => this.TextRecord.IsShowLabelPercent;
    set => this.TextRecord.IsShowLabelPercent = value;
  }

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }

  public ColorObject ColorObject => this.m_font.ColorObject;

  public int Index => this.m_font.Index;

  public FontImpl Font => this.m_font.Font;

  protected void CreateRichTextString()
  {
    this.m_rtfString = (IChartRichTextString) new ChartRichTextString(this.Application, (object) this);
  }
}
