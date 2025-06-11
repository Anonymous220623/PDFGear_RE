// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.Ex1PassCQuantizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class Ex1PassCQuantizer : ColorQuantizer
{
  private const int MAX_Q_COMPS = 4;
  private const int ODITHER_SIZE = 16 /*0x10*/;
  private const int ODITHER_CELLS = 256 /*0x0100*/;
  private const int ODITHER_MASK = 15;
  private static readonly int[] RGB_order = new int[3]
  {
    1,
    0,
    2
  };
  private static readonly byte[][] base_dither_matrix = new byte[16 /*0x10*/][]
  {
    new byte[16 /*0x10*/]
    {
      (byte) 0,
      (byte) 192 /*0xC0*/,
      (byte) 48 /*0x30*/,
      (byte) 240 /*0xF0*/,
      (byte) 12,
      (byte) 204,
      (byte) 60,
      (byte) 252,
      (byte) 3,
      (byte) 195,
      (byte) 51,
      (byte) 243,
      (byte) 15,
      (byte) 207,
      (byte) 63 /*0x3F*/,
      byte.MaxValue
    },
    new byte[16 /*0x10*/]
    {
      (byte) 128 /*0x80*/,
      (byte) 64 /*0x40*/,
      (byte) 176 /*0xB0*/,
      (byte) 112 /*0x70*/,
      (byte) 140,
      (byte) 76,
      (byte) 188,
      (byte) 124,
      (byte) 131,
      (byte) 67,
      (byte) 179,
      (byte) 115,
      (byte) 143,
      (byte) 79,
      (byte) 191,
      (byte) 127 /*0x7F*/
    },
    new byte[16 /*0x10*/]
    {
      (byte) 32 /*0x20*/,
      (byte) 224 /*0xE0*/,
      (byte) 16 /*0x10*/,
      (byte) 208 /*0xD0*/,
      (byte) 44,
      (byte) 236,
      (byte) 28,
      (byte) 220,
      (byte) 35,
      (byte) 227,
      (byte) 19,
      (byte) 211,
      (byte) 47,
      (byte) 239,
      (byte) 31 /*0x1F*/,
      (byte) 223
    },
    new byte[16 /*0x10*/]
    {
      (byte) 160 /*0xA0*/,
      (byte) 96 /*0x60*/,
      (byte) 144 /*0x90*/,
      (byte) 80 /*0x50*/,
      (byte) 172,
      (byte) 108,
      (byte) 156,
      (byte) 92,
      (byte) 163,
      (byte) 99,
      (byte) 147,
      (byte) 83,
      (byte) 175,
      (byte) 111,
      (byte) 159,
      (byte) 95
    },
    new byte[16 /*0x10*/]
    {
      (byte) 8,
      (byte) 200,
      (byte) 56,
      (byte) 248,
      (byte) 4,
      (byte) 196,
      (byte) 52,
      (byte) 244,
      (byte) 11,
      (byte) 203,
      (byte) 59,
      (byte) 251,
      (byte) 7,
      (byte) 199,
      (byte) 55,
      (byte) 247
    },
    new byte[16 /*0x10*/]
    {
      (byte) 136,
      (byte) 72,
      (byte) 184,
      (byte) 120,
      (byte) 132,
      (byte) 68,
      (byte) 180,
      (byte) 116,
      (byte) 139,
      (byte) 75,
      (byte) 187,
      (byte) 123,
      (byte) 135,
      (byte) 71,
      (byte) 183,
      (byte) 119
    },
    new byte[16 /*0x10*/]
    {
      (byte) 40,
      (byte) 232,
      (byte) 24,
      (byte) 216,
      (byte) 36,
      (byte) 228,
      (byte) 20,
      (byte) 212,
      (byte) 43,
      (byte) 235,
      (byte) 27,
      (byte) 219,
      (byte) 39,
      (byte) 231,
      (byte) 23,
      (byte) 215
    },
    new byte[16 /*0x10*/]
    {
      (byte) 168,
      (byte) 104,
      (byte) 152,
      (byte) 88,
      (byte) 164,
      (byte) 100,
      (byte) 148,
      (byte) 84,
      (byte) 171,
      (byte) 107,
      (byte) 155,
      (byte) 91,
      (byte) 167,
      (byte) 103,
      (byte) 151,
      (byte) 87
    },
    new byte[16 /*0x10*/]
    {
      (byte) 2,
      (byte) 194,
      (byte) 50,
      (byte) 242,
      (byte) 14,
      (byte) 206,
      (byte) 62,
      (byte) 254,
      (byte) 1,
      (byte) 193,
      (byte) 49,
      (byte) 241,
      (byte) 13,
      (byte) 205,
      (byte) 61,
      (byte) 253
    },
    new byte[16 /*0x10*/]
    {
      (byte) 130,
      (byte) 66,
      (byte) 178,
      (byte) 114,
      (byte) 142,
      (byte) 78,
      (byte) 190,
      (byte) 126,
      (byte) 129,
      (byte) 65,
      (byte) 177,
      (byte) 113,
      (byte) 141,
      (byte) 77,
      (byte) 189,
      (byte) 125
    },
    new byte[16 /*0x10*/]
    {
      (byte) 34,
      (byte) 226,
      (byte) 18,
      (byte) 210,
      (byte) 46,
      (byte) 238,
      (byte) 30,
      (byte) 222,
      (byte) 33,
      (byte) 225,
      (byte) 17,
      (byte) 209,
      (byte) 45,
      (byte) 237,
      (byte) 29,
      (byte) 221
    },
    new byte[16 /*0x10*/]
    {
      (byte) 162,
      (byte) 98,
      (byte) 146,
      (byte) 82,
      (byte) 174,
      (byte) 110,
      (byte) 158,
      (byte) 94,
      (byte) 161,
      (byte) 97,
      (byte) 145,
      (byte) 81,
      (byte) 173,
      (byte) 109,
      (byte) 157,
      (byte) 93
    },
    new byte[16 /*0x10*/]
    {
      (byte) 10,
      (byte) 202,
      (byte) 58,
      (byte) 250,
      (byte) 6,
      (byte) 198,
      (byte) 54,
      (byte) 246,
      (byte) 9,
      (byte) 201,
      (byte) 57,
      (byte) 249,
      (byte) 5,
      (byte) 197,
      (byte) 53,
      (byte) 245
    },
    new byte[16 /*0x10*/]
    {
      (byte) 138,
      (byte) 74,
      (byte) 186,
      (byte) 122,
      (byte) 134,
      (byte) 70,
      (byte) 182,
      (byte) 118,
      (byte) 137,
      (byte) 73,
      (byte) 185,
      (byte) 121,
      (byte) 133,
      (byte) 69,
      (byte) 181,
      (byte) 117
    },
    new byte[16 /*0x10*/]
    {
      (byte) 42,
      (byte) 234,
      (byte) 26,
      (byte) 218,
      (byte) 38,
      (byte) 230,
      (byte) 22,
      (byte) 214,
      (byte) 41,
      (byte) 233,
      (byte) 25,
      (byte) 217,
      (byte) 37,
      (byte) 229,
      (byte) 21,
      (byte) 213
    },
    new byte[16 /*0x10*/]
    {
      (byte) 170,
      (byte) 106,
      (byte) 154,
      (byte) 90,
      (byte) 166,
      (byte) 102,
      (byte) 150,
      (byte) 86,
      (byte) 169,
      (byte) 105,
      (byte) 153,
      (byte) 89,
      (byte) 165,
      (byte) 101,
      (byte) 149,
      (byte) 85
    }
  };
  private Ex1PassCQuantizer.QuantizerType m_quantizer;
  private DecompressStruct m_cinfo;
  private byte[][] m_sv_colormap;
  private int m_sv_actual;
  private byte[][] m_colorindex;
  private int[] m_colorindexOffset;
  private bool m_is_padded;
  private int[] m_Ncolors = new int[4];
  private int m_row_index;
  private int[][][] m_odither = new int[4][][];
  private short[][] m_fserrors = new short[4][];
  private bool m_on_odd_row;

  public Ex1PassCQuantizer(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.m_fserrors[0] = (short[]) null;
    this.m_odither[0] = (int[][]) null;
    this.create_colormap();
    this.create_colorindex();
    if (cinfo.m_dither_mode != J_DITHER_MODE.JDITHER_FS)
      return;
    this.alloc_fs_workspace();
  }

  public virtual void start_pass(bool is_pre_scan)
  {
    this.m_cinfo.m_colormap = this.m_sv_colormap;
    this.m_cinfo.m_actual_number_of_colors = this.m_sv_actual;
    switch (this.m_cinfo.m_dither_mode)
    {
      case J_DITHER_MODE.JDITHER_NONE:
        if (this.m_cinfo.m_out_color_components == 3)
        {
          this.m_quantizer = Ex1PassCQuantizer.QuantizerType.color_quantizer3;
          break;
        }
        this.m_quantizer = Ex1PassCQuantizer.QuantizerType.color_quantizer;
        break;
      case J_DITHER_MODE.JDITHER_ORDERED:
        this.m_quantizer = this.m_cinfo.m_out_color_components != 3 ? Ex1PassCQuantizer.QuantizerType.quantize3_ord_dither_quantizer : Ex1PassCQuantizer.QuantizerType.quantize3_ord_dither_quantizer;
        this.m_row_index = 0;
        if (!this.m_is_padded)
          this.create_colorindex();
        if (this.m_odither[0] != null)
          break;
        this.create_odither_tables();
        break;
      case J_DITHER_MODE.JDITHER_FS:
        this.m_quantizer = Ex1PassCQuantizer.QuantizerType.quantize_fs_dither_quantizer;
        this.m_on_odd_row = false;
        if (this.m_fserrors[0] == null)
          this.alloc_fs_workspace();
        int length = this.m_cinfo.m_output_width + 2;
        for (int index = 0; index < this.m_cinfo.m_out_color_components; ++index)
          Array.Clear((Array) this.m_fserrors[index], 0, length);
        break;
    }
  }

  public virtual void color_quantize(
    byte[][] input_buf,
    int in_row,
    byte[][] output_buf,
    int out_row,
    int num_rows)
  {
    switch (this.m_quantizer)
    {
      case Ex1PassCQuantizer.QuantizerType.color_quantizer3:
        this.quantize3(input_buf, in_row, output_buf, out_row, num_rows);
        break;
      case Ex1PassCQuantizer.QuantizerType.color_quantizer:
        this.quantize(input_buf, in_row, output_buf, out_row, num_rows);
        break;
      case Ex1PassCQuantizer.QuantizerType.quantize3_ord_dither_quantizer:
        this.quantize3_ord_dither(input_buf, in_row, output_buf, out_row, num_rows);
        break;
      case Ex1PassCQuantizer.QuantizerType.quantize_ord_dither_quantizer:
        this.quantize_ord_dither(input_buf, in_row, output_buf, out_row, num_rows);
        break;
      case Ex1PassCQuantizer.QuantizerType.quantize_fs_dither_quantizer:
        this.quantize_fs_dither(input_buf, in_row, output_buf, out_row, num_rows);
        break;
    }
  }

  public virtual void finish_pass()
  {
  }

  public virtual void new_color_map()
  {
  }

  private void quantize(
    byte[][] input_buf,
    int in_row,
    byte[][] output_buf,
    int out_row,
    int num_rows)
  {
    int outColorComponents = this.m_cinfo.m_out_color_components;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int index2 = 0;
      int index3 = in_row + index1;
      int index4 = 0;
      int index5 = out_row + index1;
      for (int outputWidth = this.m_cinfo.m_output_width; outputWidth > 0; --outputWidth)
      {
        int num = 0;
        for (int index6 = 0; index6 < outColorComponents; ++index6)
        {
          num += (int) this.m_colorindex[index6][this.m_colorindexOffset[index6] + (int) input_buf[index3][index2]];
          ++index2;
        }
        output_buf[index5][index4] = (byte) num;
        ++index4;
      }
    }
  }

  private void quantize3(
    byte[][] input_buf,
    int in_row,
    byte[][] output_buf,
    int out_row,
    int num_rows)
  {
    int outputWidth = this.m_cinfo.m_output_width;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int index2 = 0;
      int index3 = in_row + index1;
      int index4 = 0;
      int index5 = out_row + index1;
      for (int index6 = outputWidth; index6 > 0; --index6)
      {
        int num1 = (int) this.m_colorindex[0][this.m_colorindexOffset[0] + (int) input_buf[index3][index2]];
        int index7 = index2 + 1;
        int num2 = num1 + (int) this.m_colorindex[1][this.m_colorindexOffset[1] + (int) input_buf[index3][index7]];
        int index8 = index7 + 1;
        int num3 = num2 + (int) this.m_colorindex[2][this.m_colorindexOffset[2] + (int) input_buf[index3][index8]];
        index2 = index8 + 1;
        output_buf[index5][index4] = (byte) num3;
        ++index4;
      }
    }
  }

  private void quantize_ord_dither(
    byte[][] input_buf,
    int in_row,
    byte[][] output_buf,
    int out_row,
    int num_rows)
  {
    int outColorComponents = this.m_cinfo.m_out_color_components;
    int outputWidth = this.m_cinfo.m_output_width;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      Array.Clear((Array) output_buf[out_row + index1], 0, outputWidth);
      int rowIndex = this.m_row_index;
      for (int index2 = 0; index2 < outColorComponents; ++index2)
      {
        int index3 = index2;
        int index4 = 0;
        int index5 = out_row + index1;
        int index6 = 0;
        for (int index7 = outputWidth; index7 > 0; --index7)
        {
          output_buf[index5][index4] += this.m_colorindex[index2][this.m_colorindexOffset[index2] + (int) input_buf[in_row + index1][index3] + this.m_odither[index2][rowIndex][index6]];
          index3 += outColorComponents;
          ++index4;
          index6 = index6 + 1 & 15;
        }
      }
      this.m_row_index = rowIndex + 1 & 15;
    }
  }

  private void quantize3_ord_dither(
    byte[][] input_buf,
    int in_row,
    byte[][] output_buf,
    int out_row,
    int num_rows)
  {
    int outputWidth = this.m_cinfo.m_output_width;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int rowIndex = this.m_row_index;
      int index2 = in_row + index1;
      int index3 = 0;
      int index4 = 0;
      int index5 = out_row + index1;
      int index6 = 0;
      for (int index7 = outputWidth; index7 > 0; --index7)
      {
        int num1 = (int) this.m_colorindex[0][this.m_colorindexOffset[0] + (int) input_buf[index2][index3] + this.m_odither[0][rowIndex][index6]];
        int index8 = index3 + 1;
        int num2 = num1 + (int) this.m_colorindex[1][this.m_colorindexOffset[1] + (int) input_buf[index2][index8] + this.m_odither[1][rowIndex][index6]];
        int index9 = index8 + 1;
        int num3 = num2 + (int) this.m_colorindex[2][this.m_colorindexOffset[2] + (int) input_buf[index2][index9] + this.m_odither[2][rowIndex][index6]];
        index3 = index9 + 1;
        output_buf[index5][index4] = (byte) num3;
        ++index4;
        index6 = index6 + 1 & 15;
      }
      this.m_row_index = rowIndex + 1 & 15;
    }
  }

  private void quantize_fs_dither(
    byte[][] input_buf,
    int in_row,
    byte[][] output_buf,
    int out_row,
    int num_rows)
  {
    int outColorComponents = this.m_cinfo.m_out_color_components;
    int outputWidth = this.m_cinfo.m_output_width;
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int rangeLimitOffset = this.m_cinfo.m_sampleRangeLimitOffset;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      Array.Clear((Array) output_buf[out_row + index1], 0, outputWidth);
      for (int index2 = 0; index2 < outColorComponents; ++index2)
      {
        int index3 = in_row + index1;
        int index4 = index2;
        int index5 = 0;
        int index6 = out_row + index1;
        int num1;
        int index7;
        if (this.m_on_odd_row)
        {
          index4 += (outputWidth - 1) * outColorComponents;
          index5 += outputWidth - 1;
          num1 = -1;
          index7 = outputWidth + 1;
        }
        else
        {
          num1 = 1;
          index7 = 0;
        }
        int num2 = num1 * outColorComponents;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        for (int index8 = outputWidth; index8 > 0; --index8)
        {
          int num6 = JpegUtils.RIGHT_SHIFT(num3 + (int) this.m_fserrors[index2][index7 + num1] + 8, 4) + (int) input_buf[index3][index4];
          int num7 = (int) sampleRangeLimit[rangeLimitOffset + num6];
          int index9 = (int) this.m_colorindex[index2][this.m_colorindexOffset[index2] + num7];
          output_buf[index6][index5] += (byte) index9;
          int num8 = num7 - (int) this.m_sv_colormap[index2][index9];
          int num9 = num8;
          int num10 = num8 * 2;
          int num11 = num8 + num10;
          this.m_fserrors[index2][index7] = (short) (num5 + num11);
          int num12 = num11 + num10;
          num5 = num4 + num12;
          num4 = num9;
          num3 = num12 + num10;
          index4 += num2;
          index5 += num1;
          index7 += num1;
        }
        this.m_fserrors[index2][index7] = (short) num5;
      }
      this.m_on_odd_row = !this.m_on_odd_row;
    }
  }

  private void create_colormap()
  {
    int samplesPerRow = this.select_ncolors(this.m_Ncolors);
    byte[][] numArray = CommonStruct.AllocJpegSamples(samplesPerRow, this.m_cinfo.m_out_color_components);
    int num1 = samplesPerRow;
    for (int index1 = 0; index1 < this.m_cinfo.m_out_color_components; ++index1)
    {
      int ncolor = this.m_Ncolors[index1];
      int num2 = num1 / ncolor;
      for (int j = 0; j < ncolor; ++j)
      {
        int num3 = Ex1PassCQuantizer.output_value(j, ncolor - 1);
        for (int index2 = j * num2; index2 < samplesPerRow; index2 += num1)
        {
          for (int index3 = 0; index3 < num2; ++index3)
            numArray[index1][index2 + index3] = (byte) num3;
        }
      }
      num1 = num2;
    }
    this.m_sv_colormap = numArray;
    this.m_sv_actual = samplesPerRow;
  }

  private void create_colorindex()
  {
    int num1;
    if (this.m_cinfo.m_dither_mode == J_DITHER_MODE.JDITHER_ORDERED)
    {
      num1 = 510;
      this.m_is_padded = true;
    }
    else
    {
      num1 = 0;
      this.m_is_padded = false;
    }
    this.m_colorindex = CommonStruct.AllocJpegSamples(256 /*0x0100*/ + num1, this.m_cinfo.m_out_color_components);
    this.m_colorindexOffset = new int[this.m_cinfo.m_out_color_components];
    int svActual = this.m_sv_actual;
    for (int index1 = 0; index1 < this.m_cinfo.m_out_color_components; ++index1)
    {
      int ncolor = this.m_Ncolors[index1];
      svActual /= ncolor;
      if (num1 != 0)
        this.m_colorindexOffset[index1] += (int) byte.MaxValue;
      int num2 = 0;
      int num3 = Ex1PassCQuantizer.largest_input_value(0, ncolor - 1);
      for (int index2 = 0; index2 <= (int) byte.MaxValue; ++index2)
      {
        while (index2 > num3)
          num3 = Ex1PassCQuantizer.largest_input_value(++num2, ncolor - 1);
        this.m_colorindex[index1][this.m_colorindexOffset[index1] + index2] = (byte) (num2 * svActual);
      }
      if (num1 != 0)
      {
        for (int index3 = 1; index3 <= (int) byte.MaxValue; ++index3)
        {
          this.m_colorindex[index1][this.m_colorindexOffset[index1] + -index3] = this.m_colorindex[index1][this.m_colorindexOffset[index1]];
          this.m_colorindex[index1][this.m_colorindexOffset[index1] + (int) byte.MaxValue + index3] = this.m_colorindex[index1][this.m_colorindexOffset[index1] + (int) byte.MaxValue];
        }
      }
    }
  }

  private void create_odither_tables()
  {
    for (int index1 = 0; index1 < this.m_cinfo.m_out_color_components; ++index1)
    {
      int ncolor = this.m_Ncolors[index1];
      int index2 = -1;
      for (int index3 = 0; index3 < index1; ++index3)
      {
        if (ncolor == this.m_Ncolors[index3])
        {
          index2 = index3;
          break;
        }
      }
      this.m_odither[index1] = index2 != -1 ? this.m_odither[index2] : Ex1PassCQuantizer.make_odither_array(ncolor);
    }
  }

  private void alloc_fs_workspace()
  {
    for (int index = 0; index < this.m_cinfo.m_out_color_components; ++index)
      this.m_fserrors[index] = new short[this.m_cinfo.m_output_width + 2];
  }

  private static int largest_input_value(int j, int maxj)
  {
    return ((2 * j + 1) * (int) byte.MaxValue + maxj) / (2 * maxj);
  }

  private static int output_value(int j, int maxj) => (j * (int) byte.MaxValue + maxj / 2) / maxj;

  private int select_ncolors(int[] Ncolors)
  {
    int outColorComponents = this.m_cinfo.m_out_color_components;
    int desiredNumberOfColors = this.m_cinfo.m_desired_number_of_colors;
    int num1 = 1;
    long num2;
    do
    {
      ++num1;
      num2 = (long) num1;
      for (int index = 1; index < outColorComponents; ++index)
        num2 *= (long) num1;
    }
    while (num2 <= (long) desiredNumberOfColors);
    int num3 = num1 - 1;
    int num4 = 1;
    for (int index = 0; index < outColorComponents; ++index)
    {
      Ncolors[index] = num3;
      num4 *= num3;
    }
    bool flag;
    do
    {
      flag = false;
      for (int index1 = 0; index1 < outColorComponents; ++index1)
      {
        int index2 = this.m_cinfo.m_out_color_space == J_COLOR_SPACE.JCS_RGB ? Ex1PassCQuantizer.RGB_order[index1] : index1;
        long num5 = (long) (num4 / Ncolors[index2]) * (long) (Ncolors[index2] + 1);
        if (num5 <= (long) desiredNumberOfColors)
        {
          ++Ncolors[index2];
          num4 = (int) num5;
          flag = true;
        }
        else
          break;
      }
    }
    while (flag);
    return num4;
  }

  private static int[][] make_odither_array(int ncolors)
  {
    int[][] numArray = new int[16 /*0x10*/][];
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray[index] = new int[16 /*0x10*/];
    int num1 = 512 /*0x0200*/ * (ncolors - 1);
    for (int index1 = 0; index1 < 16 /*0x10*/; ++index1)
    {
      for (int index2 = 0; index2 < 16 /*0x10*/; ++index2)
      {
        int num2 = ((int) byte.MaxValue - 2 * (int) Ex1PassCQuantizer.base_dither_matrix[index1][index2]) * (int) byte.MaxValue;
        numArray[index1][index2] = num2 < 0 ? -(-num2 / num1) : num2 / num1;
      }
    }
    return numArray;
  }

  private enum QuantizerType
  {
    color_quantizer3,
    color_quantizer,
    quantize3_ord_dither_quantizer,
    quantize_ord_dither_quantizer,
    quantize_fs_dither_quantizer,
  }
}
