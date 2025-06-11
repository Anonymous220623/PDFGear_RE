// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.NumberRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Number)]
public class NumberRecord : CellPositionBase, IDoubleValue, IValueHolder
{
  private const int DEF_RECORD_SIZE = 14;
  [BiffRecordPos(6, 8, TFieldType.Float)]
  private double m_dbValue;

  public double Value
  {
    get => this.m_dbValue;
    set => this.m_dbValue = value;
  }

  public override int MinimumRecordSize => 14;

  public override int MaximumRecordSize => 14;

  public override int MaximumMemorySize => 14;

  public double DoubleValue => this.m_dbValue;

  protected override void ParseCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_dbValue = provider.ReadDouble(iOffset);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteDouble(iOffset, this.m_dbValue);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 14;
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  public static double ReadValue(DataProvider provider, int recordStart, ExcelVersion version)
  {
    recordStart += 10;
    if (version != ExcelVersion.Excel97to2003)
      recordStart += 4;
    return provider.ReadDouble(recordStart);
  }

  object IValueHolder.Value
  {
    get => (object) this.m_dbValue;
    set => this.m_dbValue = (double) value;
  }
}
