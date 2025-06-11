// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaRectangleField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfLoadedXfaRectangleField : PdfLoadedXfaStyledField
{
  private PdfXfaCorner m_corner = new PdfXfaCorner();
  private float m_radius;
  private PdfXfaRotateAngle m_rotate;
  private PdfXfaBorder m_border = new PdfXfaBorder();
  private string m_toolTip;

  public new PdfXfaBorder Border
  {
    get => this.m_border;
    set
    {
      if (value == null)
        return;
      this.m_border = value;
    }
  }

  public new string ToolTip
  {
    get => this.m_toolTip;
    set
    {
      if (value == null)
        return;
      this.m_toolTip = value;
    }
  }

  public PdfXfaCorner Corner
  {
    get => this.m_corner;
    set => this.m_corner = value;
  }

  public new PdfXfaRotateAngle Rotate
  {
    get => this.m_rotate;
    set => this.m_rotate = value;
  }

  internal void ReadField(XmlNode node)
  {
    this.currentNode = node;
    this.ReadCommonProperties(node);
    if (node["value"] == null || node["value"]["rectangle"] == null)
      return;
    XmlNode node1 = (XmlNode) node["value"]["rectangle"];
    if (node1["fill"] == null)
      return;
    if (this.Border == null)
      this.Border = new PdfXfaBorder();
    this.Border.ReadFillBrush(node1);
  }

  internal void DrawRectangle(PdfGraphics graphics, RectangleF bounds)
  {
    RectangleF rectangleF1 = new RectangleF();
    SizeF size = this.GetSize();
    rectangleF1 = new RectangleF(new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top), new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom)));
    PdfPen pen = PdfPens.Black;
    if (this.Border != null)
      pen = new PdfPen(this.Border.Color.IsEmpty ? new PdfColor(Color.Black) : this.Border.Color, this.Border.Width);
    PdfGraphics pdfGraphics = graphics;
    pdfGraphics.Save();
    pdfGraphics.TranslateTransform(rectangleF1.X, rectangleF1.Y);
    pdfGraphics.RotateTransform((float) -this.GetRotationAngle());
    RectangleF rectangleF2 = RectangleF.Empty;
    switch (this.GetRotationAngle())
    {
      case 0:
        rectangleF2 = new RectangleF(0.0f, 0.0f, rectangleF1.Width, rectangleF1.Height);
        break;
      case 90:
        rectangleF2 = new RectangleF(-rectangleF1.Height, 0.0f, rectangleF1.Height, rectangleF1.Width);
        break;
      case 180:
        rectangleF2 = new RectangleF(-rectangleF1.Width, -rectangleF1.Height, rectangleF1.Width, rectangleF1.Height);
        break;
      case 270:
        rectangleF2 = new RectangleF(0.0f, -rectangleF1.Width, rectangleF1.Height, rectangleF1.Width);
        break;
    }
    PdfBrush brush = this.Border.GetBrush(rectangleF2);
    if (this.Visibility != PdfXfaVisibility.Visible)
    {
      pen = (PdfPen) null;
      brush = (PdfBrush) null;
    }
    pdfGraphics.DrawRectangle(pen, brush, rectangleF2);
    pdfGraphics.Restore();
  }
}
