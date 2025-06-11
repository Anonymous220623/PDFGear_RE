// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartTrackBallControl
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartTrackBallControl : Control
{
  public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(nameof (Series), typeof (ChartSeriesBase), typeof (ChartTrackBallControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (ChartTrackBallControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (ChartTrackBallControl), new PropertyMetadata((object) 1.0));

  public ChartTrackBallControl(ChartSeriesBase series)
  {
    this.Series = series;
    this.DefaultStyleKey = (object) typeof (ChartTrackBallControl);
  }

  public ChartSeriesBase Series
  {
    get => (ChartSeriesBase) this.GetValue(ChartTrackBallControl.SeriesProperty);
    set => this.SetValue(ChartTrackBallControl.SeriesProperty, (object) value);
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(ChartTrackBallControl.StrokeProperty);
    set => this.SetValue(ChartTrackBallControl.StrokeProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(ChartTrackBallControl.StrokeThicknessProperty);
    set => this.SetValue(ChartTrackBallControl.StrokeThicknessProperty, (object) value);
  }
}
