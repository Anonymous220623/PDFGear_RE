// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.BitstreamReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class BitstreamReader : CodedCBlkDataSrcDec, InvWTData, MultiResImgData
{
  public const char OPT_PREFIX = 'B';
  internal DecodeHelper decSpec;
  internal bool[] derived;
  internal int[] gb;
  internal StdDequantizerParams[] params_Renamed;
  private static readonly string[][] pinfo;
  internal int[] mdl;
  internal int nc;
  internal int targetRes;
  internal SubbandSyn[] subbTrees;
  internal int imgW;
  internal int imgH;
  internal int ax;
  internal int ay;
  internal int px;
  internal int py;
  internal int[] offX;
  internal int[] offY;
  internal int[] culx;
  internal int[] culy;
  internal int ntW;
  internal int ntH;
  internal int ntX;
  internal int ntY;
  internal int nt;
  internal int ctX;
  internal int ctY;
  internal HeaderDecoder hd;
  internal int tnbytes;
  internal int anbytes;
  internal float trate;
  internal float arate;

  public virtual int CbULX => this.hd.CbULX;

  public virtual int CbULY => this.hd.CbULY;

  public virtual int NumComps => this.nc;

  public virtual int TileIdx => this.ctY * this.ntX + this.ctX;

  public static string[][] ParameterInfo => BitstreamReader.pinfo;

  public virtual int ImgRes => this.targetRes;

  public virtual float TargetRate => this.trate;

  public virtual float ActualRate
  {
    get
    {
      this.arate = (float) this.anbytes * 8f / (float) this.hd.MaxCompImgWidth / (float) this.hd.MaxCompImgHeight;
      return this.arate;
    }
  }

  public virtual int TargetNbytes => this.tnbytes;

  public virtual int ActualNbytes => this.anbytes;

  public virtual int TilePartULX => this.hd.getTilingOrigin((JPXImageCoordinates) null).x;

  public virtual int TilePartULY => this.hd.getTilingOrigin((JPXImageCoordinates) null).y;

  public virtual int NomTileWidth => this.hd.NomTileWidth;

  public virtual int NomTileHeight => this.hd.NomTileHeight;

  internal BitstreamReader(HeaderDecoder hd, DecodeHelper decSpec)
  {
    this.decSpec = decSpec;
    this.hd = hd;
    this.nc = hd.NumComps;
    this.offX = new int[this.nc];
    this.offY = new int[this.nc];
    this.culx = new int[this.nc];
    this.culy = new int[this.nc];
    this.imgW = hd.ImgWidth;
    this.imgH = hd.ImgHeight;
    this.ax = hd.ImgULX;
    this.ay = hd.ImgULY;
    JPXImageCoordinates tilingOrigin = hd.getTilingOrigin((JPXImageCoordinates) null);
    this.px = tilingOrigin.x;
    this.py = tilingOrigin.y;
    this.ntW = hd.NomTileWidth;
    this.ntH = hd.NomTileHeight;
    this.ntX = (this.ax + this.imgW - this.px + this.ntW - 1) / this.ntW;
    this.ntY = (this.ay + this.imgH - this.py + this.ntH - 1) / this.ntH;
    this.nt = this.ntX * this.ntY;
  }

  public int getCompSubsX(int c) => this.hd.getCompSubsX(c);

  public virtual int getCompSubsY(int c) => this.hd.getCompSubsY(c);

  public virtual int getTileWidth(int rl)
  {
    int minInTile = this.decSpec.dls.getMinInTile(this.TileIdx);
    if (rl > minInTile)
      throw new ArgumentException($"Requested resolution level is not available for, at least, one component in tile: {(object) this.ctX}x{(object) this.ctY}");
    int num1 = minInTile - rl;
    int num2 = this.ctX == 0 ? this.ax : this.px + this.ctX * this.ntW;
    return ((this.ctX < this.ntX - 1 ? this.px + (this.ctX + 1) * this.ntW : this.ax + this.imgW) + (1 << num1) - 1) / (1 << num1) - (num2 + (1 << num1) - 1) / (1 << num1);
  }

  public virtual int getTileHeight(int rl)
  {
    int minInTile = this.decSpec.dls.getMinInTile(this.TileIdx);
    if (rl > minInTile)
      throw new ArgumentException($"Requested resolution level is not available for, at least, one component in tile: {(object) this.ctX}x{(object) this.ctY}");
    int num1 = minInTile - rl;
    int num2 = this.ctY == 0 ? this.ay : this.py + this.ctY * this.ntH;
    return ((this.ctY < this.ntY - 1 ? this.py + (this.ctY + 1) * this.ntH : this.ay + this.imgH) + (1 << num1) - 1) / (1 << num1) - (num2 + (1 << num1) - 1) / (1 << num1);
  }

  public virtual int getImgWidth(int rl)
  {
    int min = this.decSpec.dls.Min;
    if (rl > min)
      throw new ArgumentException("Requested resolution level is not available for, at least, one tile-component");
    int num = min - rl;
    return (this.ax + this.imgW + (1 << num) - 1) / (1 << num) - (this.ax + (1 << num) - 1) / (1 << num);
  }

  public virtual int getImgHeight(int rl)
  {
    int min = this.decSpec.dls.Min;
    if (rl > min)
      throw new ArgumentException("Requested resolution level is not available for, at least, one tile-component");
    int num = min - rl;
    return (this.ay + this.imgH + (1 << num) - 1) / (1 << num) - (this.ay + (1 << num) - 1) / (1 << num);
  }

  public virtual int getImgULX(int rl)
  {
    int min = this.decSpec.dls.Min;
    if (rl > min)
      throw new ArgumentException("Requested resolution level is not available for, at least, one tile-component");
    int num = min - rl;
    return (this.ax + (1 << num) - 1) / (1 << num);
  }

  public virtual int getImgULY(int rl)
  {
    int min = this.decSpec.dls.Min;
    if (rl > min)
      throw new ArgumentException("Requested resolution level is not available for, at least, one tile-component");
    int num = min - rl;
    return (this.ay + (1 << num) - 1) / (1 << num);
  }

  public int getTileCompWidth(int t, int c, int rl)
  {
    int tileIdx = this.TileIdx;
    int num = this.mdl[c] - rl;
    return (((this.ctX < this.ntX - 1 ? this.px + (this.ctX + 1) * this.ntW : this.ax + this.imgW) + this.hd.getCompSubsX(c) - 1) / this.hd.getCompSubsX(c) + (1 << num) - 1) / (1 << num) - (this.culx[c] + (1 << num) - 1) / (1 << num);
  }

  public int getTileCompHeight(int t, int c, int rl)
  {
    int tileIdx = this.TileIdx;
    int num = this.mdl[c] - rl;
    return (((this.ctY < this.ntY - 1 ? this.py + (this.ctY + 1) * this.ntH : this.ay + this.imgH) + this.hd.getCompSubsY(c) - 1) / this.hd.getCompSubsY(c) + (1 << num) - 1) / (1 << num) - (this.culy[c] + (1 << num) - 1) / (1 << num);
  }

  public int getCompImgWidth(int c, int rl)
  {
    int num1 = this.decSpec.dls.getMinInComp(c) - rl;
    int num2 = (this.ax + this.hd.getCompSubsX(c) - 1) / this.hd.getCompSubsX(c);
    return ((this.ax + this.imgW + this.hd.getCompSubsX(c) - 1) / this.hd.getCompSubsX(c) + (1 << num1) - 1) / (1 << num1) - (num2 + (1 << num1) - 1) / (1 << num1);
  }

  public int getCompImgHeight(int c, int rl)
  {
    int num1 = this.decSpec.dls.getMinInComp(c) - rl;
    int num2 = (this.ay + this.hd.getCompSubsY(c) - 1) / this.hd.getCompSubsY(c);
    return ((this.ay + this.imgH + this.hd.getCompSubsY(c) - 1) / this.hd.getCompSubsY(c) + (1 << num1) - 1) / (1 << num1) - (num2 + (1 << num1) - 1) / (1 << num1);
  }

  public abstract void setTile(int x, int y);

  public abstract void nextTile();

  public JPXImageCoordinates getTile(JPXImageCoordinates co)
  {
    if (co == null)
      return new JPXImageCoordinates(this.ctX, this.ctY);
    co.x = this.ctX;
    co.y = this.ctY;
    return co;
  }

  public int getResULX(int c, int rl)
  {
    int num = this.mdl[c] - rl;
    if (num < 0)
      throw new ArgumentException($"Requested resolution level is not available for, at least, one component in tile: {(object) this.ctX}x{(object) this.ctY}");
    return (int) Math.Ceiling((double) (int) Math.Ceiling((double) Math.Max(this.px + this.ctX * this.ntW, this.ax) / (double) this.getCompSubsX(c)) / (double) (1 << num));
  }

  public int getResULY(int c, int rl)
  {
    int num = this.mdl[c] - rl;
    if (num < 0)
      throw new ArgumentException($"Requested resolution level is not available for, at least, one component in tile: {(object) this.ctX}x{(object) this.ctY}");
    return (int) Math.Ceiling((double) (int) Math.Ceiling((double) Math.Max(this.py + this.ctY * this.ntH, this.ay) / (double) this.getCompSubsY(c)) / (double) (1 << num));
  }

  public JPXImageCoordinates getNumTiles(JPXImageCoordinates co)
  {
    if (co == null)
      return new JPXImageCoordinates(this.ntX, this.ntY);
    co.x = this.ntX;
    co.y = this.ntY;
    return co;
  }

  public int getNumTiles() => this.ntX * this.ntY;

  public SubbandSyn getSynSubbandTree(int t, int c)
  {
    if (t != this.TileIdx)
      throw new ArgumentException("Can not request subband tree of a different tile than the current one");
    return c >= 0 && c < this.nc ? this.subbTrees[c] : throw new ArgumentException("Component index out of range");
  }

  internal static BitstreamReader createInstance(
    JPXRandomAccessStream in_Renamed,
    HeaderDecoder hd,
    JPXParameters pl,
    DecodeHelper decSpec,
    bool cdstrInfo,
    HeaderInformation hi)
  {
    pl.checkList('B', JPXParameters.toNameArray(BitstreamReader.ParameterInfo));
    return (BitstreamReader) new FileBitstreamReaderAgent(hd, in_Renamed, decSpec, pl, cdstrInfo, hi);
  }

  public int getPPX(int t, int c, int rl) => this.decSpec.pss.getPPX(t, c, rl);

  public int getPPY(int t, int c, int rl) => this.decSpec.pss.getPPY(t, c, rl);

  internal virtual void initSubbandsFields(int c, SubbandSyn sb)
  {
    int tileIdx = this.TileIdx;
    int resLvl = sb.resLvl;
    int cblkWidth = this.decSpec.cblks.getCBlkWidth((byte) 3, tileIdx, c);
    int cblkHeight = this.decSpec.cblks.getCBlkHeight((byte) 3, tileIdx, c);
    if (!sb.isNode)
    {
      if (this.hd.precinctPartitionUsed())
      {
        int num1 = MathUtil.log2(this.getPPX(tileIdx, c, resLvl));
        int num2 = MathUtil.log2(this.getPPY(tileIdx, c, resLvl));
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
      if (sb.w == 0 || sb.h == 0)
      {
        sb.numCb.x = 0;
        sb.numCb.y = 0;
      }
      else
      {
        int cbUlx = this.CbULX;
        int cbUly = this.CbULY;
        int num5 = cbUlx;
        int num6 = cbUly;
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
      if (this.derived[c])
        sb.magbits = this.gb[c] + (this.params_Renamed[c].exp[0][0] - (this.mdl[c] - sb.level)) - 1;
      else
        sb.magbits = this.gb[c] + this.params_Renamed[c].exp[sb.resLvl][sb.sbandIdx] - 1;
    }
    else
    {
      this.initSubbandsFields(c, (SubbandSyn) sb.LL);
      this.initSubbandsFields(c, (SubbandSyn) sb.HL);
      this.initSubbandsFields(c, (SubbandSyn) sb.LH);
      this.initSubbandsFields(c, (SubbandSyn) sb.HH);
    }
  }

  public abstract DecLyrdCBlk getCodeBlock(
    int param1,
    int param2,
    int param3,
    SubbandSyn param4,
    int param5,
    int param6,
    DecLyrdCBlk param7);
}
