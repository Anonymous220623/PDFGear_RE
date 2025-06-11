// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SubbandSyn
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class SubbandSyn : Subband
{
  private SubbandSyn parent;
  private SubbandSyn subb_LL;
  private SubbandSyn subb_HL;
  private SubbandSyn subb_LH;
  private SubbandSyn subb_HH;
  public SynWTFilter hFilter;
  public SynWTFilter vFilter;
  public int magbits;

  public override Subband Parent => (Subband) this.parent;

  public override Subband LL => (Subband) this.subb_LL;

  public override Subband HL => (Subband) this.subb_HL;

  public override Subband LH => (Subband) this.subb_LH;

  public override Subband HH => (Subband) this.subb_HH;

  internal override WaveletFilter HorWFilter => (WaveletFilter) this.hFilter;

  internal override WaveletFilter VerWFilter => (WaveletFilter) this.hFilter;

  public SubbandSyn()
  {
  }

  internal SubbandSyn(
    int w,
    int h,
    int ulcx,
    int ulcy,
    int lvls,
    WaveletFilter[] hfilters,
    WaveletFilter[] vfilters)
    : base(w, h, ulcx, ulcy, lvls, hfilters, vfilters)
  {
  }

  internal override Subband split(WaveletFilter hfilter, WaveletFilter vfilter)
  {
    this.isNode = !this.isNode ? true : throw new ArgumentException();
    this.hFilter = (SynWTFilter) hfilter;
    this.vFilter = (SynWTFilter) vfilter;
    this.subb_LL = new SubbandSyn();
    this.subb_LH = new SubbandSyn();
    this.subb_HL = new SubbandSyn();
    this.subb_HH = new SubbandSyn();
    this.subb_LL.parent = this;
    this.subb_HL.parent = this;
    this.subb_LH.parent = this;
    this.subb_HH.parent = this;
    this.initChilds();
    return (Subband) this.subb_LL;
  }
}
