// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfICCColor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfICCColor : PdfExtendedColor
{
  private double[] m_components;
  private PdfICCColorSpace m_colorspaces;

  public PdfICCColor(PdfColorSpaces colorspace)
    : base(colorspace)
  {
    this.m_colorspaces = colorspace as PdfICCColorSpace;
  }

  public double[] ColorComponents
  {
    get => this.m_components;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (ColorComponents), "ColorComponents array cannot be null.");
      PdfICCColorSpace colorSpace = this.ColorSpace as PdfICCColorSpace;
      if (value.Length != (int) colorSpace.ColorComponents)
        throw new ArgumentOutOfRangeException(nameof (ColorComponents), "Array length must match the number of color components defined on the underlying ICC colorspace.");
      this.m_components = value;
    }
  }

  internal PdfICCColorSpace ColorSpaces => this.m_colorspaces;
}
