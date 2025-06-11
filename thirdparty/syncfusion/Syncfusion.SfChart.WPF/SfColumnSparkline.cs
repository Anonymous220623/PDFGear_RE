// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfColumnSparkline
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SfColumnSparkline : ColumnBase
{
  public static readonly DependencyProperty XBindingPathProperty = DependencyProperty.Register(nameof (XBindingPath), typeof (string), typeof (SfColumnSparkline), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty AxisStyleProperty = DependencyProperty.Register(nameof (AxisStyle), typeof (Style), typeof (SfColumnSparkline), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ShowAxisProperty = DependencyProperty.Register(nameof (ShowAxis), typeof (bool), typeof (SfColumnSparkline), new PropertyMetadata((object) false, new PropertyChangedCallback(SfColumnSparkline.OnShowAxisChanged)));
  public static readonly DependencyProperty AxisOriginProperty = DependencyProperty.Register(nameof (AxisOrigin), typeof (double), typeof (SfColumnSparkline), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(SfColumnSparkline.OnAxisOriginChanged)));
  public static readonly DependencyProperty SegmentTemplateSelectorProperty = DependencyProperty.Register(nameof (SegmentTemplateSelector), typeof (TemplateSelector), typeof (SfColumnSparkline), new PropertyMetadata((PropertyChangedCallback) null));
  private Rectangle rectSegment;
  private Line axisLine;

  public string XBindingPath
  {
    get => (string) this.GetValue(SfColumnSparkline.XBindingPathProperty);
    set => this.SetValue(SfColumnSparkline.XBindingPathProperty, (object) value);
  }

  public Style AxisStyle
  {
    get => (Style) this.GetValue(SfColumnSparkline.AxisStyleProperty);
    set => this.SetValue(SfColumnSparkline.AxisStyleProperty, (object) value);
  }

  public bool ShowAxis
  {
    get => (bool) this.GetValue(SfColumnSparkline.ShowAxisProperty);
    set => this.SetValue(SfColumnSparkline.ShowAxisProperty, (object) value);
  }

  public double AxisOrigin
  {
    get => (double) this.GetValue(SfColumnSparkline.AxisOriginProperty);
    set => this.SetValue(SfColumnSparkline.AxisOriginProperty, (object) value);
  }

  public TemplateSelector SegmentTemplateSelector
  {
    get => (TemplateSelector) this.GetValue(SfColumnSparkline.SegmentTemplateSelectorProperty);
    set => this.SetValue(SfColumnSparkline.SegmentTemplateSelectorProperty, (object) value);
  }

  protected override void GeneratePoints(string xPath) => base.GeneratePoints(this.XBindingPath);

  protected override void SetIndividualPoints(int index, object obj, bool replace, string xPath)
  {
    base.SetIndividualPoints(index, obj, replace, this.XBindingPath);
  }

  protected void RemoveAxis()
  {
    if (this.RootPanel.Children.Contains((UIElement) this.axisLine))
      this.RootPanel.Children.Remove((UIElement) this.axisLine);
    this.axisLine = (Line) null;
  }

  protected virtual void UpdateHorizontalAxis()
  {
    if (this.RootPanel == null || !this.ShowAxis)
      return;
    if (this.axisLine == null)
    {
      this.axisLine = new Line();
      this.StyleBinding((FrameworkElement) this.axisLine, this.AxisStyle, "AxisStyle");
      this.RootPanel.Children.Add((UIElement) this.axisLine);
    }
    Point visible = this.TransformToVisible(0.0, this.AxisOrigin);
    this.axisLine.X1 = 0.0;
    this.axisLine.X2 = this.availableWidth;
    this.axisLine.Y1 = visible.Y;
    this.axisLine.Y2 = visible.Y;
  }

  protected override void UpdateMinMaxValues()
  {
    base.UpdateMinMaxValues();
    if (!this.IsIndexed)
      return;
    this.maxXValue += 0.8;
    this.deltaX = this.maxXValue - this.minXValue;
  }

  protected override void RenderSegments()
  {
    if (this.yValues == null)
      return;
    double num1;
    double num2;
    if (!this.IsIndexed)
    {
      num1 = 0.4;
      num2 = -0.4;
    }
    else
    {
      num2 = 0.0;
      num1 = 0.8;
    }
    if (this.ShowAxis)
      this.UpdateHorizontalAxis();
    base.RenderSegments();
    if (this.SegmentTemplateSelector != null)
      this.SegmentTemplateSelector.SetData((SparklineBase) this, this.DataCount);
    int count = this.yValues.Count;
    for (int index = 0; index < count; ++index)
    {
      double xValue = this.xValues[index];
      double yValue = this.yValues[index];
      if (!double.IsNaN(yValue))
      {
        Rect rect = new Rect(this.TransformToVisible(xValue + num2, yValue), this.TransformToVisible(xValue + num1, 0.0));
        if (this.SegmentPresenter.Children.Count > index)
        {
          this.rectSegment = this.SegmentPresenter.Children[index] as Rectangle;
        }
        else
        {
          this.rectSegment = new Rectangle();
          this.BindFillProperty((Shape) this.rectSegment, "Interior");
          this.SegmentPresenter.Children.Add((UIElement) this.rectSegment);
          this.rectSegment.Tag = (object) new object[3]
          {
            (object) "Selectable",
            (object) xValue,
            (object) yValue
          };
        }
        if (this.SegmentTemplateSelector != null)
          (this.SegmentTemplateSelector as Syncfusion.UI.Xaml.Charts.SegmentTemplateSelector).BindVisual(xValue, yValue, (Shape) this.rectSegment);
        this.rectSegment.Width = rect.Width;
        this.rectSegment.SetValue(Canvas.LeftProperty, (object) rect.X);
        this.rectSegment.Height = rect.Height;
        this.rectSegment.SetValue(Canvas.TopProperty, (object) rect.Y);
      }
    }
  }

  private static void OnShowAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SfColumnSparkline sfColumnSparkline = d as SfColumnSparkline;
    if (!(bool) e.NewValue)
      sfColumnSparkline.RemoveAxis();
    else
      sfColumnSparkline.UpdateHorizontalAxis();
  }

  private static void OnAxisOriginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as SfColumnSparkline).UpdateHorizontalAxis();
  }
}
