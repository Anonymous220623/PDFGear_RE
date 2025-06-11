// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedPolyLineAnnotation
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

public class PdfLoadedPolyLineAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private LineBorder m_lineborder;

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

  internal int[] PolylinePoints
  {
    get
    {
      int[] polylinePoints = (int[]) null;
      if (this.Dictionary.ContainsKey("Vertices") && PdfCrossTable.Dereference(this.Dictionary["Vertices"]) is PdfArray pdfArray)
      {
        polylinePoints = new int[pdfArray.Count];
        int index = 0;
        foreach (PdfNumber pdfNumber in pdfArray)
        {
          polylinePoints[index] = pdfNumber.IntValue;
          ++index;
        }
      }
      return polylinePoints;
    }
  }

  public LineBorder LineBorder
  {
    get => this.AssignLineBorder();
    set
    {
      this.m_lineborder = value;
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_lineborder);
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

  internal PdfLoadedPolyLineAnnotation(
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

  private PdfArray GetLineStyle()
  {
    PdfArray lineStyle = (PdfArray) null;
    if (this.Dictionary.ContainsKey("LE"))
      lineStyle = this.m_crossTable.GetObject(this.Dictionary["LE"]) as PdfArray;
    return lineStyle;
  }

  private PointF[] GetLinePoints()
  {
    PdfPageBase page = (PdfPageBase) this.Page;
    if (this.Page.Annotations.Count > 0 && page.Annotations.Flatten)
      this.Page.Annotations.Flatten = page.Annotations.Flatten;
    PointF[] linePoints = (PointF[]) null;
    if (this.Dictionary.ContainsKey("Vertices") && PdfCrossTable.Dereference(this.Dictionary["Vertices"]) is PdfArray pdfArray)
    {
      float[] numArray = new float[pdfArray.Count];
      int index1 = 0;
      foreach (PdfNumber pdfNumber in pdfArray)
      {
        numArray[index1] = pdfNumber.FloatValue;
        ++index1;
      }
      linePoints = new PointF[numArray.Length / 2];
      int index2 = 0;
      for (int index3 = 0; index3 < numArray.Length; index3 += 2)
      {
        float height = this.Page.Size.Height;
        linePoints[index2] = this.Flatten || this.Page.Annotations.Flatten ? new PointF(numArray[index3], height - numArray[index3 + 1]) : new PointF(numArray[index3], -numArray[index3 + 1]);
        ++index2;
      }
    }
    return linePoints;
  }

  private void GetBoundsValue()
  {
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    if (this.Dictionary.ContainsKey("Vertices") && PdfCrossTable.Dereference(this.Dictionary["Vertices"]) is PdfArray pdfArray)
    {
      int[] numArray = new int[pdfArray.Count];
      int index1 = 0;
      foreach (PdfNumber pdfNumber in pdfArray)
      {
        numArray[index1] = pdfNumber.IntValue;
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
    this.Bounds = new RectangleF((float) intList1[0], (float) intList2[0], (float) (intList1[intList1.Count - 1] - intList1[0]), (float) (intList2[intList2.Count - 1] - intList2[0]));
  }

  private PdfColor GetBackColor()
  {
    PdfColorSpace colorSpace = PdfColorSpace.RGB;
    PdfColor backColor = PdfColor.Empty;
    PdfArray pdfArray = !this.Dictionary.ContainsKey("C") ? backColor.ToArray(colorSpace) : PdfCrossTable.Dereference(this.Dictionary["C"]) as PdfArray;
    backColor = new PdfColor((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue, (pdfArray[2] as PdfNumber).FloatValue);
    return backColor;
  }

  private LineBorder AssignLineBorder()
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
      if (pdfDictionary.ContainsKey("D") && PdfCrossTable.Dereference(pdfDictionary["D"]) is PdfArray pdfArray && pdfArray[0] is PdfNumber pdfNumber)
      {
        int intValue = pdfNumber.IntValue;
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

  protected override void Save()
  {
    PdfPageBase page = (PdfPageBase) this.Page;
    if (this.Page.Annotations.Count > 0 && page.Annotations.Flatten)
      this.Page.Annotations.Flatten = page.Annotations.Flatten;
    PointF[] linePoints = this.GetLinePoints();
    byte[] pathTypes = new byte[linePoints.Length];
    pathTypes[0] = (byte) 0;
    for (int index = 1; index < linePoints.Length; ++index)
      pathTypes[index] = (byte) 1;
    PdfPath path = new PdfPath(linePoints, pathTypes);
    RectangleF rectangleF = RectangleF.Empty;
    PdfGraphics pdfGraphics = this.ObtainlayerGraphics();
    if (this.SetAppearanceDictionary)
    {
      this.GetBoundsValue();
      rectangleF = new RectangleF(this.Bounds.X - this.Border.Width, this.Bounds.Y - this.Border.Width, this.Bounds.Width + 2f * this.Border.Width, this.Bounds.Height + 2f * this.Border.Width);
      this.Dictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
      if (this.Dictionary["AP"] != null)
      {
        this.Appearance.Normal = new PdfTemplate(rectangleF);
        this.Appearance.Normal.m_writeTransformation = false;
        PdfGraphics graphics = this.Appearance.Normal.Graphics;
        PdfPen pen = (PdfPen) null;
        if ((double) this.Border.Width > 0.0)
          pen = new PdfPen(this.Color, this.Border.Width);
        if (this.Dictionary.ContainsKey("BS"))
        {
          PdfDictionary pdfDictionary = (object) (this.Dictionary.Items[new PdfName("BS")] as PdfReferenceHolder) == null ? this.Dictionary.Items[new PdfName("BS")] as PdfDictionary : (this.Dictionary.Items[new PdfName("BS")] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey("D") && PdfCrossTable.Dereference(pdfDictionary.Items[new PdfName("D")]) is PdfArray pdfArray)
          {
            float[] numArray = new float[pdfArray.Count];
            for (int index = 0; index < pdfArray.Count; ++index)
            {
              if (pdfArray.Elements[index] is PdfNumber)
                numArray[index] = (pdfArray.Elements[index] as PdfNumber).FloatValue;
            }
            pen.DashStyle = PdfDashStyle.Dash;
            pen.isSkipPatternWidth = true;
            pen.DashPattern = numArray;
          }
        }
        if (this.Flatten || this.Page.Annotations.Flatten)
        {
          this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
          if ((double) this.Opacity < 1.0)
          {
            PdfGraphicsState state = page.Graphics.Save();
            page.Graphics.SetTransparency(this.Opacity);
            if (pdfGraphics != null)
              pdfGraphics.DrawPath(pen, path);
            else
              page.Graphics.DrawPath(pen, path);
            page.Graphics.Restore(state);
          }
          else if (pdfGraphics != null)
            pdfGraphics.DrawPath(pen, path);
          else
            this.Page.Graphics.DrawPath(pen, path);
        }
        else
        {
          PdfArray pdfArray = PdfCrossTable.Dereference(this.Dictionary["Vertices"]) as PdfArray;
          int index1 = 0;
          int num;
          if (pdfArray != null)
          {
            for (int index2 = 0; index2 < linePoints.Length; index2 = num + 1)
            {
              linePoints[index1] = new PointF((pdfArray.Elements[index2] as PdfNumber).FloatValue, -(pdfArray.Elements[index2 + 1] as PdfNumber).FloatValue);
              num = index2 + 1;
              ++index1;
            }
          }
          path = new PdfPath(linePoints, pathTypes);
          if ((double) this.Opacity < 1.0)
          {
            PdfGraphicsState state = graphics.Save();
            graphics.SetTransparency(this.Opacity);
            graphics.DrawPath(pen, path);
            graphics.Restore(state);
          }
          else
            graphics.DrawPath(pen, path);
        }
        this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(rectangleF));
      }
    }
    if ((!this.Flatten || this.SetAppearanceDictionary) && (!this.Page.Annotations.Flatten || this.SetAppearanceDictionary))
      return;
    if (this.Dictionary["AP"] != null)
    {
      if (PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1 && PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary dictionary && dictionary is PdfStream template1)
      {
        PdfTemplate template = new PdfTemplate(template1);
        if (template != null)
        {
          PdfGraphicsState state = page.Graphics.Save();
          if ((double) this.Opacity < 1.0)
            page.Graphics.SetTransparency(this.Opacity);
          bool isNormalMatrix = this.ValidateTemplateMatrix(dictionary);
          RectangleF templateBounds = this.CalculateTemplateBounds(this.Bounds, page, template, isNormalMatrix);
          if (pdfGraphics != null)
            pdfGraphics.DrawPdfTemplate(template, templateBounds.Location, templateBounds.Size);
          else
            this.Page.Graphics.DrawPdfTemplate(template, templateBounds.Location, templateBounds.Size);
          page.Graphics.Restore(state);
          this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
        }
      }
    }
    else
    {
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
      PdfPen pen = new PdfPen(this.Color, this.Border.Width);
      if ((double) this.Opacity < 1.0)
      {
        PdfGraphicsState state = page.Graphics.Save();
        page.Graphics.SetTransparency(this.Opacity);
        if (pdfGraphics != null)
          pdfGraphics.DrawPath(pen, path);
        else
          page.Graphics.DrawPath(pen, path);
        page.Graphics.Restore(state);
      }
      else if (pdfGraphics != null)
        pdfGraphics.DrawPath(pen, path);
      else
        this.Page.Graphics.DrawPath(pen, path);
    }
    if (!this.FlattenPopUps)
      return;
    this.FlattenLoadedPopup();
  }
}
