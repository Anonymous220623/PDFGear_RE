// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfTextWebLink
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfTextWebLink : PdfTextElement
{
  private string m_url;
  private PdfUriAnnotation m_uriAnnotation;

  public string Url
  {
    get => this.m_url;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException("url");
        case "":
          throw new ArgumentException("Url - string can not be empty");
        default:
          this.m_url = value;
          break;
      }
    }
  }

  public PdfLayoutResult DrawTextWebLink(PdfPage page, PointF location)
  {
    SizeF size = this.Font.MeasureString(this.Value, this.StringFormat);
    this.m_uriAnnotation = new PdfUriAnnotation(this.CalculateBounds(location, size), this.Url);
    this.m_uriAnnotation.Border = new PdfAnnotationBorder(0.0f, 0.0f, 0.0f);
    if (page.Document != null && page.Document.AutoTag)
    {
      if (this.PdfTag == null)
        this.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.Link);
      this.m_uriAnnotation.PdfTag = this.PdfTag;
    }
    page.Annotations.Add((PdfAnnotation) this.m_uriAnnotation);
    this.PdfTag = (PdfTag) null;
    return this.Draw(page, location);
  }

  public void DrawTextWebLink(PdfGraphics graphics, PointF location)
  {
    if (graphics.Page is PdfLoadedPage)
    {
      SizeF size = this.Font.MeasureString(this.Value, this.StringFormat);
      this.m_uriAnnotation = new PdfUriAnnotation(this.CalculateBounds(location, size), this.Url);
      this.m_uriAnnotation.Border = new PdfAnnotationBorder(0.0f, 0.0f, 0.0f);
      graphics.Page.Annotations.Add((PdfAnnotation) this.m_uriAnnotation);
      PdfGraphicsState state = graphics.Save();
      this.AnnotationRotateAndTransform(graphics);
      this.Draw(graphics, location);
      graphics.Restore(state);
    }
    else
    {
      PdfPage pdfPage = new PdfPage();
      SizeF size = this.Font.MeasureString(this.Value, this.StringFormat);
      this.m_uriAnnotation = new PdfUriAnnotation(this.CalculateBounds(location, size), this.Url);
      this.m_uriAnnotation.Border = new PdfAnnotationBorder(0.0f, 0.0f, 0.0f);
      (graphics.Page as PdfPage).Annotations.Add((PdfAnnotation) this.m_uriAnnotation);
      this.Draw(graphics, location);
    }
  }

  private RectangleF CalculateBounds(PointF location, SizeF size)
  {
    RectangleF bounds = new RectangleF(location, size);
    if (this.StringFormat != null)
    {
      if (this.StringFormat.Alignment == PdfTextAlignment.Right)
        bounds = new RectangleF(location.X - size.Width, location.Y, size.Width, size.Height);
      else if (this.StringFormat.Alignment == PdfTextAlignment.Center)
        bounds = new RectangleF(location.X - size.Width / 2f, location.Y, size.Width, size.Height);
    }
    return bounds;
  }

  internal void AnnotationRotateAndTransform(PdfGraphics graphics)
  {
    switch (graphics.Page.Rotation)
    {
      case PdfPageRotateAngle.RotateAngle90:
        graphics.TranslateTransform(graphics.Page.Size.Height, 0.0f);
        graphics.RotateTransform(90f);
        break;
      case PdfPageRotateAngle.RotateAngle180:
        graphics.TranslateTransform(graphics.Page.Size.Width, graphics.Page.Size.Height);
        graphics.RotateTransform(180f);
        break;
      case PdfPageRotateAngle.RotateAngle270:
        graphics.TranslateTransform(0.0f, graphics.Page.Size.Width);
        graphics.RotateTransform(270f);
        break;
    }
  }
}
