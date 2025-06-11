// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.AnimationPath
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Controls;

public class AnimationPath : Shape
{
  private Storyboard _storyboard;
  private double _pathLength;
  public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof (Data), typeof (Geometry), typeof (AnimationPath), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(AnimationPath.OnPropertiesChanged)));
  public static readonly DependencyProperty PathLengthProperty = DependencyProperty.Register(nameof (PathLength), typeof (double), typeof (AnimationPath), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(AnimationPath.OnPropertiesChanged)));
  public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), typeof (Duration), typeof (AnimationPath), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Duration(TimeSpan.FromSeconds(2.0)), new PropertyChangedCallback(AnimationPath.OnPropertiesChanged)));
  public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(nameof (IsPlaying), typeof (bool), typeof (AnimationPath), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, (PropertyChangedCallback) ((o, args) =>
  {
    AnimationPath animationPath = (AnimationPath) o;
    if ((bool) args.NewValue)
      animationPath.UpdatePath();
    else
      animationPath._storyboard?.Pause();
  })));
  public static readonly DependencyProperty RepeatBehaviorProperty = Timeline.RepeatBehaviorProperty.AddOwner(typeof (AnimationPath), new PropertyMetadata((object) RepeatBehavior.Forever));
  public static readonly DependencyProperty FillBehaviorProperty = Timeline.FillBehaviorProperty.AddOwner(typeof (AnimationPath), new PropertyMetadata((object) FillBehavior.Stop));
  public static readonly RoutedEvent CompletedEvent = EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble, typeof (EventHandler), typeof (AnimationPath));

  private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is AnimationPath animationPath))
      return;
    animationPath.UpdatePath();
  }

  public Geometry Data
  {
    get => (Geometry) this.GetValue(AnimationPath.DataProperty);
    set => this.SetValue(AnimationPath.DataProperty, (object) value);
  }

  protected override Geometry DefiningGeometry => this.Data ?? Geometry.Empty;

  public double PathLength
  {
    get => (double) this.GetValue(AnimationPath.PathLengthProperty);
    set => this.SetValue(AnimationPath.PathLengthProperty, (object) value);
  }

  public Duration Duration
  {
    get => (Duration) this.GetValue(AnimationPath.DurationProperty);
    set => this.SetValue(AnimationPath.DurationProperty, (object) value);
  }

  public bool IsPlaying
  {
    get => (bool) this.GetValue(AnimationPath.IsPlayingProperty);
    set => this.SetValue(AnimationPath.IsPlayingProperty, ValueBoxes.BooleanBox(value));
  }

  public RepeatBehavior RepeatBehavior
  {
    get => (RepeatBehavior) this.GetValue(AnimationPath.RepeatBehaviorProperty);
    set => this.SetValue(AnimationPath.RepeatBehaviorProperty, (object) value);
  }

  public FillBehavior FillBehavior
  {
    get => (FillBehavior) this.GetValue(AnimationPath.FillBehaviorProperty);
    set => this.SetValue(AnimationPath.FillBehaviorProperty, (object) value);
  }

  static AnimationPath()
  {
    Shape.StretchProperty.AddOwner(typeof (AnimationPath), (PropertyMetadata) new FrameworkPropertyMetadata((object) Stretch.Uniform, new PropertyChangedCallback(AnimationPath.OnPropertiesChanged)));
    Shape.StrokeThicknessProperty.AddOwner(typeof (AnimationPath), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double1Box, new PropertyChangedCallback(AnimationPath.OnPropertiesChanged)));
  }

  public AnimationPath() => this.Loaded += (RoutedEventHandler) ((s, e) => this.UpdatePath());

  public event EventHandler Completed
  {
    add => this.AddHandler(AnimationPath.CompletedEvent, (Delegate) value);
    remove => this.RemoveHandler(AnimationPath.CompletedEvent, (Delegate) value);
  }

  private void UpdatePath()
  {
    if (!this.Duration.HasTimeSpan || !this.IsPlaying)
      return;
    this._pathLength = this.PathLength > 0.0 ? this.PathLength : this.Data.GetTotalLength(new Size(this.ActualWidth, this.ActualHeight), this.StrokeThickness);
    if (MathHelper.IsVerySmall(this._pathLength))
      return;
    this.StrokeDashOffset = this._pathLength;
    this.StrokeDashArray = new DoubleCollection((IEnumerable<double>) new List<double>()
    {
      this._pathLength,
      this._pathLength
    });
    if (this._storyboard != null)
    {
      this._storyboard.Stop();
      this._storyboard.Completed -= new EventHandler(this.Storyboard_Completed);
    }
    Storyboard storyboard = new Storyboard();
    storyboard.RepeatBehavior = this.RepeatBehavior;
    storyboard.FillBehavior = this.FillBehavior;
    this._storyboard = storyboard;
    this._storyboard.Completed += new EventHandler(this.Storyboard_Completed);
    DoubleAnimationUsingKeyFrames element = new DoubleAnimationUsingKeyFrames();
    LinearDoubleKeyFrame linearDoubleKeyFrame1 = new LinearDoubleKeyFrame();
    linearDoubleKeyFrame1.Value = this._pathLength;
    linearDoubleKeyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
    LinearDoubleKeyFrame keyFrame1 = linearDoubleKeyFrame1;
    element.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
    LinearDoubleKeyFrame linearDoubleKeyFrame2 = new LinearDoubleKeyFrame();
    linearDoubleKeyFrame2.Value = this.FillBehavior == FillBehavior.Stop ? -this._pathLength : 0.0;
    linearDoubleKeyFrame2.KeyTime = KeyTime.FromTimeSpan(this.Duration.TimeSpan);
    LinearDoubleKeyFrame keyFrame2 = linearDoubleKeyFrame2;
    element.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
    Storyboard.SetTarget((DependencyObject) element, (DependencyObject) this);
    Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) Shape.StrokeDashOffsetProperty));
    this._storyboard.Children.Add((Timeline) element);
    this._storyboard.Begin();
  }

  private void Storyboard_Completed(object sender, EventArgs e)
  {
    this.RaiseEvent(new RoutedEventArgs(AnimationPath.CompletedEvent));
  }
}
