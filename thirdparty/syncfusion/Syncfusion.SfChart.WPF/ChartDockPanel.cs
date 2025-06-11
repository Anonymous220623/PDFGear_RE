// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartDockPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartDockPanel : Panel
{
  public static readonly DependencyProperty LastChildFillProperty = DependencyProperty.Register(nameof (LastChildFill), typeof (bool), typeof (ChartDockPanel), new PropertyMetadata((object) true, new PropertyChangedCallback(ChartDockPanel.OnLastChildFillPropertyChanged)));
  public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached("Dock", typeof (ChartDock), typeof (ChartDockPanel), new PropertyMetadata((object) ChartDock.Top, new PropertyChangedCallback(ChartDockPanel.OnDockPropertyChanged)));
  internal static readonly DependencyProperty HostProperty = DependencyProperty.Register(nameof (Host), typeof (string), typeof (ChartDockPanel), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty ElementMarginProperty = DependencyProperty.Register(nameof (ElementMargin), typeof (Thickness), typeof (ChartDockPanel), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0)));
  public static readonly DependencyProperty RootElementProperty = DependencyProperty.Register(nameof (RootElement), typeof (UIElement), typeof (ChartDockPanel), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartDockPanel.OnRootElementChanged)));
  public UIElement m_rootElement;
  private Thickness m_controlsThickness = new Thickness();
  private Rect m_resultDockRect = new Rect();
  private static bool _ignorePropertyChange;

  public bool LastChildFill
  {
    get => (bool) this.GetValue(ChartDockPanel.LastChildFillProperty);
    set => this.SetValue(ChartDockPanel.LastChildFillProperty, (object) value);
  }

  public UIElement RootElement
  {
    get => this.m_rootElement;
    set
    {
      if (this.m_rootElement != null || this.m_rootElement == value)
        return;
      this.SetValue(ChartDockPanel.RootElementProperty, (object) value);
    }
  }

  public Thickness ElementMargin
  {
    get => (Thickness) this.GetValue(ChartDockPanel.ElementMarginProperty);
    set => this.SetValue(ChartDockPanel.ElementMarginProperty, (object) value);
  }

  internal string Host
  {
    set => this.SetValue(ChartDockPanel.HostProperty, (object) value);
    get => (string) this.GetValue(ChartDockPanel.HostProperty);
  }

  public static ChartDock GetDock(UIElement element)
  {
    return element != null ? (ChartDock) element.GetValue(ChartDockPanel.DockProperty) : throw new ArgumentNullException(nameof (element));
  }

  public static void SetDock(UIElement element, ChartDock dock)
  {
    if (element == null)
      throw new ArgumentNullException(nameof (element));
    element.SetValue(ChartDockPanel.DockProperty, (object) dock);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    List<double> source1 = new List<double>();
    List<double> source2 = new List<double>();
    List<double> source3 = new List<double>();
    List<double> source4 = new List<double>();
    Thickness elementMargin = this.ElementMargin;
    this.m_controlsThickness = this.ElementMargin;
    foreach (UIElement child in this.Children)
    {
      if (child != null && child != this.m_rootElement)
      {
        child.Measure(availableSize);
        Size size = ChartLayoutUtils.Addthickness(child.DesiredSize, elementMargin);
        ChartLegend chartLegend = child as ChartLegend;
        switch (ChartDockPanel.GetDock(child))
        {
          case ChartDock.Left:
            if (chartLegend != null)
            {
              int rowColumnIndex = chartLegend.RowColumnIndex;
              if (source2.Count <= rowColumnIndex)
              {
                source2.Add(size.Width);
                continue;
              }
              if (source2[rowColumnIndex] < size.Width)
              {
                source2[rowColumnIndex] = size.Width;
                continue;
              }
              continue;
            }
            this.m_controlsThickness.Left += size.Width;
            continue;
          case ChartDock.Top:
            if (chartLegend != null)
            {
              int rowColumnIndex = chartLegend.RowColumnIndex;
              if (source1.Count <= rowColumnIndex)
              {
                source1.Add(size.Height);
                continue;
              }
              if (source1[rowColumnIndex] < size.Height)
              {
                source1[rowColumnIndex] = size.Height;
                continue;
              }
              continue;
            }
            this.m_controlsThickness.Top += size.Height;
            continue;
          case ChartDock.Right:
            if (chartLegend != null)
            {
              int rowColumnIndex = chartLegend.RowColumnIndex;
              if (source4.Count <= rowColumnIndex)
              {
                source4.Add(size.Width);
                continue;
              }
              if (source4[rowColumnIndex] < size.Width)
              {
                source4[rowColumnIndex] = size.Width;
                continue;
              }
              continue;
            }
            this.m_controlsThickness.Right += size.Width;
            continue;
          case ChartDock.Bottom:
            if (chartLegend != null)
            {
              int rowColumnIndex = chartLegend.RowColumnIndex;
              if (source3.Count <= rowColumnIndex)
              {
                source3.Add(size.Height);
                continue;
              }
              if (source3[rowColumnIndex] < size.Height)
              {
                source3[rowColumnIndex] = size.Height;
                continue;
              }
              continue;
            }
            this.m_controlsThickness.Bottom += size.Height;
            continue;
          case ChartDock.Floating:
            if (chartLegend != null)
            {
              if (chartLegend.ActualHeight > 0.0 || chartLegend.ActualWidth > 0.0)
              {
                source2.Add(0.0);
                continue;
              }
              source2.Add(0.1);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    this.m_controlsThickness.Left += source2.Sum();
    this.m_controlsThickness.Right += source4.Sum();
    this.m_controlsThickness.Top += source1.Sum();
    this.m_controlsThickness.Bottom += source3.Sum();
    try
    {
      this.m_rootElement.Measure(ChartLayoutUtils.Subtractthickness(availableSize, this.m_controlsThickness));
    }
    catch (Exception ex)
    {
    }
    return availableSize;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    this.m_resultDockRect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), finalSize), this.m_controlsThickness);
    Rect rect1 = new Rect(new Point(0.0, 0.0), finalSize);
    Rect bounds = rect1;
    if (this.m_rootElement != null)
    {
      try
      {
        this.m_rootElement.Arrange(this.m_resultDockRect);
      }
      catch (Exception ex)
      {
      }
    }
    double num1 = -1.0;
    for (int index = 0; index < this.Children.Count; ++index)
    {
      UIElement child = this.Children[index];
      Size size = ChartLayoutUtils.Addthickness(child.DesiredSize, this.ElementMargin);
      if (child != null && child != this.m_rootElement)
      {
        ChartLegend chartLegend = child as ChartLegend;
        double num2 = 0.0;
        double num3 = 0.0;
        if (chartLegend != null)
        {
          num2 = double.IsNaN(chartLegend.OffsetX) ? 0.0 : chartLegend.OffsetX;
          num3 = double.IsNaN(chartLegend.OffsetY) ? 0.0 : chartLegend.OffsetY;
        }
        switch (ChartDockPanel.GetDock(child))
        {
          case ChartDock.Left:
            switch (child)
            {
              case ChartLegend _:
                Rect arrangeRect1 = (child as ChartLegend).ArrangeRect;
                this.ArrangeElement(child, ChartDock.Left, new Rect(arrangeRect1.Left + num2 + rect1.Left, arrangeRect1.Top + num3 + this.m_controlsThickness.Top, arrangeRect1.Width, arrangeRect1.Height));
                continue;
              case ChartColorBar _:
                Rect arrangeRect2 = (child as ChartColorBar).ArrangeRect;
                this.ArrangeElement(child, ChartDock.Left, new Rect(arrangeRect2.Left + rect1.Left, arrangeRect2.Top + this.m_controlsThickness.Top, arrangeRect2.Width, arrangeRect2.Height));
                continue;
              default:
                this.ArrangeElement(child, ChartDock.Left, new Rect(rect1.Left, bounds.Y, size.Width, bounds.Height));
                rect1.X += size.Width;
                double num4 = rect1.Width - size.Width;
                rect1.Width = num4 > 0.0 ? num4 : 0.0;
                continue;
            }
          case ChartDock.Top:
            switch (child)
            {
              case ChartLegend _:
                if (num1 == -1.0)
                  num1 = rect1.Top;
                Rect arrangeRect3 = (child as ChartLegend).ArrangeRect;
                this.ArrangeElement(child, ChartDock.Top, new Rect(arrangeRect3.Left + num2 + this.m_controlsThickness.Left, arrangeRect3.Top + num3 + num1, arrangeRect3.Width, arrangeRect3.Height));
                continue;
              case ChartColorBar _:
                if (num1 == -1.0)
                  num1 = rect1.Top;
                Rect arrangeRect4 = (child as ChartColorBar).ArrangeRect;
                this.ArrangeElement(child, ChartDock.Top, new Rect(arrangeRect4.Left + this.m_controlsThickness.Left, arrangeRect4.Top + num1, arrangeRect4.Width, arrangeRect4.Height));
                continue;
              default:
                this.ArrangeElement(child, ChartDock.Top, new Rect(0.0, rect1.Top, finalSize.Width, size.Height));
                rect1.Y += size.Height;
                double num5 = rect1.Height - size.Height;
                rect1.Height = num5 > 0.0 ? num5 : 0.0;
                continue;
            }
          case ChartDock.Right:
            switch (child)
            {
              case ChartLegend _:
                Rect arrangeRect5 = (child as ChartLegend).ArrangeRect;
                this.ArrangeElement(child, ChartDock.Right, new Rect(arrangeRect5.Left + num2 + this.m_controlsThickness.Left, arrangeRect5.Top + num3 + this.m_controlsThickness.Top, arrangeRect5.Width, arrangeRect5.Height));
                continue;
              case ChartColorBar _:
                Rect arrangeRect6 = (child as ChartColorBar).ArrangeRect;
                this.ArrangeElement(child, ChartDock.Right, new Rect(arrangeRect6.Left + this.m_controlsThickness.Left, arrangeRect6.Top + this.m_controlsThickness.Top, arrangeRect6.Width, arrangeRect6.Height));
                continue;
              default:
                double num6 = rect1.Width - size.Width;
                rect1.Width = num6 > 0.0 ? num6 : 0.0;
                this.ArrangeElement(child, ChartDock.Right, new Rect(rect1.Right, bounds.Top + this.m_controlsThickness.Top, size.Width, bounds.Height));
                continue;
            }
          case ChartDock.Bottom:
            switch (child)
            {
              case ChartLegend _:
                Rect arrangeRect7 = (child as ChartLegend).ArrangeRect;
                this.ArrangeElement(child, ChartDock.Bottom, new Rect(arrangeRect7.Left + num2 + this.m_controlsThickness.Left, arrangeRect7.Top + num3 + this.m_controlsThickness.Top, arrangeRect7.Width, arrangeRect7.Height));
                continue;
              case ChartColorBar _:
                Rect arrangeRect8 = (child as ChartColorBar).ArrangeRect;
                this.ArrangeElement(child, ChartDock.Bottom, new Rect(arrangeRect8.Left + this.m_controlsThickness.Left, arrangeRect8.Top + this.m_controlsThickness.Top, arrangeRect8.Width, arrangeRect8.Height));
                continue;
              default:
                double num7 = rect1.Height - size.Height;
                rect1.Height = num7 > 0.0 ? num7 : 0.0;
                this.ArrangeElement(child, ChartDock.Bottom, new Rect(0.0, rect1.Bottom, finalSize.Width, size.Height));
                continue;
            }
          case ChartDock.Floating:
            Rect rect2 = new Rect(new Point(0.0, 0.0), new Size(child.DesiredSize.Width, child.DesiredSize.Height));
            if (chartLegend != null)
            {
              child.Arrange(ChartDockPanel.EnsureRectIsInside(bounds, new Rect(chartLegend.ArrangeRect.Left + num2 + this.m_controlsThickness.Left, chartLegend.ArrangeRect.Top + num3 + this.m_controlsThickness.Top, child.DesiredSize.Width, chartLegend.DesiredSize.Height)));
              continue;
            }
            child.Arrange(new Rect(0.0, 0.0, finalSize.Width, finalSize.Height));
            continue;
          default:
            continue;
        }
      }
    }
    return base.ArrangeOverride(finalSize);
  }

  private static void OnLastChildFillPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartDockPanel).InvalidateArrange();
  }

  private static void OnDockPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue.Equals(e.OldValue))
      return;
    if (ChartDockPanel._ignorePropertyChange)
    {
      ChartDockPanel._ignorePropertyChange = false;
    }
    else
    {
      if (!(VisualTreeHelper.GetParent(d) is ChartDockPanel parent))
        return;
      parent.InvalidateMeasure();
    }
  }

  private static void OnRootElementChanged(
    DependencyObject dpObj,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(dpObj is ChartDockPanel chartDockPanel))
      return;
    if (e.OldValue != null)
    {
      chartDockPanel.Children.Remove(e.OldValue as UIElement);
      chartDockPanel.m_rootElement = (UIElement) null;
    }
    if (e.NewValue == null)
      return;
    chartDockPanel.m_rootElement = e.NewValue as UIElement;
    chartDockPanel.Children.Add(e.NewValue as UIElement);
  }

  private static Rect EnsureRectIsInside(Rect bounds, Rect rect)
  {
    if (rect.Bottom > bounds.Bottom)
      rect.Y -= rect.Bottom - bounds.Bottom;
    if (rect.Right > bounds.Right)
      rect.X -= rect.Right - bounds.Right;
    if (rect.Top < bounds.Top)
      rect.Y -= rect.Top - bounds.Top;
    if (rect.Left < bounds.Left)
      rect.X -= rect.Left - bounds.Left;
    return rect;
  }

  private void parentgrid_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.InvalidateMeasure();
    this.InvalidateArrange();
  }

  private void ArrangeElement(UIElement element, ChartDock dock, Rect rect)
  {
    element.Arrange(ChartLayoutUtils.Subtractthickness(rect, this.ElementMargin));
  }
}
