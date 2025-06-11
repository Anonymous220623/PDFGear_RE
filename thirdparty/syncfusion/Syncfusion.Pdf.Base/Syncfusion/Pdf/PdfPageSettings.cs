// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageSettings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageSettings : ICloneable
{
  private PdfPageOrientation m_orientation;
  private SizeF m_size = PdfPageSize.A4;
  private PdfMargins m_margins = new PdfMargins();
  private PdfPageRotateAngle m_rotateAngle;
  private PdfGraphicsUnit m_logicalUnit = PdfGraphicsUnit.Point;
  private PointF m_origin = PointF.Empty;
  internal bool m_isRotation;
  private bool m_isOrientation;
  private PdfPageTransition m_transition;

  public PdfPageOrientation Orientation
  {
    get => this.m_orientation;
    set
    {
      this.m_isOrientation = true;
      if (this.m_orientation == value)
        return;
      this.m_orientation = value;
      this.UpdateSize(value);
    }
  }

  public SizeF Size
  {
    get => this.m_size;
    set
    {
      if (this.m_isOrientation)
      {
        this.AssignSize(value);
      }
      else
      {
        this.m_size = value;
        this.AssignOrientation();
      }
    }
  }

  public float Width
  {
    get => this.m_size.Width;
    set
    {
      this.m_size.Width = value;
      this.AssignOrientation();
    }
  }

  public float Height
  {
    get => this.m_size.Height;
    set
    {
      this.m_size.Height = value;
      this.AssignOrientation();
    }
  }

  public PdfMargins Margins
  {
    get => this.m_margins;
    set => this.m_margins = value;
  }

  public PdfPageRotateAngle Rotate
  {
    get => this.m_rotateAngle;
    set
    {
      this.m_rotateAngle = value;
      this.m_isRotation = true;
    }
  }

  public PdfPageTransition Transition
  {
    get
    {
      if (this.m_transition == null)
        this.m_transition = new PdfPageTransition();
      return this.m_transition;
    }
    set => this.m_transition = value;
  }

  internal PdfGraphicsUnit Unit
  {
    get => this.m_logicalUnit;
    set => this.m_logicalUnit = value;
  }

  internal PointF Origin
  {
    get => this.m_origin;
    set => this.m_origin = value;
  }

  public PdfPageSettings()
  {
  }

  public PdfPageSettings(SizeF size)
  {
    this.m_size = size;
    this.AssignOrientation();
  }

  public PdfPageSettings(PdfPageOrientation pageOrientation)
  {
    this.m_orientation = pageOrientation;
    this.m_isOrientation = true;
    this.UpdateSize(pageOrientation);
  }

  public PdfPageSettings(SizeF size, PdfPageOrientation pageOrientation)
  {
    this.m_size = size;
    this.m_orientation = pageOrientation;
    this.m_isOrientation = true;
    this.UpdateSize(pageOrientation);
  }

  public PdfPageSettings(float margins) => this.m_margins.SetMargins(margins);

  public PdfPageSettings(float leftMargin, float topMargin, float rightMargin, float bottomMargin)
  {
    this.m_margins.SetMargins(leftMargin, topMargin, rightMargin, bottomMargin);
  }

  public PdfPageSettings(SizeF size, float margins)
  {
    this.m_size = size;
    this.m_margins.SetMargins(margins);
    this.AssignOrientation();
  }

  public PdfPageSettings(
    SizeF size,
    float leftMargin,
    float topMargin,
    float rightMargin,
    float bottomMargin)
  {
    this.m_size = size;
    this.m_margins.SetMargins(leftMargin, topMargin, rightMargin, bottomMargin);
    this.AssignOrientation();
  }

  public PdfPageSettings(SizeF size, PdfPageOrientation pageOrientation, float margins)
  {
    this.m_size = size;
    this.m_orientation = pageOrientation;
    this.m_isOrientation = true;
    this.m_margins.SetMargins(margins);
    this.UpdateSize(pageOrientation);
  }

  public PdfPageSettings(
    SizeF size,
    PdfPageOrientation pageOrientation,
    float leftMargin,
    float topMargin,
    float rightMargin,
    float bottomMargin)
  {
    this.m_size = size;
    this.m_orientation = pageOrientation;
    this.m_isOrientation = true;
    this.m_margins.SetMargins(leftMargin, topMargin, rightMargin, bottomMargin);
    this.UpdateSize(pageOrientation);
  }

  public void SetMargins(float margins) => this.m_margins.SetMargins(margins);

  public void SetMargins(float leftRight, float topBottom)
  {
    this.m_margins.SetMargins(leftRight, topBottom);
  }

  public void SetMargins(float left, float top, float right, float bottom)
  {
    this.m_margins.SetMargins(left, top, right, bottom);
  }

  public object Clone()
  {
    PdfPageSettings pdfPageSettings = (PdfPageSettings) this.MemberwiseClone();
    pdfPageSettings.m_margins = (PdfMargins) this.Margins.Clone();
    if (this.AssignTransition() != null)
      pdfPageSettings.Transition = (PdfPageTransition) this.Transition.Clone();
    return (object) pdfPageSettings;
  }

  internal SizeF GetActualSize()
  {
    return new SizeF(this.Width - (this.Margins.Left + this.Margins.Right), this.Height - (this.Margins.Top + this.Margins.Bottom));
  }

  internal PdfPageTransition AssignTransition() => this.m_transition;

  private void UpdateSize(PdfPageOrientation orientation)
  {
    float num1 = Math.Min(this.Width, this.Height);
    float num2 = Math.Max(this.Width, this.Height);
    switch (orientation)
    {
      case PdfPageOrientation.Portrait:
        this.Size = new SizeF(num1, num2);
        break;
      case PdfPageOrientation.Landscape:
        this.Size = new SizeF(num2, num1);
        break;
    }
  }

  private void AssignSize(SizeF size)
  {
    float num1 = Math.Min(size.Width, size.Height);
    float num2 = Math.Max(size.Width, size.Height);
    if (this.Orientation == PdfPageOrientation.Portrait)
      this.m_size = new SizeF(num1, num2);
    else
      this.m_size = new SizeF(num2, num1);
  }

  private void AssignOrientation()
  {
    this.m_orientation = (double) this.m_size.Height >= (double) this.m_size.Width ? PdfPageOrientation.Portrait : PdfPageOrientation.Landscape;
  }
}
