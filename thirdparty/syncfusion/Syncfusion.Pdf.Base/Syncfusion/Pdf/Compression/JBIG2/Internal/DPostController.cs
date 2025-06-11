// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.DPostController
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class DPostController
{
  private DPostController.ProcessorType m_processor;
  private DecompressStruct m_cinfo;
  private jvirt_array<byte> m_whole_image;
  private byte[][] m_buffer;
  private int m_strip_height;
  private int m_starting_row;
  private int m_next_row;

  public DPostController(DecompressStruct cinfo, bool need_full_buffer)
  {
    this.m_cinfo = cinfo;
    if (!cinfo.m_quantize_colors)
      return;
    this.m_strip_height = cinfo.m_max_v_samp_factor;
    if (need_full_buffer)
    {
      this.m_whole_image = CommonStruct.CreateSamplesArray(cinfo.m_output_width * cinfo.m_out_color_components, JpegUtils.jround_up(cinfo.m_output_height, this.m_strip_height));
      this.m_whole_image.ErrorProcessor = (CommonStruct) cinfo;
    }
    else
      this.m_buffer = CommonStruct.AllocJpegSamples(cinfo.m_output_width * cinfo.m_out_color_components, this.m_strip_height);
  }

  public void start_pass(J_BUF_MODE pass_mode)
  {
    switch (pass_mode)
    {
      case J_BUF_MODE.JBUF_PASS_THRU:
        if (this.m_cinfo.m_quantize_colors)
        {
          this.m_processor = DPostController.ProcessorType.OnePass;
          if (this.m_buffer == null)
          {
            this.m_buffer = this.m_whole_image.Access(0, this.m_strip_height);
            break;
          }
          break;
        }
        this.m_processor = DPostController.ProcessorType.Upsample;
        break;
      case J_BUF_MODE.JBUF_CRANK_DEST:
        this.m_processor = DPostController.ProcessorType.SecondPass;
        break;
      case J_BUF_MODE.JBUF_SAVE_AND_PASS:
        this.m_processor = DPostController.ProcessorType.PrePass;
        break;
    }
    this.m_starting_row = this.m_next_row = 0;
  }

  public void post_process_data(
    ComponentBuffer[] input_buf,
    ref int in_row_group_ctr,
    int in_row_groups_avail,
    byte[][] output_buf,
    ref int out_row_ctr,
    int out_rows_avail)
  {
    switch (this.m_processor)
    {
      case DPostController.ProcessorType.OnePass:
        this.post_process_1pass(input_buf, ref in_row_group_ctr, in_row_groups_avail, output_buf, ref out_row_ctr, out_rows_avail);
        break;
      case DPostController.ProcessorType.PrePass:
        this.post_process_prepass(input_buf, ref in_row_group_ctr, in_row_groups_avail, ref out_row_ctr);
        break;
      case DPostController.ProcessorType.Upsample:
        this.m_cinfo.m_upsample.upsample(input_buf, ref in_row_group_ctr, in_row_groups_avail, output_buf, ref out_row_ctr, out_rows_avail);
        break;
      case DPostController.ProcessorType.SecondPass:
        this.post_process_2pass(output_buf, ref out_row_ctr, out_rows_avail);
        break;
    }
  }

  private void post_process_1pass(
    ComponentBuffer[] input_buf,
    ref int in_row_group_ctr,
    int in_row_groups_avail,
    byte[][] output_buf,
    ref int out_row_ctr,
    int out_rows_avail)
  {
    int out_rows_avail1 = out_rows_avail - out_row_ctr;
    if (out_rows_avail1 > this.m_strip_height)
      out_rows_avail1 = this.m_strip_height;
    int out_row_ctr1 = 0;
    this.m_cinfo.m_upsample.upsample(input_buf, ref in_row_group_ctr, in_row_groups_avail, this.m_buffer, ref out_row_ctr1, out_rows_avail1);
    this.m_cinfo.m_cquantize.color_quantize(this.m_buffer, 0, output_buf, out_row_ctr, out_row_ctr1);
    out_row_ctr += out_row_ctr1;
  }

  private void post_process_prepass(
    ComponentBuffer[] input_buf,
    ref int in_row_group_ctr,
    int in_row_groups_avail,
    ref int out_row_ctr)
  {
    if (this.m_next_row == 0)
      this.m_buffer = this.m_whole_image.Access(this.m_starting_row, this.m_strip_height);
    int nextRow = this.m_next_row;
    this.m_cinfo.m_upsample.upsample(input_buf, ref in_row_group_ctr, in_row_groups_avail, this.m_buffer, ref this.m_next_row, this.m_strip_height);
    if (this.m_next_row > nextRow)
    {
      int num_rows = this.m_next_row - nextRow;
      this.m_cinfo.m_cquantize.color_quantize(this.m_buffer, nextRow, (byte[][]) null, 0, num_rows);
      out_row_ctr += num_rows;
    }
    if (this.m_next_row < this.m_strip_height)
      return;
    this.m_starting_row += this.m_strip_height;
    this.m_next_row = 0;
  }

  private void post_process_2pass(byte[][] output_buf, ref int out_row_ctr, int out_rows_avail)
  {
    if (this.m_next_row == 0)
      this.m_buffer = this.m_whole_image.Access(this.m_starting_row, this.m_strip_height);
    int num_rows = this.m_strip_height - this.m_next_row;
    int num1 = out_rows_avail - out_row_ctr;
    if (num_rows > num1)
      num_rows = num1;
    int num2 = this.m_cinfo.m_output_height - this.m_starting_row;
    if (num_rows > num2)
      num_rows = num2;
    this.m_cinfo.m_cquantize.color_quantize(this.m_buffer, this.m_next_row, output_buf, out_row_ctr, num_rows);
    out_row_ctr += num_rows;
    this.m_next_row += num_rows;
    if (this.m_next_row < this.m_strip_height)
      return;
    this.m_starting_row += this.m_strip_height;
    this.m_next_row = 0;
  }

  private enum ProcessorType
  {
    OnePass,
    PrePass,
    Upsample,
    SecondPass,
  }
}
