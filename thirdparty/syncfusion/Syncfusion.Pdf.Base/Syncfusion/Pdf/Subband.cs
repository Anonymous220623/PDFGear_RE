// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Subband
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class Subband
{
  public const int WT_ORIENT_LL = 0;
  public const int WT_ORIENT_HL = 1;
  public const int WT_ORIENT_LH = 2;
  public const int WT_ORIENT_HH = 3;
  public bool isNode;
  public int orientation;
  public int level;
  public int resLvl;
  internal JPXImageCoordinates numCb;
  public int anGainExp;
  public int sbandIdx;
  public int ulcx;
  public int ulcy;
  public int ulx;
  public int uly;
  public int w;
  public int h;
  public int nomCBlkW;
  public int nomCBlkH;

  public abstract Subband Parent { get; }

  public abstract Subband LL { get; }

  public abstract Subband HL { get; }

  public abstract Subband LH { get; }

  public abstract Subband HH { get; }

  public virtual Subband NextResLevel
  {
    get
    {
      if (this.level == 0)
        return (Subband) null;
      Subband subband = this;
      do
      {
        subband = subband.Parent;
        if (subband == null)
          return (Subband) null;
      }
      while (subband.resLvl == this.resLvl);
      Subband nextResLevel = subband.HL;
      while (nextResLevel.isNode)
        nextResLevel = nextResLevel.LL;
      return nextResLevel;
    }
  }

  internal abstract WaveletFilter HorWFilter { get; }

  internal abstract WaveletFilter VerWFilter { get; }

  internal abstract Subband split(WaveletFilter hfilter, WaveletFilter vfilter);

  internal virtual void initChilds()
  {
    Subband ll = this.LL;
    Subband hl = this.HL;
    Subband lh = this.LH;
    Subband hh = this.HH;
    ll.level = this.level + 1;
    ll.ulcx = this.ulcx + 1 >> 1;
    ll.ulcy = this.ulcy + 1 >> 1;
    ll.ulx = this.ulx;
    ll.uly = this.uly;
    ll.w = (this.ulcx + this.w + 1 >> 1) - ll.ulcx;
    ll.h = (this.ulcy + this.h + 1 >> 1) - ll.ulcy;
    ll.resLvl = this.orientation == 0 ? this.resLvl - 1 : this.resLvl;
    ll.anGainExp = this.anGainExp;
    ll.sbandIdx = this.sbandIdx << 2;
    hl.orientation = 1;
    hl.level = ll.level;
    hl.ulcx = this.ulcx >> 1;
    hl.ulcy = ll.ulcy;
    hl.ulx = this.ulx + ll.w;
    hl.uly = this.uly;
    hl.w = (this.ulcx + this.w >> 1) - hl.ulcx;
    hl.h = ll.h;
    hl.resLvl = this.resLvl;
    hl.anGainExp = this.anGainExp + 1;
    hl.sbandIdx = (this.sbandIdx << 2) + 1;
    lh.orientation = 2;
    lh.level = ll.level;
    lh.ulcx = ll.ulcx;
    lh.ulcy = this.ulcy >> 1;
    lh.ulx = this.ulx;
    lh.uly = this.uly + ll.h;
    lh.w = ll.w;
    lh.h = (this.ulcy + this.h >> 1) - lh.ulcy;
    lh.resLvl = this.resLvl;
    lh.anGainExp = this.anGainExp + 1;
    lh.sbandIdx = (this.sbandIdx << 2) + 2;
    hh.orientation = 3;
    hh.level = ll.level;
    hh.ulcx = hl.ulcx;
    hh.ulcy = lh.ulcy;
    hh.ulx = hl.ulx;
    hh.uly = lh.uly;
    hh.w = hl.w;
    hh.h = lh.h;
    hh.resLvl = this.resLvl;
    hh.anGainExp = this.anGainExp + 2;
    hh.sbandIdx = (this.sbandIdx << 2) + 3;
  }

  public Subband()
  {
  }

  internal Subband(
    int w,
    int h,
    int ulcx,
    int ulcy,
    int lvls,
    WaveletFilter[] hfilters,
    WaveletFilter[] vfilters)
  {
    this.w = w;
    this.h = h;
    this.ulcx = ulcx;
    this.ulcy = ulcy;
    this.resLvl = lvls;
    Subband subband = this;
    for (int index1 = 0; index1 < lvls; ++index1)
    {
      int index2 = subband.resLvl <= hfilters.Length ? subband.resLvl - 1 : hfilters.Length - 1;
      int index3 = subband.resLvl <= vfilters.Length ? subband.resLvl - 1 : vfilters.Length - 1;
      subband = subband.split(hfilters[index2], vfilters[index3]);
    }
  }

  public virtual Subband nextSubband()
  {
    if (this.isNode)
      throw new ArgumentException();
    switch (this.orientation)
    {
      case 0:
        Subband parent1 = this.Parent;
        return parent1 == null || parent1.resLvl != this.resLvl ? (Subband) null : parent1.HL;
      case 1:
        return this.Parent.LH;
      case 2:
        return this.Parent.HH;
      case 3:
        Subband subband1 = this;
        while (subband1.orientation == 3)
          subband1 = subband1.Parent;
        Subband subband2;
        switch (subband1.orientation)
        {
          case 0:
            Subband parent2 = subband1.Parent;
            if (parent2 == null || parent2.resLvl != this.resLvl)
              return (Subband) null;
            subband2 = parent2.HL;
            break;
          case 1:
            subband2 = subband1.Parent.LH;
            break;
          case 2:
            subband2 = subband1.Parent.HH;
            break;
          default:
            throw new ArgumentException();
        }
        while (subband2.isNode)
          subband2 = subband2.LL;
        return subband2;
      default:
        throw new ArgumentException();
    }
  }

  public virtual Subband getSubbandByIdx(int rl, int sbi)
  {
    Subband subbandByIdx = this;
    if (rl > subbandByIdx.resLvl || rl < 0)
      throw new ArgumentException("Resolution level index out of range");
    if (rl == subbandByIdx.resLvl && sbi == subbandByIdx.sbandIdx)
      return subbandByIdx;
    if (subbandByIdx.sbandIdx != 0)
      subbandByIdx = subbandByIdx.Parent;
    while (subbandByIdx.resLvl > rl)
      subbandByIdx = subbandByIdx.LL;
    while (subbandByIdx.resLvl < rl)
      subbandByIdx = subbandByIdx.Parent;
    switch (sbi)
    {
      case 1:
        return subbandByIdx.HL;
      case 2:
        return subbandByIdx.LH;
      case 3:
        return subbandByIdx.HH;
      default:
        return subbandByIdx;
    }
  }

  public virtual Subband getSubband(int x, int y)
  {
    if (x < this.ulx || y < this.uly || x >= this.ulx + this.w || y >= this.uly + this.h)
      throw new ArgumentException();
    Subband subband;
    Subband hh;
    for (subband = this; subband.isNode; subband = x >= hh.ulx ? (y >= hh.uly ? subband.HH : subband.HL) : (y >= hh.uly ? subband.LH : subband.LL))
      hh = subband.HH;
    return subband;
  }

  public override string ToString()
  {
    return $"w={(object) this.w},h={(object) this.h},ulx={(object) this.ulx},uly={(object) this.uly},ulcx={(object) this.ulcx},ulcy={(object) this.ulcy},idx={(object) this.sbandIdx},orient={(object) this.orientation},node={(object) this.isNode},level={(object) this.level},resLvl={(object) this.resLvl},nomCBlkW={(object) this.nomCBlkW},nomCBlkH={(object) this.nomCBlkH},numCb={(object) this.numCb}";
  }
}
