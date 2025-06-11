// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2Context
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class JBIG2Context
{
  private const int MAX_CONN_COMP_WIDTH = 350;
  private const int MAX_CHAR_COMP_WIDTH = 350;
  private const int MAX_WORD_COMP_WIDTH = 1000;
  private const int MAX_COMP_HEIGHT = 120;
  private const int JB_CONN_COMPS = 0;
  private const int JB_CHARACTERS = 1;
  private const int JB_WORDS = 2;
  private const int JB_RANKHAUS = 0;
  private const int JB_CORRELATION = 1;
  private JBIG2Classifier m_classifier;
  private int m_xRes;
  private int m_yRes;
  private bool m_fullHeaders;
  private bool m_pdfPageNumbering;
  private int m_segNumber;
  private int m_symbolTableSegment;
  private Dictionary<int, List<int>> m_pageComps;
  private Dictionary<int, List<int>> m_singleUseSymbols;
  private int m_numGlobalSymbols;
  private List<int> m_pageXRes;
  private List<int> m_pageYRes;
  private List<int> m_pageWidth;
  private List<int> m_pageHeight;
  private SortedDictionary<int, int> m_symbolMap;
  private bool m_refinement;
  private Pixa m_avgTemplates;
  private int m_refineLevel;
  private List<int> m_baseIndexes;

  internal JBIG2Classifier Classifier => this.m_classifier;

  internal int XRes
  {
    get => this.m_xRes;
    set => this.m_xRes = value;
  }

  internal int YRes
  {
    get => this.m_yRes;
    set => this.m_yRes = value;
  }

  internal bool FullHeaders
  {
    get => this.m_fullHeaders;
    set => this.m_fullHeaders = value;
  }

  internal bool PDFPageNumbering
  {
    get => this.m_pdfPageNumbering;
    set => this.m_pdfPageNumbering = value;
  }

  internal int SegNumber
  {
    get => this.m_segNumber;
    set => this.m_segNumber = value;
  }

  internal int SymbolTableSegment
  {
    get => this.m_symbolTableSegment;
    set => this.m_symbolTableSegment = value;
  }

  internal Dictionary<int, List<int>> PageComps
  {
    get => this.m_pageComps;
    set => this.m_pageComps = value;
  }

  internal Dictionary<int, List<int>> SingleUseSymbols
  {
    get => this.m_singleUseSymbols;
    set => this.m_singleUseSymbols = value;
  }

  internal int NumGlobalSymbols
  {
    get => this.m_numGlobalSymbols;
    set => this.m_numGlobalSymbols = value;
  }

  internal List<int> PageXRes
  {
    get => this.m_pageXRes;
    set => this.m_pageXRes = value;
  }

  internal List<int> PageYRes
  {
    get => this.m_pageYRes;
    set => this.m_pageYRes = value;
  }

  internal List<int> PageWidth
  {
    get => this.m_pageWidth;
    set => this.m_pageWidth = value;
  }

  internal List<int> PageHeight
  {
    get => this.m_pageHeight;
    set => this.m_pageHeight = value;
  }

  internal SortedDictionary<int, int> SymbolMap
  {
    get => this.m_symbolMap;
    set => this.m_symbolMap = value;
  }

  internal bool Refinement
  {
    get => this.m_refinement;
    set => this.m_refinement = value;
  }

  internal Pixa AvgTemplates
  {
    get => this.m_avgTemplates;
    set => this.m_avgTemplates = value;
  }

  internal int RefineLevel
  {
    get => this.m_refineLevel;
    set => this.m_refineLevel = value;
  }

  internal List<int> BaseIndexes
  {
    get => this.m_baseIndexes;
    set => this.m_baseIndexes = value;
  }

  internal JBIG2Context(float m_threshold, float weight, int xres, int yres, bool full_headers)
  {
    this.XRes = xres;
    this.YRes = yres;
    this.FullHeaders = full_headers;
    this.PDFPageNumbering = !this.FullHeaders;
    this.SegNumber = 0;
    this.SymbolTableSegment = -1;
    this.AvgTemplates = (Pixa) null;
    this.Refinement = false;
    this.m_classifier = this.JBCorrelationInitWithoutComponents(0, 9999, 9999, m_threshold, weight);
    this.PageWidth = new List<int>();
    this.PageHeight = new List<int>();
    this.PageXRes = new List<int>();
    this.PageYRes = new List<int>();
    this.PageComps = new Dictionary<int, List<int>>();
    this.SingleUseSymbols = new Dictionary<int, List<int>>();
    this.BaseIndexes = new List<int>();
  }

  private JBIG2Classifier JBCorrelationInitWithoutComponents(
    int components,
    int maxwidth,
    int maxheight,
    float thresh,
    float weightfactor)
  {
    return this.JBCorrelationInitInternal(components, maxwidth, maxheight, thresh, weightfactor, 0);
  }

  private JBIG2Classifier JBCorrelationInitInternal(
    int components,
    int maxwidth,
    int maxheight,
    float thresh,
    float weightfactor,
    int keep_components)
  {
    if (components != 0 && components != 1 && components != 2)
      throw new Exception();
    if ((double) thresh < 0.4 || (double) thresh > 0.98)
      throw new Exception();
    if ((double) weightfactor < 0.0 || (double) weightfactor > 1.0)
      throw new Exception();
    if (maxwidth == 0)
    {
      switch (components)
      {
        case 0:
          maxwidth = 350;
          break;
        case 1:
          maxwidth = 350;
          break;
        default:
          maxwidth = 1000;
          break;
      }
    }
    if (maxheight == 0)
      maxheight = 120;
    JBIG2Classifier jbiG2Classifier = this.jbClasserCreate(1, components);
    if (jbiG2Classifier == null)
      throw new NullReferenceException("Classifier is null");
    jbiG2Classifier.MaxWidth = maxwidth;
    jbiG2Classifier.MaxHeight = maxheight;
    jbiG2Classifier.Thresh = thresh;
    jbiG2Classifier.WeightFactor = weightfactor;
    jbiG2Classifier.NaHash = this.NumaHashCreate(5507, 4);
    jbiG2Classifier.KeepPixaa = keep_components;
    return jbiG2Classifier;
  }

  private NumaHash NumaHashCreate(int nbuckets, int initsize)
  {
    return nbuckets > 0 ? new NumaHash(nbuckets, initsize) : throw new ArgumentOutOfRangeException("Negative hash size");
  }

  private JBIG2Classifier jbClasserCreate(int method, int components)
  {
    int num1 = 0;
    int num2 = 1;
    int num3 = 0;
    int num4 = 1;
    int num5 = 2;
    if (method != num1 && method != num2)
    {
      Console.WriteLine("invalid type");
      return (JBIG2Classifier) null;
    }
    if (components == num3 || components == num4 || components == num5)
      return new JBIG2Classifier(method, components, JBIG2Statics.CreateNuma(0), this.PixaaCreate(0), JBIG2Statics.CreatePixa(0), JBIG2Statics.CreatePixa(0), JBIG2Statics.CreateNuma(0), JBIG2Statics.CreateNuma(0), this.CreatePta(0), this.CreatePta(0), JBIG2Statics.CreateNuma(0), JBIG2Statics.CreateNuma(0), this.CreatePta(0));
    Console.WriteLine("invalid type");
    return (JBIG2Classifier) null;
  }

  private Pixaa PixaaCreate(int n)
  {
    if (n <= 0)
      n = 20;
    return new Pixaa(n);
  }

  private Pta CreatePta(int n)
  {
    if (n <= 0)
      n = 20;
    Pta pta = new Pta(n);
    this.PtaChangeRefcount(pta, 1);
    return pta;
  }

  private void PtaChangeRefcount(Pta pta, int delta)
  {
    if (pta == null)
      throw new NullReferenceException("pta not defined");
    pta.RefCount += delta;
  }
}
