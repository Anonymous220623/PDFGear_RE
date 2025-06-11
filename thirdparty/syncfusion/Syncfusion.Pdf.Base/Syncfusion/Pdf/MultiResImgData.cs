// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.MultiResImgData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal interface MultiResImgData
{
  int NomTileWidth { get; }

  int NomTileHeight { get; }

  int NumComps { get; }

  int TileIdx { get; }

  int TilePartULX { get; }

  int TilePartULY { get; }

  int getTileWidth(int rl);

  int getTileHeight(int rl);

  int getImgWidth(int rl);

  int getImgHeight(int rl);

  int getCompSubsX(int c);

  int getCompSubsY(int c);

  int getTileCompWidth(int t, int c, int rl);

  int getTileCompHeight(int t, int c, int rl);

  int getCompImgWidth(int c, int rl);

  int getCompImgHeight(int n, int rl);

  void setTile(int x, int y);

  void nextTile();

  JPXImageCoordinates getTile(JPXImageCoordinates co);

  int getResULX(int c, int rl);

  int getResULY(int c, int rl);

  int getImgULX(int rl);

  int getImgULY(int rl);

  JPXImageCoordinates getNumTiles(JPXImageCoordinates co);

  int getNumTiles();

  SubbandSyn getSynSubbandTree(int t, int c);
}
