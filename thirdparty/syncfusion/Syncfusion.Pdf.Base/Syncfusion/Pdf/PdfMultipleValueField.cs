// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfMultipleValueField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfMultipleValueField : PdfDynamicField
{
  private Dictionary<PdfGraphics, PdfTemplateValuePair> m_list = new Dictionary<PdfGraphics, PdfTemplateValuePair>();
  private PdfTag m_tag;

  public PdfTag PdfTag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  public PdfMultipleValueField()
  {
  }

  public PdfMultipleValueField(PdfFont font)
    : base(font)
  {
  }

  public PdfMultipleValueField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfMultipleValueField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  protected internal override void PerformDraw(
    PdfGraphics graphics,
    PointF location,
    float scalingX,
    float scalingY)
  {
    base.PerformDraw(graphics, location, scalingX, scalingY);
    string s = this.GetValue(graphics);
    if (this.m_list.ContainsKey(graphics))
    {
      PdfTemplateValuePair templateValuePair = this.m_list[graphics];
      if (!(templateValuePair.Value != s))
        return;
      SizeF size = this.ObtainSize();
      templateValuePair.Template.Reset(size);
      templateValuePair.Template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, size), this.StringFormat);
    }
    else
    {
      PdfTemplate template = new PdfTemplate(this.ObtainSize());
      if (this.PdfTag != null)
        template.Graphics.Tag = this.PdfTag;
      this.m_list[graphics] = new PdfTemplateValuePair(template, s);
      template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, this.ObtainSize()), this.StringFormat);
      PointF location1 = new PointF(location.X + this.Location.X, location.Y + this.Location.Y);
      if (graphics.IsTaggedPdf && template.Graphics.Tag == null)
        template.Graphics.Tag = (PdfTag) new PdfArtifact();
      graphics.DrawPdfTemplate(template, location1, new SizeF(template.Width * scalingX, template.Height * scalingY));
    }
  }
}
