// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Decoder.JpegDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Decoder;

internal class JpegDecoder : ImageDecoder
{
  private const ushort c_SoiMarker = 55551;
  private const ushort c_JfifMarker = 57599;
  private const ushort c_SosMarker = 56063;
  private const ushort c_EoiMarker = 55807;
  private const ushort c_Sof0Marker = 192 /*0xC0*/;
  private const ushort c_Sof1Marker = 193;
  private const ushort c_Sof2Marker = 194;
  private const ushort c_Sof3Marker = 195;
  private const ushort c_Sof5Marker = 197;
  private const ushort c_Sof6Marker = 198;
  private const ushort c_Sof7Marker = 199;
  private const ushort c_Sof9Marker = 201;
  private const ushort c_Sof10Marker = 202;
  private const ushort c_Sof11Marker = 203;
  private const ushort c_Sof13Marker = 205;
  private const ushort c_Sof14Marker = 206;
  private const ushort c_Sof15Marker = 207;
  private byte[] m_jpegHeader = new byte[2]
  {
    byte.MaxValue,
    (byte) 216
  };
  private PdfStream m_imageStream;
  private bool m_isContainsLittleEndian;
  private readonly byte[] m_jpegSegmentPreambleBytes = new byte[29]
  {
    (byte) 104,
    (byte) 116,
    (byte) 116,
    (byte) 112 /*0x70*/,
    (byte) 58,
    (byte) 47,
    (byte) 47,
    (byte) 110,
    (byte) 115,
    (byte) 46,
    (byte) 97,
    (byte) 100,
    (byte) 111,
    (byte) 98,
    (byte) 101,
    (byte) 46,
    (byte) 99,
    (byte) 111,
    (byte) 109,
    (byte) 47,
    (byte) 120,
    (byte) 97,
    (byte) 112 /*0x70*/,
    (byte) 47,
    (byte) 49,
    (byte) 46,
    (byte) 48 /*0x30*/,
    (byte) 47,
    (byte) 0
  };
  private bool m_enableMetadata;
  private int m_noOfComponents = -1;

  public JpegDecoder(Stream stream)
  {
    this.InternalStream = stream;
    this.Format = ImageType.Jpeg;
    this.Initialize();
  }

  public JpegDecoder(Stream stream, bool enableMetadata)
  {
    this.InternalStream = stream;
    this.Format = ImageType.Jpeg;
    this.m_enableMetadata = enableMetadata;
    this.Initialize();
  }

  protected override void Initialize()
  {
    this.InternalStream.Position = 0L;
    byte[] buffer = new byte[this.InternalStream.Length];
    this.InternalStream.Read(buffer, 0, buffer.Length);
    this.ImageData = buffer;
    this.ReaderHeader();
  }

  private void ReaderHeader()
  {
    this.InternalStream.Position = 0L;
    this.BitsPerComponent = 8;
    byte[] buffer = new byte[this.InternalStream.Length];
    this.InternalStream.Read(buffer, 0, buffer.Length);
    long index = 4;
    bool flag = false;
    long num1 = (long) ((int) buffer[index] * 256 /*0x0100*/ + (int) buffer[index + 1L]);
    while (index < (long) buffer.Length)
    {
      index += num1;
      if (index < (long) buffer.Length && this.m_enableMetadata)
      {
        if (buffer[index + 1L] == (byte) 192 /*0xC0*/)
        {
          this.Width = (int) buffer[index + 7L] * 256 /*0x0100*/ + (int) buffer[index + 8L];
          this.Height = (int) buffer[index + 5L] * 256 /*0x0100*/ + (int) buffer[index + 6L];
          this.m_noOfComponents = (int) buffer[index + 9L];
          if (this.Width != 0 && this.Height != 0)
            return;
        }
        else
        {
          byte num2 = buffer[index + 1L];
          index += 2L;
          if (index >= (long) buffer.Length)
            return;
          num1 = (long) ((int) buffer[index] * 256 /*0x0100*/ + (int) buffer[index + 1L]);
          if (num2 == (byte) 225 && this.m_enableMetadata)
            this.ReadXmpSegment(new MemoryStream(buffer, (int) index + 2, (int) num1 - 2).ToArray());
        }
      }
      else
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    this.InternalStream.Position = 0L;
    StreamExtensions.Skip(this.InternalStream, 2);
    this.ReadExceededJPGImage(this.InternalStream);
  }

  private void ReadXmpSegment(byte[] bytes)
  {
    if (!this.IsXmpSegment(bytes))
      return;
    int length = this.m_jpegSegmentPreambleBytes.Length;
    if (this.MetadataStream == null)
      this.MetadataStream = new MemoryStream();
    this.MetadataStream.Write(bytes, length, bytes.Length - length);
  }

  private bool IsXmpSegment(byte[] bytes)
  {
    int length = this.m_jpegSegmentPreambleBytes.Length;
    bool flag = true;
    for (int index = 0; index < length; ++index)
    {
      if ((int) bytes[index] != (int) this.m_jpegSegmentPreambleBytes[index])
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  internal override PdfStream GetImageDictionary()
  {
    this.m_imageStream = new PdfStream();
    this.m_imageStream.InternalStream = new MemoryStream(this.ImageData);
    this.m_imageStream.Compress = false;
    this.m_imageStream["Type"] = (IPdfPrimitive) new PdfName("XObject");
    this.m_imageStream["Subtype"] = (IPdfPrimitive) new PdfName("Image");
    this.m_imageStream["Width"] = (IPdfPrimitive) new PdfNumber(this.Width);
    this.m_imageStream["Height"] = (IPdfPrimitive) new PdfNumber(this.Height);
    this.m_imageStream["BitsPerComponent"] = (IPdfPrimitive) new PdfNumber(this.BitsPerComponent);
    this.m_imageStream["Filter"] = (IPdfPrimitive) new PdfName("DCTDecode");
    this.m_imageStream["ColorSpace"] = (IPdfPrimitive) new PdfName(this.GetColorSpace());
    this.m_imageStream["DecodeParms"] = (IPdfPrimitive) this.GetDecodeParams();
    return this.m_imageStream;
  }

  private bool CheckForExifData(out int ImageOrientation)
  {
    ImageOrientation = 0;
    Stream internalStream = this.InternalStream;
    BinaryReader reader = new BinaryReader(internalStream);
    if (this.ConvertToUShort(this.ReadJpegBytes(2, reader)) != (ushort) 65496)
      return false;
    byte num1 = 0;
    byte num2;
    while ((num2 = reader.ReadByte()) == byte.MaxValue && (num1 = reader.ReadByte()) != (byte) 225)
    {
      int offset = (int) this.ConvertToUShort(this.ReadJpegBytes(2, reader)) - 2;
      long num3 = internalStream.Position + (long) offset;
      internalStream.Seek((long) offset, SeekOrigin.Current);
      if (internalStream.Position != num3 || internalStream.Position > internalStream.Length)
        return false;
    }
    if (num2 != byte.MaxValue || num1 != (byte) 225)
      return false;
    int num4 = (int) this.ConvertToUShort(this.ReadJpegBytes(2, reader));
    byte[] bytes1 = this.ReadJpegBytes(4, reader);
    if (Encoding.UTF8.GetString(bytes1, 0, bytes1.Length) != "Exif" || this.ConvertToUShort(this.ReadJpegBytes(2, reader)) != (ushort) 0)
      return false;
    long position = internalStream.Position;
    byte[] bytes2 = this.ReadJpegBytes(2, reader);
    this.m_isContainsLittleEndian = Encoding.UTF8.GetString(bytes2, 0, bytes2.Length) == "II";
    if (this.ConvertToUShort(this.ReadJpegBytes(2, reader)) != (ushort) 42)
      return false;
    uint num5 = this.ConvertToUint(this.ReadJpegBytes(4, reader));
    internalStream.Position = (long) num5 + position;
    ushort num6 = this.ConvertToUShort(this.ReadJpegBytes(2, reader));
    long num7 = 0;
    for (ushort index = 0; (int) index < (int) num6; ++index)
    {
      if (this.ConvertToUShort(this.ReadJpegBytes(2, reader)) == (ushort) 274)
        num7 = internalStream.Position - 2L;
      internalStream.Seek(10L, SeekOrigin.Current);
    }
    if (num7 >= 0L)
    {
      internalStream.Position = num7;
      if (this.ConvertToUShort(this.ReadJpegBytes(2, reader)) != (ushort) 274)
        return false;
      int num8 = (int) this.ConvertToUShort(this.ReadJpegBytes(2, reader));
      int num9 = (int) this.ConvertToUint(this.ReadJpegBytes(4, reader));
      byte[] numArray = this.ReadJpegBytes(4, reader);
      int num10 = 0;
      foreach (byte num11 in numArray)
      {
        if (num11 != (byte) 0)
        {
          num10 = (int) num11;
          break;
        }
      }
      if (num10 == 0)
        return false;
      ImageOrientation = num10;
    }
    return true;
  }

  private byte[] ReadJpegBytes(int byteCount, BinaryReader reader)
  {
    byte[] numArray = reader.ReadBytes(byteCount);
    if (numArray.Length != byteCount)
      throw new EndOfStreamException();
    return numArray;
  }

  private ushort ConvertToUShort(byte[] data)
  {
    if (this.m_isContainsLittleEndian != BitConverter.IsLittleEndian)
      Array.Reverse((Array) data);
    return BitConverter.ToUInt16(data, 0);
  }

  private uint ConvertToUint(byte[] data)
  {
    if (this.m_isContainsLittleEndian != BitConverter.IsLittleEndian)
      Array.Reverse((Array) data);
    return BitConverter.ToUInt32(data, 0);
  }

  private string GetColorSpace()
  {
    string colorSpace = "DeviceRGB";
    if (this.m_noOfComponents == 1 || this.m_noOfComponents == 3 || this.m_noOfComponents == 4)
    {
      switch (this.m_noOfComponents)
      {
        case 1:
          return "DeviceGray";
        case 3:
          return "DeviceRGB";
        case 4:
          return "DeviceCMYK";
      }
    }
    else
    {
      int startIndex1 = 0;
      int num1 = 2;
      if (BitConverter.ToUInt16(this.ImageData, startIndex1) == (ushort) 55551)
      {
        int startIndex2 = startIndex1 + num1;
        if ((ushort) ((uint) BitConverter.ToUInt16(this.ImageData, startIndex2) & 61695U) == (ushort) 57599)
        {
          ushort uint16_1;
          do
          {
            int startIndex3 = startIndex2 + num1;
            int uint16_2 = (int) BitConverter.ToUInt16(this.ImageData, startIndex3);
            int num2 = uint16_2 >> 8 | uint16_2 << 8 & (int) ushort.MaxValue;
            startIndex2 = startIndex3 + num2;
            uint16_1 = BitConverter.ToUInt16(this.ImageData, startIndex2);
            if (uint16_1 == (ushort) 56063)
            {
              startIndex2 += num1 + 2;
              switch (this.ImageData[startIndex2])
              {
                case 1:
                  return "DeviceGray";
                case 3:
                  return "DeviceRGB";
                case 4:
                  return "DeviceCMYK";
                default:
                  continue;
              }
            }
          }
          while (uint16_1 != (ushort) 55807);
        }
      }
    }
    return colorSpace;
  }

  private void ReadExceededJPGImage(Stream stream)
  {
    bool flag = true;
    while (flag)
    {
      switch (this.GetMarker(stream))
      {
        case 192 /*0xC0*/:
        case 193:
        case 194:
        case 195:
        case 197:
        case 198:
        case 199:
        case 201:
        case 202:
        case 203:
        case 205:
        case 206:
        case 207:
          stream.ReadByte();
          stream.ReadByte();
          stream.ReadByte();
          this.Height = this.ReadNextTwoBytes(stream);
          this.Width = this.ReadNextTwoBytes(stream);
          this.m_noOfComponents = stream.ReadByte();
          flag = false;
          continue;
        default:
          this.SkipStream(stream);
          continue;
      }
    }
  }

  private ushort GetMarker(Stream stream)
  {
    int num1 = 0;
    for (int index = stream.ReadByte(); index != (int) byte.MaxValue; index = stream.ReadByte())
      ++num1;
    int num2;
    do
    {
      num2 = (int) (ushort) stream.ReadByte();
    }
    while (num2 == (int) byte.MaxValue);
    if (num1 != 0)
      throw new Exception("Error decoding JPEG image");
    return (ushort) Convert.ToInt16(num2);
  }

  private void SkipStream(Stream stream)
  {
    int num = this.ReadNextTwoBytes(stream);
    if (num < 2)
      throw new Exception("Error decoding JPEG image");
    for (int index = num - 2; index > 0; --index)
      stream.ReadByte();
  }

  private int ReadNextTwoBytes(Stream stream) => stream.ReadByte() << 8 | stream.ReadByte();

  private PdfDictionary GetDecodeParams()
  {
    return new PdfDictionary()
    {
      ["Columns"] = (IPdfPrimitive) new PdfNumber(this.Width),
      ["BlackIs1"] = (IPdfPrimitive) new PdfBoolean(true),
      ["K"] = (IPdfPrimitive) new PdfNumber(-1),
      ["Predictor"] = (IPdfPrimitive) new PdfNumber(15),
      ["BitsPerComponent"] = (IPdfPrimitive) new PdfNumber(this.BitsPerComponent)
    };
  }
}
