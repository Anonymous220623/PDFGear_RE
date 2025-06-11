// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImageData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal interface ImageData
{
  int TileWidth { get; }

  int TileHeight { get; }

  int NomTileWidth { get; }

  int NomTileHeight { get; }

  int ImgWidth { get; }

  int ImgHeight { get; }

  int NumComps { get; }

  int TileIdx { get; }

  int TilePartULX { get; }

  int TilePartULY { get; }

  int ImgULX { get; }

  int ImgULY { get; }

  int getCompSubsX(int c);

  int getCompSubsY(int c);

  int getTileComponentWidth(int t, int c);

  int getTileComponentHeight(int t, int c);

  int getCompImgWidth(int c);

  int getCompImgHeight(int c);

  int getNomRangeBits(int c);

  void setTile(int x, int y);

  void nextTile();

  JPXImageCoordinates getTile(JPXImageCoordinates co);

  int getCompUpperLeftCornerX(int c);

  int getCompUpperLeftCornerY(int c);

  JPXImageCoordinates getNumTiles(JPXImageCoordinates co);

  int getNumTiles();
}
