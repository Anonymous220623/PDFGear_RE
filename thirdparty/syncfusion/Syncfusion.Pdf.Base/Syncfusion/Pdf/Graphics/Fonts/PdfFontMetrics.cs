// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.PdfFontMetrics
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class PdfFontMetrics : ICloneable
{
  public float Ascent;
  public float Descent;
  public string Name;
  public string PostScriptName;
  public float Size;
  public float Height;
  public int FirstChar;
  public int LastChar;
  public int LineGap;
  public float SubScriptSizeFactor;
  public float SuperscriptSizeFactor;
  private WidthTable m_widthTable;
  internal bool isUnicodeFont;
  internal bool IsBold;

  public float GetAscent(PdfStringFormat format)
  {
    return this.Ascent * (1f / 1000f) * this.GetSize(format);
  }

  public float GetDescent(PdfStringFormat format)
  {
    return this.Descent * (1f / 1000f) * this.GetSize(format);
  }

  public float GetLineGap(PdfStringFormat format)
  {
    return (float) this.LineGap * (1f / 1000f) * this.GetSize(format);
  }

  public float GetHeight(PdfStringFormat format)
  {
    return (double) this.GetDescent(format) >= 0.0 ? this.GetAscent(format) + this.GetDescent(format) + this.GetLineGap(format) : this.GetAscent(format) - this.GetDescent(format) + this.GetLineGap(format);
  }

  public float GetSize(PdfStringFormat format)
  {
    float size = this.Size;
    if (format != null)
    {
      switch (format.SubSuperScript)
      {
        case PdfSubSuperScript.SuperScript:
          size /= 1.5f;
          break;
        case PdfSubSuperScript.SubScript:
          size /= 1.5f;
          break;
      }
    }
    return size;
  }

  public object Clone()
  {
    PdfFontMetrics pdfFontMetrics = (PdfFontMetrics) this.MemberwiseClone();
    pdfFontMetrics.WidthTable = this.WidthTable.Clone();
    return (object) pdfFontMetrics;
  }

  public WidthTable WidthTable
  {
    get => this.m_widthTable;
    set => this.m_widthTable = value;
  }
}
