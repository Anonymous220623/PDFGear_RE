// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDocumentAuthorField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDocumentAuthorField : PdfSingleValueField
{
  public PdfDocumentAuthorField()
  {
  }

  public PdfDocumentAuthorField(PdfFont font)
    : base(font)
  {
  }

  public PdfDocumentAuthorField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfDocumentAuthorField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    string str = (string) null;
    if (graphics.Page is PdfPage)
      str = PdfDynamicField.GetPageFromGraphics(graphics).Document.DocumentInformation.Author;
    else if (graphics.Page is PdfLoadedPage)
      str = PdfDynamicField.GetLoadedPageFromGraphics(graphics).Document.DocumentInformation.Author;
    return str;
  }
}
