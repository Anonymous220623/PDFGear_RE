// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SubbandRectROIMask
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class SubbandRectROIMask : SubbandROIMask
{
  public int[] ulxs;
  public int[] ulys;
  public int[] lrxs;
  public int[] lrys;

  public SubbandRectROIMask(Subband sb, int[] ulxs, int[] ulys, int[] lrxs, int[] lrys, int nr)
    : base(sb.ulx, sb.uly, sb.w, sb.h)
  {
    this.ulxs = ulxs;
    this.ulys = ulys;
    this.lrxs = lrxs;
    this.lrys = lrys;
    if (!sb.isNode)
      return;
    this.isNode = true;
    int num1 = sb.ulcx % 2;
    int num2 = sb.ulcy % 2;
    WaveletFilter horWfilter = sb.HorWFilter;
    WaveletFilter verWfilter = sb.VerWFilter;
    int synLowNegSupport1 = horWfilter.SynLowNegSupport;
    int synHighNegSupport1 = horWfilter.SynHighNegSupport;
    int synLowPosSupport1 = horWfilter.SynLowPosSupport;
    int synHighPosSupport1 = horWfilter.SynHighPosSupport;
    int synLowNegSupport2 = verWfilter.SynLowNegSupport;
    int synHighNegSupport2 = verWfilter.SynHighNegSupport;
    int synLowPosSupport2 = verWfilter.SynLowPosSupport;
    int synHighPosSupport2 = verWfilter.SynHighPosSupport;
    int[] ulxs1 = new int[nr];
    int[] ulys1 = new int[nr];
    int[] lrxs1 = new int[nr];
    int[] lrys1 = new int[nr];
    int[] ulxs2 = new int[nr];
    int[] ulys2 = new int[nr];
    int[] lrxs2 = new int[nr];
    int[] lrys2 = new int[nr];
    for (int index = nr - 1; index >= 0; --index)
    {
      int num3 = ulxs[index];
      if (num1 == 0)
      {
        ulxs1[index] = (num3 + 1 - synLowNegSupport1) / 2;
        ulxs2[index] = (num3 - synHighNegSupport1) / 2;
      }
      else
      {
        ulxs1[index] = (num3 - synLowNegSupport1) / 2;
        ulxs2[index] = (num3 + 1 - synHighNegSupport1) / 2;
      }
      int uly = ulys[index];
      if (num2 == 0)
      {
        ulys1[index] = (uly + 1 - synLowNegSupport2) / 2;
        ulys2[index] = (uly - synHighNegSupport2) / 2;
      }
      else
      {
        ulys1[index] = (uly - synLowNegSupport2) / 2;
        ulys2[index] = (uly + 1 - synHighNegSupport2) / 2;
      }
      int num4 = lrxs[index];
      if (num1 == 0)
      {
        lrxs1[index] = (num4 + synLowPosSupport1) / 2;
        lrxs2[index] = (num4 - 1 + synHighPosSupport1) / 2;
      }
      else
      {
        lrxs1[index] = (num4 - 1 + synLowPosSupport1) / 2;
        lrxs2[index] = (num4 + synHighPosSupport1) / 2;
      }
      int lry = lrys[index];
      if (num2 == 0)
      {
        lrys1[index] = (lry + synLowPosSupport2) / 2;
        lrys2[index] = (lry - 1 + synHighPosSupport2) / 2;
      }
      else
      {
        lrys1[index] = (lry - 1 + synLowPosSupport2) / 2;
        lrys2[index] = (lry + synHighPosSupport2) / 2;
      }
    }
    this.hh = (SubbandROIMask) new SubbandRectROIMask(sb.HH, ulxs2, ulys2, lrxs2, lrys2, nr);
    this.lh = (SubbandROIMask) new SubbandRectROIMask(sb.LH, ulxs1, ulys2, lrxs1, lrys2, nr);
    this.hl = (SubbandROIMask) new SubbandRectROIMask(sb.HL, ulxs2, ulys1, lrxs2, lrys1, nr);
    this.ll = (SubbandROIMask) new SubbandRectROIMask(sb.LL, ulxs1, ulys1, lrxs1, lrys1, nr);
  }
}
