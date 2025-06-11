// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.InterfaceEndRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.InterfaceEnd)]
[CLSCompliant(false)]
public class InterfaceEndRecord : BiffRecordRawWithArray
{
  public override int MinimumRecordSize => 0;

  public override int MaximumRecordSize => 0;

  public InterfaceEndRecord()
  {
  }

  public InterfaceEndRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public InterfaceEndRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure() => this.AutoExtractFields();

  public override void InfillInternalData(ExcelVersion version) => this.m_iLength = 0;
}
