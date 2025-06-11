// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.RotateTransition
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Controls;

[EditorBrowsable(EditorBrowsableState.Never)]
public class RotateTransition : ContentTransition
{
  public static readonly DependencyProperty EasingProperty;
  public static readonly DependencyProperty DurationProperty;

  public TimeSpan Duration
  {
    get => (TimeSpan) this.GetValue(RotateTransition.DurationProperty);
    set => this.SetValue(RotateTransition.DurationProperty, (object) value);
  }

  public EasingFunctionBase Easing
  {
    get => (EasingFunctionBase) this.GetValue(RotateTransition.EasingProperty);
    set => this.SetValue(RotateTransition.EasingProperty, (object) value);
  }

  internal DoubleAnimationUsingKeyFrames BuildTimeLine(double from, double to)
  {
    EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame1.Value = from;
    easingDoubleKeyFrame1.KeyTime = (KeyTime) TimeSpan.FromSeconds(0.0);
    EasingDoubleKeyFrame easingDoubleKeyFrame2 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame2.Value = to;
    easingDoubleKeyFrame2.KeyTime = (KeyTime) this.Duration;
    easingDoubleKeyFrame2.EasingFunction = (IEasingFunction) this.Easing;
    return new DoubleAnimationUsingKeyFrames()
    {
      KeyFrames = {
        (DoubleKeyFrame) easingDoubleKeyFrame1,
        (DoubleKeyFrame) easingDoubleKeyFrame2
      }
    };
  }

  internal ObjectAnimationUsingKeyFrames BuildObjectTimeline(object visibility)
  {
    DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame();
    discreteObjectKeyFrame.KeyTime = (KeyTime) TimeSpan.FromSeconds(this.Duration.TotalSeconds / 2.0);
    discreteObjectKeyFrame.Value = visibility;
    return new ObjectAnimationUsingKeyFrames()
    {
      KeyFrames = {
        (ObjectKeyFrame) discreteObjectKeyFrame
      }
    };
  }

  static RotateTransition()
  {
    Type propertyType = typeof (EasingFunctionBase);
    Type ownerType = typeof (RotateTransition);
    ElasticEase defaultValue = new ElasticEase();
    defaultValue.EasingMode = EasingMode.EaseInOut;
    defaultValue.Oscillations = 0;
    defaultValue.Springiness = 1.0;
    PropertyMetadata typeMetadata = new PropertyMetadata((object) defaultValue);
    RotateTransition.EasingProperty = DependencyProperty.Register(nameof (Easing), propertyType, ownerType, typeMetadata);
    RotateTransition.DurationProperty = DependencyProperty.Register(nameof (Duration), typeof (TimeSpan), typeof (RotateTransition), new PropertyMetadata((object) TimeSpan.FromSeconds(0.6)));
  }
}
