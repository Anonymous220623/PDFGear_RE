// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartSeriesAxisImpl
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

public class ChartSeriesAxisImpl : ChartAxisImpl, IChartSeriesAxis, IChartAxis, IScalable
{
  private const int DEF_MAX_SPACING_VALUE = 31999;
  private const ushort DEF_NOMULTILVLLBL_TAG = 46;
  private const ushort DEF_TICKLBLSKIP_TAG = 81;
  private ChartCatserRangeRecord m_chartCatserRange;
  private bool m_bAutoTickLabelSpacing = true;
  private UnknownRecord m_chartMlFrt;
  internal bool m_xmlTKLabelSkipFrt;

  public ChartSeriesAxisImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.AxisId = 63149376;
  }

  public ChartSeriesAxisImpl(IApplication application, object parent, ExcelAxisType axisType)
    : this(application, parent, axisType, true)
  {
  }

  public ChartSeriesAxisImpl(
    IApplication application,
    object parent,
    ExcelAxisType axisType,
    bool bIsPrimary)
    : base(application, parent, axisType, bIsPrimary)
  {
    this.AxisId = 63149376;
  }

  [CLSCompliant(false)]
  public ChartSeriesAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : this(application, parent, data, ref iPos, true)
  {
  }

  [CLSCompliant(false)]
  public ChartSeriesAxisImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos,
    bool isPrimary)
    : base(application, parent, data, ref iPos, isPrimary)
  {
    this.AxisId = 63149376;
  }

  public new bool AutoTickLabelSpacing
  {
    get => this.TickLabelSpacing == 1 && this.m_bAutoTickLabelSpacing;
    set
    {
      if (value)
      {
        this.TickLabelSpacing = 1;
        if (this.m_chartMlFrt != null)
          this.m_chartMlFrt.m_data[22] = (byte) 81;
      }
      else if (this.m_chartMlFrt != null)
        this.m_chartMlFrt.m_data[22] = (byte) 82;
      this.m_bAutoTickLabelSpacing = value;
    }
  }

  public int LabelFrequency
  {
    get => this.TickLabelSpacing;
    set => this.TickLabelSpacing = value;
  }

  public int TickLabelSpacing
  {
    get => (int) this.m_chartCatserRange.LabelsFrequency;
    set
    {
      this.m_chartCatserRange.LabelsFrequency = value >= 0 && value <= 31999 ? (ushort) value : throw new ArgumentOutOfRangeException("Value must be great then 0 and less then 31999.");
      if (this.m_chartCatserRange.LabelsFrequency == (ushort) 1)
        return;
      this.AutoTickLabelSpacing = false;
    }
  }

  public int TickMarksFrequency
  {
    get => this.TickMarkSpacing;
    set => this.TickMarkSpacing = value;
  }

  public int TickMarkSpacing
  {
    get => (int) this.m_chartCatserRange.TickMarksFrequency;
    set
    {
      this.m_chartCatserRange.TickMarksFrequency = value >= 0 && value <= 31999 ? (ushort) value : throw new ArgumentOutOfRangeException("Value must be great then 0 and less then 31999.");
    }
  }

  public override bool ReversePlotOrder
  {
    get => this.m_chartCatserRange.IsReverse;
    set => this.m_chartCatserRange.IsReverse = value;
  }

  [CLSCompliant(false)]
  protected override ExcelObjectTextLink TextLinkType => ExcelObjectTextLink.ZAxis;

  public int CrossesAt
  {
    get => (int) this.m_chartCatserRange.CrossingPoint;
    set => this.m_chartCatserRange.CrossingPoint = (ushort) value;
  }

  public bool IsBetween
  {
    get => this.m_chartCatserRange.IsBetween;
    set => this.m_chartCatserRange.IsBetween = value;
  }

  private void ParseMaxCross(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    record.CheckTypeCode(TBIFFRecord.ChartCatserRange);
    this.m_chartCatserRange = (ChartCatserRangeRecord) record;
  }

  protected override void ParseWallsOrFloor(IList<BiffRecordRaw> data, ref int iPos)
  {
    throw new NotSupportedException("Current axis type doesn't support walls or floors");
  }

  [CLSCompliant(false)]
  protected override void ParseData(BiffRecordRaw record, IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    switch (record.TypeCode)
    {
      case TBIFFRecord.ChartMlFrt:
        this.m_chartMlFrt = (UnknownRecord) record;
        break;
      case TBIFFRecord.ChartCatserRange:
        this.ParseMaxCross(record);
        break;
    }
  }

  [CLSCompliant(false)]
  public override void Serialize(OffsetArrayList records)
  {
    ChartAxisRecord record = (ChartAxisRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAxis);
    record.AxisType = ChartAxisRecord.ChartAxisType.SeriesAxis;
    records.Add((IBiffStorage) record);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    records.Add((IBiffStorage) this.m_chartCatserRange.Clone());
    this.SerializeTickRecord(records);
    this.SerializeNumberFormat(records);
    this.SerializeFont(records);
    this.SerializeAxisBorder(records);
    if (this.m_chartMlFrt != null)
      records.Add((IBiffStorage) this.m_chartMlFrt);
    else if ((!this.ParentWorkbook.IsCreated ? (!this.AutoTickLabelSpacing ? 0 : (this.m_xmlTKLabelSkipFrt ? 1 : 0)) : (this.AutoTickLabelSpacing ? 1 : 0)) != 0)
    {
      UnknownRecord unknownRecord = new UnknownRecord();
      unknownRecord.RecordCode = 2206;
      unknownRecord.m_data = new byte[32 /*0x20*/]
      {
        (byte) 158,
        (byte) 8,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 12,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 81,
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      unknownRecord.Length = unknownRecord.m_data.Length;
      records.Add((IBiffStorage) unknownRecord);
    }
    else
    {
      UnknownRecord unknownRecord = new UnknownRecord();
      unknownRecord.RecordCode = 2206;
      unknownRecord.m_data = new byte[32 /*0x20*/]
      {
        (byte) 158,
        (byte) 8,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 12,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 82,
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      unknownRecord.Length = unknownRecord.m_data.Length;
      records.Add((IBiffStorage) unknownRecord);
    }
    this.SerializeGridLines(records);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  protected override void InitializeVariables()
  {
    this.m_chartCatserRange = (ChartCatserRangeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartCatserRange);
    base.InitializeVariables();
  }

  public override ChartAxisImpl Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartSeriesAxisImpl chartSeriesAxisImpl = (ChartSeriesAxisImpl) base.Clone(parent, dicFontIndexes, dicNewSheetNames);
    if (this.m_chartCatserRange != null)
      chartSeriesAxisImpl.m_chartCatserRange = (ChartCatserRangeRecord) this.m_chartCatserRange.Clone();
    return (ChartAxisImpl) chartSeriesAxisImpl;
  }

  internal bool CheckForXmlTKOptions(BiffRecordRaw record)
  {
    bool flag = false;
    int int32 = BitConverter.ToInt32(record.Data, 12);
    byte[] destinationArray = new byte[int32];
    Array.Copy((Array) record.Data, 16 /*0x10*/, (Array) destinationArray, 0, int32);
    if (int32 > 9 && BitConverter.ToInt16(destinationArray, 2) == (short) 4 && BitConverter.ToInt16(destinationArray, 6) == (short) 81 && this.AutoTickLabelSpacing)
    {
      this.m_xmlTKLabelSkipFrt = true;
      flag = true;
    }
    return flag;
  }

  public bool IsLogScale
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public double MaximumValue
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public double MinimumValue
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }
}
