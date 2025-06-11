// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.InvWTFull
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class InvWTFull : WaveletTransformInverse
{
  private int cblkToDecode;
  private CBlkWTDataSrcDec src;
  private int dtype;
  private DataBlock[] reconstructedComps;
  private int[] ndl;
  private Dictionary<int, bool[]> reversible = new Dictionary<int, bool[]>();

  internal InvWTFull(CBlkWTDataSrcDec src, DecodeHelper decSpec)
    : base((MultiResImgData) src, decSpec)
  {
    this.src = src;
    int numComps = src.NumComps;
    this.reconstructedComps = new DataBlock[numComps];
    this.ndl = new int[numComps];
  }

  private bool isSubbandReversible(Subband subband)
  {
    if (!subband.isNode)
      return true;
    return this.isSubbandReversible(subband.LL) && this.isSubbandReversible(subband.HL) && this.isSubbandReversible(subband.LH) && this.isSubbandReversible(subband.HH) && ((SubbandSyn) subband).hFilter.Reversible && ((SubbandSyn) subband).vFilter.Reversible;
  }

  public override bool isReversible(int t, int c)
  {
    if (this.reversible[t] == null)
    {
      this.reversible[t] = new bool[this.NumComps];
      for (int c1 = this.reversible[t].Length - 1; c1 >= 0; --c1)
        this.reversible[t][c1] = this.isSubbandReversible((Subband) ((InvWTData) this.src).getSynSubbandTree(t, c1));
    }
    return this.reversible[t][c];
  }

  public override int getNomRangeBits(int c) => this.src.getNomRangeBits(c);

  public override int getFixedPoint(int c) => this.src.getFixedPoint(c);

  public override DataBlock getInternCompData(DataBlock blk, int c)
  {
    int tileIdx = this.TileIdx;
    this.dtype = ((InvWTData) this.src).getSynSubbandTree(tileIdx, c).HorWFilter != null ? ((InvWTData) this.src).getSynSubbandTree(tileIdx, c).HorWFilter.DataType : 3;
    if (this.reconstructedComps[c] == null)
    {
      switch (this.dtype)
      {
        case 3:
          this.reconstructedComps[c] = (DataBlock) new DataBlockInt(0, 0, this.getTileComponentWidth(tileIdx, c), this.getTileComponentHeight(tileIdx, c));
          break;
        case 4:
          this.reconstructedComps[c] = (DataBlock) new DataBlockFloat(0, 0, this.getTileComponentWidth(tileIdx, c), this.getTileComponentHeight(tileIdx, c));
          break;
      }
      this.waveletTreeReconstruction(this.reconstructedComps[c], ((InvWTData) this.src).getSynSubbandTree(tileIdx, c), c);
    }
    if (blk.DataType != this.dtype)
      blk = this.dtype != 3 ? (DataBlock) new DataBlockFloat(blk.ulx, blk.uly, blk.w, blk.h) : (DataBlock) new DataBlockInt(blk.ulx, blk.uly, blk.w, blk.h);
    blk.Data = this.reconstructedComps[c].Data;
    blk.offset = this.reconstructedComps[c].w * blk.uly + blk.ulx;
    blk.scanw = this.reconstructedComps[c].w;
    blk.progressive = false;
    return blk;
  }

  public override DataBlock getCompData(DataBlock blk, int c)
  {
    object obj = (object) null;
    switch (blk.DataType)
    {
      case 3:
        int[] numArray1 = (int[]) blk.Data;
        if (numArray1 == null || numArray1.Length < blk.w * blk.h)
          numArray1 = new int[blk.w * blk.h];
        obj = (object) numArray1;
        break;
      case 4:
        float[] numArray2 = (float[]) blk.Data;
        if (numArray2 == null || numArray2.Length < blk.w * blk.h)
          numArray2 = new float[blk.w * blk.h];
        obj = (object) numArray2;
        break;
    }
    blk = this.getInternCompData(blk, c);
    blk.Data = obj;
    blk.offset = 0;
    blk.scanw = blk.w;
    return blk;
  }

  private void wavelet2DReconstruction(DataBlock db, SubbandSyn sb, int c)
  {
    if (sb.w == 0 || sb.h == 0)
      return;
    object data = db.Data;
    int ulx = sb.ulx;
    int uly = sb.uly;
    int w = sb.w;
    int h = sb.h;
    object obj = (object) null;
    switch (sb.HorWFilter.DataType)
    {
      case 3:
        obj = (object) new int[w >= h ? w : h];
        break;
      case 4:
        obj = (object) new float[w >= h ? w : h];
        break;
    }
    int num1 = (uly - db.uly) * db.w + ulx - db.ulx;
    if (sb.ulcx % 2 == 0)
    {
      int num2 = 0;
      while (num2 < h)
      {
        Array.Copy((Array) data, num1, (Array) obj, 0, w);
        sb.hFilter.synthetize_lpf(obj, 0, (w + 1) / 2, 1, obj, (w + 1) / 2, w / 2, 1, data, num1, 1);
        ++num2;
        num1 += db.w;
      }
    }
    else
    {
      int num3 = 0;
      while (num3 < h)
      {
        Array.Copy((Array) data, num1, (Array) obj, 0, w);
        sb.hFilter.synthetize_hpf(obj, 0, w / 2, 1, obj, w / 2, (w + 1) / 2, 1, data, num1, 1);
        ++num3;
        num1 += db.w;
      }
    }
    int outOff = (uly - db.uly) * db.w + ulx - db.ulx;
    switch (sb.VerWFilter.DataType)
    {
      case 3:
        int[] numArray1 = (int[]) data;
        int[] numArray2 = (int[]) obj;
        if (sb.ulcy % 2 == 0)
        {
          int num4 = 0;
          while (num4 < w)
          {
            int index1 = h - 1;
            int index2 = outOff + index1 * db.w;
            while (index1 >= 0)
            {
              numArray2[index1] = numArray1[index2];
              --index1;
              index2 -= db.w;
            }
            sb.vFilter.synthetize_lpf(obj, 0, (h + 1) / 2, 1, obj, (h + 1) / 2, h / 2, 1, data, outOff, db.w);
            ++num4;
            ++outOff;
          }
          break;
        }
        int num5 = 0;
        while (num5 < w)
        {
          int index3 = h - 1;
          int index4 = outOff + index3 * db.w;
          while (index3 >= 0)
          {
            numArray2[index3] = numArray1[index4];
            --index3;
            index4 -= db.w;
          }
          sb.vFilter.synthetize_hpf(obj, 0, h / 2, 1, obj, h / 2, (h + 1) / 2, 1, data, outOff, db.w);
          ++num5;
          ++outOff;
        }
        break;
      case 4:
        float[] numArray3 = (float[]) data;
        float[] numArray4 = (float[]) obj;
        if (sb.ulcy % 2 == 0)
        {
          int num6 = 0;
          while (num6 < w)
          {
            int index5 = h - 1;
            int index6 = outOff + index5 * db.w;
            while (index5 >= 0)
            {
              numArray4[index5] = numArray3[index6];
              --index5;
              index6 -= db.w;
            }
            sb.vFilter.synthetize_lpf(obj, 0, (h + 1) / 2, 1, obj, (h + 1) / 2, h / 2, 1, data, outOff, db.w);
            ++num6;
            ++outOff;
          }
          break;
        }
        int num7 = 0;
        while (num7 < w)
        {
          int index7 = h - 1;
          int index8 = outOff + index7 * db.w;
          while (index7 >= 0)
          {
            numArray4[index7] = numArray3[index8];
            --index7;
            index8 -= db.w;
          }
          sb.vFilter.synthetize_hpf(obj, 0, h / 2, 1, obj, h / 2, (h + 1) / 2, 1, data, outOff, db.w);
          ++num7;
          ++outOff;
        }
        break;
    }
  }

  private void waveletTreeReconstruction(DataBlock img, SubbandSyn sb, int c)
  {
    if (!sb.isNode)
    {
      if (sb.w == 0 || sb.h == 0)
        return;
      DataBlock cblk = this.dtype != 3 ? (DataBlock) new DataBlockFloat() : (DataBlock) new DataBlockInt();
      JPXImageCoordinates numCb = sb.numCb;
      object data1 = img.Data;
      for (int m = 0; m < numCb.y; ++m)
      {
        for (int n = 0; n < numCb.x; ++n)
        {
          cblk = this.src.getInternCodeBlock(c, m, n, sb, cblk);
          object data2 = cblk.Data;
          for (int index = cblk.h - 1; index >= 0; --index)
            Array.Copy((Array) data2, cblk.offset + index * cblk.scanw, (Array) data1, (cblk.uly + index) * img.w + cblk.ulx, cblk.w);
        }
      }
    }
    else
    {
      if (!sb.isNode)
        return;
      this.waveletTreeReconstruction(img, (SubbandSyn) sb.LL, c);
      if (sb.resLvl > this.reslvl - this.maxImgRes + this.ndl[c])
        return;
      this.waveletTreeReconstruction(img, (SubbandSyn) sb.HL, c);
      this.waveletTreeReconstruction(img, (SubbandSyn) sb.LH, c);
      this.waveletTreeReconstruction(img, (SubbandSyn) sb.HH, c);
      this.wavelet2DReconstruction(img, sb, c);
    }
  }

  public override int getImplementationType(int c) => WaveletTransform_Fields.WT_IMPL_FULL;

  public override void setTile(int x, int y)
  {
    base.setTile(x, y);
    int numComps = this.src.NumComps;
    int tileIdx = this.src.TileIdx;
    for (int c = 0; c < numComps; ++c)
      this.ndl[c] = ((InvWTData) this.src).getSynSubbandTree(tileIdx, c).resLvl;
    if (this.reconstructedComps != null)
    {
      for (int index = this.reconstructedComps.Length - 1; index >= 0; --index)
        this.reconstructedComps[index] = (DataBlock) null;
    }
    this.cblkToDecode = 0;
    for (int c = 0; c < numComps; ++c)
    {
      SubbandSyn synSubbandTree = ((InvWTData) this.src).getSynSubbandTree(tileIdx, c);
      for (int rl = 0; rl <= this.reslvl - this.maxImgRes + synSubbandTree.resLvl; ++rl)
      {
        if (rl == 0)
        {
          SubbandSyn subbandByIdx = (SubbandSyn) synSubbandTree.getSubbandByIdx(0, 0);
          if (subbandByIdx != null)
            this.cblkToDecode += subbandByIdx.numCb.x * subbandByIdx.numCb.y;
        }
        else
        {
          SubbandSyn subbandByIdx1 = (SubbandSyn) synSubbandTree.getSubbandByIdx(rl, 1);
          if (subbandByIdx1 != null)
            this.cblkToDecode += subbandByIdx1.numCb.x * subbandByIdx1.numCb.y;
          SubbandSyn subbandByIdx2 = (SubbandSyn) synSubbandTree.getSubbandByIdx(rl, 2);
          if (subbandByIdx2 != null)
            this.cblkToDecode += subbandByIdx2.numCb.x * subbandByIdx2.numCb.y;
          SubbandSyn subbandByIdx3 = (SubbandSyn) synSubbandTree.getSubbandByIdx(rl, 3);
          if (subbandByIdx3 != null)
            this.cblkToDecode += subbandByIdx3.numCb.x * subbandByIdx3.numCb.y;
        }
      }
    }
  }

  public override void nextTile()
  {
    base.nextTile();
    int numComps = this.src.NumComps;
    int tileIdx = this.src.TileIdx;
    for (int c = 0; c < numComps; ++c)
      this.ndl[c] = ((InvWTData) this.src).getSynSubbandTree(tileIdx, c).resLvl;
    if (this.reconstructedComps == null)
      return;
    for (int index = this.reconstructedComps.Length - 1; index >= 0; --index)
      this.reconstructedComps[index] = (DataBlock) null;
  }
}
