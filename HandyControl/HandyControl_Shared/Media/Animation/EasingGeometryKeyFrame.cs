// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.EasingGeometryKeyFrame
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Media.Animation;

public class EasingGeometryKeyFrame : GeometryKeyFrame
{
  public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(nameof (EasingFunction), typeof (IEasingFunction), typeof (EasingGeometryKeyFrame), new PropertyMetadata((object) null));

  public EasingGeometryKeyFrame()
  {
  }

  public EasingGeometryKeyFrame(Geometry value)
    : base(value)
  {
  }

  public EasingGeometryKeyFrame(Geometry value, KeyTime keyTime)
    : base(value, keyTime)
  {
  }

  public EasingGeometryKeyFrame(Geometry value, KeyTime keyTime, IEasingFunction easingFunction)
    : base(value, keyTime)
  {
    this.EasingFunction = easingFunction;
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new EasingGeometryKeyFrame();

  protected override double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress)
  {
    IEasingFunction easingFunction = this.EasingFunction;
    if (easingFunction != null)
      keyFrameProgress = easingFunction.Ease(keyFrameProgress);
    if (MathHelper.IsVerySmall(keyFrameProgress))
      return baseValue;
    return MathHelper.AreClose(keyFrameProgress, 1.0) ? this.Numbers : AnimationHelper.InterpolateGeometryValue(baseValue, this.Numbers, keyFrameProgress);
  }

  public IEasingFunction EasingFunction
  {
    get => (IEasingFunction) this.GetValue(EasingGeometryKeyFrame.EasingFunctionProperty);
    set => this.SetValue(EasingGeometryKeyFrame.EasingFunctionProperty, (object) value);
  }
}
