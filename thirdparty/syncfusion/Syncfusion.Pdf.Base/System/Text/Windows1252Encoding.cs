﻿// Decompiled with JetBrains decompiler
// Type: System.Text.Windows1252Encoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace System.Text;

internal class Windows1252Encoding : Encoding
{
  private static int[] m_charCodeTable = new int[256 /*0x0100*/]
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
    8364,
    65533,
    8218,
    402,
    8222,
    8230,
    8224,
    8225,
    710,
    8240,
    352,
    8249,
    338,
    65533,
    381,
    65533,
    65533,
    8216,
    8217,
    8220,
    8221,
    8226,
    8211,
    8212,
    732,
    8482,
    353,
    8250,
    339,
    65533,
    382,
    376,
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
    228,
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
    249,
    250,
    251,
    252,
    253,
    254,
    (int) byte.MaxValue
  };

  public override byte[] GetBytes(string s)
  {
    List<byte> byteList = new List<byte>();
    foreach (char index in s)
    {
      if ((int) index < Windows1252Encoding.m_charCodeTable.Length)
        byteList.Add((byte) Windows1252Encoding.m_charCodeTable[(int) index]);
    }
    return byteList.ToArray();
  }

  public override string GetString(byte[] bytes, int index, int count)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index1 = index; index1 < count; ++index1)
      stringBuilder.Append((char) bytes[index1]);
    return stringBuilder.ToString();
  }

  public override int GetByteCount(char[] chars, int index, int count)
  {
    throw new NotImplementedException();
  }

  public override int GetBytes(
    char[] chars,
    int charIndex,
    int charCount,
    byte[] bytes,
    int byteIndex)
  {
    throw new NotImplementedException();
  }

  public override int GetCharCount(byte[] bytes, int index, int count)
  {
    throw new NotImplementedException();
  }

  public override int GetChars(
    byte[] bytes,
    int byteIndex,
    int byteCount,
    char[] chars,
    int charIndex)
  {
    throw new NotImplementedException();
  }

  public override int GetMaxByteCount(int charCount) => throw new NotImplementedException();

  public override int GetMaxCharCount(int byteCount) => throw new NotImplementedException();
}
