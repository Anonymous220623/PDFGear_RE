// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StraightLineAnnotation
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class StraightLineAnnotation : LineAnnotation
{
  public static readonly DependencyProperty AxisLabelTemplateProperty = DependencyProperty.Register(nameof (AxisLabelTemplate), typeof (DataTemplate), typeof (StraightLineAnnotation), new PropertyMetadata((object) null, new PropertyChangedCallback(StraightLineAnnotation.OnAxisLabelTemplateChanged)));
  public static readonly DependencyProperty ShowAxisLabelProperty = DependencyProperty.Register(nameof (ShowAxisLabel), typeof (bool), typeof (StraightLineAnnotation), new PropertyMetadata((object) false, new PropertyChangedCallback(StraightLineAnnotation.OnShowAxisLabelChanged)));
  internal static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register(nameof (LabelPosition), typeof (AxisElementPosition), typeof (StraightLineAnnotation), new PropertyMetadata((object) AxisElementPosition.Outside, new PropertyChangedCallback(StraightLineAnnotation.OnAxisLabelPositionChanged)));
  internal static readonly DependencyProperty AxisLabelAlignmentProperty = DependencyProperty.Register(nameof (AxisLabelAlignment), typeof (LabelAlignment), typeof (StraightLineAnnotation), new PropertyMetadata((object) LabelAlignment.Center, new PropertyChangedCallback(StraightLineAnnotation.OnAxisLabelPositionChanged)));
  internal AxisMarker AxisMarkerObject = new AxisMarker();

  public StraightLineAnnotation()
  {
    this.AxisMarkerObject.ParentAnnotation = (ShapeAnnotation) this;
  }

  public new event EventHandler DragStarted
  {
    add
    {
      this.AxisMarkerObject.DragStarted += value;
      this.AxisMarkerObject.ParentAnnotation.DragStarted += value;
    }
    remove
    {
      this.AxisMarkerObject.DragStarted -= value;
      this.AxisMarkerObject.ParentAnnotation.DragStarted -= value;
    }
  }

  public new event EventHandler<AnnotationDragDeltaEventArgs> DragDelta
  {
    add
    {
      this.AxisMarkerObject.DragDelta += value;
      this.AxisMarkerObject.ParentAnnotation.DragDelta += value;
    }
    remove
    {
      this.AxisMarkerObject.DragDelta -= value;
      this.AxisMarkerObject.ParentAnnotation.DragDelta -= value;
    }
  }

  public new event EventHandler<AnnotationDragCompletedEventArgs> DragCompleted
  {
    add
    {
      this.AxisMarkerObject.DragCompleted += value;
      this.AxisMarkerObject.ParentAnnotation.DragCompleted += value;
    }
    remove
    {
      this.AxisMarkerObject.DragCompleted -= value;
      this.AxisMarkerObject.ParentAnnotation.DragCompleted -= value;
    }
  }

  public new event EventHandler Selected
  {
    add
    {
      this.AxisMarkerObject.Selected += value;
      this.AxisMarkerObject.ParentAnnotation.Selected += value;
    }
    remove
    {
      this.AxisMarkerObject.Selected -= value;
      this.AxisMarkerObject.ParentAnnotation.Selected -= value;
    }
  }

  public new event EventHandler UnSelected
  {
    add
    {
      this.AxisMarkerObject.UnSelected += value;
      this.AxisMarkerObject.ParentAnnotation.UnSelected += value;
    }
    remove
    {
      this.AxisMarkerObject.UnSelected -= value;
      this.AxisMarkerObject.ParentAnnotation.UnSelected -= value;
    }
  }

  public DataTemplate AxisLabelTemplate
  {
    get => (DataTemplate) this.GetValue(StraightLineAnnotation.AxisLabelTemplateProperty);
    set => this.SetValue(StraightLineAnnotation.AxisLabelTemplateProperty, (object) value);
  }

  public bool ShowAxisLabel
  {
    get => (bool) this.GetValue(StraightLineAnnotation.ShowAxisLabelProperty);
    set => this.SetValue(StraightLineAnnotation.ShowAxisLabelProperty, (object) value);
  }

  internal LabelAlignment AxisLabelAlignment
  {
    get => (LabelAlignment) this.GetValue(StraightLineAnnotation.AxisLabelAlignmentProperty);
    set => this.SetValue(StraightLineAnnotation.AxisLabelAlignmentProperty, (object) value);
  }

  internal AxisElementPosition LabelPosition
  {
    get => (AxisElementPosition) this.GetValue(StraightLineAnnotation.LabelPositionProperty);
    set => this.SetValue(StraightLineAnnotation.LabelPositionProperty, (object) value);
  }

  internal override SfChart Chart
  {
    get => this.chart;
    set
    {
      if (this.chart != null)
        this.chart.SizeChanged -= new SizeChangedEventHandler(this.OnChartSizeChanged);
      this.chart = value;
      if (this.chart != null)
        this.chart.SizeChanged += new SizeChangedEventHandler(this.OnChartSizeChanged);
      this.SetAxisFromName();
    }
  }

  internal override bool IsPointInsideRectangle(Point point) => this.RotatedRect.Contains(point);

  internal override void OnVisibilityChanged()
  {
    if (this.chart == null || this.chart.AnnotationManager == null)
      return;
    this.IsVisbilityChanged = true;
    if (this.Visibility.Equals((object) Visibility.Collapsed) || this.Visibility.Equals((object) Visibility.Hidden))
      this.chart.AnnotationManager.AddOrRemoveAnnotations((Annotation) this, true);
    else
      this.chart.AnnotationManager.AddOrRemoveAnnotations((Annotation) this, false);
  }

  protected void SetAxisMarkerValue(
    object X1,
    object X2,
    object Y1,
    object Y2,
    AxisMode axisMode)
  {
    this.AxisMarkerObject.X1 = X1;
    this.AxisMarkerObject.X2 = X2;
    this.AxisMarkerObject.Y1 = Y1;
    this.AxisMarkerObject.Y2 = Y2;
    this.AxisMarkerObject.XAxisName = this.XAxisName;
    this.AxisMarkerObject.YAxisName = this.YAxisName;
    this.AxisMarkerObject.YAxis = this.YAxis;
    this.AxisMarkerObject.XAxis = this.XAxis;
    this.AxisMarkerObject.DraggingMode = axisMode;
  }

  private static void OnAxisLabelTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as StraightLineAnnotation).OnAxisTemplateChanged(e.NewValue);
  }

  private static void OnShowAxisLabelChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as StraightLineAnnotation).OnShowAxisLabelChanged(Convert.ToBoolean(e.NewValue));
  }

  private static void OnAxisLabelPositionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as StraightLineAnnotation).UpdateAnnotation();
  }

  private void OnChartSizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (this.CoordinateUnit != CoordinateUnit.Pixel)
      return;
    this.UpdateAnnotation();
  }

  private void OnAxisTemplateChanged(object axisTemplate)
  {
    ContentControl markerContent = this.AxisMarkerObject.markerContent;
    if (markerContent == null)
      return;
    markerContent.ContentTemplate = axisTemplate == null ? ChartDictionaries.GenericCommonDictionary[(object) "AxisLabel"] as DataTemplate : axisTemplate as DataTemplate;
  }

  private void OnShowAxisLabelChanged(bool isShow)
  {
    if (isShow)
    {
      this.UpdateAnnotation();
    }
    else
    {
      if (this.AxisMarkerObject == null)
        return;
      this.Chart.AnnotationManager.AddOrRemoveAnnotations((Annotation) this.AxisMarkerObject, true);
    }
  }
}
