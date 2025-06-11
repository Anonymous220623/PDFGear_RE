// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAdornmentPresenter
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAdornmentPresenter : Canvas
{
  public static readonly DependencyProperty VisibleSeriesProperty = DependencyProperty.Register(nameof (VisibleSeries), typeof (ObservableCollection<ChartSeriesBase>), typeof (ChartAdornmentPresenter), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAdornmentPresenter.OnVisibleSeriesPropertyChanged)));
  public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(nameof (Series), typeof (ChartSeriesBase), typeof (ChartAdornmentPresenter), new PropertyMetadata((PropertyChangedCallback) null));
  private List<int> resetIndexes = new List<int>();

  public ObservableCollection<ChartSeriesBase> VisibleSeries
  {
    get
    {
      return (ObservableCollection<ChartSeriesBase>) this.GetValue(ChartAdornmentPresenter.VisibleSeriesProperty);
    }
    set => this.SetValue(ChartAdornmentPresenter.VisibleSeriesProperty, (object) value);
  }

  public ChartSeriesBase Series
  {
    get => (ChartSeriesBase) this.GetValue(ChartAdornmentPresenter.SeriesProperty);
    set => this.SetValue(ChartAdornmentPresenter.SeriesProperty, (object) value);
  }

  internal void UpdateAdornmentSelection(
    List<int> selectedAdornmentIndexes,
    bool isDataPointSelection)
  {
    Brush brush = (Brush) null;
    if (this.Series.ActualArea.GetEnableSeriesSelection() && !isDataPointSelection)
      brush = this.Series.ActualArea.GetSeriesSelectionBrush(this.Series);
    else if (this.Series.ActualArea.GetEnableSegmentSelection())
      brush = (this.Series as ISegmentSelectable).SegmentSelectionBrush;
    if (brush == null || selectedAdornmentIndexes == null || selectedAdornmentIndexes.Count <= 0)
      return;
    foreach (int selectedAdornmentIndex in selectedAdornmentIndexes)
    {
      ChartAdornmentInfoBase adornmentInfo = this.Series.adornmentInfo;
      if (adornmentInfo.ShowLabel && adornmentInfo.LabelTemplate == null && (adornmentInfo.UseSeriesPalette || adornmentInfo.Background != null || adornmentInfo.BorderBrush != null))
      {
        Border border = (Border) null;
        if (adornmentInfo.LabelPresenters.Count > 0 && selectedAdornmentIndex <= adornmentInfo.LabelPresenters.Count - 1 && VisualTreeHelper.GetChildrenCount((DependencyObject) adornmentInfo.LabelPresenters[selectedAdornmentIndex]) > 0)
        {
          ContentPresenter child = VisualTreeHelper.GetChild((DependencyObject) adornmentInfo.LabelPresenters[selectedAdornmentIndex], 0) as ContentPresenter;
          if (VisualTreeHelper.GetChildrenCount((DependencyObject) child) > 0)
            border = VisualTreeHelper.GetChild((DependencyObject) child, 0) as Border;
        }
        if (border != null)
        {
          if (border.Background != null)
            border.Background = brush;
          if (border.BorderBrush != null)
            border.BorderBrush = brush;
        }
        ChartAdornment adornment = this.Series.Adornments[selectedAdornmentIndex];
        if (adornment.ContrastForeground != null)
        {
          adornment.Foreground = brush.GetContrastColor();
          adornment.ContrastForeground = adornment.Foreground;
        }
      }
      if (adornmentInfo.ShowMarker && adornmentInfo.adormentContainers.Count > 0 && selectedAdornmentIndex <= adornmentInfo.adormentContainers.Count - 1)
      {
        ChartAdornmentContainer adormentContainer = adornmentInfo.adormentContainers[selectedAdornmentIndex];
        if (adormentContainer != null && adormentContainer.PredefinedSymbol != null)
        {
          adormentContainer.PredefinedSymbol.Background = brush;
          adormentContainer.PredefinedSymbol.BorderBrush = brush;
        }
      }
      if (adornmentInfo.ConnectorLines.Count > 0 && selectedAdornmentIndex <= adornmentInfo.ConnectorLines.Count - 1 && adornmentInfo.ShowConnectorLine)
      {
        Path connectorLine = adornmentInfo.ConnectorLines[selectedAdornmentIndex];
        if (connectorLine != null)
          connectorLine.Stroke = brush;
      }
    }
  }

  internal void ResetAdornmentSelection(int? selectedIndex, bool isResetAll)
  {
    if (!isResetAll && selectedIndex.HasValue && this.Series.ActualArea.SelectedSeriesCollection.Contains(this.Series) && this.Series.ActualArea.GetEnableSeriesSelection())
    {
      this.UpdateAdornmentSelection(this.Series.Adornments.Where<ChartAdornment>((Func<ChartAdornment, bool>) (adorment => this.Series.ActualData[selectedIndex.Value] == adorment.Item)).Select<ChartAdornment, int>((Func<ChartAdornment, int>) (adorment => this.Series.Adornments.IndexOf(adorment))).ToList<int>(), false);
    }
    else
    {
      int? nullable = selectedIndex;
      int dataCount = this.Series.DataCount;
      if ((nullable.GetValueOrDefault() >= dataCount ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
      {
        this.resetIndexes.Clear();
        if (selectedIndex.HasValue && !isResetAll)
          this.resetIndexes = !(this.Series is CircularSeriesBase) || double.IsNaN(((CircularSeriesBase) this.Series).GroupTo) ? (!(this.Series.ActualXAxis is CategoryAxis) || (this.Series.ActualXAxis as CategoryAxis).IsIndexed || !this.Series.IsSideBySide || this.Series is RangeSeriesBase || this.Series is FinancialSeriesBase || this.Series is WaterfallSeries ? this.Series.Adornments.Where<ChartAdornment>((Func<ChartAdornment, bool>) (adorment => this.Series.ActualData[selectedIndex.Value] == adorment.Item)).Select<ChartAdornment, int>((Func<ChartAdornment, int>) (adorment => this.Series.Adornments.IndexOf(adorment))).ToList<int>() : this.Series.Adornments.Where<ChartAdornment>((Func<ChartAdornment, bool>) (adorment => this.Series.GroupedActualData[selectedIndex.Value] == adorment.Item)).Select<ChartAdornment, int>((Func<ChartAdornment, int>) (adorment => this.Series.Adornments.IndexOf(adorment))).ToList<int>()) : this.Series.Adornments.Where<ChartAdornment>((Func<ChartAdornment, bool>) (adorment => this.Series.Segments[selectedIndex.Value].Item == adorment.Item)).Select<ChartAdornment, int>((Func<ChartAdornment, int>) (adorment => this.Series.Adornments.IndexOf(adorment))).ToList<int>();
        else if (isResetAll)
          this.resetIndexes = this.Series.Adornments.Select<ChartAdornment, int>((Func<ChartAdornment, int>) (adorment => this.Series.Adornments.IndexOf(adorment))).ToList<int>();
        foreach (int resetIndex in this.resetIndexes)
          this.UpdateAdormentBackground(resetIndex);
      }
      else
      {
        for (int index = 0; index < this.Series.Adornments.Count; ++index)
          this.UpdateAdormentBackground(index);
      }
    }
  }

  private void UpdateAdormentBackground(int index)
  {
    if (this.Series.adornmentInfo.LabelPresenters.Count > index && this.Series.adornmentInfo.ShowLabel && (this.Series.adornmentInfo.UseSeriesPalette || this.Series.adornmentInfo.Background != null || this.Series.adornmentInfo.BorderBrush != null))
    {
      Border border = (Border) null;
      if (VisualTreeHelper.GetChildrenCount((DependencyObject) this.Series.adornmentInfo.LabelPresenters[index]) > 0)
      {
        ContentPresenter child = VisualTreeHelper.GetChild((DependencyObject) this.Series.adornmentInfo.LabelPresenters[index], 0) as ContentPresenter;
        if (VisualTreeHelper.GetChildrenCount((DependencyObject) child) > 0)
          border = VisualTreeHelper.GetChild((DependencyObject) child, 0) as Border;
      }
      ChartAdornment adornment = this.Series.Adornments[index];
      if (border != null)
      {
        if (this.Series.Adornments[index].Background != null)
          border.Background = adornment.Background;
        else if (this.Series.adornmentInfo.UseSeriesPalette)
          border.Background = adornment.Interior;
        if (this.Series.adornmentInfo.BorderBrush != null)
          border.BorderBrush = adornment.BorderBrush;
        else if (this.Series.adornmentInfo.UseSeriesPalette)
          border.BorderBrush = adornment.Interior;
      }
      this.Series.adornmentInfo.UpdateForeground(adornment);
    }
    if (this.Series.adornmentInfo.ConnectorLines.Count > index && this.Series.adornmentInfo.ShowConnectorLine)
    {
      Path connectorLine = this.Series.adornmentInfo.ConnectorLines[index];
      if (this.Series.adornmentInfo.UseSeriesPalette && this.Series.adornmentInfo.ConnectorLineStyle == null)
        connectorLine.Stroke = this.Series.Adornments[index].Interior;
      else
        connectorLine.ClearValue(Shape.StrokeProperty);
    }
    if (this.Series.adornmentInfo.adormentContainers.Count <= index)
      return;
    SymbolControl predefinedSymbol = this.Series.adornmentInfo.adormentContainers[index].PredefinedSymbol;
    if (predefinedSymbol == null || !this.Series.adornmentInfo.ShowMarker)
      return;
    if (this.Series.adornmentInfo.SymbolInterior == null)
      predefinedSymbol.SetBinding(Control.BackgroundProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series.Adornments[index],
        Path = new PropertyPath("Interior", new object[0])
      });
    else
      predefinedSymbol.SetBinding(Control.BackgroundProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series.adornmentInfo,
        Path = new PropertyPath("SymbolInterior", new object[0])
      });
    if (this.Series.adornmentInfo.SymbolStroke == null)
      predefinedSymbol.SetBinding(Control.BorderBrushProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series.Adornments[index],
        Path = new PropertyPath("Interior", new object[0])
      });
    else
      predefinedSymbol.SetBinding(Control.BorderBrushProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series.adornmentInfo,
        Path = new PropertyPath("SymbolStroke", new object[0])
      });
  }

  internal void Update(Size availableSize)
  {
    if (this.Series == null || this.Series.adornmentInfo == null)
      return;
    this.Series.adornmentInfo.Measure(availableSize, (Panel) this);
  }

  internal void Arrange(Size finalSize)
  {
    if (this.Series == null || this.Series.adornmentInfo == null)
      return;
    this.Series.adornmentInfo.Arrange(finalSize);
  }

  private static void OnVisibleSeriesPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }
}
