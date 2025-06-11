// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.ElementLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.HtmlToPdf;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal abstract class ElementLayouter
{
  private PdfLayoutElement m_element;
  protected bool m_isImagePath;

  public ElementLayouter(PdfLayoutElement element)
  {
    this.m_element = element != null ? element : throw new ArgumentNullException(nameof (element));
  }

  public PdfLayoutElement Element => this.m_element;

  internal bool IsImagePath
  {
    get => this.m_isImagePath;
    set => this.m_isImagePath = value;
  }

  public PdfLayoutResult Layout(PdfLayoutParams param)
  {
    return param != null ? this.LayoutInternal(param) : throw new ArgumentNullException(nameof (param));
  }

  protected virtual PdfLayoutResult LayoutInternal(HtmlToPdfParams param) => (PdfLayoutResult) null;

  internal PdfLayoutResult Layout(HtmlToPdfParams param)
  {
    return param != null ? this.LayoutInternal(param) : throw new ArgumentNullException(nameof (param));
  }

  public PdfLayoutResult Layout(HtmlToPdfLayoutParams param)
  {
    return param != null ? this.LayoutInternal(param) : throw new ArgumentNullException(nameof (param));
  }

  public PdfPage GetNextPage(PdfPage currentPage)
  {
    PdfSection pdfSection = currentPage != null ? currentPage.Section : throw new ArgumentNullException(nameof (currentPage));
    int num = pdfSection.IndexOf(currentPage);
    return num != pdfSection.Count - 1 ? pdfSection[num + 1] : pdfSection.Add();
  }

  protected abstract PdfLayoutResult LayoutInternal(PdfLayoutParams param);

  protected virtual PdfLayoutResult LayoutInternal(HtmlToPdfLayoutParams param)
  {
    return (PdfLayoutResult) null;
  }

  protected RectangleF GetPaginateBounds(PdfLayoutParams param)
  {
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    return param.Format.UsePaginateBounds ? param.Format.PaginateBounds : new RectangleF(param.Bounds.X, 0.0f, param.Bounds.Width, param.Bounds.Height);
  }
}
