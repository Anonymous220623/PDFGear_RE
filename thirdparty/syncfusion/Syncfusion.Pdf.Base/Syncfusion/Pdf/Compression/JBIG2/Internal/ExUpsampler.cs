// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.ExUpsampler
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class ExUpsampler : Upsampler
{
  private DecompressStruct m_cinfo;
  private ComponentBuffer[] m_color_buf = new ComponentBuffer[10];
  private int[] m_perComponentOffsets = new int[10];
  private ExUpsampler.ComponentUpsampler[] m_upsampleMethods = new ExUpsampler.ComponentUpsampler[10];
  private int m_currentComponent;
  private int m_upsampleRowOffset;
  private int m_next_row_out;
  private int m_rows_to_go;
  private int[] m_rowgroup_height = new int[10];
  private byte[] m_h_expand = new byte[10];
  private byte[] m_v_expand = new byte[10];

  public ExUpsampler(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.m_need_context_rows = false;
    bool flag1 = cinfo.m_do_fancy_upsampling && cinfo.m_min_DCT_scaled_size > 1;
    for (int index = 0; index < cinfo.m_num_components; ++index)
    {
      ComponentInfo componentInfo = cinfo.Comp_info[index];
      int num1 = componentInfo.H_samp_factor * componentInfo.DCT_scaled_size / cinfo.m_min_DCT_scaled_size;
      int num2 = componentInfo.V_samp_factor * componentInfo.DCT_scaled_size / cinfo.m_min_DCT_scaled_size;
      int maxHSampFactor = cinfo.m_max_h_samp_factor;
      int maxVSampFactor = cinfo.m_max_v_samp_factor;
      this.m_rowgroup_height[index] = num2;
      bool flag2 = true;
      if (!componentInfo.component_needed)
      {
        this.m_upsampleMethods[index] = ExUpsampler.ComponentUpsampler.noop_upsampler;
        flag2 = false;
      }
      else if (num1 == maxHSampFactor && num2 == maxVSampFactor)
      {
        this.m_upsampleMethods[index] = ExUpsampler.ComponentUpsampler.fullsize_upsampler;
        flag2 = false;
      }
      else if (num1 * 2 == maxHSampFactor && num2 == maxVSampFactor)
        this.m_upsampleMethods[index] = !flag1 || componentInfo.downsampled_width <= 2 ? ExUpsampler.ComponentUpsampler.h2v1_upsampler : ExUpsampler.ComponentUpsampler.h2v1_fancy_upsampler;
      else if (num1 * 2 == maxHSampFactor && num2 * 2 == maxVSampFactor)
      {
        if (flag1 && componentInfo.downsampled_width > 2)
        {
          this.m_upsampleMethods[index] = ExUpsampler.ComponentUpsampler.h2v2_fancy_upsampler;
          this.m_need_context_rows = true;
        }
        else
          this.m_upsampleMethods[index] = ExUpsampler.ComponentUpsampler.h2v2_upsampler;
      }
      else if (maxHSampFactor % num1 == 0 && maxVSampFactor % num2 == 0)
      {
        this.m_upsampleMethods[index] = ExUpsampler.ComponentUpsampler.int_upsampler;
        this.m_h_expand[index] = (byte) (maxHSampFactor / num1);
        this.m_v_expand[index] = (byte) (maxVSampFactor / num2);
      }
      if (flag2)
      {
        ComponentBuffer componentBuffer = new ComponentBuffer();
        componentBuffer.SetBuffer(CommonStruct.AllocJpegSamples(JpegUtils.jround_up(cinfo.m_output_width, cinfo.m_max_h_samp_factor), cinfo.m_max_v_samp_factor), (int[]) null, 0);
        this.m_color_buf[index] = componentBuffer;
      }
    }
  }

  public override void start_pass()
  {
    this.m_next_row_out = this.m_cinfo.m_max_v_samp_factor;
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
    if (this.m_next_row_out >= this.m_cinfo.m_max_v_samp_factor)
    {
      for (int index = 0; index < this.m_cinfo.m_num_components; ++index)
      {
        this.m_perComponentOffsets[index] = 0;
        this.m_currentComponent = index;
        this.m_upsampleRowOffset = in_row_group_ctr * this.m_rowgroup_height[index];
        this.upsampleComponent(ref input_buf[index]);
      }
      this.m_next_row_out = 0;
    }
    int num_rows = this.m_cinfo.m_max_v_samp_factor - this.m_next_row_out;
    if (num_rows > this.m_rows_to_go)
      num_rows = this.m_rows_to_go;
    out_rows_avail -= out_row_ctr;
    if (num_rows > out_rows_avail)
      num_rows = out_rows_avail;
    this.m_cinfo.m_cconvert.color_convert(this.m_color_buf, this.m_perComponentOffsets, this.m_next_row_out, output_buf, out_row_ctr, num_rows);
    out_row_ctr += num_rows;
    this.m_rows_to_go -= num_rows;
    this.m_next_row_out += num_rows;
    if (this.m_next_row_out < this.m_cinfo.m_max_v_samp_factor)
      return;
    ++in_row_group_ctr;
  }

  private void upsampleComponent(ref ComponentBuffer input_data)
  {
    switch (this.m_upsampleMethods[this.m_currentComponent])
    {
      case ExUpsampler.ComponentUpsampler.noop_upsampler:
        ExUpsampler.noop_upsample();
        break;
      case ExUpsampler.ComponentUpsampler.fullsize_upsampler:
        this.fullsize_upsample(ref input_data);
        break;
      case ExUpsampler.ComponentUpsampler.h2v1_fancy_upsampler:
        this.h2v1_fancy_upsample(this.m_cinfo.Comp_info[this.m_currentComponent].downsampled_width, ref input_data);
        break;
      case ExUpsampler.ComponentUpsampler.h2v1_upsampler:
        this.h2v1_upsample(ref input_data);
        break;
      case ExUpsampler.ComponentUpsampler.h2v2_fancy_upsampler:
        this.h2v2_fancy_upsample(this.m_cinfo.Comp_info[this.m_currentComponent].downsampled_width, ref input_data);
        break;
      case ExUpsampler.ComponentUpsampler.h2v2_upsampler:
        this.h2v2_upsample(ref input_data);
        break;
      case ExUpsampler.ComponentUpsampler.int_upsampler:
        this.int_upsample(ref input_data);
        break;
    }
  }

  private static void noop_upsample()
  {
  }

  private void fullsize_upsample(ref ComponentBuffer input_data)
  {
    this.m_color_buf[this.m_currentComponent] = input_data;
    this.m_perComponentOffsets[this.m_currentComponent] = this.m_upsampleRowOffset;
  }

  private void h2v1_fancy_upsample(int downsampled_width, ref ComponentBuffer input_data)
  {
    ComponentBuffer componentBuffer = this.m_color_buf[this.m_currentComponent];
    for (int i1 = 0; i1 < this.m_cinfo.m_max_v_samp_factor; ++i1)
    {
      int i2 = this.m_upsampleRowOffset + i1;
      int index1 = 0;
      int index2 = 0;
      int num1 = (int) input_data[i2][index1];
      int index3 = index1 + 1;
      componentBuffer[i1][index2] = (byte) num1;
      int index4 = index2 + 1;
      componentBuffer[i1][index4] = (byte) (num1 * 3 + (int) input_data[i2][index3] + 2 >> 2);
      int index5 = index4 + 1;
      for (int index6 = downsampled_width - 2; index6 > 0; --index6)
      {
        int num2 = (int) input_data[i2][index3] * 3;
        ++index3;
        componentBuffer[i1][index5] = (byte) (num2 + (int) input_data[i2][index3 - 2] + 1 >> 2);
        int index7 = index5 + 1;
        componentBuffer[i1][index7] = (byte) (num2 + (int) input_data[i2][index3] + 2 >> 2);
        index5 = index7 + 1;
      }
      int num3 = (int) input_data[i2][index3];
      componentBuffer[i1][index5] = (byte) (num3 * 3 + (int) input_data[i2][index3 - 1] + 1 >> 2);
      int index8 = index5 + 1;
      componentBuffer[i1][index8] = (byte) num3;
      int num4 = index8 + 1;
    }
  }

  private void h2v1_upsample(ref ComponentBuffer input_data)
  {
    ComponentBuffer componentBuffer = this.m_color_buf[this.m_currentComponent];
    for (int i1 = 0; i1 < this.m_cinfo.m_max_v_samp_factor; ++i1)
    {
      int i2 = this.m_upsampleRowOffset + i1;
      int index1 = 0;
      int index2 = 0;
      while (index1 < this.m_cinfo.m_output_width)
      {
        byte num = input_data[i2][index2];
        componentBuffer[i1][index1] = num;
        int index3 = index1 + 1;
        componentBuffer[i1][index3] = num;
        index1 = index3 + 1;
        ++index2;
      }
    }
  }

  private void h2v2_fancy_upsample(int downsampled_width, ref ComponentBuffer input_data)
  {
    ComponentBuffer componentBuffer = this.m_color_buf[this.m_currentComponent];
    int upsampleRowOffset = this.m_upsampleRowOffset;
    int num1 = 0;
    while (num1 < this.m_cinfo.m_max_v_samp_factor)
    {
      for (int index1 = 0; index1 < 2; ++index1)
      {
        int index2 = 0;
        int index3 = 0;
        int i1 = index1 != 0 ? upsampleRowOffset + 1 : upsampleRowOffset - 1;
        int i2 = num1;
        int index4 = 0;
        ++num1;
        int num2 = (int) input_data[upsampleRowOffset][index2] * 3 + (int) input_data[i1][index3];
        int index5 = index2 + 1;
        int index6 = index3 + 1;
        int num3 = (int) input_data[upsampleRowOffset][index5] * 3 + (int) input_data[i1][index6];
        int index7 = index5 + 1;
        int index8 = index6 + 1;
        componentBuffer[i2][index4] = (byte) (num2 * 4 + 8 >> 4);
        int index9 = index4 + 1;
        componentBuffer[i2][index9] = (byte) (num2 * 3 + num3 + 7 >> 4);
        int index10 = index9 + 1;
        int num4 = num2;
        int num5 = num3;
        for (int index11 = downsampled_width - 2; index11 > 0; --index11)
        {
          int num6 = (int) input_data[upsampleRowOffset][index7] * 3 + (int) input_data[i1][index8];
          ++index7;
          ++index8;
          componentBuffer[i2][index10] = (byte) (num5 * 3 + num4 + 8 >> 4);
          int index12 = index10 + 1;
          componentBuffer[i2][index12] = (byte) (num5 * 3 + num6 + 7 >> 4);
          index10 = index12 + 1;
          num4 = num5;
          num5 = num6;
        }
        componentBuffer[i2][index10] = (byte) (num5 * 3 + num4 + 8 >> 4);
        int index13 = index10 + 1;
        componentBuffer[i2][index13] = (byte) (num5 * 4 + 7 >> 4);
        int num7 = index13 + 1;
      }
      ++upsampleRowOffset;
    }
  }

  private void h2v2_upsample(ref ComponentBuffer input_data)
  {
    ComponentBuffer componentBuffer = this.m_color_buf[this.m_currentComponent];
    int num1 = 0;
    for (int index1 = 0; index1 < this.m_cinfo.m_max_v_samp_factor; index1 += 2)
    {
      int i = this.m_upsampleRowOffset + num1;
      int index2 = 0;
      int index3 = 0;
      while (index2 < this.m_cinfo.m_output_width)
      {
        byte num2 = input_data[i][index3];
        componentBuffer[index1][index2] = num2;
        int index4 = index2 + 1;
        componentBuffer[index1][index4] = num2;
        index2 = index4 + 1;
        ++index3;
      }
      JpegUtils.jcopy_sample_rows(componentBuffer, index1, componentBuffer, index1 + 1, 1, this.m_cinfo.m_output_width);
      ++num1;
    }
  }

  private void int_upsample(ref ComponentBuffer input_data)
  {
    ComponentBuffer componentBuffer = this.m_color_buf[this.m_currentComponent];
    int num1 = (int) this.m_h_expand[this.m_currentComponent];
    int num2 = (int) this.m_v_expand[this.m_currentComponent];
    int num3 = 0;
    for (int index1 = 0; index1 < this.m_cinfo.m_max_v_samp_factor; index1 += num2)
    {
      int i = this.m_upsampleRowOffset + num3;
      for (int index2 = 0; index2 < this.m_cinfo.m_output_width; ++index2)
      {
        byte num4 = input_data[i][index2];
        int index3 = 0;
        for (int index4 = num1; index4 > 0; --index4)
        {
          componentBuffer[index1][index3] = num4;
          ++index3;
        }
      }
      if (num2 > 1)
        JpegUtils.jcopy_sample_rows(componentBuffer, index1, componentBuffer, index1 + 1, num2 - 1, this.m_cinfo.m_output_width);
      ++num3;
    }
  }

  private enum ComponentUpsampler
  {
    noop_upsampler,
    fullsize_upsampler,
    h2v1_fancy_upsampler,
    h2v1_upsampler,
    h2v2_fancy_upsampler,
    h2v2_upsampler,
    int_upsampler,
  }
}
