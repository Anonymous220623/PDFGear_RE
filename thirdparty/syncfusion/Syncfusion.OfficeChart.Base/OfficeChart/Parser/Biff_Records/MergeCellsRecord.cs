// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MergeCellsRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.MergeCells)]
[CLSCompliant(false)]
internal class MergeCellsRecord : BiffRecordRaw
{
  public const int DEF_MAXIMUM_REGIONS = 1027;
  private const int DEF_FIXED_SIZE = 2;
  private const int DEF_SUB_ITEM_SIZE = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usNumber;
  private MergeCellsRecord.MergedRegion[] m_arrRegions;

  public ushort RangesNumber => this.m_usNumber;

  public MergeCellsRecord.MergedRegion[] Regions
  {
    get => this.m_arrRegions;
    set
    {
      this.m_arrRegions = value;
      this.m_usNumber = (ushort) this.m_arrRegions.Length;
    }
  }

  public override int MinimumRecordSize => 2;

  public MergeCellsRecord()
  {
  }

  public MergeCellsRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public MergeCellsRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usNumber = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_arrRegions = new MergeCellsRecord.MergedRegion[(int) this.m_usNumber];
    this.InternalDataIntegrityCheck();
    int index = 0;
    while (index < (int) this.m_usNumber)
    {
      this.m_arrRegions[index] = new MergeCellsRecord.MergedRegion((int) provider.ReadUInt16(iOffset), (int) provider.ReadUInt16(iOffset + 2), (int) provider.ReadUInt16(iOffset + 4), (int) provider.ReadUInt16(iOffset + 6));
      ++index;
      iOffset += 8;
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usNumber);
    this.m_iLength = this.GetStoreSize(version);
    iOffset += 2;
    int index = 0;
    while (index < (int) this.m_usNumber)
    {
      provider.WriteUInt16(iOffset, (ushort) this.m_arrRegions[index].RowFrom);
      provider.WriteUInt16(iOffset + 2, (ushort) this.m_arrRegions[index].RowTo);
      provider.WriteUInt16(iOffset + 4, (ushort) this.m_arrRegions[index].ColumnFrom);
      provider.WriteUInt16(iOffset + 6, (ushort) this.m_arrRegions[index].ColumnTo);
      ++index;
      iOffset += 8;
    }
  }

  private void InternalDataIntegrityCheck()
  {
    if (this.m_iLength != (int) this.m_usNumber * 8 + 2 || (this.m_iLength - 2) % 8 != 0)
      throw new WrongBiffRecordDataException(nameof (MergeCellsRecord));
  }

  public override int GetStoreSize(OfficeVersion version) => 2 + this.m_arrRegions.Length * 8;

  public void SetRegions(int iStartIndex, int iCount, MergeCellsRecord.MergedRegion[] arrRegions)
  {
    int num = arrRegions != null ? arrRegions.Length : throw new ArgumentNullException(nameof (arrRegions));
    if (iStartIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex));
    if (iCount < 0 || iStartIndex + iCount > num)
      throw new ArgumentOutOfRangeException("iRegionsCount");
    if ((int) this.m_usNumber != iCount)
    {
      this.m_arrRegions = new MergeCellsRecord.MergedRegion[iCount];
      this.m_usNumber = (ushort) iCount;
    }
    Array.Copy((Array) arrRegions, iStartIndex, (Array) this.m_arrRegions, 0, iCount);
  }

  [CLSCompliant(false)]
  public class MergedRegion : ICloneable
  {
    private int m_iRowFrom;
    private int m_iRowTo;
    private int m_iColFrom;
    private int m_iColTo;

    private MergedRegion()
    {
    }

    public MergedRegion(MergeCellsRecord.MergedRegion region)
      : this(region.RowFrom, region.RowTo, region.ColumnFrom, region.ColumnTo)
    {
    }

    public MergedRegion(int rowFrom, int rowTo, int colFrom, int colTo)
    {
      this.m_iRowFrom = rowFrom;
      this.m_iRowTo = rowTo;
      this.m_iColFrom = colFrom;
      this.m_iColTo = colTo;
    }

    public int RowFrom
    {
      get => this.m_iRowFrom;
      set => this.m_iRowFrom = value;
    }

    public int RowTo
    {
      get => this.m_iRowTo;
      set => this.m_iRowTo = value;
    }

    public int ColumnFrom => this.m_iColFrom;

    public int ColumnTo
    {
      get => this.m_iColTo;
      set => this.m_iColTo = value;
    }

    public int CellsCount
    {
      get => (this.m_iRowTo - this.m_iRowFrom + 1) * (this.m_iColTo - this.m_iColFrom + 1);
    }

    public void MoveRegion(int iRowDelta, int iColDelta)
    {
      this.m_iRowTo += iRowDelta;
      this.m_iRowFrom += iRowDelta;
      this.m_iColFrom += iColDelta;
      this.m_iColTo += iColDelta;
    }

    internal Rectangle GetRectangle()
    {
      return Rectangle.FromLTRB(this.m_iColFrom, this.m_iRowFrom, this.m_iColTo, this.m_iRowTo);
    }

    public object Clone() => this.MemberwiseClone();

    public static bool Equals(
      MergeCellsRecord.MergedRegion region1,
      MergeCellsRecord.MergedRegion region2)
    {
      if (region1 == null && region2 == null)
        return true;
      return region1 != null && region2 != null && region1.Equals((object) region2);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (!(obj is MergeCellsRecord.MergedRegion mergedRegion))
        throw new ArgumentException(nameof (obj));
      return this.m_iColFrom == mergedRegion.m_iColFrom && this.m_iColTo == mergedRegion.m_iColTo && this.m_iRowFrom == mergedRegion.m_iRowFrom && this.m_iRowTo == mergedRegion.m_iRowTo;
    }

    public override int GetHashCode()
    {
      return this.m_iColFrom.GetHashCode() | this.m_iColTo.GetHashCode() | this.m_iRowTo.GetHashCode() | this.m_iRowFrom.GetHashCode();
    }
  }
}
