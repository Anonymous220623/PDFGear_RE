// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaRectangleField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaRectangleField : PdfXfaField
{
  private PdfXfaCorner m_corner = new PdfXfaCorner();
  private float m_radius;
  private PdfXfaRotateAngle m_rotate;
  private PdfXfaBorder m_border = new PdfXfaBorder();
  private string m_toolTip;
  private float m_width;
  private float m_height;
  internal PdfXfaForm parent;

  public PdfXfaBorder Border
  {
    get => this.m_border;
    set
    {
      if (value == null)
        return;
      this.m_border = value;
    }
  }

  public string ToolTip
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

  public PdfXfaRotateAngle Rotate
  {
    get => this.m_rotate;
    set => this.m_rotate = value;
  }

  public float Width
  {
    set => this.m_width = value;
    get => this.m_width;
  }

  public float Height
  {
    set => this.m_height = value;
    get => this.m_height;
  }

  public PdfXfaRectangleField(string name, SizeF size)
  {
    this.Name = name;
    this.Width = size.Width;
    this.Height = size.Height;
  }

  public PdfXfaRectangleField(string name, float width, float height)
  {
    this.Name = name;
    this.Width = width;
    this.Height = height;
  }

  internal void Save(XfaWriter xfaWriter)
  {
    if (this.Name == "" || this.Name == string.Empty)
      this.Name = "Rectangle" + xfaWriter.m_fieldCount++.ToString();
    xfaWriter.Write.WriteStartElement("draw");
    xfaWriter.Write.WriteAttributeString("name", this.Name);
    xfaWriter.SetSize(this.Height, this.Width, 0.0f, 0.0f);
    xfaWriter.SetRPR(this.Rotate, this.Visibility, false);
    xfaWriter.Write.WriteStartElement("value");
    xfaWriter.Write.WriteStartElement("rectangle");
    if (this.Border.FillColor != null)
      xfaWriter.DrawFillColor(this.Border.FillColor);
    string empty = string.Empty;
    xfaWriter.DrawBorder(this.Border, true);
    if (this.Corner != null)
      xfaWriter.DrawCorner(this.Corner);
    xfaWriter.Write.WriteEndElement();
    xfaWriter.Write.WriteEndElement();
    xfaWriter.WriteMargins(this.Margins);
    if (this.ToolTip != null)
      xfaWriter.WriteToolTip(this.ToolTip);
    xfaWriter.Write.WriteEndElement();
  }

  internal void SaveAcroForm(PdfPage page, RectangleF bounds)
  {
    RectangleF rectangleF1 = new RectangleF();
    SizeF size = this.GetSize();
    rectangleF1 = new RectangleF(new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top), new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom)));
    PdfPen pen = PdfPens.Black;
    if (this.Border != null)
      pen = new PdfPen(this.Border.Color, this.Border.Width);
    PdfGraphics graphics = page.Graphics;
    graphics.Save();
    graphics.TranslateTransform(rectangleF1.X, rectangleF1.Y);
    graphics.RotateTransform((float) -this.GetRotationAngle());
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
    graphics.DrawRectangle(pen, brush, rectangleF2);
    graphics.Restore();
  }

  private int GetRotationAngle()
  {
    int rotationAngle = 0;
    if (this.Rotate != PdfXfaRotateAngle.RotateAngle0)
    {
      switch (this.Rotate)
      {
        case PdfXfaRotateAngle.RotateAngle90:
          rotationAngle = 90;
          break;
        case PdfXfaRotateAngle.RotateAngle180:
          rotationAngle = 180;
          break;
        case PdfXfaRotateAngle.RotateAngle270:
          rotationAngle = 270;
          break;
      }
    }
    return rotationAngle;
  }

  internal SizeF GetSize()
  {
    return this.Rotate == PdfXfaRotateAngle.RotateAngle270 || this.Rotate == PdfXfaRotateAngle.RotateAngle90 ? new SizeF(this.Height, this.Width) : new SizeF(this.Width, this.Height);
  }

  public object Clone() => this.MemberwiseClone();
}
