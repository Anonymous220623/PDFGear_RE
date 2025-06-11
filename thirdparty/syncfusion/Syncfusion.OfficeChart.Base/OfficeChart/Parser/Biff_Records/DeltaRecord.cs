// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DeltaRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Delta)]
[CLSCompliant(false)]
internal class DeltaRecord : BiffRecordRaw
{
  public const double DEFAULT_VALUE = 0.001;
  private const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(0, 8, TFieldType.Float)]
  private double m_dbMaxChange = 0.001;

  public double MaxChange
  {
    get => this.m_dbMaxChange;
    set => this.m_dbMaxChange = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public DeltaRecord()
  {
  }

  public DeltaRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DeltaRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_dbMaxChange = provider.ReadDouble(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = 8;
    provider.WriteDouble(iOffset, this.m_dbMaxChange);
  }
}
