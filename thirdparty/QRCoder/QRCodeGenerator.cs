// Decompiled with JetBrains decompiler
// Type: QRCoder.QRCodeGenerator
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using QRCoder.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace QRCoder;

public class QRCodeGenerator : IDisposable
{
  private static readonly char[] alphanumEncTable = new char[9]
  {
    ' ',
    '$',
    '%',
    '*',
    '+',
    '-',
    '.',
    '/',
    ':'
  };
  private static readonly int[] capacityBaseValues = new int[640]
  {
    41,
    25,
    17,
    10,
    34,
    20,
    14,
    8,
    27,
    16 /*0x10*/,
    11,
    7,
    17,
    10,
    7,
    4,
    77,
    47,
    32 /*0x20*/,
    20,
    63 /*0x3F*/,
    38,
    26,
    16 /*0x10*/,
    48 /*0x30*/,
    29,
    20,
    12,
    34,
    20,
    14,
    8,
    (int) sbyte.MaxValue,
    77,
    53,
    32 /*0x20*/,
    101,
    61,
    42,
    26,
    77,
    47,
    32 /*0x20*/,
    20,
    58,
    35,
    24,
    15,
    187,
    114,
    78,
    48 /*0x30*/,
    149,
    90,
    62,
    38,
    111,
    67,
    46,
    28,
    82,
    50,
    34,
    21,
    (int) byte.MaxValue,
    154,
    106,
    65,
    202,
    122,
    84,
    52,
    144 /*0x90*/,
    87,
    60,
    37,
    106,
    64 /*0x40*/,
    44,
    27,
    322,
    195,
    134,
    82,
    (int) byte.MaxValue,
    154,
    106,
    65,
    178,
    108,
    74,
    45,
    139,
    84,
    58,
    36,
    370,
    224 /*0xE0*/,
    154,
    95,
    293,
    178,
    122,
    75,
    207,
    125,
    86,
    53,
    154,
    93,
    64 /*0x40*/,
    39,
    461,
    279,
    192 /*0xC0*/,
    118,
    365,
    221,
    152,
    93,
    259,
    157,
    108,
    66,
    202,
    122,
    84,
    52,
    552,
    335,
    230,
    141,
    432,
    262,
    180,
    111,
    312,
    189,
    130,
    80 /*0x50*/,
    235,
    143,
    98,
    60,
    652,
    395,
    271,
    167,
    513,
    311,
    213,
    131,
    364,
    221,
    151,
    93,
    288,
    174,
    119,
    74,
    772,
    468,
    321,
    198,
    604,
    366,
    251,
    155,
    427,
    259,
    177,
    109,
    331,
    200,
    137,
    85,
    883,
    535,
    367,
    226,
    691,
    419,
    287,
    177,
    489,
    296,
    203,
    125,
    374,
    227,
    155,
    96 /*0x60*/,
    1022,
    619,
    425,
    262,
    796,
    483,
    331,
    204,
    580,
    352,
    241,
    149,
    427,
    259,
    177,
    109,
    1101,
    667,
    458,
    282,
    871,
    528,
    362,
    223,
    621,
    376,
    258,
    159,
    468,
    283,
    194,
    120,
    1250,
    758,
    520,
    320,
    991,
    600,
    412,
    254,
    703,
    426,
    292,
    180,
    530,
    321,
    220,
    136,
    1408,
    854,
    586,
    361,
    1082,
    656,
    450,
    277,
    775,
    470,
    322,
    198,
    602,
    365,
    250,
    154,
    1548,
    938,
    644,
    397,
    1212,
    734,
    504,
    310,
    876,
    531,
    364,
    224 /*0xE0*/,
    674,
    408,
    280,
    173,
    1725,
    1046,
    718,
    442,
    1346,
    816,
    560,
    345,
    948,
    574,
    394,
    243,
    746,
    452,
    310,
    191,
    1903,
    1153,
    792,
    488,
    1500,
    909,
    624,
    384,
    1063,
    644,
    442,
    272,
    813,
    493,
    338,
    208 /*0xD0*/,
    2061,
    1249,
    858,
    528,
    1600,
    970,
    666,
    410,
    1159,
    702,
    482,
    297,
    919,
    557,
    382,
    235,
    2232,
    1352,
    929,
    572,
    1708,
    1035,
    711,
    438,
    1224,
    742,
    509,
    314,
    969,
    587,
    403,
    248,
    2409,
    1460,
    1003,
    618,
    1872,
    1134,
    779,
    480,
    1358,
    823,
    565,
    348,
    1056,
    640,
    439,
    270,
    2620,
    1588,
    1091,
    672,
    2059,
    1248,
    857,
    528,
    1468,
    890,
    611,
    376,
    1108,
    672,
    461,
    284,
    2812,
    1704,
    1171,
    721,
    2188,
    1326,
    911,
    561,
    1588,
    963,
    661,
    407,
    1228,
    744,
    511 /*0x01FF*/,
    315,
    3057,
    1853,
    1273,
    784,
    2395,
    1451,
    997,
    614,
    1718,
    1041,
    715,
    440,
    1286,
    779,
    535,
    330,
    3283,
    1990,
    1367,
    842,
    2544,
    1542,
    1059,
    652,
    1804,
    1094,
    751,
    462,
    1425,
    864,
    593,
    365,
    3517,
    2132,
    1465,
    902,
    2701,
    1637,
    1125,
    692,
    1933,
    1172,
    805,
    496,
    1501,
    910,
    625,
    385,
    3669,
    2223,
    1528,
    940,
    2857,
    1732,
    1190,
    732,
    2085,
    1263,
    868,
    534,
    1581,
    958,
    658,
    405,
    3909,
    2369,
    1628,
    1002,
    3035,
    1839,
    1264,
    778,
    2181,
    1322,
    908,
    559,
    1677,
    1016,
    698,
    430,
    4158,
    2520,
    1732,
    1066,
    3289,
    1994,
    1370,
    843,
    2358,
    1429,
    982,
    604,
    1782,
    1080,
    742,
    457,
    4417,
    2677,
    1840,
    1132,
    3486,
    2113,
    1452,
    894,
    2473,
    1499,
    1030,
    634,
    1897,
    1150,
    790,
    486,
    4686,
    2840,
    1952,
    1201,
    3693,
    2238,
    1538,
    947,
    2670,
    1618,
    1112,
    684,
    2022,
    1226,
    842,
    518,
    4965,
    3009,
    2068,
    1273,
    3909,
    2369,
    1628,
    1002,
    2805,
    1700,
    1168,
    719,
    2157,
    1307,
    898,
    553,
    5253,
    3183,
    2188,
    1347,
    4134,
    2506,
    1722,
    1060,
    2949,
    1787,
    1228,
    756,
    2301,
    1394,
    958,
    590,
    5529,
    3351,
    2303 /*0x08FF*/,
    1417,
    4343,
    2632,
    1809,
    1113,
    3081,
    1867,
    1283,
    790,
    2361,
    1431,
    983,
    605,
    5836,
    3537,
    2431,
    1496,
    4588,
    2780,
    1911,
    1176,
    3244,
    1966,
    1351,
    832,
    2524,
    1530,
    1051,
    647,
    6153,
    3729,
    2563,
    1577,
    4775,
    2894,
    1989,
    1224,
    3417,
    2071,
    1423,
    876,
    2625,
    1591,
    1093,
    673,
    6479,
    3927,
    2699,
    1661,
    5039,
    3054,
    2099,
    1292,
    3599,
    2181,
    1499,
    923,
    2735,
    1658,
    1139,
    701,
    6743,
    4087,
    2809,
    1729,
    5313,
    3220,
    2213,
    1362,
    3791,
    2298,
    1579,
    972,
    2927,
    1774,
    1219,
    750,
    7089,
    4296,
    2953,
    1817,
    5596,
    3391,
    2331,
    1435,
    3993,
    2420,
    1663,
    1024 /*0x0400*/,
    3057,
    1852,
    1273,
    784
  };
  private static readonly int[] capacityECCBaseValues = new int[960]
  {
    19,
    7,
    1,
    19,
    0,
    0,
    16 /*0x10*/,
    10,
    1,
    16 /*0x10*/,
    0,
    0,
    13,
    13,
    1,
    13,
    0,
    0,
    9,
    17,
    1,
    9,
    0,
    0,
    34,
    10,
    1,
    34,
    0,
    0,
    28,
    16 /*0x10*/,
    1,
    28,
    0,
    0,
    22,
    22,
    1,
    22,
    0,
    0,
    16 /*0x10*/,
    28,
    1,
    16 /*0x10*/,
    0,
    0,
    55,
    15,
    1,
    55,
    0,
    0,
    44,
    26,
    1,
    44,
    0,
    0,
    34,
    18,
    2,
    17,
    0,
    0,
    26,
    22,
    2,
    13,
    0,
    0,
    80 /*0x50*/,
    20,
    1,
    80 /*0x50*/,
    0,
    0,
    64 /*0x40*/,
    18,
    2,
    32 /*0x20*/,
    0,
    0,
    48 /*0x30*/,
    26,
    2,
    24,
    0,
    0,
    36,
    16 /*0x10*/,
    4,
    9,
    0,
    0,
    108,
    26,
    1,
    108,
    0,
    0,
    86,
    24,
    2,
    43,
    0,
    0,
    62,
    18,
    2,
    15,
    2,
    16 /*0x10*/,
    46,
    22,
    2,
    11,
    2,
    12,
    136,
    18,
    2,
    68,
    0,
    0,
    108,
    16 /*0x10*/,
    4,
    27,
    0,
    0,
    76,
    24,
    4,
    19,
    0,
    0,
    60,
    28,
    4,
    15,
    0,
    0,
    156,
    20,
    2,
    78,
    0,
    0,
    124,
    18,
    4,
    31 /*0x1F*/,
    0,
    0,
    88,
    18,
    2,
    14,
    4,
    15,
    66,
    26,
    4,
    13,
    1,
    14,
    194,
    24,
    2,
    97,
    0,
    0,
    154,
    22,
    2,
    38,
    2,
    39,
    110,
    22,
    4,
    18,
    2,
    19,
    86,
    26,
    4,
    14,
    2,
    15,
    232,
    30,
    2,
    116,
    0,
    0,
    182,
    22,
    3,
    36,
    2,
    37,
    132,
    20,
    4,
    16 /*0x10*/,
    4,
    17,
    100,
    24,
    4,
    12,
    4,
    13,
    274,
    18,
    2,
    68,
    2,
    69,
    216,
    26,
    4,
    43,
    1,
    44,
    154,
    24,
    6,
    19,
    2,
    20,
    122,
    28,
    6,
    15,
    2,
    16 /*0x10*/,
    324,
    20,
    4,
    81,
    0,
    0,
    254,
    30,
    1,
    50,
    4,
    51,
    180,
    28,
    4,
    22,
    4,
    23,
    140,
    24,
    3,
    12,
    8,
    13,
    370,
    24,
    2,
    92,
    2,
    93,
    290,
    22,
    6,
    36,
    2,
    37,
    206,
    26,
    4,
    20,
    6,
    21,
    158,
    28,
    7,
    14,
    4,
    15,
    428,
    26,
    4,
    107,
    0,
    0,
    334,
    22,
    8,
    37,
    1,
    38,
    244,
    24,
    8,
    20,
    4,
    21,
    180,
    22,
    12,
    11,
    4,
    12,
    461,
    30,
    3,
    115,
    1,
    116,
    365,
    24,
    4,
    40,
    5,
    41,
    261,
    20,
    11,
    16 /*0x10*/,
    5,
    17,
    197,
    24,
    11,
    12,
    5,
    13,
    523,
    22,
    5,
    87,
    1,
    88,
    415,
    24,
    5,
    41,
    5,
    42,
    295,
    30,
    5,
    24,
    7,
    25,
    223,
    24,
    11,
    12,
    7,
    13,
    589,
    24,
    5,
    98,
    1,
    99,
    453,
    28,
    7,
    45,
    3,
    46,
    325,
    24,
    15,
    19,
    2,
    20,
    253,
    30,
    3,
    15,
    13,
    16 /*0x10*/,
    647,
    28,
    1,
    107,
    5,
    108,
    507,
    28,
    10,
    46,
    1,
    47,
    367,
    28,
    1,
    22,
    15,
    23,
    283,
    28,
    2,
    14,
    17,
    15,
    721,
    30,
    5,
    120,
    1,
    121,
    563,
    26,
    9,
    43,
    4,
    44,
    397,
    28,
    17,
    22,
    1,
    23,
    313,
    28,
    2,
    14,
    19,
    15,
    795,
    28,
    3,
    113,
    4,
    114,
    627,
    26,
    3,
    44,
    11,
    45,
    445,
    26,
    17,
    21,
    4,
    22,
    341,
    26,
    9,
    13,
    16 /*0x10*/,
    14,
    861,
    28,
    3,
    107,
    5,
    108,
    669,
    26,
    3,
    41,
    13,
    42,
    485,
    30,
    15,
    24,
    5,
    25,
    385,
    28,
    15,
    15,
    10,
    16 /*0x10*/,
    932,
    28,
    4,
    116,
    4,
    117,
    714,
    26,
    17,
    42,
    0,
    0,
    512 /*0x0200*/,
    28,
    17,
    22,
    6,
    23,
    406,
    30,
    19,
    16 /*0x10*/,
    6,
    17,
    1006,
    28,
    2,
    111,
    7,
    112 /*0x70*/,
    782,
    28,
    17,
    46,
    0,
    0,
    568,
    30,
    7,
    24,
    16 /*0x10*/,
    25,
    442,
    24,
    34,
    13,
    0,
    0,
    1094,
    30,
    4,
    121,
    5,
    122,
    860,
    28,
    4,
    47,
    14,
    48 /*0x30*/,
    614,
    30,
    11,
    24,
    14,
    25,
    464,
    30,
    16 /*0x10*/,
    15,
    14,
    16 /*0x10*/,
    1174,
    30,
    6,
    117,
    4,
    118,
    914,
    28,
    6,
    45,
    14,
    46,
    664,
    30,
    11,
    24,
    16 /*0x10*/,
    25,
    514,
    30,
    30,
    16 /*0x10*/,
    2,
    17,
    1276,
    26,
    8,
    106,
    4,
    107,
    1000,
    28,
    8,
    47,
    13,
    48 /*0x30*/,
    718,
    30,
    7,
    24,
    22,
    25,
    538,
    30,
    22,
    15,
    13,
    16 /*0x10*/,
    1370,
    28,
    10,
    114,
    2,
    115,
    1062,
    28,
    19,
    46,
    4,
    47,
    754,
    28,
    28,
    22,
    6,
    23,
    596,
    30,
    33,
    16 /*0x10*/,
    4,
    17,
    1468,
    30,
    8,
    122,
    4,
    123,
    1128,
    28,
    22,
    45,
    3,
    46,
    808,
    30,
    8,
    23,
    26,
    24,
    628,
    30,
    12,
    15,
    28,
    16 /*0x10*/,
    1531,
    30,
    3,
    117,
    10,
    118,
    1193,
    28,
    3,
    45,
    23,
    46,
    871,
    30,
    4,
    24,
    31 /*0x1F*/,
    25,
    661,
    30,
    11,
    15,
    31 /*0x1F*/,
    16 /*0x10*/,
    1631,
    30,
    7,
    116,
    7,
    117,
    1267,
    28,
    21,
    45,
    7,
    46,
    911,
    30,
    1,
    23,
    37,
    24,
    701,
    30,
    19,
    15,
    26,
    16 /*0x10*/,
    1735,
    30,
    5,
    115,
    10,
    116,
    1373,
    28,
    19,
    47,
    10,
    48 /*0x30*/,
    985,
    30,
    15,
    24,
    25,
    25,
    745,
    30,
    23,
    15,
    25,
    16 /*0x10*/,
    1843,
    30,
    13,
    115,
    3,
    116,
    1455,
    28,
    2,
    46,
    29,
    47,
    1033,
    30,
    42,
    24,
    1,
    25,
    793,
    30,
    23,
    15,
    28,
    16 /*0x10*/,
    1955,
    30,
    17,
    115,
    0,
    0,
    1541,
    28,
    10,
    46,
    23,
    47,
    1115,
    30,
    10,
    24,
    35,
    25,
    845,
    30,
    19,
    15,
    35,
    16 /*0x10*/,
    2071,
    30,
    17,
    115,
    1,
    116,
    1631,
    28,
    14,
    46,
    21,
    47,
    1171,
    30,
    29,
    24,
    19,
    25,
    901,
    30,
    11,
    15,
    46,
    16 /*0x10*/,
    2191,
    30,
    13,
    115,
    6,
    116,
    1725,
    28,
    14,
    46,
    23,
    47,
    1231,
    30,
    44,
    24,
    7,
    25,
    961,
    30,
    59,
    16 /*0x10*/,
    1,
    17,
    2306,
    30,
    12,
    121,
    7,
    122,
    1812,
    28,
    12,
    47,
    26,
    48 /*0x30*/,
    1286,
    30,
    39,
    24,
    14,
    25,
    986,
    30,
    22,
    15,
    41,
    16 /*0x10*/,
    2434,
    30,
    6,
    121,
    14,
    122,
    1914,
    28,
    6,
    47,
    34,
    48 /*0x30*/,
    1354,
    30,
    46,
    24,
    10,
    25,
    1054,
    30,
    2,
    15,
    64 /*0x40*/,
    16 /*0x10*/,
    2566,
    30,
    17,
    122,
    4,
    123,
    1992,
    28,
    29,
    46,
    14,
    47,
    1426,
    30,
    49,
    24,
    10,
    25,
    1096,
    30,
    24,
    15,
    46,
    16 /*0x10*/,
    2702,
    30,
    4,
    122,
    18,
    123,
    2102,
    28,
    13,
    46,
    32 /*0x20*/,
    47,
    1502,
    30,
    48 /*0x30*/,
    24,
    14,
    25,
    1142,
    30,
    42,
    15,
    32 /*0x20*/,
    16 /*0x10*/,
    2812,
    30,
    20,
    117,
    4,
    118,
    2216,
    28,
    40,
    47,
    7,
    48 /*0x30*/,
    1582,
    30,
    43,
    24,
    22,
    25,
    1222,
    30,
    10,
    15,
    67,
    16 /*0x10*/,
    2956,
    30,
    19,
    118,
    6,
    119,
    2334,
    28,
    18,
    47,
    31 /*0x1F*/,
    48 /*0x30*/,
    1666,
    30,
    34,
    24,
    34,
    25,
    1276,
    30,
    20,
    15,
    61,
    16 /*0x10*/
  };
  private static readonly int[] alignmentPatternBaseValues = new int[280]
  {
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    6,
    18,
    0,
    0,
    0,
    0,
    0,
    6,
    22,
    0,
    0,
    0,
    0,
    0,
    6,
    26,
    0,
    0,
    0,
    0,
    0,
    6,
    30,
    0,
    0,
    0,
    0,
    0,
    6,
    34,
    0,
    0,
    0,
    0,
    0,
    6,
    22,
    38,
    0,
    0,
    0,
    0,
    6,
    24,
    42,
    0,
    0,
    0,
    0,
    6,
    26,
    46,
    0,
    0,
    0,
    0,
    6,
    28,
    50,
    0,
    0,
    0,
    0,
    6,
    30,
    54,
    0,
    0,
    0,
    0,
    6,
    32 /*0x20*/,
    58,
    0,
    0,
    0,
    0,
    6,
    34,
    62,
    0,
    0,
    0,
    0,
    6,
    26,
    46,
    66,
    0,
    0,
    0,
    6,
    26,
    48 /*0x30*/,
    70,
    0,
    0,
    0,
    6,
    26,
    50,
    74,
    0,
    0,
    0,
    6,
    30,
    54,
    78,
    0,
    0,
    0,
    6,
    30,
    56,
    82,
    0,
    0,
    0,
    6,
    30,
    58,
    86,
    0,
    0,
    0,
    6,
    34,
    62,
    90,
    0,
    0,
    0,
    6,
    28,
    50,
    72,
    94,
    0,
    0,
    6,
    26,
    50,
    74,
    98,
    0,
    0,
    6,
    30,
    54,
    78,
    102,
    0,
    0,
    6,
    28,
    54,
    80 /*0x50*/,
    106,
    0,
    0,
    6,
    32 /*0x20*/,
    58,
    84,
    110,
    0,
    0,
    6,
    30,
    58,
    86,
    114,
    0,
    0,
    6,
    34,
    62,
    90,
    118,
    0,
    0,
    6,
    26,
    50,
    74,
    98,
    122,
    0,
    6,
    30,
    54,
    78,
    102,
    126,
    0,
    6,
    26,
    52,
    78,
    104,
    130,
    0,
    6,
    30,
    56,
    82,
    108,
    134,
    0,
    6,
    34,
    60,
    86,
    112 /*0x70*/,
    138,
    0,
    6,
    30,
    58,
    86,
    114,
    142,
    0,
    6,
    34,
    62,
    90,
    118,
    146,
    0,
    6,
    30,
    54,
    78,
    102,
    126,
    150,
    6,
    24,
    50,
    76,
    102,
    128 /*0x80*/,
    154,
    6,
    28,
    54,
    80 /*0x50*/,
    106,
    132,
    158,
    6,
    32 /*0x20*/,
    58,
    84,
    110,
    136,
    162,
    6,
    26,
    54,
    82,
    110,
    138,
    166,
    6,
    30,
    58,
    86,
    114,
    142,
    170
  };
  private static readonly int[] remainderBits = new int[40]
  {
    0,
    7,
    7,
    7,
    7,
    7,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    3,
    3,
    3,
    3,
    3,
    3,
    3,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    3,
    3,
    3,
    3,
    3,
    3,
    3,
    0,
    0,
    0,
    0,
    0,
    0
  };
  private static readonly List<QRCodeGenerator.AlignmentPattern> alignmentPatternTable = QRCodeGenerator.CreateAlignmentPatternTable();
  private static readonly List<QRCodeGenerator.ECCInfo> capacityECCTable = QRCodeGenerator.CreateCapacityECCTable();
  private static readonly List<QRCodeGenerator.VersionInfo> capacityTable = QRCodeGenerator.CreateCapacityTable();
  private static readonly List<QRCodeGenerator.Antilog> galoisField = QRCodeGenerator.CreateAntilogTable();
  private static readonly Dictionary<char, int> alphanumEncDict = QRCodeGenerator.CreateAlphanumEncDict();

  public QRCodeData CreateQrCode(PayloadGenerator.Payload payload)
  {
    return QRCodeGenerator.GenerateQrCode(payload);
  }

  public QRCodeData CreateQrCode(
    PayloadGenerator.Payload payload,
    QRCodeGenerator.ECCLevel eccLevel)
  {
    return QRCodeGenerator.GenerateQrCode(payload, eccLevel);
  }

  public QRCodeData CreateQrCode(
    string plainText,
    QRCodeGenerator.ECCLevel eccLevel,
    bool forceUtf8 = false,
    bool utf8BOM = false,
    QRCodeGenerator.EciMode eciMode = QRCodeGenerator.EciMode.Default,
    int requestedVersion = -1)
  {
    return QRCodeGenerator.GenerateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion);
  }

  public QRCodeData CreateQrCode(byte[] binaryData, QRCodeGenerator.ECCLevel eccLevel)
  {
    return QRCodeGenerator.GenerateQrCode(binaryData, eccLevel);
  }

  public static QRCodeData GenerateQrCode(PayloadGenerator.Payload payload)
  {
    return QRCodeGenerator.GenerateQrCode(payload.ToString(), payload.EccLevel, eciMode: payload.EciMode, requestedVersion: payload.Version);
  }

  public static QRCodeData GenerateQrCode(
    PayloadGenerator.Payload payload,
    QRCodeGenerator.ECCLevel eccLevel)
  {
    return QRCodeGenerator.GenerateQrCode(payload.ToString(), eccLevel, eciMode: payload.EciMode, requestedVersion: payload.Version);
  }

  public static QRCodeData GenerateQrCode(
    string plainText,
    QRCodeGenerator.ECCLevel eccLevel,
    bool forceUtf8 = false,
    bool utf8BOM = false,
    QRCodeGenerator.EciMode eciMode = QRCodeGenerator.EciMode.Default,
    int requestedVersion = -1)
  {
    QRCodeGenerator.EncodingMode encodingFromPlaintext = QRCodeGenerator.GetEncodingFromPlaintext(plainText, forceUtf8);
    string binary = QRCodeGenerator.PlainTextToBinary(plainText, encodingFromPlaintext, eciMode, utf8BOM, forceUtf8);
    int dataLength = QRCodeGenerator.GetDataLength(encodingFromPlaintext, plainText, binary, forceUtf8);
    int version = requestedVersion;
    if (version == -1)
      version = QRCodeGenerator.GetVersion(dataLength + (eciMode != QRCodeGenerator.EciMode.Default ? 2 : 0), encodingFromPlaintext, eccLevel);
    else if (QRCodeGenerator.GetVersion(dataLength + (eciMode != QRCodeGenerator.EciMode.Default ? 2 : 0), encodingFromPlaintext, eccLevel) > version)
    {
      int maxSizeByte = QRCodeGenerator.capacityTable[version - 1].Details.First<QRCodeGenerator.VersionInfoDetails>((Func<QRCodeGenerator.VersionInfoDetails, bool>) (x => x.ErrorCorrectionLevel == eccLevel)).CapacityDict[encodingFromPlaintext];
      throw new DataTooLongException(eccLevel.ToString(), encodingFromPlaintext.ToString(), version, maxSizeByte);
    }
    string str = string.Empty;
    if (eciMode != QRCodeGenerator.EciMode.Default)
      str = QRCodeGenerator.DecToBin(7, 4) + QRCodeGenerator.DecToBin((int) eciMode, 8);
    return QRCodeGenerator.GenerateQrCode(str + QRCodeGenerator.DecToBin((int) encodingFromPlaintext, 4) + QRCodeGenerator.DecToBin(dataLength, QRCodeGenerator.GetCountIndicatorLength(version, encodingFromPlaintext)) + binary, eccLevel, version);
  }

  public static QRCodeData GenerateQrCode(byte[] binaryData, QRCodeGenerator.ECCLevel eccLevel)
  {
    int version = QRCodeGenerator.GetVersion(binaryData.Length, QRCodeGenerator.EncodingMode.Byte, eccLevel);
    string bitString = QRCodeGenerator.DecToBin(4, 4) + QRCodeGenerator.DecToBin(binaryData.Length, QRCodeGenerator.GetCountIndicatorLength(version, QRCodeGenerator.EncodingMode.Byte));
    foreach (byte decNum in binaryData)
      bitString += QRCodeGenerator.DecToBin((int) decNum, 8);
    return QRCodeGenerator.GenerateQrCode(bitString, eccLevel, version);
  }

  private static QRCodeData GenerateQrCode(
    string bitString,
    QRCodeGenerator.ECCLevel eccLevel,
    int version)
  {
    QRCodeGenerator.ECCInfo eccInfo = QRCodeGenerator.capacityECCTable.Single<QRCodeGenerator.ECCInfo>((Func<QRCodeGenerator.ECCInfo, bool>) (x => x.Version == version && x.ErrorCorrectionLevel == eccLevel));
    int length = eccInfo.TotalDataCodewords * 8;
    int val1 = length - bitString.Length;
    if (val1 > 0)
      bitString += new string('0', Math.Min(val1, 4));
    if (bitString.Length % 8 != 0)
      bitString += new string('0', 8 - bitString.Length % 8);
    while (bitString.Length < length)
      bitString += "1110110000010001";
    if (bitString.Length > length)
      bitString = bitString.Substring(0, length);
    List<QRCodeGenerator.CodewordBlock> codewordBlockList = new List<QRCodeGenerator.CodewordBlock>(eccInfo.BlocksInGroup1 + eccInfo.BlocksInGroup2);
    for (int index = 0; index < eccInfo.BlocksInGroup1; ++index)
    {
      string bitString1 = bitString.Substring(index * eccInfo.CodewordsInGroup1 * 8, eccInfo.CodewordsInGroup1 * 8);
      List<string> bitBlockList = QRCodeGenerator.BinaryStringToBitBlockList(bitString1);
      List<int> decList1 = QRCodeGenerator.BinaryStringListToDecList(bitBlockList);
      List<string> eccWords = QRCodeGenerator.CalculateECCWords(bitString1, eccInfo);
      List<int> decList2 = QRCodeGenerator.BinaryStringListToDecList(eccWords);
      codewordBlockList.Add(new QRCodeGenerator.CodewordBlock(1, index + 1, bitString1, bitBlockList, eccWords, decList1, decList2));
    }
    bitString = bitString.Substring(eccInfo.BlocksInGroup1 * eccInfo.CodewordsInGroup1 * 8);
    for (int index = 0; index < eccInfo.BlocksInGroup2; ++index)
    {
      string bitString2 = bitString.Substring(index * eccInfo.CodewordsInGroup2 * 8, eccInfo.CodewordsInGroup2 * 8);
      List<string> bitBlockList = QRCodeGenerator.BinaryStringToBitBlockList(bitString2);
      List<int> decList3 = QRCodeGenerator.BinaryStringListToDecList(bitBlockList);
      List<string> eccWords = QRCodeGenerator.CalculateECCWords(bitString2, eccInfo);
      List<int> decList4 = QRCodeGenerator.BinaryStringListToDecList(eccWords);
      codewordBlockList.Add(new QRCodeGenerator.CodewordBlock(2, index + 1, bitString2, bitBlockList, eccWords, decList3, decList4));
    }
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); ++index)
    {
      foreach (QRCodeGenerator.CodewordBlock codewordBlock in codewordBlockList)
      {
        if (codewordBlock.CodeWords.Count > index)
          stringBuilder.Append(codewordBlock.CodeWords[index]);
      }
    }
    for (int index = 0; index < eccInfo.ECCPerBlock; ++index)
    {
      foreach (QRCodeGenerator.CodewordBlock codewordBlock in codewordBlockList)
      {
        if (codewordBlock.ECCWords.Count > index)
          stringBuilder.Append(codewordBlock.ECCWords[index]);
      }
    }
    stringBuilder.Append(new string('0', QRCodeGenerator.remainderBits[version - 1]));
    string data = stringBuilder.ToString();
    QRCodeData qrCode = new QRCodeData(version);
    List<QRCodeGenerator.Rectangle> blockedModules = new List<QRCodeGenerator.Rectangle>();
    QRCodeGenerator.ModulePlacer.PlaceFinderPatterns(ref qrCode, ref blockedModules);
    QRCodeGenerator.ModulePlacer.ReserveSeperatorAreas(qrCode.ModuleMatrix.Count, ref blockedModules);
    QRCodeGenerator.ModulePlacer.PlaceAlignmentPatterns(ref qrCode, QRCodeGenerator.alignmentPatternTable.Where<QRCodeGenerator.AlignmentPattern>((Func<QRCodeGenerator.AlignmentPattern, bool>) (x => x.Version == version)).Select<QRCodeGenerator.AlignmentPattern, List<QRCodeGenerator.Point>>((Func<QRCodeGenerator.AlignmentPattern, List<QRCodeGenerator.Point>>) (x => x.PatternPositions)).First<List<QRCodeGenerator.Point>>(), ref blockedModules);
    QRCodeGenerator.ModulePlacer.PlaceTimingPatterns(ref qrCode, ref blockedModules);
    QRCodeGenerator.ModulePlacer.PlaceDarkModule(ref qrCode, version, ref blockedModules);
    QRCodeGenerator.ModulePlacer.ReserveVersionAreas(qrCode.ModuleMatrix.Count, version, ref blockedModules);
    QRCodeGenerator.ModulePlacer.PlaceDataWords(ref qrCode, data, ref blockedModules);
    int maskVersion = QRCodeGenerator.ModulePlacer.MaskCode(ref qrCode, version, ref blockedModules, eccLevel);
    string formatString = QRCodeGenerator.GetFormatString(eccLevel, maskVersion);
    QRCodeGenerator.ModulePlacer.PlaceFormat(ref qrCode, formatString);
    if (version >= 7)
    {
      string versionString = QRCodeGenerator.GetVersionString(version);
      QRCodeGenerator.ModulePlacer.PlaceVersion(ref qrCode, versionString);
    }
    QRCodeGenerator.ModulePlacer.AddQuietZone(ref qrCode);
    return qrCode;
  }

  private static string GetFormatString(QRCodeGenerator.ECCLevel level, int maskVersion)
  {
    string str1 = "10100110111";
    string str2 = "101010000010010";
    string str3;
    switch (level)
    {
      case QRCodeGenerator.ECCLevel.L:
        str3 = "01";
        break;
      case QRCodeGenerator.ECCLevel.M:
        str3 = "00";
        break;
      case QRCodeGenerator.ECCLevel.Q:
        str3 = "11";
        break;
      default:
        str3 = "10";
        break;
    }
    string str4 = str3 + QRCodeGenerator.DecToBin(maskVersion, 3);
    string str5 = str4.PadRight(15, '0');
    char[] chArray1 = new char[1]{ '0' };
    string str6;
    int num;
    string str7;
    char[] chArray2;
    for (str6 = str5.TrimStart(chArray1); str6.Length > 10; str6 = str7.TrimStart(chArray2))
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      str1 = str1.PadRight(str6.Length, '0');
      for (int index = 0; index < str6.Length; ++index)
      {
        StringBuilder stringBuilder2 = stringBuilder1;
        num = Convert.ToInt32(str6[index]) ^ Convert.ToInt32(str1[index]);
        string str8 = num.ToString();
        stringBuilder2.Append(str8);
      }
      str7 = stringBuilder1.ToString();
      chArray2 = new char[1]{ '0' };
    }
    string str9 = str6.PadLeft(10, '0');
    string str10 = str4 + str9;
    StringBuilder stringBuilder3 = new StringBuilder();
    for (int index = 0; index < str10.Length; ++index)
    {
      StringBuilder stringBuilder4 = stringBuilder3;
      num = Convert.ToInt32(str10[index]) ^ Convert.ToInt32(str2[index]);
      string str11 = num.ToString();
      stringBuilder4.Append(str11);
    }
    return stringBuilder3.ToString();
  }

  private static string GetVersionString(int version)
  {
    string str1 = "1111100100101";
    string bin = QRCodeGenerator.DecToBin(version, 6);
    string str2 = bin.PadRight(18, '0');
    char[] chArray1 = new char[1]{ '0' };
    string str3;
    string str4;
    char[] chArray2;
    for (str3 = str2.TrimStart(chArray1); str3.Length > 12; str3 = str4.TrimStart(chArray2))
    {
      StringBuilder stringBuilder = new StringBuilder();
      str1 = str1.PadRight(str3.Length, '0');
      for (int index = 0; index < str3.Length; ++index)
        stringBuilder.Append((Convert.ToInt32(str3[index]) ^ Convert.ToInt32(str1[index])).ToString());
      str4 = stringBuilder.ToString();
      chArray2 = new char[1]{ '0' };
    }
    string str5 = str3.PadLeft(12, '0');
    return bin + str5;
  }

  private static List<string> CalculateECCWords(string bitString, QRCodeGenerator.ECCInfo eccInfo)
  {
    int eccPerBlock = eccInfo.ECCPerBlock;
    QRCodeGenerator.Polynom messagePolynom = QRCodeGenerator.CalculateMessagePolynom(bitString);
    QRCodeGenerator.Polynom generatorPolynom = QRCodeGenerator.CalculateGeneratorPolynom(eccPerBlock);
    QRCodeGenerator.PolynomItem polyItem;
    for (int index1 = 0; index1 < messagePolynom.PolyItems.Count; ++index1)
    {
      List<QRCodeGenerator.PolynomItem> polyItems = messagePolynom.PolyItems;
      int index2 = index1;
      polyItem = messagePolynom.PolyItems[index1];
      int coefficient = polyItem.Coefficient;
      polyItem = messagePolynom.PolyItems[index1];
      int exponent = polyItem.Exponent + eccPerBlock;
      QRCodeGenerator.PolynomItem polynomItem = new QRCodeGenerator.PolynomItem(coefficient, exponent);
      polyItems[index2] = polynomItem;
    }
    for (int index3 = 0; index3 < generatorPolynom.PolyItems.Count; ++index3)
    {
      List<QRCodeGenerator.PolynomItem> polyItems = generatorPolynom.PolyItems;
      int index4 = index3;
      polyItem = generatorPolynom.PolyItems[index3];
      int coefficient = polyItem.Coefficient;
      polyItem = generatorPolynom.PolyItems[index3];
      int exponent = polyItem.Exponent + (messagePolynom.PolyItems.Count - 1);
      QRCodeGenerator.PolynomItem polynomItem = new QRCodeGenerator.PolynomItem(coefficient, exponent);
      polyItems[index4] = polynomItem;
    }
    QRCodeGenerator.Polynom polynom = messagePolynom;
    int lowerExponentBy = 0;
    while (polynom.PolyItems.Count > 0)
    {
      polyItem = polynom.PolyItems[polynom.PolyItems.Count - 1];
      if (polyItem.Exponent > 0)
      {
        polyItem = polynom.PolyItems[0];
        if (polyItem.Coefficient == 0)
        {
          polynom.PolyItems.RemoveAt(0);
          List<QRCodeGenerator.PolynomItem> polyItems = polynom.PolyItems;
          polyItem = polynom.PolyItems[polynom.PolyItems.Count - 1];
          QRCodeGenerator.PolynomItem polynomItem = new QRCodeGenerator.PolynomItem(0, polyItem.Exponent - 1);
          polyItems.Add(polynomItem);
        }
        else
        {
          QRCodeGenerator.Polynom decNotation = QRCodeGenerator.ConvertToDecNotation(QRCodeGenerator.MultiplyGeneratorPolynomByLeadterm(generatorPolynom, QRCodeGenerator.ConvertToAlphaNotation(polynom).PolyItems[0], lowerExponentBy));
          polynom = QRCodeGenerator.XORPolynoms(polynom, decNotation);
        }
        ++lowerExponentBy;
      }
      else
        break;
    }
    return polynom.PolyItems.Select<QRCodeGenerator.PolynomItem, string>((Func<QRCodeGenerator.PolynomItem, string>) (x => QRCodeGenerator.DecToBin(x.Coefficient, 8))).ToList<string>();
  }

  private static QRCodeGenerator.Polynom ConvertToAlphaNotation(QRCodeGenerator.Polynom poly)
  {
    QRCodeGenerator.Polynom alphaNotation = new QRCodeGenerator.Polynom();
    for (int index = 0; index < poly.PolyItems.Count; ++index)
    {
      List<QRCodeGenerator.PolynomItem> polyItems = alphaNotation.PolyItems;
      QRCodeGenerator.PolynomItem polyItem = poly.PolyItems[index];
      int coefficient;
      if (polyItem.Coefficient == 0)
      {
        coefficient = 0;
      }
      else
      {
        polyItem = poly.PolyItems[index];
        coefficient = QRCodeGenerator.GetAlphaExpFromIntVal(polyItem.Coefficient);
      }
      polyItem = poly.PolyItems[index];
      int exponent = polyItem.Exponent;
      QRCodeGenerator.PolynomItem polynomItem = new QRCodeGenerator.PolynomItem(coefficient, exponent);
      polyItems.Add(polynomItem);
    }
    return alphaNotation;
  }

  private static QRCodeGenerator.Polynom ConvertToDecNotation(QRCodeGenerator.Polynom poly)
  {
    QRCodeGenerator.Polynom decNotation = new QRCodeGenerator.Polynom();
    for (int index = 0; index < poly.PolyItems.Count; ++index)
      decNotation.PolyItems.Add(new QRCodeGenerator.PolynomItem(QRCodeGenerator.GetIntValFromAlphaExp(poly.PolyItems[index].Coefficient), poly.PolyItems[index].Exponent));
    return decNotation;
  }

  private static int GetVersion(
    int length,
    QRCodeGenerator.EncodingMode encMode,
    QRCodeGenerator.ECCLevel eccLevel)
  {
    IEnumerable<\u003C\u003Ef__AnonymousType0<int, int>> source = QRCodeGenerator.capacityTable.Where<QRCodeGenerator.VersionInfo>((Func<QRCodeGenerator.VersionInfo, bool>) (x => x.Details.Any<QRCodeGenerator.VersionInfoDetails>((Func<QRCodeGenerator.VersionInfoDetails, bool>) (y => y.ErrorCorrectionLevel == eccLevel && y.CapacityDict[encMode] >= Convert.ToInt32(length))))).Select(x => new
    {
      version = x.Version,
      capacity = x.Details.Single<QRCodeGenerator.VersionInfoDetails>((Func<QRCodeGenerator.VersionInfoDetails, bool>) (y => y.ErrorCorrectionLevel == eccLevel)).CapacityDict[encMode]
    });
    if (source.Any())
      return source.Min(x => x.version);
    int maxSizeByte = QRCodeGenerator.capacityTable.Where<QRCodeGenerator.VersionInfo>((Func<QRCodeGenerator.VersionInfo, bool>) (x => x.Details.Any<QRCodeGenerator.VersionInfoDetails>((Func<QRCodeGenerator.VersionInfoDetails, bool>) (y => y.ErrorCorrectionLevel == eccLevel)))).Max<QRCodeGenerator.VersionInfo>((Func<QRCodeGenerator.VersionInfo, int>) (x => x.Details.Single<QRCodeGenerator.VersionInfoDetails>((Func<QRCodeGenerator.VersionInfoDetails, bool>) (y => y.ErrorCorrectionLevel == eccLevel)).CapacityDict[encMode]));
    throw new DataTooLongException(eccLevel.ToString(), encMode.ToString(), maxSizeByte);
  }

  private static QRCodeGenerator.EncodingMode GetEncodingFromPlaintext(
    string plainText,
    bool forceUtf8)
  {
    if (forceUtf8)
      return QRCodeGenerator.EncodingMode.Byte;
    QRCodeGenerator.EncodingMode encodingFromPlaintext = QRCodeGenerator.EncodingMode.Numeric;
    foreach (char c in plainText)
    {
      if (!QRCodeGenerator.IsInRange(c, '0', '9'))
      {
        encodingFromPlaintext = QRCodeGenerator.EncodingMode.Alphanumeric;
        if (!QRCodeGenerator.IsInRange(c, 'A', 'Z') && !((IEnumerable<char>) QRCodeGenerator.alphanumEncTable).Contains<char>(c))
          return QRCodeGenerator.EncodingMode.Byte;
      }
    }
    return encodingFromPlaintext;
  }

  private static bool IsInRange(char c, char min, char max)
  {
    return (uint) c - (uint) min <= (uint) max - (uint) min;
  }

  private static QRCodeGenerator.Polynom CalculateMessagePolynom(string bitString)
  {
    QRCodeGenerator.Polynom messagePolynom = new QRCodeGenerator.Polynom();
    for (int exponent = bitString.Length / 8 - 1; exponent >= 0; --exponent)
    {
      messagePolynom.PolyItems.Add(new QRCodeGenerator.PolynomItem(QRCodeGenerator.BinToDec(bitString.Substring(0, 8)), exponent));
      bitString = bitString.Remove(0, 8);
    }
    return messagePolynom;
  }

  private static QRCodeGenerator.Polynom CalculateGeneratorPolynom(int numEccWords)
  {
    QRCodeGenerator.Polynom polynomBase = new QRCodeGenerator.Polynom();
    polynomBase.PolyItems.AddRange((IEnumerable<QRCodeGenerator.PolynomItem>) new QRCodeGenerator.PolynomItem[2]
    {
      new QRCodeGenerator.PolynomItem(0, 1),
      new QRCodeGenerator.PolynomItem(0, 0)
    });
    for (int coefficient = 1; coefficient <= numEccWords - 1; ++coefficient)
    {
      QRCodeGenerator.Polynom polynomMultiplier = new QRCodeGenerator.Polynom();
      polynomMultiplier.PolyItems.AddRange((IEnumerable<QRCodeGenerator.PolynomItem>) new QRCodeGenerator.PolynomItem[2]
      {
        new QRCodeGenerator.PolynomItem(0, 1),
        new QRCodeGenerator.PolynomItem(coefficient, 0)
      });
      polynomBase = QRCodeGenerator.MultiplyAlphaPolynoms(polynomBase, polynomMultiplier);
    }
    return polynomBase;
  }

  private static List<string> BinaryStringToBitBlockList(string bitString)
  {
    List<string> bitBlockList = new List<string>((int) Math.Ceiling((double) bitString.Length / 8.0));
    for (int startIndex = 0; startIndex < bitString.Length; startIndex += 8)
      bitBlockList.Add(bitString.Substring(startIndex, 8));
    return bitBlockList;
  }

  private static List<int> BinaryStringListToDecList(List<string> binaryStringList)
  {
    return binaryStringList.Select<string, int>((Func<string, int>) (binaryString => QRCodeGenerator.BinToDec(binaryString))).ToList<int>();
  }

  private static int BinToDec(string binStr) => Convert.ToInt32(binStr, 2);

  private static string DecToBin(int decNum) => Convert.ToString(decNum, 2);

  private static string DecToBin(int decNum, int padLeftUpTo)
  {
    return QRCodeGenerator.DecToBin(decNum).PadLeft(padLeftUpTo, '0');
  }

  private static int GetCountIndicatorLength(int version, QRCodeGenerator.EncodingMode encMode)
  {
    if (version < 10)
    {
      if (encMode == QRCodeGenerator.EncodingMode.Numeric)
        return 10;
      return encMode == QRCodeGenerator.EncodingMode.Alphanumeric ? 9 : 8;
    }
    if (version < 27)
    {
      switch (encMode)
      {
        case QRCodeGenerator.EncodingMode.Numeric:
          return 12;
        case QRCodeGenerator.EncodingMode.Alphanumeric:
          return 11;
        case QRCodeGenerator.EncodingMode.Byte:
          return 16 /*0x10*/;
        default:
          return 10;
      }
    }
    else
    {
      switch (encMode)
      {
        case QRCodeGenerator.EncodingMode.Numeric:
          return 14;
        case QRCodeGenerator.EncodingMode.Alphanumeric:
          return 13;
        case QRCodeGenerator.EncodingMode.Byte:
          return 16 /*0x10*/;
        default:
          return 12;
      }
    }
  }

  private static int GetDataLength(
    QRCodeGenerator.EncodingMode encoding,
    string plainText,
    string codedText,
    bool forceUtf8)
  {
    return !forceUtf8 && !QRCodeGenerator.IsUtf8(encoding, plainText, forceUtf8) ? plainText.Length : codedText.Length / 8;
  }

  private static bool IsUtf8(
    QRCodeGenerator.EncodingMode encoding,
    string plainText,
    bool forceUtf8)
  {
    return encoding == QRCodeGenerator.EncodingMode.Byte && !QRCodeGenerator.IsValidISO(plainText) | forceUtf8;
  }

  private static bool IsValidISO(string input)
  {
    byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
    string b = Encoding.GetEncoding("ISO-8859-1").GetString(bytes, 0, bytes.Length);
    return string.Equals(input, b);
  }

  private static string PlainTextToBinary(
    string plainText,
    QRCodeGenerator.EncodingMode encMode,
    QRCodeGenerator.EciMode eciMode,
    bool utf8BOM,
    bool forceUtf8)
  {
    switch (encMode)
    {
      case QRCodeGenerator.EncodingMode.Numeric:
        return QRCodeGenerator.PlainTextToBinaryNumeric(plainText);
      case QRCodeGenerator.EncodingMode.Alphanumeric:
        return QRCodeGenerator.PlainTextToBinaryAlphanumeric(plainText);
      case QRCodeGenerator.EncodingMode.Byte:
        return QRCodeGenerator.PlainTextToBinaryByte(plainText, eciMode, utf8BOM, forceUtf8);
      case QRCodeGenerator.EncodingMode.Kanji:
        return string.Empty;
      default:
        return string.Empty;
    }
  }

  private static string PlainTextToBinaryNumeric(string plainText)
  {
    string empty = string.Empty;
    for (; plainText.Length >= 3; plainText = plainText.Substring(3))
    {
      int int32 = Convert.ToInt32(plainText.Substring(0, 3));
      empty += QRCodeGenerator.DecToBin(int32, 10);
    }
    if (plainText.Length == 2)
    {
      int int32 = Convert.ToInt32(plainText);
      empty += QRCodeGenerator.DecToBin(int32, 7);
    }
    else if (plainText.Length == 1)
    {
      int int32 = Convert.ToInt32(plainText);
      empty += QRCodeGenerator.DecToBin(int32, 4);
    }
    return empty;
  }

  private static string PlainTextToBinaryAlphanumeric(string plainText)
  {
    string empty = string.Empty;
    for (; plainText.Length >= 2; plainText = plainText.Substring(2))
    {
      string str = plainText.Substring(0, 2);
      int decNum = QRCodeGenerator.alphanumEncDict[str[0]] * 45 + QRCodeGenerator.alphanumEncDict[str[1]];
      empty += QRCodeGenerator.DecToBin(decNum, 11);
    }
    if (plainText.Length > 0)
      empty += QRCodeGenerator.DecToBin(QRCodeGenerator.alphanumEncDict[plainText[0]], 6);
    return empty;
  }

  private string PlainTextToBinaryECI(string plainText)
  {
    string empty = string.Empty;
    foreach (byte decNum in Encoding.GetEncoding("ascii").GetBytes(plainText))
      empty += QRCodeGenerator.DecToBin((int) decNum, 8);
    return empty;
  }

  private static string ConvertToIso8859(string value, string Iso = "ISO-8859-2")
  {
    Encoding encoding = Encoding.GetEncoding(Iso);
    Encoding utF8 = Encoding.UTF8;
    byte[] bytes1 = utF8.GetBytes(value);
    byte[] bytes2 = Encoding.Convert(utF8, encoding, bytes1);
    return encoding.GetString(bytes2);
  }

  private static string PlainTextToBinaryByte(
    string plainText,
    QRCodeGenerator.EciMode eciMode,
    bool utf8BOM,
    bool forceUtf8)
  {
    string empty = string.Empty;
    byte[] numArray;
    if (QRCodeGenerator.IsValidISO(plainText) && !forceUtf8)
    {
      numArray = Encoding.GetEncoding("ISO-8859-1").GetBytes(plainText);
    }
    else
    {
      switch (eciMode)
      {
        case QRCodeGenerator.EciMode.Iso8859_1:
          numArray = Encoding.GetEncoding("ISO-8859-1").GetBytes(QRCodeGenerator.ConvertToIso8859(plainText, "ISO-8859-1"));
          break;
        case QRCodeGenerator.EciMode.Iso8859_2:
          numArray = Encoding.GetEncoding("ISO-8859-2").GetBytes(QRCodeGenerator.ConvertToIso8859(plainText));
          break;
        default:
          numArray = utf8BOM ? ((IEnumerable<byte>) Encoding.UTF8.GetPreamble()).Concat<byte>((IEnumerable<byte>) Encoding.UTF8.GetBytes(plainText)).ToArray<byte>() : Encoding.UTF8.GetBytes(plainText);
          break;
      }
    }
    foreach (byte decNum in numArray)
      empty += QRCodeGenerator.DecToBin((int) decNum, 8);
    return empty;
  }

  private static QRCodeGenerator.Polynom XORPolynoms(
    QRCodeGenerator.Polynom messagePolynom,
    QRCodeGenerator.Polynom resPolynom)
  {
    QRCodeGenerator.Polynom polynom1 = new QRCodeGenerator.Polynom();
    QRCodeGenerator.Polynom polynom2;
    QRCodeGenerator.Polynom polynom3;
    if (messagePolynom.PolyItems.Count >= resPolynom.PolyItems.Count)
    {
      polynom2 = messagePolynom;
      polynom3 = resPolynom;
    }
    else
    {
      polynom2 = resPolynom;
      polynom3 = messagePolynom;
    }
    for (int index = 0; index < polynom2.PolyItems.Count; ++index)
    {
      QRCodeGenerator.PolynomItem polynomItem;
      ref QRCodeGenerator.PolynomItem local = ref polynomItem;
      QRCodeGenerator.PolynomItem polyItem = polynom2.PolyItems[index];
      int coefficient1 = polyItem.Coefficient;
      int num;
      if (polynom3.PolyItems.Count <= index)
      {
        num = 0;
      }
      else
      {
        polyItem = polynom3.PolyItems[index];
        num = polyItem.Coefficient;
      }
      int coefficient2 = coefficient1 ^ num;
      polyItem = messagePolynom.PolyItems[0];
      int exponent = polyItem.Exponent - index;
      local = new QRCodeGenerator.PolynomItem(coefficient2, exponent);
      polynom1.PolyItems.Add(polynomItem);
    }
    polynom1.PolyItems.RemoveAt(0);
    return polynom1;
  }

  private static QRCodeGenerator.Polynom MultiplyGeneratorPolynomByLeadterm(
    QRCodeGenerator.Polynom genPolynom,
    QRCodeGenerator.PolynomItem leadTerm,
    int lowerExponentBy)
  {
    QRCodeGenerator.Polynom polynom = new QRCodeGenerator.Polynom();
    foreach (QRCodeGenerator.PolynomItem polyItem in genPolynom.PolyItems)
    {
      QRCodeGenerator.PolynomItem polynomItem = new QRCodeGenerator.PolynomItem((polyItem.Coefficient + leadTerm.Coefficient) % (int) byte.MaxValue, polyItem.Exponent - lowerExponentBy);
      polynom.PolyItems.Add(polynomItem);
    }
    return polynom;
  }

  private static QRCodeGenerator.Polynom MultiplyAlphaPolynoms(
    QRCodeGenerator.Polynom polynomBase,
    QRCodeGenerator.Polynom polynomMultiplier)
  {
    QRCodeGenerator.Polynom polynom = new QRCodeGenerator.Polynom();
    foreach (QRCodeGenerator.PolynomItem polyItem1 in polynomMultiplier.PolyItems)
    {
      foreach (QRCodeGenerator.PolynomItem polyItem2 in polynomBase.PolyItems)
      {
        QRCodeGenerator.PolynomItem polynomItem = new QRCodeGenerator.PolynomItem(QRCodeGenerator.ShrinkAlphaExp(polyItem1.Coefficient + polyItem2.Coefficient), polyItem1.Exponent + polyItem2.Exponent);
        polynom.PolyItems.Add(polynomItem);
      }
    }
    IEnumerable<int> source = polynom.PolyItems.GroupBy<QRCodeGenerator.PolynomItem, int>((Func<QRCodeGenerator.PolynomItem, int>) (x => x.Exponent)).Where<IGrouping<int, QRCodeGenerator.PolynomItem>>((Func<IGrouping<int, QRCodeGenerator.PolynomItem>, bool>) (x => x.Count<QRCodeGenerator.PolynomItem>() > 1)).Select<IGrouping<int, QRCodeGenerator.PolynomItem>, int>((Func<IGrouping<int, QRCodeGenerator.PolynomItem>, int>) (x => x.First<QRCodeGenerator.PolynomItem>().Exponent));
    if (!(source is IList<int> intList))
      intList = (IList<int>) source.ToList<int>();
    IList<int> toGlue = intList;
    List<QRCodeGenerator.PolynomItem> collection = new List<QRCodeGenerator.PolynomItem>(toGlue.Count);
    foreach (int num in (IEnumerable<int>) toGlue)
    {
      int exponent = num;
      QRCodeGenerator.PolynomItem polynomItem = new QRCodeGenerator.PolynomItem(QRCodeGenerator.GetAlphaExpFromIntVal(polynom.PolyItems.Where<QRCodeGenerator.PolynomItem>((Func<QRCodeGenerator.PolynomItem, bool>) (x => x.Exponent == exponent)).Aggregate<QRCodeGenerator.PolynomItem, int>(0, (Func<int, QRCodeGenerator.PolynomItem, int>) ((current, polynomOld) => current ^ QRCodeGenerator.GetIntValFromAlphaExp(polynomOld.Coefficient)))), exponent);
      collection.Add(polynomItem);
    }
    polynom.PolyItems.RemoveAll((Predicate<QRCodeGenerator.PolynomItem>) (x => toGlue.Contains(x.Exponent)));
    polynom.PolyItems.AddRange((IEnumerable<QRCodeGenerator.PolynomItem>) collection);
    polynom.PolyItems.Sort((Comparison<QRCodeGenerator.PolynomItem>) ((x, y) => -x.Exponent.CompareTo(y.Exponent)));
    return polynom;
  }

  private static int GetIntValFromAlphaExp(int exp)
  {
    return QRCodeGenerator.galoisField.Find((Predicate<QRCodeGenerator.Antilog>) (alog => alog.ExponentAlpha == exp)).IntegerValue;
  }

  private static int GetAlphaExpFromIntVal(int intVal)
  {
    return QRCodeGenerator.galoisField.Find((Predicate<QRCodeGenerator.Antilog>) (alog => alog.IntegerValue == intVal)).ExponentAlpha;
  }

  private static int ShrinkAlphaExp(int alphaExp)
  {
    return (int) ((double) (alphaExp % 256 /*0x0100*/) + Math.Floor((double) (alphaExp / 256 /*0x0100*/)));
  }

  private static Dictionary<char, int> CreateAlphanumEncDict()
  {
    Dictionary<char, int> source = new Dictionary<char, int>(45);
    for (int index = 0; index < 10; ++index)
      source.Add($"{index}"[0], index);
    for (char key = 'A'; key <= 'Z'; ++key)
      source.Add(key, source.Count<KeyValuePair<char, int>>());
    for (int index = 0; index < QRCodeGenerator.alphanumEncTable.Length; ++index)
      source.Add(QRCodeGenerator.alphanumEncTable[index], source.Count<KeyValuePair<char, int>>());
    return source;
  }

  private static List<QRCodeGenerator.AlignmentPattern> CreateAlignmentPatternTable()
  {
    List<QRCodeGenerator.AlignmentPattern> alignmentPatternTable = new List<QRCodeGenerator.AlignmentPattern>(40);
    for (int index1 = 0; index1 < 280; index1 += 7)
    {
      List<QRCodeGenerator.Point> pointList = new List<QRCodeGenerator.Point>();
      for (int index2 = 0; index2 < 7; ++index2)
      {
        if (QRCodeGenerator.alignmentPatternBaseValues[index1 + index2] != 0)
        {
          for (int index3 = 0; index3 < 7; ++index3)
          {
            if (QRCodeGenerator.alignmentPatternBaseValues[index1 + index3] != 0)
            {
              QRCodeGenerator.Point point = new QRCodeGenerator.Point(QRCodeGenerator.alignmentPatternBaseValues[index1 + index2] - 2, QRCodeGenerator.alignmentPatternBaseValues[index1 + index3] - 2);
              if (!pointList.Contains(point))
                pointList.Add(point);
            }
          }
        }
      }
      alignmentPatternTable.Add(new QRCodeGenerator.AlignmentPattern()
      {
        Version = (index1 + 7) / 7,
        PatternPositions = pointList
      });
    }
    return alignmentPatternTable;
  }

  private static List<QRCodeGenerator.ECCInfo> CreateCapacityECCTable()
  {
    List<QRCodeGenerator.ECCInfo> capacityEccTable = new List<QRCodeGenerator.ECCInfo>(160 /*0xA0*/);
    for (int index = 0; index < 960; index += 24)
      capacityEccTable.AddRange((IEnumerable<QRCodeGenerator.ECCInfo>) new QRCodeGenerator.ECCInfo[4]
      {
        new QRCodeGenerator.ECCInfo((index + 24) / 24, QRCodeGenerator.ECCLevel.L, QRCodeGenerator.capacityECCBaseValues[index], QRCodeGenerator.capacityECCBaseValues[index + 1], QRCodeGenerator.capacityECCBaseValues[index + 2], QRCodeGenerator.capacityECCBaseValues[index + 3], QRCodeGenerator.capacityECCBaseValues[index + 4], QRCodeGenerator.capacityECCBaseValues[index + 5]),
        new QRCodeGenerator.ECCInfo((index + 24) / 24, QRCodeGenerator.ECCLevel.M, QRCodeGenerator.capacityECCBaseValues[index + 6], QRCodeGenerator.capacityECCBaseValues[index + 7], QRCodeGenerator.capacityECCBaseValues[index + 8], QRCodeGenerator.capacityECCBaseValues[index + 9], QRCodeGenerator.capacityECCBaseValues[index + 10], QRCodeGenerator.capacityECCBaseValues[index + 11]),
        new QRCodeGenerator.ECCInfo((index + 24) / 24, QRCodeGenerator.ECCLevel.Q, QRCodeGenerator.capacityECCBaseValues[index + 12], QRCodeGenerator.capacityECCBaseValues[index + 13], QRCodeGenerator.capacityECCBaseValues[index + 14], QRCodeGenerator.capacityECCBaseValues[index + 15], QRCodeGenerator.capacityECCBaseValues[index + 16 /*0x10*/], QRCodeGenerator.capacityECCBaseValues[index + 17]),
        new QRCodeGenerator.ECCInfo((index + 24) / 24, QRCodeGenerator.ECCLevel.H, QRCodeGenerator.capacityECCBaseValues[index + 18], QRCodeGenerator.capacityECCBaseValues[index + 19], QRCodeGenerator.capacityECCBaseValues[index + 20], QRCodeGenerator.capacityECCBaseValues[index + 21], QRCodeGenerator.capacityECCBaseValues[index + 22], QRCodeGenerator.capacityECCBaseValues[index + 23])
      });
    return capacityEccTable;
  }

  private static List<QRCodeGenerator.VersionInfo> CreateCapacityTable()
  {
    List<QRCodeGenerator.VersionInfo> capacityTable = new List<QRCodeGenerator.VersionInfo>(40);
    for (int index = 0; index < 640; index += 16 /*0x10*/)
      capacityTable.Add(new QRCodeGenerator.VersionInfo((index + 16 /*0x10*/) / 16 /*0x10*/, new List<QRCodeGenerator.VersionInfoDetails>(4)
      {
        new QRCodeGenerator.VersionInfoDetails(QRCodeGenerator.ECCLevel.L, new Dictionary<QRCodeGenerator.EncodingMode, int>()
        {
          {
            QRCodeGenerator.EncodingMode.Numeric,
            QRCodeGenerator.capacityBaseValues[index]
          },
          {
            QRCodeGenerator.EncodingMode.Alphanumeric,
            QRCodeGenerator.capacityBaseValues[index + 1]
          },
          {
            QRCodeGenerator.EncodingMode.Byte,
            QRCodeGenerator.capacityBaseValues[index + 2]
          },
          {
            QRCodeGenerator.EncodingMode.Kanji,
            QRCodeGenerator.capacityBaseValues[index + 3]
          }
        }),
        new QRCodeGenerator.VersionInfoDetails(QRCodeGenerator.ECCLevel.M, new Dictionary<QRCodeGenerator.EncodingMode, int>()
        {
          {
            QRCodeGenerator.EncodingMode.Numeric,
            QRCodeGenerator.capacityBaseValues[index + 4]
          },
          {
            QRCodeGenerator.EncodingMode.Alphanumeric,
            QRCodeGenerator.capacityBaseValues[index + 5]
          },
          {
            QRCodeGenerator.EncodingMode.Byte,
            QRCodeGenerator.capacityBaseValues[index + 6]
          },
          {
            QRCodeGenerator.EncodingMode.Kanji,
            QRCodeGenerator.capacityBaseValues[index + 7]
          }
        }),
        new QRCodeGenerator.VersionInfoDetails(QRCodeGenerator.ECCLevel.Q, new Dictionary<QRCodeGenerator.EncodingMode, int>()
        {
          {
            QRCodeGenerator.EncodingMode.Numeric,
            QRCodeGenerator.capacityBaseValues[index + 8]
          },
          {
            QRCodeGenerator.EncodingMode.Alphanumeric,
            QRCodeGenerator.capacityBaseValues[index + 9]
          },
          {
            QRCodeGenerator.EncodingMode.Byte,
            QRCodeGenerator.capacityBaseValues[index + 10]
          },
          {
            QRCodeGenerator.EncodingMode.Kanji,
            QRCodeGenerator.capacityBaseValues[index + 11]
          }
        }),
        new QRCodeGenerator.VersionInfoDetails(QRCodeGenerator.ECCLevel.H, new Dictionary<QRCodeGenerator.EncodingMode, int>()
        {
          {
            QRCodeGenerator.EncodingMode.Numeric,
            QRCodeGenerator.capacityBaseValues[index + 12]
          },
          {
            QRCodeGenerator.EncodingMode.Alphanumeric,
            QRCodeGenerator.capacityBaseValues[index + 13]
          },
          {
            QRCodeGenerator.EncodingMode.Byte,
            QRCodeGenerator.capacityBaseValues[index + 14]
          },
          {
            QRCodeGenerator.EncodingMode.Kanji,
            QRCodeGenerator.capacityBaseValues[index + 15]
          }
        })
      }));
    return capacityTable;
  }

  private static List<QRCodeGenerator.Antilog> CreateAntilogTable()
  {
    List<QRCodeGenerator.Antilog> antilogTable = new List<QRCodeGenerator.Antilog>(256 /*0x0100*/);
    int integerValue = 1;
    for (int exponentAlpha = 0; exponentAlpha < 256 /*0x0100*/; ++exponentAlpha)
    {
      antilogTable.Add(new QRCodeGenerator.Antilog(exponentAlpha, integerValue));
      integerValue *= 2;
      if (integerValue > (int) byte.MaxValue)
        integerValue ^= 285;
    }
    return antilogTable;
  }

  public void Dispose()
  {
  }

  public enum EciMode
  {
    Default = 0,
    Iso8859_1 = 3,
    Iso8859_2 = 4,
    Utf8 = 26, // 0x0000001A
  }

  private static class ModulePlacer
  {
    public static void AddQuietZone(ref QRCodeData qrCode)
    {
      bool[] values = new bool[qrCode.ModuleMatrix.Count + 8];
      for (int index = 0; index < values.Length; ++index)
        values[index] = false;
      for (int index = 0; index < 4; ++index)
        qrCode.ModuleMatrix.Insert(0, new BitArray(values));
      for (int index = 0; index < 4; ++index)
        qrCode.ModuleMatrix.Add(new BitArray(values));
      for (int index = 4; index < qrCode.ModuleMatrix.Count - 4; ++index)
      {
        bool[] collection = new bool[4];
        List<bool> boolList = new List<bool>((IEnumerable<bool>) collection);
        boolList.AddRange(qrCode.ModuleMatrix[index].Cast<bool>());
        boolList.AddRange((IEnumerable<bool>) collection);
        qrCode.ModuleMatrix[index] = new BitArray(boolList.ToArray());
      }
    }

    private static string ReverseString(string inp)
    {
      string empty = string.Empty;
      if (inp.Length > 0)
      {
        for (int index = inp.Length - 1; index >= 0; --index)
          empty += inp[index].ToString();
      }
      return empty;
    }

    public static void PlaceVersion(ref QRCodeData qrCode, string versionStr)
    {
      int count = qrCode.ModuleMatrix.Count;
      string str = QRCodeGenerator.ModulePlacer.ReverseString(versionStr);
      for (int index1 = 0; index1 < 6; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          qrCode.ModuleMatrix[index2 + count - 11][index1] = str[index1 * 3 + index2] == '1';
          qrCode.ModuleMatrix[index1][index2 + count - 11] = str[index1 * 3 + index2] == '1';
        }
      }
    }

    public static void PlaceFormat(ref QRCodeData qrCode, string formatStr)
    {
      int count = qrCode.ModuleMatrix.Count;
      string str = QRCodeGenerator.ModulePlacer.ReverseString(formatStr);
      int[,] numArray = new int[15, 4]
      {
        {
          8,
          0,
          count - 1,
          8
        },
        {
          8,
          1,
          count - 2,
          8
        },
        {
          8,
          2,
          count - 3,
          8
        },
        {
          8,
          3,
          count - 4,
          8
        },
        {
          8,
          4,
          count - 5,
          8
        },
        {
          8,
          5,
          count - 6,
          8
        },
        {
          8,
          7,
          count - 7,
          8
        },
        {
          8,
          8,
          count - 8,
          8
        },
        {
          7,
          8,
          8,
          count - 7
        },
        {
          5,
          8,
          8,
          count - 6
        },
        {
          4,
          8,
          8,
          count - 5
        },
        {
          3,
          8,
          8,
          count - 4
        },
        {
          2,
          8,
          8,
          count - 3
        },
        {
          1,
          8,
          8,
          count - 2
        },
        {
          0,
          8,
          8,
          count - 1
        }
      };
      for (int index = 0; index < 15; ++index)
      {
        QRCodeGenerator.Point point1 = new QRCodeGenerator.Point(numArray[index, 0], numArray[index, 1]);
        QRCodeGenerator.Point point2 = new QRCodeGenerator.Point(numArray[index, 2], numArray[index, 3]);
        qrCode.ModuleMatrix[point1.Y][point1.X] = str[index] == '1';
        qrCode.ModuleMatrix[point2.Y][point2.X] = str[index] == '1';
      }
    }

    public static int MaskCode(
      ref QRCodeData qrCode,
      int version,
      ref List<QRCodeGenerator.Rectangle> blockedModules,
      QRCodeGenerator.ECCLevel eccLevel)
    {
      int? nullable = new int?();
      int num1 = 0;
      int count = qrCode.ModuleMatrix.Count;
      Dictionary<int, Func<int, int, bool>> dictionary = new Dictionary<int, Func<int, int, bool>>(8)
      {
        {
          1,
          new Func<int, int, bool>(QRCodeGenerator.ModulePlacer.MaskPattern.Pattern1)
        },
        {
          2,
          new Func<int, int, bool>(QRCodeGenerator.ModulePlacer.MaskPattern.Pattern2)
        },
        {
          3,
          new Func<int, int, bool>(QRCodeGenerator.ModulePlacer.MaskPattern.Pattern3)
        },
        {
          4,
          new Func<int, int, bool>(QRCodeGenerator.ModulePlacer.MaskPattern.Pattern4)
        },
        {
          5,
          new Func<int, int, bool>(QRCodeGenerator.ModulePlacer.MaskPattern.Pattern5)
        },
        {
          6,
          new Func<int, int, bool>(QRCodeGenerator.ModulePlacer.MaskPattern.Pattern6)
        },
        {
          7,
          new Func<int, int, bool>(QRCodeGenerator.ModulePlacer.MaskPattern.Pattern7)
        },
        {
          8,
          new Func<int, int, bool>(QRCodeGenerator.ModulePlacer.MaskPattern.Pattern8)
        }
      };
      foreach (KeyValuePair<int, Func<int, int, bool>> keyValuePair in dictionary)
      {
        QRCodeData qrCode1 = new QRCodeData(version);
        for (int index1 = 0; index1 < count; ++index1)
        {
          for (int index2 = 0; index2 < count; ++index2)
            qrCode1.ModuleMatrix[index1][index2] = qrCode.ModuleMatrix[index1][index2];
        }
        string formatString = QRCodeGenerator.GetFormatString(eccLevel, keyValuePair.Key - 1);
        QRCodeGenerator.ModulePlacer.PlaceFormat(ref qrCode1, formatString);
        if (version >= 7)
        {
          string versionString = QRCodeGenerator.GetVersionString(version);
          QRCodeGenerator.ModulePlacer.PlaceVersion(ref qrCode1, versionString);
        }
        for (int index3 = 0; index3 < count; ++index3)
        {
          for (int index4 = 0; index4 < index3; ++index4)
          {
            if (!QRCodeGenerator.ModulePlacer.IsBlocked(new QRCodeGenerator.Rectangle(index3, index4, 1, 1), blockedModules))
            {
              qrCode1.ModuleMatrix[index4][index3] ^= keyValuePair.Value(index3, index4);
              qrCode1.ModuleMatrix[index3][index4] ^= keyValuePair.Value(index4, index3);
            }
          }
          if (!QRCodeGenerator.ModulePlacer.IsBlocked(new QRCodeGenerator.Rectangle(index3, index3, 1, 1), blockedModules))
            qrCode1.ModuleMatrix[index3][index3] ^= keyValuePair.Value(index3, index3);
        }
        int num2 = QRCodeGenerator.ModulePlacer.MaskPattern.Score(ref qrCode1);
        if (!nullable.HasValue || num1 > num2)
        {
          nullable = new int?(keyValuePair.Key);
          num1 = num2;
        }
      }
      for (int index5 = 0; index5 < count; ++index5)
      {
        for (int index6 = 0; index6 < index5; ++index6)
        {
          if (!QRCodeGenerator.ModulePlacer.IsBlocked(new QRCodeGenerator.Rectangle(index5, index6, 1, 1), blockedModules))
          {
            qrCode.ModuleMatrix[index6][index5] ^= dictionary[nullable.Value](index5, index6);
            qrCode.ModuleMatrix[index5][index6] ^= dictionary[nullable.Value](index6, index5);
          }
        }
        if (!QRCodeGenerator.ModulePlacer.IsBlocked(new QRCodeGenerator.Rectangle(index5, index5, 1, 1), blockedModules))
          qrCode.ModuleMatrix[index5][index5] ^= dictionary[nullable.Value](index5, index5);
      }
      return nullable.Value - 1;
    }

    public static void PlaceDataWords(
      ref QRCodeData qrCode,
      string data,
      ref List<QRCodeGenerator.Rectangle> blockedModules)
    {
      int count = qrCode.ModuleMatrix.Count;
      bool flag = true;
      Queue<bool> boolQueue = new Queue<bool>();
      for (int index = 0; index < data.Length; ++index)
        boolQueue.Enqueue(data[index] != '0');
      for (int index1 = count - 1; index1 >= 0; index1 -= 2)
      {
        if (index1 == 6)
          index1 = 5;
        for (int index2 = 1; index2 <= count; ++index2)
        {
          if (flag)
          {
            int num = count - index2;
            if (boolQueue.Count > 0 && !QRCodeGenerator.ModulePlacer.IsBlocked(new QRCodeGenerator.Rectangle(index1, num, 1, 1), blockedModules))
              qrCode.ModuleMatrix[num][index1] = boolQueue.Dequeue();
            if (boolQueue.Count > 0 && index1 > 0 && !QRCodeGenerator.ModulePlacer.IsBlocked(new QRCodeGenerator.Rectangle(index1 - 1, num, 1, 1), blockedModules))
              qrCode.ModuleMatrix[num][index1 - 1] = boolQueue.Dequeue();
          }
          else
          {
            int num = index2 - 1;
            if (boolQueue.Count > 0 && !QRCodeGenerator.ModulePlacer.IsBlocked(new QRCodeGenerator.Rectangle(index1, num, 1, 1), blockedModules))
              qrCode.ModuleMatrix[num][index1] = boolQueue.Dequeue();
            if (boolQueue.Count > 0 && index1 > 0 && !QRCodeGenerator.ModulePlacer.IsBlocked(new QRCodeGenerator.Rectangle(index1 - 1, num, 1, 1), blockedModules))
              qrCode.ModuleMatrix[num][index1 - 1] = boolQueue.Dequeue();
          }
        }
        flag = !flag;
      }
    }

    public static void ReserveSeperatorAreas(
      int size,
      ref List<QRCodeGenerator.Rectangle> blockedModules)
    {
      blockedModules.AddRange((IEnumerable<QRCodeGenerator.Rectangle>) new QRCodeGenerator.Rectangle[6]
      {
        new QRCodeGenerator.Rectangle(7, 0, 1, 8),
        new QRCodeGenerator.Rectangle(0, 7, 7, 1),
        new QRCodeGenerator.Rectangle(0, size - 8, 8, 1),
        new QRCodeGenerator.Rectangle(7, size - 7, 1, 7),
        new QRCodeGenerator.Rectangle(size - 8, 0, 1, 8),
        new QRCodeGenerator.Rectangle(size - 7, 7, 7, 1)
      });
    }

    public static void ReserveVersionAreas(
      int size,
      int version,
      ref List<QRCodeGenerator.Rectangle> blockedModules)
    {
      blockedModules.AddRange((IEnumerable<QRCodeGenerator.Rectangle>) new QRCodeGenerator.Rectangle[6]
      {
        new QRCodeGenerator.Rectangle(8, 0, 1, 6),
        new QRCodeGenerator.Rectangle(8, 7, 1, 1),
        new QRCodeGenerator.Rectangle(0, 8, 6, 1),
        new QRCodeGenerator.Rectangle(7, 8, 2, 1),
        new QRCodeGenerator.Rectangle(size - 8, 8, 8, 1),
        new QRCodeGenerator.Rectangle(8, size - 7, 1, 7)
      });
      if (version < 7)
        return;
      blockedModules.AddRange((IEnumerable<QRCodeGenerator.Rectangle>) new QRCodeGenerator.Rectangle[2]
      {
        new QRCodeGenerator.Rectangle(size - 11, 0, 3, 6),
        new QRCodeGenerator.Rectangle(0, size - 11, 6, 3)
      });
    }

    public static void PlaceDarkModule(
      ref QRCodeData qrCode,
      int version,
      ref List<QRCodeGenerator.Rectangle> blockedModules)
    {
      qrCode.ModuleMatrix[4 * version + 9][8] = true;
      blockedModules.Add(new QRCodeGenerator.Rectangle(8, 4 * version + 9, 1, 1));
    }

    public static void PlaceFinderPatterns(
      ref QRCodeData qrCode,
      ref List<QRCodeGenerator.Rectangle> blockedModules)
    {
      int count = qrCode.ModuleMatrix.Count;
      int[] numArray = new int[6]
      {
        0,
        0,
        count - 7,
        0,
        0,
        count - 7
      };
      for (int index1 = 0; index1 < 6; index1 += 2)
      {
        for (int index2 = 0; index2 < 7; ++index2)
        {
          for (int index3 = 0; index3 < 7; ++index3)
          {
            if ((index2 != 1 && index2 != 5 || index3 <= 0 || index3 >= 6) && (index2 <= 0 || index2 >= 6 || index3 != 1 && index3 != 5))
              qrCode.ModuleMatrix[index3 + numArray[index1 + 1]][index2 + numArray[index1]] = true;
          }
        }
        blockedModules.Add(new QRCodeGenerator.Rectangle(numArray[index1], numArray[index1 + 1], 7, 7));
      }
    }

    public static void PlaceAlignmentPatterns(
      ref QRCodeData qrCode,
      List<QRCodeGenerator.Point> alignmentPatternLocations,
      ref List<QRCodeGenerator.Rectangle> blockedModules)
    {
      foreach (QRCodeGenerator.Point alignmentPatternLocation in alignmentPatternLocations)
      {
        QRCodeGenerator.Rectangle r1 = new QRCodeGenerator.Rectangle(alignmentPatternLocation.X, alignmentPatternLocation.Y, 5, 5);
        bool flag = false;
        foreach (QRCodeGenerator.Rectangle r2 in blockedModules)
        {
          if (QRCodeGenerator.ModulePlacer.Intersects(r1, r2))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          for (int index1 = 0; index1 < 5; ++index1)
          {
            for (int index2 = 0; index2 < 5; ++index2)
            {
              if (index2 != 0 && index2 != 4)
              {
                switch (index1)
                {
                  case 0:
                  case 4:
                    break;
                  case 2:
                    if (index2 != 2)
                      continue;
                    break;
                  default:
                    continue;
                }
              }
              qrCode.ModuleMatrix[alignmentPatternLocation.Y + index2][alignmentPatternLocation.X + index1] = true;
            }
          }
          blockedModules.Add(new QRCodeGenerator.Rectangle(alignmentPatternLocation.X, alignmentPatternLocation.Y, 5, 5));
        }
      }
    }

    public static void PlaceTimingPatterns(
      ref QRCodeData qrCode,
      ref List<QRCodeGenerator.Rectangle> blockedModules)
    {
      int count = qrCode.ModuleMatrix.Count;
      for (int index = 8; index < count - 8; ++index)
      {
        if (index % 2 == 0)
        {
          qrCode.ModuleMatrix[6][index] = true;
          qrCode.ModuleMatrix[index][6] = true;
        }
      }
      blockedModules.AddRange((IEnumerable<QRCodeGenerator.Rectangle>) new QRCodeGenerator.Rectangle[2]
      {
        new QRCodeGenerator.Rectangle(6, 8, 1, count - 16 /*0x10*/),
        new QRCodeGenerator.Rectangle(8, 6, count - 16 /*0x10*/, 1)
      });
    }

    private static bool Intersects(QRCodeGenerator.Rectangle r1, QRCodeGenerator.Rectangle r2)
    {
      return r2.X < r1.X + r1.Width && r1.X < r2.X + r2.Width && r2.Y < r1.Y + r1.Height && r1.Y < r2.Y + r2.Height;
    }

    private static bool IsBlocked(
      QRCodeGenerator.Rectangle r1,
      List<QRCodeGenerator.Rectangle> blockedModules)
    {
      foreach (QRCodeGenerator.Rectangle blockedModule in blockedModules)
      {
        if (QRCodeGenerator.ModulePlacer.Intersects(blockedModule, r1))
          return true;
      }
      return false;
    }

    private static class MaskPattern
    {
      public static bool Pattern1(int x, int y) => (x + y) % 2 == 0;

      public static bool Pattern2(int x, int y) => y % 2 == 0;

      public static bool Pattern3(int x, int y) => x % 3 == 0;

      public static bool Pattern4(int x, int y) => (x + y) % 3 == 0;

      public static bool Pattern5(int x, int y)
      {
        return (int) (Math.Floor((double) y / 2.0) + Math.Floor((double) x / 3.0)) % 2 == 0;
      }

      public static bool Pattern6(int x, int y) => x * y % 2 + x * y % 3 == 0;

      public static bool Pattern7(int x, int y) => (x * y % 2 + x * y % 3) % 2 == 0;

      public static bool Pattern8(int x, int y) => ((x + y) % 2 + x * y % 3) % 2 == 0;

      public static int Score(ref QRCodeData qrCode)
      {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int count = qrCode.ModuleMatrix.Count;
        for (int index1 = 0; index1 < count; ++index1)
        {
          int num4 = 0;
          int num5 = 0;
          bool flag1 = qrCode.ModuleMatrix[index1][0];
          bool flag2 = qrCode.ModuleMatrix[0][index1];
          for (int index2 = 0; index2 < count; ++index2)
          {
            if (qrCode.ModuleMatrix[index1][index2] == flag1)
              ++num4;
            else
              num4 = 1;
            if (num4 == 5)
              num1 += 3;
            else if (num4 > 5)
              ++num1;
            flag1 = qrCode.ModuleMatrix[index1][index2];
            if (qrCode.ModuleMatrix[index2][index1] == flag2)
              ++num5;
            else
              num5 = 1;
            if (num5 == 5)
              num1 += 3;
            else if (num5 > 5)
              ++num1;
            flag2 = qrCode.ModuleMatrix[index2][index1];
          }
        }
        for (int index3 = 0; index3 < count - 1; ++index3)
        {
          for (int index4 = 0; index4 < count - 1; ++index4)
          {
            if (qrCode.ModuleMatrix[index3][index4] == qrCode.ModuleMatrix[index3][index4 + 1] && qrCode.ModuleMatrix[index3][index4] == qrCode.ModuleMatrix[index3 + 1][index4] && qrCode.ModuleMatrix[index3][index4] == qrCode.ModuleMatrix[index3 + 1][index4 + 1])
              num2 += 3;
          }
        }
        for (int index5 = 0; index5 < count; ++index5)
        {
          for (int index6 = 0; index6 < count - 10; ++index6)
          {
            if (qrCode.ModuleMatrix[index5][index6] && !qrCode.ModuleMatrix[index5][index6 + 1] && qrCode.ModuleMatrix[index5][index6 + 2] && qrCode.ModuleMatrix[index5][index6 + 3] && qrCode.ModuleMatrix[index5][index6 + 4] && !qrCode.ModuleMatrix[index5][index6 + 5] && qrCode.ModuleMatrix[index5][index6 + 6] && !qrCode.ModuleMatrix[index5][index6 + 7] && !qrCode.ModuleMatrix[index5][index6 + 8] && !qrCode.ModuleMatrix[index5][index6 + 9] && !qrCode.ModuleMatrix[index5][index6 + 10] || !qrCode.ModuleMatrix[index5][index6] && !qrCode.ModuleMatrix[index5][index6 + 1] && !qrCode.ModuleMatrix[index5][index6 + 2] && !qrCode.ModuleMatrix[index5][index6 + 3] && qrCode.ModuleMatrix[index5][index6 + 4] && !qrCode.ModuleMatrix[index5][index6 + 5] && qrCode.ModuleMatrix[index5][index6 + 6] && qrCode.ModuleMatrix[index5][index6 + 7] && qrCode.ModuleMatrix[index5][index6 + 8] && !qrCode.ModuleMatrix[index5][index6 + 9] && qrCode.ModuleMatrix[index5][index6 + 10])
              num3 += 40;
            if (qrCode.ModuleMatrix[index6][index5] && !qrCode.ModuleMatrix[index6 + 1][index5] && qrCode.ModuleMatrix[index6 + 2][index5] && qrCode.ModuleMatrix[index6 + 3][index5] && qrCode.ModuleMatrix[index6 + 4][index5] && !qrCode.ModuleMatrix[index6 + 5][index5] && qrCode.ModuleMatrix[index6 + 6][index5] && !qrCode.ModuleMatrix[index6 + 7][index5] && !qrCode.ModuleMatrix[index6 + 8][index5] && !qrCode.ModuleMatrix[index6 + 9][index5] && !qrCode.ModuleMatrix[index6 + 10][index5] || !qrCode.ModuleMatrix[index6][index5] && !qrCode.ModuleMatrix[index6 + 1][index5] && !qrCode.ModuleMatrix[index6 + 2][index5] && !qrCode.ModuleMatrix[index6 + 3][index5] && qrCode.ModuleMatrix[index6 + 4][index5] && !qrCode.ModuleMatrix[index6 + 5][index5] && qrCode.ModuleMatrix[index6 + 6][index5] && qrCode.ModuleMatrix[index6 + 7][index5] && qrCode.ModuleMatrix[index6 + 8][index5] && !qrCode.ModuleMatrix[index6 + 9][index5] && qrCode.ModuleMatrix[index6 + 10][index5])
              num3 += 40;
          }
        }
        double num6 = 0.0;
        foreach (BitArray bitArray in qrCode.ModuleMatrix)
        {
          foreach (bool flag in bitArray)
          {
            if (flag)
              ++num6;
          }
        }
        double num7 = num6 / (double) (qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count) * 100.0;
        int num8 = Math.Min(Math.Abs((int) Math.Floor(num7 / 5.0) * 5 - 50) / 5, Math.Abs((int) Math.Floor(num7 / 5.0) * 5 - 45) / 5) * 10;
        return num1 + num2 + num3 + num8;
      }
    }
  }

  public enum ECCLevel
  {
    L,
    M,
    Q,
    H,
  }

  private enum EncodingMode
  {
    Numeric = 1,
    Alphanumeric = 2,
    Byte = 4,
    ECI = 7,
    Kanji = 8,
  }

  private struct AlignmentPattern
  {
    public int Version;
    public List<QRCodeGenerator.Point> PatternPositions;
  }

  private struct CodewordBlock(
    int groupNumber,
    int blockNumber,
    string bitString,
    List<string> codeWords,
    List<string> eccWords,
    List<int> codeWordsInt,
    List<int> eccWordsInt)
  {
    public int GroupNumber { get; } = groupNumber;

    public int BlockNumber { get; } = blockNumber;

    public string BitString { get; } = bitString;

    public List<string> CodeWords { get; } = codeWords;

    public List<int> CodeWordsInt { get; } = codeWordsInt;

    public List<string> ECCWords { get; } = eccWords;

    public List<int> ECCWordsInt { get; } = eccWordsInt;
  }

  private struct ECCInfo(
    int version,
    QRCodeGenerator.ECCLevel errorCorrectionLevel,
    int totalDataCodewords,
    int eccPerBlock,
    int blocksInGroup1,
    int codewordsInGroup1,
    int blocksInGroup2,
    int codewordsInGroup2)
  {
    public int Version { get; } = version;

    public QRCodeGenerator.ECCLevel ErrorCorrectionLevel { get; } = errorCorrectionLevel;

    public int TotalDataCodewords { get; } = totalDataCodewords;

    public int ECCPerBlock { get; } = eccPerBlock;

    public int BlocksInGroup1 { get; } = blocksInGroup1;

    public int CodewordsInGroup1 { get; } = codewordsInGroup1;

    public int BlocksInGroup2 { get; } = blocksInGroup2;

    public int CodewordsInGroup2 { get; } = codewordsInGroup2;
  }

  private struct VersionInfo(
    int version,
    List<QRCodeGenerator.VersionInfoDetails> versionInfoDetails)
  {
    public int Version { get; } = version;

    public List<QRCodeGenerator.VersionInfoDetails> Details { get; } = versionInfoDetails;
  }

  private struct VersionInfoDetails(
    QRCodeGenerator.ECCLevel errorCorrectionLevel,
    Dictionary<QRCodeGenerator.EncodingMode, int> capacityDict)
  {
    public QRCodeGenerator.ECCLevel ErrorCorrectionLevel { get; } = errorCorrectionLevel;

    public Dictionary<QRCodeGenerator.EncodingMode, int> CapacityDict { get; } = capacityDict;
  }

  private struct Antilog(int exponentAlpha, int integerValue)
  {
    public int ExponentAlpha { get; } = exponentAlpha;

    public int IntegerValue { get; } = integerValue;
  }

  private struct PolynomItem(int coefficient, int exponent)
  {
    public int Coefficient { get; } = coefficient;

    public int Exponent { get; } = exponent;
  }

  private class Polynom
  {
    public Polynom() => this.PolyItems = new List<QRCodeGenerator.PolynomItem>();

    public List<QRCodeGenerator.PolynomItem> PolyItems { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (QRCodeGenerator.PolynomItem polyItem in this.PolyItems)
        stringBuilder.Append($"a^{polyItem.Coefficient.ToString()}*x^{polyItem.Exponent.ToString()} + ");
      return stringBuilder.ToString().TrimEnd(' ', '+');
    }
  }

  private class Point
  {
    public int X { get; }

    public int Y { get; }

    public Point(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }
  }

  private class Rectangle
  {
    public int X { get; }

    public int Y { get; }

    public int Width { get; }

    public int Height { get; }

    public Rectangle(int x, int y, int w, int h)
    {
      this.X = x;
      this.Y = y;
      this.Width = w;
      this.Height = h;
    }
  }
}
