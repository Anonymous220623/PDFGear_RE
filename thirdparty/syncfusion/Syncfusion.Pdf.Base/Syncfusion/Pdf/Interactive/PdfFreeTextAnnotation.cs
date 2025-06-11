// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfFreeTextAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfFreeTextAnnotation : PdfAnnotation
{
  private const string c_annotationType = "FreeText";
  private PdfLineEndingStyle m_lineEndingStyle;
  private PdfAnnotationIntent m_annotationIntent;
  private string m_markUpText;
  private PdfFont m_font;
  private PointF[] m_calloutLines = new PointF[0];
  private PdfColor m_textMarkupColor;
  private WidgetAnnotation m_widgetAnnotation = new WidgetAnnotation();
  private PdfColor m_borderColor;
  private PdfMargins m_margins = new PdfMargins();
  private bool m_complexScript;
  private PdfTextAlignment alignment;
  private PdfTextDirection m_textDirection;
  private float m_lineSpacing;
  private bool isAllRotation = true;

  public float LineSpacing
  {
    get => this.m_lineSpacing;
    set => this.m_lineSpacing = value;
  }

  public PdfLineEndingStyle LineEndingStyle
  {
    get => this.m_lineEndingStyle;
    set => this.m_lineEndingStyle = value;
  }

  public PdfAnnotationIntent AnnotationIntent
  {
    get => this.m_annotationIntent;
    set => this.m_annotationIntent = value;
  }

  public string MarkupText
  {
    get => this.m_markUpText;
    set => this.m_markUpText = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value != null ? value : throw new ArgumentNullException(nameof (Font));
  }

  public PointF[] CalloutLines
  {
    get => this.m_calloutLines;
    set => this.m_calloutLines = value;
  }

  public PdfColor TextMarkupColor
  {
    get => this.m_textMarkupColor;
    set => this.m_textMarkupColor = value;
  }

  public PdfColor BorderColor
  {
    get => this.m_borderColor;
    set => this.m_borderColor = value;
  }

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

  public bool ComplexScript
  {
    get => this.m_complexScript;
    set => this.m_complexScript = value;
  }

  public PdfTextAlignment TextAlignment
  {
    get => this.alignment;
    set
    {
      if (this.alignment == value)
        return;
      this.alignment = value;
      this.Dictionary.SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) this.alignment));
    }
  }

  public PdfTextDirection TextDirection
  {
    get => this.m_textDirection;
    set => this.m_textDirection = value;
  }

  private PdfFreeTextAnnotation()
  {
  }

  public PdfFreeTextAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
    base.Initialize();
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("FreeText"));
  }

  protected override void Save()
  {
    this.CheckFlatten();
    if (this.Flatten || this.SetAppearanceDictionary)
    {
      PdfTemplate appearance = this.CreateAppearance();
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
      this.SaveFreeTextDictionary();
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

  private void SaveFreeTextDictionary()
  {
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("FreeText"));
    if (this.Font == null)
      this.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 7f);
    PdfArray primitive1 = new PdfArray();
    if (!this.Color.IsEmpty)
    {
      float num1 = (float) this.Color.R / (float) byte.MaxValue;
      float num2 = (float) this.Color.G / (float) byte.MaxValue;
      float num3 = (float) this.Color.B / (float) byte.MaxValue;
      primitive1.Insert(0, (IPdfPrimitive) new PdfNumber(num1));
      primitive1.Insert(1, (IPdfPrimitive) new PdfNumber(num2));
      primitive1.Insert(2, (IPdfPrimitive) new PdfNumber(num3));
    }
    this.Dictionary.SetProperty("C", (IPdfPrimitive) primitive1);
    if (this.MarkupText == null)
      this.MarkupText = this.Text;
    this.Dictionary.SetProperty("Contents", (IPdfPrimitive) new PdfString(this.m_markUpText));
    this.Dictionary.SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) this.alignment));
    this.Dictionary.SetProperty("IT", (IPdfPrimitive) new PdfName(this.m_annotationIntent.ToString()));
    this.Dictionary.SetProperty("LE", (IPdfPrimitive) new PdfName(this.m_lineEndingStyle.ToString()));
    this.Dictionary.SetProperty("DS", (IPdfPrimitive) new PdfString($"font:{this.Font.Name} {this.Font.Size}pt;style:{this.Font.Style};color:{ColorTranslator.ToHtml(System.Drawing.Color.FromArgb((int) this.m_textMarkupColor.R, (int) this.m_textMarkupColor.G, (int) this.m_textMarkupColor.B))}"));
    this.Dictionary.SetProperty("DA", (IPdfPrimitive) new PdfString($"{(ValueType) (float) ((double) this.m_borderColor.R / (double) byte.MaxValue)} {(ValueType) (float) ((double) this.m_borderColor.G / (double) byte.MaxValue)} {(ValueType) (float) ((double) this.m_borderColor.B / (double) byte.MaxValue)} rg "));
    this.Dictionary.SetString("RC", $"<?xml version=\"1.0\"?><body xmlns=\"http://www.w3.org/1999/xhtml\"><p dir=\"ltr\">{this.MarkupText}</p></body>");
    if (this.m_calloutLines.Length < 2)
      return;
    this.m_margins = this.ObtainMargin();
    float num = this.Page != null ? this.Page.Size.Height : this.LoadedPage.Size.Height;
    PdfArray primitive2 = new PdfArray();
    for (int index = 0; index < this.m_calloutLines.Length && index < 3; ++index)
    {
      primitive2.Add((IPdfPrimitive) new PdfNumber(this.m_calloutLines[index].X + this.m_margins.Left));
      primitive2.Add((IPdfPrimitive) new PdfNumber(num - (this.m_calloutLines[index].Y + this.m_margins.Top)));
    }
    this.Dictionary.SetProperty("CL", (IPdfPrimitive) primitive2);
    if (!this.SetAppearanceDictionary)
      return;
    this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(this.ObtainAppearanceBounds()));
  }

  private RectangleF ObtainAppearanceBounds()
  {
    RectangleF appearanceBounds = RectangleF.Empty;
    this.m_margins = this.ObtainMargin();
    if (this.CalloutLines != null && this.CalloutLines.Length > 0)
    {
      PdfPath pdfPath = new PdfPath();
      float num = this.Page != null ? this.Page.Size.Height : this.LoadedPage.Size.Height;
      PointF[] linePoints = this.CalloutLines.Length != 2 ? new PointF[3] : new PointF[this.CalloutLines.Length];
      if (this.CalloutLines.Length >= 2)
      {
        for (int index = 0; index < this.CalloutLines.Length && index < 3; ++index)
          linePoints[index] = new PointF(this.CalloutLines[index].X + this.m_margins.Left, num - (this.CalloutLines[index].Y + this.m_margins.Top));
      }
      if (linePoints.Length > 0)
        pdfPath.AddLines(linePoints);
      pdfPath.GetBounds();
      if (this.Page != null)
        pdfPath.AddRectangle(new RectangleF((float) ((double) this.Bounds.X + (double) this.m_margins.Left - 2.0), (float) ((double) num - ((double) this.Bounds.Y + (double) this.Bounds.Height + (double) this.m_margins.Top) - 2.0), this.Bounds.Width + 4f, this.Bounds.Height + 4f));
      else if (this.LoadedPage != null)
        pdfPath.AddRectangle(new RectangleF(this.Bounds.X - 2f, (float) ((double) num - ((double) this.Bounds.Y + (double) this.Bounds.Height) - 2.0), this.Bounds.Width + 4f, this.Bounds.Height + 4f));
      appearanceBounds = pdfPath.GetBounds();
    }
    else if (this.Page != null)
      appearanceBounds = new RectangleF(this.Bounds.X + this.m_margins.Left, this.Page.Size.Height - (this.Bounds.Y + this.Bounds.Height + this.m_margins.Top), this.Bounds.Width, this.Bounds.Height);
    else if (this.LoadedPage != null)
      appearanceBounds = new RectangleF(this.Bounds.X, this.LoadedPage.Size.Height - (this.Bounds.Y + this.Bounds.Height), this.Bounds.Width, this.Bounds.Height);
    return appearanceBounds;
  }

  private PdfTemplate CreateAppearance()
  {
    RectangleF appearanceBounds = this.ObtainAppearanceBounds();
    if ((double) this.RotateAngle == 90.0 || (double) this.RotateAngle == 180.0 || (double) this.RotateAngle == 270.0 || (double) this.RotateAngle == 0.0)
      this.isAllRotation = false;
    PdfTemplate appearance = (double) this.RotateAngle <= 0.0 || !this.isAllRotation ? new PdfTemplate(appearanceBounds) : new PdfTemplate(appearanceBounds.Size);
    this.SetMatrix((PdfDictionary) appearance.m_content);
    PaintParams paintParams = new PaintParams();
    appearance.m_writeTransformation = false;
    PdfGraphics graphics = appearance.Graphics;
    if ((double) this.Border.Width > 0.0 && this.BorderColor.A != (byte) 0)
    {
      PdfPen pdfPen = new PdfPen(this.BorderColor, this.Border.Width);
      paintParams.BorderPen = pdfPen;
    }
    paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
    paintParams.BackBrush = (PdfBrush) new PdfSolidBrush(this.TextMarkupColor);
    paintParams.m_complexScript = this.ComplexScript;
    paintParams.m_textDirection = this.TextDirection;
    paintParams.m_lineSpacing = this.LineSpacing;
    RectangleF rectangleF = new RectangleF(appearanceBounds.X, -appearanceBounds.Y, appearanceBounds.Width, -appearanceBounds.Height);
    if (this.MarkupText == null)
      this.MarkupText = this.Text;
    paintParams.Bounds = rectangleF;
    float num = this.Page != null ? this.Page.Size.Height : this.LoadedPage.Size.Height;
    if (this.CalloutLines.Length >= 2)
    {
      PdfPen mBorderPen = (PdfPen) null;
      if (this.BorderColor.A != (byte) 0)
        mBorderPen = new PdfPen(this.BorderColor, this.Border.Width);
      this.DrawCallOuts(graphics, mBorderPen);
      if (this.LineEndingStyle == PdfLineEndingStyle.OpenArrow)
        this.DrawArrow(paintParams, graphics, mBorderPen);
      if (this.Page != null)
        rectangleF = new RectangleF(this.Bounds.X + this.m_margins.Left, (float) -((double) this.Page.Size.Height - ((double) this.Bounds.Y + (double) this.Bounds.Height + (double) this.m_margins.Top)), this.Bounds.Width, -this.Bounds.Height);
      else if (this.LoadedPage != null)
        rectangleF = new RectangleF(this.Bounds.X, (float) -((double) this.LoadedPage.Size.Height - ((double) this.Bounds.Y + (double) this.Bounds.Height)), this.Bounds.Width, -this.Bounds.Height);
      this.SetRectangleDifferance(rectangleF);
      paintParams.Bounds = rectangleF;
    }
    else
      this.SetRectangleDifferance(rectangleF);
    if ((double) this.Opacity < 1.0)
    {
      graphics.Save();
      graphics.SetTransparency(this.Opacity);
    }
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0)
      graphics.Save();
    this.DrawFreeTextRectangle(graphics, paintParams, rectangleF);
    this.DrawFreeMarkUpText(graphics, paintParams, rectangleF);
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0)
      graphics.Restore();
    if ((double) this.Opacity < 1.0)
      graphics.Restore();
    if (this.Flatten)
    {
      if (this.Page != null)
        this.Bounds = new RectangleF(appearanceBounds.X - this.m_margins.Left, num - (appearanceBounds.Y + appearanceBounds.Height) - this.m_margins.Top, appearanceBounds.Width, appearanceBounds.Height);
      else
        this.Bounds = new RectangleF(appearanceBounds.X, num - (appearanceBounds.Y + appearanceBounds.Height), appearanceBounds.Width, appearanceBounds.Height);
    }
    return appearance;
  }

  private void DrawArrow(PaintParams paintParams, PdfGraphics graphics, PdfPen mBorderPen)
  {
    float num = this.Page != null ? this.Page.Size.Height : this.LoadedPage.Size.Height;
    PointF[] arrowPoints = this.CalculateArrowPoints(new PointF(this.CalloutLines[1].X + this.m_margins.Left, num - (this.CalloutLines[1].Y + this.m_margins.Top)), new PointF(this.CalloutLines[0].X + this.m_margins.Left, num - (this.CalloutLines[0].Y + this.m_margins.Top)));
    PointF[] points = new PointF[3];
    byte[] pathTypes = new byte[3];
    points[0] = new PointF(arrowPoints[0].X, -arrowPoints[0].Y);
    points[1] = new PointF(this.CalloutLines[0].X + this.m_margins.Left, (float) -((double) num - ((double) this.CalloutLines[0].Y + (double) this.m_margins.Top)));
    points[2] = new PointF(arrowPoints[1].X, -arrowPoints[1].Y);
    pathTypes[0] = (byte) 0;
    pathTypes[1] = (byte) 1;
    pathTypes[2] = (byte) 1;
    PdfPath path = new PdfPath(points, pathTypes);
    if (paintParams.BorderPen == null)
      return;
    graphics.DrawPath(mBorderPen, path);
  }

  private void DrawCallOuts(PdfGraphics graphics, PdfPen mBorderPen)
  {
    PdfPath path = new PdfPath();
    float num = this.Page != null ? this.Page.Size.Height : this.LoadedPage.Size.Height;
    PointF[] linePoints = this.CalloutLines.Length != 2 ? new PointF[3] : new PointF[this.CalloutLines.Length];
    for (int index = 0; index < this.CalloutLines.Length && index < 3; ++index)
      linePoints[index] = new PointF(this.CalloutLines[index].X + this.m_margins.Left, (float) -((double) num - ((double) this.CalloutLines[index].Y + (double) this.m_margins.Top)));
    if (linePoints.Length > 0)
      path.AddLines(linePoints);
    graphics.DrawPath(mBorderPen, path);
  }

  private PointF[] CalculateArrowPoints(PointF startingPoint, PointF endPoint)
  {
    float num1 = endPoint.X - startingPoint.X;
    float num2 = endPoint.Y - startingPoint.Y;
    float num3 = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    float num4 = num1 / num3;
    float num5 = num2 / num3;
    float num6 = (float) (4.0 * (-(double) num5 - (double) num4));
    float num7 = (float) (4.0 * ((double) num4 - (double) num5));
    return new PointF[2]
    {
      new PointF(endPoint.X + num6, endPoint.Y + num7),
      new PointF(endPoint.X - num7, endPoint.Y + num6)
    };
  }

  private void DrawFreeTextRectangle(
    PdfGraphics graphics,
    PaintParams paintParams,
    RectangleF rect)
  {
    bool isRotation = false;
    if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90 && !this.isAllRotation)
    {
      graphics.RotateTransform(-90f);
      paintParams.Bounds = new RectangleF(-rect.Y, rect.Width + rect.X, -rect.Height, -rect.Width);
    }
    else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle180 && !this.isAllRotation)
    {
      graphics.RotateTransform(-180f);
      paintParams.Bounds = new RectangleF((float) -((double) rect.Width + (double) rect.X), (float) -((double) rect.Height + (double) rect.Y), rect.Width, rect.Height);
    }
    else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle270 && !this.isAllRotation)
    {
      graphics.RotateTransform(-270f);
      paintParams.Bounds = new RectangleF(rect.Y + rect.Height, -rect.X, -rect.Height, -rect.Width);
    }
    FieldPainter.DrawFreeTextAnnotation(graphics, paintParams, "", this.Font, rect, false, this.alignment, isRotation);
  }

  private void DrawFreeMarkUpText(PdfGraphics graphics, PaintParams paintParams, RectangleF rect)
  {
    bool isRotation = false;
    if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90 && !this.isAllRotation)
      rect = new RectangleF(-rect.Y, rect.X, -rect.Height, rect.Width);
    else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle180 && !this.isAllRotation)
      rect = new RectangleF((float) -((double) rect.Width + (double) rect.X), -rect.Y, rect.Width, -rect.Height);
    else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle270 && !this.isAllRotation)
      rect = new RectangleF(rect.Y + rect.Height, (float) -((double) rect.Width + (double) rect.X), -rect.Height, rect.Width);
    else if ((double) this.RotateAngle == 0.0 && !this.isAllRotation)
      rect = new RectangleF(rect.X, rect.Y + rect.Height, rect.Width, rect.Height);
    float rotateAngle = this.RotateAngle;
    if ((double) rotateAngle > 0.0 && this.isAllRotation)
    {
      isRotation = true;
      SizeF sizeF = this.Font.MeasureString(this.Text);
      if ((double) rotateAngle > 0.0 && (double) rotateAngle <= 91.0)
        graphics.TranslateTransform(sizeF.Height, -this.Bounds.Height);
      else if ((double) rotateAngle > 91.0 && (double) rotateAngle <= 181.0)
        graphics.TranslateTransform(this.Bounds.Width - sizeF.Height, (float) -((double) this.Bounds.Height - (double) sizeF.Height));
      else if ((double) rotateAngle > 181.0 && (double) rotateAngle <= 271.0)
        graphics.TranslateTransform(this.Bounds.Width - sizeF.Height, -sizeF.Height);
      else if ((double) rotateAngle > 271.0 && (double) rotateAngle < 360.0)
        graphics.TranslateTransform(sizeF.Height, -sizeF.Height);
      graphics.RotateTransform(this.RotateAngle);
      paintParams.Bounds = new RectangleF(0.0f, 0.0f, paintParams.Bounds.Width, paintParams.Bounds.Height);
    }
    FieldPainter.DrawFreeTextAnnotation(graphics, paintParams, this.MarkupText, this.Font, rect, true, this.alignment, isRotation);
  }

  private void SetRectangleDifferance(RectangleF innerRectangle)
  {
    RectangleF appearanceBounds = this.ObtainAppearanceBounds();
    float[] array = new float[4]
    {
      innerRectangle.X - appearanceBounds.X,
      -innerRectangle.Y - appearanceBounds.Y,
      innerRectangle.Width - appearanceBounds.Width,
      (float) (-(double) innerRectangle.Y - (double) appearanceBounds.Y + -(double) innerRectangle.Height) - appearanceBounds.Height
    };
    for (int index = 0; index < 4; ++index)
    {
      if ((double) array[index] < 0.0)
        array[index] = -array[index];
    }
    this.Dictionary["RD"] = (IPdfPrimitive) new PdfArray(array);
  }
}
