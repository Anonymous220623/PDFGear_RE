// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfCircleAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfCircleAnnotation : PdfAnnotation
{
  private LineBorder m_border = new LineBorder();
  private float m_borderWidth;

  public PdfPopupAnnotationCollection ReviewHistory
  {
    get
    {
      return this.m_reviewHistory != null ? this.m_reviewHistory : (this.m_reviewHistory = new PdfPopupAnnotationCollection((PdfAnnotation) this, true));
    }
  }

  public PdfPopupAnnotationCollection Comments
  {
    get
    {
      return this.m_comments != null ? this.m_comments : (this.m_comments = new PdfPopupAnnotationCollection((PdfAnnotation) this, false));
    }
  }

  public LineBorder Border
  {
    get => this.m_border;
    set => this.m_border = value;
  }

  public PdfCircleAnnotation(RectangleF rectangle, string text)
    : base(rectangle)
  {
    this.Text = text;
  }

  public PdfCircleAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Circle"));
  }

  protected override void Save()
  {
    this.CheckFlatten();
    this.m_borderWidth = this.Border.BorderWidth != 1 ? (float) this.Border.BorderWidth : this.Border.BorderLineWidth;
    if (!this.m_isStandardAppearance && !this.SetAppearanceDictionary)
      this.Dictionary.SetProperty("AP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance));
    if (this.Flatten || this.SetAppearanceDictionary)
    {
      PdfTemplate appearance = (PdfTemplate) null;
      if (!this.m_isStandardAppearance && this.Flatten && !this.SetAppearanceDictionary)
        appearance = this.Appearance.Normal;
      if (appearance == null)
        appearance = this.CreateAppearance();
      if (this.Flatten)
      {
        if (appearance != null)
        {
          if (this.Page != null)
            this.FlattenAnnotation((PdfPageBase) this.Page, appearance);
          else if (this.LoadedPage != null)
            this.FlattenAnnotation((PdfPageBase) this.LoadedPage, appearance);
        }
      }
      else if (appearance != null)
      {
        this.Appearance.Normal = appearance;
        this.Dictionary.SetProperty("AP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance));
      }
    }
    if (!this.Flatten)
    {
      base.Save();
      if (this.Color.IsEmpty)
        this.Dictionary.SetProperty("C", (IPdfPrimitive) this.Color.ToArray());
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
    }
    if (!this.FlattenPopUps)
      return;
    this.FlattenPopup();
  }

  private void FlattenAnnotation(PdfPageBase page, PdfTemplate appearance)
  {
    PdfGraphics layerGraphics = this.GetLayerGraphics();
    page.Graphics.Save();
    RectangleF templateBounds = this.CalculateTemplateBounds(this.Bounds, page, appearance, true);
    if ((double) this.Opacity < 1.0)
      page.Graphics.SetTransparency(this.Opacity);
    if (layerGraphics != null)
      layerGraphics.DrawPdfTemplate(appearance, templateBounds.Location, templateBounds.Size);
    else
      page.Graphics.DrawPdfTemplate(appearance, templateBounds.Location, templateBounds.Size);
    this.RemoveAnnoationFromPage(page, (PdfAnnotation) this);
    page.Graphics.Restore();
  }

  private PdfTemplate CreateAppearance()
  {
    RectangleF rect = new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height);
    PdfTemplate appearance = new PdfTemplate(rect);
    this.SetMatrix((PdfDictionary) appearance.m_content);
    PaintParams paintParams = new PaintParams();
    PdfGraphics graphics = appearance.Graphics;
    if ((double) this.m_borderWidth > 0.0 && this.Color.A != (byte) 0)
    {
      PdfPen pdfPen = new PdfPen(this.Color, this.m_borderWidth);
      paintParams.BorderPen = pdfPen;
    }
    PdfBrush pdfBrush = (PdfBrush) null;
    if (this.InnerColor.A != (byte) 0)
      pdfBrush = (PdfBrush) new PdfSolidBrush(this.InnerColor);
    float num = this.m_borderWidth / 2f;
    paintParams.BackBrush = pdfBrush;
    paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
    RectangleF rectangleF = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
    if ((double) this.Opacity < 1.0)
    {
      PdfGraphicsState state = graphics.Save();
      graphics.SetTransparency(this.Opacity);
      FieldPainter.DrawEllipseAnnotation(graphics, paintParams, rectangleF.X + num, rectangleF.Y + num, rectangleF.Width - this.m_borderWidth, rectangleF.Height - this.m_borderWidth);
      graphics.Restore(state);
    }
    else
      FieldPainter.DrawEllipseAnnotation(graphics, paintParams, rectangleF.X + num, rectangleF.Y + num, rectangleF.Width - this.m_borderWidth, rectangleF.Height - this.m_borderWidth);
    return appearance;
  }
}
