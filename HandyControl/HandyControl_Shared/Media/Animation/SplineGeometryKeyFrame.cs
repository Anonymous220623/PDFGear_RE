// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.SplineGeometryKeyFrame
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Media.Animation;

public class SplineGeometryKeyFrame : GeometryKeyFrame
{
  public static readonly DependencyProperty KeySplineProperty = DependencyProperty.Register(nameof (KeySpline), typeof (KeySpline), typeof (SplineGeometryKeyFrame), new PropertyMetadata((object) new KeySpline()));

  public SplineGeometryKeyFrame()
  {
  }

  public SplineGeometryKeyFrame(Geometry value)
    : base(value)
  {
  }

  public SplineGeometryKeyFrame(Geometry value, KeyTime keyTime)
    : base(value, keyTime)
  {
  }

  public SplineGeometryKeyFrame(Geometry value, KeyTime keyTime, KeySpline keySpline)
    : base(value, keyTime)
  {
    this.KeySpline = keySpline ?? throw new ArgumentNullException(nameof (keySpline));
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new SplineGeometryKeyFrame();

  protected override double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress)
  {
    if (MathHelper.IsVerySmall(keyFrameProgress))
      return baseValue;
    if (MathHelper.AreClose(keyFrameProgress, 1.0))
      return this.Numbers;
    double splineProgress = this.KeySpline.GetSplineProgress(keyFrameProgress);
    return AnimationHelper.InterpolateGeometryValue(baseValue, this.Numbers, splineProgress);
  }

  public KeySpline KeySpline
  {
    get => (KeySpline) this.GetValue(SplineGeometryKeyFrame.KeySplineProperty);
    set => this.SetValue(SplineGeometryKeyFrame.KeySplineProperty, (object) value);
  }
}
