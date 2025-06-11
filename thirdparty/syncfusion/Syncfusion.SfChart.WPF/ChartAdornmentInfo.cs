// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAdornmentInfo
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public sealed class ChartAdornmentInfo : ChartAdornmentInfoBase
{
  internal Point ConnectorEndPoint { get; set; }

  internal override void Arrange(Size finalSize)
  {
    double num1 = 0.0;
    double num2 = 0.0;
    double radius = 0.0;
    bool flag1 = this.series is CircularSeriesBase series1 && series1.LabelPosition == CircularSeriesLabelPosition.OutsideExtended && series1.EnableSmartLabels;
    this.AdornmentInfoSize = finalSize;
    this.IsStraightConnectorLine2D = series1 != null && series1.ConnectorType == ConnectorMode.StraightLine;
    this.ShowMarkerAtEdge2D = series1 != null && series1.ShowMarkerAtLineEnd;
    int index = 0;
    if (this.LabelPresenters != null && this.LabelPresenters.Count > 0 && series1 != null)
    {
      radius = series1.Radius;
      foreach (ChartPieAdornment chartPieAdornment in this.Series.Adornments.Select<ChartAdornment, ChartPieAdornment>((Func<ChartAdornment, ChartPieAdornment>) (adornment => adornment as ChartPieAdornment)))
      {
        if (chartPieAdornment.ConnectorRotationAngle % (2.0 * Math.PI) <= 1.57 || chartPieAdornment.ConnectorRotationAngle % (2.0 * Math.PI) >= 4.71)
          num1 = Math.Max(num1, this.LabelPresenters[index].DesiredSize.Width);
        else
          num2 = Math.Max(num2, this.LabelPresenters[index].DesiredSize.Width);
        ++index;
      }
      num2 = finalSize.Width - num2;
    }
    int num3 = 0;
    List<Rect> bounds = new List<Rect>();
    bool flag2 = this.Series is DoughnutSeries series2 && series2.IsStackedDoughnut;
    foreach (ChartAdornment visibleAdornment in (Collection<ChartAdornment>) this.series.VisibleAdornments)
    {
      IChartTransformer transformer = this.Series.CreateTransformer(finalSize, false);
      visibleAdornment.Update(transformer);
      ChartPieAdornment chartPieAdornment = visibleAdornment as ChartPieAdornment;
      if (this.adormentContainers != null && num3 < this.adormentContainers.Count)
      {
        ChartAdornmentContainer adormentContainer = this.adormentContainers[num3];
        if (!double.IsNaN(visibleAdornment.YData) && !double.IsNaN(visibleAdornment.XData))
        {
          adormentContainer.Visibility = Visibility.Visible;
          Rect rect1 = new Rect();
          Rect rect2;
          if (flag2)
          {
            DoughnutSegment segment = series2.Segments[num3] as DoughnutSegment;
            double x = adormentContainer.SymbolOffset.X;
            double y = adormentContainer.SymbolOffset.Y;
            rect2 = new Rect(new Point(), adormentContainer.DesiredSize)
            {
              X = series2.Center.X + (chartPieAdornment.Radius - (chartPieAdornment.Radius - chartPieAdornment.InnerDoughnutRadius) / 2.0) * Math.Cos(segment.EndAngle) - x,
              Y = series2.Center.Y + (chartPieAdornment.Radius - (chartPieAdornment.Radius - chartPieAdornment.InnerDoughnutRadius) / 2.0) * Math.Sin(segment.EndAngle) - y
            };
          }
          else
            rect2 = new Rect(new Point(), adormentContainer.DesiredSize)
            {
              X = visibleAdornment.X - adormentContainer.SymbolOffset.X,
              Y = visibleAdornment.Y - adormentContainer.SymbolOffset.Y
            };
          Panel.SetZIndex((UIElement) adormentContainer, 3);
          Canvas.SetLeft((UIElement) adormentContainer, rect2.Left);
          Canvas.SetTop((UIElement) adormentContainer, rect2.Top);
        }
        else
          adormentContainer.Visibility = Visibility.Collapsed;
      }
      if (!flag1)
        this.UpdateLabelPos(flag2 ? chartPieAdornment.Radius : radius, (IList<Rect>) bounds, finalSize, visibleAdornment, num3, num1, num2);
      ++num3;
    }
    if (flag1)
      this.UpdateSpiderLabels(num1, num2, finalSize, radius);
    if (this.ConnectorLines == null)
      return;
    foreach (Path connectorLine in this.ConnectorLines)
    {
      Canvas.SetLeft((UIElement) connectorLine, 0.0);
      Canvas.SetTop((UIElement) connectorLine, 0.0);
    }
  }

  internal override DependencyObject CloneAdornmentInfo()
  {
    ChartAdornmentInfo chartAdornmentInfo = new ChartAdornmentInfo();
    chartAdornmentInfo.ShowLabel = this.ShowLabel;
    chartAdornmentInfo.ShowMarker = this.ShowMarker;
    chartAdornmentInfo.Symbol = this.Symbol;
    chartAdornmentInfo.SymbolHeight = this.SymbolHeight;
    chartAdornmentInfo.SymbolInterior = this.SymbolInterior;
    chartAdornmentInfo.SymbolTemplate = this.SymbolTemplate;
    chartAdornmentInfo.SymbolWidth = this.SymbolWidth;
    chartAdornmentInfo.ShowConnectorLine = this.ShowConnectorLine;
    chartAdornmentInfo.SegmentLabelFormat = this.SegmentLabelFormat;
    chartAdornmentInfo.SegmentLabelContent = this.SegmentLabelContent;
    chartAdornmentInfo.LabelTemplate = this.LabelTemplate;
    chartAdornmentInfo.HorizontalAlignment = this.HorizontalAlignment;
    chartAdornmentInfo.ConnectorLineStyle = this.ConnectorLineStyle;
    chartAdornmentInfo.LabelPosition = this.LabelPosition;
    chartAdornmentInfo.UseSeriesPalette = this.UseSeriesPalette;
    return (DependencyObject) chartAdornmentInfo;
  }

  internal void AlignAdornmentLabelPosition(
    FrameworkElement control,
    AdornmentsLabelPosition labelPosition,
    double x,
    double y,
    int index)
  {
    CircularSeriesBase series = this.Series as CircularSeriesBase;
    Point point = new Point(x, y);
    ChartAdornment adornment = this.Series.Adornments[index];
    double num1 = !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? (!this.IsAdornmentLabelCreatedEventHooked || adornment.CustomAdornmentLabel == null ? this.LabelPadding : adornment.CustomAdornmentLabel.LabelPadding) : 0.0;
    switch (labelPosition)
    {
      case AdornmentsLabelPosition.Auto:
        if (this.Series is StackingColumnSeries || this.Series is StackingBarSeries || this.Series is StackingAreaSeries || this.Series is FastStackingColumnBitmapSeries)
          point = this.AlignInnerLabelPosition(control, index, x, y);
        else if (this.Series is BubbleSeries || this.Series is FinancialSeriesBase || this.Series is TriangularSeriesBase)
        {
          point.X = x - control.DesiredSize.Width / 2.0;
          point.Y = y - control.DesiredSize.Height / 2.0;
        }
        else if (this.Series is LineSeries || this.Series is SplineSeries || this.Series is FastLineSeries || this.Series is StepLineSeries || this.Series is FastStepLineBitmapSeries || this.series is FastLineBitmapSeries)
        {
          if (this.Series.ActualYAxis.IsInversed)
          {
            if ((this.Series as CartesianSeries).IsTransposed)
            {
              point.Y = y - control.DesiredSize.Height / 2.0;
              point.X = this.IsTop(index) ? x - control.DesiredSize.Width - num1 : x + num1;
            }
            else
            {
              point.X = x - control.DesiredSize.Width / 2.0;
              point.Y = this.IsTop(index) ? y + num1 : y - control.DesiredSize.Height - num1;
            }
          }
          else if ((this.Series as CartesianSeries).IsTransposed)
          {
            point.Y = y - control.DesiredSize.Height / 2.0;
            point.X = this.IsTop(index) ? x + num1 : x - control.DesiredSize.Width - num1;
          }
          else
          {
            point.X = x - control.DesiredSize.Width / 2.0;
            point.Y = this.IsTop(index) ? y - control.DesiredSize.Height - num1 : y + num1;
          }
        }
        else if (this.Series is CircularSeriesBase || this.Series is PolarRadarSeriesBase)
        {
          point.X -= control.DesiredSize.Width / 2.0;
          point.Y -= control.DesiredSize.Height / 2.0;
        }
        else
          point = this.AlignOuterLabelPosition(control, index, x, y);
        if (this.Series is TriangularSeriesBase)
        {
          point.X = point.X < 0.0 ? num1 : (point.X + control.DesiredSize.Width > this.Series.GetAvailableSize().Width ? this.Series.GetAvailableSize().Width - control.DesiredSize.Width - num1 : point.X);
          point.X = this.UpdateTriangularSeriesDataLabelPositionForExplodedSegment(index, point.X);
          point.Y = point.Y < 0.0 ? num1 : (point.Y + control.DesiredSize.Height > this.Series.GetAvailableSize().Height ? this.Series.GetAvailableSize().Height - control.DesiredSize.Height - num1 : point.Y);
          break;
        }
        if (series != null || (this.Series.ActualYAxis as ChartAxisBase2D).ZoomFactor >= 1.0 && (this.Series.ActualXAxis as ChartAxisBase2D).ZoomFactor >= 1.0)
        {
          double num2;
          double num3;
          if (this.Series is PolarRadarSeriesBase && this.Series.Segments.Count > 0)
          {
            num2 = this.Series.ActualArea.SeriesClipRect.Height;
            num3 = this.Series.ActualArea.SeriesClipRect.Width;
          }
          else if (series != null)
          {
            if (!series.EnableSmartLabels)
            {
              num2 = series.Center.Y * 2.0;
              num3 = series.Center.X * 2.0;
            }
            else
              break;
          }
          else
          {
            num2 = this.Series.Clip.Bounds.Height;
            num3 = this.Series.Clip.Bounds.Width;
          }
          if ((point.Y < 0.0 || point.Y + control.DesiredSize.Height > num2) && this.Series is ColumnSeries)
            point = this.AlignInnerLabelPosition(control, index, x, y);
          if ((point.X < 0.0 || point.X + control.DesiredSize.Width > num3) && this.Series is BarSeries)
            point = this.AlignInnerLabelPosition(control, index, x, y);
          point.Y = point.Y < 0.0 ? num1 : (point.Y + control.DesiredSize.Height > num2 ? num2 - control.DesiredSize.Height - num1 : point.Y);
          point.X = point.X < 0.0 ? num1 : (point.X + control.DesiredSize.Width > num3 ? num3 - control.DesiredSize.Width - num1 : point.X);
          break;
        }
        break;
      case AdornmentsLabelPosition.Inner:
        if (this.Series is LineSeries || this.Series is SplineSeries || this.Series is FastLineSeries || this.Series is StepLineSeries || this.Series is FastStepLineBitmapSeries || this.Series is FastLineBitmapSeries)
        {
          if (this.Series.ActualYAxis.IsInversed)
          {
            if ((this.Series as CartesianSeries).IsTransposed)
            {
              point.Y = y - control.DesiredSize.Height / 2.0;
              point.X = this.IsTop(index) ? x + num1 : x - control.DesiredSize.Width - num1;
              break;
            }
            point.X = x - control.DesiredSize.Width / 2.0;
            point.Y = this.IsTop(index) ? y - control.DesiredSize.Height - num1 : y + num1;
            break;
          }
          if ((this.Series as CartesianSeries).IsTransposed)
          {
            point.Y = y - control.DesiredSize.Height / 2.0;
            point.X = this.IsTop(index) ? x - control.DesiredSize.Width - num1 : x + num1;
            break;
          }
          point.X = x - control.DesiredSize.Width / 2.0;
          point.Y = this.IsTop(index) ? y + num1 : y - control.DesiredSize.Height - num1;
          break;
        }
        point = this.AlignInnerLabelPosition(control, index, x, y);
        break;
      case AdornmentsLabelPosition.Center:
        point.Y -= control.DesiredSize.Height / 2.0;
        point.X -= control.DesiredSize.Width / 2.0;
        if (this.Series is TriangularSeriesBase)
        {
          point.X = this.UpdateTriangularSeriesDataLabelPositionForExplodedSegment(index, point.X);
          break;
        }
        break;
      default:
        if (this.Series is LineSeries || this.Series is SplineSeries || this.Series is FastLineSeries || this.Series is StepLineSeries || this.Series is FastStepLineBitmapSeries || this.Series is FastLineBitmapSeries)
        {
          if (this.Series.ActualYAxis.IsInversed)
          {
            if ((this.Series as CartesianSeries).IsTransposed)
            {
              point.Y = y - control.DesiredSize.Height / 2.0;
              point.X = this.IsTop(index) ? x - control.DesiredSize.Width - num1 : x + num1;
              break;
            }
            point.X = x - control.DesiredSize.Width / 2.0;
            point.Y = this.IsTop(index) ? y + num1 : y - control.DesiredSize.Height - num1;
            break;
          }
          if ((this.Series as CartesianSeries).IsTransposed)
          {
            point.Y = y - control.DesiredSize.Height / 2.0;
            point.X = this.IsTop(index) ? x + num1 : x - control.DesiredSize.Width - num1;
            break;
          }
          point.X = x - control.DesiredSize.Width / 2.0;
          point.Y = this.IsTop(index) ? y - control.DesiredSize.Height - num1 : y + num1;
          break;
        }
        point = this.AlignOuterLabelPosition(control, index, x, y);
        break;
    }
    Canvas.SetLeft((UIElement) control, point.X);
    Canvas.SetTop((UIElement) control, point.Y);
  }

  protected override void DrawLineSegment(List<Point> points, Path path)
  {
    if (points.Count < 1 || path == null)
      return;
    PathFigure pathFigure = new PathFigure();
    path.Data = (Geometry) new PathGeometry()
    {
      Figures = {
        pathFigure
      }
    };
    pathFigure.StartPoint = points[0];
    PolyLineSegment polyLineSegment = new PolyLineSegment()
    {
      Points = new PointCollection()
    };
    foreach (Point point in points)
      polyLineSegment.Points.Add(point);
    pathFigure.Segments.Add((PathSegment) polyLineSegment);
  }

  private static Point GetMultipleDoughnutLabelPoints(
    DoughnutSeries doughnutSeries,
    DoughnutSegment doughnutSegment,
    double width,
    double height,
    double padding,
    Point center,
    int index,
    double x,
    double y,
    bool isOuter)
  {
    Point doughnutLabelPoints = new Point();
    if (doughnutSeries.LabelPosition == CircularSeriesLabelPosition.Inside)
    {
      doughnutLabelPoints.X = x;
      doughnutLabelPoints.Y = y;
    }
    else
    {
      bool flag = !(doughnutSegment.EndAngle > doughnutSegment.StartAngle ^ isOuter);
      ChartPieAdornment adornment = doughnutSeries.Adornments[index] as ChartPieAdornment;
      double num1 = adornment.Radius - adornment.InnerDoughnutRadius;
      double num2 = adornment.InnerDoughnutRadius + num1 / 2.0;
      double d;
      double a;
      if (doughnutSeries.LabelPosition == CircularSeriesLabelPosition.OutsideExtended)
      {
        d = flag ? doughnutSegment.EndAngle + (width / 2.0 + padding) / num2 : doughnutSegment.EndAngle - (width / 2.0 + padding) / num2;
        a = flag ? doughnutSegment.EndAngle + (height / 2.0 + padding) / num2 : doughnutSegment.EndAngle - (height / 2.0 + padding) / num2;
      }
      else
      {
        d = flag ? doughnutSegment.StartAngle - (width / 2.0 + padding) / num2 : doughnutSegment.StartAngle + (width / 2.0 + padding) / num2;
        a = flag ? doughnutSegment.StartAngle - (height / 2.0 + padding) / num2 : doughnutSegment.StartAngle + (height / 2.0 + padding) / num2;
      }
      doughnutLabelPoints.X = center.X + num2 * Math.Cos(d) - width / 2.0;
      doughnutLabelPoints.Y = center.Y + num2 * Math.Sin(a) - height / 2.0;
    }
    return doughnutLabelPoints;
  }

  private void UpdateLabelPos(
    double pieRadius,
    IList<Rect> bounds,
    Size finalSize,
    ChartAdornment adornment,
    int adornmentIndex,
    double pieLeft,
    double pieRight)
  {
    if (adornment == null)
      return;
    double x = adornment.X;
    double y = adornment.Y;
    FrameworkElement labelPresenter = this.LabelPresenters == null || this.LabelPresenters.Count <= 0 ? (FrameworkElement) null : this.LabelPresenters[adornmentIndex];
    if (labelPresenter != null && this.ShowLabel)
    {
      if (adornment.CanHideLabel || double.IsNaN(x) || double.IsNaN(y))
      {
        labelPresenter.Visibility = Visibility.Collapsed;
        return;
      }
      labelPresenter.Visibility = Visibility.Visible;
    }
    this.GetActualLabelPosition(adornment);
    if (this.ConnectorLines.Count > adornmentIndex)
    {
      if (adornment.CanHideLabel)
        this.ConnectorLines[adornmentIndex].Visibility = Visibility.Collapsed;
      else
        this.ConnectorLines[adornmentIndex].Visibility = Visibility.Visible;
    }
    CircularSeriesBase series = this.series as CircularSeriesBase;
    ChartPieAdornment chartPieAdornment = adornment as ChartPieAdornment;
    AdornmentsLabelPosition adornmentsLabelPosition = !this.IsAdornmentLabelCreatedEventHooked || adornment.CustomAdornmentLabel == null ? this.LabelPosition : adornment.CustomAdornmentLabel.LabelPosition;
    if (this.ShowConnectorLine || series != null && series.EnableSmartLabels)
    {
      ConnectorMode connectorLineMode = series != null ? series.ConnectorType : ConnectorMode.Line;
      bool isPie = chartPieAdornment != null || adornment is ChartPieAdornment3D;
      if (labelPresenter != null && adornmentsLabelPosition != AdornmentsLabelPosition.Default && this.ConnectorHeight > 0.0)
      {
        if (this.Series is BubbleSeries || this.Series is ScatterSeries)
        {
          double num1 = this.Series is BubbleSeries ? (this.Series.Segments[adornmentIndex] as BubbleSegment).SegmentRadius : (this.Series.Segments[adornmentIndex] as ScatterSegment).ScatterHeight / 2.0;
          double num2 = 6.28 * (1.0 - adornment.ConnectorRotationAngle / 360.0);
          AdornmentsPosition adornmentsPosition = this.AdornmentsPosition;
          switch (adornmentsLabelPosition)
          {
            case AdornmentsLabelPosition.Inner:
              if (num1 - labelPresenter.DesiredSize.Height / 2.0 > 0.0)
              {
                if (((this.Series.ActualYAxis.IsInversed ? 1 : 0) ^ (adornment.YData < 0.0 ? 1 : (adornmentsPosition == AdornmentsPosition.Bottom ? 1 : 0))) != 0)
                {
                  x -= Math.Cos(num2) * (num1 - labelPresenter.DesiredSize.Height / 2.0);
                  y -= Math.Sin(num2) * (num1 - labelPresenter.DesiredSize.Height / 2.0);
                  break;
                }
                x += Math.Cos(num2) * (num1 - labelPresenter.DesiredSize.Height / 2.0);
                y += Math.Sin(num2) * (num1 - labelPresenter.DesiredSize.Height / 2.0);
                break;
              }
              break;
            case AdornmentsLabelPosition.Outer:
              if (((this.Series.ActualYAxis.IsInversed ? 1 : 0) ^ (adornment.YData < 0.0 ? 1 : (adornmentsPosition == AdornmentsPosition.Bottom ? 1 : 0))) != 0)
              {
                x -= Math.Cos(num2) * num1;
                y -= Math.Sin(num2) * num1;
                break;
              }
              x += Math.Cos(num2) * num1;
              y += Math.Sin(num2) * num1;
              break;
          }
        }
        else if (this.Series is TriangularSeriesBase)
        {
          double num = !(this.Series is PyramidSeries) ? (this.Series.Segments[adornmentIndex] as FunnelSegment).height : (this.Series.Segments[adornmentIndex] as PyramidSegment).height;
          switch (adornmentsLabelPosition)
          {
            case AdornmentsLabelPosition.Inner:
              y = y + num - labelPresenter.DesiredSize.Height / 2.0;
              break;
            case AdornmentsLabelPosition.Outer:
              y = y - num + labelPresenter.DesiredSize.Height / 2.0;
              break;
          }
        }
      }
      double angle = isPie ? adornment.ConnectorRotationAngle : 6.28 * (1.0 - adornment.ConnectorRotationAngle / 360.0);
      List<Point> adornmentPositions = this.GetAdornmentPositions(pieRadius, bounds, finalSize, adornment, adornmentIndex, pieLeft, pieRight, labelPresenter, (ChartSeriesBase) series, ref x, ref y, angle, isPie);
      this.DrawConnectorLine(adornmentIndex, adornmentPositions, connectorLineMode, false, 0.0);
      if (this.ShowMarkerAtEdge2D && series != null && this.ShowMarker && this.adormentContainers != null && adornmentIndex < this.adormentContainers.Count)
        ChartAdornmentInfoBase.SetSymbolPosition(new Point(adornmentPositions.Last<Point>().X, adornmentPositions.Last<Point>().Y), this.adormentContainers[adornmentIndex]);
    }
    if (!this.ShowLabel || double.IsNaN(y) || labelPresenter == null)
      return;
    if (series != null && series.ConnectorLinePosition == ConnectorLinePosition.Auto && this.IsStraightConnectorLine2D)
    {
      if (series == null)
      {
        Point point = new Point(finalSize.Width / 2.0, finalSize.Height / 2.0);
      }
      else
      {
        Point center = series.Center;
      }
      ChartAdornmentContainer adornmentSymbol = (ChartAdornmentContainer) null;
      if (this.adormentContainers != null && this.adormentContainers.Count > adornmentIndex)
        adornmentSymbol = this.adormentContainers[adornmentIndex];
      this.AlignStraightConnectorLineLabel(series.Center, adornmentSymbol, ref x, ref y, labelPresenter);
      if (series.ConnectorLinePosition == ConnectorLinePosition.Auto)
      {
        Rect rect = new Rect(Canvas.GetLeft((UIElement) labelPresenter), Canvas.GetTop((UIElement) labelPresenter), labelPresenter.DesiredSize.Width, labelPresenter.DesiredSize.Height);
        this.series.Segments[adornmentIndex].GetRenderedVisual();
        double angle = chartPieAdornment.Angle;
        double connectorAngle1 = angle;
        double connectorAngle2 = angle;
        bool flag = true;
        PieSegment segment1 = this.Series.Segments[adornmentIndex] as PieSegment;
        double num3 = this.Series.Segments[adornmentIndex] is DoughnutSegment segment2 ? segment2.StartAngle : segment1.StartAngle;
        double num4 = segment2 != null ? segment2.EndAngle : segment1.EndAngle;
        while (ChartAdornmentInfo.IsLabelCollidedWithSegment(x, y, labelPresenter, series.Center, pieRadius))
        {
          if (flag)
          {
            if (connectorAngle1 + 0.01 > angle && connectorAngle1 + 0.01 < num4)
            {
              connectorAngle1 += 0.01;
              this.UpdateLabelPositionAndConnectorLine(pieRadius, bounds, finalSize, adornment, adornmentIndex, pieLeft, pieRight, labelPresenter, series, ref x, ref y, series.ConnectorType, false, 0, connectorAngle1, true);
              this.AlignStraightConnectorLineLabel(series.Center, adornmentSymbol, ref x, ref y, labelPresenter);
            }
            else
              flag = false;
          }
          else if (connectorAngle2 - 0.01 < angle && connectorAngle2 - 0.01 > num3)
          {
            connectorAngle2 += 0.01;
            this.UpdateLabelPositionAndConnectorLine(pieRadius, bounds, finalSize, adornment, adornmentIndex, pieLeft, pieRight, labelPresenter, series, ref x, ref y, series.ConnectorType, false, 0, connectorAngle2, true);
            this.AlignStraightConnectorLineLabel(series.Center, adornmentSymbol, ref x, ref y, labelPresenter);
          }
          else
          {
            this.UpdateLabelPositionAndConnectorLine(pieRadius, bounds, finalSize, adornment, adornmentIndex, pieLeft, pieRight, labelPresenter, series, ref x, ref y, series.ConnectorType, false, 0, angle, true);
            this.AlignStraightConnectorLineLabel(series.Center, adornmentSymbol, ref x, ref y, labelPresenter);
            break;
          }
        }
      }
      Canvas.SetLeft((UIElement) labelPresenter, x);
      Canvas.SetTop((UIElement) labelPresenter, y);
    }
    else if (this.ShowMarkerAtEdge2D && this.ShowMarker && series != null)
    {
      ChartAdornmentContainer adornmentSymbol = (ChartAdornmentContainer) null;
      if (this.adormentContainers != null && this.adormentContainers.Count > adornmentIndex)
        adornmentSymbol = this.adormentContainers[adornmentIndex];
      Point center = series != null ? series.Center : new Point(finalSize.Width / 2.0, finalSize.Height / 2.0);
      this.AlignStraightConnectorLineLabel(labelPresenter, center, adornmentsLabelPosition, adornmentSymbol, x, y, series.EnableSmartLabels, series.LabelPosition);
    }
    else
    {
      double offsetX = this.OffsetX;
      double offsetY = this.OffsetY;
      if (this.IsAdornmentLabelCreatedEventHooked && adornment.CustomAdornmentLabel != null)
      {
        offsetX = adornment.CustomAdornmentLabel.OffsetX;
        offsetY = adornment.CustomAdornmentLabel.OffsetY;
      }
      if (adornmentsLabelPosition == AdornmentsLabelPosition.Default)
        ChartAdornmentInfoBase.AlignElement(labelPresenter, this.GetChartAlignment(this.VerticalAlignment), this.GetChartAlignment(this.HorizontalAlignment), this.UpdateTriangularSeriesDataLabelPositionForExplodedSegment(adornmentIndex, x), y);
      else
        this.AlignAdornmentLabelPosition(labelPresenter, adornmentsLabelPosition, x + offsetX, y + offsetY, adornmentIndex);
    }
  }

  internal void AlignStraightConnectorLineLabel(
    Point center,
    ChartAdornmentContainer adornmentSymbol,
    ref double x,
    ref double y,
    FrameworkElement label)
  {
    double num1 = 0.0;
    double num2 = 0.0;
    if (adornmentSymbol != null)
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
    x = x > center.X ? x - label.DesiredSize.Width - num1 / 2.0 : x + num1 / 2.0;
    y = y - label.DesiredSize.Height - num2 / 2.0;
  }

  private void UpdateLabelPositionAndConnectorLine(
    double pieRadius,
    IList<Rect> bounds,
    Size finalSize,
    ChartAdornment adornment,
    int adornmentIndex,
    double pieLeft,
    double pieRight,
    FrameworkElement label,
    CircularSeriesBase circularSeriesBase,
    ref double x,
    ref double y,
    ConnectorMode connectorLineMode,
    bool is3D,
    int value,
    double connectorAngle,
    bool isPie)
  {
    List<Point> adornmentPositions = this.GetAdornmentPositions(pieRadius, bounds, finalSize, adornment, adornmentIndex, pieLeft, pieRight, label, (ChartSeriesBase) circularSeriesBase, ref x, ref y, connectorAngle, isPie);
    this.DrawConnectorLine(adornmentIndex, adornmentPositions, connectorLineMode, is3D, (double) value);
    this.ConnectorEndPoint = adornmentPositions[adornmentPositions.Count - 1];
    if (!this.ShowMarkerAtEdge2D || circularSeriesBase == null || !this.ShowMarker || this.adormentContainers == null || adornmentIndex >= this.adormentContainers.Count)
      return;
    ChartAdornmentInfoBase.SetSymbolPosition(new Point(adornmentPositions.Last<Point>().X, adornmentPositions.Last<Point>().Y), this.adormentContainers[adornmentIndex]);
  }

  private static bool IsLabelCollidedWithSegment(
    double x,
    double y,
    FrameworkElement label,
    Point center,
    double pieRadius)
  {
    Rect rect = new Rect(x, y, label.DesiredSize.Width, label.DesiredSize.Height);
    return ChartMath.IsPointInsideCircle(center, pieRadius, new Point(x, y)) || ChartMath.IsPointInsideCircle(center, pieRadius, new Point(x + label.DesiredSize.Width, y)) || ChartMath.IsPointInsideCircle(center, pieRadius, new Point(x, y + label.DesiredSize.Height)) || ChartMath.IsPointInsideCircle(center, pieRadius, new Point(x + label.DesiredSize.Width, y + label.DesiredSize.Height));
  }

  private Point AlignOuterLabelPosition(FrameworkElement control, int index, double x, double y)
  {
    ChartAdornment adornment = this.Series.Adornments[index];
    double padding = !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? (!this.IsAdornmentLabelCreatedEventHooked || adornment.CustomAdornmentLabel == null ? this.LabelPadding : adornment.CustomAdornmentLabel.LabelPadding) : 0.0;
    double height = control.DesiredSize.Height;
    double width = control.DesiredSize.Width;
    if (this.Series is ScatterSeries)
    {
      ScatterSegment segment = this.Series.Segments[index] as ScatterSegment;
      padding += !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? segment.ScatterHeight / 2.0 : 0.0;
    }
    else if (this.Series is BubbleSeries)
    {
      BubbleSegment segment = this.Series.Segments[index] as BubbleSegment;
      padding += !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? segment.SegmentRadius : 0.0;
    }
    if (this.Series is CircularSeriesBase)
    {
      CircularSeriesBase series1 = this.Series as CircularSeriesBase;
      Point center = series1.Center;
      PieSegment segment1 = this.Series.Segments[index] as PieSegment;
      DoughnutSegment segment2 = this.Series.Segments[index] as DoughnutSegment;
      double num = this.Series is PieSeries ? (segment1.StartAngle + segment1.EndAngle) / 2.0 : (segment2.StartAngle + segment2.EndAngle) / 2.0;
      x -= width / 2.0;
      y -= height / 2.0;
      if (!series1.EnableSmartLabels)
      {
        if (series1.LabelPosition == CircularSeriesLabelPosition.OutsideExtended && this.ShowConnectorLine)
          x = x > center.X ? x + width / 2.0 : x - width / 2.0;
        else if (this.Series is DoughnutSeries series2 && series2.IsStackedDoughnut)
        {
          Point doughnutLabelPoints = ChartAdornmentInfo.GetMultipleDoughnutLabelPoints(series2, segment2, width, height, padding, center, index, x, y, true);
          x = doughnutLabelPoints.X;
          y = doughnutLabelPoints.Y;
        }
        else
        {
          x += Math.Cos(num) * (width / 2.0 + padding);
          y += Math.Sin(num) * (height / 2.0 + padding);
        }
      }
    }
    else if (this.Series is PyramidSeries)
    {
      x -= width / 2.0;
      x = this.UpdateTriangularSeriesDataLabelPositionForExplodedSegment(index, x);
      PyramidSegment segment = this.Series.Segments[index] as PyramidSegment;
      y = y + padding - (this.ConnectorHeight <= 0.0 ? segment.height : 0.0);
    }
    else if (this.Series is FunnelSeries)
    {
      x -= width / 2.0;
      x = this.UpdateTriangularSeriesDataLabelPositionForExplodedSegment(index, x);
      if (index < this.Series.Segments.Count)
      {
        FunnelSegment segment = this.Series.Segments[index] as FunnelSegment;
        y = y - padding + (this.ConnectorHeight <= 0.0 ? segment.height : 0.0) - height;
      }
    }
    else if (this.Series is PolarRadarSeriesBase)
    {
      if (!this.ShowConnectorLine)
      {
        Point vector = ChartTransform.ValueToVector(this.Series.ActualXAxis, (this.Series as AdornmentSeries).Adornments[index].XData);
        x = width / 2.0 * vector.X + x - width / 2.0 + padding;
        y = height / 2.0 * vector.Y + y - height / 2.0 + padding;
      }
      else
      {
        x -= width / 2.0;
        y -= height / 2.0;
        PolarRadarSeriesBase series = this.Series as PolarRadarSeriesBase;
        double num1 = series.Area.SeriesClipRect.Width / 2.0;
        double num2 = series.Area.SeriesClipRect.Height / 2.0;
        double num3 = num1 - width / 2.0;
        double num4 = num2 - height / 2.0;
        bool flag1 = x < num3;
        bool flag2 = y > num4;
        if (x == num3)
          y = flag2 ? y + height / 2.0 + padding : y - height / 2.0 - padding;
        else if (y == num4)
          x = flag1 ? x - width / 2.0 - padding : x + width / 2.0 + padding;
        else if (flag1)
        {
          x = x - width / 2.0 - padding;
          y = flag2 ? y + height / 2.0 + padding : y - height / 2.0 - padding;
        }
        else
        {
          x = x + width / 2.0 + padding;
          y = flag2 ? y + height / 2.0 + padding : y - height / 2.0 - padding;
        }
      }
    }
    else
    {
      if (this.series is RangeColumnSeries && !this.series.IsMultipleYPathRequired)
      {
        x -= width / 2.0;
        y -= height / 2.0;
        return new Point(x, y);
      }
      switch ((this.Series as AdornmentSeries).Adornments[index].ActualLabelPosition)
      {
        case ActualLabelPosition.Top:
          y = y - height - padding;
          x -= width / 2.0;
          break;
        case ActualLabelPosition.Left:
          x = x - width - padding;
          y -= height / 2.0;
          break;
        case ActualLabelPosition.Right:
          x += padding;
          y -= height / 2.0;
          break;
        case ActualLabelPosition.Bottom:
          y += padding;
          x -= width / 2.0;
          break;
      }
    }
    return new Point(x, y);
  }

  private Point AlignInnerLabelPosition(FrameworkElement control, int index, double x, double y)
  {
    ChartAdornment adornment = this.Series.Adornments[index];
    double padding = !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? (!this.IsAdornmentLabelCreatedEventHooked || adornment.CustomAdornmentLabel == null ? this.LabelPadding : adornment.CustomAdornmentLabel.LabelPadding) : 0.0;
    double height = control.DesiredSize.Height;
    double width = control.DesiredSize.Width;
    if (this.Series is ScatterSeries)
    {
      ScatterSegment segment = this.Series.Segments[index] as ScatterSegment;
      padding -= !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? segment.ScatterHeight / 2.0 : 0.0;
    }
    else if (this.Series is BubbleSeries)
    {
      BubbleSegment segment = this.Series.Segments[index] as BubbleSegment;
      padding -= !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? segment.SegmentRadius : 0.0;
    }
    if (this.Series is CircularSeriesBase)
    {
      CircularSeriesBase series1 = this.series as CircularSeriesBase;
      Point center = series1.Center;
      PieSegment segment1 = this.Series.Segments[index] as PieSegment;
      DoughnutSegment segment2 = this.Series.Segments[index] as DoughnutSegment;
      double num = this.Series is PieSeries ? (segment1.StartAngle + segment1.EndAngle) / 2.0 : (segment2.StartAngle + segment2.EndAngle) / 2.0;
      x -= width / 2.0;
      y -= height / 2.0;
      if (!series1.EnableSmartLabels)
      {
        if (series1.LabelPosition == CircularSeriesLabelPosition.OutsideExtended && this.ShowConnectorLine)
          x = x > center.X ? x - width / 2.0 : x + width / 2.0;
        else if (this.Series is DoughnutSeries series2 && series2.IsStackedDoughnut)
        {
          Point doughnutLabelPoints = ChartAdornmentInfo.GetMultipleDoughnutLabelPoints(series2, segment2, width, height, padding, center, index, x, y, false);
          x = doughnutLabelPoints.X;
          y = doughnutLabelPoints.Y;
        }
        else
        {
          x -= Math.Cos(num) * (width / 2.0 + padding);
          y -= Math.Sin(num) * (height / 2.0 + padding);
        }
      }
    }
    else if (this.Series is PyramidSeries)
    {
      x -= width / 2.0;
      x = this.UpdateTriangularSeriesDataLabelPositionForExplodedSegment(index, x);
      PyramidSegment segment = this.Series.Segments[index] as PyramidSegment;
      y = y - padding + (this.ConnectorHeight <= 0.0 ? segment.height : 0.0) - height;
    }
    else if (this.Series is FunnelSeries)
    {
      x -= width / 2.0;
      x = this.UpdateTriangularSeriesDataLabelPositionForExplodedSegment(index, x);
      if (index < this.Series.Segments.Count)
      {
        FunnelSegment segment = this.Series.Segments[index] as FunnelSegment;
        y = y - padding + (this.ConnectorHeight <= 0.0 ? segment.height : 0.0) - height;
      }
    }
    else if (this.Series is PolarRadarSeriesBase)
    {
      if (!this.ShowConnectorLine)
      {
        Point vector = ChartTransform.ValueToVector(this.Series.ActualXAxis, (this.Series as AdornmentSeries).Adornments[index].XData);
        x = -width / 2.0 * vector.X + x - width / 2.0 + padding;
        y = -height / 2.0 * vector.Y + y - height / 2.0 + padding;
      }
      else
      {
        x -= width / 2.0;
        y -= height / 2.0;
        PolarRadarSeriesBase series = this.Series as PolarRadarSeriesBase;
        double num1 = series.Area.SeriesClipRect.Width / 2.0;
        double num2 = series.Area.SeriesClipRect.Height / 2.0;
        double num3 = num1 - width / 2.0;
        double num4 = num2 - height / 2.0;
        bool flag1 = x < num3;
        bool flag2 = y > num4;
        if (x == num3)
          y = flag2 ? y - height / 2.0 - padding : y + height / 2.0 + padding;
        else if (y == num4)
          x = flag1 ? x + width / 2.0 + padding : x - width / 2.0 - padding;
        else if (flag1)
        {
          x = x + width / 2.0 + padding;
          y = flag2 ? y - height / 2.0 - padding : y + height / 2.0 + padding;
        }
        else
        {
          x = x - width / 2.0 - padding;
          y = flag2 ? y - height / 2.0 - padding : y + height / 2.0 + padding;
        }
      }
    }
    else
    {
      if (this.series is RangeColumnSeries && !this.series.IsMultipleYPathRequired)
      {
        x -= width / 2.0;
        y -= height / 2.0;
        return new Point(x, y);
      }
      switch ((this.Series as AdornmentSeries).Adornments[index].ActualLabelPosition)
      {
        case ActualLabelPosition.Top:
          y += padding;
          x -= width / 2.0;
          break;
        case ActualLabelPosition.Left:
          x += padding;
          y -= height / 2.0;
          break;
        case ActualLabelPosition.Right:
          x = x - width - padding;
          y -= height / 2.0;
          break;
        case ActualLabelPosition.Bottom:
          y = y - height - padding;
          x -= width / 2.0;
          break;
      }
    }
    return new Point(x, y);
  }
}
