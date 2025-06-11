// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaTextElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfLoadedXfaTextElement : PdfLoadedXfaStyledField
{
  private string m_text = string.Empty;
  internal string alterText = string.Empty;
  internal bool isExData;

  public string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal PdfLoadedXfaTextElement()
  {
  }

  internal void Read(XmlNode node)
  {
    this.currentNode = node;
    this.ReadCommonProperties(node);
    if (node["value"] != null && node["value"]["text"] != null)
    {
      this.Text = node["value"]["text"].InnerText;
    }
    else
    {
      if (node["value"] == null || node["value"]["exData"] == null)
        return;
      this.Text = node["value"]["exData"].OuterXml;
      this.alterText = node["value"]["exData"].InnerText;
      this.isExData = true;
    }
  }

  internal void DrawTextElement(PdfGraphics graphics, RectangleF bounds)
  {
    PdfStringFormat format = new PdfStringFormat();
    format.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    format.Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    RectangleF tempBounds = new RectangleF();
    PdfBrush brush = PdfBrushes.Black;
    if (!this.ForeColor.IsEmpty)
      brush = (PdfBrush) new PdfSolidBrush(this.ForeColor);
    SizeF size = this.GetSize();
    tempBounds = new RectangleF(new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top), new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom)));
    PdfGraphics graphics1 = graphics;
    if (this.CompleteBorder != null && this.CompleteBorder.Visibility != PdfXfaVisibility.Hidden && this.CompleteBorder.Visibility != PdfXfaVisibility.Invisible)
    {
      RectangleF rectangleF = new RectangleF(bounds.Location, size);
      graphics1.Save();
      graphics1.TranslateTransform(rectangleF.X, rectangleF.Y);
      graphics1.RotateTransform((float) -this.GetRotationAngle());
      rectangleF = this.GetRenderingRect(rectangleF);
      this.CompleteBorder.DrawBorder(graphics1, rectangleF);
      graphics1.Restore();
    }
    graphics1.Save();
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 10f, PdfFontStyle.Regular);
    if ((double) tempBounds.Height < (double) this.Font.Height)
      tempBounds.Height = (double) tempBounds.Height + ((double) this.Margins.Top + (double) this.Margins.Bottom) < (double) this.Font.Height ? this.Font.Height : this.Font.Height;
    graphics1.TranslateTransform(tempBounds.X, tempBounds.Y);
    graphics1.RotateTransform((float) -this.GetRotationAngle());
    RectangleF renderingRect = this.GetRenderingRect(tempBounds);
    graphics1.DrawString(this.isExData ? this.alterText : this.Text, this.Font, brush, renderingRect, format);
    graphics1.Restore();
  }

  internal new SizeF GetSize()
  {
    if ((double) this.Width <= 0.0)
    {
      if (this.currentNode.Attributes["maxW"] != null)
        this.Width = this.ConvertToPoint(this.currentNode.Attributes["maxW"].Value);
      if (this.currentNode.Attributes["minW"] != null)
        this.Width = this.ConvertToPoint(this.currentNode.Attributes["minW"].Value);
    }
    if ((double) this.Height <= 0.0)
    {
      if (this.currentNode.Attributes["maxH"] != null)
        this.Height = this.ConvertToPoint(this.currentNode.Attributes["maxH"].Value);
      if (this.currentNode.Attributes["minH"] != null)
      {
        this.Height = this.ConvertToPoint(this.currentNode.Attributes["minH"].Value);
        if (this.Font != null)
        {
          if (this.isExData)
          {
            float height = this.Font.MeasureString(this.alterText, this.Width).Height;
            if ((double) height > (double) this.Height)
              this.Height = height;
          }
          else if (this.Text != string.Empty)
          {
            float height = this.Font.MeasureString(this.Text, this.Width).Height;
            if ((double) height > (double) this.Height)
              this.Height = height;
          }
          else if ((double) this.Font.Height > (double) this.Height)
            this.Height = this.Font.Height + 0.5f;
        }
        if (this.parent is PdfLoadedXfaForm parent && parent.FlowDirection != PdfLoadedXfaFlowDirection.Row)
          this.Height += this.Margins.Top + this.Margins.Bottom;
      }
      else if (this.currentNode.Attributes["h"] == null && this.Font != null)
      {
        if (this.isExData)
          this.Height = this.Font.MeasureString(this.alterText, this.Width).Height;
        else if (this.Text != string.Empty)
          this.Height = this.Font.MeasureString(this.Text, this.Width).Height;
        else if ((double) this.Font.Height > (double) this.Height)
          this.Height = this.Font.Height + 0.5f;
      }
    }
    return this.Rotate == PdfXfaRotateAngle.RotateAngle270 || this.Rotate == PdfXfaRotateAngle.RotateAngle90 ? new SizeF(this.Height, this.Width) : new SizeF(this.Width, this.Height);
  }
}
