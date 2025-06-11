// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.NumaHash
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class NumaHash
{
  private int m_nBuckets;
  private int m_initSize;
  private Dictionary<int, Syncfusion.Pdf.Compression.Numa> m_numa;

  internal int NBuckets
  {
    get => this.m_nBuckets;
    set => this.m_nBuckets = value;
  }

  internal int InitSize
  {
    get => this.m_initSize;
    set => this.m_initSize = value;
  }

  internal Dictionary<int, Syncfusion.Pdf.Compression.Numa> Numa
  {
    get => this.m_numa;
    set => this.m_numa = value;
  }

  internal NumaHash(int nbuckets, int initsize)
  {
    this.NBuckets = nbuckets;
    this.InitSize = initsize;
    this.Numa = new Dictionary<int, Syncfusion.Pdf.Compression.Numa>();
  }
}
