// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontOpenTypeGlyphScaler
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontOpenTypeGlyphScaler
{
  internal const double Dpi = 72.0;
  private const double Ppi = 72.0;
  private readonly SystemFontOpenTypeFontSourceBase fontFile;

  public SystemFontOpenTypeGlyphScaler(SystemFontOpenTypeFontSourceBase fontFile)
  {
    this.fontFile = fontFile;
  }

  public void ScaleGlyphMetrics(SystemFontGlyph glyph, double fontSize)
  {
    glyph.AdvancedWidth = this.FUnitsToPixels((int) this.fontFile.HMtx.GetAdvancedWidth((int) glyph.GlyphId), fontSize);
  }

  public double FUnitsToPixels(int units, double fontSize)
  {
    return this.FUnitsToPixels((double) units, fontSize);
  }

  public double FUnitsToPixels(double units, double fontSize)
  {
    return units * 72.0 * fontSize / (72.0 * (double) this.fontFile.Head.UnitsPerEm);
  }

  public PointF FUnitsOutlinePointToPixels(System.Drawing.Point unitPoint, double fontSize)
  {
    return new PointF((float) this.FUnitsToPixels(unitPoint.X, fontSize), (float) -this.FUnitsToPixels(unitPoint.Y, fontSize));
  }

  public System.Drawing.Point FUnitsPointToPixels(System.Drawing.Point unitPoint, double fontSize)
  {
    return new System.Drawing.Point((int) this.FUnitsToPixels(unitPoint.X, fontSize), (int) this.FUnitsToPixels(unitPoint.Y, fontSize));
  }

  public void GetScaleGlyphOutlines(SystemFontGlyph glyph, double fontSize)
  {
    switch (this.fontFile.Outlines)
    {
      case SystemFontOutlines.TrueType:
        this.CreateTrueTypeOutlines(glyph, fontSize);
        break;
      case SystemFontOutlines.OpenType:
        this.CreateOpenTypeOutlines(glyph, fontSize);
        break;
    }
  }

  private static System.Drawing.Point GetMidPoint(System.Drawing.Point a, System.Drawing.Point b)
  {
    return new System.Drawing.Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
  }

  private static SystemFontLineSegment CreateLineSegment(PointF point)
  {
    return new SystemFontLineSegment()
    {
      Point = (Syncfusion.PdfViewer.Base.Point) point
    };
  }

  private static SystemFontQuadraticBezierSegment CreateBezierSegment(PointF control, PointF end)
  {
    return new SystemFontQuadraticBezierSegment()
    {
      Point1 = (Syncfusion.PdfViewer.Base.Point) control,
      Point2 = (Syncfusion.PdfViewer.Base.Point) end
    };
  }

  private SystemFontPathFigure CreatePathFigureFromContour(
    SystemFontOutlinePoint[] points,
    double fontSize)
  {
    SystemFontPathFigure figureFromContour = new SystemFontPathFigure();
    figureFromContour.StartPoint = (Syncfusion.PdfViewer.Base.Point) this.FUnitsOutlinePointToPixels((System.Drawing.Point) points[0].Point, fontSize);
    for (int index = 1; index < points.Length; ++index)
    {
      if (points[index].IsOnCurve)
        figureFromContour.Segments.Add((SystemFontPathSegment) SystemFontOpenTypeGlyphScaler.CreateLineSegment(this.FUnitsOutlinePointToPixels((System.Drawing.Point) points[index].Point, fontSize)));
      else if (points[(index + 1) % points.Length].IsOnCurve)
      {
        figureFromContour.Segments.Add((SystemFontPathSegment) SystemFontOpenTypeGlyphScaler.CreateBezierSegment(this.FUnitsOutlinePointToPixels((System.Drawing.Point) points[index].Point, fontSize), this.FUnitsOutlinePointToPixels((System.Drawing.Point) points[(index + 1) % points.Length].Point, fontSize)));
        ++index;
      }
      else
        figureFromContour.Segments.Add((SystemFontPathSegment) SystemFontOpenTypeGlyphScaler.CreateBezierSegment(this.FUnitsOutlinePointToPixels((System.Drawing.Point) points[index].Point, fontSize), this.FUnitsOutlinePointToPixels(SystemFontOpenTypeGlyphScaler.GetMidPoint((System.Drawing.Point) points[index].Point, (System.Drawing.Point) points[(index + 1) % points.Length].Point), fontSize)));
    }
    figureFromContour.IsClosed = true;
    figureFromContour.IsFilled = true;
    return figureFromContour;
  }

  private void CreateOpenTypeOutlines(SystemFontGlyph glyph, double fontSize)
  {
    this.fontFile.CFF.GetGlyphOutlines(glyph, fontSize);
  }

  private void CreateTrueTypeOutlines(SystemFontGlyph glyph, double fontSize)
  {
    glyph.Outlines = new SystemFontGlyphOutlinesCollection();
    foreach (SystemFontOutlinePoint[] contour in this.fontFile.GetGlyphData(glyph.GlyphId).Contours)
      glyph.Outlines.Add(this.CreatePathFigureFromContour(contour, fontSize));
  }
}
