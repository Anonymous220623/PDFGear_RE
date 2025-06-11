// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BeginRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.Begin)]
[CLSCompliant(false)]
public class BeginRecord : BiffRecordRawWithArray
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
    ExcelVersion version)
  {
  }

  public override void ParseStructure()
  {
  }

  public override void InfillInternalData(ExcelVersion version) => this.m_iLength = 0;
}
