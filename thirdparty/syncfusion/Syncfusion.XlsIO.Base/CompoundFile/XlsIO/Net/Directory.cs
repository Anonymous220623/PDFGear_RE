// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Net.Directory
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Net;

public class Directory
{
  private List<DirectoryEntry> m_lstEntries = new List<DirectoryEntry>();

  public List<DirectoryEntry> Entries => this.m_lstEntries;

  public Directory()
  {
  }

  public Directory(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int offset = 0;
    int length = data.Length;
    this.m_lstEntries = new List<DirectoryEntry>();
    int entryId = 0;
    while (offset < length)
    {
      this.m_lstEntries.Add(new DirectoryEntry(data, offset, entryId));
      offset += 128 /*0x80*/;
      ++entryId;
    }
  }

  public int FindEmpty()
  {
    int empty = -1;
    int index = 0;
    for (int count = this.m_lstEntries.Count; index < count; ++index)
    {
      if (this.m_lstEntries[index].Type == DirectoryEntry.EntryType.Invalid)
      {
        empty = index;
        break;
      }
    }
    return empty;
  }

  public void Add(DirectoryEntry entry)
  {
    int empty = this.FindEmpty();
    if (empty >= 0)
    {
      this.m_lstEntries[empty] = entry;
      entry.EntryId = empty;
    }
    else
      this.m_lstEntries.Add(entry);
  }

  public void Write(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    int index = 0;
    for (int count = this.m_lstEntries.Count; index < count; ++index)
      this.m_lstEntries[index].Write(stream);
  }
}
