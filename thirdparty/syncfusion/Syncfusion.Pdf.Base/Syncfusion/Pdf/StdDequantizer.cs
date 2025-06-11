// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.StdDequantizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class StdDequantizer : Dequantizer
{
  private QuantTypeSpec qts;
  private QuantStepSizeSpec qsss;
  private GuardBitsSpec gbs;
  private DataBlockInt inblk;
  private int outdtype;

  internal StdDequantizer(CBlkQuantDataSrcDec src, int[] utrb, DecodeHelper decSpec)
    : base(src, utrb, decSpec)
  {
    if (utrb.Length != src.NumComps)
      throw new ArgumentException("Invalid rb argument");
    this.qsss = decSpec.qsss;
    this.qts = decSpec.qts;
    this.gbs = decSpec.gbs;
  }

  public override int getFixedPoint(int c) => 0;

  public override DataBlock getCodeBlock(int c, int m, int n, SubbandSyn sb, DataBlock cblk)
  {
    return this.getInternCodeBlock(c, m, n, sb, cblk);
  }

  public override DataBlock getInternCodeBlock(
    int c,
    int m,
    int n,
    SubbandSyn sb,
    DataBlock cblk)
  {
    bool flag1 = this.qts.isReversible(this.tIdx, c);
    bool flag2 = this.qts.isDerived(this.tIdx, c);
    StdDequantizerParams tileCompVal1 = (StdDequantizerParams) this.qsss.getTileCompVal(this.tIdx, c);
    int tileCompVal2 = (int) this.gbs.getTileCompVal(this.tIdx, c);
    this.outdtype = cblk.DataType;
    if (flag1 && this.outdtype != 3)
      throw new ArgumentException("Reversible quantizations must use int data");
    int[] numArray1 = (int[]) null;
    float[] numArray2 = (float[]) null;
    int[] numArray3 = (int[]) null;
    switch (this.outdtype)
    {
      case 3:
        cblk = this.src.getCodeBlock(c, m, n, sb, cblk);
        numArray1 = (int[]) cblk.Data;
        break;
      case 4:
        this.inblk = (DataBlockInt) this.src.getInternCodeBlock(c, m, n, sb, (DataBlock) this.inblk);
        numArray3 = this.inblk.DataInt;
        if (cblk == null)
          cblk = (DataBlock) new DataBlockFloat();
        cblk.ulx = this.inblk.ulx;
        cblk.uly = this.inblk.uly;
        cblk.w = this.inblk.w;
        cblk.h = this.inblk.h;
        cblk.offset = 0;
        cblk.scanw = cblk.w;
        cblk.progressive = this.inblk.progressive;
        numArray2 = (float[]) cblk.Data;
        if (numArray2 == null || numArray2.Length < cblk.w * cblk.h)
        {
          numArray2 = new float[cblk.w * cblk.h];
          cblk.Data = (object) numArray2;
          break;
        }
        break;
    }
    int magbits = sb.magbits;
    if (flag1)
    {
      int num1 = 31 /*0x1F*/ - magbits;
      for (int index = numArray1.Length - 1; index >= 0; --index)
      {
        int num2 = numArray1[index];
        numArray1[index] = num2 >= 0 ? num2 >> num1 : -((num2 & int.MaxValue) >> num1);
      }
    }
    else
    {
      float num3;
      if (flag2)
      {
        int resLvl = ((InvWTData) this.src).getSynSubbandTree(this.TileIdx, c).resLvl;
        num3 = tileCompVal1.nStep[0][0] * (float) (1L << this.rb[c] + sb.anGainExp + resLvl - sb.level);
      }
      else
        num3 = tileCompVal1.nStep[sb.resLvl][sb.sbandIdx] * (float) (1L << this.rb[c] + sb.anGainExp);
      int num4 = 31 /*0x1F*/ - magbits;
      float num5 = num3 / (float) (1 << num4);
      switch (this.outdtype)
      {
        case 3:
          for (int index = numArray1.Length - 1; index >= 0; --index)
          {
            int num6 = numArray1[index];
            numArray1[index] = (int) ((num6 >= 0 ? (double) num6 : (double) -(num6 & int.MaxValue)) * (double) num5);
          }
          break;
        case 4:
          int w = cblk.w;
          int h = cblk.h;
          int index1 = w * h - 1;
          int index2 = this.inblk.offset + (h - 1) * this.inblk.scanw + w - 1;
          int num7 = w * (h - 1);
          while (index1 >= 0)
          {
            for (; index1 >= num7; --index1)
            {
              int num8 = numArray3[index2];
              numArray2[index1] = (num8 >= 0 ? (float) num8 : (float) -(num8 & int.MaxValue)) * num5;
              --index2;
            }
            index2 -= this.inblk.scanw - w;
            num7 -= w;
          }
          break;
      }
    }
    return cblk;
  }
}
