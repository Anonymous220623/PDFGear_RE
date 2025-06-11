// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfPopupAnnotationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfPopupAnnotationCollection : PdfCollection
{
  internal PdfDictionary annotDictionary;
  private bool isReview;
  internal PdfPageBase page;
  private int ReviewFlag = 30;
  private int CommentFlag = 28;

  internal PdfPopupAnnotationCollection(PdfAnnotation pdfAnnotation, bool isReview)
  {
    this.annotDictionary = pdfAnnotation.Dictionary;
    this.isReview = isReview;
    if (pdfAnnotation.Page != null)
    {
      this.page = (PdfPageBase) pdfAnnotation.Page;
    }
    else
    {
      if (pdfAnnotation.LoadedPage == null)
        return;
      this.page = (PdfPageBase) pdfAnnotation.LoadedPage;
    }
  }

  public PdfPopupAnnotation this[int index]
  {
    get
    {
      if (index < 0 || this.List.Count <= 0 || index >= this.List.Count)
        throw new IndexOutOfRangeException(nameof (index));
      return this.List[index] as PdfPopupAnnotation;
    }
  }

  public void Add(PdfPopupAnnotation popupAnnotation)
  {
    if (this.IsReviewAnnot())
      throw new PdfException("Could not add comments/reviews to the review");
    if (this.List.Count <= 0 || !this.isReview)
      popupAnnotation.Dictionary.SetProperty("IRT", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.annotDictionary));
    else
      popupAnnotation.Dictionary.SetProperty("IRT", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) (this.List[this.List.Count - 1] as PdfAnnotation)));
    if (popupAnnotation.AnnotationFlags == PdfAnnotationFlags.Locked)
    {
      if (this.isReview)
        this.ReviewFlag = 128 /*0x80*/;
      else
        this.CommentFlag = 128 /*0x80*/;
    }
    popupAnnotation.Dictionary.SetProperty("F", this.isReview ? (IPdfPrimitive) new PdfNumber(this.ReviewFlag) : (IPdfPrimitive) new PdfNumber(this.CommentFlag));
    this.CommentFlag = 28;
    this.ReviewFlag = 30;
    if (this.isReview)
    {
      popupAnnotation.Dictionary.SetProperty("State", (IPdfPrimitive) new PdfString(popupAnnotation.State.ToString()));
      popupAnnotation.Dictionary.SetProperty("StateModel", (IPdfPrimitive) new PdfString(popupAnnotation.StateModel.ToString()));
    }
    else
      popupAnnotation.Dictionary.SetDateTime("CreationDate", DateTime.Now);
    this.List.Add((object) popupAnnotation);
    this.AddInnerCommentOrReview(this.page, popupAnnotation);
  }

  private void DoAddPage(PdfDictionary annotDictionary, PdfPopupAnnotation annotation)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (annotDictionary.ContainsKey("P"))
      pdfDictionary = PdfCrossTable.Dereference(annotDictionary["P"]) as PdfDictionary;
    else if (this.page != null)
      pdfDictionary = this.page.Dictionary;
    if (pdfDictionary == null)
      return;
    if (!(PdfCrossTable.Dereference(pdfDictionary["Annots"]) is PdfArray primitive))
    {
      primitive = new PdfArray();
      pdfDictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
    annotation.Dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    annotation.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(annotation.Bounds));
    primitive.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annotation));
  }

  private void AddInnerCommentOrReview(PdfPageBase page, PdfPopupAnnotation popupAnnotation)
  {
    if (page != null)
    {
      this.DoAddComments((PdfAnnotation) popupAnnotation);
      this.DoAddReviewHistory((PdfAnnotation) popupAnnotation);
    }
    this.DoAddPage(this.annotDictionary, popupAnnotation);
  }

  private PdfPopupAnnotationCollection GetCommentsOrReview(PdfAnnotation annotation, bool isReview)
  {
    switch (annotation)
    {
      case PdfPopupAnnotation _:
        PdfPopupAnnotation pdfPopupAnnotation = annotation as PdfPopupAnnotation;
        return isReview ? pdfPopupAnnotation.ReviewHistory : pdfPopupAnnotation.Comments;
      case PdfRectangleAnnotation _:
        PdfRectangleAnnotation rectangleAnnotation = annotation as PdfRectangleAnnotation;
        return isReview ? rectangleAnnotation.ReviewHistory : rectangleAnnotation.Comments;
      case PdfCircleAnnotation _:
        PdfCircleAnnotation circleAnnotation = annotation as PdfCircleAnnotation;
        return isReview ? circleAnnotation.ReviewHistory : circleAnnotation.Comments;
      case PdfLineAnnotation _:
        PdfLineAnnotation pdfLineAnnotation = annotation as PdfLineAnnotation;
        return isReview ? pdfLineAnnotation.ReviewHistory : pdfLineAnnotation.Comments;
      case PdfSquareAnnotation _:
        PdfSquareAnnotation squareAnnotation = annotation as PdfSquareAnnotation;
        return isReview ? squareAnnotation.ReviewHistory : squareAnnotation.Comments;
      case PdfEllipseAnnotation _:
        PdfEllipseAnnotation ellipseAnnotation = annotation as PdfEllipseAnnotation;
        return isReview ? ellipseAnnotation.ReviewHistory : ellipseAnnotation.Comments;
      case PdfFreeTextAnnotation _:
        PdfFreeTextAnnotation freeTextAnnotation = annotation as PdfFreeTextAnnotation;
        return isReview ? freeTextAnnotation.ReviewHistory : freeTextAnnotation.Comments;
      case PdfTextMarkupAnnotation _:
        PdfTextMarkupAnnotation markupAnnotation = annotation as PdfTextMarkupAnnotation;
        return isReview ? markupAnnotation.ReviewHistory : markupAnnotation.Comments;
      case PdfAttachmentAnnotation _:
        PdfAttachmentAnnotation attachmentAnnotation = annotation as PdfAttachmentAnnotation;
        return isReview ? attachmentAnnotation.ReviewHistory : attachmentAnnotation.Comments;
      case PdfRubberStampAnnotation _:
        PdfRubberStampAnnotation rubberStampAnnotation = annotation as PdfRubberStampAnnotation;
        return isReview ? rubberStampAnnotation.ReviewHistory : rubberStampAnnotation.Comments;
      case PdfInkAnnotation _:
        PdfInkAnnotation pdfInkAnnotation = annotation as PdfInkAnnotation;
        return isReview ? pdfInkAnnotation.ReviewHistory : pdfInkAnnotation.Comments;
      case PdfSoundAnnotation _:
        PdfSoundAnnotation pdfSoundAnnotation = annotation as PdfSoundAnnotation;
        return isReview ? pdfSoundAnnotation.ReviewHistory : pdfSoundAnnotation.Comments;
      case PdfPolygonAnnotation _:
        PdfPolygonAnnotation polygonAnnotation = annotation as PdfPolygonAnnotation;
        return isReview ? polygonAnnotation.ReviewHistory : polygonAnnotation.Comments;
      case PdfPolyLineAnnotation _:
        PdfPolyLineAnnotation polyLineAnnotation = annotation as PdfPolyLineAnnotation;
        return isReview ? polyLineAnnotation.ReviewHistory : polyLineAnnotation.Comments;
      default:
        return (PdfPopupAnnotationCollection) null;
    }
  }

  private void DoAddComments(PdfAnnotation annotation)
  {
    PdfPopupAnnotationCollection commentsOrReview = this.GetCommentsOrReview(annotation, false);
    if (commentsOrReview == null)
      return;
    for (int index1 = 0; index1 < commentsOrReview.Count; ++index1)
    {
      PdfPopupAnnotation annotation1 = commentsOrReview[index1];
      if (annotation1 != null)
      {
        this.DoAddPage(this.annotDictionary, annotation1);
        if (annotation1.Comments.Count > 0)
          this.DoAddComments((PdfAnnotation) annotation1);
        if (annotation1.ReviewHistory.Count > 0)
        {
          for (int index2 = 0; index2 < annotation1.ReviewHistory.Count; ++index2)
            this.DoAddPage(this.annotDictionary, annotation1.ReviewHistory[index2]);
        }
      }
    }
  }

  private void DoAddReviewHistory(PdfAnnotation annotation)
  {
    PdfPopupAnnotationCollection commentsOrReview = this.GetCommentsOrReview(annotation, true);
    if (commentsOrReview == null)
      return;
    for (int index = 0; index < commentsOrReview.Count; ++index)
      this.DoAddPage(this.annotDictionary, commentsOrReview[index]);
  }

  public void Remove(PdfPopupAnnotation popupAnnotation)
  {
    this.RemoveAt(this.List.IndexOf((object) popupAnnotation));
  }

  public void RemoveAt(int index)
  {
    if (index < 0 && index >= this.List.Count)
      throw new PdfException("Index", (Exception) new IndexOutOfRangeException());
    PdfAnnotation annotation = this.List[index] as PdfAnnotation;
    if (this.isReview)
    {
      PdfDictionary pdfDictionary1 = (PdfDictionary) null;
      PdfDictionary pdfDictionary2 = (PdfDictionary) null;
      if (index == 0)
        pdfDictionary2 = this.annotDictionary;
      if (index > 0 && index < this.List.Count)
        pdfDictionary2 = (this.List[index - 1] as PdfAnnotation).Dictionary;
      if (index + 1 < this.List.Count)
        pdfDictionary1 = (this.List[index + 1] as PdfAnnotation).Dictionary;
      if (pdfDictionary1 != null && pdfDictionary2 != null)
        pdfDictionary1.SetProperty("IRT", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
    }
    this.DoRemovePage(this.annotDictionary, annotation);
  }

  private void DoRemovePage(PdfDictionary annotDictionary, PdfAnnotation annotation)
  {
    if (this.page != null)
    {
      if (this.page is PdfLoadedPage page1)
      {
        page1.Annotations.Remove(annotation);
        this.DoRemoveChildAnnots((PdfPageBase) page1, annotation);
      }
      else
      {
        PdfPage page = this.page as PdfPage;
        page.Annotations.Remove(annotation);
        this.DoRemoveChildAnnots((PdfPageBase) page, annotation);
      }
    }
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (annotDictionary.ContainsKey("P"))
      pdfDictionary = PdfCrossTable.Dereference(annotDictionary["P"]) as PdfDictionary;
    else if (this.page != null)
      pdfDictionary = this.page.Dictionary;
    if (pdfDictionary != null && PdfCrossTable.Dereference(pdfDictionary["Annots"]) is PdfArray pdfArray)
      pdfArray.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annotation));
    this.List.Remove((object) annotation);
  }

  private void DoRemoveChildAnnots(PdfPageBase lPage, PdfAnnotation annot)
  {
    PdfPopupAnnotationCollection commentsOrReview1 = this.GetCommentsOrReview(annot, false);
    PdfPopupAnnotationCollection commentsOrReview2 = this.GetCommentsOrReview(annot, true);
    if (commentsOrReview2 != null)
    {
      foreach (PdfAnnotation annotation in (PdfCollection) commentsOrReview2)
        this.DoRemovePage(this.annotDictionary, annotation);
    }
    if (commentsOrReview1 == null)
      return;
    foreach (PdfAnnotation annotation in (PdfCollection) commentsOrReview1)
      this.DoRemovePage(this.annotDictionary, annotation);
  }

  private bool IsReviewAnnot()
  {
    return this.annotDictionary["F"] is PdfNumber annot && annot.IntValue == this.ReviewFlag;
  }
}
