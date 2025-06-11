// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaLine
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaLine : PdfXfaField
{
  internal PointF m_startLocation;
  internal PointF m_endLocation;
  private float m_thickness = 1f;
  private PdfColor m_color;
  internal PdfXfaForm parent;

  public PdfColor Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  public float Thickness
  {
    get => this.m_thickness;
    set => this.m_thickness = value;
  }

  public PdfXfaLine(PointF startLocation, PointF endLocation, float thickness)
  {
    this.m_startLocation = startLocation;
    this.m_endLocation = endLocation;
    this.m_thickness = thickness;
  }

  internal void Save(XfaWriter xfaWriter)
  {
    float num1 = this.m_endLocation.X - this.m_startLocation.X;
    float num2 = this.m_endLocation.Y - this.m_startLocation.Y;
    string slope = "";
    if ((double) num1 < 0.0 || (double) num2 < 0.0)
      slope = "/";
    if ((double) num1 < 0.0 && (double) num2 < 0.0)
      slope = "";
    xfaWriter.Write.WriteStartElement("draw");
    xfaWriter.Write.WriteAttributeString("name", "line" + xfaWriter.m_fieldCount++.ToString());
    xfaWriter.SetRPR(PdfXfaRotateAngle.RotateAngle0, this.Visibility, false);
    xfaWriter.SetSize(Math.Abs(num2), Math.Abs(num1), 0.0f, 0.0f);
    xfaWriter.DrawLine(this.m_thickness, slope, $"{this.Color.R.ToString()},{this.Color.G.ToString()},{this.Color.B.ToString()}");
    xfaWriter.Write.WriteEndElement();
  }

  internal void SaveAcroForm(PdfPage page, RectangleF bounds)
  {
    PdfPen pen = !(this.Color == PdfColor.Empty) ? new PdfPen(this.Color) : PdfPens.Black;
    pen.Width = this.Thickness;
    page.Graphics.DrawLine(pen, bounds.Location, new PointF(bounds.Width + bounds.X, bounds.Height + bounds.Y));
  }

  internal SizeF GetSize()
  {
    return new SizeF(this.m_endLocation.X - this.m_startLocation.X, this.m_endLocation.Y - this.m_startLocation.Y);
  }

  public object Clone() => this.MemberwiseClone();
}
