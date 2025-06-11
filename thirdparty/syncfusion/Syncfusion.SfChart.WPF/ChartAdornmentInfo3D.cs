// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAdornmentInfo3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public sealed class ChartAdornmentInfo3D : ChartAdornmentInfoBase
{
  private double depth;
  private int index;
  private Graphics3D graphics3D;

  internal override void Arrange(Size finalSize)
  {
    this.graphics3D = ((SfChart3D) this.series.ActualArea).Graphics3D;
    if (this.series is CircularSeriesBase3D)
      (this.series as CircularSeriesBase3D).Area.IsAutoDepth = true;
    double num1 = 0.0;
    double num2 = 0.0;
    double num3 = 0.0;
    bool flag = this.series is CircularSeriesBase3D series && series.LabelPosition == CircularSeriesLabelPosition.OutsideExtended && series.EnableSmartLabels;
    this.AdornmentInfoSize = finalSize;
    int index = 0;
    if (this.LabelPresenters != null && this.LabelPresenters.Count > 0 && series != null)
    {
      foreach (ChartPieAdornment3D chartPieAdornment3D in this.Series.Adornments.Select<ChartAdornment, ChartPieAdornment3D>((Func<ChartAdornment, ChartPieAdornment3D>) (adornment => adornment as ChartPieAdornment3D)))
      {
        if (chartPieAdornment3D.ConnectorRotationAngle % (2.0 * Math.PI) <= 1.57 || chartPieAdornment3D.ConnectorRotationAngle % (2.0 * Math.PI) >= 4.71)
          num1 = Math.Max(num1, this.LabelPresenters[index].DesiredSize.Width);
        else
          num2 = Math.Max(num2, this.LabelPresenters[index].DesiredSize.Width);
        ++index;
      }
      if (this.series.Adornments.Count > 0)
        num3 = (this.series.Adornments[0] as ChartPieAdornment3D).Radius;
      num2 = finalSize.Width - num2;
    }
    int num4 = 0;
    List<Rect> bounds = new List<Rect>();
    foreach (ChartAdornment adornment in (Collection<ChartAdornment>) this.series.Adornments)
    {
      IChartTransformer transformer = this.series.CreateTransformer(finalSize, false);
      adornment.Update(transformer);
      ChartAdornment3D chartAdornment3D = adornment as ChartAdornment3D;
      if (!(this.Series is CircularSeriesBase3D))
      {
        double left = this.Series.ActualArea.SeriesClipRect.Left;
        double top = this.Series.ActualArea.SeriesClipRect.Top;
        double bottom = this.Series.ActualArea.SeriesClipRect.Bottom;
        double right = this.Series.ActualArea.SeriesClipRect.Right;
        double num5 = 0.0;
        double actualDepth = (this.Series.ActualArea as SfChart3D).ActualDepth;
        double actualStartDepth = chartAdornment3D.ActualStartDepth;
        if (this.adormentContainers != null && (adornment.Y > bottom || adornment.Y < top) || adornment.X > right || adornment.X < left || actualStartDepth < num5 || actualStartDepth > actualDepth)
        {
          ++num4;
          continue;
        }
      }
      if (this.adormentContainers != null && num4 < this.adormentContainers.Count && !double.IsNaN(adornment.Y) && !double.IsNaN(adornment.X))
      {
        ChartAdornmentContainer adormentContainer = this.adormentContainers[num4];
        double num6 = adornment.X - adormentContainer.SymbolOffset.X;
        double num7 = adornment.Y - adormentContainer.SymbolOffset.Y;
        adormentContainer.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        Size desiredSize = adormentContainer.DesiredSize;
        double num8 = num6 + (desiredSize.Width == 0.0 ? 0.0 : desiredSize.Width / 2.0);
        double vy = num7 + (desiredSize.Height == 0.0 ? 0.0 : desiredSize.Height / 2.0);
        double actualStartDepth = chartAdornment3D.ActualStartDepth;
        if (this.Series is CircularSeriesBase3D || this.Series is LineSeries3D || this.Series is AreaSeries3D)
        {
          ((ChartSeries3D) this.series).Area.Graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(num8, vy, actualStartDepth), (UIElement) adormentContainer, 0.0, -adormentContainer.DesiredSize.Height, true, UIElementLeftShift.LeftHalfShift, UIElementTopShift.TopHalfShift));
        }
        else
        {
          double angle1 = this.ModifyDepthToAngle(actualStartDepth, (UIElement) adormentContainer, index);
          double angle2 = this.ModifyLeftToAngle(num8, (UIElement) adormentContainer);
          double actualRotationAngle = (this.Series.ActualArea as SfChart3D).ActualRotationAngle;
          if (actualRotationAngle >= 315.0 || actualRotationAngle < 45.0 || actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
            ((ChartSeries3D) this.series).Area.Graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(angle2, vy, angle1), (UIElement) adormentContainer, 0.0, -adormentContainer.DesiredSize.Height, true, UIElementLeftShift.LeftHalfShift, UIElementTopShift.TopHalfShift));
          else
            ((ChartSeries3D) this.series).Area.Graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(angle2, vy, angle1), (UIElement) adormentContainer, 0.0, -adormentContainer.DesiredSize.Height, false, UIElementLeftShift.LeftHalfShift, UIElementTopShift.TopHalfShift));
        }
      }
      if (!flag)
        this.UpdateLabelPos(num3, (IList<Rect>) bounds, finalSize, adornment, num4, num1, num2);
      ++num4;
    }
    if (flag)
      this.UpdateSpiderLabels(num1, num2, finalSize, num3);
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
    ChartAdornmentInfo3D chartAdornmentInfo3D = new ChartAdornmentInfo3D();
    chartAdornmentInfo3D.ShowLabel = this.ShowLabel;
    chartAdornmentInfo3D.ShowMarker = this.ShowMarker;
    chartAdornmentInfo3D.Symbol = this.Symbol;
    chartAdornmentInfo3D.SymbolHeight = this.SymbolHeight;
    chartAdornmentInfo3D.SymbolInterior = this.SymbolInterior;
    chartAdornmentInfo3D.SymbolStroke = this.SymbolStroke;
    chartAdornmentInfo3D.SymbolTemplate = this.SymbolTemplate;
    chartAdornmentInfo3D.SymbolWidth = this.SymbolWidth;
    chartAdornmentInfo3D.ShowConnectorLine = this.ShowConnectorLine;
    chartAdornmentInfo3D.SegmentLabelFormat = this.SegmentLabelFormat;
    chartAdornmentInfo3D.SegmentLabelContent = this.SegmentLabelContent;
    chartAdornmentInfo3D.LabelTemplate = this.LabelTemplate;
    chartAdornmentInfo3D.HorizontalAlignment = this.HorizontalAlignment;
    chartAdornmentInfo3D.ConnectorLineStyle = this.ConnectorLineStyle;
    chartAdornmentInfo3D.LabelRotationAngle = this.LabelRotationAngle;
    chartAdornmentInfo3D.UseSeriesPalette = this.UseSeriesPalette;
    return (DependencyObject) chartAdornmentInfo3D;
  }

  internal void AddLabel(UIElement element, double x, double y, double depth, int index)
  {
    if (this.Series is CircularSeriesBase3D || this.Series is LineSeries3D || this.Series is AreaSeries3D)
    {
      this.graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(x, y, depth - 1.0), element, 0.0, -element.DesiredSize.Height, true, UIElementLeftShift.LeftHalfShift, UIElementTopShift.TopHalfShift));
    }
    else
    {
      double angle1 = this.ModifyDepthToAngle(depth, element, index);
      double angle2 = this.ModifyLeftToAngle(x, element);
      double actualRotationAngle = (this.Series.ActualArea as SfChart3D).ActualRotationAngle;
      if (actualRotationAngle >= 315.0 || actualRotationAngle < 45.0 || actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
        this.graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(angle2, y, angle1), element, 0.0, -element.DesiredSize.Height, true, UIElementLeftShift.LeftHalfShift, UIElementTopShift.TopHalfShift));
      else
        this.graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(angle2, y, angle1), element, 0.0, -element.DesiredSize.Height, false, UIElementLeftShift.LeftHalfShift, UIElementTopShift.TopHalfShift));
    }
  }

  internal void DrawLineSegment3D(
    List<Point> points,
    Path path,
    double actualDepth,
    int CurrentIndex)
  {
    this.depth = actualDepth;
    this.index = CurrentIndex;
    this.DrawLineSegment(points, path);
  }

  internal void AlignAdornmentLabelPosition3D(
    FrameworkElement control,
    AdornmentsLabelPosition labelPosition,
    ref double actualX,
    ref double actualY,
    int index)
  {
    if (!(this.Series is ChartSeries3D) || double.IsNaN((this.Series as ChartSeries3D).Adornments[index].YData) || control == null)
      return;
    double num1 = !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? this.LabelPadding : 0.0;
    switch (labelPosition)
    {
      case AdornmentsLabelPosition.Auto:
        if (this.Series is StackingColumnSeries3D || this.Series is StackingBarSeries3D)
          this.AlignInnerLabelPosition3D(control, index, ref actualX, ref actualY);
        else if (this.Series is LineSeries3D)
        {
          if (this.Series.ActualYAxis.IsInversed)
          {
            if ((this.Series as CartesianSeries3D).IsActualTransposed)
              actualX = this.IsTop(index) ? actualX - control.DesiredSize.Width + num1 : actualX + control.DesiredSize.Width + num1;
            else
              actualY = this.IsTop(index) ? actualY + control.DesiredSize.Height + num1 : actualY - control.DesiredSize.Height - num1;
          }
          else if ((this.Series as CartesianSeries3D).IsActualTransposed)
            actualX = this.IsTop(index) ? actualX + control.DesiredSize.Width + num1 : actualX - control.DesiredSize.Width - num1;
          else
            actualY = this.IsTop(index) ? actualY - control.DesiredSize.Height + num1 : actualY + control.DesiredSize.Height + num1;
        }
        else if (!(this.Series is CircularSeriesBase3D))
          this.AlignOuterLabelPosition3D(control, index, ref actualX, ref actualY);
        double num2 = this.Series is CircularSeriesBase3D ? (this.Series as CircularSeriesBase3D).Center.Y * 2.0 : (this.series.IsActualTransposed ? this.series.ActualXAxis.RenderedRect.Height : this.series.ActualYAxis.RenderedRect.Height);
        double num3 = this.Series is CircularSeriesBase3D ? (this.Series as CircularSeriesBase3D).Center.X * 2.0 : (this.series.IsActualTransposed ? this.series.ActualYAxis.RenderedRect.Right : this.series.ActualXAxis.RenderedRect.Right);
        if ((actualY < 0.0 || actualY + control.DesiredSize.Height > num2) && this.Series is ColumnSeries3D)
          this.AlignInnerLabelPosition3D(control, index, ref actualX, ref actualY);
        if ((actualX < 0.0 || actualX + control.DesiredSize.Width > num3) && this.Series is BarSeries3D)
          this.AlignInnerLabelPosition3D(control, index, ref actualX, ref actualY);
        actualY = actualY < 0.0 ? num1 : (actualY + control.DesiredSize.Height / 2.0 > num2 ? num2 - control.DesiredSize.Height / 2.0 - num1 : actualY);
        actualX = actualX < 0.0 ? num1 : (actualX + control.DesiredSize.Width / 2.0 > num3 ? num3 - control.DesiredSize.Width / 2.0 - num1 : actualX);
        break;
      case AdornmentsLabelPosition.Inner:
        if (this.Series is LineSeries3D)
        {
          if (this.Series.ActualYAxis.IsInversed)
          {
            if ((this.Series as CartesianSeries3D).IsActualTransposed)
            {
              actualX = this.IsTop(index) ? actualX + control.DesiredSize.Width + num1 : actualX - control.DesiredSize.Width - num1;
              break;
            }
            actualY = this.IsTop(index) ? actualY - control.DesiredSize.Height + num1 : actualY + control.DesiredSize.Height + num1;
            break;
          }
          if ((this.Series as CartesianSeries3D).IsActualTransposed)
          {
            actualX = this.IsTop(index) ? actualX - control.DesiredSize.Width + num1 : actualX + control.DesiredSize.Width + num1;
            break;
          }
          actualY = this.IsTop(index) ? actualY + control.DesiredSize.Height + num1 : actualY - control.DesiredSize.Height - num1;
          break;
        }
        this.AlignInnerLabelPosition3D(control, index, ref actualX, ref actualY);
        break;
      case AdornmentsLabelPosition.Outer:
        if (this.Series is LineSeries3D)
        {
          if (this.Series.ActualYAxis.IsInversed)
          {
            if ((this.Series as CartesianSeries3D).IsActualTransposed)
            {
              actualX = this.IsTop(index) ? actualX - control.DesiredSize.Width + num1 : actualX + control.DesiredSize.Width + num1;
              break;
            }
            actualY = this.IsTop(index) ? actualY + control.DesiredSize.Height + num1 : actualY - control.DesiredSize.Height - num1;
            break;
          }
          if ((this.Series as CartesianSeries3D).IsActualTransposed)
          {
            actualX = this.IsTop(index) ? actualX + control.DesiredSize.Width + num1 : actualX - control.DesiredSize.Width - num1;
            break;
          }
          actualY = this.IsTop(index) ? actualY - control.DesiredSize.Height + num1 : actualY + control.DesiredSize.Height + num1;
          break;
        }
        this.AlignOuterLabelPosition3D(control, index, ref actualX, ref actualY);
        break;
    }
  }

  protected override void DrawLineSegment(List<Point> points, Path path)
  {
    double x = 0.0;
    double angle1 = this.ModifyDepthToAngle(this.depth, (UIElement) path, this.index);
    double angle2 = this.ModifyLeftToAngle(x, (UIElement) path);
    for (int index = 0; index < points.Count; ++index)
      points[index] = new Point(points[index].X + angle2, points[index].Y);
    double actualRotationAngle = (this.Series.ActualArea as SfChart3D).ActualRotationAngle;
    if (this.Series is CircularSeriesBase3D || this.Series is LineSeries3D || this.Series is AreaSeries3D)
      this.graphics3D.AddVisual((Polygon3D) Polygon3D.CreatePolyline(points.Get3DVector(this.depth), path, true));
    else if (actualRotationAngle >= 315.0 || actualRotationAngle < 45.0 || actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
      this.graphics3D.AddVisual((Polygon3D) Polygon3D.CreatePolyline(points.Get3DVector(angle1), path, true));
    else
      this.graphics3D.AddVisual((Polygon3D) Polygon3D.CreatePolyline(points.Get3DVector(angle1), path, false));
    base.DrawLineSegment(points, path);
  }

  private void AlignOuterLabelPosition3D(
    FrameworkElement control,
    int index,
    ref double actualX,
    ref double actualY)
  {
    double height = control.DesiredSize.Height;
    double width = control.DesiredSize.Width;
    double num = !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? this.LabelPadding : (this.Series is BarSeries3D ? width / 2.0 : height / 2.0);
    if (this.Series is ScatterSeries3D)
    {
      ScatterSegment3D segment = this.Series.Segments[index] as ScatterSegment3D;
      num -= !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? segment.ScatterHeight / 2.0 : 0.0;
    }
    if (this.Series is CircularSeriesBase3D)
    {
      CircularSeriesBase3D series = this.Series as CircularSeriesBase3D;
      Point center = series.Center;
      double connectorRotationAngle = series.Adornments[index].ConnectorRotationAngle;
      if (series.EnableSmartLabels)
        return;
      if (series.LabelPosition == CircularSeriesLabelPosition.OutsideExtended && this.ShowConnectorLine)
      {
        actualX = actualX > center.X ? actualX + width / 2.0 : actualX - width / 2.0;
      }
      else
      {
        actualX += Math.Cos(connectorRotationAngle) * (width / 2.0);
        actualY += Math.Sin(connectorRotationAngle) * (height / 2.0);
      }
    }
    else
    {
      switch (this.Series is ChartSeries3D ? (int) (this.Series as ChartSeries3D).Adornments[index].ActualLabelPosition : (int) (this.Series as AdornmentSeries).Adornments[index].ActualLabelPosition)
      {
        case 0:
          actualY = actualY - height + num;
          break;
        case 1:
          actualX = actualX - width + num;
          break;
        case 2:
          actualX = actualX + width - num;
          break;
        case 3:
          actualY = actualY + height - num;
          break;
      }
    }
  }

  private void AlignInnerLabelPosition3D(
    FrameworkElement control,
    int index,
    ref double actualX,
    ref double actualY)
  {
    double height = control.DesiredSize.Height;
    double width = control.DesiredSize.Width;
    double num = !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? this.LabelPadding : (this.Series is BarSeries3D ? width / 2.0 : height / 2.0);
    if (this.Series is ScatterSeries3D)
    {
      ScatterSegment3D segment = this.Series.Segments[index] as ScatterSegment3D;
      num += !this.ShowConnectorLine || this.ConnectorHeight <= 0.0 ? segment.ScatterHeight / 2.0 : 0.0;
    }
    if (this.Series is CircularSeriesBase3D)
    {
      CircularSeriesBase3D series = this.series as CircularSeriesBase3D;
      Point center = series.Center;
      double connectorRotationAngle = series.Adornments[index].ConnectorRotationAngle;
      if (series.EnableSmartLabels)
        return;
      if (series.LabelPosition == CircularSeriesLabelPosition.OutsideExtended && this.ShowConnectorLine)
      {
        actualX = actualX > center.X ? actualX - width / 2.0 : actualX + width / 2.0;
      }
      else
      {
        actualX -= Math.Cos(connectorRotationAngle) * (width / 2.0);
        actualY -= Math.Sin(connectorRotationAngle) * (height / 2.0);
      }
    }
    else
    {
      switch (this.Series is ChartSeries3D ? (int) (this.Series as ChartSeries3D).Adornments[index].ActualLabelPosition : (int) (this.Series as AdornmentSeries).Adornments[index].ActualLabelPosition)
      {
        case 0:
          actualY = actualY + height - num;
          break;
        case 1:
          actualX = actualX + width - num;
          break;
        case 2:
          actualX = actualX - width + num;
          break;
        case 3:
          actualY = actualY - height + num;
          break;
      }
    }
  }

  private void UpdateLabelPos(
    double pieRadius,
    IList<Rect> bounds,
    Size finalSize,
    ChartAdornment adornment,
    int labelIndex,
    double pieLeft,
    double pieRight)
  {
    if (adornment == null)
      return;
    ChartAdornment3D chartAdornment3D = adornment as ChartAdornment3D;
    double x = adornment.X;
    double y = adornment.Y;
    FrameworkElement labelPresenter = this.LabelPresenters == null || this.LabelPresenters.Count <= 0 ? (FrameworkElement) null : this.LabelPresenters[labelIndex];
    if (labelPresenter != null && (adornment.CanHideLabel || double.IsNaN(x) || double.IsNaN(y)))
    {
      labelPresenter.Visibility = Visibility.Collapsed;
    }
    else
    {
      this.GetActualLabelPosition(adornment);
      if (this.ConnectorLines.Count > labelIndex)
        this.ConnectorLines[labelIndex].Visibility = Visibility.Visible;
      AdornmentsPosition adornmentsPosition = this.AdornmentsPosition;
      CircularSeriesBase3D series1 = this.series as CircularSeriesBase3D;
      if (this.ShowConnectorLine || series1 != null && series1.EnableSmartLabels && this.ShowLabel)
      {
        ConnectorMode connectorLineMode = this.series is CircularSeriesBase3D ? ((CircularSeriesBase3D) this.series).ConnectorType : ConnectorMode.Line;
        if (labelPresenter != null && this.LabelPosition != AdornmentsLabelPosition.Default && this.Series is ScatterSeries3D && this.ConnectorHeight > 0.0)
        {
          double num1 = 6.28 * (1.0 - adornment.ConnectorRotationAngle / 360.0);
          double num2 = (this.Series.Segments[labelIndex] as ScatterSegment3D).ScatterHeight / 2.0;
          if (this.LabelPosition == AdornmentsLabelPosition.Outer)
          {
            if (((this.Series.ActualYAxis.IsInversed ? 1 : 0) ^ (adornment.YData < 0.0 ? 1 : (adornmentsPosition == AdornmentsPosition.Bottom ? 1 : 0))) != 0)
            {
              x -= Math.Cos(num1) * num2;
              y -= Math.Sin(num1) * num2;
            }
            else
            {
              x += Math.Cos(num1) * num2;
              y += Math.Sin(num1) * num2;
            }
          }
          else if (this.LabelPosition == AdornmentsLabelPosition.Inner && num2 - labelPresenter.DesiredSize.Height / 2.0 > 0.0)
          {
            if (((this.Series.ActualYAxis.IsInversed ? 1 : 0) ^ (adornment.YData < 0.0 ? 1 : (adornmentsPosition == AdornmentsPosition.Bottom ? 1 : 0))) != 0)
            {
              x -= Math.Cos(num1) * (num2 - labelPresenter.DesiredSize.Height / 2.0);
              y -= Math.Sin(num1) * (num2 - labelPresenter.DesiredSize.Height / 2.0);
            }
            else
            {
              x += Math.Cos(num1) * (num2 - labelPresenter.DesiredSize.Height / 2.0);
              y += Math.Sin(num1) * (num2 - labelPresenter.DesiredSize.Height / 2.0);
            }
          }
        }
        bool isPie = adornment is ChartPieAdornment || adornment is ChartPieAdornment3D;
        double angle = isPie ? adornment.ConnectorRotationAngle : 6.28 * (1.0 - adornment.ConnectorRotationAngle / 360.0);
        List<Point> adornmentPositions = this.GetAdornmentPositions(pieRadius, bounds, finalSize, adornment, labelIndex, pieLeft, pieRight, labelPresenter, (ChartSeriesBase) series1, ref x, ref y, angle, isPie);
        this.DrawConnectorLine(labelIndex, adornmentPositions, connectorLineMode, true, chartAdornment3D.ActualStartDepth);
      }
      if (!this.ShowLabel || double.IsNaN(y) || labelPresenter == null)
        return;
      double actualX = x + this.OffsetX;
      double actualY = y + this.OffsetY;
      if (this.LabelPosition == AdornmentsLabelPosition.Default && (this.Series is ColumnSeries3D || this.Series is BarSeries3D) && !this.ShowConnectorLine && adornmentsPosition != AdornmentsPosition.TopAndBottom)
      {
        switch (adornment.ActualLabelPosition)
        {
          case ActualLabelPosition.Top:
            actualY -= labelPresenter.DesiredSize.Height / 2.0;
            break;
          case ActualLabelPosition.Left:
            actualX -= labelPresenter.DesiredSize.Width / 2.0;
            break;
          case ActualLabelPosition.Right:
            actualX += labelPresenter.DesiredSize.Width / 2.0;
            break;
          case ActualLabelPosition.Bottom:
            actualY += labelPresenter.DesiredSize.Height / 2.0;
            break;
        }
      }
      else
        this.AlignAdornmentLabelPosition3D(labelPresenter, this.LabelPosition, ref actualX, ref actualY, labelIndex);
      double num = this.Series is CircularSeriesBase3D ? (this.Series as CircularSeriesBase3D).Center.Y * 2.0 : (this.series.IsActualTransposed ? this.series.ActualXAxis.RenderedRect.Height : this.series.ActualYAxis.RenderedRect.Height);
      XyzDataSeries3D series2 = this.series as XyzDataSeries3D;
      labelPresenter.Visibility = Visibility.Visible;
      if (series1 != null || this.series is CartesianSeries3D && this.series.ActualYAxis != null && num >= actualY)
        this.AddLabel((UIElement) labelPresenter, actualX, actualY, chartAdornment3D.ActualStartDepth, labelIndex);
      else if (this.series is BarSeries3D && this.series.ActualYAxis != null && num >= actualY)
      {
        this.AddLabel((UIElement) labelPresenter, actualX, actualY, chartAdornment3D.ActualStartDepth, labelIndex);
      }
      else
      {
        if (!(this.series is CartesianSeries3D) || (this.series.ActualYAxis == null || !this.series.ActualYAxis.OpposedPosition) && (series2 == null || series2.ActualZAxis == null || !series2.ActualZAxis.OpposedPosition))
          return;
        this.AddLabel((UIElement) labelPresenter, actualX, actualY, chartAdornment3D.ActualStartDepth, labelIndex);
      }
    }
  }

  private double ModifyLeftToAngle(double x, UIElement element)
  {
    double actualRotationAngle = (this.Series.ActualArea as SfChart3D).ActualRotationAngle;
    double actualTiltAngle = (this.Series.ActualArea as SfChart3D).ActualTiltAngle;
    Path path = element as Path;
    if (this.Series is ScatterSeries3D)
    {
      ScatterSeries3D series = this.series as ScatterSeries3D;
      double num = series.IsTransposed ? series.ScatterHeight / 2.0 : series.ScatterWidth / 2.0;
      if (actualTiltAngle >= 45.0 && actualTiltAngle < 315.0)
      {
        if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
          return x + 1.0;
        if (actualRotationAngle >= 225.0 && actualRotationAngle < 315.0)
          return x - 1.0 - num - (path != null ? 0.0 : element.DesiredSize.Width / 2.0);
        return actualRotationAngle >= 45.0 && actualRotationAngle < 135.0 ? x + num + (path != null ? 0.0 : element.DesiredSize.Width / 2.0) + 1.0 : x;
      }
      if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
        return x + 1.0;
      if (actualRotationAngle >= 225.0 && actualRotationAngle < 315.0)
        return x - 1.0 - num;
      return actualRotationAngle >= 45.0 && actualRotationAngle < 135.0 ? x + num + 2.0 : x;
    }
    if (actualTiltAngle >= 45.0 && actualTiltAngle < 315.0)
    {
      if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
        return x + 1.0;
      if (actualRotationAngle >= 45.0 && actualRotationAngle < 135.0)
        return this.Series.IsActualTransposed && this.LabelPosition == AdornmentsLabelPosition.Inner ? x + (path != null ? 0.0 : element.DesiredSize.Width * 1.5) : x + (path != null ? 0.0 : element.DesiredSize.Width / 2.0);
      if (actualRotationAngle < 225.0 || actualRotationAngle >= 315.0)
        return x;
      return this.Series.IsActualTransposed && this.LabelPosition == AdornmentsLabelPosition.Inner ? x - (path != null ? 0.0 : element.DesiredSize.Width * 1.5) : x - (path != null ? 0.0 : element.DesiredSize.Width / 2.0);
    }
    if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
      return x + 1.0;
    if (actualRotationAngle >= 225.0 && actualRotationAngle < 315.0)
      return x - 1.0;
    if (actualRotationAngle < 45.0 || actualRotationAngle >= 135.0)
      return x;
    if (!this.Series.IsActualTransposed)
      return x + 1.0;
    return this.LabelPosition == AdornmentsLabelPosition.Inner ? x + (path != null ? 0.0 : element.DesiredSize.Width) + 1.0 : x + (path != null ? 0.0 : element.DesiredSize.Width / 2.0) + 5.0;
  }

  private double ModifyDepthToAngle(double depth, UIElement element, int index)
  {
    double actualRotationAngle = (this.Series.ActualArea as SfChart3D).ActualRotationAngle;
    double actualTiltAngle = (this.Series.ActualArea as SfChart3D).ActualTiltAngle;
    Path path = element as Path;
    if (this.Series is ScatterSeries3D)
    {
      ScatterSeries3D series = this.series as ScatterSeries3D;
      ScatterSegment3D segment = this.series.Segments[index] as ScatterSegment3D;
      if (!string.IsNullOrEmpty(series.ZBindingPath))
      {
        if (actualTiltAngle >= 45.0 && actualTiltAngle < 315.0)
        {
          if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
            return depth + segment.ScatterHeight * 1.5 + 1.0;
          return actualRotationAngle >= 315.0 || actualRotationAngle < 45.0 ? depth - segment.ScatterHeight * 1.5 : depth;
        }
        if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
          return depth + segment.ScatterHeight / 2.0 + 1.0;
        return actualRotationAngle >= 315.0 || actualRotationAngle < 45.0 ? depth - segment.ScatterHeight / 2.0 : depth;
      }
      if (actualTiltAngle >= 45.0 && actualTiltAngle < 315.0)
      {
        if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
          return (segment.startDepth + segment.ScatterHeight > segment.endDepth ? segment.endDepth : segment.startDepth + segment.ScatterHeight) + 1.0 + (path != null ? 0.0 : element.DesiredSize.Width / 2.0);
        if (actualRotationAngle >= 315.0 || actualRotationAngle < 45.0)
          return depth - (path != null ? 0.0 : element.DesiredSize.Width / 2.0);
      }
      else
      {
        if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
          return (segment.startDepth + segment.ScatterHeight > segment.endDepth ? segment.endDepth : segment.startDepth + segment.ScatterHeight) + 1.0;
        if (actualRotationAngle >= 315.0 || actualRotationAngle < 45.0)
          return depth;
      }
      return depth + ((segment.startDepth + segment.ScatterHeight > segment.endDepth ? segment.endDepth : segment.startDepth + segment.ScatterHeight) - segment.startDepth) / 2.0;
    }
    if (actualTiltAngle >= 45.0 && actualTiltAngle < 315.0)
    {
      if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0)
        return depth - (path != null ? 0.0 : element.DesiredSize.Height / 2.0);
      return actualRotationAngle >= 45.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 315.0 ? depth : depth + (path != null ? 0.0 : element.DesiredSize.Height / 2.0);
    }
    if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0)
      return depth - 1.0;
    return actualRotationAngle >= 45.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 315.0 ? depth : depth + 1.0;
  }
}
