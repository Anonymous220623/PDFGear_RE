// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Sarray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Sarray
{
  private int m_nalloc;
  private int m_n;
  private List<string> m_array;
  private int refcount;

  internal int Nalloc
  {
    get => this.m_nalloc;
    set => this.m_nalloc = value;
  }

  internal int N
  {
    get => this.m_n;
    set => this.m_n = value;
  }

  internal List<string> Array
  {
    get => this.m_array;
    set => this.m_array = value;
  }

  internal Sarray(int n)
  {
    this.Nalloc = n;
    this.N = 0;
    this.refcount = 1;
  }
}
