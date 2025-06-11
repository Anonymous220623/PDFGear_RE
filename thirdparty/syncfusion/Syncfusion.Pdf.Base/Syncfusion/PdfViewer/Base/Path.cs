// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Path
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Path
{
  public PathGeometry Data { get; set; }

  public double StrokeThickness { get; set; }

  public PenLineCap StrokeStartLineCap { get; set; }

  public PenLineCap StrokeEndLineCap { get; set; }

  public PenLineJoin StrokeLineJoin { get; set; }

  public double StrokeMiterLimit { get; set; }

  public double StrokeDashOffset { get; set; }

  public IEnumerable<double> StrokeDashArray { get; set; }
}
