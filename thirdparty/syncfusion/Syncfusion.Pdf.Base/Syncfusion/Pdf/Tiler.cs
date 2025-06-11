// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tiler
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class Tiler : ImgDataAdapter, BlockImageDataSource, ImageData
{
  private BlockImageDataSource src;
  private int x0siz;
  private int y0siz;
  private int xt0siz;
  private int yt0siz;
  private int xtsiz;
  private int ytsiz;
  private int ntX;
  private int ntY;
  private int[] compW;
  private int[] compH;
  private int[] tcx0;
  private int[] tcy0;
  private int tx;
  private int ty;
  private int tileW;
  private int tileH;

  public override int TileWidth => this.tileW;

  public override int TileHeight => this.tileH;

  public override int TileIdx => this.ty * this.ntX + this.tx;

  public override int TilePartULX => this.xt0siz;

  public override int TilePartULY => this.yt0siz;

  public override int ImgULX => this.x0siz;

  public override int ImgULY => this.y0siz;

  public override int NomTileWidth => this.xtsiz;

  public override int NomTileHeight => this.ytsiz;

  internal Tiler(BlockImageDataSource src, int ax, int ay, int px, int py, int nw, int nh)
    : base((ImageData) src)
  {
    this.src = src;
    this.x0siz = ax;
    this.y0siz = ay;
    this.xt0siz = px;
    this.yt0siz = py;
    this.xtsiz = nw;
    this.ytsiz = nh;
    if (src.getNumTiles() != 1)
      throw new ArgumentException("Source is tiled");
    if (src.ImgULX != 0 || src.ImgULY != 0)
      throw new ArgumentException("Source is \"canvased\"");
    if (this.x0siz < 0 || this.y0siz < 0 || this.xt0siz < 0 || this.yt0siz < 0 || this.xtsiz < 0 || this.ytsiz < 0 || this.xt0siz > this.x0siz || this.yt0siz > this.y0siz)
      throw new ArgumentException("Invalid image origin, tiling origin or nominal tile size");
    if (this.xtsiz == 0)
      this.xtsiz = this.x0siz + src.ImgWidth - this.xt0siz;
    if (this.ytsiz == 0)
      this.ytsiz = this.y0siz + src.ImgHeight - this.yt0siz;
    if (this.x0siz - this.xt0siz >= this.xtsiz)
      this.xt0siz += (this.x0siz - this.xt0siz) / this.xtsiz * this.xtsiz;
    if (this.y0siz - this.yt0siz >= this.ytsiz)
      this.yt0siz += (this.y0siz - this.yt0siz) / this.ytsiz * this.ytsiz;
    if (this.x0siz - this.xt0siz < this.xtsiz)
    {
      int num = this.y0siz - this.yt0siz;
      int ytsiz = this.ytsiz;
    }
    this.ntX = (int) Math.Ceiling((double) (this.x0siz + src.ImgWidth) / (double) this.xtsiz);
    this.ntY = (int) Math.Ceiling((double) (this.y0siz + src.ImgHeight) / (double) this.ytsiz);
  }

  public override int getTileComponentWidth(int t, int c)
  {
    int tileIdx = this.TileIdx;
    return this.compW[c];
  }

  public override int getTileComponentHeight(int t, int c)
  {
    int tileIdx = this.TileIdx;
    return this.compH[c];
  }

  public virtual int getFixedPoint(int c) => this.src.getFixedPoint(c);

  public DataBlock getInternCompData(DataBlock blk, int c)
  {
    if (blk.ulx < 0 || blk.uly < 0 || blk.w > this.compW[c] || blk.h > this.compH[c])
      throw new ArgumentException("Block is outside the tile");
    int num1 = (int) Math.Ceiling((double) this.x0siz / (double) this.src.getCompSubsX(c));
    int num2 = (int) Math.Ceiling((double) this.y0siz / (double) this.src.getCompSubsY(c));
    blk.ulx -= num1;
    blk.uly -= num2;
    blk = this.src.getInternCompData(blk, c);
    blk.ulx += num1;
    blk.uly += num2;
    return blk;
  }

  public DataBlock getCompData(DataBlock blk, int c)
  {
    if (blk.ulx < 0 || blk.uly < 0 || blk.w > this.compW[c] || blk.h > this.compH[c])
      throw new ArgumentException("Block is outside the tile");
    int num1 = (int) Math.Ceiling((double) this.x0siz / (double) this.src.getCompSubsX(c));
    int num2 = (int) Math.Ceiling((double) this.y0siz / (double) this.src.getCompSubsY(c));
    blk.ulx -= num1;
    blk.uly -= num2;
    blk = this.src.getCompData(blk, c);
    blk.ulx += num1;
    blk.uly += num2;
    return blk;
  }

  public override void setTile(int x, int y)
  {
    if (x < 0 || y < 0 || x >= this.ntX || y >= this.ntY)
      throw new ArgumentException("Tile's indexes out of bounds");
    this.tx = x;
    this.ty = y;
    int num1 = x != 0 ? this.xt0siz + x * this.xtsiz : this.x0siz;
    int num2 = y != 0 ? this.yt0siz + y * this.ytsiz : this.y0siz;
    int num3 = x != this.ntX - 1 ? this.xt0siz + (x + 1) * this.xtsiz : this.x0siz + this.src.ImgWidth;
    int num4 = y != this.ntY - 1 ? this.yt0siz + (y + 1) * this.ytsiz : this.y0siz + this.src.ImgHeight;
    this.tileW = num3 - num1;
    this.tileH = num4 - num2;
    int numComps = this.src.NumComps;
    if (this.compW == null)
      this.compW = new int[numComps];
    if (this.compH == null)
      this.compH = new int[numComps];
    if (this.tcx0 == null)
      this.tcx0 = new int[numComps];
    if (this.tcy0 == null)
      this.tcy0 = new int[numComps];
    for (int c = 0; c < numComps; ++c)
    {
      this.tcx0[c] = (int) Math.Ceiling((double) num1 / (double) this.src.getCompSubsX(c));
      this.tcy0[c] = (int) Math.Ceiling((double) num2 / (double) this.src.getCompSubsY(c));
      this.compW[c] = (int) Math.Ceiling((double) num3 / (double) this.src.getCompSubsX(c)) - this.tcx0[c];
      this.compH[c] = (int) Math.Ceiling((double) num4 / (double) this.src.getCompSubsY(c)) - this.tcy0[c];
    }
  }

  public override void nextTile()
  {
    if (this.tx == this.ntX - 1 && this.ty == this.ntY - 1)
      throw new Exception();
    if (this.tx < this.ntX - 1)
      this.setTile(this.tx + 1, this.ty);
    else
      this.setTile(0, this.ty + 1);
  }

  public override JPXImageCoordinates getTile(JPXImageCoordinates co)
  {
    if (co == null)
      return new JPXImageCoordinates(this.tx, this.ty);
    co.x = this.tx;
    co.y = this.ty;
    return co;
  }

  public override int getCompUpperLeftCornerX(int c) => this.tcx0[c];

  public override int getCompUpperLeftCornerY(int c) => this.tcy0[c];

  public override JPXImageCoordinates getNumTiles(JPXImageCoordinates co)
  {
    if (co == null)
      return new JPXImageCoordinates(this.ntX, this.ntY);
    co.x = this.ntX;
    co.y = this.ntY;
    return co;
  }

  public override int getNumTiles() => this.ntX * this.ntY;

  public JPXImageCoordinates getTilingOrigin(JPXImageCoordinates co)
  {
    if (co == null)
      return new JPXImageCoordinates(this.xt0siz, this.yt0siz);
    co.x = this.xt0siz;
    co.y = this.yt0siz;
    return co;
  }

  public override string ToString()
  {
    return $"Tiler: source= {(object) this.src}\n{(object) this.getNumTiles()} tile(s), nominal width={(object) this.xtsiz}, nominal height={(object) this.ytsiz}";
  }
}
