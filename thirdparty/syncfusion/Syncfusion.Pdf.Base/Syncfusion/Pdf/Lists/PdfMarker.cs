// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfMarker
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public abstract class PdfMarker
{
  private PdfFont m_font;
  private PdfBrush m_brush;
  private PdfPen m_pen;
  private PdfStringFormat m_format;
  private PdfListMarkerAlignment m_alignment;

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  public PdfBrush Brush
  {
    get => this.m_brush;
    set => this.m_brush = value;
  }

  public PdfPen Pen
  {
    get => this.m_pen;
    set => this.m_pen = value;
  }

  public PdfStringFormat StringFormat
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public PdfListMarkerAlignment Alignment
  {
    get => this.m_alignment;
    set => this.m_alignment = value;
  }

  internal bool RightToLeft => this.m_alignment == PdfListMarkerAlignment.Right;
}
