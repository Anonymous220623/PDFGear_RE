// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.FilterModeRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.FilterMode)]
public class FilterModeRecord : BiffRecordRawWithArray
{
  public override int MaximumRecordSize => 0;

  public FilterModeRecord()
  {
  }

  public FilterModeRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public FilterModeRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
  }

  public override void InfillInternalData(ExcelVersion version) => this.m_iLength = 0;
}
