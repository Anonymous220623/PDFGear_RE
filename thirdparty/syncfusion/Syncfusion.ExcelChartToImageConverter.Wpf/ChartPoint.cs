// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.ChartPoint
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using System.Windows.Media;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class ChartPoint
{
  public object X { get; set; }

  public double Value { get; set; }

  public object Size { get; set; }

  public object High { get; set; }

  public object Low { get; set; }

  public object Open { get; set; }

  public object Close { get; set; }

  public bool IsSummary { get; set; }

  public Brush SegmentBrush { get; set; }
}
