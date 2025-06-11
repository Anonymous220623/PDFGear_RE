// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfStaticField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfStaticField : PdfAutomaticField
{
  private PdfTemplate m_template;
  private List<PdfGraphics> m_graphicsList = new List<PdfGraphics>();

  public PdfStaticField()
  {
  }

  public PdfStaticField(PdfFont font)
    : base(font)
  {
  }

  public PdfStaticField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfStaticField(PdfFont font, RectangleF bounds)
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
    PointF location1 = new PointF(location.X + this.Location.X, location.Y + this.Location.Y);
    if (this.m_template == null)
    {
      this.m_template = new PdfTemplate(this.ObtainSize());
      this.m_template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, this.ObtainSize()), this.StringFormat);
      graphics.DrawPdfTemplate(this.m_template, location1, new SizeF(this.m_template.Width * scalingX, this.m_template.Height * scalingY));
      this.m_graphicsList.Add(graphics);
    }
    else
    {
      if (this.m_graphicsList.Contains(graphics))
        return;
      graphics.DrawPdfTemplate(this.m_template, location1, new SizeF(this.m_template.Width * scalingX, this.m_template.Height * scalingY));
      this.m_graphicsList.Add(graphics);
    }
  }
}
