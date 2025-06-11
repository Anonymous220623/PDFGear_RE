// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImgWriter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class ImgWriter
{
  public const int DEF_STRIP_HEIGHT = 64 /*0x40*/;
  internal BlockImageDataSource src;
  internal int w;
  internal int h;

  public abstract void close();

  public abstract void flush();

  ~ImgWriter() => this.flush();

  public abstract void write();

  public virtual void writeAll()
  {
    JPXImageCoordinates numTiles = this.src.getNumTiles((JPXImageCoordinates) null);
    for (int y = 0; y < numTiles.y; ++y)
    {
      for (int x = 0; x < numTiles.x; ++x)
      {
        this.src.setTile(x, y);
        this.write();
      }
    }
  }

  public abstract void write(int ulx, int uly, int w, int h);
}
