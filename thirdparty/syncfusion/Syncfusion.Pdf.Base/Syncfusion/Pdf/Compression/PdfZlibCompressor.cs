// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.PdfZlibCompressor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Compression;
using Syncfusion.Pdf.Primitives;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class PdfZlibCompressor : IPdfCompressor
{
  private const int DefaultBufferSize = 32 /*0x20*/;
  private static string DefaultName = StreamFilters.FlateDecode.ToString();
  private PdfCompressionLevel m_level;

  public PdfZlibCompressor() => this.m_level = PdfCompressionLevel.Normal;

  public PdfZlibCompressor(PdfCompressionLevel level)
    : this()
  {
    this.m_level = level;
  }

  public string Name => PdfZlibCompressor.DefaultName;

  public CompressionType Type => CompressionType.Zlib;

  public Encoding Encoding => Encoding.UTF8;

  public PdfCompressionLevel Level
  {
    get => this.m_level;
    set
    {
      if (this.m_level == value)
        return;
      this.m_level = value;
    }
  }

  public byte[] Compress(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    byte[] numArray = (byte[]) null;
    using (MemoryStream inputStream = new MemoryStream(data))
    {
      using (Stream input = this.Compress((Stream) inputStream))
      {
        BinaryReader binaryReader = new BinaryReader(input);
        binaryReader.BaseStream.Position = 0L;
        numArray = binaryReader.ReadBytes((int) input.Length);
        binaryReader.Close();
      }
    }
    return numArray;
  }

  public Stream Compress(Stream inputStream)
  {
    if (inputStream == null)
      throw new ArgumentNullException(nameof (inputStream));
    MemoryStream outputStream = new MemoryStream();
    Syncfusion.Compression.CompressionLevel level = (Syncfusion.Compression.CompressionLevel) this.Level;
    CompressedStreamWriter compressedStreamWriter = new CompressedStreamWriter((Stream) outputStream, level, false);
    int num = 90000000;
    byte[] numArray = inputStream.Length <= (long) num ? new byte[inputStream.Length] : new byte[inputStream.Length / 4L];
    inputStream.Position = 0L;
    int length;
    while ((length = inputStream.Read(numArray, 0, numArray.Length)) > 0)
      compressedStreamWriter.Write(numArray, 0, length, false);
    compressedStreamWriter.Close();
    return (Stream) outputStream;
  }

  public byte[] Compress(string data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    using (MemoryStream inputStream = new MemoryStream(this.Encoding.GetBytes(data)))
    {
      using (Stream stream = this.Compress((Stream) inputStream))
        return PdfStream.StreamToBytes(stream);
    }
  }

  public byte[] Decompress(string data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    using (MemoryStream inputStream = new MemoryStream(this.Encoding.GetBytes(data)))
    {
      using (Stream stream = this.Decompress((Stream) inputStream))
        return PdfStream.StreamToBytes(stream);
    }
  }

  public byte[] Decompress(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (data.Length == 0 || data.Length == 1)
      return data;
    using (MemoryStream inputStream = new MemoryStream(data))
    {
      using (Stream stream = this.Decompress((Stream) inputStream))
        return PdfStream.StreamToBytes(stream);
    }
  }

  public Stream Decompress(Stream inputStream)
  {
    if (inputStream == null)
      throw new ArgumentNullException(nameof (inputStream));
    MemoryStream memoryStream = new MemoryStream();
    byte[] buffer1 = new byte[32 /*0x20*/];
    CompressedStreamReader compressedStreamReader1 = new CompressedStreamReader(inputStream);
    try
    {
      int count;
      while ((count = compressedStreamReader1.Read(buffer1, 0, buffer1.Length)) > 0)
        memoryStream.Write(buffer1, 0, count);
    }
    catch (Exception ex)
    {
      if (ex.Message == "Wrong block length.")
      {
        inputStream.Position = 0L;
        CompressedStreamReader compressedStreamReader2 = new CompressedStreamReader(inputStream);
        byte[] buffer2 = new byte[1];
        memoryStream = new MemoryStream();
        try
        {
          int count;
          while ((count = compressedStreamReader2.Read(buffer2, 0, buffer2.Length)) > 0)
            memoryStream.Write(buffer2, 0, count);
        }
        catch
        {
        }
      }
      else if (ex.Message == "Checksum check failed.")
      {
        try
        {
          inputStream.Position = 0L;
          inputStream.ReadByte();
          inputStream.ReadByte();
          using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress, true))
          {
            byte[] buffer3 = new byte[4096 /*0x1000*/];
            memoryStream = new MemoryStream();
            while (true)
            {
              int count = deflateStream.Read(buffer3, 0, 4096 /*0x1000*/);
              if (count > 0)
                memoryStream.Write(buffer3, 0, count);
              else
                break;
            }
          }
        }
        catch
        {
        }
      }
      else
        throw;
    }
    return (Stream) memoryStream;
  }
}
