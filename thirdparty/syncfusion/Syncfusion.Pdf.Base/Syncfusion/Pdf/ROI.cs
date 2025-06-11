// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ROI
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class ROI
{
  public ImgReaderPGM maskPGM;
  public bool arbShape;
  public bool rect;
  public int comp;
  public int ulx;
  public int uly;
  public int w;
  public int h;
  public int x;
  public int y;
  public int r;

  public ROI(int comp, ImgReaderPGM maskPGM)
  {
    this.arbShape = true;
    this.rect = false;
    this.comp = comp;
    this.maskPGM = maskPGM;
  }

  public ROI(int comp, int ulx, int uly, int w, int h)
  {
    this.arbShape = false;
    this.comp = comp;
    this.ulx = ulx;
    this.uly = uly;
    this.w = w;
    this.h = h;
    this.rect = true;
  }

  public ROI(int comp, int x, int y, int rad)
  {
    this.arbShape = false;
    this.comp = comp;
    this.x = x;
    this.y = y;
    this.r = rad;
  }

  public override string ToString()
  {
    if (this.arbShape)
      return "ROI with arbitrary shape, PGM file= " + (object) this.maskPGM;
    return this.rect ? $"Rectangular ROI, comp={(object) this.comp} ulx={(object) this.ulx} uly={(object) this.uly} w={(object) this.w} h={(object) this.h}" : $"Circular ROI,  comp={(object) this.comp} x={(object) this.x} y={(object) this.y} radius={(object) this.r}";
  }
}
