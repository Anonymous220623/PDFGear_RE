// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImageStructure
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class ImageStructure
{
  private byte[] m_twoDimention = new byte[128 /*0x80*/]
  {
    (byte) 80 /*0x50*/,
    (byte) 88,
    (byte) 23,
    (byte) 71,
    (byte) 30,
    (byte) 30,
    (byte) 62,
    (byte) 62,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 11,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 35,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 51,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41,
    (byte) 41
  };
  private int[] m_BlackandWhitePixels = new int[16 /*0x10*/]
  {
    28679,
    28679,
    31752,
    32777,
    33801,
    34825,
    35849,
    36873,
    29703,
    29703,
    30727,
    30727,
    37897,
    38921,
    39945,
    40969
  };
  private int[] m_whitePixel = new int[1024 /*0x0400*/]
  {
    6430,
    6400,
    6400,
    6400,
    3225,
    3225,
    3225,
    3225,
    944,
    944,
    944,
    944,
    976,
    976,
    976,
    976,
    1456,
    1456,
    1456,
    1456,
    1488,
    1488,
    1488,
    1488,
    718,
    718,
    718,
    718,
    718,
    718,
    718,
    718,
    750,
    750,
    750,
    750,
    750,
    750,
    750,
    750,
    1520,
    1520,
    1520,
    1520,
    1552,
    1552,
    1552,
    1552,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    428,
    654,
    654,
    654,
    654,
    654,
    654,
    654,
    654,
    1072,
    1072,
    1072,
    1072,
    1104,
    1104,
    1104,
    1104,
    1136,
    1136,
    1136,
    1136,
    1168,
    1168,
    1168,
    1168,
    1200,
    1200,
    1200,
    1200,
    1232,
    1232,
    1232,
    1232,
    622,
    622,
    622,
    622,
    622,
    622,
    622,
    622,
    1008,
    1008,
    1008,
    1008,
    1040,
    1040,
    1040,
    1040,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    44,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    396,
    1712,
    1712,
    1712,
    1712,
    1744,
    1744,
    1744,
    1744,
    846,
    846,
    846,
    846,
    846,
    846,
    846,
    846,
    1264,
    1264,
    1264,
    1264,
    1296,
    1296,
    1296,
    1296,
    1328,
    1328,
    1328,
    1328,
    1360,
    1360,
    1360,
    1360,
    1392,
    1392,
    1392,
    1392,
    1424,
    1424,
    1424,
    1424,
    686,
    686,
    686,
    686,
    686,
    686,
    686,
    686,
    910,
    910,
    910,
    910,
    910,
    910,
    910,
    910,
    1968,
    1968,
    1968,
    1968,
    2000,
    2000,
    2000,
    2000,
    2032,
    2032,
    2032,
    2032,
    16 /*0x10*/,
    16 /*0x10*/,
    16 /*0x10*/,
    16 /*0x10*/,
    10257,
    10257,
    10257,
    10257,
    12305,
    12305,
    12305,
    12305,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    330,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    362,
    878,
    878,
    878,
    878,
    878,
    878,
    878,
    878,
    1904,
    1904,
    1904,
    1904,
    1936,
    1936,
    1936,
    1936,
    -18413,
    -18413,
    -16365,
    -16365,
    -14317,
    -14317,
    -10221,
    -10221,
    590,
    590,
    590,
    590,
    590,
    590,
    590,
    590,
    782,
    782,
    782,
    782,
    782,
    782,
    782,
    782,
    1584,
    1584,
    1584,
    1584,
    1616,
    1616,
    1616,
    1616,
    1648,
    1648,
    1648,
    1648,
    1680,
    1680,
    1680,
    1680,
    814,
    814,
    814,
    814,
    814,
    814,
    814,
    814,
    1776,
    1776,
    1776,
    1776,
    1808,
    1808,
    1808,
    1808,
    1840,
    1840,
    1840,
    1840,
    1872,
    1872,
    1872,
    1872,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    6157,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    -12275,
    14353,
    14353,
    14353,
    14353,
    16401,
    16401,
    16401,
    16401,
    22547,
    22547,
    24595,
    24595,
    20497,
    20497,
    20497,
    20497,
    18449,
    18449,
    18449,
    18449,
    26643,
    26643,
    28691,
    28691,
    30739,
    30739,
    -32749,
    -32749,
    -30701,
    -30701,
    -28653,
    -28653,
    -26605,
    -26605,
    -24557,
    -24557,
    -22509,
    -22509,
    -20461,
    -20461,
    8207,
    8207,
    8207,
    8207,
    8207,
    8207,
    8207,
    8207,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    72,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    104,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    4107,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    266,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    298,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    136,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    168,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    460,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    492,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    2059,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    200,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232,
    232
  };
  private int[] m_originBlack = new int[16 /*0x10*/]
  {
    3226,
    6412,
    200,
    168,
    38,
    38,
    134,
    134,
    100,
    100,
    100,
    100,
    68,
    68,
    68,
    68
  };
  private int[] m_blackPixel = new int[512 /*0x0200*/]
  {
    62,
    62,
    30,
    30,
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
    0,
    0,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    3225,
    588,
    588,
    588,
    588,
    588,
    588,
    588,
    588,
    1680,
    1680,
    20499,
    22547,
    24595,
    26643,
    1776,
    1776,
    1808,
    1808,
    -24557,
    -22509,
    -20461,
    -18413,
    1904,
    1904,
    1936,
    1936,
    -16365,
    -14317,
    782,
    782,
    782,
    782,
    814,
    814,
    814,
    814,
    -12269,
    -10221,
    10257,
    10257,
    12305,
    12305,
    14353,
    14353,
    16403,
    18451,
    1712,
    1712,
    1744,
    1744,
    28691,
    30739,
    -32749,
    -30701,
    -28653,
    -26605,
    2061,
    2061,
    2061,
    2061,
    2061,
    2061,
    2061,
    2061,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    424,
    750,
    750,
    750,
    750,
    1616,
    1616,
    1648,
    1648,
    1424,
    1424,
    1456,
    1456,
    1488,
    1488,
    1520,
    1520,
    1840,
    1840,
    1872,
    1872,
    1968,
    1968,
    8209,
    8209,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    524,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    556,
    1552,
    1552,
    1584,
    1584,
    2000,
    2000,
    2032,
    2032,
    976,
    976,
    1008,
    1008,
    1040,
    1040,
    1072,
    1072,
    1296,
    1296,
    1328,
    1328,
    718,
    718,
    718,
    718,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    456,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    326,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    358,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    490,
    4113,
    4113,
    6161,
    6161,
    848,
    848,
    880,
    880,
    912,
    912,
    944,
    944,
    622,
    622,
    622,
    622,
    654,
    654,
    654,
    654,
    1104,
    1104,
    1136,
    1136,
    1168,
    1168,
    1200,
    1200,
    1232,
    1232,
    1264,
    1264,
    686,
    686,
    686,
    686,
    1360,
    1360,
    1392,
    1392,
    12,
    12,
    12,
    12,
    12,
    12,
    12,
    12,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390,
    390
  };
  private int[] m_blackBit = new int[4]
  {
    292,
    260,
    226,
    226
  };
  internal bool m_isExtGStateContainsSMask;
  private int m_elementChanging;
  internal bool m_isDeviceGrayColorspace;
  internal bool m_isDeviceRGBColorspace;
  internal bool m_isDeviceCMYKColorspace;
  private int m_originalLineLength;
  private int m_relativeAddress;
  private int m_currentIndex = 1;
  private int m_bitArrived;
  internal bool m_isWhitePixel = true;
  internal bool m_is2dimention = true;
  private int m_bytesOnDemand;
  private int m_indexPointer;
  private int m_bitDataCount;
  private DrawImagePixels m_outputStream;
  private DrawImagePixels m_bitData;
  private PdfDictionary m_imageDictionary;
  private string[] m_imageFilter;
  private PdfDictionary[] m_decodeParam;
  private Stream m_imageStream;
  private Stream m_maskStream;
  private float m_height;
  private float m_width;
  private float m_maskWidth;
  private float m_maskHeight;
  private string[] m_maskFilter;
  private float m_maskBitsPerComponent;
  private float m_bitsPerComponent;
  private Image m_embeddedImage;
  private bool m_isImageStreamParsed;
  private PdfMatrix m_imageInfo;
  private string m_colorspace;
  private string m_colorspaceBase;
  private string internalColorSpace;
  private int m_colorspaceHival;
  private PixelFormat m_pixelFormat = PixelFormat.Format24bppRgb;
  private ColorPalette m_colorPalette;
  private MemoryStream m_colorspaceStream;
  internal bool m_isBlackIs1;
  private bool IsTransparent;
  internal bool m_isMaskImage;
  internal StringBuilder exceptions = new StringBuilder();
  internal bool m_isIccBasedAlternateDeviceGray;
  private Dictionary<string, MemoryStream> colorSpaceResourceDict = new Dictionary<string, MemoryStream>();
  private Dictionary<string, PdfStream> nonIndexedImageColorResource = new Dictionary<string, PdfStream>();
  private bool isIndexedImage;
  private bool m_isImageMask;
  public MemoryStream outStream = new MemoryStream();
  private bool m_isEarlyChange = true;
  private int[] m_pixel;
  private string m_maskColorspace;
  private int numberOfComponents;
  private byte[] inputData;
  private PdfDictionary m_maskDictionary;
  private string indexedColorSpace;
  private bool isDualFilter;
  private Stream imageStreamBackup;
  private bool m_isImageInterpolated;
  private bool m_isImageMasked;
  private bool m_isSoftMasked;
  private byte[] m_indexedRGBvalues;
  private bool m_isImageForExtraction;
  private int[] m_maskedPixels;
  private bool isDeviceN;
  private PdfArray m_decodeArray;
  private MemoryStream m_decodedMemoryStream;
  private static readonly object ImageParsingLocker = new object();

  public static event ImageStructure.ImagePreRenderEventHandler ImagePreRender;

  public ImageStructure()
  {
  }

  public ImageStructure(IPdfPrimitive fontDictionary, PdfMatrix tm)
  {
    this.m_imageDictionary = fontDictionary as PdfDictionary;
    this.ImageInfo = tm;
  }

  internal bool IsImageMasked => this.m_isImageMasked;

  internal bool IsSoftMasked => this.m_isSoftMasked;

  internal bool IsImageMask
  {
    get
    {
      this.GetIsImageMask();
      return this.m_isImageMask;
    }
    set => this.m_isImageMask = value;
  }

  internal bool IsImageInterpolated => this.m_isImageInterpolated;

  internal bool IsImageForExtraction
  {
    get => this.m_isImageForExtraction;
    set => this.m_isImageForExtraction = value;
  }

  internal bool IsEarlyChange
  {
    get
    {
      this.GetIsEarlyChange();
      return this.m_isEarlyChange;
    }
    set => this.m_isEarlyChange = value;
  }

  internal PdfDictionary ImageDictionary => this.m_imageDictionary;

  internal PdfMatrix ImageInfo
  {
    get => this.m_imageInfo;
    set => this.m_imageInfo = value;
  }

  internal string[] ImageFilter
  {
    get
    {
      if (this.m_imageFilter == null)
        this.m_imageFilter = this.GetImageFilter();
      return this.m_imageFilter;
    }
  }

  internal string ColorSpace
  {
    get
    {
      if (this.m_colorspace == null)
        this.GetColorSpace();
      return this.m_colorspace;
    }
    set => this.m_colorspace = value;
  }

  internal MemoryStream DecodedMemoryStream => this.m_decodedMemoryStream;

  internal Image EmbeddedImage
  {
    get
    {
      if (this.m_embeddedImage == null && !this.m_isImageStreamParsed)
      {
        if (PdfDocument.EnableThreadSafe)
        {
          lock (ImageStructure.ImageParsingLocker)
            this.GetEmbeddedImage();
        }
        else
          this.GetEmbeddedImage();
      }
      return this.m_embeddedImage;
    }
  }

  internal PdfDictionary[] DecodeParam
  {
    get
    {
      if (this.m_decodeParam == null)
        this.m_decodeParam = this.GetDecodeParam(this.m_imageDictionary);
      return this.m_decodeParam;
    }
  }

  internal PdfArray DecodeArray
  {
    get
    {
      this.GetDecodeArray();
      return this.m_decodeArray;
    }
    set => this.m_decodeArray = value;
  }

  public Stream ImageStream
  {
    get
    {
      if (this.m_imageStream == null)
      {
        PdfStream imageDictionary = this.m_imageDictionary as PdfStream;
        this.m_imageStream = (Stream) imageDictionary.InternalStream;
        this.inputData = new byte[this.m_imageStream.Length];
        this.inputData = imageDictionary.Data;
      }
      return this.m_imageStream;
    }
    set => this.m_imageStream = value;
  }

  internal int[] Pixels
  {
    get => this.m_pixel;
    set => this.m_pixel = value;
  }

  public Stream MaskStream
  {
    get
    {
      if (this.m_maskStream == null)
      {
        PdfStream pdfStream = (PdfStream) null;
        if (this.m_imageDictionary.ContainsKey("SMask"))
          pdfStream = (this.m_imageDictionary["SMask"] as PdfReferenceHolder).Object as PdfStream;
        else if (this.m_imageDictionary.ContainsKey("Mask"))
          pdfStream = (this.m_imageDictionary["Mask"] as PdfReferenceHolder).Object as PdfStream;
        this.m_maskStream = (Stream) pdfStream.InternalStream;
        PdfDictionary pdfDictionary = (PdfDictionary) null;
        if (this.m_imageDictionary.ContainsKey("SMask"))
          pdfDictionary = (this.m_imageDictionary["SMask"] as PdfReferenceHolder).Object as PdfDictionary;
        else if (this.m_imageDictionary.ContainsKey("Mask"))
          pdfDictionary = (this.m_imageDictionary["Mask"] as PdfReferenceHolder).Object as PdfDictionary;
        this.m_maskDictionary = pdfDictionary;
        if (pdfDictionary.ContainsKey("Width"))
          this.m_maskWidth = (float) (pdfDictionary["Width"] as PdfNumber).IntValue;
        if (pdfDictionary.ContainsKey("Height"))
          this.m_maskHeight = (float) (pdfDictionary["Height"] as PdfNumber).IntValue;
        if (pdfDictionary.ContainsKey("BitsPerComponent"))
          this.m_maskBitsPerComponent = (float) (pdfDictionary["BitsPerComponent"] as PdfNumber).IntValue;
        this.m_maskFilter = new string[1];
        if (pdfDictionary.ContainsKey("Filter"))
        {
          if (pdfDictionary["Filter"] is PdfArray)
          {
            PdfArray pdfArray = pdfDictionary["Filter"] as PdfArray;
            this.m_maskFilter = new string[pdfArray.Count];
            for (int index = 0; index < pdfArray.Count; ++index)
              this.m_maskFilter[index] = (pdfArray[index] as PdfName).Value;
          }
          else
            this.m_maskFilter[0] = (pdfDictionary["Filter"] as PdfName).Value;
        }
        if (pdfDictionary.ContainsKey("ColorSpace"))
          this.m_maskColorspace = (pdfDictionary["ColorSpace"] as PdfName).Value;
      }
      return this.m_maskStream;
    }
    set => this.m_maskStream = value;
  }

  internal float Width
  {
    get
    {
      if ((double) this.m_width == 0.0)
        this.m_width = this.GetImageWidth();
      return this.m_width;
    }
  }

  internal float Height
  {
    get
    {
      if ((double) this.m_height == 0.0)
        this.m_height = this.GetImageHeight();
      return this.m_height;
    }
  }

  internal float BitsPerComponent
  {
    get
    {
      if ((double) this.m_bitsPerComponent == 0.0)
        this.m_bitsPerComponent = this.GetBitsPerComponent();
      return this.m_bitsPerComponent;
    }
  }

  internal int WhitePixel
  {
    get
    {
      int whitePixel = 0;
      bool flag = true;
      while (flag)
      {
        int index = this.OneDimentionBit(10);
        this.m_bitArrived += 10;
        int num1 = this.m_whitePixel[index];
        int num2 = num1 >>> 1 & 15;
        switch (num2)
        {
          case 0:
          case 15:
            throw new Exception("CCITT Error in getWhitePixel");
          case 12:
            int num3 = this.OneDimentionBit(2);
            this.m_bitArrived += 2;
            int blackandWhitePixel = this.m_BlackandWhitePixels[index << 2 & 12 | num3];
            int num4 = blackandWhitePixel >>> 1 & 7;
            int num5 = blackandWhitePixel >>> 4 & 4095 /*0x0FFF*/;
            whitePixel += num5;
            this.renewPointer(4 - num4);
            continue;
          default:
            int num6 = num1 >>> 5 & 2047 /*0x07FF*/;
            whitePixel += num6;
            this.renewPointer(10 - num2);
            if ((num1 & 1) == 0)
            {
              flag = false;
              continue;
            }
            continue;
        }
      }
      return whitePixel;
    }
  }

  internal int BlackPixel
  {
    get
    {
      int blackPixel = 0;
      bool flag = true;
      while (flag)
      {
        int index1 = this.OneDimentionBit(4);
        this.m_bitArrived += 4;
        int num1 = this.m_originBlack[index1];
        int num2 = num1 >>> 1 & 15;
        int num3 = num1 >>> 5 & 2047 /*0x07FF*/;
        switch (num3)
        {
          case 100:
            int index2 = this.OneDimentionBit(9);
            this.m_bitArrived += 9;
            int num4 = this.m_blackPixel[index2];
            int num5 = num4 >>> 1 & 15;
            int num6 = num4 >>> 5 & 2047 /*0x07FF*/;
            if (num5 == 12)
            {
              this.renewPointer(5);
              int index3 = this.OneDimentionBit(4);
              this.m_bitArrived += 4;
              int blackandWhitePixel = this.m_BlackandWhitePixels[index3];
              int num7 = blackandWhitePixel >>> 1 & 7;
              int num8 = blackandWhitePixel >>> 4 & 4095 /*0x0FFF*/;
              blackPixel += num8;
              this.renewPointer(4 - num7);
              continue;
            }
            if (num5 == 15)
              throw new Exception("CCITT unexpected EOL");
            blackPixel += num6;
            this.renewPointer(9 - num5);
            if ((num4 & 1) == 0)
            {
              flag = false;
              continue;
            }
            continue;
          case 200:
            int index4 = this.OneDimentionBit(2);
            this.m_bitArrived += 2;
            int num9 = this.m_blackBit[index4];
            int num10 = num9 >>> 5 & 2047 /*0x07FF*/;
            blackPixel += num10;
            this.renewPointer(2 - (num9 >>> 1 & 15));
            flag = false;
            continue;
          default:
            blackPixel += num3;
            this.renewPointer(4 - num2);
            flag = false;
            continue;
        }
      }
      return blackPixel;
    }
  }

  internal void FixTwoDimention(
    int[] previousRange,
    int[] currentRange,
    int elementChanging,
    int[] currentElementChange)
  {
    this.m_isWhitePixel = true;
    this.m_currentIndex = 0;
    this.m_relativeAddress = 0;
    int num1 = 0;
    int renewedBits = -1;
    while ((double) this.m_relativeAddress < (double) this.Width)
    {
      this.GetSucceedingElement(renewedBits, this.m_isWhitePixel, currentElementChange, previousRange, elementChanging);
      int index = this.OneDimentionBit(7);
      this.m_bitArrived += 7;
      int num2 = (int) this.m_twoDimention[index] & (int) byte.MaxValue;
      int num3 = (num2 & 120) >>> 3;
      if (!this.m_is2dimention)
        num1 = num2 & 7;
      else if (num3 != 11)
        this.renewPointer(7 - (num2 & 7));
      switch (num3)
      {
        case 0:
          int num4 = currentElementChange[1] - this.m_relativeAddress;
          if (!this.m_isWhitePixel)
            this.m_outputStream.SetIndex(this.m_indexPointer, this.m_indexPointer + num4);
          this.m_indexPointer += num4;
          this.m_relativeAddress = currentElementChange[1];
          renewedBits = currentElementChange[1];
          if (!this.m_is2dimention)
          {
            this.m_bitArrived -= 7 - num1;
            continue;
          }
          continue;
        case 1:
          if (!this.m_is2dimention)
            this.m_bitArrived -= 7 - num1;
          int num5;
          if (this.m_isWhitePixel)
          {
            int whitePixel = this.WhitePixel;
            this.m_indexPointer += whitePixel;
            this.m_relativeAddress += whitePixel;
            currentRange[this.m_currentIndex++] = this.m_relativeAddress;
            num5 = this.BlackPixel;
            this.m_outputStream.SetIndex(this.m_indexPointer, this.m_indexPointer + num5);
            this.m_indexPointer += num5;
          }
          else
          {
            int blackPixel = this.BlackPixel;
            this.m_outputStream.SetIndex(this.m_indexPointer, this.m_indexPointer + blackPixel);
            this.m_indexPointer += blackPixel;
            this.m_relativeAddress += blackPixel;
            currentRange[this.m_currentIndex++] = this.m_relativeAddress;
            num5 = this.WhitePixel;
            this.m_indexPointer += num5;
          }
          this.m_relativeAddress += num5;
          currentRange[this.m_currentIndex++] = this.m_relativeAddress;
          renewedBits = this.m_relativeAddress;
          continue;
        case 11:
          int num6 = this.OneDimentionBit(3);
          this.m_bitArrived += 3;
          if (num6 != 7)
            throw new Exception($"The value of{(object) num6} was Unexpected");
          int num7 = 0;
          bool flag = false;
          while (!flag)
          {
            while (true)
            {
              int num8 = this.OneDimentionBit(1);
              ++this.m_bitArrived;
              if (num8 != 1)
                ++num7;
              else
                break;
            }
            if (num7 > 5)
            {
              num7 -= 6;
              if (!this.m_isWhitePixel && num7 > 0)
                currentRange[this.m_currentIndex++] = this.m_relativeAddress;
              this.m_relativeAddress += num7;
              if (num7 > 0)
                this.m_isWhitePixel = true;
              int num9 = this.OneDimentionBit(1);
              ++this.m_bitArrived;
              if (num9 == 0)
              {
                if (!this.m_isWhitePixel)
                  currentRange[this.m_currentIndex++] = this.m_relativeAddress;
                this.m_isWhitePixel = true;
              }
              else
              {
                if (this.m_isWhitePixel)
                  currentRange[this.m_currentIndex++] = this.m_relativeAddress;
                this.m_isWhitePixel = false;
              }
              flag = true;
            }
            if (num7 == 5)
            {
              if (!this.m_isWhitePixel)
                currentRange[this.m_currentIndex++] = this.m_relativeAddress;
              this.m_relativeAddress += num7;
              this.m_isWhitePixel = true;
            }
            else
            {
              this.m_relativeAddress += num7;
              currentRange[this.m_currentIndex++] = this.m_relativeAddress;
              this.m_outputStream.SetIndex(this.m_indexPointer, this.m_indexPointer + 1);
              ++this.m_indexPointer;
              ++this.m_relativeAddress;
              this.m_isWhitePixel = false;
            }
          }
          continue;
        default:
          if (num3 > 8)
            throw new Exception("CCITT unexpected value");
          currentRange[this.m_currentIndex++] = currentElementChange[0] + (num3 - 5);
          int num10 = currentElementChange[0] + (num3 - 5) - this.m_relativeAddress;
          if (!this.m_isWhitePixel)
            this.m_outputStream.SetIndex(this.m_indexPointer, this.m_indexPointer + num10);
          this.m_indexPointer += num10;
          renewedBits = currentElementChange[0] + (num3 - 5);
          this.m_relativeAddress = renewedBits;
          this.m_isWhitePixel = !this.m_isWhitePixel;
          if (!this.m_is2dimention)
          {
            this.m_bitArrived -= 7 - num1;
            continue;
          }
          continue;
      }
    }
  }

  private void GetSucceedingElement(
    int renewedBits,
    bool isWhitePixel,
    int[] currentElementChange,
    int[] previousElementChange,
    int elementChanging)
  {
    int num1 = 0;
    int index;
    for (index = !isWhitePixel ? num1 | 1 : num1 & -2; index < elementChanging; index += 2)
    {
      int num2 = previousElementChange[index];
      if (num2 > renewedBits)
      {
        currentElementChange[0] = num2;
        break;
      }
    }
    if (index + 1 >= elementChanging)
      return;
    currentElementChange[1] = previousElementChange[index + 1];
  }

  internal int OneDimentionBit(int bitsToGet) => this.GetOneDimentionBit(bitsToGet, false);

  private int GetOneDimentionBit(int bitsToGet, bool is1dimention)
  {
    int oneDimentionBit = 0;
    int num1 = 0;
    if (is1dimention && bitsToGet > 8)
      ++num1;
    for (int index = 0; index < bitsToGet; ++index)
    {
      if (this.m_bitData.GetValue(index + this.m_bitArrived))
      {
        int num2 = 1 << bitsToGet - index - 1 - num1;
        oneDimentionBit |= num2;
      }
    }
    return oneDimentionBit;
  }

  private void renewPointer(int bitsToMoveBack) => this.m_bitArrived -= bitsToMoveBack;

  private DrawImagePixels EstimateBit(MemoryStream imageStream, int bitOnDemand)
  {
    int index1 = 0;
    DrawImagePixels drawImagePixels = new DrawImagePixels(bitOnDemand);
    foreach (byte num in imageStream.GetBuffer())
    {
      for (int index2 = 7; index2 >= 0; --index2)
      {
        if (((int) num & 1 << index2) >= 1)
          drawImagePixels.SetBitdata(index1);
        ++index1;
      }
    }
    return drawImagePixels;
  }

  private Image GetEmbeddedImage()
  {
    try
    {
      this.m_decodedMemoryStream = this.GetImageStream() as MemoryStream;
      if (this.m_decodedMemoryStream == null)
        return (Image) null;
      this.m_embeddedImage = Image.FromStream((Stream) this.m_decodedMemoryStream);
      this.ImageStream = (Stream) null;
      return this.m_embeddedImage;
    }
    catch
    {
      return (Image) null;
    }
  }

  internal byte[] GetEncodedStream()
  {
    byte[] encodedStream = new byte[this.m_bytesOnDemand];
    int num1 = 7;
    int index1 = 0;
    byte num2 = 0;
    for (int index2 = 0; index2 < this.m_indexPointer; ++index2)
    {
      if (this.m_outputStream.GetValue(index2))
      {
        int num3 = 1 << num1;
        num2 |= (byte) num3;
        --num1;
      }
      else
        --num1;
      if ((double) (index2 + 1) % (double) this.Width == 0.0 && index2 != 0)
        num1 = -1;
      if (num1 < 0 && index1 < encodedStream.Length)
      {
        encodedStream[index1] = num2;
        ++index1;
        num1 = 7;
        num2 = (byte) 0;
      }
    }
    return encodedStream;
  }

  private MemoryStream DecodeCCITTFaxDecodeStream(
    MemoryStream imageStream,
    PdfDictionary imageDictionary,
    PdfDictionary decodeParams)
  {
    PdfArray pdfArray = new PdfArray();
    bool flag = false;
    int offset1 = 1;
    int offset2 = 0;
    int offset3 = 0;
    if (imageDictionary.ContainsKey("Width"))
    {
      if (imageDictionary["Width"] is PdfNumber)
        offset2 = (imageDictionary["Width"] as PdfNumber).IntValue;
      else if (imageDictionary["Width"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        offset2 = ((imageDictionary["Width"] as PdfReferenceHolder).Object as PdfNumber).IntValue;
    }
    if (imageDictionary.ContainsKey("Height"))
    {
      if (imageDictionary["Height"] is PdfNumber)
        offset3 = (imageDictionary["Height"] as PdfNumber).IntValue;
      else if (imageDictionary["Height"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        offset3 = ((imageDictionary["Height"] as PdfReferenceHolder).Object as PdfNumber).IntValue;
    }
    if (imageDictionary.ContainsKey("Decode"))
    {
      pdfArray = imageDictionary[new PdfName("Decode")] as PdfArray;
      offset1 = (pdfArray[0] as PdfNumber).IntValue;
    }
    TiffDecode tiffDecode = new TiffDecode();
    tiffDecode.m_tiffHeader.m_byteOrder = (short) 18761;
    tiffDecode.m_tiffHeader.m_version = (short) 42;
    tiffDecode.m_tiffHeader.m_dirOffset = (uint) ((ulong) imageStream.Length + 9UL);
    tiffDecode.WriteHeader(tiffDecode.m_tiffHeader);
    tiffDecode.m_stream.Seek(8L, SeekOrigin.Begin);
    if (decodeParams.ContainsKey("EncodedByteAlign"))
    {
      PdfBoolean decodeParam = decodeParams["EncodedByteAlign"] as PdfBoolean;
      if (decodeParam.Value && decodeParams.ContainsKey("K") && (decodeParams["K"] as PdfNumber).IntValue < 0)
      {
        int intValue = (decodeParams["Columns"] as PdfNumber).IntValue;
        this.m_originalLineLength = intValue + 7 >> 3;
        this.m_bytesOnDemand = (int) this.Height * this.m_originalLineLength;
        this.m_outputStream = new DrawImagePixels(this.m_bytesOnDemand << 3);
        this.m_bitDataCount = (int) imageStream.Length << 3;
        this.m_bitData = this.EstimateBit(imageStream, this.m_bitDataCount);
        int num1 = 0;
        int[] previousRange = new int[(int) this.Width + 1];
        int[] currentRange = new int[(int) this.Width + 1];
        this.m_elementChanging = 2;
        currentRange[0] = (int) this.Width;
        currentRange[1] = (int) this.Width;
        int[] currentElementChange = new int[2];
        for (int index = 0; index < (int) this.Height; ++index)
        {
          if (decodeParam.Value && this.m_bitArrived > 0)
          {
            int num2 = this.m_bitArrived % 8;
            int num3 = 8 - num2;
            if (num2 > 0)
              this.m_bitArrived += num3;
          }
          int[] numArray = previousRange;
          previousRange = currentRange;
          currentRange = numArray;
          this.FixTwoDimention(previousRange, currentRange, this.m_elementChanging, currentElementChange);
          if (currentRange.Length != this.m_currentIndex)
          {
            this.m_relativeAddress = intValue;
            currentRange[this.m_currentIndex++] = this.m_relativeAddress;
          }
          this.m_elementChanging = this.m_currentIndex;
          num1 += this.m_originalLineLength;
        }
        byte[] encodedStream = this.GetEncodedStream();
        tiffDecode.m_stream.Write(encodedStream, 0, encodedStream.Length);
        flag = true;
      }
    }
    if (decodeParams.ContainsKey("EncodedByteAlign"))
    {
      if (!(decodeParams["EncodedByteAlign"] as PdfBoolean).Value || !flag)
        tiffDecode.m_stream.Write(imageStream.ToArray(), 0, (int) imageStream.Length);
    }
    else
      tiffDecode.m_stream.Write(imageStream.ToArray(), 0, (int) imageStream.Length);
    tiffDecode.SetField(1, offset2, TiffTag.ImageWidth, TiffType.Short);
    tiffDecode.SetField(1, offset3, TiffTag.ImageLength, TiffType.Short);
    tiffDecode.SetField(1, 1, TiffTag.BitsPerSample, TiffType.Short);
    if (decodeParams != null && decodeParams.ContainsKey("K"))
    {
      if ((decodeParams["K"] as PdfNumber).IntValue < 0)
      {
        if (decodeParams.ContainsKey("EncodedByteAlign") && (decodeParams["EncodedByteAlign"] as PdfBoolean).Value)
          tiffDecode.SetField(1, 1, TiffTag.Compression, TiffType.Short);
        else
          tiffDecode.SetField(1, 4, TiffTag.Compression, TiffType.Short);
      }
      else if ((decodeParams["K"] as PdfNumber).IntValue == 0)
      {
        if (decodeParams.ContainsKey("EndOfBlock"))
        {
          if ((decodeParams["EndOfBlock"] as PdfBoolean).Value)
            tiffDecode.SetField(1, 2, TiffTag.Compression, TiffType.Short);
          else
            tiffDecode.SetField(1, 3, TiffTag.Compression, TiffType.Short);
        }
        else
          tiffDecode.SetField(1, 3, TiffTag.Compression, TiffType.Short);
      }
      else
        tiffDecode.SetField(1, 3, TiffTag.Compression, TiffType.Short);
      if (decodeParams.ContainsKey("BlackIs1"))
      {
        if ((decodeParams["BlackIs1"] as PdfBoolean).Value)
        {
          if (pdfArray.Count != 0)
            offset1 = (pdfArray[1] as PdfNumber).IntValue;
          else if (this.ColorSpace == "Indexed" && this.m_colorspaceBase == "DeviceRGB")
            offset1 = 0;
          tiffDecode.SetField(1, offset1, TiffTag.Photometric, TiffType.Short);
        }
        else
          tiffDecode.SetField(1, 0, TiffTag.Photometric, TiffType.Short);
      }
      else if (pdfArray.Count != 0)
        tiffDecode.SetField(1, offset1, TiffTag.Photometric, TiffType.Short);
      else if (this.IsImageMask && imageDictionary.ContainsKey("Decode"))
      {
        pdfArray = this.m_imageDictionary[new PdfName("Decode")] as PdfArray;
        tiffDecode.SetField(1, (pdfArray[0] as PdfNumber).IntValue, TiffTag.Photometric, TiffType.Short);
      }
    }
    else
      tiffDecode.SetField(1, 3, TiffTag.Compression, TiffType.Short);
    if (this.m_isBlackIs1)
      tiffDecode.SetField(1, 1, TiffTag.Photometric, TiffType.Short);
    tiffDecode.SetField(1, 8, TiffTag.StripOffset, TiffType.Long);
    tiffDecode.SetField(1, 1, TiffTag.SamplesPerPixel, TiffType.Short);
    tiffDecode.SetField(1, (int) imageStream.Length, TiffTag.StripByteCounts, TiffType.Long);
    tiffDecode.m_stream.Seek(9L + imageStream.Length, SeekOrigin.Begin);
    tiffDecode.WriteDirEntry(tiffDecode.directoryEntries);
    tiffDecode.m_stream.Position = 0L;
    tiffDecode.m_stream.Capacity = (int) tiffDecode.m_stream.Length;
    if (!imageDictionary.ContainsKey("ImageMask"))
    {
      imageStream = tiffDecode.m_stream;
      imageStream.Position = 0L;
      if (imageDictionary.ContainsKey("Mask"))
      {
        imageStream = this.MergeImages(new Bitmap(Image.FromStream((Stream) imageStream)), this.MaskStream as MemoryStream, false);
        this.m_isImageMasked = true;
      }
    }
    else
    {
      this.m_isImageMask = (imageDictionary["ImageMask"] as PdfBoolean).Value;
      if (this.m_isImageMask)
      {
        this.m_isImageMasked = true;
        imageStream = tiffDecode.m_stream;
        imageStream.Position = 0L;
        this.m_isBlackIs1 = false;
        if (decodeParams.ContainsKey("BlackIs1") && imageDictionary.ContainsKey("Decode"))
        {
          if ((decodeParams["BlackIs1"] as PdfBoolean).Value && (pdfArray[0] as PdfNumber).IntValue == 1 && (pdfArray[1] as PdfNumber).IntValue == 0)
          {
            this.m_isBlackIs1 = false;
            Bitmap bitmap = new Bitmap((Stream) imageStream);
            if (!this.IsImageForExtraction || this.IsTransparent)
              bitmap.MakeTransparent(Color.White);
            imageStream = new MemoryStream();
            bitmap.Save((Stream) imageStream, ImageFormat.Png);
            bitmap.Dispose();
            imageStream.Position = 0L;
          }
        }
        else if (!decodeParams.ContainsKey("BlackIs1") || this.m_isMaskImage)
        {
          this.m_isBlackIs1 = false;
          Bitmap bitmap = new Bitmap((Stream) imageStream);
          if (!this.IsImageForExtraction || this.IsTransparent)
            bitmap.MakeTransparent(Color.White);
          imageStream = new MemoryStream();
          bitmap.Save((Stream) imageStream, ImageFormat.Png);
          bitmap.Dispose();
          imageStream.Position = 0L;
        }
        if (decodeParams.ContainsKey("BlackIs1") && !this.m_isMaskImage)
          this.m_isBlackIs1 = true;
      }
      else
      {
        imageStream = tiffDecode.m_stream;
        imageStream.Position = 0L;
      }
    }
    return imageStream;
  }

  public Stream GetImageStream()
  {
    this.m_isImageStreamParsed = true;
    bool flag1 = true;
    this.GetImageInterpolation(this.ImageDictionary);
    if (this.ImageFilter == null)
    {
      this.m_imageFilter = new string[1]{ "FlateDecode" };
      flag1 = false;
    }
    if (this.ImageFilter == null)
      return (Stream) null;
    for (int index1 = 0; index1 < this.ImageFilter.Length; ++index1)
    {
      if (this.ImageFilter.Length > 1)
        this.isDualFilter = true;
      int num1;
      switch (this.ImageFilter[index1])
      {
        case "A85":
        case "ASCII85Decode":
          this.ImageStream = (Stream) this.DecodeASCII85Stream(this.ImageStream as MemoryStream);
          if (this.isDualFilter)
            this.imageStreamBackup = this.ImageStream;
          this.ImageStream.Position = 0L;
          if (this.ColorSpace == "DeviceGray")
          {
            Stream stream = (Stream) new MemoryStream();
            ColorConvertor[] colorConvertorArray = this.RenderGrayPixels((this.ImageStream as MemoryStream).ToArray());
            int[] pixelArray = new int[colorConvertorArray.Length];
            for (int index2 = 0; index2 < pixelArray.Length; ++index2)
              pixelArray[index2] = colorConvertorArray[index2].PixelConversion();
            this.m_embeddedImage = (Image) this.RenderImage(pixelArray);
            this.m_embeddedImage.Save(stream, ImageFormat.Png);
            this.ImageStream = stream;
            break;
          }
          break;
        case "ASCIIHex":
          this.ImageStream = (Stream) new MemoryStream(new ASCIIHex().Decode((this.ImageStream as MemoryStream).GetBuffer()));
          this.ImageStream.Position = 0L;
          break;
        case "RunLengthDecode":
          this.ImageStream.Position = 0L;
          MemoryStream memoryStream1 = new MemoryStream();
          byte[] array1 = (this.ImageStream as MemoryStream).ToArray();
          int length1 = array1.Length;
          int index3;
          for (int index4 = 0; index4 < length1; index4 = index3 + 1)
          {
            int num2 = (int) array1[index4];
            if (num2 < 0)
              num2 = 256 /*0x0100*/ + num2;
            if (num2 == 128 /*0x80*/)
              index3 = length1;
            else if (num2 > 128 /*0x80*/)
            {
              index3 = index4 + 1;
              int length2 = 257 - num2;
              int num3 = (int) array1[index3];
              byte[] buffer = new byte[length2];
              for (int index5 = 0; index5 < length2; ++index5)
                buffer[index5] = (byte) num3;
              memoryStream1.Write(buffer, 0, buffer.Length);
            }
            else
            {
              int num4 = index4 + 1;
              int length3 = num2 + 1;
              byte[] buffer = new byte[length3];
              for (int index6 = 0; index6 < length3; ++index6)
              {
                int num5 = (int) array1[num4 + index6];
                buffer[index6] = (byte) num5;
              }
              memoryStream1.Write(buffer, 0, buffer.Length);
              index3 = num4 + length3 - 1;
            }
          }
          byte[] buffer1 = memoryStream1.GetBuffer();
          byte[] numArray1 = buffer1;
          if (this.ColorSpace == "DeviceRGB")
          {
            using (Bitmap bitmap = this.RenderImage(this.RenderRGBPixels(numArray1)))
            {
              if (this.ImageStream != null)
                this.ImageStream.Dispose();
              this.ImageStream = (Stream) new MemoryStream();
              bitmap.Save(this.ImageStream, ImageFormat.Png);
              this.ImageStream.Position = 0L;
              break;
            }
          }
          if (this.ColorSpace == "DeviceGray")
          {
            byte[] numArray2 = buffer1;
            int height = (int) this.Height;
            int width1 = (int) this.Width;
            byte[] numArray3 = new byte[width1 * height];
            int[] numArray4 = new int[8]
            {
              1,
              2,
              4,
              8,
              16 /*0x10*/,
              32 /*0x20*/,
              64 /*0x40*/,
              128 /*0x80*/
            };
            int num6 = (int) this.Width + 7 >> 3;
            int num7 = 1;
            try
            {
              for (int index7 = 0; index7 < height; ++index7)
              {
                for (int index8 = 0; index8 < width1; ++index8)
                {
                  int num8 = 0;
                  int num9 = 0;
                  int num10 = num7;
                  int num11 = num7;
                  int num12 = (int) this.Width - index8;
                  int num13 = (int) this.Height - index7;
                  if (num10 > num12)
                    num10 = num12;
                  if (num11 > num13)
                    num11 = num13;
                  for (int index9 = 0; index9 < num11; ++index9)
                  {
                    for (int index10 = 0; index10 < num10; ++index10)
                    {
                      if (((int) numArray2[(index9 + index7 * num7) * num6 + (index8 * num7 + index10 >> 3)] & numArray4[7 - (index8 * num7 + index10 & 7)]) != 0)
                        ++num8;
                      ++num9;
                    }
                  }
                  int index11 = index8 + width1 * index7;
                  numArray3[index11] = (byte) ((int) byte.MaxValue * num8 / num9);
                }
              }
            }
            catch
            {
            }
            byte[] source = numArray3;
            this.m_pixelFormat = PixelFormat.Format8bppIndexed;
            Bitmap bitmap = new Bitmap((int) this.Width, (int) this.Height, this.m_pixelFormat);
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int startIndex = 0;
            long int64 = bitmapdata.Scan0.ToInt64();
            int width2 = (int) this.Width;
            for (int index12 = 0; (double) index12 < (double) this.Height; ++index12)
            {
              Marshal.Copy(source, startIndex, new IntPtr(int64), width2);
              startIndex += width2;
              int64 += (long) bitmapdata.Stride;
            }
            bitmap.UnlockBits(bitmapdata);
            MemoryStream memoryStream2 = new MemoryStream();
            bitmap.Save((Stream) memoryStream2, ImageFormat.Png);
            this.ImageStream = (Stream) memoryStream2;
            this.ImageStream.Position = 0L;
            break;
          }
          if (this.ColorSpace == "Indexed")
          {
            Bitmap bitmap = new Bitmap((int) this.Width, (int) this.Height, this.m_pixelFormat);
            MemoryStream memoryStream3 = new MemoryStream();
            this.RenderImage(this.GetIndexedPixelData(numArray1)).Save((Stream) memoryStream3, ImageFormat.Png);
            this.ImageStream = (Stream) memoryStream3;
            this.ImageStream.Position = 0L;
            break;
          }
          Bitmap bitmap1 = new Bitmap((int) this.Width, (int) this.Height, this.m_pixelFormat);
          MemoryStream memoryStream4 = new MemoryStream();
          Bitmap input1;
          if (this.m_imageDictionary.ContainsKey("Decode") && this.ColorSpace == null)
          {
            input1 = this.RenderImage(this.GetStencilMaskedPixels(numArray1, this.Width, this.Height));
            input1.Save((Stream) memoryStream4, ImageFormat.Png);
          }
          else
          {
            input1 = new Bitmap((int) this.Width, (int) this.Height, this.m_pixelFormat);
            BitmapData bitmapdata = input1.LockBits(new Rectangle(0, 0, input1.Width, input1.Height), ImageLockMode.ReadWrite, input1.PixelFormat);
            if (this.m_pixelFormat == PixelFormat.Format8bppIndexed)
              input1.Palette = this.m_colorPalette;
            int num14 = Image.GetPixelFormatSize(input1.PixelFormat) / 8;
            num1 = num14;
            switch (num1)
            {
              case 3:
                for (int index13 = 0; index13 + 3 < numArray1.Length; index13 += 3)
                {
                  int index14 = index13 + 2;
                  byte num15 = numArray1[index14];
                  numArray1[index14] = numArray1[index13];
                  numArray1[index13] = num15;
                }
                break;
              case 4:
                byte[] numArray5 = new byte[(int) ((double) this.Width * (double) this.Height * 3.0)];
                int num16 = (int) ((double) this.Width * (double) this.Height * 4.0);
                byte[] numArray6 = numArray1;
                int index15 = 0;
                int index16 = 0;
                for (; index15 < num16 + 4; index15 += 4)
                {
                  numArray5[index16 + 2] = numArray6[index15];
                  numArray5[index16 + 1] = numArray6[index15 + 1];
                  numArray5[index16] = numArray6[index15 + 2];
                  index16 += 3;
                }
                numArray1 = numArray5;
                break;
            }
            if (Math.Abs(bitmapdata.Stride) * input1.Height < numArray1.Length)
            {
              int startIndex = 0;
              long int64 = bitmapdata.Scan0.ToInt64();
              int length4 = (int) this.Width;
              if (num14 == 3)
                length4 = (int) this.Width * 3;
              for (int index17 = 0; (double) index17 < (double) this.Height; ++index17)
              {
                Marshal.Copy(numArray1, startIndex, new IntPtr(int64), length4);
                startIndex += length4;
                int64 += (long) bitmapdata.Stride;
              }
            }
            else
              Marshal.Copy(numArray1, 0, bitmapdata.Scan0, numArray1.Length);
            input1.UnlockBits(bitmapdata);
            input1.Save((Stream) memoryStream4, ImageFormat.Bmp);
          }
          if (!this.m_imageDictionary.ContainsKey("SMask"))
          {
            this.ImageStream = (Stream) memoryStream4;
            this.ImageStream.Position = 0L;
            break;
          }
          try
          {
            this.ImageStream = (Stream) this.MergeImages(input1, this.MaskStream as MemoryStream, false);
            this.m_isSoftMasked = true;
            this.ImageStream.Position = 0L;
            break;
          }
          catch (Exception ex)
          {
            this.ImageStream = (Stream) memoryStream4;
            this.ImageStream.Position = 0L;
            break;
          }
        case "DCTDecode":
          if (!this.m_imageDictionary.ContainsKey("SMask"))
          {
            if (!this.m_imageDictionary.ContainsKey("Mask"))
            {
              this.ImageStream.Position = 0L;
              if ((this.ColorSpace == "DeviceCMYK" || this.ColorSpace == "DeviceN" || this.ColorSpace == "DeviceGray" || this.ColorSpace == "Separation" || this.ColorSpace == "DeviceRGB" || this.ColorSpace == "ICCBased" && this.numberOfComponents == 4) && (!(this.ColorSpace == "DeviceRGB") || !this.m_imageDictionary.ContainsKey("DecodeParms") && this.m_imageDictionary.ContainsKey("Decode")))
              {
                if (this.m_imageDictionary.ContainsKey("Decode"))
                {
                  PdfArray image = this.m_imageDictionary["Decode"] as PdfArray;
                  double[] array2;
                  if (this.ColorSpace == "DeviceRGB")
                    array2 = new double[6]
                    {
                      0.0,
                      1.0,
                      0.0,
                      1.0,
                      0.0,
                      1.0
                    };
                  else
                    array2 = new double[8]
                    {
                      1.0,
                      0.0,
                      1.0,
                      0.0,
                      1.0,
                      0.0,
                      1.0,
                      0.0
                    };
                  PdfArray pdfArray = new PdfArray(array2);
                  bool flag2 = true;
                  for (int index18 = 0; index18 < pdfArray.Count; ++index18)
                  {
                    if ((double) (image[index18] as PdfNumber).FloatValue != (double) (pdfArray[index18] as PdfNumber).FloatValue)
                      flag2 = false;
                  }
                  if (flag2)
                    break;
                }
                Bitmap bitmap2 = !this.isDualFilter || this.imageStreamBackup == null ? Image.FromStream(this.ImageStream) as Bitmap : Image.FromStream(this.imageStreamBackup) as Bitmap;
                BitmapData bitmapdata = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadWrite, bitmap2.PixelFormat);
                IntPtr scan0 = bitmapdata.Scan0;
                int length5 = Math.Abs(bitmapdata.Stride) * bitmap2.Height;
                byte[] numArray7 = new byte[length5];
                Marshal.Copy(scan0, numArray7, 0, length5);
                if (this.ColorSpace != "DeviceGray")
                {
                  byte[] source = this.YCCKtoRGB(numArray7);
                  Marshal.Copy(source, 0, scan0, source.Length);
                }
                bitmap2.UnlockBits(bitmapdata);
                Image image1 = (Image) bitmap2;
                this.ImageStream = (Stream) new MemoryStream();
                image1.Save(this.ImageStream, ImageFormat.Png);
                this.ImageStream.Position = 0L;
                break;
              }
              break;
            }
          }
          try
          {
            Bitmap input2;
            try
            {
              if (this.m_imageDictionary.ContainsKey("Mask"))
                this.IsTransparent = true;
              this.ImageStream.Position = 0L;
              if (this.ColorSpace == "DeviceCMYK")
              {
                if (this.m_imageDictionary.ContainsKey("Decode"))
                {
                  PdfArray image = this.m_imageDictionary["Decode"] as PdfArray;
                  PdfArray pdfArray = new PdfArray(new double[8]
                  {
                    1.0,
                    0.0,
                    1.0,
                    0.0,
                    1.0,
                    0.0,
                    1.0,
                    0.0
                  });
                  bool flag3 = true;
                  for (int index19 = 0; index19 < pdfArray.Count; ++index19)
                  {
                    if ((double) (image[index19] as PdfNumber).FloatValue != (double) (pdfArray[index19] as PdfNumber).FloatValue)
                      flag3 = false;
                  }
                  if (flag3)
                    break;
                }
                Bitmap bitmap3 = Image.FromStream(this.ImageStream) as Bitmap;
                BitmapData bitmapdata = bitmap3.LockBits(new Rectangle(0, 0, bitmap3.Width, bitmap3.Height), ImageLockMode.ReadWrite, bitmap3.PixelFormat);
                IntPtr scan0 = bitmapdata.Scan0;
                int length6 = Math.Abs(bitmapdata.Stride) * bitmap3.Height;
                byte[] numArray8 = new byte[length6];
                Marshal.Copy(scan0, numArray8, 0, length6);
                byte[] source = this.YCCKtoRGB(numArray8);
                Marshal.Copy(source, 0, scan0, source.Length);
                bitmap3.UnlockBits(bitmapdata);
                Image image2 = (Image) bitmap3;
                this.ImageStream = (Stream) new MemoryStream();
                image2.Save(this.ImageStream, ImageFormat.Png);
                this.ImageStream.Position = 0L;
              }
              this.ImageStream.Position = 0L;
              input2 = Image.FromStream(this.ImageStream) as Bitmap;
            }
            catch
            {
              input2 = (Bitmap) null;
            }
            this.MaskStream.Position = 0L;
            PdfReferenceHolder image3 = this.m_imageDictionary["SMask"] as PdfReferenceHolder;
            if (image3 != (PdfReferenceHolder) null)
            {
              this.m_isSoftMasked = true;
              if (image3.Object is PdfStream pdfStream)
              {
                PdfDictionary pdfDictionary = (PdfDictionary) pdfStream;
                if (pdfDictionary["Filter"] is PdfArray)
                {
                  PdfArray pdfArray = pdfDictionary["Filter"] as PdfArray;
                  string[] strArray = new string[pdfArray.Count];
                  for (int index20 = 0; index20 < strArray.Length; ++index20)
                    strArray[index20] = (pdfArray[index20] as PdfName).Value;
                }
                else
                {
                  string str = (pdfDictionary["Filter"] as PdfName).Value;
                  if (pdfDictionary.ContainsKey("Decode") && str == "FlateDecode")
                  {
                    if (!pdfDictionary.ContainsKey("DecodeParms"))
                      break;
                  }
                }
              }
            }
            this.ImageStream = (Stream) this.MergeImages(input2, this.MaskStream as MemoryStream, true);
            if (!this.m_isSoftMasked)
              this.m_isImageMasked = true;
            this.ImageStream.Position = 0L;
            input2.Dispose();
            break;
          }
          catch
          {
            this.ImageStream.Position = 0L;
            break;
          }
        case "DCT":
          if (!this.m_imageDictionary.ContainsKey("SMask"))
          {
            if (!this.m_imageDictionary.ContainsKey("Mask"))
            {
              this.ImageStream.Position = 0L;
              if ((this.ColorSpace == "DeviceCMYK" || this.ColorSpace == "DeviceGray" || this.ColorSpace == "Separation" || this.ColorSpace == "DeviceRGB" || this.ColorSpace == "ICCBased" && this.numberOfComponents == 4) && (!(this.ColorSpace == "DeviceRGB") || !this.m_imageDictionary.ContainsKey("DecodeParms") && this.m_imageDictionary.ContainsKey("Decode")))
              {
                if (this.m_imageDictionary.ContainsKey("Decode"))
                {
                  PdfArray image = this.m_imageDictionary["Decode"] as PdfArray;
                  double[] array3;
                  if (this.ColorSpace == "DeviceRGB")
                    array3 = new double[6]
                    {
                      0.0,
                      1.0,
                      0.0,
                      1.0,
                      0.0,
                      1.0
                    };
                  else
                    array3 = new double[8]
                    {
                      1.0,
                      0.0,
                      1.0,
                      0.0,
                      1.0,
                      0.0,
                      1.0,
                      0.0
                    };
                  PdfArray pdfArray = new PdfArray(array3);
                  bool flag4 = true;
                  for (int index21 = 0; index21 < pdfArray.Count; ++index21)
                  {
                    if ((double) (image[index21] as PdfNumber).FloatValue != (double) (pdfArray[index21] as PdfNumber).FloatValue)
                      flag4 = false;
                  }
                  if (flag4)
                    break;
                }
                Bitmap bitmap4 = !this.isDualFilter || this.imageStreamBackup == null ? Image.FromStream(this.ImageStream) as Bitmap : Image.FromStream(this.imageStreamBackup) as Bitmap;
                BitmapData bitmapdata = bitmap4.LockBits(new Rectangle(0, 0, bitmap4.Width, bitmap4.Height), ImageLockMode.ReadWrite, bitmap4.PixelFormat);
                IntPtr scan0 = bitmapdata.Scan0;
                int length7 = Math.Abs(bitmapdata.Stride) * bitmap4.Height;
                byte[] numArray9 = new byte[length7];
                Marshal.Copy(scan0, numArray9, 0, length7);
                if (this.ColorSpace != "DeviceGray")
                {
                  byte[] source = this.YCCKtoRGB(numArray9);
                  Marshal.Copy(source, 0, scan0, source.Length);
                }
                bitmap4.UnlockBits(bitmapdata);
                Image image4 = (Image) bitmap4;
                this.ImageStream = (Stream) new MemoryStream();
                image4.Save(this.ImageStream, ImageFormat.Png);
                this.ImageStream.Position = 0L;
                break;
              }
              break;
            }
          }
          try
          {
            Bitmap input3;
            try
            {
              if (this.m_imageDictionary.ContainsKey("Mask"))
                this.IsTransparent = true;
              this.ImageStream.Position = 0L;
              if (this.ColorSpace == "DeviceCMYK")
              {
                if (this.m_imageDictionary.ContainsKey("Decode"))
                {
                  PdfArray image = this.m_imageDictionary["Decode"] as PdfArray;
                  PdfArray pdfArray = new PdfArray(new double[8]
                  {
                    1.0,
                    0.0,
                    1.0,
                    0.0,
                    1.0,
                    0.0,
                    1.0,
                    0.0
                  });
                  bool flag5 = true;
                  for (int index22 = 0; index22 < pdfArray.Count; ++index22)
                  {
                    if ((double) (image[index22] as PdfNumber).FloatValue != (double) (pdfArray[index22] as PdfNumber).FloatValue)
                      flag5 = false;
                  }
                  if (flag5)
                    break;
                }
                Bitmap bitmap5 = Image.FromStream(this.ImageStream) as Bitmap;
                BitmapData bitmapdata = bitmap5.LockBits(new Rectangle(0, 0, bitmap5.Width, bitmap5.Height), ImageLockMode.ReadWrite, bitmap5.PixelFormat);
                IntPtr scan0 = bitmapdata.Scan0;
                int length8 = Math.Abs(bitmapdata.Stride) * bitmap5.Height;
                byte[] numArray10 = new byte[length8];
                Marshal.Copy(scan0, numArray10, 0, length8);
                byte[] source = this.YCCKtoRGB(numArray10);
                Marshal.Copy(source, 0, scan0, source.Length);
                bitmap5.UnlockBits(bitmapdata);
                Image image5 = (Image) bitmap5;
                this.ImageStream = (Stream) new MemoryStream();
                image5.Save(this.ImageStream, ImageFormat.Png);
                this.ImageStream.Position = 0L;
              }
              this.ImageStream.Position = 0L;
              input3 = Image.FromStream(this.ImageStream) as Bitmap;
            }
            catch
            {
              input3 = (Bitmap) null;
            }
            this.MaskStream.Position = 0L;
            PdfReferenceHolder image6 = this.m_imageDictionary["SMask"] as PdfReferenceHolder;
            if (image6 != (PdfReferenceHolder) null)
            {
              this.m_isSoftMasked = true;
              if (image6.Object is PdfStream pdfStream)
              {
                PdfDictionary pdfDictionary = (PdfDictionary) pdfStream;
                if (pdfDictionary["Filter"] is PdfArray)
                {
                  PdfArray pdfArray = pdfDictionary["Filter"] as PdfArray;
                  string[] strArray = new string[pdfArray.Count];
                  for (int index23 = 0; index23 < strArray.Length; ++index23)
                    strArray[index23] = (pdfArray[index23] as PdfName).Value;
                }
                else
                {
                  string str = (pdfDictionary["Filter"] as PdfName).Value;
                  if (pdfDictionary.ContainsKey("Decode"))
                  {
                    if (str == "FlateDecode")
                      break;
                  }
                }
              }
            }
            this.ImageStream = (Stream) this.MergeImages(input3, this.MaskStream as MemoryStream, true);
            if (!this.m_isSoftMasked)
              this.m_isImageMasked = true;
            this.ImageStream.Position = 0L;
            input3.Dispose();
            break;
          }
          catch
          {
            this.ImageStream.Position = 0L;
            break;
          }
        case "FlateDecode":
          int predictor1 = 0;
          int colors1 = 1;
          int columns1 = 1;
          int bitsPerComponent1 = 0;
          this.outStream = !flag1 ? this.ImageStream as MemoryStream : this.DecodeFlateStream(this.ImageStream as MemoryStream);
          string colorSpace1 = this.ColorSpace;
          byte[] numArray11 = (byte[]) null;
          if (this.IsImageMask && this.ColorSpace == null)
          {
            if (index1 != 0 || this.ImageFilter.Length <= 1 && !this.IsImageForExtraction)
              return (Stream) null;
            if ((double) this.GetBitsPerComponent() != 1.0)
              return (Stream) null;
            this.ColorSpace = "DeviceGray";
          }
          if (this.colorSpaceResourceDict.Count > 0 && this.ColorSpace != "DeviceGray")
          {
            int d = 0;
            int w = 0;
            int h = 0;
            this.isIndexedImage = this.m_colorspace != "ICCBased";
            if (this.m_imageDictionary.ContainsKey("BitsPerComponent"))
              d = (this.m_imageDictionary["BitsPerComponent"] as PdfNumber).IntValue;
            if (this.m_imageDictionary.ContainsKey("Width"))
              w = (this.m_imageDictionary["Width"] as PdfNumber).IntValue;
            if (this.m_imageDictionary.ContainsKey("Height"))
              h = (this.m_imageDictionary["Height"] as PdfNumber).IntValue;
            if (this.colorSpaceResourceDict.ContainsKey("DeviceCMYK") && this.m_colorspaceBase == "DeviceCMYK" && this.m_colorspace == "DeviceN" && this.internalColorSpace == string.Empty && this.m_imageDictionary.ContainsKey("SMask"))
            {
              byte[] rgb = this.ConvertIndexCMYKToRGB(this.colorSpaceResourceDict["DeviceCMYK"].GetBuffer());
              numArray11 = this.ConvertIndexedStreamToFlat(d, w, h, this.outStream.GetBuffer(), rgb, false, false);
            }
            else if (this.colorSpaceResourceDict.ContainsKey("DeviceCMYK") || this.m_colorspace == "Indexed" && this.internalColorSpace == "DeviceN" && this.m_colorspaceBase == "DeviceCMYK")
            {
              byte[] rgb = this.ConvertIndexCMYKToRGB(this.colorSpaceResourceDict["Indexed"].GetBuffer());
              numArray11 = this.ConvertIndexedStreamToFlat(d, w, h, this.outStream.GetBuffer(), rgb, false, false);
            }
            else if (this.isDeviceN)
            {
              byte[] buffer2 = this.colorSpaceResourceDict["Indexed"].GetBuffer();
              byte[] index24 = new byte[768 /*0x0300*/];
              int index25 = 0;
              Color empty = Color.Empty;
              int length9 = buffer2.Length;
              int length10 = 2;
              float[] numArray12 = new float[length10];
              for (int index26 = 0; index26 < length9; index26 += length10)
              {
                for (int index27 = 0; index27 < length10; ++index27)
                  numArray12[index27] = (float) ((int) buffer2[index26 + index27] & (int) byte.MaxValue) / (float) byte.MaxValue;
                float num17 = numArray12[0];
                float num18 = 0.0f;
                float num19 = 0.0f;
                float num20 = numArray12[1];
                float num21 = (float) ((double) byte.MaxValue * (1.0 - (double) num17) * (1.0 - (double) num20));
                float num22 = (float) ((double) byte.MaxValue * (1.0 - (double) num18) * (1.0 - (double) num20));
                float num23 = (float) ((double) byte.MaxValue * (1.0 - (double) num19) * (1.0 - (double) num20));
                index24[index25] = (byte) num21;
                int index28 = index25 + 1;
                index24[index28] = (byte) num22;
                int index29 = index28 + 1;
                index24[index29] = (byte) num23;
                index25 = index29 + 1;
              }
              numArray11 = this.ConvertIndexedStreamToFlat(d, w, h, this.outStream.GetBuffer(), index24, false, false);
            }
            else if (this.colorSpaceResourceDict.ContainsKey("ICCBased") && this.m_colorspaceBase == "DeviceGray")
            {
              numArray11 = this.ConvertICCBasedStreamToFlat(d, w, h, this.outStream.GetBuffer(), this.colorSpaceResourceDict["ICCBased"].GetBuffer(), false, false);
              if (d != 1)
              {
                MemoryStream memoryStream5 = new MemoryStream();
                this.Pixels = this.RenderRGBPixels(numArray11);
                this.m_embeddedImage = (Image) this.RenderImage(this.Pixels);
                this.m_embeddedImage.Save((Stream) memoryStream5, ImageFormat.Png);
                this.ImageStream = (Stream) memoryStream5;
                return this.ImageStream;
              }
            }
            else if (this.m_colorspaceBase != "CalRGB")
            {
              numArray11 = this.ConvertIndexedStreamToFlat(d, w, h, this.outStream.GetBuffer(), this.colorSpaceResourceDict["Indexed"].GetBuffer(), false, false);
              this.m_indexedRGBvalues = this.colorSpaceResourceDict["Indexed"].GetBuffer();
            }
          }
          if (this.ColorSpace == "DeviceGray" || this.ColorSpace == "CalGray")
          {
            this.outStream.Position = 0L;
            if (this.ImageFilter.Length > 1 && index1 == 0 && (this.ImageFilter[index1 + 1] == "DCTDecode" || this.ImageFilter[index1 + 1] == "RunLengthDecode" || this.ImageFilter[index1 + 1] == "CCITTFaxDecode"))
            {
              this.ImageStream = (Stream) this.outStream;
              this.ImageStream.Position = 0L;
              break;
            }
            if (this.ImageDictionary.ContainsKey("SMask") && this.ImageDictionary["SMask"] as PdfReferenceHolder != (PdfReferenceHolder) null)
            {
              PdfStream pdfStream = (this.ImageDictionary["SMask"] as PdfReferenceHolder).Object as PdfStream;
              if (pdfStream.ContainsKey("BitsPerComponent"))
                this.m_bitsPerComponent = (float) (pdfStream["BitsPerComponent"] as PdfNumber).IntValue;
              if (pdfStream.ContainsKey("Width"))
                this.m_width = (float) (pdfStream["Width"] as PdfNumber).IntValue;
              if (pdfStream.ContainsKey("Height"))
                this.m_height = (float) (pdfStream["Height"] as PdfNumber).IntValue;
            }
            this.ImageStream = (Stream) this.DecodeDeviceGrayImage(this.outStream);
            if (this.m_imageDictionary.ContainsKey("SMask"))
            {
              Bitmap input4 = new Bitmap(this.ImageStream);
              try
              {
                this.ImageStream = (Stream) this.MergeImages(input4, this.MaskStream as MemoryStream, false);
                this.m_isSoftMasked = true;
              }
              catch (Exception ex)
              {
                this.ImageStream = (Stream) this.outStream;
              }
              input4.Dispose();
            }
            bool flag6 = false;
            if (this.m_imageDictionary.ContainsKey("DecodeParms"))
            {
              PdfDictionary pdfDictionary1 = new PdfDictionary();
              PdfDictionary pdfDictionary2 = this.DecodeParam[index1];
              if (pdfDictionary2 != null && pdfDictionary2.Count > 0)
              {
                if (pdfDictionary2.ContainsKey("Predictor"))
                  predictor1 = (pdfDictionary2["Predictor"] as PdfNumber).IntValue;
                if (pdfDictionary2.ContainsKey("Columns"))
                  columns1 = (pdfDictionary2["Columns"] as PdfNumber).IntValue;
                if (pdfDictionary2.ContainsKey("Colors"))
                  colors1 = (pdfDictionary2["Colors"] as PdfNumber).IntValue;
                if (pdfDictionary2.ContainsKey("BitsPerComponent"))
                  bitsPerComponent1 = (pdfDictionary2["BitsPerComponent"] as PdfNumber).IntValue;
                if (pdfDictionary2.Count > 0 && predictor1 != 0)
                {
                  if (this.m_colorspaceBase == "CalRGB" && this.ColorSpace == "Indexed")
                  {
                    byte[] encodedData = this.CMYKPredictor(this.outStream.ToArray(), columns1, colors1, bitsPerComponent1);
                    MemoryStream memoryStream6 = new MemoryStream();
                    this.m_embeddedImage = (Image) this.RenderImage(this.GetIndexedPixelData(encodedData));
                    this.m_embeddedImage.Save((Stream) memoryStream6, ImageFormat.Png);
                    this.ImageStream = (Stream) memoryStream6;
                    break;
                  }
                  bool flag7 = true;
                  byte[] buffer3 = this.DecodePredictor(predictor1, colors1, columns1, this.outStream).GetBuffer();
                  Bitmap bitmap6 = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format32bppArgb);
                  Bitmap bitmap7;
                  if (!this.ImageDictionary.ContainsKey("Decode") && flag7 && this.ColorSpace == "DeviceGray" && this.m_colorspaceBase == null && bitsPerComponent1 == 2 && !this.ImageDictionary.ContainsKey("SMask"))
                  {
                    MemoryStream memoryStream7 = new MemoryStream();
                    ColorConvertor[] colorConvertorArray = this.RenderGrayPixels(buffer3);
                    int[] pixelArray = new int[colorConvertorArray.Length];
                    for (int index30 = 0; index30 < pixelArray.Length; ++index30)
                      pixelArray[index30] = colorConvertorArray[index30].PixelConversion();
                    bitmap7 = this.RenderImage(pixelArray);
                    flag6 = false;
                  }
                  else
                    bitmap7 = this.RenderImage(this.GetStencilMaskedPixels(buffer3, this.Width, this.Height));
                  if (this.IsTransparent)
                    bitmap7.MakeTransparent();
                  MemoryStream memoryStream8 = new MemoryStream();
                  bitmap7.Save((Stream) memoryStream8, ImageFormat.Png);
                  this.ImageStream = (Stream) memoryStream8;
                  this.ImageStream.Position = 0L;
                  bitmap7.Dispose();
                  break;
                }
                break;
              }
              break;
            }
            this.ImageStream.Position = 0L;
            break;
          }
          if (this.ImageFilter.Length > 1 && index1 == 0 && (this.ImageFilter[index1 + 1] == "DCTDecode" || this.ImageFilter[index1 + 1] == "RunLengthDecode" || this.ImageFilter[index1 + 1] == "CCITTFaxDecode"))
          {
            this.ImageStream = (Stream) new MemoryStream();
            this.ImageStream = (Stream) this.outStream;
            this.ImageStream.Position = 0L;
            break;
          }
          if (this.m_colorspace == "ICCBased" && this.m_colorspaceBase == "DeviceGray" && this.nonIndexedImageColorResource != null && this.nonIndexedImageColorResource.Count > 0)
          {
            PdfDictionary pdfDictionary3 = (PdfDictionary) this.nonIndexedImageColorResource["ICCBased"];
            PdfName pdfName = pdfDictionary3["Alternate"] as PdfName;
            if (pdfDictionary3["N"] is PdfNumber && (pdfDictionary3["N"] as PdfNumber).IntValue == 1)
            {
              int num24 = 0;
              if (this.m_imageDictionary.ContainsKey("BitsPerComponent"))
                num24 = (this.m_imageDictionary["BitsPerComponent"] as PdfNumber).IntValue;
              if (num24 == 8)
                this.m_pixelFormat = PixelFormat.Format8bppIndexed;
              PdfDictionary pdfDictionary4 = new PdfDictionary();
              if (pdfName == (PdfName) null && this.m_imageDictionary.ContainsKey("DecodeParms"))
              {
                PdfDictionary pdfDictionary5 = this.DecodeParam[index1];
                if (pdfDictionary5 != null)
                {
                  if (pdfDictionary5.ContainsKey("Predictor"))
                    predictor1 = (pdfDictionary5["Predictor"] as PdfNumber).IntValue;
                  if (pdfDictionary5.ContainsKey("Columns"))
                    columns1 = (pdfDictionary5["Columns"] as PdfNumber).IntValue;
                  if (pdfDictionary5.ContainsKey("Colors"))
                    colors1 = (pdfDictionary5["Colors"] as PdfNumber).IntValue;
                  if (pdfDictionary5.ContainsKey("BitsPerComponent"))
                    bitsPerComponent1 = (pdfDictionary5["BitsPerComponent"] as PdfNumber).IntValue;
                  if (pdfDictionary5.Count > 0)
                  {
                    byte[] buffer4 = this.DecodePredictor(predictor1, colors1, columns1, this.outStream).GetBuffer();
                    MemoryStream memoryStream9 = new MemoryStream();
                    ColorConvertor[] colorConvertorArray = this.RenderGrayPixels(buffer4);
                    int[] pixelArray = new int[colorConvertorArray.Length];
                    for (int index31 = 0; index31 < pixelArray.Length; ++index31)
                      pixelArray[index31] = colorConvertorArray[index31].PixelConversion();
                    this.m_embeddedImage = (Image) this.RenderImage(pixelArray);
                    this.m_embeddedImage.Save((Stream) memoryStream9, ImageFormat.Png);
                    this.ImageStream = (Stream) memoryStream9;
                    return this.ImageStream;
                  }
                }
              }
            }
          }
          if (!this.isIndexedImage && !this.m_isIccBasedAlternateDeviceGray)
          {
            if (this.nonIndexedImageColorResource != null && this.nonIndexedImageColorResource.Count > 0)
            {
              PdfDictionary pdfDictionary = (PdfDictionary) this.nonIndexedImageColorResource["ICCBased"];
              if (pdfDictionary["N"] is PdfNumber)
              {
                if ((pdfDictionary["N"] as PdfNumber).IntValue == 1)
                {
                  int num25 = 0;
                  if (this.m_imageDictionary.ContainsKey("BitsPerComponent"))
                    num25 = (this.m_imageDictionary["BitsPerComponent"] as PdfNumber).IntValue;
                  if (num25 == 8)
                    this.m_pixelFormat = PixelFormat.Format8bppIndexed;
                  numArray11 = this.outStream.GetBuffer();
                  for (int index32 = 0; index32 < numArray11.Length; ++index32)
                  {
                    if (numArray11[index32] != (byte) 0 && numArray11[index32] != byte.MaxValue)
                      numArray11[index32] = (byte) 0;
                  }
                }
                else
                  numArray11 = this.outStream.GetBuffer();
              }
              else
                numArray11 = this.outStream.GetBuffer();
            }
            else if (this.ColorSpace == "DeviceCMYK")
            {
              PdfDictionary pdfDictionary6 = new PdfDictionary();
              if (this.m_imageDictionary.ContainsKey("DecodeParms"))
              {
                MemoryStream memoryStream10 = new MemoryStream();
                PdfDictionary pdfDictionary7 = this.DecodeParam[index1];
                if (pdfDictionary7.ContainsKey("Predictor"))
                  predictor1 = (pdfDictionary7["Predictor"] as PdfNumber).IntValue;
                if (pdfDictionary7.ContainsKey("Columns"))
                  columns1 = (pdfDictionary7["Columns"] as PdfNumber).IntValue;
                if (pdfDictionary7.ContainsKey("Colors"))
                  colors1 = (pdfDictionary7["Colors"] as PdfNumber).IntValue;
                bitsPerComponent1 = !pdfDictionary7.ContainsKey("BitsPerComponent") ? (int) this.BitsPerComponent : (pdfDictionary7["BitsPerComponent"] as PdfNumber).IntValue;
                if (predictor1 != 0)
                {
                  this.Pixels = this.m_colorspaceBase != null || bitsPerComponent1 != 8 ? this.CMYKtoRGBPixels(this.CMYKPredictor(this.outStream.GetBuffer(), columns1, colors1, bitsPerComponent1)) : this.CMYKtoRGBPixels(this.DecodePredictor(predictor1, colors1, columns1, this.outStream).GetBuffer());
                  this.m_embeddedImage = (Image) this.RenderImage(this.Pixels);
                  this.m_embeddedImage.Save((Stream) memoryStream10, ImageFormat.Png);
                  this.ImageStream = (Stream) memoryStream10;
                  break;
                }
                numArray11 = this.YCCToRGB(this.outStream.GetBuffer());
                this.outStream = new MemoryStream(numArray11);
              }
              else
              {
                numArray11 = this.YCCToRGB(this.outStream.ToArray());
                this.outStream = new MemoryStream(numArray11);
              }
            }
            else
            {
              if (this.ColorSpace == "DeviceRGB" && !this.ImageDictionary.ContainsKey("Type") && !this.ImageDictionary.ContainsKey("SMask") && !this.ImageDictionary.ContainsKey("DecodeParms"))
              {
                using (Bitmap bitmap8 = this.RenderImage(this.RenderRGBPixels(this.outStream.ToArray())))
                {
                  this.ImageStream = (Stream) new MemoryStream();
                  bitmap8.Save(this.ImageStream, ImageFormat.Png);
                  this.ImageStream.Position = 0L;
                  break;
                }
              }
              numArray11 = this.outStream.ToArray();
              if (this.ImageDictionary.ContainsKey("SMask") && !this.ImageDictionary.ContainsKey("DecodeParms"))
              {
                MemoryStream memoryStream11 = new MemoryStream();
                this.Pixels = this.RenderRGBPixels(numArray11);
                this.m_embeddedImage = (Image) this.RenderImage(this.Pixels);
                this.m_embeddedImage.Save((Stream) memoryStream11, ImageFormat.Png);
                this.m_embeddedImage.Dispose();
                this.ImageStream = (Stream) memoryStream11;
                this.m_isSoftMasked = true;
                this.Pixels = (int[]) null;
                break;
              }
            }
          }
          if (this.ImageDictionary.ContainsKey("Mask"))
          {
            this.IsTransparent = true;
            if (this.ImageDictionary["Mask"] is PdfArray)
            {
              PdfArray image = this.ImageDictionary["Mask"] as PdfArray;
              int index33 = 0;
              while (index33 < image.Count)
              {
                int num26 = 0;
                int num27 = 0;
                if (image[index33] is PdfNumber)
                {
                  num27 = (image[index33] as PdfNumber).IntValue;
                  ++index33;
                }
                if (image[index33] is PdfNumber)
                {
                  num26 = (image[index33] as PdfNumber).IntValue;
                  ++index33;
                }
                for (int index34 = 0; index34 < numArray11.Length; ++index34)
                {
                  if ((int) numArray11[index34] >= num27 && (int) numArray11[index34] <= num26)
                    numArray11[index34] = byte.MaxValue;
                }
              }
            }
          }
          if (this.ColorSpace == "Indexed" && !this.isDeviceN)
          {
            PdfDictionary pdfDictionary8 = new PdfDictionary();
            if (this.ImageDictionary.ContainsKey("DecodeParms"))
            {
              PdfDictionary pdfDictionary9 = this.DecodeParam[index1];
              if (pdfDictionary9 != null)
              {
                if (pdfDictionary9.ContainsKey("Predictor"))
                  predictor1 = (pdfDictionary9["Predictor"] as PdfNumber).IntValue;
                if (pdfDictionary9.ContainsKey("Columns"))
                  columns1 = (pdfDictionary9["Columns"] as PdfNumber).IntValue;
                if (pdfDictionary9.ContainsKey("Colors"))
                  colors1 = (pdfDictionary9["Colors"] as PdfNumber).IntValue;
                bitsPerComponent1 = !pdfDictionary9.ContainsKey("BitsPerComponent") ? (int) this.BitsPerComponent : (pdfDictionary9["BitsPerComponent"] as PdfNumber).IntValue;
                if (pdfDictionary9.Count > 0)
                  this.outStream = new MemoryStream(this.CMYKPredictor(this.outStream.ToArray(), columns1, colors1, bitsPerComponent1));
              }
            }
            if (flag1)
            {
              using (Bitmap input5 = this.RenderImage(this.GetIndexedPixelData(this.outStream.ToArray())))
              {
                if (!this.m_imageDictionary.ContainsKey("SMask"))
                {
                  if (this.m_imageDictionary.ContainsKey("Mask"))
                  {
                    if (!this.ImageDictionary.ContainsKey("DecodeParms"))
                    {
                      if (this.ImageDictionary.ContainsKey("BitsPerComponent"))
                      {
                        try
                        {
                          this.ImageStream = (Stream) this.MergeImages(input5, this.MaskStream as MemoryStream, false);
                          return this.ImageStream;
                        }
                        catch (Exception ex)
                        {
                          input5.MakeTransparent();
                          goto label_333;
                        }
                      }
                    }
                    input5.MakeTransparent();
                  }
label_333:
                  MemoryStream memoryStream12 = new MemoryStream();
                  input5.Save((Stream) memoryStream12, ImageFormat.Png);
                  this.ImageStream = (Stream) memoryStream12;
                  this.ImageStream.Position = 0L;
                  this.Pixels = (int[]) null;
                  break;
                }
                if (this.m_imageDictionary.ContainsKey("SMask"))
                {
                  this.ImageStream = (Stream) this.MergeImages(input5, this.MaskStream as MemoryStream, false);
                  return this.ImageStream;
                }
              }
            }
            else if (this.m_imageDictionary.ContainsKey("SMask"))
            {
              PdfStream pdfStream = (this.ImageDictionary["SMask"] as PdfReferenceHolder).Object as PdfStream;
              if (pdfStream.ContainsKey("BitsPerComponent"))
                this.m_bitsPerComponent = (float) (pdfStream["BitsPerComponent"] as PdfNumber).IntValue;
              if (pdfStream.ContainsKey("Width"))
                this.m_width = (float) (pdfStream["Width"] as PdfNumber).IntValue;
              if (pdfStream.ContainsKey("Height"))
                this.m_height = (float) (pdfStream["Height"] as PdfNumber).IntValue;
              if (pdfStream.ContainsKey("ColorSpace"))
                this.m_colorspace = (pdfStream["ColorSpace"] as PdfName).Value;
              byte[] data = pdfStream.Data;
              MemoryStream memoryStream13 = new MemoryStream();
              this.Pixels = this.RenderRGBPixels(data);
              this.m_embeddedImage = (Image) this.RenderImage(this.Pixels);
              this.m_embeddedImage.Save((Stream) memoryStream13, ImageFormat.Png);
              this.ImageStream = (Stream) memoryStream13;
              this.m_isSoftMasked = true;
              break;
            }
          }
          if ((this.ColorSpace == "DeviceN" || this.ColorSpace == "Separation") && (!this.colorSpaceResourceDict.ContainsKey("DeviceCMYK") || !(this.m_colorspaceBase == "DeviceCMYK") || !(this.m_colorspace == "DeviceN")))
            numArray11 = this.GetDeviceNData(numArray11);
          PdfDictionary pdfDictionary10 = new PdfDictionary();
          Bitmap input6 = new Bitmap((int) this.Width, (int) this.Height, this.m_pixelFormat);
          BitmapData bitmapdata1 = input6.LockBits(new Rectangle(0, 0, input6.Width, input6.Height), ImageLockMode.ReadWrite, input6.PixelFormat);
          if ((this.m_pixelFormat == PixelFormat.Format8bppIndexed || this.m_pixelFormat == PixelFormat.Format4bppIndexed) && this.m_colorPalette != null)
            input6.Palette = this.m_colorPalette;
          int num28 = Image.GetPixelFormatSize(input6.PixelFormat) / 8;
          if (this.m_imageDictionary.ContainsKey("DecodeParms") && this.DecodeParam != null && this.DecodeParam.Length > index1)
          {
            PdfDictionary pdfDictionary11 = this.DecodeParam[index1];
            if (pdfDictionary11 != null)
            {
              if (pdfDictionary11.ContainsKey("Predictor"))
                predictor1 = (pdfDictionary11["Predictor"] as PdfNumber).IntValue;
              if (pdfDictionary11.ContainsKey("Columns"))
                columns1 = (pdfDictionary11["Columns"] as PdfNumber).IntValue;
              if (pdfDictionary11.ContainsKey("Colors"))
                colors1 = (pdfDictionary11["Colors"] as PdfNumber).IntValue;
              if (pdfDictionary11.ContainsKey("BitsPerComponent"))
                bitsPerComponent1 = (pdfDictionary11["BitsPerComponent"] as PdfNumber).IntValue;
              if (pdfDictionary11.Count > 0)
              {
                if (this.m_colorspaceBase == "CalRGB" && this.ColorSpace == "Indexed")
                {
                  byte[] encodedData = this.CMYKPredictor(this.outStream.ToArray(), columns1, colors1, bitsPerComponent1);
                  MemoryStream memoryStream14 = new MemoryStream();
                  this.m_embeddedImage = (Image) this.RenderImage(this.GetIndexedPixelData(encodedData));
                  this.m_embeddedImage.Save((Stream) memoryStream14, ImageFormat.Png);
                  this.ImageStream = (Stream) memoryStream14;
                  break;
                }
                numArray11 = this.DecodePredictor(predictor1, colors1, columns1, this.outStream).GetBuffer();
              }
            }
          }
          num1 = num28;
          switch (num1)
          {
            case 3:
              for (int index35 = 0; index35 + 3 <= numArray11.Length; index35 += 3)
              {
                int index36 = index35 + 2;
                byte num29 = numArray11[index36];
                numArray11[index36] = numArray11[index35];
                numArray11[index35] = num29;
              }
              break;
            case 4:
              byte[] numArray13 = new byte[(int) ((double) this.Width * (double) this.Height * 3.0)];
              int num30 = (int) ((double) this.Width * (double) this.Height * 4.0);
              byte[] numArray14 = numArray11;
              int index37 = 0;
              int index38 = 0;
              for (; index37 < num30 + 4; index37 += 4)
              {
                numArray13[index38 + 2] = numArray14[index37];
                numArray13[index38 + 1] = numArray14[index37 + 1];
                numArray13[index38] = numArray14[index37 + 2];
                index38 += 3;
              }
              numArray11 = numArray13;
              break;
          }
          int startIndex1 = 0;
          long int64_1 = bitmapdata1.Scan0.ToInt64();
          int length11 = (int) this.Width;
          if (num28 == 3)
            length11 = (int) this.Width * 3;
          if (this.m_pixelFormat == PixelFormat.Format4bppIndexed)
          {
            for (int index39 = 0; (double) index39 < (double) this.Height / 2.0; ++index39)
            {
              if (index39 % 2 == 0)
              {
                Marshal.Copy(numArray11, startIndex1, new IntPtr(int64_1), length11);
                int64_1 += (long) bitmapdata1.Stride;
              }
              if (index39 % 3 == 0)
              {
                Marshal.Copy(numArray11, startIndex1, new IntPtr(int64_1), length11);
                int64_1 += (long) bitmapdata1.Stride;
              }
              if (index39 % 7 == 0)
              {
                Marshal.Copy(numArray11, startIndex1, new IntPtr(int64_1), length11);
                int64_1 += (long) bitmapdata1.Stride;
              }
              if (bitmapdata1.Scan0.ToInt64() >= int64_1)
              {
                Marshal.Copy(numArray11, startIndex1, new IntPtr(int64_1), length11);
                int64_1 += (long) bitmapdata1.Stride;
              }
              startIndex1 += length11;
            }
          }
          else
          {
            for (int index40 = 0; (double) index40 < (double) this.Height; ++index40)
            {
              Marshal.Copy(numArray11, startIndex1, new IntPtr(int64_1), length11);
              startIndex1 += length11;
              int64_1 += (long) bitmapdata1.Stride;
            }
          }
          input6.UnlockBits(bitmapdata1);
          if (this.IsTransparent)
            input6.MakeTransparent();
          MemoryStream memoryStream15 = new MemoryStream();
          input6.Save((Stream) memoryStream15, ImageFormat.Png);
          if (!this.m_imageDictionary.ContainsKey("SMask"))
          {
            this.ImageStream = (Stream) memoryStream15;
            this.ImageStream.Position = 0L;
          }
          else
          {
            try
            {
              this.ImageStream = (Stream) this.MergeImages(input6, this.MaskStream as MemoryStream, false);
              this.m_isSoftMasked = true;
              this.ImageStream.Position = 0L;
            }
            catch (Exception ex)
            {
              this.ImageStream = (Stream) memoryStream15;
              this.ImageStream.Position = 0L;
            }
          }
          input6.Dispose();
          break;
        case "CCITTFaxDecode":
          if (this.ColorSpace == "Indexed" && this.m_colorspaceBase == "DeviceRGB")
            this.m_isBlackIs1 = true;
          PdfDictionary decodeParams = new PdfDictionary();
          if (this.m_imageDictionary.ContainsKey("DecodeParms"))
            decodeParams = this.DecodeParam[index1];
          this.ImageStream = !this.isDualFilter || this.imageStreamBackup == null ? (Stream) this.DecodeCCITTFaxDecodeStream(this.ImageStream as MemoryStream, this.m_imageDictionary, decodeParams) : (Stream) this.DecodeCCITTFaxDecodeStream(this.imageStreamBackup as MemoryStream, this.m_imageDictionary, decodeParams);
          byte[] buffer5 = (this.ImageStream as MemoryStream).GetBuffer();
          string colorSpace2 = this.ColorSpace;
          if (this.m_isIccBasedAlternateDeviceGray)
          {
            byte[] numArray15 = buffer5;
            int height = (int) this.Height;
            int width3 = (int) this.Width;
            byte[] numArray16 = new byte[width3 * height];
            int[] numArray17 = new int[8]
            {
              1,
              2,
              4,
              8,
              16 /*0x10*/,
              32 /*0x20*/,
              64 /*0x40*/,
              128 /*0x80*/
            };
            int num31 = (int) this.Width + 7 >> 3;
            int num32 = 1;
            try
            {
              for (int index41 = 0; index41 < height; ++index41)
              {
                for (int index42 = 0; index42 < width3; ++index42)
                {
                  int num33 = 0;
                  int num34 = 0;
                  int num35 = num32;
                  int num36 = num32;
                  int num37 = (int) this.Width - index42;
                  int num38 = (int) this.Height - index41;
                  if (num35 > num37)
                    num35 = num37;
                  if (num36 > num38)
                    num36 = num38;
                  for (int index43 = 0; index43 < num36; ++index43)
                  {
                    for (int index44 = 0; index44 < num35; ++index44)
                    {
                      if (((int) numArray15[(index43 + index41 * num32) * num31 + (index42 * num32 + index44 >> 3)] & numArray17[7 - (index42 * num32 + index44 & 7)]) != 0)
                        ++num33;
                      ++num34;
                    }
                  }
                  int index45 = index42 + width3 * index41;
                  numArray16[index45] = (byte) ((int) byte.MaxValue * num33 / num34);
                }
              }
            }
            catch
            {
            }
            byte[] source = numArray16;
            this.m_pixelFormat = PixelFormat.Format8bppIndexed;
            Bitmap bitmap9 = new Bitmap((int) this.Width, (int) this.Height, this.m_pixelFormat);
            BitmapData bitmapdata2 = bitmap9.LockBits(new Rectangle(0, 0, bitmap9.Width, bitmap9.Height), ImageLockMode.ReadWrite, bitmap9.PixelFormat);
            int startIndex2 = 0;
            long int64_2 = bitmapdata2.Scan0.ToInt64();
            int width4 = (int) this.Width;
            for (int index46 = 0; (double) index46 < (double) this.Height; ++index46)
            {
              Marshal.Copy(source, startIndex2, new IntPtr(int64_2), width4);
              startIndex2 += width4;
              int64_2 += (long) bitmapdata2.Stride;
            }
            bitmap9.UnlockBits(bitmapdata2);
            MemoryStream memoryStream16 = new MemoryStream();
            bitmap9.Save((Stream) memoryStream16, ImageFormat.Png);
            this.ImageStream = (Stream) memoryStream16;
            this.ImageStream.Position = 0L;
            break;
          }
          break;
        case "JBIG2Decode":
          Bitmap bitmap10 = this.DecodeJBIG2EncodedStream(this.ImageStream as MemoryStream);
          MemoryStream memoryStream17 = new MemoryStream();
          if (this.ImageDictionary.ContainsKey("ImageMask") && !this.IsImageForExtraction)
          {
            if (this.ImageDictionary.ContainsKey("NuanRGB"))
              bitmap10.MakeTransparent(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
            else
              bitmap10.MakeTransparent();
          }
          bitmap10.Save((Stream) memoryStream17, ImageFormat.Png);
          memoryStream17.Position = 0L;
          this.ImageStream = (Stream) memoryStream17;
          this.ImageStream.Position = 0L;
          bitmap10.Dispose();
          break;
        case "LZWDecode":
          int predictor2 = 0;
          int colors2 = 1;
          int columns2 = 1;
          PdfLzwCompressor pdfLzwCompressor = new PdfLzwCompressor();
          MemoryStream memoryStream18 = new MemoryStream();
          byte[] numArray18;
          if (this.isDualFilter && this.imageStreamBackup != null)
          {
            numArray18 = new byte[this.imageStreamBackup.Length];
            this.inputData = (this.imageStreamBackup as MemoryStream).GetBuffer();
          }
          else
            numArray18 = new byte[this.ImageStream.Length];
          if (this.inputData == null)
            this.inputData = (this.ImageStream as MemoryStream).GetBuffer();
          byte[] numArray19 = pdfLzwCompressor.Decompress(this.inputData, this.IsEarlyChange);
          if (this.isDualFilter)
          {
            Stream stream = (Stream) new MemoryStream();
            stream.Write(numArray19, 0, numArray19.Length);
            this.imageStreamBackup = stream;
          }
          if (this.ColorSpace == "Indexed")
          {
            MemoryStream memoryStream19 = new MemoryStream();
            PdfDictionary pdfDictionary12 = new PdfDictionary();
            if (this.ImageDictionary.ContainsKey("DecodeParms"))
            {
              PdfDictionary pdfDictionary13 = this.DecodeParam[index1];
              if (pdfDictionary13 != null)
              {
                if (pdfDictionary13.ContainsKey("Predictor"))
                {
                  int intValue = (pdfDictionary13["Predictor"] as PdfNumber).IntValue;
                }
                if (pdfDictionary13.ContainsKey("Columns"))
                  columns2 = (pdfDictionary13["Columns"] as PdfNumber).IntValue;
                if (pdfDictionary13.ContainsKey("Colors"))
                  colors2 = (pdfDictionary13["Colors"] as PdfNumber).IntValue;
                int bitsPerComponent2 = !pdfDictionary13.ContainsKey("BitsPerComponent") ? (int) this.BitsPerComponent : (pdfDictionary13["BitsPerComponent"] as PdfNumber).IntValue;
                if (pdfDictionary13.Count > 0)
                  this.outStream = new MemoryStream(this.CMYKPredictor(this.outStream.ToArray(), columns2, colors2, bitsPerComponent2));
              }
            }
            Bitmap input7 = this.RenderImage(this.GetIndexedPixelData(numArray19));
            if (!this.m_imageDictionary.ContainsKey("SMask"))
            {
              if (this.ImageDictionary.ContainsKey("Intent") && (this.ImageDictionary["Intent"] as PdfName).Value == "Perceptual")
                input7.MakeTransparent();
              if (this.m_imageDictionary.ContainsKey("Mask") && !(this.m_imageDictionary["Mask"] is PdfArray))
                input7.MakeTransparent();
              input7.Save((Stream) memoryStream19, ImageFormat.Png);
              this.ImageStream = (Stream) memoryStream19;
              this.ImageStream.Position = 0L;
              break;
            }
            try
            {
              this.ImageStream = (Stream) this.MergeImages(input7, this.MaskStream as MemoryStream, false);
              this.m_isSoftMasked = true;
              this.ImageStream.Position = 0L;
              break;
            }
            catch (Exception ex)
            {
              input7.Save((Stream) memoryStream19, ImageFormat.Png);
              this.ImageStream = (Stream) memoryStream19;
              this.ImageStream.Position = 0L;
              break;
            }
          }
          else
          {
            if (this.ColorSpace == "DeviceRGB")
            {
              PdfDictionary pdfDictionary14 = new PdfDictionary();
              if (this.m_imageDictionary.ContainsKey("DecodeParms"))
              {
                PdfDictionary pdfDictionary15 = this.DecodeParam[index1];
                if (pdfDictionary15.ContainsKey("Predictor"))
                  predictor2 = (pdfDictionary15["Predictor"] as PdfNumber).IntValue;
                if (pdfDictionary15.ContainsKey("Columns"))
                  columns2 = (pdfDictionary15["Columns"] as PdfNumber).IntValue;
                if (pdfDictionary15.ContainsKey("Colors"))
                  colors2 = (pdfDictionary15["Colors"] as PdfNumber).IntValue;
                int num39 = !pdfDictionary15.ContainsKey("BitsPerComponent") ? (int) this.BitsPerComponent : (pdfDictionary15["BitsPerComponent"] as PdfNumber).IntValue;
                if (predictor2 != 0 && this.m_colorspaceBase == null && num39 == 8)
                {
                  MemoryStream data = new MemoryStream();
                  data.Write(numArray19, 0, numArray19.Length);
                  numArray19 = this.DecodePredictor(predictor2, colors2, columns2, data).GetBuffer();
                }
              }
              MemoryStream memoryStream20 = new MemoryStream();
              this.Pixels = this.RenderRGBPixels(numArray19);
              this.m_embeddedImage = (Image) this.RenderImage(this.Pixels);
              this.m_embeddedImage.Save((Stream) memoryStream20, ImageFormat.Png);
              this.ImageStream = (Stream) memoryStream20;
              break;
            }
            if (this.ColorSpace == "DeviceGray")
            {
              MemoryStream memoryStream21 = new MemoryStream();
              ColorConvertor[] colorConvertorArray = this.RenderGrayPixels(numArray19);
              int[] pixelArray = new int[colorConvertorArray.Length];
              for (int index47 = 0; index47 < pixelArray.Length; ++index47)
                pixelArray[index47] = colorConvertorArray[index47].PixelConversion();
              this.m_embeddedImage = (Image) this.RenderImage(pixelArray);
              this.m_embeddedImage.Save((Stream) memoryStream21, ImageFormat.Png);
              this.ImageStream = (Stream) memoryStream21;
              break;
            }
            if (this.ColorSpace == "DeviceCMYK")
            {
              MemoryStream memoryStream22 = new MemoryStream();
              this.Pixels = this.CMYKtoRGBPixels(numArray19);
              this.m_embeddedImage = (Image) this.RenderImage(this.Pixels);
              this.m_embeddedImage.Save((Stream) memoryStream22, ImageFormat.Png);
              this.ImageStream = (Stream) memoryStream22;
              break;
            }
            break;
          }
        case "JPXDecode":
          this.ImageStream.Position = 0L;
          Bitmap input8 = (Bitmap) new JPXImage().FromStream(this.ImageStream);
          MemoryStream imageStream = new MemoryStream();
          input8.Save((Stream) imageStream, ImageFormat.Jpeg);
          if (this.m_imageDictionary.ContainsKey("SMask") || this.m_imageDictionary.ContainsKey("Mask"))
          {
            this.MaskStream.Position = 0L;
            this.ImageStream = (Stream) this.MergeImages(input8, this.MaskStream as MemoryStream, false);
            this.m_isSoftMasked = true;
            input8.Dispose();
            return this.ImageStream;
          }
          input8.Dispose();
          return (Stream) imageStream;
        default:
          if (string.IsNullOrEmpty(this.ImageFilter[index1]))
            throw new Exception("Error in identifying ImageFilter");
          throw new Exception(this.ImageFilter.ToString() + " does not supported");
      }
    }
    this.m_imageFilter = (string[]) null;
    return this.ImageStream;
  }

  private byte[] CMYKPredictor(byte[] data, int columns, int colors, int bitsPerComponent)
  {
    List<byte[]> numArrayList = new List<byte[]>();
    int length = (int) Math.Ceiling((double) (columns * colors * bitsPerComponent) / 8.0);
    int sub = (int) Math.Ceiling((double) (bitsPerComponent * colors) / 8.0);
    byte[] prevLine = (byte[]) null;
    ByteStreamRenderer byteStreamRenderer = new ByteStreamRenderer(data);
    while (byteStreamRenderer.Remaining >= length + 1)
    {
      int num = (int) byteStreamRenderer.ReadByte() & (int) byte.MaxValue;
      byte[] numArray = new byte[length];
      byteStreamRenderer.Read(numArray);
      switch (num)
      {
        case 1:
          ImageStructure.Sub(numArray, sub);
          break;
        case 2:
          this.Up(numArray, prevLine);
          break;
        case 3:
          this.Average(numArray, prevLine, sub);
          break;
        case 4:
          this.Paeth(numArray, prevLine, sub);
          break;
      }
      numArrayList.Add(numArray);
      prevLine = numArray;
    }
    List<byte> byteList = new List<byte>();
    foreach (byte[] collection in numArrayList)
      byteList.AddRange((IEnumerable<byte>) collection);
    return byteList.ToArray();
  }

  private Bitmap RenderImage(int[] pixelArray)
  {
    Bitmap bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format32bppArgb);
    BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
    byte[] source = new byte[bitmapdata.Stride * bitmap.Height];
    for (int index = 0; index < pixelArray.Length; ++index)
    {
      Color color = Color.FromArgb(pixelArray[index]);
      int num1 = index % (int) this.Width;
      int num2 = index / (int) this.Width;
      source[bitmapdata.Stride * num2 + 4 * num1] = color.B;
      source[bitmapdata.Stride * num2 + 4 * num1 + 1] = color.G;
      source[bitmapdata.Stride * num2 + 4 * num1 + 2] = color.R;
      source[bitmapdata.Stride * num2 + 4 * num1 + 3] = color.A;
    }
    Marshal.Copy(source, 0, bitmapdata.Scan0, source.Length);
    bitmap.UnlockBits(bitmapdata);
    return bitmap;
  }

  private Bitmap RenderImage(byte[] pixelArray)
  {
    Bitmap bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format32bppArgb);
    BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
    byte[] source = new byte[bitmapdata.Stride * bitmap.Height];
    long index1 = 0;
    for (int index2 = 0; index2 < pixelArray.Length / 4; ++index2)
    {
      int num1 = index2 % (int) this.Width;
      int num2 = index2 / (int) this.Width;
      source[bitmapdata.Stride * num2 + 4 * num1] = pixelArray[index1 + 3L];
      source[bitmapdata.Stride * num2 + 4 * num1 + 1] = pixelArray[index1 + 2L];
      source[bitmapdata.Stride * num2 + 4 * num1 + 2] = pixelArray[index1 + 1L];
      source[bitmapdata.Stride * num2 + 4 * num1 + 3] = pixelArray[index1];
      index1 += 4L;
    }
    Marshal.Copy(source, 0, bitmapdata.Scan0, source.Length);
    bitmap.UnlockBits(bitmapdata);
    return bitmap;
  }

  private byte[] GetDeviceNData(byte[] data)
  {
    byte[] numArray1 = data;
    byte[] deviceNdata = new byte[3 * (int) this.Width * (int) this.Height];
    int index1 = 0;
    Color empty = Color.Empty;
    int length1 = numArray1.Length;
    int length2 = 1;
    float[] numArray2 = new float[length2];
    for (int index2 = 0; index2 < length1; index2 += length2)
    {
      for (int index3 = 0; index3 < length2; ++index3)
        numArray2[index3] = (float) ((int) numArray1[index2 + index3] & (int) byte.MaxValue) / (float) byte.MaxValue;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      float num4 = numArray2[0];
      float num5 = (float) ((double) byte.MaxValue * (1.0 - (double) num1) * (1.0 - (double) num4));
      float num6 = (float) ((double) byte.MaxValue * (1.0 - (double) num2) * (1.0 - (double) num4));
      float num7 = (float) ((double) byte.MaxValue * (1.0 - (double) num3) * (1.0 - (double) num4));
      deviceNdata[index1] = (byte) num5;
      int index4 = index1 + 1;
      deviceNdata[index4] = (byte) num6;
      int index5 = index4 + 1;
      deviceNdata[index5] = (byte) num7;
      index1 = index5 + 1;
    }
    return deviceNdata;
  }

  private byte[] ConvertIndexCMYKToRGB(byte[] data)
  {
    int length = data.Length;
    byte[] rgb1 = new byte[length * 3 / 4];
    int index1 = 0;
    for (int index2 = 0; index2 < length; index2 += 4)
    {
      float[] values = new float[4];
      for (int index3 = 0; index3 < 4; ++index3)
        values[index3] = (float) ((int) data[index2 + index3] & (int) byte.MaxValue) / (float) byte.MaxValue;
      float[] rgb2 = this.ConvertCMYKToRGB(values);
      rgb1[index1] = (byte) (int) rgb2[0];
      rgb1[index1 + 1] = (byte) (int) rgb2[1];
      rgb1[index1 + 2] = (byte) (int) rgb2[2];
      index1 += 3;
      if (length - 4 - index2 < 4)
        index2 = length;
    }
    return rgb1;
  }

  private float[] ConvertCMYKToRGB(float[] values)
  {
    float num1 = values[0];
    float num2 = values[1];
    float num3 = values[2];
    float num4 = values[3];
    return new float[3]
    {
      (float) ((double) byte.MaxValue * (1.0 - (double) num1) * (1.0 - (double) num4)),
      (float) ((double) byte.MaxValue * (1.0 - (double) num2) * (1.0 - (double) num4)),
      (float) ((double) byte.MaxValue * (1.0 - (double) num3) * (1.0 - (double) num4))
    };
  }

  private void GetColorSpace()
  {
    if (!this.m_imageDictionary.ContainsKey("ColorSpace"))
      return;
    string[] filter = (string[]) null;
    this.internalColorSpace = string.Empty;
    PdfDictionary colorspaceDictionary = (PdfDictionary) null;
    PdfArray pdfArray1 = (PdfArray) null;
    if (this.m_imageDictionary["ColorSpace"] is PdfArray)
      pdfArray1 = this.m_imageDictionary["ColorSpace"] as PdfArray;
    if ((object) (this.m_imageDictionary["ColorSpace"] as PdfReferenceHolder) != null)
    {
      if ((this.m_imageDictionary["ColorSpace"] as PdfReferenceHolder).Object is PdfArray)
        pdfArray1 = (this.m_imageDictionary["ColorSpace"] as PdfReferenceHolder).Object as PdfArray;
      else if ((this.m_imageDictionary["ColorSpace"] as PdfReferenceHolder).Object.ToString() != null)
      {
        string str = (this.m_imageDictionary["ColorSpace"] as PdfReferenceHolder).Object.ToString();
        for (int index = 1; index < str.Length; ++index)
          this.m_colorspace += (string) (object) str[index];
      }
    }
    if ((object) (this.m_imageDictionary["ColorSpace"] as PdfName) != null)
      this.m_colorspace = (this.m_imageDictionary["ColorSpace"] as PdfName).Value;
    if (pdfArray1 == null)
      return;
    this.m_colorspace = (pdfArray1[0] as PdfName).Value;
    PdfArray pdfArray2 = pdfArray1;
    if (pdfArray1.Count == 4)
    {
      bool flag = false;
      foreach (string str in this.ImageFilter)
      {
        if (str == "RunLengthDecode")
          flag = true;
      }
      if ((pdfArray2[0] as PdfName).Value == "Indexed")
      {
        if (pdfArray2[pdfArray2.Count - 1].GetType().Name == "PdfReferenceHolder")
        {
          try
          {
            if (((pdfArray2[pdfArray2.Count - 1] as PdfReferenceHolder).Object as PdfDictionary).Values.Count > 1)
            {
              PdfName pdfName = new PdfName();
              MemoryStream internalStream = ((pdfArray2[pdfArray2.Count - 1] as PdfReferenceHolder).Object as PdfStream).InternalStream;
              if (((pdfArray2[pdfArray2.Count - 1] as PdfReferenceHolder).Object as PdfStream).ContainsKey("Filter"))
                pdfName = ((pdfArray2[pdfArray2.Count - 1] as PdfReferenceHolder).Object as PdfStream)["Filter"] as PdfName;
              MemoryStream memoryStream;
              if (pdfName != (PdfName) null && pdfName.Value == "ASCII85Decode")
              {
                memoryStream = this.DecodeASCII85Stream(internalStream);
                this.m_colorspaceStream = memoryStream;
              }
              else
              {
                memoryStream = this.DecodeFlateStream(internalStream);
                this.m_colorspaceStream = memoryStream;
              }
              this.colorSpaceResourceDict.Add("Indexed", memoryStream);
              this.isIndexedImage = true;
            }
            else
            {
              MemoryStream internalStream = ((pdfArray2[pdfArray2.Count - 1] as PdfReferenceHolder).Object as PdfStream).InternalStream;
              this.colorSpaceResourceDict.Add("Indexed", internalStream);
              this.m_colorspaceStream = internalStream;
              if (this.ImageDictionary.ContainsKey("DecodeParms") || flag)
                this.GetIndexedColorSpace(pdfArray1, colorspaceDictionary, filter);
              this.isIndexedImage = true;
            }
            if (pdfArray2[pdfArray2.Count - 3].GetType().Name == "PdfName")
            {
              this.m_colorspaceBase = (pdfArray2[pdfArray2.Count - 3] as PdfName).Value;
              this.colorSpaceResourceDict.Add(this.m_colorspaceBase, new MemoryStream());
              if (this.m_colorspaceBase == "DeviceGray")
                this.m_colorspace = this.m_colorspaceBase;
            }
            else if ((object) (pdfArray2[pdfArray2.Count - 3] as PdfReferenceHolder) != null)
            {
              PdfArray pdfArray3 = (pdfArray2[pdfArray2.Count - 3] as PdfReferenceHolder).Object as PdfArray;
              if ((object) (pdfArray3[0] as PdfName) != null)
              {
                if ((pdfArray3[0] as PdfName).Value == "DeviceN")
                  this.isDeviceN = true;
                this.m_colorspaceBase = (pdfArray3[0] as PdfName).Value;
              }
              if (pdfArray3.Count >= 2 && (object) (pdfArray3[1] as PdfReferenceHolder) != null && (pdfArray3[1] as PdfReferenceHolder).Object is PdfDictionary)
              {
                PdfDictionary pdfDictionary = (pdfArray3[1] as PdfReferenceHolder).Object as PdfDictionary;
                if (pdfDictionary.ContainsKey("N"))
                {
                  this.numberOfComponents = (pdfDictionary["N"] as PdfNumber).IntValue;
                  if (this.numberOfComponents == 3)
                    this.m_colorspaceBase = "DeviceRGB";
                  else if (this.numberOfComponents == 4)
                    this.m_colorspaceBase = "DeviceCMYK";
                  else if (this.numberOfComponents == 1)
                    this.m_colorspaceBase = "DeviceGray";
                }
              }
              if (pdfArray3.Count > 2 && (object) (pdfArray3[2] as PdfName) != null && (pdfArray3[2] as PdfName).Value == "DeviceCMYK")
                this.m_colorspaceBase = "DeviceCMYK";
            }
            else if (pdfArray2[pdfArray2.Count - 3] is PdfArray)
            {
              PdfArray pdfArray4 = pdfArray2[pdfArray2.Count - 3] as PdfArray;
              if ((object) (pdfArray4[0] as PdfName) != null && (pdfArray4[0] as PdfName).Value == "DeviceN")
                this.isDeviceN = true;
              if ((pdfArray4[1] as PdfReferenceHolder).Object is PdfDictionary)
              {
                PdfDictionary pdfDictionary = (pdfArray4[1] as PdfReferenceHolder).Object as PdfDictionary;
                if (pdfDictionary.ContainsKey("N"))
                {
                  this.numberOfComponents = (pdfDictionary["N"] as PdfNumber).IntValue;
                  if (this.numberOfComponents == 3)
                    this.m_colorspaceBase = "DeviceRGB";
                  else if (this.numberOfComponents == 4)
                    this.m_colorspaceBase = "DeviceCMYK";
                  else if (this.numberOfComponents == 1)
                    this.m_colorspaceBase = "DeviceGray";
                }
              }
            }
            if (!(pdfArray2[pdfArray2.Count - 2].GetType().Name == "PdfNumber"))
              return;
            this.m_colorspaceHival = (pdfArray2[pdfArray2.Count - 2] as PdfNumber).IntValue;
            return;
          }
          catch
          {
            this.isIndexedImage = false;
            this.GetIndexedColorSpace(pdfArray1, colorspaceDictionary, filter);
            return;
          }
        }
      }
      this.isIndexedImage = false;
      this.GetIndexedColorSpace(pdfArray1, colorspaceDictionary, filter);
    }
    else if (pdfArray1.Count > 4 && this.m_imageDictionary.ContainsKey("SMask"))
    {
      if (!((pdfArray2[0] as PdfName).Value == "DeviceN") || !(pdfArray2[pdfArray2.Count - 1].GetType().Name == "PdfReferenceHolder") || (object) (pdfArray2[pdfArray2.Count - 3] as PdfName) == null || ((pdfArray2[pdfArray2.Count - 1] as PdfReferenceHolder).Object as PdfDictionary).Values.Count <= 1)
        return;
      PdfName pdfName = new PdfName();
      MemoryStream internalStream = ((pdfArray2[pdfArray2.Count - 2] as PdfReferenceHolder).Object as PdfStream).InternalStream;
      if (((pdfArray2[pdfArray2.Count - 2] as PdfReferenceHolder).Object as PdfStream).ContainsKey("Filter"))
        pdfName = ((pdfArray2[pdfArray2.Count - 2] as PdfReferenceHolder).Object as PdfStream)["Filter"] as PdfName;
      MemoryStream memoryStream = !(pdfName != (PdfName) null) || !(pdfName.Value == "ASCII85Decode") ? this.DecodeFlateStream(internalStream) : this.DecodeASCII85Stream(internalStream);
      if ((pdfArray2[pdfArray2.Count - 3] as PdfName).Value == "DeviceCMYK")
        this.colorSpaceResourceDict.Add("DeviceCMYK", memoryStream);
      this.isIndexedImage = false;
      if (!(pdfArray2[pdfArray2.Count - 3] as PdfName != (PdfName) null))
        return;
      if ((object) (pdfArray2[0] as PdfName) != null && (pdfArray2[0] as PdfName).Value == "DeviceN")
        this.isDeviceN = true;
      if ((object) (pdfArray2[2] as PdfName) == null)
        return;
      this.m_colorspaceBase = (pdfArray2[2] as PdfName).Value;
    }
    else
    {
      this.isIndexedImage = false;
      this.GetIndexedColorSpace(pdfArray1, colorspaceDictionary, filter);
    }
  }

  private void GetIndexedColorSpace(
    PdfArray value,
    PdfDictionary colorspaceDictionary,
    string[] filter)
  {
    if (this.m_colorspace == "ICCBased")
    {
      if ((value[1] as PdfReferenceHolder).Object is PdfStream)
      {
        PdfStream pdfStream1 = (value[1] as PdfReferenceHolder).Object as PdfStream;
        this.nonIndexedImageColorResource = new Dictionary<string, PdfStream>();
        this.nonIndexedImageColorResource.Add(this.m_colorspace, pdfStream1);
        PdfDictionary pdfDictionary1 = (value[1] as PdfReferenceHolder).Object as PdfDictionary;
        PdfName pdfName = pdfDictionary1["Alternate"] as PdfName;
        if (pdfName != (PdfName) null)
        {
          this.m_colorspaceBase = pdfName.Value;
          if (this.m_colorspaceBase == "DeviceGray")
          {
            this.m_isIccBasedAlternateDeviceGray = true;
            PdfStream pdfStream2 = (value[1] as PdfReferenceHolder).Object as PdfStream;
            PdfDictionary pdfDictionary2 = (value[1] as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfStream2 != null)
              this.m_colorspaceStream = pdfStream2.InternalStream;
            if (pdfDictionary2 != null)
              colorspaceDictionary = pdfDictionary2;
            this.colorSpaceResourceDict.Add("ICCBased", this.m_colorspaceStream);
          }
        }
        if (pdfDictionary1.ContainsKey("N"))
          this.numberOfComponents = (pdfDictionary1["N"] as PdfNumber).IntValue;
        if (pdfName == (PdfName) null && this.numberOfComponents == 1)
        {
          this.m_colorspaceBase = "DeviceGray";
          PdfReferenceHolder pdfReferenceHolder = value[1] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object != null)
          {
            PdfStream pdfStream3 = pdfReferenceHolder.Object as PdfStream;
            PdfDictionary pdfDictionary3 = pdfReferenceHolder.Object as PdfDictionary;
            if (pdfStream3 != null)
              this.m_colorspaceStream = pdfStream3.InternalStream;
            if (pdfDictionary3 != null)
              colorspaceDictionary = pdfDictionary3;
            this.colorSpaceResourceDict.Add("ICCBased", this.m_colorspaceStream);
          }
        }
      }
    }
    else if (this.m_colorspace == "Indexed")
    {
      if ((object) (value[1] as PdfName) != null)
        this.m_colorspaceBase = (value[1] as PdfName).Value;
      else if ((object) (value[1] as PdfReferenceHolder) != null)
      {
        if ((value[1] as PdfReferenceHolder).Object is PdfArray)
        {
          PdfArray pdfArray = (value[1] as PdfReferenceHolder).Object as PdfArray;
          if ((object) (pdfArray[0] as PdfName) != null)
            this.internalColorSpace = (pdfArray[0] as PdfName).Value;
          if ((object) (pdfArray[1] as PdfReferenceHolder) != null)
          {
            PdfDictionary pdfDictionary = (pdfArray[1] as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary.ContainsKey("Alternate"))
              this.m_colorspaceBase = (pdfDictionary["Alternate"] as PdfName).Value;
          }
          if (pdfArray.Count > 2 && (object) (pdfArray[2] as PdfName) != null)
            this.m_colorspaceBase = (pdfArray[2] as PdfName).Value;
        }
      }
      else if (value[1] is PdfArray)
      {
        PdfArray pdfArray = value[1] as PdfArray;
        if ((object) (pdfArray[0] as PdfName) != null)
          this.internalColorSpace = (pdfArray[0] as PdfName).Value;
        if ((object) (pdfArray[1] as PdfReferenceHolder) != null)
        {
          PdfDictionary pdfDictionary = (pdfArray[1] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey("Alternate"))
            this.m_colorspaceBase = (pdfDictionary["Alternate"] as PdfName).Value;
        }
      }
      if (value[2] is PdfNumber)
        this.m_colorspaceHival = (value[2] as PdfNumber).IntValue;
      if (this.m_colorspaceBase == "DeviceRGB" || this.m_colorspaceBase == "DeviceGray")
      {
        if (this.m_colorspaceBase == "DeviceGray" || this.indexedColorSpace == "DeviceGray")
          this.ColorSpace = "DeviceGray";
        this.m_colorspaceHival = (value[2] as PdfNumber).IntValue;
        if ((object) (value[3] as PdfReferenceHolder) != null)
        {
          this.m_colorspaceStream = ((value[3] as PdfReferenceHolder).Object as PdfStream).InternalStream;
          colorspaceDictionary = (value[3] as PdfReferenceHolder).Object as PdfDictionary;
        }
        else if (value[3] is PdfString)
        {
          string encodedText = (value[3] as PdfString).Value;
          if (encodedText.Contains("ColorFound") && encodedText.IndexOf("ColorFound") == 0)
            encodedText = encodedText.Remove(0, 10);
          byte[] asciiBytes = ImageStructure.GetAsciiBytes(this.SkipEscapeSequence(this.GetLiteralString(encodedText)));
          this.m_colorspaceStream = new MemoryStream(asciiBytes, 0, asciiBytes.Length, true, true);
          this.colorSpaceResourceDict.Add("Indexed", this.m_colorspaceStream);
        }
        if ((double) this.BitsPerComponent == 4.0 && this.internalColorSpace != "ICCBased")
          this.m_pixelFormat = PixelFormat.Format4bppIndexed;
        else if ((double) this.BitsPerComponent == 8.0)
          this.m_pixelFormat = PixelFormat.Format8bppIndexed;
      }
      else if (this.m_colorspaceBase == null && this.internalColorSpace == "CalRGB")
      {
        this.m_colorspaceBase = this.internalColorSpace;
        this.m_colorspaceHival = (value[2] as PdfNumber).IntValue;
        if ((object) (value[3] as PdfReferenceHolder) != null)
        {
          this.m_colorspaceStream = ((value[3] as PdfReferenceHolder).Object as PdfStream).InternalStream;
          colorspaceDictionary = (value[3] as PdfReferenceHolder).Object as PdfDictionary;
        }
        else if (value[3] is PdfString)
        {
          string encodedText = (value[3] as PdfString).Value;
          if (encodedText.Contains("ColorFound") && encodedText.IndexOf("ColorFound") == 0)
            encodedText = encodedText.Remove(0, 10);
          byte[] asciiBytes = ImageStructure.GetAsciiBytes(this.SkipEscapeSequence(this.GetLiteralString(encodedText)));
          this.m_colorspaceStream = new MemoryStream(asciiBytes, 0, asciiBytes.Length, true, true);
          if ((double) this.BitsPerComponent == 1.0)
            this.colorSpaceResourceDict.Add("Indexed", this.m_colorspaceStream);
        }
      }
      else if ((object) (value[3] as PdfReferenceHolder) != null)
      {
        this.m_colorspaceStream = ((value[3] as PdfReferenceHolder).Object as PdfStream).InternalStream;
        colorspaceDictionary = (value[3] as PdfReferenceHolder).Object as PdfDictionary;
      }
      else if (value[3] is PdfString)
      {
        string encodedText = (value[3] as PdfString).Value;
        if (encodedText.Contains("ColorFound") && encodedText.IndexOf("ColorFound") == 0)
          encodedText = encodedText.Remove(0, 10);
        byte[] asciiBytes = ImageStructure.GetAsciiBytes(this.SkipEscapeSequence(this.GetLiteralString(encodedText)));
        this.m_colorspaceStream = new MemoryStream(asciiBytes, 0, asciiBytes.Length, true, true);
        this.colorSpaceResourceDict.Add("Indexed", this.m_colorspaceStream);
      }
    }
    if (colorspaceDictionary != null && colorspaceDictionary.ContainsKey("Filter"))
    {
      if ((object) (colorspaceDictionary["Filter"] as PdfName) != null)
      {
        filter = new string[1];
        filter[0] = (colorspaceDictionary["Filter"] as PdfName).Value;
      }
      else if (colorspaceDictionary["Filter"] is PdfArray)
      {
        int count = (colorspaceDictionary["Filter"] as PdfArray).Count;
        filter = new string[count];
        for (int index = 0; index < count; ++index)
          filter[index] = ((colorspaceDictionary["Filter"] as PdfArray)[index] as PdfName).Value;
      }
      else if ((object) (colorspaceDictionary["Filter"] as PdfReferenceHolder) != null)
      {
        PdfArray pdfArray = (colorspaceDictionary["Filter"] as PdfReferenceHolder).Object as PdfArray;
        filter = new string[pdfArray.Count];
        for (int index = 0; index < pdfArray.Count; ++index)
          filter[index] = (pdfArray[0] as PdfName).Value;
      }
    }
    if (filter != null)
    {
      for (int index = 0; index < filter.Length; ++index)
      {
        switch (filter[index])
        {
          case "FlateDecode":
            this.m_colorspaceStream = this.DecodeFlateStream(this.m_colorspaceStream);
            continue;
          case "ASCII85":
          case "ASCII85Decode":
            this.m_colorspaceStream = this.DecodeASCII85Stream(this.m_colorspaceStream);
            continue;
          case "ASCIIHexDecode":
            continue;
          default:
            throw new Exception("Filter to decode colorspace not implemented.");
        }
      }
    }
    if (this.m_colorspace == "Indexed" || this.m_colorspace == "IndexedDeviceGray" || this.m_colorspace == "DeviceGray")
    {
      if (this.m_colorspaceStream == null)
        return;
      byte[] buffer = this.m_colorspaceStream.GetBuffer();
      byte[] destinationArray = new byte[786];
      Array.Copy((Array) buffer, (Array) destinationArray, Math.Min(destinationArray.Length, buffer.Length));
      if (this.colorSpaceResourceDict == null || this.colorSpaceResourceDict.Count == 0)
        this.colorSpaceResourceDict.Add(this.m_colorspace, this.m_colorspaceStream);
      this.m_colorPalette = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format8bppIndexed).Palette;
      int num1 = 0;
      for (int index1 = 0; index1 < this.m_colorPalette.Entries.Length; ++index1)
      {
        ref Color local = ref this.m_colorPalette.Entries[index1];
        byte[] numArray1 = destinationArray;
        int index2 = num1;
        int num2 = index2 + 1;
        int red = (int) numArray1[index2];
        byte[] numArray2 = destinationArray;
        int index3 = num2;
        int num3 = index3 + 1;
        int green = (int) numArray2[index3];
        byte[] numArray3 = destinationArray;
        int index4 = num3;
        num1 = index4 + 1;
        int blue = (int) numArray3[index4];
        Color color = Color.FromArgb(red, green, blue);
        local = color;
      }
    }
    else
    {
      if (!(this.m_colorspace == "ICCBased") || !(this.m_colorspaceBase == "DeviceGray"))
        return;
      this.m_colorPalette = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format1bppIndexed).Palette;
    }
  }

  private byte[] ConvertIndexedStreamToFlat(
    int d,
    int w,
    int h,
    byte[] data,
    byte[] index,
    bool isARGB,
    bool isDownsampled)
  {
    int[] numArray1 = new int[3]{ 0, 1, 2 };
    int[] numArray2 = new int[4]{ 0, 1, 2, 3 };
    int components = 3;
    int indexLength = 0;
    if (index != null)
      indexLength = index.Length;
    if (isARGB)
      components = 4;
    return this.ConvertIndexedStreamToFlat(d, w, h, data, index, isARGB, isDownsampled, components, indexLength);
  }

  private byte[] ConvertICCBasedStreamToFlat(
    int d,
    int w,
    int h,
    byte[] data,
    byte[] index,
    bool isARGB,
    bool isDownsampled)
  {
    int[] numArray1 = new int[3]{ 0, 1, 2 };
    int[] numArray2 = new int[4]{ 0, 1, 2, 3 };
    byte[] numArray3 = new byte[6];
    int components = 3;
    int indexLength = 0;
    if (index != null)
      indexLength = index.Length;
    if (isARGB)
      components = 4;
    else if (d == 1 && this.m_colorspaceBase == "DeviceGray" && this.m_colorspace == "ICCBased")
    {
      int index1 = 0;
      for (int index2 = 0; index2 < this.m_colorPalette.Entries.Length; ++index2)
      {
        numArray3[index1] = this.m_colorPalette.Entries[index2].R;
        int num1;
        numArray3[num1 = index1 + 1] = this.m_colorPalette.Entries[index2].G;
        int num2;
        numArray3[num2 = num1 + 1] = this.m_colorPalette.Entries[index2].B;
        index1 = num2 + 1;
      }
    }
    index = numArray3;
    return this.ConvertIndexedStreamToFlat(d, w, h, data, index, isARGB, isDownsampled, components, indexLength);
  }

  private byte[] ConvertIndexedStreamToFlat(
    int d,
    int w,
    int h,
    byte[] data,
    byte[] index,
    bool isARGB,
    bool isDownsampled,
    int components,
    int indexLength)
  {
    int num1 = 0;
    int length = w * h * components;
    byte[] flat = new byte[length];
    int index1 = 0;
    float num2 = 0.0f;
    indexLength = index.Length;
    switch (d)
    {
      case 1:
        int num3 = 0;
        for (int index2 = 0; index2 < data.Length; ++index2)
        {
          for (int index3 = 0; index3 < 8; ++index3)
          {
            int index4 = ((int) data[index2] >> 7 - index3 & 1) * 3;
            if (num1 < length)
            {
              if (isARGB)
              {
                if (index4 == 0)
                {
                  byte[] numArray1 = flat;
                  int index5 = num1;
                  int num4 = index5 + 1;
                  int num5 = (int) index[index4];
                  numArray1[index5] = (byte) num5;
                  byte[] numArray2 = flat;
                  int index6 = num4;
                  int num6 = index6 + 1;
                  int num7 = (int) index[index4 + 1];
                  numArray2[index6] = (byte) num7;
                  byte[] numArray3 = flat;
                  int index7 = num6;
                  int num8 = index7 + 1;
                  int num9 = (int) index[index4 + 2];
                  numArray3[index7] = (byte) num9;
                  byte[] numArray4 = flat;
                  int index8 = num8;
                  num1 = index8 + 1;
                  numArray4[index8] = byte.MaxValue;
                }
                else
                {
                  byte[] numArray5 = flat;
                  int index9 = num1;
                  int num10 = index9 + 1;
                  int num11 = (int) index[index4];
                  numArray5[index9] = (byte) num11;
                  byte[] numArray6 = flat;
                  int index10 = num10;
                  int num12 = index10 + 1;
                  int num13 = (int) index[index4 + 1];
                  numArray6[index10] = (byte) num13;
                  byte[] numArray7 = flat;
                  int index11 = num12;
                  int num14 = index11 + 1;
                  int num15 = (int) index[index4 + 2];
                  numArray7[index11] = (byte) num15;
                  byte[] numArray8 = flat;
                  int index12 = num14;
                  num1 = index12 + 1;
                  numArray8[index12] = (byte) 0;
                }
              }
              else
              {
                byte[] numArray9 = flat;
                int index13 = num1;
                int num16 = index13 + 1;
                int num17 = (int) index[index4];
                numArray9[index13] = (byte) num17;
                byte[] numArray10 = flat;
                int index14 = num16;
                int num18 = index14 + 1;
                int num19 = (int) index[index4 + 1];
                numArray10[index14] = (byte) num19;
                byte[] numArray11 = flat;
                int index15 = num18;
                num1 = index15 + 1;
                int num20 = (int) index[index4 + 2];
                numArray11[index15] = (byte) num20;
              }
              ++num3;
              if (num3 == w)
              {
                num3 = 0;
                index3 = 8;
              }
            }
            else
              break;
          }
        }
        break;
      case 2:
        int[] numArray12 = new int[4]{ 6, 4, 2, 0 };
        int num21 = 0;
        for (int index16 = 0; index16 < data.Length; ++index16)
        {
          for (int index17 = 0; index17 < 4; ++index17)
          {
            int index18 = ((int) data[index16] >> numArray12[index17] & 3) * 3;
            if (num1 < length)
            {
              if (indexLength > index18)
              {
                byte[] numArray13 = flat;
                int index19 = num1;
                int num22 = index19 + 1;
                int num23 = (int) index[index18];
                numArray13[index19] = (byte) num23;
                byte[] numArray14 = flat;
                int index20 = num22;
                int num24 = index20 + 1;
                int num25 = (int) index[index18 + 1];
                numArray14[index20] = (byte) num25;
                byte[] numArray15 = flat;
                int index21 = num24;
                num1 = index21 + 1;
                int num26 = (int) index[index18 + 2];
                numArray15[index21] = (byte) num26;
              }
              if (isARGB)
                flat[num1++] = index18 != 0 ? (byte) 0 : (byte) 0;
              ++num21;
              if (num21 == w)
              {
                num21 = 0;
                index17 = 8;
              }
            }
            else
              break;
          }
        }
        break;
      case 4:
        int[] numArray16 = new int[2]{ 4, 0 };
        int num27 = 0;
        for (int index22 = 0; index22 < data.Length; ++index22)
        {
          for (int index23 = 0; index23 < 2; ++index23)
          {
            int index24 = ((int) data[index22] >> numArray16[index23] & 15) * 3;
            if (num1 < length)
            {
              if (indexLength > index24)
              {
                byte[] numArray17 = flat;
                int index25 = num1;
                int num28 = index25 + 1;
                int num29 = (int) index[index24];
                numArray17[index25] = (byte) num29;
                byte[] numArray18 = flat;
                int index26 = num28;
                int num30 = index26 + 1;
                int num31 = (int) index[index24 + 1];
                numArray18[index26] = (byte) num31;
                byte[] numArray19 = flat;
                int index27 = num30;
                num1 = index27 + 1;
                int num32 = (int) index[index24 + 2];
                numArray19[index27] = (byte) num32;
              }
              if (isARGB)
                flat[num1++] = index24 != 0 ? (byte) 0 : (byte) 0;
              ++num27;
              if (num27 == w)
              {
                num27 = 0;
                index23 = 8;
              }
            }
            else
              break;
          }
        }
        break;
      case 8:
        for (int index28 = 0; index28 < data.Length - 1; ++index28)
        {
          if (isDownsampled)
            num2 = (float) ((int) data[index28] & (int) byte.MaxValue) / (float) byte.MaxValue;
          else
            index1 = ((int) data[index28] & (int) byte.MaxValue) * 3;
          if (num1 < length)
          {
            if (isDownsampled)
            {
              if ((double) num2 > 0.0)
              {
                byte[] numArray20 = flat;
                int index29 = num1;
                int num33 = index29 + 1;
                int num34 = (int) (byte) ((double) ((int) byte.MaxValue - (int) index[0]) * (double) num2);
                numArray20[index29] = (byte) num34;
                byte[] numArray21 = flat;
                int index30 = num33;
                int num35 = index30 + 1;
                int num36 = (int) (byte) ((double) ((int) byte.MaxValue - (int) index[1]) * (double) num2);
                numArray21[index30] = (byte) num36;
                byte[] numArray22 = flat;
                int index31 = num35;
                num1 = index31 + 1;
                int num37 = (int) (byte) ((double) ((int) byte.MaxValue - (int) index[2]) * (double) num2);
                numArray22[index31] = (byte) num37;
              }
              else
                num1 += 3;
            }
            else if (index1 < indexLength - 2)
            {
              byte[] numArray23 = flat;
              int index32 = num1;
              int num38 = index32 + 1;
              int num39 = (int) index[index1];
              numArray23[index32] = (byte) num39;
              byte[] numArray24 = flat;
              int index33 = num38;
              int num40 = index33 + 1;
              int num41 = (int) index[index1 + 1];
              numArray24[index33] = (byte) num41;
              byte[] numArray25 = flat;
              int index34 = num40;
              num1 = index34 + 1;
              int num42 = (int) index[index1 + 2];
              numArray25[index34] = (byte) num42;
            }
            else
            {
              byte[] numArray26 = flat;
              int index35 = num1;
              int num43 = index35 + 1;
              int num44 = (int) data[index28];
              numArray26[index35] = (byte) num44;
              byte[] numArray27 = flat;
              int index36 = num43;
              int num45 = index36 + 1;
              int num46 = (int) data[index28];
              numArray27[index36] = (byte) num46;
              byte[] numArray28 = flat;
              int index37 = num45;
              num1 = index37 + 1;
              int num47 = (int) data[index28];
              numArray28[index37] = (byte) num47;
            }
            if (isARGB)
              flat[num1++] = index1 != 0 || (double) num2 != 0.0 ? (byte) 0 : byte.MaxValue;
          }
          else
            break;
        }
        break;
    }
    return flat;
  }

  private string GetLiteralString(string encodedText)
  {
    string literalString = encodedText;
    int startIndex = -1;
    int num = 3;
    while (literalString.Contains("\\") || literalString.Contains("\0"))
    {
      string empty = string.Empty;
      if (literalString.IndexOf('\\', startIndex + 1) >= 0)
      {
        startIndex = literalString.IndexOf('\\', startIndex + 1);
      }
      else
      {
        startIndex = literalString.IndexOf(char.MinValue, startIndex + 1);
        if (startIndex >= 0)
          num = 2;
        else
          break;
      }
      for (int index = startIndex + 1; index <= startIndex + num; ++index)
      {
        if (index < literalString.Length)
        {
          int result = 0;
          if (int.TryParse(literalString[index].ToString(), out result))
          {
            if (result <= 8)
              empty += (string) (object) literalString[index];
          }
          else
          {
            empty = string.Empty;
            break;
          }
        }
        else
          empty = string.Empty;
      }
      if (empty != string.Empty && num == 3)
      {
        int uint64 = (int) Convert.ToUInt64(empty, 8);
        string str = Encoding.GetEncoding(1252).GetString(new byte[1]
        {
          Convert.ToByte(uint64)
        });
        literalString = literalString.Remove(startIndex, num + 1).Insert(startIndex, str);
      }
    }
    return literalString;
  }

  private string SkipEscapeSequence(string text)
  {
    string str1 = "";
    string str2 = "";
    string str3 = "escape";
    if (text.Contains("\\\\\\"))
    {
      int num = text.IndexOf('\\');
      if (num - 1 != 0)
        str1 = text.Substring(num - 3, 3);
    }
    text = text.Replace("\\r", "\r");
    text = text.Replace("\\(", "(");
    text = text.Replace("\\)", ")");
    text = text.Replace("\\n", "\n");
    text = text.Replace("\\t", "\t");
    if (text.Contains("\\b"))
    {
      text = text.Replace("\\b", str3);
      int num = text.IndexOf(str3);
      if (num - 1 != 0)
        str2 = text.Substring(num - 1, 1);
    }
    text = !(str2 == "\u0094") ? text.Replace(str3, "\b") : text.Replace(str3, "\\b");
    if (str1 != "[[[")
      text = text.Replace("\\\\", "\\");
    return text;
  }

  private static byte[] GetAsciiBytes(string value)
  {
    byte[] asciiBytes = value != null ? new byte[value.Length] : throw new ArgumentNullException(nameof (value));
    int index = 0;
    for (int length = value.Length; index < length; ++index)
    {
      if (value[index] > 'ÿ')
      {
        Encoding encoding = Encoding.GetEncoding(1252);
        asciiBytes[index] = encoding.GetBytes(value[index].ToString())[0];
      }
      else
        asciiBytes[index] = (byte) value[index];
    }
    return asciiBytes;
  }

  private MemoryStream DecodeASCII85Stream(MemoryStream encodedStream)
  {
    byte[] buffer = new ASCII85().decode(encodedStream.ToArray());
    MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length, true, true);
    memoryStream.Position = 0L;
    return memoryStream;
  }

  private MemoryStream DecodeFlateStream(MemoryStream encodedStream)
  {
    encodedStream.Position = 0L;
    encodedStream.ReadByte();
    encodedStream.ReadByte();
    DeflateStream deflateStream = new DeflateStream((Stream) encodedStream, CompressionMode.Decompress, true);
    byte[] buffer = new byte[4096 /*0x1000*/];
    MemoryStream memoryStream = new MemoryStream();
    while (true)
    {
      int count = deflateStream.Read(buffer, 0, 4096 /*0x1000*/);
      if (count > 0)
        memoryStream.Write(buffer, 0, count);
      else
        break;
    }
    return memoryStream;
  }

  private MemoryStream DecodeMaskStreamWithMultipleFilters(MemoryStream decodableStream)
  {
    MemoryStream memoryStream = (MemoryStream) null;
    for (int index = 1; index < this.m_maskFilter.Length; ++index)
    {
      if (this.m_maskFilter[index] == "ASCII85Decode")
      {
        memoryStream = this.DecodeASCII85Stream(decodableStream);
      }
      else
      {
        if (this.m_maskFilter[index] == "CCITTFaxDecode")
        {
          PdfDictionary decodeParams = new PdfDictionary();
          if (this.m_maskDictionary.ContainsKey("DecodeParms"))
            decodeParams = this.GetDecodeParam(this.m_maskDictionary)[1];
          return this.DecodeCCITTFaxDecodeStream(decodableStream, this.m_maskDictionary, decodeParams);
        }
        if (this.m_maskFilter[index] == "DCTDecode")
          return decodableStream;
        if (this.m_maskFilter[index] == "FlateDecode")
          return this.DecodeFlateStream(decodableStream);
        if (this.m_maskFilter[index] == "JPXDecode")
          new JPXImage().FromStream((Stream) decodableStream).Save((Stream) memoryStream, ImageFormat.Png);
        else if (this.m_maskFilter[index] == "JBIG2Decode")
          this.DecodeJBIG2EncodedStream(decodableStream).Save((Stream) memoryStream, ImageFormat.Png);
        else if (this.m_maskFilter[index] == "LZWDecode")
          memoryStream = new MemoryStream(new PdfLzwCompressor().Decompress(decodableStream.ToArray(), this.IsEarlyChange));
      }
    }
    return memoryStream;
  }

  private MemoryStream GetJBIG2GlobalsStream()
  {
    MemoryStream encodedStream = new MemoryStream();
    if (this.m_imageDictionary.ContainsKey("DecodeParms"))
    {
      if (this.m_imageDictionary["DecodeParms"] is PdfDictionary image1)
      {
        if (image1.ContainsKey("JBIG2Globals"))
        {
          encodedStream = ((image1["JBIG2Globals"] as PdfReferenceHolder).Object as PdfStream).InternalStream;
          if ((image1["JBIG2Globals"] as PdfReferenceHolder).Object is PdfDictionary)
          {
            PdfDictionary pdfDictionary = (image1["JBIG2Globals"] as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary.ContainsKey("Filter") && (pdfDictionary["Filter"] as PdfName).Value == "FlateDecode")
              encodedStream = this.DecodeFlateStream(encodedStream);
          }
        }
      }
      else
      {
        string str = "";
        PdfArray image = this.m_imageDictionary["DecodeParms"] as PdfArray;
        if (!(image[0] is PdfDictionary pdfDictionary1))
          pdfDictionary1 = (image[0] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary1.ContainsKey("JBIG2Globals"))
        {
          if ((pdfDictionary1["JBIG2Globals"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Filter"))
            str = (pdfDictionary2["Filter"] as PdfName).Value.ToString();
          encodedStream = ((pdfDictionary1["JBIG2Globals"] as PdfReferenceHolder).Object as PdfStream).InternalStream;
          if (str == "FlateDecode")
          {
            encodedStream.Position = 0L;
            encodedStream.ReadByte();
            encodedStream.ReadByte();
            DeflateStream deflateStream = new DeflateStream((Stream) encodedStream, CompressionMode.Decompress, true);
            byte[] buffer = new byte[4096 /*0x1000*/];
            MemoryStream memoryStream = new MemoryStream();
            while (true)
            {
              int count = deflateStream.Read(buffer, 0, 4096 /*0x1000*/);
              if (count > 0)
                memoryStream.Write(buffer, 0, count);
              else
                break;
            }
            encodedStream = memoryStream;
          }
        }
      }
      if (encodedStream.Length > 0L)
        encodedStream.Capacity = (int) encodedStream.Length;
    }
    return encodedStream;
  }

  private Bitmap DecodeJBIG2EncodedStream(MemoryStream imageStream)
  {
    JBIG2StreamDecoder jbiG2StreamDecoder = new JBIG2StreamDecoder();
    if (this.ImageDictionary.ContainsKey("DecodeParms"))
      jbiG2StreamDecoder.GlobalData = this.GetJBIG2GlobalsStream().GetBuffer();
    byte[] buffer = imageStream.GetBuffer();
    byte[] numArray = new byte[imageStream.Length];
    Buffer.BlockCopy((Array) buffer, 0, (Array) numArray, 0, numArray.Length);
    jbiG2StreamDecoder.DecodeJBIG2(numArray);
    Array.Clear((Array) numArray, 0, numArray.Length);
    JBIG2Image pageAsJbiG2Bitmap = jbiG2StreamDecoder.GetPageAsJBIG2Bitmap(0);
    bool switchPixelColor = true;
    if (this.ImageDictionary.ContainsKey("Decode"))
    {
      PdfArray pdfArray = (PdfArray) null;
      if (this.ImageDictionary["Decode"] is PdfArray)
        pdfArray = this.ImageDictionary["Decode"] as PdfArray;
      switchPixelColor = pdfArray == null || (pdfArray[0] as PdfNumber).IntValue != 1;
    }
    byte[] data = pageAsJbiG2Bitmap.GetData(switchPixelColor);
    Bitmap bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format1bppIndexed);
    BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, (int) this.Width, (int) this.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
    double height = (double) this.Height;
    int stride = bitmapdata.Stride;
    IntPtr scan0 = bitmapdata.Scan0;
    int pixelFormatSize = Image.GetPixelFormatSize(bitmap.PixelFormat);
    int num = pixelFormatSize / 8;
    int length = num * (int) this.Width;
    switch (num)
    {
      case 0:
        length = (int) this.Width * pixelFormatSize / 8;
        if ((int) this.Width * pixelFormatSize % 8 != 0)
        {
          ++length;
          break;
        }
        break;
      case 1:
        length = bitmap.Width;
        break;
      case 3:
        length = num * (int) this.Width;
        break;
    }
    int startIndex = 0;
    long int64 = bitmapdata.Scan0.ToInt64();
    for (int index = 0; (double) index < (double) this.Height; ++index)
    {
      Marshal.Copy(data, startIndex, new IntPtr(int64), length);
      startIndex += length;
      int64 += (long) bitmapdata.Stride;
    }
    bitmap.UnlockBits(bitmapdata);
    return bitmap;
  }

  private void GetImageInterpolation(PdfDictionary imageDictionary)
  {
    if (imageDictionary == null || !imageDictionary.ContainsKey("Interpolate"))
      return;
    this.m_isImageInterpolated = (imageDictionary["Interpolate"] as PdfBoolean).Value;
  }

  private MemoryStream MergeImages(Bitmap input, MemoryStream maskStream, bool DTCdecode)
  {
    bool flag1 = false;
    bool flag2 = false;
    int predictor = 0;
    int colors = 1;
    int columns = 1;
    Color transparentColor;
    if (input == null)
    {
      input = new Bitmap((int) this.m_maskWidth, (int) this.m_maskHeight);
      transparentColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    }
    else if (input != null && (double) this.m_maskWidth > (double) input.Width && (double) this.m_maskHeight > (double) input.Height && this.m_maskFilter[0] != "JBIG2Decode" && !DTCdecode)
    {
      input = new Bitmap((int) this.m_maskWidth, (int) this.m_maskHeight);
      transparentColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    }
    else
      transparentColor = input.GetPixel(0, 0);
    Bitmap original1 = (Bitmap) null;
    MemoryStream memoryStream1 = new MemoryStream();
    this.GetImageInterpolation(this.m_maskDictionary);
    if (this.m_maskFilter[0] == "ASCII85Decode")
    {
      maskStream.Position = 0L;
      maskStream = this.DecodeASCII85Stream(maskStream);
      MemoryStream memoryStream2;
      using (memoryStream2 = this.DecodeMaskStreamWithMultipleFilters(maskStream))
      {
        if (memoryStream2 != null)
        {
          memoryStream2.Position = 0L;
          maskStream = memoryStream2;
        }
      }
    }
    else if (this.m_maskFilter[0] == "DCTDecode")
    {
      MemoryStream memoryStream3;
      using (memoryStream3 = this.DecodeMaskStreamWithMultipleFilters(maskStream))
      {
        if (memoryStream3 != null)
        {
          memoryStream3.Position = 0L;
          maskStream = memoryStream3;
        }
      }
      original1 = Image.FromStream((Stream) maskStream) as Bitmap;
      if (this.m_maskColorspace == "DeviceGray" && original1.Height > input.Height)
        original1 = Image.FromStream((Stream) this.DecodeDeviceGrayImage(maskStream)) as Bitmap;
    }
    else if (this.m_maskFilter[0] == "FlateDecode")
    {
      maskStream = this.DecodeFlateStream(maskStream);
      MemoryStream memoryStream4 = this.DecodeMaskStreamWithMultipleFilters(maskStream);
      if (memoryStream4 != null)
      {
        if (this.m_maskFilter[0] == "FlateDecode" && this.m_maskFilter[1] == "CCITTFaxDecode")
        {
          this.m_isImageMask = false;
          return memoryStream4;
        }
        memoryStream4.Position = 0L;
        maskStream = memoryStream4;
      }
      if (this.m_maskDictionary.ContainsKey("DecodeParms"))
      {
        PdfDictionary pdfDictionary = this.GetDecodeParam(this.m_maskDictionary)[0];
        if (pdfDictionary != null)
        {
          flag2 = true;
          if (pdfDictionary.ContainsKey("Predictor"))
            predictor = (pdfDictionary["Predictor"] as PdfNumber).IntValue;
          if (pdfDictionary.ContainsKey("Columns"))
            columns = (pdfDictionary["Columns"] as PdfNumber).IntValue;
          if (pdfDictionary.ContainsKey("Colors"))
            colors = (pdfDictionary["Colors"] as PdfNumber).IntValue;
          if (pdfDictionary.ContainsKey("BitsPerComponent"))
            this.m_bitsPerComponent = (float) (pdfDictionary["BitsPerComponent"] as PdfNumber).IntValue;
          if (pdfDictionary.Count > 0)
            maskStream = this.DecodePredictor(predictor, colors, columns, maskStream);
        }
      }
      original1 = this.DecodeMaskImage(maskStream);
      if (this.ImageFilter.Length > 1)
      {
        MemoryStream memoryStream5 = new MemoryStream();
        using (Image image = (Image) new Bitmap((Image) original1, (int) this.Width, (int) this.Height))
        {
          image.Save((Stream) memoryStream5, ImageFormat.Png);
          original1.Dispose();
        }
        memoryStream5.Position = 0L;
        original1 = (Bitmap) Image.FromStream((Stream) memoryStream5);
      }
      if (this.ColorSpace == "ICCBased")
        transparentColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      if (this.m_maskColorspace == "DeviceGray" && original1.Height > input.Height)
        original1 = Image.FromStream((Stream) this.DecodeDeviceGrayImage(maskStream)) as Bitmap;
      else if (this.m_maskColorspace == "DeviceGray" && transparentColor != Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue) && !this.m_maskDictionary.ContainsKey("Matte") && !this.m_maskDictionary.ContainsKey("DecodeParms") && this.m_maskDictionary.ContainsKey("BitsPerComponent") && (this.m_maskDictionary["BitsPerComponent"] as PdfNumber).IntValue == 8)
        transparentColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    }
    else if (this.m_maskFilter[0] == "CCITTFaxDecode")
    {
      PdfDictionary decodeParams = new PdfDictionary();
      if (this.m_maskDictionary.ContainsKey("DecodeParms"))
        decodeParams = this.GetDecodeParam(this.m_maskDictionary)[0];
      if (this.ImageDictionary.ContainsKey("Mask"))
        this.m_isMaskImage = true;
      maskStream = this.DecodeCCITTFaxDecodeStream(maskStream, this.m_maskDictionary, decodeParams);
      MemoryStream memoryStream6;
      using (memoryStream6 = this.DecodeMaskStreamWithMultipleFilters(maskStream))
      {
        if (memoryStream6 != null)
        {
          memoryStream6.Position = 0L;
          maskStream = memoryStream6;
        }
      }
      original1 = Image.FromStream((Stream) maskStream) as Bitmap;
      int width = input.Width > original1.Width ? input.Width : original1.Width;
      int height = input.Height > original1.Height ? input.Height : original1.Height;
      if (input.Width != width || input.Height != height)
        input = new Bitmap((Image) input, width, height);
    }
    else if (this.m_maskFilter[0] == "JPXDecode")
    {
      maskStream.Position = 0L;
      original1 = (Bitmap) new JPXImage().FromStream((Stream) maskStream);
      maskStream = new MemoryStream();
      original1.Save((Stream) maskStream, ImageFormat.Png);
      MemoryStream memoryStream7;
      using (memoryStream7 = this.DecodeMaskStreamWithMultipleFilters(maskStream))
      {
        if (memoryStream7 != null)
        {
          memoryStream7.Position = 0L;
          maskStream = memoryStream7;
          original1 = Image.FromStream((Stream) maskStream) as Bitmap;
        }
      }
    }
    else if (this.m_maskFilter[0] == "JBIG2Decode")
    {
      Stream imageStream = this.ImageStream;
      JBIG2StreamDecoder jbiG2StreamDecoder = new JBIG2StreamDecoder();
      MemoryStream encodedStream = new MemoryStream();
      if (this.m_maskDictionary.ContainsKey("DecodeParms"))
      {
        if (this.m_maskDictionary["DecodeParms"] is PdfDictionary mask)
        {
          if (mask.ContainsKey("JBIG2Globals"))
          {
            encodedStream = ((mask["JBIG2Globals"] as PdfReferenceHolder).Object as PdfStream).InternalStream;
            if ((mask["JBIG2Globals"] as PdfReferenceHolder).Object is PdfDictionary)
            {
              PdfDictionary pdfDictionary = (mask["JBIG2Globals"] as PdfReferenceHolder).Object as PdfDictionary;
              if (pdfDictionary.ContainsKey("Filter") && (pdfDictionary["Filter"] as PdfName).Value == "FlateDecode")
                encodedStream = this.DecodeFlateStream(encodedStream);
            }
          }
        }
        else
        {
          string str = "";
          PdfDictionary pdfDictionary1 = (this.m_maskDictionary["DecodeParms"] as PdfArray)[0] as PdfDictionary;
          if (pdfDictionary1.ContainsKey("JBIG2Globals"))
          {
            if ((pdfDictionary1["JBIG2Globals"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Filter"))
              str = (pdfDictionary2["Filter"] as PdfName).Value.ToString();
            encodedStream = ((pdfDictionary1["JBIG2Globals"] as PdfReferenceHolder).Object as PdfStream).InternalStream;
            if (str == "FlateDecode")
            {
              encodedStream.Position = 0L;
              encodedStream.ReadByte();
              encodedStream.ReadByte();
              DeflateStream deflateStream = new DeflateStream((Stream) encodedStream, CompressionMode.Decompress, true);
              byte[] buffer = new byte[4096 /*0x1000*/];
              MemoryStream memoryStream8 = new MemoryStream();
              while (true)
              {
                int count = deflateStream.Read(buffer, 0, 4096 /*0x1000*/);
                if (count > 0)
                  memoryStream8.Write(buffer, 0, count);
                else
                  break;
              }
              encodedStream = memoryStream8;
            }
          }
        }
        if (encodedStream.Length > 0L)
        {
          encodedStream.Capacity = (int) encodedStream.Length;
          jbiG2StreamDecoder.GlobalData = encodedStream.GetBuffer();
        }
      }
      jbiG2StreamDecoder.DecodeJBIG2(maskStream.GetBuffer());
      byte[] data = jbiG2StreamDecoder.GetPageAsJBIG2Bitmap(0).GetData(true);
      Bitmap original2 = new Bitmap((int) this.m_maskWidth, (int) this.m_maskHeight, PixelFormat.Format1bppIndexed);
      BitmapData bitmapdata = original2.LockBits(new Rectangle(0, 0, (int) this.m_maskWidth, (int) this.m_maskHeight), ImageLockMode.ReadWrite, original2.PixelFormat);
      int stride = bitmapdata.Stride;
      IntPtr scan0 = bitmapdata.Scan0;
      int pixelFormatSize = Image.GetPixelFormatSize(original2.PixelFormat);
      int num = pixelFormatSize / 8;
      int length = num * (int) this.m_maskWidth;
      switch (num)
      {
        case 0:
          length = (int) this.m_maskWidth * pixelFormatSize / 8;
          if ((int) this.m_maskWidth * pixelFormatSize % 8 != 0)
          {
            ++length;
            break;
          }
          break;
        case 1:
          length = original2.Width;
          break;
        case 3:
          length = num * (int) this.m_maskWidth;
          break;
      }
      int startIndex = 0;
      long int64 = bitmapdata.Scan0.ToInt64();
      for (int index = 0; (double) index < (double) this.m_maskHeight; ++index)
      {
        Marshal.Copy(data, startIndex, new IntPtr(int64), length);
        startIndex += length;
        int64 += (long) bitmapdata.Stride;
      }
      original2.UnlockBits(bitmapdata);
      MemoryStream decodableStream = new MemoryStream();
      using (Image image = (Image) new Bitmap((Image) original2, (int) this.Width, (int) this.Height))
      {
        image.Save((Stream) decodableStream, ImageFormat.Png);
        original2.Dispose();
      }
      decodableStream.Position = 0L;
      MemoryStream memoryStream9;
      using (memoryStream9 = this.DecodeMaskStreamWithMultipleFilters(decodableStream))
      {
        if (memoryStream9 != null)
        {
          memoryStream9.Position = 0L;
          maskStream = memoryStream9;
        }
      }
      original1 = Image.FromStream((Stream) decodableStream) as Bitmap;
      maskStream = new MemoryStream(data);
    }
    else
      original1 = this.m_maskFilter[0] != null ? Image.FromStream((Stream) maskStream) as Bitmap : this.DecodeMaskImage(maskStream);
    Bitmap original3 = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb);
    Rectangle rect1 = new Rectangle(0, 0, input.Width, input.Height);
    BitmapData bitmapdata1 = input.LockBits(rect1, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
    Rectangle rect2 = new Rectangle(0, 0, original1.Width, original1.Height);
    BitmapData bitmapdata2 = original1.LockBits(rect2, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
    BitmapData bitmapdata3 = original3.LockBits(rect1, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
    int length1 = Math.Abs(bitmapdata1.Stride) * input.Height;
    byte[] numArray1 = new byte[length1];
    Marshal.Copy(bitmapdata1.Scan0, numArray1, 0, length1);
    int length2 = Math.Abs(bitmapdata2.Stride) * original1.Height;
    byte[] numArray2 = new byte[length2];
    Marshal.Copy(bitmapdata2.Scan0, numArray2, 0, length2);
    int length3 = Math.Abs(bitmapdata3.Stride) * original3.Height;
    byte[] numArray3 = new byte[length3];
    Marshal.Copy(bitmapdata3.Scan0, numArray3, 0, length3);
    MemoryStream memoryStream10 = new MemoryStream();
    int num1 = 0;
    if (DTCdecode && this.m_maskFilter[0] != "JBIG2Decode" && (this.IsWhiteImage(length3, numArray2) || this.IsImageForExtraction))
    {
      byte maxValue = byte.MaxValue;
      if (numArray2[0] == (byte) 0 && numArray2[1] == (byte) 0 && numArray2[2] == (byte) 0 && numArray2[3] == (byte) 0)
        flag1 = true;
      for (int index = 0; index < length3; index += 4)
      {
        if (numArray2[index] != (byte) 0 || numArray2[index + 1] != (byte) 0 || numArray2[index + 2] != (byte) 0 || numArray2[index + 3] != byte.MaxValue)
        {
          Color color1 = Color.FromArgb((int) numArray1[index], (int) numArray1[index + 1], (int) numArray1[index + 2], (int) numArray1[index + 3]);
          Color color2 = Color.FromArgb((int) (byte) ((uint) maxValue - (uint) numArray2[index]), (int) (byte) ((uint) maxValue - (uint) numArray2[index + 1]), (int) (byte) ((uint) maxValue - (uint) numArray2[index + 2]), (int) (byte) ((uint) maxValue - (uint) numArray2[index + 3]));
          float num2 = this.ConvertToFloat(color1.A);
          float num3 = this.ConvertToFloat(color1.R);
          float num4 = this.ConvertToFloat(color1.G);
          float num5 = this.ConvertToFloat(color1.B);
          float num6 = this.ConvertToFloat(color2.A);
          float num7 = this.ConvertToFloat(color2.R);
          float num8 = this.ConvertToFloat(color2.G);
          float num9 = this.ConvertToFloat(color2.B);
          if (color2.A == (byte) 0 && color2.R == (byte) 0 && color2.G == (byte) 0 && color2.B == (byte) 0)
            num1 += 4;
          float num10 = num2 + num6;
          float num11 = num3 + num7;
          float num12 = num4 + num8;
          float num13 = num5 + num9;
          byte num14 = this.ConvertToByte(num10);
          byte num15 = this.ConvertToByte(num11);
          byte num16 = this.ConvertToByte(num12);
          byte num17 = this.ConvertToByte(num13);
          numArray3[index] = num14;
          numArray3[index + 1] = num15;
          numArray3[index + 2] = num16;
          numArray3[index + 3] = num17;
        }
        else if (transparentColor == Color.FromArgb((int) byte.MaxValue, 0, 0, 0) || transparentColor == Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue))
        {
          numArray3[index] = transparentColor.R;
          numArray3[index + 1] = transparentColor.G;
          numArray3[index + 2] = transparentColor.B;
          numArray3[index + 3] = transparentColor.A;
        }
        else if (!this.IsImageForExtraction)
        {
          numArray3[index] = (byte) 0;
          numArray3[index + 1] = (byte) 0;
          numArray3[index + 2] = (byte) 0;
          numArray3[index + 3] = (byte) 0;
        }
        else if (!flag1)
        {
          numArray3[index] = byte.MaxValue;
          numArray3[index + 1] = byte.MaxValue;
          numArray3[index + 2] = byte.MaxValue;
          numArray3[index + 3] = byte.MaxValue;
        }
      }
      Marshal.Copy(numArray3, 0, bitmapdata3.Scan0, length3);
      original1.UnlockBits(bitmapdata2);
      input.UnlockBits(bitmapdata1);
      original3.UnlockBits(bitmapdata3);
      if (!this.IsImageForExtraction)
      {
        if (this.IsTransparent)
          original3.MakeTransparent(Color.Transparent);
        else if (transparentColor == Color.FromArgb((int) byte.MaxValue, 0, 0, 0) || transparentColor == Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue))
          original3.MakeTransparent(transparentColor);
        else if (num1 != numArray2.Length || this.m_isExtGStateContainsSMask)
          original3.MakeTransparent();
      }
      else if ((double) this.Width != (double) original3.Width || (double) this.Height != (double) original3.Height)
        original3 = new Bitmap((Image) original3, new Size((int) this.Width, (int) this.Height));
      original3.Save((Stream) memoryStream10, ImageFormat.Png);
      memoryStream10.Position = 0L;
    }
    else
    {
      if (this.m_maskFilter[0] == "JBIG2Decode")
      {
        int intValue = (this.m_maskDictionary["BitsPerComponent"] as PdfNumber).IntValue;
        double[] numArray4 = new double[6]
        {
          0.0,
          1.0,
          0.0,
          1.0,
          0.0,
          1.0
        };
        double num18 = 0.0;
        double num19 = Math.Pow(2.0, (double) this.GetBitsPerComponent()) - (double) intValue;
        double num20 = (numArray4[1] - numArray4[0]) / (num19 - num18);
        double num21 = (numArray4[3] - numArray4[2]) / (num19 - num18);
        double num22 = (numArray4[5] - numArray4[4]) / (num19 - num18);
        this.m_maskedPixels = this.GetStencilMaskedPixels(maskStream.ToArray(), this.m_maskWidth, this.m_maskHeight);
        maskStream.Dispose();
        maskStream = (MemoryStream) null;
        int[] numArray5 = new int[3];
        double imageFactorWidth = (double) this.m_maskWidth / (double) input.Width;
        double imageFactorHeight = (double) this.m_maskHeight / (double) input.Height;
        BitParser bitParser = new BitParser(numArray1, 8);
        byte[] pixels = new byte[input.Width * input.Height * 4];
        int index = 0;
        for (int row = 0; row < input.Height; ++row)
        {
          for (int column = 0; column < input.Width; ++column)
          {
            numArray5[2] = bitParser.ReadBits();
            numArray5[1] = bitParser.ReadBits();
            numArray5[0] = bitParser.ReadBits();
            ++bitParser.sourcePointer;
            if (this.IsMasked(row, column, imageFactorWidth, imageFactorHeight))
            {
              pixels[index] = (byte) 0;
              pixels[index + 1] = (byte) 0;
              pixels[index + 2] = (byte) 0;
              pixels[index + 3] = (byte) 0;
              index += 4;
            }
            else
            {
              byte[] numArray6 = ImageStructure.FromArgb(1.0, numArray4[0] + ((double) numArray5[0] - num18) * num20, numArray4[2] + ((double) numArray5[1] - num18) * num21, numArray4[4] + ((double) numArray5[2] - num18) * num22);
              pixels[index] = numArray6[0];
              pixels[index + 1] = numArray6[1];
              pixels[index + 2] = numArray6[2];
              pixels[index + 3] = numArray6[3];
              index += 4;
            }
          }
          bitParser.MoveToNextRow();
        }
        MemoryStream memoryStream11 = this.ProcessImage((Image) input, pixels);
        original1.UnlockBits(bitmapdata2);
        input.UnlockBits(bitmapdata1);
        original3.UnlockBits(bitmapdata3);
        original3.Dispose();
        input.Dispose();
        original1.Dispose();
        memoryStream11.Position = 0L;
        return memoryStream11;
      }
      int[] numArray7;
      if (this.m_maskFilter[0] == "FlateDecode" && this.m_maskColorspace == "DeviceGray" && this.isIndexedImage && this.ImageFilter.Length > 1 && this.ColorSpace == "Indexed" && this.m_colorspaceBase == "DeviceRGB" && (double) this.m_bitsPerComponent == 8.0 && !flag2)
      {
        numArray7 = new int[numArray2.Length];
      }
      else
      {
        for (int index = 0; index < length3; index += 4)
          numArray1[index + 3] = numArray2[index + 2];
        numArray7 = new int[numArray1.Length];
        int index1 = 0;
        for (int index2 = 0; index2 < numArray7.Length; index2 += 4)
        {
          numArray7[index1] = (int) numArray1[index2 + 3] << 24 | (int) numArray1[index2 + 2] << 16 /*0x10*/ | (int) numArray1[index2 + 1] << 8 | (int) numArray1[index2];
          ++index1;
        }
      }
      Bitmap bitmap = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb);
      BitmapData bitmapdata4 = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
      byte[] source = new byte[bitmapdata4.Stride * bitmap.Height];
      for (int index = 0; index < numArray7.Length / 4 - 1; ++index)
      {
        Color color = Color.FromArgb(numArray7[index]);
        int num23 = index % input.Width;
        int num24 = index / input.Width;
        source[bitmapdata4.Stride * num24 + 4 * num23] = color.B;
        source[bitmapdata4.Stride * num24 + 4 * num23 + 1] = color.G;
        source[bitmapdata4.Stride * num24 + 4 * num23 + 2] = color.R;
        source[bitmapdata4.Stride * num24 + 4 * num23 + 3] = color.A;
      }
      Marshal.Copy(source, 0, bitmapdata4.Scan0, source.Length);
      bitmap.UnlockBits(bitmapdata4);
      original1.UnlockBits(bitmapdata2);
      input.UnlockBits(bitmapdata1);
      original3.UnlockBits(bitmapdata3);
      original3.Dispose();
      input.Dispose();
      original1.Dispose();
      if (!this.IsImageForExtraction && (!(this.ColorSpace == "Indexed") || !(this.m_colorspaceBase == "DeviceCMYK")))
        bitmap.MakeTransparent(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      bitmap.Save((Stream) memoryStream10, ImageFormat.Png);
      memoryStream10.Position = 0L;
    }
    input.Dispose();
    original1.Dispose();
    original3.Dispose();
    return memoryStream10;
  }

  private bool IsWhiteImage(int bytes, byte[] maskBytes)
  {
    int num = 0;
    for (int index = 0; index < bytes; index += 4)
    {
      if (maskBytes[index] != (byte) 0 || maskBytes[index + 1] != (byte) 0 || maskBytes[index + 2] != (byte) 0 || maskBytes[index + 3] != byte.MaxValue)
      {
        Color color = Color.FromArgb((int) (byte) ((uint) byte.MaxValue - (uint) maskBytes[index]), (int) (byte) ((uint) byte.MaxValue - (uint) maskBytes[index + 1]), (int) (byte) ((uint) byte.MaxValue - (uint) maskBytes[index + 2]), (int) (byte) ((uint) byte.MaxValue - (uint) maskBytes[index + 3]));
        if (color.A == (byte) 0 && color.R == (byte) 0 && color.G == (byte) 0 && color.B == (byte) 0)
          num += 4;
      }
    }
    if (num == maskBytes.Length)
      return true;
    if (this.DecodeParam == null)
      return false;
    for (int index = 0; index < this.DecodeParam.Length; ++index)
    {
      if (this.DecodeParam[index] != null)
        return true;
    }
    return false;
  }

  private MemoryStream ProcessImage(Image input, byte[] pixels)
  {
    MemoryStream memoryStream = new MemoryStream();
    using (Bitmap bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format32bppArgb))
    {
      BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
      byte[] source = new byte[bitmapdata.Stride * bitmap.Height];
      int num1 = input.Width * input.Height;
      int index1 = 0;
      for (int index2 = 0; index2 < num1; ++index2)
      {
        int num2 = index2 % input.Width;
        int num3 = index2 / input.Width;
        source[bitmapdata.Stride * num3 + 4 * num2] = pixels[index1];
        source[bitmapdata.Stride * num3 + 4 * num2 + 1] = pixels[index1 + 1];
        source[bitmapdata.Stride * num3 + 4 * num2 + 2] = pixels[index1 + 2];
        source[bitmapdata.Stride * num3 + 4 * num2 + 3] = pixels[index1 + 3];
        index1 += 4;
      }
      Marshal.Copy(source, 0, bitmapdata.Scan0, source.Length);
      bitmap.UnlockBits(bitmapdata);
      bitmap.Save((Stream) memoryStream, ImageFormat.Png);
    }
    return memoryStream;
  }

  public static byte[] FromArgb(double alpha, double red, double green, double blue)
  {
    return new byte[4]
    {
      blue < 0.0 ? (byte) 0 : (blue > 1.0 ? byte.MaxValue : (byte) (blue * (double) byte.MaxValue)),
      green < 0.0 ? (byte) 0 : (green > 1.0 ? byte.MaxValue : (byte) (green * (double) byte.MaxValue)),
      red < 0.0 ? (byte) 0 : (red > 1.0 ? byte.MaxValue : (byte) (red * (double) byte.MaxValue)),
      alpha < 0.0 ? (byte) 0 : (alpha > 1.0 ? byte.MaxValue : (byte) (alpha * (double) byte.MaxValue))
    };
  }

  private bool IsMasked(int row, int column, double imageFactorWidth, double imageFactorHeight)
  {
    row = (int) ((double) row * imageFactorWidth);
    column = (int) ((double) column * imageFactorHeight);
    for (int index1 = 0; index1 <= 1; ++index1)
    {
      for (int index2 = 0; index2 <= 1; ++index2)
      {
        int index3 = (row + index1) * (int) this.m_maskWidth + (column + index2);
        if (0 <= index3 && index3 < this.m_maskedPixels.Length && this.m_maskedPixels[index3] != 1)
          return false;
      }
    }
    return true;
  }

  public int[] GetStencilMaskedPixels(byte[] data, float maskWidth, float maskHeight)
  {
    int index1 = 0;
    if (this.m_imageDictionary.ContainsKey("Decode") && this.m_colorspace == null)
    {
      this.m_decodeArray = this.m_imageDictionary["Decode"] as PdfArray;
      index1 = (this.m_decodeArray.Elements[index1] as PdfNumber).IntValue;
    }
    int num1 = -16777216 /*0xFF000000*/;
    int[] stencilMaskedPixels = new int[(int) ((double) maskHeight * (double) maskWidth)];
    BitParser bitParser = this.m_maskDictionary == null || !this.m_maskDictionary.ContainsKey("BitsPerComponent") ? new BitParser(data, (int) this.BitsPerComponent) : new BitParser(data, (this.m_maskDictionary["BitsPerComponent"] as PdfNumber).IntValue);
    int index2 = 0;
    for (int index3 = 0; (double) index3 < (double) maskHeight; ++index3)
    {
      for (int index4 = 0; (double) index4 < (double) maskWidth; ++index4)
      {
        int num2 = bitParser.ReadBits();
        stencilMaskedPixels[index2] = num2 != index1 ? num2 : num1;
        ++index2;
      }
      bitParser.MoveToNextRow();
    }
    return stencilMaskedPixels;
  }

  public static Image SetOpacity(Image image, float opacity)
  {
    ColorMatrix newColorMatrix = new ColorMatrix();
    newColorMatrix.Matrix33 = opacity;
    ImageAttributes imageAttr = new ImageAttributes();
    imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
    Bitmap bitmap = new Bitmap(image.Width, image.Height);
    using (Graphics graphics = Graphics.FromImage((Image) bitmap))
    {
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
    }
    return (Image) bitmap;
  }

  private float ConvertToFloat(byte byteValue)
  {
    float num = (float) byteValue / (float) byte.MaxValue;
    if ((double) num <= 0.0)
      return 0.0f;
    if ((double) num <= 0.04045)
      return num / 12.92f;
    return (double) num < 1.0 ? (float) Math.Pow(((double) num + 0.055) / 1.055, 2.4) : 1f;
  }

  private byte ConvertToByte(float value)
  {
    if ((double) value <= 0.0)
      return 0;
    if ((double) value <= 0.0031308)
      return (byte) ((double) byte.MaxValue * (double) value * 12.920000076293945 + 0.5);
    return (double) value < 1.0 ? (byte) ((double) byte.MaxValue * (1.0549999475479126 * Math.Pow((double) value, 5.0 / 12.0) - 0.054999999701976776) + 0.5) : byte.MaxValue;
  }

  private Bitmap DecodeMaskImage(MemoryStream mask)
  {
    PixelFormat format1 = PixelFormat.Format8bppIndexed;
    if ((double) this.m_maskBitsPerComponent == 1.0)
    {
      PixelFormat format2 = PixelFormat.Format1bppIndexed;
      byte[] buffer = mask.GetBuffer();
      Bitmap bitmap = new Bitmap((int) this.m_maskWidth, (int) this.m_maskHeight, format2);
      BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, (int) this.m_maskWidth, (int) this.m_maskHeight), ImageLockMode.ReadWrite, bitmap.PixelFormat);
      int stride = bitmapdata.Stride;
      IntPtr scan0 = bitmapdata.Scan0;
      int pixelFormatSize = Image.GetPixelFormatSize(bitmap.PixelFormat);
      int length = (int) this.m_maskWidth * pixelFormatSize / 8;
      if ((int) this.m_maskWidth * pixelFormatSize % 8 != 0)
        ++length;
      int startIndex = 0;
      long int64 = bitmapdata.Scan0.ToInt64();
      for (int index = 0; (double) index < (double) this.m_maskHeight; ++index)
      {
        Marshal.Copy(buffer, startIndex, new IntPtr(int64), length);
        startIndex += length;
        int64 += (long) bitmapdata.Stride;
      }
      bitmap.UnlockBits(bitmapdata);
      return bitmap;
    }
    byte[] buffer1 = mask.GetBuffer();
    Bitmap bitmap1 = new Bitmap((int) this.m_maskWidth, (int) this.m_maskHeight, format1);
    ColorPalette palette = bitmap1.Palette;
    for (int index = 0; index < palette.Entries.Length; ++index)
      palette.Entries[index] = Color.FromArgb(index, index, index);
    bitmap1.Dispose();
    Bitmap bitmap2 = new Bitmap((int) this.m_maskWidth, (int) this.m_maskHeight, format1);
    BitmapData bitmapdata1 = bitmap2.LockBits(new Rectangle(0, 0, (int) this.m_maskWidth, (int) this.m_maskHeight), ImageLockMode.ReadWrite, format1);
    Math.Abs(bitmapdata1.Stride);
    int height = bitmap2.Height;
    if (format1 == PixelFormat.Format8bppIndexed)
    {
      int startIndex = 0;
      long int64 = bitmapdata1.Scan0.ToInt64();
      for (int index = 0; (double) index < (double) this.m_maskHeight; ++index)
      {
        Marshal.Copy(buffer1, startIndex, new IntPtr(int64), (int) this.m_maskWidth);
        startIndex += (int) this.m_maskWidth;
        int64 += (long) bitmapdata1.Stride;
      }
    }
    else
      Marshal.Copy(buffer1, 0, bitmapdata1.Scan0, buffer1.Length);
    bitmap2.Palette = palette;
    bitmap2.UnlockBits(bitmapdata1);
    return bitmap2;
  }

  private MemoryStream DecodeDeviceGrayImage(MemoryStream imageStr)
  {
    byte[] numArray1 = new byte[imageStr.Length];
    byte[] array = imageStr.ToArray();
    int bitsPerComponent = (int) this.BitsPerComponent;
    int num1 = 0;
    bool flag = false;
    Bitmap bitmap = (Bitmap) null;
    switch (bitsPerComponent)
    {
      case 1:
        bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format24bppRgb);
        BitmapData bitmapdata1 = bitmap.LockBits(new Rectangle(0, 0, (int) this.Width, (int) this.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        if (array.Length == bitmapdata1.Stride * bitmap.Height)
        {
          bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format1bppIndexed);
          Marshal.Copy(array, 0, bitmapdata1.Scan0, array.Length);
        }
        else
        {
          byte[] index1 = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 0,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue
          };
          if (this.colorSpaceResourceDict.ContainsKey("Indexed") && this.internalColorSpace == "ICCBased")
          {
            byte[] numArray2 = new byte[6];
            int index2 = 0;
            for (int index3 = 0; index3 < this.m_colorPalette.Entries.Length && index2 < numArray2.Length; ++index3)
            {
              byte r = this.m_colorPalette.Entries[index3].R;
              numArray2[index2] = r;
              int num2;
              numArray2[num2 = index2 + 1] = r;
              int num3;
              numArray2[num3 = num2 + 1] = r;
              index2 = num3 + 1;
            }
            index1 = numArray2;
          }
          byte[] flat = this.ConvertIndexedStreamToFlat(bitsPerComponent, (int) this.Width, (int) this.Height, imageStr.GetBuffer(), index1, false, false);
          int num4 = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
          for (int index4 = 0; index4 + 3 <= flat.Length; index4 += 3)
          {
            int index5 = index4 + 2;
            byte num5 = flat[index5];
            flat[index5] = flat[index4];
            flat[index4] = num5;
          }
          int startIndex = 0;
          long int64 = bitmapdata1.Scan0.ToInt64();
          int length = (int) this.Width;
          if (num4 == 3)
            length = (int) this.Width * 3;
          for (int index6 = 0; (double) index6 < (double) this.Height; ++index6)
          {
            Marshal.Copy(flat, startIndex, new IntPtr(int64), length);
            startIndex += length;
            int64 += (long) bitmapdata1.Stride;
          }
        }
        bitmap.UnlockBits(bitmapdata1);
        break;
      case 4:
        byte[] numArray3 = (byte[]) null;
        if (this.m_colorspaceStream != null)
        {
          numArray3 = this.m_colorspaceStream.ToArray();
          flag = true;
        }
        if (flag)
        {
          int length1 = (int) this.Width * (int) this.Height * 3;
          byte[] numArray4 = new byte[length1];
          int[] numArray5 = new int[2]{ 4, 0 };
          int num6 = 0;
          for (int index7 = 0; index7 < array.Length; ++index7)
          {
            for (int index8 = 0; index8 < 2; ++index8)
            {
              int index9 = (int) array[index7] >> numArray5[index8] & 15;
              if (num1 < length1)
              {
                byte[] numArray6 = numArray4;
                int index10 = num1;
                int num7 = index10 + 1;
                int num8 = (int) numArray3[index9];
                numArray6[index10] = (byte) num8;
                byte[] numArray7 = numArray4;
                int index11 = num7;
                int num9 = index11 + 1;
                int num10 = (int) numArray3[index9];
                numArray7[index11] = (byte) num10;
                byte[] numArray8 = numArray4;
                int index12 = num9;
                num1 = index12 + 1;
                int num11 = (int) numArray3[index9];
                numArray8[index12] = (byte) num11;
                ++num6;
                if (num6 == (int) this.Width)
                {
                  num6 = 0;
                  index8 = 8;
                }
              }
              else
                break;
            }
          }
          byte[] numArray9 = numArray4;
          bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format24bppRgb);
          BitmapData bitmapdata2 = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
          int num12 = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
          byte[] numArray10 = new byte[length1];
          int num13 = length1;
          byte[] numArray11 = numArray9;
          int index13 = 0;
          int index14 = 0;
          for (; index13 < num13; index13 += 3)
          {
            numArray10[index14 + 2] = numArray11[index13];
            numArray10[index14 + 1] = numArray11[index13 + 1];
            numArray10[index14] = numArray11[index13 + 2];
            index14 += 3;
          }
          byte[] source = numArray10;
          int startIndex = 0;
          long int64 = bitmapdata2.Scan0.ToInt64();
          int length2 = (int) this.Width;
          if (num12 == 3)
            length2 = (int) this.Width * 3;
          for (int index15 = 0; (double) index15 < (double) this.Height; ++index15)
          {
            Marshal.Copy(source, startIndex, new IntPtr(int64), length2);
            startIndex += length2;
            int64 += (long) bitmapdata2.Stride;
          }
          bitmap.UnlockBits(bitmapdata2);
          break;
        }
        break;
      case 8:
        int index16 = 0;
        byte[] numArray12 = (byte[]) null;
        if (this.m_colorspaceStream != null)
        {
          numArray12 = this.m_colorspaceStream.ToArray();
          flag = true;
        }
        if (flag)
        {
          byte[] numArray13 = new byte[(int) this.Width * (int) this.Height];
          for (int index17 = 0; index17 < array.Length; ++index17)
          {
            int index18 = (int) array[index17];
            numArray13[num1++] = numArray12[index18];
          }
          byte[] numArray14 = numArray13;
          bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format24bppRgb);
          BitmapData bitmapdata3 = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
          byte[] source = new byte[(int) this.Width * (int) this.Height * 3];
          for (int index19 = 0; index19 < numArray14.Length; ++index19)
          {
            source[index16] = numArray14[index19];
            source[index16 + 1] = numArray14[index19];
            source[index16 + 2] = numArray14[index19];
            index16 += 3;
          }
          int startIndex = 0;
          long int64 = bitmapdata3.Scan0.ToInt64();
          int length = (int) this.Width * 3;
          for (int index20 = 0; (double) index20 < (double) this.Height; ++index20)
          {
            Marshal.Copy(source, startIndex, new IntPtr(int64), length);
            startIndex += length;
            int64 += (long) bitmapdata3.Stride;
          }
          bitmap.UnlockBits(bitmapdata3);
          break;
        }
        break;
    }
    if (!flag && bitsPerComponent != 1)
    {
      bitmap = new Bitmap((int) this.Width, (int) this.Height, PixelFormat.Format32bppArgb);
      for (int y = 0; y < (int) this.Height; ++y)
      {
        int num14 = 0;
        int num15 = 0;
        for (int x = 0; x < (int) this.Width; ++x)
        {
          if (num15 < bitsPerComponent)
          {
            num14 = num14 << 8 | imageStr.ReadByte();
            num15 += 8;
          }
          byte num16 = (byte) (num14 >> num15 - bitsPerComponent);
          num14 ^= (int) num16 << num15 - bitsPerComponent;
          num15 -= bitsPerComponent;
          Color color = Color.FromArgb((int) byte.MaxValue, (int) num16, (int) num16, (int) num16);
          bitmap.SetPixel(x, y, color);
        }
      }
    }
    MemoryStream memoryStream = new MemoryStream();
    if (this.IsTransparent)
      bitmap.MakeTransparent();
    bitmap.Save((Stream) memoryStream, ImageFormat.Png);
    return memoryStream;
  }

  private MemoryStream DecodePredictor(int predictor, int colors, int columns, MemoryStream data)
  {
    MemoryStream memoryStream = new MemoryStream();
    if (predictor == 1)
    {
      memoryStream = data;
    }
    else
    {
      int offset1 = (int) (((double) colors * (double) this.BitsPerComponent + 7.0) / 8.0);
      int length = (int) (((double) (columns * colors) * (double) this.BitsPerComponent + 7.0) / 8.0 + (double) offset1);
      byte[] buffer1 = new byte[length];
      byte[] numArray = new byte[length];
      int num1 = predictor;
      bool flag = false;
      data.Position = 0L;
      while (!flag && data.Position < data.Length)
      {
        if (predictor >= 10)
        {
          byte[] buffer2 = new byte[1];
          data.Read(buffer2, 0, 1);
          int num2 = (int) buffer2[0];
          if (num2 == -1)
            break;
          num1 = num2 + 10;
        }
        int offset2 = offset1;
        int num3;
        while (offset2 < length && (num3 = data.Read(buffer1, offset2, length - offset2)) != -1)
        {
          offset2 += num3;
          if (num3 == 0)
            break;
        }
        switch (num1)
        {
          case 2:
            for (int index = offset1; index < length; ++index)
            {
              int num4 = (int) buffer1[index] & (int) byte.MaxValue;
              int num5 = (int) buffer1[index - offset1] & (int) byte.MaxValue;
              buffer1[index] = (byte) (num4 + num5);
            }
            break;
          case 11:
            for (int index = offset1; index < length; ++index)
            {
              int num6 = (int) buffer1[index] & (int) byte.MaxValue;
              int num7 = (int) buffer1[index - offset1] & (int) byte.MaxValue;
              buffer1[index] = (byte) (num6 + num7);
            }
            break;
          case 12:
            for (int index = offset1; index < length; ++index)
            {
              int num8 = (int) buffer1[index] & (int) byte.MaxValue;
              int num9 = (int) numArray[index] & (int) byte.MaxValue;
              buffer1[index] = (byte) (num8 + num9);
            }
            break;
          case 13:
            for (int index = offset1; index < length; ++index)
            {
              int num10 = (int) buffer1[index] & (int) byte.MaxValue;
              int num11 = (int) buffer1[index - offset1] & (int) byte.MaxValue;
              int num12 = (int) numArray[index] & (int) byte.MaxValue;
              buffer1[index] = (byte) (num10 + (num11 + num12) / 2);
            }
            break;
          case 14:
            for (int index = offset1; index < length; ++index)
            {
              int num13 = (int) buffer1[index] & (int) byte.MaxValue;
              int num14 = (int) buffer1[index - offset1] & (int) byte.MaxValue;
              int num15 = (int) numArray[index] & (int) byte.MaxValue;
              int num16 = (int) numArray[index - offset1] & (int) byte.MaxValue;
              int num17 = num14 + num15 - num16;
              int num18 = Math.Abs(num17 - num14);
              int num19 = Math.Abs(num17 - num15);
              int num20 = Math.Abs(num17 - num16);
              buffer1[index] = num18 > num19 || num18 > num20 ? (num19 > num20 ? (byte) (num13 + num16) : (byte) (num13 + num15)) : (byte) (num13 + num14);
            }
            break;
        }
        numArray = (byte[]) buffer1.Clone();
        memoryStream.Write(buffer1, offset1, buffer1.Length - offset1);
      }
    }
    return memoryStream;
  }

  public Image Decode()
  {
    Image image;
    try
    {
      ImagePreRenderEventArgs args = new ImagePreRenderEventArgs(this);
      if (ImageStructure.ImagePreRender != null)
      {
        ImageStructure.ImagePreRender((object) null, args);
        image = Image.FromStream(args.ImageStream);
      }
      else
        image = Image.FromStream(this.GetImageStream());
    }
    catch (Exception ex)
    {
      image = (Image) null;
    }
    return image;
  }

  private byte[] YCCKtoRGB(byte[] encodedData)
  {
    byte[] numArray1 = new byte[encodedData.Length];
    double width = (double) this.Width;
    double height = (double) this.Height;
    int num1 = 0;
    for (int index1 = 0; index1 + 3 < encodedData.Length; index1 += 3)
    {
      double num2 = (double) ((int) encodedData[index1] & (int) byte.MaxValue);
      double num3 = (double) ((int) encodedData[index1 + 1] & (int) byte.MaxValue);
      double num4 = (double) ((int) encodedData[index1 + 2] & (int) byte.MaxValue);
      double num5 = (double) byte.MaxValue - num2;
      double num6 = (double) byte.MaxValue - num3;
      double num7 = (double) byte.MaxValue - num4;
      byte[] numArray2 = numArray1;
      int index2 = num1;
      int num8 = index2 + 1;
      int num9 = (int) (byte) num5;
      numArray2[index2] = (byte) num9;
      byte[] numArray3 = numArray1;
      int index3 = num8;
      int num10 = index3 + 1;
      int num11 = (int) (byte) num6;
      numArray3[index3] = (byte) num11;
      byte[] numArray4 = numArray1;
      int index4 = num10;
      num1 = index4 + 1;
      int num12 = (int) (byte) num7;
      numArray4[index4] = (byte) num12;
    }
    return numArray1;
  }

  private byte[] YCCToRGB(byte[] encodedData)
  {
    byte[] rgb = new byte[(int) this.Width * (int) this.Height * 3];
    int num1 = (int) this.Width * (int) this.Height * 4;
    double num2 = -1.0;
    double num3 = -1.12;
    double num4 = -1.12;
    double num5 = -1.21;
    double maxValue = (double) byte.MaxValue;
    int num6 = 0;
    for (int index1 = 0; index1 < num1 && index1 <= encodedData.Length; index1 += 4)
    {
      double num7 = (double) ((int) encodedData[index1] & (int) byte.MaxValue) / maxValue;
      double num8 = (double) ((int) encodedData[index1 + 1] & (int) byte.MaxValue) / maxValue;
      double num9 = (double) ((int) encodedData[index1 + 2] & (int) byte.MaxValue) / maxValue;
      double num10 = (double) ((int) encodedData[index1 + 3] & (int) byte.MaxValue) / maxValue;
      double num11 = 0.0;
      double num12 = 0.0;
      double num13 = 0.0;
      if (num2 != num7 || num3 != num8 || num4 != num9 || num5 != num10)
      {
        double num14 = num7;
        double num15 = num8;
        double num16 = num9;
        double num17 = num10;
        num11 = (double) byte.MaxValue + num14 * (-4.3873323846099881 * num14 + 54.486151941891762 * num15 + 18.822905021653021 * num16 + 212.25662451639585 * num17 - 285.2331026137004) + num15 * (1.7149763477362134 * num15 - 5.6096736904047315 * num16 - 17.873870861415444 * num17 - 5.4970064271963661) + num16 * (-2.5217340131683033 * num16 - 21.248923337353073 * num17 + 17.5119270841813) - num17 * (21.86122147463605 * num17 + 189.48180835922747);
        num12 = (double) byte.MaxValue + num14 * (8.8410414220361488 * num14 + 60.118027045597366 * num15 + 6.8714255920490066 * num16 + 31.159100130055922 * num17 - 79.2970844816548) + num15 * (-15.310361306967817 * num15 + 17.575251261109482 * num16 + 131.35250912493976 * num17 - 190.9453302588951) + num16 * (4.444339102852739 * num16 + 9.8632861493405 * num17 - 24.86741582555878) - num17 * (20.737325471181034 * num17 + 187.80453709719578);
        num13 = (double) byte.MaxValue + num14 * (0.88425224300032956 * num14 + 8.0786775031129281 * num15 + 30.89978309703729 * num16 - 0.23883238689178934 * num17 - 14.183576799673286) + num15 * (10.49593273432072 * num15 + 63.02378494754052 * num16 + 50.606957656360734 * num17 - 112.23884253719248) + num16 * (0.032960411148732167 * num16 + 115.60384449646641 * num17 - 193.58209356861505) - num17 * (22.33816807309886 * num17 + 180.12613974708367);
      }
      byte[] numArray1 = rgb;
      int index2 = num6;
      int num18 = index2 + 1;
      int num19 = (int) (byte) num11;
      numArray1[index2] = (byte) num19;
      byte[] numArray2 = rgb;
      int index3 = num18;
      int num20 = index3 + 1;
      int num21 = (int) (byte) num12;
      numArray2[index3] = (byte) num21;
      byte[] numArray3 = rgb;
      int index4 = num20;
      num6 = index4 + 1;
      int num22 = (int) (byte) num13;
      numArray3[index4] = (byte) num22;
    }
    return rgb;
  }

  private byte[] GetIndexedPixelData(byte[] encodedData)
  {
    int bitsPerComponent = (int) this.BitsPerComponent;
    BitParser bitParser = new BitParser(encodedData, bitsPerComponent);
    byte[] indexedPixelData = new byte[(int) this.Width * (int) this.Height * 4];
    double[] numArray1 = this.DefaultDecodeArrayForColorspace();
    double xMinimum = 0.0;
    double xMaximum = Math.Pow(2.0, (double) bitsPerComponent) - 1.0;
    byte[] array = this.colorSpaceResourceDict["Indexed"].ToArray();
    byte[] numArray2 = new byte[4];
    int index1 = 0;
    if (this.m_colorspaceBase == "DeviceGray" || this.m_colorspaceBase == "CalGray")
      this.m_isDeviceGrayColorspace = true;
    else if (this.m_colorspaceBase == "DeviceRGB" || this.m_colorspaceBase == "CalRGB")
      this.m_isDeviceRGBColorspace = true;
    else if (this.m_colorspaceBase == "DeviceCMYK" || this.m_colorspaceBase == "CalCMYK")
      this.m_isDeviceCMYKColorspace = true;
    for (int index2 = 0; (double) index2 < (double) this.Height; ++index2)
    {
      for (int index3 = 0; (double) index3 < (double) this.Width; ++index3)
      {
        byte[] indexedColor = this.GetIndexedColor((int) this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray1[0], numArray1[1]), array);
        indexedPixelData[index1] = indexedColor[0];
        indexedPixelData[index1 + 1] = indexedColor[1];
        indexedPixelData[index1 + 2] = indexedColor[2];
        indexedPixelData[index1 + 3] = indexedColor[3];
        index1 += 4;
      }
      bitParser.MoveToNextRow();
    }
    return indexedPixelData;
  }

  private byte[] GetIndexedColor(int index, byte[] colorBytes)
  {
    byte[] indexedColor1 = new byte[4]
    {
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      byte.MaxValue
    };
    if (this.m_colorspaceBase == null || this.m_colorspaceStream == null)
      return indexedColor1;
    if (index < 0)
      index = 0;
    if (index > this.m_colorspaceHival)
      index = this.m_colorspaceHival;
    byte maxValue = byte.MaxValue;
    if (this.m_isDeviceGrayColorspace)
    {
      int index1 = index;
      Color.FromArgb((int) maxValue, (int) colorBytes[index1], (int) colorBytes[index1], (int) colorBytes[index1]);
    }
    else
    {
      if (this.m_isDeviceRGBColorspace)
      {
        byte[] indexedColor2 = new byte[4];
        int index2 = index * 3;
        if (colorBytes.Length >= index2 + 2)
        {
          indexedColor2[0] = maxValue;
          indexedColor2[1] = colorBytes[index2];
          indexedColor2[2] = colorBytes[index2 + 1];
          indexedColor2[3] = colorBytes[index2 + 2];
        }
        return indexedColor2;
      }
      if (this.m_isDeviceCMYKColorspace)
      {
        byte[] indexedColor3 = new byte[4];
        byte[] data = new byte[4];
        int index3 = index * 4;
        data[0] = colorBytes[index3];
        data[1] = colorBytes[index3 + 1];
        data[2] = colorBytes[index3 + 2];
        data[3] = colorBytes[index3 + 3];
        byte[] rgb = this.ConvertIndexCMYKToRGB(data);
        indexedColor3[0] = byte.MaxValue;
        indexedColor3[1] = rgb[0];
        indexedColor3[2] = rgb[1];
        indexedColor3[3] = rgb[2];
        return indexedColor3;
      }
    }
    return indexedColor1;
  }

  private byte[] ConvertCMYKtoRGBColor(byte cyan, byte magenta, byte yellow, byte black)
  {
    return new byte[4]
    {
      byte.MaxValue,
      (byte) ((int) byte.MaxValue - Math.Min((int) byte.MaxValue, (int) cyan + (int) black)),
      (byte) ((int) byte.MaxValue - Math.Min((int) byte.MaxValue, (int) magenta + (int) black)),
      (byte) ((int) byte.MaxValue - Math.Min((int) byte.MaxValue, (int) yellow + (int) black))
    };
  }

  private int[] RenderRGBPixels(byte[] encodedData)
  {
    int bitsPerComponent = (int) this.BitsPerComponent;
    BitParser bitParser = new BitParser(encodedData, bitsPerComponent);
    List<ColorConvertor> list = new List<ColorConvertor>((int) this.Width * (int) this.Height);
    double[] numArray = this.DefaultDecodeArrayForColorspace();
    if (this.m_indexedRGBvalues != null)
    {
      bool flag = true;
      for (int index = 0; index < this.m_indexedRGBvalues.Length; ++index)
      {
        if (this.m_indexedRGBvalues[index] != byte.MaxValue)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        numArray = new double[2]{ 1.0, 1.0 };
    }
    double xMinimum = 0.0;
    double xMaximum = Math.Pow(2.0, (double) bitsPerComponent) - 1.0;
    for (int index1 = 0; (double) index1 < (double) this.Height; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) this.Width; ++index2)
      {
        if (this.ColorSpace != "DeviceGray" && this.m_colorspaceBase != "DeviceGray")
          list.Add(ColorConvertor.FromArgb(1.0, this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[0], numArray[1]), this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[2], numArray[3]), this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[4], numArray[5])));
        else
          list.Add(ColorConvertor.FromArgb(1.0, this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[0], numArray[1]), this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[0], numArray[1]), this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[0], numArray[1])));
      }
      bitParser.MoveToNextRow();
    }
    return this.RenderImagePixel(list);
  }

  private int[] CMYKtoRGBPixels(byte[] encodedData)
  {
    int bitsPerComponent = (int) this.BitsPerComponent;
    BitParser bitParser = new BitParser(encodedData, bitsPerComponent);
    List<ColorConvertor> list = new List<ColorConvertor>((int) this.Width * (int) this.Height);
    double[] numArray = this.DefaultDecodeArrayForColorspace();
    double xMinimum = 0.0;
    double xMaximum = Math.Pow(2.0, (double) bitsPerComponent) - 1.0;
    for (int index1 = 0; (double) index1 < (double) this.Height; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) this.Width; ++index2)
        list.Add(ColorConvertor.FromCmyk(this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[0], numArray[1]), this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[2], numArray[3]), this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[4], numArray[5]), this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[6], numArray[7])));
      bitParser.MoveToNextRow();
    }
    return this.RenderImagePixel(list);
  }

  private ColorConvertor[] RenderGrayPixels(byte[] encodedData)
  {
    int bitsPerComponent = (int) this.BitsPerComponent;
    BitParser bitParser = new BitParser(encodedData, bitsPerComponent);
    List<ColorConvertor> colorConvertorList = new List<ColorConvertor>((int) this.Width * (int) this.Height);
    double[] numArray = this.DefaultDecodeArrayForColorspace();
    double xMinimum = 0.0;
    double xMaximum = Math.Pow(2.0, (double) bitsPerComponent) - 1.0;
    for (int index1 = 0; (double) index1 < (double) this.Height; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) this.Width; ++index2)
        colorConvertorList.Add(ColorConvertor.FromGray(this.Interject((double) bitParser.ReadBits(), xMinimum, xMaximum, numArray[0], numArray[1])));
      bitParser.MoveToNextRow();
    }
    return colorConvertorList.ToArray();
  }

  private double[] DefaultDecodeArrayForColorspace()
  {
    double[] numArray1 = (double[]) null;
    if (this.ColorSpace == null)
    {
      PdfReferenceHolder image = this.ImageDictionary["ColorSpace"] as PdfReferenceHolder;
      if (image != (PdfReferenceHolder) null)
      {
        PdfName pdfName = image.Object as PdfName;
        if (pdfName != (PdfName) null)
          this.ColorSpace = pdfName.Value;
      }
    }
    switch (this.m_colorspaceBase)
    {
      case "DeviceCMYK":
        numArray1 = new double[8]
        {
          0.0,
          1.0,
          0.0,
          1.0,
          0.0,
          1.0,
          0.0,
          1.0
        };
        break;
      case "CalRGB":
      case "DeviceRGB":
        numArray1 = new double[6]
        {
          0.0,
          1.0,
          0.0,
          1.0,
          0.0,
          1.0
        };
        break;
      case "DeviceGray":
        numArray1 = new double[3]{ 1.0, 0.0, 0.0 };
        break;
      case "Indexed":
        numArray1 = new double[2]
        {
          0.0,
          Math.Pow(2.0, (double) this.BitsPerComponent) - 1.0
        };
        break;
    }
    double[] numArray2;
    if (this.DecodeArray != null && this.DecodeArray.Count > 0)
    {
      numArray2 = new double[this.DecodeArray.Count];
      for (int index = 0; index < numArray2.Length; ++index)
        numArray2[index] = (double) (this.DecodeArray[index] as PdfNumber).FloatValue;
    }
    else
      numArray2 = new double[2]{ 0.0, 1.0 };
    switch (this.ColorSpace)
    {
      case "DeviceCMYK":
        numArray2 = new double[8]
        {
          0.0,
          1.0,
          0.0,
          1.0,
          0.0,
          1.0,
          0.0,
          1.0
        };
        break;
      case "CalRGB":
      case "DeviceRGB":
        numArray2 = new double[6]
        {
          0.0,
          1.0,
          0.0,
          1.0,
          0.0,
          1.0
        };
        break;
      case "DeviceGray":
        numArray2 = new double[2]{ 0.0, 1.0 };
        break;
      case "Indexed":
        numArray2 = new double[2]
        {
          0.0,
          Math.Pow(2.0, (double) this.BitsPerComponent) - 1.0
        };
        break;
    }
    return numArray2;
  }

  private int[] RenderImagePixel(List<ColorConvertor> list)
  {
    ColorConvertor[] array = list.ToArray();
    if (this.m_imageDictionary.ContainsKey("SMask"))
    {
      PdfDictionary sMaskDictionary = (this.m_imageDictionary["SMask"] as PdfReferenceHolder).Object as PdfDictionary;
      PdfStream pdfStream = sMaskDictionary as PdfStream;
      pdfStream.Decompress();
      byte[] data = pdfStream.Data;
      if (data != null)
        this.RenderAlphaMask(array, data, sMaskDictionary);
    }
    int[] numArray = new int[array.Length];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = array[index].PixelConversion();
    this.m_isSoftMasked = true;
    return numArray;
  }

  private void RenderAlphaMask(
    ColorConvertor[] pixels,
    byte[] array,
    PdfDictionary sMaskDictionary)
  {
    float num1 = 0.0f;
    if (sMaskDictionary.ContainsKey("BitsPerComponent"))
      num1 = (float) (sMaskDictionary["BitsPerComponent"] as PdfNumber).IntValue;
    this.m_bitsPerComponent = num1;
    ColorConvertor[] colorConvertorArray = this.RenderGrayPixels(array);
    int num2 = Math.Min(colorConvertorArray.Length, pixels.Length);
    for (int index = 0; index < num2; ++index)
      pixels[index].Alpha = colorConvertorArray[index].GetGrayComponent();
  }

  private double Interject(
    double x,
    double xMinimum,
    double xMaximum,
    double yMinimum,
    double yMaximum)
  {
    return yMinimum + (x - xMinimum) * ((yMaximum - yMinimum) / (xMaximum - xMinimum));
  }

  private double RoundOff(double value)
  {
    if (value < 0.0)
      value = 0.0;
    if (value > 1.0)
      value = 1.0;
    return value;
  }

  private string[] GetImageFilter()
  {
    string[] imageFilter = (string[]) null;
    if (this.m_imageDictionary != null && this.m_imageDictionary.ContainsKey("Filter"))
    {
      if ((object) (this.m_imageDictionary["Filter"] as PdfName) != null)
        imageFilter = new string[1]
        {
          (this.m_imageDictionary["Filter"] as PdfName).Value
        };
      else if (this.m_imageDictionary["Filter"] is PdfArray)
      {
        PdfArray image = this.m_imageDictionary["Filter"] as PdfArray;
        imageFilter = new string[image.Count];
        for (int index = 0; index < image.Count; ++index)
          imageFilter[index] = (image[index] as PdfName).Value;
      }
      else if ((object) (this.m_imageDictionary["Filter"] as PdfReferenceHolder) != null)
      {
        if ((this.m_imageDictionary["Filter"] as PdfReferenceHolder).Object is PdfArray)
        {
          PdfArray pdfArray = (this.m_imageDictionary["Filter"] as PdfReferenceHolder).Object as PdfArray;
          imageFilter = new string[pdfArray.Count];
          for (int index = 0; index < pdfArray.Count; ++index)
            imageFilter[index] = (pdfArray[index] as PdfName).Value;
        }
        else if ((object) ((this.m_imageDictionary["Filter"] as PdfReferenceHolder).Object as PdfName) != null)
          imageFilter = new string[1]
          {
            ((this.m_imageDictionary["Filter"] as PdfReferenceHolder).Object as PdfName).Value
          };
      }
    }
    return imageFilter;
  }

  private void GetIsImageMask()
  {
    if (!this.m_imageDictionary.ContainsKey("ImageMask"))
      return;
    this.m_isImageMask = (this.m_imageDictionary["ImageMask"] as PdfBoolean).Value;
  }

  private PdfDictionary[] GetDecodeParam(PdfDictionary imageDictionary)
  {
    PdfDictionary[] decodeParam = (PdfDictionary[]) null;
    if (imageDictionary != null && imageDictionary.ContainsKey("DecodeParms"))
    {
      if (imageDictionary["DecodeParms"] is PdfDictionary)
        decodeParam = new PdfDictionary[1]
        {
          imageDictionary["DecodeParms"] as PdfDictionary
        };
      else if (imageDictionary["DecodeParms"] is PdfArray)
      {
        PdfArray image = imageDictionary["DecodeParms"] as PdfArray;
        decodeParam = new PdfDictionary[image.Count];
        for (int index = 0; index < image.Count; ++index)
          decodeParam[index] = image[index] as PdfDictionary;
      }
    }
    return decodeParam;
  }

  private float GetImageHeight()
  {
    float imageHeight = 0.0f;
    if (this.m_imageDictionary != null && this.m_imageDictionary.ContainsKey("Height"))
    {
      if ((object) (this.m_imageDictionary["Height"] as PdfReferenceHolder) != null)
        return ((this.m_imageDictionary["Height"] as PdfReferenceHolder).Object as PdfNumber).FloatValue;
      imageHeight = (this.m_imageDictionary["Height"] as PdfNumber).FloatValue;
    }
    return imageHeight;
  }

  private float GetBitsPerComponent()
  {
    float bitsPerComponent = 0.0f;
    if (this.m_imageDictionary != null)
      bitsPerComponent = (this.m_imageDictionary["BitsPerComponent"] as PdfNumber).FloatValue;
    return bitsPerComponent;
  }

  private bool GetIsEarlyChange()
  {
    PdfDictionary[] decodeParam = this.GetDecodeParam(this.m_imageDictionary);
    if (decodeParam != null)
    {
      if (decodeParam[0] != null && decodeParam[0].ContainsKey(new PdfName("EarlyChange")))
        this.m_isEarlyChange = (decodeParam[0][new PdfName("EarlyChange")] as PdfNumber).IntValue != 0;
    }
    else
      this.m_isEarlyChange = true;
    return this.m_isEarlyChange;
  }

  private void GetDecodeArray()
  {
    if (!this.m_imageDictionary.ContainsKey("Decode"))
      return;
    this.m_decodeArray = this.m_imageDictionary["Decode"] as PdfArray;
  }

  private float GetImageWidth()
  {
    float imageWidth = 0.0f;
    if (this.m_imageDictionary != null && this.m_imageDictionary.ContainsKey("Width"))
      imageWidth = (this.m_imageDictionary["Width"] as PdfNumber).FloatValue;
    return imageWidth;
  }

  private static void Sub(byte[] currentLine, int sub)
  {
    for (int index1 = 0; index1 < currentLine.Length; ++index1)
    {
      int index2 = index1 - sub;
      if (index2 >= 0)
        currentLine[index1] += currentLine[index2];
    }
  }

  private void Up(byte[] currentLine, byte[] prevLine)
  {
    if (prevLine == null)
      return;
    for (int index = 0; index < currentLine.Length; ++index)
      currentLine[index] += prevLine[index];
  }

  private void Average(byte[] currentLine, byte[] prevLine, int sub)
  {
    for (int index1 = 0; index1 < currentLine.Length; ++index1)
    {
      int num1 = 0;
      int num2 = 0;
      int index2 = index1 - sub;
      if (index2 >= 0)
        num1 = (int) currentLine[index2] & (int) byte.MaxValue;
      if (prevLine != null)
        num2 = (int) prevLine[index1] & (int) byte.MaxValue;
      currentLine[index1] += (byte) Math.Floor((double) (num1 + num2) / 2.0);
    }
  }

  private void Paeth(byte[] currentLine, byte[] prevLine, int sub)
  {
    for (int index1 = 0; index1 < currentLine.Length; ++index1)
    {
      int left = 0;
      int up = 0;
      int upLeft = 0;
      int index2 = index1 - sub;
      if (index2 >= 0)
        left = (int) currentLine[index2] & (int) byte.MaxValue;
      if (prevLine != null)
        up = (int) prevLine[index1] & (int) byte.MaxValue;
      if (index2 > 0 && prevLine != null)
        upLeft = (int) prevLine[index2] & (int) byte.MaxValue;
      currentLine[index1] += (byte) this.Paeth(left, up, upLeft);
    }
  }

  private int Paeth(int left, int up, int upLeft)
  {
    int num1 = left + up - upLeft;
    int num2 = Math.Abs(num1 - left);
    int num3 = Math.Abs(num1 - up);
    int num4 = Math.Abs(num1 - upLeft);
    if (num2 <= num3 && num2 <= num4)
      return left;
    return num3 <= num4 ? up : upLeft;
  }

  public delegate void ImagePreRenderEventHandler(object sender, ImagePreRenderEventArgs args);
}
