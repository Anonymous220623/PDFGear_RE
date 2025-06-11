// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.AnnotDrawing
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Represents a collection of drawing functions for standard annotation,
/// </summary>
internal class AnnotDrawing
{
  /// <summary>
  /// The coefficient used to calculate the bounding box for line endings.
  /// </summary>
  public static float LineEndingRectK = 4f;
  public static float roundRectCornerK = 5f;
  public static float stampBeizerK = 30f;
  public static float higlightBeizerK = 3f;
  /// <summary>Coefficient of expansion of a squiggly crest on width</summary>
  public static float squiglyKX = 6.5f;
  /// <summary>
  /// Coefficient of expansion of a squiggly crest on height
  /// </summary>
  public static float squiglyKY = 7.5f;
  /// <summary>
  /// coefficient of distance between centers of circles of neighboring waves
  /// </summary>
  public static float CloudyLengthDK = 0.6666667f;
  /// <summary>
  /// coefficient of radius of a circle of a wave, depends on intensity of effect
  /// </summary>
  public static float CloudyIntencityK = 5f;
  /// <summary>
  /// maximum number of iterations in calculating waves for an ellipse
  /// </summary>
  public static int CloudyCutout = 10;
  /// <summary>coefficient for the iteration angle</summary>
  public static float CloudyStepFactor = 100f;
  /// <summary>
  /// coefficient for calculating the minimum distance of the intersection of circles defining waves
  /// </summary>
  public static float CloudyLimitFactor = 0.2f;
  /// <summary>angle determining the tail of the wave</summary>
  public static float CloudyLengthTail = 0.3926991f;

  private static PdfFont CreateFont(PdfPage page, string fontName)
  {
    try
    {
      if (page.Dictionary.ContainsKey("Resources"))
      {
        PdfTypeDictionary pdfTypeDictionary1 = page.Dictionary["Resources"].As<PdfTypeDictionary>(false);
        if (pdfTypeDictionary1 != null && pdfTypeDictionary1.ContainsKey("Font"))
        {
          PdfTypeDictionary pdfTypeDictionary2 = pdfTypeDictionary1["Font"].As<PdfTypeDictionary>(false);
          if (pdfTypeDictionary2 != null && pdfTypeDictionary2.ContainsKey(fontName))
          {
            PdfTypeDictionary dict = pdfTypeDictionary2[fontName].As<PdfTypeDictionary>(false);
            if (dict != null)
              return PdfFont.CreateFont(page.Document, dict);
          }
        }
      }
      if (page.Document.FormFill != null)
      {
        PdfFont font = page.Document.FormFill.InterForm.Fonts[fontName];
        if (font != null)
          return font;
      }
      return PdfFont.CreateFont(page.Document, fontName);
    }
    catch
    {
      return PdfFont.CreateStock(page.Document, FontStockNames.Arial);
    }
  }

  /// <summary>
  /// Calculate bounding box for a collection of PdfPageObjects
  /// </summary>
  /// <param name="collection">Collection of <see cref="T:Patagames.Pdf.Net.PdfPageObject" /></param>
  /// <returns>Overal bounding box for entrie collection of objects</returns>
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

  /// <summary>Calculate inner rectangle  for annotation</summary>
  /// <param name="annotRect">Annotation rectangle.</param>
  /// <param name="innerRect">The four numbers correspond to the differences in default user space between the left, top, right, and bottom coordinates of Rect and those of the inner rectangle, respectively. </param>
  /// <returns>Inner rectangle for annotation.</returns>
  public static FS_RECTF GetInnerRectangle(FS_RECTF annotRect, float[] innerRect)
  {
    FS_RECTF innerRectangle = annotRect;
    float[] numArray = innerRect;
    if (numArray == null)
      return innerRectangle;
    if (numArray.Length != 0 && (double) numArray[0] > 0.0)
      innerRectangle.left += numArray[0];
    if (numArray.Length > 1 && (double) numArray[1] > 0.0)
      innerRectangle.top -= numArray[1];
    if (numArray.Length > 2 && (double) numArray[2] > 0.0)
      innerRectangle.right -= numArray[2];
    if (numArray.Length > 3 && (double) numArray[3] > 0.0)
      innerRectangle.bottom += numArray[3];
    return innerRectangle;
  }

  /// <summary>Checks two values of float type for equality</summary>
  /// <param name="a">A - value</param>
  /// <param name="b">B - value</param>
  /// <returns>True if a equal b</returns>
  private static bool AreEqual(float a, float b)
  {
    return (double) a - 1E-06 < (double) b && (double) a + 1E-06 > (double) b;
  }

  /// <summary>Calculate angle of the line specified.</summary>
  /// <param name="from">First point of the line.</param>
  /// <param name="to">Second point of the line.</param>
  /// <returns>An angle in the range from 0 to 2*PI, clockwise</returns>
  private static double GetVectorAng(FS_POINTF from, FS_POINTF to)
  {
    float a = from.X - to.X;
    double vectorAng;
    if (AnnotDrawing.AreEqual(a, 0.0f) && (double) from.Y > (double) to.Y)
      vectorAng = 1.5707963705062866;
    else if (AnnotDrawing.AreEqual(a, 0.0f) && (double) from.Y < (double) to.Y)
      vectorAng = 4.71238899230957;
    else if (AnnotDrawing.AreEqual(a, 0.0f) && AnnotDrawing.AreEqual(from.Y, to.Y))
    {
      vectorAng = 0.0;
    }
    else
    {
      double num = Math.Atan(((double) from.Y - (double) to.Y) / (double) a);
      vectorAng = (double) from.X >= (double) to.X ? Math.PI - num : (num >= 0.0 ? 2.0 * Math.PI - num : -num);
    }
    return vectorAng;
  }

  /// <summary>
  /// Transforms the quadrilateral with the specified matrix.
  /// </summary>
  /// <param name="m">Transformation matrix.</param>
  /// <param name="quad">The quadrilateral to be transformed.</param>
  /// <returns>The transformed quadrilateral.</returns>
  private static FS_QUADPOINTSF TransforQuad(FS_MATRIX m, FS_QUADPOINTSF quad)
  {
    float x1 = quad.x1;
    float y1 = quad.y1;
    float x2 = quad.x2;
    float y2 = quad.y2;
    float x3 = quad.x3;
    float y3 = quad.y3;
    float x4 = quad.x4;
    float y4 = quad.y4;
    m.TransformPoint(ref x1, ref y1);
    m.TransformPoint(ref x2, ref y2);
    m.TransformPoint(ref x3, ref y3);
    m.TransformPoint(ref x4, ref y4);
    return new FS_QUADPOINTSF(x1, y1, x2, y2, x3, y3, x4, y4);
  }

  /// <summary>
  /// Expands or shrinks the rectangle by using the specified value amount, in all directions.
  /// </summary>
  /// <param name="rect">The <see cref="T:Patagames.Pdf.FS_RECTF" /> structure to modify.</param>
  /// <param name="val">The amount by which to expand or shrink the rectangle.</param>
  /// <returns>The resulting rectangle.</returns>
  private static FS_RECTF InflateRect(FS_RECTF rect, float val)
  {
    return new FS_RECTF(rect.left - val, rect.top + val, rect.right + val, rect.bottom - val);
  }

  private static FS_POINTF RotatePoint(float ang, float x, float y)
  {
    FS_MATRIX fsMatrix = new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    fsMatrix.Rotate(ang);
    fsMatrix.TransformPoint(ref x, ref y);
    return new FS_POINTF(x, y);
  }

  private static double GetVectorLen(FS_POINTF from, FS_POINTF to)
  {
    return Math.Sqrt(Math.Pow((double) to.X - (double) from.X, 2.0) + Math.Pow((double) to.Y - (double) from.Y, 2.0));
  }

  /// <summary>Do two rays intersect or not</summary>
  /// <param name="lp1">the beginning of the first ray</param>
  /// <param name="lp2">the end of the first ray</param>
  /// <param name="rp1">the beginning of the second ray</param>
  /// <param name="rp2">the end of the second ray</param>
  /// <returns>Boolean value. 1 - the rays intersect, 0 - no</returns>
  private static bool IsRayCross(FS_POINTF lp1, FS_POINTF lp2, FS_POINTF rp1, FS_POINTF rp2)
  {
    bool flag = false;
    float a = (float) (((double) lp2.Y - (double) lp1.Y) / ((double) lp2.X - (double) lp1.X));
    float b = (float) (((double) rp2.Y - (double) rp1.Y) / ((double) rp2.X - (double) rp1.X));
    float num1 = (float) (((double) lp1.Y * (double) lp2.X - (double) lp2.Y * (double) lp1.X) / ((double) lp2.X - (double) lp1.X));
    float num2 = (float) (((double) rp1.Y * (double) rp2.X - (double) rp2.Y * (double) rp1.X) / ((double) rp2.X - (double) rp1.X));
    if (!AnnotDrawing.AreEqual(a, b))
    {
      float x = (float) (((double) num2 - (double) num1) / ((double) a - (double) b));
      float y = a * x + num1;
      FS_POINTF to = new FS_POINTF(x, y);
      AnnotDrawing.GetVectorAng(rp1, rp2);
      AnnotDrawing.GetVectorAng(rp1, to);
      AnnotDrawing.GetVectorAng(lp1, lp2);
      AnnotDrawing.GetVectorAng(lp1, to);
      if (AnnotDrawing.AreEqual((float) AnnotDrawing.GetVectorAng(lp1, lp2), (float) AnnotDrawing.GetVectorAng(lp1, to)) && AnnotDrawing.AreEqual((float) AnnotDrawing.GetVectorAng(rp1, rp2), (float) AnnotDrawing.GetVectorAng(rp1, to)))
        flag = true;
    }
    return flag;
  }

  /// <summary>
  /// Creates a collection of text objects constructed on given parameters.
  /// </summary>
  /// <param name="page">The page on which the text object is created.</param>
  /// <param name="text">Text string.</param>
  /// <param name="fontName">Font that be used for text object.</param>
  /// <param name="fontSize">Font size.</param>
  /// <param name="strokeColor">Stroke color.</param>
  /// <param name="fillColor">Fill color.</param>
  /// <param name="padding">Value used to padding calculation.</param>
  /// <param name="textAlignment">Text alignment.</param>
  /// <param name="rect">Rectangle in which the text should be inscribed</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfTextObject" /> or empty collection if there is no any text objects.</returns>
  /// <remarks>Splits a text string into lines (by words) that fit into a given rectangle and creates text objects one by one for each line.</remarks>
  public static List<PdfTextObject> CreateTextObjects(
    PdfPage page,
    string text,
    string fontName,
    float fontSize,
    FS_COLOR strokeColor,
    FS_COLOR fillColor,
    float padding,
    JustifyTypes textAlignment,
    FS_RECTF rect)
  {
    PdfDocument document = page.Document;
    PdfFont font = AnnotDrawing.CreateFont(page, fontName);
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
      pdfTextObject2 = PdfTextObject.Create(text1, rect.left + num1, y, AnnotDrawing.CreateFont(page, fontName), fontSize);
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
      pdfTextObject1 = PdfTextObject.Create("", rect.left + num1, 0.0f, AnnotDrawing.CreateFont(page, fontName), fontSize);
      text1 = "";
    }
    return textObjects;
  }

  /// <summary>
  /// Creates a collection that contains  paths and texts used to drawing the ruber stamp annotation.
  /// </summary>
  /// <param name="text">The text drawing inside  stamp.</param>
  /// <param name="color">The stamp color. Used for text and border.</param>
  /// <param name="rect">The annotation rectangle.</param>
  /// <param name="font">The font used for text objects.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> or empty collection if there is no any objects.</returns>
  /// <remarks>This method scales the created text object so that it always fits all in one line.</remarks>
  internal static List<PdfPageObject> CreateRuberStamp(
    string text,
    FS_COLOR color,
    FS_RECTF rect,
    PdfFont font)
  {
    List<PdfPageObject> ruberStamp = new List<PdfPageObject>();
    float roundK = rect.Height / AnnotDrawing.roundRectCornerK;
    FS_RECTF fsRectf = AnnotDrawing.InflateRect(rect, -roundK);
    PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject.StrokeColor = color;
    foreach (FS_PATHPOINTF roundRectPoint in AnnotDrawing.GetRoundRectPoints(rect, roundK))
      pdfPathObject.Path.Add(roundRectPoint);
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject.Handle, 2f);
    pdfPathObject.CalcBoundingBox();
    ruberStamp.Add((PdfPageObject) pdfPathObject);
    PdfTextObject pdfTextObject = PdfTextObject.Create(text, 0.0f, 0.0f, font, 1f);
    pdfTextObject.FillColor = color;
    float left;
    float top;
    float right;
    float bottom;
    Pdfium.FPDFPageObj_GetBBox(pdfTextObject.Handle, (FS_MATRIX) null, out left, out top, out right, out bottom);
    float num = Math.Min(fsRectf.Width / (right - left), fsRectf.Height / (top - bottom));
    FS_MATRIX fsMatrix = new FS_MATRIX(num, 0.0f, 0.0f, num, 0.0f, 0.0f);
    Pdfium.FPDFPageObj_GetBBox(pdfTextObject.Handle, fsMatrix, out left, out top, out right, out bottom);
    fsMatrix.e = fsRectf.left + (float) (((double) fsRectf.Width - (double) right + (double) left) / 2.0);
    fsMatrix.f = fsRectf.bottom + (float) (((double) fsRectf.Height - (double) top + (double) bottom) / 2.0) - bottom;
    pdfTextObject.Transform(fsMatrix);
    ruberStamp.Add((PdfPageObject) pdfTextObject);
    return ruberStamp;
  }

  /// <summary>
  /// Creates a collection that contains paths for caret annotation.
  /// </summary>
  /// <param name="color">The color of the caret to be draw.</param>
  /// <param name="rect">The bounding box the caret will be drawn</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> or empty collection if there is no any objects.</returns>
  /// <remarks>This method do not create new paragrapth symbol used for annotation.</remarks>
  public static List<PdfPathObject> CreateCaret(FS_COLOR color, FS_RECTF rect)
  {
    List<PdfPathObject> caret = new List<PdfPathObject>();
    PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject.FillColor = color;
    pdfPathObject.Path.Add(new FS_PATHPOINTF(rect.left, rect.bottom, PathPointFlags.MoveTo));
    float num = rect.Width / AnnotDrawing.stampBeizerK;
    pdfPathObject.Path.Add(new FS_PATHPOINTF(rect.left + rect.Width / 2f - num, rect.bottom + num, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(rect.left + rect.Width / 2f - num, rect.bottom + num, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(rect.left + rect.Width / 2f, rect.top, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(rect.left + rect.Width / 2f + num, rect.bottom + num, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(rect.left + rect.Width / 2f + num, rect.bottom + num, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(rect.right, rect.bottom, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(rect.left, rect.bottom, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject.CalcBoundingBox();
    caret.Add(pdfPathObject);
    return caret;
  }

  /// <summary>
  /// Creates a collection that contains paths for square annotation.
  /// </summary>
  /// <param name="strokeColor">Stroke color.</param>
  /// <param name="fillColor">Fill color.</param>
  /// <param name="borderWidth">The border width in points. If this value is 0, no border is drawn.</param>
  /// <param name="dashPattern">A dash array defining a pattern of dashes and gaps to be used in drawing a dashed border.</param>
  /// <param name="borderStyle">A border style.</param>
  /// <param name="borderEffect">A border effect to apply.</param>
  /// <param name="effectIntencity">A number describing the intensity of the effect. Suggested values range from 0 to 2. </param>
  /// <param name="rect">Bounding box in which the rectangle should be inscribed.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> or empty collection if there is no any path objects.</returns>
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
    bool isStroke = strokeColor.A > 0 && !AnnotDrawing.AreEqual(borderWidth, 0.0f);
    PdfPathObject pdfPathObject = PdfPathObject.Create(fillColor.A > 0 ? FillModes.Winding : FillModes.None, isStroke);
    if (borderEffect == BorderEffects.Cloudy)
    {
      foreach (FS_PATHPOINTF cloudySquarePoint in AnnotDrawing.GetCloudySquarePoints(rect, effectIntencity, borderWidth))
        pdfPathObject.Path.Add(cloudySquarePoint);
    }
    else
    {
      foreach (FS_PATHPOINTF squarePoint in AnnotDrawing.GetSquarePoints(rect, borderWidth))
        pdfPathObject.Path.Add(squarePoint);
    }
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

  /// <summary>
  /// Creates a collection that contains paths for circle annotation.
  /// </summary>
  /// <param name="strokeColor">Stroke color.</param>
  /// <param name="fillColor">Fill color.</param>
  /// <param name="lineWidth">The border width in points. If this value is 0, no border is drawn.</param>
  /// <param name="dashPattern">A dash array defining a pattern of dashes and gaps to be used in drawing a dashed border.</param>
  /// <param name="borderStyle">A border style.</param>
  /// <param name="borderEffect">A border effect to apply.</param>
  /// <param name="effectIntencity">A number describing the intensity of the effect. Suggested values range from 0 to 2. </param>
  /// <param name="rect">Bounding box in which the circle should be inscribed.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> or empty collection if there is no any path objects.</returns>
  public static List<PdfPathObject> CreateCircle(
    FS_COLOR strokeColor,
    FS_COLOR fillColor,
    float lineWidth,
    float[] dashPattern,
    BorderStyles borderStyle,
    BorderEffects borderEffect,
    int effectIntencity,
    FS_RECTF rect)
  {
    List<PdfPathObject> circle = new List<PdfPathObject>();
    if (fillColor.A == 0 && strokeColor.A == 0)
      return circle;
    bool isStroke = strokeColor.A > 0 && !AnnotDrawing.AreEqual(lineWidth, 0.0f);
    PdfPathObject pdfPathObject = PdfPathObject.Create(fillColor.A > 0 ? FillModes.Winding : FillModes.None, isStroke);
    if (borderEffect == BorderEffects.Cloudy && effectIntencity > 0)
    {
      foreach (FS_PATHPOINTF cloudyCirclePoint in AnnotDrawing.GetCloudyCirclePoints(rect, effectIntencity, lineWidth))
        pdfPathObject.Path.Add(cloudyCirclePoint);
    }
    else
    {
      foreach (FS_PATHPOINTF circlePoint in AnnotDrawing.GetCirclePoints(rect, lineWidth))
        pdfPathObject.Path.Add(circlePoint);
    }
    pdfPathObject.StrokeColor = strokeColor;
    pdfPathObject.FillColor = fillColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject.Handle, lineWidth);
    if (borderStyle == BorderStyles.Dashed && dashPattern != null)
    {
      Pdfium.FPDFPageObj_SetDashArray(pdfPathObject.Handle, dashPattern);
      Pdfium.FPDFPageObj_SetDashPhase(pdfPathObject.Handle, 0.0f);
    }
    pdfPathObject.CalcBoundingBox();
    circle.Add(pdfPathObject);
    return circle;
  }

  /// <summary>
  /// Creates a collection that contains paths for line annotation.
  /// </summary>
  /// <param name="line">A collection of two points, specifying the starting and ending coordinates of the line in default user space.</param>
  /// <param name="lineColor">The line color </param>
  /// <param name="interiorColor">The fill color with which to fill the annotation’s line endings, if any.</param>
  /// <param name="endings">A collection of two line ending styles to be used in drawing the line.</param>
  /// <param name="leaderLineLenght">The length of leader lines in default user space that extend from each endpoint of the line perpendicular to the line itself.</param>
  /// <param name="leaderLineExtension">A  non-negative number representing the length of leader line extensions that extend from the line proper 180 degrees from the leader lines.</param>
  /// <param name="leaderLineOffset">A non-negative number representing the length of the leader line offset, which is the amount of empty space between the endpoints of the annotation and the beginning of the leader lines.</param>
  /// <param name="caption"></param>
  /// <param name="showCaption">If true, the text specified by the caption should be replicated as a caption in the appearance of the line.</param>
  /// <param name="captionOffset">The offset of the caption text from its normal position.</param>
  /// <param name="captionPosition">A value describing the annotation’s caption positioning.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <param name="dashPattern">Dash pattern.</param>
  /// <param name="lineStyle">The line style</param>
  /// <param name="font">The font used for the text drawing.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> or empty collection if there is no any path objects.</returns>
  /// <remarks>
  /// <para>
  /// A positive value of leaderLineLenght means that the leader lines appear in the direction that is clockwise when traversing the line from its starting point to its ending point; a negative value indicates the opposite direction.
  /// </para>
  /// </remarks>
  public static List<PdfPageObject> CreateLine(
    IList<FS_POINTF> line,
    FS_COLOR lineColor,
    FS_COLOR interiorColor,
    PdfLineEndingCollection endings,
    float leaderLineLenght,
    float leaderLineExtension,
    float leaderLineOffset,
    string caption,
    bool showCaption,
    FS_SIZEF captionOffset,
    CaptionPositions captionPosition,
    float lineWidth,
    float[] dashPattern,
    BorderStyles lineStyle,
    PdfFont font)
  {
    List<PdfPageObject> line1 = new List<PdfPageObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.None, true);
    bool flag1 = false;
    bool flag2 = false;
    FS_POINTF fsPointf1 = line[0];
    FS_POINTF fsPointf2 = line[1];
    FS_POINTF fsPointf3 = fsPointf1;
    FS_POINTF fsPointf4 = fsPointf2;
    FS_POINTF fsPointf5 = fsPointf1;
    FS_POINTF fsPointf6 = fsPointf2;
    double vectorAng = AnnotDrawing.GetVectorAng(fsPointf1, fsPointf2);
    if (!AnnotDrawing.AreEqual(leaderLineLenght, 0.0f))
    {
      flag1 = true;
      double angle = ((double) leaderLineLenght > 0.0 ? Math.PI / 2.0 : -1.0 * Math.PI / 2.0) - vectorAng;
      FS_MATRIX fsMatrix = new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      fsMatrix.Rotate((float) angle);
      float x1 = Math.Abs(leaderLineLenght) + Math.Abs(leaderLineOffset);
      float y1 = 0.0f;
      fsMatrix.TransformPoint(ref x1, ref y1);
      fsPointf1.X += x1;
      fsPointf1.Y += y1;
      fsPointf2.X += x1;
      fsPointf2.Y += y1;
      x1 = Math.Abs(leaderLineOffset);
      float y2 = 0.0f;
      fsMatrix.TransformPoint(ref x1, ref y2);
      fsPointf3.X += x1;
      fsPointf3.Y += y2;
      fsPointf4.X += x1;
      fsPointf4.Y += y2;
      if (!AnnotDrawing.AreEqual(leaderLineExtension, 0.0f))
      {
        flag2 = true;
        fsMatrix.SetIdentity();
        fsMatrix.Rotate((float) angle);
        float x2 = Math.Abs(leaderLineExtension);
        float y3 = 0.0f;
        fsMatrix.TransformPoint(ref x2, ref y3);
        fsPointf5 = new FS_POINTF(fsPointf1.X + x2, fsPointf1.Y + y3);
        fsPointf6 = new FS_POINTF(fsPointf2.X + x2, fsPointf2.Y + y3);
      }
    }
    if (flag1)
    {
      pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf3.X, fsPointf3.Y, PathPointFlags.MoveTo));
      pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf1.X, fsPointf1.Y, PathPointFlags.LineTo));
      pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf2.X, fsPointf2.Y, PathPointFlags.LineTo));
      pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf4.X, fsPointf4.Y, PathPointFlags.LineTo));
      if (flag2)
      {
        pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf1.X, fsPointf1.Y, PathPointFlags.MoveTo));
        pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf5.X, fsPointf5.Y, PathPointFlags.LineTo));
        pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf2.X, fsPointf2.Y, PathPointFlags.MoveTo));
        pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf6.X, fsPointf6.Y, PathPointFlags.LineTo));
      }
    }
    else
    {
      pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf1.X, fsPointf1.Y, PathPointFlags.MoveTo));
      pdfPathObject1.Path.Add(new FS_PATHPOINTF(fsPointf2.X, fsPointf2.Y, PathPointFlags.LineTo));
    }
    pdfPathObject1.StrokeColor = lineColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject1.Handle, lineWidth);
    if (lineStyle == BorderStyles.Dashed && dashPattern != null)
    {
      Pdfium.FPDFPageObj_SetDashArray(pdfPathObject1.Handle, dashPattern);
      Pdfium.FPDFPageObj_SetDashPhase(pdfPathObject1.Handle, 0.0f);
    }
    pdfPathObject1.CalcBoundingBox();
    line1.Add((PdfPageObject) pdfPathObject1);
    if (endings != null && endings.Count > 0)
    {
      foreach (PdfPathObject pdfPathObject2 in AnnotDrawing.CreateLineEnding(fsPointf1, fsPointf2, endings[0], lineWidth, lineColor, interiorColor, lineStyle, dashPattern))
        line1.Add((PdfPageObject) pdfPathObject2);
    }
    if (endings != null && endings.Count > 1)
    {
      foreach (PdfPathObject pdfPathObject3 in AnnotDrawing.CreateLineEnding(fsPointf2, fsPointf1, endings[1], lineWidth, lineColor, interiorColor, lineStyle, dashPattern))
        line1.Add((PdfPageObject) pdfPathObject3);
    }
    if (showCaption && font != null)
    {
      caption = caption.Trim();
      PdfTextObject pdfTextObject = PdfTextObject.Create(caption, 0.0f, 0.0f, font, 10f);
      FS_RECTF rect = pdfTextObject.BoundingBox;
      float num = (float) Math.Sqrt(Math.Pow((double) fsPointf2.X - (double) fsPointf1.X, 2.0) + Math.Pow((double) fsPointf2.Y - (double) fsPointf1.Y, 2.0));
      float x = captionOffset.Width - (float) (((double) rect.Width - (double) num) / 2.0);
      float y = captionOffset.Height - rect.Height / 2f - rect.bottom;
      bool flag3 = (double) rect.left + (double) x < (double) lineWidth * (double) AnnotDrawing.LineEndingRectK || (double) rect.right + (double) x > (double) num - (double) lineWidth * (double) lineWidth;
      if (captionPosition == CaptionPositions.Top | flag3)
      {
        y = (double) leaderLineLenght < 0.0 ? -rect.top - lineWidth - captionOffset.Height : -rect.bottom + lineWidth + captionOffset.Height;
        if (flag3)
          y += (double) leaderLineLenght < 0.0 ? -leaderLineExtension : leaderLineExtension;
      }
      pdfTextObject.Location = new FS_POINTF(x, y);
      pdfTextObject.FillColor = lineColor;
      rect = AnnotDrawing.InflateRect(pdfTextObject.BoundingBox, lineWidth);
      FS_MATRIX fsMatrix = new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      fsMatrix.Rotate((float) -vectorAng);
      fsMatrix.Translate(fsPointf1.X, fsPointf1.Y);
      pdfTextObject.Transform(fsMatrix);
      if (captionPosition == CaptionPositions.Inline && !flag3)
      {
        PathPointsCollection pointsCollection = new PathPointsCollection();
        pointsCollection.AppendRect(pdfPathObject1.BoundingBox);
        FS_QUADPOINTSF fsQuadpointsf = AnnotDrawing.TransforQuad(fsMatrix, new FS_QUADPOINTSF(rect));
        pointsCollection.Add(new FS_PATHPOINTF(fsQuadpointsf.x1, fsQuadpointsf.y1, PathPointFlags.MoveTo));
        pointsCollection.Add(new FS_PATHPOINTF(fsQuadpointsf.x2, fsQuadpointsf.y2, PathPointFlags.LineTo));
        pointsCollection.Add(new FS_PATHPOINTF(fsQuadpointsf.x3, fsQuadpointsf.y3, PathPointFlags.LineTo));
        pointsCollection.Add(new FS_PATHPOINTF(fsQuadpointsf.x4, fsQuadpointsf.y4, PathPointFlags.LineTo));
        pointsCollection.Add(new FS_PATHPOINTF(fsQuadpointsf.x1, fsQuadpointsf.y1, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
        Pdfium.FPDFPageObj_AppendClipPath(pdfPathObject1.Handle, pointsCollection.Handle, FillModes.Alternate, false);
      }
      line1.Add((PdfPageObject) pdfTextObject);
    }
    return line1;
  }

  /// <summary>
  /// Creates a collection that contains paths for line with given parameters.
  /// </summary>
  /// <param name="strokeColor">Stroke color.</param>
  /// <param name="vertices">Line's vertices.</param>
  /// <param name="lineJoin">Join style.</param>
  /// <param name="lineWidth">Line width.</param>
  /// <param name="dashPattern">Dash pattern.</param>
  /// <param name="borderStyle">Border srtyle.</param>
  /// <param name="isClosed">Indicates whether the figure should be closed.</param>
  /// <param name="borderEffect">Border effect.</param>
  /// <param name="effectIntencity">Border effect intencity.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> or empty collection if there is no any path objects.</returns>
  public static List<PdfPathObject> CreateLines(
    FS_COLOR strokeColor,
    IList<FS_POINTF> vertices,
    LineJoin lineJoin,
    float lineWidth,
    float[] dashPattern,
    BorderStyles borderStyle,
    bool isClosed,
    BorderEffects borderEffect,
    int effectIntencity)
  {
    List<PdfPathObject> lines = new List<PdfPathObject>();
    if (vertices == null || vertices.Count < 2)
      return lines;
    PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.None, true);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject.Handle, lineJoin);
    if (borderEffect == BorderEffects.Cloudy && effectIntencity > 0)
    {
      foreach (FS_PATHPOINTF cloudyLinePoint in AnnotDrawing.GetCloudyLinePoints(vertices, effectIntencity))
        pdfPathObject.Path.Add(cloudyLinePoint);
    }
    else
    {
      for (int index = 0; index < vertices.Count; ++index)
      {
        PathPointFlags flags = PathPointFlags.LineTo;
        if (index == 0)
          flags = PathPointFlags.MoveTo;
        else if (isClosed && index == vertices.Count - 1)
          flags = PathPointFlags.CloseFigure | PathPointFlags.LineTo;
        pdfPathObject.Path.Add(new FS_PATHPOINTF(vertices[index].X, vertices[index].Y, flags));
      }
    }
    pdfPathObject.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject.Handle, lineWidth);
    if (borderStyle == BorderStyles.Dashed && dashPattern != null)
    {
      Pdfium.FPDFPageObj_SetDashArray(pdfPathObject.Handle, dashPattern);
      Pdfium.FPDFPageObj_SetDashPhase(pdfPathObject.Handle, 0.0f);
    }
    pdfPathObject.CalcBoundingBox();
    lines.Add(pdfPathObject);
    return lines;
  }

  /// <summary>
  /// Creates a collection that contains paths for the callout lines.
  /// </summary>
  /// <param name="strokeColor">The color of the line.</param>
  /// <param name="fillColor">InteriorColor for filling of line endings if any.</param>
  /// <param name="lines">A collection of vertices specifying a callout line.</param>
  /// <param name="lineJoin">The line join style specifies the shape to be used at the corners of paths that are used for callouts lines.</param>
  /// <param name="endings">A collection of the line ending styles to be used in drawing the callout lines.</param>
  /// <param name="lineWidth">The line width in points. If this value is 0, no border is drawn.</param>
  /// <param name="dashPattern">A dash array defining a pattern of dashes and gaps to be used in drawing a dashed line.</param>
  /// <param name="borderStyle">A line style.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> or empty collection if there is no any path objects.</returns>
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
    List<PdfPathObject> calloutLines = new List<PdfPathObject>();
    if (strokeColor.A == 0 || AnnotDrawing.AreEqual(lineWidth, 0.0f) || lines == null || lines.Count <= 1)
      return calloutLines;
    calloutLines.AddRange((IEnumerable<PdfPathObject>) AnnotDrawing.CreateLines(strokeColor, lines, lineJoin, lineWidth, dashPattern, borderStyle, false, BorderEffects.None, 0));
    if (endings == null || endings.Count <= 0)
      return calloutLines;
    for (int index = 0; index < endings.Count && index < lines.Count; ++index)
    {
      if (endings[index] != LineEndingStyles.None)
      {
        List<PdfPathObject> lineEnding = AnnotDrawing.CreateLineEnding(lines[index], index < lines.Count - 1 ? lines[index + 1] : (index > 0 ? lines[index - 1] : lines[index]), endings[index], lineWidth, strokeColor, fillColor, borderStyle, dashPattern);
        calloutLines.AddRange((IEnumerable<PdfPathObject>) lineEnding);
      }
    }
    return calloutLines;
  }

  /// <summary>
  ///  Creates a  a collection of path objects which represents the line ending
  /// </summary>
  /// <param name="target">The point where the line ending should be placed</param>
  /// <param name="from">The second point of the input line for this ending.</param>
  /// <param name="e">The line ending style.</param>
  /// <param name="lineWidth">The line width to be used to drawn of this ending</param>
  /// <param name="strokeColor">The color of the line.</param>
  /// <param name="interiorColor">Interior color with which to fill the annotation’s line endings.</param>
  /// <param name="lineStyle">A line style.</param>
  /// <param name="dashPattern">A dash array defining a pattern of dashes and gaps to be used in drawing a dashed line.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> or empty collection if there is no any path objects.</returns>
  public static List<PdfPathObject> CreateLineEnding(
    FS_POINTF target,
    FS_POINTF from,
    LineEndingStyles e,
    float lineWidth,
    FS_COLOR strokeColor,
    FS_COLOR interiorColor,
    BorderStyles lineStyle,
    float[] dashPattern)
  {
    List<PdfPathObject> lineEnding = new List<PdfPathObject>();
    int fillMode;
    switch (e)
    {
      case LineEndingStyles.None:
        return lineEnding;
      case LineEndingStyles.OpenArrow:
      case LineEndingStyles.Butt:
      case LineEndingStyles.ROpenArrow:
      case LineEndingStyles.Slash:
        fillMode = 0;
        break;
      default:
        fillMode = 2;
        break;
    }
    PdfPathObject pdfPathObject = PdfPathObject.Create((FillModes) fillMode, true);
    pdfPathObject.StrokeColor = strokeColor;
    pdfPathObject.FillColor = interiorColor;
    if (lineStyle == BorderStyles.Dashed && dashPattern != null)
    {
      Pdfium.FPDFPageObj_SetDashArray(pdfPathObject.Handle, dashPattern);
      Pdfium.FPDFPageObj_SetDashPhase(pdfPathObject.Handle, 0.0f);
    }
    double vectorAng = AnnotDrawing.GetVectorAng(from, target);
    List<FS_PATHPOINTF> lineEndingPoints = AnnotDrawing.GetLineEndingPoints(e, lineWidth, target, out bool _);
    if (lineEndingPoints != null)
    {
      Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject.Handle, lineWidth);
      foreach (FS_PATHPOINTF fsPathpointf in lineEndingPoints)
        pdfPathObject.Path.Add(fsPathpointf);
      pdfPathObject.TransformPath(new FS_MATRIX(1f, 0.0f, 0.0f, 1f, -target.X, -target.Y));
      pdfPathObject.TransformPath(new FS_MATRIX(Math.Cos(vectorAng), -Math.Sin(vectorAng), Math.Sin(vectorAng), Math.Cos(vectorAng), (double) target.X, (double) target.Y));
      pdfPathObject.CalcBoundingBox();
      lineEnding.Add(pdfPathObject);
    }
    return lineEnding;
  }

  /// <summary>
  /// Creates a collection of path objects which represents the text highlight.
  /// </summary>
  /// <param name="fillColor">The color of the higlight.</param>
  /// <param name="quadPoints">A collection of the quadrilaterals which should be highlighted.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> or empty collection if there is no any path objects.</returns>
  public static List<PdfPathObject> CreateHighlight(
    FS_COLOR fillColor,
    PdfQuadPointsCollection quadPoints)
  {
    List<PdfPathObject> highlight = new List<PdfPathObject>();
    foreach (FS_QUADPOINTSF quadPoint in quadPoints)
    {
      PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.Winding, false);
      foreach (FS_PATHPOINTF highlightPoint in AnnotDrawing.GetHighlightPoints(quadPoint))
        pdfPathObject.Path.Add(highlightPoint);
      pdfPathObject.FillColor = fillColor;
      Pdfium.FPDFPageObj_SetBlendMode(pdfPathObject.Handle, BlendTypes.FXDIB_BLEND_MULTIPLY);
      pdfPathObject.CalcBoundingBox();
      highlight.Add(pdfPathObject);
    }
    return highlight;
  }

  /// <summary>
  /// Creates a collection of path objects which represents the strikeout/underline.
  /// </summary>
  /// <param name="strokeColor">The color of the strikeout/underline.</param>
  /// <param name="quadPoints">A collection of the quadrilaterals in which Underline/strikeout should be drawn.</param>
  /// <param name="displacePercent">The place where the line should be placed. In percentage of length of side edges(left - (x1;y1)-(x3;y3) and right (x2;y2)-(x4;y4).</param>
  /// <param name="lineWidthPercent">The line width. In percentages of length of least side edge.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> or empty collection if there is no any path objects.</returns>
  public static List<PdfPathObject> CreateUnderlineStrikeout(
    FS_COLOR strokeColor,
    PdfQuadPointsCollection quadPoints,
    float displacePercent,
    float lineWidthPercent)
  {
    List<PdfPathObject> underlineStrikeout = new List<PdfPathObject>();
    foreach (FS_QUADPOINTSF quadPoint in quadPoints)
    {
      float lineWidth;
      FS_PATHPOINTF[] textMarkupPoints = AnnotDrawing.GetTextMarkupPoints(quadPoint, displacePercent, lineWidthPercent, out lineWidth);
      PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.None, true);
      pdfPathObject.StrokeColor = strokeColor;
      foreach (FS_PATHPOINTF fsPathpointf in textMarkupPoints)
        pdfPathObject.Path.Add(fsPathpointf);
      Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject.Handle, lineWidth);
      pdfPathObject.CalcBoundingBox();
      underlineStrikeout.Add(pdfPathObject);
    }
    return underlineStrikeout;
  }

  /// <summary>
  /// Creates a collection of path objects which represents the squiggly line.
  /// </summary>
  /// <param name="strokeColor">The color of the strikeout/underline.</param>
  /// <param name="quadPoints">A collection of the quadrilaterals in which the squiggly line should be drawn</param>
  /// <param name="displacePercent">The place where the line should be placed. In percentage of length of side edges(left - (x1;y1)-(x3;y3) and right (x2;y2)-(x4;y4).</param>
  /// <param name="lineWidthPercent">The line width. In percentages of length of least side edge.</param>
  /// <returns>Collection of <see cref="T:Patagames.Pdf.Net.PdfPathObject" /> or empty collection if there is no any path objects.</returns>
  public static List<PdfPathObject> CreateSquiggly(
    FS_COLOR strokeColor,
    PdfQuadPointsCollection quadPoints,
    float displacePercent,
    float lineWidthPercent)
  {
    List<PdfPathObject> squiggly = new List<PdfPathObject>();
    foreach (FS_QUADPOINTSF quadPoint in quadPoints)
    {
      float lineWidth;
      List<FS_PATHPOINTF> squigglyPoints = AnnotDrawing.GetSquigglyPoints(quadPoint, displacePercent, lineWidthPercent, out lineWidth);
      PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.None, true);
      pdfPathObject.StrokeColor = strokeColor;
      foreach (FS_PATHPOINTF fsPathpointf in squigglyPoints)
        pdfPathObject.Path.Add(fsPathpointf);
      Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject.Handle, LineJoin.Round);
      Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject.Handle, lineWidth);
      pdfPathObject.CalcBoundingBox();
      squiggly.Add(pdfPathObject);
    }
    return squiggly;
  }

  public static List<PdfPathObject> CreateNote(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> note = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.959f, 1.3672f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.959f, 0.9331999f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.607f, 0.5821999f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.175f, 0.5821999f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(2.047999f, 0.5821999f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.613999f, 0.5821999f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.263f, 0.9331999f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.263f, 1.3672f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.263f, 18.6332f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.263f, 19.0662f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.613999f, 19.4182f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(2.047999f, 19.4182f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.175f, 19.4182f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.607f, 19.4182f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.959f, 19.0662f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.959f, 18.6332f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.959f, 1.3672f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject1.FillColor = fillColor;
    pdfPathObject1.StrokeColor = strokeColor;
    pdfPathObject1.CalcBoundingBox();
    note.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.4023f, 13.9243f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(13.8203f, 13.9243f, PathPointFlags.LineTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    pdfPathObject2.CalcBoundingBox();
    note.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.4019f, 11.2207f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(13.8199f, 11.2207f, PathPointFlags.LineTo));
    pdfPathObject3.FillColor = fillColor;
    pdfPathObject3.StrokeColor = strokeColor;
    pdfPathObject3.CalcBoundingBox();
    note.Add(pdfPathObject3);
    PdfPathObject pdfPathObject4 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(4.4023f, 8.5176f, PathPointFlags.MoveTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(13.8203f, 8.5176f, PathPointFlags.LineTo));
    pdfPathObject4.FillColor = fillColor;
    pdfPathObject4.StrokeColor = strokeColor;
    pdfPathObject4.CalcBoundingBox();
    note.Add(pdfPathObject4);
    PdfPathObject pdfPathObject5 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(4.4023f, 5.8135f, PathPointFlags.MoveTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(13.8203f, 5.8135f, PathPointFlags.LineTo));
    pdfPathObject5.FillColor = fillColor;
    pdfPathObject5.StrokeColor = strokeColor;
    pdfPathObject5.CalcBoundingBox();
    note.Add(pdfPathObject5);
    return note;
  }

  public static List<PdfPathObject> CreateComment(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> comment = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.74f, 17.7068f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.26f, 17.7068f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.7260008f, 17.7068f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.2930002f, 17.2748f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.2930002f, 16.7398f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.2930002f, 1.2598f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.2930002f, 0.7258f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.7260008f, 0.2928004f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.26f, 0.2928004f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.74f, 0.2928004f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(17.274f, 0.2928004f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(17.707f, 0.7258f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(17.707f, 1.2598f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(17.707f, 16.7398f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(17.707f, 17.2748f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(17.274f, 17.7068f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.74f, 17.7068f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject1.FillColor = new FS_COLOR(0);
    pdfPathObject1.StrokeColor = strokeColor;
    pdfPathObject1.CalcBoundingBox();
    comment.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9f, 5.0908f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.858f, 5.0908f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.72f, 5.0988f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.582f, 5.1058f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.801f, 3.1218f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.445f, 2.8488f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.358f, 3.6708f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.976f, 4.2288f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.084f, 5.2018f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.046f, 6.0068f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.342f, 6.8858f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.228f, 8.3128f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.228f, 9.9298f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.228f, 12.5998f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.26f, 14.7648f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9f, 14.7648f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.74f, 14.7648f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(15.772f, 12.5998f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(15.772f, 9.9298f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(15.772f, 7.2578f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.74f, 5.0908f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9f, 5.0908f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(16.74f, 17.7068f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.26f, 17.7068f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.7260008f, 17.7068f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.2930002f, 17.2748f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.2930002f, 16.7398f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.2930002f, 1.2598f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.2930002f, 0.7258f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.7260008f, 0.2928004f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.26f, 0.2928004f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(16.74f, 0.2928004f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(17.274f, 0.2928004f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(17.707f, 0.7258f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(17.707f, 1.2598f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(17.707f, 16.7398f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(17.707f, 17.2748f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(17.274f, 17.7068f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(16.74f, 17.7068f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    pdfPathObject2.CalcBoundingBox();
    comment.Add(pdfPathObject2);
    return comment;
  }

  public static List<PdfPathObject> CreateKey(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> key = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(6.501f, 17.8109f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.957f, 17.8109f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.896f, 15.7499f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.896f, 13.2069f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.896f, 11.3049f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.051f, 9.6719f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.698f, 8.9709f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.698f, 7.9609f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.705f, 6.9539f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.604f, 5.8529f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.823f, 4.633901f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.905f, 3.7149f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.898f, 2.723901f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.021f, 1.8439f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(6.415f, 0.1899004f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(8.228f, 1.7419f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(8.228f, 8.9409f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(8.237f, 8.944901f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(8.25f, 8.9489f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(8.26f, 8.9519f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(9.929f, 9.6429f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(11.104f, 11.2879f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(11.104f, 13.2069f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(11.104f, 15.7499f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(9.042f, 17.8109f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(6.501f, 17.8109f, PathPointFlags.BezierTo));
    pdfPathObject1.FillColor = new FS_COLOR(0);
    pdfPathObject1.StrokeColor = strokeColor;
    pdfPathObject1.CalcBoundingBox();
    key.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.5f, 12.6729f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.424f, 12.6729f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.55f, 13.5469f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.55f, 14.6229f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.55f, 15.7009f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.424f, 15.9789f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.5f, 15.9789f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(7.577f, 15.9789f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.45f, 15.7009f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.45f, 14.6229f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.45f, 13.5469f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(7.577f, 12.6729f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.5f, 12.6729f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.501f, 17.8109f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.957f, 17.8109f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.896f, 15.7499f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.896f, 13.2069f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.896f, 11.3049f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.051f, 9.6719f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.698f, 8.9709f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.698f, 7.9609f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.705f, 6.9539f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.604f, 5.8529f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.823f, 4.633901f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.905f, 3.7149f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.898f, 2.723901f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.021f, 1.8439f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.415f, 0.1899004f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.228f, 1.7419f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.228f, 8.9409f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.237f, 8.944901f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.25f, 8.9489f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.26f, 8.9519f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.929f, 9.6429f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.104f, 11.2879f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.104f, 13.2069f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.104f, 15.7499f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.042f, 17.8109f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.501f, 17.8109f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    pdfPathObject2.CalcBoundingBox();
    key.Add(pdfPathObject2);
    return key;
  }

  public static List<PdfPathObject> CreateHelp(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> help = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(10.0005f, 19.9167f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.5575f, 19.9167f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.1454992f, 15.5037f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.1454992f, 10.0607f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.1454992f, 4.6187f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.5575f, 0.2047005f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(10.0005f, 0.2047005f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(15.4425f, 0.2047005f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.8555f, 4.6187f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.8555f, 10.0607f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.8555f, 15.5037f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(15.4425f, 19.9167f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(10.0005f, 19.9167f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject1.FillColor = new FS_COLOR(0);
    pdfPathObject1.StrokeColor = strokeColor;
    pdfPathObject1.CalcBoundingBox();
    help.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.1465f, 10.5137f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.4645f, 9.757701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.1885f, 9.0417f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.2085f, 8.2117f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.2085f, 7.881701f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.761499f, 7.881701f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.7435f, 8.359701f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(139f / 16f, 9.297701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.9995f, 10.2547f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.8305f, 11.2297f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.4175f, 11.9467f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.8955f, 12.5357f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.8955f, 13.1607f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.8955f, 13.8047f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.4725f, 14.2287f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.5525f, 14.2647f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.9445f, 14.2647f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.209499f, 14.0447f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(7.729499f, 13.7137f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(7.1055f, 15.7187f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1819f * (float) Math.PI / 734f, 16.1047f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.872499f, 16.4727f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.1785f, 16.4727f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.6065f, 16.4727f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(13.7095f, 15.1297f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(13.7095f, 13.6027f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(13.7095f, 12.2047f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.8455f, 11.2847f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.1465f, 10.5137f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.919499f, 3.650701f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.9015f, 3.650701f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.9445f, 3.650701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.282499f, 4.367701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.282499f, 5.324701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.282499f, 6.3177f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.964499f, 6.997701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.919499f, 6.997701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.9135f, 6.997701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.5575f, 6.3177f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.5765f, 5.324701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.5765f, 4.367701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.9135f, 3.650701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.919499f, 3.650701f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.0005f, 19.9167f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.5575f, 19.9167f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.1454992f, 15.5037f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.1454992f, 10.0607f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.1454992f, 4.6187f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.5575f, 0.2047005f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.0005f, 0.2047005f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(15.4425f, 0.2047005f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.8555f, 4.6187f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.8555f, 10.0607f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.8555f, 15.5037f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(15.4425f, 19.9167f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.0005f, 19.9167f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    pdfPathObject2.CalcBoundingBox();
    help.Add(pdfPathObject2);
    return help;
  }

  public static List<PdfPathObject> CreateNewParagraph(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> newParagraph = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(6.4995f, 20f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.2945004f, 7.287f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(12.7045f, 7.287f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(6.4995f, 20f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject1.FillColor = fillColor;
    pdfPathObject1.StrokeColor = strokeColor;
    pdfPathObject1.CalcBoundingBox();
    newParagraph.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1909f, 6.2949f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.4689f, 6.2949f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.5439f, 6.2949f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.5529f, 6.2749f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.5819f, 6.2289f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.3189f, 4.9319f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.9709f, 2.0199f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.1569f, 1.5819f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.1759f, 1.5819f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.1669f, 1.8419f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.1479f, 2.3849f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.1479f, 3.1579f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.1479f, 6.2189f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.1479f, 6.2749f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.1669f, 6.2949f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.2319f, 6.2949f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.1469f, 6.2949f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.2119f, 6.2949f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.2309f, 6.2659f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.2309f, 6.2109f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.2309f, 0.2459002f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.2309f, 0.1819f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.2119f, 0.1619f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.1379f, 0.1619f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.8859f, 0.1619f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.8119f, 0.1619f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.8019f, 0.1819f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.7649f, 0.2289f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.2429f, 1.3399f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.5439f, 4.2319f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1619f, 5.1089f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1519f, 5.1089f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1899f, 4.6149f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1899f, 4.1489f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1989f, 3.2699f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1989f, 0.2459002f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1989f, 0.1908998f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1799f, 0.1619f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.1239f, 0.1619f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1999f, 0.1619f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1449f, 0.1619f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1159f, 0.1719003f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1159f, 0.2459002f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1159f, 6.2289f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1159f, 6.2749f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1349f, 6.2949f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.1909f, 6.2949f, PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    pdfPathObject2.CalcBoundingBox();
    newParagraph.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1367f, 3.0273f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.211699f, 3.0273f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.3517f, 3.0193f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.7817f, 3.0193f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(10.5367f, 3.0193f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.2557f, 3.3083f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.2557f, 4.2403f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.2557f, 4.9963f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(10.7697f, 5.408299f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.873699f, 5.408299f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.4907f, 5.408299f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.211699f, 5.3983f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1367f, 5.3883f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1367f, 3.0273f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.9907f, 6.2283f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.9907f, 6.2653f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.0077f, 6.2953f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.0547f, 6.2953f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.4277f, 6.3023f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1567f, 6.3123f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.8657f, 6.3123f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.7497f, 6.3123f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.3847f, 5.3413f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.3947f, 4.2593f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.3947f, 2.7573f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.1437f, 2.1133f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.7437f, 2.1133f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.463699f, 2.1133f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1937f, 2.1133f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1367f, 2.1233f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1367f, 0.2382998f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1367f, 0.1912999f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.1177f, 0.1622999f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.062699f, 0.1622999f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.0547f, 0.1622999f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.0177f, 0.1622999f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.9907f, 0.1812999f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.9907f, 0.2282999f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.9907f, 6.2283f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject3.FillColor = fillColor;
    pdfPathObject3.StrokeColor = strokeColor;
    pdfPathObject3.CalcBoundingBox();
    newParagraph.Add(pdfPathObject3);
    return newParagraph;
  }

  public static List<PdfPathObject> CreateParagraph(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> paragraph = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.6973f, 10.0005f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.6973f, 4.664499f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(15.3713f, 0.3384991f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(10.0343f, 0.3384991f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.6993f, 0.3384991f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.3733006f, 4.664499f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.3733006f, 10.0005f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.3733006f, 15.3355f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.6993f, 19.6625f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(10.0343f, 19.6625f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(15.3713f, 19.6625f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.6973f, 15.3355f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.6973f, 10.0005f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject1.FillColor = new FS_COLOR(0);
    pdfPathObject1.StrokeColor = strokeColor;
    pdfPathObject1.CalcBoundingBox();
    paragraph.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.6973f, 10.0005f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.6973f, 4.664499f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(15.3713f, 0.3384991f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.0343f, 0.3384991f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.6993f, 0.3384991f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.3733006f, 4.664499f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.3733006f, 10.0005f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.3733006f, 15.3355f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.6993f, 19.6625f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(10.0343f, 19.6625f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(15.3713f, 19.6625f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.6973f, 15.3355f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.6973f, 10.0005f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    pdfPathObject2.CalcBoundingBox();
    paragraph.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.6787f, 2.6582f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(10.5377f, 2.6582f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(10.4517f, 2.6582f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(10.4347f, 2.7102f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(10.4517f, 2.7972f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.0227f, 3.8152f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.1587f, 5.1632f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.1587f, 5.9752f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.1587f, 6.2522f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.8457f, 6.4412f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(6.2377f, 7.4962f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(6.2377f, 10.9672f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(6.2377f, 13.4362f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.964701f, 15.2842f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.1087f, 15.6822f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.1437f, 16.1662f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.2977f, 16.7872f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.4367f, 17.0472f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.4717f, 17.0982f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.5047f, 17.1332f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.5747f, 17.1332f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.7667f, 17.1332f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.8347f, 17.1332f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.8697f, 17.1162f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.8537f, 17.0302f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.7837f, 16.7532f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.5597f, 15.7852f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.5597f, 15.0602f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.5597f, 12.0892f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.6107f, 9.982201f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.6287f, 6.7182f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.6287f, 4.9562f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.3867f, 3.4712f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.8677f, 2.7282f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.8337f, 2.6922f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.7817f, 2.6582f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(11.6787f, 2.6582f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject3.FillColor = fillColor;
    pdfPathObject3.StrokeColor = strokeColor;
    pdfPathObject3.CalcBoundingBox();
    paragraph.Add(pdfPathObject3);
    return paragraph;
  }

  public static List<PdfPathObject> CreateInsert(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> insert = new List<PdfPathObject>();
    PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject.Path.Add(new FS_PATHPOINTF(8.5386f, 19.8545f, PathPointFlags.MoveTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.1485996f, 0.1354961f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(16.9266f, 0.1354961f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(8.5386f, 19.8545f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject.FillColor = fillColor;
    pdfPathObject.StrokeColor = strokeColor;
    pdfPathObject.CalcBoundingBox();
    insert.Add(pdfPathObject);
    return insert;
  }

  public static List<PdfPathObject> CreateStar(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> star = new List<PdfPathObject>();
    PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject.Path.Add(new FS_PATHPOINTF(9.999f, 18.8838f, PathPointFlags.MoveTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(13.05f, 12.7058f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(19.866f, 11.7158f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(14.933f, 6.905799f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(16.098f, 0.115799f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(9.999f, 3.321798f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.901999f, 0.115799f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.066f, 6.905799f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.1329994f, 11.7158f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.951f, 12.7058f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(9.999f, 18.8838f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject.FillColor = fillColor;
    pdfPathObject.StrokeColor = strokeColor;
    pdfPathObject.CalcBoundingBox();
    star.Add(pdfPathObject);
    return star;
  }

  public static List<PdfPathObject> CreatePaperclip(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> paperclip = new List<PdfPathObject>();
    PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.51f, 13.63f, PathPointFlags.MoveTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.51f, 13.25f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.48f, 4.38f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.48f, 3.74f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.48f, 3.29f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.49f, 1.93f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.38f, 1.05f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.89f, 0.55f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.59f, 0.31f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.45f, 0.32f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.46f, 0.36f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.6f, 1.61f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.57f, 3.76f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.56f, 4.66f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.57f, 10.39f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.57f, 10.45f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.57f, 10.7f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.36f, 10.9f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(6.11f, 10.9f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.86f, 10.9f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.65f, 10.7f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.65f, 10.44f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.65f, 10.21f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.64f, 4.66f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.65f, 3.75f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.67f, 2.09f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.95f, 1.27f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.44f, 1.24f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.83f, 1.23f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.35f, 1.39f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.03f, 1.71f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.4f, 2.32f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.4f, 3.39f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.4f, 3.75f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.4f, 4.37f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.43f, 13.24f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.43f, 13.63f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.43f, 13.97f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.52f, 15.65f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.03f, 15.65f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.91f, 15.65f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.29f, 15.09f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.29f, 13.77f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.29f, 13.63f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.3f, 13.3f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.28f, 9.3f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.27f, 7.24f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.27f, 7.23f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.27f, 7.22f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.27f, 7.21f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.28f, 7f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.25f, 6.32f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.96f, 6.03f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.87f, 5.94f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.76f, 5.89f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.6f, 5.9f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.85f, 5.91f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.86f, 7.23f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.86f, 7.24f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.84f, 10.81f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.83f, 11.07f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.63f, 11.27f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.37f, 11.27f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.12f, 11.27f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.92f, 11.06f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.92f, 10.81f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.94f, 7.24f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.93f, 6.47f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(2.26f, 5f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.59f, 4.98f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.99f, 4.97f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.35f, 5.12f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.62f, 5.4f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.21f, 6.01f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.2f, 7.06f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.19f, 7.24f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.19f, 7.5f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.22f, 13.24f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.2f, 13.66f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.2f, 13.77f, PathPointFlags.LineTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(5.21f, 14.92f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.92f, 15.61f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.51f, 16.03f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(4.08f, 16.45f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.53f, 16.57f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(3.03f, 16.57f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(1.05f, 16.57f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.52f, 14.72f, PathPointFlags.BezierTo));
    pdfPathObject.Path.Add(new FS_PATHPOINTF(0.51f, 13.63f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject.FillColor = fillColor;
    pdfPathObject.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject.Handle, 0.5f);
    pdfPathObject.CalcBoundingBox();
    paperclip.Add(pdfPathObject);
    return paperclip;
  }

  public static List<PdfPathObject> CreateGraph(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> graph = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.65f, 0.24f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.27f, 0.24f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(0.27f, 19.62f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.65f, 19.62f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.65f, 0.24f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject1.FillColor = fillColor;
    pdfPathObject1.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject1.Handle, 0.5f);
    pdfPathObject1.CalcBoundingBox();
    graph.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.429999f, 1.35f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.34f, 1.35f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(6.34f, 10.24f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.429999f, 10.24f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.429999f, 1.35f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject2.Handle, 0.5f);
    pdfPathObject2.CalcBoundingBox();
    graph.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.98f, 1.35f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(1.89f, 1.35f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(1.89f, 14.53f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.98f, 14.53f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.98f, 1.35f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject3.FillColor = fillColor;
    pdfPathObject3.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject3.Handle, 0.5f);
    pdfPathObject3.CalcBoundingBox();
    graph.Add(pdfPathObject3);
    PdfPathObject pdfPathObject4 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(13.88f, 1.35f, PathPointFlags.MoveTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(10.79f, 1.35f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(10.79f, 17.6f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(13.88f, 17.6f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(13.88f, 1.35f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject4.FillColor = fillColor;
    pdfPathObject4.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject4.Handle, 0.5f);
    pdfPathObject4.CalcBoundingBox();
    graph.Add(pdfPathObject4);
    PdfPathObject pdfPathObject5 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(18.34f, 1.35f, PathPointFlags.MoveTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(15.24f, 1.35f, PathPointFlags.LineTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(15.24f, 13.16f, PathPointFlags.LineTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(18.34f, 13.16f, PathPointFlags.LineTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(18.34f, 1.35f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject5.FillColor = fillColor;
    pdfPathObject5.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject5.Handle, 0.5f);
    pdfPathObject5.CalcBoundingBox();
    graph.Add(pdfPathObject5);
    PdfPathObject pdfPathObject6 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(8.89f, 1.9f, PathPointFlags.MoveTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(5.79f, 1.9f, PathPointFlags.LineTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(5.79f, 10.78f, PathPointFlags.LineTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(8.89f, 10.78f, PathPointFlags.LineTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(8.89f, 1.9f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject6.FillColor = fillColor;
    pdfPathObject6.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject6.Handle, 0.5f);
    pdfPathObject6.CalcBoundingBox();
    graph.Add(pdfPathObject6);
    PdfPathObject pdfPathObject7 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(4.44f, 1.9f, PathPointFlags.MoveTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(1.34f, 1.9f, PathPointFlags.LineTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(1.34f, 15.07f, PathPointFlags.LineTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(4.44f, 15.07f, PathPointFlags.LineTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(4.44f, 1.9f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject7.FillColor = fillColor;
    pdfPathObject7.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject7.Handle, 0.5f);
    pdfPathObject7.CalcBoundingBox();
    graph.Add(pdfPathObject7);
    PdfPathObject pdfPathObject8 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(13.34f, 1.9f, PathPointFlags.MoveTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(10.25f, 1.9f, PathPointFlags.LineTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(10.25f, 18.14f, PathPointFlags.LineTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(13.34f, 18.14f, PathPointFlags.LineTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(13.34f, 1.9f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject8.FillColor = fillColor;
    pdfPathObject8.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject8.Handle, 0.5f);
    pdfPathObject8.CalcBoundingBox();
    graph.Add(pdfPathObject8);
    PdfPathObject pdfPathObject9 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject9.Path.Add(new FS_PATHPOINTF(17.79f, 1.9f, PathPointFlags.MoveTo));
    pdfPathObject9.Path.Add(new FS_PATHPOINTF(14.7f, 1.9f, PathPointFlags.LineTo));
    pdfPathObject9.Path.Add(new FS_PATHPOINTF(14.7f, 13.7f, PathPointFlags.LineTo));
    pdfPathObject9.Path.Add(new FS_PATHPOINTF(17.79f, 13.7f, PathPointFlags.LineTo));
    pdfPathObject9.Path.Add(new FS_PATHPOINTF(17.79f, 1.9f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject9.FillColor = fillColor;
    pdfPathObject9.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject9.Handle, 0.5f);
    pdfPathObject9.CalcBoundingBox();
    graph.Add(pdfPathObject9);
    return graph;
  }

  public static List<PdfPathObject> CreatePushpin(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> pushpin = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(6.63f, 6.35f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(7.04f, 0.36f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(7.4f, 6.35f, PathPointFlags.LineTo));
    pdfPathObject1.FillColor = fillColor;
    pdfPathObject1.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject1.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject1.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject1.Handle, LineCap.Round);
    pdfPathObject1.CalcBoundingBox();
    pushpin.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.95f, 19.58f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.63f, 19.53f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.78f, 16.31f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.78f, 16.31f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.78f, 10.26f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.78f, 10.26f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(14.37f, 7.11f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(13.51f, 6.42f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.74f, 6.16f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.4f, 6.22f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.52f, 6.41f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(-0.51f, 7.17f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.33f, 10.25f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.33f, 10.25f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.33f, 16.31f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.33f, 16.31f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.42f, 19.58f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.16f, 19.58f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.91f, 19.58f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.27f, 19.63f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(11.95f, 19.58f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject2.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject2.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject2.Handle, LineCap.Round);
    pdfPathObject2.CalcBoundingBox();
    pushpin.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.91f, 16.39f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.91f, 16.39f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(6.08f, 16.21f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.08f, 16.22f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.02f, 16.23f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.250001f, 16.39f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.250001f, 16.39f, PathPointFlags.BezierTo));
    pdfPathObject3.FillColor = fillColor;
    pdfPathObject3.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject3.Handle, 0.1f);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject3.Handle, LineCap.Square);
    pdfPathObject3.CalcBoundingBox();
    pushpin.Add(pdfPathObject3);
    PdfPathObject pdfPathObject4 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(4.93f, 10.11f, PathPointFlags.MoveTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(4.93f, 10.11f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(6.16f, 10.28f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.29f, 10.28f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(8.4f, 10.29f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(9.160001f, 10.11f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(9.160001f, 10.11f, PathPointFlags.BezierTo));
    pdfPathObject4.FillColor = fillColor;
    pdfPathObject4.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject4.Handle, 0.1f);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject4.Handle, LineCap.Square);
    pdfPathObject4.CalcBoundingBox();
    pushpin.Add(pdfPathObject4);
    return pushpin;
  }

  public static List<PdfPathObject> CreateTag(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> tag = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(18.32f, 15.63f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.48f, 15.63f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.78f, 15.63f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.41f, 15.03f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.41f, 15.03f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.75f, 9.23f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.54f, 8.89f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.44f, 8.44f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.44f, 8f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.44f, 7.97f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.44f, 7.54f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.54f, 7.09f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.75f, 6.75f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.42f, 0.9400001f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.42f, 0.9400001f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.78f, 0.35f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.48f, 0.35f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(18.32f, 0.35f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.02f, 0.35f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.58f, 0.95f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.58f, 1.69f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.58f, 14.29f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.58f, 15.03f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(19.02f, 15.63f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(18.32f, 15.63f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.73f, 7f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.18f, 7f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(2.74f, 7.44f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(2.74f, 7.98f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(2.74f, 8.52f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.18f, 8.96f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.73f, 8.96f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.28f, 8.96f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.72f, 8.52f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.72f, 7.98f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.72f, 7.44f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.28f, 7f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.73f, 7f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject1.FillColor = fillColor;
    pdfPathObject1.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject1.Handle, 0.5f);
    pdfPathObject1.CalcBoundingBox();
    tag.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(18.32f, 15.63f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.48f, 15.63f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.78f, 15.63f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.41f, 15.03f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.41f, 15.03f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.75f, 9.23f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.54f, 8.89f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.44f, 8.44f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.44f, 8f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.44f, 7.97f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.44f, 7.54f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.54f, 7.09f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.75f, 6.75f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.42f, 0.9400001f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.42f, 0.9400001f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.78f, 0.35f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.48f, 0.35f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(18.32f, 0.35f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.02f, 0.35f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.58f, 0.95f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.58f, 1.69f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.58f, 14.29f, PathPointFlags.LineTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.58f, 15.03f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.02f, 15.63f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(18.32f, 15.63f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.73f, 7f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.18f, 7f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.74f, 7.44f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.74f, 7.98f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.74f, 8.52f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.18f, 8.96f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.73f, 8.96f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.28f, 8.96f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.72f, 8.52f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.72f, 7.98f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.72f, 7.44f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.28f, 7f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.73f, 7f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject2.Handle, 0.5f);
    pdfPathObject2.CalcBoundingBox();
    tag.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.52f, 11.86f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.4f, 11.86f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.3f, 11.71f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.3f, 11.53f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.3f, 11.34f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.4f, 11.19f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.52f, 11.19f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(16.28f, 11.19f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(16.4f, 11.19f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(16.5f, 11.34f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(16.5f, 11.53f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(16.5f, 11.71f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(16.4f, 11.86f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(16.28f, 11.86f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(7.52f, 11.86f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject3.FillColor = strokeColor;
    pdfPathObject3.StrokeColor = fillColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject3.Handle, 0.5f);
    pdfPathObject3.CalcBoundingBox();
    tag.Add(pdfPathObject3);
    PdfPathObject pdfPathObject4 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.52f, 8.3f, PathPointFlags.MoveTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.4f, 8.3f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.3f, 8.150001f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.3f, 7.96f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.3f, 7.78f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.4f, 7.63f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.52f, 7.63f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(16.28f, 7.63f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(16.4f, 7.63f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(16.5f, 7.78f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(16.5f, 7.96f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(16.5f, 8.150001f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(16.4f, 8.3f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(16.28f, 8.3f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.52f, 8.3f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject4.FillColor = strokeColor;
    pdfPathObject4.StrokeColor = fillColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject4.Handle, 0.5f);
    pdfPathObject4.CalcBoundingBox();
    tag.Add(pdfPathObject4);
    PdfPathObject pdfPathObject5 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(7.52f, 4.74f, PathPointFlags.MoveTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(7.4f, 4.74f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(7.3f, 4.59f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(7.3f, 4.4f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(7.3f, 4.21f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(7.4f, 4.06f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(7.52f, 4.06f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(13.94f, 4.06f, PathPointFlags.LineTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(14.06f, 4.06f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(14.16f, 4.21f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(14.16f, 4.4f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(14.16f, 4.59f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(14.06f, 4.74f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(13.94f, 4.74f, PathPointFlags.BezierTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(7.52f, 4.74f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject5.FillColor = strokeColor;
    pdfPathObject5.StrokeColor = fillColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject5.Handle, 0.5f);
    pdfPathObject5.CalcBoundingBox();
    tag.Add(pdfPathObject5);
    PdfPathObject pdfPathObject6 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(3.03f, 8.77f, PathPointFlags.MoveTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(3.03f, 8.77f, PathPointFlags.BezierTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(3.47f, 8.41f, PathPointFlags.BezierTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(3.32f, 7.89f, PathPointFlags.BezierTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(3.17f, 7.36f, PathPointFlags.BezierTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(1.16f, 7.29f, PathPointFlags.BezierTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(0.68f, 6.3f, PathPointFlags.BezierTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(0.2f, 5.3f, PathPointFlags.BezierTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(0.68f, 2.9f, PathPointFlags.BezierTo));
    pdfPathObject6.Path.Add(new FS_PATHPOINTF(0.68f, 2.9f, PathPointFlags.BezierTo));
    pdfPathObject6.FillColor = fillColor;
    pdfPathObject6.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject6.Handle, 0.5f);
    pdfPathObject6.CalcBoundingBox();
    PdfPathObject pdfPathObject7 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(2.45f, 1.9f, PathPointFlags.MoveTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(2.45f, 1.9f, PathPointFlags.BezierTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(1.08f, 5.77f, PathPointFlags.BezierTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(0.68f, 7.47f, PathPointFlags.BezierTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(0.33f, 8.96f, PathPointFlags.BezierTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(1.71f, 9.160001f, PathPointFlags.BezierTo));
    pdfPathObject7.Path.Add(new FS_PATHPOINTF(1.71f, 9.160001f, PathPointFlags.BezierTo));
    pdfPathObject7.FillColor = fillColor;
    pdfPathObject7.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject7.Handle, 0.5f);
    pdfPathObject7.CalcBoundingBox();
    PdfPathObject pdfPathObject8 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(1.28f, 6.43f, PathPointFlags.MoveTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(1.28f, 6.32f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(1.08f, 6.24f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(0.83f, 6.24f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(0.58f, 6.24f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(0.38f, 6.32f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(0.38f, 6.43f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(0.38f, 6.54f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(0.58f, 6.63f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(0.83f, 6.63f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(1.08f, 6.63f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(1.28f, 6.54f, PathPointFlags.BezierTo));
    pdfPathObject8.Path.Add(new FS_PATHPOINTF(1.28f, 6.43f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject8.FillColor = fillColor;
    pdfPathObject8.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject8.Handle, 0.5f);
    pdfPathObject8.CalcBoundingBox();
    return tag;
  }

  public static List<PdfPathObject> CreateEar(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> ear = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, false);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.48f, 1.98f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.27f, 0.01f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(8.45f, 0.01f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(8.81f, 1.98f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(9.71f, 6.94f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(16.08f, 7.82f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(15.12f, 14.59f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(14.72f, 17.35f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(12.75f, 19.32f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(8.81f, 19.32f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.48f, 19.32f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.69f, 14.59f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.69f, 14.59f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.69f, 14.59f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.08f, 12.58f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.15f, 8.91f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(5.21f, 5.23f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.48f, 1.98f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.48f, 1.98f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject1.FillColor = fillColor;
    pdfPathObject1.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject1.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject1.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject1.Handle, LineCap.Round);
    pdfPathObject1.CalcBoundingBox();
    ear.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.69f, 14.59f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(3.69f, 14.59f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.48f, 19.32f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.81f, 19.32f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(12.75f, 19.32f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(14.72f, 17.35f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(15.12f, 14.59f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(16.08f, 7.82f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(9.71f, 6.94f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.81f, 1.98f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(8.45f, 0.01f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.27f, 0.01f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.48f, 1.98f, PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject2.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject2.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject2.Handle, LineCap.Round);
    pdfPathObject2.CalcBoundingBox();
    ear.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(5.31f, 14.64f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(5.31f, 14.64f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(5.31f, 17.79f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(9.25f, 17.79f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(13.59f, 17.79f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(13.46f, 13.92f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(13.46f, 14.29f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(13.46f, 14.29f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(13.77f, 13.05f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(13.29f, 11.25f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(12.82f, 9.45f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.820001f, 5.84f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(8.820001f, 5.84f, PathPointFlags.BezierTo));
    pdfPathObject3.FillColor = fillColor;
    pdfPathObject3.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject3.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject3.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject3.Handle, LineCap.Round);
    pdfPathObject3.CalcBoundingBox();
    ear.Add(pdfPathObject3);
    return ear;
  }

  public static List<PdfPathObject> CreateMic(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> mic = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.45f, 11.9f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(1.94f, 3.44f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(3.72f, 3.47f, PathPointFlags.LineTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4.3f, 11.9f, PathPointFlags.LineTo));
    pdfPathObject1.FillColor = fillColor;
    pdfPathObject1.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject1.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject1.Handle, LineJoin.Round);
    pdfPathObject1.CalcBoundingBox();
    mic.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.26f, 14.78f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.26f, 13.47f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.2f, 12.41f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.89f, 12.41f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.58f, 12.41f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.52f, 13.47f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.52f, 14.78f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(0.52f, 16.08f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(1.58f, 17.14f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(2.89f, 17.14f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(4.2f, 17.14f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.26f, 16.08f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(5.26f, 14.78f, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = strokeColor;
    pdfPathObject2.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject2.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject2.Handle, LineJoin.Round);
    pdfPathObject2.CalcBoundingBox();
    mic.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.43f, 12.12f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(1.32f, 12.14f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(1.19f, 12.92f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.6f, 12.92f, PathPointFlags.LineTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(4.43f, 12.12f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject3.FillColor = strokeColor;
    pdfPathObject3.StrokeColor = fillColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject3.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject3.Handle, LineJoin.Round);
    pdfPathObject3.CalcBoundingBox();
    mic.Add(pdfPathObject3);
    PdfPathObject pdfPathObject4 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(12.42f, 0.59f, PathPointFlags.MoveTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(11.37f, 2.03f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(9.28f, 1.94f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(7.71f, 1.94f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(6.87f, 1.94f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(5.39f, 1.76f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(5.06f, 2.77f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(4.74f, 3.75f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(6.18f, 4.37f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(5.8f, 3.32f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(5.17f, 1.56f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(3.09f, 2.79f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(1.99f, 2.7f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(1.99f, 2.76f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(1.85f, 3.44f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(1.85f, 3.44f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(2.08f, 3.42f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(3.79f, 3.44f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(3.79f, 3.44f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(3.65f, 2.72f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(3.65f, 2.72f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(2.18f, 2.72f, PathPointFlags.BezierTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(1.87f, 2.72f, PathPointFlags.BezierTo));
    pdfPathObject4.FillColor = fillColor;
    pdfPathObject4.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject4.Handle, 0.2f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject4.Handle, LineJoin.Round);
    pdfPathObject4.CalcBoundingBox();
    mic.Add(pdfPathObject4);
    return mic;
  }

  public static List<PdfPathObject> CreateSpeaker(FS_COLOR fillColor, FS_COLOR strokeColor)
  {
    List<PdfPathObject> speaker = new List<PdfPathObject>();
    PdfPathObject pdfPathObject1 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(12.08f, 9.68f, PathPointFlags.MoveTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(12.08f, 9.68f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(4125f * (float) Math.PI / 887f, 7.9f, PathPointFlags.BezierTo));
    pdfPathObject1.Path.Add(new FS_PATHPOINTF(12.08f, 5.37f, PathPointFlags.BezierTo));
    pdfPathObject1.FillColor = fillColor;
    pdfPathObject1.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject1.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject1.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject1.Handle, LineCap.Round);
    pdfPathObject1.CalcBoundingBox();
    speaker.Add(pdfPathObject1);
    PdfPathObject pdfPathObject2 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(13.77f, 11.99f, PathPointFlags.MoveTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(13.77f, 11.99f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(19.05f, 8.29f, PathPointFlags.BezierTo));
    pdfPathObject2.Path.Add(new FS_PATHPOINTF(13.77f, 3.01f, PathPointFlags.BezierTo));
    pdfPathObject2.FillColor = fillColor;
    pdfPathObject2.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject2.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject2.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject2.Handle, LineCap.Round);
    pdfPathObject2.CalcBoundingBox();
    speaker.Add(pdfPathObject2);
    PdfPathObject pdfPathObject3 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(15.74f, 14.56f, PathPointFlags.MoveTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(15.74f, 14.56f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(24.1f, 8.7f, PathPointFlags.BezierTo));
    pdfPathObject3.Path.Add(new FS_PATHPOINTF(15.74f, 0.35f, PathPointFlags.BezierTo));
    pdfPathObject3.FillColor = fillColor;
    pdfPathObject3.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject3.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject3.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject3.Handle, LineCap.Round);
    pdfPathObject3.CalcBoundingBox();
    speaker.Add(pdfPathObject3);
    PdfPathObject pdfPathObject4 = PdfPathObject.Create(FillModes.Winding, true);
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(9.68f, 14.38f, PathPointFlags.MoveTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(5.43f, 10.05f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(5.43f, 10.05f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(0.38f, 10.05f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(0.38f, 5.01f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(5.43f, 5.01f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(5.43f, 5.03f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(9.68f, 0.7f, PathPointFlags.LineTo));
    pdfPathObject4.Path.Add(new FS_PATHPOINTF(9.68f, 14.38f, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    pdfPathObject4.FillColor = fillColor;
    pdfPathObject4.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject4.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject4.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject4.Handle, LineCap.Round);
    pdfPathObject4.CalcBoundingBox();
    speaker.Add(pdfPathObject4);
    PdfPathObject pdfPathObject5 = PdfPathObject.Create(FillModes.None, true);
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(5.19f, 9.47f, PathPointFlags.MoveTo));
    pdfPathObject5.Path.Add(new FS_PATHPOINTF(5.19f, 5.62f, PathPointFlags.LineTo));
    pdfPathObject5.FillColor = fillColor;
    pdfPathObject5.StrokeColor = strokeColor;
    Pdfium.FPDFPageObj_SetLineWidth(pdfPathObject5.Handle, 0.5f);
    Pdfium.FPDFPageObj_SetLineJoin(pdfPathObject5.Handle, LineJoin.Round);
    Pdfium.FPDFPageObj_SetLineCap(pdfPathObject5.Handle, LineCap.Butt);
    pdfPathObject5.CalcBoundingBox();
    speaker.Add(pdfPathObject5);
    return speaker;
  }

  /// <summary>Get points for a square</summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetSquarePoints(FS_RECTF rect, float lineWidth)
  {
    List<FS_PATHPOINTF> squarePoints = new List<FS_PATHPOINTF>();
    rect = AnnotDrawing.InflateRect(rect, (float) (-(double) lineWidth / 2.0));
    squarePoints.Add(new FS_PATHPOINTF(rect.left, rect.top, PathPointFlags.MoveTo));
    squarePoints.Add(new FS_PATHPOINTF(rect.right, rect.top, PathPointFlags.LineTo));
    squarePoints.Add(new FS_PATHPOINTF(rect.right, rect.bottom, PathPointFlags.LineTo));
    squarePoints.Add(new FS_PATHPOINTF(rect.left, rect.bottom, PathPointFlags.LineTo));
    squarePoints.Add(new FS_PATHPOINTF(rect.left, rect.top, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
    return squarePoints;
  }

  /// <summary>Get points for a circle</summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetCirclePoints(FS_RECTF rect, float lineWidth)
  {
    rect = AnnotDrawing.InflateRect(rect, (float) (-(double) lineWidth / 2.0));
    List<FS_PATHPOINTF> circlePoints = new List<FS_PATHPOINTF>();
    float x = rect.left + rect.Width / 2f;
    float num1 = rect.bottom + rect.Height / 2f;
    double width = (double) rect.Width;
    float height = rect.Height;
    double num2 = width / 2.0;
    float num3 = (float) (width * 2.0 / 3.0);
    float num4 = height / 2f;
    circlePoints.Add(new FS_PATHPOINTF(x, num1 + num4, PathPointFlags.MoveTo));
    circlePoints.Add(new FS_PATHPOINTF(x + num3, num1 + num4, PathPointFlags.BezierTo));
    circlePoints.Add(new FS_PATHPOINTF(x + num3, num1 - num4, PathPointFlags.BezierTo));
    circlePoints.Add(new FS_PATHPOINTF(x, num1 - num4, PathPointFlags.BezierTo));
    circlePoints.Add(new FS_PATHPOINTF(x - num3, num1 - num4, PathPointFlags.BezierTo));
    circlePoints.Add(new FS_PATHPOINTF(x - num3, num1 + num4, PathPointFlags.BezierTo));
    circlePoints.Add(new FS_PATHPOINTF(x, num1 + num4, PathPointFlags.CloseFigure | PathPointFlags.BezierTo));
    return circlePoints;
  }

  /// <summary>Get points for a text highlight</summary>
  /// <param name="qp">The quadrilateral.</param>
  /// <returns>A Collection of points of this figure.</returns>
  private static List<FS_PATHPOINTF> GetHighlightPoints(FS_QUADPOINTSF qp)
  {
    float x = (float) Math.Sqrt(Math.Pow((double) qp.x2 - (double) qp.x4, 2.0) + Math.Pow((double) qp.y2 - (double) qp.y4, 2.0)) / AnnotDrawing.higlightBeizerK;
    double vectorAng1 = AnnotDrawing.GetVectorAng(new FS_POINTF(qp.x1, qp.y1), new FS_POINTF(qp.x2, qp.y2));
    float vectorAng2 = (float) AnnotDrawing.GetVectorAng(new FS_POINTF(qp.x3, qp.y3), new FS_POINTF(qp.x4, qp.y4));
    FS_POINTF fsPointf1 = AnnotDrawing.RotatePoint((float) -vectorAng1, x, 0.0f);
    FS_PATHPOINTF fsPathpointf1 = new FS_PATHPOINTF(qp.x2 + fsPointf1.X, qp.y2 + fsPointf1.Y, PathPointFlags.BezierTo);
    FS_POINTF fsPointf2 = AnnotDrawing.RotatePoint(-vectorAng2, x, 0.0f);
    FS_PATHPOINTF fsPathpointf2 = new FS_PATHPOINTF(qp.x4 + fsPointf2.X, qp.y4 + fsPointf2.Y, PathPointFlags.BezierTo);
    FS_POINTF fsPointf3 = AnnotDrawing.RotatePoint((float) (-vectorAng1 - 3.1415927410125732), x, 0.0f);
    FS_PATHPOINTF fsPathpointf3 = new FS_PATHPOINTF(qp.x1 + fsPointf3.X, qp.y1 + fsPointf3.Y, PathPointFlags.BezierTo);
    FS_POINTF fsPointf4 = AnnotDrawing.RotatePoint((float) (-(double) vectorAng2 - 3.1415927410125732), x, 0.0f);
    FS_PATHPOINTF fsPathpointf4 = new FS_PATHPOINTF(qp.x3 + fsPointf4.X, qp.y3 + fsPointf4.Y, PathPointFlags.BezierTo);
    return new List<FS_PATHPOINTF>()
    {
      new FS_PATHPOINTF(qp.x1, qp.y1, PathPointFlags.MoveTo),
      new FS_PATHPOINTF(qp.x2, qp.y2, PathPointFlags.LineTo),
      fsPathpointf1,
      fsPathpointf2,
      new FS_PATHPOINTF(qp.x4, qp.y4, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(qp.x3, qp.y3, PathPointFlags.LineTo),
      fsPathpointf4,
      fsPathpointf3,
      new FS_PATHPOINTF(qp.x1, qp.y1, PathPointFlags.CloseFigure | PathPointFlags.BezierTo)
    };
  }

  /// <summary>Get points for text strikeout and text underline.</summary>
  /// <param name="qp">The quadrilateral.</param>
  /// <param name="displacePercent">The place where the line should be placed. In percentage of length of side edges(left - (x1;y1)-(x3;y3) and right (x2;y2)-(x4;y4).</param>
  /// <param name="lineWidthPercent">The line width. In percentages of length of least side edge.</param>
  /// <param name="lineWidth">Returns the line width has been calculated.</param>
  /// <returns>An array that contais two points of the underline or strikeout line.</returns>
  public static FS_PATHPOINTF[] GetTextMarkupPoints(
    FS_QUADPOINTSF qp,
    float displacePercent,
    float lineWidthPercent,
    out float lineWidth)
  {
    double x1 = (double) qp.x1 - (double) qp.x3;
    double x2 = (double) qp.y1 - (double) qp.y3;
    double x3 = (double) qp.x2 - (double) qp.x4;
    double x4 = (double) qp.y2 - (double) qp.y4;
    double val1 = Math.Sqrt(Math.Pow(x1, 2.0) + Math.Pow(x2, 2.0));
    double val2 = Math.Sqrt(Math.Pow(x3, 2.0) + Math.Pow(x4, 2.0));
    lineWidth = (float) ((double) lineWidthPercent * Math.Min(val1, val2) / 100.0);
    double a = val1 / ((double) displacePercent * val1 / 100.0);
    if (AnnotDrawing.AreEqual((float) a, 0.0f))
      a = 1.0;
    double num1 = x1 / a;
    double num2 = x2 / a;
    double num3 = val2 / ((double) displacePercent * val2 / 100.0);
    double num4 = x3 / num3;
    double num5 = x4 / num3;
    return new FS_PATHPOINTF[2]
    {
      new FS_PATHPOINTF((double) qp.x3 + num1, (double) qp.y3 + num2, PathPointFlags.MoveTo),
      new FS_PATHPOINTF((double) qp.x4 + num4, (double) qp.y4 + num5, PathPointFlags.LineTo)
    };
  }

  /// <summary>Get points for text squiggly.</summary>
  /// <param name="qp">The quadrilateral.</param>
  /// <param name="displacePercent">The place where the line should be placed. In percentage of length of side edges(left - (x1;y1)-(x3;y3) and right (x2;y2)-(x4;y4).</param>
  /// <param name="lineWidthPercent">The line width. In percentages of length of least side edge.</param>
  /// <param name="lineWidth">Returns the line width has been calculated.</param>
  /// <returns>A Collection of points of this figure.</returns>
  private static List<FS_PATHPOINTF> GetSquigglyPoints(
    FS_QUADPOINTSF qp,
    float displacePercent,
    float lineWidthPercent,
    out float lineWidth)
  {
    FS_PATHPOINTF[] textMarkupPoints = AnnotDrawing.GetTextMarkupPoints(qp, displacePercent, lineWidthPercent, out lineWidth);
    float x1 = textMarkupPoints[0].X;
    float y1 = textMarkupPoints[0].Y;
    float x2 = textMarkupPoints[1].X;
    float y2 = textMarkupPoints[1].Y;
    double vectorAng = AnnotDrawing.GetVectorAng(new FS_POINTF(x1, y1), new FS_POINTF(x2, y2));
    FS_MATRIX fsMatrix1 = new FS_MATRIX(Math.Cos(-vectorAng), -Math.Sin(-vectorAng), Math.Sin(-vectorAng), Math.Cos(-vectorAng), 0.0, 0.0);
    FS_MATRIX fsMatrix2 = new FS_MATRIX(Math.Cos(vectorAng), -Math.Sin(vectorAng), Math.Sin(vectorAng), Math.Cos(vectorAng), 0.0, 0.0);
    fsMatrix1.TransformPoint(ref x1, ref y1);
    fsMatrix1.TransformPoint(ref x2, ref y2);
    float num1 = lineWidth * AnnotDrawing.squiglyKY;
    float num2 = lineWidth * AnnotDrawing.squiglyKX;
    float num3 = (float) (int) (((double) x2 - (double) x1) / (double) num2);
    float num4 = (x2 - x1) / num3;
    List<FS_PATHPOINTF> squigglyPoints = new List<FS_PATHPOINTF>();
    int num5 = 0;
    while ((double) x1 <= (double) x2)
    {
      float x3 = x1;
      float y3 = y1;
      fsMatrix2.TransformPoint(ref x3, ref y3);
      squigglyPoints.Add(new FS_PATHPOINTF(x3, y3, num5 == 0 ? PathPointFlags.MoveTo : PathPointFlags.LineTo));
      if (num5 % 2 == 0)
        y1 += num1;
      else
        y1 -= num1;
      x1 += num4;
      ++num5;
    }
    return squigglyPoints;
  }

  /// <summary>
  /// Creates a collection that contains paths for square annotation. Where border should appear “cloudy”.
  /// </summary>
  /// <param name="rect">object of the FS_RECTF type that defines the figure</param>
  /// <param name="effectIntencity">A number describing the intensity of the effect.</param>
  /// <param name="lineWidth">The border width in points.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetCloudySquarePoints(
    FS_RECTF rect,
    int effectIntencity,
    float lineWidth)
  {
    float num = (float) effectIntencity * AnnotDrawing.CloudyIntencityK;
    rect = AnnotDrawing.InflateRect(rect, (float) (-(double) num - (double) lineWidth / 2.0));
    return AnnotDrawing.GetCloudyLinePoints((IList<FS_POINTF>) new List<FS_POINTF>()
    {
      new FS_POINTF(rect.left, rect.bottom),
      new FS_POINTF(rect.left, rect.top),
      new FS_POINTF(rect.right, rect.top),
      new FS_POINTF(rect.right, rect.bottom),
      new FS_POINTF(rect.left, rect.bottom)
    }, effectIntencity);
  }

  /// <summary>
  /// Creates a collection that contains paths for circle (ellipse) annotation. Where border should appear “cloudy”.
  /// </summary>
  /// <param name="innerRect">Bounding box in which the ellipse should be inscribed.</param>
  /// <param name="effectIntencity">A number describing the intensity of the effect.</param>
  /// <param name="lineWidth">The border width in points.</param>
  /// <returns>A collection of points for this figure and the cloud boundary.</returns>
  public static List<FS_PATHPOINTF> GetCloudyCirclePoints(
    FS_RECTF innerRect,
    int effectIntencity,
    float lineWidth)
  {
    float radius = (float) effectIntencity * AnnotDrawing.CloudyIntencityK;
    float num1 = 2f * radius;
    float num2 = num1 * AnnotDrawing.CloudyLengthDK;
    innerRect = AnnotDrawing.InflateRect(innerRect, (float) (-(double) radius - (double) lineWidth / 2.0));
    float num3 = innerRect.Width / 2f;
    float num4 = innerRect.Height / 2f;
    if ((double) num3 < 1.0)
      num3 = 1f;
    if ((double) num4 < 1.0)
      num4 = 1f;
    float num5 = innerRect.left + innerRect.Width / 2f;
    float num6 = innerRect.bottom + innerRect.Height / 2f;
    float num7 = (float) Math.Atan((double) num1 / (double) Math.Min(num3, num4)) / AnnotDrawing.CloudyStepFactor;
    int num8 = 0;
label_5:
    ++num8;
    List<AnnotDrawing.FS_QLOUDYPOINTF> fsQloudypointfList = new List<AnnotDrawing.FS_QLOUDYPOINTF>();
    fsQloudypointfList.Add(new AnnotDrawing.FS_QLOUDYPOINTF(radius, new FS_POINTF(num3, 0.0f), true));
    fsQloudypointfList.Add(new AnnotDrawing.FS_QLOUDYPOINTF(radius, new FS_POINTF(0.0f, num4), true));
    int num9 = 1;
    for (float num10 = 0.0f; (double) num10 <= Math.PI / 2.0; num10 += num7)
    {
      FS_POINTF fsPointf = new FS_POINTF((double) num3 * Math.Cos((double) num10), (double) num4 * Math.Sin((double) num10));
      if (AnnotDrawing.GetVectorLen(fsQloudypointfList[num9 - 1].Center, fsPointf) >= (double) num2)
      {
        fsQloudypointfList.Insert(num9++, new AnnotDrawing.FS_QLOUDYPOINTF(radius, fsPointf));
        float vectorLen = (float) AnnotDrawing.GetVectorLen(fsQloudypointfList[num9 - 1].Center, fsQloudypointfList[fsQloudypointfList.Count - 1].Center);
        float num11 = num1 / 2f * AnnotDrawing.CloudyLimitFactor;
        if ((double) vectorLen <= (double) num1)
        {
          if (num8 <= AnnotDrawing.CloudyCutout)
          {
            if ((double) vectorLen <= (double) num1 / 2.0 + (double) num11)
            {
              num2 -= num11 / (float) fsQloudypointfList.Count;
              if ((double) num2 > (double) num1 / 2.0 + (double) num11)
                goto label_5;
              break;
            }
            if ((double) vectorLen > (double) num1 - (double) num11)
            {
              num2 += num11 / (float) fsQloudypointfList.Count;
              if ((double) num2 < (double) num1 - (double) num11)
                goto label_5;
              break;
            }
            break;
          }
          break;
        }
      }
    }
    int num12 = fsQloudypointfList.Count - 2;
    for (int index = 0; index <= num12; ++index)
      fsQloudypointfList.Add(new AnnotDrawing.FS_QLOUDYPOINTF(fsQloudypointfList[num12 - index].Radius, new FS_POINTF(-fsQloudypointfList[num12 - index].Center.X, fsQloudypointfList[num12 - index].Center.Y)));
    int num13 = fsQloudypointfList.Count - 2;
    for (int index = 0; index < num13; ++index)
      fsQloudypointfList.Add(new AnnotDrawing.FS_QLOUDYPOINTF(fsQloudypointfList[num13 - index].Radius, new FS_POINTF(fsQloudypointfList[num13 - index].Center.X, -fsQloudypointfList[num13 - index].Center.Y)));
    for (int index = 0; index < fsQloudypointfList.Count; ++index)
    {
      fsQloudypointfList[index].CalcFirstArcPoint(index == fsQloudypointfList.Count - 1 ? fsQloudypointfList[0] : fsQloudypointfList[index + 1]);
      if (index > 0)
        fsQloudypointfList[index].CalcSecondArcPoint(fsQloudypointfList[index - 1]);
    }
    fsQloudypointfList[0].CalcSecondArcPoint(fsQloudypointfList[fsQloudypointfList.Count - 1]);
    List<FS_PATHPOINTF> cloudyCirclePoints = new List<FS_PATHPOINTF>();
    for (int index1 = fsQloudypointfList.Count - 1; index1 >= 0; --index1)
    {
      AnnotDrawing.FS_QLOUDYPOINTF fsQloudypointf = fsQloudypointfList[index1];
      FS_POINTF fsPointf;
      if (index1 == fsQloudypointfList.Count - 1)
      {
        List<FS_PATHPOINTF> fsPathpointfList = cloudyCirclePoints;
        double num14 = (double) num5;
        fsPointf = fsQloudypointf.Points[0];
        double x1 = (double) fsPointf.X;
        double x2 = num14 + x1;
        double num15 = (double) num6;
        fsPointf = fsQloudypointf.Points[0];
        double y1 = (double) fsPointf.Y;
        double y2 = num15 + y1;
        FS_PATHPOINTF fsPathpointf = new FS_PATHPOINTF((float) x2, (float) y2, PathPointFlags.MoveTo);
        fsPathpointfList.Add(fsPathpointf);
      }
      int index2 = 1;
      int index3 = 1;
      while (index2 < fsQloudypointf.Points.Count)
      {
        List<FS_PATHPOINTF> fsPathpointfList1 = cloudyCirclePoints;
        double num16 = (double) num5;
        fsPointf = fsQloudypointf.cPoints[index3 - 1];
        double x3 = (double) fsPointf.X;
        double x4 = num16 + x3;
        double num17 = (double) num6;
        fsPointf = fsQloudypointf.cPoints[index3 - 1];
        double y3 = (double) fsPointf.Y;
        double y4 = num17 + y3;
        FS_PATHPOINTF fsPathpointf1 = new FS_PATHPOINTF((float) x4, (float) y4, PathPointFlags.BezierTo);
        fsPathpointfList1.Add(fsPathpointf1);
        List<FS_PATHPOINTF> fsPathpointfList2 = cloudyCirclePoints;
        double num18 = (double) num5;
        fsPointf = fsQloudypointf.cPoints[index3];
        double x5 = (double) fsPointf.X;
        double x6 = num18 + x5;
        double num19 = (double) num6;
        fsPointf = fsQloudypointf.cPoints[index3];
        double y5 = (double) fsPointf.Y;
        double y6 = num19 + y5;
        FS_PATHPOINTF fsPathpointf2 = new FS_PATHPOINTF((float) x6, (float) y6, PathPointFlags.BezierTo);
        fsPathpointfList2.Add(fsPathpointf2);
        List<FS_PATHPOINTF> fsPathpointfList3 = cloudyCirclePoints;
        double num20 = (double) num5;
        fsPointf = fsQloudypointf.Points[index2];
        double x7 = (double) fsPointf.X;
        double x8 = num20 + x7;
        double num21 = (double) num6;
        fsPointf = fsQloudypointf.Points[index2];
        double y7 = (double) fsPointf.Y;
        double y8 = num21 + y7;
        FS_PATHPOINTF fsPathpointf3 = new FS_PATHPOINTF((float) x8, (float) y8, PathPointFlags.BezierTo);
        fsPathpointfList3.Add(fsPathpointf3);
        ++index2;
        index3 += 2;
      }
      List<FS_PATHPOINTF> fsPathpointfList4 = cloudyCirclePoints;
      double num22 = (double) num5;
      fsPointf = fsQloudypointf.cPoints[fsQloudypointf.cPoints.Count - 1];
      double x9 = (double) fsPointf.X;
      double x10 = num22 + x9;
      double num23 = (double) num6;
      fsPointf = fsQloudypointf.cPoints[fsQloudypointf.cPoints.Count - 1];
      double y9 = (double) fsPointf.Y;
      double y10 = num23 + y9;
      FS_PATHPOINTF fsPathpointf4 = new FS_PATHPOINTF((float) x10, (float) y10, PathPointFlags.BezierTo);
      fsPathpointfList4.Add(fsPathpointf4);
      List<FS_PATHPOINTF> fsPathpointfList5 = cloudyCirclePoints;
      double num24 = (double) num5;
      fsPointf = fsQloudypointf.cPoints[fsQloudypointf.cPoints.Count - 2];
      double x11 = (double) fsPointf.X;
      double x12 = num24 + x11;
      double num25 = (double) num6;
      fsPointf = fsQloudypointf.cPoints[fsQloudypointf.cPoints.Count - 2];
      double y11 = (double) fsPointf.Y;
      double y12 = num25 + y11;
      FS_PATHPOINTF fsPathpointf5 = new FS_PATHPOINTF((float) x12, (float) y12, PathPointFlags.BezierTo);
      fsPathpointfList5.Add(fsPathpointf5);
      List<FS_PATHPOINTF> fsPathpointfList6 = cloudyCirclePoints;
      double num26 = (double) num5;
      fsPointf = fsQloudypointf.Points[fsQloudypointf.Points.Count - 2];
      double x13 = (double) fsPointf.X;
      double x14 = num26 + x13;
      double num27 = (double) num6;
      fsPointf = fsQloudypointf.Points[fsQloudypointf.Points.Count - 2];
      double y13 = (double) fsPointf.Y;
      double y14 = num27 + y13;
      FS_PATHPOINTF fsPathpointf6 = new FS_PATHPOINTF((float) x14, (float) y14, PathPointFlags.BezierTo);
      fsPathpointfList6.Add(fsPathpointf6);
    }
    return cloudyCirclePoints;
  }

  /// <summary>
  /// Creates a collection that contains the paths that define the broken line. Where the border should seem "cloudy".
  /// </summary>
  /// <param name="vertices">List of points of broken line</param>
  /// <param name="effectIntencity">A number describing the intensity of the effect.</param>
  /// <returns>A collection of points for this line and the cloud boundary.</returns>
  /// <remarks>
  /// If the line is not closed, then the last point automatically joins the first one.
  /// The wave is drawn at the left in the direction of drawing line, hence the line is recommended to draw in a clockwise direction.
  /// </remarks>
  public static List<FS_PATHPOINTF> GetCloudyLinePoints(
    IList<FS_POINTF> vertices,
    int effectIntencity)
  {
    float radius1 = (float) effectIntencity * AnnotDrawing.CloudyIntencityK;
    float num1 = 2f * radius1;
    float num2 = num1 * AnnotDrawing.CloudyLengthDK;
    List<AnnotDrawing.FS_QLOUDYPOINTF> fsQloudypointfList1 = new List<AnnotDrawing.FS_QLOUDYPOINTF>();
    FS_POINTF vertex = vertices[0];
    double x1 = (double) vertex.X;
    vertex = vertices[vertices.Count - 1];
    double x2 = (double) vertex.X;
    FS_POINTF fsPointf1;
    if (AnnotDrawing.AreEqual((float) x1, (float) x2))
    {
      fsPointf1 = vertices[0];
      double y1 = (double) fsPointf1.Y;
      fsPointf1 = vertices[vertices.Count - 1];
      double y2 = (double) fsPointf1.Y;
      if (AnnotDrawing.AreEqual((float) y1, (float) y2))
        goto label_3;
    }
    IList<FS_POINTF> fsPointfList = vertices;
    fsPointf1 = vertices[0];
    double x3 = (double) fsPointf1.X;
    fsPointf1 = vertices[0];
    double y3 = (double) fsPointf1.Y;
    FS_POINTF fsPointf2 = new FS_POINTF((float) x3, (float) y3);
    fsPointfList.Add(fsPointf2);
label_3:
    for (int index1 = 1; index1 < vertices.Count; ++index1)
    {
      double vectorLen = AnnotDrawing.GetVectorLen(vertices[index1 - 1], vertices[index1]);
      int num3 = (int) (vectorLen / (double) num2);
      double num4 = vectorLen % (double) num2;
      float num5 = num1 / 2f * AnnotDrawing.CloudyLimitFactor;
      double num6 = (double) num1 - (double) num2 - (double) num5;
      if (num4 > num6)
        ++num3;
      fsPointf1 = vertices[index1 - 1];
      float x4 = fsPointf1.X;
      fsPointf1 = vertices[index1 - 1];
      float y4 = fsPointf1.Y;
      fsPointf1 = vertices[index1];
      double x5 = (double) fsPointf1.X;
      fsPointf1 = vertices[index1 - 1];
      double x6 = (double) fsPointf1.X;
      float num7 = (float) (x5 - x6) / (float) num3;
      fsPointf1 = vertices[index1];
      double y5 = (double) fsPointf1.Y;
      fsPointf1 = vertices[index1 - 1];
      double y6 = (double) fsPointf1.Y;
      float num8 = (float) (y5 - y6) / (float) num3;
      for (int index2 = 0; index2 < num3 - 1; ++index2)
      {
        x4 += num7;
        y4 += num8;
        fsQloudypointfList1.Add(new AnnotDrawing.FS_QLOUDYPOINTF(radius1, new FS_POINTF(x4, y4)));
      }
      List<AnnotDrawing.FS_QLOUDYPOINTF> fsQloudypointfList2 = fsQloudypointfList1;
      double radius2 = (double) radius1;
      fsPointf1 = vertices[index1];
      double x7 = (double) fsPointf1.X;
      fsPointf1 = vertices[index1];
      double y7 = (double) fsPointf1.Y;
      FS_POINTF center = new FS_POINTF((float) x7, (float) y7);
      AnnotDrawing.FS_QLOUDYPOINTF fsQloudypointf = new AnnotDrawing.FS_QLOUDYPOINTF((float) radius2, center, true);
      fsQloudypointfList2.Add(fsQloudypointf);
    }
    for (int index = fsQloudypointfList1.Count - 1; index >= 0; --index)
    {
      fsQloudypointfList1[index].CalcFirstArcPoint(index == 0 ? fsQloudypointfList1[fsQloudypointfList1.Count - 1] : fsQloudypointfList1[index - 1]);
      if (index < fsQloudypointfList1.Count - 1)
        fsQloudypointfList1[index].CalcSecondArcPoint(fsQloudypointfList1[index + 1]);
    }
    fsQloudypointfList1[fsQloudypointfList1.Count - 1].CalcSecondArcPoint(fsQloudypointfList1[0]);
    List<FS_PATHPOINTF> cloudyLinePoints = new List<FS_PATHPOINTF>();
    for (int index3 = 0; index3 < fsQloudypointfList1.Count; ++index3)
    {
      AnnotDrawing.FS_QLOUDYPOINTF fsQloudypointf1 = fsQloudypointfList1[index3];
      if (fsQloudypointf1.isCorner)
      {
        AnnotDrawing.FS_QLOUDYPOINTF fsQloudypointf2 = index3 == 0 ? fsQloudypointfList1[fsQloudypointfList1.Count - 1] : fsQloudypointfList1[index3 - 1];
        AnnotDrawing.FS_QLOUDYPOINTF fsQloudypointf3 = index3 == fsQloudypointfList1.Count - 1 ? fsQloudypointfList1[0] : fsQloudypointfList1[index3 + 1];
        if (AnnotDrawing.IsRayCross(fsQloudypointf2.Points[0], fsQloudypointf2.cPoints[0], fsQloudypointf3.Points[0], fsQloudypointf3.cPoints[0]) && Math.Abs(fsQloudypointf1.GetAngleBetweenRadiuses(fsQloudypointf1.Points[0], fsQloudypointf1.Points[fsQloudypointf1.Points.Count - 2])) > Math.PI)
        {
          if (index3 == 0)
          {
            List<FS_PATHPOINTF> fsPathpointfList = cloudyLinePoints;
            fsPointf1 = fsQloudypointf2.Points[fsQloudypointf2.Points.Count - 2];
            double x8 = (double) fsPointf1.X;
            fsPointf1 = fsQloudypointf2.Points[fsQloudypointf2.Points.Count - 2];
            double y8 = (double) fsPointf1.Y;
            FS_PATHPOINTF fsPathpointf = new FS_PATHPOINTF((float) x8, (float) y8, PathPointFlags.MoveTo);
            fsPathpointfList.Add(fsPathpointf);
          }
          List<FS_PATHPOINTF> fsPathpointfList1 = cloudyLinePoints;
          fsPointf1 = fsQloudypointf3.Points[0];
          double x9 = (double) fsPointf1.X;
          fsPointf1 = fsQloudypointf3.Points[0];
          double y9 = (double) fsPointf1.Y;
          FS_PATHPOINTF fsPathpointf1 = new FS_PATHPOINTF((float) x9, (float) y9, PathPointFlags.LineTo);
          fsPathpointfList1.Add(fsPathpointf1);
          continue;
        }
      }
      if (index3 == 0)
      {
        List<FS_PATHPOINTF> fsPathpointfList = cloudyLinePoints;
        fsPointf1 = fsQloudypointf1.Points[0];
        double x10 = (double) fsPointf1.X;
        fsPointf1 = fsQloudypointf1.Points[0];
        double y10 = (double) fsPointf1.Y;
        FS_PATHPOINTF fsPathpointf = new FS_PATHPOINTF((float) x10, (float) y10, PathPointFlags.MoveTo);
        fsPathpointfList.Add(fsPathpointf);
      }
      int index4 = 1;
      int index5 = 1;
      while (index4 < fsQloudypointf1.Points.Count)
      {
        List<FS_PATHPOINTF> fsPathpointfList2 = cloudyLinePoints;
        fsPointf1 = fsQloudypointf1.cPoints[index5 - 1];
        double x11 = (double) fsPointf1.X;
        fsPointf1 = fsQloudypointf1.cPoints[index5 - 1];
        double y11 = (double) fsPointf1.Y;
        FS_PATHPOINTF fsPathpointf2 = new FS_PATHPOINTF((float) x11, (float) y11, PathPointFlags.BezierTo);
        fsPathpointfList2.Add(fsPathpointf2);
        List<FS_PATHPOINTF> fsPathpointfList3 = cloudyLinePoints;
        fsPointf1 = fsQloudypointf1.cPoints[index5];
        double x12 = (double) fsPointf1.X;
        fsPointf1 = fsQloudypointf1.cPoints[index5];
        double y12 = (double) fsPointf1.Y;
        FS_PATHPOINTF fsPathpointf3 = new FS_PATHPOINTF((float) x12, (float) y12, PathPointFlags.BezierTo);
        fsPathpointfList3.Add(fsPathpointf3);
        List<FS_PATHPOINTF> fsPathpointfList4 = cloudyLinePoints;
        fsPointf1 = fsQloudypointf1.Points[index4];
        double x13 = (double) fsPointf1.X;
        fsPointf1 = fsQloudypointf1.Points[index4];
        double y13 = (double) fsPointf1.Y;
        FS_PATHPOINTF fsPathpointf4 = new FS_PATHPOINTF((float) x13, (float) y13, PathPointFlags.BezierTo);
        fsPathpointfList4.Add(fsPathpointf4);
        ++index4;
        index5 += 2;
      }
      List<FS_PATHPOINTF> fsPathpointfList5 = cloudyLinePoints;
      fsPointf1 = fsQloudypointf1.cPoints[fsQloudypointf1.cPoints.Count - 1];
      double x14 = (double) fsPointf1.X;
      fsPointf1 = fsQloudypointf1.cPoints[fsQloudypointf1.cPoints.Count - 1];
      double y14 = (double) fsPointf1.Y;
      FS_PATHPOINTF fsPathpointf5 = new FS_PATHPOINTF((float) x14, (float) y14, PathPointFlags.BezierTo);
      fsPathpointfList5.Add(fsPathpointf5);
      List<FS_PATHPOINTF> fsPathpointfList6 = cloudyLinePoints;
      fsPointf1 = fsQloudypointf1.cPoints[fsQloudypointf1.cPoints.Count - 2];
      double x15 = (double) fsPointf1.X;
      fsPointf1 = fsQloudypointf1.cPoints[fsQloudypointf1.cPoints.Count - 2];
      double y15 = (double) fsPointf1.Y;
      FS_PATHPOINTF fsPathpointf6 = new FS_PATHPOINTF((float) x15, (float) y15, PathPointFlags.BezierTo);
      fsPathpointfList6.Add(fsPathpointf6);
      List<FS_PATHPOINTF> fsPathpointfList7 = cloudyLinePoints;
      fsPointf1 = fsQloudypointf1.Points[fsQloudypointf1.Points.Count - 2];
      double x16 = (double) fsPointf1.X;
      fsPointf1 = fsQloudypointf1.Points[fsQloudypointf1.Points.Count - 2];
      double y16 = (double) fsPointf1.Y;
      FS_PATHPOINTF fsPathpointf7 = new FS_PATHPOINTF((float) x16, (float) y16, PathPointFlags.BezierTo);
      fsPathpointfList7.Add(fsPathpointf7);
    }
    return cloudyLinePoints;
  }

  /// <summary>Get points for a  short line</summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetButtPoints(FS_RECTF rect)
  {
    List<FS_PATHPOINTF> buttPoints = new List<FS_PATHPOINTF>();
    float x = rect.left + rect.Width / 2f;
    float top = rect.top;
    float bottom = rect.bottom;
    buttPoints.Add(new FS_PATHPOINTF(x, top, PathPointFlags.MoveTo));
    buttPoints.Add(new FS_PATHPOINTF(x, bottom, PathPointFlags.LineTo));
    return buttPoints;
  }

  /// <summary>
  /// Get points for a diamond shape filled with the annotation’s interior color
  /// </summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetDiamondPoints(FS_RECTF rect, float lineWidth)
  {
    float height = rect.Height;
    float width = rect.Width;
    float d1 = (float) (((double) width * (double) width - (double) height * (double) height) / ((double) height * (double) height + (double) width * (double) width));
    double num1 = (double) lineWidth / (2.0 * Math.Sin(Math.Acos((double) d1) / 2.0));
    float d2 = (float) (((double) height * (double) height - (double) width * (double) width) / ((double) height * (double) height + (double) width * (double) width));
    double num2 = (double) lineWidth / (2.0 * Math.Sin(Math.Acos((double) d2) / 2.0));
    return new List<FS_PATHPOINTF>()
    {
      new FS_PATHPOINTF((double) rect.left + (double) rect.Width / 2.0, (double) rect.top - num2, PathPointFlags.MoveTo),
      new FS_PATHPOINTF((double) rect.right - num1, (double) rect.bottom + (double) rect.Height / 2.0, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.left + (double) rect.Width / 2.0, (double) rect.bottom + num2, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.left + num1, (double) rect.bottom + (double) rect.Height / 2.0, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.left + (double) rect.Width / 2.0, (double) rect.top - num2, PathPointFlags.CloseFigure | PathPointFlags.LineTo)
    };
  }

  /// <summary>
  /// Get points for a short line at the endpoint approximately 30 degrees clockwise from perpendicular to the line itself
  /// </summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetSlashPoints(FS_RECTF rect, float lineWidth)
  {
    float height = rect.Height;
    float width = rect.Width;
    double num1 = (double) lineWidth * Math.Cos(Math.Atan(3.0 * (double) height / (double) width)) / 2.0;
    double num2 = (double) lineWidth * Math.Cos(Math.Atan(3.0 * (double) height / (double) width)) * Math.Cos(Math.Atan(3.0 * (double) height / (double) width)) / Math.Sin(Math.Atan(3.0 * (double) height / (double) width)) / 2.0;
    return new List<FS_PATHPOINTF>()
    {
      new FS_PATHPOINTF((double) rect.left + (double) rect.Width / 3.0 * 2.0 - num2, (double) rect.top - num1, PathPointFlags.MoveTo),
      new FS_PATHPOINTF((double) rect.left + (double) rect.Width / 3.0 + num2, (double) rect.bottom + num1, PathPointFlags.LineTo)
    };
  }

  /// <summary>
  /// Get points for two short lines meeting in an acute angle to form an open arrowhead
  /// </summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetOpenArrowPoints(FS_RECTF rect, float lineWidth)
  {
    float height = rect.Height;
    float width = rect.Width;
    double num1 = (double) lineWidth / 2.0 * Math.Sin(Math.Atan((double) height / (2.0 * (double) width)));
    double num2 = (double) lineWidth / Math.Cos(Math.Atan((double) height / (2.0 * (double) width))) - Math.Sqrt((double) lineWidth * (double) lineWidth / 4.0 - num1 * num1);
    double num3 = (double) lineWidth / (2.0 * Math.Sin(Math.PI / 2.0 - Math.Atan(2.0 * (double) width / (double) height)));
    return new List<FS_PATHPOINTF>()
    {
      new FS_PATHPOINTF((double) rect.left + num1, (double) rect.top - num2, PathPointFlags.MoveTo),
      new FS_PATHPOINTF((double) rect.right - num3, (double) rect.bottom + (double) rect.Height / 2.0, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.left + num1, (double) rect.bottom + num2, PathPointFlags.LineTo)
    };
  }

  /// <summary>
  /// Get points for two short lines in the reverse direction from OpenArrow
  /// </summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetROpenArrowPoints(FS_RECTF rect, float lineWidth)
  {
    float height = rect.Height;
    float width = rect.Width;
    double num1 = (double) lineWidth / 2.0 * Math.Sin(Math.Atan((double) height / (2.0 * (double) width)));
    double num2 = (double) lineWidth / Math.Cos(Math.Atan((double) height / (2.0 * (double) width))) - Math.Sqrt((double) lineWidth * (double) lineWidth / 4.0 - num1 * num1);
    double num3 = (double) lineWidth / (2.0 * Math.Sin(Math.PI / 2.0 - Math.Atan(2.0 * (double) width / (double) height)));
    return new List<FS_PATHPOINTF>()
    {
      new FS_PATHPOINTF((double) rect.right - num1, (double) rect.top - num2, PathPointFlags.MoveTo),
      new FS_PATHPOINTF((double) rect.left + num3, (double) rect.bottom + (double) rect.Height / 2.0, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.right - num1, (double) rect.bottom + num2, PathPointFlags.LineTo)
    };
  }

  /// <summary>
  /// Get points for a triangle filled with internal color annotations
  /// </summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetClosedArrowPoints(FS_RECTF rect, float lineWidth)
  {
    float height = rect.Height;
    float width = rect.Width;
    double num1 = (double) lineWidth / (2.0 * Math.Tan(Math.Atan(2.0 * (double) width / (double) height) / 2.0));
    double num2 = (double) lineWidth / (2.0 * Math.Sin(Math.PI / 2.0 - Math.Atan(2.0 * (double) width / (double) height)));
    return new List<FS_PATHPOINTF>()
    {
      new FS_PATHPOINTF((double) rect.left + (double) lineWidth / 2.0, (double) rect.top - num1, PathPointFlags.MoveTo),
      new FS_PATHPOINTF((double) rect.right - num2, (double) rect.bottom + (double) rect.Height / 2.0, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.left + (double) lineWidth / 2.0, (double) rect.bottom + num1, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.left + (double) lineWidth / 2.0, (double) rect.top - num1, PathPointFlags.CloseFigure | PathPointFlags.LineTo)
    };
  }

  /// <summary>
  /// Get points for a triangular closed arrowhead in the reverse direction from ClosedArrow
  /// </summary>
  /// <param name="rect">Rectangle in which the figure should be inscribed.</param>
  /// <param name="lineWidth">The line width.</param>
  /// <returns>A Collection of points of this figure.</returns>
  public static List<FS_PATHPOINTF> GetRClosedArrowPoints(FS_RECTF rect, float lineWidth)
  {
    float height = rect.Height;
    float width = rect.Width;
    double num1 = (double) lineWidth / (2.0 * Math.Tan(Math.Atan(2.0 * (double) width / (double) height) / 2.0));
    double num2 = (double) lineWidth / (2.0 * Math.Sin(Math.PI / 2.0 - Math.Atan(2.0 * (double) width / (double) height)));
    return new List<FS_PATHPOINTF>()
    {
      new FS_PATHPOINTF((double) rect.right - (double) lineWidth / 2.0, (double) rect.top - num1, PathPointFlags.MoveTo),
      new FS_PATHPOINTF((double) rect.left + num2, (double) rect.bottom + (double) rect.Height / 2.0, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.right - (double) lineWidth / 2.0, (double) rect.bottom + num1, PathPointFlags.LineTo),
      new FS_PATHPOINTF((double) rect.right - (double) lineWidth / 2.0, (double) rect.top - num1, PathPointFlags.CloseFigure | PathPointFlags.LineTo)
    };
  }

  private static List<FS_PATHPOINTF> GetRoundRectPoints(FS_RECTF r, float roundK)
  {
    return new List<FS_PATHPOINTF>()
    {
      new FS_PATHPOINTF(r.left + roundK, r.top, PathPointFlags.MoveTo),
      new FS_PATHPOINTF(r.right - roundK, r.top, PathPointFlags.LineTo),
      new FS_PATHPOINTF(r.right, r.top, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.right, r.top, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.right, r.top - roundK, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.right, r.bottom + roundK, PathPointFlags.LineTo),
      new FS_PATHPOINTF(r.right, r.bottom, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.right, r.bottom, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.right - roundK, r.bottom, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.left + roundK, r.bottom, PathPointFlags.LineTo),
      new FS_PATHPOINTF(r.left, r.bottom, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.left, r.bottom, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.left, r.bottom + roundK, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.left, r.top - roundK, PathPointFlags.LineTo),
      new FS_PATHPOINTF(r.left, r.top, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.left, r.top, PathPointFlags.BezierTo),
      new FS_PATHPOINTF(r.left + roundK, r.top, PathPointFlags.CloseFigure | PathPointFlags.BezierTo)
    };
  }

  private static List<FS_PATHPOINTF> GetLineEndingPoints(
    LineEndingStyles e,
    float lineWidth,
    FS_POINTF target,
    out bool fillable)
  {
    fillable = false;
    float num1 = lineWidth * AnnotDrawing.LineEndingRectK;
    float num2 = lineWidth * AnnotDrawing.LineEndingRectK;
    FS_RECTF rect = new FS_RECTF(target.X - num1 / 2f, target.Y + num2 / 2f, target.X + num1 / 2f, target.Y - num2 / 2f);
    if (e == LineEndingStyles.OpenArrow || e == LineEndingStyles.ClosedArrow)
      rect = new FS_RECTF(target.X - (float) (2.0 * (double) num1 / 3.0), target.Y + num2 / 2f, target.X + num1 / 3f, target.Y - num2 / 2f);
    List<FS_PATHPOINTF> lineEndingPoints = (List<FS_PATHPOINTF>) null;
    switch (e)
    {
      case LineEndingStyles.Square:
        fillable = true;
        lineEndingPoints = AnnotDrawing.GetSquarePoints(rect, lineWidth);
        break;
      case LineEndingStyles.Circle:
        lineEndingPoints = AnnotDrawing.GetCirclePoints(rect, lineWidth);
        break;
      case LineEndingStyles.Diamond:
        fillable = true;
        lineEndingPoints = AnnotDrawing.GetDiamondPoints(rect, lineWidth);
        break;
      case LineEndingStyles.OpenArrow:
        lineEndingPoints = AnnotDrawing.GetOpenArrowPoints(rect, lineWidth);
        break;
      case LineEndingStyles.ClosedArrow:
        fillable = true;
        lineEndingPoints = AnnotDrawing.GetClosedArrowPoints(rect, lineWidth);
        break;
      case LineEndingStyles.Butt:
        lineEndingPoints = AnnotDrawing.GetButtPoints(rect);
        break;
      case LineEndingStyles.ROpenArrow:
        lineEndingPoints = AnnotDrawing.GetROpenArrowPoints(rect, lineWidth);
        break;
      case LineEndingStyles.RClosedArrow:
        fillable = true;
        lineEndingPoints = AnnotDrawing.GetRClosedArrowPoints(rect, lineWidth);
        break;
      case LineEndingStyles.Slash:
        lineEndingPoints = AnnotDrawing.GetSlashPoints(rect, lineWidth);
        break;
    }
    return lineEndingPoints;
  }

  /// <summary>Represents a segment of a cloud</summary>
  private class FS_QLOUDYPOINTF
  {
    public bool isCorner;
    public FS_POINTF Center;
    public List<FS_POINTF> Points;
    public float Radius;
    public List<FS_POINTF> cPoints;

    public FS_QLOUDYPOINTF(float radius, FS_POINTF center, bool isCorner = false)
    {
      this.Radius = radius;
      this.Center = center;
      this.isCorner = isCorner;
    }

    public void CalcFirstArcPoint(AnnotDrawing.FS_QLOUDYPOINTF left)
    {
      FS_POINTF from = new FS_POINTF(left.Center.X, left.Center.Y);
      FS_POINTF to = new FS_POINTF(this.Center.X, this.Center.Y);
      double radius1 = (double) left.Radius;
      double radius2 = (double) this.Radius;
      double vectorLen = AnnotDrawing.GetVectorLen(from, to);
      if (radius1 + radius2 < vectorLen || Math.Abs(radius1 - radius2) > vectorLen)
        return;
      double num1 = (Math.Pow(radius2, 2.0) - Math.Pow(radius1, 2.0) + Math.Pow(vectorLen, 2.0)) / (2.0 * vectorLen);
      double x = vectorLen - num1;
      double num2 = Math.Sqrt(Math.Pow(radius1, 2.0) - Math.Pow(x, 2.0));
      FS_POINTF fsPointf1 = new FS_POINTF((double) from.X + x / vectorLen * ((double) to.X - (double) from.X), (double) from.Y + x / vectorLen * ((double) to.Y - (double) from.Y));
      FS_POINTF fsPointf2 = new FS_POINTF((double) fsPointf1.X - ((double) to.Y - (double) from.Y) / vectorLen * num2, (double) fsPointf1.Y + ((double) to.X - (double) from.X) / vectorLen * num2);
      FS_POINTF fsPointf3 = new FS_POINTF((double) fsPointf1.X + ((double) to.Y - (double) from.Y) / vectorLen * num2, (double) fsPointf1.Y - ((double) to.X - (double) from.X) / vectorLen * num2);
      this.Points = new List<FS_POINTF>();
      this.Points.Add(fsPointf2);
    }

    public void CalcSecondArcPoint(AnnotDrawing.FS_QLOUDYPOINTF right)
    {
      this.Points.Add(right.Points[0]);
      double ang1 = (double) -AnnotDrawing.CloudyLengthTail;
      FS_POINTF point1 = right.Points[0];
      double x = (double) point1.X - (double) this.Center.X;
      point1 = right.Points[0];
      double y = (double) point1.Y - (double) this.Center.Y;
      FS_POINTF fsPointf1 = AnnotDrawing.RotatePoint((float) ang1, (float) x, (float) y);
      this.Points.Add(new FS_POINTF(fsPointf1.X + this.Center.X, fsPointf1.Y + this.Center.Y));
      double angleBetweenRadiuses = this.GetAngleBetweenRadiuses(this.Points[0], this.Points[this.Points.Count - 2]);
      if (Math.Abs(angleBetweenRadiuses) >= Math.PI / 2.0)
      {
        double ang2 = angleBetweenRadiuses / 4.0;
        for (int index = 0; index < 3; ++index)
        {
          FS_POINTF point2 = this.Points[this.Points.Count - 3];
          FS_POINTF fsPointf2 = AnnotDrawing.RotatePoint((float) ang2, point2.X - this.Center.X, point2.Y - this.Center.Y);
          this.Points.Insert(index + 1, new FS_POINTF(fsPointf2.X + this.Center.X, fsPointf2.Y + this.Center.Y));
        }
      }
      this.cPoints = new List<FS_POINTF>();
      for (int index = 1; index < this.Points.Count; ++index)
      {
        FS_POINTF cfrom;
        FS_POINTF cto;
        this.CalcControlPoints(this.Points[index - 1], this.Points[index], out cfrom, out cto);
        this.cPoints.Add(cfrom);
        this.cPoints.Add(cto);
      }
    }

    public void CalcControlPoints(
      FS_POINTF from,
      FS_POINTF to,
      out FS_POINTF cfrom,
      out FS_POINTF cto)
    {
      double angFrom;
      double angTo;
      double x = (double) this.Radius * 4.0 / 3.0 * Math.Tan(0.25 * this.GetAngleBetweenRadiuses(from, to, out angFrom, out angTo));
      FS_POINTF fsPointf1 = AnnotDrawing.RotatePoint((float) (Math.PI / 2.0 - angFrom), (float) x, 0.0f);
      cfrom = new FS_POINTF(fsPointf1.X + from.X, fsPointf1.Y + from.Y);
      FS_POINTF fsPointf2 = AnnotDrawing.RotatePoint((float) (3.0 * Math.PI / 2.0 - angTo), (float) x, 0.0f);
      cto = new FS_POINTF(fsPointf2.X + to.X, fsPointf2.Y + to.Y);
    }

    public double GetAngleBetweenRadiuses(FS_POINTF from, FS_POINTF to)
    {
      return this.GetAngleBetweenRadiuses(from, to, out double _, out double _);
    }

    public double GetAngleBetweenRadiuses(
      FS_POINTF from,
      FS_POINTF to,
      out double angFrom,
      out double angTo)
    {
      angFrom = AnnotDrawing.GetVectorAng(this.Center, from);
      angTo = AnnotDrawing.GetVectorAng(this.Center, to);
      double angleBetweenRadiuses = angFrom - angTo;
      if (angleBetweenRadiuses > 0.0)
        angleBetweenRadiuses = -(2.0 * Math.PI - angleBetweenRadiuses);
      return angleBetweenRadiuses;
    }
  }
}
