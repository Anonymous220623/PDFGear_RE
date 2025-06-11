// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.InverseComponetTransformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class InverseComponetTransformation : ImgDataAdapter, BlockImageDataSource, ImageData
{
  public const int NONE = 0;
  public const char OPT_PREFIX = 'M';
  public const int INV_RCT = 1;
  public const int INV_ICT = 2;
  private static readonly string[][] pinfo;
  private BlockImageDataSource src;
  private CompTransfSpec cts;
  private SynWTFilterSpec wfs;
  private int transfType;
  private int[][] outdata = new int[3][];
  private DataBlock block0;
  private DataBlock block1;
  private DataBlock block2;
  private DataBlockInt dbi = new DataBlockInt();
  private int[] utdepth;
  private bool noCompTransf;

  public static string[][] ParameterInfo => InverseComponetTransformation.pinfo;

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

  internal InverseComponetTransformation(
    BlockImageDataSource imgSrc,
    DecodeHelper decSpec,
    int[] utdepth,
    JPXParameters pl)
    : base((ImageData) imgSrc)
  {
    this.cts = decSpec.cts;
    this.wfs = decSpec.wfs;
    this.src = imgSrc;
    this.utdepth = utdepth;
    this.noCompTransf = !pl.getBooleanParameter("comp_transf");
  }

  public override string ToString()
  {
    switch (this.transfType)
    {
      case 0:
        return "No component transformation";
      case 1:
        return "Inverse RCT";
      case 2:
        return "Inverse ICT";
      default:
        throw new ArgumentException("Non JPEG 2000 part I component transformation");
    }
  }

  public virtual int getFixedPoint(int c) => this.src.getFixedPoint(c);

  public static int[] calcMixedBitDepths(int[] utdepth, int ttype, int[] tdepth)
  {
    if (utdepth.Length < 3 && ttype != 0)
      throw new ArgumentException();
    if (tdepth == null)
      tdepth = new int[utdepth.Length];
    switch (ttype)
    {
      case 0:
        Array.Copy((Array) utdepth, 0, (Array) tdepth, 0, utdepth.Length);
        break;
      case 1:
        if (utdepth.Length > 3)
          Array.Copy((Array) utdepth, 3, (Array) tdepth, 3, utdepth.Length - 3);
        tdepth[0] = MathUtil.log2((1 << utdepth[0]) + (2 << utdepth[1]) + (1 << utdepth[2]) - 1) - 2 + 1;
        tdepth[1] = MathUtil.log2((1 << utdepth[2]) + (1 << utdepth[1]) - 1) + 1;
        tdepth[2] = MathUtil.log2((1 << utdepth[0]) + (1 << utdepth[1]) - 1) + 1;
        break;
      case 2:
        if (utdepth.Length > 3)
          Array.Copy((Array) utdepth, 3, (Array) tdepth, 3, utdepth.Length - 3);
        tdepth[0] = MathUtil.log2((int) Math.Floor((double) (1 << utdepth[0]) * 0.299072 + (double) (1 << utdepth[1]) * 0.586914 + (double) (1 << utdepth[2]) * 0.114014) - 1) + 1;
        tdepth[1] = MathUtil.log2((int) Math.Floor((double) (1 << utdepth[0]) * 0.168701 + (double) (1 << utdepth[1]) * 0.331299 + (double) (1 << utdepth[2]) * 0.5) - 1) + 1;
        tdepth[2] = MathUtil.log2((int) Math.Floor((double) (1 << utdepth[0]) * 0.5 + (double) (1 << utdepth[1]) * 0.418701 + (double) (1 << utdepth[2]) * 0.081299) - 1) + 1;
        break;
    }
    return tdepth;
  }

  public override int getNomRangeBits(int c) => this.utdepth[c];

  public virtual DataBlock getCompData(DataBlock blk, int c)
  {
    return c >= 3 || this.transfType == 0 || this.noCompTransf ? this.src.getCompData(blk, c) : this.getInternCompData(blk, c);
  }

  public virtual DataBlock getInternCompData(DataBlock blk, int c)
  {
    if (this.noCompTransf)
      return this.src.getInternCompData(blk, c);
    switch (this.transfType)
    {
      case 0:
        return this.src.getInternCompData(blk, c);
      case 1:
        return this.invRCT(blk, c);
      case 2:
        return this.invICT(blk, c);
      default:
        throw new ArgumentException("Non JPEG 2000 part I component transformation");
    }
  }

  private DataBlock invRCT(DataBlock blk, int c)
  {
    if (c >= 3 && c < this.NumComps)
      return this.src.getInternCompData(blk, c);
    if (this.outdata[c] == null || this.dbi.ulx > blk.ulx || this.dbi.uly > blk.uly || this.dbi.ulx + this.dbi.w < blk.ulx + blk.w || this.dbi.uly + this.dbi.h < blk.uly + blk.h)
    {
      int w = blk.w;
      int h = blk.h;
      this.outdata[c] = (int[]) blk.Data;
      if (this.outdata[c] == null || this.outdata[c].Length != h * w)
      {
        this.outdata[c] = new int[h * w];
        blk.Data = (object) this.outdata[c];
      }
      this.outdata[(c + 1) % 3] = new int[this.outdata[c].Length];
      this.outdata[(c + 2) % 3] = new int[this.outdata[c].Length];
      if (this.block0 == null || this.block0.DataType != 3)
        this.block0 = (DataBlock) new DataBlockInt();
      if (this.block1 == null || this.block1.DataType != 3)
        this.block1 = (DataBlock) new DataBlockInt();
      if (this.block2 == null || this.block2.DataType != 3)
        this.block2 = (DataBlock) new DataBlockInt();
      this.block0.w = this.block1.w = this.block2.w = blk.w;
      this.block0.h = this.block1.h = this.block2.h = blk.h;
      this.block0.ulx = this.block1.ulx = this.block2.ulx = blk.ulx;
      this.block0.uly = this.block1.uly = this.block2.uly = blk.uly;
      this.block0 = this.src.getInternCompData(this.block0, 0);
      int[] data1 = (int[]) this.block0.Data;
      this.block1 = this.src.getInternCompData(this.block1, 1);
      int[] data2 = (int[]) this.block1.Data;
      this.block2 = this.src.getInternCompData(this.block2, 2);
      int[] data3 = (int[]) this.block2.Data;
      blk.progressive = this.block0.progressive || this.block1.progressive || this.block2.progressive;
      blk.offset = 0;
      blk.scanw = w;
      this.dbi.progressive = blk.progressive;
      this.dbi.ulx = blk.ulx;
      this.dbi.uly = blk.uly;
      this.dbi.w = blk.w;
      this.dbi.h = blk.h;
      int index1 = w * h - 1;
      int index2 = this.block0.offset + (h - 1) * this.block0.scanw + w - 1;
      int index3 = this.block1.offset + (h - 1) * this.block1.scanw + w - 1;
      int index4 = this.block2.offset + (h - 1) * this.block2.scanw + w - 1;
      for (int index5 = h - 1; index5 >= 0; --index5)
      {
        int num = index1 - w;
        while (index1 > num)
        {
          this.outdata[1][index1] = data1[index2] - (data2[index3] + data3[index4] >> 2);
          this.outdata[0][index1] = data3[index4] + this.outdata[1][index1];
          this.outdata[2][index1] = data2[index3] + this.outdata[1][index1];
          --index1;
          --index2;
          --index3;
          --index4;
        }
        index2 -= this.block0.scanw - w;
        index3 -= this.block1.scanw - w;
        index4 -= this.block2.scanw - w;
      }
      this.outdata[c] = (int[]) null;
    }
    else
    {
      blk.Data = c >= 0 && c < 3 ? (object) this.outdata[c] : throw new ArgumentException();
      blk.progressive = this.dbi.progressive;
      blk.offset = (blk.uly - this.dbi.uly) * this.dbi.w + blk.ulx - this.dbi.ulx;
      blk.scanw = this.dbi.w;
      this.outdata[c] = (int[]) null;
    }
    return blk;
  }

  private DataBlock invICT(DataBlock blk, int c)
  {
    if (c >= 3 && c < this.NumComps)
    {
      int w = blk.w;
      int h = blk.h;
      int[] numArray = (int[]) blk.Data;
      if (numArray == null)
      {
        numArray = new int[h * w];
        blk.Data = (object) numArray;
      }
      DataBlockFloat blk1 = new DataBlockFloat(blk.ulx, blk.uly, w, h);
      this.src.getInternCompData((DataBlock) blk1, c);
      float[] data = (float[]) blk1.Data;
      int index1 = w * h - 1;
      int index2 = blk1.offset + (h - 1) * blk1.scanw + w - 1;
      for (int index3 = h - 1; index3 >= 0; --index3)
      {
        int num = index1 - w;
        while (index1 > num)
        {
          numArray[index1] = (int) data[index2];
          --index1;
          --index2;
        }
        index2 -= blk1.scanw - w;
      }
      blk.progressive = blk1.progressive;
      blk.offset = 0;
      blk.scanw = w;
    }
    else if (this.outdata[c] == null || this.dbi.ulx > blk.ulx || this.dbi.uly > blk.uly || this.dbi.ulx + this.dbi.w < blk.ulx + blk.w || this.dbi.uly + this.dbi.h < blk.uly + blk.h)
    {
      int w = blk.w;
      int h = blk.h;
      this.outdata[c] = (int[]) blk.Data;
      if (this.outdata[c] == null || this.outdata[c].Length != w * h)
      {
        this.outdata[c] = new int[h * w];
        blk.Data = (object) this.outdata[c];
      }
      this.outdata[(c + 1) % 3] = new int[this.outdata[c].Length];
      this.outdata[(c + 2) % 3] = new int[this.outdata[c].Length];
      if (this.block0 == null || this.block0.DataType != 4)
        this.block0 = (DataBlock) new DataBlockFloat();
      if (this.block2 == null || this.block2.DataType != 4)
        this.block2 = (DataBlock) new DataBlockFloat();
      if (this.block1 == null || this.block1.DataType != 4)
        this.block1 = (DataBlock) new DataBlockFloat();
      this.block0.w = this.block2.w = this.block1.w = blk.w;
      this.block0.h = this.block2.h = this.block1.h = blk.h;
      this.block0.ulx = this.block2.ulx = this.block1.ulx = blk.ulx;
      this.block0.uly = this.block2.uly = this.block1.uly = blk.uly;
      this.block0 = this.src.getInternCompData(this.block0, 0);
      float[] data1 = (float[]) this.block0.Data;
      this.block2 = this.src.getInternCompData(this.block2, 1);
      float[] data2 = (float[]) this.block2.Data;
      this.block1 = this.src.getInternCompData(this.block1, 2);
      float[] data3 = (float[]) this.block1.Data;
      blk.progressive = this.block0.progressive || this.block1.progressive || this.block2.progressive;
      blk.offset = 0;
      blk.scanw = w;
      this.dbi.progressive = blk.progressive;
      this.dbi.ulx = blk.ulx;
      this.dbi.uly = blk.uly;
      this.dbi.w = blk.w;
      this.dbi.h = blk.h;
      int index4 = w * h - 1;
      int index5 = this.block0.offset + (h - 1) * this.block0.scanw + w - 1;
      int index6 = this.block2.offset + (h - 1) * this.block2.scanw + w - 1;
      int index7 = this.block1.offset + (h - 1) * this.block1.scanw + w - 1;
      for (int index8 = h - 1; index8 >= 0; --index8)
      {
        int num = index4 - w;
        while (index4 > num)
        {
          this.outdata[0][index4] = (int) ((double) data1[index5] + 1.4019999504089355 * (double) data3[index7] + 0.5);
          this.outdata[1][index4] = (int) ((double) data1[index5] - 0.3441300094127655 * (double) data2[index6] - 0.714139997959137 * (double) data3[index7] + 0.5);
          this.outdata[2][index4] = (int) ((double) data1[index5] + 1.7719999551773071 * (double) data2[index6] + 0.5);
          --index4;
          --index5;
          --index6;
          --index7;
        }
        index5 -= this.block0.scanw - w;
        index6 -= this.block2.scanw - w;
        index7 -= this.block1.scanw - w;
      }
      this.outdata[c] = (int[]) null;
    }
    else
    {
      blk.Data = c >= 0 && c <= 3 ? (object) this.outdata[c] : throw new ArgumentException();
      blk.progressive = this.dbi.progressive;
      blk.offset = (blk.uly - this.dbi.uly) * this.dbi.w + blk.ulx - this.dbi.ulx;
      blk.scanw = this.dbi.w;
      this.outdata[c] = (int[]) null;
    }
    return blk;
  }

  public override void setTile(int x, int y)
  {
    this.src.setTile(x, y);
    this.tIdx = this.TileIdx;
    if ((int) this.cts.getTileDef(this.tIdx) == 0)
    {
      this.transfType = 0;
    }
    else
    {
      int num1 = this.src.NumComps > 3 ? 3 : this.src.NumComps;
      int num2 = 0;
      for (int c = 0; c < num1; ++c)
        num2 += this.wfs.isReversible(this.tIdx, c) ? 1 : 0;
      if (num2 == 3)
      {
        this.transfType = 1;
      }
      else
      {
        if (num2 != 0)
          throw new ArgumentException("Wavelet transformation and component transformation not coherent in tile" + (object) this.tIdx);
        this.transfType = 2;
      }
    }
  }

  public override void nextTile()
  {
    this.src.nextTile();
    this.tIdx = this.TileIdx;
    if ((int) this.cts.getTileDef(this.tIdx) == 0)
    {
      this.transfType = 0;
    }
    else
    {
      int num1 = this.src.NumComps > 3 ? 3 : this.src.NumComps;
      int num2 = 0;
      for (int c = 0; c < num1; ++c)
        num2 += this.wfs.isReversible(this.tIdx, c) ? 1 : 0;
      if (num2 == 3)
      {
        this.transfType = 1;
      }
      else
      {
        if (num2 != 0)
          throw new ArgumentException("Wavelet transformation and component transformation not coherent in tile" + (object) this.tIdx);
        this.transfType = 2;
      }
    }
  }
}
