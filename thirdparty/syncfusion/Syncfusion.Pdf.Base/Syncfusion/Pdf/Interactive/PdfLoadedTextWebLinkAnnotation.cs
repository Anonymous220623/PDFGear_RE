// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedTextWebLinkAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedTextWebLinkAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private string m_url;

  public string Url
  {
    get => this.ObtainUrl();
    set
    {
      this.m_url = value;
      if (!this.Dictionary.ContainsKey("A"))
        return;
      if (PdfCrossTable.Dereference(this.Dictionary["A"]) is PdfDictionary pdfDictionary)
        pdfDictionary.SetString("URI", this.m_url);
      this.Dictionary.Modify();
    }
  }

  private string ObtainUrl()
  {
    string empty = string.Empty;
    if (this.Dictionary.ContainsKey("A") && PdfCrossTable.Dereference(this.Dictionary["A"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("URI") && PdfCrossTable.Dereference(pdfDictionary["URI"]) is PdfString pdfString)
      empty = pdfString.Value;
    return empty;
  }

  internal PdfLoadedTextWebLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    string text)
    : base(dictionary, crossTable)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
  }
}
