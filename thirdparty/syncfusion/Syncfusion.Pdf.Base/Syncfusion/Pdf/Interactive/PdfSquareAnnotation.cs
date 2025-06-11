// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfSquareAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfSquareAnnotation : PdfAnnotation
{
  private LineBorder m_border = new LineBorder();
  private PdfBorderEffect m_borderEffect = new PdfBorderEffect();
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

  public PdfBorderEffect BorderEffect
  {
    get => this.m_borderEffect;
    set => this.m_borderEffect = value;
  }

  public LineBorder Border
  {
    get => this.m_border;
    set => this.m_border = value;
  }

  public PdfSquareAnnotation(RectangleF rectangle, string text)
    : base(rectangle)
  {
    this.Text = text;
  }

  public PdfSquareAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Square"));
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
      if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
      {
        this.Dictionary.SetProperty("BE", (IPdfWrapper) this.m_borderEffect);
        this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
        if (this.Dictionary["BS"] is PdfDictionary pdfDictionary)
        {
          if (pdfDictionary.ContainsKey("S"))
            pdfDictionary.Remove("S");
          if (pdfDictionary.ContainsKey("D"))
            pdfDictionary.Remove("D");
        }
      }
      else
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
    RectangleF rectangleF = RectangleF.Empty;
    if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
      this.Bounds = new RectangleF((float) ((double) this.Bounds.X - (double) this.BorderEffect.Intensity * 5.0 - (double) this.m_borderWidth / 2.0), (float) ((double) this.Bounds.Y - (double) this.BorderEffect.Intensity * 5.0 - (double) this.m_borderWidth / 2.0), this.Bounds.Width + this.BorderEffect.Intensity * 10f + this.m_borderWidth, this.Bounds.Height + this.BorderEffect.Intensity * 10f + this.m_borderWidth);
    rectangleF = new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height);
    PdfTemplate appearance = new PdfTemplate(rectangleF);
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
    RectangleF style = this.ObtainStyle(rectangleF);
    if ((double) this.Opacity < 1.0)
    {
      graphics.Save();
      graphics.SetTransparency(this.Opacity);
    }
    if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
      FieldPainter.DrawRectanglecloud(graphics, paintParams, style, this.BorderEffect.Intensity, this.m_borderWidth);
    else
      FieldPainter.DrawRectangleAnnotation(graphics, paintParams, style.X + num, style.Y + num, style.Width - this.m_borderWidth, style.Height - this.m_borderWidth);
    if ((double) this.Opacity < 1.0)
      graphics.Restore();
    return appearance;
  }

  private RectangleF ObtainStyle(RectangleF rectangle)
  {
    if (!this.Flatten)
    {
      if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
      {
        float num = 0.0f;
        if ((double) this.BorderEffect.Intensity > 0.0 && (double) this.BorderEffect.Intensity <= 2.0)
          num = this.BorderEffect.Intensity * 5f;
        float[] array = new float[4]
        {
          2f * num + this.m_borderWidth,
          2f * num + this.m_borderWidth,
          2f * num + this.m_borderWidth,
          2f * num + this.m_borderWidth
        };
        this.Dictionary.SetProperty("RD", (IPdfPrimitive) new PdfArray(array));
        rectangle.X = (float) ((double) rectangle.X + (double) array[0] + (double) this.m_borderWidth / 2.0);
        rectangle.Y = (float) ((double) rectangle.Y + (double) array[1] + (double) this.m_borderWidth / 2.0);
        rectangle.Width = rectangle.Width - 2f * array[2] - this.m_borderWidth;
        rectangle.Height = rectangle.Height - 2f * array[3] - this.m_borderWidth;
      }
    }
    else if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
    {
      float num = 0.0f;
      if ((double) this.BorderEffect.Intensity > 0.0 && (double) this.BorderEffect.Intensity <= 2.0)
        num = this.BorderEffect.Intensity * 5f;
      float[] array = new float[4]
      {
        num + this.m_borderWidth / 2f,
        num + this.m_borderWidth / 2f,
        num + this.m_borderWidth / 2f,
        num + this.m_borderWidth / 2f
      };
      this.Dictionary.SetProperty("RD", (IPdfPrimitive) new PdfArray(array));
      rectangle.X = (float) ((double) rectangle.X + (double) array[0] + (double) this.m_borderWidth / 2.0);
      rectangle.Y = (float) ((double) rectangle.Y + (double) array[1] + (double) this.m_borderWidth / 2.0);
      rectangle.Width = rectangle.Width - 2f * array[2] - this.m_borderWidth;
      rectangle.Height = rectangle.Height - 2f * array[3] - this.m_borderWidth;
    }
    rectangle = new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    return rectangle;
  }
}
