// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.RectROIMaskGenerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class RectROIMaskGenerator : ROIMaskGenerator
{
  private int[] ulxs;
  private int[] ulys;
  private int[] lrxs;
  private int[] lrys;
  private int[] nrROIs;
  private SubbandRectROIMask[] sMasks;

  public RectROIMaskGenerator(ROI[] ROIs, int nrc)
    : base(ROIs, nrc)
  {
    int length = ROIs.Length;
    this.nrROIs = new int[nrc];
    this.sMasks = new SubbandRectROIMask[nrc];
    for (int index = length - 1; index >= 0; --index)
      ++this.nrROIs[ROIs[index].comp];
  }

  internal override bool getROIMask(DataBlockInt db, Subband sb, int magbits, int c)
  {
    int ulx = db.ulx;
    int uly = db.uly;
    int w = db.w;
    int h = db.h;
    int[] dataInt = db.DataInt;
    if (!this.tileMaskMade[c])
    {
      this.makeMask(sb, magbits, c);
      this.tileMaskMade[c] = true;
    }
    if (!this.roiInTile)
      return false;
    SubbandRectROIMask subbandRectRoiMask = (SubbandRectROIMask) this.sMasks[c].getSubbandRectROIMask(ulx, uly);
    int[] ulxs = subbandRectRoiMask.ulxs;
    int[] ulys = subbandRectRoiMask.ulys;
    int[] lrxs = subbandRectRoiMask.lrxs;
    int[] lrys = subbandRectRoiMask.lrys;
    int num1 = ulxs.Length - 1;
    int num2 = ulx - subbandRectRoiMask.ulx;
    int num3 = uly - subbandRectRoiMask.uly;
    for (int index1 = num1; index1 >= 0; --index1)
    {
      int num4 = ulxs[index1] - num2;
      if (num4 < 0)
        num4 = 0;
      else if (num4 >= w)
        num4 = w;
      int num5 = ulys[index1] - num3;
      if (num5 < 0)
        num5 = 0;
      else if (num5 >= h)
        num5 = h;
      int num6 = lrxs[index1] - num2;
      if (num6 < 0)
        num6 = -1;
      else if (num6 >= w)
        num6 = w - 1;
      int num7 = lrys[index1] - num3;
      if (num7 < 0)
        num7 = -1;
      else if (num7 >= h)
        num7 = h - 1;
      int index2 = w * num7 + num6;
      int num8 = num6 - num4;
      int num9 = w - num8 - 1;
      for (int index3 = num7 - num5; index3 >= 0; --index3)
      {
        int num10 = num8;
        while (num10 >= 0)
        {
          dataInt[index2] = magbits;
          --num10;
          --index2;
        }
        index2 -= num9;
      }
    }
    return true;
  }

  public override string ToString() => "Fast rectangular ROI mask generator";

  internal override void makeMask(Subband sb, int magbits, int n)
  {
    int nrRoI = this.nrROIs[n];
    int ulcx = sb.ulcx;
    int ulcy = sb.ulcy;
    int w = sb.w;
    int h = sb.h;
    ROI[] roiArray = this.roi_array;
    this.ulxs = new int[nrRoI];
    this.ulys = new int[nrRoI];
    this.lrxs = new int[nrRoI];
    this.lrys = new int[nrRoI];
    int nr = 0;
    for (int index = roiArray.Length - 1; index >= 0; --index)
    {
      if (roiArray[index].comp == n)
      {
        int ulx = roiArray[index].ulx;
        int uly = roiArray[index].uly;
        int num1 = roiArray[index].w + ulx - 1;
        int num2 = roiArray[index].h + uly - 1;
        if (ulx <= ulcx + w - 1 && uly <= ulcy + h - 1 && num1 >= ulcx && num2 >= ulcy)
        {
          int num3 = ulx - ulcx;
          int num4 = num1 - ulcx;
          int num5 = uly - ulcy;
          int num6 = num2 - ulcy;
          int num7 = num3 < 0 ? 0 : num3;
          int num8 = num5 < 0 ? 0 : num5;
          int num9 = num4 > w - 1 ? w - 1 : num4;
          int num10 = num6 > h - 1 ? h - 1 : num6;
          this.ulxs[nr] = num7;
          this.ulys[nr] = num8;
          this.lrxs[nr] = num9;
          this.lrys[nr] = num10;
          ++nr;
        }
      }
    }
    if (nr == 0)
      this.roiInTile = false;
    else
      this.roiInTile = true;
    this.sMasks[n] = new SubbandRectROIMask(sb, this.ulxs, this.ulys, this.lrxs, this.lrys, nr);
  }
}
