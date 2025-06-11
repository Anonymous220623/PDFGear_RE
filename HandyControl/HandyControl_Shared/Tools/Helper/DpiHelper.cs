// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.DpiHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using HandyControl.Tools.Interop;
using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools;

internal static class DpiHelper
{
  private const double LogicalDpi = 96.0;
  [ThreadStatic]
  private static Matrix _transformToDip;

  static DpiHelper()
  {
    IntPtr dc = InteropMethods.GetDC(IntPtr.Zero);
    if (dc != IntPtr.Zero)
    {
      DpiHelper.DeviceDpiX = (double) InteropMethods.GetDeviceCaps(dc, 88);
      DpiHelper.DeviceDpiY = (double) InteropMethods.GetDeviceCaps(dc, 90);
      InteropMethods.ReleaseDC(IntPtr.Zero, dc);
    }
    else
    {
      DpiHelper.DeviceDpiX = 96.0;
      DpiHelper.DeviceDpiY = 96.0;
    }
    Matrix identity1 = Matrix.Identity;
    Matrix identity2 = Matrix.Identity;
    identity1.Scale(DpiHelper.DeviceDpiX / 96.0, DpiHelper.DeviceDpiY / 96.0);
    identity2.Scale(96.0 / DpiHelper.DeviceDpiX, 96.0 / DpiHelper.DeviceDpiY);
    DpiHelper.TransformFromDevice = new MatrixTransform(identity2);
    DpiHelper.TransformFromDevice.Freeze();
    DpiHelper.TransformToDevice = new MatrixTransform(identity1);
    DpiHelper.TransformToDevice.Freeze();
  }

  public static MatrixTransform TransformFromDevice { get; }

  public static MatrixTransform TransformToDevice { get; }

  public static double DeviceDpiX { get; }

  public static double DeviceDpiY { get; }

  public static double LogicalToDeviceUnitsScalingFactorX => DpiHelper.TransformToDevice.Matrix.M11;

  public static double LogicalToDeviceUnitsScalingFactorY => DpiHelper.TransformToDevice.Matrix.M22;

  public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
  {
    DpiHelper._transformToDip = Matrix.Identity;
    DpiHelper._transformToDip.Scale(1.0 / dpiScaleX, 1.0 / dpiScaleY);
    return DpiHelper._transformToDip.Transform(devicePoint);
  }

  public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
  {
    Point logical = DpiHelper.DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);
    return new Size(logical.X, logical.Y);
  }

  public static Rect LogicalToDeviceUnits(this Rect logicalRect)
  {
    Rect deviceUnits = logicalRect;
    deviceUnits.Transform(DpiHelper.TransformToDevice.Matrix);
    return deviceUnits;
  }

  public static Rect DeviceToLogicalUnits(this Rect deviceRect)
  {
    Rect logicalUnits = deviceRect;
    logicalUnits.Transform(DpiHelper.TransformFromDevice.Matrix);
    return logicalUnits;
  }

  public static double RoundLayoutValue(double value, double dpiScale)
  {
    double d;
    if (!MathHelper.AreClose(dpiScale, 1.0))
    {
      d = Math.Round(value * dpiScale) / dpiScale;
      if (double.IsNaN(d) || double.IsInfinity(d) || MathHelper.AreClose(d, double.MaxValue))
        d = value;
    }
    else
      d = Math.Round(value);
    return d;
  }
}
