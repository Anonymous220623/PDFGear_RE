// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Entities.Image
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;
using System.IO;
using System.Reflection;

#nullable disable
namespace Syncfusion.DocIO.DLS.Entities;

internal class Image : IDisposable
{
  private const string DEF_GIF_HEADER = "GIF8";
  private const int DEF_TIFF_MARKER = 42;
  private Stream m_stream;
  private int m_height;
  private int m_width;
  private long m_Xdpi = 96 /*0x60*/;
  private long m_Ydpi = 96 /*0x60*/;
  private ImageFormat m_format;
  private byte[] m_pngHeader = new byte[8]
  {
    (byte) 137,
    (byte) 80 /*0x50*/,
    (byte) 78,
    (byte) 71,
    (byte) 13,
    (byte) 10,
    (byte) 26,
    (byte) 10
  };
  private byte[] m_jpegHeader = new byte[2]
  {
    byte.MaxValue,
    (byte) 216
  };
  private byte[] m_bmpHeader = new byte[2]
  {
    (byte) 66,
    (byte) 77
  };
  private byte[] m_tiffHeader1 = new byte[2]
  {
    (byte) 73,
    (byte) 73
  };
  private byte[] m_tiffHeader2 = new byte[2]
  {
    (byte) 77,
    (byte) 77
  };
  private byte[] m_imageData;

  public int Width => this.m_width;

  public int Height => this.m_height;

  public ImageFormat Format => this.m_format;

  public Size Size => new Size(this.m_width, this.m_height);

  internal byte[] ImageData => this.m_imageData;

  public ImageFormat RawFormat => this.m_format;

  internal long HorizontalDpi
  {
    get => this.m_Xdpi;
    set => this.m_Xdpi = value;
  }

  internal long VerticalDpi
  {
    get => this.m_Ydpi;
    set => this.m_Ydpi = value;
  }

  public bool IsMetafile => this.RawFormat == ImageFormat.Emf || this.RawFormat == ImageFormat.Wmf;

  public Image(Stream stream)
  {
    if (!stream.CanRead || !stream.CanSeek)
      throw new ArgumentException("Stream");
    this.m_stream = stream;
    this.Initialize();
    if (this.m_format == ImageFormat.Unknown)
      throw new ArgumentException("The given image stream is either unsupported or not a valid image stream");
  }

  internal Image()
  {
  }

  private void Initialize()
  {
    if (this.CheckIfPng())
    {
      this.m_format = ImageFormat.Png;
      this.ParsePngImage();
    }
    if (this.m_format == ImageFormat.Unknown && this.CheckIfJpeg())
    {
      this.m_format = ImageFormat.Jpeg;
      this.ParseJpegImage();
    }
    if (this.m_format == ImageFormat.Unknown && this.CheckIfGif())
    {
      this.m_format = ImageFormat.Gif;
      this.ParseGifImage();
    }
    if (this.m_format == ImageFormat.Unknown && this.CheckIfEmfOrWmf())
      this.ParseEmfOrWmfImage();
    if (this.m_format == ImageFormat.Unknown && this.CheckIfIcon())
    {
      this.m_format = ImageFormat.Icon;
      this.ParseIconImage();
    }
    if (this.m_format == ImageFormat.Unknown && this.CheckIfBmp())
    {
      this.m_format = ImageFormat.Bmp;
      this.ParseBmpImage();
    }
    if (this.m_format == ImageFormat.Unknown && this.CheckIfTiff())
    {
      this.m_format = ImageFormat.Tiff;
      this.ParseTifImage();
    }
    if (this.m_format == ImageFormat.Unknown && WordDocument.EnablePartialTrustCode)
    {
      this.m_format = ImageFormat.Emf;
      this.ParseEmfOrWmfImage();
    }
    if (this.m_format == ImageFormat.Unknown)
      return;
    this.Reset();
    this.m_imageData = new byte[this.m_stream.Length];
    this.m_stream.Read(this.m_imageData, 0, this.m_imageData.Length);
  }

  private bool CheckIfTiff()
  {
    this.Reset();
    byte[] buffer = new byte[3];
    this.m_stream.Read(buffer, 0, buffer.Length);
    return ((int) buffer[0] == (int) this.m_tiffHeader1[0] && (int) buffer[1] == (int) this.m_tiffHeader1[1] || (int) buffer[0] == (int) this.m_tiffHeader2[0] && (int) buffer[1] == (int) this.m_tiffHeader2[1]) && buffer[2] == (byte) 42;
  }

  private bool CheckIfBmp()
  {
    this.Reset();
    for (int index = 0; index < this.m_bmpHeader.Length; ++index)
    {
      if ((int) this.m_bmpHeader[index] != this.m_stream.ReadByte())
        return false;
    }
    return true;
  }

  private void ParseBmpImage()
  {
    this.Reset();
    int count = 14;
    this.m_stream.Read(new byte[count], 0, count);
    this.ReadInt32();
    this.m_width = this.ReadInt32();
    this.m_height = this.ReadInt32();
  }

  private bool CheckIfIcon()
  {
    this.Reset();
    int num1 = this.ReadWord();
    int num2 = this.ReadWord();
    return num1 == 0 && num2 == 1;
  }

  private bool CheckIfPng()
  {
    this.Reset();
    for (int index = 0; index < this.m_pngHeader.Length; ++index)
    {
      if ((int) this.m_pngHeader[index] != this.m_stream.ReadByte())
        return false;
    }
    return true;
  }

  private bool CheckIfJpeg()
  {
    this.Reset();
    for (int index = 0; index < this.m_jpegHeader.Length; ++index)
    {
      if ((int) this.m_jpegHeader[index] != this.m_stream.ReadByte())
        return false;
    }
    return true;
  }

  private bool CheckIfGif()
  {
    this.Reset();
    return this.ReadString(6).StartsWithExt("GIF8");
  }

  private bool CheckIfEmfOrWmf()
  {
    this.Reset();
    if (this.ReadInt32() == 1)
    {
      this.m_format = ImageFormat.Emf;
      return true;
    }
    this.Reset();
    if (this.ReadInt32() != -1698247209)
      return false;
    this.m_format = ImageFormat.Wmf;
    return true;
  }

  private void ParsePngImage()
  {
    Bitmap bitmap = new Bitmap(this.m_stream);
    this.m_width = bitmap.Width;
    this.m_height = bitmap.Height;
  }

  private Stream GetManifestResourceStream(string fileName)
  {
    Assembly executingAssembly = Assembly.GetExecutingAssembly();
    foreach (string manifestResourceName in executingAssembly.GetManifestResourceNames())
    {
      if (manifestResourceName.EndsWith("." + fileName))
      {
        fileName = manifestResourceName;
        break;
      }
    }
    return executingAssembly.GetManifestResourceStream(fileName);
  }

  private void ParseJpegImage()
  {
    this.Reset();
    Bitmap bitmap = new Bitmap(this.m_stream);
    this.m_width = bitmap.Width;
    this.m_height = bitmap.Height;
  }

  private void GenerateJPEGNormalImage(byte[] imgData, long index, long length, long bytesleft)
  {
    long index1 = 4;
    length = (long) ((int) imgData[index1] * 256 /*0x0100*/ + (int) imgData[index1 + 1L]);
    while (index1 < (long) imgData.Length)
    {
      long num = index1 + length;
      if (num >= (long) imgData.Length)
        break;
      if (imgData[num + 1L] == (byte) 192 /*0xC0*/ || imgData[num + 1L] == (byte) 194)
      {
        this.m_height = (int) imgData[num + 5L] * 256 /*0x0100*/ + (int) imgData[num + 6L];
        this.m_width = (int) imgData[num + 7L] * 256 /*0x0100*/ + (int) imgData[num + 8L];
        break;
      }
      index1 = num + 2L;
      length = (long) ((int) imgData[index1] * 256 /*0x0100*/ + (int) imgData[index1 + 1L]);
    }
  }

  private void GenerateJPEGJFIFImage(byte[] imgdata, long index, long length, long bytesleft)
  {
    for (; index < (long) imgdata.Length; index += length + 2L)
    {
      if (imgdata[index + 4L] == (byte) 74 && imgdata[index + 5L] == (byte) 70 && imgdata[index + 6L] == (byte) 73 && imgdata[index + 7L] == (byte) 70 && imgdata[index + 8L] == (byte) 0)
      {
        this.App0Resolution(imgdata, index);
        index = 3L;
      }
      if (imgdata[index] == (byte) 192 /*0xC0*/ || imgdata[index] == (byte) 194)
      {
        this.m_height = (int) imgdata[index + 4L] << 8 | (int) imgdata[index + 5L];
        this.m_width = (int) imgdata[index + 6L] << 8 | (int) imgdata[index + 7L];
        break;
      }
      length = (long) ((int) imgdata[index + 1L] << 8 | (int) imgdata[index + 2L]);
    }
  }

  private void GenerateJPEGEXIFImage(byte[] imgdata, long index, long length, long bytesleft)
  {
    for (; index < (long) imgdata.Length; index += length + 2L)
    {
      if (imgdata[index + 10L] == (byte) 73 && imgdata[index + 11L] == (byte) 73 || imgdata[index + 10L] == (byte) 77 && imgdata[index + 11L] == (byte) 77)
      {
        bytesleft += index;
        bytesleft += 2L;
        bytesleft += 6L;
        bytesleft += 2L;
        long length1 = (long) ((int) imgdata[index + 2L] * 256 /*0x0100*/ + (int) imgdata[index + 3L]);
        this.APP1Resolution(imgdata, length1, index, bytesleft);
        index = 3L;
      }
      if (imgdata[index] == (byte) 192 /*0xC0*/ || imgdata[index] == (byte) 194)
      {
        this.m_height = (int) imgdata[index + 4L] << 8 | (int) imgdata[index + 5L];
        this.m_width = (int) imgdata[index + 6L] << 8 | (int) imgdata[index + 7L];
        break;
      }
      length = (long) ((int) imgdata[index + 1L] << 8 | (int) imgdata[index + 2L]);
    }
  }

  private void App0Resolution(byte[] imgdata, long index)
  {
    if (imgdata[index + 11L] == (byte) 1)
    {
      this.HorizontalDpi = (long) ((int) imgdata[index + 12L] << 8 | (int) imgdata[index + 13L]);
      this.VerticalDpi = (long) ((int) imgdata[index + 14L] << 8 | (int) imgdata[index + 15L]);
    }
    else
    {
      if (imgdata[index + 11L] != (byte) 2)
        return;
      this.HorizontalDpi = (long) ((double) ((int) imgdata[index + 12L] << 8 | (int) imgdata[index + 13L]) * 2.5399999618530273);
      this.VerticalDpi = (long) ((double) ((int) imgdata[index + 14L] << 8 | (int) imgdata[index + 15L]) * 2.5399999618530273);
    }
  }

  private void APP1Resolution(byte[] imgdata, long length, long index, long bytesleft)
  {
    for (; index < length; ++index)
    {
      if (imgdata[index] == (byte) 26 & imgdata[index + 1L] == (byte) 1 && imgdata[index + 2L] == (byte) 5 && imgdata[index + 3L] == (byte) 0 && imgdata[index + 4L] == (byte) 1 && imgdata[index + 5L] == (byte) 0 && imgdata[index + 6L] == (byte) 0 && imgdata[index + 7L] == (byte) 0)
      {
        index = (long) ((int) imgdata[index + 8L] + (int) imgdata[index + 9L] + (int) imgdata[index + 10L] + (int) imgdata[index + 11L]) + bytesleft;
        this.HorizontalDpi = (long) ((int) imgdata[index + 3L] << 24 | (int) imgdata[index + 2L] << 16 /*0x10*/ | (int) imgdata[index + 1L] << 8 | (int) imgdata[index]) / (long) ((int) imgdata[index + 7L] << 24 | (int) imgdata[index + 6L] << 16 /*0x10*/ | (int) imgdata[index + 5L] << 8 | (int) imgdata[index + 4L]);
        this.VerticalDpi = (long) ((int) imgdata[index + 11L] << 24 | (int) imgdata[index + 10L] << 16 /*0x10*/ | (int) imgdata[index + 9L] << 8 | (int) imgdata[index + 8L]) / (long) ((int) imgdata[index + 15L] << 24 | (int) imgdata[index + 14L] << 16 /*0x10*/ | (int) imgdata[index + 13L] << 8 | (int) imgdata[index + 12L]);
        break;
      }
      if (imgdata[index] == (byte) 1 & imgdata[index + 1L] == (byte) 26 && imgdata[index + 2L] == (byte) 0 && imgdata[index + 3L] == (byte) 5 && imgdata[index + 4L] == (byte) 0 && imgdata[index + 5L] == (byte) 0 && imgdata[index + 6L] == (byte) 0 && imgdata[index + 7L] == (byte) 1)
      {
        index = (long) ((int) imgdata[index + 8L] + (int) imgdata[index + 9L] + (int) imgdata[index + 10L] + (int) imgdata[index + 11L]) + bytesleft;
        this.HorizontalDpi = (long) ((int) imgdata[index] << 24 | (int) imgdata[index + 1L] << 16 /*0x10*/ | (int) imgdata[index + 2L] << 8 | (int) imgdata[index + 3L]) / (long) ((int) imgdata[index + 4L] << 24 | (int) imgdata[index + 5L] << 16 /*0x10*/ | (int) imgdata[index + 6L] << 8 | (int) imgdata[index + 7L]);
        this.VerticalDpi = (long) ((int) imgdata[index + 8L] << 24 | (int) imgdata[index + 9L] << 16 /*0x10*/ | (int) imgdata[index + 10L] << 8 | (int) imgdata[index + 11L]) / (long) ((int) imgdata[index + 12L] << 24 | (int) imgdata[index + 13L] << 16 /*0x10*/ | (int) imgdata[index + 14L] << 8 | (int) imgdata[index + 15L]);
        break;
      }
    }
  }

  private void ParseGifImage()
  {
    this.m_width = this.ReadInt16();
    this.m_height = this.ReadInt16();
  }

  private void ParseIconImage()
  {
    this.Reset();
    byte[] buffer = new byte[6];
    this.m_stream.Read(buffer, 0, buffer.Length);
    this.m_width = this.m_stream.ReadByte();
    this.m_height = this.m_stream.ReadByte();
  }

  private void ParseEmfOrWmfImage()
  {
    this.Reset();
    if (this.m_format == ImageFormat.Emf)
    {
      byte[] buffer = new byte[16 /*0x10*/];
      this.m_stream.Read(buffer, 0, buffer.Length);
      this.m_width = this.ReadInt32();
      this.m_height = this.ReadInt32();
    }
    else
    {
      if (this.Format != ImageFormat.Wmf)
        return;
      byte[] buffer = new byte[10];
      this.m_stream.Read(buffer, 0, buffer.Length);
      this.m_width = this.ReadShortLE();
      this.m_height = this.ReadShortLE();
    }
  }

  private void ParseTifImage()
  {
    int num1 = 256 /*0x0100*/;
    int num2 = 257;
    this.m_stream.Position = 4L;
    int num3 = this.ReadInt32();
    if ((long) num3 > this.m_stream.Length)
      throw new Exception("Tiff image file corrupted");
    this.m_stream.Position = (long) (num3 + 2);
    while ((long) num3 < this.m_stream.Length)
    {
      int num4 = this.ReadInt16();
      this.ReadInt16();
      this.ReadInt32();
      int num5 = this.ReadInt32();
      if (num4 == num1)
        this.m_width = num5;
      else if (num4 == num2)
        this.m_height = num5;
      else if (this.m_height != 0 && this.m_width != 0)
        break;
    }
  }

  private int ReadUInt32()
  {
    byte[] buffer = new byte[4];
    this.m_stream.Read(buffer, 0, 4);
    return ((int) buffer[0] << 24) + ((int) buffer[1] << 16 /*0x10*/) + ((int) buffer[2] << 8) + (int) buffer[3];
  }

  private int ReadInt32()
  {
    byte[] buffer = new byte[4];
    this.m_stream.Read(buffer, 0, 4);
    return (int) buffer[0] + ((int) buffer[1] << 8) + ((int) buffer[2] << 16 /*0x10*/) + ((int) buffer[3] << 24);
  }

  private int ReadUInt16()
  {
    byte[] buffer = new byte[2];
    this.m_stream.Read(buffer, 0, 2);
    return ((int) buffer[0] << 8) + (int) buffer[1];
  }

  private int ReadInt16()
  {
    byte[] buffer = new byte[2];
    this.m_stream.Read(buffer, 0, 2);
    return (int) buffer[0] | (int) buffer[1] << 8;
  }

  private int ReadWord()
  {
    return this.m_stream.ReadByte() + (this.m_stream.ReadByte() << 8) & (int) ushort.MaxValue;
  }

  private int ReadShortLE()
  {
    int num = this.ReadWord();
    if (num > (int) short.MaxValue)
      num -= 65536 /*0x010000*/;
    return num;
  }

  private string ReadString(int len)
  {
    char[] chArray = new char[len];
    for (int index = 0; index < len; ++index)
      chArray[index] = (char) this.m_stream.ReadByte();
    return new string(chArray);
  }

  private void Reset() => this.m_stream.Position = 0L;

  internal static Image FromStream(MemoryStream memoryStream) => new Image((Stream) memoryStream);

  internal void Save(MemoryStream memoryStream, ImageFormat imageFormat)
  {
  }

  public void Dispose()
  {
    this.m_imageData = (byte[]) null;
    if (this.m_stream == null)
      return;
    this.m_stream.Dispose();
    this.m_stream = (Stream) null;
  }
}
