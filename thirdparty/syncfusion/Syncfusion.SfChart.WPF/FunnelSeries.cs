// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FunnelSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FunnelSeries : TriangularSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty FunnelModeProperty = DependencyProperty.Register(nameof (FunnelMode), typeof (ChartFunnelMode), typeof (FunnelSeries), new PropertyMetadata((object) ChartFunnelMode.ValueIsHeight, new PropertyChangedCallback(FunnelSeries.OnFunnelModeChanged)));
  public new static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(nameof (MinWidth), typeof (double), typeof (FunnelSeries), new PropertyMetadata((object) 40.0, new PropertyChangedCallback(FunnelSeries.OnMinWidthChanged)));
  private double currY;

  public FunnelSeries() => this.DefaultStyleKey = (object) typeof (FunnelSeries);

  public ChartFunnelMode FunnelMode
  {
    get => (ChartFunnelMode) this.GetValue(FunnelSeries.FunnelModeProperty);
    set => this.SetValue(FunnelSeries.FunnelModeProperty, (object) value);
  }

  public new double MinWidth
  {
    get => (double) this.GetValue(FunnelSeries.MinWidthProperty);
    set => this.SetValue(FunnelSeries.MinWidthProperty, (object) value);
  }

  public override void CreateSegments()
  {
    this.Segments.Clear();
    this.Adornments.Clear();
    List<double> xvalues = this.GetXValues();
    double sumValues = 0.0;
    double gapRatio = this.GapRatio;
    int dataCount = this.DataCount;
    int explodeIndex = this.ExplodeIndex;
    ChartFunnelMode funnelMode = this.FunnelMode;
    IList<double> yValues = this.ToggledLegendIndex.Count <= 0 ? this.YValues : (IList<double>) this.GetYValues();
    for (int index = 0; index < dataCount; ++index)
      sumValues += Math.Max(0.0, Math.Abs(double.IsNaN(yValues[index]) ? 0.0 : yValues[index]));
    if (funnelMode == ChartFunnelMode.ValueIsHeight)
      this.CalculateValueIsHeightSegments(yValues, xvalues, sumValues, gapRatio, explodeIndex);
    else
      this.CalculateValueIsWidthSegments(yValues, xvalues, sumValues, gapRatio, dataCount, explodeIndex);
    if (this.ShowEmptyPoints)
      this.UpdateEmptyPointSegments(xvalues, false);
    if (this.ActualArea == null)
      return;
    this.ActualArea.IsUpdateLegend = true;
  }

  internal override object GetTooltipTag(FrameworkElement element)
  {
    object tooltipTag = (object) null;
    if (element.Tag is ChartSegment)
      tooltipTag = element.Tag;
    else if (element.DataContext is ChartSegment && !(element.DataContext is ChartAdornment))
      tooltipTag = element.DataContext;
    else if (element.DataContext is ChartAdornmentContainer)
    {
      if (this.Segments.Count > 0)
        tooltipTag = (object) this.Segments[ChartExtensionUtils.GetAdornmentIndex((object) element)];
    }
    else if (VisualTreeHelper.GetParent((DependencyObject) element) is ContentPresenter parent && parent.Content is ChartAdornment)
    {
      tooltipTag = this.GetSegment((parent.Content as ChartAdornment).Item);
    }
    else
    {
      int adornmentIndex = ChartExtensionUtils.GetAdornmentIndex((object) element);
      if (adornmentIndex != -1 && adornmentIndex < this.Adornments.Count && adornmentIndex < this.Segments.Count && adornmentIndex < this.ActualData.Count)
        tooltipTag = this.GetSegment(this.ActualData[adornmentIndex]);
    }
    return tooltipTag;
  }

  protected internal override IChartTransformer CreateTransformer(Size size, bool create)
  {
    if (create || this.ChartTransformer == null)
      this.ChartTransformer = ChartTransform.CreateSimple(size);
    return this.ChartTransformer;
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new FunnelSegment();

  protected override ChartAdornment CreateAdornment(
    AdornmentSeries series,
    double xVal,
    double yVal,
    double height,
    double currY)
  {
    return (ChartAdornment) new TriangularAdornment(xVal, yVal, currY, height, series);
  }

  protected override void SetExplodeIndex(int i)
  {
    if (this.Segments.Count <= 0)
      return;
    foreach (FunnelSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      int num = this.ActualData.IndexOf(segment.Item);
      if (i == num)
      {
        segment.IsExploded = !segment.IsExploded;
        this.UpdateSegments(i, NotifyCollectionChangedAction.Remove);
      }
      else if (i == -1)
      {
        segment.IsExploded = false;
        this.UpdateSegments(i, NotifyCollectionChangedAction.Remove);
      }
    }
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    FunnelSeries funnelSeries = new FunnelSeries();
    funnelSeries.FunnelMode = this.FunnelMode;
    funnelSeries.GapRatio = this.GapRatio;
    funnelSeries.ExplodeOffset = this.ExplodeOffset;
    funnelSeries.MinWidth = this.MinWidth;
    return base.CloneSeries((DependencyObject) funnelSeries);
  }

  private static void OnFunnelModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FunnelSeries funnelSeries) || funnelSeries.Area == null)
      return;
    funnelSeries.Area.IsUpdateLegend = true;
    funnelSeries.Area.ScheduleUpdate();
  }

  private static void OnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FunnelSeries funnelSeries) || funnelSeries.Area == null)
      return;
    funnelSeries.Area.ScheduleUpdate();
  }

  private void CalculateValueIsHeightSegments(
    IList<double> yValues,
    List<double> xValues,
    double sumValues,
    double gapRatio,
    int explodedIndex)
  {
    this.currY = 0.0;
    double num1 = 1.0 / sumValues;
    double num2 = gapRatio / (double) (this.DataCount - 1);
    if (this.DataCount == 1)
      num2 = 0.0;
    for (int index = this.DataCount - 1; index >= 0; --index)
    {
      if (!double.IsNaN(this.YValues[index]))
      {
        double num3 = Math.Abs(double.IsNaN(yValues[index]) ? 0.0 : yValues[index]) * num1;
        if (this.CreateSegment() is FunnelSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(this.currY, this.currY + num3, this.currY / 2.0, (this.currY + num3) / 2.0, this.MinWidth, this.ExplodeOffset);
          segment.IsExploded = explodedIndex == index || this.ExplodeAll;
          segment.Item = this.ActualData[index];
          segment.XData = xValues[index];
          segment.YData = this.YValues[index];
          if (this.ToggledLegendIndex.Contains(index))
            segment.IsSegmentVisible = false;
          else
            segment.IsSegmentVisible = true;
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
        {
          ChartAdornment adornment = this.CreateAdornment((AdornmentSeries) this, xValues[index], yValues[index], 0.0, double.IsNaN(this.currY) ? 0.0 : this.currY + (num3 + num2) / 2.0);
          adornment.Item = this.ActualData[index];
          this.Adornments.Add(adornment);
        }
        this.currY += num3 + num2;
      }
    }
  }

  private void CalculateValueIsWidthSegments(
    IList<double> yValues,
    List<double> xValues,
    double sumValues,
    double gapRatio,
    int count,
    int explodedIndex)
  {
    this.currY = 0.0;
    if (this.ToggledLegendIndex.Count > 0)
      count = this.YValues.Count - this.ToggledLegendIndex.Count;
    double num1 = 1.0 / (double) (count - 1);
    double xPos = (1.0 - gapRatio) / (double) (count - 1);
    for (int index1 = this.DataCount - 1; index1 > 0; --index1)
    {
      if (!double.IsNaN(this.YValues[index1]))
      {
        double num2 = Math.Abs(this.YValues[index1]);
        double num3 = 0.0;
        if (this.ToggledLegendIndex.Contains(index1 - 1))
        {
          for (int index2 = index1 - 2; index2 >= 0; --index2)
          {
            if (!this.ToggledLegendIndex.Contains(index2))
            {
              num3 = Math.Abs(this.YValues[index2]);
              break;
            }
          }
        }
        else
          num3 = Math.Abs(this.YValues[index1 - 1]);
        if (this.ToggledLegendIndex.Contains(index1))
        {
          xPos = 0.0;
          num3 = num2;
        }
        else
          xPos = (1.0 - gapRatio) / (double) (count - 1);
        double num4 = num2 / sumValues;
        double num5 = num3 / sumValues;
        if (this.CreateSegment() is FunnelSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(this.currY, this.currY + xPos, (1.0 - num4) / 2.0, (1.0 - num5) / 2.0, this.MinWidth, this.ExplodeOffset);
          segment.IsExploded = explodedIndex == index1 || this.ExplodeAll;
          segment.Item = this.ActualData[index1];
          segment.XData = xValues[index1];
          segment.YData = this.YValues[index1];
          if (this.ToggledLegendIndex.Contains(index1))
            segment.IsSegmentVisible = false;
          else
            segment.IsSegmentVisible = true;
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
        {
          this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValues[index1], yValues[index1], xPos, this.currY));
          this.Adornments[this.Adornments.Count - 1].Item = this.ActualData[index1];
        }
        if (!this.ToggledLegendIndex.Contains(index1))
          this.currY += num1;
      }
    }
    if (this.AdornmentsInfo == null || this.DataCount <= 0)
      return;
    this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValues[0], yValues[0], xPos, this.currY));
    this.Adornments[this.Adornments.Count - 1].Item = this.ActualData[0];
  }
}
