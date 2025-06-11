// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAdornment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAdornment : ChartSegment
{
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (Brush), typeof (ChartAdornment), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(nameof (BorderBrush), typeof (Brush), typeof (ChartAdornment), new PropertyMetadata((object) new SolidColorBrush(Colors.Transparent)));
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (Thickness), typeof (ChartAdornment), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)));
  public static readonly DependencyProperty MarginProperty = DependencyProperty.Register(nameof (Margin), typeof (Thickness), typeof (ChartAdornment), new PropertyMetadata((object) new Thickness().GetThickness(5.0, 5.0, 5.0, 5.0)));
  public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(nameof (FontFamily), typeof (FontFamily), typeof (ChartAdornment), new PropertyMetadata((object) new FontFamily("Times New Roman")));
  public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register(nameof (FontStyle), typeof (FontStyle), typeof (ChartAdornment), new PropertyMetadata((object) FontStyles.Normal));
  public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof (FontSize), typeof (double), typeof (ChartAdornment), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof (Foreground), typeof (Brush), typeof (ChartAdornment), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ConnectorRotationAngleProperty = DependencyProperty.Register(nameof (ConnectorRotationAngle), typeof (double), typeof (ChartAdornment), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ConnectorHeightProperty = DependencyProperty.Register(nameof (ConnectorHeight), typeof (double), typeof (ChartAdornment), new PropertyMetadata((object) 0.0));
  internal ChartAdornmentContainer adormentContainer;
  private double x;
  private double y;
  private double xpos;
  private double ypos;
  private double xData;
  private double yData;
  private ActualLabelPosition actualLabelPosition;

  public ChartAdornment()
  {
  }

  public ChartAdornment(double xVal, double yVal, double x, double y, ChartSeriesBase series)
  {
  }

  public new ChartSeriesBase Series { get; protected internal set; }

  public Brush Background
  {
    get => (Brush) this.GetValue(ChartAdornment.BackgroundProperty);
    set => this.SetValue(ChartAdornment.BackgroundProperty, (object) value);
  }

  public Thickness BorderThickness
  {
    get => (Thickness) this.GetValue(ChartAdornment.BorderThicknessProperty);
    set => this.SetValue(ChartAdornment.BorderThicknessProperty, (object) value);
  }

  public Brush BorderBrush
  {
    get => (Brush) this.GetValue(ChartAdornment.BorderBrushProperty);
    set => this.SetValue(ChartAdornment.BorderBrushProperty, (object) value);
  }

  public Thickness Margin
  {
    get => (Thickness) this.GetValue(ChartAdornment.MarginProperty);
    set => this.SetValue(ChartAdornment.MarginProperty, (object) value);
  }

  public FontFamily FontFamily
  {
    get => (FontFamily) this.GetValue(ChartAdornment.FontFamilyProperty);
    set => this.SetValue(ChartAdornment.FontFamilyProperty, (object) value);
  }

  public FontStyle FontStyle
  {
    get => (FontStyle) this.GetValue(ChartAdornment.FontStyleProperty);
    set => this.SetValue(ChartAdornment.FontStyleProperty, (object) value);
  }

  public double FontSize
  {
    get => (double) this.GetValue(ChartAdornment.FontSizeProperty);
    set => this.SetValue(ChartAdornment.FontSizeProperty, (object) value);
  }

  public Brush Foreground
  {
    get => (Brush) this.GetValue(ChartAdornment.ForegroundProperty);
    set => this.SetValue(ChartAdornment.ForegroundProperty, (object) value);
  }

  public double ConnectorRotationAngle
  {
    get => (double) this.GetValue(ChartAdornment.ConnectorRotationAngleProperty);
    set => this.SetValue(ChartAdornment.ConnectorRotationAngleProperty, (object) value);
  }

  public double ConnectorHeight
  {
    get => (double) this.GetValue(ChartAdornment.ConnectorHeightProperty);
    set => this.SetValue(ChartAdornment.ConnectorHeightProperty, (object) value);
  }

  public object ActualContent
  {
    get
    {
      if (this.Series.adornmentInfo == null)
        return (object) null;
      string segmentLabelFormat = this.Series.adornmentInfo.SegmentLabelFormat;
      LabelContent content = this.Series.adornmentInfo.SegmentLabelContent;
      if (this.Series.adornmentInfo.LabelTemplate == null)
        content = LabelContent.LabelContentPath;
      return this.CalculateLabelContent(content, segmentLabelFormat);
    }
  }

  public double XData
  {
    get => this.xData;
    set
    {
      this.xData = value;
      this.OnPropertyChanged(nameof (XData));
      this.OnPropertyChanged("ActualContent");
    }
  }

  public double YData
  {
    get => this.yData;
    set
    {
      this.yData = value;
      this.OnPropertyChanged(nameof (YData));
      this.OnPropertyChanged("ActualContent");
    }
  }

  public double X
  {
    get => this.x;
    internal set => this.x = value;
  }

  public double Y
  {
    get => this.y;
    internal set => this.y = value;
  }

  internal Brush ContrastForeground { get; set; }

  internal bool CanHideLabel
  {
    get => this.YData == 0.0 && this.Series is AccumulationSeriesBase || double.IsNaN(this.YData);
  }

  internal double XPos
  {
    get => this.xpos;
    set
    {
      this.xpos = value;
      this.OnPropertyChanged(nameof (XPos));
    }
  }

  internal double YPos
  {
    get => this.ypos;
    set
    {
      this.ypos = value;
      this.OnPropertyChanged(nameof (YPos));
    }
  }

  internal ActualLabelPosition ActualLabelPosition
  {
    get => this.actualLabelPosition;
    set => this.actualLabelPosition = value;
  }

  internal List<object> Data
  {
    get
    {
      return this.Series is HistogramSeries ? (this.Series.Segments[this.Series.Adornments.IndexOf(this)] as HistogramSegment).Data : (List<object>) null;
    }
  }

  internal ChartAdornmentInfo CustomAdornmentLabel { get; set; }

  internal string Label { get; set; }

  internal double GrandTotal { get; set; }

  public override void SetData(params double[] Values)
  {
    this.XData = Values[0];
    this.YData = Values[1];
    this.XPos = Values[2];
    this.YPos = Values[3];
  }

  public override UIElement CreateVisual(Size size)
  {
    if (this.adormentContainer == null)
      this.adormentContainer = new ChartAdornmentContainer(this);
    return (UIElement) this.adormentContainer;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.adormentContainer;

  public override void Update(IChartTransformer transformer)
  {
    ChartAdornment3D chartAdornment3D = this as ChartAdornment3D;
    if (this.Series is XyzDataSeries3D && !string.IsNullOrEmpty((this.Series as XyzDataSeries3D).ZBindingPath))
    {
      Vector3D visible3D = (transformer as ChartTransform.ChartCartesianTransformer).TransformToVisible3D(this.XPos, this.YPos, chartAdornment3D.StartDepth);
      this.X = visible3D.X;
      this.Y = visible3D.Y;
      chartAdornment3D.ActualStartDepth = visible3D.Z;
    }
    else
    {
      if (this.Series is RangeColumnSeries && !this.Series.IsMultipleYPathRequired)
        this.YPos = (this.Series.ActualYAxis.VisibleRange.End - Math.Abs(this.Series.ActualYAxis.VisibleRange.Start)) / 2.0;
      Point visible = transformer.TransformToVisible(this.XPos, this.YPos);
      this.X = visible.X;
      this.Y = visible.Y;
      if (chartAdornment3D == null)
        return;
      chartAdornment3D.ActualStartDepth = chartAdornment3D.StartDepth;
    }
  }

  public override void OnSizeChanged(Size size)
  {
  }

  internal virtual ChartAdornmentContainer GetAdornmentContainer() => this.adormentContainer;

  internal override UIElement CreateSegmentVisual(Size size)
  {
    this.BindColorProperties();
    return this.CreateVisual(size);
  }

  internal object GetTextContent()
  {
    if (this.CanHideLabel)
      return (object) "";
    string segmentLabelFormat = this.Series.adornmentInfo.SegmentLabelFormat;
    LabelContent content = this.Series.adornmentInfo.SegmentLabelContent;
    if (content == LabelContent.LabelContentPath)
      content = LabelContent.YValue;
    return this.CalculateLabelContent(content, segmentLabelFormat);
  }

  internal double CalculateSumOfValues(IList<double> yValues)
  {
    yValues = this.Series is HistogramSeries ? (IList<double>) (this.Series as HistogramSeries).ActualYValues : yValues;
    if (yValues != null)
      this.GrandTotal = yValues.Where<double>((Func<double, bool>) (val => !double.IsNaN(val))).Sum();
    return this.GrandTotal;
  }

  internal object CalculateLabelContent(LabelContent content, string labelFormat)
  {
    labelFormat = !this.Series.adornmentInfo.IsAdornmentLabelCreatedEventHooked || this.CustomAdornmentLabel == null || this.CustomAdornmentLabel.SegmentLabelFormat == null ? labelFormat : this.CustomAdornmentLabel.SegmentLabelFormat;
    switch (content)
    {
      case LabelContent.XValue:
        return (object) this.XData.ToString(labelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
      case LabelContent.YValue:
        if (double.IsNaN(this.YData))
          return (object) "";
        labelFormat = string.IsNullOrEmpty(labelFormat) ? "0.##" : labelFormat;
        return (object) this.YData.ToString(labelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
      case LabelContent.Percentage:
        double num1 = this.Series is HistogramSeries ? this.Series.GetGrandTotal((IList<double>) (this.Series as HistogramSeries).ActualYValues) : this.Series.GetGrandTotal(this.Series.ActualSeriesYValues[0]);
        labelFormat = string.IsNullOrEmpty(labelFormat) ? "0.##" : labelFormat;
        return (object) ((this.YData / num1 * 100.0).ToString(labelFormat, (IFormatProvider) CultureInfo.CurrentCulture) + "%");
      case LabelContent.YofTot:
        double num2 = this.Series is HistogramSeries ? this.Series.GetGrandTotal((IList<double>) (this.Series as HistogramSeries).ActualYValues) : this.Series.GetGrandTotal(this.Series.ActualSeriesYValues[0]);
        labelFormat = string.IsNullOrEmpty(labelFormat) ? "0.##" : labelFormat;
        return (object) $"{this.YData.ToString(labelFormat, (IFormatProvider) CultureInfo.CurrentCulture)} of {num2.ToString(labelFormat, (IFormatProvider) CultureInfo.CurrentCulture)}";
      case LabelContent.DateTime:
        object labelContent;
        if (this.Series.IsIndexed)
        {
          if (this.Series.ActualXValues is List<double> actualXvalues1)
          {
            labelContent = (object) actualXvalues1[(int) this.XData].FromOADate().ToString(this.Series.adornmentInfo.SegmentLabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
          }
          else
          {
            List<string> actualXvalues = this.Series.ActualXValues as List<string>;
            DateTime result = DateTime.MinValue;
            DateTime.TryParse(actualXvalues[(int) this.XData], out result);
            labelContent = (object) result.ToString(this.Series.adornmentInfo.SegmentLabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
          }
        }
        else
          labelContent = this.Series is HistogramSeries ? (object) this.XPos.FromOADate().ToString(this.Series.adornmentInfo.SegmentLabelFormat, (IFormatProvider) CultureInfo.CurrentCulture) : (object) this.XData.FromOADate().ToString(this.Series.adornmentInfo.SegmentLabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
        return labelContent;
      case LabelContent.LabelContentPath:
        return (object) this;
      default:
        if (double.IsNaN(this.YData))
          return (object) "";
        labelFormat = string.IsNullOrEmpty(labelFormat) ? "0.##" : labelFormat;
        return (object) this.YData.ToString(labelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
    }
  }

  internal void BindColorProperties()
  {
    bool flag = this.Series.ActualXAxis is CategoryAxis ? (this.Series.ActualXAxis as CategoryAxis).IsIndexed : !(this.Series.ActualXAxis is CategoryAxis3D) || (this.Series.ActualXAxis as CategoryAxis3D).IsIndexed;
    if (this.Series is WaterfallSeries)
    {
      if (this.Series.Segments.FirstOrDefault<ChartSegment>((Func<ChartSegment, bool>) (seg => seg.Item == this.Item)) is WaterfallSegment segment)
        this.BindWaterfallSegmentInterior(segment);
    }
    else if (!this.IsEmptySegmentInterior)
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("Interior", new object[0]),
        Converter = (IValueConverter) new InteriorConverter(this.Series),
        ConverterParameter = (!(this.Series is CircularSeriesBase) || double.IsNaN(((CircularSeriesBase) this.Series).GroupTo) ? (flag || !this.Series.IsSideBySide || this.Series is RangeSeriesBase || this.Series is FinancialSeriesBase ? (object) (this.Series is HistogramSeries ? this.Series.Adornments.IndexOf(this) : this.Series.ActualData.IndexOf(this.Item)) : (object) this.Series.GroupedActualData.IndexOf(this.Item)) : (object) this.Series.Adornments.IndexOf(this))
      });
    else
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        ConverterParameter = (object) this.Series.Interior,
        Path = new PropertyPath("EmptyPointInterior", new object[0]),
        Converter = (IValueConverter) new MultiInteriorConverter()
      });
    if (this is ChartAdornment3D)
      return;
    BindingOperations.SetBinding((DependencyObject) this, ChartSegment.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("Stroke", new object[0])
    });
    BindingOperations.SetBinding((DependencyObject) this, ChartSegment.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }
}
