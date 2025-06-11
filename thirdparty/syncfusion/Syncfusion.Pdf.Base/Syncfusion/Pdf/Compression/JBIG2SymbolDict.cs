// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2SymbolDict
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal struct JBIG2SymbolDict
{
  private byte sdhuff;
  private byte sdrefagg;
  private byte sdhuffdh;
  private byte sdhuffdw;
  private byte sdhuffbmsize;
  private byte sdhuffagginst;
  private byte bmcontext;
  private byte bmcontextretained;
  private byte reserved;
  private byte m_sdtemplate;
  private byte m_sdrtemplate;
  private sbyte m_a1x;
  private sbyte m_a1y;
  private sbyte m_a2x;
  private sbyte m_a2y;
  private sbyte m_a3x;
  private sbyte m_a3y;
  private sbyte m_a4x;
  private sbyte m_a4y;
  private uint m_exSyms;
  private uint m_newSyms;

  internal byte sdtemplate
  {
    get => this.m_sdtemplate;
    set => this.m_sdtemplate = value;
  }

  internal byte sdrtemplate
  {
    get => this.m_sdrtemplate;
    set => this.m_sdrtemplate = value;
  }

  internal sbyte a1x
  {
    get => this.m_a1x;
    set => this.m_a1x = value;
  }

  internal sbyte a1y
  {
    get => this.m_a1y;
    set => this.m_a1y = value;
  }

  internal sbyte a2x
  {
    get => this.m_a2x;
    set => this.m_a2x = value;
  }

  internal sbyte a2y
  {
    get => this.m_a2y;
    set => this.m_a2y = value;
  }

  internal sbyte a3x
  {
    get => this.m_a3x;
    set => this.m_a3x = value;
  }

  internal sbyte a3y
  {
    get => this.m_a3y;
    set => this.m_a3y = value;
  }

  internal sbyte a4x
  {
    get => this.m_a4x;
    set => this.m_a4x = value;
  }

  internal sbyte a4y
  {
    get => this.m_a4y;
    set => this.m_a4y = value;
  }

  internal uint ExSyms
  {
    get => this.m_exSyms;
    set => this.m_exSyms = value;
  }

  internal uint NewSyms
  {
    get => this.m_newSyms;
    set => this.m_newSyms = value;
  }
}
