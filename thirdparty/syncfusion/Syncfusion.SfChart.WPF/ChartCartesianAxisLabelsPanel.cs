// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCartesianAxisLabelsPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartCartesianAxisLabelsPanel : ILayoutCalculator
{
  private Rect bounds;
  private Panel labelsPanels;
  private Size desiredSize;
  private UIElementsRecycler<TextBlock> textBlockRecycler;
  private UIElementsRecycler<ContentControl> contentControlRecycler;
  private UIElementsRecycler<Border> borderRecycler;
  private List<Border> borders;

  public ChartCartesianAxisLabelsPanel(Panel panel)
  {
    this.LabelLayout = (AxisLabelLayout) null;
    this.labelsPanels = panel;
    this.textBlockRecycler = new UIElementsRecycler<TextBlock>(panel);
    this.contentControlRecycler = new UIElementsRecycler<ContentControl>(panel);
    this.borderRecycler = new UIElementsRecycler<Border>(panel);
  }

  public Panel Panel => this.labelsPanels;

  public Size DesiredSize => this.desiredSize;

  public ChartAxis Axis { get; set; }

  public List<UIElement> Children
  {
    get
    {
      this.children = this.textBlockRecycler.generatedElements.Cast<UIElement>().ToList<UIElement>();
      if (this.children.Count < 1)
        this.children = this.contentControlRecycler.generatedElements.Cast<UIElement>().ToList<UIElement>();
      return this.children;
    }
  }

  public double Left { get; set; }

  public double Top { get; set; }

  internal List<UIElement> children { get; set; }

  internal AxisLabelLayout LabelLayout { get; set; }

  internal Rect Bounds
  {
    get => this.bounds;
    set => this.bounds = value;
  }

  public Size Measure(Size availableSize)
  {
    this.LabelLayout = AxisLabelLayout.CreateAxisLayout(this.Axis, this.Children);
    if (this.borders != null)
      this.LabelLayout.Borders = this.borders;
    this.desiredSize = this.LabelLayout.Measure(availableSize);
    return this.desiredSize;
  }

  public Size Arrange(Size finalSize)
  {
    if (this.LabelLayout != null)
    {
      this.LabelLayout.Left = this.Left;
      this.LabelLayout.Top = this.Top;
      this.LabelLayout.Arrange(this.DesiredSize);
      this.LabelLayout = (AxisLabelLayout) null;
    }
    return finalSize;
  }

  public void DetachElements()
  {
    this.labelsPanels = (Panel) null;
    if (this.textBlockRecycler != null)
      this.textBlockRecycler.Clear();
    if (this.contentControlRecycler != null)
      this.contentControlRecycler.Clear();
    if (this.borderRecycler == null)
      return;
    this.borderRecycler.Clear();
  }

  public void UpdateElements()
  {
    if (this.Axis is ChartAxisBase2D axis && (!axis.ShowLabelBorder || axis.LabelBorderWidth == 0.0) && this.borderRecycler.Count > 0)
      this.borderRecycler.Clear();
    this.GenerateContainers();
  }

  internal void Dispose()
  {
    this.UnbindAndDetachContentControlRecyclerElements(true);
    if (this.textBlockRecycler != null && this.textBlockRecycler.Count > 0)
    {
      foreach (DependencyObject dependencyObject in this.textBlockRecycler)
        dependencyObject.ClearValue(FrameworkElement.TagProperty);
      this.textBlockRecycler.Clear();
      this.textBlockRecycler = (UIElementsRecycler<TextBlock>) null;
    }
    this.Axis = (ChartAxis) null;
  }

  internal void SetOffsetValues(double left, double top, double width, double height)
  {
    int count = this.Children.Count;
    if (count == 0)
      return;
    if (this.Axis.Orientation == Orientation.Horizontal)
    {
      double left1 = Canvas.GetLeft(this.Children[0]);
      double num = Canvas.GetLeft(this.Children[count - 1]) + this.Children[count - 1].DesiredSize.Width;
      if (left1 < this.Axis.ArrangeRect.Left)
      {
        left = left1;
        width += this.Axis.ArrangeRect.Left - left1;
      }
      if (this.Axis.ArrangeRect.Right < num)
        width += num - this.Axis.ArrangeRect.Width;
    }
    else
    {
      double top1 = Canvas.GetTop(this.Children[count - 1]);
      double num = Canvas.GetTop(this.Children[0]) + this.Children[0].DesiredSize.Height;
      if (top1 < this.Axis.ArrangeRect.Top)
      {
        top = top1;
        height += this.Axis.ArrangeRect.Top - top1;
      }
      if (this.Axis.ArrangeRect.Bottom < num)
        height += num - this.DesiredSize.Height;
    }
    this.Bounds = new Rect(left, top, width, height);
  }

  internal void GenerateContainers()
  {
    int index = 0;
    ObservableCollection<ChartAxisLabel> visibleLabels = this.Axis.VisibleLabels;
    DataTemplate prefixLabelTemplate = this.Axis.PrefixLabelTemplate;
    DataTemplate postfixLabelTemplate = this.Axis.PostfixLabelTemplate;
    if (this.Axis is ChartAxisBase2D axis && axis.ShowLabelBorder && axis.LabelBorderWidth > 0.0)
      this.GenerateBorder();
    if (this.Axis.LabelTemplate == null && this.Axis.PrefixLabelTemplate == null && this.Axis.PostfixLabelTemplate == null && this.Axis.LabelStyle == null)
    {
      this.contentControlRecycler.Clear();
      this.textBlockRecycler.GenerateElements(visibleLabels.Count);
      foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) visibleLabels)
      {
        if (chartAxisLabel.GetContent() != null)
        {
          TextBlock textBlock = this.textBlockRecycler[index];
          textBlock.Text = chartAxisLabel.GetContent().ToString();
          textBlock.Tag = (object) visibleLabels[index];
        }
        ++index;
      }
    }
    else if (this.Axis.LabelTemplate == null)
    {
      this.textBlockRecycler.Clear();
      this.UnbindAndDetachContentControlRecyclerElements(false);
      this.contentControlRecycler.GenerateElements(visibleLabels.Count);
      foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) visibleLabels)
      {
        ChartCartesianAxisLabelsPanel.ClearLabelBinding(this.contentControlRecycler[index]);
        ContentControl control = this.contentControlRecycler[index];
        chartAxisLabel.PrefixLabelTemplate = prefixLabelTemplate;
        chartAxisLabel.PostfixLabelTemplate = postfixLabelTemplate;
        this.SetLabelStyle(chartAxisLabel, control);
        control.Content = (object) chartAxisLabel;
        control.Tag = (object) visibleLabels[index];
        control.ContentTemplate = ChartDictionaries.GenericCommonDictionary[(object) "AxisLabelsCustomTemplate"] as DataTemplate;
        control.ApplyTemplate();
        ++index;
      }
    }
    else
    {
      this.textBlockRecycler.Clear();
      this.contentControlRecycler.GenerateElements(visibleLabels.Count);
      foreach (ChartAxisLabel chartAxisLabel in (Collection<ChartAxisLabel>) visibleLabels)
      {
        ContentControl contentControl = this.contentControlRecycler[index];
        contentControl.ContentTemplate = this.Axis.LabelTemplate;
        contentControl.ApplyTemplate();
        contentControl.Content = (object) chartAxisLabel;
        contentControl.Tag = (object) visibleLabels[index];
        ++index;
      }
    }
  }

  private void UnbindAndDetachContentControlRecyclerElements(bool isDisposing)
  {
    if (this.contentControlRecycler == null || this.contentControlRecycler.Count <= 0)
      return;
    foreach (ContentControl target in this.contentControlRecycler)
    {
      BindingOperations.ClearBinding((DependencyObject) target, Control.ForegroundProperty);
      BindingOperations.ClearBinding((DependencyObject) target, Control.FontSizeProperty);
      BindingOperations.ClearBinding((DependencyObject) target, Control.FontFamilyProperty);
      target.ClearValue(FrameworkElement.TagProperty);
      target.ClearValue(ContentControl.ContentProperty);
      target.ClearValue(FrameworkElement.DataContextProperty);
      target.Content = (object) null;
    }
    if (!isDisposing)
      return;
    this.contentControlRecycler.Clear();
  }

  private void SetLabelStyle(ChartAxisLabel chartAxisLabel, ContentControl control)
  {
    LabelStyle labelStyle = this.Axis.LabelStyle;
    ChartAxisRangeStyleCollection rangeStyles = this.Axis.RangeStyles;
    if (rangeStyles != null && rangeStyles.Count > 0)
    {
      foreach (ChartAxisRangeStyle chartAxisRangeStyle in (Collection<ChartAxisRangeStyle>) rangeStyles)
      {
        DoubleRange range = chartAxisRangeStyle.Range;
        if (range.Start <= chartAxisLabel.Position && range.End >= chartAxisLabel.Position && chartAxisRangeStyle.LabelStyle != null)
        {
          labelStyle = chartAxisRangeStyle.LabelStyle;
          break;
        }
      }
    }
    if (labelStyle == null)
      return;
    if (labelStyle.Foreground != null)
    {
      Binding binding = new Binding()
      {
        Source = (object) labelStyle,
        Path = new PropertyPath("Foreground", new object[0])
      };
      control.SetBinding(Control.ForegroundProperty, (BindingBase) binding);
    }
    if (labelStyle.FontSize > 0.0)
    {
      Binding binding = new Binding()
      {
        Source = (object) labelStyle,
        Path = new PropertyPath("FontSize", new object[0])
      };
      control.SetBinding(Control.FontSizeProperty, (BindingBase) binding);
    }
    if (labelStyle.FontFamily == null)
      return;
    Binding binding1 = new Binding()
    {
      Source = (object) labelStyle,
      Path = new PropertyPath("FontFamily", new object[0])
    };
    control.SetBinding(Control.FontFamilyProperty, (BindingBase) binding1);
  }

  private static void ClearLabelBinding(ContentControl contentControl)
  {
    contentControl.ClearValue(Control.ForegroundProperty);
    contentControl.ClearValue(Control.FontSizeProperty);
    contentControl.ClearValue(Control.FontFamilyProperty);
  }

  private void GenerateBorder()
  {
    if (!this.borderRecycler.BindingProvider.Keys.Contains<DependencyProperty>(UIElement.VisibilityProperty))
      this.borderRecycler.BindingProvider.Add(UIElement.VisibilityProperty, new Binding()
      {
        Source = (object) this.Axis,
        Path = new PropertyPath("Visibility", new object[0])
      });
    if (!this.borderRecycler.BindingProvider.Keys.Contains<DependencyProperty>(Border.BorderBrushProperty))
      this.borderRecycler.BindingProvider.Add(Border.BorderBrushProperty, new Binding()
      {
        Source = (object) this.Axis,
        Path = new PropertyPath("LabelBorderBrush", new object[0])
      });
    this.borderRecycler.GenerateElements(this.Axis.VisibleLabels.Count);
    this.borders = this.borderRecycler.generatedElements;
  }
}
