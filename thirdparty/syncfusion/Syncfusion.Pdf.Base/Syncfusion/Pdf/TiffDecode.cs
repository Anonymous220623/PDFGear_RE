// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TiffDecode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class TiffDecode
{
  internal const int LittleEndianVersion = 42;
  internal const int BigEndianVersion = 43;
  internal const short BigEndian = 19789;
  internal const short LittleEndian = 18761;
  internal const short MdiLittleEndian = 20549;
  internal MemoryStream m_stream;
  internal TiffHeader m_tiffHeader;
  internal TiffDirectoryEntry m_directory = new TiffDirectoryEntry();
  internal List<TiffDirectoryEntry> directoryEntries = new List<TiffDirectoryEntry>();

  public TiffDecode() => this.m_stream = new MemoryStream();

  internal void SetField(int count, int offset, TiffTag tag, TiffType type)
  {
    this.directoryEntries.Add(new TiffDirectoryEntry()
    {
      DirectoryCount = count,
      DirectoryOffset = (uint) offset,
      DirectoryTag = tag,
      DirectoryType = type
    });
  }

  internal void WriteHeader(TiffHeader header)
  {
    this.WriteShort(header.m_byteOrder);
    this.WriteShort(header.m_version);
    this.WriteInt((int) header.m_dirOffset);
  }

  internal void WriteDirEntry(List<TiffDirectoryEntry> entries)
  {
    int count = entries.Count;
    this.WriteShort((short) count);
    for (int index = 0; index < count; ++index)
    {
      this.WriteShort((short) entries[index].DirectoryTag);
      this.WriteShort((short) entries[index].DirectoryType);
      this.WriteInt(entries[index].DirectoryCount);
      this.WriteInt((int) entries[index].DirectoryOffset);
    }
    this.WriteInt(0);
  }

  private void WriteShort(short value)
  {
    this.m_stream.Write(new byte[2]
    {
      (byte) value,
      (byte) ((uint) value >> 8)
    }, 0, 2);
  }

  private void WriteInt(int value)
  {
    this.m_stream.Write(new byte[4]
    {
      (byte) value,
      (byte) (value >> 8),
      (byte) (value >> 16 /*0x10*/),
      (byte) (value >> 24)
    }, 0, 4);
  }
}
