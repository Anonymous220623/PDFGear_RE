// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.UnkMacrosDisable
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.UnkMacrosDisable)]
public class UnkMacrosDisable : BiffRecordRaw
{
  public override int MaximumRecordSize => 0;

  public UnkMacrosDisable()
  {
  }

  public UnkMacrosDisable(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public UnkMacrosDisable(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 0;
  }
}
