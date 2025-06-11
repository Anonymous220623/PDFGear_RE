// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2TextRegion
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal struct JBIG2TextRegion
{
  private byte comb_operator;
  private uint m_width;
  private uint m_height;
  private uint m_x;
  private uint m_y;
  private byte m_sbcombop2;
  private byte m_sbdefpixel;
  private byte m_sbdsoffset;
  private byte m_sbrtemplate;
  private byte m_sbhuff;
  private byte m_sbRefine;
  private byte m_logSBStrips;
  private byte m_refcorner;
  private byte m_transposed;
  private byte m_sbcombop1;

  internal uint Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  internal uint Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  internal uint X
  {
    get => this.m_x;
    set => this.m_x = value;
  }

  internal uint Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }

  private byte sbcombop2
  {
    get => this.m_sbcombop2;
    set => this.m_sbcombop2 = value;
  }

  private byte sbdefpixel
  {
    get => this.m_sbdefpixel;
    set => this.m_sbdefpixel = value;
  }

  private byte sbdsoffset
  {
    get => this.m_sbdsoffset;
    set => this.m_sbdsoffset = value;
  }

  internal byte Sbrtemplate
  {
    get => this.m_sbrtemplate;
    set => this.m_sbrtemplate = value;
  }

  private byte sbhuff
  {
    get => this.m_sbhuff;
    set => this.m_sbhuff = value;
  }

  internal byte SBRefine
  {
    get => this.m_sbRefine;
    set => this.m_sbRefine = value;
  }

  internal byte LogSBStrips
  {
    get => this.m_logSBStrips;
    set => this.m_logSBStrips = value;
  }

  private byte refcorner
  {
    get => this.m_refcorner;
    set => this.m_refcorner = value;
  }

  private byte transposed
  {
    get => this.m_transposed;
    set => this.m_transposed = value;
  }

  private byte sbcombop1
  {
    get => this.m_sbcombop1;
    set => this.m_sbcombop1 = value;
  }
}
