// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLineAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLineAnnotation : PdfAnnotation
{
  private PdfLineEndingStyle m_beginLine;
  private PdfLineEndingStyle m_endLine;
  private LineBorder m_lineBorder = new LineBorder();
  internal PdfArray m_linePoints;
  internal PdfArray m_lineStyle;
  private int m_leaderLineExt;
  private int m_leaderLine;
  private bool m_lineCaption;
  private PdfLineIntent m_lineIntent;
  public PdfLineCaptionType m_captionType;
  private int[] m_points;
  private float[] m_point;
  private float m_borderWidth;

  public bool LineCaption
  {
    get => this.m_lineCaption;
    set => this.m_lineCaption = value;
  }

  public int LeaderLine
  {
    get => this.m_leaderLine;
    set
    {
      if (this.m_leaderLineExt == 0)
        return;
      this.m_leaderLine = value;
    }
  }

  public int LeaderLineExt
  {
    get => this.m_leaderLineExt;
    set => this.m_leaderLineExt = value;
  }

  public LineBorder lineBorder
  {
    get => this.m_lineBorder;
    set => this.m_lineBorder = value;
  }

  public PdfLineEndingStyle BeginLineStyle
  {
    get => this.m_beginLine;
    set
    {
      if (this.m_beginLine == value)
        return;
      this.m_beginLine = value;
    }
  }

  public PdfLineEndingStyle EndLineStyle
  {
    get => this.m_endLine;
    set
    {
      if (this.m_endLine == value)
        return;
      this.m_endLine = value;
    }
  }

  public PdfLineCaptionType CaptionType
  {
    get => this.m_captionType;
    set => this.m_captionType = value;
  }

  public PdfLineIntent LineIntent
  {
    get => this.m_lineIntent;
    set => this.m_lineIntent = value;
  }

  public PdfColor InnerLineColor
  {
    get => this.InnerColor;
    set => this.InnerColor = value;
  }

  public PdfColor BackColor
  {
    get => this.Color;
    set => this.Color = value;
  }

  public int[] LinePoints
  {
    get => this.m_points;
    set
    {
      this.m_points = value;
      this.m_linePoints = new PdfArray(this.m_points);
    }
  }

  internal float[] LinePoint
  {
    get => this.m_point;
    set
    {
      this.m_point = value;
      this.m_linePoints = new PdfArray(this.m_point);
    }
  }

  public PdfLineAnnotation(int[] linePoints)
  {
    this.m_linePoints = new PdfArray(linePoints);
    this.m_points = linePoints;
  }

  public PdfLineAnnotation(int[] linePoints, string text)
  {
    this.m_linePoints = new PdfArray(linePoints);
    this.Text = text;
    this.m_points = linePoints;
  }

  internal PdfLineAnnotation(float[] linePoints, string text)
  {
    this.m_linePoints = new PdfArray(linePoints);
    this.Text = text;
    this.m_point = linePoints;
  }

  public PdfLineAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
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

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Line"));
  }

  private float[] ObtainLinePoints()
  {
    float[] linePoints = (float[]) null;
    if (this.m_linePoints != null)
    {
      linePoints = new float[this.m_linePoints.Count];
      int index = 0;
      foreach (PdfNumber linePoint in this.m_linePoints)
      {
        linePoints[index] = linePoint.FloatValue;
        ++index;
      }
    }
    return linePoints;
  }

  protected override void Save()
  {
    this.CheckFlatten();
    this.m_borderWidth = this.lineBorder.BorderWidth != 1 ? (float) this.lineBorder.BorderWidth : this.lineBorder.BorderLineWidth;
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
      this.SavePdfLineDictionary();
    }
    if (!this.FlattenPopUps)
      return;
    this.FlattenPopup();
  }

  private RectangleF ObtainLineBounds()
  {
    RectangleF lineBounds = this.Bounds;
    if (this.m_points != null && this.m_points.Length == 4 || this.m_point != null && this.m_point.Length == 4)
    {
      PdfPath pdfPath = new PdfPath();
      float[] linePoints = this.ObtainLinePoints();
      if (linePoints != null && linePoints.Length == 4)
      {
        PdfArray lineStyle = new PdfArray();
        lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.BeginLineStyle));
        lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.EndLineStyle));
        lineBounds = this.CalculateLineBounds(linePoints, this.m_leaderLineExt, this.m_leaderLine, 0, lineStyle, (double) this.m_borderWidth);
        lineBounds = new RectangleF(lineBounds.X - 8f, lineBounds.Y - 8f, lineBounds.Width + 16f, lineBounds.Height + 16f);
      }
    }
    return lineBounds;
  }

  private void SavePdfLineDictionary()
  {
    base.Save();
    this.m_lineStyle = new PdfArray();
    this.m_lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.BeginLineStyle));
    this.m_lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.EndLineStyle));
    this.Dictionary.SetProperty("LE", (IPdfPrimitive) this.m_lineStyle);
    if (this.m_linePoints == null)
      throw new PdfException("LinePoints cannot be null");
    this.Dictionary.SetProperty("L", (IPdfPrimitive) this.m_linePoints);
    if (this.m_lineBorder.DashArray == 0)
    {
      if (this.m_lineBorder.BorderStyle == PdfBorderStyle.Dashed)
        this.m_lineBorder.DashArray = 4;
      else if (this.m_lineBorder.BorderStyle == PdfBorderStyle.Dot)
        this.m_lineBorder.DashArray = 2;
    }
    this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_lineBorder);
    if (this.InnerLineColor.A != (byte) 0)
      this.Dictionary.SetProperty("IC", (IPdfPrimitive) this.InnerColor.ToArray());
    this.Dictionary["C"] = (IPdfPrimitive) this.BackColor.ToArray();
    this.Dictionary.SetProperty("IT", (IPdfPrimitive) new PdfName((Enum) this.m_lineIntent));
    this.Dictionary.SetProperty("LLE", (IPdfPrimitive) new PdfNumber(this.m_leaderLineExt));
    this.Dictionary.SetProperty("LL", (IPdfPrimitive) new PdfNumber(this.m_leaderLine));
    this.Dictionary.SetProperty("CP", (IPdfPrimitive) new PdfName((Enum) this.m_captionType));
    this.Dictionary.SetProperty("Cap", (IPdfPrimitive) new PdfBoolean(this.m_lineCaption));
    this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(this.ObtainLineBounds()));
  }

  private PdfTemplate CreateAppearance()
  {
    RectangleF lineBounds = this.ObtainLineBounds();
    PdfTemplate appearance = new PdfTemplate(lineBounds);
    this.SetMatrix((PdfDictionary) appearance.m_content);
    PaintParams paintParams = new PaintParams();
    appearance.m_writeTransformation = false;
    PdfGraphics graphics = appearance.Graphics;
    PdfPen pdfPen = new PdfPen(this.BackColor, this.m_borderWidth);
    if (this.lineBorder.BorderStyle == PdfBorderStyle.Dashed)
      pdfPen.DashStyle = PdfDashStyle.Dash;
    else if (this.lineBorder.BorderStyle == PdfBorderStyle.Dot)
      pdfPen.DashStyle = PdfDashStyle.Dot;
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush(this.InnerLineColor);
    paintParams.BorderPen = pdfPen;
    paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
    PdfMargins margin = this.ObtainMargin();
    PdfFont font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 9f, PdfFontStyle.Regular);
    float width = font.MeasureString(this.Text, new PdfStringFormat()
    {
      Alignment = PdfTextAlignment.Center,
      LineAlignment = PdfVerticalAlignment.Middle
    }).Width;
    float[] linePoints = this.ObtainLinePoints();
    if (linePoints != null && linePoints.Length == 4)
    {
      float x1 = linePoints[0];
      float y1 = linePoints[1];
      float x2 = linePoints[2];
      float y2 = linePoints[3];
      double angle = (double) x2 - (double) x1 != 0.0 ? this.GetAngle(x1, y1, x2, y2) : ((double) y2 <= (double) y1 ? 270.0 : 90.0);
      int length1;
      double num1;
      if (this.LeaderLine < 0)
      {
        length1 = -this.m_leaderLine;
        num1 = angle + 180.0;
      }
      else
      {
        length1 = this.m_leaderLine;
        num1 = angle;
      }
      float[] numArray1 = new float[2]{ x1, y1 };
      float[] numArray2 = new float[2]{ x2, y2 };
      float[] axisValue1 = this.GetAxisValue(numArray1, num1 + 90.0, (double) length1);
      float[] axisValue2 = this.GetAxisValue(numArray2, num1 + 90.0, (double) length1);
      double num2 = Math.Sqrt(Math.Pow((double) axisValue2[0] - (double) axisValue1[0], 2.0) + Math.Pow((double) axisValue2[1] - (double) axisValue1[1], 2.0));
      double length2 = num2 / 2.0 - ((double) width / 2.0 + (double) this.m_borderWidth);
      float[] axisValue3 = this.GetAxisValue(axisValue1, angle, length2);
      float[] axisValue4 = this.GetAxisValue(axisValue2, angle + 180.0, length2);
      string str = this.m_captionType.ToString();
      float[] numArray3 = this.BeginLineStyle == PdfLineEndingStyle.OpenArrow || this.BeginLineStyle == PdfLineEndingStyle.ClosedArrow ? this.GetAxisValue(axisValue1, angle, (double) this.m_borderWidth) : axisValue1;
      float[] numArray4 = this.EndLineStyle == PdfLineEndingStyle.OpenArrow || this.EndLineStyle == PdfLineEndingStyle.ClosedArrow ? this.GetAxisValue(axisValue2, angle, -(double) this.m_borderWidth) : axisValue2;
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
      this.SetLineEndingStyles(axisValue1, axisValue2, graphics, angle, pdfPen, pdfBrush, lineStyle, borderWidth);
      float[] axisValue5 = this.GetAxisValue(axisValue1, num1 + 90.0, (double) this.m_leaderLineExt);
      graphics.DrawLine(pdfPen, axisValue1[0], -axisValue1[1], axisValue5[0], -axisValue5[1]);
      float[] axisValue6 = this.GetAxisValue(axisValue2, num1 + 90.0, (double) this.m_leaderLineExt);
      graphics.DrawLine(pdfPen, axisValue2[0], -axisValue2[1], axisValue6[0], -axisValue6[1]);
      float[] axisValue7 = this.GetAxisValue(axisValue1, num1 - 90.0, (double) length1);
      graphics.DrawLine(pdfPen, axisValue1[0], -axisValue1[1], axisValue7[0], -axisValue7[1]);
      float[] axisValue8 = this.GetAxisValue(axisValue2, num1 - 90.0, (double) length1);
      graphics.DrawLine(pdfPen, axisValue2[0], -axisValue2[1], axisValue8[0], -axisValue8[1]);
      double length3 = num2 / 2.0;
      float[] axisValue9 = this.GetAxisValue(axisValue1, angle, length3);
      float[] numArray5 = new float[2];
      float[] numArray6 = !(str == "Top") ? this.GetAxisValue(axisValue9, angle + 90.0, (double) font.Height / 2.0) : this.GetAxisValue(axisValue9, angle + 90.0, (double) font.Height);
      graphics.TranslateTransform(numArray6[0], -numArray6[1]);
      graphics.RotateTransform((float) -angle);
      graphics.DrawString(this.Text, font, pdfBrush, new PointF((float) (-(double) width / 2.0), 0.0f));
      graphics.Restore();
    }
    if (this.Flatten)
    {
      float num = this.Page != null ? this.Page.Size.Height : this.LoadedPage.Size.Height;
      if (this.Page != null)
        this.Bounds = new RectangleF(lineBounds.X - margin.Left, num - (lineBounds.Y + lineBounds.Height) - margin.Top, lineBounds.Width, lineBounds.Height);
      else
        this.Bounds = new RectangleF(lineBounds.X, num - (lineBounds.Y + lineBounds.Height), lineBounds.Width, lineBounds.Height);
    }
    return appearance;
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
}
