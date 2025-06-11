// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.GridsetRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Gridset)]
public class GridsetRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usGridsetFlag = 1;

  public ushort GridsetFlag
  {
    get => this.m_usGridsetFlag;
    set => this.m_usGridsetFlag = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public GridsetRecord()
  {
  }

  public GridsetRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public GridsetRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usGridsetFlag = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usGridsetFlag);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;
}
