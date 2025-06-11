// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.StdQuantizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class StdQuantizer : Quantizer
{
  public const int QSTEP_MANTISSA_BITS = 11;
  public const int QSTEP_EXPONENT_BITS = 5;
  public static readonly int QSTEP_MAX_MANTISSA = 2047 /*0x07FF*/;
  public static readonly int QSTEP_MAX_EXPONENT = 31 /*0x1F*/;
  private static double log2 = Math.Log(2.0);
  private QuantTypeSpec qts;
  private QuantStepSizeSpec qsss;
  private GuardBitsSpec gbs;
  private CBlkWTDataFloat infblk;

  public virtual QuantTypeSpec QuantTypeSpec => this.qts;

  internal StdQuantizer(CBlkWTDataSrc src, EncoderSpecs encSpec)
    : base(src)
  {
    this.qts = encSpec.qts;
    this.qsss = encSpec.qsss;
    this.gbs = encSpec.gbs;
  }

  public override int getNumGuardBits(int t, int c) => (int) this.gbs.getTileCompVal(t, c);

  public override bool isReversible(int t, int c) => this.qts.isReversible(t, c);

  public override bool isDerived(int t, int c) => this.qts.isDerived(t, c);

  public override CBlkWTData getNextCodeBlock(int c, CBlkWTData cblk)
  {
    return this.getNextInternCodeBlock(c, cblk);
  }

  public override CBlkWTData getNextInternCodeBlock(int c, CBlkWTData cblk)
  {
    float[] numArray1 = (float[]) null;
    int tileCompVal1 = (int) this.gbs.getTileCompVal(this.tIdx, c);
    bool flag = this.src.getDataType(this.tIdx, c) == 3;
    if (cblk == null)
      cblk = (CBlkWTData) new CBlkWTDataInt();
    CBlkWTDataFloat cblk1 = this.infblk;
    int[] numArray2;
    if (flag)
    {
      cblk = this.src.getNextCodeBlock(c, cblk);
      if (cblk == null)
        return (CBlkWTData) null;
      numArray2 = (int[]) cblk.Data;
    }
    else
    {
      cblk1 = (CBlkWTDataFloat) this.src.getNextInternCodeBlock(c, (CBlkWTData) cblk1);
      if (cblk1 == null)
      {
        this.infblk.Data = (object) null;
        return (CBlkWTData) null;
      }
      this.infblk = cblk1;
      numArray1 = (float[]) cblk1.Data;
      numArray2 = (int[]) cblk.Data;
      if (numArray2 == null || numArray2.Length < cblk1.w * cblk1.h)
      {
        numArray2 = new int[cblk1.w * cblk1.h];
        cblk.Data = (object) numArray2;
      }
      cblk.m = cblk1.m;
      cblk.n = cblk1.n;
      cblk.sb = cblk1.sb;
      cblk.ulx = cblk1.ulx;
      cblk.uly = cblk1.uly;
      cblk.w = cblk1.w;
      cblk.h = cblk1.h;
      cblk.wmseScaling = cblk1.wmseScaling;
      cblk.offset = 0;
      cblk.scanw = cblk.w;
    }
    int w = cblk.w;
    int h = cblk.h;
    SubbandAn sb = cblk.sb;
    if (this.isReversible(this.tIdx, c))
    {
      cblk.magbits = tileCompVal1 - 1 + this.src.getNomRangeBits(c) + sb.anGainExp;
      int num1 = 31 /*0x1F*/ - cblk.magbits;
      cblk.convertFactor = (double) (1 << num1);
      for (int index = w * h - 1; index >= 0; --index)
      {
        int num2 = numArray2[index] << num1;
        numArray2[index] = num2 < 0 ? int.MinValue | -num2 : num2;
      }
    }
    else
    {
      float tileCompVal2 = (float) this.qsss.getTileCompVal(this.tIdx, c);
      float step;
      if (this.isDerived(this.tIdx, c))
      {
        cblk.magbits = tileCompVal1 - 1 + sb.level - (int) Math.Floor(Math.Log((double) tileCompVal2) / StdQuantizer.log2);
        step = tileCompVal2 / (float) (1 << sb.level);
      }
      else
      {
        cblk.magbits = tileCompVal1 - 1 - (int) Math.Floor(Math.Log((double) tileCompVal2 / ((double) sb.l2Norm * (double) (1 << sb.anGainExp))) / StdQuantizer.log2);
        step = tileCompVal2 / (sb.l2Norm * (float) (1 << sb.anGainExp));
      }
      int num3 = 31 /*0x1F*/ - cblk.magbits;
      float num4 = StdQuantizer.convertFromExpMantissa(StdQuantizer.convertToExpMantissa(step));
      float num5 = (float) (1.0 / ((double) (1L << this.src.getNomRangeBits(c) + sb.anGainExp) * (double) num4)) * (float) (1 << num3 - this.src.getFixedPoint(c));
      cblk.convertFactor = (double) num5;
      cblk.stepSize = (double) (1L << this.src.getNomRangeBits(c) + sb.anGainExp) * (double) num4;
      if (flag)
      {
        for (int index = w * h - 1; index >= 0; --index)
        {
          int num6 = (int) ((double) numArray2[index] * (double) num5);
          numArray2[index] = num6 < 0 ? int.MinValue | -num6 : num6;
        }
      }
      else
      {
        int index1 = w * h - 1;
        int index2 = cblk1.offset + (h - 1) * cblk1.scanw + w - 1;
        int num7 = w * (h - 1);
        while (index1 >= 0)
        {
          for (; index1 >= num7; --index1)
          {
            int num8 = (int) ((double) numArray1[index2] * (double) num5);
            numArray2[index1] = num8 < 0 ? int.MinValue | -num8 : num8;
            --index2;
          }
          index2 -= cblk1.scanw - w;
          num7 -= w;
        }
      }
    }
    return cblk;
  }

  internal override void calcSbParams(SubbandAn sb, int c)
  {
    if ((double) sb.stepWMSE > 0.0)
      return;
    if (!sb.isNode)
    {
      if (this.isReversible(this.tIdx, c))
      {
        sb.stepWMSE = (float) Math.Pow(2.0, (double) -(this.src.getNomRangeBits(c) << 1)) * sb.l2Norm * sb.l2Norm;
      }
      else
      {
        float tileCompVal = (float) this.qsss.getTileCompVal(this.tIdx, c);
        if (this.isDerived(this.tIdx, c))
          sb.stepWMSE = (float) ((double) tileCompVal * (double) tileCompVal * Math.Pow(2.0, (double) (sb.anGainExp - sb.level << 1))) * sb.l2Norm * sb.l2Norm;
        else
          sb.stepWMSE = tileCompVal * tileCompVal;
      }
    }
    else
    {
      this.calcSbParams((SubbandAn) sb.LL, c);
      this.calcSbParams((SubbandAn) sb.HL, c);
      this.calcSbParams((SubbandAn) sb.LH, c);
      this.calcSbParams((SubbandAn) sb.HH, c);
      sb.stepWMSE = 1f;
    }
  }

  public static int convertToExpMantissa(float step)
  {
    int num = (int) Math.Ceiling(-Math.Log((double) step) / StdQuantizer.log2);
    return num > StdQuantizer.QSTEP_MAX_EXPONENT ? StdQuantizer.QSTEP_MAX_EXPONENT << 11 : num << 11 | (int) ((-(double) step * (double) (-1 << num) - 1.0) * 2048.0 + 0.5);
  }

  private static float convertFromExpMantissa(int ems)
  {
    return (float) (-1.0 - (double) (ems & StdQuantizer.QSTEP_MAX_MANTISSA) / 2048.0) / (float) (-1 << (ems >> 11 & StdQuantizer.QSTEP_MAX_EXPONENT));
  }

  public override int getMaxMagBits(int c)
  {
    Subband anSubbandTree = (Subband) this.getAnSubbandTree(this.tIdx, c);
    if (this.isReversible(this.tIdx, c))
      return this.getMaxMagBitsRev(anSubbandTree, c);
    return this.isDerived(this.tIdx, c) ? this.getMaxMagBitsDerived(anSubbandTree, this.tIdx, c) : this.getMaxMagBitsExpounded(anSubbandTree, this.tIdx, c);
  }

  private int getMaxMagBitsRev(Subband sb, int c)
  {
    int tileCompVal = (int) this.gbs.getTileCompVal(this.tIdx, c);
    if (!sb.isNode)
      return tileCompVal - 1 + this.src.getNomRangeBits(c) + sb.anGainExp;
    int maxMagBitsRev1 = this.getMaxMagBitsRev(sb.LL, c);
    int maxMagBitsRev2 = this.getMaxMagBitsRev(sb.LH, c);
    if (maxMagBitsRev2 > maxMagBitsRev1)
      maxMagBitsRev1 = maxMagBitsRev2;
    int maxMagBitsRev3 = this.getMaxMagBitsRev(sb.HL, c);
    if (maxMagBitsRev3 > maxMagBitsRev1)
      maxMagBitsRev1 = maxMagBitsRev3;
    int maxMagBitsRev4 = this.getMaxMagBitsRev(sb.HH, c);
    if (maxMagBitsRev4 > maxMagBitsRev1)
      maxMagBitsRev1 = maxMagBitsRev4;
    return maxMagBitsRev1;
  }

  private int getMaxMagBitsDerived(Subband sb, int t, int c)
  {
    int tileCompVal1 = (int) this.gbs.getTileCompVal(t, c);
    if (!sb.isNode)
    {
      float tileCompVal2 = (float) this.qsss.getTileCompVal(t, c);
      return tileCompVal1 - 1 + sb.level - (int) Math.Floor(Math.Log((double) tileCompVal2) / StdQuantizer.log2);
    }
    int maxMagBitsDerived1 = this.getMaxMagBitsDerived(sb.LL, t, c);
    int maxMagBitsDerived2 = this.getMaxMagBitsDerived(sb.LH, t, c);
    if (maxMagBitsDerived2 > maxMagBitsDerived1)
      maxMagBitsDerived1 = maxMagBitsDerived2;
    int maxMagBitsDerived3 = this.getMaxMagBitsDerived(sb.HL, t, c);
    if (maxMagBitsDerived3 > maxMagBitsDerived1)
      maxMagBitsDerived1 = maxMagBitsDerived3;
    int maxMagBitsDerived4 = this.getMaxMagBitsDerived(sb.HH, t, c);
    if (maxMagBitsDerived4 > maxMagBitsDerived1)
      maxMagBitsDerived1 = maxMagBitsDerived4;
    return maxMagBitsDerived1;
  }

  private int getMaxMagBitsExpounded(Subband sb, int t, int c)
  {
    int tileCompVal1 = (int) this.gbs.getTileCompVal(t, c);
    if (!sb.isNode)
    {
      float tileCompVal2 = (float) this.qsss.getTileCompVal(t, c);
      return tileCompVal1 - 1 - (int) Math.Floor(Math.Log((double) tileCompVal2 / ((double) ((SubbandAn) sb).l2Norm * (double) (1 << sb.anGainExp))) / StdQuantizer.log2);
    }
    int magBitsExpounded1 = this.getMaxMagBitsExpounded(sb.LL, t, c);
    int magBitsExpounded2 = this.getMaxMagBitsExpounded(sb.LH, t, c);
    if (magBitsExpounded2 > magBitsExpounded1)
      magBitsExpounded1 = magBitsExpounded2;
    int magBitsExpounded3 = this.getMaxMagBitsExpounded(sb.HL, t, c);
    if (magBitsExpounded3 > magBitsExpounded1)
      magBitsExpounded1 = magBitsExpounded3;
    int magBitsExpounded4 = this.getMaxMagBitsExpounded(sb.HH, t, c);
    if (magBitsExpounded4 > magBitsExpounded1)
      magBitsExpounded1 = magBitsExpounded4;
    return magBitsExpounded1;
  }
}
