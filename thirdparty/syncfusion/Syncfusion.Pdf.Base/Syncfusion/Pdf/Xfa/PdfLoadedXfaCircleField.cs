// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaCircleField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfLoadedXfaCircleField : PdfLoadedXfaStyledField
{
  private int m_sweepAngle;
  private int m_startAngle;
  private PdfXfaCircleAppearance m_Appearance;
  private string m_toolTip = string.Empty;
  private PdfXfaRotateAngle m_rotate;
  private float m_width;
  private float m_height;
  private PdfXfaBorder m_border = new PdfXfaBorder();

  internal new PdfXfaBorder Border
  {
    get => this.m_border;
    set
    {
      if (value == null)
        return;
      this.m_border = value;
    }
  }

  internal new string ToolTip
  {
    get => this.m_toolTip;
    set
    {
      if (value == null)
        return;
      this.m_toolTip = value;
    }
  }

  internal int StartAngle
  {
    get => this.m_startAngle;
    set => this.m_startAngle = value;
  }

  internal int SweepAngle
  {
    get => this.m_sweepAngle;
    set => this.m_sweepAngle = value;
  }

  internal PdfXfaCircleAppearance Appearance
  {
    get => this.m_Appearance;
    set => this.m_Appearance = value;
  }

  internal new PdfXfaRotateAngle Rotate
  {
    get => this.m_rotate;
    set => this.m_rotate = value;
  }

  internal new float Width
  {
    set => this.m_width = value;
    get => this.m_width;
  }

  internal new float Height
  {
    set => this.m_height = value;
    get => this.m_height;
  }

  internal void ReadField(XmlNode node)
  {
    this.ReadCommonProperties(node);
    this.Appearance = PdfXfaCircleAppearance.Ellipse;
    if (node["value"] == null || node["value"]["arc"] == null)
      return;
    XmlNode node1 = (XmlNode) node["value"]["arc"];
    if (node1.Attributes["sweepAngle"] != null)
    {
      this.SweepAngle = int.Parse(node1.Attributes["sweepAngle"].Value);
      if (this.SweepAngle != 0)
        this.Appearance = PdfXfaCircleAppearance.Arc;
    }
    if (node1.Attributes["startAngle"] != null)
    {
      this.StartAngle = int.Parse(node1.Attributes["startAngle"].Value);
      if (this.StartAngle != 0)
        this.Appearance = PdfXfaCircleAppearance.Arc;
    }
    if (node1.Attributes["circular"] != null)
      this.Appearance = PdfXfaCircleAppearance.Circle;
    if (node1["fill"] == null)
      return;
    if (this.Border == null)
      this.Border = new PdfXfaBorder();
    this.Border.ReadFillBrush(node1);
  }

  internal void DrawCircle(PdfGraphics graphics, RectangleF bounds)
  {
    RectangleF rectangleF1 = new RectangleF();
    SizeF size = this.GetSize();
    rectangleF1 = new RectangleF(new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top), new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom)));
    if (this.Appearance == PdfXfaCircleAppearance.Circle)
    {
      if ((double) rectangleF1.Width > (double) rectangleF1.Height)
      {
        rectangleF1.X += (float) (((double) rectangleF1.Height + (double) rectangleF1.Width) / 2.0) - rectangleF1.Height;
        rectangleF1.Width = rectangleF1.Height;
      }
      else
      {
        rectangleF1.Y += (float) (((double) rectangleF1.Height + (double) rectangleF1.Width) / 2.0) - rectangleF1.Width;
        rectangleF1.Height = rectangleF1.Width;
      }
    }
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
    if (this.Appearance == PdfXfaCircleAppearance.Arc)
      pdfGraphics.DrawArc(pen, rectangleF2, (float) this.m_startAngle, (float) this.m_sweepAngle);
    else
      pdfGraphics.DrawEllipse(pen, brush, rectangleF2);
    pdfGraphics.Restore();
  }
}
