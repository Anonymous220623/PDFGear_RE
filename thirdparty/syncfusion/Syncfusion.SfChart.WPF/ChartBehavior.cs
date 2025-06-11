// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartBehavior
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartBehavior : DependencyObject, ICloneable
{
  private const int axisTipHeight = 6;
  protected ChartAxis chartAxis;
  private Canvas adorningCanvas;
  private Canvas bottomAdorningCanvas;
  private SfChart chartArea;

  public Canvas AdorningCanvas
  {
    get => this.adorningCanvas;
    internal set
    {
      if (this.adorningCanvas == value)
        return;
      this.adorningCanvas = value;
    }
  }

  public Canvas BottomAdorningCanvas
  {
    get => this.bottomAdorningCanvas;
    internal set
    {
      if (this.bottomAdorningCanvas == value)
        return;
      this.bottomAdorningCanvas = value;
    }
  }

  public SfChart ChartArea
  {
    get => this.chartArea;
    internal set => this.chartArea = value;
  }

  internal bool IsReversed { get; set; }

  public DependencyObject Clone() => this.CloneBehavior((DependencyObject) null);

  internal static Point ValidatePoint(Point point, Rect rect)
  {
    return new Point()
    {
      X = point.X >= rect.Left ? (point.X <= rect.Right ? point.X : rect.Right) : rect.Left,
      Y = point.Y >= rect.Top ? (point.Y <= rect.Bottom ? point.Y : rect.Bottom) : rect.Top
    };
  }

  internal void Dispose()
  {
    this.DetachElements();
    this.chartAxis = (ChartAxis) null;
    this.ChartArea = (SfChart) null;
  }

  internal void InternalAttachElements() => this.AttachElements();

  internal void AlignAxisToolTipPolygon(
    ContentControl control,
    ChartAlignment verticalAlignemnt,
    ChartAlignment horizontalAlignment,
    double x,
    double y,
    ChartBehavior behavior)
  {
    double height1 = control.DesiredSize.Height;
    double width = control.DesiredSize.Width;
    double num = 12.0 / Math.Sqrt(3.0);
    bool flag = behavior is ChartTrackBallBehavior;
    PointCollection pointCollection = new PointCollection();
    pointCollection.Add(new Point(0.0, 0.0));
    pointCollection.Add(new Point(0.0, height1));
    pointCollection.Add(new Point(width, height1));
    if (this.chartAxis.Orientation == Orientation.Horizontal)
    {
      pointCollection.Add(new Point(width, 0.0));
      if (this.chartAxis.OpposedPosition)
      {
        if ((control.Content as ChartPointInfo).Axis.CrosshairLabelTemplate == null && !flag)
          control.Margin = new Thickness().GetThickness(0.0, -6.0, 0.0, 0.0);
        if ((control.Content as ChartPointInfo).X - control.DesiredSize.Height <= this.ChartArea.SeriesClipRect.X)
        {
          if (flag)
            this.AlignDefaultLabel(ChartAlignment.Far, ChartAlignment.Far, this.ChartArea.SeriesClipRect.X, (control.Content as ChartPointInfo).Y, control);
          else
            this.AlignDefaultLabel(verticalAlignemnt, ChartAlignment.Far, this.ChartArea.SeriesClipRect.X, (control.Content as ChartPointInfo).Y, control);
          double x1 = (control.Content as ChartPointInfo).BaseX - this.ChartArea.SeriesClipRect.X;
          double x2 = x1 - num / 2.0;
          double x3 = x1 + num / 2.0;
          if (x2 < 0.0)
            x2 = 0.0;
          pointCollection.Insert(2, new Point(x2, height1));
          pointCollection.Insert(3, new Point(x1, height1 + 6.0));
          pointCollection.Insert(4, new Point(x3, height1));
        }
        else if ((control.Content as ChartPointInfo).X + control.DesiredSize.Width >= this.ChartArea.SeriesClipRect.X + this.ChartArea.SeriesClipRect.Width)
        {
          if (flag)
            this.AlignDefaultLabel(ChartAlignment.Far, ChartAlignment.Near, this.ChartArea.SeriesClipRect.X + this.ChartArea.SeriesClipRect.Width, (control.Content as ChartPointInfo).Y, control);
          else
            this.AlignDefaultLabel(verticalAlignemnt, ChartAlignment.Near, this.ChartArea.SeriesClipRect.X + this.ChartArea.SeriesClipRect.Width, (control.Content as ChartPointInfo).Y, control);
          double x4 = (control.Content as ChartPointInfo).BaseX - (control.Content as ChartPointInfo).X;
          double x5 = x4 - num / 2.0;
          double x6 = x4 + num / 2.0;
          if (x6 > control.DesiredSize.Width)
            x6 = control.DesiredSize.Width;
          if (x5 < 0.0)
            x5 = 0.0;
          pointCollection.Insert(2, new Point(x5, height1));
          pointCollection.Insert(3, new Point(x4, height1 + 6.0));
          pointCollection.Insert(4, new Point(x6, height1));
        }
        else
        {
          this.AlignDefaultLabel(verticalAlignemnt, horizontalAlignment, x, y, control);
          pointCollection.Insert(2, new Point(width / 2.0 - num / 2.0, height1));
          pointCollection.Insert(3, new Point(width / 2.0, height1 + 6.0));
          pointCollection.Insert(4, new Point(width / 2.0 + num / 2.0, height1));
        }
      }
      else
      {
        if ((control.Content as ChartPointInfo).Axis.CrosshairLabelTemplate == null && !flag)
          control.Margin = new Thickness().GetThickness(0.0, 6.0, 0.0, 0.0);
        if ((control.Content as ChartPointInfo).X - control.DesiredSize.Height <= this.ChartArea.SeriesClipRect.X)
        {
          this.AlignDefaultLabel(ChartAlignment.Far, ChartAlignment.Far, this.ChartArea.SeriesClipRect.X, (control.Content as ChartPointInfo).Y, control);
          double x7 = (control.Content as ChartPointInfo).BaseX - this.ChartArea.SeriesClipRect.X;
          double x8 = x7 - num / 2.0;
          double x9 = x7 + num / 2.0;
          if (x8 < 0.0)
            x8 = 0.0;
          pointCollection.Insert(4, new Point(x9, 0.0));
          pointCollection.Insert(5, new Point(x7, -6.0));
          pointCollection.Insert(6, new Point(x8, 0.0));
        }
        else if ((control.Content as ChartPointInfo).X + control.DesiredSize.Width >= this.ChartArea.SeriesClipRect.X + this.ChartArea.SeriesClipRect.Width)
        {
          this.AlignDefaultLabel(ChartAlignment.Far, ChartAlignment.Near, this.ChartArea.SeriesClipRect.X + this.ChartArea.SeriesClipRect.Width, (control.Content as ChartPointInfo).Y, control);
          double x10 = (control.Content as ChartPointInfo).BaseX - (control.Content as ChartPointInfo).X;
          double x11 = x10 - num / 2.0;
          double x12 = x10 + num / 2.0;
          if (x12 > control.DesiredSize.Width)
            x12 = control.DesiredSize.Width;
          if (x11 < 0.0)
            x11 = 0.0;
          pointCollection.Insert(4, new Point(x12, 0.0));
          pointCollection.Insert(5, new Point(x10, -6.0));
          pointCollection.Insert(6, new Point(x11, 0.0));
        }
        else
        {
          this.AlignDefaultLabel(verticalAlignemnt, horizontalAlignment, x, y, control);
          pointCollection.Insert(4, new Point(width / 2.0 + num / 2.0, 0.0));
          pointCollection.Insert(5, new Point(width / 2.0, -6.0));
          pointCollection.Insert(6, new Point(width / 2.0 - num / 2.0, 0.0));
        }
      }
      pointCollection.Add(new Point(0.0, 0.0));
      (control.Content as ChartPointInfo).PolygonPoints = pointCollection;
    }
    else
    {
      if (this.chartAxis.OpposedPosition)
      {
        if ((control.Content as ChartPointInfo).Axis.CrosshairLabelTemplate == null && !flag)
          control.Margin = new Thickness().GetThickness(6.0, 0.0, 0.0, 0.0);
        if ((control.Content as ChartPointInfo).Y - control.DesiredSize.Height <= this.ChartArea.SeriesClipRect.Y)
        {
          if (flag)
            this.AlignDefaultLabel(ChartAlignment.Far, ChartAlignment.Far, (control.Content as ChartPointInfo).X, this.ChartArea.SeriesClipRect.Y, control);
          else
            this.AlignDefaultLabel(ChartAlignment.Far, horizontalAlignment, (control.Content as ChartPointInfo).X, this.ChartArea.SeriesClipRect.Y, control);
          double y1 = (control.Content as ChartPointInfo).BaseY - this.ChartArea.SeriesClipRect.Y;
          double y2 = y1 - num / 2.0;
          double y3 = y1 + num / 2.0;
          if (y2 < 0.0)
            y2 = 0.0;
          if (y3 > height1)
            y3 = height1;
          pointCollection.Insert(1, new Point(0.0, y2));
          pointCollection.Insert(2, new Point(-6.0, y1));
          pointCollection.Insert(3, new Point(0.0, y3));
        }
        else if ((control.Content as ChartPointInfo).Y + control.DesiredSize.Height >= this.ChartArea.SeriesClipRect.Y + this.ChartArea.SeriesClipRect.Height)
        {
          if (flag)
          {
            double x13 = (control.Content as ChartPointInfo).X;
            Rect seriesClipRect = this.ChartArea.SeriesClipRect;
            double y4 = seriesClipRect.Y;
            seriesClipRect = this.ChartArea.SeriesClipRect;
            double height2 = seriesClipRect.Height;
            double y5 = y4 + height2;
            ContentControl control1 = control;
            this.AlignDefaultLabel(ChartAlignment.Near, ChartAlignment.Far, x13, y5, control1);
          }
          else
          {
            int horizontalAlignment1 = (int) horizontalAlignment;
            double x14 = (control.Content as ChartPointInfo).X;
            Rect seriesClipRect = this.ChartArea.SeriesClipRect;
            double y6 = seriesClipRect.Y;
            seriesClipRect = this.ChartArea.SeriesClipRect;
            double height3 = seriesClipRect.Height;
            double y7 = y6 + height3;
            ContentControl control2 = control;
            this.AlignDefaultLabel(ChartAlignment.Near, (ChartAlignment) horizontalAlignment1, x14, y7, control2);
          }
          double y8 = (control.Content as ChartPointInfo).BaseY - (control.Content as ChartPointInfo).Y;
          double y9 = y8 - num / 2.0;
          double y10 = y8 + num / 2.0;
          if (y10 > control.DesiredSize.Height)
            y10 = control.DesiredSize.Height;
          if (y9 < 0.0)
            y9 = 0.0;
          pointCollection.Insert(1, new Point(0.0, y9));
          pointCollection.Insert(2, new Point(-6.0, y8));
          pointCollection.Insert(3, new Point(0.0, y10));
        }
        else
        {
          if (!flag)
            this.AlignDefaultLabel(verticalAlignemnt, horizontalAlignment, x, y, control);
          pointCollection.Insert(1, new Point(0.0, height1 / 3.0));
          pointCollection.Insert(2, new Point(-6.0, height1 / 2.0));
          pointCollection.Insert(3, new Point(0.0, height1 / 1.5));
        }
      }
      else
      {
        if ((control.Content as ChartPointInfo).Axis.CrosshairLabelTemplate == null && !flag)
          control.Margin = new Thickness().GetThickness(-6.0, 0.0, 0.0, 0.0);
        if ((control.Content as ChartPointInfo).Y - control.DesiredSize.Height <= this.ChartArea.SeriesClipRect.Y)
        {
          Rect seriesClipRect;
          if (flag)
          {
            double x15 = (control.Content as ChartPointInfo).X;
            seriesClipRect = this.ChartArea.SeriesClipRect;
            double y11 = seriesClipRect.Y;
            ContentControl control3 = control;
            this.AlignDefaultLabel(ChartAlignment.Far, ChartAlignment.Far, x15, y11, control3);
          }
          else
          {
            int horizontalAlignment2 = (int) horizontalAlignment;
            double x16 = (control.Content as ChartPointInfo).X;
            seriesClipRect = this.ChartArea.SeriesClipRect;
            double y12 = seriesClipRect.Y;
            ContentControl control4 = control;
            this.AlignDefaultLabel(ChartAlignment.Far, (ChartAlignment) horizontalAlignment2, x16, y12, control4);
          }
          double baseY = (control.Content as ChartPointInfo).BaseY;
          seriesClipRect = this.ChartArea.SeriesClipRect;
          double y13 = seriesClipRect.Y;
          double y14 = baseY - y13;
          double y15 = y14 - num / 2.0;
          double y16 = y14 + num / 2.0;
          if (y15 < 0.0)
            y15 = 0.0;
          if (y16 > height1)
            y16 = height1;
          pointCollection.Insert(3, new Point(width, y16));
          pointCollection.Insert(4, new Point(width + 6.0, y14));
          pointCollection.Insert(5, new Point(width, y15));
        }
        else if ((control.Content as ChartPointInfo).Y + control.DesiredSize.Height >= this.ChartArea.SeriesClipRect.Y + this.ChartArea.SeriesClipRect.Height)
        {
          if (flag)
          {
            double x17 = (control.Content as ChartPointInfo).X;
            Rect seriesClipRect = this.ChartArea.SeriesClipRect;
            double y17 = seriesClipRect.Y;
            seriesClipRect = this.ChartArea.SeriesClipRect;
            double height4 = seriesClipRect.Height;
            double y18 = y17 + height4;
            ContentControl control5 = control;
            this.AlignDefaultLabel(ChartAlignment.Near, ChartAlignment.Far, x17, y18, control5);
          }
          else
          {
            int horizontalAlignment3 = (int) horizontalAlignment;
            double x18 = (control.Content as ChartPointInfo).X;
            Rect seriesClipRect = this.ChartArea.SeriesClipRect;
            double y19 = seriesClipRect.Y;
            seriesClipRect = this.ChartArea.SeriesClipRect;
            double height5 = seriesClipRect.Height;
            double y20 = y19 + height5;
            ContentControl control6 = control;
            this.AlignDefaultLabel(ChartAlignment.Near, (ChartAlignment) horizontalAlignment3, x18, y20, control6);
          }
          double y21 = (control.Content as ChartPointInfo).BaseY - (control.Content as ChartPointInfo).Y;
          double y22 = y21 - num / 2.0;
          double y23 = y21 + num / 2.0;
          if (y22 < 0.0)
            y22 = 0.0;
          if (y23 > control.DesiredSize.Height)
            y23 = control.DesiredSize.Height;
          pointCollection.Add(new Point(width, y23));
          pointCollection.Add(new Point(width + 6.0, y21));
          pointCollection.Add(new Point(width, y22));
        }
        else
        {
          if (!flag)
            this.AlignDefaultLabel(verticalAlignemnt, horizontalAlignment, x, y, control);
          pointCollection.Add(new Point(width, height1 / 1.5));
          pointCollection.Add(new Point(width + 6.0, height1 / 2.0));
          pointCollection.Add(new Point(width, height1 / 3.0));
        }
      }
      pointCollection.Add(new Point(width, 0.0));
      pointCollection.Add(new Point(0.0, 0.0));
      (control.Content as ChartPointInfo).PolygonPoints = pointCollection;
    }
  }

  protected internal virtual void OnLayoutUpdated()
  {
  }

  protected internal virtual void DetachElement(UIElement element)
  {
    if (this.AdorningCanvas == null || !this.AdorningCanvas.Children.Contains(element))
      return;
    this.AdorningCanvas.Children.Remove(element);
  }

  protected internal virtual void DetachElements()
  {
  }

  protected internal virtual void OnSizeChanged(SizeChangedEventArgs e)
  {
  }

  protected internal virtual void OnDragEnter(DragEventArgs e)
  {
  }

  protected internal virtual void OnDragLeave(DragEventArgs e)
  {
  }

  protected internal virtual void OnDragOver(DragEventArgs e)
  {
  }

  protected internal virtual void OnDrop(DragEventArgs e)
  {
  }

  protected internal virtual void OnGotFocus(RoutedEventArgs e)
  {
  }

  protected internal virtual void OnLostFocus(RoutedEventArgs e)
  {
  }

  protected internal virtual void OnMouseWheel(MouseWheelEventArgs e)
  {
  }

  protected internal virtual void OnMouseEnter(MouseEventArgs e)
  {
  }

  protected internal virtual void OnMouseLeave(MouseEventArgs e)
  {
  }

  protected internal virtual void OnMouseMove(MouseEventArgs e)
  {
  }

  internal virtual void OnTouchDown(TouchEventArgs e)
  {
  }

  internal virtual void OnTouchMove(TouchEventArgs e)
  {
  }

  internal virtual void OnTouchUp(TouchEventArgs e)
  {
  }

  protected internal virtual void OnKeyUp(KeyEventArgs e)
  {
  }

  protected internal virtual void OnKeyDown(KeyEventArgs e)
  {
  }

  protected internal virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
  }

  protected internal virtual void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
  }

  protected internal virtual void OnManipulationStarted(ManipulationStartedEventArgs e)
  {
  }

  protected internal virtual void OnManipulationCompleted(ManipulationCompletedEventArgs e)
  {
  }

  protected internal virtual void OnManipulationDelta(ManipulationDeltaEventArgs e)
  {
  }

  protected internal virtual void OnDoubleTapped(MouseButtonEventArgs e)
  {
  }

  protected internal virtual void AlignDefaultLabel(
    ChartAlignment verticalAlignemnt,
    ChartAlignment horizontalAlignment,
    double x,
    double y,
    ContentControl control)
  {
  }

  protected virtual void AttachElements()
  {
  }

  protected virtual DependencyObject CloneBehavior(DependencyObject obj) => obj;

  protected IList<double> GetYValuesBasedOnIndex(double x, ChartSeriesBase series)
  {
    List<double> yvaluesBasedOnIndex = new List<double>();
    if (x < (double) series.DataCount)
    {
      for (int index = 0; index < ((IEnumerable<IList<double>>) series.ActualSeriesYValues).Count<IList<double>>(); ++index)
        yvaluesBasedOnIndex.Add(series.ActualSeriesYValues[index][(int) x]);
    }
    return (IList<double>) yvaluesBasedOnIndex;
  }

  protected void UpdateArea()
  {
    if (this.ChartArea == null)
      return;
    this.ChartArea.ScheduleUpdate();
  }
}
