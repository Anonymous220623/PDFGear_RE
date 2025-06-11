// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageContents.PdfTextObjectUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using PDFKit.Contents.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit.Utils.PageContents;

internal static class PdfTextObjectUtils
{
  public static PdfTextObject[] UpdateTextObjectContent(
    PdfPage page,
    PdfTextObject textObject,
    string text)
  {
    PdfFont font = textObject.Font;
    float fontSize = textObject.FontSize;
    FS_MATRIX matrix = textObject.Matrix;
    string[] array = ((IEnumerable<string>) text.Replace("\r", "").Split('\n')).Where<string>((Func<string, bool>) (c => !string.IsNullOrEmpty(c))).ToArray<string>();
    if (array.Length == 0)
      return Array.Empty<PdfTextObject>();
    if (array.Length == 1)
      return PdfTextObjectUtils.CreateTextObjects(page, textObject, array[0], 0.0f, matrix)?.ToArray() ?? Array.Empty<PdfTextObject>();
    List<PdfTextObject> pdfTextObjectList = new List<PdfTextObject>();
    float num = (float) (font.Ascent - font.Descent) / 1000f * textObject.FontSize + 0.1f * textObject.FontSize;
    for (int index = 0; index < array.Length; ++index)
    {
      string str = array[index];
      List<PdfTextObject> textObjects = PdfTextObjectUtils.CreateTextObjects(page, textObject, array[index], (float) -index * num, matrix);
      if (textObjects != null && textObjects.Count > 0)
        pdfTextObjectList.AddRange((IEnumerable<PdfTextObject>) textObjects);
    }
    return pdfTextObjectList.ToArray();
  }

  private static List<PdfTextObject> CreateTextObjects(
    PdfPage page,
    PdfTextObject textObject,
    string text,
    float y,
    FS_MATRIX originalMatrix)
  {
    FS_MATRIX matrix = new FS_MATRIX((PdfTypeBase) originalMatrix.ToArray());
    PdfFont font1 = textObject.Font;
    float fontSize = textObject.FontSize;
    System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> withFallbackFonts = PdfFontUtils.GetTextWithFallbackFonts(text, font1, fontSize, new FS_RECTF(0.0f, 0.0f, float.MaxValue, float.MinValue));
    if (withFallbackFonts == null || withFallbackFonts.Count <= 0)
      return (List<PdfTextObject>) null;
    List<PdfTextObject> textObjects = new List<PdfTextObject>();
    float x = 0.0f;
    for (int index = 0; index < withFallbackFonts.Count; ++index)
    {
      PdfTextObject pdfTextObject1 = (PdfTextObject) textObject.Clone();
      pdfTextObject1.RemoveClipPath();
      TextWithFallbackFontFamily fallbackFontFamily1 = withFallbackFonts[index];
      if (fallbackFontFamily1.FallbackFontFamily != null)
      {
        PdfDocument document = page.Document;
        fallbackFontFamily1 = withFallbackFonts[index];
        System.Windows.Media.FontFamily fallbackFontFamily2 = fallbackFontFamily1.FallbackFontFamily;
        fallbackFontFamily1 = withFallbackFonts[index];
        System.Windows.FontWeight fontWeight = fallbackFontFamily1.FontWeight;
        fallbackFontFamily1 = withFallbackFonts[index];
        System.Windows.FontStyle fontStyle = fallbackFontFamily1.FontStyle;
        fallbackFontFamily1 = withFallbackFonts[index];
        int charSet = (int) fallbackFontFamily1.CharSet;
        PdfFont font2 = PdfFontUtils.CreateFont(document, fallbackFontFamily2, fontWeight, fontStyle, (FontCharSet) charSet);
        pdfTextObject1.Font = font2;
        PdfTextObject pdfTextObject2 = pdfTextObject1;
        fallbackFontFamily1 = withFallbackFonts[index];
        double scaledFontSize = (double) fallbackFontFamily1.ScaledFontSize;
        pdfTextObject2.FontSize = (float) scaledFontSize;
      }
      PdfTextObject pdfTextObject3 = pdfTextObject1;
      fallbackFontFamily1 = withFallbackFonts[index];
      string text1 = fallbackFontFamily1.Text;
      pdfTextObject3.TextUnicode = text1;
      FS_MATRIX fsMatrix = new FS_MATRIX();
      fsMatrix.SetIdentity();
      pdfTextObject1.Matrix = fsMatrix;
      pdfTextObject1.Location = new FS_POINTF(x, y);
      x += pdfTextObject1.BoundingBox.Width;
      fallbackFontFamily1 = withFallbackFonts[index];
      if (!string.IsNullOrWhiteSpace(fallbackFontFamily1.Text))
      {
        pdfTextObject1.Transform(matrix);
        textObjects.Add(pdfTextObject1);
      }
    }
    return textObjects;
  }

  public static WriteableBitmap GetTextObjectImage(
    PdfPage page,
    PdfTextObject textObj,
    Rect clientRect,
    double pixelsPerDip,
    System.Windows.Media.Color color)
  {
    if (clientRect.Width == 0.0 || clientRect.Height == 0.0 || page == null || textObj == null || !PdfTextObjectUtils.IsIdentityCoordinateSystem(textObj))
      return (WriteableBitmap) null;
    IntPtr handle = Pdfium.FPDF_LoadPage(page.Document.Handle, page.PageIndex);
    if (handle == IntPtr.Zero)
      return (WriteableBitmap) null;
    using (PdfPage page1 = PdfPage.FromHandle(page.Document, handle, page.PageIndex))
    {
      FS_RECTF effectiveBox = page1.GetEffectiveBox();
      PdfTextObject pdfTextObject = (PdfTextObject) textObj.Clone();
      pdfTextObject.FillColor = new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      if (pdfTextObject.StrokeColor.A == (int) byte.MaxValue)
        pdfTextObject.StrokeColor = new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      FS_MATRIX matrix = new FS_MATRIX();
      matrix.SetIdentity();
      FS_POINTF location = pdfTextObject.Location;
      matrix.Translate(-location.X, -location.Y);
      matrix.Scale(0.5f, 0.5f);
      pdfTextObject.Transform(matrix);
      pdfTextObject.TransformClipPath(matrix);
      FS_RECTF boundingBox = pdfTextObject.BoundingBox;
      matrix.SetIdentity();
      matrix.Translate(effectiveBox.left - boundingBox.left, effectiveBox.bottom - boundingBox.bottom);
      pdfTextObject.Transform(matrix);
      pdfTextObject.TransformClipPath(matrix);
      PdfPathObject pdfPathObject = PdfPathObject.Create(FillModes.Winding, false);
      pdfPathObject.FillColor = new FS_COLOR((int) byte.MaxValue, 0, 0, 0);
      pdfPathObject.Path.Add(new FS_PATHPOINTF(effectiveBox.left, effectiveBox.bottom + boundingBox.Height, PathPointFlags.MoveTo));
      pdfPathObject.Path.Add(new FS_PATHPOINTF(effectiveBox.left + boundingBox.Width, effectiveBox.bottom + boundingBox.Height, PathPointFlags.LineTo));
      pdfPathObject.Path.Add(new FS_PATHPOINTF(effectiveBox.left + boundingBox.Width, effectiveBox.bottom, PathPointFlags.LineTo));
      pdfPathObject.Path.Add(new FS_PATHPOINTF(effectiveBox.left, effectiveBox.bottom, PathPointFlags.CloseFigure | PathPointFlags.LineTo));
      PdfStampAnnotation pdfStampAnnotation = new PdfStampAnnotation(page1);
      pdfStampAnnotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
      pdfStampAnnotation.NormalAppearance.Add((PdfPageObject) pdfPathObject);
      pdfStampAnnotation.NormalAppearance.Add((PdfPageObject) pdfTextObject);
      pdfStampAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
      if (page1.Annots == null)
        page1.CreateAnnotations();
      page1.Annots.Add((PdfAnnotation) pdfStampAnnotation);
      try
      {
        double num1 = clientRect.Width * pixelsPerDip;
        double num2 = clientRect.Height * pixelsPerDip;
        PageRotate rotation1 = page1.Rotation;
        int rotate = -(int) rotation1;
        if (rotate < 0)
          rotate += 4;
        if (rotation1 == PageRotate.Rotate90 || rotation1 == PageRotate.Rotate270)
        {
          double num3 = num2;
          num2 = num1;
          num1 = num3;
        }
        (int x, int y, int width, int height) = PdfTextObjectUtils.GetPageRenderRect(page1, new FS_RECTF(effectiveBox.left, effectiveBox.bottom + boundingBox.Height, effectiveBox.left + boundingBox.Width, effectiveBox.bottom), num1, num2);
        using (PdfBitmap bitmap1 = new PdfBitmap((int) Math.Ceiling(num1), (int) Math.Ceiling(num2), true))
        {
          page1.RenderForms(bitmap1, x, y, width, height, (PageRotate) rotate, RenderFlags.FPDF_NONE);
          Rotation rotation2;
          switch (rotation1)
          {
            case PageRotate.Rotate90:
              rotation2 = Rotation.Rotate90;
              break;
            case PageRotate.Rotate180:
              rotation2 = Rotation.Rotate180;
              break;
            case PageRotate.Rotate270:
              rotation2 = Rotation.Rotate270;
              break;
            default:
              rotation2 = Rotation.Rotate0;
              break;
          }
          Image image = bitmap1.Image;
          using (Bitmap bitmap2 = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
          {
            using (ImageAttributes imageAttr = new ImageAttributes())
            {
              using (Graphics graphics = Graphics.FromImage((Image) bitmap2))
              {
                ColorMatrix newColorMatrix = new ColorMatrix(new float[5][]
                {
                  new float[5]
                  {
                    0.0f,
                    0.0f,
                    0.0f,
                    (float) color.A / (float) byte.MaxValue,
                    0.0f
                  },
                  new float[5],
                  new float[5],
                  new float[5],
                  new float[5]
                  {
                    (float) color.R / (float) byte.MaxValue,
                    (float) color.G / (float) byte.MaxValue,
                    (float) color.B / (float) byte.MaxValue,
                    0.0f,
                    1f
                  }
                });
                imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
              }
            }
            BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromRotation(rotation2);
            Int32Rect sourceRect = new Int32Rect(0, 0, bitmap2.Width, bitmap2.Height);
            IntPtr hbitmap = bitmap2.GetHbitmap();
            try
            {
              return new WriteableBitmap(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, sourceRect, sizeOptions));
            }
            finally
            {
              try
              {
                if (hbitmap != IntPtr.Zero)
                  PdfTextObjectUtils.DeleteObject(hbitmap);
              }
              catch
              {
              }
            }
          }
        }
      }
      finally
      {
        page1.Annots.Remove((PdfAnnotation) pdfStampAnnotation);
        try
        {
          pdfStampAnnotation.Dispose();
        }
        catch
        {
        }
      }
    }
  }

  public static RemoveIntersectingTextResult RemoveIntersectingText(
    System.Collections.Generic.IReadOnlyList<PdfTextObject> textObjects,
    FS_RECTF rect)
  {
    bool success = false;
    List<PdfTextObject> newTextObjects = new List<PdfTextObject>();
    List<PdfTextObject> notIntersectingTextObjects = new List<PdfTextObject>();
    for (int index1 = 0; index1 < textObjects.Count; ++index1)
    {
      PdfTextObject textObject = textObjects[index1];
      FS_RECTF[] charRects = PdfTextObjectUtils.GetCharRects(textObject);
      int start;
      int end;
      if (PdfTextObjectUtils.GetRemoveRange(rect, charRects, out start, out end))
      {
        if (start == 0 && end == charRects.Length - 1)
        {
          textObject.Container.Remove((PdfPageObject) textObject);
          success = true;
        }
        else if (start != 0)
        {
          PdfTextObject newTextObj1;
          if (TextObjectExtensions.SplitTextObj(textObject, start, false, out newTextObj1))
          {
            newTextObjects.Add(textObject);
            PdfTextObject newTextObj2;
            if (end != charRects.Length - 1 && TextObjectExtensions.SplitTextObj(newTextObj1, end - start + 1, false, out newTextObj2))
            {
              newTextObjects.Add(newTextObj2);
              int num = textObject.Container.IndexOf((PdfPageObject) textObject);
              textObject.Container.Insert(num + 1, (PdfPageObject) newTextObj2);
              Pdfium.FPDFPageObj_Release(newTextObj1.Handle);
            }
            success = true;
          }
        }
        else
        {
          PdfTextObject newTextObj;
          if (TextObjectExtensions.SplitTextObj(textObject, end + 1, false, out newTextObj))
          {
            newTextObjects.Add(newTextObj);
            PdfPageObjectsCollection container = textObject.Container;
            int index2 = container.IndexOf((PdfPageObject) textObject);
            container[index2] = (PdfPageObject) newTextObj;
            success = true;
          }
        }
      }
      else
        notIntersectingTextObjects.Add(textObject);
    }
    return success ? new RemoveIntersectingTextResult(success, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) newTextObjects, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) notIntersectingTextObjects) : new RemoveIntersectingTextResult(false, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null);
  }

  private static (int x, int y, int width, int height) GetPageRenderRect(
    PdfPage page,
    FS_RECTF annotBounds,
    double scaledBitmapWidth,
    double scaledBitmapHeight)
  {
    PageRotate rotation = page.Rotation;
    FS_RECTF effectiveBox = page.GetEffectiveBox();
    double num1 = scaledBitmapWidth / (double) annotBounds.Width;
    int num2 = (int) ((double) effectiveBox.Width * num1);
    int num3 = (int) ((double) effectiveBox.Height * num1);
    return (0, (int) (scaledBitmapHeight - (double) num3), num2, num3);
  }

  private static FS_RECTF[] GetCharRects(PdfTextObject textObj)
  {
    Matrix matrix = textObj.MatrixFromPage2();
    PdfFont font = textObj.Font;
    float fontSize = textObj.FontSize;
    int ascent = font.Ascent;
    int descent = font.Descent;
    float[] pPosArray = new float[textObj.CharsCount * 2];
    FS_RECTF[] charRects = new FS_RECTF[textObj.CharsCount];
    Pdfium.FPDFTextObj_CalcCharPos(textObj.Handle, pPosArray);
    for (int index = 0; index < textObj.CharsCount; ++index)
    {
      float x1 = pPosArray[index * 2];
      float x2 = pPosArray[index * 2 + 1];
      float y1 = (float) ascent * fontSize / (float) (ascent - descent);
      float y2 = (float) descent * fontSize / (float) (ascent - descent);
      System.Windows.Point point1 = matrix.Transform(new System.Windows.Point((double) x1, (double) y1));
      System.Windows.Point point2 = matrix.Transform(new System.Windows.Point((double) x2, (double) y2));
      charRects[index] = new FS_RECTF(Math.Min(point1.X, point2.X), Math.Max(point1.Y, point2.Y), Math.Max(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
    }
    return charRects;
  }

  internal static bool ShouldRemoveTextObject(FS_RECTF rect, PdfTextObject textObj)
  {
    if (textObj?.Container == null)
      return false;
    FS_RECTF[] charRects = PdfTextObjectUtils.GetCharRects(textObj);
    return PdfTextObjectUtils.GetRemoveRange(rect, charRects, out int _, out int _);
  }

  private static bool GetRemoveRange(
    FS_RECTF sourceRect,
    FS_RECTF[] charRects,
    out int start,
    out int end)
  {
    start = -1;
    end = -1;
    for (int index = 0; index < charRects.Length; ++index)
    {
      if (PdfTextObjectUtils.ShouldRemoveText(sourceRect, charRects[index]))
      {
        if (start == -1)
        {
          start = index;
          end = -1;
        }
      }
      else if (end == -1)
        end = index - 1;
    }
    if (start != -1)
    {
      if (end == -1)
        end = charRects.Length - 1;
      return true;
    }
    start = -1;
    end = -1;
    return false;
  }

  private static bool ShouldRemoveText(FS_RECTF sourceRect, FS_RECTF charRect)
  {
    if ((double) charRect.Width <= 0.0 || (double) charRect.Height <= 0.0)
      return true;
    float num1 = Math.Max(sourceRect.left, charRect.left);
    float num2 = Math.Min(sourceRect.top, charRect.top);
    float num3 = Math.Min(sourceRect.right, charRect.right);
    float num4 = Math.Max(sourceRect.bottom, charRect.bottom);
    if ((double) num3 - (double) num1 <= 0.0 || (double) num2 - (double) num4 <= 0.0)
      return false;
    float num5 = charRect.Width * charRect.Height;
    return ((double) num3 - (double) num1) * ((double) num2 - (double) num4) / (double) num5 >= 0.64000000000000012;
  }

  private static bool IsIdentityCoordinateSystem(PdfTextObject textObject)
  {
    for (PdfPageObjectsCollection container = textObject?.Container; container != null; container = container.Form?.Container)
    {
      PdfFormObject form = container.Form;
      int num;
      if (form == null)
      {
        num = 0;
      }
      else
      {
        bool? nullable = form.Matrix?.IsIdentity();
        bool flag = false;
        num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
      }
      if (num != 0)
        return false;
    }
    return true;
  }

  [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool DeleteObject(IntPtr hObject);

  internal static class PdfPageTextIndexHelper
  {
    public static System.Collections.Generic.IReadOnlyList<PdfTextObject> GetSelectedTextObject(
      PdfPage page,
      int selectRangeStartIndex,
      int selectRangeEndIndex,
      out PdfTextObject startObject,
      out int startObjectFirstCharIndex,
      out PdfTextObject endObject,
      out int endObjectLastCharIndex)
    {
      startObject = (PdfTextObject) null;
      startObjectFirstCharIndex = -1;
      endObject = (PdfTextObject) null;
      endObjectLastCharIndex = -1;
      if (selectRangeStartIndex < 0 || selectRangeEndIndex >= page.Text.CountChars || selectRangeStartIndex > selectRangeEndIndex)
        return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) Array.Empty<PdfTextObject>();
      int num1 = selectRangeStartIndex;
      int num2 = selectRangeEndIndex;
      do
      {
        if (!char.IsWhiteSpace(page.Text.GetCharacter(num1)))
        {
          startObject = PdfTextObjectUtils.PdfPageTextIndexHelper.AdjustTextObject(page, num1, out startObjectFirstCharIndex);
          if (startObject != null)
            break;
        }
        ++num1;
      }
      while (num1 <= num2);
      if (num1 > num2)
        return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) Array.Empty<PdfTextObject>();
      do
      {
        if (!char.IsWhiteSpace(page.Text.GetCharacter(num2)))
        {
          endObject = PdfTextObjectUtils.PdfPageTextIndexHelper.AdjustTextObject(page, num2, out endObjectLastCharIndex);
          if (endObject != null)
            break;
        }
        --num2;
      }
      while (num1 <= num2);
      if (num1 > num2 || startObject == null || endObject == null)
        return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) Array.Empty<PdfTextObject>();
      if (num1 > selectRangeStartIndex)
      {
        PdfTextObject textObject = startObject;
        int textObjectIndex = -1;
        int charIndex = startObjectFirstCharIndex;
        PdfTextObject pdfTextObject = startObject;
        int num3 = -1;
        int num4 = startObjectFirstCharIndex;
        int moveLength;
        for (int index = 0; pdfTextObject != null && num1 + index >= selectRangeStartIndex && PdfTextObjectUtils.PdfPageTextIndexHelper.GetObjectCharAndMove(ref charIndex, ref textObject, ref textObjectIndex, -1, out moveLength) != int.MinValue; index = moveLength)
        {
          startObject = pdfTextObject;
          startObjectFirstCharIndex = num4;
          num1 += index;
          pdfTextObject = textObject;
          num3 = textObjectIndex;
          num4 = charIndex;
        }
      }
      if (num2 < selectRangeEndIndex)
      {
        PdfTextObject textObject = endObject;
        int textObjectIndex = -1;
        int charIndex = endObjectLastCharIndex;
        PdfTextObject pdfTextObject = endObject;
        int num5 = -1;
        int num6 = endObjectLastCharIndex;
        int moveLength;
        for (int index = 0; pdfTextObject != null && num2 + index <= selectRangeEndIndex && PdfTextObjectUtils.PdfPageTextIndexHelper.GetObjectCharAndMove(ref charIndex, ref textObject, ref textObjectIndex, 1, out moveLength) != int.MinValue; index = moveLength)
        {
          endObject = pdfTextObject;
          endObjectLastCharIndex = num6;
          num2 += index;
          pdfTextObject = textObject;
          num5 = textObjectIndex;
          num6 = charIndex;
        }
      }
      List<PdfTextObject> list = new List<PdfTextObject>()
      {
        startObject
      };
      HashSet<IntPtr> numSet = new HashSet<IntPtr>()
      {
        startObject.Handle
      };
      for (int charIndexInPage = num1 + 1; charIndexInPage <= num2; ++charIndexInPage)
      {
        PdfTextObject pdfTextObject1 = (PdfTextObject) null;
        if (charIndexInPage < num2)
        {
          FS_RECTF charBox;
          PdfTextObject[] intersectingTextObjects = PdfTextObjectUtils.PdfPageTextIndexHelper.GetIntersectingTextObjects(page, charIndexInPage, out charBox);
          if (intersectingTextObjects.Length != 0)
            pdfTextObject1 = ((IEnumerable<PdfTextObject>) intersectingTextObjects).FirstOrDefault<PdfTextObject>((Func<PdfTextObject, bool>) (c => c.Handle == list[list.Count - 1].Handle)) ?? PdfTextObjectUtils.PdfPageTextIndexHelper.AdjustTextObjectCore(page, charIndexInPage, charBox, intersectingTextObjects, out int _);
        }
        else
          pdfTextObject1 = endObject;
        if (pdfTextObject1 != null)
        {
          PdfPageObjectsCollection container = pdfTextObject1.Container;
          // ISSUE: explicit non-virtual call
          int index = (container != null ? __nonvirtual (container.IndexOf((PdfPageObject) pdfTextObject1)) : -1) - 1;
          if (index > 0)
          {
            Stack<PdfTextObject> pdfTextObjectStack = new Stack<PdfTextObject>();
            for (; index >= 0; --index)
            {
              if (pdfTextObject1.Container[index] is PdfTextObject pdfTextObject2)
              {
                pdfTextObjectStack.Push(pdfTextObject2);
                if (numSet.Contains(pdfTextObject2.Handle))
                  break;
              }
              if (index == 0)
                pdfTextObjectStack.Clear();
            }
            while (pdfTextObjectStack.Count > 0)
            {
              PdfTextObject pdfTextObject3 = pdfTextObjectStack.Pop();
              if (numSet.Add(pdfTextObject3.Handle))
                list.Add(pdfTextObject3);
            }
          }
          if (numSet.Add(pdfTextObject1.Handle))
            list.Add(pdfTextObject1);
          if (pdfTextObject1 == endObject)
            break;
        }
      }
      if (numSet.Add(endObject.Handle))
        list.Add(endObject);
      if (startObjectFirstCharIndex == 0)
      {
        startObject = (PdfTextObject) null;
        startObjectFirstCharIndex = -1;
      }
      if (endObjectLastCharIndex == endObject.CharsCount - 1)
      {
        endObject = (PdfTextObject) null;
        endObjectLastCharIndex = -1;
      }
      return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) list;
    }

    public static RemoveIntersectingTextResult RemoveSelectedText(
      System.Collections.Generic.IReadOnlyList<PdfTextObject> textObjects,
      PdfTextObject startObject,
      int startObjectFirstCharIndex,
      PdfTextObject endObject,
      int endObjectLastCharIndex)
    {
      if (textObjects == null || textObjects.Count == 0)
        return new RemoveIntersectingTextResult(false, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) Array.Empty<PdfTextObject>(), (System.Collections.Generic.IReadOnlyList<PdfTextObject>) Array.Empty<PdfTextObject>());
      List<PdfTextObject> pdfTextObjectList = new List<PdfTextObject>();
      bool success = false;
      for (int index1 = 0; index1 < textObjects.Count; ++index1)
      {
        PdfTextObject textObject = textObjects[index1];
        int charsCount = textObject.CharsCount;
        int splitAtIndex = 0;
        int num1 = charsCount - 1;
        if (textObject == startObject)
          splitAtIndex = startObjectFirstCharIndex;
        if (textObject == endObject)
          num1 = endObjectLastCharIndex;
        if (splitAtIndex == 0 && num1 == charsCount - 1)
        {
          textObject.Container.Remove((PdfPageObject) textObject);
          success = true;
        }
        else if (splitAtIndex != 0)
        {
          PdfTextObject newTextObj1;
          if (TextObjectExtensions.SplitTextObj(textObject, splitAtIndex, false, out newTextObj1))
          {
            pdfTextObjectList.Add(textObject);
            PdfTextObject newTextObj2;
            if (num1 != charsCount - 1 && TextObjectExtensions.SplitTextObj(newTextObj1, num1 - splitAtIndex + 1, false, out newTextObj2))
            {
              pdfTextObjectList.Add(newTextObj2);
              int num2 = textObject.Container.IndexOf((PdfPageObject) textObject);
              textObject.Container.Insert(num2 + 1, (PdfPageObject) newTextObj2);
              Pdfium.FPDFPageObj_Release(newTextObj1.Handle);
            }
            success = true;
          }
        }
        else
        {
          PdfTextObject newTextObj;
          if (TextObjectExtensions.SplitTextObj(textObject, num1 + 1, false, out newTextObj))
          {
            pdfTextObjectList.Add(newTextObj);
            PdfPageObjectsCollection container = textObject.Container;
            int index2 = container.IndexOf((PdfPageObject) textObject);
            container[index2] = (PdfPageObject) newTextObj;
            success = true;
          }
        }
      }
      return new RemoveIntersectingTextResult(success, (System.Collections.Generic.IReadOnlyList<PdfTextObject>) pdfTextObjectList.ToArray(), (System.Collections.Generic.IReadOnlyList<PdfTextObject>) Array.Empty<PdfTextObject>());
    }

    internal static PdfTextObject[] GetIntersectingTextObjects(
      PdfPage page,
      int charIndexInPage,
      out FS_RECTF charBox)
    {
      charBox = new FS_RECTF();
      if (charIndexInPage < 0 || charIndexInPage >= page.Text.CountChars)
        return Array.Empty<PdfTextObject>();
      charBox = page.Text.GetCharBox(charIndexInPage);
      return PageContentUtils.GetIntersectingTextObjectsCore(page, charBox) ?? Array.Empty<PdfTextObject>();
    }

    private static PdfTextObject AdjustTextObjectCore(
      PdfPage page,
      int charIndexInPage,
      FS_RECTF charBox,
      PdfTextObject[] objects,
      out int charIndexInTextObject)
    {
      charIndexInTextObject = -1;
      if (charIndexInPage < 0 || charIndexInPage >= page.Text.CountChars)
        return (PdfTextObject) null;
      if (objects.Length == 0)
      {
        FS_RECTF rect = charBox;
        float width = rect.Width;
        rect.left -= width;
        rect.right += width * 2f;
        objects = PageContentUtils.GetIntersectingTextObjectsCore(page, rect);
      }
      if (objects.Length == 1)
      {
        if (PdfTextObjectUtils.PdfPageTextIndexHelper.TryGetCharIndexInTextObject(page, charIndexInPage, charBox, objects[0], out charIndexInTextObject))
          return objects[0];
      }
      else if (objects.Length > 1)
      {
        for (int index = 0; index < objects.Length; ++index)
        {
          if (PdfTextObjectUtils.PdfPageTextIndexHelper.TryGetCharIndexInTextObject(page, charIndexInPage, charBox, objects[index], out charIndexInTextObject))
            return objects[index];
        }
      }
      return (PdfTextObject) null;
    }

    private static PdfTextObject AdjustTextObject(
      PdfPage page,
      int charIndexInPage,
      out int charIndexInTextObject)
    {
      charIndexInTextObject = -1;
      if (charIndexInPage < 0 || charIndexInPage >= page.Text.CountChars)
        return (PdfTextObject) null;
      FS_RECTF charBox;
      PdfTextObject[] intersectingTextObjects = PdfTextObjectUtils.PdfPageTextIndexHelper.GetIntersectingTextObjects(page, charIndexInPage, out charBox);
      return PdfTextObjectUtils.PdfPageTextIndexHelper.AdjustTextObjectCore(page, charIndexInPage, charBox, intersectingTextObjects, out charIndexInTextObject);
    }

    private static bool TryGetCharIndexInTextObject(
      PdfPage page,
      int charIndexInPage,
      FS_RECTF pageCharBox,
      PdfTextObject textObject,
      out int charIndexInTextObject)
    {
      charIndexInTextObject = -1;
      if (charIndexInPage < 0 || charIndexInPage >= page.Text.CountChars)
        return false;
      FS_RECTF[] charRects = PdfTextObjectUtils.GetCharRects(textObject);
      int num1 = -1;
      for (int index = 0; index < charRects.Length; ++index)
      {
        FS_RECTF fsRectf = charRects[index];
        if (page.Text.GetCharIndexAtPos(fsRectf.left + fsRectf.Width / 2f, fsRectf.bottom + fsRectf.Height / 2f, fsRectf.Width / 2f, fsRectf.Height / 2f) == charIndexInPage)
        {
          num1 = index;
          break;
        }
      }
      if (num1 >= 0)
      {
        bool flag = true;
        int num2 = 10;
        for (int index1 = 0; index1 < 2 & flag; ++index1)
        {
          int charIndex1 = charIndexInPage;
          int charIndex2 = num1;
          PdfTextObject textObject1 = textObject;
          int textObjectIndex = -1;
          int num3 = index1 == 0 ? 1 : -1;
          for (int index2 = 0; index2 < (index1 == 0 ? 5 : num2); --num2)
          {
            int moveLength;
            int pageCharAndMove = PdfTextObjectUtils.PdfPageTextIndexHelper.GetPageCharAndMove(page, ref charIndex1, num3, out moveLength);
            int objectCharAndMove = PdfTextObjectUtils.PdfPageTextIndexHelper.GetObjectCharAndMove(ref charIndex2, ref textObject1, ref textObjectIndex, num3, out moveLength);
            if (pageCharAndMove != objectCharAndMove)
            {
              flag = false;
              break;
            }
            if (pageCharAndMove != int.MinValue)
              ++index2;
            else
              break;
          }
        }
        if (flag)
        {
          charIndexInTextObject = num1;
          return true;
        }
      }
      return false;
    }

    private static int GetPageCharAndMove(
      PdfPage page,
      ref int charIndex,
      int step,
      out int moveLength)
    {
      moveLength = 0;
      if (charIndex < 0 || charIndex >= page.Text.CountChars)
        return int.MinValue;
      char character;
      do
      {
        character = page.Text.GetCharacter(charIndex);
        charIndex += step;
        moveLength += step;
      }
      while (charIndex >= 0 && charIndex < page.Text.CountChars && char.IsWhiteSpace(character));
      if (charIndex > page.Text.CountChars || charIndex < 0)
      {
        charIndex = -1;
        moveLength = 0;
      }
      return (int) character;
    }

    private static int GetObjectCharAndMove(
      ref int charIndex,
      ref PdfTextObject textObject,
      ref int textObjectIndex,
      int charIndexStep,
      out int moveLength)
    {
      moveLength = 0;
      int c = int.MinValue;
      if (textObject != null && charIndex >= 0 && charIndex < textObject.CharsCount)
      {
        do
        {
          PdfFont font = textObject.Font;
          if (font != null)
          {
            int charCode;
            textObject.GetCharInfo(charIndex, out charCode, out float _, out float _);
            c = (int) font.ToUnicode(charCode);
          }
          charIndex += charIndexStep;
          moveLength += charIndexStep;
          if (charIndex < 0 || charIndex >= textObject.CharsCount)
          {
            if (textObjectIndex == -1)
            {
              ref int local = ref textObjectIndex;
              PdfPageObjectsCollection container = textObject.Container;
              // ISSUE: explicit non-virtual call
              int num = container != null ? __nonvirtual (container.IndexOf((PdfPageObject) textObject)) : -1;
              local = num;
            }
            if (textObjectIndex != -1)
            {
              bool flag = charIndex < 0;
              while (textObject != null && (charIndex < 0 || charIndex >= textObject.CharsCount))
              {
                textObjectIndex += flag ? -1 : 1;
                if (textObjectIndex >= 0 && textObjectIndex < textObject.Container.Count)
                {
                  if (textObject.Container[textObjectIndex] is PdfTextObject pdfTextObject)
                  {
                    if (flag)
                      charIndex += pdfTextObject.CharsCount;
                    else
                      charIndex -= textObject.CharsCount;
                    textObject = pdfTextObject;
                  }
                }
                else
                  textObject = (PdfTextObject) null;
              }
            }
          }
        }
        while (textObject != null && charIndex >= 0 && charIndex < textObject.CharsCount && char.IsWhiteSpace((char) c));
      }
      if (textObject == null)
      {
        textObject = (PdfTextObject) null;
        charIndex = -1;
        textObjectIndex = -1;
        charIndexStep = 0;
      }
      return c;
    }
  }
}
