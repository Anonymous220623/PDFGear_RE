// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartParentAxisImpl
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

public class ChartParentAxisImpl : CommonObject
{
  private ChartAxisParentRecord m_parentAxis;
  private ChartPosRecord m_position;
  private ChartCategoryAxisImpl m_categoryAxis;
  private ChartValueAxisImpl m_valueAxis;
  private ChartSeriesAxisImpl m_seriesAxis;
  internal ChartImpl m_parentChart;
  private ChartGlobalFormatsCollection m_globalFormats;

  public ChartParentAxisImpl(IApplication application, object parent)
    : this(application, parent, true)
  {
  }

  public ChartParentAxisImpl(IApplication application, object parent, bool isPrimary)
    : base(application, parent)
  {
    this.m_parentAxis = (ChartAxisParentRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxisParent);
    if (isPrimary)
    {
      this.m_categoryAxis = new ChartCategoryAxisImpl(application, (object) this, ExcelAxisType.Category, this.IsPrimary);
      this.m_valueAxis = new ChartValueAxisImpl(application, (object) this, ExcelAxisType.Value, this.IsPrimary);
    }
    if (!this.IsPrimary)
      this.m_parentAxis.AxesIndex = (ushort) 1;
    this.SetParents();
  }

  private void SetParents()
  {
    this.m_parentChart = (ChartImpl) this.FindParent(typeof (ChartImpl));
    if (this.m_parentChart == null)
      throw new ArgumentException("Can't find parent objects.");
  }

  [CLSCompliant(false)]
  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    this.m_parentAxis = data != null ? (ChartAxisParentRecord) data[iPos] : throw new ArgumentNullException(nameof (data));
    ++iPos;
    data[iPos].CheckTypeCode(TBIFFRecord.Begin);
    ++iPos;
    int num = 1;
    while (num != 0)
    {
      BiffRecordRaw biffRecordRaw = data[iPos];
      switch (biffRecordRaw.TypeCode)
      {
        case TBIFFRecord.ChartChartFormat:
          this.ParseChartFormat(data, ref iPos);
          break;
        case TBIFFRecord.ChartAxis:
          this.ParseAxes(data, ref iPos);
          break;
        case TBIFFRecord.ChartText:
          this.ParseChartText(data, ref iPos);
          break;
        case TBIFFRecord.Begin:
          iPos = BiffRecordRaw.SkipBeginEndBlock(data, iPos) - 1;
          break;
        case TBIFFRecord.End:
          --num;
          break;
        case TBIFFRecord.ChartPlotArea:
          this.m_parentChart.PlotArea = (IChartFrameFormat) new ChartPlotAreaImpl(this.Application, (object) this.m_parentChart, data, ref iPos);
          break;
        case TBIFFRecord.ChartPos:
          this.m_position = (ChartPosRecord) biffRecordRaw;
          break;
      }
      ++iPos;
    }
  }

  private void ParseAxes(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartAxis);
    switch (((ChartAxisRecord) data[iPos]).AxisType)
    {
      case ChartAxisRecord.ChartAxisType.CategoryAxis:
        this.m_categoryAxis = new ChartCategoryAxisImpl(this.Application, (object) this, data, ref iPos, this.m_parentAxis.AxesIndex == (ushort) 0);
        break;
      case ChartAxisRecord.ChartAxisType.ValueAxis:
        this.m_valueAxis = new ChartValueAxisImpl(this.Application, (object) this, data, ref iPos, this.m_parentAxis.AxesIndex == (ushort) 0);
        break;
      case ChartAxisRecord.ChartAxisType.SeriesAxis:
        this.m_seriesAxis = new ChartSeriesAxisImpl(this.Application, (object) this, data, ref iPos, this.m_parentAxis.AxesIndex == (ushort) 0);
        break;
      default:
        throw new ArgumentOutOfRangeException("Unknown chart axis type");
    }
    --iPos;
  }

  private void ParseChartText(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    ChartTextAreaImpl text = new ChartTextAreaImpl(this.Application, (object) this);
    iPos = text.Parse(data, iPos) - 1;
    switch (text.ObjectLink.LinkObject)
    {
      case ExcelObjectTextLink.YAxis:
        if (this.m_valueAxis == null)
          break;
        this.m_valueAxis.SetTitle(text);
        break;
      case ExcelObjectTextLink.XAxis:
        if (this.m_categoryAxis == null)
          break;
        this.m_categoryAxis.SetTitle(text);
        break;
      case ExcelObjectTextLink.ZAxis:
        if (this.m_seriesAxis == null)
          break;
        this.m_seriesAxis.SetTitle(text);
        break;
    }
  }

  private void ParseChartFormat(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    ChartFormatCollection parent = this.IsPrimary ? this.m_globalFormats.PrimaryFormats : this.m_globalFormats.SecondaryFormats;
    ChartFormatImpl formatToAdd = new ChartFormatImpl(this.Application, (object) parent);
    formatToAdd.Parse(data, ref iPos);
    parent.Add(formatToAdd, false);
    --iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_parentAxis == null)
      return;
    bool flag = Array.IndexOf<ExcelChartType>(ChartImpl.DEF_SUPPORT_SERIES_AXIS, this.m_parentChart.ChartType) != -1;
    records.Add((IBiffStorage) this.m_parentAxis.Clone());
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    if (this.m_position != null && this.IsPrimary)
      records.Add((IBiffStorage) this.m_position.Clone());
    if (this.m_categoryAxis != null)
      this.m_categoryAxis.Serialize(records);
    if (this.m_valueAxis != null)
      this.m_valueAxis.Serialize(records);
    if (this.m_seriesAxis != null && this.m_parentAxis.AxesIndex == (ushort) 0 && flag)
      this.m_seriesAxis.Serialize(records);
    if (this.m_categoryAxis != null)
      this.m_categoryAxis.SerializeAxisTitle(records);
    if (this.m_valueAxis != null)
      this.m_valueAxis.SerializeAxisTitle(records);
    if (this.m_seriesAxis != null && this.m_parentAxis.AxesIndex == (ushort) 0 && flag)
      this.m_seriesAxis.SerializeAxisTitle(records);
    if (this.IsPrimary)
    {
      this.m_parentChart.SerializePlotArea(records);
      this.m_globalFormats.PrimaryFormats.Serialize(records);
    }
    else
      this.m_globalFormats.SecondaryFormats.Serialize(records);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  internal ChartAxisParentRecord ParentAxisRecord
  {
    get
    {
      if (this.m_parentAxis == null)
        this.m_parentAxis = (ChartAxisParentRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxisParent);
      return this.m_parentAxis;
    }
  }

  public ChartFormatCollection ChartFormats
  {
    get
    {
      return this.IsPrimary ? this.m_globalFormats.PrimaryFormats : this.m_globalFormats.SecondaryFormats;
    }
  }

  public bool IsPrimary => this.m_parentAxis.AxesIndex == (ushort) 0;

  public ChartCategoryAxisImpl CategoryAxis
  {
    get => this.m_categoryAxis;
    set => this.m_categoryAxis = value;
  }

  public ChartValueAxisImpl ValueAxis
  {
    get => this.m_valueAxis;
    set => this.m_valueAxis = value;
  }

  public ChartSeriesAxisImpl SeriesAxis
  {
    get => this.m_seriesAxis;
    set => this.m_seriesAxis = value;
  }

  public ChartImpl ParentChart
  {
    get
    {
      return (this.FindParent(typeof (ChartImpl)) ?? throw new ArgumentException("cannot find parent object.")) as ChartImpl;
    }
  }

  public ChartGlobalFormatsCollection Formats => this.m_globalFormats;

  public void CreatePrimaryFormats()
  {
    ChartGlobalFormatsCollection formatsCollection = new ChartGlobalFormatsCollection(this.Application, this.ParentChart.PrimaryParentAxis, this.ParentChart.SecondaryParentAxis);
    this.ParentChart.PrimaryParentAxis.m_globalFormats = formatsCollection;
    this.ParentChart.SecondaryParentAxis.m_globalFormats = formatsCollection;
    if (this.m_parentChart.ParentWorkbook.Loading)
      return;
    ChartFormatImpl formatToAdd = new ChartFormatImpl(this.Application, (object) formatsCollection.PrimaryFormats);
    formatsCollection.PrimaryFormats.Add(formatToAdd, false);
  }

  public void UpdateSecondaryAxis(bool bCreateAxis)
  {
    this.m_parentAxis = (ChartAxisParentRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxisParent);
    this.m_parentAxis.AxesIndex = (ushort) 1;
    this.m_position = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
    if (!bCreateAxis)
      return;
    if (this.m_categoryAxis == null)
      this.m_categoryAxis = new ChartCategoryAxisImpl(this.Application, (object) this, ExcelAxisType.Category, false);
    if (this.m_valueAxis == null)
      this.m_valueAxis = new ChartValueAxisImpl(this.Application, (object) this, ExcelAxisType.Value, false);
    if (this.m_categoryAxis.AxisId == (this.ParentChart.PrimaryCategoryAxis as ChartAxisImpl).AxisId)
    {
      this.m_categoryAxis.AxisId = 62908672;
      (this.ParentChart.PrimaryCategoryAxis as ChartAxisImpl).AxisId = 59983360;
    }
    if (this.m_valueAxis.AxisId != (this.ParentChart.PrimaryValueAxis as ChartAxisImpl).AxisId)
      return;
    this.m_valueAxis.AxisId = 61870848;
    (this.ParentChart.PrimaryValueAxis as ChartAxisImpl).AxisId = 57253888;
  }

  public ChartParentAxisImpl Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartParentAxisImpl parent1 = (ChartParentAxisImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_parentAxis = (ChartAxisParentRecord) CloneUtils.CloneCloneable((ICloneable) this.m_parentAxis);
    parent1.m_position = (ChartPosRecord) CloneUtils.CloneCloneable((ICloneable) this.m_position);
    if (this.IsPrimary)
    {
      if (this.m_globalFormats != null)
        parent1.m_globalFormats = this.m_globalFormats.CloneForPrimary((object) parent1);
    }
    else if (this.m_globalFormats != null)
    {
      parent1.m_globalFormats = parent1.ParentChart.PrimaryParentAxis.Formats;
      this.m_globalFormats.CloneForSecondary(parent1.m_globalFormats, (object) parent1);
    }
    if (this.m_seriesAxis != null)
      parent1.m_seriesAxis = (ChartSeriesAxisImpl) this.m_seriesAxis.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    if (this.m_valueAxis != null)
      parent1.m_valueAxis = (ChartValueAxisImpl) this.m_valueAxis.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    if (this.m_categoryAxis != null)
      parent1.m_categoryAxis = (ChartCategoryAxisImpl) this.m_categoryAxis.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    return parent1;
  }

  public void ClearGridLines()
  {
    if (this.m_categoryAxis != null)
    {
      this.m_categoryAxis.HasMajorGridLines = false;
      this.m_categoryAxis.HasMinorGridLines = false;
    }
    if (this.m_valueAxis != null)
    {
      this.m_categoryAxis.HasMajorGridLines = false;
      this.m_categoryAxis.HasMinorGridLines = false;
    }
    if (this.m_seriesAxis == null)
      return;
    this.m_categoryAxis.HasMajorGridLines = false;
    this.m_categoryAxis.HasMinorGridLines = false;
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    if (this.m_valueAxis != null)
      this.m_valueAxis.MarkUsedReferences(usedItems);
    if (this.m_categoryAxis != null)
      this.m_categoryAxis.MarkUsedReferences(usedItems);
    if (this.m_seriesAxis == null)
      return;
    this.m_seriesAxis.MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    if (this.m_valueAxis != null)
      this.m_valueAxis.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_categoryAxis != null)
      this.m_categoryAxis.UpdateReferenceIndexes(arrUpdatedIndexes);
    if (this.m_seriesAxis == null)
      return;
    this.m_seriesAxis.UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  internal void RemoveAxis(bool isCategory)
  {
    if (isCategory)
      this.m_categoryAxis = (ChartCategoryAxisImpl) null;
    else
      this.m_valueAxis = (ChartValueAxisImpl) null;
  }

  protected override void OnDispose()
  {
    if (this.m_categoryAxis != null)
    {
      this.m_categoryAxis.Dispose();
      this.m_categoryAxis.ChartValueRange = (ChartValueRangeRecord) null;
      this.m_globalFormats = (ChartGlobalFormatsCollection) null;
    }
    if (this.m_seriesAxis != null)
    {
      this.m_seriesAxis.Dispose();
      this.m_seriesAxis = (ChartSeriesAxisImpl) null;
    }
    if (this.m_valueAxis == null)
      return;
    this.m_valueAxis.ChartValueRange = (ChartValueRangeRecord) null;
    this.m_valueAxis.Dispose();
    this.m_valueAxis = (ChartValueAxisImpl) null;
  }
}
