// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotDoubleRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotDouble)]
public class PivotDoubleRecord : BiffRecordRaw, IValueHolder
{
  private int DefaultRecordSize = 8;
  [BiffRecordPos(0, 8, TFieldType.Float)]
  private double m_dValue;

  public PivotDoubleRecord()
  {
  }

  public PivotDoubleRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotDoubleRecord(int iReserve)
    : base(iReserve)
  {
  }

  public double Value
  {
    get => this.m_dValue;
    set => this.m_dValue = value;
  }

  public override int MinimumRecordSize => this.DefaultRecordSize;

  public override int MaximumRecordSize => this.DefaultRecordSize;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_dValue = provider.ReadDouble(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteDouble(iOffset, this.m_dValue);
    this.m_iLength = this.DefaultRecordSize;
  }

  public override int GetStoreSize(ExcelVersion version) => this.DefaultRecordSize;

  object IValueHolder.Value
  {
    get => (object) this.Value;
    set => this.Value = (double) value;
  }
}
