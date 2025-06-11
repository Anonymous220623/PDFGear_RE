// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfCalRGBColorSpace
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfCalRGBColorSpace : PdfColorSpaces, IPdfWrapper
{
  private double[] m_whitePoint = new double[3]
  {
    0.9505,
    1.0,
    1.089
  };
  private double[] m_blackPoint;
  private double[] m_gama;
  private double[] m_matrix;

  public PdfCalRGBColorSpace() => this.Initialize();

  public double[] BlackPoint
  {
    get => this.m_blackPoint;
    set
    {
      this.m_blackPoint = value == null || value.Length == 3 ? value : throw new ArgumentOutOfRangeException(nameof (BlackPoint), "BlackPoint array must have 3 values.");
      this.Initialize();
    }
  }

  public double[] Gamma
  {
    get => this.m_gama;
    set
    {
      this.m_gama = value == null || value.Length == 3 ? value : throw new ArgumentOutOfRangeException(nameof (Gamma), "Gamma array must have 3 values.");
      this.Initialize();
    }
  }

  public double[] Matrix
  {
    get => this.m_matrix;
    set
    {
      this.m_matrix = value == null || value.Length == 9 ? value : throw new ArgumentOutOfRangeException(nameof (Matrix), "Matrix array must have 9 values.");
      this.Initialize();
    }
  }

  public double[] WhitePoint
  {
    get => this.m_whitePoint;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (WhitePoint), "WhitePoint array cannot be null.");
      this.m_whitePoint = value.Length == 3 ? value : throw new ArgumentOutOfRangeException(nameof (WhitePoint), "WhitePoint array must have 3 values.");
      this.Initialize();
    }
  }

  private void Initialize()
  {
    lock (PdfColorSpaces.s_syncObject)
    {
      IPdfCache pdfCache = PdfDocument.Cache.Search((IPdfCache) this);
      ((IPdfCache) this).SetInternals(pdfCache != null ? pdfCache.GetInternals() : (IPdfPrimitive) this.CreateInternals());
    }
  }

  private PdfArray CreateInternals()
  {
    PdfArray internals = new PdfArray();
    if (internals != null)
    {
      PdfName element1 = new PdfName("CalRGB");
      internals.Add((IPdfPrimitive) element1);
      PdfDictionary element2 = new PdfDictionary();
      element2.SetProperty("WhitePoint", (IPdfPrimitive) new PdfArray(this.m_whitePoint));
      if (this.m_gama != null)
        element2.SetProperty("Gamma", (IPdfPrimitive) new PdfArray(this.m_gama));
      if (this.m_blackPoint != null)
        element2.SetProperty("BlackPoint", (IPdfPrimitive) new PdfArray(this.m_blackPoint));
      if (this.m_matrix != null)
        element2.SetProperty("Matrix", (IPdfPrimitive) new PdfArray(this.m_matrix));
      internals.Add((IPdfPrimitive) element2);
    }
    return internals;
  }
}
