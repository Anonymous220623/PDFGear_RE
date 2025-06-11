// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartValueAxisImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartValueAxisImpl : ChartAxisImpl, IOfficeChartValueAxis, IOfficeChartAxis, IScalable
{
  public static readonly double[] DEF_DISPLAY_UNIT_VALUES = new double[10]
  {
    0.0,
    100.0,
    1000.0,
    10000.0,
    100000.0,
    1000000.0,
    10000000.0,
    100000000.0,
    1000000000.0,
    1000000000000.0
  };
  private bool m_bHasDisplayUnitLabel;
  private ChartValueRangeRecord m_chartValueRange;
  private double m_displayUnitCustom = 1.0;
  private OfficeChartDisplayUnit m_displayUnit;
  private ChartWrappedTextAreaImpl m_displayUnitLabel;
  private bool m_bAutoTickLabelSpacing = true;

  public ChartValueAxisImpl(IApplication application, object parent)
    : base(application, parent)
  {
  }

  public ChartValueAxisImpl(IApplication application, object parent, OfficeAxisType axisType)
    : this(application, parent, axisType, true)
  {
  }

  public ChartValueAxisImpl(
    IApplication application,
    object parent,
    OfficeAxisType axisType,
    bool bIsPrimary)
    : base(application, parent, axisType, bIsPrimary)
  {
    this.AxisId = this.IsPrimary ? 57253888 : 61870848;
  }

  [CLSCompliant(false)]
  public ChartValueAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : this(application, parent, data, ref iPos, true)
  {
  }

  [CLSCompliant(false)]
  public ChartValueAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos,
    bool isPrimary)
    : base(application, parent, data, ref iPos, isPrimary)
  {
    this.AxisId = this.IsPrimary ? 57253888 : 61870848;
  }

  public double MinimumValue
  {
    get
    {
      this.CheckValueRangeRecord();
      return this.m_chartValueRange.NumMin;
    }
    set
    {
      if (!this.IsAutoMax && value >= this.MaximumValue)
        throw new ArgumentOutOfRangeException(nameof (MinimumValue));
      this.CheckValueRangeRecord();
      this.m_chartValueRange.NumMin = value;
      this.IsAutoMin = false;
    }
  }

  public double MaximumValue
  {
    get
    {
      this.CheckValueRangeRecord();
      return this.m_chartValueRange.NumMax;
    }
    set
    {
      if (!this.IsAutoMin && value <= this.MinimumValue)
        throw new ArgumentOutOfRangeException(nameof (MaximumValue));
      this.CheckValueRangeRecord();
      this.m_chartValueRange.NumMax = value;
      this.IsAutoMax = false;
    }
  }

  public virtual double MajorUnit
  {
    get
    {
      this.CheckValueRangeRecord();
      return this.m_chartValueRange.NumMajor;
    }
    set => this.SetMajorUnit(value);
  }

  public virtual double MinorUnit
  {
    get
    {
      this.CheckValueRangeRecord();
      return this.m_chartValueRange.NumMinor;
    }
    set => this.SetMinorUnit(value);
  }

  internal double CrossValue
  {
    get => this.CrossesAt;
    set => this.CrossesAt = value;
  }

  public virtual double CrossesAt
  {
    get => this.m_chartValueRange.NumCross;
    set
    {
      this.m_chartValueRange.NumCross = value;
      this.IsAutoCross = false;
    }
  }

  public virtual bool IsAutoMin
  {
    get => !this.CheckValueRangeRecord(false) || this.m_chartValueRange.IsAutoMin;
    set => this.SetAutoMin(true, value);
  }

  protected void SetAutoMin(bool check, bool value)
  {
    this.CheckValueRangeRecord(check);
    this.m_chartValueRange.IsAutoMin = value;
  }

  public virtual bool IsAutoMax
  {
    get => !this.CheckValueRangeRecord(false) || this.m_chartValueRange.IsAutoMax;
    set => this.SetAutoMax(true, value);
  }

  protected void SetAutoMax(bool check, bool value)
  {
    this.CheckValueRangeRecord(check);
    this.m_chartValueRange.IsAutoMax = value;
  }

  public new bool AutoTickLabelSpacing
  {
    get => this.IsAutoMajor && this.IsAutoMax && this.IsAutoMin && this.IsAutoMinor;
    set => this.m_bAutoTickLabelSpacing = value;
  }

  public virtual bool IsAutoMajor
  {
    get => this.m_chartValueRange.IsAutoMajor;
    set => this.m_chartValueRange.IsAutoMajor = value;
  }

  public virtual bool IsAutoMinor
  {
    get => this.m_chartValueRange.IsAutoMinor;
    set => this.m_chartValueRange.IsAutoMinor = value;
  }

  public virtual bool IsAutoCross
  {
    get => !this.CheckValueRangeRecord(false) || this.m_chartValueRange.IsAutoCross;
    set
    {
      this.CheckValueRangeRecord();
      this.m_chartValueRange.IsAutoCross = value;
    }
  }

  public bool IsLogScale
  {
    get => this.CheckValueRangeRecord(false) && this.m_chartValueRange.IsLogScale;
    set
    {
      this.CheckValueRangeRecord();
      this.m_chartValueRange.IsLogScale = value;
      if (!value)
        return;
      if (!this.m_chartValueRange.IsAutoMin && this.m_chartValueRange.NumMin < 1.0)
        this.m_chartValueRange.NumMin = 1.0;
      if (this.m_chartValueRange.IsAutoMax || this.m_chartValueRange.NumMax >= 1.0)
        return;
      this.m_chartValueRange.NumMax = 1.0;
    }
  }

  public override bool ReversePlotOrder
  {
    get => this.m_chartValueRange.IsReverse;
    set => this.m_chartValueRange.IsReverse = value;
  }

  public virtual bool IsMaxCross
  {
    get => this.m_chartValueRange.IsMaxCross;
    set => this.m_chartValueRange.IsMaxCross = value;
  }

  [CLSCompliant(false)]
  protected ChartValueRangeRecord ChartValueRange
  {
    get => this.m_chartValueRange;
    set => this.m_chartValueRange = value;
  }

  public double DisplayUnitCustom
  {
    get
    {
      this.CheckValueRangeRecord();
      return this.m_displayUnitCustom;
    }
    set
    {
      this.CheckValueRangeRecord();
      this.m_displayUnitCustom = value > 0.0 ? value : throw new ArgumentOutOfRangeException("The value must be large than zero.");
      this.DisplayUnit = OfficeChartDisplayUnit.Custom;
    }
  }

  public OfficeChartDisplayUnit DisplayUnit
  {
    get
    {
      this.CheckValueRangeRecord();
      return this.m_displayUnit;
    }
    set
    {
      this.CheckValueRangeRecord();
      this.m_displayUnit = value;
      if (value == OfficeChartDisplayUnit.None)
      {
        this.m_bHasDisplayUnitLabel = false;
        this.m_displayUnitCustom = 1.0;
        this.m_displayUnitLabel = (ChartWrappedTextAreaImpl) null;
      }
      else
      {
        int index = (int) value;
        if (index < ChartValueAxisImpl.DEF_DISPLAY_UNIT_VALUES.Length)
          this.m_displayUnitCustom = ChartValueAxisImpl.DEF_DISPLAY_UNIT_VALUES[index];
        if (this.ParentAxis.ParentChart.ParentWorkbook.IsWorkbookOpening)
          return;
        this.HasDisplayUnitLabel = true;
      }
    }
  }

  public bool HasDisplayUnitLabel
  {
    get
    {
      this.CheckValueRangeRecord();
      return this.m_bHasDisplayUnitLabel;
    }
    set
    {
      this.CheckValueRangeRecord();
      if (!this.ParentWorkbook.IsWorkbookOpening && this.m_displayUnit == OfficeChartDisplayUnit.None)
        throw new NotSupportedException("Doesnot support display unit label in DisplayUnit None mode.");
      if (value && this.m_displayUnitLabel == null)
        this.CreateDispalayUnitLabel();
      this.m_bHasDisplayUnitLabel = value;
    }
  }

  public IOfficeChartTextArea DisplayUnitLabel
  {
    get
    {
      this.CheckValueRangeRecord();
      return !this.HasDisplayUnitLabel ? (IOfficeChartTextArea) null : (IOfficeChartTextArea) this.m_displayUnitLabel;
    }
  }

  [CLSCompliant(false)]
  protected virtual void ParseMaxCross(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    record.CheckTypeCode(TBIFFRecord.ChartValueRange);
    this.m_chartValueRange = (ChartValueRangeRecord) record;
  }

  protected override void ParseWallsOrFloor(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    this.ParentChart.Floor = (IOfficeChartWallOrFloor) new ChartWallOrFloorImpl(this.Application, (object) this.ParentChart, false, data, ref iPos);
  }

  [CLSCompliant(false)]
  protected override void ParseData(BiffRecordRaw record, IList<BiffRecordRaw> data, ref int iPos)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    switch (record.TypeCode)
    {
      case TBIFFRecord.ChartBegDispUnit:
        this.ParseDisplayUnitLabel(data, ref iPos);
        break;
      case TBIFFRecord.ChartAxisDisplayUnits:
        this.ParseDisplayUnits((ChartAxisDisplayUnitsRecord) record);
        break;
      case TBIFFRecord.ChartValueRange:
        this.ParseMaxCross(record);
        break;
    }
  }

  private void ParseDisplayUnits(ChartAxisDisplayUnitsRecord record)
  {
    this.m_displayUnit = record != null ? record.DisplayUnit : throw new ArgumentNullException(nameof (record));
    this.m_displayUnitCustom = record.DisplayUnitValue;
    this.m_bHasDisplayUnitLabel = record.IsShowLabels;
  }

  private void ParseDisplayUnitLabel(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartBegDispUnit);
    ++iPos;
    for (BiffRecordRaw biffRecordRaw = ChartTextAreaImpl.UnwrapRecord(data[iPos]); biffRecordRaw.TypeCode != TBIFFRecord.ChartEndDispUnit; biffRecordRaw = ChartTextAreaImpl.UnwrapRecord(data[iPos]))
    {
      if (biffRecordRaw.TypeCode == TBIFFRecord.ChartText)
      {
        this.m_displayUnitLabel = new ChartWrappedTextAreaImpl(this.Application, (object) this, data, ref iPos);
        --iPos;
      }
      ++iPos;
    }
  }

  [CLSCompliant(false)]
  public override void Serialize(OffsetArrayList records)
  {
    this.Serialize(records, ChartAxisRecord.ChartAxisType.ValueAxis);
  }

  [CLSCompliant(false)]
  protected void Serialize(OffsetArrayList records, ChartAxisRecord.ChartAxisType axisType)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    ChartAxisRecord record = (ChartAxisRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxis);
    record.AxisType = axisType;
    records.Add((IBiffStorage) record);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    records.Add((IBiffStorage) this.m_chartValueRange.Clone());
    this.SerializeDisplayUnits(records);
    this.SerializeNumberFormat(records);
    this.SerializeTickRecord(records);
    this.SerializeFont(records);
    this.SerializeAxisBorder(records);
    if (this.IsPrimary)
    {
      this.SerializeGridLines(records);
      this.SerializeWallsOrFloor(records);
    }
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  [CLSCompliant(false)]
  protected virtual void SerializeWallsOrFloor(OffsetArrayList records)
  {
    this.ParentChart.SerializeFloor(records);
  }

  private void SerializeDisplayUnits(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_displayUnit == OfficeChartDisplayUnit.None)
      return;
    ChartAxisDisplayUnitsRecord record1 = (ChartAxisDisplayUnitsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxisDisplayUnits);
    record1.IsShowLabels = this.m_bHasDisplayUnitLabel;
    record1.DisplayUnitValue = this.m_displayUnitCustom;
    record1.DisplayUnit = this.m_displayUnit;
    records.Add((IBiffStorage) record1);
    ChartBegDispUnitRecord record2 = (ChartBegDispUnitRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartBegDispUnit);
    record2.IsShowLabels = this.m_bHasDisplayUnitLabel;
    records.Add((IBiffStorage) record2);
    if (this.m_bHasDisplayUnitLabel)
      this.m_displayUnitLabel.Serialize((IList<IBiffStorage>) records);
    ChartEndDispUnitRecord record3 = (ChartEndDispUnitRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartEndDispUnit);
    record3.IsShowLabels = this.m_bHasDisplayUnitLabel;
    records.Add((IBiffStorage) record3);
  }

  protected override void InitializeVariables()
  {
    this.m_chartValueRange = (ChartValueRangeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartValueRange);
    base.InitializeVariables();
  }

  protected bool CheckValueRangeRecord()
  {
    return this.CheckValueRangeRecord(!this.ParentWorkbook.IsWorkbookOpening && !this.ParentWorkbook.Saving && !this.ParentWorkbook.IsCreated);
  }

  protected virtual bool CheckValueRangeRecord(bool throwException) => true;

  [CLSCompliant(false)]
  protected override ExcelObjectTextLink TextLinkType => ExcelObjectTextLink.YAxis;

  public override ChartAxisImpl Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartValueAxisImpl parent1 = (ChartValueAxisImpl) base.Clone(parent, dicFontIndexes, dicNewSheetNames);
    if (this.m_chartValueRange != null)
      parent1.m_chartValueRange = (ChartValueRangeRecord) this.m_chartValueRange.Clone();
    if (this.m_displayUnitLabel != null)
      parent1.m_displayUnitLabel = (ChartWrappedTextAreaImpl) this.m_displayUnitLabel.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    parent1.m_displayUnit = this.m_displayUnit;
    return (ChartAxisImpl) parent1;
  }

  private void CreateDispalayUnitLabel()
  {
    this.m_displayUnitLabel = new ChartWrappedTextAreaImpl(this.Application, (object) this, ExcelObjectTextLink.DisplayUnit);
    this.m_displayUnitLabel.TextRecord.IsAutoText = true;
    this.m_displayUnitLabel.Bold = true;
    this.m_displayUnitLabel.IsAutoMode = true;
    if (this.AxisType != OfficeAxisType.Value)
      return;
    this.m_displayUnitLabel.TextRotationAngle = 90;
  }

  public void SetMajorUnit(double value)
  {
    if (value <= 0.0 || !this.IsAutoMinor && value < this.MinorUnit)
      throw new ArgumentOutOfRangeException("MajorUnit");
    this.m_chartValueRange.NumMajor = value;
    this.IsAutoMajor = false;
  }

  public void SetMinorUnit(double value)
  {
    if (value <= 0.0 || !this.IsAutoMajor && value > this.m_chartValueRange.NumMajor)
      throw new ArgumentOutOfRangeException("MinorUnit");
    this.m_chartValueRange.NumMinor = value;
    this.IsAutoMinor = false;
  }
}
