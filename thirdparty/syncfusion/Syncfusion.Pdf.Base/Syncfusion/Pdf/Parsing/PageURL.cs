// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PageURL
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class PageURL
{
  private Matrix m_transformPoints;
  private string m_uri;
  private PointF m_currentLocation;
  private float m_textElementWidth;
  private float m_fontSize;

  internal float FontSize => this.m_fontSize;

  internal string URI => this.m_uri;

  internal PointF CurrentLocation => this.m_currentLocation;

  internal Matrix TransformPoints => this.m_transformPoints;

  internal float TextElementWidth => this.m_textElementWidth;

  public PageURL(
    Matrix transformPoints,
    string URI,
    PointF CurrentLocation,
    float TextElementWidth,
    float fontSize)
  {
    this.m_transformPoints = transformPoints;
    this.m_uri = URI;
    this.m_currentLocation = CurrentLocation;
    this.m_textElementWidth = TextElementWidth;
    this.m_fontSize = fontSize;
  }
}
