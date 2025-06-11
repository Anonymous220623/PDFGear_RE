// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaButtonField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaButtonField : PdfXfaStyledField
{
  private PdfHighlightMode m_highlight;
  private string m_rolloverText;
  private string m_downText;
  private string m_content = string.Empty;
  internal new PdfXfaForm parent;

  public PdfHighlightMode Highlight
  {
    get => this.m_highlight;
    set => this.m_highlight = value;
  }

  public string MouseRolloverText
  {
    get => this.m_rolloverText;
    set
    {
      if (value == null)
        return;
      this.m_rolloverText = value;
    }
  }

  public string MouseDownText
  {
    get => this.m_downText;
    set
    {
      if (value == null)
        return;
      this.m_downText = value;
    }
  }

  public string Content
  {
    get => this.m_content;
    set
    {
      if (value == null)
        return;
      this.m_content = value;
    }
  }

  public PdfXfaButtonField(string name, SizeF buttonSize)
  {
    this.Height = buttonSize.Height;
    this.Width = buttonSize.Width;
    this.Name = name;
  }

  public PdfXfaButtonField(string name, float width, float height)
  {
    this.Height = height;
    this.Width = width;
    this.Name = name;
  }

  internal void Save(XfaWriter xfaWriter)
  {
    if (this.Name == "" || this.Name == string.Empty)
      this.Name = "button" + xfaWriter.m_fieldCount++.ToString();
    xfaWriter.Write.WriteStartElement("field");
    xfaWriter.Write.WriteAttributeString("name", this.Name);
    xfaWriter.SetSize(this.Height + this.Margins.Bottom + this.Margins.Top, this.Width + this.Margins.Right + this.Margins.Left, 0.0f, 0.0f);
    xfaWriter.SetRPR(this.Rotate, this.Visibility, this.ReadOnly);
    string str = this.Highlight != PdfHighlightMode.NoHighlighting ? (this.Highlight != PdfHighlightMode.Invert ? this.Highlight.ToString().ToLower() : "inverted") : (string) null;
    xfaWriter.WriteUI("button", new Dictionary<string, string>()
    {
      {
        "highlight",
        str
      }
    }, (PdfXfaBorder) null);
    if (this.Border != null)
    {
      if (this.Border.FillColor != null)
        xfaWriter.DrawBorder(this.Border, this.Border.FillColor);
      else
        xfaWriter.DrawBorder(this.Border, (PdfXfaBrush) new PdfXfaSolidBrush(new PdfColor((byte) 212, (byte) 208 /*0xD0*/, (byte) 200)));
    }
    this.SetMFTP(xfaWriter);
    if (this.Content != null)
      xfaWriter.WriteCaption(this.Content, 0.0f, this.HorizontalAlignment, this.VerticalAlignment);
    xfaWriter.WriteItems(this.MouseRolloverText, this.MouseDownText);
    xfaWriter.Write.WriteEndElement();
  }

  internal PdfField SaveAcroForm(PdfPage page, RectangleF bounds, string name)
  {
    PdfButtonField field = new PdfButtonField((PdfPageBase) page, name);
    if (this.Border != null)
      this.Border.ApplyAcroBorder((PdfStyledField) field);
    if (this.Border != null && this.Border.FillColor == null)
      field.BackColor = new PdfColor((byte) 212, (byte) 208 /*0xD0*/, (byte) 200);
    field.TextAlignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    if (this.ReadOnly || this.parent.ReadOnly || this.parent.m_isReadOnly)
      field.ReadOnly = true;
    RectangleF rectangleF = new RectangleF();
    SizeF size = this.GetSize();
    rectangleF.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    rectangleF.Size = new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom));
    if (this.Visibility == PdfXfaVisibility.Invisible)
      field.Visibility = PdfFormFieldVisibility.Hidden;
    field.Text = this.Content;
    if (this.Font == null)
      field.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else
      field.Font = this.Font;
    field.Bounds = rectangleF;
    field.Widget.WidgetAppearance.RotationAngle = this.GetRotationAngle();
    if (this.ForeColor != PdfColor.Empty)
      field.ForeColor = this.ForeColor;
    return (PdfField) field;
  }

  public object Clone() => this.MemberwiseClone();
}
