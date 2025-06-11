// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.InputController
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class InputController
{
  private DecompressStruct m_cinfo;
  private bool m_consumeData;
  private bool m_inheaders;
  private bool m_has_multiple_scans;
  private bool m_eoi_reached;

  public InputController(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.m_inheaders = true;
  }

  public ReadResult consume_input()
  {
    return this.m_consumeData ? this.m_cinfo.m_coef.consume_data() : this.consume_markers();
  }

  public void reset_input_controller()
  {
    this.m_consumeData = false;
    this.m_has_multiple_scans = false;
    this.m_eoi_reached = false;
    this.m_inheaders = true;
    this.m_cinfo.m_marker.reset_marker_reader();
    this.m_cinfo.m_coef_bits = (int[][]) null;
  }

  public void start_input_pass()
  {
    this.per_scan_setup();
    this.latch_quant_tables();
    this.m_cinfo.m_entropy.start_pass();
    this.m_cinfo.m_coef.start_input_pass();
    this.m_consumeData = true;
  }

  public void finish_input_pass() => this.m_consumeData = false;

  public bool HasMultipleScans() => this.m_has_multiple_scans;

  public bool EOIReached() => this.m_eoi_reached;

  private ReadResult consume_markers()
  {
    if (this.m_eoi_reached)
      return ReadResult.JPEG_REACHED_EOI;
    ReadResult readResult = this.m_cinfo.m_marker.read_markers();
    switch (readResult)
    {
      case ReadResult.JPEG_REACHED_SOS:
        if (this.m_inheaders)
        {
          this.initial_setup();
          this.m_inheaders = false;
          break;
        }
        int num = this.m_has_multiple_scans ? 1 : 0;
        this.m_cinfo.m_inputctl.start_input_pass();
        break;
      case ReadResult.JPEG_REACHED_EOI:
        this.m_eoi_reached = true;
        if (this.m_inheaders)
        {
          if (!this.m_cinfo.m_marker.SawSOF())
            break;
          break;
        }
        if (this.m_cinfo.m_output_scan_number > this.m_cinfo.m_input_scan_number)
        {
          this.m_cinfo.m_output_scan_number = this.m_cinfo.m_input_scan_number;
          break;
        }
        break;
    }
    return readResult;
  }

  private void initial_setup()
  {
    if (this.m_cinfo.m_image_height <= 65500)
    {
      int imageWidth = this.m_cinfo.m_image_width;
    }
    this.m_cinfo.m_max_h_samp_factor = 1;
    this.m_cinfo.m_max_v_samp_factor = 1;
    for (int index = 0; index < this.m_cinfo.m_num_components; ++index)
    {
      if (this.m_cinfo.Comp_info[index].H_samp_factor > 0 && this.m_cinfo.Comp_info[index].H_samp_factor <= 4 && this.m_cinfo.Comp_info[index].V_samp_factor > 0)
      {
        int vSampFactor = this.m_cinfo.Comp_info[index].V_samp_factor;
      }
      this.m_cinfo.m_max_h_samp_factor = Math.Max(this.m_cinfo.m_max_h_samp_factor, this.m_cinfo.Comp_info[index].H_samp_factor);
      this.m_cinfo.m_max_v_samp_factor = Math.Max(this.m_cinfo.m_max_v_samp_factor, this.m_cinfo.Comp_info[index].V_samp_factor);
    }
    this.m_cinfo.m_min_DCT_scaled_size = 8;
    for (int index = 0; index < this.m_cinfo.m_num_components; ++index)
    {
      this.m_cinfo.Comp_info[index].DCT_scaled_size = 8;
      this.m_cinfo.Comp_info[index].Width_in_blocks = JpegUtils.jdiv_round_up(this.m_cinfo.m_image_width * this.m_cinfo.Comp_info[index].H_samp_factor, this.m_cinfo.m_max_h_samp_factor * 8);
      this.m_cinfo.Comp_info[index].height_in_blocks = JpegUtils.jdiv_round_up(this.m_cinfo.m_image_height * this.m_cinfo.Comp_info[index].V_samp_factor, this.m_cinfo.m_max_v_samp_factor * 8);
      this.m_cinfo.Comp_info[index].downsampled_width = JpegUtils.jdiv_round_up(this.m_cinfo.m_image_width * this.m_cinfo.Comp_info[index].H_samp_factor, this.m_cinfo.m_max_h_samp_factor);
      this.m_cinfo.Comp_info[index].downsampled_height = JpegUtils.jdiv_round_up(this.m_cinfo.m_image_height * this.m_cinfo.Comp_info[index].V_samp_factor, this.m_cinfo.m_max_v_samp_factor);
      this.m_cinfo.Comp_info[index].component_needed = true;
      this.m_cinfo.Comp_info[index].quant_table = (JQUANT_TBL) null;
    }
    this.m_cinfo.m_total_iMCU_rows = JpegUtils.jdiv_round_up(this.m_cinfo.m_image_height, this.m_cinfo.m_max_v_samp_factor * 8);
    if (this.m_cinfo.m_comps_in_scan < this.m_cinfo.m_num_components || this.m_cinfo.m_progressive_mode)
      this.m_cinfo.m_inputctl.m_has_multiple_scans = true;
    else
      this.m_cinfo.m_inputctl.m_has_multiple_scans = false;
  }

  private void latch_quant_tables()
  {
    for (int index = 0; index < this.m_cinfo.m_comps_in_scan; ++index)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index]];
      if (componentInfo.quant_table == null)
      {
        int quantTblNo = componentInfo.Quant_tbl_no;
        JQUANT_TBL jquantTbl = new JQUANT_TBL();
        Buffer.BlockCopy((Array) this.m_cinfo.m_quant_tbl_ptrs[quantTblNo].quantval, 0, (Array) jquantTbl.quantval, 0, jquantTbl.quantval.Length * 2);
        jquantTbl.Sent_table = this.m_cinfo.m_quant_tbl_ptrs[quantTblNo].Sent_table;
        componentInfo.quant_table = jquantTbl;
        this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index]] = componentInfo;
      }
    }
  }

  private void per_scan_setup()
  {
    if (this.m_cinfo.m_comps_in_scan == 1)
    {
      ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[0]];
      this.m_cinfo.m_MCUs_per_row = componentInfo.Width_in_blocks;
      this.m_cinfo.m_MCU_rows_in_scan = componentInfo.height_in_blocks;
      componentInfo.MCU_width = 1;
      componentInfo.MCU_height = 1;
      componentInfo.MCU_blocks = 1;
      componentInfo.MCU_sample_width = componentInfo.DCT_scaled_size;
      componentInfo.last_col_width = 1;
      int num = componentInfo.height_in_blocks % componentInfo.V_samp_factor;
      if (num == 0)
        num = componentInfo.V_samp_factor;
      componentInfo.last_row_height = num;
      this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[0]] = componentInfo;
      this.m_cinfo.m_blocks_in_MCU = 1;
      this.m_cinfo.m_MCU_membership[0] = 0;
    }
    else
    {
      this.m_cinfo.m_MCUs_per_row = JpegUtils.jdiv_round_up(this.m_cinfo.m_image_width, this.m_cinfo.m_max_h_samp_factor * 8);
      this.m_cinfo.m_MCU_rows_in_scan = JpegUtils.jdiv_round_up(this.m_cinfo.m_image_height, this.m_cinfo.m_max_v_samp_factor * 8);
      this.m_cinfo.m_blocks_in_MCU = 0;
      for (int index = 0; index < this.m_cinfo.m_comps_in_scan; ++index)
      {
        ComponentInfo componentInfo = this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index]];
        componentInfo.MCU_width = componentInfo.H_samp_factor;
        componentInfo.MCU_height = componentInfo.V_samp_factor;
        componentInfo.MCU_blocks = componentInfo.MCU_width * componentInfo.MCU_height;
        componentInfo.MCU_sample_width = componentInfo.MCU_width * componentInfo.DCT_scaled_size;
        int num1 = componentInfo.Width_in_blocks % componentInfo.MCU_width;
        if (num1 == 0)
          num1 = componentInfo.MCU_width;
        componentInfo.last_col_width = num1;
        int num2 = componentInfo.height_in_blocks % componentInfo.MCU_height;
        if (num2 == 0)
          num2 = componentInfo.MCU_height;
        componentInfo.last_row_height = num2;
        int mcuBlocks = componentInfo.MCU_blocks;
        this.m_cinfo.Comp_info[this.m_cinfo.m_cur_comp_info[index]] = componentInfo;
        while (mcuBlocks-- > 0)
          this.m_cinfo.m_MCU_membership[this.m_cinfo.m_blocks_in_MCU++] = index;
      }
    }
  }
}
