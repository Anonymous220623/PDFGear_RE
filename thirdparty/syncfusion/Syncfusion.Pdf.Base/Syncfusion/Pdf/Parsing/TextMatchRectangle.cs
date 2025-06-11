// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.TextMatchRectangle
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class TextMatchRectangle
{
  private RectangleF m_rect = new RectangleF();
  private string m_text = string.Empty;
  private float m_textWidth;
  private float m_scaleX;
  private Font m_textFont;

  public TextMatchRectangle(RectangleF rec, string txt, float txtWidth, float scaleX, Font font)
  {
    this.m_rect = rec;
    this.m_text = txt;
    this.m_textWidth = txtWidth;
    this.m_scaleX = scaleX;
    this.m_textFont = font;
  }

  internal RectangleF Rect
  {
    get => this.m_rect;
    set => this.m_rect = value;
  }

  internal string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal float TextWidth
  {
    get => this.m_textWidth;
    set => this.m_textWidth = value;
  }

  internal float ScaleX
  {
    get => this.m_scaleX;
    set => this.m_scaleX = value;
  }

  internal Font TextFont
  {
    get => this.m_textFont;
    set => this.m_textFont = value;
  }
}
