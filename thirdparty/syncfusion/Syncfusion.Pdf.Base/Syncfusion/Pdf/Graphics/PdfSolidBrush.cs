// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfSolidBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.ColorSpace;
using Syncfusion.Pdf.IO;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public sealed class PdfSolidBrush : PdfBrush
{
  private PdfColor m_color;
  private PdfColorSpace m_colorSpace;
  private bool m_bImmutable;
  private PdfExtendedColor m_colorspaces;

  public PdfSolidBrush(PdfColor color) => this.m_color = color;

  public PdfSolidBrush(PdfExtendedColor color)
  {
    PdfColorSpaces colorSpace = color.ColorSpace;
    this.m_colorspaces = color;
    switch (color)
    {
      case PdfCalRGBColor _:
        PdfCalRGBColor pdfCalRgbColor = color as PdfCalRGBColor;
        this.m_color = new PdfColor((byte) pdfCalRgbColor.Red, (byte) pdfCalRgbColor.Green, (byte) pdfCalRgbColor.Blue);
        break;
      case PdfCalGrayColor _:
        PdfCalGrayColor pdfCalGrayColor = color as PdfCalGrayColor;
        this.m_color = new PdfColor((float) (byte) pdfCalGrayColor.Gray);
        this.m_color.Gray = Convert.ToSingle(pdfCalGrayColor.Gray);
        break;
      case PdfLabColor _:
        PdfLabColor pdfLabColor = color as PdfLabColor;
        this.m_color = new PdfColor((byte) pdfLabColor.L, (byte) pdfLabColor.A, (byte) pdfLabColor.B);
        break;
      case PdfICCColor _:
        PdfICCColor pdfIccColor = color as PdfICCColor;
        if (pdfIccColor.ColorSpaces.AlternateColorSpace is PdfCalGrayColorSpace)
        {
          this.m_color = new PdfColor((float) (byte) pdfIccColor.ColorComponents[0]);
          this.m_color.Gray = Convert.ToSingle(pdfIccColor.ColorComponents[0]);
          break;
        }
        if (pdfIccColor.ColorSpaces.AlternateColorSpace is PdfCalRGBColorSpace)
        {
          this.m_color = new PdfColor((byte) pdfIccColor.ColorComponents[0], (byte) pdfIccColor.ColorComponents[1], (byte) pdfIccColor.ColorComponents[2]);
          break;
        }
        if (pdfIccColor.ColorSpaces.AlternateColorSpace is PdfLabColorSpace)
        {
          this.m_color = new PdfColor((byte) pdfIccColor.ColorComponents[0], (byte) pdfIccColor.ColorComponents[1], (byte) pdfIccColor.ColorComponents[2]);
          break;
        }
        if (pdfIccColor.ColorSpaces.AlternateColorSpace is PdfDeviceColorSpace)
        {
          switch ((pdfIccColor.ColorSpaces.AlternateColorSpace as PdfDeviceColorSpace).DeviceColorSpaceType.ToString())
          {
            case "RGB":
              this.m_color = new PdfColor((byte) pdfIccColor.ColorComponents[0], (byte) pdfIccColor.ColorComponents[1], (byte) pdfIccColor.ColorComponents[2]);
              return;
            case "GrayScale":
              this.m_color = new PdfColor((float) (byte) pdfIccColor.ColorComponents[0]);
              this.m_color.Gray = Convert.ToSingle(pdfIccColor.ColorComponents[0]);
              return;
            case "CMYK":
              this.m_color = new PdfColor((float) pdfIccColor.ColorComponents[0], (float) pdfIccColor.ColorComponents[1], (float) pdfIccColor.ColorComponents[2], (float) pdfIccColor.ColorComponents[3]);
              return;
            default:
              return;
          }
        }
        else
        {
          this.m_color = new PdfColor((byte) pdfIccColor.ColorComponents[0], (byte) pdfIccColor.ColorComponents[1], (byte) pdfIccColor.ColorComponents[2]);
          break;
        }
      case PdfSeparationColor _:
        this.m_color.Gray = (float) (color as PdfSeparationColor).Tint;
        break;
      case PdfIndexedColor _:
        this.m_color.G = (byte) (color as PdfIndexedColor).SelectColorIndex;
        break;
    }
  }

  internal PdfSolidBrush(PdfColor color, bool immutable)
    : this(color)
  {
    this.m_bImmutable = immutable;
  }

  private PdfSolidBrush()
    : this(new PdfColor((byte) 0, (byte) 0, (byte) 0))
  {
  }

  public PdfColor Color
  {
    get => this.m_color;
    set
    {
      if (this.m_bImmutable)
        throw new ArgumentException("Can't change immutable object.", nameof (Color));
      this.m_color = value;
    }
  }

  internal PdfExtendedColor Colorspaces
  {
    get => this.m_colorspaces;
    set => this.m_colorspaces = value;
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace)
  {
    if (streamWriter == null)
      throw new ArgumentNullException(nameof (streamWriter));
    if (getResources == null)
      throw new ArgumentNullException(nameof (getResources));
    bool flag = false;
    if (brush == null)
    {
      flag = true;
      streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false);
    }
    else if (brush != this)
    {
      if (brush is PdfSolidBrush pdfSolidBrush)
      {
        if (pdfSolidBrush.Color != this.Color || pdfSolidBrush.m_colorSpace != currentColorSpace)
        {
          flag = true;
          streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false);
        }
        else if (pdfSolidBrush.m_colorSpace == currentColorSpace && currentColorSpace == PdfColorSpace.RGB)
        {
          flag = true;
          streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false);
        }
      }
      else
      {
        brush.ResetChanges(streamWriter);
        streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false);
        flag = true;
      }
    }
    return flag;
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check)
  {
    if (streamWriter == null)
      throw new ArgumentNullException(nameof (streamWriter));
    if (getResources == null)
      throw new ArgumentNullException(nameof (getResources));
    bool flag = false;
    if (brush == null)
    {
      flag = true;
      streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false, false);
    }
    else if (brush != this)
    {
      if (brush is PdfSolidBrush pdfSolidBrush)
      {
        if (pdfSolidBrush.Color != this.Color || pdfSolidBrush.m_colorSpace != currentColorSpace)
        {
          flag = true;
          streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false, false);
        }
      }
      else
      {
        brush.ResetChanges(streamWriter);
        streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false);
        flag = true;
      }
    }
    return flag;
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check,
    bool iccbased)
  {
    if (streamWriter == null)
      throw new ArgumentNullException(nameof (streamWriter));
    if (getResources == null)
      throw new ArgumentNullException(nameof (getResources));
    bool flag = false;
    if (brush == null)
    {
      flag = true;
      streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false, false, false);
    }
    else if (brush != this)
    {
      if (brush is PdfSolidBrush pdfSolidBrush)
      {
        if (pdfSolidBrush.Color != this.Color || pdfSolidBrush.m_colorSpace != currentColorSpace)
        {
          flag = true;
          streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false, false, false);
        }
        else if (pdfSolidBrush.m_colorSpace == currentColorSpace && currentColorSpace == PdfColorSpace.RGB)
        {
          flag = true;
          streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false, false, false);
        }
      }
      else
      {
        brush.ResetChanges(streamWriter);
        streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false);
        flag = true;
      }
    }
    return flag;
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check,
    bool iccbased,
    bool indexed)
  {
    if (streamWriter == null)
      throw new ArgumentNullException(nameof (streamWriter));
    if (getResources == null)
      throw new ArgumentNullException(nameof (getResources));
    bool flag = false;
    if (brush == null)
    {
      flag = true;
      streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false, false, false, false);
    }
    else if (brush != this)
    {
      if (brush is PdfSolidBrush pdfSolidBrush)
      {
        if (pdfSolidBrush.Color != this.Color || pdfSolidBrush.m_colorSpace != currentColorSpace)
        {
          flag = true;
          streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false, false, false, false);
        }
      }
      else
      {
        brush.ResetChanges(streamWriter);
        streamWriter.SetColorAndSpace(this.m_color, currentColorSpace, false);
        flag = true;
      }
    }
    return flag;
  }

  internal override void ResetChanges(PdfStreamWriter streamWriter)
  {
    streamWriter.SetColorAndSpace(new PdfColor((byte) 0, (byte) 0, (byte) 0), PdfColorSpace.RGB, false);
  }

  public override PdfBrush Clone() => (PdfBrush) (this.MemberwiseClone() as PdfSolidBrush);
}
