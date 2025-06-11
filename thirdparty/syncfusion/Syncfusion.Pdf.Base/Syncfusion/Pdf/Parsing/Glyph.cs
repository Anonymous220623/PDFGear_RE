// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Glyph
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Glyph
{
  internal ushort GlyphId { get; set; }

  public GlyphOutlinesCollection Outlines { get; set; }

  public double AdvancedWidth { get; set; }

  public Point HorizontalKerning { get; set; }

  public Point VerticalKerning { get; set; }

  public string Name { get; set; }
}
