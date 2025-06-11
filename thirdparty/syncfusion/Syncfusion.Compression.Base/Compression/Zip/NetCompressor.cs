// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.NetCompressor
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Compression.Zip;

public class NetCompressor : Stream
{
  private CompressedStreamWriter writer;

  public NetCompressor(CompressionLevel compressionLevel, Stream outputStream)
  {
    this.writer = new CompressedStreamWriter(outputStream, true, compressionLevel, false);
  }

  public void Write(byte[] data, int size, bool close) => this.writer.Write(data, 0, size, close);

  public override bool CanRead => false;

  public override bool CanSeek => false;

  public override bool CanWrite => true;

  public override void Flush()
  {
  }

  public override long Length => throw new NotImplementedException();

  public override long Position
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    throw new NotImplementedException();
  }

  public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

  public override void SetLength(long value) => throw new NotImplementedException();

  public override void Write(byte[] buffer, int offset, int count)
  {
    this.writer.Write(buffer, offset, count, false);
  }

  public override void Close()
  {
    this.writer.Close();
    base.Close();
  }
}
