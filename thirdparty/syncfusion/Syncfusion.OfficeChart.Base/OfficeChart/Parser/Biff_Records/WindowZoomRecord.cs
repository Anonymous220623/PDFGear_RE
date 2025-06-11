// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.WindowZoomRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.WindowZoom)]
[CLSCompliant(false)]
internal class WindowZoomRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usNscl = 100;
  [BiffRecordPos(2, 2)]
  private ushort m_usDscl = 100;

  public ushort NumMagnification
  {
    get => this.m_usNscl;
    set => this.m_usNscl = value;
  }

  public ushort DenumMagnification
  {
    get => this.m_usDscl;
    set => this.m_usDscl = value;
  }

  public int Zoom
  {
    get => (int) ((double) this.m_usNscl * 100.0 / (double) this.m_usDscl);
    set
    {
      this.m_usNscl = value >= 10 && value <= 400 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (Zoom), "Zoom must be in range from 10 and 400.");
      this.m_usDscl = (ushort) 100;
    }
  }

  public WindowZoomRecord()
  {
  }

  public WindowZoomRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public WindowZoomRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usNscl = provider.ReadUInt16(iOffset);
    this.m_usDscl = provider.ReadUInt16(iOffset + 2);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = 4;
    provider.WriteUInt16(iOffset, this.m_usNscl);
    provider.WriteUInt16(iOffset + 2, this.m_usDscl);
  }

  public override int GetStoreSize(OfficeVersion version) => 4;
}
