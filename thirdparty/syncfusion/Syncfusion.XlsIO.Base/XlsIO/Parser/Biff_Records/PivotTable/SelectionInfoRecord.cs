// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.SelectionInfoRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.SelectionInfo)]
[CLSCompliant(false)]
public class SelectionInfoRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 26;
  [BiffRecordPos(0, 2)]
  private ushort m_usWindowIndex;
  [BiffRecordPos(2, 2)]
  private ushort m_usPaneIndex;

  public SelectionInfoRecord()
  {
  }

  public SelectionInfoRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SelectionInfoRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort WindowIndex
  {
    get => this.m_usWindowIndex;
    set => this.m_usWindowIndex = value;
  }

  public ushort PaneIndex
  {
    get => this.m_usPaneIndex;
    set => this.m_usPaneIndex = value;
  }

  public override int MaximumMemorySize => 26;

  public override int MinimumRecordSize => 26;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usWindowIndex = provider.ReadUInt16(iOffset);
    this.m_usPaneIndex = provider.ReadUInt16(iOffset + 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usWindowIndex);
    provider.WriteUInt16(iOffset + 2, this.m_usPaneIndex);
    provider.WriteByte(25, (byte) 0);
    this.m_iLength = 26;
  }

  public override int GetStoreSize(ExcelVersion version) => 26;
}
