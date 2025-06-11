// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SubbandAn
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class SubbandAn : Subband
{
  public SubbandAn parentband;
  public SubbandAn subb_LL;
  public SubbandAn subb_HL;
  public SubbandAn subb_LH;
  public SubbandAn subb_HH;
  public AnWTFilter hFilter;
  public AnWTFilter vFilter;
  public float l2Norm = -1f;
  public float stepWMSE;

  public override Subband Parent => (Subband) this.parentband;

  public override Subband LL => (Subband) this.subb_LL;

  public override Subband HL => (Subband) this.subb_HL;

  public override Subband LH => (Subband) this.subb_LH;

  public override Subband HH => (Subband) this.subb_HH;

  internal override WaveletFilter HorWFilter => (WaveletFilter) this.hFilter;

  internal override WaveletFilter VerWFilter => (WaveletFilter) this.hFilter;

  public SubbandAn()
  {
  }

  internal SubbandAn(
    int w,
    int h,
    int ulcx,
    int ulcy,
    int lvls,
    WaveletFilter[] hfilters,
    WaveletFilter[] vfilters)
    : base(w, h, ulcx, ulcy, lvls, hfilters, vfilters)
  {
    this.calcL2Norms();
  }

  internal override Subband split(WaveletFilter hfilter, WaveletFilter vfilter)
  {
    this.isNode = !this.isNode ? true : throw new ArgumentException();
    this.hFilter = (AnWTFilter) hfilter;
    this.vFilter = (AnWTFilter) vfilter;
    this.subb_LL = new SubbandAn();
    this.subb_LH = new SubbandAn();
    this.subb_HL = new SubbandAn();
    this.subb_HH = new SubbandAn();
    this.subb_LL.parentband = this;
    this.subb_HL.parentband = this;
    this.subb_LH.parentband = this;
    this.subb_HH.parentband = this;
    this.initChilds();
    return (Subband) this.subb_LL;
  }

  private void calcBasisWaveForms(float[][] wfs)
  {
    if ((double) this.l2Norm >= 0.0)
      return;
    if (this.isNode)
    {
      if ((double) this.subb_LL.l2Norm < 0.0)
      {
        this.subb_LL.calcBasisWaveForms(wfs);
        wfs[0] = this.hFilter.getLPSynWaveForm(wfs[0], (float[]) null);
        wfs[1] = this.vFilter.getLPSynWaveForm(wfs[1], (float[]) null);
      }
      else if ((double) this.subb_HL.l2Norm < 0.0)
      {
        this.subb_HL.calcBasisWaveForms(wfs);
        wfs[0] = this.hFilter.getHPSynWaveForm(wfs[0], (float[]) null);
        wfs[1] = this.vFilter.getLPSynWaveForm(wfs[1], (float[]) null);
      }
      else if ((double) this.subb_LH.l2Norm < 0.0)
      {
        this.subb_LH.calcBasisWaveForms(wfs);
        wfs[0] = this.hFilter.getLPSynWaveForm(wfs[0], (float[]) null);
        wfs[1] = this.vFilter.getHPSynWaveForm(wfs[1], (float[]) null);
      }
      else
      {
        if ((double) this.subb_HH.l2Norm >= 0.0)
          return;
        this.subb_HH.calcBasisWaveForms(wfs);
        wfs[0] = this.hFilter.getHPSynWaveForm(wfs[0], (float[]) null);
        wfs[1] = this.vFilter.getHPSynWaveForm(wfs[1], (float[]) null);
      }
    }
    else
    {
      wfs[0] = new float[1];
      wfs[0][0] = 1f;
      wfs[1] = new float[1];
      wfs[1][0] = 1f;
    }
  }

  private void assignL2Norm(float l2n)
  {
    if ((double) this.l2Norm >= 0.0)
      return;
    if (this.isNode)
    {
      if ((double) this.subb_LL.l2Norm < 0.0)
        this.subb_LL.assignL2Norm(l2n);
      else if ((double) this.subb_HL.l2Norm < 0.0)
        this.subb_HL.assignL2Norm(l2n);
      else if ((double) this.subb_LH.l2Norm < 0.0)
      {
        this.subb_LH.assignL2Norm(l2n);
      }
      else
      {
        if ((double) this.subb_HH.l2Norm >= 0.0)
          return;
        this.subb_HH.assignL2Norm(l2n);
        if ((double) this.subb_HH.l2Norm < 0.0)
          return;
        this.l2Norm = 0.0f;
      }
    }
    else
      this.l2Norm = l2n;
  }

  private void calcL2Norms()
  {
    float[][] wfs = new float[2][];
    while ((double) this.l2Norm < 0.0)
    {
      this.calcBasisWaveForms(wfs);
      double d1 = 0.0;
      for (int index = wfs[0].Length - 1; index >= 0; --index)
        d1 += (double) wfs[0][index] * (double) wfs[0][index];
      float num = (float) Math.Sqrt(d1);
      double d2 = 0.0;
      for (int index = wfs[1].Length - 1; index >= 0; --index)
        d2 += (double) wfs[1][index] * (double) wfs[1][index];
      float l2n = num * (float) Math.Sqrt(d2);
      wfs[0] = (float[]) null;
      wfs[1] = (float[]) null;
      this.assignL2Norm(l2n);
    }
  }
}
