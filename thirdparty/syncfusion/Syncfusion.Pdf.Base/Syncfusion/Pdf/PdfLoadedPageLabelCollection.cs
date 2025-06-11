// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfLoadedPageLabelCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfLoadedPageLabelCollection : IPdfWrapper
{
  private int m_count;
  private List<PdfPageLabel> m_pageLabel = new List<PdfPageLabel>();
  private List<PdfReferenceHolder> m_pageLabelCollection = new List<PdfReferenceHolder>();

  public int Count => this.m_count;

  public PdfPageLabel this[int index] => this.m_pageLabel[index];

  public void Add(PdfPageLabel pageLabel)
  {
    PdfReferenceHolder pdfReferenceHolder = pageLabel != null ? new PdfReferenceHolder((IPdfWrapper) pageLabel) : throw new ArgumentNullException("section");
    this.m_pageLabel.Add(pageLabel);
    this.m_pageLabelCollection.Add(pdfReferenceHolder);
    ++this.m_count;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) null;
}
