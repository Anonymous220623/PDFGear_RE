// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.LabelSSTRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.LabelSST)]
[CLSCompliant(false)]
public class LabelSSTRecord : CellPositionBase, ICloneable
{
  private const int DEF_RECORD_SIZE = 10;
  internal const int DEF_INDEX_OFFSET = 6;
  [BiffRecordPos(6, 4, true)]
  private int m_iSSTIndex;

  public int SSTIndex
  {
    get => this.m_iSSTIndex;
    set => this.m_iSSTIndex = value;
  }

  public override int MinimumRecordSize => 10;

  public override int MaximumRecordSize => 10;

  public override int MaximumMemorySize => 10;

  protected override void ParseCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iSSTIndex = provider.ReadInt32(iOffset);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteInt32(iOffset, this.m_iSSTIndex);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 10;
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  public static void SetSSTIndex(
    DataProvider provider,
    int iOffset,
    int iNewIndex,
    ExcelVersion version)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    iOffset += 10;
    if (version != ExcelVersion.Excel97to2003)
      iOffset += 4;
    provider.WriteInt32(iOffset, iNewIndex);
  }

  public static int GetSSTIndex(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    iOffset += 10;
    if (version != ExcelVersion.Excel97to2003)
      iOffset += 4;
    return provider.ReadInt32(iOffset);
  }
}
