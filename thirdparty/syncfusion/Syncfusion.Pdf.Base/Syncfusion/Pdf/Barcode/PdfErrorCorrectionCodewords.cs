// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfErrorCorrectionCodewords
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

internal class PdfErrorCorrectionCodewords
{
  private int length;
  private int eccw;
  private int databits;
  private string[] dataCodeWord;
  private int[] gx;
  private int[] alpha = new int[(int) byte.MaxValue]
  {
    1,
    2,
    4,
    8,
    16 /*0x10*/,
    32 /*0x20*/,
    64 /*0x40*/,
    128 /*0x80*/,
    29,
    58,
    116,
    232,
    205,
    135,
    19,
    38,
    76,
    152,
    45,
    90,
    180,
    117,
    234,
    201,
    143,
    3,
    6,
    12,
    24,
    48 /*0x30*/,
    96 /*0x60*/,
    192 /*0xC0*/,
    157,
    39,
    78,
    156,
    37,
    74,
    148,
    53,
    106,
    212,
    181,
    119,
    238,
    193,
    159,
    35,
    70,
    140,
    5,
    10,
    20,
    40,
    80 /*0x50*/,
    160 /*0xA0*/,
    93,
    186,
    105,
    210,
    185,
    111,
    222,
    161,
    95,
    190,
    97,
    194,
    153,
    47,
    94,
    188,
    101,
    202,
    137,
    15,
    30,
    60,
    120,
    240 /*0xF0*/,
    253,
    231,
    211,
    187,
    107,
    214,
    177,
    (int) sbyte.MaxValue,
    254,
    225,
    223,
    163,
    91,
    182,
    113,
    226,
    217,
    175,
    67,
    134,
    17,
    34,
    68,
    136,
    13,
    26,
    52,
    104,
    208 /*0xD0*/,
    189,
    103,
    206,
    129,
    31 /*0x1F*/,
    62,
    124,
    248,
    237,
    199,
    147,
    59,
    118,
    236,
    197,
    151,
    51,
    102,
    204,
    133,
    23,
    46,
    92,
    184,
    109,
    218,
    169,
    79,
    158,
    33,
    66,
    132,
    21,
    42,
    84,
    168,
    77,
    154,
    41,
    82,
    164,
    85,
    170,
    73,
    146,
    57,
    114,
    228,
    213,
    183,
    115,
    230,
    209,
    191,
    99,
    198,
    145,
    63 /*0x3F*/,
    126,
    252,
    229,
    215,
    179,
    123,
    246,
    241,
    (int) byte.MaxValue,
    227,
    219,
    171,
    75,
    150,
    49,
    98,
    196,
    149,
    55,
    110,
    220,
    165,
    87,
    174,
    65,
    130,
    25,
    50,
    100,
    200,
    141,
    7,
    14,
    28,
    56,
    112 /*0x70*/,
    224 /*0xE0*/,
    221,
    167,
    83,
    166,
    81,
    162,
    89,
    178,
    121,
    242,
    249,
    239,
    195,
    155,
    43,
    86,
    172,
    69,
    138,
    9,
    18,
    36,
    72,
    144 /*0x90*/,
    61,
    122,
    244,
    245,
    247,
    243,
    251,
    235,
    203,
    139,
    11,
    22,
    44,
    88,
    176 /*0xB0*/,
    125,
    250,
    233,
    207,
    131,
    27,
    54,
    108,
    216,
    173,
    71,
    142
  };
  private int[] decimalValue;
  private PdfQRBarcodeValues qrBarcodeValues;

  internal string[] DC
  {
    set => this.dataCodeWord = value;
    get => this.dataCodeWord;
  }

  internal int DataBits
  {
    set => this.databits = value;
    get => this.databits;
  }

  internal int ECCW
  {
    set => this.eccw = value;
    get => this.eccw;
  }

  public PdfErrorCorrectionCodewords(QRCodeVersion version, PdfErrorCorrectionLevel correctionLevel)
  {
    this.qrBarcodeValues = new PdfQRBarcodeValues(version, correctionLevel);
    this.length = this.qrBarcodeValues.DataCapacity;
    this.eccw = this.qrBarcodeValues.NumberOfErrorCorrectingCodeWords;
  }

  internal string[] GetERCW()
  {
    this.decimalValue = new int[this.databits];
    switch (this.eccw)
    {
      case 7:
        this.gx = new int[8]
        {
          0,
          87,
          229,
          146,
          149,
          238,
          102,
          21
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 10:
        this.gx = new int[11]
        {
          0,
          251,
          67,
          46,
          61,
          118,
          70,
          64 /*0x40*/,
          94,
          32 /*0x20*/,
          45
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 13:
        this.gx = new int[14]
        {
          0,
          74,
          152,
          176 /*0xB0*/,
          100,
          86,
          100,
          106,
          104,
          130,
          218,
          206,
          140,
          78
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 15:
        this.gx = new int[16 /*0x10*/]
        {
          0,
          8,
          183,
          61,
          91,
          202,
          37,
          51,
          58,
          58,
          237,
          140,
          124,
          5,
          99,
          105
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 16 /*0x10*/:
        this.gx = new int[17]
        {
          0,
          120,
          104,
          107,
          109,
          102,
          161,
          76,
          3,
          91,
          191,
          147,
          169,
          182,
          194,
          225,
          120
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 17:
        this.gx = new int[18]
        {
          0,
          43,
          139,
          206,
          78,
          43,
          239,
          123,
          206,
          214,
          147,
          24,
          99,
          150,
          39,
          243,
          163,
          136
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 18:
        this.gx = new int[19]
        {
          0,
          215,
          234,
          158,
          94,
          184,
          97,
          118,
          170,
          79,
          187,
          152,
          148,
          252,
          179,
          5,
          98,
          96 /*0x60*/,
          153
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 20:
        this.gx = new int[21]
        {
          0,
          17,
          60,
          79,
          50,
          61,
          163,
          26,
          187,
          202,
          180,
          221,
          225,
          83,
          239,
          156,
          164,
          212,
          212,
          188,
          190
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 22:
        this.gx = new int[23]
        {
          0,
          210,
          171,
          247,
          242,
          93,
          230,
          14,
          109,
          221,
          53,
          200,
          74,
          8,
          172,
          98,
          80 /*0x50*/,
          219,
          134,
          160 /*0xA0*/,
          105,
          165,
          231
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 24:
        this.gx = new int[25]
        {
          0,
          229,
          121,
          135,
          48 /*0x30*/,
          211,
          117,
          251,
          126,
          159,
          180,
          169,
          152,
          192 /*0xC0*/,
          226,
          228,
          218,
          111,
          0,
          117,
          232,
          87,
          96 /*0x60*/,
          227,
          21
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 26:
        this.gx = new int[27]
        {
          0,
          173,
          125,
          158,
          2,
          103,
          182,
          118,
          17,
          145,
          201,
          111,
          28,
          165,
          53,
          161,
          21,
          245,
          142,
          13,
          102,
          48 /*0x30*/,
          227,
          153,
          145,
          218,
          70
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 28:
        this.gx = new int[29]
        {
          0,
          168,
          223,
          200,
          104,
          224 /*0xE0*/,
          234,
          108,
          180,
          110,
          190,
          195,
          147,
          205,
          27,
          232,
          201,
          21,
          43,
          245,
          87,
          42,
          195,
          212,
          119,
          242,
          37,
          9,
          123
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 30:
        this.gx = new int[31 /*0x1F*/]
        {
          0,
          41,
          173,
          145,
          152,
          216,
          31 /*0x1F*/,
          179,
          182,
          50,
          48 /*0x30*/,
          110,
          86,
          239,
          96 /*0x60*/,
          222,
          125,
          42,
          173,
          226,
          193,
          224 /*0xE0*/,
          130,
          156,
          37,
          251,
          216,
          238,
          40,
          192 /*0xC0*/,
          180
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 32 /*0x20*/:
        this.gx = new int[33]
        {
          0,
          10,
          6,
          106,
          190,
          249,
          167,
          4,
          67,
          209,
          138,
          138,
          32 /*0x20*/,
          242,
          123,
          89,
          27,
          120,
          185,
          80 /*0x50*/,
          156,
          38,
          69,
          171,
          60,
          28,
          222,
          80 /*0x50*/,
          52,
          254,
          185,
          220,
          241
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 34:
        this.gx = new int[35]
        {
          0,
          111,
          77,
          146,
          94,
          26,
          21,
          108,
          19,
          105,
          94,
          113,
          193,
          86,
          140,
          163,
          125,
          58,
          158,
          229,
          239,
          218,
          103,
          56,
          70,
          114,
          61,
          183,
          129,
          167,
          13,
          98,
          62,
          129,
          51
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 36:
        this.gx = new int[37]
        {
          0,
          200,
          183,
          98,
          16 /*0x10*/,
          172,
          31 /*0x1F*/,
          246,
          234,
          60,
          152,
          115,
          0,
          167,
          152,
          113,
          248,
          238,
          107,
          18,
          63 /*0x3F*/,
          218,
          37,
          87,
          210,
          105,
          177,
          120,
          74,
          121,
          196,
          117,
          251,
          113,
          233,
          30,
          120
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 40:
        this.gx = new int[41]
        {
          0,
          59,
          116,
          79,
          161,
          252,
          98,
          128 /*0x80*/,
          205,
          128 /*0x80*/,
          161,
          247,
          57,
          163,
          56,
          235,
          106,
          53,
          26,
          187,
          174,
          226,
          104,
          170,
          7,
          175,
          35,
          181,
          114,
          88,
          41,
          47,
          163,
          125,
          134,
          72,
          20,
          232,
          53,
          35,
          15
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 42:
        this.gx = new int[43]
        {
          0,
          250,
          103,
          221,
          230,
          25,
          18,
          137,
          231,
          0,
          3,
          58,
          242,
          221,
          191,
          110,
          84,
          230,
          8,
          188,
          106,
          96 /*0x60*/,
          147,
          15,
          131,
          139,
          34,
          101,
          223,
          39,
          101,
          213,
          199,
          237,
          254,
          201,
          123,
          171,
          162,
          194,
          117,
          50,
          96 /*0x60*/
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 44:
        this.gx = new int[45]
        {
          0,
          190,
          7,
          61,
          121,
          71,
          246,
          69,
          55,
          168,
          188,
          89,
          243,
          191,
          25,
          72,
          123,
          9,
          145,
          14,
          247,
          1,
          238,
          44,
          78,
          143,
          62,
          224 /*0xE0*/,
          126,
          118,
          114,
          68,
          163,
          52,
          194,
          217,
          147,
          204,
          169,
          37,
          130,
          113,
          102,
          73,
          181
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 46:
        this.gx = new int[47]
        {
          0,
          112 /*0x70*/,
          94,
          88,
          112 /*0x70*/,
          253,
          224 /*0xE0*/,
          202,
          115,
          187,
          99,
          89,
          5,
          54,
          113,
          129,
          44,
          58,
          16 /*0x10*/,
          135,
          216,
          169,
          211,
          36,
          1,
          4,
          96 /*0x60*/,
          60,
          241,
          73,
          104,
          234,
          8,
          249,
          245,
          119,
          174,
          52,
          25,
          157,
          224 /*0xE0*/,
          43,
          202,
          223,
          19,
          82,
          15
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 48 /*0x30*/:
        this.gx = new int[49]
        {
          0,
          228,
          25,
          196,
          130,
          211,
          146,
          60,
          24,
          251,
          90,
          39,
          102,
          240 /*0xF0*/,
          61,
          178,
          63 /*0x3F*/,
          46,
          123,
          115,
          18,
          221,
          111,
          135,
          160 /*0xA0*/,
          182,
          205,
          107,
          206,
          95,
          150,
          120,
          184,
          91,
          21,
          247,
          156,
          140,
          238,
          191,
          11,
          94,
          227,
          84,
          50,
          163,
          39,
          34,
          108
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 50:
        this.gx = new int[51]
        {
          0,
          232,
          125,
          157,
          161,
          164,
          9,
          118,
          46,
          209,
          99,
          203,
          193,
          35,
          3,
          209,
          111,
          195,
          242,
          203,
          225,
          46,
          13,
          32 /*0x20*/,
          160 /*0xA0*/,
          126,
          209,
          130,
          160 /*0xA0*/,
          242,
          215,
          242,
          75,
          77,
          42,
          189,
          32 /*0x20*/,
          113,
          65,
          124,
          69,
          228,
          114,
          235,
          175,
          124,
          170,
          215,
          232,
          133,
          205
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 52:
        this.gx = new int[53]
        {
          0,
          116,
          50,
          86,
          186,
          50,
          220,
          251,
          89,
          192 /*0xC0*/,
          46,
          86,
          (int) sbyte.MaxValue,
          124,
          19,
          184,
          233,
          151,
          215,
          22,
          14,
          59,
          145,
          37,
          242,
          203,
          134,
          254,
          89,
          190,
          94,
          59,
          65,
          124,
          113,
          100,
          233,
          235,
          121,
          22,
          76,
          86,
          97,
          39,
          242,
          200,
          220,
          101,
          33,
          239,
          254,
          116,
          51
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 54:
        this.gx = new int[55]
        {
          0,
          183,
          26,
          201,
          87,
          210,
          221,
          113,
          21,
          46,
          65,
          45,
          50,
          238,
          184,
          249,
          225,
          102,
          58,
          209,
          218,
          109,
          165,
          26,
          95,
          184,
          192 /*0xC0*/,
          52,
          245,
          35,
          254,
          238,
          175,
          172,
          79,
          123,
          25,
          122,
          43,
          120,
          108,
          215,
          80 /*0x50*/,
          128 /*0x80*/,
          201,
          235,
          8,
          153,
          59,
          101,
          31 /*0x1F*/,
          198,
          76,
          31 /*0x1F*/,
          156
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 56:
        this.gx = new int[57]
        {
          0,
          106,
          120,
          107,
          157,
          164,
          216,
          112 /*0x70*/,
          116,
          2,
          91,
          248,
          163,
          36,
          201,
          202,
          229,
          6,
          144 /*0x90*/,
          254,
          155,
          135,
          208 /*0xD0*/,
          170,
          209,
          12,
          139,
          (int) sbyte.MaxValue,
          142,
          182,
          249,
          177,
          174,
          190,
          28,
          10,
          85,
          239,
          184,
          101,
          124,
          152,
          206,
          96 /*0x60*/,
          23,
          163,
          61,
          27,
          196,
          247,
          151,
          154,
          202,
          207,
          20,
          61,
          10
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 58:
        this.gx = new int[59]
        {
          0,
          82,
          116,
          26,
          247,
          66,
          27,
          62,
          107,
          252,
          182,
          200,
          185,
          235,
          55,
          251,
          242,
          210,
          144 /*0x90*/,
          154,
          237,
          176 /*0xB0*/,
          141,
          192 /*0xC0*/,
          248,
          152,
          249,
          206,
          85,
          253,
          142,
          65,
          165,
          125,
          23,
          24,
          30,
          122,
          240 /*0xF0*/,
          214,
          6,
          129,
          218,
          29,
          145,
          (int) sbyte.MaxValue,
          134,
          206,
          245,
          117,
          29,
          41,
          63 /*0x3F*/,
          159,
          142,
          233,
          125,
          148,
          123
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 60:
        this.gx = new int[61]
        {
          0,
          107,
          140,
          26,
          12,
          9,
          141,
          243,
          197,
          226,
          197,
          219,
          45,
          211,
          101,
          219,
          120,
          28,
          181,
          (int) sbyte.MaxValue,
          6,
          100,
          247,
          2,
          205,
          198,
          57,
          115,
          219,
          101,
          109,
          160 /*0xA0*/,
          82,
          37,
          38,
          238,
          49,
          160 /*0xA0*/,
          209,
          121,
          86,
          11,
          124,
          30,
          181,
          84,
          25,
          194,
          87,
          65,
          102,
          190,
          220,
          70,
          27,
          209,
          16 /*0x10*/,
          89,
          7,
          33,
          240 /*0xF0*/
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 62:
        this.gx = new int[63 /*0x3F*/]
        {
          0,
          65,
          202,
          113,
          98,
          71,
          223,
          248,
          118,
          214,
          94,
          0,
          122,
          37,
          23,
          2,
          228,
          58,
          121,
          7,
          105,
          135,
          78,
          243,
          118,
          70,
          76,
          223,
          89,
          72,
          50,
          70,
          111,
          194,
          17,
          212,
          126,
          181,
          35,
          221,
          117,
          235,
          11,
          229,
          149,
          147,
          123,
          213,
          40,
          115,
          6,
          200,
          100,
          26,
          246,
          182,
          218,
          (int) sbyte.MaxValue,
          215,
          36,
          186,
          110,
          106
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 64 /*0x40*/:
        this.gx = new int[65]
        {
          0,
          45,
          51,
          175,
          9,
          7,
          158,
          159,
          49,
          68,
          119,
          92,
          123,
          177,
          204,
          187,
          254,
          200,
          78,
          141,
          149,
          119,
          26,
          (int) sbyte.MaxValue,
          53,
          160 /*0xA0*/,
          93,
          199,
          212,
          29,
          24,
          145,
          156,
          208 /*0xD0*/,
          150,
          218,
          209,
          4,
          216,
          91,
          47,
          184,
          146,
          47,
          140,
          195,
          195,
          125,
          242,
          238,
          63 /*0x3F*/,
          99,
          108,
          140,
          230,
          242,
          31 /*0x1F*/,
          204,
          11,
          178,
          243,
          217,
          156,
          213,
          231
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 66:
        this.gx = new int[67]
        {
          0,
          5,
          118,
          222,
          180,
          136,
          136,
          162,
          51,
          46,
          117,
          13,
          215,
          81,
          17,
          139,
          247,
          197,
          171,
          95,
          173,
          65,
          137,
          178,
          68,
          111,
          95,
          101,
          41,
          72,
          214,
          169,
          197,
          95,
          7,
          44,
          154,
          77,
          111,
          236,
          40,
          121,
          143,
          63 /*0x3F*/,
          87,
          80 /*0x50*/,
          253,
          240 /*0xF0*/,
          126,
          217,
          77,
          34,
          232,
          106,
          50,
          168,
          82,
          76,
          146,
          67,
          106,
          171,
          25,
          132,
          93,
          45,
          105
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
      case 68:
        this.gx = new int[69]
        {
          0,
          247,
          159,
          223,
          33,
          224 /*0xE0*/,
          93,
          77,
          70,
          90,
          160 /*0xA0*/,
          32 /*0x20*/,
          254,
          43,
          150,
          84,
          101,
          190,
          205,
          133,
          52,
          60,
          202,
          165,
          220,
          203,
          151,
          93,
          84,
          15,
          84,
          253,
          173,
          160 /*0xA0*/,
          89,
          227,
          52,
          199,
          97,
          95,
          231,
          52,
          177,
          41,
          125,
          137,
          241,
          166,
          225,
          118,
          2,
          54,
          32 /*0x20*/,
          82,
          215,
          175,
          198,
          43,
          238,
          235,
          27,
          101,
          184,
          (int) sbyte.MaxValue,
          3,
          5,
          8,
          163,
          238
        };
        this.gx = this.GetElement(this.gx, this.alpha);
        break;
    }
    this.BinaryToDecimal(this.dataCodeWord);
    return this.ConvertDecimalToBinary(this.CalculatePolynomialDivision());
  }

  private void BinaryToDecimal(string[] inString)
  {
    for (int index = 0; index < inString.Length; ++index)
      this.decimalValue[index] = Convert.ToInt32(inString[index], 2);
  }

  private string[] ConvertDecimalToBinary(int[] decimalRepresentation)
  {
    string[] binary = new string[this.eccw];
    for (int index = 0; index < this.eccw; ++index)
    {
      string str1 = Convert.ToString(decimalRepresentation[index], 2);
      string str2 = new string('0', 8 - str1.Length);
      binary[index] = str2 + str1;
    }
    return binary;
  }

  private int[] CalculatePolynomialDivision()
  {
    Dictionary<int, int> dictionary1 = new Dictionary<int, int>();
    for (int index = 0; index < this.decimalValue.Length; ++index)
      dictionary1.Add(this.decimalValue.Length - 1 - index, this.decimalValue[index]);
    Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
    for (int index = 0; index < this.gx.Length; ++index)
      dictionary2.Add(this.gx.Length - 1 - index, this.FindElement(this.gx[index], this.alpha));
    Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
    foreach (KeyValuePair<int, int> keyValuePair in dictionary1)
      dictionary3.Add(keyValuePair.Key + this.eccw, keyValuePair.Value);
    Dictionary<int, int> dictionary4 = dictionary3;
    int num = this.decimalValue.Length + this.eccw - this.gx.Length;
    Dictionary<int, int> dictionary5 = new Dictionary<int, int>();
    foreach (KeyValuePair<int, int> keyValuePair in dictionary2)
      dictionary5.Add(keyValuePair.Key + num, keyValuePair.Value);
    Dictionary<int, int> genPolynom = dictionary5;
    Dictionary<int, int> dictionary6 = dictionary4;
    for (int lowerExponentBy = 0; lowerExponentBy < dictionary4.Count; ++lowerExponentBy)
    {
      int largestExponent = this.FindLargestExponent(dictionary6);
      if (dictionary6[largestExponent] == 0)
      {
        dictionary6.Remove(largestExponent);
      }
      else
      {
        Dictionary<int, int> alphaNotation = this.ConvertToAlphaNotation(dictionary6);
        Dictionary<int, int> decimalNotation = this.ConvertToDecimalNotation(this.MultiplyGeneratorPolynomByLeadterm(genPolynom, alphaNotation[this.FindLargestExponent(alphaNotation)], lowerExponentBy));
        dictionary6 = this.XORPolynoms(dictionary6, decimalNotation);
      }
    }
    this.eccw = dictionary6.Count;
    List<int> intList = new List<int>();
    foreach (KeyValuePair<int, int> keyValuePair in dictionary6)
      intList.Add(keyValuePair.Value);
    return intList.ToArray();
  }

  private Dictionary<int, int> XORPolynoms(
    Dictionary<int, int> messagePolynom,
    Dictionary<int, int> resPolynom)
  {
    Dictionary<int, int> polynom1 = new Dictionary<int, int>();
    Dictionary<int, int> dictionary1 = new Dictionary<int, int>();
    Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
    Dictionary<int, int> dictionary3;
    Dictionary<int, int> polynom2;
    if (messagePolynom.Count >= resPolynom.Count)
    {
      dictionary3 = messagePolynom;
      polynom2 = resPolynom;
    }
    else
    {
      dictionary3 = resPolynom;
      polynom2 = messagePolynom;
    }
    int largestExponent1 = this.FindLargestExponent(messagePolynom);
    int largestExponent2 = this.FindLargestExponent(polynom2);
    int num = 0;
    foreach (KeyValuePair<int, int> keyValuePair in dictionary3)
    {
      polynom1.Add(largestExponent1 - num, keyValuePair.Value ^ (polynom2.Count > num ? polynom2[largestExponent2 - num] : 0));
      ++num;
    }
    int largestExponent3 = this.FindLargestExponent(polynom1);
    polynom1.Remove(largestExponent3);
    return polynom1;
  }

  private Dictionary<int, int> MultiplyGeneratorPolynomByLeadterm(
    Dictionary<int, int> genPolynom,
    int leadTermCoefficient,
    int lowerExponentBy)
  {
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    foreach (KeyValuePair<int, int> keyValuePair in genPolynom)
      dictionary.Add(keyValuePair.Key - lowerExponentBy, (keyValuePair.Value + leadTermCoefficient) % (int) byte.MaxValue);
    return dictionary;
  }

  private Dictionary<int, int> ConvertToDecimalNotation(Dictionary<int, int> poly)
  {
    Dictionary<int, int> decimalNotation = new Dictionary<int, int>();
    foreach (KeyValuePair<int, int> keyValuePair in poly)
      decimalNotation.Add(keyValuePair.Key, this.GetIntValFromAlphaExp(keyValuePair.Value, this.alpha));
    return decimalNotation;
  }

  private Dictionary<int, int> ConvertToAlphaNotation(Dictionary<int, int> polynom)
  {
    Dictionary<int, int> alphaNotation = new Dictionary<int, int>();
    foreach (KeyValuePair<int, int> keyValuePair in polynom)
    {
      if (keyValuePair.Value != 0)
        alphaNotation.Add(keyValuePair.Key, this.FindElement(keyValuePair.Value, this.alpha));
    }
    return alphaNotation;
  }

  private int FindLargestExponent(Dictionary<int, int> polynom)
  {
    int largestExponent = 0;
    foreach (KeyValuePair<int, int> keyValuePair in polynom)
    {
      if (keyValuePair.Key > largestExponent)
        largestExponent = keyValuePair.Key;
    }
    return largestExponent;
  }

  private int GetIntValFromAlphaExp(int element, int[] alpha)
  {
    if (element > (int) byte.MaxValue)
      element -= (int) byte.MaxValue;
    return alpha[element];
  }

  private int FindElement(int element, int[] alpha)
  {
    int element1 = 0;
    while (element1 < alpha.Length && element != alpha[element1])
      ++element1;
    return element1;
  }

  private int[] GetElement(int[] element, int[] alpha)
  {
    int[] element1 = new int[element.Length];
    for (int index = 0; index < element.Length; ++index)
    {
      if (element[index] > (int) byte.MaxValue)
        element[index] = element[index] - (int) byte.MaxValue;
      element1[index] = alpha[element[index]];
    }
    return element1;
  }
}
