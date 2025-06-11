// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.TiffDirectory
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class TiffDirectory
{
  public int[] td_fieldsset = new int[4];
  public int td_imagewidth;
  public int td_imagelength;
  public int td_imagedepth;
  public int td_tilewidth;
  public int td_tilelength;
  public int td_tiledepth;
  public FileType td_subfiletype;
  public short td_bitspersample;
  public SampleFormat td_sampleformat;
  public Syncfusion.Pdf.Compression.JBIG2.Compression td_compression;
  public Photometric td_photometric;
  public Threshold td_threshholding;
  public FillOrder td_fillorder;
  public Orientation td_orientation;
  public short td_samplesperpixel;
  public int td_rowsperstrip;
  public ushort td_minsamplevalue;
  public ushort td_maxsamplevalue;
  public double td_sminsamplevalue;
  public double td_smaxsamplevalue;
  public float td_xresolution;
  public float td_yresolution;
  public ResUnit td_resolutionunit;
  public PlanarConfig td_planarconfig;
  public float td_xposition;
  public float td_yposition;
  public short[] td_pagenumber = new short[2];
  public short[][] td_colormap = new short[3][];
  public short[] td_halftonehints = new short[2];
  public short td_extrasamples;
  public ExtraSample[] td_sampleinfo;
  public int td_stripsperimage;
  public int td_nstrips;
  public uint[] td_stripoffset;
  public uint[] td_stripbytecount;
  public bool td_stripbytecountsorted;
  public short td_nsubifd;
  public int[] td_subifd;
  public short[] td_ycbcrsubsampling = new short[2];
  public YCbCrPosition td_ycbcrpositioning;
  public float[] td_refblackwhite;
  public short[][] td_transferfunction = new short[3][];
  public int td_inknameslen;
  public string td_inknames;
  public int td_customValueCount;
  public TiffTagValue[] td_customValues;

  public TiffDirectory()
  {
    this.td_subfiletype = (FileType) 0;
    this.td_compression = (Syncfusion.Pdf.Compression.JBIG2.Compression) 0;
    this.td_photometric = Photometric.MINISWHITE;
    this.td_planarconfig = PlanarConfig.UNKNOWN;
    this.td_fillorder = FillOrder.MSB2LSB;
    this.td_bitspersample = (short) 1;
    this.td_threshholding = Threshold.BILEVEL;
    this.td_orientation = Orientation.TOPLEFT;
    this.td_samplesperpixel = (short) 1;
    this.td_rowsperstrip = -1;
    this.td_tiledepth = 1;
    this.td_stripbytecountsorted = true;
    this.td_resolutionunit = ResUnit.INCH;
    this.td_sampleformat = SampleFormat.UINT;
    this.td_imagedepth = 1;
    this.td_ycbcrsubsampling[0] = (short) 2;
    this.td_ycbcrsubsampling[1] = (short) 2;
    this.td_ycbcrpositioning = YCbCrPosition.CENTERED;
  }
}
