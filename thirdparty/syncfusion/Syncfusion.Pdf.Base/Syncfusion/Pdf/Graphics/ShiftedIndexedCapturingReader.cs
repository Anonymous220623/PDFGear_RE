// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.ShiftedIndexedCapturingReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class ShiftedIndexedCapturingReader : CatalogedReaderBase
{
  private readonly CatalogedReader m_baseReader;
  private readonly int m_baseOffset;

  internal ShiftedIndexedCapturingReader(
    CatalogedReader baseReader,
    int baseOffset,
    bool isBigEndian)
    : base(isBigEndian)
  {
    if (baseOffset < 0)
      throw new Exception("Invalid offset");
    this.m_baseReader = baseReader;
    this.m_baseOffset = baseOffset;
  }

  internal override CatalogedReaderBase WithByteOrder(bool isBigEndian)
  {
    return isBigEndian != this.IsBigEndian ? (CatalogedReaderBase) new ShiftedIndexedCapturingReader(this.m_baseReader, this.m_baseOffset, isBigEndian) : (CatalogedReaderBase) this;
  }

  internal override CatalogedReaderBase WithShiftedBaseOffset(int shift)
  {
    return shift != 0 ? (CatalogedReaderBase) new ShiftedIndexedCapturingReader(this.m_baseReader, this.m_baseOffset + shift, this.IsBigEndian) : (CatalogedReaderBase) this;
  }

  internal override int ToUnshiftedOffset(int localOffset) => localOffset + this.m_baseOffset;

  internal override byte ReadByte(int index)
  {
    return this.m_baseReader.ReadByte(this.m_baseOffset + index);
  }

  internal override byte[] GetBytes(int index, int count)
  {
    return this.m_baseReader.GetBytes(this.m_baseOffset + index, count);
  }

  internal override bool IsValidIndex(int index, int length)
  {
    return this.m_baseReader.IsValidIndex(index + this.m_baseOffset, length);
  }

  internal override long Length => this.m_baseReader.Length - (long) this.m_baseOffset;
}
