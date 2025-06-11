// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.LabelSSTRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.LabelSST)]
internal class LabelSSTRecord : CellPositionBase, ICloneable
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

  protected override void ParseCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_iSSTIndex = provider.ReadInt32(iOffset);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    provider.WriteInt32(iOffset, this.m_iSSTIndex);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 10;
    if (version != OfficeVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  public static void SetSSTIndex(
    DataProvider provider,
    int iOffset,
    int iNewIndex,
    OfficeVersion version)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    iOffset += 10;
    if (version != OfficeVersion.Excel97to2003)
      iOffset += 4;
    provider.WriteInt32(iOffset, iNewIndex);
  }

  public static int GetSSTIndex(DataProvider provider, int iOffset, OfficeVersion version)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    iOffset += 10;
    if (version != OfficeVersion.Excel97to2003)
      iOffset += 4;
    return provider.ReadInt32(iOffset);
  }
}
