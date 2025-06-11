// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.FreeTextUtils.FreeTextPdfTextAppearancesHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using PDFKit.Services;
using PDFKit.Utils.PdfRichTextStrings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

#nullable disable
namespace PDFKit.Utils.FreeTextUtils;

internal class FreeTextPdfTextAppearancesHelper : IFreeTextAppearancesHelper
{
  public void RegenerateAppearancesWithRichText(PdfFreeTextAnnotation annot, bool force)
  {
    if (!force && !FreeTextAppearancesHelper.CancellationTokenSources.IsFree(annot))
      return;
    FreeTextAppearancesHelper.CancellationTokenSources.GetOrCreateCts(annot);
    this.RegenerateAppearancesWithRichTextAsync(annot).GetAwaiter().GetResult();
    if (annot?.Page?.Document == null)
      return;
    PdfDocumentStateService.TryRedrawViewerCurrentPage(annot.Page);
  }

  public async Task RegenerateAppearancesWithRichTextAsync(
    PdfFreeTextAnnotation annot,
    RichTextBox rtb)
  {
    if (annot == null)
      throw new ArgumentNullException(nameof (annot));
    try
    {
      if (rtb == null)
      {
        await this.RegenerateAppearancesWithRichTextAsync(annot).ConfigureAwait(false);
      }
      else
      {
        try
        {
          FreeTextAppearancesHelper.CancellationTokenSources.CancelCts(annot);
          CancellationTokenSource freeTextRegenCts = FreeTextAppearancesHelper.CancellationTokenSources.GetOrCreateCts(annot);
          PdfDefaultAppearance da;
          if (!PdfDefaultAppearance.TryParse(annot.DefaultAppearance, out da))
            da = PdfDefaultAppearance.Default;
          if (annot.Color != FS_COLOR.Empty)
            da.FillColor = annot.Color;
          bool flag = await FreeTextPdfTextAppearancesHelper.TryRegenerateAppearancesWithRichTextCoreAsync(annot, rtb, da, freeTextRegenCts.Token);
          if (!flag)
            FreeTextPdfTextAppearancesHelper.TryRegenerateAppearancesCore(annot);
          freeTextRegenCts = (CancellationTokenSource) null;
          da = new PdfDefaultAppearance();
        }
        finally
        {
          FreeTextAppearancesHelper.CancellationTokenSources.CancelCts(annot);
        }
      }
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  public async Task RegenerateAppearancesWithRichTextAsync(PdfFreeTextAnnotation annot)
  {
    if (annot == null)
      throw new ArgumentNullException(nameof (annot));
    FreeTextAppearancesHelper.CancellationTokenSources.CancelCts(annot);
    CancellationTokenSource freeTextRegenCts = FreeTextAppearancesHelper.CancellationTokenSources.GetOrCreateCts(annot);
    try
    {
      if (await FreeTextPdfTextAppearancesHelper.TryRegenerateAppearancesWithRichTextCore(annot, freeTextRegenCts.Token).ConfigureAwait(false))
      {
        freeTextRegenCts = (CancellationTokenSource) null;
      }
      else
      {
        FreeTextPdfTextAppearancesHelper.TryRegenerateAppearancesCore(annot);
        freeTextRegenCts = (CancellationTokenSource) null;
      }
    }
    catch (OperationCanceledException ex)
    {
      freeTextRegenCts = (CancellationTokenSource) null;
    }
    finally
    {
      FreeTextAppearancesHelper.CancellationTokenSources.CancelCts(annot);
    }
  }

  private static async Task<bool> TryRegenerateAppearancesWithRichTextCoreAsync(
    PdfFreeTextAnnotation annot,
    RichTextBox rtb,
    PdfDefaultAppearance da,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (annot == null)
      return false;
    PdfDocument document = annot.Page?.Document;
    if (document == null || rtb == null)
      return false;
    await FreeTextPdfTextAppearancesHelper.ExtendFreeTextAnnotRectangleAsync(rtb, annot, cancellationToken);
    PdfBorderStyle borderStyle = annot.BorderStyle;
    float borderWidth = borderStyle != null ? borderStyle.Width : 1f;
    FS_RECTF rect = annot.GetRECT();
    PageRotate pageRotate;
    float rotate = annot.GetRotate(out PageRotate? _, out pageRotate);
    FS_POINTF anchorPoint = PdfRotateUtils.GetAnchorPoint(rect, pageRotate);
    FS_RECTF annotRect = PdfRotateUtils.GetOriginalRectangle(rect, pageRotate);
    annot.Rectangle = annotRect;
    List<PdfTextObject> textObjects = await RichTextBoxUtils.CreatePdfTextObjectsAsync(rtb, document, borderWidth, annotRect, annot.InnerRectangle, cancellationToken).ConfigureAwait(false);
    bool res = FreeTextPdfTextAppearancesHelper.TryRegenerateAppearancesCore(annot, textObjects, da, cancellationToken);
    if (res)
    {
      foreach (PdfPageObject obj in annot.NormalAppearance)
      {
        FS_MATRIX matrix = obj.Matrix;
        matrix.Translate((float) (-(double) annotRect.left - (double) annotRect.Width / 2.0), (float) (-(double) annotRect.bottom - (double) annotRect.Height / 2.0));
        PdfRotateUtils.RotateMatrix(pageRotate, matrix);
        matrix.Translate(annotRect.left + annotRect.Width / 2f, annotRect.bottom + annotRect.Height / 2f);
        obj.Matrix = matrix;
        matrix = (FS_MATRIX) null;
      }
      annot.GenerateAppearance(AppearanceStreamModes.Normal);
      FS_RECTF rect3 = rect;
      FS_POINTF anchorPoint2 = PdfRotateUtils.GetAnchorPoint(rect3, pageRotate);
      float offsetX = anchorPoint.X - anchorPoint2.X;
      float offsetY = anchorPoint.Y - anchorPoint2.Y;
      rect3.left += offsetX;
      rect3.right += offsetX;
      rect3.top += offsetY;
      rect3.bottom += offsetY;
      PdfTypeDictionary.Create(Pdfium.FPDFOBJ_GetDict(Pdfium.FPDFAnnot_GetAppearanceStream(annot.Dictionary.Handle, AppearanceStreamModes.Normal))).SetAt("BBox", (PdfTypeBase) rect3.ToArray());
      annot.Rectangle = rect3;
      rect3 = new FS_RECTF();
      anchorPoint2 = new FS_POINTF();
    }
    return res;
  }

  private static async Task<bool> TryRegenerateAppearancesWithRichTextCore(
    PdfFreeTextAnnotation annot,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (annot == null)
      return false;
    PdfDocument document = annot.Page?.Document;
    if (document == null)
      return false;
    string richText = annot.RichText;
    PdfDefaultAppearance da;
    if (!PdfDefaultAppearance.TryParse(annot.DefaultAppearance, out da))
      da = PdfDefaultAppearance.Default;
    PdfRichTextStyle defaultStyle;
    PdfRichTextStyle ds = !PdfRichTextStyle.TryParse(annot.DefaultStyle, PdfRichTextStyle.Default, out defaultStyle) ? da.ToRichTextStyle() : defaultStyle;
    PdfRichTextString pdfRichTextString;
    if (!PdfRichTextString.TryParse(richText, ds, out pdfRichTextString, annot.Name))
      return false;
    RichTextBox rtb = RichTextBoxUtils.CreateRichTextBox(annot, pdfRichTextString, da, defaultStyle);
    if (rtb == null)
      throw new ArgumentException("rtb");
    bool flag = await FreeTextPdfTextAppearancesHelper.TryRegenerateAppearancesWithRichTextCoreAsync(annot, rtb, da, cancellationToken).ConfigureAwait(false);
    return flag;
  }

  private static void TryRegenerateAppearancesCore(PdfFreeTextAnnotation annot)
  {
    if (annot == null)
      return;
    PdfDocument document = annot.Page?.Document;
    if (document == null)
      return;
    string contents = annot.Contents;
    List<PdfTextObject> textObjects = (List<PdfTextObject>) null;
    PdfDefaultAppearance pdfFontStyle = PdfDefaultAppearance.Default;
    FS_RECTF rect1 = annot.GetRECT();
    PageRotate adjustRotate;
    annot.GetRotate(out PageRotate? _, out adjustRotate);
    FS_POINTF anchorPoint1 = PdfRotateUtils.GetAnchorPoint(rect1, adjustRotate);
    FS_RECTF originalRectangle = PdfRotateUtils.GetOriginalRectangle(rect1, adjustRotate);
    if (!string.IsNullOrEmpty(contents))
    {
      if (!PdfDefaultAppearance.TryParse(annot.DefaultAppearance, out pdfFontStyle))
        pdfFontStyle = PdfDefaultAppearance.Default;
      PdfRichTextStyle richTextStyle;
      if (!PdfRichTextStyle.TryParse(annot.DefaultStyle, PdfRichTextStyle.Default, out richTextStyle))
        richTextStyle = pdfFontStyle.ToRichTextStyle();
      FS_RECTF innerRectangle = AnnotDrawingHelper.GetInnerRectangle(originalRectangle, annot.InnerRectangle);
      float padding = 0.0f;
      if (annot.Intent != AnnotationIntent.FreeTextTypeWriter)
      {
        PdfBorderStyle borderStyle = annot.BorderStyle;
        padding = borderStyle != null ? borderStyle.Width : 1f;
      }
      FS_COLOR strokeColor = pdfFontStyle.StrokeColor;
      FS_COLOR fillColor = pdfFontStyle.FillColor;
      if (fillColor == FS_COLOR.Empty)
      {
        if (strokeColor == FS_COLOR.Empty || strokeColor == FS_COLOR.Black)
          strokeColor = FS_COLOR.Transparent;
        fillColor = FS_COLOR.Black;
      }
      else if (strokeColor == FS_COLOR.Empty)
        strokeColor = FS_COLOR.Transparent;
      if (fillColor == strokeColor)
        strokeColor = FS_COLOR.Transparent;
      textObjects = AnnotDrawingHelper.PatagamesCreateTextObjects(document, contents, richTextStyle.FontFamily, (float) ((double) richTextStyle.FontSize ?? (double) PdfRichTextStyle.Default.FontSize.Value), strokeColor, fillColor, padding, annot.TextAlignment, innerRectangle);
    }
    if (!FreeTextPdfTextAppearancesHelper.TryRegenerateAppearancesCore(annot, textObjects, pdfFontStyle))
      return;
    foreach (PdfPageObject pdfPageObject in annot.NormalAppearance)
    {
      FS_MATRIX matrix = pdfPageObject.Matrix;
      matrix.Translate((float) (-(double) originalRectangle.left - (double) originalRectangle.Width / 2.0), (float) (-(double) originalRectangle.bottom - (double) originalRectangle.Height / 2.0));
      PdfRotateUtils.RotateMatrix(adjustRotate, matrix);
      matrix.Translate(originalRectangle.left + originalRectangle.Width / 2f, originalRectangle.bottom + originalRectangle.Height / 2f);
      pdfPageObject.Matrix = matrix;
    }
    annot.GenerateAppearance(AppearanceStreamModes.Normal);
    FS_RECTF rect2 = rect1;
    FS_POINTF anchorPoint2 = PdfRotateUtils.GetAnchorPoint(rect2, adjustRotate);
    float num1 = anchorPoint1.X - anchorPoint2.X;
    float num2 = anchorPoint1.Y - anchorPoint2.Y;
    rect2.left += num1;
    rect2.right += num1;
    rect2.top += num2;
    rect2.bottom += num2;
    PdfTypeDictionary.Create(Pdfium.FPDFOBJ_GetDict(Pdfium.FPDFAnnot_GetAppearanceStream(annot.Dictionary.Handle, AppearanceStreamModes.Normal))).SetAt("BBox", (PdfTypeBase) rect2.ToArray());
    annot.Rectangle = rect2;
  }

  private static async Task ExtendFreeTextAnnotRectangleAsync(
    RichTextBox rtb,
    PdfFreeTextAnnotation annot,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (rtb == null)
      throw new ArgumentNullException(nameof (rtb));
    if (annot == null)
      throw new ArgumentNullException(nameof (annot));
    if (double.IsNaN(rtb.Width))
      rtb.Width = 1.0;
    if (double.IsNaN(rtb.Height))
      rtb.Height = 1.0;
    await RichTextBoxUtils.ExtendRichTextBoxAsync(rtb, annot.Intent == AnnotationIntent.FreeTextTypeWriter);
    PdfBorderStyle borderStyle = annot.BorderStyle;
    float borderWidth = borderStyle != null ? borderStyle.Width : 1f;
    FS_RECTF annotRect = annot.GetRECT();
    FS_RECTF textRectangle = new FS_RECTF(annotRect.left + borderWidth, annotRect.top - borderWidth, annotRect.right - borderWidth, annotRect.bottom + borderWidth);
    float width = (float) ((double) textRectangle.Width * 96.0 / 72.0);
    float height = (float) ((double) textRectangle.Height * 96.0 / 72.0);
    PageRotate pageRotate;
    float rotate = annot.GetRotate(out PageRotate? _, out pageRotate);
    FS_POINTF anchorPoint = PdfRotateUtils.GetAnchorPoint(annotRect, pageRotate);
    if (pageRotate == PageRotate.Rotate90 || pageRotate == PageRotate.Rotate270)
    {
      float num = height;
      height = width;
      width = num;
    }
    if (rtb.ActualWidth > (double) width || rtb.ActualHeight > (double) height || annot.Intent == AnnotationIntent.FreeTextTypeWriter)
    {
      if (rtb.ActualWidth > (double) width)
      {
        double diffWidth = rtb.ActualWidth - (double) width;
        textRectangle.right += (float) (diffWidth * 72.0 / 96.0);
        annotRect.right = textRectangle.right + borderWidth;
      }
      if (rtb.ActualHeight > (double) height || annot.Intent == AnnotationIntent.FreeTextTypeWriter)
      {
        double diffHeight = rtb.ActualHeight - (double) height;
        textRectangle.bottom -= (float) (diffHeight * 72.0 / 96.0);
        annotRect.bottom = textRectangle.bottom - borderWidth;
      }
    }
    FS_POINTF anchorPoint2 = PdfRotateUtils.GetAnchorPoint(annotRect, pageRotate);
    float offsetX = anchorPoint.X - anchorPoint2.X;
    float offsetY = anchorPoint.Y - anchorPoint2.Y;
    annotRect.left += offsetX;
    annotRect.right += offsetX;
    annotRect.top += offsetY;
    annotRect.bottom += offsetY;
    annot.Rectangle = annotRect;
  }

  private static bool TryRegenerateAppearancesCore(
    PdfFreeTextAnnotation annot,
    List<PdfTextObject> textObjects,
    PdfDefaultAppearance da,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (annot == null)
      return false;
    if (annot.Page?.Document == null)
      return false;
    try
    {
      PdfBorderStyle borderStyle1 = annot.BorderStyle;
      float num = borderStyle1 != null ? borderStyle1.Width : 1f;
      annot.NormalAppearance?.Clear();
      annot.CreateEmptyAppearance(AppearanceStreamModes.Normal);
      List<PdfPathObject> pdfPathObjectList = (List<PdfPathObject>) null;
      FS_RECTF innerRectangle1 = AnnotDrawingHelper.GetInnerRectangle(annot.GetRECT(), annot.InnerRectangle);
      FS_COLOR fsColor1;
      if (annot.Color == FS_COLOR.Empty)
      {
        fsColor1 = new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      else
      {
        ref FS_COLOR local = ref fsColor1;
        int a = (int) ((double) annot.Opacity * (double) byte.MaxValue);
        FS_COLOR color = annot.Color;
        int r = color.R;
        color = annot.Color;
        int g = color.G;
        int b = annot.Color.B;
        local = new FS_COLOR(a, r, g, b);
      }
      FS_COLOR fsColor2;
      if (da.StrokeColor == FS_COLOR.Empty)
      {
        fsColor2 = new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      }
      else
      {
        ref FS_COLOR local = ref fsColor2;
        int a = (int) ((double) annot.Opacity * (double) byte.MaxValue);
        FS_COLOR strokeColor = da.StrokeColor;
        int r = strokeColor.R;
        strokeColor = da.StrokeColor;
        int g = strokeColor.G;
        int b = da.StrokeColor.B;
        local = new FS_COLOR(a, r, g, b);
      }
      if (annot.Intent == AnnotationIntent.FreeTextCallout)
      {
        FS_COLOR fsColor3;
        ref FS_COLOR local = ref fsColor3;
        int a = (int) ((double) annot.Opacity * (double) byte.MaxValue);
        FS_COLOR color = annot.Color;
        int r = color.R;
        color = annot.Color;
        int g = color.G;
        int b = annot.Color.B;
        local = new FS_COLOR(a, r, g, b);
        FS_COLOR strokeColor = fsColor1;
        FS_COLOR fillColor = fsColor3;
        PdfLinePointCollection<PdfFreeTextAnnotation> calloutLine = annot.CalloutLine;
        PdfLineEndingCollection lineEnding = annot.LineEnding;
        double lineWidth = (double) num;
        float[] dashPattern = annot.BorderStyle?.DashPattern;
        PdfBorderStyle borderStyle2 = annot.BorderStyle;
        int style = borderStyle2 != null ? (int) borderStyle2.Style : 0;
        pdfPathObjectList = AnnotDrawingHelper.CreateCalloutLines(strokeColor, fillColor, (IList<FS_POINTF>) calloutLine, LineJoin.Round, lineEnding, (float) lineWidth, dashPattern, (BorderStyles) style);
      }
      if (annot.Intent != AnnotationIntent.FreeTextTypeWriter)
      {
        FS_COLOR strokeColor = fsColor2;
        FS_COLOR fillColor = fsColor1;
        double borderWidth = (double) num;
        float[] dashPattern = annot.BorderStyle?.DashPattern;
        PdfBorderStyle borderStyle3 = annot.BorderStyle;
        int style = borderStyle3 != null ? (int) borderStyle3.Style : 0;
        PdfBorderEffect borderEffect1 = annot.BorderEffect;
        int effect = borderEffect1 != null ? (int) borderEffect1.Effect : 0;
        PdfBorderEffect borderEffect2 = annot.BorderEffect;
        int intensity = borderEffect2 != null ? borderEffect2.Intensity : 0;
        FS_RECTF rect = innerRectangle1;
        foreach (PdfPathObject pdfPathObject in AnnotDrawingHelper.CreateSquare(strokeColor, fillColor, (float) borderWidth, dashPattern, (BorderStyles) style, (BorderEffects) effect, intensity, rect))
          annot.NormalAppearance.Add((PdfPageObject) pdfPathObject);
      }
      if (textObjects != null)
      {
        foreach (PdfTextObject textObject in textObjects)
          annot.NormalAppearance.Add((PdfPageObject) textObject);
      }
      if (pdfPathObjectList != null)
      {
        foreach (PdfPathObject pdfPathObject in pdfPathObjectList)
          annot.NormalAppearance.Add((PdfPageObject) pdfPathObject);
      }
      FS_RECTF rect1 = annot.GetRECT();
      float[] innerRectangle2 = annot.InnerRectangle;
      annot.GenerateAppearance(AppearanceStreamModes.Normal);
      if (pdfPathObjectList != null && pdfPathObjectList.Count > 0)
      {
        float[] innerRectangle3 = annot.InnerRectangle;
        if (innerRectangle3 != null)
        {
          FS_RECTF rect2 = annot.GetRECT();
          if (innerRectangle3.Length != 0)
            innerRectangle3[0] = rect1.left + innerRectangle3[0] - rect2.left;
          if (innerRectangle3.Length > 1)
            innerRectangle3[1] = rect2.top - rect1.top + innerRectangle3[1];
          if (innerRectangle3.Length > 2)
            innerRectangle3[2] = rect2.right - rect1.right + innerRectangle3[2];
          if (innerRectangle3.Length > 3)
            innerRectangle3[3] = rect1.bottom + innerRectangle3[3] - rect2.bottom;
          annot.InnerRectangle = innerRectangle3;
        }
      }
      else
      {
        PdfTypeDictionary.Create(Pdfium.FPDFOBJ_GetDict(Pdfium.FPDFAnnot_GetAppearanceStream(annot.Dictionary.Handle, AppearanceStreamModes.Normal))).SetAt("BBox", (PdfTypeBase) rect1.ToArray());
        annot.Rectangle = rect1;
      }
      return true;
    }
    catch
    {
    }
    return false;
  }
}
