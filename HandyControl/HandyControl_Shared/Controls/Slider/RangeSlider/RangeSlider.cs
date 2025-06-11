// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RangeSlider
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[DefaultEvent("ValueChanged")]
[DefaultProperty("Value")]
[TemplatePart(Name = "PART_Track", Type = typeof (Track))]
public class RangeSlider : TwoWayRangeBase
{
  private const string ElementTrack = "PART_Track";
  private RangeTrack _track;
  private readonly System.Windows.Controls.ToolTip _autoToolTipStart;
  private readonly System.Windows.Controls.ToolTip _autoToolTipEnd;
  private RangeThumb _thumbCurrent;
  private object _thumbOriginalToolTip;
  private Point _originThumbPoint;
  private Point _previousScreenCoordPosition;
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (RangeSlider), new PropertyMetadata((object) Orientation.Horizontal));
  public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register(nameof (IsDirectionReversed), typeof (bool), typeof (RangeSlider), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) RangeSlider.GetKeyboardDelay()));
  public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) RangeSlider.GetKeyboardSpeed()));
  public static readonly DependencyProperty AutoToolTipPlacementProperty = DependencyProperty.Register(nameof (AutoToolTipPlacement), typeof (AutoToolTipPlacement), typeof (RangeSlider), new PropertyMetadata((object) AutoToolTipPlacement.None));
  public static readonly DependencyProperty AutoToolTipPrecisionProperty = DependencyProperty.Register(nameof (AutoToolTipPrecision), typeof (int), typeof (RangeSlider), new PropertyMetadata(ValueBoxes.Int0Box), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosIntIncludeZero));
  public static readonly DependencyProperty IsSnapToTickEnabledProperty = DependencyProperty.Register(nameof (IsSnapToTickEnabled), typeof (bool), typeof (RangeSlider), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty TickPlacementProperty = DependencyProperty.Register(nameof (TickPlacement), typeof (TickPlacement), typeof (RangeSlider), new PropertyMetadata((object) TickPlacement.None));
  public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register(nameof (TickFrequency), typeof (double), typeof (RangeSlider), new PropertyMetadata(ValueBoxes.Double1Box), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));
  public static readonly DependencyProperty TicksProperty = DependencyProperty.Register(nameof (Ticks), typeof (DoubleCollection), typeof (RangeSlider), new PropertyMetadata((object) new DoubleCollection()));
  public static readonly DependencyProperty IsMoveToPointEnabledProperty = DependencyProperty.Register(nameof (IsMoveToPointEnabled), typeof (bool), typeof (RangeSlider), new PropertyMetadata(ValueBoxes.FalseBox));

  static RangeSlider()
  {
    RangeSlider.InitializeCommands();
    TwoWayRangeBase.MinimumProperty.OverrideMetadata(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));
    TwoWayRangeBase.MaximumProperty.OverrideMetadata(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double10Box, FrameworkPropertyMetadataOptions.AffectsMeasure));
    TwoWayRangeBase.ValueStartProperty.OverrideMetadata(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));
    TwoWayRangeBase.ValueEndProperty.OverrideMetadata(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));
    EventManager.RegisterClassHandler(typeof (RangeSlider), Thumb.DragStartedEvent, (Delegate) new DragStartedEventHandler(RangeSlider.OnThumbDragStarted));
    EventManager.RegisterClassHandler(typeof (RangeSlider), Thumb.DragDeltaEvent, (Delegate) new DragDeltaEventHandler(RangeSlider.OnThumbDragDelta));
    EventManager.RegisterClassHandler(typeof (RangeSlider), Thumb.DragCompletedEvent, (Delegate) new DragCompletedEventHandler(RangeSlider.OnThumbDragCompleted));
    EventManager.RegisterClassHandler(typeof (RangeSlider), Mouse.MouseDownEvent, (Delegate) new MouseButtonEventHandler(RangeSlider.OnMouseLeftButtonDown), true);
  }

  public RangeSlider()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.IncreaseLarge, new ExecutedRoutedEventHandler(this.OnIncreaseLarge)));
    this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.IncreaseSmall, new ExecutedRoutedEventHandler(this.OnIncreaseSmall)));
    this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.DecreaseLarge, new ExecutedRoutedEventHandler(this.OnDecreaseLarge)));
    this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.DecreaseSmall, new ExecutedRoutedEventHandler(this.OnDecreaseSmall)));
    this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.CenterLarge, new ExecutedRoutedEventHandler(this.OnCenterLarge)));
    this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.CenterSmall, new ExecutedRoutedEventHandler(this.OnCenterSmall)));
  }

  private void OnIncreaseLarge(object sender, ExecutedRoutedEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnIncreaseLarge();
  }

  private void OnIncreaseSmall(object sender, ExecutedRoutedEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnIncreaseSmall();
  }

  private void OnDecreaseLarge(object sender, ExecutedRoutedEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnDecreaseLarge();
  }

  private void OnDecreaseSmall(object sender, ExecutedRoutedEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnDecreaseSmall();
  }

  private void OnCenterLarge(object sender, ExecutedRoutedEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnCenterLarge(e.Parameter);
  }

  private void OnCenterSmall(object sender, ExecutedRoutedEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnCenterSmall(e.Parameter);
  }

  protected virtual void OnIncreaseLarge() => this.MoveToNextTick(this.LargeChange, false);

  protected virtual void OnIncreaseSmall() => this.MoveToNextTick(this.SmallChange, false);

  protected virtual void OnDecreaseLarge() => this.MoveToNextTick(-this.LargeChange, true);

  protected virtual void OnDecreaseSmall() => this.MoveToNextTick(-this.SmallChange, true);

  protected virtual void OnCenterLarge(object parameter)
  {
    this.MoveToNextTick(this.LargeChange, false, true);
  }

  protected virtual void OnCenterSmall(object parameter)
  {
    this.MoveToNextTick(this.SmallChange, false, true);
  }

  public static RoutedCommand IncreaseLarge { get; private set; }

  public static RoutedCommand IncreaseSmall { get; private set; }

  public static RoutedCommand DecreaseLarge { get; private set; }

  public static RoutedCommand DecreaseSmall { get; private set; }

  public static RoutedCommand CenterLarge { get; private set; }

  public static RoutedCommand CenterSmall { get; private set; }

  private static void InitializeCommands()
  {
    RangeSlider.IncreaseLarge = new RoutedCommand("IncreaseLarge", typeof (RangeSlider));
    RangeSlider.IncreaseSmall = new RoutedCommand("IncreaseSmall", typeof (RangeSlider));
    RangeSlider.DecreaseLarge = new RoutedCommand("DecreaseLarge", typeof (RangeSlider));
    RangeSlider.DecreaseSmall = new RoutedCommand("DecreaseSmall", typeof (RangeSlider));
    RangeSlider.CenterLarge = new RoutedCommand("CenterLarge", typeof (RangeSlider));
    RangeSlider.CenterSmall = new RoutedCommand("CenterSmall", typeof (RangeSlider));
  }

  public override void OnApplyTemplate()
  {
    this._thumbCurrent = (RangeThumb) null;
    base.OnApplyTemplate();
    this._track = this.GetTemplateChild("PART_Track") as RangeTrack;
    if (this._autoToolTipStart != null)
      this._autoToolTipStart.PlacementTarget = (UIElement) this._track?.ThumbStart;
    if (this._autoToolTipEnd == null)
      return;
    this._autoToolTipEnd.PlacementTarget = (UIElement) this._track?.ThumbEnd;
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(RangeSlider.OrientationProperty);
    set => this.SetValue(RangeSlider.OrientationProperty, (object) value);
  }

  public bool IsDirectionReversed
  {
    get => (bool) this.GetValue(RangeSlider.IsDirectionReversedProperty);
    set => this.SetValue(RangeSlider.IsDirectionReversedProperty, ValueBoxes.BooleanBox(value));
  }

  public int Delay
  {
    get => (int) this.GetValue(RangeSlider.DelayProperty);
    set => this.SetValue(RangeSlider.DelayProperty, (object) value);
  }

  internal static int GetKeyboardDelay()
  {
    int num = SystemParameters.KeyboardDelay;
    if (num < 0 || num > 3)
      num = 0;
    return (num + 1) * 250;
  }

  public int Interval
  {
    get => (int) this.GetValue(RangeSlider.IntervalProperty);
    set => this.SetValue(RangeSlider.IntervalProperty, (object) value);
  }

  internal static int GetKeyboardSpeed()
  {
    int num = SystemParameters.KeyboardSpeed;
    if (num < 0 || num > 31 /*0x1F*/)
      num = 31 /*0x1F*/;
    return (31 /*0x1F*/ - num) * 367 / 31 /*0x1F*/ + 33;
  }

  public AutoToolTipPlacement AutoToolTipPlacement
  {
    get => (AutoToolTipPlacement) this.GetValue(RangeSlider.AutoToolTipPlacementProperty);
    set => this.SetValue(RangeSlider.AutoToolTipPlacementProperty, (object) value);
  }

  public int AutoToolTipPrecision
  {
    get => (int) this.GetValue(RangeSlider.AutoToolTipPrecisionProperty);
    set => this.SetValue(RangeSlider.AutoToolTipPrecisionProperty, (object) value);
  }

  public bool IsSnapToTickEnabled
  {
    get => (bool) this.GetValue(RangeSlider.IsSnapToTickEnabledProperty);
    set => this.SetValue(RangeSlider.IsSnapToTickEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public TickPlacement TickPlacement
  {
    get => (TickPlacement) this.GetValue(RangeSlider.TickPlacementProperty);
    set => this.SetValue(RangeSlider.TickPlacementProperty, (object) value);
  }

  public double TickFrequency
  {
    get => (double) this.GetValue(RangeSlider.TickFrequencyProperty);
    set => this.SetValue(RangeSlider.TickFrequencyProperty, (object) value);
  }

  public DoubleCollection Ticks
  {
    get => (DoubleCollection) this.GetValue(RangeSlider.TicksProperty);
    set => this.SetValue(RangeSlider.TicksProperty, (object) value);
  }

  public bool IsMoveToPointEnabled
  {
    get => (bool) this.GetValue(RangeSlider.IsMoveToPointEnabledProperty);
    set => this.SetValue(RangeSlider.IsMoveToPointEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.IsMoveToPointEnabled)
    {
      RangeThumb thumbStart = this._track.ThumbStart;
      if (thumbStart != null && !thumbStart.IsMouseOver)
      {
        RangeThumb thumbEnd = this._track.ThumbEnd;
        if (thumbEnd != null && !thumbEnd.IsMouseOver)
        {
          this.UpdateValue(e.MouseDevice.GetPosition((IInputElement) this._track));
          e.Handled = true;
        }
      }
    }
    base.OnPreviewMouseLeftButtonDown(e);
  }

  private void MoveToNextTick(double direction, bool isStart, bool isCenter = false, object parameter = null)
  {
    if (MathHelper.AreClose(direction, 0.0))
      return;
    if (isCenter)
    {
      if (parameter == null)
      {
        double num = this._track.ValueFromPoint(Mouse.GetPosition((IInputElement) this._track));
        if (ValidateHelper.IsInRangeOfDouble((object) num))
        {
          isStart = (this.ValueStart + this.ValueEnd) / 2.0 > num;
          if (!isStart)
            direction = -Math.Abs(direction);
        }
      }
      if (parameter is bool flag)
        isStart = flag;
    }
    double num1 = isStart ? this.ValueStart : this.ValueEnd;
    double num2 = this.SnapToTick(Math.Max(this.Minimum, Math.Min(this.Maximum, num1 + direction)));
    bool flag1 = direction > 0.0;
    if (MathHelper.AreClose(num2, num1) && (!flag1 || !MathHelper.AreClose(num1, this.Maximum)) && (flag1 || !MathHelper.AreClose(num1, this.Minimum)))
    {
      DoubleCollection ticks = this.Ticks;
      if (ticks != null && ticks.Count > 0)
      {
        foreach (double tick in this.Ticks)
        {
          if (flag1 && MathHelper.GreaterThan(tick, num1) && (MathHelper.LessThan(tick, num2) || MathHelper.AreClose(num2, num1)) || !flag1 && MathHelper.LessThan(tick, num1) && (MathHelper.GreaterThan(tick, num2) || MathHelper.AreClose(num2, num1)))
            num2 = tick;
        }
      }
      else if (MathHelper.GreaterThan(this.TickFrequency, 0.0))
      {
        double num3 = Math.Round((num1 - this.Minimum) / this.TickFrequency);
        num2 = this.Minimum + (!flag1 ? num3 - 1.0 : num3 + 1.0) * this.TickFrequency;
      }
    }
    if (MathHelper.AreClose(num2, num1))
      return;
    this.SetCurrentValue(isStart ? TwoWayRangeBase.ValueStartProperty : TwoWayRangeBase.ValueEndProperty, (object) num2);
  }

  private double SnapToTick(double value)
  {
    if (!this.IsSnapToTickEnabled)
      return value;
    double num1 = this.Minimum;
    double num2 = this.Maximum;
    DoubleCollection ticks = this.Ticks;
    if (ticks != null && ticks.Count > 0)
    {
      foreach (double tick in this.Ticks)
      {
        if (MathHelper.AreClose(tick, value))
          return value;
        if (MathHelper.LessThan(tick, value) && MathHelper.GreaterThan(tick, num1))
          num1 = tick;
        else if (MathHelper.GreaterThan(tick, value) && MathHelper.LessThan(tick, num2))
          num2 = tick;
      }
    }
    else if (MathHelper.GreaterThan(this.TickFrequency, 0.0))
    {
      num1 = this.Minimum + Math.Round((value - this.Minimum) / this.TickFrequency) * this.TickFrequency;
      num2 = Math.Min(this.Maximum, num1 + this.TickFrequency);
    }
    return !MathHelper.GreaterThanOrClose(value, (num1 + num2) * 0.5) ? num1 : num2;
  }

  private void UpdateValue(Point point)
  {
    double num = this._track.ValueFromPoint(point);
    if (!ValidateHelper.IsInRangeOfDouble((object) num))
      return;
    bool isStart = (this.ValueStart + this.ValueEnd) / 2.0 > num;
    this.UpdateValue(num, isStart);
  }

  private void UpdateValue(double value, bool isStart)
  {
    double tick = this.SnapToTick(value);
    if (isStart)
    {
      if (MathHelper.AreClose(tick, this.ValueStart))
        return;
      double num = Math.Max(this.Minimum, Math.Min(this.Maximum, tick));
      if (num > this.ValueEnd)
      {
        this.SetCurrentValue(TwoWayRangeBase.ValueStartProperty, (object) this.ValueEnd);
        this.SetCurrentValue(TwoWayRangeBase.ValueEndProperty, (object) num);
        this._track.ThumbStart.CancelDrag();
        this._track.ThumbEnd.StartDrag();
        this._thumbCurrent = this._track.ThumbEnd;
      }
      else
        this.SetCurrentValue(TwoWayRangeBase.ValueStartProperty, (object) num);
    }
    else
    {
      if (MathHelper.AreClose(tick, this.ValueEnd))
        return;
      double num = Math.Max(this.Minimum, Math.Min(this.Maximum, tick));
      if (num < this.ValueStart)
      {
        this.SetCurrentValue(TwoWayRangeBase.ValueEndProperty, (object) this.ValueStart);
        this.SetCurrentValue(TwoWayRangeBase.ValueStartProperty, (object) num);
        this._track.ThumbEnd.CancelDrag();
        this._track.ThumbStart.StartDrag();
        this._thumbCurrent = this._track.ThumbStart;
      }
      else
        this.SetCurrentValue(TwoWayRangeBase.ValueEndProperty, (object) num);
    }
  }

  private static void OnThumbDragStarted(object sender, DragStartedEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnThumbDragStarted(e);
  }

  protected virtual void OnThumbDragStarted(DragStartedEventArgs e)
  {
    if (!(e.OriginalSource is RangeThumb originalSource))
      return;
    this._thumbCurrent = originalSource;
    this._originThumbPoint = Mouse.GetPosition((IInputElement) this._thumbCurrent);
    this._thumbCurrent.StartDrag();
    if (this.AutoToolTipPlacement == AutoToolTipPlacement.None)
      return;
    bool isStart = originalSource.Equals((object) this._track.ThumbStart);
    if (!isStart && !originalSource.Equals((object) this._track.ThumbEnd))
      return;
    this._thumbOriginalToolTip = originalSource.ToolTip;
    this.OnThumbDragStarted(isStart ? this._autoToolTipStart : this._autoToolTipEnd, isStart);
  }

  private void OnThumbDragStarted(System.Windows.Controls.ToolTip toolTip, bool isStart)
  {
    if (toolTip == null)
      toolTip = new System.Windows.Controls.ToolTip()
      {
        Placement = PlacementMode.Custom,
        PlacementTarget = isStart ? (UIElement) this._track.ThumbStart : (UIElement) this._track.ThumbEnd,
        CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.AutoToolTipCustomPlacementCallback)
      };
    if (isStart)
      this._track.ThumbStart.ToolTip = (object) toolTip;
    else
      this._track.ThumbEnd.ToolTip = (object) toolTip;
    toolTip.Content = (object) this.GetAutoToolTipNumber(isStart);
    toolTip.IsOpen = true;
  }

  private CustomPopupPlacement[] AutoToolTipCustomPlacementCallback(
    Size popupSize,
    Size targetSize,
    Point offset)
  {
    switch (this.AutoToolTipPlacement)
    {
      case AutoToolTipPlacement.TopLeft:
        return this.Orientation == Orientation.Horizontal ? new CustomPopupPlacement[1]
        {
          new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height), PopupPrimaryAxis.Horizontal)
        } : new CustomPopupPlacement[1]
        {
          new CustomPopupPlacement(new Point(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
        };
      case AutoToolTipPlacement.BottomRight:
        return this.Orientation == Orientation.Horizontal ? new CustomPopupPlacement[1]
        {
          new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height), PopupPrimaryAxis.Horizontal)
        } : new CustomPopupPlacement[1]
        {
          new CustomPopupPlacement(new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
        };
      default:
        return new CustomPopupPlacement[0];
    }
  }

  private string GetAutoToolTipNumber(bool isStart)
  {
    NumberFormatInfo provider = (NumberFormatInfo) NumberFormatInfo.CurrentInfo.Clone();
    provider.NumberDecimalDigits = this.AutoToolTipPrecision;
    return !isStart ? this.ValueEnd.ToString("N", (IFormatProvider) provider) : this.ValueStart.ToString("N", (IFormatProvider) provider);
  }

  private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnThumbDragDelta(e);
  }

  protected virtual void OnThumbDragDelta(DragDeltaEventArgs e)
  {
    if (!(e.OriginalSource is Thumb originalSource))
      return;
    bool isStart = originalSource.Equals((object) this._track.ThumbStart);
    if (!isStart && !originalSource.Equals((object) this._track.ThumbEnd))
      return;
    this.OnThumbDragDelta(this._track, isStart, e);
  }

  private void OnThumbDragDelta(RangeTrack track, bool isStart, DragDeltaEventArgs e)
  {
    if (track == null || track.ThumbStart == null | this._track.ThumbEnd == null)
      return;
    double num = (isStart ? this.ValueStart : this.ValueEnd) + track.ValueFromDistance(e.HorizontalChange, e.VerticalChange);
    if (ValidateHelper.IsInRangeOfDouble((object) num))
      this.UpdateValue(num, isStart);
    if (this.AutoToolTipPlacement == AutoToolTipPlacement.None)
      return;
    System.Windows.Controls.ToolTip objB = (isStart ? this._autoToolTipStart : this._autoToolTipEnd) ?? new System.Windows.Controls.ToolTip();
    objB.Content = (object) this.GetAutoToolTipNumber(isStart);
    RangeThumb rangeThumb = isStart ? this._track.ThumbStart : this._track.ThumbEnd;
    if (!object.Equals(rangeThumb.ToolTip, (object) objB))
      rangeThumb.ToolTip = (object) objB;
    if (objB.IsOpen)
      return;
    objB.IsOpen = true;
  }

  private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
  {
    if (!(sender is RangeSlider rangeSlider))
      return;
    rangeSlider.OnThumbDragCompleted(e);
  }

  protected virtual void OnThumbDragCompleted(DragCompletedEventArgs e)
  {
    if (!(e.OriginalSource is Thumb originalSource) || this.AutoToolTipPlacement == AutoToolTipPlacement.None)
      return;
    bool flag = originalSource.Equals((object) this._track.ThumbStart);
    if (!flag && !originalSource.Equals((object) this._track.ThumbEnd))
      return;
    System.Windows.Controls.ToolTip toolTip = flag ? this._autoToolTipStart : this._autoToolTipEnd;
    if (toolTip != null)
      toolTip.IsOpen = false;
    originalSource.ToolTip = this._thumbOriginalToolTip;
  }

  private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Left)
      return;
    RangeSlider rangeSlider = (RangeSlider) sender;
    if (!rangeSlider.IsKeyboardFocusWithin)
      e.Handled = rangeSlider.Focus() || e.Handled;
    if (rangeSlider._track.ThumbStart.IsMouseOver)
    {
      rangeSlider._track.ThumbStart.StartDrag();
      rangeSlider._thumbCurrent = rangeSlider._track.ThumbStart;
    }
    if (!rangeSlider._track.ThumbEnd.IsMouseOver)
      return;
    rangeSlider._track.ThumbEnd.StartDrag();
    rangeSlider._thumbCurrent = rangeSlider._track.ThumbEnd;
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (this._thumbCurrent == null || e.MouseDevice.LeftButton != MouseButtonState.Pressed || !this._thumbCurrent.IsDragging)
      return;
    Point position = e.GetPosition((IInputElement) this._thumbCurrent);
    Point point = this.PointFromScreen(position);
    if (!(point != this._previousScreenCoordPosition))
      return;
    this._previousScreenCoordPosition = point;
    this._thumbCurrent.RaiseEvent((RoutedEventArgs) new DragDeltaEventArgs(position.X - this._originThumbPoint.X, position.Y - this._originThumbPoint.Y));
  }

  protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonUp(e);
    this._thumbCurrent = (RangeThumb) null;
  }
}
