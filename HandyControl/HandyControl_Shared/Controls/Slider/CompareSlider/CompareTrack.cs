// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CompareTrack
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

public class CompareTrack : Track
{
  protected override Size ArrangeOverride(Size arrangeSize)
  {
    base.ArrangeOverride(arrangeSize);
    bool isVertical = this.Orientation == Orientation.Vertical;
    double decreaseButtonLength;
    double thumbLength;
    double increaseButtonLength;
    this.ComputeSliderLengths(arrangeSize, isVertical, out decreaseButtonLength, out thumbLength, out increaseButtonLength);
    Point location = new Point();
    Size size = arrangeSize;
    bool directionReversed = this.IsDirectionReversed;
    if (isVertical)
    {
      CompareTrack.CoerceLength(ref decreaseButtonLength, arrangeSize.Height);
      CompareTrack.CoerceLength(ref increaseButtonLength, arrangeSize.Height);
      CompareTrack.CoerceLength(ref thumbLength, arrangeSize.Height);
      location.Y = directionReversed ? decreaseButtonLength + thumbLength : 0.0;
      size.Height = increaseButtonLength;
      this.IncreaseRepeatButton?.Arrange(new Rect(location, size));
      location.Y = directionReversed ? 0.0 : increaseButtonLength + thumbLength;
      size.Height = decreaseButtonLength;
      if (this.DecreaseRepeatButton != null)
      {
        this.DecreaseRepeatButton.Arrange(new Rect(location, size));
        this.DecreaseRepeatButton.Height = size.Height;
      }
      location.Y = directionReversed ? decreaseButtonLength : increaseButtonLength;
      size.Height = thumbLength;
      this.Thumb?.Arrange(new Rect(location, size));
    }
    else
    {
      CompareTrack.CoerceLength(ref decreaseButtonLength, arrangeSize.Width);
      CompareTrack.CoerceLength(ref increaseButtonLength, arrangeSize.Width);
      CompareTrack.CoerceLength(ref thumbLength, arrangeSize.Width);
      location.X = directionReversed ? increaseButtonLength + thumbLength : 0.0;
      size.Width = decreaseButtonLength;
      this.DecreaseRepeatButton?.Arrange(new Rect(location, size));
      location.X = directionReversed ? 0.0 : decreaseButtonLength + thumbLength;
      size.Width = increaseButtonLength;
      if (this.IncreaseRepeatButton != null)
      {
        this.IncreaseRepeatButton.Arrange(new Rect(location, size));
        this.IncreaseRepeatButton.Width = size.Width;
      }
      location.X = directionReversed ? increaseButtonLength : decreaseButtonLength;
      size.Width = thumbLength;
      this.Thumb?.Arrange(new Rect(location, size));
    }
    return arrangeSize;
  }

  private void ComputeSliderLengths(
    Size arrangeSize,
    bool isVertical,
    out double decreaseButtonLength,
    out double thumbLength,
    out double increaseButtonLength)
  {
    double minimum = this.Minimum;
    double val1 = Math.Max(0.0, this.Maximum - minimum);
    double num1 = Math.Min(val1, this.Value - minimum);
    double trackLength1;
    if (isVertical)
    {
      trackLength1 = arrangeSize.Height;
      ref double local = ref thumbLength;
      Thumb thumb = this.Thumb;
      double num2 = thumb != null ? thumb.DesiredSize.Height : 0.0;
      local = num2;
    }
    else
    {
      trackLength1 = arrangeSize.Width;
      ref double local = ref thumbLength;
      Thumb thumb = this.Thumb;
      double num3 = thumb != null ? thumb.DesiredSize.Width : 0.0;
      local = num3;
    }
    CompareTrack.CoerceLength(ref thumbLength, trackLength1);
    double trackLength2 = trackLength1 - thumbLength;
    decreaseButtonLength = trackLength2 * num1 / val1;
    CompareTrack.CoerceLength(ref decreaseButtonLength, trackLength2);
    increaseButtonLength = trackLength2 - decreaseButtonLength;
    CompareTrack.CoerceLength(ref increaseButtonLength, trackLength2);
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
}
