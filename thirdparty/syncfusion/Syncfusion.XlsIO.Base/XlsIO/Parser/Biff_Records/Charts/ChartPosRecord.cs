// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartPosRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartPos)]
public class ChartPosRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 20;
  [BiffRecordPos(0, 2)]
  private ushort m_usTopLeft;
  [BiffRecordPos(2, 2)]
  private ushort m_usBottomRight = 2;
  [BiffRecordPos(4, 4, true)]
  private int m_iX1;
  [BiffRecordPos(8, 4, true)]
  private int m_iY1;
  [BiffRecordPos(12, 4, true)]
  private int m_iX2;
  [BiffRecordPos(16 /*0x10*/, 4, true)]
  private int m_iY2;

  public ushort TopLeft
  {
    get => this.m_usTopLeft;
    set => this.m_usTopLeft = value;
  }

  public ushort BottomRight
  {
    get => this.m_usBottomRight;
    set => this.m_usBottomRight = value;
  }

  public int X1
  {
    get => this.m_iX1;
    set => this.m_iX1 = value;
  }

  public int Y1
  {
    get => this.m_iY1;
    set => this.m_iY1 = value;
  }

  public int X2
  {
    get => this.m_iX2;
    set => this.m_iX2 = value;
  }

  public int Y2
  {
    get => this.m_iY2;
    set => this.m_iY2 = value;
  }

  public override int MinimumRecordSize => 20;

  public override int MaximumRecordSize => 20;

  public ChartPosRecord()
  {
  }

  public ChartPosRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartPosRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usTopLeft = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usBottomRight = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_iX1 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iY1 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iX2 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iY2 = provider.ReadInt32(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usTopLeft);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usBottomRight);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iX1);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iY1);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iX2);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iY2);
  }

  public override int GetStoreSize(ExcelVersion version) => 20;
}
