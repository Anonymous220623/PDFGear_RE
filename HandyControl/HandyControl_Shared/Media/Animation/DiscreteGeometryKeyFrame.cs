// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.DiscreteGeometryKeyFrame
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Media.Animation;

public class DiscreteGeometryKeyFrame : GeometryKeyFrame
{
  public DiscreteGeometryKeyFrame()
  {
  }

  public DiscreteGeometryKeyFrame(Geometry value)
    : base(value)
  {
  }

  public DiscreteGeometryKeyFrame(Geometry value, KeyTime keyTime)
    : base(value, keyTime)
  {
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new DiscreteGeometryKeyFrame();

  protected override double[] InterpolateValueCore(double[] baseValue, double keyFrameProgress)
  {
    return keyFrameProgress >= 1.0 ? this.Numbers : baseValue;
  }
}
