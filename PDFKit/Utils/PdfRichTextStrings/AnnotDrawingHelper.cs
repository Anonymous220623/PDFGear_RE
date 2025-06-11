// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.AnnotDrawingHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

internal static class AnnotDrawingHelper
{
  private static readonly Type annotDrawingType;
  public const float roundRectCornerK = 5f;
  private static Func<FS_COLOR, FS_COLOR, IList<FS_POINTF>, LineJoin, PdfLineEndingCollection, float, float[], BorderStyles, List<PdfPathObject>> createCalloutLinesFunc;
  private static Func<string, FS_COLOR, FS_RECTF, PdfFont, List<PdfPageObject>> createRuberStampFunc;

  static AnnotDrawingHelper()
  {
    try
    {
      AnnotDrawingHelper.annotDrawingType = typeof (Pdfium).Assembly.GetType("Patagames.Pdf.AnnotDrawing");
    }
    catch
    {
    }
  }

  public static System.Collections.Generic.IReadOnlyList<PdfTextObject> CreatePdfTextObjects(
    PdfDocument document,
    Run run,
    string text,
    Rect textBounds,
    double baseline,
    float padding,
    FS_RECTF annotRect)
  {
    float l = annotRect.left + (float) (textBounds.Left / 96.0 * 72.0) + padding;
    float t = annotRect.top - (float) (textBounds.Top / 96.0 * 72.0) - padding;
    float num1 = (float) (textBounds.Width / 96.0 * 72.0);
    float num2 = (float) (textBounds.Height / 96.0 * 72.0);
    FS_RECTF rect = new FS_RECTF(l, t, l + num1, t - num2);
    Color? nullable = run.Foreground is SolidColorBrush foreground ? new Color?(foreground.Color) : new Color?();
    FS_COLOR black = FS_COLOR.Black;
    if (nullable.HasValue)
    {
      ref FS_COLOR local = ref black;
      Color color = nullable.Value;
      int a = (int) color.A;
      color = nullable.Value;
      int r = (int) color.R;
      color = nullable.Value;
      int g = (int) color.G;
      color = nullable.Value;
      int b = (int) color.B;
      local = new FS_COLOR(a, r, g, b);
    }
    float baseline1 = (float) (baseline / 96.0 * 72.0);
    return AnnotDrawingHelper.CreateTextObjects(document, text, run.FontFamily, run.FontWeight, (float) (run.FontSize / 96.0 * 72.0), new FS_COLOR(0), black, rect, baseline1);
  }

  private static System.Collections.Generic.IReadOnlyList<PdfTextObject> CreateTextObjects(
    PdfDocument doc,
    string text,
    FontFamily fontFamily,
    System.Windows.FontWeight fontWeight,
    float fontSize,
    FS_COLOR strokeColor,
    FS_COLOR fillColor,
    FS_RECTF rect,
    float baseline)
  {
    if (string.IsNullOrEmpty(text))
      return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null;
    float posY = rect.top - baseline;
    List<PdfTextObject> list = PdfFontUtils.GetTextWithFallbackFonts(text, fontFamily.Source, fontSize, new FS_RECTF?(rect), new System.Windows.FontWeight?(fontWeight)).Select<TextWithFallbackFontFamily, PdfTextObject>((Func<TextWithFallbackFontFamily, PdfTextObject>) (c =>
    {
      PdfFont font = PdfFontUtils.CreateFont(doc, string.IsNullOrEmpty(c.FallbackFontFamily?.Source) ? fontFamily.Source : c.FallbackFontFamily.Source, c.FontWeight, c.FontStyle, c.CharSet);
      PdfTextObject textObjects = PdfTextObject.Create(c.Text, (float) c.Bounds.Left, posY, font, c.ScaledFontSize);
      textObjects.FillColor = fillColor;
      textObjects.StrokeColor = strokeColor;
      textObjects.StrokeOverprint = false;
      return textObjects;
    })).ToList<PdfTextObject>();
    float num = list.Sum<PdfTextObject>((Func<PdfTextObject, float>) (c => c.BoundingBox.Width));
    if ((double) num > (double) rect.Width)
    {
      float sx = rect.Width / num;
      foreach (PdfTextObject pdfTextObject in list)
      {
        FS_RECTF boundingBox = pdfTextObject.BoundingBox;
        FS_MATRIX matrix = new FS_MATRIX();
        matrix.SetIdentity();
        matrix.Translate(-boundingBox.left, -boundingBox.bottom);
        matrix.Scale(sx, 1f);
        matrix.Translate(boundingBox.left, boundingBox.bottom);
        pdfTextObject.Transform(matrix);
      }
    }
    return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) list;
  }

  public static FS_RECTF GetInnerRectangle(FS_RECTF annotRect, float[] innerRect)
  {
    FS_RECTF innerRectangle = annotRect;
    if (innerRect == null)
      return innerRectangle;
    if (innerRect.Length != 0 && (double) innerRect[0] > 0.0)
      innerRectangle.left += innerRect[0];
    if (innerRect.Length > 1 && (double) innerRect[1] > 0.0)
      innerRectangle.top -= innerRect[1];
    if (innerRect.Length > 2 && (double) innerRect[2] > 0.0)
      innerRectangle.right -= innerRect[2];
    if (innerRect.Length > 3 && (double) innerRect[3] > 0.0)
      innerRectangle.bottom += innerRect[3];
    return innerRectangle;
  }

  public static FS_RECTF InflateRect(FS_RECTF rect, float val)
  {
    return new FS_RECTF(rect.left - val, rect.top + val, rect.right + val, rect.bottom - val);
  }

  private static List<FS_PATHPOINTF> GetSquarePoints(FS_RECTF rect, float lineWidth)
  {
    List<FS_PATHPOINTF> squarePoints = new List<FS_PATHPOINTF>();
    rect = AnnotDrawingHelper.InflateRect(rect, (float) ((0.0 - (double) lineWidth) / 2.0));
    squarePoints.Add(new FS_PATHPOINTF(rect.left, rect.top, PathPointFlags.MoveTo));
    squarePoints.Add(new FS_PATHPOINTF(rect.right, rect.top, PathPointFlags.LineTo));
    squarePoints.Add(new FS_PATHPOINTF(rect.right, rect.bottom, PathPointFlags.LineTo));
    squarePoints.Add(new FS_PATHPOINTF(rect.left, rect.bottom, PathPointFlags.LineTo));
    squarePoints.Add(new FS_PATHPOINTF(rect.left, rect.top, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    return squarePoints;
  }

  public static List<PdfPathObject> CreateSquare(
    FS_COLOR strokeColor,
    FS_COLOR fillColor,
    float borderWidth,
    float[] dashPattern,
    BorderStyles borderStyle,
    BorderEffects borderEffect,
    int effectIntencity,
    FS_RECTF rect)
  {
    List<PdfPathObject> square = new List<PdfPathObject>();
    if (fillColor.A == 0 && strokeColor.A == 0)
      return square;
    bool isStroke = strokeColor.A > 0 && (double) borderWidth != 0.0;
    PdfPathObject pdfPathObject = PdfPathObject.Create(fillColor.A > 0 ? FillModes.Winding : FillModes.None, isStroke);
    foreach (FS_PATHPOINTF squarePoint in AnnotDrawingHelper.GetSquarePoints(rect, borderWidth))
      pdfPathObject.Path.Add(squarePoint);
    pdfPathObject.StrokeColor = strokeColor;
    pdfPathObject.FillColor = fillColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject.Handle, borderWidth);
    if (borderStyle == BorderStyles.Dashed && dashPattern != null)
    {
      Pdfium.FPDFPageObj_SetDashArray(pdfPathObject.Handle, dashPattern);
      Pdfium.FPDFPageObj_SetDashPhase(pdfPathObject.Handle, 0.0f);
    }
    pdfPathObject.CalcBoundingBox();
    square.Add(pdfPathObject);
    return square;
  }

  public static List<PdfPathObject> CreateCalloutLines(
    FS_COLOR strokeColor,
    FS_COLOR fillColor,
    IList<FS_POINTF> lines,
    LineJoin lineJoin,
    PdfLineEndingCollection endings,
    float lineWidth,
    float[] dashPattern,
    BorderStyles borderStyle)
  {
    if (AnnotDrawingHelper.createCalloutLinesFunc == null)
    {
      Func<object, FS_COLOR, FS_COLOR, IList<FS_POINTF>, LineJoin, PdfLineEndingCollection, float, float[], BorderStyles, List<PdfPathObject>> tmpFunc = ReflectionHelper.BuildMethodFunction<object, FS_COLOR, FS_COLOR, IList<FS_POINTF>, LineJoin, PdfLineEndingCollection, float, float[], BorderStyles, List<PdfPathObject>>(AnnotDrawingHelper.annotDrawingType.GetMethod(nameof (CreateCalloutLines)));
      AnnotDrawingHelper.createCalloutLinesFunc = (Func<FS_COLOR, FS_COLOR, IList<FS_POINTF>, LineJoin, PdfLineEndingCollection, float, float[], BorderStyles, List<PdfPathObject>>) ((p1, p2, p3, p4, p5, p6, p7, p8) => tmpFunc((object) null, p1, p2, p3, p4, p5, p6, p7, p8));
    }
    return AnnotDrawingHelper.createCalloutLinesFunc(strokeColor, fillColor, lines, lineJoin, endings, lineWidth, dashPattern, borderStyle);
  }

  public static List<PdfTextObject> PatagamesCreateTextObjects(
    PdfDocument doc,
    string text,
    string fontName,
    float fontSize,
    FS_COLOR strokeColor,
    FS_COLOR fillColor,
    float padding,
    JustifyTypes textAlignment,
    FS_RECTF rect)
  {
    PdfFont font = CreateFont(doc, fontName);
    List<PdfTextObject> textObjects = new List<PdfTextObject>();
    float num1 = padding + 3f;
    float num2 = padding + 3f;
    float num3 = padding + 0.0f;
    float num4 = padding + 3f;
    float num5 = (float) (font.Ascent - font.Descent) / 1000f * fontSize;
    float y = (float) ((double) rect.top - (double) num3 - (double) font.Ascent / 1000.0 * (double) fontSize);
    string[] strArray = (text ?? "").Split(' ');
    PdfTextObject pdfTextObject1 = PdfTextObject.Create("", rect.left + num1, 0.0f, font, fontSize);
    PdfTextObject pdfTextObject2 = (PdfTextObject) null;
    string text1 = "";
    for (int index = 0; index < strArray.Length; ++index)
    {
      pdfTextObject1.TextUnicode = index <= 0 ? (strArray[index] == "" ? " " : strArray[index]) : $"{text1} {strArray[index]}";
      FS_RECTF fsRectf = new FS_RECTF();
      Pdfium.FPDFPageObj_GetBBox(pdfTextObject1.Handle, (FS_MATRIX) null, out fsRectf.left, out fsRectf.top, out fsRectf.right, out fsRectf.bottom);
      if ((double) fsRectf.right < (double) rect.right - (double) num2)
      {
        text1 = index <= 0 ? (strArray[index] == "" ? " " : strArray[index]) : $"{text1} {strArray[index]}";
        if (index < strArray.Length - 1)
          continue;
      }
      else if (text1 != "")
        --index;
      else if (text1 == "")
        text1 = strArray[index] ?? "";
      if (textAlignment == JustifyTypes.Left && pdfTextObject2 != null)
      {
        text1 = text1.TrimStart();
      }
      else
      {
        switch (textAlignment)
        {
          case JustifyTypes.Centered:
            text1 = text1.Trim();
            break;
          case JustifyTypes.Right:
            text1 = text1.TrimEnd();
            break;
        }
      }
      pdfTextObject2 = PdfTextObject.Create(text1, rect.left + num1, y, CreateFont(doc, fontName), fontSize);
      if (textAlignment == JustifyTypes.Right || textAlignment == JustifyTypes.Centered)
      {
        Pdfium.FPDFPageObj_GetBBox(pdfTextObject2.Handle, (FS_MATRIX) null, out fsRectf.left, out fsRectf.top, out fsRectf.right, out fsRectf.bottom);
        pdfTextObject2.Location = textAlignment != JustifyTypes.Right ? new FS_POINTF(pdfTextObject2.Location.X + (float) (((double) rect.Width - (double) num1 - (double) num2 - (double) fsRectf.Width) / 2.0), y) : new FS_POINTF(rect.right - num2 - fsRectf.Width, y);
      }
      pdfTextObject2.FillColor = fillColor;
      pdfTextObject2.StrokeColor = strokeColor;
      int a1 = fillColor.A;
      int a2 = strokeColor.A;
      if (a1 != 0 && a2 != 0)
        pdfTextObject2.RenderMode = TextRenderingModes.FillThenStroke;
      else if (a1 != 0 && a2 == 0)
        pdfTextObject2.RenderMode = TextRenderingModes.Fill;
      else if (a1 == 0 && a2 != 0)
        pdfTextObject2.RenderMode = TextRenderingModes.Stroke;
      else if (a1 == 0 && a2 == 0)
        pdfTextObject2.RenderMode = TextRenderingModes.Nothing;
      IntPtr path = Pdfium.FPDFPath_Create();
      Pdfium.FPDFPath_AppendRect(path, new FS_RECTF(rect.left + num1, rect.top - num3, rect.right - num2, rect.bottom + num4));
      Pdfium.FPDFPageObj_AppendClipPath(pdfTextObject2.Handle, path, FillModes.Winding, true);
      textObjects.Add(pdfTextObject2);
      y -= num5;
      Pdfium.FPDFPageObj_Release(pdfTextObject1.Handle);
      pdfTextObject1 = PdfTextObject.Create("", rect.left + num1, 0.0f, CreateFont(doc, fontName), fontSize);
      text1 = "";
    }
    return textObjects;

    static PdfFont CreateFont(PdfDocument _doc, string _fontName)
    {
      try
      {
        return PdfFont.CreateStandardFont(_doc, _fontName, 1);
      }
      catch
      {
        return PdfFont.CreateStock(_doc, FontStockNames.Arial);
      }
    }
  }

  public static List<PdfPageObject> CreateRuberStamp(
    string text,
    FS_COLOR color,
    FS_RECTF rect,
    PdfFont font)
  {
    if (AnnotDrawingHelper.createRuberStampFunc == null)
    {
      MethodInfo method = AnnotDrawingHelper.annotDrawingType.GetMethod(nameof (CreateRuberStamp));
      if (method == (MethodInfo) null)
        method = AnnotDrawingHelper.annotDrawingType.GetMethod(nameof (CreateRuberStamp), BindingFlags.Static | BindingFlags.NonPublic);
      Func<object, string, FS_COLOR, FS_RECTF, PdfFont, List<PdfPageObject>> tmpFunc = ReflectionHelper.BuildMethodFunction<object, string, FS_COLOR, FS_RECTF, PdfFont, List<PdfPageObject>>(method);
      AnnotDrawingHelper.createRuberStampFunc = (Func<string, FS_COLOR, FS_RECTF, PdfFont, List<PdfPageObject>>) ((p1, p2, p3, p4) => tmpFunc((object) null, p1, p2, p3, p4));
    }
    return AnnotDrawingHelper.createRuberStampFunc(text, color, rect, font);
  }

  public static FS_RECTF CalcBBox(IEnumerable collection)
  {
    float num1 = float.MaxValue;
    float num2 = float.MinValue;
    float num3 = float.MaxValue;
    float num4 = float.MinValue;
    foreach (object obj in collection)
    {
      FS_RECTF boundingBox = (obj as PdfPageObject).BoundingBox;
      num1 = Math.Min(num1, boundingBox.left);
      num2 = Math.Max(num2, boundingBox.right);
      num4 = Math.Max(num4, boundingBox.top);
      num3 = Math.Min(num3, boundingBox.bottom);
    }
    return new FS_RECTF(num1, num4, num2, num3);
  }

  private static bool AreEqual(float a, float b)
  {
    return (double) a - 1E-06 < (double) b && (double) a + 1E-06 > (double) b;
  }
}
