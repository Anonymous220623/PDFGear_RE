// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfPen
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.ColorSpace;
using Syncfusion.Pdf.IO;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfPen : ICloneable
{
  private PdfColor m_color = PdfColor.Empty;
  private float m_dashOffset;
  private float[] m_dashPattern = new float[0];
  private PdfDashStyle m_dashStyle;
  private PdfLineCap m_lineCap;
  private PdfLineJoin m_lineJoin;
  private float m_width = 1f;
  private PdfBrush m_brush;
  private float m_miterLimit;
  private PdfColorSpace m_colorSpace;
  private PdfExtendedColor m_colorspaces;
  private bool m_bImmutable;
  private float[] m_comoundArray;
  private PdfCustomLineCap m_customStartCap;
  private PdfCustomLineCap m_customEndCap;
  private PdfLineCap m_StartCap;
  private PdfLineCap m_EndCap;
  internal bool isSkipPatternWidth;

  internal PdfExtendedColor Colorspaces
  {
    get => this.m_colorspaces;
    set => this.m_colorspaces = value;
  }

  public PdfBrush Brush
  {
    get
    {
      PdfBrush brush = this.m_brush != null ? this.m_brush.Clone() : (PdfBrush) null;
      if (this.m_brush != null)
        this.ResetStroking(brush);
      return this.m_brush;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Brush));
      this.CheckImmutability(nameof (Brush));
      this.AssignBrush(value);
    }
  }

  public PdfColor Color
  {
    get => this.m_color;
    set
    {
      this.CheckImmutability(nameof (Color));
      this.m_color = value;
    }
  }

  public float DashOffset
  {
    get => this.m_dashOffset;
    set
    {
      this.CheckImmutability(nameof (DashOffset));
      this.m_dashOffset = value;
    }
  }

  public float[] DashPattern
  {
    get => this.m_dashPattern;
    set
    {
      if (this.DashStyle == PdfDashStyle.Solid)
        throw new ArgumentException("This operation is not allowed. Set Custom dash style to change the pattern.");
      this.CheckImmutability(nameof (DashPattern));
      this.m_dashPattern = value;
    }
  }

  public PdfDashStyle DashStyle
  {
    get => this.m_dashStyle;
    set
    {
      this.CheckImmutability(nameof (DashStyle));
      if (this.m_dashStyle == value)
        return;
      this.m_dashStyle = value;
      switch (this.m_dashStyle)
      {
        case PdfDashStyle.Dash:
          this.m_dashPattern = new float[2]{ 3f, 1f };
          break;
        case PdfDashStyle.Dot:
          this.m_dashPattern = new float[2]{ 1f, 1f };
          break;
        case PdfDashStyle.DashDot:
          this.m_dashPattern = new float[4]
          {
            3f,
            1f,
            1f,
            1f
          };
          break;
        case PdfDashStyle.DashDotDot:
          this.m_dashPattern = new float[6]
          {
            3f,
            1f,
            1f,
            1f,
            1f,
            1f
          };
          break;
        case PdfDashStyle.Custom:
          break;
        default:
          this.m_dashStyle = PdfDashStyle.Solid;
          this.m_dashPattern = new float[0];
          break;
      }
    }
  }

  public PdfLineCap LineCap
  {
    get => this.m_lineCap;
    set
    {
      this.CheckImmutability(nameof (LineCap));
      this.m_lineCap = value;
    }
  }

  public PdfLineJoin LineJoin
  {
    get => this.m_lineJoin;
    set
    {
      this.CheckImmutability(nameof (LineJoin));
      this.m_lineJoin = value;
    }
  }

  public float Width
  {
    get => this.m_width;
    set
    {
      this.CheckImmutability(nameof (Width));
      this.m_width = value;
    }
  }

  public float MiterLimit
  {
    get => this.m_miterLimit;
    set
    {
      this.CheckImmutability(nameof (MiterLimit));
      this.m_miterLimit = value;
    }
  }

  internal bool IsImmutable => this.m_bImmutable;

  internal float[] CompoundArray
  {
    get => this.m_comoundArray;
    set
    {
      this.m_comoundArray = value;
      this.CreateCompoundPen();
    }
  }

  internal PdfCustomLineCap CustomStartCap
  {
    get => this.m_customStartCap;
    set => this.m_customStartCap = value;
  }

  internal PdfCustomLineCap CustomEndCap
  {
    get => this.m_customEndCap;
    set => this.m_customEndCap = value;
  }

  internal PdfLineCap StartCap
  {
    get => this.m_StartCap;
    set
    {
      this.m_StartCap = value;
      this.LineCap = this.m_StartCap;
    }
  }

  internal PdfLineCap EndCap
  {
    get => this.m_EndCap;
    set
    {
      this.m_EndCap = value;
      this.LineCap = this.m_EndCap;
    }
  }

  private PdfPen()
  {
  }

  public PdfPen(PdfColor color) => this.Color = color;

  public PdfPen(System.Drawing.Color color) => this.Color = new PdfColor(color);

  public PdfPen(PdfColor color, float width)
    : this(color)
  {
    this.Width = width;
  }

  public PdfPen(System.Drawing.Color color, float width)
    : this(color)
  {
    this.Width = width;
  }

  public PdfPen(PdfBrush brush)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    this.AssignBrush(brush);
  }

  public PdfPen(PdfBrush brush, float width)
    : this(brush)
  {
    this.Width = width;
  }

  internal PdfPen(PdfColor color, bool immutable)
    : this(color)
  {
    this.m_bImmutable = immutable;
  }

  public PdfPen(PdfExtendedColor color)
  {
    PdfColorSpaces colorSpace1 = color.ColorSpace;
    this.m_colorspaces = color;
    PdfColorSpaces colorSpace2 = color.ColorSpace;
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

  object ICloneable.Clone() => (object) this.Clone();

  public PdfPen Clone() => this.MemberwiseClone() as PdfPen;

  private void AssignBrush(PdfBrush brush)
  {
    if (brush is PdfSolidBrush pdfSolidBrush)
    {
      this.Color = pdfSolidBrush.Color;
      this.m_brush = (PdfBrush) pdfSolidBrush;
    }
    else
    {
      this.m_brush = brush.Clone();
      this.SetStrokingToBrush(this.m_brush);
    }
  }

  private void SetStrokingToBrush(PdfBrush brush)
  {
    PdfTilingBrush pdfTilingBrush = brush as PdfTilingBrush;
    PdfGradientBrush pdfGradientBrush = brush as PdfGradientBrush;
    if (pdfTilingBrush != null)
      pdfTilingBrush.Stroking = true;
    else if (pdfGradientBrush != null)
      pdfGradientBrush.Stroking = true;
    else if (!(brush is PdfSolidBrush))
      throw new ArgumentException("Unsupported brush.", nameof (brush));
  }

  private void ResetStroking(PdfBrush brush)
  {
    PdfTilingBrush brush1 = this.m_brush as PdfTilingBrush;
    PdfGradientBrush brush2 = this.m_brush as PdfGradientBrush;
    if (brush1 != null)
      brush1.Stroking = false;
    else if (brush2 != null)
      brush2.Stroking = false;
    else if (!(brush is PdfSolidBrush))
      throw new ArgumentException("Unsupported brush.", nameof (brush));
  }

  internal bool MonitorChanges(
    PdfPen currentPen,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveState,
    PdfColorSpace currentColorSpace,
    PdfTransformationMatrix matrix)
  {
    bool flag1 = false;
    saveState = true;
    if (currentPen == null)
      flag1 = true;
    bool flag2 = this.DashControl(currentPen, saveState, streamWriter);
    if (saveState || (double) this.Width != (double) currentPen.Width)
    {
      streamWriter.SetLineWidth(this.Width);
      flag2 = true;
    }
    if (saveState || this.LineJoin != currentPen.LineJoin)
    {
      streamWriter.SetLineJoin(this.LineJoin);
      flag2 = true;
    }
    if (saveState || this.LineCap != currentPen.LineCap)
    {
      streamWriter.SetLineCap(this.LineCap);
      flag2 = true;
    }
    if (saveState || (double) this.MiterLimit != (double) currentPen.MiterLimit)
    {
      float miterLimit = this.MiterLimit;
      if ((double) miterLimit > 0.0)
      {
        streamWriter.SetMiterLimit(miterLimit);
        flag2 = true;
      }
    }
    if (saveState || this.Color != currentPen.Color || this.Brush != currentPen.Brush || this.m_colorSpace != currentColorSpace)
    {
      PdfBrush brush1 = this.m_brush;
      switch (brush1)
      {
        case null:
        case PdfSolidBrush _:
          if (this.Colorspaces == null)
          {
            streamWriter.SetColorAndSpace(this.Color, currentColorSpace, true);
            flag2 = true;
            break;
          }
          streamWriter.SetColorAndSpace(this.Color, currentColorSpace, true, true);
          flag2 = true;
          break;
        default:
          PdfBrush brush2 = brush1.Clone();
          this.SetStrokingToBrush(brush2);
          if (brush2 is PdfGradientBrush pdfGradientBrush)
          {
            PdfTransformationMatrix matrix1 = pdfGradientBrush.Matrix;
            if (matrix1 != null)
              matrix.Multiply(matrix1);
            pdfGradientBrush.Matrix = matrix;
          }
          PdfBrush brush3 = currentPen?.Brush;
          flag2 |= brush2.MonitorChanges(brush3, streamWriter, getResources, saveState, currentColorSpace);
          break;
      }
    }
    return flag2;
  }

  internal bool MonitorChanges(
    PdfPen currentPen,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveState,
    PdfColorSpace currentColorSpace,
    PdfTransformationMatrix matrix,
    bool iccBased)
  {
    bool flag1 = false;
    saveState = true;
    if (currentPen == null)
      flag1 = true;
    bool flag2 = this.DashControl(currentPen, saveState, streamWriter);
    if (saveState || (double) this.Width != (double) currentPen.Width)
    {
      streamWriter.SetLineWidth(this.Width);
      flag2 = true;
    }
    if (saveState || this.LineJoin != currentPen.LineJoin)
    {
      streamWriter.SetLineJoin(this.LineJoin);
      flag2 = true;
    }
    if (saveState || this.LineCap != currentPen.LineCap)
    {
      streamWriter.SetLineCap(this.LineCap);
      flag2 = true;
    }
    if (saveState || (double) this.MiterLimit != (double) currentPen.MiterLimit)
    {
      float miterLimit = this.MiterLimit;
      if ((double) miterLimit > 0.0)
      {
        streamWriter.SetMiterLimit(miterLimit);
        flag2 = true;
      }
    }
    if (saveState || this.Color != currentPen.Color || this.Brush != currentPen.Brush || this.m_colorSpace != currentColorSpace)
    {
      PdfBrush brush1 = this.m_brush;
      if (brush1 != null)
      {
        PdfBrush brush2 = brush1.Clone();
        this.SetStrokingToBrush(brush2);
        if (brush2 is PdfGradientBrush pdfGradientBrush)
        {
          PdfTransformationMatrix matrix1 = pdfGradientBrush.Matrix;
          if (matrix1 != null)
            matrix.Multiply(matrix1);
          pdfGradientBrush.Matrix = matrix;
        }
        PdfBrush brush3 = currentPen?.Brush;
        flag2 |= brush2.MonitorChanges(brush3, streamWriter, getResources, saveState, currentColorSpace);
      }
      else if (this.Colorspaces == null)
      {
        streamWriter.SetColorAndSpace(this.Color, currentColorSpace, true);
        flag2 = true;
      }
      else if (this.Colorspaces is PdfIndexedColor)
      {
        streamWriter.SetColorAndSpace(this.Color, currentColorSpace, true, true, true, true);
        flag2 = true;
      }
      else
      {
        streamWriter.SetColorAndSpace(this.Color, currentColorSpace, true, true, true);
        flag2 = true;
      }
    }
    return flag2;
  }

  internal float[] GetPattern()
  {
    float[] pattern = this.DashPattern.Clone() as float[];
    if (!this.isSkipPatternWidth)
    {
      for (int index = 0; index < pattern.Length; ++index)
        pattern[index] *= this.Width;
      this.isSkipPatternWidth = false;
    }
    return pattern;
  }

  private bool DashControl(PdfPen pen, bool saveState, PdfStreamWriter streamWriter)
  {
    if (pen != null)
      saveState |= (double) this.DashOffset != (double) pen.DashOffset | this.DashPattern != pen.DashPattern | this.DashStyle != pen.DashStyle | (double) this.Width != (double) pen.Width;
    else
      saveState = true;
    if (saveState)
    {
      float width = this.Width;
      float[] pattern = this.GetPattern();
      streamWriter.SetLineDashPattern(pattern, this.DashOffset * width);
    }
    return saveState;
  }

  private void CheckImmutability(string propertyName)
  {
    if (this.m_bImmutable)
      throw new ArgumentException("The immutable object can't be changed", propertyName);
  }

  private void CreateCompoundPen()
  {
    PdfTilingBrush pdfTilingBrush = new PdfTilingBrush(new SizeF(this.Width, this.Width));
    float num1 = 0.0f;
    float num2 = 0.0f;
    for (int index = 0; index < this.CompoundArray.Length; index += 2)
    {
      PdfPen pen = new PdfPen(this.Color);
      pen.Width = (this.CompoundArray[index + 1] - this.CompoundArray[index]) * pdfTilingBrush.Size.Width;
      if (index != 0)
        num1 += (this.CompoundArray[index] - this.CompoundArray[index - 1]) * pdfTilingBrush.Size.Width + num2;
      pdfTilingBrush.Graphics.DrawLine(pen, 0.0f, num1, pdfTilingBrush.Size.Width, num1);
      num2 = pen.Width;
    }
    this.Brush = (PdfBrush) pdfTilingBrush;
  }
}
