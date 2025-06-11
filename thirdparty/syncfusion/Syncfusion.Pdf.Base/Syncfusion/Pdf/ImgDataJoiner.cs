// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImgDataJoiner
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class ImgDataJoiner : BlockImageDataSource, ImageData
{
  private int w;
  private int h;
  private int nc;
  private BlockImageDataSource[] imageData;
  private int[] compIdx;
  private int[] subsX;
  private int[] subsY;

  public virtual int TileWidth => this.w;

  public virtual int TileHeight => this.h;

  public virtual int NomTileWidth => this.w;

  public virtual int NomTileHeight => this.h;

  public virtual int ImgWidth => this.w;

  public virtual int ImgHeight => this.h;

  public virtual int NumComps => this.nc;

  public virtual int TileIdx => 0;

  public virtual int TilePartULX => 0;

  public virtual int TilePartULY => 0;

  public virtual int ImgULX => 0;

  public virtual int ImgULY => 0;

  internal ImgDataJoiner(BlockImageDataSource[] imD, int[] cIdx)
  {
    this.imageData = imD;
    this.compIdx = cIdx;
    if (this.imageData.Length != this.compIdx.Length)
      throw new ArgumentException("imD and cIdx must have the same length");
    this.nc = imD.Length;
    this.subsX = new int[this.nc];
    this.subsY = new int[this.nc];
    for (int index = 0; index < this.nc; ++index)
    {
      if (imD[index].getNumTiles() != 1 || imD[index].getCompUpperLeftCornerX(cIdx[index]) != 0 || imD[index].getCompUpperLeftCornerY(cIdx[index]) != 0)
        throw new ArgumentException("All input components must, not use tiles and must have the origin at the canvas origin");
    }
    int num1 = 0;
    int num2 = 0;
    for (int index = 0; index < this.nc; ++index)
    {
      if (imD[index].getCompImgWidth(cIdx[index]) > num1)
        num1 = imD[index].getCompImgWidth(cIdx[index]);
      if (imD[index].getCompImgHeight(cIdx[index]) > num2)
        num2 = imD[index].getCompImgHeight(cIdx[index]);
    }
    this.w = num1;
    this.h = num2;
    for (int index = 0; index < this.nc; ++index)
    {
      this.subsX[index] = (num1 + imD[index].getCompImgWidth(cIdx[index]) - 1) / imD[index].getCompImgWidth(cIdx[index]);
      this.subsY[index] = (num2 + imD[index].getCompImgHeight(cIdx[index]) - 1) / imD[index].getCompImgHeight(cIdx[index]);
      if ((num1 + this.subsX[index] - 1) / this.subsX[index] == imD[index].getCompImgWidth(cIdx[index]))
      {
        int num3 = (num2 + this.subsY[index] - 1) / this.subsY[index];
        imD[index].getCompImgHeight(cIdx[index]);
      }
    }
  }

  public virtual int getCompSubsX(int c) => this.subsX[c];

  public virtual int getCompSubsY(int c) => this.subsY[c];

  public virtual int getTileComponentWidth(int t, int c)
  {
    return this.imageData[c].getTileComponentWidth(t, this.compIdx[c]);
  }

  public virtual int getTileComponentHeight(int t, int c)
  {
    return this.imageData[c].getTileComponentHeight(t, this.compIdx[c]);
  }

  public virtual int getCompImgWidth(int c) => this.imageData[c].getCompImgWidth(this.compIdx[c]);

  public virtual int getCompImgHeight(int n) => this.imageData[n].getCompImgHeight(this.compIdx[n]);

  public virtual int getNomRangeBits(int c) => this.imageData[c].getNomRangeBits(this.compIdx[c]);

  public virtual int getFixedPoint(int c) => this.imageData[c].getFixedPoint(this.compIdx[c]);

  public virtual DataBlock getInternCompData(DataBlock blk, int c)
  {
    return this.imageData[c].getInternCompData(blk, this.compIdx[c]);
  }

  public virtual DataBlock getCompData(DataBlock blk, int c)
  {
    return this.imageData[c].getCompData(blk, this.compIdx[c]);
  }

  public virtual void setTile(int x, int y)
  {
    if (x != 0 || y != 0)
      throw new ArgumentException();
  }

  public virtual void nextTile() => throw new Exception();

  public virtual JPXImageCoordinates getTile(JPXImageCoordinates co)
  {
    if (co == null)
      return new JPXImageCoordinates(0, 0);
    co.x = 0;
    co.y = 0;
    return co;
  }

  public virtual int getCompUpperLeftCornerX(int c) => 0;

  public virtual int getCompUpperLeftCornerY(int c) => 0;

  public virtual JPXImageCoordinates getNumTiles(JPXImageCoordinates co)
  {
    if (co == null)
      return new JPXImageCoordinates(1, 1);
    co.x = 1;
    co.y = 1;
    return co;
  }

  public virtual int getNumTiles() => 1;

  public override string ToString()
  {
    string str = $"ImgDataJoiner: WxH = {(object) this.w}x{(object) this.h}";
    for (int index = 0; index < this.nc; ++index)
      str = $"{str}\n- Component {(object) index} {(object) this.imageData[index]}";
    return str;
  }
}
