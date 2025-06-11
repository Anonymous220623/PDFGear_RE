// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AdornmentSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class AdornmentSeries : ChartSeries
{
  public static readonly DependencyProperty AdornmentsInfoProperty = DependencyProperty.Register(nameof (AdornmentsInfo), typeof (ChartAdornmentInfo), typeof (ChartSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(AdornmentSeries.OnAdornmentsInfoChanged)));

  public ChartAdornmentInfo AdornmentsInfo
  {
    get => (ChartAdornmentInfo) this.GetValue(AdornmentSeries.AdornmentsInfoProperty);
    set => this.SetValue(AdornmentSeries.AdornmentsInfoProperty, (object) value);
  }

  public event EventHandler<AdornmentLabelCreatedEventArgs> AdornmentLabelCreated;

  internal bool IsAdornmentLabelCreatedEventHooked => this.AdornmentLabelCreated != null;

  public override void CreateSegments() => throw new NotImplementedException();

  internal override void UpdateOnSeriesBoundChanged(Size size)
  {
    if (this.AdornmentPresenter != null && this.AdornmentsInfo != null)
    {
      if (this.IsAdornmentLabelCreatedEventHooked)
        this.RaiseAdornmentLabelCreatedEvent();
      this.AdornmentsInfo.UpdateElements();
    }
    base.UpdateOnSeriesBoundChanged(size);
    if (this.AdornmentPresenter == null || this.AdornmentsInfo == null)
      return;
    this.AdornmentPresenter.Update(size);
    this.AdornmentPresenter.Arrange(size);
  }

  private void RaiseAdornmentLabelCreatedEvent()
  {
    for (int index = 0; index < this.Adornments.Count; ++index)
    {
      ChartAdornment adornment = this.Adornments[index];
      adornment.Label = adornment.GetTextContent().ToString();
      ChartAdornmentInfo adornmentLabel = new ChartAdornmentInfo();
      adornmentLabel.LabelRotationAngle = this.AdornmentsInfo.LabelRotationAngle;
      adornmentLabel.Background = this.AdornmentsInfo.Background;
      adornmentLabel.BorderBrush = this.AdornmentsInfo.BorderBrush;
      adornmentLabel.BorderThickness = this.AdornmentsInfo.BorderThickness;
      adornmentLabel.Data = adornment.Item;
      adornmentLabel.FontFamily = this.AdornmentsInfo.FontFamily;
      adornmentLabel.FontSize = this.AdornmentsInfo.FontSize;
      adornmentLabel.FontStyle = this.AdornmentsInfo.FontStyle;
      adornmentLabel.Foreground = this.AdornmentsInfo.Foreground;
      adornmentLabel.Index = index;
      adornmentLabel.Label = adornment.Label;
      adornmentLabel.LabelBackgroundBrush = this.AdornmentsInfo.Background;
      adornmentLabel.SegmentLabelFormat = this.AdornmentsInfo.SegmentLabelFormat;
      adornmentLabel.LabelPadding = this.AdornmentsInfo.LabelPadding;
      adornmentLabel.LabelPosition = this.AdornmentsInfo.LabelPosition;
      adornmentLabel.Margin = this.AdornmentsInfo.Margin;
      adornmentLabel.OffsetX = this.AdornmentsInfo.OffsetX;
      adornmentLabel.OffsetY = this.AdornmentsInfo.OffsetY;
      if (adornment.Series != null && adornment.Series.SeriesYValues != null)
        adornmentLabel.GrandTotal = adornment.Series.SeriesYValues.Length > 1 ? adornment.GrandTotal : adornment.CalculateSumOfValues(adornment.Series.SeriesYValues[0]);
      adornmentLabel.Symbol = this.AdornmentsInfo.Symbol;
      adornmentLabel.SymbolHeight = this.AdornmentsInfo.SymbolHeight;
      adornmentLabel.SymbolInterior = this.AdornmentsInfo.SymbolInterior;
      adornmentLabel.SymbolStroke = this.AdornmentsInfo.SymbolStroke;
      adornmentLabel.SymbolWidth = this.AdornmentsInfo.SymbolWidth;
      adornmentLabel.XPosition = adornment.XPos;
      adornmentLabel.YPosition = adornment.YPos;
      this.AdornmentLabelCreated((object) this, new AdornmentLabelCreatedEventArgs(adornmentLabel));
      adornmentLabel.Background = adornmentLabel.LabelBackgroundBrush != this.AdornmentsInfo.Background ? adornmentLabel.LabelBackgroundBrush : (Brush) null;
      adornmentLabel.SegmentLabelFormat = adornmentLabel.SegmentLabelFormat != this.AdornmentsInfo.SegmentLabelFormat ? adornmentLabel.SegmentLabelFormat : (string) null;
      adornmentLabel.Label = !adornment.Label.Equals(adornmentLabel.Label) ? adornmentLabel.Label : (string) null;
      adornment.CustomAdornmentLabel = adornmentLabel;
    }
  }

  internal override void CalculateSegments()
  {
    base.CalculateSegments();
    if (this.VisibleAdornments.Count > 0)
      this.VisibleAdornments.Clear();
    if (this.DataCount != 0 || this.AdornmentsInfo == null)
      return;
    if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
      this.ClearUnUsedAdornments(this.DataCount * 4);
    else
      this.ClearUnUsedAdornments(this.DataCount * 2);
  }

  protected internal override void GeneratePoints() => throw new NotImplementedException();

  protected virtual ChartAdornment CreateAdornment(
    AdornmentSeries series,
    double xVal,
    double yVal,
    double xPos,
    double yPos)
  {
    return new ChartAdornment(xVal, yVal, xPos, yPos, (ChartSeriesBase) series)
    {
      XData = xVal,
      YData = yVal,
      XPos = xPos,
      YPos = yPos,
      Series = (ChartSeriesBase) series
    };
  }

  protected virtual void AddColumnAdornments(params double[] values)
  {
    double xPos = values[2] + values[5];
    double yPos = values[3];
    int index = (int) values[4];
    if (this.EmptyPointIndexes != null && ((IEnumerable<List<int>>) this.EmptyPointIndexes).Any<List<int>>() && this.EmptyPointIndexes[0].Contains(index) && (this.EmptyPointStyle == EmptyPointStyle.Symbol || this.EmptyPointStyle == EmptyPointStyle.SymbolAndInterior))
      yPos = !(this is StackingSeriesBase) ? values[1] : (this.EmptyPointValue == EmptyPointValue.Average ? values[3] : values[1]);
    if (index < this.Adornments.Count)
      this.Adornments[index].SetData(values[0], values[1], xPos, yPos);
    else
      this.Adornments.Add(this.CreateAdornment(this, values[0], values[1], xPos, yPos));
    if (this is HistogramSeries)
      return;
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed && this.GroupedActualData.Count > 0)
      this.Adornments[index].Item = this.GroupedActualData[index];
    else
      this.Adornments[index].Item = this.ActualData[index];
  }

  protected virtual void AddAdornmentAtXY(double x, double y, int pointindex)
  {
    double xPos = x;
    double yPos = y;
    if (pointindex < this.Adornments.Count)
      this.Adornments[pointindex].SetData(x, y, xPos, yPos);
    else
      this.Adornments.Add(this.CreateAdornment(this, x, y, xPos, yPos));
    if (pointindex >= this.ActualData.Count)
      return;
    this.Adornments[pointindex].Item = this.ActualData[pointindex];
  }

  protected virtual void AddAreaAdornments(params IList<double>[] values)
  {
    IList<double> doubleList1 = values[0];
    List<double> doubleList2 = new List<double>();
    List<double> doubleList3 = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (values.Length != 1)
      return;
    for (int index = 0; index < this.DataCount; ++index)
    {
      if (index < doubleList3.Count && index < doubleList1.Count)
      {
        double xPos = doubleList3[index];
        double yPos = doubleList1[index];
        if (index < this.Adornments.Count)
          this.Adornments[index].SetData(doubleList3[index], doubleList1[index], xPos, yPos);
        else
          this.Adornments.Add(this.CreateAdornment(this, doubleList3[index], doubleList1[index], xPos, yPos));
        this.Adornments[index].Item = this.ActualData[index];
      }
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this is ErrorBarSeries)
      return;
    this.AdornmentPresenter.Series = (ChartSeriesBase) this;
    if (this.Area == null || this.AdornmentsInfo == null)
      return;
    Panel adornmentPresenter = (Panel) this.AdornmentPresenter;
    if (adornmentPresenter == null)
      return;
    this.AdornmentsInfo.PanelChanged(adornmentPresenter);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (this.AdornmentsInfo != null)
    {
      this.VisibleAdornments.Clear();
      this.Adornments.Clear();
      this.AdornmentsInfo.UpdateElements();
    }
    base.OnDataSourceChanged(oldValue, newValue);
    SfChart area = this.Area;
    if (area == null)
      return;
    area.IsUpdateLegend = area.HasDataPointBasedLegend();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    if (this.AdornmentsInfo != null)
      (obj as AdornmentSeries).AdornmentsInfo = (ChartAdornmentInfo) this.AdornmentsInfo.Clone();
    return base.CloneSeries(obj);
  }

  protected void ClearUnUsedAdornments(int startIndex)
  {
    if (this.Adornments.Count <= startIndex)
      return;
    int count = this.Adornments.Count;
    for (int index = startIndex; index < count; ++index)
      this.Adornments.RemoveAt(startIndex);
  }

  private static void OnAdornmentsInfoChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    AdornmentSeries adornmentSeries = d as AdornmentSeries;
    if (e.OldValue != null)
    {
      ChartAdornmentInfo oldValue = e.OldValue as ChartAdornmentInfo;
      if (adornmentSeries != null)
      {
        adornmentSeries.Adornments.Clear();
        adornmentSeries.VisibleAdornments.Clear();
      }
      if (oldValue != null)
      {
        oldValue.ClearChildren();
        oldValue.Series = (ChartSeriesBase) null;
      }
    }
    if (e.NewValue == null || adornmentSeries == null)
      return;
    adornmentSeries.adornmentInfo = (ChartAdornmentInfoBase) (e.NewValue as ChartAdornmentInfo);
    adornmentSeries.AdornmentsInfo.Series = (ChartSeriesBase) adornmentSeries;
    if (adornmentSeries.Area == null || adornmentSeries.AdornmentsInfo == null)
      return;
    Panel adornmentPresenter = (Panel) adornmentSeries.AdornmentPresenter;
    if (adornmentPresenter == null)
      return;
    adornmentSeries.AdornmentsInfo.PanelChanged(adornmentPresenter);
    adornmentSeries.Area.ScheduleUpdate();
  }
}
