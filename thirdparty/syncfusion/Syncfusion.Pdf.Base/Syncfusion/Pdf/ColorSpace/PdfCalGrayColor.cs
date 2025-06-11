// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfCalGrayColor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfCalGrayColor(PdfColorSpaces colorspace) : PdfExtendedColor(colorspace)
{
  private double m_gray;

  public double Gray
  {
    get => this.m_gray;
    set
    {
      this.m_gray = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException(nameof (Gray), "Gray level must be between 0 and 1");
    }
  }
}
