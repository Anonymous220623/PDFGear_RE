// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfGraphicsState
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfGraphicsState
{
  private PdfGraphics m_graphics;
  private PdfTransformationMatrix m_matrix;
  private TextRenderingMode m_textRenderingMode;
  private float m_characterSpacing;
  private float m_wordSpacing;
  private float m_textScaling = 100f;
  private PdfPen m_pen;
  private PdfBrush m_brush;
  private PdfFont m_font;
  private PdfColorSpace m_colorSpace;

  internal PdfGraphics Graphics => this.m_graphics;

  internal PdfTransformationMatrix Matrix => this.m_matrix;

  internal float CharacterSpacing
  {
    get => this.m_characterSpacing;
    set => this.m_characterSpacing = value;
  }

  internal float WordSpacing
  {
    get => this.m_wordSpacing;
    set => this.m_wordSpacing = value;
  }

  internal float TextScaling
  {
    get => this.m_textScaling;
    set => this.m_textScaling = value;
  }

  internal PdfPen Pen
  {
    get => this.m_pen;
    set => this.m_pen = value;
  }

  internal PdfBrush Brush
  {
    get => this.m_brush;
    set => this.m_brush = value;
  }

  internal PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  internal PdfColorSpace ColorSpace
  {
    get => this.m_colorSpace;
    set => this.m_colorSpace = value;
  }

  internal TextRenderingMode TextRenderingMode
  {
    get => this.m_textRenderingMode;
    set => this.m_textRenderingMode = value;
  }

  private PdfGraphicsState()
  {
  }

  internal PdfGraphicsState(PdfGraphics graphics, PdfTransformationMatrix matrix)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if (matrix == null)
      throw new ArgumentNullException(nameof (matrix));
    this.m_graphics = graphics;
    this.m_matrix = matrix;
  }
}
