// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.WidgetBorder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

internal class WidgetBorder : IPdfWrapper
{
  private float m_width = 1f;
  private PdfBorderStyle m_style;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public float Width
  {
    get => this.m_width;
    set
    {
      this.m_width = value;
      int width = (int) this.m_width;
      if ((double) this.m_width - (double) width == 0.0)
        this.m_dictionary.SetNumber("W", width);
      else
        this.m_dictionary.SetNumber("W", this.m_width);
    }
  }

  public PdfBorderStyle Style
  {
    get => this.m_style;
    set
    {
      this.m_style = value;
      this.m_dictionary.SetName("S", this.StyleToString(this.m_style));
    }
  }

  public WidgetBorder()
  {
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Border"));
    this.m_dictionary.SetName("S", this.StyleToString(this.m_style));
  }

  private string StyleToString(PdfBorderStyle style)
  {
    switch (style)
    {
      case PdfBorderStyle.Dashed:
        return "D";
      case PdfBorderStyle.Beveled:
        return "B";
      case PdfBorderStyle.Inset:
        return "I";
      case PdfBorderStyle.Underline:
        return "U";
      default:
        return "S";
    }
  }

  public IPdfPrimitive Element => (IPdfPrimitive) this.m_dictionary;
}
