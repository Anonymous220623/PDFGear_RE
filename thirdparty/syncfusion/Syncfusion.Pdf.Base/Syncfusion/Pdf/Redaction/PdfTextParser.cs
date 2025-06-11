// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Redaction.PdfTextParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Exporting;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading;

#nullable disable
namespace Syncfusion.Pdf.Redaction;

internal class PdfTextParser
{
  private PdfLoadedPage m_loadedPage;
  private List<RectangleF> m_bounds = new List<RectangleF>();
  private Page m_page;
  private PdfImageRenderer m_renderer;
  private float pt = 1.3333f;
  internal bool redactionTrackProcess;

  internal PdfTextParser(PdfLoadedPage page)
  {
    this.m_loadedPage = page;
    this.CombineBounds();
  }

  internal void Process()
  {
    PdfGraphics graphics1 = this.m_loadedPage.Graphics;
    Syncfusion.PdfViewer.Base.DeviceCMYK cmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
    this.m_page = new Page((PdfPageBase) this.m_loadedPage);
    this.m_page.Initialize((PdfPageBase) this.m_loadedPage, true);
    PdfUnitConvertor pdfUnitConvertor1 = new PdfUnitConvertor();
    float num1 = 1f;
    if (this.m_loadedPage.Document != null && this.m_loadedPage.Document.Catalog != null && this.m_loadedPage.Document.Catalog.ContainsKey("StructTreeRoot"))
      this.m_loadedPage.Document.Catalog.Remove("StructTreeRoot");
    Bitmap bitmap1;
    if (this.m_page.CropBox != RectangleF.Empty && this.m_page.CropBox != this.m_page.MediaBox)
    {
      PdfUnitConvertor pdfUnitConvertor2 = pdfUnitConvertor1;
      RectangleF cropBox = this.m_page.CropBox;
      double width1 = (double) cropBox.Width;
      cropBox = this.m_page.CropBox;
      double x = (double) cropBox.X;
      double num2 = width1 - x;
      int width2 = (int) ((double) pdfUnitConvertor2.ConvertToPixels((float) num2, PdfGraphicsUnit.Point) * (double) num1);
      PdfUnitConvertor pdfUnitConvertor3 = pdfUnitConvertor1;
      cropBox = this.m_page.CropBox;
      double height1 = (double) cropBox.Height;
      cropBox = this.m_page.CropBox;
      double y = (double) cropBox.Y;
      double num3 = height1 - y;
      int height2 = (int) ((double) pdfUnitConvertor3.ConvertToPixels((float) num3, PdfGraphicsUnit.Point) * (double) num1);
      bitmap1 = new Bitmap(width2, height2);
    }
    else
      bitmap1 = new Bitmap((int) ((double) this.m_page.Bounds.Width * (double) num1), (int) ((double) this.m_page.Bounds.Height * (double) num1));
    MemoryStream memoryStream = new MemoryStream();
    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((Image) bitmap1))
    {
      if (cmyk == null)
        cmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      this.m_renderer = new PdfImageRenderer(this.m_page.RecordCollection, this.m_page.Resources, g, false, this.m_page.Height, (float) this.m_page.CurrentLeftLocation, cmyk);
      this.m_renderer.pageRotation = (float) this.m_page.Rotation;
      this.m_renderer.m_loadedPage = this.m_loadedPage;
      this.m_renderer.RedactionBounds = this.m_bounds;
      this.m_renderer.isFindText = true;
      memoryStream = this.m_renderer.RenderAsImage();
      this.m_renderer.isFindText = false;
    }
    if (this.m_loadedPage.Dictionary.ContainsKey("Contents"))
    {
      PdfStream stream = this.m_loadedPage.Graphics.StreamWriter.GetStream();
      stream.Data = memoryStream.ToArray();
      stream.Compress = true;
      this.GetArrayFromReferenceHolder(this.m_loadedPage.Dictionary["Contents"])?.Clear();
      this.m_loadedPage.Dictionary["Contents"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) stream);
      this.m_loadedPage.Graphics.StreamWriter = new PdfStreamWriter(stream);
    }
    if (this.m_renderer.PdfPaths.Count != 0)
    {
      PdfBrush brush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.White);
      for (int index = 0; index < this.m_renderer.PdfPaths.Count; ++index)
      {
        PdfPath pdfPath = this.m_renderer.PdfPaths[index];
        pdfPath.FillMode = PdfFillMode.Alternate;
        this.m_loadedPage.Graphics.DrawPath(PdfPens.White, brush, pdfPath);
      }
    }
    if (this.redactionTrackProcess)
    {
      RedactionProgressEventArgs arguments = new RedactionProgressEventArgs();
      arguments.m_progress = 50f;
      if (this.m_loadedPage != null && this.m_loadedPage.Document != null && this.m_loadedPage.Document is PdfLoadedDocument)
        (this.m_loadedPage.Document as PdfLoadedDocument).OnTrackProgress(arguments);
    }
    PdfLoadedAnnotationCollection annotations = this.m_loadedPage.Annotations;
    for (int index1 = 0; index1 < annotations.Count; ++index1)
    {
      PdfDictionary dictionary = annotations[index1].Dictionary;
      PdfCrossTable crossTable = this.m_loadedPage.CrossTable;
      PdfName name = PdfLoadedAnnotation.GetValue(dictionary, crossTable, "Subtype", true) as PdfName;
      PdfLoadedAnnotationTypes annotationType = annotations.GetAnnotationType(name, dictionary, crossTable);
      RectangleF rect = RectangleF.Empty;
      bool isValidAnnotation = true;
      switch (annotationType)
      {
        case PdfLoadedAnnotationTypes.Highlight:
        case PdfLoadedAnnotationTypes.Underline:
        case PdfLoadedAnnotationTypes.StrikeOut:
        case PdfLoadedAnnotationTypes.Squiggly:
        case PdfLoadedAnnotationTypes.ScreenAnnotation:
          if (annotations[index1] is PdfLoadedTextMarkupAnnotation markupAnnotation1)
          {
            if (markupAnnotation1.BoundsCollection != null && markupAnnotation1.BoundsCollection.Count > 0)
            {
              bool flag = false;
              foreach (RectangleF bounds in markupAnnotation1.BoundsCollection)
              {
                if (this.IsFoundRect(bounds))
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
              {
                annotations.RemoveAt(index1);
                --index1;
                continue;
              }
            }
            rect = markupAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfTextMarkupAnnotation markupAnnotation2)
          {
            rect = markupAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.AnnotationStates:
        case PdfLoadedAnnotationTypes.TextAnnotation:
        case PdfLoadedAnnotationTypes.SquareandCircleAnnotation:
        case PdfLoadedAnnotationTypes.PolygonandPolylineAnnotation:
        case PdfLoadedAnnotationTypes.TextMarkupAnnotation:
        case PdfLoadedAnnotationTypes.CaretAnnotation:
        case PdfLoadedAnnotationTypes.MovieAnnotation:
        case PdfLoadedAnnotationTypes.PrinterMarkAnnotation:
        case PdfLoadedAnnotationTypes.TrapNetworkAnnotation:
        case PdfLoadedAnnotationTypes.WatermarkAnnotation:
          if (annotations[index1] is PdfTextMarkupAnnotation markupAnnotation3)
          {
            rect = markupAnnotation3.Bounds;
            if (markupAnnotation3.BoundsCollection != null && markupAnnotation3.BoundsCollection.Count > 0)
            {
              bool flag = false;
              foreach (RectangleF bounds in markupAnnotation3.BoundsCollection)
              {
                if (this.IsFoundRect(bounds))
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
              {
                annotations.RemoveAt(index1);
                --index1;
                continue;
              }
              break;
            }
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.LinkAnnotation:
          if (annotations[index1] is PdfLoadedFileLinkAnnotation fileLinkAnnotation1)
          {
            rect = fileLinkAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfLoadedUriAnnotation loadedUriAnnotation)
          {
            rect = loadedUriAnnotation.Bounds;
            break;
          }
          if (annotations[index1] is PdfUriAnnotation pdfUriAnnotation)
          {
            rect = pdfUriAnnotation.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.DocumentLinkAnnotation:
          if (annotations[index1] is PdfLoadedDocumentLinkAnnotation documentLinkAnnotation)
          {
            rect = documentLinkAnnotation.Bounds;
            break;
          }
          if (annotations[index1] is PdfFileLinkAnnotation fileLinkAnnotation2)
          {
            rect = fileLinkAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.FileLinkAnnotation:
        case PdfLoadedAnnotationTypes.LnkAnnotation:
          if (annotations[index1] is PdfLoadedFileLinkAnnotation fileLinkAnnotation3)
          {
            rect = fileLinkAnnotation3.Bounds;
            break;
          }
          if (annotations[index1] is PdfFileLinkAnnotation fileLinkAnnotation4)
          {
            rect = fileLinkAnnotation4.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.FreeTextAnnotation:
          if (annotations[index1] is PdfLoadedFreeTextAnnotation freeTextAnnotation1)
          {
            rect = freeTextAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfFreeTextAnnotation freeTextAnnotation2)
          {
            rect = freeTextAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.LineAnnotation:
          PdfLoadedLineAnnotation loadedLineAnnotation = annotations[index1] as PdfLoadedLineAnnotation;
          int[] numArray = new int[4];
          if (loadedLineAnnotation != null)
          {
            numArray = loadedLineAnnotation.LinePoints;
            rect = loadedLineAnnotation.Bounds;
          }
          else if (annotations[index1] is PdfLineAnnotation pdfLineAnnotation)
          {
            numArray = pdfLineAnnotation.LinePoints;
            rect = pdfLineAnnotation.Bounds;
          }
          bool flag1 = false;
          foreach (PdfRedaction redaction in this.m_loadedPage.Redactions)
          {
            if (this.IsLineIntersectRectangle(redaction.Bounds, (double) numArray[0], (double) this.m_loadedPage.Graphics.Size.Height - (double) numArray[1], (double) numArray[2], (double) this.m_loadedPage.Graphics.Size.Height - (double) numArray[3]))
            {
              redaction.m_success = true;
              flag1 = true;
              break;
            }
          }
          if (flag1)
          {
            annotations.RemoveAt(index1);
            --index1;
            continue;
          }
          break;
        case PdfLoadedAnnotationTypes.CircleAnnotation:
          if (annotations[index1] is PdfLoadedCircleAnnotation circleAnnotation1)
          {
            rect = circleAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfCircleAnnotation circleAnnotation2)
          {
            rect = circleAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.EllipseAnnotation:
          if (annotations[index1] is PdfLoadedEllipseAnnotation ellipseAnnotation1)
          {
            rect = ellipseAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfEllipseAnnotation ellipseAnnotation2)
          {
            rect = ellipseAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.SquareAnnotation:
          if (annotations[index1] is PdfLoadedSquareAnnotation squareAnnotation1)
          {
            rect = squareAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfSquareAnnotation squareAnnotation2)
          {
            rect = squareAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.RectangleAnnotation:
          if (annotations[index1] is PdfLoadedRectangleAnnotation rectangleAnnotation1)
          {
            rect = rectangleAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfRectangleAnnotation rectangleAnnotation2)
          {
            rect = rectangleAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.PolygonAnnotation:
          if (annotations[index1] is PdfLoadedPolygonAnnotation polygonAnnotation)
          {
            int[] polygonPoints = polygonAnnotation.PolygonPoints;
            float[] points = new float[polygonPoints.Length];
            int index2 = 0;
            foreach (int num4 in polygonPoints)
            {
              points[index2] = (float) num4;
              ++index2;
            }
            rect = this.GetBoundsFromPoints(points, out isValidAnnotation);
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.PolyLineAnnotation:
          if (annotations[index1] is PdfLoadedPolyLineAnnotation polyLineAnnotation)
          {
            int[] polylinePoints = polyLineAnnotation.PolylinePoints;
            float[] points = new float[polylinePoints.Length];
            int index3 = 0;
            foreach (int num5 in polylinePoints)
            {
              points[index3] = (float) num5;
              ++index3;
            }
            rect = this.GetBoundsFromPoints(points, out isValidAnnotation);
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.RubberStampAnnotation:
          if (annotations[index1] is PdfLoadedRubberStampAnnotation rubberStampAnnotation1)
          {
            rect = rubberStampAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfRubberStampAnnotation rubberStampAnnotation2)
          {
            rect = rubberStampAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.PopupAnnotation:
          if (annotations[index1] is PdfLoadedPopupAnnotation loadedPopupAnnotation)
          {
            rect = loadedPopupAnnotation.Bounds;
            break;
          }
          if (annotations[index1] is PdfPopupAnnotation pdfPopupAnnotation)
          {
            rect = pdfPopupAnnotation.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.FileAttachmentAnnotation:
          if (annotations[index1] is PdfLoadedAttachmentAnnotation attachmentAnnotation1)
          {
            rect = attachmentAnnotation1.Bounds;
            break;
          }
          if (annotations[index1] is PdfAttachmentAnnotation attachmentAnnotation2)
          {
            rect = attachmentAnnotation2.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.SoundAnnotation:
          if (annotations[index1] is PdfLoadedSoundAnnotation loadedSoundAnnotation)
          {
            rect = loadedSoundAnnotation.Bounds;
            break;
          }
          if (annotations[index1] is PdfSoundAnnotation pdfSoundAnnotation)
          {
            rect = pdfSoundAnnotation.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.TextWebLinkAnnotation:
          if (annotations[index1] is PdfLoadedTextWebLinkAnnotation webLinkAnnotation)
          {
            rect = webLinkAnnotation.Bounds;
            break;
          }
          break;
        case PdfLoadedAnnotationTypes.InkAnnotation:
          if (annotations[index1] is PdfLoadedInkAnnotation loadedInkAnnotation)
          {
            rect = this.GetBoundsFromPoints(loadedInkAnnotation.InkList.ToArray(), out isValidAnnotation);
            break;
          }
          if (annotations[index1] is PdfInkAnnotation pdfInkAnnotation)
          {
            rect = this.GetBoundsFromPoints(pdfInkAnnotation.InkList.ToArray(), out isValidAnnotation);
            break;
          }
          break;
        default:
          rect = new RectangleF();
          isValidAnnotation = false;
          break;
      }
      if (isValidAnnotation && this.IsFoundRect(rect))
      {
        annotations.RemoveAt(index1);
        --index1;
      }
    }
    List<PdfReferenceHolder> pdfReferenceHolderList = new List<PdfReferenceHolder>();
    if (this.m_loadedPage.Dictionary.ContainsKey("Annots"))
    {
      PdfArray pdfArray = this.m_loadedPage.CrossTable.GetObject(this.m_loadedPage.Dictionary["Annots"]) as PdfArray;
      PdfDocumentBase document = this.m_loadedPage.Document;
      if (pdfArray != null)
      {
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          PdfDictionary pdfDictionary = this.m_loadedPage.CrossTable.GetObject(pdfArray[index]) as PdfDictionary;
          PdfReferenceHolder pdfReferenceHolder = pdfArray[index] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Reference != (PdfReference) null && pdfDictionary.ContainsKey("Subtype") && (pdfDictionary["Subtype"] as PdfName).Value == "Widget")
            pdfReferenceHolderList.Add(pdfReferenceHolder);
        }
      }
    }
    PdfLoadedForm form = (this.m_loadedPage.Document as PdfLoadedDocument).Form;
    if (form != null)
    {
      PdfLoadedFormFieldCollection fields = form.Fields;
      bool flag = true;
      for (int index4 = 0; index4 < fields.Count; ++index4)
      {
        PdfLoadedField pdfLoadedField = fields[index4] as PdfLoadedField;
        if (pdfLoadedField.Page == this.m_loadedPage)
        {
          RectangleF rect = RectangleF.Empty;
          PdfDictionary dictionary = pdfLoadedField.Dictionary;
          PdfCrossTable crossTable = form.CrossTable;
          PdfName name = PdfLoadedField.GetValue(dictionary, crossTable, "FT", true) as PdfName;
          PdfLoadedFieldTypes loadedFieldTypes = PdfLoadedFieldTypes.Null;
          if (name != (PdfName) null)
            loadedFieldTypes = form.Fields.GetFieldType(name, dictionary, crossTable);
          switch (loadedFieldTypes)
          {
            case PdfLoadedFieldTypes.PushButton:
              rect = (pdfLoadedField as PdfLoadedButtonField).Bounds;
              break;
            case PdfLoadedFieldTypes.CheckBox:
              rect = (pdfLoadedField as PdfLoadedCheckBoxField).Bounds;
              break;
            case PdfLoadedFieldTypes.RadioButton:
              PdfArray kids1 = (pdfLoadedField as PdfLoadedRadioButtonListField).Kids;
              if (kids1.Count > 0)
              {
                for (int index5 = 0; index5 < kids1.Count; ++index5)
                {
                  PdfDictionary pdfDictionary = (PdfDictionary) null;
                  if ((object) (kids1[index5] as PdfReferenceHolder) != null)
                    pdfDictionary = (kids1[index5] as PdfReferenceHolder).Object as PdfDictionary;
                  else if (kids1[index5] is PdfDictionary)
                    pdfDictionary = kids1[index5] as PdfDictionary;
                  if (pdfDictionary != null && pdfDictionary.ContainsKey("Rect"))
                  {
                    rect = (pdfDictionary["Rect"] as PdfArray).ToRectangle();
                    rect.Y = this.m_loadedPage.Graphics.Size.Height - (rect.Y + rect.Height);
                    if (!rect.IsEmpty && this.IsFoundRect(rect))
                    {
                      PdfReferenceHolder element = (object) (kids1[index5] as PdfReferenceHolder) != null ? kids1[index5] as PdfReferenceHolder : new PdfReferenceHolder(kids1[index5]);
                      if (this.m_loadedPage.Dictionary.ContainsKey("Annots"))
                      {
                        PdfArray primitive = form.CrossTable.GetObject(this.m_loadedPage.Dictionary["Annots"]) as PdfArray;
                        primitive.Remove((IPdfPrimitive) element);
                        primitive.MarkChanged();
                        this.m_loadedPage.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
                      }
                      (pdfLoadedField as PdfLoadedRadioButtonListField).Dictionary.Modify();
                      kids1.RemoveAt(index5);
                      kids1.MarkChanged();
                      --index5;
                    }
                  }
                }
                break;
              }
              rect = (pdfLoadedField as PdfLoadedRadioButtonListField).Bounds;
              break;
            case PdfLoadedFieldTypes.TextField:
              PdfLoadedTextBoxField loadedTextBoxField = pdfLoadedField as PdfLoadedTextBoxField;
              if (loadedTextBoxField.Kids != null && loadedTextBoxField.Kids.Count > 1)
              {
                PdfArray kids2 = loadedTextBoxField.Kids;
                for (int index6 = 0; index6 < kids2.Count; ++index6)
                {
                  PdfDictionary kid = (PdfDictionary) null;
                  if ((object) (kids2[index6] as PdfReferenceHolder) != null)
                    kid = (kids2[index6] as PdfReferenceHolder).Object as PdfDictionary;
                  else if (kids2[index6] is PdfDictionary)
                    kid = kids2[index6] as PdfDictionary;
                  if (kid != null && kid.ContainsKey("Rect"))
                  {
                    rect = (kid["Rect"] as PdfArray).ToRectangle();
                    rect.Y = this.m_loadedPage.Graphics.Size.Height - (rect.Y + rect.Height);
                    if (!rect.IsEmpty && this.IsFoundRect(rect) && this.IsKidInSamePage(kid))
                    {
                      PdfReferenceHolder element = (object) (kids2[index6] as PdfReferenceHolder) != null ? kids2[index6] as PdfReferenceHolder : new PdfReferenceHolder(kids2[index6]);
                      if (this.m_loadedPage.Dictionary.ContainsKey("Annots"))
                      {
                        PdfArray primitive = form.CrossTable.GetObject(this.m_loadedPage.Dictionary["Annots"]) as PdfArray;
                        primitive.Remove((IPdfPrimitive) element);
                        primitive.MarkChanged();
                        this.m_loadedPage.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
                      }
                      (pdfLoadedField as PdfLoadedTextBoxField).Dictionary.Modify();
                      kids2.RemoveAt(index6);
                      kids2.MarkChanged();
                      --index6;
                      flag = false;
                    }
                  }
                }
                break;
              }
              rect = loadedTextBoxField.Bounds;
              break;
            case PdfLoadedFieldTypes.ListBox:
              rect = (pdfLoadedField as PdfLoadedListBoxField).Bounds;
              break;
            case PdfLoadedFieldTypes.ComboBox:
              rect = (pdfLoadedField as PdfLoadedComboBoxField).Bounds;
              break;
            case PdfLoadedFieldTypes.SignatureField:
              rect = (pdfLoadedField as PdfLoadedSignatureField).Bounds;
              break;
            case PdfLoadedFieldTypes.Null:
              rect = (pdfLoadedField as PdfLoadedStyledField).Bounds;
              break;
          }
          bool isEmpty = rect.IsEmpty;
          if (flag && !isEmpty && this.IsFoundRect(rect))
          {
            form.Fields.RemoveAt(index4);
            --index4;
          }
          flag = true;
        }
      }
    }
    if (this.m_loadedPage.m_imageinfo != null)
      this.m_loadedPage.m_imageinfo = (PdfImageInfo[]) null;
    this.m_loadedPage.is_Contains_Redaction = true;
    PdfImageInfo[] imagesInfo = this.m_loadedPage.ImagesInfo;
    this.m_loadedPage.is_Contains_Redaction = false;
    PdfPageRotateAngle rotation = this.m_loadedPage.Rotation;
    foreach (PdfImageInfo pdfImageInfo in imagesInfo)
    {
      RectangleF bounds1 = pdfImageInfo.Bounds;
      bounds1.X *= this.pt;
      bounds1.Y *= this.pt;
      bounds1.Width *= this.pt;
      bounds1.Height *= this.pt;
      Bitmap bitmap2 = pdfImageInfo.Image as Bitmap;
      if (bitmap2.PixelFormat == PixelFormat.Undefined || bitmap2.PixelFormat == PixelFormat.Format1bppIndexed || bitmap2.PixelFormat == PixelFormat.Format8bppIndexed || bitmap2.PixelFormat == PixelFormat.Format16bppArgb1555 || bitmap2.PixelFormat == PixelFormat.Undefined || bitmap2.PixelFormat == PixelFormat.Format16bppGrayScale || bitmap2.PixelFormat == PixelFormat.Format4bppIndexed)
        bitmap2 = new Bitmap(pdfImageInfo.Image);
      bool flag = false;
      using (System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage((Image) bitmap2))
      {
        SizeF size = this.m_loadedPage.Size;
        foreach (PdfRedaction redaction in this.m_loadedPage.Redactions)
        {
          RectangleF rectangleF = RectangleF.Intersect(new RectangleF(redaction.Bounds.X * this.pt, redaction.Bounds.Y * this.pt, redaction.Bounds.Width * this.pt, redaction.Bounds.Height * this.pt), bounds1);
          if (rectangleF != RectangleF.Empty)
          {
            flag = true;
            RectangleF rect;
            switch (rotation)
            {
              case PdfPageRotateAngle.RotateAngle90:
                rectangleF = new RectangleF(rectangleF.Y, size.Height * this.pt - rectangleF.X - rectangleF.Width, rectangleF.Height, rectangleF.Width);
                double height3 = (double) size.Height;
                RectangleF bounds2 = pdfImageInfo.Bounds;
                double x1 = (double) bounds2.X;
                bounds2 = pdfImageInfo.Bounds;
                double width3 = (double) bounds2.Width;
                double num6 = x1 + width3;
                double num7 = height3 - num6;
                bounds2 = pdfImageInfo.Bounds;
                double y1 = (double) bounds2.Y;
                float num8 = (float) (num7 - y1) * this.pt;
                rect = new RectangleF((rectangleF.X - bounds1.Y) * (float) bitmap2.Width / bounds1.Height, (rectangleF.Y - (bounds1.Y + num8)) * (float) bitmap2.Height / bounds1.Width, rectangleF.Width * (float) bitmap2.Width / bounds1.Height, rectangleF.Height * (float) bitmap2.Height / bounds1.Width);
                break;
              case PdfPageRotateAngle.RotateAngle180:
                rectangleF = new RectangleF(size.Width * this.pt - rectangleF.X - rectangleF.Width, size.Height * this.pt - rectangleF.Y - rectangleF.Height, rectangleF.Width, rectangleF.Height);
                float num9 = (size.Width - (pdfImageInfo.Bounds.Width + pdfImageInfo.Bounds.X) - pdfImageInfo.Bounds.X) * this.pt;
                double height4 = (double) size.Height;
                RectangleF bounds3 = pdfImageInfo.Bounds;
                double height5 = (double) bounds3.Height;
                bounds3 = pdfImageInfo.Bounds;
                double y2 = (double) bounds3.Y;
                double num10 = height5 + y2;
                double num11 = height4 - num10;
                bounds3 = pdfImageInfo.Bounds;
                double y3 = (double) bounds3.Y;
                float num12 = (float) (num11 - y3) * this.pt;
                rect = new RectangleF((rectangleF.X - (bounds1.X + num9)) * (float) bitmap2.Width / bounds1.Width, (rectangleF.Y - (bounds1.Y + num12)) * (float) bitmap2.Height / bounds1.Height, rectangleF.Width * (float) bitmap2.Width / bounds1.Width, rectangleF.Height * (float) bitmap2.Height / bounds1.Height);
                break;
              case PdfPageRotateAngle.RotateAngle270:
                rectangleF = new RectangleF(size.Width * this.pt - rectangleF.Y - rectangleF.Height, rectangleF.X, rectangleF.Height, rectangleF.Width);
                double width4 = (double) size.Width;
                RectangleF bounds4 = pdfImageInfo.Bounds;
                double y4 = (double) bounds4.Y;
                bounds4 = pdfImageInfo.Bounds;
                double height6 = (double) bounds4.Height;
                double num13 = y4 + height6;
                double num14 = width4 - num13;
                bounds4 = pdfImageInfo.Bounds;
                double x2 = (double) bounds4.X;
                float num15 = (float) (num14 - x2) * this.pt;
                rect = new RectangleF((rectangleF.X - (bounds1.X + num15)) * (float) bitmap2.Width / bounds1.Height, (rectangleF.Y - bounds1.X) * (float) bitmap2.Height / bounds1.Width, rectangleF.Width * (float) bitmap2.Width / bounds1.Height, rectangleF.Height * (float) bitmap2.Height / bounds1.Width);
                break;
              default:
                rect = new RectangleF((rectangleF.X - bounds1.X) * (float) bitmap2.Width / bounds1.Width, (rectangleF.Y - bounds1.Y) * (float) bitmap2.Height / bounds1.Height, rectangleF.Width * (float) bitmap2.Width / bounds1.Width, rectangleF.Height * (float) bitmap2.Height / bounds1.Height);
                break;
            }
            GraphicsPath path = new GraphicsPath();
            redaction.m_success = true;
            path.AddRectangle(rect);
            graphics2.SetClip(path);
            graphics2.Clear(Color.White);
            graphics2.ResetClip();
          }
        }
      }
      if (flag)
        this.m_loadedPage.ReplaceImage(pdfImageInfo.Index, (PdfImage) new PdfBitmap((Image) bitmap2));
    }
    foreach (PdfRedaction redaction in this.m_loadedPage.Redactions)
    {
      if (redaction.FillColor != Color.Transparent)
      {
        PdfPath path = new PdfPath();
        path.AddRectangle(redaction.Bounds);
        path.CloseAllFigures();
        if (redaction.PathRedaction)
        {
          if (redaction.m_success)
            this.m_loadedPage.Graphics.DrawPath(new PdfPen(redaction.FillColor), path);
        }
        else if (redaction.m_success)
          this.m_loadedPage.Graphics.DrawPath((PdfBrush) new PdfSolidBrush(new PdfColor(redaction.FillColor)), path);
      }
      if (redaction.AppearanceEnabled && redaction.m_success)
        this.m_loadedPage.Graphics.DrawPdfTemplate(redaction.Appearance, redaction.Bounds.Location);
    }
    if (!this.redactionTrackProcess)
      return;
    RedactionProgressEventArgs arguments1 = new RedactionProgressEventArgs();
    arguments1.m_progress = 100f;
    if (this.m_loadedPage == null || this.m_loadedPage.Document == null || !(this.m_loadedPage.Document is PdfLoadedDocument))
      return;
    (this.m_loadedPage.Document as PdfLoadedDocument).OnTrackProgress(arguments1);
  }

  private PdfStream GetSaveState()
  {
    PdfStream stream = new PdfStream();
    new PdfStreamWriter(stream).Write("q");
    return stream;
  }

  private PdfStream GetRestoreState()
  {
    PdfStream stream = new PdfStream();
    new PdfStreamWriter(stream).Write("Q");
    return stream;
  }

  private bool IsKidInSamePage(PdfDictionary kid)
  {
    PdfReference reference1 = this.m_loadedPage.CrossTable.GetReference((IPdfPrimitive) this.m_loadedPage.Dictionary);
    PdfReference reference2 = (kid["P"] as PdfReferenceHolder).Reference;
    return reference1.ObjNum == reference2.ObjNum && reference1.GenNum == reference2.GenNum;
  }

  private RectangleF GetBoundsFromPoints(float[] points, out bool isValidAnnotation)
  {
    int num = 0;
    if (points.Length > 0)
    {
      float x = points[0];
      float width = points[0];
      float y = this.m_loadedPage.Graphics.Size.Height - points[1];
      float height = this.m_loadedPage.Graphics.Size.Height - points[1];
      foreach (float point in points)
      {
        if (num % 2 == 0)
        {
          if ((double) x > (double) point)
            x = point;
          if ((double) width < (double) point)
            width = point;
        }
        else
        {
          if ((double) y > (double) this.m_loadedPage.Graphics.Size.Height - (double) point)
            y = this.m_loadedPage.Graphics.Size.Height - point;
          if ((double) height < (double) this.m_loadedPage.Graphics.Size.Height - (double) point)
            height = this.m_loadedPage.Graphics.Size.Height - point;
        }
        ++num;
      }
      isValidAnnotation = true;
      return new RectangleF(x, y, width, height);
    }
    isValidAnnotation = false;
    return new RectangleF();
  }

  private void CombineBounds()
  {
    foreach (PdfRedaction redaction in this.m_loadedPage.Redactions)
    {
      this.m_bounds.Add(redaction.Bounds);
      this.m_loadedPage.m_RedactionBounds.Add(redaction.Bounds);
    }
  }

  private PdfDictionary GetObject(IPdfPrimitive primitive)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (primitive is PdfDictionary)
      pdfDictionary = primitive as PdfDictionary;
    else if ((object) (primitive as PdfReferenceHolder) != null)
      pdfDictionary = (primitive as PdfReferenceHolder).Object as PdfDictionary;
    return pdfDictionary;
  }

  private bool IsLineIntersectRectangle(
    RectangleF redactBounds,
    double p1X,
    double p1Y,
    double p2X,
    double p2Y)
  {
    double num1 = p1X;
    double num2 = p2X;
    if (p1X > p2X)
    {
      num1 = p2X;
      num2 = p1X;
    }
    if (num2 > (double) redactBounds.X + (double) redactBounds.Width)
      num2 = (double) redactBounds.X + (double) redactBounds.Width;
    if (num1 < (double) redactBounds.X)
      num1 = (double) redactBounds.X;
    if (num1 > num2)
      return false;
    double num3 = p1Y;
    double num4 = p2Y;
    double num5 = p2X - p1X;
    if (num5 > 1E-07)
    {
      double num6 = (p2Y - p1Y) / num5;
      double num7 = p1Y - num6 * p1X;
      num3 = num6 * num1 + num7;
      num4 = num6 * num2 + num7;
    }
    if (num3 > num4)
    {
      double num8 = num4;
      num4 = num3;
      num3 = num8;
    }
    if (num4 > (double) redactBounds.Y + (double) redactBounds.Height)
      num4 = (double) redactBounds.Y + (double) redactBounds.Height;
    if (num3 < (double) redactBounds.Y)
      num3 = (double) redactBounds.Y;
    return num3 <= num4;
  }

  private bool IsFoundRect(RectangleF rect)
  {
    bool flag = false;
    foreach (PdfRedaction redaction in this.m_loadedPage.Redactions)
    {
      if (redaction.Bounds.IntersectsWith(rect))
      {
        redaction.m_success = true;
        flag = true;
        break;
      }
    }
    return flag;
  }

  private PdfArray GetArrayFromReferenceHolder(IPdfPrimitive primitive)
  {
    if ((object) (primitive as PdfReferenceHolder) == null)
      return primitive as PdfArray;
    PdfReferenceHolder pdfReferenceHolder = primitive as PdfReferenceHolder;
    return (object) (pdfReferenceHolder.Object as PdfReferenceHolder) != null ? this.GetArrayFromReferenceHolder(pdfReferenceHolder.Object) : pdfReferenceHolder.Object as PdfArray;
  }

  private PdfStream GetStreamFromRefernceHolder(IPdfPrimitive primitive)
  {
    if ((object) (primitive as PdfReferenceHolder) == null)
      return primitive as PdfStream;
    PdfReferenceHolder pdfReferenceHolder = primitive as PdfReferenceHolder;
    return (object) (pdfReferenceHolder.Object as PdfReferenceHolder) != null ? this.GetStreamFromRefernceHolder(pdfReferenceHolder.Object) : pdfReferenceHolder.Object as PdfStream;
  }
}
