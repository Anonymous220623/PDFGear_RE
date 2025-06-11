// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BlankRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Blank)]
[CLSCompliant(false)]
internal class BlankRecord : CellPositionBase
{
  private const int DEF_RECORD_SIZE = 6;
  internal const int DEF_RECORD_SIZE_WITH_HEADER = 10;

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 6;

  protected override void ParseCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 6;
    if (version != OfficeVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }
}
