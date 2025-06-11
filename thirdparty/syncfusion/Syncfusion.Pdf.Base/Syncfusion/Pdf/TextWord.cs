// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TextWord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class TextWord
{
  private string m_text;
  private RectangleF m_wordBounds = new RectangleF();
  private float m_fontSize;
  private string m_fontName;
  private FontStyle m_fontStyle;
  private List<TextGlyph> m_glyphs = new List<TextGlyph>();

  public string Text
  {
    get => this.m_text;
    internal set => this.m_text = value;
  }

  public RectangleF Bounds
  {
    get => this.m_wordBounds;
    internal set => this.m_wordBounds = value;
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

  internal List<TextGlyph> Glyphs => this.m_glyphs;
}
