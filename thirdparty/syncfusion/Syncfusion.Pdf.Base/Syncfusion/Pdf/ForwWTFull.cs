// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ForwWTFull
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class ForwWTFull : ForwardWT
{
  private bool intData;
  private SubbandAn[][] subbTrees;
  private BlockImageDataSource src;
  private int cb0x;
  private int cb0y;
  private IntegerSpec dls;
  private AnWTFilterSpec filters;
  private CBlkSizeSpec cblks;
  private PrecinctSizeSpec pss;
  private DataBlock[] decomposedComps;
  private int[] lastn;
  private int[] lastm;
  internal SubbandAn[] currentSubband;
  internal JPXImageCoordinates ncblks;

  public override int CbULX => this.cb0x;

  public override int CbULY => this.cb0y;

  internal ForwWTFull(BlockImageDataSource src, EncoderSpecs encSpec, int cb0x, int cb0y)
    : base((ImageData) src)
  {
    this.src = src;
    this.cb0x = cb0x;
    this.cb0y = cb0y;
    this.dls = encSpec.dls;
    this.filters = encSpec.wfs;
    this.cblks = encSpec.cblks;
    this.pss = encSpec.pss;
    int numComps = src.NumComps;
    int numTiles = src.getNumTiles();
    this.currentSubband = new SubbandAn[numComps];
    this.decomposedComps = new DataBlock[numComps];
    this.subbTrees = new SubbandAn[numTiles][];
    for (int index = 0; index < numTiles; ++index)
      this.subbTrees[index] = new SubbandAn[numComps];
    this.lastn = new int[numComps];
    this.lastm = new int[numComps];
  }

  public override int getImplementationType(int c) => WaveletTransform_Fields.WT_IMPL_FULL;

  public override int getDecompLevels(int t, int c) => (int) this.dls.getTileCompVal(t, c);

  public override int getDecomp(int t, int c) => 0;

  public override AnWTFilter[] getHorAnWaveletFilters(int t, int c)
  {
    return this.filters.getHFilters(t, c);
  }

  public override AnWTFilter[] getVertAnWaveletFilters(int t, int c)
  {
    return this.filters.getVFilters(t, c);
  }

  public override bool isReversible(int t, int c) => this.filters.isReversible(t, c);

  public override int getFixedPoint(int c) => this.src.getFixedPoint(c);

  public override CBlkWTData getNextInternCodeBlock(int c, CBlkWTData cblk)
  {
    this.intData = this.filters.getWTDataType(this.tIdx, c) == 3;
    if (this.decomposedComps[c] == null)
    {
      int tileComponentWidth = this.getTileComponentWidth(this.tIdx, c);
      int tileComponentHeight = this.getTileComponentHeight(this.tIdx, c);
      DataBlock blk;
      if (this.intData)
      {
        this.decomposedComps[c] = (DataBlock) new DataBlockInt(0, 0, tileComponentWidth, tileComponentHeight);
        blk = (DataBlock) new DataBlockInt();
      }
      else
      {
        this.decomposedComps[c] = (DataBlock) new DataBlockFloat(0, 0, tileComponentWidth, tileComponentHeight);
        blk = (DataBlock) new DataBlockFloat();
      }
      object data = this.decomposedComps[c].Data;
      int upperLeftCornerX = this.getCompUpperLeftCornerX(c);
      blk.ulx = upperLeftCornerX;
      blk.w = tileComponentWidth;
      blk.h = 1;
      int upperLeftCornerY = this.getCompUpperLeftCornerY(c);
      int num = 0;
      while (num < tileComponentHeight)
      {
        blk.uly = upperLeftCornerY;
        blk.ulx = upperLeftCornerX;
        blk = this.src.getInternCompData(blk, c);
        Array.Copy((Array) blk.Data, blk.offset, (Array) data, num * tileComponentWidth, tileComponentWidth);
        ++num;
        ++upperLeftCornerY;
      }
      this.waveletTreeDecomposition(this.decomposedComps[c], this.getAnSubbandTree(this.tIdx, c), c);
      this.currentSubband[c] = this.getNextSubband(c);
      this.lastn[c] = -1;
      this.lastm[c] = 0;
    }
    do
    {
      this.ncblks = this.currentSubband[c].numCb;
      ++this.lastn[c];
      if (this.lastn[c] == this.ncblks.x)
      {
        this.lastn[c] = 0;
        ++this.lastm[c];
      }
      if (this.lastm[c] >= this.ncblks.y)
      {
        this.currentSubband[c] = this.getNextSubband(c);
        this.lastn[c] = -1;
        this.lastm[c] = 0;
      }
      else
        goto label_13;
    }
    while (this.currentSubband[c] != null);
    this.decomposedComps[c] = (DataBlock) null;
    return (CBlkWTData) null;
label_13:
    int num1 = this.cb0x;
    int num2 = this.cb0y;
    switch (this.currentSubband[c].sbandIdx)
    {
      case 1:
        num1 = 0;
        break;
      case 2:
        num2 = 0;
        break;
      case 3:
        num1 = 0;
        num2 = 0;
        break;
    }
    if (cblk == null)
      cblk = !this.intData ? (CBlkWTData) new CBlkWTDataFloat() : (CBlkWTData) new CBlkWTDataInt();
    int num3 = this.lastn[c];
    int num4 = this.lastm[c];
    SubbandAn subbandAn = this.currentSubband[c];
    cblk.n = num3;
    cblk.m = num4;
    cblk.sb = subbandAn;
    int num5 = (subbandAn.ulcx - num1 + subbandAn.nomCBlkW) / subbandAn.nomCBlkW - 1;
    int num6 = (subbandAn.ulcy - num2 + subbandAn.nomCBlkH) / subbandAn.nomCBlkH - 1;
    cblk.ulx = num3 != 0 ? (num5 + num3) * subbandAn.nomCBlkW - (subbandAn.ulcx - num1) + subbandAn.ulx : subbandAn.ulx;
    cblk.uly = num4 != 0 ? (num6 + num4) * subbandAn.nomCBlkH - (subbandAn.ulcy - num2) + subbandAn.uly : subbandAn.uly;
    cblk.w = num3 >= this.ncblks.x - 1 ? subbandAn.ulx + subbandAn.w - cblk.ulx : (num5 + num3 + 1) * subbandAn.nomCBlkW - (subbandAn.ulcx - num1) + subbandAn.ulx - cblk.ulx;
    cblk.h = num4 >= this.ncblks.y - 1 ? subbandAn.uly + subbandAn.h - cblk.uly : (num6 + num4 + 1) * subbandAn.nomCBlkH - (subbandAn.ulcy - num2) + subbandAn.uly - cblk.uly;
    cblk.wmseScaling = 1f;
    cblk.offset = cblk.uly * this.decomposedComps[c].w + cblk.ulx;
    cblk.scanw = this.decomposedComps[c].w;
    cblk.Data = this.decomposedComps[c].Data;
    return cblk;
  }

  public override CBlkWTData getNextCodeBlock(int c, CBlkWTData cblk)
  {
    this.intData = this.filters.getWTDataType(this.tIdx, c) == 3;
    object destinationArray = (object) null;
    if (cblk != null)
      destinationArray = cblk.Data;
    cblk = this.getNextInternCodeBlock(c, cblk);
    if (cblk == null)
      return (CBlkWTData) null;
    if (this.intData)
    {
      int[] numArray = (int[]) destinationArray;
      if (numArray == null || numArray.Length < cblk.w * cblk.h)
        destinationArray = (object) new int[cblk.w * cblk.h];
    }
    else
    {
      float[] numArray = (float[]) destinationArray;
      if (numArray == null || numArray.Length < cblk.w * cblk.h)
        destinationArray = (object) new float[cblk.w * cblk.h];
    }
    object data = cblk.Data;
    int w = cblk.w;
    int destinationIndex = w * (cblk.h - 1);
    int sourceIndex = cblk.offset + (cblk.h - 1) * cblk.scanw;
    while (destinationIndex >= 0)
    {
      Array.Copy((Array) data, sourceIndex, (Array) destinationArray, destinationIndex, w);
      destinationIndex -= w;
      sourceIndex -= cblk.scanw;
    }
    cblk.Data = destinationArray;
    cblk.offset = 0;
    cblk.scanw = w;
    return cblk;
  }

  public override int getDataType(int t, int c) => this.filters.getWTDataType(t, c);

  private SubbandAn getNextSubband(int c)
  {
    int num1 = 1;
    int num2 = 0;
    int num3 = num1;
    SubbandAn nextSubband = this.currentSubband[c];
    if (nextSubband == null)
    {
      nextSubband = this.getAnSubbandTree(this.tIdx, c);
      if (!nextSubband.isNode)
        return nextSubband;
    }
    while (nextSubband != null)
    {
      if (!nextSubband.isNode)
      {
        switch (nextSubband.orientation)
        {
          case 0:
            nextSubband = (SubbandAn) nextSubband.Parent;
            num3 = num2;
            break;
          case 1:
            nextSubband = (SubbandAn) nextSubband.Parent.LL;
            num3 = num1;
            break;
          case 2:
            nextSubband = (SubbandAn) nextSubband.Parent.HL;
            num3 = num1;
            break;
          case 3:
            nextSubband = (SubbandAn) nextSubband.Parent.LH;
            num3 = num1;
            break;
        }
      }
      else if (nextSubband.isNode)
      {
        if (num3 == num1)
          nextSubband = (SubbandAn) nextSubband.HH;
        else if (num3 == num2)
        {
          switch (nextSubband.orientation)
          {
            case 0:
              nextSubband = (SubbandAn) nextSubband.Parent;
              num3 = num2;
              break;
            case 1:
              nextSubband = (SubbandAn) nextSubband.Parent.LL;
              num3 = num1;
              break;
            case 2:
              nextSubband = (SubbandAn) nextSubband.Parent.HL;
              num3 = num1;
              break;
            case 3:
              nextSubband = (SubbandAn) nextSubband.Parent.LH;
              num3 = num1;
              break;
          }
        }
      }
      if (nextSubband == null || !nextSubband.isNode)
        break;
    }
    return nextSubband;
  }

  private void waveletTreeDecomposition(DataBlock band, SubbandAn subband, int c)
  {
    if (!subband.isNode)
      return;
    this.wavelet2DDecomposition(band, subband, c);
    this.waveletTreeDecomposition(band, (SubbandAn) subband.HH, c);
    this.waveletTreeDecomposition(band, (SubbandAn) subband.LH, c);
    this.waveletTreeDecomposition(band, (SubbandAn) subband.HL, c);
    this.waveletTreeDecomposition(band, (SubbandAn) subband.LL, c);
  }

  private void wavelet2DDecomposition(DataBlock band, SubbandAn subband, int c)
  {
    if (subband.w == 0 || subband.h == 0)
      return;
    int ulx = subband.ulx;
    int uly = subband.uly;
    int w = subband.w;
    int h = subband.h;
    int tileComponentWidth = this.getTileComponentWidth(this.tIdx, c);
    this.getTileComponentHeight(this.tIdx, c);
    if (this.intData)
    {
      int[] inSig = new int[Math.Max(w, h)];
      int[] dataInt = ((DataBlockInt) band).DataInt;
      if (subband.ulcy % 2 == 0)
      {
        for (int index1 = 0; index1 < w; ++index1)
        {
          int lowOff = uly * tileComponentWidth + ulx + index1;
          for (int index2 = 0; index2 < h; ++index2)
            inSig[index2] = dataInt[lowOff + index2 * tileComponentWidth];
          subband.vFilter.analyze_lpf((object) inSig, 0, h, 1, (object) dataInt, lowOff, tileComponentWidth, (object) dataInt, lowOff + (h + 1) / 2 * tileComponentWidth, tileComponentWidth);
        }
      }
      else
      {
        for (int index3 = 0; index3 < w; ++index3)
        {
          int lowOff = uly * tileComponentWidth + ulx + index3;
          for (int index4 = 0; index4 < h; ++index4)
            inSig[index4] = dataInt[lowOff + index4 * tileComponentWidth];
          subband.vFilter.analyze_hpf((object) inSig, 0, h, 1, (object) dataInt, lowOff, tileComponentWidth, (object) dataInt, lowOff + h / 2 * tileComponentWidth, tileComponentWidth);
        }
      }
      if (subband.ulcx % 2 == 0)
      {
        for (int index5 = 0; index5 < h; ++index5)
        {
          int lowOff = (uly + index5) * tileComponentWidth + ulx;
          for (int index6 = 0; index6 < w; ++index6)
            inSig[index6] = dataInt[lowOff + index6];
          subband.hFilter.analyze_lpf((object) inSig, 0, w, 1, (object) dataInt, lowOff, 1, (object) dataInt, lowOff + (w + 1) / 2, 1);
        }
      }
      else
      {
        for (int index7 = 0; index7 < h; ++index7)
        {
          int lowOff = (uly + index7) * tileComponentWidth + ulx;
          for (int index8 = 0; index8 < w; ++index8)
            inSig[index8] = dataInt[lowOff + index8];
          subband.hFilter.analyze_hpf((object) inSig, 0, w, 1, (object) dataInt, lowOff, 1, (object) dataInt, lowOff + w / 2, 1);
        }
      }
    }
    else
    {
      float[] inSig = new float[Math.Max(w, h)];
      float[] dataFloat = ((DataBlockFloat) band).DataFloat;
      if (subband.ulcy % 2 == 0)
      {
        for (int index9 = 0; index9 < w; ++index9)
        {
          int lowOff = uly * tileComponentWidth + ulx + index9;
          for (int index10 = 0; index10 < h; ++index10)
            inSig[index10] = dataFloat[lowOff + index10 * tileComponentWidth];
          subband.vFilter.analyze_lpf((object) inSig, 0, h, 1, (object) dataFloat, lowOff, tileComponentWidth, (object) dataFloat, lowOff + (h + 1) / 2 * tileComponentWidth, tileComponentWidth);
        }
      }
      else
      {
        for (int index11 = 0; index11 < w; ++index11)
        {
          int lowOff = uly * tileComponentWidth + ulx + index11;
          for (int index12 = 0; index12 < h; ++index12)
            inSig[index12] = dataFloat[lowOff + index12 * tileComponentWidth];
          subband.vFilter.analyze_hpf((object) inSig, 0, h, 1, (object) dataFloat, lowOff, tileComponentWidth, (object) dataFloat, lowOff + h / 2 * tileComponentWidth, tileComponentWidth);
        }
      }
      if (subband.ulcx % 2 == 0)
      {
        for (int index13 = 0; index13 < h; ++index13)
        {
          int lowOff = (uly + index13) * tileComponentWidth + ulx;
          for (int index14 = 0; index14 < w; ++index14)
            inSig[index14] = dataFloat[lowOff + index14];
          subband.hFilter.analyze_lpf((object) inSig, 0, w, 1, (object) dataFloat, lowOff, 1, (object) dataFloat, lowOff + (w + 1) / 2, 1);
        }
      }
      else
      {
        for (int index15 = 0; index15 < h; ++index15)
        {
          int lowOff = (uly + index15) * tileComponentWidth + ulx;
          for (int index16 = 0; index16 < w; ++index16)
            inSig[index16] = dataFloat[lowOff + index16];
          subband.hFilter.analyze_hpf((object) inSig, 0, w, 1, (object) dataFloat, lowOff, 1, (object) dataFloat, lowOff + w / 2, 1);
        }
      }
    }
  }

  public override void setTile(int x, int y)
  {
    base.setTile(x, y);
    if (this.decomposedComps == null)
      return;
    for (int index = this.decomposedComps.Length - 1; index >= 0; --index)
    {
      this.decomposedComps[index] = (DataBlock) null;
      this.currentSubband[index] = (SubbandAn) null;
    }
  }

  public override void nextTile()
  {
    base.nextTile();
    if (this.decomposedComps == null)
      return;
    for (int index = this.decomposedComps.Length - 1; index >= 0; --index)
    {
      this.decomposedComps[index] = (DataBlock) null;
      this.currentSubband[index] = (SubbandAn) null;
    }
  }

  public override SubbandAn getAnSubbandTree(int t, int c)
  {
    if (this.subbTrees[t][c] == null)
    {
      this.subbTrees[t][c] = new SubbandAn(this.getTileComponentWidth(t, c), this.getTileComponentHeight(t, c), this.getCompUpperLeftCornerX(c), this.getCompUpperLeftCornerY(c), this.getDecompLevels(t, c), (WaveletFilter[]) this.getHorAnWaveletFilters(t, c), (WaveletFilter[]) this.getVertAnWaveletFilters(t, c));
      this.initSubbandsFields(t, c, (Subband) this.subbTrees[t][c]);
    }
    return this.subbTrees[t][c];
  }

  private void initSubbandsFields(int t, int c, Subband sb)
  {
    int cblkWidth = this.cblks.getCBlkWidth((byte) 3, t, c);
    int cblkHeight = this.cblks.getCBlkHeight((byte) 3, t, c);
    if (!sb.isNode)
    {
      int ppx = this.pss.getPPX(t, c, sb.resLvl);
      int ppy = this.pss.getPPY(t, c, sb.resLvl);
      if (ppx != (int) ushort.MaxValue || ppy != (int) ushort.MaxValue)
      {
        int num1 = MathUtil.log2(ppx);
        int num2 = MathUtil.log2(ppy);
        int num3 = MathUtil.log2(cblkWidth);
        int num4 = MathUtil.log2(cblkHeight);
        if (sb.resLvl == 0)
        {
          sb.nomCBlkW = num3 < num1 ? 1 << num3 : 1 << num1;
          sb.nomCBlkH = num4 < num2 ? 1 << num4 : 1 << num2;
        }
        else
        {
          sb.nomCBlkW = num3 < num1 - 1 ? 1 << num3 : 1 << num1 - 1;
          sb.nomCBlkH = num4 < num2 - 1 ? 1 << num4 : 1 << num2 - 1;
        }
      }
      else
      {
        sb.nomCBlkW = cblkWidth;
        sb.nomCBlkH = cblkHeight;
      }
      if (sb.numCb == null)
        sb.numCb = new JPXImageCoordinates();
      if (sb.w != 0 && sb.h != 0)
      {
        int num5 = this.cb0x;
        int num6 = this.cb0y;
        switch (sb.sbandIdx)
        {
          case 1:
            num5 = 0;
            break;
          case 2:
            num6 = 0;
            break;
          case 3:
            num5 = 0;
            num6 = 0;
            break;
        }
        if (sb.ulcx - num5 < 0 || sb.ulcy - num6 < 0)
          throw new ArgumentException("Invalid code-blocks partition origin or image offset in the reference grid.");
        int num7 = sb.ulcx - num5 + sb.nomCBlkW;
        sb.numCb.x = (num7 + sb.w - 1) / sb.nomCBlkW - (num7 / sb.nomCBlkW - 1);
        int num8 = sb.ulcy - num6 + sb.nomCBlkH;
        sb.numCb.y = (num8 + sb.h - 1) / sb.nomCBlkH - (num8 / sb.nomCBlkH - 1);
      }
      else
        sb.numCb.x = sb.numCb.y = 0;
    }
    else
    {
      this.initSubbandsFields(t, c, sb.LL);
      this.initSubbandsFields(t, c, sb.HL);
      this.initSubbandsFields(t, c, sb.LH);
      this.initSubbandsFields(t, c, sb.HH);
    }
  }
}
