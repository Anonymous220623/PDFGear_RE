// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.RtlRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class RtlRenderer
{
  private const char c_openBracket = '(';
  private const char c_closeBracket = ')';
  private static Bitmap s_bmp = new Bitmap(1, 1);
  private static Dictionary<Font, IntPtr> m_hFontCollection = new Dictionary<Font, IntPtr>();

  private RtlRenderer() => throw new NotImplementedException();

  public static string[] Layout(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    bool wordSpace,
    PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    string[] strArray;
    if (font.Unicode)
      strArray = font.Font == null || format.isCustomRendering ? RtlRenderer.CustomLayout(line, font, rtl, wordSpace, format) : RtlRenderer.SystemLayout(line, font, rtl, wordSpace, format);
    else
      strArray = new string[1]{ line };
    return strArray;
  }

  internal static string[] SplitLayout(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    bool wordSpace,
    PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    string[] strArray = (string[]) null;
    bool flag = font.Unicode && font.Font != null;
    if (flag && !format.isCustomRendering)
      strArray = RtlRenderer.SystemSplitLayout(line, font, rtl, wordSpace);
    if (!flag || strArray == null)
      strArray = RtlRenderer.CustomSplitLayout(line, font, rtl, wordSpace, format);
    return strArray;
  }

  private static bool IsEnglish(string word)
  {
    char ch = word.Length > 0 ? word[0] : char.MinValue;
    return ch >= char.MinValue && ch < 'ÿ';
  }

  private static void KeepOrder(
    string[] words,
    int startIndex,
    int count,
    string[] result,
    int resultIndex)
  {
    int num = 0;
    int index = resultIndex - count + 1;
    while (num < count)
    {
      result[index] = words[num + startIndex];
      ++num;
      ++index;
    }
  }

  private static string[] SystemLayout(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    bool wordSpace,
    PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    string[] strArray1;
    if (!wordSpace)
    {
      strArray1 = new string[1]
      {
        RtlRenderer.SystemLayout(line, font, rtl, format)
      };
    }
    else
    {
      string[] strArray2 = RtlRenderer.SplitLayout(line, font, rtl, wordSpace, format);
      string[] strArray3 = new string[strArray2.Length];
      int index = 0;
      for (int length = strArray2.Length; index < length; ++index)
        strArray3[index] = RtlRenderer.AddChars(font, strArray2[index], format);
      strArray1 = strArray3;
    }
    return strArray1;
  }

  private static string SystemLayout(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    ushort[] glyphs = (ushort[]) null;
    string str;
    if (RtlRenderer.GetGlyphIndices(line, font, rtl, out glyphs) && glyphs != null && glyphs.Length > 0)
    {
      str = RtlRenderer.AddChars(font, glyphs, format);
    }
    else
    {
      string line1 = RtlRenderer.CustomLayout(line, rtl, format);
      str = RtlRenderer.AddChars(font, line1, format);
    }
    return str;
  }

  internal static bool GetGlyphIndices(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    out ushort[] glyphs,
    bool custom)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    glyphs = (ushort[]) null;
    if (line.Length == 0)
      return false;
    string str = new ArabicShapeRenderer().Shape(line.ToCharArray(), 0);
    TtfReader ttfReader = (font.InternalFont as UnicodeTrueTypeFont).TtfReader;
    ttfReader.m_missedGlyphCount = 0;
    glyphs = new ushort[str.Length];
    int num = 0;
    int index = 0;
    for (int length = str.Length; index < length; ++index)
    {
      char charCode = str[index];
      TtfGlyphInfo glyph = ttfReader.GetGlyph(charCode);
      if (!glyph.Empty)
        glyphs[num++] = (ushort) glyph.Index;
    }
    return true;
  }

  private static string[] CustomLayout(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    bool wordSpace,
    PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    string line1 = RtlRenderer.TrimLRM(format == null || format.TextDirection == PdfTextDirection.None ? RtlRenderer.CustomLayout(line, rtl, format) : RtlRenderer.CustomLayout(new ArabicShapeRenderer().Shape(line.ToCharArray(), 0), rtl, format));
    string[] strArray1;
    if (wordSpace)
    {
      string[] strArray2 = line1.Split((char[]) null);
      int length = strArray2.Length;
      for (int index = 0; index < length; ++index)
        strArray2[index] = RtlRenderer.AddChars(font, strArray2[index], format);
      strArray1 = strArray2;
    }
    else
      strArray1 = new string[1]
      {
        RtlRenderer.AddChars(font, line1, format)
      };
    return strArray1;
  }

  internal static string TrimLRM(string text)
  {
    string str = text;
    if (str != null)
    {
      char[] charArray = str.ToCharArray();
      if (charArray.Length > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < charArray.Length; ++index)
        {
          if (!RtlRenderer.InvisibleCharacter((int) charArray[index]))
            stringBuilder.Append(charArray[index]);
        }
        str = stringBuilder.ToString();
      }
    }
    return str;
  }

  private static bool InvisibleCharacter(int character)
  {
    if (character >= 8203 && character <= 8207)
      return true;
    return character >= 8234 && character <= 8238;
  }

  private static string CustomLayout(string line, bool rtl, PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    return format == null || format.TextDirection == PdfTextDirection.None ? (!rtl ? RtlRenderer.CustomLtr(line) : RtlRenderer.CustomRtl(line)) : new Bidi().GetLogicalToVisualString(line, rtl);
  }

  private static string[] ReverseWords(string[] words)
  {
    int length = words != null ? words.Length : throw new ArgumentNullException(nameof (words));
    string[] result = new string[length];
    string word = (string) null;
    int num = 0;
    int count = 0;
    int index = 0;
    int resultIndex = length - 1;
    while (index < length)
    {
      switch (num)
      {
        case 0:
          word = words[index];
          if (RtlRenderer.IsEnglish(word))
          {
            count = 0;
            num = 1;
            continue;
          }
          result[resultIndex] = word;
          ++index;
          --resultIndex;
          continue;
        case 1:
          ++count;
          ++index;
          if (index < length)
            word = words[index];
          if (index >= length || !RtlRenderer.IsEnglish(word))
          {
            RtlRenderer.KeepOrder(words, index - count, count, result, resultIndex);
            resultIndex -= count;
            num = 0;
            continue;
          }
          continue;
        default:
          throw new PdfException("Internal error.");
      }
    }
    return result;
  }

  internal static bool GetGlyphIndices(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    out ushort[] glyphs)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    glyphs = (ushort[]) null;
    RtlApi.SCRIPT_ITEM[] items = (RtlApi.SCRIPT_ITEM[]) null;
    int count = 0;
    bool glyphIndices = RtlRenderer.StringItemize(line, rtl, out items, out count);
    if (glyphIndices)
    {
      int[] visualToLogical = (int[]) null;
      int[] logicalToVisual = (int[]) null;
      glyphIndices = RtlRenderer.StringLayout(RtlRenderer.GetBidiLevel(items, count), count, out visualToLogical, out logicalToVisual);
      if (glyphIndices)
        glyphIndices = RtlRenderer.StringShape(line, items, font.Font, count, visualToLogical, out glyphs);
    }
    return glyphIndices;
  }

  private static bool StringItemize(
    string text,
    bool rtl,
    out RtlApi.SCRIPT_ITEM[] items,
    out int count)
  {
    if (text == null || text.Length == 0)
    {
      items = (RtlApi.SCRIPT_ITEM[]) null;
      count = 0;
      return false;
    }
    int num1 = 10;
    int num2 = 0;
    int val2 = text.Length + 1;
    count = 0;
    uint num3;
    do
    {
      RtlApi.SCRIPT_CONTROL psControl = new RtlApi.SCRIPT_CONTROL();
      RtlApi.SCRIPT_STATE psState = new RtlApi.SCRIPT_STATE();
      int cMaxItems = Math.Max(16 /*0x10*/, val2);
      items = new RtlApi.SCRIPT_ITEM[cMaxItems];
      if (rtl)
        psState.val |= (ushort) 1;
      num3 = Environment.Version.Major >= 4 ? RtlApi.ScriptItemize(text, text.Length, cMaxItems, ref psControl, ref psState, ref items[0], ref count) : (IntPtr.Size == 8 ? RtlApi.ScriptItemize(text, text.Length, cMaxItems, ref psControl, ref psState, items[0], ref count) : RtlApi.ScriptItemize(text, text.Length, cMaxItems, ref psControl, ref psState, ref items[0], ref count));
      if (num3 != 0U && num3 != 2147942414U /*0x8007000E*/ || num3 != 0U && num2 >= num1)
      {
        items = (RtlApi.SCRIPT_ITEM[]) null;
        count = 0;
        return false;
      }
      ++num2;
      val2 = cMaxItems * 2;
    }
    while (num3 != 0U);
    return true;
  }

  private static bool StringShape(
    string text,
    RtlApi.SCRIPT_ITEM[] items,
    Font font,
    int count,
    int[] visualToLogical,
    out ushort[] glyphs)
  {
    if (text == null || text.Length == 0 || items == null || items.Length < count + 1 || font == null || visualToLogical == null || visualToLogical.Length != count)
    {
      glyphs = (ushort[]) null;
      return false;
    }
    IntPtr zero1 = IntPtr.Zero;
    IntPtr hgdiobj;
    if (RtlRenderer.m_hFontCollection.ContainsKey(font))
    {
      hgdiobj = RtlRenderer.m_hFontCollection[font];
    }
    else
    {
      try
      {
        hgdiobj = font.ToHfont();
      }
      catch
      {
        font = new Font("Arial", font.Size, font.Style);
        hgdiobj = font.ToHfont();
      }
      if (!RtlRenderer.m_hFontCollection.ContainsKey(font))
        RtlRenderer.m_hFontCollection.Add(font, hgdiobj);
    }
    System.Drawing.Graphics graphics;
    lock (RtlRenderer.s_bmp)
      graphics = System.Drawing.Graphics.FromImage((Image) RtlRenderer.s_bmp);
    IntPtr hdc = graphics.GetHdc();
    GdiApi.SelectObject(hdc, hgdiobj);
    IntPtr zero2 = IntPtr.Zero;
    int num1 = 10;
    ArrayList glyphs1 = new ArrayList();
    try
    {
      for (int index1 = 0; index1 < count; ++index1)
      {
        int index2 = visualToLogical[index1];
        int iCharPos = items[index2 + 1].iCharPos;
        string pwcChars = text.Substring(items[index2].iCharPos, iCharPos - items[index2].iCharPos);
        int num2 = 0;
        int cMaxGlyphs = 0;
        ushort[] pwOutGlyphs;
        int pcGlyphs;
        uint num3;
        do
        {
          cMaxGlyphs += pwcChars.Length * 3 / 2;
          pwOutGlyphs = new ushort[cMaxGlyphs];
          ushort[] numArray = new ushort[cMaxGlyphs];
          RtlApi.SCRIPT_VISATTR[] scriptVisattrArray = new RtlApi.SCRIPT_VISATTR[cMaxGlyphs];
          pcGlyphs = 0;
          num3 = RtlApi.ScriptShape(hdc, ref zero2, pwcChars, pwcChars.Length, cMaxGlyphs, ref items[index2].a, ref pwOutGlyphs[0], ref numArray[0], ref scriptVisattrArray[0], ref pcGlyphs);
          if (num3 == 2147746304U /*0x80040200*/)
            items[index2].a.val &= (ushort) 64512;
          else if (num3 != 0U && num3 != 2147942414U /*0x8007000E*/ || num3 != 0U && num2 >= num1)
          {
            glyphs = (ushort[]) null;
            return false;
          }
          ++num2;
        }
        while (num3 != 0U);
        if (num3 == 0U)
          RtlRenderer.AddGlyphs(glyphs1, pwOutGlyphs, pcGlyphs);
      }
    }
    finally
    {
      graphics.ReleaseHdc(hdc);
      graphics.Dispose();
    }
    glyphs = (ushort[]) glyphs1.ToArray(typeof (ushort));
    return true;
  }

  private static bool StringLayout(
    byte[] bidi,
    int count,
    out int[] visualToLogical,
    out int[] logicalToVisual)
  {
    visualToLogical = (int[]) null;
    logicalToVisual = (int[]) null;
    bool flag = false;
    if (bidi != null && bidi.Length == count && count > 0)
    {
      visualToLogical = new int[count];
      logicalToVisual = new int[count];
      flag = true;
      if (RtlApi.ScriptLayout(count, ref bidi[0], ref visualToLogical[0], ref logicalToVisual[0]) != 0U)
      {
        flag = false;
        visualToLogical = (int[]) null;
        logicalToVisual = (int[]) null;
      }
    }
    return flag;
  }

  private static void AddGlyphs(ArrayList glyphs, ushort[] pwOutGlyphs, int count)
  {
    if (glyphs == null || pwOutGlyphs == null || pwOutGlyphs.Length < count)
      return;
    for (int index = 0; index < count; ++index)
    {
      ushort pwOutGlyph = pwOutGlyphs[index];
      glyphs.Add((object) pwOutGlyph);
    }
  }

  private static byte[] GetBidiLevel(RtlApi.SCRIPT_ITEM[] items, int count)
  {
    byte[] bidiLevel = (byte[]) null;
    if (items != null && items.Length >= count)
    {
      bidiLevel = new byte[count];
      for (int index = 0; index < count; ++index)
      {
        int num = RtlApi.Decrypt((int) items[index].a.s.val, 0, 5);
        bidiLevel[index] = (byte) num;
      }
    }
    return bidiLevel;
  }

  private static string AddChars(PdfTrueTypeFont font, ushort[] glyphs, PdfStringFormat format)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (glyphs == null)
      throw new ArgumentNullException(nameof (glyphs));
    if (font.InternalFont is UnicodeTrueTypeFont internalFont && format != null && format.ComplexScript)
      internalFont.SetSymbols(glyphs, true);
    else
      font.SetSymbols(glyphs);
    char[] chArray = new char[glyphs.Length];
    for (int index = 0; index < glyphs.Length; ++index)
      chArray[index] = (char) glyphs[index];
    return PdfString.ByteToString(PdfString.ToUnicodeArray(new string(chArray), false));
  }

  private static string AddChars(PdfTrueTypeFont font, string line, PdfStringFormat format)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    string text = line != null ? line : throw new ArgumentNullException(nameof (line));
    UnicodeTrueTypeFont internalFont = font.InternalFont as UnicodeTrueTypeFont;
    TtfReader ttfReader = internalFont.TtfReader;
    if (format != null && format.ComplexScript)
      internalFont.SetSymbols(line, true);
    else
      font.SetSymbols(text);
    return PdfString.ByteToString(PdfString.ToUnicodeArray(ttfReader.ConvertString(text), false));
  }

  private static string[] SystemSplitLayout(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    bool wordSpace)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    string[] strArray = (string[]) null;
    ushort[] glyphs = (ushort[]) null;
    if (RtlRenderer.GetGlyphIndices(line, font, rtl, out glyphs) && glyphs != null && glyphs.Length > 0)
    {
      TtfReader ttfReader = (font.InternalFont as UnicodeTrueTypeFont).TtfReader;
      char[] chArray = new char[glyphs.Length];
      for (int index = 0; index < glyphs.Length; ++index)
      {
        int glyphIndex = (int) glyphs[index];
        TtfGlyphInfo glyph = ttfReader.GetGlyph(glyphIndex);
        if (!glyph.Empty)
        {
          char charCode = (char) glyph.CharCode;
          chArray[index] = charCode;
        }
      }
      strArray = new string(chArray).Split((char[]) null);
    }
    return strArray;
  }

  private static string CustomRtl(string text)
  {
    string str = text != null ? text : throw new ArgumentNullException(nameof (text));
    char[] charArray = str.ToCharArray();
    bool flag1 = true;
    bool flag2 = true;
    ushort[] numArray = new ushort[text.Length];
    KernelApi.GetStringTypeExW(2048U /*0x0800*/, StringInfoType.CT_TYPE2, text, text.Length, numArray);
    int indexCursor = numArray.Length - 1;
    int indexLength = 0;
    if (RtlRenderer.ContainsRTLSymbol(numArray))
    {
      int index1 = 0;
      for (int length1 = numArray.Length; index1 < length1; ++index1)
      {
        char symbol = str[index1];
        ushort symbolCode = numArray[index1];
        if (RtlRenderer.IsLTRText(symbolCode))
        {
          RtlRenderer.WriteInLTR(charArray, symbol, true, ref indexCursor, ref indexLength);
          flag1 = false;
        }
        else if (RtlRenderer.IsRTLSymbol(symbolCode))
        {
          RtlRenderer.SaveSymbol(charArray, symbol, true, ref indexCursor, ref indexLength);
          flag1 = true;
        }
        else if (RtlRenderer.IsGeneralEuroNumber(symbolCode) || RtlRenderer.IsEuroTerminator(symbolCode) && RtlRenderer.IsBackEuroNumber(numArray, index1) && flag1)
          RtlRenderer.WriteInLTR(charArray, symbol, true, ref indexCursor, ref indexLength);
        else if (RtlRenderer.IsBracket(symbol))
        {
          RtlRenderer.ReverseBrackets(charArray, symbol, ref indexCursor, ref indexLength);
        }
        else
        {
          int index2 = index1;
          for (int length2 = numArray.Length; index2 < length2; ++index2)
          {
            if (RtlRenderer.IsLTRText(numArray[index2]) || !flag1 && RtlRenderer.IsEuroNumber(numArray[index2]))
            {
              flag2 = false;
              index2 = length2;
            }
            else if (RtlRenderer.IsRTLSymbol(numArray[index2]) || flag1 && RtlRenderer.IsEuroNumber(numArray[index2]))
            {
              flag2 = true;
              index2 = length2;
            }
          }
          if (!flag1 && !flag2 && index1 != length1 - 1)
            RtlRenderer.WriteInLTR(charArray, symbol, true, ref indexCursor, ref indexLength);
          else
            RtlRenderer.SaveSymbol(charArray, symbol, true, ref indexCursor, ref indexLength);
        }
      }
    }
    return new string(charArray);
  }

  private static string CustomLtr(string text)
  {
    string str = text != null ? text : throw new ArgumentNullException(nameof (text));
    char[] charArray = str.ToCharArray();
    bool flag1 = true;
    bool flag2 = true;
    ushort[] numArray = new ushort[text.Length];
    KernelApi.GetStringTypeExW(2048U /*0x0800*/, StringInfoType.CT_TYPE2, text, text.Length, numArray);
    int indexCursor = 0;
    int indexLength = 0;
    if (RtlRenderer.ContainsRTLSymbol(numArray))
    {
      int index1 = 0;
      for (int length1 = numArray.Length; index1 < length1; ++index1)
      {
        char symbol = str[index1];
        ushort symbolCode = numArray[index1];
        if (RtlRenderer.IsRTLSymbol(symbolCode))
        {
          RtlRenderer.WriteInLTR(charArray, symbol, false, ref indexCursor, ref indexLength);
          flag1 = false;
        }
        else if (RtlRenderer.IsLTRText(symbolCode))
        {
          RtlRenderer.SaveSymbol(charArray, symbol, false, ref indexCursor, ref indexLength);
          flag1 = true;
        }
        else if (RtlRenderer.IsGeneralEuroNumber(symbolCode) || RtlRenderer.IsEuroTerminator(symbolCode) && RtlRenderer.IsNextEuroNumber(numArray, index1))
        {
          if (flag1)
          {
            RtlRenderer.SaveSymbol(charArray, symbol, false, ref indexCursor, ref indexLength);
            flag1 = true;
          }
          else
          {
            RtlRenderer.WriteInLTR(charArray, symbol, false, ref indexCursor, ref indexLength);
            ++indexCursor;
            --indexLength;
            flag1 = false;
          }
        }
        else if (RtlRenderer.IsWhitespace(symbolCode) && RtlRenderer.IsBackEuroNumber(numArray, index1) && !flag1)
        {
          for (int index2 = indexLength + indexCursor; RtlRenderer.IsEuroNumber(numArray[index2 - 1]); --index2)
          {
            ++indexLength;
            --indexCursor;
          }
          RtlRenderer.WriteInLTR(charArray, symbol, false, ref indexCursor, ref indexLength);
          flag1 = false;
        }
        else
        {
          int index3 = index1;
          for (int length2 = numArray.Length; index3 < length2; ++index3)
          {
            if (RtlRenderer.IsRTLText(numArray[index3]) || !flag1 && RtlRenderer.IsEuroNumber(numArray[index3]))
            {
              flag2 = false;
              index3 = length2;
            }
            else if (RtlRenderer.IsLTRText(numArray[index3]) || flag1 && RtlRenderer.IsEuroNumber(numArray[index3]))
            {
              flag2 = true;
              index3 = length2;
            }
          }
          if (!flag1 && !flag2)
            RtlRenderer.WriteInLTR(charArray, symbol, false, ref indexCursor, ref indexLength);
          else
            RtlRenderer.SaveSymbol(charArray, symbol, false, ref indexCursor, ref indexLength);
        }
      }
    }
    return new string(charArray);
  }

  private static bool IsNextEuroNumber(ushort[] characterCodes, int index)
  {
    if (characterCodes == null)
      throw new ArgumentNullException(nameof (characterCodes));
    if (index + 1 > characterCodes.Length)
      return false;
    bool flag = false;
    if (index < characterCodes.Length - 1 && index >= 0)
      flag = characterCodes[index + 1] == (ushort) 3;
    return flag;
  }

  private static bool IsBackEuroNumber(ushort[] characterCodes, int index)
  {
    if (characterCodes == null)
      throw new ArgumentNullException(nameof (characterCodes));
    if (index - 1 < 0)
      return false;
    bool flag = false;
    if (index < characterCodes.Length - 1 && index >= 0)
      flag = characterCodes[index - 1] == (ushort) 3;
    return flag;
  }

  private static void SaveSymbol(
    char[] convertedData,
    char symbol,
    bool rtl,
    ref int indexCursor,
    ref int indexLength)
  {
    if (convertedData == null)
      throw new ArgumentNullException(nameof (convertedData));
    indexCursor = rtl ? indexCursor - indexLength : indexCursor + indexLength;
    indexLength = 0;
    convertedData[indexCursor] = symbol;
    indexCursor = rtl ? indexCursor - 1 : indexCursor + 1;
  }

  private static bool ContainsRTLSymbol(ushort[] characterCodes)
  {
    if (characterCodes == null)
      throw new ArgumentNullException(nameof (characterCodes));
    bool flag = false;
    int index = 0;
    for (int length = characterCodes.Length; index < length; ++index)
    {
      if (characterCodes[index] == (ushort) 2 || characterCodes[index] == (ushort) 6)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private static void WriteInLTR(
    char[] convertedData,
    char symbol,
    bool rtl,
    ref int indexCursor,
    ref int indexLength)
  {
    if (convertedData == null)
      throw new ArgumentNullException(nameof (convertedData));
    if (rtl)
    {
      for (int index = indexLength; index > 0; --index)
        convertedData[indexCursor - index] = convertedData[indexCursor - index + 1];
      convertedData[indexCursor] = symbol;
      ++indexLength;
    }
    else
    {
      for (int index = indexLength; index > 0; --index)
        convertedData[index + indexCursor] = convertedData[index + indexCursor - 1];
      convertedData[indexCursor] = symbol;
      ++indexLength;
    }
  }

  private static void ReverseBrackets(
    char[] convertedData,
    char symbol,
    ref int indexCursor,
    ref int indexLength)
  {
    if (convertedData == null)
      throw new ArgumentNullException(nameof (convertedData));
    indexCursor -= indexLength;
    indexLength = 0;
    convertedData[indexCursor] = symbol == '(' ? ')' : '(';
    --indexCursor;
  }

  private static bool IsLTRText(ushort symbolCode) => symbolCode == (ushort) 1;

  private static bool IsRTLSymbol(ushort symbolCode)
  {
    return symbolCode == (ushort) 2 || symbolCode == (ushort) 6;
  }

  private static bool IsRTLText(ushort symbolCode) => symbolCode == (ushort) 2;

  private static bool IsGeneralEuroNumber(ushort symbolCode)
  {
    return symbolCode == (ushort) 3 || symbolCode == (ushort) 4;
  }

  private static bool IsEuroNumber(ushort symbolCode) => symbolCode == (ushort) 3;

  private static bool IsEuroTerminator(ushort symbolCode) => symbolCode == (ushort) 5;

  private static bool IsWhitespace(ushort symbolCode) => symbolCode == (ushort) 10;

  private static bool IsBracket(char symbol) => symbol == '(' || symbol == ')';

  private static string[] CustomSplitLayout(
    string line,
    PdfTrueTypeFont font,
    bool rtl,
    bool wordSpace,
    PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    return RtlRenderer.CustomLayout(line, rtl, format).Split((char[]) null);
  }
}
