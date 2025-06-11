// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.ZippedContentStream
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Compression.Zip;

public class ZippedContentStream : Stream
{
  private MemoryStream m_stream = new MemoryStream();
  private Stream m_deflateStream;
  private uint m_uiCrc32;
  private long m_lSize;

  public override bool CanRead => this.m_deflateStream.CanRead;

  public override bool CanSeek => this.m_deflateStream.CanSeek;

  public override bool CanWrite => this.m_deflateStream.CanWrite;

  public override long Length => this.m_deflateStream.Length;

  public override long Position
  {
    get => this.m_deflateStream.Position;
    set => throw new Exception("The method or operation is not implemented.");
  }

  public Stream ZippedContent
  {
    get
    {
      this.m_deflateStream.Close();
      return (Stream) this.m_stream;
    }
  }

  [CLSCompliant(false)]
  public uint Crc32 => this.m_uiCrc32;

  public long UnzippedSize => this.m_lSize;

  private ZippedContentStream()
  {
    this.m_deflateStream = this.CreateDeflateStream((Stream) this.m_stream);
  }

  public ZippedContentStream(ZipArchive.CompressorCreator createCompressor)
  {
    this.m_deflateStream = createCompressor((Stream) this.m_stream);
  }

  private Stream CreateDeflateStream(Stream stream)
  {
    return (Stream) new NetCompressor(CompressionLevel.Best, stream);
  }

  public override void Flush() => this.m_deflateStream.Flush();

  public override int Read(byte[] buffer, int offset, int count)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public override void SetLength(long value)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    this.m_deflateStream.Write(buffer, offset, count);
    this.m_uiCrc32 = ZipCrc32.ComputeCrc(buffer, offset, count, this.m_uiCrc32);
    this.m_lSize += (long) count;
  }
}
