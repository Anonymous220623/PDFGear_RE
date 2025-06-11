// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JbTemplatesState
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class JbTemplatesState
{
  private JBIG2Classifier m_classer;
  private int m_w;
  private int m_h;
  private int m_i;
  private Numa m_numa;
  private int m_n;

  internal JBIG2Classifier Classer
  {
    get => this.m_classer;
    set => this.m_classer = value;
  }

  internal int W
  {
    get => this.m_w;
    set => this.m_w = value;
  }

  internal int H
  {
    get => this.m_h;
    set => this.m_h = value;
  }

  internal int I
  {
    get => this.m_i;
    set => this.m_i = value;
  }

  internal Numa Numa
  {
    get => this.m_numa;
    set => this.m_numa = value;
  }

  internal int N
  {
    get => this.m_n;
    set => this.m_n = value;
  }
}
