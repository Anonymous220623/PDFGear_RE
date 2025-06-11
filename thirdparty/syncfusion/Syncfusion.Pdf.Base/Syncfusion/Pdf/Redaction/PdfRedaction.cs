// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Redaction.PdfRedaction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Redaction;

public class PdfRedaction
{
  private RectangleF m_bounds;
  private Color m_fillColor = Color.Transparent;
  private PdfTemplate m_appearance;
  internal bool m_success;
  internal bool AppearanceEnabled;
  internal PdfLoadedPage page;
  internal bool PathRedaction;

  public Color FillColor
  {
    get => this.m_fillColor;
    set => this.m_fillColor = value;
  }

  internal RectangleF Bounds => this.m_bounds;

  internal bool Success
  {
    get => this.m_success;
    set => this.m_success = value;
  }

  public PdfTemplate Appearance
  {
    get
    {
      if (this.m_appearance == null)
      {
        this.m_appearance = new PdfTemplate(this.m_bounds.Size);
        this.AppearanceEnabled = true;
      }
      return this.m_appearance;
    }
    internal set
    {
      this.m_appearance = value;
      this.AppearanceEnabled = true;
    }
  }

  public PdfRedaction(RectangleF bounds) => this.m_bounds = bounds;

  public PdfRedaction(RectangleF bounds, Color fillColor)
  {
    this.m_bounds = bounds;
    this.m_fillColor = fillColor;
  }
}
