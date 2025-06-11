// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.ProgressRing
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#nullable disable
namespace CommomLib.Controls;

public sealed partial class ProgressRing : Control
{
  private bool oldIsActive;
  private bool oldIsIndeterminate;
  private Storyboard activeSb;
  public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof (IsActive), typeof (bool), typeof (ProgressRing), new PropertyMetadata((object) true, new PropertyChangedCallback(ProgressRing.OnIsActivePropertyChanged)));
  public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof (IsIndeterminate), typeof (bool), typeof (ProgressRing), new PropertyMetadata((object) true, new PropertyChangedCallback(ProgressRing.OnIsIndeterminatePropertyChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 100.0, new PropertyChangedCallback(ProgressRing.OnMaximumPropertyChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ProgressRing.OnMinimumPropertyChanged)));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ProgressRing.OnValuePropertyChanged)));
  public static readonly DependencyProperty TemplateSettingsProperty = DependencyProperty.Register(nameof (TemplateSettings), typeof (ProgressRingTemplateSettings), typeof (ProgressRingTemplateSettings), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));

  static ProgressRing()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ProgressRing), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ProgressRing)));
  }

  public ProgressRing()
  {
    this.TemplateSettings = new ProgressRingTemplateSettings();
    this.Unloaded += new RoutedEventHandler(this.ProgressRing_Unloaded);
    this.Loaded += new RoutedEventHandler(this.ProgressRing_Loaded);
  }

  private void ProgressRing_Loaded(object sender, RoutedEventArgs e)
  {
    this.UpdateIsActiveState(true);
  }

  private void ProgressRing_Unloaded(object sender, RoutedEventArgs e)
  {
    this.activeSb?.Stop();
    this.activeSb = (Storyboard) null;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.UpdateIsActiveState(true);
  }

  private void UpdateIsActiveState(bool force = false)
  {
    if (((this.oldIsActive != this.IsActive ? 1 : (this.oldIsIndeterminate != this.IsIndeterminate ? 1 : 0)) | (force ? 1 : 0)) == 0)
      return;
    this.oldIsActive = this.IsActive;
    this.oldIsIndeterminate = this.IsIndeterminate;
    this.TemplateSettings.IsIndeterminate = this.IsIndeterminate;
    this.activeSb?.Stop();
    this.activeSb = (Storyboard) null;
    if (!this.IsActive)
      VisualStateManager.GoToState((FrameworkElement) this, "Inactive", true);
    else if (this.IsIndeterminate)
      VisualStateManager.GoToState((FrameworkElement) this, "Active", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "DeterminateActive", true);
    if (!this.IsActive)
      return;
    if (this.IsIndeterminate)
    {
      this.TemplateSettings.RadiusRatio = 0.0;
      if (!this.IsLoaded)
        return;
      this.activeSb = new Storyboard();
      DoubleAnimationUsingKeyFrames element = new DoubleAnimationUsingKeyFrames();
      Storyboard.SetTarget((DependencyObject) element, (DependencyObject) this.TemplateSettings);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) ProgressRingTemplateSettings.RadiusRatioProperty));
      element.Duration = new Duration(TimeSpan.FromSeconds(2.0));
      DoubleKeyFrameCollection keyFrames1 = element.KeyFrames;
      DiscreteDoubleKeyFrame keyFrame1 = new DiscreteDoubleKeyFrame();
      keyFrame1.KeyTime = (KeyTime) TimeSpan.Zero;
      keyFrame1.Value = 0.0;
      keyFrames1.Add((DoubleKeyFrame) keyFrame1);
      DoubleKeyFrameCollection keyFrames2 = element.KeyFrames;
      SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
      keyFrame2.KeyTime = (KeyTime) TimeSpan.FromSeconds(1.0);
      keyFrame2.KeySpline = new KeySpline(0.166999996, 0.166999996, 0.833000004, 0.833000004);
      keyFrame2.Value = 0.5;
      keyFrames2.Add((DoubleKeyFrame) keyFrame2);
      DoubleKeyFrameCollection keyFrames3 = element.KeyFrames;
      SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
      keyFrame3.KeyTime = (KeyTime) TimeSpan.FromSeconds(2.0);
      keyFrame3.KeySpline = new KeySpline(0.166999996, 0.166999996, 0.833000004, 0.833000004);
      keyFrame3.Value = 1.0;
      keyFrames3.Add((DoubleKeyFrame) keyFrame3);
      element.RepeatBehavior = RepeatBehavior.Forever;
      this.activeSb.Children.Add((Timeline) element);
      this.activeSb.Freeze();
      this.activeSb.Begin();
    }
    else
      this.UpdateDeterminateValue();
  }

  private void UpdateDeterminateValue()
  {
    if (!this.IsActive || this.IsIndeterminate)
      return;
    this.TemplateSettings.RadiusRatio = this.Value / (this.Maximum - this.Minimum);
  }

  public bool IsActive
  {
    get => (bool) this.GetValue(ProgressRing.IsActiveProperty);
    set => this.SetValue(ProgressRing.IsActiveProperty, (object) value);
  }

  private static void OnIsActivePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ProgressRing progressRing))
      return;
    progressRing.UpdateIsActiveState();
  }

  public bool IsIndeterminate
  {
    get => (bool) this.GetValue(ProgressRing.IsIndeterminateProperty);
    set => this.SetValue(ProgressRing.IsIndeterminateProperty, (object) value);
  }

  private static void OnIsIndeterminatePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ProgressRing progressRing))
      return;
    progressRing.UpdateIsActiveState();
  }

  public double Maximum
  {
    get => (double) this.GetValue(ProgressRing.MaximumProperty);
    set => this.SetValue(ProgressRing.MaximumProperty, (object) value);
  }

  private static void OnMaximumPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ProgressRing progressRing))
      return;
    progressRing.UpdateDeterminateValue();
  }

  public double Minimum
  {
    get => (double) this.GetValue(ProgressRing.MinimumProperty);
    set => this.SetValue(ProgressRing.MinimumProperty, (object) value);
  }

  private static void OnMinimumPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ProgressRing progressRing))
      return;
    progressRing.UpdateDeterminateValue();
  }

  public double Value
  {
    get => (double) this.GetValue(ProgressRing.ValueProperty);
    set => this.SetValue(ProgressRing.ValueProperty, (object) value);
  }

  private static void OnValuePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ProgressRing progressRing))
      return;
    progressRing.UpdateDeterminateValue();
  }

  public ProgressRingTemplateSettings TemplateSettings
  {
    get => (ProgressRingTemplateSettings) this.GetValue(ProgressRing.TemplateSettingsProperty);
    set => this.SetValue(ProgressRing.TemplateSettingsProperty, (object) value);
  }
}
