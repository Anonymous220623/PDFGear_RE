// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LegendItem
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LegendItem : DependencyObject, INotifyPropertyChanged
{
  public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (string), typeof (LegendItem), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(LegendItem.OnLabelChanged)));
  public static readonly DependencyProperty LegendIconTemplateProperty = DependencyProperty.Register(nameof (LegendIconTemplate), typeof (DataTemplate), typeof (LegendItem), new PropertyMetadata((object) null, new PropertyChangedCallback(LegendItem.OnLegendIconTemplateChanged)));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (LegendItem), new PropertyMetadata((object) null, new PropertyChangedCallback(LegendItem.OnStrokePropertyChanged)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (LegendItem), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(LegendItem.OnStrokeThicknessProperty)));
  public static readonly DependencyProperty InteriorProperty = DependencyProperty.Register(nameof (Interior), typeof (Brush), typeof (LegendItem), new PropertyMetadata((object) null, new PropertyChangedCallback(LegendItem.OnInteriorChanged)));
  public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register(nameof (IconVisibility), typeof (Visibility), typeof (LegendItem), new PropertyMetadata((object) Visibility.Collapsed, new PropertyChangedCallback(LegendItem.OnIconVisibilityChanged)));
  public static readonly DependencyProperty CheckBoxVisibilityProperty = DependencyProperty.Register(nameof (CheckBoxVisibility), typeof (Visibility), typeof (LegendItem), new PropertyMetadata((object) Visibility.Collapsed, new PropertyChangedCallback(LegendItem.OnCheckBoxVisibilityChanged)));
  public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof (IconWidth), typeof (double), typeof (LegendItem), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(LegendItem.OnIconWidthPropertyChanged)));
  public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof (IconHeight), typeof (double), typeof (LegendItem), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(LegendItem.OnIconHeightPropertyChanged)));
  public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(nameof (ItemMargin), typeof (Thickness), typeof (LegendItem), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0), new PropertyChangedCallback(LegendItem.OnItemMarginPropertyChanged)));
  public static readonly DependencyProperty IsSeriesVisibleProperty = DependencyProperty.Register(nameof (IsSeriesVisible), typeof (bool), typeof (LegendItem), new PropertyMetadata((object) true, new PropertyChangedCallback(LegendItem.OnSeriesVisible)));
  public static readonly DependencyProperty VisibilityOnLegendProperty = DependencyProperty.Register(nameof (VisibilityOnLegend), typeof (Visibility), typeof (LegendItem), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(LegendItem.OnVisibilityOnLegend)));
  public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(nameof (Opacity), typeof (double), typeof (LegendItem), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(LegendItem.OnOpacityChanged)));
  private ChartSegment segment;
  private object item;
  private ChartSeriesBase series;
  private TrendlineBase trendline;
  private ChartLegend legend;

  public event PropertyChangedEventHandler PropertyChanged;

  public string Label
  {
    get => (string) this.GetValue(LegendItem.LabelProperty);
    set => this.SetValue(LegendItem.LabelProperty, (object) value);
  }

  public DataTemplate LegendIconTemplate
  {
    get => (DataTemplate) this.GetValue(LegendItem.LegendIconTemplateProperty);
    set => this.SetValue(LegendItem.LegendIconTemplateProperty, (object) value);
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(LegendItem.StrokeProperty);
    set => this.SetValue(LegendItem.StrokeProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(LegendItem.StrokeThicknessProperty);
    set => this.SetValue(LegendItem.StrokeThicknessProperty, (object) value);
  }

  public Brush Interior
  {
    get => (Brush) this.GetValue(LegendItem.InteriorProperty);
    set => this.SetValue(LegendItem.InteriorProperty, (object) value);
  }

  public Visibility IconVisibility
  {
    get => (Visibility) this.GetValue(LegendItem.IconVisibilityProperty);
    set => this.SetValue(LegendItem.IconVisibilityProperty, (object) value);
  }

  public Visibility CheckBoxVisibility
  {
    get => (Visibility) this.GetValue(LegendItem.CheckBoxVisibilityProperty);
    set => this.SetValue(LegendItem.CheckBoxVisibilityProperty, (object) value);
  }

  public double IconWidth
  {
    get => (double) this.GetValue(LegendItem.IconWidthProperty);
    set => this.SetValue(LegendItem.IconWidthProperty, (object) value);
  }

  public double IconHeight
  {
    get => (double) this.GetValue(LegendItem.IconHeightProperty);
    set => this.SetValue(LegendItem.IconHeightProperty, (object) value);
  }

  public Thickness ItemMargin
  {
    get => (Thickness) this.GetValue(LegendItem.ItemMarginProperty);
    set => this.SetValue(LegendItem.ItemMarginProperty, (object) value);
  }

  public bool IsSeriesVisible
  {
    get => (bool) this.GetValue(LegendItem.IsSeriesVisibleProperty);
    set => this.SetValue(LegendItem.IsSeriesVisibleProperty, (object) value);
  }

  public Visibility VisibilityOnLegend
  {
    get => (Visibility) this.GetValue(LegendItem.VisibilityOnLegendProperty);
    set => this.SetValue(LegendItem.VisibilityOnLegendProperty, (object) value);
  }

  public double Opacity
  {
    get => (double) this.GetValue(LegendItem.OpacityProperty);
    set => this.SetValue(LegendItem.OpacityProperty, (object) value);
  }

  public ChartSegment Segment
  {
    get => this.segment;
    set
    {
      this.segment = value;
      if (this.segment == null)
        return;
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.segment,
        Path = new PropertyPath("Interior", new object[0]),
        Converter = (IValueConverter) new InteriorConverter(this.segment.Series),
        ConverterParameter = (object) this.segment.Series.Segments.IndexOf(this.segment)
      });
    }
  }

  public object Item
  {
    get => this.item;
    set => this.item = value;
  }

  public TrendlineBase Trendline
  {
    get => this.trendline;
    set
    {
      this.trendline = value;
      if (this.trendline == null)
        return;
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.trendline,
        Path = new PropertyPath("Stroke", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.LabelProperty, (BindingBase) new Binding()
      {
        Source = (object) this.trendline,
        Path = new PropertyPath("Label", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.LegendIconTemplateProperty, (BindingBase) new Binding()
      {
        Source = (object) this.trendline,
        Path = new PropertyPath("LegendIconTemplate", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.VisibilityOnLegendProperty, (BindingBase) new Binding()
      {
        Source = (object) this.trendline,
        Path = new PropertyPath("VisibilityOnLegend", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.IsSeriesVisibleProperty, (BindingBase) new Binding()
      {
        Source = (object) this.trendline,
        Mode = BindingMode.TwoWay,
        Path = new PropertyPath("IsTrendlineVisible", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.StrokeProperty, (BindingBase) new Binding()
      {
        Source = (object) this.trendline,
        Path = new PropertyPath("Stroke", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.StrokeThicknessProperty, (BindingBase) new Binding()
      {
        Source = (object) this.trendline,
        Path = new PropertyPath("StrokeThickness", new object[0])
      });
    }
  }

  public ChartSeriesBase Series
  {
    get => this.series;
    set
    {
      this.series = value;
      if (this.series == null)
        return;
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.series,
        Path = new PropertyPath("Interior", new object[0]),
        Converter = (IValueConverter) new InteriorConverter(this.series),
        ConverterParameter = (object) this.Index
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.LabelProperty, (BindingBase) new Binding()
      {
        Source = (object) this.series,
        Path = new PropertyPath("Label", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.LegendIconTemplateProperty, (BindingBase) new Binding()
      {
        Source = (object) this.series,
        Path = new PropertyPath("LegendIconTemplate", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.VisibilityOnLegendProperty, (BindingBase) new Binding()
      {
        Source = (object) this.series,
        Path = new PropertyPath("VisibilityOnLegend", new object[0])
      });
      if (!this.Series.IsSingleAccumulationSeries)
        BindingOperations.SetBinding((DependencyObject) this, LegendItem.IsSeriesVisibleProperty, (BindingBase) new Binding()
        {
          Source = (object) this.series,
          Mode = BindingMode.TwoWay,
          Path = new PropertyPath("IsSeriesVisible", new object[0])
        });
      if (!(this.series is ChartSeries))
        return;
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.StrokeProperty, (BindingBase) new Binding()
      {
        Source = (object) this.series,
        Path = new PropertyPath("Stroke", new object[0])
      });
      BindingOperations.SetBinding((DependencyObject) this, LegendItem.StrokeThicknessProperty, (BindingBase) new Binding()
      {
        Source = (object) this.series,
        Path = new PropertyPath("StrokeThickness", new object[0])
      });
    }
  }

  internal object XFormsLabelStyle { get; set; }

  internal object XFormsLegendItem { get; set; }

  internal int Index { get; set; }

  internal ChartLegend Legend
  {
    get => this.legend;
    set
    {
      this.legend = value;
      if (this.legend == null)
        return;
      if (this.Segment != null && this.Segment.Series is AccumulationSeriesBase)
      {
        BindingOperations.SetBinding((DependencyObject) this, LegendItem.IconVisibilityProperty, (BindingBase) new Binding()
        {
          Source = (object) this.legend,
          Path = new PropertyPath("IconVisibility", new object[0])
        });
        BindingOperations.SetBinding((DependencyObject) this, LegendItem.CheckBoxVisibilityProperty, (BindingBase) new Binding()
        {
          Source = (object) this.legend,
          Path = new PropertyPath("CheckBoxVisibility", new object[0])
        });
        this.CheckBoxVisibility = Visibility.Collapsed;
      }
      else
      {
        BindingOperations.SetBinding((DependencyObject) this, LegendItem.IconVisibilityProperty, (BindingBase) new Binding()
        {
          Source = (object) this.legend,
          Path = new PropertyPath("IconVisibility", new object[0])
        });
        BindingOperations.SetBinding((DependencyObject) this, LegendItem.CheckBoxVisibilityProperty, (BindingBase) new Binding()
        {
          Source = (object) this.legend,
          Path = new PropertyPath("CheckBoxVisibility", new object[0])
        });
        BindingOperations.SetBinding((DependencyObject) this, LegendItem.IconWidthProperty, (BindingBase) new Binding()
        {
          Source = (object) this.legend,
          Path = new PropertyPath("IconWidth", new object[0])
        });
        BindingOperations.SetBinding((DependencyObject) this, LegendItem.IconHeightProperty, (BindingBase) new Binding()
        {
          Source = (object) this.legend,
          Path = new PropertyPath("IconHeight", new object[0])
        });
        BindingOperations.SetBinding((DependencyObject) this, LegendItem.ItemMarginProperty, (BindingBase) new Binding()
        {
          Source = (object) this.legend,
          Path = new PropertyPath("ItemMargin", new object[0])
        });
      }
    }
  }

  internal void Dispose()
  {
    if (this.PropertyChanged != null)
    {
      foreach (Delegate invocation in this.PropertyChanged.GetInvocationList())
        this.PropertyChanged -= invocation as PropertyChangedEventHandler;
      this.PropertyChanged = (PropertyChangedEventHandler) null;
    }
    this.segment = (ChartSegment) null;
    this.item = (object) null;
    this.series = (ChartSeriesBase) null;
    this.trendline = (TrendlineBase) null;
    this.legend = (ChartLegend) null;
    this.XFormsLabelStyle = (object) null;
    this.XFormsLegendItem = (object) null;
  }

  internal void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  private static void OnStrokePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("Stroke");
  }

  private static void OnStrokeThicknessProperty(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("StrokeThickness");
  }

  private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("Label");
  }

  private static void OnLegendIconTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("LegendIconTemplate");
  }

  private static void OnInteriorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("Interior");
  }

  private static void OnIconVisibilityChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("IconVisibility");
  }

  private static void OnCheckBoxVisibilityChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("CheckBoxVisibility");
  }

  private static void OnIconWidthPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("IconWidth");
  }

  private static void OnIconHeightPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("IconHeight");
  }

  private static void OnItemMarginPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("ItemMargin");
  }

  private static void OnSeriesVisible(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    LegendItem legendItem = d as LegendItem;
    legendItem.OnPropertyChanged("IsSeriesVisible");
    if (legendItem.legend == null)
      return;
    List<ChartSeriesBase> actualSeries = legendItem.legend.ChartArea.ActualSeries;
    if (actualSeries[0].IsSingleAccumulationSeries && !legendItem.legend.isSegmentsUpdated)
      legendItem.legend.ComputeToggledSegment(actualSeries[0], legendItem);
    else if (legendItem.IsSeriesVisible)
      legendItem.Opacity = 1.0;
    else
      legendItem.Opacity = 0.5;
  }

  private static void OnVisibilityOnLegend(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("VisibilityOnLegend");
  }

  private static void OnOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as LegendItem).OnPropertyChanged("Opacity");
  }
}
