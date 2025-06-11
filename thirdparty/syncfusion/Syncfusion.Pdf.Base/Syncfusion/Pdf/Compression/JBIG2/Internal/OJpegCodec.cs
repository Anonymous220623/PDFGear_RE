// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.OJpegCodec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class OJpegCodec : TiffCodec
{
  internal const int FIELD_OJPEG_JPEGINTERCHANGEFORMAT = 66;
  internal const int FIELD_OJPEG_JPEGINTERCHANGEFORMATLENGTH = 67;
  internal const int FIELD_OJPEG_JPEGQTABLES = 68;
  internal const int FIELD_OJPEG_JPEGDCTABLES = 69;
  internal const int FIELD_OJPEG_JPEGACTABLES = 70;
  internal const int FIELD_OJPEG_JPEGPROC = 71;
  internal const int FIELD_OJPEG_JPEGRESTARTINTERVAL = 72;
  internal const int FIELD_OJPEG_COUNT = 7;
  private const int OJPEG_BUFFER = 2048 /*0x0800*/;
  private static readonly TiffFieldInfo[] ojpeg_field_info = new TiffFieldInfo[7]
  {
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGIFOFFSET, (short) 1, (short) 1, Syncfusion.Pdf.Compression.JBIG2.TiffType.LONG, (short) 66, true, false, "JpegInterchangeFormat"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGIFBYTECOUNT, (short) 1, (short) 1, Syncfusion.Pdf.Compression.JBIG2.TiffType.LONG, (short) 67, true, false, "JpegInterchangeFormatLength"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGQTABLES, (short) -1, (short) -1, Syncfusion.Pdf.Compression.JBIG2.TiffType.LONG, (short) 68, false, true, "JpegQTables"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGDCTABLES, (short) -1, (short) -1, Syncfusion.Pdf.Compression.JBIG2.TiffType.LONG, (short) 69, false, true, "JpegDcTables"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGACTABLES, (short) -1, (short) -1, Syncfusion.Pdf.Compression.JBIG2.TiffType.LONG, (short) 70, false, true, "JpegAcTables"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGPROC, (short) 1, (short) 1, Syncfusion.Pdf.Compression.JBIG2.TiffType.SHORT, (short) 71, false, false, "JpegProc"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGRESTARTINTERVAL, (short) 1, (short) 1, Syncfusion.Pdf.Compression.JBIG2.TiffType.SHORT, (short) 72, false, false, "JpegRestartInterval")
  };
  internal uint m_jpeg_interchange_format;
  internal uint m_jpeg_interchange_format_length;
  internal byte m_jpeg_proc;
  internal bool m_subsamplingcorrect_done;
  internal bool m_subsampling_tag;
  internal byte m_subsampling_hor;
  internal byte m_subsampling_ver;
  internal byte m_qtable_offset_count;
  internal byte m_dctable_offset_count;
  internal byte m_actable_offset_count;
  internal uint[] m_qtable_offset = new uint[3];
  internal uint[] m_dctable_offset = new uint[3];
  internal uint[] m_actable_offset = new uint[3];
  internal ushort m_restart_interval;
  internal DecompressStruct m_libjpeg_jpeg_decompress_struct;
  private TiffTagMethods m_tagMethods;
  private TiffTagMethods m_parentTagMethods;
  private uint m_file_size;
  private uint m_image_width;
  private uint m_image_length;
  private uint m_strile_width;
  private uint m_strile_length;
  private uint m_strile_length_total;
  private byte m_samples_per_pixel;
  private byte m_plane_sample_offset;
  private byte m_samples_per_pixel_per_plane;
  private bool m_subsamplingcorrect;
  private bool m_subsampling_force_desubsampling_inside_decompression;
  private byte[][] m_qtable = new byte[4][];
  private byte[][] m_dctable = new byte[4][];
  private byte[][] m_actable = new byte[4][];
  private byte m_restart_index;
  private bool m_sof_log;
  private byte m_sof_marker_id;
  private uint m_sof_x;
  private uint m_sof_y;
  private byte[] m_sof_c = new byte[3];
  private byte[] m_sof_hv = new byte[3];
  private byte[] m_sof_tq = new byte[3];
  private byte[] m_sos_cs = new byte[3];
  private byte[] m_sos_tda = new byte[3];
  private OJpegCodec.SosEnd[] m_sos_end = new OJpegCodec.SosEnd[3];
  private bool m_readheader_done;
  private bool m_writeheader_done;
  private short m_write_cursample;
  private uint m_write_curstrile;
  private bool m_libjpeg_session_active;
  private byte m_libjpeg_jpeg_query_style;
  private SourceMgr m_libjpeg_jpeg_source_mgr;
  private bool m_subsampling_convert_log;
  private uint m_subsampling_convert_ylinelen;
  private uint m_subsampling_convert_ylines;
  private uint m_subsampling_convert_clinelen;
  private uint m_subsampling_convert_clines;
  private byte[][] m_subsampling_convert_ybuf;
  private byte[][] m_subsampling_convert_cbbuf;
  private byte[][] m_subsampling_convert_crbuf;
  private byte[][][] m_subsampling_convert_ycbcrimage;
  private uint m_subsampling_convert_clinelenout;
  private uint m_subsampling_convert_state;
  private uint m_bytes_per_line;
  private uint m_lines_per_strile;
  private OJpegCodec.OJPEGStateInBufferSource m_in_buffer_source;
  private uint m_in_buffer_next_strile;
  private uint m_in_buffer_strile_count;
  private uint m_in_buffer_file_pos;
  private bool m_in_buffer_file_pos_log;
  private uint m_in_buffer_file_togo;
  private ushort m_in_buffer_togo;
  private int m_in_buffer_cur;
  private byte[] m_in_buffer = new byte[2048 /*0x0800*/];
  private OJpegCodec.OJPEGStateOutState m_out_state;
  private byte[] m_out_buffer = new byte[2048 /*0x0800*/];
  private byte[] m_skip_buffer;
  private bool m_forceProcessedRgbOutput;

  public OJpegCodec(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.Compression scheme, string name)
    : base(tif, scheme, name)
  {
    this.m_tagMethods = (TiffTagMethods) new OJpegCodecTagMethods();
  }

  private void cleanState()
  {
    this.m_jpeg_interchange_format = 0U;
    this.m_jpeg_interchange_format_length = 0U;
    this.m_jpeg_proc = (byte) 0;
    this.m_subsamplingcorrect_done = false;
    this.m_subsampling_tag = false;
    this.m_subsampling_hor = (byte) 0;
    this.m_subsampling_ver = (byte) 0;
    this.m_qtable_offset_count = (byte) 0;
    this.m_dctable_offset_count = (byte) 0;
    this.m_actable_offset_count = (byte) 0;
    this.m_qtable_offset = new uint[3];
    this.m_dctable_offset = new uint[3];
    this.m_actable_offset = new uint[3];
    this.m_restart_interval = (ushort) 0;
    this.m_libjpeg_jpeg_decompress_struct = (DecompressStruct) null;
    this.m_file_size = 0U;
    this.m_image_width = 0U;
    this.m_image_length = 0U;
    this.m_strile_width = 0U;
    this.m_strile_length = 0U;
    this.m_strile_length_total = 0U;
    this.m_samples_per_pixel = (byte) 0;
    this.m_plane_sample_offset = (byte) 0;
    this.m_samples_per_pixel_per_plane = (byte) 0;
    this.m_subsamplingcorrect = false;
    this.m_subsampling_force_desubsampling_inside_decompression = false;
    this.m_qtable = new byte[4][];
    this.m_dctable = new byte[4][];
    this.m_actable = new byte[4][];
    this.m_restart_index = (byte) 0;
    this.m_sof_log = false;
    this.m_sof_marker_id = (byte) 0;
    this.m_sof_x = 0U;
    this.m_sof_y = 0U;
    this.m_sof_c = new byte[3];
    this.m_sof_hv = new byte[3];
    this.m_sof_tq = new byte[3];
    this.m_sos_cs = new byte[3];
    this.m_sos_tda = new byte[3];
    this.m_sos_end = new OJpegCodec.SosEnd[3];
    this.m_readheader_done = false;
    this.m_writeheader_done = false;
    this.m_write_cursample = (short) 0;
    this.m_write_curstrile = 0U;
    this.m_libjpeg_session_active = false;
    this.m_libjpeg_jpeg_query_style = (byte) 0;
    this.m_libjpeg_jpeg_source_mgr = (SourceMgr) null;
    this.m_subsampling_convert_log = false;
    this.m_subsampling_convert_ylinelen = 0U;
    this.m_subsampling_convert_ylines = 0U;
    this.m_subsampling_convert_clinelen = 0U;
    this.m_subsampling_convert_clines = 0U;
    this.m_subsampling_convert_ybuf = (byte[][]) null;
    this.m_subsampling_convert_cbbuf = (byte[][]) null;
    this.m_subsampling_convert_crbuf = (byte[][]) null;
    this.m_subsampling_convert_ycbcrimage = (byte[][][]) null;
    this.m_subsampling_convert_clinelenout = 0U;
    this.m_subsampling_convert_state = 0U;
    this.m_bytes_per_line = 0U;
    this.m_lines_per_strile = 0U;
    this.m_in_buffer_source = OJpegCodec.OJPEGStateInBufferSource.osibsNotSetYet;
    this.m_in_buffer_next_strile = 0U;
    this.m_in_buffer_strile_count = 0U;
    this.m_in_buffer_file_pos = 0U;
    this.m_in_buffer_file_pos_log = false;
    this.m_in_buffer_file_togo = 0U;
    this.m_in_buffer_togo = (ushort) 0;
    this.m_in_buffer_cur = 0;
    this.m_in_buffer = new byte[2048 /*0x0800*/];
    this.m_out_state = OJpegCodec.OJPEGStateOutState.ososSoi;
    this.m_out_buffer = new byte[2048 /*0x0800*/];
    this.m_skip_buffer = (byte[]) null;
    this.m_forceProcessedRgbOutput = false;
  }

  public override bool Init()
  {
    this.m_tif.MergeFieldInfo(OJpegCodec.ojpeg_field_info, OJpegCodec.ojpeg_field_info.Length);
    this.cleanState();
    this.m_jpeg_proc = (byte) 1;
    this.m_subsampling_hor = (byte) 2;
    this.m_subsampling_ver = (byte) 2;
    this.m_tif.SetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.YCBCRSUBSAMPLING, (object) 2, (object) 2);
    this.m_parentTagMethods = this.m_tif.m_tagmethods;
    this.m_tif.m_tagmethods = this.m_tagMethods;
    this.m_tif.m_flags |= TiffFlags.NOREADRAW;
    return true;
  }

  public override bool CanDecode => true;

  public Tiff GetTiff() => this.m_tif;

  public override bool SetupDecode() => this.OJPEGSetupDecode();

  public override bool PreDecode(short plane) => this.OJPEGPreDecode(plane);

  public override bool DecodeRow(byte[] buffer, int offset, int count, short plane)
  {
    return this.OJPEGDecode(buffer, offset, count, plane);
  }

  public override bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
  {
    return this.OJPEGDecode(buffer, offset, count, plane);
  }

  public override bool DecodeTile(byte[] buffer, int offset, int count, short plane)
  {
    return this.OJPEGDecode(buffer, offset, count, plane);
  }

  public override void Cleanup() => this.OJPEGCleanup();

  private bool OJPEGSetupDecode() => true;

  private bool OJPEGPreDecode(short s)
  {
    if (!this.m_subsamplingcorrect_done)
      this.OJPEGSubsamplingCorrect();
    if (!this.m_readheader_done && !this.OJPEGReadHeaderInfo() || !this.m_sos_end[(int) s].m_log && !this.OJPEGReadSecondarySos(s))
      return false;
    uint num = !this.m_tif.IsTiled() ? (uint) this.m_tif.m_curstrip : (uint) this.m_tif.m_curtile;
    if (this.m_writeheader_done && ((int) this.m_write_cursample != (int) s || this.m_write_curstrile > num))
    {
      if (this.m_libjpeg_session_active)
        this.OJPEGLibjpegSessionAbort();
      this.m_writeheader_done = false;
    }
    if (!this.m_writeheader_done)
    {
      this.m_plane_sample_offset = (byte) s;
      this.m_write_cursample = s;
      this.m_write_curstrile = (uint) s * (uint) this.m_tif.m_dir.td_stripsperimage;
      if (!this.m_in_buffer_file_pos_log || (int) this.m_in_buffer_file_pos - (int) this.m_in_buffer_togo != (int) this.m_sos_end[(int) s].m_in_buffer_file_pos)
      {
        this.m_in_buffer_source = this.m_sos_end[(int) s].m_in_buffer_source;
        this.m_in_buffer_next_strile = this.m_sos_end[(int) s].m_in_buffer_next_strile;
        this.m_in_buffer_file_pos = this.m_sos_end[(int) s].m_in_buffer_file_pos;
        this.m_in_buffer_file_pos_log = false;
        this.m_in_buffer_file_togo = this.m_sos_end[(int) s].m_in_buffer_file_togo;
        this.m_in_buffer_togo = (ushort) 0;
        this.m_in_buffer_cur = 0;
      }
      if (!this.OJPEGWriteHeaderInfo())
        return false;
    }
    for (; this.m_write_curstrile < num; ++this.m_write_curstrile)
    {
      if (this.m_libjpeg_jpeg_query_style == (byte) 0)
      {
        if (!this.OJPEGPreDecodeSkipRaw())
          return false;
      }
      else if (!this.OJPEGPreDecodeSkipScanlines())
        return false;
    }
    return true;
  }

  private bool OJPEGDecode(byte[] buf, int offset, int cc, short s)
  {
    if (this.m_libjpeg_jpeg_query_style == (byte) 0)
    {
      if (!this.OJPEGDecodeRaw(buf, offset, cc))
        return false;
    }
    else if (!this.OJPEGDecodeScanlines(buf, offset, cc))
      return false;
    return true;
  }

  private void OJPEGCleanup()
  {
    this.m_tif.m_tagmethods = this.m_parentTagMethods;
    if (!this.m_libjpeg_session_active)
      return;
    this.OJPEGLibjpegSessionAbort();
  }

  private bool OJPEGPreDecodeSkipRaw()
  {
    uint linesPerStrile = this.m_lines_per_strile;
    if (this.m_subsampling_convert_state != 0U)
    {
      if (this.m_subsampling_convert_clines - this.m_subsampling_convert_state >= linesPerStrile)
      {
        this.m_subsampling_convert_state += linesPerStrile;
        if ((int) this.m_subsampling_convert_state == (int) this.m_subsampling_convert_clines)
          this.m_subsampling_convert_state = 0U;
        return true;
      }
      linesPerStrile -= this.m_subsampling_convert_clines - this.m_subsampling_convert_state;
      this.m_subsampling_convert_state = 0U;
    }
    for (; linesPerStrile >= this.m_subsampling_convert_clines; linesPerStrile -= this.m_subsampling_convert_clines)
    {
      if (this.jpeg_read_raw_data_encap((int) this.m_subsampling_ver * 8) == 0)
        return false;
    }
    if (linesPerStrile > 0U)
    {
      if (this.jpeg_read_raw_data_encap((int) this.m_subsampling_ver * 8) == 0)
        return false;
      this.m_subsampling_convert_state = linesPerStrile;
    }
    return true;
  }

  private bool OJPEGPreDecodeSkipScanlines()
  {
    if (this.m_skip_buffer == null)
      this.m_skip_buffer = new byte[(IntPtr) this.m_bytes_per_line];
    for (uint index = 0; index < this.m_lines_per_strile; ++index)
    {
      if (this.jpeg_read_scanlines_encap(this.m_skip_buffer, 1) == 0)
        return false;
    }
    return true;
  }

  private bool OJPEGDecodeRaw(byte[] buf, int offset, int cc)
  {
    if ((long) cc % (long) this.m_bytes_per_line != 0L)
      return false;
    int num1 = offset;
    int num2 = cc;
    while (this.m_subsampling_convert_state != 0U || this.jpeg_read_raw_data_encap((int) this.m_subsampling_ver * 8) != 0)
    {
      uint num3 = this.m_subsampling_convert_state * (uint) this.m_subsampling_ver * this.m_subsampling_convert_ylinelen;
      uint num4 = this.m_subsampling_convert_state * this.m_subsampling_convert_clinelen;
      uint num5 = this.m_subsampling_convert_state * this.m_subsampling_convert_clinelen;
      int num6 = num1;
      for (uint index1 = 0; index1 < this.m_subsampling_convert_clinelenout; ++index1)
      {
        uint num7 = num3;
        for (byte index2 = 0; (int) index2 < (int) this.m_subsampling_ver; ++index2)
        {
          for (byte index3 = 0; (int) index3 < (int) this.m_subsampling_hor; ++index3)
          {
            int index4 = (int) (num7 / this.m_subsampling_convert_ylinelen);
            int index5 = (int) (num7 % this.m_subsampling_convert_ylinelen);
            ++num7;
            buf[num6++] = this.m_subsampling_convert_ybuf[index4][index5];
          }
          num7 += this.m_subsampling_convert_ylinelen - (uint) this.m_subsampling_hor;
        }
        num3 += (uint) this.m_subsampling_hor;
        int index6 = (int) (num4 / this.m_subsampling_convert_clinelen);
        int index7 = (int) (num4 % this.m_subsampling_convert_clinelen);
        ++num4;
        byte[] numArray1 = buf;
        int index8 = num6;
        int num8 = index8 + 1;
        int num9 = (int) this.m_subsampling_convert_cbbuf[index6][index7];
        numArray1[index8] = (byte) num9;
        int index9 = (int) (num5 / this.m_subsampling_convert_clinelen);
        int index10 = (int) (num5 % this.m_subsampling_convert_clinelen);
        ++num5;
        byte[] numArray2 = buf;
        int index11 = num8;
        num6 = index11 + 1;
        int num10 = (int) this.m_subsampling_convert_crbuf[index9][index10];
        numArray2[index11] = (byte) num10;
      }
      ++this.m_subsampling_convert_state;
      if ((int) this.m_subsampling_convert_state == (int) this.m_subsampling_convert_clines)
        this.m_subsampling_convert_state = 0U;
      num1 += (int) this.m_bytes_per_line;
      num2 -= (int) this.m_bytes_per_line;
      if (num2 <= 0)
        return true;
    }
    return false;
  }

  private bool OJPEGDecodeScanlines(byte[] buf, int offset, int cc)
  {
    if ((long) cc % (long) this.m_bytes_per_line != 0L)
      return false;
    int dstOffset = offset;
    byte[] numArray = new byte[(IntPtr) this.m_bytes_per_line];
    int num = cc;
    while (this.jpeg_read_scanlines_encap(numArray, 1) != 0)
    {
      Buffer.BlockCopy((Array) numArray, 0, (Array) buf, dstOffset, numArray.Length);
      dstOffset += (int) this.m_bytes_per_line;
      num -= (int) this.m_bytes_per_line;
      if (num <= 0)
        return true;
    }
    return false;
  }

  public void OJPEGSubsamplingCorrect()
  {
    if (this.m_tif.m_dir.td_samplesperpixel != (short) 3 || this.m_tif.m_dir.td_photometric != Photometric.YCBCR && this.m_tif.m_dir.td_photometric != Photometric.ITULAB)
    {
      int num = this.m_subsampling_tag ? 1 : 0;
      this.m_subsampling_hor = (byte) 1;
      this.m_subsampling_ver = (byte) 1;
      this.m_subsampling_force_desubsampling_inside_decompression = false;
    }
    else
    {
      this.m_subsamplingcorrect_done = true;
      this.m_subsamplingcorrect = true;
      this.OJPEGReadHeaderInfoSec();
      if (this.m_subsampling_force_desubsampling_inside_decompression)
      {
        this.m_subsampling_hor = (byte) 1;
        this.m_subsampling_ver = (byte) 1;
      }
      this.m_subsamplingcorrect = false;
    }
    this.m_subsamplingcorrect_done = true;
  }

  private bool OJPEGReadHeaderInfo()
  {
    this.m_image_width = (uint) this.m_tif.m_dir.td_imagewidth;
    this.m_image_length = (uint) this.m_tif.m_dir.td_imagelength;
    if (this.m_tif.IsTiled())
    {
      this.m_strile_width = (uint) this.m_tif.m_dir.td_tilewidth;
      this.m_strile_length = (uint) this.m_tif.m_dir.td_tilelength;
      this.m_strile_length_total = (uint) ((int) this.m_image_length + (int) this.m_strile_length - 1) / this.m_strile_length * this.m_strile_length;
    }
    else
    {
      this.m_strile_width = this.m_image_width;
      this.m_strile_length = (uint) this.m_tif.m_dir.td_rowsperstrip;
      this.m_strile_length_total = this.m_image_length;
    }
    this.m_samples_per_pixel = (byte) this.m_tif.m_dir.td_samplesperpixel;
    if (this.m_samples_per_pixel == (byte) 1)
    {
      this.m_plane_sample_offset = (byte) 0;
      this.m_samples_per_pixel_per_plane = this.m_samples_per_pixel;
      this.m_subsampling_hor = (byte) 1;
      this.m_subsampling_ver = (byte) 1;
    }
    else
    {
      if (this.m_samples_per_pixel != (byte) 3)
        return false;
      this.m_plane_sample_offset = (byte) 0;
      this.m_samples_per_pixel_per_plane = this.m_tif.m_dir.td_planarconfig != PlanarConfig.CONTIG ? (byte) 1 : (byte) 3;
    }
    if (this.m_strile_length < this.m_image_length)
    {
      if ((long) this.m_strile_length % (long) ((int) this.m_subsampling_ver * 8) != 0L)
        return false;
      this.m_restart_interval = (ushort) (((long) this.m_strile_width + (long) ((int) this.m_subsampling_hor * 8) - 1L) / (long) ((int) this.m_subsampling_hor * 8) * ((long) this.m_strile_length / (long) ((int) this.m_subsampling_ver * 8)));
    }
    if (!this.OJPEGReadHeaderInfoSec())
      return false;
    this.m_sos_end[0].m_log = true;
    this.m_sos_end[0].m_in_buffer_source = this.m_in_buffer_source;
    this.m_sos_end[0].m_in_buffer_next_strile = this.m_in_buffer_next_strile;
    this.m_sos_end[0].m_in_buffer_file_pos = this.m_in_buffer_file_pos - (uint) this.m_in_buffer_togo;
    this.m_sos_end[0].m_in_buffer_file_togo = this.m_in_buffer_file_togo + (uint) this.m_in_buffer_togo;
    this.m_readheader_done = true;
    return true;
  }

  private bool OJPEGReadSecondarySos(short s)
  {
    this.m_plane_sample_offset = (byte) ((uint) s - 1U);
    while (!this.m_sos_end[(int) this.m_plane_sample_offset].m_log)
      --this.m_plane_sample_offset;
    this.m_in_buffer_source = this.m_sos_end[(int) this.m_plane_sample_offset].m_in_buffer_source;
    this.m_in_buffer_next_strile = this.m_sos_end[(int) this.m_plane_sample_offset].m_in_buffer_next_strile;
    this.m_in_buffer_file_pos = this.m_sos_end[(int) this.m_plane_sample_offset].m_in_buffer_file_pos;
    this.m_in_buffer_file_pos_log = false;
    this.m_in_buffer_file_togo = this.m_sos_end[(int) this.m_plane_sample_offset].m_in_buffer_file_togo;
    this.m_in_buffer_togo = (ushort) 0;
    this.m_in_buffer_cur = 0;
label_13:
    if ((int) this.m_plane_sample_offset >= (int) s)
      return true;
label_4:
    byte b;
    while (this.OJPEGReadByte(out b))
    {
      if (b == byte.MaxValue)
      {
        while (this.OJPEGReadByte(out b))
        {
          switch (b)
          {
            case 218:
              ++this.m_plane_sample_offset;
              if (!this.OJPEGReadHeaderInfoSecStreamSos())
                return false;
              this.m_sos_end[(int) this.m_plane_sample_offset].m_log = true;
              this.m_sos_end[(int) this.m_plane_sample_offset].m_in_buffer_source = this.m_in_buffer_source;
              this.m_sos_end[(int) this.m_plane_sample_offset].m_in_buffer_next_strile = this.m_in_buffer_next_strile;
              this.m_sos_end[(int) this.m_plane_sample_offset].m_in_buffer_file_pos = this.m_in_buffer_file_pos - (uint) this.m_in_buffer_togo;
              this.m_sos_end[(int) this.m_plane_sample_offset].m_in_buffer_file_togo = this.m_in_buffer_file_togo + (uint) this.m_in_buffer_togo;
              goto label_13;
            case byte.MaxValue:
              continue;
            default:
              goto label_4;
          }
        }
        return false;
      }
    }
    return false;
  }

  private bool OJPEGWriteHeaderInfo()
  {
    this.m_out_state = OJpegCodec.OJPEGStateOutState.ososSoi;
    this.m_restart_index = (byte) 0;
    if (!this.jpeg_create_decompress_encap())
      return false;
    this.m_libjpeg_session_active = true;
    this.m_libjpeg_jpeg_source_mgr = (SourceMgr) new OJpegSrcManager(this);
    this.m_libjpeg_jpeg_decompress_struct.Src = this.m_libjpeg_jpeg_source_mgr;
    if (this.jpeg_read_header_encap(true) == ReadResult.JPEG_SUSPENDED)
      return false;
    if (!this.m_subsampling_force_desubsampling_inside_decompression && this.m_samples_per_pixel_per_plane > (byte) 1)
    {
      this.m_libjpeg_jpeg_decompress_struct.Raw_data_out = true;
      this.m_libjpeg_jpeg_decompress_struct.Do_fancy_upsampling = false;
      this.m_libjpeg_jpeg_query_style = (byte) 0;
      if (!this.m_subsampling_convert_log)
      {
        this.m_subsampling_convert_ylinelen = (uint) ((ulong) ((long) this.m_strile_width + (long) ((int) this.m_subsampling_hor * 8) - 1L) / (ulong) ((int) this.m_subsampling_hor * 8) * (ulong) this.m_subsampling_hor * 8UL);
        this.m_subsampling_convert_ylines = (uint) this.m_subsampling_ver * 8U;
        this.m_subsampling_convert_clinelen = this.m_subsampling_convert_ylinelen / (uint) this.m_subsampling_hor;
        this.m_subsampling_convert_clines = 8U;
        this.m_subsampling_convert_ybuf = new byte[(IntPtr) this.m_subsampling_convert_ylines][];
        for (int index = 0; (long) index < (long) this.m_subsampling_convert_ylines; ++index)
          this.m_subsampling_convert_ybuf[index] = new byte[(IntPtr) this.m_subsampling_convert_ylinelen];
        this.m_subsampling_convert_cbbuf = new byte[(IntPtr) this.m_subsampling_convert_clines][];
        this.m_subsampling_convert_crbuf = new byte[(IntPtr) this.m_subsampling_convert_clines][];
        for (int index = 0; (long) index < (long) this.m_subsampling_convert_clines; ++index)
        {
          this.m_subsampling_convert_cbbuf[index] = new byte[(IntPtr) this.m_subsampling_convert_clinelen];
          this.m_subsampling_convert_crbuf[index] = new byte[(IntPtr) this.m_subsampling_convert_clinelen];
        }
        this.m_subsampling_convert_ycbcrimage = new byte[3][][];
        this.m_subsampling_convert_ycbcrimage[0] = new byte[(IntPtr) this.m_subsampling_convert_ylines][];
        for (uint index = 0; index < this.m_subsampling_convert_ylines; ++index)
          this.m_subsampling_convert_ycbcrimage[0][(IntPtr) index] = this.m_subsampling_convert_ybuf[(IntPtr) index];
        this.m_subsampling_convert_ycbcrimage[1] = new byte[(IntPtr) this.m_subsampling_convert_clines][];
        for (uint index = 0; index < this.m_subsampling_convert_clines; ++index)
          this.m_subsampling_convert_ycbcrimage[1][(IntPtr) index] = this.m_subsampling_convert_cbbuf[(IntPtr) index];
        this.m_subsampling_convert_ycbcrimage[2] = new byte[(IntPtr) this.m_subsampling_convert_clines][];
        for (uint index = 0; index < this.m_subsampling_convert_clines; ++index)
          this.m_subsampling_convert_ycbcrimage[2][(IntPtr) index] = this.m_subsampling_convert_crbuf[(IntPtr) index];
        this.m_subsampling_convert_clinelenout = (uint) ((int) this.m_strile_width + (int) this.m_subsampling_hor - 1) / (uint) this.m_subsampling_hor;
        this.m_subsampling_convert_state = 0U;
        this.m_bytes_per_line = (uint) ((ulong) this.m_subsampling_convert_clinelenout * (ulong) ((int) this.m_subsampling_ver * (int) this.m_subsampling_hor + 2));
        this.m_lines_per_strile = (uint) ((int) this.m_strile_length + (int) this.m_subsampling_ver - 1) / (uint) this.m_subsampling_ver;
        this.m_subsampling_convert_log = true;
      }
    }
    else
    {
      if (this.m_forceProcessedRgbOutput)
      {
        this.m_libjpeg_jpeg_decompress_struct.Do_fancy_upsampling = false;
        this.m_libjpeg_jpeg_decompress_struct.Jpeg_color_space = J_COLOR_SPACE.JCS_YCbCr;
        this.m_libjpeg_jpeg_decompress_struct.Out_color_space = J_COLOR_SPACE.JCS_RGB;
      }
      else
      {
        this.m_libjpeg_jpeg_decompress_struct.Jpeg_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
        this.m_libjpeg_jpeg_decompress_struct.Out_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
      }
      this.m_libjpeg_jpeg_query_style = (byte) 1;
      this.m_bytes_per_line = (uint) this.m_samples_per_pixel_per_plane * this.m_strile_width;
      this.m_lines_per_strile = this.m_strile_length;
    }
    if (!this.jpeg_start_decompress_encap())
      return false;
    this.m_writeheader_done = true;
    return true;
  }

  private void OJPEGLibjpegSessionAbort()
  {
    this.m_libjpeg_jpeg_decompress_struct.jpeg_destroy();
    this.m_libjpeg_session_active = false;
  }

  private bool OJPEGReadHeaderInfoSec()
  {
    if (this.m_file_size == 0U)
      this.m_file_size = (uint) this.m_tif.GetStream().Size(this.m_tif.m_clientdata);
    if (this.m_jpeg_interchange_format != 0U)
    {
      if (this.m_jpeg_interchange_format >= this.m_file_size)
      {
        this.m_jpeg_interchange_format = 0U;
        this.m_jpeg_interchange_format_length = 0U;
      }
      else if (this.m_jpeg_interchange_format_length == 0U || this.m_jpeg_interchange_format + this.m_jpeg_interchange_format_length > this.m_file_size)
        this.m_jpeg_interchange_format_length = this.m_file_size - this.m_jpeg_interchange_format;
    }
    this.m_in_buffer_source = OJpegCodec.OJPEGStateInBufferSource.osibsNotSetYet;
    this.m_in_buffer_next_strile = 0U;
    this.m_in_buffer_strile_count = (uint) this.m_tif.m_dir.td_nstrips;
    this.m_in_buffer_file_togo = 0U;
    this.m_in_buffer_togo = (ushort) 0;
label_8:
    byte b;
    if (!this.OJPEGReadBytePeek(out b))
      return false;
    if (b == byte.MaxValue)
    {
      this.OJPEGReadByteAdvance();
      while (this.OJPEGReadByte(out b))
      {
        if (b != byte.MaxValue)
        {
          switch ((JPEG_MARKER) b)
          {
            case JPEG_MARKER.SOF0:
            case JPEG_MARKER.SOF1:
            case JPEG_MARKER.SOF3:
              if (!this.OJPEGReadHeaderInfoSecStreamSof(b))
                return false;
              if (this.m_subsamplingcorrect)
                return true;
              goto case JPEG_MARKER.SOI;
            case JPEG_MARKER.DHT:
              if (!this.OJPEGReadHeaderInfoSecStreamDht())
                return false;
              goto case JPEG_MARKER.SOI;
            case JPEG_MARKER.SOI:
              if (b != (byte) 218)
                goto label_8;
              goto label_38;
            case JPEG_MARKER.SOS:
              if (this.m_subsamplingcorrect)
                return true;
              if (!this.OJPEGReadHeaderInfoSecStreamSos())
                return false;
              goto case JPEG_MARKER.SOI;
            case JPEG_MARKER.DQT:
              if (!this.OJPEGReadHeaderInfoSecStreamDqt())
                return false;
              goto case JPEG_MARKER.SOI;
            case JPEG_MARKER.DRI:
              if (!this.OJPEGReadHeaderInfoSecStreamDri())
                return false;
              goto case JPEG_MARKER.SOI;
            case JPEG_MARKER.APP0:
            case JPEG_MARKER.APP1:
            case JPEG_MARKER.APP2:
            case JPEG_MARKER.APP3:
            case JPEG_MARKER.APP4:
            case JPEG_MARKER.APP5:
            case JPEG_MARKER.APP6:
            case JPEG_MARKER.APP7:
            case JPEG_MARKER.APP8:
            case JPEG_MARKER.APP9:
            case JPEG_MARKER.APP10:
            case JPEG_MARKER.APP11:
            case JPEG_MARKER.APP12:
            case JPEG_MARKER.APP13:
            case JPEG_MARKER.APP14:
            case JPEG_MARKER.APP15:
            case JPEG_MARKER.COM:
              ushort word;
              if (!this.OJPEGReadWord(out word))
                return false;
              if (word < (ushort) 2)
              {
                int num = this.m_subsamplingcorrect ? 1 : 0;
                return false;
              }
              if (word > (ushort) 2)
              {
                this.OJPEGReadSkip((ushort) ((uint) word - 2U));
                goto case JPEG_MARKER.SOI;
              }
              goto case JPEG_MARKER.SOI;
            default:
              return false;
          }
        }
      }
      return false;
    }
label_38:
    if (this.m_subsamplingcorrect || this.m_sof_log)
      return true;
    if (!this.OJPEGReadHeaderInfoSecTablesQTable())
      return false;
    this.m_sof_marker_id = (byte) 192 /*0xC0*/;
    for (byte index = 0; (int) index < (int) this.m_samples_per_pixel; ++index)
      this.m_sof_c[(int) index] = index;
    this.m_sof_hv[0] = (byte) ((uint) this.m_subsampling_hor << 4 | (uint) this.m_subsampling_ver);
    for (byte index = 1; (int) index < (int) this.m_samples_per_pixel; ++index)
      this.m_sof_hv[(int) index] = (byte) 17;
    this.m_sof_x = this.m_strile_width;
    this.m_sof_y = this.m_strile_length_total;
    this.m_sof_log = true;
    if (!this.OJPEGReadHeaderInfoSecTablesDcTable() || !this.OJPEGReadHeaderInfoSecTablesAcTable())
      return false;
    for (byte index = 1; (int) index < (int) this.m_samples_per_pixel; ++index)
      this.m_sos_cs[(int) index] = index;
    return true;
  }

  private bool OJPEGReadHeaderInfoSecStreamDri()
  {
    ushort word;
    if (!this.OJPEGReadWord(out word) || word != (ushort) 4 || !this.OJPEGReadWord(out word))
      return false;
    this.m_restart_interval = word;
    return true;
  }

  private bool OJPEGReadHeaderInfoSecStreamDqt()
  {
    ushort word;
    if (!this.OJPEGReadWord(out word))
      return false;
    if (word <= (ushort) 2)
    {
      int num = this.m_subsamplingcorrect ? 1 : 0;
      return false;
    }
    if (this.m_subsamplingcorrect)
    {
      this.OJPEGReadSkip((ushort) ((uint) word - 2U));
    }
    else
    {
      ushort num = (ushort) ((uint) word - 2U);
      while (num >= (ushort) 65)
      {
        byte[] mem = new byte[new IntPtr(69)];
        mem[0] = byte.MaxValue;
        mem[1] = (byte) 219;
        mem[2] = (byte) 0;
        mem[3] = (byte) 67;
        if (!this.OJPEGReadBlock((ushort) 65, mem, 4))
          return false;
        byte index = (byte) ((uint) mem[4] & 15U);
        if ((byte) 3 < index)
          return false;
        this.m_qtable[(int) index] = mem;
        num -= (ushort) 65;
        if (num <= (ushort) 0)
          goto label_14;
      }
      return false;
    }
label_14:
    return true;
  }

  private bool OJPEGReadHeaderInfoSecStreamDht()
  {
    ushort word;
    if (!this.OJPEGReadWord(out word))
      return false;
    if (word <= (ushort) 2)
    {
      int num = this.m_subsamplingcorrect ? 1 : 0;
      return false;
    }
    if (this.m_subsamplingcorrect)
    {
      this.OJPEGReadSkip((ushort) ((uint) word - 2U));
    }
    else
    {
      byte[] mem = new byte[(IntPtr) (2U + (uint) word)];
      mem[0] = byte.MaxValue;
      mem[1] = (byte) 196;
      mem[2] = (byte) ((uint) word >> 8);
      mem[3] = (byte) ((uint) word & (uint) byte.MaxValue);
      if (!this.OJPEGReadBlock((ushort) ((uint) word - 2U), mem, 4))
        return false;
      byte index1 = mem[4];
      if (((int) index1 & 240 /*0xF0*/) == 0)
      {
        if ((byte) 3 < index1)
          return false;
        this.m_dctable[(int) index1] = mem;
      }
      else
      {
        if (((int) index1 & 240 /*0xF0*/) != 16 /*0x10*/)
          return false;
        byte index2 = (byte) ((uint) index1 & 15U);
        if ((byte) 3 < index2)
          return false;
        this.m_actable[(int) index2] = mem;
      }
    }
    return true;
  }

  private bool OJPEGReadHeaderInfoSecStreamSof(byte marker_id)
  {
    if (this.m_sof_log)
      return false;
    if (!this.m_subsamplingcorrect)
      this.m_sof_marker_id = marker_id;
    ushort word1;
    if (!this.OJPEGReadWord(out word1))
      return false;
    if (word1 < (ushort) 11)
    {
      int num = this.m_subsamplingcorrect ? 1 : 0;
      return false;
    }
    ushort num1 = (ushort) ((uint) word1 - 8U);
    if ((int) num1 % 3 != 0)
    {
      int num2 = this.m_subsamplingcorrect ? 1 : 0;
      return false;
    }
    ushort num3 = (ushort) ((uint) num1 / 3U);
    byte b;
    if (!this.m_subsamplingcorrect && (int) num3 != (int) this.m_samples_per_pixel || !this.OJPEGReadByte(out b) || b != (byte) 8 && !this.m_subsamplingcorrect)
      return false;
    if (this.m_subsamplingcorrect)
    {
      this.OJPEGReadSkip((ushort) 4);
    }
    else
    {
      ushort word2;
      if (!this.OJPEGReadWord(out word2) || (uint) word2 < this.m_image_length && (uint) word2 < this.m_strile_length_total)
        return false;
      this.m_sof_y = (uint) word2;
      if (!this.OJPEGReadWord(out word2) || (uint) word2 < this.m_image_width && (uint) word2 < this.m_strile_width)
        return false;
      this.m_sof_x = (uint) word2;
    }
    if (!this.OJPEGReadByte(out b))
      return false;
    if ((int) b != (int) num3)
    {
      int num4 = this.m_subsamplingcorrect ? 1 : 0;
      return false;
    }
    for (ushort index = 0; (int) index < (int) num3; ++index)
    {
      if (!this.OJPEGReadByte(out b))
        return false;
      if (!this.m_subsamplingcorrect)
        this.m_sof_c[(int) index] = b;
      if (!this.OJPEGReadByte(out b))
        return false;
      if (this.m_subsamplingcorrect)
      {
        if (index == (ushort) 0)
        {
          this.m_subsampling_hor = (byte) ((uint) b >> 4);
          this.m_subsampling_ver = (byte) ((uint) b & 15U);
          if (this.m_subsampling_hor != (byte) 1 && this.m_subsampling_hor != (byte) 2 && this.m_subsampling_hor != (byte) 4 || this.m_subsampling_ver != (byte) 1 && this.m_subsampling_ver != (byte) 2 && this.m_subsampling_ver != (byte) 4 || this.m_forceProcessedRgbOutput)
            this.m_subsampling_force_desubsampling_inside_decompression = true;
        }
        else if (b != (byte) 17)
          this.m_subsampling_force_desubsampling_inside_decompression = true;
      }
      else
      {
        this.m_sof_hv[(int) index] = b;
        if (!this.m_subsampling_force_desubsampling_inside_decompression)
        {
          if (index == (ushort) 0)
          {
            if ((int) b != ((int) this.m_subsampling_hor << 4 | (int) this.m_subsampling_ver))
              return false;
          }
          else if (b != (byte) 17)
            return false;
        }
      }
      if (!this.OJPEGReadByte(out b))
        return false;
      if (!this.m_subsamplingcorrect)
        this.m_sof_tq[(int) index] = b;
    }
    if (!this.m_subsamplingcorrect)
      this.m_sof_log = true;
    return true;
  }

  private bool OJPEGReadHeaderInfoSecStreamSos()
  {
    ushort word;
    byte b;
    if (!this.m_sof_log || !this.OJPEGReadWord(out word) || (int) word != 6 + (int) this.m_samples_per_pixel_per_plane * 2 || !this.OJPEGReadByte(out b) || (int) b != (int) this.m_samples_per_pixel_per_plane)
      return false;
    for (byte index = 0; (int) index < (int) this.m_samples_per_pixel_per_plane; ++index)
    {
      if (!this.OJPEGReadByte(out b))
        return false;
      this.m_sos_cs[(int) this.m_plane_sample_offset + (int) index] = b;
      if (!this.OJPEGReadByte(out b))
        return false;
      this.m_sos_tda[(int) this.m_plane_sample_offset + (int) index] = b;
    }
    this.OJPEGReadSkip((ushort) 3);
    return true;
  }

  private bool OJPEGReadHeaderInfoSecTablesQTable()
  {
    if (this.m_qtable_offset[0] == 0U)
      return false;
    this.m_in_buffer_file_pos_log = false;
    for (byte index1 = 0; (int) index1 < (int) this.m_samples_per_pixel; ++index1)
    {
      if (this.m_qtable_offset[(int) index1] != 0U && (index1 == (byte) 0 || (int) this.m_qtable_offset[(int) index1] != (int) this.m_qtable_offset[(int) index1 - 1]))
      {
        for (byte index2 = 0; (int) index2 < (int) index1 - 1; ++index2)
        {
          if ((int) this.m_qtable_offset[(int) index1] == (int) this.m_qtable_offset[(int) index2])
            return false;
        }
        byte[] buffer = new byte[new IntPtr(69)];
        buffer[0] = byte.MaxValue;
        buffer[1] = (byte) 219;
        buffer[2] = (byte) 0;
        buffer[3] = (byte) 67;
        buffer[4] = index1;
        TiffStream stream = this.m_tif.GetStream();
        stream.Seek(this.m_tif.m_clientdata, (long) this.m_qtable_offset[(int) index1], SeekOrigin.Begin);
        if (stream.Read(this.m_tif.m_clientdata, buffer, 5, 64 /*0x40*/) != 64 /*0x40*/)
          return false;
        this.m_qtable[(int) index1] = buffer;
        this.m_sof_tq[(int) index1] = index1;
      }
      else
        this.m_sof_tq[(int) index1] = this.m_sof_tq[(int) index1 - 1];
    }
    return true;
  }

  private bool OJPEGReadHeaderInfoSecTablesDcTable()
  {
    byte[] buffer1 = new byte[16 /*0x10*/];
    if (this.m_dctable_offset[0] == 0U)
      return false;
    this.m_in_buffer_file_pos_log = false;
    for (byte index1 = 0; (int) index1 < (int) this.m_samples_per_pixel; ++index1)
    {
      if (this.m_dctable_offset[(int) index1] != 0U && (index1 == (byte) 0 || (int) this.m_dctable_offset[(int) index1] != (int) this.m_dctable_offset[(int) index1 - 1]))
      {
        for (byte index2 = 0; (int) index2 < (int) index1 - 1; ++index2)
        {
          if ((int) this.m_dctable_offset[(int) index1] == (int) this.m_dctable_offset[(int) index2])
            return false;
        }
        TiffStream stream = this.m_tif.GetStream();
        stream.Seek(this.m_tif.m_clientdata, (long) this.m_dctable_offset[(int) index1], SeekOrigin.Begin);
        if (stream.Read(this.m_tif.m_clientdata, buffer1, 0, 16 /*0x10*/) != 16 /*0x10*/)
          return false;
        uint count = 0;
        for (byte index3 = 0; index3 < (byte) 16 /*0x10*/; ++index3)
          count += (uint) buffer1[(int) index3];
        byte[] buffer2 = new byte[(IntPtr) (21U + count)];
        buffer2[0] = byte.MaxValue;
        buffer2[1] = (byte) 196;
        buffer2[2] = (byte) (19U + count >> 8);
        buffer2[3] = (byte) (19 + (int) count & (int) byte.MaxValue);
        buffer2[4] = index1;
        for (byte index4 = 0; index4 < (byte) 16 /*0x10*/; ++index4)
          buffer2[5 + (int) index4] = buffer1[(int) index4];
        if (stream.Read(this.m_tif.m_clientdata, buffer2, 21, (int) count) != (int) count)
          return false;
        this.m_dctable[(int) index1] = buffer2;
        this.m_sos_tda[(int) index1] = (byte) ((uint) index1 << 4);
      }
      else
        this.m_sos_tda[(int) index1] = this.m_sos_tda[(int) index1 - 1];
    }
    return true;
  }

  private bool OJPEGReadHeaderInfoSecTablesAcTable()
  {
    byte[] buffer1 = new byte[16 /*0x10*/];
    if (this.m_actable_offset[0] == 0U)
      return false;
    this.m_in_buffer_file_pos_log = false;
    for (byte index1 = 0; (int) index1 < (int) this.m_samples_per_pixel; ++index1)
    {
      if (this.m_actable_offset[(int) index1] != 0U && (index1 == (byte) 0 || (int) this.m_actable_offset[(int) index1] != (int) this.m_actable_offset[(int) index1 - 1]))
      {
        for (byte index2 = 0; (int) index2 < (int) index1 - 1; ++index2)
        {
          if ((int) this.m_actable_offset[(int) index1] == (int) this.m_actable_offset[(int) index2])
            return false;
        }
        TiffStream stream = this.m_tif.GetStream();
        stream.Seek(this.m_tif.m_clientdata, (long) this.m_actable_offset[(int) index1], SeekOrigin.Begin);
        if (stream.Read(this.m_tif.m_clientdata, buffer1, 0, 16 /*0x10*/) != 16 /*0x10*/)
          return false;
        uint count = 0;
        for (byte index3 = 0; index3 < (byte) 16 /*0x10*/; ++index3)
          count += (uint) buffer1[(int) index3];
        byte[] buffer2 = new byte[(IntPtr) (21U + count)];
        buffer2[0] = byte.MaxValue;
        buffer2[1] = (byte) 196;
        buffer2[2] = (byte) (19U + count >> 8);
        buffer2[3] = (byte) (19 + (int) count & (int) byte.MaxValue);
        buffer2[4] = (byte) (16U /*0x10*/ | (uint) index1);
        for (byte index4 = 0; index4 < (byte) 16 /*0x10*/; ++index4)
          buffer2[5 + (int) index4] = buffer1[(int) index4];
        if (stream.Read(this.m_tif.m_clientdata, buffer2, 21, (int) count) != (int) count)
          return false;
        this.m_actable[(int) index1] = buffer2;
        this.m_sos_tda[(int) index1] = (byte) ((uint) this.m_sos_tda[(int) index1] | (uint) index1);
      }
      else
        this.m_sos_tda[(int) index1] = (byte) ((uint) this.m_sos_tda[(int) index1] | (uint) this.m_sos_tda[(int) index1 - 1] & 15U);
    }
    return true;
  }

  private bool OJPEGReadBufferFill()
  {
    while (this.m_in_buffer_file_togo == 0U)
    {
      this.m_in_buffer_file_pos_log = false;
      switch (this.m_in_buffer_source)
      {
        case OJpegCodec.OJPEGStateInBufferSource.osibsNotSetYet:
          if (this.m_jpeg_interchange_format != 0U)
          {
            this.m_in_buffer_file_pos = this.m_jpeg_interchange_format;
            this.m_in_buffer_file_togo = this.m_jpeg_interchange_format_length;
          }
          this.m_in_buffer_source = OJpegCodec.OJPEGStateInBufferSource.osibsJpegInterchangeFormat;
          continue;
        case OJpegCodec.OJPEGStateInBufferSource.osibsJpegInterchangeFormat:
          this.m_in_buffer_source = OJpegCodec.OJPEGStateInBufferSource.osibsStrile;
          goto case OJpegCodec.OJPEGStateInBufferSource.osibsStrile;
        case OJpegCodec.OJPEGStateInBufferSource.osibsStrile:
          if ((int) this.m_in_buffer_next_strile == (int) this.m_in_buffer_strile_count)
          {
            this.m_in_buffer_source = OJpegCodec.OJPEGStateInBufferSource.osibsEof;
            continue;
          }
          if (this.m_tif.m_dir.td_stripoffset == null)
            return false;
          this.m_in_buffer_file_pos = this.m_tif.m_dir.td_stripoffset[(IntPtr) this.m_in_buffer_next_strile];
          if (this.m_in_buffer_file_pos != 0U)
          {
            if (this.m_in_buffer_file_pos >= this.m_file_size)
            {
              this.m_in_buffer_file_pos = 0U;
            }
            else
            {
              this.m_in_buffer_file_togo = this.m_tif.m_dir.td_stripbytecount[(IntPtr) this.m_in_buffer_next_strile];
              if (this.m_in_buffer_file_togo == 0U)
                this.m_in_buffer_file_pos = 0U;
              else if (this.m_in_buffer_file_pos + this.m_in_buffer_file_togo > this.m_file_size)
                this.m_in_buffer_file_togo = this.m_file_size - this.m_in_buffer_file_pos;
            }
          }
          ++this.m_in_buffer_next_strile;
          continue;
        default:
          return false;
      }
    }
    TiffStream stream = this.m_tif.GetStream();
    if (!this.m_in_buffer_file_pos_log)
    {
      stream.Seek(this.m_tif.m_clientdata, (long) this.m_in_buffer_file_pos, SeekOrigin.Begin);
      this.m_in_buffer_file_pos_log = true;
    }
    ushort count = 2048 /*0x0800*/;
    if ((uint) count > this.m_in_buffer_file_togo)
      count = (ushort) this.m_in_buffer_file_togo;
    int num1 = stream.Read(this.m_tif.m_clientdata, this.m_in_buffer, 0, (int) count);
    if (num1 == 0)
      return false;
    ushort num2 = (ushort) num1;
    this.m_in_buffer_togo = num2;
    this.m_in_buffer_cur = 0;
    this.m_in_buffer_file_togo -= (uint) num2;
    this.m_in_buffer_file_pos += (uint) num2;
    return true;
  }

  private bool OJPEGReadByte(out byte b)
  {
    if (this.m_in_buffer_togo == (ushort) 0 && !this.OJPEGReadBufferFill())
    {
      b = (byte) 0;
      return false;
    }
    b = this.m_in_buffer[this.m_in_buffer_cur];
    ++this.m_in_buffer_cur;
    --this.m_in_buffer_togo;
    return true;
  }

  public bool OJPEGReadBytePeek(out byte b)
  {
    if (this.m_in_buffer_togo == (ushort) 0 && !this.OJPEGReadBufferFill())
    {
      b = (byte) 0;
      return false;
    }
    b = this.m_in_buffer[this.m_in_buffer_cur];
    return true;
  }

  private void OJPEGReadByteAdvance()
  {
    ++this.m_in_buffer_cur;
    --this.m_in_buffer_togo;
  }

  private bool OJPEGReadWord(out ushort word)
  {
    word = (ushort) 0;
    byte b;
    if (!this.OJPEGReadByte(out b))
      return false;
    word = (ushort) ((uint) b << 8);
    if (!this.OJPEGReadByte(out b))
      return false;
    word |= (ushort) b;
    return true;
  }

  public bool OJPEGReadBlock(ushort len, byte[] mem, int offset)
  {
    ushort num = len;
    int dstOffset = offset;
    while (this.m_in_buffer_togo != (ushort) 0 || this.OJPEGReadBufferFill())
    {
      ushort count = num;
      if ((int) count > (int) this.m_in_buffer_togo)
        count = this.m_in_buffer_togo;
      Buffer.BlockCopy((Array) this.m_in_buffer, this.m_in_buffer_cur, (Array) mem, dstOffset, (int) count);
      this.m_in_buffer_cur += (int) count;
      this.m_in_buffer_togo -= count;
      num -= count;
      dstOffset += (int) count;
      if (num <= (ushort) 0)
        return true;
    }
    return false;
  }

  private void OJPEGReadSkip(ushort len)
  {
    ushort num1 = len;
    ushort num2 = num1;
    if ((int) num2 > (int) this.m_in_buffer_togo)
      num2 = this.m_in_buffer_togo;
    this.m_in_buffer_cur += (int) num2;
    this.m_in_buffer_togo -= num2;
    ushort num3 = (ushort) ((uint) num1 - (uint) num2);
    if (num3 <= (ushort) 0)
      return;
    ushort num4 = num3;
    if ((uint) num4 > this.m_in_buffer_file_togo)
      num4 = (ushort) this.m_in_buffer_file_togo;
    this.m_in_buffer_file_pos += (uint) num4;
    this.m_in_buffer_file_togo -= (uint) num4;
    this.m_in_buffer_file_pos_log = false;
  }

  internal bool OJPEGWriteStream(out byte[] mem, out uint len)
  {
    mem = (byte[]) null;
    len = 0U;
    do
    {
      switch (this.m_out_state)
      {
        case OJpegCodec.OJPEGStateOutState.ososSoi:
          this.OJPEGWriteStreamSoi(out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososQTable0:
          this.OJPEGWriteStreamQTable((byte) 0, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososQTable1:
          this.OJPEGWriteStreamQTable((byte) 1, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososQTable2:
          this.OJPEGWriteStreamQTable((byte) 2, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososQTable3:
          this.OJPEGWriteStreamQTable((byte) 3, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososDcTable0:
          this.OJPEGWriteStreamDcTable((byte) 0, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososDcTable1:
          this.OJPEGWriteStreamDcTable((byte) 1, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososDcTable2:
          this.OJPEGWriteStreamDcTable((byte) 2, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososDcTable3:
          this.OJPEGWriteStreamDcTable((byte) 3, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososAcTable0:
          this.OJPEGWriteStreamAcTable((byte) 0, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososAcTable1:
          this.OJPEGWriteStreamAcTable((byte) 1, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososAcTable2:
          this.OJPEGWriteStreamAcTable((byte) 2, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososAcTable3:
          this.OJPEGWriteStreamAcTable((byte) 3, out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososDri:
          this.OJPEGWriteStreamDri(out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososSof:
          this.OJPEGWriteStreamSof(out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososSos:
          this.OJPEGWriteStreamSos(out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososCompressed:
          if (!this.OJPEGWriteStreamCompressed(out mem, out len))
            return false;
          break;
        case OJpegCodec.OJPEGStateOutState.ososRst:
          this.OJPEGWriteStreamRst(out mem, out len);
          break;
        case OJpegCodec.OJPEGStateOutState.ososEoi:
          this.OJPEGWriteStreamEoi(out mem, out len);
          break;
      }
    }
    while (len == 0U);
    return true;
  }

  private void OJPEGWriteStreamSoi(out byte[] mem, out uint len)
  {
    this.m_out_buffer[0] = byte.MaxValue;
    this.m_out_buffer[1] = (byte) 216;
    len = 2U;
    mem = this.m_out_buffer;
    ++this.m_out_state;
  }

  private void OJPEGWriteStreamQTable(byte table_index, out byte[] mem, out uint len)
  {
    mem = (byte[]) null;
    len = 0U;
    if (this.m_qtable[(int) table_index] != null)
    {
      mem = this.m_qtable[(int) table_index];
      len = (uint) this.m_qtable[(int) table_index].Length;
    }
    ++this.m_out_state;
  }

  private void OJPEGWriteStreamDcTable(byte table_index, out byte[] mem, out uint len)
  {
    mem = (byte[]) null;
    len = 0U;
    if (this.m_dctable[(int) table_index] != null)
    {
      mem = this.m_dctable[(int) table_index];
      len = (uint) this.m_dctable[(int) table_index].Length;
    }
    ++this.m_out_state;
  }

  private void OJPEGWriteStreamAcTable(byte table_index, out byte[] mem, out uint len)
  {
    mem = (byte[]) null;
    len = 0U;
    if (this.m_actable[(int) table_index] != null)
    {
      mem = this.m_actable[(int) table_index];
      len = (uint) this.m_actable[(int) table_index].Length;
    }
    ++this.m_out_state;
  }

  private void OJPEGWriteStreamDri(out byte[] mem, out uint len)
  {
    mem = (byte[]) null;
    len = 0U;
    if (this.m_restart_interval != (ushort) 0)
    {
      this.m_out_buffer[0] = byte.MaxValue;
      this.m_out_buffer[1] = (byte) 221;
      this.m_out_buffer[2] = (byte) 0;
      this.m_out_buffer[3] = (byte) 4;
      this.m_out_buffer[4] = (byte) ((uint) this.m_restart_interval >> 8);
      this.m_out_buffer[5] = (byte) ((uint) this.m_restart_interval & (uint) byte.MaxValue);
      len = 6U;
      mem = this.m_out_buffer;
    }
    ++this.m_out_state;
  }

  private void OJPEGWriteStreamSof(out byte[] mem, out uint len)
  {
    this.m_out_buffer[0] = byte.MaxValue;
    this.m_out_buffer[1] = this.m_sof_marker_id;
    this.m_out_buffer[2] = (byte) 0;
    this.m_out_buffer[3] = (byte) (8 + (int) this.m_samples_per_pixel_per_plane * 3);
    this.m_out_buffer[4] = (byte) 8;
    this.m_out_buffer[5] = (byte) (this.m_sof_y >> 8);
    this.m_out_buffer[6] = (byte) (this.m_sof_y & (uint) byte.MaxValue);
    this.m_out_buffer[7] = (byte) (this.m_sof_x >> 8);
    this.m_out_buffer[8] = (byte) (this.m_sof_x & (uint) byte.MaxValue);
    this.m_out_buffer[9] = this.m_samples_per_pixel_per_plane;
    for (byte index = 0; (int) index < (int) this.m_samples_per_pixel_per_plane; ++index)
    {
      this.m_out_buffer[10 + (int) index * 3] = this.m_sof_c[(int) this.m_plane_sample_offset + (int) index];
      this.m_out_buffer[10 + (int) index * 3 + 1] = this.m_sof_hv[(int) this.m_plane_sample_offset + (int) index];
      this.m_out_buffer[10 + (int) index * 3 + 2] = this.m_sof_tq[(int) this.m_plane_sample_offset + (int) index];
    }
    len = (uint) (10 + (int) this.m_samples_per_pixel_per_plane * 3);
    mem = this.m_out_buffer;
    ++this.m_out_state;
  }

  private void OJPEGWriteStreamSos(out byte[] mem, out uint len)
  {
    this.m_out_buffer[0] = byte.MaxValue;
    this.m_out_buffer[1] = (byte) 218;
    this.m_out_buffer[2] = (byte) 0;
    this.m_out_buffer[3] = (byte) (6 + (int) this.m_samples_per_pixel_per_plane * 2);
    this.m_out_buffer[4] = this.m_samples_per_pixel_per_plane;
    for (byte index = 0; (int) index < (int) this.m_samples_per_pixel_per_plane; ++index)
    {
      this.m_out_buffer[5 + (int) index * 2] = this.m_sos_cs[(int) this.m_plane_sample_offset + (int) index];
      this.m_out_buffer[5 + (int) index * 2 + 1] = this.m_sos_tda[(int) this.m_plane_sample_offset + (int) index];
    }
    this.m_out_buffer[5 + (int) this.m_samples_per_pixel_per_plane * 2] = (byte) 0;
    this.m_out_buffer[5 + (int) this.m_samples_per_pixel_per_plane * 2 + 1] = (byte) 63 /*0x3F*/;
    this.m_out_buffer[5 + (int) this.m_samples_per_pixel_per_plane * 2 + 2] = (byte) 0;
    len = (uint) (8 + (int) this.m_samples_per_pixel_per_plane * 2);
    mem = this.m_out_buffer;
    ++this.m_out_state;
  }

  private bool OJPEGWriteStreamCompressed(out byte[] mem, out uint len)
  {
    mem = (byte[]) null;
    len = 0U;
    if (this.m_in_buffer_togo == (ushort) 0 && !this.OJPEGReadBufferFill())
      return false;
    len = (uint) this.m_in_buffer_togo;
    if (this.m_in_buffer_cur == 0)
    {
      mem = this.m_in_buffer;
    }
    else
    {
      mem = new byte[(IntPtr) len];
      Buffer.BlockCopy((Array) this.m_in_buffer, this.m_in_buffer_cur, (Array) mem, 0, (int) len);
    }
    this.m_in_buffer_togo = (ushort) 0;
    if (this.m_in_buffer_file_togo == 0U)
    {
      switch (this.m_in_buffer_source)
      {
        case OJpegCodec.OJPEGStateInBufferSource.osibsStrile:
          this.m_out_state = this.m_in_buffer_next_strile >= this.m_in_buffer_strile_count ? OJpegCodec.OJPEGStateOutState.ososEoi : OJpegCodec.OJPEGStateOutState.ososRst;
          break;
        case OJpegCodec.OJPEGStateInBufferSource.osibsEof:
          this.m_out_state = OJpegCodec.OJPEGStateOutState.ososEoi;
          break;
      }
    }
    return true;
  }

  private void OJPEGWriteStreamRst(out byte[] mem, out uint len)
  {
    this.m_out_buffer[0] = byte.MaxValue;
    this.m_out_buffer[1] = (byte) (208U /*0xD0*/ + (uint) this.m_restart_index);
    ++this.m_restart_index;
    if (this.m_restart_index == (byte) 8)
      this.m_restart_index = (byte) 0;
    len = 2U;
    mem = this.m_out_buffer;
    this.m_out_state = OJpegCodec.OJPEGStateOutState.ososCompressed;
  }

  private void OJPEGWriteStreamEoi(out byte[] mem, out uint len)
  {
    this.m_out_buffer[0] = byte.MaxValue;
    this.m_out_buffer[1] = (byte) 217;
    len = 2U;
    mem = this.m_out_buffer;
  }

  private bool jpeg_create_decompress_encap()
  {
    try
    {
      this.m_libjpeg_jpeg_decompress_struct = new DecompressStruct();
    }
    catch (Exception ex)
    {
      return false;
    }
    return true;
  }

  private ReadResult jpeg_read_header_encap(bool require_image)
  {
    try
    {
      return this.m_libjpeg_jpeg_decompress_struct.jpeg_read_header(require_image);
    }
    catch (Exception ex)
    {
      return ReadResult.JPEG_SUSPENDED;
    }
  }

  private bool jpeg_start_decompress_encap()
  {
    try
    {
      this.m_libjpeg_jpeg_decompress_struct.jpeg_start_decompress();
    }
    catch (Exception ex)
    {
      return false;
    }
    return true;
  }

  private int jpeg_read_scanlines_encap(byte[] scanlines, int max_lines)
  {
    try
    {
      return this.m_libjpeg_jpeg_decompress_struct.jpeg_read_scanlines(new byte[1][]
      {
        scanlines
      }, max_lines);
    }
    catch (Exception ex)
    {
      return 0;
    }
  }

  private int jpeg_read_raw_data_encap(int max_lines)
  {
    try
    {
      return this.m_libjpeg_jpeg_decompress_struct.jpeg_read_raw_data(this.m_subsampling_convert_ycbcrimage, max_lines);
    }
    catch (Exception ex)
    {
      return 0;
    }
  }

  private enum OJPEGStateInBufferSource
  {
    osibsNotSetYet,
    osibsJpegInterchangeFormat,
    osibsStrile,
    osibsEof,
  }

  private enum OJPEGStateOutState
  {
    ososSoi,
    ososQTable0,
    ososQTable1,
    ososQTable2,
    ososQTable3,
    ososDcTable0,
    ososDcTable1,
    ososDcTable2,
    ososDcTable3,
    ososAcTable0,
    ososAcTable1,
    ososAcTable2,
    ososAcTable3,
    ososDri,
    ososSof,
    ososSos,
    ososCompressed,
    ososRst,
    ososEoi,
  }

  private struct SosEnd
  {
    public bool m_log;
    public OJpegCodec.OJPEGStateInBufferSource m_in_buffer_source;
    public uint m_in_buffer_next_strile;
    public uint m_in_buffer_file_pos;
    public uint m_in_buffer_file_togo;
  }
}
