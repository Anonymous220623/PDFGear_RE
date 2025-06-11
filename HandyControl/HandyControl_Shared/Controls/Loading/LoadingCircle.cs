// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.LoadingCircle
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Controls;

public class LoadingCircle : LoadingBase
{
  public static readonly DependencyProperty DotOffSetProperty = DependencyProperty.Register(nameof (DotOffSet), typeof (double), typeof (LoadingCircle), (PropertyMetadata) new FrameworkPropertyMetadata((object) 20.0, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty NeedHiddenProperty = DependencyProperty.Register(nameof (NeedHidden), typeof (bool), typeof (LoadingCircle), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender));

  public double DotOffSet
  {
    get => (double) this.GetValue(LoadingCircle.DotOffSetProperty);
    set => this.SetValue(LoadingCircle.DotOffSetProperty, (object) value);
  }

  public bool NeedHidden
  {
    get => (bool) this.GetValue(LoadingCircle.NeedHiddenProperty);
    set => this.SetValue(LoadingCircle.NeedHiddenProperty, ValueBoxes.BooleanBox(value));
  }

  static LoadingCircle()
  {
    LoadingBase.DotSpeedProperty.OverrideMetadata(typeof (LoadingCircle), (PropertyMetadata) new FrameworkPropertyMetadata((object) 6.0, FrameworkPropertyMetadataOptions.AffectsRender));
    LoadingBase.DotDelayTimeProperty.OverrideMetadata(typeof (LoadingCircle), (PropertyMetadata) new FrameworkPropertyMetadata((object) 220.0, FrameworkPropertyMetadataOptions.AffectsRender));
  }

  protected sealed override void UpdateDots()
  {
    int dotCount = this.DotCount;
    double dotInterval = this.DotInterval;
    double dotSpeed = this.DotSpeed;
    double dotDelayTime = this.DotDelayTime;
    bool needHidden = this.NeedHidden;
    if (dotCount < 1)
      return;
    this.PrivateCanvas.Children.Clear();
    Storyboard storyboard = new Storyboard();
    storyboard.RepeatBehavior = RepeatBehavior.Forever;
    this.Storyboard = storyboard;
    for (int index = 0; index < dotCount; ++index)
    {
      Border border = this.CreateBorder(index, dotInterval, needHidden);
      DoubleAnimationUsingKeyFrames animationUsingKeyFrames1 = new DoubleAnimationUsingKeyFrames();
      animationUsingKeyFrames1.BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(dotDelayTime * (double) index));
      DoubleAnimationUsingKeyFrames element1 = animationUsingKeyFrames1;
      double num = -dotInterval * (double) index;
      LinearDoubleKeyFrame linearDoubleKeyFrame1 = new LinearDoubleKeyFrame();
      linearDoubleKeyFrame1.Value = 0.0 + num;
      linearDoubleKeyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
      LinearDoubleKeyFrame keyFrame1 = linearDoubleKeyFrame1;
      EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
      PowerEase powerEase1 = new PowerEase();
      powerEase1.EasingMode = EasingMode.EaseOut;
      easingDoubleKeyFrame1.EasingFunction = (IEasingFunction) powerEase1;
      easingDoubleKeyFrame1.Value = 180.0 + num;
      easingDoubleKeyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (3.0 / 28.0)));
      EasingDoubleKeyFrame keyFrame2 = easingDoubleKeyFrame1;
      LinearDoubleKeyFrame linearDoubleKeyFrame2 = new LinearDoubleKeyFrame();
      linearDoubleKeyFrame2.Value = 180.0 + this.DotOffSet + num;
      linearDoubleKeyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (11.0 / 28.0)));
      LinearDoubleKeyFrame keyFrame3 = linearDoubleKeyFrame2;
      EasingDoubleKeyFrame easingDoubleKeyFrame2 = new EasingDoubleKeyFrame();
      PowerEase powerEase2 = new PowerEase();
      powerEase2.EasingMode = EasingMode.EaseIn;
      easingDoubleKeyFrame2.EasingFunction = (IEasingFunction) powerEase2;
      easingDoubleKeyFrame2.Value = 360.0 + num;
      easingDoubleKeyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * 0.5));
      EasingDoubleKeyFrame keyFrame4 = easingDoubleKeyFrame2;
      EasingDoubleKeyFrame easingDoubleKeyFrame3 = new EasingDoubleKeyFrame();
      PowerEase powerEase3 = new PowerEase();
      powerEase3.EasingMode = EasingMode.EaseOut;
      easingDoubleKeyFrame3.EasingFunction = (IEasingFunction) powerEase3;
      easingDoubleKeyFrame3.Value = 540.0 + num;
      easingDoubleKeyFrame3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (17.0 / 28.0)));
      EasingDoubleKeyFrame keyFrame5 = easingDoubleKeyFrame3;
      LinearDoubleKeyFrame linearDoubleKeyFrame3 = new LinearDoubleKeyFrame();
      linearDoubleKeyFrame3.Value = 540.0 + this.DotOffSet + num;
      linearDoubleKeyFrame3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * (25.0 / 28.0)));
      LinearDoubleKeyFrame keyFrame6 = linearDoubleKeyFrame3;
      EasingDoubleKeyFrame easingDoubleKeyFrame4 = new EasingDoubleKeyFrame();
      PowerEase powerEase4 = new PowerEase();
      powerEase4.EasingMode = EasingMode.EaseIn;
      easingDoubleKeyFrame4.EasingFunction = (IEasingFunction) powerEase4;
      easingDoubleKeyFrame4.Value = 720.0 + num;
      easingDoubleKeyFrame4.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed));
      EasingDoubleKeyFrame keyFrame7 = easingDoubleKeyFrame4;
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame5);
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame6);
      element1.KeyFrames.Add((DoubleKeyFrame) keyFrame7);
      Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) border);
      Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)", Array.Empty<object>()));
      this.Storyboard.Children.Add((Timeline) element1);
      if (this.NeedHidden)
      {
        DiscreteObjectKeyFrame discreteObjectKeyFrame1 = new DiscreteObjectKeyFrame();
        discreteObjectKeyFrame1.Value = (object) Visibility.Collapsed;
        discreteObjectKeyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed));
        DiscreteObjectKeyFrame keyFrame8 = discreteObjectKeyFrame1;
        DiscreteObjectKeyFrame discreteObjectKeyFrame2 = new DiscreteObjectKeyFrame();
        discreteObjectKeyFrame2.Value = (object) Visibility.Collapsed;
        discreteObjectKeyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed + 0.4));
        DiscreteObjectKeyFrame keyFrame9 = discreteObjectKeyFrame2;
        DiscreteObjectKeyFrame discreteObjectKeyFrame3 = new DiscreteObjectKeyFrame();
        discreteObjectKeyFrame3.Value = (object) Visibility.Visible;
        discreteObjectKeyFrame3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
        DiscreteObjectKeyFrame keyFrame10 = discreteObjectKeyFrame3;
        ObjectAnimationUsingKeyFrames animationUsingKeyFrames2 = new ObjectAnimationUsingKeyFrames();
        animationUsingKeyFrames2.BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(dotDelayTime * (double) index));
        ObjectAnimationUsingKeyFrames element2 = animationUsingKeyFrames2;
        element2.KeyFrames.Add((ObjectKeyFrame) keyFrame10);
        element2.KeyFrames.Add((ObjectKeyFrame) keyFrame8);
        element2.KeyFrames.Add((ObjectKeyFrame) keyFrame9);
        Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) border);
        Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(UIElement.Visibility)", Array.Empty<object>()));
        this.Storyboard.Children.Add((Timeline) element2);
      }
      this.PrivateCanvas.Children.Add((UIElement) border);
    }
    this.Storyboard.Begin();
    if (this.IsRunning)
      return;
    this.Storyboard.Pause();
  }

  private Border CreateBorder(int index, double dotInterval, bool needHidden)
  {
    Ellipse ellipse = this.CreateEllipse(index);
    ellipse.HorizontalAlignment = HorizontalAlignment.Center;
    ellipse.VerticalAlignment = VerticalAlignment.Bottom;
    RotateTransform rotateTransform = new RotateTransform()
    {
      Angle = -dotInterval * (double) index
    };
    TransformGroup transformGroup = new TransformGroup();
    transformGroup.Children.Add((Transform) rotateTransform);
    Border border = new Border();
    border.RenderTransformOrigin = new Point(0.5, 0.5);
    border.RenderTransform = (Transform) transformGroup;
    border.Child = (UIElement) ellipse;
    border.Visibility = needHidden ? Visibility.Collapsed : Visibility.Visible;
    border.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding("Width")
    {
      Source = (object) this
    });
    border.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding("Height")
    {
      Source = (object) this
    });
    return border;
  }
}
