// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.SlideTransition
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Primitives;
using System;
using System.Windows;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class SlideTransition : ContentTransition
{
  public Position Position;
  public static readonly DependencyProperty EasingProperty = DependencyProperty.Register(nameof (Easing), typeof (EasingFunctionBase), typeof (SlideTransition), new PropertyMetadata((object) new PowerEase()
  {
    Power = 13.0
  }, new PropertyChangedCallback(SlideTransition.OnDirectionChanged)));
  public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof (Direction), typeof (SlideDirection), typeof (SlideTransition), new PropertyMetadata((object) SlideDirection.Default));
  public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), typeof (TimeSpan), typeof (SlideTransition), new PropertyMetadata((object) TimeSpan.FromSeconds(3.0)));

  public TimeSpan Duration
  {
    get => (TimeSpan) this.GetValue(SlideTransition.DurationProperty);
    set => this.SetValue(SlideTransition.DurationProperty, (object) value);
  }

  public EasingFunctionBase Easing
  {
    get => (EasingFunctionBase) this.GetValue(SlideTransition.EasingProperty);
    set => this.SetValue(SlideTransition.EasingProperty, (object) value);
  }

  public SlideDirection Direction
  {
    get => (SlideDirection) this.GetValue(SlideTransition.DirectionProperty);
    set => this.SetValue(SlideTransition.DirectionProperty, (object) value);
  }

  public Timeline CreateExitAnimation(double toValue)
  {
    DoubleAnimationUsingKeyFrames exitAnimation = new DoubleAnimationUsingKeyFrames();
    EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame1.Value = 0.0;
    easingDoubleKeyFrame1.KeyTime = (KeyTime) TimeSpan.FromSeconds(0.0);
    EasingDoubleKeyFrame keyFrame1 = easingDoubleKeyFrame1;
    if (this.Direction == SlideDirection.Up || this.Direction == SlideDirection.Default || this.Direction == SlideDirection.Left)
      toValue = -toValue;
    EasingDoubleKeyFrame easingDoubleKeyFrame2 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame2.KeyTime = (KeyTime) this.Duration;
    easingDoubleKeyFrame2.Value = toValue;
    EasingDoubleKeyFrame keyFrame2 = easingDoubleKeyFrame2;
    keyFrame2.EasingFunction = (IEasingFunction) this.Easing;
    exitAnimation.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
    exitAnimation.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
    return (Timeline) exitAnimation;
  }

  public Timeline CreateEnterAnimation(double fromValue)
  {
    DoubleAnimationUsingKeyFrames enterAnimation = new DoubleAnimationUsingKeyFrames();
    if (this.Direction == SlideDirection.Down || this.Direction == SlideDirection.Right)
      fromValue = -fromValue;
    EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame1.Value = fromValue;
    easingDoubleKeyFrame1.KeyTime = (KeyTime) TimeSpan.FromSeconds(0.0);
    EasingDoubleKeyFrame keyFrame1 = easingDoubleKeyFrame1;
    EasingDoubleKeyFrame easingDoubleKeyFrame2 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame2.KeyTime = (KeyTime) this.Duration;
    easingDoubleKeyFrame2.Value = 0.0;
    EasingDoubleKeyFrame keyFrame2 = easingDoubleKeyFrame2;
    keyFrame2.EasingFunction = (IEasingFunction) this.Easing;
    enterAnimation.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
    enterAnimation.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
    return (Timeline) enterAnimation;
  }

  private static void OnDirectionChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    SlideTransition slideTransition = (SlideTransition) sender;
    if (slideTransition.Direction == SlideDirection.Down)
      slideTransition.Position = Position.Top;
    else if (slideTransition.Direction == SlideDirection.Up)
      slideTransition.Position = Position.Bottom;
    else if (slideTransition.Direction == SlideDirection.Left)
      slideTransition.Position = Position.Left;
    else if (slideTransition.Direction == SlideDirection.Right)
      slideTransition.Position = Position.Right;
    else
      slideTransition.Position = Position.Bottom;
  }
}
