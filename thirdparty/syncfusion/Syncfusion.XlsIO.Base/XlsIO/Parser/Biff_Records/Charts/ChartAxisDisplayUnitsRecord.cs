// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAxisDisplayUnitsRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartAxisDisplayUnits)]
[CLSCompliant(false)]
public class ChartAxisDisplayUnitsRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 16 /*0x10*/;
  [BiffRecordPos(4, 2)]
  private ushort m_displayUnit;
  [BiffRecordPos(6, 8, TFieldType.Float)]
  private double m_displayUnitValue;
  [BiffRecordPos(14, 1)]
  private byte m_isShowLabels;
  [BiffRecordPos(15, 1)]
  private byte m_reserved;

  public ChartAxisDisplayUnitsRecord()
  {
  }

  public ChartAxisDisplayUnitsRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAxisDisplayUnitsRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ExcelChartDisplayUnit DisplayUnit
  {
    get => (ExcelChartDisplayUnit) this.m_displayUnit;
    set => this.m_displayUnit = (ushort) value;
  }

  public double DisplayUnitValue
  {
    get => this.m_displayUnitValue;
    set => this.m_displayUnitValue = value;
  }

  public bool IsShowLabels
  {
    get => this.m_isShowLabels == (byte) 3;
    set => this.m_isShowLabels = value ? (byte) 3 : (byte) 1;
  }

  public byte Recerved
  {
    get => this.m_reserved;
    set => this.m_reserved = value;
  }

  public override int MinimumRecordSize => 16 /*0x10*/;

  public override int MaximumRecordSize => 16 /*0x10*/;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    iOffset += 4;
    this.m_displayUnit = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_displayUnitValue = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_isShowLabels = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_reserved = provider.ReadByte(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, (ushort) this.TypeCode);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_displayUnit);
    iOffset += 2;
    provider.WriteDouble(iOffset, this.m_displayUnitValue);
    iOffset += 8;
    provider.WriteByte(iOffset, this.m_isShowLabels);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_reserved);
    this.m_iLength = 16 /*0x10*/;
  }

  public override int GetStoreSize(ExcelVersion version) => 16 /*0x10*/;
}
