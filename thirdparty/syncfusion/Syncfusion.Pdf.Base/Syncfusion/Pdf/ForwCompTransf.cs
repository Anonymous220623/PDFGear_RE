// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ForwCompTransf
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class ForwCompTransf : ImgDataAdapter, BlockImageDataSource, ImageData
{
  public const int NONE = 0;
  public const int FORW_RCT = 1;
  public const int FORW_ICT = 2;
  internal const char OPT_PREFIX = 'M';
  private BlockImageDataSource src;
  private CompTransfSpec cts;
  private AnWTFilterSpec wfs;
  private int transfType;
  private int[] tdepth;
  private DataBlock outBlk;
  private DataBlockInt block0;
  private DataBlockInt block1;
  private DataBlockInt block2;
  private static readonly string[][] pinfo = new string[1][]
  {
    new string[4]
    {
      "Mct",
      "[<tile index>] [on|off] ...",
      "Specifies in which tiles to use a multiple component transform. Note that this multiple component transform can only be applied in tiles that contain at least three components and whose components are processed with the same wavelet filters and quantization type. If the wavelet transform is reversible (w5x3 filter), the Reversible Component Transformation (RCT) is applied. If not (w9x7 filter), the Irreversible Component Transformation (ICT) is used.",
      null
    }
  };

  public static string[][] ParameterInfo => ForwCompTransf.pinfo;

  public virtual bool Reversible
  {
    get
    {
      switch (this.transfType)
      {
        case 0:
        case 1:
          return true;
        case 2:
          return false;
        default:
          throw new ArgumentException("Non JPEG 2000 part I component transformation");
      }
    }
  }

  internal ForwCompTransf(BlockImageDataSource imgSrc, EncoderSpecs encSpec)
    : base((ImageData) imgSrc)
  {
    this.cts = encSpec.cts;
    this.wfs = encSpec.wfs;
    this.src = imgSrc;
  }

  public virtual int getFixedPoint(int c) => this.src.getFixedPoint(c);

  public static int[] calcMixedBitDepths(int[] ntdepth, int ttype, int[] tdepth)
  {
    if (ntdepth.Length < 3 && ttype != 0)
      throw new ArgumentException();
    if (tdepth == null)
      tdepth = new int[ntdepth.Length];
    switch (ttype)
    {
      case 0:
        Array.Copy((Array) ntdepth, 0, (Array) tdepth, 0, ntdepth.Length);
        break;
      case 1:
        if (ntdepth.Length > 3)
          Array.Copy((Array) ntdepth, 3, (Array) tdepth, 3, ntdepth.Length - 3);
        tdepth[0] = MathUtil.log2((1 << ntdepth[0]) + (2 << ntdepth[1]) + (1 << ntdepth[2]) - 1) - 2 + 1;
        tdepth[1] = MathUtil.log2((1 << ntdepth[2]) + (1 << ntdepth[1]) - 1) + 1;
        tdepth[2] = MathUtil.log2((1 << ntdepth[0]) + (1 << ntdepth[1]) - 1) + 1;
        break;
      case 2:
        if (ntdepth.Length > 3)
          Array.Copy((Array) ntdepth, 3, (Array) tdepth, 3, ntdepth.Length - 3);
        tdepth[0] = MathUtil.log2((int) Math.Floor((double) (1 << ntdepth[0]) * 0.299072 + (double) (1 << ntdepth[1]) * 0.586914 + (double) (1 << ntdepth[2]) * 0.114014) - 1) + 1;
        tdepth[1] = MathUtil.log2((int) Math.Floor((double) (1 << ntdepth[0]) * 0.168701 + (double) (1 << ntdepth[1]) * 0.331299 + (double) (1 << ntdepth[2]) * 0.5) - 1) + 1;
        tdepth[2] = MathUtil.log2((int) Math.Floor((double) (1 << ntdepth[0]) * 0.5 + (double) (1 << ntdepth[1]) * 0.418701 + (double) (1 << ntdepth[2]) * 0.081299) - 1) + 1;
        break;
    }
    return tdepth;
  }

  private void initForwRCT()
  {
    int tileIdx = this.TileIdx;
    if (this.src.NumComps < 3)
      throw new ArgumentException();
    int[] ntdepth = this.src.getTileComponentWidth(tileIdx, 0) == this.src.getTileComponentWidth(tileIdx, 1) && this.src.getTileComponentWidth(tileIdx, 0) == this.src.getTileComponentWidth(tileIdx, 2) && this.src.getTileComponentHeight(tileIdx, 0) == this.src.getTileComponentHeight(tileIdx, 1) && this.src.getTileComponentHeight(tileIdx, 0) == this.src.getTileComponentHeight(tileIdx, 2) ? new int[this.src.NumComps] : throw new ArgumentException("Can not use RCT on components with different dimensions");
    for (int c = ntdepth.Length - 1; c >= 0; --c)
      ntdepth[c] = this.src.getNomRangeBits(c);
    this.tdepth = ForwCompTransf.calcMixedBitDepths(ntdepth, 1, (int[]) null);
  }

  private void initForwICT()
  {
    int tileIdx = this.TileIdx;
    if (this.src.NumComps < 3)
      throw new ArgumentException();
    int[] ntdepth = this.src.getTileComponentWidth(tileIdx, 0) == this.src.getTileComponentWidth(tileIdx, 1) && this.src.getTileComponentWidth(tileIdx, 0) == this.src.getTileComponentWidth(tileIdx, 2) && this.src.getTileComponentHeight(tileIdx, 0) == this.src.getTileComponentHeight(tileIdx, 1) && this.src.getTileComponentHeight(tileIdx, 0) == this.src.getTileComponentHeight(tileIdx, 2) ? new int[this.src.NumComps] : throw new ArgumentException("Can not use ICT on components with different dimensions");
    for (int c = ntdepth.Length - 1; c >= 0; --c)
      ntdepth[c] = this.src.getNomRangeBits(c);
    this.tdepth = ForwCompTransf.calcMixedBitDepths(ntdepth, 2, (int[]) null);
  }

  public override string ToString()
  {
    switch (this.transfType)
    {
      case 0:
        return "No component transformation";
      case 1:
        return "Forward RCT";
      case 2:
        return "Forward ICT";
      default:
        throw new ArgumentException("Non JPEG 2000 part I component transformation");
    }
  }

  public override int getNomRangeBits(int c)
  {
    switch (this.transfType)
    {
      case 0:
        return this.src.getNomRangeBits(c);
      case 1:
      case 2:
        return this.tdepth[c];
      default:
        throw new ArgumentException("Non JPEG 2000 part I component transformation");
    }
  }

  public virtual DataBlock getCompData(DataBlock blk, int c)
  {
    return c >= 3 || this.transfType == 0 ? this.src.getCompData(blk, c) : this.getInternCompData(blk, c);
  }

  public virtual DataBlock getInternCompData(DataBlock blk, int c)
  {
    switch (this.transfType)
    {
      case 0:
        return this.src.getInternCompData(blk, c);
      case 1:
        return this.forwRCT(blk, c);
      case 2:
        return this.forwICT(blk, c);
      default:
        throw new ArgumentException("Non JPEG 2000 part 1 component transformation for tile: " + (object) this.tIdx);
    }
  }

  private DataBlock forwRCT(DataBlock blk, int c)
  {
    int w = blk.w;
    int h = blk.h;
    if (c >= 0 && c <= 2)
    {
      if (blk.DataType != 3)
      {
        if (this.outBlk == null || this.outBlk.DataType != 3)
          this.outBlk = (DataBlock) new DataBlockInt();
        this.outBlk.w = w;
        this.outBlk.h = h;
        this.outBlk.ulx = blk.ulx;
        this.outBlk.uly = blk.uly;
        blk = this.outBlk;
      }
      int[] numArray = (int[]) blk.Data;
      if (numArray == null || numArray.Length < h * w)
      {
        numArray = new int[h * w];
        blk.Data = (object) numArray;
      }
      if (this.block0 == null)
        this.block0 = new DataBlockInt();
      if (this.block1 == null)
        this.block1 = new DataBlockInt();
      if (this.block2 == null)
        this.block2 = new DataBlockInt();
      this.block0.w = this.block1.w = this.block2.w = blk.w;
      this.block0.h = this.block1.h = this.block2.h = blk.h;
      this.block0.ulx = this.block1.ulx = this.block2.ulx = blk.ulx;
      this.block0.uly = this.block1.uly = this.block2.uly = blk.uly;
      this.block0 = (DataBlockInt) this.src.getInternCompData((DataBlock) this.block0, 0);
      int[] data1 = (int[]) this.block0.Data;
      this.block1 = (DataBlockInt) this.src.getInternCompData((DataBlock) this.block1, 1);
      int[] data2 = (int[]) this.block1.Data;
      this.block2 = (DataBlockInt) this.src.getInternCompData((DataBlock) this.block2, 2);
      int[] data3 = (int[]) this.block2.Data;
      blk.progressive = this.block0.progressive || this.block1.progressive || this.block2.progressive;
      blk.offset = 0;
      blk.scanw = w;
      int index1 = w * h - 1;
      int index2 = this.block0.offset + (h - 1) * this.block0.scanw + w - 1;
      int index3 = this.block1.offset + (h - 1) * this.block1.scanw + w - 1;
      int index4 = this.block2.offset + (h - 1) * this.block2.scanw + w - 1;
      switch (c)
      {
        case 0:
          for (int index5 = h - 1; index5 >= 0; --index5)
          {
            int num = index1 - w;
            while (index1 > num)
            {
              numArray[index1] = data1[index1] + 2 * data2[index1] + data3[index1] >> 2;
              --index1;
              --index2;
              --index3;
              --index4;
            }
            index2 -= this.block0.scanw - w;
            index3 -= this.block1.scanw - w;
            index4 -= this.block2.scanw - w;
          }
          break;
        case 1:
          for (int index6 = h - 1; index6 >= 0; --index6)
          {
            int num = index1 - w;
            while (index1 > num)
            {
              numArray[index1] = data3[index4] - data2[index3];
              --index1;
              --index3;
              --index4;
            }
            index3 -= this.block1.scanw - w;
            index4 -= this.block2.scanw - w;
          }
          break;
        case 2:
          for (int index7 = h - 1; index7 >= 0; --index7)
          {
            int num = index1 - w;
            while (index1 > num)
            {
              numArray[index1] = data1[index2] - data2[index3];
              --index1;
              --index2;
              --index3;
            }
            index2 -= this.block0.scanw - w;
            index3 -= this.block1.scanw - w;
          }
          break;
      }
      return blk;
    }
    if (c >= 3)
      return this.src.getInternCompData(blk, c);
    throw new ArgumentException();
  }

  private DataBlock forwICT(DataBlock blk, int c)
  {
    int w = blk.w;
    int h = blk.h;
    if (blk.DataType != 4)
    {
      if (this.outBlk == null || this.outBlk.DataType != 4)
        this.outBlk = (DataBlock) new DataBlockFloat();
      this.outBlk.w = w;
      this.outBlk.h = h;
      this.outBlk.ulx = blk.ulx;
      this.outBlk.uly = blk.uly;
      blk = this.outBlk;
    }
    float[] numArray = (float[]) blk.Data;
    if (numArray == null || numArray.Length < w * h)
    {
      numArray = new float[h * w];
      blk.Data = (object) numArray;
    }
    if (c >= 0 && c <= 2)
    {
      if (this.block0 == null)
        this.block0 = new DataBlockInt();
      if (this.block1 == null)
        this.block1 = new DataBlockInt();
      if (this.block2 == null)
        this.block2 = new DataBlockInt();
      this.block0.w = this.block1.w = this.block2.w = blk.w;
      this.block0.h = this.block1.h = this.block2.h = blk.h;
      this.block0.ulx = this.block1.ulx = this.block2.ulx = blk.ulx;
      this.block0.uly = this.block1.uly = this.block2.uly = blk.uly;
      this.block0 = (DataBlockInt) this.src.getInternCompData((DataBlock) this.block0, 0);
      int[] data1 = (int[]) this.block0.Data;
      this.block1 = (DataBlockInt) this.src.getInternCompData((DataBlock) this.block1, 1);
      int[] data2 = (int[]) this.block1.Data;
      this.block2 = (DataBlockInt) this.src.getInternCompData((DataBlock) this.block2, 2);
      int[] data3 = (int[]) this.block2.Data;
      blk.progressive = this.block0.progressive || this.block1.progressive || this.block2.progressive;
      blk.offset = 0;
      blk.scanw = w;
      int index1 = w * h - 1;
      int index2 = this.block0.offset + (h - 1) * this.block0.scanw + w - 1;
      int index3 = this.block1.offset + (h - 1) * this.block1.scanw + w - 1;
      int index4 = this.block2.offset + (h - 1) * this.block2.scanw + w - 1;
      switch (c)
      {
        case 0:
          for (int index5 = h - 1; index5 >= 0; --index5)
          {
            int num = index1 - w;
            while (index1 > num)
            {
              numArray[index1] = (float) (0.29899999499320984 * (double) data1[index2] + 0.58700001239776611 * (double) data2[index3] + 57.0 / 500.0 * (double) data3[index4]);
              --index1;
              --index2;
              --index3;
              --index4;
            }
            index2 -= this.block0.scanw - w;
            index3 -= this.block1.scanw - w;
            index4 -= this.block2.scanw - w;
          }
          break;
        case 1:
          for (int index6 = h - 1; index6 >= 0; --index6)
          {
            int num = index1 - w;
            while (index1 > num)
            {
              numArray[index1] = (float) (-0.16875000298023224 * (double) data1[index2] - 0.33125999569892883 * (double) data2[index3] + 0.5 * (double) data3[index4]);
              --index1;
              --index2;
              --index3;
              --index4;
            }
            index2 -= this.block0.scanw - w;
            index3 -= this.block1.scanw - w;
            index4 -= this.block2.scanw - w;
          }
          break;
        case 2:
          for (int index7 = h - 1; index7 >= 0; --index7)
          {
            int num = index1 - w;
            while (index1 > num)
            {
              numArray[index1] = (float) (0.5 * (double) data1[index2] - 0.41868999600410461 * (double) data2[index3] - 0.081309996545314789 * (double) data3[index4]);
              --index1;
              --index2;
              --index3;
              --index4;
            }
            index2 -= this.block0.scanw - w;
            index3 -= this.block1.scanw - w;
            index4 -= this.block2.scanw - w;
          }
          break;
      }
      return blk;
    }
    if (c < 3)
      throw new ArgumentException();
    DataBlockInt blk1 = new DataBlockInt(blk.ulx, blk.uly, w, h);
    this.src.getInternCompData((DataBlock) blk1, c);
    int[] data = (int[]) blk1.Data;
    int index8 = w * h - 1;
    int index9 = blk1.offset + (h - 1) * blk1.scanw + w - 1;
    for (int index10 = h - 1; index10 >= 0; --index10)
    {
      int num = index8 - w;
      while (index8 > num)
      {
        numArray[index8] = (float) data[index9];
        --index8;
        --index9;
      }
      index9 += blk1.w - w;
    }
    blk.progressive = blk1.progressive;
    blk.offset = 0;
    blk.scanw = w;
    return blk;
  }

  public override void setTile(int x, int y)
  {
    this.src.setTile(x, y);
    this.tIdx = this.TileIdx;
    switch ((string) this.cts.getTileDef(this.tIdx))
    {
      case "none":
        this.transfType = 0;
        break;
      case "rct":
        this.transfType = 1;
        this.initForwRCT();
        break;
      case "ict":
        this.transfType = 2;
        this.initForwICT();
        break;
      default:
        throw new ArgumentException("Component transformation not recognized");
    }
  }

  public override void nextTile()
  {
    this.src.nextTile();
    this.tIdx = this.TileIdx;
    switch ((string) this.cts.getTileDef(this.tIdx))
    {
      case "none":
        this.transfType = 0;
        break;
      case "rct":
        this.transfType = 1;
        this.initForwRCT();
        break;
      case "ict":
        this.transfType = 2;
        this.initForwICT();
        break;
      default:
        throw new ArgumentException("Component transformation not recognized");
    }
  }
}
