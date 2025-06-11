// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.GutsRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.Guts)]
[CLSCompliant(false)]
public class GutsRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usLeftRowGutter;
  [BiffRecordPos(2, 2)]
  private ushort m_usTopColGutter;
  [BiffRecordPos(4, 2)]
  private ushort m_usMaxRowLevel;
  [BiffRecordPos(6, 2)]
  private ushort m_usMaxColLevel;

  public ushort LeftRowGutter
  {
    get => this.m_usLeftRowGutter;
    set => this.m_usLeftRowGutter = value;
  }

  public ushort TopColumnGutter
  {
    get => this.m_usTopColGutter;
    set => this.m_usTopColGutter = value;
  }

  public ushort MaxRowLevel
  {
    get => this.m_usMaxRowLevel;
    set => this.m_usMaxRowLevel = value;
  }

  public ushort MaxColumnLevel
  {
    get => this.m_usMaxColLevel;
    set => this.m_usMaxColLevel = value;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public GutsRecord()
  {
  }

  public GutsRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public GutsRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usLeftRowGutter = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usTopColGutter = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usMaxRowLevel = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usMaxColLevel = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usLeftRowGutter);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usTopColGutter);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usMaxRowLevel);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usMaxColLevel);
  }
}
