// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAdornmentInfoBase
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

public abstract class ChartAdornmentInfoBase : DependencyObject, ICloneable
{
  public static readonly DependencyProperty LabelRotationAngleProperty = DependencyProperty.Register(nameof (LabelRotationAngle), typeof (double), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAdornmentInfoBase.OnLabelRotationAngleChanged)));
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (Brush), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAdornmentInfoBase.OnColorPropertyChanged)));
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (Thickness), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0), new PropertyChangedCallback(ChartAdornmentInfoBase.OnDefaultAdornmentChanged)));
  public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(nameof (BorderBrush), typeof (Brush), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(ChartAdornmentInfoBase.OnStylingPropertyChanged)));
  public static readonly DependencyProperty MarginProperty = DependencyProperty.Register(nameof (Margin), typeof (Thickness), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) new Thickness().GetThickness(5.0, 5.0, 5.0, 5.0), new PropertyChangedCallback(ChartAdornmentInfoBase.OnStylingPropertyChanged)));
  public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register(nameof (FontStyle), typeof (FontStyle), typeof (ChartAdornmentInfoBase), new PropertyMetadata(TextBlock.FontStyleProperty.GetMetadata(typeof (TextBlock)).DefaultValue, new PropertyChangedCallback(ChartAdornmentInfoBase.OnFontStylePropertyChanged)));
  public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof (FontSize), typeof (double), typeof (ChartAdornmentInfoBase), new PropertyMetadata(TextBlock.FontSizeProperty.GetMetadata(typeof (TextBlock)).DefaultValue, new PropertyChangedCallback(ChartAdornmentInfoBase.OnStylingPropertyChanged)));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof (Foreground), typeof (Brush), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAdornmentInfoBase.OnLabelsPropertyChanged)));
  public static readonly DependencyProperty UseSeriesPaletteProperty = DependencyProperty.Register(nameof (UseSeriesPalette), typeof (bool), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartAdornmentInfoBase.OnDefaultAdornmentChanged)));
  public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register(nameof (LabelPosition), typeof (AdornmentsLabelPosition), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) AdornmentsLabelPosition.Default, new PropertyChangedCallback(ChartAdornmentInfoBase.OnAdornmentPropertyChanged)));
  public static readonly DependencyProperty HighlightOnSelectionProperty = DependencyProperty.Register(nameof (HighlightOnSelection), typeof (bool), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartAdornmentInfoBase.OnHighlightOnSelectionChanged)));
  public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register(nameof (HorizontalAlignment), typeof (HorizontalAlignment), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) HorizontalAlignment.Center, new PropertyChangedCallback(ChartAdornmentInfoBase.OnAdornmentPropertyChanged)));
  public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register(nameof (VerticalAlignment), typeof (VerticalAlignment), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) VerticalAlignment.Center, new PropertyChangedCallback(ChartAdornmentInfoBase.OnAdornmentPropertyChanged)));
  public static readonly DependencyProperty ConnectorHeightProperty = DependencyProperty.Register(nameof (ConnectorHeight), typeof (double), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ChartAdornmentInfoBase.OnAdornmentPositionChanged)));
  public static readonly DependencyProperty ConnectorRotationAngleProperty = DependencyProperty.Register(nameof (ConnectorRotationAngle), typeof (double), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ChartAdornmentInfoBase.OnAdornmentPositionChanged)));
  public static readonly DependencyProperty ConnectorLineStyleProperty = DependencyProperty.Register(nameof (ConnectorLineStyle), typeof (Style), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAdornmentInfoBase.OnShowConnectingLine)));
  public static readonly DependencyProperty ShowConnectorLineProperty = DependencyProperty.Register(nameof (ShowConnectorLine), typeof (bool), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAdornmentInfoBase.OnShowConnectingLine)));
  public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.Register(nameof (LabelTemplate), typeof (DataTemplate), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAdornmentInfoBase.OnLabelChanged)));
  public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof (Symbol), typeof (ChartSymbol), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) ChartSymbol.Custom, new PropertyChangedCallback(ChartAdornmentInfoBase.OnSymbolTypeChanged)));
  public static readonly DependencyProperty SymbolWidthProperty = DependencyProperty.Register(nameof (SymbolWidth), typeof (double), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) 12.0, new PropertyChangedCallback(ChartAdornmentInfoBase.OnSymbolPropertyChanged)));
  public static readonly DependencyProperty SymbolHeightProperty = DependencyProperty.Register(nameof (SymbolHeight), typeof (double), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) 12.0, new PropertyChangedCallback(ChartAdornmentInfoBase.OnSymbolPropertyChanged)));
  public static readonly DependencyProperty SymbolTemplateProperty = DependencyProperty.Register(nameof (SymbolTemplate), typeof (DataTemplate), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAdornmentInfoBase.OnSymbolPropertyChanged)));
  public static readonly DependencyProperty SymbolInteriorProperty = DependencyProperty.Register(nameof (SymbolInterior), typeof (Brush), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartAdornmentInfoBase.OnSymbolInteriorChanged)));
  public static readonly DependencyProperty SymbolStrokeProperty = DependencyProperty.Register(nameof (SymbolStroke), typeof (Brush), typeof (ChartAdornmentInfoBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(nameof (FontFamily), typeof (FontFamily), typeof (ChartAdornmentInfoBase), new PropertyMetadata(TextBlock.FontFamilyProperty.GetMetadata(typeof (TextBlock)).DefaultValue, new PropertyChangedCallback(ChartAdornmentInfoBase.OnStylingPropertyChanged)));
  public static readonly DependencyProperty AdornmentsPositionProperty = DependencyProperty.Register(nameof (AdornmentsPosition), typeof (AdornmentsPosition), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) AdornmentsPosition.Top, new PropertyChangedCallback(ChartAdornmentInfoBase.OnAdornmentPositionChanged)));
  public static readonly DependencyProperty SegmentLabelContentProperty = DependencyProperty.Register(nameof (SegmentLabelContent), typeof (LabelContent), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) LabelContent.YValue, new PropertyChangedCallback(ChartAdornmentInfoBase.OnLabelChanged)));
  public static readonly DependencyProperty SegmentLabelFormatProperty = DependencyProperty.Register(nameof (SegmentLabelFormat), typeof (string), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(ChartAdornmentInfoBase.OnLabelChanged)));
  public static readonly DependencyProperty ShowMarkerProperty = DependencyProperty.Register(nameof (ShowMarker), typeof (bool), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartAdornmentInfoBase.OnShowMarker)));
  public static readonly DependencyProperty ShowLabelProperty = DependencyProperty.Register(nameof (ShowLabel), typeof (bool), typeof (ChartAdornmentInfoBase), new PropertyMetadata((object) false, new PropertyChangedCallback(ChartAdornmentInfoBase.OnLabelChanged)));
  internal UIElementsRecycler<ChartAdornmentContainer> adormentContainers;
  internal ChartSeriesBase series;
  private double labelPadding = 3.0;
  private double offsetX;
  private double offsetY;
  private double grandTotal;

  public double LabelRotationAngle
  {
    get => (double) this.GetValue(ChartAdornmentInfoBase.LabelRotationAngleProperty);
    set => this.SetValue(ChartAdornmentInfoBase.LabelRotationAngleProperty, (object) value);
  }

  public Brush Background
  {
    get => (Brush) this.GetValue(ChartAdornmentInfoBase.BackgroundProperty);
    set => this.SetValue(ChartAdornmentInfoBase.BackgroundProperty, (object) value);
  }

  public Thickness BorderThickness
  {
    get => (Thickness) this.GetValue(ChartAdornmentInfoBase.BorderThicknessProperty);
    set => this.SetValue(ChartAdornmentInfoBase.BorderThicknessProperty, (object) value);
  }

  public Brush BorderBrush
  {
    get => (Brush) this.GetValue(ChartAdornmentInfoBase.BorderBrushProperty);
    set => this.SetValue(ChartAdornmentInfoBase.BorderBrushProperty, (object) value);
  }

  public Thickness Margin
  {
    get => (Thickness) this.GetValue(ChartAdornmentInfoBase.MarginProperty);
    set => this.SetValue(ChartAdornmentInfoBase.MarginProperty, (object) value);
  }

  public FontStyle FontStyle
  {
    get => (FontStyle) this.GetValue(ChartAdornmentInfoBase.FontStyleProperty);
    set => this.SetValue(ChartAdornmentInfoBase.FontStyleProperty, (object) value);
  }

  public double FontSize
  {
    get => (double) this.GetValue(ChartAdornmentInfoBase.FontSizeProperty);
    set => this.SetValue(ChartAdornmentInfoBase.FontSizeProperty, (object) value);
  }

  public Brush Foreground
  {
    get => (Brush) this.GetValue(ChartAdornmentInfoBase.ForegroundProperty);
    set => this.SetValue(ChartAdornmentInfoBase.ForegroundProperty, (object) value);
  }

  public bool UseSeriesPalette
  {
    get => (bool) this.GetValue(ChartAdornmentInfoBase.UseSeriesPaletteProperty);
    set => this.SetValue(ChartAdornmentInfoBase.UseSeriesPaletteProperty, (object) value);
  }

  public AdornmentsLabelPosition LabelPosition
  {
    get => (AdornmentsLabelPosition) this.GetValue(ChartAdornmentInfoBase.LabelPositionProperty);
    set => this.SetValue(ChartAdornmentInfoBase.LabelPositionProperty, (object) value);
  }

  public bool HighlightOnSelection
  {
    get => (bool) this.GetValue(ChartAdornmentInfoBase.HighlightOnSelectionProperty);
    set => this.SetValue(ChartAdornmentInfoBase.HighlightOnSelectionProperty, (object) value);
  }

  public HorizontalAlignment HorizontalAlignment
  {
    get => (HorizontalAlignment) this.GetValue(ChartAdornmentInfoBase.HorizontalAlignmentProperty);
    set => this.SetValue(ChartAdornmentInfoBase.HorizontalAlignmentProperty, (object) value);
  }

  public VerticalAlignment VerticalAlignment
  {
    get => (VerticalAlignment) this.GetValue(ChartAdornmentInfoBase.VerticalAlignmentProperty);
    set => this.SetValue(ChartAdornmentInfoBase.VerticalAlignmentProperty, (object) value);
  }

  public double ConnectorHeight
  {
    get => (double) this.GetValue(ChartAdornmentInfoBase.ConnectorHeightProperty);
    set => this.SetValue(ChartAdornmentInfoBase.ConnectorHeightProperty, (object) value);
  }

  public double ConnectorRotationAngle
  {
    get => (double) this.GetValue(ChartAdornmentInfoBase.ConnectorRotationAngleProperty);
    set => this.SetValue(ChartAdornmentInfoBase.ConnectorRotationAngleProperty, (object) value);
  }

  public Style ConnectorLineStyle
  {
    get => (Style) this.GetValue(ChartAdornmentInfoBase.ConnectorLineStyleProperty);
    set => this.SetValue(ChartAdornmentInfoBase.ConnectorLineStyleProperty, (object) value);
  }

  public bool ShowConnectorLine
  {
    get => (bool) this.GetValue(ChartAdornmentInfoBase.ShowConnectorLineProperty);
    set => this.SetValue(ChartAdornmentInfoBase.ShowConnectorLineProperty, (object) value);
  }

  public DataTemplate LabelTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAdornmentInfoBase.LabelTemplateProperty);
    set => this.SetValue(ChartAdornmentInfoBase.LabelTemplateProperty, (object) value);
  }

  public ChartSymbol Symbol
  {
    get => (ChartSymbol) this.GetValue(ChartAdornmentInfoBase.SymbolProperty);
    set => this.SetValue(ChartAdornmentInfoBase.SymbolProperty, (object) value);
  }

  public double SymbolWidth
  {
    get => (double) this.GetValue(ChartAdornmentInfoBase.SymbolWidthProperty);
    set => this.SetValue(ChartAdornmentInfoBase.SymbolWidthProperty, (object) value);
  }

  public double SymbolHeight
  {
    get => (double) this.GetValue(ChartAdornmentInfoBase.SymbolHeightProperty);
    set => this.SetValue(ChartAdornmentInfoBase.SymbolHeightProperty, (object) value);
  }

  public DataTemplate SymbolTemplate
  {
    get => (DataTemplate) this.GetValue(ChartAdornmentInfoBase.SymbolTemplateProperty);
    set => this.SetValue(ChartAdornmentInfoBase.SymbolTemplateProperty, (object) value);
  }

  public Brush SymbolInterior
  {
    get => (Brush) this.GetValue(ChartAdornmentInfoBase.SymbolInteriorProperty);
    set => this.SetValue(ChartAdornmentInfoBase.SymbolInteriorProperty, (object) value);
  }

  public Brush SymbolStroke
  {
    get => (Brush) this.GetValue(ChartAdornmentInfoBase.SymbolStrokeProperty);
    set => this.SetValue(ChartAdornmentInfoBase.SymbolStrokeProperty, (object) value);
  }

  public FontFamily FontFamily
  {
    get => (FontFamily) this.GetValue(ChartAdornmentInfoBase.FontFamilyProperty);
    set => this.SetValue(ChartAdornmentInfoBase.FontFamilyProperty, (object) value);
  }

  public ChartSeriesBase Series
  {
    get => this.series;
    internal set
    {
      this.series = value;
      if (this.series == null || this.adormentContainers == null)
        return;
      this.adormentContainers.GenerateElements(this.series.Adornments.Count);
    }
  }

  public AdornmentsPosition AdornmentsPosition
  {
    get => (AdornmentsPosition) this.GetValue(ChartAdornmentInfoBase.AdornmentsPositionProperty);
    set => this.SetValue(ChartAdornmentInfoBase.AdornmentsPositionProperty, (object) value);
  }

  public LabelContent SegmentLabelContent
  {
    get => (LabelContent) this.GetValue(ChartAdornmentInfoBase.SegmentLabelContentProperty);
    set => this.SetValue(ChartAdornmentInfoBase.SegmentLabelContentProperty, (object) value);
  }

  public string SegmentLabelFormat
  {
    get => (string) this.GetValue(ChartAdornmentInfoBase.SegmentLabelFormatProperty);
    set => this.SetValue(ChartAdornmentInfoBase.SegmentLabelFormatProperty, (object) value);
  }

  public bool ShowMarker
  {
    get => (bool) this.GetValue(ChartAdornmentInfoBase.ShowMarkerProperty);
    set => this.SetValue(ChartAdornmentInfoBase.ShowMarkerProperty, (object) value);
  }

  public bool ShowLabel
  {
    get => (bool) this.GetValue(ChartAdornmentInfoBase.ShowLabelProperty);
    set => this.SetValue(ChartAdornmentInfoBase.ShowLabelProperty, (object) value);
  }

  internal double GrandTotal
  {
    get => this.grandTotal;
    set
    {
      if (this.grandTotal == value)
        return;
      this.grandTotal = value;
    }
  }

  internal UIElementsRecycler<FrameworkElement> LabelPresenters { get; set; }

  internal UIElementsRecycler<Path> ConnectorLines { get; set; }

  internal Size AdornmentInfoSize { get; set; }

  internal bool IsStraightConnectorLine2D { get; set; }

  internal bool ShowMarkerAtEdge2D { get; set; }

  internal bool IsMarkerRequired
  {
    get
    {
      if (!this.ShowMarker)
        return false;
      return this.Symbol == ChartSymbol.Custom && this.SymbolTemplate != null || this.Symbol != ChartSymbol.Custom;
    }
  }

  internal bool IsTextRequired
  {
    get
    {
      return this.Background == null && this.BorderThickness == new Thickness(0.0) && !this.UseSeriesPalette && this.LabelTemplate == null;
    }
  }

  internal double LabelPadding
  {
    get => this.labelPadding;
    set
    {
      if (this.labelPadding == value)
        return;
      this.labelPadding = value;
      this.OnAdornmentPropertyChanged();
    }
  }

  internal double OffsetX
  {
    get => this.offsetX;
    set
    {
      if (this.offsetX == value)
        return;
      this.offsetX = value;
      this.OnAdornmentPropertyChanged();
    }
  }

  internal double OffsetY
  {
    get => this.offsetY;
    set
    {
      if (this.offsetY == value)
        return;
      this.offsetY = value;
      this.OnAdornmentPropertyChanged();
    }
  }

  internal bool IsAdornmentLabelCreatedEventHooked
  {
    get => this.Series is AdornmentSeries series && series.IsAdornmentLabelCreatedEventHooked;
  }

  internal string Label { get; set; }

  internal int Index { get; set; }

  internal double XPosition { get; set; }

  internal double YPosition { get; set; }

  internal Brush LabelBackgroundBrush { get; set; }

  internal object Data { get; set; }

  internal void Dispose()
  {
    if (this.LabelPresenters != null && this.LabelPresenters.Count > 0 && this.LabelPresenters[0] is ContentControl)
    {
      foreach (DependencyObject labelPresenter in this.LabelPresenters)
        labelPresenter.SetValue(ContentControl.ContentProperty, (object) null);
      this.LabelPresenters.Clear();
    }
    if (this.adormentContainers != null)
    {
      foreach (ChartAdornmentContainer adormentContainer in this.adormentContainers)
        adormentContainer.Dispose();
      this.adormentContainers.Clear();
    }
    this.Series = (ChartSeriesBase) null;
  }

  public DependencyObject Clone() => this.CloneAdornmentInfo();

  internal static List<Point> GetBezierApproximation(
    IList<Point> controlPoints,
    int outputSegmentCount)
  {
    List<Point> bezierApproximation = new List<Point>();
    for (int index = 0; index <= outputSegmentCount; ++index)
    {
      double t = (double) index / (double) outputSegmentCount;
      bezierApproximation.Add(ChartAdornmentInfoBase.GetBezierPoint(t, controlPoints, 0, controlPoints.Count));
    }
    return bezierApproximation;
  }

  internal static void AlignElement(
    FrameworkElement control,
    ChartAlignment verticalAlignment,
    ChartAlignment horizontalAlignment,
    double x,
    double y)
  {
    switch (horizontalAlignment)
    {
      case ChartAlignment.Near:
        x -= control.DesiredSize.Width;
        break;
      case ChartAlignment.Center:
        x -= control.DesiredSize.Width / 2.0;
        break;
    }
    switch (verticalAlignment)
    {
      case ChartAlignment.Near:
        y -= control.DesiredSize.Height;
        break;
      case ChartAlignment.Center:
        y -= control.DesiredSize.Height / 2.0;
        break;
    }
    Canvas.SetLeft((UIElement) control, x);
    Canvas.SetTop((UIElement) control, y);
  }

  internal virtual void Arrange(Size finalSize)
  {
  }

  internal virtual DependencyObject CloneAdornmentInfo() => (DependencyObject) null;

  internal void UpdateLabels()
  {
    if (this.ShowLabel && this.LabelPresenters != null && this.series != null)
    {
      if (this.series.Adornments.Count <= 0)
        return;
      AdornmentSeries series = this.Series as AdornmentSeries;
      bool flag = this.IsTextRequired && (series == null || !series.IsAdornmentLabelCreatedEventHooked);
      if (flag)
      {
        if (this.LabelPresenters.Count > 0 && this.LabelPresenters[0] is ContentControl)
          this.LabelPresenters.Clear();
        this.CalculateVisibleAdornments();
        this.LabelPresenters.GenerateElementsOfType(this.series.VisibleAdornments.Count, typeof (TextBlock));
      }
      else
      {
        if (this.LabelPresenters.Count > 0 && this.LabelPresenters[0] is TextBlock)
          this.LabelPresenters.Clear();
        this.CalculateVisibleAdornments();
        this.LabelPresenters.GenerateElementsOfType(this.series.VisibleAdornments.Count, typeof (ContentControl));
      }
      if (this.series.VisibleAdornments.Count <= 0)
        return;
      RotateTransform target = new RotateTransform();
      Binding binding = new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("LabelRotationAngle", new object[0]),
        Mode = BindingMode.TwoWay
      };
      DataTemplate dataTemplate = this.LabelTemplate ?? ChartDictionaries.GenericCommonDictionary[(object) "AdornmentLabelTemplate"] as DataTemplate;
      Style style = new Style();
      if (flag)
      {
        style.TargetType = typeof (TextBlock);
        style.Setters.Add((SetterBase) new Setter()
        {
          Property = TextBlock.FontStyleProperty,
          Value = (object) this.FontStyle
        });
        style.Setters.Add((SetterBase) new Setter()
        {
          Property = TextBlock.FontFamilyProperty,
          Value = (object) this.FontFamily
        });
        style.Setters.Add((SetterBase) new Setter()
        {
          Property = TextBlock.FontSizeProperty,
          Value = (object) this.FontSize
        });
        style.Setters.Add((SetterBase) new Setter()
        {
          Property = FrameworkElement.MarginProperty,
          Value = (object) this.Margin
        });
        if (this.Foreground != null)
          style.Setters.Add((SetterBase) new Setter()
          {
            Property = TextBlock.ForegroundProperty,
            Value = (object) this.Foreground
          });
      }
      for (int index = 0; index < this.LabelPresenters.Count; ++index)
      {
        FrameworkElement labelPresenter = this.LabelPresenters[index];
        ChartAdornment visibleAdornment = this.Series.VisibleAdornments[index];
        object obj = visibleAdornment.Item;
        this.UpdateForeground(visibleAdornment);
        if (labelPresenter.Visibility == Visibility.Collapsed)
          labelPresenter.Visibility = Visibility.Visible;
        if (!this.Series.ActualArea.IsChartLoaded)
          labelPresenter.Visibility = Visibility.Collapsed;
        labelPresenter.Tag = !(this.series is CircularSeriesBase) || double.IsNaN(((CircularSeriesBase) this.series).GroupTo) ? (!(this.series.ActualXAxis is CategoryAxis) || (this.series.ActualXAxis as CategoryAxis).IsIndexed || !this.Series.IsSideBySide || this.Series is RangeSeriesBase || this.Series is FinancialSeriesBase || this.Series is WaterfallSeries ? (object) this.Series.ActualData.IndexOf(obj) : (object) this.series.GroupedActualData.IndexOf(obj)) : (object) index;
        if (flag)
        {
          labelPresenter.Style = style;
          TextBlock textBlock = labelPresenter as TextBlock;
          textBlock.Text = visibleAdornment.GetTextContent().ToString();
          textBlock.IsHitTestVisible = false;
        }
        else
        {
          Binding adormentBinding = ChartAdornmentInfoBase.CreateAdormentBinding("ActualContent", (object) this.series.VisibleAdornments[index]);
          labelPresenter.SetBinding(ContentControl.ContentProperty, (BindingBase) adormentBinding);
          if (this.LabelTemplate == null)
            labelPresenter.ClearValue(ContentControl.ContentTemplateProperty);
          (labelPresenter as ContentControl).ContentTemplate = dataTemplate;
        }
        TransformGroup transformGroup1 = new TransformGroup();
        labelPresenter.RenderTransform = (Transform) transformGroup1;
        if (visibleAdornment.CustomAdornmentLabel != null)
        {
          TransformGroup transformGroup2 = new TransformGroup();
          RotateTransform rotateTransform = new RotateTransform();
          rotateTransform.Angle = visibleAdornment.CustomAdornmentLabel.LabelRotationAngle;
          labelPresenter.RenderTransformOrigin = new Point(0.5, 0.5);
          transformGroup2.Children.Add((Transform) rotateTransform);
          labelPresenter.RenderTransform = (Transform) transformGroup2;
        }
        else if (this.LabelRotationAngle != 0.0)
        {
          BindingOperations.SetBinding((DependencyObject) target, RotateTransform.AngleProperty, (BindingBase) binding);
          labelPresenter.RenderTransformOrigin = new Point(0.5, 0.5);
          transformGroup1.Children.Add((Transform) target);
        }
      }
    }
    else
    {
      if (this.LabelPresenters == null)
        return;
      this.LabelPresenters.GenerateElements(0);
    }
  }

  internal void UpdateForeground(ChartAdornment adornment)
  {
    if (adornment.ContrastForeground != null)
      adornment.Foreground = (Brush) null;
    if (adornment != null && adornment.Foreground == null && this.Foreground == null)
    {
      if (this.Background != null)
        adornment.Foreground = this.Background.GetContrastColor();
      else if (this.UseSeriesPalette && adornment.Interior != null)
        adornment.Foreground = adornment.Interior.GetContrastColor();
      adornment.ContrastForeground = adornment.Foreground;
    }
    else
    {
      if (this.Foreground == null || adornment.Foreground != null)
        return;
      adornment.Foreground = this.Foreground;
      adornment.ContrastForeground = (Brush) null;
    }
  }

  private void CalculateVisibleAdornments()
  {
    int areaType = (int) this.series.ActualArea.AreaType;
    ChartAxisBase2D actualXaxis = this.series.ActualXAxis as ChartAxisBase2D;
    this.series.VisibleAdornments.Clear();
    if (actualXaxis != null && actualXaxis.ZoomFactor < 1.0)
    {
      DoubleRange visibleRange = actualXaxis.VisibleRange;
      double actualInterval = actualXaxis.ActualInterval;
      DoubleRange doubleRange = new DoubleRange(visibleRange.Start - actualInterval, visibleRange.End + actualInterval);
      for (int index = 0; index < this.series.Adornments.Count; ++index)
      {
        ChartAdornment adornment = this.series.Adornments[index];
        if (doubleRange.Inside(adornment.XPos))
          this.series.VisibleAdornments.Add(adornment);
      }
    }
    else
    {
      for (int index = 0; index < this.series.Adornments.Count; ++index)
        this.series.VisibleAdornments.Add(this.series.Adornments[index]);
    }
  }

  internal void UpdateConnectingLines()
  {
    if (this.ShowConnectorLine && this.ConnectorLines != null && this.series != null)
    {
      if (this.series.Adornments.Count <= 0)
        return;
      this.ConnectorLines.GenerateElements(this.Series.VisibleAdornments.Count);
      CircularSeriesBase series = this.series as CircularSeriesBase;
      for (int index = 0; index < this.ConnectorLines.Count; ++index)
      {
        if (double.IsNaN(this.Series.VisibleAdornments[index].YData))
        {
          this.ConnectorLines[index].Visibility = Visibility.Collapsed;
        }
        else
        {
          Style style = this.ConnectorLineStyle ?? ChartDictionaries.GenericCommonDictionary[(object) "pathStyle"] as Style;
          int num = series == null || double.IsNaN(series.GroupTo) ? this.series.ActualData.IndexOf(this.series.Adornments[index].Item) : this.series.Segments.IndexOf(this.series.Segments[index]);
          this.ConnectorLines[index].Visibility = Visibility.Visible;
          if (this.series.ActualArea.SelectedSeriesCollection.Contains(this.series) && this.series.ActualArea.GetSeriesSelectionBrush(this.series) != null && this.series.adornmentInfo.HighlightOnSelection && this.Series.ActualArea.GetEnableSeriesSelection() && this.series is ChartSeries)
            this.ConnectorLines[index].Stroke = this.series.ActualArea.GetSeriesSelectionBrush(this.series);
          else if (this.series.SelectedSegmentsIndexes.Contains(num) && this.series is ISegmentSelectable && this.series.adornmentInfo.HighlightOnSelection && (this.series as ISegmentSelectable).SegmentSelectionBrush != null && this.Series.ActualArea.GetEnableSegmentSelection())
            this.ConnectorLines[index].Stroke = (this.series as ISegmentSelectable).SegmentSelectionBrush;
          else if (this.UseSeriesPalette && !ChartAdornmentInfoBase.CheckStrokeAppliedInStyle(this.ConnectorLineStyle))
          {
            Binding binding = new Binding()
            {
              Source = (object) this.series.VisibleAdornments[index],
              Path = new PropertyPath("Interior", new object[0])
            };
            this.ConnectorLines[index].SetBinding(Shape.StrokeProperty, (BindingBase) binding);
          }
          else
            this.ConnectorLines[index].ClearValue(Shape.StrokeProperty);
          this.ConnectorLines[index].Style = style;
        }
      }
    }
    else
    {
      if (this.ConnectorLines == null)
        return;
      this.ConnectorLines.GenerateElements(0);
    }
  }

  internal void UpdateElements()
  {
    this.UpdateAdornments();
    this.UpdateLabels();
    this.UpdateConnectingLines();
  }

  internal double UpdateTriangularSeriesDataLabelPositionForExplodedSegment(int index, double x)
  {
    if (this.Series is PyramidSeries)
    {
      PyramidSeries series = this.Series as PyramidSeries;
      if (series.ExplodeAll || index == series.ExplodeIndex)
        x = series.ExplodeOffset + x;
    }
    else if (this.Series is FunnelSeries)
    {
      FunnelSeries series = this.Series as FunnelSeries;
      if (series.ExplodeAll || index == series.Adornments.Count - 1 - series.ExplodeIndex)
        x = series.ExplodeOffset + x;
    }
    return x;
  }

  internal void Measure(Size availableSize, Panel panel)
  {
    if (this.LabelPresenters == null && panel != null)
      this.LabelPresenters = new UIElementsRecycler<FrameworkElement>(panel);
    if (this.ConnectorLines == null && panel != null)
      this.ConnectorLines = new UIElementsRecycler<Path>(panel);
    if (this.adormentContainers == null && panel != null)
      this.adormentContainers = new UIElementsRecycler<ChartAdornmentContainer>(panel);
    int index1 = 0;
    if (this.adormentContainers != null && this.series.VisibleAdornments.Count > 0)
    {
      foreach (ChartAdornmentContainer adormentContainer in this.adormentContainers)
      {
        adormentContainer.Adornment = this.series.VisibleAdornments[index1];
        adormentContainer.Measure(availableSize);
        ++index1;
      }
    }
    int index2 = 0;
    if (!this.ShowLabel || this.series.VisibleAdornments.Count <= 0)
      return;
    for (; index2 < this.LabelPresenters.Count; ++index2)
    {
      FrameworkElement labelPresenter = this.LabelPresenters[index2];
      if (this.Series.ActualArea.IsChartLoaded && labelPresenter.Visibility == Visibility.Collapsed)
        labelPresenter.Visibility = Visibility.Visible;
      labelPresenter.Measure(availableSize);
      Panel.SetZIndex((UIElement) labelPresenter, 4);
      if (double.IsNaN(this.series.VisibleAdornments[index2].YData))
        labelPresenter.Visibility = Visibility.Collapsed;
      else
        labelPresenter.Visibility = Visibility.Visible;
    }
  }

  internal void UpdateSpiderLabels(double pieLeft, double pieRight, Size finalSize, double radius)
  {
    IList<ChartAdornment> orderedAdornments = this.GetOrderedAdornments();
    List<Rect> rectCollection = new List<Rect>();
    Rect rect1 = new Rect();
    double num1 = this.series is CircularSeriesBase ? (this.series is PieSeries ? ((PieSeries) this.series).InternalPieCoefficient : ((DoughnutSeries) this.series).InternalDoughnutCoefficient) : (this.series is DoughnutSeries3D ? ((DoughnutSeries3D) this.series).InternlDoughnutCoefficient : ((CircularSeriesBase3D) this.series).InternalCircleCoefficient);
    Point center = this.series is CircularSeriesBase ? (this.series as CircularSeriesBase).Center : (this.series as CircularSeriesBase3D).Center;
    double num2 = pieRight;
    double num3 = pieLeft;
    pieLeft = finalSize.Width / 2.0 - radius - radius * 0.5;
    pieRight = finalSize.Width / 2.0 + radius + radius * 0.5;
    double num4 = radius * 0.2;
    pieRight = pieRight > num2 ? num2 : pieRight;
    pieLeft = pieLeft < num3 ? num3 : pieLeft;
    int num5;
    double explodeRadius;
    ConnectorMode connectorType;
    if (this.series is CircularSeriesBase series1)
    {
      num5 = series1.ExplodeAll ? -2 : series1.ExplodeIndex;
      explodeRadius = series1.ExplodeRadius;
      connectorType = series1.ConnectorType;
    }
    else
    {
      CircularSeriesBase3D series = this.series as CircularSeriesBase3D;
      num5 = series.ExplodeAll ? -2 : series.ExplodeIndex;
      explodeRadius = series.ExplodeRadius;
      if (num5 == series.Segments.Count - 1 || series.ExplodeAll)
      {
        Rect rect2 = new Rect(0.0, 0.0, finalSize.Width, finalSize.Height);
        center = series.GetActualCenter(new Point(rect2.X + rect2.Width / 2.0, rect2.Y + rect2.Height / 2.0), radius);
      }
      connectorType = series.ConnectorType;
    }
    for (int index = 0; index < orderedAdornments.Count<ChartAdornment>(); ++index)
    {
      List<Point> pointList = new List<Point>();
      ChartAdornment chartAdornment = orderedAdornments[index];
      int num6 = this.series.Adornments.IndexOf(chartAdornment);
      if (this.ConnectorLines.Count > num6)
      {
        if (chartAdornment.CanHideLabel)
          this.ConnectorLines[num6].Visibility = Visibility.Collapsed;
        else
          this.ConnectorLines[num6].Visibility = Visibility.Visible;
      }
      double num7 = num6 == num5 ? explodeRadius : (num5 == -2 ? explodeRadius : 0.0);
      double connectorRotationAngle = chartAdornment.ConnectorRotationAngle;
      FrameworkElement labelPresenter = this.LabelPresenters[num6];
      double x1 = center.X + Math.Cos(connectorRotationAngle) * radius;
      double y1 = center.Y + Math.Sin(connectorRotationAngle) * radius;
      if (this.IsStraightConnectorLine2D)
      {
        pointList.Add(new Point(x1, y1));
      }
      else
      {
        double x2 = x1 + Math.Cos(connectorRotationAngle) * (num7 - radius * num1 / 10.0);
        double y2 = y1 + Math.Sin(connectorRotationAngle) * (num7 - radius * num1 / 10.0);
        pointList.Add(new Point(x2, y2));
        x1 = x2 + Math.Cos(connectorRotationAngle) * num4;
        y1 = y2 + Math.Sin(connectorRotationAngle) * num4;
        pointList.Add(new Point(x1, y1));
      }
      double num8 = connectorRotationAngle % (2.0 * Math.PI);
      bool flag = num8 > 1.57 && num8 < 4.71;
      if (labelPresenter != null)
      {
        double num9;
        double num10;
        if (flag)
        {
          num9 = pieLeft - labelPresenter.DesiredSize.Width / 2.0;
          num10 = labelPresenter.DesiredSize.Width / 2.0;
        }
        else
        {
          num9 = pieRight + labelPresenter.DesiredSize.Width / 2.0;
          num10 = -labelPresenter.DesiredSize.Width / 2.0;
        }
        double num11 = Math.Sqrt(Math.Pow(chartAdornment.X - num9, 2.0) + Math.Pow(chartAdornment.Y - y1, 2.0)) / 10.0;
        x1 = flag ? num9 + num11 : num9 - num11;
        Rect newRect = new Rect(x1, y1, labelPresenter.DesiredSize.Width, labelPresenter.DesiredSize.Height);
        if (rectCollection.IntersectWith(newRect))
        {
          pointList.Add(flag ? new Point(x1 + num4 + num10, y1) : new Point(x1 - num4 + num10, y1));
          y1 = rect1.Bottom + 2.0;
        }
        double num12 = num10;
        if (this.ShowMarkerAtEdge2D && this.ShowMarker && this.adormentContainers[num6] != null)
        {
          double num13 = x1 + num12;
          num12 = 0.0;
          x1 = num13 + (pointList.Last<Point>().X < center.X ? this.SymbolWidth / 2.0 : -this.SymbolWidth / 2.0);
        }
        pointList.Add(new Point(x1 + num12, y1));
        newRect.Y = y1;
        rect1 = newRect;
        rectCollection.Add(newRect);
      }
      this.DrawConnectorLine(num6, pointList, connectorType, this.Series is CircularSeriesBase3D, 0.0);
      if (this.ShowLabel)
      {
        if (this is ChartAdornmentInfo chartAdornmentInfo)
        {
          AdornmentsLabelPosition labelPosition = this.LabelPosition;
          double offsetX = this.OffsetX;
          double offsetY = this.OffsetY;
          if (this.IsAdornmentLabelCreatedEventHooked && chartAdornment.CustomAdornmentLabel != null)
          {
            labelPosition = chartAdornment.CustomAdornmentLabel.LabelPosition;
            offsetX = chartAdornment.CustomAdornmentLabel.OffsetX;
            offsetY = chartAdornment.CustomAdornmentLabel.OffsetY;
          }
          if (this.ShowMarker && this.ShowMarkerAtEdge2D && series1 != null)
          {
            ChartAdornmentContainer adornmentSymbol = (ChartAdornmentContainer) null;
            if (this.adormentContainers != null && this.adormentContainers.Count > num6)
              adornmentSymbol = this.adormentContainers[num6];
            this.AlignStraightConnectorLineLabel(labelPresenter, center, labelPosition, adornmentSymbol, x1, y1, series1.EnableSmartLabels, series1.LabelPosition);
          }
          else if (labelPosition == AdornmentsLabelPosition.Default)
            ChartAdornmentInfoBase.AlignElement(labelPresenter, this.GetChartAlignment(this.VerticalAlignment), this.GetChartAlignment(this.HorizontalAlignment), x1, y1);
          else
            chartAdornmentInfo.AlignAdornmentLabelPosition(labelPresenter, labelPosition, x1 + offsetX, y1 + offsetY, num6);
        }
        else
          ((ChartAdornmentInfo3D) this).AddLabel((UIElement) labelPresenter, x1, y1, (chartAdornment as ChartAdornment3D).ActualStartDepth, num6);
        if (this.ShowMarkerAtEdge2D && series1 != null && this.ShowMarker && this.adormentContainers != null && num6 < this.adormentContainers.Count)
          ChartAdornmentInfoBase.SetSymbolPosition(new Point(pointList.Last<Point>().X, pointList.Last<Point>().Y), this.adormentContainers[num6]);
      }
    }
  }

  internal void AlignStraightConnectorLineLabel(
    FrameworkElement label,
    Point center,
    AdornmentsLabelPosition adornmentsPosition,
    ChartAdornmentContainer adornmentSymbol,
    double x,
    double y,
    bool enableSmartLabel,
    CircularSeriesLabelPosition labelPosition)
  {
    double num1 = 0.0;
    double num2 = 0.0;
    if (adornmentSymbol != null && this.ShowMarker && this.ShowMarkerAtEdge2D)
    {
      if (this.SymbolTemplate == null)
      {
        num1 = this.SymbolWidth;
        num2 = this.SymbolHeight;
      }
      else
      {
        num1 = adornmentSymbol.DesiredSize.Width;
        num2 = adornmentSymbol.DesiredSize.Height;
      }
    }
    bool flag = x > center.X;
    if (!enableSmartLabel)
    {
      switch (adornmentsPosition)
      {
        case AdornmentsLabelPosition.Default:
        case AdornmentsLabelPosition.Outer:
          x = flag ? x + num1 / 2.0 : x - (label.DesiredSize.Width + num1 / 2.0);
          y -= label.DesiredSize.Height / 2.0;
          break;
        case AdornmentsLabelPosition.Auto:
        case AdornmentsLabelPosition.Inner:
          x = flag ? x - label.DesiredSize.Width - num1 / 2.0 : x + num1 / 2.0;
          y -= label.DesiredSize.Height;
          break;
        default:
          x -= label.DesiredSize.Width / 2.0;
          y = y - label.DesiredSize.Height - num2 / 2.0;
          break;
      }
    }
    else if (enableSmartLabel && labelPosition == CircularSeriesLabelPosition.OutsideExtended)
    {
      x = flag ? x + num1 / 2.0 : x - (label.DesiredSize.Width + num1 / 2.0);
      y -= label.DesiredSize.Height / 2.0;
    }
    else
    {
      x = flag ? x - (label.DesiredSize.Width / 2.0 - num1 / 2.0) : x - (label.DesiredSize.Width / 2.0 + num1 / 2.0);
      y -= label.DesiredSize.Height / 2.0;
    }
    Canvas.SetLeft((UIElement) label, x);
    Canvas.SetTop((UIElement) label, y);
  }

  internal static void SetSymbolPosition(
    Point ConnectorEndPoint,
    ChartAdornmentContainer adornmentPresenter)
  {
    Rect rect = new Rect(new Point(), adornmentPresenter.DesiredSize)
    {
      X = ConnectorEndPoint.X - adornmentPresenter.SymbolOffset.X,
      Y = ConnectorEndPoint.Y - adornmentPresenter.SymbolOffset.Y
    };
    Canvas.SetLeft((UIElement) adornmentPresenter, rect.Left);
    Canvas.SetTop((UIElement) adornmentPresenter, rect.Top);
  }

  internal void DrawConnectorLine(
    int connectorIndex,
    List<Point> drawingPoints,
    ConnectorMode connectorLineMode,
    bool is3DChart,
    double depth)
  {
    if (this.ConnectorLines.Count <= connectorIndex)
      return;
    Path connectorLine = this.ConnectorLines[connectorIndex];
    if (connectorLineMode == ConnectorMode.Bezier)
      drawingPoints = ChartAdornmentInfoBase.GetBezierApproximation((IList<Point>) drawingPoints, 256 /*0x0100*/);
    if (is3DChart)
      (this as ChartAdornmentInfo3D).DrawLineSegment3D(drawingPoints, connectorLine, depth, connectorIndex);
    else
      (this as ChartAdornmentInfo).DrawLineSegment(drawingPoints, connectorLine);
  }

  internal void PanelChanged(Panel panel)
  {
    if (this.LabelPresenters == null)
      this.LabelPresenters = new UIElementsRecycler<FrameworkElement>(panel);
    if (this.ConnectorLines == null)
      this.ConnectorLines = new UIElementsRecycler<Path>(panel);
    if (this.adormentContainers == null)
      this.adormentContainers = new UIElementsRecycler<ChartAdornmentContainer>(panel);
    this.UpdateAdornments();
    this.UpdateLabels();
    this.UpdateConnectingLines();
  }

  internal void ClearChildren()
  {
    if (this.LabelPresenters != null)
      this.LabelPresenters.Clear();
    if (this.adormentContainers != null)
      this.adormentContainers.Clear();
    if (this.ConnectorLines == null)
      return;
    this.ConnectorLines.Clear();
  }

  internal void AddAdornment(UIElement element, Panel panel)
  {
    if (this.adormentContainers == null)
      this.adormentContainers = new UIElementsRecycler<ChartAdornmentContainer>(panel);
    this.adormentContainers.Add(element as ChartAdornmentContainer);
  }

  internal void RemoveAdornment(UIElement element)
  {
    this.adormentContainers.Remove(element as ChartAdornmentContainer);
  }

  internal List<Point> GetAdornmentPositions(
    double pieRadius,
    IList<Rect> bounds,
    Size finalSize,
    ChartAdornment adornment,
    int labelIndex,
    double pieLeft,
    double pieRight,
    FrameworkElement label,
    ChartSeriesBase series,
    ref double x,
    ref double y,
    double angle,
    bool isPie)
  {
    double num1 = this.ShowConnectorLine ? adornment.ConnectorHeight : 0.0;
    AdornmentsLabelPosition adornmentsLabelPosition = !this.IsAdornmentLabelCreatedEventHooked || adornment.CustomAdornmentLabel == null ? this.LabelPosition : adornment.CustomAdornmentLabel.LabelPosition;
    List<Point> drawingPoints = new List<Point>()
    {
      new Point(x, y)
    };
    Point center = new Point(x, y);
    if (isPie)
    {
      drawingPoints.Clear();
      double explodeRadius;
      CircularSeriesLabelPosition labelPosition;
      int num2;
      bool flag;
      if (series is CircularSeriesBase circularSeriesBase)
      {
        explodeRadius = circularSeriesBase.ExplodeRadius;
        int explodeIndex = circularSeriesBase.ExplodeIndex;
        labelPosition = circularSeriesBase.LabelPosition;
        num2 = circularSeriesBase.ExplodeAll ? -2 : explodeIndex;
        flag = circularSeriesBase.EnableSmartLabels && this.ShowLabel && label != null;
        center = (double) this.Series.ActualArea.VisibleSeries.Count != 1.0 ? new Point(finalSize.Width / 2.0, finalSize.Height / 2.0) : circularSeriesBase.Center;
      }
      else
      {
        CircularSeriesBase3D circularSeriesBase3D = series as CircularSeriesBase3D;
        explodeRadius = circularSeriesBase3D.ExplodeRadius;
        int explodeIndex = circularSeriesBase3D.ExplodeIndex;
        labelPosition = circularSeriesBase3D.LabelPosition;
        num2 = circularSeriesBase3D.ExplodeAll ? -2 : explodeIndex;
        flag = circularSeriesBase3D.EnableSmartLabels && this.ShowLabel && label != null;
        center = circularSeriesBase3D.Center;
      }
      double explodedRadius = num2 == labelIndex || num2 == -2 ? explodeRadius : 0.0;
      double labelRadiusFromOrigin = pieRadius / 2.0 + num1;
      if (labelPosition != CircularSeriesLabelPosition.Inside)
      {
        labelRadiusFromOrigin = pieRadius + num1;
        if (this.Series is CircularSeriesBase3D && (num2 == (this.Series as CircularSeriesBase3D).Segments.Count - 1 || (this.Series as CircularSeriesBase3D).ExplodeAll))
          center = new Point(finalSize.Width / 2.0, finalSize.Height / 2.0);
        center.X += Math.Cos(angle) * explodedRadius;
        center.Y += Math.Sin(angle) * explodedRadius;
        drawingPoints.Add(new Point(center.X + Math.Cos(angle) * pieRadius, center.Y + Math.Sin(angle) * pieRadius));
        if (labelPosition != CircularSeriesLabelPosition.OutsideExtended && !this.IsStraightConnectorLine2D)
        {
          x = center.X + Math.Cos(angle) * labelRadiusFromOrigin;
          y = center.Y + Math.Sin(angle) * labelRadiusFromOrigin;
          if (adornmentsLabelPosition == AdornmentsLabelPosition.Auto)
          {
            x = x < 0.0 ? 0.0 : (x > center.X * 2.0 ? center.X * 2.0 : x);
            y = y < 0.0 ? 0.0 : (y > center.Y * 2.0 ? center.Y * 2.0 : y);
          }
          drawingPoints.Add(new Point(x, y));
        }
        if (labelPosition == CircularSeriesLabelPosition.Outside && this.IsStraightConnectorLine2D)
        {
          x = this.GetStraightLineXPosition(center, angle, x, num1);
          y = center.Y + Math.Sin(angle) * pieRadius;
          drawingPoints.Add(new Point(x, y));
        }
      }
      else
      {
        x += Math.Cos(angle) * explodedRadius;
        y += Math.Sin(angle) * explodedRadius;
        drawingPoints.Add(new Point(x, y));
        if (!this.IsStraightConnectorLine2D)
        {
          x += Math.Cos(angle) * num1;
          y += Math.Sin(angle) * num1;
          if (adornmentsLabelPosition == AdornmentsLabelPosition.Auto)
          {
            x = x < 0.0 ? 0.0 : (x > center.X * 2.0 ? center.X * 2.0 : x);
            y = y < 0.0 ? 0.0 : (y > center.Y * 2.0 ? center.Y * 2.0 : y);
          }
        }
        else
          x = this.GetStraightLineXPosition(center, angle, x, num1);
        drawingPoints.Add(new Point(x, y));
      }
      if (flag)
      {
        Rect currRect = new Rect(x, y, label.DesiredSize.Width, label.DesiredSize.Height);
        switch (labelPosition)
        {
          case CircularSeriesLabelPosition.Inside:
            Point point1 = this.SmartLabelsForInside(adornment, bounds, label, num1, labelRadiusFromOrigin, pieRadius + explodedRadius, drawingPoints, center, currRect);
            x = point1.X;
            y = point1.Y;
            break;
          case CircularSeriesLabelPosition.Outside:
            Point point2 = this.SmartLabelsForOutside(bounds, (IList<Point>) drawingPoints, currRect, label, center, labelRadiusFromOrigin, num1, explodedRadius, adornment);
            x = point2.X;
            y = point2.Y;
            break;
        }
      }
      else if (labelPosition == CircularSeriesLabelPosition.OutsideExtended)
      {
        double num3 = pieRight;
        double num4 = pieLeft;
        pieLeft = finalSize.Width / 2.0 - pieRadius - pieRadius;
        pieRight = finalSize.Width / 2.0 + pieRadius + pieRadius;
        if (!this.IsStraightConnectorLine2D)
        {
          x = center.X + Math.Cos(angle) * (pieRadius + pieRadius * 0.2);
          y = center.Y + Math.Sin(angle) * (pieRadius + pieRadius * 0.2);
          drawingPoints.Add(new Point(x, y));
        }
        double num5 = !this.ShowMarkerAtEdge2D || !this.ShowMarker ? 0.0 : this.SymbolWidth / 2.0;
        pieRight = pieRight > num3 ? num3 : pieRight;
        pieLeft = pieLeft < num4 ? num4 : pieLeft;
        double num6 = angle % (2.0 * Math.PI);
        x = num6 <= 1.57 && num6 >= 0.0 || num6 >= 4.71 ? (x < pieRight ? pieRight : x) : (x > pieLeft ? pieLeft : x);
        x = center.X < x ? x - num5 : x + num5;
        y = this.IsStraightConnectorLine2D ? center.Y + Math.Sin(angle) * pieRadius : y;
        drawingPoints.Add(new Point(x, y));
      }
    }
    else if (this.Series is TriangularSeriesBase)
    {
      x += Math.Cos(angle) * this.ConnectorHeight;
      y += Math.Sin(angle) * this.ConnectorHeight;
      if (adornmentsLabelPosition == AdornmentsLabelPosition.Auto)
      {
        x = x < 0.0 ? 0.0 : (x > center.X * 2.0 ? center.X * 2.0 : x);
        y = y < 0.0 ? 0.0 : (y > center.Y * 2.0 ? center.Y * 2.0 : y);
      }
      drawingPoints.Add(new Point(x, y));
    }
    else
    {
      Point connectorLinePoint = this.CalculateConnectorLinePoint(ref x, ref y, adornment, angle, labelIndex);
      drawingPoints.Add(connectorLinePoint);
    }
    return drawingPoints;
  }

  private double GetStraightLineXPosition(
    Point center,
    double angle,
    double x,
    double labelRadiusFromOrigin)
  {
    double num1 = Math.Abs(angle) % (2.0 * Math.PI);
    bool flag = num1 > 1.57 && num1 < 4.71;
    double num2 = !this.ShowMarkerAtEdge2D || !this.ShowMarker ? 0.0 : this.SymbolWidth;
    float d = flag ? 3.14159274f : 0.0f;
    return x + Math.Cos((double) d) * (labelRadiusFromOrigin - num2 / 2.0);
  }

  internal bool IsTop(int index)
  {
    double ydata = this.Series.Adornments[index].YData;
    double d1 = 0.0;
    double d2 = 0.0;
    if (this.Series.Adornments.Count - 1 > index)
      d1 = this.Series.Adornments[index + 1].YData;
    if (index > 0)
      d2 = this.Series.Adornments[index - 1].YData;
    if (index == 0)
      return double.IsNaN(d1) || ydata > d1;
    if (index == this.Series.Adornments.Count - 1)
      return double.IsNaN(d2) || ydata > d2;
    if (double.IsNaN(d1) && double.IsNaN(d2))
      return true;
    if (double.IsNaN(d1))
      return d2 <= ydata;
    if (double.IsNaN(d2))
      return d1 <= ydata;
    double num1 = (double) (index - 1);
    double num2 = (double) (index + 1);
    double num3 = (double) index;
    double num4 = (d1 - d2) / (num2 - num1);
    double num5 = d1 - num4 * num2;
    return num4 * num3 + num5 < ydata;
  }

  internal void GetActualLabelPosition(ChartAdornment adornment)
  {
    if (this.Series is RangeSeriesBase || this.Series is FinancialSeriesBase || this.Series is CircularSeriesBase3D || this.Series is AccumulationSeriesBase || this.Series is PolarRadarSeriesBase || this.Series is BoxAndWhiskerSeries)
      return;
    AdornmentsPosition adornmentsPosition = this.AdornmentsPosition;
    if (adornment.Series.IsActualTransposed)
      adornment.ActualLabelPosition = adornmentsPosition == AdornmentsPosition.Bottom ? (adornment.Series.ActualYAxis.IsInversed ^ adornment.YData < 0.0 ? ActualLabelPosition.Right : ActualLabelPosition.Left) : (adornment.Series.ActualYAxis.IsInversed ^ adornment.YData < 0.0 ? ActualLabelPosition.Left : ActualLabelPosition.Right);
    else
      adornment.ActualLabelPosition = adornmentsPosition == AdornmentsPosition.Bottom ? (adornment.Series.ActualYAxis.IsInversed ^ adornment.YData < 0.0 ? ActualLabelPosition.Top : ActualLabelPosition.Bottom) : (adornment.Series.ActualYAxis.IsInversed ^ adornment.YData < 0.0 ? ActualLabelPosition.Bottom : ActualLabelPosition.Top);
  }

  internal void OnAdornmentPropertyChanged()
  {
    if (this is ChartAdornmentInfo)
    {
      if ((this.adormentContainers == null || this.adormentContainers.Count <= 0) && (this.ConnectorLines == null || this.ConnectorLines.Count <= 0) && (this.LabelPresenters == null || this.LabelPresenters.Count <= 0))
        return;
      this.Measure(this.AdornmentInfoSize, (Panel) null);
      this.Arrange(this.AdornmentInfoSize);
    }
    else
    {
      if (this.series == null || this.series.ActualArea == null)
        return;
      this.series.ActualArea.ScheduleUpdate();
    }
  }

  internal AdornmentsPosition GetAdornmentPosition() => this.AdornmentsPosition;

  protected virtual void DrawLineSegment(List<Point> points, Path path)
  {
  }

  protected ChartAlignment GetChartAlignment(VerticalAlignment alignment)
  {
    if (alignment == VerticalAlignment.Bottom)
      return ChartAlignment.Far;
    return alignment == VerticalAlignment.Top ? ChartAlignment.Near : ChartAlignment.Center;
  }

  protected ChartAlignment GetChartAlignment(HorizontalAlignment alignment)
  {
    if (alignment == HorizontalAlignment.Right)
      return ChartAlignment.Far;
    return alignment == HorizontalAlignment.Left ? ChartAlignment.Near : ChartAlignment.Center;
  }

  private static Binding CreateAdormentBinding(string path, object source)
  {
    return new Binding()
    {
      Path = new PropertyPath(path, new object[0]),
      Source = source,
      Mode = BindingMode.OneWay
    };
  }

  private static void OnShowConnectingLine(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartAdornmentInfoBase adornmentInfoBase = d as ChartAdornmentInfoBase;
    adornmentInfoBase.UpdateConnectingLines();
    adornmentInfoBase.OnAdornmentPropertyChanged();
  }

  private static void OnShowMarker(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartAdornmentInfoBase adornmentInfoBase = d as ChartAdornmentInfoBase;
    if (!(bool) e.NewValue && adornmentInfoBase.adormentContainers != null)
      adornmentInfoBase.adormentContainers.GenerateElements(0);
    adornmentInfoBase.UpdateAdornments();
    adornmentInfoBase.OnAdornmentPropertyChanged();
  }

  private static Point GetBezierPoint(double t, IList<Point> controlPoints, int index, int count)
  {
    if (count == 1)
      return controlPoints[index];
    Point bezierPoint1 = ChartAdornmentInfoBase.GetBezierPoint(t, controlPoints, index, count - 1);
    Point bezierPoint2 = ChartAdornmentInfoBase.GetBezierPoint(t, controlPoints, index + 1, count - 1);
    return new Point((1.0 - t) * bezierPoint1.X + t * bezierPoint2.X, (1.0 - t) * bezierPoint1.Y + t * bezierPoint2.Y);
  }

  private static void OnSymbolTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAdornmentInfoBase).OnSymbolTypeChanged();
  }

  private static void OnFontStylePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartAdornmentInfoBase adornmentInfoBase = d as ChartAdornmentInfoBase;
    adornmentInfoBase.OnFontStylePropertyChanged();
    if (!adornmentInfoBase.IsAdornmentLabelCreatedEventHooked)
      return;
    adornmentInfoBase.OnStylingPropertyChanged();
  }

  private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ChartAdornmentInfoBase adornmentInfoBase = d as ChartAdornmentInfoBase;
    if (adornmentInfoBase.series != null)
      adornmentInfoBase.series.VisibleAdornments.Clear();
    adornmentInfoBase.UpdateLabels();
    adornmentInfoBase.OnAdornmentPropertyChanged();
  }

  private static void OnStylingPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAdornmentInfoBase).OnStylingPropertyChanged();
  }

  private static void OnLabelsPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAdornmentInfoBase).UpdateAdornmentLabelProperties();
    (d as ChartAdornmentInfoBase).UpdateLabels();
  }

  private static void OnColorPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAdornmentInfoBase).UpdateAdornmentLabelProperties();
    (d as ChartAdornmentInfoBase).OnColorPropertyChanged();
  }

  private static void OnAdornmentPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAdornmentInfoBase).UpdateArea();
  }

  private static void OnLabelRotationAngleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAdornmentInfoBase).UpdateLabels();
  }

  private static void OnDefaultAdornmentChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as ChartAdornmentInfoBase).OnDefaultAdornmentChanged();
  }

  private static void OnHighlightOnSelectionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAdornmentInfoBase).UpdateSelection(e);
  }

  private static void OnSymbolPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAdornmentInfoBase d1))
      return;
    if (d1.IsAdornmentLabelCreatedEventHooked)
      ChartAdornmentInfoBase.UpdateMarker(d1);
    else if (d1.adormentContainers != null)
      d1.adormentContainers.GenerateElements(0);
    d1.UpdateAdornments();
    d1.OnAdornmentPropertyChanged();
  }

  private static void UpdateMarker(ChartAdornmentInfoBase d)
  {
    ChartAdornmentInfoBase adornmentInfoBase = d;
    if (adornmentInfoBase.adormentContainers == null)
      return;
    foreach (ChartAdornmentContainer adormentContainer in adornmentInfoBase.adormentContainers)
    {
      int index = 0;
      adormentContainer.Adornment = adornmentInfoBase.Series.Adornments[index];
      int num = index + 1;
    }
  }

  private static void OnAdornmentPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartAdornmentInfoBase).OnAdornmentPropertyChanged();
  }

  private static void OnSymbolInteriorChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.OldValue != null)
      return;
    ChartAdornmentInfoBase d1 = d as ChartAdornmentInfoBase;
    if (d1.adormentContainers == null)
      return;
    if (d1.IsAdornmentLabelCreatedEventHooked)
    {
      ChartAdornmentInfoBase.UpdateMarker(d1);
    }
    else
    {
      foreach (ChartAdornmentContainer adormentContainer in d1.adormentContainers)
        adormentContainer.UpdateContainers(true);
    }
  }

  private static bool CheckStrokeAppliedInStyle(Style connectorStyle)
  {
    bool flag = false;
    if (connectorStyle != null)
    {
      foreach (Setter setter in (Collection<SetterBase>) connectorStyle.Setters)
      {
        if (setter.Property.Name == "Stroke")
        {
          if (setter.Value != null)
          {
            flag = true;
            break;
          }
          break;
        }
      }
    }
    return flag;
  }

  private void UpdateArea()
  {
    if (this.Series == null || this.Series.ActualArea == null)
      return;
    this.Series.ActualArea.ScheduleUpdate();
  }

  private void UpdateAdornmentLabelProperties()
  {
    if (this.series == null || !this.IsAdornmentLabelCreatedEventHooked)
      return;
    this.UpdateAdornments();
  }

  internal void UpdateAdornments()
  {
    if (this.adormentContainers != null && this.series != null && this.IsMarkerRequired)
    {
      if (this.series.Adornments.Count > 0)
      {
        this.CalculateVisibleAdornments();
        this.adormentContainers.GenerateElements(this.series.VisibleAdornments.Count);
      }
    }
    else if (this.adormentContainers != null)
      this.adormentContainers.GenerateElements(0);
    if (this.series == null)
      return;
    foreach (ChartAdornment adornment in (Collection<ChartAdornment>) this.series.Adornments)
    {
      object obj = (object) this;
      if (this.IsAdornmentLabelCreatedEventHooked && adornment.CustomAdornmentLabel != null)
      {
        ChartAdornmentInfo customAdornmentLabel = adornment.CustomAdornmentLabel;
        obj = (object) adornment;
        adornment.Background = customAdornmentLabel.Background != null ? customAdornmentLabel.Background : this.Background;
        adornment.BorderBrush = customAdornmentLabel.BorderBrush;
        adornment.BorderThickness = customAdornmentLabel.BorderThickness;
        adornment.FontFamily = customAdornmentLabel.FontFamily;
        adornment.FontSize = customAdornmentLabel.FontSize;
        adornment.FontStyle = customAdornmentLabel.FontStyle;
        adornment.Foreground = customAdornmentLabel.Foreground;
        adornment.Margin = customAdornmentLabel.Margin;
      }
      adornment.Series = this.series;
      adornment.BindColorProperties();
      Binding binding1 = new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("ConnectorHeight", new object[0])
      };
      BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.ConnectorHeightProperty, (BindingBase) binding1);
      Binding binding2;
      if (this.series is CircularSeriesBase || this.series is CircularSeriesBase3D)
        binding2 = new Binding()
        {
          Source = (object) adornment,
          Path = new PropertyPath("Angle", new object[0])
        };
      else
        binding2 = new Binding()
        {
          Source = (object) this,
          Path = new PropertyPath("ConnectorRotationAngle", new object[0]),
          Converter = (IValueConverter) new ConnectorRotationAngleConverter(this.series)
        };
      if (!(this.Series is AdornmentSeries series) || !series.IsAdornmentLabelCreatedEventHooked)
      {
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.ConnectorRotationAngleProperty, (BindingBase) binding2);
        Binding binding3 = new Binding()
        {
          Source = obj,
          Path = new PropertyPath("Foreground", new object[0])
        };
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.ForegroundProperty, (BindingBase) binding3);
        Binding binding4 = new Binding()
        {
          Source = obj,
          Path = new PropertyPath("FontFamily", new object[0])
        };
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.FontFamilyProperty, (BindingBase) binding4);
        Binding binding5 = new Binding()
        {
          Source = obj,
          Path = new PropertyPath("FontSize", new object[0])
        };
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.FontSizeProperty, (BindingBase) binding5);
        Binding binding6 = new Binding()
        {
          Source = obj,
          Path = new PropertyPath("FontStyle", new object[0])
        };
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.FontStyleProperty, (BindingBase) binding6);
        Binding binding7 = new Binding()
        {
          Source = obj,
          Path = new PropertyPath("Margin", new object[0])
        };
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.MarginProperty, (BindingBase) binding7);
        Binding binding8 = new Binding()
        {
          Source = obj,
          Path = new PropertyPath("BorderBrush", new object[0])
        };
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.BorderBrushProperty, (BindingBase) binding8);
        Binding binding9 = new Binding()
        {
          Source = obj,
          Path = new PropertyPath("BorderThickness", new object[0])
        };
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.BorderThicknessProperty, (BindingBase) binding9);
        Binding binding10 = new Binding()
        {
          Source = obj,
          Path = new PropertyPath("Background", new object[0])
        };
        BindingOperations.SetBinding((DependencyObject) adornment, ChartAdornment.BackgroundProperty, (BindingBase) binding10);
      }
    }
    if (this.adormentContainers == null || this.series.Adornments.Count <= 0)
      return;
    for (int index = 0; index < this.adormentContainers.Count; ++index)
    {
      object obj = this.series is HistogramSeries ? this.Series.ActualData[index] : this.Series.Adornments[index].Item;
      if (this.series is CircularSeriesBase && !double.IsNaN(((CircularSeriesBase) this.series).GroupTo))
        this.adormentContainers[index].Tag = (object) index;
      else if (this.series.ActualXAxis is CategoryAxis && !(this.series.ActualXAxis as CategoryAxis).IsIndexed && this.Series.IsSideBySide && !(this.Series is RangeSeriesBase) && !(this.Series is FinancialSeriesBase) && !(this.Series is WaterfallSeries))
        this.adormentContainers[index].Tag = (object) this.Series.GroupedActualData.IndexOf(obj);
      else
        this.adormentContainers[index].Tag = (object) this.Series.ActualData.IndexOf(obj);
    }
  }

  private Point CalculateConnectorLinePoint(
    ref double x,
    ref double y,
    ChartAdornment adornment,
    double angle,
    int index)
  {
    switch (this.Series is ChartSeries3D ? (int) (this.Series as ChartSeries3D).Adornments[index].ActualLabelPosition : (int) (this.Series as AdornmentSeries).Adornments[index].ActualLabelPosition)
    {
      case 0:
        x += Math.Cos(angle) * this.ConnectorHeight;
        y += Math.Sin(angle) * this.ConnectorHeight;
        break;
      case 1:
        y += Math.Sin(angle) * this.ConnectorHeight;
        x -= Math.Cos(angle) * this.ConnectorHeight;
        break;
      case 2:
        y += Math.Sin(angle) * this.ConnectorHeight;
        x += Math.Cos(angle) * this.ConnectorHeight;
        break;
      case 3:
        x += Math.Cos(angle) * this.ConnectorHeight;
        y += Math.Sin(-angle) * this.ConnectorHeight;
        break;
    }
    if ((!this.IsAdornmentLabelCreatedEventHooked || adornment.CustomAdornmentLabel == null ? (int) this.LabelPosition : (int) adornment.CustomAdornmentLabel.LabelPosition) == 1 && (this.Series is ChartSeries3D || (this.Series.ActualYAxis as ChartAxisBase2D).ZoomFactor >= 1.0 && (this.Series.ActualXAxis as ChartAxisBase2D).ZoomFactor >= 1.0))
    {
      if (this.Series is PolarRadarSeriesBase)
      {
        x = x < 0.0 ? 0.0 : (x > this.Series.ActualArea.SeriesClipRect.Width ? this.Series.ActualArea.SeriesClipRect.Width : x);
        y = y < 0.0 ? 0.0 : (y > this.Series.ActualArea.SeriesClipRect.Height ? this.Series.ActualArea.SeriesClipRect.Height : y);
      }
      else if (this.Series is ChartSeries3D)
      {
        double num1 = this.series.IsActualTransposed ? this.series.ActualXAxis.RenderedRect.Height : this.series.ActualYAxis.RenderedRect.Height;
        double num2 = this.series.IsActualTransposed ? this.series.ActualYAxis.RenderedRect.Right : this.series.ActualXAxis.RenderedRect.Right;
        x = x < 0.0 ? 0.0 : (x > num2 ? num2 : x);
        y = y < 0.0 ? 0.0 : (y > num1 ? num1 : y);
      }
      else
      {
        x = x < 0.0 ? 0.0 : (x > this.Series.Clip.Bounds.Width ? this.Series.Clip.Bounds.Width : x);
        y = y < 0.0 ? 0.0 : (y > this.Series.Clip.Bounds.Height ? this.Series.Clip.Bounds.Height : y);
      }
    }
    return new Point(x, y);
  }

  private Point SmartLabelsForOutside(
    IList<Rect> bounds,
    IList<Point> drawingPoints,
    Rect currRect,
    FrameworkElement label,
    Point center,
    double labelRadiusFromOrigin,
    double connectorHeight,
    double explodedRadius,
    ChartAdornment pieAdornment)
  {
    double num1 = 0.0;
    double connectorRotationAngle = pieAdornment.ConnectorRotationAngle;
    if (pieAdornment.Series is CircularSeriesBase)
      num1 = ((CircularSeriesBase) pieAdornment.Series).StartAngle * Math.PI / 180.0;
    double num2 = connectorRotationAngle;
    bool flag1 = false;
    drawingPoints.RemoveAt(1);
    bool flag2;
    do
    {
      flag2 = false;
      if (bounds.IntersectWith(currRect))
      {
        flag1 = flag2 = true;
        if (connectorRotationAngle > 2.0 * Math.PI + num1)
        {
          label.Visibility = Visibility.Collapsed;
          flag1 = flag2 = false;
          int index = this.LabelPresenters.IndexOf(label);
          if (this.ConnectorLines.Count > index)
            this.ConnectorLines[index].Visibility = Visibility.Collapsed;
        }
        connectorRotationAngle += 0.01;
        double num3 = center.X + Math.Cos(connectorRotationAngle) * labelRadiusFromOrigin;
        double num4 = center.Y + Math.Sin(connectorRotationAngle) * labelRadiusFromOrigin;
        currRect.X = num3;
        currRect.Y = num4;
      }
    }
    while (flag2);
    double x1 = currRect.X;
    double y = currRect.Y;
    bounds.Add(currRect);
    drawingPoints.Add(flag1 ? new Point(pieAdornment.X + Math.Cos(num2) * (connectorHeight + explodedRadius - connectorHeight / 1.5), pieAdornment.Y + Math.Sin(num2) * (connectorHeight + explodedRadius - connectorHeight / 1.5)) : new Point(x1, y));
    drawingPoints.Add(new Point(x1, y));
    if (this.ShowConnectorLine && connectorHeight != 0.0)
    {
      double num5 = connectorRotationAngle % (2.0 * Math.PI);
      bool flag3 = num5 <= (flag1 ? 1.35 : 1.55) && num5 >= 0.0 || num5 >= (flag1 ? 4.51 : 4.71);
      double num6 = connectorHeight / 5.0;
      double x2 = x1 + (flag3 ? num6 : -num6);
      drawingPoints.Add(new Point(x2, y));
      x1 = x2 + (flag3 ? label.DesiredSize.Width / 2.0 : -label.DesiredSize.Width / 2.0);
    }
    return new Point(x1, y);
  }

  private Point SmartLabelsForInside(
    ChartAdornment adornment,
    IList<Rect> bounds,
    FrameworkElement label,
    double connectorHeight,
    double labelRadiusFromOrigin,
    double pieRadius,
    List<Point> drawingPoints,
    Point center,
    Rect currRect)
  {
    bool flag1 = false;
    labelRadiusFromOrigin = pieRadius + connectorHeight + (label.DesiredSize.Width + label.DesiredSize.Height) / 2.0;
    double num1 = 0.0;
    double connectorRotationAngle = adornment.ConnectorRotationAngle;
    double num2 = 0.0;
    if (adornment.Series.Segments.Count > 0)
    {
      if (adornment.Series.Segments[0] is PieSegment)
        num2 = (adornment.Series.Segments[0] as PieSegment).AngleOfSlice / 2.0;
      else if (adornment.Series.Segments[0] is DoughnutSegment)
        num2 = (adornment.Series.Segments[0] as DoughnutSegment).AngleOfSlice / 2.0;
    }
    if (adornment.Series is CircularSeriesBase)
      num1 = ((CircularSeriesBase) adornment.Series).StartAngle * Math.PI / 180.0;
    double x = currRect.X;
    double y = currRect.Y;
    double num3 = connectorRotationAngle;
    int num4 = this.LabelPresenters.IndexOf(label);
    Rect centeredRect = new Rect(currRect.X - currRect.Width / 2.0, currRect.Y - currRect.Height / 2.0, currRect.Width, currRect.Height);
    bool flag2 = this.IsLabelOutsideSegmentSector(num4, centeredRect);
    bool flag3;
    do
    {
      flag3 = false;
      if (bounds.IntersectWith(currRect) || flag2)
      {
        flag2 = false;
        flag1 = flag3 = true;
        if (num3 > 2.0 * Math.PI + (num1 + num2))
        {
          label.Visibility = Visibility.Collapsed;
          flag1 = flag3 = false;
          if (this.ConnectorLines.Count > num4)
            this.ConnectorLines[num4].Visibility = Visibility.Collapsed;
        }
        num3 += 0.01;
        x = center.X + Math.Cos(num3) * labelRadiusFromOrigin;
        y = center.Y + Math.Sin(num3) * labelRadiusFromOrigin;
        currRect.X = x;
        currRect.Y = y;
      }
    }
    while (flag3);
    if (flag1)
    {
      drawingPoints.Clear();
      drawingPoints.Add(new Point(center.X + Math.Cos(connectorRotationAngle) * pieRadius, center.Y + Math.Sin(connectorRotationAngle) * pieRadius));
      drawingPoints.Add(new Point(center.X + Math.Cos(connectorRotationAngle) * pieRadius, center.Y + Math.Sin(connectorRotationAngle) * pieRadius));
    }
    drawingPoints.Add(new Point(x, y));
    bounds.Add(currRect);
    if (flag1 && this.ShowConnectorLine && connectorHeight != 0.0)
    {
      bool flag4 = connectorRotationAngle % (2.0 * Math.PI) <= 1.55 && connectorRotationAngle % (2.0 * Math.PI) >= 0.0 || connectorRotationAngle % (2.0 * Math.PI) >= 4.71;
      x += flag4 ? label.DesiredSize.Width / 2.0 : -(label.DesiredSize.Width / 2.0);
    }
    return new Point(x, y);
  }

  private bool IsLabelOutsideSegmentSector(int labelIndex, Rect centeredRect)
  {
    Path renderedVisual = this.Series.Segments[labelIndex].GetRenderedVisual() as Path;
    bool flag = false;
    if (renderedVisual != null && renderedVisual.Data != null)
      flag = !renderedVisual.Data.FillContains(centeredRect.TopLeft) || !renderedVisual.Data.FillContains(centeredRect.TopRight) || !renderedVisual.Data.FillContains(centeredRect.BottomRight) || !renderedVisual.Data.FillContains(centeredRect.BottomLeft);
    return flag;
  }

  private void OnSymbolTypeChanged()
  {
    this.UpdateAdornments();
    this.OnAdornmentPropertyChanged();
  }

  private void OnFontStylePropertyChanged()
  {
    if (!this.IsTextRequired)
      return;
    this.UpdateLabels();
  }

  private void OnStylingPropertyChanged()
  {
    if (this.IsTextRequired)
    {
      this.UpdateLabels();
      this.OnAdornmentPropertyChanged();
    }
    else if (this is ChartAdornmentInfo)
      this.UpdateArea();
    else
      this.OnDefaultAdornmentChanged();
  }

  private void OnColorPropertyChanged()
  {
    if (this.IsTextRequired ^ (this.LabelPresenters != null && this.LabelPresenters.Count > 0 && this.LabelPresenters[0] is TextBlock))
      this.UpdateArea();
    else
      this.UpdateLabels();
  }

  private void OnDefaultAdornmentChanged()
  {
    this.UpdateLabels();
    this.UpdateConnectingLines();
    this.UpdateAdornments();
    this.OnAdornmentPropertyChanged();
  }

  private void UpdateSelection(DependencyPropertyChangedEventArgs e)
  {
    if (this.Series == null || this.Series.ActualArea == null)
      return;
    if ((bool) e.NewValue)
    {
      if (this.Series.ActualArea.GetEnableSeriesSelection() && this.Series.ActualArea.SeriesSelectedIndex > -1 && this.Series is ChartSeries)
      {
        this.Series.AdornmentPresenter.UpdateAdornmentSelection(this.series.Adornments.Select<ChartAdornment, int>((Func<ChartAdornment, int>) (adorment => this.series.Adornments.IndexOf(adorment))).ToList<int>(), false);
      }
      else
      {
        if (!(this.Series is ISegmentSelectable) || (this.Series as ISegmentSelectable).SelectedIndex <= -1)
          return;
        this.Series.UpdateAdornmentSelection((this.Series as ISegmentSelectable).SelectedIndex);
      }
    }
    else if (this.Series.ActualArea.SeriesSelectedIndex > -1 && this.Series.ActualArea.GetEnableSeriesSelection() && this.Series.ActualArea.GetSeriesSelectionBrush(this.Series) != null)
    {
      this.Series.AdornmentPresenter.ResetAdornmentSelection(new int?(), true);
    }
    else
    {
      if (!(this.Series is ISegmentSelectable) || (this.Series as ISegmentSelectable).SelectedIndex <= -1 || !this.Series.ActualArea.GetEnableSegmentSelection() || (this.Series as ISegmentSelectable).SegmentSelectionBrush == null)
        return;
      this.Series.AdornmentPresenter.ResetAdornmentSelection(new int?((this.Series as ISegmentSelectable).SelectedIndex), false);
    }
  }

  private IList<ChartAdornment> GetOrderedAdornments()
  {
    return (IList<ChartAdornment>) this.series.Adornments.Where<ChartAdornment>((Func<ChartAdornment, bool>) (item => item.ConnectorRotationAngle % (2.0 * Math.PI) > 1.57 && item.ConnectorRotationAngle % (2.0 * Math.PI) < 4.71)).ToList<ChartAdornment>().OrderBy<ChartAdornment, double>((Func<ChartAdornment, double>) (item => item.Y)).Union<ChartAdornment>((IEnumerable<ChartAdornment>) this.series.Adornments.Where<ChartAdornment>((Func<ChartAdornment, bool>) (item => item.ConnectorRotationAngle % (2.0 * Math.PI) <= 1.57 || item.ConnectorRotationAngle % (2.0 * Math.PI) >= 4.71)).ToList<ChartAdornment>().OrderBy<ChartAdornment, double>((Func<ChartAdornment, double>) (item => item.Y))).ToList<ChartAdornment>();
  }
}
