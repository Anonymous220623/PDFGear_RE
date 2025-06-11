// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfGradientBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Functions;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfGradientBrush : PdfBrush, IPdfWrapper
{
  private PdfColor m_background;
  private bool m_bStroking;
  private PdfDictionary m_patternDictionary;
  private PdfDictionary m_shading;
  private PdfTransformationMatrix m_matrix;
  private PdfExternalGraphicsState m_externalState;
  private PdfColorSpace m_colorSpace;
  private PdfFunction m_function;

  internal PdfGradientBrush(PdfDictionary shading)
  {
    if (shading == null)
      throw new ArgumentNullException(nameof (shading));
    this.m_patternDictionary = new PdfDictionary();
    this.m_patternDictionary["Type"] = (IPdfPrimitive) new PdfName("Pattern");
    this.m_patternDictionary["PatternType"] = (IPdfPrimitive) new PdfNumber(2);
    this.Shading = shading;
    this.ColorSpace = PdfColorSpace.RGB;
  }

  public PdfColor Background
  {
    get => this.m_background;
    set
    {
      this.m_background = value;
      PdfDictionary shading = this.Shading;
      if (value == PdfColor.Empty)
        shading.Remove(nameof (Background));
      else
        shading[nameof (Background)] = (IPdfPrimitive) value.ToArray(this.ColorSpace);
    }
  }

  public bool AntiAlias
  {
    get => (this.Shading[nameof (AntiAlias)] as PdfBoolean).Value;
    set
    {
      PdfDictionary shading = this.Shading;
      if (!(shading[nameof (AntiAlias)] is PdfBoolean pdfBoolean1))
      {
        PdfBoolean pdfBoolean = new PdfBoolean(value);
        shading[nameof (AntiAlias)] = (IPdfPrimitive) pdfBoolean;
      }
      else
        pdfBoolean1.Value = value;
    }
  }

  internal PdfFunction Function
  {
    get => this.m_function;
    set
    {
      this.m_function = value;
      if (value != null)
        this.Shading[nameof (Function)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_function);
      else
        this.Shading.Remove(nameof (Function));
    }
  }

  internal PdfArray BBox
  {
    get => this.Shading[nameof (BBox)] as PdfArray;
    set
    {
      PdfDictionary shading = this.Shading;
      if (value == null)
        shading.Remove(nameof (BBox));
      else
        shading[nameof (BBox)] = (IPdfPrimitive) value;
    }
  }

  internal PdfColorSpace ColorSpace
  {
    get => this.m_colorSpace;
    set
    {
      IPdfPrimitive pdfPrimitive = this.Shading[nameof (ColorSpace)];
      if (value == this.m_colorSpace && pdfPrimitive != null)
        return;
      this.m_colorSpace = value;
      string deviceName = PdfGradientBrush.ColorSpaceToDeviceName(value);
      if (pdfPrimitive != null)
      {
        PdfName pdfName = pdfPrimitive as PdfName;
        if (pdfName != (PdfName) null)
          pdfName.Value = deviceName;
        else
          this.Shading[nameof (ColorSpace)] = (IPdfPrimitive) new PdfName(deviceName);
      }
      else
        this.Shading[nameof (ColorSpace)] = (IPdfPrimitive) new PdfName(deviceName);
    }
  }

  internal bool Stroking
  {
    get => this.m_bStroking;
    set => this.m_bStroking = value;
  }

  internal PdfDictionary PatternDictionary
  {
    get
    {
      if (this.m_patternDictionary == null)
        this.m_patternDictionary = new PdfDictionary();
      return this.m_patternDictionary;
    }
  }

  internal PdfDictionary Shading
  {
    get => this.m_shading;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Shading));
      if (value == this.m_shading)
        return;
      this.m_shading = value;
      this.PatternDictionary[nameof (Shading)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_shading);
    }
  }

  internal PdfTransformationMatrix Matrix
  {
    get => this.m_matrix;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Matrix));
      if (value == this.m_matrix)
        return;
      this.m_matrix = value.Clone();
      this.m_patternDictionary[nameof (Matrix)] = (IPdfPrimitive) new PdfArray(this.m_matrix.Matrix.Elements);
    }
  }

  internal PdfExternalGraphicsState ExternalState
  {
    get => this.m_externalState;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (ExternalState));
      if (value == this.m_externalState)
        return;
      this.m_externalState = value;
      this.m_patternDictionary["ExtGState"] = this.m_externalState.Element;
    }
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace)
  {
    bool flag = false;
    if (brush != this)
    {
      if (this.ColorSpace != currentColorSpace)
      {
        this.ColorSpace = currentColorSpace;
        this.ResetFunction();
      }
      streamWriter.SetColorSpace("Pattern", this.m_bStroking);
      PdfName name = getResources().GetName((IPdfWrapper) this);
      streamWriter.SetColourWithPattern((IList) null, name, this.m_bStroking);
      flag = true;
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
    bool flag = false;
    if (brush != this)
    {
      if (this.ColorSpace != currentColorSpace)
      {
        this.ColorSpace = currentColorSpace;
        this.ResetFunction();
      }
      streamWriter.SetColorSpace("Pattern", this.m_bStroking);
      PdfName name = getResources().GetName((IPdfWrapper) this);
      streamWriter.SetColourWithPattern((IList) null, name, this.m_bStroking);
      flag = true;
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
    bool flag = false;
    if (brush != this)
    {
      if (this.ColorSpace != currentColorSpace)
      {
        this.ColorSpace = currentColorSpace;
        this.ResetFunction();
      }
      streamWriter.SetColorSpace("Pattern", this.m_bStroking);
      PdfName name = getResources().GetName((IPdfWrapper) this);
      streamWriter.SetColourWithPattern((IList) null, name, this.m_bStroking);
      flag = true;
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
    bool flag = false;
    if (brush != this)
    {
      if (this.ColorSpace != currentColorSpace)
      {
        this.ColorSpace = currentColorSpace;
        this.ResetFunction();
      }
      streamWriter.SetColorSpace("Pattern", this.m_bStroking);
      PdfName name = getResources().GetName((IPdfWrapper) this);
      streamWriter.SetColourWithPattern((IList) null, name, this.m_bStroking);
      flag = true;
    }
    return flag;
  }

  internal override void ResetChanges(PdfStreamWriter streamWriter)
  {
  }

  internal static string ColorSpaceToDeviceName(PdfColorSpace colorSpace)
  {
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
        return "DeviceRGB";
      case PdfColorSpace.CMYK:
        return "DeviceCMYK";
      case PdfColorSpace.GrayScale:
        return "DeviceGray";
      default:
        throw new ArgumentException("Unsupported colour space: " + (object) colorSpace, nameof (colorSpace));
    }
  }

  internal void ResetPatternDictionary(PdfDictionary dictionary)
  {
    this.m_patternDictionary = dictionary;
  }

  internal abstract void ResetFunction();

  protected void CloneAntiAliasingValue(PdfGradientBrush brush)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    if (!(this.Shading["AntiAlias"] is PdfBoolean pdfBoolean))
      return;
    brush.Shading["AntiAlias"] = (IPdfPrimitive) new PdfBoolean(pdfBoolean.Value);
  }

  protected void CloneBackgroundValue(PdfGradientBrush brush)
  {
    PdfColor background = this.Background;
    if (background.IsEmpty)
      return;
    brush.Background = background;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_patternDictionary;
}
