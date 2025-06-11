// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PageText
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class PageText
{
  private Matrix m_transformPoints;
  private string m_txt;
  private PointF m_currentLocation;
  private float m_textElementWidth;
  private float m_fontSize;
  private Font m_textFont;

  internal float FontSize => this.m_fontSize;

  internal string Text => this.m_txt;

  internal PointF CurrentLocation => this.m_currentLocation;

  internal Matrix TransformPoints => this.m_transformPoints;

  internal float TextElementWidth => this.m_textElementWidth;

  internal Font TextFont => this.m_textFont;

  public PageText(
    Matrix transformPoints,
    string txt,
    PointF CurrentLocation,
    float TextElementWidth,
    float fontSize,
    Font font)
  {
    this.m_transformPoints = transformPoints;
    this.m_txt = txt;
    this.m_currentLocation = CurrentLocation;
    this.m_textElementWidth = TextElementWidth;
    this.m_fontSize = fontSize;
    this.m_textFont = font;
  }
}
