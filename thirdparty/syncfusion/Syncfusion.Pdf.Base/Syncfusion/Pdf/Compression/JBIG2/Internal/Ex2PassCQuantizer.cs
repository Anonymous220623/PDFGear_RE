// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.Ex2PassCQuantizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class Ex2PassCQuantizer : ColorQuantizer
{
  private const int MAXNUMCOLORS = 256 /*0x0100*/;
  private const int HIST_C0_BITS = 5;
  private const int HIST_C1_BITS = 6;
  private const int HIST_C2_BITS = 5;
  private const int HIST_C0_ELEMS = 32 /*0x20*/;
  private const int HIST_C1_ELEMS = 64 /*0x40*/;
  private const int HIST_C2_ELEMS = 32 /*0x20*/;
  private const int C0_SHIFT = 3;
  private const int C1_SHIFT = 2;
  private const int C2_SHIFT = 3;
  private const int R_SCALE = 2;
  private const int G_SCALE = 3;
  private const int B_SCALE = 1;
  private const int BOX_C0_LOG = 2;
  private const int BOX_C1_LOG = 3;
  private const int BOX_C2_LOG = 2;
  private const int BOX_C0_ELEMS = 4;
  private const int BOX_C1_ELEMS = 8;
  private const int BOX_C2_ELEMS = 4;
  private const int BOX_C0_SHIFT = 5;
  private const int BOX_C1_SHIFT = 5;
  private const int BOX_C2_SHIFT = 5;
  private Ex2PassCQuantizer.QuantizerType m_quantizer;
  private bool m_useFinishPass1;
  private DecompressStruct m_cinfo;
  private byte[][] m_sv_colormap;
  private int m_desired;
  private ushort[][] m_histogram;
  private bool m_needs_zeroed;
  private short[] m_fserrors;
  private bool m_on_odd_row;
  private int[] m_error_limiter;

  public Ex2PassCQuantizer(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.m_histogram = new ushort[32 /*0x20*/][];
    for (int index = 0; index < 32 /*0x20*/; ++index)
      this.m_histogram[index] = new ushort[2048 /*0x0800*/];
    this.m_needs_zeroed = true;
    if (cinfo.m_enable_2pass_quant)
    {
      int desiredNumberOfColors = cinfo.m_desired_number_of_colors;
      this.m_sv_colormap = CommonStruct.AllocJpegSamples(desiredNumberOfColors, 3);
      this.m_desired = desiredNumberOfColors;
    }
    if (cinfo.m_dither_mode != J_DITHER_MODE.JDITHER_NONE)
      cinfo.m_dither_mode = J_DITHER_MODE.JDITHER_FS;
    if (cinfo.m_dither_mode != J_DITHER_MODE.JDITHER_FS)
      return;
    this.m_fserrors = new short[(cinfo.m_output_width + 2) * 3];
    this.init_error_limit();
  }

  public virtual void start_pass(bool is_pre_scan)
  {
    if (this.m_cinfo.m_dither_mode != J_DITHER_MODE.JDITHER_NONE)
      this.m_cinfo.m_dither_mode = J_DITHER_MODE.JDITHER_FS;
    if (is_pre_scan)
    {
      this.m_quantizer = Ex2PassCQuantizer.QuantizerType.prescan_quantizer;
      this.m_useFinishPass1 = true;
      this.m_needs_zeroed = true;
    }
    else
    {
      this.m_quantizer = this.m_cinfo.m_dither_mode != J_DITHER_MODE.JDITHER_FS ? Ex2PassCQuantizer.QuantizerType.pass2_no_dither_quantizer : Ex2PassCQuantizer.QuantizerType.pass2_fs_dither_quantizer;
      this.m_useFinishPass1 = false;
      int actualNumberOfColors = this.m_cinfo.m_actual_number_of_colors;
      if (this.m_cinfo.m_dither_mode == J_DITHER_MODE.JDITHER_FS)
      {
        if (this.m_fserrors == null)
          this.m_fserrors = new short[(this.m_cinfo.m_output_width + 2) * 3];
        else
          Array.Clear((Array) this.m_fserrors, 0, this.m_fserrors.Length);
        if (this.m_error_limiter == null)
          this.init_error_limit();
        this.m_on_odd_row = false;
      }
    }
    if (!this.m_needs_zeroed)
      return;
    for (int index = 0; index < 32 /*0x20*/; ++index)
      Array.Clear((Array) this.m_histogram[index], 0, this.m_histogram[index].Length);
    this.m_needs_zeroed = false;
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
      case Ex2PassCQuantizer.QuantizerType.prescan_quantizer:
        this.prescan_quantize(input_buf, in_row, num_rows);
        break;
      case Ex2PassCQuantizer.QuantizerType.pass2_fs_dither_quantizer:
        this.pass2_fs_dither(input_buf, in_row, output_buf, out_row, num_rows);
        break;
      case Ex2PassCQuantizer.QuantizerType.pass2_no_dither_quantizer:
        this.pass2_no_dither(input_buf, in_row, output_buf, out_row, num_rows);
        break;
    }
  }

  public virtual void finish_pass()
  {
    if (!this.m_useFinishPass1)
      return;
    this.finish_pass1();
  }

  public virtual void new_color_map() => this.m_needs_zeroed = true;

  private void prescan_quantize(byte[][] input_buf, int in_row, int num_rows)
  {
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int index2 = 0;
      for (int outputWidth = this.m_cinfo.m_output_width; outputWidth > 0; --outputWidth)
      {
        int index3 = (int) input_buf[in_row + index1][index2] >> 3;
        int index4 = ((int) input_buf[in_row + index1][index2 + 1] >> 2) * 32 /*0x20*/ + ((int) input_buf[in_row + index1][index2 + 2] >> 3);
        ++this.m_histogram[index3][index4];
        if (this.m_histogram[index3][index4] <= (ushort) 0)
          --this.m_histogram[index3][index4];
        index2 += 3;
      }
    }
  }

  private void pass2_fs_dither(
    byte[][] input_buf,
    int in_row,
    byte[][] output_buf,
    int out_row,
    int num_rows)
  {
    byte[] sampleRangeLimit = this.m_cinfo.m_sample_range_limit;
    int rangeLimitOffset = this.m_cinfo.m_sampleRangeLimitOffset;
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int index2 = 0;
      int index3 = 0;
      int num1;
      int num2;
      int index4;
      if (this.m_on_odd_row)
      {
        index2 += (this.m_cinfo.m_output_width - 1) * 3;
        index3 += this.m_cinfo.m_output_width - 1;
        num1 = -1;
        num2 = -3;
        index4 = (this.m_cinfo.m_output_width + 1) * 3;
        this.m_on_odd_row = false;
      }
      else
      {
        num1 = 1;
        num2 = 3;
        index4 = 0;
        this.m_on_odd_row = true;
      }
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      int num6 = 0;
      int num7 = 0;
      int num8 = 0;
      int num9 = 0;
      int num10 = 0;
      int num11 = 0;
      for (int outputWidth = this.m_cinfo.m_output_width; outputWidth > 0; --outputWidth)
      {
        int num12 = JpegUtils.RIGHT_SHIFT(num3 + (int) this.m_fserrors[index4 + num2] + 8, 4);
        int num13 = JpegUtils.RIGHT_SHIFT(num4 + (int) this.m_fserrors[index4 + num2 + 1] + 8, 4);
        int num14 = JpegUtils.RIGHT_SHIFT(num5 + (int) this.m_fserrors[index4 + num2 + 2] + 8, 4);
        int num15 = this.m_error_limiter[(int) byte.MaxValue + num12];
        int num16 = this.m_error_limiter[(int) byte.MaxValue + num13];
        int num17 = this.m_error_limiter[(int) byte.MaxValue + num14];
        int num18 = num15 + (int) input_buf[in_row + index1][index2];
        int num19 = num16 + (int) input_buf[in_row + index1][index2 + 1];
        int num20 = num17 + (int) input_buf[in_row + index1][index2 + 2];
        int num21 = (int) sampleRangeLimit[rangeLimitOffset + num18];
        int num22 = (int) sampleRangeLimit[rangeLimitOffset + num19];
        int num23 = (int) sampleRangeLimit[rangeLimitOffset + num20];
        int index5 = num21 >> 3;
        int index6 = (num22 >> 2) * 32 /*0x20*/ + (num23 >> 3);
        if (this.m_histogram[index5][index6] == (ushort) 0)
          this.fill_inverse_cmap(num21 >> 3, num22 >> 2, num23 >> 3);
        int index7 = (int) this.m_histogram[index5][index6] - 1;
        output_buf[out_row + index1][index3] = (byte) index7;
        int num24 = num21 - (int) this.m_cinfo.m_colormap[0][index7];
        int num25 = num22 - (int) this.m_cinfo.m_colormap[1][index7];
        int num26 = num23 - (int) this.m_cinfo.m_colormap[2][index7];
        int num27 = num24;
        int num28 = num24 * 2;
        int num29 = num24 + num28;
        this.m_fserrors[index4] = (short) (num9 + num29);
        int num30 = num29 + num28;
        num9 = num6 + num30;
        num6 = num27;
        num3 = num30 + num28;
        int num31 = num25;
        int num32 = num25 * 2;
        int num33 = num25 + num32;
        this.m_fserrors[index4 + 1] = (short) (num10 + num33);
        int num34 = num33 + num32;
        num10 = num7 + num34;
        num7 = num31;
        num4 = num34 + num32;
        int num35 = num26;
        int num36 = num26 * 2;
        int num37 = num26 + num36;
        this.m_fserrors[index4 + 2] = (short) (num11 + num37);
        int num38 = num37 + num36;
        num11 = num8 + num38;
        num8 = num35;
        num5 = num38 + num36;
        index2 += num2;
        index3 += num1;
        index4 += num2;
      }
      this.m_fserrors[index4] = (short) num9;
      this.m_fserrors[index4 + 1] = (short) num10;
      this.m_fserrors[index4 + 2] = (short) num11;
    }
  }

  private void pass2_no_dither(
    byte[][] input_buf,
    int in_row,
    byte[][] output_buf,
    int out_row,
    int num_rows)
  {
    for (int index1 = 0; index1 < num_rows; ++index1)
    {
      int index2 = index1 + in_row;
      int index3 = 0;
      int index4 = 0;
      int index5 = out_row + index1;
      for (int outputWidth = this.m_cinfo.m_output_width; outputWidth > 0; --outputWidth)
      {
        int c0 = (int) input_buf[index2][index3] >> 3;
        int index6 = index3 + 1;
        int c1 = (int) input_buf[index2][index6] >> 2;
        int index7 = index6 + 1;
        int c2 = (int) input_buf[index2][index7] >> 3;
        index3 = index7 + 1;
        int index8 = c0;
        int index9 = c1 * 32 /*0x20*/ + c2;
        if (this.m_histogram[index8][index9] == (ushort) 0)
          this.fill_inverse_cmap(c0, c1, c2);
        output_buf[index5][index4] = (byte) ((uint) this.m_histogram[index8][index9] - 1U);
        ++index4;
      }
    }
  }

  private void finish_pass1()
  {
    this.m_cinfo.m_colormap = this.m_sv_colormap;
    this.select_colors(this.m_desired);
    this.m_needs_zeroed = true;
  }

  private void compute_color(Ex2PassCQuantizer.box[] boxlist, int boxIndex, int icolor)
  {
    long num1 = 0;
    long num2 = 0;
    long num3 = 0;
    long num4 = 0;
    Ex2PassCQuantizer.box box = boxlist[boxIndex];
    for (int c0min = box.c0min; c0min <= box.c0max; ++c0min)
    {
      for (int c1min = box.c1min; c1min <= box.c1max; ++c1min)
      {
        int index = c1min * 32 /*0x20*/ + box.c2min;
        for (int c2min = box.c2min; c2min <= box.c2max; ++c2min)
        {
          long num5 = (long) this.m_histogram[c0min][index];
          ++index;
          if (num5 != 0L)
          {
            num1 += num5;
            num2 += (long) ((c0min << 3) + 4) * num5;
            num3 += (long) ((c1min << 2) + 2) * num5;
            num4 += (long) ((c2min << 3) + 4) * num5;
          }
        }
      }
    }
    this.m_cinfo.m_colormap[0][icolor] = (byte) ((num2 + (num1 >> 1)) / num1);
    this.m_cinfo.m_colormap[1][icolor] = (byte) ((num3 + (num1 >> 1)) / num1);
    this.m_cinfo.m_colormap[2][icolor] = (byte) ((num4 + (num1 >> 1)) / num1);
  }

  private void select_colors(int desired_colors)
  {
    Ex2PassCQuantizer.box[] boxlist = new Ex2PassCQuantizer.box[desired_colors];
    int numboxes = 1;
    boxlist[0].c0min = 0;
    boxlist[0].c0max = 31 /*0x1F*/;
    boxlist[0].c1min = 0;
    boxlist[0].c1max = 63 /*0x3F*/;
    boxlist[0].c2min = 0;
    boxlist[0].c2max = 31 /*0x1F*/;
    this.update_box(boxlist, 0);
    int num = this.median_cut(boxlist, numboxes, desired_colors);
    for (int index = 0; index < num; ++index)
      this.compute_color(boxlist, index, index);
    this.m_cinfo.m_actual_number_of_colors = num;
  }

  private int median_cut(Ex2PassCQuantizer.box[] boxlist, int numboxes, int desired_colors)
  {
    for (; numboxes < desired_colors; ++numboxes)
    {
      int boxIndex = numboxes * 2 > desired_colors ? Ex2PassCQuantizer.find_biggest_volume(boxlist, numboxes) : Ex2PassCQuantizer.find_biggest_color_pop(boxlist, numboxes);
      if (boxIndex != -1)
      {
        boxlist[numboxes].c0max = boxlist[boxIndex].c0max;
        boxlist[numboxes].c1max = boxlist[boxIndex].c1max;
        boxlist[numboxes].c2max = boxlist[boxIndex].c2max;
        boxlist[numboxes].c0min = boxlist[boxIndex].c0min;
        boxlist[numboxes].c1min = boxlist[boxIndex].c1min;
        boxlist[numboxes].c2min = boxlist[boxIndex].c2min;
        int num1 = (boxlist[boxIndex].c0max - boxlist[boxIndex].c0min << 3) * 2;
        int num2 = (boxlist[boxIndex].c1max - boxlist[boxIndex].c1min << 2) * 3;
        int num3 = boxlist[boxIndex].c2max - boxlist[boxIndex].c2min << 3;
        int num4 = num2;
        int num5 = 1;
        if (num1 > num4)
        {
          num4 = num1;
          num5 = 0;
        }
        if (num3 > num4)
          num5 = 2;
        switch (num5)
        {
          case 0:
            int num6 = (boxlist[boxIndex].c0max + boxlist[boxIndex].c0min) / 2;
            boxlist[boxIndex].c0max = num6;
            boxlist[numboxes].c0min = num6 + 1;
            break;
          case 1:
            int num7 = (boxlist[boxIndex].c1max + boxlist[boxIndex].c1min) / 2;
            boxlist[boxIndex].c1max = num7;
            boxlist[numboxes].c1min = num7 + 1;
            break;
          case 2:
            int num8 = (boxlist[boxIndex].c2max + boxlist[boxIndex].c2min) / 2;
            boxlist[boxIndex].c2max = num8;
            boxlist[numboxes].c2min = num8 + 1;
            break;
        }
        this.update_box(boxlist, boxIndex);
        this.update_box(boxlist, numboxes);
      }
      else
        break;
    }
    return numboxes;
  }

  private static int find_biggest_color_pop(Ex2PassCQuantizer.box[] boxlist, int numboxes)
  {
    long num = 0;
    int biggestColorPop = -1;
    for (int index = 0; index < numboxes; ++index)
    {
      if (boxlist[index].colorcount > num && boxlist[index].volume > 0)
      {
        biggestColorPop = index;
        num = boxlist[index].colorcount;
      }
    }
    return biggestColorPop;
  }

  private static int find_biggest_volume(Ex2PassCQuantizer.box[] boxlist, int numboxes)
  {
    int num = 0;
    int biggestVolume = -1;
    for (int index = 0; index < numboxes; ++index)
    {
      if (boxlist[index].volume > num)
      {
        biggestVolume = index;
        num = boxlist[index].volume;
      }
    }
    return biggestVolume;
  }

  private void update_box(Ex2PassCQuantizer.box[] boxlist, int boxIndex)
  {
    Ex2PassCQuantizer.box box = boxlist[boxIndex];
    bool flag1 = false;
    if (box.c0max > box.c0min)
    {
      for (int c0min = box.c0min; c0min <= box.c0max; ++c0min)
      {
        for (int c1min = box.c1min; c1min <= box.c1max; ++c1min)
        {
          int num = c1min * 32 /*0x20*/ + box.c2min;
          for (int c2min = box.c2min; c2min <= box.c2max; ++c2min)
          {
            if (this.m_histogram[c0min][num++] != (ushort) 0)
            {
              box.c0min = c0min;
              flag1 = true;
              break;
            }
          }
          if (flag1)
            break;
        }
        if (flag1)
          break;
      }
    }
    bool flag2 = false;
    if (box.c0max > box.c0min)
    {
      for (int c0max = box.c0max; c0max >= box.c0min; --c0max)
      {
        for (int c1min = box.c1min; c1min <= box.c1max; ++c1min)
        {
          int num = c1min * 32 /*0x20*/ + box.c2min;
          for (int c2min = box.c2min; c2min <= box.c2max; ++c2min)
          {
            if (this.m_histogram[c0max][num++] != (ushort) 0)
            {
              box.c0max = c0max;
              flag2 = true;
              break;
            }
          }
          if (flag2)
            break;
        }
        if (flag2)
          break;
      }
    }
    bool flag3 = false;
    if (box.c1max > box.c1min)
    {
      for (int c1min = box.c1min; c1min <= box.c1max; ++c1min)
      {
        for (int c0min = box.c0min; c0min <= box.c0max; ++c0min)
        {
          int num = c1min * 32 /*0x20*/ + box.c2min;
          for (int c2min = box.c2min; c2min <= box.c2max; ++c2min)
          {
            if (this.m_histogram[c0min][num++] != (ushort) 0)
            {
              box.c1min = c1min;
              flag3 = true;
              break;
            }
          }
          if (flag3)
            break;
        }
        if (flag3)
          break;
      }
    }
    bool flag4 = false;
    if (box.c1max > box.c1min)
    {
      for (int c1max = box.c1max; c1max >= box.c1min; --c1max)
      {
        for (int c0min = box.c0min; c0min <= box.c0max; ++c0min)
        {
          int num = c1max * 32 /*0x20*/ + box.c2min;
          for (int c2min = box.c2min; c2min <= box.c2max; ++c2min)
          {
            if (this.m_histogram[c0min][num++] != (ushort) 0)
            {
              box.c1max = c1max;
              flag4 = true;
              break;
            }
          }
          if (flag4)
            break;
        }
        if (flag4)
          break;
      }
    }
    bool flag5 = false;
    if (box.c2max > box.c2min)
    {
      for (int c2min = box.c2min; c2min <= box.c2max; ++c2min)
      {
        for (int c0min = box.c0min; c0min <= box.c0max; ++c0min)
        {
          int index = box.c1min * 32 /*0x20*/ + c2min;
          int c1min = box.c1min;
          while (c1min <= box.c1max)
          {
            if (this.m_histogram[c0min][index] != (ushort) 0)
            {
              box.c2min = c2min;
              flag5 = true;
              break;
            }
            ++c1min;
            index += 32 /*0x20*/;
          }
          if (flag5)
            break;
        }
        if (flag5)
          break;
      }
    }
    bool flag6 = false;
    if (box.c2max > box.c2min)
    {
      for (int c2max = box.c2max; c2max >= box.c2min; --c2max)
      {
        for (int c0min = box.c0min; c0min <= box.c0max; ++c0min)
        {
          int index = box.c1min * 32 /*0x20*/ + c2max;
          int c1min = box.c1min;
          while (c1min <= box.c1max)
          {
            if (this.m_histogram[c0min][index] != (ushort) 0)
            {
              box.c2max = c2max;
              flag6 = true;
              break;
            }
            ++c1min;
            index += 32 /*0x20*/;
          }
          if (flag6)
            break;
        }
        if (flag6)
          break;
      }
    }
    int num1 = (box.c0max - box.c0min << 3) * 2;
    int num2 = (box.c1max - box.c1min << 2) * 3;
    int num3 = box.c2max - box.c2min << 3;
    box.volume = num1 * num1 + num2 * num2 + num3 * num3;
    long num4 = 0;
    for (int c0min = box.c0min; c0min <= box.c0max; ++c0min)
    {
      for (int c1min = box.c1min; c1min <= box.c1max; ++c1min)
      {
        int index = c1min * 32 /*0x20*/ + box.c2min;
        int c2min = box.c2min;
        while (c2min <= box.c2max)
        {
          if (this.m_histogram[c0min][index] != (ushort) 0)
            ++num4;
          ++c2min;
          ++index;
        }
      }
    }
    box.colorcount = num4;
    boxlist[boxIndex] = box;
  }

  private void init_error_limit()
  {
    this.m_error_limiter = new int[511 /*0x01FF*/];
    int maxValue = (int) byte.MaxValue;
    int num1 = 0;
    int num2 = 0;
    while (num2 < 16 /*0x10*/)
    {
      this.m_error_limiter[maxValue + num2] = num1;
      this.m_error_limiter[maxValue - num2] = -num1;
      ++num2;
      ++num1;
    }
    for (; num2 < 48 /*0x30*/; ++num2)
    {
      this.m_error_limiter[maxValue + num2] = num1;
      this.m_error_limiter[maxValue - num2] = -num1;
      num1 += (num2 & 1) != 0 ? 1 : 0;
    }
    for (; num2 <= (int) byte.MaxValue; ++num2)
    {
      this.m_error_limiter[maxValue + num2] = num1;
      this.m_error_limiter[maxValue - num2] = -num1;
    }
  }

  private int find_nearby_colors(int minc0, int minc1, int minc2, byte[] colorlist)
  {
    int num1 = minc0 + 24;
    int num2 = minc0 + num1 >> 1;
    int num3 = minc1 + 28;
    int num4 = minc1 + num3 >> 1;
    int num5 = minc2 + 24;
    int num6 = minc2 + num5 >> 1;
    int num7 = int.MaxValue;
    int[] numArray = new int[256 /*0x0100*/];
    for (int index = 0; index < this.m_cinfo.m_actual_number_of_colors; ++index)
    {
      int num8 = (int) this.m_cinfo.m_colormap[0][index];
      int num9;
      int num10;
      if (num8 < minc0)
      {
        int num11 = (num8 - minc0) * 2;
        num9 = num11 * num11;
        int num12 = (num8 - num1) * 2;
        num10 = num12 * num12;
      }
      else if (num8 > num1)
      {
        int num13 = (num8 - num1) * 2;
        num9 = num13 * num13;
        int num14 = (num8 - minc0) * 2;
        num10 = num14 * num14;
      }
      else
      {
        num9 = 0;
        if (num8 <= num2)
        {
          int num15 = (num8 - num1) * 2;
          num10 = num15 * num15;
        }
        else
        {
          int num16 = (num8 - minc0) * 2;
          num10 = num16 * num16;
        }
      }
      int num17 = (int) this.m_cinfo.m_colormap[1][index];
      int num18;
      if (num17 < minc1)
      {
        int num19 = (num17 - minc1) * 3;
        num9 += num19 * num19;
        int num20 = (num17 - num3) * 3;
        num18 = num10 + num20 * num20;
      }
      else if (num17 > num3)
      {
        int num21 = (num17 - num3) * 3;
        num9 += num21 * num21;
        int num22 = (num17 - minc1) * 3;
        num18 = num10 + num22 * num22;
      }
      else if (num17 <= num4)
      {
        int num23 = (num17 - num3) * 3;
        num18 = num10 + num23 * num23;
      }
      else
      {
        int num24 = (num17 - minc1) * 3;
        num18 = num10 + num24 * num24;
      }
      int num25 = (int) this.m_cinfo.m_colormap[2][index];
      int num26;
      if (num25 < minc2)
      {
        int num27 = num25 - minc2;
        num9 += num27 * num27;
        int num28 = num25 - num5;
        num26 = num18 + num28 * num28;
      }
      else if (num25 > num5)
      {
        int num29 = num25 - num5;
        num9 += num29 * num29;
        int num30 = num25 - minc2;
        num26 = num18 + num30 * num30;
      }
      else if (num25 <= num6)
      {
        int num31 = num25 - num5;
        num26 = num18 + num31 * num31;
      }
      else
      {
        int num32 = num25 - minc2;
        num26 = num18 + num32 * num32;
      }
      numArray[index] = num9;
      if (num26 < num7)
        num7 = num26;
    }
    int nearbyColors = 0;
    for (int index = 0; index < this.m_cinfo.m_actual_number_of_colors; ++index)
    {
      if (numArray[index] <= num7)
        colorlist[nearbyColors++] = (byte) index;
    }
    return nearbyColors;
  }

  private void find_best_colors(
    int minc0,
    int minc1,
    int minc2,
    int numcolors,
    byte[] colorlist,
    byte[] bestcolor)
  {
    int[] numArray = new int[128 /*0x80*/];
    int index1 = 0;
    for (int maxValue = (int) sbyte.MaxValue; maxValue >= 0; --maxValue)
    {
      numArray[index1] = int.MaxValue;
      ++index1;
    }
    for (int index2 = 0; index2 < numcolors; ++index2)
    {
      int index3 = (int) colorlist[index2];
      int num1 = (minc0 - (int) this.m_cinfo.m_colormap[0][index3]) * 2;
      int num2 = num1 * num1;
      int num3 = (minc1 - (int) this.m_cinfo.m_colormap[1][index3]) * 3;
      int num4 = num2 + num3 * num3;
      int num5 = minc2 - (int) this.m_cinfo.m_colormap[2][index3];
      int num6 = num4 + num5 * num5;
      int num7 = num1 * 32 /*0x20*/ + 256 /*0x0100*/;
      int num8 = num3 * 24 + 144 /*0x90*/;
      int num9 = num5 * 16 /*0x10*/ + 64 /*0x40*/;
      int index4 = 0;
      int index5 = 0;
      int num10 = num7;
      for (int index6 = 3; index6 >= 0; --index6)
      {
        int num11 = num6;
        int num12 = num8;
        for (int index7 = 7; index7 >= 0; --index7)
        {
          int num13 = num11;
          int num14 = num9;
          for (int index8 = 3; index8 >= 0; --index8)
          {
            if (num13 < numArray[index4])
            {
              numArray[index4] = num13;
              bestcolor[index5] = (byte) index3;
            }
            num13 += num14;
            num14 += 128 /*0x80*/;
            ++index4;
            ++index5;
          }
          num11 += num12;
          num12 += 288;
        }
        num6 += num10;
        num10 += 512 /*0x0200*/;
      }
    }
  }

  private void fill_inverse_cmap(int c0, int c1, int c2)
  {
    c0 >>= 2;
    c1 >>= 3;
    c2 >>= 2;
    int minc0 = (c0 << 5) + 4;
    int minc1 = (c1 << 5) + 2;
    int minc2 = (c2 << 5) + 4;
    byte[] colorlist = new byte[256 /*0x0100*/];
    int nearbyColors = this.find_nearby_colors(minc0, minc1, minc2, colorlist);
    byte[] bestcolor = new byte[128 /*0x80*/];
    this.find_best_colors(minc0, minc1, minc2, nearbyColors, colorlist, bestcolor);
    c0 <<= 2;
    c1 <<= 3;
    c2 <<= 2;
    int index1 = 0;
    for (int index2 = 0; index2 < 4; ++index2)
    {
      for (int index3 = 0; index3 < 8; ++index3)
      {
        int index4 = (c1 + index3) * 32 /*0x20*/ + c2;
        for (int index5 = 0; index5 < 4; ++index5)
        {
          this.m_histogram[c0 + index2][index4] = (ushort) ((uint) bestcolor[index1] + 1U);
          ++index4;
          ++index1;
        }
      }
    }
  }

  private struct box
  {
    public int c0min;
    public int c0max;
    public int c1min;
    public int c1max;
    public int c2min;
    public int c2max;
    public int volume;
    public long colorcount;
  }

  private enum QuantizerType
  {
    prescan_quantizer,
    pass2_fs_dither_quantizer,
    pass2_no_dither_quantizer,
  }
}
