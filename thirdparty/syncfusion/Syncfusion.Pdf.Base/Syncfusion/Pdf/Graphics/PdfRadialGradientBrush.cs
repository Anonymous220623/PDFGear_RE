// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfRadialGradientBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfRadialGradientBrush : PdfGradientBrush
{
  private PointF m_pointStart;
  private float m_radiusStart;
  private PointF m_pointEnd;
  private float m_radiusEnd;
  private PdfColor[] m_colours;
  private PdfColorBlend m_colourBlend;
  private PdfBlend m_blend;
  private RectangleF m_boundaries;

  public PdfRadialGradientBrush(
    PointF centreStart,
    float radiusStart,
    PointF centreEnd,
    float radiusEnd,
    PdfColor colorStart,
    PdfColor colorEnd)
    : this(colorStart, colorEnd)
  {
    if ((double) radiusStart < 0.0)
      throw new ArgumentOutOfRangeException(nameof (radiusStart), "The radius can't be less then zero.");
    if ((double) radiusEnd < 0.0)
      throw new ArgumentOutOfRangeException(nameof (radiusEnd), "The radius can't be less then zero.");
    this.m_pointEnd = centreEnd;
    this.m_pointStart = centreStart;
    this.m_radiusStart = radiusStart;
    this.m_radiusEnd = radiusEnd;
    this.SetPoints(this.m_pointStart, this.m_pointEnd, this.m_radiusStart, this.m_radiusEnd);
  }

  private PdfRadialGradientBrush(PdfColor color1, PdfColor color2)
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

  public RectangleF Rectangle
  {
    get => this.m_boundaries;
    set
    {
      this.m_boundaries = value;
      this.BBox = PdfArray.FromRectangle(value);
    }
  }

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

  private void SetPoints(PointF pointStart, PointF pointEnd, float radiusStart, float radiusEnd)
  {
    PdfArray pdfArray = new PdfArray();
    pdfArray.Add((IPdfPrimitive) new PdfNumber(pointStart.X));
    pdfArray.Add((IPdfPrimitive) new PdfNumber(PdfGraphics.UpdateY(pointStart.Y)));
    pdfArray.Add((IPdfPrimitive) new PdfNumber(radiusStart));
    pdfArray.Add((IPdfPrimitive) new PdfNumber(pointEnd.X));
    pdfArray.Add((IPdfPrimitive) new PdfNumber(PdfGraphics.UpdateY(pointEnd.Y)));
    if ((double) radiusStart != (double) radiusEnd)
      pdfArray.Add((IPdfPrimitive) new PdfNumber(radiusEnd));
    else
      pdfArray.Add((IPdfPrimitive) new PdfNumber(0));
    this.Shading["Coords"] = (IPdfPrimitive) pdfArray;
  }

  private void InitShading()
  {
    this.ColorSpace = this.ColorSpace;
    this.Function = this.m_colourBlend.GetFunction(this.ColorSpace);
    this.Shading["ShadingType"] = (IPdfPrimitive) new PdfNumber(3);
  }

  public override PdfBrush Clone()
  {
    PdfRadialGradientBrush brush = this.MemberwiseClone() as PdfRadialGradientBrush;
    brush.ResetPatternDictionary(new PdfDictionary(this.PatternDictionary));
    brush.Shading = new PdfDictionary();
    brush.InitShading();
    brush.SetPoints(this.m_pointStart, this.m_pointEnd, this.m_radiusStart, this.m_radiusEnd);
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
