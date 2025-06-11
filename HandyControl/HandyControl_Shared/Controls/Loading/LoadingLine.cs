// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.LoadingLine
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Controls;

public class LoadingLine : LoadingBase
{
  private const double MoveLength = 80.0;
  private const double UniformScale = 0.6;

  public LoadingLine()
  {
    this.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding("DotDiameter")
    {
      Source = (object) this
    });
  }

  protected sealed override void UpdateDots()
  {
    int dotCount = this.DotCount;
    double dotInterval = this.DotInterval;
    double dotDiameter = this.DotDiameter;
    double dotSpeed = this.DotSpeed;
    double dotDelayTime = this.DotDelayTime;
    if (dotCount < 1)
      return;
    this.PrivateCanvas.Children.Clear();
    double num1 = dotDiameter * (double) dotCount + dotInterval * (double) (dotCount - 1) + 80.0;
    double num2 = (this.ActualWidth - 80.0) / 2.0;
    double num3 = num1 / 2.0;
    Storyboard storyboard = new Storyboard();
    storyboard.RepeatBehavior = RepeatBehavior.Forever;
    this.Storyboard = storyboard;
    for (int index = 0; index < dotCount; ++index)
    {
      Ellipse ellipse = this.CreateEllipse(index, dotInterval, dotDiameter);
      ThicknessAnimationUsingKeyFrames animationUsingKeyFrames = new ThicknessAnimationUsingKeyFrames();
      animationUsingKeyFrames.BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(dotDelayTime * (double) index));
      ThicknessAnimationUsingKeyFrames element = animationUsingKeyFrames;
      LinearThicknessKeyFrame thicknessKeyFrame1 = new LinearThicknessKeyFrame();
      Thickness margin = ellipse.Margin;
      thicknessKeyFrame1.Value = new Thickness(margin.Left, 0.0, 0.0, 0.0);
      thicknessKeyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
      LinearThicknessKeyFrame keyFrame1 = thicknessKeyFrame1;
      EasingThicknessKeyFrame thicknessKeyFrame2 = new EasingThicknessKeyFrame();
      PowerEase powerEase1 = new PowerEase();
      powerEase1.EasingMode = EasingMode.EaseOut;
      thicknessKeyFrame2.EasingFunction = (IEasingFunction) powerEase1;
      double num4 = num2;
      margin = ellipse.Margin;
      double left1 = margin.Left;
      thicknessKeyFrame2.Value = new Thickness(num4 + left1, 0.0, 0.0, 0.0);
      thicknessKeyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * 0.4 / 2.0));
      EasingThicknessKeyFrame keyFrame2 = thicknessKeyFrame2;
      LinearThicknessKeyFrame thicknessKeyFrame3 = new LinearThicknessKeyFrame();
      double num5 = num2 + num3;
      margin = ellipse.Margin;
      double left2 = margin.Left;
      thicknessKeyFrame3.Value = new Thickness(num5 + left2, 0.0, 0.0, 0.0);
      thicknessKeyFrame3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed * 1.6 / 2.0));
      LinearThicknessKeyFrame keyFrame3 = thicknessKeyFrame3;
      EasingThicknessKeyFrame thicknessKeyFrame4 = new EasingThicknessKeyFrame();
      PowerEase powerEase2 = new PowerEase();
      powerEase2.EasingMode = EasingMode.EaseIn;
      thicknessKeyFrame4.EasingFunction = (IEasingFunction) powerEase2;
      double actualWidth = this.ActualWidth;
      margin = ellipse.Margin;
      double left3 = margin.Left;
      thicknessKeyFrame4.Value = new Thickness(actualWidth + left3 + num3, 0.0, 0.0, 0.0);
      thicknessKeyFrame4.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(dotSpeed));
      EasingThicknessKeyFrame keyFrame4 = thicknessKeyFrame4;
      element.KeyFrames.Add((ThicknessKeyFrame) keyFrame1);
      element.KeyFrames.Add((ThicknessKeyFrame) keyFrame2);
      element.KeyFrames.Add((ThicknessKeyFrame) keyFrame3);
      element.KeyFrames.Add((ThicknessKeyFrame) keyFrame4);
      Storyboard.SetTarget((DependencyObject) element, (DependencyObject) ellipse);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) FrameworkElement.MarginProperty));
      this.Storyboard.Children.Add((Timeline) element);
      this.PrivateCanvas.Children.Add((UIElement) ellipse);
    }
    this.Storyboard.Begin();
    if (this.IsRunning)
      return;
    this.Storyboard.Pause();
  }

  private Ellipse CreateEllipse(int index, double dotInterval, double dotDiameter)
  {
    Ellipse ellipse = this.CreateEllipse(index);
    ellipse.HorizontalAlignment = HorizontalAlignment.Left;
    ellipse.VerticalAlignment = VerticalAlignment.Top;
    ellipse.Margin = new Thickness(-(dotInterval + dotDiameter) * (double) index, 0.0, 0.0, 0.0);
    return ellipse;
  }
}
