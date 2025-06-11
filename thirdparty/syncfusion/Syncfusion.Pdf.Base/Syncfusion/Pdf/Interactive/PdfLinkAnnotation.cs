// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLinkAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfLinkAnnotation : PdfAnnotation
{
  private PdfHighlightMode m_highlightMode;

  public PdfHighlightMode HighlightMode
  {
    get => this.m_highlightMode;
    set
    {
      this.m_highlightMode = value;
      this.Dictionary.SetName("H", this.GetHighlightMode(this.m_highlightMode));
    }
  }

  public PdfLinkAnnotation()
  {
  }

  public PdfLinkAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Link"));
  }

  private string GetHighlightMode(PdfHighlightMode mode)
  {
    string highlightMode = (string) null;
    switch (mode)
    {
      case PdfHighlightMode.NoHighlighting:
        highlightMode = "N";
        break;
      case PdfHighlightMode.Invert:
        highlightMode = "I";
        break;
      case PdfHighlightMode.Outline:
        highlightMode = "O";
        break;
      case PdfHighlightMode.Push:
        highlightMode = "P";
        break;
    }
    return highlightMode;
  }
}
