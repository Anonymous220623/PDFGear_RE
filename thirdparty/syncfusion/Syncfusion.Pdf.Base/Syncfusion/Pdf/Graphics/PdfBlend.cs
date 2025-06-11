// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfBlend
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public sealed class PdfBlend : PdfBlendBase
{
  private float[] m_factors;

  public PdfBlend()
  {
  }

  public PdfBlend(int count)
    : base(count)
  {
  }

  public float[] Factors
  {
    get => this.m_factors;
    set
    {
      this.m_factors = value != null ? this.SetArray((Array) value) as float[] : throw new ArgumentNullException(nameof (Factors));
    }
  }

  internal PdfColorBlend GenerateColorBlend(PdfColor[] colours, PdfColorSpace colorSpace)
  {
    if (colours == null)
      throw new ArgumentNullException(nameof (colours));
    if (this.Positions == null)
      this.Positions = new float[1];
    PdfColorBlend colorBlend = new PdfColorBlend(this.Count);
    float[] numArray = this.Positions;
    PdfColor[] pdfColorArray;
    if (numArray.Length == 1)
    {
      numArray = new float[3]{ 0.0f, this.Positions[0], 1f };
      pdfColorArray = new PdfColor[3]
      {
        colours[0],
        colours[0],
        colours[1]
      };
    }
    else
    {
      PdfColor colour1 = colours[0];
      PdfColor colour2 = colours[1];
      pdfColorArray = new PdfColor[this.Count];
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        pdfColorArray[index] = PdfBlendBase.Interpolate((double) this.m_factors[index], colour1, colour2, colorSpace);
    }
    colorBlend.Positions = numArray;
    colorBlend.Colors = pdfColorArray;
    return colorBlend;
  }

  internal PdfBlend ClonePdfBlend()
  {
    PdfBlend pdfBlend = this.MemberwiseClone() as PdfBlend;
    if (this.m_factors != null)
      pdfBlend.Factors = this.m_factors.Clone() as float[];
    if (this.Positions != null)
      pdfBlend.Positions = this.Positions.Clone() as float[];
    return pdfBlend;
  }
}
