// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.OleSizeRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.OleSize)]
public class OleSizeRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(2, 2)]
  private ushort m_usFirstRow;
  [BiffRecordPos(4, 2)]
  private ushort m_usLastRow;
  [BiffRecordPos(6, 1)]
  private byte m_FirstColumn;
  [BiffRecordPos(7, 1)]
  private byte m_LastColumn;

  public ushort Reserved => this.m_usReserved;

  public ushort FirstRow
  {
    get => this.m_usFirstRow;
    set => this.m_usFirstRow = value;
  }

  public ushort LastRow
  {
    get => this.m_usLastRow;
    set => this.m_usLastRow = value;
  }

  public byte FirstColumn
  {
    get => this.m_FirstColumn;
    set => this.m_FirstColumn = value;
  }

  public byte LastColumn
  {
    get => this.m_LastColumn;
    set => this.m_LastColumn = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public OleSizeRecord()
  {
  }

  public OleSizeRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public OleSizeRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usReserved = provider.ReadUInt16(iOffset);
    this.m_usFirstRow = provider.ReadUInt16(iOffset + 2);
    this.m_usLastRow = provider.ReadUInt16(iOffset + 4);
    this.m_FirstColumn = provider.ReadByte(iOffset + 6);
    this.m_LastColumn = provider.ReadByte(iOffset + 7);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usReserved);
    provider.WriteUInt16(iOffset + 2, this.m_usFirstRow);
    provider.WriteUInt16(iOffset + 4, this.m_usLastRow);
    provider.WriteByte(iOffset + 6, this.m_FirstColumn);
    provider.WriteByte(iOffset + 7, this.m_LastColumn);
    this.m_iLength = 8;
  }

  public override int GetStoreSize(ExcelVersion version) => 8;
}
