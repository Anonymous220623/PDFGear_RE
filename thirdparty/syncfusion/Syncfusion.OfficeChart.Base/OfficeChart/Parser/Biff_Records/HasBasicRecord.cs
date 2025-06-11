// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.HasBasicRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.HasBasic)]
internal class HasBasicRecord : BiffRecordRaw
{
  public override int MaximumRecordSize => 0;

  public HasBasicRecord()
  {
  }

  public HasBasicRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public HasBasicRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = 0;
  }
}
