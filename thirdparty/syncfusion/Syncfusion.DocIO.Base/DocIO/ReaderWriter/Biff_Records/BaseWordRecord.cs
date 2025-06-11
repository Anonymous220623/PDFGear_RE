// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BaseWordRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal abstract class BaseWordRecord
{
  private const int DEF_BITS_IN_BYTE = 8;
  private const int DEF_BITS_IN_SHORT = 16 /*0x10*/;
  private const int DEF_BITS_IN_INT = 32 /*0x20*/;

  internal BaseWordRecord()
  {
  }

  internal BaseWordRecord(byte[] data) => this.Parse(data);

  internal BaseWordRecord(byte[] arrData, int iOffset) => this.ParseBytes(arrData, iOffset);

  internal BaseWordRecord(byte[] arrData, int iOffset, int iCount)
  {
    this.Parse(arrData, iOffset, iCount);
  }

  internal BaseWordRecord(Stream stream, int iCount) => this.Parse(stream, iCount);

  internal virtual int Length
  {
    get
    {
      IDataStructure underlyingStructure = this.UnderlyingStructure;
      return underlyingStructure != null ? underlyingStructure.Length : 0;
    }
  }

  protected virtual IDataStructure UnderlyingStructure
  {
    get => throw new Exception("UnderlyingStructure of BiffRecord");
  }

  internal static bool GetBit(byte btOptions, int bitPos)
  {
    return bitPos >= 0 && bitPos < 8 ? BaseWordRecord.GetBit((int) btOptions, bitPos) : throw new ArgumentOutOfRangeException(nameof (bitPos), (object) bitPos, "Bit Position cannot be less than 0 or greater than 7.");
  }

  internal static bool GetBit(short sOptions, int bitPos)
  {
    if (bitPos < 0 || bitPos >= 16 /*0x10*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), (object) bitPos, "Bit Position cannot be less than 0 or greater 15.");
    return bitPos == 15 ? sOptions < (short) 0 : BaseWordRecord.GetBit((int) sOptions, bitPos);
  }

  internal static bool GetBit(int iOptions, int bitPos)
  {
    if (bitPos < 0 || bitPos >= 32 /*0x20*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), (object) bitPos, "Bit Position cannot be less than 0 or greater 31.");
    return bitPos == 31 /*0x1F*/ ? iOptions < 0 : (iOptions & 1 << bitPos) != 0;
  }

  internal static int GetBitsByMask(int value, int BitMask, int iStartBit)
  {
    return (value & BitMask) >> iStartBit;
  }

  internal static uint GetBitsByMask(uint value, int BitMask, int iStartBit)
  {
    return (uint) (((long) value & (long) BitMask) >> iStartBit);
  }

  internal static int SetBit(int iValue, int bitPos, bool value)
  {
    if (bitPos < 0 || bitPos >= 32 /*0x20*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), (object) bitPos, "Bit Position cannot be less than 0 or greater 32.");
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

  internal static int SetBitsByMask(int destination, int BitMask, int value)
  {
    destination &= ~BitMask;
    destination += value & BitMask;
    return destination;
  }

  internal static int SetBitsByMask(int destination, int BitMask, int iStartBit, int value)
  {
    destination &= ~BitMask;
    destination += value << iStartBit & BitMask;
    return destination;
  }

  internal static uint SetBitsByMask(uint destination, int BitMask, int value)
  {
    destination &= (uint) ~BitMask;
    destination += (uint) (value & BitMask);
    return destination;
  }

  internal static bool GetBit(uint uiOptions, int bitPos)
  {
    if (bitPos < 0 || bitPos > 32 /*0x20*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position cannot be less than 0 or greater 31.");
    return ((long) uiOptions & (long) (1 << bitPos)) != 0L;
  }

  internal static uint SetBit(uint uiValue, int bitPos, bool value)
  {
    if (bitPos < 0 || bitPos >= 32 /*0x20*/)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zeroless or greater 32.");
    if (value)
      uiValue |= (uint) (1 << bitPos);
    else
      uiValue &= (uint) ~(1 << bitPos);
    return uiValue;
  }

  internal static ushort ReadUInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    return stream.Read(buffer, 0, 2) == 2 ? BitConverter.ToUInt16(buffer, 0) : throw new StreamReadException();
  }

  internal static uint ReadUInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    return stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToUInt32(buffer, 0) : throw new StreamReadException();
  }

  internal static short ReadInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    return stream.Read(buffer, 0, 2) == 2 ? BitConverter.ToInt16(buffer, 0) : throw new StreamReadException();
  }

  internal static int ReadInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    return stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToInt32(buffer, 0) : throw new StreamReadException();
  }

  internal static ushort ReadUInt16(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 2)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less 0 and greater arrData.Length - Constants.BytesInWord");
    return BitConverter.ToUInt16(arrData, iOffset);
  }

  internal static ushort ReadUInt16(byte[] arrData, ref int iOffset)
  {
    ushort num = BaseWordRecord.ReadUInt16(arrData, iOffset);
    iOffset += 2;
    return num;
  }

  internal static string ReadString(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    int count = (int) BaseWordRecord.ReadUInt16(stream) * 2;
    if ((long) count + stream.Position > stream.Length)
      return string.Empty;
    byte[] numArray = new byte[count];
    if (stream.Read(numArray, 0, count) != count)
      throw new Exception("Unable to read required data from the stream");
    return Encoding.Unicode.GetString(numArray);
  }

  internal static void WriteString(Stream stream, string str)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    BaseWordRecord.WriteUInt16(stream, (ushort) (Encoding.Unicode.GetByteCount(str) / 2));
    byte[] bytes = Encoding.Unicode.GetBytes(str);
    stream.Write(bytes, 0, bytes.Length);
  }

  internal static string ReadString(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 2)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less 0 and greater arrData.Length - Constants.BytesInWord");
    ushort count = BaseWordRecord.ReadUInt16(arrData, iOffset);
    iOffset += 2;
    return Encoding.Unicode.GetString(arrData, iOffset, (int) count);
  }

  internal static string ReadString(byte[] arrData, int iOffset, ushort usCount)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 2)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less 0 and greater arrData.Length - Constants.BytesInWord");
    return Encoding.Unicode.GetString(arrData, iOffset, (int) usCount);
  }

  internal static string GetZeroTerminatedString(byte[] arrData, int iOffset, out int iEndPos)
  {
    byte num = arrData[iOffset];
    iOffset += 2;
    string empty = string.Empty;
    iEndPos = iOffset + (int) num * 2;
    if (num != (byte) 0)
      empty = Encoding.Unicode.GetString(arrData, iOffset, (int) num * 2);
    for (int index = iEndPos; index < arrData.Length - 1; ++index)
    {
      if (arrData[index] == (byte) 0 && arrData[index + 1] == (byte) 0)
      {
        iEndPos += 2;
        return empty;
      }
    }
    throw new Exception("Stored string should be zero-ended");
  }

  internal static byte[] ToZeroTerminatedArray(string str)
  {
    byte[] bytes = new byte[str.Length * 2 + 4];
    bytes[0] = (byte) str.Length;
    Encoding.Unicode.GetBytes(str.ToCharArray(), 0, str.Length, bytes, 2);
    return bytes;
  }

  internal static void WriteUInt16(byte[] arrData, ushort usValue, ref int iOffset)
  {
    iOffset = BaseWordRecord.WriteBytes(arrData, BitConverter.GetBytes(usValue), iOffset);
  }

  internal static void WriteUInt32(byte[] arrData, uint uintValue, ref int iOffset)
  {
    iOffset = BaseWordRecord.WriteBytes(arrData, BitConverter.GetBytes(uintValue), iOffset);
  }

  internal static void WriteUInt32(Stream stream, uint value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }

  internal static void WriteInt32(Stream stream, int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }

  internal static void WriteInt16(Stream stream, short value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }

  internal static void WriteUInt16(Stream stream, ushort value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }

  internal static void WriteString(byte[] arrData, string strValue, ref int iOffset)
  {
    Encoding unicode = Encoding.Unicode;
    iOffset = BaseWordRecord.WriteBytes(arrData, unicode.GetBytes(strValue), iOffset);
  }

  internal static int WriteBytes(byte[] arrData, byte[] bytes, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 2)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less 0 and greater arrData.Length - Constants.BytesInWord");
    bytes.CopyTo((Array) arrData, iOffset);
    return iOffset + bytes.Length;
  }

  internal byte[] ReadBytes(Stream stream, int i)
  {
    byte[] buffer = new byte[i];
    if (stream.Read(buffer, 0, i) != i)
      throw new StreamReadException();
    return buffer;
  }

  internal virtual void Parse(byte[] data) => this.ParseBytes(data, 0);

  internal virtual void ParseBytes(byte[] arrData, int iOffset)
  {
    this.Parse(arrData, iOffset, arrData.Length - iOffset);
  }

  internal virtual void Parse(byte[] arrData, int iOffset, int iCount)
  {
    IDataStructure underlyingStructure = this.UnderlyingStructure;
    if (underlyingStructure == null)
      throw new ArgumentNullException("UnderlyingStructure");
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    if (iCount < 0)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    if (iOffset + iCount > arrData.Length)
      throw new ArgumentOutOfRangeException("iOffset + iCount");
    MemoryConverter.Instance.Copy(arrData, iOffset, iCount, (object) underlyingStructure);
  }

  internal virtual void Parse(Stream stream, int iCount)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] numArray = iCount >= 0 ? new byte[iCount] : throw new ArgumentOutOfRangeException("iCount cannot be less than 0");
    if (stream.Read(numArray, 0, iCount) != iCount)
      throw new Exception("Couldn't read required bytes from the stream");
    this.Parse(numArray, 0, iCount);
  }

  internal virtual int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    int iLength = arrData.Length - iOffset;
    MemoryConverter.Instance.Copy((object) (this.UnderlyingStructure ?? throw new ArgumentNullException("UnderlyingStructure")), arrData, iOffset, iLength);
    return iLength;
  }

  internal virtual int Save(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    int length = this.Length;
    byte[] numArray = length >= 0 ? new byte[length] : throw new ArgumentOutOfRangeException("iLength");
    this.Save(numArray, 0);
    stream.Write(numArray, 0, length);
    return length;
  }

  internal virtual void Close()
  {
  }
}
