// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ArrayRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Array)]
public class ArrayRecord : BiffRecordRaw, ISharedFormula, ICloneable, IFormulaRecord
{
  private const int DEF_RECORD_MIN_SIZE = 14;
  private const int DEF_FORMULA_OFFSET = 14;
  [BiffRecordPos(0, 2)]
  private int m_iFirstRow;
  [BiffRecordPos(2, 2)]
  private int m_iLastRow;
  [BiffRecordPos(4, 1)]
  private int m_iFirstColumn;
  [BiffRecordPos(5, 1)]
  private int m_iLastColumn;
  [BiffRecordPos(6, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(6, 0, TFieldType.Bit)]
  private bool m_bRecalculateAlways;
  [BiffRecordPos(6, 1, TFieldType.Bit)]
  private bool m_bRecalculateOnOpen;
  [BiffRecordPos(8, 4, true)]
  private int m_iReserved;
  [BiffRecordPos(12, 2)]
  private ushort m_usExpressionLength;
  private byte[] m_arrExpression;
  private Ptg[] m_arrFormula;

  public int FirstRow
  {
    get => this.m_iFirstRow;
    set => this.m_iFirstRow = value;
  }

  public int LastRow
  {
    get => this.m_iLastRow;
    set => this.m_iLastRow = value;
  }

  public int FirstColumn
  {
    get => this.m_iFirstColumn;
    set => this.m_iFirstColumn = value;
  }

  public int LastColumn
  {
    get => this.m_iLastColumn;
    set => this.m_iLastColumn = value;
  }

  public ushort ExpressionLen => this.m_usExpressionLength;

  public byte[] Expression
  {
    get => this.m_arrExpression;
    set
    {
      this.m_arrExpression = value;
      this.m_usExpressionLength = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public Ptg[] Formula
  {
    get => this.m_arrFormula;
    set
    {
      int formulaLen;
      this.m_arrExpression = value != null ? FormulaUtil.PtgArrayToByteArray(value, out formulaLen, ExcelVersion.Excel2007) : throw new ArgumentNullException(nameof (Formula));
      this.m_usExpressionLength = (ushort) formulaLen;
      this.m_arrFormula = value;
    }
  }

  public int Reserved => this.m_iReserved;

  public override int MinimumRecordSize => 14;

  public bool IsRecalculateAlways
  {
    get => this.m_bRecalculateAlways;
    set => this.m_bRecalculateAlways = value;
  }

  public bool IsRecalculateOnOpen
  {
    get => this.m_bRecalculateOnOpen;
    set => this.m_bRecalculateOnOpen = value;
  }

  public ushort Options => this.m_usOptions;

  public ArrayRecord()
  {
  }

  public ArrayRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ArrayRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    iOffset = ArrayRecord.ParseDimensions((ISharedFormula) this, provider, iOffset, version);
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bRecalculateAlways = provider.ReadBit(iOffset, 0);
    this.m_bRecalculateOnOpen = provider.ReadBit(iOffset, 1);
    iOffset += 2;
    this.m_iReserved = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_usExpressionLength = provider.ReadUInt16(iOffset);
    iOffset += 2;
    int finalOffset;
    this.m_arrFormula = FormulaUtil.ParseExpression(provider, iOffset, (int) this.m_usExpressionLength, out finalOffset, version);
    this.m_arrExpression = new byte[finalOffset - iOffset];
    provider.ReadArray(iOffset, this.m_arrExpression);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iReserved = 0;
    int num = iOffset;
    int formulaLen;
    this.m_arrExpression = FormulaUtil.PtgArrayToByteArray(this.m_arrFormula, out formulaLen, version);
    this.m_usExpressionLength = (ushort) formulaLen;
    iOffset = ArrayRecord.SerializeDimensions((ISharedFormula) this, provider, iOffset, version);
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bRecalculateAlways, 0);
    provider.WriteBit(iOffset, this.m_bRecalculateOnOpen, 1);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iReserved);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usExpressionLength);
    iOffset += 2;
    this.m_iLength = iOffset - num;
    int length = this.m_arrExpression.Length;
    provider.WriteBytes(iOffset, this.m_arrExpression, 0, length);
    this.m_iLength += length;
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 14 + DVRecord.GetFormulaSize(this.m_arrFormula, version, true);
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 10;
    return storeSize;
  }

  public static int SerializeDimensions(
    ISharedFormula shared,
    DataProvider provider,
    int iOffset,
    ExcelVersion version)
  {
    if (version == ExcelVersion.Excel97to2003)
    {
      provider.WriteUInt16(iOffset, (ushort) shared.FirstRow);
      iOffset += 2;
      provider.WriteUInt16(iOffset, (ushort) shared.LastRow);
      iOffset += 2;
      provider.WriteByte(iOffset, (byte) shared.FirstColumn);
      ++iOffset;
      provider.WriteByte(iOffset, (byte) shared.LastColumn);
      ++iOffset;
    }
    else
    {
      if (version == ExcelVersion.Excel97to2003)
        throw new ArgumentOutOfRangeException(nameof (version));
      provider.WriteInt32(iOffset, shared.FirstRow);
      iOffset += 4;
      provider.WriteInt32(iOffset, shared.LastRow);
      iOffset += 4;
      provider.WriteInt32(iOffset, shared.FirstColumn);
      iOffset += 4;
      provider.WriteInt32(iOffset, shared.LastColumn);
      iOffset += 4;
    }
    return iOffset;
  }

  public static int ParseDimensions(
    ISharedFormula shared,
    DataProvider provider,
    int iOffset,
    ExcelVersion version)
  {
    if (version == ExcelVersion.Excel97to2003)
    {
      shared.FirstRow = (int) provider.ReadUInt16(iOffset);
      iOffset += 2;
      shared.LastRow = (int) provider.ReadUInt16(iOffset);
      iOffset += 2;
      shared.FirstColumn = (int) provider.ReadByte(iOffset);
      ++iOffset;
      shared.LastColumn = (int) provider.ReadByte(iOffset);
      ++iOffset;
    }
    else
    {
      if (version == ExcelVersion.Excel97to2003)
        throw new ArgumentOutOfRangeException(nameof (version));
      shared.FirstRow = provider.ReadInt32(iOffset);
      iOffset += 4;
      shared.LastRow = provider.ReadInt32(iOffset);
      iOffset += 4;
      shared.FirstColumn = provider.ReadInt32(iOffset);
      iOffset += 4;
      shared.LastColumn = provider.ReadInt32(iOffset);
      iOffset += 4;
    }
    return iOffset;
  }

  public override bool Equals(object obj)
  {
    if (!(obj is ArrayRecord arrayRecord))
      return base.Equals(obj);
    return arrayRecord.FirstColumn == this.FirstColumn && arrayRecord.FirstRow == this.FirstRow && arrayRecord.LastColumn == this.LastColumn && arrayRecord.LastRow == this.LastRow && Ptg.CompareArrays(arrayRecord.m_arrFormula, this.m_arrFormula);
  }

  public override int GetHashCode()
  {
    return this.FirstColumn.GetHashCode() ^ this.FirstRow.GetHashCode() ^ this.LastColumn.GetHashCode() ^ this.LastRow.GetHashCode();
  }

  public new object Clone()
  {
    ArrayRecord arrayRecord = (ArrayRecord) base.Clone();
    arrayRecord.m_arrExpression = CloneUtils.CloneByteArray(this.m_arrExpression);
    arrayRecord.m_arrFormula = CloneUtils.ClonePtgArray(this.m_arrFormula);
    return (object) arrayRecord;
  }
}
