// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TextGlyph
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

internal class TextGlyph
{
  private RectangleF m_glyphBounds = new RectangleF();
  private char m_text;
  private float m_fontSize;
  private string m_fontName;
  private FontStyle m_fontStyle;

  internal char Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal RectangleF Bounds
  {
    get => this.m_glyphBounds;
    set => this.m_glyphBounds = value;
  }

  internal float FontSize
  {
    get => this.m_fontSize;
    set => this.m_fontSize = value;
  }

  internal string FontName
  {
    get => this.m_fontName;
    set => this.m_fontName = value;
  }

  internal FontStyle FontStyle
  {
    get => this.m_fontStyle;
    set => this.m_fontStyle = value;
  }
}
