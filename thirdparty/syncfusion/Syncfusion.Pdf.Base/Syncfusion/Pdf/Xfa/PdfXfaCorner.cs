// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaCorner
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaCorner
{
  private bool m_isInverted;
  private float m_thickness;
  private float m_radius;
  private PdfXfaBorderStyle m_borderStyle;
  private PdfColor m_borderColor = new PdfColor(Color.Black);
  private PdfXfaVisibility m_visibility;
  private PdfXfaCornerShape m_shape;

  public PdfXfaCornerShape Shape
  {
    get => this.m_shape;
    set => this.m_shape = value;
  }

  public bool IsInverted
  {
    get => this.m_isInverted;
    set => this.m_isInverted = value;
  }

  public PdfColor BorderColor
  {
    get => this.m_borderColor;
    set => this.m_borderColor = value;
  }

  public PdfXfaVisibility Visibility
  {
    get => this.m_visibility;
    set => this.m_visibility = value;
  }

  public float Thickness
  {
    get => this.m_thickness;
    set => this.m_thickness = value;
  }

  public float Radius
  {
    get => this.m_radius;
    set => this.m_radius = value;
  }

  public PdfXfaBorderStyle BorderStyle
  {
    get => this.m_borderStyle;
    set => this.m_borderStyle = value;
  }
}
