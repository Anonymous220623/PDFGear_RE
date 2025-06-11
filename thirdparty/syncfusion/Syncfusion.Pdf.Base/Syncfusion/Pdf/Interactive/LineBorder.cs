// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.LineBorder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class LineBorder : IPdfWrapper
{
  private float m_borderLineWidth = 1f;
  private int m_borderWidth = 1;
  private int m_dashArray;
  private PdfBorderStyle m_borderStyle;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public int BorderWidth
  {
    get => this.m_borderWidth;
    set
    {
      this.m_borderWidth = value;
      this.m_dictionary.SetNumber("W", this.m_borderWidth);
    }
  }

  internal float BorderLineWidth
  {
    get => this.m_borderLineWidth;
    set
    {
      this.m_borderLineWidth = value;
      this.m_dictionary.SetNumber("W", this.m_borderLineWidth);
    }
  }

  public PdfBorderStyle BorderStyle
  {
    get => this.m_borderStyle;
    set
    {
      this.m_borderStyle = value;
      this.m_dictionary.SetName("S", this.StyleToString(this.m_borderStyle));
    }
  }

  public int DashArray
  {
    get => this.m_dashArray;
    set
    {
      this.m_dashArray = value;
      PdfArray primitive = new PdfArray();
      primitive.Insert(0, (IPdfPrimitive) new PdfNumber(this.m_dashArray));
      primitive.Insert(1, (IPdfPrimitive) new PdfNumber(this.m_dashArray));
      this.m_dictionary.SetProperty("D", (IPdfPrimitive) primitive);
    }
  }

  public LineBorder()
  {
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Border"));
  }

  private string StyleToString(PdfBorderStyle style)
  {
    switch (style)
    {
      case PdfBorderStyle.Dashed:
      case PdfBorderStyle.Dot:
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

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
