// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.BitParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class BitParser
{
  private byte[] source;
  private int bits;
  private int mask;
  internal int sourcePointer;
  private int bitPointer;

  public BitParser(byte[] source, int intSizeInBits)
  {
    this.source = source;
    this.bits = intSizeInBits;
    this.mask = (1 << this.bits) - 1;
  }

  public int ReadBits()
  {
    if (this.sourcePointer >= this.source.Length)
      return -1;
    switch (this.bits)
    {
      case 8:
        return (int) this.source[this.sourcePointer++];
      case 16 /*0x10*/:
        return (int) this.source[this.sourcePointer++] << 8 | (int) this.source[this.sourcePointer++];
      default:
        int num = (int) this.source[this.sourcePointer] >> 8 - this.bitPointer - this.bits & this.mask;
        this.bitPointer += this.bits;
        if (this.bitPointer == 8)
        {
          ++this.sourcePointer;
          this.bitPointer = 0;
        }
        return num;
    }
  }

  public bool MoveToNextRow()
  {
    if (this.sourcePointer >= this.source.Length)
      return false;
    if (this.bitPointer != 0)
    {
      ++this.sourcePointer;
      this.bitPointer = 0;
    }
    return true;
  }
}
