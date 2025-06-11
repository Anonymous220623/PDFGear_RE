// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MMSRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.MMS)]
[CLSCompliant(false)]
public class MMSRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 1)]
  private byte m_AddMenuCount;
  [BiffRecordPos(1, 1)]
  private byte m_DelMenuCount;

  public byte AddMenuCount
  {
    get => this.m_AddMenuCount;
    set => this.m_AddMenuCount = value;
  }

  public byte DelMenuCount
  {
    get => this.m_DelMenuCount;
    set => this.m_DelMenuCount = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public MMSRecord()
  {
  }

  public MMSRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public MMSRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_AddMenuCount = provider.ReadByte(iOffset);
    this.m_DelMenuCount = provider.ReadByte(iOffset + 1);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteByte(iOffset, this.m_AddMenuCount);
    provider.WriteByte(iOffset + 1, this.m_DelMenuCount);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;
}
