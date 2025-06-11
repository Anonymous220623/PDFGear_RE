// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.TextObjectExtensions
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Contents.Utils;

internal static class TextObjectExtensions
{
  internal static bool IsEndOfPunctuation(this PdfTextObject textObj)
  {
    PdfFont font = textObj.Font;
    for (int index = textObj.CharsCount - 1; index >= 0; --index)
    {
      int charCode;
      textObj.GetCharInfo(index, out charCode, out float _, out float _);
      if (charCode != -1)
      {
        char unicode = font.ToUnicode(charCode);
        if (unicode > char.MinValue && unicode != ' ')
          return CharHelper.IsPunctuation((int) unicode);
      }
    }
    return false;
  }

  internal static bool CharIsPunctuation(this PdfTextObject textObj, int charIndex)
  {
    return CharHelper.IsPunctuation((int) textObj.GetUnicodeChar(charIndex));
  }

  internal static bool IsSameFontProperties(
    this PdfTextObject textObj,
    PdfFont font,
    float fontSize,
    FS_COLOR fillColor,
    FS_COLOR strokeColor,
    BoldItalicFlags boldItalic)
  {
    if (textObj == null)
      throw new ArgumentException();
    return font != null && font.Handle == textObj.Font.Handle && (double) fontSize == (double) textObj.GetFontSize() && fillColor == textObj.FillColor;
  }

  internal static bool IsSameFontProperties(this PdfTextObject textObj, PdfTextObject textObj2)
  {
    if (textObj == null)
      throw new ArgumentException();
    return textObj2 != null && textObj.IsSameFontProperties(textObj2.Font, textObj2.FontSize, textObj2.FillColor, textObj.StrokeColor, BoldItalicFlags.None);
  }

  internal static float[] GetCharPos(this PdfTextObject textObj, int charIndex)
  {
    if (textObj == null)
      throw new ArgumentException();
    charIndex = Math.Max(Math.Min(charIndex, textObj.CharsCount - 1), 0);
    FS_MATRIX fsMatrix = textObj.MatrixFromPage();
    float fontSize = textObj.FontSize;
    int ascent = textObj.Font.Ascent;
    int descent = textObj.Font.Descent;
    float[] pPosArray = new float[textObj.CharsCount * 2];
    Pdfium.FPDFTextObj_CalcCharPos(textObj.Handle, pPosArray);
    float x1 = pPosArray[charIndex * 2];
    float y1 = (float) ascent * fontSize / (float) (ascent - descent);
    fsMatrix.TransformPoint(ref x1, ref y1);
    float x2 = pPosArray[charIndex * 2 + 1];
    float y2 = (float) ascent * fontSize / (float) (ascent - descent);
    fsMatrix.TransformPoint(ref x2, ref y2);
    return new float[4]{ x1, y1, x2, y2 };
  }

  internal static void GetCharPos(
    this PdfTextObject textObj,
    int charIndex,
    out FS_POINTF ascentPoint,
    out FS_POINTF descentPoint)
  {
    ascentPoint = new FS_POINTF();
    descentPoint = new FS_POINTF();
    Matrix matrix = textObj.MatrixFromPage2();
    PdfFont font = textObj.Font;
    float fontSize = textObj.FontSize;
    int charsCount = textObj.CharsCount;
    float[] pPosArray = new float[charsCount * 2];
    Pdfium.FPDFTextObj_CalcCharPos(textObj.Handle, pPosArray);
    float x1 = charIndex < charsCount ? pPosArray[charIndex * 2] : pPosArray[(charsCount - 1) * 2 + 1];
    float num1 = x1;
    int ascent = font.Ascent;
    int descent = font.Descent;
    float num2 = fontSize;
    float y1 = (float) descent * num2 / (float) (ascent - descent);
    Point point1 = matrix.Transform(new Point((double) x1, (double) y1));
    descentPoint.X = (float) point1.X;
    descentPoint.Y = (float) point1.Y;
    float x2 = num1;
    float y2 = (float) ascent * num2 / (float) (ascent - descent);
    Point point2 = matrix.Transform(new Point((double) x2, (double) y2));
    ascentPoint.X = (float) point2.X;
    ascentPoint.Y = (float) point2.Y;
  }

  internal static float GetSpaceCharWidth(this PdfTextObject textObj)
  {
    float width;
    Pdfium.FPDFTextObj_GetSpaceCharWidth(textObj.Handle, out width);
    return width * (float) textObj.MatrixFromPage2().M11;
  }

  internal static bool IsRotate(this PdfTextObject textObj)
  {
    FS_MATRIX fsMatrix = textObj.MatrixFromPage();
    if ((double) fsMatrix.b == 0.0 && (double) fsMatrix.c == 0.0)
      return false;
    FS_QUADPOINTSF fsQuadpointsf = textObj.CalcCharBox(0);
    return (double) fsQuadpointsf.y1 != (double) fsQuadpointsf.y2 || (double) fsQuadpointsf.y3 != (double) fsQuadpointsf.y4;
  }

  internal static FS_QUADPOINTSF CalcCharBox(this PdfTextObject textObj, int index)
  {
    int charCode;
    float originX;
    float originY;
    Pdfium.FPDFTextObj_GetCharInfo(textObj.Handle, index, out charCode, out originX, out originY);
    FS_MATRIX fsMatrix = textObj.MatrixFromPage();
    FX_RECT charBbox = textObj.Font.GetCharBBox(charCode);
    float fontSize = textObj.FontSize;
    FS_RECTF fsRectf = new FS_RECTF((float) ((double) charBbox.left * (double) fontSize / 1000.0) + originX, (float) ((double) charBbox.top * (double) fontSize / 1000.0) + originY, (float) ((double) charBbox.right * (double) fontSize / 1000.0) + originX, (float) ((double) charBbox.bottom * (double) fontSize / 1000.0) + originY);
    float left1 = fsRectf.left;
    float bottom1 = fsRectf.bottom;
    float right1 = fsRectf.right;
    float bottom2 = fsRectf.bottom;
    float right2 = fsRectf.right;
    float top1 = fsRectf.top;
    float left2 = fsRectf.left;
    float top2 = fsRectf.top;
    fsMatrix.TransformPoint(ref left1, ref bottom1);
    fsMatrix.TransformPoint(ref right1, ref bottom2);
    fsMatrix.TransformPoint(ref right2, ref top1);
    fsMatrix.TransformPoint(ref left2, ref top2);
    return new FS_QUADPOINTSF(left1, bottom1, right1, bottom2, right2, top1, left2, top2);
  }

  internal static FS_RECTF GetBoxFromPos(this PdfTextObject textObj)
  {
    if (textObj.IsRotate())
      return new FS_RECTF();
    int charsCount = textObj.CharsCount;
    if (charsCount == 0)
      return textObj.GetBox();
    Matrix matrix = textObj.MatrixFromPage2();
    PdfFont font = textObj.Font;
    float fontSize = textObj.FontSize;
    float[] pPosArray = new float[charsCount * 2];
    Pdfium.FPDFTextObj_CalcCharPos(textObj.Handle, pPosArray);
    float x1 = pPosArray[0];
    float num1 = pPosArray[pPosArray.Length - 1];
    float num2 = 0.0f;
    int ascent = font.Ascent;
    int descent = font.Descent;
    float num3 = fontSize;
    float num4 = x1;
    float y1 = (float) descent * num3 / (float) (ascent - descent);
    Point point = matrix.Transform(new Point((double) x1, (double) y1));
    float x2 = (float) point.X;
    float y2 = (float) point.Y;
    num4 = num1;
    num2 = (float) ascent * num3 / (float) (ascent - descent);
    point = matrix.Transform(new Point((double) x2, (double) y2));
    float x3 = (float) point.X;
    float y3 = (float) point.Y;
    return new FS_RECTF(Math.Min(x2, x3), Math.Max(y2, y3), Math.Max(x2, x3), Math.Min(y2, y3));
  }

  internal static FS_RECTF GetBox(this PdfTextObject textObj)
  {
    if (textObj.IsRotate())
      return new FS_RECTF();
    float wordSpacing = textObj.WordSpacing;
    float spaceCharWidth = textObj.GetSpaceCharWidth();
    float num1 = textObj.CharIsBlankSpace(0) ? spaceCharWidth : 0.0f;
    float num2 = textObj.CharIsBlankSpace(textObj.CharsCount - 1) ? spaceCharWidth : 0.0f;
    float left;
    float top;
    float right;
    float bottom;
    Pdfium.FPDFPageObj_GetBBox(textObj.Handle, (FS_MATRIX) null, out left, out top, out right, out bottom);
    return (double) Math.Abs(right - left) > 0.5 ? new FS_RECTF(left - num1, top, right + num2, bottom) : new FS_RECTF(left, top, right, bottom);
  }

  internal static FS_RECTF GetBoundingBoxWithoutTransform(this PdfTextObject textObj)
  {
    float left;
    float top;
    float right;
    float bottom;
    Pdfium.FPDFPageObj_GetBBox(textObj.Handle, (FS_MATRIX) null, out left, out top, out right, out bottom);
    return new FS_RECTF(left, top, right, bottom);
  }

  internal static bool IsBlankText(this PdfTextObject textObj)
  {
    if (textObj.CharsCount == 1)
    {
      PdfFont font = textObj.Font;
      if (font != null)
      {
        int charCode;
        textObj.GetCharInfo(0, out charCode, out float _, out float _);
        return font.IsUnicodeCompatible ? font.ToUnicode(charCode) == ' ' : charCode == 32 /*0x20*/;
      }
    }
    return false;
  }

  internal static char GetUnicodeChar(this PdfTextObject textObj, int index)
  {
    PdfFont font = textObj.Font;
    if (font == null)
      return ' ';
    int charCode;
    textObj.GetCharInfo(index, out charCode, out float _, out float _);
    return font.ToUnicode(charCode);
  }

  internal static bool IsBold(this PdfFont font)
  {
    return font.Weight == 700 || font.BaseFontName.ToLowerInvariant().Contains("bold");
  }

  internal static bool IsItalic(this PdfFont font)
  {
    if (font.ItalicAngel != 0)
      return true;
    string lowerInvariant = font.BaseFontName.ToLowerInvariant();
    return lowerInvariant.Contains("italic") || lowerInvariant.Contains("oblique");
  }

  internal static bool IsBold(this PdfTextObject textObj)
  {
    return textObj.Font != null && textObj.Font.IsBold() || textObj.RenderMode == TextRenderingModes.FillThenStroke;
  }

  internal static bool IsItalic(this PdfTextObject textObj)
  {
    if (textObj.Font != null && textObj.Font.IsItalic())
      return true;
    FS_MATRIX textMatrix = Pdfium.FPDFTextObj_GetTextMatrix(textObj.Handle);
    return (double) textMatrix.c > 0.20000000298023224 * (double) textMatrix.a && (double) textMatrix.c <= 0.40000000596046448 * (double) textMatrix.a;
  }

  internal static void SetBold(
    this PdfTextObject textObj,
    PdfDocument doc,
    bool set,
    CultureInfo cultureInfo = null)
  {
    bool flag1 = textObj.IsBold();
    if (flag1 & set || !flag1 && !set)
      return;
    (PdfFont font1, BoldItalicFlags flag2) = textObj.GetStoredFont();
    if (font1 != null && (!set && flag2 == BoldItalicFlags.None || set && flag2 == BoldItalicFlags.Bold))
    {
      textObj.SetFont(font1);
      textObj.RecalcPositionData();
      textObj.SetStoredFont((PdfFont) null, BoldItalicFlags.None);
    }
    else
    {
      PdfFont font2 = textObj.Font;
      if (font2 == null)
        return;
      IntPtr ExtHandle;
      int Charset;
      Pdfium.FPDFFont_GetSubstFont(font2.Handle, out ExtHandle, out string _, out Charset, out FontSubstFlags _, out int _, out int _, out bool _, out int _, out bool _);
      FontCharSet fontCharSet = (FontCharSet) Charset;
      if (ExtHandle == IntPtr.Zero && font2.IsEmbedded)
        fontCharSet = textObj.GetCharset(cultureInfo);
      PdfFont font3 = TextObjectExtensions.LoadBoldItalicSubstFont(doc, font2, fontCharSet, set, false);
      if (font3 != null && (set && font3.IsBold() || !set && !font3.IsBold()))
      {
        if (textObj.GetStoredFont().font == null)
          textObj.SetStoredFont(font2, set ? BoldItalicFlags.None : BoldItalicFlags.Bold);
        textObj.SetFont(font3);
        if (!set)
          textObj.RenderMode = TextRenderingModes.Fill;
        textObj.RecalcPositionData();
      }
      else
      {
        textObj.RenderMode = set ? TextRenderingModes.FillThenStroke : TextRenderingModes.Fill;
        textObj.RecalcPositionData();
        if (!set)
          return;
        float width = textObj.GetFontSize() / 35f;
        Pdfium.FPDFPageObj_SetLineWidth(textObj.Handle, width);
      }
    }
  }

  internal static void SetItalic(
    this PdfTextObject textObj,
    PdfDocument doc,
    bool set,
    CultureInfo cultureInfo = null)
  {
    bool flag1 = textObj.IsItalic();
    if (flag1 & set || !flag1 && !set)
      return;
    (PdfFont font1, BoldItalicFlags flag2) = textObj.GetStoredFont();
    if (font1 != null && (!set && flag2 == BoldItalicFlags.None || set && flag2 == BoldItalicFlags.Italic))
    {
      textObj.SetFont(font1);
      textObj.RecalcPositionData();
      textObj.SetStoredFont((PdfFont) null, BoldItalicFlags.None);
    }
    else
    {
      PdfFont font2 = textObj.Font;
      if (font2 == null)
        return;
      IntPtr ExtHandle;
      int Charset;
      Pdfium.FPDFFont_GetSubstFont(font2.Handle, out ExtHandle, out string _, out Charset, out FontSubstFlags _, out int _, out int _, out bool _, out int _, out bool _);
      FontCharSet fontCharSet = (FontCharSet) Charset;
      if (ExtHandle == IntPtr.Zero && font2.IsEmbedded)
        fontCharSet = textObj.GetCharset(cultureInfo);
      PdfFont font3 = TextObjectExtensions.LoadBoldItalicSubstFont(doc, font2, fontCharSet, false, set);
      if (font3 != null && (set && font3.IsItalic() || !set && !font3.IsItalic()))
      {
        if (textObj.GetStoredFont().font == null)
          textObj.SetStoredFont(font2, set ? BoldItalicFlags.None : BoldItalicFlags.Italic);
        textObj.SetFont(font3);
        if (!set)
        {
          FS_MATRIX textMatrix = Pdfium.FPDFTextObj_GetTextMatrix(textObj.Handle);
          textMatrix.c = 0.0f;
          Pdfium.FPDFTextObj_SetTextMatrix(textObj.Handle, textMatrix);
        }
        textObj.RecalcPositionData();
      }
      else
      {
        FS_MATRIX textMatrix = Pdfium.FPDFTextObj_GetTextMatrix(textObj.Handle);
        if (set)
          textMatrix.c = 0.3333f * textMatrix.a;
        else if (textObj.IsItalic())
          textMatrix.c = 0.0f;
        Pdfium.FPDFTextObj_SetTextMatrix(textObj.Handle, textMatrix);
        textObj.RecalcPositionData();
      }
    }
  }

  internal static PdfFont LoadBoldItalicSubstFont(
    PdfDocument document,
    PdfFont font,
    FontCharSet fontCharSet,
    bool bold,
    bool italic)
  {
    try
    {
      IntPtr ExtHandle;
      string Family;
      Pdfium.FPDFFont_GetSubstFont(font.Handle, out ExtHandle, out Family, out int _, out FontSubstFlags _, out int _, out int _, out bool _, out int _, out bool _);
      if (ExtHandle != IntPtr.Zero && !string.IsNullOrEmpty(Family))
      {
        FontFlags flags = font.Flags;
        if (bold)
          flags |= FontFlags.PDFFONT_FORCEBOLD;
        if (italic)
          flags |= FontFlags.PDFFONT_ITALIC;
        string faceName = Family;
        if (bold | italic)
        {
          StringBuilder stringBuilder = new StringBuilder(faceName);
          stringBuilder.Append(',');
          if (bold)
            stringBuilder.Append("Bold");
          if (italic)
            stringBuilder.Append("Italic");
          faceName = stringBuilder.ToString();
        }
        int italicAngel = italic ? 12 : 0;
        Patagames.Pdf.Enums.FontWeight weight = bold ? Patagames.Pdf.Enums.FontWeight.FW_BOLD : Patagames.Pdf.Enums.FontWeight.FW_NORMAL;
        return PdfFont.CreateFont(document, faceName, true, flags, weight, italicAngel, fontCharSet, font.IsVertWriting);
      }
    }
    catch
    {
    }
    return (PdfFont) null;
  }

  internal static void Offset(this PdfTextObject textObj, float dx, float dy)
  {
    float x1;
    float y1;
    Pdfium.FPDFTextObj_GetPos(textObj.Handle, out x1, out y1);
    float x2 = x1 + dx;
    float y2 = y1 + dy;
    Pdfium.FPDFTextObj_SetPosition(textObj.Handle, x2, y2);
  }

  internal static bool DeleteText(
    this PdfTextObject textObj,
    int startIndex,
    int endIndex,
    out bool totalAndRemoved)
  {
    totalAndRemoved = false;
    int charsCount = textObj.CharsCount;
    if (startIndex < 0)
      startIndex = 0;
    else if (startIndex >= charsCount)
      startIndex = charsCount - 1;
    if (endIndex < 0)
      endIndex = 0;
    else if (endIndex >= charsCount)
      endIndex = charsCount - 1;
    int num = endIndex - startIndex + 1;
    if (num == charsCount)
    {
      textObj.Container.Remove((PdfPageObject) textObj);
      totalAndRemoved = true;
      return true;
    }
    if (num == 0)
      return false;
    int countChars = charsCount - num;
    int[] chars = new int[countChars];
    float[] kernings = new float[countChars];
    int index1 = 0;
    for (int index2 = 0; index2 < charsCount; ++index2)
    {
      if (index2 < startIndex || index2 > endIndex)
      {
        int charCode;
        float kerning;
        textObj.GetCharInfo(index2, out charCode, out kerning);
        chars[index1] = charCode;
        kernings[index1] = kerning;
        ++index1;
      }
    }
    Pdfium.FPDFTextObj_SetEmpty(textObj.Handle);
    Pdfium.FPDFTextObj_SetText(textObj.Handle, countChars, chars, kernings);
    textObj.RecalcPositionData();
    return true;
  }

  internal static bool AppendSpace(this PdfTextObject textObj)
  {
    PdfFont font = textObj.Font;
    if (font == null || !textObj.Font.ContainsChar(' ', false))
      return false;
    int charsCount = textObj.CharsCount;
    int[] chars = new int[charsCount + 1];
    float[] kernings = new float[charsCount + 1];
    for (int index = 0; index < charsCount; ++index)
    {
      int charCode;
      float kerning;
      textObj.GetCharInfo(index, out charCode, out kerning);
      chars[index] = charCode;
      kernings[index] = kerning;
    }
    chars[charsCount] = font.ToCharCode(' ');
    kernings[charsCount] = 0.0f;
    Pdfium.FPDFTextObj_SetEmpty(textObj.Handle);
    Pdfium.FPDFTextObj_SetText(textObj.Handle, chars.Length, chars, kernings);
    textObj.RecalcPositionData();
    return true;
  }

  internal static bool CanSplitAt(this PdfTextObject textObj, int charIndex)
  {
    char unicodeChar = textObj.GetUnicodeChar(charIndex);
    return unicodeChar != char.MaxValue && (CharHelper.IsPunctuation((int) unicodeChar) || unicodeChar == ' ' || unicodeChar == '\t' || CharHelper.IsCJK(unicodeChar));
  }

  internal static PdfTextObject SplitAt(this PdfTextObject textObject, int charIndex)
  {
    PdfTextObject newTextObj;
    if (charIndex <= 0 || charIndex >= textObject.CharsCount || !TextObjectExtensions.SplitTextObj(textObject, charIndex, false, out newTextObj))
      return (PdfTextObject) null;
    PdfPageObjectsCollection container = textObject.Container;
    int index = container.IndexOf((PdfPageObject) textObject) + 1;
    container.Insert(index, (PdfPageObject) newTextObj);
    return newTextObj;
  }

  internal static bool IsCJK(this PdfTextObject textObj)
  {
    PdfFont font = textObj.Font;
    if (font == null)
      return false;
    int Charset;
    bool SubstOfCJK;
    Pdfium.FPDFFont_GetSubstFont(font.Handle, out IntPtr _, out string _, out Charset, out FontSubstFlags _, out int _, out int _, out SubstOfCJK, out int _, out bool _);
    if (SubstOfCJK)
      return true;
    FontCharSet fontCharSet = (FontCharSet) Charset;
    int num;
    switch (fontCharSet)
    {
      case FontCharSet.SHIFTJIS_CHARSET:
      case FontCharSet.HANGEUL_CHARSET:
      case FontCharSet.GB2312_CHARSET:
        num = 1;
        break;
      default:
        num = fontCharSet == FontCharSet.CHINESEBIG5_CHARSET ? 1 : 0;
        break;
    }
    return num != 0 || textObj.CharIsCJK(0);
  }

  internal static bool CharIsCJK(this PdfTextObject textObj, int charIndex)
  {
    char unicodeChar = textObj.GetUnicodeChar(charIndex);
    return unicodeChar > char.MinValue && CharHelper.IsCJK(unicodeChar);
  }

  internal static bool CharIsBlankSpace(this PdfTextObject textObj, int index)
  {
    char unicodeChar = textObj.GetUnicodeChar(index);
    return unicodeChar == ' ' || unicodeChar == '\t';
  }

  internal static bool ContainsChar(this PdfFont font, char ch, bool forceVisible = true)
  {
    int charCode = font.ToCharCode(ch);
    if (charCode >= 0)
    {
      char unicode = font.ToUnicode(charCode);
      if ((int) ch == (int) unicode)
      {
        if (!forceVisible)
          return true;
        return (font.FontType != FontTypes.PDFFONT_CIDFONT ? (uint) font.GetCharFontWidth(charCode) : (uint) font.GetCharTypeWidth(charCode, out bool _)) > 0U;
      }
    }
    return false;
  }

  internal static int[] GetSymbolAndSpacePos(this PdfTextObject textObj)
  {
    PdfFont font = textObj.Font;
    if (font == null)
      return Array.Empty<int>();
    List<int> intList = new List<int>();
    int charsCount = textObj.CharsCount;
    bool flag1 = charsCount > 0 && CharHelper.IsPunctuation(0);
    int charCode1 = -1;
    char word = char.MinValue;
    int charCode2 = -1;
    for (int index = 0; index < charsCount; ++index)
    {
      float num1;
      float num2;
      char ch;
      if (index == 0)
      {
        textObj.GetCharInfo(index + 1, out charCode2, out num1, out num2);
        ch = font.ToUnicode(charCode2);
      }
      else
      {
        charCode2 = charCode1;
        ch = word;
      }
      if (index + 1 < charsCount)
      {
        textObj.GetCharInfo(index + 1, out charCode1, out num2, out num1);
        word = font.ToUnicode(charCode1);
      }
      bool flag2 = flag1;
      flag1 = index + 1 < charsCount && CharHelper.IsPunctuation((int) word);
      bool flag3 = CharHelper.IsCJK(ch);
      if (flag2 || ch == ' ' || !flag1 & flag3)
        intList.Add(index);
    }
    return intList.ToArray();
  }

  internal static bool SplitTextObj(
    PdfTextObject textObj,
    int splitAtIndex,
    bool skipIfSpace,
    out PdfTextObject newTextObj)
  {
    if (splitAtIndex <= 0 || splitAtIndex >= textObj.CharsCount)
    {
      newTextObj = (PdfTextObject) null;
      return false;
    }
    FS_MATRIX matrix = textObj.Matrix;
    int charCode1;
    float originX;
    float originY;
    Pdfium.FPDFTextObj_GetCharInfo(textObj.Handle, splitAtIndex, out charCode1, out originX, out originY);
    PdfFont font = textObj.Font;
    bool flag = false;
    if (font != null)
    {
      if (font.IsUnicodeCompatible)
      {
        char unicode = font.ToUnicode(charCode1);
        flag = unicode == ' ' || unicode == '\t';
      }
      else
        flag = charCode1 == 32 /*0x20*/ || charCode1 == 9;
    }
    int num = 0;
    if (flag & skipIfSpace)
      num = 1;
    int charsCount = textObj.CharsCount;
    int countChars = charsCount - splitAtIndex - num;
    if (countChars == 0)
    {
      newTextObj = (PdfTextObject) null;
      return false;
    }
    float x = originX;
    float y = originY;
    matrix.TransformPoint(ref x, ref y);
    int[] numArray1 = new int[charsCount];
    float[] numArray2 = new float[charsCount];
    for (int index = 0; index < charsCount; ++index)
    {
      int charCode2;
      float kerning;
      textObj.GetCharInfo(index, out charCode2, out kerning);
      numArray1[index] = charCode2;
      numArray2[index] = kerning;
    }
    Pdfium.FPDFTextObj_SetEmpty(textObj.Handle);
    Pdfium.FPDFTextObj_SetText(textObj.Handle, splitAtIndex, numArray1, numArray2);
    textObj.RecalcPositionData();
    int[] array1 = ((IEnumerable<int>) numArray1).Skip<int>(splitAtIndex + num).ToArray<int>();
    float[] array2 = ((IEnumerable<float>) numArray2).Skip<float>(splitAtIndex + num).ToArray<float>();
    newTextObj = (PdfTextObject) textObj.Clone();
    Pdfium.FPDFTextObj_SetEmpty(newTextObj.Handle);
    Pdfium.FPDFTextObj_SetText(newTextObj.Handle, countChars, array1, array2);
    newTextObj.Location = new FS_POINTF(x, y);
    return true;
  }

  internal static FontCharSet GetCharset(this PdfTextObject textObj, CultureInfo cultureInfo = null)
  {
    string text = textObj.GetText();
    if (string.IsNullOrEmpty(text))
      return FontCharSet.ANSI_CHARSET;
    for (int index = 0; index < text.Length; ++index)
    {
      char ch = text[index];
      if (CharHelper.IsCJK(ch))
        return TextObjectExtensions.GetCultureCJKCharSet(cultureInfo);
      if (ch >= '\u0E00' && ch <= '\u0E7F')
        return FontCharSet.THAI_CHARSET;
      if (ch >= 'Ͱ' && ch <= 'Ͽ' || ch >= 'ἀ' && ch <= '\u1FFF')
        return FontCharSet.GREEK_CHARSET;
      if (ch >= '\u0600' && ch <= 'ۿ' || ch >= 'ﭐ' && ch <= 'ﻼ')
        return FontCharSet.ARABIC_CHARSET;
      if (ch >= '\u0590' && ch <= '\u05FF')
        return FontCharSet.HEBREW_CHARSET;
      if (ch >= 'Ѐ' && ch <= 'ӿ')
        return FontCharSet.RUSSIAN_CHARSET;
      if (ch == 'Ğ' || ch == 'ğ' || ch == 'İ' || ch == 'ı' || ch == 'Ş' || ch == 'ş')
        return FontCharSet.TURKISH_CHARSET;
      if (ch >= 'Ā' && ch <= 'ɏ')
        return FontCharSet.EASTEUROPE_CHARSET;
      if (ch >= 'Ḁ' && ch <= 'ỿ')
        return FontCharSet.VIETNAMESE_CHARSET;
    }
    return FontCharSet.ANSI_CHARSET;
  }

  internal static string GetText(this PdfTextObject textObj)
  {
    return Pdfium.FPDFTextObj_GetTextUnicode(textObj.Handle);
  }

  internal static void SetFont(this PdfTextObject textObj, PdfFont font)
  {
    PdfFont font1 = textObj.Font;
    if (font1.Handle == font.Handle)
      return;
    int charsCount = textObj.CharsCount;
    int[] chars = new int[charsCount];
    float[] kernings = new float[charsCount];
    for (int index = 0; index < charsCount; ++index)
      textObj.GetCharInfo(index, out chars[index], out kernings[index]);
    bool flag = true;
    for (int index = 0; index < charsCount; ++index)
    {
      char unicode = font1.ToUnicode(chars[index]);
      int charCode = font.ToCharCode(unicode);
      if (charCode == -1)
      {
        flag = false;
        break;
      }
      chars[index] = charCode;
    }
    if (!flag)
      return;
    textObj.Font = font;
    Pdfium.FPDFTextObj_SetEmpty(textObj.Handle);
    Pdfium.FPDFTextObj_SetText(textObj.Handle, charsCount, chars, kernings);
  }

  internal static float GetFontSize(this PdfTextObject textObj)
  {
    return textObj.FontSize * (float) textObj.MatrixFromPage2().M11;
  }

  internal static void SetFontSize(this PdfTextObject textObj, float fontSize)
  {
    Matrix matrix = textObj.MatrixFromPage2();
    float size = 0.0f;
    if (matrix.M11 != 0.0)
      size = fontSize / (float) matrix.M11;
    Pdfium.FPDFTextObj_SetFontSize(textObj.Handle, size);
    textObj.RecalcPositionData();
  }

  private static FontCharSet GetCultureCJKCharSet(CultureInfo cultureInfo)
  {
    if (cultureInfo == null)
      return FontCharSet.GB2312_CHARSET;
    for (CultureInfo cultureInfo1 = cultureInfo; cultureInfo1 != null && !string.IsNullOrEmpty(cultureInfo1.Name) && !string.Equals(cultureInfo1.Name, "zh-hans", StringComparison.OrdinalIgnoreCase); cultureInfo1 = cultureInfo1.Parent)
    {
      if (string.Equals(cultureInfo1.Name, "zh-hant", StringComparison.OrdinalIgnoreCase))
        return FontCharSet.CHINESEBIG5_CHARSET;
      if (string.Equals(cultureInfo1.Name, "jp"))
        return FontCharSet.SHIFTJIS_CHARSET;
      if (string.Equals(cultureInfo1.Name, "kr"))
        return FontCharSet.HANGEUL_CHARSET;
    }
    return FontCharSet.GB2312_CHARSET;
  }
}
