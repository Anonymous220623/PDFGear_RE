// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BlankRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Blank)]
public class BlankRecord : CellPositionBase
{
  private const int DEF_RECORD_SIZE = 6;
  internal const int DEF_RECORD_SIZE_WITH_HEADER = 10;

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 6;

  protected override void ParseCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 6;
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }
}
