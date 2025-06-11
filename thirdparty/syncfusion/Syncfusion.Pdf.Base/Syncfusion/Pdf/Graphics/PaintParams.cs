// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PaintParams
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class PaintParams
{
  private PdfBrush m_backBrush;
  private PdfBrush m_foreBrush;
  private float m_borderWidth = 1f;
  private PdfPen m_borderPen;
  private PdfBorderStyle m_borderStyle;
  private RectangleF m_bounds = RectangleF.Empty;
  private PdfBrush m_shadowBrush;
  private int m_rotationAngle;
  private bool m_insertSpace;
  private PdfPageRotateAngle m_pageRotationAngle;
  private bool m_isFlatten;
  internal bool m_complexScript;
  internal PdfTextDirection m_textDirection;
  internal float m_lineSpacing;

  public PaintParams()
  {
  }

  public PaintParams(
    RectangleF bounds,
    PdfBrush backBrush,
    PdfBrush foreBrush,
    PdfPen borderPen,
    PdfBorderStyle style,
    float borderWidth,
    PdfBrush shadowBrush,
    int rotationAngle)
  {
    this.m_bounds = bounds;
    this.m_backBrush = backBrush;
    this.m_foreBrush = foreBrush;
    this.m_borderPen = borderPen;
    this.m_borderStyle = style;
    this.m_borderWidth = borderWidth;
    this.m_shadowBrush = shadowBrush;
    this.m_rotationAngle = rotationAngle;
  }

  public PdfBrush BackBrush
  {
    get => this.m_backBrush;
    set => this.m_backBrush = value;
  }

  public PdfBrush ForeBrush
  {
    get => this.m_foreBrush;
    set => this.m_foreBrush = value;
  }

  public PdfPen BorderPen
  {
    get => this.m_borderPen;
    set => this.m_borderPen = value;
  }

  public PdfBorderStyle BorderStyle
  {
    get => this.m_borderStyle;
    set => this.m_borderStyle = value;
  }

  public float BorderWidth
  {
    get => this.m_borderWidth;
    set => this.m_borderWidth = value;
  }

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  public PdfBrush ShadowBrush
  {
    get => this.m_shadowBrush;
    set => this.m_shadowBrush = value;
  }

  public int RotationAngle
  {
    get => this.m_rotationAngle;
    set => this.m_rotationAngle = value;
  }

  internal bool InsertSpace
  {
    get => this.m_insertSpace;
    set => this.m_insertSpace = value;
  }

  internal PdfPageRotateAngle PageRotationAngle
  {
    get => this.m_pageRotationAngle;
    set => this.m_pageRotationAngle = value;
  }

  internal bool isFlatten
  {
    get => this.m_isFlatten;
    set => this.m_isFlatten = value;
  }
}
