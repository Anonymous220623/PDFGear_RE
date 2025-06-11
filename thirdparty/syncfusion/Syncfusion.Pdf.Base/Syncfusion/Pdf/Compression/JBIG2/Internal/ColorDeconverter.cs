// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.ColorDeconverter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class ColorDeconverter
{
  private const int SCALEBITS = 16 /*0x10*/;
  private const int ONE_HALF = 32768 /*0x8000*/;
  private ColorDeconverter.ColorConverter m_converter;
  private DecompressStruct m_cinfo;
  private int[] m_perComponentOffsets;
  private int[] m_Cr_r_tab;
  private int[] m_Cb_b_tab;
  private int[] m_Cr_g_tab;
  private int[] m_Cb_g_tab;

  public ColorDeconverter(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    switch (cinfo.m_jpeg_color_space)
    {
      default:
        switch (cinfo.m_out_color_space)
        {
          case J_COLOR_SPACE.JCS_GRAYSCALE:
            cinfo.m_out_color_components = 1;
            if (cinfo.m_jpeg_color_space == J_COLOR_SPACE.JCS_GRAYSCALE || cinfo.m_jpeg_color_space == J_COLOR_SPACE.JCS_YCbCr)
            {
              this.m_converter = ColorDeconverter.ColorConverter.grayscale_converter;
              for (int index = 1; index < cinfo.m_num_components; ++index)
                cinfo.Comp_info[index].component_needed = false;
              break;
            }
            break;
          case J_COLOR_SPACE.JCS_RGB:
            cinfo.m_out_color_components = 3;
            if (cinfo.m_jpeg_color_space == J_COLOR_SPACE.JCS_YCbCr)
            {
              this.m_converter = ColorDeconverter.ColorConverter.ycc_rgb_converter;
              this.build_ycc_rgb_table();
              break;
            }
            if (cinfo.m_jpeg_color_space == J_COLOR_SPACE.JCS_GRAYSCALE)
            {
              this.m_converter = ColorDeconverter.ColorConverter.gray_rgb_converter;
              break;
            }
            if (cinfo.m_jpeg_color_space == J_COLOR_SPACE.JCS_RGB)
            {
              this.m_converter = ColorDeconverter.ColorConverter.null_converter;
              break;
            }
            break;
          case J_COLOR_SPACE.JCS_CMYK:
            cinfo.m_out_color_components = 4;
            if (cinfo.m_jpeg_color_space == J_COLOR_SPACE.JCS_YCCK)
            {
              this.m_converter = ColorDeconverter.ColorConverter.ycck_cmyk_converter;
              this.build_ycc_rgb_table();
              break;
            }
            if (cinfo.m_jpeg_color_space == J_COLOR_SPACE.JCS_CMYK)
            {
              this.m_converter = ColorDeconverter.ColorConverter.null_converter;
              break;
            }
            break;
          default:
            if (cinfo.m_out_color_space == cinfo.m_jpeg_color_space)
            {
              cinfo.m_out_color_components = cinfo.m_num_components;
              this.m_converter = ColorDeconverter.ColorConverter.null_converter;
              break;
            }
            break;
        }
        if (cinfo.m_quantize_colors)
        {
          cinfo.m_output_components = 1;
          break;
        }
        cinfo.m_output_components = cinfo.m_out_color_components;
        break;
    }
  }

  public void color_convert(
    ComponentBuffer[] input_buf,
    int[] perComponentOffsets,
    int input_row,
    byte[][] output_buf,
    int output_row,
    int num_rows)
  {
    this.m_perComponentOffsets = perComponentOffsets;
    switch (this.m_converter)
    {
      case ColorDeconverter.ColorConverter.grayscale_converter:
        this.grayscale_convert(input_buf, input_row, output_buf, output_row, num_rows);
        break;
      case ColorDeconverter.ColorConverter.ycc_rgb_converter:
        this.ycc_rgb_convert(input_buf, input_row, output_buf, output_row, num_rows);
        break;
      case ColorDeconverter.ColorConverter.gray_rgb_converter:
        this.gray_rgb_convert(input_buf, input_row, output_buf, output_row, num_rows);
        break;
      case ColorDeconverter.ColorConverter.null_converter:
        this.null_convert(input_buf, input_row, output_buf, output_row, num_rows);
        break;
      case ColorDeconverter.ColorConverter.ycck_cmyk_converter:
        this.ycck_cmyk_convert(input_buf, input_row, output_buf, output_row, num_rows);
        break;
    }
  }

  private void build_ycc_rgb_table()
  {
    this.m_Cr_r_tab = new int[256 /*0x0100*/];
    this.m_Cb_b_tab = new int[256 /*0x0100*/];
    this.m_Cr_g_tab = new int[256 /*0x0100*/];
    this.m_Cb_g_tab = new int[256 /*0x0100*/];
    int index = 0;
    int minValue = (int) sbyte.MinValue;
    while (index <= (int) byte.MaxValue)
    {
      this.m_Cr_r_tab[index] = JpegUtils.RIGHT_SHIFT(ColorDeconverter.FIX(1.402) * minValue + 32768 /*0x8000*/, 16 /*0x10*/);
      this.m_Cb_b_tab[index] = JpegUtils.RIGHT_SHIFT(ColorDeconverter.FIX(1.772) * minValue + 32768 /*0x8000*/, 16 /*0x10*/);
      this.m_Cr_g_tab[index] = -ColorDeconverter.FIX(0.71414) * minValue;
      this.m_Cb_g_tab[index] = -ColorDeconverter.FIX(0.34414) * minValue + 32768 /*0x8000*/;
      ++index;
      ++minValue;
    }
  }

  private void ycc_rgb_convert(
    ComponentBuffer[] input_buf,
    int input_row,
    byte[][] output_buf,
    int output_row,
    int num_rows)
  {
    int perComponentOffset1 = this.m_perComponentOffsets[0];
    int perComponentOffset2 = this.m_perComponentOffsets[1];
    int perComponentOffset3 = this.m_perComponentOffsets[2];
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int rangeLimitOffset = this.m_cinfo.m_sampleRangeLimitOffset;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int index2 = 0;
      for (int index3 = 0; index3 < this.m_cinfo.m_output_width; ++index3)
      {
        int num = (int) input_buf[0][input_row + perComponentOffset1][index3];
        int index4 = (int) input_buf[1][input_row + perComponentOffset2][index3];
        int index5 = (int) input_buf[2][input_row + perComponentOffset3][index3];
        output_buf[output_row + index1][index2] = sampleRangeLimit[rangeLimitOffset + num + this.m_Cr_r_tab[index5]];
        output_buf[output_row + index1][index2 + 1] = sampleRangeLimit[rangeLimitOffset + num + JpegUtils.RIGHT_SHIFT(this.m_Cb_g_tab[index4] + this.m_Cr_g_tab[index5], 16 /*0x10*/)];
        output_buf[output_row + index1][index2 + 2] = sampleRangeLimit[rangeLimitOffset + num + this.m_Cb_b_tab[index4]];
        index2 += 3;
      }
      ++input_row;
    }
  }

  private void ycck_cmyk_convert(
    ComponentBuffer[] input_buf,
    int input_row,
    byte[][] output_buf,
    int output_row,
    int num_rows)
  {
    int perComponentOffset1 = this.m_perComponentOffsets[0];
    int perComponentOffset2 = this.m_perComponentOffsets[1];
    int perComponentOffset3 = this.m_perComponentOffsets[2];
    int perComponentOffset4 = this.m_perComponentOffsets[3];
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int rangeLimitOffset = this.m_cinfo.m_sampleRangeLimitOffset;
    int outputWidth = this.m_cinfo.m_output_width;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int index2 = 0;
      for (int index3 = 0; index3 < outputWidth; ++index3)
      {
        int num = (int) input_buf[0][input_row + perComponentOffset1][index3];
        int index4 = (int) input_buf[1][input_row + perComponentOffset2][index3];
        int index5 = (int) input_buf[2][input_row + perComponentOffset3][index3];
        output_buf[output_row + index1][index2] = sampleRangeLimit[rangeLimitOffset + (int) byte.MaxValue - (num + this.m_Cr_r_tab[index5])];
        output_buf[output_row + index1][index2 + 1] = sampleRangeLimit[rangeLimitOffset + (int) byte.MaxValue - (num + JpegUtils.RIGHT_SHIFT(this.m_Cb_g_tab[index4] + this.m_Cr_g_tab[index5], 16 /*0x10*/))];
        output_buf[output_row + index1][index2 + 2] = sampleRangeLimit[rangeLimitOffset + (int) byte.MaxValue - (num + this.m_Cb_b_tab[index4])];
        output_buf[output_row + index1][index2 + 3] = input_buf[3][input_row + perComponentOffset4][index3];
        index2 += 4;
      }
      ++input_row;
    }
  }

  private void gray_rgb_convert(
    ComponentBuffer[] input_buf,
    int input_row,
    byte[][] output_buf,
    int output_row,
    int num_rows)
  {
    int perComponentOffset1 = this.m_perComponentOffsets[0];
    int perComponentOffset2 = this.m_perComponentOffsets[1];
    int perComponentOffset3 = this.m_perComponentOffsets[2];
    int outputWidth = this.m_cinfo.m_output_width;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int index2 = 0;
      for (int index3 = 0; index3 < outputWidth; ++index3)
      {
        output_buf[output_row + index1][index2] = input_buf[0][input_row + perComponentOffset1][index3];
        output_buf[output_row + index1][index2 + 1] = input_buf[0][input_row + perComponentOffset2][index3];
        output_buf[output_row + index1][index2 + 2] = input_buf[0][input_row + perComponentOffset3][index3];
        index2 += 3;
      }
      ++input_row;
    }
  }

  private void grayscale_convert(
    ComponentBuffer[] input_buf,
    int input_row,
    byte[][] output_buf,
    int output_row,
    int num_rows)
  {
    JpegUtils.jcopy_sample_rows(input_buf[0], input_row + this.m_perComponentOffsets[0], output_buf, output_row, num_rows, this.m_cinfo.m_output_width);
  }

  private void null_convert(
    ComponentBuffer[] input_buf,
    int input_row,
    byte[][] output_buf,
    int output_row,
    int num_rows)
  {
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      for (int index2 = 0; index2 < this.m_cinfo.m_num_components; ++index2)
      {
        int index3 = 0;
        int num = 0;
        int perComponentOffset = this.m_perComponentOffsets[index2];
        for (int outputWidth = this.m_cinfo.m_output_width; outputWidth > 0; --outputWidth)
        {
          output_buf[output_row + index1][index2 + num] = input_buf[index2][input_row + perComponentOffset][index3];
          num += this.m_cinfo.m_num_components;
          ++index3;
        }
      }
      ++input_row;
    }
  }

  private static int FIX(double x) => (int) (x * 65536.0 + 0.5);

  private enum ColorConverter
  {
    grayscale_converter,
    ycc_rgb_converter,
    gray_rgb_converter,
    null_converter,
    ycck_cmyk_converter,
  }
}
