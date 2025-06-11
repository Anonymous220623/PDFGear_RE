// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.FadeTransition
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class FadeTransition : ContentTransition
{
  public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), typeof (TimeSpan), typeof (FadeTransition), new PropertyMetadata((object) TimeSpan.FromSeconds(0.5)));
  public static readonly DependencyProperty EasingProperty = DependencyProperty.Register(nameof (Easing), typeof (EasingFunctionBase), typeof (FadeTransition), new PropertyMetadata((object) new BackEase()
  {
    Amplitude = -1.0
  }));

  public TimeSpan Duration
  {
    get => (TimeSpan) this.GetValue(FadeTransition.DurationProperty);
    set => this.SetValue(FadeTransition.DurationProperty, (object) value);
  }

  public EasingFunctionBase Easing
  {
    get => (EasingFunctionBase) this.GetValue(FadeTransition.EasingProperty);
    set => this.SetValue(FadeTransition.EasingProperty, (object) value);
  }

  internal Timeline BuildAnimation(
    double from,
    double to,
    TimeSpan duration,
    EasingFunctionBase easingfunction)
  {
    DoubleAnimationUsingKeyFrames animationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
    EasingDoubleKeyFrame keyFrame1 = new EasingDoubleKeyFrame();
    keyFrame1.Value = from;
    keyFrame1.KeyTime = (KeyTime) TimeSpan.FromSeconds(0.0);
    EasingDoubleKeyFrame keyFrame2 = new EasingDoubleKeyFrame();
    keyFrame2.Value = to;
    keyFrame2.KeyTime = (KeyTime) duration;
    keyFrame2.EasingFunction = (IEasingFunction) easingfunction;
    animationUsingKeyFrames.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
    animationUsingKeyFrames.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
    return (Timeline) animationUsingKeyFrames;
  }
}
