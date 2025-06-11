// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CRNRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.CRN)]
[CLSCompliant(false)]
internal class CRNRecord : BiffRecordRaw, ICloneable
{
  private const int DEF_VALUES_OFFSET = 4;
  private const string DEF_ERROR_MESSAGE = "Unknown data type";
  private const int DefaultSize = 8;
  private static readonly byte[] DEF_RESERVED_BYTES = new byte[7];
  [BiffRecordPos(0, 1)]
  private byte m_btLastCol;
  [BiffRecordPos(1, 1)]
  private byte m_btFirstCol;
  [BiffRecordPos(2, 2)]
  private ushort m_usRow;
  private List<object> m_arrValues = new List<object>();

  public override bool NeedDataArray => true;

  public byte LastColumn
  {
    get => this.m_btLastCol;
    set => this.m_btLastCol = value;
  }

  public byte FirstColumn
  {
    get => this.m_btFirstCol;
    set => this.m_btFirstCol = value;
  }

  public ushort Row
  {
    get => this.m_usRow;
    set => this.m_usRow = value;
  }

  public override int MinimumRecordSize => 4;

  public List<object> Values => this.m_arrValues;

  public CRNRecord()
  {
  }

  public CRNRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CRNRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_btLastCol = provider.ReadByte(iOffset);
    this.m_btFirstCol = provider.ReadByte(iOffset + 1);
    this.m_usRow = provider.ReadUInt16(iOffset + 2);
    int num = iOffset;
    iOffset += 4;
    this.m_arrValues.Clear();
    while (iOffset - num < iLength)
      this.m_arrValues.Add(this.GetValue(provider, ref iOffset));
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteByte(iOffset, this.m_btLastCol);
    provider.WriteByte(iOffset + 1, this.m_btFirstCol);
    provider.WriteUInt16(iOffset + 2, this.m_usRow);
    iOffset += 4;
    int index = 0;
    for (int count = this.m_arrValues.Count; index < count; ++index)
      iOffset = this.SetValue(provider, iOffset, this.m_arrValues[index]);
  }

  private object GetValue(DataProvider provider, ref int iOffset)
  {
    object obj = (object) null;
    byte num = provider.ReadByte(iOffset);
    ++iOffset;
    switch ((CRNRecord.CellValueType) num)
    {
      case CRNRecord.CellValueType.Nil:
        iOffset += 8;
        break;
      case CRNRecord.CellValueType.Number:
        obj = (object) provider.ReadDouble(iOffset);
        iOffset += 8;
        break;
      case CRNRecord.CellValueType.String:
        obj = (object) provider.ReadString16BitUpdateOffset(ref iOffset);
        break;
      case CRNRecord.CellValueType.Boolean:
        obj = (object) provider.ReadBoolean(iOffset);
        iOffset += 8;
        break;
      case CRNRecord.CellValueType.Error:
        obj = (object) provider.ReadByte(iOffset);
        iOffset += 8;
        break;
      default:
        throw new ApplicationException("Unknown data type");
    }
    return obj;
  }

  private int SetValue(DataProvider provider, int iOffset, object value)
  {
    switch (value)
    {
      case null:
        throw new ArgumentNullException(nameof (value));
      case double num1:
        provider.WriteByte(iOffset, (byte) 1);
        ++iOffset;
        provider.WriteDouble(iOffset, num1);
        iOffset += 8;
        break;
      case string _:
        provider.WriteByte(iOffset, (byte) 2);
        ++iOffset;
        string str = value as string;
        provider.WriteString16BitUpdateOffset(ref iOffset, str);
        if (str.Length == 0)
        {
          provider.WriteByte(iOffset++, (byte) 0);
          break;
        }
        break;
      case bool flag:
        provider.WriteByte(iOffset, (byte) 4);
        ++iOffset;
        provider.WriteByte(iOffset++, flag ? (byte) 1 : (byte) 0);
        provider.WriteBytes(iOffset, CRNRecord.DEF_RESERVED_BYTES);
        iOffset += CRNRecord.DEF_RESERVED_BYTES.Length;
        break;
      case byte num2:
        provider.WriteByte(iOffset++, (byte) 16 /*0x10*/);
        provider.WriteByte(iOffset++, num2);
        provider.WriteBytes(iOffset, CRNRecord.DEF_RESERVED_BYTES);
        iOffset += CRNRecord.DEF_RESERVED_BYTES.Length;
        break;
      default:
        throw new ArgumentOutOfRangeException("Wrong data type");
    }
    return iOffset;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 4;
    int index = 0;
    for (int count = this.m_arrValues.Count; index < count; ++index)
    {
      if (this.m_arrValues[index] is string arrValue)
        storeSize += 4 + arrValue.Length * 2;
      else
        storeSize += 9;
    }
    return storeSize;
  }

  public override object Clone()
  {
    CRNRecord crnRecord = (CRNRecord) base.Clone();
    crnRecord.m_arrValues = new List<object>((IEnumerable<object>) this.m_arrValues);
    return (object) crnRecord;
  }

  private enum CellValueType
  {
    Nil = 0,
    Number = 1,
    String = 2,
    Boolean = 4,
    Error = 16, // 0x00000010
  }
}
