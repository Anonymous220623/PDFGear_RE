// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfUnitConvertor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using System;
using System.Drawing;
using System.Security;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

[SecurityCritical]
public class PdfUnitConvertor
{
  internal static readonly float HorizontalResolution = 96f;
  internal static readonly float VerticalResolution = 96f;
  internal static readonly float HorizontalSize;
  internal static readonly float VerticalSize;
  internal static readonly float PxHorizontalResolution;
  internal static readonly float PxVerticalResolution;
  private double[] m_proportions;

  static PdfUnitConvertor()
  {
    IntPtr dc = GdiApi.CreateDC("DISPLAY", (string) null, (string) null, IntPtr.Zero);
    PdfUnitConvertor.HorizontalResolution = (float) GdiApi.GetDeviceCaps(dc, 88);
    PdfUnitConvertor.VerticalResolution = (float) GdiApi.GetDeviceCaps(dc, 90);
    PdfUnitConvertor.HorizontalSize = (float) GdiApi.GetDeviceCaps(dc, 4);
    PdfUnitConvertor.VerticalSize = (float) GdiApi.GetDeviceCaps(dc, 6);
    PdfUnitConvertor.PxHorizontalResolution = (float) GdiApi.GetDeviceCaps(dc, 8);
    PdfUnitConvertor.PxVerticalResolution = (float) GdiApi.GetDeviceCaps(dc, 10);
    GdiApi.DeleteDC(dc);
  }

  public PdfUnitConvertor() => this.UpdateProportions(PdfUnitConvertor.HorizontalResolution);

  public PdfUnitConvertor(float dpi) => this.UpdateProportions(dpi);

  public PdfUnitConvertor(System.Drawing.Graphics g)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    this.UpdateProportions(g.DpiX);
  }

  public float ConvertUnits(float value, PdfGraphicsUnit from, PdfGraphicsUnit to)
  {
    return this.ConvertFromPixels(this.ConvertToPixels(value, from), to);
  }

  public float ConvertToPixels(float value, PdfGraphicsUnit from)
  {
    int index = (int) from;
    return value * (float) this.m_proportions[index];
  }

  public RectangleF ConvertToPixels(RectangleF rect, PdfGraphicsUnit from)
  {
    return new RectangleF(this.ConvertToPixels(rect.X, from), this.ConvertToPixels(rect.Y, from), this.ConvertToPixels(rect.Width, from), this.ConvertToPixels(rect.Height, from));
  }

  public PointF ConvertToPixels(PointF point, PdfGraphicsUnit from)
  {
    return new PointF(this.ConvertToPixels(point.X, from), this.ConvertToPixels(point.Y, from));
  }

  public SizeF ConvertToPixels(SizeF size, PdfGraphicsUnit from)
  {
    return new SizeF(this.ConvertToPixels(size.Width, from), this.ConvertToPixels(size.Height, from));
  }

  public float ConvertFromPixels(float value, PdfGraphicsUnit to)
  {
    int index = (int) to;
    return value / (float) this.m_proportions[index];
  }

  public RectangleF ConvertFromPixels(RectangleF rect, PdfGraphicsUnit to)
  {
    return new RectangleF(this.ConvertFromPixels(rect.X, to), this.ConvertFromPixels(rect.Y, to), this.ConvertFromPixels(rect.Width, to), this.ConvertFromPixels(rect.Height, to));
  }

  public PointF ConvertFromPixels(PointF point, PdfGraphicsUnit to)
  {
    return new PointF(this.ConvertFromPixels(point.X, to), this.ConvertFromPixels(point.Y, to));
  }

  public SizeF ConvertFromPixels(SizeF size, PdfGraphicsUnit to)
  {
    return new SizeF(this.ConvertFromPixels(size.Width, to), this.ConvertFromPixels(size.Height, to));
  }

  private void UpdateProportions(float pixelPerInch)
  {
    this.m_proportions = new double[7]
    {
      (double) pixelPerInch / 2.54,
      (double) pixelPerInch / 6.0,
      1.0,
      (double) pixelPerInch / 72.0,
      (double) pixelPerInch,
      (double) pixelPerInch / 300.0,
      (double) pixelPerInch / 25.4
    };
  }
}
