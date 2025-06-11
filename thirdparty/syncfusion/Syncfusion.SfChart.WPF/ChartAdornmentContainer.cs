// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAdornmentContainer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAdornmentContainer : Panel
{
  public static readonly DependencyProperty LabelVerticalAlignmentProperty = DependencyProperty.Register(nameof (LabelVerticalAlignment), typeof (VerticalAlignment), typeof (ChartAdornmentContainer), new PropertyMetadata((object) VerticalAlignment.Center));
  public static readonly DependencyProperty LabelHorizontalAlignmentProperty = DependencyProperty.Register(nameof (LabelHorizontalAlignment), typeof (HorizontalAlignment), typeof (ChartAdornmentContainer), new PropertyMetadata((object) HorizontalAlignment.Center));
  public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof (Symbol), typeof (object), typeof (ChartAdornmentContainer), new PropertyMetadata((object) ChartSymbol.Custom, new PropertyChangedCallback(ChartAdornmentContainer.OnAdornmentsInfoChanged)));
  private Point m_symbolOffset = new Point();
  private ContentPresenter m_symbolPresenter = new ContentPresenter();
  private ChartAdornment adornment;

  public VerticalAlignment LabelVerticalAlignment
  {
    get
    {
      return (VerticalAlignment) this.GetValue(ChartAdornmentContainer.LabelVerticalAlignmentProperty);
    }
    set => this.SetValue(ChartAdornmentContainer.LabelVerticalAlignmentProperty, (object) value);
  }

  public HorizontalAlignment LabelHorizontalAlignment
  {
    get
    {
      return (HorizontalAlignment) this.GetValue(ChartAdornmentContainer.LabelHorizontalAlignmentProperty);
    }
    set => this.SetValue(ChartAdornmentContainer.LabelHorizontalAlignmentProperty, (object) value);
  }

  public ChartSymbol Symbol
  {
    get => (ChartSymbol) this.GetValue(ChartAdornmentContainer.SymbolProperty);
    set => this.SetValue(ChartAdornmentContainer.SymbolProperty, (object) value);
  }

  public Point SymbolOffset => this.m_symbolOffset;

  internal ChartAdornment Adornment
  {
    get => this.adornment;
    set
    {
      if (value != this.adornment)
      {
        this.adornment = value;
        this.UpdateContainers(true);
      }
      else
        this.UpdateContainers(false);
    }
  }

  internal SymbolControl PredefinedSymbol { get; set; }

  public ChartAdornmentContainer() => this.PredefinedSymbol = new SymbolControl();

  public ChartAdornmentContainer(ChartAdornment adornment)
  {
    this.PredefinedSymbol = new SymbolControl();
    this.Adornment = adornment;
  }

  internal void Dispose()
  {
    this.m_symbolPresenter.SetValue(ContentPresenter.ContentTemplateProperty, (object) null);
    this.m_symbolPresenter.SetValue(ContentPresenter.ContentProperty, (object) null);
    this.adornment = (ChartAdornment) null;
    this.PredefinedSymbol = (SymbolControl) null;
    this.m_symbolPresenter = (ContentPresenter) null;
  }

  internal void UpdateContainers(bool setBinding)
  {
    if (this.Adornment != null)
    {
      ChartAdornmentInfoBase adornmentInfo = this.Adornment.Series.adornmentInfo;
      this.LabelVerticalAlignment = adornmentInfo.VerticalAlignment;
      this.LabelHorizontalAlignment = adornmentInfo.HorizontalAlignment;
      this.Symbol = !adornmentInfo.IsAdornmentLabelCreatedEventHooked || this.Adornment.CustomAdornmentLabel == null ? adornmentInfo.Symbol : this.Adornment.CustomAdornmentLabel.Symbol;
      this.SetSymbol(this.Symbol.ToString());
      if (!setBinding)
        return;
      this.SetContentBinding(this.Adornment);
    }
    else
    {
      if (this.Children.Contains((UIElement) this.m_symbolPresenter))
        this.Children.Remove((UIElement) this.m_symbolPresenter);
      if (!this.Children.Contains((UIElement) this.PredefinedSymbol))
        return;
      this.Children.Remove((UIElement) this.PredefinedSymbol);
    }
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    Rect finalRect = new Rect(new Point(), this.DesiredSize);
    Rect rect = new Rect(new Point(), this.DesiredSize);
    switch (this.LabelHorizontalAlignment)
    {
      case HorizontalAlignment.Left:
        this.m_symbolPresenter.HorizontalAlignment = HorizontalAlignment.Right;
        this.PredefinedSymbol.HorizontalAlignment = HorizontalAlignment.Right;
        break;
      case HorizontalAlignment.Center:
      case HorizontalAlignment.Stretch:
        this.m_symbolPresenter.HorizontalAlignment = HorizontalAlignment.Center;
        this.PredefinedSymbol.HorizontalAlignment = HorizontalAlignment.Center;
        break;
      case HorizontalAlignment.Right:
        this.m_symbolPresenter.HorizontalAlignment = HorizontalAlignment.Left;
        this.PredefinedSymbol.HorizontalAlignment = HorizontalAlignment.Left;
        break;
    }
    switch (this.LabelVerticalAlignment)
    {
      case VerticalAlignment.Top:
        this.m_symbolPresenter.VerticalAlignment = VerticalAlignment.Bottom;
        this.PredefinedSymbol.VerticalAlignment = VerticalAlignment.Bottom;
        break;
      case VerticalAlignment.Center:
      case VerticalAlignment.Stretch:
        this.m_symbolPresenter.VerticalAlignment = VerticalAlignment.Center;
        this.PredefinedSymbol.VerticalAlignment = VerticalAlignment.Center;
        break;
      case VerticalAlignment.Bottom:
        this.m_symbolPresenter.VerticalAlignment = VerticalAlignment.Top;
        this.PredefinedSymbol.VerticalAlignment = VerticalAlignment.Top;
        break;
    }
    this.m_symbolPresenter.Arrange(finalRect);
    this.PredefinedSymbol.Arrange(finalRect);
    return this.DesiredSize;
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    if (this.Adornment == null)
      return ChartLayoutUtils.CheckSize(availableSize);
    this.PredefinedSymbol.Measure(availableSize);
    this.m_symbolPresenter.Measure(availableSize);
    Size size1 = new Size();
    Size empty = Size.Empty;
    Size size2 = this.Adornment.Series.adornmentInfo.Symbol != ChartSymbol.Custom ? this.PredefinedSymbol.DesiredSize : this.m_symbolPresenter.DesiredSize;
    switch (this.LabelHorizontalAlignment)
    {
      case HorizontalAlignment.Left:
        size1.Width = Math.Max(empty.Width, size2.Width);
        this.m_symbolOffset.X = 0.5 * size1.Width;
        break;
      case HorizontalAlignment.Center:
      case HorizontalAlignment.Stretch:
        size1.Width = Math.Max(empty.Width, size2.Width);
        this.m_symbolOffset.X = 0.5 * size1.Width;
        break;
      case HorizontalAlignment.Right:
        size1.Width = Math.Max(empty.Width, size2.Width);
        this.m_symbolOffset.X = 0.5 * size1.Width;
        break;
    }
    switch (this.LabelVerticalAlignment)
    {
      case VerticalAlignment.Top:
        size1.Height = Math.Max(empty.Height, size2.Height);
        this.m_symbolOffset.Y = 0.5 * size1.Height;
        break;
      case VerticalAlignment.Center:
      case VerticalAlignment.Stretch:
        size1.Height = Math.Max(empty.Height, size2.Height);
        this.m_symbolOffset.Y = 0.5 * size1.Height;
        break;
      case VerticalAlignment.Bottom:
        size1.Height = Math.Max(empty.Height, size2.Height);
        this.m_symbolOffset.Y = 0.5 * size1.Height;
        break;
    }
    return size1;
  }

  private static void OnAdornmentsInfoChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAdornmentContainer adornmentContainer = d as ChartAdornmentContainer;
    adornmentContainer.SetSymbol(adornmentContainer.Symbol.ToString());
  }

  private void SetContentBinding(ChartAdornment adornment)
  {
    ChartAdornmentInfoBase adornmentInfo = adornment.Series.adornmentInfo;
    if (!adornmentInfo.ShowMarker)
      return;
    object source = (object) adornmentInfo;
    ChartAdornmentInfo customAdornmentLabel = adornment.CustomAdornmentLabel;
    if (customAdornmentLabel != null)
      source = (object) customAdornmentLabel;
    this.SetBinding(ChartAdornmentContainer.SymbolProperty, (BindingBase) this.CreateAdormentBinding("Symbol", source));
    this.PredefinedSymbol.SetBinding(FrameworkElement.HeightProperty, (BindingBase) this.CreateAdormentBinding("SymbolHeight", source));
    this.PredefinedSymbol.SetBinding(FrameworkElement.WidthProperty, (BindingBase) this.CreateAdormentBinding("SymbolWidth", source));
    if (adornment.Series is ChartSeries && adornment.Series.ActualArea.SelectedSeriesCollection.Contains(adornment.Series) && adornmentInfo.HighlightOnSelection && adornment.Series.ActualArea.GetSeriesSelectionBrush(adornment.Series) != null && adornment.Series.ActualArea.GetEnableSeriesSelection())
      this.PredefinedSymbol.SetBinding(Control.BackgroundProperty, (BindingBase) this.CreateAdormentBinding("SeriesSelectionBrush", (object) adornment.Series));
    else if (adornment.Series is ISegmentSelectable && adornment.Series.SelectedSegmentsIndexes.Contains(adornment.Series.ActualData.IndexOf(adornment.Item)) && adornmentInfo.HighlightOnSelection && adornment.Series.ActualArea.GetEnableSegmentSelection() && (adornment.Series as ISegmentSelectable).SegmentSelectionBrush != null)
      this.PredefinedSymbol.SetBinding(Control.BackgroundProperty, (BindingBase) this.CreateAdormentBinding("SegmentSelectionBrush", (object) adornment.Series));
    else if (customAdornmentLabel != null && customAdornmentLabel.SymbolInterior != adornmentInfo.SymbolInterior)
      this.PredefinedSymbol.SetBinding(Control.BackgroundProperty, (BindingBase) this.CreateAdormentBinding("SymbolInterior", (object) customAdornmentLabel));
    else if (adornmentInfo.SymbolInterior == null)
      this.PredefinedSymbol.SetBinding(Control.BackgroundProperty, (BindingBase) this.CreateAdormentBinding("Interior", (object) adornment));
    else
      this.PredefinedSymbol.SetBinding(Control.BackgroundProperty, (BindingBase) this.CreateAdormentBinding("SymbolInterior", (object) adornmentInfo));
    if (adornment.Series is ChartSeries && adornment.Series.ActualArea.SelectedSeriesCollection.Contains(adornment.Series) && adornmentInfo.HighlightOnSelection && adornment.Series.ActualArea.GetSeriesSelectionBrush(adornment.Series) != null && adornment.Series.ActualArea.GetEnableSeriesSelection())
      this.PredefinedSymbol.SetBinding(Control.BorderBrushProperty, (BindingBase) this.CreateAdormentBinding("SeriesSelectionBrush", (object) adornment.Series));
    else if (adornment.Series is ISegmentSelectable && adornment.Series.SelectedSegmentsIndexes.Contains(adornment.Series.ActualData.IndexOf(adornment.Item)) && adornmentInfo.HighlightOnSelection && adornment.Series.ActualArea.GetEnableSegmentSelection() && (adornment.Series as ISegmentSelectable).SegmentSelectionBrush != null)
      this.PredefinedSymbol.SetBinding(Control.BorderBrushProperty, (BindingBase) this.CreateAdormentBinding("SegmentSelectionBrush", (object) adornment.Series));
    else
      this.PredefinedSymbol.SetBinding(Control.BorderBrushProperty, (BindingBase) this.CreateAdormentBinding("SymbolStroke", source));
    if (adornmentInfo.SymbolTemplate != null)
      this.m_symbolPresenter.SetBinding(ContentPresenter.ContentTemplateProperty, (BindingBase) this.CreateAdormentBinding("SymbolTemplate", (object) adornmentInfo));
    this.m_symbolPresenter.SetBinding(ContentPresenter.ContentProperty, (BindingBase) this.CreateAdormentBinding("", (object) adornment));
  }

  private Binding CreateAdormentBinding(string path, object source)
  {
    Binding adormentBinding = new Binding();
    adormentBinding.Path = new PropertyPath(path, new object[0]);
    adormentBinding.Source = source;
    adormentBinding.Mode = BindingMode.OneWay;
    if (path == "SeriesSelectionBrush")
    {
      adormentBinding.Converter = (IValueConverter) new SeriesSelectionBrushConverter(this.adornment.Series);
      adormentBinding.ConverterParameter = (object) this.adornment.Series.ActualData.IndexOf(this.adornment.Item);
    }
    return adormentBinding;
  }

  private void SetSymbol(string symbol)
  {
    ChartAdornmentInfoBase adornmentInfo = this.adornment.Series.adornmentInfo;
    if (symbol != "Custom")
    {
      if (this.Children.Contains((UIElement) this.m_symbolPresenter))
        this.Children.Remove((UIElement) this.m_symbolPresenter);
      if (!this.Children.Contains((UIElement) this.PredefinedSymbol))
        this.Children.Add((UIElement) this.PredefinedSymbol);
      if (double.IsNaN(this.adornment.YData) || double.IsNaN(this.adornment.XData) || this.adornment.Series is AccumulationSeriesBase && this.adornment.YData == 0.0)
        this.Children.Remove((UIElement) this.PredefinedSymbol);
      this.PredefinedSymbol.DataContext = (object) this;
      this.PredefinedSymbol.Template = ChartDictionaries.GenericSymbolDictionary[(object) symbol] as ControlTemplate;
    }
    else
    {
      if (adornmentInfo.SymbolTemplate != null && !this.Children.Contains((UIElement) this.m_symbolPresenter))
        this.Children.Add((UIElement) this.m_symbolPresenter);
      if (this.Children.Contains((UIElement) this.PredefinedSymbol))
        this.Children.Remove((UIElement) this.PredefinedSymbol);
      if (!double.IsNaN(this.adornment.YData) && !double.IsNaN(this.adornment.XData))
        return;
      this.Children.Remove((UIElement) this.PredefinedSymbol);
    }
  }
}
