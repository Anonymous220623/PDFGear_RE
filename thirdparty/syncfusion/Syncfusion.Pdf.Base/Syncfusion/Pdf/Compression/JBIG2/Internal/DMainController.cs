// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.DMainController
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class DMainController
{
  private const int CTX_PREPARE_FOR_IMCU = 0;
  private const int CTX_PROCESS_IMCU = 1;
  private const int CTX_POSTPONED_ROW = 2;
  private DMainController.DataProcessor m_dataProcessor;
  private DecompressStruct m_cinfo;
  private byte[][][] m_buffer = new byte[10][][];
  private bool m_buffer_full;
  private int m_rowgroup_ctr;
  private int[][][] m_funnyIndices = new int[2][][]
  {
    new int[10][],
    new int[10][]
  };
  private int[] m_funnyOffsets = new int[10];
  private int m_whichFunny;
  private int m_context_state;
  private int m_rowgroups_avail;
  private int m_iMCU_row_ctr;

  public DMainController(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    int num1 = cinfo.m_min_DCT_scaled_size;
    if (cinfo.m_upsample.NeedContextRows())
    {
      this.alloc_funny_pointers();
      num1 = cinfo.m_min_DCT_scaled_size + 2;
    }
    for (int index = 0; index < cinfo.m_num_components; ++index)
    {
      int num2 = cinfo.Comp_info[index].V_samp_factor * cinfo.Comp_info[index].DCT_scaled_size / cinfo.m_min_DCT_scaled_size;
      this.m_buffer[index] = CommonStruct.AllocJpegSamples(cinfo.Comp_info[index].Width_in_blocks * cinfo.Comp_info[index].DCT_scaled_size, num2 * num1);
    }
  }

  public void start_pass(J_BUF_MODE pass_mode)
  {
    switch (pass_mode)
    {
      case J_BUF_MODE.JBUF_PASS_THRU:
        if (this.m_cinfo.m_upsample.NeedContextRows())
        {
          this.m_dataProcessor = DMainController.DataProcessor.context_main;
          this.make_funny_pointers();
          this.m_whichFunny = 0;
          this.m_context_state = 0;
          this.m_iMCU_row_ctr = 0;
        }
        else
          this.m_dataProcessor = DMainController.DataProcessor.simple_main;
        this.m_buffer_full = false;
        this.m_rowgroup_ctr = 0;
        break;
      case J_BUF_MODE.JBUF_CRANK_DEST:
        this.m_dataProcessor = DMainController.DataProcessor.crank_post;
        break;
    }
  }

  public void process_data(byte[][] output_buf, ref int out_row_ctr, int out_rows_avail)
  {
    switch (this.m_dataProcessor)
    {
      case DMainController.DataProcessor.context_main:
        this.process_data_context_main(output_buf, ref out_row_ctr, out_rows_avail);
        break;
      case DMainController.DataProcessor.simple_main:
        this.process_data_simple_main(output_buf, ref out_row_ctr, out_rows_avail);
        break;
      case DMainController.DataProcessor.crank_post:
        this.process_data_crank_post(output_buf, ref out_row_ctr, out_rows_avail);
        break;
    }
  }

  private void process_data_simple_main(
    byte[][] output_buf,
    ref int out_row_ctr,
    int out_rows_avail)
  {
    ComponentBuffer[] componentBufferArray = new ComponentBuffer[10];
    for (int index = 0; index < 10; ++index)
    {
      componentBufferArray[index] = new ComponentBuffer();
      componentBufferArray[index].SetBuffer(this.m_buffer[index], (int[]) null, 0);
    }
    if (!this.m_buffer_full)
    {
      if (this.m_cinfo.m_coef.decompress_data(componentBufferArray) == ReadResult.JPEG_SUSPENDED)
        return;
      this.m_buffer_full = true;
    }
    int minDctScaledSize = this.m_cinfo.m_min_DCT_scaled_size;
    this.m_cinfo.m_post.post_process_data(componentBufferArray, ref this.m_rowgroup_ctr, minDctScaledSize, output_buf, ref out_row_ctr, out_rows_avail);
    if (this.m_rowgroup_ctr < minDctScaledSize)
      return;
    this.m_buffer_full = false;
    this.m_rowgroup_ctr = 0;
  }

  private void process_data_context_main(
    byte[][] output_buf,
    ref int out_row_ctr,
    int out_rows_avail)
  {
    ComponentBuffer[] componentBufferArray = new ComponentBuffer[this.m_cinfo.m_num_components];
    for (int index = 0; index < this.m_cinfo.m_num_components; ++index)
    {
      componentBufferArray[index] = new ComponentBuffer();
      componentBufferArray[index].SetBuffer(this.m_buffer[index], this.m_funnyIndices[this.m_whichFunny][index], this.m_funnyOffsets[index]);
    }
    if (!this.m_buffer_full)
    {
      if (this.m_cinfo.m_coef.decompress_data(componentBufferArray) == ReadResult.JPEG_SUSPENDED)
        return;
      this.m_buffer_full = true;
      ++this.m_iMCU_row_ctr;
    }
    if (this.m_context_state == 2)
    {
      this.m_cinfo.m_post.post_process_data(componentBufferArray, ref this.m_rowgroup_ctr, this.m_rowgroups_avail, output_buf, ref out_row_ctr, out_rows_avail);
      if (this.m_rowgroup_ctr < this.m_rowgroups_avail)
        return;
      this.m_context_state = 0;
      if (out_row_ctr >= out_rows_avail)
        return;
    }
    if (this.m_context_state == 0)
    {
      this.m_rowgroup_ctr = 0;
      this.m_rowgroups_avail = this.m_cinfo.m_min_DCT_scaled_size - 1;
      if (this.m_iMCU_row_ctr == this.m_cinfo.m_total_iMCU_rows)
        this.set_bottom_pointers();
      this.m_context_state = 1;
    }
    if (this.m_context_state != 1)
      return;
    this.m_cinfo.m_post.post_process_data(componentBufferArray, ref this.m_rowgroup_ctr, this.m_rowgroups_avail, output_buf, ref out_row_ctr, out_rows_avail);
    if (this.m_rowgroup_ctr < this.m_rowgroups_avail)
      return;
    if (this.m_iMCU_row_ctr == 1)
      this.set_wraparound_pointers();
    this.m_whichFunny ^= 1;
    this.m_buffer_full = false;
    this.m_rowgroup_ctr = this.m_cinfo.m_min_DCT_scaled_size + 1;
    this.m_rowgroups_avail = this.m_cinfo.m_min_DCT_scaled_size + 2;
    this.m_context_state = 2;
  }

  private void process_data_crank_post(
    byte[][] output_buf,
    ref int out_row_ctr,
    int out_rows_avail)
  {
    int in_row_group_ctr = 0;
    this.m_cinfo.m_post.post_process_data((ComponentBuffer[]) null, ref in_row_group_ctr, 0, output_buf, ref out_row_ctr, out_rows_avail);
  }

  private void alloc_funny_pointers()
  {
    int minDctScaledSize = this.m_cinfo.m_min_DCT_scaled_size;
    for (int index = 0; index < this.m_cinfo.m_num_components; ++index)
    {
      int num = this.m_cinfo.Comp_info[index].V_samp_factor * this.m_cinfo.Comp_info[index].DCT_scaled_size / this.m_cinfo.m_min_DCT_scaled_size;
      this.m_funnyIndices[0][index] = new int[num * (minDctScaledSize + 4)];
      this.m_funnyIndices[1][index] = new int[num * (minDctScaledSize + 4)];
      this.m_funnyOffsets[index] = num;
    }
  }

  private void make_funny_pointers()
  {
    int minDctScaledSize = this.m_cinfo.m_min_DCT_scaled_size;
    for (int index1 = 0; index1 < this.m_cinfo.m_num_components; ++index1)
    {
      int index2 = this.m_cinfo.Comp_info[index1].V_samp_factor * this.m_cinfo.Comp_info[index1].DCT_scaled_size / this.m_cinfo.m_min_DCT_scaled_size;
      int[] numArray1 = this.m_funnyIndices[0][index1];
      int[] numArray2 = this.m_funnyIndices[1][index1];
      for (int index3 = 0; index3 < index2 * (minDctScaledSize + 2); ++index3)
      {
        numArray1[index3 + index2] = index3;
        numArray2[index3 + index2] = index3;
      }
      for (int index4 = 0; index4 < index2 * 2; ++index4)
      {
        numArray2[index2 * (minDctScaledSize - 1) + index4] = index2 * minDctScaledSize + index4;
        numArray2[index2 * (minDctScaledSize + 1) + index4] = index2 * (minDctScaledSize - 2) + index4;
      }
      for (int index5 = 0; index5 < index2; ++index5)
        numArray1[index5] = numArray1[index2];
    }
  }

  private void set_wraparound_pointers()
  {
    int minDctScaledSize = this.m_cinfo.m_min_DCT_scaled_size;
    for (int index1 = 0; index1 < this.m_cinfo.m_num_components; ++index1)
    {
      int num = this.m_cinfo.Comp_info[index1].V_samp_factor * this.m_cinfo.Comp_info[index1].DCT_scaled_size / this.m_cinfo.m_min_DCT_scaled_size;
      int[] numArray1 = this.m_funnyIndices[0][index1];
      int[] numArray2 = this.m_funnyIndices[1][index1];
      for (int index2 = 0; index2 < num; ++index2)
      {
        numArray1[index2] = numArray1[num * (minDctScaledSize + 2) + index2];
        numArray2[index2] = numArray2[num * (minDctScaledSize + 2) + index2];
        numArray1[num * (minDctScaledSize + 3) + index2] = numArray1[index2 + num];
        numArray2[num * (minDctScaledSize + 3) + index2] = numArray2[index2 + num];
      }
    }
  }

  private void set_bottom_pointers()
  {
    for (int index1 = 0; index1 < this.m_cinfo.m_num_components; ++index1)
    {
      int num1 = this.m_cinfo.Comp_info[index1].V_samp_factor * this.m_cinfo.Comp_info[index1].DCT_scaled_size;
      int num2 = num1 / this.m_cinfo.m_min_DCT_scaled_size;
      int num3 = this.m_cinfo.Comp_info[index1].downsampled_height % num1;
      if (num3 == 0)
        num3 = num1;
      if (index1 == 0)
        this.m_rowgroups_avail = (num3 - 1) / num2 + 1;
      for (int index2 = 0; index2 < num2 * 2; ++index2)
        this.m_funnyIndices[this.m_whichFunny][index1][num3 + index2 + num2] = this.m_funnyIndices[this.m_whichFunny][index1][num3 - 1 + num2];
    }
  }

  private enum DataProcessor
  {
    context_main,
    simple_main,
    crank_post,
  }
}
