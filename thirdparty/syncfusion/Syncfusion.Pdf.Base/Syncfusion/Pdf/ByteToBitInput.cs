// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ByteToBitInput
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class ByteToBitInput
{
  internal ByteInputBuffer in_Renamed;
  internal int bbuf;
  internal int bpos = -1;

  public ByteToBitInput(ByteInputBuffer in_Renamed) => this.in_Renamed = in_Renamed;

  public int readBit()
  {
    if (this.bpos < 0)
    {
      if ((this.bbuf & (int) byte.MaxValue) != (int) byte.MaxValue)
      {
        this.bbuf = this.in_Renamed.read();
        this.bpos = 7;
      }
      else
      {
        this.bbuf = this.in_Renamed.read();
        this.bpos = 6;
      }
    }
    return this.bbuf >> this.bpos-- & 1;
  }

  public virtual bool checkBytePadding()
  {
    if (this.bpos < 0 && (this.bbuf & (int) byte.MaxValue) == (int) byte.MaxValue)
    {
      this.bbuf = this.in_Renamed.read();
      this.bpos = 6;
    }
    if (this.bpos >= 0 && (this.bbuf & (1 << this.bpos + 1) - 1) != 85 >> 7 - this.bpos)
      return true;
    if (this.bbuf != -1)
    {
      if (this.bbuf == (int) byte.MaxValue && this.bpos == 0)
      {
        if ((this.in_Renamed.read() & (int) byte.MaxValue) >= 128 /*0x80*/)
          return true;
      }
      else if (this.in_Renamed.read() != -1)
        return true;
    }
    return false;
  }

  internal void flush()
  {
    this.bbuf = 0;
    this.bpos = -1;
  }

  internal void setByteArray(byte[] buf, int off, int len)
  {
    this.in_Renamed.setByteArray(buf, off, len);
    this.bbuf = 0;
    this.bpos = -1;
  }
}
