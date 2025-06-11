// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ForwardWT
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class ForwardWT : 
  ImgDataAdapter,
  ForwWT,
  WaveletTransform,
  CBlkWTDataSrc,
  ForwWTDataProps,
  ImageData
{
  public const int WT_DECOMP_DYADIC = 0;
  public const char OPT_PREFIX = 'W';
  private static readonly string[][] pinfo = new string[3][]
  {
    new string[4]
    {
      "Wlev",
      "<number of decomposition levels>",
      "Specifies the number of wavelet decomposition levels to apply to the image. If 0 no wavelet transform is performed. All components and all tiles have the same number of decomposition levels.",
      "5"
    },
    new string[4]
    {
      "Wwt",
      "[full]",
      "Specifies the wavelet transform to be used. Possible value is: 'full' (full page). The value 'full' performs a normal DWT.",
      "full"
    },
    new string[4]
    {
      "Wcboff",
      "<x y>",
      "Code-blocks partition offset in the reference grid. Allowed for <x> and <y> are 0 and 1.\nNote: This option is defined in JPEG 2000 part 2 and may not be supported by all JPEG 2000 decoders.",
      "0 0"
    }
  };

  public static string[][] ParameterInfo => ForwardWT.pinfo;

  public abstract int CbULY { get; }

  public abstract int CbULX { get; }

  internal ForwardWT(ImageData src)
    : base(src)
  {
  }

  internal static ForwardWT createInstance(
    BlockImageDataSource src,
    JPXParameters pl,
    EncoderSpecs encSpec)
  {
    pl.checkList('W', JPXParameters.toNameArray(ForwardWT.pinfo));
    int num = (int) encSpec.dls.getDefault();
    pl.getParameter("Wcboff");
    SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(pl.getParameter("Wcboff"));
    string s1 = tokenizer.Count == 2 ? tokenizer.NextToken() : throw new ArgumentException("'-Wcboff' option needs two arguments. See usage with the '-u' option.");
    int cb0x;
    try
    {
      cb0x = int.Parse(s1);
    }
    catch (FormatException ex)
    {
      throw new ArgumentException("Bad first parameter for the '-Wcboff' option: " + s1);
    }
    if (cb0x < 0 || cb0x > 1)
      throw new ArgumentException("Invalid horizontal code-block partition origin.");
    string s2 = tokenizer.NextToken();
    int cb0y;
    try
    {
      cb0y = int.Parse(s2);
    }
    catch (FormatException ex)
    {
      throw new ArgumentException("Bad second parameter for the '-Wcboff' option: " + s2);
    }
    if (cb0y < 0 || cb0y > 1)
      throw new ArgumentException("Invalid vertical code-block partition origin.");
    if (cb0x == 0)
      ;
    return (ForwardWT) new ForwWTFull(src, encSpec, cb0x, cb0y);
  }

  public abstract bool isReversible(int param1, int param2);

  public abstract CBlkWTData getNextInternCodeBlock(int param1, CBlkWTData param2);

  public abstract int getFixedPoint(int param1);

  public abstract AnWTFilter[] getHorAnWaveletFilters(int param1, int param2);

  public abstract AnWTFilter[] getVertAnWaveletFilters(int param1, int param2);

  public abstract SubbandAn getAnSubbandTree(int param1, int param2);

  public abstract int getDecompLevels(int param1, int param2);

  public abstract CBlkWTData getNextCodeBlock(int param1, CBlkWTData param2);

  public abstract int getImplementationType(int param1);

  public abstract int getDataType(int param1, int param2);

  public abstract int getDecomp(int param1, int param2);
}
