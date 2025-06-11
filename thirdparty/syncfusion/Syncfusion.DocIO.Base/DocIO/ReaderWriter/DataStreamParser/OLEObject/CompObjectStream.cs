// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.CompObjectStream
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;

internal class CompObjectStream
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

  internal int Length
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

  internal CompObjectStream(Stream stream) => this.Parse((stream as MemoryStream).ToArray(), 0);

  internal CompObjectStream(OleObjectType oleType)
  {
    this.m_header = new CompObjectStream.CompObjHeader();
    switch (oleType)
    {
      case OleObjectType.AdobeAcrobatDocument:
        this.m_ansiUserTypeData = "Acrobat Document\0";
        this.m_reserved1Data = "AcroExch.Document.7\0";
        break;
      case OleObjectType.BitmapImage:
      case OleObjectType.MediaClip:
      case OleObjectType.Equation:
      case OleObjectType.GraphChart:
      case OleObjectType.Excel_97_2003_Worksheet:
      case OleObjectType.ExcelBinaryWorksheet:
      case OleObjectType.ExcelChart:
      case OleObjectType.ExcelMacroWorksheet:
      case OleObjectType.ExcelWorksheet:
      case OleObjectType.PowerPoint_97_2003_Presentation:
      case OleObjectType.PowerPoint_97_2003_Slide:
      case OleObjectType.PowerPointMacroPresentation:
      case OleObjectType.PowerPointMacroSlide:
      case OleObjectType.PowerPointPresentation:
      case OleObjectType.PowerPointSlide:
      case OleObjectType.Word_97_2003_Document:
      case OleObjectType.WordDocument:
      case OleObjectType.WordMacroDocument:
      case OleObjectType.MIDISequence:
      case OleObjectType.Package:
      case OleObjectType.VideoClip:
        this.m_ansiUserTypeData = OleTypeConvertor.ToString(oleType, true) + "\0";
        break;
      case OleObjectType.WaveSound:
        this.m_ansiUserTypeData = "Wave Sound\0";
        this.m_reserved1Data = "SoundRec\0";
        break;
    }
  }

  internal void Parse(byte[] arrData, int iOffset)
  {
    this.m_streamLength = arrData.Length;
    int length = 0;
    Encoding encoding = (Encoding) new ASCIIEncoding();
    UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
    this.m_header = new CompObjectStream.CompObjHeader();
    this.m_header.Parse(arrData, iOffset);
    iOffset += this.m_header.Length;
    if (arrData.Length > iOffset + 4)
      length = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (length > 0)
    {
      byte[] bytes = ByteConverter.ReadBytes(arrData, length, ref iOffset);
      this.m_ansiUserTypeData = encoding.GetString(bytes, 0, bytes.Length);
    }
    uint num = 0;
    if (arrData.Length > iOffset + 4)
      num = ByteConverter.ReadUInt32(arrData, ref iOffset);
    if (length > 0)
    {
      if (num == uint.MaxValue || num == 4294967294U)
      {
        byte[] bytes = ByteConverter.ReadBytes(arrData, 4, ref iOffset);
        this.m_ansiUserTypeData = encoding.GetString(bytes, 0, bytes.Length);
      }
      else if (num > 400U)
        throw new InvalidDataException("OLE stream is not valid");
    }
    if (arrData.Length > iOffset + 4)
      length = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (length > 0 && length <= 40)
    {
      byte[] bytes = ByteConverter.ReadBytes(arrData, length, ref iOffset);
      this.m_reserved1Data = encoding.GetString(bytes, 0, bytes.Length);
    }
    if (arrData.Length > iOffset + 4)
      this.m_unicodeMarker = ByteConverter.ReadUInt32(arrData, ref iOffset);
    if (this.m_unicodeMarker != 1907505652U)
      return;
    if (arrData.Length > iOffset + 4)
      length = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (length > 0)
    {
      byte[] bytes = ByteConverter.ReadBytes(arrData, length, ref iOffset);
      this.m_unicodeUserTypeData = unicodeEncoding.GetString(bytes, 0, bytes.Length);
    }
    if (arrData.Length > iOffset + 4)
      num = ByteConverter.ReadUInt32(arrData, ref iOffset);
    if (num > 0U)
    {
      if (num == uint.MaxValue || num == 4294967294U)
      {
        byte[] bytes = ByteConverter.ReadBytes(arrData, 4, ref iOffset);
        this.m_unicodeClipboardFormatData = unicodeEncoding.GetString(bytes, 0, bytes.Length);
      }
      else if (num > 400U)
        throw new InvalidDataException("OLE stream is not valid");
    }
    if (arrData.Length > iOffset + 4)
      length = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (length <= 0 || length > 40)
      return;
    byte[] bytes1 = ByteConverter.ReadBytes(arrData, length, ref iOffset);
    this.m_reserved2Data = unicodeEncoding.GetString(bytes1, 0, bytes1.Length);
  }

  internal int Save(byte[] arrData, int iOffset)
  {
    throw new NotImplementedException("Not implemented");
  }

  internal void SaveTo(Stream stream)
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

  private void WriteZeroByteArr(Stream stream, int byteLength)
  {
    byte[] buffer = new byte[byteLength];
    stream.Write(buffer, 0, byteLength);
  }

  private void WriteLengthPrefixedString(Stream stream, string data)
  {
    byte[] numArray = new byte[4];
    Encoding encoding = (Encoding) new ASCIIEncoding();
    int iOffset = 0;
    byte[] bytes = encoding.GetBytes(data);
    ByteConverter.WriteInt32(numArray, ref iOffset, bytes.Length);
    stream.Write(numArray, 0, numArray.Length);
    if (bytes.Length <= 0)
      return;
    stream.Write(bytes, 0, bytes.Length);
  }

  private class CompObjHeader
  {
    internal const int DEF_STRUCT_LEN = 28;
    internal const int DEF_RESERVED2_ARR_LEN = 20;
    internal int m_reserved1;
    internal int m_version;
    internal byte[] m_reserved2;

    internal int Length => 28;

    internal CompObjHeader()
    {
      this.m_reserved1 = -131071;
      this.m_version = 2563;
      this.m_reserved2 = new byte[20];
    }

    internal void Parse(byte[] arrData, int iOffset)
    {
      this.m_reserved1 = ByteConverter.ReadInt32(arrData, ref iOffset);
      this.m_version = ByteConverter.ReadInt32(arrData, ref iOffset);
      if (arrData.Length <= iOffset)
        return;
      this.m_reserved2 = ByteConverter.ReadBytes(arrData, 20, ref iOffset);
    }

    internal int Save(byte[] arrData, int iOffset)
    {
      ByteConverter.WriteInt32(arrData, ref iOffset, -131071);
      ByteConverter.WriteInt32(arrData, ref iOffset, 2563);
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
      ByteConverter.WriteBytes(arrData, ref iOffset, this.m_reserved2);
      return iOffset;
    }

    internal void SaveTo(Stream stream)
    {
      int count = 4;
      byte[] bytes1 = BitConverter.GetBytes(this.m_reserved1);
      stream.Write(bytes1, 0, count);
      byte[] bytes2 = BitConverter.GetBytes(this.m_version);
      stream.Write(bytes2, 0, count);
      if (this.m_reserved2 == null)
        stream.Write(new byte[20], 0, 20);
      else
        stream.Write(this.m_reserved2, 0, 20);
    }
  }
}
