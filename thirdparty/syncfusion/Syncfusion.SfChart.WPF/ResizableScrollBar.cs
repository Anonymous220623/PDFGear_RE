// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ResizableScrollBar
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

[TemplatePart(Name = "VerticalThumbHand1", Type = typeof (Thumb))]
[TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
[TemplateVisualState(Name = "OnFocus", GroupName = "TouchMode")]
[TemplateVisualState(Name = "OnLostFocus", GroupName = "TouchMode")]
[TemplateVisualState(Name = "OnExit", GroupName = "TouchMode")]
[TemplateVisualState(Name = "OnView", GroupName = "TouchMode")]
[TemplatePart(Name = "HorizontalThumbHand1", Type = typeof (Popup))]
[TemplatePart(Name = "VerticalRoot", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "HorizontalRoot", Type = typeof (FrameworkElement))]
[TemplatePart(Name = "HorizontalLargeIncrease", Type = typeof (RepeatButton))]
[TemplatePart(Name = "HorizontalLargeDecrease", Type = typeof (RepeatButton))]
[TemplatePart(Name = "HorizontalSmallDecrease", Type = typeof (RepeatButton))]
[TemplatePart(Name = "HorizontalSmallIncrease", Type = typeof (RepeatButton))]
[TemplatePart(Name = "HorizontalThumb", Type = typeof (Thumb))]
[TemplatePart(Name = "VerticalSmallIncrease", Type = typeof (RepeatButton))]
[TemplatePart(Name = "HorizontalThumbHand2", Type = typeof (Popup))]
[TemplatePart(Name = "VerticalLargeIncrease", Type = typeof (RepeatButton))]
[TemplatePart(Name = "VerticalLargeDecrease", Type = typeof (RepeatButton))]
[TemplatePart(Name = "VerticalThumb", Type = typeof (Thumb))]
[TemplatePart(Name = "VerticalThumbHand2", Type = typeof (Thumb))]
[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
[TemplatePart(Name = "VerticalSmallDecrease", Type = typeof (RepeatButton))]
public class ResizableScrollBar : ContentControl
{
  private const double GapSize = 4.0;
  private const double MinimumThumbSize = 0.0;
  private const double ResizableBarSize = 15.0;
  private const double MinimumDiff = 0.0;
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (ResizableScrollBar), new PropertyMetadata((object) Orientation.Vertical, new PropertyChangedCallback(ResizableScrollBar.OnOrientationChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (ResizableScrollBar), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (ResizableScrollBar), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ViewSizePortProperty = DependencyProperty.Register(nameof (ViewSizePort), typeof (double), typeof (ResizableScrollBar), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(nameof (SmallChange), typeof (double), typeof (ResizableScrollBar), new PropertyMetadata((object) 0.01));
  public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(nameof (LargeChange), typeof (double), typeof (ResizableScrollBar), new PropertyMetadata((object) 0.1));
  public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof (Scale), typeof (double), typeof (ResizableScrollBar), new PropertyMetadata((object) 1.0));
  public static readonly DependencyProperty RangeStartProperty = DependencyProperty.Register(nameof (RangeStart), typeof (double), typeof (ResizableScrollBar), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ResizableScrollBar.OnRangeChanged)));
  public static readonly DependencyProperty RangeEndProperty = DependencyProperty.Register(nameof (RangeEnd), typeof (double), typeof (ResizableScrollBar), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ResizableScrollBar.OnRangeChanged)));
  public static readonly DependencyProperty ScrollButtonVisibilityProperty = DependencyProperty.Register(nameof (ScrollButtonVisibility), typeof (Visibility), typeof (ResizableScrollBar), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(ResizableScrollBar.OnIncreaseDecreaseVisibilityChanged)));
  public static readonly DependencyProperty EnableTouchModeProperty = DependencyProperty.Register(nameof (EnableTouchMode), typeof (bool), typeof (ResizableScrollBar), new PropertyMetadata((object) false));
  public bool isFarDragged;
  public bool isNearDragged;
  internal bool canDrag;
  private Grid horizontalRoot;
  private Grid verticalRoot;
  private Size desiredSize;
  private Size availableSize;
  private double resizeThumbSize;
  private double smallThumbSize;
  private double actualTrackSize;
  private double actualSize;
  private double previousThumbSize;
  private double rangeDiff;
  private double middleThumbSize;
  private double largeDecreaseThumbSize;
  private double largeIncreaseThumbSize;
  private double actualLargeThumbSize;

  public ResizableScrollBar() => this.DefaultStyleKey = (object) typeof (ResizableScrollBar);

  public event EventHandler ValueChanged;

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(ResizableScrollBar.OrientationProperty);
    set => this.SetValue(ResizableScrollBar.OrientationProperty, (object) value);
  }

  public double Maximum
  {
    get => (double) this.GetValue(ResizableScrollBar.MaximumProperty);
    set => this.SetValue(ResizableScrollBar.MaximumProperty, (object) value);
  }

  public double Minimum
  {
    get => (double) this.GetValue(ResizableScrollBar.MinimumProperty);
    set => this.SetValue(ResizableScrollBar.MinimumProperty, (object) value);
  }

  public double ViewSizePort
  {
    get => (double) this.GetValue(ResizableScrollBar.ViewSizePortProperty);
    set => this.SetValue(ResizableScrollBar.ViewSizePortProperty, (object) value);
  }

  public double SmallChange
  {
    get => (double) this.GetValue(ResizableScrollBar.SmallChangeProperty);
    set => this.SetValue(ResizableScrollBar.SmallChangeProperty, (object) value);
  }

  public double LargeChange
  {
    get => (double) this.GetValue(ResizableScrollBar.LargeChangeProperty);
    set => this.SetValue(ResizableScrollBar.LargeChangeProperty, (object) value);
  }

  public double Scale
  {
    get => (double) this.GetValue(ResizableScrollBar.ScaleProperty);
    set => this.SetValue(ResizableScrollBar.ScaleProperty, (object) value);
  }

  public double RangeStart
  {
    get => (double) this.GetValue(ResizableScrollBar.RangeStartProperty);
    set => this.SetValue(ResizableScrollBar.RangeStartProperty, (object) value);
  }

  public double RangeEnd
  {
    get => (double) this.GetValue(ResizableScrollBar.RangeEndProperty);
    set => this.SetValue(ResizableScrollBar.RangeEndProperty, (object) value);
  }

  public Visibility ScrollButtonVisibility
  {
    get => (Visibility) this.GetValue(ResizableScrollBar.ScrollButtonVisibilityProperty);
    set => this.SetValue(ResizableScrollBar.ScrollButtonVisibilityProperty, (object) value);
  }

  public bool EnableTouchMode
  {
    get => (bool) this.GetValue(ResizableScrollBar.EnableTouchModeProperty);
    set => this.SetValue(ResizableScrollBar.EnableTouchModeProperty, (object) value);
  }

  public double ResizableThumbSize => this.resizeThumbSize;

  internal Size AvailabeSize => this.availableSize;

  internal double TrackSize => this.actualTrackSize;

  protected internal bool IsValueChangedTrigger { get; set; }

  protected Thumb NearHand { get; set; }

  protected Thumb FarHand { get; set; }

  protected Thumb MiddleThumb { get; set; }

  protected RepeatButton SmallDecrease { get; set; }

  protected RepeatButton LargeDecrease { get; set; }

  protected RepeatButton LargeIncrease { get; set; }

  protected RepeatButton SmallIncrease { get; set; }

  public override void OnApplyTemplate() => this.ApplyOrientationTemplate();

  protected virtual void OnOrientationChanged(DependencyPropertyChangedEventArgs e)
  {
    this.ApplyOrientationTemplate();
  }

  protected virtual void OnRangeValueChanged()
  {
    this.RangeMinMax();
    this.Scale = (this.Maximum - this.Minimum) / (this.RangeEnd - this.RangeStart);
    this.InvalidateArrange();
    if (this.IsValueChangedTrigger)
      this.OnValueChanged();
    this.IsValueChangedTrigger = !this.canDrag;
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    base.MeasureOverride(availableSize);
    this.availableSize = ChartLayoutUtils.CheckSize(availableSize);
    if (this.Orientation == Orientation.Horizontal && this.NearHand != null && this.FarHand != null)
    {
      this.desiredSize.Width = this.availableSize.Width;
      this.desiredSize.Height = 15.0;
      if (this.NearHand.Visibility != Visibility.Collapsed)
      {
        this.NearHand.Measure(availableSize);
        Size desiredSize = this.NearHand.DesiredSize;
        this.resizeThumbSize = desiredSize.Width != 0.0 ? desiredSize.Width : this.NearHand.MinWidth;
        this.NearHand.Width = this.resizeThumbSize;
        this.FarHand.Width = this.resizeThumbSize;
      }
      else
        this.resizeThumbSize = 0.0;
      if (this.ScrollButtonVisibility != Visibility.Collapsed)
      {
        this.SmallIncrease.Measure(availableSize);
        Size desiredSize = this.SmallIncrease.DesiredSize;
        this.smallThumbSize = desiredSize.Width != 0.0 ? (this.smallThumbSize == 0.0 ? desiredSize.Width : Math.Min(this.smallThumbSize, desiredSize.Width)) : this.SmallIncrease.MinWidth;
        this.SmallDecrease.Width = this.smallThumbSize;
        this.SmallIncrease.Width = this.smallThumbSize;
      }
      else
      {
        this.SmallDecrease.Width = this.smallThumbSize = 0.0;
        this.SmallIncrease.Width = this.smallThumbSize = 0.0;
      }
    }
    else if (this.Orientation == Orientation.Vertical && this.FarHand != null && this.NearHand != null)
    {
      this.desiredSize.Height = this.availableSize.Height;
      this.desiredSize.Width = 15.0;
      this.NearHand.Measure(availableSize);
      if (this.NearHand.Visibility != Visibility.Collapsed)
      {
        Size desiredSize = this.NearHand.DesiredSize;
        this.resizeThumbSize = desiredSize.Height != 0.0 ? desiredSize.Height : this.NearHand.MinHeight;
        this.NearHand.Height = this.resizeThumbSize;
        this.FarHand.Height = this.resizeThumbSize;
      }
      else
        this.resizeThumbSize = 0.0;
      if (this.ScrollButtonVisibility != Visibility.Collapsed)
      {
        this.SmallIncrease.Measure(availableSize);
        Size desiredSize = this.SmallIncrease.DesiredSize;
        this.smallThumbSize = desiredSize.Width != 0.0 ? (this.smallThumbSize = this.smallThumbSize == 0.0 ? desiredSize.Height : Math.Min(this.smallThumbSize, desiredSize.Height)) : this.SmallIncrease.MinHeight;
        this.SmallDecrease.Height = this.smallThumbSize;
        this.SmallIncrease.Height = this.smallThumbSize;
      }
      else
      {
        this.SmallDecrease.Height = this.smallThumbSize = 0.0;
        this.SmallIncrease.Height = this.smallThumbSize = 0.0;
      }
    }
    return this.desiredSize;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    base.ArrangeOverride(finalSize);
    this.actualSize = this.Orientation == Orientation.Horizontal ? finalSize.Width : finalSize.Height;
    this.actualTrackSize = this.actualSize - 2.0 * this.smallThumbSize - (this.EnableTouchMode ? 0.0 : 2.0 * this.resizeThumbSize);
    this.CalculateSize();
    this.actualLargeThumbSize = this.EnableTouchMode ? this.actualSize : this.actualTrackSize;
    this.ThumbMinMax();
    this.rangeDiff = this.rangeDiff == 0.0 ? this.SmallChange : this.rangeDiff;
    if (this.Orientation == Orientation.Horizontal)
    {
      this.MiddleThumb.Width = this.middleThumbSize;
      this.LargeDecrease.Width = this.largeDecreaseThumbSize;
      this.LargeIncrease.Width = this.largeIncreaseThumbSize;
    }
    else if (this.Orientation == Orientation.Vertical)
    {
      this.MiddleThumb.Height = this.middleThumbSize;
      this.LargeDecrease.Height = this.largeDecreaseThumbSize;
      this.LargeIncrease.Height = this.largeIncreaseThumbSize;
    }
    return finalSize;
  }

  protected virtual void OnThumbDragged(object sender, DragDeltaEventArgs e)
  {
    if (this.Orientation != Orientation.Horizontal)
    {
      double height = this.MiddleThumb.Height;
    }
    else
    {
      double width = this.MiddleThumb.Width;
    }
    if (this.RangeStart == this.Minimum && this.RangeEnd == this.Maximum)
      return;
    this.CalculateRangeDifference();
    double num1 = this.Orientation == Orientation.Horizontal ? e.HorizontalChange : -e.VerticalChange;
    if (num1 < 0.0 && this.RangeStart != this.Minimum)
      this.canDrag = true;
    else if (num1 > 0.0 && this.RangeEnd != this.Maximum)
      this.canDrag = true;
    if (!this.canDrag)
      return;
    double num2 = num1 * (this.Maximum - this.Minimum) / this.actualTrackSize;
    this.IsValueChangedTrigger = false;
    this.CalculateChange(num2, num2);
    this.RangeStart = this.RangeStart > this.Maximum - this.rangeDiff ? this.Maximum - this.rangeDiff : this.RangeStart;
    this.RangeEnd = this.RangeEnd < this.rangeDiff ? this.rangeDiff : this.RangeEnd;
    this.isFarDragged = false;
    this.isNearDragged = false;
    this.OnValueChanged();
    this.CalculateOperations();
  }

  protected virtual void OnFarHandDragged(object sender, DragDeltaEventArgs e)
  {
    double num1 = this.actualTrackSize * (this.Orientation == Orientation.Horizontal ? e.HorizontalChange : -e.VerticalChange) / (this.actualTrackSize - (this.EnableTouchMode ? 0.0 : 2.0 * this.resizeThumbSize));
    double endChange = num1 * (this.Maximum - this.Minimum) / this.actualSize;
    double num2 = this.Orientation == Orientation.Horizontal ? this.MiddleThumb.Width : this.MiddleThumb.Height;
    bool flag = false;
    if (this.RangeEnd + endChange <= this.Maximum)
    {
      if (this.RangeEnd + endChange - this.RangeStart >= 0.0 * this.SmallChange)
        flag = true;
    }
    else if (this.RangeEnd < this.Maximum)
    {
      this.RangeEnd = this.Maximum;
      flag = true;
    }
    if (!flag || num2 == 0.0 && num1 <= 0.0)
      return;
    this.IsValueChangedTrigger = false;
    this.rangeDiff = 0.0 * this.SmallChange;
    this.CalculateChange(0.0, endChange);
    this.Scale = (this.Maximum - this.Minimum) / (this.RangeEnd - this.RangeStart);
    this.CalculateOperations();
    this.isFarDragged = true;
    this.isNearDragged = false;
    this.OnValueChanged();
  }

  protected virtual void OnNearHandDragged(object sender, DragDeltaEventArgs e)
  {
    double num1 = this.actualTrackSize * (this.Orientation == Orientation.Horizontal ? e.HorizontalChange : -e.VerticalChange) / (this.actualTrackSize - (this.EnableTouchMode ? 0.0 : 2.0 * this.resizeThumbSize));
    double startChange = num1 * (this.Maximum - this.Minimum) / this.actualSize;
    double num2 = this.Orientation == Orientation.Horizontal ? this.MiddleThumb.Width : this.MiddleThumb.Height;
    bool flag = false;
    if (this.RangeStart + startChange >= this.Minimum)
    {
      if (this.RangeEnd - (this.RangeStart + startChange) >= 0.0 * this.SmallChange)
        flag = true;
    }
    else if (this.RangeStart > this.Minimum)
    {
      this.RangeStart = this.Minimum;
      flag = true;
    }
    if (!flag || num2 == 0.0 && num1 >= 0.0)
      return;
    this.IsValueChangedTrigger = false;
    this.rangeDiff = 0.0 * this.SmallChange;
    this.CalculateChange(startChange, 0.0);
    this.Scale = (this.Maximum - this.Minimum) / (this.RangeEnd - this.RangeStart);
    this.CalculateOperations();
    this.isFarDragged = false;
    this.isNearDragged = true;
    this.OnValueChanged();
  }

  protected virtual void OnValueChanged()
  {
    if (this.ValueChanged != null)
    {
      this.ValueChanged((object) this, EventArgs.Empty);
      this.IsValueChangedTrigger = true;
    }
    this.canDrag = false;
  }

  private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ResizableScrollBar).OnOrientationChanged(e);
  }

  private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ResizableScrollBar).OnRangeValueChanged();
  }

  private static void OnIncreaseDecreaseVisibilityChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ResizableScrollBar resizableScrollBar = d as ResizableScrollBar;
    resizableScrollBar.MeasureOverride(resizableScrollBar.AvailabeSize);
  }

  private void ApplyOrientationTemplate()
  {
    this.horizontalRoot = this.GetTemplateChild("HorizontalRoot") as Grid;
    this.verticalRoot = this.GetTemplateChild("VerticalRoot") as Grid;
    if (this.LargeIncrease != null)
      this.LargeIncrease.Click -= new RoutedEventHandler(this.OnLargeIncreaseClick);
    if (this.LargeDecrease != null)
      this.LargeDecrease.Click -= new RoutedEventHandler(this.OnLargeDecreaseClick);
    if (this.SmallDecrease != null)
      this.SmallDecrease.Click -= new RoutedEventHandler(this.OnSmallDecreaseClick);
    if (this.SmallIncrease != null)
      this.SmallIncrease.Click -= new RoutedEventHandler(this.OnSmallIncreaseClick);
    if (this.NearHand != null)
      this.NearHand.DragDelta -= new DragDeltaEventHandler(this.OnNearHandDragged);
    if (this.FarHand != null)
      this.FarHand.DragDelta -= new DragDeltaEventHandler(this.OnFarHandDragged);
    if (this.MiddleThumb != null)
      this.MiddleThumb.DragDelta -= new DragDeltaEventHandler(this.OnThumbDragged);
    if (this.Orientation == Orientation.Horizontal && this.horizontalRoot != null)
    {
      this.horizontalRoot.Visibility = Visibility.Visible;
      this.verticalRoot.Visibility = Visibility.Collapsed;
      this.NearHand = this.GetTemplateChild("HorizontalThumbHand1") as Thumb;
      this.FarHand = this.GetTemplateChild("HorizontalThumbHand2") as Thumb;
      this.MiddleThumb = this.GetTemplateChild("HorizontalThumb") as Thumb;
      this.SmallDecrease = this.GetTemplateChild("HorizontalSmallDecrease") as RepeatButton;
      this.LargeDecrease = this.GetTemplateChild("HorizontalLargeDecrease") as RepeatButton;
      this.LargeIncrease = this.GetTemplateChild("HorizontalLargeIncrease") as RepeatButton;
      this.SmallIncrease = this.GetTemplateChild("HorizontalSmallIncrease") as RepeatButton;
      this.NearHand.DragDelta += new DragDeltaEventHandler(this.OnNearHandDragged);
      this.FarHand.DragDelta += new DragDeltaEventHandler(this.OnFarHandDragged);
      this.MiddleThumb.DragDelta += new DragDeltaEventHandler(this.OnThumbDragged);
      this.LargeIncrease.Click += new RoutedEventHandler(this.OnLargeIncreaseClick);
      this.LargeDecrease.Click += new RoutedEventHandler(this.OnLargeDecreaseClick);
      this.SmallDecrease.Click += new RoutedEventHandler(this.OnSmallDecreaseClick);
      this.SmallIncrease.Click += new RoutedEventHandler(this.OnSmallIncreaseClick);
    }
    else if (this.verticalRoot != null)
    {
      this.horizontalRoot.Visibility = Visibility.Collapsed;
      this.verticalRoot.Visibility = Visibility.Visible;
      this.NearHand = this.GetTemplateChild("VerticalThumbHand1") as Thumb;
      this.FarHand = this.GetTemplateChild("VerticalThumbHand2") as Thumb;
      this.MiddleThumb = this.GetTemplateChild("VerticalThumb") as Thumb;
      this.SmallDecrease = this.GetTemplateChild("VerticalSmallDecrease") as RepeatButton;
      this.LargeDecrease = this.GetTemplateChild("VerticalLargeDecrease") as RepeatButton;
      this.LargeIncrease = this.GetTemplateChild("VerticalLargeIncrease") as RepeatButton;
      this.SmallIncrease = this.GetTemplateChild("VerticalSmallIncrease") as RepeatButton;
      this.NearHand.DragDelta += new DragDeltaEventHandler(this.OnNearHandDragged);
      this.FarHand.DragDelta += new DragDeltaEventHandler(this.OnFarHandDragged);
      this.MiddleThumb.DragDelta += new DragDeltaEventHandler(this.OnThumbDragged);
      this.LargeIncrease.Click += new RoutedEventHandler(this.OnLargeIncreaseClick);
      this.LargeDecrease.Click += new RoutedEventHandler(this.OnLargeDecreaseClick);
      this.SmallDecrease.Click += new RoutedEventHandler(this.OnSmallDecreaseClick);
      this.SmallIncrease.Click += new RoutedEventHandler(this.OnSmallIncreaseClick);
    }
    this.IsValueChangedTrigger = true;
  }

  private void OnSmallIncreaseClick(object sender, RoutedEventArgs e)
  {
    if (this.RangeEnd == this.Maximum)
      return;
    this.IncreaseDecreaseOperation(this.SmallChange, this.SmallChange);
  }

  private void OnSmallDecreaseClick(object sender, RoutedEventArgs e)
  {
    if (this.RangeStart == this.Minimum)
      return;
    this.IncreaseDecreaseOperation(-this.SmallChange, -this.SmallChange);
  }

  private void OnLargeDecreaseClick(object sender, RoutedEventArgs e)
  {
    if (this.RangeStart == this.Minimum)
      return;
    this.IncreaseDecreaseOperation(-this.LargeChange, -this.LargeChange);
  }

  private void OnLargeIncreaseClick(object sender, RoutedEventArgs e)
  {
    if (this.RangeEnd == this.Maximum)
      return;
    this.IncreaseDecreaseOperation(this.LargeChange, this.LargeChange);
  }

  private void CalculateSize()
  {
    this.middleThumbSize = this.actualTrackSize * ((this.RangeEnd - this.RangeStart) / (this.Maximum - this.Minimum)) - (this.ScrollButtonVisibility != Visibility.Collapsed ? 4.0 : 0.0);
    this.largeDecreaseThumbSize = this.actualTrackSize * ((this.RangeStart - this.Minimum) / (this.Maximum - this.Minimum));
    this.largeIncreaseThumbSize = this.actualTrackSize * ((this.Maximum - this.RangeEnd) / (this.Maximum - this.Minimum));
  }

  private void ThumbMinMax()
  {
    this.middleThumbSize = this.middleThumbSize < 0.0 ? 0.0 : (this.middleThumbSize > this.actualLargeThumbSize ? this.actualLargeThumbSize : this.middleThumbSize);
    this.largeDecreaseThumbSize = this.largeDecreaseThumbSize <= 0.0 ? 0.0 : (this.largeDecreaseThumbSize > this.actualLargeThumbSize ? this.actualLargeThumbSize : this.largeDecreaseThumbSize);
    this.largeIncreaseThumbSize = this.largeIncreaseThumbSize <= 0.0 ? 0.0 : (this.largeIncreaseThumbSize > this.actualLargeThumbSize ? this.actualLargeThumbSize : this.largeIncreaseThumbSize);
  }

  private void CalculateChange(double startChange, double endChange)
  {
    this.RangeStart += startChange;
    this.RangeEnd += endChange;
    this.RangeMinMax();
  }

  private void RangeMinMax()
  {
    this.RangeStart = this.RangeStart < this.Minimum ? this.Minimum : (this.RangeStart <= this.Maximum - this.Maximum / 100.0 || this.ScrollButtonVisibility == Visibility.Collapsed ? (!this.EnableTouchMode || this.RangeStart <= this.Maximum - 1.5 * this.SmallChange ? this.RangeStart : this.Maximum - 1.5 * this.SmallChange) : this.Maximum - this.Maximum / 100.0);
    this.RangeEnd = this.RangeEnd > this.Maximum ? this.Maximum : (this.RangeEnd >= this.Minimum || this.ScrollButtonVisibility != Visibility.Collapsed ? (this.RangeEnd >= this.Maximum / 100.0 || this.ScrollButtonVisibility != Visibility.Visible ? this.RangeEnd : this.Maximum / 100.0) : this.Minimum);
  }

  private void SetLargeThumbSize()
  {
    if (this.Orientation == Orientation.Horizontal)
    {
      this.MiddleThumb.Width = this.middleThumbSize;
      this.LargeDecrease.Width = this.largeDecreaseThumbSize;
      this.LargeIncrease.Width = this.largeIncreaseThumbSize;
    }
    else
    {
      this.MiddleThumb.Height = this.middleThumbSize;
      this.LargeDecrease.Height = this.largeDecreaseThumbSize;
      this.LargeIncrease.Height = this.largeIncreaseThumbSize;
    }
  }

  private void CalculateRangeDifference()
  {
    if (this.previousThumbSize == (this.Orientation == Orientation.Horizontal ? this.MiddleThumb.Width : this.MiddleThumb.Height))
      return;
    this.previousThumbSize = this.Orientation == Orientation.Horizontal ? this.MiddleThumb.Width : this.MiddleThumb.Height;
    this.rangeDiff = this.RangeEnd - this.RangeStart;
  }

  private void IncreaseDecreaseOperation(double startChange, double endChange)
  {
    this.canDrag = true;
    this.IsValueChangedTrigger = false;
    this.CalculateRangeDifference();
    this.CalculateChange(startChange, endChange);
    this.RangeStart = this.RangeStart > this.Maximum - this.rangeDiff ? this.Maximum - this.rangeDiff : this.RangeStart;
    this.RangeEnd = this.RangeEnd < this.rangeDiff ? this.rangeDiff : this.RangeEnd;
    this.CalculateOperations();
    this.OnValueChanged();
  }

  private void CalculateOperations()
  {
    this.CalculateSize();
    this.ThumbMinMax();
    this.SetLargeThumbSize();
  }
}
