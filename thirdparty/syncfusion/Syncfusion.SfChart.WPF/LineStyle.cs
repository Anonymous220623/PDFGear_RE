// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LineStyle
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LineStyle : DependencyObject
{
  internal ChartSeriesBase Series;
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (LineStyle), new PropertyMetadata((object) new SolidColorBrush(Colors.Cyan)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (LineStyle), new PropertyMetadata((object) 2.0));
  public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register(nameof (StrokeDashCap), typeof (PenLineCap), typeof (LineStyle), new PropertyMetadata((object) PenLineCap.Flat));
  public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register(nameof (StrokeEndLineCap), typeof (PenLineCap), typeof (LineStyle), new PropertyMetadata((object) PenLineCap.Flat));
  public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register(nameof (StrokeLineJoin), typeof (PenLineJoin), typeof (LineStyle), new PropertyMetadata((object) PenLineJoin.Bevel));
  public static readonly DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register(nameof (StrokeMiterLimit), typeof (double), typeof (LineStyle), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register(nameof (StrokeDashOffset), typeof (double), typeof (LineStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (LineStyle), new PropertyMetadata((object) null, new PropertyChangedCallback(LineStyle.OnPropertyChange)));

  public LineStyle()
  {
  }

  public LineStyle(ChartSeriesBase series) => this.Series = series;

  public Brush Stroke
  {
    get => (Brush) this.GetValue(LineStyle.StrokeProperty);
    set => this.SetValue(LineStyle.StrokeProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(LineStyle.StrokeThicknessProperty);
    set => this.SetValue(LineStyle.StrokeThicknessProperty, (object) value);
  }

  public PenLineCap StrokeDashCap
  {
    get => (PenLineCap) this.GetValue(LineStyle.StrokeDashCapProperty);
    set => this.SetValue(LineStyle.StrokeDashCapProperty, (object) value);
  }

  public PenLineCap StrokeEndLineCap
  {
    get => (PenLineCap) this.GetValue(LineStyle.StrokeEndLineCapProperty);
    set => this.SetValue(LineStyle.StrokeEndLineCapProperty, (object) value);
  }

  public PenLineJoin StrokeLineJoin
  {
    get => (PenLineJoin) this.GetValue(LineStyle.StrokeLineJoinProperty);
    set => this.SetValue(LineStyle.StrokeLineJoinProperty, (object) value);
  }

  public double StrokeMiterLimit
  {
    get => (double) this.GetValue(LineStyle.StrokeMiterLimitProperty);
    set => this.SetValue(LineStyle.StrokeMiterLimitProperty, (object) value);
  }

  public double StrokeDashOffset
  {
    get => (double) this.GetValue(LineStyle.StrokeDashOffsetProperty);
    set => this.SetValue(LineStyle.StrokeDashOffsetProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(LineStyle.StrokeDashArrayProperty);
    set => this.SetValue(LineStyle.StrokeDashArrayProperty, (object) value);
  }

  private static void OnPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
  {
    if (!(obj is LineStyle lineStyle))
      return;
    ChartSeriesBase series = lineStyle.Series;
    if (series == null || e.NewValue == null)
      return;
    foreach (ChartSegment segment in (Collection<ChartSegment>) series.Segments)
    {
      DoubleCollection newValue = (DoubleCollection) e.NewValue;
      if (newValue != null && newValue.Count > 0)
      {
        DoubleCollection doubleCollection1 = new DoubleCollection();
        DoubleCollection doubleCollection2 = new DoubleCollection();
        foreach (double num in newValue)
        {
          doubleCollection1.Add(num);
          doubleCollection2.Add(num);
        }
        ErrorBarSeries errorBarSeries = series as ErrorBarSeries;
        ErrorBarSegment errorBarSegment = segment as ErrorBarSegment;
        if (errorBarSeries != null && errorBarSegment != null)
        {
          if (lineStyle == errorBarSeries.HorizontalLineStyle)
            errorBarSegment.HorLine.StrokeDashArray = doubleCollection1;
          if (lineStyle == errorBarSeries.HorizontalCapLineStyle)
          {
            errorBarSegment.HorRightCapLine.StrokeDashArray = doubleCollection1;
            errorBarSegment.HorLeftCapLine.StrokeDashArray = doubleCollection2;
          }
          if (lineStyle == errorBarSeries.VerticalLineStyle)
            errorBarSegment.VerLine.StrokeDashArray = doubleCollection1;
          if (lineStyle == errorBarSeries.VerticalCapLineStyle)
          {
            errorBarSegment.VerBottomCapLine.StrokeDashArray = doubleCollection1;
            errorBarSegment.VerTopCapLine.StrokeDashArray = doubleCollection2;
          }
        }
      }
    }
  }
}
