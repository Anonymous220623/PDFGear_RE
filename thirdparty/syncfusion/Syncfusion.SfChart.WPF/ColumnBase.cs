// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ColumnBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ColumnBase : SparklineBase
{
  public static readonly DependencyProperty HighlightSegmentProperty = DependencyProperty.Register(nameof (HighlightSegment), typeof (bool), typeof (ColumnBase), new PropertyMetadata((object) false));
  private Shape mouseUnderRect;
  private Shape previousRect;
  private BindingExpression bindingExpression;

  public bool HighlightSegment
  {
    get => (bool) this.GetValue(ColumnBase.HighlightSegmentProperty);
    set => this.SetValue(ColumnBase.HighlightSegmentProperty, (object) value);
  }

  internal override void SetBinding(Shape element)
  {
    base.SetBinding(element);
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    });
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    if (!this.HighlightSegment || !(e.OriginalSource is Shape))
      return;
    this.mouseUnderRect = (Shape) e.OriginalSource;
    string str = "";
    if ((object[]) this.mouseUnderRect.Tag != null)
      str = ((object[]) this.mouseUnderRect.Tag)[0] as string;
    if (this.mouseUnderRect == this.previousRect || !(str == "Selectable"))
      return;
    if (this.previousRect != null)
      this.ResetColor(this.previousRect);
    this.ApplySelectionColor(this.mouseUnderRect);
    this.previousRect = this.mouseUnderRect;
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    this.ResetColor(this.previousRect);
    this.previousRect = this.mouseUnderRect = (Shape) null;
  }

  protected override void AnimateSegments(UIElementCollection elements)
  {
    int index = 0;
    foreach (UIElement element1 in elements)
    {
      Storyboard storyboard = new Storyboard();
      Shape shape = element1 as Shape;
      if (!double.IsNaN(shape.Height))
      {
        shape.RenderTransform = (Transform) new ScaleTransform();
        if (this.yValues[index] > 0.0)
          shape.RenderTransformOrigin = new Point(1.0, 1.0);
        DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
        keyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
        keyFrame1.Value = 0.0;
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
        SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
        keyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0));
        keyFrame2.KeySpline = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
        keyFrame2.Value = 1.0;
        Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)", new object[0]));
        Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) element1);
        storyboard.Children.Add((Timeline) element2);
        storyboard.Begin();
        ++index;
      }
    }
  }

  protected override void RenderSegments()
  {
    this.ClearUnUsedSegments(this.xValues.Count - this.EmptyPointIndexes.Count);
  }

  protected void BindFillProperty(Shape element, string propertyPath)
  {
    base.SetBinding(element);
    element.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath(propertyPath, new object[0])
    });
  }

  private void ApplySelectionColor(Shape segment)
  {
    this.bindingExpression = segment.GetBindingExpression(Shape.FillProperty);
    Color color = (segment.Fill as SolidColorBrush).Color;
    segment.Fill = (Brush) new SolidColorBrush(Color.FromArgb(color.A, (byte) ((double) color.R * 0.6), (byte) ((double) color.G * 0.6), (byte) ((double) color.B * 0.6)));
  }

  private void ResetColor(Shape segment)
  {
    segment?.SetBinding(Shape.FillProperty, (BindingBase) this.bindingExpression.ParentBinding);
  }
}
