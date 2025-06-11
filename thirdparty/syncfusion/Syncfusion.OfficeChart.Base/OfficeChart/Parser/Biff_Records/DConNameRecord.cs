// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DConNameRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.DCONNAME)]
[CLSCompliant(false)]
internal class DConNameRecord : DConBinRecord
{
  public DConNameRecord()
  {
  }

  public DConNameRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DConNameRecord(int iReserve)
    : base(iReserve)
  {
  }
}
