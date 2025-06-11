// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartAxcextRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartAxcext)]
internal class ChartAxcextRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 18;
  [BiffRecordPos(0, 2)]
  private ushort m_usMinCategoryAxis;
  [BiffRecordPos(2, 2)]
  private ushort m_usMaxCategoryAxis;
  [BiffRecordPos(4, 2)]
  private ushort m_usMajor = 1;
  [BiffRecordPos(6, 2)]
  private ushort m_usMajorUnits;
  [BiffRecordPos(8, 2)]
  private ushort m_usMinor = 1;
  [BiffRecordPos(10, 2)]
  private ushort m_usMinorUnits;
  [BiffRecordPos(12, 2)]
  private ushort m_usBaseUnits;
  [BiffRecordPos(14, 2)]
  private ushort m_usCrossingPoint;
  [BiffRecordPos(16 /*0x10*/, 2)]
  private ChartAxcextRecord.OptionFlags m_options = ChartAxcextRecord.OptionFlags.DefaultMinimum | ChartAxcextRecord.OptionFlags.DefaultMaximum | ChartAxcextRecord.OptionFlags.DefaultMajorUnits | ChartAxcextRecord.OptionFlags.DefaultMinorUnits | ChartAxcextRecord.OptionFlags.DateAxis | ChartAxcextRecord.OptionFlags.DefaultBaseUnits | ChartAxcextRecord.OptionFlags.DefaultCrossPoint;

  public ushort MinCategoryOnAxis
  {
    get => this.m_usMinCategoryAxis;
    set
    {
      if ((int) value == (int) this.m_usMinCategoryAxis)
        return;
      this.m_usMinCategoryAxis = value;
    }
  }

  public ushort MaxCategoryOnAxis
  {
    get => this.m_usMaxCategoryAxis;
    set
    {
      if ((int) value == (int) this.m_usMaxCategoryAxis)
        return;
      this.m_usMaxCategoryAxis = value;
    }
  }

  public ushort Major
  {
    get => this.m_usMajor;
    set
    {
      if ((int) value == (int) this.m_usMajor)
        return;
      this.m_usMajor = value;
    }
  }

  public OfficeChartBaseUnit MajorUnits
  {
    get => (OfficeChartBaseUnit) this.m_usMajorUnits;
    set => this.m_usMajorUnits = (ushort) value;
  }

  public ushort Minor
  {
    get => this.m_usMinor;
    set
    {
      if ((int) value == (int) this.m_usMinor)
        return;
      this.m_usMinor = value;
    }
  }

  public OfficeChartBaseUnit MinorUnits
  {
    get => (OfficeChartBaseUnit) this.m_usMinorUnits;
    set => this.m_usMinorUnits = (ushort) value;
  }

  public OfficeChartBaseUnit BaseUnits
  {
    get => (OfficeChartBaseUnit) this.m_usBaseUnits;
    set => this.m_usBaseUnits = (ushort) value;
  }

  public ushort CrossingPoint
  {
    get => this.m_usCrossingPoint;
    set
    {
      if ((int) value == (int) this.m_usCrossingPoint)
        return;
      this.m_usCrossingPoint = value;
    }
  }

  public ushort Options => (ushort) this.m_options;

  public bool UseDefaultMinimum
  {
    get
    {
      return (this.m_options & ChartAxcextRecord.OptionFlags.DefaultMinimum) != ChartAxcextRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAxcextRecord.OptionFlags.DefaultMinimum;
      else
        this.m_options &= ~ChartAxcextRecord.OptionFlags.DefaultMinimum;
    }
  }

  public bool UseDefaultMaximum
  {
    get
    {
      return (this.m_options & ChartAxcextRecord.OptionFlags.DefaultMaximum) != ChartAxcextRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAxcextRecord.OptionFlags.DefaultMaximum;
      else
        this.m_options &= ~ChartAxcextRecord.OptionFlags.DefaultMaximum;
    }
  }

  public bool UseDefaultMajorUnits
  {
    get
    {
      return (this.m_options & ChartAxcextRecord.OptionFlags.DefaultMajorUnits) != ChartAxcextRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAxcextRecord.OptionFlags.DefaultMajorUnits;
      else
        this.m_options &= ~ChartAxcextRecord.OptionFlags.DefaultMajorUnits;
    }
  }

  public bool UseDefaultMinorUnits
  {
    get
    {
      return (this.m_options & ChartAxcextRecord.OptionFlags.DefaultMinorUnits) != ChartAxcextRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAxcextRecord.OptionFlags.DefaultMinorUnits;
      else
        this.m_options &= ~ChartAxcextRecord.OptionFlags.DefaultMinorUnits;
    }
  }

  public bool IsDateAxis
  {
    get
    {
      return (this.m_options & ChartAxcextRecord.OptionFlags.DateAxis) != ChartAxcextRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAxcextRecord.OptionFlags.DateAxis;
      else
        this.m_options &= ~ChartAxcextRecord.OptionFlags.DateAxis;
    }
  }

  public bool UseDefaultBaseUnits
  {
    get
    {
      return (this.m_options & ChartAxcextRecord.OptionFlags.DefaultBaseUnits) != ChartAxcextRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAxcextRecord.OptionFlags.DefaultBaseUnits;
      else
        this.m_options &= ~ChartAxcextRecord.OptionFlags.DefaultBaseUnits;
    }
  }

  public bool UseDefaultCrossPoint
  {
    get
    {
      return (this.m_options & ChartAxcextRecord.OptionFlags.DefaultCrossPoint) != ChartAxcextRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAxcextRecord.OptionFlags.DefaultCrossPoint;
      else
        this.m_options &= ~ChartAxcextRecord.OptionFlags.DefaultCrossPoint;
    }
  }

  public bool UseDefaultDateSettings
  {
    get
    {
      return (this.m_options & ChartAxcextRecord.OptionFlags.DefaultDateSettings) != ChartAxcextRecord.OptionFlags.None;
    }
    set
    {
      if (value)
        this.m_options |= ChartAxcextRecord.OptionFlags.DefaultDateSettings;
      else
        this.m_options &= ~ChartAxcextRecord.OptionFlags.DefaultDateSettings;
    }
  }

  public ChartAxcextRecord()
  {
  }

  public ChartAxcextRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAxcextRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usMinCategoryAxis = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usMaxCategoryAxis = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usMajor = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usMajorUnits = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usMinor = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usMinorUnits = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usBaseUnits = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usCrossingPoint = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_options = (ChartAxcextRecord.OptionFlags) provider.ReadUInt16(iOffset);
    if (this.m_usMajor <= (ushort) 1)
      return;
    this.m_options &= ~ChartAxcextRecord.OptionFlags.DefaultMajorUnits;
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usMinCategoryAxis);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usMaxCategoryAxis);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usMajor);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usMajorUnits);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usMinor);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usMinorUnits);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usBaseUnits);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usCrossingPoint);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) this.m_options);
  }

  public override int GetStoreSize(OfficeVersion version) => 18;

  [Flags]
  private enum OptionFlags
  {
    None = 0,
    DefaultMinimum = 1,
    DefaultMaximum = 2,
    DefaultMajorUnits = 4,
    DefaultMinorUnits = 8,
    DateAxis = 16, // 0x00000010
    DefaultBaseUnits = 32, // 0x00000020
    DefaultCrossPoint = 64, // 0x00000040
    DefaultDateSettings = 128, // 0x00000080
  }
}
