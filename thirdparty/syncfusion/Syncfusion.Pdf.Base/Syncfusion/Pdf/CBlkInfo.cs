// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CBlkInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class CBlkInfo
{
  public int ulx;
  public int uly;
  public int w;
  public int h;
  public int msbSkipped;
  public int[] len;
  public int[] off;
  public int[] ntp;
  public int ctp;
  public int[][] segLen;
  public int[] pktIdx;

  public CBlkInfo(int ulx, int uly, int w, int h, int nl)
  {
    this.ulx = ulx;
    this.uly = uly;
    this.w = w;
    this.h = h;
    this.off = new int[nl];
    this.len = new int[nl];
    this.ntp = new int[nl];
    this.segLen = new int[nl][];
    this.pktIdx = new int[nl];
    for (int index = nl - 1; index >= 0; --index)
      this.pktIdx[index] = -1;
  }

  public virtual void addNTP(int l, int newtp)
  {
    this.ntp[l] = newtp;
    this.ctp = 0;
    for (int index = 0; index <= l; ++index)
      this.ctp += this.ntp[index];
  }

  public override string ToString()
  {
    string str1 = $"{$"(ulx,uly,w,h)= ({(object) this.ulx},{(object) this.uly},{(object) this.w},{(object) this.h}"}) {(object) this.msbSkipped} MSB bit(s) skipped\n";
    if (this.len != null)
    {
      for (int index1 = 0; index1 < this.len.Length; ++index1)
      {
        string str2 = $"{str1}\tl:{(object) index1}, start:{(object) this.off[index1]}, len:{(object) this.len[index1]}, ntp:{(object) this.ntp[index1]}, pktIdx={(object) this.pktIdx[index1]}";
        if (this.segLen != null && this.segLen[index1] != null)
        {
          string str3 = str2 + " { ";
          for (int index2 = 0; index2 < this.segLen[index1].Length; ++index2)
            str3 = $"{str3}{(object) this.segLen[index1][index2]} ";
          str2 = str3 + "}";
        }
        str1 = str2 + "\n";
      }
    }
    return $"{str1}\tctp={(object) this.ctp}";
  }
}
