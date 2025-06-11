// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MulRKRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.MulRK)]
public class MulRKRecord : CellPositionBase, IMultiCellRecord, ICellPositionFormat
{
  public const int DEF_FIXED_SIZE = 6;
  public const int DEF_SUB_ITEM_SIZE = 6;
  private List<MulRKRecord.RkRec> m_arrRKs;
  private int m_iLastCol;

  public int FirstColumn
  {
    get => this.m_iColumn;
    set => this.m_iColumn = value;
  }

  public int LastColumn
  {
    get => this.m_iLastCol;
    set => this.m_iLastCol = value;
  }

  public List<MulRKRecord.RkRec> Records
  {
    get => this.m_arrRKs;
    set => this.m_arrRKs = value;
  }

  public override int MinimumRecordSize => 6;

  protected override void ParseCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    iOffset -= 2;
    int num1 = this.Length - 6;
    if (version != ExcelVersion.Excel97to2003)
      num1 -= 6;
    int capacity = num1 / 6;
    this.m_arrRKs = new List<MulRKRecord.RkRec>(capacity);
    if (this.Length % 6 != 0)
      throw new WrongBiffRecordDataException();
    int iOffset1 = iOffset;
    int num2 = 0;
    while (num2 < capacity)
    {
      this.m_arrRKs.Add(new MulRKRecord.RkRec(provider.ReadUInt16(iOffset1), provider.ReadInt32(iOffset1 + 2)));
      ++num2;
      iOffset1 += 6;
    }
    if (version == ExcelVersion.Excel97to2003)
      this.m_iLastCol = (int) provider.ReadUInt16(iOffset1);
    else
      this.m_iLastCol = provider.ReadInt32(iOffset1);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    iOffset -= 2;
    int index = 0;
    int count = this.m_arrRKs.Count;
    while (index < count)
    {
      MulRKRecord.RkRec arrRk = this.m_arrRKs[index];
      provider.WriteUInt16(iOffset, arrRk.ExtFormatIndex);
      provider.WriteInt32(iOffset + 2, arrRk.Rk);
      ++index;
      iOffset += 6;
    }
    if (version == ExcelVersion.Excel97to2003)
      provider.WriteUInt16(iOffset, (ushort) this.m_iLastCol);
    else
      provider.WriteInt32(iOffset, this.m_iLastCol);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = this.m_arrRKs.Count * 6 + 6;
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 6;
    return storeSize;
  }

  public int GetSeparateSubRecordSize(ExcelVersion version)
  {
    int separateSubRecordSize = 14;
    if (version != ExcelVersion.Excel97to2003)
      separateSubRecordSize += 4;
    return separateSubRecordSize;
  }

  public int SubRecordSize => 6;

  public TBIFFRecord SubRecordType => TBIFFRecord.RK;

  public void Insert(ICellPositionFormat cell)
  {
    if (cell.TypeCode == this.TypeCode)
      this.MergeRecords((MulRKRecord) cell);
    else
      this.InsertSubRecord(cell);
  }

  private void MergeRecords(MulRKRecord mulRK)
  {
    if (mulRK == null)
      throw new ArgumentNullException(nameof (mulRK));
    if (mulRK.Row != this.m_iRow)
      throw new ArgumentOutOfRangeException("Row", "Rows should be equal for both MulRK records.");
    if (mulRK.FirstColumn == this.LastColumn + 1)
    {
      this.m_iLastCol = mulRK.LastColumn;
      this.m_arrRKs.AddRange((IEnumerable<MulRKRecord.RkRec>) mulRK.m_arrRKs);
    }
    else
    {
      if (mulRK.LastColumn + 1 != this.FirstColumn)
        throw new ArgumentException("Two MulRK records doesn't correspond each other.");
      this.m_iColumn = mulRK.m_iColumn;
      this.m_arrRKs.InsertRange(0, (IEnumerable<MulRKRecord.RkRec>) mulRK.m_arrRKs);
    }
  }

  public void InsertSubRecord(ICellPositionFormat cell)
  {
    int num = cell.TypeCode == TBIFFRecord.RK ? cell.Column : throw new ArgumentOutOfRangeException("cell.TypeCode");
    int row = cell.Row;
    ushort extendedFormatIndex = cell.ExtendedFormatIndex;
    bool flag = this.m_arrRKs == null;
    if (flag || this.m_arrRKs.Count == 0)
    {
      if (flag)
        this.m_arrRKs = new List<MulRKRecord.RkRec>();
      this.m_arrRKs.Add(this.CreateSubRecord((RKRecord) cell));
      this.m_iRow = cell.Row;
      this.m_iColumn = this.m_iLastCol = cell.Column;
    }
    else
    {
      if (this.Row != row)
        throw new ArgumentOutOfRangeException("Row");
      if (this.m_iColumn <= num && this.m_iLastCol >= num)
      {
        RKRecord rkRecord = (RKRecord) cell;
        MulRKRecord.RkRec arrRk = this.m_arrRKs[num - this.m_iColumn];
        arrRk.ExtFormatIndex = extendedFormatIndex;
        arrRk.Rk = rkRecord.RKNumberInt;
      }
      else if (num == this.m_iColumn - 1)
      {
        this.m_arrRKs.Insert(0, this.CreateSubRecord((RKRecord) cell));
        --this.m_iColumn;
      }
      else
      {
        if (num != this.m_iLastCol + 1)
          throw new ArgumentOutOfRangeException("cell.Column");
        this.m_arrRKs.Add(this.CreateSubRecord((RKRecord) cell));
        ++this.m_iLastCol;
      }
    }
  }

  private MulRKRecord.RkRec CreateSubRecord(RKRecord rk)
  {
    if (rk == null)
      throw new ArgumentNullException(nameof (rk));
    return new MulRKRecord.RkRec(rk.ExtendedFormatIndex, rk.RKNumberInt);
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
      return this.CreateRkRecord(iFirstCol);
    MulRKRecord record = (MulRKRecord) BiffRecordFactory.GetRecord(TBIFFRecord.MulRK);
    record.m_iColumn = iFirstCol;
    record.m_iLastCol = iLastCol;
    record.m_iRow = this.m_iRow;
    int capacity = iLastCol - iFirstCol + 1;
    List<MulRKRecord.RkRec> rkRecList = new List<MulRKRecord.RkRec>(capacity);
    record.m_arrRKs = rkRecList;
    int index1 = 0;
    int index2 = iFirstCol - this.m_iColumn;
    while (index1 < capacity)
    {
      rkRecList[index1] = this.m_arrRKs[index2];
      ++index1;
      ++index2;
    }
    return (ICellPositionFormat) record;
  }

  private ICellPositionFormat CreateRkRecord(int iColumnIndex)
  {
    RKRecord record = (RKRecord) BiffRecordFactory.GetRecord(TBIFFRecord.RK);
    MulRKRecord.RkRec arrRk = this.m_arrRKs[iColumnIndex - this.m_iColumn];
    record.ExtendedFormatIndex = arrRk.ExtFormatIndex;
    record.RKNumberInt = arrRk.Rk;
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
      ICellPositionFormat rkRecord = this.CreateRkRecord(iColumn);
      biffRecordRawArray[index] = (BiffRecordRaw) rkRecord;
      ++iColumn;
      ++index;
    }
    return biffRecordRawArray;
  }

  [CLSCompliant(false)]
  public class RkRec
  {
    private ushort m_usExtFormatIndex;
    private int m_iRk;

    private RkRec()
    {
    }

    public RkRec(ushort xf, int rk)
    {
      this.m_usExtFormatIndex = xf;
      this.m_iRk = rk;
    }

    public ushort ExtFormatIndex
    {
      get => this.m_usExtFormatIndex;
      set => this.m_usExtFormatIndex = value;
    }

    public int Rk
    {
      get => this.m_iRk;
      set => this.m_iRk = value;
    }

    public double RkNumber
    {
      get
      {
        bool flag1 = (this.m_iRk & 2) == 2;
        bool flag2 = (this.m_iRk & 1) == 1;
        long num1 = (long) (this.m_iRk >> 2);
        if (flag1)
        {
          double num2 = (double) num1;
          return !flag2 ? num2 : num2 / 100.0;
        }
        double num3 = BitConverterGeneral.Int64BitsToDouble(num1 << 34);
        return !flag2 ? num3 : num3 / 100.0;
      }
    }
  }
}
