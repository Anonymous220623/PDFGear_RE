// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DeScalerROI
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class DeScalerROI : MultiResImgDataAdapter, CBlkQuantDataSrcDec, InvWTData, MultiResImgData
{
  public const char OPT_PREFIX = 'R';
  private MaxShiftSpec mss;
  private static readonly string[][] pinfo = new string[1][]
  {
    new string[4]
    {
      "Rno_roi",
      null,
      "This argument makes sure that the no ROI de-scaling is performed. Decompression is done like there is no ROI in the image",
      null
    }
  };
  private CBlkQuantDataSrcDec src;

  public virtual int CbULX => this.src.CbULX;

  public virtual int CbULY => this.src.CbULY;

  public static string[][] ParameterInfo => DeScalerROI.pinfo;

  internal DeScalerROI(CBlkQuantDataSrcDec src, MaxShiftSpec mss)
    : base((MultiResImgData) src)
  {
    this.src = src;
    this.mss = mss;
  }

  public override SubbandSyn getSynSubbandTree(int t, int c)
  {
    return ((InvWTData) this.src).getSynSubbandTree(t, c);
  }

  public virtual DataBlock getCodeBlock(int c, int m, int n, SubbandSyn sb, DataBlock cblk)
  {
    return this.getInternCodeBlock(c, m, n, sb, cblk);
  }

  public virtual DataBlock getInternCodeBlock(int c, int m, int n, SubbandSyn sb, DataBlock cblk)
  {
    cblk = this.src.getInternCodeBlock(c, m, n, sb, cblk);
    bool flag = false;
    if (this.mss == null || this.mss.getTileCompVal(this.TileIdx, c) == null)
      flag = true;
    if (flag || cblk == null)
      return cblk;
    int[] data = (int[]) cblk.Data;
    int ulx = cblk.ulx;
    int uly = cblk.uly;
    int w = cblk.w;
    int h = cblk.h;
    int tileCompVal = (int) this.mss.getTileCompVal(this.TileIdx, c);
    int num1 = (1 << sb.magbits) - 1 << 31 /*0x1F*/ - sb.magbits;
    int num2 = ~num1 & int.MaxValue;
    int num3 = cblk.scanw - w;
    int index1 = cblk.offset + cblk.scanw * (h - 1) + w - 1;
    for (int index2 = h; index2 > 0; --index2)
    {
      int num4 = w;
      while (num4 > 0)
      {
        int num5 = data[index1];
        if ((num5 & num1) == 0)
          data[index1] = num5 & int.MinValue | num5 << tileCompVal;
        else if ((num5 & num2) != 0)
          data[index1] = num5 & ~num2 | 1 << 30 - sb.magbits;
        --num4;
        --index1;
      }
      index1 -= num3;
    }
    return cblk;
  }

  internal static DeScalerROI createInstance(
    CBlkQuantDataSrcDec src,
    JPXParameters pl,
    DecodeHelper decSpec)
  {
    pl.checkList('R', JPXParameters.toNameArray(DeScalerROI.pinfo));
    return pl.getParameter("Rno_roi") != null || decSpec.rois == null ? new DeScalerROI(src, (MaxShiftSpec) null) : new DeScalerROI(src, decSpec.rois);
  }
}
