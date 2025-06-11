// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedInkAnnotation
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

public class PdfLoadedInkAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private List<float> m_inkList;
  private List<List<float>> m_inkListcollection;
  private int[] m_dashArray;
  private int m_borderWidth = 1;
  private float m_borderLineWidth = 1f;
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

  public List<float> InkList
  {
    get => this.ObtainInkList();
    set
    {
      this.m_inkList = value;
      this.Dictionary.SetProperty(nameof (InkList), (IPdfPrimitive) new PdfArray(new PdfArray()
      {
        (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfArray(this.m_inkList.ToArray()))
      }));
      this.InkPointsCollection = (List<List<float>>) null;
    }
  }

  public List<List<float>> InkPointsCollection
  {
    get
    {
      if (this.m_inkListcollection == null)
        this.m_inkListcollection = this.ObtainInkListCollection();
      return this.m_inkListcollection;
    }
    set => this.m_inkListcollection = value;
  }

  public int BorderWidth
  {
    get => (int) this.ObtainBorderWidth();
    set
    {
      this.m_borderWidth = value;
      this.m_borderDic.SetProperty("W", (IPdfPrimitive) new PdfNumber(this.m_borderWidth));
    }
  }

  internal float BorderLineWidth
  {
    get => this.ObtainBorderWidth();
    set
    {
      this.m_borderLineWidth = value;
      this.m_borderDic.SetNumber("W", this.m_borderLineWidth);
    }
  }

  public PdfLineBorderStyle BorderStyle
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

  public int[] DashArray
  {
    get => this.ObtainDashArray();
    set
    {
      this.m_dashArray = value;
      this.m_borderDic.SetProperty("D", (IPdfPrimitive) new PdfArray(this.m_dashArray));
    }
  }

  internal PdfLoadedInkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle)
    : base(dictionary, crossTable)
  {
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
    if (!this.Dictionary.ContainsKey("BS"))
      return;
    this.m_borderDic = this.m_crossTable.GetObject(this.Dictionary["BS"]) as PdfDictionary;
  }

  private List<float> ObtainInkList()
  {
    List<float> inkList = new List<float>();
    if (this.Dictionary.ContainsKey("InkList") && this.m_crossTable.GetObject((this.m_crossTable.GetObject(this.Dictionary["InkList"]) as PdfArray)[0]) is PdfArray pdfArray)
    {
      foreach (PdfNumber pdfNumber in pdfArray)
        inkList.Add(pdfNumber.FloatValue);
    }
    return inkList;
  }

  private List<List<float>> ObtainInkListCollection()
  {
    List<List<float>> inkListCollection = new List<List<float>>();
    if (this.Dictionary.ContainsKey("InkList"))
    {
      PdfArray pdfArray = this.m_crossTable.GetObject(this.Dictionary["InkList"]) as PdfArray;
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        PdfArray pointer = this.m_crossTable.GetObject(pdfArray[index]) as PdfArray;
        List<float> floatList = new List<float>();
        foreach (PdfNumber pdfNumber in this.m_crossTable.GetObject((IPdfPrimitive) pointer) as PdfArray)
          floatList.Add(pdfNumber.FloatValue);
        inkListCollection.Add(floatList);
      }
    }
    return inkListCollection;
  }

  private float ObtainBorderWidth()
  {
    float borderWidth = 1f;
    if (this.Dictionary.ContainsKey("Border"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Border"]) is PdfArray pdfArray && pdfArray.Count >= 2 && pdfArray[2] is PdfNumber)
        borderWidth = (pdfArray[2] as PdfNumber).FloatValue;
    }
    else if (this.Dictionary.ContainsKey("BS"))
    {
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["BS"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("W"))
        borderWidth = (pdfDictionary["W"] as PdfNumber).FloatValue;
    }
    return borderWidth;
  }

  private int[] ObtainDashArray()
  {
    List<int> intList = new List<int>();
    if (this.Dictionary.ContainsKey("BS"))
    {
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["BS"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("D"))
      {
        PdfArray pdfArray = this.m_crossTable.GetObject(pdfDictionary["D"]) as PdfArray;
        for (int index = 0; index < pdfArray.Count; ++index)
          intList.Add((pdfArray[index] as PdfNumber).IntValue);
      }
    }
    return intList.ToArray();
  }

  private void GetBoundsValue()
  {
    if (this.InkPointsCollection == null || this.m_inkListcollection.Count <= 0)
      return;
    List<float> floatList = new List<float>();
    for (int index1 = 0; index1 < this.m_inkListcollection.Count; ++index1)
    {
      if (this.m_inkListcollection[index1] != null && this.m_inkListcollection[index1].Count % 2 == 0)
      {
        for (int index2 = 0; index2 < this.m_inkListcollection[index1].Count; ++index2)
          floatList.Add(this.m_inkListcollection[index1][index2]);
      }
    }
    PointF[] points = new PointF[floatList.Count / 2];
    int index3 = 0;
    for (int index4 = 0; index4 < floatList.Count; index4 += 2)
    {
      points[index3] = new PointF(floatList[index4], floatList[index4 + 1]);
      ++index3;
    }
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddCurve(points, 1f);
    this.Bounds = graphicsPath.GetBounds();
    this.Bounds = new RectangleF(this.Bounds.X - (float) this.BorderWidth, this.Bounds.Y - (float) this.BorderWidth, this.Bounds.Width + (float) (2 * this.BorderWidth), this.Bounds.Height + (float) (2 * this.BorderWidth));
  }

  protected override void Save()
  {
    this.CheckFlatten();
    this.AddInkPoints();
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
    if (!this.FlattenPopUps)
      return;
    this.FlattenLoadedPopup();
  }

  private PdfTemplate CreateAppearance()
  {
    if (!this.SetAppearanceDictionary)
      return (PdfTemplate) null;
    this.GetBoundsValue();
    RectangleF rectangleF = new RectangleF(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
    PdfTemplate appearance = new PdfTemplate(rectangleF);
    this.SetMatrix((PdfDictionary) appearance.m_content);
    appearance.m_writeTransformation = false;
    PdfGraphics graphics = appearance.Graphics;
    if (this.m_inkListcollection != null && this.m_inkListcollection.Count > 0)
    {
      for (int index1 = 0; index1 < this.m_inkListcollection.Count; ++index1)
      {
        if (this.m_inkListcollection[index1].Count % 2 == 0)
        {
          float[] numArray = this.m_inkListcollection[index1].ToArray();
          if (numArray.Length == 2)
            numArray = new float[4]
            {
              numArray[0] - 0.5f,
              numArray[1] - 0.5f,
              numArray[0] + 0.5f,
              numArray[1] + 0.5f
            };
          PointF[] points1 = new PointF[numArray.Length / 2];
          int index2 = 0;
          for (int index3 = 0; index3 < numArray.Length; index3 += 2)
          {
            points1[index2] = new PointF(numArray[index3], numArray[index3 + 1]);
            ++index2;
          }
          PointF[] pointFArray = new PointF[index2 + index2 * 2 - 2];
          GraphicsPath graphicsPath = new GraphicsPath();
          if (points1.Length == 2)
            graphicsPath.AddEllipse(points1[0].X + 0.5f, points1[0].Y + 0.5f, points1[1].X - points1[0].X, points1[1].Y - points1[0].Y);
          else
            graphicsPath.AddCurve(points1, 1f);
          PointF[] pathPoints = graphicsPath.PathPoints;
          if (pathPoints != null)
          {
            PointF[] points2 = pathPoints;
            for (int index4 = 0; index4 < points2.Length; ++index4)
            {
              PointF pointF = points2[index4];
              points2[index4] = new PointF(pointF.X, -pointF.Y);
            }
            PdfPath path = new PdfPath(points2, graphicsPath.PathTypes);
            if ((double) this.Opacity < 1.0)
            {
              PdfGraphicsState state = graphics.Save();
              graphics.SetTransparency(this.Opacity);
              graphics.DrawPath(new PdfPen(this.Color, this.BorderLineWidth), path);
              graphics.Restore(state);
            }
            else
              graphics.DrawPath(new PdfPen(this.Color, this.BorderLineWidth), path);
          }
        }
      }
      this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(rectangleF));
    }
    return appearance;
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
        if (!this.Dictionary.ContainsKey("Matrix"))
          this.SetMatrix((PdfDictionary) appearance.m_content);
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

  private void AddInkPoints()
  {
    if (this.m_inkList == null && this.m_inkListcollection == null)
      return;
    if (this.m_inkList != null && this.m_inkListcollection != null)
    {
      if (this.InkPointsCollection[0] != this.m_inkList)
        this.InkPointsCollection.Insert(0, this.m_inkList);
    }
    else if (this.m_inkList != null && this.m_inkListcollection == null)
      this.InkPointsCollection = new List<List<float>>()
      {
        this.m_inkList
      };
    List<PdfArray> array = new List<PdfArray>();
    for (int index = 0; index < this.m_inkListcollection.Count; ++index)
    {
      PdfArray pdfArray = new PdfArray(this.m_inkListcollection[index].ToArray());
      array.Add(pdfArray);
    }
    this.Dictionary.SetProperty("InkList", (IPdfPrimitive) new PdfArray(array));
  }

  private void GetControlPoints(
    PointF[] pointCollection,
    out PointF[] controlPointOne,
    out PointF[] controlPointTwo)
  {
    if (pointCollection == null)
      throw new ArgumentNullException(nameof (pointCollection));
    int length = pointCollection.Length - 1;
    if (length < 1)
      throw new ArgumentException("At least two knot PointFs required", nameof (pointCollection));
    if (length == 1)
    {
      controlPointOne = new PointF[1];
      controlPointOne[0].X = (float) ((2.0 * (double) pointCollection[0].X + (double) pointCollection[1].X) / 3.0);
      controlPointOne[0].Y = (float) ((2.0 * (double) pointCollection[0].Y + (double) pointCollection[1].Y) / 3.0);
      controlPointTwo = new PointF[1];
      controlPointTwo[0].X = 2f * controlPointOne[0].X - pointCollection[0].X;
      controlPointTwo[0].Y = 2f * controlPointOne[0].Y - pointCollection[0].Y;
    }
    else
    {
      double[] rightVector = new double[length];
      for (int index = 1; index < length - 1; ++index)
        rightVector[index] = 4.0 * (double) pointCollection[index].X + 2.0 * (double) pointCollection[index + 1].X;
      rightVector[0] = (double) pointCollection[0].X + 2.0 * (double) pointCollection[1].X;
      rightVector[length - 1] = (8.0 * (double) pointCollection[length - 1].X + (double) pointCollection[length].X) / 2.0;
      double[] singleControlPoint1 = this.GetSingleControlPoint(rightVector);
      for (int index = 1; index < length - 1; ++index)
        rightVector[index] = 4.0 * (double) pointCollection[index].Y + 2.0 * (double) pointCollection[index + 1].Y;
      rightVector[0] = (double) pointCollection[0].Y + 2.0 * (double) pointCollection[1].Y;
      rightVector[length - 1] = (8.0 * (double) pointCollection[length - 1].Y + (double) pointCollection[length].Y) / 2.0;
      double[] singleControlPoint2 = this.GetSingleControlPoint(rightVector);
      controlPointOne = new PointF[length];
      controlPointTwo = new PointF[length];
      for (int index = 0; index < length; ++index)
      {
        controlPointOne[index] = new PointF(Convert.ToSingle(singleControlPoint1[index]), Convert.ToSingle(singleControlPoint2[index]));
        controlPointTwo[index] = index >= length - 1 ? new PointF(Convert.ToSingle(((double) pointCollection[length].X + singleControlPoint1[length - 1]) / 2.0), Convert.ToSingle(((double) pointCollection[length].Y + singleControlPoint2[length - 1]) / 2.0)) : new PointF(Convert.ToSingle(2.0 * (double) pointCollection[index + 1].X - singleControlPoint1[index + 1]), Convert.ToSingle(2.0 * (double) pointCollection[index + 1].Y - singleControlPoint2[index + 1]));
      }
    }
  }

  private double[] GetSingleControlPoint(double[] rightVector)
  {
    int length = rightVector.Length;
    double[] singleControlPoint = new double[length];
    double[] numArray = new double[length];
    double num = 2.0;
    singleControlPoint[0] = rightVector[0] / num;
    for (int index = 1; index < length; ++index)
    {
      numArray[index] = 1.0 / num;
      num = (index < length - 1 ? 4.0 : 3.5) - numArray[index];
      singleControlPoint[index] = (rightVector[index] - singleControlPoint[index - 1]) / num;
    }
    for (int index = 1; index < length; ++index)
      singleControlPoint[length - index - 1] -= numArray[length - index] * singleControlPoint[length - index];
    return singleControlPoint;
  }
}
