// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.CustomWaterfallSeries
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class CustomWaterfallSeries : WaterfallSeries
{
  private Dictionary<int, Brush> m_borderBrushes;
  private Dictionary<int, double> m_thicknessDictionary;
  private Brush m_defaultInterior;
  private Brush m_defaultNegativeBrush;
  private Brush m_defaultSummaryBrush;

  internal Dictionary<int, Brush> BorderBrushes
  {
    get => this.m_borderBrushes;
    set => this.m_borderBrushes = value;
  }

  internal Dictionary<int, double> ThicknessDictionary
  {
    get => this.m_thicknessDictionary;
    set => this.m_thicknessDictionary = value;
  }

  internal Brush DefaultInterior
  {
    get => this.m_defaultInterior;
    set => this.m_defaultInterior = value;
  }

  internal Brush DefaultNegativeBrush
  {
    get => this.m_defaultNegativeBrush;
    set => this.m_defaultNegativeBrush = value;
  }

  internal Brush DefaultSummaryBrush
  {
    get => this.m_defaultSummaryBrush;
    set => this.m_defaultSummaryBrush = value;
  }

  public override void CreateSegments()
  {
    base.CreateSegments();
    for (int index = 0; index < this.Segments.Count; ++index)
    {
      if (this.m_borderBrushes != null && this.m_borderBrushes.ContainsKey(index))
        this.Segments[index].Stroke = this.m_borderBrushes[index];
      if (this.m_borderBrushes != null && this.m_thicknessDictionary.ContainsKey(index))
        this.Segments[index].StrokeThickness = this.m_thicknessDictionary[index];
    }
  }
}
