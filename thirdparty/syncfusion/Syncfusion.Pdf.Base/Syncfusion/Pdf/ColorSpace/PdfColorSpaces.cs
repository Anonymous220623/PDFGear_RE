// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfColorSpaces
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public abstract class PdfColorSpaces : IPdfWrapper, IPdfCache
{
  internal PdfResources resources;
  protected static object s_syncObject = new object();
  private IPdfPrimitive m_colorInternals;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfArray colorspace = new PdfArray();

  IPdfPrimitive IPdfWrapper.Element => this.m_colorInternals;

  bool IPdfCache.EqualsTo(IPdfCache obj) => false;

  IPdfPrimitive IPdfCache.GetInternals() => this.m_colorInternals;

  void IPdfCache.SetInternals(IPdfPrimitive internals)
  {
    this.m_colorInternals = internals != null ? internals : throw new ArgumentNullException(nameof (internals));
  }
}
