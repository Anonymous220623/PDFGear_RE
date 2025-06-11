// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartSurfaceRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartSurface)]
[CLSCompliant(false)]
public class ChartSurfaceRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bFillSurface;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_b3DPhongShade;

  public ushort Options => this.m_usOptions;

  public bool IsFillSurface
  {
    get => this.m_bFillSurface;
    set => this.m_bFillSurface = value;
  }

  public bool Is3DPhongShade
  {
    get => this.m_b3DPhongShade;
    set => this.m_b3DPhongShade = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public ChartSurfaceRecord()
  {
  }

  public ChartSurfaceRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartSurfaceRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bFillSurface = provider.ReadBit(iOffset, 0);
    this.m_b3DPhongShade = provider.ReadBit(iOffset, 1);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bFillSurface, 0);
    provider.WriteBit(iOffset, this.m_b3DPhongShade, 1);
    this.m_iLength = 2;
  }
}
