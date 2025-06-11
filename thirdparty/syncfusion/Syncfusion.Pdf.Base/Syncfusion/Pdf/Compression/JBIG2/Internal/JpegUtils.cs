// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.JpegUtils
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class JpegUtils
{
  public static readonly int[] jpeg_natural_order = new int[80 /*0x50*/]
  {
    0,
    1,
    8,
    16 /*0x10*/,
    9,
    2,
    3,
    10,
    17,
    24,
    32 /*0x20*/,
    25,
    18,
    11,
    4,
    5,
    12,
    19,
    26,
    33,
    40,
    48 /*0x30*/,
    41,
    34,
    27,
    20,
    13,
    6,
    7,
    14,
    21,
    28,
    35,
    42,
    49,
    56,
    57,
    50,
    43,
    36,
    29,
    22,
    15,
    23,
    30,
    37,
    44,
    51,
    58,
    59,
    52,
    45,
    38,
    31 /*0x1F*/,
    39,
    46,
    53,
    60,
    61,
    54,
    47,
    55,
    62,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/,
    63 /*0x3F*/
  };

  public static int RIGHT_SHIFT(int x, int shft) => x >> shft;

  public static int DESCALE(int x, int n) => JpegUtils.RIGHT_SHIFT(x + (1 << n - 1), n);

  public static int jdiv_round_up(int a, int b) => (a + b - 1) / b;

  public static int jround_up(int a, int b)
  {
    a += b - 1;
    return a - a % b;
  }

  public static void jcopy_sample_rows(
    ComponentBuffer input_array,
    int source_row,
    byte[][] output_array,
    int dest_row,
    int num_rows,
    int num_cols)
  {
    for (int index = 0; index < num_rows; ++index)
      Buffer.BlockCopy((Array) input_array[source_row + index], 0, (Array) output_array[dest_row + index], 0, num_cols);
  }

  public static void jcopy_sample_rows(
    ComponentBuffer input_array,
    int source_row,
    ComponentBuffer output_array,
    int dest_row,
    int num_rows,
    int num_cols)
  {
    for (int index = 0; index < num_rows; ++index)
      Buffer.BlockCopy((Array) input_array[source_row + index], 0, (Array) output_array[dest_row + index], 0, num_cols);
  }

  public static void jcopy_sample_rows(
    byte[][] input_array,
    int source_row,
    byte[][] output_array,
    int dest_row,
    int num_rows,
    int num_cols)
  {
    for (int index = 0; index < num_rows; ++index)
      Buffer.BlockCopy((Array) input_array[source_row++], 0, (Array) output_array[dest_row++], 0, num_cols);
  }
}
