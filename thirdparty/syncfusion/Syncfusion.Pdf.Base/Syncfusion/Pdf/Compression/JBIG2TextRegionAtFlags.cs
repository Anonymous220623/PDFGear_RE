// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2TextRegionAtFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal struct JBIG2TextRegionAtFlags
{
  private int m_a1x;
  private int m_a1y;
  private int m_a2x;
  private int m_a2y;

  internal int a1x
  {
    get => this.m_a1x;
    set => this.m_a1x = value;
  }

  internal int a1y
  {
    get => this.m_a1y;
    set => this.m_a1y = value;
  }

  internal int a2x
  {
    get => this.m_a2x;
    set => this.m_a2x = value;
  }

  internal int a2y
  {
    get => this.m_a2y;
    set => this.m_a2y = value;
  }
}
