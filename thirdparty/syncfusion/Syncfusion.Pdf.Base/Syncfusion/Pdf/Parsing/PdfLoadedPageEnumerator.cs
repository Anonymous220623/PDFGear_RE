// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedPageEnumerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedPageEnumerator : IEnumerator
{
  private PdfLoadedPageCollection m_collection;
  private int m_index = -1;

  public PdfLoadedPageEnumerator(PdfLoadedPageCollection collection)
  {
    this.m_collection = collection != null ? collection : throw new ArgumentNullException(nameof (collection));
  }

  public object Current
  {
    get
    {
      if (this.m_index < 0 && this.m_index >= this.m_collection.Count)
        throw new InvalidOperationException("The index is out of range.");
      return (object) this.m_collection[this.m_index];
    }
  }

  public bool MoveNext()
  {
    ++this.m_index;
    return this.m_index < this.m_collection.Count;
  }

  public void Reset() => this.m_index = -1;
}
