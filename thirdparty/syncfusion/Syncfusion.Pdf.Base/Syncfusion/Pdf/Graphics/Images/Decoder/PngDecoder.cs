// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Decoder.PngDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Decoder;

internal class PngDecoder : ImageDecoder
{
  private static float[] m_decode = new float[6]
  {
    0.0f,
    1f,
    0.0f,
    1f,
    0.0f,
    1f
  };
  private int m_currentChunkLength;
  private PngDecoder.PngHeader m_header;
  private bool m_bDecodeIdat;
  private bool m_issRGB;
  private int m_bitsPerPixel;
  private long m_idatLength;
  private int m_colors;
  private int m_inputBands;
  private int m_bytesPerPixel;
  private Stream m_iDatStream;
  internal PdfArray m_colorSpace;
  internal bool m_isDecode;
  private byte[] m_maskData;
  private byte[] m_alpha;
  private bool m_shades;
  private Stream m_dataStream;
  private bool m_ideateDecode = true;
  private bool m_enableMetadata;
  private int transparentPixel;
  private int transparentPixelId;

  public PngDecoder(Stream stream)
  {
    this.InternalStream = stream;
    this.Format = ImageType.Png;
    this.Initialize();
  }

  public PngDecoder(Stream stream, bool enableMetadata)
  {
    this.m_enableMetadata = enableMetadata;
    this.InternalStream = stream;
    this.Format = ImageType.Png;
    this.Initialize();
  }

  protected override void Initialize()
  {
    PngDecoder.PngChunkTypes header;
    while (this.ReadNextchunk(out header))
    {
      switch (header)
      {
        case PngDecoder.PngChunkTypes.IHDR:
          this.ReadHeader();
          continue;
        case PngDecoder.PngChunkTypes.PLTE:
          this.ReadPLTE();
          continue;
        case PngDecoder.PngChunkTypes.IDAT:
          this.ReadImageData();
          continue;
        case PngDecoder.PngChunkTypes.IEND:
          this.DecodeImageData();
          continue;
        case PngDecoder.PngChunkTypes.bKGD:
        case PngDecoder.PngChunkTypes.cHRM:
        case PngDecoder.PngChunkTypes.gAMA:
        case PngDecoder.PngChunkTypes.hIST:
        case PngDecoder.PngChunkTypes.pHYs:
        case PngDecoder.PngChunkTypes.sBIT:
        case PngDecoder.PngChunkTypes.tIME:
        case PngDecoder.PngChunkTypes.iCCP:
        case PngDecoder.PngChunkTypes.Unknown:
          this.IgnoreChunk();
          continue;
        case PngDecoder.PngChunkTypes.tEXt:
        case PngDecoder.PngChunkTypes.iTXt:
          if (this.m_enableMetadata)
          {
            byte[] buffer1 = StreamExtensions.ReadByte(this.InternalStream, this.m_currentChunkLength);
            using (MemoryStream input = new MemoryStream(buffer1))
            {
              BinaryReader reader = new BinaryReader((Stream) input);
              byte[] bytes = this.ReadNullTerminatedBytes(79, reader);
              string str = Encoding.GetEncoding("ISO-8859-1").GetString(bytes, 0, bytes.Length);
              if (str == "XML:com.adobe.xmp")
              {
                sbyte num1 = (sbyte) reader.ReadByte();
                sbyte num2 = (sbyte) reader.ReadByte();
                byte[] numArray1 = this.ReadNullTerminatedBytes(buffer1.Length, reader);
                byte[] numArray2 = this.ReadNullTerminatedBytes(buffer1.Length, reader);
                int num3 = buffer1.Length - str.Length - 5 - numArray1.Length - numArray2.Length;
                switch (num1)
                {
                  case 0:
                    byte[] buffer2 = this.ReadNullTerminatedBytes(num3, reader);
                    if (this.MetadataStream == null)
                      this.MetadataStream = new MemoryStream();
                    this.MetadataStream.Write(buffer2, 0, buffer2.Length);
                    continue;
                  case 1:
                    if (num2 == (sbyte) 0)
                    {
                      if (this.MetadataStream == null)
                        this.MetadataStream = new MemoryStream();
                      this.MetadataStream.Write(buffer1, buffer1.Length - num3, num3);
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
              else
                continue;
            }
          }
          else
          {
            this.IgnoreChunk();
            continue;
          }
        case PngDecoder.PngChunkTypes.tRNS:
          this.ReadTRNS();
          continue;
        case PngDecoder.PngChunkTypes.zTXt:
          if (this.m_enableMetadata)
          {
            this.ReadZTextMetadata(StreamExtensions.ReadByte(this.InternalStream, this.m_currentChunkLength));
            continue;
          }
          this.IgnoreChunk();
          continue;
        case PngDecoder.PngChunkTypes.sRGB:
          this.m_issRGB = true;
          this.IgnoreChunk();
          continue;
        default:
          continue;
      }
    }
  }

  private void ReadZTextMetadata(byte[] chunkBytes)
  {
    using (MemoryStream input = new MemoryStream(chunkBytes))
    {
      BinaryReader reader = new BinaryReader((Stream) input);
      byte[] bytes = this.ReadNullTerminatedBytes(79, reader);
      string str = Encoding.GetEncoding("ISO-8859-1").GetString(bytes, 0, bytes.Length);
      if (!(str == "XML:com.adobe.xmp"))
        return;
      sbyte num1 = (sbyte) reader.ReadByte();
      int num2 = 4;
      int count = chunkBytes.Length - str.Length - num2;
      if (num1 != (sbyte) 0)
        return;
      if (this.MetadataStream == null)
        this.MetadataStream = new MemoryStream();
      this.MetadataStream.Write(chunkBytes, chunkBytes.Length - count, count);
    }
  }

  internal byte[] ReadNullTerminatedBytes(int maxLength, BinaryReader reader)
  {
    byte[] sourceArray = new byte[maxLength];
    int length = 0;
    while (length < sourceArray.Length && (sourceArray[length] = reader.ReadByte()) != (byte) 0)
      ++length;
    if (length == maxLength)
      return sourceArray;
    byte[] destinationArray = new byte[length];
    if (length > 0)
      Array.Copy((Array) sourceArray, (Array) destinationArray, length);
    return destinationArray;
  }

  private void DecodeImageData()
  {
    this.m_isDecode = this.m_header.Interlace == 1 || this.m_header.BitDepth == 16 /*0x10*/ || (this.m_header.ColorType & 4) != 0 || this.m_shades;
    if (this.m_isDecode)
    {
      if ((this.m_header.ColorType & 4) != 0 || this.m_shades)
        this.m_maskData = new byte[this.Width * this.Height];
      if (this.m_iDatStream != null)
      {
        this.m_iDatStream.Position = 0L;
        this.m_dataStream = this.GetDeflatedData(this.m_iDatStream);
        this.m_dataStream.Position = 0L;
      }
      this.ImageData = new byte[this.m_idatLength];
      this.ReadDecodeData();
      if (this.ImageData == null || this.ImageData.Length != 0 || !this.m_shades)
        return;
      this.m_ideateDecode = false;
      this.ImageData = (this.m_iDatStream as MemoryStream).ToArray();
    }
    else
    {
      this.m_ideateDecode = false;
      this.ImageData = (this.m_iDatStream as MemoryStream).ToArray();
    }
  }

  private Stream GetDeflatedData(Stream iDatStream)
  {
    byte[] array = (iDatStream as MemoryStream).ToArray();
    DeflateStream deflateStream = new DeflateStream((Stream) new MemoryStream(array, 2, array.Length - 6), CompressionMode.Decompress, true);
    Stream deflatedData = (Stream) new MemoryStream();
    byte[] buffer = new byte[4096 /*0x1000*/];
    int count;
    while ((count = deflateStream.Read(buffer, 0, buffer.Length)) > 0)
      deflatedData.Write(buffer, 0, count);
    return deflatedData;
  }

  private void ReadDecodeData()
  {
    if (this.m_header.Interlace != 1)
    {
      this.DecodeData(0, 0, 1, 1, this.Width, this.Height);
    }
    else
    {
      this.DecodeData(0, 0, 8, 8, (this.Width + 7) / 8, (this.Height + 7) / 8);
      this.DecodeData(4, 0, 8, 8, (this.Width + 3) / 8, (this.Height + 7) / 8);
      this.DecodeData(0, 4, 4, 8, (this.Width + 3) / 4, (this.Height + 3) / 8);
      this.DecodeData(2, 0, 4, 4, (this.Width + 1) / 4, (this.Height + 3) / 4);
      this.DecodeData(0, 2, 2, 4, (this.Width + 1) / 2, (this.Height + 1) / 4);
      this.DecodeData(1, 0, 2, 2, this.Width / 2, (this.Height + 1) / 2);
      this.DecodeData(0, 1, 1, 2, this.Width, this.Height / 2);
    }
  }

  private void DecodeData(int xOffset, int yOffset, int xStep, int yStep, int width, int height)
  {
    if (width == 0 || height == 0)
      return;
    int count = (this.m_inputBands * width * this.m_header.BitDepth + 7) / 8;
    byte[] data = new byte[count];
    byte[] pData = new byte[count];
    int num1 = 0;
    int y = yOffset;
    while (num1 < height)
    {
      int num2 = this.m_dataStream.ReadByte();
      this.Read(this.m_dataStream, data, 0, count);
      switch (num2)
      {
        case 0:
          this.ProcessPixels(data, xOffset, xStep, y, width);
          byte[] numArray = pData;
          pData = data;
          data = numArray;
          ++num1;
          y += yStep;
          continue;
        case 1:
          this.DecompressSub(data, count, this.m_bitsPerPixel);
          goto case 0;
        case 2:
          this.DecompressUp(data, pData, count);
          goto case 0;
        case 3:
          this.DecompressAverage(data, pData, count, this.m_bitsPerPixel);
          goto case 0;
        case 4:
          this.DecompressPaeth(data, pData, count, this.m_bitsPerPixel);
          goto case 0;
        default:
          throw new Exception("Unknown PNG filter");
      }
    }
  }

  private void ProcessPixels(byte[] data, int x, int step, int y, int width)
  {
    int size = 0;
    int[] pixel = this.GetPixel(data);
    if (this.m_header.ColorType == 0 || this.m_header.ColorType == 3 || this.m_header.ColorType == 4)
      size = 1;
    else if (this.m_header.ColorType == 2 || this.m_header.ColorType == 6)
      size = 3;
    if (this.ImageData != null && this.ImageData.Length > 0)
    {
      int x1 = x;
      int num = this.m_header.BitDepth == 16 /*0x10*/ ? 8 : this.m_header.BitDepth;
      int bpr = (size * this.Width * num + 7) / 8;
      for (int index = 0; index < width; ++index)
      {
        this.SetPixel(this.ImageData, pixel, this.m_inputBands * index, size, x1, y, this.m_header.BitDepth, bpr);
        x1 += step;
      }
    }
    if ((this.m_header.ColorType & 4) == 0 && !this.m_shades)
      return;
    if ((this.m_header.ColorType & 4) != 0)
    {
      if (this.m_header.BitDepth == 16 /*0x10*/)
      {
        for (int index1 = 0; index1 < width; ++index1)
        {
          int index2 = index1 * this.m_inputBands + size;
          pixel[index2] = pixel[index2] >>> 8;
        }
      }
      int width1 = this.Width;
      int x2 = x;
      for (int index = 0; index < width; ++index)
      {
        this.SetPixel(this.m_maskData, pixel, this.m_inputBands * index + size, 1, x2, y, 8, width1);
        x2 += step;
      }
    }
    else
    {
      int width2 = this.Width;
      int[] data1 = new int[1];
      int x3 = x;
      for (int index3 = 0; index3 < width; ++index3)
      {
        int index4 = pixel[index3];
        data1[0] = index4 >= this.m_alpha.Length ? (int) byte.MaxValue : (int) this.m_alpha[index4];
        this.SetPixel(this.m_maskData, data1, 0, 1, x3, y, 8, width2);
        x3 += step;
      }
    }
  }

  private int[] GetPixel(byte[] data)
  {
    if (this.m_header.BitDepth == 8)
    {
      int[] pixel = new int[data.Length];
      for (int index = 0; index < pixel.Length; ++index)
        pixel[index] = (int) data[index] & (int) byte.MaxValue;
      return pixel;
    }
    if (this.m_header.BitDepth == 16 /*0x10*/)
    {
      int[] pixel = new int[data.Length / 2];
      for (int index = 0; index < pixel.Length; ++index)
        pixel[index] = (((int) data[index * 2] & (int) byte.MaxValue) << 8) + ((int) data[index * 2 + 1] & (int) byte.MaxValue);
      return pixel;
    }
    int[] pixel1 = new int[data.Length * 8 / this.m_header.BitDepth];
    int num1 = 0;
    int num2 = 8 / this.m_header.BitDepth;
    int num3 = (1 << this.m_header.BitDepth) - 1;
    for (int index1 = 0; index1 < data.Length; ++index1)
    {
      for (int index2 = num2 - 1; index2 >= 0; --index2)
      {
        int num4 = this.m_header.BitDepth * index2;
        int num5 = (int) data[index1];
        pixel1[num1++] = (num4 < 1 ? num5 : num5 >>> num4) & num3;
      }
    }
    return pixel1;
  }

  private void SetPixel(
    byte[] imageData,
    int[] data,
    int offset,
    int size,
    int x,
    int y,
    int bitDepth,
    int bpr)
  {
    switch (bitDepth)
    {
      case 8:
        int num1 = bpr * y + size * x;
        for (int index = 0; index < size; ++index)
          imageData[num1 + index] = (byte) data[index + offset];
        break;
      case 16 /*0x10*/:
        int num2 = bpr * y + size * x;
        for (int index = 0; index < size; ++index)
          imageData[num2 + index] = (byte) (data[index + offset] >> 8);
        break;
      default:
        int index1 = bpr * y + x / (8 / bitDepth);
        int num3 = data[offset] << 8 - bitDepth * (x % (8 / bitDepth)) - bitDepth;
        imageData[index1] |= (byte) num3;
        break;
    }
  }

  private void Read(Stream stream, byte[] data, int offset, int count)
  {
    while (count > 0)
    {
      int num = stream.Read(data, offset, count);
      if (num <= 0)
        throw new IOException("Insufficient data");
      count -= num;
      offset += num;
    }
  }

  private void DecompressSub(byte[] data, int count, int bpp)
  {
    for (int index = bpp; index < count; ++index)
    {
      int num = ((int) data[index] & (int) byte.MaxValue) + ((int) data[index - bpp] & (int) byte.MaxValue);
      data[index] = (byte) num;
    }
  }

  private void DecompressUp(byte[] data, byte[] pData, int count)
  {
    for (int index = 0; index < count; ++index)
    {
      int num1 = (int) data[index] & (int) byte.MaxValue;
      int num2 = (int) pData[index] & (int) byte.MaxValue;
      data[index] = (byte) (num1 + num2);
    }
  }

  private void DecompressAverage(byte[] data, byte[] pData, int count, int bpp)
  {
    for (int index = 0; index < bpp; ++index)
    {
      int num1 = (int) data[index] & (int) byte.MaxValue;
      int num2 = (int) pData[index] & (int) byte.MaxValue;
      data[index] = (byte) (num1 + num2 / 2);
    }
    for (int index = bpp; index < count; ++index)
    {
      int num3 = (int) data[index] & (int) byte.MaxValue;
      int num4 = (int) data[index - bpp] & (int) byte.MaxValue;
      int num5 = (int) pData[index] & (int) byte.MaxValue;
      data[index] = (byte) (num3 + (num4 + num5) / 2);
    }
  }

  private int PaethPredictor(int a, int b, int c)
  {
    int num1 = a + b - c;
    int num2 = Math.Abs(num1 - a);
    int num3 = Math.Abs(num1 - b);
    int num4 = Math.Abs(num1 - c);
    if (num2 <= num3 && num2 <= num4)
      return a;
    return num3 <= num4 ? b : c;
  }

  private void DecompressPaeth(byte[] data, byte[] pData, int count, int bpp)
  {
    for (int index = 0; index < bpp; ++index)
    {
      int num1 = (int) data[index] & (int) byte.MaxValue;
      int num2 = (int) pData[index] & (int) byte.MaxValue;
      data[index] = (byte) (num1 + num2);
    }
    for (int index = bpp; index < count; ++index)
    {
      int num = (int) data[index] & (int) byte.MaxValue;
      int a = (int) data[index - bpp] & (int) byte.MaxValue;
      int b = (int) pData[index] & (int) byte.MaxValue;
      int c = (int) pData[index - bpp] & (int) byte.MaxValue;
      data[index] = (byte) (num + this.PaethPredictor(a, b, c));
    }
  }

  private void ReadPLTE()
  {
    if (this.m_header.ColorType == 3)
    {
      this.m_colorSpace = new PdfArray();
      this.m_colorSpace.Add((IPdfPrimitive) new PdfName("Indexed"));
      this.m_colorSpace.Add(this.GetPngColorSpace());
      this.m_colorSpace.Add((IPdfPrimitive) new PdfNumber(this.m_currentChunkLength / 3 - 1));
      this.m_colorSpace.Add((IPdfPrimitive) new PdfString(StreamExtensions.ReadByte(this.InternalStream, this.m_currentChunkLength)));
    }
    else
      this.IgnoreChunk();
  }

  private void ReadTRNS()
  {
    if (this.m_header.ColorType == 3)
    {
      byte[] numArray = StreamExtensions.ReadByte(this.InternalStream, this.m_currentChunkLength);
      this.m_alpha = new byte[numArray.Length];
      for (int index = 0; index < numArray.Length; ++index)
      {
        this.m_alpha[index] = numArray[index];
        int num = (int) numArray[index] & (int) byte.MaxValue;
        if (num == 0)
        {
          ++this.transparentPixel;
          this.transparentPixelId = index;
        }
        if (num != 0 && num != (int) byte.MaxValue)
          this.m_shades = true;
      }
    }
    else
      this.IgnoreChunk();
  }

  private IPdfPrimitive GetPngColorSpace()
  {
    if (!this.m_issRGB)
      return (this.m_header.ColorType & 2) == 0 ? (IPdfPrimitive) new PdfName("DeviceGray") : (IPdfPrimitive) new PdfName("DeviceRGB");
    PdfArray pngColorSpace = new PdfArray();
    PdfDictionary element = new PdfDictionary();
    PdfArray primitive = new PdfArray();
    primitive.Add((IPdfPrimitive) new PdfNumber(1));
    primitive.Add((IPdfPrimitive) new PdfNumber(1));
    primitive.Add((IPdfPrimitive) new PdfNumber(1));
    element.SetProperty("Gamma", (IPdfPrimitive) new PdfArray()
    {
      (IPdfPrimitive) new PdfNumber(2.2f),
      (IPdfPrimitive) new PdfNumber(2.2f),
      (IPdfPrimitive) new PdfNumber(2.2f)
    });
    if (this.m_issRGB)
    {
      float num1 = 0.3127f;
      float num2 = 0.329f;
      float num3 = 0.64f;
      float num4 = 0.33f;
      float num5 = 0.3f;
      float num6 = 0.6f;
      float num7 = 0.15f;
      float num8 = 0.06f;
      float num9 = num2 * (float) (((double) num5 - (double) num7) * (double) num4 - ((double) num3 - (double) num7) * (double) num6 + ((double) num3 - (double) num5) * (double) num8);
      float num10 = num4 * (float) (((double) num5 - (double) num7) * (double) num2 - ((double) num1 - (double) num7) * (double) num6 + ((double) num1 - (double) num5) * (double) num8) / num9;
      float num11 = num10 * num3 / num4;
      float num12 = num10 * (float) ((1.0 - (double) num3) / (double) num4 - 1.0);
      float num13 = (float) (-(double) num6 * (((double) num3 - (double) num7) * (double) num2 - ((double) num1 - (double) num7) * (double) num4 + ((double) num1 - (double) num3) * (double) num8)) / num9;
      float num14 = num13 * num5 / num6;
      float num15 = num13 * (float) ((1.0 - (double) num5) / (double) num6 - 1.0);
      float num16 = num8 * (float) (((double) num3 - (double) num5) * (double) num2 - ((double) num1 - (double) num5) * (double) num2 + ((double) num1 - (double) num3) * (double) num6) / num9;
      float num17 = num16 * num7 / num8;
      float num18 = num16 * (float) ((1.0 - (double) num7) / (double) num8 - 1.0);
      primitive = new PdfArray()
      {
        (IPdfPrimitive) new PdfNumber(num11 + num14 + num17),
        (IPdfPrimitive) new PdfNumber(1f),
        (IPdfPrimitive) new PdfNumber(num12 + num15 + num18)
      };
      element.SetProperty("Matrix", (IPdfPrimitive) new PdfArray()
      {
        (IPdfPrimitive) new PdfNumber(num11),
        (IPdfPrimitive) new PdfNumber(num10),
        (IPdfPrimitive) new PdfNumber(num12),
        (IPdfPrimitive) new PdfNumber(num14),
        (IPdfPrimitive) new PdfNumber(num13),
        (IPdfPrimitive) new PdfNumber(num15),
        (IPdfPrimitive) new PdfNumber(num17),
        (IPdfPrimitive) new PdfNumber(num16),
        (IPdfPrimitive) new PdfNumber(num18)
      });
    }
    element.SetProperty("WhitePoint", (IPdfPrimitive) primitive);
    pngColorSpace.Add((IPdfPrimitive) new PdfName("CalRGB"));
    pngColorSpace.Add((IPdfPrimitive) element);
    return (IPdfPrimitive) pngColorSpace;
  }

  private void ReadImageData()
  {
    byte[] buffer = new byte[this.m_currentChunkLength];
    this.InternalStream.Read(buffer, 0, this.m_currentChunkLength);
    if (this.m_iDatStream == null)
      this.m_iDatStream = (Stream) new MemoryStream();
    this.m_iDatStream.Write(buffer, 0, buffer.Length);
    StreamExtensions.Skip(this.InternalStream, 4);
  }

  internal void InitializeBase()
  {
    this.Width = this.m_header.Width;
    this.Height = this.m_header.Height;
    this.BitsPerComponent = this.m_header.BitDepth;
  }

  internal override PdfStream GetImageDictionary()
  {
    PdfStream imageStream = new PdfStream();
    imageStream.InternalStream = new MemoryStream(this.ImageData);
    imageStream.Compress = this.m_isDecode && this.m_ideateDecode;
    imageStream["Type"] = (IPdfPrimitive) new PdfName("XObject");
    imageStream["Subtype"] = (IPdfPrimitive) new PdfName("Image");
    imageStream["Width"] = (IPdfPrimitive) new PdfNumber(this.Width);
    imageStream["Height"] = (IPdfPrimitive) new PdfNumber(this.Height);
    if (this.BitsPerComponent == 16 /*0x10*/)
      imageStream["BitsPerComponent"] = (IPdfPrimitive) new PdfNumber(8);
    else
      imageStream["BitsPerComponent"] = (IPdfPrimitive) new PdfNumber(this.BitsPerComponent);
    if (!this.m_isDecode || !this.m_ideateDecode)
      imageStream["Filter"] = (IPdfPrimitive) new PdfName("FlateDecode");
    if ((this.m_header.ColorType & 2) == 0)
      imageStream["ColorSpace"] = (IPdfPrimitive) new PdfName("DeviceGray");
    else
      imageStream["ColorSpace"] = (IPdfPrimitive) new PdfName("DeviceRGB");
    if (!this.m_isDecode || this.m_shades && !this.m_ideateDecode)
      imageStream["DecodeParms"] = (IPdfPrimitive) this.GetDecodeParams();
    this.SetMask(imageStream);
    return imageStream;
  }

  private void SetMask(PdfStream imageStream)
  {
    if (this.m_maskData != null && this.m_maskData.Length > 0)
    {
      PdfStream pdfStream = new PdfStream();
      pdfStream.InternalStream = new MemoryStream(this.m_maskData);
      pdfStream["Type"] = (IPdfPrimitive) new PdfName("XObject");
      pdfStream["Subtype"] = (IPdfPrimitive) new PdfName("Image");
      pdfStream["Width"] = (IPdfPrimitive) new PdfNumber(this.Width);
      pdfStream["Height"] = (IPdfPrimitive) new PdfNumber(this.Height);
      if (this.BitsPerComponent == 16 /*0x10*/)
        pdfStream["BitsPerComponent"] = (IPdfPrimitive) new PdfNumber(8);
      else
        pdfStream["BitsPerComponent"] = (IPdfPrimitive) new PdfNumber(this.BitsPerComponent);
      pdfStream["ColorSpace"] = (IPdfPrimitive) new PdfName("DeviceGray");
      imageStream.SetProperty("SMask", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
    }
    else
    {
      if (this.m_shades || this.transparentPixel != 1)
        return;
      int[] array = new int[2]
      {
        this.transparentPixelId,
        this.transparentPixelId
      };
      imageStream.SetProperty("Mask", (IPdfPrimitive) new PdfArray(array));
    }
  }

  private PdfDictionary GetDecodeParams()
  {
    return new PdfDictionary()
    {
      ["Columns"] = (IPdfPrimitive) new PdfNumber(this.Width),
      ["Colors"] = (IPdfPrimitive) new PdfNumber(this.m_colors),
      ["Predictor"] = (IPdfPrimitive) new PdfNumber(15),
      ["BitsPerComponent"] = (IPdfPrimitive) new PdfNumber(this.BitsPerComponent)
    };
  }

  internal void Dispose()
  {
    if (this.m_iDatStream != null)
      this.m_iDatStream.Dispose();
    if (this.m_dataStream != null)
      this.m_dataStream.Dispose();
    this.m_iDatStream = (Stream) null;
    this.m_dataStream = (Stream) null;
    this.m_maskData = (byte[]) null;
    this.ImageData = (byte[]) null;
    if (this.InternalStream != null)
      this.InternalStream.Dispose();
    this.InternalStream = (Stream) null;
  }

  private bool ReadNextchunk(out PngDecoder.PngChunkTypes header)
  {
    header = PngDecoder.PngChunkTypes.Unknown;
    this.m_currentChunkLength = StreamExtensions.ReadUInt32(this.InternalStream);
    string str = StreamExtensions.ReadString(this.InternalStream, 4);
    if (Enum.IsDefined(typeof (PngDecoder.PngChunkTypes), (object) str))
    {
      header = (PngDecoder.PngChunkTypes) Enum.Parse(typeof (PngDecoder.PngChunkTypes), str, true);
      return true;
    }
    return this.InternalStream.Length != this.InternalStream.Position;
  }

  private void ReadHeader()
  {
    this.m_header.Width = StreamExtensions.ReadUInt32(this.InternalStream);
    this.m_header.Height = StreamExtensions.ReadUInt32(this.InternalStream);
    this.m_header.BitDepth = this.InternalStream.ReadByte();
    this.m_header.ColorType = this.InternalStream.ReadByte();
    this.m_header.Compression = this.InternalStream.ReadByte();
    this.m_header.Filter = (PngDecoder.PngFilterTypes) this.InternalStream.ReadByte();
    this.m_header.Interlace = this.InternalStream.ReadByte();
    this.m_bDecodeIdat = (this.m_header.ColorType & 4) != 0;
    this.m_colors = this.m_header.ColorType == 3 || (this.m_header.ColorType & 2) == 0 ? 1 : 3;
    this.m_bytesPerPixel = this.m_header.BitDepth == 16 /*0x10*/ ? 2 : 1;
    this.InitializeBase();
    this.SetBitsPerPixel();
    StreamExtensions.Skip(this.InternalStream, 4);
  }

  private void SetBitsPerPixel()
  {
    this.m_bitsPerPixel = this.m_header.BitDepth == 16 /*0x10*/ ? 2 : 1;
    if (this.m_header.ColorType == 0)
    {
      this.m_idatLength = (long) ((this.BitsPerComponent * this.Width + 7) / 8 * this.Height);
      this.m_inputBands = 1;
    }
    else if (this.m_header.ColorType == 2)
    {
      this.m_idatLength = (long) (this.Width * this.Height * 3);
      this.m_inputBands = 3;
      this.m_bitsPerPixel *= 3;
    }
    else if (this.m_header.ColorType == 3)
    {
      if (this.m_header.Interlace == 1)
        this.m_idatLength = (long) ((this.m_header.BitDepth * this.Width + 7) / 8 * this.Height);
      this.m_inputBands = 1;
      this.m_bitsPerPixel = 1;
    }
    else if (this.m_header.ColorType == 4)
    {
      this.m_idatLength = (long) (this.Width * this.Height);
      this.m_inputBands = 2;
      this.m_bitsPerPixel *= 2;
    }
    else
    {
      if (this.m_header.ColorType != 6)
        return;
      this.m_idatLength = (long) (this.Width * 3 * this.Height);
      this.m_inputBands = 4;
      this.m_bitsPerPixel *= 4;
    }
  }

  private void IgnoreChunk()
  {
    if (this.m_currentChunkLength <= 0)
      return;
    StreamExtensions.Skip(this.InternalStream, this.m_currentChunkLength + 4);
  }

  internal enum PngChunkTypes
  {
    IHDR,
    PLTE,
    IDAT,
    IEND,
    bKGD,
    cHRM,
    gAMA,
    hIST,
    pHYs,
    sBIT,
    tEXt,
    tIME,
    tRNS,
    zTXt,
    sRGB,
    iCCP,
    iTXt,
    Unknown,
  }

  internal enum PngFilterTypes
  {
    None,
    Sub,
    Up,
    Average,
    Paeth,
  }

  internal enum PngImageTypes
  {
    GreyScale = 0,
    TrueColor = 2,
    IndexedColor = 3,
    GrayScaleWithAlpha = 4,
    TrueColorWithAlpha = 6,
  }

  internal struct PngHeader
  {
    public int Width;
    public int Height;
    public int ColorType;
    public int Compression;
    public int BitDepth;
    public PngDecoder.PngFilterTypes Filter;
    public int Interlace;
  }
}
