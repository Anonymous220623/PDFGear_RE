// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImgDataAdapter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class ImgDataAdapter : ImageData
{
  internal int tIdx;
  internal ImageData imgdatasrc;

  public virtual int TileWidth => this.imgdatasrc.TileWidth;

  public virtual int TileHeight => this.imgdatasrc.TileHeight;

  public virtual int NomTileWidth => this.imgdatasrc.NomTileWidth;

  public virtual int NomTileHeight => this.imgdatasrc.NomTileHeight;

  public virtual int ImgWidth => this.imgdatasrc.ImgWidth;

  public virtual int ImgHeight => this.imgdatasrc.ImgHeight;

  public virtual int NumComps => this.imgdatasrc.NumComps;

  public virtual int TileIdx => this.imgdatasrc.TileIdx;

  public virtual int TilePartULX => this.imgdatasrc.TilePartULX;

  public virtual int TilePartULY => this.imgdatasrc.TilePartULY;

  public virtual int ImgULX => this.imgdatasrc.ImgULX;

  public virtual int ImgULY => this.imgdatasrc.ImgULY;

  internal ImgDataAdapter(ImageData src) => this.imgdatasrc = src;

  public virtual int getCompSubsX(int c) => this.imgdatasrc.getCompSubsX(c);

  public virtual int getCompSubsY(int c) => this.imgdatasrc.getCompSubsY(c);

  public virtual int getTileComponentWidth(int t, int c)
  {
    return this.imgdatasrc.getTileComponentWidth(t, c);
  }

  public virtual int getTileComponentHeight(int t, int c)
  {
    return this.imgdatasrc.getTileComponentHeight(t, c);
  }

  public virtual int getCompImgWidth(int c) => this.imgdatasrc.getCompImgWidth(c);

  public virtual int getCompImgHeight(int c) => this.imgdatasrc.getCompImgHeight(c);

  public virtual int getNomRangeBits(int c) => this.imgdatasrc.getNomRangeBits(c);

  public virtual void setTile(int x, int y)
  {
    this.imgdatasrc.setTile(x, y);
    this.tIdx = this.TileIdx;
  }

  public virtual void nextTile()
  {
    this.imgdatasrc.nextTile();
    this.tIdx = this.TileIdx;
  }

  public virtual JPXImageCoordinates getTile(JPXImageCoordinates co) => this.imgdatasrc.getTile(co);

  public virtual int getCompUpperLeftCornerX(int c) => this.imgdatasrc.getCompUpperLeftCornerX(c);

  public virtual int getCompUpperLeftCornerY(int c) => this.imgdatasrc.getCompUpperLeftCornerY(c);

  public virtual JPXImageCoordinates getNumTiles(JPXImageCoordinates co)
  {
    return this.imgdatasrc.getNumTiles(co);
  }

  public virtual int getNumTiles() => this.imgdatasrc.getNumTiles();
}
