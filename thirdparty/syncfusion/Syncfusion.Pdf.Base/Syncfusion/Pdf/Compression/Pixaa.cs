// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Pixaa
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Pixaa
{
  private int m_nalloc;
  private int m_n;
  private List<Syncfusion.Pdf.Compression.Pixa> m_pixa;
  private List<Syncfusion.Pdf.Compression.Boxa> m_boxa;

  internal int N
  {
    get => this.m_n;
    set => this.m_n = value;
  }

  internal int Nalloc
  {
    get => this.m_nalloc;
    set => this.m_nalloc = value;
  }

  internal List<Syncfusion.Pdf.Compression.Pixa> Pixa
  {
    get => this.m_pixa;
    set => this.m_pixa = value;
  }

  internal List<Syncfusion.Pdf.Compression.Boxa> Boxa
  {
    get => this.m_boxa;
    set => this.m_boxa = value;
  }

  internal Pixaa(int n)
  {
    this.Nalloc = n;
    this.N = 0;
    this.Pixa = new List<Syncfusion.Pdf.Compression.Pixa>();
    this.Boxa = new List<Syncfusion.Pdf.Compression.Boxa>();
  }
}
