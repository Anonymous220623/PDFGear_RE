// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCircularAxisPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartCircularAxisPanel : ILayoutCalculator
{
  private Size desiredSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
  private Panel panel;
  private UIElementsRecycler<Line> lineRecycler;
  private UIElementsRecycler<ContentControl> contentControlRecycler;
  private ChartAxis axis;
  private Size m_maxLabelsSize;

  public ChartCircularAxisPanel(Panel panel)
  {
    this.panel = panel != null ? panel : throw new ArgumentNullException();
    this.lineRecycler = new UIElementsRecycler<Line>(panel);
    this.contentControlRecycler = new UIElementsRecycler<ContentControl>(panel);
  }

  public Point Center { get; set; }

  public Panel Panel => this.panel;

  public double Radius { get; set; }

  public ChartAxis Axis
  {
    get => this.axis;
    set => this.axis = value;
  }

  public Size DesiredSize => this.desiredSize;

  public List<UIElement> Children => this.panel.Children.Cast<UIElement>().ToList<UIElement>();

  public double Left { get; set; }

  public double Top { get; set; }

  public void RenderElements()
  {
    this.RenderLabels();
    this.RenderTicks();
  }

  public Size Measure(Size availableSize)
  {
    this.m_maxLabelsSize = new Size();
    foreach (UIElement uiElement in (IEnumerable) this.contentControlRecycler)
    {
      uiElement.Measure(availableSize);
      this.m_maxLabelsSize.Width = Math.Max(uiElement.DesiredSize.Width, this.m_maxLabelsSize.Width);
      this.m_maxLabelsSize.Height = Math.Max(uiElement.DesiredSize.Height, this.m_maxLabelsSize.Height);
    }
    this.Radius = 0.5 * Math.Min(availableSize.Width - 2.0 * this.m_maxLabelsSize.Width, availableSize.Height - 2.0 * this.m_maxLabelsSize.Height) - Math.Max(this.Axis.TickLineSize, 0.0);
    this.Center = ChartLayoutUtils.GetCenter(availableSize);
    return availableSize;
  }

  public void DetachElements()
  {
    this.panel = (Panel) null;
    if (this.lineRecycler != null)
      this.lineRecycler.Clear();
    if (this.contentControlRecycler == null)
      return;
    this.contentControlRecycler.Clear();
  }

  public Size Arrange(Size finalSize)
  {
    this.Radius = Math.Max(0.0, 0.5 * Math.Min(finalSize.Width - 2.0 * this.m_maxLabelsSize.Width, finalSize.Height - 2.0 * this.m_maxLabelsSize.Height) - Math.Max(this.Axis.TickLineSize, 0.0));
    this.Center = ChartLayoutUtils.GetCenter(finalSize);
    this.RenderElements();
    return finalSize;
  }

  public void UpdateElements()
  {
    this.UpdateLabels();
    this.UpdateTickLines();
  }

  private void UpdateLabels()
  {
    int index = 0;
    ObservableCollection<ChartAxisLabel> visibleLabels = this.Axis.VisibleLabels;
    this.contentControlRecycler.GenerateElements(visibleLabels.Count);
    DataTemplate prefixLabelTemplate = this.Axis.PrefixLabelTemplate;
    DataTemplate postfixLabelTemplate = this.Axis.PostfixLabelTemplate;
    foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) visibleLabels)
    {
      if (this.Axis is NumericalAxis && index == this.Axis.VisibleLabels.Count - 1)
        break;
      ContentControl contentControl = this.contentControlRecycler[index];
      contentControl.Tag = (object) visibleLabels[index];
      if (this.Axis.LabelTemplate == null)
      {
        contentControl.ContentTemplate = ChartDictionaries.GenericCommonDictionary[(object) "AxisLabelsCustomTemplate"] as DataTemplate;
        contentControl.ApplyTemplate();
        chartAxisLabel.PrefixLabelTemplate = prefixLabelTemplate;
        chartAxisLabel.PostfixLabelTemplate = postfixLabelTemplate;
        if (this.Axis.LabelStyle != null)
        {
          if (this.Axis.LabelStyle.Foreground != null)
          {
            Binding binding = new Binding()
            {
              Source = (object) this.Axis.LabelStyle,
              Path = new PropertyPath("Foreground", new object[0])
            };
            contentControl.SetBinding(Control.ForegroundProperty, (BindingBase) binding);
          }
          if (this.Axis.LabelStyle.FontSize > 0.0)
          {
            Binding binding = new Binding()
            {
              Source = (object) this.Axis.LabelStyle,
              Path = new PropertyPath("FontSize", new object[0])
            };
            contentControl.SetBinding(Control.FontSizeProperty, (BindingBase) binding);
          }
          if (this.Axis.LabelStyle.FontFamily != null)
          {
            Binding binding = new Binding()
            {
              Source = (object) this.Axis.LabelStyle,
              Path = new PropertyPath("FontFamily", new object[0])
            };
            contentControl.SetBinding(Control.FontFamilyProperty, (BindingBase) binding);
          }
        }
        contentControl.Content = (object) chartAxisLabel;
      }
      else
      {
        contentControl.ContentTemplate = this.Axis.LabelTemplate;
        contentControl.ApplyTemplate();
        contentControl.Content = (object) chartAxisLabel;
      }
      ++index;
    }
  }

  private void UpdateTickLines()
  {
    int count = this.Axis.VisibleLabels.Count;
    if (!this.lineRecycler.BindingProvider.Keys.Contains<DependencyProperty>(FrameworkElement.StyleProperty))
      this.lineRecycler.BindingProvider.Add(FrameworkElement.StyleProperty, new Binding()
      {
        Source = (object) this.Axis,
        Path = new PropertyPath("MajorTickLineStyle", new object[0])
      });
    this.lineRecycler.GenerateElements(count);
  }

  private void RenderTicks()
  {
    Point center = this.Center;
    double radius = this.Radius;
    int index = 0;
    foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) this.Axis.VisibleLabels)
    {
      Point vector = ChartTransform.ValueToVector(this.Axis, visibleLabel.Position);
      Line line = this.lineRecycler[index];
      Point point = new Point(center.X + radius * vector.X, center.Y + radius * vector.Y);
      line.X1 = point.X;
      line.Y1 = point.Y;
      line.X2 = point.X + this.Axis.TickLineSize * vector.X;
      line.Y2 = point.Y + this.Axis.TickLineSize * vector.Y;
      ++index;
    }
  }

  private void RenderLabels()
  {
    double num1 = this.Radius + Math.Max(this.Axis.TickLineSize, 0.0);
    int index = 0;
    foreach (ChartAxisLabel visibleLabel in (Collection<ChartAxisLabel>) this.Axis.VisibleLabels)
    {
      double polarCoefficient = this.Axis.ValueToPolarCoefficient(visibleLabel.Position);
      FrameworkElement element = (FrameworkElement) this.contentControlRecycler[index];
      Point vector = ChartTransform.ValueToVector(this.Axis, visibleLabel.Position);
      Point point = new Point(this.Center.X + num1 * vector.X, this.Center.Y + num1 * vector.Y);
      double num2 = element.DesiredSize.Width / 2.0;
      if (polarCoefficient == 0.25)
      {
        point.X -= element.DesiredSize.Width;
        point.Y -= element.DesiredSize.Height / 2.0;
      }
      else if (polarCoefficient == 0.5)
        point.X -= element.DesiredSize.Width / 2.0;
      else if (polarCoefficient == 0.75)
        point.Y -= element.DesiredSize.Height / 2.0;
      else if (polarCoefficient == 1.0 || polarCoefficient == 0.0)
      {
        point.X -= element.DesiredSize.Width / 2.0;
        point.Y -= element.DesiredSize.Height;
      }
      else if (0.0 < polarCoefficient && polarCoefficient < 0.25)
      {
        point.X -= element.DesiredSize.Width;
        point.Y -= element.DesiredSize.Height;
      }
      else if (0.25 < polarCoefficient && polarCoefficient < 0.5)
        point.X -= element.DesiredSize.Width;
      else if (0.75 < polarCoefficient && polarCoefficient < 1.0)
        point.Y -= element.DesiredSize.Height;
      Canvas.SetLeft((UIElement) element, point.X);
      Canvas.SetTop((UIElement) element, point.Y);
      ++index;
    }
  }
}
