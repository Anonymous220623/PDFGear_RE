// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LanguageUtil
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LanguageUtil
{
  internal Dictionary<int, UnicodeLanguageInfo> languageTags = new Dictionary<int, UnicodeLanguageInfo>();
  private static int[] dicardCharacters = new int[100]
  {
    0,
    1,
    2,
    3,
    4,
    5,
    6,
    7,
    8,
    14,
    15,
    16 /*0x10*/,
    17,
    18,
    19,
    20,
    21,
    22,
    23,
    24,
    25,
    26,
    27,
    (int) sbyte.MaxValue,
    128 /*0x80*/,
    129,
    130,
    131,
    132,
    133,
    134,
    135,
    136,
    137,
    138,
    139,
    140,
    141,
    142,
    143,
    144 /*0x90*/,
    145,
    146,
    147,
    148,
    149,
    150,
    151,
    152,
    153,
    154,
    155,
    156,
    157,
    158,
    159,
    173,
    1536 /*0x0600*/,
    1537,
    1538,
    1539,
    1757,
    1807,
    6068,
    6069,
    8203,
    8204,
    8205,
    8206,
    8207,
    8234,
    8235,
    8236,
    8237,
    8238,
    8288,
    8289,
    8290,
    8291,
    8292,
    8298,
    8299,
    8300,
    8301,
    8302,
    8303,
    65279,
    65529,
    65530,
    65531,
    69821,
    119155,
    119156,
    119157,
    119158,
    119159,
    119160,
    119161,
    119162,
    917505 /*0x0E0001*/
  };

  internal LanguageUtil() => this.UpdateLanguages();

  internal ScriptTags GetLanguage(char c)
  {
    int[] array = new int[this.languageTags.Keys.Count];
    this.languageTags.Keys.CopyTo(array, 0);
    int index = Array.BinarySearch<int>(array, (int) c);
    if (index < 0)
      index = -index - 2;
    UnicodeLanguageInfo unicodeLanguageInfo;
    this.languageTags.TryGetValue(array[index], out unicodeLanguageInfo);
    return this.GetScriptTag<ScriptTags>(unicodeLanguageInfo.LanguageName);
  }

  private T GetScriptTag<T>(string languageName)
  {
    T scriptTag = default (T);
    if (Enum.IsDefined(typeof (T), (object) languageName))
    {
      scriptTag = (T) Enum.Parse(typeof (T), languageName, true);
    }
    else
    {
      foreach (string name in Enum.GetNames(typeof (T)))
      {
        if (name.Equals(languageName, StringComparison.OrdinalIgnoreCase))
          scriptTag = (T) Enum.Parse(typeof (T), name);
      }
    }
    return scriptTag;
  }

  internal int FindIndex(int[] array, char key)
  {
    int num1 = 0;
    int num2 = array.Length - 1;
    int index;
    do
    {
      index = num1 + (num2 - num1) / 2;
      if ((int) key > array[index])
        num1 = index + 1;
      else
        num2 = index - 1;
      if (array[index] == (int) key)
        return index;
    }
    while (num1 <= num2);
    return index - 1;
  }

  private void UpdateLanguages()
  {
    this.languageTags.Add(0, new UnicodeLanguageInfo("Latin", 0, (int) sbyte.MaxValue));
    this.languageTags.Add(128 /*0x80*/, new UnicodeLanguageInfo("Latin", 128 /*0x80*/, (int) byte.MaxValue));
    this.languageTags.Add(256 /*0x0100*/, new UnicodeLanguageInfo("Latin", 256 /*0x0100*/, 383));
    this.languageTags.Add(384, new UnicodeLanguageInfo("Latin", 384, 591));
    this.languageTags.Add(592, new UnicodeLanguageInfo("IPA Extensions", 592, 687));
    this.languageTags.Add(688, new UnicodeLanguageInfo("Spacing Modifier Letters", 688, 767 /*0x02FF*/));
    this.languageTags.Add(768 /*0x0300*/, new UnicodeLanguageInfo("Combining Diacritical Marks", 768 /*0x0300*/, 879));
    this.languageTags.Add(880, new UnicodeLanguageInfo("Greek and Coptic", 880, 1023 /*0x03FF*/));
    this.languageTags.Add(1024 /*0x0400*/, new UnicodeLanguageInfo("Cyrillic", 1024 /*0x0400*/, 1279 /*0x04FF*/));
    this.languageTags.Add(1280 /*0x0500*/, new UnicodeLanguageInfo("Cyrillic", 1280 /*0x0500*/, 1327));
    this.languageTags.Add(1328, new UnicodeLanguageInfo("Armenian", 1328, 1423));
    this.languageTags.Add(1424, new UnicodeLanguageInfo("Hebrew", 1424, 1535 /*0x05FF*/));
    this.languageTags.Add(1536 /*0x0600*/, new UnicodeLanguageInfo("Arabic", 1536 /*0x0600*/, 1791 /*0x06FF*/));
    this.languageTags.Add(1792 /*0x0700*/, new UnicodeLanguageInfo("Syriac", 1792 /*0x0700*/, 1871));
    this.languageTags.Add(1872, new UnicodeLanguageInfo("Arabic", 1872, 1919));
    this.languageTags.Add(1920, new UnicodeLanguageInfo("Thaana", 1920, 1983));
    this.languageTags.Add(1984, new UnicodeLanguageInfo("NKo", 1984, 2047 /*0x07FF*/));
    this.languageTags.Add(2048 /*0x0800*/, new UnicodeLanguageInfo("Samaritan", 2048 /*0x0800*/, 2111));
    this.languageTags.Add(2112, new UnicodeLanguageInfo("Mandaic", 2112, 2143));
    this.languageTags.Add(2144, new UnicodeLanguageInfo("Syriac", 2144, 2159));
    this.languageTags.Add(2208, new UnicodeLanguageInfo("Arabic", 2208, 2303 /*0x08FF*/));
    this.languageTags.Add(2304 /*0x0900*/, new UnicodeLanguageInfo("Devanagari", 2304 /*0x0900*/, 2431));
    this.languageTags.Add(2432, new UnicodeLanguageInfo("Bengali", 2432, 2559 /*0x09FF*/));
    this.languageTags.Add(2560 /*0x0A00*/, new UnicodeLanguageInfo("Gurmukhi", 2560 /*0x0A00*/, 2687));
    this.languageTags.Add(2688, new UnicodeLanguageInfo("Gujarati", 2688, 2815 /*0x0AFF*/));
    this.languageTags.Add(2816 /*0x0B00*/, new UnicodeLanguageInfo("Oriya", 2816 /*0x0B00*/, 2943));
    this.languageTags.Add(2944, new UnicodeLanguageInfo("Tamil", 2944, 3071 /*0x0BFF*/));
    this.languageTags.Add(3072 /*0x0C00*/, new UnicodeLanguageInfo("Telugu", 3072 /*0x0C00*/, 3199));
    this.languageTags.Add(3200, new UnicodeLanguageInfo("Kannada", 3200, 3327 /*0x0CFF*/));
    this.languageTags.Add(3328 /*0x0D00*/, new UnicodeLanguageInfo("Malayalam", 3328 /*0x0D00*/, 3455));
    this.languageTags.Add(3456, new UnicodeLanguageInfo("Sinhala", 3456, 3583 /*0x0DFF*/));
    this.languageTags.Add(3584 /*0x0E00*/, new UnicodeLanguageInfo("Thai", 3584 /*0x0E00*/, 3711));
    this.languageTags.Add(3712, new UnicodeLanguageInfo("Lao", 3712, 3839 /*0x0EFF*/));
    this.languageTags.Add(3840 /*0x0F00*/, new UnicodeLanguageInfo("Tibetan", 3840 /*0x0F00*/, 4095 /*0x0FFF*/));
    this.languageTags.Add(4096 /*0x1000*/, new UnicodeLanguageInfo("Myanmar", 4096 /*0x1000*/, 4255));
    this.languageTags.Add(4256, new UnicodeLanguageInfo("Georgian", 4256, 4351));
    this.languageTags.Add(4352, new UnicodeLanguageInfo("Hangul", 4352, 4607));
    this.languageTags.Add(4608, new UnicodeLanguageInfo("Ethiopic", 4608, 4991));
    this.languageTags.Add(4992, new UnicodeLanguageInfo("Ethiopic Supplement", 4992, 5023));
    this.languageTags.Add(5024, new UnicodeLanguageInfo("Cherokee", 5024, 5119));
    this.languageTags.Add(5120, new UnicodeLanguageInfo("Canadian", 5120, 5759));
    this.languageTags.Add(5760, new UnicodeLanguageInfo("Ogham", 5760, 5791));
    this.languageTags.Add(5792, new UnicodeLanguageInfo("Runic", 5792, 5887));
    this.languageTags.Add(5888, new UnicodeLanguageInfo("Tagalog", 5888, 5919));
    this.languageTags.Add(5920, new UnicodeLanguageInfo("Hanunoo", 5920, 5951));
    this.languageTags.Add(5952, new UnicodeLanguageInfo("Buhid", 5952, 5983));
    this.languageTags.Add(5984, new UnicodeLanguageInfo("Tagbanwa", 5984, 6015));
    this.languageTags.Add(6016, new UnicodeLanguageInfo("Khmer", 6016, 6143));
    this.languageTags.Add(6144, new UnicodeLanguageInfo("Mongolian", 6144, 6319));
    this.languageTags.Add(6320, new UnicodeLanguageInfo("Canadian", 6320, 6399));
    this.languageTags.Add(6400, new UnicodeLanguageInfo("Limbu", 6400, 6479));
    this.languageTags.Add(6480, new UnicodeLanguageInfo("Tai Le", 6480, 6527));
    this.languageTags.Add(6528, new UnicodeLanguageInfo("New Tai Lue", 6528, 6623));
    this.languageTags.Add(6624, new UnicodeLanguageInfo("Khmer Symbols", 6624, 6655));
    this.languageTags.Add(6656, new UnicodeLanguageInfo("Buginese", 6656, 6687));
    this.languageTags.Add(6688, new UnicodeLanguageInfo("Tai Tham", 6688, 6831));
    this.languageTags.Add(6832, new UnicodeLanguageInfo("Diacritical Marks", 6832, 6911));
    this.languageTags.Add(6912, new UnicodeLanguageInfo("Balinese", 6912, 7039));
    this.languageTags.Add(7040, new UnicodeLanguageInfo("Sundanese", 7040, 7103));
    this.languageTags.Add(7104, new UnicodeLanguageInfo("Batak", 7104, 7167));
    this.languageTags.Add(7168, new UnicodeLanguageInfo("Lepcha", 7168, 7247));
    this.languageTags.Add(7248, new UnicodeLanguageInfo("Ol Chiki", 7248, 7295));
    this.languageTags.Add(7296, new UnicodeLanguageInfo("Cyrillic", 7296, 7311));
    this.languageTags.Add(7312, new UnicodeLanguageInfo("Georgian", 7312, 7359));
    this.languageTags.Add(7360, new UnicodeLanguageInfo("Sundanese", 7360, 7375));
    this.languageTags.Add(7376, new UnicodeLanguageInfo("Vedic Extensions", 7376, 7423));
    this.languageTags.Add(7424, new UnicodeLanguageInfo("Phonetic", 7424, 7551));
    this.languageTags.Add(7552, new UnicodeLanguageInfo("Phonetic", 7552, 7615));
    this.languageTags.Add(7616, new UnicodeLanguageInfo("Diacritical Marks", 7616, 7679));
    this.languageTags.Add(7680, new UnicodeLanguageInfo("Latin", 7680, 7935));
    this.languageTags.Add(7936, new UnicodeLanguageInfo("Greek", 7936, 8191 /*0x1FFF*/));
    this.languageTags.Add(8192 /*0x2000*/, new UnicodeLanguageInfo("General Punctuation", 8192 /*0x2000*/, 8303));
    this.languageTags.Add(8304, new UnicodeLanguageInfo("Superscripts and Subscripts", 8304, 8351));
    this.languageTags.Add(8352, new UnicodeLanguageInfo("Currency Symbols", 8352, 8399));
    this.languageTags.Add(8400, new UnicodeLanguageInfo("Diacritical Marks", 8400, 8447));
    this.languageTags.Add(8448, new UnicodeLanguageInfo("Letterlike Symbols", 8448, 8527));
    this.languageTags.Add(8528, new UnicodeLanguageInfo("Number Forms", 8528, 8591));
    this.languageTags.Add(8592, new UnicodeLanguageInfo("Arrows", 8592, 8703));
    this.languageTags.Add(8704, new UnicodeLanguageInfo("Operators", 8704, 8959));
    this.languageTags.Add(8960, new UnicodeLanguageInfo("Miscellaneous", 8960, 9215));
    this.languageTags.Add(9216, new UnicodeLanguageInfo("Control Pictures", 9216, 9279));
    this.languageTags.Add(9280, new UnicodeLanguageInfo("Optical Character Recognition", 9280, 9311));
    this.languageTags.Add(9312, new UnicodeLanguageInfo("Enclosed Alphanumerics", 9312, 9471));
    this.languageTags.Add(9472, new UnicodeLanguageInfo("Box Drawing", 9472, 9599));
    this.languageTags.Add(9600, new UnicodeLanguageInfo("Block Elements", 9600, 9631));
    this.languageTags.Add(9632, new UnicodeLanguageInfo("Geometric Shapes", 9632, 9727));
    this.languageTags.Add(9728, new UnicodeLanguageInfo("Miscellaneous", 9728, 9983));
    this.languageTags.Add(9984, new UnicodeLanguageInfo("Dingbats", 9984, 10175));
    this.languageTags.Add(10176, new UnicodeLanguageInfo("Miscellaneous", 10176, 10223));
    this.languageTags.Add(10224, new UnicodeLanguageInfo("Supplemental", 10224, 10239));
    this.languageTags.Add(10240, new UnicodeLanguageInfo("Braille Patterns", 10240, 10495));
    this.languageTags.Add(10496, new UnicodeLanguageInfo("Supplemental", 10496, 10623));
    this.languageTags.Add(10624, new UnicodeLanguageInfo("Miscellaneous", 10624, 10751));
    this.languageTags.Add(10752, new UnicodeLanguageInfo("Supplemental", 10752, 11007));
    this.languageTags.Add(11008, new UnicodeLanguageInfo("Miscellaneous", 11008, 11263));
    this.languageTags.Add(11264, new UnicodeLanguageInfo("Glagolitic", 11264, 11359));
    this.languageTags.Add(11360, new UnicodeLanguageInfo("Latin", 11360, 11391));
    this.languageTags.Add(11392, new UnicodeLanguageInfo("Coptic", 11392, 11519));
    this.languageTags.Add(11520, new UnicodeLanguageInfo("Georgian Supplement", 11520, 11567));
    this.languageTags.Add(11568, new UnicodeLanguageInfo("Tifinagh", 11568, 11647));
    this.languageTags.Add(11648, new UnicodeLanguageInfo("Ethiopic Extended", 11648, 11743));
    this.languageTags.Add(11744, new UnicodeLanguageInfo("Cyrillic", 11744, 11775));
    this.languageTags.Add(11776, new UnicodeLanguageInfo("Supplemental", 11776, 11903));
    this.languageTags.Add(11904, new UnicodeLanguageInfo("CJK", 11904, 12031));
    this.languageTags.Add(12032, new UnicodeLanguageInfo("Kangxi Radicals", 12032, 12255));
    this.languageTags.Add(12272, new UnicodeLanguageInfo("Ideographic", 12272, 12287 /*0x2FFF*/));
    this.languageTags.Add(12288 /*0x3000*/, new UnicodeLanguageInfo("CJK", 12288 /*0x3000*/, 12351));
    this.languageTags.Add(12352, new UnicodeLanguageInfo("Hiragana", 12352, 12447));
    this.languageTags.Add(12448, new UnicodeLanguageInfo("Katakana", 12448, 12543));
    this.languageTags.Add(12544, new UnicodeLanguageInfo("Bopomofo", 12544, 12591));
    this.languageTags.Add(12592, new UnicodeLanguageInfo("Hangul", 12592, 12687));
    this.languageTags.Add(12688, new UnicodeLanguageInfo("Kanbun", 12688, 12703));
    this.languageTags.Add(12704, new UnicodeLanguageInfo("Bopomofo Extended", 12704, 12735));
    this.languageTags.Add(12736, new UnicodeLanguageInfo("CJK", 12736, 12783));
    this.languageTags.Add(12784, new UnicodeLanguageInfo("Katakana", 12784, 12799));
    this.languageTags.Add(12800, new UnicodeLanguageInfo("CJK", 12800, 13055));
    this.languageTags.Add(13056, new UnicodeLanguageInfo("CJK", 13056, 13311));
    this.languageTags.Add(13312, new UnicodeLanguageInfo("CJK", 13312, 19903));
    this.languageTags.Add(19904, new UnicodeLanguageInfo("Yijing", 19904, 19967));
    this.languageTags.Add(19968, new UnicodeLanguageInfo("CJK", 19968, 40959 /*0x9FFF*/));
    this.languageTags.Add(40960 /*0xA000*/, new UnicodeLanguageInfo("Yi", 40960 /*0xA000*/, 42127));
    this.languageTags.Add(42128, new UnicodeLanguageInfo("Yi", 42128, 42191));
    this.languageTags.Add(42192, new UnicodeLanguageInfo("Lisu", 42192, 42239));
    this.languageTags.Add(42240, new UnicodeLanguageInfo("Vai", 42240, 42559));
    this.languageTags.Add(42560, new UnicodeLanguageInfo("Cyrillic", 42560, 42655));
    this.languageTags.Add(42656, new UnicodeLanguageInfo("Bamum", 42656, 42751));
    this.languageTags.Add(42752, new UnicodeLanguageInfo("Modifier Tone Letters", 42752, 42783));
    this.languageTags.Add(42784, new UnicodeLanguageInfo("Latin", 42784, 43007));
    this.languageTags.Add(43008, new UnicodeLanguageInfo("Syloti Nagri", 43008, 43055));
    this.languageTags.Add(43056, new UnicodeLanguageInfo("Indic Number", 43056, 43071));
    this.languageTags.Add(43072, new UnicodeLanguageInfo("Phags", 43072, 43135));
    this.languageTags.Add(43136, new UnicodeLanguageInfo("Saurashtra", 43136, 43231));
    this.languageTags.Add(43232, new UnicodeLanguageInfo("Devanagari", 43232, 43263));
    this.languageTags.Add(43264, new UnicodeLanguageInfo("Kayah Li", 43264, 43311));
    this.languageTags.Add(43312, new UnicodeLanguageInfo("Rejang", 43312, 43359));
    this.languageTags.Add(43360, new UnicodeLanguageInfo("Hangul", 43360, 43391));
    this.languageTags.Add(43392, new UnicodeLanguageInfo("Javanese", 43392, 43487));
    this.languageTags.Add(43488, new UnicodeLanguageInfo("Myanmar", 43488, 43519));
    this.languageTags.Add(43520, new UnicodeLanguageInfo("Cham", 43520, 43615));
    this.languageTags.Add(43616, new UnicodeLanguageInfo("Myanmar", 43616, 43647));
    this.languageTags.Add(43648, new UnicodeLanguageInfo("Tai Viet", 43648, 43743));
    this.languageTags.Add(43744, new UnicodeLanguageInfo("Meetei", 43744, 43775));
    this.languageTags.Add(43776, new UnicodeLanguageInfo("Ethiopic", 43776, 43823));
    this.languageTags.Add(43824, new UnicodeLanguageInfo("Latin", 43824, 43887));
    this.languageTags.Add(43888, new UnicodeLanguageInfo("Cherokee", 43888, 43967));
    this.languageTags.Add(43968, new UnicodeLanguageInfo("Meetei", 43968, 44031));
    this.languageTags.Add(44032, new UnicodeLanguageInfo("Hangul", 44032, 55215));
    this.languageTags.Add(55216, new UnicodeLanguageInfo("Hangul", 55216, 55295));
    this.languageTags.Add(55296, new UnicodeLanguageInfo("Surrogates", 55296, 56191));
    this.languageTags.Add(56192, new UnicodeLanguageInfo("Surrogates", 56192, 56319));
    this.languageTags.Add(56320, new UnicodeLanguageInfo("Surrogates", 56320, 57343 /*0xDFFF*/));
    this.languageTags.Add(57344 /*0xE000*/, new UnicodeLanguageInfo("Private Use Area", 57344 /*0xE000*/, 63743));
    this.languageTags.Add(63744, new UnicodeLanguageInfo("CJK", 63744, 64255));
    this.languageTags.Add(64256, new UnicodeLanguageInfo("Alphabetic", 64256, 64335));
    this.languageTags.Add(64336, new UnicodeLanguageInfo("Arabic", 64336, 65023));
    this.languageTags.Add(65024, new UnicodeLanguageInfo("Variation Selectors", 65024, 65039));
    this.languageTags.Add(65040, new UnicodeLanguageInfo("Vertical Forms", 65040, 65055));
    this.languageTags.Add(65056, new UnicodeLanguageInfo("Combining Half Marks", 65056, 65071));
    this.languageTags.Add(65072, new UnicodeLanguageInfo("CJK", 65072, 65103));
    this.languageTags.Add(65104, new UnicodeLanguageInfo("Small Form Variants", 65104, 65135));
    this.languageTags.Add(65136, new UnicodeLanguageInfo("Arabic", 65136, 65279));
    this.languageTags.Add(65280, new UnicodeLanguageInfo("Halfwidth and Fullwidth Forms", 65280, 65519));
    this.languageTags.Add(65520, new UnicodeLanguageInfo("Specials", 65520, (int) ushort.MaxValue));
    this.languageTags.Add(65536 /*0x010000*/, new UnicodeLanguageInfo("Linear", 65536 /*0x010000*/, 65663));
    this.languageTags.Add(65664 /*0x010080*/, new UnicodeLanguageInfo("Linear", 65664 /*0x010080*/, 65791));
    this.languageTags.Add(65792 /*0x010100*/, new UnicodeLanguageInfo("Aegean", 65792 /*0x010100*/, 65855));
    this.languageTags.Add(65856, new UnicodeLanguageInfo("Ancient", 65856, 65935));
    this.languageTags.Add(65936, new UnicodeLanguageInfo("Ancient", 65936, 65999));
    this.languageTags.Add(66000, new UnicodeLanguageInfo("Phaistos", 66000, 66047));
    this.languageTags.Add(66176, new UnicodeLanguageInfo("Lycian", 66176, 66207));
    this.languageTags.Add(66208, new UnicodeLanguageInfo("Carian", 66208, 66271));
    this.languageTags.Add(66272, new UnicodeLanguageInfo("Coptic", 66272, 66303));
    this.languageTags.Add(66304 /*0x010300*/, new UnicodeLanguageInfo("Italic", 66304 /*0x010300*/, 66351));
    this.languageTags.Add(66352, new UnicodeLanguageInfo("Gothic", 66352, 66383));
    this.languageTags.Add(66384, new UnicodeLanguageInfo("Permic", 66384, 66431));
    this.languageTags.Add(66432, new UnicodeLanguageInfo("Ugaritic", 66432, 66463));
    this.languageTags.Add(66464, new UnicodeLanguageInfo("Persian", 66464, 66527));
    this.languageTags.Add(66560 /*0x010400*/, new UnicodeLanguageInfo("Deseret", 66560 /*0x010400*/, 66639));
    this.languageTags.Add(66640, new UnicodeLanguageInfo("Shavian", 66640, 66687));
    this.languageTags.Add(66688, new UnicodeLanguageInfo("Osmanya", 66688, 66735));
    this.languageTags.Add(66736, new UnicodeLanguageInfo("Osage", 66736, 66815));
    this.languageTags.Add(66816 /*0x010500*/, new UnicodeLanguageInfo("Elbasan", 66816 /*0x010500*/, 66863));
    this.languageTags.Add(66864, new UnicodeLanguageInfo("Caucasian Albanian", 66864, 66927));
    this.languageTags.Add(67072 /*0x010600*/, new UnicodeLanguageInfo("Linear A", 67072 /*0x010600*/, 67455));
    this.languageTags.Add(67584 /*0x010800*/, new UnicodeLanguageInfo("Cypriot Syllabary", 67584 /*0x010800*/, 67647));
    this.languageTags.Add(67648, new UnicodeLanguageInfo("Imperial Aramaic", 67648, 67679));
    this.languageTags.Add(67680, new UnicodeLanguageInfo("Palmyrene", 67680, 67711));
    this.languageTags.Add(67712, new UnicodeLanguageInfo("Nabataean", 67712, 67759));
    this.languageTags.Add(67808, new UnicodeLanguageInfo("Hatran", 67808, 67839));
    this.languageTags.Add(67840 /*0x010900*/, new UnicodeLanguageInfo("Phoenician", 67840 /*0x010900*/, 67871));
    this.languageTags.Add(67872, new UnicodeLanguageInfo("Lydian", 67872, 67903));
    this.languageTags.Add(67968, new UnicodeLanguageInfo("Meroitic Hieroglyphs", 67968, 67999));
    this.languageTags.Add(68000, new UnicodeLanguageInfo("Meroitic Cursive", 68000, 68095));
    this.languageTags.Add(68096 /*0x010A00*/, new UnicodeLanguageInfo("Kharoshthi", 68096 /*0x010A00*/, 68191));
    this.languageTags.Add(68192, new UnicodeLanguageInfo("Old South Arabian", 68192, 68223));
    this.languageTags.Add(68224, new UnicodeLanguageInfo("Old North Arabian", 68224, 68255));
    this.languageTags.Add(68288, new UnicodeLanguageInfo("Manichaean", 68288, 68351));
    this.languageTags.Add(68352 /*0x010B00*/, new UnicodeLanguageInfo("Avestan", 68352 /*0x010B00*/, 68415));
    this.languageTags.Add(68416, new UnicodeLanguageInfo("Inscriptional Parthian", 68416, 68447));
    this.languageTags.Add(68448, new UnicodeLanguageInfo("Inscriptional Pahlavi", 68448, 68479));
    this.languageTags.Add(68480, new UnicodeLanguageInfo("Psalter Pahlavi", 68480, 68527));
    this.languageTags.Add(68608 /*0x010C00*/, new UnicodeLanguageInfo("Old Turkic", 68608 /*0x010C00*/, 68687));
    this.languageTags.Add(68736, new UnicodeLanguageInfo("Old Hungarian", 68736, 68863));
    this.languageTags.Add(68864 /*0x010D00*/, new UnicodeLanguageInfo("Hanifi Rohingya", 68864 /*0x010D00*/, 68927));
    this.languageTags.Add(69216, new UnicodeLanguageInfo("Rumi Numeral Symbols", 69216, 69247));
    this.languageTags.Add(69376 /*0x010F00*/, new UnicodeLanguageInfo("Old Sogdian", 69376 /*0x010F00*/, 69423));
    this.languageTags.Add(69424, new UnicodeLanguageInfo("Sogdian", 69424, 69487));
    this.languageTags.Add(69632 /*0x011000*/, new UnicodeLanguageInfo("Brahmi", 69632 /*0x011000*/, 69759));
    this.languageTags.Add(69760, new UnicodeLanguageInfo("Kaithi", 69760, 69839));
    this.languageTags.Add(69840, new UnicodeLanguageInfo("Sora Sompeng", 69840, 69887));
    this.languageTags.Add(69888, new UnicodeLanguageInfo("Chakma", 69888, 69967));
    this.languageTags.Add(69968, new UnicodeLanguageInfo("Mahajani", 69968, 70015));
    this.languageTags.Add(70016, new UnicodeLanguageInfo("Sharada", 70016, 70111));
    this.languageTags.Add(70112, new UnicodeLanguageInfo("Sinhala", 70112, 70143));
    this.languageTags.Add(70144, new UnicodeLanguageInfo("Khojki", 70144, 70223));
    this.languageTags.Add(70272, new UnicodeLanguageInfo("Multani", 70272, 70319));
    this.languageTags.Add(70320, new UnicodeLanguageInfo("Khudawadi", 70320, 70399));
    this.languageTags.Add(70400, new UnicodeLanguageInfo("Grantha", 70400, 70527));
    this.languageTags.Add(70656, new UnicodeLanguageInfo("Newa", 70656, 70783));
    this.languageTags.Add(70784, new UnicodeLanguageInfo("Tirhuta", 70784, 70879));
    this.languageTags.Add(71040, new UnicodeLanguageInfo("Siddham", 71040, 71167));
    this.languageTags.Add(71168, new UnicodeLanguageInfo("Modi", 71168, 71263));
    this.languageTags.Add(71264, new UnicodeLanguageInfo("Mongolian", 71264, 71295));
    this.languageTags.Add(71296, new UnicodeLanguageInfo("Takri", 71296, 71375));
    this.languageTags.Add(71424, new UnicodeLanguageInfo("Ahom", 71424, 71487));
    this.languageTags.Add(71680, new UnicodeLanguageInfo("Dogra", 71680, 71759));
    this.languageTags.Add(71840, new UnicodeLanguageInfo("Warang Citi", 71840, 71935));
    this.languageTags.Add(72192, new UnicodeLanguageInfo("Zanabazar Square", 72192, 72271));
    this.languageTags.Add(72272, new UnicodeLanguageInfo("Soyombo", 72272, 72367));
    this.languageTags.Add(72384, new UnicodeLanguageInfo("Pau Cin Hau", 72384, 72447));
    this.languageTags.Add(72704, new UnicodeLanguageInfo("Bhaiksuki", 72704, 72815));
    this.languageTags.Add(72816, new UnicodeLanguageInfo("Marchen", 72816, 72895));
    this.languageTags.Add(72960, new UnicodeLanguageInfo("Masaram Gondi", 72960, 73055));
    this.languageTags.Add(73056, new UnicodeLanguageInfo("Gunjala Gondi", 73056, 73135));
    this.languageTags.Add(73440, new UnicodeLanguageInfo("Makasar", 73440, 73471));
    this.languageTags.Add(73728 /*0x012000*/, new UnicodeLanguageInfo("Cuneiform", 73728 /*0x012000*/, 74751));
    this.languageTags.Add(74752, new UnicodeLanguageInfo("Cuneiform Numbers and Punctuation", 74752, 74879));
    this.languageTags.Add(74880, new UnicodeLanguageInfo("Early Dynastic Cuneiform", 74880, 75087));
    this.languageTags.Add(77824 /*0x013000*/, new UnicodeLanguageInfo("Egyptian Hieroglyphs", 77824 /*0x013000*/, 78895));
    this.languageTags.Add(82944, new UnicodeLanguageInfo("Anatolian Hieroglyphs", 82944, 83583));
    this.languageTags.Add(92160, new UnicodeLanguageInfo("Bamum Supplement", 92160, 92735));
    this.languageTags.Add(92736, new UnicodeLanguageInfo("Mro", 92736, 92783));
    this.languageTags.Add(92880, new UnicodeLanguageInfo("Bassa Vah", 92880, 92927));
    this.languageTags.Add(92928, new UnicodeLanguageInfo("Pahawh Hmong", 92928, 93071));
    this.languageTags.Add(93760, new UnicodeLanguageInfo("Medefaidrin", 93760, 93855));
    this.languageTags.Add(93952, new UnicodeLanguageInfo("Miao", 93952, 94111));
    this.languageTags.Add(94176, new UnicodeLanguageInfo("Ideographic", 94176, 94207));
    this.languageTags.Add(94208 /*0x017000*/, new UnicodeLanguageInfo("Tangut", 94208 /*0x017000*/, 100351));
    this.languageTags.Add(100352, new UnicodeLanguageInfo("Tangut", 100352, 101119));
    this.languageTags.Add(110592 /*0x01B000*/, new UnicodeLanguageInfo("Kana", 110592 /*0x01B000*/, 110847));
    this.languageTags.Add(110848, new UnicodeLanguageInfo("Kana", 110848, 110895));
    this.languageTags.Add(110960, new UnicodeLanguageInfo("Nushu", 110960, 111359));
    this.languageTags.Add(113664, new UnicodeLanguageInfo("Duployan", 113664, 113823));
    this.languageTags.Add(113824, new UnicodeLanguageInfo("Shorthand", 113824, 113839));
    this.languageTags.Add(118784 /*0x01D000*/, new UnicodeLanguageInfo("Byzantine Musical Symbols", 118784 /*0x01D000*/, 119039));
    this.languageTags.Add(119040, new UnicodeLanguageInfo("Musical Symbols", 119040, 119295));
    this.languageTags.Add(119296, new UnicodeLanguageInfo("Greek", 119296, 119375));
    this.languageTags.Add(119520, new UnicodeLanguageInfo("Mayan Numerals", 119520, 119551));
    this.languageTags.Add(119552, new UnicodeLanguageInfo("Tai Xuan Jing Symbols", 119552, 119647));
    this.languageTags.Add(119648, new UnicodeLanguageInfo("Counting Rod Numerals", 119648, 119679));
    this.languageTags.Add(119808, new UnicodeLanguageInfo("Alphanumeric", 119808, 120831));
    this.languageTags.Add(120832, new UnicodeLanguageInfo("Sutton SignWriting", 120832, 121519));
    this.languageTags.Add(122880 /*0x01E000*/, new UnicodeLanguageInfo("Glagolitic Supplement", 122880 /*0x01E000*/, 122927));
    this.languageTags.Add(124928, new UnicodeLanguageInfo("Mende Kikakui", 124928, 125151));
    this.languageTags.Add(125184, new UnicodeLanguageInfo("Adlam", 125184, 125279));
    this.languageTags.Add(126064, new UnicodeLanguageInfo("Indic Siyaq Numbers", 126064, 126143));
    this.languageTags.Add(126464, new UnicodeLanguageInfo("Arabic", 126464, 126719));
    this.languageTags.Add(126976 /*0x01F000*/, new UnicodeLanguageInfo("Mahjong Tiles", 126976 /*0x01F000*/, 127023));
    this.languageTags.Add(127024, new UnicodeLanguageInfo("Domino Tiles", 127024, 127135));
    this.languageTags.Add(127136, new UnicodeLanguageInfo("Playing Cards", 127136, 127231));
    this.languageTags.Add(127232, new UnicodeLanguageInfo("Alphanumeric", 127232, 127487));
    this.languageTags.Add(127488, new UnicodeLanguageInfo("Ideographic", 127488, 127743));
    this.languageTags.Add(127744, new UnicodeLanguageInfo("Miscellaneous Symbols and Pictographs", 127744, 128511));
    this.languageTags.Add(128512, new UnicodeLanguageInfo("Emoticons", 128512, 128591));
    this.languageTags.Add(128592, new UnicodeLanguageInfo("Ornamental Dingbats", 128592, 128639));
    this.languageTags.Add(128640, new UnicodeLanguageInfo("Transport and Map Symbols", 128640, 128767));
    this.languageTags.Add(128768, new UnicodeLanguageInfo("Alchemical Symbols", 128768, 128895));
    this.languageTags.Add(128896, new UnicodeLanguageInfo("Geometric Shapes Extended", 128896, 129023));
    this.languageTags.Add(129024, new UnicodeLanguageInfo("Supplemental", 129024, 129279));
    this.languageTags.Add(129280, new UnicodeLanguageInfo("Supplemental", 129280, 129535));
    this.languageTags.Add(129536, new UnicodeLanguageInfo("Chess Symbols", 129536, 129647));
    this.languageTags.Add(131072 /*0x020000*/, new UnicodeLanguageInfo("CJK", 131072 /*0x020000*/, 173791));
    this.languageTags.Add(173824, new UnicodeLanguageInfo("CJK", 173824, 177983));
    this.languageTags.Add(177984, new UnicodeLanguageInfo("CJK", 177984, 178207));
    this.languageTags.Add(178208, new UnicodeLanguageInfo("CJK", 178208, 183983));
    this.languageTags.Add(183984, new UnicodeLanguageInfo("CJK", 183984, 191471));
    this.languageTags.Add(194560, new UnicodeLanguageInfo("CJK", 194560, 195103));
    this.languageTags.Add(917504 /*0x0E0000*/, new UnicodeLanguageInfo("Tags", 917504 /*0x0E0000*/, 917631));
    this.languageTags.Add(917760 /*0x0E0100*/, new UnicodeLanguageInfo("Variation Selectors Supplement", 917760 /*0x0E0100*/, 917999));
    this.languageTags.Add(983040 /*0x0F0000*/, new UnicodeLanguageInfo("Supplementary", 983040 /*0x0F0000*/, 1048575 /*0x0FFFFF*/));
    this.languageTags.Add(1048576 /*0x100000*/, new UnicodeLanguageInfo("Supplementary", 1048576 /*0x100000*/, 1114111));
  }

  internal static bool IsDiscardGlyph(int charCode)
  {
    bool flag = false;
    if (charCode > 917535)
    {
      if (charCode < 917632 /*0x0E0080*/)
        flag = true;
    }
    else if (Array.BinarySearch<int>(LanguageUtil.dicardCharacters, charCode) > -1)
      flag = true;
    return flag || charCode == 173;
  }
}
