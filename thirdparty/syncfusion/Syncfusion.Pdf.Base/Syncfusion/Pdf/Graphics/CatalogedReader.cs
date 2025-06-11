// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.CatalogedReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class CatalogedReader : CatalogedReaderBase
{
  private const int m_defaultSegmentLength = 2048 /*0x0800*/;
  private readonly Stream m_stream;
  private readonly int m_length;
  private readonly List<byte[]> m_segments = new List<byte[]>();
  private bool m_isStreamFinished;
  private int m_streamLength;

  internal CatalogedReader(Stream stream)
    : base(true)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_length = 2048 /*0x0800*/;
    this.m_stream = stream;
  }

  internal CatalogedReader(Stream stream, bool isBigEndian)
    : base(isBigEndian)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_length = 2048 /*0x0800*/;
    this.m_stream = stream;
  }

  internal override long Length
  {
    get
    {
      if (this.m_stream.Length > 0L)
        return this.m_stream.Length;
      this.IsValidIndex(int.MaxValue, 1);
      return (long) this.m_streamLength;
    }
  }

  internal override bool IsValidIndex(int index, int bytesRequested)
  {
    if (index < 0 || bytesRequested < 0)
      return false;
    long num1 = (long) index + (long) bytesRequested - 1L;
    if (num1 > (long) int.MaxValue)
      return false;
    int num2 = (int) num1;
    if (this.m_isStreamFinished)
      return num2 < this.m_streamLength;
    int num3 = num2 / this.m_length;
    while (num3 >= this.m_segments.Count)
    {
      byte[] buffer = new byte[this.m_length];
      int offset = 0;
      while (!this.m_isStreamFinished && offset != this.m_length)
      {
        int num4 = this.m_stream.Read(buffer, offset, this.m_length - offset);
        if (num4 == 0)
        {
          this.m_isStreamFinished = true;
          this.m_streamLength = this.m_segments.Count * this.m_length + offset;
          if (num2 >= this.m_streamLength)
          {
            this.m_segments.Add(buffer);
            return false;
          }
        }
        else
          offset += num4;
      }
      this.m_segments.Add(buffer);
    }
    return true;
  }

  internal override int ToUnshiftedOffset(int offset) => offset;

  internal override byte ReadByte(int index)
  {
    if (this.IsValidIndex(index, 1))
      return this.m_segments[index / this.m_length][index % this.m_length];
    throw new Exception("Invalid index to read byte");
  }

  internal override byte[] GetBytes(int index, int count)
  {
    byte[] destinationArray = new byte[count];
    if (this.IsValidIndex(index, count))
    {
      int val1 = count;
      int num = index;
      int destinationIndex = 0;
      while (val1 != 0)
      {
        int index1 = num / this.m_length;
        int sourceIndex = num % this.m_length;
        int length = Math.Min(val1, this.m_length - sourceIndex);
        Array.Copy((Array) this.m_segments[index1], sourceIndex, (Array) destinationArray, destinationIndex, length);
        val1 -= length;
        num += length;
        destinationIndex += length;
      }
    }
    return destinationArray;
  }

  internal override CatalogedReaderBase WithShiftedBaseOffset(int shift)
  {
    return shift != 0 ? (CatalogedReaderBase) new ShiftedIndexedCapturingReader(this, shift, this.IsBigEndian) : (CatalogedReaderBase) this;
  }

  internal override CatalogedReaderBase WithByteOrder(bool isBigEndian)
  {
    return isBigEndian != this.IsBigEndian ? (CatalogedReaderBase) new ShiftedIndexedCapturingReader(this, 0, isBigEndian) : (CatalogedReaderBase) this;
  }
}
