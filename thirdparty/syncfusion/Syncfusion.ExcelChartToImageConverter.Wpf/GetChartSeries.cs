// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.GetChartSeries
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class GetChartSeries : ChartCommon
{
  private const int ChartPointsCount = 1000;
  internal int PieAngle = 270;
  internal ObservableCollection<ChartPoint> FirstSeriesPoints;
  internal int[] SortedIndexes;
  internal string[] RectangleSerieTypes = new string[8]
  {
    "ColumnSeries",
    "BarSeries",
    "StackingBar100Series",
    "StackingBarSeries",
    "StackingColumn100Series",
    "StackingColumnSeries",
    "RangeColumnSeries",
    "CustomWaterfallSeries"
  };

  internal ChartSeries SfBarseries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (ChartSeries) null;
    ObservableCollection<ChartPoint> products = viewModel.Products;
    CartesianSeries cartesianSeries = (products.Count <= 1000 ? ChartSeriesFactory.CreateSeries(typeof (BarSeries)) : ChartSeriesFactory.CreateSeries(typeof (FastBarBitmapSeries))) as CartesianSeries;
    cartesianSeries.LegendIcon = ChartLegendIcon.Rectangle;
    List<int> negativeIndexes;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) cartesianSeries, products, out negativeIndexes);
    this.SetGapWidthandOverlap((ChartSeriesBase) cartesianSeries, serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) cartesianSeries);
    cartesianSeries.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) cartesianSeries) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, cartesianSeries);
    this.ApplyTemplateFormats(serie, (ChartSeries) cartesianSeries, negativeIndexes, borderOnCommonSeries);
    return (ChartSeries) cartesianSeries;
  }

  internal BarSeries3D SfBarseries3D(ChartSerieImpl serie)
  {
    BarSeries3D barSeries3D1 = new BarSeries3D();
    barSeries3D1.XBindingPath = "X";
    barSeries3D1.YBindingPath = "Value";
    BarSeries3D barSeries3D2 = barSeries3D1;
    barSeries3D2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) barSeries3D2);
    this.SetGapWidthandOverlap((ChartSeriesBase) barSeries3D2, serie);
    barSeries3D2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) barSeries3D2) as ChartAdornmentInfo3D;
    return barSeries3D2;
  }

  internal StackingBar100Series SfStackBar100Series(ChartSerieImpl serie)
  {
    StackingBar100Series stackingBar100Series1 = new StackingBar100Series();
    stackingBar100Series1.XBindingPath = "X";
    stackingBar100Series1.YBindingPath = "Value";
    StackingBar100Series stackingBar100Series2 = stackingBar100Series1;
    stackingBar100Series2.LegendIcon = ChartLegendIcon.Rectangle;
    List<int> negativeIndexes;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingBar100Series2, out negativeIndexes);
    this.SetGapWidthandOverlap((ChartSeriesBase) stackingBar100Series2, serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) stackingBar100Series2);
    stackingBar100Series2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingBar100Series2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) stackingBar100Series2);
    this.ApplyTemplateFormats(serie, (ChartSeries) stackingBar100Series2, negativeIndexes, borderOnCommonSeries);
    return stackingBar100Series2;
  }

  internal AreaSeries SfAreaSeries(ChartSerieImpl serie)
  {
    AreaSeries areaSeries1 = new AreaSeries();
    areaSeries1.XBindingPath = "X";
    areaSeries1.YBindingPath = "Value";
    AreaSeries areaSeries2 = areaSeries1;
    areaSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) areaSeries2);
    this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) areaSeries2);
    areaSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) areaSeries2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) areaSeries2);
    return areaSeries2;
  }

  internal AreaSeries3D SfAreaSeries3D(ChartSerieImpl serie)
  {
    AreaSeries3D areaSeries3D = new AreaSeries3D();
    areaSeries3D.XBindingPath = "X";
    areaSeries3D.YBindingPath = "Value";
    AreaSeries3D sfChartSeries = areaSeries3D;
    sfChartSeries.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) sfChartSeries);
    sfChartSeries.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) sfChartSeries) as ChartAdornmentInfo3D;
    return sfChartSeries;
  }

  internal ChartSeries SfColumnSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (ChartSeries) null;
    CartesianSeries cartesianSeries = (viewModel.Products.Count <= 1000 ? ChartSeriesFactory.CreateSeries(typeof (ColumnSeries)) : ChartSeriesFactory.CreateSeries(typeof (FastColumnBitmapSeries))) as CartesianSeries;
    cartesianSeries.LegendIcon = ChartLegendIcon.Rectangle;
    List<int> negativeIndexes;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) cartesianSeries, viewModel.Products, out negativeIndexes);
    this.SetGapWidthandOverlap((ChartSeriesBase) cartesianSeries, serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) cartesianSeries);
    cartesianSeries.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) cartesianSeries) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, cartesianSeries);
    this.ApplyTemplateFormats(serie, (ChartSeries) cartesianSeries, negativeIndexes, borderOnCommonSeries);
    return (ChartSeries) cartesianSeries;
  }

  private void ApplyTemplateFormats(
    ChartSerieImpl serie,
    ChartSeries sfChartSerie,
    List<int> negativeIndexes,
    bool isDefaultBorder)
  {
    if (sfChartSerie.Palette != ChartColorPalette.Custom || !((IEnumerable<string>) this.RectangleSerieTypes).Contains<string>(sfChartSerie.GetType().Name))
      return;
    bool flag1 = sfChartSerie is WaterfallSeries;
    List<System.Windows.Media.Brush> customBrushes = sfChartSerie.ColorModel.CustomBrushes;
    System.Windows.Media.Brush brush1 = customBrushes[0];
    System.Drawing.Color color;
    System.Windows.Media.Brush brush2 = !(serie.SerieFormat as ChartSerieDataFormatImpl).IsAutomaticFormat ? (!this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, serie.Number, serie.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(serie.Number == -1 ? 0 : serie.Number, serie.ParentSeries.Count, false, false))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color))) : brush1;
    ChartDataPointsCollection dataPoints = serie.DataPoints as ChartDataPointsCollection;
    int deninedDpCount = dataPoints.DeninedDPCount;
    bool flag2 = negativeIndexes != null && negativeIndexes.Contains(0);
    int count = serie.Values != null ? serie.Values.Count : (serie.EnteredDirectlyValues != null ? serie.EnteredDirectlyValues.Length : 0);
    if (!flag1 && (flag2 || deninedDpCount > 0 && dataPoints.m_hashDataPoints.ContainsKey(0)))
    {
      brush1 = flag2 ? this.GetCommonColorBrush(customBrushes, negativeIndexes) : customBrushes[0];
      this.ApplyLegendTemplate((ChartSeriesBase) sfChartSerie, brush1);
    }
    Dictionary<int, System.Windows.Media.Brush> fillBrushes = (Dictionary<int, System.Windows.Media.Brush>) null;
    Dictionary<int, System.Windows.Media.Brush> borderBrushes = (Dictionary<int, System.Windows.Media.Brush>) null;
    Dictionary<int, double> thicknessDictionary = (Dictionary<int, double>) null;
    if (deninedDpCount > 0)
    {
      if (!flag1)
        fillBrushes = new Dictionary<int, System.Windows.Media.Brush>(deninedDpCount);
      borderBrushes = new Dictionary<int, System.Windows.Media.Brush>(deninedDpCount);
      thicknessDictionary = new Dictionary<int, double>(deninedDpCount);
      foreach (ChartDataPointImpl chartDataPointImpl in dataPoints.m_hashDataPoints.Values)
      {
        System.Windows.Media.Brush brush3 = (System.Windows.Media.Brush) null;
        if (chartDataPointImpl != null && chartDataPointImpl.DataFormatOrNull != null && (!(chartDataPointImpl.DataFormat as ChartSerieDataFormatImpl).IsAutomaticFormat || chartDataPointImpl.DataFormat.HasLineProperties))
        {
          ChartSerieDataFormatImpl dataFormat = chartDataPointImpl.DataFormat as ChartSerieDataFormatImpl;
          if (!flag1)
          {
            System.Windows.Media.Brush brush4 = negativeIndexes == null || !negativeIndexes.Contains(chartDataPointImpl.Index) || !(dataFormat.Fill as ChartFillImpl).InvertIfNegative ? (!(chartDataPointImpl.DataFormat as ChartSerieDataFormatImpl).IsAutomaticFormat ? this.GetBrushFromDataFormat((IChartFillBorder) dataFormat) : (this.IsChartEx ? brush1 : brush2)) : customBrushes[chartDataPointImpl.Index];
            if (brush4 != null)
              fillBrushes[chartDataPointImpl.Index] = brush4;
          }
          if (dataFormat.LineProperties.LinePattern != ExcelChartLinePattern.None && chartDataPointImpl.DataFormat.HasLineProperties)
            brush3 = !chartDataPointImpl.DataFormat.LineProperties.IsAutoLineColor ? this.GetBrushFromBorder(dataFormat.LineProperties as ChartBorderImpl) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0, this.parentWorkbook.Version != ExcelVersion.Excel97to2003 ? 1.0 : 0.0));
          double borderThickness = this.GetBorderThickness(dataFormat.LineProperties as ChartBorderImpl);
          if (brush3 != null)
            borderBrushes[chartDataPointImpl.Index] = brush3;
          if (borderThickness != 0.0)
            thicknessDictionary[chartDataPointImpl.Index] = borderThickness;
          else if (brush3 != null && chartDataPointImpl.DataFormat.LineProperties.AutoFormat)
            thicknessDictionary[chartDataPointImpl.Index] = 0.0;
        }
      }
    }
    if (fillBrushes != null && fillBrushes.Count > 0)
      this.SetCustomBrushes((ChartSeriesBase) sfChartSerie, count, fillBrushes, brush1, negativeIndexes);
    if (!this.IsChartEx && this.IsVaryColorSupported(serie))
      this.SetVaryColorsOnSeries(sfChartSerie, serie, count, negativeIndexes);
    if (flag1)
    {
      CustomWaterfallSeries customWaterfallSeries = sfChartSerie as CustomWaterfallSeries;
      customWaterfallSeries.BorderBrushes = borderBrushes;
      customWaterfallSeries.ThicknessDictionary = thicknessDictionary;
    }
    else
    {
      if ((borderBrushes == null || borderBrushes.Count <= 0) && (thicknessDictionary == null || thicknessDictionary.Count <= 0) && (negativeIndexes == null || negativeIndexes.Count <= 0 || !isDefaultBorder || !(serie.InvertIfNegativeColor == (ColorObject) Syncfusion.XlsIO.Implementation.ColorExtension.White) && !(serie.InvertIfNegativeColor.GetRGB((IWorkbook) this.parentWorkbook).Name.ToUpper() == "FFFFFF")))
        return;
      this.ApplyShapeTemplate(sfChartSerie, negativeIndexes, isDefaultBorder, borderBrushes, thicknessDictionary);
    }
  }

  private void SetVaryColorsOnSeries(
    ChartSeries sfChartSerie,
    ChartSerieImpl serie,
    int count,
    List<int> negativeIndexes)
  {
    ChartDataPointsCollection dataPoints = serie.DataPoints as ChartDataPointsCollection;
    bool flag = true;
    if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003 && !this.IsChartEx)
      flag = false;
    if (sfChartSerie.Palette != ChartColorPalette.Custom || sfChartSerie.ColorModel.CustomBrushes.Count < 1)
      return;
    for (int index = 0; index < count; ++index)
    {
      ChartDataPointImpl hashDataPoint = dataPoints.m_hashDataPoints.ContainsKey(index) ? dataPoints.m_hashDataPoints[index] : (ChartDataPointImpl) null;
      if ((negativeIndexes != null ? (!negativeIndexes.Contains(index) ? 1 : 0) : 1) != 0 && (hashDataPoint != null ? ((hashDataPoint.DataFormat as ChartSerieDataFormatImpl).IsAutomaticFormat ? 1 : 0) : 1) != 0)
      {
        System.Windows.Media.Brush brush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(index, count, !flag, false)));
        if (sfChartSerie.ColorModel.CustomBrushes.Count > index && sfChartSerie.ColorModel.CustomBrushes.ElementAt<System.Windows.Media.Brush>(index) != null)
          sfChartSerie.ColorModel.CustomBrushes[index] = brush;
        else
          sfChartSerie.ColorModel.CustomBrushes.Add(brush);
      }
    }
  }

  private void SetCustomBrushes(
    ChartSeriesBase sfChartSerie,
    int count,
    Dictionary<int, System.Windows.Media.Brush> fillBrushes,
    System.Windows.Media.Brush commonBrush,
    List<int> negativeIndexes)
  {
    List<System.Windows.Media.Brush> customBrushes = sfChartSerie.ColorModel.CustomBrushes;
    for (int index = 0; index < count; ++index)
    {
      System.Windows.Media.Brush brush = commonBrush;
      if (fillBrushes.ContainsKey(index))
        brush = fillBrushes[index];
      else if (negativeIndexes != null && negativeIndexes.Contains(index))
        continue;
      if (index < customBrushes.Count)
        customBrushes[index] = brush;
      else
        customBrushes.Add(brush);
    }
  }

  private System.Windows.Media.Brush GetCommonColorBrush(
    List<System.Windows.Media.Brush> customBrushes,
    List<int> customDptIndexes)
  {
    System.Windows.Media.Brush customBrush = customBrushes[0];
    if (customBrushes.Count != customDptIndexes.Count)
    {
      for (int index1 = 0; index1 < customDptIndexes.Count; ++index1)
      {
        int index2 = index1 - 1;
        int index3 = index1 + 1;
        if (!customDptIndexes.Contains(index2) && index2 > 0)
        {
          customBrush = customBrushes[index2];
          break;
        }
        if (!customDptIndexes.Contains(index3) && index3 < customBrushes.Count)
        {
          customBrush = customBrushes[index3];
          break;
        }
      }
    }
    return customBrush;
  }

  private void ApplyShapeTemplate(
    ChartSeries sfChartSerie,
    List<int> negativeIndexes,
    bool isDefaultBorder,
    Dictionary<int, System.Windows.Media.Brush> borderBrushes,
    Dictionary<int, double> thicknessDictionary)
  {
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (Canvas));
    FrameworkElementFactory child = new FrameworkElementFactory(typeof (System.Windows.Shapes.Rectangle));
    frameworkElementFactory.AppendChild(child);
    child.SetBinding(Shape.FillProperty, (BindingBase) new Binding("Interior"));
    child.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding("Height"));
    child.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding("Width"));
    child.SetBinding(Canvas.LeftProperty, (BindingBase) new Binding("RectX"));
    child.SetBinding(Canvas.TopProperty, (BindingBase) new Binding("RectY"));
    bool flag = negativeIndexes != null && negativeIndexes.Count > 0 && isDefaultBorder;
    if (borderBrushes != null && borderBrushes.Count > 0 || flag)
    {
      ColorConverter colorConverter = new ColorConverter();
      colorConverter.LabelBrushes = borderBrushes;
      if (colorConverter.LabelBrushes == null)
        colorConverter.LabelBrushes = new Dictionary<int, System.Windows.Media.Brush>(2);
      colorConverter.LabelBrushes[-1] = sfChartSerie.Stroke;
      if (flag)
        colorConverter.LabelBrushes[-2] = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(System.Drawing.Color.Black));
      Binding binding = new Binding()
      {
        Converter = (IValueConverter) colorConverter,
        ConverterParameter = (object) true
      };
      child.SetBinding(Shape.StrokeProperty, (BindingBase) binding);
    }
    if (thicknessDictionary != null && thicknessDictionary.Count > 0)
    {
      Binding binding = new Binding()
      {
        Converter = (IValueConverter) new ThicknessConverter()
        {
          IsSeries = true,
          BorderThicknesses = thicknessDictionary,
          DefaultThickness = new Thickness(sfChartSerie.StrokeThickness)
        }
      };
      child.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) binding);
    }
    else
    {
      if (flag && sfChartSerie.StrokeThickness == 0.0)
        sfChartSerie.StrokeThickness = 0.5;
      child.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding("StrokeThickness"));
    }
    DataTemplate dataTemplate = new DataTemplate((object) typeof (Canvas));
    dataTemplate.VisualTree = frameworkElementFactory;
    this.ApplyDataTemplateonSeries(sfChartSerie, dataTemplate);
  }

  private void ApplyDataTemplateonSeries(ChartSeries sfChartSerie, DataTemplate dataTemplate)
  {
    switch (sfChartSerie.GetType().Name)
    {
      case "ColumnSeries":
        (sfChartSerie as ColumnSeries).CustomTemplate = dataTemplate;
        break;
      case "BarSeries":
        (sfChartSerie as BarSeries).CustomTemplate = dataTemplate;
        break;
      case "StackingBar100Series":
        (sfChartSerie as StackingBar100Series).CustomTemplate = dataTemplate;
        break;
      case "StackingBarSeries":
        (sfChartSerie as StackingBarSeries).CustomTemplate = dataTemplate;
        break;
      case "StackingColumn100Series":
        (sfChartSerie as StackingColumn100Series).CustomTemplate = dataTemplate;
        break;
      case "StackingColumnSeries":
        (sfChartSerie as StackingColumnSeries).CustomTemplate = dataTemplate;
        break;
      case "RangeColumnSeries":
        (sfChartSerie as RangeColumnSeries).CustomTemplate = dataTemplate;
        break;
    }
  }

  private void ApplyLegendTemplate(ChartSeriesBase sfChartSerie, System.Windows.Media.Brush iconBrush)
  {
    DataTemplate dataTemplate = new DataTemplate((object) typeof (System.Windows.Shapes.Rectangle));
    FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (System.Windows.Shapes.Rectangle));
    if (sfChartSerie is LineSeries3D)
      frameworkElementFactory.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding("IconHeight"));
    else
      frameworkElementFactory.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding("IconWidth"));
    frameworkElementFactory.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding("IconHeight"));
    if (sfChartSerie is LineSeries3D || iconBrush == null && sfChartSerie is CircularSeriesBase)
      frameworkElementFactory.SetBinding(Shape.FillProperty, (BindingBase) new Binding("Interior"));
    else
      frameworkElementFactory.SetValue(Shape.FillProperty, (object) iconBrush);
    dataTemplate.VisualTree = frameworkElementFactory;
    sfChartSerie.LegendIconTemplate = dataTemplate;
  }

  internal ChartSeries3D SfColumnSeries3D(ChartSerieImpl serie)
  {
    ColumnSeries3D columnSeries3D1 = new ColumnSeries3D();
    columnSeries3D1.XBindingPath = "X";
    columnSeries3D1.YBindingPath = "Value";
    ColumnSeries3D columnSeries3D2 = columnSeries3D1;
    columnSeries3D2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) columnSeries3D2);
    this.SetGapWidthandOverlap((ChartSeriesBase) columnSeries3D2, serie);
    columnSeries3D2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) columnSeries3D2) as ChartAdornmentInfo3D;
    return (ChartSeries3D) columnSeries3D2;
  }

  internal ChartSeries SfLineSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (ChartSeries) null;
    CartesianSeries cartesianSeries = (viewModel.Products.Count <= 1000 ? ChartSeriesFactory.CreateSeries(typeof (LineSeries)) : ChartSeriesFactory.CreateSeries(typeof (FastLineSeries))) as CartesianSeries;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) cartesianSeries, viewModel.Products, false, out List<int> _);
    this.GetBorderOnLineSeries(serie, (ChartSeries) cartesianSeries);
    cartesianSeries.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) cartesianSeries) as ChartAdornmentInfo;
    DoubleCollection patternValues = (DoubleCollection) null;
    if (this.GetStrokeDashArrayValues((short) serie.SerieFormat.LineProperties.LinePattern, out patternValues))
    {
      switch (cartesianSeries)
      {
        case FastLineSeries _:
          (cartesianSeries as FastLineSeries).StrokeDashArray = patternValues;
          break;
        case LineSeries _:
          FrameworkElementFactory frameworkElementFactory1 = new FrameworkElementFactory(typeof (Line));
          frameworkElementFactory1.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding("Interior"));
          frameworkElementFactory1.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding("StrokeThickness"));
          frameworkElementFactory1.SetBinding(Line.X1Property, (BindingBase) new Binding("X1"));
          frameworkElementFactory1.SetBinding(Line.Y1Property, (BindingBase) new Binding("Y1"));
          frameworkElementFactory1.SetBinding(Line.X2Property, (BindingBase) new Binding("X2"));
          frameworkElementFactory1.SetBinding(Line.Y2Property, (BindingBase) new Binding("Y2"));
          frameworkElementFactory1.SetValue(Shape.StrokeDashArrayProperty, (object) patternValues);
          DataTemplate dataTemplate1 = new DataTemplate((object) typeof (Line));
          dataTemplate1.VisualTree = frameworkElementFactory1;
          (cartesianSeries as LineSeries).CustomTemplate = dataTemplate1;
          FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof (Line));
          frameworkElementFactory2.SetValue(Line.X1Property, (object) 0.0);
          frameworkElementFactory2.SetValue(Line.Y1Property, (object) 8.0);
          frameworkElementFactory2.SetValue(Line.X2Property, (object) 20.0);
          frameworkElementFactory2.SetValue(Line.Y2Property, (object) 8.0);
          frameworkElementFactory2.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding("Interior"));
          frameworkElementFactory2.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding("StrokeThickness"));
          frameworkElementFactory2.SetValue(Shape.StrokeDashArrayProperty, (object) patternValues);
          DataTemplate dataTemplate2 = new DataTemplate((object) typeof (Line));
          dataTemplate2.VisualTree = frameworkElementFactory2;
          (cartesianSeries as LineSeries).LegendIconTemplate = dataTemplate2;
          cartesianSeries.LegendIcon = ChartLegendIcon.SeriesType;
          break;
      }
    }
    else
      cartesianSeries.LegendIcon = ChartLegendIcon.StraightLine;
    this.SfChartTrendLine(serie, cartesianSeries);
    return (ChartSeries) cartesianSeries;
  }

  internal LineSeries3D SfLineSeries3D(ChartSerieImpl serie)
  {
    LineSeries3D lineSeries3D1 = new LineSeries3D();
    lineSeries3D1.XBindingPath = "X";
    lineSeries3D1.YBindingPath = "Value";
    LineSeries3D lineSeries3D2 = lineSeries3D1;
    lineSeries3D2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) lineSeries3D2, true);
    lineSeries3D2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) lineSeries3D2) as ChartAdornmentInfo3D;
    this.ApplyLegendTemplate((ChartSeriesBase) lineSeries3D2, lineSeries3D2.Interior);
    return lineSeries3D2;
  }

  internal SplineSeries SfSplineSeries(ChartSerieImpl serie)
  {
    SplineSeries splineSeries1 = new SplineSeries();
    splineSeries1.XBindingPath = "X";
    splineSeries1.YBindingPath = "Value";
    SplineSeries splineSeries2 = splineSeries1;
    splineSeries2.SplineType = SplineType.Monotonic;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) splineSeries2, false);
    this.GetBorderOnLineSeries(serie, (ChartSeries) splineSeries2);
    splineSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) splineSeries2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) splineSeries2);
    splineSeries2.LegendIcon = ChartLegendIcon.StraightLine;
    return splineSeries2;
  }

  internal PieSeries SfPieSeries(ChartSerieImpl serie)
  {
    PieSeries pieSeries1 = new PieSeries();
    pieSeries1.XBindingPath = "X";
    pieSeries1.YBindingPath = "Value";
    PieSeries pieSeries2 = pieSeries1;
    pieSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) pieSeries2, false);
    int num1 = 0;
    if (serie.ParentChart.ChartType != ExcelChartType.PieOfPie && serie.ParentChart.ChartType != ExcelChartType.Pie_Bar)
      num1 = serie.SerieFormat.CommonSerieOptions.FirstSliceAngle;
    pieSeries2.StartAngle = (double) ((num1 + this.PieAngle) % 360);
    pieSeries2.EndAngle = 360.0 + pieSeries2.StartAngle;
    if (serie.SerieFormat.Percent != 0)
    {
      pieSeries2.ExplodeAll = true;
      double num2 = (double) serie.SerieFormat.Percent;
      if (num2 > 400.0)
        num2 = 400.0;
      if (num2 <= 100.0)
      {
        double num3 = num2 / 2.0;
        pieSeries2.ExplodeRadius = num3;
        double num4 = 1.0 - num3 / 100.0;
        pieSeries2.PieCoefficient = num4 <= 0.8 ? num4 : 0.8;
      }
      else
      {
        double num5 = (num2 - 100.0) / 10.0;
        pieSeries2.ExplodeRadius = 50.0 + num5;
        pieSeries2.PieCoefficient = 0.5 - num5 / 100.0;
      }
    }
    else if (serie.SerieType.ToString().Contains("Pie_Exploded"))
    {
      pieSeries2.ExplodeAll = true;
      pieSeries2.ExplodeRadius = 25.0;
      pieSeries2.PieCoefficient = 0.7;
    }
    System.Windows.Media.Brush borderBrush = (System.Windows.Media.Brush) null;
    double borderThickness = 0.0;
    bool pieDougnutSeries = this.GetFillOnPieDougnutSeries(serie, (ChartSeriesBase) pieSeries2, true, out borderBrush, out borderThickness);
    if (borderBrush != null && borderThickness != 0.0)
    {
      pieSeries2.Stroke = borderBrush;
      pieSeries2.StrokeThickness = borderThickness;
    }
    else
      this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) pieSeries2);
    if (pieSeries2.StrokeThickness == 0.0 && !pieDougnutSeries)
      pieSeries2.StrokeThickness = 0.25;
    pieSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) pieSeries2) as ChartAdornmentInfo;
    if (pieSeries2.Palette == ChartColorPalette.Custom && pieSeries2.Stroke != null && pieSeries2.Stroke.ToString() != "#00000000" && pieSeries2.StrokeThickness != 0.0)
      this.ApplyLegendTemplate((ChartSeriesBase) pieSeries2, (System.Windows.Media.Brush) null);
    return pieSeries2;
  }

  internal PieSeries3D SfPieSeries3D(ChartSerieImpl serie)
  {
    PieSeries3D pieSeries3D1 = new PieSeries3D();
    pieSeries3D1.XBindingPath = "X";
    pieSeries3D1.YBindingPath = "Value";
    pieSeries3D1.CircleCoefficient = 0.78;
    PieSeries3D pieSeries3D2 = pieSeries3D1;
    pieSeries3D2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) pieSeries3D2, false);
    System.Windows.Media.Brush borderBrush = (System.Windows.Media.Brush) null;
    double borderThickness = 0.0;
    this.GetFillOnPieDougnutSeries(serie, (ChartSeriesBase) pieSeries3D2, true, out borderBrush, out borderThickness);
    pieSeries3D2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) pieSeries3D2) as ChartAdornmentInfo3D;
    pieSeries3D2.StartAngle = (double) ((serie.SerieFormat.CommonSerieOptions.FirstSliceAngle + this.PieAngle) % 360);
    pieSeries3D2.EndAngle = 360.0 + pieSeries3D2.StartAngle;
    if (serie.SerieFormat.Percent != 0)
    {
      pieSeries3D2.ExplodeAll = true;
      pieSeries3D2.ExplodeRadius = (double) serie.SerieFormat.Percent;
    }
    else if (serie.SerieType == ExcelChartType.Pie_Exploded || serie.SerieType == ExcelChartType.Pie_Exploded_3D)
    {
      pieSeries3D2.ExplodeAll = true;
      pieSeries3D2.ExplodeRadius = 25.0;
    }
    return pieSeries3D2;
  }

  internal DoughnutSeries SfDoughnutSeries(ChartSerieImpl serie)
  {
    DoughnutSeries doughnutSeries1 = new DoughnutSeries();
    doughnutSeries1.XBindingPath = "X";
    doughnutSeries1.YBindingPath = "Value";
    DoughnutSeries doughnutSeries2 = doughnutSeries1;
    doughnutSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) doughnutSeries2, false);
    doughnutSeries2.StartAngle = (double) ((serie.SerieFormat.CommonSerieOptions.FirstSliceAngle + this.PieAngle) % 360);
    doughnutSeries2.EndAngle = 360.0 + doughnutSeries2.StartAngle;
    if (serie.Equals((object) serie.ParentSeries[serie.ParentSeries.Count - 1]))
    {
      if (serie.SerieFormat.Percent == 0)
        doughnutSeries2.ExplodeRadius = (double) serie.GetCommonSerieFormat().SerieDataFormat.Percent;
      else
        doughnutSeries2.ExplodeRadius = (double) serie.SerieFormat.Percent;
      doughnutSeries2.ExplodeAll = true;
    }
    if (serie.ParentChart.Series.Count == 1)
    {
      double num = (double) serie.SerieFormat.CommonSerieOptions.DoughnutHoleSize / 100.0;
      doughnutSeries2.DoughnutCoefficient = num <= 0.15 ? 0.0 : 1.15 - num;
    }
    System.Windows.Media.Brush borderBrush = (System.Windows.Media.Brush) null;
    double borderThickness = 0.0;
    bool flag = false;
    if (serie.DataPoints != null)
      flag = this.GetFillOnPieDougnutSeries(serie, (ChartSeriesBase) doughnutSeries2, false, out borderBrush, out borderThickness);
    if (borderBrush != null && borderThickness != 0.0)
    {
      doughnutSeries2.Stroke = borderBrush;
      doughnutSeries2.StrokeThickness = borderThickness;
    }
    else
      this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) doughnutSeries2);
    if (doughnutSeries2.StrokeThickness == 0.0 && !flag)
      doughnutSeries2.StrokeThickness = 0.25;
    doughnutSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) doughnutSeries2) as ChartAdornmentInfo;
    doughnutSeries2.Label = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
    if (doughnutSeries2.Palette == ChartColorPalette.Custom && doughnutSeries2.Stroke != null && doughnutSeries2.Stroke.ToString() != "#00000000" && doughnutSeries2.StrokeThickness != 0.0)
      this.ApplyLegendTemplate((ChartSeriesBase) doughnutSeries2, (System.Windows.Media.Brush) null);
    return doughnutSeries2;
  }

  internal StackingAreaSeries SfStackAreaSeries(ChartSerieImpl serie)
  {
    StackingAreaSeries stackingAreaSeries1 = new StackingAreaSeries();
    stackingAreaSeries1.XBindingPath = "X";
    stackingAreaSeries1.YBindingPath = "Value";
    StackingAreaSeries stackingAreaSeries2 = stackingAreaSeries1;
    stackingAreaSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingAreaSeries2);
    this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) stackingAreaSeries2);
    stackingAreaSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingAreaSeries2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) stackingAreaSeries2);
    stackingAreaSeries2.GroupingLabel = "SfStackArea_" + (serie.UsePrimaryAxis ? "1" : "2");
    return stackingAreaSeries2;
  }

  internal StackingArea100Series SfStackArea100Series(ChartSerieImpl serie)
  {
    StackingArea100Series stackingArea100Series1 = new StackingArea100Series();
    stackingArea100Series1.XBindingPath = "X";
    stackingArea100Series1.YBindingPath = "Value";
    StackingArea100Series stackingArea100Series2 = stackingArea100Series1;
    stackingArea100Series2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingArea100Series2);
    this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) stackingArea100Series2);
    stackingArea100Series2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingArea100Series2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) stackingArea100Series2);
    stackingArea100Series2.GroupingLabel = "SfStackArea100_" + (serie.UsePrimaryAxis ? "1" : "2");
    return stackingArea100Series2;
  }

  internal StackingBarSeries SfStackBarSeries(ChartSerieImpl serie)
  {
    StackingBarSeries stackingBarSeries1 = new StackingBarSeries();
    stackingBarSeries1.XBindingPath = "X";
    stackingBarSeries1.YBindingPath = "Value";
    StackingBarSeries stackingBarSeries2 = stackingBarSeries1;
    stackingBarSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    List<int> negativeIndexes;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingBarSeries2, out negativeIndexes);
    this.SetGapWidthandOverlap((ChartSeriesBase) stackingBarSeries2, serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) stackingBarSeries2);
    stackingBarSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingBarSeries2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) stackingBarSeries2);
    this.ApplyTemplateFormats(serie, (ChartSeries) stackingBarSeries2, negativeIndexes, borderOnCommonSeries);
    return stackingBarSeries2;
  }

  internal StackingBarSeries3D SfStackBarSeries3D(ChartSerieImpl serie)
  {
    StackingBarSeries3D stackingBarSeries3D1 = new StackingBarSeries3D();
    stackingBarSeries3D1.XBindingPath = "X";
    stackingBarSeries3D1.YBindingPath = "Value";
    StackingBarSeries3D stackingBarSeries3D2 = stackingBarSeries3D1;
    stackingBarSeries3D2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingBarSeries3D2);
    this.SetGapWidthandOverlap((ChartSeriesBase) stackingBarSeries3D2, serie);
    stackingBarSeries3D2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingBarSeries3D2) as ChartAdornmentInfo3D;
    return stackingBarSeries3D2;
  }

  internal StackingBar100Series3D SfStackBar100Series3D(ChartSerieImpl serie)
  {
    StackingBar100Series3D stackingBar100Series3D1 = new StackingBar100Series3D();
    stackingBar100Series3D1.XBindingPath = "X";
    stackingBar100Series3D1.YBindingPath = "Value";
    StackingBar100Series3D stackingBar100Series3D2 = stackingBar100Series3D1;
    stackingBar100Series3D2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingBar100Series3D2);
    this.SetGapWidthandOverlap((ChartSeriesBase) stackingBar100Series3D2, serie);
    stackingBar100Series3D2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingBar100Series3D2) as ChartAdornmentInfo3D;
    return stackingBar100Series3D2;
  }

  internal ChartSeries SfStackedColumnSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (ChartSeries) null;
    StackingSeriesBase stackingSeriesBase = (viewModel.Products.Count <= 1000 ? ChartSeriesFactory.CreateSeries(typeof (StackingColumnSeries)) : ChartSeriesFactory.CreateSeries(typeof (FastStackingColumnBitmapSeries))) as StackingSeriesBase;
    stackingSeriesBase.LegendIcon = ChartLegendIcon.Rectangle;
    List<int> negativeIndexes;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingSeriesBase, out negativeIndexes);
    this.SetGapWidthandOverlap((ChartSeriesBase) stackingSeriesBase, serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) stackingSeriesBase);
    stackingSeriesBase.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingSeriesBase) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) stackingSeriesBase);
    stackingSeriesBase.GroupingLabel = "SfStackColumn_" + (serie.UsePrimaryAxis ? "1" : "2");
    this.ApplyTemplateFormats(serie, (ChartSeries) stackingSeriesBase, negativeIndexes, borderOnCommonSeries);
    return (ChartSeries) stackingSeriesBase;
  }

  internal StackingColumnSeries3D SfStackedColumnSeries3D(ChartSerieImpl serie)
  {
    StackingColumnSeries3D stackingColumnSeries3D1 = new StackingColumnSeries3D();
    stackingColumnSeries3D1.XBindingPath = "X";
    stackingColumnSeries3D1.YBindingPath = "Value";
    StackingColumnSeries3D stackingColumnSeries3D2 = stackingColumnSeries3D1;
    stackingColumnSeries3D2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingColumnSeries3D2);
    this.SetGapWidthandOverlap((ChartSeriesBase) stackingColumnSeries3D2, serie);
    stackingColumnSeries3D2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingColumnSeries3D2) as ChartAdornmentInfo3D;
    return stackingColumnSeries3D2;
  }

  internal StackingColumn100Series SfStackColum100Series(ChartSerieImpl serie)
  {
    StackingColumn100Series stackingColumn100Series1 = new StackingColumn100Series();
    stackingColumn100Series1.XBindingPath = "X";
    stackingColumn100Series1.YBindingPath = "Value";
    StackingColumn100Series stackingColumn100Series2 = stackingColumn100Series1;
    stackingColumn100Series2.LegendIcon = ChartLegendIcon.Rectangle;
    List<int> negativeIndexes;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) stackingColumn100Series2, out negativeIndexes);
    this.SetGapWidthandOverlap((ChartSeriesBase) stackingColumn100Series2, serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) stackingColumn100Series2);
    stackingColumn100Series2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) stackingColumn100Series2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) stackingColumn100Series2);
    this.ApplyTemplateFormats(serie, (ChartSeries) stackingColumn100Series2, negativeIndexes, borderOnCommonSeries);
    stackingColumn100Series2.GroupingLabel = "SfStackColumn100_" + (serie.UsePrimaryAxis ? "1" : "2");
    return stackingColumn100Series2;
  }

  internal StackingColumn100Series3D SfStackColum100Series3D(ChartSerieImpl serie)
  {
    StackingColumn100Series3D column100Series3D1 = new StackingColumn100Series3D();
    column100Series3D1.XBindingPath = "X";
    column100Series3D1.YBindingPath = "Value";
    StackingColumn100Series3D column100Series3D2 = column100Series3D1;
    column100Series3D2.LegendIcon = ChartLegendIcon.Rectangle;
    this.ChartSeriesCommon(serie, (ChartSeriesBase) column100Series3D2);
    this.SetGapWidthandOverlap((ChartSeriesBase) column100Series3D2, serie);
    column100Series3D2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) column100Series3D2) as ChartAdornmentInfo3D;
    return column100Series3D2;
  }

  internal RadarSeries SfRadarSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (RadarSeries) null;
    ObservableCollection<ChartPoint> products = viewModel.Products;
    RadarSeries radarSeries1 = new RadarSeries();
    radarSeries1.XBindingPath = "X";
    radarSeries1.YBindingPath = "Value";
    RadarSeries radarSeries2 = radarSeries1;
    radarSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    this.SfSecondaryAxis(serie, serie.ParentChart, (ChartSeriesBase) radarSeries2);
    this.TryAnsEmptyPointDisplayInChart(serie, (ChartSeriesBase) radarSeries2);
    ObservableCollection<ChartPoint> values = this.ListofPoints.ContainsKey(serie.Index) ? this.ListofPoints[serie.Index] : this.GetChartPointsValues(serie, radarSeries2.ShowEmptyPoints, radarSeries2.EmptyPointValue == EmptyPointValue.Average ? 1 : 0, false, products);
    ObservableCollection<ChartPoint> observableCollection = this.TryAndRemoveFirstEmptyPointsOnSeries(true, serie, (ChartSeriesBase) radarSeries2, values);
    radarSeries2.ItemsSource = (object) observableCollection;
    ChartBorderImpl lineProperties = serie.SerieFormat.LineProperties as ChartBorderImpl;
    bool isXMLVersion = true;
    if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003)
      isXMLVersion = false;
    System.Windows.Media.Brush brush1 = (System.Windows.Media.Brush) null;
    if (lineProperties.LinePattern != ExcelChartLinePattern.None)
    {
      if (!lineProperties.AutoFormat)
        brush1 = this.GetBrushFromBorder(lineProperties);
      else if (serie.SerieType != ExcelChartType.Radar_Filled)
      {
        System.Drawing.Color color;
        brush1 = !this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, serie.Number, serie.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(serie.Number, serie.ParentSeries.Count, !isXMLVersion, true))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
      }
    }
    switch (serie.SerieType)
    {
      case ExcelChartType.Radar:
      case ExcelChartType.Radar_Markers:
        radarSeries2.DrawType = ChartSeriesDrawType.Line;
        if (!this.TryAndUpdateSegmentsColorsInLineSeries(serie, (ChartSeries) radarSeries2, brush1, isXMLVersion))
        {
          if (brush1 != null)
          {
            radarSeries2.Interior = brush1;
            radarSeries2.Palette = ChartColorPalette.Custom;
          }
          if (radarSeries2.ShowEmptyPoints)
            radarSeries2.EmptyPointInterior = brush1;
        }
        radarSeries2.LegendIcon = ChartLegendIcon.StraightLine;
        break;
      case ExcelChartType.Radar_Filled:
        radarSeries2.DrawType = ChartSeriesDrawType.Area;
        System.Drawing.Color color1;
        System.Windows.Media.Brush brush2 = !(serie.SerieFormat as ChartSerieDataFormatImpl).IsAutomaticFormat || !isXMLVersion ? this.GetBrushFromDataFormat((IChartFillBorder) serie.SerieFormat) : (!this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, serie.Number, serie.ParentSeries.Count - 1, out color1) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(serie.Number, serie.ParentSeries.Count, false, true))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color1)));
        if (brush2 != null)
        {
          radarSeries2.Interior = brush2;
          radarSeries2.Palette = ChartColorPalette.Custom;
        }
        if (brush1 == null && lineProperties.AutoFormat && !isXMLVersion)
        {
          brush1 = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0));
          break;
        }
        break;
    }
    if (brush1 != null)
    {
      radarSeries2.Stroke = brush1;
      radarSeries2.StrokeThickness = this.GetBorderThickness(lineProperties);
      if (radarSeries2.StrokeThickness == 0.0 || radarSeries2.StrokeThickness == 0.75 && (short) lineProperties.LineWeight == (short) -1 && this.parentWorkbook.IsLoaded && !this.parentWorkbook.IsConverted && isXMLVersion)
      {
        double thickness;
        if (radarSeries2.DrawType == ChartSeriesDrawType.Line && this.TryAndGetThicknessBasedOnElement(ChartElementsEnum.DataPointLineThickness, serie.ParentChart, out thickness, new short?()))
          radarSeries2.StrokeThickness = thickness;
        else if (radarSeries2.DrawType == ChartSeriesDrawType.Area && !isXMLVersion)
          radarSeries2.StrokeThickness = 1.0;
        else
          radarSeries2.StrokeThickness = isXMLVersion ? 2.0 : 1.0;
      }
    }
    else
      radarSeries2.StrokeThickness = 0.0;
    radarSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) radarSeries2) as ChartAdornmentInfo;
    radarSeries2.Label = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
    return radarSeries2;
  }

  internal ChartSeries SfScatterrSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (ChartSeries) null;
    ObservableCollection<ChartPoint> products = viewModel.Products;
    CartesianSeries cartesianSeries = (products.Count <= 1000 ? ChartSeriesFactory.CreateSeries(typeof (ScatterSeries)) : ChartSeriesFactory.CreateSeries(typeof (FastScatterBitmapSeries))) as CartesianSeries;
    cartesianSeries.LegendIcon = ChartLegendIcon.Rectangle;
    this.SfSecondaryAxis(serie, serie.ParentChart, (ChartSeriesBase) cartesianSeries);
    this.TryAnsEmptyPointDisplayInChart(serie, (ChartSeriesBase) cartesianSeries);
    ObservableCollection<ChartPoint> observableCollection = this.ListofPoints.ContainsKey(serie.Index) ? this.ListofPoints[serie.Index] : this.GetChartPointsValues(serie, cartesianSeries.ShowEmptyPoints, cartesianSeries.EmptyPointValue == EmptyPointValue.Average ? 1 : 0, false, products);
    cartesianSeries.ItemsSource = (object) observableCollection;
    ChartSerieDataFormatImpl serieFormat = serie.SerieFormat as ChartSerieDataFormatImpl;
    cartesianSeries.LegendIcon = this.GetChartLegendIcon(serieFormat, serie.Index);
    if (cartesianSeries is ScatterSeries)
    {
      (cartesianSeries as ScatterSeries).ScatterHeight = 0.0;
      (cartesianSeries as ScatterSeries).ScatterWidth = 0.0;
    }
    else
    {
      (cartesianSeries as FastScatterBitmapSeries).ScatterHeight = 0.0;
      (cartesianSeries as FastScatterBitmapSeries).ScatterWidth = 0.0;
    }
    cartesianSeries.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) cartesianSeries) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, cartesianSeries);
    cartesianSeries.Label = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
    return (ChartSeries) cartesianSeries;
  }

  internal BubbleSeries SfBubbleSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (BubbleSeries) null;
    ObservableCollection<ChartPoint> products = viewModel.Products;
    BubbleSeries bubbleSeries1 = new BubbleSeries();
    bubbleSeries1.XBindingPath = "X";
    bubbleSeries1.YBindingPath = "Value";
    BubbleSeries bubbleSeries2 = bubbleSeries1;
    bubbleSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    this.SfSecondaryAxis(serie, serie.ParentChart, (ChartSeriesBase) bubbleSeries2);
    this.TryAnsEmptyPointDisplayInChart(serie, (ChartSeriesBase) bubbleSeries2);
    bubbleSeries2.ItemsSource = this.ListofPoints.ContainsKey(serie.Index) ? (object) this.ListofPoints[serie.Index] : (object) this.GetChartPointsValues(serie, bubbleSeries2.ShowEmptyPoints, bubbleSeries2.EmptyPointValue == EmptyPointValue.Average ? 1 : 0, true, products);
    bubbleSeries2.LegendIcon = ChartLegendIcon.Circle;
    bubbleSeries2.Size = "Size";
    List<int> negativeIndexes;
    this.GetFillOnCommonSeries(serie, (ChartSeriesBase) bubbleSeries2, out negativeIndexes);
    if (this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) bubbleSeries2) && negativeIndexes != null && negativeIndexes.Count > 0 && this.parentWorkbook.Version != ExcelVersion.Excel97to2003)
    {
      bubbleSeries2.Stroke = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0, 0.7));
      bubbleSeries2.StrokeThickness = 0.5;
    }
    bubbleSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) bubbleSeries2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) bubbleSeries2);
    bubbleSeries2.ShowZeroBubbles = false;
    bubbleSeries2.Label = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
    return bubbleSeries2;
  }

  internal CandleSeries SfCandleSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (CandleSeries) null;
    ObservableCollection<ChartPoint> products = viewModel.Products;
    CandleSeries candleSeries1 = new CandleSeries();
    candleSeries1.XBindingPath = "X";
    candleSeries1.High = "High";
    candleSeries1.Low = "Low";
    candleSeries1.Open = "Open";
    candleSeries1.Close = "Close";
    CandleSeries candleSeries2 = candleSeries1;
    candleSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    this.SfSecondaryAxis(serie, serie.ParentChart, (ChartSeriesBase) candleSeries2);
    ObservableCollection<ChartPoint> observableCollection = this.ListofPoints.ContainsKey(serie.Index) ? this.ListofPoints[serie.Index] : this.GetChartPointsValuesForStockChart(candleSeries2.ShowEmptyPoints, serie.ParentChart, serie, products, true);
    candleSeries2.ItemsSource = (object) observableCollection;
    candleSeries2.LegendIcon = ChartLegendIcon.None;
    ChartFormatImpl commonSerieOptions = serie.SerieFormat.CommonSerieOptions as ChartFormatImpl;
    ChartDropBarImpl firstDropBar = commonSerieOptions.IsDropBar ? commonSerieOptions.FirstDropBar as ChartDropBarImpl : (ChartDropBarImpl) null;
    ChartDropBarImpl secondDropBar = firstDropBar != null ? commonSerieOptions.SecondDropBar as ChartDropBarImpl : (ChartDropBarImpl) null;
    if (firstDropBar == null)
    {
      IChartSerie chartSerie = serie.ParentSeries.FirstOrDefault<IChartSerie>((Func<IChartSerie, bool>) (x => (x.SerieFormat.CommonSerieOptions as ChartFormatImpl).IsDropBar));
      if (chartSerie != null)
        commonSerieOptions = chartSerie.SerieFormat.CommonSerieOptions as ChartFormatImpl;
      firstDropBar = commonSerieOptions.FirstDropBar as ChartDropBarImpl;
      secondDropBar = commonSerieOptions.SecondDropBar as ChartDropBarImpl;
    }
    System.Drawing.Color color1;
    System.Windows.Media.Brush brush1 = firstDropBar.IsAutomaticFormat ? (!this.TryAndGetColorBasedOnElement(ChartElementsEnum.UpBarFill, serie.ParentChart, out color1) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue)) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color1))) : this.GetBrushFromDataFormat((IChartFillBorder) firstDropBar);
    System.Windows.Media.Brush brush2 = secondDropBar.IsAutomaticFormat ? (!this.TryAndGetColorBasedOnElement(ChartElementsEnum.DownBarFill, serie.ParentChart, out color1) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 51, (byte) 51, (byte) 51)) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color1))) : this.GetBrushFromDataFormat((IChartFillBorder) secondDropBar);
    if (brush1 != null)
      candleSeries2.BullFillColor = brush1;
    if (brush2 != null)
      candleSeries2.BearFillColor = brush2;
    ChartBorderImpl border = (!commonSerieOptions.HasHighLowLines || commonSerieOptions.HighLowLines == null ? (ChartBorderImpl) null : commonSerieOptions.HighLowLines as ChartBorderImpl) ?? secondDropBar.LineProperties as ChartBorderImpl;
    double num = 0.0;
    if (border.LinePattern != ExcelChartLinePattern.None)
    {
      System.Drawing.Color color2;
      System.Windows.Media.Brush brush3 = !border.AutoFormat || !this.TryAndGetColorBasedOnElement(ChartElementsEnum.OtherLines, serie.ParentChart, out color2) ? this.GetBrushFromBorder(border) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color2));
      if (brush3 != null)
        candleSeries2.Stroke = brush3;
      double borderThickness = this.GetBorderThickness(border);
      num = borderThickness < 1.0 ? 1.0 : borderThickness;
    }
    candleSeries2.StrokeThickness = num;
    candleSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) candleSeries2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) candleSeries2);
    candleSeries2.Label = "Open  High  Low  Close";
    return candleSeries2;
  }

  internal HiLoSeries SfStockHiLoSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    if (viewModel == null)
      return (HiLoSeries) null;
    ObservableCollection<ChartPoint> products = viewModel.Products;
    HiLoSeries hiLoSeries1 = new HiLoSeries();
    hiLoSeries1.XBindingPath = "X";
    hiLoSeries1.High = "High";
    hiLoSeries1.Low = "Low";
    HiLoSeries hiLoSeries2 = hiLoSeries1;
    hiLoSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    this.SfSecondaryAxis(serie, serie.ParentChart, (ChartSeriesBase) hiLoSeries2);
    ObservableCollection<ChartPoint> observableCollection = this.ListofPoints.ContainsKey(serie.Index) ? this.ListofPoints[serie.Index] : this.GetChartPointsValuesForStockChart(hiLoSeries2.ShowEmptyPoints, serie.ParentChart, serie, products, false);
    hiLoSeries2.ItemsSource = (object) observableCollection;
    ChartFormatImpl commonSerieOptions = serie.SerieFormat.CommonSerieOptions as ChartFormatImpl;
    if (commonSerieOptions.HasHighLowLines && commonSerieOptions.HighLowLines != null)
    {
      if (commonSerieOptions.HighLowLines.LinePattern != ExcelChartLinePattern.None)
      {
        ChartBorderImpl highLowLines = commonSerieOptions.HighLowLines as ChartBorderImpl;
        System.Drawing.Color color;
        System.Windows.Media.Brush brush = !highLowLines.AutoFormat || !this.TryAndGetColorBasedOnElement(ChartElementsEnum.OtherLines, serie.ParentChart, out color) ? this.GetBrushFromBorder(highLowLines) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
        if (brush != null)
          hiLoSeries2.Interior = brush;
        else
          hiLoSeries2.Interior = (System.Windows.Media.Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 0, (byte) 0, (byte) 0));
        double borderThickness = this.GetBorderThickness(highLowLines);
        hiLoSeries2.StrokeThickness = borderThickness < 1.0 ? 1.0 : borderThickness;
      }
      else
        hiLoSeries2.StrokeThickness = 0.0;
    }
    else
    {
      hiLoSeries2.Interior = (System.Windows.Media.Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 0, (byte) 0, (byte) 0));
      hiLoSeries2.StrokeThickness = 1.0;
    }
    hiLoSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) hiLoSeries2) as ChartAdornmentInfo;
    this.SfChartTrendLine(serie, (CartesianSeries) hiLoSeries2);
    for (int index = 0; index < serie.ParentSeries.Count; ++index)
    {
      ChartSerieImpl serie1 = serie.ParentSeries[index] as ChartSerieImpl;
      HiLoSeries hiLoSeries3 = hiLoSeries2;
      hiLoSeries3.Label = $"{hiLoSeries3.Label}{(this.Names.ContainsKey(serie1.Index) ? this.Names[serie1.Index] : this.GetSerieName(serie1))} ";
    }
    return hiLoSeries2;
  }

  internal RangeColumnSeries SfFunnelSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    RangeColumnSeries rangeColumnSeries1 = new RangeColumnSeries();
    rangeColumnSeries1.XBindingPath = "X";
    rangeColumnSeries1.High = "High";
    rangeColumnSeries1.Low = "Low";
    rangeColumnSeries1.IsTransposed = true;
    RangeColumnSeries rangeColumnSeries2 = rangeColumnSeries1;
    rangeColumnSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    viewModel.Products = this.ListofPoints.ContainsKey(serie.Index) ? this.ListofPoints[serie.Index] : this.GetChartPointsValues(serie, false, -1, false, viewModel.Products);
    IList<ChartPoint> rangeColumn = this.ChangeFunnelItemsSourceToRangeColumn(viewModel.Products);
    rangeColumnSeries2.ItemsSource = (object) rangeColumn;
    this.GetFillOnCommonSeries(serie, (ChartSeriesBase) rangeColumnSeries2, out List<int> _);
    rangeColumnSeries2.Label = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
    this.SetGapWidthandOverlap((ChartSeriesBase) rangeColumnSeries2, serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) rangeColumnSeries2);
    rangeColumnSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) rangeColumnSeries2) as ChartAdornmentInfo;
    List<int> negativeIndexes = (List<int>) null;
    this.ApplyTemplateFormats(serie, (ChartSeries) rangeColumnSeries2, negativeIndexes, borderOnCommonSeries);
    return rangeColumnSeries2;
  }

  internal CustomWaterfallSeries SfWaterfallSeries(ChartSerieImpl serie)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    CustomWaterfallSeries customWaterfallSeries1 = new CustomWaterfallSeries();
    customWaterfallSeries1.XBindingPath = "X";
    customWaterfallSeries1.SummaryBindingPath = "IsSummary";
    customWaterfallSeries1.YBindingPath = "Value";
    customWaterfallSeries1.AllowAutoSum = false;
    CustomWaterfallSeries customWaterfallSeries2 = customWaterfallSeries1;
    customWaterfallSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    viewModel.Products = this.ListofPoints.ContainsKey(serie.Index) ? this.ListofPoints[serie.Index] : this.GetChartPointsValues(serie, false, -1, false, viewModel.Products);
    ChartDataPointsCollection dataPoints = serie.DataPoints as ChartDataPointsCollection;
    if (dataPoints.DeninedDPCount > 0)
    {
      for (int index = 0; index < viewModel.Products.Count; ++index)
      {
        ChartDataPointImpl hashDataPoint = dataPoints.m_hashDataPoints.ContainsKey(index) ? dataPoints.m_hashDataPoints[index] : (ChartDataPointImpl) null;
        if (hashDataPoint != null)
          viewModel.Products[index].IsSummary = hashDataPoint.SetAsTotal;
      }
    }
    customWaterfallSeries2.ItemsSource = (object) viewModel.Products;
    customWaterfallSeries2.ShowConnector = serie.SerieFormat.ShowConnectorLines;
    this.GetFillOnWaterFallSeries(serie, customWaterfallSeries2, viewModel);
    customWaterfallSeries2.Label = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
    this.SetGapWidthandOverlap((ChartSeriesBase) customWaterfallSeries2, serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) customWaterfallSeries2);
    if (customWaterfallSeries2.ShowConnector)
    {
      Style style = new Style()
      {
        TargetType = typeof (Line)
      };
      Setter setter1 = new Setter()
      {
        Property = Shape.StrokeProperty
      };
      Setter setter2 = new Setter()
      {
        Property = Shape.StrokeThicknessProperty
      };
      if (borderOnCommonSeries)
      {
        setter1.Value = (object) new SolidColorBrush(this.SfColor((byte) 190, (byte) 190, (byte) 190));
        double num = 1.0;
        setter2.Value = (object) num;
      }
      else
      {
        setter1.Value = (object) customWaterfallSeries2.Stroke;
        double strokeThickness = customWaterfallSeries2.StrokeThickness;
        setter2.Value = (object) strokeThickness;
      }
      style.Setters.Add((SetterBase) setter1);
      style.Setters.Add((SetterBase) setter2);
      customWaterfallSeries2.ConnectorLineStyle = style;
    }
    customWaterfallSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) customWaterfallSeries2) as ChartAdornmentInfo;
    this.ApplyTemplateFormats(serie, (ChartSeries) customWaterfallSeries2, (List<int>) null, borderOnCommonSeries);
    return customWaterfallSeries2;
  }

  private void GetFillOnWaterFallSeries(
    ChartSerieImpl serie,
    CustomWaterfallSeries waterfall,
    ViewModel model)
  {
    ChartColorModel chartColorModel = (ChartColorModel) null;
    ChartDataPointsCollection dataPoints = serie.DataPoints as ChartDataPointsCollection;
    int deninedDpCount = dataPoints.DeninedDPCount;
    if (deninedDpCount > 0)
    {
      waterfall.Palette = ChartColorPalette.Custom;
      chartColorModel = new ChartColorModel(ChartColorPalette.Custom);
      chartColorModel.CustomBrushes = new List<System.Windows.Media.Brush>((waterfall.ItemsSource as ObservableCollection<ChartPoint>).Count);
      waterfall.ColorModel = chartColorModel;
    }
    if ((serie.SerieFormat as ChartSerieDataFormatImpl).IsAutomaticFormat)
    {
      System.Windows.Media.Brush brush1 = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 91, (byte) 155, (byte) 213));
      System.Windows.Media.Brush brush2 = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 237, (byte) 125, (byte) 49));
      System.Windows.Media.Brush brush3 = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 165, (byte) 165, (byte) 165));
      if (deninedDpCount > 0)
      {
        for (int index = 0; index < chartColorModel.CustomBrushes.Capacity; ++index)
        {
          ChartDataPointImpl hashDataPoint = dataPoints.m_hashDataPoints.ContainsKey(index) ? dataPoints.m_hashDataPoints[index] : (ChartDataPointImpl) null;
          if (hashDataPoint != null && !(hashDataPoint.DataFormat as ChartSerieDataFormatImpl).IsAutomaticFormat)
            chartColorModel.CustomBrushes.Add(this.GetBrushFromDataFormat((IChartFillBorder) hashDataPoint.DataFormat));
          else if (hashDataPoint != null && hashDataPoint.SetAsTotal)
            chartColorModel.CustomBrushes.Add(brush3);
          else if (model.Products[index].Value < 0.0)
            chartColorModel.CustomBrushes.Add(brush2);
          else
            chartColorModel.CustomBrushes.Add(brush1);
        }
      }
      else
      {
        waterfall.Interior = brush1;
        waterfall.SummarySegmentBrush = brush3;
        waterfall.NegativeSegmentBrush = brush2;
      }
      waterfall.DefaultInterior = brush1;
      waterfall.DefaultNegativeBrush = brush2;
      waterfall.DefaultSummaryBrush = brush3;
    }
    else
    {
      System.Windows.Media.Brush brushFromDataFormat = this.GetBrushFromDataFormat((IChartFillBorder) serie.SerieFormat);
      if (deninedDpCount > 0)
      {
        for (int key = 0; key < chartColorModel.CustomBrushes.Capacity; ++key)
        {
          ChartDataPointImpl hashDataPoint = dataPoints.m_hashDataPoints.ContainsKey(key) ? dataPoints.m_hashDataPoints[key] : (ChartDataPointImpl) null;
          if (hashDataPoint != null && !(hashDataPoint.DataFormat as ChartSerieDataFormatImpl).IsAutomaticFormat)
            chartColorModel.CustomBrushes.Add(this.GetBrushFromDataFormat((IChartFillBorder) hashDataPoint.DataFormat));
          else
            chartColorModel.CustomBrushes.Add(brushFromDataFormat);
        }
      }
      else
      {
        waterfall.Interior = brushFromDataFormat;
        waterfall.SummarySegmentBrush = (System.Windows.Media.Brush) null;
        waterfall.NegativeSegmentBrush = (System.Windows.Media.Brush) null;
      }
      waterfall.DefaultInterior = brushFromDataFormat;
      waterfall.DefaultNegativeBrush = (System.Windows.Media.Brush) null;
      waterfall.DefaultSummaryBrush = (System.Windows.Media.Brush) null;
    }
  }

  internal ColumnSeries SfHistogramOrPareto(
    ChartSerieImpl serie,
    ChartImpl _chart,
    out LineSeries paretoLine)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    ColumnSeries columnSeries1 = new ColumnSeries();
    columnSeries1.XBindingPath = "X";
    columnSeries1.YBindingPath = "Value";
    ColumnSeries columnSeries2 = columnSeries1;
    columnSeries2.LegendIcon = ChartLegendIcon.Rectangle;
    viewModel.Products = this.GetChartPointsForHistogram(serie, viewModel.Products);
    columnSeries2.ItemsSource = (object) viewModel.Products;
    this.SetGapWidthandOverlap((ChartSeriesBase) columnSeries2, serie);
    this.GetFillOnCommonSeries(serie, (ChartSeriesBase) columnSeries2, out List<int> _);
    columnSeries2.Label = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
    bool borderOnCommonSeries = this.GetBorderOnCommonSeries(serie.SerieFormat.LineProperties as ChartBorderImpl, (ChartSeries) columnSeries2);
    columnSeries2.AdornmentsInfo = this.SfChartDataLabel(serie, (ChartSeriesBase) columnSeries2) as ChartAdornmentInfo;
    this.ApplyTemplateFormats(serie, (ChartSeries) columnSeries2, (List<int>) null, borderOnCommonSeries);
    paretoLine = serie.ParetoLineFormat == null || (columnSeries2.ItemsSource as ObservableCollection<ChartPoint>).Count <= 1 || serie.IsParetoLineHidden ? (LineSeries) null : this.SfParetoLine(serie, columnSeries2, viewModel.Products);
    return columnSeries2;
  }

  private LineSeries SfParetoLine(
    ChartSerieImpl serie,
    ColumnSeries column,
    ObservableCollection<ChartPoint> observableCollection)
  {
    ViewModel viewModel = new ViewModel(observableCollection.Count);
    LineSeries lineSeries1 = new LineSeries();
    lineSeries1.XBindingPath = "X";
    lineSeries1.YBindingPath = "Value";
    lineSeries1.VisibilityOnLegend = Visibility.Collapsed;
    LineSeries lineSeries2 = lineSeries1;
    double num1 = observableCollection.Sum<ChartPoint>((Func<ChartPoint, double>) (x => x.Value));
    double num2 = 0.0;
    for (int index = 0; index < observableCollection.Count; ++index)
    {
      viewModel.Products[index].X = observableCollection[index].X;
      viewModel.Products[index].Value = (num2 + observableCollection[index].Value) / num1;
      num2 += observableCollection[index].Value;
    }
    lineSeries2.ItemsSource = (object) viewModel.Products;
    ChartBorderImpl lineProperties = serie.ParetoLineFormat.LineProperties as ChartBorderImpl;
    if (lineProperties.LinePattern != ExcelChartLinePattern.None)
    {
      System.Windows.Media.Brush brush = lineProperties.IsAutoLineColor ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(serie.ParetoLineFormatIndex == -1 ? 1 : serie.ParetoLineFormatIndex, serie.ParentSeries.Count, false, true))) : this.GetBrushFromBorder(lineProperties);
      if (brush != null)
      {
        lineSeries2.Palette = ChartColorPalette.Custom;
        lineSeries2.ColorModel = new ChartColorModel(ChartColorPalette.Custom)
        {
          CustomBrushes = new List<System.Windows.Media.Brush>() { brush }
        };
      }
      lineSeries2.StrokeThickness = this.GetBorderThickness(lineProperties);
      if (lineSeries2.StrokeThickness < 1.0)
        lineSeries2.StrokeThickness = 2.25;
    }
    else
      lineSeries2.StrokeThickness = 0.0;
    ChartAxisImpl secondaryValueAxis = serie.ParentChart.SecondaryValueAxis as ChartAxisImpl;
    if (!secondaryValueAxis.Deleted)
    {
      LineSeries lineSeries3 = lineSeries2;
      CustomNumericalAxis customNumericalAxis1 = new CustomNumericalAxis();
      customNumericalAxis1.ShowGridLines = false;
      customNumericalAxis1.RangePadding = NumericalPadding.Normal;
      customNumericalAxis1.PlotOffset = 2.0;
      customNumericalAxis1.OpposedPosition = true;
      customNumericalAxis1.Interval = new double?(0.1);
      customNumericalAxis1.Maximum = new double?(1.0);
      CustomNumericalAxis customNumericalAxis2 = customNumericalAxis1;
      lineSeries3.YAxis = (RangeAxisBase) customNumericalAxis2;
      this.SfSecondaryAxisCommon((IChart) serie.ParentChart, (ChartAxisBase2D) null, lineSeries2.YAxis);
      if (secondaryValueAxis.HasAxisTitle)
      {
        RectangleF manualRect = new RectangleF(-1f, -1f, -1f, -1f);
        bool isVertical = false;
        lineSeries2.YAxis.Header = (object) this.GetSfAxisTitle(secondaryValueAxis, out manualRect, out isVertical);
        if (lineSeries2.YAxis.Header != null && lineSeries2.YAxis.Header is Border && (lineSeries2.YAxis.Header as Border).Child is TextBlock child)
          child.LayoutTransform = (Transform) new RotateTransform(-180.0);
      }
      lineSeries2.YAxis.OpposedPosition = true;
    }
    DoubleCollection patternValues = (DoubleCollection) null;
    if (this.GetStrokeDashArrayValues((short) serie.SerieFormat.LineProperties.LinePattern, out patternValues))
    {
      FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof (Line));
      frameworkElementFactory.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding("Interior"));
      frameworkElementFactory.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding("StrokeThickness"));
      frameworkElementFactory.SetBinding(Line.X1Property, (BindingBase) new Binding("X1"));
      frameworkElementFactory.SetBinding(Line.Y1Property, (BindingBase) new Binding("Y1"));
      frameworkElementFactory.SetBinding(Line.X2Property, (BindingBase) new Binding("X2"));
      frameworkElementFactory.SetBinding(Line.Y2Property, (BindingBase) new Binding("Y2"));
      frameworkElementFactory.SetValue(Shape.StrokeDashArrayProperty, (object) patternValues);
      DataTemplate dataTemplate = new DataTemplate((object) typeof (Line));
      dataTemplate.VisualTree = frameworkElementFactory;
      lineSeries2.CustomTemplate = dataTemplate;
    }
    lineSeries2.Label = "pareto";
    return lineSeries2;
  }

  private ObservableCollection<ChartPoint> GetChartPointsForHistogram(
    ChartSerieImpl serie,
    ObservableCollection<ChartPoint> observableCollection)
  {
    ObservableCollection<ChartPoint> source = new ObservableCollection<ChartPoint>();
    observableCollection = this.ListofPoints.ContainsKey(serie.Index) ? this.ListofPoints[serie.Index] : this.GetChartPointsValues(serie, false, -1, false, observableCollection);
    double binwidth = 0.0;
    double overflowBin = 0.0;
    double underflowBin = 0.0;
    HistogramAxisFormat axisFormat = (serie.SerieFormat as ChartSerieDataFormatImpl).HistogramAxisFormatProperty == null ? (serie.ParentChart.PrimaryCategoryAxis as ChartCategoryAxisImpl).HistogramAxisFormatProperty : (serie.SerieFormat as ChartSerieDataFormatImpl).HistogramAxisFormatProperty;
    if (axisFormat != null && axisFormat.IsBinningByCategory)
    {
      for (int i = 0; i < observableCollection.Count; ++i)
      {
        ChartPoint chartPoint = source.FirstOrDefault<ChartPoint>((Func<ChartPoint, bool>) (x => x.X == observableCollection[i].X));
        if (chartPoint == null)
          source.Add(new ChartPoint()
          {
            X = observableCollection[i].X,
            Value = observableCollection[i].Value
          });
        else
          chartPoint.Value += observableCollection[i].Value;
      }
    }
    else
    {
      double min = observableCollection.Min<ChartPoint>((Func<ChartPoint, double>) (x => x.Value));
      double max = observableCollection.Max<ChartPoint>((Func<ChartPoint, double>) (x => x.Value));
      bool flag = false;
      if (axisFormat == null || axisFormat.HasAutomaticBins || axisFormat.NumberOfBins <= 0 && axisFormat.BinWidth <= 0.0)
      {
        double standardDeviationValue = this.GetStandardDeviationValue(observableCollection);
        binwidth = this.GetAutomaticBinwidthValue(standardDeviationValue, min, max, (double) observableCollection.Count);
        double average = this.GetAverage(observableCollection);
        if (axisFormat != null)
        {
          if (!axisFormat.IsNotAutomaticOverFlowValue)
            overflowBin = Math.Round(average + 3.0 * standardDeviationValue);
          if (!axisFormat.IsNotAutomaticUnderFlowValue)
            underflowBin = Math.Round(average - 3.0 * standardDeviationValue);
        }
      }
      else if (axisFormat.NumberOfBins > 0)
      {
        binwidth = (max - min) / (double) axisFormat.NumberOfBins;
        flag = true;
      }
      else if (axisFormat.BinWidth > 0.0)
        binwidth = axisFormat.BinWidth;
      if (axisFormat != null && axisFormat.IsNotAutomaticOverFlowValue)
        overflowBin = axisFormat.OverflowBinValue;
      if (axisFormat != null && axisFormat.IsNotAutomaticUnderFlowValue)
        underflowBin = axisFormat.UnderflowBinValue;
      source = this.GetChartPointsByBinValues(observableCollection, binwidth, flag ? (axisFormat != null ? axisFormat.NumberOfBins : 0) : 0, min, max, overflowBin, underflowBin, axisFormat);
    }
    if (serie.SerieType == ExcelChartType.Pareto)
      source = new ObservableCollection<ChartPoint>((IEnumerable<ChartPoint>) source.OrderByDescending<ChartPoint, double>((Func<ChartPoint, double>) (x => x.Value)));
    return source;
  }

  private ObservableCollection<ChartPoint> GetChartPointsByBinValues(
    ObservableCollection<ChartPoint> modifiedProducts,
    double binwidth,
    int numberOfBins,
    double min,
    double max,
    double overflowBin,
    double underflowBin,
    HistogramAxisFormat axisFormat)
  {
    bool flag1 = axisFormat != null && axisFormat.IsNotAutomaticUnderFlowValue;
    bool flag2 = axisFormat != null && axisFormat.IsNotAutomaticOverFlowValue;
    ViewModel viewModel;
    if (flag2 && flag1 && min < underflowBin && underflowBin <= max && min <= overflowBin && overflowBin < max && overflowBin >= underflowBin)
    {
      min = underflowBin;
      max = overflowBin;
      int count = numberOfBins != 0 ? numberOfBins : (min == max ? 3 : (binwidth <= max - min ? (int) Math.Ceiling((max - min) / binwidth) + 2 : 3));
      double binWidth = (max - min) / (double) (count - 2);
      viewModel = this.CalculateValuesFromFlowBins(modifiedProducts, min, max, 0, count, binWidth, axisFormat != null && axisFormat.IsIntervalClosedinLeft);
    }
    else if (flag1 && min < underflowBin && underflowBin <= max)
    {
      min = underflowBin;
      int count = numberOfBins != 0 ? numberOfBins : (int) Math.Ceiling((max - min) / binwidth) + 1;
      viewModel = this.CalculateValuesFromFlowBins(modifiedProducts, min, max, -1, count, binwidth, axisFormat != null && axisFormat.IsIntervalClosedinLeft);
    }
    else if (flag2 && min <= overflowBin && overflowBin < max)
    {
      max = overflowBin;
      int count = numberOfBins != 0 ? numberOfBins : (int) Math.Ceiling((max - min) / binwidth) + 1;
      viewModel = this.CalculateValuesFromFlowBins(modifiedProducts, min, max, 1, count, binwidth, axisFormat != null && axisFormat.IsIntervalClosedinLeft);
    }
    else
    {
      int count = binwidth == 0.0 ? 1 : (numberOfBins != 0 ? numberOfBins : (int) ((max - min) / binwidth) + 1);
      viewModel = new ViewModel(count);
      double start = min;
      for (int index = 0; index < count; ++index)
      {
        if (axisFormat != null && axisFormat.IsIntervalClosedinLeft)
          viewModel.Products[index].X = (object) $"[{start.ToString()},{(start + binwidth).ToString()}{(index == count - 1 ? "]" : ")")}";
        else
          viewModel.Products[index].X = (object) $"{(index == 0 ? "[" : "(")}{start.ToString()},{(start + binwidth).ToString()}]";
        if (min == max && count > 1)
          viewModel.Products[index].Value = index == 0 ? (double) modifiedProducts.Count : 0.0;
        else if (axisFormat == null || !axisFormat.IsIntervalClosedinLeft)
        {
          if (start == min)
            viewModel.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= start && x.Value <= start + binwidth));
          else if (start + binwidth != max)
            viewModel.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value > start && x.Value <= start + binwidth));
          else
            viewModel.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value > start && x.Value <= max));
        }
        else if (start == min)
          viewModel.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= start && x.Value < start + binwidth));
        else if (start + binwidth != max)
          viewModel.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= start && x.Value < start + binwidth));
        else
          viewModel.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= start && x.Value <= max));
        start += binwidth;
      }
    }
    return viewModel.Products;
  }

  private ViewModel CalculateValuesFromFlowBins(
    ObservableCollection<ChartPoint> modifiedProducts,
    double min,
    double max,
    int bintype,
    int count,
    double binWidth,
    bool isIntervalClosedInLeft)
  {
    string str1 = char.ConvertFromUtf32(8805).ToString();
    string str2 = char.ConvertFromUtf32(8804).ToString();
    ViewModel valuesFromFlowBins = new ViewModel(count);
    double current = min;
    double previous = min;
    bool flag = false;
    for (int index = 0; index < count; ++index)
    {
      if (index == 0 && bintype <= 0)
      {
        valuesFromFlowBins.Products[index].X = (object) ((isIntervalClosedInLeft ? "<" : str2) + min.ToString());
        valuesFromFlowBins.Products[index].Value = isIntervalClosedInLeft ? (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value < min)) : (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value <= min));
      }
      else if (index == count - 1 && bintype >= 0)
      {
        valuesFromFlowBins.Products[index].X = (object) ((isIntervalClosedInLeft ? str1 : ">") + max.ToString());
        valuesFromFlowBins.Products[index].Value = isIntervalClosedInLeft ? (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= max)) : (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value > max));
      }
      else
      {
        current += binWidth;
        if (current >= max && bintype >= 0)
          current = max;
        valuesFromFlowBins.Products[index].X = (object) $"{(isIntervalClosedInLeft || index == 0 ? "[" : "(")}{previous.ToString()},{current.ToString()}{(!isIntervalClosedInLeft || index == count - 1 ? "]" : ")")}";
        if (flag)
          valuesFromFlowBins.Products[index].Value = 0.0;
        else if (!isIntervalClosedInLeft)
        {
          if (index == 0)
            valuesFromFlowBins.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= previous && x.Value <= current));
          else if (index < count - 1)
            valuesFromFlowBins.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value > previous && x.Value <= current));
          else
            valuesFromFlowBins.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value > previous && x.Value <= max));
        }
        else if (index == 0)
          valuesFromFlowBins.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= previous && x.Value < current));
        else if (index < count - 1)
          valuesFromFlowBins.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= previous && x.Value < current));
        else
          valuesFromFlowBins.Products[index].Value = (double) modifiedProducts.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.Value >= previous && x.Value <= max));
        if (current == max)
          flag = true;
        previous += binWidth;
      }
    }
    return valuesFromFlowBins;
  }

  private IList<ChartPoint> ChangeFunnelItemsSourceToRangeColumn(
    ObservableCollection<ChartPoint> data)
  {
    double num1 = data.Max<ChartPoint>((Func<ChartPoint, double>) (point => point.Value));
    if (num1 <= 0.0)
      num1 = 1.0;
    ObservableCollection<ChartPoint> rangeColumn = new ObservableCollection<ChartPoint>();
    for (int index = 0; index < data.Count; ++index)
    {
      double num2 = data[index].Value < 0.0 ? 0.0 : data[index].Value;
      double num3 = num2 / num1;
      rangeColumn.Add(new ChartPoint()
      {
        X = data[index].X,
        Low = (object) (0.5 - num3 / 2.0),
        High = (object) (0.5 + num3 / 2.0),
        Value = num2
      });
    }
    return (IList<ChartPoint>) rangeColumn;
  }

  private double GetStandardDeviationValue(ObservableCollection<ChartPoint> array)
  {
    int count = array.Count;
    double num1 = 0.0;
    for (int index = 0; index < count; ++index)
      num1 += array[index].Value;
    double num2 = num1 / (double) count;
    double num3 = 0.0;
    for (int index = 0; index < count; ++index)
    {
      double num4 = array[index].Value - num2;
      num3 += num4 * num4;
    }
    return count == 1 ? 0.0 : Math.Sqrt(num3 / (double) (count - 1));
  }

  private double GetAutomaticBinwidthValue(double stdev, double min, double max, double length)
  {
    double num1 = 3.5 * stdev / Math.Pow(length, 0.333);
    double automaticBinwidthValue;
    if (num1 < 1.0)
    {
      double num2 = Math.Ceiling((max - min) / num1);
      int length1 = Math.Truncate(10.0 / ((max - min) / num2)).ToString().Length;
      automaticBinwidthValue = Math.Round(num1, length1);
    }
    else
      automaticBinwidthValue = num1 >= 10.0 || Math.Truncate(num1) < 0.1 ? Math.Round(num1) : Math.Round(num1, 1);
    return automaticBinwidthValue;
  }

  private double GetAverage(ObservableCollection<ChartPoint> array)
  {
    int count = array.Count;
    double num = 0.0;
    for (int index = 0; index < count; ++index)
      num += array[index].Value;
    return num / (double) count;
  }

  private ViewModel GetViewModel(ChartSerieImpl serie)
  {
    ViewModel viewModel = (ViewModel) null;
    if (serie.Values != null && serie.FilteredValue == null)
      viewModel = new ViewModel(serie.Values.Count);
    else if (serie.EnteredDirectlyValues != null)
      viewModel = new ViewModel(serie.EnteredDirectlyValues.Length);
    if (!this.CheckIfValidValueRange(serie.Values) && serie.EnteredDirectlyValues != null)
      viewModel = new ViewModel(serie.EnteredDirectlyValues.Length);
    return viewModel;
  }

  private ObservableCollection<ChartPoint> GetChartPointsValues(
    ChartSerieImpl serie,
    bool showEmptyPoints,
    int emptyPointValue,
    bool isBubbles,
    ObservableCollection<ChartPoint> values)
  {
    ObservableCollection<ChartPoint> chartPoints = values;
    ChartImpl parentChart = serie.ParentChart;
    string startSerieType = ChartFormatImpl.GetStartSerieType(serie.SerieType);
    bool isBubbleOrScatter = startSerieType == "Bubble" || startSerieType == "Scatter";
    bool histogramOrPareto = parentChart.IsHistogramOrPareto;
    bool flag1 = false;
    if (isBubbles)
      flag1 = serie.SerieFormat.CommonSerieOptions.ShowNegativeBubbles;
    List<int> intList = (List<int>) null;
    List<int> categoryIndexesArray = (List<int>) null;
    object[] directCategories = (object[]) null;
    IRange values1 = serie.FilteredValue == null ? serie.Values : (IRange) null;
    IRange range1 = (IRange) null;
    if (!this.IsChartEx && !isBubbleOrScatter)
    {
      IEnumerable<IChartSerie> source = serie.ParentChart.Series.Where<IChartSerie>((Func<IChartSerie, bool>) (item => !item.IsFiltered && item.UsePrimaryAxis == serie.UsePrimaryAxis));
      if (source != null && source.Count<IChartSerie>() > 0 && source.FirstOrDefault<IChartSerie>((Func<IChartSerie, bool>) (x => x.CategoryLabels != null || x.EnteredDirectlyCategoryLabels != null)) is ChartSerieImpl chartSerieImpl)
      {
        range1 = chartSerieImpl.CategoryLabels;
        directCategories = chartSerieImpl.EnteredDirectlyCategoryLabels;
      }
    }
    else
    {
      range1 = serie.CategoryLabels;
      directCategories = serie.EnteredDirectlyCategoryLabels;
    }
    IRange range2 = (IRange) null;
    if (isBubbles)
      range2 = serie.Bubbles;
    IRange range3 = this.CheckIfValidValueRange(values1) ? values1 : (IRange) null;
    IRange categoryRanges = this.CheckIfValidValueRange(range1) ? range1 : (IRange) null;
    if (isBubbles)
      range2 = this.CheckIfValidValueRange(range2) ? range2 : (IRange) null;
    object[] enteredDirectlyValues = serie.EnteredDirectlyValues;
    object[] enteredDirectlyBubbles = serie.EnteredDirectlyBubbles;
    ChartCategoryAxisImpl categoryAxisImpl = (serie.UsePrimaryAxis ? serie.ParentChart.PrimaryCategoryAxis : serie.ParentChart.SecondaryCategoryAxis) as ChartCategoryAxisImpl;
    ChartValueAxisImpl valueAxis = serie.UsePrimaryAxis ? parentChart.PrimaryValueAxis as ChartValueAxisImpl : parentChart.SecondaryValueAxis as ChartValueAxisImpl;
    WorksheetImpl worksheetImpl1 = (WorksheetImpl) null;
    WorksheetImpl worksheetImpl2 = (WorksheetImpl) null;
    WorksheetImpl sheet = (WorksheetImpl) null;
    if (range3 != null)
    {
      worksheetImpl1 = range3.Worksheet as WorksheetImpl;
      intList = this.IsChartEx ? (List<int>) null : (parentChart.PlotVisibleOnly ? new List<int>(chartPoints.Count) : (List<int>) null);
    }
    if (categoryRanges != null)
    {
      worksheetImpl2 = categoryRanges.Worksheet as WorksheetImpl;
      categoryIndexesArray = this.IsChartEx || parentChart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelNone ? (List<int>) null : (parentChart.PlotVisibleOnly ? new List<int>(chartPoints.Count) : (List<int>) null);
    }
    if (isBubbles && range2 != null)
      sheet = range2.Worksheet as WorksheetImpl;
    double categoryDisplayUnit = 1.0;
    double displayUnitValue = this.GetDisplayUnitValue(valueAxis);
    if (isBubbleOrScatter && categoryAxisImpl.IsChartBubbleOrScatter)
      categoryDisplayUnit = this.GetDisplayUnitValue((ChartValueAxisImpl) categoryAxisImpl);
    string numberFormat = categoryAxisImpl.NumberFormat;
    string str = categoryRanges != null ? (categoryRanges.Cells.Length >= 1 ? categoryRanges.Cells[0].NumberFormat : "General") : (string.IsNullOrEmpty(serie.CategoriesFormatCode) ? "General" : serie.CategoriesFormatCode);
    ChartPointValueType expectedType = ChartPointValueType.TextAxisValue;
    bool valueContainsAnyString = this.ValuesContainsAnyString(categoryRanges, directCategories, categoryIndexesArray, worksheetImpl2);
    if (!this.IsChartEx)
      expectedType = directCategories != null || categoryRanges != null ? this.GetChartPointValueType(isBubbleOrScatter, valueContainsAnyString, str, numberFormat, parentChart, categoryAxisImpl) : (!isBubbleOrScatter ? ChartPointValueType.DefaultIndexValueWithAxisNumFmt : ChartPointValueType.ScatterDefaultIndexXValue);
    if (categoryIndexesArray != null && categoryIndexesArray.Contains(0))
    {
      int index = 0;
      while (categoryIndexesArray.Contains(index) && index < chartPoints.Count)
        ++index;
      str = index < categoryRanges.Cells.Length ? categoryRanges.Cells[index].NumberFormat : "General";
    }
    bool isCategoryLabelsNeeded = false;
    string dataLabelNumberFormat = (string) null;
    if (!this.IsChartEx && (serie.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels)
    {
      ChartDataLabelsImpl dataLabels = serie.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
      bool flag2 = (serie.DataPoints as ChartDataPointsCollection).CheckDPDataLabels();
      if (dataLabels != null && !dataLabels.IsDelete && dataLabels.IsCategoryName || flag2)
      {
        isCategoryLabelsNeeded = true;
        dataLabelNumberFormat = dataLabels.NumberFormat;
      }
    }
    int index1 = 0;
    int index2 = 0;
    int index3 = 0;
    for (; index1 < chartPoints.Count; ++index1)
    {
      IRange cell1 = (IRange) null;
      object dataLabelResult = (object) null;
      if ((expectedType == ChartPointValueType.ScatterXAxisValue ? (!showEmptyPoints ? 1 : 0) : (expectedType == ChartPointValueType.DateTimeAxisValue ? 1 : 0)) != 0 && directCategories != null && index2 < directCategories.Length && directCategories[index2] == null)
      {
        if (intList == null)
          intList = new List<int>(chartPoints.Count);
        intList.Add(index1);
        ++index2;
        ++index3;
      }
      else
      {
        if (range3 != null)
        {
          WorksheetImpl.TRangeValueType cellType = WorksheetImpl.TRangeValueType.Number;
          IRange cell2 = range3.Cells[index1];
          if (intList != null && this.IsRowOrColumnIsHidden(cell2, worksheetImpl1))
            intList.Add(index1);
          else
            chartPoints[index1].Value = this.GetdoubleValueFromCell(cell2, worksheetImpl1, out cellType);
          if (cellType == WorksheetImpl.TRangeValueType.Blank && !this.IsChartEx && showEmptyPoints)
          {
            switch (emptyPointValue)
            {
              case 0:
                if (startSerieType.Contains("Pie") || startSerieType.Contains("Doughnut"))
                {
                  chartPoints[index1].Value = 0.0;
                  break;
                }
                break;
              case 1:
                chartPoints[index1].IsSummary = true;
                break;
            }
          }
        }
        else if (enteredDirectlyValues != null)
        {
          if (enteredDirectlyValues[index1] == null)
            chartPoints[index1].Value = double.NaN;
          else if (enteredDirectlyValues[index1] is string)
          {
            if (enteredDirectlyValues[index1].ToString().Equals("#N/A", StringComparison.OrdinalIgnoreCase))
              chartPoints[index1].Value = double.NaN;
            else
              chartPoints[index1].Value = 0.0;
          }
          else
            chartPoints[index1].Value = Convert.ToDouble(enteredDirectlyValues[index1]);
        }
        if (displayUnitValue > 1.0)
          chartPoints[index1].Value /= displayUnitValue;
        object categoryValue = (object) "";
        if (categoryRanges != null)
        {
          if (index2 < categoryRanges.Cells.Length)
            cell1 = categoryRanges.Cells[index2];
          else if (isBubbleOrScatter || expectedType == ChartPointValueType.DateTimeAxisValue)
            break;
        }
        else if (directCategories != null)
        {
          if (index2 < directCategories.Length)
            categoryValue = directCategories[index2];
          else if (isBubbleOrScatter || expectedType == ChartPointValueType.DateTimeAxisValue)
            break;
        }
        else if (!histogramOrPareto)
          categoryValue = (object) (index2 + 1);
        if (categoryIndexesArray != null)
        {
          if (index2 < categoryRanges.Cells.Length)
          {
            while (categoryIndexesArray.Contains(index2) && index2 < chartPoints.Count)
              ++index2;
          }
          if (index1 != index2)
          {
            if (index2 < categoryRanges.Cells.Length)
              cell1 = categoryRanges.Cells[index2];
            else if (!isBubbleOrScatter && expectedType != ChartPointValueType.DateTimeAxisValue)
              cell1 = (IRange) null;
            else
              break;
          }
        }
        if (intList == null || !intList.Contains(index1))
        {
          if ((categoryIndexesArray == null ? 1 : (cell1 != null ? 1 : 0)) != 0)
          {
            chartPoints[index1].X = this.GetValueFromCell(cell1, categoryValue, worksheetImpl2, index3, expectedType, numberFormat, str, categoryDisplayUnit, isCategoryLabelsNeeded, out dataLabelResult, dataLabelNumberFormat);
            if (isCategoryLabelsNeeded)
            {
              int num;
              switch (expectedType)
              {
                case ChartPointValueType.DefaultIndexValue:
                  num = dataLabelResult != null ? 1 : 0;
                  break;
                case ChartPointValueType.TextAxisValue:
                  goto label_84;
                default:
                  num = 1;
                  break;
              }
              if (num != 0)
                chartPoints[index1].Close = dataLabelResult ?? (object) "";
            }
          }
          else
          {
            if (isCategoryLabelsNeeded && expectedType != ChartPointValueType.TextAxisValue)
              chartPoints[index1].Close = (object) "";
            if (expectedType == ChartPointValueType.TextAxisValue || expectedType == ChartPointValueType.TextAxisValueWithNumFmt)
              chartPoints[index1].X = (object) "";
            else
              chartPoints[index1].X = (object) 0;
          }
label_84:
          ++index2;
          ++index3;
        }
        if (isBubbles)
        {
          chartPoints[index1].Size = (object) 0;
          if (range2 != null)
          {
            if (index1 < range2.Cells.Length)
              chartPoints[index1].Size = (object) this.GetdoubleValueFromCell(range2.Cells[index1], sheet, out WorksheetImpl.TRangeValueType _);
          }
          else if (enteredDirectlyBubbles != null)
          {
            if (index1 < enteredDirectlyBubbles.Length)
              chartPoints[index1].Size = (object) (enteredDirectlyBubbles[index1] is string ? 0.0 : Convert.ToDouble(enteredDirectlyBubbles[index1]));
          }
          else
            chartPoints[index1].Size = (object) 1;
          if (chartPoints[index1].Size.ToString() == double.NaN.ToString())
            chartPoints[index1].Size = (object) 0.0;
          if (!flag1 && Convert.ToDouble(chartPoints[index1].Size) < 0.0)
            chartPoints[index1].Size = (object) 0.0;
        }
      }
    }
    if (intList != null)
    {
      ObservableCollection<ChartPoint> observableCollection1 = new ObservableCollection<ChartPoint>();
      int num1 = 0;
      foreach (ChartPoint chartPoint in (Collection<ChartPoint>) chartPoints)
      {
        if (!intList.Contains(num1))
        {
          if (observableCollection1.Count == 2 && (serie.SerieType == ExcelChartType.Line || serie.SerieType == ExcelChartType.Scatter_Line) && serie.SerieFormat.LineProperties != null && serie.SerieFormat.LineProperties.LinePattern != ExcelChartLinePattern.Solid && serie.SerieFormat.LineProperties.LinePattern != ExcelChartLinePattern.None)
          {
            int result = 0;
            if (int.TryParse(observableCollection1[0].X.ToString(), out result) && Convert.ToInt32(observableCollection1[0].X) == 0 && Convert.ToInt32(observableCollection1[1].X) == 0 && Convert.ToInt32(chartPoint.X) == 0)
            {
              if (chartPoint.Value == observableCollection1[0].Value)
                break;
            }
          }
          observableCollection1.Add(chartPoint);
        }
        ++num1;
      }
      int num2 = 0;
      if (observableCollection1.Count > 3 && observableCollection1[0].X != null && observableCollection1[1].X != null && observableCollection1[2].X != null && observableCollection1[3].X != null && observableCollection1[0].X.Equals(observableCollection1[1].X) && observableCollection1[2].X.Equals(observableCollection1[3].X) && observableCollection1[0].Value == observableCollection1[2].Value && observableCollection1[1].Value == observableCollection1[3].Value && (serie.SerieType == ExcelChartType.Line || serie.SerieType == ExcelChartType.Scatter_Line))
      {
        ObservableCollection<ChartPoint> observableCollection2 = new ObservableCollection<ChartPoint>();
        foreach (ChartPoint chartPoint in (Collection<ChartPoint>) observableCollection1)
        {
          if (num2 != 0 && num2 % 2 == 0)
            observableCollection2.Add(new ChartPoint()
            {
              Value = double.NaN,
              X = chartPoint.X
            });
          observableCollection2.Add(chartPoint);
          ++num2;
        }
        observableCollection1 = observableCollection2;
      }
      chartPoints = observableCollection1;
    }
    if ((isBubbleOrScatter || expectedType == ChartPointValueType.DateTimeAxisValue) && index1 + 1 != chartPoints.Count)
    {
      for (int index4 = chartPoints.Count - 1; index4 >= index1; --index4)
        chartPoints.RemoveAt(index4);
    }
    this.SetDateTimeIntervalType(serie, chartPoints, false);
    return chartPoints;
  }

  private ChartPointValueType GetChartPointValueType(
    bool isBubbleOrScatter,
    bool valueContainsAnyString,
    string labelNumFmt,
    string axisNumFmt,
    ChartImpl chart,
    ChartCategoryAxisImpl categoryAxis)
  {
    return !isBubbleOrScatter ? (chart.CategoryLabelLevel != ExcelCategoriesLabelLevel.CategoriesLabelLevelNone ? (valueContainsAnyString ? ChartPointValueType.TextAxisValue : (categoryAxis.CategoryType != ExcelCategoryType.Time ? (categoryAxis.IsSourceLinked ? ChartPointValueType.TextAxisValue : ChartPointValueType.TextAxisValueWithNumFmt) : ChartPointValueType.DateTimeAxisValue)) : (!categoryAxis.IsSourceLinked ? (!(axisNumFmt == "General") ? ChartPointValueType.DefaultIndexValueWithAxisNumFmt : ChartPointValueType.DefaultIndexValue) : (!valueContainsAnyString ? (!(labelNumFmt != "General") || !(labelNumFmt.ToLower() != "standard") ? ChartPointValueType.DefaultIndexValue : ChartPointValueType.DefaultIndexValueWithNumFmt) : ChartPointValueType.DefaultIndexValue))) : (!valueContainsAnyString ? (categoryAxis.CategoryType != ExcelCategoryType.Time ? ChartPointValueType.ScatterXAxisValue : ChartPointValueType.DateTimeAxisValue) : ChartPointValueType.ScatterDefaultIndexXValue);
  }

  private bool ValuesContainsAnyString(
    IRange categoryRanges,
    object[] directCategories,
    List<int> categoryIndexesArray,
    WorksheetImpl worksheet)
  {
    bool flag = false;
    if (categoryRanges != null)
    {
      int num = 0;
      foreach (IRange categoryRange in (IEnumerable<IRange>) categoryRanges)
      {
        if (categoryIndexesArray != null && this.IsRowOrColumnIsHidden(categoryRange, worksheet))
        {
          categoryIndexesArray.Add(num);
        }
        else
        {
          switch (worksheet.GetCellType(categoryRange.Row, categoryRange.Column, true))
          {
            case WorksheetImpl.TRangeValueType.String:
            case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
              if (categoryIndexesArray == null)
                return true;
              flag = true;
              break;
          }
        }
        ++num;
      }
    }
    else if (directCategories != null && ((IEnumerable<object>) directCategories).Any<object>((Func<object, bool>) (x =>
    {
      if (x == null)
        return false;
      return x.GetType() == typeof (string) || !(x is IConvertible);
    })))
      return true;
    return flag;
  }

  private object GetValueFromCell(
    IRange cell,
    object categoryValue,
    WorksheetImpl sheet,
    int index,
    ChartPointValueType expectedType,
    string axisNumberFormat,
    string labelNumberFormat,
    double categoryDisplayUnit,
    bool isCategoryLabelsNeeded,
    out object dataLabelResult,
    string dataLabelNumberFormat)
  {
    object d = (object) "";
    if (categoryValue == null)
      categoryValue = (object) "";
    dataLabelResult = (object) null;
    switch (expectedType)
    {
      case ChartPointValueType.ScatterXAxisValue:
      case ChartPointValueType.DateTimeAxisValue:
      case ChartPointValueType.TextAxisValueWithNumFmt:
        WorksheetImpl.TRangeValueType cellType = WorksheetImpl.TRangeValueType.Number;
        double num = 0.0;
        if (cell != null)
          num = this.GetdoubleValueFromCell(cell, sheet, out cellType);
        else if (categoryValue != (object) "")
          num = Convert.ToDouble(categoryValue);
        if (num.ToString() == double.NaN.ToString() && (expectedType != ChartPointValueType.TextAxisValueWithNumFmt || cellType != WorksheetImpl.TRangeValueType.Blank && (cellType & WorksheetImpl.TRangeValueType.Error) != WorksheetImpl.TRangeValueType.Error))
          num = 0.0;
        if (num.ToString() != double.NaN.ToString())
        {
          if (categoryDisplayUnit > 1.0 && expectedType == ChartPointValueType.ScatterXAxisValue)
            num /= categoryDisplayUnit;
          d = (object) num;
        }
        else
          d = (object) "";
        if (expectedType == ChartPointValueType.DateTimeAxisValue)
        {
          d = (object) DateTime.FromOADate((double) d);
          break;
        }
        break;
      case ChartPointValueType.ScatterDefaultIndexXValue:
      case ChartPointValueType.DefaultIndexValueWithAxisNumFmt:
      case ChartPointValueType.DefaultIndexValueWithNumFmt:
        d = (object) (index + 1);
        if (categoryDisplayUnit > 1.0 && expectedType == ChartPointValueType.ScatterDefaultIndexXValue)
        {
          d = (object) ((double) (index + 1) / categoryDisplayUnit);
          break;
        }
        break;
      case ChartPointValueType.DefaultIndexValue:
        d = (object) (index + 1).ToString();
        break;
      case ChartPointValueType.TextAxisValue:
        if (cell != null)
        {
          RangeImpl rangeImpl = cell as RangeImpl;
          bool bAutofitText = rangeImpl.m_bAutofitText;
          rangeImpl.m_bAutofitText = true;
          d = (object) cell.DisplayText;
          rangeImpl.m_bAutofitText = bAutofitText;
          break;
        }
        d = categoryValue == (object) "" || !(labelNumberFormat.ToLower() != "standard") || !(labelNumberFormat != "General") ? (object) categoryValue.ToString() : (object) this.ApplyNumberFormat(categoryValue, labelNumberFormat);
        break;
    }
    if (expectedType == ChartPointValueType.TextAxisValue)
      return d;
    bool flag = expectedType == ChartPointValueType.DefaultIndexValueWithNumFmt;
    if (isCategoryLabelsNeeded)
    {
      switch (expectedType)
      {
        case ChartPointValueType.ScatterXAxisValue:
          if (categoryDisplayUnit > 0.0)
          {
            if (d == (object) "")
            {
              dataLabelResult = (object) "";
              break;
            }
            dataLabelResult = cell != null ? d : (object) ((double) d * categoryDisplayUnit);
            dataLabelResult = (object) this.ApplyNumberFormat(dataLabelResult, cell != null ? cell.NumberFormat : labelNumberFormat);
            break;
          }
          if (cell != null)
          {
            RangeImpl rangeImpl = cell as RangeImpl;
            bool bAutofitText = rangeImpl.m_bAutofitText;
            rangeImpl.m_bAutofitText = true;
            dataLabelResult = (object) cell.DisplayText;
            rangeImpl.m_bAutofitText = bAutofitText;
            break;
          }
          dataLabelResult = categoryValue == (object) "" || !(labelNumberFormat.ToLower() != "standard") || !(labelNumberFormat != "General") ? (object) categoryValue.ToString() : (object) this.ApplyNumberFormat(categoryValue, labelNumberFormat);
          break;
        case ChartPointValueType.ScatterDefaultIndexXValue:
        case ChartPointValueType.DateTimeAxisValue:
        case ChartPointValueType.TextAxisValueWithNumFmt:
          if (cell != null)
          {
            RangeImpl rangeImpl = cell as RangeImpl;
            bool bAutofitText = rangeImpl.m_bAutofitText;
            rangeImpl.m_bAutofitText = true;
            dataLabelResult = (object) cell.DisplayText;
            rangeImpl.m_bAutofitText = bAutofitText;
            break;
          }
          dataLabelResult = categoryValue == (object) "" || !(labelNumberFormat.ToLower() != "standard") || !(labelNumberFormat != "General") ? (object) categoryValue.ToString() : (object) this.ApplyNumberFormat(categoryValue, labelNumberFormat);
          break;
        case ChartPointValueType.DefaultIndexValue:
        case ChartPointValueType.DefaultIndexValueWithAxisNumFmt:
        case ChartPointValueType.DefaultIndexValueWithNumFmt:
          if (dataLabelNumberFormat != null && dataLabelNumberFormat.ToLower() != "standard" && dataLabelNumberFormat != "General")
            dataLabelResult = (object) this.ApplyNumberFormat(d, dataLabelNumberFormat);
          else if (expectedType != ChartPointValueType.DefaultIndexValue)
            dataLabelResult = (object) d.ToString();
          if (expectedType == ChartPointValueType.DefaultIndexValue)
            return d;
          break;
      }
    }
    if (expectedType == ChartPointValueType.TextAxisValueWithNumFmt || expectedType == ChartPointValueType.DefaultIndexValueWithNumFmt || expectedType == ChartPointValueType.DefaultIndexValueWithAxisNumFmt)
    {
      if (d == (object) "")
      {
        d = cell != null ? (object) cell.DisplayText : (object) "";
      }
      else
      {
        string numberFormat = flag ? labelNumberFormat : axisNumberFormat;
        if (numberFormat == "standard" || numberFormat == "General")
          d = (object) d.ToString();
        else if (cell != null)
          d = (object) this.ApplyNumberFormat(d, numberFormat);
      }
    }
    return d;
  }

  private double GetdoubleValueFromCell(
    IRange cell,
    WorksheetImpl sheet,
    out WorksheetImpl.TRangeValueType cellType)
  {
    string calculatedValue = cell.CalculatedValue;
    cellType = sheet.GetCellType(cell.Row, cell.Column, true);
    double num = cell.Number;
    if (num.ToString() == double.NaN.ToString())
    {
      if (cell.HasFormulaNumberValue || cell.HasFormulaDateTime)
      {
        double formulaNumberValue = cell.FormulaNumberValue;
        num = formulaNumberValue != 0.0 || calculatedValue == null ? formulaNumberValue : cell.FormulaNumberValue;
      }
      else if ((cellType & WorksheetImpl.TRangeValueType.Formula) == WorksheetImpl.TRangeValueType.Formula && (cellType & WorksheetImpl.TRangeValueType.Error) == WorksheetImpl.TRangeValueType.Error)
      {
        if (calculatedValue != null && (sheet.GetCellType(cell.Row, cell.Column, true) & WorksheetImpl.TRangeValueType.Number) != WorksheetImpl.TRangeValueType.Blank)
          num = cell.FormulaNumberValue;
        else if (cell.HasFormulaErrorValue && cell.FormulaErrorValue == "#DIV/0!")
          num = 0.0;
      }
      else
        num = cellType == WorksheetImpl.TRangeValueType.Blank || (cellType & WorksheetImpl.TRangeValueType.Error) == WorksheetImpl.TRangeValueType.Error && (string.IsNullOrEmpty(cell.Error) || cell.Error.Equals("#N/A", StringComparison.OrdinalIgnoreCase)) ? double.NaN : 0.0;
    }
    return num;
  }

  private void SetDateTimeIntervalType(
    ChartSerieImpl serie,
    ObservableCollection<ChartPoint> chartPoints,
    bool isStock)
  {
    if (serie.ParentChart.PrimaryCategoryAxis.CategoryType != ExcelCategoryType.Time || chartPoints.Count <= 1 || !(chartPoints[0].X is DateTime))
      return;
    if (serie.ParentSeries[0].Equals((object) serie) || serie.ParentSeries.OrderByType()[0].Equals((object) serie))
    {
      List<DateTime> source = new List<DateTime>(chartPoints.Count);
      foreach (ChartPoint chartPoint in (Collection<ChartPoint>) chartPoints)
        source.Add((DateTime) chartPoint.X);
      this.SortedIndexes = source.Select<DateTime, KeyValuePair<DateTime, int>>((Func<DateTime, int, KeyValuePair<DateTime, int>>) ((x, i) => new KeyValuePair<DateTime, int>(x, i))).OrderBy<KeyValuePair<DateTime, int>, DateTime>((Func<KeyValuePair<DateTime, int>, DateTime>) (x => x.Key)).ToList<KeyValuePair<DateTime, int>>().Select<KeyValuePair<DateTime, int>, int>((Func<KeyValuePair<DateTime, int>, int>) (x => x.Value)).ToList<int>().ToArray();
      ChartPoint[] array = new ChartPoint[chartPoints.Count];
      chartPoints.CopyTo(array, 0);
      chartPoints.Clear();
      foreach (int sortedIndex in this.SortedIndexes)
        chartPoints.Add(array[sortedIndex]);
      this.FirstSeriesPoints = chartPoints;
      DateTime dateTime = (DateTime) chartPoints[0].X;
      if (chartPoints.Where<ChartPoint>((Func<ChartPoint, bool>) (x => ((DateTime) x.X).Day == dateTime.Day && ((DateTime) x.X).Year == dateTime.Year)).Count<ChartPoint>() == chartPoints.Count)
        this.DateTimeIntervalType = DateTimeIntervalType.Months;
      else if (chartPoints.Where<ChartPoint>((Func<ChartPoint, bool>) (x => ((DateTime) x.X).Month == dateTime.Month && ((DateTime) x.X).Year == dateTime.Year)).Count<ChartPoint>() == chartPoints.Count)
        this.DateTimeIntervalType = DateTimeIntervalType.Days;
      else if (chartPoints.Where<ChartPoint>((Func<ChartPoint, bool>) (x => ((DateTime) x.X).Day == dateTime.Day && ((DateTime) x.X).Month == dateTime.Month)).Count<ChartPoint>() == chartPoints.Count)
        this.DateTimeIntervalType = DateTimeIntervalType.Years;
      else
        this.DateTimeIntervalType = DateTimeIntervalType.Auto;
    }
    else
    {
      ChartPoint[] array = new ChartPoint[chartPoints.Count];
      chartPoints.CopyTo(array, 0);
      chartPoints.Clear();
      for (int index = 0; index < this.FirstSeriesPoints.Count; ++index)
      {
        ChartPoint chartPoint = new ChartPoint();
        chartPoint.X = this.FirstSeriesPoints[index].X;
        if (isStock)
          chartPoint.Size = this.FirstSeriesPoints[index].Size;
        else
          chartPoint.Close = this.FirstSeriesPoints[index].Close;
        if (this.SortedIndexes[index] < array.Length)
          chartPoint.Value = array[this.SortedIndexes[index]].Value;
        chartPoints.Add(chartPoint);
      }
    }
  }

  private ObservableCollection<ChartPoint> GetChartPointsValuesForStockChart(
    bool showEmptyPoints,
    ChartImpl chart,
    ChartSerieImpl serie,
    ObservableCollection<ChartPoint> values,
    bool isCandleChart)
  {
    ObservableCollection<ChartPoint> chartPoints = values;
    ChartSerieImpl chartSerieImpl1 = (ChartSerieImpl) null;
    List<int> categoryIndexesArray = (List<int>) null;
    ChartImpl parentChart = serie.ParentChart;
    ChartSerieImpl chartSerieImpl2;
    ChartSerieImpl chartSerieImpl3;
    ChartSerieImpl chartSerieImpl4;
    if (isCandleChart)
    {
      int[] numArray;
      if (!serie.ParentChart.ChartType.ToString().ToLower().Contains("volume"))
        numArray = new int[4]{ 0, 1, 2, 3 };
      else
        numArray = (int[]) null;
      int[] source = numArray;
      if (source == null)
      {
        source = new int[4];
        int index1 = 0;
        int index2 = 0;
        for (; index1 < 5; ++index1)
        {
          if (!chart.Series[index1].SerieType.ToString().ToLower().Contains("column"))
          {
            source[index2] = index1;
            ++index2;
          }
        }
        if (((IEnumerable<int>) source).Count<int>((Func<int, bool>) (x => x == 0)) > 1)
          source = new int[4]{ 0, 1, 2, 3 };
      }
      chartSerieImpl1 = chart.Series[source[0]] as ChartSerieImpl;
      chartSerieImpl2 = chart.Series[source[1]] as ChartSerieImpl;
      chartSerieImpl3 = chart.Series[source[2]] as ChartSerieImpl;
      chartSerieImpl4 = chart.Series[source[3]] as ChartSerieImpl;
    }
    else
    {
      chartSerieImpl2 = chart.Series[0] as ChartSerieImpl;
      chartSerieImpl3 = chart.Series[1] as ChartSerieImpl;
      chartSerieImpl4 = chart.Series[2] as ChartSerieImpl;
    }
    IRange range1 = (IRange) null;
    object[] directValues = (object[]) null;
    if (isCandleChart)
      directValues = chartSerieImpl1.EnteredDirectlyValues;
    object[] enteredDirectlyValues1 = chartSerieImpl2.EnteredDirectlyValues;
    object[] enteredDirectlyValues2 = chartSerieImpl3.EnteredDirectlyValues;
    object[] enteredDirectlyValues3 = chartSerieImpl4.EnteredDirectlyValues;
    object[] directlyCategoryLabels = serie.EnteredDirectlyCategoryLabels;
    List<int> intList = (List<int>) null;
    if (isCandleChart)
      range1 = chartSerieImpl1.FilteredValue == null ? chartSerieImpl1.Values : (IRange) null;
    IRange values1 = chartSerieImpl2.FilteredValue == null ? chartSerieImpl2.Values : (IRange) null;
    IRange values2 = chartSerieImpl3.FilteredValue == null ? chartSerieImpl3.Values : (IRange) null;
    IRange values3 = chartSerieImpl4.FilteredValue == null ? chartSerieImpl4.Values : (IRange) null;
    IRange categoryLabels = serie.CategoryLabels;
    IRange range2 = this.CheckIfValidValueRange(range1) ? range1 : (IRange) null;
    IRange range3 = this.CheckIfValidValueRange(values1) ? values1 : (IRange) null;
    IRange range4 = this.CheckIfValidValueRange(values2) ? values2 : (IRange) null;
    IRange range5 = this.CheckIfValidValueRange(values3) ? values3 : (IRange) null;
    IRange categoryRanges = this.CheckIfValidValueRange(categoryLabels) ? categoryLabels : (IRange) null;
    WorksheetImpl worksheet1 = (WorksheetImpl) null;
    WorksheetImpl worksheet2 = (WorksheetImpl) null;
    WorksheetImpl worksheet3 = (WorksheetImpl) null;
    WorksheetImpl worksheet4 = (WorksheetImpl) null;
    WorksheetImpl worksheetImpl = (WorksheetImpl) null;
    if (range2 != null)
      worksheet1 = range2.Worksheet as WorksheetImpl;
    if (range3 != null)
      worksheet2 = range3.Worksheet as WorksheetImpl;
    if (range4 != null)
      worksheet3 = range4.Worksheet as WorksheetImpl;
    if (range5 != null)
      worksheet4 = range5.Worksheet as WorksheetImpl;
    if (categoryRanges != null)
    {
      worksheetImpl = categoryRanges.Worksheet as WorksheetImpl;
      categoryIndexesArray = chart.CategoryLabelLevel == ExcelCategoriesLabelLevel.CategoriesLabelLevelNone ? (List<int>) null : (chart.PlotVisibleOnly ? new List<int>(chartPoints.Count) : (List<int>) null);
    }
    ChartCategoryAxisImpl categoryAxis = (serie.UsePrimaryAxis ? serie.ParentChart.PrimaryCategoryAxis : serie.ParentChart.SecondaryCategoryAxis) as ChartCategoryAxisImpl;
    double displayUnitValue = this.GetDisplayUnitValue((serie.UsePrimaryAxis ? serie.ParentChart.PrimaryValueAxis : serie.ParentChart.SecondaryValueAxis) as ChartValueAxisImpl);
    string numberFormat = categoryAxis.NumberFormat;
    string str = categoryRanges != null ? (categoryRanges.Cells.Length >= 1 ? categoryRanges.Cells[0].NumberFormat : "General") : (string.IsNullOrEmpty(serie.CategoriesFormatCode) ? "General" : serie.CategoriesFormatCode);
    bool valueContainsAnyString = this.ValuesContainsAnyString(categoryRanges, directlyCategoryLabels, categoryIndexesArray, worksheetImpl);
    if (categoryIndexesArray != null && categoryIndexesArray.Contains(0))
    {
      int index = 0;
      while (categoryIndexesArray.Contains(index) && index < chartPoints.Count)
        ++index;
      str = index < categoryRanges.Cells.Length ? categoryRanges.Cells[index].NumberFormat : "General";
    }
    ChartPointValueType expectedType = directlyCategoryLabels != null || categoryRanges != null ? this.GetChartPointValueType(false, valueContainsAnyString, str, numberFormat, parentChart, categoryAxis) : ChartPointValueType.DefaultIndexValueWithAxisNumFmt;
    bool isCategoryLabelsNeeded = false;
    string dataLabelNumberFormat = (string) null;
    if ((serie.DataPoints.DefaultDataPoint as ChartDataPointImpl).HasDataLabels)
    {
      ChartDataLabelsImpl dataLabels = serie.DataPoints.DefaultDataPoint.DataLabels as ChartDataLabelsImpl;
      bool flag = (serie.DataPoints as ChartDataPointsCollection).CheckDPDataLabels();
      if (dataLabels != null && !dataLabels.IsDelete && dataLabels.IsCategoryName || flag)
      {
        isCategoryLabelsNeeded = true;
        dataLabelNumberFormat = dataLabels.NumberFormat;
      }
    }
    int num1 = isCandleChart ? (range2 != null ? range2.Cells.Length : directValues.Length) : (range3 != null ? range3.Cells.Length : enteredDirectlyValues1.Length);
    int index3 = 0;
    int index4 = 0;
    while (index3 < num1)
    {
      if ((expectedType == ChartPointValueType.ScatterXAxisValue ? (!showEmptyPoints ? 1 : 0) : (expectedType == ChartPointValueType.DateTimeAxisValue ? 1 : 0)) != 0 && directlyCategoryLabels != null && index4 < directlyCategoryLabels.Length && directlyCategoryLabels[index4] == null)
      {
        if (intList == null)
          intList = new List<int>(chartPoints.Count);
        intList.Add(index3);
        ++index4;
      }
      else
      {
        IRange cell = (IRange) null;
        if (isCandleChart)
        {
          values[index3].Open = (object) this.GetValueForStockCharts(range2, directValues, worksheet1, index3, displayUnitValue);
          values[index3].High = (object) this.GetValueForStockCharts(range3, enteredDirectlyValues1, worksheet2, index3, displayUnitValue);
          values[index3].Low = (object) this.GetValueForStockCharts(range4, enteredDirectlyValues2, worksheet3, index3, displayUnitValue);
          values[index3].Close = (object) this.GetValueForStockCharts(range5, enteredDirectlyValues3, worksheet4, index3, displayUnitValue);
        }
        else
        {
          List<double> doubleList = new List<double>(3);
          doubleList.Add(this.GetValueForStockCharts(range3, enteredDirectlyValues1, worksheet2, index3, displayUnitValue));
          doubleList.Add(this.GetValueForStockCharts(range4, enteredDirectlyValues2, worksheet3, index3, displayUnitValue));
          doubleList.Add(this.GetValueForStockCharts(range5, enteredDirectlyValues3, worksheet4, index3, displayUnitValue));
          doubleList.Sort();
          values[index3].Low = (object) doubleList[0];
          values[index3].High = (object) doubleList[2];
          doubleList.Clear();
        }
        object categoryValue = (object) "";
        object dataLabelResult = (object) null;
        if (categoryRanges != null && index3 < categoryRanges.Cells.Length)
          cell = categoryRanges.Cells[index3];
        else if (directlyCategoryLabels != null && index3 < directlyCategoryLabels.Length)
          categoryValue = directlyCategoryLabels[index3];
        if (categoryIndexesArray != null && categoryRanges != null)
        {
          if (index4 < categoryRanges.Cells.Length)
          {
            while (categoryIndexesArray.Contains(index4) && index4 < chartPoints.Count)
              ++index4;
          }
          if (index3 != index4)
            cell = index4 >= categoryRanges.Cells.Length ? (IRange) null : categoryRanges.Cells[index4];
        }
        if ((categoryIndexesArray == null ? 1 : (cell != null ? 1 : 0)) != 0)
        {
          chartPoints[index3].X = this.GetValueFromCell(cell, categoryValue, worksheetImpl, index3, expectedType, numberFormat, str, 1.0, isCategoryLabelsNeeded, out dataLabelResult, dataLabelNumberFormat);
          if (isCategoryLabelsNeeded && expectedType != ChartPointValueType.TextAxisValue && expectedType != ChartPointValueType.DefaultIndexValue)
            chartPoints[index3].Size = dataLabelResult ?? (object) "";
        }
        else
        {
          if (isCategoryLabelsNeeded && expectedType != ChartPointValueType.TextAxisValue)
            chartPoints[index3].Size = (object) "";
          if (expectedType == ChartPointValueType.TextAxisValue || expectedType == ChartPointValueType.TextAxisValueWithNumFmt)
            chartPoints[index3].X = (object) "";
          else
            chartPoints[index3].X = (object) 0;
        }
      }
      ++index3;
      ++index4;
    }
    if (intList != null)
    {
      ObservableCollection<ChartPoint> observableCollection = new ObservableCollection<ChartPoint>();
      int num2 = 0;
      foreach (ChartPoint chartPoint in (Collection<ChartPoint>) chartPoints)
      {
        if (!intList.Contains(num2))
          observableCollection.Add(chartPoint);
        ++num2;
      }
      chartPoints = observableCollection;
    }
    this.SetDateTimeIntervalType(serie, chartPoints, true);
    return chartPoints;
  }

  private double GetValueForStockCharts(
    IRange range,
    object[] directValues,
    WorksheetImpl worksheet,
    int index,
    double displayUnit)
  {
    double valueForStockCharts = 0.0;
    if (range != null && index < range.Cells.Length)
      valueForStockCharts = this.GetdoubleValueFromCell(range.Cells[index], worksheet, out WorksheetImpl.TRangeValueType _);
    else if (directValues != null && index < directValues.Length)
      valueForStockCharts = directValues[index] == null || directValues[index] is string ? 0.0 : Convert.ToDouble(directValues[index]);
    if (valueForStockCharts.ToString() == double.NaN.ToString())
      valueForStockCharts = 0.0;
    if (displayUnit > 1.0)
      valueForStockCharts /= displayUnit;
    return valueForStockCharts;
  }

  internal Dictionary<int, ObservableCollection<ChartPoint>> GetListofPoints(
    ChartImpl chart,
    out Dictionary<int, string> names)
  {
    Dictionary<int, ObservableCollection<ChartPoint>> listofPoints = new Dictionary<int, ObservableCollection<ChartPoint>>(chart.Series.Count);
    names = new Dictionary<int, string>(chart.Series.Count);
    string str1 = chart.ChartType.ToString();
    bool flag1 = str1.Contains("Combination_Chart");
    bool flag2 = false;
    bool flag3 = false;
    this.IsChartEx = ChartImpl.IsChartExSerieType(chart.ChartType);
    bool showEmptyPoints = false;
    int emptyPointValue = -1;
    switch (chart.DisplayBlanksAs)
    {
      case ExcelChartPlotEmpty.NotPlotted:
        showEmptyPoints = false;
        break;
      case ExcelChartPlotEmpty.Zero:
        showEmptyPoints = true;
        emptyPointValue = 0;
        break;
      case ExcelChartPlotEmpty.Interpolated:
        showEmptyPoints = true;
        break;
    }
    foreach (IChartSerie chartSerie in (IEnumerable<IChartSerie>) chart.Series)
    {
      if (!chartSerie.IsFiltered || this.IsChartEx)
      {
        ChartSerieImpl serie = chartSerie as ChartSerieImpl;
        bool flag4 = serie.Values == null && serie.EnteredDirectlyValues == null;
        if (flag3 && flag1)
        {
          if (chart.PrimaryFormats.Count == 1)
          {
            if (chart.SecondaryFormats.Count != 0)
              break;
          }
          else
            break;
        }
        string str2 = serie.SerieType.ToString();
        flag3 = str2.Contains("Radar");
        if (flag4)
        {
          listofPoints.Add(serie.Index, (ObservableCollection<ChartPoint>) null);
        }
        else
        {
          ViewModel viewModel = this.GetViewModel(serie);
          if (str2.Contains("Line") || str2.Contains("Radar"))
            emptyPointValue = 1;
          if (str1.Contains("Stock"))
          {
            listofPoints.Add(serie.Index, this.GetChartPointsValuesForStockChart(showEmptyPoints, chart, serie, viewModel.Products, str1.Contains("Open")));
            break;
          }
          listofPoints.Add(serie.Index, this.GetChartPointsValues(serie, showEmptyPoints, emptyPointValue, str2.Contains("Bubble"), viewModel.Products));
        }
        if (!flag2)
        {
          if (this.IsChartEx)
            break;
        }
        else
          break;
      }
    }
    foreach (IChartSerie chartSerie in (IEnumerable<IChartSerie>) chart.Series)
    {
      ChartSerieImpl serie = chartSerie as ChartSerieImpl;
      names.Add(serie.Index, this.GetSerieName(serie));
    }
    return listofPoints;
  }

  private void GetFillOnCommonSeries(
    ChartSerieImpl serie,
    ChartSeriesBase sfChartSerie,
    out List<int> negativeIndexes)
  {
    negativeIndexes = (List<int>) null;
    bool flag1 = true;
    if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003 && !this.IsChartEx)
      flag1 = false;
    System.Windows.Media.Brush brush1 = (System.Windows.Media.Brush) null;
    ObservableCollection<ChartPoint> itemsSource = sfChartSerie.ItemsSource as ObservableCollection<ChartPoint>;
    System.Windows.Media.Brush brush2;
    if (!(serie.SerieFormat as ChartSerieDataFormatImpl).IsAutomaticFormat || !flag1)
    {
      brush2 = this.GetBrushFromDataFormat((IChartFillBorder) serie.SerieFormat);
    }
    else
    {
      System.Drawing.Color color;
      brush1 = !this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, serie.Number, serie.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(serie.Number == -1 ? 0 : serie.Number, serie.ParentSeries.Count, false, false))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
      brush2 = brush1;
    }
    if (brush2 != null)
    {
      sfChartSerie.Palette = ChartColorPalette.Custom;
      ChartColorModel chartColorModel = new ChartColorModel(ChartColorPalette.Custom);
      if (sfChartSerie.ShowEmptyPoints)
        sfChartSerie.EmptyPointInterior = brush2;
      chartColorModel.CustomBrushes = new List<System.Windows.Media.Brush>(itemsSource.Count);
      sfChartSerie.ColorModel = chartColorModel;
      string name = sfChartSerie.GetType().Name;
      bool flag2 = name.Contains("Bubble");
      bool flag3 = (serie.InvertIfNegative.HasValue && serie.InvertIfNegative.Value && serie.InvertIfNegativeColor != (ColorObject) null || !serie.InvertIfNegative.HasValue && this.parentWorkbook.IsLoaded && this.parentWorkbook.Version != ExcelVersion.Excel97to2003) && serie.SerieFormat.Fill.FillType == ExcelFillType.SolidColor;
      if (flag3 && ((IEnumerable<string>) this.RectangleSerieTypes).Contains<string>(name) && this.TryParseNegativeIndexes(itemsSource, out negativeIndexes))
      {
        SolidColorBrush solidColorBrush = new SolidColorBrush(this.SfColor(serie.InvertIfNegativeColor.GetRGB((IWorkbook) serie.InnerWorkbook), serie.SerieFormat.Fill.Transparency));
        for (int index = 0; index < itemsSource.Count; ++index)
        {
          if (negativeIndexes.Contains(index))
            chartColorModel.CustomBrushes.Add((System.Windows.Media.Brush) solidColorBrush);
          else
            chartColorModel.CustomBrushes.Add(brush2);
        }
        if (((IEnumerable<string>) this.RectangleSerieTypes).Contains<string>(name))
          return;
        negativeIndexes = (List<int>) null;
      }
      else if (name.Contains("3D") && (name.Contains("Column") || name.Contains("Bar") || name.Contains("Line")) || flag2)
      {
        ChartDataPointsCollection dataPoints = serie.DataPoints as ChartDataPointsCollection;
        int deninedDpCount = dataPoints.DeninedDPCount;
        SolidColorBrush solidColorBrush = (SolidColorBrush) null;
        if (flag3 && !name.Contains("Line") && !flag2 && this.TryParseNegativeIndexes(itemsSource, out negativeIndexes))
          solidColorBrush = new SolidColorBrush(this.SfColor(serie.InvertIfNegativeColor.GetRGB((IWorkbook) serie.InnerWorkbook), serie.SerieFormat.Fill.Transparency));
        else if (flag2 && serie.SerieFormat.CommonSerieOptions.ShowNegativeBubbles)
        {
          for (int index = 0; index < itemsSource.Count; ++index)
          {
            if (!itemsSource[index].Size.Equals((object) 0) && (double) itemsSource[index].Size < 0.0)
            {
              if (negativeIndexes == null)
                negativeIndexes = new List<int>(itemsSource.Count);
              negativeIndexes.Add(index);
            }
          }
          if (negativeIndexes != null)
            solidColorBrush = new SolidColorBrush(this.SfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue));
        }
        bool flag4 = this.IsVaryColorSupported(serie);
        if (deninedDpCount > 0 || flag4 || solidColorBrush != null)
        {
          int count = itemsSource.Count;
          for (int index = 0; index < count; ++index)
          {
            ChartDataPointImpl hashDataPoint = dataPoints.m_hashDataPoints.ContainsKey(index) ? dataPoints.m_hashDataPoints[index] : (ChartDataPointImpl) null;
            if (hashDataPoint != null && (!(hashDataPoint.DataFormat as ChartSerieDataFormatImpl).IsAutomaticFormat || !flag1))
              chartColorModel.CustomBrushes.Add(this.GetBrushFromDataFormat((IChartFillBorder) hashDataPoint.DataFormat));
            else if (solidColorBrush != null && negativeIndexes.Contains(index))
              chartColorModel.CustomBrushes.Add((System.Windows.Media.Brush) solidColorBrush);
            else if (brush1 == null && brush2 != null)
              chartColorModel.CustomBrushes.Add(brush2);
            else if (flag4)
            {
              System.Drawing.Color color;
              if (this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, index, count - 1, out color))
                chartColorModel.CustomBrushes.Add((System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color)));
              else
                chartColorModel.CustomBrushes.Add((System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(index, count, !flag1, false))));
            }
            else if (brush1 != null)
              chartColorModel.CustomBrushes.Add(brush1);
            else if (brush1 == null)
            {
              System.Drawing.Color color;
              brush1 = !this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, flag4 ? index : serie.Number, flag4 ? count - 1 : serie.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(flag4 ? index : (serie.Number == -1 ? 0 : serie.Number), flag4 ? count : serie.ParentSeries.Count, !flag1, false))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
              chartColorModel.CustomBrushes.Add(brush1);
            }
          }
        }
        else
          chartColorModel.CustomBrushes.Add(brush2);
      }
      else
        chartColorModel.CustomBrushes.Add(brush2);
    }
    else
      sfChartSerie.Palette = ChartColorPalette.Metro;
  }

  private bool TryParseNegativeIndexes(
    ObservableCollection<ChartPoint> items,
    out List<int> listIndexes)
  {
    bool negativeIndexes = false;
    listIndexes = new List<int>();
    for (int index = 0; index < items.Count; ++index)
    {
      if (items[index].Value < 0.0)
      {
        listIndexes.Add(index);
        negativeIndexes = true;
      }
    }
    return negativeIndexes;
  }

  private bool GetFillOnPieDougnutSeries(
    ChartSerieImpl serie,
    ChartSeriesBase sfChartSerie,
    bool isPie,
    out System.Windows.Media.Brush borderBrush,
    out double borderThickness)
  {
    bool pieDougnutSeries = false;
    List<System.Windows.Media.Brush> brushList = new List<System.Windows.Media.Brush>();
    bool flag1 = true;
    if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003 && !this.IsChartEx)
      flag1 = false;
    borderBrush = (System.Windows.Media.Brush) null;
    borderThickness = 0.0;
    ChartColorModel chartColorModel = new ChartColorModel(ChartColorPalette.Custom);
    int num = serie.Values != null ? serie.Values.Count : (serie.EnteredDirectlyValues != null ? serie.EnteredDirectlyValues.Length : 0);
    int deninedDpCount = (serie.DataPoints as ChartDataPointsCollection).DeninedDPCount;
    bool isVaryColor = serie.GetCommonSerieFormat().IsVaryColor;
    System.Windows.Media.Brush brush1 = (System.Windows.Media.Brush) null;
    System.Drawing.Color color;
    if (!(serie.SerieFormat as ChartSerieDataFormatImpl).IsAutomaticFormat)
      brush1 = this.GetBrushFromDataFormat((IChartFillBorder) serie.SerieFormat);
    else if (!isVaryColor)
      brush1 = !this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, serie.Number, serie.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(serie.Number, serie.ParentSeries.Count, !flag1, false))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
    if (deninedDpCount > 0)
    {
      ChartDataPointsCollection dataPoints = serie.DataPoints as ChartDataPointsCollection;
      bool flag2 = true;
      for (int index = 0; index < num; ++index)
      {
        System.Windows.Media.Brush brush2 = (System.Windows.Media.Brush) null;
        ChartDataPointImpl hashDataPoint = dataPoints.m_hashDataPoints.ContainsKey(index) ? dataPoints.m_hashDataPoints[index] : (ChartDataPointImpl) null;
        if (hashDataPoint != null)
        {
          if (!(hashDataPoint.DataFormat as ChartSerieDataFormatImpl).IsAutomaticFormat || !flag1)
          {
            brush2 = this.GetBrushFromDataFormat((IChartFillBorder) hashDataPoint.DataFormat);
            ChartBorderImpl lineProperties = hashDataPoint.DataFormat.LineProperties as ChartBorderImpl;
            if (flag2 && lineProperties != null && lineProperties.HasLineProperties && lineProperties.LinePattern != ExcelChartLinePattern.None)
            {
              System.Windows.Media.Brush brushFromBorder = this.GetBrushFromBorder(lineProperties);
              borderThickness = this.GetBorderThickness(lineProperties);
              if (borderBrush == null)
                borderBrush = brushFromBorder;
              else if (brushFromBorder.ToString() != borderBrush.ToString())
                flag2 = false;
            }
          }
          else
            brush2 = !this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, isVaryColor ? index : serie.Number, isVaryColor ? num - 1 : serie.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(isVaryColor ? index : serie.Number, serie.ParentSeries.Count, !flag1, false))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
        }
        else if (brush1 == null)
          brush2 = !this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, isVaryColor ? index : serie.Number, isVaryColor ? num - 1 : serie.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(index, serie.ParentSeries.Count, !flag1, false))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
        if (brush2 != null)
        {
          brushList.Add(brush2);
          sfChartSerie.Palette = ChartColorPalette.Custom;
          if (brush2.ToString() == "#00000000")
            pieDougnutSeries = true;
        }
        else if (brush1 != null)
        {
          brushList.Add(brush1);
          sfChartSerie.Palette = ChartColorPalette.Custom;
        }
        else if (isPie)
        {
          sfChartSerie.Palette = ChartColorPalette.Metro;
          break;
        }
      }
      if (!flag2)
      {
        borderBrush = (System.Windows.Media.Brush) null;
        borderThickness = 0.0;
      }
    }
    else
    {
      sfChartSerie.Palette = ChartColorPalette.Custom;
      if (brush1 != null)
      {
        brushList.Add(brush1);
      }
      else
      {
        for (int index = 0; index < num; ++index)
        {
          System.Windows.Media.Brush brush3 = !this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, index, num - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(index, serie.ParentSeries.Count, !flag1, false))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
          brushList.Add(brush3);
        }
      }
    }
    if (sfChartSerie.Palette == ChartColorPalette.Custom)
    {
      chartColorModel.CustomBrushes = brushList;
      sfChartSerie.ColorModel = chartColorModel;
    }
    return pieDougnutSeries;
  }

  private bool GetBorderOnCommonSeries(ChartBorderImpl border, ChartSeries sfChartSerie)
  {
    bool flag = true;
    bool borderOnCommonSeries = false;
    if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003)
      flag = false;
    if (border.LinePattern != ExcelChartLinePattern.None)
    {
      if (border.HasLineProperties)
      {
        System.Windows.Media.Brush brushFromBorder = this.GetBrushFromBorder(border);
        if (brushFromBorder != null)
          sfChartSerie.Stroke = brushFromBorder;
      }
      else
      {
        ChartSerieImpl parent = border.FindParent(typeof (ChartSerieImpl)) as ChartSerieImpl;
        System.Drawing.Color color;
        sfChartSerie.Stroke = !flag || !this.TryAndGetFillOrLineColorBasedOnPattern(parent.ParentChart, true, parent.Number, parent.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0, flag ? 1.0 : 0.0)) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
        borderOnCommonSeries = true;
      }
    }
    else
      borderOnCommonSeries = true;
    double num = this.GetBorderThickness(border);
    if (num != 0.0)
      num = num < 1.0 ? 1.0 : num + 1.0;
    sfChartSerie.StrokeThickness = num;
    return borderOnCommonSeries;
  }

  internal void GetBorderOnLineSeries(ChartSerieImpl serie, ChartSeries sfChartSerie)
  {
    bool isXMLVersion = true;
    if (this.parentWorkbook.Version == ExcelVersion.Excel97to2003 && !this.IsChartEx)
      isXMLVersion = false;
    ChartBorderImpl lineProperties = serie.SerieFormat.LineProperties as ChartBorderImpl;
    if (lineProperties.LinePattern != ExcelChartLinePattern.None)
    {
      System.Drawing.Color color;
      System.Windows.Media.Brush brush = lineProperties.IsAutoLineColor ? (!this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, false, serie.Number, serie.ParentSeries.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(serie.Number, serie.ParentSeries.Count, !isXMLVersion, true))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color))) : this.GetBrushFromBorder(lineProperties);
      this.TryAndUpdateSegmentsColorsInLineSeries(serie, sfChartSerie, brush, isXMLVersion);
      if (brush != null)
      {
        sfChartSerie.Palette = ChartColorPalette.Custom;
        sfChartSerie.ColorModel = new ChartColorModel(ChartColorPalette.Custom)
        {
          CustomBrushes = new List<System.Windows.Media.Brush>() { brush }
        };
        if (sfChartSerie.ShowEmptyPoints)
          sfChartSerie.EmptyPointInterior = brush;
      }
      sfChartSerie.StrokeThickness = this.GetBorderThickness(lineProperties);
      double thickness = -1.0;
      if (sfChartSerie.StrokeThickness == 0.0 || sfChartSerie.StrokeThickness == 0.75 && (short) lineProperties.LineWeight == (short) -1 && this.parentWorkbook.IsLoaded && !this.parentWorkbook.IsConverted && isXMLVersion)
        sfChartSerie.StrokeThickness = !this.TryAndGetThicknessBasedOnElement(ChartElementsEnum.DataPointLineThickness, serie.ParentChart, out thickness, new short?()) ? (isXMLVersion ? 2.0 : 1.0) : thickness;
      else if (sfChartSerie.StrokeThickness < 1.0)
        sfChartSerie.StrokeThickness = 1.0;
      if (this.parentWorkbook.IsCreated || this.parentWorkbook.IsConverted || !isXMLVersion || lineProperties.LineWeightString != null || thickness != -1.0)
        return;
      sfChartSerie.StrokeThickness = 2.0;
    }
    else
      sfChartSerie.StrokeThickness = 0.0;
  }

  private bool TryAndUpdateSegmentsColorsInLineSeries(
    ChartSerieImpl serie,
    ChartSeries sfChartSerie,
    System.Windows.Media.Brush brush,
    bool isXMLVersion)
  {
    bool flag1 = false;
    bool flag2 = !this.IsChartEx && this.IsVaryColorSupported(serie);
    ChartDataPointsCollection dataPoints = serie.DataPoints as ChartDataPointsCollection;
    ObservableCollection<ChartPoint> itemsSource = sfChartSerie.ItemsSource as ObservableCollection<ChartPoint>;
    int count = itemsSource.Count;
    if (flag2 || dataPoints.DeninedDPCount > 0)
    {
      for (int index = 0; index < itemsSource.Count; ++index)
      {
        ChartDataPointImpl hashDataPoint = dataPoints.m_hashDataPoints.ContainsKey(index) ? dataPoints.m_hashDataPoints[index] : (ChartDataPointImpl) null;
        if (hashDataPoint != null && hashDataPoint.DataFormatOrNull != null && hashDataPoint.DataFormat.HasLineProperties && !hashDataPoint.DataFormat.LineProperties.IsAutoLineColor)
        {
          if (hashDataPoint.DataFormat.LineProperties.LinePattern == ExcelChartLinePattern.None)
            itemsSource[index].SegmentBrush = (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor((byte) 0, (byte) 0, (byte) 0, 0.0));
          else
            itemsSource[index].SegmentBrush = this.GetBrushFromBorder(hashDataPoint.DataFormat.LineProperties as ChartBorderImpl);
        }
        else if (flag2)
        {
          System.Drawing.Color color;
          System.Windows.Media.Brush brush1 = !this.TryAndGetFillOrLineColorBasedOnPattern(serie.ParentChart, true, index, itemsSource.Count - 1, out color) ? (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(serie.ParentChart.GetChartColor(index, count, !isXMLVersion, true))) : (System.Windows.Media.Brush) new SolidColorBrush(this.SfColor(color));
          itemsSource[index].SegmentBrush = brush1;
        }
      }
    }
    if (itemsSource.Count<ChartPoint>((Func<ChartPoint, bool>) (x => x.SegmentBrush != null)) > 0)
    {
      foreach (ChartPoint chartPoint in (Collection<ChartPoint>) itemsSource)
      {
        if (chartPoint.SegmentBrush == null)
          chartPoint.SegmentBrush = brush;
      }
      sfChartSerie.SegmentColorPath = "SegmentBrush";
      flag1 = true;
    }
    return flag1;
  }

  internal ChartLegendIcon GetChartLegendIcon(ChartSerieDataFormatImpl dataFormat, int serieIndex)
  {
    ChartLegendIcon chartLegendIcon = ChartLegendIcon.InvertedTriangle;
    int num = (int) dataFormat.MarkerStyle;
    if (dataFormat.MarkerFormatOrNull == null || dataFormat.IsAutoMarker)
      num = this.m_def_MarkerType_Array[serieIndex % this.m_def_MarkerType_Array.Length];
    switch (num)
    {
      case 0:
        chartLegendIcon = ChartLegendIcon.None;
        break;
      case 1:
        chartLegendIcon = ChartLegendIcon.Rectangle;
        break;
      case 2:
      case 5:
        chartLegendIcon = ChartLegendIcon.Diamond;
        break;
      case 3:
        chartLegendIcon = ChartLegendIcon.Triangle;
        break;
      case 4:
        chartLegendIcon = ChartLegendIcon.Cross;
        break;
      case 6:
      case 7:
        chartLegendIcon = ChartLegendIcon.StraightLine;
        break;
      case 8:
        chartLegendIcon = ChartLegendIcon.Circle;
        break;
      case 9:
        chartLegendIcon = ChartLegendIcon.Plus;
        break;
    }
    return chartLegendIcon;
  }

  private void ChartSeriesCommon(
    ChartSerieImpl serie,
    ChartSeriesBase chartSeriesBase,
    ObservableCollection<ChartPoint> values,
    bool applyFill,
    out List<int> negativeIndexes)
  {
    if (!this.IsChartEx)
      this.TryAnsEmptyPointDisplayInChart(serie, chartSeriesBase);
    values = this.ListofPoints.ContainsKey(serie.Index) ? this.ListofPoints[serie.Index] : this.GetChartPointsValues(serie, chartSeriesBase.ShowEmptyPoints, chartSeriesBase.EmptyPointValue == EmptyPointValue.Average ? 1 : 0, false, values);
    values = this.TryAndRemoveFirstEmptyPointsOnSeries(serie.SerieType.ToString().Contains("Line"), serie, chartSeriesBase, values);
    chartSeriesBase.ItemsSource = (object) values;
    this.ItemSource = values;
    this.SfSecondaryAxis(serie, serie.ParentChart, chartSeriesBase);
    negativeIndexes = (List<int>) null;
    if (applyFill)
      this.GetFillOnCommonSeries(serie, chartSeriesBase, out negativeIndexes);
    chartSeriesBase.Label = this.Names.ContainsKey(serie.Index) ? this.Names[serie.Index] : this.GetSerieName(serie);
  }

  private ObservableCollection<ChartPoint> TryAndRemoveFirstEmptyPointsOnSeries(
    bool isLine,
    ChartSerieImpl serie,
    ChartSeriesBase chartSeriesBase,
    ObservableCollection<ChartPoint> values)
  {
    switch (serie.ParentChart.DisplayBlanksAs)
    {
      case ExcelChartPlotEmpty.NotPlotted:
      case ExcelChartPlotEmpty.Interpolated:
        IEnumerable<ChartPoint> chartPoints = values.SkipWhile<ChartPoint>((Func<ChartPoint, bool>) (x => double.IsNaN(x.Value) && x.X is DateTime));
        if (!isLine || chartPoints.Count<ChartPoint>() > 1)
          return new ObservableCollection<ChartPoint>(chartPoints);
        chartSeriesBase.ShowEmptyPoints = false;
        return values;
      default:
        return values;
    }
  }

  private void TryAnsEmptyPointDisplayInChart(ChartSerieImpl serie, ChartSeriesBase chartSeriesBase)
  {
    switch (serie.ParentChart.DisplayBlanksAs)
    {
      case ExcelChartPlotEmpty.NotPlotted:
        chartSeriesBase.ShowEmptyPoints = false;
        break;
      case ExcelChartPlotEmpty.Zero:
        chartSeriesBase.ShowEmptyPoints = true;
        chartSeriesBase.EmptyPointValue = EmptyPointValue.Zero;
        break;
      case ExcelChartPlotEmpty.Interpolated:
        string str = serie.SerieType.ToString();
        if (!str.ToLower().Contains("line") && !str.Contains("Radar"))
          break;
        chartSeriesBase.ShowEmptyPoints = true;
        chartSeriesBase.EmptyPointValue = EmptyPointValue.Average;
        break;
    }
  }

  private void ChartSeriesCommon(
    ChartSerieImpl serie,
    ChartSeriesBase chartSeriesBase,
    ObservableCollection<ChartPoint> values,
    out List<int> negativeIndexes)
  {
    this.ChartSeriesCommon(serie, chartSeriesBase, values, true, out negativeIndexes);
  }

  private void ChartSeriesCommon(ChartSerieImpl serie, ChartSeriesBase chartSeriesBase)
  {
    this.ChartSeriesCommon(serie, chartSeriesBase, true);
  }

  private void ChartSeriesCommon(
    ChartSerieImpl serie,
    ChartSeriesBase chartSeriesBase,
    bool applyFill)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    this.ChartSeriesCommon(serie, chartSeriesBase, viewModel.Products, applyFill, out List<int> _);
  }

  private void ChartSeriesCommon(
    ChartSerieImpl serie,
    ChartSeriesBase chartSeriesBase,
    out List<int> negativeIndexes)
  {
    ViewModel viewModel = this.GetViewModel(serie);
    this.ChartSeriesCommon(serie, chartSeriesBase, viewModel.Products, true, out negativeIndexes);
  }

  internal void DetachEventsForRadarAxes(SfChartExt sfChart)
  {
    if (sfChart.PrimaryAxis != null)
      sfChart.PrimaryAxis.AxisBoundsChanged -= new EventHandler<ChartAxisBoundsEventArgs>(((ChartCommon) this).CategoryAxis_AxisBoundsChanged);
    if (!this.SecondayAxisAchived)
      return;
    for (int index = 0; index < sfChart.Series.Count; ++index)
    {
      if (sfChart.Series[index] is RadarSeries radarSeries && radarSeries.XAxis != null)
        radarSeries.XAxis.AxisBoundsChanged -= new EventHandler<ChartAxisBoundsEventArgs>(((ChartCommon) this).CategoryAxis_AxisBoundsChanged);
    }
  }

  internal void TryAndSortLegendItems(ChartBase sfChart, int[] sortedLegendOrders)
  {
    ChartLegend chartLegend = (ChartLegend) null;
    ChartLegend legend1;
    if (sfChart.Legend is ChartLegendCollection)
    {
      ChartLegendCollection legend2 = sfChart.Legend as ChartLegendCollection;
      legend1 = legend2[0];
      chartLegend = legend2[1];
    }
    else
      legend1 = sfChart.Legend as ChartLegend;
    if (sortedLegendOrders == null || sortedLegendOrders.Length != legend1.Items.Count)
      return;
    ObservableCollection<LegendItem> observableCollection = new ObservableCollection<LegendItem>();
    for (int index = 0; index < sortedLegendOrders.Length; ++index)
      observableCollection.Add((legend1.ItemsSource as ObservableCollection<LegendItem>)[sortedLegendOrders[index]]);
    legend1.ItemsSource = (IEnumerable) observableCollection;
    if (chartLegend == null)
    {
      sfChart.Legend = (object) legend1;
    }
    else
    {
      ChartLegendCollection legendCollection = new ChartLegendCollection();
      legendCollection.Add(legend1);
      legendCollection.Add(chartLegend);
      sfChart.Legend = (object) legendCollection;
    }
  }
}
