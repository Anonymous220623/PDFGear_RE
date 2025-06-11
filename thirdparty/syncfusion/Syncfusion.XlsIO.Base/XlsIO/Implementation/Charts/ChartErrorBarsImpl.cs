// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartErrorBarsImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartErrorBarsImpl : CommonObject, IChartErrorBars
{
  public const int DEF_NUMBER_X_VALUE = 1;
  public const int DEF_NUMBER_Y_VALUE = 10;
  private ChartBorderImpl m_border;
  private ChartSerAuxErrBarRecord m_errorBarRecord;
  private ShadowImpl m_shadow;
  private ExcelErrorBarInclude m_include;
  private ChartSerieImpl m_serie;
  private IRange m_plusRange;
  private IRange m_minusRange;
  private bool m_bIsY;
  private ChartAIRecord m_chartAi = (ChartAIRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAI);
  private ChartMarkerFormatRecord m_markerFormat;
  private bool m_bChanged;
  private ThreeDFormatImpl m_3D;
  private object[] m_plusRangeValues;
  private object[] m_minusRangeValues;
  private bool m_isPlusNumberLiteral;
  private bool m_isMinusNumberLiteral;
  private string m_formatCode;

  public static void SerializeSerieRecord(IList<IBiffStorage> records, int count)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    ChartSeriesRecord record = (ChartSeriesRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSeries);
    record.BubbleDataType = ChartSeriesRecord.DataType.Numeric;
    record.StdX = ChartSeriesRecord.DataType.Numeric;
    record.StdY = ChartSeriesRecord.DataType.Numeric;
    record.ValuesCount = (ushort) count;
    records.Add((IBiffStorage) record);
  }

  public static void SerializeDataFormatRecords(
    IList<IBiffStorage> records,
    ChartBorderImpl border,
    int iSerieIndex,
    int iIndex,
    ChartMarkerFormatRecord marker)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (border == null)
      throw new ArgumentNullException(nameof (border));
    ChartDataFormatRecord record1 = (ChartDataFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartDataFormat);
    record1.PointNumber = ushort.MaxValue;
    record1.SeriesIndex = (ushort) iIndex;
    record1.SeriesNumber = (ushort) iSerieIndex;
    records.Add((IBiffStorage) record1);
    BiffRecordRaw record2 = BiffRecordFactory.GetRecord(TBIFFRecord.Begin);
    records.Add((IBiffStorage) record2);
    BiffRecordRaw record3 = BiffRecordFactory.GetRecord(TBIFFRecord.Chart3DDataFormat);
    records.Add((IBiffStorage) record3);
    border.Serialize(records);
    BiffRecordRaw record4 = BiffRecordFactory.GetRecord(TBIFFRecord.ChartAreaFormat);
    records.Add((IBiffStorage) record4);
    BiffRecordRaw record5 = BiffRecordFactory.GetRecord(TBIFFRecord.ChartPieFormat);
    records.Add((IBiffStorage) record5);
    BiffRecordRaw biffRecordRaw = marker == null ? BiffRecordFactory.GetRecord(TBIFFRecord.ChartMarkerFormat) : (BiffRecordRaw) marker;
    records.Add((IBiffStorage) biffRecordRaw);
    BiffRecordRaw record6 = BiffRecordFactory.GetRecord(TBIFFRecord.End);
    records.Add((IBiffStorage) record6);
  }

  public ChartErrorBarsImpl(IApplication application, object parent, bool bIsY)
    : base(application, parent)
  {
    this.m_border = new ChartBorderImpl(application, (object) this);
    this.m_bIsY = bIsY;
    this.m_errorBarRecord = (ChartSerAuxErrBarRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSerAuxErrBar);
    if (!bIsY)
      this.NumberValue = 1.0;
    this.FindParents();
  }

  public ChartErrorBarsImpl(IApplication application, object parent, IList<BiffRecordRaw> data)
    : base(application, parent)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    this.FindParents();
    this.Parse(data);
  }

  private void FindParents()
  {
    this.m_serie = (ChartSerieImpl) this.FindParent(typeof (ChartSerieImpl));
    if (this.m_serie == null)
      throw new NotSupportedException("Cannot find parent objects");
  }

  public IChartBorder Border => (IChartBorder) this.m_border;

  public ExcelErrorBarInclude Include
  {
    get => this.m_include;
    set
    {
      if (value == this.Include)
        return;
      if (!this.m_serie.ParentBook.Loading && !this.CheckInclude(value))
        throw new NotSupportedException("Cannot change include value, before set range values.");
      this.m_include = value;
    }
  }

  public bool HasCap
  {
    get => this.m_errorBarRecord.TeeTop;
    set => this.m_errorBarRecord.TeeTop = value;
  }

  public ExcelErrorBarType Type
  {
    get => this.m_errorBarRecord.ErrorBarType;
    set => this.m_errorBarRecord.ErrorBarType = value;
  }

  public double NumberValue
  {
    get => this.m_errorBarRecord.NumValue;
    set
    {
      this.m_errorBarRecord.NumValue = value >= 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (NumberValue));
    }
  }

  internal bool IsPlusNumberLiteral
  {
    get => this.m_isPlusNumberLiteral;
    set => this.m_isPlusNumberLiteral = value;
  }

  internal bool IsMinusNumberLiteral
  {
    get => this.m_isMinusNumberLiteral;
    set => this.m_isMinusNumberLiteral = value;
  }

  public IRange PlusRange
  {
    get => this.m_plusRange;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (PlusRange));
      if (this.m_include != ExcelErrorBarInclude.Both)
        this.m_include = this.m_minusRange == null ? ExcelErrorBarInclude.Plus : ExcelErrorBarInclude.Both;
      this.m_errorBarRecord.ErrorBarType = ExcelErrorBarType.Custom;
      this.m_plusRange = value;
      if (this.m_serie.ParentBook.Loading)
        return;
      this.m_chartAi.ParsedExpression = (value as INativePTG).GetNativePtg();
      this.m_bChanged = true;
    }
  }

  public IRange MinusRange
  {
    get => this.m_minusRange;
    set
    {
      if (value == null)
        throw new ArgumentNullException("PlusRange");
      if (this.m_include != ExcelErrorBarInclude.Both)
        this.m_include = this.m_plusRange == null ? ExcelErrorBarInclude.Minus : ExcelErrorBarInclude.Both;
      this.m_errorBarRecord.ErrorBarType = ExcelErrorBarType.Custom;
      this.m_minusRange = value;
      if (this.m_serie.ParentBook.Loading)
        return;
      this.m_chartAi.ParsedExpression = (value as INativePTG).GetNativePtg();
      this.m_bChanged = true;
    }
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

  public void ClearFormats()
  {
    this.Type = ExcelErrorBarType.Fixed;
    this.Border.AutoFormat = true;
    this.HasCap = true;
    this.NumberValue = this.m_bIsY ? 10.0 : 1.0;
    this.m_include = ExcelErrorBarInclude.Both;
    this.m_plusRange = (IRange) null;
    this.m_minusRange = (IRange) null;
  }

  public void Delete()
  {
    if (this.m_bIsY)
      this.m_serie.HasErrorBarsY = false;
    else
      this.m_serie.HasErrorBarsX = false;
  }

  private void Parse(IList<BiffRecordRaw> data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    IRange range = (IRange) null;
    int index = 0;
    for (int count = data.Count; index < count; ++index)
    {
      BiffRecordRaw line = data[index];
      switch (line.TypeCode)
      {
        case TBIFFRecord.ChartLineFormat:
          this.m_border = new ChartBorderImpl(this.Application, (object) this, (ChartLineFormatRecord) line);
          break;
        case TBIFFRecord.ChartMarkerFormat:
          this.m_markerFormat = (ChartMarkerFormatRecord) line;
          break;
        case TBIFFRecord.ChartAI:
          ChartAIRecord chartAiRecord = (ChartAIRecord) line;
          if (chartAiRecord.IndexIdentifier == ChartAIRecord.LinkIndex.LinkToValues)
          {
            this.m_chartAi = chartAiRecord;
            if (this.m_chartAi.Reference == ChartAIRecord.ReferenceType.Worksheet)
            {
              range = this.m_serie.GetRange(this.m_chartAi);
              break;
            }
            break;
          }
          break;
        case TBIFFRecord.ChartSerAuxErrBar:
          this.m_errorBarRecord = (ChartSerAuxErrBarRecord) line;
          break;
      }
    }
    ChartSerAuxErrBarRecord.TErrorBarValue errorBarValue = this.m_errorBarRecord.ErrorBarValue;
    bool flag = errorBarValue == ChartSerAuxErrBarRecord.TErrorBarValue.XDirectionPlus || errorBarValue == ChartSerAuxErrBarRecord.TErrorBarValue.YDirectionPlus;
    this.m_bIsY = errorBarValue == ChartSerAuxErrBarRecord.TErrorBarValue.YDirectionMinus || errorBarValue == ChartSerAuxErrBarRecord.TErrorBarValue.YDirectionPlus;
    this.m_include = flag ? ExcelErrorBarInclude.Plus : ExcelErrorBarInclude.Minus;
    if (range == null)
      return;
    if (flag)
      this.m_plusRange = range;
    else
      this.m_minusRange = range;
  }

  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_errorBarRecord.ValuesNumber == (ushort) 0)
      this.m_errorBarRecord.ValuesNumber = (ushort) 1;
    int index = this.m_serie.Index;
    if (this.m_include == ExcelErrorBarInclude.Both)
    {
      this.SerializeErrorBar(records, this.m_bIsY, true, index);
      this.m_errorBarRecord = (ChartSerAuxErrBarRecord) this.m_errorBarRecord.Clone();
      this.SerializeErrorBar(records, this.m_bIsY, false, index);
    }
    else
    {
      bool bIsPlus = this.m_include == ExcelErrorBarInclude.Plus;
      this.SerializeErrorBar(records, this.m_bIsY, bIsPlus, index);
    }
  }

  private void SerializeAiRecords(IList<IBiffStorage> records, bool bIsPlus)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    ChartAIRecord record = (ChartAIRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAI);
    record.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToTitleOrText;
    record.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    records.Add((IBiffStorage) record);
    ChartAIRecord chartAiRecord1 = (ChartAIRecord) record.Clone();
    chartAiRecord1.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToValues;
    records.Add((IBiffStorage) chartAiRecord1);
    if (this.Type == ExcelErrorBarType.Custom && this.m_bIsY)
    {
      chartAiRecord1.Reference = ChartAIRecord.ReferenceType.Worksheet;
      chartAiRecord1.ParsedExpression = this.GetPtg(bIsPlus);
    }
    ChartAIRecord chartAiRecord2 = (ChartAIRecord) chartAiRecord1.Clone();
    chartAiRecord2.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToCategories;
    if (this.Type == ExcelErrorBarType.Custom && !this.m_bIsY)
    {
      chartAiRecord2.Reference = ChartAIRecord.ReferenceType.Worksheet;
      chartAiRecord2.ParsedExpression = this.GetPtg(bIsPlus);
    }
    else
    {
      chartAiRecord2.ParsedExpression = (Ptg[]) null;
      chartAiRecord2.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    }
    records.Add((IBiffStorage) chartAiRecord2);
    ChartAIRecord chartAiRecord3 = (ChartAIRecord) chartAiRecord2.Clone();
    chartAiRecord3.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToBubbles;
    chartAiRecord3.ParsedExpression = (Ptg[]) null;
    chartAiRecord3.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    records.Add((IBiffStorage) chartAiRecord3);
  }

  private int GetCount(IRange range)
  {
    return range == null ? (this.m_chartAi == null || this.m_chartAi.ParsedExpression == null || this.m_chartAi.ParsedExpression.Length <= 0 ? 0 : (this.m_chartAi.ParsedExpression[0] is IRectGetter ? 1 : 0)) : (range as ICombinedRange).CellsCount;
  }

  private void SerializeErrorBar(
    IList<IBiffStorage> records,
    bool bIsYAxis,
    bool bIsPlus,
    int iSerieIndex)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int count1 = this.GetCount(this.m_minusRange);
    int count2 = this.GetCount(this.m_plusRange);
    ChartErrorBarsImpl.SerializeSerieRecord(records, bIsPlus ? count2 : count1);
    BiffRecordRaw record1 = BiffRecordFactory.GetRecord(TBIFFRecord.Begin);
    records.Add((IBiffStorage) record1);
    this.SerializeAiRecords(records, bIsPlus);
    ChartSeriesCollection parentSeries = this.m_serie.ParentSeries;
    ChartErrorBarsImpl.SerializeDataFormatRecords(records, this.m_border, iSerieIndex, parentSeries.TrendErrorBarIndex, this.m_markerFormat);
    ++parentSeries.TrendErrorBarIndex;
    ChartSerParentRecord record2 = (ChartSerParentRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSerParent);
    record2.Series = (ushort) (iSerieIndex + 1);
    records.Add((IBiffStorage) record2);
    this.m_errorBarRecord.ErrorBarValue = !bIsYAxis ? (bIsPlus ? ChartSerAuxErrBarRecord.TErrorBarValue.XDirectionPlus : ChartSerAuxErrBarRecord.TErrorBarValue.XDirectionMinus) : (bIsPlus ? ChartSerAuxErrBarRecord.TErrorBarValue.YDirectionPlus : ChartSerAuxErrBarRecord.TErrorBarValue.YDirectionMinus);
    records.Add((IBiffStorage) this.m_errorBarRecord);
    BiffRecordRaw record3 = BiffRecordFactory.GetRecord(TBIFFRecord.End);
    records.Add((IBiffStorage) record3);
  }

  private Ptg[] GetPtg(bool bIsPlus)
  {
    Ptg[] ptg;
    if (this.m_bChanged)
    {
      IRange range = bIsPlus ? this.m_plusRange : this.m_minusRange;
      ptg = range == null ? (Ptg[]) null : ((INativePTG) range).GetNativePtg();
    }
    else
      ptg = this.m_chartAi.ParsedExpression;
    return ptg;
  }

  public bool IsY => this.m_bIsY;

  internal object[] PlusRangeValues
  {
    get => this.m_plusRangeValues;
    set => this.m_plusRangeValues = value;
  }

  internal object[] MinusRangeValues
  {
    get => this.m_minusRangeValues;
    set => this.m_minusRangeValues = value;
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

  private bool CheckInclude(ExcelErrorBarInclude value)
  {
    if (this.Type != ExcelErrorBarType.Custom)
      return true;
    if (value == ExcelErrorBarInclude.Plus)
    {
      this.m_minusRange = (IRange) null;
      return this.m_plusRange != null;
    }
    if (value == ExcelErrorBarInclude.Minus)
    {
      this.m_plusRange = (IRange) null;
      return this.m_minusRange != null;
    }
    return this.m_minusRange != null && this.m_plusRange != null;
  }

  public ChartErrorBarsImpl Clone(object parent, Dictionary<string, string> hashNewNames)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ChartErrorBarsImpl parent1 = (ChartErrorBarsImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.FindParents();
    WorkbookImpl parentBook = parent1.m_serie.ParentBook;
    parent1.m_border = this.m_border.Clone((object) parent1);
    if (this.m_errorBarRecord != null)
      parent1.m_errorBarRecord = (ChartSerAuxErrBarRecord) CloneUtils.CloneCloneable((ICloneable) this.m_errorBarRecord);
    if (this.m_plusRange != null)
      parent1.m_plusRange = ((ICombinedRange) this.m_plusRange).Clone((object) parent1, hashNewNames, parentBook);
    if (this.m_minusRange != null)
      parent1.m_minusRange = ((ICombinedRange) this.m_minusRange).Clone((object) parent1, hashNewNames, parentBook);
    return parent1;
  }
}
