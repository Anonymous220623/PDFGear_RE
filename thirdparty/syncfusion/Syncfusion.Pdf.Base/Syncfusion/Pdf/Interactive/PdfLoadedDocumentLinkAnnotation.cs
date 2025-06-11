// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedDocumentLinkAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedDocumentLinkAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;

  public PdfDestination Destination
  {
    get => this.ObtainDestination();
    set => this.Dictionary.SetProperty("Dest", (IPdfWrapper) value);
  }

  internal PdfLoadedDocumentLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle)
    : base(dictionary, crossTable)
  {
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
  }

  private PdfDestination ObtainDestination()
  {
    PdfDestination destination = (PdfDestination) null;
    if (this.Dictionary.ContainsKey("Dest"))
    {
      IPdfPrimitive pdfPrimitive = this.CrossTable.GetObject(this.Dictionary["Dest"]);
      PdfArray pdfArray = pdfPrimitive as PdfArray;
      PdfName name1 = pdfPrimitive as PdfName;
      PdfString name2 = pdfPrimitive as PdfString;
      if (this.CrossTable.Document is PdfLoadedDocument document)
      {
        if (name1 != (PdfName) null)
          pdfArray = document.GetNamedDestination(name1);
        else if (name2 != null)
          pdfArray = document.GetNamedDestination(name2);
      }
      PdfReferenceHolder pointer = pdfArray[0] as PdfReferenceHolder;
      if (pointer == (PdfReferenceHolder) null && pdfArray[0] is PdfNumber)
      {
        PdfPageBase page = (this.CrossTable.Document as PdfLoadedDocument).Pages[(pdfArray[0] as PdfNumber).IntValue];
        PdfName pdfName = pdfArray[1] as PdfName;
        if (pdfName != (PdfName) null)
        {
          if (pdfName.Value == "XYZ")
          {
            PdfNumber pdfNumber1 = pdfArray[2] as PdfNumber;
            PdfNumber pdfNumber2 = pdfArray[3] as PdfNumber;
            PdfNumber pdfNumber3 = pdfArray[4] as PdfNumber;
            float y = pdfNumber2 == null ? 0.0f : page.Size.Height - pdfNumber2.FloatValue;
            float x = pdfNumber1 == null ? 0.0f : pdfNumber1.FloatValue;
            destination = new PdfDestination(page, new PointF(x, y));
            if (pdfNumber3 != null)
              destination.Zoom = pdfNumber3.FloatValue;
            if (pdfNumber1 == null || pdfNumber2 == null || pdfNumber3 == null)
              destination.SetValidation(false);
          }
        }
        else if (page != null)
        {
          destination = new PdfDestination(page);
          destination.Mode = PdfDestinationMode.FitToPage;
        }
      }
      if (pointer != (PdfReferenceHolder) null)
      {
        PdfPageBase page = (this.CrossTable.Document as PdfLoadedDocument).Pages.GetPage(this.CrossTable.GetObject((IPdfPrimitive) pointer) as PdfDictionary);
        PdfName pdfName = pdfArray[1] as PdfName;
        if (pdfName != (PdfName) null)
        {
          if (pdfName.Value == "XYZ")
          {
            PdfNumber pdfNumber4 = pdfArray[2] as PdfNumber;
            PdfNumber pdfNumber5 = pdfArray[3] as PdfNumber;
            PdfNumber pdfNumber6 = pdfArray[4] as PdfNumber;
            float y = pdfNumber5 == null ? 0.0f : page.Size.Height - pdfNumber5.FloatValue;
            float x = pdfNumber4 == null ? 0.0f : pdfNumber4.FloatValue;
            destination = new PdfDestination(page, new PointF(x, y));
            if (pdfNumber6 != null)
              destination.Zoom = pdfNumber6.FloatValue;
            if (pdfNumber4 == null || pdfNumber5 == null || pdfNumber6 == null)
              destination.SetValidation(false);
          }
        }
        else if (page != null && pdfName.Value == "Fit")
        {
          destination = new PdfDestination(page);
          destination.Mode = PdfDestinationMode.FitToPage;
        }
      }
    }
    else if (this.Dictionary.ContainsKey("A") && destination == null)
    {
      IPdfPrimitive pdfPrimitive = (this.CrossTable.GetObject(this.Dictionary["A"]) as PdfDictionary)["D"];
      if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
        pdfPrimitive = (pdfPrimitive as PdfReferenceHolder).Object;
      PdfArray pdfArray = pdfPrimitive as PdfArray;
      PdfName name3 = pdfPrimitive as PdfName;
      PdfString name4 = pdfPrimitive as PdfString;
      if (this.CrossTable.Document is PdfLoadedDocument document)
      {
        if (name3 != (PdfName) null)
          pdfArray = document.GetNamedDestination(name3);
        else if (name4 != null)
          pdfArray = document.GetNamedDestination(name4);
      }
      if (pdfArray != null)
      {
        PdfReferenceHolder pdfReferenceHolder = pdfArray[0] as PdfReferenceHolder;
        PdfPageBase page = (PdfPageBase) null;
        if (pdfReferenceHolder != (PdfReferenceHolder) null && PdfCrossTable.Dereference((IPdfPrimitive) pdfReferenceHolder) is PdfDictionary dic)
          page = (this.CrossTable.Document as PdfLoadedDocument).Pages.GetPage(dic);
        if (page != null)
        {
          PdfName pdfName = pdfArray[1] as PdfName;
          if (pdfName.Value == "FitBH" || pdfName.Value == "FitH")
          {
            float y = !(pdfArray[2] is PdfNumber pdfNumber) ? 0.0f : page.Size.Height - pdfNumber.FloatValue;
            destination = new PdfDestination(page, new PointF(0.0f, y));
            if (pdfNumber == null)
              destination.SetValidation(false);
          }
          else if (pdfName.Value == "XYZ")
          {
            PdfNumber pdfNumber7 = pdfArray[2] as PdfNumber;
            PdfNumber pdfNumber8 = pdfArray[3] as PdfNumber;
            PdfNumber pdfNumber9 = pdfArray[4] as PdfNumber;
            if (page != null)
            {
              float y = pdfNumber8 == null ? 0.0f : page.Size.Height - pdfNumber8.FloatValue;
              float x = pdfNumber7 == null ? 0.0f : pdfNumber7.FloatValue;
              destination = new PdfDestination(page, new PointF(x, y));
              if (pdfNumber9 != null)
                destination.Zoom = pdfNumber9.FloatValue;
              if (pdfNumber7 == null || pdfNumber8 == null || pdfNumber9 == null)
                destination.SetValidation(false);
            }
          }
          else if (pdfName.Value == "FitR")
          {
            if (pdfArray.Count == 6)
            {
              PdfNumber pdfNumber10 = pdfArray[2] as PdfNumber;
              PdfNumber pdfNumber11 = pdfArray[3] as PdfNumber;
              PdfNumber pdfNumber12 = pdfArray[4] as PdfNumber;
              PdfNumber pdfNumber13 = pdfArray[5] as PdfNumber;
              destination = new PdfDestination(page, new RectangleF(pdfNumber10.FloatValue, pdfNumber11.FloatValue, pdfNumber12.FloatValue, pdfNumber13.FloatValue));
            }
          }
          else if (page != null && pdfName.Value == "Fit")
          {
            destination = new PdfDestination(page);
            destination.Mode = PdfDestinationMode.FitToPage;
          }
        }
      }
    }
    return destination;
  }
}
