// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfSectionNumberField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfSectionNumberField : PdfMultipleNumberValueField
{
  public PdfSectionNumberField()
  {
  }

  public PdfSectionNumberField(PdfFont font)
    : base(font)
  {
  }

  public PdfSectionNumberField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfSectionNumberField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    string str = (string) null;
    if (graphics.Page is PdfPage)
    {
      PdfPage page = graphics.Page as PdfPage;
      if (page.Section.m_document is PdfLoadedDocument)
      {
        PdfReferenceHolder pointer = page.Dictionary["Parent"] as PdfReferenceHolder;
        PdfDictionary pdfDictionary = page.Section.m_document.CrossTable.GetObject((IPdfPrimitive) pointer) as PdfDictionary;
        PdfLoadedDocument document = page.Section.m_document as PdfLoadedDocument;
        PdfDictionary catalog = (PdfDictionary) document.Catalog;
        PdfArray pdfArray = (document.CrossTable.GetObject(catalog["Pages"]) as PdfDictionary)["Kids"] as PdfArray;
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          if (((pdfArray[index] as PdfReferenceHolder).Object as PdfDictionary).Equals((object) pdfDictionary))
            str = PdfNumbersConvertor.Convert(index + 1, this.NumberStyle);
        }
      }
      else
        str = PdfNumbersConvertor.Convert(page.Document.Sections.IndexOf(page.Section) + 1, this.NumberStyle);
    }
    else if (graphics.Page is PdfLoadedPage)
    {
      PdfLoadedPage page = graphics.Page as PdfLoadedPage;
      PdfDocumentBase document = page.Document;
      PdfDictionary catalog = (PdfDictionary) page.Document.Catalog;
      PdfArray pdfArray = (page.CrossTable.GetObject(catalog["Pages"]) as PdfDictionary)["Kids"] as PdfArray;
      int objNum = (int) (page.Dictionary["Parent"] as PdfReferenceHolder).Reference.ObjNum;
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        PdfReferenceHolder pdfReferenceHolder = pdfArray[index] as PdfReferenceHolder;
        if (pdfReferenceHolder.Reference != (PdfReference) null && (int) pdfReferenceHolder.Reference.ObjNum == objNum)
          str = PdfNumbersConvertor.Convert(index + 1, this.NumberStyle);
      }
    }
    return str;
  }
}
