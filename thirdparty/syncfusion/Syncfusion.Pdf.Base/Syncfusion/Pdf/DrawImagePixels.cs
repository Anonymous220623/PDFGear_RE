// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DrawImagePixels
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class DrawImagePixels
{
  private const int m_shiftBit = 5;
  private const int m_maskBit = 31 /*0x1F*/;
  private int m_range;
  private uint[] m_bitValue;

  internal DrawImagePixels(int range)
  {
    this.m_range = range;
    this.m_bitValue = new uint[this.Subfix(range + 31 /*0x1F*/)];
  }

  internal void SetIndex(int fromIndex, int toIndex)
  {
    for (int bitIndex = fromIndex; bitIndex <= toIndex; ++bitIndex)
    {
      int index = this.Subfix(bitIndex) - 2;
      this.ConformLength(index + 1);
      this.m_bitValue[index] |= (uint) (1 << bitIndex);
    }
  }

  internal void SetBitdata(int index)
  {
    int index1 = this.Subfix(index);
    this.ConformLength(index1 + 1);
    this.m_bitValue[index1] |= (uint) (1 << index);
  }

  internal bool GetValue(int index)
  {
    bool flag = false;
    if (index < this.m_range)
      flag = ((long) this.m_bitValue[this.Subfix(index)] & (long) (1 << index)) != 0L;
    return flag;
  }

  private int Subfix(int bitIndex) => bitIndex >> 5;

  private void ConformLength(int length)
  {
    if (length <= this.m_bitValue.Length)
      return;
    int length1 = 2 * this.m_bitValue.Length;
    if (length1 < length)
      length1 = length;
    uint[] destinationArray = new uint[length1];
    Array.Copy((Array) this.m_bitValue, (Array) destinationArray, this.m_bitValue.Length);
    this.m_bitValue = destinationArray;
  }
}
