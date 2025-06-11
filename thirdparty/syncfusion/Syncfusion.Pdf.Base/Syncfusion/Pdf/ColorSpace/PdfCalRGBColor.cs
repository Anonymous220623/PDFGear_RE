// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfCalRGBColor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfCalRGBColor(PdfColorSpaces colorspace) : PdfExtendedColor(colorspace)
{
  private double m_red;
  private double m_green;
  private double m_blue;

  public double Blue
  {
    get => this.m_blue;
    set
    {
      this.m_blue = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException(nameof (Blue), "Blue level must be between 0 and 1");
    }
  }

  public double Green
  {
    get => this.m_green;
    set
    {
      this.m_green = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException(nameof (Green), "Green level must be between 0 and 1");
    }
  }

  public double Red
  {
    get => this.m_red;
    set
    {
      this.m_red = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException(nameof (Red), "Red level must be between 0 and 1");
    }
  }
}
