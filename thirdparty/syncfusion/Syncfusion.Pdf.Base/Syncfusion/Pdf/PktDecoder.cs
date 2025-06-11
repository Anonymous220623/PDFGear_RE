// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PktDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class PktDecoder
{
  private BitstreamReader src;
  private bool pph;
  private MemoryStream pphbais;
  private DecodeHelper decSpec;
  private HeaderDecoder hd;
  private int INIT_LBLOCK = 3;
  private PktHeaderBitReader bin;
  private JPXRandomAccessStream ehs;
  private JPXImageCoordinates[][] numPrec;
  private int tIdx;
  private PrecInfo[][][] ppinfo;
  private int[][][][][] lblock;
  private TagTreeDecoder[][][][] ttIncl;
  private TagTreeDecoder[][][][] ttMaxBP;
  private int nl;
  private int nc;
  private bool sopUsed;
  private bool ephUsed;
  private int pktIdx;
  private List<object>[] cblks;
  private int ncb;
  private int maxCB;
  private bool ncbQuit;
  private int tQuit;
  private int cQuit;
  private int sQuit;
  private int rQuit;
  private int xQuit;
  private int yQuit;
  private bool isTruncMode;

  internal PktDecoder(
    DecodeHelper decSpec,
    HeaderDecoder hd,
    JPXRandomAccessStream ehs,
    BitstreamReader src,
    bool isTruncMode,
    int maxCB)
  {
    this.decSpec = decSpec;
    this.hd = hd;
    this.ehs = ehs;
    this.isTruncMode = isTruncMode;
    this.bin = new PktHeaderBitReader(ehs);
    this.src = src;
    this.ncb = 0;
    this.ncbQuit = false;
    this.maxCB = maxCB;
  }

  internal virtual CBlkInfo[][][][][] restart(
    int nc,
    int[] mdl,
    int nl,
    CBlkInfo[][][][][] cbI,
    bool pph,
    MemoryStream pphbais)
  {
    this.nc = nc;
    this.nl = nl;
    this.tIdx = this.src.TileIdx;
    this.pph = pph;
    this.pphbais = pphbais;
    this.sopUsed = (bool) this.decSpec.sops.getTileDef(this.tIdx);
    this.pktIdx = 0;
    this.ephUsed = (bool) this.decSpec.ephs.getTileDef(this.tIdx);
    cbI = new CBlkInfo[nc][][][][];
    this.lblock = new int[nc][][][][];
    this.ttIncl = new TagTreeDecoder[nc][][][];
    this.ttMaxBP = new TagTreeDecoder[nc][][][];
    this.numPrec = new JPXImageCoordinates[nc][];
    this.ppinfo = new PrecInfo[nc][][];
    int cbUlx = this.src.CbULX;
    int cbUly = this.src.CbULY;
    for (int c = 0; c < nc; ++c)
    {
      cbI[c] = new CBlkInfo[mdl[c] + 1][][][];
      this.lblock[c] = new int[mdl[c] + 1][][][];
      this.ttIncl[c] = new TagTreeDecoder[mdl[c] + 1][][];
      this.ttMaxBP[c] = new TagTreeDecoder[mdl[c] + 1][][];
      this.numPrec[c] = new JPXImageCoordinates[mdl[c] + 1];
      this.ppinfo[c] = new PrecInfo[mdl[c] + 1][];
      int resUlx = this.src.getResULX(c, mdl[c]);
      int resUly = this.src.getResULY(c, mdl[c]);
      int num1 = resUlx + this.src.getTileCompWidth(this.tIdx, c, mdl[c]);
      int num2 = resUly + this.src.getTileCompHeight(this.tIdx, c, mdl[c]);
      for (int index1 = 0; index1 <= mdl[c]; ++index1)
      {
        int num3 = (int) Math.Ceiling((double) resUlx / (double) (1 << mdl[c] - index1));
        int num4 = (int) Math.Ceiling((double) resUly / (double) (1 << mdl[c] - index1));
        int num5 = (int) Math.Ceiling((double) num1 / (double) (1 << mdl[c] - index1));
        int num6 = (int) Math.Ceiling((double) num2 / (double) (1 << mdl[c] - index1));
        double ppx = (double) this.getPPX(this.tIdx, c, index1);
        double ppy = (double) this.getPPY(this.tIdx, c, index1);
        this.numPrec[c][index1] = new JPXImageCoordinates();
        this.numPrec[c][index1].x = num5 <= num3 ? 0 : (int) Math.Ceiling((double) (num5 - cbUlx) / ppx) - (int) Math.Floor((double) (num3 - cbUlx) / ppx);
        this.numPrec[c][index1].y = num6 <= num4 ? 0 : (int) Math.Ceiling((double) (num6 - cbUly) / ppy) - (int) Math.Floor((double) (num4 - cbUly) / ppy);
        int num7 = index1 == 0 ? 0 : 1;
        int num8 = index1 == 0 ? 1 : 4;
        int length = this.numPrec[c][index1].x * this.numPrec[c][index1].y;
        this.ttIncl[c][index1] = new TagTreeDecoder[length][];
        for (int index2 = 0; index2 < length; ++index2)
          this.ttIncl[c][index1][index2] = new TagTreeDecoder[num8 + 1];
        this.ttMaxBP[c][index1] = new TagTreeDecoder[length][];
        for (int index3 = 0; index3 < length; ++index3)
          this.ttMaxBP[c][index1][index3] = new TagTreeDecoder[num8 + 1];
        cbI[c][index1] = new CBlkInfo[num8 + 1][][];
        this.lblock[c][index1] = new int[num8 + 1][][];
        this.ppinfo[c][index1] = new PrecInfo[length];
        this.fillPrecInfo(c, index1, mdl[c]);
        SubbandSyn synSubbandTree = this.src.getSynSubbandTree(this.tIdx, c);
        for (int sbi = num7; sbi < num8; ++sbi)
        {
          JPXImageCoordinates numCb = synSubbandTree.getSubbandByIdx(index1, sbi).numCb;
          cbI[c][index1][sbi] = new CBlkInfo[numCb.y][];
          for (int index4 = 0; index4 < numCb.y; ++index4)
            cbI[c][index1][sbi][index4] = new CBlkInfo[numCb.x];
          this.lblock[c][index1][sbi] = new int[numCb.y][];
          for (int index5 = 0; index5 < numCb.y; ++index5)
            this.lblock[c][index1][sbi][index5] = new int[numCb.x];
          for (int index6 = numCb.y - 1; index6 >= 0; --index6)
            ArrayUtil.intArraySet(this.lblock[c][index1][sbi][index6], this.INIT_LBLOCK);
        }
      }
    }
    return cbI;
  }

  private void fillPrecInfo(int c, int r, int mdl)
  {
    if (this.ppinfo[c][r].Length == 0)
      return;
    JPXImageCoordinates tile = this.src.getTile((JPXImageCoordinates) null);
    JPXImageCoordinates numTiles = this.src.getNumTiles((JPXImageCoordinates) null);
    int tilePartUlx = this.src.TilePartULX;
    int tilePartUly = this.src.TilePartULY;
    int nomTileWidth = this.src.NomTileWidth;
    int nomTileHeight = this.src.NomTileHeight;
    int imgUlx = this.hd.ImgULX;
    int imgUly = this.hd.ImgULY;
    int imgWidth = this.hd.ImgWidth;
    int imgHeight = this.hd.ImgHeight;
    int num1 = tile.x == 0 ? imgUlx : tilePartUlx + tile.x * nomTileWidth;
    int num2 = tile.y == 0 ? imgUly : tilePartUly + tile.y * nomTileHeight;
    if (tile.x != numTiles.x - 1)
    {
      int x = tile.x;
    }
    if (tile.y != numTiles.y - 1)
    {
      int y = tile.y;
    }
    int compSubsX = this.hd.getCompSubsX(c);
    int compSubsY = this.hd.getCompSubsY(c);
    int resUlx = this.src.getResULX(c, mdl);
    int resUly = this.src.getResULY(c, mdl);
    int num3 = resUlx + this.src.getTileCompWidth(this.tIdx, c, mdl);
    int num4 = resUly + this.src.getTileCompHeight(this.tIdx, c, mdl);
    int num5 = mdl - r;
    int num6 = (int) Math.Ceiling((double) resUlx / (double) (1 << num5));
    int num7 = (int) Math.Ceiling((double) resUly / (double) (1 << num5));
    int num8 = (int) Math.Ceiling((double) num3 / (double) (1 << num5));
    int num9 = (int) Math.Ceiling((double) num4 / (double) (1 << num5));
    int cbUlx = this.src.CbULX;
    int cbUly = this.src.CbULY;
    double ppx = (double) this.getPPX(this.tIdx, c, r);
    double ppy = (double) this.getPPY(this.tIdx, c, r);
    int num10 = (int) (ppx / 2.0);
    int num11 = (int) (ppy / 2.0);
    PrecInfo[] precInfoArray = this.ppinfo[c][r];
    int index1 = 0;
    int num12 = (int) Math.Floor((double) (num7 - cbUly) / ppy);
    int num13 = (int) Math.Floor((double) (num9 - 1 - cbUly) / ppy);
    int num14 = (int) Math.Floor((double) (num6 - cbUlx) / ppx);
    int num15 = (int) Math.Floor((double) (num8 - 1 - cbUlx) / ppx);
    SubbandSyn synSubbandTree = this.src.getSynSubbandTree(this.tIdx, c);
    int rgw = (int) ppx << num5;
    int rgh = (int) ppy << num5;
    for (int index2 = num12; index2 <= num13; ++index2)
    {
      int num16 = num14;
      while (num16 <= num15)
      {
        int rgulx = num16 != num14 || (num6 - cbUlx) % (compSubsX * (int) ppx) == 0 ? cbUlx + num16 * compSubsX * ((int) ppx << num5) : num1;
        int rguly = index2 != num12 || (num7 - cbUly) % (compSubsY * (int) ppy) == 0 ? cbUly + index2 * compSubsY * ((int) ppy << num5) : num2;
        this.ppinfo[c][r][index1] = new PrecInfo(r, (int) ((double) cbUlx + (double) num16 * ppx), (int) ((double) cbUly + (double) index2 * ppy), (int) ppx, (int) ppy, rgulx, rguly, rgw, rgh);
        if (r == 0)
        {
          int num17 = cbUlx;
          int num18 = cbUly;
          int num19 = num17 + num16 * (int) ppx;
          int num20 = num19 + (int) ppx;
          int num21 = num18 + index2 * (int) ppy;
          int num22 = num21 + (int) ppy;
          SubbandSyn subbandByIdx = (SubbandSyn) synSubbandTree.getSubbandByIdx(0, 0);
          int num23 = num19 < subbandByIdx.ulcx ? subbandByIdx.ulcx : num19;
          int num24 = num20 > subbandByIdx.ulcx + subbandByIdx.w ? subbandByIdx.ulcx + subbandByIdx.w : num20;
          int num25 = num21 < subbandByIdx.ulcy ? subbandByIdx.ulcy : num21;
          int num26 = num22 > subbandByIdx.ulcy + subbandByIdx.h ? subbandByIdx.ulcy + subbandByIdx.h : num22;
          int nomCblkW = subbandByIdx.nomCBlkW;
          int nomCblkH = subbandByIdx.nomCBlkH;
          int num27 = (int) Math.Floor((double) (subbandByIdx.ulcy - num18) / (double) nomCblkH);
          int num28 = (int) Math.Floor((double) (num25 - num18) / (double) nomCblkH);
          int num29 = (int) Math.Floor((double) (num26 - 1 - num18) / (double) nomCblkH);
          int num30 = (int) Math.Floor((double) (subbandByIdx.ulcx - num17) / (double) nomCblkW);
          int num31 = (int) Math.Floor((double) (num23 - num17) / (double) nomCblkW);
          int num32 = (int) Math.Floor((double) (num24 - 1 - num17) / (double) nomCblkW);
          if (num24 - num23 <= 0 || num26 - num25 <= 0)
          {
            this.ppinfo[c][r][index1].nblk[0] = 0;
            this.ttIncl[c][r][index1][0] = new TagTreeDecoder(0, 0);
            this.ttMaxBP[c][r][index1][0] = new TagTreeDecoder(0, 0);
          }
          else
          {
            this.ttIncl[c][r][index1][0] = new TagTreeDecoder(num29 - num28 + 1, num32 - num31 + 1);
            this.ttMaxBP[c][r][index1][0] = new TagTreeDecoder(num29 - num28 + 1, num32 - num31 + 1);
            CBlkCoordInfo[][] cblkCoordInfoArray = new CBlkCoordInfo[num29 - num28 + 1][];
            for (int index3 = 0; index3 < num29 - num28 + 1; ++index3)
              cblkCoordInfoArray[index3] = new CBlkCoordInfo[num32 - num31 + 1];
            this.ppinfo[c][r][index1].cblk[0] = cblkCoordInfoArray;
            this.ppinfo[c][r][index1].nblk[0] = (num29 - num28 + 1) * (num32 - num31 + 1);
            for (int index4 = num28; index4 <= num29; ++index4)
            {
              for (int index5 = num31; index5 <= num32; ++index5)
              {
                CBlkCoordInfo cblkCoordInfo = new CBlkCoordInfo(index4 - num27, index5 - num30);
                if (index5 == num30)
                  cblkCoordInfo.ulx = subbandByIdx.ulx;
                else
                  cblkCoordInfo.ulx = subbandByIdx.ulx + index5 * nomCblkW - (subbandByIdx.ulcx - num17);
                if (index4 == num27)
                  cblkCoordInfo.uly = subbandByIdx.uly;
                else
                  cblkCoordInfo.uly = subbandByIdx.uly + index4 * nomCblkH - (subbandByIdx.ulcy - num18);
                int num33 = num17 + index5 * nomCblkW;
                int num34 = num33 > subbandByIdx.ulcx ? num33 : subbandByIdx.ulcx;
                int num35 = num17 + (index5 + 1) * nomCblkW;
                int num36 = num35 > subbandByIdx.ulcx + subbandByIdx.w ? subbandByIdx.ulcx + subbandByIdx.w : num35;
                cblkCoordInfo.w = num36 - num34;
                int num37 = num18 + index4 * nomCblkH;
                int num38 = num37 > subbandByIdx.ulcy ? num37 : subbandByIdx.ulcy;
                int num39 = num18 + (index4 + 1) * nomCblkH;
                int num40 = num39 > subbandByIdx.ulcy + subbandByIdx.h ? subbandByIdx.ulcy + subbandByIdx.h : num39;
                cblkCoordInfo.h = num40 - num38;
                this.ppinfo[c][r][index1].cblk[0][index4 - num28][index5 - num31] = cblkCoordInfo;
              }
            }
          }
        }
        else
        {
          int num41 = 0;
          int num42 = cbUly;
          int num43 = num41 + num16 * num10;
          int num44 = num43 + num10;
          int num45 = num42 + index2 * num11;
          int num46 = num45 + num11;
          SubbandSyn subbandByIdx1 = (SubbandSyn) synSubbandTree.getSubbandByIdx(r, 1);
          int num47 = num43 < subbandByIdx1.ulcx ? subbandByIdx1.ulcx : num43;
          int num48 = num44 > subbandByIdx1.ulcx + subbandByIdx1.w ? subbandByIdx1.ulcx + subbandByIdx1.w : num44;
          int num49 = num45 < subbandByIdx1.ulcy ? subbandByIdx1.ulcy : num45;
          int num50 = num46 > subbandByIdx1.ulcy + subbandByIdx1.h ? subbandByIdx1.ulcy + subbandByIdx1.h : num46;
          int nomCblkW1 = subbandByIdx1.nomCBlkW;
          int nomCblkH1 = subbandByIdx1.nomCBlkH;
          int num51 = (int) Math.Floor((double) (subbandByIdx1.ulcy - num42) / (double) nomCblkH1);
          int num52 = (int) Math.Floor((double) (num49 - num42) / (double) nomCblkH1);
          int num53 = (int) Math.Floor((double) (num50 - 1 - num42) / (double) nomCblkH1);
          int num54 = (int) Math.Floor((double) (subbandByIdx1.ulcx - num41) / (double) nomCblkW1);
          int num55 = (int) Math.Floor((double) (num47 - num41) / (double) nomCblkW1);
          int num56 = (int) Math.Floor((double) (num48 - 1 - num41) / (double) nomCblkW1);
          if (num48 - num47 <= 0 || num50 - num49 <= 0)
          {
            this.ppinfo[c][r][index1].nblk[1] = 0;
            this.ttIncl[c][r][index1][1] = new TagTreeDecoder(0, 0);
            this.ttMaxBP[c][r][index1][1] = new TagTreeDecoder(0, 0);
          }
          else
          {
            this.ttIncl[c][r][index1][1] = new TagTreeDecoder(num53 - num52 + 1, num56 - num55 + 1);
            this.ttMaxBP[c][r][index1][1] = new TagTreeDecoder(num53 - num52 + 1, num56 - num55 + 1);
            CBlkCoordInfo[][] cblkCoordInfoArray = new CBlkCoordInfo[num53 - num52 + 1][];
            for (int index6 = 0; index6 < num53 - num52 + 1; ++index6)
              cblkCoordInfoArray[index6] = new CBlkCoordInfo[num56 - num55 + 1];
            this.ppinfo[c][r][index1].cblk[1] = cblkCoordInfoArray;
            this.ppinfo[c][r][index1].nblk[1] = (num53 - num52 + 1) * (num56 - num55 + 1);
            for (int index7 = num52; index7 <= num53; ++index7)
            {
              for (int index8 = num55; index8 <= num56; ++index8)
              {
                CBlkCoordInfo cblkCoordInfo = new CBlkCoordInfo(index7 - num51, index8 - num54);
                if (index8 == num54)
                  cblkCoordInfo.ulx = subbandByIdx1.ulx;
                else
                  cblkCoordInfo.ulx = subbandByIdx1.ulx + index8 * nomCblkW1 - (subbandByIdx1.ulcx - num41);
                if (index7 == num51)
                  cblkCoordInfo.uly = subbandByIdx1.uly;
                else
                  cblkCoordInfo.uly = subbandByIdx1.uly + index7 * nomCblkH1 - (subbandByIdx1.ulcy - num42);
                int num57 = num41 + index8 * nomCblkW1;
                int num58 = num57 > subbandByIdx1.ulcx ? num57 : subbandByIdx1.ulcx;
                int num59 = num41 + (index8 + 1) * nomCblkW1;
                int num60 = num59 > subbandByIdx1.ulcx + subbandByIdx1.w ? subbandByIdx1.ulcx + subbandByIdx1.w : num59;
                cblkCoordInfo.w = num60 - num58;
                int num61 = num42 + index7 * nomCblkH1;
                int num62 = num61 > subbandByIdx1.ulcy ? num61 : subbandByIdx1.ulcy;
                int num63 = num42 + (index7 + 1) * nomCblkH1;
                int num64 = num63 > subbandByIdx1.ulcy + subbandByIdx1.h ? subbandByIdx1.ulcy + subbandByIdx1.h : num63;
                cblkCoordInfo.h = num64 - num62;
                this.ppinfo[c][r][index1].cblk[1][index7 - num52][index8 - num55] = cblkCoordInfo;
              }
            }
          }
          int num65 = cbUlx;
          int num66 = 0;
          int num67 = num65 + num16 * num10;
          int num68 = num67 + num10;
          int num69 = num66 + index2 * num11;
          int num70 = num69 + num11;
          SubbandSyn subbandByIdx2 = (SubbandSyn) synSubbandTree.getSubbandByIdx(r, 2);
          int num71 = num67 < subbandByIdx2.ulcx ? subbandByIdx2.ulcx : num67;
          int num72 = num68 > subbandByIdx2.ulcx + subbandByIdx2.w ? subbandByIdx2.ulcx + subbandByIdx2.w : num68;
          int num73 = num69 < subbandByIdx2.ulcy ? subbandByIdx2.ulcy : num69;
          int num74 = num70 > subbandByIdx2.ulcy + subbandByIdx2.h ? subbandByIdx2.ulcy + subbandByIdx2.h : num70;
          int nomCblkW2 = subbandByIdx2.nomCBlkW;
          int nomCblkH2 = subbandByIdx2.nomCBlkH;
          int num75 = (int) Math.Floor((double) (subbandByIdx2.ulcy - num66) / (double) nomCblkH2);
          int num76 = (int) Math.Floor((double) (num73 - num66) / (double) nomCblkH2);
          int num77 = (int) Math.Floor((double) (num74 - 1 - num66) / (double) nomCblkH2);
          int num78 = (int) Math.Floor((double) (subbandByIdx2.ulcx - num65) / (double) nomCblkW2);
          int num79 = (int) Math.Floor((double) (num71 - num65) / (double) nomCblkW2);
          int num80 = (int) Math.Floor((double) (num72 - 1 - num65) / (double) nomCblkW2);
          if (num72 - num71 <= 0 || num74 - num73 <= 0)
          {
            this.ppinfo[c][r][index1].nblk[2] = 0;
            this.ttIncl[c][r][index1][2] = new TagTreeDecoder(0, 0);
            this.ttMaxBP[c][r][index1][2] = new TagTreeDecoder(0, 0);
          }
          else
          {
            this.ttIncl[c][r][index1][2] = new TagTreeDecoder(num77 - num76 + 1, num80 - num79 + 1);
            this.ttMaxBP[c][r][index1][2] = new TagTreeDecoder(num77 - num76 + 1, num80 - num79 + 1);
            CBlkCoordInfo[][] cblkCoordInfoArray = new CBlkCoordInfo[num77 - num76 + 1][];
            for (int index9 = 0; index9 < num77 - num76 + 1; ++index9)
              cblkCoordInfoArray[index9] = new CBlkCoordInfo[num80 - num79 + 1];
            this.ppinfo[c][r][index1].cblk[2] = cblkCoordInfoArray;
            this.ppinfo[c][r][index1].nblk[2] = (num77 - num76 + 1) * (num80 - num79 + 1);
            for (int index10 = num76; index10 <= num77; ++index10)
            {
              for (int index11 = num79; index11 <= num80; ++index11)
              {
                CBlkCoordInfo cblkCoordInfo = new CBlkCoordInfo(index10 - num75, index11 - num78);
                if (index11 == num78)
                  cblkCoordInfo.ulx = subbandByIdx2.ulx;
                else
                  cblkCoordInfo.ulx = subbandByIdx2.ulx + index11 * nomCblkW2 - (subbandByIdx2.ulcx - num65);
                if (index10 == num75)
                  cblkCoordInfo.uly = subbandByIdx2.uly;
                else
                  cblkCoordInfo.uly = subbandByIdx2.uly + index10 * nomCblkH2 - (subbandByIdx2.ulcy - num66);
                int num81 = num65 + index11 * nomCblkW2;
                int num82 = num81 > subbandByIdx2.ulcx ? num81 : subbandByIdx2.ulcx;
                int num83 = num65 + (index11 + 1) * nomCblkW2;
                int num84 = num83 > subbandByIdx2.ulcx + subbandByIdx2.w ? subbandByIdx2.ulcx + subbandByIdx2.w : num83;
                cblkCoordInfo.w = num84 - num82;
                int num85 = num66 + index10 * nomCblkH2;
                int num86 = num85 > subbandByIdx2.ulcy ? num85 : subbandByIdx2.ulcy;
                int num87 = num66 + (index10 + 1) * nomCblkH2;
                int num88 = num87 > subbandByIdx2.ulcy + subbandByIdx2.h ? subbandByIdx2.ulcy + subbandByIdx2.h : num87;
                cblkCoordInfo.h = num88 - num86;
                this.ppinfo[c][r][index1].cblk[2][index10 - num76][index11 - num79] = cblkCoordInfo;
              }
            }
          }
          int num89 = 0;
          int num90 = 0;
          int num91 = num89 + num16 * num10;
          int num92 = num91 + num10;
          int num93 = num90 + index2 * num11;
          int num94 = num93 + num11;
          SubbandSyn subbandByIdx3 = (SubbandSyn) synSubbandTree.getSubbandByIdx(r, 3);
          int num95 = num91 < subbandByIdx3.ulcx ? subbandByIdx3.ulcx : num91;
          int num96 = num92 > subbandByIdx3.ulcx + subbandByIdx3.w ? subbandByIdx3.ulcx + subbandByIdx3.w : num92;
          int num97 = num93 < subbandByIdx3.ulcy ? subbandByIdx3.ulcy : num93;
          int num98 = num94 > subbandByIdx3.ulcy + subbandByIdx3.h ? subbandByIdx3.ulcy + subbandByIdx3.h : num94;
          int nomCblkW3 = subbandByIdx3.nomCBlkW;
          int nomCblkH3 = subbandByIdx3.nomCBlkH;
          int num99 = (int) Math.Floor((double) (subbandByIdx3.ulcy - num90) / (double) nomCblkH3);
          int num100 = (int) Math.Floor((double) (num97 - num90) / (double) nomCblkH3);
          int num101 = (int) Math.Floor((double) (num98 - 1 - num90) / (double) nomCblkH3);
          int num102 = (int) Math.Floor((double) (subbandByIdx3.ulcx - num89) / (double) nomCblkW3);
          int num103 = (int) Math.Floor((double) (num95 - num89) / (double) nomCblkW3);
          int num104 = (int) Math.Floor((double) (num96 - 1 - num89) / (double) nomCblkW3);
          if (num96 - num95 <= 0 || num98 - num97 <= 0)
          {
            this.ppinfo[c][r][index1].nblk[3] = 0;
            this.ttIncl[c][r][index1][3] = new TagTreeDecoder(0, 0);
            this.ttMaxBP[c][r][index1][3] = new TagTreeDecoder(0, 0);
          }
          else
          {
            this.ttIncl[c][r][index1][3] = new TagTreeDecoder(num101 - num100 + 1, num104 - num103 + 1);
            this.ttMaxBP[c][r][index1][3] = new TagTreeDecoder(num101 - num100 + 1, num104 - num103 + 1);
            CBlkCoordInfo[][] cblkCoordInfoArray = new CBlkCoordInfo[num101 - num100 + 1][];
            for (int index12 = 0; index12 < num101 - num100 + 1; ++index12)
              cblkCoordInfoArray[index12] = new CBlkCoordInfo[num104 - num103 + 1];
            this.ppinfo[c][r][index1].cblk[3] = cblkCoordInfoArray;
            this.ppinfo[c][r][index1].nblk[3] = (num101 - num100 + 1) * (num104 - num103 + 1);
            for (int index13 = num100; index13 <= num101; ++index13)
            {
              for (int index14 = num103; index14 <= num104; ++index14)
              {
                CBlkCoordInfo cblkCoordInfo = new CBlkCoordInfo(index13 - num99, index14 - num102);
                if (index14 == num102)
                  cblkCoordInfo.ulx = subbandByIdx3.ulx;
                else
                  cblkCoordInfo.ulx = subbandByIdx3.ulx + index14 * nomCblkW3 - (subbandByIdx3.ulcx - num89);
                if (index13 == num99)
                  cblkCoordInfo.uly = subbandByIdx3.uly;
                else
                  cblkCoordInfo.uly = subbandByIdx3.uly + index13 * nomCblkH3 - (subbandByIdx3.ulcy - num90);
                int num105 = num89 + index14 * nomCblkW3;
                int num106 = num105 > subbandByIdx3.ulcx ? num105 : subbandByIdx3.ulcx;
                int num107 = num89 + (index14 + 1) * nomCblkW3;
                int num108 = num107 > subbandByIdx3.ulcx + subbandByIdx3.w ? subbandByIdx3.ulcx + subbandByIdx3.w : num107;
                cblkCoordInfo.w = num108 - num106;
                int num109 = num90 + index13 * nomCblkH3;
                int num110 = num109 > subbandByIdx3.ulcy ? num109 : subbandByIdx3.ulcy;
                int num111 = num90 + (index13 + 1) * nomCblkH3;
                int num112 = num111 > subbandByIdx3.ulcy + subbandByIdx3.h ? subbandByIdx3.ulcy + subbandByIdx3.h : num111;
                cblkCoordInfo.h = num112 - num110;
                this.ppinfo[c][r][index1].cblk[3][index13 - num100][index14 - num103] = cblkCoordInfo;
              }
            }
          }
        }
        ++num16;
        ++index1;
      }
    }
  }

  public virtual int getNumPrecinct(int c, int r) => this.numPrec[c][r].x * this.numPrec[c][r].y;

  public virtual bool readPktHead(int l, int r, int c, int p, CBlkInfo[][][] cbI, int[] nb)
  {
    int num1 = 0;
    int pos = this.ehs.Pos;
    if (pos >= this.ehs.length())
      return true;
    int tileIdx = this.src.TileIdx;
    SubbandSyn synSubbandTree = this.src.getSynSubbandTree(tileIdx, c);
    PktHeaderBitReader pktHeaderBitReader = !this.pph ? this.bin : new PktHeaderBitReader(this.pphbais);
    int num2 = r == 0 ? 0 : 1;
    int num3 = r == 0 ? 1 : 4;
    bool flag = false;
    for (int index = num2; index < num3; ++index)
    {
      if (p < this.ppinfo[c][r].Length)
        flag = true;
    }
    if (!flag)
      return false;
    PrecInfo precInfo = this.ppinfo[c][r][p];
    pktHeaderBitReader.sync();
    if (pktHeaderBitReader.readBit() == 0)
    {
      this.cblks = new List<object>[num3 + 1];
      for (int index = num2; index < num3; ++index)
        this.cblks[index] = new List<object>(10);
      ++this.pktIdx;
      if (this.isTruncMode && this.maxCB == -1)
      {
        int num4 = this.ehs.Pos - pos;
        if (num4 > nb[tileIdx])
        {
          nb[tileIdx] = 0;
          return true;
        }
        nb[tileIdx] -= num4;
      }
      if (this.ephUsed)
        this.readEPHMarker(pktHeaderBitReader);
      return false;
    }
    if (this.cblks == null || this.cblks.Length < num3 + 1)
      this.cblks = new List<object>[num3 + 1];
    for (int sbi = num2; sbi < num3; ++sbi)
    {
      if (this.cblks[sbi] == null)
        this.cblks[sbi] = new List<object>(10);
      else
        this.cblks[sbi].Clear();
      SubbandSyn subbandByIdx = (SubbandSyn) synSubbandTree.getSubbandByIdx(r, sbi);
      if (precInfo.nblk[sbi] != 0)
      {
        TagTreeDecoder tagTreeDecoder1 = this.ttIncl[c][r][p][sbi];
        TagTreeDecoder tagTreeDecoder2 = this.ttMaxBP[c][r][p][sbi];
        int length1 = precInfo.cblk[sbi] == null ? 0 : precInfo.cblk[sbi].Length;
        for (int m = 0; m < length1; ++m)
        {
          int length2 = precInfo.cblk[sbi][m] == null ? 0 : precInfo.cblk[sbi][m].Length;
          for (int n1 = 0; n1 < length2; ++n1)
          {
            JPXImageCoordinates idx = precInfo.cblk[sbi][m][n1].idx;
            int x1 = idx.x;
            int y = idx.y;
            int x2 = subbandByIdx.numCb.x;
            CBlkInfo cblkInfo = cbI[sbi][idx.y][idx.x];
            try
            {
              int num5;
              if (cblkInfo == null || cblkInfo.ctp == 0)
              {
                if (cblkInfo == null)
                  cblkInfo = cbI[sbi][idx.y][idx.x] = new CBlkInfo(precInfo.cblk[sbi][m][n1].ulx, precInfo.cblk[sbi][m][n1].uly, precInfo.cblk[sbi][m][n1].w, precInfo.cblk[sbi][m][n1].h, this.nl);
                cblkInfo.pktIdx[l] = this.pktIdx;
                if (tagTreeDecoder1.update(m, n1, l + 1, pktHeaderBitReader) <= l)
                {
                  int num6 = 1;
                  int t;
                  for (t = 1; num6 >= t; ++t)
                    num6 = tagTreeDecoder2.update(m, n1, t, pktHeaderBitReader);
                  cblkInfo.msbSkipped = t - 2;
                  num5 = 1;
                  cblkInfo.addNTP(l, 0);
                  ++this.ncb;
                  if (this.maxCB != -1 && !this.ncbQuit && this.ncb == this.maxCB)
                  {
                    this.ncbQuit = true;
                    this.tQuit = tileIdx;
                    this.cQuit = c;
                    this.sQuit = sbi;
                    this.rQuit = r;
                    this.xQuit = idx.x;
                    this.yQuit = idx.y;
                  }
                }
                else
                  continue;
              }
              else
              {
                cblkInfo.pktIdx[l] = this.pktIdx;
                if (pktHeaderBitReader.readBit() == 1)
                  num5 = 1;
                else
                  continue;
              }
              if (pktHeaderBitReader.readBit() == 1)
              {
                ++num5;
                if (pktHeaderBitReader.readBit() == 1)
                {
                  int num7 = num5 + 1;
                  int num8 = pktHeaderBitReader.readBits(2);
                  num5 = num7 + num8;
                  if (num8 == 3)
                  {
                    int num9 = pktHeaderBitReader.readBits(5);
                    num5 += num9;
                    if (num9 == 31 /*0x1F*/)
                      num5 += pktHeaderBitReader.readBits(7);
                  }
                }
              }
              cblkInfo.addNTP(l, num5);
              num1 += num5;
              this.cblks[sbi].Add((object) precInfo.cblk[sbi][m][n1]);
              int tileCompVal = (int) this.decSpec.ecopts.getTileCompVal(tileIdx, c);
              int length3;
              if ((tileCompVal & StdEntropyCoderOptions.OPT_TERM_PASS) != 0)
                length3 = num5;
              else if ((tileCompVal & StdEntropyCoderOptions.OPT_BYPASS) != 0)
              {
                if (cblkInfo.ctp <= StdEntropyCoderOptions.FIRST_BYPASS_PASS_IDX)
                {
                  length3 = 1;
                }
                else
                {
                  length3 = 1;
                  for (int index = cblkInfo.ctp - num5; index < cblkInfo.ctp - 1; ++index)
                  {
                    if (index >= StdEntropyCoderOptions.FIRST_BYPASS_PASS_IDX - 1)
                    {
                      switch ((index + StdEntropyCoderOptions.NUM_EMPTY_PASSES_IN_MS_BP) % StdEntropyCoderOptions.NUM_PASSES)
                      {
                        case 1:
                        case 2:
                          ++length3;
                          continue;
                        default:
                          continue;
                      }
                    }
                  }
                }
              }
              else
                length3 = 1;
              while (pktHeaderBitReader.readBit() != 0)
                ++this.lblock[c][r][sbi][idx.y][idx.x];
              int num10;
              if (length3 == 1)
              {
                num10 = pktHeaderBitReader.readBits(this.lblock[c][r][sbi][idx.y][idx.x] + MathUtil.log2(num5));
              }
              else
              {
                cblkInfo.segLen[l] = new int[length3];
                num10 = 0;
                if ((tileCompVal & StdEntropyCoderOptions.OPT_TERM_PASS) != 0)
                {
                  int num11 = cblkInfo.ctp - num5;
                  int index = 0;
                  while (num11 < cblkInfo.ctp)
                  {
                    int n2 = this.lblock[c][r][sbi][idx.y][idx.x];
                    int num12 = pktHeaderBitReader.readBits(n2);
                    cblkInfo.segLen[l][index] = num12;
                    num10 += num12;
                    ++num11;
                    ++index;
                  }
                }
                else
                {
                  int num13 = cblkInfo.ctp - num5 - 1;
                  int num14 = cblkInfo.ctp - num5;
                  int index = 0;
                  for (; num14 < cblkInfo.ctp - 1; ++num14)
                  {
                    if (num14 >= StdEntropyCoderOptions.FIRST_BYPASS_PASS_IDX - 1 && (num14 + StdEntropyCoderOptions.NUM_EMPTY_PASSES_IN_MS_BP) % StdEntropyCoderOptions.NUM_PASSES != 0)
                    {
                      int num15 = this.lblock[c][r][sbi][idx.y][idx.x];
                      int num16 = pktHeaderBitReader.readBits(num15 + MathUtil.log2(num14 - num13));
                      cblkInfo.segLen[l][index] = num16;
                      num10 += num16;
                      num13 = num14;
                      ++index;
                    }
                  }
                  int num17 = this.lblock[c][r][sbi][idx.y][idx.x];
                  int num18 = pktHeaderBitReader.readBits(num17 + MathUtil.log2(num14 - num13));
                  num10 += num18;
                  cblkInfo.segLen[l][index] = num18;
                }
              }
              cblkInfo.len[l] = num10;
              if (this.isTruncMode)
              {
                if (this.maxCB == -1)
                {
                  if (this.ehs.Pos - pos > nb[tileIdx])
                  {
                    nb[tileIdx] = 0;
                    if (l == 0)
                    {
                      cbI[sbi][idx.y][idx.x] = (CBlkInfo) null;
                    }
                    else
                    {
                      cblkInfo.off[l] = cblkInfo.len[l] = 0;
                      cblkInfo.ctp -= cblkInfo.ntp[l];
                      cblkInfo.ntp[l] = 0;
                      cblkInfo.pktIdx[l] = -1;
                    }
                    return true;
                  }
                }
              }
            }
            catch (EndOfStreamException ex)
            {
              if (l == 0)
              {
                cbI[sbi][idx.y][idx.x] = (CBlkInfo) null;
              }
              else
              {
                cblkInfo.off[l] = cblkInfo.len[l] = 0;
                cblkInfo.ctp -= cblkInfo.ntp[l];
                cblkInfo.ntp[l] = 0;
                cblkInfo.pktIdx[l] = -1;
              }
              return true;
            }
          }
        }
      }
    }
    if (this.ephUsed)
      this.readEPHMarker(pktHeaderBitReader);
    ++this.pktIdx;
    if (this.isTruncMode && this.maxCB == -1)
    {
      int num19 = this.ehs.Pos - pos;
      if (num19 > nb[tileIdx])
      {
        nb[tileIdx] = 0;
        return true;
      }
      nb[tileIdx] -= num19;
    }
    return false;
  }

  public virtual bool readPktBody(int l, int r, int c, int p, CBlkInfo[][][] cbI, int[] nb)
  {
    int pos = this.ehs.Pos;
    bool flag1 = false;
    int tileIdx = this.src.TileIdx;
    bool flag2 = false;
    int num1 = r == 0 ? 0 : 1;
    int num2 = r == 0 ? 1 : 4;
    for (int index = num1; index < num2; ++index)
    {
      if (p < this.ppinfo[c][r].Length)
        flag2 = true;
    }
    if (!flag2)
      return false;
    for (int index1 = num1; index1 < num2; ++index1)
    {
      for (int index2 = 0; index2 < this.cblks[index1].Count; ++index2)
      {
        JPXImageCoordinates idx = ((CBlkCoordInfo) this.cblks[index1][index2]).idx;
        CBlkInfo cblkInfo = cbI[index1][idx.y][idx.x];
        cblkInfo.off[l] = pos;
        pos += cblkInfo.len[l];
        try
        {
          this.ehs.seek(pos);
        }
        catch (EndOfStreamException ex)
        {
          if (l == 0)
          {
            cbI[index1][idx.y][idx.x] = (CBlkInfo) null;
          }
          else
          {
            cblkInfo.off[l] = cblkInfo.len[l] = 0;
            cblkInfo.ctp -= cblkInfo.ntp[l];
            cblkInfo.ntp[l] = 0;
            cblkInfo.pktIdx[l] = -1;
          }
          throw new EndOfStreamException();
        }
        if (this.isTruncMode)
        {
          if (flag1 || cblkInfo.len[l] > nb[tileIdx])
          {
            if (l == 0)
            {
              cbI[index1][idx.y][idx.x] = (CBlkInfo) null;
            }
            else
            {
              cblkInfo.off[l] = cblkInfo.len[l] = 0;
              cblkInfo.ctp -= cblkInfo.ntp[l];
              cblkInfo.ntp[l] = 0;
              cblkInfo.pktIdx[l] = -1;
            }
            flag1 = true;
          }
          if (!flag1)
            nb[tileIdx] -= cblkInfo.len[l];
        }
        if (this.ncbQuit && r == this.rQuit && index1 == this.sQuit && idx.x == this.xQuit && idx.y == this.yQuit && tileIdx == this.tQuit && c == this.cQuit)
        {
          cbI[index1][idx.y][idx.x] = (CBlkInfo) null;
          flag1 = true;
        }
      }
    }
    this.ehs.seek(pos);
    return flag1;
  }

  public int getPPX(int t, int c, int r) => this.decSpec.pss.getPPX(t, c, r);

  public int getPPY(int t, int c, int rl) => this.decSpec.pss.getPPY(t, c, rl);

  public virtual bool readSOPMarker(int[] nBytes, int p, int c, int r)
  {
    byte[] b = new byte[6];
    int tileIdx = this.src.TileIdx;
    int num1 = r == 0 ? 0 : 1;
    int num2 = r == 0 ? 1 : 4;
    bool flag = false;
    for (int index = num1; index < num2; ++index)
    {
      if (p < this.ppinfo[c][r].Length)
        flag = true;
    }
    if (!flag || !this.sopUsed)
      return false;
    int pos = this.ehs.Pos;
    if ((short) ((int) this.ehs.read() << 8 | (int) this.ehs.read()) != (short) -111)
    {
      this.ehs.seek(pos);
      return false;
    }
    this.ehs.seek(pos);
    if (nBytes[tileIdx] < 6)
      return true;
    nBytes[tileIdx] -= 6;
    this.ehs.readFully(b, 0, 6);
    int num3 = (int) b[0] << 8 | (int) b[1];
    int num4 = ((int) b[2] & (int) byte.MaxValue) << 8 | (int) b[3] & (int) byte.MaxValue;
    int num5 = ((int) b[4] & (int) byte.MaxValue) << 8 | (int) b[5] & (int) byte.MaxValue;
    if (!this.pph)
    {
      int pktIdx = this.pktIdx;
    }
    if (this.pph)
    {
      int num6 = this.pktIdx - 1;
    }
    return false;
  }

  public virtual void readEPHMarker(PktHeaderBitReader bin)
  {
    byte[] numArray = new byte[2];
    if (bin.usebais)
      bin.bais.Read(numArray, 0, 2);
    else
      bin.in_Renamed.readFully(numArray, 0, 2);
    int num = (int) numArray[0] << 8 | (int) numArray[1];
  }

  public virtual PrecInfo getPrecInfo(int c, int r, int p) => this.ppinfo[c][r][p];
}
