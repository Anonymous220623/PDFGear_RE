// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ContinueRecordPublisher
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

internal class ContinueRecordPublisher
{
  private BinaryWriter m_writer;

  public ContinueRecordPublisher(BinaryWriter writer)
  {
    this.m_writer = writer != null ? writer : throw new ArgumentNullException(nameof (writer));
  }

  public int PublishContinue(byte[] data, int start)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    return this.PublishContinue(data, start, data.Length - start, 8224);
  }

  public int PublishContinue(byte[] data, int start, int length)
  {
    return this.PublishContinue(data, start, length, 8224);
  }

  public int PublishContinue(byte[] data, int start, int length, int maxSize)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (data.Length < start || start < 0)
      throw new ArgumentOutOfRangeException(nameof (start));
    if (data.Length < start + length || length < 0)
      throw new ArgumentOutOfRangeException(nameof (length));
    if (maxSize < 0 || maxSize > 8224)
      throw new ArgumentOutOfRangeException(nameof (maxSize));
    int num1 = 0;
    int num2 = start + length;
    int index;
    for (index = start; index < num2; index += maxSize)
    {
      int count = num2 - index < maxSize ? num2 - index : maxSize;
      this.m_writer.Write((ushort) 60);
      this.m_writer.Write((ushort) count);
      this.m_writer.Write(data, index, count);
      num1 += count + 4;
    }
    if (num2 > index)
    {
      int count = num2 - index;
      this.m_writer.Write((ushort) 60);
      this.m_writer.Write((ushort) count);
      this.m_writer.Write(data, index, count);
      num1 += 4 + count;
    }
    return num1;
  }

  public int PublishContinue(
    byte[] data,
    int start,
    BiffRecordRawWithArray destination,
    int offset)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    return this.PublishContinue(data, start, data.Length - start, 8224, destination, offset);
  }

  public int PublishContinue(
    byte[] data,
    int start,
    int length,
    BiffRecordRawWithArray destination,
    int offset)
  {
    return this.PublishContinue(data, start, length, 8224, destination, offset);
  }

  public int PublishContinue(
    byte[] data,
    int start,
    int length,
    int maxSize,
    BiffRecordRawWithArray destination,
    int offset)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (data.Length < start || start < 0)
      throw new ArgumentOutOfRangeException(nameof (start));
    if (data.Length < start + length || length < 0)
      throw new ArgumentOutOfRangeException(nameof (length));
    if (maxSize < 0 || maxSize > 8224)
      throw new ArgumentOutOfRangeException(nameof (maxSize));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    int num1 = 0;
    int num2 = start + length;
    int pos = start;
    bool autoGrowData = destination.AutoGrowData;
    destination.AutoGrowData = true;
    for (; pos < num2; pos += maxSize)
    {
      int length1 = num2 - pos < maxSize ? num2 - pos : maxSize;
      destination.SetUInt16(offset, (ushort) 60);
      destination.SetUInt16(offset + 2, (ushort) length1);
      destination.SetBytes(offset + 4, data, pos, length1);
      num1 += length1 + 4;
      offset += length1 + 4;
    }
    if (num2 > pos)
    {
      int length2 = num2 - pos;
      destination.SetUInt16(offset, (ushort) 60);
      destination.SetUInt16(offset + 2, (ushort) length2);
      destination.SetBytes(offset + 4, data, pos, length2);
      num1 += length2 + 4;
      offset += length2 + 4;
    }
    destination.AutoGrowData = autoGrowData;
    return num1;
  }
}
