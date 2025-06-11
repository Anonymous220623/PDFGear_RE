// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfPageTemplate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfPageTemplate
{
  private string m_pageTemplateName;
  private bool m_isVisible = true;
  private PdfPageBase m_pageBase;

  internal PdfPageTemplate()
  {
  }

  public PdfPageTemplate(PdfPageBase page, string name, bool isVisible)
  {
    if (page == null)
      throw new ArgumentNullException("The Page can't be null");
    if (name == null || name != null && name.Length == 0)
      throw new ArgumentNullException("PdfPageTemplate name can't be null/empty");
    this.m_pageBase = page;
    this.m_pageTemplateName = name;
    this.m_isVisible = isVisible;
  }

  public PdfPageTemplate(PdfPageBase page)
  {
    this.m_pageBase = page != null ? page : throw new ArgumentNullException("The Page can't be null");
  }

  public string Name
  {
    get => this.m_pageTemplateName;
    set
    {
      this.m_pageTemplateName = value != null || value == null || value.Length != 0 ? value : throw new ArgumentNullException("The PageTemplate name can't be null/empty");
    }
  }

  public bool IsVisible
  {
    get => this.m_isVisible;
    set => this.m_isVisible = value;
  }

  internal PdfPageBase PdfPageBase
  {
    get => this.m_pageBase;
    set => this.m_pageBase = value;
  }
}
