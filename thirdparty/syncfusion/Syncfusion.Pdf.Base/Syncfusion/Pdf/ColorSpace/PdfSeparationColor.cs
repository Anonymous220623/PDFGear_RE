// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfSeparationColor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfSeparationColor(PdfColorSpaces colorspace) : PdfExtendedColor(colorspace)
{
  private double m_tint = 1.0;

  public double Tint
  {
    get => this.m_tint;
    set => this.m_tint = value;
  }
}
