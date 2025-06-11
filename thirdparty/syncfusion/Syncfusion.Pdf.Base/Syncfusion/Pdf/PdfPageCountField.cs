// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageCountField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageCountField : PdfSingleValueField
{
  private PdfNumberStyle m_numberStyle = PdfNumberStyle.Numeric;

  public PdfPageCountField()
  {
  }

  public PdfPageCountField(PdfFont font)
    : base(font)
  {
  }

  public PdfPageCountField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfPageCountField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  public PdfNumberStyle NumberStyle
  {
    get => this.m_numberStyle;
    set => this.m_numberStyle = value;
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    string str = (string) null;
    if (graphics.Page is PdfPage)
    {
      PdfPage pageFromGraphics = PdfDynamicField.GetPageFromGraphics(graphics);
      if (pageFromGraphics.Section.m_document is PdfLoadedDocument)
      {
        PdfDocumentBase document = pageFromGraphics.Section.m_document;
        str = (pageFromGraphics.Section.m_document as PdfLoadedDocument).Pages.Count.ToString();
      }
      else
        str = PdfNumbersConvertor.Convert(pageFromGraphics.Section.Parent.Document.Pages.Count, this.NumberStyle);
    }
    else if (graphics.Page is PdfLoadedPage)
      str = PdfNumbersConvertor.Convert((PdfDynamicField.GetLoadedPageFromGraphics(graphics).Document as PdfLoadedDocument).Pages.Count, this.NumberStyle);
    return str;
  }
}
