// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.IntEncRange
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class IntEncRange
{
  private int m_bot;
  private int m_top;
  private int m_data;
  private int m_bits;
  private short m_delta;
  private int m_intBits;

  internal int Bot
  {
    get => this.m_bot;
    set => this.m_bot = value;
  }

  internal int Top
  {
    get => this.m_top;
    set => this.m_top = value;
  }

  internal int Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  internal int Bits
  {
    get => this.m_bits;
    set => this.m_bits = value;
  }

  internal short Delta
  {
    get => this.m_delta;
    set => this.m_delta = value;
  }

  internal int IntBits
  {
    get => this.m_intBits;
    set => this.m_intBits = value;
  }

  internal IntEncRange(int bot, int top, int data, int bits, short delta, int intbits)
  {
    this.Bot = bot;
    this.Top = top;
    this.Data = data;
    this.Bits = bits;
    this.Delta = delta;
    this.IntBits = intbits;
  }
}
