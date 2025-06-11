// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BiffRecordRaw
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal abstract class BiffRecordRaw : ICloneable, IBiffStorage
{
  private const int DEF_RESERVE_SIZE = 100;
  public const int DEF_RECORD_MAX_SIZE = 8224;
  public const int DEF_RECORD_MAX_SIZE_WITH_HADER = 8228;
  public const int DEF_HEADER_SIZE = 4;
  public const int DEF_BITS_IN_BYTE = 8;
  private const int DEF_BITS_IN_SHORT = 16 /*0x10*/;
  private const int DEF_BITS_IN_INT = 32 /*0x20*/;
  protected static Dictionary<int, SortedList<BiffRecordPosAttribute, FieldInfo>> m_ReflectCache = new Dictionary<int, SortedList<BiffRecordPosAttribute, FieldInfo>>(100);
  private static readonly Encoding s_latin1 = Encoding.GetEncoding("latin1");
  protected int m_iCode = -1;
  protected int m_iLength = -1;
  private bool m_bNeedInfill = true;

  public static int SkipBeginEndBlock(IList<BiffRecordRaw> recordList, int iPos)
  {
    recordList[iPos].CheckTypeCode(TBIFFRecord.Begin);
    int num = 1;
    ++iPos;
    while (num > 0)
    {
      switch (recordList[iPos].TypeCode)
      {
        case TBIFFRecord.Begin:
          ++num;
          break;
        case TBIFFRecord.End:
          --num;
          break;
      }
      ++iPos;
    }
    return iPos;
  }

  public TBIFFRecord TypeCode => (TBIFFRecord) this.m_iCode;

  public int RecordCode => this.m_iCode;

  public int Length
  {
    get => this.m_iLength;
    set => this.m_iLength = value;
  }

  public virtual byte[] Data
  {
    get
    {
      this.m_iLength = this.GetStoreSize(OfficeVersion.Excel97to2003);
      byte[] arrData = new byte[this.m_iLength];
      this.InfillInternalData((DataProvider) new ByteArrayDataProvider(arrData), 0, OfficeVersion.Excel97to2003);
      return arrData;
    }
    set
    {
      if (value == null)
        return;
      int length = value.Length;
      this.ParseStructure((DataProvider) new ByteArrayDataProvider(value), 0, length, OfficeVersion.Excel97to2003);
    }
  }

  public virtual bool AutoGrowData
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public virtual long StreamPos
  {
    get => -1;
    set
    {
    }
  }

  public virtual int MinimumRecordSize => 0;

  public virtual int MaximumRecordSize => 8224;

  public virtual int MaximumMemorySize => int.MaxValue;

  public bool NeedInfill
  {
    get => this.m_bNeedInfill;
    set => this.m_bNeedInfill = value;
  }

  public virtual bool NeedDataArray => false;

  public virtual bool IsAllowShortData => false;

  public virtual bool NeedDecoding => true;

  public virtual int StartDecodingOffset => 0;

  internal static ushort GetUInt16BitsByMask(ushort value, ushort BitMask)
  {
    return (ushort) ((uint) value & (uint) BitMask);
  }

  internal static void SetUInt16BitsByMask(ref ushort destination, ushort BitMask, ushort value)
  {
    destination &= ~BitMask;
    destination += (ushort) ((uint) value & (uint) BitMask);
  }

  internal static uint GetUInt32BitsByMask(uint value, uint BitMask) => value & BitMask;

  internal static void SetUInt32BitsByMask(ref uint destination, uint BitMask, uint value)
  {
    destination &= ~BitMask;
    destination += value & BitMask;
  }

  protected BiffRecordRaw()
  {
    object[] customAttributes = this.GetType().GetCustomAttributes(typeof (BiffAttribute), true);
    if (customAttributes.Length == 0)
      return;
    this.m_iCode = (int) ((BiffAttribute) customAttributes[0]).Code;
  }

  protected BiffRecordRaw(Stream stream, out int itemSize) => throw new NotImplementedException();

  protected BiffRecordRaw(int iReserve)
  {
  }

  public virtual void UpdateOffsets(List<BiffRecordRaw> records)
  {
    throw new ApplicationException("Class marked as offset contains field but does not provide override of UpdateOffset method. Or you try to call parent class virtual method. Please check code.");
  }

  public virtual void ParseStructure(
    DataProvider arrData,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    throw new NotImplementedException(this.TypeCode.ToString());
  }

  public virtual void InfillInternalData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    throw new NotImplementedException(this.TypeCode.ToString());
  }

  public virtual int GetStoreSize(OfficeVersion version)
  {
    int minimumRecordSize = this.MinimumRecordSize;
    return minimumRecordSize == this.MaximumRecordSize ? minimumRecordSize : throw new ApplicationException("StoreSize should be overloaded " + this.TypeCode.ToString());
  }

  public static void CheckOffsetAndLength(byte[] arrData, int offset, int length)
  {
    int length1 = arrData.Length;
    if (offset < 0 || offset > length1)
      throw new ArgumentOutOfRangeException(nameof (offset), "");
    if (length < 0 || length > length1)
      throw new ArgumentOutOfRangeException(nameof (length), "");
    if (length + offset > length1)
      throw new ArgumentException("Length or offset has wrong value.", "length & offset");
  }

  public static byte[] GetBytes(byte[] arrData, int offset, int length)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, length);
    byte[] dst = new byte[length];
    Buffer.BlockCopy((Array) arrData, offset, (Array) dst, 0, length);
    return dst;
  }

  public static byte GetByte(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 1);
    return arrData[offset];
  }

  [CLSCompliant(false)]
  public static ushort GetUInt16(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 2);
    return BitConverter.ToUInt16(arrData, offset);
  }

  [CLSCompliant(false)]
  public static short GetInt16(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 2);
    return BitConverter.ToInt16(arrData, offset);
  }

  public static int GetInt32(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 4);
    return BitConverter.ToInt32(arrData, offset);
  }

  [CLSCompliant(false)]
  public static uint GetUInt32(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 4);
    return BitConverter.ToUInt32(arrData, offset);
  }

  public static long GetInt64(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 28);
    return BitConverter.ToInt64(arrData, offset);
  }

  [CLSCompliant(false)]
  public static ulong GetUInt64(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 8);
    return BitConverter.ToUInt64(arrData, offset);
  }

  public static float GetFloat(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 4);
    return BitConverter.ToSingle(arrData, offset);
  }

  public static double GetDouble(byte[] arrData, int offset)
  {
    BiffRecordRaw.CheckOffsetAndLength(arrData, offset, 8);
    return BitConverter.ToDouble(arrData, offset);
  }

  public static bool GetBit(byte[] arrData, int offset, int bitPos)
  {
    if (bitPos < 0 || bitPos > 7)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater than 7.");
    if (arrData.Length <= offset)
      throw new ArgumentOutOfRangeException(nameof (offset));
    return ((int) arrData[offset] & 1 << bitPos) == 1 << bitPos;
  }

  [SecurityCritical]
  public static bool GetBit(IntPtr ptrData, int offset, int bitPos)
  {
    if (bitPos < 0 || bitPos > 7)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater than 7.");
    return ((int) Marshal.ReadByte(ptrData, offset) & 1 << bitPos) == 1 << bitPos;
  }

  public static string GetString16BitUpdateOffset(byte[] arrData, ref int offset)
  {
    int uint16 = (int) BiffRecordRaw.GetUInt16(arrData, offset);
    offset += 2;
    if (uint16 <= 0)
      return string.Empty;
    int iReadBytes;
    string string16BitUpdateOffset = BiffRecordRaw.GetString(arrData, offset, uint16, out iReadBytes);
    offset += iReadBytes;
    return string16BitUpdateOffset;
  }

  public static string GetStringUpdateOffset(byte[] arrData, ref int offset, int iStrLen)
  {
    if (iStrLen <= 0)
      return string.Empty;
    int iReadBytes;
    string stringUpdateOffset = BiffRecordRaw.GetString(arrData, offset, iStrLen, out iReadBytes);
    offset += iReadBytes;
    return stringUpdateOffset;
  }

  public static string GetStringByteLen(byte[] arrData, int offset)
  {
    int iStrLen = (int) BiffRecordRaw.GetByte(arrData, offset);
    return BiffRecordRaw.GetString(arrData, offset + 1, iStrLen);
  }

  public static string GetString(byte[] arrData, int offset, int iStrLen)
  {
    return BiffRecordRaw.GetString(arrData, offset, iStrLen, out int _);
  }

  public static string GetString(
    byte[] arrData,
    int offset,
    int iStrLen,
    out int iBytesInString,
    bool isByteCounted)
  {
    byte num = BiffRecordRaw.GetByte(arrData, offset);
    if ((num == (byte) 0 || isByteCounted ? iStrLen : 2 * iStrLen) + (offset + 1) > arrData.Length)
      throw new WrongBiffRecordDataException(string.Format("String and arrData array do not fit each other {0}."));
    if (num == (byte) 0)
    {
      iBytesInString = iStrLen;
      return BiffRecordRaw.LatinEncoding.GetString(BiffRecordRaw.GetBytes(arrData, offset + 1, iStrLen), 0, iStrLen);
    }
    iBytesInString = isByteCounted ? iStrLen : iStrLen * 2;
    return Encoding.Unicode.GetString(BiffRecordRaw.GetBytes(arrData, offset + 1, iBytesInString), 0, iBytesInString);
  }

  public static string GetUnkTypeString(
    byte[] arrData,
    int offset,
    int[] continuePos,
    out int length,
    out byte[] rich,
    out byte[] extended)
  {
    string empty = string.Empty;
    int num1 = 3;
    rich = (byte[]) null;
    extended = (byte[]) null;
    int offset1 = offset;
    ushort uint16 = BiffRecordRaw.GetUInt16(arrData, offset1);
    byte num2 = BiffRecordRaw.GetByte(arrData, offset1 + 2);
    bool flag1 = ((int) num2 & 1) == 1;
    bool flag2 = num2 == (byte) 8 || num2 == (byte) 9;
    bool flag3 = num2 == (byte) 4 || num2 == (byte) 5;
    bool flag4 = num2 == (byte) 12 || num2 == (byte) 13;
    int num3 = 3;
    short num4 = 0;
    if (flag2)
    {
      num4 = BiffRecordRaw.GetInt16(arrData, offset1 + 3);
      num3 = 5;
      extended = (byte[]) null;
      num1 += 2;
    }
    else if (flag3)
    {
      int int32 = BiffRecordRaw.GetInt32(arrData, offset1 + 3);
      num3 = 7;
      rich = (byte[]) null;
      int offset2 = num1 + 4;
      extended = BiffRecordRaw.GetBytes(arrData, offset2, int32);
      num1 = offset2 + int32;
    }
    else if (flag4)
    {
      num4 = BiffRecordRaw.GetInt16(arrData, offset1 + 3);
      int int32 = BiffRecordRaw.GetInt32(arrData, offset1 + 5);
      num3 = 9;
      int offset3 = num1 + 6;
      rich = BiffRecordRaw.GetBytes(arrData, offset3, (int) num4 * 4);
      int offset4 = offset3 + (int) num4 * 4;
      extended = BiffRecordRaw.GetBytes(arrData, offset4, int32);
      num1 = offset4 + int32;
    }
    int num5 = offset1 + num3;
    int num6 = 0;
    int iStartIndex = 0;
    while (num6 < (int) uint16)
    {
      int num7 = flag1 ? ((int) uint16 - num6) * 2 : (int) uint16 - num6;
      int num8 = BiffRecordRaw.FindNextBreak((IList<int>) continuePos, continuePos.Length, num5, ref iStartIndex) - num5;
      if (num7 <= num8)
      {
        empty += flag1 ? Encoding.Unicode.GetString(BiffRecordRaw.GetBytes(arrData, num5, num7), 0, num7) : BiffRecordRaw.LatinEncoding.GetString(BiffRecordRaw.GetBytes(arrData, num5, num7), 0, num7);
        num1 += num7;
        break;
      }
      if (num8 > 0)
      {
        empty += flag1 ? Encoding.Unicode.GetString(BiffRecordRaw.GetBytes(arrData, num5, num8), 0, num8) : BiffRecordRaw.LatinEncoding.GetString(BiffRecordRaw.GetBytes(arrData, num5, num8), 0, num8);
        num6 += flag1 ? num8 / 2 : num8;
      }
      if (arrData[num5 + num8] == (byte) 0 || arrData[num5 + num8] == (byte) 1)
      {
        flag1 = arrData[num5 + num8] == (byte) 1;
        ++num5;
        ++num1;
      }
      num5 += num8;
      num1 += num8;
    }
    if (flag2)
    {
      rich = BiffRecordRaw.GetBytes(arrData, offset + num1, (int) num4 * 4);
      num1 += (int) num4 * 4;
    }
    length = num1;
    return empty;
  }

  [CLSCompliant(false)]
  public static TAddr GetAddr(byte[] arrData, int offset)
  {
    return new TAddr()
    {
      FirstRow = (int) BiffRecordRaw.GetUInt16(arrData, offset),
      LastRow = (int) BiffRecordRaw.GetUInt16(arrData, offset + 2),
      FirstCol = (int) BiffRecordRaw.GetUInt16(arrData, offset + 4),
      LastCol = (int) BiffRecordRaw.GetUInt16(arrData, offset + 6)
    };
  }

  public static byte[] GetRPNData(byte[] arrData, int offset, int length)
  {
    if (length == 0)
      return new byte[0];
    List<byte> byteList = new List<byte>(length * 2);
    int num1 = 0;
    switch (BiffRecordRaw.GetByte(arrData, offset + num1))
    {
      case 32 /*0x20*/:
      case 64 /*0x40*/:
      case 96 /*0x60*/:
        int num2 = ((int) BiffRecordRaw.GetByte(arrData, offset + num1 + 1) + 1) * ((int) BiffRecordRaw.GetInt16(arrData, offset + num1 + 2) + 1) + 1;
        int num3 = 0;
        for (; num2 > 0; --num2)
        {
          switch (BiffRecordRaw.GetByte(arrData, offset + length + num3))
          {
            case 2:
              num3 += (int) BiffRecordRaw.GetInt16(arrData, offset + length + num3 + 1) + 4;
              break;
            case 4:
              num3 += 3;
              break;
            default:
              num3 += 9;
              break;
          }
        }
        for (int index = offset + num1; index < offset + length + num3; ++index)
          byteList.Add(arrData[index]);
        return byteList.ToArray();
      default:
        return BiffRecordRaw.GetBytes(arrData, offset, length);
    }
  }

  protected static int FindNextBreak(
    IList<int> arrBreaks,
    int iCount,
    int curPos,
    ref int iStartIndex)
  {
    for (int index = iStartIndex; index < iCount; ++index)
    {
      int arrBreak = arrBreaks[index];
      if (curPos <= arrBreak)
      {
        iStartIndex = index;
        return arrBreak;
      }
    }
    return -1;
  }

  [CLSCompliant(false)]
  public static void SetUInt16(byte[] arrData, int offset, ushort value)
  {
    byte num1 = (byte) ((uint) value & (uint) byte.MaxValue);
    byte num2 = (byte) ((int) value >> 8 & (int) byte.MaxValue);
    arrData[offset] = num1;
    arrData[offset + 1] = num2;
  }

  public static void SetBit(byte[] arrData, int offset, bool value, int bitPos)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (bitPos < 0 || bitPos > 7)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zero or greater than 7.");
    if (value)
      arrData[offset] |= (byte) (1 << bitPos);
    else
      arrData[offset] &= (byte) ~(1 << bitPos);
  }

  public static void SetInt16(byte[] arrData, int offset, short value)
  {
    Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) arrData, offset, 2);
  }

  public static void SetInt32(byte[] arrData, int offset, int value)
  {
    Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) arrData, offset, 4);
  }

  [CLSCompliant(false)]
  public static void SetUInt32(byte[] arrData, int offset, uint value)
  {
    Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) arrData, offset, 4);
  }

  public static void SetDouble(byte[] arrData, int offset, double value)
  {
    Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) arrData, offset, 8);
  }

  public static void SetStringNoLenUpdateOffset(byte[] arrData, ref int offset, string value)
  {
    switch (value)
    {
      case null:
        break;
      case "":
        break;
      default:
        byte[] bytes = Encoding.Unicode.GetBytes(value);
        arrData[offset] = (byte) 1;
        BiffRecordRaw.SetBytes(arrData, offset + 1, bytes, 0, bytes.Length);
        offset += bytes.Length + 1;
        break;
    }
  }

  public static void SetStringByteLen(byte[] arrData, int offset, string value)
  {
    arrData[offset] = (byte) value.Length;
    BiffRecordRaw.SetStringNoLen(arrData, offset + 1, value);
  }

  protected internal static void SetBytes(
    byte[] arrBuffer,
    int offset,
    byte[] value,
    int pos,
    int length)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (pos < 0)
      throw new ArgumentOutOfRangeException(nameof (pos), "Position cannot be zeroless.");
    if (length < 0)
      throw new ArgumentOutOfRangeException(nameof (length), "Length of data to copy must be greater then zero.");
    if (pos + length > value.Length)
      throw new ArgumentOutOfRangeException(nameof (value), "Position or length has wrong value.");
    Buffer.BlockCopy((Array) value, pos, (Array) arrBuffer, offset, length);
  }

  [CLSCompliant(false)]
  protected internal void SetBitInVar(ref ushort variable, bool value, int bitPos)
  {
    if (bitPos < 0 || bitPos > 15)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zero or greater than 7.");
    if (value)
      variable |= (ushort) (1 << bitPos);
    else
      variable &= (ushort) ~(1 << bitPos);
  }

  [CLSCompliant(false)]
  protected internal void SetBitInVar(ref uint variable, bool value, int bitPos)
  {
    if (bitPos < 0 || bitPos > 31 /*0x1F*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zero or greater than 7.");
    if (value)
      variable |= (uint) (1 << bitPos);
    else
      variable &= (uint) ~(1 << bitPos);
  }

  public int Get16BitStringSize(string strValue, bool isCompressed)
  {
    return strValue == null || strValue.Length == 0 ? 2 : 3 + (isCompressed ? Encoding.ASCII : Encoding.Unicode).GetByteCount(strValue);
  }

  public virtual void ClearData()
  {
  }

  public virtual bool IsEqual(BiffRecordRaw raw) => throw new NotImplementedException();

  public virtual void CopyTo(BiffRecordRaw raw) => throw new NotImplementedException();

  public void CheckTypeCode(TBIFFRecord typeCode)
  {
    if (this.TypeCode != typeCode)
      throw new ArgumentOutOfRangeException(typeCode.ToString() + " record was expected");
  }

  public static bool CompareArrays(
    byte[] array1,
    int iStartIndex1,
    byte[] array2,
    int iStartIndex2,
    int iLength)
  {
    int num = 0;
    int index1 = iStartIndex1;
    for (int index2 = iStartIndex2; num < iLength && index1 < array1.Length && index2 < array2.Length; ++index2)
    {
      if ((int) array1[index1] != (int) array2[index2])
        return false;
      ++num;
      ++index1;
    }
    return num == iLength && num != 0;
  }

  public static bool CompareArrays(byte[] array1, byte[] array2)
  {
    if (array1 == null && array2 == null)
      return true;
    if (array1 == null || array2 == null)
      return false;
    int length1 = array1.Length;
    int length2 = array2.Length;
    if (length1 != length2)
      return false;
    return length1 == 0 && length2 == 0 || BiffRecordRaw.CompareArrays(array1, 0, array2, 0, array1.Length);
  }

  internal void SetRecordCode(int code) => this.m_iCode = code;

  public virtual object Clone() => this.MemberwiseClone();

  public static Encoding LatinEncoding => BiffRecordRaw.s_latin1;

  public static byte[] CombineArrays(int iCombinedLength, List<byte[]> arrCombined)
  {
    if (arrCombined == null || arrCombined.Count == 0)
      return new byte[0];
    int count = arrCombined.Count;
    byte[] dst = new byte[iCombinedLength];
    int dstOffset = 0;
    for (int index = 0; index < count; ++index)
    {
      byte[] src = arrCombined[index];
      int length = src.Length;
      Buffer.BlockCopy((Array) src, 0, (Array) dst, dstOffset, length);
      dstOffset += length;
    }
    return dst;
  }

  public static string GetString(byte[] arrData, int iOffset, int iLength, out int iReadBytes)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset + iLength > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    if (iLength < 0)
      throw new ArgumentOutOfRangeException(nameof (iLength));
    byte num = arrData[iOffset];
    if ((num != (byte) 0 ? 2 * iLength : iLength) + (iOffset + 1) > arrData.Length)
      throw new WrongBiffRecordDataException(string.Format("String and m_data array do not fit each other"));
    Encoding encoding;
    if (num == (byte) 0)
    {
      iReadBytes = iLength;
      encoding = BiffRecordRaw.LatinEncoding;
    }
    else
    {
      iReadBytes = iLength * 2;
      encoding = Encoding.Unicode;
    }
    string str = encoding.GetString(arrData, iOffset + 1, iReadBytes);
    ++iReadBytes;
    return str;
  }

  public static int SetStringNoLen(byte[] arrData, int iOffset, string strValue)
  {
    if (strValue == null || strValue.Length == 0)
      return 0;
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    byte[] bytes = Encoding.Unicode.GetBytes(strValue);
    if (iOffset < 0 || iOffset + bytes.Length + 1 > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    arrData[iOffset] = (byte) 1;
    ++iOffset;
    bytes.CopyTo((Array) arrData, iOffset);
    return bytes.Length + 1;
  }

  public static void SetString16BitUpdateOffset(byte[] arrData, ref int offset, string value)
  {
    BiffRecordRaw.SetUInt16(arrData, offset, (ushort) value.Length);
    offset += 2;
    BiffRecordRaw.SetStringNoLenUpdateOffset(arrData, ref offset, value);
  }

  public static bool GetBitFromVar(byte btOptions, int bitPos)
  {
    return bitPos >= 0 && bitPos < 8 ? BiffRecordRaw.GetBitFromVar((int) btOptions, bitPos) : throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater 7.");
  }

  public static bool GetBitFromVar(short sOptions, int bitPos)
  {
    if (bitPos < 0 || bitPos >= 16 /*0x10*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater 15.");
    return bitPos == 15 ? sOptions < (short) 0 : BiffRecordRaw.GetBitFromVar((int) sOptions, bitPos);
  }

  [CLSCompliant(false)]
  public static bool GetBitFromVar(ushort usOptions, int bitPos)
  {
    return bitPos >= 0 && bitPos < 16 /*0x10*/ ? BiffRecordRaw.GetBitFromVar((int) usOptions, bitPos) : throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater 15.");
  }

  public static bool GetBitFromVar(int iOptions, int bitPos)
  {
    if (bitPos < 0 || bitPos >= 32 /*0x20*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater 31.");
    return bitPos == 31 /*0x1F*/ ? iOptions < 0 : (iOptions & 1 << bitPos) != 0;
  }

  [CLSCompliant(false)]
  public static bool GetBitFromVar(uint uiOptions, int bitPos)
  {
    if (bitPos < 0 || bitPos >= 32 /*0x20*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater 31.");
    return ((long) uiOptions & (long) (1 << bitPos)) != 0L;
  }

  public static int SetBit(int iValue, int bitPos, bool value)
  {
    if (bitPos < 0 || bitPos >= 32 /*0x20*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater 32.");
    if (bitPos == 31 /*0x1F*/)
    {
      iValue = Math.Abs(iValue);
      if (!value)
        iValue = -iValue;
    }
    else if (value)
      iValue |= 1 << bitPos;
    else
      iValue &= ~(1 << bitPos);
    return iValue;
  }

  public static int ReadArray(byte[] arrSource, int iOffset, byte[] arrDest)
  {
    if (arrSource == null)
      throw new ArgumentNullException(nameof (arrSource));
    int count = arrDest != null ? arrDest.Length : throw new ArgumentNullException(nameof (arrDest));
    Buffer.BlockCopy((Array) arrSource, iOffset, (Array) arrDest, 0, count);
    return iOffset + count;
  }
}
