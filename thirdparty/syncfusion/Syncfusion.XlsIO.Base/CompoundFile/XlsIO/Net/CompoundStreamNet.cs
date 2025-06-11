// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Net.CompoundStreamNet
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Net;

internal class CompoundStreamNet : CompoundStream, ICompoundItem
{
  private Syncfusion.CompoundFile.XlsIO.Net.CompoundFile m_parentFile;
  private DirectoryEntry m_entry;
  private Stream m_stream;

  public DirectoryEntry Entry => this.m_entry;

  protected Stream Stream
  {
    get => this.m_stream;
    set => this.m_stream = value;
  }

  public Syncfusion.CompoundFile.XlsIO.Net.CompoundFile ParentFile => this.m_parentFile;

  public CompoundStreamNet(Syncfusion.CompoundFile.XlsIO.Net.CompoundFile file, DirectoryEntry entry)
    : base(entry.Name)
  {
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    if (entry == null)
      throw new ArgumentNullException(nameof (entry));
    if (entry.Type != DirectoryEntry.EntryType.Stream)
      throw new ArgumentOutOfRangeException(nameof (entry));
    this.m_parentFile = file;
    this.m_entry = entry;
  }

  public virtual void Open()
  {
    if (this.m_stream != null)
      return;
    this.m_stream = this.m_parentFile.GetEntryStream(this.m_entry);
  }

  public override int Read(byte[] buffer, int offset, int length)
  {
    return this.m_stream.Read(buffer, offset, length);
  }

  public override void Write(byte[] buffer, int offset, int length)
  {
    this.m_stream.Write(buffer, offset, length);
  }

  public override long Seek(long offset, SeekOrigin origin) => this.m_stream.Seek(offset, origin);

  public override void SetLength(long value) => this.m_stream.SetLength(value);

  public override void Close()
  {
    this.Flush();
    this.m_stream.Dispose();
    this.m_stream = (Stream) null;
  }

  public override long Length
  {
    get => this.m_stream == null ? (long) this.m_entry.Size : this.m_stream.Length;
  }

  public override long Position
  {
    get => this.m_stream.Position;
    set => this.m_stream.Position = value;
  }

  public override void Flush()
  {
    if (this.m_stream == null)
      return;
    this.m_parentFile.SetEntryStream(this.m_entry, this.m_stream);
  }

  public override bool CanRead => true;

  public override bool CanSeek => true;

  public override bool CanWrite => true;

  protected override void Dispose(bool disposing)
  {
    if (this.m_stream == null)
      return;
    base.Dispose(disposing);
    this.m_stream.Dispose();
    this.m_stream = (Stream) null;
    this.m_parentFile = (Syncfusion.CompoundFile.XlsIO.Net.CompoundFile) null;
    this.m_entry = (DirectoryEntry) null;
    GC.SuppressFinalize((object) this);
  }
}
