// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotEmptyRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotEmpty)]
public class PivotEmptyRecord : BiffRecordRaw, IValueHolder
{
  private const int DefaultRecordSize = 0;

  public PivotEmptyRecord()
  {
  }

  public PivotEmptyRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotEmptyRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override int MinimumRecordSize => 0;

  public override int MaximumRecordSize => 0;

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

  public override int GetStoreSize(ExcelVersion version) => 0;

  object IValueHolder.Value
  {
    get => (object) null;
    set
    {
    }
  }
}
