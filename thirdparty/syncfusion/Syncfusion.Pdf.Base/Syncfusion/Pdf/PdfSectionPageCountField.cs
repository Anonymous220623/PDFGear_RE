// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfSectionPageCountField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfSectionPageCountField : PdfMultipleNumberValueField
{
  public PdfSectionPageCountField()
  {
  }

  public PdfSectionPageCountField(PdfFont font)
    : base(font)
  {
  }

  public PdfSectionPageCountField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfSectionPageCountField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  protected internal override string GetValue(PdfGraphics graphics)
  {
    string str = (string) null;
    if (graphics.Page is PdfPage)
      str = PdfNumbersConvertor.Convert(PdfDynamicField.GetPageFromGraphics(graphics).Section.Count, this.NumberStyle);
    else if (graphics.Page is PdfLoadedPage)
    {
      PdfLoadedPage pageFromGraphics = PdfDynamicField.GetLoadedPageFromGraphics(graphics);
      PdfLoadedPage page = graphics.Page as PdfLoadedPage;
      PdfDocumentBase document = page.Document;
      PdfDictionary catalog = (PdfDictionary) page.Document.Catalog;
      PdfArray pdfArray1 = (page.CrossTable.GetObject(catalog["Pages"]) as PdfDictionary)["Kids"] as PdfArray;
      for (int index1 = 0; index1 < pdfArray1.Count; ++index1)
      {
        PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfWrapper) page);
        PdfDictionary pdfDictionary = (pdfArray1[index1] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary["Type"].ToString() == "/Pages")
        {
          PdfArray pdfArray2 = page.CrossTable.GetObject(pdfDictionary["Kids"]) as PdfArray;
          for (int index2 = 0; index2 < pdfArray2.Count; ++index2)
          {
            PdfReferenceHolder pointer = pdfArray2[index2] as PdfReferenceHolder;
            if ((pageFromGraphics.CrossTable.GetObject((IPdfPrimitive) pointer) as PdfDictionary).Equals((object) pageFromGraphics.Dictionary))
              str = PdfNumbersConvertor.Convert(pdfArray2.Count, this.NumberStyle);
          }
        }
      }
    }
    return str;
  }
}
