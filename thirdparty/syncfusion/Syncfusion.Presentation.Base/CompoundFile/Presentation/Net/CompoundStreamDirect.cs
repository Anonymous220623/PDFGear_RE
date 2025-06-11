// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.CompoundStreamDirect
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

internal class CompoundStreamDirect(Syncfusion.CompoundFile.Presentation.Net.CompoundFile file, DirectoryEntry entry) : 
  CompoundStreamNet(file, entry),
  ICompoundItem
{
  private const int MinimumSize = 32768 /*0x8000*/;
  private long m_lPosition;

  public override void Open()
  {
    if (this.Entry.Size < 32768U /*0x8000*/)
      base.Open();
    this.m_lPosition = 0L;
  }

  public override int Read(byte[] buffer, int offset, int length)
  {
    int num = this.Stream == null ? this.ParentFile.ReadData(this.Entry, this.m_lPosition, buffer, length) : base.Read(buffer, offset, length);
    this.m_lPosition += (long) num;
    return num;
  }

  public override void Write(byte[] buffer, int offset, int length)
  {
    if (this.Stream == null)
    {
      this.ParentFile.WriteData(this.Entry, this.m_lPosition, buffer, offset, length);
    }
    else
    {
      base.Write(buffer, offset, length);
      if (this.Stream.Length > 32768L /*0x8000*/)
      {
        this.ParentFile.SetEntryStream(this.Entry, this.Stream);
        this.Stream = (Stream) null;
      }
    }
    this.m_lPosition += (long) length;
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    if (this.Stream != null)
      this.Stream.Seek(offset, origin);
    switch (origin)
    {
      case SeekOrigin.Begin:
        this.m_lPosition = offset;
        break;
      case SeekOrigin.Current:
        this.m_lPosition += offset;
        break;
      case SeekOrigin.End:
        this.m_lPosition = (long) this.Entry.Size + offset;
        break;
    }
    return this.m_lPosition;
  }

  public override void SetLength(long value)
  {
    if (this.Stream != null)
      base.SetLength(value);
    this.Entry.Size = (uint) value;
  }

  public override long Length => this.Stream == null ? (long) this.Entry.Size : this.Stream.Length;

  public override long Position
  {
    get => this.m_lPosition;
    set
    {
      this.m_lPosition = value;
      if (this.Stream == null)
        return;
      this.Stream.Position = value;
    }
  }

  public override void Flush()
  {
    if (this.Stream == null)
      return;
    base.Flush();
  }
}
