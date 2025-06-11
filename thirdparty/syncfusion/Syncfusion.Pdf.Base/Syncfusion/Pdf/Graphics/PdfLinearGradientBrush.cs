// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfLinearGradientBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfLinearGradientBrush : PdfGradientBrush
{
  private PointF m_pointStart;
  private PointF m_pointEnd;
  private PdfColor[] m_colours;
  private PdfColorBlend m_colourBlend;
  private PdfBlend m_blend;
  private RectangleF m_boundaries;

  public PdfLinearGradientBrush(PointF point1, PointF point2, PdfColor color1, PdfColor color2)
    : this(color1, color2)
  {
    this.m_pointStart = point1;
    this.m_pointEnd = point2;
    this.SetPoints(this.m_pointStart, this.m_pointEnd);
  }

  public PdfLinearGradientBrush(
    RectangleF rect,
    PdfColor color1,
    PdfColor color2,
    PdfLinearGradientMode mode)
    : this(color1, color2)
  {
    this.m_boundaries = rect;
    switch (mode)
    {
      case PdfLinearGradientMode.BackwardDiagonal:
        this.m_pointStart = new PointF(rect.Right, rect.Top);
        this.m_pointEnd = new PointF(rect.Left, rect.Bottom);
        break;
      case PdfLinearGradientMode.ForwardDiagonal:
        this.m_pointStart = new PointF(rect.Left, rect.Top);
        this.m_pointEnd = new PointF(rect.Right, rect.Bottom);
        break;
      case PdfLinearGradientMode.Horizontal:
        this.m_pointStart = new PointF(rect.Left, rect.Top);
        this.m_pointEnd = new PointF(rect.Right, rect.Top);
        break;
      case PdfLinearGradientMode.Vertical:
        this.m_pointStart = new PointF(rect.Left, rect.Top);
        this.m_pointEnd = new PointF(rect.Left, rect.Bottom);
        break;
      default:
        throw new ArgumentException("Unsupported linear gradient mode: " + (object) mode, nameof (mode));
    }
    this.SetPoints(this.m_pointStart, this.m_pointEnd);
  }

  public PdfLinearGradientBrush(RectangleF rect, PdfColor color1, PdfColor color2, float angle)
    : this(color1, color2)
  {
    this.m_boundaries = rect;
    angle %= 360f;
    if ((double) angle == 0.0)
    {
      this.m_pointStart = new PointF(rect.Left, rect.Top);
      this.m_pointEnd = new PointF(rect.Right, rect.Top);
    }
    else if ((double) angle == 90.0)
    {
      this.m_pointStart = new PointF(rect.Left, rect.Top);
      this.m_pointEnd = new PointF(rect.Left, rect.Bottom);
    }
    else if ((double) angle == 180.0)
    {
      this.m_pointEnd = new PointF(rect.Left, rect.Top);
      this.m_pointStart = new PointF(rect.Right, rect.Top);
    }
    else if ((double) angle == 270.0)
    {
      this.m_pointEnd = new PointF(rect.Left, rect.Top);
      this.m_pointStart = new PointF(rect.Left, rect.Bottom);
    }
    else
    {
      double num1 = Math.PI / 180.0;
      double num2 = (double) angle * num1;
      double num3 = Math.Tan(num2);
      PointF pointF1 = new PointF(this.m_boundaries.Left + (float) (((double) this.m_boundaries.Right - (double) this.m_boundaries.Left) / 2.0), this.m_boundaries.Top + (float) (((double) this.m_boundaries.Bottom - (double) this.m_boundaries.Top) / 2.0));
      float num4 = this.m_boundaries.Width / 2f * (float) Math.Cos(num2);
      float num5 = (float) num3 * num4;
      PointF pointF2 = PdfLinearGradientBrush.SubPoints(new PointF(num4 + pointF1.X, num5 + pointF1.Y), pointF1);
      float num6 = PdfLinearGradientBrush.MulPoints(PdfLinearGradientBrush.SubPoints(this.ChoosePoint(angle), pointF1), pointF2) / PdfLinearGradientBrush.MulPoints(pointF2, pointF2);
      this.m_pointEnd = PdfLinearGradientBrush.AddPoints(pointF1, PdfLinearGradientBrush.MulPoint(pointF2, num6));
      this.m_pointStart = PdfLinearGradientBrush.AddPoints(pointF1, PdfLinearGradientBrush.MulPoint(pointF2, -num6));
    }
    this.SetPoints(this.m_pointEnd, this.m_pointStart);
  }

  private PdfLinearGradientBrush(PdfColor color1, PdfColor color2)
    : base(new PdfDictionary())
  {
    this.m_colours = new PdfColor[2]{ color1, color2 };
    this.m_colourBlend = new PdfColorBlend(2);
    this.m_colourBlend.Positions = new float[2]{ 0.0f, 1f };
    this.m_colourBlend.Colors = this.m_colours;
    this.InitShading();
  }

  public PdfBlend Blend
  {
    get => this.m_blend;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Blend));
      if (this.m_colours == null)
        throw new NotSupportedException("There is no starting and ending colours specified.");
      this.m_blend = value;
      this.m_colourBlend = this.m_blend.GenerateColorBlend(this.m_colours, this.ColorSpace);
      this.ResetFunction();
    }
  }

  public PdfColorBlend InterpolationColors
  {
    get => this.m_colourBlend;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (InterpolationColors));
      this.m_blend = (PdfBlend) null;
      this.m_colours = (PdfColor[]) null;
      this.m_colourBlend = value;
      this.ResetFunction();
    }
  }

  public PdfColor[] LinearColors
  {
    get => this.m_colours;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (LinearColors));
      if (value.Length < 2)
        throw new ArgumentException("The array is too small", nameof (LinearColors));
      if (this.m_colours == null)
      {
        this.m_colours = new PdfColor[2]
        {
          value[0],
          value[1]
        };
      }
      else
      {
        this.m_colours[0] = value[0];
        this.m_colours[1] = value[1];
      }
      if (this.m_blend == null)
      {
        this.m_colourBlend = new PdfColorBlend(2);
        this.m_colourBlend.Colors = this.m_colours;
        this.m_colourBlend.Positions = new float[2]
        {
          0.0f,
          1f
        };
      }
      else
        this.m_colourBlend = this.m_blend.GenerateColorBlend(this.m_colours, this.ColorSpace);
      this.ResetFunction();
    }
  }

  public RectangleF Rectangle => this.m_boundaries;

  public PdfExtend Extend
  {
    get
    {
      PdfExtend extend = PdfExtend.None;
      if (this.Shading[nameof (Extend)] is PdfArray pdfArray)
      {
        PdfBoolean pdfBoolean1 = pdfArray[0] as PdfBoolean;
        PdfBoolean pdfBoolean2 = pdfArray[1] as PdfBoolean;
        if (pdfBoolean1.Value)
          extend |= PdfExtend.Start;
        if (pdfBoolean2.Value)
          extend |= PdfExtend.End;
      }
      return extend;
    }
    set
    {
      PdfBoolean pdfBoolean1;
      PdfBoolean pdfBoolean2;
      if (!(this.Shading[nameof (Extend)] is PdfArray pdfArray))
      {
        pdfBoolean1 = new PdfBoolean(false);
        pdfBoolean2 = new PdfBoolean(false);
        this.Shading[nameof (Extend)] = (IPdfPrimitive) new PdfArray()
        {
          (IPdfPrimitive) pdfBoolean1,
          (IPdfPrimitive) pdfBoolean2
        };
      }
      else
      {
        pdfBoolean1 = pdfArray[0] as PdfBoolean;
        pdfBoolean2 = pdfArray[1] as PdfBoolean;
      }
      pdfBoolean1.Value = (value & PdfExtend.Start) > PdfExtend.None;
      pdfBoolean2.Value = (value & PdfExtend.End) > PdfExtend.None;
    }
  }

  private static PointF AddPoints(PointF point1, PointF point2)
  {
    return new PointF(point1.X + point2.X, point1.Y + point2.Y);
  }

  private static PointF SubPoints(PointF point1, PointF point2)
  {
    return new PointF(point1.X - point2.X, point1.Y - point2.Y);
  }

  private static float MulPoints(PointF point1, PointF point2)
  {
    return (float) ((double) point1.X * (double) point2.X + (double) point1.Y * (double) point2.Y);
  }

  private static PointF MulPoint(PointF point, float value)
  {
    point.X *= value;
    point.Y *= value;
    return point;
  }

  private PointF ChoosePoint(float angle)
  {
    PointF pointF = PointF.Empty;
    if ((double) angle < 90.0 && (double) angle > 0.0)
      pointF = new PointF(this.m_boundaries.Right, this.m_boundaries.Bottom);
    else if ((double) angle < 180.0 && (double) angle > 90.0)
      pointF = new PointF(this.m_boundaries.Left, this.m_boundaries.Bottom);
    else if ((double) angle < 270.0 && (double) angle > 180.0)
    {
      pointF = new PointF(this.m_boundaries.Left, this.m_boundaries.Top);
    }
    else
    {
      if ((double) angle <= 270.0)
        throw new PdfException("Internal error.");
      pointF = new PointF(this.m_boundaries.Right, this.m_boundaries.Top);
    }
    return pointF;
  }

  private void SetPoints(PointF point1, PointF point2)
  {
    this.Shading["Coords"] = (IPdfPrimitive) new PdfArray()
    {
      (IPdfPrimitive) new PdfNumber(point1.X),
      (IPdfPrimitive) new PdfNumber(PdfGraphics.UpdateY(point1.Y)),
      (IPdfPrimitive) new PdfNumber(point2.X),
      (IPdfPrimitive) new PdfNumber(PdfGraphics.UpdateY(point2.Y))
    };
  }

  private void InitShading()
  {
    this.ColorSpace = this.ColorSpace;
    this.Function = this.m_colourBlend.GetFunction(this.ColorSpace);
    this.Shading["ShadingType"] = (IPdfPrimitive) new PdfNumber(2);
  }

  public override PdfBrush Clone()
  {
    PdfLinearGradientBrush brush = this.MemberwiseClone() as PdfLinearGradientBrush;
    brush.ResetPatternDictionary(new PdfDictionary(this.PatternDictionary));
    brush.Shading = new PdfDictionary();
    brush.InitShading();
    brush.SetPoints(brush.m_pointStart, brush.m_pointEnd);
    if (this.Matrix != null)
      brush.Matrix = this.Matrix.Clone();
    if (this.m_colours != null)
      brush.m_colours = this.m_colours.Clone() as PdfColor[];
    if (this.Blend != null)
      brush.Blend = this.Blend.ClonePdfBlend();
    else if (this.InterpolationColors != null)
      brush.InterpolationColors = this.InterpolationColors.CloneColorBlend();
    brush.Extend = this.Extend;
    this.CloneBackgroundValue((PdfGradientBrush) brush);
    this.CloneAntiAliasingValue((PdfGradientBrush) brush);
    return (PdfBrush) brush;
  }

  internal override void ResetFunction()
  {
    this.Function = this.m_colourBlend.GetFunction(this.ColorSpace);
  }
}
