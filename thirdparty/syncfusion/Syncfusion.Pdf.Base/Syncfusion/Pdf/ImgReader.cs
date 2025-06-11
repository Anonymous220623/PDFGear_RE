// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImgReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class ImgReader : BlockImageDataSource, ImageData
{
  internal int w;
  internal int h;
  internal int nc;

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

  public abstract void close();

  public virtual int getCompSubsX(int c) => 1;

  public virtual int getCompSubsY(int c) => 1;

  public virtual int getTileComponentWidth(int t, int c) => this.w;

  public virtual int getTileComponentHeight(int t, int c) => this.h;

  public virtual int getCompImgWidth(int c) => this.w;

  public virtual int getCompImgHeight(int c) => this.h;

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

  public abstract bool isOrigSigned(int c);

  public abstract int getFixedPoint(int param1);

  public abstract DataBlock getInternCompData(DataBlock param1, int param2);

  public abstract int getNomRangeBits(int param1);

  public abstract DataBlock getCompData(DataBlock param1, int param2);
}
