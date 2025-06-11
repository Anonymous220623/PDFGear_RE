// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.DCoefController
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class DCoefController
{
  private const int SAVED_COEFS = 6;
  private const int Q01_POS = 1;
  private const int Q10_POS = 8;
  private const int Q20_POS = 16 /*0x10*/;
  private const int Q11_POS = 9;
  private const int Q02_POS = 2;
  private DecompressStruct m_cinfo;
  private bool m_useDummyConsumeData;
  private DCoefController.DecompressorType m_decompressor;
  private int m_MCU_ctr;
  private int m_MCU_vert_offset;
  private int m_MCU_rows_per_iMCU_row;
  private JBLOCK[] m_MCU_buffer = new JBLOCK[10];
  private jvirt_array<JBLOCK>[] m_whole_image = new jvirt_array<JBLOCK>[10];
  private jvirt_array<JBLOCK>[] m_coef_arrays;
  private int[] m_coef_bits_latch;
  private int m_coef_bits_savedOffset;

  public DCoefController(DecompressStruct cinfo, bool need_full_buffer)
  {
    this.m_cinfo = cinfo;
    if (need_full_buffer)
    {
      for (int index = 0; index < cinfo.m_num_components; ++index)
      {
        this.m_whole_image[index] = CommonStruct.CreateBlocksArray(JpegUtils.jround_up(cinfo.Comp_info[index].Width_in_blocks, cinfo.Comp_info[index].H_samp_factor), JpegUtils.jround_up(cinfo.Comp_info[index].height_in_blocks, cinfo.Comp_info[index].V_samp_factor));
        this.m_whole_image[index].ErrorProcessor = (CommonStruct) cinfo;
      }
      this.m_useDummyConsumeData = false;
      this.m_decompressor = DCoefController.DecompressorType.Ordinary;
      this.m_coef_arrays = this.m_whole_image;
    }
    else
    {
      JBLOCK[] jblockArray = new JBLOCK[10];
      for (int index1 = 0; index1 < 10; ++index1)
      {
        jblockArray[index1] = new JBLOCK();
        for (int index2 = 0; index2 < jblockArray[index1].data.Length; ++index2)
          jblockArray[index1].data[index2] = (short) -12851;
        this.m_MCU_buffer[index1] = jblockArray[index1];
      }
      this.m_useDummyConsumeData = true;
      this.m_decompressor = DCoefController.DecompressorType.OnePass;
      this.m_coef_arrays = (jvirt_array<JBLOCK>[]) null;
    }
  }

  public void start_input_pass()
  {
    this.m_cinfo.m_input_iMCU_row = 0;
    this.start_iMCU_row();
  }

  public ReadResult consume_data()
  {
    if (this.m_useDummyConsumeData)
      return ReadResult.JPEG_SUSPENDED;
    JBLOCK[][][] jblockArray = new JBLOCK[4][][];
    for (int index = 0; index < this.m_cinfo.m_comps_in_scan; ++index)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index]];
      jblockArray[index] = this.m_whole_image[componentInfo.Component_index].Access(this.m_cinfo.m_input_iMCU_row * componentInfo.V_samp_factor, componentInfo.V_samp_factor);
    }
    for (int mcuVertOffset = this.m_MCU_vert_offset; mcuVertOffset < this.m_MCU_rows_per_iMCU_row; ++mcuVertOffset)
    {
      for (int mcuCtr = this.m_MCU_ctr; mcuCtr < this.m_cinfo.m_MCUs_per_row; ++mcuCtr)
      {
        int index1 = 0;
        for (int index2 = 0; index2 < this.m_cinfo.m_comps_in_scan; ++index2)
        {
          ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index2]];
          int num = mcuCtr * componentInfo.MCU_width;
          for (int index3 = 0; index3 < componentInfo.MCU_height; ++index3)
          {
            for (int index4 = 0; index4 < componentInfo.MCU_width; ++index4)
            {
              this.m_MCU_buffer[index1] = jblockArray[index2][index3 + mcuVertOffset][num + index4];
              ++index1;
            }
          }
        }
        if (!this.m_cinfo.m_entropy.decode_mcu(this.m_MCU_buffer))
        {
          this.m_MCU_vert_offset = mcuVertOffset;
          this.m_MCU_ctr = mcuCtr;
          return ReadResult.JPEG_SUSPENDED;
        }
      }
      this.m_MCU_ctr = 0;
    }
    ++this.m_cinfo.m_input_iMCU_row;
    if (this.m_cinfo.m_input_iMCU_row < this.m_cinfo.m_total_iMCU_rows)
    {
      this.start_iMCU_row();
      return ReadResult.JPEG_ROW_COMPLETED;
    }
    this.m_cinfo.m_inputctl.finish_input_pass();
    return ReadResult.JPEG_SCAN_COMPLETED;
  }

  public void start_output_pass()
  {
    if (this.m_coef_arrays != null)
      this.m_decompressor = !this.m_cinfo.m_do_block_smoothing || !this.smoothing_ok() ? DCoefController.DecompressorType.Ordinary : DCoefController.DecompressorType.Smooth;
    this.m_cinfo.m_output_iMCU_row = 0;
  }

  public ReadResult decompress_data(ComponentBuffer[] output_buf)
  {
    switch (this.m_decompressor)
    {
      case DCoefController.DecompressorType.Ordinary:
        return this.decompress_data_ordinary(output_buf);
      case DCoefController.DecompressorType.Smooth:
        return this.decompress_smooth_data(output_buf);
      case DCoefController.DecompressorType.OnePass:
        return this.decompress_onepass(output_buf);
      default:
        return ReadResult.JPEG_SUSPENDED;
    }
  }

  public jvirt_array<JBLOCK>[] GetCoefArrays() => this.m_coef_arrays;

  private ReadResult decompress_onepass(ComponentBuffer[] output_buf)
  {
    int num1 = this.m_cinfo.m_MCUs_per_row - 1;
    int num2 = this.m_cinfo.m_total_iMCU_rows - 1;
    for (int mcuVertOffset = this.m_MCU_vert_offset; mcuVertOffset < this.m_MCU_rows_per_iMCU_row; ++mcuVertOffset)
    {
      for (int mcuCtr = this.m_MCU_ctr; mcuCtr <= num1; ++mcuCtr)
      {
        for (int index = 0; index < this.m_cinfo.m_blocks_in_MCU; ++index)
          Array.Clear((Array) this.m_MCU_buffer[index].data, 0, this.m_MCU_buffer[index].data.Length);
        if (!this.m_cinfo.m_entropy.decode_mcu(this.m_MCU_buffer))
        {
          this.m_MCU_vert_offset = mcuVertOffset;
          this.m_MCU_ctr = mcuCtr;
          return ReadResult.JPEG_SUSPENDED;
        }
        int num3 = 0;
        for (int index1 = 0; index1 < this.m_cinfo.m_comps_in_scan; ++index1)
        {
          ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index1]];
          if (!componentInfo.component_needed)
          {
            num3 += componentInfo.MCU_blocks;
          }
          else
          {
            int num4 = mcuCtr < num1 ? componentInfo.MCU_width : componentInfo.last_col_width;
            int output_row = mcuVertOffset * componentInfo.DCT_scaled_size;
            int num5 = mcuCtr * componentInfo.MCU_sample_width;
            for (int index2 = 0; index2 < componentInfo.MCU_height; ++index2)
            {
              if (this.m_cinfo.m_input_iMCU_row < num2 || mcuVertOffset + index2 < componentInfo.last_row_height)
              {
                int output_col = num5;
                for (int index3 = 0; index3 < num4; ++index3)
                {
                  this.m_cinfo.m_idct.inverse(componentInfo.Component_index, this.m_MCU_buffer[num3 + index3].data, output_buf[componentInfo.Component_index], output_row, output_col);
                  output_col += componentInfo.DCT_scaled_size;
                }
              }
              num3 += componentInfo.MCU_width;
              output_row += componentInfo.DCT_scaled_size;
            }
          }
        }
      }
      this.m_MCU_ctr = 0;
    }
    ++this.m_cinfo.m_output_iMCU_row;
    ++this.m_cinfo.m_input_iMCU_row;
    if (this.m_cinfo.m_input_iMCU_row < this.m_cinfo.m_total_iMCU_rows)
    {
      this.start_iMCU_row();
      return ReadResult.JPEG_ROW_COMPLETED;
    }
    this.m_cinfo.m_inputctl.finish_input_pass();
    return ReadResult.JPEG_SCAN_COMPLETED;
  }

  private ReadResult decompress_data_ordinary(ComponentBuffer[] output_buf)
  {
    while (this.m_cinfo.m_input_scan_number < this.m_cinfo.m_output_scan_number || this.m_cinfo.m_input_scan_number == this.m_cinfo.m_output_scan_number && this.m_cinfo.m_input_iMCU_row <= this.m_cinfo.m_output_iMCU_row)
    {
      if (this.m_cinfo.m_inputctl.consume_input() == ReadResult.JPEG_SUSPENDED)
        return ReadResult.JPEG_SUSPENDED;
    }
    int num1 = this.m_cinfo.m_total_iMCU_rows - 1;
    for (int index1 = 0; index1 < this.m_cinfo.m_num_components; ++index1)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[index1];
      if (componentInfo.component_needed)
      {
        JBLOCK[][] jblockArray = this.m_whole_image[index1].Access(this.m_cinfo.m_output_iMCU_row * componentInfo.V_samp_factor, componentInfo.V_samp_factor);
        int num2;
        if (this.m_cinfo.m_output_iMCU_row < num1)
        {
          num2 = componentInfo.V_samp_factor;
        }
        else
        {
          num2 = componentInfo.height_in_blocks % componentInfo.V_samp_factor;
          if (num2 == 0)
            num2 = componentInfo.V_samp_factor;
        }
        int output_row = 0;
        for (int index2 = 0; index2 < num2; ++index2)
        {
          int output_col = 0;
          for (int index3 = 0; index3 < componentInfo.Width_in_blocks; ++index3)
          {
            this.m_cinfo.m_idct.inverse(componentInfo.Component_index, jblockArray[index2][index3].data, output_buf[index1], output_row, output_col);
            output_col += componentInfo.DCT_scaled_size;
          }
          output_row += componentInfo.DCT_scaled_size;
        }
      }
    }
    ++this.m_cinfo.m_output_iMCU_row;
    return this.m_cinfo.m_output_iMCU_row < this.m_cinfo.m_total_iMCU_rows ? ReadResult.JPEG_ROW_COMPLETED : ReadResult.JPEG_SCAN_COMPLETED;
  }

  private ReadResult decompress_smooth_data(ComponentBuffer[] output_buf)
  {
    while (this.m_cinfo.m_input_scan_number <= this.m_cinfo.m_output_scan_number && !this.m_cinfo.m_inputctl.EOIReached() && (this.m_cinfo.m_input_scan_number != this.m_cinfo.m_output_scan_number || this.m_cinfo.m_input_iMCU_row <= this.m_cinfo.m_output_iMCU_row + (this.m_cinfo.m_Ss == 0 ? 1 : 0)))
    {
      if (this.m_cinfo.m_inputctl.consume_input() == ReadResult.JPEG_SUSPENDED)
        return ReadResult.JPEG_SUSPENDED;
    }
    int num1 = this.m_cinfo.m_total_iMCU_rows - 1;
    for (int index1 = 0; index1 < this.m_cinfo.m_num_components; ++index1)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[index1];
      if (componentInfo.component_needed)
      {
        int num2;
        int numberOfRows1;
        bool flag1;
        if (this.m_cinfo.m_output_iMCU_row < num1)
        {
          num2 = componentInfo.V_samp_factor;
          numberOfRows1 = num2 * 2;
          flag1 = false;
        }
        else
        {
          num2 = componentInfo.height_in_blocks % componentInfo.V_samp_factor;
          if (num2 == 0)
            num2 = componentInfo.V_samp_factor;
          numberOfRows1 = num2;
          flag1 = true;
        }
        int num3 = 0;
        JBLOCK[][] jblockArray;
        bool flag2;
        if (this.m_cinfo.m_output_iMCU_row > 0)
        {
          int numberOfRows2 = numberOfRows1 + componentInfo.V_samp_factor;
          jblockArray = this.m_whole_image[index1].Access((this.m_cinfo.m_output_iMCU_row - 1) * componentInfo.V_samp_factor, numberOfRows2);
          num3 = componentInfo.V_samp_factor;
          flag2 = false;
        }
        else
        {
          jblockArray = this.m_whole_image[index1].Access(0, numberOfRows1);
          flag2 = true;
        }
        int num4 = index1 * 6;
        int num5 = (int) componentInfo.quant_table.quantval[0];
        int num6 = (int) componentInfo.quant_table.quantval[1];
        int num7 = (int) componentInfo.quant_table.quantval[8];
        int num8 = (int) componentInfo.quant_table.quantval[16 /*0x10*/];
        int num9 = (int) componentInfo.quant_table.quantval[9];
        int num10 = (int) componentInfo.quant_table.quantval[2];
        int index2 = index1;
        for (int index3 = 0; index3 < num2; ++index3)
        {
          int index4 = num3 + index3;
          int index5 = !flag2 || index3 != 0 ? index4 - 1 : index4;
          int index6 = !flag1 || index3 != num2 - 1 ? index4 + 1 : index4;
          int num11 = (int) jblockArray[index5][0][0];
          int num12 = num11;
          int num13 = num11;
          int num14 = (int) jblockArray[index4][0][0];
          int num15 = num14;
          int num16 = num14;
          int num17 = (int) jblockArray[index6][0][0];
          int num18 = num17;
          int num19 = num17;
          int output_col = 0;
          int num20 = componentInfo.Width_in_blocks - 1;
          for (int index7 = 0; index7 <= num20; ++index7)
          {
            JBLOCK jblock = new JBLOCK();
            Buffer.BlockCopy((Array) jblockArray[index4][0].data, 0, (Array) jblock.data, 0, jblock.data.Length * 2);
            if (index7 < num20)
            {
              num13 = (int) jblockArray[index5][1][0];
              num16 = (int) jblockArray[index4][1][0];
              num19 = (int) jblockArray[index6][1][0];
            }
            int num21 = this.m_coef_bits_latch[this.m_coef_bits_savedOffset + num4 + 1];
            if (num21 != 0 && jblock[1] == (short) 0)
            {
              int num22 = 36 * num5 * (num14 - num16);
              int num23;
              if (num22 >= 0)
              {
                num23 = ((num6 << 7) + num22) / (num6 << 8);
                if (num21 > 0 && num23 >= 1 << num21)
                  num23 = (1 << num21) - 1;
              }
              else
              {
                int num24 = ((num6 << 7) - num22) / (num6 << 8);
                if (num21 > 0 && num24 >= 1 << num21)
                  num24 = (1 << num21) - 1;
                num23 = -num24;
              }
              jblock[1] = (short) num23;
            }
            int num25 = this.m_coef_bits_latch[this.m_coef_bits_savedOffset + num4 + 2];
            if (num25 != 0 && jblock[8] == (short) 0)
            {
              int num26 = 36 * num5 * (num12 - num18);
              int num27;
              if (num26 >= 0)
              {
                num27 = ((num7 << 7) + num26) / (num7 << 8);
                if (num25 > 0 && num27 >= 1 << num25)
                  num27 = (1 << num25) - 1;
              }
              else
              {
                int num28 = ((num7 << 7) - num26) / (num7 << 8);
                if (num25 > 0 && num28 >= 1 << num25)
                  num28 = (1 << num25) - 1;
                num27 = -num28;
              }
              jblock[8] = (short) num27;
            }
            int num29 = this.m_coef_bits_latch[this.m_coef_bits_savedOffset + num4 + 3];
            if (num29 != 0 && jblock[16 /*0x10*/] == (short) 0)
            {
              int num30 = 9 * num5 * (num12 + num18 - 2 * num15);
              int num31;
              if (num30 >= 0)
              {
                num31 = ((num8 << 7) + num30) / (num8 << 8);
                if (num29 > 0 && num31 >= 1 << num29)
                  num31 = (1 << num29) - 1;
              }
              else
              {
                int num32 = ((num8 << 7) - num30) / (num8 << 8);
                if (num29 > 0 && num32 >= 1 << num29)
                  num32 = (1 << num29) - 1;
                num31 = -num32;
              }
              jblock[16 /*0x10*/] = (short) num31;
            }
            int num33 = this.m_coef_bits_latch[this.m_coef_bits_savedOffset + num4 + 4];
            if (num33 != 0 && jblock[9] == (short) 0)
            {
              int num34 = 5 * num5 * (num11 - num13 - num17 + num19);
              int num35;
              if (num34 >= 0)
              {
                num35 = ((num9 << 7) + num34) / (num9 << 8);
                if (num33 > 0 && num35 >= 1 << num33)
                  num35 = (1 << num33) - 1;
              }
              else
              {
                int num36 = ((num9 << 7) - num34) / (num9 << 8);
                if (num33 > 0 && num36 >= 1 << num33)
                  num36 = (1 << num33) - 1;
                num35 = -num36;
              }
              jblock[9] = (short) num35;
            }
            int num37 = this.m_coef_bits_latch[this.m_coef_bits_savedOffset + num4 + 5];
            if (num37 != 0 && jblock[2] == (short) 0)
            {
              int num38 = 9 * num5 * (num14 + num16 - 2 * num15);
              int num39;
              if (num38 >= 0)
              {
                num39 = ((num10 << 7) + num38) / (num10 << 8);
                if (num37 > 0 && num39 >= 1 << num37)
                  num39 = (1 << num37) - 1;
              }
              else
              {
                int num40 = ((num10 << 7) - num38) / (num10 << 8);
                if (num37 > 0 && num40 >= 1 << num37)
                  num40 = (1 << num37) - 1;
                num39 = -num40;
              }
              jblock[2] = (short) num39;
            }
            this.m_cinfo.m_idct.inverse(componentInfo.Component_index, jblock.data, output_buf[index2], 0, output_col);
            num11 = num12;
            num12 = num13;
            num14 = num15;
            num15 = num16;
            num17 = num18;
            num18 = num19;
            ++index4;
            ++index5;
            ++index6;
            output_col += componentInfo.DCT_scaled_size;
          }
          index2 += componentInfo.DCT_scaled_size;
        }
      }
    }
    ++this.m_cinfo.m_output_iMCU_row;
    return this.m_cinfo.m_output_iMCU_row < this.m_cinfo.m_total_iMCU_rows ? ReadResult.JPEG_ROW_COMPLETED : ReadResult.JPEG_SCAN_COMPLETED;
  }

  private bool smoothing_ok()
  {
    if (!this.m_cinfo.m_progressive_mode || this.m_cinfo.m_coef_bits == null)
      return false;
    if (this.m_coef_bits_latch == null)
    {
      this.m_coef_bits_latch = new int[this.m_cinfo.m_num_components * 6];
      this.m_coef_bits_savedOffset = 0;
    }
    bool flag = false;
    for (int index1 = 0; index1 < this.m_cinfo.m_num_components; ++index1)
    {
      JQUANT_TBL quantTable = this.m_cinfo.Comp_info[index1].quant_table;
      if (quantTable == null || quantTable.quantval[0] == (short) 0 || quantTable.quantval[1] == (short) 0 || quantTable.quantval[8] == (short) 0 || quantTable.quantval[16 /*0x10*/] == (short) 0 || quantTable.quantval[9] == (short) 0 || quantTable.quantval[2] == (short) 0 || this.m_cinfo.m_coef_bits[index1][0] < 0)
        return false;
      for (int index2 = 1; index2 <= 5; ++index2)
      {
        this.m_coef_bits_latch[this.m_coef_bits_savedOffset + index2] = this.m_cinfo.m_coef_bits[index1][index2];
        if (this.m_cinfo.m_coef_bits[index1][index2] != 0)
          flag = true;
      }
      this.m_coef_bits_savedOffset += 6;
    }
    return flag;
  }

  private void start_iMCU_row()
  {
    if (this.m_cinfo.m_comps_in_scan > 1)
    {
      this.m_MCU_rows_per_iMCU_row = 1;
    }
    else
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[0]];
      this.m_MCU_rows_per_iMCU_row = this.m_cinfo.m_input_iMCU_row >= this.m_cinfo.m_total_iMCU_rows - 1 ? componentInfo.last_row_height : componentInfo.V_samp_factor;
    }
    this.m_MCU_ctr = 0;
    this.m_MCU_vert_offset = 0;
  }

  private enum DecompressorType
  {
    Ordinary,
    Smooth,
    OnePass,
  }
}
