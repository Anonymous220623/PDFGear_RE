// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfTextMarkupAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfTextMarkupAnnotation : PdfAnnotation
{
  private PdfTextMarkupAnnotationType m_textMarkupAnnotationType;
  private int[] m_quadPoints = new int[8];
  private PdfArray m_points;
  private PdfColor m_textMarkupColor;
  private string m_text;
  internal SizeF m_textSize;
  private PointF m_textPoint;
  internal PdfFont m_font;
  private List<RectangleF> m_boundscollection = new List<RectangleF>();
  internal PdfDictionary m_borderDic = new PdfDictionary();
  private PdfLineBorderStyle m_borderStyle;

  public PdfTextMarkupAnnotationType TextMarkupAnnotationType
  {
    get => this.m_textMarkupAnnotationType;
    set => this.m_textMarkupAnnotationType = value;
  }

  public PdfColor TextMarkupColor
  {
    get => this.m_textMarkupColor;
    set => this.m_textMarkupColor = value;
  }

  internal PdfLineBorderStyle BorderStyle
  {
    get => this.m_borderStyle;
    set
    {
      this.m_borderStyle = value;
      if (this.m_borderStyle == PdfLineBorderStyle.Solid)
        this.m_borderDic.SetProperty("S", (IPdfPrimitive) new PdfName("S"));
      else if (this.m_borderStyle == PdfLineBorderStyle.Inset)
        this.m_borderDic.SetProperty("S", (IPdfPrimitive) new PdfName("I"));
      else if (this.m_borderStyle == PdfLineBorderStyle.Dashed)
        this.m_borderDic.SetProperty("S", (IPdfPrimitive) new PdfName("D"));
      else if (this.m_borderStyle == PdfLineBorderStyle.Beveled)
      {
        this.m_borderDic.SetProperty("S", (IPdfPrimitive) new PdfName("B"));
      }
      else
      {
        if (this.m_borderStyle != PdfLineBorderStyle.Underline)
          return;
        this.m_borderDic.SetProperty("S", (IPdfPrimitive) new PdfName("U"));
      }
    }
  }

  public List<RectangleF> BoundsCollection
  {
    get => this.m_boundscollection;
    set
    {
      if (this.m_boundscollection.Count > 0)
      {
        this.m_quadPoints = new int[8 + value.Count * 8];
        for (int index = 0; index < value.Count; ++index)
          this.m_boundscollection.Add(value[index]);
      }
      else
      {
        this.m_quadPoints = new int[8];
        this.m_boundscollection = value;
      }
    }
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

  public PdfTextMarkupAnnotation()
  {
    this.BoundsCollection.Add(new RectangleF(this.Location, this.m_textSize));
  }

  public PdfTextMarkupAnnotation(
    string markupTitle,
    string text,
    string markupText,
    PointF point,
    PdfFont pdfFont)
  {
    this.Text = text;
    this.m_text = markupTitle;
    this.m_font = pdfFont;
    this.Location = point;
    this.m_textSize = this.m_font.MeasureString(markupText);
    this.m_textPoint = point;
    this.m_textPoint.X += 25f;
    this.m_textPoint.Y = 800f - this.m_textPoint.Y;
    this.BoundsCollection.Add(new RectangleF(point, this.m_textSize));
  }

  public PdfTextMarkupAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
    this.BoundsCollection.Add(rectangle);
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName((Enum) this.m_textMarkupAnnotationType));
  }

  protected override void Save()
  {
    this.CheckFlatten();
    PdfArray primitive = new PdfArray();
    if (this.TextMarkupColor.IsEmpty)
      throw new Exception("TextMarkupColor is not null");
    float num1 = (float) this.TextMarkupColor.R / (float) byte.MaxValue;
    float num2 = (float) this.TextMarkupColor.G / (float) byte.MaxValue;
    float num3 = (float) this.TextMarkupColor.B / (float) byte.MaxValue;
    primitive.Insert(0, (IPdfPrimitive) new PdfNumber(num1));
    primitive.Insert(1, (IPdfPrimitive) new PdfNumber(num2));
    primitive.Insert(2, (IPdfPrimitive) new PdfNumber(num3));
    this.Dictionary.SetProperty("C", (IPdfPrimitive) primitive);
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
      this.SaveTextMarkUpDictionary();
    }
    if (!this.FlattenPopUps)
      return;
    this.FlattenPopup();
  }

  private void SaveTextMarkUpDictionary()
  {
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName((Enum) this.m_textMarkupAnnotationType));
    if (!this.isAuthorExplicitSet && this.m_text != null)
      this.Dictionary.SetString("T", this.m_text);
    this.m_borderDic.SetProperty("Type", (IPdfPrimitive) new PdfName("Border"));
    this.m_borderDic.SetNumber("W", this.Border.Width);
    this.Dictionary.SetProperty("BS", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_borderDic));
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

  private PdfPath DrawSquiggly(float width, float height)
  {
    if ((int) width % 2 != 0)
      width = (float) ((int) width + 1);
    PdfPath pdfPath = new PdfPath();
    PointF[] linePoints = new PointF[(int) Convert.ToInt16(width / 2f)];
    int num1 = (int) Math.Round((double) height / 2.0 / 3.5);
    int num2 = num1;
    for (int index = 0; (double) index < (double) width; index += 2)
    {
      linePoints[index / 2] = new PointF((float) (index * (num1 / 2)), height - (float) num1 + (float) num2);
      num2 = num2 != 0 ? 0 : num1;
    }
    pdfPath.AddLines(linePoints);
    return pdfPath;
  }

  private PdfTemplate CreateAppearance()
  {
    double num1 = 0.0;
    double height = 0.0;
    RectangleF rectangleF = RectangleF.Empty;
    if (this.BoundsCollection.Count > 1)
    {
      PdfPath pdfPath = new PdfPath();
      for (int index = 0; index < this.BoundsCollection.Count; ++index)
        pdfPath.AddRectangle(this.BoundsCollection[index]);
      rectangleF = pdfPath.GetBounds();
      this.Bounds = rectangleF;
      num1 = (double) rectangleF.Width;
      height = (double) rectangleF.Height;
    }
    else if (this.Dictionary.ContainsKey("QuadPoints"))
    {
      PdfArray pdfArray = this.Dictionary["QuadPoints"] as PdfArray;
      if (this.m_quadPoints != null)
      {
        for (int index = 0; index < pdfArray.Count / 8; ++index)
        {
          float num2 = (float) ((pdfArray[4 + index * 8] as PdfNumber).IntValue - (pdfArray[index * 8] as PdfNumber).IntValue);
          float num3 = (float) ((pdfArray[5 + index * 8] as PdfNumber).IntValue - (pdfArray[1 + index * 8] as PdfNumber).IntValue);
          height = Math.Sqrt((double) num2 * (double) num2 + (double) num3 * (double) num3);
          float num4 = (float) ((pdfArray[6 + index * 8] as PdfNumber).IntValue - (pdfArray[4 + index * 8] as PdfNumber).IntValue);
          float num5 = (float) ((pdfArray[7 + index * 8] as PdfNumber).IntValue - (pdfArray[5 + index * 8] as PdfNumber).IntValue);
          num1 = Math.Sqrt((double) num4 * (double) num4 + (double) num5 * (double) num5);
          this.Bounds = new RectangleF(this.Bounds.X, this.Bounds.Y, (float) num1, (float) height);
        }
      }
    }
    PdfTemplate appearance = new PdfTemplate(new RectangleF(0.0f, 0.0f, (float) num1, (float) height));
    this.SetMatrix((PdfDictionary) appearance.m_content);
    PdfGraphics graphics = appearance.Graphics;
    graphics.SetTransparency(this.Opacity, this.Opacity, PdfBlendMode.Multiply);
    if (this.BoundsCollection.Count > 1)
    {
      for (int index = 0; index < this.BoundsCollection.Count; ++index)
      {
        if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Highlight)
          graphics.DrawRectangle((PdfBrush) new PdfSolidBrush(this.TextMarkupColor), this.BoundsCollection[index].X - rectangleF.X, this.BoundsCollection[index].Y - rectangleF.Y, this.BoundsCollection[index].Width, this.BoundsCollection[index].Height);
        else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Underline)
          graphics.DrawLine(new PdfPen(this.TextMarkupColor), this.BoundsCollection[index].X - rectangleF.X, (float) ((double) this.BoundsCollection[index].Y - (double) rectangleF.Y + ((double) this.BoundsCollection[index].Height - (double) this.BoundsCollection[index].Height / 2.0 / 3.0)), this.BoundsCollection[index].Width + (this.BoundsCollection[index].X - rectangleF.X), (float) ((double) this.BoundsCollection[index].Y - (double) rectangleF.Y + ((double) this.BoundsCollection[index].Height - (double) this.BoundsCollection[index].Height / 2.0 / 3.0)));
        else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.StrikeOut)
          graphics.DrawLine(new PdfPen(this.TextMarkupColor), this.BoundsCollection[index].X - rectangleF.X, (float) ((double) this.BoundsCollection[index].Y - (double) rectangleF.Y + ((double) this.BoundsCollection[index].Height - (double) this.BoundsCollection[index].Height / 2.0)), this.BoundsCollection[index].Width + (this.BoundsCollection[index].X - rectangleF.X), (float) ((double) this.BoundsCollection[index].Y - (double) rectangleF.Y + ((double) this.BoundsCollection[index].Height - (double) this.BoundsCollection[index].Height / 2.0)));
        else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Squiggly)
        {
          PdfPen pen = new PdfPen(this.TextMarkupColor);
          pen.Width = this.Border.Width;
          graphics.Save();
          graphics.TranslateTransform(this.BoundsCollection[index].X - rectangleF.X, this.BoundsCollection[index].Y - rectangleF.Y);
          graphics.SetClip(new RectangleF(0.0f, 0.0f, this.BoundsCollection[index].Width, this.BoundsCollection[index].Height));
          graphics.DrawPath(pen, this.DrawSquiggly(this.BoundsCollection[index].Width, this.BoundsCollection[index].Height));
          graphics.Restore();
        }
      }
    }
    else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Highlight)
      graphics.DrawRectangle((PdfBrush) new PdfSolidBrush(this.TextMarkupColor), 0.0f, 0.0f, (float) num1, (float) height);
    else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Underline)
      graphics.DrawLine(new PdfPen(this.TextMarkupColor), 0.0f, (float) height - (float) (height / 2.0 / 3.0), (float) num1, (float) height - (float) (height / 2.0 / 3.0));
    else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.StrikeOut)
      graphics.DrawLine(new PdfPen(this.TextMarkupColor), 0.0f, (float) height / 2f, (float) num1, (float) height / 2f);
    else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Squiggly)
      graphics.DrawPath(new PdfPen(this.TextMarkupColor)
      {
        Width = this.Border.Width
      }, this.DrawSquiggly((float) num1, (float) height));
    return appearance;
  }

  internal void SetQuadPoints(SizeF pageSize)
  {
    float[] array = new float[this.m_quadPoints.Length];
    float height = pageSize.Height;
    PdfMargins margin = this.ObtainMargin();
    if (this.m_textSize == new SizeF(0.0f, 0.0f))
      this.m_textSize = this.Size;
    this.m_boundscollection[0] = new RectangleF(this.Location, this.m_textSize);
    int num = this.m_quadPoints.Length / 8;
    for (int index = 0; index < num; ++index)
    {
      float x = this.m_boundscollection[index].X;
      float y = this.m_boundscollection[index].Y;
      array[index * 8] = x + margin.Left;
      array[1 + index * 8] = height - y - margin.Top;
      array[2 + index * 8] = x + this.m_boundscollection[index].Width + margin.Left;
      array[3 + index * 8] = height - y - margin.Top;
      array[4 + index * 8] = x + margin.Left;
      array[5 + index * 8] = array[1 + index * 8] - this.m_boundscollection[index].Height;
      array[6 + index * 8] = x + this.m_boundscollection[index].Width + margin.Left;
      array[7 + index * 8] = array[5 + index * 8];
    }
    this.m_points = new PdfArray(array);
    this.Dictionary.SetProperty("QuadPoints", (IPdfPrimitive) this.m_points);
  }
}
