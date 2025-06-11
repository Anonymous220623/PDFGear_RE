// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.Image
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Drawing;

public class Image
{
  private const string BmpHeader = "BM";
  private const string GifHeader = "GIF8";
  private static readonly byte[] JpegHeader = new byte[2]
  {
    byte.MaxValue,
    (byte) 216
  };
  private static readonly byte[] PngHeader = new byte[8]
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
  private ImageFormat _format;
  private int _height;
  private byte[] _imageData;
  private Stream _stream;
  private int _width;

  public ImageFormat Format => this._format;

  public int Height => this._height;

  public byte[] ImageData => this._imageData;

  public ImageFormat RawFormat => this.Format;

  public Size Size => new Size(this._width, this._height);

  public int Width => this._width;

  public Image(Stream stream)
  {
    if (!stream.CanRead || !stream.CanSeek)
      throw new ArgumentException("Stream");
    this._stream = stream;
    this.Initialize();
  }

  private bool CheckIfBmp()
  {
    this.Reset();
    return this.ReadString(2).StartsWith("BM");
  }

  private bool CheckIfEmfOrWmf()
  {
    this.Reset();
    if (this.ReadInt32() == 1)
    {
      this._format = ImageFormat.Emf;
      return true;
    }
    this.Reset();
    if (this.ReadInt32() != -1698247209)
      return false;
    this._format = ImageFormat.Wmf;
    return true;
  }

  private bool CheckIfGif()
  {
    this.Reset();
    return this.ReadString(6).StartsWith("GIF8");
  }

  private bool CheckIfIcon()
  {
    this.Reset();
    int num1 = this.ReadWord();
    int num2 = this.ReadWord();
    return num1 == 0 && num2 == 1;
  }

  private bool CheckIfJpeg()
  {
    this.Reset();
    foreach (int num in Image.JpegHeader)
    {
      if (num != this._stream.ReadByte())
        return false;
    }
    return true;
  }

  private bool CheckIfPng()
  {
    this.Reset();
    foreach (int num in Image.PngHeader)
    {
      if (num != this._stream.ReadByte())
        return false;
    }
    return true;
  }

  private void Initialize()
  {
    if (this.CheckIfPng())
    {
      this._format = ImageFormat.Png;
      this.ParsePngImage();
    }
    if (this._format == ImageFormat.Unknown && this.CheckIfJpeg())
    {
      this._format = ImageFormat.Jpeg;
      this.ParseJpegImage();
    }
    if (this._format == ImageFormat.Unknown && this.CheckIfGif())
    {
      this._format = ImageFormat.Gif;
      this.ParseGifImage();
    }
    if (this._format == ImageFormat.Unknown && this.CheckIfEmfOrWmf())
      this.ParseEmfOrWmfImage();
    if (this._format == ImageFormat.Unknown && this.CheckIfIcon())
    {
      this._format = ImageFormat.Icon;
      this.ParseIconImage();
    }
    if (this._format == ImageFormat.Unknown && this.CheckIfBmp())
    {
      this._format = ImageFormat.Bmp;
      this.ParseBmpImage();
    }
    this.Reset();
    this._imageData = new byte[this._stream.Length];
    this._stream.Read(this._imageData, 0, this._imageData.Length);
  }

  private void ParseBmpImage()
  {
    this._stream.Position = 18L;
    this._width = this.ReadInt32();
    this._height = this.ReadInt32();
  }

  private void ParseEmfOrWmfImage()
  {
    this.Reset();
    if (this._format == ImageFormat.Emf)
    {
      byte[] buffer = new byte[16 /*0x10*/];
      this._stream.Read(buffer, 0, buffer.Length);
      this._width = this.ReadInt32();
      this._height = this.ReadInt32();
    }
    else
    {
      if (this.Format != ImageFormat.Wmf)
        return;
      byte[] buffer = new byte[10];
      this._stream.Read(buffer, 0, buffer.Length);
      this._width = this.ReadShortLe();
      this._height = this.ReadShortLe();
    }
  }

  private void ParseGifImage()
  {
    this._width = this.ReadInt16();
    this._height = this.ReadInt16();
  }

  private void ParseIconImage()
  {
    this.Reset();
    byte[] buffer = new byte[6];
    this._stream.Read(buffer, 0, buffer.Length);
    this._width = this._stream.ReadByte();
    this._height = this._stream.ReadByte();
  }

  private void ParseJpegImage()
  {
    this.Reset();
    byte[] buffer = new byte[this._stream.Length];
    this._stream.Read(buffer, 0, buffer.Length);
    long index1 = 4;
    if (buffer[index1 + 2L] != (byte) 74 || buffer[index1 + 3L] != (byte) 70 || buffer[index1 + 4L] != (byte) 73 || buffer[index1 + 5L] != (byte) 70 || buffer[index1 + 6L] != (byte) 0)
      return;
    for (long index2 = (long) ((int) buffer[index1] * 256 /*0x0100*/ + (int) buffer[index1 + 1L]); index1 < (long) buffer.Length && index1 + index2 < (long) buffer.Length; index2 = (long) ((int) buffer[index1] * 256 /*0x0100*/ + (int) buffer[index1 + 1L]))
    {
      long num = index1 + index2;
      if (buffer[num + 1L] == (byte) 192 /*0xC0*/)
      {
        this._height = (int) buffer[num + 5L] * 256 /*0x0100*/ + (int) buffer[num + 6L];
        this._width = (int) buffer[num + 7L] * 256 /*0x0100*/ + (int) buffer[num + 8L];
        break;
      }
      index1 = num + 2L;
    }
  }

  private void ParsePngImage()
  {
    while (true)
    {
      int count = this.ReadUInt32();
      if (!this.ReadString(4).Equals("IHDR"))
        this._stream.Read(new byte[count], 0, count);
      else
        break;
    }
    this._width = this.ReadUInt32();
    this._height = this.ReadUInt32();
  }

  private int ReadInt16()
  {
    byte[] buffer = new byte[2];
    this._stream.Read(buffer, 0, 2);
    return (int) buffer[0] | (int) buffer[1] << 8;
  }

  private int ReadInt32()
  {
    byte[] buffer = new byte[4];
    this._stream.Read(buffer, 0, 4);
    return (int) buffer[0] + ((int) buffer[1] << 8) + ((int) buffer[2] << 16 /*0x10*/) + ((int) buffer[3] << 24);
  }

  private int ReadShortLe()
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
      chArray[index] = (char) this._stream.ReadByte();
    return new string(chArray);
  }

  private int ReadUInt32()
  {
    byte[] buffer = new byte[4];
    this._stream.Read(buffer, 0, 4);
    return ((int) buffer[0] << 24) + ((int) buffer[1] << 16 /*0x10*/) + ((int) buffer[2] << 8) + (int) buffer[3];
  }

  private int ReadWord()
  {
    return this._stream.ReadByte() + (this._stream.ReadByte() << 8) & (int) ushort.MaxValue;
  }

  private void Reset() => this._stream.Position = 0L;

  internal void Close()
  {
    this._imageData = (byte[]) null;
    if (this._stream == null)
      return;
    this._stream.Dispose();
    this._stream = (Stream) null;
  }

  public static Image FromStream(Stream stream) => new Image(stream);

  public Image Clone() => (Image) this.MemberwiseClone();

  public static Image FromFile(string fileName)
  {
    return new Image((Stream) new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read));
  }

  internal void Save(MemoryStream stream, ImageFormat imageFormat)
  {
    if (imageFormat != this._format)
      throw new NotSupportedException();
    stream.Write(this._imageData, 0, this._imageData.Length);
  }
}
