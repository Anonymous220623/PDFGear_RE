// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartBase : Control, ICloneable, INotifyPropertyChanged
{
  public static readonly DependencyProperty AxisThicknessProperty = DependencyProperty.Register(nameof (AxisThickness), typeof (Thickness), typeof (ChartBase), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)));
  public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached("Row", typeof (int), typeof (ChartBase), new PropertyMetadata((object) 0, new PropertyChangedCallback(ChartBase.OnRowColumnChanged)));
  public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.RegisterAttached("ColumnSpan", typeof (int), typeof (ChartBase), new PropertyMetadata((object) 1, new PropertyChangedCallback(ChartBase.OnRowColumnChanged)));
  public static readonly DependencyProperty RowSpanProperty = DependencyProperty.RegisterAttached("RowSpan", typeof (int), typeof (ChartBase), new PropertyMetadata((object) 1, new PropertyChangedCallback(ChartBase.OnRowColumnChanged)));
  public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached("Column", typeof (int), typeof (ChartBase), new PropertyMetadata((object) 0, new PropertyChangedCallback(ChartBase.OnRowColumnChanged)));
  public static readonly DependencyProperty VisibleSeriesProperty = DependencyProperty.Register(nameof (VisibleSeries), typeof (ChartVisibleSeriesCollection), typeof (ChartBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(nameof (Palette), typeof (ChartColorPalette), typeof (ChartBase), new PropertyMetadata((object) ChartColorPalette.Metro, new PropertyChangedCallback(ChartBase.OnPaletteChanged)));
  public static readonly DependencyProperty SeriesSelectedIndexProperty = DependencyProperty.Register(nameof (SeriesSelectedIndex), typeof (int), typeof (ChartBase), new PropertyMetadata((object) -1, new PropertyChangedCallback(ChartBase.OnSeriesSelectedIndexChanged)));
  public static readonly DependencyProperty SideBySideSeriesPlacementProperty = DependencyProperty.Register(nameof (SideBySideSeriesPlacement), typeof (bool), typeof (ChartBase), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartBase.OnSideBySideSeriesPlacementProperty)));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ChartBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty HorizontalHeaderAlignmentProperty = DependencyProperty.Register(nameof (HorizontalHeaderAlignment), typeof (HorizontalAlignment), typeof (ChartBase), new PropertyMetadata((object) HorizontalAlignment.Center));
  public static readonly DependencyProperty VerticalHeaderAlignmentProperty = DependencyProperty.Register(nameof (VerticalHeaderAlignment), typeof (VerticalAlignment), typeof (ChartBase), new PropertyMetadata((object) VerticalAlignment.Center));
  public static readonly DependencyProperty ColorModelProperty = DependencyProperty.Register(nameof (ColorModel), typeof (ChartColorModel), typeof (ChartBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartBase.OnColorModelChanged)));
  internal static readonly DependencyProperty TooltipProperty = DependencyProperty.Register(nameof (Tooltip), typeof (ChartTooltip), typeof (ChartBase), new PropertyMetadata((PropertyChangedCallback) null));
  internal bool isUpdateDispatched;
  internal double InternalDoughnutHoleSize = 0.5;
  internal Canvas AdorningCanvas;
  internal Canvas ToolkitCanvas;
  internal bool ShowTooltip;
  internal bool IsTemplateApplied;
  internal bool IsUpdateLegend;
  internal List<ChartSeriesBase> ActualSeries = new List<ChartSeriesBase>();
  internal List<ChartSeriesBase> SelectedSeriesCollection = new List<ChartSeriesBase>();
  internal ChartSeriesBase CurrentSelectedSeries;
  internal ChartSeriesBase PreviousSelectedSeries;
  protected Printing Printing;
  private ChartRowDefinitions rowDefinitions;
  private ChartColumnDefinitions columnDefinitions;
  private ILayoutCalculator gridLinesLayout;
  private Rect seriesClipRect;
  private ILayoutCalculator chartAxisLayoutPanel;
  private double m_minPointsDelta = double.NaN;
  private bool isSbsWithOneData;
  private Size? rootPanelDesiredSize;
  private ChartAreaType areaType = ChartAreaType.CartesianAxes;
  private ChartSelectionBehavior selectionBehavior;
  private ChartTooltipBehavior tooltipBehavior;
  private Dictionary<object, int> seriesPosition = new Dictionary<object, int>();
  private bool isLoading = true;
  public static readonly DependencyProperty ChartResourceDictionaryProperty = DependencyProperty.Register(nameof (ChartResourceDictionary), typeof (ResourceDictionary), typeof (ChartBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LegendProperty = DependencyProperty.Register(nameof (Legend), typeof (object), typeof (ChartBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartBase.OnLegendPropertyChanged)));
  internal IList<UIElement> LegendCollection;

  public event EventHandler<ChartSelectionChangedEventArgs> SelectionChanged;

  public event EventHandler<ChartSelectionChangingEventArgs> SelectionChanging;

  public event EventHandler<ChartSeriesBoundsEventArgs> SeriesBoundsChanged;

  public event PropertyChangedEventHandler PropertyChanged;

  public event OnPrintingEventHandler OnPrinting;

  public Thickness AxisThickness
  {
    get => (Thickness) this.GetValue(ChartBase.AxisThicknessProperty);
    internal set => this.SetValue(ChartBase.AxisThicknessProperty, (object) value);
  }

  public Rect SeriesClipRect
  {
    get => this.seriesClipRect;
    internal set
    {
      if (this.seriesClipRect == value)
        return;
      Rect seriesClipRect = this.seriesClipRect;
      this.seriesClipRect = value;
      this.OnSeriesBoundsChanged(new ChartSeriesBoundsEventArgs()
      {
        OldBounds = seriesClipRect,
        NewBounds = value
      });
      this.OnPropertyChanged(nameof (SeriesClipRect));
    }
  }

  public ChartVisibleSeriesCollection VisibleSeries
  {
    get => (ChartVisibleSeriesCollection) this.GetValue(ChartBase.VisibleSeriesProperty);
    internal set => this.SetValue(ChartBase.VisibleSeriesProperty, (object) value);
  }

  public ChartColorPalette Palette
  {
    get => (ChartColorPalette) this.GetValue(ChartBase.PaletteProperty);
    set => this.SetValue(ChartBase.PaletteProperty, (object) value);
  }

  public int SeriesSelectedIndex
  {
    get => (int) this.GetValue(ChartBase.SeriesSelectedIndexProperty);
    set => this.SetValue(ChartBase.SeriesSelectedIndexProperty, (object) value);
  }

  public ChartColumnDefinitions ColumnDefinitions
  {
    get
    {
      if (this.columnDefinitions != null)
        return this.columnDefinitions;
      this.columnDefinitions = new ChartColumnDefinitions();
      this.columnDefinitions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnRowColChanged);
      return this.columnDefinitions;
    }
    set
    {
      if (this.columnDefinitions != null)
        this.columnDefinitions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnRowColChanged);
      this.columnDefinitions = value;
      if (this.columnDefinitions != null)
        this.columnDefinitions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnRowColChanged);
      this.ScheduleUpdate();
    }
  }

  public ChartRowDefinitions RowDefinitions
  {
    get
    {
      if (this.rowDefinitions != null)
        return this.rowDefinitions;
      this.rowDefinitions = new ChartRowDefinitions();
      this.rowDefinitions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnRowColChanged);
      return this.rowDefinitions;
    }
    set
    {
      if (this.rowDefinitions != null)
        this.rowDefinitions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnRowColChanged);
      this.rowDefinitions = value;
      if (this.rowDefinitions != null)
        this.rowDefinitions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnRowColChanged);
      this.ScheduleUpdate();
    }
  }

  public ChartAxisCollection Axes { get; internal set; }

  public bool SideBySideSeriesPlacement
  {
    get => (bool) this.GetValue(ChartBase.SideBySideSeriesPlacementProperty);
    set => this.SetValue(ChartBase.SideBySideSeriesPlacementProperty, (object) value);
  }

  public object Header
  {
    get => this.GetValue(ChartBase.HeaderProperty);
    set => this.SetValue(ChartBase.HeaderProperty, value);
  }

  public HorizontalAlignment HorizontalHeaderAlignment
  {
    get => (HorizontalAlignment) this.GetValue(ChartBase.HorizontalHeaderAlignmentProperty);
    set => this.SetValue(ChartBase.HorizontalHeaderAlignmentProperty, (object) value);
  }

  public VerticalAlignment VerticalHeaderAlignment
  {
    get => (VerticalAlignment) this.GetValue(ChartBase.VerticalHeaderAlignmentProperty);
    set => this.SetValue(ChartBase.VerticalHeaderAlignmentProperty, (object) value);
  }

  public ChartColorModel ColorModel
  {
    get => (ChartColorModel) this.GetValue(ChartBase.ColorModelProperty);
    set => this.SetValue(ChartBase.ColorModelProperty, (object) value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public ResourceDictionary ChartResourceDictionary
  {
    get => (ResourceDictionary) this.GetValue(ChartBase.ChartResourceDictionaryProperty);
    set => this.SetValue(ChartBase.ChartResourceDictionaryProperty, (object) value);
  }

  internal ChartAxis InternalPrimaryAxis { get; set; }

  internal ChartAxis InternalDepthAxis { get; set; }

  internal ChartAxis InternalSecondaryAxis { get; set; }

  internal bool IsChartLoaded { get; set; }

  internal ChartDockPanel ChartDockPanel { get; set; }

  internal List<ChartAxis> DependentSeriesAxes { get; set; }

  internal ChartSelectionBehavior SelectionBehaviour
  {
    get
    {
      if (this.selectionBehavior == null)
        this.SetSelectionBehaviour();
      return this.selectionBehavior;
    }
    set => this.selectionBehavior = value;
  }

  internal ChartTooltipBehavior TooltipBehavior
  {
    get
    {
      if (this.tooltipBehavior == null)
        this.SetTooltipBehavior();
      return this.tooltipBehavior;
    }
    set => this.tooltipBehavior = value;
  }

  internal bool SBSInfoCalculated { get; set; }

  internal Size? RootPanelDesiredSize
  {
    get => this.rootPanelDesiredSize;
    set
    {
      Size? panelDesiredSize = this.rootPanelDesiredSize;
      Size? nullable = value;
      if ((panelDesiredSize.HasValue != nullable.HasValue ? 0 : (!panelDesiredSize.HasValue ? 1 : (panelDesiredSize.GetValueOrDefault() == nullable.GetValueOrDefault() ? 1 : 0))) != 0)
        return;
      this.rootPanelDesiredSize = value;
      this.OnRootPanelSizeChanged(value.HasValue ? value.Value : new Size());
    }
  }

  internal Size AvailableSize { get; set; }

  internal UpdateAction UpdateAction { get; set; }

  internal Dictionary<object, int> SeriesPosition
  {
    get => this.seriesPosition;
    set => this.seriesPosition = value;
  }

  internal Dictionary<object, StackingValues> StackedValues { get; set; }

  internal int[,] SbsSeriesCount { get; set; }

  internal double MinPointsDelta
  {
    get
    {
      this.m_minPointsDelta = double.MaxValue;
      foreach (ChartSeriesBase series in (Collection<ChartSeriesBase>) this.VisibleSeries)
      {
        List<double> actualXvalues = series.ActualXValues as List<double>;
        if (!series.IsIndexed && actualXvalues != null && series.IsSideBySide)
          this.GetMinPointsDelta(actualXvalues, ref this.m_minPointsDelta, series, series.IsIndexed);
      }
      if (this.VisibleSeries.Count > 1 && this.isSbsWithOneData)
      {
        List<double> values = new List<double>();
        foreach (ChartSeriesBase series in (Collection<ChartSeriesBase>) this.VisibleSeries)
        {
          List<double> actualXvalues = series.ActualXValues as List<double>;
          if (!series.IsIndexed && actualXvalues != null && actualXvalues.Count > 0)
          {
            if (!series.IsSideBySide)
            {
              this.GetMinPointsDelta(actualXvalues, ref this.m_minPointsDelta, series, series.IsIndexed);
            }
            else
            {
              List<double> list = actualXvalues.ToList<double>();
              if (list != null)
              {
                values.AddRange((IEnumerable<double>) list);
                values.Sort();
                this.GetMinPointsDelta(values, ref this.m_minPointsDelta, series, series.IsIndexed);
                values = list;
              }
            }
          }
        }
        this.isSbsWithOneData = false;
      }
      else if (this.isSbsWithOneData)
      {
        foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        {
          if (chartSeriesBase.ActualXAxis is DateTimeAxis actualXaxis)
            this.m_minPointsDelta = actualXaxis.ActualRange.End - actualXaxis.ActualRange.Start;
        }
      }
      this.m_minPointsDelta = this.m_minPointsDelta == double.MaxValue || this.m_minPointsDelta < 0.0 ? 1.0 : this.m_minPointsDelta;
      return this.m_minPointsDelta;
    }
  }

  internal ILayoutCalculator GridLinesLayout
  {
    get => this.gridLinesLayout;
    set => this.gridLinesLayout = value;
  }

  internal ChartAreaType AreaType
  {
    get => this.areaType;
    set
    {
      if (this.areaType == value)
        return;
      this.areaType = value;
      this.OnAreaTypeChanged();
    }
  }

  internal ILayoutCalculator ChartAxisLayoutPanel
  {
    get => this.chartAxisLayoutPanel;
    set => this.chartAxisLayoutPanel = value;
  }

  internal ChartTooltip Tooltip
  {
    get => (ChartTooltip) this.GetValue(ChartBase.TooltipProperty);
    set => this.SetValue(ChartBase.TooltipProperty, (object) value);
  }

  internal bool IsLoading
  {
    get => this.isLoading;
    set
    {
      if (this.isLoading == value)
        return;
      this.isLoading = value;
      this.OnPropertyChanged(nameof (IsLoading));
    }
  }

  protected internal virtual List<ChartSegment> SelectedSegments
  {
    get
    {
      return this.VisibleSeries.Count > 0 ? this.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.SelectedSegments != null)).SelectMany<ChartSeriesBase, ChartSegment>((Func<ChartSeriesBase, IEnumerable<ChartSegment>>) (series => (IEnumerable<ChartSegment>) series.SelectedSegments)).ToList<ChartSegment>() : (List<ChartSegment>) null;
    }
  }

  public static int GetRow(UIElement obj) => (int) obj.GetValue(ChartBase.RowProperty);

  public static void SetRow(UIElement obj, int value)
  {
    obj.SetValue(ChartBase.RowProperty, (object) value);
  }

  public static int GetColumn(UIElement obj) => (int) obj.GetValue(ChartBase.ColumnProperty);

  public static int GetColumnSpan(UIElement element)
  {
    return (int) element.GetValue(ChartBase.ColumnSpanProperty);
  }

  public static int GetRowSpan(UIElement element)
  {
    return (int) element.GetValue(ChartBase.RowSpanProperty);
  }

  public static void SetColumn(UIElement obj, int value)
  {
    obj.SetValue(ChartBase.ColumnProperty, (object) value);
  }

  public static void SetColumnSpan(UIElement element, int value)
  {
    element.SetValue(ChartBase.ColumnSpanProperty, (object) value);
  }

  public static void SetRowSpan(UIElement element, int value)
  {
    element.SetValue(ChartBase.RowSpanProperty, (object) value);
  }

  public void SuspendSeriesNotification()
  {
    if (this.ActualSeries == null)
      return;
    foreach (ChartSeriesBase chartSeriesBase in this.ActualSeries)
      chartSeriesBase.SuspendNotification();
  }

  public void ResumeSeriesNotification()
  {
    if (this.ActualSeries == null)
      return;
    foreach (ChartSeriesBase chartSeriesBase in this.ActualSeries)
      chartSeriesBase.ResumeNotification();
  }

  public DependencyObject Clone() => this.CloneChart();

  public List<double> GetCumulativeStackInfo(ChartSeriesBase series, bool reqNegStack)
  {
    if (series == null)
      return (List<double>) null;
    double origin = series.ActualYAxis.Origin;
    List<double> cumulativeStackInfo = new List<double>();
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
    {
      IList<double> yvalues = ((XyDataSeries) chartSeriesBase).YValues;
      if (chartSeriesBase.ActualXValues != null)
      {
        if (cumulativeStackInfo.Count > 0)
        {
          for (int index1 = 0; index1 < chartSeriesBase.DataCount; ++index1)
          {
            double num = reqNegStack ? Math.Abs(yvalues[index1]) : yvalues[index1];
            if (index1 < cumulativeStackInfo.Count)
            {
              List<double> doubleList;
              int index2;
              (doubleList = cumulativeStackInfo)[index2 = index1] = doubleList[index2] + (num + origin);
            }
            else
              cumulativeStackInfo.Add(num + origin);
          }
        }
        else
        {
          for (int index = 0; index < chartSeriesBase.DataCount; ++index)
          {
            double num = reqNegStack ? Math.Abs(yvalues[index]) : yvalues[index];
            cumulativeStackInfo.Add(num + origin);
          }
        }
        if (series == chartSeriesBase)
          break;
      }
    }
    return cumulativeStackInfo;
  }

  public void Save(string fileName)
  {
    if (this is SfChart sfChart)
    {
      if (sfChart.isRenderSeriesDispatched)
      {
        sfChart.RenderSeries();
        if (this.LegendCollection != null)
        {
          foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
            legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }
        this.UpdateLayout();
      }
    }
    else
    {
      SfChart3D sfChart3D = this as SfChart3D;
      if (sfChart3D.IsRenderSeriesDispatched)
      {
        sfChart3D.RenderSeries();
        if (this.LegendCollection != null)
        {
          foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
            legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }
        this.UpdateLayout();
      }
    }
    FrameworkElement frameworkElement = (FrameworkElement) this;
    BitmapEncoder bitmapEncoder;
    switch (new FileInfo(fileName).Extension.ToLower(CultureInfo.InvariantCulture))
    {
      case ".bmp":
        bitmapEncoder = (BitmapEncoder) new BmpBitmapEncoder();
        break;
      case ".jpg":
      case ".jpeg":
        bitmapEncoder = (BitmapEncoder) new JpegBitmapEncoder();
        break;
      case ".png":
        bitmapEncoder = (BitmapEncoder) new PngBitmapEncoder();
        break;
      case ".gif":
        bitmapEncoder = (BitmapEncoder) new GifBitmapEncoder();
        break;
      case ".tif":
      case ".tiff":
        bitmapEncoder = (BitmapEncoder) new TiffBitmapEncoder();
        break;
      case ".wdp":
        bitmapEncoder = (BitmapEncoder) new WmpBitmapEncoder();
        break;
      default:
        bitmapEncoder = (BitmapEncoder) new BmpBitmapEncoder();
        break;
    }
    if (frameworkElement == null)
      return;
    DrawingVisual drawingVisual = new DrawingVisual();
    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
    {
      VisualBrush visualBrush1 = new VisualBrush((Visual) frameworkElement);
      visualBrush1.Stretch = Stretch.Fill;
      VisualBrush visualBrush2 = visualBrush1;
      drawingContext.DrawRectangle((Brush) visualBrush2, (Pen) null, new Rect(0.0, 0.0, frameworkElement.ActualWidth, frameworkElement.ActualHeight));
      drawingContext.Close();
    }
    RenderTargetBitmap source = new RenderTargetBitmap((int) frameworkElement.ActualWidth, (int) frameworkElement.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
    source.Render((Visual) drawingVisual);
    bitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
    using (Stream stream = (Stream) File.Create(fileName))
      bitmapEncoder.Save(stream);
  }

  public void Save(Stream stream, BitmapEncoder imgEncoder)
  {
    if (this is SfChart sfChart)
    {
      if (sfChart.isRenderSeriesDispatched)
      {
        sfChart.RenderSeries();
        if (this.LegendCollection != null)
        {
          foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
            legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }
        this.UpdateLayout();
      }
    }
    else
    {
      SfChart3D sfChart3D = this as SfChart3D;
      if ((this as SfChart3D).IsRenderSeriesDispatched)
      {
        sfChart3D.RenderSeries();
        if (this.LegendCollection != null)
        {
          foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
            legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }
        this.UpdateLayout();
      }
    }
    FrameworkElement frameworkElement = (FrameworkElement) this;
    if (frameworkElement == null || stream == null || imgEncoder == null)
      return;
    RenderTargetBitmap source = new RenderTargetBitmap((int) frameworkElement.ActualWidth, (int) frameworkElement.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
    source.Render((Visual) frameworkElement);
    imgEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
    imgEncoder.Save(stream);
  }

  public void Print()
  {
    if (this is SfChart sfChart)
    {
      if (sfChart.isRenderSeriesDispatched)
      {
        sfChart.RenderSeries();
        if (this.LegendCollection != null)
        {
          foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
            legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }
        this.UpdateLayout();
      }
    }
    else
    {
      SfChart3D sfChart3D = this as SfChart3D;
      if (sfChart3D.IsRenderSeriesDispatched)
      {
        sfChart3D.RenderSeries();
        if (this.LegendCollection != null)
        {
          foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
            legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        }
        this.UpdateLayout();
      }
    }
    ChartPrintDialog childobject = new ChartPrintDialog();
    childobject.BorderStyle = this.ChartResourceDictionary[(object) "printDialogBorderStyle"] as Style;
    childobject.DashedBorderStyle = this.ChartResourceDictionary[(object) "printDialogDashedLineStyle"] as Style;
    PrintingEventArgs args = new PrintingEventArgs()
    {
      PrintDialog = (Window) childobject,
      PrintVisual = (Visual) childobject.GetPrintVisual((FrameworkElement) this)
    };
    this.RaiseOnPrinting(args);
    if (args.ShowPrintDialog)
    {
      if (ChartSkinManagerExtension.GetBaseThemeName((DependencyObject) this) != null)
        ChartSkinManagerExtension.SetTheme((DependencyObject) this, (DependencyObject) childobject);
      childobject.ShowPrintDialog((FrameworkElement) this, Rect.Empty, this.ActualHeight, this.ActualWidth);
      Keyboard.Focus((IInputElement) this);
    }
    else
    {
      if (args.CancelPrinting)
        return;
      new PrintDialog().PrintVisual((Visual) this, "Syncfusion SfChart WPF " + ChartLocalizationResourceAccessor.Instance.GetString("PrintingMessage"));
    }
  }

  public void Print(
    HorizontalAlignment HorizontalAlignment,
    VerticalAlignment VerticalAlignment,
    Thickness PageMargin,
    bool PrintLandscape,
    bool ShrinkToFit)
  {
    if (this is SfChart sfChart)
    {
      if (sfChart.isRenderSeriesDispatched)
      {
        sfChart.RenderSeries();
        foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
          legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        this.UpdateLayout();
      }
    }
    else
    {
      SfChart3D sfChart3D = this as SfChart3D;
      if (sfChart3D.IsRenderSeriesDispatched)
      {
        sfChart3D.RenderSeries();
        foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
          legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        this.UpdateLayout();
      }
    }
    PrintDialog printDialog = new PrintDialog();
    bool? nullable = printDialog.ShowDialog();
    if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
      return;
    printDialog.PrintVisual(this.Printing.Layout((FrameworkElement) this, new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight), "Syncfusion SfChart WPF " + ChartLocalizationResourceAccessor.Instance.GetString("PrintingMessage"), HorizontalAlignment, VerticalAlignment, PageMargin, PrintLandscape, ShrinkToFit), "Syncfusion Sfchart WPF " + ChartLocalizationResourceAccessor.Instance.GetString("PrintingMessage"));
  }

  public void Serialize(string fileName)
  {
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    SfChart sfChart = this as SfChart;
    string str1 = (string) null;
    if (sfChart != null && sfChart.Annotations != null && sfChart.Annotations.Count != 0)
    {
      foreach (Annotation annotation in (Collection<Annotation>) sfChart.Annotations)
        str1 += annotation.Serialize();
      result = result.Replace("</SfChart>", $"<SfChart.Annotations>{str1}</SfChart.Annotations></SfChart>");
    }
    string str2 = (string) null;
    if (sfChart != null && (this.VisibleSeries != null || this.VisibleSeries.Count != 0))
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        str2 += chartSeriesBase.Serialize();
      foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) sfChart.TechnicalIndicators)
        str2 += technicalIndicator.Serialize();
      result = result.Replace("</SfChart>", str2 + "</SfChart>");
    }
    File.WriteAllText(fileName, result.ToString());
  }

  public void Serialize(Stream stream)
  {
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    SfChart sfChart = this as SfChart;
    string str1 = (string) null;
    if (sfChart != null && sfChart.Annotations != null && sfChart.Annotations.Count != 0)
    {
      foreach (Annotation annotation in (Collection<Annotation>) sfChart.Annotations)
        str1 += annotation.Serialize();
      result = result.Replace("</SfChart>", $"<SfChart.Annotations>{str1}</SfChart.Annotations></SfChart>");
    }
    string str2 = (string) null;
    if (sfChart != null && (this.VisibleSeries != null || this.VisibleSeries.Count != 0))
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        str2 += chartSeriesBase.Serialize();
      foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) sfChart.TechnicalIndicators)
        str2 += technicalIndicator.Serialize();
      result = result.Replace("</SfChart>", str2 + "</SfChart>");
    }
    using (StreamWriter streamWriter = new StreamWriter(stream))
      streamWriter.WriteLine(result.ToString());
  }

  public void Serialize()
  {
    StringBuilder result = new StringBuilder();
    ChartBase.GetSerializedString(out result, (object) this);
    SfChart sfChart = this as SfChart;
    string str1 = (string) null;
    if (sfChart != null && sfChart.Annotations != null && sfChart.Annotations.Count != 0)
    {
      foreach (Annotation annotation in (Collection<Annotation>) sfChart.Annotations)
        str1 += annotation.Serialize();
      result = result.Replace("</SfChart>", $"<SfChart.Annotations>{str1}</SfChart.Annotations></SfChart>");
    }
    string str2 = (string) null;
    if (sfChart != null && (this.VisibleSeries != null || this.VisibleSeries.Count != 0))
    {
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
        str2 += chartSeriesBase.Serialize();
      foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) sfChart.TechnicalIndicators)
        str2 += technicalIndicator.Serialize();
      result = result.Replace("</SfChart>", str2 + "</SfChart>");
    }
    File.WriteAllText(Directory.GetParent("../").FullName + "\\chart.xml", result.ToString());
  }

  public object Deserialize(Stream stream) => XamlReader.Load(new StreamReader(stream).BaseStream);

  public object Deserialize()
  {
    return XamlReader.Load(new StreamReader(Directory.GetParent("../").FullName + "\\chart.xml").BaseStream);
  }

  public object Deserialize(string fileName)
  {
    return XamlReader.Load(new StreamReader(fileName).BaseStream);
  }

  public virtual void SeriesSelectedIndexChanged(int newIndex, int oldIndex)
  {
  }

  public virtual double ValueToPoint(ChartAxis axis, double value)
  {
    if (axis == null)
      return double.NaN;
    if (!axis.Orientation.Equals((object) Orientation.Horizontal))
      return axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top + (1.0 - axis.ValueToCoefficientCalc(value)) * axis.RenderedRect.Height;
    return axis.ActualWidth == 0.0 ? axis.ValueToCoefficientCalc(value) * axis.Area.SeriesClipRect.Width : axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left + axis.ValueToCoefficientCalc(value) * axis.RenderedRect.Width;
  }

  public virtual double PointToValue(ChartAxis axis, Point point)
  {
    if (axis == null)
      return double.NaN;
    return axis.Orientation.Equals((object) Orientation.Horizontal) ? axis.CoefficientToValueCalc((point.X - (axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left)) / axis.RenderedRect.Width) : axis.CoefficientToValueCalc(1.0 - (point.Y - (axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top)) / axis.RenderedRect.Height);
  }

  internal static void GetSerializedString(out StringBuilder result, object source)
  {
    SerializationBindingHelper.Register<MultiBindingExpression, BindingConvertor>();
    SerializationBindingHelper.Register<BindingExpression, BindingConvertor>();
    result = new StringBuilder();
    XamlWriter.Save(source, new XamlDesignerSerializationManager(XmlWriter.Create(result, new XmlWriterSettings()
    {
      Indent = true,
      OmitXmlDeclaration = true
    }))
    {
      XamlWriterMode = XamlWriterMode.Expression
    });
  }

  internal virtual void OnRowColumnCollectionChanged(NotifyCollectionChangedEventArgs e)
  {
  }

  internal virtual DependencyObject CloneChart() => (DependencyObject) null;

  internal virtual void UpdateArea(bool forceUpdate)
  {
  }

  internal virtual double ValueToLogPoint(ChartAxis axis, double value) => double.NaN;

  internal virtual void UpdateAxisLayoutPanels()
  {
  }

  internal void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  internal void DisposeRowColumnsDefinitions()
  {
    if (this.rowDefinitions != null)
    {
      foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.rowDefinitions)
        rowDefinition.Dispose();
      this.rowDefinitions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnRowColChanged);
      this.rowDefinitions.Clear();
    }
    if (this.columnDefinitions == null)
      return;
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.columnDefinitions)
      columnDefinition.Dispose();
    this.columnDefinitions.Clear();
    this.columnDefinitions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnRowColChanged);
  }

  internal void DisposeSelectionEvents()
  {
    if (this.SelectionChanged != null)
    {
      foreach (Delegate invocation in this.SelectionChanged.GetInvocationList())
        this.SelectionChanged -= invocation as EventHandler<ChartSelectionChangedEventArgs>;
      this.SelectionChanged = (EventHandler<ChartSelectionChangedEventArgs>) null;
    }
    if (this.SelectionChanging != null)
    {
      foreach (Delegate invocation in this.SelectionChanging.GetInvocationList())
        this.SelectionChanging -= invocation as EventHandler<ChartSelectionChangingEventArgs>;
      this.SelectionChanging = (EventHandler<ChartSelectionChangingEventArgs>) null;
    }
    if (this.SeriesBoundsChanged == null)
      return;
    foreach (Delegate invocation in this.SeriesBoundsChanged.GetInvocationList())
      this.SeriesBoundsChanged -= invocation as EventHandler<ChartSeriesBoundsEventArgs>;
    this.SeriesBoundsChanged = (EventHandler<ChartSeriesBoundsEventArgs>) null;
  }

  internal int GetActualRow(UIElement obj)
  {
    int count = this.RowDefinitions.Count;
    int row = ChartBase.GetRow(obj);
    int num = row >= count ? count - 1 : (row < 0 ? 0 : row);
    return num >= 0 ? num : 0;
  }

  internal int GetActualColumn(UIElement obj)
  {
    int count = this.ColumnDefinitions.Count;
    int column = ChartBase.GetColumn(obj);
    int num = column >= count ? count - 1 : (column < 0 ? 0 : column);
    return num >= 0 ? num : 0;
  }

  internal int GetActualColumnSpan(UIElement element)
  {
    int count = this.ColumnDefinitions.Count;
    int columnSpan = ChartBase.GetColumnSpan(element);
    if (columnSpan > count)
      return count;
    return columnSpan >= 0 ? columnSpan : 0;
  }

  internal int GetActualRowSpan(UIElement obj)
  {
    int count = this.RowDefinitions.Count;
    int rowSpan = ChartBase.GetRowSpan(obj);
    if (rowSpan > count)
      return count;
    return rowSpan >= 0 ? rowSpan : 0;
  }

  internal void GetMinPointsDelta(
    List<double> values,
    ref double minPointsDelta,
    ChartSeriesBase series,
    bool isIndexed)
  {
    if (!series.isLinearData)
    {
      values = values.ToList<double>();
      values.Sort();
    }
    if (values.Count == 1)
      this.isSbsWithOneData = true;
    for (int index = 1; index < values.Count; ++index)
    {
      double num = values[index] - values[index - 1];
      if (num != 0.0 && !double.IsNaN(num))
        minPointsDelta = Math.Min(minPointsDelta, num);
    }
  }

  internal Canvas GetAdorningCanvas() => this.AdorningCanvas;

  internal Brush GetSeriesSelectionBrush(ChartSeriesBase series)
  {
    if (this is SfChart && this.SelectionBehaviour != null)
      return this.SelectionBehaviour.GetSeriesSelectionBrush(series);
    return series is ChartSeries3D ? (series as ChartSeries3D).GetSeriesSelectionBrush(series) : (Brush) null;
  }

  internal bool GetEnableSeriesSelection()
  {
    switch (this)
    {
      case SfChart _ when this.SelectionBehaviour != null:
        return this.SelectionBehaviour.EnableSeriesSelection;
      case SfChart3D _:
        return (this as SfChart3D).EnableSeriesSelection;
      default:
        return false;
    }
  }

  internal bool GetEnableSegmentSelection()
  {
    switch (this)
    {
      case SfChart _ when this.SelectionBehaviour != null:
        return this.SelectionBehaviour.EnableSegmentSelection;
      case SfChart3D _:
        return (this as SfChart3D).EnableSegmentSelection;
      default:
        return false;
    }
  }

  internal int GetSeriesIndex(ChartSeriesBase series) => this.ActualSeries.IndexOf(series);

  internal void SetSelectionBehaviour()
  {
    if (!(this is SfChart))
      return;
    List<ChartBehavior> list = (this as SfChart).Behaviors.Where<ChartBehavior>((Func<ChartBehavior, bool>) (behavior => behavior is ChartSelectionBehavior)).ToList<ChartBehavior>();
    if (list != null && list.Count > 0)
      this.SelectionBehaviour = list[0] as ChartSelectionBehavior;
    else
      this.SelectionBehaviour = (ChartSelectionBehavior) null;
  }

  internal void SetTooltipBehavior()
  {
    if (!(this is SfChart))
      return;
    List<ChartBehavior> list = (this as SfChart).Behaviors.Where<ChartBehavior>((Func<ChartBehavior, bool>) (behavior => behavior is ChartTooltipBehavior)).ToList<ChartBehavior>();
    if (list != null && list.Count > 0)
      this.TooltipBehavior = list[0] as ChartTooltipBehavior;
    else
      this.TooltipBehavior = (ChartTooltipBehavior) null;
  }

  internal void ScheduleUpdate()
  {
    if (this.isUpdateDispatched)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.UpdateArea));
    this.isUpdateDispatched = true;
  }

  internal void UpdateArea() => this.UpdateArea(false);

  internal void RaiseSeriesSelectionChangedEvent()
  {
    ChartSeriesBase chartSeriesBase = (ChartSeriesBase) null;
    if (this is SfChart3D && this.SeriesSelectedIndex < (this as SfChart3D).Series.Count)
      chartSeriesBase = (ChartSeriesBase) (this as SfChart3D).Series[this.SeriesSelectedIndex];
    else if (this.SeriesSelectedIndex < (this as SfChart).Series.Count)
      chartSeriesBase = (ChartSeriesBase) (this as SfChart).Series[this.SeriesSelectedIndex];
    this.OnSelectionChanged(new ChartSelectionChangedEventArgs()
    {
      SelectedSegment = (ChartSegment) null,
      SelectedSeries = chartSeriesBase,
      SelectedSeriesCollection = this.SelectedSeriesCollection,
      SelectedIndex = this.SeriesSelectedIndex,
      PreviousSelectedIndex = -1,
      IsDataPointSelection = false
    });
  }

  protected internal virtual void OnSeriesBoundsChanged(ChartSeriesBoundsEventArgs args)
  {
    if (this.SeriesBoundsChanged == null || args == null)
      return;
    this.SeriesBoundsChanged((object) this, args);
  }

  protected internal virtual void OnSelectionChanged(ChartSelectionChangedEventArgs eventArgs)
  {
    switch (this)
    {
      case SfChart _ when this.SelectionBehaviour != null:
        if (this.SelectionChanged != null && eventArgs != null)
          this.SelectionChanged((object) this, eventArgs);
        this.SelectionBehaviour.OnSelectionChanged(eventArgs);
        break;
      case SfChart3D _ when this.SelectionChanged != null && eventArgs != null:
        this.SelectionChanged((object) this, eventArgs);
        break;
    }
  }

  protected internal virtual void OnBoxSelectionChanged(ChartSelectionChangedEventArgs eventArgs)
  {
    if (this.SelectionChanged != null && eventArgs != null)
      this.SelectionChanged((object) this, eventArgs);
    if (this.SelectionBehaviour == null)
      return;
    this.SelectionBehaviour.OnSelectionChanged(eventArgs);
  }

  protected internal virtual void OnSelectionChanging(ChartSelectionChangingEventArgs eventArgs)
  {
    if (this.SelectionChanging != null && eventArgs != null)
      this.SelectionChanging((object) this, eventArgs);
    if (!(this is SfChart) || this.SelectionBehaviour == null)
      return;
    this.SelectionBehaviour.OnSelectionChanging(eventArgs);
  }

  protected virtual void OnRootPanelSizeChanged(Size size)
  {
    if (!this.IsTemplateApplied || !this.RootPanelDesiredSize.HasValue)
      return;
    this.UpdateAction |= UpdateAction.LayoutAndRender;
    this.UpdateArea(true);
  }

  private static void OnRowColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartBase chartBase;
    switch (d)
    {
      case ChartAxis _:
        chartBase = (d as ChartAxis).Area;
        break;
      case ChartLegend _:
        chartBase = (d as ChartLegend).ChartArea;
        break;
      default:
        chartBase = (ChartBase) null;
        break;
    }
    chartBase?.ScheduleUpdate();
  }

  private static void OnSeriesSelectedIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as ChartBase).SeriesSelectedIndexChanged((int) args.NewValue, (int) args.OldValue);
  }

  private static void OnSideBySideSeriesPlacementProperty(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    ((ChartBase) d).ScheduleUpdate();
  }

  private static void OnColorModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartBase chartBase = d as ChartBase;
    if (chartBase.ColorModel == null)
      return;
    chartBase.ColorModel.Palette = chartBase.Palette;
    chartBase.ColorModel.ChartArea = chartBase;
  }

  private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ChartBase) d).OnPaletteChanged(e);
  }

  private void OnPaletteChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.ColorModel != null)
      this.ColorModel.Palette = this.Palette;
    if (this.VisibleSeries.Count <= 0)
      return;
    for (int index = 0; index < this.VisibleSeries.Count; ++index)
      this.VisibleSeries[index].Segments.Clear();
    this.IsUpdateLegend = true;
    this.ScheduleUpdate();
  }

  private void OnAreaTypeChanged() => this.UpdateAxisLayoutPanels();

  private void RaiseOnPrinting(PrintingEventArgs args)
  {
    if (this.OnPrinting == null)
      return;
    this.OnPrinting((object) this, args);
  }

  private void OnRowColChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Reset || e.Action == NotifyCollectionChangedAction.Remove)
      this.OnRowColumnCollectionChanged(e);
    this.ScheduleUpdate();
  }

  public object Legend
  {
    get => this.GetValue(ChartBase.LegendProperty);
    set => this.SetValue(ChartBase.LegendProperty, value);
  }

  internal ObservableCollection<ObservableCollection<LegendItem>> LegendItems { get; set; }

  internal bool HasDataPointBasedLegend()
  {
    if (this.Legend != null)
    {
      if (this.Legend is ChartLegendCollection legend2)
      {
        using (IEnumerator<ChartLegend> enumerator = legend2.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            ChartLegend current = enumerator.Current;
            return current.Series != null && this.VisibleSeries.Contains(current.Series);
          }
        }
      }
      else if (this.Legend is ChartLegend legend1 && legend1.Series != null)
        return this.VisibleSeries.Contains(legend1.Series);
    }
    return false;
  }

  internal void UpdateLegendArrangeRect(ChartLegend legend)
  {
    if (legend == null && !this.RootPanelDesiredSize.HasValue)
      return;
    Size desiredSize = legend.DesiredSize;
    if (this.Legend is IList && this.AreaType == ChartAreaType.CartesianAxes)
    {
      if (legend.XAxis == null || legend.YAxis == null || legend.InternalDockPosition != ChartDock.Floating)
        return;
      ChartAxis xaxis = legend.XAxis;
      ChartAxis yaxis = legend.YAxis;
      Rect rect = new Rect(xaxis.ArrangeRect.X, yaxis.ArrangeRect.Y, xaxis.ArrangeRect.Width, yaxis.ArrangeRect.Height);
      ChartBase.UpdateLegendInside(legend, rect);
    }
    else if (this.AreaType == ChartAreaType.PolarAxes && legend.GetPosition() == LegendPosition.Outside)
    {
      Rect rect = new Rect(0.0, 0.0, this.RootPanelDesiredSize.Value.Width, this.RootPanelDesiredSize.Value.Height);
      switch (legend.InternalDockPosition)
      {
        case ChartDock.Left:
          legend.ArrangeRect = new Rect(rect.Left, rect.Top, desiredSize.Width, rect.Height);
          break;
        case ChartDock.Top:
          legend.ArrangeRect = new Rect(rect.Left, rect.Top, rect.Width, desiredSize.Height);
          break;
        case ChartDock.Right:
          legend.ArrangeRect = new Rect(rect.Width + rect.Left, 0.0, desiredSize.Width, rect.Height);
          break;
        case ChartDock.Bottom:
          legend.ArrangeRect = new Rect(rect.Left, rect.Bottom, rect.Width, desiredSize.Height);
          break;
        case ChartDock.Floating:
          ChartBase.UpdateLegendInside(legend, rect);
          break;
      }
    }
    else
    {
      Rect rect = this.AreaType == ChartAreaType.None ? new Rect(0.0, 0.0, this.RootPanelDesiredSize.Value.Width, this.RootPanelDesiredSize.Value.Height) : this.SeriesClipRect;
      switch (legend.InternalDockPosition)
      {
        case ChartDock.Left:
          legend.ArrangeRect = new Rect(rect.Left - this.AxisThickness.Left, rect.Top, desiredSize.Width, rect.Height);
          break;
        case ChartDock.Top:
          legend.ArrangeRect = new Rect(rect.Left, rect.Top - this.AxisThickness.Top, rect.Width, desiredSize.Height);
          break;
        case ChartDock.Right:
          legend.ArrangeRect = new Rect(rect.Width + rect.Left + this.AxisThickness.Right, 0.0, desiredSize.Width, rect.Height);
          break;
        case ChartDock.Bottom:
          legend.ArrangeRect = new Rect(rect.Left, rect.Bottom + this.AxisThickness.Bottom, rect.Width, desiredSize.Height);
          break;
        case ChartDock.Floating:
          ChartBase.UpdateLegendInside(legend, this.AreaType == ChartAreaType.CartesianAxes ? this.seriesClipRect : rect);
          break;
      }
    }
  }

  internal void InitializeLegendItems()
  {
    if (this.Legend is ChartLegendCollection)
    {
      this.LegendItems = new ObservableCollection<ObservableCollection<LegendItem>>();
      for (int index = 0; index < (this.Legend as ChartLegendCollection).Count; ++index)
        this.LegendItems.Add(new ObservableCollection<LegendItem>());
    }
    else
    {
      ObservableCollection<ObservableCollection<LegendItem>> observableCollection = new ObservableCollection<ObservableCollection<LegendItem>>();
      observableCollection.Add(new ObservableCollection<LegendItem>());
      this.LegendItems = observableCollection;
    }
  }

  internal void UpdateLegendArrangeRect()
  {
    if (this.LegendCollection == null)
      return;
    foreach (ChartLegend legend in this.LegendCollection.OfType<ChartLegend>())
      this.UpdateLegendArrangeRect(legend);
    if (!(this.ChartAxisLayoutPanel is ChartCartesianAxisLayoutPanel chartAxisLayoutPanel))
      return;
    chartAxisLayoutPanel.UpdateLegendsArrangeRect();
  }

  internal void LayoutLegends()
  {
    for (int index = 0; index < this.RowDefinitions.Count; ++index)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      this.RowDefinitions[index].Legends.Clear();
      foreach (ChartLegend legend in (IEnumerable<UIElement>) this.LegendCollection)
      {
        if (this.GetActualRow((UIElement) legend) == index && legend.GetPosition() != LegendPosition.Inside)
        {
          if (legend.DockPosition == ChartDock.Left)
          {
            this.RowDefinitions[index].Legends.Add(legend);
            if (num1 < num3)
              ++num1;
            legend.RowColumnIndex = num1;
            ++num3;
          }
          else if (legend.DockPosition == ChartDock.Right)
          {
            this.RowDefinitions[index].Legends.Add(legend);
            if (num2 < num4)
              ++num2;
            legend.RowColumnIndex = num2;
            ++num4;
          }
          if (ChartDockPanel.GetDock((UIElement) legend) != legend.InternalDockPosition)
            ChartDockPanel.SetDock((UIElement) legend, legend.InternalDockPosition);
        }
      }
    }
    for (int index = 0; index < this.ColumnDefinitions.Count; ++index)
    {
      int num5 = 0;
      int num6 = 0;
      int num7 = 0;
      int num8 = 0;
      this.ColumnDefinitions[index].Legends.Clear();
      foreach (ChartLegend legend in (IEnumerable<UIElement>) this.LegendCollection)
      {
        if (this.GetActualColumn((UIElement) legend) == index && legend.GetPosition() != LegendPosition.Inside)
        {
          if (legend.DockPosition == ChartDock.Top)
          {
            if (num5 < num7)
              ++num5;
            legend.RowColumnIndex = num5;
            ++num7;
            this.ColumnDefinitions[index].Legends.Add(legend);
          }
          else if (legend.DockPosition == ChartDock.Bottom)
          {
            if (num6 < num8)
              ++num6;
            legend.RowColumnIndex = num6;
            ++num8;
            this.ColumnDefinitions[index].Legends.Add(legend);
          }
        }
        if (ChartDockPanel.GetDock((UIElement) legend) != legend.InternalDockPosition)
          ChartDockPanel.SetDock((UIElement) legend, legend.InternalDockPosition);
      }
    }
  }

  internal void UpdateLegend(object newLegend, bool isCollectionChanged)
  {
    try
    {
      if (this.ChartDockPanel == null)
        return;
      if (this.LegendCollection != null && isCollectionChanged)
      {
        foreach (UIElement element in this.LegendCollection.Where<UIElement>((Func<UIElement, bool>) (item => this.ChartDockPanel.Children.Contains(item))))
          this.ChartDockPanel.Children.Remove(element);
      }
      SfChart sfChart = this as SfChart;
      if (newLegend == null || sfChart != null && sfChart.Series == null)
        return;
      this.LegendCollection = (IList<UIElement>) new List<UIElement>();
      if (newLegend is ChartLegendCollection legendCollection)
      {
        legendCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.LegendCollectionChanged);
        legendCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(this.LegendCollectionChanged);
        foreach (UIElement uiElement in (Collection<ChartLegend>) legendCollection)
          this.LegendCollection.Add(uiElement);
        this.LayoutLegends();
      }
      else
        this.LegendCollection.Add(newLegend as UIElement);
      int index = 0;
      if (this.LegendItems == null)
        return;
      foreach (ChartLegend legend in (IEnumerable<UIElement>) this.LegendCollection)
      {
        if (!this.ChartDockPanel.Children.Contains((UIElement) legend))
          this.ChartDockPanel.Children.Add((UIElement) legend);
        if (legend.Items.Count == 0)
          legend.ItemsSource = (IEnumerable) this.LegendItems[index];
        this.SetLegendItemsSource(legend, index);
        ++index;
      }
    }
    catch
    {
    }
  }

  internal void SetLegendItemsSource(ChartLegend legend, int index)
  {
    if (legend == null || this.ActualSeries == null)
      return;
    legend.ChartArea = this;
    if (this.VisibleSeries.Count == 1 && this.VisibleSeries[0].IsSingleAccumulationSeries || this.HasDataPointBasedLegend())
      this.SetLegendItems(legend, index);
    else if (this.VisibleSeries.Count == 1 && this.ActualSeries.Count == 1 && this.VisibleSeries[0] is WaterfallSeries)
    {
      this.SetWaterfallLegendItems(legend, index);
      legend.XAxis = this.VisibleSeries[0].IsActualTransposed ? (this.VisibleSeries[0] as ISupportAxes).ActualYAxis : (this.VisibleSeries[0] as ISupportAxes).ActualXAxis;
      legend.YAxis = this.VisibleSeries[0].IsActualTransposed ? (this.VisibleSeries[0] as ISupportAxes).ActualXAxis : (this.VisibleSeries[0] as ISupportAxes).ActualYAxis;
    }
    else
    {
      List<ChartSeriesBase> second = new List<ChartSeriesBase>();
      if (this is SfChart sfChart)
      {
        foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) sfChart.TechnicalIndicators)
          second.Add((ChartSeriesBase) technicalIndicator);
      }
      List<ChartSeriesBase> source1 = sfChart != null ? this.ActualSeries.Union<ChartSeriesBase>((IEnumerable<ChartSeriesBase>) second).ToList<ChartSeriesBase>() : this.ActualSeries;
      if (this.AreaType == ChartAreaType.PolarAxes || this.AreaType == ChartAreaType.CartesianAxes && this.Legend is ChartLegendCollection)
        source1 = source1.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (actualSeries => this.GetActualRow((UIElement) legend) == this.GetActualRow(actualSeries.IsActualTransposed ? (UIElement) actualSeries.ActualXAxis : (UIElement) actualSeries.ActualYAxis) && this.GetActualColumn((UIElement) legend) == this.GetActualColumn(actualSeries.IsActualTransposed ? (UIElement) actualSeries.ActualYAxis : (UIElement) actualSeries.ActualXAxis))).ToList<ChartSeriesBase>();
      if (source1.Count > 0 && source1[0] is ISupportAxes)
      {
        legend.XAxis = source1[0].IsActualTransposed ? (source1[0] as ISupportAxes).ActualYAxis : (source1[0] as ISupportAxes).ActualXAxis;
        legend.YAxis = source1[0].IsActualTransposed ? (source1[0] as ISupportAxes).ActualXAxis : (source1[0] as ISupportAxes).ActualYAxis;
      }
      IEnumerable<ChartSeriesBase> source2;
      switch (this.AreaType)
      {
        case ChartAreaType.CartesianAxes:
          source2 = source1.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is ISupportAxes2D || series is ISupportAxes3D));
          break;
        case ChartAreaType.PolarAxes:
          source2 = source1.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is PolarRadarSeriesBase));
          break;
        default:
          source2 = source1.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is AccumulationSeriesBase || series is CircularSeriesBase3D));
          break;
      }
      ChartSeriesBase[] source3 = source2 is ChartSeries[] chartSeriesArray ? (ChartSeriesBase[]) chartSeriesArray : source2.ToArray<ChartSeriesBase>();
      foreach (ChartSeriesBase chartSeriesBase in source3)
      {
        if (chartSeriesBase.IsSeriesVisible && this.LegendItems[index].Count > 0 && index < this.LegendItems[index].Count)
          this.LegendItems[index].Clear();
      }
      foreach (ChartSeriesBase chartSeriesBase in ((IEnumerable<ChartSeriesBase>) source3).Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.VisibilityOnLegend == Visibility.Visible)))
      {
        ChartSeriesBase item = chartSeriesBase;
        List<LegendItem> list = this.LegendItems[index].Where<LegendItem>((Func<LegendItem, bool>) (it => it.Series == item)).ToList<LegendItem>();
        if (this.LegendItems.Count == 0 || list.Count<LegendItem>() == 0)
        {
          LegendItem legendItem = new LegendItem()
          {
            Legend = legend,
            Series = item
          };
          if (item.IsSingleAccumulationSeries)
          {
            legendItem.Legend.isSegmentsUpdated = true;
            legendItem.IsSeriesVisible = item.IsSeriesVisible;
            legendItem.Legend.isSegmentsUpdated = false;
          }
          this.LegendItems[index].Add(legendItem);
        }
      }
      foreach (ChartSeriesBase chartSeriesBase in source3)
      {
        if (chartSeriesBase is CartesianSeries cartesianSeries)
        {
          foreach (Trendline trendline in cartesianSeries.Trendlines.Where<Trendline>((Func<Trendline, bool>) (trendline => trendline.VisibilityOnLegend == Visibility.Visible)))
          {
            Trendline trend = trendline;
            List<LegendItem> list = this.LegendItems[index].Where<LegendItem>((Func<LegendItem, bool>) (it => it.Trendline == trend)).ToList<LegendItem>();
            if (this.LegendItems.Count == 0 || list.Count<LegendItem>() == 0)
            {
              LegendItem legendItem = new LegendItem()
              {
                Legend = legend,
                Trendline = (TrendlineBase) trend
              };
              this.LegendItems[index].Add(legendItem);
              trend.UpdateLegendIconTemplate(true);
            }
          }
        }
      }
    }
  }

  internal void LegendCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        ChartLegend newItem = e.NewItems[0] as ChartLegend;
        this.LegendItems.Add(new ObservableCollection<LegendItem>());
        newItem.ItemsSource = (IEnumerable) this.LegendItems[this.LegendItems.Count - 1];
        this.SetLegendItemsSource(newItem, this.LegendItems.Count - 1);
        this.LegendCollection.Add((UIElement) newItem);
        this.ChartDockPanel.Children.Add((UIElement) newItem);
        this.LayoutLegends();
        break;
      case NotifyCollectionChangedAction.Remove:
        ChartLegend oldItem = e.OldItems[0] as ChartLegend;
        if (this.LegendCollection.Contains((UIElement) oldItem))
          this.LegendCollection.Remove((UIElement) oldItem);
        if (!this.ChartDockPanel.Children.Contains((UIElement) oldItem))
          break;
        this.ChartDockPanel.Children.Remove((UIElement) oldItem);
        break;
      case NotifyCollectionChangedAction.Reset:
        foreach (UIElement legend in (IEnumerable<UIElement>) this.LegendCollection)
          this.ChartDockPanel.Children.Remove(legend);
        this.LegendCollection.Clear();
        break;
    }
  }

  private static void OnLegendPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartBase chartBase))
      return;
    chartBase.OnLegendPropertyChanged(e);
  }

  private static void UpdateLegendInside(ChartLegend legend, Rect rect)
  {
    double x = 0.0;
    double y = 0.0;
    double width = 0.0;
    double height = 0.0;
    switch (legend.DockPosition)
    {
      case ChartDock.Left:
        y = ChartBase.GetVerticalLegendAlignment(legend.VerticalAlignment, rect, legend.DesiredSize);
        x = rect.X;
        width = legend.DesiredSize.Width;
        height = rect.Height;
        break;
      case ChartDock.Top:
        y = rect.Y;
        x = ChartBase.GetHorizontalLegendAlignment(legend.HorizontalAlignment, rect, legend.DesiredSize);
        width = rect.Width;
        height = legend.DesiredSize.Height;
        break;
      case ChartDock.Right:
        y = ChartBase.GetVerticalLegendAlignment(legend.VerticalAlignment, rect, legend.DesiredSize);
        x = rect.X + rect.Width - legend.DesiredSize.Width;
        width = legend.DesiredSize.Width;
        height = rect.Height;
        break;
      case ChartDock.Bottom:
        y = rect.Y + rect.Height - legend.DesiredSize.Height;
        x = ChartBase.GetHorizontalLegendAlignment(legend.HorizontalAlignment, rect, legend.DesiredSize);
        width = rect.Width;
        height = legend.DesiredSize.Height;
        break;
      case ChartDock.Floating:
        x = rect.Left;
        y = rect.Top;
        width = legend.DesiredSize.Width;
        height = legend.DesiredSize.Height;
        break;
    }
    legend.ArrangeRect = new Rect(x, y, width, height);
  }

  private static double GetHorizontalLegendAlignment(
    HorizontalAlignment alignment,
    Rect rect,
    Size desiredSize)
  {
    double horizontalLegendAlignment = rect.Left;
    switch (alignment)
    {
      case HorizontalAlignment.Center:
        horizontalLegendAlignment = horizontalLegendAlignment + rect.Width / 2.0 - desiredSize.Width / 2.0;
        break;
      case HorizontalAlignment.Right:
        horizontalLegendAlignment = horizontalLegendAlignment + rect.Width - desiredSize.Width;
        break;
    }
    return horizontalLegendAlignment;
  }

  private static double GetVerticalLegendAlignment(
    VerticalAlignment alignment,
    Rect rect,
    Size desiredSize)
  {
    double verticalLegendAlignment = rect.Top;
    switch (alignment)
    {
      case VerticalAlignment.Center:
        verticalLegendAlignment = verticalLegendAlignment + rect.Height / 2.0 - desiredSize.Height / 2.0;
        break;
      case VerticalAlignment.Bottom:
        verticalLegendAlignment = verticalLegendAlignment + rect.Height - desiredSize.Height;
        break;
    }
    return verticalLegendAlignment;
  }

  private void OnLegendPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    this.InitializeLegendItems();
    this.UpdateLegend(e.NewValue, true);
  }

  private void SetLegendItems(ChartLegend legend, int index)
  {
    ChartSeriesBase chartSeriesBase = this.HasDataPointBasedLegend() ? legend.Series : this.VisibleSeries[0];
    List<double> actualSeriesYvalue = chartSeriesBase.ActualSeriesYValues[0] != null ? chartSeriesBase.ActualSeriesYValues[0] as List<double> : (List<double>) null;
    bool flag = chartSeriesBase is CircularSeriesBase circularSeriesBase && !double.IsNaN(circularSeriesBase.GroupTo);
    if (flag)
      actualSeriesYvalue = circularSeriesBase.GetGroupToYValues().Item1;
    this.LegendItems[index].Clear();
    int index1 = 0;
    List<ChartSegment> list = chartSeriesBase.Segments.ToList<ChartSegment>();
    if (chartSeriesBase is FunnelSeries)
      list.Reverse();
    for (int index2 = 0; index2 < actualSeriesYvalue.Count; ++index2)
    {
      int num = index2;
      if (double.IsNaN(actualSeriesYvalue[index2]))
      {
        switch (chartSeriesBase)
        {
          case TriangularSeriesBase _:
          case CircularSeriesBase3D _:
            continue;
          default:
            ++index1;
            continue;
        }
      }
      else
      {
        LegendItem target;
        if (list.Count == 0)
        {
          target = new LegendItem()
          {
            Index = num,
            Series = chartSeriesBase,
            Legend = legend
          };
        }
        else
        {
          ChartSegment chartSegment = list[index1];
          target = new LegendItem()
          {
            Index = num,
            Item = chartSegment.Item,
            Series = chartSeriesBase,
            Legend = legend
          };
          if (chartSeriesBase.ToggledLegendIndex.Count > 0 && (legend.CheckBoxVisibility == Visibility.Visible || legend.ToggleSeriesVisibility) && !chartSegment.IsSegmentVisible)
          {
            legend.isSegmentsUpdated = true;
            target.IsSeriesVisible = false;
            legend.isSegmentsUpdated = false;
          }
          BindingOperations.SetBinding((DependencyObject) target, LegendItem.InteriorProperty, (BindingBase) new Binding()
          {
            Source = (object) chartSegment,
            Path = new PropertyPath("Interior", new object[0])
          });
        }
        target.Label = !flag || circularSeriesBase.GroupedData.Count <= 0 || target.Index != list.Count - 1 ? (target.Item == null ? chartSeriesBase.GetActualXValue(target.Index).ToString() : chartSeriesBase.GetActualXValue(chartSeriesBase.ActualData.IndexOf(target.Item)).ToString()) : circularSeriesBase.GroupingLabel;
        this.LegendItems[index].Add(target);
        ++index1;
      }
    }
  }

  private void SetWaterfallLegendItems(ChartLegend legend, int index)
  {
    this.LegendItems[index].Clear();
    ChartSegment source1 = this.VisibleSeries[0].Segments.FirstOrDefault<ChartSegment>((Func<ChartSegment, bool>) (seg => (seg as WaterfallSegment).SegmentType == WaterfallSegmentType.Positive));
    if (source1 != null)
    {
      LegendItem waterfallLegendItem = this.GetWaterfallLegendItem((object) source1, ChartLocalizationResourceAccessor.Instance.GetString("Increase"), 0, legend);
      this.LegendItems[index].Add(waterfallLegendItem);
    }
    ChartSegment source2 = this.VisibleSeries[0].Segments.FirstOrDefault<ChartSegment>((Func<ChartSegment, bool>) (seg => (seg as WaterfallSegment).SegmentType == WaterfallSegmentType.Negative));
    if (source2 != null)
    {
      LegendItem waterfallLegendItem = this.GetWaterfallLegendItem((object) source2, ChartLocalizationResourceAccessor.Instance.GetString("Decrease"), 1, legend);
      this.LegendItems[index].Add(waterfallLegendItem);
    }
    ChartSegment source3 = this.VisibleSeries[0].Segments.FirstOrDefault<ChartSegment>((Func<ChartSegment, bool>) (seg => (seg as WaterfallSegment).SegmentType == WaterfallSegmentType.Sum));
    if (source3 == null)
      return;
    LegendItem waterfallLegendItem1 = this.GetWaterfallLegendItem((object) source3, ChartLocalizationResourceAccessor.Instance.GetString("Total"), 2, legend);
    this.LegendItems[index].Add(waterfallLegendItem1);
  }

  private LegendItem GetWaterfallLegendItem(
    object source,
    string label,
    int index,
    ChartLegend legend)
  {
    LegendItem target = new LegendItem()
    {
      Index = index,
      Series = this.VisibleSeries[0],
      Legend = legend
    };
    BindingOperations.SetBinding((DependencyObject) target, LegendItem.InteriorProperty, (BindingBase) new Binding()
    {
      Source = source,
      Path = new PropertyPath("Interior", new object[0])
    });
    target.Label = label;
    return target;
  }
}
