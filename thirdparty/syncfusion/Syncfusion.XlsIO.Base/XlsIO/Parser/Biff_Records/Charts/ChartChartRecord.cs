// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartChartRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartChart)]
[CLSCompliant(false)]
public class ChartChartRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 16 /*0x10*/;
  [BiffRecordPos(0, 4, true)]
  private int m_iTopLeftX;
  [BiffRecordPos(4, 4, true)]
  private int m_iTopLeftY;
  [BiffRecordPos(8, 4, true)]
  private int m_iWidth;
  [BiffRecordPos(12, 4, true)]
  private int m_iHeight;

  public int X
  {
    get => this.m_iTopLeftX;
    set
    {
      if (value == this.m_iTopLeftX)
        return;
      this.m_iTopLeftX = value;
    }
  }

  public int Y
  {
    get => this.m_iTopLeftY;
    set
    {
      if (value == this.m_iTopLeftY)
        return;
      this.m_iTopLeftY = value;
    }
  }

  public int Width
  {
    get => this.m_iWidth;
    set
    {
      if (value == this.m_iWidth)
        return;
      this.m_iWidth = value;
    }
  }

  public int Height
  {
    get => this.m_iHeight;
    set
    {
      if (value == this.m_iHeight)
        return;
      this.m_iHeight = value;
    }
  }

  public override int MinimumRecordSize => 16 /*0x10*/;

  public override int MaximumRecordSize => 16 /*0x10*/;

  public ChartChartRecord()
  {
  }

  public ChartChartRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartChartRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iTopLeftX = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iTopLeftY = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iWidth = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iHeight = provider.ReadInt32(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteInt32(iOffset, this.m_iTopLeftX);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iTopLeftY);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iWidth);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iHeight);
  }

  public override int GetStoreSize(ExcelVersion version) => 16 /*0x10*/;
}
