// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SubbandROIMask
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class SubbandROIMask
{
  internal SubbandROIMask ll;
  internal SubbandROIMask lh;
  internal SubbandROIMask hl;
  internal SubbandROIMask hh;
  internal bool isNode;
  public int ulx;
  public int uly;
  public int w;
  public int h;

  public SubbandROIMask(int ulx, int uly, int w, int h)
  {
    this.ulx = ulx;
    this.uly = uly;
    this.w = w;
    this.h = h;
  }

  public virtual SubbandROIMask getSubbandRectROIMask(int x, int y)
  {
    if (x < this.ulx || y < this.uly || x >= this.ulx + this.w || y >= this.uly + this.h)
      throw new ArgumentException();
    SubbandROIMask subbandRectRoiMask;
    SubbandROIMask hh;
    for (subbandRectRoiMask = this; subbandRectRoiMask.isNode; subbandRectRoiMask = x >= hh.ulx ? (y >= hh.uly ? subbandRectRoiMask.hh : subbandRectRoiMask.hl) : (y >= hh.uly ? subbandRectRoiMask.lh : subbandRectRoiMask.ll))
      hh = subbandRectRoiMask.hh;
    return subbandRectRoiMask;
  }
}
