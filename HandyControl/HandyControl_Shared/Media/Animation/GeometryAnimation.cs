// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.GeometryAnimation
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Media.Animation;

public class GeometryAnimation : GeometryAnimationBase
{
  private string[] _strings;
  private double[] _numbersFrom;
  private double[] _numbersTo;
  private double[] _numbersAccumulator;
  public static readonly DependencyProperty FromProperty = DependencyProperty.Register(nameof (From), typeof (Geometry), typeof (GeometryAnimation), new PropertyMetadata((object) null, new PropertyChangedCallback(GeometryAnimation.OnFromChanged)));
  public static readonly DependencyProperty ToProperty = DependencyProperty.Register(nameof (To), typeof (Geometry), typeof (GeometryAnimation), new PropertyMetadata((object) null, new PropertyChangedCallback(GeometryAnimation.OnToChanged)));
  public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(nameof (EasingFunction), typeof (IEasingFunction), typeof (GeometryAnimation), new PropertyMetadata((object) null));

  public GeometryAnimation()
  {
  }

  public GeometryAnimation(string fromValue, string toValue)
    : this()
  {
    this.From = Geometry.Parse(fromValue);
    this.To = Geometry.Parse(toValue);
  }

  private void UpdateValue()
  {
    if (this._numbersFrom == null || this._numbersTo == null || this._numbersFrom.Length != this._numbersTo.Length)
      return;
    this._numbersAccumulator = new double[this._numbersFrom.Length];
    for (int index = 0; index < this._numbersFrom.Length; ++index)
      this._numbersAccumulator[index] = this._numbersTo[index] - this._numbersFrom[index];
  }

  public GeometryAnimation(Geometry fromValue, Geometry toValue)
    : this()
  {
    this.From = fromValue;
    this.To = toValue;
  }

  public GeometryAnimation(Geometry fromValue, Geometry toValue, Duration duration)
    : this()
  {
    this.From = fromValue;
    this.To = toValue;
    this.Duration = duration;
  }

  public GeometryAnimation(
    Geometry fromValue,
    Geometry toValue,
    Duration duration,
    FillBehavior fillBehavior)
    : this()
  {
    this.From = fromValue;
    this.To = toValue;
    this.Duration = duration;
    this.FillBehavior = fillBehavior;
  }

  public GeometryAnimation Clone() => (GeometryAnimation) base.Clone();

  protected override Freezable CreateInstanceCore() => (Freezable) new GeometryAnimation();

  protected override Geometry GetCurrentValueCore(
    Geometry defaultOriginValue,
    Geometry defaultDestinationValue,
    AnimationClock animationClock)
  {
    if (this._numbersAccumulator == null)
    {
      if (this._numbersFrom == null)
        AnimationHelper.DecomposeGeometryStr(defaultOriginValue.ToString((IFormatProvider) CultureInfo.InvariantCulture), out this._numbersFrom);
      if (this._numbersTo == null)
      {
        string str = defaultDestinationValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        AnimationHelper.DecomposeGeometryStr(str, out this._numbersTo);
        this._strings = Regex.Split(str, "[+-]?\\d*\\.?\\d+(?:\\.\\d+)?(?:[eE][+-]?\\d+)?");
      }
      this.UpdateValue();
    }
    if (this._numbersAccumulator == null)
      return defaultOriginValue;
    double normalizedTime = animationClock.CurrentProgress.Value;
    IEasingFunction easingFunction = this.EasingFunction;
    if (easingFunction != null)
      normalizedTime = easingFunction.Ease(normalizedTime);
    double[] numArray = new double[this._numbersAccumulator.Length];
    if (this.IsCumulative)
    {
      int? currentIteration = animationClock.CurrentIteration;
      double num = (double) (currentIteration.HasValue ? new int?(currentIteration.GetValueOrDefault() - 1) : new int?()).Value;
      if (num > 0.0)
      {
        numArray = new double[this._numbersAccumulator.Length];
        for (int index = 0; index < this._numbersAccumulator.Length; ++index)
          numArray[index] = this._numbersAccumulator[index] * num;
      }
    }
    double[] arr = new double[this._numbersAccumulator.Length];
    for (int index = 0; index < this._numbersAccumulator.Length; ++index)
      arr[index] = numArray[index] + this._numbersFrom[index] + this._numbersAccumulator[index] * normalizedTime;
    return AnimationHelper.ComposeGeometry(this._strings, arr);
  }

  private static void OnFromChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    GeometryAnimation geometryAnimation = (GeometryAnimation) d;
    if (!(e.NewValue is Geometry newValue))
      return;
    AnimationHelper.DecomposeGeometryStr(newValue.ToString((IFormatProvider) CultureInfo.InvariantCulture), out geometryAnimation._numbersFrom);
    geometryAnimation.UpdateValue();
  }

  public Geometry From
  {
    get => (Geometry) this.GetValue(GeometryAnimation.FromProperty);
    set => this.SetValue(GeometryAnimation.FromProperty, (object) value);
  }

  private static void OnToChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    GeometryAnimation geometryAnimation = (GeometryAnimation) d;
    if (!(e.NewValue is Geometry newValue))
      return;
    string str = newValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    AnimationHelper.DecomposeGeometryStr(str, out geometryAnimation._numbersTo);
    geometryAnimation._strings = Regex.Split(str, "[+-]?\\d*\\.?\\d+(?:\\.\\d+)?(?:[eE][+-]?\\d+)?");
    geometryAnimation.UpdateValue();
  }

  public Geometry To
  {
    get => (Geometry) this.GetValue(GeometryAnimation.ToProperty);
    set => this.SetValue(GeometryAnimation.ToProperty, (object) value);
  }

  public IEasingFunction EasingFunction
  {
    get => (IEasingFunction) this.GetValue(GeometryAnimation.EasingFunctionProperty);
    set => this.SetValue(GeometryAnimation.EasingFunctionProperty, (object) value);
  }

  public bool IsCumulative
  {
    get => (bool) this.GetValue(AnimationTimeline.IsCumulativeProperty);
    set => this.SetValue(AnimationTimeline.IsCumulativeProperty, ValueBoxes.BooleanBox(value));
  }
}
