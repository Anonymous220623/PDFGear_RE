// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ROIMaskGenerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class ROIMaskGenerator
{
  internal ROI[] roi_array;
  internal int nrc;
  internal bool[] tileMaskMade;
  internal bool roiInTile;

  internal virtual ROI[] ROIs => this.roi_array;

  internal ROIMaskGenerator(ROI[] rois, int nrc)
  {
    this.roi_array = rois;
    this.nrc = nrc;
    this.tileMaskMade = new bool[nrc];
  }

  internal abstract bool getROIMask(DataBlockInt db, Subband sb, int magbits, int c);

  internal abstract void makeMask(Subband sb, int magbits, int n);

  public virtual void tileChanged()
  {
    for (int index = 0; index < this.nrc; ++index)
      this.tileMaskMade[index] = false;
  }
}
