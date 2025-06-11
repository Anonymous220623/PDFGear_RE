// Decompiled with JetBrains decompiler
// Type: TextProperties
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
internal class TextProperties
{
  private Color m_strokingBrush;
  private RectangleF m_bounds;

  internal Color StrokingBrush => this.m_strokingBrush;

  internal RectangleF Bounds => this.m_bounds;

  internal TextProperties(Color brush, RectangleF bounds)
  {
    this.m_strokingBrush = brush;
    this.m_bounds = bounds;
  }
}
