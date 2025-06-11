// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartAxisImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public abstract class ChartAxisImpl : CommonObject, IChartAxis
{
  protected const int DEF_NUMBER_FORMAT_INDEX = -1;
  private const int DEF_GENERAL_FORMAT = 0;
  private bool m_isWrapText;
  private ExcelAxisType m_axisType;
  private bool m_bPrimary;
  private ChartTextAreaImpl m_titleArea;
  private ChartTickRecord m_chartTick;
  private ShadowImpl m_shadow;
  private bool m_bLineFormat;
  internal UnknownRecord m_startBlock;
  internal IList<UnknownRecord> m_shapePropsStreams;
  internal UnknownRecord m_endBlock;
  private FontWrapper m_font;
  private FontWrapper old_Font;
  private ChartGridLineImpl m_majorGrid;
  private ChartGridLineImpl m_minorGrid;
  private bool m_bHasMajor;
  private bool m_bHasMinor;
  private ChartParentAxisImpl m_parentAxis;
  private int m_iNumberFormat = -1;
  private ChartBorderImpl m_border;
  private ExcelAxisTextDirection m_textDirection;
  internal Excel2007TextRotation m_textRotation;
  private int m_iAxisId;
  private bool m_bDeleted;
  private bool m_bAutoTickLabelSpacing = true;
  private bool m_bAutoTickMarkSpacing;
  internal AxisLabelAlignment LabelAlign = AxisLabelAlignment.Center;
  private ThreeDFormatImpl m_3D;
  private ChartAxisPos? m_axisPos;
  private bool m_sourceLinked = true;
  private Stream m_textStream;
  private ChartParagraphType m_paraType;
  private ChartFrameFormatImpl m_axisFormat;
  private bool m_IsDefaultTextSettings;
  private bool m_isChartFont;

  public ChartAxisImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.InitializeVariables();
  }

  public ChartAxisImpl(IApplication application, object parent, ExcelAxisType axisType)
    : this(application, parent, axisType, true)
  {
  }

  public ChartAxisImpl(
    IApplication application,
    object parent,
    ExcelAxisType axisType,
    bool bIsPrimary)
    : base(application, parent)
  {
    this.m_axisType = axisType;
    this.m_bPrimary = bIsPrimary;
    this.InitializeVariables();
  }

  [CLSCompliant(false)]
  public ChartAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : this(application, parent, data, ref iPos, true)
  {
  }

  [CLSCompliant(false)]
  public ChartAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos,
    bool isPrimary)
    : this(application, parent)
  {
    this.Parse(data, ref iPos, isPrimary);
  }

  private void SetParents()
  {
    this.m_parentAxis = (ChartParentAxisImpl) this.FindParent(typeof (ChartParentAxisImpl));
    if (this.m_parentAxis == null)
      throw new ArgumentNullException("There is no parent axis.");
  }

  internal bool IsWrapText
  {
    get => this.m_isWrapText;
    set => this.m_isWrapText = value;
  }

  public ExcelAxisType AxisType
  {
    get => this.m_axisType;
    set => this.m_axisType = value;
  }

  public bool IsPrimary
  {
    get => this.m_bPrimary;
    set => this.m_bPrimary = value;
  }

  public string Title
  {
    get => this.m_titleArea == null ? (string) null : this.m_titleArea.Text;
    set => this.TitleArea.Text = value;
  }

  public int TextRotationAngle
  {
    get
    {
      return this.ParentWorkbook.Version == ExcelVersion.Excel97to2003 ? (int) this.m_chartTick.RotationAngle : (int) -this.m_chartTick.RotationAngle;
    }
    set
    {
      if (value == 0)
        this.IsWrapText = true;
      if (value < -90 || value > 90)
      {
        this.m_chartTick.RotationAngle = (short) 0;
      }
      else
      {
        this.m_chartTick.RotationAngle = this.ParentWorkbook.Version != ExcelVersion.Excel97to2003 ? (short) -value : (short) value;
        this.m_chartTick.IsAutoRotation = false;
        this.IsDefaultTextSettings = false;
      }
    }
  }

  public bool IsAutoTextRotation => this.m_chartTick.IsAutoRotation;

  public IChartTextArea TitleArea
  {
    get
    {
      if (this.m_titleArea == null)
      {
        this.m_titleArea = new ChartTextAreaImpl(this.Application, (object) this, this.TextLinkType);
        this.m_titleArea.Bold = true;
        this.m_titleArea.Size = 10.0;
      }
      return (IChartTextArea) this.m_titleArea;
    }
  }

  public IFont Font
  {
    get
    {
      if (this.m_font == null)
        this.m_font = new FontWrapper((FontImpl) this.ParentWorkbook.InnerFonts[0]);
      if (!this.IsChartFont)
      {
        this.IsDefaultTextSettings = false;
        this.m_paraType = ChartParagraphType.CustomDefault;
      }
      if (!this.ParentWorkbook.Loading && !this.ParentWorkbook.Saving)
      {
        this.m_font.AfterChangeEvent -= new EventHandler(this.ColorChangeEventHandler);
        this.m_font.AfterChangeEvent += new EventHandler(this.ColorChangeEventHandler);
      }
      return (IFont) this.m_font;
    }
    set
    {
      if (value == this.m_font)
        return;
      if (!this.IsChartFont)
        this.IsDefaultTextSettings = false;
      this.m_font = (FontWrapper) value;
      this.m_isChartFont = false;
    }
  }

  internal bool IsChartFont
  {
    get => this.m_isChartFont;
    set => this.m_isChartFont = value;
  }

  public IChartGridLine MajorGridLines => (IChartGridLine) this.m_majorGrid;

  public IChartGridLine MinorGridLines => (IChartGridLine) this.m_minorGrid;

  public bool HasMinorGridLines
  {
    get => this.m_bHasMinor;
    set
    {
      if (value == this.HasMinorGridLines)
        return;
      ChartImpl parentChart = this.m_parentAxis.ParentChart;
      if (!parentChart.TypeChanging && !parentChart.CheckForSupportGridLine())
        throw new ApplicationException("This chart type does not support gridlines.");
      this.m_bHasMinor = value;
      this.m_minorGrid = value ? new ChartGridLineImpl(this.Application, (object) this, ExcelAxisLineIdentifier.MinorGridLine) : (ChartGridLineImpl) null;
    }
  }

  public bool HasMajorGridLines
  {
    get => this.m_bHasMajor;
    set
    {
      if (value == this.HasMajorGridLines)
        return;
      ChartImpl parentChart = this.m_parentAxis.ParentChart;
      if (!parentChart.TypeChanging && !parentChart.CheckForSupportGridLine())
        throw new ApplicationException("This chart type does not support gridlines.");
      this.m_bHasMajor = value;
      this.m_majorGrid = value ? new ChartGridLineImpl(this.Application, (object) this, ExcelAxisLineIdentifier.MajorGridLine) : (ChartGridLineImpl) null;
    }
  }

  public bool isNumber => this.m_iNumberFormat != -1;

  protected ChartParentAxisImpl ParentAxis => this.m_parentAxis;

  public int NumberFormatIndex
  {
    get => this.m_iNumberFormat;
    set => this.m_iNumberFormat = value;
  }

  public string NumberFormat
  {
    get
    {
      int iIndex = this.m_iNumberFormat;
      FormatsCollection innerFormats = this.ParentWorkbook.InnerFormats;
      if (this.m_iNumberFormat == -1 || !innerFormats.Contains(this.m_iNumberFormat))
        iIndex = 0;
      return innerFormats[iIndex].FormatString;
    }
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException("formatString");
        case "":
          throw new ArgumentException("value - string cannot be empty");
        default:
          if (!this.ParentWorkbook.Loading && this.IsSourceLinked)
            this.IsSourceLinked = false;
          this.m_iNumberFormat = this.ParentWorkbook.InnerFormats.FindOrCreateFormat(value);
          break;
      }
    }
  }

  public ExcelTickMark MinorTickMark
  {
    get => this.m_chartTick.MinorMark;
    set => this.m_chartTick.MinorMark = value;
  }

  public ExcelTickMark MajorTickMark
  {
    get => this.m_chartTick.MajorMark;
    set => this.m_chartTick.MajorMark = value;
  }

  public IChartBorder Border
  {
    get
    {
      if (this.m_border == null)
        this.m_border = new ChartBorderImpl(this.Application, (object) this);
      return (IChartBorder) this.m_border;
    }
  }

  public ExcelTickLabelPosition TickLabelPosition
  {
    get => this.m_chartTick.LabelPos;
    set => this.m_chartTick.LabelPos = value;
  }

  public bool Visible
  {
    get => !this.Deleted;
    set
    {
      if (value == this.Visible)
        return;
      this.Border.LinePattern = value ? ExcelChartLinePattern.Solid : ExcelChartLinePattern.None;
      this.Deleted = !value;
    }
  }

  public ExcelAxisTextDirection Alignment
  {
    get => this.m_textDirection;
    set => this.m_textDirection = value;
  }

  [Obsolete("Please use ReversePlotOrder property instead of this one.")]
  public bool IsReversed
  {
    get => this.ReversePlotOrder;
    set => this.ReversePlotOrder = value;
  }

  public abstract bool ReversePlotOrder { get; set; }

  public int AxisId
  {
    get => this.m_iAxisId;
    internal set => this.m_iAxisId = value;
  }

  public ChartImpl ParentChart => this.m_parentAxis.m_parentChart;

  public bool Deleted
  {
    get
    {
      if (!this.Border.DrawTickLabels && this.Border.LinePattern == ExcelChartLinePattern.None)
        this.m_bDeleted = true;
      return this.m_bDeleted;
    }
    set
    {
      if (value != this.Deleted)
      {
        this.Border.DrawTickLabels = !value;
        if (value)
          this.Border.LinePattern = ExcelChartLinePattern.None;
      }
      this.m_bDeleted = value;
    }
  }

  public bool AutoTickLabelSpacing
  {
    get => this.m_bAutoTickLabelSpacing;
    set => this.m_bAutoTickLabelSpacing = value;
  }

  public bool AutoTickMarkSpacing
  {
    get => this.m_bAutoTickMarkSpacing;
    set => this.m_bAutoTickMarkSpacing = value;
  }

  public IShadow Shadow
  {
    get
    {
      if (this.m_shadow == null)
        this.m_shadow = new ShadowImpl(this.Application, (object) this);
      return (IShadow) this.m_shadow;
    }
  }

  public IShadow ShadowProperties => this.Shadow;

  public bool HasShadowProperties
  {
    get => this.m_shadow != null;
    internal set
    {
      if (value)
      {
        IShadow shadow = this.Shadow;
      }
      else
        this.m_shadow = (ShadowImpl) null;
    }
  }

  public IThreeDFormat Chart3DOptions
  {
    get
    {
      if (this.m_3D == null)
        this.m_3D = new ThreeDFormatImpl(this.Application, (object) this);
      return (IThreeDFormat) this.m_3D;
    }
  }

  public IThreeDFormat Chart3DProperties => this.Chart3DOptions;

  public bool Has3dProperties
  {
    get => this.m_3D != null;
    internal set
    {
      if (value)
      {
        IThreeDFormat chart3Doptions = this.Chart3DOptions;
      }
      else
        this.m_3D = (ThreeDFormatImpl) null;
    }
  }

  internal ChartAxisPos? AxisPosition
  {
    get => this.m_axisPos;
    set => this.m_axisPos = value;
  }

  internal bool IsSourceLinked
  {
    get => this.m_sourceLinked;
    set => this.m_sourceLinked = value;
  }

  internal Stream TextStream
  {
    get => this.m_textStream;
    set => this.m_textStream = value;
  }

  public IChartFrameFormat FrameFormat
  {
    get
    {
      if (this.m_axisFormat == null)
        this.InitFrameFormat();
      return (IChartFrameFormat) this.m_axisFormat;
    }
  }

  public bool HasAxisTitle => this.m_titleArea != null;

  public ChartParagraphType ParagraphType
  {
    get => this.m_paraType;
    set => this.m_paraType = value;
  }

  internal bool IsDefaultTextSettings
  {
    get => this.m_IsDefaultTextSettings;
    set => this.m_IsDefaultTextSettings = value;
  }

  protected abstract ExcelObjectTextLink TextLinkType { get; }

  protected WorkbookImpl ParentWorkbook => this.m_parentAxis.m_parentChart.InnerWorkbook;

  [CLSCompliant(false)]
  protected void Parse(IList<BiffRecordRaw> data, ref int iPos, bool isPrimary)
  {
    this.m_bPrimary = isPrimary;
    this.ParagraphType = ChartParagraphType.CustomDefault;
    BiffRecordRaw biffRecordRaw = data[iPos];
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartAxis);
    ChartAxisRecord chartAxisRecord = (ChartAxisRecord) biffRecordRaw;
    ++iPos;
    BiffRecordRaw record = data[iPos];
    record.CheckTypeCode(TBIFFRecord.Begin);
    categoryAxisImpl = (ChartCategoryAxisImpl) null;
    chartSeriesAxisImpl = (ChartSeriesAxisImpl) null;
    this.IsDefaultTextSettings = false;
    for (; record.TypeCode != TBIFFRecord.End; record = data[iPos])
    {
      switch (record.TypeCode)
      {
        case TBIFFRecord.StartBlock:
          this.m_startBlock = (UnknownRecord) record;
          ++iPos;
          break;
        case TBIFFRecord.EndBlock:
          this.m_endBlock = (UnknownRecord) record;
          ++iPos;
          break;
        case TBIFFRecord.ChartMlFrt:
          bool flag = false;
          if (chartAxisRecord.AxisType == ChartAxisRecord.ChartAxisType.CategoryAxis)
          {
            if (this is ChartCategoryAxisImpl categoryAxisImpl)
              flag = categoryAxisImpl.CheckForXmlTKOptions(record);
          }
          else if (chartAxisRecord.AxisType == ChartAxisRecord.ChartAxisType.SeriesAxis && this is ChartSeriesAxisImpl chartSeriesAxisImpl)
            flag = chartSeriesAxisImpl.CheckForXmlTKOptions(record);
          if (!flag)
            this.ParseData(record, data, ref iPos);
          ++iPos;
          break;
        case TBIFFRecord.ShapePropsStream:
          if (this.m_shapePropsStreams == null)
            this.m_shapePropsStreams = (IList<UnknownRecord>) new List<UnknownRecord>();
          this.m_shapePropsStreams.Add((UnknownRecord) record);
          ++iPos;
          break;
        case TBIFFRecord.ChartTick:
          this.ParseTickRecord((ChartTickRecord) data[iPos]);
          ++iPos;
          break;
        case TBIFFRecord.ChartAxisLineFormat:
          this.ParseChartAxisLineFormat(data, ref iPos);
          break;
        case TBIFFRecord.ChartFontx:
          this.ParseFontXRecord((ChartFontxRecord) data[iPos]);
          ++iPos;
          break;
        case TBIFFRecord.ChartIfmt:
          this.ParseIfmt(record as ChartIfmtRecord);
          ++iPos;
          break;
        default:
          this.ParseData(record, data, ref iPos);
          ++iPos;
          break;
      }
    }
    if (this.AutoTickLabelSpacing)
    {
      if ((categoryAxisImpl == null ? (chartAxisRecord.AxisType == ChartAxisRecord.ChartAxisType.CategoryAxis ? 1 : 0) : (!categoryAxisImpl.m_xmlTKLabelSkipFrt ? 1 : 0)) != 0)
      {
        if (this is ChartCategoryAxisImpl categoryAxisImpl1)
        {
          if (categoryAxisImpl1.m_xmlTKLabelSkipFrt)
            (this as ChartCategoryAxisImpl).AutoTickLabelSpacing = true;
          else
            (this as ChartCategoryAxisImpl).AutoTickLabelSpacing = false;
        }
      }
      else if ((chartSeriesAxisImpl == null ? (chartAxisRecord.AxisType == ChartAxisRecord.ChartAxisType.SeriesAxis ? 1 : 0) : (!chartSeriesAxisImpl.m_xmlTKLabelSkipFrt ? 1 : 0)) != 0 && this is ChartSeriesAxisImpl chartSeriesAxisImpl1)
      {
        if (chartSeriesAxisImpl1.m_xmlTKLabelSkipFrt)
          (this as ChartSeriesAxisImpl).AutoTickLabelSpacing = true;
        else
          (this as ChartSeriesAxisImpl).AutoTickLabelSpacing = false;
      }
    }
    ++iPos;
    switch (chartAxisRecord.AxisType)
    {
      case ChartAxisRecord.ChartAxisType.CategoryAxis:
        this.m_axisType = ExcelAxisType.Category;
        break;
      case ChartAxisRecord.ChartAxisType.ValueAxis:
        this.m_axisType = ExcelAxisType.Value;
        break;
      case ChartAxisRecord.ChartAxisType.SeriesAxis:
        this.m_axisType = ExcelAxisType.Serie;
        break;
    }
  }

  private void ParseChartAxisLineFormat(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    switch (((ChartAxisLineFormatRecord) data[iPos]).LineIdentifier)
    {
      case ExcelAxisLineIdentifier.AxisLineItself:
        ++iPos;
        if (data[iPos].TypeCode != TBIFFRecord.ChartLineFormat)
          break;
        this.m_border = new ChartBorderImpl(this.Application, (object) this, data, ref iPos);
        break;
      case ExcelAxisLineIdentifier.MajorGridLine:
        this.m_bHasMajor = true;
        this.m_majorGrid = new ChartGridLineImpl(this.Application, (object) this, data, ref iPos);
        break;
      case ExcelAxisLineIdentifier.MinorGridLine:
        this.m_bHasMinor = true;
        this.m_minorGrid = new ChartGridLineImpl(this.Application, (object) this, data, ref iPos);
        break;
      case ExcelAxisLineIdentifier.WallsOrFloor:
        this.ParseWallsOrFloor(data, ref iPos);
        break;
      default:
        throw new NotSupportedException("Unknown line identifier.");
    }
  }

  [CLSCompliant(false)]
  protected void ParseFontXRecord(ChartFontxRecord fontx)
  {
    if (fontx == null)
      throw new ArgumentNullException(nameof (fontx));
    this.m_font = new FontWrapper((FontImpl) this.ParentWorkbook.InnerFonts[(int) fontx.FontIndex]);
  }

  protected abstract void ParseWallsOrFloor(IList<BiffRecordRaw> data, ref int iPos);

  [CLSCompliant(false)]
  protected void ParseIfmt(ChartIfmtRecord record)
  {
    this.NumberFormatIndex = record != null ? (int) record.FormatIndex : throw new ArgumentNullException(nameof (record));
  }

  [CLSCompliant(false)]
  protected virtual void ParseData(BiffRecordRaw record, IList<BiffRecordRaw> data, ref int iPos)
  {
  }

  private void ParseTickRecord(ChartTickRecord chartTick)
  {
    this.m_chartTick = chartTick != null ? chartTick : throw new ArgumentNullException(nameof (chartTick));
    this.m_textDirection = ExcelAxisTextDirection.Context;
    if (chartTick.IsLeftToRight)
    {
      this.m_textDirection = ExcelAxisTextDirection.LeftToRight;
    }
    else
    {
      if (!chartTick.IsRightToLeft)
        return;
      this.m_textDirection = ExcelAxisTextDirection.RightToLeft;
    }
  }

  [CLSCompliant(false)]
  public virtual void Serialize(OffsetArrayList records)
  {
    throw new NotSupportedException("This method should not be called.");
  }

  [CLSCompliant(false)]
  public void SerializeAxisTitle(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_titleArea == null)
      return;
    this.m_titleArea.Serialize((IList<IBiffStorage>) records);
  }

  [CLSCompliant(false)]
  protected void SerializeFont(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    List<int> intList = (List<int>) null;
    if (this.ParentWorkbook.ArrayFontIndex != null)
      intList = this.ParentWorkbook.ArrayFontIndex;
    if (this.m_font == null)
      return;
    ChartFontxRecord record = (ChartFontxRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartFontx);
    record.FontIndex = intList == null ? (ushort) this.m_font.Index : (ushort) intList[this.m_font.Index];
    records.Add((IBiffStorage) record);
  }

  [CLSCompliant(false)]
  protected void SerializeGridLines(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_majorGrid != null)
      this.m_majorGrid.Serialize(records);
    if (this.m_minorGrid == null)
      return;
    this.m_minorGrid.Serialize(records);
  }

  [CLSCompliant(false)]
  protected void SerializeNumberFormat(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.NumberFormatIndex == -1)
      return;
    ChartIfmtRecord record = (ChartIfmtRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartIfmt);
    record.FormatIndex = (ushort) this.NumberFormatIndex;
    records.Add((IBiffStorage) record);
  }

  [CLSCompliant(false)]
  protected void SerializeAxisBorder(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_border == null)
      return;
    ChartAxisLineFormatRecord record = (ChartAxisLineFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxisLineFormat);
    record.LineIdentifier = ExcelAxisLineIdentifier.AxisLineItself;
    records.Add((IBiffStorage) record);
    this.m_border.Serialize((IList<IBiffStorage>) records);
  }

  [CLSCompliant(false)]
  protected void SerializeTickRecord(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    ChartTickRecord chartTickRecord = (ChartTickRecord) this.m_chartTick.Clone();
    chartTickRecord.IsLeftToRight = false;
    chartTickRecord.IsRightToLeft = false;
    if (this.Alignment == ExcelAxisTextDirection.LeftToRight)
      chartTickRecord.IsLeftToRight = true;
    else if (this.Alignment == ExcelAxisTextDirection.RightToLeft)
      chartTickRecord.IsRightToLeft = true;
    records.Add((IBiffStorage) chartTickRecord);
  }

  protected virtual void InitializeVariables()
  {
    this.SetParents();
    this.m_paraType = ChartParagraphType.Default;
    this.m_IsDefaultTextSettings = true;
    this.InitializeTickRecord();
  }

  private void InitializeTickRecord()
  {
    this.m_chartTick = (ChartTickRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartTick);
    this.m_chartTick.MajorMark = ExcelTickMark.TickMark_Outside;
    this.m_chartTick.LabelPos = ExcelTickLabelPosition.TickLabelPosition_NextToAxis;
    this.m_chartTick.IsAutoTextColor = true;
  }

  private void ColorChangeEventHandler(object sender, EventArgs e)
  {
    if (!(sender is FontWrapper) || this.m_font.Index <= -1 || this.m_font.IsAutoColor)
      return;
    this.m_chartTick.IsAutoTextColor = false;
    this.m_chartTick.TickColorIndex = (ushort) this.m_font.ColorObject.GetIndexed((IWorkbook) this.ParentWorkbook);
    this.m_chartTick.TextColorInInt = this.m_font.ColorObject.Value;
  }

  protected internal void SetTitleArea(ChartTextAreaImpl titleArea)
  {
    this.m_titleArea = titleArea != null ? titleArea : throw new ArgumentNullException(nameof (titleArea));
  }

  public virtual ChartAxisImpl Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartAxisImpl parent1 = (ChartAxisImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.NumberFormat = this.NumberFormat;
    parent1.m_axisType = this.m_axisType;
    parent1.m_bIsDisposed = this.m_bIsDisposed;
    parent1.m_bLineFormat = this.m_bLineFormat;
    parent1.m_bPrimary = this.m_bPrimary;
    if (this.m_chartTick != null)
      parent1.m_chartTick = (ChartTickRecord) this.m_chartTick.Clone();
    if (this.m_titleArea != null)
      parent1.m_titleArea = (ChartTextAreaImpl) this.m_titleArea.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    if (this.m_majorGrid != null)
      parent1.m_majorGrid = (ChartGridLineImpl) this.m_majorGrid.Clone((object) parent1);
    if (this.m_minorGrid != null)
      parent1.m_minorGrid = (ChartGridLineImpl) this.m_minorGrid.Clone((object) parent1);
    if (this.m_font != null)
      parent1.m_font = this.m_font.Clone(parent1.ParentWorkbook, (object) this, (IDictionary) dicFontIndexes);
    if (this.m_axisFormat != null)
      parent1.m_axisFormat = this.m_axisFormat.Clone((object) parent1);
    if (this.m_border != null)
      parent1.m_border = this.m_axisFormat == null || this.m_border != this.m_axisFormat.Border ? this.m_border.Clone((object) parent1) : parent1.m_axisFormat.Border as ChartBorderImpl;
    return parent1;
  }

  public ChartAxisImpl Clone(FontWrapper font)
  {
    ChartAxisImpl parent = this.MemberwiseClone() as ChartAxisImpl;
    parent.Font = (IFont) font.Clone((object) parent);
    return parent;
  }

  public void SetTitle(ChartTextAreaImpl text) => this.m_titleArea = text;

  public void UpdateTickRecord(ExcelTickLabelPosition value) => this.m_chartTick.LabelPos = value;

  public void MarkUsedReferences(bool[] usedItems)
  {
    this.m_titleArea.MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    this.m_titleArea.UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  protected void InitFrameFormat()
  {
    this.m_axisFormat = this.CreateFrameFormat();
    this.m_axisFormat.FrameRecord.AutoSize = true;
    this.m_axisFormat.Border.LinePattern = ExcelChartLinePattern.None;
    this.m_axisFormat.Border.AutoFormat = false;
    this.m_axisFormat.Interior.UseAutomaticFormat = false;
    this.m_axisFormat.Interior.Pattern = ExcelPattern.None;
  }

  protected virtual ChartFrameFormatImpl CreateFrameFormat()
  {
    return new ChartFrameFormatImpl(this.Application, (object) this);
  }

  internal void AssignReference(IChartBorder border) => this.m_border = border as ChartBorderImpl;

  internal void SetDefaultFont(string defaultFont, float defaultFontSize)
  {
    if (this.m_font == null)
      this.m_font = new FontWrapper((FontImpl) this.ParentWorkbook.InnerFonts[0]);
    this.m_font.FontName = defaultFont;
    this.m_font.Size = (double) defaultFontSize;
  }
}
