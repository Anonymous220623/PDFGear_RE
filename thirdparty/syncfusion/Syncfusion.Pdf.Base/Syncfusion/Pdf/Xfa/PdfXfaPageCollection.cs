// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaPageCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaPageCollection
{
  internal List<PdfXfaPage> m_pages = new List<PdfXfaPage>();
  internal PdfXfaDocument m_parent;

  public PdfXfaPage this[int index] => this.m_pages[index];

  public PdfXfaPage Add()
  {
    PdfXfaPage page = new PdfXfaPage();
    page.pageId = this.m_parent.Pages.m_pages.Count + 1;
    this.SetPageSettings(page);
    this.m_pages.Add(page);
    return page;
  }

  private void SetPageSettings(PdfXfaPage page)
  {
    if (this.m_parent == null)
      return;
    page.pageSettings.Margins = (PdfMargins) this.m_parent.PageSettings.Margins.Clone();
    page.pageSettings.PageOrientation = this.m_parent.PageSettings.PageOrientation;
    page.pageSettings.PageSize = this.m_parent.PageSettings.PageSize;
  }

  internal object Clone() => this.MemberwiseClone();
}
