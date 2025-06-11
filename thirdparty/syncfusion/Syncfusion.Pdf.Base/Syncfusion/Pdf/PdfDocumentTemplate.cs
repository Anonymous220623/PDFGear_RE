// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDocumentTemplate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDocumentTemplate
{
  private PdfPageTemplateElement m_left;
  private PdfPageTemplateElement m_top;
  private PdfPageTemplateElement m_right;
  private PdfPageTemplateElement m_bottom;
  private PdfPageTemplateElement m_evenLeft;
  private PdfPageTemplateElement m_evenTop;
  private PdfPageTemplateElement m_evenRight;
  private PdfPageTemplateElement m_evenBottom;
  private PdfPageTemplateElement m_oddLeft;
  private PdfPageTemplateElement m_oddTop;
  private PdfPageTemplateElement m_oddRight;
  private PdfPageTemplateElement m_oddBottom;
  private PdfStampCollection m_stamps;
  internal PdfMargins blinkMargin;

  public PdfPageTemplateElement Left
  {
    get => this.m_left;
    set => this.m_left = this.CheckElement(value, TemplateType.Left);
  }

  public PdfPageTemplateElement Top
  {
    get => this.m_top;
    set => this.m_top = this.CheckElement(value, TemplateType.Top);
  }

  public PdfPageTemplateElement Right
  {
    get => this.m_right;
    set => this.m_right = this.CheckElement(value, TemplateType.Right);
  }

  public PdfPageTemplateElement Bottom
  {
    get => this.m_bottom;
    set => this.m_bottom = this.CheckElement(value, TemplateType.Bottom);
  }

  public PdfPageTemplateElement EvenLeft
  {
    get => this.m_evenLeft;
    set => this.m_evenLeft = this.CheckElement(value, TemplateType.Left);
  }

  public PdfPageTemplateElement EvenTop
  {
    get => this.m_evenTop;
    set => this.m_evenTop = this.CheckElement(value, TemplateType.Top);
  }

  public PdfPageTemplateElement EvenRight
  {
    get => this.m_evenRight;
    set => this.m_evenRight = this.CheckElement(value, TemplateType.Right);
  }

  public PdfPageTemplateElement EvenBottom
  {
    get => this.m_evenBottom;
    set => this.m_evenBottom = this.CheckElement(value, TemplateType.Bottom);
  }

  public PdfPageTemplateElement OddLeft
  {
    get => this.m_oddLeft;
    set => this.m_oddLeft = this.CheckElement(value, TemplateType.Left);
  }

  public PdfPageTemplateElement OddTop
  {
    get => this.m_oddTop;
    set => this.m_oddTop = this.CheckElement(value, TemplateType.Top);
  }

  public PdfPageTemplateElement OddRight
  {
    get => this.m_oddRight;
    set => this.m_oddRight = this.CheckElement(value, TemplateType.Right);
  }

  public PdfPageTemplateElement OddBottom
  {
    get => this.m_oddBottom;
    set => this.m_oddBottom = this.CheckElement(value, TemplateType.Bottom);
  }

  public PdfStampCollection Stamps
  {
    get
    {
      if (this.m_stamps == null)
        this.m_stamps = new PdfStampCollection();
      return this.m_stamps;
    }
  }

  internal PdfPageTemplateElement GetLeft(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfPageTemplateElement left = (PdfPageTemplateElement) null;
    if (page.Document.Pages != null && (this.EvenLeft != null || this.OddLeft != null || this.Left != null))
      left = !this.IsEven(page) ? (this.OddLeft != null ? this.OddLeft : this.Left) : (this.EvenLeft != null ? this.EvenLeft : this.Left);
    return left;
  }

  internal PdfPageTemplateElement GetTop(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfPageTemplateElement top = (PdfPageTemplateElement) null;
    if (page.Document.Pages != null && (this.EvenTop != null || this.OddTop != null || this.Top != null))
      top = !this.IsEven(page) ? (this.OddTop != null ? this.OddTop : this.Top) : (this.EvenTop != null ? this.EvenTop : this.Top);
    return top;
  }

  internal PdfPageTemplateElement GetRight(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfPageTemplateElement right = (PdfPageTemplateElement) null;
    if (page.Document.Pages != null && (this.EvenRight != null || this.OddRight != null || this.Right != null))
      right = !this.IsEven(page) ? (this.OddRight != null ? this.OddRight : this.Right) : (this.EvenRight != null ? this.EvenRight : this.Right);
    return right;
  }

  internal PdfPageTemplateElement GetBottom(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfPageTemplateElement bottom = (PdfPageTemplateElement) null;
    if (page.Document.Pages != null && (this.EvenBottom != null || this.OddBottom != null || this.Bottom != null))
      bottom = !this.IsEven(page) ? (this.OddBottom != null ? this.OddBottom : this.Bottom) : (this.EvenBottom != null ? this.EvenBottom : this.Bottom);
    return bottom;
  }

  private bool IsEven(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfDocumentPageCollection pages = page.Section.Document.Pages;
    return (!pages.PageCollectionIndex.ContainsKey(page) ? pages.IndexOf(page) + 1 : pages.PageCollectionIndex[page] + 1) % 2 == 0;
  }

  private PdfPageTemplateElement CheckElement(
    PdfPageTemplateElement templateElement,
    TemplateType type)
  {
    if (templateElement != null)
      templateElement.Type = templateElement.Type == TemplateType.None ? type : throw new NotSupportedException("Can't reassign the template element. Please, create new one.");
    return templateElement;
  }
}
