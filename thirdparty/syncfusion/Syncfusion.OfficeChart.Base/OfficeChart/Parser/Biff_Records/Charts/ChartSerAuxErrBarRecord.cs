// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartSerAuxErrBarRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartSerAuxErrBar)]
[CLSCompliant(false)]
internal class ChartSerAuxErrBarRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 14;
  [BiffRecordPos(0, 1)]
  private byte m_ErrorBarValue;
  [BiffRecordPos(1, 1)]
  private byte m_ErrorBarType = 2;
  [BiffRecordPos(2, 1)]
  private byte m_TeeTop = 1;
  [BiffRecordPos(3, 1)]
  private byte m_Reserved = 1;
  [BiffRecordPos(4, 8, TFieldType.Float)]
  private double m_NumValue = 10.0;
  [BiffRecordPos(12, 2)]
  private ushort m_usValuesNumber;

  public ChartSerAuxErrBarRecord.TErrorBarValue ErrorBarValue
  {
    get => (ChartSerAuxErrBarRecord.TErrorBarValue) this.m_ErrorBarValue;
    set => this.m_ErrorBarValue = (byte) value;
  }

  public OfficeErrorBarType ErrorBarType
  {
    get => (OfficeErrorBarType) this.m_ErrorBarType;
    set => this.m_ErrorBarType = (byte) value;
  }

  public bool TeeTop
  {
    get => this.m_TeeTop == (byte) 1;
    set => this.m_TeeTop = value ? (byte) 1 : (byte) 0;
  }

  public byte Reserved => this.m_Reserved;

  public double NumValue
  {
    get => this.m_NumValue;
    set => this.m_NumValue = value;
  }

  public ushort ValuesNumber
  {
    get => this.m_usValuesNumber;
    set => this.m_usValuesNumber = value;
  }

  public ChartSerAuxErrBarRecord()
  {
  }

  public ChartSerAuxErrBarRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSerAuxErrBarRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_ErrorBarValue = provider.ReadByte(iOffset);
    this.m_ErrorBarType = provider.ReadByte(iOffset + 1);
    this.m_TeeTop = provider.ReadByte(iOffset + 2);
    this.m_Reserved = provider.ReadByte(iOffset + 3);
    this.m_NumValue = provider.ReadDouble(iOffset + 4);
    this.m_usValuesNumber = provider.ReadUInt16(iOffset + 12);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteByte(iOffset, this.m_ErrorBarValue);
    provider.WriteByte(iOffset + 1, this.m_ErrorBarType);
    provider.WriteByte(iOffset + 2, this.m_TeeTop);
    provider.WriteByte(iOffset + 3, this.m_Reserved);
    provider.WriteDouble(iOffset + 4, this.m_NumValue);
    provider.WriteUInt16(iOffset + 12, this.m_usValuesNumber);
    this.m_iLength = 14;
  }

  public override int GetStoreSize(OfficeVersion version) => 14;

  public enum TErrorBarValue
  {
    XDirectionPlus = 1,
    XDirectionMinus = 2,
    YDirectionPlus = 3,
    YDirectionMinus = 4,
  }
}
