// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BiffRecordRawWithArray
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public abstract class BiffRecordRawWithArray : BiffRecordRaw, IDisposable
{
  protected internal byte[] m_data;
  private bool m_bAutoGrow;

  protected BiffRecordRawWithArray()
  {
    if (!this.NeedDataArray)
      return;
    this.m_data = new byte[0];
  }

  protected BiffRecordRawWithArray(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  protected BiffRecordRawWithArray(BinaryReader reader, out int itemSize)
    : base(reader, out itemSize)
  {
  }

  protected BiffRecordRawWithArray(int iReserve)
    : base(iReserve)
  {
    this.m_data = iReserve >= 0 ? new byte[iReserve] : throw new ArgumentOutOfRangeException(nameof (iReserve), "Reserved memory count must be greater than zero.");
  }

  ~BiffRecordRawWithArray() => this.Dispose();

  public override byte[] Data
  {
    get
    {
      if (this.NeedInfill)
        this.InfillInternalData(ExcelVersion.Excel97to2003);
      return this.m_data;
    }
    set
    {
      if (value == null)
        return;
      this.m_data = value;
    }
  }

  public override bool AutoGrowData
  {
    get => this.m_bAutoGrow;
    set => this.m_bAutoGrow = value;
  }

  public override int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    return this.FillStream(writer, encryptor, streamPosition);
  }

  public virtual int FillStream(BinaryWriter writer, IEncryptor encryptor, int streamPosition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (this.NeedInfill)
      this.InfillInternalData(ExcelVersion.Excel97to2003);
    else
      this.NeedInfill = true;
    if (this.m_iLength < 0)
      throw new ApplicationException("Wrong Record data infill. " + this.TypeCode.ToString());
    writer.Write((ushort) this.m_iCode);
    writer.Write((ushort) this.m_iLength);
    streamPosition += 4;
    int length = this.m_data == null ? 0 : this.m_data.Length;
    if (length < this.m_iLength)
      throw new ApplicationException("Length of data is greater than internal storage contains.Object Type is " + this.GetType().Name);
    if (length > 0)
    {
      if (encryptor != null)
      {
        int startDecodingOffset = this.StartDecodingOffset;
        encryptor.Encrypt(this.m_data, startDecodingOffset, this.m_iLength - startDecodingOffset, (long) (streamPosition + startDecodingOffset));
      }
      writer.Write(this.m_data, 0, this.m_iLength);
    }
    return this.m_iLength + 4;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_data = new byte[iLength];
    this.m_iLength = iLength;
    provider.CopyTo(iOffset, this.m_data, 0, iLength);
    this.ParseStructure();
    if (this.NeedDataArray)
      return;
    this.m_data = new byte[0];
    this.AutoGrowData = true;
  }

  public abstract void InfillInternalData(ExcelVersion version);

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    this.InfillInternalData(ExcelVersion.Excel97to2003);
    if (this.m_iLength <= 0)
      return;
    provider.WriteBytes(iOffset, this.m_data, 0, this.m_iLength);
  }

  protected void CheckOffsetAndLength(int offset, int length)
  {
    int length1 = this.m_data.Length;
    if (offset < 0 || offset > length1)
      throw new ArgumentOutOfRangeException(nameof (offset), "");
    if (length < 0 || length > length1)
      throw new ArgumentOutOfRangeException(nameof (length), "");
    if (length + offset > length1)
      throw new ArgumentException("Length or offset has wrong value.", "length & offset");
  }

  protected byte[] GetBytes(int offset, int length)
  {
    if (length == 0)
      return new byte[0];
    this.CheckOffsetAndLength(offset, length);
    byte[] dst = new byte[length];
    Buffer.BlockCopy((Array) this.m_data, offset, (Array) dst, 0, length);
    return dst;
  }

  protected byte GetByte(int offset)
  {
    this.CheckOffsetAndLength(offset, 1);
    return this.m_data[offset];
  }

  protected ushort GetUInt16(int offset)
  {
    this.CheckOffsetAndLength(offset, 2);
    return BitConverter.ToUInt16(this.m_data, offset);
  }

  protected short GetInt16(int offset)
  {
    this.CheckOffsetAndLength(offset, 2);
    return BitConverter.ToInt16(this.m_data, offset);
  }

  protected int GetInt32(int offset)
  {
    this.CheckOffsetAndLength(offset, 4);
    return BitConverter.ToInt32(this.m_data, offset);
  }

  protected uint GetUInt32(int offset)
  {
    this.CheckOffsetAndLength(offset, 4);
    return BitConverter.ToUInt32(this.m_data, offset);
  }

  protected long GetInt64(int offset)
  {
    this.CheckOffsetAndLength(offset, 28);
    return BitConverter.ToInt64(this.m_data, offset);
  }

  protected ulong GetUInt64(int offset)
  {
    this.CheckOffsetAndLength(offset, 8);
    return BitConverter.ToUInt64(this.m_data, offset);
  }

  protected float GetFloat(int offset)
  {
    this.CheckOffsetAndLength(offset, 4);
    return BitConverter.ToSingle(this.m_data, offset);
  }

  protected double GetDouble(int offset)
  {
    this.CheckOffsetAndLength(offset, 8);
    return BitConverter.ToDouble(this.m_data, offset);
  }

  protected bool GetBit(int offset, int bitPos)
  {
    if (bitPos < 0 || bitPos > 7)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zeroless or greater than 7.");
    this.CheckOffsetAndLength(offset, 1);
    return ((int) this.m_data[offset] & 1 << bitPos) != 0;
  }

  protected string GetString16BitUpdateOffset(ref int offset, out bool asciiString)
  {
    int uint16 = (int) this.GetUInt16(offset);
    offset += 2;
    asciiString = false;
    if (uint16 > 0)
    {
      int iBytesInString;
      string string16BitUpdateOffset = this.GetString(offset, uint16, out iBytesInString);
      offset += iBytesInString + 1;
      asciiString = iBytesInString == uint16;
      return string16BitUpdateOffset;
    }
    ++offset;
    return string.Empty;
  }

  protected string GetString16BitUpdateOffset(ref int offset)
  {
    return this.GetString16BitUpdateOffset(ref offset, out bool _);
  }

  protected string GetStringUpdateOffset(ref int offset, int iStrLen)
  {
    if (iStrLen <= 0)
      return string.Empty;
    int iBytesInString;
    string stringUpdateOffset = this.GetString(offset, iStrLen, out iBytesInString);
    offset += iBytesInString + 1;
    return stringUpdateOffset;
  }

  protected string GetStringByteLen(int offset)
  {
    int iStrLen = (int) this.m_data[offset];
    return this.GetString(offset + 1, iStrLen);
  }

  protected string GetStringByteLen(int offset, out int iBytes)
  {
    int iStrLen = (int) this.m_data[offset];
    return this.GetString(offset + 1, iStrLen, out iBytes);
  }

  protected internal string GetString(int offset, int iStrLen)
  {
    return this.GetString(offset, iStrLen, out int _);
  }

  protected internal string GetString(int offset, int iStrLen, out int iBytesInString)
  {
    return this.GetString(offset, iStrLen, out iBytesInString, false);
  }

  protected internal string GetString(
    int offset,
    int iStrLen,
    out int iBytesInString,
    bool isByteCounted)
  {
    byte num = this.m_data[offset];
    if ((num == (byte) 0 || isByteCounted ? iStrLen : 2 * iStrLen) + (offset + 1) > this.m_iLength)
      throw new WrongBiffRecordDataException($"String and m_data array do not fit each other {this.TypeCode}.");
    if (num == (byte) 0)
    {
      iBytesInString = iStrLen;
      this.CheckOffsetAndLength(offset + 1, iStrLen);
      return BiffRecordRaw.LatinEncoding.GetString(this.m_data, offset + 1, iStrLen);
    }
    iBytesInString = isByteCounted ? iStrLen : iStrLen * 2;
    this.CheckOffsetAndLength(offset + 1, iBytesInString);
    return Encoding.Unicode.GetString(this.m_data, offset + 1, iBytesInString);
  }

  protected string GetUnkTypeString(
    int offset,
    IList<int> continuePos,
    int continueCount,
    ref int iBreakIndex,
    out int length,
    out byte[] rich,
    out byte[] extended)
  {
    string str1 = (string) null;
    int num1 = 3;
    rich = (byte[]) null;
    extended = (byte[]) null;
    int startIndex = offset;
    ushort uint16 = BitConverter.ToUInt16(this.m_data, startIndex);
    byte num2 = this.m_data[startIndex + 2];
    bool flag1 = ((int) num2 & 1) == 1;
    bool flag2 = ((int) num2 & 4) != 0;
    bool flag3 = ((int) num2 & 8) != 0;
    int num3 = 3;
    short num4 = 0;
    int length1 = 0;
    if (flag3)
    {
      num4 = this.GetInt16(startIndex + num3);
      num3 += 2;
      num1 += 2;
    }
    if (flag2)
    {
      length1 = this.GetInt32(startIndex + num3);
      num3 += 4;
      num1 += 4;
    }
    int num5 = startIndex + num3;
    int num6 = 0;
    Encoding encoding = flag1 ? Encoding.Unicode : BiffRecordRaw.LatinEncoding;
    while (num6 < (int) uint16)
    {
      int count1 = flag1 ? ((int) uint16 - num6) * 2 : (int) uint16 - num6;
      int count2 = BiffRecordRaw.FindNextBreak(continuePos, continueCount, num5, ref iBreakIndex) - num5;
      if (count1 <= count2)
      {
        string str2 = encoding.GetString(this.m_data, num5, count1);
        str1 = str1 == null ? str2 : str1 + str2;
        num1 += count1;
        break;
      }
      string str3 = encoding.GetString(this.m_data, num5, count2);
      str1 = str1 == null ? str3 : str1 + str3;
      num6 += flag1 ? count2 / 2 : count2;
      if (this.m_data[num5 + count2] == (byte) 0 || this.m_data[num5 + count2] == (byte) 1)
      {
        flag1 = this.m_data[num5 + count2] == (byte) 1;
        encoding = flag1 ? Encoding.Unicode : BiffRecordRaw.LatinEncoding;
        ++num5;
        ++num1;
      }
      num5 += count2;
      num1 += count2;
    }
    if (flag3)
    {
      int length2 = (int) num4 * 4;
      rich = this.GetBytes(offset + num1, length2);
      num1 += length2;
    }
    if (flag2)
    {
      extended = this.GetBytes(offset + num1, length1);
      num1 += length1;
    }
    length = num1;
    return str1 ?? string.Empty;
  }

  protected TAddr GetAddr(int offset)
  {
    return new TAddr()
    {
      FirstRow = (int) this.GetUInt16(offset),
      LastRow = (int) this.GetUInt16(offset + 2),
      FirstCol = (int) this.GetUInt16(offset + 4),
      LastCol = (int) this.GetUInt16(offset + 6)
    };
  }

  protected Rectangle GetAddrAsRectangle(int offset)
  {
    int uint16_1 = (int) this.GetUInt16(offset);
    int uint16_2 = (int) this.GetUInt16(offset + 2);
    int uint16_3 = (int) this.GetUInt16(offset + 4);
    int uint16_4 = (int) this.GetUInt16(offset + 6);
    return Rectangle.FromLTRB(uint16_3, uint16_1, uint16_4, uint16_2);
  }

  protected void EnlargeDataStorageIfNeeded(int offset, int length)
  {
    if (this.m_data != null && offset + length <= this.m_data.Length)
      return;
    int length1 = this.m_data == null ? 0 : this.m_data.Length;
    int length2 = Math.Min(offset * 2 + length + 16 /*0x10*/, this.MaximumMemorySize);
    if (length2 <= length1)
      return;
    byte[] dst = new byte[length2];
    if (length1 > 0)
      Buffer.BlockCopy((Array) this.m_data, 0, (Array) dst, 0, length1);
    this.m_data = dst;
  }

  protected internal void Reserve(int length)
  {
    if (this.m_data.Length > length)
      return;
    byte[] dst = new byte[length];
    Buffer.BlockCopy((Array) this.m_data, 0, (Array) dst, 0, this.m_data.Length);
    this.m_data = dst;
  }

  protected internal void SetBytes(int offset, byte[] value, int pos, int length)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (pos < 0)
      throw new ArgumentOutOfRangeException(nameof (pos), "Position cannot be zeroless.");
    if (length < 0)
      throw new ArgumentOutOfRangeException(nameof (length), "Length of data to copy must be greater then zero.");
    if (pos + length > value.Length)
      throw new ArgumentOutOfRangeException(nameof (value), "Position or length has wrong value.");
    if (this.AutoGrowData)
      this.EnlargeDataStorageIfNeeded(offset, length);
    else if (offset + length > this.m_data.Length)
      throw new ArgumentOutOfRangeException("m_data", "Internal array size is too small.");
    Buffer.BlockCopy((Array) value, pos, (Array) this.m_data, offset, length);
  }

  protected internal void SetBytes(int offset, byte[] value)
  {
    this.SetBytes(offset, value, 0, value.Length);
  }

  protected internal void SetByte(int offset, byte value)
  {
    if (this.AutoGrowData)
      this.EnlargeDataStorageIfNeeded(offset, 1);
    this.m_data[offset] = value;
  }

  protected internal void SetByte(int offset, byte value, int count)
  {
    byte[] numArray = new byte[count];
    for (int index = 0; index < count; ++index)
      numArray[index] = value;
    this.SetBytes(offset, numArray, 0, count);
  }

  protected internal void SetUInt16(int offset, ushort value)
  {
    if (this.AutoGrowData)
      this.EnlargeDataStorageIfNeeded(offset, 2);
    byte num1 = (byte) ((uint) value & (uint) byte.MaxValue);
    byte num2 = (byte) ((int) value >> 8 & (int) byte.MaxValue);
    this.m_data[offset] = num1;
    this.m_data[offset + 1] = num2;
  }

  protected internal void SetInt16(int offset, short value)
  {
    this.SetBytes(offset, BitConverter.GetBytes(value), 0, 2);
  }

  protected internal void SetInt32(int offset, int value)
  {
    if (this.AutoGrowData)
      this.EnlargeDataStorageIfNeeded(offset, 4);
    Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_data, offset, 4);
  }

  protected internal void SetUInt32(int offset, uint value)
  {
    this.SetBytes(offset, BitConverter.GetBytes(value), 0, 4);
  }

  protected internal void SetInt64(int offset, long value)
  {
    this.SetBytes(offset, BitConverter.GetBytes(value), 0, 8);
  }

  protected internal void SetUInt64(int offset, ulong value)
  {
    this.SetBytes(offset, BitConverter.GetBytes(value), 0, 8);
  }

  protected internal void SetFloat(int offset, float value)
  {
    this.SetBytes(offset, BitConverter.GetBytes(value), 0, 4);
  }

  protected internal void SetDouble(int offset, double value)
  {
    this.SetBytes(offset, BitConverter.GetBytes(value), 0, 8);
  }

  protected internal void SetBit(int offset, bool value, int bitPos)
  {
    if (bitPos < 0 || bitPos > 7)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zero or greater than 7.");
    if (this.AutoGrowData)
      this.EnlargeDataStorageIfNeeded(offset, 1);
    if (value)
      this.m_data[offset] |= (byte) (1 << bitPos);
    else
      this.m_data[offset] &= (byte) ~(1 << bitPos);
  }

  protected internal void SetStringNoLenUpdateOffset(
    ref int offset,
    string value,
    bool isCompression)
  {
    switch (value)
    {
      case null:
        break;
      case "":
        break;
      default:
        Encoding encoding = Encoding.Default;
        byte[] bytes = (isCompression ? encoding : Encoding.Unicode).GetBytes(value);
        byte num = isCompression ? (byte) 0 : (byte) 1;
        this.SetByte(offset, num);
        this.SetBytes(offset + 1, bytes, 0, bytes.Length);
        offset += bytes.Length + 1;
        break;
    }
  }

  protected internal int SetStringNoLenDetectEncoding(int offset, string value)
  {
    return BiffRecordRawWithArray.IsAsciiString(value) ? this.SetStringNoLen(offset, value, true, true) : this.SetStringNoLen(offset, value);
  }

  public static bool IsAsciiString(string strTextPart)
  {
    bool flag = true;
    int length = strTextPart != null ? strTextPart.Length : 0;
    for (int index = 0; index < length; ++index)
    {
      if (strTextPart[index] > '\u007F')
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  protected internal int SetStringNoLen(int offset, string value)
  {
    return this.SetStringNoLen(offset, value, false, false);
  }

  protected internal int SetStringNoLen(
    int offset,
    string value,
    bool bEmptyCompressed,
    bool bCompressed)
  {
    if (value == null || value.Length == 0)
    {
      if (!bEmptyCompressed)
        return 0;
      this.SetByte(offset, (byte) 0);
      return 1;
    }
    byte[] bytes = (bCompressed ? Encoding.Default : Encoding.Unicode).GetBytes(value);
    byte num = bCompressed ? (byte) 0 : (byte) 1;
    this.SetByte(offset, num);
    this.SetBytes(offset + 1, bytes, 0, bytes.Length);
    return bytes.Length + 1;
  }

  protected internal int SetStringByteLen(int offset, string value)
  {
    this.SetByte(offset, (byte) value.Length);
    return this.SetStringNoLen(offset + 1, value) + 1;
  }

  protected internal int SetString16BitLen(int offset, string value)
  {
    ushort length = value != null ? (ushort) value.Length : (ushort) 0;
    this.SetUInt16(offset, length);
    return 2 + this.SetStringNoLen(offset + 2, value);
  }

  protected internal int SetString16BitLen(
    int offset,
    string value,
    bool bEmptyCompressed,
    bool isCompressed)
  {
    this.SetUInt16(offset, (ushort) value.Length);
    return 2 + this.SetStringNoLen(offset + 2, value, bEmptyCompressed, isCompressed);
  }

  protected internal void SetString16BitUpdateOffset(ref int offset, string value)
  {
    this.SetUInt16(offset, (ushort) value.Length);
    offset += 2;
    this.SetStringNoLenUpdateOffset(ref offset, value, false);
  }

  protected internal void SetString16BitUpdateOffset(
    ref int offset,
    string value,
    bool isCompressed)
  {
    this.SetUInt16(offset, (ushort) value.Length);
    offset += 2;
    this.SetStringNoLenUpdateOffset(ref offset, value, isCompressed);
  }

  protected internal void SetAddr(int offset, TAddr addr)
  {
    this.SetUInt16(offset, (ushort) addr.FirstRow);
    this.SetUInt16(offset + 2, (ushort) addr.LastRow);
    this.SetUInt16(offset + 4, (ushort) addr.FirstCol);
    this.SetUInt16(offset + 6, (ushort) addr.LastCol);
  }

  protected internal void SetAddr(int offset, Rectangle addr)
  {
    this.SetUInt16(offset, (ushort) addr.Top);
    this.SetUInt16(offset + 2, (ushort) addr.Bottom);
    this.SetUInt16(offset + 4, (ushort) addr.Left);
    this.SetUInt16(offset + 6, (ushort) addr.Right);
  }

  public SortedList<BiffRecordPosAttribute, FieldInfo> GetSortedFields()
  {
    SortedList<BiffRecordPosAttribute, FieldInfo> sortedFields;
    if (!BiffRecordRaw.m_ReflectCache.TryGetValue(this.m_iCode, out sortedFields))
    {
      FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
      sortedFields = new SortedList<BiffRecordPosAttribute, FieldInfo>((IComparer<BiffRecordPosAttribute>) new RecordsPosComparer());
      int index = 0;
      for (int length = fields.Length; index < length; ++index)
      {
        FieldInfo fieldInfo = fields[index];
        object[] customAttributes = fieldInfo.GetCustomAttributes(typeof (BiffRecordPosAttribute), true);
        if (customAttributes.Length > 0)
          sortedFields.Add((BiffRecordPosAttribute) customAttributes[0], fieldInfo);
      }
      BiffRecordRaw.m_ReflectCache[this.m_iCode] = sortedFields;
    }
    return sortedFields;
  }

  protected void AutoExtractFields()
  {
    SortedList<BiffRecordPosAttribute, FieldInfo> sortedFields = this.GetSortedFields();
    Debug.IndentLevel = 1;
    IList<BiffRecordPosAttribute> keys = sortedFields.Keys;
    IList<FieldInfo> values = sortedFields.Values;
    int index = 0;
    for (int count = sortedFields.Count; index < count; ++index)
    {
      BiffRecordPosAttribute attr = keys[index];
      values[index].SetValue((object) this, this.GetValueByAttributeType(attr));
    }
    Debug.IndentLevel = 0;
  }

  protected object GetValueByAttributeType(BiffRecordPosAttribute attr)
  {
    int position = attr.Position;
    if (attr.IsBit)
      return (object) this.GetBit(attr.Position, attr.SizeOrBitPosition);
    if (attr.IsString)
    {
      byte iStrLen = this.m_data[attr.Position];
      return (object) this.GetString(attr.Position + 1, (int) iStrLen);
    }
    if (attr.IsString16Bit)
    {
      ushort uint16 = this.GetUInt16(attr.Position);
      return uint16 <= (ushort) 0 ? (object) string.Empty : (object) this.GetString(attr.Position + 2, (int) uint16);
    }
    if (attr.IsOEMString)
    {
      byte num = this.m_data[attr.Position];
      if (attr.Position + (int) num > this.m_data.Length)
        throw new WrongBiffRecordDataException("Wrong Record data: string is too long.");
      return num != (byte) 0 ? (object) BiffRecordRaw.LatinEncoding.GetString(this.GetBytes(attr.Position + 1, (int) num), 0, (int) num) : (object) "";
    }
    if (attr.IsOEMString16Bit)
    {
      ushort uint16 = this.GetUInt16(attr.Position);
      if (attr.Position + (int) uint16 + 2 > this.m_data.Length)
        throw new WrongBiffRecordDataException("Wrong Record data: string is too long.");
      return uint16 != (ushort) 0 ? (object) BiffRecordRaw.LatinEncoding.GetString(this.GetBytes(attr.Position + 2, (int) uint16), 0, (int) uint16) : (object) "";
    }
    switch (attr.SizeOrBitPosition)
    {
      case 1:
        return (object) this.GetByte(position);
      case 2:
        return attr.IsSigned ? (object) this.GetInt16(position) : (object) this.GetUInt16(position);
      case 4:
        if (attr.IsFloat)
          return (object) this.GetFloat(position);
        return attr.IsSigned ? (object) this.GetInt32(position) : (object) this.GetUInt32(position);
      case 8:
        if (attr.IsFloat)
          return (object) this.GetDouble(position);
        return attr.IsSigned ? (object) this.GetInt64(position) : (object) this.GetUInt64(position);
      default:
        throw new ApplicationException($"AutoReader - Unknown size of item field. Record.{(object) this.TypeCode}. Code {(object) this.RecordCode}");
    }
  }

  protected int AutoInfillFromFields()
  {
    SortedList<BiffRecordPosAttribute, FieldInfo> sortedFields = this.GetSortedFields();
    bool autoGrowData = this.AutoGrowData;
    this.AutoGrowData = true;
    int val1 = 0;
    IList<BiffRecordPosAttribute> keys = sortedFields.Keys;
    IList<FieldInfo> values = sortedFields.Values;
    int index = 0;
    for (int count = sortedFields.Count; index < count; ++index)
    {
      BiffRecordPosAttribute attr = keys[index];
      object data = values[index].GetValue((object) this);
      int num = this.SetValueByAttributeType(attr, data);
      val1 = Math.Max(val1, attr.Position + num);
    }
    this.AutoGrowData = autoGrowData;
    return val1;
  }

  protected int SetValueByAttributeType(BiffRecordPosAttribute attr, object data)
  {
    int num = 0;
    int position = attr.Position;
    if (attr.IsOEMString)
    {
      byte[] bytes = BiffRecordRaw.LatinEncoding.GetBytes((string) data);
      this.SetByte(position, (byte) bytes.Length);
      this.SetBytes(position + 1, bytes, 0, bytes.Length);
      num = 1 + bytes.Length;
    }
    if (attr.IsOEMString16Bit)
    {
      byte[] bytes = BiffRecordRaw.LatinEncoding.GetBytes((string) data);
      this.SetUInt16(position, (ushort) bytes.Length);
      this.SetBytes(position + 2, bytes, 0, bytes.Length);
      num = 2 + bytes.Length;
    }
    else if (attr.IsString)
    {
      string s = (string) data;
      byte[] bytes = Encoding.Unicode.GetBytes(s);
      this.SetByte(position, (byte) s.Length);
      this.SetByte(position + 1, (byte) 1);
      this.SetBytes(position + 2, bytes, 0, bytes.Length);
      num = 2 + bytes.Length;
    }
    else if (attr.IsString16Bit)
    {
      string s = (string) data;
      int length = s != null ? s.Length : 0;
      this.SetUInt16(position, (ushort) length);
      num = 2;
      if (length > 0)
      {
        byte[] bytes = Encoding.Unicode.GetBytes(s);
        this.SetByte(position + 2, (byte) 1);
        this.SetBytes(position + 3, bytes, 0, bytes.Length);
        num = 3 + bytes.Length;
      }
    }
    else if (attr.IsBit)
    {
      this.SetBit(position, (bool) data, attr.SizeOrBitPosition);
    }
    else
    {
      switch (attr.SizeOrBitPosition)
      {
        case 1:
          this.SetByte(position, (byte) data);
          break;
        case 2:
          if (attr.IsSigned)
          {
            this.SetInt16(position, (short) data);
            break;
          }
          this.SetUInt16(position, (ushort) data);
          break;
        case 4:
          if (attr.IsFloat)
          {
            this.SetFloat(position, (float) data);
            break;
          }
          if (attr.IsSigned)
          {
            this.SetInt32(position, (int) data);
            break;
          }
          this.SetUInt32(position, (uint) data);
          break;
        case 8:
          if (attr.IsFloat)
          {
            this.SetDouble(position, (double) data);
            break;
          }
          if (attr.IsSigned)
          {
            this.SetInt64(position, (long) data);
            break;
          }
          this.SetUInt64(position, (ulong) data);
          break;
      }
      num = attr.SizeOrBitPosition;
    }
    return num;
  }

  public override void ClearData() => this.m_data = new byte[0];

  public override bool IsEqual(BiffRecordRaw raw)
  {
    if (!(raw is BiffRecordRawWithArray recordRawWithArray))
      return false;
    this.InfillInternalData(ExcelVersion.Excel2007);
    recordRawWithArray.InfillInternalData(ExcelVersion.Excel2007);
    if (this.m_iLength == recordRawWithArray.m_iLength)
    {
      for (int index = 0; index < this.m_iLength; ++index)
      {
        if ((int) this.m_data[index] != (int) recordRawWithArray.m_data[index])
          return false;
      }
    }
    return true;
  }

  public override void CopyTo(BiffRecordRaw raw)
  {
    BiffRecordRawWithArray recordRawWithArray = this.RecordCode == raw.RecordCode ? raw as BiffRecordRawWithArray : throw new ArgumentException("Records should have same type for copy.");
    this.InfillInternalData(ExcelVersion.Excel2007);
    recordRawWithArray.m_data = new byte[this.Length];
    Array.Copy((Array) this.m_data, 0, (Array) recordRawWithArray.m_data, 0, this.Length);
    recordRawWithArray.m_iLength = this.m_iLength;
    recordRawWithArray.ParseStructure();
  }

  protected internal void SetInternalData(byte[] arrData) => this.SetInternalData(arrData, true);

  protected void SetInternalData(byte[] arrData, bool bNeedInfill)
  {
    this.m_data = arrData;
    this.NeedInfill = bNeedInfill;
  }

  public abstract void ParseStructure();

  public override int GetStoreSize(ExcelVersion version)
  {
    int minimumRecordSize = this.MinimumRecordSize;
    if (minimumRecordSize == this.MaximumRecordSize)
      return minimumRecordSize;
    if (this.NeedInfill)
    {
      this.InfillInternalData(version);
      this.NeedInfill = false;
    }
    return this.m_iLength;
  }

  public virtual void Dispose()
  {
    this.OnDispose();
    this.m_data = (byte[]) null;
    GC.SuppressFinalize((object) this);
  }

  protected virtual void OnDispose()
  {
  }
}
