// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfCreationDateField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfCreationDateField : PdfSingleValueField
{
  private string m_formatString = "dd'/'MM'/'yyyy";

  public PdfCreationDateField()
  {
  }

  public PdfCreationDateField(PdfFont font)
    : base(font)
  {
  }

  public PdfCreationDateField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfCreationDateField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  public string DateFormatString
  {
    get => this.m_formatString;
    set => this.m_formatString = value;
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    string str = (string) null;
    if (graphics.Page is PdfPage)
    {
      PdfPage pageFromGraphics = PdfDynamicField.GetPageFromGraphics(graphics);
      str = !(pageFromGraphics.Section.m_document is PdfLoadedDocument) ? pageFromGraphics.Document.DocumentInformation.CreationDate.ToString(this.m_formatString) : (pageFromGraphics.Section.m_document as PdfLoadedDocument).DocumentInformation.CreationDate.ToString(this.m_formatString);
    }
    else if (graphics.Page is PdfLoadedPage)
      str = (PdfDynamicField.GetLoadedPageFromGraphics(graphics).Document as PdfLoadedDocument).DocumentInformation.CreationDate.ToString(this.m_formatString);
    return str;
  }
}
