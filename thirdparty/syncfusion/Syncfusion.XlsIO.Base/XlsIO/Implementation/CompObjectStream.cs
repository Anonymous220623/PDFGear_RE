// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CompObjectStream
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.CompoundFile.XlsIO.Native;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class CompObjectStream : DataStructure
{
  private const int DEF_STREAM_SIZE = 93;
  private const int DEF_MARKER_OR_LENGTH4 = 400;
  private const int DEF_MARKER_OR_LENGTH5 = 40;
  private const uint DEF_UNICODE_MARKER = 1907505652;
  private int m_streamLength;
  private CompObjectStream.CompObjHeader m_header;
  private string m_ansiUserTypeData = string.Empty;
  private string m_ansiClipboardFormatData = string.Empty;
  private string m_reserved1Data = string.Empty;
  private uint m_unicodeMarker = 1907505652;
  private string m_unicodeUserTypeData = string.Empty;
  private string m_unicodeClipboardFormatData = string.Empty;
  private string m_reserved2Data = string.Empty;

  internal override int Length
  {
    get
    {
      if (this.m_streamLength == 0)
        this.m_streamLength = 93;
      return this.m_streamLength;
    }
  }

  internal string ObjectType => this.m_ansiUserTypeData;

  internal string ObjectTypeReserved => this.m_reserved1Data;

  internal CompObjectStream(CompoundStream compStream)
  {
    byte[] numArray = new byte[compStream.Length];
    compStream.Read(numArray, 0, numArray.Length);
    this.Parse(numArray, 0);
  }

  internal CompObjectStream()
  {
    this.m_header = new CompObjectStream.CompObjHeader();
    this.m_ansiUserTypeData = "Package\0";
  }

  internal override void Parse(byte[] arrData, int iOffset)
  {
    this.m_streamLength = arrData.Length;
    Encoding encoding = (Encoding) new ASCIIEncoding();
    UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
    this.m_header = new CompObjectStream.CompObjHeader();
    this.m_header.Parse(arrData, iOffset);
    iOffset += this.m_header.Length;
    int length1 = DataStructure.ReadInt32(arrData, ref iOffset);
    if (length1 > 0)
    {
      byte[] bytes = DataStructure.ReadBytes(arrData, length1, ref iOffset);
      this.m_ansiUserTypeData = encoding.GetString(bytes, 0, bytes.Length);
    }
    uint num1 = DataStructure.ReadUInt32(arrData, ref iOffset);
    if (length1 > 0)
    {
      if (num1 == uint.MaxValue || num1 == 4294967294U)
      {
        byte[] bytes = DataStructure.ReadBytes(arrData, 4, ref iOffset);
        this.m_ansiUserTypeData = encoding.GetString(bytes, 0, bytes.Length);
      }
      else if (num1 > 400U)
        throw new InvalidDataException("OLE stream in not valid");
    }
    int length2 = DataStructure.ReadInt32(arrData, ref iOffset);
    if (length2 > 0 && length2 <= 40)
    {
      byte[] bytes = DataStructure.ReadBytes(arrData, length2, ref iOffset);
      this.m_reserved1Data = encoding.GetString(bytes, 0, bytes.Length);
    }
    this.m_unicodeMarker = DataStructure.ReadUInt32(arrData, ref iOffset);
    if (this.m_unicodeMarker != 1907505652U)
      return;
    int length3 = DataStructure.ReadInt32(arrData, ref iOffset);
    if (length3 > 0)
    {
      byte[] bytes = DataStructure.ReadBytes(arrData, length3, ref iOffset);
      this.m_unicodeUserTypeData = unicodeEncoding.GetString(bytes, 0, bytes.Length);
    }
    uint num2 = DataStructure.ReadUInt32(arrData, ref iOffset);
    if (num2 > 0U)
    {
      if (num2 == uint.MaxValue || num2 == 4294967294U)
      {
        byte[] bytes = DataStructure.ReadBytes(arrData, 4, ref iOffset);
        this.m_unicodeClipboardFormatData = unicodeEncoding.GetString(bytes, 0, bytes.Length);
      }
      else if (num2 > 400U)
        throw new InvalidDataException("OLE stream in not valid");
    }
    int length4 = DataStructure.ReadInt32(arrData, ref iOffset);
    if (length4 <= 0 || length4 > 40)
      return;
    byte[] bytes1 = DataStructure.ReadBytes(arrData, length4, ref iOffset);
    this.m_reserved2Data = unicodeEncoding.GetString(bytes1, 0, bytes1.Length);
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    throw new NotImplementedException("Not implemented");
  }

  internal void SaveTo(StgStream stream)
  {
    int byteLength = 4;
    this.m_header.SaveTo(stream);
    this.WriteLengthPrefixedString(stream, this.m_ansiUserTypeData);
    this.WriteLengthPrefixedString(stream, this.m_ansiClipboardFormatData);
    this.WriteLengthPrefixedString(stream, this.m_reserved1Data);
    this.WriteZeroByteArr(stream, byteLength);
    this.WriteZeroByteArr(stream, byteLength);
    this.WriteZeroByteArr(stream, byteLength);
    this.WriteZeroByteArr(stream, byteLength);
  }

  private void WriteZeroByteArr(StgStream stream, int byteLength)
  {
    byte[] buffer = new byte[byteLength];
    stream.Write(buffer, 0, byteLength);
  }

  private void WriteLengthPrefixedString(StgStream stream, string data)
  {
    byte[] numArray = new byte[4];
    Encoding encoding = (Encoding) new ASCIIEncoding();
    int iOffset = 0;
    byte[] bytes = encoding.GetBytes(data);
    DataStructure.WriteInt32(numArray, ref iOffset, bytes.Length);
    stream.Write(numArray, 0, numArray.Length);
    if (bytes.Length <= 0)
      return;
    stream.Write(bytes, 0, bytes.Length);
  }

  private class CompObjHeader : DataStructure
  {
    internal const int DEF_STRUCT_LEN = 28;
    internal const int DEF_RESERVED2_ARR_LEN = 20;
    internal int m_reserved1;
    internal int m_version;
    internal byte[] m_reserved2;

    internal override int Length => 28;

    internal CompObjHeader()
    {
      this.m_reserved1 = -131071;
      this.m_version = 2563;
      this.m_reserved2 = new byte[20];
    }

    internal override void Parse(byte[] arrData, int iOffset)
    {
      this.m_reserved1 = DataStructure.ReadInt32(arrData, ref iOffset);
      this.m_version = DataStructure.ReadInt32(arrData, ref iOffset);
      this.m_reserved2 = DataStructure.ReadBytes(arrData, 20, ref iOffset);
    }

    internal override int Save(byte[] arrData, int iOffset)
    {
      DataStructure.WriteInt32(arrData, ref iOffset, -131071);
      DataStructure.WriteInt32(arrData, ref iOffset, 2563);
      this.m_reserved2 = new byte[20]
      {
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        (byte) 101,
        (byte) 202,
        (byte) 1,
        (byte) 184,
        (byte) 252,
        (byte) 161,
        (byte) 208 /*0xD0*/,
        (byte) 17,
        (byte) 133,
        (byte) 173,
        (byte) 68,
        (byte) 69,
        (byte) 83,
        (byte) 84,
        (byte) 0,
        (byte) 0
      };
      DataStructure.WriteBytes(arrData, ref iOffset, this.m_reserved2);
      return iOffset;
    }

    internal void SaveTo(StgStream stream)
    {
      byte[] bytes1 = BitConverter.GetBytes(this.m_reserved1);
      stream.Write(bytes1, 0, 4);
      byte[] bytes2 = BitConverter.GetBytes(this.m_version);
      stream.Write(bytes2, 0, 4);
      if (this.m_reserved2 == null)
        stream.Write(new byte[20], 0, 20);
      else
        stream.Write(this.m_reserved2, 0, 20);
    }
  }
}
