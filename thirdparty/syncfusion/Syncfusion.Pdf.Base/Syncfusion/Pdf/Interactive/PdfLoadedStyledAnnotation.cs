// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedStyledAnnotation
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
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedStyledAnnotation : PdfLoadedAnnotation
{
  private PdfSound m_sound;
  private PdfDictionary m_dictionary;
  private PdfCrossTable m_crossTable;
  private PdfColor m_color;
  private string m_text;
  private string m_name = string.Empty;
  private string m_author;
  private string m_subject;
  private DateTime m_modifiedDate;
  private DateTime m_creationDate;
  private PdfColor m_innerColor;
  private float m_opacity = 1f;
  internal bool isBounds;
  private PdfAnnotationBorder m_border;
  private bool m_isCreationDateObtained;
  internal bool isOpacity;
  private bool m_isContainBorder;
  internal bool isContentUpdated;
  internal PdfLoadedPopupAnnotationCollection m_reviewHistory;
  internal PdfLoadedPopupAnnotationCollection m_comments;

  public override PdfColor Color
  {
    get => this.ObtainColor();
    set
    {
      base.Color = value;
      this.m_color = value;
    }
  }

  public override float Opacity
  {
    get => this.ObtainOpacity();
    set
    {
      this.m_opacity = (double) value >= 0.0 && (double) value <= 1.0 ? value : throw new ArgumentException("Valid value should be between 0 to 1.");
      this.Dictionary.SetNumber("CA", this.m_opacity);
      this.isOpacity = true;
    }
  }

  public override PdfColor InnerColor
  {
    get => this.ObtainInnerColor();
    set
    {
      this.m_innerColor = value;
      if (this.m_innerColor.A != (byte) 0)
      {
        this.Dictionary.SetProperty("IC", (IPdfPrimitive) this.m_innerColor.ToArray());
      }
      else
      {
        if (!this.Dictionary.ContainsKey("IC"))
          return;
        this.Dictionary.Remove("IC");
      }
    }
  }

  public override string Text
  {
    get => string.IsNullOrEmpty(this.m_text) ? this.ObtainText() : this.m_text;
    set
    {
      base.Text = value;
      this.m_text = value;
      this.isContentUpdated = true;
    }
  }

  public override string Author
  {
    get
    {
      this.m_author = this.ObtainAuthor();
      return this.m_author;
    }
    set
    {
      this.m_author = value;
      this.Dictionary.SetString("T", this.m_author);
    }
  }

  public override string Subject
  {
    get
    {
      this.m_subject = this.ObtainSubject();
      return this.m_subject;
    }
    set
    {
      this.m_subject = value;
      this.Dictionary.SetString("Subj", this.m_subject);
    }
  }

  public override DateTime ModifiedDate
  {
    get => this.ObtainModifiedDate();
    set
    {
      this.m_modifiedDate = value;
      this.Dictionary.SetDateTime("M", this.m_modifiedDate);
    }
  }

  public DateTime CreationDate
  {
    get => !this.m_isCreationDateObtained ? this.ObtainCreationDate() : this.m_creationDate;
  }

  public new string Name
  {
    get
    {
      if (this.m_name == string.Empty && this.Dictionary.ContainsKey("NM"))
        this.m_name = (this.Dictionary["NM"] as PdfString).Value;
      return this.m_name;
    }
    set
    {
      if (value == null || value == string.Empty)
        throw new ArgumentNullException("Name value cannot be null or Empty");
      if (!(this.m_name != value))
        return;
      this.m_name = value;
      this.Dictionary["NM"] = (IPdfPrimitive) new PdfString(this.m_name);
    }
  }

  public override RectangleF Bounds
  {
    get
    {
      RectangleF bounds = this.GetBounds(this.Dictionary, this.CrossTable);
      if (this.Page != null && this.Page.Dictionary.ContainsKey("CropBox"))
      {
        PdfArray pdfArray = !(this.Page.Dictionary["CropBox"] is PdfArray) ? (this.Page.Dictionary["CropBox"] as PdfReferenceHolder).Object as PdfArray : this.Page.Dictionary["CropBox"] as PdfArray;
        if (((double) (pdfArray[0] as PdfNumber).FloatValue != 0.0 || (double) (pdfArray[1] as PdfNumber).FloatValue != 0.0 || (double) this.Page.Size.Width == (double) (pdfArray[2] as PdfNumber).FloatValue || (double) this.Page.Size.Height == (double) (pdfArray[3] as PdfNumber).FloatValue) && (double) bounds.X != (double) (pdfArray[0] as PdfNumber).FloatValue)
        {
          bounds.X -= (pdfArray[0] as PdfNumber).FloatValue;
          bounds.Y = (pdfArray[3] as PdfNumber).FloatValue - (bounds.Y + bounds.Height);
        }
        else
          bounds.Y = this.Page.Size.Height - (bounds.Y + bounds.Height);
      }
      else if (this.Page != null && this.Page.Dictionary.ContainsKey("MediaBox"))
      {
        PdfArray pdfArray = (PdfArray) null;
        if (PdfCrossTable.Dereference(this.Page.Dictionary["MediaBox"]) is PdfArray)
          pdfArray = PdfCrossTable.Dereference(this.Page.Dictionary["MediaBox"]) as PdfArray;
        if ((double) (pdfArray[0] as PdfNumber).FloatValue > 0.0 || (double) (pdfArray[1] as PdfNumber).FloatValue > 0.0 || (double) this.Page.Size.Width == (double) (pdfArray[2] as PdfNumber).FloatValue || (double) this.Page.Size.Height == (double) (pdfArray[3] as PdfNumber).FloatValue)
        {
          bounds.X -= (pdfArray[0] as PdfNumber).FloatValue;
          bounds.Y = (pdfArray[3] as PdfNumber).FloatValue - (bounds.Y + bounds.Height);
        }
        else
          bounds.Y = this.Page.Size.Height - (bounds.Y + bounds.Height);
      }
      else if (this.Page != null)
        bounds.Y = this.Page.Size.Height - (bounds.Y + bounds.Height);
      else
        bounds.Y += bounds.Height;
      return bounds;
    }
    set
    {
      RectangleF rectangleF = value;
      this.isBounds = true;
      if (rectangleF == RectangleF.Empty)
        throw new ArgumentNullException("rectangle");
      float height = this.Page.Size.Height;
      PdfNumber[] pdfNumberArray = new PdfNumber[4]
      {
        new PdfNumber(rectangleF.X),
        new PdfNumber(height - (rectangleF.Y + rectangleF.Height)),
        new PdfNumber(rectangleF.X + rectangleF.Width),
        new PdfNumber(height - rectangleF.Y)
      };
      PdfDictionary pdfDictionary = this.Dictionary;
      if (!pdfDictionary.ContainsKey("Rect"))
        pdfDictionary = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      pdfDictionary.SetArray("Rect", (IPdfPrimitive[]) pdfNumberArray);
      this.Changed = true;
    }
  }

  public override PdfAnnotationBorder Border
  {
    get
    {
      if (this.m_border == null)
      {
        this.m_border = this.ObtainBorder();
        this.Dictionary.SetProperty(nameof (Border), (IPdfWrapper) this.m_border);
      }
      return this.m_border;
    }
    set
    {
      base.Border = value;
      this.Changed = true;
    }
  }

  public override PointF Location
  {
    get => this.Bounds.Location;
    set => this.Bounds = new RectangleF(value, this.Bounds.Size);
  }

  public override SizeF Size
  {
    get => this.Bounds.Size;
    set => this.Bounds = new RectangleF(this.Bounds.Location, value);
  }

  public override PdfAnnotationFlags AnnotationFlags
  {
    get => this.ObtainAnnotationFlags();
    set
    {
      base.AnnotationFlags = value;
      this.Changed = true;
    }
  }

  internal bool IsContainBorder
  {
    get
    {
      if (this.Dictionary.ContainsKey("Border") || this.Dictionary.ContainsKey("BS"))
        this.m_isContainBorder = true;
      return this.m_isContainBorder;
    }
  }

  internal PdfAnnotationState ObtainState()
  {
    PdfAnnotationState state = PdfAnnotationState.None;
    if (this.Dictionary.ContainsKey("State") && this.Dictionary["State"] is PdfString pdfString)
    {
      switch (pdfString.Value)
      {
        case "None":
          state = PdfAnnotationState.None;
          break;
        case "Accepted":
          state = PdfAnnotationState.Accepted;
          break;
        case "Cancelled":
          state = PdfAnnotationState.Cancelled;
          break;
        case "Completed":
          state = PdfAnnotationState.Completed;
          break;
        case "Rejected":
          state = PdfAnnotationState.Rejected;
          break;
        case "Marked":
          state = PdfAnnotationState.Marked;
          break;
        case "Unmarked":
          state = PdfAnnotationState.Unmarked;
          break;
        default:
          state = PdfAnnotationState.Unknown;
          break;
      }
    }
    return state;
  }

  internal PdfAnnotationStateModel ObtainStateModel()
  {
    PdfAnnotationStateModel stateModel = PdfAnnotationStateModel.None;
    if (this.Dictionary.ContainsKey("StateModel") && this.Dictionary["StateModel"] is PdfString pdfString)
    {
      switch (pdfString.Value)
      {
        case "Review":
          stateModel = PdfAnnotationStateModel.Review;
          break;
        case "Marked":
          stateModel = PdfAnnotationStateModel.Marked;
          break;
      }
    }
    return stateModel;
  }

  private string ObtainText()
  {
    if (this is PdfLoadedFreeTextAnnotation)
    {
      this.m_text = (this as PdfLoadedFreeTextAnnotation).MarkUpText;
      return this.m_text;
    }
    if (this.Dictionary.ContainsKey("Contents"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Contents"]) is PdfString pdfString)
        this.m_text = pdfString.Value.ToString();
      return this.m_text;
    }
    if (!(this is PdfLoadedWidgetAnnotation) || !this.Dictionary.ContainsKey("V"))
      return string.Empty;
    if (PdfCrossTable.Dereference(this.Dictionary["V"]) is PdfString pdfString1)
      this.m_text = pdfString1.Value.ToString();
    return this.m_text;
  }

  private string ObtainAuthor()
  {
    string author = (string) null;
    if (this.Dictionary.ContainsKey("Author"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Author"]) is PdfString pdfString1)
        author = pdfString1.Value;
    }
    else if (this.Dictionary.ContainsKey("T") && PdfCrossTable.Dereference(this.Dictionary["T"]) is PdfString pdfString2)
      author = pdfString2.Value;
    return author;
  }

  private string ObtainSubject()
  {
    string subject = (string) null;
    if (this.Dictionary.ContainsKey("Subject"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Subject"]) is PdfString pdfString1)
        subject = pdfString1.Value;
    }
    else if (this.Dictionary.ContainsKey("Subj") && PdfCrossTable.Dereference(this.Dictionary["Subj"]) is PdfString pdfString2)
      subject = pdfString2.Value;
    return subject;
  }

  private DateTime ObtainModifiedDate()
  {
    if (this.Dictionary.ContainsKey("ModDate") || this.Dictionary.ContainsKey("M"))
    {
      if (!(PdfCrossTable.Dereference(this.Dictionary["ModDate"]) is PdfString dateTimeStringValue))
        dateTimeStringValue = PdfCrossTable.Dereference(this.Dictionary["M"]) as PdfString;
      this.m_modifiedDate = this.Dictionary.GetDateTime(dateTimeStringValue);
    }
    return this.m_modifiedDate;
  }

  private DateTime ObtainCreationDate()
  {
    this.m_isCreationDateObtained = true;
    if (this.Dictionary.ContainsKey("CreationDate") && PdfCrossTable.Dereference(this.Dictionary["CreationDate"]) is PdfString dateTimeStringValue)
      this.m_creationDate = this.Dictionary.GetDateTime(dateTimeStringValue);
    return this.m_creationDate;
  }

  internal RectangleF GetBounds(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfArray pdfArray = (PdfArray) null;
    if (dictionary.ContainsKey("Kids"))
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(dictionary, crossTable);
      if (widgetAnnotation.ContainsKey("Rect"))
        pdfArray = crossTable.GetObject(widgetAnnotation["Rect"]) as PdfArray;
    }
    else if (dictionary.ContainsKey("Rect"))
      pdfArray = crossTable.GetObject(dictionary["Rect"]) as PdfArray;
    return pdfArray.ToRectangle();
  }

  private PdfAnnotationBorder ObtainBorder()
  {
    if (this.Dictionary.ContainsKey("Border"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Border"]) is PdfArray pdfArray && pdfArray.Count > 0 && pdfArray[0] is PdfNumber && pdfArray[1] is PdfNumber && pdfArray[2] is PdfNumber)
      {
        float floatValue1 = (pdfArray[0] as PdfNumber).FloatValue;
        float floatValue2 = (pdfArray[1] as PdfNumber).FloatValue;
        float floatValue3 = (pdfArray[2] as PdfNumber).FloatValue;
        this.m_border = new PdfAnnotationBorder(floatValue1, floatValue2, floatValue3);
        this.m_border.Width = floatValue3;
        this.m_border.HorizontalRadius = floatValue1;
        this.m_border.VerticalRadius = floatValue2;
      }
    }
    else if (this.Dictionary.ContainsKey("BS"))
    {
      if (this.Dictionary["BS"] as PdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfReferenceHolder pdfReferenceHolder = this.Dictionary["BS"] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
        {
          PdfDictionary pdfDictionary = pdfReferenceHolder.Object as PdfDictionary;
          if (pdfDictionary.ContainsKey("W"))
          {
            float floatValue = (pdfDictionary["W"] as PdfNumber).FloatValue;
            this.m_border = new PdfAnnotationBorder();
            this.m_border.Width = floatValue;
            this.m_border.HorizontalRadius = floatValue;
            this.m_border.VerticalRadius = floatValue;
          }
        }
      }
      else if (this.Dictionary["BS"] is PdfDictionary && this.Dictionary["BS"] is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("W"))
      {
        float floatValue = (pdfDictionary1["W"] as PdfNumber).FloatValue;
        this.m_border = new PdfAnnotationBorder();
        this.m_border.Width = floatValue;
        this.m_border.HorizontalRadius = floatValue;
        this.m_border.VerticalRadius = floatValue;
      }
    }
    else
      this.m_border = new PdfAnnotationBorder();
    if (this.m_border == null)
      this.m_border = new PdfAnnotationBorder();
    return this.m_border;
  }

  private PdfColor ObtainColor()
  {
    PdfColor color = new PdfColor(System.Drawing.Color.Empty);
    PdfArray pdfArray = (PdfArray) null;
    if (this.Dictionary.ContainsKey("C"))
      pdfArray = this.Dictionary["C"] as PdfArray;
    if (pdfArray != null && pdfArray.Elements.Count == 1)
    {
      if (this.CrossTable.GetObject(pdfArray[0]) is PdfNumber pdfNumber1)
        color = new PdfColor(pdfNumber1.FloatValue);
    }
    else if (pdfArray != null && pdfArray.Elements.Count == 3)
    {
      PdfNumber pdfNumber2 = pdfArray[0] as PdfNumber;
      PdfNumber pdfNumber3 = pdfArray[1] as PdfNumber;
      PdfNumber pdfNumber4 = pdfArray[2] as PdfNumber;
      if (pdfNumber2 != null && pdfNumber3 != null && pdfNumber4 != null)
        color = new PdfColor((byte) Math.Round((double) pdfNumber2.FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) pdfNumber3.FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) pdfNumber4.FloatValue * (double) byte.MaxValue));
    }
    else if (pdfArray != null && pdfArray.Elements.Count == 4)
    {
      PdfNumber pdfNumber5 = this.CrossTable.GetObject(pdfArray[0]) as PdfNumber;
      PdfNumber pdfNumber6 = this.CrossTable.GetObject(pdfArray[1]) as PdfNumber;
      PdfNumber pdfNumber7 = this.CrossTable.GetObject(pdfArray[2]) as PdfNumber;
      PdfNumber pdfNumber8 = this.CrossTable.GetObject(pdfArray[3]) as PdfNumber;
      if (pdfNumber5 != null && pdfNumber6 != null && pdfNumber7 != null && pdfNumber8 != null)
        color = new PdfColor(pdfNumber5.FloatValue, pdfNumber6.FloatValue, pdfNumber7.FloatValue, pdfNumber8.FloatValue);
    }
    return color;
  }

  private float ObtainOpacity()
  {
    if (this.Dictionary.ContainsKey("CA"))
      this.m_opacity = this.GetNumber("CA");
    return this.m_opacity;
  }

  private float GetNumber(string keyName)
  {
    float number = 0.0f;
    if (this.m_dictionary[keyName] is PdfNumber pdfNumber)
      number = pdfNumber.FloatValue;
    return number;
  }

  private PdfColor ObtainInnerColor()
  {
    PdfColor innerColor = PdfColor.Empty;
    if (this.Dictionary.ContainsKey("IC") && PdfCrossTable.Dereference(this.Dictionary["IC"]) is PdfArray pdfArray && pdfArray.Count > 0)
      innerColor = new PdfColor((byte) Math.Round((double) (pdfArray[0] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (pdfArray[1] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (pdfArray[2] as PdfNumber).FloatValue * (double) byte.MaxValue));
    return innerColor;
  }

  private PdfAnnotationFlags ObtainAnnotationFlags()
  {
    PdfAnnotationFlags annotationFlags = PdfAnnotationFlags.Default;
    if (this.Dictionary.ContainsKey("F"))
      annotationFlags = (PdfAnnotationFlags) (PdfLoadedAnnotation.GetValue(this.Dictionary, this.m_crossTable, "F", false) as PdfNumber).IntValue;
    return annotationFlags;
  }

  private bool IsClockWise(PointF[] points)
  {
    double num = 0.0;
    for (int index = 0; index < points.Length; ++index)
    {
      PointF point1 = points[index];
      PointF point2 = points[(index + 1) % points.Length];
      num += ((double) point2.X - (double) point1.X) * ((double) point2.Y + (double) point1.Y);
    }
    return num > 0.0;
  }

  private PointF GetIntersectionDegrees(PointF point1, PointF point2, float radius)
  {
    float x = point2.X - point1.X;
    float y = point2.Y - point1.Y;
    float d = 0.5f * (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y) / radius;
    if ((double) d < -1.0)
      d = -1f;
    if ((double) d > 1.0)
      d = 1f;
    float num1 = (float) Math.Atan2((double) y, (double) x);
    float num2 = (float) Math.Acos((double) d);
    return new PointF((float) (((double) num1 - (double) num2) * (180.0 / Math.PI)), (float) ((Math.PI + (double) num1 + (double) num2) * (180.0 / Math.PI)));
  }

  internal void DrawCloudStyle(
    PdfGraphics graphics,
    PdfBrush brush,
    PdfPen pen,
    float radius,
    float overlap,
    PointF[] points,
    bool isAppearance)
  {
    if (this.IsClockWise(points))
    {
      PointF[] pointFArray = new PointF[points.Length];
      int index1 = points.Length - 1;
      int index2 = 0;
      while (index1 >= 0)
      {
        pointFArray[index2] = points[index1];
        --index1;
        ++index2;
      }
      points = pointFArray;
    }
    List<CloudStyleArc> cloudStyleArcList = new List<CloudStyleArc>();
    float num1 = 2f * radius * overlap;
    PointF pointF = points[points.Length - 1];
    for (int index = 0; index < points.Length; ++index)
    {
      PointF point = points[index];
      float num2 = point.X - pointF.X;
      float num3 = point.Y - pointF.Y;
      float num4 = (float) Math.Sqrt((double) num2 * (double) num2 + (double) num3 * (double) num3);
      float num5 = num2 / num4;
      float num6 = num3 / num4;
      float num7 = num1;
      for (float num8 = 0.0f; (double) num8 + 0.1 * (double) num7 < (double) num4; num8 += num7)
        cloudStyleArcList.Add(new CloudStyleArc()
        {
          point = new PointF(pointF.X + num8 * num5, pointF.Y + num8 * num6)
        });
      pointF = point;
    }
    new GraphicsPath().AddPolygon(points);
    CloudStyleArc cloudStyleArc1 = cloudStyleArcList[cloudStyleArcList.Count - 1];
    for (int index = 0; index < cloudStyleArcList.Count; ++index)
    {
      CloudStyleArc cloudStyleArc2 = cloudStyleArcList[index];
      PointF intersectionDegrees = this.GetIntersectionDegrees(cloudStyleArc1.point, cloudStyleArc2.point, radius);
      cloudStyleArc1.endAngle = intersectionDegrees.X;
      cloudStyleArc2.startAngle = intersectionDegrees.Y;
      cloudStyleArc1 = cloudStyleArc2;
    }
    GraphicsPath graphicsPath1 = new GraphicsPath();
    for (int index = 0; index < cloudStyleArcList.Count; ++index)
    {
      CloudStyleArc cloudStyleArc3 = cloudStyleArcList[index];
      float startAngle = cloudStyleArc3.startAngle % 360f;
      float num9 = cloudStyleArc3.endAngle % 360f;
      float sweepAngle = 0.0f;
      if ((double) startAngle > 0.0 && (double) num9 < 0.0)
        sweepAngle = (float) (180.0 - (double) startAngle + (180.0 - ((double) num9 < 0.0 ? -(double) num9 : (double) num9)));
      else if ((double) startAngle < 0.0 && (double) num9 > 0.0)
        sweepAngle = -startAngle + num9;
      else if ((double) startAngle > 0.0 && (double) num9 > 0.0)
        sweepAngle = (double) startAngle <= (double) num9 ? num9 - startAngle : 360f - (startAngle - num9);
      else if ((double) startAngle < 0.0 && (double) num9 < 0.0)
        sweepAngle = (double) startAngle <= (double) num9 ? (float) -((double) startAngle + -(double) num9) : 360f - (startAngle - num9);
      if ((double) sweepAngle < 0.0)
        sweepAngle = -sweepAngle;
      cloudStyleArc3.endAngle = sweepAngle;
      graphicsPath1.AddArc(new RectangleF(cloudStyleArc3.point.X - radius, cloudStyleArc3.point.Y - radius, 2f * radius, 2f * radius), startAngle, sweepAngle);
    }
    graphicsPath1.CloseFigure();
    PointF[] points1 = new PointF[graphicsPath1.PathPoints.Length];
    if (isAppearance)
    {
      for (int index = 0; index < graphicsPath1.PathPoints.Length; ++index)
        points1[index] = new PointF(graphicsPath1.PathPoints[index].X, -graphicsPath1.PathPoints[index].Y);
    }
    PdfPath path1 = !isAppearance ? new PdfPath(graphicsPath1.PathPoints, graphicsPath1.PathTypes) : new PdfPath(points1, graphicsPath1.PathTypes);
    if (brush != null)
      graphics.DrawPath(brush, path1);
    float num10 = 19.0985928f;
    GraphicsPath graphicsPath2 = new GraphicsPath();
    for (int index = 0; index < cloudStyleArcList.Count; ++index)
    {
      CloudStyleArc cloudStyleArc4 = cloudStyleArcList[index];
      graphicsPath2.AddArc(new RectangleF(cloudStyleArc4.point.X - radius, cloudStyleArc4.point.Y - radius, 2f * radius, 2f * radius), cloudStyleArc4.startAngle, cloudStyleArc4.endAngle + num10);
    }
    graphicsPath2.CloseFigure();
    PointF[] points2 = new PointF[graphicsPath2.PathPoints.Length];
    if (isAppearance)
    {
      for (int index = 0; index < graphicsPath2.PathPoints.Length; ++index)
        points2[index] = new PointF(graphicsPath2.PathPoints[index].X, -graphicsPath2.PathPoints[index].Y);
    }
    PdfPath path2 = !isAppearance ? new PdfPath(graphicsPath2.PathPoints, graphicsPath2.PathTypes) : new PdfPath(points2, graphicsPath2.PathTypes);
    graphics.DrawPath(pen, path2);
  }

  protected new void CheckFlatten()
  {
    PdfPageBase page = (PdfPageBase) this.Page;
    if (this.Page.Annotations.Count <= 0 || !page.Annotations.Flatten)
      return;
    this.Page.Annotations.Flatten = page.Annotations.Flatten;
  }

  protected void FlattenAnnotationTemplate(PdfTemplate appearance, bool isNormalMatrix)
  {
    PdfGraphics graphics = this.Page.Graphics;
    PdfGraphics pdfGraphics = this.ObtainlayerGraphics();
    if (pdfGraphics != null)
      graphics = pdfGraphics;
    PdfGraphicsState state = graphics.Save();
    if (!isNormalMatrix)
    {
      appearance.IsAnnotationTemplate = true;
      appearance.NeedScaling = true;
    }
    if ((double) this.Opacity < 1.0)
      graphics.SetTransparency(this.Opacity);
    RectangleF templateBounds = this.CalculateTemplateBounds(this.Bounds, (PdfPageBase) this.Page, appearance, isNormalMatrix, graphics);
    graphics.DrawPdfTemplate(appearance, templateBounds.Location, templateBounds.Size);
    graphics.Restore(state);
    this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
  }

  internal PdfGraphics ObtainlayerGraphics()
  {
    PdfGraphics pdfGraphics = (PdfGraphics) null;
    if (this.Layer != null)
    {
      PdfLayer layer = this.Layer;
      if (layer.LayerId == null)
        layer.LayerId = "OCG_" + Guid.NewGuid().ToString();
      pdfGraphics = layer.CreateGraphics((PdfPageBase) this.Page);
    }
    return pdfGraphics;
  }

  internal bool IsValidTemplateMatrix(
    PdfDictionary dictionary,
    PointF bounds,
    PdfTemplate template)
  {
    bool flag = true;
    PointF location = bounds;
    if (dictionary.ContainsKey("Matrix"))
    {
      PdfArray pdfArray1 = PdfCrossTable.Dereference(dictionary["BBox"]) as PdfArray;
      if (PdfCrossTable.Dereference(dictionary["Matrix"]) is PdfArray pdfArray2 && pdfArray1 != null && pdfArray2.Count > 3 && pdfArray1.Count > 2 && (double) (pdfArray2[0] as PdfNumber).FloatValue == 1.0 && (double) (pdfArray2[1] as PdfNumber).FloatValue == 0.0 && (double) (pdfArray2[2] as PdfNumber).FloatValue == 0.0 && (double) (pdfArray2[3] as PdfNumber).FloatValue == 1.0 && ((double) (pdfArray1[0] as PdfNumber).FloatValue != -(double) (pdfArray2[4] as PdfNumber).FloatValue && (double) (pdfArray1[1] as PdfNumber).FloatValue != -(double) (pdfArray2[5] as PdfNumber).FloatValue || (double) (pdfArray1[0] as PdfNumber).FloatValue == 0.0 && -(double) (pdfArray2[4] as PdfNumber).FloatValue == 0.0))
      {
        PdfGraphics pdfGraphics1 = this.Page.Graphics;
        PdfGraphics pdfGraphics2 = this.ObtainlayerGraphics();
        if (pdfGraphics2 != null)
          pdfGraphics1 = pdfGraphics2;
        PdfGraphicsState state = pdfGraphics1.Save();
        if ((double) this.Opacity < 1.0)
          pdfGraphics1.SetTransparency(this.Opacity);
        location.X -= (pdfArray1[0] as PdfNumber).FloatValue;
        location.Y += (pdfArray1[1] as PdfNumber).FloatValue;
        pdfGraphics1.DrawPdfTemplate(template, location);
        pdfGraphics1.Restore(state);
        this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
        flag = false;
      }
    }
    return flag;
  }

  internal PdfLineBorderStyle GetLineBorder()
  {
    PdfLineBorderStyle lineBorder = PdfLineBorderStyle.Solid;
    if (this.Dictionary.ContainsKey("BS"))
    {
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["BS"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("S"))
        lineBorder = this.GetBorderStyle((pdfDictionary["S"] as PdfName).Value.ToString());
    }
    return lineBorder;
  }

  internal PdfLineBorderStyle GetBorderStyle(string bstyle)
  {
    PdfLineBorderStyle borderStyle = PdfLineBorderStyle.Solid;
    switch (bstyle)
    {
      case "S":
        borderStyle = PdfLineBorderStyle.Solid;
        break;
      case "D":
        borderStyle = PdfLineBorderStyle.Dashed;
        break;
      case "B":
        borderStyle = PdfLineBorderStyle.Beveled;
        break;
      case "I":
        borderStyle = PdfLineBorderStyle.Inset;
        break;
      case "U":
        borderStyle = PdfLineBorderStyle.Underline;
        break;
    }
    return borderStyle;
  }

  internal PdfLoadedStyledAnnotation(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
    this.m_dictionary = dictionary;
    this.m_crossTable = crossTable;
  }

  internal void FlattenLoadedPopup()
  {
    string empty = string.Empty;
    if (this.Dictionary.ContainsKey("Contents"))
      empty = (this.Dictionary["Contents"] as PdfString).Value;
    string author = this.ObtainAuthor();
    string subject = this.ObtainSubject();
    if (!this.Dictionary.ContainsKey("Popup"))
    {
      this.FlattenPopup((PdfPageBase) this.Page, this.Color, this.Bounds, this.Border, author, subject, empty);
      this.RemoveAnnoation((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
    else
    {
      RectangleF boundsValue = this.GetBoundsValue();
      PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush(this.Color);
      float num1 = this.Border.Width / 2f;
      float trackingHeight = 0.0f;
      PdfBrush aBrush = (PdfBrush) new PdfSolidBrush(this.GetForeColor(this.Color));
      if (author != null && author != string.Empty)
        this.DrawAuthor(author, subject, boundsValue, pdfBrush, aBrush, (PdfPageBase) this.Page, out trackingHeight, this.Border);
      else if (subject != null && subject != string.Empty)
      {
        RectangleF rectangle = new RectangleF(boundsValue.X + num1, boundsValue.Y + num1, boundsValue.Width - this.Border.Width, 40f);
        this.SaveGraphics((PdfPageBase) this.Page, PdfBlendMode.HardLight);
        this.Page.Graphics.DrawRectangle(PdfPens.Black, pdfBrush, rectangle);
        this.Page.Graphics.Restore();
        RectangleF bounds = new RectangleF(rectangle.X + 11f, rectangle.Y, rectangle.Width, rectangle.Height / 2f);
        bounds = new RectangleF(bounds.X, (float) ((double) bounds.Y + (double) bounds.Height - 2.0), bounds.Width, rectangle.Height / 2f);
        this.SaveGraphics((PdfPageBase) this.Page, PdfBlendMode.Normal);
        this.DrawSubject(subject, bounds, (PdfPageBase) this.Page);
        trackingHeight = 40f;
        this.Page.Graphics.Restore();
      }
      else
      {
        this.SaveGraphics((PdfPageBase) this.Page, PdfBlendMode.HardLight);
        RectangleF rectangle = new RectangleF(boundsValue.X + num1, boundsValue.Y + num1, boundsValue.Width - this.Border.Width, 20f);
        this.Page.Graphics.DrawRectangle(PdfPens.Black, pdfBrush, rectangle);
        trackingHeight = 20f;
        this.Page.Graphics.Restore();
      }
      this.SaveGraphics((PdfPageBase) this.Page, PdfBlendMode.HardLight);
      RectangleF rectangleF = new RectangleF(boundsValue.X + num1, boundsValue.Y + num1 + trackingHeight, boundsValue.Width - this.Border.Width, boundsValue.Height - (trackingHeight + this.Border.Width));
      this.Page.Graphics.DrawRectangle(PdfPens.Black, PdfBrushes.White, rectangleF);
      rectangleF.X += 11f;
      rectangleF.Y += 5f;
      rectangleF.Width -= 22f;
      this.Page.Graphics.Restore();
      this.SaveGraphics((PdfPageBase) this.Page, PdfBlendMode.Normal);
      List<object> objectList = (List<object>) null;
      if (this.Dictionary.ContainsKey("RC"))
        objectList = this.ParseXMLData();
      if (objectList != null)
      {
        float num2 = 0.0f;
        for (int index = 0; index < objectList.Count; index += 4)
        {
          PdfFont font = objectList[index] as PdfFont;
          string str = objectList[index + 3] as string;
          if (!(objectList[index + 2] is PdfBrush black))
            black = PdfBrushes.Black;
          PdfStringFormat format = new PdfStringFormat((PdfTextAlignment) objectList[index + 1]);
          if (str != null)
          {
            SizeF sizeF = font.MeasureString(str, rectangleF.Width);
            int count = Regex.Matches(str, "\r\r").Count;
            float height = sizeF.Height;
            if (count > 0)
              height += font.Height * (float) count;
            if ((double) rectangleF.Height < (double) height)
              height = rectangleF.Height;
            if ((double) num2 + (double) height > (double) rectangleF.Height)
            {
              float num3 = num2 + height - rectangleF.Height;
              height -= num3;
            }
            this.Page.Graphics.DrawString(str, font, black, new RectangleF(rectangleF.X, rectangleF.Y + num2, sizeF.Width, height), format);
            num2 += height;
          }
        }
      }
      else
        this.Page.Graphics.DrawString(empty, (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10.5f), PdfBrushes.Black, rectangleF);
      this.Page.Graphics.Restore();
      this.RemoveAnnoation((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
  }

  internal List<object> ParseXMLData()
  {
    PdfFont pdfFont1 = (PdfFont) null;
    PdfDocumentBase document = this.m_crossTable.Document;
    pdfFont1 = PdfDocument.ConformanceLevel != PdfConformanceLevel.None ? (PdfFont) new PdfTrueTypeFont(new Font("Arial", 10.5f, FontStyle.Regular, GraphicsUnit.Point), true, true, true) : (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10.5f, PdfFontStyle.Regular);
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml((this.Dictionary["RC"] as PdfString).Value);
    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("span");
    if (elementsByTagName.Count <= 0)
      return (List<object>) null;
    List<object> xmlData = new List<object>();
    foreach (XmlNode xmlNode in elementsByTagName)
    {
      float result = 10.5f;
      PdfTextAlignment pdfTextAlignment = PdfTextAlignment.Left;
      PdfFontStyle style = PdfFontStyle.Regular;
      PdfBrush pdfBrush = (PdfBrush) null;
      if (xmlNode.Attributes["style"] != null)
      {
        string[] strArray1 = xmlNode.Attributes["style"].Value.Split(';');
        if (strArray1.Length == 0 && xmlNode.OwnerDocument.OuterXml != null)
          strArray1 = xmlNode.OwnerDocument.OuterXml.Split(';');
        foreach (string str in strArray1)
        {
          char[] chArray = new char[1]{ ':' };
          string[] strArray2 = str.Split(chArray);
          switch (strArray2[0])
          {
            case "font-size":
              float.TryParse(strArray2[1].TrimEnd('p', 't').TrimStart('-'), out result);
              break;
            case "font-weight":
              if (strArray2[1].Contains("bold"))
              {
                style |= PdfFontStyle.Bold;
                break;
              }
              break;
            case "font-style":
              if (strArray2[1].Contains("normal"))
                style = style;
              if (strArray2[1].Contains("underline"))
                style |= PdfFontStyle.Underline;
              if (strArray2[1].Contains("strikeout"))
                style |= PdfFontStyle.Strikeout;
              if (strArray2[1].Contains("italic"))
                style |= PdfFontStyle.Italic;
              if (strArray2[1].Contains("bold"))
              {
                style |= PdfFontStyle.Bold;
                break;
              }
              break;
            case "text-align":
              switch (strArray2[1])
              {
                case "left":
                  pdfTextAlignment = PdfTextAlignment.Left;
                  continue;
                case "right":
                  pdfTextAlignment = PdfTextAlignment.Right;
                  continue;
                case "center":
                  pdfTextAlignment = PdfTextAlignment.Center;
                  continue;
                case "justify":
                  pdfTextAlignment = PdfTextAlignment.Justify;
                  continue;
                default:
                  continue;
              }
            case "color":
              pdfBrush = (PdfBrush) new PdfSolidBrush(new PdfColor(this.FromHtml(strArray2[1])));
              break;
          }
        }
      }
      if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
      {
        xmlData.Add((object) new PdfStandardFont(PdfFontFamily.Helvetica, result, style));
      }
      else
      {
        PdfFont pdfFont2 = (PdfFont) new PdfTrueTypeFont(new Font("Arial", result, FontStyle.Regular, GraphicsUnit.Point), true, true, true);
        xmlData.Add((object) pdfFont2);
      }
      xmlData.Add((object) pdfTextAlignment);
      xmlData.Add((object) pdfBrush);
      xmlData.Add(xmlNode.FirstChild != null ? (object) xmlNode.FirstChild.Value : (object) string.Empty);
    }
    return xmlData;
  }

  internal System.Drawing.Color FromHtml(string colorString)
  {
    return ColorTranslator.FromHtml(colorString);
  }

  private RectangleF GetBoundsValue()
  {
    if (!this.Dictionary.ContainsKey("Popup") || !(PdfCrossTable.Dereference(((this.Dictionary["Popup"] as PdfReferenceHolder).Object as PdfDictionary)["Rect"]) is PdfArray pdfArray))
      return RectangleF.Empty;
    RectangleF rectangle = pdfArray.ToRectangle();
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    if (this.Page != null)
    {
      if ((double) rectangle.Y == 0.0 && (double) rectangle.Height == 0.0)
        rectangle.Y += rectangle.Height;
      else
        rectangle.Y = this.Page.Size.Height - (rectangle.Y + rectangle.Height);
    }
    else
      rectangle.Y -= rectangle.Height;
    return rectangle;
  }

  private void RemoveAnnoation(PdfPageBase page, PdfAnnotation annot)
  {
    if (page is PdfPage pdfPage)
    {
      pdfPage.Annotations.Remove(annot);
    }
    else
    {
      PdfLoadedPage wrapper = page as PdfLoadedPage;
      PdfDictionary dictionary = wrapper.Dictionary;
      PdfArray primitive = !dictionary.ContainsKey("Annots") ? new PdfArray() : wrapper.CrossTable.GetObject(dictionary["Annots"]) as PdfArray;
      annot.Dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
      int index1 = -1;
      List<IPdfPrimitive> pdfPrimitiveList = new List<IPdfPrimitive>();
      for (int index2 = 0; index2 < primitive.Count; ++index2)
      {
        if (annot.Dictionary.ContainsKey("Popup") && (primitive[index2] as PdfReferenceHolder).Reference == (annot.Dictionary["Popup"] as PdfReferenceHolder).Reference)
        {
          pdfPrimitiveList.Add(primitive[index2]);
          index1 = index2;
          break;
        }
      }
      if (index1 != -1)
        primitive.RemoveAt(index1);
      primitive.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annot));
      page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
  }
}
