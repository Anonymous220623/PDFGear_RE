// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.ExMergedUpsampler
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class ExMergedUpsampler : Upsampler
{
  private const int SCALEBITS = 16 /*0x10*/;
  private const int ONE_HALF = 32768 /*0x8000*/;
  private DecompressStruct m_cinfo;
  private bool m_use_2v_upsample;
  private int[] m_Cr_r_tab;
  private int[] m_Cb_b_tab;
  private int[] m_Cr_g_tab;
  private int[] m_Cb_g_tab;
  private byte[] m_spare_row;
  private bool m_spare_full;
  private int m_out_row_width;
  private int m_rows_to_go;

  public ExMergedUpsampler(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.m_need_context_rows = false;
    this.m_out_row_width = cinfo.m_output_width * cinfo.m_out_color_components;
    if (cinfo.m_max_v_samp_factor == 2)
    {
      this.m_use_2v_upsample = true;
      this.m_spare_row = new byte[this.m_out_row_width];
    }
    else
      this.m_use_2v_upsample = false;
    this.build_ycc_rgb_table();
  }

  public override void start_pass()
  {
    this.m_spare_full = false;
    this.m_rows_to_go = this.m_cinfo.m_output_height;
  }

  public override void upsample(
    ComponentBuffer[] input_buf,
    ref int in_row_group_ctr,
    int in_row_groups_avail,
    byte[][] output_buf,
    ref int out_row_ctr,
    int out_rows_avail)
  {
    if (this.m_use_2v_upsample)
      this.merged_2v_upsample(input_buf, ref in_row_group_ctr, output_buf, ref out_row_ctr, out_rows_avail);
    else
      this.merged_1v_upsample(input_buf, ref in_row_group_ctr, output_buf, ref out_row_ctr);
  }

  private void merged_1v_upsample(
    ComponentBuffer[] input_buf,
    ref int in_row_group_ctr,
    byte[][] output_buf,
    ref int out_row_ctr)
  {
    this.h2v1_merged_upsample(input_buf, in_row_group_ctr, output_buf, out_row_ctr);
    ++out_row_ctr;
    ++in_row_group_ctr;
  }

  private void merged_2v_upsample(
    ComponentBuffer[] input_buf,
    ref int in_row_group_ctr,
    byte[][] output_buf,
    ref int out_row_ctr,
    int out_rows_avail)
  {
    int num;
    if (this.m_spare_full)
    {
      JpegUtils.jcopy_sample_rows(new byte[1][]
      {
        this.m_spare_row
      }, 0, output_buf, out_row_ctr, 1, this.m_out_row_width);
      num = 1;
      this.m_spare_full = false;
    }
    else
    {
      num = 2;
      if (num > this.m_rows_to_go)
        num = this.m_rows_to_go;
      out_rows_avail -= out_row_ctr;
      if (num > out_rows_avail)
        num = out_rows_avail;
      byte[][] output_buf1 = new byte[2][]
      {
        output_buf[out_row_ctr],
        null
      };
      if (num > 1)
      {
        output_buf1[1] = output_buf[out_row_ctr + 1];
      }
      else
      {
        output_buf1[1] = this.m_spare_row;
        this.m_spare_full = true;
      }
      this.h2v2_merged_upsample(input_buf, in_row_group_ctr, output_buf1);
    }
    out_row_ctr += num;
    this.m_rows_to_go -= num;
    if (this.m_spare_full)
      return;
    ++in_row_group_ctr;
  }

  private void h2v1_merged_upsample(
    ComponentBuffer[] input_buf,
    int in_row_group_ctr,
    byte[][] output_buf,
    int outRow)
  {
    int index1 = 0;
    int index2 = 0;
    int index3 = 0;
    int index4 = 0;
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int rangeLimitOffset = this.m_cinfo.m_sampleRangeLimitOffset;
    for (int index5 = this.m_cinfo.m_output_width >> 1; index5 > 0; --index5)
    {
      int index6 = (int) input_buf[1][in_row_group_ctr][index2];
      ++index2;
      int index7 = (int) input_buf[2][in_row_group_ctr][index3];
      ++index3;
      int num1 = this.m_Cr_r_tab[index7];
      int num2 = JpegUtils.RIGHT_SHIFT(this.m_Cb_g_tab[index6] + this.m_Cr_g_tab[index7], 16 /*0x10*/);
      int num3 = this.m_Cb_b_tab[index6];
      int num4 = (int) input_buf[0][in_row_group_ctr][index1];
      int index8 = index1 + 1;
      output_buf[outRow][index4] = sampleRangeLimit[rangeLimitOffset + num4 + num1];
      output_buf[outRow][index4 + 1] = sampleRangeLimit[rangeLimitOffset + num4 + num2];
      output_buf[outRow][index4 + 2] = sampleRangeLimit[rangeLimitOffset + num4 + num3];
      int index9 = index4 + 3;
      int num5 = (int) input_buf[0][in_row_group_ctr][index8];
      index1 = index8 + 1;
      output_buf[outRow][index9] = sampleRangeLimit[rangeLimitOffset + num5 + num1];
      output_buf[outRow][index9 + 1] = sampleRangeLimit[rangeLimitOffset + num5 + num2];
      output_buf[outRow][index9 + 2] = sampleRangeLimit[rangeLimitOffset + num5 + num3];
      index4 = index9 + 3;
    }
    if ((this.m_cinfo.m_output_width & 1) == 0)
      return;
    int index10 = (int) input_buf[1][in_row_group_ctr][index2];
    int index11 = (int) input_buf[2][in_row_group_ctr][index3];
    int num6 = this.m_Cr_r_tab[index11];
    int num7 = JpegUtils.RIGHT_SHIFT(this.m_Cb_g_tab[index10] + this.m_Cr_g_tab[index11], 16 /*0x10*/);
    int num8 = this.m_Cb_b_tab[index10];
    int num9 = (int) input_buf[0][in_row_group_ctr][index1];
    output_buf[outRow][index4] = sampleRangeLimit[rangeLimitOffset + num9 + num6];
    output_buf[outRow][index4 + 1] = sampleRangeLimit[rangeLimitOffset + num9 + num7];
    output_buf[outRow][index4 + 2] = sampleRangeLimit[rangeLimitOffset + num9 + num8];
  }

  private void h2v2_merged_upsample(
    ComponentBuffer[] input_buf,
    int in_row_group_ctr,
    byte[][] output_buf)
  {
    int i1 = in_row_group_ctr * 2;
    int index1 = 0;
    int i2 = in_row_group_ctr * 2 + 1;
    int index2 = 0;
    int index3 = 0;
    int index4 = 0;
    int index5 = 0;
    int index6 = 0;
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int rangeLimitOffset = this.m_cinfo.m_sampleRangeLimitOffset;
    for (int index7 = this.m_cinfo.m_output_width >> 1; index7 > 0; --index7)
    {
      int index8 = (int) input_buf[1][in_row_group_ctr][index3];
      ++index3;
      int index9 = (int) input_buf[2][in_row_group_ctr][index4];
      ++index4;
      int num1 = this.m_Cr_r_tab[index9];
      int num2 = JpegUtils.RIGHT_SHIFT(this.m_Cb_g_tab[index8] + this.m_Cr_g_tab[index9], 16 /*0x10*/);
      int num3 = this.m_Cb_b_tab[index8];
      int num4 = (int) input_buf[0][i1][index1];
      int index10 = index1 + 1;
      output_buf[0][index5] = sampleRangeLimit[rangeLimitOffset + num4 + num1];
      output_buf[0][index5 + 1] = sampleRangeLimit[rangeLimitOffset + num4 + num2];
      output_buf[0][index5 + 2] = sampleRangeLimit[rangeLimitOffset + num4 + num3];
      int index11 = index5 + 3;
      int num5 = (int) input_buf[0][i1][index10];
      index1 = index10 + 1;
      output_buf[0][index11] = sampleRangeLimit[rangeLimitOffset + num5 + num1];
      output_buf[0][index11 + 1] = sampleRangeLimit[rangeLimitOffset + num5 + num2];
      output_buf[0][index11 + 2] = sampleRangeLimit[rangeLimitOffset + num5 + num3];
      index5 = index11 + 3;
      int num6 = (int) input_buf[0][i2][index2];
      int index12 = index2 + 1;
      output_buf[1][index6] = sampleRangeLimit[rangeLimitOffset + num6 + num1];
      output_buf[1][index6 + 1] = sampleRangeLimit[rangeLimitOffset + num6 + num2];
      output_buf[1][index6 + 2] = sampleRangeLimit[rangeLimitOffset + num6 + num3];
      int index13 = index6 + 3;
      int num7 = (int) input_buf[0][i2][index12];
      index2 = index12 + 1;
      output_buf[1][index13] = sampleRangeLimit[rangeLimitOffset + num7 + num1];
      output_buf[1][index13 + 1] = sampleRangeLimit[rangeLimitOffset + num7 + num2];
      output_buf[1][index13 + 2] = sampleRangeLimit[rangeLimitOffset + num7 + num3];
      index6 = index13 + 3;
    }
    if ((this.m_cinfo.m_output_width & 1) == 0)
      return;
    int index14 = (int) input_buf[1][in_row_group_ctr][index3];
    int index15 = (int) input_buf[2][in_row_group_ctr][index4];
    int num8 = this.m_Cr_r_tab[index15];
    int num9 = JpegUtils.RIGHT_SHIFT(this.m_Cb_g_tab[index14] + this.m_Cr_g_tab[index15], 16 /*0x10*/);
    int num10 = this.m_Cb_b_tab[index14];
    int num11 = (int) input_buf[0][i1][index1];
    output_buf[0][index5] = sampleRangeLimit[rangeLimitOffset + num11 + num8];
    output_buf[0][index5 + 1] = sampleRangeLimit[rangeLimitOffset + num11 + num9];
    output_buf[0][index5 + 2] = sampleRangeLimit[rangeLimitOffset + num11 + num10];
    int num12 = (int) input_buf[0][i2][index2];
    output_buf[1][index6] = sampleRangeLimit[rangeLimitOffset + num12 + num8];
    output_buf[1][index6 + 1] = sampleRangeLimit[rangeLimitOffset + num12 + num9];
    output_buf[1][index6 + 2] = sampleRangeLimit[rangeLimitOffset + num12 + num10];
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
      this.m_Cr_r_tab[index] = JpegUtils.RIGHT_SHIFT(ExMergedUpsampler.FIX(1.402) * minValue + 32768 /*0x8000*/, 16 /*0x10*/);
      this.m_Cb_b_tab[index] = JpegUtils.RIGHT_SHIFT(ExMergedUpsampler.FIX(1.772) * minValue + 32768 /*0x8000*/, 16 /*0x10*/);
      this.m_Cr_g_tab[index] = -ExMergedUpsampler.FIX(0.71414) * minValue;
      this.m_Cb_g_tab[index] = -ExMergedUpsampler.FIX(0.34414) * minValue + 32768 /*0x8000*/;
      ++index;
      ++minValue;
    }
  }

  private static int FIX(double x) => (int) (x * 65536.0 + 0.5);
}
