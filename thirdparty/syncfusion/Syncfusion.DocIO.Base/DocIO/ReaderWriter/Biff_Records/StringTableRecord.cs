// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.StringTableRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class StringTableRecord : BaseWordRecord
{
  internal StringTableRecord()
  {
  }

  internal StringTableRecord(byte[] data)
    : base(data)
  {
  }

  internal StringTableRecord(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }

  internal StringTableRecord(byte[] arrData, int iOffset, int iCount)
    : base(arrData, iOffset, iCount)
  {
  }

  internal StringTableRecord(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    base.Parse(arrData, iOffset, iCount);
  }
}
