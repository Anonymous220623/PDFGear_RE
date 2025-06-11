// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSelectionBehavior
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartSelectionBehavior : ChartBehavior
{
  public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof (SelectionMode), typeof (SelectionMode), typeof (ChartSelectionBehavior), new PropertyMetadata((object) SelectionMode.MouseClick));
  public static readonly DependencyProperty EnableSeriesSelectionProperty = DependencyProperty.Register(nameof (EnableSeriesSelection), typeof (bool), typeof (ChartSelectionBehavior), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartSelectionBehavior.OnEnableSeriesSelectionChanged)));
  public static readonly DependencyProperty EnableSegmentSelectionProperty = DependencyProperty.Register(nameof (EnableSegmentSelection), typeof (bool), typeof (ChartSelectionBehavior), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartSelectionBehavior.OnEnableSegmentSelectionChanged)));
  public static readonly DependencyProperty SelectionStyleProperty = DependencyProperty.Register(nameof (SelectionStyle), typeof (SelectionStyle), typeof (ChartSelectionBehavior), new PropertyMetadata((object) SelectionStyle.Single, new PropertyChangedCallback(ChartSelectionBehavior.OnSelectionStyleChanged)));
  public static readonly DependencyProperty SelectionCursorProperty = DependencyProperty.Register(nameof (SelectionCursor), typeof (Cursor), typeof (ChartSelectionBehavior), new PropertyMetadata((PropertyChangedCallback) null));
  private ChartSegment mouseUnderSegment;
  private List<ChartSeries> seriesCollection;
  private ChartAdornmentPresenter selectedAdornmentPresenter;
  private int index;

  public SelectionMode SelectionMode
  {
    get => (SelectionMode) this.GetValue(ChartSelectionBehavior.SelectionModeProperty);
    set => this.SetValue(ChartSelectionBehavior.SelectionModeProperty, (object) value);
  }

  public bool EnableSeriesSelection
  {
    get => (bool) this.GetValue(ChartSelectionBehavior.EnableSeriesSelectionProperty);
    set => this.SetValue(ChartSelectionBehavior.EnableSeriesSelectionProperty, (object) value);
  }

  public bool EnableSegmentSelection
  {
    get => (bool) this.GetValue(ChartSelectionBehavior.EnableSegmentSelectionProperty);
    set => this.SetValue(ChartSelectionBehavior.EnableSegmentSelectionProperty, (object) value);
  }

  public SelectionStyle SelectionStyle
  {
    get => (SelectionStyle) this.GetValue(ChartSelectionBehavior.SelectionStyleProperty);
    set => this.SetValue(ChartSelectionBehavior.SelectionStyleProperty, (object) value);
  }

  public Cursor SelectionCursor
  {
    get => (Cursor) this.GetValue(ChartSelectionBehavior.SelectionCursorProperty);
    set => this.SetValue(ChartSelectionBehavior.SelectionCursorProperty, (object) value);
  }

  public virtual Brush GetSeriesSelectionBrush(ChartSeriesBase series)
  {
    return series.SeriesSelectionBrush != null ? series.SeriesSelectionBrush : (Brush) null;
  }

  protected internal override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    if (this.SelectionMode == SelectionMode.MouseClick)
    {
      FrameworkElement originalSource = e.OriginalSource as FrameworkElement;
      ChartSegment chartSegment = (ChartSegment) null;
      this.ChartArea.CurrentSelectedSeries = (ChartSeriesBase) null;
      if (originalSource != null)
      {
        if (originalSource.Tag != null)
          chartSegment = originalSource.Tag as ChartSegment;
        else if (originalSource.TemplatedParent is ContentPresenter templatedParent && templatedParent.Content is ChartSegment && !(templatedParent.Content is ChartAdornment))
          chartSegment = templatedParent.Content as ChartSegment;
      }
      if (chartSegment is TrendlineSegment)
        return;
      ScatterSeries series = originalSource is ChartSeriesPanel chartSeriesPanel ? chartSeriesPanel.Series as ScatterSeries : (ScatterSeries) null;
      if (originalSource is Image image && image.Source is WriteableBitmap)
        this.OnBitmapSeriesMouseDownSelection(originalSource, (MouseEventArgs) e);
      else if (chartSegment != null && chartSegment == this.mouseUnderSegment && chartSegment.Series is ISegmentSelectable && !(chartSegment.Item is Trendline))
      {
        if (!chartSegment.Series.IsSideBySide && chartSegment.Series is CartesianSeries && !(chartSegment.Series is ScatterSeries) && !(chartSegment.Series is BubbleSeries))
        {
          Point position = e.GetPosition((IInputElement) chartSegment.Series.ActualArea.GetAdorningCanvas());
          ChartDataPointInfo dataPoint = (chartSegment.Series as ChartSeries).GetDataPoint(position);
          this.OnMouseDownSelection(chartSegment.Series, (object) dataPoint);
        }
        else
        {
          int num = !(chartSegment.Series.ActualXAxis is CategoryAxis) || (chartSegment.Series.ActualXAxis as CategoryAxis).IsIndexed || !chartSegment.Series.IsSideBySide || chartSegment.Series is FinancialSeriesBase || chartSegment.Series is RangeSeriesBase || chartSegment.Series is WaterfallSeries ? (!(chartSegment.Series is CircularSeriesBase) || double.IsNaN(((CircularSeriesBase) chartSegment.Series).GroupTo) ? chartSegment.Series.ActualData.IndexOf(chartSegment.Item) : chartSegment.Series.Segments.IndexOf(chartSegment)) : chartSegment.Series.GroupedActualData.IndexOf(chartSegment.Item);
          this.OnMouseDownSelection(chartSegment.Series, (object) num);
        }
      }
      else if (this.mouseUnderSegment != null && series != null && series.IsDraggingActivateOnly)
      {
        this.OnMouseDownSelection(this.mouseUnderSegment.Series, (object) (!(this.mouseUnderSegment.Series.ActualXAxis is CategoryAxis) || (this.mouseUnderSegment.Series.ActualXAxis as CategoryAxis).IsIndexed || !this.mouseUnderSegment.Series.IsSideBySide || this.mouseUnderSegment.Series is FinancialSeriesBase || this.mouseUnderSegment.Series is RangeSeriesBase || this.mouseUnderSegment.Series is WaterfallSeries ? this.mouseUnderSegment.Series.ActualData.IndexOf(this.mouseUnderSegment.Item) : this.mouseUnderSegment.Series.GroupedActualData.IndexOf(this.mouseUnderSegment.Item)));
      }
      else
      {
        this.index = ChartExtensionUtils.GetAdornmentIndex((object) originalSource);
        FrameworkElement reference = e.OriginalSource as FrameworkElement;
        ChartAdornmentPresenter adornmentPresenter;
        for (adornmentPresenter = reference as ChartAdornmentPresenter; reference != null && adornmentPresenter == null; adornmentPresenter = reference as ChartAdornmentPresenter)
          reference = VisualTreeHelper.GetParent((DependencyObject) reference) as FrameworkElement;
        if (adornmentPresenter != null && adornmentPresenter.Series is ISegmentSelectable)
          this.OnMouseDownSelection(adornmentPresenter.Series, (object) this.index);
      }
      if (this.selectedAdornmentPresenter != null)
        this.selectedAdornmentPresenter = (ChartAdornmentPresenter) null;
    }
    this.AdorningCanvas.ReleaseMouseCapture();
  }

  protected internal override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    FrameworkElement originalSource = e.OriginalSource as FrameworkElement;
    ChartSegment chartSegment = (ChartSegment) null;
    this.ChartArea.CurrentSelectedSeries = (ChartSeriesBase) null;
    if (originalSource != null)
    {
      if (originalSource.Tag != null)
        chartSegment = originalSource.Tag as ChartSegment;
      else if (originalSource.TemplatedParent is ContentPresenter templatedParent && templatedParent.Content is ChartSegment && !(templatedParent.Content is ChartAdornment))
        chartSegment = templatedParent.Content as ChartSegment;
    }
    if (chartSegment == null || !(chartSegment.Series is ISegmentSelectable))
      return;
    this.mouseUnderSegment = chartSegment;
  }

  protected internal override void OnMouseMove(MouseEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    if (e.OriginalSource != null && (this.EnableSegmentSelection || this.EnableSeriesSelection))
    {
      FrameworkElement originalSource = e.OriginalSource as FrameworkElement;
      ChartSegment chartSegment = (ChartSegment) null;
      if (originalSource != null)
      {
        if (originalSource.Tag != null)
          chartSegment = originalSource.Tag as ChartSegment;
        else if (originalSource.TemplatedParent is ContentPresenter templatedParent && templatedParent.Content is ChartSegment && !(templatedParent.Content is ChartAdornment))
          chartSegment = templatedParent.Content as ChartSegment;
      }
      if (chartSegment is TrendlineSegment || originalSource != null && originalSource.DataContext is LegendItem)
        return;
      Image image = originalSource as Image;
      if (chartSegment != null && chartSegment.Series is ISegmentSelectable && !(chartSegment.Item is Trendline))
      {
        if (!(chartSegment.Series is ScatterSeries) && this.IsDraggableSeries(chartSegment.Series))
          return;
        if (!chartSegment.Series.IsLinear || this.EnableSeriesSelection)
          this.ChangeSelectionCursor(true);
        else
          this.ChangeSelectionCursor(false);
        if (this.SelectionMode != SelectionMode.MouseMove)
          return;
        if (!chartSegment.Series.IsSideBySide && chartSegment.Series is CartesianSeries && !(chartSegment.Series is ScatterSeries) && !(chartSegment.Series is BubbleSeries))
        {
          Point position = e.GetPosition((IInputElement) chartSegment.Series.ActualArea.GetAdorningCanvas());
          ChartDataPointInfo dataPoint = (chartSegment.Series as ChartSeries).GetDataPoint(position);
          this.OnMouseMoveSelection(chartSegment.Series, (object) dataPoint);
        }
        else
        {
          int num = !(chartSegment.Series is CircularSeriesBase) || double.IsNaN(((CircularSeriesBase) chartSegment.Series).GroupTo) ? chartSegment.Series.ActualData.IndexOf(chartSegment.Item) : chartSegment.Series.Segments.IndexOf(chartSegment);
          this.OnMouseMoveSelection(chartSegment.Series, (object) num);
        }
      }
      else if (e.OriginalSource is Shape && (e.OriginalSource as Shape).DataContext is ChartAdornmentContainer && ((e.OriginalSource as Shape).DataContext as ChartAdornmentContainer).Tag is int)
      {
        this.selectedAdornmentPresenter = VisualTreeHelper.GetParent((DependencyObject) ((e.OriginalSource as Shape).DataContext as ChartAdornmentContainer)) as ChartAdornmentPresenter;
        if (this.selectedAdornmentPresenter != null && this.IsDraggableSeries(this.selectedAdornmentPresenter.Series))
          return;
        this.ChangeSelectionCursor(true);
        if (this.SelectionMode != SelectionMode.MouseMove)
          return;
        this.index = (int) ((e.OriginalSource as Shape).DataContext as ChartAdornmentContainer).Tag;
        if (this.selectedAdornmentPresenter == null || !(this.selectedAdornmentPresenter.Series is ISegmentSelectable))
          return;
        this.OnMouseMoveSelection(this.selectedAdornmentPresenter.Series, (object) this.index);
      }
      else
      {
        switch (originalSource)
        {
          case Border _:
          case TextBlock _:
          case Shape _:
            this.ChangeSelectionCursor(false);
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            int num1 = e.OriginalSource is TextBlock ? 3 : 2;
            for (int index = 0; index < num1 && frameworkElement != null; ++index)
              frameworkElement = VisualTreeHelper.GetParent((DependencyObject) frameworkElement) as FrameworkElement;
            if (frameworkElement is ContentPresenter)
            {
              this.index = ChartExtensionUtils.GetAdornmentIndex((object) frameworkElement);
              if (this.index != -1)
                this.ChangeSelectionCursor(true);
              if (this.SelectionMode == SelectionMode.MouseMove)
              {
                frameworkElement = VisualTreeHelper.GetParent((DependencyObject) frameworkElement) as FrameworkElement;
                if (frameworkElement is ChartAdornmentPresenter || frameworkElement is ChartAdornmentContainer)
                {
                  while (!(frameworkElement is ChartAdornmentPresenter) && frameworkElement != null)
                    frameworkElement = VisualTreeHelper.GetParent((DependencyObject) frameworkElement) as FrameworkElement;
                  this.selectedAdornmentPresenter = frameworkElement as ChartAdornmentPresenter;
                  if (this.selectedAdornmentPresenter != null)
                  {
                    if (this.IsDraggableSeries(this.selectedAdornmentPresenter.Series))
                      break;
                    if (this.selectedAdornmentPresenter.Series is ISegmentSelectable)
                      this.OnMouseMoveSelection(this.selectedAdornmentPresenter.Series, (object) this.index);
                  }
                }
              }
            }
            if (!(frameworkElement is ContentControl contentControl) || !(contentControl.Tag is int))
              break;
            this.ChangeSelectionCursor(true);
            if (this.SelectionMode != SelectionMode.MouseMove)
              break;
            this.index = (int) contentControl.Tag;
            if (!(VisualTreeHelper.GetParent((DependencyObject) frameworkElement) as FrameworkElement is ChartAdornmentPresenter parent))
              break;
            this.selectedAdornmentPresenter = parent;
            if (this.IsDraggableSeries(this.selectedAdornmentPresenter.Series) || this.selectedAdornmentPresenter == null || !(this.selectedAdornmentPresenter.Series is ISegmentSelectable))
              break;
            this.OnMouseMoveSelection(this.selectedAdornmentPresenter.Series, (object) this.index);
            break;
          default:
            if (image != null && image.Source is WriteableBitmap)
            {
              this.GetBitmapSeriesCollection(originalSource, e);
              if (this.SelectionMode != SelectionMode.MouseMove)
                break;
              this.OnBitmapSeriesMouseMoveSelection(originalSource, e);
              break;
            }
            if (this.ChartArea.PreviousSelectedSeries != null && this.ChartArea.CurrentSelectedSeries != null && this.SelectionMode == SelectionMode.MouseMove && this.ChartArea.VisibleSeries.Contains(this.ChartArea.PreviousSelectedSeries))
            {
              this.ChangeSelectionCursor(false);
              if (!this.EnableSeriesSelection ? this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(-1, (this.ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex) : this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(-1, this.ChartArea.SeriesSelectedIndex))
                break;
              this.Deselect();
              break;
            }
            this.ChangeSelectionCursor(false);
            break;
        }
      }
    }
    else
      this.ChangeSelectionCursor(false);
  }

  protected internal override void OnMouseLeave(MouseEventArgs e)
  {
    if (this.ChartArea == null)
      return;
    this.ChangeSelectionCursor(false);
  }

  protected internal virtual void OnSelectionChanging(ChartSelectionChangingEventArgs eventArgs)
  {
  }

  protected internal virtual void OnSelectionChanged(ChartSelectionChangedEventArgs eventArgs)
  {
  }

  protected override DependencyObject CloneBehavior(DependencyObject obj)
  {
    return base.CloneBehavior((DependencyObject) new ChartSelectionBehavior()
    {
      EnableSeriesSelection = this.EnableSeriesSelection,
      EnableSegmentSelection = this.EnableSegmentSelection,
      SelectionMode = this.SelectionMode,
      SelectionStyle = this.SelectionStyle,
      SelectionCursor = this.SelectionCursor
    });
  }

  private static void OnEnableSeriesSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    SfChart chartArea = (d as ChartSelectionBehavior).ChartArea;
    if (chartArea != null && !(bool) e.NewValue)
    {
      foreach (ChartSeries series in (Collection<ChartSeries>) chartArea.Series)
      {
        if (chartArea.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
        {
          chartArea.SelectedSeriesCollection.Remove((ChartSeriesBase) series);
          chartArea.OnResetSeries(series);
        }
      }
      chartArea.SeriesSelectedIndex = -1;
      chartArea.SelectedSeriesCollection.Clear();
    }
    else
    {
      if (chartArea == null || !(bool) e.NewValue || chartArea.SeriesSelectedIndex == -1)
        return;
      chartArea.SeriesSelectedIndexChanged(chartArea.SeriesSelectedIndex, -1);
    }
  }

  private static void OnEnableSegmentSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartBase chartArea = (ChartBase) (d as ChartSelectionBehavior).ChartArea;
    if (chartArea != null && !(bool) e.NewValue)
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) chartArea.VisibleSeries)
      {
        for (int index = 0; index < chartSeriesBase.ActualData.Count; ++index)
        {
          if (chartSeriesBase.SelectedSegmentsIndexes.Contains(index))
          {
            chartSeriesBase.SelectedSegmentsIndexes.Remove(index);
            chartSeriesBase.OnResetSegment(index);
          }
        }
        if (chartSeriesBase is ISegmentSelectable segmentSelectable)
          segmentSelectable.SelectedIndex = -1;
        chartSeriesBase.SelectedSegmentsIndexes.Clear();
      }
    }
    else
    {
      if (chartArea == null || !(bool) e.NewValue)
        return;
      for (int index = 0; index < chartArea.VisibleSeries.Count; ++index)
      {
        ChartSeriesBase chartSeriesBase = chartArea.VisibleSeries[index];
        if (chartSeriesBase is ISegmentSelectable segmentSelectable && segmentSelectable.SelectedIndex != -1)
          chartSeriesBase.SelectedIndexChanged(segmentSelectable.SelectedIndex, -1);
      }
    }
  }

  private static void OnSelectionStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    SfChart chartArea = (d as ChartSelectionBehavior).ChartArea;
    if (chartArea == null || chartArea.Series == null)
      return;
    chartArea.SeriesSelectedIndex = -1;
    foreach (ChartSeries series in (Collection<ChartSeries>) chartArea.Series)
    {
      if (series is ISegmentSelectable segmentSelectable)
        segmentSelectable.SelectedIndex = -1;
      if (chartArea.SelectedSeriesCollection.Contains((ChartSeriesBase) series) && chartArea.SeriesSelectedIndex != chartArea.Series.IndexOf(series))
      {
        chartArea.SelectedSeriesCollection.Remove((ChartSeriesBase) series);
        chartArea.OnResetSeries(series);
      }
      for (int index = 0; index < series.ActualData.Count; ++index)
      {
        if (series.SelectedSegmentsIndexes.Contains(index))
        {
          series.SelectedSegmentsIndexes.Remove(index);
          series.OnResetSegment(index);
        }
      }
    }
  }

  private void Deselect()
  {
    (this.ChartArea.PreviousSelectedSeries as ISegmentSelectable).SelectedIndex = -1;
    this.ChartArea.SeriesSelectedIndex = -1;
    this.ChartArea.PreviousSelectedSeries = (ChartSeriesBase) null;
    this.ChartArea.CurrentSelectedSeries = (ChartSeriesBase) null;
    this.seriesCollection = (List<ChartSeries>) null;
  }

  private void ChangeSelectionCursor(bool isCursorChanged)
  {
    if (isCursorChanged)
    {
      if (Mouse.OverrideCursor != null)
        return;
      Mouse.OverrideCursor = this.SelectionCursor;
    }
    else
    {
      if (Mouse.OverrideCursor == null)
        return;
      Mouse.OverrideCursor = (Cursor) null;
    }
  }

  private bool IsDraggableSeries(ChartSeriesBase chartSeries)
  {
    if (!ChartExtensionUtils.IsDraggable(chartSeries))
      return false;
    this.ChangeSelectionCursor(false);
    return true;
  }

  private void OnMouseMoveSelection(ChartSeriesBase series, object value)
  {
    if (this.SelectionStyle != SelectionStyle.Single)
      return;
    this.ChartArea.CurrentSelectedSeries = series;
    int num;
    if (this.EnableSeriesSelection)
    {
      if (!series.IsSideBySide)
      {
        switch (series)
        {
          case ScatterSeries _:
          case BubbleSeries _:
          case AccumulationSeriesBase _:
          case FastScatterBitmapSeries _:
            break;
          default:
            num = series.IsSideBySide ? 0 : (!this.EnableSegmentSelection ? 1 : (!this.EnableSegmentSelection ? 0 : (value == null ? 1 : 0)));
            goto label_8;
        }
      }
      num = 1;
    }
    else
      num = 0;
label_8:
    bool flag = num != 0;
    ChartDataPointInfo chartDataPointInfo = value as ChartDataPointInfo;
    if (flag)
    {
      if (this.ChartArea.PreviousSelectedSeries != null && this.ChartArea.PreviousSelectedSeries != this.ChartArea.CurrentSelectedSeries)
        (this.ChartArea.PreviousSelectedSeries as ISegmentSelectable).SelectedIndex = -1;
      int newIndex = this.ChartArea.VisibleSeries.IndexOf(series);
      this.ChartArea.CurrentSelectedSeries.selectionChangingEventArgs.IsDataPointSelection = false;
      if (this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(newIndex, this.ChartArea.SeriesSelectedIndex))
        return;
      this.ChartArea.SeriesSelectedIndex = newIndex;
      this.ChartArea.PreviousSelectedSeries = this.ChartArea.CurrentSelectedSeries;
    }
    else if (this.ChartArea.CurrentSelectedSeries is ISegmentSelectable && this.EnableSegmentSelection && (value != null && value.GetType() == typeof (int) || chartDataPointInfo != null))
    {
      this.ChartArea.SeriesSelectedIndex = -1;
      if (this.ChartArea.PreviousSelectedSeries != null && this.ChartArea.PreviousSelectedSeries != this.ChartArea.CurrentSelectedSeries)
        (this.ChartArea.PreviousSelectedSeries as ISegmentSelectable).SelectedIndex = -1;
      int newIndex = value.GetType() == typeof (int) ? (int) value : chartDataPointInfo.Index;
      this.ChartArea.CurrentSelectedSeries.selectionChangingEventArgs.IsDataPointSelection = true;
      if (this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(newIndex, (this.ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex))
        return;
      (this.ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex = newIndex;
      this.ChartArea.PreviousSelectedSeries = this.ChartArea.CurrentSelectedSeries;
    }
    else
    {
      if (this.ChartArea.PreviousSelectedSeries == null || !this.ChartArea.VisibleSeries.Contains(this.ChartArea.PreviousSelectedSeries) || (!flag ? this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(-1, (this.ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex) : this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(-1, this.ChartArea.SeriesSelectedIndex)))
        return;
      this.Deselect();
    }
  }

  private void OnMouseDownSelection(ChartSeriesBase series, object value)
  {
    bool flag1 = series is ScatterSeries;
    if (!flag1 && this.IsDraggableSeries(series))
      return;
    this.ChartArea.CurrentSelectedSeries = series;
    int num;
    if (this.EnableSeriesSelection)
    {
      if (!series.IsSideBySide && !flag1)
      {
        switch (series)
        {
          case BubbleSeries _:
          case AccumulationSeriesBase _:
          case FastScatterBitmapSeries _:
          case FastLineSeries _:
            break;
          default:
            num = series.IsSideBySide ? 0 : (!this.EnableSegmentSelection ? 1 : (!this.EnableSegmentSelection ? 0 : (value == null ? 1 : 0)));
            goto label_8;
        }
      }
      num = 1;
    }
    else
      num = 0;
label_8:
    bool flag2 = num != 0;
    ChartDataPointInfo chartDataPointInfo = value as ChartDataPointInfo;
    if (flag2)
    {
      int newIndex = this.ChartArea.VisibleSeries.IndexOf(series);
      this.ChartArea.CurrentSelectedSeries.selectionChangingEventArgs.IsDataPointSelection = false;
      if (this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(newIndex, this.ChartArea.SeriesSelectedIndex))
        return;
      if (this.SelectionStyle != SelectionStyle.Single && this.ChartArea.SelectedSeriesCollection.Contains(this.ChartArea.CurrentSelectedSeries))
      {
        this.ChartArea.SelectedSeriesCollection.Remove(this.ChartArea.CurrentSelectedSeries);
        this.ChartArea.SeriesSelectedIndex = -1;
        this.ChartArea.OnResetSeries(this.ChartArea.CurrentSelectedSeries as ChartSeries);
      }
      else if (this.ChartArea.SeriesSelectedIndex == newIndex)
      {
        this.ChartArea.SeriesSelectedIndex = -1;
      }
      else
      {
        this.ChartArea.SeriesSelectedIndex = newIndex;
        this.ChartArea.PreviousSelectedSeries = this.ChartArea.CurrentSelectedSeries;
      }
    }
    else
    {
      if (!(this.ChartArea.CurrentSelectedSeries is ISegmentSelectable) || !this.EnableSegmentSelection || (value == null || !(value.GetType() == typeof (int))) && chartDataPointInfo == null)
        return;
      int newIndex = value.GetType() == typeof (int) ? (int) value : chartDataPointInfo.Index;
      this.ChartArea.CurrentSelectedSeries.selectionChangingEventArgs.IsDataPointSelection = true;
      if (this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(newIndex, (this.ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex))
        return;
      if (this.SelectionStyle != SelectionStyle.Single)
      {
        if (this.ChartArea.CurrentSelectedSeries.SelectedSegmentsIndexes.Contains(newIndex))
        {
          this.ChartArea.CurrentSelectedSeries.SelectedSegmentsIndexes.Remove(newIndex);
        }
        else
        {
          this.ChartArea.CurrentSelectedSeries.SelectedSegmentsIndexes.Add(newIndex);
          this.ChartArea.PreviousSelectedSeries = this.ChartArea.CurrentSelectedSeries;
        }
      }
      else
      {
        ISegmentSelectable currentSelectedSeries = this.ChartArea.CurrentSelectedSeries as ISegmentSelectable;
        if (currentSelectedSeries.SelectedIndex == newIndex)
        {
          currentSelectedSeries.SelectedIndex = -1;
        }
        else
        {
          currentSelectedSeries.SelectedIndex = newIndex;
          this.ChartArea.PreviousSelectedSeries = this.ChartArea.CurrentSelectedSeries;
        }
      }
    }
  }

  private void OnBitmapSeriesMouseMoveSelection(FrameworkElement element, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this.ChartArea.GetAdorningCanvas());
    if (this.seriesCollection.Count > 0)
    {
      this.ChartArea.CurrentSelectedSeries = (ChartSeriesBase) this.seriesCollection[this.seriesCollection.Count - 1];
      if (this.IsDraggableSeries(this.ChartArea.CurrentSelectedSeries) || !(this.ChartArea.CurrentSelectedSeries is ISegmentSelectable))
        return;
      this.OnMouseMoveSelection(this.ChartArea.CurrentSelectedSeries, (object) this.ChartArea.CurrentSelectedSeries.GetDataPoint(position));
    }
    else
    {
      if (this.ChartArea.PreviousSelectedSeries == null || !this.ChartArea.VisibleSeries.Contains(this.ChartArea.PreviousSelectedSeries) || (!this.EnableSeriesSelection ? this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(-1, (this.ChartArea.CurrentSelectedSeries as ISegmentSelectable).SelectedIndex) : this.ChartArea.CurrentSelectedSeries.RaiseSelectionChanging(-1, this.ChartArea.SeriesSelectedIndex)))
        return;
      this.Deselect();
    }
  }

  private void GetBitmapSeriesCollection(FrameworkElement element, MouseEventArgs e)
  {
    Image relativeTo = element as Image;
    Point position1 = e.GetPosition((IInputElement) relativeTo);
    int position = (relativeTo.Source as WriteableBitmap).PixelWidth * (int) position1.Y + (int) position1.X;
    if (!this.ChartArea.isBitmapPixelsConverted)
      this.ChartArea.ConvertBitmapPixels();
    this.seriesCollection = this.ChartArea.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series => series.Pixels.Count > 0 && series.Pixels.Contains(position))).ToList<ChartSeries>();
    if (this.seriesCollection.Count > 0)
    {
      foreach (ChartSeriesBase series in this.seriesCollection)
      {
        if (!series.IsLinear || this.EnableSeriesSelection)
          this.ChangeSelectionCursor(true);
      }
    }
    else
      this.ChangeSelectionCursor(false);
  }

  private void OnBitmapSeriesMouseDownSelection(FrameworkElement element, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this.ChartArea.GetAdorningCanvas());
    if (this.seriesCollection.Count <= 0)
      return;
    this.ChartArea.CurrentSelectedSeries = (ChartSeriesBase) this.seriesCollection[this.seriesCollection.Count - 1];
    if (!(this.ChartArea.CurrentSelectedSeries is ISegmentSelectable))
      return;
    this.OnMouseDownSelection(this.ChartArea.CurrentSelectedSeries, (object) this.ChartArea.CurrentSelectedSeries.GetDataPoint(position));
  }
}
