// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Functions.PdfFunction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Functions;

public abstract class PdfFunction : IPdfWrapper
{
  private PdfDictionary m_dictionary;

  internal PdfFunction(PdfDictionary dic) => this.m_dictionary = dic;

  internal PdfArray Domain
  {
    get => this.m_dictionary[nameof (Domain)] as PdfArray;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Domain));
      this.m_dictionary.SetProperty(nameof (Domain), (IPdfPrimitive) value);
    }
  }

  internal PdfArray Range
  {
    get => this.m_dictionary[nameof (Range)] as PdfArray;
    set => this.m_dictionary.SetProperty(nameof (Range), (IPdfPrimitive) value);
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
