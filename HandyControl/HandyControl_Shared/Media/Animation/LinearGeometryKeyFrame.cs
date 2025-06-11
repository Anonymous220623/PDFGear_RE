// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.LinearGeometryKeyFrame
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

public class LinearGeometryKeyFrame : GeometryKeyFrame
{
  public LinearGeometryKeyFrame()
  {
  }

  public LinearGeometryKeyFrame(Geometry value)
    : base(value)
  {
  }

  public LinearGeometryKeyFrame(Geometry value, KeyTime keyTime)
    : base(value, keyTime)
  {
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new LinearGeometryKeyFrame();

  protected override double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress)
  {
    if (MathHelper.IsVerySmall(keyFrameProgress))
      return baseValue;
    return MathHelper.AreClose(keyFrameProgress, 1.0) ? this.Numbers : AnimationHelper.InterpolateGeometryValue(baseValue, this.Numbers, keyFrameProgress);
  }
}
