// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TextData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class TextData
{
  private string m_text;
  private RectangleF m_bounds;
  private string m_fontName;
  private float m_fontSize;
  private FontStyle m_fontStyle;
  private Color m_fontColor;

  public string Text => this.m_text;

  public RectangleF Bounds => this.m_bounds;

  public string FontName => this.m_fontName;

  public float FontSize => this.m_fontSize;

  public FontStyle FontStyle => this.m_fontStyle;

  public Color FontColor => this.m_fontColor;

  internal TextData(
    string text,
    string fontName,
    FontStyle fontStyle,
    double fontSize,
    Color fontColor,
    RectangleF bounds)
  {
    this.m_text = text;
    this.m_fontName = fontName;
    this.m_fontStyle = fontStyle;
    this.m_fontSize = (float) fontSize;
    this.m_fontColor = fontColor;
    this.m_bounds = bounds;
  }
}
