// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfLabColorSpace
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfLabColorSpace : PdfColorSpaces, IPdfWrapper
{
  private double[] m_whitePoint = new double[3]
  {
    0.9505,
    1.0,
    1.089
  };
  private double[] m_blackPoint;
  private double[] m_range;

  public PdfLabColorSpace() => this.Initialize();

  public double[] BlackPoint
  {
    get => this.m_blackPoint;
    set
    {
      this.m_blackPoint = value == null || value.Length == 3 ? value : throw new ArgumentOutOfRangeException(nameof (BlackPoint), "BlackPoint array must have 3 values.");
      this.Initialize();
    }
  }

  public double[] Range
  {
    get => this.m_range;
    set
    {
      this.m_range = value == null || value.Length == 4 ? value : throw new ArgumentOutOfRangeException(nameof (Range), "Range array must have 4 values.");
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
      PdfName element1 = new PdfName("Lab");
      internals.Add((IPdfPrimitive) element1);
      PdfDictionary element2 = new PdfDictionary();
      element2.SetProperty("WhitePoint", (IPdfPrimitive) new PdfArray(this.m_whitePoint));
      if (this.m_blackPoint != null)
        element2.SetProperty("BlackPoint", (IPdfPrimitive) new PdfArray(this.m_blackPoint));
      if (this.m_range != null)
        element2.SetProperty("Range", (IPdfPrimitive) new PdfArray(this.m_range));
      internals.Add((IPdfPrimitive) element2);
    }
    return internals;
  }
}
