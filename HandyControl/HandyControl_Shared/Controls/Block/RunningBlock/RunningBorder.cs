// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RunningBorder
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class RunningBorder : Border
{
  private bool _test;

  protected override Size MeasureOverride(Size constraint)
  {
    if (this._test)
    {
      this._test = false;
      return constraint;
    }
    UIElement child = this.Child;
    Thickness th = this.BorderThickness;
    Thickness padding = this.Padding;
    if (this.UseLayoutRounding)
    {
      double deviceDpiX = DpiHelper.DeviceDpiX;
      double deviceDpiY = DpiHelper.DeviceDpiY;
      th = new Thickness(DpiHelper.RoundLayoutValue(th.Left, deviceDpiX), DpiHelper.RoundLayoutValue(th.Top, deviceDpiY), DpiHelper.RoundLayoutValue(th.Right, deviceDpiX), DpiHelper.RoundLayoutValue(th.Bottom, deviceDpiY));
    }
    Size size1 = RunningBorder.ConvertThickness2Size(th);
    Size size2 = RunningBorder.ConvertThickness2Size(padding);
    Size size3 = new Size();
    if (child != null)
    {
      Size size4 = new Size(size1.Width + size2.Width, size1.Height + size2.Height);
      Size availableSize1 = new Size(Math.Max(0.0, constraint.Width - size4.Width), Math.Max(0.0, constraint.Height - size4.Height));
      Size availableSize2 = new Size(Math.Max(0.0, double.PositiveInfinity - size4.Width), Math.Max(0.0, double.PositiveInfinity - size4.Height));
      child.Measure(availableSize1);
      Size desiredSize = child.DesiredSize;
      size3.Width = desiredSize.Width + size4.Width;
      size3.Height = desiredSize.Height + size4.Height;
      child.Measure(availableSize2);
    }
    else
      size3 = new Size(size1.Width + size2.Width, size1.Height + size2.Height);
    return size3;
  }

  private static Size ConvertThickness2Size(Thickness th)
  {
    return new Size(th.Left + th.Right, th.Top + th.Bottom);
  }
}
