// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfDestination
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfDestination : IPdfWrapper
{
  private PdfDestinationMode m_destinationMode;
  private float m_zoom;
  private PointF m_location = PointF.Empty;
  private RectangleF m_bounds = RectangleF.Empty;
  private PdfPageBase m_page;
  private int m_index;
  private PdfArray m_array = new PdfArray();
  private bool m_isValid = true;

  public PdfDestination(PdfPageBase page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfPageRotateAngle pdfPageRotateAngle = PdfPageRotateAngle.RotateAngle0;
    if (page.Rotation != PdfPageRotateAngle.RotateAngle0 && page.Rotation != PdfPageRotateAngle.RotateAngle90)
      pdfPageRotateAngle = page.Rotation;
    if (page is PdfPage && (page as PdfPage).m_section != null)
    {
      PdfPageRotateAngle rotate = (page as PdfPage).Section.PageSettings.Rotate;
      switch (rotate)
      {
        case PdfPageRotateAngle.RotateAngle0:
        case PdfPageRotateAngle.RotateAngle90:
          break;
        default:
          if (rotate != pdfPageRotateAngle)
            break;
          break;
      }
    }
    this.m_location = page.Rotation != PdfPageRotateAngle.RotateAngle180 ? (page.Rotation != PdfPageRotateAngle.RotateAngle90 ? (page.Rotation != PdfPageRotateAngle.RotateAngle270 ? new PointF(0.0f, this.m_location.Y) : new PointF(page.Size.Width, 0.0f)) : new PointF(0.0f, 0.0f)) : new PointF(page.Size.Width, this.m_location.Y);
    this.m_page = page;
  }

  public PdfDestination(PdfPageBase page, PointF location)
    : this(page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    this.m_location = location;
  }

  internal PdfDestination(PdfPageBase page, RectangleF rect)
    : this(page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    this.m_location = rect.Location;
    this.m_bounds = rect;
  }

  public float Zoom
  {
    get => this.m_zoom;
    set
    {
      if ((double) this.m_zoom == (double) value)
        return;
      this.m_zoom = value;
      this.InitializePrimitive();
    }
  }

  public PdfPageBase Page
  {
    get => this.m_page;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Page));
      if (this.m_page == value)
        return;
      this.m_page = value;
      this.InitializePrimitive();
    }
  }

  public int PageIndex
  {
    get => this.m_index;
    internal set => this.m_index = value;
  }

  public PdfDestinationMode Mode
  {
    get => this.m_destinationMode;
    set
    {
      if (this.m_destinationMode == value)
        return;
      this.m_destinationMode = value;
      this.InitializePrimitive();
    }
  }

  public PointF Location
  {
    get => this.m_location;
    set
    {
      if (!(this.m_location != value))
        return;
      this.m_location = value;
      this.InitializePrimitive();
    }
  }

  internal RectangleF Bounds
  {
    get => this.m_bounds;
    set
    {
      if (!(this.m_bounds != value))
        return;
      this.m_bounds = value;
      this.InitializePrimitive();
    }
  }

  public bool IsValid => this.m_isValid;

  internal void SetValidation(bool valid) => this.m_isValid = valid;

  private PointF PointToNativePdf(PdfPage page, PointF point)
  {
    return page.Section.PointToNativePdf(page, point);
  }

  private void InitializePrimitive()
  {
    this.m_array.Clear();
    if (((IPdfWrapper) this.m_page).Element != null)
      this.m_array.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_page));
    switch (this.m_destinationMode)
    {
      case PdfDestinationMode.Location:
        PdfPage page1 = this.m_page as PdfPage;
        PointF pointF = PointF.Empty;
        if (page1 != null)
          pointF = page1.m_section == null ? this.m_location : this.PointToNativePdf(page1, this.m_location);
        else if (this.m_page is PdfLoadedPage page2)
        {
          pointF.X = this.m_location.X;
          pointF.Y = page2.Size.Height - this.m_location.Y;
        }
        this.m_array.Add((IPdfPrimitive) new PdfName("XYZ"));
        this.m_array.Add((IPdfPrimitive) new PdfNumber(pointF.X));
        this.m_array.Add((IPdfPrimitive) new PdfNumber(pointF.Y));
        this.m_array.Add((IPdfPrimitive) new PdfNumber(this.m_zoom));
        break;
      case PdfDestinationMode.FitToPage:
        this.m_array.Add((IPdfPrimitive) new PdfName("Fit"));
        break;
      case PdfDestinationMode.FitR:
        this.m_array.Add((IPdfPrimitive) new PdfName("FitR"));
        this.m_array.Add((IPdfPrimitive) new PdfNumber(this.m_bounds.X));
        this.m_array.Add((IPdfPrimitive) new PdfNumber(this.m_bounds.Y));
        this.m_array.Add((IPdfPrimitive) new PdfNumber(this.m_bounds.Width));
        this.m_array.Add((IPdfPrimitive) new PdfNumber(this.m_bounds.Height));
        break;
      case PdfDestinationMode.FitH:
        PdfLoadedPage page3 = this.m_page as PdfLoadedPage;
        PdfPage page4 = this.m_page as PdfPage;
        float num = page3 == null ? page4.Size.Height - this.m_location.Y : page3.Size.Height - this.m_location.Y;
        this.m_array.Add((IPdfPrimitive) new PdfName("FitH"));
        this.m_array.Add((IPdfPrimitive) new PdfNumber(num));
        break;
    }
  }

  private void Initialize()
  {
  }

  IPdfPrimitive IPdfWrapper.Element
  {
    get
    {
      this.InitializePrimitive();
      return (IPdfPrimitive) this.m_array;
    }
  }
}
