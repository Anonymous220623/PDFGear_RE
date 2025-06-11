// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ScreenHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

#nullable disable
namespace HandyControl.Tools;

internal class ScreenHelper
{
  internal static void FindMaximumSingleMonitorRectangle(
    Rect windowRect,
    out Rect screenSubRect,
    out Rect monitorRect)
  {
    InteropValues.RECT screenSubRect1;
    InteropValues.RECT monitorRect1;
    ScreenHelper.FindMaximumSingleMonitorRectangle(new InteropValues.RECT(windowRect), out screenSubRect1, out monitorRect1);
    screenSubRect = new Rect(screenSubRect1.Position, screenSubRect1.Size);
    monitorRect = new Rect(monitorRect1.Position, monitorRect1.Size);
  }

  private static void FindMaximumSingleMonitorRectangle(
    InteropValues.RECT windowRect,
    out InteropValues.RECT screenSubRect,
    out InteropValues.RECT monitorRect)
  {
    List<InteropValues.RECT> rects = new List<InteropValues.RECT>();
    InteropMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (InteropValues.EnumMonitorsDelegate) ((IntPtr hMonitor, IntPtr hdcMonitor, ref InteropValues.RECT rect, IntPtr lpData) =>
    {
      InteropValues.MONITORINFO monitorInfo = new InteropValues.MONITORINFO();
      monitorInfo.cbSize = (uint) Marshal.SizeOf(typeof (InteropValues.MONITORINFO));
      InteropMethods.GetMonitorInfo(hMonitor, ref monitorInfo);
      rects.Add(monitorInfo.rcWork);
      return true;
    }), IntPtr.Zero);
    long num1 = 0;
    screenSubRect = new InteropValues.RECT()
    {
      Left = 0,
      Right = 0,
      Top = 0,
      Bottom = 0
    };
    monitorRect = new InteropValues.RECT()
    {
      Left = 0,
      Right = 0,
      Top = 0,
      Bottom = 0
    };
    foreach (InteropValues.RECT rect in rects)
    {
      InteropValues.RECT lprcSrc1 = rect;
      InteropValues.RECT lprcDst;
      InteropMethods.IntersectRect(out lprcDst, ref lprcSrc1, ref windowRect);
      long num2 = (long) (lprcDst.Width * lprcDst.Height);
      if (num2 > num1)
      {
        screenSubRect = lprcDst;
        monitorRect = rect;
        num1 = num2;
      }
    }
  }

  internal static void FindMonitorRectsFromPoint(
    Point point,
    out Rect monitorRect,
    out Rect workAreaRect)
  {
    IntPtr hMonitor = InteropMethods.MonitorFromPoint(new InteropValues.POINT()
    {
      X = (int) point.X,
      Y = (int) point.Y
    }, 2);
    monitorRect = new Rect(0.0, 0.0, 0.0, 0.0);
    workAreaRect = new Rect(0.0, 0.0, 0.0, 0.0);
    if (!(hMonitor != IntPtr.Zero))
      return;
    InteropValues.MONITORINFO monitorInfo = new InteropValues.MONITORINFO();
    monitorInfo.cbSize = (uint) Marshal.SizeOf(typeof (InteropValues.MONITORINFO));
    InteropMethods.GetMonitorInfo(hMonitor, ref monitorInfo);
    monitorRect = new Rect(monitorInfo.rcMonitor.Position, monitorInfo.rcMonitor.Size);
    workAreaRect = new Rect(monitorInfo.rcWork.Position, monitorInfo.rcWork.Size);
  }
}
