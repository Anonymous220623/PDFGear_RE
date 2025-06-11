// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridStyleBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public abstract class PdfGridStyleBase : ICloneable
{
  private PdfBrush m_backgroundBrush;
  private PdfBrush m_textBrush;
  private PdfPen m_textPen;
  private PdfFont m_font;
  private PdfPaddings m_gridCellpadding;

  public PdfBrush BackgroundBrush
  {
    get => this.m_backgroundBrush;
    set => this.m_backgroundBrush = value;
  }

  public PdfBrush TextBrush
  {
    get => this.m_textBrush;
    set => this.m_textBrush = value;
  }

  public PdfPen TextPen
  {
    get => this.m_textPen;
    set => this.m_textPen = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  internal PdfPaddings GridCellPadding
  {
    get => this.m_gridCellpadding;
    set => this.m_gridCellpadding = value;
  }

  public object Clone() => this.MemberwiseClone();
}
