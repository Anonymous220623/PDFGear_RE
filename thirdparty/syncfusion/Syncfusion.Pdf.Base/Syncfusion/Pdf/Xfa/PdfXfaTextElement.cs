// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaTextElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaTextElement : PdfXfaField
{
  private string m_text = string.Empty;
  private PdfXfaRotateAngle m_rotate;
  private PdfFont m_font;
  private PdfColor m_foreColor;
  private float m_width;
  private float m_height;
  private PdfXfaHorizontalAlignment m_hAlign;
  private PdfXfaVerticalAlignment m_vAlign;
  internal PdfXfaForm parent;

  public PdfXfaHorizontalAlignment HorizontalAlignment
  {
    get => this.m_hAlign;
    set => this.m_hAlign = value;
  }

  public PdfXfaVerticalAlignment VerticalAlignment
  {
    get => this.m_vAlign;
    set => this.m_vAlign = value;
  }

  public string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  public PdfXfaRotateAngle Rotate
  {
    get => this.m_rotate;
    set => this.m_rotate = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  public PdfColor ForeColor
  {
    get => this.m_foreColor;
    set
    {
      if (!(value != PdfColor.Empty))
        return;
      this.m_foreColor = value;
    }
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

  public PdfXfaTextElement()
  {
    this.Name = "";
    this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
  }

  public PdfXfaTextElement(string text)
  {
    this.Text = text;
    this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
  }

  public PdfXfaTextElement(string text, float width, float height)
  {
    this.Text = text;
    this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    this.Width = width;
    this.Height = height;
  }

  public PdfXfaTextElement(string text, PdfFont font)
  {
    this.m_font = font;
    this.Text = text;
    this.Name = "";
  }

  public PdfXfaTextElement(string text, PdfFont font, float width, float height)
  {
    this.m_font = font;
    this.Text = text;
    this.Name = "";
    this.Width = width;
    this.Height = height;
  }

  internal void Save(XfaWriter xfaWriter)
  {
    SizeF sizeF = this.Font.MeasureString(this.m_text);
    if ((double) this.Height <= 0.0)
      this.Height = sizeF.Height;
    if ((double) this.Width <= 0.0)
      this.Width = sizeF.Width;
    if (this.Name == "" || this.Name == string.Empty)
      this.Name = "StaticText" + xfaWriter.m_fieldCount++.ToString();
    xfaWriter.Write.WriteStartElement("draw");
    xfaWriter.Write.WriteAttributeString("name", this.Name);
    xfaWriter.SetRPR(this.Rotate, this.Visibility, false);
    xfaWriter.SetSize(this.Height, this.Width, 0.0f, 0.0f);
    Dictionary<string, string> values = new Dictionary<string, string>();
    xfaWriter.WriteUI("textEdit", values, (PdfXfaBorder) null);
    xfaWriter.WriteValue(this.m_text, 0);
    xfaWriter.WriteFontInfo(this.Font, this.ForeColor);
    xfaWriter.WriteMargins(this.Margins);
    xfaWriter.WritePragraph(this.m_vAlign, this.m_hAlign);
    xfaWriter.Write.WriteEndElement();
  }

  internal void SaveAcroForm(PdfPage page, RectangleF bounds)
  {
    PdfStringFormat format = new PdfStringFormat();
    format.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    format.Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    RectangleF rectangleF = new RectangleF();
    PdfBrush brush = PdfBrushes.Black;
    if (this.ForeColor != PdfColor.Empty && ((double) this.ForeColor.Red != 0.0 || (double) this.ForeColor.Green != 0.0 || (double) this.ForeColor.Blue != 0.0))
      brush = (PdfBrush) new PdfSolidBrush(this.ForeColor);
    SizeF size = this.GetSize();
    rectangleF = new RectangleF(new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top), new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom)));
    PdfGraphics graphics = page.Graphics;
    graphics.Save();
    SizeF empty = SizeF.Empty;
    if (this.Font != null)
      this.Font.MeasureString(this.Text);
    graphics.TranslateTransform(rectangleF.X, rectangleF.Y);
    graphics.RotateTransform((float) -this.GetRotationAngle());
    RectangleF layoutRectangle = RectangleF.Empty;
    switch (this.GetRotationAngle())
    {
      case 0:
        layoutRectangle = new RectangleF(0.0f, 0.0f, rectangleF.Width, rectangleF.Height);
        break;
      case 90:
        layoutRectangle = new RectangleF(-rectangleF.Height, 0.0f, rectangleF.Height, rectangleF.Width);
        break;
      case 180:
        layoutRectangle = new RectangleF(-rectangleF.Width, -rectangleF.Height, rectangleF.Width, rectangleF.Height);
        break;
      case 270:
        layoutRectangle = new RectangleF(0.0f, -rectangleF.Width, rectangleF.Height, rectangleF.Width);
        break;
    }
    graphics.DrawString(this.Text, this.Font, brush, layoutRectangle, format);
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
