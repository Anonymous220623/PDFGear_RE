// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfOrderedMarker
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public class PdfOrderedMarker : PdfMarker
{
  private PdfNumberStyle m_style;
  private int m_startNumber = 1;
  private string m_delimiter;
  private string m_suffix;
  private int m_currentIndex;

  public PdfNumberStyle Style
  {
    get => this.m_style;
    set => this.m_style = value;
  }

  public int StartNumber
  {
    get => this.m_startNumber;
    set
    {
      this.m_startNumber = value > 0 ? value : throw new ArgumentException("Start number should be greater than 0");
    }
  }

  public string Delimiter
  {
    get => this.m_delimiter == string.Empty || this.m_delimiter == null ? "." : this.m_delimiter;
    set => this.m_delimiter = value;
  }

  public string Suffix
  {
    get => this.m_suffix == null || this.m_suffix == string.Empty ? "." : this.m_suffix;
    set => this.m_suffix = value;
  }

  internal int CurrentIndex
  {
    get => this.m_currentIndex;
    set => this.m_currentIndex = value;
  }

  public PdfOrderedMarker(PdfNumberStyle style, string delimiter, string suffix, PdfFont font)
  {
    this.m_style = style;
    this.m_delimiter = delimiter;
    this.m_suffix = suffix;
    this.Font = font;
  }

  public PdfOrderedMarker(PdfNumberStyle style, string suffix, PdfFont font)
    : this(style, string.Empty, suffix, font)
  {
  }

  public PdfOrderedMarker(PdfNumberStyle style, PdfFont font)
    : this(style, string.Empty, string.Empty, font)
  {
  }

  internal void Draw(PdfGraphics graphics, PointF point)
  {
    string number = this.GetNumber();
    graphics.DrawString(number + this.Suffix, this.Font, this.Brush, point);
  }

  internal void Draw(PdfPage page, PointF point) => this.Draw(page.Graphics, point);

  internal string GetNumber()
  {
    return PdfNumbersConvertor.Convert(this.m_startNumber + this.m_currentIndex, this.m_style);
  }
}
