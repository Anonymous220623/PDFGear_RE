// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.DConNameRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.DCONNAME)]
public class DConNameRecord : DConBinRecord
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
