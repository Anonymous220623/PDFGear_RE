// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAnnotationBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

internal class PdfAnnotationBase : PdfAnnotation
{
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfCrossTable m_crossTable = new PdfCrossTable();
  private System.Collections.Generic.List<PdfAnnotation> m_list = new System.Collections.Generic.List<PdfAnnotation>();

  public PdfAnnotation this[int index] => this.List[index];

  internal virtual System.Collections.Generic.List<PdfAnnotation> List => this.m_list;

  internal new PdfDictionary Dictionary => this.m_dictionary;

  internal PdfCrossTable CrossTable => this.m_crossTable;

  internal PdfAnnotationBase()
  {
  }

  internal PdfAnnotationBase(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    this.m_dictionary = dictionary;
    if (crossTable == null)
      return;
    this.m_crossTable = crossTable;
  }

  public PdfAnnotation Add(string title)
  {
    if (title == null)
      throw new ArgumentNullException(nameof (title));
    PdfAnnotation pdfAnnotation = (PdfAnnotation) null;
    pdfAnnotation.Text = title;
    this.List.Add(pdfAnnotation);
    this.UpdateFields();
    return pdfAnnotation;
  }

  private void UpdateFields()
  {
    if (this.List.Count > 0)
      this.m_dictionary.SetNumber("Count", this.List.Count);
    else
      this.m_dictionary.Clear();
    this.m_dictionary.Modify();
  }
}
