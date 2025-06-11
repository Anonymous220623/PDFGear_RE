// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.DecompressStruct
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression.JBIG2.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class DecompressStruct : CommonStruct
{
  internal SourceMgr m_src;
  internal int m_image_width;
  internal int m_image_height;
  internal int m_num_components;
  internal J_COLOR_SPACE m_jpeg_color_space;
  internal J_COLOR_SPACE m_out_color_space;
  internal int m_scale_num;
  internal int m_scale_denom;
  internal bool m_buffered_image;
  internal bool m_raw_data_out;
  internal J_DCT_METHOD m_dct_method;
  internal bool m_do_fancy_upsampling;
  internal bool m_do_block_smoothing;
  internal bool m_quantize_colors;
  internal J_DITHER_MODE m_dither_mode;
  internal bool m_two_pass_quantize;
  internal int m_desired_number_of_colors;
  internal bool m_enable_1pass_quant;
  internal bool m_enable_external_quant;
  internal bool m_enable_2pass_quant;
  internal int m_output_width;
  internal int m_output_height;
  internal int m_out_color_components;
  internal int m_output_components;
  internal int m_rec_outbuf_height;
  internal int m_actual_number_of_colors;
  internal byte[][] m_colormap;
  internal int m_output_scanline;
  internal int m_input_scan_number;
  internal int m_input_iMCU_row;
  internal int m_output_scan_number;
  internal int m_output_iMCU_row;
  internal int[][] m_coef_bits;
  internal JQUANT_TBL[] m_quant_tbl_ptrs = new JQUANT_TBL[4];
  internal JHUFF_TBL[] m_dc_huff_tbl_ptrs = new JHUFF_TBL[4];
  internal JHUFF_TBL[] m_ac_huff_tbl_ptrs = new JHUFF_TBL[4];
  internal int m_data_precision;
  private ComponentInfo[] m_comp_info;
  internal bool m_progressive_mode;
  internal int m_restart_interval;
  internal bool m_saw_JFIF_marker;
  internal byte m_JFIF_major_version;
  internal byte m_JFIF_minor_version;
  internal DensityUnit m_density_unit;
  internal short m_X_density;
  internal short m_Y_density;
  internal bool m_saw_Adobe_marker;
  internal byte m_Adobe_transform;
  internal bool m_CCIR601_sampling;
  internal List<MarkerStruct> m_marker_list;
  internal int m_max_h_samp_factor;
  internal int m_max_v_samp_factor;
  internal int m_min_DCT_scaled_size;
  internal int m_total_iMCU_rows;
  internal byte[] m_sample_range_limit;
  internal int m_sampleRangeLimitOffset;
  internal int m_comps_in_scan;
  internal int[] m_cur_comp_info = new int[4];
  internal int m_MCUs_per_row;
  internal int m_MCU_rows_in_scan;
  internal int m_blocks_in_MCU;
  internal int[] m_MCU_membership = new int[10];
  internal int m_Ss;
  internal int m_Se;
  internal int m_Ah;
  internal int m_Al;
  internal int m_unread_marker;
  internal DecompMaster m_master;
  internal DMainController m_main;
  internal DCoefController m_coef;
  internal DPostController m_post;
  internal InputController m_inputctl;
  internal MarkerReader m_marker;
  internal Syncfusion.Pdf.Compression.JBIG2.Internal.EntropyDecoder m_entropy;
  internal InverseDCT m_idct;
  internal Upsampler m_upsample;
  internal ColorDeconverter m_cconvert;
  internal ColorQuantizer m_cquantize;

  public DecompressStruct() => this.initialize();

  public override bool IsDecompressor => true;

  public SourceMgr Src
  {
    get => this.m_src;
    set => this.m_src = value;
  }

  public int Image_width => this.m_image_width;

  public int Image_height => this.m_image_height;

  public int Num_components => this.m_num_components;

  public J_COLOR_SPACE Jpeg_color_space
  {
    get => this.m_jpeg_color_space;
    set => this.m_jpeg_color_space = value;
  }

  public ReadOnlyCollection<MarkerStruct> Marker_list => this.m_marker_list.AsReadOnly();

  public J_COLOR_SPACE Out_color_space
  {
    get => this.m_out_color_space;
    set => this.m_out_color_space = value;
  }

  public int Scale_num
  {
    get => this.m_scale_num;
    set => this.m_scale_num = value;
  }

  public int Scale_denom
  {
    get => this.m_scale_denom;
    set => this.m_scale_denom = value;
  }

  public bool Buffered_image
  {
    get => this.m_buffered_image;
    set => this.m_buffered_image = value;
  }

  public bool Raw_data_out
  {
    get => this.m_raw_data_out;
    set => this.m_raw_data_out = value;
  }

  public J_DCT_METHOD Dct_method
  {
    get => this.m_dct_method;
    set => this.m_dct_method = value;
  }

  public bool Do_fancy_upsampling
  {
    get => this.m_do_fancy_upsampling;
    set => this.m_do_fancy_upsampling = value;
  }

  public bool Do_block_smoothing
  {
    get => this.m_do_block_smoothing;
    set => this.m_do_block_smoothing = value;
  }

  public bool Quantize_colors
  {
    get => this.m_quantize_colors;
    set => this.m_quantize_colors = value;
  }

  public J_DITHER_MODE Dither_mode
  {
    get => this.m_dither_mode;
    set => this.m_dither_mode = value;
  }

  public bool Two_pass_quantize
  {
    get => this.m_two_pass_quantize;
    set => this.m_two_pass_quantize = value;
  }

  public int Desired_number_of_colors
  {
    get => this.m_desired_number_of_colors;
    set => this.m_desired_number_of_colors = value;
  }

  public bool Enable_1pass_quant
  {
    get => this.m_enable_1pass_quant;
    set => this.m_enable_1pass_quant = value;
  }

  public bool Enable_external_quant
  {
    get => this.m_enable_external_quant;
    set => this.m_enable_external_quant = value;
  }

  public bool Enable_2pass_quant
  {
    get => this.m_enable_2pass_quant;
    set => this.m_enable_2pass_quant = value;
  }

  public int Output_width => this.m_output_width;

  public int Output_height => this.m_output_height;

  public int Out_color_components => this.m_out_color_components;

  public int Output_components => this.m_output_components;

  public int Rec_outbuf_height => this.m_rec_outbuf_height;

  public int Actual_number_of_colors
  {
    get => this.m_actual_number_of_colors;
    set => this.m_actual_number_of_colors = value;
  }

  public byte[][] Colormap
  {
    get => this.m_colormap;
    set => this.m_colormap = value;
  }

  public int Output_scanline => this.m_output_scanline;

  public int Input_scan_number => this.m_input_scan_number;

  public int Input_iMCU_row => this.m_input_iMCU_row;

  public int Output_scan_number => this.m_output_scan_number;

  public int Output_iMCU_row => this.m_output_iMCU_row;

  public int[][] Coef_bits => this.m_coef_bits;

  public DensityUnit Density_unit => this.m_density_unit;

  public short X_density => this.m_X_density;

  public short Y_density => this.m_Y_density;

  public int Data_precision => this.m_data_precision;

  public int Max_v_samp_factor => this.m_max_v_samp_factor;

  public int Unread_marker => this.m_unread_marker;

  public ComponentInfo[] Comp_info
  {
    get => this.m_comp_info;
    internal set => this.m_comp_info = value;
  }

  public void jpeg_stdio_src(Stream infile)
  {
    if (this.m_src == null)
      this.m_src = (SourceMgr) new ExSourceMgr(this);
    if (!(this.m_src is ExSourceMgr src))
      return;
    src.Attach(infile);
  }

  public ReadResult jpeg_read_header(bool require_image)
  {
    switch (this.jpeg_consume_input())
    {
      case ReadResult.JPEG_REACHED_SOS:
        return ReadResult.JPEG_HEADER_OK;
      case ReadResult.JPEG_REACHED_EOI:
        this.jpeg_abort();
        return ReadResult.JPEG_HEADER_TABLES_ONLY;
      default:
        return ReadResult.JPEG_SUSPENDED;
    }
  }

  public bool jpeg_start_decompress()
  {
    if (this.m_global_state == CommonStruct.JpegState.DSTATE_READY)
    {
      this.m_master = new DecompMaster(this);
      if (this.m_buffered_image)
      {
        this.m_global_state = CommonStruct.JpegState.DSTATE_BUFIMAGE;
        return true;
      }
      this.m_global_state = CommonStruct.JpegState.DSTATE_PRELOAD;
    }
    if (this.m_global_state == CommonStruct.JpegState.DSTATE_PRELOAD)
    {
      if (this.m_inputctl.HasMultipleScans())
      {
        while (true)
        {
          do
          {
            ReadResult readResult;
            do
            {
              if (this.m_progress != null)
                this.m_progress.Updated();
              readResult = this.m_inputctl.consume_input();
              switch (readResult)
              {
                case ReadResult.JPEG_SUSPENDED:
                  return false;
                case ReadResult.JPEG_REACHED_EOI:
                  goto label_13;
                default:
                  continue;
              }
            }
            while (this.m_progress == null || readResult != ReadResult.JPEG_ROW_COMPLETED && readResult != ReadResult.JPEG_REACHED_SOS);
            ++this.m_progress.Pass_counter;
          }
          while (this.m_progress.Pass_counter < this.m_progress.Pass_limit);
          this.m_progress.Pass_limit += this.m_total_iMCU_rows;
        }
      }
label_13:
      this.m_output_scan_number = this.m_input_scan_number;
    }
    return this.output_pass_setup();
  }

  public int jpeg_read_scanlines(byte[][] scanlines, int max_lines)
  {
    if (this.m_output_scanline >= this.m_output_height)
      return 0;
    if (this.m_progress != null)
    {
      this.m_progress.Pass_counter = this.m_output_scanline;
      this.m_progress.Pass_limit = this.m_output_height;
      this.m_progress.Updated();
    }
    int out_row_ctr = 0;
    this.m_main.process_data(scanlines, ref out_row_ctr, max_lines);
    this.m_output_scanline += out_row_ctr;
    return out_row_ctr;
  }

  public bool jpeg_finish_decompress()
  {
    if ((this.m_global_state == CommonStruct.JpegState.DSTATE_SCANNING || this.m_global_state == CommonStruct.JpegState.DSTATE_RAW_OK) && !this.m_buffered_image)
    {
      this.m_master.finish_output_pass();
      this.m_global_state = CommonStruct.JpegState.DSTATE_STOPPING;
    }
    else if (this.m_global_state == CommonStruct.JpegState.DSTATE_BUFIMAGE)
      this.m_global_state = CommonStruct.JpegState.DSTATE_STOPPING;
    else if (this.m_global_state == CommonStruct.JpegState.DSTATE_STOPPING)
      ;
    while (!this.m_inputctl.EOIReached())
    {
      if (this.m_inputctl.consume_input() == ReadResult.JPEG_SUSPENDED)
        return false;
    }
    this.m_src.term_source();
    this.jpeg_abort();
    return true;
  }

  public int jpeg_read_raw_data(byte[][][] data, int max_lines)
  {
    if (this.m_output_scanline >= this.m_output_height)
      return 0;
    if (this.m_progress != null)
    {
      this.m_progress.Pass_counter = this.m_output_scanline;
      this.m_progress.Pass_limit = this.m_output_height;
      this.m_progress.Updated();
    }
    int num = this.m_max_v_samp_factor * this.m_min_DCT_scaled_size;
    int length = data.Length;
    ComponentBuffer[] output_buf = new ComponentBuffer[length];
    for (int index = 0; index < length; ++index)
    {
      output_buf[index] = new ComponentBuffer();
      output_buf[index].SetBuffer(data[index], (int[]) null, 0);
    }
    if (this.m_coef.decompress_data(output_buf) == ReadResult.JPEG_SUSPENDED)
      return 0;
    this.m_output_scanline += num;
    return num;
  }

  public bool jpeg_has_multiple_scans() => this.m_inputctl.HasMultipleScans();

  public bool jpeg_start_output(int scan_number)
  {
    if (scan_number <= 0)
      scan_number = 1;
    if (this.m_inputctl.EOIReached() && scan_number > this.m_input_scan_number)
      scan_number = this.m_input_scan_number;
    this.m_output_scan_number = scan_number;
    return this.output_pass_setup();
  }

  public bool jpeg_finish_output()
  {
    if ((this.m_global_state == CommonStruct.JpegState.DSTATE_SCANNING || this.m_global_state == CommonStruct.JpegState.DSTATE_RAW_OK) && this.m_buffered_image)
    {
      this.m_master.finish_output_pass();
      this.m_global_state = CommonStruct.JpegState.DSTATE_BUFPOST;
    }
    else if (this.m_global_state == CommonStruct.JpegState.DSTATE_BUFPOST)
      ;
    while (this.m_input_scan_number <= this.m_output_scan_number && !this.m_inputctl.EOIReached())
    {
      if (this.m_inputctl.consume_input() == ReadResult.JPEG_SUSPENDED)
        return false;
    }
    this.m_global_state = CommonStruct.JpegState.DSTATE_BUFIMAGE;
    return true;
  }

  public bool jpeg_input_complete() => this.m_inputctl.EOIReached();

  public ReadResult jpeg_consume_input()
  {
    ReadResult readResult = ReadResult.JPEG_SUSPENDED;
    switch (this.m_global_state)
    {
      case CommonStruct.JpegState.DSTATE_START:
        this.jpeg_consume_input_start();
        readResult = this.jpeg_consume_input_inHeader();
        break;
      case CommonStruct.JpegState.DSTATE_INHEADER:
        readResult = this.jpeg_consume_input_inHeader();
        break;
      case CommonStruct.JpegState.DSTATE_READY:
        readResult = ReadResult.JPEG_REACHED_SOS;
        break;
      case CommonStruct.JpegState.DSTATE_PRELOAD:
      case CommonStruct.JpegState.DSTATE_PRESCAN:
      case CommonStruct.JpegState.DSTATE_SCANNING:
      case CommonStruct.JpegState.DSTATE_RAW_OK:
      case CommonStruct.JpegState.DSTATE_BUFIMAGE:
      case CommonStruct.JpegState.DSTATE_BUFPOST:
      case CommonStruct.JpegState.DSTATE_STOPPING:
        readResult = this.m_inputctl.consume_input();
        break;
    }
    return readResult;
  }

  public void jpeg_calc_output_dimensions()
  {
    if (this.m_scale_num * 8 <= this.m_scale_denom)
    {
      this.m_output_width = JpegUtils.jdiv_round_up(this.m_image_width, 8);
      this.m_output_height = JpegUtils.jdiv_round_up(this.m_image_height, 8);
      this.m_min_DCT_scaled_size = 1;
    }
    else if (this.m_scale_num * 4 <= this.m_scale_denom)
    {
      this.m_output_width = JpegUtils.jdiv_round_up(this.m_image_width, 4);
      this.m_output_height = JpegUtils.jdiv_round_up(this.m_image_height, 4);
      this.m_min_DCT_scaled_size = 2;
    }
    else if (this.m_scale_num * 2 <= this.m_scale_denom)
    {
      this.m_output_width = JpegUtils.jdiv_round_up(this.m_image_width, 2);
      this.m_output_height = JpegUtils.jdiv_round_up(this.m_image_height, 2);
      this.m_min_DCT_scaled_size = 4;
    }
    else
    {
      this.m_output_width = this.m_image_width;
      this.m_output_height = this.m_image_height;
      this.m_min_DCT_scaled_size = 8;
    }
    for (int index = 0; index < this.m_num_components; ++index)
    {
      int minDctScaledSize = this.m_min_DCT_scaled_size;
      while (minDctScaledSize < 8 && this.m_comp_info[index].H_samp_factor * minDctScaledSize * 2 <= this.m_max_h_samp_factor * this.m_min_DCT_scaled_size && this.m_comp_info[index].V_samp_factor * minDctScaledSize * 2 <= this.m_max_v_samp_factor * this.m_min_DCT_scaled_size)
        minDctScaledSize *= 2;
      this.m_comp_info[index].DCT_scaled_size = minDctScaledSize;
    }
    for (int index = 0; index < this.m_num_components; ++index)
    {
      this.m_comp_info[index].downsampled_width = JpegUtils.jdiv_round_up(this.m_image_width * this.m_comp_info[index].H_samp_factor * this.m_comp_info[index].DCT_scaled_size, this.m_max_h_samp_factor * 8);
      this.m_comp_info[index].downsampled_height = JpegUtils.jdiv_round_up(this.m_image_height * this.m_comp_info[index].V_samp_factor * this.m_comp_info[index].DCT_scaled_size, this.m_max_v_samp_factor * 8);
    }
    switch (this.m_out_color_space)
    {
      case J_COLOR_SPACE.JCS_GRAYSCALE:
        this.m_out_color_components = 1;
        break;
      case J_COLOR_SPACE.JCS_RGB:
      case J_COLOR_SPACE.JCS_YCbCr:
        this.m_out_color_components = 3;
        break;
      case J_COLOR_SPACE.JCS_CMYK:
      case J_COLOR_SPACE.JCS_YCCK:
        this.m_out_color_components = 4;
        break;
      default:
        this.m_out_color_components = this.m_num_components;
        break;
    }
    this.m_output_components = this.m_quantize_colors ? 1 : this.m_out_color_components;
    if (this.use_merged_upsample())
      this.m_rec_outbuf_height = this.m_max_v_samp_factor;
    else
      this.m_rec_outbuf_height = 1;
  }

  public jvirt_array<JBLOCK>[] jpeg_read_coefficients()
  {
    if (this.m_global_state == CommonStruct.JpegState.DSTATE_READY)
    {
      this.transdecode_master_selection();
      this.m_global_state = CommonStruct.JpegState.DSTATE_RDCOEFS;
    }
    if (this.m_global_state == CommonStruct.JpegState.DSTATE_RDCOEFS)
    {
      while (true)
      {
        do
        {
          ReadResult readResult;
          do
          {
            if (this.m_progress != null)
              this.m_progress.Updated();
            readResult = this.m_inputctl.consume_input();
            switch (readResult)
            {
              case ReadResult.JPEG_SUSPENDED:
                return (jvirt_array<JBLOCK>[]) null;
              case ReadResult.JPEG_REACHED_EOI:
                goto label_10;
              default:
                continue;
            }
          }
          while (this.m_progress == null || readResult != ReadResult.JPEG_ROW_COMPLETED && readResult != ReadResult.JPEG_REACHED_SOS);
          ++this.m_progress.Pass_counter;
        }
        while (this.m_progress.Pass_counter < this.m_progress.Pass_limit);
        this.m_progress.Pass_limit += this.m_total_iMCU_rows;
      }
label_10:
      this.m_global_state = CommonStruct.JpegState.DSTATE_STOPPING;
    }
    return (this.m_global_state == CommonStruct.JpegState.DSTATE_STOPPING || this.m_global_state == CommonStruct.JpegState.DSTATE_BUFIMAGE) && this.m_buffered_image ? this.m_coef.GetCoefArrays() : (jvirt_array<JBLOCK>[]) null;
  }

  public void jpeg_abort_decompress() => this.jpeg_abort();

  public void jpeg_set_marker_processor(
    int marker_code,
    DecompressStruct.jpeg_marker_parser_method routine)
  {
    this.m_marker.jpeg_set_marker_processor(marker_code, routine);
  }

  public void jpeg_save_markers(int marker_code, int length_limit)
  {
    this.m_marker.jpeg_save_markers(marker_code, length_limit);
  }

  internal bool use_merged_upsample()
  {
    return !this.m_do_fancy_upsampling && !this.m_CCIR601_sampling && this.m_jpeg_color_space == J_COLOR_SPACE.JCS_YCbCr && this.m_num_components == 3 && this.m_out_color_space == J_COLOR_SPACE.JCS_RGB && this.m_out_color_components == 3 && this.m_comp_info[0].H_samp_factor == 2 && this.m_comp_info[1].H_samp_factor == 1 && this.m_comp_info[2].H_samp_factor == 1 && this.m_comp_info[0].V_samp_factor <= 2 && this.m_comp_info[1].V_samp_factor == 1 && this.m_comp_info[2].V_samp_factor == 1 && this.m_comp_info[0].DCT_scaled_size == this.m_min_DCT_scaled_size && this.m_comp_info[1].DCT_scaled_size == this.m_min_DCT_scaled_size && this.m_comp_info[2].DCT_scaled_size == this.m_min_DCT_scaled_size;
  }

  private void initialize()
  {
    this.m_progress = (ProgressMgr) null;
    this.m_src = (SourceMgr) null;
    for (int index = 0; index < 4; ++index)
      this.m_quant_tbl_ptrs[index] = (JQUANT_TBL) null;
    for (int index = 0; index < 4; ++index)
    {
      this.m_dc_huff_tbl_ptrs[index] = (JHUFF_TBL) null;
      this.m_ac_huff_tbl_ptrs[index] = (JHUFF_TBL) null;
    }
    this.m_marker_list = new List<MarkerStruct>();
    this.m_marker = new MarkerReader(this);
    this.m_inputctl = new InputController(this);
    this.m_global_state = CommonStruct.JpegState.DSTATE_START;
  }

  private void transdecode_master_selection()
  {
    this.m_buffered_image = true;
    this.m_entropy = !this.m_progressive_mode ? (Syncfusion.Pdf.Compression.JBIG2.Internal.EntropyDecoder) new HuffEntropyDecoder(this) : (Syncfusion.Pdf.Compression.JBIG2.Internal.EntropyDecoder) new PHuffEntropyDecoder(this);
    this.m_coef = new DCoefController(this, true);
    this.m_inputctl.start_input_pass();
    if (this.m_progress == null)
      return;
    int num = 1;
    if (this.m_progressive_mode)
      num = 2 + 3 * this.m_num_components;
    else if (this.m_inputctl.HasMultipleScans())
      num = this.m_num_components;
    this.m_progress.Pass_counter = 0;
    this.m_progress.Pass_limit = this.m_total_iMCU_rows * num;
    this.m_progress.Completed_passes = 0;
    this.m_progress.Total_passes = 1;
  }

  private bool output_pass_setup()
  {
    if (this.m_global_state != CommonStruct.JpegState.DSTATE_PRESCAN)
    {
      this.m_master.prepare_for_output_pass();
      this.m_output_scanline = 0;
      this.m_global_state = CommonStruct.JpegState.DSTATE_PRESCAN;
    }
    while (this.m_master.IsDummyPass())
    {
      while (this.m_output_scanline < this.m_output_height)
      {
        if (this.m_progress != null)
        {
          this.m_progress.Pass_counter = this.m_output_scanline;
          this.m_progress.Pass_limit = this.m_output_height;
          this.m_progress.Updated();
        }
        int outputScanline = this.m_output_scanline;
        this.m_main.process_data((byte[][]) null, ref this.m_output_scanline, 0);
        if (this.m_output_scanline == outputScanline)
          return false;
      }
      this.m_master.finish_output_pass();
      this.m_master.prepare_for_output_pass();
      this.m_output_scanline = 0;
    }
    this.m_global_state = this.m_raw_data_out ? CommonStruct.JpegState.DSTATE_RAW_OK : CommonStruct.JpegState.DSTATE_SCANNING;
    return true;
  }

  private void default_decompress_parms()
  {
    switch (this.m_num_components)
    {
      case 1:
        this.m_jpeg_color_space = J_COLOR_SPACE.JCS_GRAYSCALE;
        this.m_out_color_space = J_COLOR_SPACE.JCS_GRAYSCALE;
        break;
      case 3:
        if (this.m_saw_JFIF_marker)
          this.m_jpeg_color_space = J_COLOR_SPACE.JCS_YCbCr;
        else if (this.m_saw_Adobe_marker)
        {
          switch (this.m_Adobe_transform)
          {
            case 0:
              this.m_jpeg_color_space = J_COLOR_SPACE.JCS_RGB;
              break;
            case 1:
              this.m_jpeg_color_space = J_COLOR_SPACE.JCS_YCbCr;
              break;
            default:
              this.m_jpeg_color_space = J_COLOR_SPACE.JCS_YCbCr;
              break;
          }
        }
        else
        {
          int componentId1 = this.m_comp_info[0].Component_id;
          int componentId2 = this.m_comp_info[1].Component_id;
          int componentId3 = this.m_comp_info[2].Component_id;
          this.m_jpeg_color_space = componentId1 != 1 || componentId2 != 2 || componentId3 != 3 ? (componentId1 != 82 || componentId2 != 71 || componentId3 != 66 ? J_COLOR_SPACE.JCS_YCbCr : J_COLOR_SPACE.JCS_RGB) : J_COLOR_SPACE.JCS_YCbCr;
        }
        this.m_out_color_space = J_COLOR_SPACE.JCS_RGB;
        break;
      case 4:
        if (this.m_saw_Adobe_marker)
        {
          switch (this.m_Adobe_transform)
          {
            case 0:
              this.m_jpeg_color_space = J_COLOR_SPACE.JCS_CMYK;
              break;
            case 2:
              this.m_jpeg_color_space = J_COLOR_SPACE.JCS_YCCK;
              break;
            default:
              this.m_jpeg_color_space = J_COLOR_SPACE.JCS_YCCK;
              break;
          }
        }
        else
          this.m_jpeg_color_space = J_COLOR_SPACE.JCS_CMYK;
        this.m_out_color_space = J_COLOR_SPACE.JCS_CMYK;
        break;
      default:
        this.m_jpeg_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
        this.m_out_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
        break;
    }
    this.m_scale_num = 1;
    this.m_scale_denom = 1;
    this.m_buffered_image = false;
    this.m_raw_data_out = false;
    this.m_dct_method = J_DCT_METHOD.JDCT_ISLOW;
    this.m_do_fancy_upsampling = true;
    this.m_do_block_smoothing = true;
    this.m_quantize_colors = false;
    this.m_dither_mode = J_DITHER_MODE.JDITHER_FS;
    this.m_two_pass_quantize = true;
    this.m_desired_number_of_colors = 256 /*0x0100*/;
    this.m_colormap = (byte[][]) null;
    this.m_enable_1pass_quant = false;
    this.m_enable_external_quant = false;
    this.m_enable_2pass_quant = false;
  }

  private void jpeg_consume_input_start()
  {
    this.m_inputctl.reset_input_controller();
    this.m_src.init_source();
    this.m_global_state = CommonStruct.JpegState.DSTATE_INHEADER;
  }

  private ReadResult jpeg_consume_input_inHeader()
  {
    ReadResult readResult = this.m_inputctl.consume_input();
    if (readResult == ReadResult.JPEG_REACHED_SOS)
    {
      this.default_decompress_parms();
      this.m_global_state = CommonStruct.JpegState.DSTATE_READY;
    }
    return readResult;
  }

  public delegate bool jpeg_marker_parser_method(DecompressStruct cinfo);
}
