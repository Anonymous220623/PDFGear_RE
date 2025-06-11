// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PushStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PushStream : Stream
{
  private int m_buffer = -1;
  internal Stream m_stream;

  internal PushStream(Stream stream) => this.m_stream = stream;

  public override int ReadByte()
  {
    if (this.m_buffer == -1)
      return this.m_stream.ReadByte();
    int buffer = this.m_buffer;
    this.m_buffer = -1;
    return buffer;
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    if (this.m_buffer == -1 || count <= 0)
      return this.m_stream.Read(buffer, offset, count);
    buffer[offset] = (byte) this.m_buffer;
    this.m_buffer = -1;
    return 1;
  }

  public virtual void Unread(int b) => this.m_buffer = b & (int) byte.MaxValue;

  public override bool CanRead => this.m_stream.CanRead;

  public override bool CanSeek => this.m_stream.CanSeek;

  public override bool CanWrite => this.m_stream.CanWrite;

  public override long Length => this.m_stream.Length;

  public override long Position
  {
    get => this.m_stream.Position;
    set => this.m_stream.Position = value;
  }

  public override void Close() => this.m_stream.Close();

  public override void Flush() => this.m_stream.Flush();

  public override long Seek(long offset, SeekOrigin origin) => this.m_stream.Seek(offset, origin);

  public override void SetLength(long value) => this.m_stream.SetLength(value);

  public override void Write(byte[] buffer, int offset, int count)
  {
    this.m_stream.Write(buffer, offset, count);
  }

  public override void WriteByte(byte value) => this.m_stream.WriteByte(value);
}
