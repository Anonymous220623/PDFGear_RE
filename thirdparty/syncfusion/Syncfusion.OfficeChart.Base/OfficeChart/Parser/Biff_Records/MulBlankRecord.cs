// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MulBlankRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.MulBlank)]
[CLSCompliant(false)]
internal class MulBlankRecord : CellPositionBase, IMultiCellRecord, ICellPositionFormat
{
  public const int DEF_FIXED_SIZE = 6;
  private const int DEF_MINIMUM_SIZE = 6;
  public const int DEF_SUB_ITEM_SIZE = 2;
  private List<ushort> m_arrExtFormatIndexes;
  private int m_iLastCol;

  public int FirstColumn
  {
    get => this.m_iColumn;
    set => this.m_iColumn = value;
  }

  public List<ushort> ExtendedFormatIndexes
  {
    get => this.m_arrExtFormatIndexes;
    set
    {
      this.m_arrExtFormatIndexes = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public int LastColumn
  {
    get => this.m_iLastCol;
    set => this.m_iLastCol = value;
  }

  public override int MinimumRecordSize => 6;

  protected override void ParseCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    iOffset -= 2;
    if (this.m_iLength % 2 != 0)
      throw new WrongBiffRecordDataException("( Length - 6 ) % 2 != 0");
    int num = this.m_iLength - 6;
    if (version != OfficeVersion.Excel97to2003)
      num -= 6;
    int capacity = num / 2;
    this.m_arrExtFormatIndexes = new List<ushort>(capacity);
    for (int index = 0; index < capacity; ++index)
    {
      this.m_arrExtFormatIndexes.Add(provider.ReadUInt16(iOffset));
      iOffset += 2;
    }
    this.m_iLastCol = version != OfficeVersion.Excel97to2003 ? provider.ReadInt32(iOffset) : (int) provider.ReadUInt16(iOffset);
    this.InternalDataIntegrityCheck();
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    iOffset -= 2;
    int count = this.m_arrExtFormatIndexes.Count;
    for (int index = 0; index < count; ++index)
    {
      provider.WriteUInt16(iOffset, this.m_arrExtFormatIndexes[index]);
      iOffset += 2;
    }
    provider.WriteUInt16(iOffset, (ushort) this.m_iLastCol);
  }

  private void InternalDataIntegrityCheck()
  {
    if (this.m_iLastCol - this.m_iColumn + 1 != this.m_arrExtFormatIndexes.Count)
      throw new WrongBiffRecordDataException("m_usLastCol - m_usFirstCol + 1 != m_arrExtFormatIndexes.Length");
  }

  public BlankRecord GetBlankRecord(int iColumnIndex)
  {
    if (iColumnIndex < this.m_iColumn || iColumnIndex > this.m_iLastCol)
      throw new ArgumentOutOfRangeException(nameof (iColumnIndex), "Value cannot be less m_usFirstCol and greater than m_usLastCol");
    ushort arrExtFormatIndex = this.m_arrExtFormatIndexes[iColumnIndex - this.m_iColumn];
    BlankRecord record = (BlankRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Blank);
    record.Row = this.m_iRow;
    record.Column = iColumnIndex;
    record.ExtendedFormatIndex = arrExtFormatIndex;
    return record;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = this.m_arrExtFormatIndexes.Count * 2 + 6;
    if (version != OfficeVersion.Excel97to2003)
      storeSize += 6;
    return storeSize;
  }

  public static void IncreaseLastColumn(
    DataProvider provider,
    int recordStart,
    int iLength,
    OfficeVersion version,
    int columnDelta)
  {
    int num1 = recordStart + iLength + 4;
    switch (version)
    {
      case OfficeVersion.Excel97to2003:
        int iOffset1 = num1 - 2;
        int num2 = (int) provider.ReadInt16(iOffset1) + columnDelta;
        provider.WriteInt16(iOffset1, (short) num2);
        break;
      case OfficeVersion.Excel2007:
      case OfficeVersion.Excel2010:
      case OfficeVersion.Excel2013:
        int iOffset2 = num1 - 4;
        int num3 = provider.ReadInt32(iOffset2) + columnDelta;
        provider.WriteInt32(iOffset2, (int) (short) num3);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (version));
    }
  }

  public int GetSeparateSubRecordSize(OfficeVersion version)
  {
    int separateSubRecordSize = 10;
    if (version != OfficeVersion.Excel97to2003)
      separateSubRecordSize += 4;
    return separateSubRecordSize;
  }

  public int SubRecordSize => 2;

  public TBIFFRecord SubRecordType => TBIFFRecord.Blank;

  public void Insert(ICellPositionFormat cell)
  {
    int column = cell.Column;
    int row = cell.Row;
    ushort extendedFormatIndex = cell.ExtendedFormatIndex;
    if (this.Row != row || this.m_iColumn > column || this.m_iLastCol < column)
      throw new ArgumentOutOfRangeException("cell.Column");
    this.m_arrExtFormatIndexes[column - this.m_iColumn] = extendedFormatIndex;
  }

  public ICellPositionFormat[] Split(int iColumnIndex)
  {
    if (iColumnIndex < this.m_iColumn || iColumnIndex > this.m_iLastCol)
      return new ICellPositionFormat[1]
      {
        (ICellPositionFormat) this
      };
    int iColumn = this.m_iColumn;
    return new ICellPositionFormat[2]
    {
      this.CreateRecord(this.m_iColumn, iColumnIndex - 1),
      this.CreateRecord(iColumnIndex + 1, this.m_iLastCol)
    };
  }

  private ICellPositionFormat CreateRecord(int iFirstCol, int iLastCol)
  {
    if (iFirstCol > iLastCol)
      return (ICellPositionFormat) null;
    if (iFirstCol == iLastCol)
      return this.CreateBlankRecord(iFirstCol);
    MulBlankRecord record = (MulBlankRecord) BiffRecordFactory.GetRecord(TBIFFRecord.MulBlank);
    record.m_iColumn = iFirstCol;
    record.m_iLastCol = iLastCol;
    record.m_iRow = this.m_iRow;
    int capacity = iLastCol - iFirstCol + 1;
    List<ushort> ushortList = new List<ushort>(capacity);
    record.m_arrExtFormatIndexes = ushortList;
    int index1 = 0;
    int index2 = iFirstCol - this.m_iColumn;
    while (index1 < capacity)
    {
      ushortList[index1] = this.m_arrExtFormatIndexes[index2];
      ++index1;
      ++index2;
    }
    return (ICellPositionFormat) record;
  }

  private ICellPositionFormat CreateBlankRecord(int iColumnIndex)
  {
    BlankRecord record = (BlankRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Blank);
    record.ExtendedFormatIndex = this.m_arrExtFormatIndexes[iColumnIndex - this.m_iColumn];
    record.Row = this.Row;
    record.Column = iColumnIndex;
    return (ICellPositionFormat) record;
  }

  public BiffRecordRaw[] Split(bool bIgnoreStyles)
  {
    BiffRecordRaw[] biffRecordRawArray = new BiffRecordRaw[this.m_iLastCol - this.m_iColumn + 1];
    int iColumn = this.m_iColumn;
    int index = 0;
    while (iColumn <= this.m_iLastCol)
    {
      ICellPositionFormat blankRecord = this.CreateBlankRecord(iColumn);
      biffRecordRawArray[index] = (BiffRecordRaw) blankRecord;
      ++iColumn;
      ++index;
    }
    return biffRecordRawArray;
  }
}
