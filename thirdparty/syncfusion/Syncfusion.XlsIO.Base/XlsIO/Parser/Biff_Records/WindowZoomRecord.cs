// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.WindowZoomRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.WindowZoom)]
[CLSCompliant(false)]
public class WindowZoomRecord : BiffRecordRaw
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
    ExcelVersion version)
  {
    this.m_usNscl = provider.ReadUInt16(iOffset);
    this.m_usDscl = provider.ReadUInt16(iOffset + 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 4;
    provider.WriteUInt16(iOffset, this.m_usNscl);
    provider.WriteUInt16(iOffset + 2, this.m_usDscl);
  }

  public override int GetStoreSize(ExcelVersion version) => 4;
}
