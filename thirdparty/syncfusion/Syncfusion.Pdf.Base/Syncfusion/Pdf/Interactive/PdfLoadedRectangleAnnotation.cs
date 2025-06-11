// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedRectangleAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedRectangleAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private LineBorder m_lineborder = new LineBorder();
  private PdfArray m_dashedArray;
  private bool m_isDashArrayReset;
  private PdfBorderEffect m_borderEffect = new PdfBorderEffect();
  private float m_borderWidth;
  private bool ismodified;

  internal bool IsModified
  {
    get => this.ismodified;
    set => this.ismodified = value;
  }

  public PdfLoadedPopupAnnotationCollection ReviewHistory
  {
    get
    {
      if (this.m_reviewHistory == null)
        this.m_reviewHistory = new PdfLoadedPopupAnnotationCollection(this.Page, this.Dictionary, true);
      return this.m_reviewHistory;
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

  public LineBorder LineBorder
  {
    get
    {
      this.m_lineborder = this.ObtainLineBorder();
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_lineborder);
      return this.m_lineborder;
    }
    set
    {
      this.m_lineborder = value;
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_lineborder);
    }
  }

  public PdfBorderEffect BorderEffect
  {
    get => this.m_borderEffect;
    set
    {
      this.m_borderEffect = value;
      this.Dictionary.SetProperty("BE", (IPdfWrapper) this.m_borderEffect);
      if (!this.Dictionary.ContainsKey("AP"))
        return;
      this.ismodified = true;
    }
  }

  internal PdfLoadedRectangleAnnotation(
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

  private LineBorder ObtainLineBorder()
  {
    if (this.Dictionary.ContainsKey("Border"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Border"]) is PdfArray pdfArray1 && pdfArray1.Count >= 2 && pdfArray1[2] is PdfNumber)
      {
        float floatValue = (pdfArray1[2] as PdfNumber).FloatValue;
        this.m_lineborder.BorderWidth = (int) floatValue;
        this.m_lineborder.BorderLineWidth = floatValue;
      }
    }
    else if (this.Dictionary.ContainsKey("BS"))
    {
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["BS"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("W") && PdfCrossTable.Dereference(pdfDictionary["W"]) is PdfNumber pdfNumber)
      {
        int intValue = pdfNumber.IntValue;
        float floatValue = pdfNumber.FloatValue;
        this.m_lineborder.BorderWidth = intValue;
        this.m_lineborder.BorderLineWidth = floatValue;
      }
      if (pdfDictionary.ContainsKey("S"))
      {
        PdfName pdfName = PdfCrossTable.Dereference(pdfDictionary["S"]) as PdfName;
        if (pdfName != (PdfName) null)
          this.m_lineborder.BorderStyle = this.GetBorderStyle(pdfName.Value.ToString());
      }
      if (pdfDictionary.ContainsKey("D"))
      {
        PdfArray pdfArray2 = pdfDictionary["D"] as PdfArray;
        if (!this.m_isDashArrayReset)
        {
          this.m_dashedArray = pdfArray2.Clone(this.CrossTable) as PdfArray;
          this.m_isDashArrayReset = true;
        }
        int intValue = (pdfArray2[0] as PdfNumber).IntValue;
        pdfArray2.Clear();
        pdfArray2.Insert(0, (IPdfPrimitive) new PdfNumber(intValue));
        pdfArray2.Insert(1, (IPdfPrimitive) new PdfNumber(intValue));
        this.m_lineborder.DashArray = intValue;
      }
    }
    return this.m_lineborder;
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

  protected override void Save()
  {
    this.CheckFlatten();
    if (this.Flatten || this.Page.Annotations.Flatten || this.SetAppearanceDictionary || this.IsModified)
    {
      PdfTemplate appearance = this.CreateAppearance();
      if (this.Flatten || this.Page.Annotations.Flatten)
        this.FlattenAnnotation((PdfPageBase) this.Page, appearance);
      else if (appearance != null)
      {
        this.Appearance.Normal = appearance;
        this.Dictionary.SetProperty("AP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance));
      }
    }
    if (this.FlattenPopUps)
      this.FlattenLoadedPopup();
    this.m_isDashArrayReset = false;
  }

  private void FlattenAnnotation(PdfPageBase page, PdfTemplate appearance)
  {
    if (this.Dictionary.ContainsKey("AP") && appearance == null)
    {
      if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary))
        return;
      if (PdfCrossTable.Dereference(pdfDictionary["N"]) is PdfDictionary dictionary)
      {
        if (!(dictionary is PdfStream template))
          return;
        appearance = new PdfTemplate(template);
        if (appearance == null)
          return;
        bool isNormalMatrix = this.ValidateTemplateMatrix(dictionary);
        if ((!isNormalMatrix || this.Page.Rotation == PdfPageRotateAngle.RotateAngle0) && !this.IsValidTemplateMatrix(dictionary, this.Bounds.Location, appearance))
          return;
        this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
      }
      else
      {
        this.SetAppearanceDictionary = true;
        appearance = this.CreateAppearance();
        if (appearance == null)
          return;
        bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
        this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
      }
    }
    else if (!this.Dictionary.ContainsKey("AP") && appearance == null)
    {
      this.SetAppearanceDictionary = true;
      appearance = this.CreateAppearance();
      if (appearance == null)
        return;
      bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
      this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
    else if (!this.Dictionary.ContainsKey("AP") && appearance != null)
    {
      bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
      this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
    else
    {
      if (!this.Dictionary.ContainsKey("AP") || appearance == null)
        return;
      bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
      this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
  }

  private PdfTemplate CreateAppearance()
  {
    this.m_borderWidth = this.LineBorder.BorderWidth == 1 || (double) this.LineBorder.BorderLineWidth != 0.0 ? this.LineBorder.BorderLineWidth : (float) this.LineBorder.BorderWidth;
    if (!this.SetAppearanceDictionary && !this.IsModified)
      return (PdfTemplate) null;
    PaintParams paintParams = new PaintParams();
    float borderWidth = this.m_borderWidth / 2f;
    PdfPen mBorderPen = new PdfPen(this.Color, this.m_borderWidth);
    PdfBrush pdfBrush = (PdfBrush) null;
    RectangleF rectangleF1 = RectangleF.Empty;
    if (this.Dictionary["RD"] == null && (double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
    {
      RectangleF rectangleF2 = new RectangleF((float) ((double) this.Bounds.X - (double) this.BorderEffect.Intensity * 5.0 - (double) this.m_borderWidth / 2.0), (float) ((double) this.Bounds.Y - (double) this.BorderEffect.Intensity * 5.0 - (double) this.m_borderWidth / 2.0), this.Bounds.Width + this.BorderEffect.Intensity * 10f + this.m_borderWidth, this.Bounds.Height + this.BorderEffect.Intensity * 10f + this.m_borderWidth);
      float num = this.BorderEffect.Intensity * 5f;
      this.Dictionary.SetProperty("RD", (IPdfPrimitive) new PdfArray(new float[4]
      {
        num + this.m_borderWidth / 2f,
        num + this.m_borderWidth / 2f,
        num + this.m_borderWidth / 2f,
        num + this.m_borderWidth / 2f
      }));
      this.Bounds = rectangleF2;
    }
    if (!this.isBounds && this.Dictionary["RD"] != null)
    {
      PdfArray pdfArray = this.Dictionary["RD"] as PdfArray;
      RectangleF rectangleF3 = new RectangleF(this.Bounds.X + (pdfArray.Elements[0] as PdfNumber).FloatValue, this.Bounds.Y + (pdfArray.Elements[1] as PdfNumber).FloatValue, this.Bounds.Width - (pdfArray.Elements[2] as PdfNumber).FloatValue * 2f, this.Bounds.Height - (pdfArray.Elements[3] as PdfNumber).FloatValue * 2f);
      if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
      {
        rectangleF3.X = (float) ((double) rectangleF3.X - (double) this.BorderEffect.Intensity * 5.0 - (double) this.m_borderWidth / 2.0);
        rectangleF3.Y = (float) ((double) rectangleF3.Y - (double) this.BorderEffect.Intensity * 5.0 - (double) this.m_borderWidth / 2.0);
        rectangleF3.Width = rectangleF3.Width + this.BorderEffect.Intensity * 10f + this.m_borderWidth;
        rectangleF3.Height = rectangleF3.Height + this.BorderEffect.Intensity * 10f + this.m_borderWidth;
        float num = this.BorderEffect.Intensity * 5f;
        this.Dictionary.SetProperty("RD", (IPdfPrimitive) new PdfArray(new float[4]
        {
          num + this.m_borderWidth / 2f,
          num + this.m_borderWidth / 2f,
          num + this.m_borderWidth / 2f,
          num + this.m_borderWidth / 2f
        }));
      }
      else
        this.Dictionary.Remove("RD");
      this.Bounds = rectangleF3;
    }
    rectangleF1 = new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height);
    PdfTemplate appearance = new PdfTemplate(rectangleF1);
    this.SetMatrix((PdfDictionary) appearance.m_content);
    if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
      appearance.m_writeTransformation = false;
    PdfGraphics graphics = appearance.Graphics;
    if (this.InnerColor.A != (byte) 0)
      pdfBrush = (PdfBrush) new PdfSolidBrush(this.InnerColor);
    if ((double) this.m_borderWidth > 0.0)
      paintParams.BorderPen = mBorderPen;
    paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
    paintParams.BackBrush = pdfBrush;
    RectangleF style = this.ObtainStyle(mBorderPen, rectangleF1, borderWidth);
    if ((double) this.Opacity < 1.0)
    {
      graphics.Save();
      graphics.SetTransparency(this.Opacity);
    }
    if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
      this.DrawAppearance(style, borderWidth, graphics, paintParams);
    else
      FieldPainter.DrawRectangleAnnotation(graphics, paintParams, style.X, style.Y, style.Width, style.Height);
    if ((double) this.Opacity < 1.0)
      graphics.Restore();
    return appearance;
  }

  private void DrawAppearance(
    RectangleF rectangle,
    float borderWidth,
    PdfGraphics graphics,
    PaintParams paintParams)
  {
    GraphicsPath graphicsPath1 = new GraphicsPath();
    graphicsPath1.AddRectangle(rectangle);
    float radius = this.BorderEffect.Intensity * 4.25f;
    if ((double) radius > 0.0)
    {
      PointF[] points = new PointF[graphicsPath1.PathPoints.Length];
      for (int index = 0; index < graphicsPath1.PathPoints.Length; ++index)
        points[index] = new PointF(graphicsPath1.PathPoints[index].X, -graphicsPath1.PathPoints[index].Y);
      GraphicsPath graphicsPath2 = new GraphicsPath();
      graphicsPath2.AddPolygon(points);
      this.DrawCloudStyle(graphics, paintParams.BackBrush, paintParams.BorderPen, radius, 0.833f, graphicsPath2.PathPoints, false);
    }
    else
      FieldPainter.DrawRectangleAnnotation(graphics, paintParams, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  private RectangleF ObtainStyle(PdfPen mBorderPen, RectangleF rectangle, float borderWidth)
  {
    this.m_borderWidth = this.LineBorder.BorderWidth != 1 ? (float) this.LineBorder.BorderWidth : this.LineBorder.BorderLineWidth;
    if (this.Dictionary.ContainsKey("BS") && PdfCrossTable.Dereference(this.Dictionary["BS"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("D"))
    {
      PdfArray dashedArray = this.m_dashedArray;
      float[] numArray = new float[dashedArray.Count];
      bool flag = false;
      for (int index = 0; index < dashedArray.Count; ++index)
      {
        numArray[index] = (dashedArray.Elements[index] as PdfNumber).FloatValue;
        if ((double) numArray[index] > 0.0)
          flag = true;
      }
      if (flag && this.LineBorder.BorderStyle == PdfBorderStyle.Dashed)
      {
        mBorderPen.DashStyle = PdfDashStyle.Dash;
        mBorderPen.DashPattern = numArray;
      }
    }
    if ((double) this.BorderEffect.Intensity != 0.0 && this.BorderEffect.Style == PdfBorderEffectStyle.Cloudy)
    {
      float num = this.BorderEffect.Intensity * 5f;
      rectangle.X = rectangle.X + num + borderWidth;
      rectangle.Y = rectangle.Y + num + borderWidth;
      rectangle.Width = (float) ((double) rectangle.Width - 2.0 * (double) num - 2.0 * (double) borderWidth);
      rectangle.Height = (float) ((double) rectangle.Height - 2.0 * (double) num - 2.0 * (double) borderWidth);
    }
    else
    {
      rectangle.X += borderWidth;
      rectangle.Y += borderWidth;
      rectangle.Width -= this.m_borderWidth;
      rectangle.Height = this.Bounds.Height - this.m_borderWidth;
    }
    return rectangle;
  }
}
