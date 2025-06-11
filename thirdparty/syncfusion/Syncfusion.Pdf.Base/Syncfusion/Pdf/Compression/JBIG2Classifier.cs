// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2Classifier
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class JBIG2Classifier
{
  private int m_method;
  private int m_components;
  private int m_maxWidth;
  private int m_maxHeight;
  private int m_nPages;
  private int m_baseIndex;
  private Numa m_naComps;
  private int m_sizeHaus;
  private float m_rankHaus;
  private float m_thresh;
  private float m_weightFactor;
  private Numa m_naArea;
  private int m_w;
  private int m_h;
  private int m_nClass;
  private int m_keepPixaa;
  private Pixaa m_pixaa;
  private Pixa m_pixat;
  private Pixa m_pixatd;
  private NumaHash m_naHash;
  private Numa m_nafgt;
  private Pta m_ptac;
  private Pta m_ptaTemplate;
  private Numa m_naClass;
  private Numa m_naPage;
  private Pta m_ptaUL;
  private Pta m_ptaLL;

  internal int Method
  {
    get => this.m_method;
    set => this.m_method = value;
  }

  internal int Components
  {
    get => this.m_components;
    set => this.m_components = value;
  }

  internal int MaxWidth
  {
    get => this.m_maxWidth;
    set => this.m_maxWidth = value;
  }

  internal int MaxHeight
  {
    get => this.m_maxHeight;
    set => this.m_maxHeight = value;
  }

  internal int NPages
  {
    get => this.m_nPages;
    set => this.m_nPages = value;
  }

  internal int BaseIndex
  {
    get => this.m_baseIndex;
    set => this.m_baseIndex = value;
  }

  internal Numa NaComps
  {
    get => this.m_naComps;
    set => this.m_naComps = value;
  }

  internal int SizeHaus
  {
    get => this.m_sizeHaus;
    set => this.m_sizeHaus = value;
  }

  internal float RankHaus
  {
    get => this.m_rankHaus;
    set => this.m_rankHaus = value;
  }

  internal float Thresh
  {
    get => this.m_thresh;
    set => this.m_thresh = value;
  }

  internal float WeightFactor
  {
    get => this.m_weightFactor;
    set => this.m_weightFactor = value;
  }

  internal Numa NaArea
  {
    get => this.m_naArea;
    set => this.m_naArea = value;
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

  internal int NClass
  {
    get => this.m_nClass;
    set => this.m_nClass = value;
  }

  internal int KeepPixaa
  {
    get => this.m_keepPixaa;
    set => this.m_keepPixaa = value;
  }

  internal Pixaa Pixaa
  {
    get => this.m_pixaa;
    set => this.m_pixaa = value;
  }

  internal Pixa Pixat
  {
    get => this.m_pixat;
    set => this.m_pixat = value;
  }

  internal Pixa Pixatd
  {
    get => this.m_pixatd;
    set => this.m_pixatd = value;
  }

  internal NumaHash NaHash
  {
    get => this.m_naHash;
    set => this.m_naHash = value;
  }

  internal Numa Nafgt
  {
    get => this.m_nafgt;
    set => this.m_nafgt = value;
  }

  internal Pta Ptac
  {
    get => this.m_ptac;
    set => this.m_ptac = value;
  }

  internal Pta PtaTemplate
  {
    get => this.m_ptaTemplate;
    set => this.m_ptaTemplate = value;
  }

  internal Numa NaClass
  {
    get => this.m_naClass;
    set => this.m_naClass = value;
  }

  internal Numa NaPage
  {
    get => this.m_naPage;
    set => this.m_naPage = value;
  }

  internal Pta PtaUL
  {
    get => this.m_ptaUL;
    set => this.m_ptaUL = value;
  }

  internal Pta PtaLL
  {
    get => this.m_ptaLL;
    set => this.m_ptaLL = value;
  }

  internal JBIG2Classifier(
    int method,
    int components,
    Numa nacomps,
    Pixaa pixaa,
    Pixa pixat,
    Pixa pixatd,
    Numa nafgt,
    Numa naarea,
    Pta ptac,
    Pta ptact,
    Numa naclass,
    Numa napage,
    Pta ptaul)
  {
    this.Method = method;
    this.Components = components;
    this.NaComps = nacomps;
    this.Pixaa = pixaa;
    this.Pixat = pixat;
    this.Pixatd = pixatd;
    this.Nafgt = nafgt;
    this.NaArea = naarea;
    this.Ptac = ptac;
    this.PtaTemplate = ptact;
    this.NaClass = naclass;
    this.NaPage = napage;
    this.PtaUL = ptaul;
  }
}
