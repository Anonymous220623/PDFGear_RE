// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedCircleAnnotation
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

public class PdfLoadedCircleAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private LineBorder m_border = new LineBorder();
  private PdfArray m_DashedArray;
  private bool m_isDashArrayReset;
  private float m_borderWidth;

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

  internal PdfLoadedCircleAnnotation(
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
  }

  private LineBorder GetLineBorder()
  {
    if (this.Dictionary.ContainsKey("Border"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Border"]) is PdfArray pdfArray1 && pdfArray1.Count >= 2 && pdfArray1[2] is PdfNumber)
      {
        float floatValue = (pdfArray1[2] as PdfNumber).FloatValue;
        this.m_border.BorderWidth = (int) floatValue;
        this.m_border.BorderLineWidth = floatValue;
      }
    }
    else if (this.Dictionary.ContainsKey("BS"))
    {
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["BS"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("W"))
      {
        int intValue = (pdfDictionary["W"] as PdfNumber).IntValue;
        float floatValue = (pdfDictionary["W"] as PdfNumber).FloatValue;
        this.m_border.BorderWidth = intValue;
        this.m_border.BorderLineWidth = floatValue;
      }
      if (pdfDictionary.ContainsKey("S"))
        this.m_border.BorderStyle = this.GetBorderStyle((pdfDictionary["S"] as PdfName).Value.ToString());
      if (pdfDictionary.ContainsKey("D") && PdfCrossTable.Dereference(pdfDictionary["D"]) is PdfArray pdfArray2)
      {
        if (!this.m_isDashArrayReset)
        {
          this.m_DashedArray = pdfArray2.Clone(this.CrossTable) as PdfArray;
          this.m_isDashArrayReset = true;
        }
        if (pdfArray2[0] is PdfNumber pdfNumber)
        {
          int intValue = pdfNumber.IntValue;
          pdfArray2.Clear();
          pdfArray2.Insert(0, (IPdfPrimitive) new PdfNumber(intValue));
          pdfArray2.Insert(1, (IPdfPrimitive) new PdfNumber(intValue));
          this.m_border.DashArray = intValue;
        }
      }
    }
    return this.m_border;
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
    this.m_borderWidth = this.Border.BorderWidth == 1 || (double) this.Border.BorderLineWidth != 0.0 ? this.Border.BorderLineWidth : (float) this.Border.BorderWidth;
    if (this.Flatten || this.Page.Annotations.Flatten || this.SetAppearanceDictionary)
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
    if (!this.SetAppearanceDictionary)
      return (PdfTemplate) null;
    PaintParams paintParams = new PaintParams();
    float borderWidth = this.m_borderWidth / 2f;
    PdfPen mBorderPen = new PdfPen(this.Color, this.m_borderWidth);
    PdfBrush pdfBrush = (PdfBrush) null;
    RectangleF rectangleF = new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height);
    PdfTemplate appearance = new PdfTemplate(rectangleF);
    this.SetMatrix((PdfDictionary) appearance.m_content);
    if (this.Dictionary.ContainsKey("BE"))
      appearance.m_writeTransformation = false;
    PdfGraphics graphics = appearance.Graphics;
    if (this.InnerColor.A != (byte) 0)
      pdfBrush = (PdfBrush) new PdfSolidBrush(this.InnerColor);
    if ((double) this.m_borderWidth > 0.0)
      paintParams.BorderPen = mBorderPen;
    paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
    paintParams.BackBrush = pdfBrush;
    paintParams.BorderWidth = (float) (int) this.m_borderWidth;
    RectangleF style = this.ObtainStyle(mBorderPen, rectangleF, borderWidth);
    if ((double) this.Opacity < 1.0)
    {
      graphics.Save();
      graphics.SetTransparency(this.Opacity);
    }
    if (this.Dictionary.ContainsKey("BE"))
      this.DrawAppearance(style, borderWidth, graphics, paintParams);
    else
      FieldPainter.DrawEllipseAnnotation(graphics, paintParams, style.X + borderWidth, style.Y, style.Width - this.m_borderWidth, style.Height);
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
    this.m_borderWidth = this.Border.BorderWidth != 1 ? (float) this.Border.BorderWidth : this.Border.BorderLineWidth;
    new GraphicsPath().AddEllipse(new RectangleF(rectangle.X + borderWidth, -rectangle.Y - rectangle.Height, rectangle.Width - this.m_borderWidth, rectangle.Height));
    float radius = 0.0f;
    if (this.Dictionary.ContainsKey("RD") && PdfCrossTable.Dereference(this.Dictionary.Items[new PdfName("RD")]) is PdfArray pdfArray)
      radius = (pdfArray.Elements[0] as PdfNumber).FloatValue;
    if ((double) radius > 0.0)
    {
      RectangleF rectangleF = new RectangleF(rectangle.X + borderWidth, -rectangle.Y - rectangle.Height, rectangle.Width - this.m_borderWidth, rectangle.Height);
      List<PointF> pointFList1 = new List<PointF>();
      List<PointF> pointFList2 = new List<PointF>();
      List<PointF> pointFList3 = new List<PointF>();
      List<PointF> bezierPoints = new List<PointF>();
      pointFList2.Add(new PointF(rectangleF.Right, rectangleF.Bottom));
      pointFList2.Add(new PointF(rectangleF.Left, rectangleF.Bottom));
      pointFList2.Add(new PointF(rectangleF.Left, rectangleF.Top));
      pointFList2.Add(new PointF(rectangleF.Right, rectangleF.Top));
      pointFList1.Add(new PointF(rectangleF.Right, rectangleF.Top + rectangleF.Height / 2f));
      pointFList1.Add(new PointF(rectangleF.Left + rectangleF.Width / 2f, rectangleF.Bottom));
      pointFList1.Add(new PointF(rectangleF.Left, rectangleF.Top + rectangleF.Height / 2f));
      pointFList1.Add(new PointF(rectangleF.Left + rectangleF.Width / 2f, rectangleF.Top));
      pointFList3.Add(new PointF(rectangleF.Left + rectangleF.Width / 2f, rectangleF.Bottom));
      pointFList3.Add(new PointF(rectangleF.Left, rectangleF.Top + rectangleF.Height / 2f));
      pointFList3.Add(new PointF(rectangleF.Left + rectangleF.Width / 2f, rectangleF.Top));
      pointFList3.Add(new PointF(rectangleF.Right, rectangleF.Top + rectangleF.Height / 2f));
      for (int index = 0; index < pointFList2.Count; ++index)
        this.CreateBezier(pointFList1[index], pointFList2[index], pointFList3[index], bezierPoints);
      this.DrawCloudStyle(graphics, paintParams.BackBrush, paintParams.BorderPen, radius, 0.833f, bezierPoints.ToArray(), false);
      pointFList1.Clear();
      pointFList2.Clear();
      pointFList3.Clear();
      bezierPoints.Clear();
    }
    else
      FieldPainter.DrawEllipseAnnotation(graphics, paintParams, rectangle.X + borderWidth, -rectangle.Y, rectangle.Width - this.m_borderWidth, -rectangle.Height);
  }

  private RectangleF ObtainStyle(PdfPen mBorderPen, RectangleF rectangle, float borderWidth)
  {
    if (this.Dictionary.ContainsKey("BS") && PdfCrossTable.Dereference(this.Dictionary["BS"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("D"))
    {
      PdfArray dashedArray = this.m_DashedArray;
      float[] numArray = new float[dashedArray.Count];
      bool flag = false;
      for (int index = 0; index < dashedArray.Count; ++index)
      {
        numArray[index] = (dashedArray.Elements[index] as PdfNumber).FloatValue;
        if ((double) numArray[index] > 0.0)
          flag = true;
      }
      if (flag && this.Border.BorderStyle == PdfBorderStyle.Dashed)
      {
        mBorderPen.DashStyle = PdfDashStyle.Dash;
        mBorderPen.DashPattern = numArray;
      }
    }
    if (!this.isBounds && this.Dictionary["RD"] != null)
    {
      if (PdfCrossTable.Dereference(this.Dictionary["RD"]) is PdfArray pdfArray)
      {
        PdfNumber element1 = pdfArray.Elements[0] as PdfNumber;
        PdfNumber element2 = pdfArray.Elements[1] as PdfNumber;
        PdfNumber element3 = pdfArray.Elements[2] as PdfNumber;
        PdfNumber element4 = pdfArray.Elements[3] as PdfNumber;
        rectangle.X += element1.FloatValue;
        rectangle.Y = rectangle.Y + borderWidth + element2.FloatValue;
        rectangle.Width -= 2f * element3.FloatValue;
        rectangle.Height -= this.m_borderWidth;
        rectangle.Height -= 2f * element4.FloatValue;
      }
    }
    else
    {
      rectangle.Y += borderWidth;
      rectangle.Height = this.Bounds.Height - this.m_borderWidth;
    }
    return rectangle;
  }

  private void CreateBezier(PointF ctrl1, PointF ctrl2, PointF ctrl3, List<PointF> bezierPoints)
  {
    bezierPoints.Add(ctrl1);
    this.PopulateBezierPoints(ctrl1, ctrl2, ctrl3, 0, bezierPoints);
    bezierPoints.Add(ctrl3);
  }

  private void PopulateBezierPoints(
    PointF ctrl1,
    PointF ctrl2,
    PointF ctrl3,
    int currentIteration,
    List<PointF> bezierPoints)
  {
    if (currentIteration >= 2)
      return;
    PointF pointF1 = this.MidPoint(ctrl1, ctrl2);
    PointF pointF2 = this.MidPoint(ctrl2, ctrl3);
    PointF pointF3 = this.MidPoint(pointF1, pointF2);
    ++currentIteration;
    this.PopulateBezierPoints(ctrl1, pointF1, pointF3, currentIteration, bezierPoints);
    bezierPoints.Add(pointF3);
    this.PopulateBezierPoints(pointF3, pointF2, ctrl3, currentIteration, bezierPoints);
  }

  private PointF MidPoint(PointF controlPoint1, PointF controlPoint2)
  {
    return new PointF((float) (((double) controlPoint1.X + (double) controlPoint2.X) / 2.0), (float) (((double) controlPoint1.Y + (double) controlPoint2.Y) / 2.0));
  }
}
