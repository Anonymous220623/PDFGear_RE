// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.XCTRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.XCT)]
public class XCTRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usCRNCount;
  [BiffRecordPos(2, 2)]
  private ushort m_usSheetTableIndex;

  public ushort CRNCount
  {
    get => this.m_usCRNCount;
    set => this.m_usCRNCount = value;
  }

  public ushort SheetTableIndex
  {
    get => this.m_usSheetTableIndex;
    set => this.m_usSheetTableIndex = value;
  }

  public override int MinimumRecordSize => 4;

  public override int MaximumRecordSize => 4;

  public XCTRecord()
  {
  }

  public XCTRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public XCTRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usCRNCount = provider.ReadUInt16(iOffset);
    this.m_usSheetTableIndex = provider.ReadUInt16(iOffset + 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 4;
    provider.WriteUInt16(iOffset, this.m_usCRNCount);
    provider.WriteUInt16(iOffset + 2, this.m_usSheetTableIndex);
  }
}
