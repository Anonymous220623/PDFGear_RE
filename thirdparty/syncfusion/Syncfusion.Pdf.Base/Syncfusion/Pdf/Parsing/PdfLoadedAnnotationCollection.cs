// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedAnnotationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedAnnotationCollection : PdfAnnotationCollection
{
  private PdfLoadedPage m_page;
  private bool m_flatten;

  public override PdfAnnotation this[int index]
  {
    get
    {
      int count = this.List.Count;
      PdfAnnotation pdfAnnotation = count >= 0 && index < count ? this.List[index] as PdfAnnotation : throw new IndexOutOfRangeException(nameof (index));
      if (pdfAnnotation is PdfLoadedAnnotation)
        (pdfAnnotation as PdfLoadedAnnotation).Page = this.Page;
      else
        pdfAnnotation?.SetPage((PdfPageBase) this.Page);
      return pdfAnnotation;
    }
  }

  public PdfAnnotation this[string text]
  {
    get
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      int index = !(text == string.Empty) ? this.GetAnnotationIndex(text) : throw new ArgumentException("Annotation text can't be empty");
      return index != -1 ? this[index] : throw new ArgumentException("Incorrect field name");
    }
  }

  public PdfLoadedPage Page
  {
    get => this.m_page;
    set => this.m_page = value;
  }

  public bool Flatten
  {
    get => this.m_flatten;
    set
    {
      this.m_flatten = value;
      PdfLoadedDocument document = this.m_page.Document as PdfLoadedDocument;
      if (this.m_flatten && document != null && document.Form == null)
      {
        PdfCrossTable crossTable = this.m_page.CrossTable;
        if (this.Page.Dictionary.ContainsKey("Annots") && crossTable.GetObject(this.Page.Dictionary["Annots"]) is PdfArray pdfArray)
        {
          for (int index = 0; index < pdfArray.Count; ++index)
          {
            if (crossTable.GetObject(pdfArray[index]) is PdfDictionary pdfDictionary)
            {
              if (pdfDictionary.ContainsKey("FT"))
                pdfDictionary.Remove("FT");
              if (pdfDictionary.ContainsKey("V"))
                pdfDictionary.Remove("V");
            }
          }
        }
      }
      if (!this.m_flatten)
        return;
      int index1 = 0;
      for (int count = this.m_page.TerminalAnnotation.Count; index1 < count; ++index1)
      {
        PdfAnnotation annotation = this.GetAnnotation(index1);
        if (annotation is PdfLoadedRedactionAnnotation)
          (annotation as PdfLoadedRedactionAnnotation).Flatten = true;
      }
    }
  }

  internal PdfLoadedAnnotationCollection(PdfLoadedPage page)
  {
    this.m_page = page != null ? page : throw new ArgumentException(nameof (page));
    int index = 0;
    for (int count = this.m_page.TerminalAnnotation.Count; index < count; ++index)
    {
      PdfAnnotation annotation = this.GetAnnotation(index);
      if (annotation != null)
        this.DoAdd(annotation);
    }
    this.Page = this.m_page;
  }

  private void ldAnnotation_NameChanded(string name)
  {
    if (!this.IsValidName(name))
      throw new ArgumentException("Annotation with the same name already exist");
  }

  public override int Add(PdfAnnotation annotation)
  {
    if (annotation == null)
      throw new ArgumentNullException(nameof (annotation));
    if (annotation is PdfTextMarkupAnnotation)
      (annotation as PdfTextMarkupAnnotation).SetQuadPoints(this.m_page.Size);
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
    commentsOrReview.page = (PdfPageBase) this.Page;
    for (int index1 = 0; index1 < commentsOrReview.Count; ++index1)
    {
      PdfPopupAnnotation pdfPopupAnnotation = commentsOrReview[index1];
      this.DoAddState((PdfAnnotation) pdfPopupAnnotation);
      if (pdfPopupAnnotation != null)
      {
        if (pdfPopupAnnotation.Comments.Count != 0)
          this.DoAddComments((PdfAnnotation) pdfPopupAnnotation);
        if (pdfPopupAnnotation.ReviewHistory.Count != 0)
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
    commentsOrReview.page = (PdfPageBase) this.Page;
    for (int index = 0; index < commentsOrReview.Count; ++index)
      this.DoAddState((PdfAnnotation) commentsOrReview[index]);
  }

  private void DoAddState(PdfAnnotation popupAnnoataion)
  {
    if (popupAnnoataion == null)
      return;
    popupAnnoataion.SetPage((PdfPageBase) this.m_page);
    PdfArray primitive = (PdfArray) null;
    if (this.m_page.Dictionary.ContainsKey("Annots"))
      primitive = PdfCrossTable.Dereference(this.m_page.Dictionary["Annots"]) as PdfArray;
    if (primitive == null)
      primitive = new PdfArray();
    PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) popupAnnoataion);
    if (primitive.Contains((IPdfPrimitive) element))
      return;
    primitive.Add((IPdfPrimitive) element);
    this.m_page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
  }

  protected override int DoAdd(PdfAnnotation annot)
  {
    if (annot == null)
      throw new ArgumentNullException("annotation");
    annot.SetPage((PdfPageBase) this.m_page);
    PdfArray primitive = (PdfArray) null;
    if (this.m_page.Dictionary.ContainsKey("Annots"))
      primitive = PdfCrossTable.Dereference(this.m_page.Dictionary["Annots"]) as PdfArray;
    if (primitive == null)
      primitive = new PdfArray();
    PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) annot);
    bool flag = false;
    if (annot.Dictionary != null && annot.Dictionary.ContainsKey("Subtype"))
    {
      PdfName pdfName = PdfCrossTable.Dereference(annot.Dictionary["Subtype"]) as PdfName;
      if (pdfName != (PdfName) null && !this.m_savePopup)
        flag = pdfName.Value == "Popup";
    }
    if (!primitive.Contains((IPdfPrimitive) element) && !flag)
    {
      primitive.Add((IPdfPrimitive) element);
      this.m_page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
    int num = base.DoAdd(annot);
    if (annot != null && this.Page != null)
    {
      this.DoAddReviewHistory(annot);
      this.DoAddComments(annot);
    }
    return num;
  }

  internal string GetCorrectName(string name)
  {
    System.Collections.Generic.List<string> stringList = new System.Collections.Generic.List<string>();
    foreach (PdfAnnotation pdfAnnotation in this.List)
      stringList.Add(pdfAnnotation.Text);
    string correctName = name;
    int num = 0;
    while (stringList.IndexOf(correctName) != -1)
    {
      correctName = name + (object) num;
      ++num;
    }
    return correctName;
  }

  internal bool IsValidName(string name)
  {
    foreach (PdfAnnotation pdfAnnotation in this.List)
    {
      if (pdfAnnotation.Text == name)
        return false;
    }
    return true;
  }

  private int GetAnnotationIndex(string text)
  {
    int annotationIndex = -1;
    foreach (PdfAnnotation pdfAnnotation in this.List)
    {
      ++annotationIndex;
      if (!(pdfAnnotation.Text == text))
      {
        if (!string.IsNullOrEmpty(pdfAnnotation.Text))
        {
          if (pdfAnnotation.Text.Split('(')[0] == text)
            return annotationIndex;
        }
      }
      else
        break;
    }
    if (annotationIndex == this.List.Count - 1 && this.List[this.List.Count - 1] is PdfLoadedAnnotation loadedAnnotation && loadedAnnotation.Text != text)
      annotationIndex = -1;
    return annotationIndex;
  }

  private PdfAnnotation GetAnnotation(int index)
  {
    PdfDictionary dictionary = this.m_page.TerminalAnnotation[index];
    PdfCrossTable crossTable = this.m_page.CrossTable;
    PdfAnnotation annotation = (PdfAnnotation) null;
    if (dictionary == null || !dictionary.ContainsKey("Subtype"))
      return annotation;
    PdfLoadedAnnotationTypes annotationType = this.GetAnnotationType(PdfLoadedAnnotation.GetValue(dictionary, crossTable, "Subtype", true) as PdfName, dictionary, crossTable);
    if (!(PdfCrossTable.Dereference(dictionary["Rect"]) is PdfArray pdfArray1))
      return annotation;
    RectangleF rectangle = pdfArray1.ToRectangle();
    string empty = string.Empty;
    if (dictionary.ContainsKey("Contents") && PdfCrossTable.Dereference(dictionary["Contents"]) is PdfString pdfString1)
      empty = pdfString1.Value.ToString();
    switch (annotationType)
    {
      case PdfLoadedAnnotationTypes.Highlight:
      case PdfLoadedAnnotationTypes.Underline:
      case PdfLoadedAnnotationTypes.StrikeOut:
      case PdfLoadedAnnotationTypes.Squiggly:
        annotation = this.CreateMarkupAnnotation(dictionary, crossTable, rectangle);
        break;
      case PdfLoadedAnnotationTypes.RedactionAnnotation:
        annotation = this.CreateRedactionAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.AnnotationStates:
        annotation = this.CreateAnnotationStates(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.TextAnnotation:
        annotation = this.CreateTextAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.LinkAnnotation:
        if (dictionary.ContainsKey("A"))
        {
          PdfDictionary pdfDictionary1 = new PdfDictionary();
          PdfArray pdfArray2 = new PdfArray();
          if (PdfCrossTable.Dereference(dictionary["A"]) is PdfDictionary pdfDictionary3 && pdfDictionary3.ContainsKey("S"))
          {
            PdfArray destination = PdfCrossTable.Dereference(pdfDictionary3["D"]) as PdfArray;
            PdfName pdfName = PdfCrossTable.Dereference(pdfDictionary3["S"]) as PdfName;
            if (pdfName != (PdfName) null && pdfName.Value == "GoToR")
            {
              if (PdfCrossTable.Dereference(pdfDictionary3["F"]) is PdfString)
              {
                if (PdfCrossTable.Dereference(pdfDictionary3["F"]) is PdfString fileName)
                {
                  annotation = this.CreateFileRemoteGoToLinkAnnotation(dictionary, crossTable, fileName, destination, rectangle);
                  break;
                }
                break;
              }
              if (PdfCrossTable.Dereference(pdfDictionary3["F"]) is PdfDictionary && PdfCrossTable.Dereference(pdfDictionary3["F"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("F") && PdfCrossTable.Dereference(pdfDictionary2["F"]) is PdfString fileName1)
              {
                annotation = this.CreateFileRemoteGoToLinkAnnotation(dictionary, crossTable, fileName1, destination, rectangle);
                break;
              }
              break;
            }
            if (pdfName != (PdfName) null && pdfName.Value == "URI")
            {
              annotation = this.CreateLinkAnnotation(dictionary, crossTable, rectangle, empty);
              break;
            }
            break;
          }
          break;
        }
        annotation = this.CreateLinkAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.DocumentLinkAnnotation:
        annotation = this.CreateDocumentLinkAnnotation(dictionary, crossTable, rectangle);
        break;
      case PdfLoadedAnnotationTypes.FileLinkAnnotation:
        if (dictionary.ContainsKey("A") && PdfCrossTable.Dereference(dictionary["A"]) is PdfDictionary pdfDictionary4 && pdfDictionary4.ContainsKey("F"))
        {
          PdfDictionary pdfDictionary = PdfCrossTable.Dereference(pdfDictionary4["F"]) as PdfDictionary;
          if (pdfDictionary != null && pdfDictionary.ContainsKey("F"))
          {
            if (PdfCrossTable.Dereference(pdfDictionary["F"]) is PdfString pdfString2)
            {
              annotation = this.CreateFileLinkAnnotation(dictionary, crossTable, rectangle, pdfString2.Value);
              break;
            }
            break;
          }
          if (pdfDictionary != null && pdfDictionary.ContainsKey("UF") && PdfCrossTable.Dereference(pdfDictionary["UF"]) is PdfString pdfString3)
          {
            annotation = this.CreateFileLinkAnnotation(dictionary, crossTable, rectangle, pdfString3.Value);
            break;
          }
          break;
        }
        break;
      case PdfLoadedAnnotationTypes.FreeTextAnnotation:
        annotation = this.CreateFreeTextAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.LineAnnotation:
        annotation = this.CreateLineAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.CircleAnnotation:
        annotation = this.CreateCircleAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.EllipseAnnotation:
        annotation = this.CreateEllipseAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.SquareAnnotation:
        annotation = this.CreateSquareAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.RectangleAnnotation:
        annotation = this.CreateRectangleAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.PolygonAnnotation:
        annotation = this.CreatePolygonAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.PolyLineAnnotation:
        annotation = this.CreatePolyLineAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.SquareandCircleAnnotation:
        annotation = this.CreateSquareandCircleAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.PolygonandPolylineAnnotation:
        annotation = this.CreatePolygonandPolylineAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.TextMarkupAnnotation:
        annotation = this.CreateTextMarkupAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.CaretAnnotation:
        annotation = this.CreateCaretAnnotation(dictionary, crossTable, rectangle);
        break;
      case PdfLoadedAnnotationTypes.RubberStampAnnotation:
        annotation = this.CreateRubberStampAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.LnkAnnotation:
        PdfString pdfString4 = (PdfString) null;
        if (dictionary.ContainsKey("A") && PdfCrossTable.Dereference(dictionary["A"]) is PdfDictionary pdfDictionary5 && pdfDictionary5.ContainsKey("F") && PdfCrossTable.Dereference(pdfDictionary5["F"]) is PdfDictionary pdfDictionary6 && pdfDictionary6.ContainsKey("F"))
          pdfString4 = PdfCrossTable.Dereference(pdfDictionary6["F"]) as PdfString;
        if (pdfString4 != null)
        {
          annotation = this.CreateLnkAnnotation(dictionary, crossTable, rectangle, pdfString4.Value.Substring(1));
          break;
        }
        break;
      case PdfLoadedAnnotationTypes.PopupAnnotation:
        annotation = this.CreatePopupAnnotation(dictionary, crossTable, rectangle, empty);
        break;
      case PdfLoadedAnnotationTypes.FileAttachmentAnnotation:
        PdfDictionary pdfDictionary7 = PdfCrossTable.Dereference(dictionary["FS"]) as PdfDictionary;
        PdfString pdfString5 = (PdfString) null;
        if (pdfDictionary7 != null && pdfDictionary7.ContainsKey("F"))
          pdfString5 = pdfDictionary7["F"] as PdfString;
        else if (pdfDictionary7 != null && pdfDictionary7.ContainsKey("UF"))
          pdfString5 = pdfDictionary7["UF"] as PdfString;
        if (pdfString5 == null)
          pdfString5 = new PdfString("");
        annotation = this.CreateFileAttachmentAnnotation(dictionary, crossTable, rectangle, pdfString5.Value);
        break;
      case PdfLoadedAnnotationTypes.SoundAnnotation:
        PdfDictionary pdfDictionary8 = PdfCrossTable.Dereference(dictionary["Sound"]) as PdfDictionary;
        annotation = this.CreateSoundAnnotation(dictionary, crossTable, rectangle);
        break;
      case PdfLoadedAnnotationTypes.MovieAnnotation:
        annotation = this.CreateMovieAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.ScreenAnnotation:
        annotation = this.CreateScreenAnnotation(dictionary, crossTable, rectangle);
        break;
      case PdfLoadedAnnotationTypes.WidgetAnnotation:
        annotation = this.CreateWidgetAnnotation(dictionary, crossTable, rectangle);
        break;
      case PdfLoadedAnnotationTypes.PrinterMarkAnnotation:
        annotation = this.CreatePrinterMarkAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.TrapNetworkAnnotation:
        annotation = this.CreateTrapNetworkAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.WatermarkAnnotation:
        annotation = this.CreateWatermarkAnnotation(dictionary, crossTable);
        break;
      case PdfLoadedAnnotationTypes.TextWebLinkAnnotation:
        annotation = this.CreateTextWebLinkAnnotation(dictionary, crossTable, empty);
        break;
      case PdfLoadedAnnotationTypes.InkAnnotation:
        annotation = this.CreateInkAnnotation(dictionary, crossTable, rectangle);
        break;
    }
    if (annotation is PdfLoadedAnnotation loadedAnnotation)
      loadedAnnotation.BeforeNameChanges += new PdfLoadedAnnotation.BeforeNameChangesEventHandler(this.ldAnnotation_NameChanded);
    return annotation;
  }

  internal PdfLoadedAnnotationTypes GetAnnotationType(
    PdfName name,
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    string str = name.Value;
    PdfLoadedAnnotationTypes annotationType = PdfLoadedAnnotationTypes.Null;
    if (PdfLoadedAnnotation.GetValue(dictionary, crossTable, "Subtype", true) is PdfNumber pdfNumber)
    {
      int intValue = pdfNumber.IntValue;
    }
    switch (str.ToLower())
    {
      case "sound":
        annotationType = PdfLoadedAnnotationTypes.SoundAnnotation;
        break;
      case "text":
      case "popup":
        annotationType = PdfLoadedAnnotationTypes.PopupAnnotation;
        break;
      case "link":
        PdfDictionary pdfDictionary = (PdfDictionary) null;
        if (dictionary.ContainsKey("A"))
          pdfDictionary = PdfCrossTable.Dereference(dictionary["A"]) as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("S"))
        {
          name = PdfCrossTable.Dereference(pdfDictionary["S"]) as PdfName;
          if (name != (PdfName) null)
          {
            bool annotation = this.FindAnnotation(PdfCrossTable.Dereference(dictionary["Border"]) as PdfArray);
            if (name.Value == "URI")
            {
              annotationType = annotation ? PdfLoadedAnnotationTypes.TextWebLinkAnnotation : PdfLoadedAnnotationTypes.LinkAnnotation;
              break;
            }
            if (name.Value == "Launch")
            {
              annotationType = PdfLoadedAnnotationTypes.FileLinkAnnotation;
              break;
            }
            if (name.Value == "GoToR")
            {
              annotationType = PdfLoadedAnnotationTypes.LinkAnnotation;
              break;
            }
            if (name.Value == "GoTo")
            {
              annotationType = PdfLoadedAnnotationTypes.DocumentLinkAnnotation;
              break;
            }
            break;
          }
          break;
        }
        if (dictionary.ContainsKey("Subtype"))
        {
          PdfName pdfName = PdfCrossTable.Dereference(dictionary["Subtype"]) as PdfName;
          if (pdfName != (PdfName) null)
          {
            switch (pdfName.Value)
            {
              case "Link":
                annotationType = PdfLoadedAnnotationTypes.DocumentLinkAnnotation;
                break;
            }
          }
          else
            break;
        }
        else
          break;
        break;
      case "fileattachment":
        annotationType = PdfLoadedAnnotationTypes.FileAttachmentAnnotation;
        break;
      case "line":
        annotationType = PdfLoadedAnnotationTypes.LineAnnotation;
        break;
      case "circle":
        if (PdfLoadedAnnotation.GetValue(dictionary, crossTable, "Rect", true) is PdfArray pdfArray1)
        {
          RectangleF rectangle = pdfArray1.ToRectangle();
          annotationType = (double) rectangle.Width == (double) rectangle.Height ? PdfLoadedAnnotationTypes.CircleAnnotation : PdfLoadedAnnotationTypes.EllipseAnnotation;
          break;
        }
        break;
      case "square":
        if (PdfLoadedAnnotation.GetValue(dictionary, crossTable, "Rect", true) is PdfArray pdfArray2)
        {
          RectangleF rectangle = pdfArray2.ToRectangle();
          annotationType = (double) rectangle.Width == (double) rectangle.Height ? PdfLoadedAnnotationTypes.SquareAnnotation : PdfLoadedAnnotationTypes.RectangleAnnotation;
          break;
        }
        break;
      case "polygon":
        annotationType = PdfLoadedAnnotationTypes.PolygonAnnotation;
        break;
      case "redact":
        annotationType = PdfLoadedAnnotationTypes.RedactionAnnotation;
        break;
      case "polyline":
        annotationType = PdfLoadedAnnotationTypes.PolyLineAnnotation;
        break;
      case "widget":
        annotationType = PdfLoadedAnnotationTypes.WidgetAnnotation;
        break;
      case "highlight":
        annotationType = PdfLoadedAnnotationTypes.Highlight;
        break;
      case "underline":
        annotationType = PdfLoadedAnnotationTypes.Underline;
        break;
      case "strikeout":
        annotationType = PdfLoadedAnnotationTypes.StrikeOut;
        break;
      case "squiggly":
        annotationType = PdfLoadedAnnotationTypes.Squiggly;
        break;
      case "stamp":
        annotationType = PdfLoadedAnnotationTypes.RubberStampAnnotation;
        break;
      case "ink":
        annotationType = PdfLoadedAnnotationTypes.InkAnnotation;
        break;
      case "freetext":
        annotationType = PdfLoadedAnnotationTypes.FreeTextAnnotation;
        break;
      case "caret":
        annotationType = PdfLoadedAnnotationTypes.CaretAnnotation;
        break;
    }
    return annotationType;
  }

  private PdfAnnotation CreateFileRemoteGoToLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    PdfString fileName,
    PdfArray destination,
    RectangleF rect)
  {
    PdfAnnotation toLinkAnnotation = (PdfAnnotation) new PdfLoadedFileLinkAnnotation(dictionary, crossTable, destination, rect, fileName.Value.ToString());
    toLinkAnnotation.SetPage((PdfPageBase) this.m_page);
    (toLinkAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return toLinkAnnotation;
  }

  private PdfAnnotation CreateTextWebLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    string text)
  {
    PdfAnnotation webLinkAnnotation = (PdfAnnotation) new PdfLoadedTextWebLinkAnnotation(dictionary, crossTable, text);
    webLinkAnnotation.SetPage((PdfPageBase) this.m_page);
    (webLinkAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return webLinkAnnotation;
  }

  private PdfAnnotation CreateDocumentLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect)
  {
    PdfAnnotation documentLinkAnnotation = (PdfAnnotation) new PdfLoadedDocumentLinkAnnotation(dictionary, crossTable, rect);
    documentLinkAnnotation.SetPage((PdfPageBase) this.m_page);
    (documentLinkAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return documentLinkAnnotation;
  }

  private PdfAnnotation CreateFileLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string filename)
  {
    PdfAnnotation fileLinkAnnotation = (PdfAnnotation) new PdfLoadedFileLinkAnnotation(dictionary, crossTable, rect, filename);
    fileLinkAnnotation.SetPage((PdfPageBase) this.m_page);
    (fileLinkAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return fileLinkAnnotation;
  }

  private PdfAnnotation CreateWidgetAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect)
  {
    PdfAnnotation widgetAnnotation = (PdfAnnotation) new PdfLoadedWidgetAnnotation(dictionary, crossTable, rect);
    widgetAnnotation.SetPage((PdfPageBase) this.m_page);
    (widgetAnnotation as PdfLoadedWidgetAnnotation).Page = this.Page;
    return widgetAnnotation;
  }

  private PdfAnnotation CreateInkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect)
  {
    PdfAnnotation inkAnnotation = (PdfAnnotation) new PdfLoadedInkAnnotation(dictionary, crossTable, rect);
    inkAnnotation.SetPage((PdfPageBase) this.m_page);
    (inkAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return inkAnnotation;
  }

  private PdfAnnotation CreateWatermarkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    PdfAnnotation watermarkAnnotation = (PdfAnnotation) new PdfTextMarkupAnnotation();
    watermarkAnnotation.SetPage((PdfPageBase) this.m_page);
    (watermarkAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return watermarkAnnotation;
  }

  private PdfAnnotation CreateTrapNetworkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    PdfAnnotation networkAnnotation = (PdfAnnotation) new PdfTextMarkupAnnotation();
    networkAnnotation.SetPage((PdfPageBase) this.m_page);
    (networkAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return networkAnnotation;
  }

  private PdfAnnotation CreateTextMarkupAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    PdfAnnotation markupAnnotation = (PdfAnnotation) new PdfTextMarkupAnnotation();
    markupAnnotation.SetPage((PdfPageBase) this.m_page);
    (markupAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return markupAnnotation;
  }

  private PdfAnnotation CreateTextAnnotation(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfAnnotation textAnnotation = (PdfAnnotation) new PdfTextMarkupAnnotation();
    textAnnotation.SetPage((PdfPageBase) this.m_page);
    (textAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return textAnnotation;
  }

  private PdfAnnotation CreateSquareandCircleAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    PdfAnnotation circleAnnotation = (PdfAnnotation) new PdfTextMarkupAnnotation();
    circleAnnotation.SetPage((PdfPageBase) this.m_page);
    (circleAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return circleAnnotation;
  }

  private PdfAnnotation CreateSoundAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect)
  {
    PdfLoadedAnnotation soundAnnotation = (PdfLoadedAnnotation) new PdfLoadedSoundAnnotation(dictionary, crossTable, rect);
    soundAnnotation.SetPage((PdfPageBase) this.m_page);
    soundAnnotation.Page = this.Page;
    return (PdfAnnotation) soundAnnotation;
  }

  private PdfAnnotation CreateScreenAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect)
  {
    PdfAnnotation screenAnnotation = (PdfAnnotation) new PdfLoadedTextMarkupAnnotation(dictionary, crossTable, rect);
    screenAnnotation.SetPage((PdfPageBase) this.m_page);
    (screenAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return screenAnnotation;
  }

  private PdfAnnotation CreateRubberStampAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string text)
  {
    PdfAnnotation rubberStampAnnotation = (PdfAnnotation) new PdfLoadedRubberStampAnnotation(dictionary, crossTable, rect, text);
    rubberStampAnnotation.SetPage((PdfPageBase) this.m_page);
    (rubberStampAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return rubberStampAnnotation;
  }

  private PdfAnnotation CreatePrinterMarkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    PdfAnnotation printerMarkAnnotation = (PdfAnnotation) new PdfTextMarkupAnnotation();
    printerMarkAnnotation.SetPage((PdfPageBase) this.m_page);
    (printerMarkAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return printerMarkAnnotation;
  }

  private PdfAnnotation CreatePopupAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string text)
  {
    PdfLoadedAnnotation popupAnnotation = (PdfLoadedAnnotation) new PdfLoadedPopupAnnotation(dictionary, crossTable, rect, text);
    popupAnnotation.SetPage((PdfPageBase) this.m_page);
    popupAnnotation.Page = this.Page;
    return (PdfAnnotation) popupAnnotation;
  }

  private PdfAnnotation CreatePolygonandPolylineAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    PdfAnnotation polylineAnnotation = (PdfAnnotation) new PdfTextMarkupAnnotation();
    polylineAnnotation.SetPage((PdfPageBase) this.m_page);
    (polylineAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return polylineAnnotation;
  }

  private PdfAnnotation CreateMovieAnnotation(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfAnnotation movieAnnotation = (PdfAnnotation) new PdfTextMarkupAnnotation();
    movieAnnotation.SetPage((PdfPageBase) this.m_page);
    (movieAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return movieAnnotation;
  }

  private PdfAnnotation CreateMarkupAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect)
  {
    PdfLoadedAnnotation markupAnnotation = (PdfLoadedAnnotation) new PdfLoadedTextMarkupAnnotation(dictionary, crossTable, rect);
    markupAnnotation.SetPage((PdfPageBase) this.m_page);
    markupAnnotation.Page = this.Page;
    return (PdfAnnotation) markupAnnotation;
  }

  private PdfAnnotation CreateLnkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string filename)
  {
    PdfLoadedAnnotation lnkAnnotation = (PdfLoadedAnnotation) new PdfLoadedFileLinkAnnotation(dictionary, crossTable, rect, filename);
    lnkAnnotation.SetPage((PdfPageBase) this.m_page);
    lnkAnnotation.Page = this.Page;
    return (PdfAnnotation) lnkAnnotation;
  }

  private PdfAnnotation CreateLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string text)
  {
    PdfLoadedAnnotation linkAnnotation = (PdfLoadedAnnotation) new PdfLoadedUriAnnotation(dictionary, crossTable, rect, text);
    linkAnnotation.SetPage((PdfPageBase) this.m_page);
    linkAnnotation.Page = this.Page;
    return (PdfAnnotation) linkAnnotation;
  }

  private PdfAnnotation CreateLineAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string text)
  {
    PdfLoadedAnnotation lineAnnotation = (PdfLoadedAnnotation) new PdfLoadedLineAnnotation(dictionary, crossTable, rect, text);
    lineAnnotation.SetPage((PdfPageBase) this.m_page);
    lineAnnotation.Page = this.Page;
    return (PdfAnnotation) lineAnnotation;
  }

  private PdfAnnotation CreateCircleAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string text)
  {
    PdfLoadedAnnotation circleAnnotation = (PdfLoadedAnnotation) new PdfLoadedCircleAnnotation(dictionary, crossTable, rect, text);
    circleAnnotation.SetPage((PdfPageBase) this.m_page);
    circleAnnotation.Page = this.Page;
    return (PdfAnnotation) circleAnnotation;
  }

  private PdfAnnotation CreateEllipseAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
  {
    PdfLoadedAnnotation ellipseAnnotation = (PdfLoadedAnnotation) new PdfLoadedEllipseAnnotation(dictionary, crossTable, rectangle, text);
    ellipseAnnotation.SetPage((PdfPageBase) this.m_page);
    ellipseAnnotation.Page = this.Page;
    return (PdfAnnotation) ellipseAnnotation;
  }

  private PdfAnnotation CreateSquareAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
  {
    PdfLoadedAnnotation squareAnnotation = (PdfLoadedAnnotation) new PdfLoadedSquareAnnotation(dictionary, crossTable, rectangle, text);
    squareAnnotation.SetPage((PdfPageBase) this.m_page);
    squareAnnotation.Page = this.Page;
    return (PdfAnnotation) squareAnnotation;
  }

  private PdfAnnotation CreateRectangleAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
  {
    PdfLoadedAnnotation rectangleAnnotation = (PdfLoadedAnnotation) new PdfLoadedRectangleAnnotation(dictionary, crossTable, rectangle, text);
    rectangleAnnotation.SetPage((PdfPageBase) this.m_page);
    rectangleAnnotation.Page = this.Page;
    return (PdfAnnotation) rectangleAnnotation;
  }

  private PdfAnnotation CreatePolygonAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
  {
    PdfLoadedAnnotation polygonAnnotation = (PdfLoadedAnnotation) new PdfLoadedPolygonAnnotation(dictionary, crossTable, rectangle, text);
    polygonAnnotation.SetPage((PdfPageBase) this.m_page);
    polygonAnnotation.Page = this.Page;
    return (PdfAnnotation) polygonAnnotation;
  }

  private PdfAnnotation CreatePolyLineAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
  {
    PdfLoadedAnnotation polyLineAnnotation = (PdfLoadedAnnotation) new PdfLoadedPolyLineAnnotation(dictionary, crossTable, rectangle, text);
    polyLineAnnotation.SetPage((PdfPageBase) this.m_page);
    polyLineAnnotation.Page = this.Page;
    return (PdfAnnotation) polyLineAnnotation;
  }

  private PdfAnnotation CreateFreeTextAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string text)
  {
    PdfAnnotation freeTextAnnotation = (PdfAnnotation) new PdfLoadedFreeTextAnnotation(dictionary, crossTable, rect, text);
    freeTextAnnotation.SetPage((PdfPageBase) this.m_page);
    (freeTextAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return freeTextAnnotation;
  }

  private PdfAnnotation CreateRedactionAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    PdfLoadedAnnotation redactionAnnotation = (PdfLoadedAnnotation) new PdfLoadedRedactionAnnotation(dictionary, crossTable);
    redactionAnnotation.SetPage((PdfPageBase) this.m_page);
    redactionAnnotation.Page = this.Page;
    return (PdfAnnotation) redactionAnnotation;
  }

  private PdfAnnotation CreateFileAttachmentAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect,
    string filename)
  {
    PdfLoadedAnnotation attachmentAnnotation = (PdfLoadedAnnotation) new PdfLoadedAttachmentAnnotation(dictionary, crossTable, rect, filename);
    attachmentAnnotation.SetPage((PdfPageBase) this.m_page);
    attachmentAnnotation.Page = this.Page;
    return (PdfAnnotation) attachmentAnnotation;
  }

  private PdfAnnotation CreateCaretAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rect)
  {
    PdfAnnotation caretAnnotation = (PdfAnnotation) new PdfLoadedTextMarkupAnnotation(dictionary, crossTable, rect);
    caretAnnotation.SetPage((PdfPageBase) this.m_page);
    (caretAnnotation as PdfLoadedAnnotation).Page = this.Page;
    return caretAnnotation;
  }

  private PdfAnnotation CreateAnnotationStates(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfAnnotation annotationStates = (PdfAnnotation) new PdfTextMarkupAnnotation();
    annotationStates.SetPage((PdfPageBase) this.m_page);
    (annotationStates as PdfLoadedAnnotation).Page = this.Page;
    return annotationStates;
  }

  protected override void DoInsert(int index, PdfAnnotation annot)
  {
    if (index < 0 || index > this.List.Count)
      throw new IndexOutOfRangeException();
    if (annot == null)
      throw new ArgumentNullException("annotation");
    annot.SetPage((PdfPageBase) this.m_page);
    if (!(annot is PdfLoadedAnnotation))
    {
      PdfArray primitive = (PdfArray) null;
      if (this.m_page.Dictionary.ContainsKey("Annots"))
        primitive = this.m_page.CrossTable.GetObject(this.m_page.Dictionary["Annots"]) as PdfArray;
      if (primitive == null)
        primitive = new PdfArray();
      primitive.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annot));
      this.m_page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
    base.DoInsert(index, annot);
  }

  protected override void DoClear()
  {
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
    {
      if (this.List[index] is PdfAnnotation annot)
        this.m_page.RemoveFromDictionaries(annot);
    }
  }

  protected override void DoRemoveAt(int index)
  {
    if (index < 0 || index > this.List.Count)
      throw new IndexOutOfRangeException();
    if (this.List[index] is PdfAnnotation annot)
      this.m_page.RemoveFromDictionaries(annot);
    base.DoRemoveAt(index);
  }

  protected override void DoRemove(PdfAnnotation annot)
  {
    if (annot == null)
      throw new ArgumentNullException("annotation");
    this.m_page.RemoveFromDictionaries(annot);
    base.DoRemove(annot);
  }

  internal bool FindAnnotation(PdfArray arr)
  {
    if (arr == null)
      return false;
    for (int index1 = 0; index1 < arr.Count; ++index1)
    {
      if (arr[index1] is PdfArray)
      {
        PdfArray pdfArray = arr[index1] as PdfArray;
        for (int index2 = 0; index2 < pdfArray.Count; ++index2)
        {
          PdfNumber pdfNumber = pdfArray[index2] as PdfNumber;
          int num = 0;
          if (pdfNumber != null)
            num = pdfNumber.IntValue;
          if (num > 0)
            return false;
        }
      }
      else
      {
        int num = 0;
        if (arr[index1] is PdfNumber pdfNumber)
          num = pdfNumber.IntValue;
        if (num > 0)
          return false;
      }
    }
    return true;
  }

  internal PdfArray Rearrange(PdfReference reference, int tabIndex, int index)
  {
    if (this.m_page.CrossTable.GetObject(this.m_page.Dictionary["Annots"]) is PdfArray pdfArray)
    {
      if (tabIndex > pdfArray.Count)
        tabIndex = 0;
      if (index >= pdfArray.Count)
        index = this.m_page.AnnotsReference.IndexOf((IPdfPrimitive) reference);
      PdfReferenceHolder element = pdfArray.Elements[index] as PdfReferenceHolder;
      if (element != (PdfReferenceHolder) null && element.Object is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Parent"))
      {
        PdfReferenceHolder pdfReferenceHolder = pdfDictionary["Parent"] as PdfReferenceHolder;
        if (element.Reference == reference || pdfReferenceHolder != (PdfReferenceHolder) null && reference == pdfReferenceHolder.Reference)
        {
          IPdfPrimitive pdfPrimitive = pdfArray[index];
          pdfArray.Elements[index] = pdfArray[tabIndex];
          pdfArray.Elements[tabIndex] = pdfPrimitive;
        }
      }
    }
    return pdfArray;
  }
}
