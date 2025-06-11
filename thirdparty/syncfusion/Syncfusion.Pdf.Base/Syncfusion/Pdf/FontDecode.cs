// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.FontDecode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class FontDecode
{
  internal MemoryStream m_fontStream = new MemoryStream();
  internal FontHeader m_fontHeader;
  internal int m_tableCount;
  private int tagOffset;
  private int offset;
  private int previousLength;

  internal MemoryStream CreateFontStream(List<TableEntry> entries)
  {
    this.m_tableCount = entries.Count;
    FontHeader head = new FontHeader()
    {
      scalarType = 65536 /*0x010000*/,
      noOfTables = (short) this.m_tableCount
    };
    head.searchRange = (short) this.GetSearchRange((int) head.noOfTables);
    head.entrySelector = (short) this.GetEntrySelector((int) head.noOfTables);
    head.rangeShift = (short) this.GetRangeShift((int) head.noOfTables, (int) head.searchRange);
    this.WriteHeader(head);
    this.tagOffset = 12 + entries.Count * 16 /*0x10*/;
    foreach (TableEntry entry in entries)
    {
      this.tagOffset += this.previousLength;
      entry.offset = this.tagOffset;
      this.previousLength = entry.length;
      this.WriteEntry(entry);
    }
    foreach (TableEntry entry in entries)
      this.WriteBytes(entry.bytes);
    this.m_fontStream.Capacity = (int) this.m_fontStream.Length + 1;
    return this.m_fontStream;
  }

  private void WriteHeader(FontHeader head)
  {
    this.m_fontStream.Position = 0L;
    this.WriteInt(head.scalarType);
    this.WriteShort(head.noOfTables);
    this.WriteShort(head.searchRange);
    this.WriteShort(head.entrySelector);
    this.WriteShort(head.rangeShift);
    this.offset = (int) this.m_fontStream.Position;
  }

  private void WriteEntry(TableEntry entry)
  {
    this.WriteString(entry.id);
    this.WriteInt(entry.checkSum);
    this.WriteInt(entry.offset);
    this.WriteInt(entry.length);
  }

  private void WriteShort(short value)
  {
    byte[] buffer = new byte[2]{ (byte) 0, (byte) value };
    buffer[0] = (byte) ((uint) value >> 8);
    this.m_fontStream.Write(buffer, 0, 2);
  }

  private void WriteInt(int value)
  {
    byte[] buffer = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) value
    };
    buffer[2] = (byte) (value >> 8);
    buffer[1] = (byte) (value >> 16 /*0x10*/);
    buffer[0] = (byte) (value >> 24);
    this.m_fontStream.Write(buffer, 0, 4);
  }

  private void WriteString(string value)
  {
    byte[] buffer = new byte[value.Length];
    int index = 0;
    foreach (char ch in value)
    {
      buffer[index] = (byte) ch;
      ++index;
    }
    this.m_fontStream.Write(buffer, 0, 4);
  }

  public void WriteBytes(byte[] buffer) => this.m_fontStream.Write(buffer, 0, buffer.Length);

  private int GetSearchRange(int noOfTables)
  {
    int num = 2;
    while (num * 2 <= noOfTables)
      num *= 2;
    return num * 16 /*0x10*/;
  }

  private int GetEntrySelector(int noOfTables)
  {
    int a = 2;
    while (a * 2 <= noOfTables)
      a *= 2;
    return (int) Math.Log((double) a, 2.0);
  }

  private int GetRangeShift(int noOfTables, int searchRange)
  {
    return noOfTables * 16 /*0x10*/ - searchRange;
  }
}
