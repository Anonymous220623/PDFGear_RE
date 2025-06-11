// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PktHeaderBitReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class PktHeaderBitReader
{
  internal JPXRandomAccessStream in_Renamed;
  internal MemoryStream bais;
  internal bool usebais;
  internal int bbuf;
  internal int bpos;
  internal int nextbbuf;

  internal PktHeaderBitReader(JPXRandomAccessStream in_Renamed)
  {
    this.in_Renamed = in_Renamed;
    this.usebais = false;
  }

  internal PktHeaderBitReader(MemoryStream bais)
  {
    this.bais = bais;
    this.usebais = true;
  }

  internal int readBit()
  {
    if (this.bpos == 0)
    {
      if (this.bbuf != (int) byte.MaxValue)
      {
        this.bbuf = !this.usebais ? (int) this.in_Renamed.read() : this.bais.ReadByte();
        this.bpos = 8;
        if (this.bbuf == (int) byte.MaxValue)
          this.nextbbuf = !this.usebais ? (int) this.in_Renamed.read() : this.bais.ReadByte();
      }
      else
      {
        this.bbuf = this.nextbbuf;
        this.bpos = 7;
      }
    }
    return this.bbuf >> --this.bpos & 1;
  }

  internal int readBits(int n)
  {
    if (n <= this.bpos)
      return this.bbuf >> (this.bpos -= n) & (1 << n) - 1;
    int num1 = 0;
    do
    {
      int num2 = num1 << this.bpos;
      n -= this.bpos;
      num1 = num2 | this.readBits(this.bpos);
      if (this.bbuf != (int) byte.MaxValue)
      {
        this.bbuf = !this.usebais ? (int) this.in_Renamed.read() : this.bais.ReadByte();
        this.bpos = 8;
        if (this.bbuf == (int) byte.MaxValue)
          this.nextbbuf = !this.usebais ? (int) this.in_Renamed.read() : this.bais.ReadByte();
      }
      else
      {
        this.bbuf = this.nextbbuf;
        this.bpos = 7;
      }
    }
    while (n > this.bpos);
    return num1 << n | this.bbuf >> (this.bpos -= n) & (1 << n) - 1;
  }

  internal virtual void sync()
  {
    this.bbuf = 0;
    this.bpos = 0;
  }

  internal virtual void setInput(JPXRandomAccessStream in_Renamed)
  {
    this.in_Renamed = in_Renamed;
    this.bbuf = 0;
    this.bpos = 0;
  }

  internal virtual void setInput(MemoryStream bais)
  {
    this.bais = bais;
    this.bbuf = 0;
    this.bpos = 0;
  }
}
