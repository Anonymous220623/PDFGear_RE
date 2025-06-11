// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.NumberRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Number)]
internal class NumberRecord : CellPositionBase, IDoubleValue, IValueHolder
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

  protected override void ParseCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_dbValue = provider.ReadDouble(iOffset);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    provider.WriteDouble(iOffset, this.m_dbValue);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 14;
    if (version != OfficeVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  public static double ReadValue(DataProvider provider, int recordStart, OfficeVersion version)
  {
    recordStart += 10;
    if (version != OfficeVersion.Excel97to2003)
      recordStart += 4;
    return provider.ReadDouble(recordStart);
  }

  object IValueHolder.Value
  {
    get => (object) this.m_dbValue;
    set => this.m_dbValue = (double) value;
  }
}
