// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CoordInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class CoordInfo
{
  public int ulx;
  public int uly;
  public int w;
  public int h;

  public CoordInfo(int ulx, int uly, int w, int h)
  {
    this.ulx = ulx;
    this.uly = uly;
    this.w = w;
    this.h = h;
  }

  public CoordInfo()
  {
  }

  public override string ToString()
  {
    return $"ulx={(object) this.ulx},uly={(object) this.uly},w={(object) this.w},h={(object) this.h}";
  }
}
