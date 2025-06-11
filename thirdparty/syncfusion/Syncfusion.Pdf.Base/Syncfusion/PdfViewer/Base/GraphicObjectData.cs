// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.GraphicObjectData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;
using System.Drawing;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class GraphicObjectData
{
  internal float m_mitterLength;
  private Color m_strokingColor;
  private Color m_nonStokingColor;
  private Font m_font;
  private string m_currentFont;
  private float m_fontSize;
  private float m_textLeading;
  private float m_characterSpacing;
  private float m_wordSpacing;
  internal Matrix Ctm;
  internal Matrix textLineMatrix;
  internal Matrix textMatrix;
  internal Matrix documentMatrix;
  internal Matrix textMatrixUpdate;
  internal System.Drawing.Drawing2D.Matrix drawing2dMatrixCTM;
  internal float HorizontalScaling = 100f;
  internal int Rise;
  internal Matrix transformMatrixTM = new Matrix();
  private Brush m_strokingBrush;
  private Brush m_nonStrokingBrush;
  private Colorspace m_strokingColorspace;
  private Colorspace m_nonStrokingColorspace;
  internal float m_strokingOpacity = 1f;
  internal float m_nonStrokingOpacity = 1f;

  internal Colorspace StrokingColorspace
  {
    get => this.m_strokingColorspace;
    set => this.m_strokingColorspace = value;
  }

  internal Colorspace NonStrokingColorspace
  {
    get => this.m_nonStrokingColorspace;
    set => this.m_nonStrokingColorspace = value;
  }

  internal Brush StrokingBrush
  {
    get => this.m_strokingBrush;
    set => this.m_strokingBrush = value;
  }

  internal Brush NonStrokingBrush
  {
    get => this.m_nonStrokingBrush;
    set => this.m_nonStrokingBrush = value;
  }

  internal Color StrokingColor
  {
    get => this.m_strokingColor;
    set => this.m_strokingColor = value;
  }

  internal Color NonStrokingColor
  {
    get => this.m_nonStokingColor;
    set => this.m_nonStokingColor = value;
  }

  internal Font Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  internal string CurrentFont
  {
    get => this.m_currentFont;
    set => this.m_currentFont = value;
  }

  internal float FontSize
  {
    get => this.m_fontSize;
    set => this.m_fontSize = value;
  }

  internal float TextLeading
  {
    get => this.m_textLeading;
    set => this.m_textLeading = value;
  }

  internal float CharacterSpacing
  {
    get => this.m_characterSpacing;
    set => this.m_characterSpacing = value;
  }

  internal float WordSpacing
  {
    get => this.m_wordSpacing;
    set => this.m_wordSpacing = value;
  }

  internal GraphicObjectData()
  {
  }
}
