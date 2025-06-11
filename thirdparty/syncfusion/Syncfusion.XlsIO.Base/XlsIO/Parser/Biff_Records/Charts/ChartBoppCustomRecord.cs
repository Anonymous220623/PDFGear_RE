// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartBoppCustomRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartBoppCustom)]
public class ChartBoppCustomRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usQuantity;
  private byte[] m_bits;

  public ushort Counter => this.m_usQuantity;

  public byte[] BitFields
  {
    get => this.m_bits;
    set
    {
      this.m_bits = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_usQuantity = (ushort) value.Length;
    }
  }

  public ChartBoppCustomRecord()
  {
  }

  public ChartBoppCustomRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartBoppCustomRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usQuantity = provider.ReadUInt16(iOffset);
    this.m_bits = new byte[(int) this.m_usQuantity];
    provider.ReadArray(iOffset + 2, this.m_bits);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usQuantity);
    provider.WriteBytes(iOffset + 2, this.m_bits, 0, this.m_bits.Length);
    this.m_iLength = this.m_bits.Length + 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2 + (int) this.m_usQuantity;

  public override object Clone()
  {
    ChartBoppCustomRecord boppCustomRecord = (ChartBoppCustomRecord) base.Clone();
    boppCustomRecord.m_bits = CloneUtils.CloneByteArray(this.m_bits);
    return (object) boppCustomRecord;
  }
}
