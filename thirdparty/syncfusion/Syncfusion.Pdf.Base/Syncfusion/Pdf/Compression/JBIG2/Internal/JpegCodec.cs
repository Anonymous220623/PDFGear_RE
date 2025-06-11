// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.JpegCodec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class JpegCodec : TiffCodec
{
  public const int FIELD_JPEGTABLES = 66;
  public const int FIELD_RECVPARAMS = 67;
  public const int FIELD_SUBADDRESS = 68;
  public const int FIELD_RECVTIME = 69;
  public const int FIELD_FAXDCS = 70;
  internal DecompressStruct m_decompression;
  internal CommonStruct m_common;
  internal int m_h_sampling;
  internal int m_v_sampling;
  internal byte[] m_jpegtables;
  internal int m_jpegtables_length;
  internal int m_jpegquality;
  internal JpegColorMode m_jpegcolormode;
  internal JpegTablesMode m_jpegtablesmode;
  internal bool m_ycbcrsampling_fetched;
  internal int m_recvparams;
  internal string m_subaddress;
  internal int m_recvtime;
  internal string m_faxdcs;
  private static readonly TiffFieldInfo[] jpegFieldInfo = new TiffFieldInfo[8]
  {
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGTABLES, (short) -3, (short) -3, Syncfusion.Pdf.Compression.JBIG2.TiffType.UNDEFINED, (short) 66, false, true, "JPEGTables"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGQUALITY, (short) 0, (short) 0, Syncfusion.Pdf.Compression.JBIG2.TiffType.NOTYPE, (short) 0, true, false, string.Empty),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGCOLORMODE, (short) 0, (short) 0, Syncfusion.Pdf.Compression.JBIG2.TiffType.NOTYPE, (short) 0, false, false, string.Empty),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGTABLESMODE, (short) 0, (short) 0, Syncfusion.Pdf.Compression.JBIG2.TiffType.NOTYPE, (short) 0, false, false, string.Empty),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXRECVPARAMS, (short) 1, (short) 1, Syncfusion.Pdf.Compression.JBIG2.TiffType.LONG, (short) 67, true, false, "FaxRecvParams"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXSUBADDRESS, (short) -1, (short) -1, Syncfusion.Pdf.Compression.JBIG2.TiffType.ASCII, (short) 68, true, false, "FaxSubAddress"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXRECVTIME, (short) 1, (short) 1, Syncfusion.Pdf.Compression.JBIG2.TiffType.LONG, (short) 69, true, false, "FaxRecvTime"),
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXDCS, (short) -1, (short) -1, Syncfusion.Pdf.Compression.JBIG2.TiffType.ASCII, (short) 70, true, false, "FaxDcs")
  };
  private bool m_rawDecode;
  private bool m_rawEncode;
  private TiffTagMethods m_tagMethods;
  private TiffTagMethods m_parentTagMethods;
  private bool m_cinfo_initialized;
  private Photometric m_photometric;
  private int m_bytesperline;
  private byte[][][] m_ds_buffer = new byte[10][][];
  private int m_scancount;
  private int m_samplesperclump;

  public JpegCodec(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.Compression scheme, string name)
    : base(tif, scheme, name)
  {
    this.m_tagMethods = (TiffTagMethods) new JpegCodecTagMethods();
  }

  private void cleanState()
  {
    this.m_decompression = (DecompressStruct) null;
    this.m_common = (CommonStruct) null;
    this.m_h_sampling = 0;
    this.m_v_sampling = 0;
    this.m_jpegtables = (byte[]) null;
    this.m_jpegtables_length = 0;
    this.m_jpegquality = 0;
    this.m_jpegcolormode = JpegColorMode.RAW;
    this.m_jpegtablesmode = JpegTablesMode.NONE;
    this.m_ycbcrsampling_fetched = false;
    this.m_recvparams = 0;
    this.m_subaddress = (string) null;
    this.m_recvtime = 0;
    this.m_faxdcs = (string) null;
    this.m_rawDecode = false;
    this.m_rawEncode = false;
    this.m_cinfo_initialized = false;
    this.m_photometric = Photometric.MINISWHITE;
    this.m_bytesperline = 0;
    this.m_ds_buffer = new byte[10][][];
    this.m_scancount = 0;
    this.m_samplesperclump = 0;
  }

  public override bool Init()
  {
    this.m_tif.MergeFieldInfo(JpegCodec.jpegFieldInfo, JpegCodec.jpegFieldInfo.Length);
    this.cleanState();
    this.m_parentTagMethods = this.m_tif.m_tagmethods;
    this.m_tif.m_tagmethods = this.m_tagMethods;
    this.m_jpegquality = 75;
    this.m_jpegcolormode = JpegColorMode.RGB;
    this.m_jpegtablesmode = JpegTablesMode.QUANT | JpegTablesMode.HUFF;
    this.m_tif.m_flags |= TiffFlags.NOBITREV;
    if (this.m_tif.m_diroff == 0U)
    {
      this.m_jpegtables_length = 2000;
      this.m_jpegtables = new byte[this.m_jpegtables_length];
    }
    this.m_tif.setFieldBit(39);
    return true;
  }

  public override bool CanDecode => true;

  public override bool SetupDecode() => this.JPEGSetupDecode();

  public override bool PreDecode(short plane) => this.JPEGPreDecode(plane);

  public override bool DecodeRow(byte[] buffer, int offset, int count, short plane)
  {
    return this.m_rawDecode ? this.JPEGDecodeRaw(buffer, offset, count, plane) : this.JPEGDecode(buffer, offset, count, plane);
  }

  public override bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
  {
    return this.m_rawDecode ? this.JPEGDecodeRaw(buffer, offset, count, plane) : this.JPEGDecode(buffer, offset, count, plane);
  }

  public override bool DecodeTile(byte[] buffer, int offset, int count, short plane)
  {
    return this.m_rawDecode ? this.JPEGDecodeRaw(buffer, offset, count, plane) : this.JPEGDecode(buffer, offset, count, plane);
  }

  public override void Cleanup() => this.JPEGCleanup();

  public override int DefStripSize(int size) => this.JPEGDefaultStripSize(size);

  public override void DefTileSize(ref int width, ref int height)
  {
    this.JPEGDefaultTileSize(ref width, ref height);
  }

  public bool InitializeJpeg(bool force_encode, bool force_decode)
  {
    bool flag = true;
    if (this.m_cinfo_initialized)
    {
      if (force_encode && this.m_common.IsDecompressor)
      {
        this.TIFFjpeg_destroy();
      }
      else
      {
        if (!force_decode || this.m_common.IsDecompressor)
          return true;
        this.TIFFjpeg_destroy();
      }
      this.m_cinfo_initialized = false;
    }
    FieldValue[] field1 = this.m_tif.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.TILEBYTECOUNTS);
    if (this.m_tif.IsTiled() && field1 != null)
    {
      int[] intArray = field1[0].ToIntArray();
      if (intArray != null)
        flag = intArray[0] == 0;
    }
    FieldValue[] field2 = this.m_tif.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.STRIPBYTECOUNTS);
    if (!this.m_tif.IsTiled() && field2 != null)
    {
      int[] intArray = field2[0].ToIntArray();
      if (intArray != null)
        flag = intArray[0] == 0;
    }
    if (!force_decode && (force_encode || this.m_tif.m_mode != 0 && flag) || !this.TIFFjpeg_create_decompress())
      return false;
    this.m_cinfo_initialized = true;
    return true;
  }

  public Tiff GetTiff() => this.m_tif;

  public void JPEGResetUpsampled()
  {
    this.m_tif.m_flags &= ~TiffFlags.UPSAMPLED;
    if (this.m_tif.m_dir.td_planarconfig == PlanarConfig.CONTIG && this.m_tif.m_dir.td_photometric == Photometric.YCBCR && this.m_jpegcolormode == JpegColorMode.RGB)
      this.m_tif.m_flags |= TiffFlags.UPSAMPLED;
    if (this.m_tif.m_tilesize > 0)
      this.m_tif.m_tilesize = this.m_tif.IsTiled() ? this.m_tif.TileSize() : -1;
    if (this.m_tif.m_scanlinesize <= 0)
      return;
    this.m_tif.m_scanlinesize = this.m_tif.ScanlineSize();
  }

  private void JPEGCleanup()
  {
    this.m_tif.m_tagmethods = this.m_parentTagMethods;
    if (!this.m_cinfo_initialized)
      return;
    this.TIFFjpeg_destroy();
  }

  private bool JPEGPreDecode(short s)
  {
    TiffDirectory dir = this.m_tif.m_dir;
    if (!this.TIFFjpeg_abort() || this.TIFFjpeg_read_header(true) != ReadResult.JPEG_HEADER_OK)
      return false;
    int x1 = dir.td_imagewidth;
    int x2 = dir.td_imagelength - this.m_tif.m_row;
    if (this.m_tif.IsTiled())
    {
      x1 = dir.td_tilewidth;
      x2 = dir.td_tilelength;
      this.m_bytesperline = this.m_tif.TileRowSize();
    }
    else
    {
      if (x2 > dir.td_rowsperstrip && dir.td_rowsperstrip != -1)
        x2 = dir.td_rowsperstrip;
      this.m_bytesperline = this.m_tif.oldScanlineSize();
    }
    if (dir.td_planarconfig == PlanarConfig.SEPARATE && s > (short) 0)
    {
      x1 = Tiff.howMany(x1, this.m_h_sampling);
      x2 = Tiff.howMany(x2, this.m_v_sampling);
    }
    if (this.m_decompression.Image_width >= x1)
    {
      int imageHeight = this.m_decompression.Image_height;
    }
    if (this.m_decompression.Image_width > x1 || this.m_decompression.Image_height > x2 || this.m_decompression.Num_components != (dir.td_planarconfig == PlanarConfig.CONTIG ? (int) dir.td_samplesperpixel : 1) || this.m_decompression.Data_precision != (int) dir.td_bitspersample)
      return false;
    if (dir.td_planarconfig == PlanarConfig.CONTIG)
    {
      if (this.m_decompression.Comp_info[0].H_samp_factor != this.m_h_sampling || this.m_decompression.Comp_info[0].V_samp_factor != this.m_v_sampling)
      {
        if (this.m_decompression.Comp_info[0].H_samp_factor > this.m_h_sampling || this.m_decompression.Comp_info[0].V_samp_factor > this.m_v_sampling)
          return false;
        if (this.m_tif.FindFieldInfo((Syncfusion.Pdf.Compression.JBIG2.TiffTag) 33918, Syncfusion.Pdf.Compression.JBIG2.TiffType.NOTYPE) == null)
        {
          this.m_h_sampling = this.m_decompression.Comp_info[0].H_samp_factor;
          this.m_v_sampling = this.m_decompression.Comp_info[0].V_samp_factor;
        }
      }
      for (int index = 1; index < this.m_decompression.Num_components; ++index)
      {
        if (this.m_decompression.Comp_info[index].H_samp_factor != 1 || this.m_decompression.Comp_info[index].V_samp_factor != 1)
          return false;
      }
    }
    else if (this.m_decompression.Comp_info[0].H_samp_factor != 1 || this.m_decompression.Comp_info[0].V_samp_factor != 1)
      return false;
    bool flag = false;
    if (dir.td_planarconfig == PlanarConfig.CONTIG && this.m_photometric == Photometric.YCBCR && this.m_jpegcolormode == JpegColorMode.RGB)
    {
      this.m_decompression.Jpeg_color_space = J_COLOR_SPACE.JCS_YCbCr;
      this.m_decompression.Out_color_space = J_COLOR_SPACE.JCS_RGB;
    }
    else
    {
      this.m_decompression.Jpeg_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
      this.m_decompression.Out_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
      if (dir.td_planarconfig == PlanarConfig.CONTIG && (this.m_h_sampling != 1 || this.m_v_sampling != 1))
        flag = true;
    }
    if (flag)
    {
      this.m_decompression.Raw_data_out = true;
      this.m_rawDecode = true;
    }
    else
    {
      this.m_decompression.Raw_data_out = false;
      this.m_rawDecode = false;
    }
    if (!this.TIFFjpeg_start_decompress())
      return false;
    if (flag)
    {
      if (!this.alloc_downsampled_buffers(this.m_decompression.Comp_info, this.m_decompression.Num_components))
        return false;
      this.m_scancount = 8;
    }
    return true;
  }

  private bool JPEGSetupDecode()
  {
    TiffDirectory dir = this.m_tif.m_dir;
    this.InitializeJpeg(false, true);
    if (this.m_tif.fieldSet(66))
    {
      this.m_decompression.Src = (SourceMgr) new JpegTablesSource(this);
      if (this.TIFFjpeg_read_header(false) != ReadResult.JPEG_HEADER_TABLES_ONLY)
        return false;
    }
    this.m_photometric = dir.td_photometric;
    if (this.m_photometric == Photometric.YCBCR)
    {
      this.m_h_sampling = (int) dir.td_ycbcrsubsampling[0];
      this.m_v_sampling = (int) dir.td_ycbcrsubsampling[1];
    }
    else
    {
      this.m_h_sampling = 1;
      this.m_v_sampling = 1;
    }
    this.m_decompression.Src = (SourceMgr) new JpegStdSource(this);
    this.m_tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmNone;
    return true;
  }

  private int TIFFjpeg_read_scanlines(byte[][] scanlines, int max_lines)
  {
    try
    {
      return this.m_decompression.jpeg_read_scanlines(scanlines, max_lines);
    }
    catch (Exception ex)
    {
      return -1;
    }
  }

  private bool JPEGDecode(byte[] buffer, int offset, int count, short plane)
  {
    int num1 = count / this.m_bytesperline;
    int num2 = count % this.m_bytesperline;
    if (num1 > this.m_decompression.Image_height)
      num1 = this.m_decompression.Image_height;
    if (num1 != 0)
    {
      byte[][] scanlines = new byte[1][]
      {
        new byte[this.m_bytesperline]
      };
      do
      {
        Array.Clear((Array) scanlines[0], 0, this.m_bytesperline);
        if (this.TIFFjpeg_read_scanlines(scanlines, 1) != 1)
          return false;
        ++this.m_tif.m_row;
        Buffer.BlockCopy((Array) scanlines[0], 0, (Array) buffer, offset, this.m_bytesperline);
        offset += this.m_bytesperline;
        count -= this.m_bytesperline;
      }
      while (--num1 > 0);
    }
    return this.m_decompression.Output_scanline < this.m_decompression.Output_height || this.TIFFjpeg_finish_decompress();
  }

  private bool JPEGDecodeRaw(byte[] buffer, int offset, int count, short plane)
  {
    int imageHeight = this.m_decompression.Image_height;
    if (imageHeight != 0)
    {
      int downsampledWidth = this.m_decompression.Comp_info[1].Downsampled_width;
      do
      {
        if (this.m_scancount >= 8)
        {
          int max_lines = this.m_decompression.Max_v_samp_factor * 8;
          if (this.TIFFjpeg_read_raw_data(this.m_ds_buffer, max_lines) != max_lines)
            return false;
          this.m_scancount = 0;
        }
        int num1 = 0;
        for (int index1 = 0; index1 < this.m_decompression.Num_components; ++index1)
        {
          int hSampFactor = this.m_decompression.Comp_info[index1].H_samp_factor;
          int vSampFactor = this.m_decompression.Comp_info[index1].V_samp_factor;
          for (int index2 = 0; index2 < vSampFactor; ++index2)
          {
            byte[] numArray = this.m_ds_buffer[index1][this.m_scancount * vSampFactor + index2];
            int index3 = 0;
            int index4 = offset + num1;
            if (index4 < buffer.Length)
            {
              if (hSampFactor == 1)
              {
                int num2 = downsampledWidth;
                while (num2-- > 0)
                {
                  buffer[index4] = numArray[index3];
                  ++index3;
                  index4 += this.m_samplesperclump;
                }
              }
              else
              {
                int num3 = downsampledWidth;
                while (num3-- > 0)
                {
                  for (int index5 = 0; index5 < hSampFactor; ++index5)
                  {
                    buffer[index4 + index5] = numArray[index3];
                    ++index3;
                  }
                  index4 += this.m_samplesperclump;
                }
              }
              num1 += hSampFactor;
            }
            else
              break;
          }
        }
        ++this.m_scancount;
        this.m_tif.m_row += this.m_v_sampling;
        offset += this.m_bytesperline;
        count -= this.m_bytesperline;
        imageHeight -= this.m_v_sampling;
      }
      while (imageHeight > 0);
    }
    return this.m_decompression.Output_scanline < this.m_decompression.Output_height || this.TIFFjpeg_finish_decompress();
  }

  private int JPEGDefaultStripSize(int s)
  {
    s = base.DefStripSize(s);
    if (s < this.m_tif.m_dir.td_imagelength)
      s = Tiff.roundUp(s, (int) this.m_tif.m_dir.td_ycbcrsubsampling[1] * 8);
    return s;
  }

  private void JPEGDefaultTileSize(ref int tw, ref int th)
  {
    base.DefTileSize(ref tw, ref th);
    tw = Tiff.roundUp(tw, (int) this.m_tif.m_dir.td_ycbcrsubsampling[0] * 8);
    th = Tiff.roundUp(th, (int) this.m_tif.m_dir.td_ycbcrsubsampling[1] * 8);
  }

  private bool TIFFjpeg_create_decompress()
  {
    try
    {
      this.m_decompression = new DecompressStruct();
      this.m_common = (CommonStruct) this.m_decompression;
    }
    catch (Exception ex)
    {
      return false;
    }
    return true;
  }

  private ReadResult TIFFjpeg_read_header(bool require_image)
  {
    try
    {
      return this.m_decompression.jpeg_read_header(require_image);
    }
    catch (Exception ex)
    {
      return ReadResult.JPEG_SUSPENDED;
    }
  }

  private bool TIFFjpeg_start_decompress()
  {
    try
    {
      this.m_decompression.jpeg_start_decompress();
    }
    catch (Exception ex)
    {
      return false;
    }
    return true;
  }

  private int TIFFjpeg_read_raw_data(byte[][][] data, int max_lines)
  {
    try
    {
      return this.m_decompression.jpeg_read_raw_data(data, max_lines);
    }
    catch (Exception ex)
    {
      return -1;
    }
  }

  private bool TIFFjpeg_finish_decompress()
  {
    try
    {
      return this.m_decompression.jpeg_finish_decompress();
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  private bool TIFFjpeg_abort()
  {
    try
    {
      this.m_common.jpeg_abort();
    }
    catch (Exception ex)
    {
      return false;
    }
    return true;
  }

  private bool TIFFjpeg_destroy()
  {
    try
    {
      this.m_common.jpeg_destroy();
    }
    catch (Exception ex)
    {
      return false;
    }
    return true;
  }

  private static byte[][] TIFFjpeg_alloc_sarray(int samplesperrow, int numrows)
  {
    byte[][] numArray = new byte[numrows][];
    for (int index = 0; index < numrows; ++index)
      numArray[index] = new byte[samplesperrow];
    return numArray;
  }

  private bool alloc_downsampled_buffers(ComponentInfo[] comp_info, int num_components)
  {
    int num = 0;
    for (int index = 0; index < num_components; ++index)
    {
      ComponentInfo componentInfo = comp_info[index];
      num += componentInfo.H_samp_factor * componentInfo.V_samp_factor;
      byte[][] numArray = JpegCodec.TIFFjpeg_alloc_sarray(componentInfo.Width_in_blocks * 8, componentInfo.V_samp_factor * 8);
      this.m_ds_buffer[index] = numArray;
    }
    this.m_samplesperclump = num;
    return true;
  }
}
