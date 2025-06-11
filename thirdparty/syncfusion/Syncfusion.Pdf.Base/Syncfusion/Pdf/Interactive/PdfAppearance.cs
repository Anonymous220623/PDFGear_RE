// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAppearance
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfAppearance : IPdfWrapper
{
  private PdfTemplate m_templateNormal;
  private PdfTemplate m_templateMouseHover;
  private PdfTemplate m_templatePressed;
  private PdfAnnotation m_annotation;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfTemplate m_appearanceLayer;
  internal bool IsCompletedValidationAppearance;

  public PdfTemplate Normal
  {
    get
    {
      if (this.m_templateNormal == null)
      {
        this.m_templateNormal = new PdfTemplate(this.GetRotatedBounds(this.m_annotation.Bounds, (float) ((int) this.m_annotation.Rotate * 90)).Size);
        this.m_dictionary.SetProperty("N", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_templateNormal));
      }
      return this.m_templateNormal != null && this.m_templateNormal.IsSignatureAppearanceValidation && !this.IsCompletedValidationAppearance ? this.AppearanceLayer : this.m_templateNormal;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Normal));
      if (this.m_templateNormal == value)
        return;
      this.m_templateNormal = value;
      this.m_dictionary.SetProperty("N", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_templateNormal));
    }
  }

  public PdfTemplate MouseHover
  {
    get
    {
      if (this.m_templateMouseHover == null)
      {
        this.m_templateMouseHover = new PdfTemplate(this.m_annotation.Size);
        this.m_dictionary.SetProperty("R", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_templateMouseHover));
      }
      return this.m_templateMouseHover;
    }
    set
    {
      if (this.m_templateMouseHover == value)
        return;
      this.m_templateMouseHover = value;
      this.m_dictionary.SetProperty("R", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_templateMouseHover));
    }
  }

  public PdfTemplate Pressed
  {
    get
    {
      if (this.m_templatePressed == null)
      {
        this.m_templatePressed = new PdfTemplate(this.m_annotation.Size);
        this.m_dictionary.SetProperty("D", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_templatePressed));
      }
      return this.m_templatePressed;
    }
    set
    {
      if (value == this.m_templatePressed)
        return;
      this.m_templatePressed = value;
      this.m_dictionary.SetProperty("D", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_templatePressed));
    }
  }

  internal PdfTemplate AppearanceLayer
  {
    get
    {
      if (this.m_appearanceLayer == null)
      {
        PdfPageBase pdfPageBase = this.m_annotation.Page == null ? (PdfPageBase) this.m_annotation.LoadedPage : (PdfPageBase) this.m_annotation.Page;
        this.m_appearanceLayer = pdfPageBase == null || pdfPageBase.Rotation != PdfPageRotateAngle.RotateAngle90 && pdfPageBase.Rotation != PdfPageRotateAngle.RotateAngle270 ? new PdfTemplate(this.m_annotation.Size.Width, this.m_annotation.Size.Height) : new PdfTemplate(this.m_annotation.Size.Height, this.m_annotation.Size.Width);
        this.m_appearanceLayer.CustomPdfTemplateName = "n2";
      }
      return this.m_appearanceLayer;
    }
  }

  public PdfAppearance(PdfAnnotation annotation) => this.m_annotation = annotation;

  internal PdfTemplate GetNormalTemplate() => this.m_templateNormal;

  internal PdfTemplate GetPressedTemplate() => this.m_templatePressed;

  internal RectangleF GetRotatedBounds(RectangleF bounds, float angle)
  {
    if ((double) bounds.Width <= 0.0 || (double) bounds.Height <= 0.0)
      return bounds;
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddRectangle(bounds);
    graphicsPath.Transform(this.GetRotatedTransformMatrix(bounds, angle));
    RectangleF boundingBox = this.CalculateBoundingBox(graphicsPath.PathPoints);
    graphicsPath.Dispose();
    return boundingBox;
  }

  internal Matrix GetRotatedTransformMatrix(RectangleF bounds, float angle)
  {
    Matrix rotatedTransformMatrix = new Matrix();
    PointF point = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
    rotatedTransformMatrix.RotateAt(angle, point, MatrixOrder.Append);
    return rotatedTransformMatrix;
  }

  internal RectangleF CalculateBoundingBox(PointF[] imageCoordinates)
  {
    float x1 = imageCoordinates[0].X;
    float x2 = imageCoordinates[3].X;
    float y1 = imageCoordinates[0].Y;
    float y2 = imageCoordinates[3].Y;
    for (int index = 0; index < 4; ++index)
    {
      if ((double) imageCoordinates[index].X < (double) x1)
        x1 = imageCoordinates[index].X;
      if ((double) imageCoordinates[index].X > (double) x2)
        x2 = imageCoordinates[index].X;
      if ((double) imageCoordinates[index].Y < (double) y1)
        y1 = imageCoordinates[index].Y;
      if ((double) imageCoordinates[index].Y > (double) y2)
        y2 = imageCoordinates[index].Y;
    }
    return new RectangleF(x1, y1, x2 - x1, y2 - y1);
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
