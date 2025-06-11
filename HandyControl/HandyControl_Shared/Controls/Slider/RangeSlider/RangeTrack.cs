// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RangeTrack
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class RangeTrack : FrameworkElement
{
  private RepeatButton _increaseButton;
  private RepeatButton _centerButton;
  private RepeatButton _decreaseButton;
  private RangeThumb _thumbStart;
  private RangeThumb _thumbEnd;
  private Visual[] _visualChildren;
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (RangeTrack), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (RangeTrack), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsArrange));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (RangeTrack), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsArrange));
  public static readonly DependencyProperty ValueStartProperty = DependencyProperty.Register(nameof (ValueStart), typeof (double), typeof (RangeTrack), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
  public static readonly DependencyProperty ValueEndProperty = DependencyProperty.Register(nameof (ValueEnd), typeof (double), typeof (RangeTrack), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
  public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register(nameof (IsDirectionReversed), typeof (bool), typeof (RangeTrack), new PropertyMetadata(ValueBoxes.FalseBox));

  private double Density { get; set; } = double.NaN;

  public RepeatButton DecreaseRepeatButton
  {
    get => this._decreaseButton;
    set
    {
      if (object.Equals((object) this._increaseButton, (object) value) || object.Equals((object) this._centerButton, (object) value))
        throw new NotSupportedException("SameButtons");
      this.UpdateComponent((Control) this._decreaseButton, (Control) value);
      this._decreaseButton = value;
      if (this._decreaseButton == null)
        return;
      CommandManager.InvalidateRequerySuggested();
    }
  }

  public RepeatButton CenterRepeatButton
  {
    get => this._centerButton;
    set
    {
      if (object.Equals((object) this._increaseButton, (object) value) || object.Equals((object) this._decreaseButton, (object) value))
        throw new NotSupportedException("SameButtons");
      this.UpdateComponent((Control) this._centerButton, (Control) value);
      this._centerButton = value;
      if (this._centerButton == null)
        return;
      CommandManager.InvalidateRequerySuggested();
    }
  }

  public RepeatButton IncreaseRepeatButton
  {
    get => this._increaseButton;
    set
    {
      if (object.Equals((object) this._decreaseButton, (object) value) || object.Equals((object) this._centerButton, (object) value))
        throw new NotSupportedException("SameButtons");
      this.UpdateComponent((Control) this._increaseButton, (Control) value);
      this._increaseButton = value;
      if (this._increaseButton == null)
        return;
      CommandManager.InvalidateRequerySuggested();
    }
  }

  public RangeThumb ThumbStart
  {
    get => this._thumbStart;
    set
    {
      this.UpdateComponent((Control) this._thumbStart, (Control) value);
      this._thumbStart = value;
    }
  }

  public RangeThumb ThumbEnd
  {
    get => this._thumbEnd;
    set
    {
      this.UpdateComponent((Control) this._thumbEnd, (Control) value);
      this._thumbEnd = value;
    }
  }

  static RangeTrack()
  {
    UIElement.IsEnabledProperty.OverrideMetadata(typeof (RangeTrack), (PropertyMetadata) new UIPropertyMetadata(new PropertyChangedCallback(RangeTrack.OnIsEnabledChanged)));
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(RangeTrack.OrientationProperty);
    set => this.SetValue(RangeTrack.OrientationProperty, (object) value);
  }

  public double Minimum
  {
    get => (double) this.GetValue(RangeTrack.MinimumProperty);
    set => this.SetValue(RangeTrack.MinimumProperty, (object) value);
  }

  public double Maximum
  {
    get => (double) this.GetValue(RangeTrack.MaximumProperty);
    set => this.SetValue(RangeTrack.MaximumProperty, (object) value);
  }

  public double ValueStart
  {
    get => (double) this.GetValue(RangeTrack.ValueStartProperty);
    set => this.SetValue(RangeTrack.ValueStartProperty, (object) value);
  }

  public double ValueEnd
  {
    get => (double) this.GetValue(RangeTrack.ValueEndProperty);
    set => this.SetValue(RangeTrack.ValueEndProperty, (object) value);
  }

  public bool IsDirectionReversed
  {
    get => (bool) this.GetValue(RangeTrack.IsDirectionReversedProperty);
    set => this.SetValue(RangeTrack.IsDirectionReversedProperty, ValueBoxes.BooleanBox(value));
  }

  protected override Visual GetVisualChild(int index)
  {
    return this._visualChildren?[index] != null ? this._visualChildren[index] : throw new ArgumentOutOfRangeException(nameof (index), (object) index, "ArgumentOutOfRange");
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    Size size = new Size();
    if (this._thumbStart != null)
    {
      this._thumbStart.Measure(availableSize);
      size = this._thumbStart.DesiredSize;
    }
    if (this._thumbEnd != null)
    {
      this._thumbEnd.Measure(availableSize);
      size = new Size(Math.Max(this._thumbEnd.DesiredSize.Width, size.Width), Math.Max(this._thumbEnd.DesiredSize.Height, size.Height));
    }
    return size;
  }

  private static void CoerceLength(ref double componentLength, double trackLength)
  {
    if (componentLength < 0.0)
    {
      componentLength = 0.0;
    }
    else
    {
      if (componentLength <= trackLength && !double.IsNaN(componentLength))
        return;
      componentLength = trackLength;
    }
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    bool isVertical = this.Orientation == Orientation.Vertical;
    double decreaseButtonLength;
    double centerButtonLength;
    double increaseButtonLength;
    double thumbStartLength;
    double thumbEndLength;
    this.ComputeLengths(arrangeSize, isVertical, out decreaseButtonLength, out centerButtonLength, out increaseButtonLength, out thumbStartLength, out thumbEndLength);
    Point point = new Point();
    Size size = arrangeSize;
    bool directionReversed = this.IsDirectionReversed;
    if (isVertical)
    {
      RangeTrack.CoerceLength(ref decreaseButtonLength, arrangeSize.Height);
      RangeTrack.CoerceLength(ref centerButtonLength, arrangeSize.Height);
      RangeTrack.CoerceLength(ref increaseButtonLength, arrangeSize.Height);
      RangeTrack.CoerceLength(ref thumbStartLength, arrangeSize.Height);
      RangeTrack.CoerceLength(ref thumbEndLength, arrangeSize.Height);
      point.Y = directionReversed ? decreaseButtonLength + thumbEndLength + centerButtonLength + thumbStartLength : 0.0;
      size.Height = increaseButtonLength;
      this.IncreaseRepeatButton?.Arrange(new Rect(point, size));
      point.Y = directionReversed ? decreaseButtonLength + thumbEndLength : increaseButtonLength + thumbStartLength;
      size.Height = centerButtonLength;
      this.CenterRepeatButton?.Arrange(new Rect(point, size));
      point.Y = directionReversed ? 0.0 : increaseButtonLength + thumbStartLength + centerButtonLength + thumbEndLength;
      size.Height = decreaseButtonLength;
      this.DecreaseRepeatButton?.Arrange(new Rect(point, size));
      point.Y = directionReversed ? decreaseButtonLength + thumbEndLength + centerButtonLength : increaseButtonLength + thumbStartLength + centerButtonLength;
      size.Height = thumbStartLength;
      this.ArrangeThumb(directionReversed, false, point, size);
      point.Y = directionReversed ? decreaseButtonLength : increaseButtonLength;
      size.Height = thumbEndLength;
      this.ArrangeThumb(directionReversed, true, point, size);
    }
    else
    {
      RangeTrack.CoerceLength(ref decreaseButtonLength, arrangeSize.Width);
      RangeTrack.CoerceLength(ref centerButtonLength, arrangeSize.Width);
      RangeTrack.CoerceLength(ref increaseButtonLength, arrangeSize.Width);
      RangeTrack.CoerceLength(ref thumbStartLength, arrangeSize.Width);
      RangeTrack.CoerceLength(ref thumbEndLength, arrangeSize.Width);
      point.X = directionReversed ? 0.0 : decreaseButtonLength + thumbEndLength + centerButtonLength + thumbStartLength;
      size.Width = increaseButtonLength;
      this.IncreaseRepeatButton?.Arrange(new Rect(point, size));
      point.X = directionReversed ? increaseButtonLength + thumbStartLength : decreaseButtonLength + thumbEndLength;
      size.Width = centerButtonLength;
      this.CenterRepeatButton?.Arrange(new Rect(point, size));
      point.X = directionReversed ? increaseButtonLength + thumbStartLength + centerButtonLength + thumbEndLength : 0.0;
      size.Width = decreaseButtonLength;
      this.DecreaseRepeatButton?.Arrange(new Rect(point, size));
      point.X = directionReversed ? increaseButtonLength : decreaseButtonLength;
      size.Width = thumbStartLength;
      this.ArrangeThumb(directionReversed, false, point, size);
      point.X = directionReversed ? increaseButtonLength + thumbStartLength + centerButtonLength : decreaseButtonLength + thumbEndLength + centerButtonLength;
      size.Width = thumbEndLength;
      this.ArrangeThumb(directionReversed, true, point, size);
    }
    return arrangeSize;
  }

  private void ArrangeThumb(bool isDirectionReversed, bool isStart, Point offset, Size pieceSize)
  {
    if (isStart)
    {
      if (isDirectionReversed)
        this.ThumbStart?.Arrange(new Rect(offset, pieceSize));
      else
        this.ThumbEnd?.Arrange(new Rect(offset, pieceSize));
    }
    else if (isDirectionReversed)
      this.ThumbEnd?.Arrange(new Rect(offset, pieceSize));
    else
      this.ThumbStart?.Arrange(new Rect(offset, pieceSize));
  }

  private void ComputeLengths(
    Size arrangeSize,
    bool isVertical,
    out double decreaseButtonLength,
    out double centerButtonLength,
    out double increaseButtonLength,
    out double thumbStartLength,
    out double thumbEndLength)
  {
    double minimum = this.Minimum;
    double val1 = Math.Max(0.0, this.Maximum - minimum);
    double num1 = Math.Min(val1, this.ValueStart - minimum);
    double num2 = Math.Min(val1, this.ValueEnd - minimum);
    double trackLength1;
    if (isVertical)
    {
      trackLength1 = arrangeSize.Height;
      ref double local1 = ref thumbStartLength;
      RangeThumb thumbStart = this._thumbStart;
      Size desiredSize;
      double num3;
      if (thumbStart == null)
      {
        num3 = 0.0;
      }
      else
      {
        desiredSize = thumbStart.DesiredSize;
        num3 = desiredSize.Height;
      }
      local1 = num3;
      ref double local2 = ref thumbEndLength;
      RangeThumb thumbEnd = this._thumbEnd;
      double num4;
      if (thumbEnd == null)
      {
        num4 = 0.0;
      }
      else
      {
        desiredSize = thumbEnd.DesiredSize;
        num4 = desiredSize.Height;
      }
      local2 = num4;
    }
    else
    {
      trackLength1 = arrangeSize.Width;
      ref double local3 = ref thumbStartLength;
      RangeThumb thumbStart = this._thumbStart;
      Size desiredSize;
      double num5;
      if (thumbStart == null)
      {
        num5 = 0.0;
      }
      else
      {
        desiredSize = thumbStart.DesiredSize;
        num5 = desiredSize.Width;
      }
      local3 = num5;
      ref double local4 = ref thumbEndLength;
      RangeThumb thumbEnd = this._thumbEnd;
      double num6;
      if (thumbEnd == null)
      {
        num6 = 0.0;
      }
      else
      {
        desiredSize = thumbEnd.DesiredSize;
        num6 = desiredSize.Width;
      }
      local4 = num6;
    }
    RangeTrack.CoerceLength(ref thumbStartLength, trackLength1);
    RangeTrack.CoerceLength(ref thumbEndLength, trackLength1);
    double trackLength2 = trackLength1 - thumbStartLength - thumbEndLength;
    decreaseButtonLength = trackLength2 * num1 / val1;
    RangeTrack.CoerceLength(ref decreaseButtonLength, trackLength2);
    centerButtonLength = trackLength2 * num2 / val1 - decreaseButtonLength;
    RangeTrack.CoerceLength(ref centerButtonLength, trackLength2);
    increaseButtonLength = trackLength2 - decreaseButtonLength - centerButtonLength;
    RangeTrack.CoerceLength(ref increaseButtonLength, trackLength2);
    this.Density = val1 / trackLength2;
  }

  protected override int VisualChildrenCount
  {
    get
    {
      if (this._visualChildren == null)
        return 0;
      for (int visualChildrenCount = 0; visualChildrenCount < this._visualChildren.Length; ++visualChildrenCount)
      {
        if (this._visualChildren[visualChildrenCount] == null)
          return visualChildrenCount;
      }
      return this._visualChildren.Length;
    }
  }

  private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(bool) e.NewValue)
      return;
    Mouse.Synchronize();
  }

  public virtual double ValueFromPoint(Point pt)
  {
    return this.Orientation != Orientation.Horizontal ? (this.IsDirectionReversed ? (1.0 - pt.X / this.RenderSize.Height) * this.Maximum : pt.Y / this.RenderSize.Height * this.Maximum) : (this.IsDirectionReversed ? (1.0 - pt.X / this.RenderSize.Width) * this.Maximum : pt.X / this.RenderSize.Width * this.Maximum);
  }

  public virtual double ValueFromDistance(double horizontal, double vertical)
  {
    double num = this.IsDirectionReversed ? -1.0 : 1.0;
    return this.Orientation != Orientation.Horizontal ? -1.0 * num * vertical * this.Density : num * horizontal * this.Density;
  }

  private void UpdateComponent(Control oldValue, Control newValue)
  {
    if (oldValue == newValue)
      return;
    if (this._visualChildren == null)
      this._visualChildren = new Visual[5];
    if (oldValue != null)
      this.RemoveVisualChild((Visual) oldValue);
    int index = 0;
label_11:
    while (index < 5 && this._visualChildren[index] != null)
    {
      if (this._visualChildren[index] == oldValue)
      {
        while (true)
        {
          if (index < 4 && this._visualChildren[index + 1] != null)
          {
            this._visualChildren[index] = this._visualChildren[index + 1];
            ++index;
          }
          else
            goto label_11;
        }
      }
      else
        ++index;
    }
    this._visualChildren[index] = (Visual) newValue;
    this.AddVisualChild((Visual) newValue);
    this.InvalidateMeasure();
    this.InvalidateArrange();
  }
}
