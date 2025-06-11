// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PrecInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class PrecInfo
{
  public int rgulx;
  public int rguly;
  public int rgw;
  public int rgh;
  public int ulx;
  public int uly;
  public int w;
  public int h;
  public int r;
  public CBlkCoordInfo[][][] cblk;
  public int[] nblk;

  public PrecInfo(
    int r,
    int ulx,
    int uly,
    int w,
    int h,
    int rgulx,
    int rguly,
    int rgw,
    int rgh)
  {
    this.r = r;
    this.ulx = ulx;
    this.uly = uly;
    this.w = w;
    this.h = h;
    this.rgulx = rgulx;
    this.rguly = rguly;
    this.rgw = rgw;
    this.rgh = rgh;
    if (r == 0)
    {
      this.cblk = new CBlkCoordInfo[1][][];
      this.nblk = new int[1];
    }
    else
    {
      this.cblk = new CBlkCoordInfo[4][][];
      this.nblk = new int[4];
    }
  }

  public override string ToString()
  {
    return $"ulx={(object) this.ulx},uly={(object) this.uly},w={(object) this.w},h={(object) this.h},rgulx={(object) this.rgulx},rguly={(object) this.rguly},rgw={(object) this.rgw},rgh={(object) this.rgh}";
  }
}
