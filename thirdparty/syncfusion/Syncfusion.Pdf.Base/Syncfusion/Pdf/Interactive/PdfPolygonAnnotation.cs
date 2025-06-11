// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfPolygonAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfPolygonAnnotation : PdfAnnotation
{
  private LineBorder m_border = new LineBorder();
  internal PdfArray m_linePoints;
  private int m_lineExtension;
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

  public int LineExtension
  {
    get => this.m_lineExtension;
    set => this.m_lineExtension = value;
  }

  public PdfPolygonAnnotation(int[] points, string text)
  {
    this.m_linePoints = new PdfArray(points);
    this.Text = text;
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Polygon"));
  }

  private PointF[] GetLinePoints()
  {
    PointF[] linePoints = (PointF[]) null;
    if (this.m_linePoints != null)
    {
      int[] numArray = new int[this.m_linePoints.Count];
      int index1 = 0;
      foreach (PdfNumber linePoint in this.m_linePoints)
      {
        numArray[index1] = linePoint.IntValue;
        ++index1;
      }
      linePoints = new PointF[numArray.Length / 2];
      int index2 = 0;
      for (int index3 = 0; index3 < numArray.Length; index3 += 2)
      {
        float num = this.Page != null ? this.Page.Size.Height : this.LoadedPage.Size.Height;
        linePoints[index2] = !this.Flatten ? new PointF((float) numArray[index3], (float) -numArray[index3 + 1]) : (this.Page == null ? new PointF((float) numArray[index3], num - (float) numArray[index3 + 1]) : new PointF((float) numArray[index3] - this.Page.m_section.PageSettings.Margins.Left, num - (float) numArray[index3 + 1] - this.Page.m_section.PageSettings.Margins.Right));
        ++index2;
      }
    }
    return linePoints;
  }

  private void GetBoundsValue()
  {
    int count = this.m_linePoints.Count;
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    if (this.m_linePoints.Count > 0)
    {
      int[] numArray = new int[this.m_linePoints.Count];
      int index1 = 0;
      foreach (PdfNumber linePoint in this.m_linePoints)
      {
        numArray[index1] = linePoint.IntValue;
        ++index1;
      }
      for (int index2 = 0; index2 < numArray.Length; ++index2)
      {
        if (index2 % 2 == 0)
          intList1.Add(numArray[index2]);
        else
          intList2.Add(numArray[index2]);
      }
    }
    intList1.Sort();
    intList2.Sort();
    if (this.Flatten)
      return;
    this.Bounds = new RectangleF((float) intList1[0], (float) intList2[0], (float) (intList1[intList1.Count - 1] - intList1[0]), (float) (intList2[intList2.Count - 1] - intList2[0]));
  }

  protected override void Save()
  {
    this.CheckFlatten();
    this.m_borderWidth = this.Border.BorderWidth != 1 ? (float) this.Border.BorderWidth : this.Border.BorderLineWidth;
    PdfPageBase page = this.Page == null ? (PdfPageBase) this.LoadedPage : (PdfPageBase) this.Page;
    RectangleF rectangleF = RectangleF.Empty;
    PdfGraphics layerGraphics = this.GetLayerGraphics();
    List<float> floatList = new List<float>();
    foreach (PdfNumber linePoint in this.m_linePoints)
      floatList.Add(linePoint.FloatValue);
    floatList.ToArray();
    if ((double) floatList[0] != (double) floatList[floatList.Count - 2] || (double) floatList[1] != (double) floatList[floatList.Count - 1])
    {
      this.m_linePoints.Add(this.m_linePoints[0]);
      this.m_linePoints.Add(this.m_linePoints[1]);
    }
    if (!this.m_isStandardAppearance && !this.SetAppearanceDictionary)
    {
      this.Dictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
      if (this.Flatten)
      {
        PdfTemplate normal = this.Appearance.Normal;
        if (normal != null)
          this.FlattenAnnotation(page, normal);
      }
    }
    if (this.SetAppearanceDictionary)
    {
      this.GetBoundsValue();
      rectangleF = (double) this.BorderEffect.Intensity == 0.0 || this.BorderEffect.Style != PdfBorderEffectStyle.Cloudy ? new RectangleF(this.Bounds.X - this.m_borderWidth, this.Bounds.Y - this.m_borderWidth, this.Bounds.Width + 2f * this.m_borderWidth, this.Bounds.Height + 2f * this.m_borderWidth) : new RectangleF(this.Bounds.X - this.BorderEffect.Intensity * 5f - this.m_borderWidth, this.Bounds.Y - this.BorderEffect.Intensity * 5f - this.m_borderWidth, (float) ((double) this.Bounds.Width + (double) this.BorderEffect.Intensity * 10.0 + 2.0 * (double) this.m_borderWidth), (float) ((double) this.Bounds.Height + (double) this.BorderEffect.Intensity * 10.0 + 2.0 * (double) this.m_borderWidth));
      this.Dictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
      if (this.Dictionary["AP"] != null)
      {
        this.Appearance.Normal = new PdfTemplate(rectangleF);
        PdfTemplate normal = this.Appearance.Normal;
        PaintParams paintParams = new PaintParams();
        normal.m_writeTransformation = false;
        PdfGraphics graphics = this.Appearance.Normal.Graphics;
        PdfBrush brush = this.InnerColor.IsEmpty ? (PdfBrush) null : (PdfBrush) new PdfSolidBrush(this.InnerColor);
        PdfPen pen = (PdfPen) null;
        if ((double) this.m_borderWidth > 0.0 && this.Color.A != (byte) 0)
          pen = new PdfPen(this.Color, this.m_borderWidth);
        paintParams.BackBrush = brush;
        paintParams.BorderPen = pen;
        if (this.Flatten)
        {
          if ((double) this.Opacity < 1.0)
            page.Graphics.SetTransparency(this.Opacity);
          this.RemoveAnnoationFromPage(page, (PdfAnnotation) this);
          if (layerGraphics != null)
            layerGraphics.DrawPolygon(pen, brush, this.GetLinePoints());
          else if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
            FieldPainter.DrawPolygonCloud(page.Graphics, paintParams.BorderPen, paintParams.BackBrush, this.BorderEffect.Intensity, this.GetLinePoints(), this.m_borderWidth);
          else
            page.Graphics.DrawPolygon(pen, brush, this.GetLinePoints());
          if ((double) this.Opacity < 1.0)
            page.Graphics.Restore();
        }
        else
        {
          if ((double) this.Opacity < 1.0)
          {
            graphics.Save();
            graphics.SetTransparency(this.Opacity);
          }
          if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
            FieldPainter.DrawPolygonCloud(graphics, paintParams.BorderPen, paintParams.BackBrush, this.BorderEffect.Intensity, this.GetLinePoints(), this.m_borderWidth);
          else
            graphics.DrawPolygon(pen, brush, this.GetLinePoints());
          if ((double) this.Opacity < 1.0)
            graphics.Restore();
        }
      }
    }
    if (this.Flatten && !this.SetAppearanceDictionary && this.m_isStandardAppearance)
    {
      this.RemoveAnnoationFromPage(page, (PdfAnnotation) this);
      PdfPen pdfPen = (PdfPen) null;
      if ((double) this.m_borderWidth > 0.0 && this.Color.A != (byte) 0)
        pdfPen = new PdfPen(this.Color, this.m_borderWidth);
      PdfBrush pdfBrush = this.InnerColor.IsEmpty ? (PdfBrush) null : (PdfBrush) new PdfSolidBrush(this.InnerColor);
      if ((double) this.Opacity < 1.0)
        page.Graphics.SetTransparency(this.Opacity);
      if (layerGraphics != null)
        layerGraphics.DrawPolygon(pdfPen, pdfBrush, this.GetLinePoints());
      else if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
        FieldPainter.DrawPolygonCloud(page.Graphics, pdfPen, pdfBrush, this.BorderEffect.Intensity, this.GetLinePoints(), this.m_borderWidth);
      else
        page.Graphics.DrawPolygon(pdfPen, pdfBrush, this.GetLinePoints());
      if ((double) this.Opacity >= 1.0)
        return;
      page.Graphics.Restore();
    }
    else
    {
      if (this.Flatten)
        return;
      base.Save();
      this.Dictionary.SetProperty("Vertices", (IPdfPrimitive) new PdfArray(this.m_linePoints));
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
      this.GetBoundsValue();
      this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(this.Bounds));
      this.Dictionary.SetProperty("LLE", (IPdfPrimitive) new PdfNumber(this.m_lineExtension));
      if (!this.SetAppearanceDictionary)
        return;
      this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(rectangleF));
    }
  }

  private void FlattenAnnotation(PdfPageBase page, PdfTemplate appearance)
  {
    RectangleF bounds = RectangleF.Empty;
    if (appearance.m_content.ContainsKey("BBox"))
    {
      bounds = (appearance.m_content["BBox"] as PdfArray).ToRectangle();
      bounds.Y = this.Page.Size.Height - (bounds.Y + bounds.Height);
    }
    PdfGraphics graphics = page.Graphics;
    PdfGraphics layerGraphics = this.GetLayerGraphics();
    if (layerGraphics != null)
      graphics = layerGraphics;
    PdfGraphicsState state = graphics.Save();
    if ((double) this.Opacity < 1.0)
      graphics.SetTransparency(this.Opacity);
    bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
    RectangleF templateBounds = this.CalculateTemplateBounds(bounds, page, appearance, isNormalMatrix, graphics);
    graphics.DrawPdfTemplate(appearance, templateBounds.Location, templateBounds.Size);
    graphics.Restore(state);
    this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
  }
}
