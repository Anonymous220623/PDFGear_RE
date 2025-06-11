// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAnnotationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfAnnotationCollection : PdfCollection, IPdfWrapper
{
  private string AlreadyExistsAnnotationError = "This annotatation had been already added to page";
  private string MissingAnnotationException = "Annotation is not contained in collection.";
  private PdfPage m_page;
  private PdfArray m_annotations = new PdfArray();
  private Dictionary<PdfDictionary, PdfAnnotation> m_popupCollection = new Dictionary<PdfDictionary, PdfAnnotation>();
  internal bool m_savePopup;

  public virtual PdfAnnotation this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count - 1 ? (PdfAnnotation) this.List[index] : throw new ArgumentOutOfRangeException(nameof (index), "Index is out of range.");
    }
  }

  internal PdfArray Annotations
  {
    get => this.m_annotations;
    set => this.m_annotations = value;
  }

  public PdfAnnotationCollection()
  {
  }

  public PdfAnnotationCollection(PdfPage page)
  {
    this.m_page = page != null ? page : throw new ArgumentNullException(nameof (page));
  }

  public virtual int Add(PdfAnnotation annotation)
  {
    if (annotation == null)
      throw new ArgumentNullException(nameof (annotation));
    this.SetPrint(annotation);
    return this.DoAdd(annotation);
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
    commentsOrReview.page = (PdfPageBase) this.m_page;
    for (int index1 = 0; index1 < commentsOrReview.Count; ++index1)
    {
      PdfPopupAnnotation pdfPopupAnnotation = commentsOrReview[index1];
      if (pdfPopupAnnotation != null)
      {
        this.DoAddState((PdfAnnotation) pdfPopupAnnotation);
        if (pdfPopupAnnotation.Comments.Count > 0)
          this.DoAddComments((PdfAnnotation) pdfPopupAnnotation);
        if (pdfPopupAnnotation.ReviewHistory.Count > 0)
        {
          for (int index2 = 0; index2 < pdfPopupAnnotation.ReviewHistory.Count; ++index2)
            this.DoAddState((PdfAnnotation) pdfPopupAnnotation.ReviewHistory[index2]);
        }
      }
    }
  }

  private void DoAddReviewHistory(PdfAnnotation annotation)
  {
    PdfPopupAnnotationCollection commentsOrReview = this.GetCommentsOrReview(annotation, true);
    if (commentsOrReview == null)
      return;
    commentsOrReview.page = (PdfPageBase) this.m_page;
    for (int index = 0; index < commentsOrReview.Count; ++index)
      this.DoAddState((PdfAnnotation) commentsOrReview[index]);
  }

  public void Clear()
  {
    this.DoClear();
    for (int index = this.Count - 1; index >= 0; --index)
      this.RemoveAt(index);
    this.List.Clear();
  }

  public bool Contains(PdfAnnotation annotation)
  {
    return annotation != null ? this.List.Contains((object) annotation) : throw new ArgumentNullException(nameof (annotation));
  }

  public int IndexOf(PdfAnnotation annotation)
  {
    return annotation != null ? this.List.IndexOf((object) annotation) : throw new ArgumentNullException(nameof (annotation));
  }

  public void Insert(int index, PdfAnnotation annotation) => this.DoInsert(index, annotation);

  public void RemoveAt(int index)
  {
    if (index < 0 || index > this.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (index), "Index is out of range.");
    this.RemoveAnnotationAt(index);
  }

  public void Remove(PdfAnnotation annot)
  {
    if (annot == null)
      throw new ArgumentNullException("annotation");
    this.DoRemove(annot);
  }

  public void SetPrint(PdfAnnotation annot)
  {
    if (this.m_page.Document == null || this.m_page.Document.Conformance != PdfConformanceLevel.Pdf_A1B && this.m_page.Document.Conformance != PdfConformanceLevel.Pdf_A1A && this.m_page.Document.Conformance != PdfConformanceLevel.Pdf_A2B && this.m_page.Document.Conformance != PdfConformanceLevel.Pdf_A3B)
      return;
    annot.Dictionary.SetNumber("F", 4);
  }

  private int AddAnnotation(PdfAnnotation annotation)
  {
    annotation.SetPage((PdfPageBase) this.m_page);
    this.List.Add((object) annotation);
    int num = this.List.Count - 1;
    this.m_annotations.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annotation));
    return num;
  }

  private void InsertAnnotation(int index, PdfAnnotation annotation)
  {
    annotation.SetPage((PdfPageBase) this.m_page);
    this.List.Insert(index, (object) annotation);
    this.m_annotations.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annotation));
  }

  private void RemoveAnnotation(PdfAnnotation annotation)
  {
    int index = this.List.IndexOf((object) annotation);
    annotation.SetPage((PdfPageBase) null);
    this.List.Remove((object) annotation);
    this.m_annotations.RemoveAt(index);
  }

  private void RemoveAnnotationAt(int index) => this.DoRemoveAt(index);

  protected virtual int DoAdd(PdfAnnotation annot)
  {
    annot.SetPage((PdfPageBase) this.m_page);
    if (this.m_page != null && annot is PdfTextMarkupAnnotation)
      (annot as PdfTextMarkupAnnotation).SetQuadPoints(this.m_page.Size);
    if (annot is PdfRedactionAnnotation)
    {
      PdfPageBase page = this.GetPage(annot);
      PdfRedactionAnnotation redactionAnnotation = annot as PdfRedactionAnnotation;
      if (redactionAnnotation.Flatten)
      {
        if (!(page is PdfLoadedPage))
          throw new PdfException("Redaction annotation cannot be flatten while creating");
        redactionAnnotation.ApplyRedaction(page as PdfLoadedPage);
      }
    }
    this.m_annotations.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annot));
    this.List.Add((object) annot);
    if (annot != null && this.m_page != null)
    {
      this.DoAddReviewHistory(annot);
      this.DoAddComments(annot);
    }
    this.ParsingPopUpAnnotation(annot);
    PdfStructTreeRoot structTreeRoot = PdfCatalog.StructTreeRoot;
    if (structTreeRoot != null && !(annot is WidgetAnnotation) && annot.Page != null)
    {
      if (annot.PdfTag != null && annot.PdfTag is PdfStructureElement)
        structTreeRoot.Add(annot.PdfTag as PdfStructureElement, (PdfPageBase) annot.Page, annot.Dictionary);
      else if (annot.Dictionary.ContainsKey("Subtype") && (annot.Dictionary["Subtype"] as PdfName).Value == "Link")
        structTreeRoot.Add(new PdfStructureElement(PdfTagType.Link), (PdfPageBase) annot.Page, annot.Dictionary);
      else
        structTreeRoot.Add(new PdfStructureElement(PdfTagType.Annotation), (PdfPageBase) annot.Page, annot.Dictionary);
    }
    return this.List.Count - 1;
  }

  private PdfPageBase GetPage(PdfAnnotation annotation)
  {
    PdfPageBase page = (PdfPageBase) this.m_page;
    if (page == null && annotation.Page != null)
      page = (PdfPageBase) annotation.Page;
    else if (page == null && annotation.LoadedPage != null)
      page = (PdfPageBase) annotation.LoadedPage;
    return page;
  }

  private void DoAddState(PdfAnnotation popupAnnoataion)
  {
    PdfPageBase page = this.GetPage(popupAnnoataion);
    if (page != null)
      popupAnnoataion.Dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) page));
    popupAnnoataion.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(popupAnnoataion.Bounds));
    this.m_annotations.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) popupAnnoataion));
  }

  protected virtual void DoInsert(int index, PdfAnnotation annot)
  {
    this.m_annotations.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annot));
    this.List.Insert(index, (object) annot);
  }

  protected new virtual void DoClear()
  {
    this.m_annotations.Clear();
    this.List.Clear();
  }

  protected virtual void DoRemoveAt(int index)
  {
    this.m_annotations.RemoveAt(index);
    this.List.RemoveAt(index);
  }

  protected virtual void DoRemove(PdfAnnotation annot)
  {
    int index = this.List.IndexOf((object) annot);
    this.m_annotations.RemoveAt(index);
    this.List.RemoveAt(index);
  }

  private void ParsingPopUpAnnotation(PdfAnnotation annot)
  {
    if (!annot.Dictionary.ContainsKey("Popup"))
    {
      switch (annot)
      {
        case PdfLoadedPopupAnnotation _:
        case PdfPopupAnnotation _:
          break;
        default:
          return;
      }
    }
    bool flag1 = false;
    bool flag2 = false;
    if (annot.Dictionary.ContainsKey("Subtype"))
    {
      PdfName pdfName = annot.Dictionary["Subtype"] as PdfName;
      if (pdfName != (PdfName) null)
      {
        if (pdfName.Value == "Popup")
          flag1 = true;
        if (pdfName.Value == "FreeText" || pdfName.Value == "Sound" || pdfName.Value == "FileAttachment")
          flag2 = true;
      }
    }
    if (annot is PdfLoadedPopupAnnotation && annot.Popup == null && flag1 && !flag2 && annot.Dictionary.ContainsKey("Parent") && PdfCrossTable.Dereference(annot.Dictionary["Parent"]) is PdfDictionary key && this.m_popupCollection.ContainsKey(key))
    {
      PdfAnnotation popup = this.m_popupCollection[key];
      if (popup != null && popup is PdfLoadedAnnotation)
      {
        (popup as PdfLoadedAnnotation).Popup = (PdfAnnotation) (annot as PdfLoadedPopupAnnotation);
        this.m_popupCollection.Remove(key);
      }
    }
    if (annot.Popup == null && !flag1 && !flag2 && !this.m_popupCollection.ContainsKey(annot.Dictionary))
      this.m_popupCollection.Add(annot.Dictionary, annot);
    if (!flag1 || this.m_savePopup)
      return;
    this.m_annotations.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annot));
    this.List.Remove((object) annot);
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_annotations;
}
