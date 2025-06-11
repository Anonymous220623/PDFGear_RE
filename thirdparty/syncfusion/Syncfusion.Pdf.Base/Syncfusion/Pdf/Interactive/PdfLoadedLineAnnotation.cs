// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedLineAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedLineAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private PdfColor m_backcolor;
  private LineBorder m_lineborder;
  private PdfArray m_linePoints;
  private int[] m_points;
  private float[] m_point;
  internal PdfFont m_font;
  private PdfRecordCollection readTextCollection;
  private ContentParser parser;
  private bool m_isfontAPStream;
  private MemoryStream freeTextStream;
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

  public PdfLoadedPopupAnnotationCollection Comments
  {
    get
    {
      if (this.m_comments == null)
        this.m_comments = new PdfLoadedPopupAnnotationCollection(this.Page, this.Dictionary, false);
      return this.m_comments;
    }
  }

  public PdfColor BackColor
  {
    get => this.ObtainBackColor();
    set
    {
      PdfArray primitive = new PdfArray();
      this.m_backcolor = value;
      primitive.Insert(0, (IPdfPrimitive) new PdfNumber((float) this.m_backcolor.R / (float) byte.MaxValue));
      primitive.Insert(1, (IPdfPrimitive) new PdfNumber((float) this.m_backcolor.G / (float) byte.MaxValue));
      primitive.Insert(2, (IPdfPrimitive) new PdfNumber((float) this.m_backcolor.B / (float) byte.MaxValue));
      this.Dictionary.SetProperty("C", (IPdfPrimitive) primitive);
    }
  }

  public PdfLineEndingStyle BeginLineStyle
  {
    get => this.GetLineStyle(0);
    set
    {
      PdfArray lineStyle = this.GetLineStyle();
      if (lineStyle == null)
        lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) PdfLineEndingStyle.Square));
      else
        lineStyle.RemoveAt(0);
      lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.GetLineStyle(value.ToString())));
      this.Dictionary.SetProperty("LE", (IPdfPrimitive) lineStyle);
    }
  }

  public PdfLineCaptionType CaptionType
  {
    get => this.ObtainCaptionType();
    set
    {
      this.Dictionary.SetProperty("CP", (IPdfPrimitive) new PdfName((Enum) this.GetCaptionType(value.ToString())));
    }
  }

  public PdfLineEndingStyle EndLineStyle
  {
    get => this.GetLineStyle(1);
    set
    {
      PdfArray lineStyle = this.GetLineStyle();
      if (lineStyle == null)
        lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) PdfLineEndingStyle.Square));
      else
        lineStyle.RemoveAt(1);
      lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.GetLineStyle(value.ToString())));
      this.Dictionary.SetProperty("LE", (IPdfPrimitive) lineStyle);
    }
  }

  public PdfColor InnerLineColor
  {
    get => this.ObtainInnerLineColor();
    set
    {
      this.m_backcolor = value;
      if (value.A != (byte) 0)
      {
        this.Dictionary.SetProperty("IC", (IPdfPrimitive) value.ToArray());
      }
      else
      {
        if (!this.Dictionary.ContainsKey("IC"))
          return;
        this.Dictionary.Remove("IC");
      }
    }
  }

  public int LeaderLine
  {
    get => this.ObtainLeaderLine();
    set => this.Dictionary.SetNumber("LL", value);
  }

  public int LeaderExt
  {
    get => this.ObtainLeaderExt();
    set => this.Dictionary.SetNumber("LLE", value);
  }

  public int LeaderOffset
  {
    get => this.ObtainLeaderOffset();
    set => this.Dictionary.SetNumber("LLO", value);
  }

  public LineBorder LineBorder
  {
    get => this.ObtainLineBorder();
    set
    {
      this.m_lineborder = value;
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_lineborder);
    }
  }

  public int[] LinePoints
  {
    get
    {
      this.m_points = this.ObtainLinePoints();
      return this.m_points;
    }
    set
    {
      this.m_points = value;
      this.m_linePoints = new PdfArray(this.m_points);
      this.Dictionary.SetProperty("L", (IPdfPrimitive) this.m_linePoints);
    }
  }

  internal float[] LinePoint
  {
    get
    {
      this.m_point = this.ObtainLinePoint();
      return this.m_point;
    }
    set
    {
      this.m_point = value;
      this.m_linePoints = new PdfArray(this.m_point);
      this.Dictionary.SetProperty("L", (IPdfPrimitive) this.m_linePoints);
    }
  }

  public bool LineCaption
  {
    get => this.ObtainLineCaption();
    set => this.Dictionary.SetBoolean("Cap", value);
  }

  public PdfLineIntent LineIntent
  {
    get => this.ObtainLineIntent();
    set => this.Dictionary.SetName("IT", value.ToString());
  }

  internal PdfLoadedLineAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
    : base(dictionary, crossTable)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
    this.Text = text;
  }

  private PdfLineIntent ObtainLineIntent()
  {
    PdfLineIntent lineIntent = PdfLineIntent.LineArrow;
    if (this.Dictionary.ContainsKey("IT"))
      lineIntent = this.GetLineIntentText((this.m_crossTable.GetObject(this.Dictionary["IT"]) as PdfName).Value.ToString());
    return lineIntent;
  }

  private PdfArray GetLineStyle()
  {
    PdfArray lineStyle = (PdfArray) null;
    if (this.Dictionary.ContainsKey("LE"))
      lineStyle = this.m_crossTable.GetObject(this.Dictionary["LE"]) as PdfArray;
    return lineStyle;
  }

  private PdfLineEndingStyle GetLineStyle(int Ch)
  {
    PdfLineEndingStyle lineStyle1 = PdfLineEndingStyle.None;
    PdfArray lineStyle2 = this.GetLineStyle();
    if (lineStyle2 != null)
      lineStyle1 = this.GetLineStyle((lineStyle2[Ch] as PdfName).Value);
    return lineStyle1;
  }

  private PdfLineEndingStyle GetLineStyle(string style)
  {
    PdfLineEndingStyle lineStyle = PdfLineEndingStyle.None;
    switch (style)
    {
      case "Square":
        lineStyle = PdfLineEndingStyle.Square;
        break;
      case "Circle":
        lineStyle = PdfLineEndingStyle.Circle;
        break;
      case "Diamond":
        lineStyle = PdfLineEndingStyle.Diamond;
        break;
      case "OpenArrow":
        lineStyle = PdfLineEndingStyle.OpenArrow;
        break;
      case "ClosedArrow":
        lineStyle = PdfLineEndingStyle.ClosedArrow;
        break;
      case "None":
        lineStyle = PdfLineEndingStyle.None;
        break;
      case "ROpenArrow":
        lineStyle = PdfLineEndingStyle.ROpenArrow;
        break;
      case "Butt":
        lineStyle = PdfLineEndingStyle.Butt;
        break;
      case "RClosedArrow":
        lineStyle = PdfLineEndingStyle.RClosedArrow;
        break;
      case "Slash":
        lineStyle = PdfLineEndingStyle.Slash;
        break;
    }
    return lineStyle;
  }

  private PdfColor ObtainInnerLineColor()
  {
    PdfColorSpace colorSpace = PdfColorSpace.RGB;
    PdfColor innerLineColor = PdfColor.Empty;
    bool flag = false;
    PdfArray pdfArray;
    if (this.Dictionary.ContainsKey("IC"))
    {
      pdfArray = PdfCrossTable.Dereference(this.Dictionary["IC"]) as PdfArray;
      flag = true;
    }
    else
      pdfArray = innerLineColor.ToArray(colorSpace);
    if (pdfArray != null && pdfArray[0] is PdfNumber && pdfArray[1] is PdfNumber && pdfArray[2] is PdfNumber)
    {
      byte red = (byte) Math.Round((double) (pdfArray[0] as PdfNumber).FloatValue * (double) byte.MaxValue);
      byte green = (byte) Math.Round((double) (pdfArray[1] as PdfNumber).FloatValue * (double) byte.MaxValue);
      byte blue = (byte) Math.Round((double) (pdfArray[2] as PdfNumber).FloatValue * (double) byte.MaxValue);
      innerLineColor = !flag ? new PdfColor((byte) 0, red, green, blue) : new PdfColor(red, green, blue);
    }
    return innerLineColor;
  }

  private PdfColor ObtainBackColor()
  {
    PdfColorSpace colorSpace = PdfColorSpace.RGB;
    PdfColor backColor = PdfColor.Empty;
    PdfArray pdfArray = !this.Dictionary.ContainsKey("C") ? backColor.ToArray(colorSpace) : PdfCrossTable.Dereference(this.Dictionary["C"]) as PdfArray;
    if (pdfArray != null && pdfArray[0] is PdfNumber && pdfArray[1] is PdfNumber && pdfArray[2] is PdfNumber)
      backColor = new PdfColor((byte) Math.Round((double) (pdfArray[0] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (pdfArray[1] as PdfNumber).FloatValue * (double) byte.MaxValue), (byte) Math.Round((double) (pdfArray[2] as PdfNumber).FloatValue * (double) byte.MaxValue));
    return backColor;
  }

  private PdfLineCaptionType ObtainCaptionType()
  {
    PdfLineCaptionType captionType = PdfLineCaptionType.Inline;
    if (this.Dictionary.ContainsKey("CP"))
      captionType = this.GetCaptionType((this.Dictionary["CP"] as PdfName).Value.ToString());
    return captionType;
  }

  private PdfLineCaptionType GetCaptionType(string cType)
  {
    return !(cType == "Inline") ? PdfLineCaptionType.Top : PdfLineCaptionType.Inline;
  }

  private bool ObtainLineCaption()
  {
    bool lineCaption = false;
    if (this.Dictionary.ContainsKey("Cap"))
      lineCaption = (this.Dictionary["Cap"] as PdfBoolean).Value;
    return lineCaption;
  }

  private int ObtainLeaderLine()
  {
    int leaderLine = 0;
    if (this.Dictionary.ContainsKey("LL"))
      leaderLine = (this.Dictionary["LL"] as PdfNumber).IntValue;
    return leaderLine;
  }

  private int ObtainLeaderExt()
  {
    int leaderExt = 0;
    if (this.Dictionary.ContainsKey("LLE"))
      leaderExt = (this.Dictionary["LLE"] as PdfNumber).IntValue;
    return leaderExt;
  }

  private int ObtainLeaderOffset()
  {
    int leaderOffset = 0;
    if (this.Dictionary.ContainsKey("LLO"))
      leaderOffset = (this.Dictionary["LLO"] as PdfNumber).IntValue;
    return leaderOffset;
  }

  private LineBorder ObtainLineBorder()
  {
    LineBorder lineBorder = new LineBorder();
    if (this.Dictionary.ContainsKey("Border"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Border"]) is PdfArray pdfArray1 && pdfArray1.Count >= 2 && pdfArray1[2] is PdfNumber)
      {
        float floatValue = (pdfArray1[2] as PdfNumber).FloatValue;
        lineBorder.BorderWidth = (int) floatValue;
        lineBorder.BorderLineWidth = floatValue;
      }
    }
    else if (this.Dictionary.ContainsKey("BS"))
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
      if (pdfDictionary.ContainsKey("D") && PdfCrossTable.Dereference(pdfDictionary["D"]) is PdfArray pdfArray2 && pdfArray2.Count > 0)
      {
        int intValue = (pdfArray2[0] as PdfNumber).IntValue;
        pdfArray2.Clear();
        pdfArray2.Insert(0, (IPdfPrimitive) new PdfNumber(intValue));
        pdfArray2.Insert(1, (IPdfPrimitive) new PdfNumber(intValue));
        lineBorder.DashArray = intValue;
      }
    }
    return lineBorder;
  }

  private int[] ObtainLinePoints()
  {
    int[] linePoints = (int[]) null;
    if (this.Dictionary.ContainsKey("L"))
    {
      this.m_linePoints = PdfCrossTable.Dereference(this.Dictionary["L"]) as PdfArray;
      if (this.m_linePoints != null)
      {
        linePoints = new int[this.m_linePoints.Count];
        int index = 0;
        foreach (PdfNumber linePoint in this.m_linePoints)
        {
          linePoints[index] = linePoint.IntValue;
          ++index;
        }
      }
    }
    return linePoints;
  }

  private float[] ObtainLinePoint()
  {
    float[] linePoint1 = (float[]) null;
    if (this.Dictionary.ContainsKey("L"))
    {
      this.m_linePoints = PdfCrossTable.Dereference(this.Dictionary["L"]) as PdfArray;
      if (this.m_linePoints != null)
      {
        linePoint1 = new float[this.m_linePoints.Count];
        int index = 0;
        foreach (PdfNumber linePoint2 in this.m_linePoints)
        {
          linePoint1[index] = linePoint2.FloatValue;
          ++index;
        }
      }
    }
    return linePoint1;
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

  private PdfLineIntent GetLineIntentText(string lintent)
  {
    PdfLineIntent lineIntentText = PdfLineIntent.LineArrow;
    switch (lintent)
    {
      case "LineArrow":
        lineIntentText = PdfLineIntent.LineArrow;
        break;
      case "LineDimension":
        lineIntentText = PdfLineIntent.LineDimension;
        break;
    }
    return lineIntentText;
  }

  protected override void Save()
  {
    this.CheckFlatten();
    this.m_borderWidth = this.LineBorder.BorderWidth != 1 ? (float) this.LineBorder.BorderWidth : this.LineBorder.BorderLineWidth;
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

  private RectangleF ObtainLineBounds()
  {
    RectangleF lineBounds = this.Bounds;
    if (this.LinePoints != null && this.m_points != null || this.LinePoint != null && this.m_point != null)
    {
      PdfPath pdfPath = new PdfPath();
      float[] linePoint = this.ObtainLinePoint();
      if (linePoint != null && linePoint.Length == 4)
      {
        PdfArray lineStyle = new PdfArray();
        lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.BeginLineStyle));
        lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.EndLineStyle));
        lineBounds = this.CalculateLineBounds(linePoint, this.LeaderExt, this.LeaderLine, this.LeaderOffset, lineStyle, (double) this.m_borderWidth);
        lineBounds = new RectangleF(lineBounds.X - 8f, lineBounds.Y - 8f, lineBounds.Width + 16f, lineBounds.Height + 16f);
      }
    }
    return lineBounds;
  }

  private PdfTemplate CreateAppearance()
  {
    if (!this.SetAppearanceDictionary)
      return (PdfTemplate) null;
    PdfTemplate appearance = new PdfTemplate(this.ObtainLineBounds());
    this.SetMatrix((PdfDictionary) appearance.m_content);
    PaintParams paintParams = new PaintParams();
    appearance.m_writeTransformation = false;
    PdfGraphics graphics = appearance.Graphics;
    PdfPen pdfPen = new PdfPen(this.BackColor, this.m_borderWidth);
    PdfBrush m_backBrush = (PdfBrush) new PdfSolidBrush(this.InnerLineColor);
    if (this.LineBorder.BorderStyle == PdfBorderStyle.Dashed)
      pdfPen.DashStyle = PdfDashStyle.Dash;
    else if (this.LineBorder.BorderStyle == PdfBorderStyle.Dot)
      pdfPen.DashStyle = PdfDashStyle.Dot;
    paintParams.BorderPen = pdfPen;
    paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
    PdfFont font = this.ObtainFont();
    if (font == null)
    {
      if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
      {
        font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
      }
      else
      {
        PdfDocumentBase document = this.m_crossTable.Document;
        font = (PdfFont) new PdfTrueTypeFont(new Font("Arial", 10f, FontStyle.Regular, GraphicsUnit.Point), true, true, true);
      }
    }
    float width = font.MeasureString(this.Text, new PdfStringFormat()
    {
      Alignment = PdfTextAlignment.Center,
      LineAlignment = PdfVerticalAlignment.Middle
    }).Width;
    PdfSolidBrush brush = new PdfSolidBrush(this.Color);
    int[] linePoints = this.ObtainLinePoints();
    if (linePoints != null && linePoints.Length == 4)
    {
      float x1 = (float) linePoints[0];
      float y1 = (float) linePoints[1];
      float x2 = (float) linePoints[2];
      float y2 = (float) linePoints[3];
      double angle = (double) x2 - (double) x1 != 0.0 ? this.GetAngle(x1, y1, x2, y2) : ((double) y2 <= (double) y1 ? 270.0 : 90.0);
      int length1;
      double num1;
      if (this.LeaderLine < 0)
      {
        length1 = -this.LeaderLine;
        num1 = angle + 180.0;
      }
      else
      {
        length1 = this.LeaderLine;
        num1 = angle;
      }
      float[] numArray1 = new float[2]{ x1, y1 };
      float[] numArray2 = new float[2]{ x2, y2 };
      float[] axisValue1 = this.GetAxisValue(numArray1, num1 + 90.0, (double) (length1 + this.LeaderOffset));
      float[] axisValue2 = this.GetAxisValue(numArray2, num1 + 90.0, (double) (length1 + this.LeaderOffset));
      double num2 = Math.Sqrt(Math.Pow((double) axisValue2[0] - (double) axisValue1[0], 2.0) + Math.Pow((double) axisValue2[1] - (double) axisValue1[1], 2.0));
      double length2 = num2 / 2.0 - ((double) width / 2.0 + (double) this.m_borderWidth);
      float[] axisValue3 = this.GetAxisValue(axisValue1, angle, length2);
      float[] axisValue4 = this.GetAxisValue(axisValue2, angle + 180.0, length2);
      float[] numArray3 = this.BeginLineStyle == PdfLineEndingStyle.OpenArrow || this.BeginLineStyle == PdfLineEndingStyle.ClosedArrow ? this.GetAxisValue(axisValue1, angle, (double) this.m_borderWidth) : axisValue1;
      float[] numArray4 = this.EndLineStyle == PdfLineEndingStyle.OpenArrow || this.EndLineStyle == PdfLineEndingStyle.ClosedArrow ? this.GetAxisValue(axisValue2, angle, -(double) this.m_borderWidth) : axisValue2;
      string str = this.CaptionType.ToString();
      if ((double) this.Opacity < 1.0)
      {
        PdfGraphicsState state = graphics.Save();
        graphics.SetTransparency(this.Opacity);
        if (string.IsNullOrEmpty(this.Text) || str == "Top")
        {
          graphics.DrawLine(pdfPen, numArray3[0], -numArray3[1], numArray4[0], -numArray4[1]);
        }
        else
        {
          graphics.DrawLine(pdfPen, numArray3[0], -numArray3[1], axisValue3[0], -axisValue3[1]);
          graphics.DrawLine(pdfPen, numArray4[0], -numArray4[1], axisValue4[0], -axisValue4[1]);
        }
        graphics.Restore(state);
      }
      else if (string.IsNullOrEmpty(this.Text) || str == "Top")
      {
        graphics.DrawLine(pdfPen, numArray3[0], -numArray3[1], numArray4[0], -numArray4[1]);
      }
      else
      {
        graphics.DrawLine(pdfPen, numArray3[0], -numArray3[1], axisValue3[0], -axisValue3[1]);
        graphics.DrawLine(pdfPen, numArray4[0], -numArray4[1], axisValue4[0], -axisValue4[1]);
      }
      PdfArray lineStyle = new PdfArray();
      lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.BeginLineStyle));
      lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.EndLineStyle));
      double borderWidth = (double) this.m_borderWidth;
      this.SetLineEndingStyles(axisValue1, axisValue2, graphics, angle, pdfPen, m_backBrush, lineStyle, borderWidth);
      float[] axisValue5 = this.GetAxisValue(axisValue1, num1 + 90.0, (double) this.LeaderExt);
      graphics.DrawLine(pdfPen, axisValue1[0], -axisValue1[1], axisValue5[0], -axisValue5[1]);
      float[] axisValue6 = this.GetAxisValue(axisValue2, num1 + 90.0, (double) this.LeaderExt);
      graphics.DrawLine(pdfPen, axisValue2[0], -axisValue2[1], axisValue6[0], -axisValue6[1]);
      float[] axisValue7 = this.GetAxisValue(axisValue1, num1 - 90.0, (double) length1);
      graphics.DrawLine(pdfPen, axisValue1[0], -axisValue1[1], axisValue7[0], -axisValue7[1]);
      float[] axisValue8 = this.GetAxisValue(axisValue2, num1 - 90.0, (double) length1);
      graphics.DrawLine(pdfPen, axisValue2[0], -axisValue2[1], axisValue8[0], -axisValue8[1]);
      double length3 = num2 / 2.0;
      float[] axisValue9 = this.GetAxisValue(axisValue1, angle, length3);
      float[] numArray5 = new float[2];
      float[] numArray6 = !(str == "Top") ? (!this.Dictionary.Items.ContainsKey(new PdfName("Measure")) ? this.GetAxisValue(axisValue9, angle + 90.0, (double) font.Height / 2.0) : this.GetAxisValue(axisValue9, angle + 90.0, 3.0 * ((double) font.Height / 2.0))) : (!this.Dictionary.Items.ContainsKey(new PdfName("Measure")) ? this.GetAxisValue(axisValue9, angle + 90.0, (double) font.Height) : this.GetAxisValue(axisValue9, angle + 90.0, 2.0 * (double) font.Height));
      graphics.TranslateTransform(numArray6[0], -numArray6[1]);
      graphics.RotateTransform((float) -angle);
      graphics.DrawString(this.Text, font, (PdfBrush) brush, new PointF((float) (-(double) width / 2.0), 0.0f));
      graphics.Restore();
    }
    this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(this.ObtainLineBounds()));
    return appearance;
  }

  private PdfFont ObtainFont()
  {
    string str1 = "";
    float result = 1f;
    if (this.Dictionary.ContainsKey("DS") || this.Dictionary.ContainsKey("DA"))
    {
      if (this.Dictionary.ContainsKey("DS"))
      {
        string[] strArray1 = (PdfCrossTable.Dereference(this.Dictionary["DS"]) as PdfString).Value.ToString().Split(';');
        for (int index1 = 0; index1 < strArray1.Length; ++index1)
        {
          string[] strArray2 = strArray1[index1].Split(':');
          if (strArray1[index1].Contains("font-family"))
            str1 = strArray2[1];
          else if (strArray1[index1].Contains("font-size"))
          {
            if (strArray2[1].EndsWith("pt"))
              result = float.Parse(strArray2[1].Replace("pt", ""), (IFormatProvider) CultureInfo.InvariantCulture);
          }
          else if (strArray1[index1].Contains("font-style"))
          {
            string str2 = strArray1[1];
          }
          else if (strArray1[index1].Contains("font"))
          {
            string str3 = strArray2[1];
            string str4 = string.Empty;
            string[] strArray3 = str3.Split(' ');
            for (int index2 = 0; index2 < strArray3.Length; ++index2)
            {
              if (strArray3[index2] != "" && !strArray3[index2].EndsWith("pt"))
                str4 = $"{str4}{strArray3[index2]} ";
              if (strArray3[index2].EndsWith("pt"))
                result = float.Parse(strArray3[index2].Replace("pt", ""), (IFormatProvider) CultureInfo.InvariantCulture);
            }
            str1 = str4.TrimEnd(' ');
            if (str1.Contains(","))
              str1 = str1.Split(',')[0];
          }
        }
      }
      else if (this.Dictionary.ContainsKey("DA"))
      {
        PdfString pdfString = this.Dictionary["DA"] as PdfString;
        string empty = string.Empty;
        if (pdfString != null)
          empty = pdfString.Value;
        if (empty != string.Empty && empty.Contains("Tf"))
        {
          string[] strArray = empty.ToString().Split(' ');
          for (int index = 0; index < strArray.Length; ++index)
          {
            if (strArray[index].Contains("Tf"))
            {
              str1 = strArray[index - 2].TrimStart('/');
              float.TryParse(strArray[index - 1], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result);
            }
          }
        }
        else if (this.Dictionary["AP"] != null)
        {
          this.m_isfontAPStream = true;
          this.ObtainFromAPStream(this.m_isfontAPStream);
          return this.m_font;
        }
      }
      string familyName = str1.Trim();
      if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
      {
        switch (familyName)
        {
          case "Helvetica":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, result);
            break;
          case "Courier":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, result);
            break;
          case "Symbol":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Symbol, result);
            break;
          case "TimesRoman":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, result);
            break;
          case "ZapfDingbats":
            this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, result);
            break;
          case "MonotypeSungLight":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeSungLight, result);
            break;
          case "SinoTypeSongLight":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.SinoTypeSongLight, result);
            break;
          case "MonotypeHeiMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeHeiMedium, result);
            break;
          case "HanyangSystemsGothicMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsGothicMedium, result);
            break;
          case "HanyangSystemsShinMyeongJoMedium":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium, result);
            break;
          case "HeiseiKakuGothicW5":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiKakuGothicW5, result);
            break;
          case "HeiseiMinchoW3":
            this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiMinchoW3, result);
            break;
          default:
            this.m_font = (PdfFont) new PdfTrueTypeFont(new Font(familyName, result), result);
            break;
        }
      }
      else
        this.m_font = (PdfFont) new PdfTrueTypeFont(new Font("Arial", result, FontStyle.Regular, GraphicsUnit.Point), true, true, true);
    }
    else if (this.Dictionary["AP"] != null)
    {
      this.m_isfontAPStream = true;
      this.ObtainFromAPStream(this.m_isfontAPStream);
    }
    if (this.freeTextStream != null)
      this.freeTextStream.Dispose();
    return this.m_font;
  }

  private void ObtainFromAPStream(bool isfontStream)
  {
    if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1) || !(PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2))
      return;
    if (!(pdfDictionary2["Resources"] is PdfDictionary pdfDictionary3))
    {
      PdfReferenceHolder pdfReferenceHolder = pdfDictionary2["Resources"] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary3 = pdfReferenceHolder.Object as PdfDictionary;
    }
    if (pdfDictionary3 == null || !(pdfDictionary2 is PdfStream pdfStream))
      return;
    byte[] buffer = PdfString.StringToByte("\r\n");
    pdfStream.Decompress();
    if (this.freeTextStream == null)
      this.freeTextStream = new MemoryStream();
    pdfStream.InternalStream.WriteTo((Stream) this.freeTextStream);
    this.freeTextStream.Write(buffer, 0, buffer.Length);
    if (this.parser == null)
      this.parser = new ContentParser(this.freeTextStream.ToArray());
    this.readTextCollection = this.parser.ReadContent();
    if (this.readTextCollection == null || !isfontStream)
      return;
    string familyName = "";
    float num = 1f;
    foreach (PdfRecord readText in this.readTextCollection)
    {
      string empty = string.Empty;
      string[] operands = readText.Operands;
      if (readText.OperatorName == "Tf")
      {
        string[] strArray = operands;
        string str = strArray[0];
        PdfDictionary pdfDictionary4 = pdfDictionary3["Font"] as PdfDictionary;
        if (str.Contains("/"))
        {
          string key = str.Trim('/');
          familyName = ((PdfCrossTable.Dereference(pdfDictionary4[key]) as PdfDictionary)["BaseFont"] as PdfName).Value;
        }
        num = float.Parse(strArray[1], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
      }
    }
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
    {
      switch (familyName)
      {
        case "Helvetica":
          this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, num);
          break;
        case "Courier":
          this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, num);
          break;
        case "Symbol":
          this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Symbol, num);
          break;
        case "TimesRoman":
          this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, num);
          break;
        case "Times-Roman":
          this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, num);
          break;
        case "ZapfDingbats":
          this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, num);
          break;
        case "MonotypeSungLight":
          this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeSungLight, num);
          break;
        case "SinoTypeSongLight":
          this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.SinoTypeSongLight, num);
          break;
        case "MonotypeHeiMedium":
          this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.MonotypeHeiMedium, num);
          break;
        case "HanyangSystemsGothicMedium":
          this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsGothicMedium, num);
          break;
        case "HanyangSystemsShinMyeongJoMedium":
          this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HanyangSystemsShinMyeongJoMedium, num);
          break;
        case "HeiseiKakuGothicW5":
          this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiKakuGothicW5, num);
          break;
        case "HeiseiMinchoW3":
          this.m_font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiMinchoW3, num);
          break;
        default:
          this.m_font = (PdfFont) new PdfTrueTypeFont(new Font(familyName, num), num);
          break;
      }
    }
    else
      this.m_font = (PdfFont) new PdfTrueTypeFont(new Font("Arial", num, FontStyle.Regular, GraphicsUnit.Point), true, true, true);
  }
}
