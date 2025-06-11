// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.MultiResImgDataAdapter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class MultiResImgDataAdapter : MultiResImgData
{
  internal int tIdx;
  internal MultiResImgData mressrc;

  public virtual int NomTileWidth => this.mressrc.NomTileWidth;

  public virtual int NomTileHeight => this.mressrc.NomTileHeight;

  public virtual int NumComps => this.mressrc.NumComps;

  public virtual int TileIdx => this.mressrc.TileIdx;

  public virtual int TilePartULX => this.mressrc.TilePartULX;

  public virtual int TilePartULY => this.mressrc.TilePartULY;

  internal MultiResImgDataAdapter(MultiResImgData src) => this.mressrc = src;

  public virtual int getTileWidth(int rl) => this.mressrc.getTileWidth(rl);

  public virtual int getTileHeight(int rl) => this.mressrc.getTileHeight(rl);

  public virtual int getImgWidth(int rl) => this.mressrc.getImgWidth(rl);

  public virtual int getImgHeight(int rl) => this.mressrc.getImgHeight(rl);

  public virtual int getCompSubsX(int c) => this.mressrc.getCompSubsX(c);

  public virtual int getCompSubsY(int c) => this.mressrc.getCompSubsY(c);

  public virtual int getTileCompWidth(int t, int c, int rl)
  {
    return this.mressrc.getTileCompWidth(t, c, rl);
  }

  public virtual int getTileCompHeight(int t, int c, int rl)
  {
    return this.mressrc.getTileCompHeight(t, c, rl);
  }

  public virtual int getCompImgWidth(int c, int rl) => this.mressrc.getCompImgWidth(c, rl);

  public virtual int getCompImgHeight(int c, int rl) => this.mressrc.getCompImgHeight(c, rl);

  public virtual void setTile(int x, int y)
  {
    this.mressrc.setTile(x, y);
    this.tIdx = this.TileIdx;
  }

  public virtual void nextTile()
  {
    this.mressrc.nextTile();
    this.tIdx = this.TileIdx;
  }

  public virtual JPXImageCoordinates getTile(JPXImageCoordinates co) => this.mressrc.getTile(co);

  public virtual int getResULX(int c, int rl) => this.mressrc.getResULX(c, rl);

  public virtual int getResULY(int c, int rl) => this.mressrc.getResULY(c, rl);

  public virtual int getImgULX(int rl) => this.mressrc.getImgULX(rl);

  public virtual int getImgULY(int rl) => this.mressrc.getImgULY(rl);

  public virtual JPXImageCoordinates getNumTiles(JPXImageCoordinates co)
  {
    return this.mressrc.getNumTiles(co);
  }

  public virtual int getNumTiles() => this.mressrc.getNumTiles();

  public abstract SubbandSyn getSynSubbandTree(int param1, int param2);
}
