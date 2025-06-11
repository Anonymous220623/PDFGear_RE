// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.FontFile3
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class FontFile3
{
  private int glyphCount;
  private char[] nybChars = new char[15]
  {
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    '.',
    'e',
    'e',
    ' ',
    '-'
  };
  private int ROS = -1;
  private int CIDFontVersion;
  private int CIDFontRevision;
  private int CIDFontType;
  private int CIDcount;
  private int UIDBase = -1;
  private int FDArray = -1;
  private int FDSelect = -1;
  private int top;
  private string copyright;
  private string embeddedFontName;
  private int charset;
  public double[] FontMatrix = new double[6]
  {
    0.001,
    0.0,
    0.0,
    0.001,
    0.0,
    0.0
  };
  private int encodeValue;
  internal bool hasFontMatrix;
  private int italicAngle;
  private int charstrings;
  internal bool isCID;
  private int stringIndex;
  private bool trackIndices;
  private int stringStart;
  private Dictionary<string, byte[]> glyphs = new Dictionary<string, byte[]>();
  private int stringOffsetSize;
  public int GlobalBias;
  internal double[] m_fontMatrix = new double[6];
  internal CffGlyphs m_cffGlyphs = new CffGlyphs();
  private int privateDict = -1;
  private int privateDictOffset = -1;
  private int defaultWidthX;
  private int nominalWidthX;
  public float[] FontBBox = new float[4]
  {
    0.0f,
    0.0f,
    1000f,
    1000f
  };
  public string[] m_diffTable;
  private int maxCharCount = 256 /*0x0100*/;
  private static int[] ExpertSubCharset = new int[87]
  {
    0,
    1,
    231,
    232,
    235,
    236,
    237,
    238,
    13,
    14,
    15,
    99,
    239,
    240 /*0xF0*/,
    241,
    242,
    243,
    244,
    245,
    246,
    247,
    248,
    27,
    28,
    249,
    250,
    251,
    253,
    254,
    (int) byte.MaxValue,
    256 /*0x0100*/,
    257,
    258,
    259,
    260,
    261,
    262,
    263,
    264,
    265,
    266,
    109,
    110,
    267,
    268,
    269,
    270,
    272,
    300,
    301,
    302,
    305,
    314,
    315,
    158,
    155,
    163,
    320,
    321,
    322,
    323,
    324,
    325,
    326,
    150,
    164,
    169,
    327,
    328,
    329,
    330,
    331,
    332,
    333,
    334,
    335,
    336,
    337,
    338,
    339,
    340,
    341,
    342,
    343,
    344,
    345,
    346
  };
  private static string[] type1CStdStrings = new string[391]
  {
    ".notdef",
    "space",
    "exclam",
    "quotedbl",
    "numbersign",
    "dollar",
    "percent",
    "ampersand",
    "quoteright",
    "parenleft",
    "parenright",
    "asterisk",
    "plus",
    "comma",
    "hyphen",
    "period",
    "slash",
    "zero",
    "one",
    "two",
    "three",
    "four",
    "five",
    "six",
    "seven",
    "eight",
    "nine",
    "colon",
    "semicolon",
    "less",
    "equal",
    "greater",
    "question",
    "at",
    "A",
    "B",
    "C",
    "D",
    "E",
    "F",
    "G",
    "H",
    "I",
    "J",
    "K",
    "L",
    "M",
    "N",
    "O",
    "P",
    "Q",
    "R",
    "S",
    "T",
    "U",
    "V",
    "W",
    "X",
    "Y",
    "Z",
    "bracketleft",
    "backslash",
    "bracketright",
    "asciicircum",
    "underscore",
    "quoteleft",
    "a",
    "b",
    "c",
    "d",
    "e",
    "f",
    "g",
    "h",
    "i",
    "j",
    "k",
    "l",
    "m",
    "n",
    "o",
    "p",
    "q",
    "r",
    "s",
    "t",
    "u",
    "v",
    "w",
    "x",
    "y",
    "z",
    "braceleft",
    "bar",
    "braceright",
    "asciitilde",
    "exclamdown",
    "cent",
    "sterling",
    "fraction",
    "yen",
    "florin",
    "section",
    "currency",
    "quotesingle",
    "quotedblleft",
    "guillemotleft",
    "guilsinglleft",
    "guilsinglright",
    "fi",
    "fl",
    "endash",
    "dagger",
    "daggerdbl",
    "periodcentered",
    "paragraph",
    "bullet",
    "quotesinglbase",
    "quotedblbase",
    "quotedblright",
    "guillemotright",
    "ellipsis",
    "perthousand",
    "questiondown",
    "grave",
    "acute",
    "circumflex",
    "tilde",
    "macron",
    "breve",
    "dotaccent",
    "dieresis",
    "ring",
    "cedilla",
    "hungarumlaut",
    "ogonek",
    "caron",
    "emdash",
    "AE",
    "ordfeminine",
    "Lslash",
    "Oslash",
    "OE",
    "ordmasculine",
    "ae",
    "dotlessi",
    "lslash",
    "oslash",
    "oe",
    "germandbls",
    "onesuperior",
    "logicalnot",
    "mu",
    "trademark",
    "Eth",
    "onehalf",
    "plusminus",
    "Thorn",
    "onequarter",
    "divide",
    "brokenbar",
    "degree",
    "thorn",
    "threequarters",
    "twosuperior",
    "registered",
    "minus",
    "eth",
    "multiply",
    "threesuperior",
    nameof (copyright),
    "Aacute",
    "Acircumflex",
    "Adieresis",
    "Agrave",
    "Aring",
    "Atilde",
    "Ccedilla",
    "Eacute",
    "Ecircumflex",
    "Edieresis",
    "Egrave",
    "Iacute",
    "Icircumflex",
    "Idieresis",
    "Igrave",
    "Ntilde",
    "Oacute",
    "Ocircumflex",
    "Odieresis",
    "Ograve",
    "Otilde",
    "Scaron",
    "Uacute",
    "Ucircumflex",
    "Udieresis",
    "Ugrave",
    "Yacute",
    "Ydieresis",
    "Zcaron",
    "aacute",
    "acircumflex",
    "adieresis",
    "agrave",
    "aring",
    "atilde",
    "ccedilla",
    "eacute",
    "ecircumflex",
    "edieresis",
    "egrave",
    "iacute",
    "icircumflex",
    "idieresis",
    "igrave",
    "ntilde",
    "oacute",
    "ocircumflex",
    "odieresis",
    "ograve",
    "otilde",
    "scaron",
    "uacute",
    "ucircumflex",
    "udieresis",
    "ugrave",
    "yacute",
    "ydieresis",
    "zcaron",
    "exclamsmall",
    "Hungarumlautsmall",
    "dollaroldstyle",
    "dollarsuperior",
    "ampersandsmall",
    "Acutesmall",
    "parenleftsuperior",
    "parenrightsuperior",
    "twodotenleader",
    "onedotenleader",
    "zerooldstyle",
    "oneoldstyle",
    "twooldstyle",
    "threeoldstyle",
    "fouroldstyle",
    "fiveoldstyle",
    "sixoldstyle",
    "sevenoldstyle",
    "eightoldstyle",
    "nineoldstyle",
    "commasuperior",
    "threequartersemdash",
    "periodsuperior",
    "questionsmall",
    "asuperior",
    "bsuperior",
    "centsuperior",
    "dsuperior",
    "esuperior",
    "isuperior",
    "lsuperior",
    "msuperior",
    "nsuperior",
    "osuperior",
    "rsuperior",
    "ssuperior",
    "tsuperior",
    "ff",
    "ffi",
    "ffl",
    "parenleftinferior",
    "parenrightinferior",
    "Circumflexsmall",
    "hyphensuperior",
    "Gravesmall",
    "Asmall",
    "Bsmall",
    "Csmall",
    "Dsmall",
    "Esmall",
    "Fsmall",
    "Gsmall",
    "Hsmall",
    "Ismall",
    "Jsmall",
    "Ksmall",
    "Lsmall",
    "Msmall",
    "Nsmall",
    "Osmall",
    "Psmall",
    "Qsmall",
    "Rsmall",
    "Ssmall",
    "Tsmall",
    "Usmall",
    "Vsmall",
    "Wsmall",
    "Xsmall",
    "Ysmall",
    "Zsmall",
    "colonmonetary",
    "onefitted",
    "rupiah",
    "Tildesmall",
    "exclamdownsmall",
    "centoldstyle",
    "Lslashsmall",
    "Scaronsmall",
    "Zcaronsmall",
    "Dieresissmall",
    "Brevesmall",
    "Caronsmall",
    "Dotaccentsmall",
    "Macronsmall",
    "figuredash",
    "hypheninferior",
    "Ogoneksmall",
    "Ringsmall",
    "Cedillasmall",
    "questiondownsmall",
    "oneeighth",
    "threeeighths",
    "fiveeighths",
    "seveneighths",
    "onethird",
    "twothirds",
    "zerosuperior",
    "foursuperior",
    "fivesuperior",
    "sixsuperior",
    "sevensuperior",
    "eightsuperior",
    "ninesuperior",
    "zeroinferior",
    "oneinferior",
    "twoinferior",
    "threeinferior",
    "fourinferior",
    "fiveinferior",
    "sixinferior",
    "seveninferior",
    "eightinferior",
    "nineinferior",
    "centinferior",
    "dollarinferior",
    "periodinferior",
    "commainferior",
    "Agravesmall",
    "Aacutesmall",
    "Acircumflexsmall",
    "Atildesmall",
    "Adieresissmall",
    "Aringsmall",
    "AEsmall",
    "Ccedillasmall",
    "Egravesmall",
    "Eacutesmall",
    "Ecircumflexsmall",
    "Edieresissmall",
    "Igravesmall",
    "Iacutesmall",
    "Icircumflexsmall",
    "Idieresissmall",
    "Ethsmall",
    "Ntildesmall",
    "Ogravesmall",
    "Oacutesmall",
    "Ocircumflexsmall",
    "Otildesmall",
    "Odieresissmall",
    "OEsmall",
    "Oslashsmall",
    "Ugravesmall",
    "Uacutesmall",
    "Ucircumflexsmall",
    "Udieresissmall",
    "Yacutesmall",
    "Thornsmall",
    "Ydieresissmall",
    "001.000",
    "001.001",
    "001.002",
    "001.003",
    "Black",
    "Bold",
    "Book",
    "Light",
    "Medium",
    "Regular",
    "Roman",
    "Semibold"
  };
  private static int[] ISOAdobeCharset = new int[229]
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
    9,
    10,
    11,
    12,
    13,
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
    28,
    29,
    30,
    31 /*0x1F*/,
    32 /*0x20*/,
    33,
    34,
    35,
    36,
    37,
    38,
    39,
    40,
    41,
    42,
    43,
    44,
    45,
    46,
    47,
    48 /*0x30*/,
    49,
    50,
    51,
    52,
    53,
    54,
    55,
    56,
    57,
    58,
    59,
    60,
    61,
    62,
    63 /*0x3F*/,
    64 /*0x40*/,
    65,
    66,
    67,
    68,
    69,
    70,
    71,
    72,
    73,
    74,
    75,
    76,
    77,
    78,
    79,
    80 /*0x50*/,
    81,
    82,
    83,
    84,
    85,
    86,
    87,
    88,
    89,
    90,
    91,
    92,
    93,
    94,
    95,
    96 /*0x60*/,
    97,
    98,
    99,
    100,
    101,
    102,
    103,
    104,
    105,
    106,
    107,
    108,
    109,
    110,
    111,
    112 /*0x70*/,
    113,
    114,
    115,
    116,
    117,
    118,
    119,
    120,
    121,
    122,
    123,
    124,
    125,
    126,
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
    160 /*0xA0*/,
    161,
    162,
    163,
    164,
    165,
    166,
    167,
    168,
    169,
    170,
    171,
    172,
    173,
    174,
    175,
    176 /*0xB0*/,
    177,
    178,
    179,
    180,
    181,
    182,
    183,
    184,
    185,
    186,
    187,
    188,
    189,
    190,
    191,
    192 /*0xC0*/,
    193,
    194,
    195,
    196,
    197,
    198,
    199,
    200,
    201,
    202,
    203,
    204,
    205,
    206,
    207,
    208 /*0xD0*/,
    209,
    210,
    211,
    212,
    213,
    214,
    215,
    216,
    217,
    218,
    219,
    220,
    221,
    222,
    223,
    224 /*0xE0*/,
    225,
    226,
    227,
    228
  };
  private static int[] ExpertCharset = new int[166]
  {
    0,
    1,
    229,
    230,
    231,
    232,
    233,
    234,
    235,
    236,
    237,
    238,
    13,
    14,
    15,
    99,
    239,
    240 /*0xF0*/,
    241,
    242,
    243,
    244,
    245,
    246,
    247,
    248,
    27,
    28,
    249,
    250,
    251,
    252,
    253,
    254,
    (int) byte.MaxValue,
    256 /*0x0100*/,
    257,
    258,
    259,
    260,
    261,
    262,
    263,
    264,
    265,
    266,
    109,
    110,
    267,
    268,
    269,
    270,
    271,
    272,
    273,
    274,
    275,
    276,
    277,
    278,
    279,
    280,
    281,
    282,
    283,
    284,
    285,
    286,
    287,
    288,
    289,
    290,
    291,
    292,
    293,
    294,
    295,
    296,
    297,
    298,
    299,
    300,
    301,
    302,
    303,
    304,
    305,
    306,
    307,
    308,
    309,
    310,
    311,
    312,
    313,
    314,
    315,
    316,
    317,
    318,
    158,
    155,
    163,
    319,
    320,
    321,
    322,
    323,
    324,
    325,
    326,
    150,
    164,
    169,
    327,
    328,
    329,
    330,
    331,
    332,
    333,
    334,
    335,
    336,
    337,
    338,
    339,
    340,
    341,
    342,
    343,
    344,
    345,
    346,
    347,
    348,
    349,
    350,
    351,
    352,
    353,
    354,
    355,
    356,
    357,
    358,
    359,
    360,
    361,
    362,
    363,
    364,
    365,
    366,
    367,
    368,
    369,
    370,
    371,
    372,
    373,
    374,
    375,
    376,
    377,
    378
  };
  public int localBias;

  internal CffGlyphs readType1CFontFile(byte[] fontDataAsArray)
  {
    StreamReader streamReader = new StreamReader((Stream) new MemoryStream(fontDataAsArray));
label_1:
    string str1;
    do
    {
      str1 = streamReader.ReadLine();
      if (str1 == null)
        goto label_10;
    }
    while (str1.IndexOf("/FontMatrix") == -1);
    string str2 = "";
    int num1 = str1.IndexOf('[');
    if (num1 != -1)
    {
      int num2 = str1.IndexOf(']');
      str2 = str1.Substring(num1 + 1, num2 - (num1 + 1));
    }
    else
    {
      int num3 = str1.IndexOf('{');
      if (num3 != -1)
      {
        int num4 = str1.IndexOf('}');
        str2 = str1.Substring(num3 + 1, num4 - (num3 + 1));
      }
    }
    string[] strArray = str2.Split(new string[1]{ " " }, StringSplitOptions.RemoveEmptyEntries);
    for (int index = 0; index < 6; ++index)
      this.m_fontMatrix[index] = Convert.ToDouble(strArray[index]);
    goto label_1;
label_10:
    int size = 2;
    int fontDataAs1 = (int) fontDataAsArray[0];
    int fontDataAs2 = (int) fontDataAsArray[1];
    this.top = (int) fontDataAsArray[2];
    int word1 = this.getWord(fontDataAsArray, this.top, size);
    int fontDataAs3 = (int) fontDataAsArray[this.top + size];
    this.top += size + 1;
    this.top = this.top + (word1 + 1) * fontDataAs3 - 1 + this.getWord(fontDataAsArray, this.top + word1 * fontDataAs3, fontDataAs3);
    int word2 = this.getWord(fontDataAsArray, this.top, size);
    int fontDataAs4 = (int) fontDataAsArray[this.top + size];
    this.top += size + 1;
    int start = this.top + (word2 + 1) * fontDataAs4 - 1;
    int dicStart1 = start + this.getWord(fontDataAsArray, this.top, fontDataAs4);
    int dicEnd1 = start + this.getWord(fontDataAsArray, this.top + fontDataAs4, fontDataAs4);
    string[] strings = this.readStringIndex(fontDataAsArray, start, fontDataAs4, word2);
    this.readGlobalSubRoutines(fontDataAsArray);
    this.decodeDictionary(fontDataAsArray, dicStart1, dicEnd1, strings);
    if (this.FDSelect != -1)
    {
      int fdArray = this.FDArray;
      int word3 = this.getWord(fontDataAsArray, fdArray, size);
      int fontDataAs5 = (int) fontDataAsArray[fdArray + size];
      int index = fdArray + (size + 1);
      int num5 = index + (word3 + 1) * fontDataAs5 - 1;
      int dicStart2 = num5 + this.getWord(fontDataAsArray, index, fontDataAs5);
      int dicEnd2 = num5 + this.getWord(fontDataAsArray, index + fontDataAs5, fontDataAs5);
      this.decodeDictionary(fontDataAsArray, dicStart2, dicEnd2, strings);
    }
    this.top = this.charstrings;
    this.glyphCount = this.getWord(fontDataAsArray, this.top, size);
    int[] names = this.readCharset(this.charset, this.glyphCount, this.charstrings, fontDataAsArray);
    this.setEncoding(fontDataAsArray, this.glyphCount, names);
    this.top = this.charstrings;
    this.readGlyphs(fontDataAsArray, this.glyphCount, names);
    if (this.privateDict != -1)
    {
      this.decodeDictionary(fontDataAsArray, this.privateDict, this.privateDictOffset + this.privateDict, strings);
      this.top = this.privateDict + this.privateDictOffset;
      if (this.top + 2 < fontDataAsArray.Length)
      {
        int word4 = this.getWord(fontDataAsArray, this.top, size);
        if (word4 > 0)
          this.readSubrs(fontDataAsArray, word4);
      }
    }
    this.m_cffGlyphs.Glyphs = this.glyphs;
    this.m_cffGlyphs.FontMatrix = this.m_fontMatrix;
    this.m_cffGlyphs.DiffTable = this.m_diffTable;
    this.m_cffGlyphs.GlobalBias = this.GlobalBias;
    return this.m_cffGlyphs;
  }

  private void setEncoding(byte[] fontDataAsArray, int totalGlyphCount, int[] names)
  {
    bool flag = fontDataAsArray != null;
    this.top = this.encodeValue;
    int charInt = 0;
    int num1 = 0;
    if (flag)
      num1 = (int) fontDataAsArray[this.top++] & (int) byte.MaxValue;
    string mappedChar = (string) null;
    if ((num1 & (int) sbyte.MaxValue) == 0)
    {
      int num2 = 0;
      if (flag)
        num2 = 1 + ((int) fontDataAsArray[this.top++] & (int) byte.MaxValue);
      if (num2 > totalGlyphCount)
        num2 = totalGlyphCount;
      for (int index = 1; index < num2; ++index)
      {
        if (flag)
        {
          charInt = (int) fontDataAsArray[this.top++] & (int) byte.MaxValue;
          mappedChar = this.getString(fontDataAsArray, names[index], this.stringIndex, this.stringStart, this.stringOffsetSize);
        }
        this.DifferenceTableMapping(charInt, mappedChar);
      }
    }
    else if ((num1 & (int) sbyte.MaxValue) == 1)
    {
      int num3 = 0;
      if (flag)
        num3 = (int) fontDataAsArray[this.top++] & (int) byte.MaxValue;
      int index1 = 1;
      for (int index2 = 0; index2 < num3; ++index2)
      {
        if (flag)
          charInt = (int) fontDataAsArray[this.top++] & (int) byte.MaxValue;
        for (int index3 = 0; index3 <= charInt && index1 < totalGlyphCount; ++index3)
        {
          if (flag)
            mappedChar = this.getString(fontDataAsArray, names[index1], this.stringIndex, this.stringStart, this.stringOffsetSize);
          this.DifferenceTableMapping(charInt, mappedChar);
          ++index1;
          ++charInt;
        }
      }
    }
    if ((num1 & 128 /*0x80*/) == 0)
      return;
    int num4 = 0;
    if (flag)
      num4 = (int) fontDataAsArray[this.top++] & (int) byte.MaxValue;
    for (int index = 0; index < num4; ++index)
    {
      if (flag)
        charInt = (int) fontDataAsArray[this.top++] & (int) byte.MaxValue;
      int stringId = 0;
      if (flag)
        stringId = this.getWord(fontDataAsArray, this.top, 2);
      this.top += 2;
      if (flag)
        mappedChar = this.getString(fontDataAsArray, stringId, this.stringIndex, this.stringStart, this.stringOffsetSize);
      this.DifferenceTableMapping(charInt, mappedChar);
    }
  }

  internal void DifferenceTableMapping(int charInt, string mappedChar)
  {
    if (this.m_diffTable == null)
      this.m_diffTable = new string[this.maxCharCount];
    if (this.m_diffTable.Length > charInt)
      this.m_diffTable[charInt] = mappedChar;
    if (this.isCID)
      return;
    this.InsertMappedChar(charInt, mappedChar);
  }

  public void InsertMappedChar(int charInt, string mappedChar)
  {
    if (this.m_diffTable == null)
    {
      this.m_diffTable = new string[this.maxCharCount];
    }
    else
    {
      if (this.m_diffTable.Length <= charInt || this.m_diffTable[charInt] != null || mappedChar == null || mappedChar.StartsWith("glyph"))
        return;
      this.m_diffTable[charInt] = mappedChar;
    }
  }

  private int getWord(byte[] fontDataAsArray, int index, int size)
  {
    int word = 0;
    for (int index1 = 0; index1 < size; ++index1)
    {
      if (index + index1 < fontDataAsArray.Length)
        word = (word << 8) + ((int) fontDataAsArray[index + index1] & (int) byte.MaxValue);
    }
    return word;
  }

  private string[] readStringIndex(byte[] fontDataAsArray, int start, int offsize, int count)
  {
    int num1 = 0;
    bool flag = fontDataAsArray != null;
    if (flag)
    {
      this.top = start + this.getWord(fontDataAsArray, this.top + count * offsize, offsize);
      num1 = this.getWord(fontDataAsArray, this.top, 2);
      this.stringOffsetSize = (int) fontDataAsArray[this.top + 2];
    }
    this.top += 3;
    this.stringIndex = this.top;
    this.stringStart = this.top + (num1 + 1) * this.stringOffsetSize - 1;
    if (flag)
      this.top = this.stringStart + this.getWord(fontDataAsArray, this.top + num1 * this.stringOffsetSize, this.stringOffsetSize);
    int[] numArray = new int[num1 + 2];
    string[] strArray = new string[num1 + 2];
    int stringIndex = this.stringIndex;
    for (int index = 0; index < num1 + 1; ++index)
    {
      if (flag)
        numArray[index] = this.getWord(fontDataAsArray, stringIndex, this.stringOffsetSize);
      stringIndex += this.stringOffsetSize;
    }
    numArray[num1 + 1] = this.top - this.stringStart;
    int num2 = 0;
    for (int index1 = 0; index1 < num1 + 1; ++index1)
    {
      StringBuilder stringBuilder = new StringBuilder(numArray[index1] - num2);
      for (int index2 = num2; index2 < numArray[index1]; ++index2)
      {
        if (flag)
          stringBuilder.Append((char) fontDataAsArray[this.stringStart + index2]);
      }
      strArray[index1] = stringBuilder.ToString();
      num2 = numArray[index1];
    }
    return strArray;
  }

  private void readGlobalSubRoutines(byte[] fontDataAsArray)
  {
    bool flag = fontDataAsArray != null;
    int size = 0;
    int subroutineCount = 0;
    if (flag)
    {
      size = (int) fontDataAsArray[this.top + 2] & (int) byte.MaxValue;
      subroutineCount = this.getWord(fontDataAsArray, this.top, 2);
    }
    this.top += 3;
    if (subroutineCount <= 0)
      return;
    int top = this.top;
    int num1 = this.top + (subroutineCount + 1) * size - 1;
    if (flag)
      this.top = num1 + this.getWord(fontDataAsArray, this.top + subroutineCount * size, size);
    int[] numArray = new int[subroutineCount + 2];
    int index1 = top;
    for (int index2 = 0; index2 < subroutineCount + 1; ++index2)
    {
      if (flag)
        numArray[index2] = num1 + this.getWord(fontDataAsArray, index1, size);
      index1 += size;
    }
    numArray[subroutineCount + 1] = this.top;
    this.GlobalBias = this.calculateSubroutineBias(subroutineCount);
    int num2 = numArray[0];
    for (int index3 = 1; index3 < subroutineCount + 1; ++index3)
    {
      MemoryStream memoryStream = new MemoryStream();
      for (int index4 = num2; index4 < numArray[index3]; ++index4)
      {
        if (flag)
          memoryStream.WriteByte(fontDataAsArray[index4]);
      }
      memoryStream.Close();
      this.glyphs.Add("global" + (index3 - 1).ToString(), memoryStream.ToArray());
      num2 = numArray[index3];
    }
  }

  private void decodeDictionary(
    byte[] fontDataAsArray,
    int dicStart,
    int dicEnd,
    string[] strings)
  {
    bool flag1 = false;
    bool flag2 = fontDataAsArray != null;
    int pos = dicStart;
    int num1 = 0;
    int valuePointer = 0;
    double[] numArray = new double[48 /*0x30*/];
    while (pos < dicEnd)
    {
      if (flag2)
        num1 = (int) fontDataAsArray[pos] & (int) byte.MaxValue;
      if (num1 <= 27 || num1 == 31 /*0x1F*/)
      {
        int num2 = num1;
        ++pos;
        switch (num2)
        {
          case 2:
            int index1 = (int) numArray[0];
            if (index1 > 390)
              index1 -= 390;
            this.embeddedFontName = strings[index1];
            break;
          case 5:
            for (int index2 = 0; index2 < 4; ++index2)
              this.FontBBox[index2] = (float) numArray[index2];
            break;
          case 12:
            if (flag2)
              num2 = (int) fontDataAsArray[pos] & (int) byte.MaxValue;
            ++pos;
            if (num2 == 36 || num2 == 37 || num2 == 7 || this.FDSelect == -1)
            {
              switch (num2)
              {
                case 0:
                  int index3 = (int) numArray[0];
                  if (index3 > 390)
                    index3 -= 390;
                  this.copyright = strings[index3];
                  break;
                case 2:
                  this.italicAngle = (int) numArray[0];
                  break;
                case 7:
                  if (!this.hasFontMatrix)
                    Array.Copy((Array) numArray, 0, (Array) this.FontMatrix, 0, 6);
                  this.hasFontMatrix = true;
                  break;
                case 30:
                  this.ROS = (int) numArray[0];
                  this.isCID = true;
                  break;
                case 31 /*0x1F*/:
                  this.CIDFontVersion = (int) numArray[0];
                  break;
                case 32 /*0x20*/:
                  this.CIDFontRevision = (int) numArray[0];
                  break;
                case 33:
                  this.CIDFontType = (int) numArray[0];
                  break;
                case 34:
                  this.CIDcount = (int) numArray[0];
                  break;
                case 35:
                  this.UIDBase = (int) numArray[0];
                  break;
                case 36:
                  this.FDArray = (int) numArray[0];
                  break;
                case 37:
                  this.FDSelect = (int) numArray[0];
                  flag1 = true;
                  break;
                default:
                  if (num2 == 21 || num2 == 22 || num2 != 38)
                    break;
                  break;
              }
            }
            else
              break;
            break;
          case 15:
            this.charset = (int) numArray[0];
            break;
          case 16 /*0x10*/:
            this.encodeValue = (int) numArray[0];
            break;
          case 17:
            this.charstrings = (int) numArray[0];
            break;
          case 18:
            this.privateDict = (int) numArray[1];
            this.privateDictOffset = (int) numArray[0];
            break;
          case 20:
            this.defaultWidthX = (int) numArray[0];
            break;
          case 21:
            this.nominalWidthX = (int) numArray[0];
            break;
        }
        valuePointer = 0;
      }
      else
      {
        if (flag2)
          pos = this.getNumber(fontDataAsArray, pos, numArray, valuePointer, false);
        ++valuePointer;
      }
    }
    if (flag1)
      return;
    this.FDSelect = -1;
  }

  public int getNumber(
    byte[] fontDataAsArray,
    int pos,
    double[] values,
    int valuePointer,
    bool debug)
  {
    double num1 = 0.0;
    int num2 = (int) fontDataAsArray[pos] & (int) byte.MaxValue;
    if (num2 < 28 | num2 == 31 /*0x1F*/)
    {
      Console.Error.WriteLine("!!!!Incorrect type1C operand");
    }
    else
    {
      switch (num2)
      {
        case 28:
          num1 = (double) (((int) fontDataAsArray[pos + 1] << 8) + ((int) fontDataAsArray[pos + 2] & (int) byte.MaxValue));
          pos += 3;
          break;
        case 29:
          num1 = (double) ((((int) fontDataAsArray[pos + 1] & (int) byte.MaxValue) << 24) + (((int) fontDataAsArray[pos + 2] & (int) byte.MaxValue) << 16 /*0x10*/) + (((int) fontDataAsArray[pos + 3] & (int) byte.MaxValue) << 8) + ((int) fontDataAsArray[pos + 4] & (int) byte.MaxValue));
          pos += 5;
          break;
        case 30:
          char[] chArray = new char[65];
          ++pos;
          int length = 0;
          while (length < 64 /*0x40*/)
          {
            int num3 = (int) fontDataAsArray[pos++] & (int) byte.MaxValue;
            int index1 = num3 >> 4 & 15;
            int index2 = num3 & 15;
            if (index1 != 15)
            {
              chArray[length++] = this.nybChars[index1];
              if (length != 64 /*0x40*/)
              {
                if (index1 == 12)
                  chArray[length++] = '-';
                if (length != 64 /*0x40*/ && index2 != 15)
                {
                  chArray[length++] = this.nybChars[index2];
                  if (length != 64 /*0x40*/)
                  {
                    if (index2 == 12)
                      chArray[length++] = '-';
                  }
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
            }
            else
              break;
          }
          num1 = Convert.ToDouble(new string(chArray, 0, length));
          break;
        case (int) byte.MaxValue:
          int num4 = (((int) fontDataAsArray[pos + 1] & (int) byte.MaxValue) << 8) + ((int) fontDataAsArray[pos + 2] & (int) byte.MaxValue);
          if (num4 > 32768 /*0x8000*/)
            num4 = 65536 /*0x010000*/ - num4;
          num1 = (double) num4 + (double) ((((int) fontDataAsArray[pos + 3] & (int) byte.MaxValue) << 8) + ((int) fontDataAsArray[pos + 4] & (int) byte.MaxValue)) / 65536.0;
          if (fontDataAsArray[pos + 1] < (byte) 0)
          {
            if (debug)
              Console.WriteLine("Negative " + (object) num1);
            num1 = -num1;
          }
          if (debug)
          {
            Console.WriteLine("x=" + (object) num1);
            for (int index = 0; index < 5; ++index)
              Console.WriteLine($"{(object) index} {(object) fontDataAsArray[pos + index]}{(object) ' '}{(object) ((int) fontDataAsArray[pos + index] & (int) byte.MaxValue)}{(object) ' '}{(object) ((int) fontDataAsArray[pos + index] & (int) sbyte.MaxValue)}");
          }
          pos += 5;
          break;
        default:
          if (num2 < 247)
          {
            num1 = (double) (num2 - 139);
            ++pos;
            break;
          }
          if (num2 < 251)
          {
            num1 = (double) ((num2 - 247 << 8) + ((int) fontDataAsArray[pos + 1] & (int) byte.MaxValue) + 108);
            pos += 2;
            break;
          }
          num1 = (double) (-(num2 - 251 << 8) - ((int) fontDataAsArray[pos + 1] & (int) byte.MaxValue) - 108);
          pos += 2;
          break;
      }
    }
    values[valuePointer] = num1;
    return pos;
  }

  private int[] readCharset(int charset, int nGlyphs, int top, byte[] fontDataAsArray)
  {
    bool flag = fontDataAsArray != null;
    int[] numArray;
    switch (charset)
    {
      case 0:
        numArray = FontFile3.ISOAdobeCharset;
        break;
      case 1:
        numArray = FontFile3.ExpertCharset;
        break;
      case 2:
        numArray = FontFile3.ExpertSubCharset;
        break;
      default:
        numArray = new int[nGlyphs + 1];
        numArray[0] = 0;
        top = charset;
        int num1 = 0;
        if (flag)
          num1 = (int) fontDataAsArray[top++] & (int) byte.MaxValue;
        switch (num1)
        {
          case 0:
            for (int index = 1; index < nGlyphs; ++index)
            {
              if (flag)
                numArray[index] = this.getWord(fontDataAsArray, top, 2);
              top += 2;
            }
            break;
          case 1:
            int num2 = 1;
            int num3 = 0;
            int num4 = 0;
            while (num2 < nGlyphs)
            {
              if (flag)
                num3 = this.getWord(fontDataAsArray, top, 2);
              top += 2;
              if (flag)
                num4 = (int) fontDataAsArray[top++] & (int) byte.MaxValue;
              for (int index = 0; index <= num4; ++index)
                numArray[num2++] = num3++;
            }
            break;
          case 2:
            int num5 = 1;
            int num6 = 0;
            int num7 = 0;
            while (num5 < nGlyphs)
            {
              if (flag)
                num6 = this.getWord(fontDataAsArray, top, 2);
              top += 2;
              if (flag)
                num7 = this.getWord(fontDataAsArray, top, 2);
              top += 2;
              for (int index = 0; index <= num7; ++index)
                numArray[num5++] = num6++;
            }
            break;
        }
        break;
    }
    return numArray;
  }

  private int calculateSubroutineBias(int subroutineCount)
  {
    return subroutineCount >= 1240 ? (subroutineCount >= 33900 ? 32768 /*0x8000*/ : 1131) : 107;
  }

  internal virtual void readGlyphs(byte[] fontDataAsArray, int nGlyphs, int[] names)
  {
    bool flag = fontDataAsArray != null;
    int size = 0;
    if (flag)
      size = (int) fontDataAsArray[this.top + 2];
    this.top += 3;
    int top = this.top;
    int num1 = this.top + (nGlyphs + 1) * size - 1;
    if (flag)
      this.top = num1 + this.getWord(fontDataAsArray, this.top + nGlyphs * size, size);
    int[] numArray1 = new int[nGlyphs + 2];
    int index1 = top;
    for (int index2 = 0; index2 < nGlyphs + 1; ++index2)
    {
      if (flag)
        numArray1[index2] = num1 + this.getWord(fontDataAsArray, index1, size);
      index1 += size;
    }
    numArray1[nGlyphs + 1] = this.top;
    int num2 = numArray1[0];
    string key = "";
    for (int index3 = 1; index3 < nGlyphs + 1; ++index3)
    {
      byte[] numArray2 = new byte[numArray1[index3] - num2];
      for (int index4 = num2; index4 < numArray1[index3]; ++index4)
      {
        if (flag)
          numArray2[index4 - num2] = fontDataAsArray[index4];
      }
      if (this.isCID)
        key = Convert.ToString(names[index3 - 1]);
      else if (flag)
        key = this.getString(fontDataAsArray, names[index3 - 1], this.stringIndex, this.stringStart, this.stringOffsetSize);
      this.glyphs.Add(key, numArray2);
      num2 = numArray1[index3];
      int num3 = this.trackIndices ? 1 : 0;
    }
  }

  private string getString(byte[] fontDataAsArray, int stringId, int idx, int start, int offsize)
  {
    string type1CstdString;
    if (stringId < 391)
    {
      type1CstdString = FontFile3.type1CStdStrings[stringId];
    }
    else
    {
      stringId -= 391;
      int index = start + this.getWord(fontDataAsArray, idx + stringId * offsize, offsize);
      int num1 = start + this.getWord(fontDataAsArray, idx + (stringId + 1) * offsize, offsize);
      int num2;
      int count;
      if ((num2 = num1 - index) > (int) byte.MaxValue)
        count = (int) byte.MaxValue;
      else if ((count = num1 - index) < 0)
        count = 0;
      type1CstdString = Encoding.UTF8.GetString(fontDataAsArray, index, count);
    }
    return type1CstdString;
  }

  internal virtual void readSubrs(byte[] fontDataAsArray, int nSubrs)
  {
    bool flag = fontDataAsArray != null;
    int size = 0;
    if (flag)
      size = (int) fontDataAsArray[this.top + 2];
    this.top += 3;
    int top = this.top;
    int num = this.top + (nSubrs + 1) * size - 1;
    int index1 = this.top + nSubrs * size;
    if (flag)
      this.top = index1 >= fontDataAsArray.Length ? fontDataAsArray.Length - 1 : num + this.getWord(fontDataAsArray, index1, size);
    int[] numArray = new int[nSubrs + 2];
    int index2 = top;
    for (int index3 = 0; index3 < nSubrs + 1; ++index3)
    {
      if (flag && index2 + size < fontDataAsArray.Length)
        numArray[index3] = num + this.getWord(fontDataAsArray, index2, size);
      index2 += size;
    }
    numArray[nSubrs + 1] = this.top;
    this.localBias = this.calculateSubroutineBias(nSubrs);
    int sourceIndex = numArray[0];
    for (int index4 = 1; index4 < nSubrs + 1; ++index4)
    {
      if (sourceIndex != 0 && numArray[index4] <= fontDataAsArray.Length && numArray[index4] >= 0 && numArray[index4] != 0)
      {
        if (flag)
        {
          int length = numArray[index4] - sourceIndex;
          if (length > 0)
          {
            byte[] destinationArray = new byte[length];
            Array.Copy((Array) fontDataAsArray, sourceIndex, (Array) destinationArray, 0, length);
            this.glyphs.Add("subrs" + (index4 - 1).ToString(), destinationArray);
          }
        }
        sourceIndex = numArray[index4];
      }
    }
  }
}
