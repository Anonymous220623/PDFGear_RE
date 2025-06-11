// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontGlyph
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontGlyph
{
  private double descent;

  internal ushort GlyphId { get; set; }

  public CharCode CharId { get; set; }

  public SystemFontGlyphOutlinesCollection Outlines { get; set; }

  public double AdvancedWidth { get; set; }

  public Syncfusion.PdfViewer.Base.Point HorizontalKerning { get; set; }

  public Syncfusion.PdfViewer.Base.Point VerticalKerning { get; set; }

  public string Name { get; set; }

  public int FontId { get; set; }

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

  internal Brush NonStroke { get; set; }

  public bool IsFilled { get; set; }

  public bool IsStroked { get; set; }

  public SystemFontMatrix TransformMatrix { get; set; }

  public Syncfusion.PdfViewer.Base.Size Size { get; set; }

  public Rect BoundingRect { get; set; }

  public int ZIndex { get; set; }

  public SystemFontPathGeometry Clip { get; set; }

  public bool HasChildren => false;

  public double StrokeThickness { get; set; }

  public SystemFontGlyph()
  {
    this.Ascent = 1000.0;
    this.Descent = 0.0;
    this.TransformMatrix = SystemFontMatrix.Identity;
  }

  public override string ToString() => this.ToUnicode;
}
