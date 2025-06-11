// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfAutomaticField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfAutomaticField : PdfGraphicsElement
{
  private RectangleF m_bounds = RectangleF.Empty;
  private PdfFont m_font;
  private PdfBrush m_brush;
  private PdfPen m_pen;
  private PdfStringFormat m_stringFormat;
  private SizeF m_templateSize = SizeF.Empty;

  protected PdfAutomaticField()
  {
  }

  protected PdfAutomaticField(PdfFont font) => this.Font = font;

  protected PdfAutomaticField(PdfFont font, PdfBrush brush)
  {
    this.Font = font;
    this.Brush = brush;
  }

  protected PdfAutomaticField(PdfFont font, RectangleF bounds)
  {
    this.Font = font;
    this.Bounds = bounds;
  }

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  public SizeF Size
  {
    get => this.m_bounds.Size;
    set => this.m_bounds.Size = value;
  }

  public PointF Location
  {
    get => this.m_bounds.Location;
    set => this.m_bounds.Location = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value != null ? value : throw new ArgumentNullException(nameof (Font));
  }

  public PdfBrush Brush
  {
    get => this.m_brush;
    set => this.m_brush = value != null ? value : throw new ArgumentNullException(nameof (Brush));
  }

  public PdfPen Pen
  {
    get => this.m_pen;
    set => this.m_pen = value;
  }

  public PdfStringFormat StringFormat
  {
    get => this.m_stringFormat;
    set => this.m_stringFormat = value;
  }

  public override void Draw(PdfGraphics graphics, float x, float y)
  {
    base.Draw(graphics, x, y);
    graphics.AutomaticFields.Add(new PdfAutomaticFieldInfo(this, new PointF(x, y)));
  }

  protected internal abstract string GetValue(PdfGraphics graphics);

  protected internal virtual void PerformDraw(
    PdfGraphics graphics,
    PointF location,
    float scalingX,
    float scalingY)
  {
    if ((double) this.Bounds.Height != 0.0 && (double) this.Bounds.Width != 0.0)
      return;
    string text = this.GetValue(graphics);
    this.m_templateSize = this.ObtainFont().MeasureString(text, this.Size, this.StringFormat);
  }

  protected SizeF ObtainSize()
  {
    return (double) this.Bounds.Height == 0.0 || (double) this.Bounds.Width == 0.0 ? this.m_templateSize : this.Size;
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
  }

  protected PdfBrush ObtainBrush() => this.m_brush != null ? this.m_brush : PdfBrushes.Black;

  protected PdfFont ObtainFont() => this.m_font != null ? this.m_font : PdfDocument.DefaultFont;
}
