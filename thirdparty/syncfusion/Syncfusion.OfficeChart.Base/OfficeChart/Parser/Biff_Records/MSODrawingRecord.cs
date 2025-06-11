// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MSODrawingRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.MSODrawing)]
internal class MSODrawingRecord : BiffRecordRawWithArray, ICloneable, ILengthSetter
{
  public int RecordLength
  {
    get => this.Length;
    set
    {
      if (value < 0 && value > this.m_data.Length)
        throw new ArgumentOutOfRangeException(nameof (RecordLength));
      this.m_iLength = value;
    }
  }

  public MSODrawingRecord()
  {
  }

  public MSODrawingRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public MSODrawingRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.m_iLength = this.m_data.Length;
  }

  public override bool NeedDataArray => true;

  public void SetData(int length, byte[] data)
  {
    if (length < 0 || data.Length < length)
      throw new ArgumentOutOfRangeException(nameof (length));
    this.m_data = new byte[length];
    Array.Copy((Array) data, 0, (Array) this.m_data, 0, length);
  }

  public void SetLength(int iLength) => this.m_iLength = iLength;

  public override int GetStoreSize(OfficeVersion version) => this.m_data.Length;
}
