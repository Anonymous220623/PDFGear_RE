// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedNamedDestination
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedNamedDestination : PdfNamedDestination
{
  internal PdfLoadedNamedDestination(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
  }

  public override PdfDestination Destination
  {
    get => this.ObtainDestination();
    set => base.Destination = value;
  }

  public override string Title
  {
    get
    {
      string empty = string.Empty;
      if (this.Dictionary.ContainsKey(nameof (Title)))
        empty = (this.CrossTable.GetObject(this.Dictionary[nameof (Title)]) as PdfString).Value;
      return empty;
    }
    set => base.Title = value;
  }

  private PdfDestination ObtainDestination()
  {
    if (this.Dictionary.ContainsKey("D") && base.Destination == null && this.CrossTable.GetObject(this.Dictionary["D"]) is PdfArray pdfArray && pdfArray.Count > 1)
    {
      PdfReferenceHolder pointer = pdfArray[0] as PdfReferenceHolder;
      PdfPageBase page = (PdfPageBase) null;
      if (pointer != (PdfReferenceHolder) null && this.CrossTable.GetObject((IPdfPrimitive) pointer) is PdfDictionary dic)
        page = (this.CrossTable.Document as PdfLoadedDocument).Pages.GetPage(dic);
      PdfName pdfName = pdfArray[1] as PdfName;
      if (pdfName != (PdfName) null)
      {
        if ((pdfName.Value == "FitBH" || pdfName.Value == "FitH") && pdfArray.Count > 2)
        {
          PdfNumber pdfNumber = pdfArray[2] as PdfNumber;
          if (page != null)
          {
            float y = pdfNumber == null ? 0.0f : page.Size.Height - pdfNumber.FloatValue;
            base.Destination = new PdfDestination(page, new PointF(0.0f, y));
            base.Destination.Mode = PdfDestinationMode.FitH;
            if (pdfNumber == null)
              base.Destination.SetValidation(false);
          }
        }
        else if (pdfName.Value == "XYZ" && pdfArray.Count > 3)
        {
          PdfNumber pdfNumber1 = pdfArray[2] as PdfNumber;
          PdfNumber pdfNumber2 = pdfArray[3] as PdfNumber;
          PdfNumber pdfNumber3 = (PdfNumber) null;
          if (pdfArray.Count > 4)
            pdfNumber3 = pdfArray[4] as PdfNumber;
          if (page != null)
          {
            float y = pdfNumber2 == null ? 0.0f : page.Size.Height - pdfNumber2.FloatValue;
            float x = pdfNumber1 == null ? 0.0f : pdfNumber1.FloatValue;
            base.Destination = new PdfDestination(page, new PointF(x, y));
            if (pdfNumber3 != null)
              base.Destination.Zoom = pdfNumber3.FloatValue;
            if (pdfNumber1 == null || pdfNumber2 == null || pdfNumber3 == null)
              base.Destination.SetValidation(false);
          }
        }
        else if (page != null && pdfName.Value == "Fit")
        {
          base.Destination = new PdfDestination(page);
          base.Destination.Mode = PdfDestinationMode.FitToPage;
        }
      }
    }
    return base.Destination;
  }
}
