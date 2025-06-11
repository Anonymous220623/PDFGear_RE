// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Jbig2StreamReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class Jbig2StreamReader
{
  private byte[] m_data;
  private int m_bitPointer = 7;
  internal int bytePointer;

  internal Jbig2StreamReader(byte[] data) => this.m_data = data;

  internal short ReadByte()
  {
    return (short) ((int) this.m_data[this.bytePointer++] & (int) byte.MaxValue);
  }

  internal void ReadByte(short[] buf)
  {
    for (int index = 0; index < buf.Length; ++index)
    {
      if (this.bytePointer < this.m_data.Length)
        buf[index] = (short) ((int) this.m_data[this.bytePointer++] & (int) byte.MaxValue);
    }
  }

  internal int ReadBit()
  {
    int num = ((int) this.ReadByte() & (int) (short) (1 << this.m_bitPointer)) >> this.m_bitPointer;
    --this.m_bitPointer;
    if (this.m_bitPointer == -1)
      this.m_bitPointer = 7;
    else
      this.MovePointer(-1);
    return num;
  }

  internal int ReadBits(int num)
  {
    int num1 = 0;
    for (int index = 0; index < num; ++index)
      num1 = num1 << 1 | this.ReadBit();
    return num1;
  }

  internal void MovePointer(int ammount) => this.bytePointer += ammount;

  internal void ConsumeRemainingBits()
  {
    if (this.m_bitPointer == 7)
      return;
    this.ReadBits(this.m_bitPointer + 1);
  }

  internal bool Getfinished() => this.bytePointer == this.m_data.Length;
}
