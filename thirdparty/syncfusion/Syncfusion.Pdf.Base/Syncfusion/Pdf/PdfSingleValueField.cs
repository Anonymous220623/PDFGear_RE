// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfSingleValueField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfSingleValueField : PdfDynamicField
{
  private Dictionary<PdfDocumentBase, PdfTemplateValuePair> m_list = new Dictionary<PdfDocumentBase, PdfTemplateValuePair>();
  private List<PdfGraphics> m_painterGraphics = new List<PdfGraphics>();

  public PdfSingleValueField()
  {
  }

  public PdfSingleValueField(PdfFont font)
    : base(font)
  {
  }

  public PdfSingleValueField(PdfFont font, PdfBrush brush)
    : base(font, brush)
  {
  }

  public PdfSingleValueField(PdfFont font, RectangleF bounds)
    : base(font, bounds)
  {
  }

  protected internal override void PerformDraw(
    PdfGraphics graphics,
    PointF location,
    float scalingX,
    float scalingY)
  {
    if (graphics.Page is PdfPage)
    {
      base.PerformDraw(graphics, location, scalingX, scalingY);
      PdfPage pageFromGraphics = PdfDynamicField.GetPageFromGraphics(graphics);
      if (pageFromGraphics.Section.m_document is PdfLoadedDocument)
      {
        PdfLoadedDocument document1 = pageFromGraphics.Section.m_document as PdfLoadedDocument;
        base.PerformDraw(graphics, location, scalingX, scalingY);
        PdfDynamicField.GetPageFromGraphics(graphics);
        PdfLoadedDocument document2 = pageFromGraphics.Section.m_document as PdfLoadedDocument;
        string s = this.GetValue(graphics);
        if (this.m_list.ContainsKey((PdfDocumentBase) document2))
        {
          PdfTemplateValuePair templateValuePair = this.m_list[(PdfDocumentBase) document1];
          if (templateValuePair.Value != s)
          {
            SizeF size = this.ObtainSize();
            templateValuePair.Template.Reset(size);
            templateValuePair.Template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, size), this.StringFormat);
          }
          if (this.m_painterGraphics.Contains(graphics))
            return;
          PointF location1 = new PointF(location.X + this.Location.X, location.Y + this.Location.Y);
          graphics.DrawPdfTemplate(templateValuePair.Template, location1, new SizeF(templateValuePair.Template.Width * scalingX, templateValuePair.Template.Height * scalingY));
          this.m_painterGraphics.Add(graphics);
        }
        else
        {
          PdfTemplate template = new PdfTemplate(this.ObtainSize());
          this.m_list[(PdfDocumentBase) document1] = new PdfTemplateValuePair(template, s);
          template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, this.ObtainSize()), this.StringFormat);
          PointF location2 = new PointF(location.X + this.Location.X, location.Y + this.Location.Y);
          graphics.DrawPdfTemplate(template, location2, new SizeF(template.Width * scalingX, template.Height * scalingY));
          this.m_painterGraphics.Add(graphics);
        }
      }
      else
      {
        PdfDocument document = pageFromGraphics.Document;
        string s = this.GetValue(graphics);
        if (this.m_list.ContainsKey((PdfDocumentBase) document))
        {
          PdfTemplateValuePair templateValuePair = this.m_list[(PdfDocumentBase) document];
          if (templateValuePair.Value != s)
          {
            SizeF size = this.ObtainSize();
            templateValuePair.Template.Reset(size);
            templateValuePair.Template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, size), this.StringFormat);
          }
          if (this.m_painterGraphics.Contains(graphics))
            return;
          PointF location3 = new PointF(location.X + this.Location.X, location.Y + this.Location.Y);
          graphics.DrawPdfTemplate(templateValuePair.Template, location3, new SizeF(templateValuePair.Template.Width * scalingX, templateValuePair.Template.Height * scalingY));
          this.m_painterGraphics.Add(graphics);
        }
        else
        {
          PdfTemplate template = new PdfTemplate(this.ObtainSize());
          this.m_list[(PdfDocumentBase) document] = new PdfTemplateValuePair(template, s);
          template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, this.ObtainSize()), this.StringFormat);
          PointF location4 = new PointF(location.X + this.Location.X, location.Y + this.Location.Y);
          graphics.DrawPdfTemplate(template, location4, new SizeF(template.Width * scalingX, template.Height * scalingY));
          this.m_painterGraphics.Add(graphics);
        }
      }
    }
    else
    {
      if (!(graphics.Page is PdfLoadedPage))
        return;
      base.PerformDraw(graphics, location, scalingX, scalingY);
      PdfLoadedDocument document = PdfDynamicField.GetLoadedPageFromGraphics(graphics).Document as PdfLoadedDocument;
      string s = this.GetValue(graphics);
      if (this.m_list.ContainsKey((PdfDocumentBase) document))
      {
        PdfTemplateValuePair templateValuePair = this.m_list[(PdfDocumentBase) document];
        if (templateValuePair.Value != s)
        {
          SizeF size = this.ObtainSize();
          templateValuePair.Template.Reset(size);
          templateValuePair.Template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, size), this.StringFormat);
        }
        if (this.m_painterGraphics.Contains(graphics))
          return;
        PointF location5 = new PointF(location.X + this.Location.X, location.Y + this.Location.Y);
        graphics.DrawPdfTemplate(templateValuePair.Template, location5, new SizeF(templateValuePair.Template.Width * scalingX, templateValuePair.Template.Height * scalingY));
        this.m_painterGraphics.Add(graphics);
      }
      else
      {
        PdfTemplate template = new PdfTemplate(this.ObtainSize());
        this.m_list[(PdfDocumentBase) document] = new PdfTemplateValuePair(template, s);
        template.Graphics.DrawString(s, this.ObtainFont(), this.Pen, this.ObtainBrush(), new RectangleF(PointF.Empty, this.ObtainSize()), this.StringFormat);
        PointF location6 = new PointF(location.X + this.Location.X, location.Y + this.Location.Y);
        graphics.DrawPdfTemplate(template, location6, new SizeF(template.Width * scalingX, template.Height * scalingY));
        this.m_painterGraphics.Add(graphics);
      }
    }
  }
}
