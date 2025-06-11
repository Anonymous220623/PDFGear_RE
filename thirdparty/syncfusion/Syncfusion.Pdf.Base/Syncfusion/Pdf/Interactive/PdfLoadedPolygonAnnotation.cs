// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedPolygonAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedPolygonAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private LineBorder m_border = new LineBorder();
  private float[] m_dashPattern;
  private PdfBorderEffect m_borderEffect = new PdfBorderEffect();
  private float m_borderWidth;

  public PdfLoadedPopupAnnotationCollection ReviewHistory
  {
    get
    {
      if (this.m_reviewHistory == null)
        this.m_reviewHistory = new PdfLoadedPopupAnnotationCollection(this.Page, this.Dictionary, true);
      return this.m_reviewHistory;
    }
  }

  public PdfBorderEffect BorderEffect
  {
    get => this.m_borderEffect;
    set
    {
      this.m_borderEffect = value;
      this.Dictionary.SetProperty("BE", (IPdfWrapper) this.m_borderEffect);
    }
  }

  public PdfLoadedPopupAnnotationCollection Comments
  {
    get
    {
      if (this.m_comments == null)
        this.m_comments = new PdfLoadedPopupAnnotationCollection(this.Page, this.Dictionary, false);
      return this.m_comments;
    }
  }

  public LineBorder Border
  {
    get
    {
      this.m_border = this.GetLineBorder();
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
      return this.m_border;
    }
    set
    {
      this.m_border = value;
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
    }
  }

  public int[] PolygonPoints
  {
    get
    {
      int[] polygonPoints = (int[]) null;
      if (this.Dictionary.ContainsKey("Vertices") && this.Dictionary["Vertices"] is PdfArray pdfArray)
      {
        polygonPoints = new int[pdfArray.Count];
        int index = 0;
        foreach (PdfNumber pdfNumber in pdfArray)
        {
          polygonPoints[index] = pdfNumber.IntValue;
          ++index;
        }
      }
      return polygonPoints;
    }
  }

  internal PdfLoadedPolygonAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
    : base(dictionary, crossTable)
  {
    if (text == null)
      throw new ArgumentNullException("Text must be not null");
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
    this.Text = text;
    this.m_borderEffect = new PdfBorderEffect(dictionary);
  }

  private LineBorder GetLineBorder()
  {
    LineBorder lineBorder = new LineBorder();
    if (this.Dictionary.ContainsKey("BS"))
    {
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["BS"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("W"))
      {
        int intValue = (pdfDictionary["W"] as PdfNumber).IntValue;
        float floatValue = (pdfDictionary["W"] as PdfNumber).FloatValue;
        lineBorder.BorderWidth = intValue;
        lineBorder.BorderLineWidth = floatValue;
      }
      if (pdfDictionary.ContainsKey("S"))
      {
        PdfName pdfName = pdfDictionary["S"] as PdfName;
        lineBorder.BorderStyle = this.GetBorderStyle(pdfName.Value.ToString());
      }
      if (pdfDictionary.ContainsKey("D") && PdfCrossTable.Dereference(pdfDictionary["D"]) is PdfArray pdfArray)
      {
        if (this.m_dashPattern == null)
        {
          this.m_dashPattern = new float[pdfArray.Count];
          for (int index = 0; index < pdfArray.Count; ++index)
          {
            if (pdfArray.Elements[index] is PdfNumber)
              this.m_dashPattern[index] = (pdfArray.Elements[index] as PdfNumber).FloatValue;
          }
        }
        int intValue = (pdfArray[0] as PdfNumber).IntValue;
        pdfArray.Clear();
        pdfArray.Insert(0, (IPdfPrimitive) new PdfNumber(intValue));
        pdfArray.Insert(1, (IPdfPrimitive) new PdfNumber(intValue));
        lineBorder.DashArray = intValue;
      }
    }
    return lineBorder;
  }

  private PdfBorderStyle GetBorderStyle(string bstyle)
  {
    PdfBorderStyle borderStyle = PdfBorderStyle.Solid;
    switch (bstyle)
    {
      case "S":
        borderStyle = PdfBorderStyle.Solid;
        break;
      case "D":
        borderStyle = PdfBorderStyle.Dashed;
        break;
      case "B":
        borderStyle = PdfBorderStyle.Beveled;
        break;
      case "I":
        borderStyle = PdfBorderStyle.Inset;
        break;
      case "U":
        borderStyle = PdfBorderStyle.Underline;
        break;
    }
    return borderStyle;
  }

  private PointF[] GetLinePoints()
  {
    PdfPageBase page = (PdfPageBase) this.Page;
    if (this.Page.Annotations.Count > 0 && page.Annotations.Flatten)
      this.Page.Annotations.Flatten = page.Annotations.Flatten;
    PointF[] linePoints = (PointF[]) null;
    PdfNumber pdfNumber1 = (PdfNumber) null;
    if (this.Dictionary.ContainsKey("Vertices"))
    {
      float height = this.Page.Size.Height;
      float width = this.Page.Size.Width;
      if (page != null && page.Dictionary.ContainsKey("Rotate"))
        pdfNumber1 = page.Dictionary["Rotate"] as PdfNumber;
      int rotation = (int) page.Rotation;
      if (page.Rotation != PdfPageRotateAngle.RotateAngle0)
      {
        if (page.Rotation == PdfPageRotateAngle.RotateAngle90)
          pdfNumber1 = new PdfNumber(90);
        else if (page.Rotation == PdfPageRotateAngle.RotateAngle180)
          pdfNumber1 = new PdfNumber(180);
        else if (page.Rotation == PdfPageRotateAngle.RotateAngle270)
          pdfNumber1 = new PdfNumber(270);
      }
      if (this.Dictionary["Vertices"] is PdfArray pdfArray)
      {
        int[] numArray = new int[pdfArray.Count];
        int index1 = 0;
        foreach (PdfNumber pdfNumber2 in pdfArray)
        {
          numArray[index1] = pdfNumber2.IntValue;
          ++index1;
        }
        linePoints = new PointF[numArray.Length / 2];
        int index2 = 0;
        for (int index3 = 0; index3 < numArray.Length; index3 += 2)
        {
          linePoints[index2] = this.Flatten || this.Page.Annotations.Flatten ? new PointF((float) numArray[index3], height - (float) numArray[index3 + 1]) : new PointF((float) numArray[index3], (float) -numArray[index3 + 1]);
          ++index2;
        }
        if (pdfNumber1 != null && (this.Flatten || this.Page.Annotations.Flatten))
        {
          if (pdfNumber1.IntValue == 270)
          {
            for (int index4 = 0; index4 < linePoints.Length; ++index4)
            {
              float x = linePoints[index4].X;
              linePoints[index4].X = linePoints[index4].Y;
              linePoints[index4].Y = width - x;
            }
          }
          else if (pdfNumber1.IntValue == 90)
          {
            for (int index5 = 0; index5 < linePoints.Length; ++index5)
            {
              float x = linePoints[index5].X;
              linePoints[index5].X = height - linePoints[index5].Y;
              linePoints[index5].Y = x;
            }
          }
          else if (pdfNumber1.IntValue == 180)
          {
            for (int index6 = 0; index6 < linePoints.Length; ++index6)
            {
              float x = linePoints[index6].X;
              linePoints[index6].X = width - x;
              linePoints[index6].Y = height - linePoints[index6].Y;
            }
          }
        }
      }
    }
    return linePoints;
  }

  private void GetBoundsValue()
  {
    this.Bounds = (this.Dictionary["Rect"] as PdfArray).ToRectangle();
    List<float> floatList1 = new List<float>();
    List<float> floatList2 = new List<float>();
    if (this.Dictionary.ContainsKey("Vertices"))
    {
      PdfArray pdfArray = PdfCrossTable.Dereference(this.Dictionary["Vertices"]) as PdfArray;
      int count = pdfArray.Count;
      if (pdfArray.Count > 0)
      {
        float[] numArray = new float[pdfArray.Count];
        int index1 = 0;
        foreach (PdfNumber pdfNumber in pdfArray)
        {
          numArray[index1] = pdfNumber.FloatValue;
          ++index1;
        }
        for (int index2 = 0; index2 < numArray.Length; ++index2)
        {
          if (index2 % 2 == 0)
            floatList1.Add(numArray[index2]);
          else
            floatList2.Add(numArray[index2]);
        }
      }
    }
    floatList1.Sort();
    floatList2.Sort();
    this.Bounds = new RectangleF(floatList1[0], floatList2[0], floatList1[floatList1.Count - 1] - floatList1[0], floatList2[floatList2.Count - 1] - floatList2[0]);
  }

  protected override void Save()
  {
    this.m_borderWidth = this.Border.BorderWidth == 1 || (double) this.Border.BorderLineWidth != 0.0 ? this.Border.BorderLineWidth : (float) this.Border.BorderWidth;
    PdfPageBase page = (PdfPageBase) this.Page;
    int count = this.Page.Annotations.Count;
    PdfGraphicsState state1 = (PdfGraphicsState) null;
    if (count > 0 && page.Annotations.Flatten)
      this.Page.Annotations.Flatten = page.Annotations.Flatten;
    RectangleF rectangleF = RectangleF.Empty;
    if (this.SetAppearanceDictionary)
    {
      PdfGraphics pdfGraphics = this.Page.Graphics;
      PdfGraphics graphics1 = this.ObtainlayerGraphics();
      if (graphics1 != null)
        pdfGraphics = graphics1;
      this.GetBoundsValue();
      rectangleF = (double) this.BorderEffect.Intensity == 0.0 || this.BorderEffect.Style != PdfBorderEffectStyle.Cloudy ? new RectangleF(this.Bounds.X - this.m_borderWidth, this.Bounds.Y - this.m_borderWidth, this.Bounds.Width + 2f * this.m_borderWidth, this.Bounds.Height + 2f * this.m_borderWidth) : new RectangleF(this.Bounds.X - this.BorderEffect.Intensity * 5f - this.m_borderWidth, this.Bounds.Y - this.BorderEffect.Intensity * 5f - this.m_borderWidth, (float) ((double) this.Bounds.Width + (double) this.BorderEffect.Intensity * 10.0 + 2.0 * (double) this.m_borderWidth), (float) ((double) this.Bounds.Height + (double) this.BorderEffect.Intensity * 10.0 + 2.0 * (double) this.m_borderWidth));
      this.Dictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
      if (this.Dictionary["AP"] != null)
      {
        this.Appearance.Normal = new PdfTemplate(rectangleF);
        PaintParams paintParams = new PaintParams();
        this.Appearance.Normal.m_writeTransformation = false;
        PdfGraphics graphics2 = this.Appearance.Normal.Graphics;
        PdfBrush brush = (PdfBrush) null;
        if (this.InnerColor.A != (byte) 0)
          brush = (PdfBrush) new PdfSolidBrush(this.InnerColor);
        paintParams.BackBrush = brush;
        PdfPen pen = (PdfPen) null;
        if ((double) this.m_borderWidth > 0.0)
          pen = new PdfPen(this.Color, this.m_borderWidth);
        paintParams.BorderPen = pen;
        if (this.Dictionary.ContainsKey("BS"))
        {
          PdfDictionary pdfDictionary = (object) (this.Dictionary.Items[new PdfName("BS")] as PdfReferenceHolder) == null ? this.Dictionary.Items[new PdfName("BS")] as PdfDictionary : (this.Dictionary.Items[new PdfName("BS")] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey("D"))
          {
            PdfArray pdfArray = PdfCrossTable.Dereference(pdfDictionary.Items[new PdfName("D")]) as PdfArray;
            float[] numArray = new float[pdfArray.Count];
            for (int index = 0; index < pdfArray.Count; ++index)
              numArray[index] = (pdfArray.Elements[index] as PdfNumber).FloatValue;
            pen.DashStyle = PdfDashStyle.Dash;
            pen.isSkipPatternWidth = true;
            pen.DashPattern = numArray;
            pen.DashPattern = this.m_dashPattern;
          }
        }
        if (this.Flatten || this.Page.Annotations.Flatten)
        {
          this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
          if ((double) this.Opacity < 1.0)
          {
            state1 = pdfGraphics.Save();
            pdfGraphics.SetTransparency(this.Opacity);
          }
          if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
          {
            this.CalculateBounds(this.Bounds, (PdfPage) null, this.Page);
            float radius = (float) ((double) this.BorderEffect.Intensity * 4.0 + 0.5 * (double) this.m_borderWidth);
            if ((double) radius > 0.0)
            {
              GraphicsPath graphicsPath = new GraphicsPath();
              graphicsPath.AddPolygon(this.GetLinePoints());
              if ((double) graphicsPath.PathPoints[0].Y > (double) graphicsPath.PathPoints[graphicsPath.PathPoints.Length - 1].Y)
              {
                graphicsPath.Reverse();
                this.DrawCloudStyle(graphics2, brush, pen, radius, 0.833f, graphicsPath.PathPoints, false);
              }
              if (graphics1 != null)
                this.DrawCloudStyle(graphics1, brush, pen, radius, 0.833f, graphicsPath.PathPoints, false);
              else
                this.DrawCloudStyle(page.Graphics, brush, pen, radius, 0.833f, graphicsPath.PathPoints, false);
            }
            else if (graphics1 != null)
              graphics1.DrawPolygon(pen, brush, this.GetLinePoints());
            else
              this.Page.Graphics.DrawPolygon(pen, brush, this.GetLinePoints());
          }
          else if (graphics1 != null)
            graphics1.DrawPolygon(pen, brush, this.GetLinePoints());
          else
            this.Page.Graphics.DrawPolygon(pen, brush, this.GetLinePoints());
          if ((double) this.Opacity < 1.0)
            page.Graphics.Restore(state1);
        }
        else
        {
          if ((double) this.Opacity < 1.0)
          {
            state1 = graphics2.Save();
            graphics2.SetTransparency(this.Opacity);
          }
          if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
          {
            this.CalculateBounds(this.Bounds, (PdfPage) null, this.Page);
            float radius = (float) ((double) this.BorderEffect.Intensity * 4.0 + 0.5 * (double) this.m_borderWidth);
            int num = (this.Dictionary["Vertices"] as PdfArray).Count / 2;
            GraphicsPath graphicsPath1 = new GraphicsPath();
            graphicsPath1.AddPolygon(this.GetLinePoints());
            if ((double) graphicsPath1.PathPoints[0].Y > (double) graphicsPath1.PathPoints[graphicsPath1.PathPoints.Length - 1].Y)
            {
              PointF[] points = new PointF[graphicsPath1.PathPoints.Length];
              for (int index = 0; index < graphicsPath1.PathPoints.Length; ++index)
                points[index] = new PointF(graphicsPath1.PathPoints[index].X, graphicsPath1.PathPoints[index].Y);
              GraphicsPath graphicsPath2 = new GraphicsPath();
              graphicsPath2.AddPolygon(points);
              this.DrawCloudStyle(graphics2, brush, pen, radius, 0.833f, graphicsPath2.PathPoints, false);
            }
            else
              this.DrawCloudStyle(graphics2, brush, pen, radius, 0.833f, graphicsPath1.PathPoints, false);
          }
          else
            graphics2.DrawPolygon(pen, brush, this.GetLinePoints());
          if ((double) this.Opacity < 1.0)
            graphics2.Restore(state1);
        }
        this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(rectangleF));
      }
    }
    if ((!this.Flatten || this.SetAppearanceDictionary) && (!this.Page.Annotations.Flatten || this.SetAppearanceDictionary))
      return;
    PdfGraphics graphics3 = this.Page.Graphics;
    PdfGraphics graphics4 = this.ObtainlayerGraphics();
    if (graphics4 != null)
      graphics3 = graphics4;
    if (this.Dictionary["AP"] != null)
    {
      if (PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1 && PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary dictionary && dictionary is PdfStream template1)
      {
        PdfTemplate template = new PdfTemplate(template1);
        if (template != null)
        {
          PdfGraphicsState state2 = graphics3.Save();
          if ((double) this.Opacity < 1.0)
            graphics3.SetTransparency(this.Opacity);
          bool isNormalMatrix = this.ValidateTemplateMatrix(dictionary);
          RectangleF templateBounds = this.CalculateTemplateBounds(this.Bounds, page, template, isNormalMatrix, graphics3);
          graphics3.DrawPdfTemplate(template, templateBounds.Location, templateBounds.Size);
          graphics3.Restore(state2);
          this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
        }
      }
    }
    else
    {
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
      PdfPen pen = new PdfPen(this.Color, this.m_borderWidth);
      PdfColor innerColor = this.InnerColor;
      PdfBrush brush = innerColor.IsEmpty ? (PdfBrush) null : (PdfBrush) new PdfSolidBrush(innerColor);
      if ((double) this.Opacity < 1.0)
      {
        state1 = graphics3.Save();
        graphics3.SetTransparency(this.Opacity);
      }
      if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
      {
        this.CalculateBounds(this.Bounds, (PdfPage) null, this.Page);
        float radius = (float) ((double) this.BorderEffect.Intensity * 4.0 + 0.5 * (double) this.m_borderWidth);
        GraphicsPath graphicsPath = new GraphicsPath();
        graphicsPath.AddPolygon(this.GetLinePoints());
        if (graphics4 != null)
          this.DrawCloudStyle(graphics4, brush, pen, radius, 0.833f, graphicsPath.PathPoints, false);
        else
          this.DrawCloudStyle(page.Graphics, brush, pen, radius, 0.833f, graphicsPath.PathPoints, false);
      }
      else if (graphics4 != null)
        graphics4.DrawPolygon(pen, brush, this.GetLinePoints());
      else
        this.Page.Graphics.DrawPolygon(pen, brush, this.GetLinePoints());
      if ((double) this.Opacity < 1.0)
        graphics3.Restore(state1);
    }
    if (!this.FlattenPopUps)
      return;
    this.FlattenLoadedPopup();
  }
}
