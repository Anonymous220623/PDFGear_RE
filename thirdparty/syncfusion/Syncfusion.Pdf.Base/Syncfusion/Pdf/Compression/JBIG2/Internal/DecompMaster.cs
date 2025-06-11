// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.DecompMaster
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class DecompMaster
{
  private DecompressStruct m_cinfo;
  private int m_pass_number;
  private bool m_is_dummy_pass;
  private bool m_using_merged_upsample;
  private ColorQuantizer m_quantizer_1pass;
  private ColorQuantizer m_quantizer_2pass;

  public DecompMaster(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.master_selection();
  }

  public void prepare_for_output_pass()
  {
    if (this.m_is_dummy_pass)
    {
      this.m_is_dummy_pass = false;
      this.m_cinfo.m_cquantize.start_pass(false);
      this.m_cinfo.m_post.start_pass(J_BUF_MODE.JBUF_CRANK_DEST);
      this.m_cinfo.m_main.start_pass(J_BUF_MODE.JBUF_CRANK_DEST);
    }
    else
    {
      if (this.m_cinfo.m_quantize_colors && this.m_cinfo.m_colormap == null)
      {
        if (this.m_cinfo.m_two_pass_quantize && this.m_cinfo.m_enable_2pass_quant)
        {
          this.m_cinfo.m_cquantize = this.m_quantizer_2pass;
          this.m_is_dummy_pass = true;
        }
        else if (this.m_cinfo.m_enable_1pass_quant)
          this.m_cinfo.m_cquantize = this.m_quantizer_1pass;
      }
      this.m_cinfo.m_idct.start_pass();
      this.m_cinfo.m_coef.start_output_pass();
      if (!this.m_cinfo.m_raw_data_out)
      {
        this.m_cinfo.m_upsample.start_pass();
        if (this.m_cinfo.m_quantize_colors)
          this.m_cinfo.m_cquantize.start_pass(this.m_is_dummy_pass);
        this.m_cinfo.m_post.start_pass(this.m_is_dummy_pass ? J_BUF_MODE.JBUF_SAVE_AND_PASS : J_BUF_MODE.JBUF_PASS_THRU);
        this.m_cinfo.m_main.start_pass(J_BUF_MODE.JBUF_PASS_THRU);
      }
    }
    if (this.m_cinfo.m_progress == null)
      return;
    this.m_cinfo.m_progress.Completed_passes = this.m_pass_number;
    this.m_cinfo.m_progress.Total_passes = this.m_pass_number + (this.m_is_dummy_pass ? 2 : 1);
    if (!this.m_cinfo.m_buffered_image || this.m_cinfo.m_inputctl.EOIReached())
      return;
    this.m_cinfo.m_progress.Total_passes += this.m_cinfo.m_enable_2pass_quant ? 2 : 1;
  }

  public void finish_output_pass()
  {
    if (this.m_cinfo.m_quantize_colors)
      this.m_cinfo.m_cquantize.finish_pass();
    ++this.m_pass_number;
  }

  public bool IsDummyPass() => this.m_is_dummy_pass;

  private void master_selection()
  {
    this.m_cinfo.jpeg_calc_output_dimensions();
    this.prepare_range_limit_table();
    long num1 = (long) (this.m_cinfo.m_output_width * this.m_cinfo.m_out_color_components);
    this.m_pass_number = 0;
    this.m_using_merged_upsample = this.m_cinfo.use_merged_upsample();
    this.m_quantizer_1pass = (ColorQuantizer) null;
    this.m_quantizer_2pass = (ColorQuantizer) null;
    if (!this.m_cinfo.m_quantize_colors || !this.m_cinfo.m_buffered_image)
    {
      this.m_cinfo.m_enable_1pass_quant = false;
      this.m_cinfo.m_enable_external_quant = false;
      this.m_cinfo.m_enable_2pass_quant = false;
    }
    if (this.m_cinfo.m_quantize_colors)
    {
      if (this.m_cinfo.m_out_color_components != 3)
      {
        this.m_cinfo.m_enable_1pass_quant = true;
        this.m_cinfo.m_enable_external_quant = false;
        this.m_cinfo.m_enable_2pass_quant = false;
        this.m_cinfo.m_colormap = (byte[][]) null;
      }
      else if (this.m_cinfo.m_colormap != null)
        this.m_cinfo.m_enable_external_quant = true;
      else if (this.m_cinfo.m_two_pass_quantize)
        this.m_cinfo.m_enable_2pass_quant = true;
      else
        this.m_cinfo.m_enable_1pass_quant = true;
      if (this.m_cinfo.m_enable_1pass_quant)
      {
        this.m_cinfo.m_cquantize = (ColorQuantizer) new Ex1PassCQuantizer(this.m_cinfo);
        this.m_quantizer_1pass = this.m_cinfo.m_cquantize;
      }
      if (this.m_cinfo.m_enable_2pass_quant || this.m_cinfo.m_enable_external_quant)
      {
        this.m_cinfo.m_cquantize = (ColorQuantizer) new Ex2PassCQuantizer(this.m_cinfo);
        this.m_quantizer_2pass = this.m_cinfo.m_cquantize;
      }
    }
    if (!this.m_cinfo.m_raw_data_out)
    {
      if (this.m_using_merged_upsample)
      {
        this.m_cinfo.m_upsample = (Upsampler) new ExMergedUpsampler(this.m_cinfo);
      }
      else
      {
        this.m_cinfo.m_cconvert = new ColorDeconverter(this.m_cinfo);
        this.m_cinfo.m_upsample = (Upsampler) new ExUpsampler(this.m_cinfo);
      }
      this.m_cinfo.m_post = new DPostController(this.m_cinfo, this.m_cinfo.m_enable_2pass_quant);
    }
    this.m_cinfo.m_idct = new InverseDCT(this.m_cinfo);
    this.m_cinfo.m_entropy = !this.m_cinfo.m_progressive_mode ? (EntropyDecoder) new HuffEntropyDecoder(this.m_cinfo) : (EntropyDecoder) new PHuffEntropyDecoder(this.m_cinfo);
    this.m_cinfo.m_coef = new DCoefController(this.m_cinfo, this.m_cinfo.m_inputctl.HasMultipleScans() || this.m_cinfo.m_buffered_image);
    if (!this.m_cinfo.m_raw_data_out)
      this.m_cinfo.m_main = new DMainController(this.m_cinfo);
    this.m_cinfo.m_inputctl.start_input_pass();
    if (this.m_cinfo.m_progress == null || this.m_cinfo.m_buffered_image || !this.m_cinfo.m_inputctl.HasMultipleScans())
      return;
    int num2 = !this.m_cinfo.m_progressive_mode ? this.m_cinfo.m_num_components : 2 + 3 * this.m_cinfo.m_num_components;
    this.m_cinfo.m_progress.Pass_counter = 0;
    this.m_cinfo.m_progress.Pass_limit = this.m_cinfo.m_total_iMCU_rows * num2;
    this.m_cinfo.m_progress.Completed_passes = 0;
    this.m_cinfo.m_progress.Total_passes = this.m_cinfo.m_enable_2pass_quant ? 3 : 2;
    ++this.m_pass_number;
  }

  private void prepare_range_limit_table()
  {
    byte[] dst = new byte[1408];
    int num1 = 256 /*0x0100*/;
    this.m_cinfo.m_sample_range_limit = dst;
    this.m_cinfo.m_sampleRangeLimitOffset = num1;
    Array.Clear((Array) dst, 0, 256 /*0x0100*/);
    for (int index = 0; index <= (int) byte.MaxValue; ++index)
      dst[num1 + index] = (byte) index;
    int num2 = num1 + 128 /*0x80*/;
    for (int index = 128 /*0x80*/; index < 512 /*0x0200*/; ++index)
      dst[num2 + index] = byte.MaxValue;
    Array.Clear((Array) dst, num2 + 512 /*0x0200*/, 384);
    Buffer.BlockCopy((Array) this.m_cinfo.m_sample_range_limit, 0, (Array) dst, num2 + 1024 /*0x0400*/ - 128 /*0x80*/, 128 /*0x80*/);
  }
}
