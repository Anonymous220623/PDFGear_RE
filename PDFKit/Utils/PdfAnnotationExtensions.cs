// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfAnnotationExtensions
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using PDFKit.Utils.FreeTextUtils;
using PDFKit.Utils.PageContents;
using PDFKit.Utils.StampUtils;
using PDFKit.Utils.WatermarkUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

#nullable disable
namespace PDFKit.Utils;

public static class PdfAnnotationExtensions
{
  private static HashSet<string> useFreeTextPathHelperLanguages = new HashSet<string>()
  {
    "ja"
  };
  private static IFreeTextAppearancesHelper freeTextPdfTextHelper = (IFreeTextAppearancesHelper) new FreeTextPdfTextAppearancesHelper();
  private static IFreeTextAppearancesHelper freeTextPathHelper = (IFreeTextAppearancesHelper) new FreeTextPathAppearancesHelper();

  public static async Task WaitForAnnotationGenerateAsync()
  {
    await FreeTextAppearancesHelper.WaitForAnnotationGenerateAsync();
  }

  public static void RegenerateAppearancesAdvance(this PdfTextAnnotation text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (text.Relationship == RelationTypes.Reply)
    {
      if (text.NormalAppearance == null)
        return;
      List<PdfPageObject> list = text.NormalAppearance.ToList<PdfPageObject>();
      text.NormalAppearance.Clear();
      foreach (IDisposable disposable in list.OfType<IDisposable>())
        disposable.Dispose();
      text.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    }
    else
      text.RegenerateAppearances();
  }

  public static void RegenerateAppearancesWithoutRound(this PdfHighlightAnnotation highlight)
  {
    if (highlight == null)
      throw new ArgumentNullException(nameof (highlight));
    highlight.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    highlight.NormalAppearance?.Clear();
    FS_COLOR fillColor;
    ref FS_COLOR local = ref fillColor;
    int a = (int) ((double) highlight.Opacity * (double) byte.MaxValue);
    int r = highlight.Color.R;
    FS_COLOR color = highlight.Color;
    int g = color.G;
    color = highlight.Color;
    int b = color.B;
    local = new FS_COLOR(a, r, g, b);
    foreach (PdfPathObject pdfPathObject in CreateHighlight(fillColor, highlight.QuadPoints))
      highlight.NormalAppearance.Add((PdfPageObject) pdfPathObject);
    highlight.GenerateAppearance(AppearanceStreamModes.Normal);

    static List<PdfPathObject> CreateHighlight(
      FS_COLOR fillColor,
      PdfQuadPointsCollection quadPoints)
    {
      List<PdfPathObject> highlight = new List<PdfPathObject>();
      foreach (FS_QUADPOINTSF quadPoint in quadPoints)
      {
        PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.Winding, false);
        foreach (FS_PATHPOINTF highlightPoint in GetHighlightPoints(quadPoint))
          pdfPathObject.Path.Add(highlightPoint);
        pdfPathObject.FillColor = fillColor;
        Pdfium.FPDFPageObj_SetBlendMode(pdfPathObject.Handle, BlendTypes.FXDIB_BLEND_MULTIPLY);
        pdfPathObject.CalcBoundingBox();
        highlight.Add(pdfPathObject);
      }
      return highlight;
    }

    static List<FS_PATHPOINTF> GetHighlightPoints(FS_QUADPOINTSF qp)
    {
      return new List<FS_PATHPOINTF>()
      {
        new FS_PATHPOINTF(qp.x1, qp.y1, PathPointFlags.MoveTo),
        new FS_PATHPOINTF(qp.x2, qp.y2, PathPointFlags.LineTo),
        new FS_PATHPOINTF(qp.x4, qp.y3, PathPointFlags.LineTo),
        new FS_PATHPOINTF(qp.x3, qp.y4, PathPointFlags.LineTo),
        new FS_PATHPOINTF(qp.x1, qp.y1, PathPointFlags.CloseFigure)
      };
    }
  }

  public static void RegenerateWatermarkAppeaances(
    this PdfWatermarkAnnotation annot,
    WatermarkParam watermarkparam)
  {
    WatermarkUtil.GenerateAppearance(annot, watermarkparam);
  }

  public static void RegenerateAppearancesWithRichText(this PdfFreeTextAnnotation annot)
  {
    PdfAnnotationExtensions.FreeTextHelper.RegenerateAppearancesWithRichText(annot, true);
  }

  internal static void RegenerateAppearancesWithRichText(
    this PdfFreeTextAnnotation annot,
    bool force)
  {
    PdfAnnotationExtensions.FreeTextHelper.RegenerateAppearancesWithRichText(annot, false);
  }

  public static async Task RegenerateAppearancesWithRichTextAsync(
    this PdfFreeTextAnnotation annot,
    RichTextBox rtb)
  {
    await PdfAnnotationExtensions.FreeTextHelper.RegenerateAppearancesWithRichTextAsync(annot, rtb).ConfigureAwait(false);
  }

  public static async Task RegenerateAppearancesWithRichTextAsync(this PdfFreeTextAnnotation annot)
  {
    await PdfAnnotationExtensions.FreeTextHelper.RegenerateAppearancesWithRichTextAsync(annot).ConfigureAwait(false);
  }

  internal static bool FlattenAnnotation(PdfAnnotation annot)
  {
    return PdfAnnotationExtensions.FlattenAnnotations(annot?.Page, (System.Collections.Generic.IReadOnlyList<PdfAnnotation>) new PdfAnnotation[1]
    {
      annot
    });
  }

  internal static bool FlattenAnnotations(PdfPage page, System.Collections.Generic.IReadOnlyList<PdfAnnotation> annots)
  {
    if (page == null || annots == null || annots.Count == 0 || annots.Any<PdfAnnotation>((Func<PdfAnnotation, bool>) (c => c?.Page != page)))
      return false;
    for (int index = 0; index < annots.Count; ++index)
    {
      PdfAnnotationExtensions.FlattenCore(annots[index]);
      page.Annots.Remove(annots[index]);
    }
    page.GenerateContentAdvance();
    return true;
  }

  public static bool FlattenPage(PdfPage page)
  {
    return PdfAnnotationExtensions.FlattenAnnotations(page, (System.Collections.Generic.IReadOnlyList<PdfAnnotation>) page.Annots.ToList<PdfAnnotation>());
  }

  private static bool FlattenCore(PdfAnnotation annot)
  {
    if (annot?.Page == null)
      return false;
    PdfPage page = annot.Page;
    PdfFormObject formObject = PdfAnnotationExtensions.ConvertAnnotationToFormObject(annot, AppearanceStreamModes.Normal);
    if (formObject == null)
      return false;
    int count = page.PageObjects.Count;
    page.PageObjects.Insert(count, (PdfPageObject) formObject);
    return true;
  }

  private static PdfPageObjectsCollection GetAppearance(
    PdfAnnotation annot,
    AppearanceStreamModes mode)
  {
    switch (mode)
    {
      case AppearanceStreamModes.Normal:
        return annot.NormalAppearance;
      case AppearanceStreamModes.Rollover:
        return annot.RolloverAppearance;
      case AppearanceStreamModes.Down:
        return annot.DownAppearance;
      default:
        return (PdfPageObjectsCollection) null;
    }
  }

  private static void GetAppearanceMatrix(
    PdfAnnotation annot,
    AppearanceStreamModes mode,
    ref FS_RECTF appearanceBBox,
    out FS_MATRIX appearanceMatrix)
  {
    appearanceBBox = new FS_RECTF();
    appearanceMatrix = (FS_MATRIX) null;
    string key = "";
    if (mode == AppearanceStreamModes.Normal)
      key = "N";
    if (mode == AppearanceStreamModes.Down)
      key = "D";
    if (mode == AppearanceStreamModes.Rollover)
      key = "R";
    PdfTypeStream pdfTypeStream;
    int num;
    if (!string.IsNullOrEmpty(key) && annot.Dictionary.ContainsKey("AP") && annot.Dictionary["AP"].Is<PdfTypeDictionary>())
    {
      PdfTypeDictionary pdfTypeDictionary = annot.Dictionary["AP"].As<PdfTypeDictionary>();
      if (pdfTypeDictionary != null && pdfTypeDictionary.ContainsKey(key) && pdfTypeDictionary[key].Is<PdfTypeStream>())
      {
        pdfTypeStream = pdfTypeDictionary[key].As<PdfTypeStream>();
        num = pdfTypeStream != null ? 1 : 0;
        goto label_10;
      }
    }
    num = 0;
label_10:
    if (num == 0)
      return;
    if (pdfTypeStream.Dictionary.ContainsKey("BBox") && pdfTypeStream.Dictionary["BBox"].Is<PdfTypeArray>())
      appearanceBBox = new FS_RECTF((PdfTypeBase) pdfTypeStream.Dictionary["BBox"].As<PdfTypeArray>());
    if (pdfTypeStream.Dictionary.ContainsKey("Matrix") && pdfTypeStream.Dictionary["Matrix"].Is<PdfTypeArray>())
    {
      appearanceMatrix = new FS_MATRIX((PdfTypeBase) pdfTypeStream.Dictionary["Matrix"].As<PdfTypeArray>());
      if (appearanceMatrix.IsIdentity())
        appearanceMatrix = (FS_MATRIX) null;
    }
  }

  private static PdfFormObject ConvertAnnotationToFormObject(
    PdfAnnotation annot,
    AppearanceStreamModes mode)
  {
    if (annot?.Page == null)
      return (PdfFormObject) null;
    PdfPage page = annot.Page;
    PdfPageObjectsCollection appearance = PdfAnnotationExtensions.GetAppearance(annot, mode);
    if (appearance == null || appearance.Count == 0)
      return (PdfFormObject) null;
    PdfFormObject formObject = PdfFormObject.Create(page);
    if (!Pdfium.FPDF_GenerateContentToStream(page.Document.Handle, appearance.Handle, formObject.Stream.Handle, IntPtr.Zero) || formObject.IsParsed || !Pdfium.FPDFFormContent_Parse(Pdfium.FPDFFormObj_GetFormContent(formObject.Handle)))
      return (PdfFormObject) null;
    FS_RECTF rect = annot.GetRECT();
    FS_RECTF appearanceBBox = rect;
    FS_MATRIX matrix = (FS_MATRIX) null;
    FS_MATRIX appearanceMatrix;
    PdfAnnotationExtensions.GetAppearanceMatrix(annot, mode, ref appearanceBBox, out appearanceMatrix);
    if (appearanceBBox != rect && (double) rect.Width > 0.0 && (double) rect.Height > 0.0)
    {
      matrix = new FS_MATRIX();
      matrix.SetIdentity();
      matrix.Translate(-appearanceBBox.left, -appearanceBBox.bottom);
      matrix.Scale(rect.Width / appearanceBBox.Width, rect.Height / appearanceBBox.Height);
      matrix.Translate(rect.left, rect.bottom);
    }
    for (int index = 0; index < formObject.PageObjects.Count; ++index)
    {
      IntPtr num = Pdfium.FPDFPageObjectList_GetObject(formObject.PageObjects.Handle, index);
      PageObjectTypes type = Pdfium.FPDFPageObj_GetType(num);
      Pdfium.FPDFPageObj_GetBBox(num, (FS_MATRIX) null, out float _, out float _, out float _, out float _);
      FS_MATRIX fsMatrix = (FS_MATRIX) null;
      switch (type)
      {
        case PageObjectTypes.PDFPAGE_TEXT:
          fsMatrix = Pdfium.FPDFTextObj_GetTextMatrix(num);
          break;
        case PageObjectTypes.PDFPAGE_PATH:
          fsMatrix = Pdfium.FPDFPathObj_GetMatrix(num);
          break;
        case PageObjectTypes.PDFPAGE_IMAGE:
          fsMatrix = Pdfium.FPDFImageObj_GetMatrix(num);
          break;
        case PageObjectTypes.PDFPAGE_SHADING:
          fsMatrix = Pdfium.FPDFShadingObj_GetMatrix(num);
          break;
        case PageObjectTypes.PDFPAGE_FORM:
          fsMatrix = Pdfium.FPDFFormObj_GetFormMatrix(num);
          break;
      }
      if (fsMatrix != null)
      {
        if (appearanceMatrix != null)
          fsMatrix.Concat(appearanceMatrix);
        if (matrix != null)
          fsMatrix.Concat(matrix);
      }
      switch (type)
      {
        case PageObjectTypes.PDFPAGE_TEXT:
          Pdfium.FPDFTextObj_SetTextMatrix(num, fsMatrix);
          break;
        case PageObjectTypes.PDFPAGE_PATH:
          Pdfium.FPDFPathObj_SetMatrix(num, fsMatrix);
          break;
        case PageObjectTypes.PDFPAGE_IMAGE:
          Pdfium.FPDFImageObj_SetMatrix(num, fsMatrix);
          Pdfium.FPDFPageObj_RemoveClipPath(num);
          break;
        case PageObjectTypes.PDFPAGE_SHADING:
          Pdfium.FPDFShadingObj_SetMatrix(num, fsMatrix);
          break;
        case PageObjectTypes.PDFPAGE_FORM:
          Pdfium.FPDFFormObj_SetFormMatrix(num, fsMatrix);
          break;
      }
    }
    formObject.CalcBoundingBox();
    return formObject;
  }

  private static IFreeTextAppearancesHelper FreeTextHelper
  {
    get
    {
      return !PdfAnnotationExtensions.useFreeTextPathHelperLanguages.Contains(Common.ActualLanguage) ? PdfAnnotationExtensions.freeTextPdfTextHelper : PdfAnnotationExtensions.freeTextPathHelper;
    }
  }

  public static void RegenerateAppearancesAdvance(this PdfStampAnnotation annot)
  {
    StampUtil.GenerateAppearance(annot);
  }
}
