// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaButtonField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfLoadedXfaButtonField : PdfLoadedXfaStyledField
{
  private PdfHighlightMode m_highlight;
  private string m_rolloverText;
  private string m_downText;
  private string m_content = string.Empty;

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

  internal void Read(XmlNode node)
  {
    this.currentNode = node;
    this.ReadCommonProperties(node);
    if (node["value"] != null && node["value"]["text"] != null)
    {
      this.Content = node["value"]["text"].InnerText;
    }
    else
    {
      if (node["value"] == null || node["value"]["exData"] == null)
        return;
      this.Content = node["value"]["exData"].InnerText;
    }
  }

  internal void DrawField(PdfGraphics graphics, RectangleF bounds)
  {
    PdfStringFormat format = new PdfStringFormat();
    format.LineAlignment = this.Caption != null ? (PdfVerticalAlignment) this.Caption.VerticalAlignment : (PdfVerticalAlignment) this.VerticalAlignment;
    format.Alignment = this.ConvertToPdfTextAlignment(this.Caption != null ? this.Caption.HorizontalAlignment : this.HorizontalAlignment);
    PdfBrush foreBrush = PdfBrushes.Black;
    if (!this.ForeColor.IsEmpty)
      foreBrush = (PdfBrush) new PdfSolidBrush(this.ForeColor);
    PdfBrush backBrush = (PdfBrush) null;
    PdfPen borderPen = (PdfPen) null;
    PdfBorderStyle style = PdfBorderStyle.Solid;
    int borderWidth = 0;
    if (this.CompleteBorder != null && this.CompleteBorder.Visibility != PdfXfaVisibility.Hidden && this.CompleteBorder.Visibility != PdfXfaVisibility.Invisible)
    {
      backBrush = this.CompleteBorder.GetBrush(bounds);
      borderPen = this.CompleteBorder.GetFlattenPen();
      style = this.CompleteBorder.GetBorderStyle();
      borderWidth = (int) this.CompleteBorder.Width;
    }
    if (borderWidth == 0 && borderPen != null)
      borderWidth = (int) borderPen.Width;
    RectangleF bounds1 = new RectangleF();
    SizeF size = this.GetSize();
    bounds1 = new RectangleF(new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top), new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom)));
    if (this.Caption != null && this.Caption.Text != string.Empty)
      this.CheckUnicodeFont(this.Caption.Text);
    graphics.Save();
    PaintParams paintParams = new PaintParams(bounds1, backBrush, foreBrush, borderPen, style, (float) borderWidth, (PdfBrush) null, this.GetRotationAngle());
    FieldPainter.DrawButton(graphics, paintParams, this.Caption.Text, this.Font, format);
    graphics.Restore();
  }
}
