// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageNumberField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageNumberField : PdfMultipleNumberValueField
{
  public PdfPageNumberField()
  {
  }

  public PdfPageNumberField(PdfFont font)
    : base(font)
  {
  }

  public PdfPageNumberField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfPageNumberField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    string str = (string) null;
    if (graphics.Page is PdfPage)
    {
      PdfPage pageFromGraphics = PdfDynamicField.GetPageFromGraphics(graphics);
      if (pageFromGraphics.Section.m_document is PdfLoadedDocument)
      {
        PdfLoadedDocument document = pageFromGraphics.Section.m_document as PdfLoadedDocument;
        int count = (pageFromGraphics.Section.m_document as PdfLoadedDocument).Pages.Count;
        for (int index = 0; index < count; ++index)
        {
          if (document.Pages[index] is PdfPage && (document.Pages[index] as PdfPage).Dictionary.Equals((object) graphics.Page.Dictionary))
            str = (index + 1).ToString();
        }
      }
      else
        str = this.InternalGetValue(pageFromGraphics);
    }
    else if (graphics.Page is PdfLoadedPage)
      str = this.InternalLoadedGetValue(PdfDynamicField.GetLoadedPageFromGraphics(graphics));
    return str;
  }

  protected string InternalGetValue(PdfPage page)
  {
    return PdfNumbersConvertor.Convert(page.Section.Parent.Document.Pages.IndexOf(page) + 1, this.NumberStyle);
  }

  protected string InternalLoadedGetValue(PdfLoadedPage page)
  {
    return PdfNumbersConvertor.Convert((page.Document as PdfLoadedDocument).Pages.IndexOf((PdfPageBase) page) + 1, this.NumberStyle);
  }
}
