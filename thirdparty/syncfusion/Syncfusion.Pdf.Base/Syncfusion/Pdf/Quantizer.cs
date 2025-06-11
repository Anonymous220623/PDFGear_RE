// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Quantizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class Quantizer : ImgDataAdapter, CBlkQuantDataSrcEnc, ForwWTDataProps, ImageData
{
  public const char OPT_PREFIX = 'Q';
  private static readonly string[][] pinfo = new string[3][]
  {
    new string[4]
    {
      "Qtype",
      "[<tile-component idx>] <id> [ [<tile-component idx>] <id> ...]",
      "Specifies which quantization type to use for specified tile-component. The default type is either 'reversible' or 'expounded' depending on whether or not the '-lossless' option  is specified.\n<tile-component idx> : see general note.\n<id>: Supported quantization types specification are : 'reversible' (no quantization), 'derived' (derived quantization step size) and 'expounded'.\nExample: -Qtype reversible or -Qtype t2,4-8 c2 reversible t9 derived.",
      null
    },
    new string[4]
    {
      "Qstep",
      "[<tile-component idx>] <bnss> [ [<tile-component idx>] <bnss> ...]",
      "This option specifies the base normalized quantization step size (bnss) for tile-components. It is normalized to a dynamic range of 1 in the image domain. This parameter is ignored in reversible coding. The default value is '1/128' (i.e. 0.0078125).",
      "0.0078125"
    },
    new string[4]
    {
      "Qguard_bits",
      "[<tile-component idx>] <gb> [ [<tile-component idx>] <gb> ...]",
      "The number of bits used for each tile-component in the quantizer to avoid overflow (gb).",
      "2"
    }
  };
  internal CBlkWTDataSrc src;

  public virtual int CbULX => this.src.CbULX;

  public virtual int CbULY => this.src.CbULY;

  public static string[][] ParameterInfo => Quantizer.pinfo;

  internal Quantizer(CBlkWTDataSrc src)
    : base((ImageData) src)
  {
    this.src = src;
  }

  public abstract int getNumGuardBits(int t, int c);

  public abstract bool isDerived(int t, int c);

  internal abstract void calcSbParams(SubbandAn sb, int n);

  public virtual SubbandAn getAnSubbandTree(int t, int c)
  {
    SubbandAn anSubbandTree = this.src.getAnSubbandTree(t, c);
    this.calcSbParams(anSubbandTree, c);
    return anSubbandTree;
  }

  internal static Quantizer createInstance(CBlkWTDataSrc src, EncoderSpecs encSpec)
  {
    return (Quantizer) new StdQuantizer(src, encSpec);
  }

  public abstract int getMaxMagBits(int c);

  public abstract CBlkWTData getNextInternCodeBlock(int param1, CBlkWTData param2);

  public abstract CBlkWTData getNextCodeBlock(int param1, CBlkWTData param2);

  public abstract bool isReversible(int param1, int param2);
}
