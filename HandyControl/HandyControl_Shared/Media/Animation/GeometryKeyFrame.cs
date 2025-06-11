// Decompiled with JetBrains decompiler
// Type: GeometryKeyFrame
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
public abstract class GeometryKeyFrame : Freezable, IKeyFrame
{
  internal double[] Numbers;
  public static readonly DependencyProperty KeyTimeProperty = DependencyProperty.Register(nameof (KeyTime), typeof (KeyTime), typeof (GeometryKeyFrame), new PropertyMetadata((object) KeyTime.Uniform));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (Geometry), typeof (GeometryKeyFrame), new PropertyMetadata((object) null, new PropertyChangedCallback(GeometryKeyFrame.OnValueChanged)));

  protected GeometryKeyFrame()
  {
  }

  protected GeometryKeyFrame(Geometry value)
  {
    double[] arr;
    AnimationHelper.DecomposeGeometryStr(value.ToString((IFormatProvider) CultureInfo.InvariantCulture), out arr);
    this.Numbers = arr;
    this.Value = value;
  }

  protected GeometryKeyFrame(Geometry value, KeyTime keyTime)
    : this(value)
  {
    this.KeyTime = keyTime;
  }

  public KeyTime KeyTime
  {
    get => (KeyTime) this.GetValue(GeometryKeyFrame.KeyTimeProperty);
    set => this.SetValue(GeometryKeyFrame.KeyTimeProperty, (object) value);
  }

  object IKeyFrame.Value
  {
    get => (object) this.Value;
    set => this.Value = (Geometry) value;
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    GeometryKeyFrame geometryKeyFrame = (GeometryKeyFrame) d;
    double[] arr;
    AnimationHelper.DecomposeGeometryStr(((Geometry) e.NewValue).ToString((IFormatProvider) CultureInfo.InvariantCulture), out arr);
    double[] numArray = arr;
    geometryKeyFrame.Numbers = numArray;
  }

  public Geometry Value
  {
    get => (Geometry) this.GetValue(GeometryKeyFrame.ValueProperty);
    set => this.SetValue(GeometryKeyFrame.ValueProperty, (object) value);
  }

  public double[] InterpolateValue(double[] baseValue, double keyFrameProgress)
  {
    return keyFrameProgress >= 0.0 && keyFrameProgress <= 1.0 ? this.InterpolateValueCore(baseValue, keyFrameProgress) : throw new ArgumentOutOfRangeException(nameof (keyFrameProgress));
  }

  protected abstract double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress);
}
