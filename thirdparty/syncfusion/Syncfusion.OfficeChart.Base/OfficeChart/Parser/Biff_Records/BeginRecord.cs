// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BeginRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Begin)]
internal class BeginRecord : BiffRecordRawWithArray
{
  public BeginRecord()
  {
  }

  public BeginRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public BeginRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override int MaximumRecordSize => 0;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
  }

  public override void ParseStructure()
  {
  }

  public override void InfillInternalData(OfficeVersion version) => this.m_iLength = 0;
}
