// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartDataPointInfo
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class ChartDataPointInfo : ChartSegment
{
  public int Index = -1;

  public double XData { get; set; }

  public double YData { get; set; }

  public double High { get; set; }

  public double Low { get; set; }

  public double Open { get; set; }

  public double Close { get; set; }

  public override UIElement CreateVisual(Size size) => throw new NotImplementedException();

  public override UIElement GetRenderedVisual() => throw new NotImplementedException();

  public override void Update(IChartTransformer transformer) => throw new NotImplementedException();

  public override void OnSizeChanged(Size size) => throw new NotImplementedException();
}
