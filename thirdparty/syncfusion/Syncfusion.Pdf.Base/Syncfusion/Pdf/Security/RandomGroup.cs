// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RandomGroup
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RandomGroup : IRandom, IDisposable
{
  private readonly RandomGroup.SourceEntry[] m_sources;
  private RandomGroup.SourceEntry m_cse;
  private readonly long m_size;

  internal RandomGroup(ICollection<IRandom> sources)
  {
    this.m_sources = new RandomGroup.SourceEntry[sources.Count];
    long offset = 0;
    int index = 0;
    foreach (IRandom source in (IEnumerable<IRandom>) sources)
    {
      this.m_sources[index] = new RandomGroup.SourceEntry(index, source, offset);
      ++index;
      offset += source.Length;
    }
    this.m_size = offset;
    this.m_cse = this.m_sources[sources.Count - 1];
    this.SourceInUse(this.m_cse.m_source);
  }

  protected internal int GetStartIndex(long offset)
  {
    return offset >= this.m_cse.m_startByte ? this.m_cse.m_index : 0;
  }

  private RandomGroup.SourceEntry GetEntry(long offset)
  {
    if (offset >= this.m_size)
      return (RandomGroup.SourceEntry) null;
    if (offset >= this.m_cse.m_startByte && offset <= this.m_cse.m_endByte)
      return this.m_cse;
    this.SourceReleased(this.m_cse.m_source);
    for (int startIndex = this.GetStartIndex(offset); startIndex < this.m_sources.Length; ++startIndex)
    {
      if (offset >= this.m_sources[startIndex].m_startByte && offset <= this.m_sources[startIndex].m_endByte)
      {
        this.m_cse = this.m_sources[startIndex];
        this.SourceInUse(this.m_cse.m_source);
        return this.m_cse;
      }
    }
    return (RandomGroup.SourceEntry) null;
  }

  protected internal virtual void SourceReleased(IRandom source)
  {
  }

  protected internal virtual void SourceInUse(IRandom source)
  {
  }

  public virtual int Get(long position)
  {
    RandomGroup.SourceEntry entry = this.GetEntry(position);
    return entry == null ? -1 : entry.m_source.Get(entry.OffsetN(position));
  }

  public virtual int Get(long position, byte[] bytes, int off, int len)
  {
    RandomGroup.SourceEntry entry = this.GetEntry(position);
    if (entry == null)
      return -1;
    long position1 = entry.OffsetN(position);
    int length;
    for (length = len; length > 0 && entry != null && position1 <= entry.m_source.Length; entry = this.GetEntry(position))
    {
      int num = entry.m_source.Get(position1, bytes, off, length);
      if (num != -1)
      {
        off += num;
        position += (long) num;
        length -= num;
        position1 = 0L;
      }
      else
        break;
    }
    return length != len ? len - length : -1;
  }

  public virtual long Length => this.m_size;

  public virtual void Close()
  {
    foreach (RandomGroup.SourceEntry source in this.m_sources)
      source.m_source.Close();
  }

  public virtual void Dispose() => this.Close();

  private sealed class SourceEntry
  {
    internal readonly IRandom m_source;
    internal readonly long m_startByte;
    internal readonly long m_endByte;
    internal readonly int m_index;

    internal SourceEntry(int index, IRandom source, long offset)
    {
      this.m_index = index;
      this.m_source = source;
      this.m_startByte = offset;
      this.m_endByte = offset + source.Length - 1L;
    }

    internal long OffsetN(long absoluteOffset) => absoluteOffset - this.m_startByte;
  }
}
