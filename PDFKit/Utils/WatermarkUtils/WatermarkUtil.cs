// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.WatermarkUtils.WatermarkUtil
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

#nullable disable
namespace PDFKit.Utils.WatermarkUtils;

public static class WatermarkUtil
{
  private static PdfDocument internalDoc;
  private static ConcurrentDictionary<string, PdfFont> cachedFonts;

  private static bool IsPdfXTextWatermark(PdfWatermarkAnnotation watermark)
  {
    if ((PdfWrapper) watermark == (PdfWrapper) null || watermark.NormalAppearance == null || watermark.NormalAppearance.Count == 0)
      return false;
    PdfPageObject[] array = watermark.NormalAppearance.ToArray<PdfPageObject>();
    return array.Length == 1 && array[0].ObjectType == PageObjectTypes.PDFPAGE_TEXT;
  }

  private static bool IsPdfXImageWatermark(PdfWatermarkAnnotation watermark)
  {
    if ((PdfWrapper) watermark == (PdfWrapper) null || watermark.NormalAppearance == null || watermark.NormalAppearance.Count == 0)
      return false;
    PdfPageObject[] array = watermark.NormalAppearance.ToArray<PdfPageObject>();
    return array.Length == 1 && array[0].ObjectType == PageObjectTypes.PDFPAGE_IMAGE;
  }

  private static string GetWatermarkTextContent(PdfWatermarkAnnotation watermark)
  {
    if ((PdfWrapper) watermark == (PdfWrapper) null)
      return "";
    if (WatermarkUtil.IsPdfXTextWatermark(watermark))
      return watermark.NormalAppearance.OfType<PdfTextObject>().FirstOrDefault<PdfTextObject>()?.TextUnicode ?? "";
    return watermark.NormalAppearance != null ? watermark.NormalAppearance.Where<PdfPageObject>((Func<PdfPageObject, bool>) (x => x.ObjectType == PageObjectTypes.PDFPAGE_TEXT)).OfType<PdfTextObject>().Aggregate<PdfTextObject, StringBuilder>(new StringBuilder(), (Func<StringBuilder, PdfTextObject, StringBuilder>) ((x, y) => x.Append(y.TextUnicode))).ToString() ?? "" : "";
  }

  public static List<PdfTextObject> CreateWatermarkTextObjects(
    PdfDocument doc,
    string text,
    FS_COLOR color,
    PdfFont font,
    float fontSize)
  {
    return WatermarkUtil.CreateWatermarkTextObjects(doc, text, color, font, fontSize, out FS_RECTF _);
  }

  public static List<PdfTextObject> CreateWatermarkTextObjects(
    PdfDocument doc,
    string text,
    FS_COLOR color,
    PdfFont font,
    float fontSize,
    out FS_RECTF bounds)
  {
    bounds = new FS_RECTF();
    System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> withFallbackFonts = PdfFontUtils.GetTextWithFallbackFonts(text, font.BaseFontName, fontSize);
    List<PdfTextObject> watermarkTextObjects = new List<PdfTextObject>();
    float y = withFallbackFonts.Where<TextWithFallbackFontFamily>((Func<TextWithFallbackFontFamily, bool>) (c => !string.IsNullOrEmpty(c.Text))).Select<TextWithFallbackFontFamily, float>((Func<TextWithFallbackFontFamily, float>) (c => c.Baseline)).DefaultIfEmpty<float>().Max();
    float num1 = float.MaxValue;
    float num2 = float.MinValue;
    float num3 = float.MinValue;
    float num4 = float.MaxValue;
    foreach (TextWithFallbackFontFamily fallbackFontFamily in (IEnumerable<TextWithFallbackFontFamily>) withFallbackFonts)
    {
      PdfFont font1 = font;
      if (fallbackFontFamily.FallbackFontFamily != null)
        font1 = PdfFontUtils.CreateFont(doc, fallbackFontFamily.FallbackFontFamily, fallbackFontFamily.FontWeight, fallbackFontFamily.FontStyle, fallbackFontFamily.CharSet);
      PdfTextObject pdfTextObject = PdfTextObject.Create(fallbackFontFamily.Text, (float) fallbackFontFamily.Bounds.Left, y, font1, fontSize);
      pdfTextObject.FillColor = color;
      watermarkTextObjects.Add(pdfTextObject);
      float left;
      float top;
      float right;
      float bottom;
      Pdfium.FPDFPageObj_GetBBox(pdfTextObject.Handle, (FS_MATRIX) null, out left, out top, out right, out bottom);
      num1 = Math.Min(left, num1);
      num2 = Math.Max(top, num2);
      num3 = Math.Max(right, num3);
      num4 = Math.Min(bottom, num4);
    }
    bounds = new FS_RECTF(num1, num2, num3, num4);
    return watermarkTextObjects;
  }

  public static System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> CreateWatermarkTextFonts(
    string text,
    FS_COLOR color,
    string fontName,
    float fontSize,
    out FS_RECTF bounds,
    out float bottomBaseline)
  {
    bounds = new FS_RECTF();
    System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> withFallbackFonts = PdfFontUtils.GetTextWithFallbackFonts(text, fontName, fontSize, cultureInfo: CultureInfo.CurrentCulture);
    List<PdfTextObject> pdfTextObjectList = new List<PdfTextObject>();
    bottomBaseline = withFallbackFonts.Where<TextWithFallbackFontFamily>((Func<TextWithFallbackFontFamily, bool>) (c => !string.IsNullOrEmpty(c.Text))).Select<TextWithFallbackFontFamily, float>((Func<TextWithFallbackFontFamily, float>) (c => c.Baseline)).DefaultIfEmpty<float>().Max();
    double num1 = double.MaxValue;
    double num2 = double.MaxValue;
    double num3 = double.MinValue;
    double num4 = double.MinValue;
    foreach (TextWithFallbackFontFamily fallbackFontFamily in (IEnumerable<TextWithFallbackFontFamily>) withFallbackFonts)
    {
      Rect bounds1 = fallbackFontFamily.Bounds;
      num1 = Math.Min(bounds1.Left, num1);
      bounds1 = fallbackFontFamily.Bounds;
      num2 = Math.Min(bounds1.Top, num2);
      bounds1 = fallbackFontFamily.Bounds;
      num3 = Math.Max(bounds1.Right, num3);
      bounds1 = fallbackFontFamily.Bounds;
      num4 = Math.Max(bounds1.Bottom, num4);
    }
    bounds = new FS_RECTF(num1, num4, num3, num2);
    return withFallbackFonts;
  }

  public static void RegenerateWatermarkTextAppearance(
    PdfWatermarkAnnotation annot,
    float opacity,
    bool showOnPrint,
    bool showOnDisplay,
    float vDistance,
    float hDistance,
    float rotation,
    PdfContentAlignment alignment)
  {
    string text = !((PdfWrapper) annot == (PdfWrapper) null) ? WatermarkUtil.GetWatermarkTextContent(annot) : throw new ArgumentNullException(nameof (annot));
    PdfTextObject pageObject = annot.NormalAppearance.OfType<PdfTextObject>().FirstOrDefault<PdfTextObject>();
    annot.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    if (showOnPrint)
      annot.Flags = AnnotationFlags.Print;
    if (!showOnDisplay)
      annot.Flags |= AnnotationFlags.NoView;
    annot.GetRECT();
    FS_COLOR color1 = annot.Color;
    FS_COLOR color2 = new FS_COLOR((int) ((double) color1.A * (double) opacity), color1.R, color1.G, color1.B);
    PdfFont stock = PdfFont.CreateStock(annot.Page.Document, FontStockNames.Arial);
    annot.GetRECT();
    annot.Contents = text;
    FS_MATRIX fsMatrix = pageObject.Matrix ?? new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    fsMatrix.Rotate((float) ((double) rotation * 3.1400001049041748 / 180.0));
    pageObject.Matrix = fsMatrix;
    foreach (PdfTextObject watermarkTextObject in WatermarkUtil.CreateWatermarkTextObjects(annot.Page.Document, text, color2, stock, 14f))
      annot.NormalAppearance.Add((PdfPageObject) watermarkTextObject);
    WatermarkUtil.SetRectangle((PdfPageObject) pageObject, alignment, annot, vDistance, hDistance);
    annot.GenerateAppearance(AppearanceStreamModes.Normal);
  }

  public static void RegenerateWatermarkImageAppearance(
    PdfWatermarkAnnotation annot,
    float opacity,
    bool showOnPrint,
    bool showOnDisplay,
    float vDistance,
    float hDistance,
    float rotation,
    PdfContentAlignment alignment,
    float scale = 1f)
  {
    if ((PdfWrapper) annot == (PdfWrapper) null)
      throw new ArgumentNullException(nameof (annot));
    if (annot.NormalAppearance != null)
    {
      PdfPageObject[] array = annot.NormalAppearance.ToArray<PdfPageObject>();
      annot.NormalAppearance?.Clear();
      foreach (IDisposable disposable in array.OfType<IDisposable>())
        disposable.Dispose();
    }
    if (showOnPrint)
      annot.Flags = AnnotationFlags.Print;
    if (!showOnDisplay)
      annot.Flags |= AnnotationFlags.NoView;
    annot.GetRECT();
    PdfImageObject pageObject = (PdfImageObject) annot.NormalAppearance.OfType<PdfImageObject>().FirstOrDefault<PdfImageObject>().Clone();
    annot.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    FS_MATRIX fsMatrix = pageObject.Matrix ?? new FS_MATRIX((float) pageObject.Bitmap.Width, 0.0f, 0.0f, (float) pageObject.Bitmap.Height, 0.0f, 0.0f);
    fsMatrix.Scale(scale, scale);
    fsMatrix.Rotate((float) ((double) rotation * 3.1400001049041748 / 180.0));
    pageObject.Matrix = fsMatrix;
    WatermarkUtil.SetRectangle((PdfPageObject) pageObject, alignment, annot, vDistance, hDistance);
    annot.NormalAppearance.Add((PdfPageObject) pageObject);
    annot.GenerateAppearance(AppearanceStreamModes.Normal);
  }

  private static void SetRectangle(
    PdfPageObject pageObject,
    PdfContentAlignment alignment,
    PdfWatermarkAnnotation annot,
    float vDistance,
    float hDistance)
  {
    float width = annot.Page.Width;
    float height = annot.Page.Height;
    float left;
    float top;
    float right;
    float bottom;
    Pdfium.FPDFPageObj_GetBBox(pageObject.Handle, (FS_MATRIX) null, out left, out top, out right, out bottom);
    int num1 = 0;
    int num2 = 0;
    switch (alignment)
    {
      case PdfContentAlignment.TopLeft:
        num2 = 1;
        num1 = 0;
        break;
      case PdfContentAlignment.TopCenter:
        num2 = 1;
        num1 = 2;
        break;
      case PdfContentAlignment.TopRight:
        num2 = 1;
        num1 = 1;
        break;
      case PdfContentAlignment.MiddleLeft:
        num2 = 2;
        num1 = 0;
        break;
      case PdfContentAlignment.MiddleCenter:
        num2 = 2;
        num1 = 2;
        break;
      case PdfContentAlignment.MiddleRight:
        num2 = 2;
        num1 = 1;
        break;
      case PdfContentAlignment.BottomLeft:
        num2 = 0;
        num1 = 0;
        break;
      case PdfContentAlignment.BottomCenter:
        num2 = 0;
        num1 = 2;
        break;
      case PdfContentAlignment.BottomRight:
        num2 = 0;
        num1 = 1;
        break;
    }
    float num3 = num1 == 0 ? 0.0f : (width - right + left) / (float) num1;
    float num4 = num2 == 0 ? 0.0f : (height - top + bottom) / (float) num2;
    float l = num3 + hDistance;
    float b = num4 + vDistance;
    annot.Rectangle = new FS_RECTF(l, b + (top - bottom), l + (right - left), b);
  }

  internal static void GenerateAppearance(PdfWatermarkAnnotation annot, WatermarkParam param)
  {
    if (annot == null)
      throw new ArgumentNullException(nameof (annot));
    if (WatermarkUtil.IsPdfXTextWatermark(annot))
      WatermarkUtil.RegenerateWatermarkTextAppearance(annot, param.Opacity, param.ShowOnPrint, param.ShowOnDisplay, param.Vdistance, param.Hdistance, param.Rotation, param.Alignment);
    if (!WatermarkUtil.IsPdfXImageWatermark(annot))
      return;
    WatermarkUtil.RegenerateWatermarkImageAppearance(annot, param.Opacity, param.ShowOnPrint, param.ShowOnDisplay, param.Vdistance, param.Hdistance, param.Rotation, param.Alignment);
  }
}
