// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedTextMarkupAnnotation
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

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedTextMarkupAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private PdfDictionary m_dictionary;
  private PdfTextMarkupAnnotationType m_TextMarkupAnnotationType;
  private PdfColor m_color;
  private List<RectangleF> m_boundscollection = new List<RectangleF>();
  private PdfDictionary m_borderDic = new PdfDictionary();
  private PdfLineBorderStyle m_borderStyle;

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

  public PdfTextMarkupAnnotationType TextMarkupAnnotationType
  {
    get => this.ObtainTextMarkupAnnotationType();
    set
    {
      this.m_TextMarkupAnnotationType = value;
      this.Dictionary.SetName("Subtype", this.m_TextMarkupAnnotationType.ToString());
    }
  }

  public PdfColor TextMarkupColor
  {
    get => this.ObtainTextMarkupColor();
    set
    {
      PdfArray primitive = new PdfArray();
      this.m_color = value;
      primitive.Insert(0, (IPdfPrimitive) new PdfNumber((float) this.m_color.R / (float) byte.MaxValue));
      primitive.Insert(1, (IPdfPrimitive) new PdfNumber((float) this.m_color.G / (float) byte.MaxValue));
      primitive.Insert(2, (IPdfPrimitive) new PdfNumber((float) this.m_color.B / (float) byte.MaxValue));
      this.Dictionary.SetProperty("C", (IPdfPrimitive) primitive);
    }
  }

  public List<RectangleF> BoundsCollection
  {
    get
    {
      this.m_boundscollection = this.ObtainBoundsValue();
      return this.m_boundscollection;
    }
    set
    {
      this.m_boundscollection = value;
      this.SetQuadPoints(this.Page.Size);
    }
  }

  internal PdfLineBorderStyle BorderStyle
  {
    get => this.GetLineBorder();
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

  internal PdfLoadedTextMarkupAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectagle)
    : base(dictionary, crossTable)
  {
    this.m_dictionary = dictionary;
    this.m_crossTable = crossTable;
  }

  public void SetTitleText(string text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (text == string.Empty)
      throw new ArgumentException("The text can't be empty");
    if (!(this.Text != text))
      return;
    PdfString pdfString = new PdfString(text);
    this.Dictionary.SetString("T", text);
    this.Changed = true;
  }

  private List<RectangleF> ObtainBoundsValue()
  {
    List<RectangleF> boundsValue = new List<RectangleF>();
    if (this.Dictionary.ContainsKey("QuadPoints") && PdfCrossTable.Dereference(this.Dictionary["QuadPoints"]) is PdfArray pdfArray)
    {
      int num1 = pdfArray.Count / 8;
      for (int index = 0; index < num1; ++index)
      {
        float num2 = (float) ((pdfArray[4 + index * 8] as PdfNumber).IntValue - (pdfArray[index * 8] as PdfNumber).IntValue);
        float num3 = (float) ((pdfArray[5 + index * 8] as PdfNumber).IntValue - (pdfArray[1 + index * 8] as PdfNumber).IntValue);
        double height = Math.Sqrt((double) num2 * (double) num2 + (double) num3 * (double) num3);
        float num4 = (float) ((pdfArray[6 + index * 8] as PdfNumber).IntValue - (pdfArray[4 + index * 8] as PdfNumber).IntValue);
        float num5 = (float) ((pdfArray[7 + index * 8] as PdfNumber).IntValue - (pdfArray[5 + index * 8] as PdfNumber).IntValue);
        double width = Math.Sqrt((double) num4 * (double) num4 + (double) num5 * (double) num5);
        RectangleF rectangleF = new RectangleF((pdfArray[index * 8] as PdfNumber).FloatValue, this.Page.Size.Height - (pdfArray[1 + index * 8] as PdfNumber).FloatValue, (float) width, (float) height);
        boundsValue.Add(rectangleF);
      }
    }
    return boundsValue;
  }

  private void SetQuadPoints(SizeF pageSize)
  {
    float[] array = new float[this.m_boundscollection.Count * 8];
    double width = (double) pageSize.Width;
    float height = pageSize.Height;
    for (int index = 0; index < this.m_boundscollection.Count; ++index)
    {
      float x = this.m_boundscollection[index].X;
      float y = this.m_boundscollection[index].Y;
      array[index * 8] = x;
      array[1 + index * 8] = height - y;
      array[2 + index * 8] = x + this.m_boundscollection[index].Width;
      array[3 + index * 8] = height - y;
      array[4 + index * 8] = x;
      array[5 + index * 8] = array[1 + index * 8] - this.m_boundscollection[index].Height;
      array[6 + index * 8] = x + this.m_boundscollection[index].Width;
      array[7 + index * 8] = array[5 + index * 8];
    }
    this.Dictionary.SetProperty("QuadPoints", (IPdfPrimitive) new PdfArray(array));
  }

  private PdfTextMarkupAnnotationType ObtainTextMarkupAnnotationType()
  {
    return this.GetTextMarkupAnnotation((this.Dictionary["Subtype"] as PdfName).Value.ToString());
  }

  private PdfTextMarkupAnnotationType GetTextMarkupAnnotation(string aType)
  {
    PdfTextMarkupAnnotationType markupAnnotation = PdfTextMarkupAnnotationType.Highlight;
    switch (aType)
    {
      case "Highlight":
        markupAnnotation = PdfTextMarkupAnnotationType.Highlight;
        break;
      case "Squiggly":
        markupAnnotation = PdfTextMarkupAnnotationType.Squiggly;
        break;
      case "StrikeOut":
        markupAnnotation = PdfTextMarkupAnnotationType.StrikeOut;
        break;
      case "Underline":
        markupAnnotation = PdfTextMarkupAnnotationType.Underline;
        break;
    }
    return markupAnnotation;
  }

  private PdfColor ObtainTextMarkupColor()
  {
    PdfColorSpace colorSpace = PdfColorSpace.RGB;
    PdfColor textMarkupColor = PdfColor.Empty;
    PdfArray pdfArray = !this.Dictionary.ContainsKey("C") ? textMarkupColor.ToArray(colorSpace) : PdfCrossTable.Dereference(this.Dictionary["C"]) as PdfArray;
    if (pdfArray != null && pdfArray[0] is PdfNumber && pdfArray[1] is PdfNumber && pdfArray[2] is PdfNumber)
      textMarkupColor = new PdfColor((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue, (pdfArray[2] as PdfNumber).FloatValue);
    return textMarkupColor;
  }

  protected override void Save()
  {
    this.CheckFlatten();
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
    if (this.Flatten)
      return;
    this.m_borderDic.SetProperty("Type", (IPdfPrimitive) new PdfName("Border"));
    this.m_borderDic.SetNumber("W", this.Border.Width);
    this.Dictionary.SetProperty("BS", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_borderDic));
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
        if (isNormalMatrix && this.Page.Rotation != PdfPageRotateAngle.RotateAngle0)
        {
          this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
        }
        else
        {
          if (!isNormalMatrix || !this.IsValidTemplateMatrix(dictionary, this.Bounds.Location, appearance))
            return;
          this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
        }
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
    RectangleF rectangle = RectangleF.Empty;
    if (this.BoundsCollection.Count > 1)
    {
      PdfPath pdfPath = new PdfPath();
      for (int index = 0; index < this.BoundsCollection.Count; ++index)
        pdfPath.AddRectangle(this.BoundsCollection[index]);
      rectangle = pdfPath.GetBounds();
      this.Bounds = rectangle;
    }
    else if (this.Dictionary.ContainsKey("QuadPoints") && PdfCrossTable.Dereference(this.Dictionary["QuadPoints"]) is PdfArray pdfArray)
    {
      for (int index1 = 0; index1 < pdfArray.Count / 8; ++index1)
      {
        PointF[] linePoints = new PointF[pdfArray.Count / 2];
        int index2 = 0;
        int index3 = 0;
        while (index3 < pdfArray.Count)
        {
          float floatValue1 = (pdfArray[index3] as PdfNumber).FloatValue;
          float floatValue2 = (pdfArray[index3 + 1] as PdfNumber).FloatValue;
          linePoints[index2] = new PointF(floatValue1, floatValue2);
          index3 += 2;
          ++index2;
        }
        PdfPath pdfPath = new PdfPath();
        pdfPath.AddLines(linePoints);
        rectangle = pdfPath.GetBounds();
      }
    }
    PdfTemplate appearance = new PdfTemplate(new RectangleF(0.0f, 0.0f, rectangle.Width, rectangle.Height));
    this.SetMatrix((PdfDictionary) appearance.m_content);
    PdfGraphics graphics = appearance.Graphics;
    graphics.SetTransparency(this.Opacity, this.Opacity, PdfBlendMode.Multiply);
    if (this.BoundsCollection.Count > 1)
    {
      for (int index = 0; index < this.BoundsCollection.Count; ++index)
      {
        if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Highlight)
          graphics.DrawRectangle((PdfBrush) new PdfSolidBrush(this.TextMarkupColor), this.BoundsCollection[index].X - rectangle.X, this.BoundsCollection[index].Y - rectangle.Y, this.BoundsCollection[index].Width, this.BoundsCollection[index].Height);
        else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Underline)
          graphics.DrawLine(new PdfPen(this.TextMarkupColor), this.BoundsCollection[index].X - rectangle.X, (float) ((double) this.BoundsCollection[index].Y - (double) rectangle.Y + ((double) this.BoundsCollection[index].Height - (double) this.BoundsCollection[index].Height / 2.0 / 3.0)), this.BoundsCollection[index].Width + (this.BoundsCollection[index].X - rectangle.X), (float) ((double) this.BoundsCollection[index].Y - (double) rectangle.Y + ((double) this.BoundsCollection[index].Height - (double) this.BoundsCollection[index].Height / 2.0 / 3.0)));
        else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.StrikeOut)
          graphics.DrawLine(new PdfPen(this.TextMarkupColor), this.BoundsCollection[index].X - rectangle.X, (float) ((double) this.BoundsCollection[index].Y - (double) rectangle.Y + ((double) this.BoundsCollection[index].Height - (double) this.BoundsCollection[index].Height / 2.0)), this.BoundsCollection[index].Width + (this.BoundsCollection[index].X - rectangle.X), (float) ((double) this.BoundsCollection[index].Y - (double) rectangle.Y + ((double) this.BoundsCollection[index].Height - (double) this.BoundsCollection[index].Height / 2.0)));
        else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Squiggly)
        {
          PdfPen pen = new PdfPen(this.TextMarkupColor);
          pen.Width = this.Border.Width;
          graphics.Save();
          graphics.TranslateTransform(this.BoundsCollection[index].X - rectangle.X, this.BoundsCollection[index].Y - rectangle.Y);
          graphics.SetClip(new RectangleF(0.0f, 0.0f, this.BoundsCollection[index].Width, this.BoundsCollection[index].Height));
          graphics.DrawPath(pen, this.DrawSquiggly(this.BoundsCollection[index].Width, this.BoundsCollection[index].Height));
          graphics.Restore();
        }
      }
    }
    else
    {
      if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Highlight)
        graphics.DrawRectangle((PdfBrush) new PdfSolidBrush(this.TextMarkupColor), 0.0f, 0.0f, rectangle.Width, rectangle.Height);
      else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Underline)
        graphics.DrawLine(new PdfPen(this.TextMarkupColor), 0.0f, rectangle.Height - (float) ((double) rectangle.Height / 2.0 / 3.0), rectangle.Width, rectangle.Height - (float) ((double) rectangle.Height / 2.0 / 3.0));
      else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.StrikeOut)
        graphics.DrawLine(new PdfPen(this.TextMarkupColor), 0.0f, rectangle.Height / 2f, rectangle.Width, rectangle.Height / 2f);
      else if (this.TextMarkupAnnotationType == PdfTextMarkupAnnotationType.Squiggly)
        graphics.DrawPath(new PdfPen(this.TextMarkupColor)
        {
          Width = this.Border.Width
        }, this.DrawSquiggly(rectangle.Width, rectangle.Height));
      this.Dictionary["Rect"] = (IPdfPrimitive) PdfArray.FromRectangle(rectangle);
    }
    return appearance;
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
}
