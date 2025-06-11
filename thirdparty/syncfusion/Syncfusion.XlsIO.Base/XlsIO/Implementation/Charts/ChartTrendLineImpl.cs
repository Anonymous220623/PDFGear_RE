// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartTrendLineImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartTrendLineImpl : CommonObject, IChartTrendLine
{
  private const int DEF_ORDER_MAX_VALUE = 6;
  private static Dictionary<ExcelTrendLineType, string> m_hashNames = new Dictionary<ExcelTrendLineType, string>(6);
  private ChartLegendEntryImpl chartLegendEntry;
  private ChartSerAuxTrendRecord m_record;
  private ShadowImpl m_shadow;
  private ChartBorderImpl m_border;
  private ChartBorderImpl m_trendLineBorder;
  private ChartSerieImpl m_serie;
  private ExcelTrendLineType m_type = ExcelTrendLineType.Linear;
  private bool m_isAutoName = true;
  private string m_strName = "";
  private ChartTextAreaImpl m_textArea;
  private ChartTextAreaImpl m_trendLineTextArea;
  private int m_iIndex;
  private ThreeDFormatImpl m_3D;

  static ChartTrendLineImpl()
  {
    ChartTrendLineImpl.m_hashNames.Add(ExcelTrendLineType.Exponential, "Expon. ");
    ChartTrendLineImpl.m_hashNames.Add(ExcelTrendLineType.Linear, "Linear ");
    ChartTrendLineImpl.m_hashNames.Add(ExcelTrendLineType.Logarithmic, "Log. ");
    ChartTrendLineImpl.m_hashNames.Add(ExcelTrendLineType.Moving_Average, " per. Mov. Avg. ");
    ChartTrendLineImpl.m_hashNames.Add(ExcelTrendLineType.Polynomial, "Poly. ");
    ChartTrendLineImpl.m_hashNames.Add(ExcelTrendLineType.Power, "Power ");
  }

  public ChartTrendLineImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.m_border = new ChartBorderImpl(application, (object) this);
    this.m_border.HasLineProperties = true;
    this.m_trendLineBorder = new ChartBorderImpl(application, (object) this);
    this.m_trendLineBorder.HasLineProperties = true;
    this.m_record = (ChartSerAuxTrendRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSerAuxTrend);
  }

  private void FindParents()
  {
    this.m_serie = (ChartSerieImpl) this.FindParent(typeof (ChartSerieImpl));
    if (this.m_serie == null)
      throw new NotSupportedException("Cannot find parent objects");
  }

  public ChartTrendLineImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos,
    out ChartLegendEntryImpl entry)
    : base(application, parent)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    this.FindParents();
    entry = (ChartLegendEntryImpl) null;
    this.Parse(data, ref iPos, ref entry);
  }

  internal ChartLegendEntryImpl LegendEntry
  {
    get => this.chartLegendEntry;
    set => this.chartLegendEntry = value;
  }

  public IChartBorder Border => (IChartBorder) this.m_border;

  internal IChartBorder TrendLineBorder
  {
    get
    {
      if (this.m_trendLineBorder == null)
        this.m_trendLineBorder = new ChartBorderImpl(this.Application, (object) this);
      return (IChartBorder) this.m_trendLineBorder;
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

  public double Backward
  {
    get => this.m_record.NumBackcast;
    set
    {
      if (value == this.Backward)
        return;
      this.CheckRecordProprties();
      this.CheckBackward(value);
      this.m_record.NumBackcast = value;
    }
  }

  public double Forward
  {
    get => this.m_record.NumForecast;
    set
    {
      if (this.Forward == value)
        return;
      this.CheckRecordProprties();
      this.m_record.NumForecast = value >= 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (Forward));
    }
  }

  public bool DisplayEquation
  {
    get => this.m_record.IsEquation;
    set
    {
      if (value == this.DisplayEquation)
        return;
      this.CheckRecordProprties();
      this.m_record.IsEquation = value;
      this.UpdateDataLabels(value);
    }
  }

  public bool DisplayRSquared
  {
    get => this.m_record.IsRSquared;
    set
    {
      if (value == this.DisplayRSquared)
        return;
      this.CheckRecordProprties();
      this.m_record.IsRSquared = value;
      this.UpdateDataLabels(value);
    }
  }

  public double Intercept
  {
    get => this.m_record.NumIntercept;
    set
    {
      if (this.Intercept == value)
        return;
      this.CheckRecordProprties();
      this.CheckIntercept();
      this.m_record.NumIntercept = this.Type != ExcelTrendLineType.Exponential || value > 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (Intercept));
    }
  }

  public bool InterceptIsAuto
  {
    get => double.IsNaN(this.Intercept);
    set
    {
      if (this.InterceptIsAuto == value)
        return;
      this.CheckRecordProprties();
      this.CheckIntercept();
      if (value)
        this.Intercept = ChartSerAuxTrendRecord.DEF_NAN_VALUE;
      else
        this.Intercept = this.Type == ExcelTrendLineType.Exponential ? 1.0 : 0.0;
    }
  }

  public ExcelTrendLineType Type
  {
    get => this.m_type;
    set
    {
      this.m_type = value;
      this.OnTypeChanging(value);
    }
  }

  public int Order
  {
    get => (int) this.m_record.Order;
    set
    {
      this.m_record.Order = value > 0 ? (byte) value : throw new ArgumentOutOfRangeException(nameof (Order));
    }
  }

  public bool NameIsAuto
  {
    get => this.m_isAutoName;
    set
    {
      if (this.NameIsAuto == value)
        return;
      if (value)
        this.m_strName = string.Empty;
      this.m_isAutoName = value;
    }
  }

  public string Name
  {
    get
    {
      if (!this.NameIsAuto)
        return this.m_strName;
      ExcelTrendLineType type = this.Type;
      string str = ChartTrendLineImpl.m_hashNames[type];
      if (type == ExcelTrendLineType.Moving_Average)
        str = this.Order.ToString() + str;
      return $"{str}({this.m_serie.Name})";
    }
    set
    {
      this.m_strName = value != null ? value : throw new ArgumentNullException(nameof (Name));
      this.NameIsAuto = false;
    }
  }

  public IChartTextArea DataLabel
  {
    get
    {
      return this.m_textArea != null ? (IChartTextArea) this.m_textArea : throw new NotSupportedException("Cannot return data label.");
    }
  }

  internal IChartTextArea TrendLineTextArea
  {
    get
    {
      if (this.m_trendLineTextArea == null)
        this.m_trendLineTextArea = new ChartTextAreaImpl(this.Application, this.Parent);
      return (IChartTextArea) this.m_trendLineTextArea;
    }
  }

  public void ClearFormats()
  {
    this.m_border.AutoFormat = true;
    this.m_type = ExcelTrendLineType.Linear;
    this.m_isAutoName = true;
    this.m_strName = "";
    this.m_record.Order = (byte) 1;
    this.m_record.IsRSquared = false;
    this.m_record.IsEquation = false;
    this.m_textArea = (ChartTextAreaImpl) null;
    this.m_record.NumForecast = 0.0;
    this.m_record.NumBackcast = 0.0;
    this.m_record.NumIntercept = ChartSerAuxTrendRecord.DEF_NAN_VALUE;
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    if (this.m_textArea == null)
      return;
    this.m_textArea.MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    if (this.m_textArea == null)
      return;
    this.m_textArea.UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  private void Parse(IList<BiffRecordRaw> data, ref int iPos, ref ChartLegendEntryImpl entry)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (data[iPos].TypeCode != TBIFFRecord.ChartSeries)
      throw new ArgumentOutOfRangeException(nameof (iPos));
    iPos += 2;
    int num = 1;
    ChartImpl parentChart = this.m_serie.ParentChart;
    bool hasLegend = parentChart.HasLegend;
    ChartLegendEntriesColl parent = (ChartLegendEntriesColl) null;
    if (hasLegend)
      parent = (ChartLegendEntriesColl) parentChart.Legend.LegendEntries;
    while (num > 0)
    {
      BiffRecordRaw line = data[iPos];
      switch (line.TypeCode)
      {
        case TBIFFRecord.ChartDataFormat:
          this.m_iIndex = (int) ((ChartDataFormatRecord) line).SeriesIndex;
          break;
        case TBIFFRecord.ChartLineFormat:
          this.m_border = new ChartBorderImpl(this.Application, (object) this, (ChartLineFormatRecord) line);
          break;
        case TBIFFRecord.ChartSeriesText:
          this.Name = ((ChartSeriesTextRecord) line).Text;
          break;
        case TBIFFRecord.Begin:
          ++num;
          break;
        case TBIFFRecord.End:
          --num;
          break;
        case TBIFFRecord.ChartLegendxn:
          if (hasLegend)
          {
            int trendIndex = this.m_serie.ParentSeries.TrendIndex;
            entry = new ChartLegendEntryImpl(this.Application, (object) parent, trendIndex, data, ref iPos);
            --iPos;
            break;
          }
          break;
        case TBIFFRecord.ChartSerAuxTrend:
          this.m_record = (ChartSerAuxTrendRecord) line;
          break;
      }
      ++iPos;
    }
    --iPos;
    this.UpdateType();
  }

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = this.m_serie.Index;
    ChartSeriesCollection parentSeries = this.m_serie.ParentSeries;
    ChartErrorBarsImpl.SerializeSerieRecord(records, 0);
    BiffRecordRaw record1 = BiffRecordFactory.GetRecord(TBIFFRecord.Begin);
    records.Add((IBiffStorage) record1);
    this.SerializeChartAi(records);
    ChartErrorBarsImpl.SerializeDataFormatRecords(records, this.m_border, index, parentSeries.TrendErrorBarIndex, (ChartMarkerFormatRecord) null);
    ChartSerParentRecord record2 = (ChartSerParentRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSerParent);
    this.SerializeDataLabels(parentSeries.TrendLabels);
    record2.Series = (ushort) (index + 1);
    records.Add((IBiffStorage) record2);
    this.m_record.UpdateType(this.m_type);
    records.Add((IBiffStorage) this.m_record);
    this.SerializeLegendEntry(records);
    ++parentSeries.TrendIndex;
    ++parentSeries.TrendErrorBarIndex;
    BiffRecordRaw record3 = BiffRecordFactory.GetRecord(TBIFFRecord.End);
    records.Add((IBiffStorage) record3);
  }

  private void SerializeChartAi(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    ChartAIRecord record1 = (ChartAIRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAI);
    record1.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToTitleOrText;
    record1.Reference = ChartAIRecord.ReferenceType.EnteredDirectly;
    records.Add((IBiffStorage) record1);
    if (!this.NameIsAuto)
    {
      ChartSeriesTextRecord record2 = (ChartSeriesTextRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSeriesText);
      record2.Text = this.m_strName;
      records.Add((IBiffStorage) record2);
    }
    ChartAIRecord chartAiRecord1 = (ChartAIRecord) record1.Clone();
    chartAiRecord1.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToValues;
    records.Add((IBiffStorage) chartAiRecord1);
    ChartAIRecord chartAiRecord2 = (ChartAIRecord) chartAiRecord1.Clone();
    chartAiRecord2.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToCategories;
    records.Add((IBiffStorage) chartAiRecord2);
    ChartAIRecord chartAiRecord3 = (ChartAIRecord) chartAiRecord2.Clone();
    chartAiRecord3.IndexIdentifier = ChartAIRecord.LinkIndex.LinkToBubbles;
    records.Add((IBiffStorage) chartAiRecord3);
  }

  private void SerializeLegendEntry(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    ChartImpl parentChart = this.m_serie.ParentChart;
    if (!parentChart.HasLegend)
      return;
    int trendIndex = this.m_serie.ParentSeries.TrendIndex;
    ((ChartLegendEntryImpl) parentChart.Legend.LegendEntries[trendIndex]).Serialize(records);
  }

  private void SerializeDataLabels(IList<IBiffStorage> records)
  {
    if (this.m_textArea == null)
      return;
    this.m_textArea.ObjectLink.SeriesNumber = (ushort) this.m_serie.ParentSeries.TrendErrorBarIndex;
    this.m_textArea.Serialize(records);
  }

  private void CheckRecordProprties()
  {
    if (this.Type == ExcelTrendLineType.Moving_Average)
      throw new NotSupportedException("This property doesnot support on current trend line type.");
  }

  private void CheckIntercept()
  {
    switch (this.Type)
    {
      case ExcelTrendLineType.Logarithmic:
      case ExcelTrendLineType.Power:
        throw new NotSupportedException("This property doesnot support in current trend line type.");
    }
  }

  private void CheckBackward(double value)
  {
    if (value < 0.0)
      throw new ArgumentOutOfRangeException("Backward");
    string startSerieType = ChartFormatImpl.GetStartSerieType(this.m_serie.SerieType);
    if ((startSerieType == "Bar" || startSerieType == "Column" || startSerieType == "Line") && value > 0.5)
      throw new ArgumentOutOfRangeException("The value must be between zero and 0,5");
    if (startSerieType == "Area")
      throw new NotSupportedException("This property doesnot supported on current trendline object.");
  }

  private void OnTypeChanging(ExcelTrendLineType type)
  {
    bool flag = type == ExcelTrendLineType.Moving_Average;
    if (flag && this.m_serie.PointNumber < 3 && !this.m_serie.ParentBook.Loading)
      throw new NotSupportedException("This trendline type is supported only if data points count is greater than 2");
    this.m_record.Order = flag || type == ExcelTrendLineType.Polynomial ? (byte) 2 : (byte) 1;
    this.m_record.NumIntercept = ChartSerAuxTrendRecord.DEF_NAN_VALUE;
  }

  private void CheckOrder(int value)
  {
    if (this.m_type != ExcelTrendLineType.Polynomial && this.m_type != ExcelTrendLineType.Moving_Average)
      throw new NotSupportedException("This property doesnot support in current trendline type.");
    int num = 6;
    if (this.m_type != ExcelTrendLineType.Moving_Average)
      num = this.m_serie.PointNumber - 1;
    if (value < 2 || value > num)
      throw new ArgumentOutOfRangeException("Order");
  }

  private void UpdateDataLabels(bool value)
  {
    if (value && this.m_textArea == null)
    {
      this.m_textArea = new ChartTextAreaImpl(this.Application, (object) this);
      this.m_textArea.IsTrend = true;
    }
    else
    {
      if (this.DisplayEquation || this.DisplayRSquared)
        return;
      this.m_textArea = (ChartTextAreaImpl) null;
    }
  }

  private void UpdateType()
  {
    bool flag = this.m_record.RegressionType == ChartSerAuxTrendRecord.TRegression.Polynomial;
    ChartSeriesCollection parentSeries = this.m_serie.ParentSeries;
    this.m_type = !flag || this.m_record.Order >= (byte) 2 ? (ExcelTrendLineType) this.m_record.RegressionType : ExcelTrendLineType.Linear;
    ++parentSeries.TrendIndex;
  }

  public void SetDataLabel(ChartTextAreaImpl area)
  {
    this.m_textArea = area != null ? area : throw new ArgumentNullException(nameof (area));
    this.m_textArea.IsTrend = true;
  }

  public int Index
  {
    get => this.m_iIndex;
    set => this.m_iIndex = value;
  }

  public ChartTrendLineImpl Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ChartTrendLineImpl parent1 = (ChartTrendLineImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.FindParents();
    parent1.m_border = this.m_border.Clone((object) parent1);
    if (this.m_record != null)
      parent1.m_record = (ChartSerAuxTrendRecord) CloneUtils.CloneCloneable((ICloneable) this.m_record);
    if (this.m_textArea != null)
      parent1.m_textArea = (ChartTextAreaImpl) this.m_textArea.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    return parent1;
  }
}
