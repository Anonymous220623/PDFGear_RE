// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartAxisParentRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartAxisParent)]
internal class ChartAxisParentRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 18;
  [BiffRecordPos(0, 2)]
  private ushort m_usAxisIndex;
  [BiffRecordPos(2, 4, true)]
  private int m_iTopLeftX;
  [BiffRecordPos(6, 4, true)]
  private int m_iTopLeftY;
  [BiffRecordPos(10, 4, true)]
  private int m_iXLength;
  [BiffRecordPos(14, 4, true)]
  private int m_iYLength;

  public ushort AxesIndex
  {
    get => this.m_usAxisIndex;
    set
    {
      if ((int) value == (int) this.m_usAxisIndex)
        return;
      this.m_usAxisIndex = value;
    }
  }

  public int TopLeftX
  {
    get => this.m_iTopLeftX;
    set
    {
      if (value == this.m_iTopLeftX)
        return;
      this.m_iTopLeftX = value;
    }
  }

  public int TopLeftY
  {
    get => this.m_iTopLeftY;
    set
    {
      if (value == this.m_iTopLeftY)
        return;
      this.m_iTopLeftY = value;
    }
  }

  public int XAxisLength
  {
    get => this.m_iXLength;
    set
    {
      if (value == this.m_iXLength)
        return;
      this.m_iXLength = value;
    }
  }

  public int YAxisLength
  {
    get => this.m_iYLength;
    set
    {
      if (value == this.m_iYLength)
        return;
      this.m_iYLength = value;
    }
  }

  public ChartAxisParentRecord()
  {
  }

  public ChartAxisParentRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAxisParentRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usAxisIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_iTopLeftX = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iTopLeftY = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iXLength = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iYLength = provider.ReadInt32(iOffset);
    iOffset += 4;
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usAxisIndex);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iTopLeftX);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iTopLeftY);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iXLength);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iYLength);
    iOffset += 4;
  }

  public override int GetStoreSize(OfficeVersion version) => 18;
}
