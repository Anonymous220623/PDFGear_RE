// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.CharHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace PDFKit.Contents.Utils;

internal static class CharHelper
{
  private static readonly HashSet<char> bullets = new HashSet<char>()
  {
    '•',
    '◆',
    '●',
    '◼',
    '⚫',
    '✓',
    '➢',
    '\uF0B2'
  };
  private static readonly HashSet<char> lineEndPuncs = new HashSet<char>()
  {
    '.',
    '?',
    ':',
    '!',
    '。',
    '？',
    '！',
    '：',
    '…'
  };
  private static readonly int[] special_chars = new int[128 /*0x80*/]
  {
    0,
    12,
    8,
    12,
    8,
    0,
    32 /*0x20*/,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    8,
    8,
    0,
    16 /*0x10*/,
    0,
    0,
    40,
    12,
    8,
    0,
    0,
    40,
    40,
    40,
    40,
    2,
    2,
    2,
    2,
    2,
    2,
    2,
    2,
    2,
    2,
    8,
    8,
    0,
    0,
    0,
    8,
    0,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    12,
    0,
    8,
    0,
    0,
    0,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    12,
    0,
    8,
    0,
    0
  };

  internal static bool IsBullet(char c) => CharHelper.bullets.Contains(c);

  internal static bool CharIsLineEndPunctuation(char ch) => CharHelper.lineEndPuncs.Contains(ch);

  internal static bool IsPunctuation(int word)
  {
    if (word == 47)
      return false;
    if (word <= (int) sbyte.MaxValue)
    {
      if ((CharHelper.special_chars[word] >> 3 & 1) != 0)
        return true;
    }
    else if (word >= 128 /*0x80*/ && word <= (int) byte.MaxValue)
    {
      if (word == 130 || word == 132 || word == 133 || word == 145 || word == 146 || word == 147 || word <= 148 || word == 150 || word == 180 || word == 184)
        return true;
    }
    else if (word >= 8192 /*0x2000*/ && word <= 8303)
    {
      if (word == 8208 || word == 8209 || word == 8210 || word == 8211 || word == 8216 || word == 8217 || word == 8218 || word == 8219 || word == 8220 || word == 8221 || word == 8222 || word == 8223 || word == 8242 || word == 8243 || word == 8244 || word == 8245 || word == 8246 || word == 8247 || word == 8252 || word == 8253 || word == 8254 || word == 8260)
        return true;
    }
    else if (word >= 12288 /*0x3000*/ && word <= 12351)
    {
      if (word == 12289 || word == 12290 || word == 12291 || word == 12293 || word == 12297 || word == 12298 || word == 12299 || word == 12300 || word == 12301 || word == 12303 || word == 12302 || word == 12304 || word == 12305 || word == 12308 || word == 12309 || word == 12310 || word == 12311 || word == 12312 || word == 12313 || word == 12314 || word == 12315 || word == 12317 || word == 12318 || word == 12319)
        return true;
    }
    else if (word >= 65104 && word <= 65135)
    {
      if (word >= 65104 && word <= 65118 || word == 65123)
        return true;
    }
    else if (word >= 65280 && word <= 65519 && (word == 65281 || word == 65282 || word == 65287 || word == 65288 || word == 65289 || word == 65292 || word == 65294 || word == 65295 || word == 65306 || word == 65307 || word == 65311 || word == 65339 || word == 65341 || word == 65344 || word == 65371 || word == 65372 || word == 65373 || word == 65377 || word == 65378 || word == 65379 || word == 65380 || word == 65381 || word == 65438 || word == 65439))
      return true;
    return false;
  }

  internal static bool IsCJK(char ch, bool excludeSymbols = true)
  {
    if (ch >= 'ᄀ' && ch <= 'ᇿ' || ch >= '⺀' && ch <= '\u2FFF' || ch >= '\u3040' && ch <= '龿' || ch >= '가' && ch <= '\uD7AF' || ch >= '豈' && ch <= '\uFAFF' || ch >= '︰' && ch <= '﹏' || ch >= char.MinValue && ch <= 'ꛟ' || ch >= '\uF800' && ch <= '﨟')
      return true;
    return ch >= '　' && ch <= '〿' ? ch == '々' || ch == '〆' || ch == '〡' || ch == '〢' || ch == '〣' || ch == '〤' || ch == '〥' || ch == '〦' || ch == '〧' || ch == '〨' || ch == '〩' || ch == '〱' || ch == '〲' || ch == '〳' || ch == '〴' || ch == '〵' || !excludeSymbols : ch >= 'ｦ' && ch <= 'ﾝ';
  }

  internal static FontCharSet CharsetFromUnicode(char ch, CultureInfo cultureInfo)
  {
    return CharHelper.CharsetFromUnicode(ch, cultureInfo, FontCharSet.GB2312_CHARSET);
  }

  internal static FontCharSet CharsetFromUnicode(
    char ch,
    CultureInfo cultureInfo,
    FontCharSet defaultCjkCharSet)
  {
    if (ch < '\u007F')
      return FontCharSet.ANSI_CHARSET;
    if (ch >= '⺀' && ch <= '\u2EFF' || ch >= '　' && ch <= '〿' || ch >= '㈀' && ch <= '\u32FF' || ch >= '㌀' && ch <= '㏿' || ch >= '㐀' && ch <= '䶵' || ch >= '一' && ch <= '\u9FFF' || ch >= '豈' && ch <= '\uFAFF' || ch >= '︰' && ch <= '﹏' || ch >= char.MinValue && ch <= 'ꛖ' || ch >= '\uF800' && ch <= '﨟' || ch >= '\uFF00' && ch <= '～')
    {
      foreach (FontCharSet fontCharSet in (IEnumerable<FontCharSet>) WindowsFonts.MapCultureInfoToCharSet(cultureInfo))
      {
        switch (fontCharSet)
        {
          case FontCharSet.SHIFTJIS_CHARSET:
            return FontCharSet.SHIFTJIS_CHARSET;
          case FontCharSet.HANGEUL_CHARSET:
            return FontCharSet.HANGEUL_CHARSET;
          case FontCharSet.GB2312_CHARSET:
            return FontCharSet.GB2312_CHARSET;
          case FontCharSet.CHINESEBIG5_CHARSET:
            return FontCharSet.CHINESEBIG5_CHARSET;
          default:
            continue;
        }
      }
      return defaultCjkCharSet;
    }
    if (ch == '₩')
      return FontCharSet.HANGEUL_CHARSET;
    if (ch >= '一' && ch <= '龥' || ch >= '\uE7C7' && ch <= '\uE7F3' || ch >= '　' && ch <= '〿' || ch >= ' ' && ch <= '\u206F')
      return FontCharSet.GB2312_CHARSET;
    if (ch >= '\u3040' && ch <= 'ゟ' || ch >= '゠' && ch <= 'ヿ' || ch >= 'ㇰ' && ch <= 'ㇿ' || ch >= '｟' && ch <= '\uFFEF')
      return FontCharSet.SHIFTJIS_CHARSET;
    if (ch >= '가' && ch <= '\uD7AF' || ch >= 'ᄀ' && ch <= 'ᇿ' || ch >= '\u3130' && ch <= '\u318F')
      return FontCharSet.HANGEUL_CHARSET;
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
    return ch >= 'Ḁ' && ch <= 'ỿ' ? FontCharSet.VIETNAMESE_CHARSET : FontCharSet.DEFAULT_CHARSET;
  }

  internal static IReadOnlyList<KeyValuePair<FontCharSet, string>> SplitTextByCharSet(
    string text,
    CultureInfo cultureInfo)
  {
    if (string.IsNullOrEmpty(text))
      return (IReadOnlyList<KeyValuePair<FontCharSet, string>>) Array.Empty<KeyValuePair<FontCharSet, string>>();
    List<KeyValuePair<FontCharSet, StringBuilder>> source = new List<KeyValuePair<FontCharSet, StringBuilder>>();
    StringInfo stringInfo = new StringInfo(text);
    for (int startingTextElement = 0; startingTextElement < stringInfo.LengthInTextElements; ++startingTextElement)
    {
      string str = stringInfo.SubstringByTextElements(startingTextElement, 1);
      FontCharSet key = str.Length <= 1 ? CharHelper.CharsetFromUnicode(str[0], cultureInfo) : FontCharSet.DEFAULT_CHARSET;
      if (source.Count == 0)
        source.Add(new KeyValuePair<FontCharSet, StringBuilder>(key, new StringBuilder()));
      KeyValuePair<FontCharSet, StringBuilder> keyValuePair = source.Last<KeyValuePair<FontCharSet, StringBuilder>>();
      if (keyValuePair.Key != key)
      {
        keyValuePair = new KeyValuePair<FontCharSet, StringBuilder>(key, new StringBuilder());
        source.Add(keyValuePair);
      }
      keyValuePair.Value.Append(str);
    }
    return (IReadOnlyList<KeyValuePair<FontCharSet, string>>) source.Select<KeyValuePair<FontCharSet, StringBuilder>, KeyValuePair<FontCharSet, string>>((Func<KeyValuePair<FontCharSet, StringBuilder>, KeyValuePair<FontCharSet, string>>) (c => new KeyValuePair<FontCharSet, string>(c.Key, c.Value.ToString()))).ToArray<KeyValuePair<FontCharSet, string>>();
  }
}
