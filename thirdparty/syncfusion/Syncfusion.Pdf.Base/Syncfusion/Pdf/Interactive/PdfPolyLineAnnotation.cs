// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfPolyLineAnnotation
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

public class PdfPolyLineAnnotation : PdfAnnotation
{
  private LineBorder m_border;
  internal PdfArray m_linePoints;
  private int m_lineExtension;
  internal PdfArray m_lineStyle;
  private PdfLineEndingStyle m_beginLine;
  private PdfLineEndingStyle m_endLine;
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

  public LineBorder Border
  {
    get
    {
      if (this.m_border == null)
        this.m_border = new LineBorder();
      return this.m_border;
    }
    set => this.m_border = value;
  }

  public int LineExtension
  {
    get => this.m_lineExtension;
    set => this.m_lineExtension = value;
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

  public PdfPolyLineAnnotation(int[] points, string text)
  {
    this.m_linePoints = new PdfArray(points);
    this.Text = text;
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("PolyLine"));
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
    PdfPageBase page = this.Page == null ? (PdfPageBase) this.LoadedPage : (PdfPageBase) this.Page;
    this.m_borderWidth = this.Border.BorderWidth != 1 ? (float) this.Border.BorderWidth : this.Border.BorderLineWidth;
    PdfGraphics layerGraphics = this.GetLayerGraphics();
    RectangleF rectangleF = RectangleF.Empty;
    PointF[] linePoints = this.GetLinePoints();
    byte[] pathTypes = new byte[linePoints.Length];
    pathTypes[0] = (byte) 0;
    for (int index = 1; index < linePoints.Length; ++index)
      pathTypes[index] = (byte) 1;
    PdfPath path = new PdfPath(linePoints, pathTypes);
    base.Save();
    this.Dictionary.SetProperty("Vertices", (IPdfPrimitive) new PdfArray(this.m_linePoints));
    this.m_lineStyle = new PdfArray();
    this.m_lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.BeginLineStyle));
    this.m_lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.EndLineStyle));
    this.Dictionary.SetProperty("LE", (IPdfPrimitive) this.m_lineStyle);
    this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
    this.Dictionary.SetProperty("LLE", (IPdfPrimitive) new PdfNumber(this.m_lineExtension));
    if (this.SetAppearanceDictionary)
    {
      this.GetBoundsValue();
      rectangleF = new RectangleF(this.Bounds.X - this.m_borderWidth, this.Bounds.Y - this.m_borderWidth, this.Bounds.Width + 2f * this.m_borderWidth, this.Bounds.Height + 2f * this.m_borderWidth);
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
          {
            page.Graphics.Save();
            page.Graphics.SetTransparency(this.Opacity);
          }
          this.RemoveAnnoationFromPage(page, (PdfAnnotation) this);
          if (layerGraphics != null)
            layerGraphics.DrawPath(pen, brush, path);
          else
            page.Graphics.DrawPath(pen, brush, path);
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
          graphics.DrawPath(pen, brush, path);
          if ((double) this.Opacity < 1.0)
            graphics.Restore();
        }
      }
    }
    if (this.Flatten && !this.SetAppearanceDictionary)
    {
      this.RemoveAnnoationFromPage(page, (PdfAnnotation) this);
      PdfPen pen = (PdfPen) null;
      if ((double) this.m_borderWidth > 0.0 && this.Color.A != (byte) 0)
        pen = new PdfPen(this.Color, this.m_borderWidth);
      PdfBrush brush = this.InnerColor.IsEmpty ? (PdfBrush) null : (PdfBrush) new PdfSolidBrush(this.InnerColor);
      if ((double) this.Opacity < 1.0)
      {
        page.Graphics.Save();
        page.Graphics.SetTransparency(this.Opacity);
      }
      if (layerGraphics != null)
        layerGraphics.DrawPath(pen, brush, path);
      else
        page.Graphics.DrawPath(pen, brush, path);
      if ((double) this.Opacity >= 1.0)
        return;
      page.Graphics.Restore();
    }
    else
    {
      if (this.Flatten)
        return;
      base.Save();
      this.GetBoundsValue();
      if (!this.SetAppearanceDictionary)
        return;
      this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(rectangleF));
    }
  }
}
