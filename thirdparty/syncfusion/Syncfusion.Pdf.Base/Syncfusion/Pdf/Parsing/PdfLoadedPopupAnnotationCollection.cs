// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedPopupAnnotationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedPopupAnnotationCollection : PdfCollection
{
  private const int ReviewFlag = 30;
  private const int CommentFlag = 28;
  internal PdfDictionary m_annotDictionary;
  private bool m_isReview;
  private PdfLoadedPage m_loadedPage;

  public PdfLoadedPopupAnnotation this[int index]
  {
    get
    {
      if (index < 0 || this.List.Count <= 0 || index >= this.List.Count)
        throw new IndexOutOfRangeException(nameof (index));
      return this.List[index] as PdfLoadedPopupAnnotation;
    }
  }

  internal PdfLoadedPopupAnnotationCollection(
    PdfLoadedPage page,
    PdfDictionary annotDictionary,
    bool isReview)
  {
    this.m_isReview = isReview;
    this.m_loadedPage = page;
    this.m_annotDictionary = annotDictionary;
    if (isReview)
      this.GetReviewHistory(page, annotDictionary);
    else
      this.GetComments(page, annotDictionary);
  }

  private void GetReviewHistory(PdfLoadedPage page, PdfDictionary annotDictionary)
  {
    if (this.IsReviewAnnot(annotDictionary))
      return;
    System.Collections.Generic.List<PdfReference> pdfReferenceList = new System.Collections.Generic.List<PdfReference>();
    System.Collections.Generic.List<PdfDictionary> pdfDictionaryList = new System.Collections.Generic.List<PdfDictionary>();
    pdfDictionaryList.Add(annotDictionary);
    PdfReference reference1 = page.CrossTable.GetReference((IPdfPrimitive) annotDictionary);
    pdfReferenceList.Add(reference1);
    foreach (PdfLoadedAnnotation annotation in (PdfCollection) page.Annotations)
    {
      if (annotation.Dictionary.ContainsKey("IRT") && annotation is PdfLoadedPopupAnnotation)
      {
        PdfLoadedPopupAnnotation loadedPopupAnnotation = annotation as PdfLoadedPopupAnnotation;
        if (this.IsReviewAnnot(loadedPopupAnnotation.Dictionary))
        {
          IPdfPrimitive pdfPrimitive = loadedPopupAnnotation.Dictionary["IRT"];
          if (pdfPrimitive != null)
          {
            PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null)
            {
              PdfReference reference2 = pdfReferenceHolder.Reference;
              if (pdfReferenceList != null && reference2 != (PdfReference) null && pdfReferenceList.Contains(reference2))
              {
                this.List.Add((object) loadedPopupAnnotation);
                PdfReference reference3 = page.CrossTable.GetReference((IPdfPrimitive) loadedPopupAnnotation.Dictionary);
                pdfReferenceList.Add(reference3);
              }
              else if (pdfReferenceHolder.Object is PdfDictionary pdfDictionary && pdfDictionaryList.Contains(pdfDictionary))
              {
                this.List.Add((object) loadedPopupAnnotation);
                pdfDictionaryList.Add(loadedPopupAnnotation.Dictionary);
              }
            }
          }
        }
      }
    }
    pdfReferenceList.Clear();
    pdfDictionaryList.Clear();
  }

  private void GetComments(PdfLoadedPage page, PdfDictionary annotDictionary)
  {
    if (this.IsReviewAnnot(annotDictionary))
      return;
    PdfReference reference1 = page.CrossTable.GetReference((IPdfPrimitive) annotDictionary);
    foreach (PdfLoadedAnnotation annotation in (PdfCollection) page.Annotations)
    {
      if (annotation.Dictionary.ContainsKey("IRT") && annotation is PdfLoadedPopupAnnotation)
      {
        PdfLoadedPopupAnnotation loadedPopupAnnotation = annotation as PdfLoadedPopupAnnotation;
        if (!this.IsReviewAnnot(loadedPopupAnnotation.Dictionary))
        {
          IPdfPrimitive pdfPrimitive = loadedPopupAnnotation.Dictionary["IRT"];
          if (pdfPrimitive != null)
          {
            PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null)
            {
              PdfReference reference2 = pdfReferenceHolder.Reference;
              if (reference1 != (PdfReference) null && reference2 != (PdfReference) null && reference1 == reference2)
                this.List.Add((object) loadedPopupAnnotation);
              else if (pdfReferenceHolder.Object is PdfDictionary pdfDictionary && annotDictionary == pdfDictionary)
                this.List.Add((object) loadedPopupAnnotation);
            }
          }
        }
      }
    }
  }

  private bool IsReviewAnnot(PdfDictionary annotDictionary)
  {
    return annotDictionary.ContainsKey("State") || annotDictionary.ContainsKey("StateModel");
  }

  public void Add(PdfPopupAnnotation popupAnnotation)
  {
    if (this.IsReviewAnnot())
      throw new PdfException("Could not add comments/reviews to the review");
    if (this.List.Count <= 0 || !this.m_isReview)
      popupAnnotation.Dictionary.SetProperty("IRT", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_annotDictionary));
    else
      popupAnnotation.Dictionary.SetProperty("IRT", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) (this.List[this.List.Count - 1] as PdfAnnotation)));
    popupAnnotation.Dictionary.SetProperty("F", this.m_isReview ? (IPdfPrimitive) new PdfNumber(30) : (IPdfPrimitive) new PdfNumber(28));
    if (this.m_isReview)
    {
      popupAnnotation.Dictionary.SetProperty("State", (IPdfPrimitive) new PdfString(popupAnnotation.State.ToString()));
      popupAnnotation.Dictionary.SetProperty("StateModel", (IPdfPrimitive) new PdfString(popupAnnotation.StateModel.ToString()));
    }
    else
      popupAnnotation.Dictionary.SetDateTime("CreationDate", DateTime.Now);
    this.List.Add((object) popupAnnotation);
    this.AddInnerCommentOrReview(this.m_loadedPage, popupAnnotation);
  }

  private void AddInnerCommentOrReview(PdfLoadedPage page, PdfPopupAnnotation popupAnnotation)
  {
    if (page != null)
    {
      this.DoAddComments((PdfAnnotation) popupAnnotation);
      this.DoAddReviewHistory((PdfAnnotation) popupAnnotation);
    }
    this.DoAddPage(this.m_annotDictionary, popupAnnotation);
  }

  private void DoAddPage(PdfDictionary annotDictionary, PdfPopupAnnotation annotation)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (annotDictionary.ContainsKey("P"))
      pdfDictionary = PdfCrossTable.Dereference(annotDictionary["P"]) as PdfDictionary;
    else if (this.m_loadedPage != null)
      pdfDictionary = this.m_loadedPage.Dictionary;
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

  public void Remove(PdfAnnotation popupAnnotation)
  {
    this.RemoveAt(this.List.IndexOf((object) popupAnnotation));
  }

  public void RemoveAt(int index)
  {
    if (index < 0 && index >= this.List.Count)
      throw new PdfException("Index", (Exception) new IndexOutOfRangeException());
    PdfAnnotation annotation = this.List[index] as PdfAnnotation;
    if (this.m_isReview)
    {
      PdfDictionary pdfDictionary1 = (PdfDictionary) null;
      PdfDictionary pdfDictionary2 = (PdfDictionary) null;
      if (index == 0)
        pdfDictionary2 = this.m_annotDictionary;
      if (index > 0 && index < this.List.Count)
        pdfDictionary2 = (this.List[index - 1] as PdfAnnotation).Dictionary;
      if (index + 1 < this.List.Count)
        pdfDictionary1 = (this.List[index + 1] as PdfAnnotation).Dictionary;
      if (pdfDictionary1 != null && pdfDictionary2 != null)
        pdfDictionary1.SetProperty("IRT", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
    }
    this.DoRemovePage(this.m_annotDictionary, annotation);
  }

  private void DoRemovePage(PdfDictionary annotDictionary, PdfAnnotation annotation)
  {
    if (this.m_loadedPage != null)
    {
      this.m_loadedPage.Annotations.Remove(annotation);
      this.DoRemoveChildAnnots(this.m_loadedPage, annotation);
    }
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (annotDictionary.ContainsKey("P"))
      pdfDictionary = PdfCrossTable.Dereference(annotDictionary["P"]) as PdfDictionary;
    else if (this.m_loadedPage != null)
      pdfDictionary = this.m_loadedPage.Dictionary;
    if (pdfDictionary != null && PdfCrossTable.Dereference(pdfDictionary["Annots"]) is PdfArray pdfArray)
      pdfArray.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annotation));
    this.List.Remove((object) annotation);
  }

  private void DoRemoveChildAnnots(PdfLoadedPage lPage, PdfAnnotation annot)
  {
    PdfPopupAnnotationCollection commentsOrReview1 = this.GetCommentsOrReview(annot, false);
    PdfPopupAnnotationCollection commentsOrReview2 = this.GetCommentsOrReview(annot, true);
    if (commentsOrReview2 != null)
    {
      foreach (PdfAnnotation annotation in (PdfCollection) commentsOrReview2)
        this.DoRemovePage(this.m_annotDictionary, annotation);
    }
    if (commentsOrReview1 == null)
      return;
    foreach (PdfAnnotation annotation in (PdfCollection) commentsOrReview1)
      this.DoRemovePage(this.m_annotDictionary, annotation);
  }

  private bool IsReviewAnnot()
  {
    return this.m_annotDictionary["F"] is PdfNumber annot && annot.IntValue == 30;
  }

  private void DoAddComments(PdfAnnotation annotation)
  {
    PdfPopupAnnotationCollection commentsOrReview = this.GetCommentsOrReview(annotation, false);
    if (commentsOrReview == null)
      return;
    for (int index1 = 0; index1 < commentsOrReview.Count; ++index1)
    {
      PdfPopupAnnotation annotation1 = commentsOrReview[index1];
      this.DoAddPage(this.m_annotDictionary, annotation1);
      if (annotation1 != null)
      {
        if (annotation1.Comments.Count != 0)
          this.DoAddComments((PdfAnnotation) annotation1);
        if (annotation1.ReviewHistory.Count != 0)
        {
          for (int index2 = 0; index2 < annotation1.ReviewHistory.Count; ++index2)
            this.DoAddPage(this.m_annotDictionary, annotation1.ReviewHistory[index2]);
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
      this.DoAddPage(this.m_annotDictionary, commentsOrReview[index]);
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
}
