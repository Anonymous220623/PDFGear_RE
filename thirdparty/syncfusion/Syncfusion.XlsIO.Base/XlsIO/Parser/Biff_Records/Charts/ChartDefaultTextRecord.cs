// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartDefaultTextRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartDefaultText)]
[CLSCompliant(false)]
public class ChartDefaultTextRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usObjectIdentifier;

  public ChartDefaultTextRecord.TextDefaults TextCharacteristics
  {
    get => (ChartDefaultTextRecord.TextDefaults) this.m_usObjectIdentifier;
    set => this.m_usObjectIdentifier = (ushort) value;
  }

  public ChartDefaultTextRecord()
  {
  }

  public ChartDefaultTextRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartDefaultTextRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usObjectIdentifier = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usObjectIdentifier);
  }

  public override int GetStoreSize(ExcelVersion version) => 2;

  public enum TextDefaults
  {
    ShowLabels,
    ValueAndPercents,
    All,
  }
}
