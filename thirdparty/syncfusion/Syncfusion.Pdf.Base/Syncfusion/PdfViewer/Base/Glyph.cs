// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Glyph
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Glyph : Syncfusion.Pdf.Parsing.Glyph
{
  private double descent;
  private string m_embededFontFamily;
  private double m_matrixFontSize;

  internal string EmbededFontFamily
  {
    get => this.m_embededFontFamily;
    set => this.m_embededFontFamily = value;
  }

  internal double MatrixFontSize
  {
    get => this.m_matrixFontSize;
    set => this.m_matrixFontSize = value;
  }

  internal bool IsReplace { get; set; }

  public int FontId { get; set; }

  public CharCode CharId { get; set; }

  public string ToUnicode { get; set; }

  public string FontFamily { get; set; }

  public FontStyle FontStyle { get; set; }

  public bool IsBold { get; set; }

  public bool IsItalic { get; set; }

  public double FontSize { get; set; }

  public double Rise { get; set; }

  public double CharSpacing { get; set; }

  public double WordSpacing { get; set; }

  public double HorizontalScaling { get; set; }

  public double Width { get; set; }

  public double Ascent { get; set; }

  public double Descent
  {
    get => this.descent;
    set
    {
      if (value > 0.0)
        this.descent = -value;
      else
        this.descent = value;
    }
  }

  public Brush Fill { get; set; }

  public Brush Stroke { get; set; }

  public Brush NonStroke { get; set; }

  public bool IsFilled { get; set; }

  public bool IsStroked { get; set; }

  public Matrix TransformMatrix { get; set; }

  public Size Size { get; set; }

  public Rect BoundingRect { get; set; }

  public bool IsSpace => this.CharId.BytesCount == 1 && this.CharId.Bytes[0] == (byte) 32 /*0x20*/;

  public int ZIndex { get; set; }

  public PathGeometry Clip { get; set; }

  public bool HasChildren => false;

  public double StrokeThickness { get; set; }

  public bool IsRotated { get; set; }

  public int RotationAngle { get; set; }

  public Glyph()
  {
    this.Ascent = 1000.0;
    this.Descent = 0.0;
    this.TransformMatrix = Matrix.Identity;
  }

  public Rect Arrange(Matrix transformMatrix)
  {
    this.BoundingRect = PdfHelper.GetBoundingRect(new Rect(new Point(0.0, 0.0), new Size(this.Width, Math.Max(1.0, (this.Ascent - 2.0 * this.Descent) / 1000.0))), this.TransformMatrix * transformMatrix);
    return this.BoundingRect;
  }

  public override string ToString() => this.ToUnicode;
}
