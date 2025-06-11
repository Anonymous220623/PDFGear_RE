// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TextLine
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class TextLine
{
  private List<TextWord> m_wordCollection = new List<TextWord>();
  private RectangleF m_lineBounds = new RectangleF();
  private float m_fontSize;
  private string m_fontName;
  private FontStyle m_fontStyle;
  private string m_text;

  public string Text
  {
    get => this.m_text;
    internal set => this.m_text = value;
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

  public List<TextWord> WordCollection
  {
    get => this.m_wordCollection;
    internal set => this.m_wordCollection = value;
  }

  public RectangleF Bounds
  {
    get => this.m_lineBounds;
    internal set => this.m_lineBounds = value;
  }
}
