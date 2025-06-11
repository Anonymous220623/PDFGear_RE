// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewItemAnimationBase
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class TileViewItemAnimationBase : HeaderedContentControl
{
  private bool animatingSize;
  private bool animatingPosition;
  private TimeSpan animationTimeSpanSize = new TimeSpan(0, 0, 0, 0, 700);
  private TimeSpan animationTimespanPosition = new TimeSpan(0, 0, 0, 0, 700);
  internal SplineDoubleKeyFrame animationWidthKeyFrameSize;
  internal SplineDoubleKeyFrame animationHeightKeyFrameSize;
  internal SplineDoubleKeyFrame animationXKeyFramePosition;
  internal SplineDoubleKeyFrame animationYKeyFramePosition;
  internal Storyboard animationSize;
  internal Storyboard animationPosition;
  public TileViewControl ParentTileViewControl;
  internal static readonly DependencyProperty SizeAnimationDurationProperty = DependencyProperty.Register(nameof (SizeAnimationDuration), typeof (TimeSpan), typeof (TileViewItemAnimationBase), new PropertyMetadata(new PropertyChangedCallback(TileViewItemAnimationBase.SetAnimationSizeDuration)));
  internal static readonly DependencyProperty PositionAnimationDurationProperty = DependencyProperty.Register(nameof (PositionAnimationDuration), typeof (TimeSpan), typeof (TileViewItemAnimationBase), new PropertyMetadata(new PropertyChangedCallback(TileViewItemAnimationBase.SetPositionAnimationDuration)));

  internal TileViewItemAnimationBase()
  {
  }

  internal void AnimationChanged()
  {
    this.animationSize = new Storyboard();
    DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
    element1.FillBehavior = FillBehavior.Stop;
    Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) this);
    Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(FrameworkElement.Width)", new object[0]));
    this.animationWidthKeyFrameSize = new SplineDoubleKeyFrame();
    this.animationWidthKeyFrameSize.KeySpline = new KeySpline()
    {
      ControlPoint1 = new Point(0.528, 0.0),
      ControlPoint2 = new Point(0.142, 0.847)
    };
    if (this.ParentTileViewControl != null)
    {
      if (this.ParentTileViewControl.EnableAnimation)
        this.animationWidthKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds((double) this.ParentTileViewControl.AnimationDuration.Milliseconds));
      else
        this.animationWidthKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1.0));
    }
    this.animationWidthKeyFrameSize.Value = 0.0;
    element1.KeyFrames.Add((DoubleKeyFrame) this.animationWidthKeyFrameSize);
    DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
    element2.FillBehavior = FillBehavior.Stop;
    Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) this);
    Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(FrameworkElement.Height)", new object[0]));
    this.animationHeightKeyFrameSize = new SplineDoubleKeyFrame();
    this.animationHeightKeyFrameSize.KeySpline = new KeySpline()
    {
      ControlPoint1 = new Point(0.528, 0.0),
      ControlPoint2 = new Point(0.142, 0.847)
    };
    if (this.ParentTileViewControl != null)
    {
      if (this.ParentTileViewControl.EnableAnimation)
        this.animationHeightKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds((double) this.ParentTileViewControl.AnimationDuration.Milliseconds));
      else
        this.animationHeightKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1.0));
    }
    this.animationHeightKeyFrameSize.Value = 0.0;
    element2.KeyFrames.Add((DoubleKeyFrame) this.animationHeightKeyFrameSize);
    this.animationSize.Children.Add((Timeline) element1);
    this.animationSize.Children.Add((Timeline) element2);
    this.animationSize.Completed += new EventHandler(this.AnimationSize_Completed);
    this.animationPosition = new Storyboard();
    DoubleAnimationUsingKeyFrames element3 = new DoubleAnimationUsingKeyFrames();
    element3.FillBehavior = FillBehavior.Stop;
    Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) this);
    Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath("(Canvas.Left)", new object[0]));
    this.animationXKeyFramePosition = new SplineDoubleKeyFrame();
    this.animationXKeyFramePosition.KeySpline = new KeySpline()
    {
      ControlPoint1 = new Point(0.528, 0.0),
      ControlPoint2 = new Point(0.142, 0.847)
    };
    if (this.ParentTileViewControl != null)
    {
      if (this.ParentTileViewControl.EnableAnimation)
        this.animationXKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds((double) this.ParentTileViewControl.AnimationDuration.Milliseconds));
      else
        this.animationXKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1.0));
    }
    this.animationXKeyFramePosition.Value = 0.0;
    element3.KeyFrames.Add((DoubleKeyFrame) this.animationXKeyFramePosition);
    DoubleAnimationUsingKeyFrames element4 = new DoubleAnimationUsingKeyFrames();
    element4.FillBehavior = FillBehavior.Stop;
    Storyboard.SetTarget((DependencyObject) element4, (DependencyObject) this);
    Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath("(Canvas.Top)", new object[0]));
    this.animationYKeyFramePosition = new SplineDoubleKeyFrame();
    this.animationYKeyFramePosition.KeySpline = new KeySpline()
    {
      ControlPoint1 = new Point(0.528, 0.0),
      ControlPoint2 = new Point(0.142, 0.847)
    };
    if (this.ParentTileViewControl != null)
    {
      if (this.ParentTileViewControl.EnableAnimation)
        this.animationYKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds((double) this.ParentTileViewControl.AnimationDuration.Milliseconds));
      else
        this.animationYKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1.0));
    }
    this.animationYKeyFramePosition.Value = 0.0;
    element4.KeyFrames.Add((DoubleKeyFrame) this.animationYKeyFramePosition);
    this.animationPosition.Children.Add((Timeline) element3);
    this.animationPosition.Children.Add((Timeline) element4);
    this.animationPosition.Completed += new EventHandler(this.AnimationPosition_Completed);
  }

  internal TimeSpan PositionAnimationDuration
  {
    get => (TimeSpan) this.GetValue(TileViewItemAnimationBase.PositionAnimationDurationProperty);
    set
    {
      this.SetValue(TileViewItemAnimationBase.PositionAnimationDurationProperty, (object) value);
    }
  }

  internal TimeSpan SizeAnimationDuration
  {
    get => (TimeSpan) this.GetValue(TileViewItemAnimationBase.SizeAnimationDurationProperty);
    set => this.SetValue(TileViewItemAnimationBase.SizeAnimationDurationProperty, (object) value);
  }

  private static void SetAnimationSizeDuration(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    ((TileViewItemAnimationBase) obj).SetAnimationSizeDuration(e);
  }

  protected virtual void SetAnimationSizeDuration(DependencyPropertyChangedEventArgs e)
  {
    if (this.animationHeightKeyFrameSize != null)
      this.animationHeightKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(this.animationTimeSpanSize);
    if (this.animationWidthKeyFrameSize == null)
      return;
    this.animationWidthKeyFrameSize.KeyTime = KeyTime.FromTimeSpan(this.animationTimeSpanSize);
  }

  private static void SetPositionAnimationDuration(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    ((TileViewItemAnimationBase) obj).SetAnimationSizeDuration(e);
  }

  protected virtual void SetPositionAnimationDuration(DependencyPropertyChangedEventArgs e)
  {
    if (this.animationXKeyFramePosition != null)
      this.animationXKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(this.animationTimespanPosition);
    if (this.animationYKeyFramePosition == null)
      return;
    this.animationYKeyFramePosition.KeyTime = KeyTime.FromTimeSpan(this.animationTimespanPosition);
  }

  private void AnimationSize_Completed(object sender, EventArgs e)
  {
    if (this.animationWidthKeyFrameSize.Value > 0.0)
      this.Width = this.animationWidthKeyFrameSize.Value;
    else
      this.Width = 0.0;
    if (this.animationHeightKeyFrameSize.Value > 0.0)
      this.Height = this.animationHeightKeyFrameSize.Value;
    else
      this.Height = 0.0;
  }

  private void AnimationPosition_Completed(object sender, EventArgs e)
  {
    Canvas.SetLeft((UIElement) this, this.animationXKeyFramePosition.Value);
    Canvas.SetTop((UIElement) this, this.animationYKeyFramePosition.Value);
  }

  public void AnimateSize(double width, double height)
  {
    if (this.animatingSize)
      this.animationSize.Pause();
    if (VisualTreeHelper.GetParent((DependencyObject) this) == null)
      return;
    this.Width = this.ActualWidth;
    this.Height = this.ActualHeight;
    this.animatingSize = true;
    this.animationWidthKeyFrameSize.Value = width;
    this.animationHeightKeyFrameSize.Value = height;
    this.animationSize.Begin();
  }

  internal virtual void AnimatePosition(double x, double y)
  {
    if (this.animatingPosition)
      this.animationPosition.Pause();
    if (VisualTreeHelper.GetParent((DependencyObject) this) == null)
      return;
    this.animatingPosition = true;
    this.animationXKeyFramePosition.Value = x;
    this.animationYKeyFramePosition.Value = y;
    this.animationPosition.Begin();
  }
}
